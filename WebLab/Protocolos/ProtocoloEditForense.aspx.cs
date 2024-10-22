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
using System.Web;
using Business.Data.GenMarcadores;

namespace WebLab.Protocolos
{
    public partial class ProtocoloEditForense : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {

            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;

            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

     

        private void CargarGrilla()
        {
            ////Metodo que carga la grilla de Protocolos
            string m_strSQL = " Select distinct P.idProtocolo, " +
                             " P.numero as numero," +
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
                VerificaPermisos("Casos Forense");
                //PreventingDoubleSubmit(btnGuardar);
                if (Session["idUsuario"] != null)
                {
                    //lblIdPaciente.Text = Request["idPaciente"].ToString();


                    if (Request["idServicio"] != null) Session["idServicio"] = Request["idServicio"].ToString();
                    if (Request["idUrgencia"] != null) Session["idUrgencia"] = Request["idUrgencia"].ToString();
                    CargarListas();

                  
                   

                    if (Request["Operacion"].ToString() == "Modifica")
                    {



                        lblTitulo.Visible = true;
                        pnlNumero.Visible = true;
                        lnkAnterior.Visible = true;
                        lnkSiguiente.Visible = true;
                        //pnlNuevo.Visible = false;

                        MuestraDatos();

                        if (Request["Desde"].ToString() == "Control")
                        {
                            pnlLista.Visible = true;
                            CargarGrilla();
                            gvLista.Visible = true;
                            pnlNavegacion.Visible = true;
                            lnkAnterior.Visible = true;
                            lnkSiguiente.Visible = true;
                            //divPaciente.Visible = false;

                        }
                        else
                        {
                            pnlLista.Visible = false;
                            gvLista.Visible = false;
                            pnlNavegacion.Visible = false;
                            lnkAnterior.Visible = false;
                            lnkSiguiente.Visible = false;

                        }
                    }
                    else

                    { //alta desde un caso
                        if (Session["idCaso"] != null)
                        {
                            lnkAnterior.Visible = false;
                            lnkSiguiente.Visible = false;
                            lblServicio0.Visible = true;
                            lblServicio0.Text = "Caso Nro. " + Session["idCaso"].ToString();
                        }
                        //lnkReimprimirComprobante.Visible = false;
                        //lnkReimprimirCodigoBarras.Visible = false;

                        hplActualizarPaciente.Visible = false;
                        hplModificarPaciente.Visible = false;


                        lblTitulo.Visible = false;
                        txtFecha.Value = DateTime.Now.ToShortDateString();

                        ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString(); // el efector del sil por defecto.
                        ///verificar si la configuracion determina fecha actual por defecto o sin valor
                        if (oC.ValorDefectoFechaOrden == 0) txtFechaOrden.Value = "";
                        else txtFechaOrden.Value = DateTime.Now.ToShortDateString();

                        if (oC.ValorDefectoFechaTomaMuestra == 0) txtFechaTomaMuestra.Value = "";
                        else txtFechaTomaMuestra.Value = DateTime.Now.ToShortDateString();
                        ///

                        if (Request["idPaciente"].ToString() != "-1")
                        {
                            //filiacion
                            MostrarPaciente();
                            //divPaciente.Visible = false;
                        }
                        else
                        {// no paciente  : forense

                            //divPaciente.Visible = true;
                            pnlPaciente.Visible = false;
                            //lblEdad.Text = "-1";
                            //lblUnidadEdad.Text = "-1";
                            lblIdPaciente.Text = "-1";
                        }



                        if (Session["idCaso"] != null)
                        {
                            if (Session["idCaso"].ToString() != "0")
                            {
                                Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                                oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), "IdCasoFiliacion", int.Parse(Session["idCaso"].ToString()));
                                try
                                {
                                    SectorServicio oServicio = new SectorServicio();
                                    oServicio = (SectorServicio)oCaso.Get(typeof(SectorServicio), "Nombre", oCaso.Solicitante);

                                    ddlSectorServicio.SelectedValue = oServicio.IdSectorServicio.ToString();
                                }
                                catch { }
                                if (oCaso.IdTipoCaso == 2) // forense
                                {
                                    lblCadenaCustodia.Visible = true;
                                    divfoto.Visible = false;
                                }
                                else //filiacion
                                {
                                    lblCadenaCustodia.Visible = false;

                                }
                            }
                            else
                                lblServicio0.Visible = false;
                        }
                        else
                        {
                            Session["idCaso"] = "0";
                            lblServicio0.Visible = false;
                        }

                        btnCancelar.Text = "Cancelar";
                        btnCancelar.Width = Unit.Pixel(80);

                        if (Request["idServicio"].ToString() == "6")
                        {
                            pnlNavegacion.Visible = false;
                            pnlTitulo.Attributes.Add("class", "panel panel-success");
                            //tableTitulo.Attributes.Add("class", "tituloCeldaVerde");
                            btnGuardar.Attributes.Add("class", "btn btn-success");
                            btnCancelar.Attributes.Add("class", "btn btn-success");
                            CargarDeterminacionesForense();
                        }
                        else
                        {



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
            //if (s_diagnostico != "0")
            //{
            //    ///Traer datos de diagnsotico        
            //    Cie10 oC = new Cie10();
            //    oC = (Cie10)oC.Get(typeof(Cie10), int.Parse(s_diagnostico));
            //    ListItem oDia = new ListItem();
            //    oDia.Text = oC.Nombre;
            //    oDia.Value = oC.Id.ToString();
            //    lstDiagnosticosFinal.Items.Add(oDia);
            //}
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
                        {
                            btnGuardar.Visible = false;
                            //    btnGuardarImprimir.Visible = false; 
                        }
                        break;
                }
            }
            else Response.Redirect(Page.ResolveUrl("~/FinSesion.aspx"), false);
        }

        private void CargarDeterminacionesForense()
        {


            string pivot = "";
            string sDatos = "";


            sDatos = "F001#Si";



            pivot = "FORENSE";



            TxtDatosCargados.Value = sDatos;



        }


        private void MuestraDatos()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            //Actualiza los datos de los objetos : alta o modificacion .
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));

            gvListaCaso.DataSource = oRegistro.getListaCasos();
            gvListaCaso.DataBind();

            if (gvListaCaso.Rows.Count == 0)
                pnlMarcadores.Visible = true;
            else
                pnlMarcadores.Visible = false;

            if (oRegistro.IdTipoServicio.IdTipoServicio == 6)
            {
                //gdMarcadores.DataSource = oRegistro.getMarcadores();
                //gdMarcadores.DataBind();

                ICriteria crit1 = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
                crit1.Add(Expression.Eq("IdProtocolo", oRegistro));
                //       crit1.Add(Expression.Eq("IdCasoFiliacion", oCaso));
                IList listcaso = crit1.List();

                foreach (CasoFiliacionProtocolo oCasof in listcaso)
                {
                    Session["idCaso"] = oCasof.IdCasoFiliacion.IdCasoFiliacion.ToString();

                    lblServicio0.Text = "Caso Nro. " + Session["idCaso"].ToString();
                    ddlParentesco.SelectedValue = oCasof.IdTipoParentesco.ToString();
                    txtObservacionParentesco.Text = oCasof.ObservacionParentesco;


                }

                pnlTitulo.Attributes.Add("class", "panel panel-success");
                //tableTitulo.Attributes.Add("class", "tituloCeldaVerde");
                btnGuardar.CssClass = "btn btn-success";
                btnCancelar.CssClass = "btn btn-success";
            }

            if (oRegistro.tieneAdjuntoProtocolo())
            { pinadjunto.Visible = true; }
            else
            { pinadjunto.Visible = false; }

            gvTablaForense.DataSource = oRegistro.getMarcadores();
            gvTablaForense.DataBind();



            if (oRegistro.IdPrioridad.IdPrioridad == 2)
                Session["idUrgencia"] = 2;
            else
                Session["idUrgencia"] = 0;



            if (oRegistro.tieneConsentimiento())
            {
                lnkConsentimiento.Visible = true;
            }

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

            lblTitulo.Text += oRegistro.GetNumero().ToString();

            //ddlServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();

            CargarItems();
            txtFecha.Value = oRegistro.Fecha.ToShortDateString();
            txtFechaOrden.Value = oRegistro.FechaOrden.ToShortDateString();
            txtFechaTomaMuestra.Value = oRegistro.FechaTomaMuestra.ToShortDateString();

            txtDescripcionProducto.Text = oRegistro.DescripcionProducto;



            txtNumeroOrigen.Text = oRegistro.NumeroOrigen;

            ///Datos del Paciente
            if (Request["idPaciente"] == null)
            {
                if (oRegistro.IdPaciente.IdPaciente == -1)
                {

                    pnlPaciente.Visible = false; // en la modificacion de muestra sin paciente puedo querer agregar una persona
                    //divPaciente.Visible = true;
                }
                else
                { //muestra con paciente
                    //divPaciente.Visible = false;
                    pnlPaciente.Visible = true;
                    HFIdPaciente.Value = oRegistro.IdPaciente.IdPaciente.ToString();
                    HFNumeroDocumento.Value = oRegistro.IdPaciente.NumeroDocumento.ToString();
                    lblIdPaciente.Text = oRegistro.IdPaciente.IdPaciente.ToString();
                    lblPaciente.Text = oRegistro.IdPaciente.NumeroDocumento.ToString() + " - " + oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();
                    if (oRegistro.IdPaciente.IdEstado == 2) lblPaciente.Text = oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper() + " (ID Temporal:" + oRegistro.IdPaciente.NumeroDocumento.ToString() + ")";
                    if (oRegistro.IdPaciente.IdEstado == 3)
                        logoRenaper.Visible = true;
                    else
                        logoRenaper.Visible = false;

                    lblFechaNacimiento.Text = oRegistro.IdPaciente.FechaNacimiento.ToShortDateString();

                    lblTelefono.Text = oRegistro.IdPaciente.InformacionContacto;

                    Utility oUtil = new Utility();
                    string[] edad = oUtil.DiferenciaFechas(oRegistro.IdPaciente.FechaNacimiento, oRegistro.Fecha).Split(';');
                    lblEdad.Text = edad[0].ToString();
                    lblUnidadEdad.Text = " " + edad[1].ToUpper();


                    ///cargar foto
                    if (Session["idCaso"] != null)
                    {
                        if (Session["idCaso"].ToString() != "0")
                        {
                            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), "IdCasoFiliacion", int.Parse(Session["idCaso"].ToString()));
                            switch (oCaso.IdTipoCaso)
                            {
                                case 1: //filiacion
                                    {
                                        lblCadenaCustodia.Visible = false;
                                        divfoto.Visible = true;
                                        imgFoto.ImageUrl = string.Format("../imagen.ashx?idPaciente={0}", oRegistro.IdPaciente.IdPaciente.ToString());
                                    }
                                    break;
                                case 2: //forense
                                    {
                                        lblCadenaCustodia.Visible = true;
                                        divfoto.Visible = false;
                                    }
                                    break;
                                case 3: //quimerismo
                                    {
                                        lblCadenaCustodia.Visible = false;
                                        divfoto.Visible = false;
                                    }
                                    break;
                            }


                            switch (oRegistro.getEstadoCaso(Session["idCaso"].ToString()))
                            {
                                case 2: // validado
                                    {
                                        //     btnGuardar.Visible = false;
                                        btnGuardar.Visible = oC.ModificarProtocoloTerminado;
                                        lblEstado.Text = "VALIDADO";
                                        lblEstado.ForeColor = System.Drawing.Color.Green;
                                        lblEstado.Visible = true;
                                    }
                                    break;
                                case 3: //anulado
                                    {
                                        btnGuardar.Visible = true;
                                        lblEstado.Text = "CASO ANULADO";
                                        lblEstado.ForeColor = System.Drawing.Color.Gray;
                                        lblEstado.Visible = true;
                                    }
                                    break;
                                case 1: //SIN VALIDAR
                                    {
                                        btnGuardar.Visible = true;
                                        lblEstado.Text = "SIN INFORMAR RESULTADOS";
                                        lblEstado.ForeColor = System.Drawing.Color.Red;
                                        lblEstado.Visible = true;
                                    }
                                    break;
                            }
                        }
                    }



                    switch (oRegistro.IdPaciente.IdSexo)
                    {
                        case 1: lblSexo.Text = "I"; break;
                        case 2: lblSexo.Text = "F"; HFSexo.Value = "F"; break;
                        case 3: lblSexo.Text = "M"; HFSexo.Value = "M"; break;
                    }

                    if ((oRegistro.IdPaciente.IdEstado == 1) && ((lblSexo.Text == "F") || (lblSexo.Text == "M")))
                        lnkValidarRenaper.Visible = true;
                }


            }
            else
            {
                MostrarPaciente();
            }




            ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();



            ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString();



            ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
            txtObservacion.Text = oRegistro.Observacion;




            MostrarDeterminaciones(oRegistro);



            //if (oRegistro.Estado == 2) btnGuardar.Visible = oC.ModificarProtocoloTerminado;
            //if (oRegistro.getEstadoCaso(Session["idCaso"].ToString()) == 2) btnGuardar.Visible = false;


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
                    if (sDatos == "")
                        sDatos = oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                    else
                        sDatos += ";" + oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                    //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#" + oDet.TrajoMuestra + "@";                   
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

            pnlPaciente.Visible = true;
            Utility oUtil = new Utility();
            ///Muestro los datos del paciente 
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));

            lblPaciente.Text = oPaciente.NumeroDocumento.ToString() + " - " + oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper();
            //lblPacienteNuevo.Text = oPaciente.NumeroDocumento.ToString() + " - " + oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper();
            lblIdPaciente.Text = oPaciente.IdPaciente.ToString();
            HFIdPaciente.Value = oPaciente.IdPaciente.ToString();
            HFNumeroDocumento.Value = oPaciente.NumeroDocumento.ToString();
            lblTelefono.Text = oPaciente.InformacionContacto;

            /// verifica si el paciente tiene protocolos en los ultimos 7 dias
            string tieneingreso = oPaciente.GetProtocolosReciente(7,Request["idServicio"].ToString());

            if (tieneingreso != "")
            {
                lblAlertaProtocolo.Text = " La persona tiene un protocolo ingresado el día " + tieneingreso;
                lblAlertaProtocolo.Visible = true;
            }

            /// 



            if (oPaciente.IdEstado == 2)
            {
                lblPaciente.Text = oPaciente.Apellido.ToUpper() + " " + oPaciente.Nombre.ToUpper() + " (Temporal) ";
                //lblPacienteNuevo.Text += " (Temporal) ";
            }
            //TimeSpan dif = DateTime.Now - DateTime.Parse(oPaciente.FechaNacimiento.ToString());
            // string edadpropuesta = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento);

            string[] edad = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento, DateTime.Now).Split(';');
            lblEdad.Text = edad[0].ToString();
            lblUnidadEdad.Text = " " + edad[1].ToUpper();
            //    ddlEfector.SelectedValue = Origen[1].ToString();

            //txtEdad.Value = edadpropuesta.Split(;);
            lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();


            switch (oPaciente.IdSexo)
            {

                case 1: lblSexo.Text = "I"; break;
                case 2: lblSexo.Text = "F"; break;
                case 3: lblSexo.Text = "M"; break;



            }

            ///cargar foto
            /// 
            imgFoto.ImageUrl = string.Format("../imagen.ashx?idPaciente={0}", Request["idPaciente"].ToString());

            imgFoto.Visible = true;


        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
            //pnlMuestra.Visible = false;

            ///Carga de combos de tipos de servicios          
            TipoServicio oServicio = new TipoServicio();
            oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
            lblServicio.Text = oServicio.Nombre;



            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,   nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));


            ////////////Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();



            if ((Session["idServicio"].ToString() == "3") || (Session["idServicio"].ToString() == "6"))//microbiologia o forense microbiologia
            {
                //ddlPrioridad.SelectedValue = "1";
                //ddlPrioridad.Enabled = false;

                lblServicio0.Text = "";
                ////////////Carga de combos de Muestras
                //pnlMuestra.Visible = true;
                m_ssql = "SELECT idMuestra, nombre + ' - ' + codigo as nombre FROM LAB_Muestra  where (tipo=0 or tipo=1)";
                if (Request["Operacion"].ToString() != "Modifica")  //alta
                    m_ssql += " and baja=0 ";

                m_ssql += " order by nombre ";
                oUtil.CargarCombo(ddlMuestra, m_ssql, "idMuestra", "nombre");
                ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
                rvMuestra.Enabled = true;

            }




            m_ssql = "select idParentesco, nombre from Lab_Parentesco WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlParentesco, m_ssql, "idParentesco", "nombre");
            ddlParentesco.Items.Insert(0, new ListItem("Seleccione un parentesco", "0"));






            if (Request["Operacion"].ToString() != "Modifica")
            {
                if (Session["idUrgencia"] != null)
                {
                    if (Session["idUrgencia"].ToString() == "0")

                        IniciarValores();
                }
            }

            if (Request["Operacion"].ToString() == "AltaDerivacion") IniciarValores();

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
                        //ddlOrigen.SelectedValue = oC.IdOrigenUrgencia.ToString(); //Origen: Guardia
                        ddlSectorServicio.SelectedValue = oC.IdSectorUrgencia.ToString(); // sector de urgencia
                        //ddlPrioridad.SelectedValue = "2"; // Prioridad: Urgencia
                    }
                    else
                    {

                        //ddlPrioridad.SelectedValue = "1"; //Prioridad: Rutina
                    }
                }

            }

            m_ssql = null;
            oUtil = null;
        }



        private void CargarItems()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            //string m_ssql = "SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, " +
            //               " I.disponible " +
            //                " FROM Lab_item I  " +
            //                " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
            //                " where A.baja=0 and I.baja=0  and A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";
            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            //SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            //DataSet ds = new DataSet();
            //da.Fill(ds, "T");
            string m_ssql = "SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, " +
                         " IE.disponible " +
                          " FROM Lab_item I  (nolock) " +
                          " inner join lab_itemEfector IE  (nolock) on I.idItem= IE.idItem and Ie.idefector=" + oC.IdEfector.IdEfector.ToString() + //MultiEfector 
                          " INNER JOIN Lab_area A  (nolock) ON A.idArea= I.idArea " +
                          " where A.baja=0 and I.baja=0 " +
                          " and  A.idtipoServicio= " + Session["idServicio"].ToString() + " AND (I.tipo= 'P') order by I.nombre ";
            SqlConnection strconn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

            //gvLista.DataSource = ds.Tables["T"];
            //gvLista.DataBind();


            //ddlItem.Items.Insert(0, new ListItem("", "0"));

            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                sTareas += "#" + ds.Tables["T"].Rows[i][1].ToString() + "#" + ds.Tables["T"].Rows[i][2].ToString() + "#" + ds.Tables["T"].Rows[i][4].ToString() + "@";
            }
            txtTareas.Value = sTareas;




            m_ssql = null;
            oUtil = null;
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { ///Verifica si se trata de un alta o modificacion de protocolo               
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                if (Request["Operacion"].ToString() == "Modifica") oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));


                Guardar(oRegistro);


                if ((oRegistro.IdTipoServicio.IdTipoServicio == 6))
                {
                    if (Session["idCaso"].ToString() != "0")
                    {
                        Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                        oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), "IdCasoFiliacion", int.Parse(Session["idCaso"].ToString()));
                        if (Request["Operacion"].ToString() == "Alta")
                        {
                            AlmacenarSesion();

                            if (oCaso.IdTipoCaso == 2)
                            {
                                HttpContext Context;

                                Context = HttpContext.Current;
                                Context.Items.Add("idServicio", "6");
                                Context.Items.Add("id", Session["idCaso"].ToString());
                                Server.Transfer("../CasoFiliacion/CasoForenseView.aspx");
                            }

                            if (oCaso.IdTipoCaso != 2)
                                //caso de filiacion: sigue el circuito con identificacion de personas y consentimiento
                                Response.Redirect("Default2.aspx?idServicio=6&idUrgencia=0&idCaso=" + oCaso.IdCasoFiliacion.ToString() + "&idTipoCaso=" + oCaso.IdTipoCaso.ToString(), false);
                        }
                    }
                    else
                    {
                        if (Request["Operacion"].ToString() == "Alta")
                        {
                            AlmacenarSesion();
                            Response.Redirect("ProtocoloMensaje.aspx?id=" + oRegistro.IdProtocolo.ToString());
                        }
                        else Response.Redirect("ProtocoloListForense.aspx?idServicio=6&tipo=Lista",false);
                    }
                }
                //// se muestra un mensaje con el numero de protocolo y te da opcion de adjuntos y de consentieminto si es forense.






                //if (Request["idTurno"] != null)
                //    ActualizarTurno(Request["idTurno"].ToString(), oRegistro);

                if (Request["Operacion"].ToString() == "Modifica")
                {

                    switch (Request["Desde"].ToString())
                    {
                        //case "Default": Response.Redirect("Default.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
                        case "ProtocoloList": Response.Redirect("ProtocoloListForense.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Lista"); break;
                        case "CasoCarga":
                            {
                                Context.Items.Add("id", Session["idCaso"].ToString());

                                Context.Items.Add("Desde", "Carga");

                                Server.Transfer("../CasoFiliacion/CasoResultado3.aspx?");
                            }
                            break;
                        case "CasoValida":
                            {
                                Context.Items.Add("id", Session["idCaso"].ToString());

                                Context.Items.Add("Desde", "Valida");

                                Server.Transfer("../CasoFiliacion/CasoResultado3.aspx?");
                            }
                            break;
                        case "CasoRevalida":
                            {
                                Context.Items.Add("id", Session["idCaso"].ToString());

                                Context.Items.Add("Desde", "Revalida");

                                Server.Transfer("../CasoFiliacion/CasoResultado3.aspx?");
                            }
                            break;
                        case "Control": Avanzar(1); break;
                            //case "Urgencia": Response.Redirect("../Urgencia/UrgenciaList.aspx", false); break;

                    }


                }
              
              
            }

        }






        private void IniciarValores()
        {
            if (Session["ProtocoloForense"] != null)
            {
                string[] arr = Session["ProtocoloForense"].ToString().Split(("@").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        case "ddlMuestra":
                            {
                                if (Request["Operacion"].ToString() == "Alta")

                                {
                                    ddlMuestra.SelectedValue = s_control[1].ToString();
                                    mostrarCodigoMuestra();
                                }
                            }
                            break;
                        case "ddlSectorServicio":
                            {
                                if (Request["Operacion"].ToString() == "Alta")
                                {
                                    ddlSectorServicio.SelectedValue = s_control[1].ToString();
                                }
                            }
                            break;
                        case "nroOrigen":
                            txtNumeroOrigen.Text = s_control[1].ToString(); break;                            
                        case "ddlEfector":
                            {
                                if (Request["Operacion"].ToString() == "Alta")
                                {
                                    ddlEfector.SelectedValue = s_control[1].ToString();
                                }
                            }
                            break;
                        case "txtFechaTomaMuestra":
                            txtFechaTomaMuestra.Value = s_control[1].ToString(); break;
                        case "txtFechaOrden":
                            txtFechaOrden.Value = s_control[1].ToString(); break;
                    }
                }
            }



        }

        private void ActualizarTurno(string p, Business.Data.Laboratorio.Protocolo oRegistro)
        {
            Turno oTurno = new Turno();
            oTurno = (Turno)oTurno.Get(typeof(Turno), int.Parse(p));
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
                //Efector oEfector = new Efector();
                //Usuario oUser = new Usuario();

                Paciente oPaciente = new Paciente();
                ObraSocial oObra = new ObraSocial();
                Origen oOrigen = new Origen();
                Prioridad oPri = new Prioridad();
                DateTime fecha = DateTime.Parse(txtFecha.Value);

                //Configuracion oC = new Configuracion();
                //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

                oRegistro.IdEfector = oC.IdEfector;
                SectorServicio oSector = new SectorServicio();
                oSector = (SectorServicio)oSector.Get(typeof(SectorServicio), int.Parse(ddlSectorServicio.SelectedValue));
                oRegistro.IdSector = oSector;

                TipoServicio oServicio = new TipoServicio();
                oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
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
                oRegistro.FechaTomaMuestra = DateTime.Parse(txtFechaTomaMuestra.Value);
                oRegistro.FechaRetiro = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);

                Efector oEfectorSol = new Efector();

                oRegistro.IdEfectorSolicitante = (Efector)oEfectorSol.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                oRegistro.IdEspecialistaSolicitante = 0; // int.Parse(ddlEspecialista.SelectedValue);

                oRegistro.FechaInicioSintomas = DateTime.Parse("01/01/1900");
                oRegistro.FechaUltimoContacto = DateTime.Parse("01/01/1900");

                if (lblIdPaciente.Text != "")
                    oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(lblIdPaciente.Text));
                if (Request["Operacion"].ToString() == "Modifica")
                {

                    if (oRegistro.IdPaciente != oPaciente)
                        oRegistro.GrabarAuditoriaProtocolo("Cambia de Paciente", int.Parse(Session["idUsuario"].ToString()));
                }
                else


                    ///Desde aca guarda los datos del paciente en Protocolo
                    oRegistro.IdPaciente = oPaciente;


                oRegistro.Edad = int.Parse(lblEdad.Text);
                //if (txtHoraNac.Value!="")oRegistro.HoraNacimiento = txtHoraNac.Value;
                //if (txtPesoNac.Value!="") oRegistro.PesoNacimiento = int.Parse(txtPesoNac.Value, System.Globalization.CultureInfo.InvariantCulture);
                //if (txtSemanaGestacion.Value!="") oRegistro.SemanaGestacion = int.Parse(txtSemanaGestacion.Value);
                //if (txtNumeroOrigen.Text != "")
                oRegistro.NumeroOrigen = txtNumeroOrigen.Text;
                oRegistro.DescripcionProducto = txtDescripcionProducto.Text;
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
                oRegistro.Sala = ""; // txtSala.Text;
                oRegistro.Cama = ""; // txtCama.Text;


                ObraSocial oObraSocial = new ObraSocial();
                oRegistro.IdObraSocial = (ObraSocial)oObraSocial.Get(typeof(ObraSocial), -1); //reemplzar

                oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), 1);
                oRegistro.IdPrioridad = (Prioridad)oPri.Get(typeof(Prioridad), 1);
                oRegistro.Observacion = txtObservacion.Text;
                oRegistro.ObservacionResultado = "";
                oRegistro.IdMuestra = int.Parse(ddlMuestra.SelectedValue);
                if (Request["Operacion"].ToString() != "Modifica")
                {
                    oRegistro.IdUsuarioRegistro = oUser; // (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;
                }



                oRegistro.Save();
                oRegistro.ActualizarNumeroDesdeID();





                GuardarDetalle(oRegistro);
                //oRegistro.VerificarExisteNumeroAsignado();

                oRegistro.GrabarAuditoriaProtocolo(Request["Operacion"].ToString(), oUser.IdUsuario); // int.Parse(Session["idUsuario"].ToString()));


                if (Session["idCaso"] == null)                    Session["idCaso"] = "0";
                if (Session["idCaso"].ToString() != "0")
                {
                    GuardarDetalleCaso(oRegistro);


                    Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                    oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));
                    oCaso.GrabarAuditoria(Request["Operacion"].ToString() + " Protocolo ", oUser.IdUsuario, "Nro. " + oRegistro.GetNumero());

                    ///veririfcia si fue validada la seccion muestras, se desvalida
                    AuditoriaCasoFiliacion oMarca = new AuditoriaCasoFiliacion();
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit2 = m_session.CreateCriteria(typeof(AuditoriaCasoFiliacion));
                    crit2.Add(Expression.Eq("IdCasoFiliacion", oCaso.IdCasoFiliacion));
                    crit2.Add(Expression.Eq("Accion", "Valida Muestras"));


                    IList itemsm = crit2.List();
                    if (itemsm.Count > 0)
                    {
                        oCaso.GrabarAuditoria("DesValida Muestras", oUser.IdUsuario, "Motivo Modificacion Protocolo " + oRegistro.GetNumero());
                    }

                    /// si el caso ya fue validado se desvalida para volver a generar el informe
                    if (oCaso.IdUsuarioValida > 0)
                    {
                        oCaso.FechaValida = DateTime.Parse("1900-01-01");
                        oCaso.IdUsuarioValida = 0;
                        oCaso.Save();
                        oCaso.GrabarAuditoria("Desvalida Caso ", oUser.IdUsuario, "Motivo Modificacion Protocolo " + oRegistro.GetNumero());
                    }
                }


            }
            else
            { //doble submit
            }
        }

         

        private void GuardarDetalleCaso(Protocolo oRegistro)
        {
            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), "IdCasoFiliacion", int.Parse(Session["idCaso"].ToString()));

            //Borra el protocolo del caso con la relacion de parentesco
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            crit.Add(Expression.Eq("IdCasoFiliacion", oCaso));
            IList detalle = crit.List();
            foreach (CasoFiliacionProtocolo oDetalle1 in detalle)
            {
                oDetalle1.Delete();
            }

            //Graba el protocolo del caso con la relacion de parentesco
            CasoFiliacionProtocolo oDetalle = new CasoFiliacionProtocolo();
            oDetalle.IdCasoFiliacion = oCaso;
            oDetalle.IdTipoParentesco = int.Parse(ddlParentesco.SelectedValue);
            oDetalle.IdProtocolo = oRegistro;
            oDetalle.ObservacionParentesco = txtObservacionParentesco.Text;
            oDetalle.Save();

            oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), oUser.IdUsuario);
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
                    recordar_practicas = codigo + "#Si#False";
                else
                    recordar_practicas += ";" + codigo + "#Si#False";

                if (codigo != "")
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo, "Baja", false);

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
                        oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");


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
                                if (codigo == oDetalle.IdItem.Codigo) noesta = false;

                            }
                        }
                        if (noesta)
                        {
                            oDetalle.Delete();
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Elimina", oUser.IdUsuario);
                        }
                    }
                }
            }

            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

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

        private void AlmacenarSesion()
        {

            string s_valores = "ddlSectorServicio:" + ddlSectorServicio.SelectedValue;
            if (ddlMuestra.SelectedValue != "0") s_valores += "@ddlMuestra:" + ddlMuestra.SelectedValue;
            s_valores += "@nroOrigen:" + txtNumeroOrigen.Text;
            s_valores += "@ddlEfector:" + ddlEfector.SelectedValue;
            s_valores += "@txtFechaTomaMuestra:" + txtFechaTomaMuestra.Value;
            s_valores += "@txtFechaOrden:" + txtFechaOrden.Value;
            Session["ProtocoloForense"] = s_valores;

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
                if (codigo != "")
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
                    oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                    GuardarDetallePractica(oDetalle);
                }
            }


            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            //     DateTime fechaentrega;
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
                                oDetalle.IdProtocolo = oDet.IdProtocolo;
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


            //AgregarDiagnostico();
        }






        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            int id_efector = oC.IdEfector.IdEfector;
            if (Request["Operacion"].ToString() == "Modifica")
            {
                if (Request["DesdeUrgencia"] != null)
                    Response.Redirect("../Urgencia/UrgenciaList.aspx");
                else
                {
                    switch (Request["Desde"].ToString())
                    {
                        case "Default": Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;
                        case "ProtocoloList": Response.Redirect("ProtocoloListForense.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Lista"); break;
                        case "Control": Response.Redirect("ProtocoloListForense.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Control"); break;
                        case "Urgencia": Response.Redirect("../Urgencia/UrgenciaList.aspx", false); break;
                        case "Derivacion": Response.Redirect("Derivacion.aspx?idServicio=" + Session["idServicio"].ToString(), false); break;

                        case "CasoCarga":
                            {
                                Context.Items.Add("id", Session["idCaso"].ToString());

                                Context.Items.Add("Desde", "Carga");

                                Server.Transfer("../CasoFiliacion/CasoResultado3.aspx?");
                            }
                            break;
                        case "CasoValida":
                            {
                                Context.Items.Add("id", Session["idCaso"].ToString());

                                Context.Items.Add("Desde", "Valida");

                                Server.Transfer("../CasoFiliacion/CasoResultado3.aspx?");
                            }
                            break;
                        case "CasoRevalida":
                            {
                                Context.Items.Add("id", Session["idCaso"].ToString());

                                Context.Items.Add("Desde", "Revalida");

                                Server.Transfer("../CasoFiliacion/CasoResultado3.aspx?");
                            }
                            break;

                    }

 
                }
            }
            else
            {
                if (Request["Operacion"].ToString() == "AltaTurno")
                    Response.Redirect("../turnos/Turnolist.aspx", false);
                else
                {
                    if (Request["Operacion"].ToString() == "AltaDerivacion")
                        Response.Redirect("Derivacion.aspx?idEfector=" + id_efector.ToString() + "&idServicio=" + Session["idServicio"].ToString(), false);
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




        protected void btnAgregarRutina_Click(object sender, EventArgs e)
        {
            // if (ddlRutina.SelectedValue != "0")
            // AgregarRutina();           

        }




        protected void ddlServicio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            CargarItems();
            TxtDatos.Value = "";

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




        protected void btnGuardarSolicitante_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //GuardarSolicitanteExterno();

                LimpiarDatosSolicitante();
                //Panel1.Visible = false;
                //Panel1.UpdateAfterCallBack = true;
            }
        }




        protected void btnCancelarSolicitante_Click(object sender, EventArgs e)
        {
            LimpiarDatosSolicitante();

        }

        private void LimpiarDatosSolicitante()
        {
            //txtMatricula.Text = "";
            //txtApellidoSolicitante.Text = "";
            //txtNombreSolicitante.Text = "";
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            ///Validacion de la fecha de la fecha de toma de muestra

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
            //}
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
                        if ((codigo2 != "") && (codigo != codigo2))
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
                for (int i = 0; i < lista.Length; i++)
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
                        else  //retrocede                        
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
             crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
            Protocolo oProtocolo = (Protocolo)crit.UniqueResult();

            string m_parametro = "";
            if (Request["DesdeUrgencia"] != null) m_parametro = "&DesdeUrgencia=1";

            if (oProtocolo != null)
            {
                //if (Request["Desde"].ToString() == "Control")
                Response.Redirect("ProtocoloEditForense.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
                //else
                //    Response.Redirect("ProtocoloEdit2.aspx?Desde="+Request["Desde"].ToString()+"&idServicio=" + Session["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
            }
            else
                Response.Redirect("ProtocoloEditForense.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloActual + m_parametro);

        }



        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
                Response.Redirect("ProtocoloEditForense.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + e.CommandArgument.ToString());


        }



        protected void btnSelObraSocial_Click(object sender, EventArgs e)
        {
            actualizarObraSocial();
        }

        private void actualizarObraSocial()
        {
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(HFIdPaciente.Value));

            //ObraSocial oObraSocial = new ObraSocial();
            //oObraSocial = (ObraSocial)oObraSocial.Get(typeof(ObraSocial), int.Parse(oPaciente.IdObraSocial.ToString()));

            //lblObraSocial.Text = oObraSocial.Nombre;

            if (Request["idProtocolo"] != null)
            {
                TxtDatos.Value = "";
                TxtDatosCargados.Value = "";
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

                MostrarDeterminaciones(oRegistro);
            }
        }

        //protected void txtNumeroDocumento_TextChanged(object sender, EventArgs e)
        //{
        //    Paciente oPaciente = new Paciente();
        //    oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "NumeroDocumento",int.Parse( txtNumeroDocumento.Text), "IdEstado",1);
        //    if (oPaciente != null)
        //    {

        //        txtApellido.Text = oPaciente.Apellido;
        //        txtApellido.Enabled = false;

        //        txtNombre.Text = oPaciente.Nombre;
        //        txtNombre.Enabled = false;
        //        txtFechaNac.Text = oPaciente.FechaNacimiento.ToShortDateString();
        //        txtFechaNac.Enabled = false;
        //        ddlSexo.SelectedValue = oPaciente.IdSexo.ToString();
        //        lblIdPaciente.Text = oPaciente.IdPaciente.ToString();
        //    }
        //    else
        //    {
        //        txtApellido.Text = "";
        //        txtApellido.Enabled = true;
        //        txtNombre.Text = ""; txtNombre.Enabled = true;

        //        txtFechaNac.Text = ""; txtFechaNac.Enabled = true;
        //        ddlSexo.Enabled = true;
        //    }
        //    txtApellido.UpdateAfterCallBack = true;
        //    txtNombre.UpdateAfterCallBack = true;
        //    txtFechaNac.UpdateAfterCallBack = true;
        //    ddlSexo.UpdateAfterCallBack = true;

        //}

        protected void lnkSiguiente0_Click(object sender, EventArgs e)
        {

            Response.Redirect("ProtocoloAdjuntar.aspx?idProtocolo=" + Request["idProtocolo"].ToString() + "&desde=protocolo");
        }

        protected void lnkConsentimiento_Click(object sender, EventArgs e)
        {
            ReimprimirConsentimmiento();

        }

        private void ReimprimirConsentimmiento()
        {
            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));
            oCaso.GrabarAuditoria("Reimprime Consentimiento ", oUser.IdUsuario, "Persona: " + lblPaciente.Text);



            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            string m_strSQL = @" select P.apellido, 
case when idestado=2 then 'Sin DNI' else convert(varchar,P.numerodocumento)   end as numerodocumento, 
C.fecha, P.nombre, P.fechaNacimiento, C.lugar as sector,
P.informacionContacto as observaciones, C.imgFoto as imagen from sys_paciente  as P
inner join lab_consentimiento as C on C.idpaciente = P.idpaciente where C.idPaciente=" + HFIdPaciente.Value;

            DataSet Ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            oCr.Report.FileName = "../Informes/Consentimiento.rpt";
            oCr.ReportDocument.SetDataSource(Ds.Tables[0]);



            oCr.DataBind();

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Consentimiento");

        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Avanzar(0);

        }

        protected void gvListaCaso_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Desvincular")
            {
                Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), "IdCasoFiliacion", int.Parse(e.CommandArgument.ToString()));




                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));


                //Borra el protocolo del caso con la relacion de parentesco
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
                crit.Add(Expression.Eq("IdProtocolo", oRegistro));
                crit.Add(Expression.Eq("IdCasoFiliacion", oCaso));
                IList detalle = crit.List();
                foreach (CasoFiliacionProtocolo oDetalle1 in detalle)
                {
                    oDetalle1.Delete();
                }

                oRegistro.GrabarAuditoriaProtocolo ("Desvinculado de caso " + oCaso.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));



            }

            }

        protected void gvListaCaso_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {



                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdModificar = (LinkButton)e.Row.Cells[3].Controls[1];
                    CmdModificar.CommandArgument = this.gvListaCaso.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdModificar.CommandName = "Desvincular";
                    CmdModificar.ToolTip = "Desvincular";


                    Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                    oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), "IdCasoFiliacion", int.Parse(this.gvListaCaso.DataKeys[e.Row.RowIndex].Value.ToString()));
                    if (oCaso.IdUsuarioValida > 0)  // si está terminado no se puede eliminar.
                        CmdModificar.Visible = false;



                }
            }
        }

        protected void subir_Click(object sender, EventArgs e)
        {

            try
            {
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

                    if (Directory.Exists(directorio))
                    {
                        string archivo = directorio + "\\" + trepador.FileName;


                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";


                        ProcesarFichero();

                        CargarTablaResultados();

                        //SetSelectedTab(TabIndex.ONE);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }


        }


        private void ProcesarFichero()
        {



            if (this.trepador.HasFile)
            {
                string filename = this.trepador.PostedFile.FileName;
                BorrarResultadosTemporales(1);
                int i = 1;
                if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                {
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            if (i != 1)

                                ProcesarLinea(line, 1);

                            i += 1;
                        }
                    }
                }
            }
        }

        private void BorrarResultadosTemporales(int tipo)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(MarcadoresTemp));

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (MarcadoresTemp oDetalle in detalle)
                {
                    oDetalle.Delete();

                }
            }
        }
        private void ProcesarLinea(string linea, int tipo)
        {
            try
            {


                Utility oUtil = new Utility();
                string[] arr = linea.Split(("\t").ToCharArray());



                if (tipo == 1)
                {
                    if (arr.Length >= 4)
                    {
                        string campo_cero = arr[0].ToString();
                        string campo_MFI = arr[1].ToString();
                        string campo_Bead = arr[2].ToString();
                        string campo_valor = arr[3].ToString();

                        Protocolo oP = new Protocolo();
                        oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", campo_cero.Substring(0, 5));

                        if (oP != null)
                        {    // carga directamente en la base nueva 

                            MarcadoresTemp oRegistro = new MarcadoresTemp();

                            oRegistro.IdProtocolo = oP.IdProtocolo;
                            oRegistro.IdPaciente = oP.IdPaciente.IdPaciente;

                            oRegistro.Marcador = campo_MFI;
                            oRegistro.Allello1 = campo_Bead;
                            oRegistro.Allello2 = campo_valor;

                            oRegistro.Save();
                        }

                    } //tipo ==1


                }







            }
            catch (Exception ex)
            {
                string exception = "";
                exception = ex.Message + "<br>";
                estatus.Text = "hay líneas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }

        private void CargarTablaResultados( )
        {
            gvTablaForense.DataSource = LeerDatos();
            gvTablaForense.DataBind();



        }
        private object LeerDatos()
        {
            string columnas = "";
            string lista = "";
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;

            string m_strSQL = @"  select distinct idprotocolo from [GEN_MarcadoresTemp]  ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                string s_idprotocolo = Ds.Tables[0].Rows[i][0].ToString();

                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idprotocolo));

                if (lista == "")
                    lista = s_idprotocolo;
                else
                    lista += ", " + s_idprotocolo;


                if (columnas == "")
                    columnas = "[" + oProtocolo.Numero.ToString() + "]";
                else
                    columnas += ",[" + oProtocolo.Numero.ToString() + "]";
            }

            m_strSQL = @" SELECT tipo as [ ], " + columnas + @"
 FROM(    
  SELECT isnull( P.numero,0) as numero, marcador as tipo , 
  allello1 +' - ' +   case when marcador<>'DYS391' then
  case when allello2='' then allello1 else allello2 end   
  else allello2 end  as y,  
  
  case marcador  when 'AMEL' then 1 when 'D3S1358' then 2 when 'D1S1656' then 3 when 'D2S441' 
  then 4 when 'D10S1248' then 5 when 'D13S317' then 6 when 'PENTA E' then 7 when 'D16S539' then 8 when 'D18S51' then 9 
  when 'D2S1338' then 10 when 'CSF1PO' then 11 when 'PENTA D' then 12 when 'TH01' then 13 when 'VWA' then 14 when 'D21S11' 
  then 15 when 'D7S820' then 16 when 'D5S818' then 17 when 'TPOX' then 18 when 'DYS391' then 19 when 'D8S1179' then 20 
  when 'D12S391' then 21 when 'D19S433' then 22 when 'FGA' then 23 when 'D22S1045' then 24 end as orden    
   FROM GEN_MarcadoresTemp    as M
				  left join LAB_Protocolo as P on P.idprotocolo= M.idprotocolo
				    where   M.idprotocolo in (" + lista + @") and M.idprotocolo not in (select idprotocolo from gen_marcadores))
   AS SourceTable PIVOT(max(y) 
   FOR numero IN(" + columnas + @" )) AS PivotTable order by orden";


            DataSet Ds1 = new DataSet();

            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1);

            return Ds1.Tables[0];
        }



        protected void btnAgregar_Click(object sender, EventArgs e)
        {


            ISession m_session = NHibernateHttpModule.CurrentSession;

            //verifica que ingrese solo el protocolo del caso
            MarcadoresTemp oCasoMarcadores = new MarcadoresTemp();
            ICriteria crit = m_session.CreateCriteria(typeof(MarcadoresTemp));


            IList detalle = crit.List();
            foreach (MarcadoresTemp oDetalle in detalle)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), oDetalle.IdProtocolo);

                /// se agrega a la base de frecuencias alelicas y a la base total de marcadores
                /// 
                CasoMarcadores oBase = new CasoMarcadores();
                oBase.IdProtocolo = oDetalle.IdProtocolo;
                oBase.IdCasoFiliacion = 0;
                oBase.Marcador = oDetalle.Marcador;
                oBase.Allello1 = oDetalle.Allello1;
                oBase.Allello2 = oDetalle.Allello2;
                oBase.Save();

               



            } //for                
             
        }

      

        protected void lnkValidarRenaper_Click(object sender, EventArgs e)
        {
            Avanzar(0);
        }
    }
}

