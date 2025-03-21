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

namespace WebLab.AutoAnalizador
{
    public partial class ProtocoloBusqueda2 : System.Web.UI.Page
    {
       public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

          

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                switch (Request["Equipo"].ToString())
                {
                    case "SysmexXS1000":
                        {
                            VerificaPermisos("Sysmex XS1000i");
                            lblTituloEquipo.Text = "Sysmex XS1000i".ToUpper();
                        } break;
                    case "SysmexXT1800":
                        {
                            VerificaPermisos("Sysmex XT1800i");
                            lblTituloEquipo.Text = "Sysmex XT1800i".ToUpper();
                        } break;
                    case "Mindray":
                        {
                            VerificaPermisos("Mindray BS-300 Envio de datos");
                            lblTituloEquipo.Text = "Mindray BS-300".ToUpper();
                        } break;

                    case "Stago":
                        {
                            VerificaPermisos("STA Compact");
                            lblTituloEquipo.Text = "STA Compact".ToUpper();
                        } break;

                    case "Metrolab":
                        {
                            VerificaPermisos("Metrolab - Envío de datos");
                            lblTituloEquipo.Text = "METROLAB".ToUpper();
                        } break;

                    case "Miura":
                        {
                            VerificaPermisos("Miura");
                            lblTituloEquipo.Text = "MIURA QUIMICA".ToUpper();
                        } break;

                    case "Incca":
                        {
                            VerificaPermisos("Incca - Envío de datos");
                            lblTituloEquipo.Text = "INCAA - Diconex QUIMICA".ToUpper();
                        }
                        break;
                    case "CobasC311":
                        {
                            VerificaPermisos("Cobas C311 - Envío de datos");
                            lblTituloEquipo.Text = "Cobas C311".ToUpper();
                        }
                        break;
                }
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

        private void CargarPagina()
        {
            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            CargarListas();

            //if ( (Request["Equipo"].ToString() == "SysmexXS1000") || (Request["Equipo"].ToString() == "SysmexXT1800"))
            if (Request["Equipo"].ToString() != "Mindray")
            {
                imgEquipo.ImageUrl = "../App_Themes/default/images/sysmex.jpg";
                pnlMindray.Visible = false;
            }
            if (Request["Equipo"].ToString() == "Mindray")
            {
                pnlMindray.Visible = true;
                imgEquipo.ImageUrl = "../App_Themes/default/images/mindray.jpg";
            }

            if (Request["Equipo"].ToString() == "Metrolab")
            {
                pnlMindray.Visible = true; imgEquipo.Visible = false;
                //imgEquipo.ImageUrl = "../App_Themes/default/images/mindray.jpg";
            }

            if (Request["Equipo"].ToString() == "Miura")
            {
                pnlMindray.Visible = true; imgEquipo.Visible = false;
                //imgEquipo.ImageUrl = "../App_Themes/default/images/mindray.jpg";
            }
            if (Request["Equipo"].ToString() == "CobasC311")
            {
                pnlMindray.Visible = true; imgEquipo.Visible = false;
            }
            //   txtCantidad.Value = "50";

        }

        

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            ///Carga de Sectores          
            string m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre  FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = true;
            }

            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre", connReady);
            ddlOrigen.Items.Insert(0, new ListItem("Todos", "0"));

            ///Carga de combos de Prioridad
            m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre",connReady);
            ddlPrioridad.Items.Insert(0, new ListItem("Todos", "0"));        


            ///Carga de Efectores solicitantes
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector with (nolock) order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));
            //ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();

            if (Request["Equipo"].ToString() == "Miura")
            {
                m_ssql = @"SELECT DISTINCT LAB_MiuraItem.prefijo FROM LAB_MiuraItem   with (nolock)                       
                         WHERE (LAB_MiuraItem.prefijo <> '')";
                oUtil.CargarCombo(ddlPrefijo, m_ssql, "prefijo", "prefijo", connReady);
                ddlTipoMuestra.Enabled = false;
            }

            if (Request["Equipo"].ToString() == "Metrolab")
            {
                m_ssql = @"SELECT DISTINCT LAB_MetrolabItem.prefijo FROM LAB_MetrolabItem    with (nolock)                      
                         WHERE (LAB_MetrolabItem.prefijo <> '')";
                oUtil.CargarCombo(ddlPrefijo, m_ssql, "prefijo", "prefijo", connReady);
                

                m_ssql = @"select distinct M.idMuestra, M.nombre  
                            from lab_item i  with (nolock)
                            inner join LAB_Muestra as M  with (nolock) on M.idMuestra=  i.idMuestra
                            inner join LAB_ItemEfector Ie  with (nolock) on  I.iditem=  ie.idItem 
                            inner join LAB_MetrolabItem Me  with (nolock) on Me.idItem= I.idItem
                            where  ie.idEfector="+oUser.IdEfector.IdEfector.ToString() +@"
                            and ie.disponible=1
                            and ie.sininsumo=0    
                            and i.idMuestra>0
                            and i.baja=0";
                oUtil.CargarCombo(ddlTipoMuestra, m_ssql, "idMuestra", "nombre", connReady);
                ddlTipoMuestra.Items.Insert(0, new ListItem("Todas", "0"));
                ddlTipoMuestra.Enabled = true;
              
            }

            if (Request["Equipo"].ToString() == "CobasC311")
            {

                 
                    m_ssql = @"SELECT DISTINCT LAB_CobasC311.prefijo FROM LAB_CobasC311  with (nolock)
                       WHERE (LAB_CobasC311.prefijo <> '') and habilitado=1";
                    oUtil.CargarCombo(ddlPrefijo, m_ssql, "prefijo", "prefijo");
                    ddlTipoMuestra.Enabled = false;
 

                m_ssql = @"select distinct tipoMuestra  from LAB_CobasC311 with (nolock)
where habilitado=1
order by tipoMuestra";
                oUtil.CargarCombo(ddlTipoMuestra, m_ssql, "tipoMuestra", "tipoMuestra", connReady);
                ddlTipoMuestra.Items.Insert(0, new ListItem("Todas", "0"));
                ddlTipoMuestra.Enabled = true;

            }

            if (Request["Equipo"].ToString() == "Mindray")
            {
                m_ssql = @"SELECT DISTINCT LAB_MindrayItem.prefijo, LAB_Item.nombre FROM LAB_MindrayItem  with (nolock)
                        INNER JOIN LAB_Item  with (nolock) ON LAB_MindrayItem.idItem = LAB_Item.idItem
                        WHERE (LAB_MindrayItem.prefijo <> '')";
                oUtil.CargarCombo(ddlPrefijo, m_ssql, "prefijo", "prefijo", connReady);
            }
              
            if (Request["Equipo"].ToString() == "Incca")
            {
                m_ssql = @"SELECT DISTINCT LAB_InccaItem.prefijo FROM LAB_InccaItem  with (nolock)                        
                         WHERE (LAB_InccaItem.prefijo <> '')";
                oUtil.CargarCombo(ddlPrefijo, m_ssql, "prefijo", "prefijo", connReady);
                ddlTipoMuestra.Enabled = false;

                m_ssql = @"select distinct M.idMuestra, M.nombre  
                            from lab_item i  with (nolock)
                            inner join LAB_Muestra as M  with (nolock) on M.idMuestra=  i.idMuestra
                            inner join LAB_ItemEfector Ie  with (nolock) on  I.iditem=  ie.idItem 
                            inner join LAB_InccaItem Me  with (nolock) on Me.idItem= I.idItem
                            where  ie.idEfector=" + oUser.IdEfector.IdEfector.ToString() + @"
                            and ie.disponible=1
                            and ie.sininsumo=0
                            and i.idMuestra>0
                            and i.baja=0";
                oUtil.CargarCombo(ddlTipoMuestra, m_ssql, "idMuestra", "nombre", connReady);
                ddlTipoMuestra.Items.Insert(0, new ListItem("Todas", "0"));
                ddlTipoMuestra.Enabled = true;
            }
            ddlPrefijo.Items.Insert(0, new ListItem("Rutina", "0"));
            m_ssql = null;
            oUtil = null;
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

                if (oC != null)
                {
                    string m_parametro = " P.baja=0 and P.idEfector=" + oC.IdEfector.IdEfector.ToString() + " and P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "' AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";

                    //if (ddlArea.SelectedValue != "0") m_parametro += " AND i.idArea=" + ddlArea.SelectedValue;
                    if (txtProtocoloDesde.Value != "") m_parametro += " And P.numero>=" + int.Parse(txtProtocoloDesde.Value);
                    if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numero<=" + int.Parse(txtProtocoloHasta.Value);

               /*     switch (oC.TipoNumeracionProtocolo)// busqueda con autonumerico
                    {
                        case 0:
                            {
                                if (txtProtocoloDesde.Value != "") m_parametro += " And P.numero>=" + int.Parse(txtProtocoloDesde.Value);
                                if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numero<=" + int.Parse(txtProtocoloHasta.Value);
                            }
                            break;
                        case 1:
                            {
                                if (txtProtocoloDesde.Value != "") m_parametro += " And P.numeroDiario>=" + int.Parse(txtProtocoloDesde.Value);
                                if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numeroDiario<=" + int.Parse(txtProtocoloHasta.Value);
                            }
                            break;
                        case 2:
                            {
                                if (txtProtocoloDesde.Value != "") m_parametro += " And P.numeroSector>=" + int.Parse(txtProtocoloDesde.Value);
                                if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numeroSector<=" + int.Parse(txtProtocoloHasta.Value);
                            }
                            break;
                        case 3:
                            {
                                if (txtProtocoloDesde.Value != "") m_parametro += " And P.numeroTipoServicio>=" + int.Parse(txtProtocoloDesde.Value);
                                if (txtProtocoloHasta.Value != "") m_parametro += " AND  P.numeroTipoServicio<=" + int.Parse(txtProtocoloHasta.Value);
                            }
                            break;
                    }*/


                    if (ddlEfector.SelectedValue != "0") m_parametro += " AND P.idEfectorSolicitante=" + ddlEfector.SelectedValue;
                    if (ddlOrigen.SelectedValue != "0") m_parametro += " AND P.idOrigen=" + ddlOrigen.SelectedValue;
                    if (ddlPrioridad.SelectedValue != "0") m_parametro += " AND P.idPrioridad=" + ddlPrioridad.SelectedValue;



                    m_parametro += " AND P.idSector in (" + getListaSectores() + ")";
                    //switch (rdbEstado.SelectedValue)
                    //{
                    //    case "0": // pendientes de validar
                    //        m_parametro += " AND DP.enviado=0 "; break;
                    //    case "1": // validados
                    //        m_parametro += " AND DP.enviado=1 "; break;
                    //    //case "2":
                    //    //    m_parametro += " AND DP.estado=0 "; break;
                    //}



                    string s_limpiaTemporal = "0";
                    if (chkLimpiarTemporal.Checked) s_limpiaTemporal = "1";
                    Response.Redirect("ProtocoloEnviar2.aspx?Equipo=" + Request["Equipo"].ToString() + "&Parametros=" + m_parametro + "&tipoMuestra=" + ddlTipoMuestra.SelectedValue + "&IDMuestra=" + txtIDMuestra.Value + "&Prefijo=" + ddlPrefijo.SelectedItem.Text + "&estado=" + rdbEstado.SelectedValue + "&LimpiaTemporal=" + s_limpiaTemporal, false);
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
        

        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
            {
                args.IsValid = false;
                cvFechas.Text = "Debe ingresar fecha de inicio";
            }
            else
            {
                if (txtFechaHasta.Value == "")
                {
                    args.IsValid = false;
                    cvFechas.Text = "Debe ingresar fecha de fin";
                }
                else
                {                    
                        if (txtIDMuestra.Value == "")
                        {
                            args.IsValid = false;
                            cvFechas.Text = "Debe ingresar el ID de muestra";
                        }
                        else
                            args.IsValid = true;
                   
                }
            }

        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            //if (Request["Equipo"].ToString() == "SysmexXS1000")
            //    Response.Redirect("PrincipalSysmex.aspx", false);
            //if (Request["Equipo"].ToString() == "Mindray")
            //    Response.Redirect("PrincipalMindray.aspx", false);
        }

        protected void lnKConfiguracion_Click(object sender, EventArgs e)
        {
            //if (Request["Equipo"].ToString() == "SysmexXS1000")
            //    Response.Redirect("SysmexXS1000/ConfiguracionEdit.aspx", false);
            //if (Request["Equipo"].ToString() == "Mindray")                
            //    Response.Redirect("MindrayBS300/ConfiguracionEdit.aspx", false);
        }

   



    }

}
