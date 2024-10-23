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
using System.Web.Script.Serialization;
using System.Collections.Generic;

using System.Web;
using System.Net.Http;

namespace WebLab.Protocolos
{
    public partial class ProtocoloEditEfector : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public  Configuracion oC = new Configuracion();
       

        protected void Page_PreInit(object sender, EventArgs e)
        {           
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
        }

   

        public string GetPuco(int numeroDocumento)
        {
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            string codigo = "";
            string nombre = "";
            connetionString = ConfigurationManager.ConnectionStrings["Puco"].ConnectionString;
            
                ; // "Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserName;Password=Password";
            sql = "select S.nombre, S.cod_os as os from pd_puco P inner join obras_sociales S on S.cod_os=P.codigoOS where P.dni = " + numeroDocumento.ToString();

            connection = new SqlConnection(connetionString);
            try
            {
                SqlDataReader rdr = null;
                connection.Open();
                command = new SqlCommand(sql, connection);
              rdr=  command.ExecuteReader();
              
                while (rdr.Read())
                {
                    nombre = rdr[0].ToString();
                    codigo = rdr[1].ToString();
                }
                if (nombre == "") nombre = "Sin obra social"; // sin obra social
                else nombre = nombre + "&" + codigo;
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                nombre = "Sin obra social&0";
            }

            return nombre;
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
                PreventingDoubleSubmit(btnGuardar);
                if (Session["idUsuario"] != null)
                {
                    //pnlIncidencia.Visible = false;
                    //tituloCalidad.Visible = false;
                    //this.IncidenciaEdit1.Visible = false;
                    if (Request["idServicio"] != null) Session["idServicio"] = Request["idServicio"].ToString();
                    if (Request["idUrgencia"] != null) Session["idUrgencia"] = Request["idUrgencia"].ToString() ;
                    CargarListas();                 

                    if (Request["Operacion"].ToString() == "Modifica")
                    {
                        //lnkReimprimirComprobante.Visible = true;
                        //lnkReimprimirCodigoBarras.Visible = true;

                        lblTitulo.Visible = true;
                        pnlNumero.Visible = true;
                        //pnlNuevo.Visible = false;
                        
                        MuestraDatos();
                        //VerificaPermisos("Pacientes sin turno");
                        if (Request["Desde"].ToString() =="Control")
                        {
                            pnlLista.Visible = true;
                            CargarGrilla();
                            gvLista.Visible = true;
                            pnlNavegacion.Visible = true;
        
                        }
                        else
                        {
                            pnlLista.Visible = false;
                            gvLista.Visible = false;
                            pnlNavegacion.Visible = false;
        
                        }
                    }
                    else

                    {
                       
                        //lnkReimprimirComprobante.Visible = false;
                        //lnkReimprimirCodigoBarras.Visible = false;

                        hplActualizarPaciente.Visible = false;
                        hplModificarPaciente.Visible = false;


                        lblTitulo.Visible = false;
                        txtFecha.Value = DateTime.Now.ToShortDateString();

                        ///verificar si la configuracion determina fecha actual por defecto o sin valor
                        if (oC.ValorDefectoFechaOrden == 0)  txtFechaOrden.Value = "";
                        else   txtFechaOrden.Value = DateTime.Now.ToShortDateString();

                        if (oC.ValorDefectoFechaTomaMuestra == 0) txtFechaTomaMuestra.Value = "";
                        else txtFechaTomaMuestra.Value = DateTime.Now.ToShortDateString();
                        ///

                        MostrarPaciente();

                        //pnlNuevo.Visible = true;
                        pnlNavegacion.Visible = false;

                        btnCancelar.Text = "Cancelar";
                        btnCancelar.Width = Unit.Pixel(80);

                        if (Request["idServicio"].ToString() == "6")
                        {
                            //tabContainer.Visible = false;
                            ddlOrigen.SelectedValue = "1"; //Ambulatorio /Externo
                            ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina
                            lblSalaCama.Visible = false;
                            txtSala.Visible = false;
                            txtCama.Visible = false;
                           
                       
                                pnlTitulo.Attributes.Add("class", "panel panel-success");
                            //tableTitulo.Attributes.Add("class", "tituloCeldaVerde");
                            btnGuardar.Attributes.Add("class", "btn btn-success");
                            btnCancelar.Attributes.Add("class", "btn btn-success");
                            //CargarDeterminacionesForense();
                        }
                        else
                        { 
                            if (Request["Operacion"].ToString() == "AltaTurno")
                            {
                                CargarDeterminacionesTurno();
                                ddlOrigen.SelectedValue = "1"; //Ambulatorio /Externo
                                ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina
                        

                            }
                            if (Request["Operacion"].ToString() == "AltaDerivacion")
                            {
                                txtNumeroOrigen.Text = Request["numeroOrigen"].ToString();
                                txtFechaOrden.Value = Request["fechaOrden"].ToString();
                                ddlEfector.SelectedValue = Request["idEfectorSolicitante"].ToString(); SelectedEfector();
                                txtSala.Text = Request["sala"].ToString();
                                txtCama.Text = Request["cama"].ToString();
                                CargarDeterminacionesDerivacion( Request["analisis"].ToString(), Request["diagnostico"].ToString());                          
                                ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina
                            

                            }

                            if (Request["Operacion"].ToString() == "AltaPeticion")
                            {
                                string idPeticion = Request["idPeticion"].ToString();
                                Peticion oRegistro = new Peticion();
                                oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(idPeticion));

                                txtFechaOrden.Value = oRegistro.Fecha.ToShortDateString();
                                ddlEfector.SelectedValue = oRegistro.IdEfector.IdEfector.ToString();
                                txtSala.Text = oRegistro.Sala;
                                txtCama.Text = oRegistro.Cama;
                                ddlEspecialista.SelectedValue = oRegistro.IdSolicitante.ToString();
                                ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();
                                ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();
                                if (oRegistro.IdOrigen.IdOrigen == 3) ddlPrioridad.SelectedValue = "2"; // si es de guardia es urgente
                                if (oRegistro.IdTipoServicio.IdTipoServicio == 3) ddlPrioridad.SelectedValue = "1"; // si es de microbiologia NO es urgente
                                txtObservacion.Text = oRegistro.Observaciones;
                                txtNumeroOrigen.Text = "PE-" + oRegistro.IdPeticion.ToString();
                                txtNumeroOrigen.Enabled = false;
                                if (oRegistro.IdTipoServicio.IdTipoServicio == 3) ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();

                                CargarDeterminacionesPeticion(oRegistro);

                            }
                        }


                    }
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
        }

     

        //private void CargarDeterminacionesForense()
        //{
        //   TxtDatosCargados.Value = "F001#Si";
        //    TxtDatos.Value = TxtDatosCargados.Value;
        //}

        private void CargarDeterminacionesPeticion(Peticion oRegistro)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(PeticionItem));
            crit.Add(Expression.Eq("IdPeticion", oRegistro));

            IList items = crit.List();
            string pivot = "";
            string sDatos = "";
            foreach (PeticionItem oDet in items)
            {
                if (pivot != oDet.IdItem.Nombre)
                {
                    //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#false@";
                    if (sDatos == "")
                        sDatos = oDet.IdItem.Codigo + "#Si";
                    else
                        sDatos += ";" + oDet.IdItem.Codigo + "#Si";

                    pivot = oDet.IdItem.Nombre;
                }
            }

            TxtDatosCargados.Value = sDatos;

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
                if (oC.NomencladorDiagnostico == 0)
                {
                    Cie10 oD = new Cie10();
                    oD = (Cie10)oD.Get(typeof(Cie10), int.Parse(s_diagnostico));
                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Nombre;
                    oDia.Value = oD.Id.ToString(); lstDiagnosticosFinal.Items.Add(oDia);
                }
                else {
                    DiagnosticoP oD = new DiagnosticoP();
                    oD = (DiagnosticoP)oD.Get(typeof(DiagnosticoP), int.Parse(s_diagnostico));
                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Nombre;
                    oDia.Value = oD.IdDiagnostico.ToString();   lstDiagnosticosFinal.Items.Add(oDia);
                }


             
            }
        }

      

        protected void txtCodigoMuestra_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Muestra oMuestra = new Muestra();
                oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), "Codigo", txtCodigoMuestra.Text, "Baja", false);
                if (oMuestra != null) ddlMuestra.SelectedValue = oMuestra.IdMuestra.ToString();
                ddlMuestra.UpdateAfterCallBack = true;
            }
            catch (Exception ex)
            {
                string exception = "";
                //while (ex != null)
                //{
                    exception = ex.Message + "<br>";

                //}
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
       
        private void CargarDeterminacionesTurno()
        {
            Turno oTurno = new Turno();
            oTurno = (Turno)oTurno.Get(typeof(Turno), int.Parse(Request["idTurno"].ToString()));

            ddlSectorServicio.SelectedValue = oTurno.IdSector.ToString();
            ddlEspecialista.SelectedValue = oTurno.IdEspecialistaSolicitante.ToString();

            lblObraSocial.Text = oTurno.IdObraSocial.Nombre;


            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(TurnoItem));
            crit.Add(Expression.Eq("IdTurno", oTurno));

            IList items = crit.List();
            string pivot = "";
            string sDatos = "";
            foreach (TurnoItem oDet in items)
            {
                if (pivot != oDet.IdItem.Nombre)
                {
                    //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#false@";
                    if (sDatos == "")
                        sDatos = oDet.IdItem.Codigo + "#Si";
                    else
                        sDatos += ";" + oDet.IdItem.Codigo + "#Si" ;

                    pivot = oDet.IdItem.Nombre;
                }
            }

            TxtDatosCargados.Value = sDatos;


            ///Agregar a la tabla las diagnosticos para mostrarlas en el gridview                             
         //   dtDiagnosticos = (System.Data.DataTable)(Session["Tabla2"]);
            TurnoDiagnostico oDiagnostico = new TurnoDiagnostico();
            //ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session.CreateCriteria(typeof(TurnoDiagnostico));
            crit2.Add(Expression.Eq("IdTurno", oTurno));

            IList diagnosticos = crit2.List();

            foreach (TurnoDiagnostico oDiag in diagnosticos)
            {
                if (oC.NomencladorDiagnostico == 0)
                {
                    Cie10 oD = new Cie10();
                    oD = (Cie10)oD.Get(typeof(Cie10), oDiag.IdDiagnostico);
                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Nombre;
                    oDia.Value = oD.Id.ToString();
                    lstDiagnosticosFinal.Items.Add(oDia);
                }
                else
                {
                    DiagnosticoP oD = new DiagnosticoP();
                    oD = (DiagnosticoP)oD.Get(typeof(DiagnosticoP), oDiag.IdDiagnostico);
                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Nombre;
                    oDia.Value = oD.IdDiagnostico.ToString();
                    lstDiagnosticosFinal.Items.Add(oDia);
                }    
            }                      
        }

        private void MuestraDatos()
        {
            Utility oUtil = new Utility();
            //Actualiza los datos de los objetos : alta o modificacion .
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));


            ///Si el protocolo ya existe trae los servicios activos mas el dado de baja correspondeiente al protocolo
            string m_ssql = @"SELECT  idSectorServicio,  prefijo + ' - ' + nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) 
union
SELECT  idSectorServicio,  prefijo + ' - ' + nombre as nombre FROM LAB_SectorServicio WHERE idsectorservicio = "+oRegistro.IdSector.IdSectorServicio.ToString()+" order by nombre";



            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));

         


            if (oRegistro.IdTipoServicio.IdTipoServicio == 6)
            {
                lblSalaCama.Visible = false;
                txtSala.Visible = false;
                txtCama.Visible = false;


                pnlTitulo.Attributes.Add("class", "panel panel-success");
                //tableTitulo.Attributes.Add("class", "tituloCeldaVerde");
                btnGuardar.CssClass= "btn btn-success";
                btnCancelar.CssClass = "btn btn-success";
            }

            if (oRegistro.tieneAdjuntoProtocolo())
            { spanadjunto.Visible = true;   }
            else
            { spanadjunto.Visible = false;   }

            if (oRegistro.IdPrioridad.IdPrioridad == 2)
                Session["idUrgencia"] = 2;
            else
                Session["idUrgencia"] = 0;

            pnlIncidencia.Visible = true;
            tituloCalidad.Visible = true;
            this.IncidenciaEdit1.Visible = true;
            this.IncidenciaEdit1.MostrarDatosdelProtocolo(oRegistro.IdProtocolo);
          
            if (oRegistro.getIncidencias() > 0)
                inci.Visible = true;
            else
                inci.Visible = false;
                        

            ///Cambiar de Paciente: LLeva a elección de un otro paciente para asignarlo al protocolo.
            hplModificarPaciente.NavigateUrl = "Default2.aspx?Operacion=Modifica&idUrgencia="+Session["idUrgencia"].ToString()+"&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString()+"&idProtocolo=" + Request["idProtocolo"].ToString()+"&Desde="+ Request["Desde"].ToString();
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

            lblUsuario.Text = oRegistro.IdUsuarioRegistro.Apellido + " " + oRegistro.FechaRegistro.ToString();

            lblTitulo.Text +=  oRegistro.GetNumero().ToString();
            
            //ddlServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
            
            CargarItems();
            txtFecha.Value = oRegistro.Fecha.ToShortDateString();
            txtFechaOrden.Value= oRegistro.FechaOrden.ToShortDateString();
            txtFechaTomaMuestra.Value = oRegistro.FechaTomaMuestra.ToShortDateString();

            txtSala.Text = oRegistro.Sala;
            txtCama.Text = oRegistro.Cama;

            ddlCaracter.SelectedValue = oRegistro.IdCaracter.ToString();

            txtNumeroOrigen.Text = oRegistro.NumeroOrigen;
            txtNumeroOrigen2.Text = oRegistro.NumeroOrigen2;
            if (oRegistro.IdCasoSISA > 0)
            { lblNroSISA.Text = "Nro. SISA: "+ oRegistro.IdCasoSISA.ToString(); lblNroSISA.Visible = true; }
            txtEspecialista.Text = oRegistro.MatriculaEspecialista;
            
            ddlEspecialista.Items.Insert(0, new ListItem( oRegistro.Especialista, oRegistro.MatriculaEspecialista));
            ///Datos del Paciente
            if (Request["idPaciente"] == null)
            {
                HFNumeroDocumento.Value = oRegistro.IdPaciente.NumeroDocumento.ToString();
                HFIdPaciente.Value = oRegistro.IdPaciente.IdPaciente.ToString();
                lblIdPaciente.Text = oRegistro.IdPaciente.IdPaciente.ToString();
                if (oRegistro.IdPaciente.IdEstado != 2)
                    lblPaciente.Text = oRegistro.IdPaciente.NumeroDocumento.ToString() + " - " + oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();
                if (oRegistro.IdPaciente.IdEstado == 2)
                    lblPaciente.Text = oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper() + " (ID Temporal:"+ oRegistro.IdPaciente.NumeroDocumento.ToString() +") ";
              
                if (oRegistro.IdPaciente.IdEstado == 3)
                {
                    logoRenaper.Visible = true;
                    if (oRegistro.IdCasoSISA > 0)
                        btnNotificarSISA.Visible = false;
                    else
                    {
                        //if ((oC.NotificarSISA) && (oRegistro.IdEfectorSolicitante.NotificarSISA))
                      //  if (oC.NotificarSISA)
                            if (oRegistro.VerificaObligatoriedadFIS())
                           // if (oRegistro.IdCaracter == 1) //|| (oRegistro.IdCaracter == 4))
                            { btnNotificarSISA.Visible = true; }
                    }
                }
                else
                    logoRenaper.Visible = false;


                txtTelefono.Text = oRegistro.IdPaciente.InformacionContacto;

                string[] edad = oUtil.DiferenciaFechas(oRegistro.IdPaciente.FechaNacimiento, oRegistro.Fecha).Split(';');
                lblEdad.Text = edad[0].ToString();
                lblUnidadEdad.Text = " " + edad[1].ToUpper();

                //txtEdad.Value = edadpropuesta.Split(;);
                lblFechaNacimiento.Text = "F.Nac.:" + oRegistro.IdPaciente.FechaNacimiento.ToShortDateString();

               
                switch (oRegistro.IdPaciente.IdSexo)
                {
                    case 1: lblSexo.Text = "I"; break;
                    case 2: lblSexo.Text = "F"; HFSexo.Value = "F"; break;
                    case 3: lblSexo.Text = "M"; HFSexo.Value = "M"; break;
                }

                //if ((oRegistro.IdPaciente.IdEstado == 1) && ((lblSexo.Text=="F")|| (lblSexo.Text=="M")))
                //    lnkValidarRenaper.Visible = true;
                //if  (oRegistro.IdObraSocial.Sigla.Length>3)
                //lblObraSocial.Text = oRegistro.IdObraSocial.Sigla ;
                //else

                lblObraSocial.Text = oRegistro.NombreObraSocial; // oRegistro.IdObraSocial.Nombre;
                CodOS.Value = oRegistro.CodOs.ToString();

               
                    if ((lblObraSocial.Text == "-") || (lblObraSocial.Text == "Sin obra social"))
                        btnSelObraSocial.Visible = true;
                    else
                        btnSelObraSocial.Visible = false;
                 

                //OSociales.setOS(oRegistro.IdObraSocial.IdObraSocial); //reemplezar por texto
            }
            else
            {
                MostrarPaciente();
            }
                      
       

            ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString(); 
            //if (oRegistro.IdEfectorSolicitante!= oRegistro.IdEfector)
            //    CargarSolicitantesExternos("");
            //else
            //    CargarSolicitantesInternos();

            ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();
            ddlEspecialista.SelectedValue=oRegistro.IdEspecialistaSolicitante.ToString();



            ///Carga de combos de Origen        
                  
            ddlOrigen.Items.Insert(0, new ListItem(oRegistro.IdOrigen.Nombre, oRegistro.IdOrigen.IdOrigen.ToString()));

            ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();

            ////
                        
            ddlPrioridad.SelectedValue = oRegistro.IdPrioridad.IdPrioridad.ToString();


            ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
            txtObservacion.Text = oRegistro.Observacion;


            MostrarDeterminaciones(oRegistro);

            //TxtDatos.Value = sDatos;

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ///Agregar a la tabla las diagnosticos para mostrarlas en el gridview                             
          //   dtDiagnosticos = (System.Data.DataTable)(Session["Tabla2"]);
            ProtocoloDiagnostico oDiagnostico = new ProtocoloDiagnostico();            
            ICriteria crit2 = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
            crit2.Add(Expression.Eq("IdProtocolo", oRegistro));

            IList diagnosticos = crit2.List();

            if (diagnosticos.Count > 0)
                diag.Visible = true;
            else
                diag.Visible = false;


           
            foreach (ProtocoloDiagnostico oDiag in diagnosticos)
            {
                if (oC.NomencladorDiagnostico == 0) //cie10
                {
                    Cie10 oD = new Cie10();
                    oD = (Cie10)oD.Get(typeof(Cie10), oDiag.IdDiagnostico);

                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Codigo + " - " + oD.Nombre;
                    oDia.Value = oD.Id.ToString();
                    lstDiagnosticosFinal.Items.Add(oDia);


                }
                else
                {
                    DiagnosticoP oD = new DiagnosticoP();
                    oD = (DiagnosticoP)oD.Get(typeof(DiagnosticoP), oDiag.IdDiagnostico);

                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Codigo + " - " + oD.Nombre;
                    oDia.Value = oD.IdDiagnostico.ToString();
                    lstDiagnosticosFinal.Items.Add(oDia);
                }
            }



            //chkRecordarConfiguracion.Checked = false;
            //chkRecordarPractica.Checked = false;


            //chkRecordarConfiguracion.Visible = false;
            //chkRecordarPractica.Visible = false;
            //chkImprimir.Visible = false;
            //chkRecordarConfiguracion.Visible = false;

            if (oRegistro.Estado==2)  btnGuardar.Visible = oC.ModificarProtocoloTerminado;          
        }

        private void MostrarDeterminaciones(Protocolo oRegistro)
        {
            ///Agregar a la tabla las determinaciones para mostrarlas en el gridview                             
            //dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
        
            crit.AddOrder(Order.Asc("IdDetalleProtocolo"));

            IList items = crit.List();
            string pivot = "";
            string sDatos = "";
            foreach (DetalleProtocolo oDet in items)
            {

                ddlResultado.SelectedItem.Text = oDet.ResultadoCar;
                rvResultado.Enabled = false;
                //if (pivot != oDet.IdItem.Nombre)
                //{
                //    if (sDatos == "")
                //        sDatos = oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                //    else
                //        sDatos += ";" + oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                //    //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#" + oDet.TrajoMuestra + "@";                   
                //    pivot = oDet.IdItem.Nombre;
                //}

            }

            TxtDatosCargados.Value = sDatos;
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
                    //chkImprimir.Visible = false;
                    hplModificarPaciente.Enabled = false;
                    hplActualizarPaciente.Enabled = false;
                }
            }
            else
            {
                result = "ANULADO";
                lblEstado.ForeColor = System.Drawing.Color.Red;
                btnGuardar.Visible = false;
                //chkImprimir.Visible = false;
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
           
          
            HFNumeroDocumento.Value = oPaciente.NumeroDocumento.ToString();
            switch (oPaciente.IdSexo)
            {
                case 1: lblSexo.Text = "I"; break;
                case 2: lblSexo.Text = "F"; break;
                case 3: lblSexo.Text = "M"; break;
            }

            //if (oPaciente.IdEstado == 1) Response.Redirect("ProcesaRenaper.aspx?Tipo=DNI&dni=" + oPaciente.NumeroDocumento.ToString() + "&sexo=" + lblSexo.Text + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString());

            lblIdPaciente.Text = oPaciente.IdPaciente.ToString();
            HFIdPaciente.Value = oPaciente.IdPaciente.ToString();

            if (oPaciente.IdEstado != 2)
                lblPaciente.Text = oPaciente.NumeroDocumento.ToString() + " - " + oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper();
            if (oPaciente.IdEstado == 2)
                lblPaciente.Text = oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper() + " (ID Temporal:" + oPaciente.NumeroDocumento.ToString() + ") ";
          
            if (oPaciente.IdEstado == 3)
                logoRenaper.Visible = true;
            else
                logoRenaper.Visible = false;

            /// verifica si el paciente tiene protocolos en los ultimos 7 dias
            string tieneingreso = oPaciente.GetProtocolosReciente(7,Request["idServicio"].ToString());

            if (tieneingreso != "")
            {
                lblAlertaProtocolo.Text = " El paciente tiene un protocolo ingresado el día " + tieneingreso;
                lblAlertaProtocolo.Visible = true;

              
                    
                 
            }

            else
            {
                lblAlertaProtocolo.Text = tieneingreso;
                lblAlertaProtocolo.Visible = false;
            }

            /// 

           

            if (oPaciente.IdEstado == 2) // no identificado-temporal
            {
                lblObraSocial.Text = "-";
                btnSelObraSocial.Visible = true;
            }
            else
            {
                string nombreOS = "";
                string codigoOS = "0";
                string[] os = GetPuco(oPaciente.NumeroDocumento).Split('&');
                if (os.Length > 1)
                {
                    nombreOS = os[0].ToString();
                    codigoOS = os[1].ToUpper();
                    CodOS.Value = codigoOS;
                }

                lblObraSocial.Text = nombreOS; // oObraSocial.Nombre;
               if ((lblObraSocial.Text == "-") || (lblObraSocial.Text == "Sin obra social"))
                    btnSelObraSocial.Visible = true;
                else
                btnSelObraSocial.Visible = false;
            }
            // ver como guardar el nombre de la obra social.

        

            
            string[] edad = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento, DateTime.Now).Split(';');
                        lblEdad.Text = edad[0].ToString();
                        lblUnidadEdad.Text = " " + edad[1].ToUpper();
                    //    ddlEfector.SelectedValue = Origen[1].ToString();

            //txtEdad.Value = edadpropuesta.Split(;);
            lblFechaNacimiento.Text = "F.Nac.:" + oPaciente.FechaNacimiento.ToShortDateString();
            txtTelefono.Text = oPaciente.InformacionContacto;




        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            
            ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
            pnlMuestra.Visible = false;

            string sDatos= "9124#Si";
            TxtDatosCargados.Value = sDatos;
            TxtDatos.Value = sDatos;

            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", "9124" );
            if (oItem != null)
                lblDeterminacion.Text = oItem.Nombre;

            //if (int.Parse(Session["idServicio"].ToString()) == 1)
            //{
            //    pnlComprobantePaciente.Visible = oC.GeneraComprobanteProtocolo;
            //    lblImprimeComprobantePaciente.Enabled = oC.GeneraComprobanteProtocolo;
            //    chkImprimir.Enabled = oC.GeneraComprobanteProtocolo;                                
            //    ddlImpresora.Enabled = oC.GeneraComprobanteProtocolo;
            //    lnkReimprimirComprobante.Enabled = oC.GeneraComprobanteProtocolo;
            //    lnkReimprimirCodigoBarras.Enabled = oC.GeneraComprobanteProtocolo;

            //}

            //if (int.Parse(Session["idServicio"].ToString()) == 3)
            //{
            //    pnlComprobantePaciente.Visible = oC.GeneraComprobanteProtocoloMicrobiologia;
            //    lblImprimeComprobantePaciente.Enabled = oC.GeneraComprobanteProtocoloMicrobiologia;                
            //    chkImprimir.Enabled = oC.GeneraComprobanteProtocoloMicrobiologia;                
            //    ddlImpresora.Enabled = oC.GeneraComprobanteProtocoloMicrobiologia;
            //    lnkReimprimirComprobante.Enabled = oC.GeneraComprobanteProtocoloMicrobiologia;
            //    lnkReimprimirCodigoBarras.Enabled = oC.GeneraComprobanteProtocoloMicrobiologia;
            //}




            ///Carga de combos de tipos de servicios          
            TipoServicio oServicio = new TipoServicio();
            oServicio =(TipoServicio) oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
            lblServicio.Text = oServicio.Nombre;
            
            
            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,  prefijo + ' - ' + nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";                   

            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));
            if (oC.IdSectorDefecto > 0)
            {
                ddlSectorServicio.SelectedValue = oC.IdSectorDefecto.ToString();
                ddlSectorServicio.Visible = false;
            }


            m_ssql = @"select idResultadoItem, resultado from lab_resultadoitem  as R
inner join lab_item  as I on R.iditem= I.iditem
where i.codigo='9124'
order by idResultadoItem";
            oUtil.CargarCombo(ddlResultado, m_ssql, "idResultadoItem", "resultado");
            ddlResultado.Items.Insert(0, new ListItem("--Seleccione---", "0"));
            ///////////////////////////////////////////////CODIGO DE BARRAS//////////////////////////////////////////////////////////////////////
            //ConfiguracionCodigoBarra oConfiguracion = new ConfiguracionCodigoBarra();
            //oConfiguracion = (ConfiguracionCodigoBarra)oConfiguracion.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oServicio);
            //if (oConfiguracion == null)
            //{
            //    lblImprimeCodigoBarras.Visible = false;
            //    chkAreaCodigoBarra.Items.Clear();
            //    ddlImpresoraEtiqueta.Visible = false;
            //}
            //else
            //{
            //    if (oConfiguracion.Habilitado)
            //    {
            //        lblImprimeCodigoBarras.Visible = true;
            //        ///cargar de areas con codigo de barras                            
            //        m_ssql = "select idArea, nombre from Lab_Area WHERE imprimeCodigoBarra=1  and baja=0 order by nombre";
            //        oUtil.CargarCheckBox(chkAreaCodigoBarra, m_ssql, "idArea", "nombre");
            //        chkAreaCodigoBarra.Items.Insert(0, new ListItem("General", "0"));                
            //    }
            //    else
            //    {
            //        lblImprimeCodigoBarras.Enabled = false;
            //        chkAreaCodigoBarra.Items.Clear();
            //        ddlImpresoraEtiqueta.Enabled = false;
            //    }
            //}



            //m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
            //oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
            //oUtil.CargarCombo(ddlImpresoraEtiqueta, m_ssql, "nombre", "nombre");
            //ddlImpresoraEtiqueta.Items.Insert(0, new ListItem("Seleccione impresora", "0"));

            //////////////////////////////////////////////////

            //////////////////////////FIN DE CODIGO DE BARRAS///////////////////////////////////////////////////////////////////////////////////                    

            ///Carga de combos de Origen
            m_ssql = " SELECT  idOrigen, nombre FROM LAB_Origen WHERE (baja = 0)";
            if (oC.OrigenHabilitado != "")
                m_ssql = m_ssql + " and idOrigen in (" + oC.OrigenHabilitado + ")";

          

            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("", "0"));
                       

            ///Carga de combos de Prioridad
            m_ssql = " SELECT idPrioridad, nombre FROM LAB_Prioridad WHERE (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre");          
            
            
            if ((Session["idServicio"].ToString() == "3") ||    (Session["idServicio"].ToString() == "6"))//microbiologia o forense microbiologia
                {
                ddlPrioridad.SelectedValue = "1";
                //ddlPrioridad.Enabled = false;


                ////////////Carga de combos de Muestras
                pnlMuestra.Visible = true;
                m_ssql = "SELECT idMuestra, nombre + ' - ' + codigo as nombre FROM LAB_Muestra  where (tipo=0 or tipo=1)";
                if (Request["Operacion"].ToString() != "Modifica")  //alta
                m_ssql += " and baja=0 ";
                
                m_ssql += " order by nombre ";
                oUtil.CargarCombo(ddlMuestra, m_ssql, "idMuestra", "nombre");
                ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
                rvMuestra.Enabled = true;

            }        

            ////////////Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.SelectedValue = Session["idEfectorSolicitante"].ToString();

            //////////////Carga de combos de Medicos Solicitantes
            //if (Request["Operacion"].ToString()=="Carga")///muestra solo los activos
            //    m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional WHERE activo=1 ORDER BY apellido, nombre ";
            //else
            //    m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional   ORDER BY apellido, nombre ";
            //oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
            if (Request["Operacion"].ToString() != "Modifica")
                ddlEspecialista.Items.Insert(0, new ListItem("--Ingrese un médico--", "-1"));

      



            m_ssql = "SELECT idCaracter, nombre   FROM LAB_Caracter ";          
            oUtil.CargarCombo(ddlCaracter, m_ssql, "idCaracter", "nombre");
            ddlCaracter.Items.Insert(0, new ListItem("--Seleccione Caracteristica--", "0"));
             

            ////////////////////////////Carga de combos de ObraSocial//////////////////////////////////////////
            //m_ssql = "SELECT idObraSocial,  nombre AS nombre FROM Sys_ObraSocial order by idObraSocial ";          
            //oUtil.CargarCombo(ddlObraSocial, m_ssql, "idObraSocial", "nombre");
          
            //////////////////////////////////////////////////////////////////////////////////////////////////


            //m_ssql ="SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre " +         
            //        " FROM Lab_item I  " +
            //        " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
            //        " where A.baja=0 and I.baja=0 and  I.disponible=1 and A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";
            //oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");


            if (Request["Operacion"].ToString() != "Modifica") 
            {
                if (Session["idUrgencia"] != null)
                {
                    if (Session["idUrgencia"].ToString() == "0")

                        IniciarValores(oC);
                }
            }

            if (Request["Operacion"].ToString() == "AltaDerivacion") IniciarValores(oC);

            //if (oC.IdEfector.IdEfector.ToString() != ddlEfector.SelectedValue)
            //    CargarSolicitantesExternos("");
            //else
            //    CargarSolicitantesInternos();
        
          ///Carga de determinaciones y rutinas dependen de la selección del tipo de servicio
            CargarItems();

            //CargarDiagnosticosFrecuentes();
            if (int.Parse(Session["idServicio"].ToString()) == 1)
            {
                if (Session["idUrgencia"] != null)
                {
                    if (Session["idUrgencia"].ToString() != "0")
                    {
                        ddlOrigen.SelectedValue = oC.IdOrigenUrgencia.ToString(); //Origen: Guardia
                        ddlSectorServicio.SelectedValue = oC.IdSectorUrgencia.ToString(); // sector de urgencia
                        ddlPrioridad.SelectedValue = "2"; // Prioridad: Urgencia
                    }
                    else
                    {

                        ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina
                    }
                }

            }

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
                        //case "chkRecordarConfiguracion":
                        //    {
                        //        if (s_control[1].ToString() == "False")
                        //            chkRecordarConfiguracion.Checked = false;
                        //        else
                        //            chkRecordarConfiguracion.Checked = true;
                        //    }
                        //    break;

                        //case "chkImprimir":
                        //    {
                        //        if (s_control[1].ToString() == "False")
                        //            chkImprimir.Checked = false;
                        //        else
                        //            chkImprimir.Checked = true;
                        //    }
                        //    break;

                        //case "chkAreaCodigoBarra":
                        //    {

                        //        string[] arrSector = s_control[1].ToString().Split((",").ToCharArray());
                        //        foreach (string itemSector in arrSector)
                        //        {
                        //            for (int j = 0; j < arrSector.Length; j++)
                        //            {
                        //                for (int i = 0; i < chkAreaCodigoBarra.Items.Count; i++)
                        //                {
                        //                    if (arrSector[j].ToString() != "")
                        //                    {
                        //                        if (int.Parse(chkAreaCodigoBarra.Items[i].Value) == int.Parse(arrSector[j].ToString()))
                        //                            chkAreaCodigoBarra.Items[i].Selected = true;
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    break;
                        //case "chkRecordarPractica":
                        //    {
                        //        if (s_control[1].ToString() == "False")
                        //            chkRecordarPractica.Checked = false;
                        //        else
                        //            chkRecordarPractica.Checked = true;
                        //    }
                        //    break;
                        case "prácticas":
                            TxtDatosCargados.Value = s_control[1].ToString(); break;
                        //case "ddlImpresora":
                        //    ddlImpresora.SelectedValue = s_control[1].ToString(); break;

                        //case "ddlImpresoraEtiqueta":
                        //    ddlImpresoraEtiqueta.SelectedValue = s_control[1].ToString(); break;
                    }
                }
            }
            //else
            //{
            //    if (Session["Impresora"]!=null) ddlImpresora.SelectedValue=Session["Impresora"].ToString();
            //    if (Session["Etiquetadora"]!=null)  ddlImpresoraEtiqueta.SelectedValue=Session["Etiquetadora"] .ToString();
            //}

        }
   
        private void CargarItems()
        {
            //Utility oUtil = new Utility();
            /////Carga del combo de determinaciones
            //string m_ssql = "SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, " +
            //               " I.disponible "+
            //                " FROM Lab_item I  " +
            //                " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
            //                " where A.baja=0 and I.baja=0  and A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";
            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            //SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "T");
           
            ////gvLista.DataSource = ds.Tables["T"];
            ////gvLista.DataBind();

           
            //ddlItem.Items.Insert(0, new ListItem("", "0"));

            //string sTareas = "";            
            //for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            //{
            //    sTareas += "#" + ds.Tables["T"].Rows[i][1].ToString() + "#" + ds.Tables["T"].Rows[i][2].ToString() + "#" + ds.Tables["T"].Rows[i][4].ToString() + "@";
            //}
            //txtTareas.Value = sTareas;            
            
            ////Carga de combo de rutinas
            //m_ssql = "SELECT idRutina, nombre FROM Lab_Rutina where baja=0 and idTipoServicio= " + Session["idServicio"].ToString()+ " order by nombre ";
            //oUtil.CargarCombo(ddlRutina, m_ssql, "idRutina", "nombre");
            //ddlRutina.Items.Insert(0, new ListItem("Seleccione una rutina", "0"));
            
            //ddlItem.UpdateAfterCallBack = true;
            //ddlRutina.UpdateAfterCallBack = true;                                    

            //m_ssql = null;
            //oUtil = null;
        }

       
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { ///Verifica si se trata de un alta o modificacion de protocolo               
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                if (Request["Operacion"].ToString() == "Modifica") oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

              
                Guardar(oRegistro);


                ////if (oRegistro.IdTipoServicio==6) Response.Redirect ("ProtocoloMensaje.aspx")
                //// se muestra un mensaje con el numero de protocolo y te da opcion de adjuntos y de consentieminto si es forense.


                if ((Request["Operacion"].ToString() == "Alta") || 
                    (Request["Operacion"].ToString() == "AltaTurno") ||
                    (Request["Operacion"].ToString() == "AltaDerivacion")||
                    (Request["Operacion"].ToString() == "AltaPeticion"))
                {

                    /// actualiza al paciente con la ultima obra social guardada: solo en las altas
                    oRegistro.IdPaciente.IdObraSocial = oRegistro.IdObraSocial.IdObraSocial;
                    oRegistro.IdPaciente.Save();
                    /////////////////


                    ///Imprimir codigo de barras.
                    //string s_AreasCodigosBarras = getListaAreasCodigoBarras();
                    //if (s_AreasCodigosBarras != "")                    
                    //    ImprimirCodigoBarras(oRegistro, getListaAreasCodigoBarras());


                    //////Imprimir Comprobante para el paciente
                    //if (chkImprimir.Checked) Imprimir(oRegistro);

                 //   if (oC.NotificarSISA)
                   // {
                      //  if ((oRegistro.IdCaracter==1)|| (oRegistro.IdCaracter == 4)) // sospechoso contacto de covid
                      //   if (oRegistro.IdCaracter == 1) // solo sospechoso
                      if (oRegistro.VerificaObligatoriedadFIS())
                            {
                            if (oRegistro.IdPaciente.IdEstado == 3) // validado por renaper
                            {
                                // if (oRegistro.IdEfectorSolicitante.NotificarSISA) // valida si es efector que se debe generar la notificacion
                                    Response.Redirect("ProcesaSISA.aspx?idP=" + oRegistro.IdProtocolo.ToString() +"&Desde=Efector&codigo=9124");
                            }
                                    
                        }
                   // }


                   
                    //////////////////////////
                }

//                EnviarProtocoloEquipo_DescargaManual();///Pone el protocolo en la tabla 

                if ((Request["Operacion"].ToString() == "AltaPeticion") && (Request["idPeticion"] != null))
                    ActualizarPeticion(Request["idPeticion"].ToString(), oRegistro);

                if (Request["idTurno"] != null)
                    ActualizarTurno(Request["idTurno"].ToString(), oRegistro);
                //if (Request["idSolicitudScreening"] != null) { Response.Redirect("../Neonatal/Default.aspx"); }

                //if (oC.GeneraComprobanteProtocolo)
                //    Response.Redirect("ProtocoloMensaje.aspx?id=" + oRegistro.IdProtocolo.ToString(), false);
                //else
                if (Request["Operacion"].ToString() == "Modifica")
                {
                    if (Request["DesdeUrgencia"] != null)
                        Response.Redirect("../Urgencia/UrgenciaList.aspx");
                    else
                    {
                        switch (Request["Desde"].ToString())
                        {
                            case "Default": Response.Redirect("Defaultefector.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
                            case "ProtocoloList": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Lista"); break;
                            case "Control": Avanzar(1); break;
                            case "Urgencia": Response.Redirect("../Urgencia/UrgenciaList.aspx", false); break;

                        }

                        //if (Request["Desde"].ToString() == "Control")
                        //    Avanzar(1);
                        //else
                        //    Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&tipo=Lista", false);
                     }
                }
                else
                {
                    if (Request["Operacion"].ToString() == "AltaDerivacion")
                        Response.Redirect("Derivacion.aspx?idEfector=" + Request["idEfectorSolicitante"].ToString() + "&idServicio="+ Session["idServicio"].ToString());
                    else
                    {
                        if (Request["Operacion"].ToString() == "AltaTurno")
                            Response.Redirect("../turnos/Turnolist.aspx?ultimoProtocolo=" + oRegistro.IdProtocolo.ToString(), false);
                        else
                        {
                     //       if (Request["Operacion"].ToString() == "AltaPeticion")
                       //         Response.Redirect("../PeticionElectronica/PeticionList.aspx", false);
                         //   else
                         //   {
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
                                            Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=2", false); break;
                                        case "0":
                                            Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false); break;
                                    }
                                }
                                else
                                    Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                         //   }
                        }
                    }
                }
            }
            
        }

    

        //private string getListaAreasCodigoBarras()
        //{
        //    string lista= "";
        //    for (int i = 0; i<chkAreaCodigoBarra.Items.Count; i++)
        //    { 
        //        if (chkAreaCodigoBarra.Items[i].Selected)
        //        {
        //            if (lista == "")
        //                lista = chkAreaCodigoBarra.Items[i].Value;
        //            else
        //                lista += ","+chkAreaCodigoBarra.Items[i].Value;
        //        }
        //    }
        //    return lista;
        //}

        //private void ImprimirCodigoBarras(Protocolo oProt, string s_listaAreas)
        //{
        //    oProt.GrabarAuditoriaProtocolo("Imprime Código de Barras", int.Parse(Session["idUsuario"].ToString()));       
        //    //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

        //    ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();  
        //    oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oProt.IdTipoServicio);

        //    string sFuenteBarCode = oConBarra.Fuente;
        //    bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
        //    bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
        //    bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
        //    bool imprimeProtocoloNumeroOrigen = oConBarra.ProtocoloNumeroOrigen;
        //    bool imprimePacienteNumeroDocumento = oConBarra.PacienteNumeroDocumento;
        //    bool imprimePacienteApellido = oConBarra.PacienteApellido;
        //    bool imprimePacienteSexo = oConBarra.PacienteSexo;
        //    bool imprimePacienteEdad = oConBarra.PacienteEdad;
        //    bool adicionalGeneral = false;
        //    if (s_listaAreas.Substring(0, 1) == "0") adicionalGeneral = true;                                               
     
        //    DataTable Dt= new DataTable();
        //    Dt =(DataTable) oProt.GetDataSetCodigoBarras("Protocolo", s_listaAreas, oProt.IdTipoServicio.IdTipoServicio, adicionalGeneral);
        //    foreach (DataRow item in Dt.Rows)
        //    {              
        //        ///Desde acá impresion de archivos
        //        string reg_numero = item[2].ToString();
        //        string reg_area = item[3].ToString();
        //        string reg_Fecha = item[4].ToString().Substring(0, 10);
        //        string reg_Origen = item[5].ToString();
        //        string reg_Sector = item[6].ToString();
        //        string reg_NumeroOrigen = item[7].ToString();
        //        string reg_NumeroDocumento = oProt.IdPaciente.getNumeroImprimir();

             

        //        string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);

        //        string reg_apellido = "";
        //        if (chkCodificaPaciente.Checked)
        //        {
        //            reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
        //        }
        //        else
        //        {
        //            if (reg_codificaHIV == "FALSE")
        //                reg_apellido = oProt.IdPaciente.Apellido + " " + oProt.IdPaciente.Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
        //            else
        //                reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
        //        }
        //            //reg_apellido = item[12].ToString().ToUpper();
        //        string reg_sexo = item[10].ToString();
        //        string reg_edad = item[11].ToString();
        //        //tabla.Rows.Add(reg);
        //        //tabla.AcceptChanges();

                
        //        if (!imprimeProtocoloFecha) reg_Fecha = "          ";
        //        if (!imprimeProtocoloOrigen) reg_Origen = "          ";
        //        if (!imprimeProtocoloSector) reg_Sector = "   ";
        //        if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
        //        if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
        //        if (!imprimePacienteApellido) reg_apellido = "";
        //        if (!imprimePacienteSexo) reg_sexo = " ";
        //        if (!imprimePacienteEdad) reg_edad = "   ";
        //        //ParameterDiscreteValue fuenteCodigoBarras = new ParameterDiscreteValue(); fuenteCodigoBarras.Value = oConBarra.Fuente;


        //        Business.Etiqueta ticket = new Business.Etiqueta();
        //        ticket.TipoEtiqueta = oC.TipoEtiqueta;
        //        if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);

                
        //        ticket.AddHeaderLine(reg_apellido.ToUpper());              
        //        ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha);
        //        if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector))   ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);
        //        if (reg_area!="")ticket.AddSubHeaderLineNegrita(reg_area);
        //        //ticket.AddSubHeaderLine(reg_area);

        //        ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
        //        ticket.AddFooterLine(reg_numero); // + "  " + reg_NumeroOrigen);

        //        Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;
        //        ticket.PrintTicket(ddlImpresoraEtiqueta.SelectedValue, oConBarra.Fuente);
        //        /////fin de impresion de archivos
        //    }
        
        //}
        //private void Imprimir(Protocolo oProt)
        //{

        //    ///////////////
        //    Business.Reporte ticket = new Business.Reporte();

        //    string textoAdicional = "";
        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //    ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra(); oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oProt.IdTipoServicio);

        //    string sFuenteBarCode = oConBarra.Fuente;
        //    string reg_numero = oProt.GetNumero();

        //    if (oProt.IdTipoServicio.IdTipoServicio == 1)
        //        textoAdicional = oCon.TextoAdicionalComprobanteProtocolo;
        //    if (oProt.IdTipoServicio.IdTipoServicio == 3)
        //        textoAdicional = oCon.TextoAdicionalComprobanteProtocoloMicrobiologia;


        //    DataTable dt = oProt.GetDataSetComprobante();
        //    string analisis = dt.Rows[0][7].ToString();
        //    string s_hiv = dt.Rows[0][13].ToString();
        //    string paciente = "";
        //    if (s_hiv != "False") paciente = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
        //    else paciente = oProt.IdPaciente.Apellido.ToUpper() + " " + oProt.IdPaciente.Nombre.ToUpper();

        //    ticket.AddHeaderLine("LABORATORIO " + oCon.EncabezadoLinea1);
        //    ticket.AddSubHeaderLine("_____________________________________________________________________________________________");
        //    ticket.AddSubHeaderLine("PROTOCOLO Nro. " + reg_numero + "         Fecha: " + oProt.Fecha.ToShortDateString() + "              Fecha de Entrega: " + oProt.FechaRetiro.ToShortDateString());
        //    ticket.AddSubHeaderLine(" ");
        //    ticket.AddSubHeaderLine(paciente.ToUpper() + "                       DU:" + oProt.IdPaciente.NumeroDocumento.ToString() + "      Fecha de Nacimiento:" + oProt.IdPaciente.FechaNacimiento.ToShortDateString() + "      SEXO:" + oProt.IdPaciente.getSexo());

            

        //    ticket.AddSubHeaderLine("_____________________________________________________________________________________________");
            
        //    int largo = analisis.Length;
        //    int cantidadFilas = largo / 90;
        //    if (cantidadFilas >= 0)
        //    {
        //        ticket.AddSubHeaderLine("PRACTICAS SOLICITADAS");
        //        for (int i = 1; i <= cantidadFilas; i++)
        //        {
        //            int l = i * 90;
        //            analisis = analisis.Insert(l, "&");

        //        }
        //        string[] tabla = analisis.Split('&');

        //        /////Crea nuevamente los detalles.
        //        for (int i = 0; i <= tabla.Length - 1; i++)
        //        {
        //            ticket.AddSubHeaderLine("     " + tabla[i].ToUpper());
        //        }
            
        //    } 
        //    ticket.AddSubHeaderLine("_____________________________________________________________________________________________");

        //    ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
        //    //ticket.AddFooterLine(reg_numero);

        //    ticket.AddFooterLine("******************************" + textoAdicional);



        //    Session["Impresora"] = ddlImpresora.SelectedValue;

        //    ticket.PrintTicket(ddlImpresora.SelectedValue, oConBarra.Fuente);
        //    /////fin de impresion de archivos
        //}



        private void ActualizarTurno(string p, Business.Data.Laboratorio.Protocolo oRegistro)
        {
            Turno oTurno = new Turno();
            oTurno = (Turno)oTurno.Get(typeof(Turno),int.Parse( p));
            oTurno.IdProtocolo = oRegistro.IdProtocolo;
            oTurno.Save();
        }

        private void ActualizarPeticion(string p, Protocolo oRegistro)
        {
            Peticion oPeticion = new Peticion();
            oPeticion = (Peticion)oPeticion.Get(typeof(Peticion), int.Parse(p));
            oPeticion.IdProtocolo = oRegistro.IdProtocolo;
            oPeticion.Save();
        }
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

                TipoServicio oServicio = new TipoServicio();
                oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
                oRegistro.IdTipoServicio = oServicio;


                if (Request["Operacion"].ToString() != "Modifica")
                {

                    oRegistro.Numero = oRegistro.GenerarNumero();
                    oRegistro.NumeroDiario = oRegistro.GenerarNumeroDiario(fecha.ToString("yyyyMMdd"));
                    oRegistro.PrefijoSector = oSector.Prefijo.Trim();
                    oRegistro.NumeroSector = oRegistro.GenerarNumeroGrupo(oSector);
                    oRegistro.NumeroTipoServicio = oRegistro.GenerarNumeroTipoServicio(oServicio);
                }

                oRegistro.IdCaracter =int.Parse( ddlCaracter.SelectedValue);

                oRegistro.Fecha = DateTime.Parse(txtFecha.Value);
                oRegistro.FechaOrden = DateTime.Parse(txtFechaOrden.Value);
                oRegistro.FechaTomaMuestra= DateTime.Parse(txtFechaTomaMuestra.Value);
                oRegistro.FechaRetiro = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);

                oRegistro.IdEfectorSolicitante = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                oRegistro.IdEspecialistaSolicitante = 0;
                oRegistro.FechaInicioSintomas = DateTime.Parse("01/01/1900");
                oRegistro.FechaUltimoContacto = DateTime.Parse("01/01/1900");


                oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(lblIdPaciente.Text));
                if (Request["Operacion"].ToString() == "Modifica")
                {
                    if (oRegistro.IdPaciente != oPaciente)
                        oRegistro.GrabarAuditoriaProtocolo("Cambia de Paciente", int.Parse(Session["idUsuario"].ToString()));
                }
                if (txtTelefono.Text!="")
                { oPaciente.InformacionContacto = txtTelefono.Text;
                    oPaciente.Save();
                }

                ///Desde aca guarda los datos del paciente en Protocolo
                oRegistro.IdPaciente = oPaciente;


                oRegistro.Edad = int.Parse(lblEdad.Text);
               
                    oRegistro.NumeroOrigen = txtNumeroOrigen.Text;
                oRegistro.NumeroOrigen2 = txtNumeroOrigen2.Text.Replace("HISOP00","").Replace("HISOP0","").Replace("HISOP", "");

                switch (lblUnidadEdad.Text.Trim())
                {
                    case "A": oRegistro.UnidadEdad = 0; break;
                    case "M": oRegistro.UnidadEdad = 1; break;
                    case "D": oRegistro.UnidadEdad = 2; break;
                }

                //oRegistro.Sexo = lblSexo.Text;

                switch (oPaciente.IdSexo)
                {
                    case 1: oRegistro.Sexo = "I"; break;
                    case 2: oRegistro.Sexo = "F"; break;
                    case 3: oRegistro.Sexo = "M"; break;
                }


                oRegistro.Embarazada = "N";
                oRegistro.Sala = txtSala.Text;
                oRegistro.Cama = txtCama.Text;


                 ObraSocial oObraSocial = new ObraSocial();
                oRegistro.IdObraSocial =   (ObraSocial)oObraSocial.Get(typeof(ObraSocial), -1); 
                
                oRegistro.NombreObraSocial = lblObraSocial.Text;
                oRegistro.CodOs = int.Parse(CodOS.Value);
                oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), int.Parse(this.ddlOrigen.SelectedValue));
                oRegistro.IdPrioridad = (Prioridad)oPri.Get(typeof(Prioridad), int.Parse(this.ddlPrioridad.SelectedValue));
                oRegistro.Observacion = txtObservacion.Text;
                oRegistro.ObservacionResultado = "";
                oRegistro.IdMuestra = int.Parse(ddlMuestra.SelectedValue);
                if (Request["Operacion"].ToString() != "Modifica")
                {
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;
                    oRegistro.IpCarga = "";
                    oRegistro.Impres = "";
                }

                oRegistro.Especialista = ddlEspecialista.SelectedItem.Text;
                oRegistro.MatriculaEspecialista = ddlEspecialista.SelectedValue;
                
                oRegistro.Save();
                oRegistro.ActualizarNumeroDesdeID();

               

                if (Request["Operacion"].ToString() != "Modifica") { if (Request["Operacion"].ToString() != "AltaPeticion") { if (Session["idUrgencia"] != null) { if (Session["idUrgencia"].ToString() == "0") AlmacenarSesion(oC); } } }
                    if (Request["Operacion"].ToString() == "AltaDerivacion") AlmacenarSesion(oC);
                
                //   if (Request["idSolicitudScreening"] != null) ActualizarSolicitudScreening(Request["idSolicitudScreening"].ToString(),oRegistro);

                GuardarDetalle(oRegistro);
                GuardarDiagnosticos(oRegistro);
                this.IncidenciaEdit1.GuardarProtocoloIncidencia(oRegistro);

                
                //oRegistro.VerificarExisteNumeroAsignado();
                
                oRegistro.GrabarAuditoriaProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));

              
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
        private void PreventingDoubleSubmit(Button button)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Processing...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
        }
        private void AlmacenarSesion(Configuracion oC)
        {


            string s_valores=""; //= "chkRecordarPractica:" + chkRecordarPractica.Checked;
            
            //if (chkRecordarConfiguracion.Checked)
            //{
            //    s_valores += "@chkImprimir:" + chkImprimir.Checked;
            //    s_valores += "@chkAreaCodigoBarra:" + getListaAreasCodigoBarras();
            //}
            //s_valores += "@chkRecordarConfiguracion:" + chkRecordarConfiguracion.Checked;
            //s_valores += "@ddlImpresora:" + ddlImpresora.SelectedValue;
            //s_valores += "@ddlImpresoraEtiqueta:" + ddlImpresoraEtiqueta.SelectedValue;

            //Session["Impresora"] = ddlImpresora.SelectedValue;
            //Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;

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
                    if (oC.NomencladorDiagnostico == 0)
                    {
                        Cie10 oD = new Cie10(oDetalle.IdDiagnostico);
                        oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", oD.Nombre);
                    }
                    else
                    {
                        DiagnosticoP oD = new DiagnosticoP(oDetalle.IdDiagnostico);

                        oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", oD.Nombre);
                    }
                    oDetalle.Delete();
                }

            }
            string listaDx = "";
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
                    if (listaDx=="")
                    listaDx = lstDiagnosticosFinal.Items[i].Value;
                    else
                        listaDx +=";"+ lstDiagnosticosFinal.Items[i].Value;
                }
            }

            Session["Dx"] = listaDx;

        }

        

        private void GuardarDetalle(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;

            if (Request["Operacion"].ToString() != "Alta")
            {
                // Modificacion de protocolo en proceso: Elimina los detalles que no se ingresaron por pantalla         
                

                ICriteria critBorrado = m_session.CreateCriteria(typeof(DetalleProtocolo));
                critBorrado.Add(Expression.Eq("IdProtocolo", oRegistro));
                IList detalleaBorrar = critBorrado.List();
                if (detalleaBorrar.Count > 0)
                {
                    foreach (DetalleProtocolo oDetalle in detalleaBorrar)
                    {

                        oDetalle.Delete();
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()));

                    }
                }
            }

            //int dias_espera = 0;
            string[] tabla = "1#9124#Si#False".Split('@');   // TxtDatosCargados.Value.Split('@');
     

            string recordar_practicas = "";

            for (int i = 0; i < tabla.Length ; i++)
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

                  //string trajomuestra = fila[3].ToString();

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
                        oDetalle.IdSubItem = oItem;
                        //if (dias_espera < oDetalle.IdItem.Duracion) dias_espera = oDetalle.IdItem.Duracion;

                        /*CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                        if (a.Checked)
                            oDetalle.TrajoMuestra = "Si";
                        else*/

                        //if (trajomuestra == "true")
                        //    oDetalle.TrajoMuestra = "No";
                        //else
                            oDetalle.TrajoMuestra = "Si";


                        oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                        oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                        oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                        oDetalle.Informable = oItem.Informable;
                        //if (oDetalle.IdItem.Codigo=="9124")
                        //{
                        Usuario oUser = new Usuario();
                            oUser = (Usuario)oUser.Get(typeof(Usuario), "Apellido", "UNIDADCOVID");
                            if (oUser != null)
                            { oDetalle.ConResultado = true;
                            oDetalle.ResultadoCar = ddlResultado.SelectedItem.Text;
                                oDetalle.FechaValida = DateTime.Today;
                                oDetalle.IdUsuarioValida = oUser.IdUsuario;
                              
                             }



                        //}

                        oDetalle.Save();
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Valida Resultado", oUser.IdUsuario);
                     


                        //GuardarDetallePractica(oDetalle);
                    }
                    else  //si ya esta actualizo si trajo muestra o no
                    {
                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {
                            //if (trajomuestra == "true")
                            //    oDetalle.TrajoMuestra = "No";
                            //else
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



          

            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        
            //if (oCon.TipoCalculoDiasRetiro == 0)

            //if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
            //    if (oCon.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
            //        oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(0));
            //    else   // calcula con los días predeterminados de espera
            //        oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oCon.DiasRetiro));
            //else
                oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(0);
            oRegistro.Estado = 2;




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
                    oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                    GuardarDetallePractica(oDetalle);
                }
            }
         

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
       //     DateTime fechaentrega;
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
                            oSItem = (Item)oSItem.Get(typeof(Item), oSubitem.IdItemDeterminacion);
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
                                oDetalle.IdSubItem = oSItem;
                                 oDetalle.TrajoMuestra = oDet.TrajoMuestra;
                                oDetalle.Informable = oSItem.Informable;
                                oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                                 oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                                oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");

                                oDetalle.Save();
                            }
                            i = i + 1;                        
                        }//fin if
                    }//fin foreach
                }         
                else
                {
                    oDet.IdSubItem = oDet.IdItem;
                    oDet.Informable = oDet.IdSubItem.Informable;
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

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            /////////Con la selección del item se muestra el codigo
            //if (ddlItem.SelectedValue != "0")
            //{
            //    Item oItem = new Item();
            //    oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
            //    txtCodigo.Text = oItem.Codigo;
                
            //}
            //else
            //    txtCodigo.Text = "";

            //txtCodigo.UpdateAfterCallBack = true;
            

        }



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
                        case "Default": Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
                        case "ProtocoloList": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Lista"); break;
                        case "Control": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Control"); break;
                        case "Urgencia": Response.Redirect("../Urgencia/UrgenciaList.aspx",false); break;
                        case "Derivacion": Response.Redirect("Derivacion.aspx?idServicio="+Session["idServicio"].ToString(), false); break;
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
                        Response.Redirect("Derivacion.aspx?idEfector=" + ddlEfector.SelectedValue+ "&idServicio=" + Session["idServicio"].ToString(), false);
                    else
                    {
                        if (Request["Operacion"].ToString() == "AltaPeticion")
                            Response.Redirect("../PeticionElectronica/PeticionList.aspx", false);
                        else
                        {
                            if (Session["idUrgencia"].ToString() != "0")
                                Response.Redirect("DefaultEfector.aspx?idServicio=1&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                            else
                                Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);
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

        private void AgregarRutina()
        {
            //Rutina oRutina = new Rutina();
            //oRutina = (Rutina)oRutina.Get(typeof(Rutina), int.Parse(ddlRutina.SelectedValue));

            //ISession m_session = NHibernateHttpModule.CurrentSession;
            //ICriteria crit = m_session.CreateCriteria(typeof(DetalleRutina));
            //crit.Add(Expression.Eq("IdRutina", oRutina));
            //IList detalle = crit.List();
            //if (detalle.Count > 0)
            //{
            //    string codigos="";
            //    foreach (DetalleRutina oDetalle in detalle)
            //    {

            //        if (codigos == "")
            //            codigos = oDetalle.IdItem.Codigo;
            //        else
            //            codigos += ";" + oDetalle.IdItem.Codigo;

                  

            //        //ddlRutina.SelectedValue = "0";
            //        //ddlRutina.UpdateAfterCallBack = true;


            //    }
            //    txtCodigosRutina.Text = codigos;
            //    txtCodigosRutina.UpdateAfterCallBack = true;

            //}

        }

       

        protected void ddlServicio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CargarItems();
            TxtDatos.Value = "";

        }

        protected void ddlRutina_SelectedIndexChanged(object sender, EventArgs e)
        {
             //if (ddlRutina.SelectedValue != "0")
             //AgregarRutina();
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
                //CargarSolicitantesExternos("");

                try { 
                Efector oEfectorExterno = new Efector();
                oEfectorExterno = (Efector)oEfectorExterno.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                    if (oEfectorExterno.IdTipoEfector == 2) // es privado
                    {
                        if (lblObraSocial.Text == "-") // es temporal
                        {
                            lblAlertaObraSocial.Text = "El efector derivante es privado verifique Financiador u Obra Social.";
                            lblAlertaObraSocial.Visible = true;                       

                        }
                    }
                    else lblAlertaObraSocial.Visible = false;

                switch (oEfectorExterno.IdZona.IdZona)
                {
                    case 1: // subse
                        ddlOrigen.SelectedValue = "5"; break;
                    case 2: // zona I
                        ddlOrigen.SelectedValue = "6"; break;
                    case 3: // zona II
                        ddlOrigen.SelectedValue = "8"; break; //7
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

                }
                catch {
                    ddlOrigen.SelectedValue = "0";
                }
                //btnGuardarSolicitante.Visible = true;
                //btnGuardarSolicitante.UpdateAfterCallBack = true;
                lblAlertaObraSocial.UpdateAfterCallBack = true;
                //btnCancelarSolicitante.Visible = true;
                //btnCancelarSolicitante.UpdateAfterCallBack = true;


                ddlOrigen.UpdateAfterCallBack = true;

            }
            else
            {
                ddlOrigen.SelectedValue = "0"; 
                ddlOrigen.UpdateAfterCallBack = true;

                //btnGuardarSolicitante.Visible = false;
                //btnGuardarSolicitante.UpdateAfterCallBack = true;

                //btnCancelarSolicitante.Visible = false;
                //btnCancelarSolicitante.UpdateAfterCallBack = true;
                //CargarSolicitantesInternos();
            }

        }

        //private void CargarSolicitantesInternos()
        //{
        //    Utility oUtil = new Utility();      
        //    ///Carga de combos de Medicos Solicitantes
        //   string  m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional ORDER BY apellido, nombre ";
        //    oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
        //    ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
        //    ddlEspecialista.UpdateAfterCallBack = true;
        //    //imgCrearSolicitante.Visible = false;
        //    //imgCrearSolicitante.UpdateAfterCallBack = true;
        //}

        //private void CargarSolicitantesExternos(string m_solicitante)
        //{
        //    Utility oUtil = new Utility();            

        //    ///Carga de combos de solicitantes expertos
        //    string m_ssql = "select idSolicitanteExterno, apellido + ', ' + nombre as nombre from Lab_SolicitanteExterno where baja=0 order by apellido, nombre";
        //    oUtil.CargarCombo(ddlEspecialista, m_ssql, "idSolicitanteExterno", "nombre");
        //    ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
        //    if (m_solicitante != "")                ddlEspecialista.SelectedValue = m_solicitante;
        //    ddlEspecialista.UpdateAfterCallBack = true;
        //    //imgCrearSolicitante.Visible = true;
        //    //imgCrearSolicitante.UpdateAfterCallBack = true;
        //}

        //protected void btnGuardarSolicitante_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        GuardarSolicitanteExterno();

        //        LimpiarDatosSolicitante();
        //        //Panel1.Visible = false;
        //        //Panel1.UpdateAfterCallBack = true;
        //    }
        //}

        //private void GuardarSolicitanteExterno()
        //{
        //    Usuario oUser = new Usuario();
        //    SolicitanteExterno oRegistro = new SolicitanteExterno();
        //    Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
        //    oRegistro.IdEfector = oC.IdEfector;
        //    oRegistro.Matricula = txtMatricula.Text;
        //    oRegistro.Apellido = txtApellidoSolicitante.Text;
        //    oRegistro.Nombre = txtNombreSolicitante.Text;
        //    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
        //    oRegistro.FechaRegistro = DateTime.Now;
        //    oRegistro.Save();
        //    CargarSolicitantesExternos(oRegistro.IdSolicitanteExterno.ToString());
        //}

        //protected void btnCancelarSolicitante_Click(object sender, EventArgs e)
        //{
        //    LimpiarDatosSolicitante();

        //}

        //private void LimpiarDatosSolicitante()
        //{
        //    txtMatricula.Text = "";
        //    txtApellidoSolicitante.Text = "";
        //    txtNombreSolicitante.Text = "";
        //}

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtCodigoDiagnostico_TextChanged(object sender, EventArgs e)
        {
           
            //BuscarCodigoDiagnostico();
             
        }

        private void BuscarCodigoDiagnostico()
        {
            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {

                if (oC.NomencladorDiagnostico == 0) /// Cie10
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
                else /// diagnostico propio
                {
                    DiagnosticoP oDiagnostico = new DiagnosticoP();
                    oDiagnostico = (DiagnosticoP)oDiagnostico.Get(typeof(DiagnosticoP), "Codigo", txtCodigoDiagnostico.Text);
                    if (oDiagnostico != null)
                    {
                        ListItem oDia = new ListItem();
                        oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                        oDia.Value = oDiagnostico.IdDiagnostico.ToString();
                        lstDiagnosticos.Items.Add(oDia);

                    }
                    else
                        lstDiagnosticos.Items.Clear();
                }

            }
            lstDiagnosticos.UpdateAfterCallBack = true;

        }

        protected void txtNombreDiagnostico_TextChanged(object sender, EventArgs e)
        {
           
            //BuscarNombreDiagnostico();
            
        }

        private void BuscarNombreDiagnostico()
        { lstDiagnosticos.Items.Clear();
            if (txtNombreDiagnostico.Text != "")
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                if (oC.NomencladorDiagnostico == 0)
                {
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

                    lstDiagnosticos.UpdateAfterCallBack = true;
                }

                else //nomenclador propio
                {
                    ICriteria crit1 = m_session.CreateCriteria(typeof(DiagnosticoP));
                  
                    crit1.Add(Expression.InsensitiveLike("Nombre", txtNombreDiagnostico.Text, MatchMode.Anywhere));
                    crit1.Add(Expression.Eq ( "Baja", false));
                    IList items = crit1.List();

                    foreach (DiagnosticoP oDiagnostico in items)
                    {
                        ListItem oDia = new ListItem();
                        oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                        oDia.Value = oDiagnostico.IdDiagnostico.ToString();
                        lstDiagnosticos.Items.Add(oDia);
                    }

                    lstDiagnosticos.UpdateAfterCallBack = true;
                }

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
            TxtDatosCargados.Value = TxtDatos.Value;

            string sDatos = "";

              string[] tabla = TxtDatos.Value.Split('@');
          
            for (int i = 0; i < tabla.Length - 1; i++)
            {
                string[] fila = tabla[i].Split('#');
                string codigo = fila[1].ToString();
                string muestra= fila[2].ToString();                
            
                    if (sDatos == "")
                        sDatos = codigo + "#" + muestra;
                    else
                        sDatos += ";" +  codigo + "#" + muestra;                                                        

            }

          


            TxtDatosCargados.Value = sDatos;

            if (Request["Operacion"].ToString() != "Modifica")
                if (!VerificarFechaPacienteMuestra())
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "No es posible ingresar para la misma fecha, muestra y paciente un nuevo protocolo.";
                    return;
                }

            if (!VerificarAnalisisContenidos() )
            {  TxtDatos.Value = "";
                args.IsValid = false;
             
                return;
            }
            else
            {

              

            ///

            //if ((TxtDatos.Value == "") || (TxtDatos.Value == "1###on@"))
            //    {

            //        args.IsValid = false;
            //        this.cvValidacionInput.ErrorMessage = "Debe completar al menos un análisis";
            //        return;
            //    }
            //    else args.IsValid = true;


                //validacion Diagnostico
                //if (oC.DiagObligatorio)
                //{if (lstDiagnosticosFinal.Items.Count == 0)
                //    {
                //        TxtDatos.Value = "";
                //        args.IsValid = false;
                //        this.cvValidacionInput.ErrorMessage = "Debe ingresar al menos un diagnóstico presuntivo del paciente";
                //        return;
                //    }
                //}

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
                if  (ddlCaracter.SelectedValue == "0")
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe seleccionar el caracter del protocolo";
                    return;
                }
                if ((ddlEspecialista.SelectedValue=="-1") && (oC.MedicoObligatorio))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la mátricula del médico solicitante";
                    return;
                }

                if ((lblAlertaObraSocial.Visible) &&  (lblObraSocial.Text == "-"))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la obra social/financiador";
                    return;
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
                    {
                        if (DateTime.Parse(txtFechaOrden.Value) > DateTime.Parse(txtFecha.Value))
                        {
                            TxtDatos.Value = "";
                            args.IsValid = false;
                            this.cvValidacionInput.ErrorMessage = "La fecha de la orden no puede ser superior a la fecha del protocolo";
                            return;
                        }
                        else
                            args.IsValid = true;
                    }
                }


              

                if (txtFechaTomaMuestra.Value == "")
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la fecha de toma de muestra";
                    return;
                }
                else
                {
                    if (DateTime.Parse(txtFechaTomaMuestra.Value) > DateTime.Now)
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "La fecha de toma de muestra no puede ser superior a la fecha actual";
                        return;
                    }
                    else
                    {
                        if (DateTime.Parse(txtFechaTomaMuestra.Value) > DateTime.Parse(txtFecha.Value))
                        {
                            TxtDatos.Value = "";
                            args.IsValid = false;
                            this.cvValidacionInput.ErrorMessage = "La fecha de toma de muestra no puede ser superior a la fecha del protocolo";
                            return;
                        }
                        else
                            args.IsValid = true;
                    }
                }
            }
        }

        private bool VerificaRequiereCaracter(string sDatos)
        {

            bool devolver = false;
            string[] tabla = sDatos.Split(';');


            for (int i = 0; i < tabla.Length - 1; i++)
            {
                string[] fila = tabla[i].Split('#');
                string codigo = fila[0].ToString();

                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo, "Baja", false);

                if (oItem.RequiereCaracter)
                {
                    devolver = true;
                    break;
                }

            }

            return devolver;
        }

        private bool VerificarFechaPacienteMuestra()
        {
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(HFIdPaciente.Value));

            string tieneingreso = oPaciente.GetFechaProtocolosReciente(Request["idServicio"].ToString(),ddlMuestra.SelectedValue, "9124" );
            if (tieneingreso==txtFecha.Value) return false;
            else return true;
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
            //Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            //oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));            

            //////Imprimir Comprobante para el paciente
            //Imprimir(oRegistro);

         
           
        }

        protected void lnkReimprimirCodigoBarras_Click(object sender, EventArgs e)
        {
            //Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            //oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            /////Imprimir codigo de barras.
            //string s_AreasCodigosBarras = getListaAreasCodigoBarras();
            //if (s_AreasCodigosBarras != "")
            //{
                 
            //    ImprimirCodigoBarras(oRegistro, getListaAreasCodigoBarras());
            //}
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
                            if (i < lista.Length - 1)
                            {
                                ProtocoloNuevo = int.Parse(lista[i + 1].ToString()); break;
                            }
                        }
                        else
                        {
                            if (avance == 0)
                            {
                                if (i < lista.Length - 1)
                                {
                                    ProtocoloNuevo = int.Parse(lista[i].ToString()); break;
                                }
                            }
                            else
                            if (i > 0)
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
                    Response.Redirect("ProtocoloEdit2.aspx?Desde="+Request["Desde"].ToString()+"&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
                //else
                //    Response.Redirect("ProtocoloEdit2.aspx?Desde="+Request["Desde"].ToString()+"&idServicio=" + Session["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
            }
            else
                Response.Redirect("ProtocoloEdit2.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloActual + m_parametro);                                                               
               
        }



        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
              if (e.CommandName== "Modificar")
                  Response.Redirect("ProtocoloEdit2.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + e.CommandArgument.ToString());
            

        }

        protected void btnBusquedaDiagnostico_Click(object sender, EventArgs e)
        {
            
            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {
                BuscarCodigoDiagnostico();
            }

            if (txtNombreDiagnostico.Text != "")
            {
                BuscarNombreDiagnostico();
               
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
            if (oC.NomencladorDiagnostico==1) //prppio
                m_ssql = "SELECT idDiagnostico as ID, Codigo + ' - ' + Nombre as nombre FROM lab_Diagnostico WHERE (idDiagnostico IN (SELECT DISTINCT idDiagnostico FROM LAB_ProtocoloDiagnostico)) ORDER BY Nombre";
            oUtil.CargarListBox(lstDiagnosticos, m_ssql, "id", "nombre");
            lstDiagnosticos.UpdateAfterCallBack = true;


        }

        protected void btnBusquedaFrecuente_Click(object sender, EventArgs e)
        {
            CargarDiagnosticosFrecuentes();
        }

        protected void rdbSeleccionarAreasEtiquetas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rdbSeleccionarAreasEtiquetas.SelectedValue == "1")
            //    MarcarTodasAreas(true);
            //else
            //    MarcarTodasAreas(false);
            //chkAreaCodigoBarra.UpdateAfterCallBack = true;
        }

        private void MarcarTodasAreas(bool p)
        {
            //for (int i = 0; i < chkAreaCodigoBarra.Items.Count; i++)
            //    chkAreaCodigoBarra.Items[i].Selected = p;

        }

        protected void btnSelObraSocial_Click(object sender, EventArgs e)
        {
            actualizarObraSocial();
        }

        private void actualizarObraSocial()
        {
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(HFIdPaciente.Value));

            ObraSocial oObraSocial = new ObraSocial();
            oObraSocial = (ObraSocial)oObraSocial.Get(typeof(ObraSocial), int.Parse(oPaciente.IdObraSocial.ToString()));
      
            lblObraSocial.Text = oObraSocial.Nombre;

            if (Request["idProtocolo"]!= null)
            { 
            TxtDatos.Value = "";
            TxtDatosCargados.Value = "";
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

            MostrarDeterminaciones(oRegistro);
            }
            else
                TxtDatos.Value = "";
        }

        protected void lnkValidarRenaper_Click(object sender, EventArgs e)
        {
            Avanzar(0);
        }

        protected void txtEspecialista_TextChanged(object sender, EventArgs e)
        {
            MostrarMedico();
        }

        private void MostrarMedico()
        {
            try
            {
                ///Buscar especilista
                string matricula = txtEspecialista.Text;
                string s_urlWFC = oC.UrlMatriculacion;
                string s_url = s_urlWFC + "numeroMatricula=" + matricula + "&codigoProfesion=1";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s_url);
                HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                Stream st = ws1.GetResponseStream();
                StreamReader sr = new StreamReader(st);

                string s = sr.ReadToEnd();
                if (s != "0")
                {

                    List<ProfesionalMatriculado> pro = jsonSerializer.Deserialize<List<ProfesionalMatriculado>>(s);
                    string espe;
                    if (pro.Count > 0)
                    {
                        ddlEspecialista.Items.Clear();

                        for (int i = 0; i < pro.Count; i++)
                        {
                            espe = pro[i].apellido + " " + pro[i].nombre;
                            //documento = pro[i].documento.ToString();
                            ddlEspecialista.Items.Insert(0, new ListItem(espe, matricula));
                        }
                        if (pro.Count > 1)
                            ddlEspecialista.Items.Insert(0, new ListItem("--Seleccione--", "0"));

                        lblErrorMedico.Visible = false;

                    }
                    else
                    { //error no encontrado}
                        if (matricula == "0")
                        {
                            ddlEspecialista.Items.Clear();
                            ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
                        }
                        else
                        {

                            lblErrorMedico.Text = "Médico no encontrado!!";
                            lblErrorMedico.Visible = true;
                            ddlEspecialista.Items.Clear();
                            ddlEspecialista.Items.Insert(0, new ListItem("Médico no encontrado", "-1"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ddlEspecialista.Items.Clear();
                ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
            }

            ddlEspecialista.UpdateAfterCallBack = true;
            lblErrorMedico.UpdateAfterCallBack = true;


        }

        public class Profesiones
        {
            public List<Matricula> matriculacion { get; set; }
        }

        public class Matricula

        {
            public string  matriculaNumero { get; set; }
        }
        public class ProfesionalMatriculado
        {
            public int documento { get; set; }
            public string nombre { get; set; }
            public string apellido { get; set; }
            public List<Profesiones> profesiones { get; set; }
            //public string Nombre { get; set; }
            //public string FechaNacimiento { get; set; }
            //public string FechaNac { get; set; }
            //public string Sexo { get; set; }
            ////public string Efector { get; set; }
            ////public int idTipoServicio { get; set; }
            //public string NumeroProtocolo { get; set; }
            //public string FechaProtocolo { get; set; }
            //public string CodigoAnalisis { get; set; } // los codigos de analisis separados por "|"
            //public string Codigos { get; set; } // los codigos de analisis separados por "|"
            //public string Sala { get; set; }
            //public string Cama { get; set; }
            //public int TipoServicio { get; set; }
            //public string Calle { get; set; }
            //public int NumeroDomicilio { get; set; }
            //public string Referencia { get; set; }
            //public string InformacionContacto { get; set; }

            //public int DocumentoParentesco { get; set; }
            //public string ApellidosParentesco { get; set; }
            //public string NombresParentesco { get; set; }
            //public string FechaNacimientoParentesco { get; set; }
            //public string TipoParentesco { get; set; }
            //public string DiagnosticoEmbarazo { get; set; }
            ///// <summary>
            ///// /varaiables screening
            ///// </summary>
            //public int idEfectorSolicitante { get; set; }
            //public string horaNacimiento { get; set; }
            //public int edadGestacional { get; set; }
            //public int peso { get; set; }
            //public string primeraMuestra { get; set; }
            //public int idMotivoRepeticion { get; set; }
            //public string fechaExtraccion { get; set; }
            //public string horaExtraccion { get; set; }
            //public string ingestaLeche24Horas { get; set; }
            //public string tipoAlimentacion { get; set; }
            //public string antibiotico { get; set; }
            //public string transfusion { get; set; }
            //public string corticoide { get; set; }
            //public string dopamina { get; set; }
            //public string enfermedadTiroideaMaterna { get; set; }
            //public string antecedentesMaternos { get; set; }
            //public string corticoidesMaterno { get; set; }
            //public string medicoSolicitante { get; set; }
            //public string numeroTarjeta { get; set; }
            //public int idLugarControl { get; set; }
            //public string fechaCarga { get; set; }
            //public string fechaEnvio { get; set; }



            //public int idProtocolo { get; set; }
            //public int idEfector { get; set; }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Session["matricula"] != null)
            {

                txtEspecialista.Text = Session["matricula"].ToString();
                MostrarMedico();
                TxtDatos.Value = "";
            }
        }

        protected void btnRecordarDiagnostico_Click(object sender, EventArgs e)
        {
            try
            {
                string dx = Session["Dx"].ToString();
                string[] tabla = dx.Split(';');

                /////Crea nuevamente los detalles.
                for (int i = 0; i <= tabla.Length - 1; i++)
                {
                    Cie10 oD = new Cie10();
                    oD = (Cie10)oD.Get(typeof(Cie10), int.Parse(tabla[i].ToString()));
                    ListItem oDia = new ListItem();
                    oDia.Text = oD.Nombre;
                    oDia.Value = oD.Id.ToString();
                    lstDiagnosticosFinal.Items.Add(oDia);

                }
                lstDiagnosticosFinal.UpdateAfterCallBack = true;
            }
            catch (Exception ex)
            { }
        }
        public class RespuestaCaso
        {
            public string id_caso { get; set; }

            public string resultado { get; set; }

            public string description { get; set; }
        }
        public class AltaCaso
        {
            public string usuario { get; set; }
            public string clave { get; set; }
            public evento altaEventoCasoNominal { get; set; }

        }
        public class evento
        {
            public string idTipodoc { get; set; }
            public string nrodoc { get; set; }
            public string sexo { get; set; }
            public string fechaNacimiento { get; set; }
            public string idGrupoEvento { get; set; }

            public string idEvento { get; set; }
            public string idEstablecimientoCarga { get; set; }
            public string fechaPapel { get; set; }
            public string idClasificacionManualCaso { get; set; }


        }
        protected void btnNotificarSISA_Click(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;
            TxtDatos.Value = "";
            Business.Data.Laboratorio.Protocolo protocolo = new Business.Data.Laboratorio.Protocolo();
            protocolo = (Business.Data.Laboratorio.Protocolo)protocolo.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

           
            string idevento = "307";
           
            string idclasificacionmanual = "";
           
            string idgrupoevento = "113";
            

            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", "9124", "Baja", false);
            if (oItem != null)
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;

                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                crit.Add(Expression.Eq("IdProtocolo", protocolo));
                crit.Add(Expression.Eq("IdItem", oItem));
                IList listadetalle = crit.List();

                foreach (DetalleProtocolo oDetalle in listadetalle)
                {
                    if (oDetalle.ResultadoCar == "REACTIVO")
                    {
                        idclasificacionmanual = "795";
                       // "Caso Confirmado por Test de Antígeno en terreno (2019-nCoV)";
                        //idclasificacionmanual = "754";
                        //nombreclasificacionmanual = "Caso confirmado de nuevo coronavirus (2019-nCoV)";
                    }
                    if (oDetalle.ResultadoCar == "NO REACTIVO")
                    {
                        idclasificacionmanual = "752";
                        // "Caso sospechosos de nuevo coronavirus (2019-nCoV)";
                    }
                }

                //txtGrupoEvento.Value = idgrupoevento;
                //lblGrupoEvento.Text = nombregrupoevento;
                //txtEvento.Value = idevento;
                //lblEvento.Text = nombreevento;
                //txtClasificacionManualCaso.Value = idclasificacionmanual;
                //lblClasificacionManual.Text = nombreclasificacionmanual;

            }

            try
            {

                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                string URL = oCon.UrlServicioSISA;
                string s_idestablecimiento = oCon.CodigoEstablecimientoSISA; // "14580562167000"
                string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
                string[] a = usersisa.Split(':');
                string s_user = a[0].ToString();
                string s_userpass = a[1].ToString();
                string s_sexo = "";
                switch (protocolo.IdPaciente.IdSexo)
                {
                    case 1: s_sexo = "I"; break;
                    case 2: s_sexo = "F"; break;
                    case 3: s_sexo = "M"; break;
                }
                
                
              
                 
                string fn = protocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");
                string s_idEvento = idevento;
                string fnpapel = protocolo.FechaOrden.ToShortDateString().Replace("/", "-");
                string numerodocumento = protocolo.IdPaciente.NumeroDocumento.ToString();
                string clasificacionmanual = idclasificacionmanual;
                
              
                string error = "";




                evento newevento = new evento
                {
                    idTipodoc = "1",
                    nrodoc = numerodocumento,
                    sexo = s_sexo,
                    fechaNacimiento = fn,  //"05-06-1989",
                    idGrupoEvento = idgrupoevento,
                    idEvento = s_idEvento, // "77",
                    idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                    fechaPapel = fnpapel, // "10-12-2019",
                    idClasificacionManualCaso = clasificacionmanual, // "22"
                };

                AltaCaso caso = new AltaCaso
                {

                    usuario = s_user, // "bcpintos",
                    clave = s_userpass, // "2398HH6RK6",
                    altaEventoCasoNominal = newevento
                };

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                string DATA = jsonSerializer.Serialize(caso);



                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                client.BaseAddress = new System.Uri(URL);

                System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");
                HttpResponseMessage messge = client.PostAsync(URL, content).Result;
                string description = string.Empty;
                if (messge.IsSuccessStatusCode)
                {
                    string result = messge.Content.ReadAsStringAsync().Result;
                    description = result;
                    RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(description);


                    if (respuesta_d.id_caso != "")
                    { //  devolver el idcaso para guardar en la base de datos
                        string s_idcaso = respuesta_d.id_caso;
                        ///grabar a protocolo idCaso
                        //Protocolo protocolo = new Protocolo();
                        //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));
                        if (protocolo != null)
                        {
                            protocolo.IdCasoSISA = int.Parse(s_idcaso);
                            protocolo.Save();
                            if (respuesta_d.resultado == "OK")
                                protocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                            else // ERROR_DATOS
                                protocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                        }

                    }
                    else
                    {
                        
                        error = respuesta_d.resultado + ": " + respuesta_d.description;

                    }
                }

             

            }
            catch /*(Exception e)*/
            {
                //lblError.Text = "Hubo algun problema al conectar al servicio SISA. Intente de nuevo o haga clic en Salir";
                //lblError.Visible = true;
                //btnSalir.Visible = true;
            }




        }
    }
    
}

