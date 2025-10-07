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
using Business;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class NuevoLote : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if (!Page.IsPostBack)
                {
                
                    if (Request["Tipo"] == "Alta")
                    {
                        lblLote.Text = "Se genero el lote número " + Request["Lote"].ToString();
                        lblTitulo.Text = "Nuevo Lote generado";
                        HyperLink1.NavigateUrl = "~/Derivaciones/Derivados2.aspx?tipo=informe";
                    }
                    else
                    {
                        lblLote.Text = "Se modifico el lote número " + Request["Lote"].ToString();
                        lblTitulo.Text = "Lote Modificado";
                        HyperLink1.NavigateUrl = "~/Derivaciones/GestionarLote.aspx";
                    }

                }

            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }


        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
                MostrarInforme();
            else
                Response.Redirect("../FinSesion.aspx", false);
        }


        private void MostrarInforme()
        {
            Usuario oUser = new Usuario();
            DataTable dt = GetDataSet();
            string informe = "../Informes/DerivacionLote.rpt";
            Configuracion oCon = new Configuracion();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);


            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;


            oCr.Report.FileName = informe;
            oCr.ReportDocument.SetDataSource(dt);
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();

            Utility oUtil = new Utility();
            string nombrePDF = oUtil.CompletarNombrePDF("Derivaciones"+ Request["Lote"]);
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
        }

        public DataTable GetDataSet()
        {

            string m_strSQL = Business.Data.Laboratorio.LoteDerivacion.derivacionPDF(int.Parse(Request["Lote"]));

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;//LAB-130 usar conexion principal no la de consulta
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];

        }

        

    }
}
