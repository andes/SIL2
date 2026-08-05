using Business;
using Business.Data;
using Business.Data.Laboratorio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebLab.Protocolos
{
    public partial class ObraSocialBuscar : System.Web.UI.Page
    {
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null) 
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["obraSocial"] = null;
                CargarListas();
            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura
            string  m_ssql = "select distinct nombreObraSocial as nombre, cod_os from LAB_Protocolo with (nolock)  where baja=0 and idEfector=" + oUser.IdEfector.IdEfector.ToString() + " order by nombreObraSocial ";
            oUtil.CargarCombo(ddlObrasSociales, m_ssql, "cod_os", "nombre", connReady);
            ddlObrasSociales.Items.Insert(0, new ListItem("--Seleccione una obra social --", "0"));

        }
        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            string script = "";

            if (ddlObrasSociales.SelectedValue != "0")
            {
                Session["obraSocial"] = ddlObrasSociales.SelectedItem.Text;
                Session["idObraSocial"] = ddlObrasSociales.SelectedItem.Value;
                // Script para cerrar y forzar postback en el padre
                script = @"
                (function() {
                    // Cerrar el diálogo
                    window.parent.$('.ui-dialog-content').dialog('close');
                    // Forzar postback 
                    window.parent.postBackBuscarObraSocial();
                })();
                 ";
            }
            else
            {
                Session["obraSocial"] = null;

                // Script para cerrar sin postback 
                script = @"
                (function() {
                    // Cerrar el diálogo
                    window.parent.$('.ui-dialog-content').dialog('close');
                   
                })();
            ";
            }

            ClientScript.RegisterStartupScript(
            GetType(),
            "Cerrar",
            script,
            true);

        }
    }
}