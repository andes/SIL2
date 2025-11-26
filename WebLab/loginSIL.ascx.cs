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
                    if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")
                        Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx?relogin=1", "login.aspx");
                }
            }
        
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            Utility oUtil = new Utility();
            string m_password = oUtil.Encrypt(Login1.Password);


            int i_idusuario = 0;
            string m_strSQL = @" SELECT top 1 idUsuario FROM Sys_usuario with (nolock)  WHERE (username = '" + Login1.UserName + "') AND ([password] = '" + m_password + "')  ";

            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            DataTable dtPermisos = Ds.Tables[0];

            if (dtPermisos.Rows.Count > 0) {
                i_idusuario = int.Parse(dtPermisos.Rows[0][0].ToString());
            }


            Usuario oUser = new Usuario();
            //oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Login1.UserName, "Password", m_password);
            if (i_idusuario > 0)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), i_idusuario);
                if (MostrarTerminosCondiciones(oUser))
                {
                    Session["usuarioPendienteAceptacion"] = oUser;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarModal", "$('#modalTerminosCondiciones').modal('show');", true);
                    return;
                }
                
            }
            else
            {
                oUser = null;
                e.Authenticated = false;
                Login1.FailureText = "El usuario y/o contraseña no son correctos.";
            }


            IngresoSistema(oUser, e);
        }


        private void AutenticarUsuarioLdap(string username, string key)

        {
            ////   string path = @"LDAP://ldap.neuquen.gov.ar:389";
            ////string path = "LDAP://OU=People,O=integrabilidad,O=neuquen, DC=ldap.neuquen.gov.ar";
            //string path = @"LDAP://ldap.neuquen.gov.ar:389/OU=People,O=integrabilidad,O=neuquen";


            //DirectoryEntry ds = new DirectoryEntry(path, "26982063", "bc*123456", AuthenticationTypes.Secure);
            //DirectorySearcher dssearch = new DirectorySearcher(ds);
            //SearchResult result = dssearch.FindOne();

            try
            {
                // Create the new LDAP connection
                LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier("ldap.neuquen.gov.ar", 389);
                System.DirectoryServices.Protocols.LdapConnection ldapConnection =
                    new System.DirectoryServices.Protocols.LdapConnection(ldi);
                Console.WriteLine("LdapConnection is created successfully.");
                ldapConnection.AuthType = AuthType.Basic;
                ldapConnection.SessionOptions.ProtocolVersion = 3;
                NetworkCredential nc = new NetworkCredential("uid=" + username + ",ou=people,O=integrabilidad,O=neuquen",
                    key); // "bc*1213456"); //password
                ldapConnection.Bind(nc);
                //   ldapConnection.pr

                VerificarMatriculacion(username, key);
                Console.WriteLine("LdapConnection authentication success");
                ldapConnection.Dispose();
            }
            catch (LdapException e)
            {
                Console.WriteLine("\r\nUnable to login:\r\n\t" + e.Message);
                VerificarMatriculacion(username, key);
            }
            catch (Exception e)
            {
                Console.WriteLine("\r\nUnexpected exception occured:\r\n\t" + e.GetType() + ":" + e.Message);
            }

        }

        public void VerificarMatriculacion(string username, string contra)
        {
            //HttpContext CurrContext = HttpContext.Current;
            //CurrContext.Items.Add("Documento", "20321008");
            //CurrContext.Items.Add("Clave", "20321008");
            //CurrContext.Items.Add("Apellido", "RESCHIA");
            //CurrContext.Items.Add("Nombre", "SANDRA CEFERINA");
            //CurrContext.Items.Add("Titulo", "MEDICA CIRUJANA");
            //Server.Transfer("DatosPersona.aspx");


            Session["Documento"] = "20321008";
            Session["Clave"] = "20321008";
            Session["Apellido"] = "RESCHIA";
            Session["Nombre"] = "SANDRA CEFERINA";
            Session["Titulo"] = "MEDICA CIRUJANA";

            Response.Redirect("DatosPersona.aspx", false);


            /*   Configuracion oC = new Configuracion();


               oC = (Configuracion)oC.Get(typeof(Configuracion), 1);// "IdEfector", oEfector);

               string s_urlWFC = oC.UrlMatriculacion;
               string s_url = s_urlWFC + "documento=" + username; // + "&codigoProfesion in (1,23)";

               HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s_url);
               HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
               JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
               Stream st = ws1.GetResponseStream();
               StreamReader sr = new StreamReader(st);

               string s = sr.ReadToEnd();
               if (s != "0")
               {

                   List<Protocolos.ProtocoloEdit2.ProfesionalMatriculado> pro = jsonSerializer.Deserialize<List<Protocolos.ProtocoloEdit2.ProfesionalMatriculado>>(s);
                   string espe;
                   if (pro.Count > 0)
                   {

                       for (int i = 0; i < pro.Count; i++)
                       {
                           espe = pro[i].apellido + " " + pro[i].nombre + " - " + pro[i].profesiones[0].titulo;
                           HttpContext CurrContext = HttpContext.Current;
                           CurrContext.Items.Add("Documento", username);
                           CurrContext.Items.Add("Clave", contra);
                           CurrContext.Items.Add("Apellido", pro[i].apellido);
                           CurrContext.Items.Add("Nombre", pro[i].nombre);
                           CurrContext.Items.Add("Titulo", pro[i].profesiones[0].titulo);
                           Server.Transfer("DatosPersona.aspx");


                       }




                   }
                   else
                   { //error no encontrado}



                   } // procount
               }//s!=0
               */
        }

        private bool VerificarSiTienePermisodeValidar(string user, string m_url)
        {
            bool b_permiso = false;
            string m_strSQL = @" SELECT   P.permiso, M.objeto, M.url, U.username
            FROM         Sys_Menu AS M INNER JOIN
            Sys_Permiso AS P ON M.idMenu = P.idMenu INNER JOIN
            Sys_Usuario AS U ON P.idPerfil = U.idPerfil
            WHERE     (M.url = '" + m_url + "') AND (U.username = '" + user + "') AND (P.permiso = 2) and  (U.activo=1 ) ";

            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {
                DataSet Ds = new DataSet();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];

                if (dtPermisos.Rows.Count > 0) b_permiso = true;
                conn.Close();
                return b_permiso;
            }
        }

        private void IngresoSistema(Usuario oUser, AuthenticateEventArgs e)
        {
            
            /* Habilitar cuando se quiera autogestion de usuarios medicos
            if (oUser == null)
                AutenticarUsuarioLdap(Login1.UserName, Login1.Password);
             */
            if (oUser != null)
            {
                if ((oUser.Activo) && (oUser.IdPerfil.Activo))
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
                                Response.Redirect("~/Usuarios/PasswordEdit2.aspx?idUsuario=" + oUser.IdUsuario.ToString(), false);

                            else
                            {
                                Session["idUsuarioAux"] = oUser.IdUsuario.ToString();
                                Response.Redirect("LoginEfector.aspx", false);

                                //Session["idUsuario"] = oUser.IdUsuario.ToString();
                                //Response.Redirect("Default.aspx", false);
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
        }
        protected void btn_aceptarTerminosCondiciones_Click(object sender, EventArgs e)
        {
            Usuario oUser = (Usuario) Session["usuarioPendienteAceptacion"];

            if (oUser != null)
            {
                Session["usuarioPendienteAceptacion"] = null;
                //Actualizar la fecha en la bd
                oUser.FechaAceptaTerminosCondiciones = DateTime.Now;
                oUser.Save();
                //Actualizo acceso en log
                CrearLogAcceso(oUser);
                //Ingreso al sistema
                AuthenticateEventArgs evento = new AuthenticateEventArgs();
                IngresoSistema(oUser, evento);
            }
            else
            {
                // Fallback: recargar página o mostrar error
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "alert('❌ No hay dias definidos para los terminos y condiciones');", true);
                return true; 
            }
            
          
        }
    }
}