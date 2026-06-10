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
using System.Text.RegularExpressions;

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


        private void RegistrarHistoricoSISA(      DetalleProtocolo oDetalle,       string estadoEnvio,       string mensaje,       string tipoInteroperabilidad)
        {
            /*
             CARO:
             Este registro es para poder generar estadísticas de envíos a SISA
             sin acceder a las tablas madres ya que los queries son poco performantes.
            */

            try
            {
                if (oDetalle == null)
                    return;

                SqlConnection conn =
                    (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                string sql = @"
        INSERT INTO LAB_HistoricoEnvioSISA
        (
            fechaEnvio,fechaProtocolo,
            idProtocolo,
            idDetalleProtocolo,
            numeroProtocolo,
            idPaciente,
            documento,
            apellidoPaciente,
            nombrePaciente,
            idEfector,
            nombreEfector,
            idCasoSISA,
            idEventoMuestraSISA,
            idItem,
            nombreItem,
            resultado,
            estadoEnvio,
            tipoInteroperabilidad,
            mensaje,
            usuarioEnvio,
            fechaRegistro
        )
        VALUES
        (
            GETDATE(), @fechaProtocolo,
            @idProtocolo,
            @idDetalleProtocolo,
            @numeroProtocolo,
            @idPaciente,
            @documento,
            @apellidoPaciente,
            @nombrePaciente,
            @idEfector,
            @nombreEfector,
            @idCasoSISA,
            @idEventoMuestraSISA,
            @idItem,
            @nombreItem,
            @resultado,
            @estadoEnvio,
            @tipoInteroperabilidad,
            @mensaje,
            @usuarioEnvio,
            GETDATE()
        )";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Protocolo
                    cmd.Parameters.AddWithValue("@fechaProtocolo",
                        oDetalle.IdProtocolo.Fecha);
                   

                    cmd.Parameters.AddWithValue("@idProtocolo",
                        oDetalle.IdProtocolo.IdProtocolo);

                    cmd.Parameters.AddWithValue("@idDetalleProtocolo",
                        oDetalle.IdDetalleProtocolo);

                    cmd.Parameters.AddWithValue("@numeroProtocolo",
                        oDetalle.IdProtocolo.Numero);

                    // Paciente
                    cmd.Parameters.AddWithValue("@idPaciente",
                        oDetalle.IdProtocolo.IdPaciente.IdPaciente);

                    cmd.Parameters.AddWithValue("@documento",
                        oDetalle.IdProtocolo.IdPaciente.NumeroDocumento );

                    cmd.Parameters.AddWithValue("@apellidoPaciente",
                        oDetalle.IdProtocolo.IdPaciente.Apellido );

                    cmd.Parameters.AddWithValue("@nombrePaciente",
                        oDetalle.IdProtocolo.IdPaciente.Nombre  );

                    // Efector
                    cmd.Parameters.AddWithValue("@idEfector",
                        oDetalle.IdProtocolo.IdEfector.IdEfector);

                    cmd.Parameters.AddWithValue("@nombreEfector",
                        oDetalle.IdProtocolo.IdEfector.Nombre );

                    // IDs SISA
                    cmd.Parameters.AddWithValue("@idCasoSISA",
                        oDetalle.IdProtocolo.IdCasoSISA);

                    cmd.Parameters.AddWithValue("@idEventoMuestraSISA",
                        oDetalle.IdeventomuestraSISA);

                    // Item
                    cmd.Parameters.AddWithValue("@idItem",
                        oDetalle.IdSubItem.IdItem);

                    cmd.Parameters.AddWithValue("@nombreItem",
                        oDetalle.IdSubItem.Nombre );

                    // Resultado
                    cmd.Parameters.AddWithValue("@resultado",
                        oDetalle.ResultadoCar );

                    // Estado
                    cmd.Parameters.AddWithValue("@estadoEnvio",
                        estadoEnvio );

                    // Tipo interoperabilidad
                    cmd.Parameters.AddWithValue("@tipoInteroperabilidad",
                        tipoInteroperabilidad );

                    // Mensaje
                    if (string.IsNullOrEmpty(mensaje))
                        mensaje = "";

                    if (mensaje.Length > 4000)
                        mensaje = mensaje.Substring(0, 4000);

                    cmd.Parameters.AddWithValue("@mensaje", mensaje);

                    // Usuario
                    cmd.Parameters.AddWithValue("@usuarioEnvio",
                        oUser.IdUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                grabarLogMensaje(
                    "Error RegistrarHistoricoSISA: " + ex.Message);
            }
        }
        private void CargarGrilla()
        {
            gvListaFicha.Visible = false;
            gvLista.Visible = false;
            DataSet Ds = new DataSet();            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            

            string str_condicion = "";
            if (ddlResultado.SelectedValue != "0")
                str_condicion += " AND d.resultadocar= '" + ddlResultado.SelectedValue + "'";
            if (ddlTipoFicha.SelectedValue != "0")
            {
                str_condicion += " and p.idEfector=" + ddlEfector.SelectedValue;             
                if (txtFechaDesde.Value != "")
                {
                     DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);                
                     str_condicion += " AND d.fechaValida>= '" + fecha1.ToString("yyyyMMdd") + "'"; 
                }

                cmd.CommandText = "[LAB_ResultadosASisaFicha]";

                cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
                cmd.Parameters["@FiltroBusqueda"].Value = str_condicion;

                cmd.Parameters.Add("@idficha", SqlDbType.NVarChar);
                cmd.Parameters["@idficha"].Value = ddlTipoFicha.SelectedValue;

                cmd.Parameters.Add("@Estado", SqlDbType.Int);
                cmd.Parameters["@Estado"].Value = rdbEstado.SelectedValue;

                cmd.Parameters.Add("@agrupado", SqlDbType.Bit);
                cmd.Parameters["@agrupado"].Value = 0;//a no grupado: lista de procotocolos


                cmd.Connection = conn;


                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(Ds);

                gvListaFicha.Visible = true;
                gvListaFicha.DataSource = Ds.Tables[0];
                gvListaFicha.DataBind();
            }
            else
            {
                 str_condicion += " and C.idEfector=" + ddlEfector.SelectedValue;
             
                if (txtFechaDesde.Value != "")
                {
                     DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value); 
                     str_condicion += " AND d.fechaValida>= '" + fecha1.ToString("yyyyMMdd") + "'"; 
                }
                
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


                gvLista.Visible = true;
                gvLista.DataSource = Ds.Tables[0];
                gvLista.DataBind();
            }
                lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
                conn.Close();
            //}
        }
        

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);         
        }
 

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);            
        }

        private void MarcarSeleccionados(bool p)
        {
            if (ddlTipoFicha.SelectedValue != "0")
            {
                foreach (GridViewRow row in gvListaFicha.Rows)
                {
                     CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox2")));
                     if (a.Checked == !p)
                         ((CheckBox)(row.Cells[0].FindControl("CheckBox2"))).Checked = p;
                }
            }
            else
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                     CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                     if (a.Checked == !p)
                         ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
                }
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
            if (ddlTipoFicha.SelectedValue == "")
                SubirSISAdesdeAPI();
            else
                SubirSISAdesdeAPI_Ficha();
        }

        private bool ProcesaSISAFicha(DetalleProtocolo oDetalle)
        {
            bool generacaso = false;

            try
            {
           
                HdidEventoSISA.Value = "";
                /*     Caro: Se busca el idcasosisa invocando la api de andes que devuelve datos por ficha , se busca siempre en esta instancia por si cambio el id snvs */
                //if (oDetalle.IdProtocolo.ideventosnvs == 0) 
              //  if ((oDetalle.IdProtocolo.IdCasoSISA == 0) || (oDetalle.IdProtocolo.IdEvento == 0))
                    BuscarCasoSISA_Ficha(oDetalle.IdProtocolo);
           
                if ((oDetalle.IdProtocolo.IdCasoSISA > 0) && (oDetalle.IdProtocolo.IdEvento > 0) && (oDetalle.IdeventomuestraSISA == 0))
                {
                     BuscarMuestraporEvento(oDetalle);
                   
                     bool existe = ExisteTipoMuestraFicha  (oDetalle, HdIdMuestra.Value, HdIdTipoMuestra.Value);
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

        private void BuscarMuestraporEvento(DetalleProtocolo oDetalle)
        {
            string m_strSQL = @" select top 1 idmuestra ,  idTipoMuestra, idPrueba    ,idTipoPrueba from LAB_ConfiguracionSISAFicha
                                where idevento=" + oDetalle.IdProtocolo.IdEvento.ToString() + @"
                                and idFicha='" + ddlTipoFicha.SelectedValue + "' and idMuestraSIL=" + oDetalle.IdProtocolo.IdMuestra.ToString() + " and iditem=" + HidItemSIL.Value; ;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            HdidEventoSISA.Value = oDetalle.IdProtocolo.IdEvento.ToString();  //dato necesario para interopoerar resultados
            if (Ds.Tables[0].Rows.Count > 0)
            {

                HdIdMuestra.Value = Ds.Tables[0].Rows[0][0].ToString();/// idmuestra;
                HdIdTipoMuestra.Value = Ds.Tables[0].Rows[0][1].ToString();///idtipomuestra;

                HdIdPrueba.Value = Ds.Tables[0].Rows[0][2].ToString();///
                HdIdTipoPrueba.Value = Ds.Tables[0].Rows[0][3].ToString();///
                 

                }
             
        }

        private void BuscarCasoSISA_Ficha(Protocolo oRegistro)
        {          
            try
            {

                System.Net.ServicePointManager.SecurityProtocol =
              System.Net.SecurityProtocolType.Tls12;
                string URL = ConfigurationManager.AppSettings["urlffeeandes"].ToString();
                URL = URL + "&desde=2000-01-01T23:18:08.382Z&hasta=2050-01-01T23:18:11.407Z&totalOrganizaciones=true";               
                if (ddlTipoFicha.SelectedValue != "0")
                        URL = URL + "&tipoForm=" + ddlTipoFicha.SelectedValue;
                //if (txtDni.Value != "")
                ///    URL = URL + "&documento=" + oRegistro.IdPaciente.NumeroDocumento.ToString();///busca por dni en algun momento deberá buscar por id de ficha.
              
                URL = URL + "&idFicha=" + HidFicha.Value.Trim();//forma univoca de traer un solo registro
              
                string s_token = ConfigurationManager.AppSettings["tokenffeeandes"].ToString();

                
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                HttpWebRequest request;
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.Timeout = 10 * 1000;
                request.Method = "GET";

                request.ContentType = "application/json";
                request.Headers.Add("Authorization", s_token);


                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();                

                if (!string.IsNullOrEmpty(body))
                {
                         body = body.Trim();

                         Protocolos.DefaultFFEE2.FFEE respuesta_d = null;

                         // devuelve array
                         if (body.StartsWith("["))
                         {
                             List<Protocolos.DefaultFFEE2.FFEE> lista =
                                 jsonSerializer.Deserialize
                                 <List<Protocolos.DefaultFFEE2.FFEE>>(body);

                             // actualizar solo si hay un único objeto
                             if (lista != null && lista.Count == 1)
                             {
                                 respuesta_d = lista[0];
                             }
                         }
                         else
                         {
                             // devuelve objeto único
                             respuesta_d =
                                 jsonSerializer.Deserialize
                                 <Protocolos.DefaultFFEE2.FFEE>(body);
                         }

                         if (respuesta_d != null &&
                             !string.IsNullOrEmpty(respuesta_d.idCasoSnvs))
                         {
                                oRegistro.IdCasoSISA =  int.Parse(respuesta_d.idCasoSnvs.Trim());
                                oRegistro.IdEvento = int.Parse(respuesta_d.idEvento.Trim());
                                oRegistro.Save();
                         }
                }
            }
            catch (WebException ex)
            {
                estatus.Text = "Error al conectarse a Andes para recuperar evento SNVS: " + ex.Message.ToString();
                estatus.Visible = true;

                string mensaje = "Error al conectarse a Andes para recuperar evento SNVS: " + ex.Message.ToString();
                grabarLogMensaje(mensaje + " protocolo: " + oRegistro.Numero.ToString());
            }
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

            ///carga de tipo de ficha
            m_ssql = "SELECT  nombre   as nombre, codigo FROM LAB_TipoFicha  with (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlTipoFicha, m_ssql, "codigo", "nombre", connReady);
            ddlTipoFicha.Items.Insert(0, new ListItem("Seleccione", "0"));

            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            //              m_ssql = @"SELECT distinct i.idItem, i.nombre FROM LAB_item i (nolock)
            //inner join LAB_ConfiguracionSISA c (nolock) on c.idItem = i.idItem
            //order by i.nombre ";

            //            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            //            ddlItem.Items.Insert(0, new ListItem("Seleccione", "0"));

            m_ssql = @"SELECT distinct c.idEvento, c.nombreEvento
 FROM   LAB_ConfiguracionSISA c with (nolock) 
 where idItem not in (select distinct idItem from LAB_ConfiguracionSISAficha) 
 order by c.nombreEvento";

            oUtil.CargarCombo(ddlItem, m_ssql, "idEvento", "nombreEvento", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione", "0"));
            m_ssql = null;
            oUtil = null;
        }
  

        private void GenerarCasoSISA_V2(DetalleProtocolo oDetalle)
        {
            /*Version 2*/
            /*Cambio para que vuelva a buscar los datos en la base y tome los datos desde la grilla*/
            System.Net.ServicePointManager.SecurityProtocol =             System.Net.SecurityProtocolType.Tls12;         
            bool seguir = false;            
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            try
            {                
                seguir = true;                
                if (seguir)
                {
                     string conexionServicio = oC.UrlServicioSISA;
                     string[] arr = conexionServicio.Split((";").ToCharArray());

                     if (arr.Length >= 1)
                     {
                         string URL = arr[0].ToString();
                         string s_user = arr[1].ToString();
                         string s_userpass = arr[2].ToString();     
                         string s_idestablecimiento = oC.CodigoEstablecimientoSISA; 
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
                             RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(body);

                             if (respuesta_d.id_caso != "")
                             { //  devolver el idcaso para guardar en la base de datos
                                 string s_idcaso = respuesta_d.id_caso;
                                 //oDetalle.IdProtocolo.IdEvento = int.Parse(HdidEventoSISA.Value);
                                 oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                                 oDetalle.IdProtocolo.Save();
                                 if (respuesta_d.resultado == "OK")
                                     oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, oUser.IdUsuario);
                                 else // ERROR_DATOS
                                     oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, oUser.IdUsuario);
                             }
                             else
                             {
         
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

       

        private bool ExisteTipoMuestraFicha(DetalleProtocolo oDetalle,string idmuestra,string idtipomuestra)
        {
            /// Verifica si para la misma muestra SISA ya existe creada
            /// y replica el ideventomuestraSISA en el detalle actual

            if (oDetalle == null ||  oDetalle.IdProtocolo == null ||  oDetalle.IdProtocolo.IdCasoSISA <= 0)
                return false;

            bool existe = false;

            string sql = @" SELECT TOP 1 dp.ideventomuestraSISA
         FROM Lab_detalleprotocolo dp WITH (NOLOCK)
         INNER JOIN LAB_Protocolo p WITH (NOLOCK)
             ON p.idProtocolo = dp.idProtocolo
         INNER JOIN lab_configuracionsisaFicha cf WITH (NOLOCK)
             ON cf.iditem = dp.idsubitem
         WHERE p.idcasosisa = @idcasosisa
             AND dp.ideventomuestraSISA > 0
             AND cf.idFicha = @idficha
             AND cf.idmuestra = @idmuestra
             AND cf.idTipoMuestra = @idtipomuestra
         ORDER BY dp.ideventomuestraSISA DESC";

            SqlConnection conn =  (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idcasosisa",oDetalle.IdProtocolo.IdCasoSISA);
                cmd.Parameters.AddWithValue("@idficha",ddlTipoFicha.SelectedValue);
                cmd.Parameters.AddWithValue("@idmuestra",int.Parse(idmuestra));
                cmd.Parameters.AddWithValue("@idtipomuestra",int.Parse(idtipomuestra));

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value && oDetalle.IdeventomuestraSISA == 0)
                {
                     int ideventomuestraSISA = Convert.ToInt32(result);

                     if (ideventomuestraSISA > 0)
                     {
                         existe = true;
                         oDetalle.IdeventomuestraSISA =  ideventomuestraSISA;
                         oDetalle.Save();

                         oDetalle.GrabarAuditoriaDetalleProtocolo(  "Replica Muestra SISA " +  ideventomuestraSISA,  oUser.IdUsuario);
                     }
                }
            }

            return existe;
        }

        //       private bool ExisteTipoMuestra(DetalleProtocolo oDetalle, string idmuestra, string idtipomuestra)
        //       {
        //           ///Verifica si para la misma muestra sisa ya existe creada se replica el ideventomuestraSISA en todos los detalles del mismo protocolo
        //           bool existe = false;
        //           string idevento=HdidEventoSISA.Value;
        //           string ideventomuestraSISA = "0";
        //           string m_strSQL = @"select top 1  ideventomuestraSISA 
        //            from Lab_detalleprotocolo  
        //            where idProtocolo in (select idProtocolo from LAB_Protocolo with (nolock) where idcasosisa=" + oDetalle.IdProtocolo.IdCasoSISA.ToString() + @")             
        //            and ideventomuestraSISA>0
        //            and idsubitem in (select iditem from lab_configuracionsisa with (nolock) where idevento="+ idevento + " and idmuestra = " + idmuestra.ToString() + @" 
        //             and idTipoMuestra = " + idtipomuestra.ToString() + ") order by ideventomuestraSISA desc";

        //           DataSet Ds = new DataSet();
        //           SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;            
        //           SqlDataAdapter adapter = new SqlDataAdapter();
        //           adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //           adapter.Fill(Ds);

        //           DataTable dt = Ds.Tables[0];

        //           for (int i = 0; i < dt.Rows.Count; i++)
        //           { 
        //ideventomuestraSISA = dt.Rows[i][0].ToString();               
        //if ((ideventomuestraSISA != "0") &&  (oDetalle.IdeventomuestraSISA == 0))
        //        {
        //        existe = true;
        //            oDetalle.IdeventomuestraSISA = int.Parse(ideventomuestraSISA);
        //            oDetalle.Save();

        //            oDetalle.GrabarAuditoriaDetalleProtocolo("Replica Muestra SISA " + ideventomuestraSISA.ToString(), oUser.IdUsuario);
        //        }
        //           }

        //           return existe;
        //       }


        private bool ExisteTipoMuestra(DetalleProtocolo oDetalle, string idmuestra, string idtipomuestra)
        {
            // Verifica si para la misma muestra SISA ya existe creada
            // y replica el ideventomuestraSISA en todos los detalles del mismo protocolo.

            bool existe = false;

            int idevento;
            if (!int.TryParse(HdidEventoSISA.Value, out idevento))
                return false;

            string m_strSQL = @"SELECT TOP 1 ideventomuestraSISA
        FROM Lab_detalleprotocolo dp
        WHERE dp.idProtocolo IN (
                SELECT p.idProtocolo
                FROM LAB_Protocolo p WITH (NOLOCK)
                WHERE p.idcasosisa = @idCasoSISA
              )
          AND dp.ideventomuestraSISA > 0
          AND dp.idsubitem IN (
                SELECT cs.iditem
                FROM lab_configuracionsisa cs WITH (NOLOCK)
                WHERE cs.idevento = @idevento
                  AND cs.idmuestra = @idmuestra
                  AND cs.idTipoMuestra = @idTipoMuestra
              )
        ORDER BY dp.ideventomuestraSISA DESC";

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            using (SqlCommand cmd = new SqlCommand(m_strSQL, conn))
            {
                cmd.Parameters.AddWithValue("@idCasoSISA", oDetalle.IdProtocolo.IdCasoSISA);
                cmd.Parameters.AddWithValue("@idevento", idevento);
                cmd.Parameters.AddWithValue("@idmuestra", idmuestra);
                cmd.Parameters.AddWithValue("@idTipoMuestra", idtipomuestra);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int ideventomuestraSISA;
                    if (int.TryParse(result.ToString(), out ideventomuestraSISA))
                    {
                        if (ideventomuestraSISA > 0 && oDetalle.IdeventomuestraSISA == 0)
                        {
                            existe = true;
                            oDetalle.IdeventomuestraSISA = ideventomuestraSISA;
                            oDetalle.Save();
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Replica Muestra SISA " + ideventomuestraSISA, oUser.IdUsuario);
                        }
                    }
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
                     string idestablecimientodiagnostico = oDetalle.IdProtocolo.IdEfector.CodigoSISA;///.IdEfectorSolicitante.CodigoSISA; 
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
                             oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oUser.IdUsuario);
                             if (gvLista.Visible)
                             RegistrarHistoricoSISA(oDetalle, "ENVIADO", body, "EVENTO");
                             else
                             RegistrarHistoricoSISA(oDetalle, "ENVIADO", body, "FICHA");
                         }

                     }


                }
                catch (WebException ex)
                {
                     string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                     grabarLogMensaje("Genera Resultado SISA:" + mensaje + " ideventomuestra:" + oDetalle.IdeventomuestraSISA.ToString());
 
                }

            }
        }

        private void SubirSISAdesdeAPI_Ficha()
        {
            int i = 0;
            foreach (GridViewRow row in gvListaFicha.Rows)
            {
          
                //string idmuestra = gvListaFicha.Rows[row.RowIndex].Cells[8].Text;
                //string idtipomuestra = gvListaFicha.Rows[row.RowIndex].Cells[9].Text;
                //HdIdMuestra.Value = idmuestra;
                //HdIdTipoMuestra.Value = idtipomuestra;

                //HdIdPrueba.Value = gvListaFicha.Rows[row.RowIndex].Cells[10].Text;
                //HdIdTipoPrueba.Value = gvListaFicha.Rows[row.RowIndex].Cells[11].Text;

                HdidResultadoSISA.Value = gvListaFicha.Rows[row.RowIndex].Cells[8].Text;
                HidItemSIL.Value = gvListaFicha.Rows[row.RowIndex].Cells[10].Text;
                HidFicha.Value = gvListaFicha.Rows[row.RowIndex].Cells[11].Text;
           
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox2")));
                if (a.Checked == true) 
                {
                     DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
                     oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvListaFicha.DataKeys[row.RowIndex].Value.ToString()));

                     if (oDetalleProtocolo != null)
                     {    
                         if (ProcesaSISAFicha(oDetalleProtocolo))
                        {
                         
                            i = i + 1;
                         }
                     }
                }//checked
            }// grid

            rdbEstado.SelectedValue = "1";
            CargarGrilla();
            

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
                string idcasosisa= gvLista.Rows[row.RowIndex].Cells[14].Text;
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                     DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
                     oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                     if (oDetalleProtocolo != null)
                     {     
                             if (ProcesaSISA(oDetalleProtocolo))
                             {
                                 i = i + 1;
                         }     
                     }
                }//checked
            }// grid

            rdbEstado.SelectedValue = "1";
            CargarGrilla();
      

        }
        

    
     
         
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

 //       protected void btnDescargarExcelControl_Click(object sender, EventArgs e)
 //       {
 //           ExportarExcel();
 //       }
 //       private DataTable MostrarDatos()
 //       {
 //           DateTime fecha1 = DateTime.Now;
           

 //           string m_strSQL = @" select P.numero, P.fecha, pa.numeroDocumento as dni,  pa.apellido, pa.nombre, d.resultadoCar 
 //           from            LAB_AuditoriaProtocolo A
 //           inner join LAB_Protocolo P on P.idProtocolo= A.idProtocolo
 //           inner join Sys_Paciente pa on pa.idPaciente= P.idPaciente
 //           inner join LAB_DetalleProtocolo d on d.idProtocolo= P.idProtocolo
 //           where A.idUsuario="+ Session["idUsuario"].ToString() +"  and A.fecha>='"+fecha1.ToString("yyyyMMdd")+"' and accion='Genera Resultado en SISA'  ";           


 //           DataSet Ds = new DataSet();
 //           SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
 //           SqlDataAdapter adapter = new SqlDataAdapter();
 //           adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
 //           adapter.Fill(Ds);
          
 //           return Ds.Tables[0];
 //       }
 //       private void ExportarExcel()
 //       {
 //           try
 //           {
 //               DataTable tabla = MostrarDatos();
 //               if (tabla.Rows.Count > 0)
 //               {
 //StringBuilder sb = new StringBuilder();
 //StringWriter sw = new StringWriter(sb);
 //HtmlTextWriter htw = new HtmlTextWriter(sw);
 //Page pagina = new Page();
 //HtmlForm form = new HtmlForm();
 //GridView dg = new GridView();
 //dg.EnableViewState = false;
 //dg.DataSource = tabla;
                
 //dg.DataBind();
 //pagina.EnableEventValidation = false;
 //pagina.DesignerInitialize();
 //pagina.Controls.Add(form);
 //form.Controls.Add(dg);
 //pagina.RenderControl(htw);
 //Response.Clear();
 //Response.Buffer = true;
 ////Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
 //Response.ContentType = "application/vnd.ms-excel";
 //Response.AddHeader("Content-Disposition", "attachment;filename=ControlActualizacionSISA_" + DateTime.Now.ToShortDateString() + ".xls");
 //Response.Charset = "UTF-8";
 //Response.ContentEncoding = Encoding.Default;
 //Response.Write(sb.ToString());
 //Response.End();
 //               }
 //           }

 //           catch
 //           {

               
 //           }




 //       }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                CargarResultado();
                Buscar();
            }
            
         



        }

        private void CargarResultadoFicha()
        {
            Utility oUtil = new Utility(); string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            string m_ssql = @"  select  distinct ds.resultado 
             from   LAB_ConfiguracionSISAFicha S with (nolock)
             inner join LAB_ConfiguracionSISADetalle DS with  (nolock) on DS.idItem=s.iditem  
             where s.idficha='" + ddlTipoFicha.SelectedValue+"'";
            ///where s.iditem=  " + ddlItem.SelectedValue;

            oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado", connReady);

            ddlResultado.Items.Insert(0, new ListItem("--Todos--", "0"));



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

        protected void ddlTipoFicha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoFicha.SelectedValue != "0")
            {
                CargarResultadoFicha();
                Buscar();
            }
        }
    }
}