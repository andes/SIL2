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
using System.Data.SqlClient;
using Business;
using System.Text;
using System.IO;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.Drawing;

namespace WebLab.Estadisticas
{
    public partial class GeneralReporte : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            //oCr.ReportDocument.PrintOptions.PrinterName = oC.NombreImpresora;// System.Configuration.ConfigurationSettings.AppSettings["Impresora"]; 
        }

       
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (!Page.IsPostBack)
            {*/
                if (Request["informe"].ToString() == "General")
                    MostrarInformeGeneral("Pantalla");
                if (Request["informe"].ToString() == "PorResultado")
                    MostrarInformePorResultado("Pantalla");
            //}
              //  CargarGrilla();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private DataTable GetDatosPorResultado()
        {

            ///Llena la tabla con los datos
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            // Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            cmd.CommandText = "LAB_EstadisticaPorResultados";

            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(Request["fechaDesde"].ToString());
            DateTime fecha2 = DateTime.Parse(Request["fechaHasta"].ToString());

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            ///


            /////Parametro lista de analisis
            cmd.Parameters.Add("@idAnalisis", SqlDbType.NVarChar);
            cmd.Parameters["@idAnalisis"].Value = Request["idAnalisis"].ToString();

            ////Parametro tipo de informe
            //string tipoinforme = "0";
            //if (rdbTipoInforme.Items[0].Selected) tipoinforme = "0";
            //if (rdbTipoInforme.Items[1].Selected) tipoinforme = "1";
            cmd.Parameters.Add("@tipoInforme", SqlDbType.NVarChar);
            cmd.Parameters["@tipoInforme"].Value = Request["tipo"].ToString();
            /////


            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            return Ds.Tables[0];


        }

        

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
          
        }



        protected void lnkPdf_Click(object sender, EventArgs e)
        {
            if (Request["informe"].ToString() == "General")
            MostrarInformeGeneral("PDF");
            else
                MostrarInformePorResultado("PDF");
        }


        private void MostrarInformePorResultado(string tipo)
        {


            DataTable dt = new DataTable();
            dt = GetDatosPorResultado();
            if (dt.Rows.Count > 0)
            {

                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;


                ParameterDiscreteValue rangoFechas = new ParameterDiscreteValue();
                rangoFechas.Value = Request["fechaDesde"].ToString() + "  - " + Request["fechaHasta"].ToString();

;


                if (Request["tipo"].ToString()=="0") oCr.Report.FileName = "PorResultadoPredefinido.rpt";
                if (Request["tipo"].ToString() == "1") oCr.Report.FileName = "PorResultadoNumerico.rpt";

                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(rangoFechas);
                oCr.DataBind();


                switch (tipo)
                {
                    case "Pantalla":
                        {
                            CrystalReportViewer1.ReportSourceID = oCr.ID;
                            CrystalReportViewer1.ReportSource = oCr;
                            Condiciones_del_visualizador();
                        }
                        break;
                    case "Imprimir":  oCr.ReportDocument.PrintToPrinter(1, false, 0,0); break;
                    case "PDF":
                        {
                            Utility oUtil = new Utility();
                            string nombrePDF = oUtil.CompletarNombrePDF("Estadistica");
                            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                           
                        }
                        break;
                    case "Excel":
                        {
                            MemoryStream oStream; // using System.IO
                            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "attachment;filename=Estadistica.xls");

                            Response.BinaryWrite(oStream.ToArray());
                            Response.End();
                        }
                        break;


                }
             
            }
            else
                Response.Redirect("SinDatos.aspx", false);

        }
        private void MostrarInformeGeneral(string tipo)
        {

            DataTable dt = new DataTable();
            dt = GetDatos();
            if (dt.Rows.Count > 0)
            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;

                ParameterDiscreteValue encabezado4 = new ParameterDiscreteValue();

                oCr.Report.FileName = "General.rpt";
                switch (Request["tipo"].ToString())
                {
                    case "0": encabezado4.Value = "REPORTE POR AREAS"; break;
                    case "1": encabezado4.Value = "REPORTE POR ANALISIS"; break;
                    case "2": encabezado4.Value = "REPORTE POR MEDICO SOLICITANTE"; break;
                    case "3": encabezado4.Value = "REPORTE POR EFECTOR SOLICITANTE"; break;
                    case "4": encabezado4.Value = "REPORTE DE DERIVACIONES"; break;
                    
                    case "5":
                        {
                            encabezado4.Value = "REPORTE RESUMIDO TOTALIZADO";
                            oCr.Report.FileName = "Resumido.rpt";
                        } break;
                    case "6": encabezado4.Value = "REPORTE DE DIAGNOSTICOS"; break;

                }
                ParameterDiscreteValue subTitulo = new ParameterDiscreteValue();
                subTitulo.Value = Request["fechaDesde"].ToString() + " - " + Request["fechaHasta"].ToString();

                ParameterDiscreteValue grafico = new ParameterDiscreteValue();
                if (Request["grafico"].ToString()=="1")                grafico.Value =false ;
                else grafico.Value = true;

                string aux = "Agrupado por Origen";
                if (Request["Agrupado"].ToString() == "1") aux = "Agrupado por Prioridad";

                subTitulo.Value = Request["fechaDesde"].ToString() + " - " + Request["fechaHasta"].ToString() + " - " + aux;



                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(encabezado4);
                oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(subTitulo);
                oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(grafico);
                oCr.DataBind();



                switch (tipo)
                {
                    case "Pantalla":
                        {
                            CrystalReportViewer1.ReportSourceID = oCr.ID;
                            CrystalReportViewer1.ReportSource = oCr;
                            Condiciones_del_visualizador();
                        }
                        break;
                    case "Imprimir":  oCr.ReportDocument.PrintToPrinter(1, false, 0,0); break;
                    case "PDF":
                        {
                            MemoryStream oStream; // using System.IO
                            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "attachment;filename=Estadistica.pdf");

                            Response.BinaryWrite(oStream.ToArray());
                            Response.End();
                        }
                        break;
                    case "Excel":
                        {
                            MemoryStream oStream; // using System.IO
                            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.Excel);
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("Content-Disposition", "attachment;filename=Estadistica.xls");

                            Response.BinaryWrite(oStream.ToArray());
                            Response.End();
                        }
                        break;
                }
            }
            else
                Response.Redirect("SinDatos.aspx", false);
        }

        private void Condiciones_del_visualizador()
        {
            this.CrystalReportViewer1.BestFitPage = false;
            this.CrystalReportViewer1.BorderColor = Color.FromArgb(0x78006599);
            this.CrystalReportViewer1.BorderStyle = BorderStyle.Solid;
            this.CrystalReportViewer1.BorderWidth = 1;

            this.CrystalReportViewer1.DisplayGroupTree = false;
            //this.CrystalReportViewer1.DisplayToolbar = false;
            this.CrystalReportViewer1.DisplayPage = true;

            this.CrystalReportViewer1.HasDrillUpButton = false;
            this.CrystalReportViewer1.HasGotoPageButton = true;
            this.CrystalReportViewer1.HasPageNavigationButtons = true;
            this.CrystalReportViewer1.HasRefreshButton = false;
            this.CrystalReportViewer1.HasSearchButton = false;
            this.CrystalReportViewer1.HasZoomFactorList = true;
            this.CrystalReportViewer1.HasExportButton = false;
            this.CrystalReportViewer1.HasToggleGroupTreeButton = false;
            this.CrystalReportViewer1.HasCrystalLogo = false;
            this.CrystalReportViewer1.HasPrintButton = false;


            this.CrystalReportViewer1.SeparatePages = true;
            this.CrystalReportViewer1.ShowFirstPage();
            //this.CrystalReportViewer1.PageZoomFactor = 100;            
            this.CrystalReportViewer1.Height = 460;
            this.CrystalReportViewer1.Width = 780;
        }

        private DataTable GetDatos()
        {
           
         
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            // Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);


            switch (Request["tipo"].ToString())
            {
                case "0": cmd.CommandText = "LAB_EstadisticaPorArea"; break;
                case "1": cmd.CommandText = "LAB_EstadisticaPorAnalisis"; break;
                case "2": cmd.CommandText = "LAB_EstadisticaPorMedico"; break;
                case "3": cmd.CommandText = "LAB_EstadisticaPorEfector"; break;
                case "4": cmd.CommandText = "LAB_EstadisticaPorDerivaciones"; break;
                case "5": cmd.CommandText = "LAB_EstadisticaTotalizado"; break;
                case "6": cmd.CommandText = "LAB_EstadisticaPorDiagnostico"; break;
                
            }



            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(Request["fechaDesde"].ToString());
            DateTime fecha2 = DateTime.Parse(Request["fechaHasta"].ToString());

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            /////

            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@agrupado", SqlDbType.NVarChar);
            cmd.Parameters["@agrupado"].Value = Request["Agrupado"].ToString();
            ///
            ///Parametro Servicio
            cmd.Parameters.Add("@idTipoServicio", SqlDbType.NVarChar);
            cmd.Parameters["@idTipoServicio"].Value = Request["idTipoServicio"].ToString();
            ///

           
            ///
            ///Parametro Area
            cmd.Parameters.Add("@idArea", SqlDbType.NVarChar);
            cmd.Parameters["@idArea"].Value =  Request["idArea"].ToString();
            ///
            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            return Ds.Tables[0];
        }

        protected void lnkExcel_Click1(object sender, EventArgs e)
        {
            if (Request["informe"].ToString() == "General")
                MostrarInformeGeneral("Excel");
            else
                MostrarInformePorResultado("Excel");
            
        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            if (Request["informe"].ToString() == "General")
                MostrarInformeGeneral("Imprimir");
            else
                MostrarInformePorResultado("Imprimir");
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            if (Request["informe"].ToString() == "General")
                Response.Redirect("GeneralFiltro.aspx", false);
            else
                Response.Redirect("PorResultado.aspx", false);

        }
    }
}
