using Business;
using Business.Data;
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
    public partial class LoginEfector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuarioAux"] != null)
            {
                if (!Page.IsPostBack)
                {

                    Usuario oRegistro = new Usuario();
                    oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuarioAux"].ToString()));
                    lblUsuario.Text = oRegistro.Username;
                    lblNombre.Text = oRegistro.Nombre + " " + oRegistro.Apellido;

                    if (oRegistro.IdPerfil.IdPerfil == 15)  ///administrativo externo
                    {
                        Session["idUsuario"] = oRegistro.IdUsuario.ToString();
                        Response.Redirect("Default.aspx", false);
                    }
                    else
                        CargarListas(oRegistro);
                }
            }
            else Response.Redirect("FinSesion.aspx", false);

       
        }

         
        private void CargarListas(Usuario oRegistro)
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            Utility oUtil = new Utility();

            string m_ssql = @"  SELECT idEfector, nombre FROM Sys_Efector (nolock)
where idEfector in (select idEfector from sys_UsuarioEfector (nolock) where activo=1 and idUsuario=" + oRegistro.IdUsuario.ToString()+ @") 
and  idEfector in (select idEfector from lab_configuracion (nolock) ) ORDER BY nombre ";

            if (oRegistro.Administrador)
                m_ssql = @"  SELECT idEfector, nombre FROM Sys_Efector (nolock)
where idEfector in (select idEfector from lab_configuracion (nolock) ) 
UNION
SELECT idEfector, nombre FROM Sys_Efector (nolock) WHERE IDEFECTOR=227
ORDER BY nombre";


            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);


            if (ddlEfector.Items.Count == 0)
                Response.Redirect("AccesoDenegado.aspx?mensaje=Efector no Habilitado. Verifique con el Administrador.", false);
            else
            {
                if (ddlEfector.Items.Count > 1)
                    ddlEfector.Items.Insert(0, new ListItem("--Seleccione un efector--", "0"));
                else

                {
                    Efector oEfector = new Efector();
                    oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));

                    

                    oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuarioAux"].ToString()));
                    oRegistro.IdEfector = oEfector;
                    oRegistro.Save();

                    Session["idUsuario"] = oRegistro.IdUsuario.ToString();
                    Response.Redirect("Default.aspx", false);
                    //Response.Redirect("Default.aspx?IdUsuario=" + oRegistro.IdUsuario.ToString(), false);
                }
            }

            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ddlEfector.SelectedValue != "0")
                {
                    if (Session["idUsuarioAux"] != null)
                    {
                        Efector oEfector = new Efector();
                        oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));

                        Usuario oRegistro = new Usuario();
                        oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuarioAux"].ToString()));
                        oRegistro.IdEfector = oEfector;
                        oRegistro.Save();
                        Session["idUsuario"] = oRegistro.IdUsuario.ToString();
                        Response.Redirect("Default.aspx", false);
                        // Response.Redirect("Default.aspx?IdUsuario=" + oRegistro.IdUsuario.ToString(), false);
                    }
                    else Response.Redirect("FinSesion.aspx", false);
                }
            }
        }
    }
}
