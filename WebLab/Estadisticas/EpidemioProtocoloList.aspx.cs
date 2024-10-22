using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebLab.Estadisticas
{
    public partial class EpidemioProtocoloList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                string a = Server.MapPath("") + "\\epi.xml";
                DataSet ds = new DataSet();//Se crea un dataset
                ds.ReadXml(a);

                GridView1.DataSource = ds;
                GridView1.DataBind();

                 
            }
        }

    
        public string Generateurl(string v)
        {
            return "EpidemioProtococolo.aspx?protocolo=" + v;
          


        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ExportarExcel();

        }


        private void ExportarExcel()
        {
            string a = Server.MapPath("") + "\\epi.xml";
            DataSet ds = new DataSet();//Se crea un dataset
            ds.ReadXml(a);


            DataTable tabla = ds.Tables[0];
            if (tabla.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Page pagina = new Page();
                HtmlForm form = new HtmlForm();
                GridView dg = new GridView();
                dg.EnableViewState = false;
                dg.DataSource = tabla;
               

                dg.DataBind();
                pagina.EnableEventValidation = false;
                pagina.DesignerInitialize();
                pagina.Controls.Add(form);
                form.Controls.Add(dg);
                pagina.RenderControl(htw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=EpiCovid_" + DateTime.Now.ToShortDateString() + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}