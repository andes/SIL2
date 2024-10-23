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
    public partial class FinSesion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["SIL"] != null)
            //Response.Redirect("Logout.aspx", false);

            if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")

                Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx?relogin=1", "login.aspx");
            else
                Response.Redirect("Logout.aspx", false);
        }

        protected void btnSIPS_Click(object sender, EventArgs e)
        {
            Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx?relogin=1", "login.aspx");
        }

        protected void btnLaboCentral_Click(object sender, EventArgs e)
        {
            Response.Redirect("logout.aspx", false);
        }
    }
}
