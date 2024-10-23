using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Business.Data;

namespace WebLab.Usuarios
{
    public partial class Site2 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO") IdentificarSSO();
                
            }    
        }


            private void IdentificarSSO()
            {
                ///////Simula Log In//////
                //string sessionId = "admin";
                //HttpContext.Current.User = new GenericPrincipal(new Salud.Security.SSO.SSOIdentity(new HttpCookie("SSO_AUTH_COOKIE", sessionId)), null);
                ////////////////////////////

                int idUsuarioLogueado = IdentificarUsuarioSSO();
                if (idUsuarioLogueado == 0)
                    Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Login.aspx", Request.Url.ToString());
                else
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Salud.Security.SSO.SSOHelper.CurrentIdentity.Username);
                    if (oUser != null)
                    {
                        idUsuarioLogueado = oUser.IdUsuario;                                   
                        Session["idUsuario"] = idUsuarioLogueado;                   
                    }
                    else
                        Response.Redirect("AccesoDenegado.htm");
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

   

        
    }
}