using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.AutoAnalizador
{
    public partial class Mensaje : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.lblMensaje.Text = "Se han procesado " + Request["Cantidad"].ToString() + " protocolos ";

        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportaDatos.aspx?Equipo="+ Request["Equipo"].ToString(), false);
        }
    }
}