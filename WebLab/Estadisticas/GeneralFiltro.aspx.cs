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

namespace WebLab.Estadisticas
{
    public partial class GeneralFiltro : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public  Usuario oUser = new Usuario();

    
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

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));


            if (oUser.IdEfector.IdEfector==227) // puede elegir el efector que quiere ver o todos
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E (nolock)" +
                     " INNER JOIN lab_Configuracion C (nolock) on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                ddlEfector.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E (nolock)  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }





            CargarArea();

            ///Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector (nolock) order by nombre ";
            oUtil.CargarCombo(ddlEfectorSolicitante, m_ssql, "idEfector", "nombre", connReady);
            ddlEfectorSolicitante.Items.Insert(0, new ListItem("Todos", "0"));

            ///Carga de combos de Medicos Solicitantes
            if (oUser.IdEfector.IdEfector == 227)
                m_ssql = @"select distinct P.Especialista from LAB_Protocolo P  (nolock) order by P.Especialista ";
            else
                m_ssql = @"select distinct P.Especialista from LAB_Protocolo P (nolock) where P.idEfector=" + oUser.IdEfector.IdEfector.ToString()+" order by P.Especialista ";
            oUtil.CargarCombo(ddlEspecialista, m_ssql, "Especialista", "Especialista", connReady);
            ddlEspecialista.Items.Insert(0, new ListItem("Todos", "0"));

            m_ssql = null;
            oUtil = null;
        }



        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();

            ddlArea.UpdateAfterCallBack = true;
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de areas
            string m_ssql ="";
            if (ddlServicio.SelectedValue =="0")
                m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0  order by nombre";
            else
             m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0 and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));

            m_ssql = null;
            oUtil = null;

        }

        

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Response.Redirect("GeneralReporte.aspx", false);
            Response.Redirect("Reporte.aspx", false);
        }

     

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                HttpContext Context;

                Context = HttpContext.Current;
                Session["informe"]= "General";
                Session["tipo"]= rdbTipoInforme.SelectedValue;
                Session["idEfector"]= ddlEfector.SelectedValue;
                Session["fechaDesde"]= txtFechaDesde.Value;
                Session["fechaHasta"]= txtFechaHasta.Value;
                Session["Agrupado"]= rdbAgrupacion.SelectedValue;
                Session["idTipoServicio"]= ddlServicio.SelectedValue;
                Session["idArea"]= ddlArea.SelectedValue;
                Session["grafico"]= rdbGrafico.SelectedValue;
                Session["estado"]= rdbEstado.SelectedValue;
                Session["idEfectorSolicitante"]= ddlEfectorSolicitante.SelectedValue;
                Session["idEspecialista"]= ddlEspecialista.SelectedValue;
                Session["horaDesde"]= txtHoraDesde.Text;
                Session["horaHasta"]= txtHoraHasta.Text;
                Server.Transfer("Reporte.aspx");

                //Response.Redirect("GeneralReporte.aspx?informe=General&tipo="+rdbTipoInforme.SelectedValue +"&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value + "&Agrupado=" + rdbAgrupacion.SelectedValue + "&idTipoServicio="+ ddlServicio.SelectedValue + "&idArea=" + ddlArea.SelectedValue + "&grafico="+ rdbGrafico.SelectedValue  , false);
                //Response.Redirect("Reporte.aspx?informe=General&tipo=" + rdbTipoInforme.SelectedValue + "&idEfector="+ 
                //    ddlEfector.SelectedValue +"&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value +
                //    "&Agrupado=" + rdbAgrupacion.SelectedValue + "&idTipoServicio=" + ddlServicio.SelectedValue + "&idArea=" + ddlArea.SelectedValue 
                //    + "&grafico=" + rdbGrafico.SelectedValue+"&estado="+ rdbEstado.SelectedValue+"&idEfectorSolicitante="+ ddlEfectorSolicitante.SelectedValue+ 
                //    "&idEspecialista="+ ddlEspecialista.SelectedValue, false);
            }
        }

        protected void rdbTipoInforme_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlArea.Enabled = true; rdbGrafico.Enabled = true;
            txtHoraDesde.Enabled = false;
            txtHoraHasta.Enabled = false;
            CustomValidatorHoras.Enabled = false;

            if (rdbTipoInforme.SelectedValue == "0") lblDescripcion.Text = "Conteo por Areas: Muestra la cantidad de analisis solicitados, discriminados por areas del laboratorio.";
            if (rdbTipoInforme.SelectedValue == "1") lblDescripcion.Text = "Conteo por Análisis: Muestra la cantidad de analisis solicitados, discriminados por analisis del laboratorio..";
            if (rdbTipoInforme.SelectedValue == "2")
            {
                lblDescripcion.Text = "Conteo por Médico: Muestra la cantidad de protocolos solicitados, discriminados por médico solicitante.";
                ddlArea.Enabled = false;

            }

            if (rdbTipoInforme.SelectedValue == "3")
            {
                lblDescripcion.Text = "Conteo por Efector Solicitante: Muestra la cantidad de protocolos solicitados, discriminados por efector solicitante.";
                ddlArea.Enabled = false;
            }
            if (rdbTipoInforme.SelectedValue == "12")///nuevo
            {
                lblDescripcion.Text = "Conteo de Derivaciones Derivadas: Muestra la cantidad de determinaciones que fueron derivadas, discriminados por efector derivante.";
                /*contar de la tabla lab_derivaciones por lab_derivacion.idEfector, por ej. Andacollo envió xxx determinaciones a HPN; se mnuestra en el reporte Andacollo*/
            }

            if (rdbTipoInforme.SelectedValue == "4")
            {
                lblDescripcion.Text = "Conteo de Derivaciones Enviadas: Muestra la cantidad de determinaciones que fueron derivadas, discriminados por efector de derivación, es decir, efector de destino.";
                /*contar de la tabla lab_derivaciones por lab_derivacion.idEfector, por ej. Andacollo envió xxx determinaciones a HPN; se mnuestra en el reporte HPN*/
            }
            if (rdbTipoInforme.SelectedValue == "11") ///nuevo
            {
                lblDescripcion.Text = "Conteo de Derivaciones Recibidas: Muestra la cantidad de determinaciones que fueron recibidas, discriminados por efector derivante, es decir, efector de origen.";
                /*contar de la tabla lab_protocolo por lab_protocolo.idSector='Derivacion', 
                por ej. Andacollo recibió xxx determinaciones de  Las ovejas; se mnuestra en el reporte Andacollo*/
            }
            if (rdbTipoInforme.SelectedValue == "5")
            {
                lblDescripcion.Text = "Totalizado: Muestra la cantidad de analisis, cantidad de protocolos y el promedio de analisis por protocolo.";
                ddlArea.Enabled = false;
            }

            if (rdbTipoInforme.SelectedValue == "6") lblDescripcion.Text = "Conteo por Diagnósticos: Muestra la cantidad de protocolos, discriminados por diagnóstico.";

            if (rdbTipoInforme.SelectedValue == "7")
            {///no muestra grafico
                lblDescripcion.Text = "Protocolos por Día: Muestra para cada dia, dentro de un rango de fechas seleccionado, la cantidad de protocolos recepcionados.";
                ddlArea.Enabled = false;
                rdbGrafico.Enabled = false;

            }
            if (rdbTipoInforme.SelectedValue == "10")
            {///no muestra grafico
                lblDescripcion.Text = "Protocolos por Día- Franja Horaria: Muestra para cada dia los protocolos recepcionados dentro de la franja horaria indicada.";
                ddlArea.Enabled = false;
                rdbGrafico.Enabled = false;
                txtHoraDesde.Enabled = true;
                txtHoraHasta.Enabled = true;
                CustomValidatorHoras.Enabled = true;
            }

            if (rdbTipoInforme.SelectedValue == "8")
            {
                lblDescripcion.Text = "Conteo por Sector/Servicio: Muestra la cantidad de protocolos recepcionados, discriminados por sector/servicio de origen.";
                ddlArea.Enabled = false;

            }

            if (rdbTipoInforme.SelectedValue == "9")
            {
                lblDescripcion.Text = "Ranking por día de la semana: Muestra la cantidad de protocolos recepcionados en cada día de la semana.";
                ddlArea.Enabled = false;

            }
            rdbGrafico.UpdateAfterCallBack=true;
            ddlArea.UpdateAfterCallBack = true;
            lblDescripcion.Visible = true;
            lblDescripcion.UpdateAfterCallBack = true;
            txtHoraDesde.UpdateAfterCallBack = true;
            txtHoraHasta.UpdateAfterCallBack = true;
            CustomValidatorHoras.UpdateAfterCallBack = true;

        }

        protected void CustomValidatorHoras_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool valido = true;
            if (txtHoraDesde.Text == "")
            {
                valido = false;
            }
            else
            {
                try
                {
                    var s = TimeSpan.Parse(txtHoraDesde.Text);
                    valido = true;
                }
                catch (Exception e)
                { valido = false; }
               
            }


               if (txtHoraHasta.Text == "") valido = false;
            else
            {
                try
                {
                    var s = TimeSpan.Parse(txtHoraHasta.Text);
                    valido = true;
                }
                catch (Exception e1)
                { valido = false; }

            }

            args.IsValid = valido;
        }
    }
}
