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
using Business.Data;
using System.Data.SqlClient;
using Business.Data.Laboratorio;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.IO;
using System.DirectoryServices.Protocols;
using System.Security.Cryptography;
using System.Text;

namespace WebLab
{
    public partial class loginSIL : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                this.Login1.Focus();
                if (Request["Operacion"] == null)
                {
                    Session["idUsuarioValida"] = null;
                    Session["idUsuario"] = null;
                    Session.Clear();
                 /*     Caro: en miultiefector, no se autentica con SSO
                 if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")
                        Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx?relogin=1", "login.aspx");
                        */
                }
            }
        
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {

            ///Usuario Tercera Parte: no verifica que el ultimo perfil loguaeado esté activo; solo verifica que el usuario esté activo
            string mensajeError;
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Login1.UserName);
            if (oUser!= null)                //(i_idusuario > 0)
            {
               if (oUser.Activo)/// && (oUser.IdPerfil.Activo))                         
                {
                    if (VerificarTipoAutenticacion(oUser ,out mensajeError))
                    {
                        if (MostrarTerminosCondiciones(oUser))
                        {
                            Session["usuarioPendienteAceptacion"] = oUser.IdUsuario;// i_idusuario;
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "$('#modalTerminosCondiciones').modal('show');", true);
                            return;
                        }
                    }
                    else
                    {
                        oUser = null;
                        e.Authenticated = false;
                        Login1.FailureText = mensajeError;//"Usuario y/o contraseña incorrecta.";
                        return;
                    }
                }
                else
                {
                    oUser = null;
                    e.Authenticated = false;
                    Login1.FailureText = "El usuario inválido. Consulte con el Administrador.";
                    return;
                }

            }
            else
            {
                oUser = null;
                e.Authenticated = false;
                Login1.FailureText = "El usuario inválido."; return;
            }


            IngresoSistema(oUser, e);
        }


      

       
        private bool VerificarSiTienePermisodeValidar(string user, string m_url)
        {
            // Sys_Usuario contiene en idefector y idperfil los seleccionados al loguearse.
            string m_strSQL = @" SELECT   P.permiso,P.objeto,P.url, U.username
            FROM        [LAB_MenuTempUsuario] P            
            INNER JOIN   Sys_Usuario AS U ON P.idusuario = U.idusuario
            WHERE     (P.url = @url) AND (U.username = @username) AND (P.permiso = 2) and  (U.activo=1 ) ";

            //string m_strSQL = @" SELECT   P.permiso, M.objeto, M.url, U.username
            //FROM         Sys_Menu AS M 
            //INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
            //INNER JOIN   Sys_Usuario AS U ON P.idPerfil = U.idPerfil
            //WHERE     (M.url = @url) AND (U.username = @username) AND (P.permiso = 2) and  (U.activo=1 ) ";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura           
            using (SqlCommand cmd = new SqlCommand(m_strSQL, conn))
            {
                cmd.Parameters.AddWithValue("@url", m_url);
                cmd.Parameters.AddWithValue("@username", user);

                conn.Open();

                var result = cmd.ExecuteScalar();

                return result != null;
            }
        }

        private void IngresoSistema(Usuario oUser, AuthenticateEventArgs e)
        {                                     

            if (oUser != null)
            {
                ////Usuario Tercera Parte: no verifica el perfil de usuario, se verifica mas adelante en loginefector
                //if ((oUser.Activo) && (oUser.IdPerfil.Activo))
                if (oUser.Activo) 
                {
                    if ((oUser.Activo) && (oUser.Externo))
                    {
                        Session["idUsuario"] = oUser.IdUsuario.ToString();
                        Response.Redirect("~/Consulta/Historiaclinicafiltro.aspx", false);

                    }
                    else
                    {
                        Session["idUsuarioValida"] = null;

                        if (Request["Operacion"] == null)
                        {//////////////nuevo login
                            Session["SIL"] = "1";

                            if (oUser.RequiereCambioPass)
                                Response.Redirect("~/Usuarios/PasswordEdit2.aspx?idUsuario=" + oUser.IdUsuario.ToString() +"&Desde=loginSil", false);

                            else
                            {
                                Session["idUsuarioAux"] = oUser.IdUsuario.ToString();
                                Response.Redirect("LoginEfector.aspx", false);
                                 
                            }
                        }
                        else
                        {///////////////validacion
                            if (Request["idCasoFiliacion"] != null)
                            {


                                if ((Request["idServicio"].ToString() == "6") && (VerificarSiTienePermisodeValidar(oUser.Username, "/CasoFiliacion/CasoResultado.aspx")))
                                {
                                    //HttpContext Context;
                                    //Context = HttpContext.Current;
                                    //Context.Items.Add("id", Request["idCasoFiliacion"].ToString());
                                    Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                    //Context.Items.Add("Desde", "Valida");
                                    //Server.Transfer("~/CasoFiliacion/CasoResultado.aspx");

                                    Response.Redirect("~/casoFiliacion/CasoResultado3.aspx?id=" + Request["idCasoFiliacion"].ToString() + "&Desde=Valida&logIn=1");


                                }
                                if ((Request["idServicio"].ToString() == "3") && (VerificarSiTienePermisodeValidar(oUser.Username, "/CasoFiliacion/CasoResultadoHisto.aspx")))
                                {
                                    //HttpContext Context2;
                                    //Context2 = HttpContext.Current;
                                    //Context2.Items.Add("id", Request["idCasoFiliacion"].ToString());
                                    Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                    //Context2.Items.Add("Desde", "Valida");
                                    //Server.Transfer("~/CasoFiliacion/CasoResultadoHisto.aspx");

                                    Response.Redirect("~/CasoFiliacion/CasoResultadoHisto.aspx?id=" + Request["idCasoFiliacion"].ToString() + "&Desde=Valida&logIn=1", false);

                                }

                            }
                            else
                            {
                                string idServicio = Request["idServicio"].ToString();
                                string operacion = Request["Operacion"].ToString();
                                string modo = Request["modo"].ToString();
                                if (VerificarSiTienePermisodeValidar(oUser.Username, "/Resultados/ResultadoBusqueda.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo))
                                {
                                    Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                    if (Request["urgencia"] != null)
                                    {

                                        string sredirect = "~/resultados/ResultadoEdit2.aspx?idServicio=1&Operacion=Valida&idProtocolo=" + Request["idProtocolo"].ToString() + "&Index=0&Parametros=" + Request["idProtocolo"].ToString() + "&idArea=0&urgencia=1&validado=0&modo=Urgencia"; //&idUsuarioValida=" + oUser.IdUsuario.ToString();
                                        if (Request["desde"] != null)
                                            sredirect += "&desde=" + Request["desde"].ToString();
                                        Response.Redirect(sredirect, false);
                                        //    Response.Redirect("~/Resultados/ResultadoBusqueda.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo + "&idUsuarioValida=" + oUser.IdUsuario, false);
                                    }
                                    else
                                        Response.Redirect("~/Resultados/ResultadoBusqueda.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo + "&logIn=1", false);// + "&idUsuarioValida=" + oUser.IdUsuario, false);
                                }
                                else
                                {
                                    e.Authenticated = false;
                                    Login1.FailureText = "El usuario no tiene permisos para validar.";
                                }
                            }
                        }//fin else casofi
                    }//externo
                }//activo
                else
                {
                    e.Authenticated = false;
                    Login1.FailureText = "El usuario no está activo por inactividad.";
                }
            }
            else
            {
                e.Authenticated = false;
                Login1.FailureText = "El usuario y/o contraseña no son correctos.";
            }

            lblMensajeError.Text = Login1.FailureText;
            Login1.FailureText = ""; //Muestro solo lblMensajeError, ya que Login1.FailureText solo sirve para Login1_Authenticate y no para btn_aceptarTerminosCondiciones_Click
        }

        private bool VerificarTipoAutenticacion(Usuario oUser,  out string mensajeError)
        {  
            mensajeError = "";
            bool autentica = false;
          
            if (oUser != null)
            { //verifica el tipo de autenticacion
                
                string tipoAutenticacion = oUser.TipoAutenticacion.ToUpper().Trim();
                if (tipoAutenticacion == "SIL")
                {
                  
                    Utility oUtil = new Utility();
                    string m_password = oUtil.Encrypt(Login1.Password);

                    if (oUser.Password != m_password)
                    {
                        autentica = false;
                        mensajeError = "Usuario o contraseña incorrecta en SIL.";
                    }
                    else
                    {
                        autentica = true;                    
                        Session["FechaLogin"] = DateTime.Now;                    
                    }

                }

                else if (tipoAutenticacion == "ONELOGIN")
                {
                    try
                    {
                        // Create the new LDAP connection
                        LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier("ldap.neuquen.gov.ar", 389);
                        System.DirectoryServices.Protocols.LdapConnection ldapConnection =
                            new System.DirectoryServices.Protocols.LdapConnection(ldi);                        
                        ldapConnection.AuthType = AuthType.Basic;
                        ldapConnection.SessionOptions.ProtocolVersion = 3;
                        NetworkCredential nc = new NetworkCredential("uid=" + Login1.UserName + ",ou=people,O=integrabilidad,O=neuquen", Login1.Password);
                        ldapConnection.Bind(nc);                        
                        ldapConnection.Dispose();

                        // Autenticación correcta
                        string hash = GenerarSHA256(Login1.Password);

                        // Insertar o actualizar
                        GuardarCredencialContingencia(oUser.IdUsuario, hash);                       
                        Session["FechaLogin"] = DateTime.Now;                       
                        autentica = true;

                    }
                    catch (LdapException ex)
                    {
                        ////"Servicio LDAP no disponible"
                        //     autentica = false;
                        //   mensajeError = "Error de autenticación ONELOGIN: " + ex.Message;
                        // LDAP caído
                        switch (ex.ErrorCode)
                        {
                            case 49:
                                // Usuario o contraseña incorrecta
                                autentica = false;
                                mensajeError = "Usuario o contraseña incorrecta en ONELOGIN.";
                                break;


                            case 81:
                                // LDAP caído
                                if (ValidarContingencia(oUser.IdUsuario, Login1.Password))
                                {
                                    autentica = true;                                 
                                    Session["FechaLogin"] = DateTime.Now;                                 
                                    oUser.GrabaAuditoria(
                                        "LOGIN CONTINGENCIA LDAP",
                                        oUser.IdUsuario,
                                        oUser.Username,
                                        "",
                                        "LDAP caído");
                                }
                                else
                                {
                                    autentica = false;
                                    mensajeError =
                                        "El servicio ONELOGIN no se encuentra disponible.";
                                }

                                break;


                            default:
                                autentica = false;
                                mensajeError =
                                    "Error LDAP: " + ex.Message;
                                break;
                        }


                    }
                    catch (Exception ex)
                    {                                       
                        autentica = false;
                        mensajeError = "Error general ONELOGIN: " + ex.Message;
                    }
                }
                else //ni sil no onlogin
                {
                 
                    /// "Tipo de autenticación no soportado.";               
                    autentica = false;
                    mensajeError =    "Tipo de autenticación no soportado: " + tipoAutenticacion;
                }
            }
            return autentica;
           
        }


        private void GuardarCredencialContingencia(int idUsuario, string password)
        {
            string hash = password;

            string conn = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(conn))
            {
                cn.Open();

                string sql = @"
IF EXISTS (SELECT 1
           FROM Sys_OneLoginContingencia
           WHERE IdUsuario = @IdUsuario)
BEGIN
    UPDATE Sys_OneLoginContingencia
       SET PasswordHash = @PasswordHash,
           FechaUltimaAutenticacion = GETDATE(),
           FechaActualizacion = GETDATE()
     WHERE IdUsuario = @IdUsuario
END
ELSE
BEGIN
    INSERT INTO Sys_OneLoginContingencia
        (IdUsuario, PasswordHash, FechaUltimaAutenticacion, FechaActualizacion)
    VALUES
        (@IdUsuario, @PasswordHash, GETDATE(), GETDATE())
END";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", hash);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private bool ValidarContingencia(int idUsuario, string password)
        {
            string hash = GenerarSHA256(password);

            string conn = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(conn))
            {
                cn.Open();

                string sql = @"
            SELECT COUNT(*)
            FROM Sys_OneLoginContingencia
            WHERE IdUsuario = @IdUsuario
              AND PasswordHash = @PasswordHash
              AND FechaUltimaAutenticacion >= DATEADD(HOUR,-24,GETDATE())";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", hash);

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }
        public static string GenerarSHA256(string texto)
        {
            SHA256 sha = new SHA256Managed();
            byte[] bytes = Encoding.UTF8.GetBytes(texto);
            byte[] hash = sha.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }
       
        protected void btn_aceptarTerminosCondiciones_Click(object sender, EventArgs e)
        {
            if(Session["usuarioPendienteAceptacion"] == null)
            {
                Response.Redirect("Logout.aspx", true);
                return;
            }

            int i_idusuario = int.Parse(Session["usuarioPendienteAceptacion"].ToString());
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), i_idusuario);

            if (oUser != null)
            {
                Session["usuarioPendienteAceptacion"] = null;
                //Actualizar la fecha en la bd
                oUser.FechaAceptaTerminosCondiciones = DateTime.Now;
                oUser.Save();
                //Actualizo acceso en log
                CrearLogAcceso(oUser);
                //Ingreso al sistema
                AuthenticateEventArgs evento = new AuthenticateEventArgs(true);
                IngresoSistema(oUser, evento);
            }
            else
            {
                Response.Redirect("Logout.aspx", true);
            }
        }
        private void CrearLogAcceso(Usuario oUser)
        {
            LogAccesoTerminosCondiciones RegistroAcceso = new LogAccesoTerminosCondiciones();
            RegistroAcceso.IdUsuario = oUser.IdUsuario;
            RegistroAcceso.Fecha = DateTime.Now;
            RegistroAcceso.Save();
        }

        private bool MostrarTerminosCondiciones(Usuario oUser)
        {
            /*
             PARAMETRIZACION:
             Cantidad X de dias
                > 0 --> Muestra "Terminos y Condiciones" dado X dias
               == 0 --> Muestra Siempre "Terminos y Condiciones"
                < 0 --> NO muestra "Terminos y Condiciones"
             */
            int dias = Convert.ToInt32(ConfigurationManager.AppSettings["DiasTerminosCondiciones"]);
            if(dias > 0)
            {
                DateTime ultimaFecha = oUser.FechaAceptaTerminosCondiciones;
                DateTime hoy = DateTime.Now;

                TimeSpan diferencia = hoy.Subtract(ultimaFecha);
                int diferenciasDias = diferencia.Days;

                if (diferenciasDias > dias)
                    return true;
                else
                    return false;
            }
            else
            {
               if(dias == 0) return true;
                else return false;
            }
            
          
        }
    }
}