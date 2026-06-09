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
using System.Data.SqlClient;
using InfoSoftGlobal;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using System.IO;
using CrystalDecisions.Web;
using System.Text;
using Business.Data;

namespace WebLab.Estadisticas
{
    public partial class Turnos : System.Web.UI.Page
    {

        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();




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
                if (oUser != null)
                    oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

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
                    VerificaPermisos("De Turnos");
                    CargarListas();
                    txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
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
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                        case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
            if (oUser.IdEfector.IdEfector == 227) // puede elegir el efector que quiere ver o todos
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E  (nolock)" +
                     " INNER JOIN lab_Configuracion C  (nolock) on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                ddlEfector.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  (nolock) where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                listaEfectorSolicitante();

            }

            

            m_ssql = null;
            oUtil = null;
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            MostrarReporte();

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;
            try
            {
                if (DateTime.Parse(txtFechaDesde.Value) > DateTime.Parse(txtFechaHasta.Value))
                {
                    args.IsValid = false;
                    CustomValidator1.ErrorMessage = "Fecha hasta debe ser menor o igual que fecha desde";
                }
                else args.IsValid = true;

            }
            catch
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "Error fecha invalida ingresada";
            }
        }

    
        private void MostrarReporte()
        {
            if (ddlEfector.SelectedValue == "0")
                imgPdf.Visible = false;
            else
            imgPdf.Visible = true;

            DataTable dt= getDatosEstadisticos("G");

            //if (dt.Rows[2][1].ToString() != "0")
            if (dt.Rows.Count>0)
            {
                pnlDatos.Visible = true;
                pnlSinDatos.Visible = false;
                gvLista.DataSource = dt;
                gvLista.DataBind();

               // FCLiteral.Text = CreateChart1(dt);
            }
            else
            {
                pnlDatos.Visible = false;
                pnlSinDatos.Visible = true;
            }
          


        }

        private DataTable getDatosEstadisticos(string tipo)
        {
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_EstadisticaTurnos]";

            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            /////
            ///
            ///Parametro Servicio
            cmd.Parameters.Add("@idTipoServicio", SqlDbType.NVarChar);
            cmd.Parameters["@idTipoServicio"].Value = ddlServicio.SelectedValue;
            ///


            ///Parametro tipo de agrupacion
            cmd.Parameters.Add("@agrupado", SqlDbType.NVarChar);
            cmd.Parameters["@agrupado"].Value = tipo;


            cmd.Parameters.Add("@idEfector", SqlDbType.Int);
            cmd.Parameters["@idEfector"].Value = int.Parse(ddlEfector.SelectedValue);

            cmd.Parameters.Add("@idEfectorSolicitante", SqlDbType.Int);
            int idEfectorSolicitante =   (ddlEfectorSolicitante.SelectedValue != "") ? int.Parse(ddlEfectorSolicitante.SelectedValue) : 0;
            cmd.Parameters["@idEfectorSolicitante"].Value = idEfectorSolicitante;

            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            return Ds.Tables[0];
        }

       

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            MostrarPDF();
        }

        private void ExportarExcel()
        {
            DataTable dt = getDatosEstadisticos("G");
            string encabezado="";
            if (oUser.IdEfector.IdEfector != 227)
            {
                if (int.Parse(ddlEfectorSolicitante.SelectedValue) != 0)
                    encabezado = ddlEfectorSolicitante.SelectedItem.Text;
                else 
                    encabezado =  ddlEfector.SelectedItem.Text;
            }
            Utility.ExportDataTableToXlsx(dt, "estadistica_turnos", encabezado);

           
        }
        private void MostrarPDF()
        {

            //DataTable dt = new DataTable();
              DataTable dt= getDatosEstadisticos("C");
            if (dt.Rows.Count > 0)
            {
             //   Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                if (ddlEfector.SelectedValue == "0")
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
                    oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                    if (oEfector != null)
                    {
                        Configuracion oCon = new Configuracion();
                        oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oEfector);

                        //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                        encabezado1.Value = oCon.EncabezadoLinea1;

                        //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                        encabezado2.Value = oCon.EncabezadoLinea2;

                        //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                        encabezado3.Value = oCon.EncabezadoLinea3;

                        if(ddlEfectorSolicitante.SelectedValue != "" && ddlEfectorSolicitante.SelectedValue != "0")
                        {
                            Efector oEfectorSolicitante = new Efector();
                            oEfectorSolicitante = (Efector)oEfectorSolicitante.Get(typeof(Efector), int.Parse(ddlEfectorSolicitante.SelectedValue));
                            if (oEfectorSolicitante != null)
                            {
                                encabezado3.Value = oEfectorSolicitante.Nombre;
                            }
                        }
                        
                    }

                }

                ParameterDiscreteValue encabezado4 = new ParameterDiscreteValue();
                encabezado4.Value = "ASISTENCIAS DE TURNOS";

                oCr.Report.FileName = "Turno.rpt";
              
                        
                  

                
                ParameterDiscreteValue subTitulo = new ParameterDiscreteValue();
                subTitulo.Value = txtFechaDesde.Value + " - " + txtFechaHasta.Value;

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("EstadisticaTurno");
               

                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(encabezado4);
                oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(subTitulo);
                
                oCr.DataBind();

                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
 

            }
            
        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel();
        }

        protected void btnDescargarDetallado_Click(object sender, EventArgs e)
        {

        }

        private void listaEfectorSolicitante()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = @" SELECT distinct e.idEfector,E.nombre as nombre
                             FROM  LAB_Agenda A (nolock) 
                             INNER JOIN sys_Efector E (nolock) on E.idEfector=A.idEfectorSolicitante
                            where A.baja=0 and a.idEfector=" + ddlEfector.SelectedValue.ToString() + " and a.idEfectorSolicitante<>" + ddlEfector.SelectedValue.ToString();
            oUtil.CargarCombo(ddlEfectorSolicitante, m_ssql, "idEfector", "nombre", connReady);
            
            if (ddlEfectorSolicitante.Items.Count > 0)
            {
                ddlEfectorSolicitante.Items.Insert(0, new ListItem("--TODOS--", "0"));
                ddlEfectorSolicitante.Visible = true; lblEfectorSolicitante.Visible = true;
            }
            else
            {
                ddlEfectorSolicitante.Visible = false; lblEfectorSolicitante.Visible = false;
            }
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (oUser.IdEfector.IdEfector == 227)
            {
                if (ddlEfector.SelectedIndex != 0)
                {
                     //Borro las Caps de otro Efector y vuelvo a cargar
                    ddlEfectorSolicitante.Items.Clear();
                    listaEfectorSolicitante();
                }
                else
                {
                    ddlEfectorSolicitante.Visible = false; lblEfectorSolicitante.Visible = false;
                    ddlEfectorSolicitante.Items.Clear();
                }
            }

                
            
        }
    }
}
