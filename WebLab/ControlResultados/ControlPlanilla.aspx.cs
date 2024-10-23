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
using Business.Data;

namespace WebLab.ControlResultados
{
    public partial class ControlPlanilla : System.Web.UI.Page
    {
        Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
             
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {



                CargarPagina();



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
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
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
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");

            ///Carga de Sectores
            m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre FROM LAB_SectorServicio with (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");

            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = true;
            }
            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("Todos", "0"));

            ///Carga de combos de Prioridad
            m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre");
            ddlPrioridad.Items.Insert(0, new ListItem("Todos", "0"));

            ddlPrioridad.SelectedValue = "1"; //RUTINA
          
          

            ///Carga de combos de areas
            CargarArea(); HabilitarHojaTrabajo();
            //CargarAnalisis();


            ///Carga de Efectores solicitantes
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector with (nolock) order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));
            //ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();


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
            ddlArea.Items.Clear();
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "select idArea, nombre from Lab_Area with (nolock) where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            if (Request["tipo"].ToString() == "formula")
            {
                ddlArea.Items.Insert(0, new ListItem("Todas", "0"));
                rvArea.Enabled = false;
                ddlHojaTrabajo.Visible = false;
                rvHojaTrabajo.Enabled = false;

            }
            else
            { 
                ddlArea.Items.Insert(0, new ListItem("Seleccione Area", "0"));
                rvArea.Enabled = true;
                ddlHojaTrabajo.Visible = true;
                rvHojaTrabajo.Enabled = true;
            }

            rvArea.UpdateAfterCallBack = true;
            ddlHojaTrabajo.UpdateAfterCallBack = true;
            rvHojaTrabajo.UpdateAfterCallBack = true;

              


                ddlPrioridad.UpdateAfterCallBack = true;
            rvArea.UpdateAfterCallBack = true;
            m_ssql = null;
            oUtil = null;



        }

        private void MostrarInforme()
        {


        }



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

                string m_parametro = " P.idTipoServicio=" + ddlServicio.SelectedValue;
                m_parametro += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "' AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
                m_parametro += " and P.idEfector =" + oUser.IdEfector.IdEfector.ToString();
                //if (ddlArea.SelectedValue != "0") m_parametro += " AND i.idArea=" + ddlArea.SelectedValue;


                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                switch (oCon.TipoNumeracionProtocolo)// busqueda con autonumerico
                {
                    case 0:
                        {
                            if (txtProtocoloDesde.Value != "") m_parametro += " And P.numero>=" + int.Parse(txtProtocoloDesde.Value);
                            if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numero<=" + int.Parse(txtProtocoloHasta.Value);
                        } break;
                    case 1:
                        {
                            if (txtProtocoloDesde.Value != "") m_parametro += " And P.numeroDiario>=" + int.Parse(txtProtocoloDesde.Value);
                            if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numeroDiario<=" + int.Parse(txtProtocoloHasta.Value);
                        } break;
                    case 2:
                        {
                            if (txtProtocoloDesde.Value != "") m_parametro += " And P.numeroSector>=" + int.Parse(txtProtocoloDesde.Value);
                            if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numeroSector<=" + int.Parse(txtProtocoloHasta.Value);
                        } break;

                    case 3:
                        {
                            if (txtProtocoloDesde.Value != "") m_parametro += " And P.numeroTipoServicio>=" + int.Parse(txtProtocoloDesde.Value);
                            if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numeroTipoServicio<=" + int.Parse(txtProtocoloHasta.Value);
                        } break;
                }


                if (ddlEfector.SelectedValue != "0") m_parametro += " AND P.idEfectorSolicitante=" + ddlEfector.SelectedValue;
                if (ddlOrigen.SelectedValue != "0") m_parametro += " AND P.idOrigen=" + ddlOrigen.SelectedValue;
                if (ddlPrioridad.SelectedValue != "0") m_parametro += " AND P.idPrioridad=" + ddlPrioridad.SelectedValue;

            //    if (ddlSectorServicio.SelectedValue != "0") m_parametro += " AND P.idSector = " + ddlSectorServicio.SelectedValue;

                m_parametro += " AND P.idSector in (" + getListaSectores() + ")";
                string m_orden = " order by  P.numero ";
                //string operacion = "Carga";
                // string modoCarga = "HT";

                if (chkRecordarFiltro.Checked) AlmacenarSesion();

                if (Request["tipo"].ToString() == "formula")
                    Response.Redirect("ProtocoloList.aspx?idServicio=" + ddlServicio.SelectedValue + "&Parametros=" + m_parametro + "&tipo=" + Request["tipo"].ToString(), false);
                if (Request["tipo"].ToString()=="ht")
                    Response.Redirect("../Resultados/Procesa.aspx?idServicio="+ddlServicio.SelectedValue + "&ModoCarga=HT&Operacion=Control&Parametros=" + m_parametro + "&idArea=" + ddlArea.SelectedValue + "&idHojaTrabajo=" + ddlHojaTrabajo.SelectedValue + "&idItem=0&validado=0&modo=" + Request["modo"] + "&control=1&orden=" + m_orden , false);
                                //Response.Redirect("Procesa.aspx?idServicio=" + Request["idServicio"].ToString() + "&ModoCarga=" + modoCarga + "&Operacion=" + operacion + "&Parametros=" + m_parametro + "&idArea=" + s_areas + "&idHojaTrabajo=" + ddlHojaTrabajo.SelectedValue + "&idItem=" + ddlAnalisis.SelectedValue + "&validado=0&modo=" + Request["modo"] + "&dni=" + txtDNI.Value + "&orden=" + m_orden, false);
            }

        }

        private void AlmacenarSesion()
        {
            string s_valores = "ddlServicio:" + ddlServicio.SelectedValue;
            s_valores += ";ddlArea:" + ddlArea.SelectedValue;
            s_valores += ";ddlHojaTrabajo:" + ddlHojaTrabajo.SelectedValue;            
            s_valores += ";txtFechaDesde:" + txtFechaDesde.Value;
            s_valores += ";txtFechaHasta:" + txtFechaHasta.Value;
            s_valores += ";txtProtocoloDesde:" + txtProtocoloDesde.Value;
            s_valores += ";txtProtocoloHasta:" + txtProtocoloHasta.Value;
            s_valores += ";ddlOrigen:" + ddlOrigen.SelectedValue;
            s_valores += ";ddlEfector:" + ddlEfector.SelectedValue;
           //  s_valores += ";ddlPrioridad:" + ddlPrioridad.SelectedValue;            
            s_valores += ";lstSector:" + getListaSectores();
            Session["Control"] = s_valores;
        }

      

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (Request["tipo"].ToString() == "ht")
            {
                HabilitarHojaTrabajo();
            }
         
        }

        private void HabilitarAnalisis()
        {
            throw new NotImplementedException();
        }

        private void HabilitarHojaTrabajo()
        {
            if (ddlArea.SelectedValue != "0") 
            {
                if (Request["tipo"].ToString() == "formula")
                {
                    ddlHojaTrabajo.Visible = false;
                    rvHojaTrabajo.Enabled = false;
                    rvHojaTrabajo.UpdateAfterCallBack = true;
                }
                if (Request["tipo"].ToString() == "ht")
                {
                    CargarHojaTrabajo();
                    ddlHojaTrabajo.Enabled = true;
                    ddlHojaTrabajo.Visible = true;
                    rvHojaTrabajo.Enabled = true;
                }

            }
            else
            {
                ddlHojaTrabajo.Items.Insert(0, new ListItem("Seleccione", "0"));
               
            }
           



            ddlHojaTrabajo.UpdateAfterCallBack = true;
            rvHojaTrabajo.UpdateAfterCallBack = true;
        }

        private void CargarHojaTrabajo()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "select idHojaTrabajo, codigo from Lab_HojaTrabajo with (nolock)  where baja=0  and idArea=" + ddlArea.SelectedValue + "  and   idEfector=" + oUser.IdEfector.IdEfector.ToString() + " order by codigo ";

             

            oUtil.CargarCombo(ddlHojaTrabajo, m_ssql, "idHojaTrabajo", "codigo");
            // ddlHojaTrabajo.Items.Insert(0, new ListItem("Seleccione Hoja de Trabajo", "0"));     

        }

        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;

        }

        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            Session["Control"] = null;
          

            CargarPagina();
        }

        private void CargarPagina()
        {
            if (Request["tipo"].ToString() == "formula")
            {
                VerificaPermisos("Calcular Formulas");
                lblTitulo.Text = "CALCULO MASIVO DE FORMULAS";
                lblTituloFormula.Visible = true;

            }
            else
            {
                VerificaPermisos("Control de HT");
                lblTitulo.Text = "CONTROL DE HOJA DE TRABAJO";
                lblTituloFormula.Visible = false;

            }
            CargarListas();


            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            IniciarValores();
        }
        private void IniciarValores()
        {             

            if (Session["Control"] != null)
            {
                string[] arr = Session["Control"].ToString().Split((";").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        case "ddlServicio":
                            {
                                ddlServicio.SelectedValue = s_control[1].ToString();                                
                            } break;
                        case "ddlArea":
                            {
                                ddlArea.SelectedValue = s_control[1].ToString();
                                ddlArea.UpdateAfterCallBack = true;
                            } break;
                        case "ddlHojaTrabajo":
                            if (ddlArea.SelectedValue != "0")
                            {
                                HabilitarHojaTrabajo();
                                ddlHojaTrabajo.SelectedValue = s_control[1].ToString();
                                ddlHojaTrabajo.UpdateAfterCallBack = true;
                            } break;
                    
                        case "txtFechaDesde": txtFechaDesde.Value = s_control[1].ToString(); break;
                        case "txtFechaHasta": txtFechaHasta.Value = s_control[1].ToString(); break;
                        case "txtProtocoloDesde": txtProtocoloDesde.Value = s_control[1].ToString(); break;
                        case "txtProtocoloHasta": txtProtocoloHasta.Value = s_control[1].ToString(); break;
                        case "ddlOrigen": ddlOrigen.SelectedValue = s_control[1].ToString(); break;
                        case "ddlEfector": ddlEfector.SelectedValue = s_control[1].ToString(); break;
                        //case "ddlPrioridad": ddlPrioridad.SelectedValue = s_control[1].ToString(); break;
                        
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

                    }
                }
            }

        }
     

       
    }
}
