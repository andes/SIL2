using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.website
{
    public partial class DirectorioAnalisis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                CargarGrilla();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            GridView1.SetPageIndex(0);
        }

     

        private void CargarGrilla()
        {
            using (DataSet dataset = new DataSet())
            {
                dataset.ReadXml(Server.MapPath("determinaciones.xml"));

                string filtro = "";

                DataTable dt = dataset.Tables[0];
                if (txtNombre.Text.Trim() != "")                    
                        filtro = "title like '%" + txtNombre.Text.Trim() + "%'";
                switch (ddlArea.SelectedValue)
                {
                    case "Microbiología molecular": if (filtro=="") filtro = "tipo like '%Microbiología%'";
                    else
                        filtro = filtro + " and tipo like '%Microbiología%'"; break;
                    case "Histocompatibilidad": if (filtro == "") filtro = "tipo like '%Histocompatibilidad%'";
                        else
                            filtro = filtro + " and tipo like '%Histocompatibilidad%'"; break;
                    case "Genética molecular": if (filtro == "") filtro = "tipo like '%Genética molecular%'";
                        else
                            filtro = filtro + " and tipo like '%Genética molecular%'"; break;
                }

                dt.DefaultView.RowFilter = filtro;
                dt.DefaultView.Sort = "title asc";

                GridView1.DataSource = dt;
                GridView1.DataBind();
                
            }
        }

        public string GenerateContent(string v)
        {
            if (v == "")
                return "";
            else
            { 
                string[] arr = v.Split(("|").ToCharArray());
           
                    
                    return arr[0].Trim().ToString();
            }

        }

        public string Generateurl(string v)
        {
            if (v == "")
                return "";
            else
            {
                string[] arr = v.Split(("|").ToCharArray());


                return  arr[1].Trim().ToString();
            }


        }
        public bool Generatevisible(string v)
        {
            if (v == "")
                return false;
            else
                return true;


        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;

            int currentPage = GridView1.PageIndex + 1;
            //CurrentPageLabel.Text = "Página " + currentPage.ToString() + " de " + GridView1.PageCount.ToString();
            CargarGrilla();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //HyperLink CmdAnular = (HyperLink)e.Row.Cells[0].FindControl("hplFicha");
            //if (CmdAnular != null)
            //    if (CmdAnular.Text != "") CmdAnular.Text = "algitototttt";


        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
            GridView1.SetPageIndex(0);
        }
    }
}
