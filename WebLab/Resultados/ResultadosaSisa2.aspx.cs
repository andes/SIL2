 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.Configuration;
using Business.Data;
using System.Web.UI.HtmlControls;

namespace WebLab.Resultados
{
    public partial class ResultadosaSisa2 : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            
            if (Session["idUsuario"] == null)
                Response.Redirect("../logout.aspx", false);
            else
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oUser != null)
                    oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("../logout.aspx", false);
                else
                {
                    VerificaPermisos("Resultados a SISA");
                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();

                    CargarListas();
                    //CargarGrilla();                   
                }
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
                    case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarGrilla()
        {
            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            string str_condicion = " and C.idEfector=" + ddlEfector.SelectedValue;
            //Item oDet = new Item();
            //oDet = (Item)oDet.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
            //if (oDet != null)
            //{
                if (txtFechaDesde.Value != "")
                {
                    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                //    if (oDet.Informable)
                        str_condicion += " AND d.fechaValida>= '" + fecha1.ToString("yyyyMMdd") + "'";
                  /*  else
                        str_condicion += " AND c.fecha >= '" + fecha1.ToString("yyyyMMdd") + "'";
                        */
                }

                if (ddlResultado.SelectedValue != "0")
                    str_condicion += " AND d.resultadocar= '" + ddlResultado.SelectedValue + "'";

             

                cmd.CommandText = "[LAB_ResultadosASisa]";

                cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
                cmd.Parameters["@FiltroBusqueda"].Value = str_condicion;

                cmd.Parameters.Add("@idItem", SqlDbType.Int);
                cmd.Parameters["@idItem"].Value = ddlItem.SelectedValue;

                cmd.Parameters.Add("@Estado", SqlDbType.Int);
                cmd.Parameters["@Estado"].Value = rdbEstado.SelectedValue;

            cmd.Parameters.Add("@agrupado", SqlDbType.Bit);
            cmd.Parameters["@agrupado"].Value = 0;//a no grupado: lista de procotocolos


            cmd.Connection = conn;


                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(Ds);



                gvLista.DataSource = Ds.Tables[0];
                gvLista.DataBind();
                lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
                conn.Close();
            //}
        }
        

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            //PintarReferencias();
        }
 

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            //PintarReferencias();
        }

        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }
         
        private void BorrarResultadosTemporales()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_Temp_ResultadoSISA] where idusuario=" + Session["idUsuario"].ToString();
            SqlCommand cmd = new SqlCommand(query, conn);


            int idres = Convert.ToInt32(cmd.ExecuteScalar());

        }


        private void BorrarLineaTemporales(string idderivacion, string ideventomuestra)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_Temp_ResultadoSISA] where [idDerivacion]=" + idderivacion+" and [idEventoMuestra]=" + ideventomuestra;
            SqlCommand cmd = new SqlCommand(query, conn);

         



            int idres = Convert.ToInt32(cmd.ExecuteScalar());

        }
        private void BorrarLineaTemporalesdesdeAPI(string ideventocaso)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_Temp_ResultadoSISA] where [idEvento]=" + ideventocaso;
            SqlCommand cmd = new SqlCommand(query, conn);





            int idres = Convert.ToInt32(cmd.ExecuteScalar());

        }




       


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
           
                SubirSISAdesdeAPI();

        }

        private bool ProcesaSISA(DetalleProtocolo oDetalle )
        {
            bool generacaso = false;

            try
            {
                
                if (oDetalle.IdProtocolo.IdCasoSISA == 0)                             
                     GenerarCasoSISA_V2(oDetalle);                                

                if ((oDetalle.IdProtocolo.IdCasoSISA > 0) && (oDetalle.IdeventomuestraSISA == 0))
                {
                    bool existe = ExisteTipoMuestra(oDetalle , HdIdMuestra.Value, HdIdTipoMuestra.Value);
                    if (!existe)                    
                        GenerarMuestraSISA(oDetalle.IdProtocolo, oDetalle.IdProtocolo.IdCasoSISA);

                    if (oDetalle.IdeventomuestraSISA > 0)
                        GenerarResultadoSISA(oDetalle);

                }
                BorrarDescartado(oDetalle);
                 
            }
            catch (Exception e)
            {
                generacaso = false;
            }
            return generacaso;

        }

        private void BorrarDescartado(DetalleProtocolo oDetalleProtocolo)
        {

            string m_strSQL = " select * from LAB_DetalleProtocoloExcluidoSISA  where idDetalleProtocolo= " + oDetalleProtocolo.IdDetalleProtocolo.ToString();

        DataSet Ds = new DataSet();
        //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        adapter.Fill(Ds);

                    DataTable dt = Ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                string query = @" delete from LAB_DetalleProtocoloExcluidoSISA where idDetalleProtocolo= " + oDetalleProtocolo.IdDetalleProtocolo.ToString();
                SqlCommand cmd = new SqlCommand(query, conn2);
                int idres = Convert.ToInt32(cmd.ExecuteScalar());
                oDetalleProtocolo.GrabarAuditoriaDetalleProtocolo("Deshacer No informar SISA", oUser.IdUsuario);
            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility(); string m_ssql = ""; string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            if (oUser.IdEfector.IdEfector.ToString() == "227")
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E " +
                     " INNER JOIN lab_Configuracion C on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }



            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            //              m_ssql = @"SELECT distinct i.idItem, i.nombre FROM LAB_item i (nolock)
            //inner join LAB_ConfiguracionSISA c (nolock) on c.idItem = i.idItem
            //order by i.nombre ";

            //            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            //            ddlItem.Items.Insert(0, new ListItem("Seleccione", "0"));

            m_ssql = @"SELECT distinct c.idEvento, c.nombreEvento
                    FROM   LAB_ConfiguracionSISA c with (nolock)  
                    order by c.nombreEvento";

            oUtil.CargarCombo(ddlItem, m_ssql, "idEvento", "nombreEvento", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione", "0"));
            m_ssql = null;
            oUtil = null;
        }
        private bool GenerarCasoSISA(DetalleProtocolo oDetalle, string res)
        {
            /*Version 1*/
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

            bool generacaso = false;
            string caracter = "";
            string idevento = "";
            string nombreevento = "";
            string idclasificacionmanual = "";
            string nombreclasificacionmanual = "";
            string idgrupoevento = "";
            string nombregrupoevento = "";
            bool seguir = false;
            string m_strSQL = "";

            try
            {
                // query levanta todos los que se generan segun el caracter
                m_strSQL = " select * from LAB_ConfiguracionSISA with (nolock) where  idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem= " + oDetalle.IdSubItem.IdItem.ToString();
                // si es contacto se sube==>si es negativo como contacto y si es positivo como sospechoso.
                if ((res == "SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter == 4) && (oC.CodigoCovid == oDetalle.IdSubItem.Codigo))
                {
                    m_strSQL = " select * from LAB_ConfiguracionSISA with (nolock) where idCaracter=1 and idItem= " + oDetalle.IdSubItem.IdItem.ToString();
                }





                DataSet Ds = new DataSet();
                //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dt = Ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    caracter = dt.Rows[i][1].ToString();
                    idevento = dt.Rows[i][2].ToString();
                    HdidEventoSISA.Value = idevento;
                    nombreevento = dt.Rows[i][3].ToString();
                    idclasificacionmanual = dt.Rows[i][4].ToString();
                    nombreclasificacionmanual = dt.Rows[i][5].ToString();
                    idgrupoevento = dt.Rows[i][6].ToString();
                    nombregrupoevento = dt.Rows[i][7].ToString();
                    seguir = true;
                    break;
                }

                if (seguir)
                {

                    string URL = oC.UrlServicioSISA;
                    string s_idestablecimiento = oC.CodigoEstablecimientoSISA; // "14580562167000"
                    string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
                    string[] a = usersisa.Split(':');
                    string s_user = a[0].ToString();
                    string s_userpass = a[1].ToString();

                    string s_sexo = "";
                    switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                    {
                        case 1: s_sexo = "I"; break;
                        case 2: s_sexo = "F"; break;
                        case 3: s_sexo = "M"; break;
                    }
                    string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");
                    string fnpapel = oDetalle.IdProtocolo.FechaOrden.ToShortDateString().Replace("/", "-");
                    string numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                    string error = "";
                    //bool hayerror = false;

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
                        usuario = s_user,
                        clave = s_userpass,
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

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            if (respuesta_d.resultado == "OK")
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                            else // ERROR_DATOS
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));


                        }
                        else
                        {
                            generacaso = false;
                            //hayerror = true;
                            error = respuesta_d.resultado;

                        }
                    }

                }

            }
            //catch
            //{
            //    generacaso = false;
            //    //lblError.Text = "Hubo algun problema al conectar al servicio SISA: " + e.InnerException.InnerException.Message.ToString() + ". Intente de nuevo o haga clic en Salir";
            //    //lblError.Visible = true;
            //    //btnSalir.Visible = true;
            //}
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                generacaso = false;
            }
            return generacaso;

        }


        private void GenerarCasoSISA_V2(DetalleProtocolo oDetalle)
        {
            /*Version 2*/
            /*Cambio para que vuelva a buscar los datos en la base y tome los datos desde la grilla*/
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

         //   bool generacaso = false;
            //string caracter = "";
            //string idevento = "";
            //string nombreevento = "";
            //string idclasificacionmanual = "";
            //string nombreclasificacionmanual = "";
            //string idgrupoevento = "";
            //string nombregrupoevento = "";
            bool seguir = false;
            //string m_strSQL = "";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            try
            {
                //                // query levanta todos los que se generan segun el caracter
                //                m_strSQL = @" select * from LAB_ConfiguracionSISA with (nolock) where  idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem= " + oDetalle.IdSubItem.IdItem.ToString() + " and idEvento=" + ddlItem.SelectedValue;
                //                // si es contacto se sube==>si es negativo como contacto y si es positivo como sospechoso.
                //                if ((res == "SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter == 4) && (oC.CodigoCovid == oDetalle.IdSubItem.Codigo))
                //                {
                //                    m_strSQL = " select * from LAB_ConfiguracionSISA with (nolock) where idCaracter=1 and idItem= " + oDetalle.IdSubItem.IdItem.ToString()+ " and idEvento=" + ddlItem.SelectedValue;
                //                }


                //                m_strSQL += @"  and fechavigenciadesde<=convert(date,convert(varchar,getdate(),112)) 
                //and ( fechavigenciahasta  >convert(date,convert(varchar,getdate(),112)) or convert(varchar, fechavigenciahasta, 103) = '01/01/1900')
                //                                and (idorigen=0 or idOrigen=" +oDetalle.IdProtocolo.IdOrigen.IdOrigen.ToString() +")";

                //                //Control de efector solicitante
                //                //Monitoreo de SARS COV - 2 y OVR en ambulatorios ==> solo aplica para Hospital Heller
                //                //Demas eventos para todos los efectores solicitantes.
                //                m_strSQL += @" and (idefectorsolicitante=0 or 
                //                                idefectorsolicitante in (" + oDetalle.IdProtocolo.IdEfectorSolicitante.IdEfector.ToString() + "))";

                //                //control de embarzada=s /N
                //                m_strSQL += @"  and soloEmbarazada='" + oDetalle.IdProtocolo.Embarazada.ToString()+"'";
                //                ///control de edades
                //                m_strSQL += @" and ("+oDetalle.IdProtocolo.Edad+" between edadDesde and edadHasta and "+oDetalle.IdProtocolo.UnidadEdad +" = 0) ";


                //                DataSet Ds = new DataSet();
                //                //      SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                //                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                //                SqlDataAdapter adapter = new SqlDataAdapter();
                //                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                //                adapter.Fill(Ds);

                //                DataTable dt = Ds.Tables[0];

                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                    caracter = dt.Rows[i][1].ToString();
                //                    idevento = dt.Rows[i][2].ToString();
                //                    HdidEventoSISA.Value = idevento;
                //                    nombreevento = dt.Rows[i][3].ToString();
                //                    idclasificacionmanual = dt.Rows[i][4].ToString();
                //                    nombreclasificacionmanual = dt.Rows[i][5].ToString();
                //                    idgrupoevento = dt.Rows[i][6].ToString();
                //                    nombregrupoevento = dt.Rows[i][7].ToString();
                //                    seguir = true;
                //                    break;
                //                }

              

              //caracter = dt.Rows[i][1].ToString();
              //  idevento = dt.Rows[i][2].ToString();
              //  HdidEventoSISA.Value = idevento;
              //  nombreevento = dt.Rows[i][3].ToString();
              //  idclasificacionmanual = dt.Rows[i][4].ToString();
              //  nombreclasificacionmanual = dt.Rows[i][5].ToString();
              //  idgrupoevento = dt.Rows[i][6].ToString();
              //  nombregrupoevento = dt.Rows[i][7].ToString();

              //  string idmuestra = gvLista.Rows[row.RowIndex].Cells[8].Text;
              //  string idtipomuestra = gvLista.Rows[row.RowIndex].Cells[9].Text;
              //  HdIdMuestra.Value = idmuestra;
              //  HdIdTipoMuestra.Value = idtipomuestra;

              //  HdIdPrueba.Value = gvLista.Rows[row.RowIndex].Cells[10].Text;
              //  HdIdTipoPrueba.Value = gvLista.Rows[row.RowIndex].Cells[11].Text;

              //  HdidResultadoSISA.Value = gvLista.Rows[row.RowIndex].Cells[12].Text;
              //  HidItemSIL.Value = gvLista.Rows[row.RowIndex].Cells[15].Text;


              //  /*nuevos campos recuperados desde la grilla*/
              //  HidCaracter.Value = gvLista.Rows[row.RowIndex].Cells[16].Text;
              //  //idevento = dt.Rows[i][2].ToString();
              //  HdidEventoSISA.Value = gvLista.Rows[row.RowIndex].Cells[13].Text;


                seguir = true;
                //break;
                if (seguir)
                {
                    string conexionServicio = oC.UrlServicioSISA;
                    string[] arr = conexionServicio.Split((";").ToCharArray());


                    if (arr.Length >= 1)
                    {
                        string URL = arr[0].ToString();
                        string s_user = arr[1].ToString();
                        string s_userpass = arr[2].ToString();




                        //string URL = oC.UrlServicioSISA;// "https://ws400-qa.sisa.msal.gov.ar/snvsCasoNominal/v2/snvsCasoNominal"; //oC.UrlServicioSISA;
                        string s_idestablecimiento = oC.CodigoEstablecimientoSISA; // "14580562167000"                    
                                                                                   //string s_user ="e56f25eb"; // "PruebasWSQA_SNVS_ID"; //
                                                                                   //string s_userpass = "64a16ba3bedbae19e9010e3184fa9926"; //"PruebasWSQA_SNVS_KEY"; //

                        string s_sexo = "";
                        switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                        {
                            case 1: s_sexo = "I"; break;
                            case 2: s_sexo = "F"; break;
                            case 3: s_sexo = "M"; break;
                        }
                        string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");
                        string fnpapel = oDetalle.IdProtocolo.FechaOrden.ToShortDateString().Replace("/", "-");
                        string s_numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                        string error = "";
                        string s_apellido = oDetalle.IdProtocolo.IdPaciente.Apellido;
                        if (s_apellido.Length >= 100) s_apellido = s_apellido.Substring(0, 99);
                        string s_nombre = oDetalle.IdProtocolo.IdPaciente.Nombre;
                        if (s_nombre.Length >= 100) s_nombre = s_nombre.Substring(0, 99);


                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(DomicilioPaciente));
                        crit.Add(Expression.Eq("IdPaciente", oDetalle.IdProtocolo.IdPaciente));
                        IList detalle = crit.List();
                        string s_calle = "SIN DATOS";// PARA CORREGIR ERROR QUE DA CUANDO NO SE ENVIAN DATOS: ciudadano.domicilio.calle":"No se debe superar los 200 caracteres.
                        foreach (DomicilioPaciente oDom in detalle)
                        {

                            s_calle = oDom.Calle;
                            if (s_calle.Length >= 200) s_calle = s_calle.Substring(0, 199);
                            if (s_calle.Trim() == "") s_calle = null;
                        }


                        string s_telefono = oDetalle.IdProtocolo.IdPaciente.InformacionContacto;
                        if (s_telefono.Length < 7) s_telefono = "sindatos";
                        if (s_telefono.Length > 15) s_telefono = "sindatos";
                        ///  s_telefono = "sindatos444444444444444444444444444444444444444444";// prueba para forzar error
                        string s_tipodocumento = "1";
                        if (oDetalle.IdProtocolo.IdPaciente.IdEstado == 2)
                        {
                            s_tipodocumento = "3"; //indocumentado
                            s_numerodocumento = "";
                        }
                        string seDeclaraPuebloIndigena = "No";
                        if (oDetalle.IdProtocolo.IdPaciente.SeDeclaraAborigen)
                            seDeclaraPuebloIndigena = "Si";

                        string s_mail = null;
                        if (oDetalle.IdProtocolo.IdPaciente.Mail != "")
                            s_mail = oDetalle.IdProtocolo.IdPaciente.Mail;
                        //bool hayerror = false;
                        domicilio newdomicilio = new domicilio
                        {
                            calle = s_calle,
                            idDepartamento = null,
                            idLocalidad = null,
                            idProvincia = null,
                            idPais = null,


                        };
                        personaACargo newpersonaaCargo = new personaACargo
                        {
                            tipoDocumento = null,
                            numeroDocumento = null,
                            vinculo = null,

                        };
                        ciudadano newciudadano = new ciudadano
                        {
                            apellido = s_apellido,
                            nombre = s_nombre,
                            tipoDocumento = s_tipodocumento, // 1: DNI- 2: PASAPorte - 3: Indocumentado
                            numeroDocumento = s_numerodocumento,
                            fechaNacimiento = fn,
                            sexo = s_sexo,
                            paisEmisionTipoDocumento = "200",
                            seDeclaraPuebloIndigena = seDeclaraPuebloIndigena,
                            domicilio = newdomicilio,
                            telefono = s_telefono,
                            mail = s_mail,
                            personaACargo = newpersonaaCargo


                        };

                        eventoCasoNominal newevento = new eventoCasoNominal
                        {

                            fechaPapel = fnpapel, // "10-12-2019",                        
                            idGrupoEvento = HidGrupoEvento.Value,//idgrupoevento,
                            idEvento = HdidEventoSISA.Value ,///idevento, // "77",                      
                            idClasificacionManualCaso = HidClasificacionManual.Value,/// idclasificacionmanual, // "22"
                            idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                        };

                        AltaCasoV2 caso = new AltaCasoV2
                        {
                            ciudadano = newciudadano,
                            eventoCasoNominal = newevento
                        };

                        string DATA = jsonSerializer.Serialize(caso);

                        byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                        HttpWebRequest request;
                        request = WebRequest.Create(URL) as HttpWebRequest;
                        request.Timeout = 10 * 1000;
                        request.Method = "POST";
                        request.ContentLength = data.Length;
                        request.ContentType = "application/json";
                        request.Headers.Add("APP_ID", s_user);
                        request.Headers.Add("APP_KEY", s_userpass);


                        Stream postStream = request.GetRequestStream();
                        postStream.Write(data, 0, data.Length);

                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string body = reader.ReadToEnd();


                        if (body != "")
                        {
                            //string result = messge.Content.ReadAsStringAsync().Result;
                            //description = result;
                            RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(body);

                            if (respuesta_d.id_caso != "")
                            { //  devolver el idcaso para guardar en la base de datos
                                string s_idcaso = respuesta_d.id_caso;
                                ///grabar a protocolo idCaso
                                //Protocolo protocolo = new Protocolo();
                                //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));

                                oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                                oDetalle.IdProtocolo.Save();
                                if (respuesta_d.resultado == "OK")
                                    oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, oUser.IdUsuario);
                                else // ERROR_DATOS
                                    oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, oUser.IdUsuario);
                            }
                            else
                            {
                             //   generacaso = false;
                                //hayerror = true;
                                error = respuesta_d.resultado;
                            }
                        }
                    }
                }

            }
           
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                grabarLogMensaje(mensaje+ " protocolo: " + oDetalle.IdProtocolo.Numero.ToString());

                RespuestaCaso respuesta_error = jsonSerializer.Deserialize<RespuestaCaso>(mensaje);
                if (respuesta_error.id_caso != "")
                { //  devolver el idcaso para guardar en la base de datos
                    string s_idcaso = respuesta_error.id_caso;
                    ///grabar a protocolo idCaso
                    //Protocolo protocolo = new Protocolo();
                    //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));

                    oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                    oDetalle.IdProtocolo.Save();
                    if (respuesta_error.resultado == "OK")
                        oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                    else // ERROR_DATOS
                        oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                }
                //else
                //generacaso = false;
            }
         //   return generacaso;

        }

        private void grabarLogMensaje(string mensaje)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            mensaje = "Error SISA:" + mensaje;
            string query = @"INSERT INTO [dbo].[Temp_Mensaje]
           ( fecharegistro, mensaje, idEfector)           
     VALUES  (GETDATE()  ,'" + mensaje + "'," + oUser.IdEfector.IdEfector.ToString()+")";
            SqlCommand cmd = new SqlCommand(query, conn);                       

            int mensajegrabado = Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void GenerarMuestraSISA(Protocolo protocolo, int idCasoSISA)

        {
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

            string conexionServicio = oC.URLMuestraSISA;
            string[] arr = conexionServicio.Split((";").ToCharArray());


            if (arr.Length >= 1)
            {
                string URL = arr[0].ToString();
                string s_user = arr[1].ToString();
                string s_userpass = arr[2].ToString();                
             
                    string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    //if (protocolo.FechaTomaMuestra< protocolo.FechaOrden)
                    //    ftoma = protocolo.FechaOrden.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
                    if ((idestablecimientotoma == "") || (idestablecimientotoma == "0"))
                        //pongo por defecto laboratorio central
                        idestablecimientotoma = protocolo.IdEfector.CodigoSISA;


                    ResultadoxNro.EventoMuestra newmuestra = new ResultadoxNro.EventoMuestra
                    {
                        adecuada = true,
                        aislamiento = false,
                        fechaToma = ftoma, // "2020-08-23",
                        idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                        idEventoCaso = idCasoSISA,// protocolo.IdCasoSISA, // 2061287,
                        idMuestra = int.Parse(HdIdMuestra.Value), //272, --covid
                        idtipoMuestra = int.Parse(HdIdTipoMuestra.Value), // 4,--covid
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
                    request.Headers.Add("app_key", s_userpass);// "b0fd61c3a08917cfd20491b24af6049e");
                    request.Headers.Add("app_id", s_user);/// "22891c8f");

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
                                Item oItem = new Item();
                                oItem = (Item)oItem.Get(typeof(Item), int.Parse(HidItemSIL.Value));//se cambia al cambiar filtro por evento y no por determinacion

                                //string trajomuestra = fila[3].ToString();
                                ISession m_session = NHibernateHttpModule.CurrentSession;
                                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                                crit.Add(Expression.Eq("IdProtocolo", protocolo));
                                crit.Add(Expression.Eq("IdSubItem", oItem));
                                IList listadetalle = crit.List();
                                foreach (DetalleProtocolo oDetalle in listadetalle)
                                {
                                    oDetalle.IdeventomuestraSISA = respuesta_d.id;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + respuesta_d.id.ToString(), oUser.IdUsuario);
                                } //for each
                            } //respuesta_o
                        }// body
                    }
                    catch (WebException ex)
                   {
                        string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    grabarLogMensaje("Genera Muestra SISA: "+ mensaje + "- idcasosisa:" + idCasoSISA.ToString());
                    }
                 

            }
        }

        private bool ExisteTipoMuestra(DetalleProtocolo oDetalle, string idmuestra, string idtipomuestra)
        {
            ///Verifica si para la misma muestra sisa ya existe creada se replica el ideventomuestraSISA en todos los detalles del mismo protocolo
            bool existe = false;
            string idevento=HdidEventoSISA.Value;
            string ideventomuestraSISA = "0";
            string m_strSQL = @"select top 1  ideventomuestraSISA 
                                from Lab_detalleprotocolo  
                                where idProtocolo in (select idProtocolo from LAB_Protocolo with (nolock) where idcasosisa=" + oDetalle.IdProtocolo.IdCasoSISA.ToString() + @")                                
                                and ideventomuestraSISA>0
                                and idsubitem in (select iditem from lab_configuracionsisa with (nolock) where idevento="+ idevento + " and idmuestra = " + idmuestra.ToString() + @" 
                                                    and idTipoMuestra = " + idtipomuestra.ToString() + ") order by ideventomuestraSISA desc";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;            
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            DataTable dt = Ds.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                    ideventomuestraSISA = dt.Rows[i][0].ToString();               
                    if ((ideventomuestraSISA != "0") &&  (oDetalle.IdeventomuestraSISA == 0))
                            {
                            existe = true;
                                oDetalle.IdeventomuestraSISA = int.Parse(ideventomuestraSISA);
                                oDetalle.Save();

                                oDetalle.GrabarAuditoriaDetalleProtocolo("Replica Muestra SISA " + ideventomuestraSISA.ToString(), oUser.IdUsuario);
                            }
            }

            return existe;
        }


        private void GenerarResultadoSISA(DetalleProtocolo oDetalle )

        {
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

            int ideventomuestra = oDetalle.IdeventomuestraSISA;
             
            //string URL = oC.URLResultadoSISA;
            string conexionServicio = oC.URLResultadoSISA;

            string[] arr = conexionServicio.Split((";").ToCharArray());


            if (arr.Length >= 1)
            {
                string URL = arr[0].ToString();
                string s_user = arr[1].ToString();
                string s_userpass = arr[2].ToString();


                try
                {


//                    // query levanta todos los que se generan segun el caracter
//                    string m_strSQL = " select * from LAB_ConfiguracionSISA with (nolock) where  idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem= " + oDetalle.IdSubItem.IdItem.ToString();


//                    // si es contacto se sube==>si es negativo como contacto y si es positivo como sospechoso.
//                    if ((resul == "SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter == 4) && (oC.CodigoCovid == oDetalle.IdSubItem.Codigo))
//                    {
//                        m_strSQL = " select * from LAB_ConfiguracionSISA  with (nolock) where idCaracter=1 and idItem= " + oDetalle.IdSubItem.IdItem.ToString();
//                    }
//                    m_strSQL += @"  and fechavigenciadesde<=convert(date,convert(varchar,getdate(),112)) 
//and ( fechavigenciahasta  >convert(date,convert(varchar,getdate(),112)) or convert(varchar, fechavigenciahasta, 103) = '01/01/1900')
//                                and (idorigen=0 or idOrigen=" + oDetalle.IdProtocolo.IdOrigen.IdOrigen.ToString() + ")";

//                    //Control de efector solicitante
//                    //Monitoreo de SARS COV - 2 y OVR en ambulatorios ==> solo aplica para Hospital Heller
//                    //Demas eventos para todos los efectores solicitantes.
//                    m_strSQL += @" and (idefectorsolicitante=0 or idefectorsolicitante in (" + oDetalle.IdProtocolo.IdEfectorSolicitante.IdEfector.ToString() + "))";
//                    //control de embarzada=s /N
//                    m_strSQL += @"  and soloEmbarazada='" + oDetalle.IdProtocolo.Embarazada.ToString() + "'";
//                    ///control de edades
//                    m_strSQL += @" and (" + oDetalle.IdProtocolo.Edad + " between edadDesde and edadHasta and " + oDetalle.IdProtocolo.UnidadEdad + " = 0) ";


                    string idestablecimientodiagnostico = oDetalle.IdProtocolo.IdEfector.CodigoSISA;///.IdEfectorSolicitante.CodigoSISA;
                    //if ((idestablecimientodiagnostico == "") || (idestablecimientodiagnostico == "0"))                    
                    //idestablecimientodiagnostico = oDetalle.IdProtocolo.IdEfector.CodigoSISA;
                    ////idestablecimientodiagnostico = "107093";


                    //DataSet Ds = new DataSet();
                    ////      SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                    //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                    //SqlDataAdapter adapter = new SqlDataAdapter();
                    //adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    //adapter.Fill(Ds);

                    //DataTable dt = Ds.Tables[0];

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{


                    //    HdidEventoSISA.Value = dt.Rows[i][2].ToString();

                    //    break;
                    //}

                    int id_resultado_a_informar = int.Parse(HdidResultadoSISA.Value);
                    int idevento = int.Parse(HdidEventoSISA.Value);
                    int id_Prueba = int.Parse(HdIdPrueba.Value);
                    int id_TipoPrueba = int.Parse(HdIdTipoPrueba.Value);



                    string res = oDetalle.ResultadoCar;



                    if (id_resultado_a_informar != 0)
                    {


                        string femision = oDetalle.FechaValida.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");
                        if (femision == "1900-01-01")
                            femision = DateTime.Now.ToString("yyyy-MM-dd");
                        string frecepcion = oDetalle.IdProtocolo.Fecha.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                        resultado newresultado = new resultado
                        { // resultado de dni: 31935346
                            derivada = false,
                            fechaEmisionResultado = femision, //"2020-09-14", //
                            fechaRecepcion = null, // frecepcion, // "2020-09-13" 
                            idDerivacion = null, //1125675,//
                            idEstablecimiento = int.Parse(idestablecimientodiagnostico), //107093,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                            idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                            idEventoMuestra = ideventomuestra,  // 2131682, 
                            idPrueba = id_Prueba,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                            idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                            idTipoPrueba = id_TipoPrueba, // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
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
                        request.Headers.Add("app_key", s_userpass);/// "8482d41353ecd747c271f2ec869345e4");
                        request.Headers.Add("app_id", s_user);// "0e4fcbbf");



                        Stream postStream = request.GetRequestStream();
                        postStream.Write(data, 0, data.Length);

                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                        StreamReader reader = new StreamReader(response.GetResponseStream());
                        string body = reader.ReadToEnd();
                        if (body != "")
                        {
                            /*   if (oDetalle.IdUsuarioValida>0)
                               oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oUser.IdUsuario);
                              else*/
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oUser.IdUsuario);
                        }

                    }


                }
                catch (WebException ex)
                {
                    string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                    grabarLogMensaje(mensaje + " ideventomuestra:" + oDetalle.IdeventomuestraSISA.ToString());

                }

            }
        }


        private void SubirSISAdesdeAPI()
        {
            int i = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {
                string idItem = ddlItem.SelectedValue.ToString();// idevento

                //   string res = gvLista.Rows[row.RowIndex].Cells[6].Text;
                ////string idderivacion = gvLista.Rows[row.RowIndex].Cells[4].Text;
                string idmuestra = gvLista.Rows[row.RowIndex].Cells[8].Text;
                string idtipomuestra = gvLista.Rows[row.RowIndex].Cells[9].Text;
                HdIdMuestra.Value = idmuestra;
                HdIdTipoMuestra.Value = idtipomuestra; 

                HdIdPrueba.Value = gvLista.Rows[row.RowIndex].Cells[10].Text;
                HdIdTipoPrueba.Value = gvLista.Rows[row.RowIndex].Cells[11].Text;

                HdidResultadoSISA.Value = gvLista.Rows[row.RowIndex].Cells[12].Text;
                HidItemSIL.Value= gvLista.Rows[row.RowIndex].Cells[15].Text;


                /*nuevos campos recuperados desde la grilla*/
                HidCaracter.Value = gvLista.Rows[row.RowIndex].Cells[16].Text;// por ahora no se usa para interoperar---vER!!
                //idevento = dt.Rows[i][2].ToString();
                HdidEventoSISA.Value = gvLista.Rows[row.RowIndex].Cells[13].Text;
                HidGrupoEvento.Value = gvLista.Rows[row.RowIndex].Cells[17].Text;
                HidClasificacionManual.Value = gvLista.Rows[row.RowIndex].Cells[18].Text;
                //nombreevento = dt.Rows[i][3].ToString();
                //idclasificacionmanual = dt.Rows[i][4].ToString();
                //nombreclasificacionmanual = dt.Rows[i][5].ToString();
                //idgrupoevento = dt.Rows[i][6].ToString();
                //nombregrupoevento = dt.Rows[i][7].ToString();


                string idcasosisa= gvLista.Rows[row.RowIndex].Cells[14].Text;
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
                    oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    if (oDetalleProtocolo != null)
                    {
                        //string res = oDetalleProtocolo.ResultadoCar;



                        //if (oDetalleProtocolo.IdSubItem.Codigo == oC.CodigoCovid)
                        //{
                        //    if (res.Length > 10)
                        //    {
                        //        if (res.Substring(0, 10) == "SE DETECTA")
                        //        { if (ProcesaSISA(oDetalleProtocolo, "SE DETECTA", idcasosisa)) i = i + 1; }

                        //        if (res.Substring(0, 13) == "NO SE DETECTA")
                        //        { if (ProcesaSISA(oDetalleProtocolo, "NO SE DETECTA", idcasosisa)) i = i + 1; }


                        //    }
                        //}
                        //else

                        //{
                            if (ProcesaSISA(oDetalleProtocolo))
                            {
                                i = i + 1;
                                
                            }

                        //}

                        // BorrarLineaTemporalesdesdeAPI(ideventomuestra);
                    }


                }//checked
            }// grid

            rdbEstado.SelectedValue = "1";
            CargarGrilla();
         //   estatus.Text = "se han informado " + i.ToString() + " resultados";
      //      btnDescargarExcelControl.Visible = true;
            //GenerarResultadoSISA(  );

        }
        //private void SubirSISA()
        //{
        //    int i = 0;
        //    foreach (GridViewRow row in gvLista.Rows)
        //    {

        //    //    string res = gvLista.Rows[row.RowIndex].Cells[6].Text;
        //        string idderivacion = gvLista.Rows[row.RowIndex].Cells[4].Text;
        //        string ideventomuestra = gvLista.Rows[row.RowIndex].Cells[5].Text;
        //        CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
        //        if (a.Checked == true)
        //        {

        //            DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
        //            oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
        //            if (GenerarResultadoSISA(oDetalleProtocolo, idderivacion, ideventomuestra))
        //            {
        //                i = i + 1;
        //              //  oDetalleProtocolo.IdProtocolo.GrabarAuditoriaDetalleProtocolo(" Resultado a SISA", int.Parse(Session["idUsuario"].ToString()), "", oDetalleProtocolo.ResultadoCar);
        //                BorrarLineaTemporales(idderivacion, ideventomuestra);
        //            }


        //        }//checked
        //    }// grid


        //    CargarGrilla();
        //    estatus.Text = "se han informado " + i.ToString() + " resultados";
        //    //GenerarResultadoSISA(  );

        //}

        // private bool GenerarResultadoSISA(DetalleProtocolo oDetalle,  string idderivacion, string ideventomuestra)
     
        //{
        //    bool generacaso = false;

        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //    string URL = oCon.URLResultadoSISA;



        //    try
        //    {
        //        int id_resultado_a_informar = 0;

        //        // nose notificò antes y es sospechoso o contacto
             
        //        int idevento = 307; // sospechoso

        //        if (oDetalle.IdProtocolo.IdCaracter == 4) idevento = 309; // contacto
        //        // nose notificò antes y es sospechoso o contacto


        //        string res = oDetalle.ResultadoCar;


        //        if (res.Length > 10)
        //        {
        //            if (res.Substring(0, 10) == "SE DETECTA")
        //            { id_resultado_a_informar = 3; }

        //            if (res.Substring(0, 13) == "NO SE DETECTA")
        //            { id_resultado_a_informar = 4; }

        //        } // if res 

        //        if (id_resultado_a_informar != 0)
        //        {
        //             string femision = oDetalle.FechaValida.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

        //            string frecepcion = oDetalle.IdProtocolo.Fecha.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                  



        //            resultado newresultado = new resultado
        //            { // resultado de dni: 31935346
        //                derivada = true,
        //                fechaEmisionResultado =  femision, //"2020-09-14", //
        //                fechaRecepcion = frecepcion, // "2020-09-13" 
        //                idDerivacion = int.Parse(idderivacion), //1125675,//
        //                idEstablecimiento = 107093,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
        //                idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
        //                idEventoMuestra = int.Parse(ideventomuestra),  // 2131682, // sale del excel
        //                idPrueba = 1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
        //                idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
        //                idTipoPrueba = 727, // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
        //                noApta = true,
        //                valor = ""
        //            };



        //            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        //            string DATA = jsonSerializer.Serialize(newresultado);


        //            byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

        //            HttpWebRequest request;
        //            request = WebRequest.Create(URL) as HttpWebRequest;
        //            request.Timeout = 10 * 1000;
        //            request.Method = "POST";
        //            request.ContentLength = data.Length;
        //            request.ContentType = "application/json";
        //            string a_apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("8482d41353ecd747c271f2ec869345e4"));
        //            string a_apiId = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("0e4fcbbf"));
        //            request.Headers.Add("app_key", "8482d41353ecd747c271f2ec869345e4");
        //            request.Headers.Add("app_id", "0e4fcbbf");



        //            Stream postStream = request.GetRequestStream();
        //            postStream.Write(data, 0, data.Length);

        //            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //            StreamReader reader = new StreamReader(response.GetResponseStream());
        //            string body = reader.ReadToEnd();
        //            if (body != "")
        //            {
        //                oDetalle.IdeventomuestraSISA = int.Parse(ideventomuestra);
        //                oDetalle.Save();

        //                oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + ideventomuestra,int.Parse( Session["idUsuario"].ToString()));



        //                oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", int.Parse(Session["idUsuario"].ToString()));
        //                generacaso = true;
        //            }

        //        }
                 

        //    }
        //    catch (Exception e)
        //    {
        //        generacaso = false;
                    


        //        //lblError.Visible = true;
        //        //btnSalir.Visible = true;
        //    }
        //    return generacaso;

        //}


    

        //private void BorrarResultadosLuminex(int nroProtocolo)
        //{
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
        //    crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
        //    IList detalle = crit.List();
        //    if (detalle.Count > 0)
        //    {
        //        foreach (ProtocoloLuminex oDetalle in detalle)
        //        {
        //            oDetalle.Delete();
        //        }
        //    }

        //}
         
        public class resultado
        {
            public Boolean derivada { get; set; }
            public string fechaEmisionResultado { get; set; }
            public string fechaRecepcion { get; set; }
            public int? idDerivacion { get; set; }
            public int idEstablecimiento { get; set; }
            public int idEvento { get; set; }

            public int idEventoMuestra { get; set; }
            public int idPrueba { get; set; }
            public int idResultado { get; set; }
            public int idTipoPrueba { get; set; }
            public Boolean noApta { get; set; }
            public string valor { get; set; }


        }

        protected void btnDescargarExcelControl_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }
        private DataTable MostrarDatos()
        {
            DateTime fecha1 = DateTime.Now;
           

            string m_strSQL = @" select P.numero, P.fecha, pa.numeroDocumento as dni,  pa.apellido, pa.nombre, d.resultadoCar 
            from            LAB_AuditoriaProtocolo A
            inner join LAB_Protocolo P on P.idProtocolo= A.idProtocolo
            inner join Sys_Paciente pa on pa.idPaciente= P.idPaciente
            inner join LAB_DetalleProtocolo d on d.idProtocolo= P.idProtocolo
            where A.idUsuario="+ Session["idUsuario"].ToString() +"  and A.fecha>='"+fecha1.ToString("yyyyMMdd")+"' and accion='Genera Resultado en SISA'  ";           


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
          
            return Ds.Tables[0];
        }
        private void ExportarExcel()
        {
            try
            {
                DataTable tabla = MostrarDatos();
                if (tabla.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    Page pagina = new Page();
                    HtmlForm form = new HtmlForm();
                    GridView dg = new GridView();
                    dg.EnableViewState = false;
                    dg.DataSource = tabla;
                
                    dg.DataBind();
                    pagina.EnableEventValidation = false;
                    pagina.DesignerInitialize();
                    pagina.Controls.Add(form);
                    form.Controls.Add(dg);
                    pagina.RenderControl(htw);
                    Response.Clear();
                    Response.Buffer = true;
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=ControlActualizacionSISA_" + DateTime.Now.ToShortDateString() + ".xls");
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;
                    Response.Write(sb.ToString());
                    Response.End();
                }
            }

            catch
            {

               
            }




        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                CargarResultado();
                Buscar();
            }



        }

        private void CargarResultado()
        {                    
            Utility oUtil = new Utility();  string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

           
                string m_ssql = @" select distinct ds.resultado 
                                from   LAB_ConfiguracionSISA S with (nolock)
                                inner join LAB_ConfiguracionSISADetalle DS with  (nolock) on DS.idItem=s.iditem  
                                where s.idEvento=  " + ddlItem.SelectedValue;
                ///where s.iditem=  " + ddlItem.SelectedValue;

                oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado", connReady);
           
                ddlResultado.Items.Insert(0, new ListItem("--Todos--", "0"));
            


        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();

        }

        private void Buscar()
        {
            CargarGrilla();
            if (rdbEstado.SelectedValue == "0")
            {
                btnSISA.Visible = true;
                btnNoInformarSISA.Visible = true;
            }

            else
            {
                if (rdbEstado.SelectedValue == "1")
                {
                    btnSISA.Visible = false;
                    btnNoInformarSISA.Visible = false;
                }
                if (rdbEstado.SelectedValue == "2")
                {
                    btnSISA.Visible = true;
                    btnNoInformarSISA.Visible = false;
                }
            }

        }

        protected void btnNoInformarSISA_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
                    oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    if (oDetalleProtocolo != null)
                    {
                        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                        string query = @" INSERT INTO [dbo].[LAB_DetalleProtocoloExcluidoSISA]
           (idDetalleProtocolo,fechaRegistro,	idUsuarioRegistro )
     VALUES
           ( " + oDetalleProtocolo.IdDetalleProtocolo.ToString() + ",getdate()" + "," + oUser.IdUsuario.ToString() + " )";
                        SqlCommand cmd = new SqlCommand(query, conn);


                        int idres = Convert.ToInt32(cmd.ExecuteScalar());

                        oDetalleProtocolo.GrabarAuditoriaDetalleProtocolo("Excluido SISA", oUser.IdUsuario);
                    }
                }
            }
        }

        protected void ddlResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar();
        }
    }
}