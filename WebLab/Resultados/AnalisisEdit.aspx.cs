using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Configuration;
using Business.Data;

namespace WebLab.Resultados
{
    public partial class AnalisisEdit : System.Web.UI.Page
    {
        private Random
     random = new Random();

        private static int
            TEST = 0;

        //private bool IsTokenValid()
        //{
        //    bool result = double.Parse(hidToken.Value) == ((double)Session["NextToken"]);
        //    SetToken();
        //    return result;
        //}

        //private void SetToken()
        //{
        //    double next = random.Next();
        //    hidToken.Value = next + "";
        //    Session["NextToken"] = next;
        //}
        Usuario oUser = new Usuario();

        Protocolo oProtocolo = new Protocolo();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuarioValida"] != null)
            {
               
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
                //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuarioValida"] != null)
            {
                if (!Page.IsPostBack)
                {
                    //SetToken();
                    Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                    oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

                    if (oRegistro.IdTipoServicio.IdTipoServicio == 5)

                        lblProtocolo.Text = oRegistro.Numero.ToString() + " " + oRegistro.getMuestra();
                    else

                        lblProtocolo.Text = oRegistro.Numero.ToString() + " " + oRegistro.IdPaciente.Apellido + " " + oRegistro.IdPaciente.Nombre;

                    CargarListas(oRegistro);
                    MuestraDatos();

                }  
                
            }
            else
            {
                //Si se perdio la sesion le pida a la pagina padre que se redirija al login
                string script = "window.top.location.href = '../FinSesion.aspx';"; 
                ScriptManager.RegisterStartupScript(this, GetType(), "redirigirLogin", script, true);
                return;
            }
        }

        private void MuestraDatos()
        {
            //Actualiza los datos de los objetos : alta o modificacion .
        
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
          
            ddlMuestra.SelectedValue = oRegistro.IdMuestra.ToString();
         
            CargarItems(oRegistro);
         
         
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

            //TxtDatos.Value = sDatos;


        }
        

        protected void txtCodigoMuestra_TextChanged(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
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
                    while (ex != null)
                    {
                        exception = ex.Message + "<br>";

                    }
                }
            }
        }

        protected void ddlMuestra_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
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

        private void CargarListas(Protocolo oRegistro)
        {
            Utility oUtil = new Utility();
            ////Solo cargar las listas deplegables con las determinaciones disponibles en este momento.
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string str_condicion = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
            if (oRegistro.IdTipoServicio.IdTipoServicio == 5)
                str_condicion = "1,3";

            ////Caro mejorar el tiposervicio=5
            string m_ssql = "SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre " +                       
                        " FROM Lab_item I with (nolock) " +
                        " inner join lab_itemEfector IE  with (nolock) on I.idItem= IE.idItem and Ie.idefector=" + oRegistro.IdEfector.IdEfector.ToString() + //MultiEfector 
                        " INNER JOIN Lab_area A  (nolock) ON A.idArea= I.idArea " +
                        " where A.baja=0 and I.baja=0 and IE.disponible=1 " +
                        " and  A.idtipoServicio in ("+str_condicion+") AND (I.tipo= 'P') order by I.nombre ";
            //if (oRegistro.IdTipoServicio.IdTipoServicio==5)
            //    m_ssql = "SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre " +
            //      " FROM Lab_item I  with (nolock)  " +
            //      " INNER JOIN Lab_area A with (nolock) ON A.idArea= I.idArea " +
            //      " where A.baja=0 and I.baja=0 and  I.disponible=1 and A.idtipoServicio (1,3) AND (I.tipo= 'P') order by I.nombre ";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);

            //////Caro, ver por que hay un cargarlistas para llenarddlitem

            /////Carga de determinaciones para la 
            //CargarItems(oRegistro);

            //CargarDiagnosticosFrecuentes();

            //Carga de combo de rutinas
            ///Correccion multiEfector
            m_ssql = @"SELECT idRutina, nombre FROM Lab_Rutina with (nolock) where baja=0 and IdEfector= " + oRegistro.IdEfector.IdEfector.ToString() +
                @" and idTipoServicio= " + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + " order by nombre ";
            if (oRegistro.IdTipoServicio.IdTipoServicio == 5)
                m_ssql = "SELECT idRutina, nombre FROM Lab_Rutina with (nolock) where baja=0 and IdEfector= " + oRegistro.IdEfector.IdEfector.ToString() + " and idTipoServicio in (1, 3) order by nombre ";

            oUtil.CargarCombo(ddlRutina, m_ssql, "idRutina", "nombre", connReady);
            ddlRutina.Items.Insert(0, new ListItem("Seleccione una rutina", "0"));

            //ddlItem.UpdateAfterCallBack = true;
            //ddlRutina.UpdateAfterCallBack = true;


            if ((oRegistro.IdTipoServicio.IdTipoServicio == 3) || (oRegistro.IdTipoServicio.IdTipoServicio == 5))
            {
                ////////////Carga de combos de Muestras
                pnlMuestra.Visible = true;
                m_ssql = "SELECT idMuestra, nombre + ' - ' + codigo as nombre FROM LAB_Muestra with (nolock)   order by nombre ";
                oUtil.CargarCombo(ddlMuestra, m_ssql, "idMuestra", "nombre", connReady);
                ////////////////7
            }
            m_ssql = null;
            oUtil = null;
        }



        private void CargarItems(Protocolo oRegistro)
        {

            Utility oUtil = new Utility();
            string str_condicion = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
            if (oRegistro.IdTipoServicio.IdTipoServicio == 5)
                str_condicion = "1,3";


            /////Cargar las determinaciones disponibles ahora y las que estan en el protocolo
            string m_ssql = @"SELECT I.idItem as idItem, I.codigo as codigo, I.nombre as nombre, I.nombre + ' - ' + I.codigo as nombreLargo, IE.disponible 
                            FROM Lab_item I with (nolock) 
                            inner join lab_itemEfector IE  with (nolock) on I.idItem= IE.idItem and Ie.idefector=" + oRegistro.IdEfector.IdEfector.ToString() + 
                            @"INNER JOIN Lab_area A  (nolock) ON A.idArea= I.idArea 
                            where (A.baja=0 and I.baja=0 and IE.disponible=1  and  A.idtipoServicio in (" + str_condicion + @") AND (I.tipo= 'P') )
                            or exists(select 1 from LAB_DetalleProtocolo p WHERE p.iditem = I.iditem and idProtocolo = "+oRegistro.IdProtocolo.ToString()+")  order by I.nombre ";            
            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter da = new SqlDataAdapter(m_ssql,  conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

            ///Caro: no hace falta cargar ddlitem; ya se cargó en CargarLists()
            ///ddlItem.Items.Insert(0, new ListItem("", "0"));

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
            if (Session["idUsuario"] != null)
            {
                if (Page.IsValid)
                { ///Verifica si se trata de un alta o modificacion de protocolo               
                    Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                    oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                    if (oRegistro != null)
                    {
                        Guardar(oRegistro);
                    }
                    Response.Redirect("AnalisisEdit.aspx?idProtocolo=" + oRegistro.IdProtocolo.ToString(), false);
                }
            }

        }

    
     


      


     

        private void Guardar(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            //if (IsTokenValid())
            //{
            //    TEST++;

                GuardarDetalle(oRegistro);
            //}
           // oRegistro.GrabarAuditoriaProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
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
                        
                        // GuardarDerivacion(oDetalle);
                        //oDetalle.GuardarDerivacion(oUser);
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
            

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oRegistro.IdEfector);

            //if (oCon.TipoCalculoDiasRetiro == 0)

            if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
                if (oCon.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(dias_espera));
                else   // calcula con los días predeterminados de espera
                    oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oCon.DiasRetiro));
            else
                oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);

            //Para casos de modificacion, si cambio el tipo de Muestra que tambien contemple el Servicio 5
            if( oRegistro.IdTipoServicio.IdTipoServicio==3 || oRegistro.IdTipoServicio.IdTipoServicio == 5) oRegistro.IdMuestra = int.Parse(ddlMuestra.SelectedValue);
            oRegistro.Save();


        }
        //private void GuardarDetalle2(Business.Data.Laboratorio.Protocolo oRegistro)
        //{
        //    ///Eliminar los detalles para volverlos a crear            
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
        //    crit.Add(Expression.Eq("IdProtocolo", oRegistro));
        //    IList detalle = crit.List();
        //    if (detalle.Count > 0)
        //    {
        //        foreach (DetalleProtocolo oDetalle in detalle)
        //        {
        //            oDetalle.Delete();
        //        }
        //    }


        //    int dias_espera = 0;
        //    string[] tabla = TxtDatos.Value.Split('@');

        //    for (int i = 0; i < tabla.Length - 1; i++)
        //    {
        //        string[] fila = tabla[i].Split('#');


        //        string codigo = fila[1].ToString();
        //        if (codigo != "")
        //        {
        //            DetalleProtocolo oDetalle = new DetalleProtocolo();
        //            Item oItem = new Item();
        //            oDetalle.IdProtocolo = oRegistro;
        //            oDetalle.IdEfector = oRegistro.IdEfector;

        //            string trajomuestra = fila[3].ToString();

        //            oDetalle.IdItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo);

        //            if (dias_espera < oDetalle.IdItem.Duracion) dias_espera = oDetalle.IdItem.Duracion;

        //            /*CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
        //            if (a.Checked)
        //                oDetalle.TrajoMuestra = "Si";
        //            else*/

        //            if (trajomuestra == "true")
        //                oDetalle.TrajoMuestra = "No";
        //            else
        //                oDetalle.TrajoMuestra = "Si";


        //            oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
        //            oDetalle.FechaValida = DateTime.Parse("01/01/1900");
        //            oDetalle.FechaControl = DateTime.Parse("01/01/1900");
        //            oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
        //            oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
        //            oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
        //            oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
        //            GuardarDetallePractica(oDetalle);
        //        }
        //    }


        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oRegistro.IdEfector);
        //  //  DateTime fechaentrega;
        //    //if (oCon.TipoCalculoDiasRetiro == 0)

        //    if (oRegistro.IdOrigen.IdOrigen == 1) /// Solo calcula con Calendario si es Externo
        //        if (oCon.TipoCalculoDiasRetiro == 0)  //Calcula con los días de espera del analisis
        //            oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(dias_espera));
        //        else   // calcula con los días predeterminados de espera
        //            oRegistro.FechaRetiro = oRegistro.CalcularCalendarioEntrega(oRegistro.Fecha.AddDays(oCon.DiasRetiro));
        //    else
        //        oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(dias_espera);




        //    oRegistro.Save();


        //}



        private void GuardarDetallePractica(DetalleProtocolo oDet)
        {

            if (oDet.VerificarSiEsDerivable(oDet.IdProtocolo.IdEfector)) //oDet.IdItem.IdEfector.IdEfector != oDet.IdItem.IdEfectorDerivacion.IdEfector) //Si es un item derivable no busca hijos y guarda directamente.
            {
                oDet.IdSubItem = oDet.IdItem;
                oDet.Save();
                oDet.GuardarValorReferencia();
                //Guarda Derivacion              
                oDet.GuardarDerivacion(oUser);

            }
            else
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                crit.Add(Expression.Eq("IdItemPractica", oDet.IdItem));
                crit.Add(Expression.Eq("IdEfector", oDet.IdProtocolo.IdEfector));
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
                                //oDet.IdSubItem = oSItem;
                                //oDet.Save();      
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
                                    oDetalle.GuardarSinInsumo();
                                    oDetalle.GuardarValorReferencia();
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
                    
                }//fin   if (detalle.Count > 0)  
            }

        }

        

        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Si el sexo es femenino se habilita la selecció de Embarazada
            // HabilitarEmbarazada();
        }



        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
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
        }

        private void AgregarRutina()
        {
            Rutina oRutina = new Rutina();
            oRutina = (Rutina)oRutina.Get(typeof(Rutina), int.Parse(ddlRutina.SelectedValue));

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleRutina));
            crit.Add(Expression.Eq("IdRutina", oRutina));
            crit.AddOrder(Order.Asc("IdDetalleRutina"));


            IList detalle = crit.List();
           
                string codigos = "";
                foreach (DetalleRutina oDetalle in detalle)
                {

                    if (codigos == "")
                        codigos = oDetalle.IdItem.Codigo;
                    else
                        codigos += ";" + oDetalle.IdItem.Codigo;                    
                }
                txtCodigosRutina.Text = codigos;
                txtCodigosRutina.UpdateAfterCallBack = true;

            

        }



        protected void ddlRutina_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Session["idUsuario"] != null)
                if (ddlRutina.SelectedValue != "0")
                     AgregarRutina();
        }


        protected void cvValidacionInput_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Session["idUsuario"] != null) 
            {
                TxtDatosCargados.Value = TxtDatos.Value;

                string sDatos = "";

                string[] tabla = TxtDatos.Value.Split('@');

                for (int i = 0; i < tabla.Length - 1; i++)
                {
                    string[] fila = tabla[i].Split('#');
                    string codigo = fila[1].ToString();
                    string muestra = fila[2].ToString();

                    if (sDatos == "")
                        sDatos = codigo + "#" + muestra;
                    else
                        sDatos += ";" + codigo + "#" + muestra;

                }

                TxtDatosCargados.Value = sDatos;

                if (!VerificarAnalisisContenidos())
                {
                    TxtDatos.Value = "";
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
                }
            }
               
        }

        private bool VerificarAnalisisContenidos()
        {
            bool devolver = true;
            try
            {
                Business.Data.Laboratorio.Protocolo oProtocolo = new Business.Data.Laboratorio.Protocolo();
                oProtocolo = (Business.Data.Laboratorio.Protocolo)oProtocolo.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

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

                                ISession m_session = NHibernateHttpModule.CurrentSession;
                                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                                crit.Add(Expression.Eq("IdItemPractica", oItem));
                                crit.Add(Expression.Eq("IdItemDeterminacion", oItem2.IdItem));
                                crit.Add(Expression.Eq("IdEfector", oProtocolo.IdEfector));
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

            }
            catch (Exception error)
            {
                this.cvValidacionInput.ErrorMessage = "Ha ocurrido un error. Verifique con el administrador del sistema";
                devolver = false;
            }
            return devolver;

        }

        private bool VerificarAnalisisComplejosContenidos(string listaCodigo)
        { ///Este es un segundo nivel de validacion en donde los analisis contenidos no estan directamente sino en diagramas
            Business.Data.Laboratorio.Protocolo oProtocolo = new Business.Data.Laboratorio.Protocolo();
            oProtocolo = (Business.Data.Laboratorio.Protocolo)oProtocolo.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

            bool devolver = true;
            string m_ssql = "SELECT  distinct PD.idItemDeterminacion, I.codigo" +
                            " FROM         LAB_PracticaDeterminacion AS PD with (nolock) " +
                            " INNER JOIN   LAB_Item AS I with (nolock) ON PD.idItemPractica = I.idItem " +
                            " WHERE     I.codigo IN (" + listaCodigo + ") AND (I.baja = 0)" +
                            " and PD.idEfector= "+ oProtocolo.IdEfector.IdEfector.ToString()+" ORDER BY PD.idItemDeterminacion ";

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
        

    
         


         

    }
}

