using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using System.IO;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Security.Principal;
using System.Text;
using System.Net;

namespace WebLab.Protocolos
{
    public partial class ProtocoloEditPesquisa : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {           
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;           
        }

        private void CargarGrilla()
        {
            ////Metodo que carga la grilla de Protocolos
            string m_strSQL = " Select distinct P.idProtocolo, " +
                             " dbo.NumeroProtocolo(P.idProtocolo) as numero," +
                                " convert(varchar(10),P.fecha,103) as fecha,P.estado " +
                              " from Lab_Protocolo P " + // +str_condicion;            
                              " WHERE P.idProtocolo in (" + Session["ListaProtocolo"].ToString() + ")" +
                               " order by numero ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();     
        }

       
        private Random
        random = new Random();

        private static int
            TEST = 0;

        private bool IsTokenValid()
        {
            bool result = double.Parse(hidToken.Value) == ((double)Session["NextToken"]);
            SetToken();
            return result;
        }

        private void SetToken()
        {
            double next = random.Next();
            hidToken.Value = next + "";
            Session["NextToken"] = next;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                SetToken();
                //Session["idServicio"] = "4";
                //PreventingDoubleSubmit(btnGuardar);
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Recepción de Pacientes");
                    if (Request["idServicio"] != null) Session["idServicio"] = Request["idServicio"].ToString();
                    if (Request["idUrgencia"] != null) Session["idUrgencia"] = Request["idUrgencia"].ToString();
                    CargarListas();                 

                    if (Request["Operacion"].ToString() == "Modifica")
                    {

                        lnkReimprimirComprobante.Visible = true;
                        lnkReimprimirCodigoBarras.Visible = true;

                        lblTitulo.Visible = true;
                        pnlNuevo.Visible = false;
                        pnlActualiza.Visible = true;
                        MuestraDatos();
           
                        if (Request["Desde"].ToString() =="Control")
                        {
                            pnlLista.Visible = true;
                            CargarGrilla();
                            gvLista.Visible = true;
                            pnlNavegacion.Visible = true;
                            //lnkAnterior.Visible = true;
                            //lnkSiguiente.Visible = true;
                        }
                        else
                        {
                            pnlLista.Visible = false;
                            gvLista.Visible = false;
                            pnlNavegacion.Visible = false;
                            //lnkAnterior.Visible = false;
                          
                            //lnkSiguiente.Visible = false;
                        }
                    }
                    else

                    {
                     
                        lnkReimprimirComprobante.Visible = false;
                        lnkReimprimirCodigoBarras.Visible = false;

                        lblTitulo.Visible = false;
                        txtFecha.Value = DateTime.Now.ToShortDateString();
                        txtFechaOrden.Value = DateTime.Now.ToShortDateString();                      
                        MostrarPaciente();

                        pnlNuevo.Visible = true;
                        pnlActualiza.Visible = false;

                        btnCancelar.Text = "Cancelar";
                        btnCancelar.Width = Unit.Pixel(80);

                     

                    
                        if (Request["Operacion"].ToString() == "AltaDerivacion")
                        {
                            this.PesquisaNeonatal1.CargarScreening(int.Parse(Request["idSolicitudScreening"].ToString()));
                            txtNumeroOrigen.Text = Request["numeroOrigen"].ToString();
                            txtFechaOrden.Value = Request["fechaOrden"].ToString();
                            ddlEfector.SelectedValue = Request["idEfectorSolicitante"].ToString(); SelectedEfector();
                            txtSala.Text = Request["sala"].ToString();
                            txtCama.Text = Request["cama"].ToString();
                            CargarDeterminacionesDerivacion( Request["analisis"].ToString(), Request["diagnostico"].ToString());                          
                            ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina
                            //  ddlPrioridad.SelectedValue = "1"; //Normal;
                        }

                        if (Request["Operacion"].ToString() == "Alta")
                        {
                            this.PesquisaNeonatal1.InicializarIngreso();
                        
                        }
                    
                             
                    }
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
        }

        

        private void CargarDeterminacionesDerivacion(string s_analisis, string s_diagnostico)
        {


            string[] tabla = s_analisis.Split('|');
            string sDatos = "";
            /////Crea nuevamente los detalles.
            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                if (sDatos == "")
                    sDatos = tabla[i].ToString() + "#Si";
                else
                    sDatos += ";" + tabla[i].ToString() + "#Si";
               
            }
            

         

            TxtDatosCargados.Value = sDatos;
            if (s_diagnostico != "0")
            {
                ///Traer datos de diagnsotico        
                Cie10 oC = new Cie10();
                oC = (Cie10)oC.Get(typeof(Cie10), int.Parse(s_diagnostico));
                ListItem oDia = new ListItem();
                oDia.Text = oC.Nombre;
                oDia.Value = oC.Id.ToString();
                lstDiagnosticosFinal.Items.Add(oDia);
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
                    case 1:
                        { btnGuardar.Visible = false; 
                        //    btnGuardarImprimir.Visible = false; 
                        }
                        break;
                }
            }
            else Response.Redirect(Page.ResolveUrl("~/FinSesion.aspx"), false);
        }
       
        //private void CargarDeterminacionesTurno()
        //{
        //    Turno oTurno = new Turno();
        //    oTurno = (Turno)oTurno.Get(typeof(Turno), int.Parse(Request["idTurno"].ToString()));

        //    ddlSectorServicio.SelectedValue = oTurno.IdSector.ToString();
        //    ddlEspecialista.SelectedValue = oTurno.IdEspecialistaSolicitante.ToString();
        //    ddlObraSocial.SelectedValue = oTurno.IdObraSocial.IdObraSocial.ToString();
        // //   ddlServicio.SelectedValue = oTurno.IdTipoServicio.IdTipoServicio.ToString();

        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(TurnoItem));
        //    crit.Add(Expression.Eq("IdTurno", oTurno));

        //    IList items = crit.List();
        //    string pivot = "";
        //    string sDatos = "";
        //    foreach (TurnoItem oDet in items)
        //    {
        //        if (pivot != oDet.IdItem.Nombre)
        //        {
        //            //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#false@";
        //            if (sDatos == "")
        //                sDatos = oDet.IdItem.Codigo + "#Si";
        //            else
        //                sDatos += ";" + oDet.IdItem.Codigo + "#Si" ;

        //            pivot = oDet.IdItem.Nombre;
        //        }
        //    }

        //    TxtDatosCargados.Value = sDatos;


        //    ///Agregar a la tabla las diagnosticos para mostrarlas en el gridview                             
        // //   dtDiagnosticos = (System.Data.DataTable)(Session["Tabla2"]);
        //    TurnoDiagnostico oDiagnostico = new TurnoDiagnostico();
        //    //ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit2 = m_session.CreateCriteria(typeof(TurnoDiagnostico));
        //    crit2.Add(Expression.Eq("IdTurno", oTurno));

        //    IList diagnosticos = crit2.List();

        //    foreach (TurnoDiagnostico oDiag in diagnosticos)
        //    {
        //        Cie10 oC = new Cie10();
        //        oC = (Cie10)oC.Get(typeof(Cie10), oDiag.IdDiagnostico);
        //        ListItem oDia = new ListItem();
        //        oDia.Text = oC.Nombre;
        //        oDia.Value = oC.Id.ToString();
        //        lstDiagnosticosFinal.Items.Add(oDia);            
        //    }                      
        //}

        private void MuestraDatos()
        {
            //Actualiza los datos de los objetos : alta o modificacion .
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));

            //if (oRegistro.IdPrioridad.IdPrioridad == 2)
            //    Session["idUrgencia"] = 2;
            //else
                Session["idUrgencia"] = 0;


            ///Cambiar de Paciente: LLeva a elección de un otro paciente para asignarlo al protocolo.
            hplModificarPaciente.NavigateUrl = "Default.aspx?Operacion=Modifica&idUrgencia="+Session["idUrgencia"].ToString()+"&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString()+"&idProtocolo=" + Request["idProtocolo"].ToString()+"&Desde="+ Request["Desde"].ToString();
            //Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=2", false); break;



            ////Modificacion de datos del paciente; retorna al protocolo.
            if (ConfigurationManager.AppSettings["urlPaciente"].ToString() != "0")
            {
                hplActualizarPaciente.Visible = false;
                //string s_urlLabo = ConfigurationManager.AppSettings["urlLabo"].ToString();
                //string s_urlModifica =s_urlLabo+ "Protocolos/ProtocoloEdit2.aspx?llamada=LaboProtocolo&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request.QueryString["idProtocolo"] + "&Desde=" + Request["Desde"].ToString();
                //hplActualizarPaciente.NavigateUrl = ConfigurationManager.AppSettings["urlPaciente"].ToString() + "/modifica?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&url='" + s_urlModifica + "'";
            }
            else
                hplActualizarPaciente.NavigateUrl = "../Pacientes/PacienteEdit.aspx?id=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&llamada=LaboProtocolo&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString() + "&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString();
      

            lblEstado.Visible = true;
            
            lblEstado.Text = VerEstado(oRegistro);
            


            if (oC.TipoNumeracionProtocolo==2)
            {
             //   lblTitulo.Text = oRegistro.PrefijoSector;
                ///Si es la numeracion es con letra no se puede modificar el prefijo sector.
                ddlSectorServicio.Enabled = false;
                ///////////////////////////
            }

            lblUsuario.Text = oRegistro.IdUsuarioRegistro.Username + " " + oRegistro.FechaRegistro.ToString();

            lblTitulo.Text +=  oRegistro.GetNumero().ToString();
            
            //ddlServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
            
            //CargarItems();
            txtFecha.Value = oRegistro.Fecha.ToShortDateString();
            txtFechaOrden.Value= oRegistro.FechaOrden.ToShortDateString();
            txtSala.Text = oRegistro.Sala;
            txtCama.Text = oRegistro.Cama;
            //txtHoraNac.Value = oRegistro.HoraNacimiento;
            //txtPesoNac.Value =  oRegistro.PesoNacimiento.ToString();
            //txtSemanaGestacion.Value = oRegistro.SemanaGestacion.ToString();

            txtNumeroOrigen.Text = oRegistro.NumeroOrigen;

            ///Datos del Paciente
            if (Request["idPaciente"] == null)
            {
                lblIdPaciente.Text = oRegistro.IdPaciente.IdPaciente.ToString();
                lblPaciente.Text = oRegistro.IdPaciente.NumeroDocumento.ToString() + " - " + oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();
                if (oRegistro.IdPaciente.IdEstado == 2)      lblPaciente.Text += " (Temporal) ";
                 
                lblFechaNacimiento.Text = "F.Nac.: " +  oRegistro.IdPaciente.FechaNacimiento.ToShortDateString();
                txtEdad.Value= oRegistro.Edad.ToString();
                switch (oRegistro.Sexo)
                {
                    case "F": ddlSexo.SelectedValue = "1"; break;
                    case "M": ddlSexo.SelectedValue = "2"; ; break;
                    case "I": ddlSexo.SelectedValue = "3"; ; break;
                }
               
                switch (oRegistro.UnidadEdad)
                {
                    case 0: lblUnidadEdad.Text = " A"; break;
                    case 1: lblUnidadEdad.Text = " M"; break;
                    case 2: lblUnidadEdad.Text = " D"; break;
                }                
                ddlObraSocial.SelectedValue=oRegistro.IdObraSocial.IdObraSocial.ToString();
            }
            else
            {
                MostrarPaciente();
            }
                      
       

            ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString(); 
            if (oRegistro.IdEfectorSolicitante!= oRegistro.IdEfector)
                CargarSolicitantesExternos("");
            else
                CargarSolicitantesInternos();

            ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();
            ddlEspecialista.SelectedValue=oRegistro.IdEspecialistaSolicitante.ToString();
                    

            ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();
                        
            ddlPrioridad.SelectedValue = oRegistro.IdPrioridad.IdPrioridad.ToString();


            ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
            txtObservacion.Text = oRegistro.Observacion;


         
            ISession m_session = NHibernateHttpModule.CurrentSession;         
            ProtocoloDiagnostico oDiagnostico = new ProtocoloDiagnostico();            
            ICriteria crit2 = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
            crit2.Add(Expression.Eq("IdProtocolo", oRegistro));

            IList diagnosticos = crit2.List();
         
            foreach (ProtocoloDiagnostico oDiag in diagnosticos)
            {
                Cie10 oCie10 = new Cie10();
                oCie10 = (Cie10)oCie10.Get(typeof(Cie10), oDiag.IdDiagnostico);

                ListItem oDia = new ListItem();
                oDia.Text = oCie10.Codigo + " - " + oCie10.Nombre;
                oDia.Value = oCie10.Id.ToString();
                lstDiagnosticosFinal.Items.Add(oDia);                 
            }

            SolicitudScreening oSolicitud = new SolicitudScreening();
            oSolicitud = (Business.Data.Laboratorio.SolicitudScreening)oSolicitud.Get(typeof(Business.Data.Laboratorio.SolicitudScreening), "IdProtocolo", oRegistro);

            this.PesquisaNeonatal1.MostrarSolicitudScreeening(oSolicitud);

          
            
            //chkRecordarConfiguracion.Checked = false;
            //chkRecordarPractica.Checked = false;
            

            //chkRecordarConfiguracion.Visible = false;
            //chkRecordarPractica.Visible = false;
            chkImprimir.Visible = false;
            chkRecordarConfiguracion.Visible = false;

            if (oRegistro.Estado==2)  btnGuardar.Visible = oC.ModificarProtocoloTerminado;          
        }

        private string VerEstado(Protocolo oRegistro)
        {
            string result = "";
            int p = oRegistro.Estado;
            if (!oRegistro.Baja)
            {
                if ((p == 1) || (p == 2))
                {
                    if (p == 1) result = "EN PROCESO";
                    else result = "TERMINADO";
                    lblEstado.ForeColor = System.Drawing.Color.Green;
                }
                if (p == 2)  //terminado
                { /// solo si está terminado no se puede modificar                
                    btnGuardar.Visible = false;
                    chkImprimir.Visible = false;
                    hplModificarPaciente.Enabled = false;
                    hplActualizarPaciente.Enabled = false;
                }
            }
            else
            {
                result = "ANULADO";
                lblEstado.ForeColor = System.Drawing.Color.Red;
                btnGuardar.Visible = false;
                chkImprimir.Visible = false;
                hplModificarPaciente.Enabled = false;
                hplActualizarPaciente.Enabled = false;
            }


            return result;
        }

      

        private void MostrarPaciente()
        {
            Utility oUtil = new Utility();
            ///Muestro los datos del paciente 
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));
            
            lblPaciente.Text = oPaciente.NumeroDocumento.ToString() + " - " + oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper();           
            lblPacienteNuevo.Text = oPaciente.NumeroDocumento.ToString() + " - " + oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper();
            lblIdPaciente.Text = oPaciente.IdPaciente.ToString();

            if (oPaciente.IdEstado == 2)
            {
                lblPaciente.Text += " (Temporal) ";
                lblPacienteNuevo.Text += " (Temporal) ";
            }
            //TimeSpan dif = DateTime.Now - DateTime.Parse(oPaciente.FechaNacimiento.ToString());
           // string edadpropuesta = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento);
            
            string[] edad = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento, DateTime.Now).Split(';');
                        txtEdad.Value = edad[0].ToString();
                        lblUnidadEdad.Text = " " + edad[1].ToUpper();
                    //    ddlEfector.SelectedValue = Origen[1].ToString();

            //txtEdad.Value = edadpropuesta.Split(;);
            lblFechaNacimiento.Text = "F.Nac.:" + oPaciente.FechaNacimiento.ToShortDateString();
            switch (oPaciente.IdSexo)
                {
                    case 1: ddlSexo.SelectedValue = "3"; break;    
                    case 2: ddlSexo.SelectedValue = "1"; break;
                    case 3: ddlSexo.SelectedValue= "2"; break;                    
                }   
        
            ddlObraSocial.SelectedValue = oPaciente.IdObraSocial.ToString();
            
        }

      
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            
            ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
            pnlMuestra.Visible = false;

            lblImprimeComprobantePaciente.Enabled = oC.GeneraComprobanteProtocolo;
            chkImprimir.Enabled = oC.GeneraComprobanteProtocolo;                                
            ddlImpresora.Enabled = oC.GeneraComprobanteProtocolo;
            lnkReimprimirComprobante.Enabled = oC.GeneraComprobanteProtocolo;
            lnkReimprimirCodigoBarras.Enabled = oC.GeneraComprobanteProtocolo;
                

            ///Carga de combos de tipos de servicios          
            TipoServicio oServicio = new TipoServicio();
            oServicio =(TipoServicio) oServicio.Get(typeof(TipoServicio), 4);
            lblServicio.Text = oServicio.Nombre;
            
            
            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,  prefijo + ' - ' + nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));


            /////////////////////////////////////////////CODIGO DE BARRAS//////////////////////////////////////////////////////////////////////
            ConfiguracionCodigoBarra oConfiguracion = new ConfiguracionCodigoBarra();
            oConfiguracion = (ConfiguracionCodigoBarra)oConfiguracion.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oServicio);
            if (oConfiguracion == null)
            {
                lblImprimeCodigoBarras.Visible = false;
                chkAreaCodigoBarra.Items.Clear();
                ddlImpresoraEtiqueta.Visible = false;
            }
            else
            {
                if (oConfiguracion.Habilitado)
                {
                    lblImprimeCodigoBarras.Visible = true;
                    ///cargar de areas con codigo de barras                            
                    m_ssql = "select idArea, nombre from Lab_Area WHERE imprimeCodigoBarra=1  order by nombre";
                    oUtil.CargarCheckBox(chkAreaCodigoBarra, m_ssql, "idArea", "nombre");
                    chkAreaCodigoBarra.Items.Insert(0, new ListItem("General", "0"));                
                }
                else
                {
                    lblImprimeCodigoBarras.Enabled = false;
                    chkAreaCodigoBarra.Items.Clear();
                    ddlImpresoraEtiqueta.Enabled = false;
                }
            }

            ///////////////Impresoras////////////////////////      
            m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
            oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
            oUtil.CargarCombo(ddlImpresoraEtiqueta, m_ssql, "nombre", "nombre");
           

            //////////////////////////FIN DE CODIGO DE BARRAS///////////////////////////////////////////////////////////////////////////////////                    

            ///Carga de combos de Origen
            m_ssql = " SELECT  idOrigen, nombre FROM LAB_Origen WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("", "0"));
                       

            ///Carga de combos de Prioridad
            m_ssql = " SELECT idPrioridad, nombre FROM LAB_Prioridad WHERE (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre");                                  
     

            ////////////Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();

            ////////////Carga de combos de Medicos Solicitantes
            if (Request["Operacion"].ToString()=="Carga")///muestra solo los activos
                m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional WHERE activo=1 ORDER BY apellido, nombre ";
            else
                m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional   ORDER BY apellido, nombre ";
            oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
            ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));


            //////////////////////////Carga de combos de ObraSocial//////////////////////////////////////////
            m_ssql = "SELECT idObraSocial,  nombre AS nombre FROM Sys_ObraSocial order by idObraSocial ";          
            oUtil.CargarCombo(ddlObraSocial, m_ssql, "idObraSocial", "nombre");
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();
            //////////////////////////////////////////////////////////////////////////////////////////////////


       
            if (Request["Operacion"].ToString() != "Modifica") IniciarValores(oC);

       
            if (oC.IdEfector.IdEfector.ToString() != ddlEfector.SelectedValue)
                CargarSolicitantesExternos("");
            else
                CargarSolicitantesInternos();                
           
            ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina           
            m_ssql = null;
            oUtil = null;
        }

        private void IniciarValores(Configuracion oC)
        {
            if (Session["ProtocoloLaboratorio"] != null)
            {
                string[] arr = Session["ProtocoloLaboratorio"].ToString().Split(("@").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        case "ddlMuestra":
                            {
                                
                                    if (Request["Operacion"].ToString() != "Modifica")
                                        if (Request["Operacion"].ToString() != "AltaTurno")
                                        {
                                            ddlMuestra.SelectedValue = s_control[1].ToString();
                                            mostrarCodigoMuestra();
                                        }
                            }
                            break;
                        case "ddlOrigen":
                            {
                                if ((oC.RecordarOrigenProtocolo))
                                    if (Request["Operacion"].ToString() != "Modifica")
                                        if (Request["Operacion"].ToString() != "AltaTurno")
                                        {
                                            ddlOrigen.SelectedValue = s_control[1].ToString();
                                        }
                            }
                            break;
                        case "ddlEfector":
                            if ((oC.RecordarOrigenProtocolo))
                                if (Request["Operacion"].ToString() != "Modifica")
                                    if (Request["Operacion"].ToString() != "AltaTurno")
                                    {
                                        ddlEfector.SelectedValue = s_control[1].ToString();
                                    }
                            break;
                        case "ddlSectorServicio":
                            {
                                if ((oC.RecordarSectorProtocolo))
                                    if (Request["Operacion"].ToString() != "Modifica")
                                        if (Request["Operacion"].ToString() != "AltaTurno")
                                        {
                                            ddlSectorServicio.SelectedValue = s_control[1].ToString();
                                        }
                            }
                            break;
                        case "chkRecordarConfiguracion":
                            {
                                if (s_control[1].ToString() == "False")
                                    chkRecordarConfiguracion.Checked = false;
                                else
                                    chkRecordarConfiguracion.Checked = true;
                            }
                            break;

                        case "chkImprimir":
                            {
                                if (s_control[1].ToString() == "False")
                                    chkImprimir.Checked = false;
                                else
                                    chkImprimir.Checked = true;
                            }
                            break;

                        case "chkAreaCodigoBarra":
                            {

                                string[] arrSector = s_control[1].ToString().Split((",").ToCharArray());
                                foreach (string itemSector in arrSector)
                                {
                                    for (int j = 0; j < arrSector.Length; j++)
                                    {
                                        for (int i = 0; i < chkAreaCodigoBarra.Items.Count; i++)
                                        {
                                            if (arrSector[j].ToString() != "")
                                            {
                                                if (int.Parse(chkAreaCodigoBarra.Items[i].Value) == int.Parse(arrSector[j].ToString()))
                                                    chkAreaCodigoBarra.Items[i].Selected = true;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "chkRecordarPractica":
                            {
                                //if (s_control[1].ToString() == "False")
                                //    chkRecordarPractica.Checked = false;
                                //else
                                //    chkRecordarPractica.Checked = true;
                            }
                            break;
                        case "prácticas":
                            TxtDatosCargados.Value = s_control[1].ToString(); break;
                        case "ddlImpresora":
                            ddlImpresora.SelectedValue = s_control[1].ToString(); break;

                        case "ddlImpresoraEtiqueta":
                            ddlImpresoraEtiqueta.SelectedValue = s_control[1].ToString(); break;
                    }
                }
            }
            else
            {
                if (Session["Impresora"]!=null) ddlImpresora.SelectedValue=Session["Impresora"].ToString();
                if (Session["Etiquetadora"]!=null)  ddlImpresoraEtiqueta.SelectedValue=Session["Etiquetadora"] .ToString();
            }

        }
   
      

       
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { ///Verifica si se trata de un alta o modificacion de protocolo               
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                if (Request["Operacion"].ToString() == "Modifica") oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

              
                Guardar(oRegistro);

                if ((Request["Operacion"].ToString() == "Alta") || 
                    (Request["Operacion"].ToString() == "AltaTurno") ||
                    (Request["Operacion"].ToString() == "AltaDerivacion")||
                    (Request["Operacion"].ToString() == "AltaPeticion"))
                {
                 
                    ///Imprimir codigo de barras.
                    string s_AreasCodigosBarras = getListaAreasCodigoBarras();
                    if (s_AreasCodigosBarras != "")                    
                        ImprimirCodigoBarras(oRegistro, getListaAreasCodigoBarras());


                    ////Imprimir Comprobante para el paciente
                    if (chkImprimir.Checked) Imprimir(oRegistro);

                    //////////////////////////
                }

//                EnviarProtocoloEquipo_DescargaManual();///Pone el protocolo en la tabla 

                //if ((Request["Operacion"].ToString() == "AltaPeticion") && (Request["idPeticion"] != null))
                //    ActualizarPeticion(Request["idPeticion"].ToString(), oRegistro);

                if (Request["idTurno"] != null)
                    ActualizarTurno(Request["idTurno"].ToString(), oRegistro);
                //if (Request["idSolicitudScreening"] != null) { Response.Redirect("../Neonatal/Default.aspx"); }

                //if (oC.GeneraComprobanteProtocolo)
                //    Response.Redirect("ProtocoloMensaje.aspx?id=" + oRegistro.IdProtocolo.ToString(), false);
                //else
                if (Request["Operacion"].ToString() == "Modifica")
                {                    
                        switch (Request["Desde"].ToString())
                        {
                            case "DerivacionScreening": Response.Redirect("DerivacionScreening.aspx", false); break;
                            case "ProtocoloList": Response.Redirect("ProtocoloList.aspx?idServicio=4&Tipo=Lista"); break;
                            case "Control": Avanzar(1); break;
                            //case "Urgencia": Response.Redirect("../Urgencia/UrgenciaList.aspx", false); break;

                        }
                    
                }
                else
                {
                    if (Request["Operacion"].ToString() == "AltaDerivacion")
                        Response.Redirect("DerivacionScreening.aspx");
                    else
                    {
                        if (Request["Operacion"].ToString() == "AltaTurno")
                            Response.Redirect("../turnos/Turnolist.aspx?ultimoProtocolo=" + oRegistro.IdProtocolo.ToString(), false);
                        else
                        {
                            if (Request["Operacion"].ToString() == "AltaPeticion")
                                Response.Redirect("../PeticionElectronica/PeticionList.aspx", false);
                            else
                            {
                                if (Session["idServicio"].ToString() == "3") { Session["idUrgencia"] = "0"; }
                                if (Session["idUrgencia"] != null)
                                {
                                    switch (Session["idUrgencia"].ToString())
                                    {
                                        case "1": /// va directo a la carga de protocolo      
                                            {
                                                Session["Parametros"] = oRegistro.IdProtocolo.ToString();
                                                Response.Redirect("../resultados/ResultadoEdit2.aspx?idServicio=1&Operacion=Carga&idProtocolo=" + oRegistro.IdProtocolo.ToString() + "&Index=0&Parametros=" + oRegistro.IdProtocolo.ToString() + "&idArea=0&urgencia=1&validado=0&modo=Normal");
                                            }
                                            break;
                                        case "2": // va a la carga de otro protocolo con urgencia
                                            Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=2", false); break;
                                        case "0":
                                            Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false); break;
                                    }
                                }
                                else
                                    Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                            }
                        }
                    }
                }
            }
            
        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtNumeroOrigen.Text != "") { if (oUtil.EsEntero(txtNumeroOrigen.Text)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

        private string getListaAreasCodigoBarras()
        {
            string lista= "";
            for (int i = 0; i<chkAreaCodigoBarra.Items.Count; i++)
            { 
                if (chkAreaCodigoBarra.Items[i].Selected)
                {
                    if (lista == "")
                        lista = chkAreaCodigoBarra.Items[i].Value;
                    else
                        lista += ","+chkAreaCodigoBarra.Items[i].Value;
                }
            }
            return lista;
        }

        private void ImprimirCodigoBarras(Protocolo oProt, string s_listaAreas)
        {
            oProt.GrabarAuditoriaProtocolo("Imprime Código de Barras", int.Parse(Session["idUsuario"].ToString()));       
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();   oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oProt.IdTipoServicio);

            string sFuenteBarCode = oConBarra.Fuente;
            bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
            bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
            bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
            bool imprimeProtocoloNumeroOrigen = oConBarra.ProtocoloNumeroOrigen;
            bool imprimePacienteNumeroDocumento = oConBarra.PacienteNumeroDocumento;
            bool imprimePacienteApellido = oConBarra.PacienteApellido;
            bool imprimePacienteSexo = oConBarra.PacienteSexo;
            bool imprimePacienteEdad = oConBarra.PacienteEdad;
            bool adicionalGeneral = false;
            if (s_listaAreas.Substring(0, 1) == "0") adicionalGeneral = true;
                                    

           

//             string m_strSQL =  @" SELECT [idProtocolo]      ,[idArea]      ,[numero]      ,[area]
//            ,[Fecha]      ,[Origen]      ,[Sector]      ,[NumeroOrigen]      ,[NumeroDocumento]      ,[apellido]      ,[Sexo]      ,[edad]
//            FROM vta_LAB_GeneraCodigoBarras
//            WHERE    (idArea IN (" + s_listaAreas + ")) AND idProtocolo =" + this.IdProtocolo.ToString();

     
            DataTable Dt= new DataTable();
            Dt =(DataTable) oProt.GetDataSetCodigoBarras("Protocolo", s_listaAreas, oProt.IdTipoServicio.IdTipoServicio, adicionalGeneral);
            foreach (DataRow item in Dt.Rows)
            {              
                ///Desde acá impresion de archivos
                string reg_numero = item[2].ToString();
                string reg_area = item[3].ToString();
                string reg_Fecha = item[4].ToString().Substring(0, 10);
                string reg_Origen = item[5].ToString();
                string reg_Sector = item[6].ToString();
                string reg_NumeroOrigen = item[7].ToString();
                string reg_NumeroDocumento = oProt.IdPaciente.NumeroDocumento.ToString();// item[8].ToString();

                string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);

                string reg_apellido = "";
                if (chkCodificaPaciente.Checked)
                {
                    reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
                }
                else
                {
                    if (reg_codificaHIV == "FALSE")
                        reg_apellido = oProt.IdPaciente.Apellido + " " + oProt.IdPaciente.Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
                    else
                        reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
                }
                    //reg_apellido = item[12].ToString().ToUpper();
                string reg_sexo = item[10].ToString();
                string reg_edad = item[11].ToString();
                //tabla.Rows.Add(reg);
                //tabla.AcceptChanges();


                if (!imprimeProtocoloFecha) reg_Fecha = "          ";
                if (!imprimeProtocoloOrigen) reg_Origen = "          ";
                if (!imprimeProtocoloSector) reg_Sector = "   ";
                if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
                if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
                if (!imprimePacienteApellido) reg_apellido = "";
                if (!imprimePacienteSexo) reg_sexo = " ";
                if (!imprimePacienteEdad) reg_edad = "   ";
                //ParameterDiscreteValue fuenteCodigoBarras = new ParameterDiscreteValue(); fuenteCodigoBarras.Value = oConBarra.Fuente;


                Business.Etiqueta ticket = new Business.Etiqueta();
                if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);

                ticket.AddHeaderLine(reg_apellido.ToUpper());
                //ticket.AddSubHeaderLine(reg_apellido.ToUpper());
                ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha);
                ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);// reg_Sector);
                ticket.AddSubHeaderLineNegrita(reg_area);
                //ticket.AddSubHeaderLine(reg_area);

                ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
                ticket.AddFooterLine(reg_numero); // + "  " + reg_NumeroOrigen);

                Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;
                ticket.PrintTicket(ddlImpresoraEtiqueta.SelectedValue, oConBarra.Fuente);
                /////fin de impresion de archivos
            }
        
        }
        private void Imprimir(Protocolo oProt)
        {

            ///////////////
            Business.Reporte ticket = new Business.Reporte();

            string textoAdicional = "";
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra(); oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oProt.IdTipoServicio);

            string sFuenteBarCode = oConBarra.Fuente;
            string reg_numero = oProt.GetNumero();

            if (oProt.IdTipoServicio.IdTipoServicio == 1)
                textoAdicional = oCon.TextoAdicionalComprobanteProtocolo;
            if (oProt.IdTipoServicio.IdTipoServicio == 3)
                textoAdicional = oCon.TextoAdicionalComprobanteProtocoloMicrobiologia;


            DataTable dt = oProt.GetDataSetComprobante();
            string analisis = dt.Rows[0][7].ToString();
            string s_hiv = dt.Rows[0][13].ToString();
            string paciente = "";
            if (s_hiv != "False") paciente = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
            else paciente = oProt.IdPaciente.Apellido.ToUpper() + " " + oProt.IdPaciente.Nombre.ToUpper();

            ticket.AddHeaderLine("LABORATORIO " + oCon.EncabezadoLinea1);
            ticket.AddSubHeaderLine("_____________________________________________________________________________________________");
            ticket.AddSubHeaderLine("PROTOCOLO Nro. " + reg_numero + "         Fecha: " + oProt.Fecha.ToShortDateString() + "              Fecha de Entrega: " + oProt.FechaRetiro.ToShortDateString());
            ticket.AddSubHeaderLine(" ");
            ticket.AddSubHeaderLine(paciente.ToUpper() + "                       DU:" + oProt.IdPaciente.NumeroDocumento.ToString() + "      Fecha de Nacimiento:" + oProt.IdPaciente.FechaNacimiento.ToShortDateString() + "      SEXO:" + oProt.IdPaciente.getSexo());

            

            ticket.AddSubHeaderLine("_____________________________________________________________________________________________");
            
            int largo = analisis.Length;
            int cantidadFilas = largo / 90;
            if (cantidadFilas >= 0)
            {
                ticket.AddSubHeaderLine("PRACTICAS SOLICITADAS");
                for (int i = 1; i <= cantidadFilas; i++)
                {
                    int l = i * 90;
                    analisis = analisis.Insert(l, "&");

                }
                string[] tabla = analisis.Split('&');

                /////Crea nuevamente los detalles.
                for (int i = 0; i <= tabla.Length - 1; i++)
                {
                    ticket.AddSubHeaderLine("     " + tabla[i].ToUpper());
                }
            
            } 
            ticket.AddSubHeaderLine("_____________________________________________________________________________________________");

            ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
            //ticket.AddFooterLine(reg_numero);

            ticket.AddFooterLine("******************************" + textoAdicional);



            Session["Impresora"] = ddlImpresora.SelectedValue;

            ticket.PrintTicket(ddlImpresora.SelectedValue, oConBarra.Fuente);
            /////fin de impresion de archivos
        }




        private void ActualizarTurno(string p, Business.Data.Laboratorio.Protocolo oRegistro)
        {
            Turno oTurno = new Turno();
            oTurno = (Turno)oTurno.Get(typeof(Turno),int.Parse( p));
            oTurno.IdProtocolo = oRegistro.IdProtocolo;
            oTurno.Save();
        }

        //private void ActualizarPeticion(string p, Protocolo oRegistro)
        //{
        //    Peticion oPeticion = new Peticion();
        //    oPeticion = (Peticion)oPeticion.Get(typeof(Peticion), int.Parse(p));
        //    oPeticion.IdProtocolo = oRegistro.IdProtocolo;
        //    oPeticion.Save();
        //}
        private void Guardar(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            if (IsTokenValid())
            {
                TEST++;
                //Actualiza los datos de los objetos : alta o modificacion .
                Efector oEfector = new Efector();
                Usuario oUser = new Usuario();

                Paciente oPaciente = new Paciente();
                ObraSocial oObra = new ObraSocial();
                Origen oOrigen = new Origen();
                Prioridad oPri = new Prioridad();
                DateTime fecha = DateTime.Parse(txtFecha.Value);

                Configuracion oC = new Configuracion();
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

                oRegistro.IdEfector = oC.IdEfector;
                SectorServicio oSector = new SectorServicio();
                oSector = (SectorServicio)oSector.Get(typeof(SectorServicio), int.Parse(ddlSectorServicio.SelectedValue));
                oRegistro.IdSector = oSector;

                TipoServicio oServicio = new TipoServicio(); oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), 4);
                oRegistro.IdTipoServicio = oServicio;


                if (Request["Operacion"].ToString() != "Modifica")
                {

                    oRegistro.Numero = 0; // oRegistro.GenerarNumero();
                    //oRegistro.NumeroDiario = oRegistro.GenerarNumeroDiario(fecha.ToString("yyyyMMdd"));
                    //oRegistro.PrefijoSector = oSector.Prefijo.Trim();
                    //oRegistro.NumeroSector = oRegistro.GenerarNumeroGrupo(oSector);
                    //oRegistro.NumeroTipoServicio = oRegistro.GenerarNumeroTipoServicio(oServicio);
                }


                oRegistro.Fecha = DateTime.Parse(txtFecha.Value);
                oRegistro.FechaOrden = DateTime.Parse(txtFechaOrden.Value);
                oRegistro.FechaRetiro = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);

                oRegistro.IdEfectorSolicitante = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                oRegistro.IdEspecialistaSolicitante = int.Parse(ddlEspecialista.SelectedValue);


                ///Desde aca guarda los datos del paciente en Protocolo
                oRegistro.IdPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(lblIdPaciente.Text));
                oRegistro.Edad = int.Parse(txtEdad.Value);
                //if (txtHoraNac.Value!="")oRegistro.HoraNacimiento = txtHoraNac.Value;
                //if (txtPesoNac.Value!="") oRegistro.PesoNacimiento = int.Parse(txtPesoNac.Value, System.Globalization.CultureInfo.InvariantCulture);
                //if (txtSemanaGestacion.Value!="") oRegistro.SemanaGestacion = int.Parse(txtSemanaGestacion.Value);
                if (txtNumeroOrigen.Text != "") oRegistro.NumeroOrigen = txtNumeroOrigen.Text;

                switch (lblUnidadEdad.Text.Trim())
                {
                    case "A": oRegistro.UnidadEdad = 0; break;
                    case "M": oRegistro.UnidadEdad = 1; break;
                    case "D": oRegistro.UnidadEdad = 2; break;
                }

                switch (ddlSexo.SelectedValue)
                {
                    case "1": oRegistro.Sexo = "F"; break;
                    case "2": oRegistro.Sexo = "M"; break;
                    case "3": oRegistro.Sexo = "I"; break;
                }


                oRegistro.Embarazada = "N";
                oRegistro.Sala = txtSala.Text;
                oRegistro.Cama = txtCama.Text;

                oRegistro.IdObraSocial = (ObraSocial)oObra.Get(typeof(ObraSocial), int.Parse(this.ddlObraSocial.SelectedValue));
                oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), int.Parse(this.ddlOrigen.SelectedValue));
                oRegistro.IdPrioridad = (Prioridad)oPri.Get(typeof(Prioridad), int.Parse(this.ddlPrioridad.SelectedValue));
                oRegistro.Observacion = txtObservacion.Text;
                oRegistro.ObservacionResultado = "";
                oRegistro.IdMuestra = int.Parse(ddlMuestra.SelectedValue);
                if (Request["Operacion"].ToString() != "Modifica")
                {
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;
                }

                

                oRegistro.Save();
                oRegistro.ActualizarNumeroDesdeID();

                if (Request["Operacion"].ToString() != "Modifica") AlmacenarSesion(oC);

                //if (Request["Operacion"].ToString() != "Modifica") { if (Session["idUrgencia"] != null) { if (Session["idUrgencia"].ToString() == "0") AlmacenarSesion(oC); } }
                //if (Request["Operacion"].ToString() == "AltaDerivacion") AlmacenarSesion(oC);

                //   if (Request["idSolicitudScreening"] != null) ActualizarSolicitudScreening(Request["idSolicitudScreening"].ToString(),oRegistro);

                GuardarDetalle(oRegistro);
                GuardarDiagnosticos(oRegistro);
                PesquisaNeonatal1.GuardarSolicitudScreeening(oRegistro, 0, Request["Operacion"].ToString());
                //int id_solicitudScreening = 0;
                //if (Request["idSolicitudScreening"] != null) id_solicitudScreening = int.Parse(Request["idSolicitudScreening"]);

                //if (Request["Operacion"].ToString() != "Modifica") PesquisaNeonatal1.GuardarSolicitudScreeening(oRegistro, 0);
                //else { ///modifica la sollictud de screening 
                //    SolicitudScreening oSolicitud = new SolicitudScreening();
                //    oSolicitud = (SolicitudScreening)oSolicitud.Get(typeof(SolicitudScreening),"IdProtocolo",oRegistro);                       
                //    if (oSolicitud!=null)
                //        PesquisaNeonatal1.GuardarSolicitudScreeening(oRegistro, oSolicitud.IdSolicitudScreening);
                //}


                //if (oC.TipoNumeracionProtocolo == 0) /// solo si es tipo de numeracion: numero (Castro Rendon)
                //{ 
                //oRegistro.VerificarExisteNumeroAsignado();
                //}
                oRegistro.GrabarAuditoriaProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));

                /////////////////inicio try /////////////////
                try
                {
                    ////confirma en el sip la recepcion de la tarjeta
                    if (Request["Operacion"].ToString() != "Modifica")
                    {
                        if (oRegistro.NumeroOrigen != "")
                        {
                            string s_urlWFC = ConfigurationManager.AppSettings["WSLaboratorio"].ToString();
                            s_urlWFC = s_urlWFC + "/ConfirmarRecepcionTarjeta?numeroProtocolo=" + oRegistro.NumeroOrigen + "&s_usuario=" + oRegistro.IdUsuarioRegistro.Username;
                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(s_urlWFC);
                            HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();

                            Stream st = ws1.GetResponseStream();
                            StreamReader sr = new StreamReader(st);
                            string s = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string exception = "";
                    while (ex != null)
                    {
                        exception = ex.Message + "<br>";

                    }
                }
                /////////////////fin try /////////////////
              
            }
            else
            { //doble submit
            }
        }

        //private void ActualizarSolicitudScreening(string p, Protocolo oProtocolo)
        //{
        //    SolicitudScreening oRegistro = new SolicitudScreening();
        //    oRegistro = (SolicitudScreening)oRegistro.Get(typeof(SolicitudScreening), int.Parse(p));
        //    oRegistro.IdProtocolo = oProtocolo.IdProtocolo;
        //    oRegistro.Save();
        //}

        private void AlmacenarSesion(Configuracion oC)
        {


            string s_valores = ""; // "chkRecordarPractica:" + chkRecordarPractica.Checked;
            
            if (chkRecordarConfiguracion.Checked)
            {
                s_valores += "@chkImprimir:" + chkImprimir.Checked;
                s_valores += "@chkAreaCodigoBarra:" + getListaAreasCodigoBarras();
            }
            s_valores += "@chkRecordarConfiguracion:" + chkRecordarConfiguracion.Checked;
            s_valores += "@ddlImpresora:" + ddlImpresora.SelectedValue;
            s_valores += "@ddlImpresoraEtiqueta:" + ddlImpresoraEtiqueta.SelectedValue;

            Session["Impresora"] = ddlImpresora.SelectedValue;
            Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;

            if (oC.RecordarOrigenProtocolo)
                if (Request["Operacion"].ToString() != "Modifica")
                    if (Request["Operacion"].ToString() != "AltaTurno")
                    {
                        s_valores += "@ddlOrigen:" + ddlOrigen.SelectedValue;
                        s_valores += "@ddlEfector:" + ddlEfector.SelectedValue;
                        if (ddlMuestra.SelectedValue!="0") s_valores += "@ddlMuestra:" + ddlMuestra.SelectedValue;

                    }
            if (oC.RecordarSectorProtocolo)
                if (Request["Operacion"].ToString() != "Modifica")
                    if (Request["Operacion"].ToString() != "AltaTurno")
                    {
                        s_valores += "@ddlSectorServicio:" + ddlSectorServicio.SelectedValue;            
                    }                                                                                      

               
            Session["ProtocoloLaboratorio"] = s_valores;
        
        }

        private void GuardarDiagnosticos(Business.Data.Laboratorio.Protocolo oRegistro)
        {
         //   dtDiagnosticos = (System.Data.DataTable)(Session["Tabla2"]);
            ///Eliminar los detalles y volverlos a crear
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (ProtocoloDiagnostico oDetalle in detalle)
                {
                    Cie10 oCie10= new Cie10(oDetalle.IdDiagnostico);
                    oDetalle.Delete();
                    oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", oCie10.Nombre);
                }

            }

            ///Busca en la lista de diagnosticos buscados
            if (lstDiagnosticosFinal.Items.Count > 0)
            {             
                /////Crea nuevamente los detalles.
                for (int i = 0; i < lstDiagnosticosFinal.Items.Count; i++)
                {
                    ProtocoloDiagnostico oDetalle = new ProtocoloDiagnostico();
                    oDetalle.IdProtocolo = oRegistro;
                    oDetalle.IdEfector = oRegistro.IdEfector;
                    oDetalle.IdDiagnostico = int.Parse(lstDiagnosticosFinal.Items[i].Value);
                    oDetalle.Save();                    
                }
            }

        
        }

        //private void GuardarSolicitudScreeening(Business.Data.Laboratorio.Protocolo oRegistro, int idsolicitudscreening)
        //{

         

           
        //}


        private void GuardarDetalle(Business.Data.Laboratorio.Protocolo oRegistro)
        {         
            int dias_espera = 0;
            string aux1 = "";            
            CheckBoxList a = (CheckBoxList)(this.PesquisaNeonatal1.FindControl("chkItemPesquisa"));
            if (a != null)
            {
                for (int i = 0; i < a.Items.Count; i++)
                {
                    if (a.Items[i].Selected)
                    {
                        if (aux1 == "")
                            aux1 =i.ToString()+"#"+ a.Items[i].Text + "#Si#False";
                        else
                            aux1 = aux1 + "@" + i.ToString() + "#" + a.Items[i].Text + "#Si#False";
                    }
                }
                aux1 = aux1 + "@";
            }
            string[] tabla = aux1.Split('@'); //TxtDatos.Value.Split('@'); 
            ISession m_session = NHibernateHttpModule.CurrentSession;

            string recordar_practicas = "";

            for (int i = 0; i < tabla.Length-1; i++)
            {
                string[] fila = tabla[i].Split('#');


                string codigo = fila[1].ToString();

                
                if (recordar_practicas == "")
                    recordar_practicas = codigo +"#Si#False";
                else
                    recordar_practicas += ";" + codigo + "#Si#False";

                if (codigo != "")
                {
                    Item oItem = new Item();                                        
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo,"Baja",false);

                  string trajomuestra = fila[3].ToString();

                    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                    crit.Add(Expression.Eq("IdProtocolo", oRegistro));
                    crit.Add(Expression.Eq("IdItem", oItem));
                    IList listadetalle = crit.List();
                    if (listadetalle.Count == 0)
                    { //// si no está lo agrego --- si ya está no hago nada


                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        //Item oItem = new Item();
                        oDetalle.IdProtocolo = oRegistro;
                        oDetalle.IdEfector = oRegistro.IdEfector;

                       

                        oDetalle.IdItem = oItem; // (Item)oItem.Get(typeof(Item), "Codigo", codigo);

                        if (dias_espera < oDetalle.IdItem.Duracion) dias_espera = oDetalle.IdItem.Duracion;

                        /*CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                        if (a.Checked)
                            oDetalle.TrajoMuestra = "Si";
                        else*/

                        if (trajomuestra == "true")
                            oDetalle.TrajoMuestra = "No";
                        else
                            oDetalle.TrajoMuestra = "Si";


                        oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                        oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                        oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");


                        GuardarDetallePractica(oDetalle);
                    }
                    else  //si ya esta actualizo si trajo muestra o no
                    {
                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {
                            if (trajomuestra == "true")
                                oDetalle.TrajoMuestra = "No";
                            else
                                oDetalle.TrajoMuestra = "Si";

                            oDetalle.Save();
                        }

                    }
                }
            }

            //if (Request["Operacion"].ToString() != "Modifica")
            //{
            //    if (chkRecordarPractica.Checked)
            //        Session["ProtocoloLaboratorio"] += "@prácticas:" + recordar_practicas;
            //}



            if (Request["Operacion"].ToString() == "Modifica")
            {
                // Modificacion de protocolo en proceso: Elimina los detalles que no se ingresaron por pantalla         
                //  ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria critBorrado = m_session.CreateCriteria(typeof(DetalleProtocolo));
                critBorrado.Add(Expression.Eq("IdProtocolo", oRegistro));
                IList detalleaBorrar = critBorrado.List();
                if (detalleaBorrar.Count > 0)
                {
                    foreach (DetalleProtocolo oDetalle in detalleaBorrar)
                    {
                        bool noesta = true;
                        //oDetalle.Delete();                     
                        //string[] tablaAux = TxtDatos.Value.Split('@');
                        for (int i = 0; i < tabla.Length - 1; i++)
                        {
                            string[] fila = tabla[i].Split('#');
                            string codigo = fila[1].ToString();
                            if (codigo != "")
                            {
                                if (codigo == oDetalle.IdItem.Codigo) noesta = false;

                            }
                        }
                        if (noesta)
                        {
                            oDetalle.Delete();
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()));
                        }
                    }
                }
            }

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        
            //if (oCon.TipoCalculoDiasRetiro == 0)

            if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
                if (oCon.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(dias_espera));
                else   // calcula con los días predeterminados de espera
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oCon.DiasRetiro));
            else
                oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);




            oRegistro.Save();


        }
        private void GuardarDetalle2(Business.Data.Laboratorio.Protocolo oRegistro)
        {                           
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {              
                foreach (DetalleProtocolo oDetalle in detalle)
                {
                    oDetalle.Delete();
                }                
            }

       
            int dias_espera = 0;
            string[] tabla = TxtDatos.Value.Split('@');

            for (int i = 0; i < tabla.Length - 1; i++)
            {
                string[] fila = tabla[i].Split('#');


                string codigo = fila[1].ToString();
                if (codigo!="")
                {
                    DetalleProtocolo oDetalle = new DetalleProtocolo();
                    Item oItem = new Item();
                    oDetalle.IdProtocolo = oRegistro;
                    oDetalle.IdEfector = oRegistro.IdEfector;

                    string trajomuestra = fila[3].ToString();

                    oDetalle.IdItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo);

                    if (dias_espera < oDetalle.IdItem.Duracion) dias_espera = oDetalle.IdItem.Duracion;

                    /*CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked)
                        oDetalle.TrajoMuestra = "Si";
                    else*/

                    if (trajomuestra=="true")
                        oDetalle.TrajoMuestra = "No";
                    else
                        oDetalle.TrajoMuestra = "Si";


                    oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                    oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                    oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                    oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                    oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                    oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                    oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                    GuardarDetallePractica(oDetalle);
                }
            }
         

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //    DateTime fechaentrega;
            //if (oCon.TipoCalculoDiasRetiro == 0)

            if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
                if (oCon.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(dias_espera)  );
                else   // calcula con los días predeterminados de espera
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oCon.DiasRetiro)  );                                                       
            else
                oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);  
            
            
 
            
            oRegistro.Save();
          
          
        }

       

        private void GuardarDetallePractica(DetalleProtocolo oDet)
        {
            if (oDet.IdItem.IdEfector.IdEfector != oDet.IdItem.IdEfectorDerivacion.IdEfector) //Si es un item derivable no busca hijos y guarda directamente.
            {
                 oDet.IdSubItem = oDet.IdItem;
                            oDet.Save();      
            }
            else
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                crit.Add(Expression.Eq("IdItemPractica", oDet.IdItem));
                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    int i = 1;
                    foreach (PracticaDeterminacion oSubitem in detalle)
                    {
                        if (oSubitem.IdItemDeterminacion != 0)
                        { 
                            Item oSItem = new Item();
                            if (i == 1)
                            {                                                     
                                oDet.IdSubItem = (Item)oSItem.Get(typeof(Item), oSubitem.IdItemDeterminacion); 
                                oDet.Save();                                                   
                            }
                            else
                            {
                                 DetalleProtocolo oDetalle = new DetalleProtocolo();                           
                                 oDetalle.IdProtocolo =oDet.IdProtocolo;
                                 oDetalle.IdEfector = oDet.IdEfector;
                                 oDetalle.IdItem = oDet.IdItem;
                                 oDetalle.IdSubItem = (Item)oSItem.Get(typeof(Item), oSubitem.IdItemDeterminacion);
                                 oDetalle.TrajoMuestra = oDet.TrajoMuestra;

                                 oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");

                                 oDetalle.Save();
                            }
                            i = i + 1;                        
                        }//fin if
                    }//fin foreach
                }         
                else
                {
                    oDet.IdSubItem = oDet.IdItem;
                    oDet.Save();      
                }//fin   if (detalle.Count > 0)  
            }

         
            
        }



        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si el sexo es femenino se habilita la selecció de Embarazada
           // HabilitarEmbarazada();
        }

  

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
           
        }

        //protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ///////Con la selección del item se muestra el codigo
        //    if (ddlItem.SelectedValue != "0")
        //    {
        //        Item oItem = new Item();
        //        oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
        //        txtCodigo.Text = oItem.Codigo;
                
        //    }
        //    else
        //        txtCodigo.Text = "";

        //    txtCodigo.UpdateAfterCallBack = true;
            

        //}



        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[1].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modificar";
            }
        }

      

        protected void btnAgregarDiagnostico_Click(object sender, EventArgs e)
        {
          

            AgregarDiagnostico();
        }

        private void AgregarDiagnostico()
        {
            if (lstDiagnosticos.SelectedValue != "")
            {
                lstDiagnosticosFinal.Items.Add(lstDiagnosticos.SelectedItem);
                lstDiagnosticosFinal.UpdateAfterCallBack = true;
            }
        }


        private void SacarDiagnostico()
        {
            if (lstDiagnosticosFinal.SelectedValue != "")
            {
                lstDiagnosticosFinal.Items.Remove(lstDiagnosticosFinal.SelectedItem);
                lstDiagnosticosFinal.UpdateAfterCallBack = true;
            }
        }


       

        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {
         
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (Request["Operacion"].ToString() == "Modifica")
            {
                if (Request["DesdeUrgencia"] != null)
                    Response.Redirect("../Urgencia/UrgenciaList.aspx");
                else
                {
                    switch (Request["Desde"].ToString())
                    {
                        case "Default": Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
                        case "ProtocoloList": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Lista"); break;
                        case "Control": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Control"); break;
                        case "Urgencia": Response.Redirect("../Urgencia/UrgenciaList.aspx",false); break;
                        case "Derivacion": Response.Redirect("DerivacionScreening.aspx", false); break;
                    }
                    
                    
                    //if (Request["Control"] != null)
                    //    Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Control");
                    //else
                    //    Response.Redirect("ProtocoloList.aspx?idServicio="+ Session["idServicio"].ToString()+"&Tipo=Lista");
                }
            }
            else
            {
                if (Request["Operacion"].ToString() == "AltaTurno")
                    Response.Redirect("../turnos/Turnolist.aspx", false);
                else
                {
                    if (Request["Operacion"].ToString() == "AltaDerivacion")
                        Response.Redirect("DerivacionScreening.aspx", false);
                    else
                    {
                        if (Request["Operacion"].ToString() == "AltaPeticion")
                            Response.Redirect("../PeticionElectronica/PeticionList.aspx", false);
                        else
                        {
                            if (Session["idUrgencia"].ToString() != "0")
                                Response.Redirect("Default.aspx?idServicio=1&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                            else
                                Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                        }
                    }
                }
            }
        }
      

    

        protected void btnAgregarRutina_Click(object sender, EventArgs e)
        {
           // if (ddlRutina.SelectedValue != "0")
               // AgregarRutina();           
           
        }

        

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {

            SelectedEfector();
        }


        protected void ddlMuestra_SelectedIndexChanged(object sender, EventArgs e)
        {
         
                mostrarCodigoMuestra();
          
        }

        private void mostrarCodigoMuestra()
        {
            if (ddlMuestra.SelectedValue != "0")
            {
                Muestra oMuestra = new Muestra();
                oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), int.Parse(ddlMuestra.SelectedValue));
                if (oMuestra != null) txtCodigoMuestra.Text = oMuestra.Codigo;
                txtCodigoMuestra.UpdateAfterCallBack = true;
            }
        }

        private void SelectedEfector()
        {
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            if (ddlEfector.SelectedValue != oC.IdEfector.IdEfector.ToString())
            {
                CargarSolicitantesExternos("");

               
                Efector oEfectorExterno = new Efector();
                oEfectorExterno = (Efector)oEfectorExterno.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                switch (oEfectorExterno.IdZona.IdZona)
                {
                    case 1: // subse
                        ddlOrigen.SelectedValue = "5"; break;
                    case 2: // zona I
                        ddlOrigen.SelectedValue = "6"; break;
                    case 3: // zona II
                        ddlOrigen.SelectedValue = "8"; break;
                    case 5: // zona III
                        ddlOrigen.SelectedValue = "9"; break;
                    case 7: // zona IV
                        ddlOrigen.SelectedValue = "10"; break;
                    case 8: // zona v
                        ddlOrigen.SelectedValue = "11"; break;
                    case 9: // zona metro
                        ddlOrigen.SelectedValue = "5"; break;
                    case 10: // rio negro
                        ddlOrigen.SelectedValue = "12"; break;

                }
                btnGuardarSolicitante.Visible = true;
                btnGuardarSolicitante.UpdateAfterCallBack = true;

                btnCancelarSolicitante.Visible = true;
                btnCancelarSolicitante.UpdateAfterCallBack = true;


                ddlOrigen.UpdateAfterCallBack = true;

            }
            else
            {
                ddlOrigen.SelectedValue = "0"; 
                ddlOrigen.UpdateAfterCallBack = true;

                btnGuardarSolicitante.Visible = false;
                btnGuardarSolicitante.UpdateAfterCallBack = true;

                btnCancelarSolicitante.Visible = false;
                btnCancelarSolicitante.UpdateAfterCallBack = true;
                CargarSolicitantesInternos();
            }

        }

        private void CargarSolicitantesInternos()
        {
            Utility oUtil = new Utility();      
            ///Carga de combos de Medicos Solicitantes
           string  m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional ORDER BY apellido, nombre ";
            oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
            ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
            ddlEspecialista.UpdateAfterCallBack = true;
            //imgCrearSolicitante.Visible = false;
            //imgCrearSolicitante.UpdateAfterCallBack = true;
        }

        private void CargarSolicitantesExternos(string m_solicitante)
        {
            Utility oUtil = new Utility();            

            ///Carga de combos de solicitantes expertos
            string m_ssql = "select idSolicitanteExterno, apellido + ', ' + nombre as nombre from Lab_SolicitanteExterno where baja=0 order by apellido, nombre";
            oUtil.CargarCombo(ddlEspecialista, m_ssql, "idSolicitanteExterno", "nombre");
            ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
            if (m_solicitante != "")                ddlEspecialista.SelectedValue = m_solicitante;
            ddlEspecialista.UpdateAfterCallBack = true;
            //imgCrearSolicitante.Visible = true;
            //imgCrearSolicitante.UpdateAfterCallBack = true;
        }

        protected void btnGuardarSolicitante_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GuardarSolicitanteExterno();

                LimpiarDatosSolicitante();
                //Panel1.Visible = false;
                //Panel1.UpdateAfterCallBack = true;
            }
        }

        private void GuardarSolicitanteExterno()
        {
            Usuario oUser = new Usuario();
            SolicitanteExterno oRegistro = new SolicitanteExterno();
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            oRegistro.IdEfector = oC.IdEfector;
            oRegistro.Matricula = txtMatricula.Text;
            oRegistro.Apellido = txtApellidoSolicitante.Text;
            oRegistro.Nombre = txtNombreSolicitante.Text;
            oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
            CargarSolicitantesExternos(oRegistro.IdSolicitanteExterno.ToString());
        }

        protected void btnCancelarSolicitante_Click(object sender, EventArgs e)
        {
            LimpiarDatosSolicitante();

        }

        private void LimpiarDatosSolicitante()
        {
            txtMatricula.Text = "";
            txtApellidoSolicitante.Text = "";
            txtNombreSolicitante.Text = "";
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtCodigoDiagnostico_TextChanged(object sender, EventArgs e)
        {
            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {                
                Cie10 oDiagnostico = new Cie10();
                oDiagnostico = (Cie10)oDiagnostico.Get(typeof(Cie10), "Codigo", txtCodigoDiagnostico.Text);
                if (oDiagnostico != null)
                {
                    ListItem oDia = new ListItem();
                    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    oDia.Value = oDiagnostico.Id.ToString();
                    lstDiagnosticos.Items.Add(oDia);
                  
                }
                else
                    lstDiagnosticos.Items.Clear();
            }
              lstDiagnosticos.UpdateAfterCallBack = true;
            
        }

        protected void txtNombreDiagnostico_TextChanged(object sender, EventArgs e)
        {
            if (txtNombreDiagnostico.Text != "")
            {
                lstDiagnosticos.Items.Clear();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Cie10));
                crit.Add(Expression.Sql(" Nombre like '%" + txtNombreDiagnostico.Text + "%' order by Nombre"));

                IList items = crit.List();

                foreach (Cie10 oDiagnostico in items)
                {
                    ListItem oDia = new ListItem();
                    oDia.Text =  oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    oDia.Value = oDiagnostico.Id.ToString();
                    lstDiagnosticos.Items.Add(oDia);
                }

                lstDiagnosticos.UpdateAfterCallBack = true;
            }
            
        }

        protected void btnAgregarDiagnostico_Click1(object sender, ImageClickEventArgs e)
        {
            AgregarDiagnostico();
        }

        protected void btnSacarDiagnostico_Click(object sender, ImageClickEventArgs e)
        {
            SacarDiagnostico();
        }

        protected void cvAnalisis_ServerValidate(object source, ServerValidateEventArgs args)
        {

         
        }

        protected void cvValidacionInput_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //TxtDatosCargados.Value = TxtDatos.Value;

            //string sDatos = "";

            //  string[] tabla = TxtDatos.Value.Split('@');
          
            //for (int i = 0; i < tabla.Length - 1; i++)
            //{
            //    string[] fila = tabla[i].Split('#');
            //    string codigo = fila[1].ToString();
            //    string muestra= fila[2].ToString();                
            
            //        if (sDatos == "")
            //            sDatos = codigo + "#" + muestra;
            //        else
            //            sDatos += ";" +  codigo + "#" + muestra;                                                        

            //}
          
            //TxtDatosCargados.Value = sDatos;

            //if (!VerificarAnalisisContenidos())
            //{  TxtDatos.Value = "";
            //    args.IsValid = false;
             
            //    return;
            //}
            //else
            //{
            //    if ((TxtDatos.Value == "") || (TxtDatos.Value == "1###on@"))
            //    {

            //        args.IsValid = false;
            //        this.cvValidacionInput.ErrorMessage = "Debe completar al menos un análisis";
            //        return;
            //    }
            //    else args.IsValid = true;

            string aux1 = "";
        
            CheckBoxList a = (CheckBoxList)(this.PesquisaNeonatal1.FindControl("chkItemPesquisa"));
            if (a != null)
            {
                for (int i = 0; i < a.Items.Count; i++)
                {
                    if (a.Items[i].Selected)
                    {
                        if (aux1 == "")
                            aux1 = a.Items[i].Text;
                        else
                            aux1 = aux1 + "@" + a.Items[i].Text;
                    }                   
                }
            }


            if (aux1=="")             
            {
                args.IsValid = false;
                this.cvValidacionInput.ErrorMessage = "Debe completar al menos un análisis";
                return;
            }
            else
            {

                ///Validacion de la fecha de protocolo
                if (txtFecha.Value == "")
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la fecha del protocolo";
                    return;
                }
                else
                {

                    if (DateTime.Parse(txtFecha.Value) > DateTime.Now)
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "La fecha del protocolo no puede ser superior a la fecha actual";
                        return;
                    }
                    else
                        args.IsValid = true;
                }

                ///Validacion de la fecha de la orden

                if (txtFechaOrden.Value == "")
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la fecha de la orden";
                    return;
                }
                else
                {
                    if (DateTime.Parse(txtFechaOrden.Value) > DateTime.Now)
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "La fecha de la orden no puede ser superior a la fecha actual";
                        return;
                    }
                    else
                        args.IsValid = true;
                }

            }
        }

        private bool VerificarAnalisisContenidos()
        {
            bool devolver = true;
            string[] tabla = TxtDatos.Value.Split('@');
            string listaCodigo = "";
          
            for (int i = 0; i < tabla.Length - 1; i++)
            {
                string[] fila = tabla[i].Split('#');
                string codigo = fila[1].ToString();
                if (listaCodigo == "")
                    listaCodigo = "'" + codigo + "'";
                else
                    listaCodigo += ",'" + codigo + "'";

                int i_idItemPractica = 0;
                if (codigo != "")
                {

                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo, "Baja", false);                 


                    i_idItemPractica = oItem.IdItem;
                    for (int j = 0; j < tabla.Length - 1; j++)
                    {
                        string[] fila2 = tabla[j].Split('#');
                        string codigo2 = fila2[1].ToString();
                        if ((codigo2 != "") && (codigo!=codigo2))
                        {
                            Item oItem2 = new Item();
                            oItem2 = (Item)oItem2.Get(typeof(Item), "Codigo", codigo2, "Baja", false);

                            PracticaDeterminacion oGrupo = new PracticaDeterminacion();
                            oGrupo = (PracticaDeterminacion)oGrupo.Get(typeof(PracticaDeterminacion), "IdItemPractica", oItem, "IdItemDeterminacion", oItem2.IdItem);
                            if (oGrupo != null)
                            {
                    
                                this.cvValidacionInput.ErrorMessage = "Ha cargado análisis contenidos en otros. Verifique los códigos " + codigo + " y " + codigo2 + "!";
                                devolver = false; break;
                    
                            }
                          
                        }
                    }////for           
                }/// if codigo
                if (!devolver) break;
            }

            if ((devolver) && (listaCodigo != ""))
            { devolver = VerificarAnalisisComplejosContenidos(listaCodigo); }
           
            return devolver;
         
        }

        private bool VerificarAnalisisComplejosContenidos(string listaCodigo)
        { ///Este es un segundo nivel de validacion en donde los analisis contenidos no estan directamente sino en diagramas
            bool devolver = true;
            string m_ssql = "SELECT  PD.idItemDeterminacion, I.codigo" +
                            " FROM         LAB_PracticaDeterminacion AS PD " +
                            " INNER JOIN   LAB_Item AS I ON PD.idItemPractica = I.idItem " +
                            " WHERE     I.codigo IN (" + listaCodigo + ") AND (I.baja = 0)" +
                            " ORDER BY PD.idItemDeterminacion ";

            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds);

            string itempivot = "";
            string codigopivot = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i][0].ToString() == itempivot)
                {
                    devolver = false;
                    cvValidacionInput.ErrorMessage = "Ha cargado análisis contenidos en otros. Verifique los códigos " + codigopivot + " y " + ds.Tables[0].Rows[i][1].ToString() + "!";
                    break;
                }
                codigopivot = ds.Tables[0].Rows[i][1].ToString();
                itempivot = ds.Tables[0].Rows[i][0].ToString();
            }
            return devolver;

        }

        
        protected void lnkReimprimirComprobante_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));            

            ////Imprimir Comprobante para el paciente
            Imprimir(oRegistro);

         
           
        }

        protected void lnkReimprimirCodigoBarras_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            ///Imprimir codigo de barras.
            string s_AreasCodigosBarras = getListaAreasCodigoBarras();
            if (s_AreasCodigosBarras != "")
            {
                 
                ImprimirCodigoBarras(oRegistro, getListaAreasCodigoBarras());
            }
        }


        protected void lnkSiguiente_Click(object sender, EventArgs e)
        {
            Avanzar(1);
        }

        protected void lnkAnterior_Click(object sender, EventArgs e)
        {
            Avanzar(-1);
        }

        private void Avanzar(int avance)
        {

            int ProtocoloActual = int.Parse(Request["idProtocolo"].ToString());
            //Protocolo oProtocoloActual = new Protocolo();
            //oProtocoloActual = (Protocolo)oProtocoloActual.Get(typeof(Protocolo), ProtocoloActual);
            int ProtocoloNuevo = ProtocoloActual;

            if (Session["ListaProtocolo"] != null)
            {
                string[] lista = Session["ListaProtocolo"].ToString().Split(',');
                for (int i = 0; i < lista.Length ; i++)
                {
                    if (ProtocoloActual == int.Parse(lista[i].ToString()))
                    {
                        if (avance == 1)
                        {
                            if (i < lista.Length-1 )
                            {
                                ProtocoloNuevo = int.Parse(lista[i + 1].ToString()); break;
                            }
                        }
                        else  //retrocede                        
                        {
                            if (i >0)
                            {
                                ProtocoloNuevo = int.Parse(lista[i - 1].ToString()); break;
                            }
                        }

                        
                    }
                }
            }
                //if (avance == 1)
                //{
                //    ProtocoloNuevo = ProtocoloActual+1;
                //}
                //else  //retrocede                        
                //    ProtocoloNuevo = ProtocoloActual - 1;

            

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Protocolo));
            crit.Add(Expression.Eq("IdProtocolo", ProtocoloNuevo));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Protocolo oProtocolo = (Protocolo)crit.UniqueResult();

            string m_parametro = "";
            if (Request["DesdeUrgencia"]!=null)m_parametro= "&DesdeUrgencia=1";

            if (oProtocolo != null)
            {
                //if (Request["Desde"].ToString() == "Control")
                    Response.Redirect("ProtocoloEditPesquisa.aspx?Desde="+Request["Desde"].ToString()+"&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
                //else
                //    Response.Redirect("ProtocoloEdit2.aspx?Desde="+Request["Desde"].ToString()+"&idServicio=" + Session["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
            }
            else
                Response.Redirect("ProtocoloEditPesquisa.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloActual + m_parametro);                                                               
               
        }



        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
              if (e.CommandName== "Modificar")
                  Response.Redirect("ProtocoloEditPesquisa.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + e.CommandArgument.ToString());
            

        }

        protected void btnBusquedaDiagnostico_Click(object sender, EventArgs e)
        {
            
            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {
                Cie10 oDiagnostico = new Cie10();
                oDiagnostico = (Cie10)oDiagnostico.Get(typeof(Cie10), "Codigo", txtCodigoDiagnostico.Text);
                if (oDiagnostico != null)
                {
                    ListItem oDia = new ListItem();
                    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    oDia.Value = oDiagnostico.Id.ToString();
                    lstDiagnosticos.Items.Add(oDia);

                }
                else
                    lstDiagnosticos.Items.Clear();
            }

            if (txtNombreDiagnostico.Text != "")
            {
                lstDiagnosticos.Items.Clear();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Cie10));
                crit.Add(Expression.Sql(" Nombre like '%" + txtNombreDiagnostico.Text + "%' order by Nombre"));

                IList items = crit.List();

                foreach (Cie10 oDiagnostico in items)
                {
                    ListItem oDia = new ListItem();
                    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    oDia.Value = oDiagnostico.Id.ToString();
                    lstDiagnosticos.Items.Add(oDia);
                }

               
            }
            lstDiagnosticos.UpdateAfterCallBack = true;



            
        }

        private void CargarDiagnosticosFrecuentes()
        {
            Utility oUtil = new Utility();


            //  btnGuardarImprimir.Visible = oC.GeneraComprobanteProtocolo;
            lstDiagnosticos.Items.Clear();

            ///Carga de combos de tipos de servicios
            string m_ssql = "SELECT ID, Codigo + ' - ' + Nombre as nombre FROM Sys_CIE10 WHERE (ID IN (SELECT DISTINCT idDiagnostico FROM LAB_ProtocoloDiagnostico)) ORDER BY Nombre";
            oUtil.CargarListBox(lstDiagnosticos, m_ssql, "id", "nombre");
            lstDiagnosticos.UpdateAfterCallBack = true;


        }

        protected void btnBusquedaFrecuente_Click(object sender, EventArgs e)
        {
            CargarDiagnosticosFrecuentes();
        }

        protected void rdbSeleccionarAreasEtiquetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbSeleccionarAreasEtiquetas.SelectedValue == "1")
                MarcarTodasAreas(true);
            else
                MarcarTodasAreas(false);
            chkAreaCodigoBarra.UpdateAfterCallBack = true;
        }

        private void MarcarTodasAreas(bool p)
        {
            for (int i = 0; i < chkAreaCodigoBarra.Items.Count; i++)
                chkAreaCodigoBarra.Items[i].Selected = p;

        }



    }
}

