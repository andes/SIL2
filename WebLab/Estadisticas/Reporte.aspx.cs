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
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using InfoSoftGlobal;
using System.IO;
using CrystalDecisions.Web;
using System.Text;
using Business.Data;

namespace WebLab.Estadisticas
{
    public partial class Reporte : System.Web.UI.Page
    {

        public CrystalReportSource oCr = new CrystalReportSource();
        int suma1 = 0;
        int suma2 = 0;
        int suma3 = 0;
        int suma4 = 0;
        int suma5= 0;
        int suma6 = 0;

        int col1 = 0;
        Usuario oUser = new Usuario();

      


        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Session["idUsuario"] == null)
                Response.Redirect("logout.aspx", false);
            else
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }


        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("logout.aspx", false);
                else
                {
                    lblInforme.Text = Session["informe"].ToString();
                    lblTipo.Text = Session["tipo"].ToString();
                    if (Session["informe"].ToString() == "General")
                        MostrarReporteGeneral();
                    //if (Request["informe"].ToString() == "General")
                    //    MostrarReporteGeneral();
                    if (Session["informe"].ToString() == "PorResultado")
                        MostrarInformePorResultado("Pantalla");
                }
               
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private void MostrarReporteGeneral()
        {
            bool mostrarGrafico1 = true;
            bool mostrarGrafico2 = true;
           
        

            string s_titulo = "";
            string s_tituloChart = "";
            string s_tituloAdicional = "";
            switch (Session["tipo"].ToString())
            {
                case "0":
                    {
                        s_titulo = "REPORTE ESTADISTICO POR AREAS"; s_tituloChart = "Distribución de Análisis";
                        mostrarGrafico2 = false;                    
                    } break;
                case "1":
                    {
                        s_titulo = "REPORTE ESTADISTICO POR ANALISIS"; s_tituloChart = "Distribución de Análisis";
                    mostrarGrafico2 = false;
                    } break;
                        
                case "2":{ s_titulo = "REPORTE ESTADISTICO POR MEDICO SOLICITANTE"; s_tituloChart = "Distribución de Protocolos";
                mostrarGrafico2 = false;
                } break; 
                case "3": 
                    {s_titulo = "REPORTE ESTADISTICO POR EFECTOR SOLICITANTE"; s_tituloChart = "Distribución de Protocolos";
                    mostrarGrafico2 = false;
                    } break; 
                case "4": 
                    {s_titulo = "REPORTE ESTADISTICO DE DERIVACIONES"; s_tituloChart = "Distribución de Análisis Derivados";
                    mostrarGrafico2 = false;
                    } break;
                case "6":
                    {
                        s_titulo = "REPORTE ESTADISTICO POR DIAGNOSTICOS"; s_tituloChart = "Distribución de Protocolos"; mostrarGrafico2 = false;
                    } break; 
                case "5": 
                    {s_titulo = "REPORTE RESUMIDO TOTALIZADO";
                    mostrarGrafico1 = false;
                     mostrarGrafico2 = false;
                    } break; 
                case "7":
                    {
                        s_titulo = "PROTOCOLOS POR DIA";
                         mostrarGrafico1 = false; mostrarGrafico2 = false;
                    } break;
                case "8": 
                    {s_titulo = "REPORTE ESTADISTICO POR SECTOR/SERVICIO";
                    s_tituloChart = "Distribución de Protocolos";
                    mostrarGrafico2 = false;
                    } break; 
                case "9": s_titulo = "RANKING POR DIA DE LA SEMANA"; break;

                case "10":
                    {
                        imgPdf.Visible = false;  // no tiene salida a pdf
                           s_titulo = "PROTOCOLOS POR DIA - FRANJA HORARIA";
                        s_tituloAdicional= Session["horaDesde"].ToString() + " - " + Session["horaHasta"].ToString();
                        mostrarGrafico1 = false; mostrarGrafico2 = false;
                        gvEstadistica.ShowFooter = false;
                    }
                    break;
            }
         
            lblFiltro.Text= Session["fechaDesde"].ToString() + " - " + Session["fechaHasta"].ToString()+ " " + s_tituloAdicional;
            
            if (Session["grafico"].ToString() == "1") pnlGrafico.Visible = false;
            else                pnlGrafico.Visible = true;



            if (mostrarGrafico1)
            {              
                     FCLiteral.Text = CreateChart1(s_tituloChart, "Por Origen");              
            }

                /// Grafico 2
            if (mostrarGrafico2)
            {             
                FCLiteral0.Text = CreateChart2(s_tituloChart);            
            }


            if (Session["idEfector"].ToString() != "0")
            {
                Efector oEfector = new Efector();
                if (oEfector != null)
                {
                    oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Session["idEfector"].ToString()));
                    s_titulo += " - " + oEfector.Nombre;
                }
            }
            lblTitulo.Text = s_titulo;

            gvEstadistica.DataSource = GetDatosEstadistica("GV");            
            gvEstadistica.DataBind();

            if (Session["tipo"].ToString() == "5") ///mOSTRAR EL PROMEDIO DE ANALISIS POR PROTOCLO : FILA 1/FILA2
            {
                CalcularPromedio();    /// cantidad de analisis por protocolo          
            }
       
            if (gvEstadistica.Rows.Count == 0) Response.Redirect("SinDatos.aspx?Desde=GeneralFiltro.aspx", false);



        }

        private void CalcularPromedio()
        {
            int i = 0;
            int cantAna = 0;
            int cantPro = 0;
            int columnas = gvEstadistica.Columns.Count-1;
            int prom = 0;
            

            if (Session["Agrupado"].ToString() == "0") columnas = 4;
            if (Session["Agrupado"].ToString() == "1") columnas = 2;
          
            for (int j=1; j<columnas;j++)
            {
                i = 0; prom = 0;  cantAna = 0;cantPro = 0;
                foreach ( GridViewRow row in gvEstadistica.Rows)
                {
                    if (i == 0)
                    {
                        if (row.Cells[j].Text != "&nbsp;")
                        cantAna = int.Parse(row.Cells[j].Text); }
                    if (i == 1)
                    {
                        if (row.Cells[j].Text != "&nbsp;")
                        cantPro = int.Parse(row.Cells[j].Text); }
                    if (i == 2)
                    {
                        if ((cantAna > 0) && (cantPro > 0))
                            prom = cantAna / cantPro;
                        else
                            prom = 0;
                        row.Cells[0].Text = "Promedio Analisis/Protocolo";
                        row.Cells[j].Text = prom.ToString();
                    }

                    i += 1;
                }
                
            }

        }
        private DataTable GetDatosPorResultado()
        {

            ///Llena la tabla con los datos
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); //Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            // Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            cmd.CommandText = "LAB_EstadisticaPorResultados";

            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(Session["fechaDesde"].ToString());
            DateTime fecha2 = DateTime.Parse(Session["fechaHasta"].ToString());

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            ///


            /////Parametro lista de analisis
            cmd.Parameters.Add("@idAnalisis", SqlDbType.NVarChar);
            cmd.Parameters["@idAnalisis"].Value = Session["idAnalisis"].ToString();

            ////Parametro tipo de informe
            //string tipoinforme = "0";
            //if (rdbTipoInforme.Items[0].Selected) tipoinforme = "0";
            //if (rdbTipoInforme.Items[1].Selected) tipoinforme = "1";
            cmd.Parameters.Add("@tipoInforme", SqlDbType.NVarChar);
            cmd.Parameters["@tipoInforme"].Value = Session["tipo"].ToString();
            /////


            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            return Ds.Tables[0];


        }


        protected void lnkPdf_Click(object sender, EventArgs e)
        {
           
        }


        private void MostrarInformePorResultado(string tipo)
        {


            DataTable dt = new DataTable();
            dt = GetDatosPorResultado();
       

        }
        private void MostrarInformeGeneral(string tipo)
        {

            DataTable dt = new DataTable();
            dt = GetDatosEstadistica("S");
            if (dt.Rows.Count > 0)
            {
                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();                

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                if (Session["idEfector"].ToString() == "0")
                {



                    //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = "Subsecretaria de Salud";

                    //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = "Laboratorios";

                    //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = "";
                }
                else
                {

                    Efector oEfector = new Efector();

                    oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Session["idEfector"].ToString()));
                    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oEfector);

                    //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = oCon.EncabezadoLinea1;

                    //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = oCon.EncabezadoLinea2;

                    //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = oCon.EncabezadoLinea3;


                }

                ParameterDiscreteValue encabezado4 = new ParameterDiscreteValue();
                oCr.Report.FileName = "General.rpt";
                switch (Session["tipo"].ToString())
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
                    case "7": encabezado4.Value = "PROTOCOLOS POR DIA"; break;
                    case "8": encabezado4.Value = "REPORTE POR SECTOR/SERVICIO"; break;
                    case "9": encabezado4.Value = "RANKING POR DIA DE LA SEMANA"; break;
                    case "10": encabezado4.Value = "PROTOCOLOS POR FRANJA HORARIA"; break;

                }
                ParameterDiscreteValue subTitulo = new ParameterDiscreteValue();
                subTitulo.Value = Session["fechaDesde"].ToString() + " - " + Session["fechaHasta"].ToString();
              
                ParameterDiscreteValue grafico1 = new ParameterDiscreteValue();                
                ParameterDiscreteValue grafico2 = new ParameterDiscreteValue();
                


                //if (Request["grafico"].ToString() == "1") grafico.Value = false;
                //else grafico.Value = true;

                string aux = "Agrupado por Origen";
                if (Session["Agrupado"].ToString() == "1") aux = "Agrupado por Prioridad";

                subTitulo.Value = Session["fechaDesde"].ToString() + " - " + Session["fechaHasta"].ToString() + " - " + aux;
                if (Session["tipo"].ToString() == "10")
                {
                    subTitulo.Value += " " + Session["horaDesde"].ToString() + " - " + Session["horaHasta"].ToString();
                }
                grafico1.Value = false;
                grafico2.Value = false;

                if (tipo == "Excel")
                {
                    grafico1.Value = false; grafico2.Value=false;
                }
                else /// pdf /imprimir
                {
                    if (Session["tipo"].ToString() == "5")
                    { grafico1.Value = true; grafico2.Value = true; }
                    else
                    { grafico1.Value = false; grafico2.Value = true; }
                    if ((Session["tipo"].ToString() == "7")|| (Session["tipo"].ToString() == "9"))
                    {
                        grafico1.Value = false;
                        grafico2.Value = false;
                    }
                }

                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(encabezado4);
                oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(subTitulo);
                oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(grafico1);
                if (Session["tipo"].ToString()!="5")  oCr.ReportDocument.ParameterFields[6].CurrentValues.Add(grafico2);
                oCr.DataBind();



                switch (tipo)
                {
                   
                    case "PDF":
                        {
                            Utility oUtil = new Utility();
                            string nombrePDF = oUtil.CompletarNombrePDF("Estadistica");
                            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
 
                       
                        }
                        break;
                   
                }
            }
            else
                Response.Redirect("SinDatos.aspx?Desde=GeneralFiltro.aspx", false);
        }


        


        private DataTable GetDatosEstadistica(string s_tipo)
        {
        

            DataSet Ds = new DataSet();
            //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

           


            switch (lblTipo.Text)
            {
                case "0": cmd.CommandText = "LAB_EstadisticaPorArea"; break;
                case "1": cmd.CommandText = "LAB_EstadisticaPorAnalisis"; break;
                case "2": cmd.CommandText = "LAB_EstadisticaPorMedico"; break;
                case "3": cmd.CommandText = "LAB_EstadisticaPorEfector"; break;
                case "4": cmd.CommandText = "LAB_EstadisticaPorDerivaciones"; break;
                case "5": cmd.CommandText = "LAB_EstadisticaTotalizado"; break;
                case "6": cmd.CommandText = "LAB_EstadisticaPorDiagnostico"; break;
                case "7": cmd.CommandText = "LAB_EstadisticaDetalladaDia"; break;
                case "8": cmd.CommandText = "LAB_EstadisticaPorSector"; break;
                case "9": cmd.CommandText = "LAB_EstadisticaRankingDia"; break;
                case "10": cmd.CommandText = "LAB_EstadisticaPorHorario"; break;

            }



            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(Session["fechaDesde"].ToString());
            DateTime fecha2 = DateTime.Parse(Session["fechaHasta"].ToString());

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            /////

            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@agrupado", SqlDbType.NVarChar);
            cmd.Parameters["@agrupado"].Value = Session["Agrupado"].ToString();
            ///
            ///Parametro Servicio
            cmd.Parameters.Add("@idTipoServicio", SqlDbType.NVarChar);
            cmd.Parameters["@idTipoServicio"].Value = Session["idTipoServicio"].ToString();
            ///

                
            ///
            ///Parametro Area
            cmd.Parameters.Add("@idArea", SqlDbType.NVarChar);
            cmd.Parameters["@idArea"].Value = Session["idArea"].ToString();
            ///

            ///Parametro @idEfectorSol
            cmd.Parameters.Add("@idEfector", SqlDbType.NVarChar);
            cmd.Parameters["@idEfector"].Value = Session["idEfectorSolicitante"].ToString();
            ///

            ///Parametro @@idEspecialistaSol
            cmd.Parameters.Add("@idEspecialista", SqlDbType.NVarChar);
            cmd.Parameters["@idEspecialista"].Value = Session["idEspecialista"].ToString();
            ///

            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@imprimir", SqlDbType.NVarChar);
            cmd.Parameters["@imprimir"].Value = s_tipo;


            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@estado", SqlDbType.NVarChar);
            cmd.Parameters["@estado"].Value = Session["estado"].ToString();

            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@idEfectorProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idEfectorProtocolo"].Value = Session["idEfector"].ToString();



            if (lblTipo.Text == "10")
            {
                cmd.Parameters.Add("@horaDesde", SqlDbType.NVarChar);
                cmd.Parameters["@horaDesde"].Value = Session["horaDesde"].ToString();
                cmd.Parameters.Add("@horaHasta", SqlDbType.NVarChar);
                cmd.Parameters["@horaHasta"].Value = Session["horaHasta"].ToString();
                /////
            }
            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);


            

            da.Fill(Ds);

            
            
              

                if (lblTipo.Text == "5") Ds.Tables[0].Rows.Add();
            
            return Ds.Tables[0];
        }


        protected void lnkExcel_Click1(object sender, EventArgs e)
        {
           

        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
           
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            if (lblInforme.Text == "General")
                Response.Redirect("GeneralFiltro.aspx", false);
            else
                Response.Redirect("PorResultado.aspx", false);

        }
        private string CreateChart1(string s_titulo, string s_tipo)
        {

            DataTable dt = new DataTable();
            dt = GetDatosEstadistica("CH1");
            
       

            string strXML = "<graph caption='"+s_titulo+"' subCaption='"+ s_tipo+"' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";            

            if (dt.Rows.Count > 0)
            {
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    strXML += "<set name='" + dt.Rows[i][0].ToString() + "' value='" + dt.Rows[i][1].ToString() + "' />";                  
                }
            }
            else
                Response.Redirect("SinDatos.aspx?Desde=GeneralFiltro.aspx", false);


            strXML += "</graph>";
            
            return FusionCharts.RenderChart("../FusionCharts/FCF_Pie3D.swf", "", strXML, "Sales", "600", "250", false, false);


         

        }

        private string CreateChart2(string s_titulo)
        {

          

            DataTable dt = new DataTable();
            dt = GetDatosEstadistica("CH2");
            string strXML = "<graph caption='"+s_titulo+"'  showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strXML += "<set name='" + dt.Rows[i][0].ToString() + "' value='" + dt.Rows[i][1].ToString() + "' />";
                }
            }
            else
                Response.Redirect("SinDatos.aspx?Desde=GeneralFiltro.aspx", false);

            strXML += "</graph>";

            if (Session["tipo"].ToString() != "9")
            return FusionCharts.RenderChart("../FusionCharts/FCF_Pie3D.swf", "", strXML, "Sales1", "600", "250", false, true);
            else
                return FusionCharts.RenderChart("../FusionCharts/FCF_Line.swf", "", strXML, "Sales2", "600", "250", false, true);


        }

        protected void gvEstadistica_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            

            try
            {
                if (lblTipo.Text != "10")
                {
                    //if (Request["tipo"].ToString() != "5")
                    //{

                    if ((e.Row.RowType == DataControlRowType.Header))
                    {
                        //if (Request["Agrupado"].ToString() == "0")/// por origen
                        //e.Row.Cells[5].Text = "TOTAL";
                        //else

                        //e.Row.Cells[3].Text = "TOTAL";

                    }
                    if ((e.Row.RowType == DataControlRowType.DataRow))
                    {
                        if (e.Row.Cells[1].Text != "&nbsp;")
                        {
                            suma1 += int.Parse(e.Row.Cells[1].Text);
                            col1 += int.Parse(e.Row.Cells[1].Text);
                        }

                        if (e.Row.Cells[2].Text != "&nbsp;")
                        {
                            suma2 += int.Parse(e.Row.Cells[2].Text);
                            col1 += int.Parse(e.Row.Cells[2].Text);
                        }

                        if (Session["Agrupado"].ToString() == "0")/// por origen
                        {
                            if (e.Row.Cells[3].Text != "&nbsp;")
                            {
                                suma3 += int.Parse(e.Row.Cells[3].Text);
                                col1 += int.Parse(e.Row.Cells[3].Text);
                            }

                            if (e.Row.Cells[4].Text != "&nbsp;")
                            {
                                suma4 += int.Parse(e.Row.Cells[4].Text);
                                col1 += int.Parse(e.Row.Cells[4].Text);
                            }
                            if (e.Row.Cells[5].Text != "&nbsp;")
                            {
                                suma5 += int.Parse(e.Row.Cells[5].Text);
                                col1 += int.Parse(e.Row.Cells[5].Text);
                            }

                            if (e.Row.Cells[6].Text != "&nbsp;")
                            {
                                suma6 += int.Parse(e.Row.Cells[6].Text);
                                col1 += int.Parse(e.Row.Cells[6].Text);
                            }
                            //e.Row.Cells[5].Text = col1.ToString();
                        }

                        if (Session["Agrupado"].ToString() == "1")/// por prioridad
                        {
                            e.Row.Cells[3].Text = col1.ToString();
                        }

                        col1 = 0;

                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        if (Session["tipo"].ToString() != "5")
                        {

                            e.Row.Cells[0].Text = "Total";
                            e.Row.Cells[1].Text = suma1.ToString();
                            e.Row.Cells[2].Text = suma2.ToString();

                            if (Session["Agrupado"].ToString() == "0")/// por origen
                            {
                                e.Row.Cells[3].Text = suma3.ToString();
                                e.Row.Cells[4].Text = suma4.ToString();
                                e.Row.Cells[5].Text = suma5.ToString();
                                e.Row.Cells[6].Text = suma6.ToString();

                                col1 = suma1 + suma2 + suma3 + suma4 + suma5 + suma6;
                                e.Row.Cells[7].Text = col1.ToString();
                            }
                            if (Session["Agrupado"].ToString() == "1")/// por prioridad
                            {
                                col1 = suma1 + suma2;
                                e.Row.Cells[3].Text = col1.ToString();
                            }

                            col1 = 0;
                        }


                    }

                    if (Session["tipo"].ToString() == "5") if (e.Row.RowType == DataControlRowType.Footer) e.Row.Visible = false;
                    //}

                }

            
                //if (e.Row.RowType == DataControlRowType.Footer)
                //{
                //    e.Row.Cells[5].Text = col1.ToString();
                    
                //}


            }
            catch
            { }

        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            if (lblInforme.Text == "General")
                MostrarInformeGeneral("PDF");
            else
                MostrarInformePorResultado("PDF");
        }

        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {
            if (lblInforme.Text == "General")
                MostrarInformeGeneral("Imprimir");
            else
                MostrarInformePorResultado("Imprimir");
        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (lblInforme.Text == "General")
                // MostrarInformeGeneral("Excel");
                ExportarExcel();
            //else
            //    MostrarInformePorResultado("Excel");
        }

        private void ExportarExcel()
        {


            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gvEstadistica.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false; 

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize(); 
            page.Controls.Add(form);
            form.Controls.Add(gvEstadistica);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=estadistica.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void lnkDetallePorDet_Click(object sender, EventArgs e)
        {
            dETALLEAExcel(GetDataSetDetalle(),"DetalleDerivaciones");
        }

        public DataTable GetDataSetDetalle()
        {
            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_EstadisticaDetalleDerivacion]";


            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(Session["fechaDesde"].ToString());
            DateTime fecha2 = DateTime.Parse(Session["fechaHasta"].ToString());

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");


            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@idEfectorProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idEfectorProtocolo"].Value = Session["idEfector"].ToString();

            //cmd.Parameters.Add("@derivacion", SqlDbType.Bit);
            //if (ddlDerivaciones.SelectedValue == "S")
            //    cmd.Parameters["@derivacion"].Value = 1;  ///tabla cruzada con efectores
            //else
            //    cmd.Parameters["@derivacion"].Value = 0;  ///agrupado por efectores

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);

            return Ds.Tables[0];
        }
        private void dETALLEAExcel(DataTable tabla, string nombreArchivo)
        {
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
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreArchivo + "_" + oUser.IdEfector.IdEfector2 + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
