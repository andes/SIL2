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
    public partial class AccesoDenegado : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {

         
            if (Request["master"] != null)
                Page.MasterPageFile = "~/Site1.master";
            else
                Page.MasterPageFile = "~/Site2.master";


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblMensaje.Text = "";
                if (Request["mensaje"] != null)
                {
                    lblMensaje.Text = Request["mensaje"].ToString();
                }
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Principal.aspx");
        }
    }
}
