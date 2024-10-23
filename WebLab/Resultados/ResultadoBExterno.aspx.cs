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
using NHibernate;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Drawing;
using Business.Data;

namespace WebLab.Resultados
{
    public partial class ResultadoBExterno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.txtProtocoloDesde.Focus();
                CargarPagina();
            }
        }

        private void CargarPagina()
        {
            imgAgregarArea.Visible = false;
            hplCambiarContrasenia.Visible = false;
            txtCodigo.Text = "";
            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            txtProtocoloDesde.Focus();
            
            if (Request["modo"].ToString() == "Urgencia") imgUrgencia.Visible = true;
            else imgUrgencia.Visible = false;

            if (Request["Operacion"].ToString() == "Carga")
            {
                //rdbValidaResultados.Visible = false;
                VerificaPermisos("Carga");
                CargarListas();
                lblTitulo.Text = "CARGA DE RESULTADOS";
                lblUsuarioValida.Visible = false;
             
                //HabilitarHojaTrabajo();
                IniciarValores();
                HabilitarCargaResultados();

            }
            if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Confirma"))
            {
                hplCambiarContrasenia.NavigateUrl = "";
                //////////////////Se controla quien es el usuario que está por validar////////////////
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                //if ((oCon.AutenticaValidacion)&&  (Request["idUsuarioValida"] == null))
                if ((oCon.AutenticaValidacion) && (Request["logIn"] == null)) Session["idUsuarioValida"] = null;
                if ((oCon.AutenticaValidacion) && (Session["idUsuarioValida"] == null))
                 
                    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString());
                if (Request["urgencia"]!=null)
                    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString() + "&urgencia=1&idProtocolo=" + Request["idProtocolo"].ToString() + "&Parametros=" + Request["idProtocolo"].ToString(), false);

                if (Session["idUsuarioValida"] == null)                
                    //Session["idUsuarioValida"] = Request["idUsuarioValida"];                                   
                //else
                    Session["idUsuarioValida"] = Session["idUsuario"];                                   

                Usuario oUserValida = new Usuario();
                oUserValida = (Usuario)oUserValida.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
                lblUsuarioValida.Text = "Usuario Autenticado para validar: " + oUserValida.Apellido + " " + oUserValida.Nombre;
                hplCambiarContrasenia.NavigateUrl = "../Usuarios/PasswordEdit.aspx?id=" + oUserValida.IdUsuario.ToString();
                hplCambiarContrasenia.Visible = true;
                divValidacion.Visible = true;
                //////////////////fin ///////////////////////////////////////////
                //if (Request["Operacion"].ToString() == "Confirma")
                //    Response.Redirect("ResultadoConfirmacion.aspx?idServicio=3&Operacion=Confirma&Modo=Normal");
                VerificaPermisos("Validacion Externo");
                CargarListas();

                lblTitulo.Text = "VALIDACION DE RESULTADOS";
                pnlTitulo.Attributes.Add("class", "panel panel-success");

                if (Request["idServicio"].ToString() == "6")
                {
                    pnlTitulo.Attributes.Add("class", "panel panel-success");
                    lnkBuscar.CssClass= "btn btn-success";
                }



                //rdbCargaResultados.Visible = false;

                //rdbValidaResultados.Visible = true;
                //rdbValidaResultados.Items[0].Selected = true;
                //lblFormaCarga.Visible = false;

                ddlHojaTrabajo.Visible = false;
                rvHojaTrabajo.Enabled = false;
         
                //ddlAnalisis.Visible = false;
                //lblAnalisis.Visible = false;
                //txtCodigo.Visible = false;
                //rvAnalisis.Visible = false;
                //lblMensaje.Visible = false;
                if (Request["modo"].ToString() != "Urgencia")
                IniciarValoresValidacion();

                HabilitarCargaResultados();
            }

            if (Request["Operacion"].ToString() == "Control")
            {
                //rdbValidaResultados.Visible = false;
                lblUsuarioValida.Visible = false;
                VerificaPermisos("Control");
                CargarListas();

                lblTitulo.Text = "CONTROL DE RESULTADOS";
                lblTitulo.ForeColor = Color.Green;
                rdbCargaResultados.Visible = false;
                lblFormaCarga.Visible = false;
                ddlHojaTrabajo.Visible = false;
                rvHojaTrabajo.Enabled = false;

                rdbEstado.Enabled = false;

                ddlAnalisis.Visible = false;
                lblAnalisis.Visible = false;
                txtCodigo.Visible = false;
                rvAnalisis.Visible = false;
                lblMensaje.Visible = false;

                IniciarValoresControl();
            }


            if (Request["Operacion"].ToString() == "HC")
            {
                //rdbValidaResultados.Visible = false;
                lblUsuarioValida.Visible = false;
                VerificaPermisos("Consulta");
                CargarListas();

                lblTitulo.Text = "CONSULTA DE RESULTADOS";
                
                rdbCargaResultados.Visible = false;
                lblFormaCarga.Visible = false;
                ddlHojaTrabajo.Visible = false;
                rvHojaTrabajo.Enabled = false;

                rdbEstado.Enabled = true;
                

                ddlAnalisis.Visible = false;
                lblAnalisis.Visible = false;
                txtCodigo.Visible = false;
                rvAnalisis.Visible = false;
                lblMensaje.Visible = false;
                IniciarValores();
                //IniciarValoresControl();
            }
            if (Request["modo"].ToString() == "Urgencia")
                ddlPrioridad.SelectedValue = "2";
            //if (Request["idServicio"].ToString() == "3")//microbiologia
            //    ddlPrioridad.SelectedValue = "1"; //rutina
             
               
        }

        private void MarcarSeleccionados(bool p)
        {
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = p;
            }
            
            

        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
         
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
         
        }
        private void IniciarValoresControl()
        {
            if (Session["Control"] != null)
            {
                string[] arr = Session["Control"].ToString().Split((";").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        //case "rdbCargaResultados":
                        //    {
                        //        rdbCargaResultados.SelectedValue = s_control[1].ToString();
                        //        HabilitarCargaResultados();
                        //        rdbCargaResultados.UpdateAfterCallBack = true;
                        //    } break;
                        case "ddlArea":
                            {
                                ddlArea.SelectedValue = s_control[1].ToString();
                                ddlArea.UpdateAfterCallBack = true;
                            } break;
                        //case "ddlHojaTrabajo":
                        //    if ((ddlArea.SelectedValue != "0") && (rdbCargaResultados.Items[1].Selected))
                        //    {
                        //        HabilitarHojaTrabajo();
                        //        ddlHojaTrabajo.SelectedValue = s_control[1].ToString();
                        //        ddlHojaTrabajo.UpdateAfterCallBack = true;
                        //    } break;
                        //case "ddlAnalisis":
                        //    {
                        //        if (rdbCargaResultados.SelectedValue == "2")
                        //        {
                        //            CargarAnalisis();
                        //        }
                        //        ddlAnalisis.SelectedValue = s_control[1].ToString();
                        //        MostrarCodigoAnalisis();
                        //    } break;
                        case "txtFechaDesde": txtFechaDesde.Value = s_control[1].ToString(); break;
                        case "txtFechaHasta": txtFechaHasta.Value = s_control[1].ToString(); break;
                        case "txtProtocoloDesde": txtProtocoloDesde.Value = s_control[1].ToString(); break;
                        case "txtProtocoloHasta": txtProtocoloHasta.Value = s_control[1].ToString(); break;
                        case "txtNroOrigen": txtNroOrigen.Text = s_control[1].ToString(); break;
                        case "ddlOrigen": ddlOrigen.SelectedValue = s_control[1].ToString(); break;
                        case "ddlEfector": ddlEfector.SelectedValue = s_control[1].ToString(); break;
                        //case "ddlPrioridad": ddlPrioridad.SelectedValue = s_control[1].ToString(); break;
                        case "rdbEstado": rdbEstado.SelectedValue = s_control[1].ToString(); break;
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
        private void IniciarValoresValidacion()
        {
            try
            {
                if (Session["Validacion"] != null)
                {
                    string[] arr = Session["Validacion"].ToString().Split((";").ToCharArray());
                    foreach (string item in arr)
                    {
                        string[] s_control = item.Split((":").ToCharArray());
                        switch (s_control[0].ToString())
                        {

                            case "rdbValidaResultados":
                                {
                                    //rdbValidaResultados.SelectedValue = s_control[1].ToString();
                                    HabilitarCargaResultados();
                                    rdbCargaResultados.UpdateAfterCallBack = true;
                                } break;
                            case "ddlArea":
                                {
                                    ddlArea.SelectedValue = s_control[1].ToString();
                                    ddlArea.UpdateAfterCallBack = true;
                                } break;
                            //case "ddlHojaTrabajo":
                            //    if ((ddlArea.SelectedValue != "0") && (rdbCargaResultados.Items[1].Selected))
                            //    {
                            //        HabilitarHojaTrabajo();
                            //        ddlHojaTrabajo.SelectedValue = s_control[1].ToString();
                            //        ddlHojaTrabajo.UpdateAfterCallBack = true;
                            //    } break;
                            case "ddlAnalisis":
                                {
                                    if (rdbCargaResultados.SelectedValue == "1")
                                    {
                                        CargarAnalisis();
                                    }
                                    ddlAnalisis.SelectedValue = s_control[1].ToString();
                                    MostrarCodigoAnalisis();
                                } break;
                            case "txtFechaDesde": txtFechaDesde.Value = s_control[1].ToString(); break;
                            case "txtFechaHasta": txtFechaHasta.Value = s_control[1].ToString(); break;
                            case "txtProtocoloDesde": txtProtocoloDesde.Value = s_control[1].ToString(); break;
                            case "txtProtocoloHasta": txtProtocoloHasta.Value = s_control[1].ToString(); break;
                            case "txtNroOrigen": txtNroOrigen.Text = s_control[1].ToString(); break;
                            case "ddlOrigen": ddlOrigen.SelectedValue = s_control[1].ToString(); break;
                            case "ddlEfector": ddlEfector.SelectedValue = s_control[1].ToString(); break;
                            //case "ddlPrioridad": ddlPrioridad.SelectedValue = s_control[1].ToString(); break;
                            case "rdbEstado": rdbEstado.SelectedValue = s_control[1].ToString(); break;
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
            catch { }
        }


        private void AlmacenarSesion()
        {
            string s_valores = "rdbCargaResultados:" + rdbCargaResultados.SelectedValue;
            s_valores += ";ddlArea:" + ddlArea.SelectedValue;
            s_valores += ";ddlHojaTrabajo:" + ddlHojaTrabajo.SelectedValue;
            s_valores += ";ddlAnalisis:" + ddlAnalisis.SelectedValue;
            s_valores += ";txtFechaDesde:" + txtFechaDesde.Value;
            s_valores += ";txtFechaHasta:" + txtFechaHasta.Value;            
            s_valores += ";txtProtocoloDesde:" + txtProtocoloDesde.Value;
            s_valores += ";txtProtocoloHasta:" + txtProtocoloHasta.Value;
            s_valores += ";ddlOrigen:" + ddlOrigen.SelectedValue;
            s_valores += ";ddlEfector:" + ddlEfector.SelectedValue;
            //s_valores += ";ddlPrioridad:" + ddlPrioridad.SelectedValue;
            s_valores += ";rdbEstado:" + rdbEstado.SelectedValue;
            s_valores += ";lstSector:" + getListaSectores(false);
            s_valores += ";txtNroOrigen:" + txtNroOrigen.Text;
            Session["Resultados"] = s_valores;
        }

        private void AlmacenarSesionValidacion()
        {

            string s_valores = "rdbValidaResultados:" + rdbCargaResultados.SelectedValue;
            s_valores += ";ddlArea:" + ddlArea.SelectedValue;
            //s_valores += ";ddlHojaTrabajo:" + ddlHojaTrabajo.SelectedValue;
            s_valores += ";ddlAnalisis:" + ddlAnalisis.SelectedValue;
            s_valores += ";txtFechaDesde:" + txtFechaDesde.Value;
            s_valores += ";txtFechaHasta:" + txtFechaHasta.Value;
            s_valores += ";txtProtocoloDesde:" + txtProtocoloDesde.Value;
            s_valores += ";txtProtocoloHasta:" + txtProtocoloHasta.Value;
            s_valores += ";ddlOrigen:" + ddlOrigen.SelectedValue;
            s_valores += ";ddlEfector:" + ddlEfector.SelectedValue;
            //s_valores += ";ddlPrioridad:" + ddlPrioridad.SelectedValue;
            s_valores += ";rdbEstado:" + rdbEstado.SelectedValue;
            s_valores += ";lstSector:" + getListaSectores(false);
            s_valores += ";txtNroOrigen:" + txtNroOrigen.Text;
            Session["Validacion"] = s_valores;
        }

        private void AlmacenarSesionControl()
        {
            //string s_valores = "rdbCargaResultados:" + rdbCargaResultados.SelectedValue;
            string s_valores = "ddlArea:" + ddlArea.SelectedValue;
            //s_valores += ";ddlHojaTrabajo:" + ddlHojaTrabajo.SelectedValue;
            //s_valores += ";ddlAnalisis:" + ddlAnalisis.SelectedValue;
            s_valores += ";txtFechaDesde:" + txtFechaDesde.Value;
            s_valores += ";txtFechaHasta:" + txtFechaHasta.Value;
            s_valores += ";txtProtocoloDesde:" + txtProtocoloDesde.Value;
            s_valores += ";txtProtocoloHasta:" + txtProtocoloHasta.Value;
            s_valores += ";ddlOrigen:" + ddlOrigen.SelectedValue;
            s_valores += ";ddlEfector:" + ddlEfector.SelectedValue;
              //s_valores += ";ddlPrioridad:" + ddlPrioridad.SelectedValue;
            s_valores += ";rdbEstado:" + rdbEstado.SelectedValue;
            s_valores += ";lstSector:" + getListaSectores(false);
            s_valores += ";txtNroOrigen:" + txtNroOrigen.Text;
            Session["Control"] = s_valores;
        }

        private void IniciarValores()
        {
            if (Session["Resultados"] != null)
            {
                string[] arr = Session["Resultados"].ToString().Split((";").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        case "rdbCargaResultados":
                            {
                                rdbCargaResultados.SelectedValue = s_control[1].ToString();
                                HabilitarCargaResultados();
                                rdbCargaResultados.UpdateAfterCallBack = true;
                            } break;
                        case "ddlArea":
                            {
                                ddlArea.SelectedValue = s_control[1].ToString();
                                ddlArea.UpdateAfterCallBack = true;
                            } break;
                        case "ddlHojaTrabajo":
                            if ((ddlArea.SelectedValue != "0") && (rdbCargaResultados.Items[1].Selected))
                            {
                                HabilitarHojaTrabajo();
                                ddlHojaTrabajo.SelectedValue = s_control[1].ToString();
                                ddlHojaTrabajo.UpdateAfterCallBack = true;
                            } break;
                        case "ddlAnalisis":
                            {
                                if (rdbCargaResultados.SelectedValue == "2")
                                {
                                    CargarAnalisis();
                                }
                                ddlAnalisis.SelectedValue = s_control[1].ToString();
                                MostrarCodigoAnalisis();                               
                            } break;
                        case "txtFechaDesde": txtFechaDesde.Value = s_control[1].ToString(); break;
                        case "txtFechaHasta": txtFechaHasta.Value = s_control[1].ToString(); break;
                        case "txtProtocoloDesde": txtProtocoloDesde.Value = s_control[1].ToString(); break;
                        case "txtProtocoloHasta": txtProtocoloHasta.Value = s_control[1].ToString(); break;
                        case "txtNroOrigen": txtNroOrigen.Text = s_control[1].ToString(); break;
                        case "ddlOrigen": ddlOrigen.SelectedValue = s_control[1].ToString(); break;
                        case "ddlEfector": ddlEfector.SelectedValue = s_control[1].ToString(); break;
                        //case "ddlPrioridad": ddlPrioridad.SelectedValue = s_control[1].ToString(); break;
                        case "rdbEstado": rdbEstado.SelectedValue = s_control[1].ToString(); break;
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

        private void VerificaPermisos(string sObjeto)
        {
            if (Request["Operacion"].ToString() == "Valida") { if (Session["idUsuarioValida"] == null) Response.Redirect("../FinSesion.aspx"); }
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    if (i_permiso==0)  Response.Redirect("../AccesoDenegado.aspx", false); 
                    
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
              else Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarListas()
        {
            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            Utility oUtil = new Utility();
            string s_idTipoServicio=Request["idServicio"].ToString();
            if (s_idTipoServicio == "0") s_idTipoServicio = "1";
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE idTipoServicio= " + s_idTipoServicio;
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            ddlServicio.Visible = false;
            lblServicio.Text = ddlServicio.SelectedItem.Text;

            ///Carga de Sectores          
            m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre  FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";            
            oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = true;
            }
            
            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("--Todos--", "0"));


            m_ssql = "SELECT idCaracter,  nombre as nombre  FROM LAB_Caracter ";
            oUtil.CargarCheckBox(chkCaracterCovid, m_ssql, "idCaracter", "nombre");
            for (int i = 0; i < chkCaracterCovid.Items.Count; i++)
            {
                chkCaracterCovid.Items[i].Selected = true;
            }
            ///Carga de combos de Prioridad
            m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad WHERE (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre");
            ddlPrioridad.Items.Insert(0, new ListItem("--Todos--", "0"));

            ///Marca la forma de carga por defecto 
            if (Request["Operacion"].ToString() != "Valida")
            {
                if (Request["modo"].ToString() == "Normal")
                {



                    switch (oC.TipoCargaResultado)
                    {
                        case 0:
                            {
                                rdbCargaResultados.Items[0].Selected = true;
                                rdbCargaResultados.Items[1].Selected = false;
                                rdbCargaResultados.Items[2].Selected = false;
                            }
                            break;
                        case 1:
                            {
                                rdbCargaResultados.Items[0].Selected = false;
                                rdbCargaResultados.Items[1].Selected = true;
                                rdbCargaResultados.Items[2].Selected = false;
                            }
                            break;
                        case 2:
                            {
                                rdbCargaResultados.Items[0].Selected = false;
                                rdbCargaResultados.Items[1].Selected = false;
                                rdbCargaResultados.Items[2].Selected = true;
                            }
                            break;
                    }
                }
            }
            else
            {
                rdbCargaResultados.Items[0].Selected = true;

                //if (oC.PreValida)
                //{
                //    lblModoPreValidacion.Text = "MODO PREVALIDACION ACTIVADO";
                //    lblModoPreValidacion.Visible = true;
                //}
            }
                

            if (Request["modo"].ToString() == "Urgencia")
            {
                rdbCargaResultados.Items[0].Selected = true;
                rdbCargaResultados.Items[1].Selected = false;
                rdbCargaResultados.Items[2].Selected = false; 
                ddlPrioridad.Visible = false;
                ddlPrioridad.SelectedValue="2";
                //lblPrioridad.Visible = false;
                
            }


            if ((s_idTipoServicio == "3")|| (s_idTipoServicio == "5"))//microbiologia
            {
                // La unica forma de carga es por protocolo
                rdbCargaResultados.Items[0].Selected = true;
                rdbCargaResultados.Items[1].Selected = false;
                rdbCargaResultados.Items[2].Selected = false;
             

                //////////El rango de fechas no es a fecha actual, sino los ultimos 30 dias
                txtFechaDesde.Value = DateTime.Now.AddDays(-2).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                

                if (s_idTipoServicio == "5")
                {
                    lblDniPaciente.Visible = false;
                    txtDNI.Visible = false;
                    //lblOrigen.Visible = false;
                    ddlOrigen.Visible = false;
                }
                 
            }

            ///Carga de combos de areas
            CargarArea(); HabilitarHojaTrabajo();
            //CargarAnalisis();


            ///Carga de Efectores solicitantes
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("--Todos--", "0"));
            //ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();
         

            m_ssql = null;
            oUtil = null;
        }
        private string getListaCaracter(bool v)
        {
            string m_lista = "";
            bool todasseleccionadas = true;
            for (int i = 0; i < chkCaracterCovid.Items.Count; i++)
            {
                if (chkCaracterCovid.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = chkCaracterCovid.Items[i].Value;
                    else
                        m_lista += "," + chkCaracterCovid.Items[i].Value;
                }
                else todasseleccionadas = false;

            }
            if ((todasseleccionadas)) m_lista = "";
            return m_lista;
        }
        private void CargarAnalisis()
        {
            Usuario oUser = new Usuario();

           
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
            if (rdbCargaResultados.Items[2].Selected)
            {
                imgAgregarArea.Visible = false;
                Utility oUtil = new Utility();
                string s_condicion = " I.baja=0 and I.idtiporesultado in (1,2,3,4) and A.idTipoServicio=" + ddlServicio.SelectedValue;

                if (ddlServicio.SelectedValue == "5")
                    s_condicion = " I.baja=0 and I.idtiporesultado in (1,2,3,4) and A.idTipoServicio=3";

                if (ddlArea.SelectedValue != "0")
                    s_condicion = s_condicion + " and I.idArea= " + ddlArea.SelectedValue;
                s_condicion = s_condicion + " and r.idEfectorDeriva = " + oUser.IdEfector.IdEfector.ToString();

                //if (esHemoterapia()) s_condicion = s_condicion + " and codigo like '433%'";
                ///Carga de combos de areas
                string m_ssql = " SELECT     I.idItem as idItem,  I.descripcion + ' ['+ I.codigo + ']'  as nombre " +
                         " FROM         LAB_Item AS I " +
                         " INNER JOIN lab_AREA A on A.idArea= I.idArea " +
                         " inner join LAB_ResultadoItem R on r.idItem= i.idItem "+
                         "  WHERE  " + s_condicion + " order by nombre ";

                oUtil.CargarCombo(ddlAnalisis, m_ssql, "idItem", "nombre");
                ddlAnalisis.Items.Insert(0, new ListItem("--Seleccione--", "0"));



                ddlAnalisis.UpdateAfterCallBack = true;
                rvArea.UpdateAfterCallBack = true;
                rvAnalisis.UpdateAfterCallBack = true;
                m_ssql = null;
                oUtil = null;
            }
        
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
            string m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue+" order by nombre"; 
            if (ddlServicio.SelectedValue=="5") //microbiologia de no pacientes
                m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=3 order by nombre";

            int b_idArea= esdeArea();
            if (b_idArea>0)
            {
                m_ssql = @" select idArea, nombre from Lab_Area where baja=0 and idarea="+b_idArea.ToString();
            }

             
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");

            //Configuracion oCon = (Configuracion)Session["Configuracion"];
            
          
            if (Request["Operacion"].ToString() == "Carga")
            {
                if (rdbCargaResultados.Items[1].Selected) /// por hoja de trabajo
                {
                    ddlArea.Items.Insert(0, new ListItem("--Seleccione Area--", "0")); 
                    rvArea.Enabled = true;
                  //  ddlPrioridad.SelectedValue = "1"; ///rutina           
                }
                else
                {
                    if (b_idArea == 0) ddlArea.Items.Insert(0, new ListItem("--Todas--", "0")); 

                    rvArea.Enabled = false;
                    if (rdbCargaResultados.Items[0].Selected) /// por hprotocolo
                    {
                        ddlPrioridad.SelectedValue = "0"; 

                        if (Request["modo"].ToString() == "Urgencia")
                        {
                            ddlPrioridad.SelectedValue = "2"; ///urgencia 
                            ddlPrioridad.Enabled = false;
                        }
                    }
                    //else
                    //{
                    //    ddlPrioridad.SelectedValue = "1"; ///todos
                        
                    //}                                     
                                                      
                }               

            }
            else
            { ///Validacion

                if (Request["modo"].ToString() == "Urgencia")
                {
                    ddlPrioridad.SelectedValue = "2";

                ddlPrioridad.Enabled = false;
                }
                //if (Request["modo"].ToString() == "Normal") ddlPrioridad.SelectedValue = "1"; ///rutina
                if (b_idArea==0)  ddlArea.Items.Insert(0, new ListItem("--Todas--", "0"));  
                rvArea.Enabled = false;              
            }

            ddlPrioridad.UpdateAfterCallBack = true;
            rvArea.UpdateAfterCallBack = true;
            m_ssql = null;
            oUtil = null;

          
           
        }

        //private bool esHemoterapia()
        //{
        //    Usuario oUser= new Usuario();

        //    if (Request["Operacion"].ToString() == "Valida")                                    
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
        //    else
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        //    return oUser.esHemoterapia();            

        //}

        private int esdeArea()
        {
            Usuario oUser = new Usuario();

            if (Request["Operacion"].ToString() == "Valida")
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
            else
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            return oUser.IdArea;

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
                m_parametro += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "' AND P.fecha<='"+ fecha2.ToString("yyyyMMdd") + "'" ;

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
                if (chkFactura.Checked) m_parametro += " AND P.nombreObraSocial not in (" + ConfigurationManager.AppSettings["NoFacturable"].ToString() + ")"; // solo las que tienen obra social

                if (txtNroOrigen.Text != "") m_parametro += " And P.numeroOrigen='" + txtNroOrigen.Text+ "'";
                string modoCarga = ""; string operacion = Request["Operacion"].ToString();

              //  if (ddlSectorServicio.SelectedValue != "0") m_parametro += " AND P.idSector = " + ddlSectorServicio.SelectedValue;

                string sectores = getListaSectores(true);
                if (sectores!="")
                    m_parametro += " AND P.idSector in (" + sectores + ")";
                string s_areas = "0";

                if (ddlArea.SelectedValue != "0")
                {
                   // m_parametro += " AND i.idArea in (" + ddlArea.SelectedValue +")";
                    s_areas = ddlArea.SelectedValue;

                }

                if ((ddlArea.SelectedValue != "0") && (ddlArea2.SelectedValue!="0") && (ddlArea2.Visible))
                {
                    //m_parametro += " AND i.idArea in (" + ddlArea.SelectedValue + "," + ddlArea2.SelectedValue + ")";
                    s_areas = s_areas+ "," + ddlArea2.SelectedValue;
                }

                string caracteres = getListaCaracter(true);
                if (caracteres != "")
                    m_parametro += " AND P.idCaracter in (" + caracteres + ")";
              

                if ((operacion == "Carga")||(operacion == "Valida"))
                {
                    switch (int.Parse(rdbCargaResultados.SelectedValue))
                    {/// por hoja de trabajo
                        case 0:   modoCarga = "LP"; break;
                        case 1:   modoCarga = "HT"; break;
                        case 2:   modoCarga = "AN"; break;
                    }
                }

                //if (modoCarga == "LP")
                //{ 
                //    if (s_areas != "0") m_parametro += " AND i.idArea in (" + s_areas + ")"; 
                //}

                //if (operacion == "Valida")
                //{
                //    switch (int.Parse(rdbCargaResultados.SelectedValue))
                //    {/// por hoja de trabajo
                //        case 0:
                //            modoCarga = "LP"; break;
                //        case 1:
                //            modoCarga = "HT"; break;
                //        case 2:
                //            modoCarga = "AN"; break;
                //    }
                //}

                    //switch (rdbEstado.SelectedValue)
                    //{
                    //    case "0": // pendientes de validar
                    //        {
                    //            m_parametro += " AND P.estado<=1 ";

                    //        if (ddlArea.SelectedValue != "0")
                    //                m_parametro += " AND DP.idUsuarioValida = 0 ";
                    //            if (modoCarga != "HT")
                    //                if (s_areas != "0")
                    //                m_parametro += " and DP.idsubitem in (select iditem from lab_item where idarea in (" + s_areas + "))";
                    //        }
                    //        break;
                    //    case "1": // validados
                    //        {
                    //            m_parametro += " AND P.estado>=2 ";
                    //            if (ddlArea.SelectedValue != "0")
                    //                m_parametro += " AND DP.idUsuarioValida > 0";
                    //            if (modoCarga != "HT")
                    //                if (s_areas != "0")
                    //                m_parametro += " and DP.idsubitem in (select iditem from lab_item where idarea in (" + s_areas + "))";
                    //        }
                    //        break;
                    //    case "2":
                    //        //m_parametro += " AND P.estado>=0 ";
                    //        if (modoCarga != "HT")
                    //            if (s_areas != "0")
                    //                m_parametro += " and DP.idsubitem in (select iditem from lab_item where idarea in (" + s_areas + "))";
                    //        break;
                    //}

                    //bool b_esHemoterapia = esHemoterapia();
                    //if (b_esHemoterapia) m_parametro += " and DP.idsubitem in (select iditem from lab_item where codigo like '433%')";
                
               
                
                if (operacion == "Control") { modoCarga = "LP"; }             
                if ((operacion == "Carga")||(operacion == "HC"))  { if (chkRecordarFiltro.Checked) AlmacenarSesion(); }
                if (operacion == "Valida") { if (chkRecordarFiltro.Checked) AlmacenarSesionValidacion(); }
                if (operacion == "Control"){ if (chkRecordarFiltro.Checked) AlmacenarSesionControl(); }
                               

                Response.Redirect("ProcesaExterno.aspx?idServicio="+ Request["idServicio"].ToString()+"&ModoCarga=" + modoCarga + "&Operacion=" + operacion + "&Parametros=" + m_parametro + "&idArea=" + s_areas + "&idHojaTrabajo=" + ddlHojaTrabajo.SelectedValue+ "&idItem=" + ddlAnalisis.SelectedValue+"&validado=0&modo="+Request["modo"]+"&dni="+ txtDNI.Value +"&estado="+ rdbEstado.SelectedValue, false);
            }
           
        }

       

    

        private string getListaSectores(bool filtro)
        {
            string m_lista = "";
            bool todasseleccionadas=true;
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                if (lstSector.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstSector.Items[i].Value;
                    else
                        m_lista += "," + lstSector.Items[i].Value;
                }
                else todasseleccionadas = false;

            }
            if ((filtro)&&(todasseleccionadas)) m_lista = "";
            return m_lista;
        }

        protected void rdbCargaResultados_SelectedIndexChanged(object sender, EventArgs e)
        { 
            //CargarArea();
            HabilitarCargaResultados();
            if (int.Parse(rdbCargaResultados.SelectedValue) == 1) { CargarHojaTrabajo();
            ddlHojaTrabajo.Visible = true;
            ddlHojaTrabajo.UpdateAfterCallBack = true;
            rvHojaTrabajo.UpdateAfterCallBack = true;
            }
        }

        protected void rdbValidaResultados_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarArea();
            HabilitarCargaResultados();
        }
        private void HabilitarCargaResultados()
        {
            ddlArea2.Visible = false; imgAgregarArea.Visible = false;
            rdbCargaResultados.SelectedValue = "2";

            if ((Request["Operacion"].ToString() == "Carga") ||(Request["Operacion"].ToString() == "Valida"))
            {
                switch (int.Parse(rdbCargaResultados.SelectedValue))
                {
                    case 0:  /// por protocolo
                        {
                            //  ddlArea.Enabled = false;
                            ddlHojaTrabajo.Items.Clear();
                            ddlHojaTrabajo.Visible = false;
                            ddlHojaTrabajo.UpdateAfterCallBack = true;
                            rvArea.Enabled = false;

                            ddlAnalisis.Enabled = false;
                            txtCodigo.Enabled = false;
                            rvAnalisis.Enabled = false;
                            if (ddlArea.SelectedValue != "0") HabilitarSegundaArea();
                            //ddlPrioridad.SelectedValue = "2"; ///urgencia
                        }
                        break;

                    case 1: // por hoja de trabajo
                        {
                            //HabilitarHojaTrabajo();
                            ddlArea.Enabled = true;
                            rvArea.Enabled = true;

                            txtCodigo.Text = "";

                            ddlAnalisis.Enabled = false;
                            txtCodigo.Enabled = false;
                            rvAnalisis.Enabled = false;
                            //ddlPrioridad.SelectedValue = "1"; ///prioridad

                        }
                        break;

                    case 2: // por analisis
                        {

                            ddlHojaTrabajo.Items.Clear();
                            ddlHojaTrabajo.Visible = false;
                            ddlHojaTrabajo.UpdateAfterCallBack = true;

                            ddlAnalisis.Enabled = true;

                            txtCodigo.Enabled = true;

                            rvAnalisis.Enabled = true;
//                            CargarArea();
                            CargarAnalisis();
                        }
                        break;
                }
            }

            //if (Request["Operacion"].ToString() == "Valida")
            //{
            //    switch (int.Parse(rdbValidaResultados.SelectedValue))
            //    {
            //        case 0:  /// por protocolo
            //            {
            //                //  ddlArea.Enabled = false;
            //                ddlHojaTrabajo.Items.Clear();
            //                ddlHojaTrabajo.Visible = false;
            //                ddlHojaTrabajo.UpdateAfterCallBack = true;
            //                rvArea.Enabled = false;

            //                ddlAnalisis.Enabled = false;
            //                txtCodigo.Enabled = false;
            //                rvAnalisis.Enabled = false;
            //                //ddlPrioridad.SelectedValue = "2"; ///urgencia
            //            }
            //            break;         

            //        case 2: // por analisis
            //            {

            //                ddlHojaTrabajo.Items.Clear();
            //                ddlHojaTrabajo.Visible = false;
            //                ddlHojaTrabajo.UpdateAfterCallBack = true;

            //                ddlAnalisis.Enabled = true;

            //                txtCodigo.Enabled = true;

            //                rvAnalisis.Enabled = true;
            //            }
            //            break;
            //    }
            //}

            //ddlPrioridad.UpdateAfterCallBack = true;
            ddlArea.UpdateAfterCallBack = true;
            rvArea.UpdateAfterCallBack = true;
            ddlAnalisis.UpdateAfterCallBack = true;
            rvAnalisis.UpdateAfterCallBack = true;
            txtCodigo.UpdateAfterCallBack = true;
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((Request["Operacion"].ToString() == "Carga")||(Request["Operacion"].ToString() == "Valida"))
                switch (int.Parse(rdbCargaResultados.SelectedValue))
                {
                    case 1:
                        HabilitarHojaTrabajo(); break;
                    case 2:
                        {
                            CargarAnalisis();
                         
                        } break;
                    default: { 
                        
                       
                        HabilitarSegundaArea(); } break;
                }
        }

        private void HabilitarAnalisis()
        {
            throw new NotImplementedException();
        }

        private void HabilitarHojaTrabajo()
        {
            if ((ddlArea.SelectedValue != "0") &&(rdbCargaResultados.Items[1].Selected))
            {
                ddlArea2.Visible = false;
                imgAgregarArea.Visible = false;
                CargarHojaTrabajo();
                ddlHojaTrabajo.Enabled = true;
                ddlHojaTrabajo.Visible = true;
                rvHojaTrabajo.Enabled = true;
               

            }
            else
            {
                ddlHojaTrabajo.Items.Insert(0, new ListItem("--Seleccione--", "0"));     
                
            }
            if ((rdbCargaResultados.Items[0].Selected) || (rdbCargaResultados.Items[2].Selected)) //por lista de protocolos o por analisis
            {
                
                ddlHojaTrabajo.Visible = false;
                rvHojaTrabajo.Enabled = false;
            }


            
            ddlHojaTrabajo.UpdateAfterCallBack = true;
            rvHojaTrabajo.UpdateAfterCallBack = true;
        }

        private void CargarHojaTrabajo()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "select idHojaTrabajo, codigo from Lab_HojaTrabajo where baja=0  and idArea=" +ddlArea.SelectedValue + " order by codigo ";
//            if (esHemoterapia())
//                m_ssql = @"select distinct h.idHojaTrabajo, codigo from Lab_HojaTrabajo  as H 
//inner join LAB_DetalleHojaTrabajo as DH on H.idHojaTrabajo= DH.idHojaTrabajo
//where baja=0  and idArea=" +ddlArea.SelectedValue + "  and  iditem in (select iditem from LAB_Item where codigo like '433%') order by codigo ";


            oUtil.CargarCombo(ddlHojaTrabajo, m_ssql, "idHojaTrabajo", "codigo");
            if (ddlHojaTrabajo.Items.Count == 0)
            { ddlHojaTrabajo.Items.Insert(0, new ListItem("", "0")); }

        }

        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {  try{
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
            {
               args.IsValid = true;
                DateTime f1 = DateTime.Parse(txtFechaDesde.Value);
                if (txtFechaHasta.Value == "")
                    args.IsValid = false;
                else
                {
                    DateTime f2 = DateTime.Parse(txtFechaHasta.Value);
                    args.IsValid = true; }
            }
        }
        catch (Exception ex)
        {
            string exception = "";
            while (ex != null)
            {
                exception = ex.Message + "<br>";

            } args.IsValid = false;
        }

        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            BuscarAnalisisPorCodigo();
        }

        private void BuscarAnalisisPorCodigo()
        {
            if (txtCodigo.Text != "")
            {
                Item oItem = new Item();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                Area oArea = new Area();
                ICriteria crit = m_session.CreateCriteria(typeof(Item));

                crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
                crit.Add(Expression.Eq("Baja", false));
                crit.Add(Expression.Eq("IdCategoria", 0));
                if (ddlArea.SelectedValue!="0")
                  crit.Add(Expression.Eq("IdArea", (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue))));

                oItem = (Item)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlAnalisis.SelectedValue = oItem.IdItem.ToString();
                    lblMensaje.Text = "";                    

                }
                else
                {
                    lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no es valido. ";
                    //ddlAnalisis.SelectedValue = "0";
                    txtCodigo.Text = "";
                    rvAnalisis.Enabled = true;


                }

            
                
            }
            else
            {
                ddlAnalisis.SelectedValue = "0";
            
            }
            txtCodigo.UpdateAfterCallBack = true;
            lblMensaje.UpdateAfterCallBack = true;
            ddlAnalisis.UpdateAfterCallBack = true;
                rvAnalisis.UpdateAfterCallBack = true;
        }

        protected void ddlAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarCodigoAnalisis();
        }

        private void MostrarCodigoAnalisis()
        {
            if (ddlAnalisis.SelectedValue != "0")
            {
                if (ddlAnalisis.SelectedValue != "")
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlAnalisis.SelectedValue));
                    txtCodigo.Text = oItem.Codigo;
                }

            }
            else
            {
                txtCodigo.Text = "";

            }
            //txtNombre.UpdateAfterCallBack = true;
            txtCodigo.UpdateAfterCallBack = true;
            //txtNombreAnalisis.UpdateAfterCallBack = true;
        }

        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            Session["Validacion"] = null;
            Session["Resultados"] = null;

            CargarPagina();

        }

       
        protected void imgAgregarArea_Click(object sender, EventArgs e)
        {
       
        }

        private void HabilitarSegundaArea()
        {
            if (rdbCargaResultados.Items[0].Selected)
            {
                if (ddlArea.SelectedValue != "0")
                {
                    if (!ddlArea2.Visible)
                    {
                        ddlArea2.Visible = true;
                        imgAgregarArea.Visible = true;


                        ddlArea2.Items.Clear();
                        Utility oUtil = new Utility();
                        ///Carga de combos de areas
                        string m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue +
                            " and idArea <>" + ddlArea.SelectedValue + "  order by nombre";
                        if (ddlServicio.SelectedValue=="5")
                            m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=3 and idArea <>" + ddlArea.SelectedValue + "  order by nombre";


                        //if (esHemoterapia()) m_ssql = @" select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue +
                        //    " and idArea <>" + ddlArea.SelectedValue + " and idarea in (select idarea from LAB_Item where codigo like '433%')";

                        oUtil.CargarCombo(ddlArea2, m_ssql, "idArea", "nombre");
                        ddlArea2.Items.Insert(0, new ListItem("--Seleccione Area Adicional--", "0"));

                        if (esdeArea() > 0)
                            ddlArea2.Enabled = false;
                        else
                            ddlArea2.Enabled = true;

                        ddlArea2.UpdateAfterCallBack = true;
                        imgAgregarArea.UpdateAfterCallBack = true;


                    }
                }
            }

        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Utility oUtil = new Utility();
           
            try
            {
                if (txtProtocoloDesde.Value != "") 
                {
                    int n1= int.Parse(txtProtocoloDesde.Value);
                    args.IsValid = true; 
                }
                else                         
                    args.IsValid = true;
            }
            catch (Exception ex)
            {
                string exception = "";
                while (ex != null)
                {
                    exception = ex.Message + "<br>";

                } 
                args.IsValid = false; 
            }

       
        }

        protected void cvNumeroHasta_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Utility oUtil = new Utility();

            //if (txtProtocoloHasta.Value != "") { if (oUtil.EsEntero(txtProtocoloHasta.Value)) args.IsValid = true; else args.IsValid = false; }
            //else                         
            //    args.IsValid = true;            



            try
            {
                if (txtProtocoloHasta.Value != "")
                {
                    int n1 = int.Parse(txtProtocoloHasta.Value);
                    args.IsValid = true;
                }
                else
                    args.IsValid = true;
            }
            catch (Exception ex)
            {
                string exception = "";
                while (ex != null)
                {
                    exception = ex.Message + "<br>";

                }
                args.IsValid = false;
            }
        }
          
       
       

        }
   
}
