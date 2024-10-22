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

namespace WebLab
{
    public partial class CerrarSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")
            {
                string url = HttpContext.Current.Request.QueryString["url"];
                if (string.IsNullOrEmpty(url))
                    url = Salud.Security.SSO.SSOHelper.Configuration["StartPage"] as string;

                string strsso = Salud.Security.SSO.SSOHelper.Configuration["Publicacion_SSO"] as string;
             Response.Redirect(   "/" + strsso + "/Logout.aspx?relogin=1&url=" + url);
                //Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx", "login.aspx");
                //Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx", "login.aspx", true);
            }
            else
                Response.Redirect("FinSesion.aspx", false);

        }
    }
}
