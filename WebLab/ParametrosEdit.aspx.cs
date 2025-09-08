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
using Business.Data.Laboratorio;
using NHibernate;
using Business;
using NHibernate.Expression;
using System.Drawing;
using System.Data.SqlClient;
using Business.Data;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.Web.Script.Serialization;

namespace WebLab
{
    public partial class ParametrosEdit : System.Web.UI.Page
    {
        private enum TabIndex
        {  
            DEFAULT = 0,    //General
            ONE = 1,        //Protocolos
            TWO = 2,        //Turnos
            THREE = 3,      //Calendario
            CUARTO = 4,     //Carga/Validación Resultados
            QUINTO = 5,     //Laboratorio General
            SIX=6 ,         //Microbiología
            SEVEN=7,         //Impresoras  
                   OCHO = 8    ,                   //ImAGENN DE IMPRESION
                   NUEVE=9 // CODIGO DE BARRAS
        }
        private void SetSelectedTab(TabIndex tabIndex)
        {
            HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        }
        public CrystalReportSource oCr = new CrystalReportSource();
        Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Session["idUsuario"] != null)
             
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
              
            else Response.Redirect("../FinSesion.aspx", false);
          
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {               
                VerificaPermisos("Parametros SI");                              
                CargarListas();
                MostrarDatos();
                HabilitarDatosModificables();
                         
            }

        }

        private void HabilitarDatosModificables()
        {
            ddlRenaper.Enabled = false;
            txtUrlRenaper.Enabled = false;
            ddlAndes.Enabled = false;
            txtUrlAndes.Enabled = false;
            ddlMedicoObligatorio.Enabled = false;
            txtUrlMatriculacion.Enabled = false;
            ddlDiagnostico.Enabled = false;
            ddlSectorDefecto.Enabled = false;
            ddlSectorUrgencia.Enabled = false;
            chkOrigen.Enabled = false;
            ddlTipoHojaImpresionResultados.Enabled = false;
            ddlTipoHojaImpresionResultadosMicrobiologia.Enabled = false;
            //tab7.Visible = false; //panel de impresoras
          //  tabImpresora.Enabled = false;
            tabCovid.Enabled = false;
            //tab13.Visible = false; ///panel de covid
            //panel sisa
            ddlNotificarSISA.Enabled = false;
            txtUrlServicioSISA.Enabled = false;
            txtUrlMuestraSISA.Enabled = false;
            txtUrlResultadoSISA.Enabled = false;

            if (oUser.IdEfector.IdEfector.ToString() == "227")
            {
                ddlRenaper.Enabled = true;
                txtUrlRenaper.Enabled = true;
                ddlAndes.Enabled = true;
                txtUrlAndes.Enabled = true;
                ddlMedicoObligatorio.Enabled = true;
                txtUrlMatriculacion.Enabled = true;
                ddlDiagnostico.Enabled = true;
                ddlSectorDefecto.Enabled = true;
                ddlSectorUrgencia.Enabled = true;
                chkOrigen.Enabled = true;
                ddlTipoHojaImpresionResultados.Enabled = true;
                ddlTipoHojaImpresionResultadosMicrobiologia.Enabled = true;
                //tab7.Visible = true; //panel de impresoras
                //tabImpresora.Enabled = true;
                tabCovid.Enabled = true;
             //   tab13.Visible = true; ///panel de covid
                //panel sisa
                ddlNotificarSISA.Enabled = true;
                txtUrlServicioSISA.Enabled = true;
                txtUrlMuestraSISA.Enabled = true;
                txtUrlResultadoSISA.Enabled = true;

                lblRefes.Enabled = true;
              
                ddlRegion.Enabled = true;
                lblIdEfector2.Enabled = true;
                lblJefe.Enabled = true;
                lblCorreoJefe.Enabled = true;

            }
        }

        //protected void btnGuardarImpresora_Click(object sender, EventArgs e)
        //{
        //    GuardarImpresoras();
        //    SetSelectedTab(TabIndex.SEVEN);

        //}

        //private void GuardarImpresoras()
        //{
        //    //if (lstImpresora.Items.Count > 0)
        //    //{
        //        ////borra las impresoras guardadas
        //        ISession m_session = NHibernateHttpModule.CurrentSession;
        //        ICriteria crit2 = m_session.CreateCriteria(typeof(Impresora));
        //            crit2.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));
        //    IList listaImpresoras = crit2.List();
        //        foreach (Impresora oImpr in listaImpresoras)
        //        {
        //            oImpr.Delete();
        //        }

        //        /////Crea nuevamente los detalles.
        //        for (int i = 0; i < lstImpresora.Items.Count; i++)
        //        {
        //            Impresora oRegistro = new Impresora();
        //        oRegistro.IdEfector = oUser.IdEfector.IdEfector;
        //            oRegistro.Nombre = lstImpresora.Items[i].Value;
        //            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
        //            oRegistro.FechaRegistro = DateTime.Now;
        //            oRegistro.Save();
        //        }
        //    //}
        //}
        //protected void btnAgregarImpresora_Click(object sender, EventArgs e)
        //{
        //    if (!VerificarExisteImpresoras())
        //    {
        //        if (ddlImpresora.SelectedValue != "0")
        //        {
        //            ListItem oImpresora = new ListItem();
        //            oImpresora.Text = ddlImpresora.SelectedValue;
        //            oImpresora.Value = ddlImpresora.SelectedValue;
        //            lstImpresora.Items.Add(oImpresora);
        //        }

        //        if (txtNuevaImpresora.Text!="")
        //        {
        //            ListItem oImpresora2 = new ListItem();
        //            oImpresora2.Text = txtNuevaImpresora.Text;
        //            oImpresora2.Value = txtNuevaImpresora.Text;
        //            lstImpresora.Items.Add(oImpresora2);

        //        }
        //    }
        //    lstImpresora.UpdateAfterCallBack = true;
        //    SetSelectedTab(TabIndex.SEVEN);
        //}

        //protected void btnSacarImpresora_Click(object sender, EventArgs e)
        //{
        //    if (lstImpresora.SelectedValue != "")
        //    {
        //        lstImpresora.Items.Remove(lstImpresora.SelectedItem);               
        //    }


        //    lstImpresora.UpdateAfterCallBack = true;
        //    SetSelectedTab(TabIndex.SEVEN);
        //}
        //private bool VerificarExisteImpresoras()
        //{
        //    bool existe = false;
        //    if (lstImpresora.Items.Count > 0)
        //    {
        //        /////Crea nuevamente los detalles.
        //        for (int i = 0; i < lstImpresora.Items.Count; i++)
        //        {
        //            if (ddlImpresora.SelectedValue == lstImpresora.Items[i].Value)
        //            {
        //                existe = true; break;
        //            }

        //            if (txtNuevaImpresora.Text==                        lstImpresora.Items[i].Value)
        //            {
        //            existe = true; break;
        //        }
        //    }
        //    }
        //    return existe;
        //}

        //private void MostrarImpresoras()
        //{
        //    ISession m_session = NHibernateHttpModule.CurrentSession;

        //    Impresora oRegistro = new Impresora();
        //    ICriteria crit2 = m_session.CreateCriteria(typeof(Impresora));
        //    crit2.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));
        //    IList listaImpresoras = crit2.List();

        //    foreach (Impresora oImpr in listaImpresoras)
        //    {
        //        ListItem oItem = new ListItem();
        //        oItem.Text = oImpr.Nombre;
        //        oItem.Value = oImpr.Nombre;
        //        lstImpresora.Items.Add(oItem);
        //    }
        //}

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("FinSesion.aspx", false); 
            }
            else
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("AccesoDenegado.aspx", false); break;
                        case 1: btnGuardar.Visible = false; break;
                    }
                }
                else Response.Redirect("FinSesion.aspx", false);
            }
        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            //            btnReinializacion.Visible = false;
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de Sectores
            string m_ssql = " SELECT idSectorServicio,  nombre  + ' - ' + prefijo as nombre FROM LAB_SectorServicio with (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorUrgencia, m_ssql, "idSectorServicio", "nombre", connReady);
            ddlSectorUrgencia.Items.Insert(0, new ListItem("", "0"));

            oUtil.CargarCombo(ddlSectorDefecto, m_ssql, "idSectorServicio", "nombre", connReady);
            ddlSectorDefecto.Items.Insert(0, new ListItem("", "0"));
            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigenUrgencia, m_ssql, "idOrigen", "nombre", connReady);
            ddlOrigenUrgencia.Items.Insert(0, new ListItem("", "0"));
            oUtil.CargarCheckBox(chkOrigen, m_ssql, "idOrigen", "nombre", connReady);

            ///Carga de hojas de trabajo para histocomptaibilidad
            m_ssql = " SELECT idHojaTrabajo,  codigo FROM LAB_HojaTrabajo with (nolock) WHERE idEfector=" + oUser.IdEfector.IdEfector.ToString()+" and (baja = 0) order by codigo";
            oUtil.CargarCombo(ddlHisto, m_ssql, "idHojaTrabajo", "codigo", connReady);
            ddlHisto.Items.Insert(0, new ListItem("", "0"));


            ///Carga de combos de REgiones
            m_ssql = "select idregion, nombre from sys_region with (nolock)";
            oUtil.CargarCombo(ddlRegion, m_ssql, "idregion", "nombre", connReady);
            ddlRegion.Items.Insert(0, new ListItem("", "0"));
            //foreach (string MiImpresora in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    ddlImpresora.Items.Add(MiImpresora);
            //}
            //ddlImpresora.Items.Insert(0, new ListItem("", "0"));


            ///Carga de hojas de trabajo para histocomptaibilidad
            m_ssql = " SELECT idCaracter,  nombre FROM LAB_caracter with (nolock) ";
            oUtil.CargarCheckBox(chkFISCaracter, m_ssql, "idCaracter", "nombre");

            ///Carga de areas a imprimir
            m_ssql = @"with areas as (select -1 as idArea, 'Etiqueta general' as nombre
union
SELECT  idArea, A.nombre+ ' ('+ S.nombre + ')' as nombre FROM LAB_Area A with (nolock)
inner join LAB_TipoServicio S with (nolock) on A.idTipoServicio= S.idTipoServicio 
WHERE (A.baja = 0) and A.imprimecodigoBarra=1
)
select * from areas order by nombre ";
            oUtil.CargarCheckBox (chkAreas, m_ssql, "idArea", "nombre", connReady);
           

            if (oUser.IdEfector.IdEfector.ToString() == "227")
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E with (nolock) " +
                     " INNER JOIN lab_Configuracion C with (nolock) on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);

              //  btnReinializacion.Visible = true;
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  with (nolock) where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }

            m_ssql = null;
            oUtil = null;
        }

        private void MostrarAreasCodigoBarrasLaboratorio()
        {
           
           

            ConfiguracionAreaEtiqueta oItem = new ConfiguracionAreaEtiqueta();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ConfiguracionAreaEtiqueta));
            crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));

            IList items = crit.List();
            foreach (ConfiguracionAreaEtiqueta oArea in items)
            {
                string s_idArea = oArea.IdArea.ToString();
                for (int i = 0; i < chkAreas.Items.Count; i++)
                {
                   
                    if (s_idArea == chkAreas.Items[i].Value)
                        chkAreas.Items[i].Selected = true;
                }              
            }
 
        }

        private void MostrarDatosCodigoBarrasLaboratorio()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 1);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo, "IdEfector", oUser.IdEfector);

            if (oRegistro == null)
            {
                chkImprimeCodigoBarrasLaboratorio.Checked = false;
                pnlLaboratorio.Enabled = false;
            }
            else
            {
                chkImprimeCodigoBarrasLaboratorio.Checked = oRegistro.Habilitado;
                pnlLaboratorio.Enabled = chkImprimeCodigoBarrasLaboratorio.Checked;
                if  (oRegistro.Fuente!="")
                    ddlFuente.SelectedValue = oRegistro.Fuente;
                chkProtocolo.Items[1].Selected = oRegistro.ProtocoloFecha;
                chkProtocolo.Items[2].Selected = oRegistro.ProtocoloOrigen;

                chkProtocolo.Items[3].Selected = oRegistro.ProtocoloSector;
                chkProtocolo.Items[4].Selected = oRegistro.ProtocoloNumeroOrigen;

                chkPaciente.Items[0].Selected = oRegistro.PacienteApellido;
                chkPaciente.Items[1].Selected = oRegistro.PacienteSexo;
                chkPaciente.Items[2].Selected = oRegistro.PacienteEdad;
                chkPaciente.Items[3].Selected = oRegistro.PacienteNumeroDocumento;



            }
        }

        private void MostrarDatosCodigoBarrasMicrobiologia()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 3);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo , "IdEfector", oUser.IdEfector);

            if (oRegistro == null)
            {
                chkImprimeCodigoBarrasMicrobiologia.Checked = false;
                pnlMicrobiologia.Enabled = false;
            }
            else
            {
                chkImprimeCodigoBarrasMicrobiologia.Checked = oRegistro.Habilitado;
                pnlMicrobiologia.Enabled = chkImprimeCodigoBarrasMicrobiologia.Checked;
                if (oRegistro.Fuente != "")
                    rdbFuente2.SelectedValue = oRegistro.Fuente;
                chkProtocolo2.Items[1].Selected = oRegistro.ProtocoloFecha;
                chkProtocolo2.Items[2].Selected = oRegistro.ProtocoloOrigen;

                chkProtocolo2.Items[3].Selected = oRegistro.ProtocoloSector;
                chkProtocolo2.Items[4].Selected = oRegistro.ProtocoloNumeroOrigen;

                chkPaciente2.Items[0].Selected = oRegistro.PacienteApellido;
                chkPaciente2.Items[1].Selected = oRegistro.PacienteSexo;
                chkPaciente2.Items[2].Selected = oRegistro.PacienteEdad;
                chkPaciente2.Items[3].Selected = oRegistro.PacienteNumeroDocumento;



            }
        }


        private void MostrarDatosCodigoBarrasPesquisa()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 4);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo, "IdEfector", oUser.IdEfector);

            if (oRegistro == null)
            {
                chkImprimeCodigoBarrasPesquisa.Checked = false;

                pnlPesquisa.Enabled = false;
            }
            else
            {
                chkImprimeCodigoBarrasPesquisa.Checked = oRegistro.Habilitado;
                pnlPesquisa.Enabled = chkImprimeCodigoBarrasPesquisa.Checked;
                if (oRegistro.Fuente != "")
                    rdbFuente3.SelectedValue = oRegistro.Fuente;
                chkProtocolo3.Items[1].Selected = oRegistro.ProtocoloFecha;
                chkProtocolo3.Items[2].Selected = oRegistro.ProtocoloOrigen;

                chkProtocolo3.Items[3].Selected = oRegistro.ProtocoloSector;
                chkProtocolo3.Items[4].Selected = oRegistro.ProtocoloNumeroOrigen;

                chkPaciente3.Items[0].Selected = oRegistro.PacienteApellido;
                chkPaciente3.Items[1].Selected = oRegistro.PacienteSexo;
                chkPaciente3.Items[2].Selected = oRegistro.PacienteEdad;
                chkPaciente3.Items[3].Selected = oRegistro.PacienteNumeroDocumento;



            }
        }
        private void MostrarDatos()
        {
            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
            if (oEfector != null)
            {

                lblRefes.Text = oEfector.CodigoREFES;
              
                lblIdEfector2.Text = oEfector.IdEfector2;
                ddlRegion.SelectedValue = oEfector.IdRegion.ToString();
                lblJefe.Text = oEfector.JefeLaboratorio;
                lblCorreoJefe.Text = oEfector.CorreoJefe;

                //Usuario oUser = new Usuario();
                //oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                Configuracion oC = new Configuracion();
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oEfector);

                ddlSectorDefecto.SelectedValue = oC.IdSectorDefecto.ToString();

                if (!SiNoHayProtocolosCargados()) { lblMensajeNumeracion.Visible = true; rdbTipoNumeracionProtocolo.Enabled = false; }
                //lblZona.Text += oC.IdEfector.IdZona.Nombre;
                //lblEfector.Text += oC.IdEfector.Nombre;

                if (oC.PeticionElectronica) ddlPeticionElectronica.SelectedValue = "1";
                else ddlPeticionElectronica.SelectedValue = "0";


                ddlHisto.SelectedValue = oC.IdHistocompatibilidad.ToString();
                if (oC.PreValida)
                    ddlPreValidacion.SelectedValue = "1";
                else
                    ddlPreValidacion.SelectedValue = "0";

                ///SISA
                if (oC.NotificarSISA)
                    ddlNotificarSISA.SelectedValue = "1";
                else
                    ddlNotificarSISA.SelectedValue = "0";

                // andes
                if (oC.NotificaAndes)
                    ddlNotificaAndes.SelectedValue = "1";
                else
                    ddlNotificaAndes.SelectedValue = "0";


                if (oC.ConectaRenaper)
                    ddlRenaper.SelectedValue = "1";
                else
                    ddlRenaper.SelectedValue = "0";
                if (oC.VerificaIngresoAnterior)
                    ddlVerificaIngreso.SelectedValue = "1";
                else
                    ddlVerificaIngreso.SelectedValue = "0";


                txtUrlServicioSISA.Text = oC.UrlServicioSISA;
                txtUrlMuestraSISA.Text = oC.URLMuestraSISA;
                txtUrlResultadoSISA.Text = oC.URLResultadoSISA;
                txtCodigoEstablecimientoSISA.Text = oC.CodigoEstablecimientoSISA;

                txtUrlRenaper.Text = oC.UrlRenaper;
                txtCodigoCovid.Text = oC.CodigoCovid;


                if (oC.ConectaMPI)
                    ddlAndes.SelectedValue = "1";
                else
                    ddlAndes.SelectedValue = "0";
                txtUrlAndes.Text = oC.UrlMPI;


                if (oC.DiagObligatorio)
                    ddlDiagnostico.SelectedValue = "1";
                else
                    ddlDiagnostico.SelectedValue = "0";

                if (oC.MedicoObligatorio)
                    ddlMedicoObligatorio.SelectedValue = "1";
                else
                    ddlMedicoObligatorio.SelectedValue = "0";
                txtUrlMatriculacion.Text = oC.UrlMatriculacion;
                string[] arr = oC.OrigenHabilitado.Split((",").ToCharArray());
                foreach (string item in arr)
                {

                    for (int i = 0; i < chkOrigen.Items.Count; i++)
                    {
                        if (item == chkOrigen.Items[i].Value)
                            chkOrigen.Items[i].Selected = true;
                    }
                }

                string[] arrfis = oC.FISCaracter.Split((",").ToCharArray());
                foreach (string item in arrfis)
                {
                    for (int i = 0; i < chkFISCaracter.Items.Count; i++)
                    {
                        if (item == chkFISCaracter.Items[i].Value)
                            chkFISCaracter.Items[i].Selected = true;
                    }
                }

                ////
                MostrarAreasCodigoBarrasLaboratorio();
                /////////Grupo referido al Comprobante para el paciente Protocolo//////////////
                if (!oC.GeneraComprobanteProtocolo) ddlProtocoloComprobante.SelectedValue = "0";
                else ddlProtocoloComprobante.SelectedValue = "1";

                if (!oC.GeneraComprobanteProtocoloMicrobiologia) ddlProtocoloComprobanteMicrobiologia.SelectedValue = "0";
                else ddlProtocoloComprobanteMicrobiologia.SelectedValue = "1";

                if (!oC.HabilitaConsentimientoMicrobiologia) ddlConsentimientoMicrobiologia.SelectedValue = "0";
                else ddlConsentimientoMicrobiologia.SelectedValue = "1";


                txtTextoAdicionalComprobante.Text = oC.TextoAdicionalComprobanteProtocolo;
                txtTextoAdicionalComprobanteMicrobiologia.Text = oC.TextoAdicionalComprobanteProtocoloMicrobiologia;

                /////////////////////fin/////////////////////////

                ////Accesos directos de la pantalla principal

                chkAccesoPrincipal.Items[0].Selected = oC.PrincipalTurno;
                chkAccesoPrincipal.Items[1].Selected = oC.PrincipalRecepcion;
                chkAccesoPrincipal.Items[2].Selected = oC.PrincipalImpresionHT;
                chkAccesoPrincipal.Items[3].Selected = oC.PrincipalCargaResultados;
                chkAccesoPrincipal.Items[4].Selected = oC.PrincipalValidacion;
                chkAccesoPrincipal.Items[5].Selected = oC.PrincipalImpresionResultados;
                chkAccesoPrincipal.Items[6].Selected = oC.PrincipalUrgencias;
                chkAccesoPrincipal.Items[7].Selected = oC.PrincipalResultados;


                rdbTipoNumeracionProtocolo.Items[oC.TipoNumeracionProtocolo].Selected = true;

                ddlOrdenProtocolos.SelectedValue = oC.TipoOrdenProtocolo;

                ////dias de entrega            
                if (oC.TipoCalculoDiasRetiro == 0)
                    rdbDiasEspera.Items[0].Selected = true;
                else
                    rdbDiasEspera.Items[1].Selected = true;


                txtDiasEntrega.Value = oC.DiasRetiro.ToString();

                HabilitarValidadorDias();


                CalendarioEntrega oItem = new CalendarioEntrega();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(CalendarioEntrega));
                crit.Add(Expression.Eq("IdEfector", oC.IdEfector));

                IList items = crit.List();
                foreach (CalendarioEntrega oDia in items)
                {
                    int i = oDia.Dia;
                    cklDias.Items[i - 1].Selected = true;
                }

                /////////////////////////////////////////////////

                /////Recordar el ultimo origen cargado ///////
                if (oC.RecordarOrigenProtocolo) ddlRecordarOrigenProtocolo.SelectedValue = "1";
                else ddlRecordarOrigenProtocolo.SelectedValue = "0";
                //////////////////////////////////////////////

                /////Recordar el ultimo sector cargado ///////
                if (oC.RecordarSectorProtocolo) ddlRecordarSectorProtocolo.SelectedValue = "1";
                else ddlRecordarSectorProtocolo.SelectedValue = "0";
                //////////////////////////////////////////////


                ///Tamaño maximo de las paginas de la lista de protocolos            
                ddlPaginadoProtocolo.SelectedValue = oC.CantidadProtocolosPorPagina.ToString();
                ///////////

                /////modificar el protocolo terminado ///////
                if (oC.ModificarProtocoloTerminado) ddlModificaProtocoloTerminado.SelectedValue = "1";
                else ddlModificaProtocoloTerminado.SelectedValue = "0";
                //////////////////////////////////////////////

                /////eliminar el protocolo terminado ///////
                if (oC.EliminarProtocoloTerminado) ddlEliminaProtocoloTerminado.SelectedValue = "1";
                else ddlEliminaProtocoloTerminado.SelectedValue = "0";
                //////

                if (oC.HabilitaNoPublicacion)
                    ddlHabilitaPublicacionProtocolo.SelectedValue = "1";
                else
                    ddlHabilitaPublicacionProtocolo.SelectedValue = "0";
                ////Modulo Urgencia
                ddlOrigenUrgencia.SelectedValue = oC.IdOrigenUrgencia.ToString();
                ddlSectorUrgencia.SelectedValue = oC.IdSectorUrgencia.ToString();
                //////////

                /////////////Grupo referido al Turno/////////////
                if (!oC.Turno)
                    ddlTurno.SelectedValue = "0";
                else
                    ddlTurno.SelectedValue = "1";

                if (!oC.GeneraComprobanteTurno)
                    ddlTurnoComprobante.SelectedValue = "0";
                else
                    ddlTurnoComprobante.SelectedValue = "1";

                if (!oC.SmsCancelaTurno) ddlSmsCancelaTurno.SelectedValue = "0";
                else ddlSmsCancelaTurno.SelectedValue = "1";
                ////////////////////////////////////////////////

                /////Formato de la Lista de Protocolos ///////
                rdbTipoListaProtocolo.SelectedValue = oC.TipoListaProtocolo.ToString();

                //////////////////////////////////////////////

                //////////Formato de la Hoja de Trabajo///////////
                //if (oC.TipoHojaTrabajo == 0)
                //{
                //    rdbHojaTrabajo.Items[0].Selected = true;
                //    rdbHojaTrabajo.Items[1].Selected = false;
                //}
                //else
                //{
                //    rdbHojaTrabajo.Items[0].Selected = false;
                //    rdbHojaTrabajo.Items[1].Selected = true;
                //}
                ////////////////////////////////////////////////


                //////////Formato de la Carga de Resultados//////
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

                if (!oC.OrdenCargaResultado)
                    rdbOrdenCargaResultados.SelectedValue = "0";
                else
                    rdbOrdenCargaResultados.SelectedValue = "1";
                /////////////////////////////////////////////////

                ///////Tipo de impresion de resultado///

                if (oC.TipoImpresionResultado)
                {
                    rdbTipoImpresionResultado.Items[0].Selected = true;
                    rdbTipoImpresionResultado.Items[1].Selected = false;
                }
                else
                {
                    rdbTipoImpresionResultado.Items[0].Selected = false;
                    rdbTipoImpresionResultado.Items[1].Selected = true;
                }

                if (oC.TipoImpresionResultadoMicrobiologia)
                {
                    rdbTipoImpresionResultadoMicrobiologia.Items[0].Selected = true;
                    rdbTipoImpresionResultadoMicrobiologia.Items[1].Selected = false;
                }
                else
                {
                    rdbTipoImpresionResultadoMicrobiologia.Items[0].Selected = false;
                    rdbTipoImpresionResultadoMicrobiologia.Items[1].Selected = true;
                }



                ddlTipoHojaImpresionResultados.SelectedValue = oC.TipoHojaImpresionResultado;
                ddlTipoHojaImpresionResultadosMicrobiologia.SelectedValue = oC.TipoHojaImpresionResultadoMicrobiologia;
                //////////////////////////////////////////

                ///Aplicar formula por defecto
                if (oC.AplicarFormulaDefecto) ddlAplicaFormula.SelectedValue = "1";
                else ddlAplicaFormula.SelectedValue = "0";



                ////Datos a imprimir del Protocolo///////////////

                chkDatosProtocoloImprimir.Items[0].Selected = oC.ResultadoNumeroRegistro;
                chkDatosProtocoloImprimir.Items[1].Selected = oC.ResultadoFechaEntrega;
                chkDatosProtocoloImprimir.Items[2].Selected = oC.ResultadoSector;
                chkDatosProtocoloImprimir.Items[3].Selected = oC.ResultadoSolicitante;
                chkDatosProtocoloImprimir.Items[4].Selected = oC.ResultadoOrigen;
                chkDatosProtocoloImprimir.Items[5].Selected = oC.ResultadoPrioridad;

                chkDatosProtocoloImprimirMicrobiologia.Items[0].Selected = oC.ResultadoNumeroRegistroMicrobiologia;
                chkDatosProtocoloImprimirMicrobiologia.Items[1].Selected = oC.ResultadoFechaEntregaMicrobiologia;
                chkDatosProtocoloImprimirMicrobiologia.Items[2].Selected = oC.ResultadoSectorMicrobiologia;
                chkDatosProtocoloImprimirMicrobiologia.Items[3].Selected = oC.ResultadoSolicitanteMicrobiologia;
                chkDatosProtocoloImprimirMicrobiologia.Items[4].Selected = oC.ResultadoOrigenMicrobiologia;
                chkDatosProtocoloImprimirMicrobiologia.Items[5].Selected = oC.ResultadoPrioridadMicrobiologia;
                /////////////////////////////////////////////////


                ////Datos a imprimir del Paciente///////////////

                chkDatosPacienteImprimir.Items[3].Selected = oC.ResultadoEdad; ///edad
                chkDatosPacienteImprimir.Items[4].Selected = oC.ResultadoFNacimiento; ///f.nacimiento
                chkDatosPacienteImprimir.Items[5].Selected = oC.ResultadoSexo; ///sexo
                chkDatosPacienteImprimir.Items[2].Selected = oC.ResultadoHC; ///hc
                chkDatosPacienteImprimir.Items[1].Selected = oC.ResultadoDNI; ///dni
                chkDatosPacienteImprimir.Items[6].Selected = oC.ResultadoDomicilio; ///domicilio

                chkDatosPacienteImprimirMicrobiologia.Items[3].Selected = oC.ResultadoEdadMicrobiologia;
                chkDatosPacienteImprimirMicrobiologia.Items[4].Selected = oC.ResultadoFNacimientoMicrobiologia;
                chkDatosPacienteImprimirMicrobiologia.Items[5].Selected = oC.ResultadoSexoMicrobiologia;
                chkDatosPacienteImprimirMicrobiologia.Items[2].Selected = oC.ResultadoHCMicrobiologia;
                chkDatosPacienteImprimirMicrobiologia.Items[1].Selected = oC.ResultadoDNIMicrobiologia;
                chkDatosPacienteImprimirMicrobiologia.Items[6].Selected = oC.ResultadoDomicilioMicrobiologia;

                /////////////////////////////////////////////////


                ////////firma electronica///////////
                ddlImprimePieResultados.SelectedValue = oC.FirmaElectronicaLaboratorio.ToString();
                ddlImprimePieResultadosMicrobiologia.SelectedValue = oC.FirmaElectronicaMicrobiologia.ToString();
                //if (oC.ResultadoImprimePie)ddlImprimePieResultados.SelectedValue = "1";
                //else ddlImprimePieResultados.SelectedValue = "0";

                ////////////////////////////////////////////////
                /////////Formato de Impresión///////////////////
                txtEncabezado1.Text = oC.EncabezadoLinea1;
                txtEncabezado2.Text = oC.EncabezadoLinea2;
                txtEncabezado3.Text = oC.EncabezadoLinea3;

                txtEncabezado1Microbiologia.Text = oC.EncabezadoLinea1Microbiologia;
                txtEncabezado2Microbiologia.Text = oC.EncabezadoLinea2Microbiologia;
                txtEncabezado3Microbiologia.Text = oC.EncabezadoLinea3Microbiologia;

                ////////////////////////////////////////////////

                ddlTipoHojaImpresionResultados.SelectedValue = oC.TipoHojaImpresionResultado;
                /*Logo: Se saca codigo realacionado al logo
              if (oC.RutaLogo != "")
              {
                  Image1.Visible = true;
                  Image1.ImageUrl = "~/Logo/" + oC.RutaLogo;

              }
              */



              if (!oC.AutenticaValidacion)
                  ddlAutenticaValidacion.SelectedValue = "0";
              else
                  ddlAutenticaValidacion.SelectedValue = "1";



              ddlFechaOrden.SelectedValue = oC.ValorDefectoFechaOrden.ToString();
              ddlFechaTomaMuestra.SelectedValue = oC.ValorDefectoFechaTomaMuestra.ToString();
              ddlNomencladorDiagnostico.SelectedValue = oC.NomencladorDiagnostico.ToString();
              ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

              ddlTipoEtiqueta.SelectedValue = oC.TipoEtiqueta;
              mostrarImagenEtiqueta();

              MostrarDatosCodigoBarrasLaboratorio(); MostrarDatosCodigoBarrasMicrobiologia(); MostrarDatosCodigoBarrasPesquisa();
              //MostrarImpresoras();
              ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

              oC = null;
          }
      }

      private bool SiNoHayProtocolosCargados()
      {
          bool dev=true;
          ISession m_session = NHibernateHttpModule.CurrentSession;
          ICriteria crit = m_session.CreateCriteria(typeof(Protocolo));            
          crit.Add(Expression.Sql(" IdProtocolo in (Select top 1 IdProtocolo From LAb_Protocolo where Baja=0)"));
          IList lista = crit.List();
          if (lista.Count > 0)
          {
              dev = false;
          }
          return dev;
      }

      protected void btnGuardar_Click(object sender, EventArgs e)
      {
          Guardar();


          Response.Redirect("ParametrosMsje.aspx", false);
      }

      private void Guardar()
      {
          Efector oEfector = new Efector();
          oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
          if (oEfector != null)
          {
              //Usuario oUser = new Usuario();
              //oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

              Configuracion oC = new Configuracion();
              oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oEfector);

              //Usuario oUser = new Usuario();
              //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

              //Configuracion oC = new Configuracion();
              //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

              //oC.NombreImpresora=ddlImpresora.SelectedValue  ;

              ////Accesos directos de la pantalla principal
              oEfector.CodigoREFES = lblRefes.Text;

              oEfector.IdEfector2=lblIdEfector2.Text  ;
              oEfector.JefeLaboratorio=                lblJefe.Text;
              oEfector.CorreoJefe=lblCorreoJefe.Text ;
              oEfector.IdRegion = int.Parse(ddlRegion.SelectedValue);
              oEfector.Save();


              oC.PrincipalTurno = chkAccesoPrincipal.Items[0].Selected;
              oC.PrincipalRecepcion = chkAccesoPrincipal.Items[1].Selected;
              oC.PrincipalImpresionHT = chkAccesoPrincipal.Items[2].Selected;
              oC.PrincipalCargaResultados = chkAccesoPrincipal.Items[3].Selected;
              oC.PrincipalValidacion = chkAccesoPrincipal.Items[4].Selected;
              oC.PrincipalImpresionResultados = chkAccesoPrincipal.Items[5].Selected;
              oC.PrincipalUrgencias = chkAccesoPrincipal.Items[6].Selected;
              oC.PrincipalResultados = chkAccesoPrincipal.Items[7].Selected;

              oC.UrlMatriculacion = txtUrlMatriculacion.Text;
              oC.TipoOrdenProtocolo = ddlOrdenProtocolos.SelectedValue ;
              oC.CodigoCovid = txtCodigoCovid.Text;
              string origenes = "";
              for (int i = 0; i < chkOrigen.Items.Count; i++)
              {
                  if (chkOrigen.Items[i].Selected)
                  {
                      if (origenes == "")
                          origenes = chkOrigen.Items[i].Value;
                      else
                          origenes += "," + chkOrigen.Items[i].Value;
                  }
              }
              oC.OrigenHabilitado = origenes;
              //////////////////////////////

              string fiscaracter = "";
              for (int i = 0; i < chkFISCaracter.Items.Count; i++)
              {
                  if (chkFISCaracter.Items[i].Selected)
                  {
                      if (fiscaracter == "")
                          fiscaracter = chkFISCaracter.Items[i].Value;
                      else
                          fiscaracter += "," + chkFISCaracter.Items[i].Value;
                  }
              }
              oC.FISCaracter = fiscaracter;


                ////guardar areas a imprimir
                GuardarAreasImprimir();
          
                ////fin de areas de impresion

                oC.IdSectorDefecto = int.Parse(ddlSectorDefecto.SelectedValue);

              if (ddlDiagnostico.SelectedValue == "0") oC.DiagObligatorio = false;
              else oC.DiagObligatorio = true;
              if (ddlPreValidacion.SelectedValue == "0") oC.PreValida = false;
              else oC.PreValida = true;

              if (ddlNotificarSISA.SelectedValue == "0") oC.NotificarSISA = false;
              else oC.NotificarSISA = true;

              if (ddlNotificaAndes.SelectedValue == "0") oC.NotificaAndes = false;
              else oC.NotificaAndes = true;

              oC.UrlServicioSISA = txtUrlServicioSISA.Text;
              oC.URLMuestraSISA = txtUrlMuestraSISA.Text;
              oC.URLResultadoSISA = txtUrlResultadoSISA.Text;

              oC.CodigoEstablecimientoSISA = txtCodigoEstablecimientoSISA.Text;

              oC.UrlRenaper = txtUrlRenaper.Text;
              oC.UrlMPI = txtUrlAndes.Text;

              if (ddlRenaper.SelectedValue == "0") oC.ConectaRenaper = false;
              else oC.ConectaRenaper = true;

              if (ddlAndes.SelectedValue == "0") oC.ConectaMPI = false;
              else oC.ConectaMPI = true;

              if (ddlMedicoObligatorio.SelectedValue == "0") oC.MedicoObligatorio = false;
              else oC.MedicoObligatorio = true;

              if (ddlVerificaIngreso.SelectedValue == "0")
                  oC.VerificaIngresoAnterior = false;
              else
                  oC.VerificaIngresoAnterior = true;

              /////////Grupo referido al comprobante para el paciente//////////////
              if (ddlProtocoloComprobante.SelectedValue == "0") oC.GeneraComprobanteProtocolo = false;
              else oC.GeneraComprobanteProtocolo = true;

              if (ddlProtocoloComprobanteMicrobiologia.SelectedValue == "0") oC.GeneraComprobanteProtocoloMicrobiologia = false;
              else oC.GeneraComprobanteProtocoloMicrobiologia = true;


              if (ddlConsentimientoMicrobiologia.SelectedValue == "0") oC.HabilitaConsentimientoMicrobiologia = false;
              else oC.HabilitaConsentimientoMicrobiologia = true;

              oC.TextoAdicionalComprobanteProtocolo = txtTextoAdicionalComprobante.Text;
              oC.TextoAdicionalComprobanteProtocoloMicrobiologia = txtTextoAdicionalComprobanteMicrobiologia.Text;

              ///////////////fin/////////////////

              if (ddlPeticionElectronica.SelectedValue == "1") oC.PeticionElectronica = true;
              else oC.PeticionElectronica = false;



              if (rdbTipoNumeracionProtocolo.Items[0].Selected) oC.TipoNumeracionProtocolo = 0;
              if (rdbTipoNumeracionProtocolo.Items[1].Selected) oC.TipoNumeracionProtocolo = 1;
              if (rdbTipoNumeracionProtocolo.Items[2].Selected) oC.TipoNumeracionProtocolo = 2;
              if (rdbTipoNumeracionProtocolo.Items[3].Selected) oC.TipoNumeracionProtocolo = 3;

              ///////utilizaNumeroEliminado ///////
              //if (ddlUtilizarNumeroEliminado.SelectedValue == "0") oC.UtilizaNumeroEliminado = false;
              //else oC.UtilizaNumeroEliminado = true;
              ////////////////////////////////////////////////


              ////dias de entrega
              if (rdbDiasEspera.Items[0].Selected)  //Calcula segun duración de analisis
              {
                  oC.TipoCalculoDiasRetiro = 0;
                  oC.DiasRetiro = 0;
              }
              else  //valor predeterminado
              {
                  oC.TipoCalculoDiasRetiro = 1;
                  oC.DiasRetiro = int.Parse(txtDiasEntrega.Value);
              }
              oC.IdHistocompatibilidad = int.Parse(ddlHisto.SelectedValue);

              ///Calendario de Entregas            


              ///Primero borra lo que hay
              CalendarioEntrega oItem = new CalendarioEntrega();
              ISession m_session = NHibernateHttpModule.CurrentSession;
              ICriteria crit = m_session.CreateCriteria(typeof(CalendarioEntrega));
              crit.Add(Expression.Eq("IdEfector", oC.IdEfector));

              IList items = crit.List();
              foreach (CalendarioEntrega oDia in items)
              {
                  oDia.Delete();
              }

              ///
              ///Escribe los nuevos datos
              for (int i = 0; i < cklDias.Items.Count; i++)
              {
                  if (cklDias.Items[i].Selected)
                  {
                      CalendarioEntrega oDia = new CalendarioEntrega();
                      oDia.IdEfector = oC.IdEfector;
                      oDia.Dia = i + 1;
                      oDia.Save();
                  }
              }
              /////////////////////////////////////////////////

              /////Recordar el ultimo origen cargado ///////
              if (ddlRecordarOrigenProtocolo.SelectedValue == "0") oC.RecordarOrigenProtocolo = false;
              else oC.RecordarOrigenProtocolo = true;
              //////////////////////////////////////////////

              /////Recordar el ultimo sector cargado ///////
              if (ddlRecordarSectorProtocolo.SelectedValue == "0") oC.RecordarSectorProtocolo = false;
              else oC.RecordarSectorProtocolo = true;
              //////

              ///Tamaño maximo de las paginas de la lista de protocolos            
              oC.CantidadProtocolosPorPagina = int.Parse(ddlPaginadoProtocolo.SelectedValue);

              ///////////

              /////modificar el protocolo terminado ///////
              if (ddlModificaProtocoloTerminado.SelectedValue == "0") oC.ModificarProtocoloTerminado = false;
              else oC.ModificarProtocoloTerminado = true;
              //////////////////////////////////////////////

              /////eliminar el protocolo terminado ///////
              if (ddlEliminaProtocoloTerminado.SelectedValue == "0") oC.EliminarProtocoloTerminado = false;
              else oC.EliminarProtocoloTerminado = true;
              //////

              if (ddlHabilitaPublicacionProtocolo.SelectedValue == "0")
                  oC.HabilitaNoPublicacion = false;
              else
                  oC.HabilitaNoPublicacion = true;

              ////Modulo Urgencia
              oC.IdOrigenUrgencia = int.Parse(ddlOrigenUrgencia.SelectedValue);
              oC.IdSectorUrgencia = int.Parse(ddlSectorUrgencia.SelectedValue);
              //////////


              /////////////Grupo referido al Turno/////////////
              if (ddlTurno.SelectedValue == "0") oC.Turno = false;
              else oC.Turno = true;


              if (ddlTurnoComprobante.SelectedValue == "0") oC.GeneraComprobanteTurno = false;
              else oC.GeneraComprobanteTurno = true;

              if (ddlSmsCancelaTurno.SelectedValue == "0") oC.SmsCancelaTurno = false;
              else
                  oC.SmsCancelaTurno = true;
              ////////////////////////////////////////////////

              /////Formato de la Lista de Protocolos ///////
              oC.TipoListaProtocolo = int.Parse(rdbTipoListaProtocolo.SelectedValue);


              //////////////////////////////////////////////


              ////////Formato de la Hoja de Trabajo///////////
              //if (rdbHojaTrabajo.Items[0].Selected)  oC.TipoHojaTrabajo = 0;
              //else    oC.TipoHojaTrabajo = 1;

              ////////////////////////////////////////////////




              oC.TipoCargaResultado = int.Parse(rdbCargaResultados.SelectedValue);
              if (rdbOrdenCargaResultados.SelectedValue == "0")
                  oC.OrdenCargaResultado = false;
              else
                  oC.OrdenCargaResultado = true;
              /////////////////////////////////////////////////

              ///////Tipo de impresion de resultado///

              oC.TipoImpresionResultado = rdbTipoImpresionResultado.Items[0].Selected;
              oC.TipoHojaImpresionResultado = ddlTipoHojaImpresionResultados.SelectedValue.Trim();

              oC.TipoImpresionResultadoMicrobiologia = rdbTipoImpresionResultadoMicrobiologia.Items[0].Selected;
              oC.TipoHojaImpresionResultadoMicrobiologia = ddlTipoHojaImpresionResultadosMicrobiologia.SelectedValue.Trim();
              //   oC.tipo
              //////////////////////////////////////////


              ///Aplicar formula por defecto ///

              if (ddlAplicaFormula.SelectedValue == "0") oC.AplicarFormulaDefecto = false;
              else oC.AplicarFormulaDefecto = true;


              ////Datos a imprimir del Protocolo///////////////

              oC.ResultadoNumeroRegistro = chkDatosProtocoloImprimir.Items[0].Selected;
              oC.ResultadoFechaEntrega = chkDatosProtocoloImprimir.Items[1].Selected;
              oC.ResultadoSector = chkDatosProtocoloImprimir.Items[2].Selected;
              oC.ResultadoSolicitante = chkDatosProtocoloImprimir.Items[3].Selected;
              oC.ResultadoOrigen = chkDatosProtocoloImprimir.Items[4].Selected;
              oC.ResultadoPrioridad = chkDatosProtocoloImprimir.Items[5].Selected;


              oC.ResultadoNumeroRegistroMicrobiologia = chkDatosProtocoloImprimirMicrobiologia.Items[0].Selected;
              oC.ResultadoFechaEntregaMicrobiologia = chkDatosProtocoloImprimirMicrobiologia.Items[1].Selected;
              oC.ResultadoSectorMicrobiologia = chkDatosProtocoloImprimirMicrobiologia.Items[2].Selected;
              oC.ResultadoSolicitanteMicrobiologia = chkDatosProtocoloImprimirMicrobiologia.Items[3].Selected;
              oC.ResultadoOrigenMicrobiologia = chkDatosProtocoloImprimirMicrobiologia.Items[4].Selected;
              oC.ResultadoPrioridadMicrobiologia = chkDatosProtocoloImprimirMicrobiologia.Items[5].Selected;
              /////////////////////////////////////////////////


              ////Datos a imprimir del Paciente///////////////

              oC.ResultadoEdad = chkDatosPacienteImprimir.Items[3].Selected; ///edad
              oC.ResultadoFNacimiento = chkDatosPacienteImprimir.Items[4].Selected; ///f.nacimiento
              oC.ResultadoSexo = chkDatosPacienteImprimir.Items[5].Selected; ///sexo
              oC.ResultadoHC = chkDatosPacienteImprimir.Items[2].Selected; ///hc
              oC.ResultadoDNI = chkDatosPacienteImprimir.Items[1].Selected; ///dni
              oC.ResultadoDomicilio = chkDatosPacienteImprimir.Items[6].Selected; ///domicilio
                                                                                  ///

              oC.ResultadoEdadMicrobiologia = chkDatosPacienteImprimirMicrobiologia.Items[3].Selected;
              oC.ResultadoFNacimientoMicrobiologia = chkDatosPacienteImprimirMicrobiologia.Items[4].Selected;
              oC.ResultadoSexoMicrobiologia = chkDatosPacienteImprimirMicrobiologia.Items[5].Selected;
              oC.ResultadoHCMicrobiologia = chkDatosPacienteImprimirMicrobiologia.Items[2].Selected;
              oC.ResultadoDNIMicrobiologia = chkDatosPacienteImprimirMicrobiologia.Items[1].Selected;
              oC.ResultadoDomicilioMicrobiologia = chkDatosPacienteImprimirMicrobiologia.Items[6].Selected;
              /////////////////////////////////////////////////

              ////////Imprime firma electronica///////////
              oC.FirmaElectronicaLaboratorio = int.Parse(ddlImprimePieResultados.SelectedValue);
              oC.FirmaElectronicaMicrobiologia = int.Parse(ddlImprimePieResultadosMicrobiologia.SelectedValue);
              //if (ddlImprimePieResultados.SelectedValue == "0")
              //    oC.ResultadoImprimePie = false;
              //else
              //    oC.ResultadoImprimePie = true;

              ////////////////////////////////////////////////

              /////////Formato de Impresión///////////////////
              oC.EncabezadoLinea1 = txtEncabezado1.Text;
              oC.EncabezadoLinea2 = txtEncabezado2.Text;
              oC.EncabezadoLinea3 = txtEncabezado3.Text;

              oC.EncabezadoLinea1Microbiologia = txtEncabezado1Microbiologia.Text;
              oC.EncabezadoLinea2Microbiologia = txtEncabezado2Microbiologia.Text;
              oC.EncabezadoLinea3Microbiologia = txtEncabezado3Microbiologia.Text;
              ////////////////////////////////////////
              /*Logo: Se saca codigo realacionado al logo
              if (chkBorrarImagen.Checked)
                  oC.RutaLogo = "";
              else
                  if (fupLogo.FileName != "") oC.RutaLogo = "logo." + fupLogo.PostedFile.FileName.Split('.')[1];

              if (fupLogo.FileName != "")
              {
                  //arch.PostedFile.SaveAs("nuevo_nombre." + arch.PostedFile.FileName.Split('.')[1]);

                  fupLogo.PostedFile.SaveAs(Server.MapPath("~/Logo/logo." + fupLogo.PostedFile.FileName.Split('.')[1]));
                  //this.fupLogo.SaveAs(Server.MapPath("~/Logo/" + oC.RutaLogo));                                   
              }
              */
                if (ddlAutenticaValidacion.SelectedValue == "0")
                    oC.AutenticaValidacion = false;
                else
                    oC.AutenticaValidacion = true;


                ////

                oC.ValorDefectoFechaOrden = int.Parse(ddlFechaOrden.SelectedValue);
                oC.ValorDefectoFechaTomaMuestra = int.Parse(ddlFechaTomaMuestra.SelectedValue);
                oC.NomencladorDiagnostico = int.Parse(ddlNomencladorDiagnostico.SelectedValue);

                oC.TipoEtiqueta = ddlTipoEtiqueta.SelectedValue;
                oC.Save();
                GuardarCodigoBarrasMicrobiologia(); GuardarCodigoBarrasLaboratorio(); GuardarCodigoBarrasPesquisa();
            }
        }

        private void GuardarAreasImprimir()
        {
            ///Primero borra lo que hay
            ConfiguracionAreaEtiqueta oItem = new ConfiguracionAreaEtiqueta();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ConfiguracionAreaEtiqueta));
            crit.Add(Expression.Eq("IdEfector",oUser.IdEfector));

            IList items = crit.List();
            foreach (ConfiguracionAreaEtiqueta oItemcito in items)
            {
                oItemcito.Delete();
            }

            ///
            ///Escribe los nuevos datos
            for (int i = 0; i < chkAreas.Items.Count; i++)
            {
                if (chkAreas.Items[i].Selected)
                {
                    string s_idarea = chkAreas.Items[i].Value;
                    
                        ConfiguracionAreaEtiqueta oRegistro = new ConfiguracionAreaEtiqueta();
                        oRegistro.IdEfector = oUser.IdEfector;
                        oRegistro.IdArea =int.Parse( s_idarea);
                        oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
                        oRegistro.FechaRegistro = DateTime.Now;

                        oRegistro.Save();
                     
                }
            }
        }

        private void GuardarCodigoBarrasPesquisa()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 4);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo, "IdEfector", oUser.IdEfector);

            if (oRegistro == null)
            {
                ConfiguracionCodigoBarra oRegistroNew = new ConfiguracionCodigoBarra();
                GuardarCDsPesquisa(oRegistroNew, oTipo);
            }
            else
                GuardarCDsPesquisa(oRegistro, oTipo);




        }

        private void GuardarCodigoBarrasMicrobiologia()
        {
           TipoServicio oTipo = new TipoServicio();
                oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 3);

                ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
                oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo, "IdEfector", oUser.IdEfector);

                if (oRegistro == null)
                {
                    ConfiguracionCodigoBarra oRegistroNew = new ConfiguracionCodigoBarra();
                    GuardarCDsMicrobiologia(oRegistroNew, oTipo);
                }
                else
                GuardarCDsMicrobiologia(oRegistro,oTipo);
           
          


        }


        private void GuardarCDsPesquisa(ConfiguracionCodigoBarra oRegistro, TipoServicio oTipo)
        {

            oRegistro.Habilitado =chkImprimeCodigoBarrasPesquisa.Checked;

            oRegistro.IdTipoServicio = oTipo;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Fuente = rdbFuente3.SelectedValue;
            oRegistro.ProtocoloFecha = chkProtocolo3.Items[1].Selected;
            oRegistro.ProtocoloOrigen = chkProtocolo3.Items[2].Selected;
            oRegistro.ProtocoloSector = chkProtocolo3.Items[3].Selected;
            oRegistro.ProtocoloNumeroOrigen = chkProtocolo3.Items[4].Selected;

            oRegistro.PacienteApellido = chkPaciente3.Items[0].Selected;
            oRegistro.PacienteSexo = chkPaciente3.Items[1].Selected;
            oRegistro.PacienteEdad = chkPaciente3.Items[2].Selected;
            oRegistro.PacienteNumeroDocumento = chkPaciente3.Items[3].Selected;


            oRegistro.Save();
        }
        private void GuardarCDsMicrobiologia(ConfiguracionCodigoBarra oRegistro, TipoServicio oTipo)
        {
           
            oRegistro.Habilitado = chkImprimeCodigoBarrasMicrobiologia.Checked;

            oRegistro.IdTipoServicio = oTipo;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Fuente = rdbFuente2.SelectedValue;
            oRegistro.ProtocoloFecha = chkProtocolo2.Items[1].Selected;
            oRegistro.ProtocoloOrigen = chkProtocolo2.Items[2].Selected;
            oRegistro.ProtocoloSector = chkProtocolo2.Items[3].Selected;
            oRegistro.ProtocoloNumeroOrigen = chkProtocolo2.Items[4].Selected;

            oRegistro.PacienteApellido = chkPaciente2.Items[0].Selected;
            oRegistro.PacienteSexo = chkPaciente2.Items[1].Selected;
            oRegistro.PacienteEdad = chkPaciente2.Items[2].Selected;
            oRegistro.PacienteNumeroDocumento = chkPaciente2.Items[3].Selected;


            oRegistro.Save();
        }

        private void GuardarCodigoBarrasLaboratorio()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 1);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo ,"IdEfector", oUser.IdEfector);

            if (oRegistro == null)
            {
                ConfiguracionCodigoBarra oRegistroNew = new ConfiguracionCodigoBarra();
                GuardarCDLaboratorio(oRegistroNew, oTipo);
            }
            else
                GuardarCDLaboratorio(oRegistro, oTipo);
           


        }

        private void GuardarCDLaboratorio(ConfiguracionCodigoBarra oRegistro, TipoServicio oTipo)
        {
            oRegistro.Habilitado = chkImprimeCodigoBarrasLaboratorio.Checked;
            oRegistro.IdTipoServicio = oTipo;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Fuente = ddlFuente.SelectedValue;
            oRegistro.ProtocoloFecha = chkProtocolo.Items[1].Selected;
            oRegistro.ProtocoloOrigen = chkProtocolo.Items[2].Selected;
            oRegistro.ProtocoloSector = chkProtocolo.Items[3].Selected;
            oRegistro.ProtocoloNumeroOrigen = chkProtocolo.Items[4].Selected;

            oRegistro.PacienteApellido = chkPaciente.Items[0].Selected;
            oRegistro.PacienteSexo = chkPaciente.Items[1].Selected;
            oRegistro.PacienteEdad = chkPaciente.Items[2].Selected;
            oRegistro.PacienteNumeroDocumento = chkPaciente.Items[3].Selected;

            oRegistro.Save();
        }

        protected void chkImprimeCodigoBarrasLaboratorio_CheckedChanged(object sender, EventArgs e)
        {

            pnlLaboratorio.Enabled = chkImprimeCodigoBarrasLaboratorio.Checked;
            pnlLaboratorio.UpdateAfterCallBack = true;
            SetSelectedTab(TabIndex.DEFAULT);
        }


        protected void chkImprimeCodigoBarrasPesquisa_CheckedChanged(object sender, EventArgs e)
        {
            pnlPesquisa.Enabled = chkImprimeCodigoBarrasPesquisa.Checked;
            pnlPesquisa.UpdateAfterCallBack = true;
            SetSelectedTab(TabIndex.DEFAULT);
        }
        protected void chkImprimeCodigoBarrasMicrobiologia_CheckedChanged(object sender, EventArgs e)
        {

            pnlMicrobiologia.Enabled = chkImprimeCodigoBarrasMicrobiologia.Checked;
            pnlMicrobiologia.UpdateAfterCallBack = true;
            SetSelectedTab(TabIndex.ONE);
        }
        protected void rdbDiasEspera_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarValidadorDias();
        }

        private void HabilitarValidadorDias()
        {
            if (rdbDiasEspera.Items[1].Selected)
                rfvDiasEspera.Enabled = true;
            else
                rfvDiasEspera.Enabled = false;
            rfvDiasEspera.UpdateAfterCallBack = true;
        }

        protected void rdbTipoDias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbTipoDias.Items[0].Selected)//Todos los días
                for (int i = 0; i < 7; i++)
                {
                    cklDias.Items[i].Selected = true;
                }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    cklDias.Items[i].Selected = true;
                }
                for (int i = 5; i < 7; i++)
                {
                    cklDias.Items[i].Selected = false;
                }
            }
            cklDias.UpdateAfterCallBack = true;

        }

        protected void fupLogo_DataBinding(object sender, EventArgs e)
        {
          
            
            
        }

        protected void ddlRecordarOrigenProtocolo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void rdbTipoNumeracionProtocolo_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        protected void btnAgregarSector_Click(object sender, EventArgs e)
        {
     
        }

        protected void ddlTipoEtiqueta_SelectedIndexChanged(object sender, EventArgs e)
        {
            mostrarImagenEtiqueta();
            SetSelectedTab(TabIndex.NUEVE);

        }

        private void mostrarImagenEtiqueta()
        {
            if (ddlTipoEtiqueta.SelectedValue == "5x2.5")
            {
                imgEtiqueta5.Visible = true;
                imgEtiqueta8.Visible = false;
            }
            else
            {
                imgEtiqueta5.Visible = false;
                imgEtiqueta8.Visible = true;
            }

            imgEtiqueta5.UpdateAfterCallBack = true;
            imgEtiqueta8.UpdateAfterCallBack = true;
        }

        protected void btnReinializacion_Click(object sender, EventArgs e)
        {
          
            ReprocesarFichas();
            EjecutarMantenimiento();

            /////

        }
        private void EjecutarMantenimiento()
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_Mantenimiento]";


            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);



        }

        private void ReprocesarFichas()
        {
            ////Metodo que carga la grilla de Protocolos
            string m_strSQL = " Select body from [dbo].[LAB_FichaElectronica] where body not like '%barrio%'";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            //Ds.Tables[0];
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                try
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                string body = Ds.Tables[0].Rows[i][0].ToString();
                Protocolos.DefaultFFEE.FFEE respuesta_d = jsonSerializer.Deserialize<Protocolos.DefaultFFEE.FFEE>(body);
                if (respuesta_d.identificadorpcr != null)
                {
                    string _id = respuesta_d.identificadorpcr.Replace("HISOP00", "").Replace("HISOP0", "").Replace("HISOP", "");
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Protocolo));
                    crit.Add(Expression.Eq("NumeroOrigen2", _id));
                    IList lista = crit.List();
                    foreach (Business.Data.Laboratorio.Protocolo oRegistro in lista)
                    {
                       
                            if (oRegistro.IdPaciente.NumeroDocumento == int.Parse(respuesta_d.Paciente_documento))
                            {
                                if ((respuesta_d.direccioncaso != null) && (respuesta_d.localidadresidencia != null))
                                {
                                    if (respuesta_d.barrioresidencia == null)
                                        respuesta_d.barrioresidencia = "";
                                    if (respuesta_d.direccioncaso.Length > 100) respuesta_d.direccioncaso = respuesta_d.direccioncaso.Substring(0, 99);
                                    if (respuesta_d.barrioresidencia.Length > 100) respuesta_d.barrioresidencia = respuesta_d.barrioresidencia.Substring(0, 99);
                                    if (respuesta_d.localidadresidencia.Length > 100) respuesta_d.localidadresidencia = respuesta_d.localidadresidencia.Substring(0, 99);

                                    oRegistro.IdPaciente.GuardarDomicilio(oRegistro.FechaRegistro.ToShortDateString(), respuesta_d.direccioncaso, respuesta_d.barrioresidencia, respuesta_d.localidadresidencia, "", "", "");

                                }
                            }
                       
                    }
                }
                }
                catch
                { }
            }
        }
       
        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarDatos();
        }

        //protected void lnkImpresionPrueba_Click(object sender, EventArgs e)
        //{
        //    ImpresiondePrueba();
        //}

        //private void ImpresiondePrueba()
        //{
        //    ParameterDiscreteValue impresora = new ParameterDiscreteValue();
        //    impresora.Value = ddlImpresora.SelectedValue;

        //    oCr.Report.FileName = "iNFORMES /PaginaPrueba.rpt";
        //    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(impresora);

        //    oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
        //    oCr.ReportDocument.PrintToPrinter(1, false, 0,0);                
        //}

    }

    internal class jsonSerializer
    {
    }
}
