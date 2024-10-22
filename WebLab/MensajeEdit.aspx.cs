using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data.Laboratorio;
using Business.Data;

namespace WebLab
{
    public partial class MensajeEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if (Request["Operacion"].ToString() == "Alta")
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));//Session["idUsuario"].ToString());
                    txtDe.Text = oUser.Nombre + " " + oUser.Apellido;
                }
                if (Request["Operacion"].ToString() == "Eliminar")
                {
                    MensajeInterno oRegistro = new MensajeInterno();
                    oRegistro = (MensajeInterno)oRegistro.Get(typeof(MensajeInterno), int.Parse(Request["idMensaje"].ToString()));//Session["idUsuario"].ToString()); 
                    oRegistro.Delete();
                    Response.Redirect("Principal.aspx", false);
                }
            }
            else Response.Redirect("FinSesion.aspx", false);
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GuardarMensaje();
                Response.Redirect("Principal.aspx");
            }
        }

        private void GuardarMensaje()
        {
            MensajeInterno oRegistro = new MensajeInterno();
            oRegistro.Remitente = txtDe.Text;
            oRegistro.Destinatario = txtPara.Text;
            oRegistro.Mensaje = txtMensaje.Text;
            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaHoraRegistro = DateTime.Now;
            oRegistro.Save();
        }
    }
}