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

namespace WebLab.Protocolos
{
    public partial class ProtocoloProductoEdit : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oC = new Configuracion();

      
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

       

        private void CargarGrilla()
        {
            ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @" Select   P.idProtocolo, P.numero as numero,
                                 convert(varchar(10),P.fecha,103) as fecha,P.estado
                              from Lab_Protocolo P with (nolock)
                               WHERE P.idProtocolo in (" + Session["ListaProtocolo"].ToString() + ")    order by numero ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();     
        }

        private void PreventingDoubleSubmit(Button button)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Procesando...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
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
                //PreventingDoubleSubmit(btnGuardar);
                if (Session["idUsuario"] != null)
                {
                    pnlIncidencia.Visible = false;
                    tituloCalidad.Visible = false;
                    this.IncidenciaEdit1.Visible = false;
                    btnCancelar.Visible = false;

                    CargarListas();
                    btnArchivos.Visible = false;
                    if (Request["Operacion"].ToString() == "Modifica")
                    {
                        chkRecordarPractica.Visible = false;
                        btnCancelar.Visible = true;                
                        //lnkReimprimirCodigoBarras.Visible = true;
                        lblServicio.Visible = false; lblServicio1.Visible = true;
                        lblTitulo.Visible = true;
                        
                        pnlActualiza.Visible = true;
                        lblUsuario.Visible = true;
                        lblEstado.Visible = true;
                        MuestraDatos();
                        //VerificaPermisos("Recepcion de Muestras");
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
                        chkRecordarPractica.Visible = true;
                        //lnkReimprimirCodigoBarras.Visible = false;
                        lblTitulo.Visible = false;
                        txtFecha.Value = DateTime.Now.ToShortDateString();
                        txtFechaOrden.Value = DateTime.Now.ToShortDateString();
                        //MostrarPaciente();

                        lblServicio1.Visible = true;
                        pnlActualiza.Visible = false;

                        btnCancelar.Text = "Cancelar";
                        btnCancelar.Width = Unit.Pixel(80);
                        btnCancelar.Visible = true;
                        if (Request["Operacion"].ToString() == "AltaDerivacionMultiEfectorLote")
                        {
                            CargarProtocoloDerivadoLote(); 
                        }

                    }
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
        }

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

        private void CargarDeterminacionesDerivacion(string s_analisis)
        {
            string[] tabla = s_analisis.Split('|');
            string pivot = "";
            string sDatos = "";

            /////Crea nuevamente los detalles.
            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(tabla[i].ToString()));
                if (oItem != null)
                    if (pivot != oItem.Nombre)
                    {
                        if (sDatos == "")
                            sDatos = oItem.Codigo + "#Si#False";
                        else
                            sDatos += ";" + oItem.Codigo + "#Si#False";
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

            ddlSectorServicio.SelectedValue = oTurno.IdSector.ToString();
            ddlMuestra.SelectedValue = oTurno.IdEspecialistaSolicitante.ToString();
            //ddlObraSocial.SelectedValue = oTurno.IdObraSocial.IdObraSocial.ToString();
         //   ddlServicio.SelectedValue = oTurno.IdTipoServicio.IdTipoServicio.ToString();

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

            
        }

        private void MuestraDatos()
        {
            //Actualiza los datos de los objetos : alta o modificacion .
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));


            if (oRegistro.tieneAdjuntoProtocolo())
            { spanadjunto.Visible = true; btnArchivos.Visible = true; }
            else
            { spanadjunto.Visible = false; btnArchivos.Visible = false; }

            Session["idUrgencia"] = 0;

            pnlIncidencia.Visible = true;
            tituloCalidad.Visible = true;
            this.IncidenciaEdit1.Visible = true;
            this.IncidenciaEdit1.MostrarDatosdelProtocolo(oRegistro.IdProtocolo);
          
            if (oRegistro.getIncidencias() > 0)
                inci.Visible = true;
            else
                inci.Visible = false;
                        
            
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
            

            txtNumeroOrigen.Text = oRegistro.NumeroOrigen;



            ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString(); 
            

            ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();
         

             

            //ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();
                        
            //ddlPrioridad.SelectedValue = oRegistro.IdPrioridad.IdPrioridad.ToString();


            ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
            ddlConservacion.SelectedValue = oRegistro.IdConservacion.ToString();
            txtDescripcionProducto.Text = oRegistro.DescripcionProducto;
            txtObservacion.Text = oRegistro.Observacion;


            ///Agregar a la tabla las determinaciones para mostrarlas en el gridview                             
            //dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            crit.AddOrder ( Order.Asc("IdDetalleProtocolo"));

            IList items = crit.List();
            string pivot = "";
            string sDatos = "";
            foreach (DetalleProtocolo oDet in items)
            {
                if (pivot != oDet.IdItem.Nombre)
                {
                    /*    if (sDatos == "")
                            sDatos = oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                        else
                            sDatos += ";" + oDet.IdItem.Codigo + "#" + oDet.TrajoMuestra + "#" + oDet.ConResultado;
                    */

                    string estado = "0";
                    if (oDet.IdUsuarioValida > 0) //validado
                        estado = "2";
                    else
                    {
                        if ((oDet.IdUsuarioResultado > 0) || (oDet.Enviado == 2)) //cargado
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
            

            
            //chkRecordarConfiguracion.Visible = false;

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
                    if (p == 1)
                    {
                        result = "EN PROCESO";
                        lblEstado.CssClass = "label label-warning";
                    }
                    else
                    {
                        result = "TERMINADO";
                        lblEstado.CssClass = "label label-success";
                    }
                }
                if (p == 2)  //terminado
                { /// solo si está terminado no se puede modificar                
                    btnGuardar.Visible = false;
            

                }
            }
            else
            {
                result = "ANULADO";
                lblEstado.CssClass = "label label-default";                
                btnGuardar.Visible = false;              

            }
            return result;
        }

            
        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            ///Carga de combos de tipos de servicios          
            TipoServicio oServicio = new TipoServicio();            oServicio =(TipoServicio) oServicio.Get(typeof(TipoServicio), 5);
            
            lblServicio.Text = oServicio.Nombre + " NUEVO PROTOCOLO " ;
            lblServicio1.Text = "PROTOCOLO DE " + oServicio.Nombre.ToUpper();
            
            
            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,  prefijo + ' - ' + nombre   as nombre FROM LAB_SectorServicio with (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre", connReady);
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));


            /////////////////////////////////////////////CODIGO DE BARRAS//////////////////////////////////////////////////////////////////////
            tab3Titulo.Visible = false;
            pnlEtiquetas.Visible = false;
            TipoServicio oServicioMicrobiologia = new TipoServicio();
            oServicioMicrobiologia = (TipoServicio)oServicioMicrobiologia.Get(typeof(TipoServicio), 3);
            

            ConfiguracionCodigoBarra oConfiguracion = new ConfiguracionCodigoBarra();
            oConfiguracion = (ConfiguracionCodigoBarra)oConfiguracion.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oServicioMicrobiologia, "IdEfector", oUser.IdEfector);
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
                  (Request["Operacion"].ToString() == "AltaDerivacionMultiEfectorLote") ||
                  (Request["Operacion"].ToString() == "AltaPeticion") ||
                  (Request["Operacion"].ToString() == "AltaFFEE")
                  )
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

                        m_ssql = @"select idArea, nombre from Lab_Area  A with (nolock)
                            WHERE imprimeCodigoBarra=1 
                            and baja=0
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

                    chkAreaCodigoBarra.Items.Clear();
                    ddlImpresoraEtiqueta.Items.Insert(0, new ListItem("Sin impresora", "0"));
                    ddlImpresora2.Items.Insert(0, new ListItem("Sin impresora", "0"));
                    ddlImpresoraEtiqueta.Enabled = false;
                    btnReimprimirCodigoBarras.Enabled = false;
                    lblMensajeImpresion.Text = "No se ha habilitado la impresion de etiquetas";
                    lblMensajeImpresion.UpdateAfterCallBack = true;
                }
            }
                ////////////Carga de combos de Muestras
                //    pnlMuestra.Visible = true;
                m_ssql = @"SELECT idMuestra, nombre  + ' - ' + codigo as nombre FROM LAB_Muestra M with (nolock)
                            where (tipo=0 or tipo=2)";
            if (Request["Operacion"].ToString() != "Modifica")  //alta
                m_ssql += " and baja=0 and exists (select 1 from lab_muestraEfector E  with (nolock) where M.idMuestra = E.idmuestra and E.idefector = " + oUser.IdEfector.IdEfector.ToString() + ")"; //Multiefector";


            m_ssql += " order by nombre ";
                oUtil.CargarCombo(ddlMuestra, m_ssql, "idMuestra", "nombre", connReady);
                ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Tipo de Muestra--", "0"));
            //  rvMuestra.Enabled = true;

            //  }        

            ///Carga de conservacion
             m_ssql = " select idConservacion, descripcion from lab_conservacion with (nolock) where baja=0 order by descripcion";
            oUtil.CargarCombo(ddlConservacion, m_ssql, "idConservacion", "descripcion", connReady);
            ddlConservacion.Items.Insert(0, new ListItem("Seleccione", "0"));

            ////////////Carga de combos de Efector
            ////////////Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector E with (nolock) ";

            //if (Request["Operacion"].ToString() != "Modifica")  //alta
            m_ssql += " where exists (select 1 from LAB_EfectorRelacionado R with (nolock) where E.idEfector = R.idEfectorRel and R.idefector = " + oUser.IdEfector.IdEfector.ToString() +
                ")  or (E.idEfector=" + oC.IdEfector.IdEfector.ToString() + @" )";

            m_ssql += " order by nombre ";

            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();

            
            m_ssql = @"SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre 
                     FROM Lab_item I  with (nolock)
                     INNER JOIN Lab_area A with (nolock) ON A.idArea= I.idArea 
                     where A.baja=0 and I.baja=0 and  I.disponible=1 and A.idtipoServicio in (1, 3) AND (I.tipo= 'P') order by I.nombre ";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("--Seleccione una practica/determinacion--", "0"));
            ddlItem.SelectedValue = "0";

            if (Request["Operacion"].ToString() != "Modifica")                        
                        IniciarValores(oC);
              

        
          ///Carga de determinaciones y rutinas dependen de la selección del tipo de servicio
             CargarItems();

            

            m_ssql = null;
            oUtil = null;
        }

        private void IniciarValores(Configuracion oC)
        {
            if (Session["ProtocoloNoPaciente"] != null)
            {
            
                string[] arr = Session["ProtocoloNoPaciente"].ToString().Split(("@").ToCharArray());
                foreach (string item in arr)
                {
                    string[] s_control = item.Split((":").ToCharArray());
                    switch (s_control[0].ToString())
                    {
                        case "ddlConservacion":
                            {

                                if (Request["Operacion"].ToString() != "Modifica")
                                    if (Request["Operacion"].ToString() != "AltaTurno")
                                    {
                                        ddlConservacion.SelectedValue = s_control[1].ToString();                                      
                                    }
                            }
                            break;

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
                    
                        case "ddlEfector":
                            { 
                                if (Request["Operacion"].ToString() != "Modifica")
                                    if (Request["Operacion"].ToString() != "AltaTurno")                                     
                                        ddlEfector.SelectedValue = s_control[1].ToString();
                                }
                            break;
                        case "ddlSectorServicio":
                            {                                
                                    if (Request["Operacion"].ToString() != "Modifica")
                                        if (Request["Operacion"].ToString() != "AltaTurno")                                        
                                            ddlSectorServicio.SelectedValue = s_control[1].ToString();                                        
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
                        case "practicas":
                            TxtDatosCargados.Value = s_control[1].ToString(); break;

                        case "ddlImpresoraEtiqueta":
                            ddlImpresoraEtiqueta.SelectedValue = s_control[1].ToString(); break;
                    }
                }
            }
            else
            {
                

                //if (Session["Etiquetadora"]!=null)  ddlImpresoraEtiqueta.SelectedValue=Session["Etiquetadora"] .ToString();
            }

        }
   
        private void CargarItems()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            //string m_ssql = @" SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, I.disponible 
            //                 FROM Lab_item I with (nolock) 
            //                 INNER JOIN Lab_area A with (nolock) ON A.idArea= I.idArea
            //                 where A.baja=0 and I.baja=0  and A.idtipoServicio in (1, 3) AND (I.tipo= 'P') order by I.nombre ";

            string m_ssql = @"SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, IE.disponible 
                         FROM Lab_item I  with (nolock) 
                         inner join lab_itemEfector IE  with (nolock) on I.idItem= IE.idItem and Ie.idefector=" + oC.IdEfector.IdEfector.ToString() + //MultiEfector 
                         @"INNER JOIN Lab_area A  (nolock) ON A.idArea= I.idArea                          
                          where A.baja=0 and I.baja=0 and IE.disponible=1  and A.idtipoServicio in (1, 3) AND (I.tipo= 'P') order by I.nombre ";
            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, conn);
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
            m_ssql = "SELECT idRutina, nombre FROM Lab_Rutina with (nolock) where baja=0 and idTipoServicio in ( 1,3) and idEfector=" + oUser.IdEfector.IdEfector.ToString() +" order by nombre ";
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
                {
                    if (Request["Operacion"].ToString() == "Alta")
                    {

                        

                        if (ddlImpresoraEtiqueta.SelectedValue != "0")
                        //   oRegistro.ImprimirCodigoBarras(ddlImpresoraEtiqueta.SelectedItem.Text, int.Parse(Session["idUsuario"].ToString()));
                        {
                            ///Imprimir codigo de barras.
                            string s_AreasCodigosBarras = oRegistro.getListaAreasCodigoBarras();
                            if (s_AreasCodigosBarras != "")
                            {
                              
                               ImprimirCodigoBarrasAreas(oRegistro, s_AreasCodigosBarras, ddlImpresoraEtiqueta.SelectedItem.Text);
                            }
                            
                        }

                        Response.Redirect("ProtocoloMensaje.aspx?id=" + oRegistro.IdProtocolo, false);
                        //////////////////////////
                    }



                    if (Request["Operacion"].ToString() == "Modifica")
                    {
                        switch (Request["Desde"].ToString())
                        {
                            case "ProtocoloList": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=ListaProducto"); break;
                            case "Control": Avanzar(1); break;
                        }
                    }

                    if (Request["Operacion"].ToString() == "AltaDerivacionMultiEfectorLote")
                    {
                        Business.Data.Laboratorio.Protocolo oRegistroAnterior = new Business.Data.Laboratorio.Protocolo();
                        oRegistroAnterior = (Business.Data.Laboratorio.Protocolo) oRegistroAnterior.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                        ActualizarEstadoDerivacion(oRegistro,oRegistroAnterior);
                        VerificacionEstadoLote(oRegistro, oRegistroAnterior);
                        Response.Redirect("ProtocoloMensaje.aspx?id=" + oRegistro.IdProtocolo + "&idLote=" + Request["idLote"] + "&idEfectorSolicitante=" + Request["idEfectorSolicitante"], false);
                    }
                }
                else
                {
                    Response.Redirect("ProtocoloMensaje.aspx?error=1");
                }
            }
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
     VALUES   ( " + oProt.IdProtocolo.ToString() + "," + oUser.IdEfector.IdEfector.ToString() + "," + s_area + ",0,'" + impresora + "' , getdate()    )";
                SqlCommand cmd = new SqlCommand(query, conn);


                int idres = Convert.ToInt32(cmd.ExecuteScalar());
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
            //        string reg_NumeroDocumento = oProt.IdPaciente.NumeroDocumento.ToString();// item[8].ToString();

            //        string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);

            //        string reg_apellido = "";
            //        //if (chkCodificaPaciente.Checked)
            //        //{
            //        //    reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
            //        //}
            //        //else
            //        //{
            //        //    //if (reg_codificaHIV == "FALSE")
            //        //    //    reg_apellido = oProt.IdPaciente.Apellido + " " + oProt.GetPaciente().Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
            //        //    //else
            //        //    //    reg_apellido = oProt.Sexo + oProt.GetPaciente().Nombre.Substring(0, 2) + oProt.GetPaciente().Apellido.Substring(0, 2) + oProt.GetPaciente().FechaNacimiento.ToShortDateString().Replace("/", "");
            //        //}
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
            //        if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);

            //        ticket.AddHeaderLine(reg_apellido.ToUpper());
            //        //ticket.AddSubHeaderLine(reg_apellido.ToUpper());
            //        ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha);
            //        ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);// reg_Sector);
            //        ticket.AddSubHeaderLineNegrita(reg_area);
            //        //ticket.AddSubHeaderLine(reg_area);

            //        ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
            //        ticket.AddFooterLine(reg_numero); // + "  " + reg_NumeroOrigen);

            //        Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;
            //        ticket.PrintTicket(ddlImpresoraEtiqueta.SelectedValue, oConBarra.Fuente);
            //        /////fin de impresion de archivos
            //    }

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
            oPeticion.IdProtocolo = oRegistro.IdProtocolo;
            oPeticion.Save();
        }
        private bool Guardar(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            bool guardo = false;
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

             

                oRegistro.IdEfector = oC.IdEfector;
                SectorServicio oSector = new SectorServicio();
                oSector = (SectorServicio)oSector.Get(typeof(SectorServicio), int.Parse(ddlSectorServicio.SelectedValue));
                oRegistro.IdSector = oSector;

                oRegistro.FechaInicioSintomas = DateTime.Parse("01/01/1900");
                oRegistro.FechaUltimoContacto = DateTime.Parse("01/01/1900");

                if (Request["Operacion"].ToString() != "Modifica")
                {
                    TipoServicio oServicio = new TipoServicio();
                    oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), 5);
                    oRegistro.IdTipoServicio = oServicio;

                    oRegistro.Numero = 0; // oRegistro.GenerarNumero();
                    //oRegistro.NumeroDiario = oRegistro.GenerarNumeroDiario(fecha.ToString("yyyyMMdd"));
                    //oRegistro.PrefijoSector = oSector.Prefijo.Trim();
                    //oRegistro.NumeroSector = oRegistro.GenerarNumeroGrupo(oSector);
                    //oRegistro.NumeroTipoServicio = oRegistro.GenerarNumeroTipoServicio(oServicio);

                    oRegistro.IdPaciente = (Paciente)oPaciente.Get(typeof(Paciente), -1);
                    oRegistro.Edad = -1;
                    oRegistro.UnidadEdad = -1;
                    oRegistro.Sexo = "I";
                    oRegistro.Embarazada = "N";

                    oRegistro.IdObraSocial = (ObraSocial)oObra.Get(typeof(ObraSocial), -1);
                    oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), 4);
                    oRegistro.IdPrioridad = (Prioridad)oPri.Get(typeof(Prioridad), 1);
                    oRegistro.IdEspecialistaSolicitante = 0;

                    
                }


                oRegistro.Fecha = DateTime.Parse(txtFecha.Value);
                oRegistro.FechaOrden = DateTime.Parse(txtFechaOrden.Value);
                oRegistro.FechaRetiro = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);
                oRegistro.FechaTomaMuestra = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);

                oRegistro.IdEfectorSolicitante = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                oRegistro.Notificarresultado = false;


                              
                
                //if (txtNumeroOrigen.Text != "")
                    oRegistro.NumeroOrigen = txtNumeroOrigen.Text;

                
                
                
                oRegistro.Observacion = txtObservacion.Text;
                oRegistro.ObservacionResultado = "";
                oRegistro.IdMuestra = int.Parse(ddlMuestra.SelectedValue);
                oRegistro.IdConservacion= int.Parse(ddlConservacion.SelectedValue); 
                oRegistro.DescripcionProducto = txtDescripcionProducto.Text;
                if (Request["Operacion"].ToString() != "Modifica")
                {
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;
                    AlmacenarSesion(oC);
                }

                oRegistro.Save();

                oRegistro.ActualizarNumeroDesdeID();


                GuardarDetalle(oRegistro);
                
                this.IncidenciaEdit1.GuardarProtocoloIncidencia(oRegistro);
                //if (Request["Operacion"].ToString() != "Modifica")
                //{
                    
                //    oRegistro.VerificarExisteNumeroAsignado();
                //}

                oRegistro.GrabarAuditoriaProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                guardo = true;

            }
            else
            { //doble submit
                guardo = false;
            }

            return guardo;
        }

        

        private void AlmacenarSesion(Configuracion oC)
        {
            
                
            string s_valores = "chkRecordarPractica:" + chkRecordarPractica.Checked;

            if (chkRecordarPractica.Checked)

            {
                //s_valores += "@chkAreaCodigoBarra:" + getListaAreasCodigoBarras();

                //s_valores += "@chkRecordarConfiguracion:" + chkRecordarConfiguracion.Checked;

                 s_valores += "@ddlImpresoraEtiqueta:" + ddlImpresoraEtiqueta.SelectedValue;


                //Session["Etiquetadora"] = ddlImpresoraEtiqueta.SelectedValue;



                s_valores += "@ddlEfector:" + ddlEfector.SelectedValue;
                s_valores += "@ddlConservacion:" + ddlConservacion.SelectedValue;
                if (ddlMuestra.SelectedValue != "0") s_valores += "@ddlMuestra:" + ddlMuestra.SelectedValue;



                s_valores += "@ddlSectorServicio:" + ddlSectorServicio.SelectedValue;
                Session["ProtocoloNoPaciente"] = s_valores;
            }
            else
                Session["ProtocoloNoPaciente"] = null;



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

            /////Busca en la lista de diagnosticos buscados
            //if (lstDiagnosticosFinal.Items.Count > 0)
            //{             
            //    /////Crea nuevamente los detalles.
            //    for (int i = 0; i < lstDiagnosticosFinal.Items.Count; i++)
            //    {
            //        ProtocoloDiagnostico oDetalle = new ProtocoloDiagnostico();
            //        oDetalle.IdProtocolo = oRegistro;
            //        oDetalle.IdEfector = oRegistro.IdEfector;
            //        oDetalle.IdDiagnostico = int.Parse(lstDiagnosticosFinal.Items[i].Value);
            //        oDetalle.Save();                    
            //    }
            //}

        
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
                        oDetalle.Informable = oItem.GetInformableEfector(oUser.IdEfector);


                        GuardarDetallePractica(oDetalle);
                        GuardarDerivacion(oDetalle);
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

            if (Request["Operacion"].ToString() != "Modifica")
            {
            

                if (chkRecordarPractica.Checked)
                    Session["ProtocoloNoPaciente"] += "@practicas:" + recordar_practicas;
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
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()));
                        }
                    }
                }
            }


            oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);


            oRegistro.Save();


        }

        private void GuardarDerivacion(DetalleProtocolo oDetalle)
        {
            if (oDetalle.IdItem.esDerivado(oC.IdEfector))
            {
                Business.Data.Laboratorio.Derivacion oRegistro = new Business.Data.Laboratorio.Derivacion();
                oRegistro.IdDetalleProtocolo = oDetalle;
                oRegistro.Estado = 0;
                oRegistro.Observacion = txtObservacion.Text;
                oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.FechaResultado = DateTime.Parse("01/01/1900");

                oRegistro.IdEfectorDerivacion = oDetalle.IdItem.GetIDEfectorDerivacion(oC.IdEfector);  // se graba el efector configurado en ese momento.

                oRegistro.Save();

                // graba el resultado en ResultadCar   "Derivado: " + oItem.GetEfectorDerivacion(oCon.IdEfector);
                //oDetalle.ResultadoCar = "Pendiente de Derivacion";//"se podria poner a que efector....         
                //oDetalle.Save();
                oDetalle.GrabarAuditoriaDetalleProtocolo("Graba Derivado", oUser.IdUsuario);
            }

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
            if (oDet.VerificarSiEsDerivable(oUser.IdEfector)) //Cambio para que quede como ProtocoloEdit2
            {
                oDet.IdSubItem = oDet.IdItem;
                oDet.Save();
            }
            else
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                crit.Add(Expression.Eq("IdItemPractica", oDet.IdItem));
                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
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
                                oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");

                                oDetalle.Save();

                                GuardarValorReferencia(oDetalle);
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
                    GuardarValorReferencia(oDet);
                }//fin   if (detalle.Count > 0)  
            }

         
            
        }
        private void GuardarValorReferencia(DetalleProtocolo oDetalle)
        {

            string s_unidadMedida = ""; //string s_metodo = "";
            //Item oItem = new Item();
            //oItem = (Item)oItem.Get(typeof(Item), int.Parse(m_idItem));
            if (oDetalle.IdSubItem.IdUnidadMedida > 0)
            {
                UnidadMedida oUnidad = new UnidadMedida();
                oUnidad = (UnidadMedida)oUnidad.Get(typeof(UnidadMedida), oDetalle.IdSubItem.IdUnidadMedida);
                if (oUnidad != null) s_unidadMedida = oUnidad.Nombre;

            }

            ///Calculo de valor de referencia al momento de generar el registro
            /// 

            string m_metodo = "";
            string m_valorReferencia = "";
            string valorRef = oDetalle.CalcularValoresReferencia_NoPaciente();
            if (valorRef != "")
            {
                string[] arr = valorRef.Split(("|").ToCharArray());
                switch (arr.Length)
                {
                    case 1: m_valorReferencia = arr[0].ToString(); break;
                    case 2:
                        {
                            m_valorReferencia = arr[0].ToString();
                            m_metodo = arr[1].ToString();
                        }
                        break;
                }

            }
            oDetalle.UnidadMedida = s_unidadMedida;
            oDetalle.Metodo = m_metodo;
            oDetalle.ValorReferencia = m_valorReferencia;
            oDetalle.Save();
            //Fin calculo de valor de refrencia y metodo
        }

        //private bool VerificarSiEsDerivable(DetalleProtocolo oDet)
        //{
        //    bool ok = false;
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

      

        


       

        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {
         
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            switch (Request["Desde"].ToString())
            {
                case "ProtocoloList": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=ListaProducto"); break;
                case "Control": Response.Redirect("ProtocoloList.aspx?idServicio=" + Session["idServicio"].ToString() + "&Tipo=Control"); break;
                case "AltaDerivacionMultiEfectorLote": Response.Redirect("DerivacionMultiEfectorLote.aspx?idEfectorSolicitante=" + Request["idEfectorSolicitante"].ToString() + "&idServicio=1&idLote=" + Request["idLote"]); break;
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

     //       SelectedEfector();
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

        




        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

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

            if (!VerificarAnalisisContenidos())
            {  TxtDatos.Value = "";
                args.IsValid = false;
             
                return;
            }
            else
            {
                if ((TxtDatos.Value == "") || (TxtDatos.Value == "1###on@"))
                {

                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe completar al menos un análisis";
                    return;
                }
                else args.IsValid = true;



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

                            //PracticaDeterminacion oGrupo = new PracticaDeterminacion();
                            //oGrupo = (PracticaDeterminacion)oGrupo.Get(typeof(PracticaDeterminacion), "IdItemPractica", oItem, "IdItemDeterminacion", oItem2.IdItem);


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
                          " and PD.idEfector= " + oUser.IdEfector.IdEfector.ToString() + " ORDER BY PD.idItemDeterminacion ";


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




        protected void lnkReimprimirCodigoBarras_Click(object sender, EventArgs e)
        {
            lblMensajeImpresion.Text = "Se ha enviado la impresión.";
            if (ddlImpresora2.SelectedIndex > 0)
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
                    Response.Redirect("ProtocoloProductoEdit.aspx?Desde=" + Request["Desde"].ToString()+"&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
                //else
                //    Response.Redirect("ProtocoloEdit2.aspx?Desde="+Request["Desde"].ToString()+"&idServicio=" + Session["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloNuevo + m_parametro);
            }
            else
                Response.Redirect("ProtocoloProductoEdit.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + ProtocoloActual + m_parametro);                                                               
               
        }



        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
              if (e.CommandName== "Modificar")
                  Response.Redirect("ProtocoloProductoEdit.aspx?Desde=" + Request["Desde"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&Operacion=Modifica&idProtocolo=" + e.CommandArgument.ToString());
            

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

        protected void btnArchivos_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProtocoloAdjuntar.aspx?idProtocolo=" + Request["idProtocolo"].ToString() +"&desde=protocolo");
        }

        private void CargarProtocoloDerivadoLote()
        {
            string idProtocolo = Request["idProtocolo"].ToString();
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo),  int.Parse(idProtocolo));
            if (oRegistro != null)
            {
                lblTitulo.Visible = false;
                lblServicio1.Visible = true;
                lblServicio.Visible = true;
                txtFecha.Value = DateTime.Now.ToShortDateString();
                txtFechaOrden.Value = oRegistro.FechaOrden.ToShortDateString();
                txtCodigoMuestra.Text = "";
                txtDescripcionProducto.Text = oRegistro.DescripcionProducto;
                ddlConservacion.SelectedValue = oRegistro.IdConservacion.ToString();
                txtNumeroOrigen.Text = oRegistro.Numero.ToString();
                ddlEfector.SelectedValue = oRegistro.IdEfector.IdEfector.ToString();
                ddlSectorServicio.SelectedValue = oRegistro.IdSector.IdSectorServicio.ToString();
                txtObservacion.Text = oRegistro.Observacion;
                pnlNavegacion.Visible = false;
                btnCancelar.Text = "Cancelar";
                btnCancelar.Width = Unit.Pixel(80);
                ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
               

                if(!oRegistro.Baja)
                {
                    Business.Data.Laboratorio.Derivacion oDerivacion = new Business.Data.Laboratorio.Derivacion();
                    //Obtengo analisis
                    string analisis = oDerivacion.ObtenerItemsPendientes(Request["idLote"].ToString(), oRegistro.IdProtocolo.ToString());
                    CargarDeterminacionesDerivacion(analisis);
                }
               
            }
        }


        private void ActualizarEstadoDerivacion(Protocolo oRegistro, Protocolo oAnterior)
        {
            Business.Data.Laboratorio.Derivacion oDerivacion = new Business.Data.Laboratorio.Derivacion();
            oDerivacion.MarcarComoRecibidas(oAnterior, oRegistro, oUser, Convert.ToInt32(Request["idLote"]));
        }

        private void VerificacionEstadoLote(Protocolo oRegistro, Protocolo oAnterior)
        {
            if (Request["idLote"] != null) //Si no tiene Lote, no actualiza estado de Lote
            {
                int idLote = Convert.ToInt32(Request["idLote"]);
                LoteDerivacion lote = new LoteDerivacion();
                lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), idLote);
                lote.ActualizaEstadoLote(oUser.IdUsuario, oRegistro.Numero.ToString(), oAnterior.Numero.ToString());
            }
        }
    }
}

