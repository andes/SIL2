using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using System.IO;
using System.Data.SqlClient;
using Business;

namespace SIPS.Laboratorio.Neonatal
{
    public partial class IngresoMensaje : System.Web.UI.Page
    {
        CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Pesquisa Neonatal");
                if (Session["idUsuario"] == null)
                    Response.Redirect("../FinSesion.aspx", false);
                else
                {
                    lblNumeroSolicitud.Text = Request["id"].ToString();
                    CargarGrilla();

                }
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                      Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    //case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        private void CargarGrilla()
        {
            
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }



        private object LeerDatos()
        {
            string m_strSQL = @" SELECT   descripcion as [Alarmas] FROM LAB_AlarmaScreening WHERE idSolicitudScreening =" + Request["id"].ToString(); ;


            DataSet Ds = new DataSet(); SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SicConnectionString"].ToString());
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }
        private void Imprimir()
        {
            string[] arr = Request["id"].ToString().Split((".").ToCharArray());

            ///Se descompone el id compuesto= idProtocolo + idEfector y se pasan como parametros
            string idProtocolo = arr[0].ToString();
            string idEfector = arr[1].ToString();

            oCr.Report.FileName = "Resultado.rpt";
            oCr.ReportDocument.SetDataSource(GetDataResultados(idProtocolo, idEfector));
            oCr.DataBind();

            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=Resultado.pdf");

            Response.BinaryWrite(oStream.ToArray());
            Response.End();

        }

        private DataTable GetDataResultados(string p, string e)
        {
            string m_strSQL = " Select * from   LAB_ImprimeResultado" +
                              " WHERE  idProtocolo = " + p + "  and idEfector=" + e + " order by ordenarea, orden , orden1 ";

            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SicConnectionString"].ToString());
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            DataTable data = Ds.Tables[0];
            return data;
        }

        protected void lnkDescargar_Click(object sender, EventArgs e)
        {
            Imprimir();
        }
    }
}
