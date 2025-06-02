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
using System.Net.Http;
using System.Web;
using WebLab.Resultados;

namespace WebLab.Protocolos
{
    public partial class ProtocoloEdit2 : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            //MiltiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (this.oCr.ReportDocument != null)
                {
                    this.oCr.ReportDocument.Close();
                    this.oCr.ReportDocument.Dispose();
                }
            }
            catch { }
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
                rdr = command.ExecuteReader();

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
            string m_strSQL = @" Select   P.idProtocolo,   numero as numero, convert(varchar(10),P.fecha,103) as fecha,P.estado 
                               from Lab_Protocolo P with (nolock) 
                               WHERE P.idProtocolo in (" + Session["ListaProtocolo"].ToString() + ") order by numero ";

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
                    if (Request["idUrgencia"] != null) Session["idUrgencia"] = Request["idUrgencia"].ToString();
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
                        if (Request["Desde"].ToString() == "Control")
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
                        if (oC.ValorDefectoFechaOrden == 0) txtFechaOrden.Value = "";
                        else txtFechaOrden.Value = DateTime.Now.ToShortDateString();

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
                            tableTitulo.Attributes.Add("class", "tituloCeldaVerde");
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
                                CargarDeterminacionesDerivacion(Request["analisis"].ToString(), Request["diagnostico"].ToString());
                                ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina


                            }
                            if (Request["Operacion"].ToString() == "AltaDerivacionMultiEfector" || Request["Operacion"].ToString() == "AltaDerivacionMultiEfectorLote")
                            {

                               string numeroProtocolo= Request["numeroProtocolo"].ToString();
                                string analisis= Request["analisis"].ToString();
                                CargarProtocoloDerivado(numeroProtocolo, analisis);
                                
                                

                            }
                            if (Request["Operacion"].ToString() == "AltaFFEE")
                            {
                                 
                                string idFicha = Request["idFicha"].ToString();
                                 CargarFFEE(  idFicha);



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
        private SectorServicio BuscarSectorDefecto()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = "select idsil  from Rel_andes with (nolock) where tipo='Sector'  ";
            SectorServicio oSector = new SectorServicio();

            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

            m_ssql = null;
            oUtil = null;
            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                sTareas = ds.Tables["T"].Rows[i][0].ToString();

                oSector = (SectorServicio)oSector.Get(typeof(SectorServicio), int.Parse(sTareas));

            }
            return oSector;

        }
        private void CargarFFEE(  string idFicha)
        {
            Utility oUtil = new Utility();
            //Actualiza los datos de los objetos : alta o modificacion .

            Business.Data.Laboratorio.Ficha oRegistro = new Business.Data.Laboratorio.Ficha();
            oRegistro = (Business.Data.Laboratorio.Ficha)oRegistro.Get(typeof(Business.Data.Laboratorio.Ficha), "IdFicha", idFicha);
            //oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));
            if (oRegistro != null)
            {
                hplActualizarPaciente.Visible = false;
                hplModificarPaciente.Visible = false;

                //        oRegistro.IdEfector = oC.IdEfector;
                SectorServicio oSector = new SectorServicio();
                oSector = BuscarSectorDefecto();



                lblTitulo.Visible = false;
                txtFecha.Value = DateTime.Now.ToShortDateString();
                txtFechaOrden.Value = oRegistro.Fecha.ToShortDateString();///fecha en la ficha
                txtFechaTomaMuestra.Value = DateTime.Now.ToShortDateString();

                pnlNavegacion.Visible = false;
                btnCancelar.Text = "Cancelar";
                btnCancelar.Width = Unit.Pixel(80);

                txtNumeroOrigen.Text = oRegistro.Identificadorlabo;
                ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString(); SelectedEfector();
                ddlOrigen.SelectedValue = "1";// oRegistro.IdOrigen.IdOrigen.ToString();//ver el origen 
                ddlSectorServicio.SelectedValue =  oSector.IdSectorServicio.ToString();// oRegistro.IdSector.IdSectorServicio.ToString(); //ver el servicio 
                ddlPrioridad.SelectedValue = "1";// oRegistro.IdPrioridad.IdPrioridad.ToString();
                                                 //if (oRegistro.IdTipoServicio.IdTipoServicio == 3) ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();


                txtEspecialista.Text = "9999";//oRegistro.MatriculaEspecialista;
                string espe = oRegistro.Solicitante;
                string matricula = "9999" + '#' + oRegistro.Solicitante;  //oRegistro.MatriculaEspecialista + '#' + oRegistro.Especialista;
                //      MostrarMedico();
                ddlEspecialista.Items.Insert(0, new ListItem(espe, matricula + '#' + espe));
                //if ((matricula == oRegistro.MatriculaEspecialista) && (oRegistro.Especialista== apellidoynombre))
                ddlEspecialista.SelectedValue = "9999" + '#' + oRegistro.Solicitante;
                ddlEspecialista.UpdateAfterCallBack = true;

                ddlMuestra.SelectedValue = oRegistro.IdTipoMuestra.ToString();

                txtFechaFIS.Value= oRegistro.FechaSintoma.ToShortDateString();

                if (oRegistro.Analisis != "")
                {
                    CargarDeterminacionesDerivacionMultiEfector(oRegistro.Analisis);
                }

                CargarDiagnosticoFicha(oRegistro.TipoFicha);



            }

        }


        private void CargarDiagnosticoFicha(string Tipo_ficha)
        {///CARO poner en la tabla las determinaciones por ficha dengue /sifilis
         //   Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = @"select nombresil from    Rel_andes where tipo='Diagnostico' and nombreAndes='Dengue' ";
            
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

        //    m_ssql = null;
        //    oUtil = null;
          
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                string diag = ds.Tables["T"].Rows[i][0].ToString();
                if (diag != "")
                { 

                        if (oC.NomencladorDiagnostico == 0) //cie10
                        {
                            Cie10 oD = new Cie10();
                            oD = (Cie10)oD.Get(typeof(Cie10),"Codigo", diag);
                        if (oD != null)
                        {
                            ListItem oDia = new ListItem();
                            oDia.Text = oD.Codigo + " - " + oD.Nombre;
                            oDia.Value = oD.Id.ToString();
                            lstDiagnosticosFinal.Items.Add(oDia);
                        }


                        }
                        else
                        {
                            DiagnosticoP oD = new DiagnosticoP();
                            oD = (DiagnosticoP)oD.Get(typeof(DiagnosticoP),  "Codigo", diag);
                        if (oD != null)
                        {
                            ListItem oDia = new ListItem();
                            oDia.Text = oD.Codigo + " - " + oD.Nombre;
                            oDia.Value = oD.IdDiagnostico.ToString();
                            lstDiagnosticosFinal.Items.Add(oDia);
                        }
                        }
                    
                }
            }
            



        }

        private void CargarProtocoloDerivado(string numeroProtocolo, string analisis)
        {

            Utility oUtil = new Utility();
            //Actualiza los datos de los objetos : alta o modificacion .
    
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), "Numero", int.Parse(numeroProtocolo));
            //oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));
            if (oRegistro != null)
            {
                hplActualizarPaciente.Visible = false;
                hplModificarPaciente.Visible = false;


                lblTitulo.Visible = false;
                txtFecha.Value = DateTime.Now.ToShortDateString();
                txtFechaOrden.Value = oRegistro.FechaOrden.ToShortDateString();
                txtFechaTomaMuestra.Value = oRegistro.FechaTomaMuestra.ToShortDateString();                           

                pnlNavegacion.Visible = false;
                btnCancelar.Text = "Cancelar";
                btnCancelar.Width = Unit.Pixel(80);

                txtNumeroOrigen.Text = oRegistro.Numero.ToString();
                ddlEfector.SelectedValue = oRegistro.IdEfector.IdEfector.ToString(); SelectedEfector();
                txtSala.Text = oRegistro.Sala.ToString();
                txtCama.Text = oRegistro.Cama;
                ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();
                ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();                
                ddlPrioridad.SelectedValue = oRegistro.IdPrioridad.IdPrioridad.ToString();
                if (oRegistro.IdTipoServicio.IdTipoServicio == 3) ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
                             
                txtEspecialista.Text = oRegistro.MatriculaEspecialista;
                string espe = oRegistro.Especialista;
                string matricula = oRegistro.MatriculaEspecialista + '#' + oRegistro.Especialista;
                //      MostrarMedico();
                ddlEspecialista.Items.Insert(0, new ListItem(espe, matricula + '#' + espe));
                //if ((matricula == oRegistro.MatriculaEspecialista) && (oRegistro.Especialista== apellidoynombre))
                ddlEspecialista.SelectedValue = oRegistro.MatriculaEspecialista + '#' + oRegistro.Especialista;
                ddlEspecialista.UpdateAfterCallBack = true;

                CargarDeterminacionesDerivacionMultiEfector(Request["analisis"].ToString());

                txtObservacion.Text = oRegistro.Observacion;

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
                    oDia.Value = oD.IdDiagnostico.ToString(); lstDiagnosticosFinal.Items.Add(oDia);
                }



            }
        }


        private void CargarDeterminacionesDerivacionMultiEfector(string s_analisis)
        {            
            string[] tabla = s_analisis.Split('|');
            string pivot = "";
            string sDatos = "";

            /////Crea nuevamente los detalles.
            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(tabla[i].ToString()));
                if (oItem!= null)
                    if (pivot != oItem.Nombre)
                    {
                        if (sDatos == "")
                            sDatos = oItem.Codigo + "#Si#False";
                        else
                            sDatos += ";"  +oItem.Codigo + "#Si#False";
                        //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#" + oDet.TrajoMuestra + "@";                   
                        pivot = oItem.Nombre;
                    }
            }             
            TxtDatosCargados.Value = sDatos;
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

            if (oTurno != null)
            {
                ddlEfector.SelectedValue = oTurno.IdEfectorSolicitante.IdEfector.ToString();
                ddlSectorServicio.SelectedValue = oTurno.IdSector.ToString();
                txtEspecialista.Text = oTurno.MatriculaEspecialista;                
                ddlEspecialista.Items.Insert(0, new ListItem(oTurno.Especialista, oTurno.MatriculaEspecialista + '#' + oTurno.Especialista));
                ddlEspecialista.SelectedValue = oTurno.MatriculaEspecialista + '#' + oTurno.Especialista;
                ddlEspecialista.UpdateAfterCallBack = true;
                lblObraSocial.Text = oTurno.NombreObraSocial; // oRegistro.IdObraSocial.Nombre;
                CodOS.Value = oTurno.CodOs.ToString();


                if ((lblObraSocial.Text == "-") || (lblObraSocial.Text == "Sin obra social"))
                    btnSelObraSocial.Visible = true;
                else
                    btnSelObraSocial.Visible = false;

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
                            sDatos += ";" + oDet.IdItem.Codigo + "#Si";

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
        }

        private void MuestraDatos()
        {
            Utility oUtil = new Utility();            
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            if (oRegistro != null)
            {
                oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));                 
                if (oRegistro.IdTipoServicio.IdTipoServicio == 6)
                {
                    lblSalaCama.Visible = false;
                    txtSala.Visible = false;
                    txtCama.Visible = false;


                    pnlTitulo.Attributes.Add("class", "panel panel-success");
                    tableTitulo.Attributes.Add("class", "tituloCeldaVerde");
                    btnGuardar.CssClass = "btn btn-success";
                    btnCancelar.CssClass = "btn btn-success";
                }

                if (oRegistro.tieneAdjuntoProtocolo())
                { spanadjunto.Visible = true; }
                else
                { spanadjunto.Visible = false; }

                if (oRegistro.IdPrioridad.IdPrioridad == 2)
                    Session["idUrgencia"] = 2;
                else
                    Session["idUrgencia"] = 0;

                pnlIncidencia.Visible = true;
                tituloCalidad.Visible = true;
                this.IncidenciaEdit1.Visible = true;
                this.IncidenciaEdit1.MostrarDatosdelProtocolo(oRegistro.IdProtocolo);


                int cantidadIncidencias = oRegistro.getIncidencias();
                if (cantidadIncidencias > 0)
                    inci.Visible = true;
                else
                    inci.Visible = false;


                ///Cambiar de Paciente: LLeva a elección de un otro paciente para asignarlo al protocolo.
                hplModificarPaciente.NavigateUrl = "Default2.aspx?Operacion=Modifica&idUrgencia=" + Session["idUrgencia"].ToString() + "&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString();
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



                if (oC.TipoNumeracionProtocolo == 2)
                {
                    //   lblTitulo.Text = oRegistro.PrefijoSector;
                    ///Si es la numeracion es con letra no se puede modificar el prefijo sector.
                    ddlSectorServicio.Enabled = false;
                    ///////////////////////////
                }

                lblUsuario.Text = oRegistro.IdUsuarioRegistro.Apellido + " " + oRegistro.FechaRegistro.ToString();

                lblTitulo.Text += oRegistro.Numero.ToString(); //.GetNumero().ToString();

                //ddlServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();

             //   CargarItems();  ///Caro Performance no es necesario llamar nuevamente
                txtFecha.Value = oRegistro.Fecha.ToShortDateString();
                txtFechaOrden.Value = oRegistro.FechaOrden.ToShortDateString();
                txtFechaTomaMuestra.Value = oRegistro.FechaTomaMuestra.ToShortDateString();

                txtSala.Text = oRegistro.Sala;
                txtCama.Text = oRegistro.Cama;

                ddlCaracter.SelectedValue = oRegistro.IdCaracter.ToString();

                txtNumeroOrigen.Text = oRegistro.NumeroOrigen;
                txtNumeroOrigen2.Text = oRegistro.NumeroOrigen2;

            
                chkNotificar.Checked = oRegistro.Notificarresultado;
                // muestra FIS
                chkSinFIS.Checked = false;
                chkSinFUC.Checked = false;
                if (cantidadIncidencias > 0)
                { 
                    chkSinFIS.Checked = oRegistro.VerificaIncidencia(46);
                    if (!chkSinFIS.Checked)
                    {
                        if (oRegistro.FechaInicioSintomas.Year != 1900)
                            txtFechaFIS.Value = oRegistro.FechaInicioSintomas.ToShortDateString();
                    }
                    // muestra FUC
                    chkSinFUC.Checked = oRegistro.VerificaIncidencia(47);
                    if (!chkSinFUC.Checked)
                    {
                        if (oRegistro.FechaUltimoContacto.Year != 1900)
                            txtFechaFUC.Value = oRegistro.FechaUltimoContacto.ToShortDateString();
                    }
                }

                if (oRegistro.IdCasoSISA > 0)
                { lblNroSISA.Text = "Nro. SISA: " + oRegistro.IdCasoSISA.ToString(); lblNroSISA.Visible = true; }
                txtEspecialista.Text = oRegistro.MatriculaEspecialista;
                string espe = oRegistro.Especialista;
                string matricula = oRegistro.MatriculaEspecialista + '#' + oRegistro.Especialista;
                //      MostrarMedico();
                ddlEspecialista.Items.Insert(0, new ListItem(espe, matricula + '#' + espe));
                //if ((matricula == oRegistro.MatriculaEspecialista) && (oRegistro.Especialista== apellidoynombre))
                ddlEspecialista.SelectedValue = oRegistro.MatriculaEspecialista + '#' + oRegistro.Especialista;
                ddlEspecialista.UpdateAfterCallBack = true;
                ///Datos del Paciente
                if (Request["idPaciente"] == null)
                {
                    HFNumeroDocumento.Value = oRegistro.IdPaciente.NumeroDocumento.ToString();
                    HFIdPaciente.Value = oRegistro.IdPaciente.IdPaciente.ToString();
                    lblIdPaciente.Text = oRegistro.IdPaciente.IdPaciente.ToString();
                    if (oRegistro.IdPaciente.IdEstado != 2)
                        lblPaciente.Text = oRegistro.IdPaciente.NumeroDocumento.ToString() + " - " + oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();
                    if (oRegistro.IdPaciente.IdEstado == 2)
                        lblPaciente.Text = oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper() + " (Temporal:" + oRegistro.IdPaciente.NumeroAdic + ") ";

                    if (oRegistro.IdPaciente.IdEstado == 3)
                    {
                        logoRenaper.Visible = true;
                    }
                    else
                        logoRenaper.Visible = false;

                    if (oRegistro.IdCasoSISA > 0)
                        btnNotificarSISA.Visible = false;
                    else
                    {

                        if (oC.NotificarSISA)
                        {
                            if (oRegistro.VerificaObligatoriedadFIS()) //sospechoso o detectar: cambiar segun parametrizacion de tabla
                                                                       //                                                                //   if ((oRegistro.IdCaracter == 1) || (oRegistro.IdCaracter == 8)) //sospechoso o detectar
                            {

                                string idItem = oRegistro.GenerarCasoSISA(); // se fija si hay algun item que tiene configurado notificacion a sisa
                                if (idItem != "")
                                { btnNotificarSISA.Visible = true; }
                            }
                        }
                    }



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

                    if ((oRegistro.IdPaciente.IdEstado == 1) && ((lblSexo.Text == "F") || (lblSexo.Text == "M")))
                        lnkValidarRenaper.Visible = true;
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




                ddlEfector.Items.Insert(0, new ListItem(oRegistro.IdEfectorSolicitante.Nombre, oRegistro.IdEfectorSolicitante.IdEfector.ToString()));
                ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString();
                //if (oRegistro.IdEfectorSolicitante!= oRegistro.IdEfector)
                //    CargarSolicitantesExternos("");
                //else
                //    CargarSolicitantesInternos();

                ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();
                //    ddlEspecialista.SelectedValue = oRegistro.IdEspecialistaSolicitante.ToString();



                ///Carga de combos de Origen        

                ddlOrigen.Items.Insert(0, new ListItem(oRegistro.IdOrigen.Nombre, oRegistro.IdOrigen.IdOrigen.ToString()));

                ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();

                ////

                ddlPrioridad.SelectedValue = oRegistro.IdPrioridad.IdPrioridad.ToString();


                ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
                txtObservacion.Text = oRegistro.Observacion;


                MostrarDeterminaciones(oRegistro);

                //TxtDatos.Value = sDatos;


                MostrarDiagnosticos(oRegistro);
           


                //chkRecordarConfiguracion.Checked = false;
                //chkRecordarPractica.Checked = false;


                //chkRecordarConfiguracion.Visible = false;
                //chkRecordarPractica.Visible = false;
                //chkImprimir.Visible = false;
                //chkRecordarConfiguracion.Visible = false;

                if (oRegistro.Estado == 2) btnGuardar.Visible = oC.ModificarProtocoloTerminado;
            }
        }

        private void MostrarDiagnosticos(Protocolo oRegistro)
        {


            if (oC.NomencladorDiagnostico == 0) //cie10
            {

                string m_strSQL = @"select c.id, c.codigo + ' -' + c.nombre 
from sys_cie10 c (nolock)
inner join LAB_ProtocoloDiagnostico pd (nolock) on c.id = pd.idDiagnostico
where pd.idProtocolo=" + oRegistro.IdProtocolo.ToString();

            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
                lstDiagnosticosFinal.Items.Clear(); diag.Visible = false;
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                diag.Visible = true;

                ListItem oDia = new ListItem();
                oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                    lstDiagnosticosFinal.Items.Add(oDia);


            }



          



            

                }
                else
                {
                    //DiagnosticoP oD = new DiagnosticoP();
                    //oD = (DiagnosticoP)oD.Get(typeof(DiagnosticoP), oDiag.IdDiagnostico);

                    //ListItem oDia = new ListItem();
                    //oDia.Text = oD.Codigo + " - " + oD.Nombre;
                    //oDia.Value = oD.IdDiagnostico.ToString();
                    //lstDiagnosticosFinal.Items.Add(oDia);
                }
             

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
                if (pivot != oDet.IdItem.Nombre)
                {
                    /*if (sDatos == "")
                        sDatos = oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                    else
                        sDatos += ";" + oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;                    
                    pivot = oDet.IdItem.Nombre;
                    */
                    string estado = "0";
                    if ((oDet.IdUsuarioValida > 0)  || (oDet.IdUsuarioValidaObservacion > 0)) //validado
                        estado = "2";
                    else
                    {
                        if ((oDet.IdUsuarioResultado > 0) || (oDet.IdUsuarioObservacion> 0) ||  (oDet.Enviado==2)) //cargado
                            estado = "1";
                    }

                            if (sDatos == "")
                        sDatos = oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + estado;
                    else
                        sDatos += ";" + oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + estado;
                    pivot = oDet.IdItem.Nombre;
                }

                

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
            if (oPaciente != null)
            {
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
                {
                    lblPaciente.Text = oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper() + " (Temporal :" + oPaciente.NumeroAdic + ") ";
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
              

                if (oPaciente.IdEstado == 3)
                    logoRenaper.Visible = true;
                else
                    logoRenaper.Visible = false;

                /// verifica si el paciente tiene protocolos en los ultimos 7 dias
                /// //Caro Performance: no buscar si tiene ingresos por defecto.
                /// 
                lblAlertaProtocolo.Visible = false;
                if ((Request["Operacion"].ToString() == "Alta") && (oC.VerificaIngresoAnterior))
                {
                    string tieneingreso = oPaciente.GetProtocolosReciente(7, Request["idServicio"].ToString());

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
                }                             


                string[] edad = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento, DateTime.Now).Split(';');
                lblEdad.Text = edad[0].ToString();
                lblUnidadEdad.Text = " " + edad[1].ToUpper();
                //    ddlEfector.SelectedValue = Origen[1].ToString();

                //txtEdad.Value = edadpropuesta.Split(;);
                lblFechaNacimiento.Text = "F.Nac.:" + oPaciente.FechaNacimiento.ToShortDateString();
                txtTelefono.Text = oPaciente.InformacionContacto;


            }

        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
            pnlMuestra.Visible = false;

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
            oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
            lblServicio.Text = oServicio.Nombre;


            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,  prefijo + ' - ' + nombre   as nombre FROM LAB_SectorServicio  with (nolock) WHERE (baja = 0) order by nombre";

            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre", connReady);
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));
            if (oC.IdSectorDefecto > 0)
            {
                ddlSectorServicio.SelectedValue = oC.IdSectorDefecto.ToString();
              //  ddlSectorServicio.Visible = false;
            }

            /////////////////////////////////////////////CODIGO DE BARRAS//////////////////////////////////////////////////////////////////////
            tab3Titulo.Visible = false;
            pnlEtiquetas.Visible = false;

            ConfiguracionCodigoBarra oConfiguracion = new ConfiguracionCodigoBarra();
            oConfiguracion = (ConfiguracionCodigoBarra)oConfiguracion.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oServicio,"IdEfector", oUser.IdEfector); //Multiefector: filtrar por efector
            if (oConfiguracion == null)
            {
                //    lblImprimeCodigoBarras.Visible = false;
                //    chkAreaCodigoBarra.Items.Clear();
                ddlImpresoraEtiqueta.Visible = false;
            }
            else
            {
                if (oConfiguracion.Habilitado)
                {
                    m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora with (nolock) where idEfector=" + oUser.IdEfector.IdEfector.ToString() + " order by nombre";  //MultiEfector
                                                                                                                                                             //oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
                    oUtil.CargarCombo(ddlImpresoraEtiqueta, m_ssql, "nombre", "nombre", connReady);
                    ddlImpresoraEtiqueta.Items.Insert(0, new ListItem("Seleccione impresora", "0"));
                  


                    if ((Request["Operacion"].ToString() == "Alta") ||
                  (Request["Operacion"].ToString() == "AltaTurno") ||
                  (Request["Operacion"].ToString() == "AltaDerivacion") ||
                  (Request["Operacion"].ToString() == "AltaDerivacionMultiEfector") ||
                  (Request["Operacion"].ToString() == "AltaPeticion") ||
                  (Request["Operacion"].ToString() == "AltaFFEE") ||
                  (Request["Operacion"].ToString() == "AltaDerivacionMultiEfectorLote") )
                  
                    {
                        pnlImpresoraAlta.Visible = true;
                        pnlEtiquetas.Visible = false;
                    }
                    else
                    {
                        pnlImpresoraAlta.Visible = false;
                        oUtil.CargarCombo(ddlImpresora2, m_ssql, "nombre", "nombre", connReady);
                        ddlImpresora2.Items.Insert(0, new ListItem("Seleccione impresora", "0"));


                        tab3Titulo.Visible = true;
                        pnlEtiquetas.Visible = true;

                        //lblImprimeCodigoBarras.Visible = true;
                        ///cargar de areas con codigo de barras           ==> solo areas que estan en el protocolo.
                                         
                        m_ssql = @"select idArea, nombre from Lab_Area  A with (nolock)
                            WHERE imprimeCodigoBarra=1 and idTipoServicio=" + oServicio.IdTipoServicio.ToString() +
                            @"  and baja=0
                            and exists (select 1 from lab_detalleprotocolo dp with (nolock)
                                        inner  join lab_item P with (nolock) on dp.idsubitem = p.iditem
                                        where dp.idProtocolo = " + Request["idProtocolo"].ToString() + @"
                                        and dp.trajoMuestra = 'Si'
                                        and p.idarea = A.idArea) order by nombre";
                        oUtil.CargarCheckBox(chkAreaCodigoBarra, m_ssql, "idArea", "nombre");
                        chkAreaCodigoBarra.Items.Insert(0, new ListItem("General", "-1"));
                    }
                }
                else
                {
                    //lblImprimeCodigoBarras.Enabled = false;
                    chkAreaCodigoBarra.Items.Clear();
                    ddlImpresoraEtiqueta.Items.Insert(0, new ListItem("Sin impresora", "0"));
                    ddlImpresora2.Items.Insert(0, new ListItem("Sin impresora", "0"));
                    ddlImpresoraEtiqueta.Enabled = false;
                    btnReimprimirCodigoBarras.Enabled = false;
                    lblMensajeImpresion.Text = "No se ha habilitado la impresion de etiquetas";
                    lblMensajeImpresion.UpdateAfterCallBack = true;
                }
                 
                
            }



           

            //////////////////////////////////////////////////

            //////////////////////////FIN DE CODIGO DE BARRAS///////////////////////////////////////////////////////////////////////////////////                    

            ///Carga de combos de Origen
            m_ssql = " SELECT  idOrigen, nombre FROM LAB_Origen with (nolock) WHERE (baja = 0)";
            if (oC.OrigenHabilitado != "")
                m_ssql = m_ssql + " and idOrigen in (" + oC.OrigenHabilitado + ")";



            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre", connReady);
            ddlOrigen.Items.Insert(0, new ListItem("", "0"));


            ///Carga de combos de Prioridad
            m_ssql = " SELECT idPrioridad, nombre FROM LAB_Prioridad with (nolock) WHERE (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre", connReady);


            if ((Session["idServicio"].ToString() == "3") || (Session["idServicio"].ToString() == "6"))//microbiologia o forense microbiologia
            {
                ddlPrioridad.SelectedValue = "1";
                //ddlPrioridad.Enabled = false;

                txtNumeroOrigen2.Visible = true;
                lblNroHisopado.Visible = true;
                ////////////Carga de combos de Muestras

                pnlMuestra.Visible = true;
                m_ssql = @"SELECT idMuestra, nombre + ' - ' + codigo as nombre FROM LAB_Muestra  M with (nolock)
                            where (tipo=0 or tipo=1)";
                if (Request["Operacion"].ToString() != "Modifica")  //alta
                    m_ssql += " and baja=0 and exists (select 1 from lab_muestraEfector E  (nolock) where M.idMuestra = E.idmuestra and E.idefector = " + oUser.IdEfector.IdEfector.ToString()+")"; //Multiefector";


                m_ssql += " order by nombre ";
                oUtil.CargarCombo(ddlMuestra, m_ssql, "idMuestra", "nombre", connReady);
                ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
                

            }

            ////////////Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector  E  (nolock) ";

            //if (Request["Operacion"].ToString() != "Modifica")  //alta
                m_ssql += " where exists (select 1 from LAB_EfectorRelacionado R (nolock) where E.idEfector = R.idEfectorRel and R.idefector = " + oUser.IdEfector.IdEfector.ToString() +
                    ")  or (E.idEfector="+oC.IdEfector.IdEfector.ToString()+ @" )";
           
           m_ssql += " order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();
            

            //////////////Carga de combos de Medicos Solicitantes
            //if (Request["Operacion"].ToString()=="Carga")///muestra solo los activos
            //    m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional WHERE activo=1 ORDER BY apellido, nombre ";
            //else
            //    m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional   ORDER BY apellido, nombre ";
            //oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
            if (Request["Operacion"].ToString() != "Modifica")
                ddlEspecialista.Items.Insert(0, new ListItem("--Ingrese un médico--", "-1"));





            m_ssql = "SELECT idCaracter, nombre   FROM LAB_Caracter with (nolock) ";
            oUtil.CargarCombo(ddlCaracter, m_ssql, "idCaracter", "nombre", connReady);
            ddlCaracter.Items.Insert(0, new ListItem("--Seleccione Caracteristica--", "0"));

            if (ddlCaracter.Items.Count > 1)
            {
                lblCaracterSisa.Visible = true;
                ddlCaracter.Visible = true;
            }
            ////////////////////////////Carga de combos de ObraSocial//////////////////////////////////////////
            //m_ssql = "SELECT idObraSocial,  nombre AS nombre FROM Sys_ObraSocial order by idObraSocial ";          
            //oUtil.CargarCombo(ddlObraSocial, m_ssql, "idObraSocial", "nombre");
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();
            //////////////////////////////////////////////////////////////////////////////////////////////////

            //Multiefector: antes
            //m_ssql = "SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre " +
            //        " FROM Lab_item I  " +
            //        " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
            //        " where A.baja=0 and I.baja=0 and  I.disponible=1 and A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";

            m_ssql = "SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre " +
                  " FROM Lab_item I with (nolock) " +
                  " inner join lab_itemEfector IE with (nolock) on IE.idItem= I.iditem and ie.idefector=" + oC.IdEfector.IdEfector.ToString()+
                  " INNER JOIN Lab_area A with (nolock) ON A.idArea= I.idArea " +
                  " where A.baja=0 and I.baja=0 and  IE.disponible=1 and A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);


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

            chkNotificar.Checked = true;
            chkNotificar.Enabled = oC.HabilitaNoPublicacion;

          
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
                        case "chkRecordarPractica":
                            {
                                if (s_control[1].ToString() == "False")
                                    chkRecordarPractica.Checked = false;
                                else
                                    chkRecordarPractica.Checked = true;
                            }
                            break;
                        case "prácticas":
                            TxtDatosCargados.Value = s_control[1].ToString(); break;
                        //case "ddlImpresora":
                        //    ddlImpresora.SelectedValue = s_control[1].ToString(); break;

                        case "ddlImpresoraEtiqueta":
                            ddlImpresoraEtiqueta.SelectedValue = s_control[1].ToString(); break;
                    }
                }
            }
            else
            {
                //if (Session["Impresora"]!=null) ddlImpresora.SelectedValue=Session["Impresora"].ToString();
                if (Session["Etiquetadora"] != null) ddlImpresoraEtiqueta.SelectedValue = Session["Etiquetadora"].ToString();
            }

        }

        private void CargarItems()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = "SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, " +
                           " IE.disponible " +
                            " FROM Lab_item I with (nolock) " +
                            " inner join lab_itemEfector IE  with (nolock) on I.idItem= IE.idItem and Ie.idefector=" + oC.IdEfector.IdEfector.ToString() + //MultiEfector 
                            " INNER JOIN Lab_area A with (nolock) ON A.idArea= I.idArea " +
                            " where A.baja=0 and I.baja=0 and IE.disponible=1 "+  
                            " and  A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";
            SqlConnection strconn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

            //gvLista.DataSource = ds.Tables["T"];
            //gvLista.DataBind();


            ddlItem.Items.Insert(0, new ListItem("", "0"));

            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                sTareas += "#" + ds.Tables["T"].Rows[i][1].ToString() + "#" + ds.Tables["T"].Rows[i][2].ToString() + "#" + ds.Tables["T"].Rows[i][4].ToString() + "@";
            }
            txtTareas.Value = sTareas;

            //Carga de combo de rutinas
            m_ssql = "SELECT idRutina, nombre FROM Lab_Rutina (nolock) where baja=0 and IdEfector= "+ oUser.IdEfector.IdEfector.ToString() +" and idTipoServicio= " + Session["idServicio"].ToString() + " order by nombre ";//Ver MultiEfector 
            oUtil.CargarCombo(ddlRutina, m_ssql, "idRutina", "nombre");
            ddlRutina.Items.Insert(0, new ListItem("Seleccione una rutina", "0"));

            ddlItem.UpdateAfterCallBack = true;
            ddlRutina.UpdateAfterCallBack = true;

            m_ssql = null;
            oUtil = null;
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { ///Verifica si se trata de un alta o modificacion de protocolo               
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                if (Request["Operacion"].ToString() == "Modifica") oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));


                bool guardo = Guardar(oRegistro);

                if (guardo)
                {    ////if (oRegistro.IdTipoServicio==6) Response.Redirect ("ProtocoloMensaje.aspx")
                    //// se muestra un mensaje con el numero de protocolo y te da opcion de adjuntos y de consentieminto si es forense.


                    if ((Request["Operacion"].ToString() == "Alta") ||
                    (Request["Operacion"].ToString() == "AltaTurno") ||
                    (Request["Operacion"].ToString() == "AltaDerivacion") ||
                    (Request["Operacion"].ToString() == "AltaDerivacionMultiEfector") ||
                    (Request["Operacion"].ToString() == "AltaDerivacionMultiEfectorLote") ||
                    (Request["Operacion"].ToString() == "AltaPeticion"))
                    {


                        /// actualiza al paciente con la ultima obra social guardada: solo en las altas
                        oRegistro.IdPaciente.IdObraSocial = oRegistro.IdObraSocial.IdObraSocial;
                        oRegistro.IdPaciente.FechaUltimaActualizacion = DateTime.Now;
                        oRegistro.IdPaciente.Save();

                        if (ddlImpresoraEtiqueta.SelectedValue != "0")
                        //   oRegistro.ImprimirCodigoBarras(ddlImpresoraEtiqueta.SelectedItem.Text, int.Parse(Session["idUsuario"].ToString()));
                        {
                            ///Imprimir codigo de barras.
                            string s_AreasCodigosBarras = oRegistro.getListaAreasCodigoBarras();
                            if (s_AreasCodigosBarras != "")
                            {

                                ImprimirCodigoBarrasAreas(oRegistro, s_AreasCodigosBarras, ddlImpresoraEtiqueta.SelectedItem.Text);
                            }
                            //string s_Items = oRegistro.getListaEtiquetaDeterminacion();
                            //if (s_Items != "")
                            //{                                
                            //    ImprimirCodigoBarrasDeterminacion(oRegistro,  s_Items, ddlImpresoraEtiqueta.SelectedItem.Text);
                            //}
                        }



                        /////////////////


                        ///Imprimir codigo de barras.
                        //string s_AreasCodigosBarras = getListaAreasCodigoBarras();
                        //if (s_AreasCodigosBarras != "")
                        //    ImprimirCodigoBarras(oRegistro, getListaAreasCodigoBarras());


                        //////Imprimir Comprobante para el paciente
                        //if (chkImprimir.Checked) Imprimir(oRegistro);

                        if (oC.NotificarSISA)
                        {
                            if (oRegistro.VerificaObligatoriedadFIS()) //sospechoso o detectar: cambiar segun parametrizacion de tabla
                                                                       //                                                                //   if ((oRegistro.IdCaracter == 1) || (oRegistro.IdCaracter == 8)) //sospechoso o detectar
                            {
                                Response.Redirect("ProcesaSISA.aspx?idP=" + oRegistro.IdProtocolo.ToString());
                            }
                        }

                    }

                    //////////////////////////

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
                                case "Default": Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
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
                        //if (Request["Operacion"].ToString() == "AltaDerivacion") 
                        if (Request["Operacion"].ToString() == "AltaDerivacionMultiEfector")
                        {
                            ActualizarEstadoDerivacion(oRegistro);

                            Response.Redirect("DerivacionMultiEfector.aspx?idEfectorSolicitante=" + Request["idEfectorSolicitante"].ToString() + "&idServicio=" + Session["idServicio"].ToString());
                        }
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
                                            Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=2", false); break;
                                        case "0":
                                            Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false); break;
                                    }
                                }
                                else
                                    Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                                //   }
                            }
                        }
                    }
                }
          else
            {
                    Response.Redirect("ProtocoloMensaje.aspx?error=1");
             
            }
            }
           

        }

        private void ActualizarEstadoDerivacion(Protocolo oRegistro)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"update LAB_Derivacion
set estado=3---recibido
,idProtocoloDerivacion="+oRegistro.IdProtocolo.ToString()+@"
from LAB_Derivacion D
inner join LAB_DetalleProtocolo Det on Det.idDetalleProtocolo= d.idDetalleProtocolo
inner join LAB_Protocolo P on P.idProtocolo= Det.idProtocolo
where P.numero=" + Request["numeroProtocolo"].ToString() + @" and idsubItem in (" + Request["analisis"].ToString().Replace("|",",") + ")";
           
            SqlCommand cmd = new SqlCommand(query, conn);

            ///grabar auditoria de 

           int idRealizado = Convert.ToInt32(cmd.ExecuteScalar());

            //Se indica en el protocolo de Origen que fue recibido en el destino
            Business.Data.Laboratorio.Protocolo oPOrigen = new Business.Data.Laboratorio.Protocolo();
            oPOrigen = (Business.Data.Laboratorio.Protocolo)oPOrigen.Get(typeof(Business.Data.Laboratorio.Protocolo), "Numero", int.Parse(Request["numeroProtocolo"].ToString()));
            if (oPOrigen != null)
            {
                oPOrigen.GrabarAuditoriaDetalleProtocolo("Recepcion Derivacion", oUser.IdUsuario, "", oRegistro.Numero.ToString());
            }


        }

        private string getListaAreasCodigoBarras()
        {
            string lista = "";
            for (int i = 0; i < chkAreaCodigoBarra.Items.Count; i++)
            {
                if (chkAreaCodigoBarra.Items[i].Selected)
                {
                    if (lista == "")
                        lista = chkAreaCodigoBarra.Items[i].Value;
                    else
                        lista += "," + chkAreaCodigoBarra.Items[i].Value;
                }
            }
            return lista;
        }


       

        private void ImprimirCodigoBarrasAreas(Protocolo oProt, string s_listaAreas, string impresora)
        {
           
            string[] tabla = s_listaAreas.Split(',');


            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                string s_area = tabla[i].ToUpper();

                if (s_area == "-1")
                    oProt.GrabarAuditoriaProtocolo("Imprime Etiqueta General", oUser.IdUsuario);
                else
                {
                    Area oArea = new Area();
                    oArea = (Area)oArea.Get(typeof(Area), int.Parse(s_area));
                    string s_narea = oArea.Nombre;
                    if (oArea.Nombre.Length > 25)
                        s_narea = oArea.Nombre.Substring(0, 25);

                    oProt.GrabarAuditoriaProtocolo("Imprime Etiqueta " + s_narea, oUser.IdUsuario);
                }


                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                string query = @" INSERT INTO LAB_ProtocoloEtiqueta
                    (idProtocolo ,idEfector  ,[idArea]  ,[idItem]      ,[impresora],fechaRegistro )
     VALUES   ( " + oProt.IdProtocolo.ToString() + "," + oUser.IdEfector.IdEfector.ToString()+"," + s_area + ",0,'" + impresora + "' , getdate()    )";
                SqlCommand cmd = new SqlCommand(query, conn);


                int idres = Convert.ToInt32(cmd.ExecuteScalar());
            }
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            //ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();
            //oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oProt.IdTipoServicio);

            //string sFuenteBarCode = oConBarra.Fuente;
            //bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
            //bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
            //bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
            //bool imprimeProtocoloNumeroOrigen = oConBarra.ProtocoloNumeroOrigen;
            //bool imprimePacienteNumeroDocumento = oConBarra.PacienteNumeroDocumento;
            //bool imprimePacienteApellido = oConBarra.PacienteApellido;
            //bool imprimePacienteSexo = oConBarra.PacienteSexo;
            //bool imprimePacienteEdad = oConBarra.PacienteEdad;
            //bool adicionalGeneral = false;
            //if (s_listaAreas.Substring(0, 1) == "0") adicionalGeneral = true;

            //DataTable Dt = new DataTable();
            //Dt = (DataTable)oProt.GetDataSetCodigoBarras("Protocolo", s_listaAreas, oProt.IdTipoServicio.IdTipoServicio, adicionalGeneral);
            //foreach (DataRow item in Dt.Rows)
            //{
            //    ///Desde acá impresion de archivos
            //    string reg_numero = item[2].ToString();
            //    string reg_area = item[3].ToString();
            //    string reg_Fecha = item[4].ToString().Substring(0, 10);
            //    string reg_Origen = item[5].ToString();
            //    string reg_Sector = item[6].ToString();
            //    string reg_NumeroOrigen = item[7].ToString();
            //    string reg_NumeroDocumento = oProt.IdPaciente.getNumeroImprimir();



            //    string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);

            //    string reg_apellido = "";
            //    if (chkCodificaPaciente.Checked)
            //    {
            //        reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
            //    }
            //    else
            //    {
            //        if (reg_codificaHIV == "FALSE")
            //            reg_apellido = oProt.IdPaciente.Apellido + " " + oProt.IdPaciente.Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
            //        else
            //            reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
            //    }
            //    //reg_apellido = item[12].ToString().ToUpper();
            //    string reg_sexo = item[10].ToString();
            //    string reg_edad = item[11].ToString();
            //    //tabla.Rows.Add(reg);
            //    //tabla.AcceptChanges();


            //    if (!imprimeProtocoloFecha) reg_Fecha = "          ";
            //    if (!imprimeProtocoloOrigen) reg_Origen = "          ";
            //    if (!imprimeProtocoloSector) reg_Sector = "   ";
            //    if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
            //    if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
            //    if (!imprimePacienteApellido) reg_apellido = "";
            //    if (!imprimePacienteSexo) reg_sexo = " ";
            //    if (!imprimePacienteEdad) reg_edad = "   ";
            //    //ParameterDiscreteValue fuenteCodigoBarras = new ParameterDiscreteValue(); fuenteCodigoBarras.Value = oConBarra.Fuente;


            //    Business.Etiqueta ticket = new Business.Etiqueta();
            //    ticket.TipoEtiqueta = oC.TipoEtiqueta;
            //    if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);


            //    ticket.AddHeaderLine(reg_apellido.ToUpper());
            //    ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha);
            //    if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector)) ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);
            //    if (reg_area != "") ticket.AddSubHeaderLineNegrita(reg_area);
            //    //ticket.AddSubHeaderLine(reg_area);

            //    ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
            //    ticket.AddFooterLine(reg_numero); // + "  " + reg_NumeroOrigen);

            //    Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;
            //    ticket.PrintTicket(ddlImpresoraEtiqueta.SelectedValue, oConBarra.Fuente);
            /////fin de impresion de archivos
            // }

        }
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
            if (oTurno != null)
            {
                oTurno.IdProtocolo = oRegistro.IdProtocolo;
                oTurno.Save();
            }
        }

        private void ActualizarPeticion(string p, Protocolo oRegistro)
        {
            Peticion oPeticion = new Peticion();
            oPeticion = (Peticion)oPeticion.Get(typeof(Peticion), int.Parse(p));
            if (oPeticion != null)
            {
                oPeticion.IdProtocolo = oRegistro.IdProtocolo;
                oPeticion.Save();
            }
        }
        private bool Guardar(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            bool guardo = false;
            if (IsTokenValid())
            {
                Utility oUtil = new Utility();
                TEST++;
                //Actualiza los datos de los objetos : alta o modificacion .
                Efector oEfector = new Efector();
                //Usuario oUser = new Usuario();
                //oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                Paciente oPaciente = new Paciente();
                ObraSocial oObra = new ObraSocial();
                Origen oOrigen = new Origen();
                Prioridad oPri = new Prioridad();
                DateTime fecha = DateTime.Parse(txtFecha.Value);

                //Configuracion oC = new Configuracion();
                //oC = (Configuracion)oC.Get(typeof(Configuracion),   "IdEfector", oUser.IdEfector.IdEfector);

                oRegistro.IdEfector = oC.IdEfector;
                SectorServicio oSector = new SectorServicio();
                oSector = (SectorServicio)oSector.Get(typeof(SectorServicio), int.Parse(ddlSectorServicio.SelectedValue));
                oRegistro.IdSector = oSector;

                TipoServicio oServicio = new TipoServicio();
                oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
                oRegistro.IdTipoServicio = oServicio;

                oRegistro.Notificarresultado = chkNotificar.Checked;

                if (Request["Operacion"].ToString() != "Modifica")
                {

                    oRegistro.Numero = 0;
                     //oRegistro.NumeroDiario = oRegistro.GenerarNumeroDiario(fecha.ToString("yyyyMMdd"));
                    //oRegistro.PrefijoSector = oSector.Prefijo.Trim();
                    //oRegistro.NumeroSector = oRegistro.GenerarNumeroGrupo(oSector);
                    //oRegistro.NumeroTipoServicio = oRegistro.GenerarNumeroTipoServicio(oServicio);
                }
                bool grabarincidenciafis = false;
                bool grabarincidenciafuc = false;
                oRegistro.FechaInicioSintomas = DateTime.Parse("01/01/1900");
                oRegistro.FechaUltimoContacto = DateTime.Parse("01/01/1900");
                oRegistro.IdCaracter =int.Parse( ddlCaracter.SelectedValue);
                if (VerificaObligatoriedadFIS())
                //if ((ddlCaracter.SelectedValue == "1") || (ddlCaracter.SelectedValue == "8") || (ddlCaracter.SelectedValue == "9")) //SOSPECHOSO O DETECTAR O REINFECCION
                {
                    if (chkSinFIS.Checked) grabarincidenciafis = true;
                    else oRegistro.FechaInicioSintomas = DateTime.Parse(txtFechaFIS.Value);
                                        
                        
                      
                }
                if (ddlCaracter.SelectedValue == "4")  //CONTACTO
                {
                    if (chkSinFUC.Checked) grabarincidenciafuc = true;
                    else oRegistro.FechaUltimoContacto = DateTime.Parse(txtFechaFUC.Value);
                    //GRABAR fUC
                }

                // si tiene datos graba igual
                DateTime fechaFis; DateTime fechaFuc;

                if (DateTime.TryParse(txtFechaFIS.Value, out fechaFis))
                
                    oRegistro.FechaInicioSintomas = fechaFis;
                

                if (DateTime.TryParse(txtFechaFUC.Value, out fechaFuc))
                
                    oRegistro.FechaUltimoContacto = fechaFuc;

                oRegistro.Fecha = DateTime.Parse(txtFecha.Value);
                oRegistro.FechaOrden = DateTime.Parse(txtFechaOrden.Value);
                oRegistro.FechaTomaMuestra= DateTime.Parse(txtFechaTomaMuestra.Value);
                oRegistro.FechaRetiro = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);

                oRegistro.IdEfectorSolicitante = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                oRegistro.IdEspecialistaSolicitante = 0;



                oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(lblIdPaciente.Text));
                if (Request["Operacion"].ToString() == "Modifica")
                {
                    if (oRegistro.IdPaciente != oPaciente)
                        oRegistro.GrabarAuditoriaProtocolo("Cambia de Paciente", int.Parse(Session["idUsuario"].ToString()));
                }
                if (txtTelefono.Text!="")
                { oPaciente.InformacionContacto = txtTelefono.Text;
                    oPaciente.FechaUltimaActualizacion = DateTime.Now;
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
                    oRegistro.IdUsuarioRegistro = oUser; // (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;
                    oRegistro.IpCarga = "";
                    oRegistro.Impres = ddlImpresoraEtiqueta.SelectedItem.Text;
                }

                ///separar medico
                ///  ddlEspecialista.SelectedValue
                string matricula = "";
                string apellidoynombre = "";
                if (ddlEspecialista.SelectedValue == "0")
                {
                    matricula = "0";
                    apellidoynombre = "No identificado";
                }
                else
                {
                    string[] fila = ddlEspecialista.SelectedValue.Split('#');
                    matricula = fila[0].ToString();

                    apellidoynombre = fila[1].ToString();
                    string[] fila2 = apellidoynombre.Split('-');
                    apellidoynombre= fila2[0].ToString().TrimEnd();
                }
                oRegistro.Especialista = apellidoynombre; // ddlEspecialista.SelectedItem.Text;
                oRegistro.MatriculaEspecialista = matricula; // ddlEspecialista.SelectedValue;

                if (Request["idFicha"] != null)
                {
                    string idFicha = Request["idFicha"].ToString();

                    Business.Data.Laboratorio.Ficha oRegistroFFEE = new Business.Data.Laboratorio.Ficha();
                    oRegistroFFEE = (Business.Data.Laboratorio.Ficha)oRegistroFFEE.Get(typeof(Business.Data.Laboratorio.Ficha), "IdFicha", idFicha);
                    if (oRegistroFFEE != null)
                        if (oRegistroFFEE.IdCasoSnvs!="")
                            if (oUtil.EsNumerico(oRegistroFFEE.IdCasoSnvs))
                                oRegistro.IdCasoSISA =int.Parse(oRegistroFFEE.IdCasoSnvs);

                }

                oRegistro.Save();
                oRegistro.ActualizarNumeroDesdeID();

               




                if (Request["Operacion"].ToString() != "Modifica") { if (Request["Operacion"].ToString() != "AltaPeticion") { if (Session["idUrgencia"] != null) { if (Session["idUrgencia"].ToString() == "0") AlmacenarSesion(oC); } } }
                    if (Request["Operacion"].ToString() == "AltaDerivacion") AlmacenarSesion(oC);

                //   if (Request["idSolicitudScreening"] != null) ActualizarSolicitudScreening(Request["idSolicitudScreening"].ToString(),oRegistro);
                GuardarDiagnosticos(oRegistro);
                GuardarDetalle(oRegistro);
                //GuardarDiagnosticos(oRegistro);
                this.IncidenciaEdit1.GuardarProtocoloIncidencia(oRegistro);
                if (grabarincidenciafis)
                    oRegistro.GeneraIncidenciaAutomatica(46, int.Parse(Session["idUsuario"].ToString()));
                if (grabarincidenciafuc)
                    oRegistro.GeneraIncidenciaAutomatica(47, int.Parse(Session["idUsuario"].ToString()));
            /*    if ((!grabarincidenciafis) && (!grabarincidenciafuc))
                    oRegistro.BorrarIncidenciasFISyFUC(int.Parse(Session["idUsuario"].ToString()));
                    */
               

                //oRegistro.VerificarExisteNumeroAsignado();
                
                oRegistro.GrabarAuditoriaProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));

                ///cambair estado


                /*  if (Request["Operacion"].ToString() == "Alta")
                      oRegistro.Estado = 0;
                  else // modificacion 
                  {
                      if (oRegistro.Estado != 0) // si esta terminado se fija si sigue terminado o no 
                      {
                          if (oRegistro.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString())))
                              oRegistro.Estado = 2;  //validado total (cerrado);                    
                          else
                              oRegistro.Estado = 1; // el estado 1 se pone cuando se carga resultados.
                      }

                }
              */

                 if (oRegistro.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString())))
                  {
                      oRegistro.Estado = 2;  //validado total (cerrado);
                                              //oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                      if (!oRegistro.Notificarresultado) // no se notifica a sips
                          oRegistro.Estado = 3; ///Accceso Restringido;
                  }
                  else
                  {
                      if (oRegistro.EnProceso())
                      {
                          oRegistro.Estado = 1;//en proceso
                                                // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                      }
                      else
                          oRegistro.Estado = 0;

                  }

                /////optimizacion Caro: para recorrer detalleprotocolo una sola ve y no dos con ValidadoTotal y EnProceso
                //int estadoProt = oRegistro.GetEstadoProtocolo(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString()));
                //if ((estadoProt == 2) && (!oRegistro.Notificarresultado))
                //    oRegistro.Estado = 3; ///Accceso Restringido;
                //else
                //    oRegistro.Estado = estadoProt;


                oRegistro.Save();
                guardo = true;

                /*    if (Request["Operacion"].ToString() == "Alta")*/

                //if ((Request["Operacion"].ToString() == "Alta") ||
                //    (Request["Operacion"].ToString() == "AltaTurno") ||
                //    (Request["Operacion"].ToString() == "AltaDerivacion") ||
                //    (Request["Operacion"].ToString() == "AltaDerivacionMultiEfector") ||
                //    (Request["Operacion"].ToString() == "AltaPeticion"))

                //    oRegistro.ImprimirCodigoBarras(ddlImpresoraEtiqueta.SelectedItem.Text, int.Parse(Session["idUsuario"].ToString()));
                /*if (Request["Operacion"].ToString() == "AltaTurno")
                    oRegistro.ImprimirCodigoBarras(ddlImpresoraEtiqueta.SelectedItem.Text, int.Parse(Session["idUsuario"].ToString()));*/

                /////////////////inicio try /////////////////
                // De desactiva conexion con heller ya que cambio el sistema
                //try
                //{
                //    if ((oRegistro.IdEfector.IdEfector != 221) && (oRegistro.IdEfectorSolicitante.IdEfector == 221) && (oRegistro.NumeroOrigen != ""))
                //    {
                //        string s_urlWFC = ConfigurationManager.AppSettings["Efector221Confirmar"].ToString();
                //        s_urlWFC = s_urlWFC + "?idpet=" + oRegistro.NumeroOrigen + "&fecha=" + DateTime.Now.ToString("yyyy-MM-dd");
                //        Response.Redirect(s_urlWFC,false);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    string exception = "";
                //    //while (ex != null)
                //    //{
                //        exception = ex.Message + "<br>";

                //    //}
                //}
                /////////////////fin try /////////////////
            }
            else
            { //doble submit
                guardo = false;
            }

            return guardo;
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
            
                
            string s_valores = "chkRecordarPractica:" + chkRecordarPractica.Checked;
            
            //if (chkRecordarConfiguracion.Checked)
            //{
            //    s_valores += "@chkImprimir:" + chkImprimir.Checked;
            //    s_valores += "@chkAreaCodigoBarra:" + getListaAreasCodigoBarras();
            //}
            //s_valores += "@chkRecordarConfiguracion:" + chkRecordarConfiguracion.Checked;
            //s_valores += "@ddlImpresora:" + ddlImpresora.SelectedValue;
            s_valores += "@ddlImpresoraEtiqueta:" + ddlImpresoraEtiqueta.SelectedValue;

            //Session["Impresora"] = ddlImpresora.SelectedValue;
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
            string embarazada = ""; string accion = "Graba";
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
                   /* if (oC.NomencladorDiagnostico == 0)
                    {
                        Cie10 oD = new Cie10(oDetalle.IdDiagnostico);
                        oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", oD.Nombre);
                    }
                    else
                    {
                        DiagnosticoP oD = new DiagnosticoP(oDetalle.IdDiagnostico);

                        oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", oD.Nombre);
                    }*/

                    oDetalle.Delete();
                    accion = "Cambia";
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
                    string s_diag = lstDiagnosticosFinal.Items[i].Text;
                    oDetalle.IdProtocolo.GrabarAuditoriaDetalleProtocolo( accion, int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", s_diag);
                    embarazada = oDetalle.EsEmbarazada();
                  

                 
                    if (listaDx=="")
                    listaDx = lstDiagnosticosFinal.Items[i].Value;
                    else
                        listaDx +=";"+ lstDiagnosticosFinal.Items[i].Value;
                }
            }
           
            if (embarazada == "E")
            {
                oRegistro.Embarazada = "S";
                oRegistro.Save();
            }
            Session["Dx"] = listaDx;

        }

        

        private void GuardarDetalle(Business.Data.Laboratorio.Protocolo oRegistro)
        {         
            int dias_espera = 0;
            string[] tabla = TxtDatos.Value.Split('@');
            ISession m_session = NHibernateHttpModule.CurrentSession;

            string recordar_practicas = "";

            for (int i = 0; i < tabla.Length - 1; i++)
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
                        {
                            oDetalle.TrajoMuestra = "No";
                            oDetalle.FechaResultado = DateTime.Now;
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Sin Muestra", oUser.IdUsuario);
                        }
                        else
                        {
                            oDetalle.TrajoMuestra = "Si";
                            oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                        }


                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                        oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                        oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                        oDetalle.Informable = oItem.GetInformableEfector(oUser.IdEfector);


                        GuardarDetallePractica(oDetalle);

                        // GuardarDerivacion(oDetalle);
                        oDetalle.GuardarDerivacion(oUser);


                        if (i == 0)///correccion de que se graba con la session el tipo de servicio, se vuelve a controlar que sea el de la deterinacion
                        {
                            oRegistro.IdTipoServicio = oItem.IdArea.IdTipoServicio;
                            oRegistro.Save();
                        }



                        //si ya esta actualizo si trajo muestra o no

                        ////foreach (DetalleProtocolo oDetalle in listadetalle)
                        ////{
                        //    if (trajomuestra == "true")
                        //        oDetalle.TrajoMuestra = "No";
                        //    else
                        //        oDetalle.TrajoMuestra = "Si";

                        //    oDetalle.Save();
                        //    oDetalle.GrabarAuditoriaDetalleProtocolo("Sin Muestra", oUser.IdUsuario);
                        ////}


                    }  // fin if count()
                    else
                    { /// si cambió la marca de sin muetsra
                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {
                            if (trajomuestra == "true") /// es no trajo
                            {
                                if (oDetalle.TrajoMuestra == "Si") // estaba grabado Si
                                {
                                    oDetalle.TrajoMuestra = "No";
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Sin Muestra", oUser.IdUsuario);
                                    oDetalle.Save();
                                }
                            }
                            else  // si  trajo muestra
                            {
                                if (oDetalle.TrajoMuestra == "No")  /// si estaba grabado que no cambia
                                {
                                    oDetalle.TrajoMuestra = "Si";
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Con Muestra", oUser.IdUsuario);
                                    oDetalle.Save();
                                }
                            }

                          
                            
                        }
                    }
                }
            }

            if (Request["Operacion"].ToString() != "Modifica")
            {
                if (chkRecordarPractica.Checked)
                    Session["ProtocoloLaboratorio"] += "@prácticas:" + recordar_practicas;
            }



            if (Request["Operacion"].ToString() != "Alta")
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
                                if (codigo == oDetalle.IdItem.Codigo)   noesta = false;

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

         //   Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        
            //if (oCon.TipoCalculoDiasRetiro == 0)

            if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
                if (oC.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(dias_espera));
                else   // calcula con los días predeterminados de espera
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oC.DiasRetiro));
            else
                oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);




            oRegistro.Save();


        }

        //private void GuardarDerivacion(DetalleProtocolo oDetalle)
        //{
        //    if (oDetalle.IdItem.esDerivado(oC.IdEfector))
        //    {
        //        Business.Data.Laboratorio.Derivacion oRegistro = new Business.Data.Laboratorio.Derivacion();
        //        oRegistro.IdDetalleProtocolo = oDetalle;
        //        oRegistro.Estado = 0;
        //        oRegistro.Observacion = txtObservacion.Text;
        //        oRegistro.IdUsuarioRegistro = oUser.IdUsuario;// int.Parse(Session["idUsuario"].ToString());
        //        oRegistro.FechaRegistro = DateTime.Now;
        //        oRegistro.FechaResultado = DateTime.Parse("01/01/1900");

        //        oRegistro.IdEfectorDerivacion = oDetalle.IdItem.GetIDEfectorDerivacion(oC.IdEfector);  // se graba el efector configurado en ese momento.

        //        oRegistro.Save();

        //        // graba el resultado en ResultadCar   "Derivado: " + oItem.GetEfectorDerivacion(oCon.IdEfector);
        //        //oDetalle.ResultadoCar = "Pendiente de Derivacion";//"se podria poner a que efector....         
        //        //oDetalle.Save();
        //        oDetalle.GrabarAuditoriaDetalleProtocolo("Graba Derivado", oUser.IdUsuario);
        //    }

        //}

       // private void GuardarDetalle2(Business.Data.Laboratorio.Protocolo oRegistro)
       // {                           
       //     ///Eliminar los detalles para volverlos a crear            
       //     ISession m_session = NHibernateHttpModule.CurrentSession;
       //     ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
       //     crit.Add(Expression.Eq("IdProtocolo", oRegistro));
       //     IList detalle = crit.List();
                         
       //         foreach (DetalleProtocolo oDetalle in detalle)
       //         {
       //             oDetalle.Delete();
       //         }                
            

       
       //     int dias_espera = 0;
       //     string[] tabla = TxtDatos.Value.Split('@');

       //     for (int i = 0; i < tabla.Length - 1; i++)
       //     {
       //         string[] fila = tabla[i].Split('#');


       //         string codigo = fila[1].ToString();
       //         if (codigo!="")
       //         {
       //             DetalleProtocolo oDetalle = new DetalleProtocolo();
       //             Item oItem = new Item();
       //             oDetalle.IdProtocolo = oRegistro;
       //             oDetalle.IdEfector = oRegistro.IdEfector;

       //             string trajomuestra = fila[3].ToString();

       //             oDetalle.IdItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo);

       //             if (dias_espera < oDetalle.IdItem.Duracion) dias_espera = oDetalle.IdItem.Duracion;

       //             /*CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
       //             if (a.Checked)
       //                 oDetalle.TrajoMuestra = "Si";
       //             else*/

       //             if (trajomuestra=="true")
       //                 oDetalle.TrajoMuestra = "No";
       //             else
       //                 oDetalle.TrajoMuestra = "Si";


       //             oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaValida = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaControl = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
       //             oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
       //             GuardarDetallePractica(oDetalle);
       //         }
       //     }
         

       //  //   Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
       ////     DateTime fechaentrega;
       //     //if (oCon.TipoCalculoDiasRetiro == 0)

       //     if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
       //         if (oC.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
       //             oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(dias_espera)  );
       //         else   // calcula con los días predeterminados de espera
       //             oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oC.DiasRetiro)  );                                                       
       //     else
       //         oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);  
            
            
 
            
       //     oRegistro.Save();
          
          
       // }

       

        private void GuardarDetallePractica(DetalleProtocolo oDet)
        {


            if (oDet.VerificarSiEsDerivable(oUser.IdEfector)) //oDet.IdItem.IdEfector.IdEfector != oDet.IdItem.IdEfectorDerivacion.IdEfector) //Si es un item derivable no busca hijos y guarda directamente.
            {
                 oDet.IdSubItem = oDet.IdItem;
                            oDet.Save();
                oDet.GuardarValorReferencia();
                 
            }
            else
            {
               ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                crit.Add(Expression.Eq("IdItemPractica", oDet.IdItem));
                crit.Add(Expression.Eq("IdEfector",oUser.IdEfector));

                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    int i = 1;
                    foreach (PracticaDeterminacion oSubitem in detalle)
                    {
                        if (oSubitem.IdItemDeterminacion != 0)
                        { 
                            Item oSItem = new Item();
                            oSItem=(Item)oSItem.Get(typeof(Item), oSubitem.IdItemDeterminacion);
                            if (i == 1)
                            {
                                if (oDet.TrajoMuestra == "Si")
                                {
                                    oDet.IdSubItem = oSItem;
                                    oDet.FechaResultado = DateTime.Parse("01/01/1900");
                                }
                                else
                                {
                                    oDet.IdSubItem = oDet.IdItem;  //no trajo muestra le pone el mismo id
                                    oDet.FechaResultado = DateTime.Now;
                                }

                                oDet.Save();
                                oDet.GuardarSinInsumo();
                                oDet.GuardarValorReferencia();
                                if (oDet.TrajoMuestra == "No")
                                    oDet.GrabarAuditoriaDetalleProtocolo("Sin Muestra", oUser.IdUsuario);
                            }
                            else
                            {
                                if (oDet.TrajoMuestra == "Si")  // sino trajo muestra no guardo el detalle
                                {
                                    DetalleProtocolo oDetalle = new DetalleProtocolo();
                                    oDetalle.IdProtocolo = oDet.IdProtocolo;
                                    oDetalle.IdEfector = oDet.IdEfector;
                                    oDetalle.IdItem = oDet.IdItem;
                                    oDetalle.IdSubItem = oSItem;
                                    oDetalle.TrajoMuestra = oDet.TrajoMuestra;
                                    oDetalle.Informable = oSItem.GetInformableEfector(oUser.IdEfector);


                                    oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                                    oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");


                                    oDetalle.Save();
                                    oDetalle.GuardarSinInsumo();
                                    oDetalle.GuardarValorReferencia();
                                    //GuardarValorReferencia(oDetalle);
                                }
                                 
                            }
                            i = i + 1;                        
                        }//fin if
                    }//fin foreach
                }         
                else
                {
                    oDet.IdSubItem = oDet.IdItem;
                    oDet.Informable = oDet.IdSubItem.GetInformableEfector(oUser.IdEfector);

               
                    oDet.Save();
                     oDet.GuardarSinInsumo();
                    oDet.GuardarValorReferencia();
                    if (oDet.TrajoMuestra == "No")
                        oDet.GrabarAuditoriaDetalleProtocolo("Sin Muestra", oUser.IdUsuario);
                    //GuardarValorReferencia(oDet);
                }//fin   if (detalle.Count > 0)  
            }

         
            
        }

        //private void GuardarValorReferencia(DetalleProtocolo oDetalle)
        //{
           
          
       

        //string s_unidadMedida = ""; //string s_metodo = "";
        //    //Item oItem = new Item();
        //    //oItem = (Item)oItem.Get(typeof(Item), int.Parse(m_idItem));
        //    if (oDetalle.IdSubItem.IdUnidadMedida > 0)
        //    {
        //        UnidadMedida oUnidad = new UnidadMedida();
        //        oUnidad = (UnidadMedida)oUnidad.Get(typeof(UnidadMedida), oDetalle.IdSubItem.IdUnidadMedida);
        //        if (oUnidad != null) s_unidadMedida = oUnidad.Nombre;
               
        //    }

        //    ///Calculo de valor de referencia al momento de generar el registro
        //     int pres=oDetalle.IdSubItem.GetPresentacionEfector(oDetalle.IdEfector);


        //    string m_metodo = "";
        //    string m_valorReferencia = "";
        //    string valorRef = oDetalle.CalcularValoresReferencia(pres);
        //    if (valorRef != "")
        //    {
        //        string[] arr = valorRef.Split(("|").ToCharArray());
        //        switch (arr.Length)
        //        {
        //            case 1: m_valorReferencia = arr[0].ToString(); break;
        //            case 2:
        //                {
        //                    m_valorReferencia = arr[0].ToString();
        //                    m_metodo = arr[1].ToString();
        //                }
        //                break;
        //        }
               
        //    }
        //    oDetalle.UnidadMedida = s_unidadMedida;
        //    oDetalle.Metodo = m_metodo;
        //        oDetalle.ValorReferencia = m_valorReferencia;
        //        oDetalle.Save();
        //    //Fin calculo de valor de refrencia y metodo
        //}

        //private bool VerificarSiEsDerivable(DetalleProtocolo oDet)
        //{
        //    bool ok=false;
        //    /// buscar idefectorderivacion desde lab_itemefector
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria critItemEfector = m_session.CreateCriteria(typeof(ItemEfector));
        //    critItemEfector.Add(Expression.Eq("IdItem", oDet.IdItem));
        //    critItemEfector.Add(Expression.Eq("IdEfector", oUser.IdEfector));
        //    IList detalle1 = critItemEfector.List();
        //    if (detalle1.Count > 0)
        //    {
        //        foreach (ItemEfector oitemEfector in detalle1)
        //        {
        //            if (oDet.IdEfector.IdEfector != oitemEfector.IdEfectorDerivacion.IdEfector)
        //            {
        //                ok = true; break;
        //            }
        //        }
        //    }
        //    else
        //        ok = false;

        //    return ok;

        //}

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
            ///////Con la selección del item se muestra el codigo
            if (ddlItem.SelectedValue != "0")
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
                if (oItem!=null)
                txtCodigo.Text = oItem.Codigo;
                
            }
            else
                txtCodigo.Text = "";

            txtCodigo.UpdateAfterCallBack = true;
            

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
            lblMensajeDiagnostico.Visible = false; 
            if (lstDiagnosticos.SelectedValue != "")
            {
                bool agrego = true;
                /////Verifica si ya fue agregado el diagnostico
                for (int i = 0; i < lstDiagnosticosFinal.Items.Count; i++)
                {

                    if (lstDiagnosticosFinal.Items[i].Value == lstDiagnosticos.SelectedItem.Value)
                    {
                        agrego = false; break;
                    }
                }


                if (agrego)
                {
                    lstDiagnosticosFinal.Items.Add(lstDiagnosticos.SelectedItem);
                    lstDiagnosticosFinal.UpdateAfterCallBack = true;
                }
                else
                {
                    lblMensajeDiagnostico.Visible = true;
                    lblMensajeDiagnostico.Text = "Alerta: Diagnostico ya ingresado para el paciente.";
                  
                }
            }
            lblMensajeDiagnostico.UpdateAfterCallBack = true;
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
                        case "Default": Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
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
                        if (Request["Operacion"].ToString() == "AltaDerivacionMultiEfector")                            
                            Response.Redirect("DerivacionMultiEfector.aspx?idEfectorSolicitante=" + Request["idEfectorSolicitante"].ToString() + "&idServicio=" + Session["idServicio"].ToString());
                        else
                        {
                            if (Request["Operacion"].ToString() == "AltaPeticion")
                                Response.Redirect("../PeticionElectronica/PeticionList.aspx", false);
                            else
                            {
                                if (Session["idUrgencia"].ToString() != "0")
                                    Response.Redirect("Default2.aspx?idServicio=1&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                                else
                                    Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                            }
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
            Rutina oRutina = new Rutina();
            oRutina = (Rutina)oRutina.Get(typeof(Rutina), int.Parse(ddlRutina.SelectedValue));

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleRutina));
            crit.Add(Expression.Eq("IdRutina", oRutina));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                string codigos="";
                foreach (DetalleRutina oDetalle in detalle)
                {

                    if (codigos == "")
                        codigos = oDetalle.IdItem.Codigo;
                    else
                        codigos += ";" + oDetalle.IdItem.Codigo;

                  

                    //ddlRutina.SelectedValue = "0";
                    //ddlRutina.UpdateAfterCallBack = true;


                }
                txtCodigosRutina.Text = codigos;
                txtCodigosRutina.UpdateAfterCallBack = true;

            }

        }

       

        protected void ddlServicio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CargarItems();
            TxtDatos.Value = "";

        }

        protected void ddlRutina_SelectedIndexChanged(object sender, EventArgs e)
        {
             if (ddlRutina.SelectedValue != "0")
             AgregarRutina();
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
         //   Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
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
                    string m_strSQL = @"select id, codigo + ' -' + nombre from sys_cie10 with (nolock) where CODIGO like '%" + txtCodigoDiagnostico.Text.Trim() + "%'";

                    DataSet Ds = new DataSet();
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    adapter.Fill(Ds);
                    lstDiagnosticos.Items.Clear();
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        ListItem oDia = new ListItem();
                        oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                        oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                        lstDiagnosticos.Items.Add(oDia);

 
                    }


                     
                       

                     
                    
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

     


    private void BuscarCodigoDiagnostico_ant()
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
        {
            lstDiagnosticos.Items.Clear();
            if (txtNombreDiagnostico.Text != "")
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                if (oC.NomencladorDiagnostico == 0)
                {
                    //ICriteria crit = m_session.CreateCriteria(typeof(Cie10));
                    //crit.Add(Expression.Sql(" Nombre like '%" + txtNombreDiagnostico.Text + "%' order by Nombre"));

                    //IList items = crit.List();

                    //foreach (Cie10 oDiagnostico in items)
                    //{
                    //    ListItem oDia = new ListItem();
                    //    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    //    oDia.Value = oDiagnostico.Id.ToString();
                    //    lstDiagnosticos.Items.Add(oDia);
                    //}


                    string m_strSQL = @"select id, codigo + ' -' + nombre from sys_cie10 (nolock) where Nombre like '%" + txtNombreDiagnostico.Text.Trim() + "%' order by Nombre";

                    DataSet Ds = new DataSet();
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    adapter.Fill(Ds);
                    lstDiagnosticos.Items.Clear();
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        ListItem oDia = new ListItem();
                        oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                        oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
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



        private void BuscarNombreDiagnostico_ant()
        {
            lstDiagnosticos.Items.Clear();
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
                    crit1.Add(Expression.Eq("Baja", false));
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
            //saco restriccion de forma temporal
            //if (Request["Operacion"].ToString()!="Modifica")
            //    if (!VerificarFechaPacienteMuestra())
            //    {
            //        TxtDatos.Value = "";
            //        args.IsValid = false;
            //        this.cvValidacionInput.ErrorMessage = "No es posible ingresar para la misma fecha, muestra y paciente un nuevo protocolo.";
            //        return;
            //    }

            if (!VerificarAnalisisContenidos() )
            {  TxtDatos.Value = "";
                args.IsValid = false;
             
                return;
            }
            else
            {

              

            ///

            if ((TxtDatos.Value == "") || (TxtDatos.Value == "1###on@"))
                {

                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe completar al menos un análisis";
                    return;
                }
                else args.IsValid = true;


                //validacion Diagnostico
                if (oC.DiagObligatorio)
                {if (lstDiagnosticosFinal.Items.Count == 0)
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "Debe ingresar al menos un diagnóstico presuntivo del paciente";
                        return;
                    }
                }

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


                if ((ddlSectorServicio.SelectedValue == "0"))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar sector";
                    return;
                }

                if ((ddlOrigen.SelectedValue == "0"))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar Origen";
                    return;
                }
                
                if ((ddlPrioridad.SelectedValue == "0"))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar Prioridad";
                    return;
                }
                
                if ((ddlMuestra.SelectedValue == "0") && (pnlMuestra.Visible))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar Tipo de Muestra";
                    return;
                }
                /// Valida que debe seleccionar un caracter si es un caso notificable a SISA



                if ((VerificaRequiereCaracter(sDatos)) && (ddlCaracter.SelectedValue == "0"))
                //if ((sDatos.Contains(oC.CodigoCovid) && (ddlCaracter.SelectedValue=="0")))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe seleccionar el caracter del protocolo";
                    return;
                }
                // fin valida
                // validacion si es sospechoso o detctar ingresar fecha de inicio de sintomas

                if (VerificaObligatoriedadFIS()) 
                {
                    if ((txtFechaFIS.Value == "") && (chkSinFIS.Checked==false))
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "Debe ingresar fecha de inicio de síntomas";
                        return;
                    }
                }
                // validacion si es contacto  ingresar fecha de ultimo contacto
                if ((ddlCaracter.SelectedValue == "4") && (txtFechaFUC.Value=="") && (chkSinFUC.Checked==false))
                {
                    
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "Debe ingresar fecha de último contacto";
                        return;
                     
                }
                if ((ddlEspecialista.SelectedValue=="-1") && (oC.MedicoObligatorio))
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la mátricula del médico solicitante";
                    return;
                }
                if (ddlOrigen.SelectedValue == "0")
                {
                    TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar el origen";
                    return;
                }
                if ((oC.IdSectorDefecto== 0) && (ddlSectorServicio.SelectedValue == "0"))
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "Debe ingresar el Servicio";
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


                /// control de fecha inicio de sintomas
                /// 
                
                if (txtFechaFIS.Value != "")
               
                {
                    if (DateTime.Parse(txtFechaFIS.Value) > DateTime.Now)
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "La FIS no puede ser superior a la fecha actual";
                        return;
                    }
                    else
                    {
                        if (DateTime.Parse(txtFechaTomaMuestra.Value) < DateTime.Parse(txtFechaFIS.Value))
                        {
                            TxtDatos.Value = "";
                            args.IsValid = false;
                            this.cvValidacionInput.ErrorMessage = "La FIS no puede ser despues de la fecha de toma de muestra";
                            return;
                        }
                        else
                            args.IsValid = true;
                    }
                }//fin control


                /// control de fecha inicio de sintomas
                /// 
                

                if (txtFechaFUC.Value != "")

                {
                    if (DateTime.Parse(txtFechaFUC.Value) > DateTime.Now)
                    {
                        TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "La FUC no puede ser superior a la fecha actual";
                        return;
                    }
                    else
                    {
                        if (DateTime.Parse(txtFechaTomaMuestra.Value) < DateTime.Parse(txtFechaFUC.Value))
                        {
                            TxtDatos.Value = "";
                            args.IsValid = false;
                            this.cvValidacionInput.ErrorMessage = "La FUC no puede ser despues de la fecha de toma de muestra";
                            return;
                        }
                        else
                            args.IsValid = true;
                    }
                }//fin control

            }
        }

        private bool VerificaRequiereCaracter(string sDatos)
        {
           
            bool devolver = false ;
            string[] tabla = sDatos.Split(';');
            

            for (int i = 0; i < tabla.Length  ; i++)
            {
                string[] fila = tabla[i].Split('#');
                string codigo = fila[0].ToString();
                if (codigo != "")
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo, "Baja", false);

                    if (oItem.RequiereCaracter)
                    {
                        devolver = true;
                        break;
                    }
                }

            }

            return devolver;
        }

        private bool VerificaObligatoriedadFIS()
        {
            bool obligafis = false;

            string[] arrfis = oC.FISCaracter.Split((",").ToCharArray());
            foreach (string item in arrfis)
            {
                if (item == ddlCaracter.SelectedValue)
                { obligafis = true; break; }
            }
            return obligafis;

        }

        //private bool VerificarFechaPacienteMuestra()
        //{
        //    Paciente oPaciente = new Paciente();
        //    oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(HFIdPaciente.Value));

        //    string tieneingreso = oPaciente.GetFechaProtocolosReciente(Request["idServicio"].ToString(),ddlMuestra.SelectedValue);
        //    if (tieneingreso==txtFecha.Value) return false;
        //    else return true;
        //}

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
                    if (oItem.VerificaMuestrasAsociadas(int.Parse(ddlMuestra.SelectedValue)))
                    { 

                    i_idItemPractica = oItem.IdItem;
                        for (int j = 0; j < tabla.Length - 1; j++)

                        {
                            string[] fila2 = tabla[j].Split('#');
                            string codigo2 = fila2[1].ToString();
                            if ((codigo2 != "") && (codigo != codigo2))
                            {
                                Item oItem2 = new Item();
                                oItem2 = (Item)oItem2.Get(typeof(Item), "Codigo", codigo2, "Baja", false);

                                //MultiEfector: filtro por efector


                                ISession m_session = NHibernateHttpModule.CurrentSession;
                                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                                crit.Add(Expression.Eq("IdItemPractica", oItem));
                                crit.Add(Expression.Eq("IdItemDeterminacion", oItem2.IdItem));
                                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                                PracticaDeterminacion oGrupo = (PracticaDeterminacion)crit.UniqueResult();



                                if (oGrupo != null)
                                {

                                    this.cvValidacionInput.ErrorMessage = "Ha cargado análisis contenidos en otros. Verifique los códigos " + codigo + " y " + codigo2 + "!";
                                    devolver = false; break;

                                }

                            }
                        
                    }////for           
                    }
                    else
                    {
                        this.cvValidacionInput.ErrorMessage = "Ha ingresado tipo de muestra que no corresponde con el codigo " + codigo + ". Verifique configuracion.";
                        devolver = false; break;

                    }

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
            string m_ssql = "SELECT  distinct PD.idItemDeterminacion, I.codigo" +
                            " FROM         LAB_PracticaDeterminacion AS PD " +
                            " INNER JOIN   LAB_Item AS I ON PD.idItemPractica = I.idItem " +
                            " WHERE     I.codigo IN (" + listaCodigo + ") AND (I.baja = 0)" +
                            " and PD.idEfector= "+oUser.IdEfector.IdEfector.ToString()+" ORDER BY PD.idItemDeterminacion ";

            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter da = new SqlDataAdapter(m_ssql,  conn);
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



        //protected void lnkReimprimirComprobante_Click(object sender, EventArgs e)
        //{
        //    Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
        //    oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));            

        //    ////Imprimir Comprobante para el paciente
        //    Imprimir(oRegistro);



        //}

        protected void lnkReimprimirCodigoBarras_Click(object sender, EventArgs e)
        {
            lblMensajeImpresion.Text = "Se ha enviado la impresión.";
            if (ddlImpresora2.SelectedIndex>0)
            {
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                ///Imprimir codigo de barras.
                string s_AreasCodigosBarras = getListaAreasCodigoBarras();
                if (s_AreasCodigosBarras != "")
                {

                    ImprimirCodigoBarrasAreas(oRegistro, s_AreasCodigosBarras, ddlImpresora2.SelectedItem.Text);
                }
                else
                    lblMensajeImpresion.Text = "Debe seleccionar al menos un area para imprimir";
            }
            else
                lblMensajeImpresion.Text = "Debe seleccionar una impresora";
            lblMensajeImpresion.UpdateAfterCallBack = true;
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

            string m_ssql = @"SELECT top 20 ID, Codigo + ' - ' + Nombre as nombre, count (*)  cantidad
FROM Sys_CIE10 c (nolock)
inner join LAB_ProtocoloDiagnostico p (nolock) on c.id = p.idDiagnostico
where p.idEfector=" + oUser.IdEfector.IdEfector.ToString() + @"
group by id, codigo, nombre
ORDER BY cantidad desc";


           
            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_ssql, conn);
            adapter.Fill(Ds);
            lstDiagnosticos.Items.Clear();
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {

                ListItem oDia = new ListItem();
                oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                lstDiagnosticos.Items.Add(oDia);


            }

            //if (oC.NomencladorDiagnostico==1) //prppio
            //    m_ssql = "SELECT idDiagnostico as ID, Codigo + ' - ' + Nombre as nombre FROM lab_Diagnostico WHERE (idDiagnostico IN (SELECT DISTINCT idDiagnostico FROM LAB_ProtocoloDiagnostico)) ORDER BY Nombre";
            //oUtil.CargarListBox(lstDiagnosticos, m_ssql, "id", "nombre");
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
            
                if (matricula == "0")
                {
                    ddlEspecialista.Items.Clear();
                    ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));
                }
                else
                {
                    string s_urlWFC = oC.UrlMatriculacion;
                    string s_url = s_urlWFC + "numeroMatricula=" + matricula; // + "&codigoProfesion in (1,23)";

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
                                espe = pro[i].apellido + " " + pro[i].nombre + " - " + pro[i].profesiones[0].titulo;
                                //documento = pro[i].documento.ToString();
                                ddlEspecialista.Items.Insert(0, new ListItem(espe, matricula+ '#' + espe));
                            }
                            if (pro.Count > 1)
                                if (Request["idProtocolo"] == null)
                                { ddlEspecialista.Items.Insert(0, new ListItem("--Seleccione--", "0")); }

                            lblErrorMedico.Visible = false;

                        }
                        else
                        { //error no encontrado}


                            lblErrorMedico.Text = "Médico no encontrado!!";
                            lblErrorMedico.Visible = true;
                            ddlEspecialista.Items.Clear();
                            ddlEspecialista.Items.Insert(0, new ListItem("Médico no encontrado", "-1"));

                        } // procount
                    }//s!=0
                } // else mtraicula
            } // try
             catch (Exception ex)

          //  catch (WebException ex)
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
            public string titulo { get; set; }
        }

        public class Matricula

        {
            public string  matriculaNumero { get; set; }
           
        }

       
            public class ProfesionalMatriculado
        {
        //    public int documento { get; set; }
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
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

            string caracter = "";
            string idevento = "";
            string nombreevento = "";
            string idclasificacionmanual = "";
            string nombreclasificacionmanual = "";
            string idgrupoevento = "";
            string nombregrupoevento = "";

            string m_strSQL = @"select C.* from LAB_ConfiguracionSISA C 
            where exists (select iditem from lab_detalleprotocolo d where D.idsubItem = C.iditem and idprotocolo =" + oRegistro.IdProtocolo.ToString() + ")" +
@" and C.idCaracter =" + oRegistro.IdCaracter.ToString();

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            DataTable dt = Ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                caracter = dt.Rows[i][1].ToString();
                idevento = dt.Rows[i][2].ToString();
                nombreevento = dt.Rows[i][3].ToString();
                idclasificacionmanual = dt.Rows[i][4].ToString();
                nombreclasificacionmanual = dt.Rows[i][5].ToString();
                idgrupoevento = dt.Rows[i][6].ToString();
                nombregrupoevento = dt.Rows[i][7].ToString();

                break;
             
            }
                try
                {

                    
                    string URL = oC.UrlServicioSISA;
                    string s_idestablecimiento = oC.CodigoEstablecimientoSISA; // "14580562167000"
                    string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
                    string[] a = usersisa.Split(':');
                    string s_user = a[0].ToString();
                    string s_userpass = a[1].ToString();

                    string s_sexo = lblSexo.Text;
                    string fn = oRegistro.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");
                   
                    string fnpapel =txtFechaOrden.Value.Replace("/", "-");


                    string numerodocumento = oRegistro.IdPaciente.NumeroDocumento.ToString();

                    string error = "";
                 

                    evento newevento = new evento
                    {
                        idTipodoc = "1",
                        nrodoc = numerodocumento,
                        sexo = s_sexo,
                        fechaNacimiento = fn,  //"05-06-1989",
                        idGrupoEvento = idgrupoevento,
                        idEvento = idevento, // "77",
                        idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                        fechaPapel = fnpapel, // "10-12-2019",
                        idClasificacionManualCaso = idclasificacionmanual, // "22"
                    };

                    AltaCaso caso = new AltaCaso
                    {

                        //usuario = "mapaterniti",
                        //clave = "Alicia1938",
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
                          
                            if (oRegistro != null)
                            {
                                oRegistro.IdCasoSISA = int.Parse(s_idcaso);
                                oRegistro.Save();
                                if (respuesta_d.resultado == "OK")
                                    oRegistro.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                                else // ERROR_DATOS
                                    oRegistro.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                            if (oRegistro.Estado == 2) // si ya tiene resultados informados
                            {
                                NotificaLaboratorio(oRegistro); }
                                Avanzar(0); 
                            }
                        }
                        else
                        { 
                        error = respuesta_d.resultado + ": " + respuesta_d.description;
                       
                        lblErrorMedico.Text = error;
                        lblErrorMedico.Visible = true;
                    }
                    }

                   

                }
            catch (WebException ex)
            {
                   string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                //lblError.Text = "Hubo algun problema al conectar al servicio SISA: " + e.InnerException.InnerException.Message.ToString() + ". Intente de nuevo o haga clic en Salir";
                //lblError.Visible = true;
                //btnSalir.Visible = true;
            }


        }

        private void NotificaLaboratorio(Protocolo oRegistro)
        {


            string m_strSQL = @"SELECT  distinct idDetalleProtocolo,  S.idMuestra as IdMuestraSISA,	  S.idTipoMuestra as idTipoMuestraSISA, s.idPrueba as idPruebaSISA, s.idTipoPrueba as idTipoPruebaSISA,  
                ds.idResultadoSISA,S.idEvento
                  FROM    LAB_DetalleProtocolo d
                   inner join LAB_ConfiguracionSISA S on S.idCaracter=" + oRegistro.IdCaracter.ToString() + @" and s.idItem= d.idSubItem
                   inner join LAB_ConfiguracionSISADetalle DS on DS.idItem=d.idSubItem  and resultadocar= ds.resultado
                    where d.idusuariovalida>0 and d.idProtocolo= " + oRegistro.IdProtocolo.ToString();


        
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            string idDetalleProtocolo;
            string idMuestra;
            string idTipoMuestra;
            string idPrueba;
            string idTipoPrueba;
            string idResultadoSISA;
            string idEvento;

            DataTable dt = Ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                idDetalleProtocolo = dt.Rows[i][0].ToString();
                idMuestra = dt.Rows[i][1].ToString();
                idTipoMuestra = dt.Rows[i][2].ToString();
                idPrueba = dt.Rows[i][3].ToString();
                idTipoPrueba = dt.Rows[i][4].ToString();
                idResultadoSISA = dt.Rows[i][5].ToString();
                idEvento = dt.Rows[i][6].ToString();


                if ((oRegistro.IdCasoSISA > 0)  )
                    GenerarMuestraSISA(oRegistro, idMuestra, idTipoMuestra, idDetalleProtocolo,idPrueba, idTipoPrueba, idResultadoSISA, idEvento);

                //if (oDetalle.IdeventomuestraSISA > 0)
                //    GenerarResultadoSISA(oDetalle, );

                break;
            }
        }

        //private void GenerarMuestraSISA(Protocolo protocolo)
        private void GenerarMuestraSISA(Protocolo protocolo, string idMuestraSISA, string idtipoMuestraSISA, string idDetalleProtocolo, string idPruebaSISA, string idTipoPruebaSISA, string idResultadoSISA, string idEventoSISA)

        {
            
            string URL = oC.URLMuestraSISA;


            bool generacaso = true;
            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if (idestablecimientotoma == "")
                //pongo por defecto laboratorio central
                idestablecimientotoma = "107093";


            ResultadoxNro.EventoMuestra newmuestra = new ResultadoxNro.EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso = protocolo.IdCasoSISA, // 2061287,
                idMuestra = int.Parse(idMuestraSISA),
                idtipoMuestra = int.Parse(idtipoMuestraSISA),
                muestra = true
            };
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            string DATA = jsonSerializer.Serialize(newmuestra);


            byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

            HttpWebRequest request;
            request = WebRequest.Create(URL) as HttpWebRequest;
            request.Timeout = 10 * 1000;
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.ContentType = "application/json";
            request.Headers.Add("app_key", "b0fd61c3a08917cfd20491b24af6049e");
            request.Headers.Add("app_id", "22891c8f");

            try
            {

                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();


                if (body != "")
                {
                    ResultadoxNro.EventoMuestraResultado respuesta_d = jsonSerializer.Deserialize<ResultadoxNro.EventoMuestraResultado>(body);

                    if (respuesta_d.id != 1)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));

                        if (oDetalle != null)
                        {
                            oDetalle.IdeventomuestraSISA = respuesta_d.id;
                            oDetalle.Save();
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + respuesta_d.id.ToString(), oDetalle.IdUsuarioValida);
                                                       
                            if (oDetalle.IdeventomuestraSISA > 0)
                                GenerarResultadoSISA(oDetalle, idPruebaSISA, idTipoPruebaSISA, idResultadoSISA, idEventoSISA);
                            
                        } //for each
                    } //respuesta_o


                }// body

            }


            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

        }

        //private void GenerarResultadoSISA(DetalleProtocolo oDetalle)


private void GenerarResultadoSISA(DetalleProtocolo oDetalle, string idPruebaSISA, string idTipoPruebaSISA, string idResultadoSISA, string idEventoSISA)

        {
            
            int ideventomuestra = oDetalle.IdeventomuestraSISA;
          
            string URL = oC.URLResultadoSISA;


            try
            {
                int id_resultado_a_informar = int.Parse(idResultadoSISA); // 0;
                int idevento = int.Parse(idEventoSISA); //  307; // sospechoso

 

                if (id_resultado_a_informar != 0)
                {
                    string femision = oDetalle.FechaValida.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    string frecepcion = oDetalle.IdProtocolo.Fecha.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                    resultado newresultado = new resultado
                    { // resultado de dni: 31935346
                        derivada = false,
                        fechaEmisionResultado = femision, //"2020-09-14", //
                        fechaRecepcion = frecepcion, // "2020-09-13" 
                        idDerivacion = null, //1125675,//
                        idEstablecimiento = 107093,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                        idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                        idEventoMuestra = ideventomuestra,  // 2131682, // sale del excel
                        idPrueba = int.Parse(idPruebaSISA), //1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = int.Parse(idTipoPruebaSISA), // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
                        noApta = true,
                        valor = ""
                    };

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    string DATA = jsonSerializer.Serialize(newresultado);

                    byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                    HttpWebRequest request;
                    request = WebRequest.Create(URL) as HttpWebRequest;
                    request.Timeout = 10 * 1000;
                    request.Method = "POST";
                    request.ContentLength = data.Length;
                    request.ContentType = "application/json";
                    request.Headers.Add("app_key", "8482d41353ecd747c271f2ec869345e4");
                    request.Headers.Add("app_id", "0e4fcbbf");


                    Stream postStream = request.GetRequestStream();
                    postStream.Write(data, 0, data.Length);

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();
                    if (body != "")
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oDetalle.IdUsuarioValida);
                        
                    }

                }


            }
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                 

            }


        }

    }

}

