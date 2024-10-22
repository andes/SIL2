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

namespace WebLab.ImpresionResult
{
    public partial class ImprimirBusqueda : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               CargarPagina();
            }
        }

        private void CargarPagina()
        {

            VerificaPermisos("Impresión");
            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            CargarListas();

            IniciarValores(); HabilitarSegundaArea();
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
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string m_parametro = "?idTipoServicio=" + ddlServicio.SelectedValue;
                /////////
                ///  m_parametro += "&idArea=" + ddlArea.SelectedValue;
                string s_areas = "0";

                if (ddlArea.SelectedValue != "0")
                {

                    s_areas = ddlArea.SelectedValue;

                }

                if ((ddlArea.SelectedValue != "0") && (ddlArea2.SelectedValue != "0") && (ddlArea2.Visible))
                {

                    s_areas = s_areas + "," + ddlArea2.SelectedValue;
                }
                if (s_areas != "") m_parametro += "&idArea=" + s_areas;
                //////////////
                m_parametro += "&FechaDesde=" + txtFechaDesde.Value;
                m_parametro += "&FechaHasta=" + txtFechaHasta.Value;
                m_parametro += "&ProtocoloDesde=" + txtProtocoloDesde.Value;
                m_parametro += "&ProtocoloHasta=" + txtProtocoloHasta.Value;
                m_parametro += "&ProtocoloRango=" + txtRangoProtocolo.Text;

                m_parametro += "&idOrigen=" + ddlOrigen.SelectedValue;
                m_parametro += "&idPrioridad=" + ddlPrioridad.SelectedValue;
                m_parametro += "&idEfectorSolicitante=" + ddlEfector.SelectedValue;
                //   m_parametro += "&idSectorServicio=" + ddlSectorServicio.SelectedValue;

                m_parametro += "&idSectorServicio=" + getListaSectores();

                if (rdbOpcion.Items[0].Selected) m_parametro += "&Impreso=0"; //pendientes de imprimir
                if (rdbOpcion.Items[1].Selected) m_parametro += "&Impreso=1"; /// impresos
                if (rdbOpcion.Items[2].Selected) m_parametro += "&Impreso=2";//todos

                if (rdbEstado.Items[0].Selected) m_parametro += "&Estado=0"; //no procesado
                if (rdbEstado.Items[1].Selected) m_parametro += "&Estado=1";// en proceso
                if (rdbEstado.Items[2].Selected) m_parametro += "&Estado=2";// terminado
                if (rdbEstado.Items[3].Selected) m_parametro += "&Estado=3";// todos

                if (chkRecordarFiltro.Checked) AlmacenarSesion();


                Response.Redirect("ResultadoList.aspx" + m_parametro + "&modo=" + Request["modo"].ToString(), false);
            }

        }

        private void AlmacenarSesion()
        {
           string s_valores = "ddlServicio:" + ddlServicio.SelectedValue;
           s_valores += ";ddlArea:" + ddlArea.SelectedValue;            
           s_valores += ";txtFechaDesde:" + txtFechaDesde.Value;
           s_valores += ";txtFechaHasta:" + txtFechaHasta.Value;
           s_valores += ";lstSector:" + getListaSectores();  
           s_valores += ";txtProtocoloDesde:" + txtProtocoloDesde.Value;
           s_valores += ";txtProtocoloHasta:" + txtProtocoloHasta.Value;
           s_valores += ";txtRangoProtocolo:" + txtRangoProtocolo.Text;            
           s_valores += ";ddlOrigen:" + ddlOrigen.SelectedValue;            
           s_valores += ";ddlEfector:" + ddlEfector.SelectedValue;
           s_valores += ";ddlPrioridad:" + ddlPrioridad.SelectedValue;
           s_valores += ";rdbEstado:" + rdbEstado.SelectedValue;
           s_valores += ";rdbOpcion:" + rdbOpcion.SelectedValue;            
           Session["Impresion"] = s_valores;
        }

        private void IniciarValores()
        {
            if (Session["Impresion"] != null)
            {
                string[] arr = Session["Impresion"].ToString().Split((";").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        case "ddlServicio": ddlServicio.SelectedValue = s_control[1].ToString(); break;
                        case "ddlArea": ddlArea.SelectedValue = s_control[1].ToString(); break;                            
                        case "txtFechaDesde": txtFechaDesde.Value = s_control[1].ToString(); break;
                        case "txtFechaHasta": txtFechaHasta.Value = s_control[1].ToString(); break;
                        case "lstSector":
                            {
                                for (int i = 0; i < lstSector.Items.Count; i++)
                                {
                                    lstSector.Items[i].Selected = false;
                                }
                                string[] arrSector = s_control[1].ToString().Split((",").ToCharArray());
                                foreach (string itemSector in arrSector)
                                {
                                    for (int j = 0; j < arrSector.Count(); j++)
                                    {
                                        for (int i = 0; i < lstSector.Items.Count; i++)
                                        {
                                            if (int.Parse(lstSector.Items[i].Value) == int.Parse(arrSector[j].ToString()))
                                                lstSector.Items[i].Selected = true;
                                        }
                                    }
                                }
                            }
                            break;
                        case "txtProtocoloDesde": txtProtocoloDesde.Value = s_control[1].ToString(); break;
                        case "txtProtocoloHasta": txtProtocoloHasta.Value = s_control[1].ToString(); break;
                        case "txtRangoProtocolo": txtRangoProtocolo.Text = s_control[1].ToString(); break;                            
                        case "ddlOrigen": ddlOrigen.SelectedValue = s_control[1].ToString(); break;
                        case "ddlEfector": ddlEfector.SelectedValue = s_control[1].ToString(); break;
                        case "ddlPrioridad": ddlPrioridad.SelectedValue = s_control[1].ToString(); break;
                        case "rdbEstado": rdbEstado.SelectedValue = s_control[1].ToString(); break;
                        case "rdbOpcion": rdbOpcion.SelectedValue = s_control[1].ToString(); break;                     
                    }
                }
            }

        }
        private object getListaSectores()
        {
            string m_lista = "";
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                if (lstSector.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstSector.Items[i].Value;
                    else
                        m_lista += "," + lstSector.Items[i].Value;
                }

            }
            return m_lista;
        }
        private void CargarListas()
        {

            if (Request["modo"].ToString() == "Urgencia") imgUrgencia.Visible = true;

            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE idTipoServicio= " + Request["idServicio"].ToString();
            //if ((Request["idServicio"].ToString()=="3")|| (Request["idServicio"].ToString() == "5"))
            //      m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE idTipoServicio= 3";

            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlServicio.Visible = false;
            lblServicio.Text = ddlServicio.SelectedItem.Text;

            
            CargarArea();

            ///Carga de Sectores          
            m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre FROM LAB_SectorServicio (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = true;
            }
    

            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre", connReady);
            ddlOrigen.Items.Insert(0, new ListItem("-- Todos --", "0"));

            ///Carga de combos de Prioridad
            m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad (nolock) WHERE     (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre", connReady);
        ddlPrioridad.Items.Insert(0, new ListItem("-- Todos --", "0"));

          //  if (Request["modo"].ToString() == "Normal") {  ddlPrioridad.SelectedValue = "1"; }
            if (Request["modo"].ToString() == "Urgencia")
            {
                ddlPrioridad.SelectedValue = "2";
                ddlPrioridad.Enabled = false;
            }

            ///Carga de Efectores solicitantes
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector (nolock) order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            ddlEfector.Items.Insert(0, new ListItem("-- Todos --", "0"));

            if ((Request["idServicio"].ToString() == "1") ||(Request["idServicio"].ToString() == "3"))//microbiologia//microbiologia 
            {              
                //////////El rango de fechas no es a fecha actual, sino los ultimos 30 dias
                txtFechaDesde.Value = DateTime.Now.AddDays(-10).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                //// La prioridad siempre es a rutina
             /*   ddlPrioridad.SelectedValue = "1"; //rutina
                ddlPrioridad.Visible = false;
                lblPrioridad.Visible = false;*/
            }


            if (Session["FiltroImpresion"] != null)
            {
                string[] Filtro = Session["FiltroImpresion"].ToString().Split(';');
                rdbOpcion.SelectedValue= Filtro[0].ToString();
                rdbEstado.SelectedValue= Filtro[1].ToString();
            }

            m_ssql = null;
            oUtil = null;
        }
        //protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CargarArea();

        //    ddlArea.UpdateAfterCallBack = true;
        //}

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0 and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";

            if ((ddlServicio.SelectedValue=="3") || (ddlServicio.SelectedValue=="5"))
                m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0 and idTipoServicio=3 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre",connReady);
            ddlArea.Items.Insert(0, new ListItem("--Todas--", "0"));
            m_ssql = null;
            oUtil = null;

        }

        protected void ddlServicio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CargarArea();

        }

        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            Session["Impresion"] = null;
            CargarPagina();

        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            HabilitarSegundaArea();
        }

        private void HabilitarSegundaArea()
        {
            if (ddlArea.SelectedValue != "0")
            {

                if (!ddlArea2.Visible)
                {
                    ddlArea2.Visible = true;
                    imgAgregarArea.Visible = true;

                }
                    ddlArea2.Items.Clear();
                    Utility oUtil = new Utility(); string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

                ///Carga de combos de areas
                string m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue;
                if ((ddlServicio.SelectedValue == "3") || (ddlServicio.SelectedValue == "5"))
                    m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=3";

                m_ssql+=" and idArea <>" + ddlArea.SelectedValue + "  order by nombre";
                    oUtil.CargarCombo(ddlArea2, m_ssql, "idArea", "nombre", connReady);
                    ddlArea2.Items.Insert(0, new ListItem("--Seleccione Area Adicional--", "0"));

                    ddlArea2.UpdateAfterCallBack = true;
                    imgAgregarArea.UpdateAfterCallBack = true;


                
            }
            else
            {
                ddlArea2.Visible = false;
                imgAgregarArea.Visible = false;
                ddlArea2.UpdateAfterCallBack = true;
                imgAgregarArea.UpdateAfterCallBack = true;

            }

        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtProtocoloDesde.Value != "") { if (oUtil.EsEntero(txtProtocoloDesde.Value)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

        protected void cvNumeroHasta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtProtocoloHasta.Value != "") { if (oUtil.EsEntero(txtProtocoloHasta.Value)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;      
        }

        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        private void MarcarSeleccionados(bool p)
        {
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = p;
            }



        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }
    }
}
