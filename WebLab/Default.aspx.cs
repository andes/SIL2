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
using Business.Data.Laboratorio;
using Business.Data;
using System.Data.SqlClient;
using Business;
using System.Security.Principal;
using Salud.Security.SSO;

namespace WebLab
{
    public partial class Default : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Utility oUtil = new Utility();
                //string m_password = oUtil.Decrypt("5XNGNHdEH1LLmyHjFa5ENg==");

                if (Session["SIL"] != null)
                    IdentificarSIL();
                else
                {
                    if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")
                        IdentificarSSO();
                    else IdentificarSIL();
                }

              

            }          
        }
     
        private void IdentificarSIL()
        {
            if (Session["idUsuario"] == null)
                Response.Redirect("logout.aspx", false);
            else
            {
                int idUsuarioLogueado = int.Parse(Session["idUsuario"].ToString());
                //if (Session["idUsuario"] == null)
                //{
                //    Session["idUsuario"] = idUsuarioLogueado;
                //}

                //Session["idEfectorSolicitante"] = "228";
                //Response.Redirect("PeticionElectronica/PeticionLC.aspx", false);


                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oUser != null)
                {
                    Session["idEfectorSolicitante"] = oUser.IdEfector.IdEfector.ToString();
                    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                    oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdConfiguracion", 1);
                    //CrearLogAcceso(idUsuarioLogueado);
                /*    if (oUser.IdEfector.IdEfector.ToString() == "227")
                        CrearPermisosAdministrador(idUsuarioLogueado);
                    else*/
                    CrearPermisos(idUsuarioLogueado);
                    if (oUser.IdEfector != oCon.IdEfector) ///externo para pruebas sin sso                
                    {
                        //if (oUser.IdPerfil.Nombre == "Externo")
                        switch (oUser.IdPerfil.Nombre)
                        {
                            case "Externo":
                                Response.Redirect("PeticionElectronica/ResultadosBusqueda.aspx", false); break;
                            case "Consulta":
                                Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;
                            case "Bioquimicos SSS":
                                Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;
                            case "Administrativo Externo":  ///usuarios que dan turnos desde afuera en los laboratorios
                                Response.Redirect("PrincipalTurnos.aspx", false); break;
                            default:
                                Response.Redirect("Principal.aspx", false); break;
                        }
                    }
                    else
                    {      //if (oUser.IdPerfil.Nombre == "Externo")
                        switch (oUser.IdPerfil.Nombre)
                        {
                            case "Externo":
                                Response.Redirect("PeticionElectronica/ResultadosBusqueda.aspx", false); break;
                            case "Consulta":
                                Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;
                            case "Consulta Histo":
                                Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;

                            default:
                                Response.Redirect("Principal.aspx", false); break;
                        }
                    }
                    //Response.Redirect("Principal.aspx", false);
                    //Response.Redirect("Estadisticas/reportemicrobiologianopaciente.aspx", false);

                }
            }

        }

        private void IdentificarSSO()
        {
            ///////Simula Log In//////
            //string sessionId = "11622697";
            //HttpContext.Current.User = new GenericPrincipal(new Salud.Security.SSO.SSOIdentity(new HttpCookie("SSO_AUTH_COOKIE", sessionId)), null);
            //////////////////////////////            
            int idUsuarioLogueado =                 IdentificarUsuarioSSO();
            if (idUsuarioLogueado == 0)
                Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Login.aspx", Request.Url.ToString());
            else
            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdConfiguracion", 1);
                ////validacion de usuario externo

              
                //if (Session["idEfectorSolicitante"].ToString() != oCon.IdEfector.IdEfector.ToString())  // es un usuario externo 
                //{
                //Usuario oUser = new Usuario();
                // oUser = oUser.buscarUsuarioLocal(Salud.Security.SSO.SSOHelper.CurrentIdentity.Username, Salud.Security.SSO.SSOHelper.CurrentIdentity.Surname, Salud.Security.SSO.SSOHelper.CurrentIdentity.FirstName, oCon.IdEfector);
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Salud.Security.SSO.SSOHelper.CurrentIdentity.Username);
                if (oUser != null)
                    {
                    Session["idEfectorSolicitante"] = oUser.IdEfector.IdEfector.ToString();
                    idUsuarioLogueado = oUser.IdUsuario;
                    //    CrearLogAcceso(idUsuarioLogueado);
                        CrearPermisos(idUsuarioLogueado);
                        Session["idUsuario"] = idUsuarioLogueado;

                    switch (oUser.IdPerfil.Nombre)
                    {
                        case "Externo":
                            Response.Redirect("PeticionElectronica/ResultadosBusqueda.aspx", false); break;
                        case "Consulta":
                            Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;
                        case "Consulta Histo":
                            Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;
                        case "Bioquimicos SSS":
                            Response.Redirect("Resultados/ResultadosPanel.aspx", false); break;
                        default:
                            Response.Redirect("Principal.aspx", false); break;
                    }

                    //if (oUser.IdPerfil.Nombre=="Externo")
                    //    Response.Redirect("PeticionElectronica/PeticionLC.aspx", false);
                    //else
                    //    Response.Redirect("Principal.aspx", false);   //caso de externo pero de salud publica
                }
                    else
                        Response.Redirect("AccesoDenegado.htm");
                //}
                ////

                //else
                //{ 
                //    /// usuario interno del laboratorio 
                //    Usuario oUser = new Usuario();
                //    oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Salud.Security.SSO.SSOHelper.CurrentIdentity.Username);
                //    if (oUser != null)
                //    {
                //        idUsuarioLogueado = oUser.IdUsuario;
                //    CrearLogAcceso(idUsuarioLogueado);
                //    CrearPermisos(idUsuarioLogueado);
                //    Session["idUsuario"] = idUsuarioLogueado;
                //    Response.Redirect("Principal.aspx", false);
                //    }
                //    else
                //        Response.Redirect("AccesoDenegado.htm");
                //    }
            }
                
        }


        private void IdentificarSSO_new()
        {
            ///////Simula Log In//////
            //string sessionId = "26136062";
            //HttpContext.Current.User = new GenericPrincipal(new Salud.Security.SSO.SSOIdentity(new HttpCookie("SSO_AUTH_COOKIE", sessionId)), null);
            ////////////////////////////            
            int idUsuarioLogueado =  IdentificarUsuarioSSO();
            if (idUsuarioLogueado == 0)
                Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Login.aspx", Request.Url.ToString());
            else
            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdConfiguracion", 1);
                ////validacion de usuario externo
            
                    Usuario oUser = new Usuario();

                oUser = oUser.buscarUsuarioLocal(Salud.Security.SSO.SSOHelper.CurrentIdentity.Username, Salud.Security.SSO.SSOHelper.CurrentIdentity.Surname, Salud.Security.SSO.SSOHelper.CurrentIdentity.FirstName, oCon.IdEfector);
                //oUser = oUser.buscarUsuarioLocal("26136062", Salud.Security.SSO.SSOHelper.CurrentIdentity.Surname, Salud.Security.SSO.SSOHelper.CurrentIdentity.FirstName, oCon.IdEfector);

                if (oUser != null)
                    {
                        idUsuarioLogueado = oUser.IdUsuario;
                      //  CrearLogAcceso(idUsuarioLogueado);
                   if ( oUser.IdEfector.IdEfector.ToString()=="227")

                        CrearPermisosAdministrador(idUsuarioLogueado);
                   else
                    CrearPermisos(idUsuarioLogueado);
                        Session["idUsuario"] = idUsuarioLogueado;
                        if (oUser.IdPerfil.Nombre == "Externo")
                            Response.Redirect("PeticionElectronica/PeticionLC.aspx", false);
                        else
                            Response.Redirect("Principal.aspx", false);   //caso de externo pero de salud publica
                    }
                    else
                        Response.Redirect("AccesoDenegado.htm");
               

            }

        }

        private void CrearPermisosAdministrador(int p)
        {
            string m_strSQL = " SELECT  M.objeto, P.permiso " +
                            " FROM Sys_Perfil " +
                            " INNER JOIN   Sys_Usuario AS U ON Sys_Perfil.idPerfil = U.idPerfil " +
                            " INNER JOIN   Sys_Permiso AS P ON Sys_Perfil.idPerfil = P.idPerfil " +
                            " INNER JOIN   Sys_Menu AS M ON P.idMenu = M.idMenu " +
                            " WHERE   (M.idModulo = 2) and  (U.activo=1 )  AND (U.idUsuario =" + p.ToString() + ") and M.habilitado=1 "; // M.idMenuSuperior in (174,197)";
            //M.esaccion = 0 and
            //using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))  ///Performance: conexion de solo lectura)
            {
                DataSet Ds = new DataSet();
                //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];
                ArrayList l_permisos;
                l_permisos = new ArrayList();
                foreach (DataRow dr in dtPermisos.Rows)
                {
                    l_permisos.Add(dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString());
                    Session["s_permiso"] = l_permisos;
                }
                conn.Close();
                conn.Dispose();
            }
        }

        private int IdentificarUsuarioSSO()
        {
            Salud.Security.SSO.SSOHelper.Authenticate();
            if (Salud.Security.SSO.SSOHelper.CurrentIdentity == null)
            {
                // 1.1. No lo está. Debe redirigir al sitio de SSO
                // Redirigir ...
                return 0;
                //pnlSinUsuario.Visible = true;
            }
            else
            {
                return Salud.Security.SSO.SSOHelper.CurrentIdentity.Id;

            }
        }
        //private int IdentificarUsuario()
        //{
        //    if (Salud.Security.SSO.SSOHelper.CurrentIdentity == null)
        //    {
        //        // 1.1. No lo está. Debe redirigir al sitio de SSO
        //        // Redirigir ...
        //        Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Login.aspx", Request.Url.ToString());
        //        //pnlSinUsuario.Visible = true;
        //    }
        //    else
        //    {
        //        // 1.2 El usuario está loggeado. Obtiene todos los datos
        //        pnlUsuario.Visible = true;
        //        lblID.Text = Salud.Security.SSO.SSOHelper.CurrentIdentity.Id.ToString();
        //        lblNombre.Text = Salud.Security.SSO.SSOHelper.CurrentIdentity.Fullname;

        //        // Verifica el perfil
        //        object perfil = Salud.Security.SSO.SSOHelper.CurrentIdentity.GetSetting("Perfil_Laboratorio");
        //        lblPerfil.Text = (perfil == null) ? "Sin perfil asignado" : perfil.ToString();

        //        // Para simular un cambio de perfil, quitar el comentario de la siguiente línea
        //        //Salud.Security.SSO.SSOHelper.CurrentIdentity.SetSetting("Perfil_Laboratorio", "Bioquímico");
        //    }
        //}

        //private void CrearLogAcceso(int idUsuarioLogueado)
        //{
        //    LogAcceso RegistroAcceso = new LogAcceso();
        //    RegistroAcceso.IdUsuario = idUsuarioLogueado;
        //    RegistroAcceso.Fecha = DateTime.Now;
        //    RegistroAcceso.Save();
        //}

        private void CrearPermisos(int p)
        {

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_LoginUsuario]";

            cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
            cmd.Parameters["@idUsuario"].Value = p;

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);


            //string m_strSQL = " SELECT  M.objeto, P.permiso " +
            //                  " FROM Sys_Perfil " +
            //                  " INNER JOIN   Sys_Usuario AS U ON Sys_Perfil.idPerfil = U.idPerfil " +
            //                  " INNER JOIN   Sys_Permiso AS P ON Sys_Perfil.idPerfil = P.idPerfil " +
            //                  " INNER JOIN   Sys_Menu AS M ON P.idMenu = M.idMenu " +
            //                  " WHERE  (M.habilitado = 1)  AND (M.idModulo = 2) and  (U.activo=1 )  AND (U.idUsuario =" + p.ToString() + ")";
            ////M.esaccion = 0 and
            ////using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))  ///Performance: conexion de solo lectura)
            //{
            //    DataSet Ds = new DataSet();
            //    //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //    SqlDataAdapter adapter = new SqlDataAdapter();
            //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            //    adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];
                ArrayList l_permisos;
                l_permisos = new ArrayList();
                foreach (DataRow dr in dtPermisos.Rows)
                {
                    l_permisos.Add(dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString());
                    Session["s_permiso"] = l_permisos;
                }
            //    conn.Close();
            //    conn.Dispose();
            //}

          //  LlenarTablaMenu(p);
            
        }

        //private void LlenarTablaMenu(int idUsuarioLogueado)
        //{
        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "[LAB_LoginUsuario]";

        //    cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
        //    cmd.Parameters["@idUsuario"].Value = idUsuarioLogueado;

        //    cmd.Connection = conn;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(Ds);
           
        //}
    }
}
