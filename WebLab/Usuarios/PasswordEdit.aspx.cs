using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data;
using Business;
using System.Collections;
using Business.Data.Laboratorio;

namespace WebLab.Usuarios
{
    public partial class PasswordEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            if (oUser.IdEfector != oC.IdEfector) // es externo
            {
                this.MasterPageFile = "~/PeticionElectronica/SitePE.master";

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                Configuracion oC = new Configuracion();
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
                if (oUser.IdEfector != oC.IdEfector) // es externo
                    VerificaPermisos("Validacion Externo");

                else
                    VerificaPermisos("Validacion");
           
                if (Request["id"] != null)
                    MostrarDatos();
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuarioValida"] == null) Response.Redirect("../FinSesion.aspx");
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    if (i_permiso == 0) Response.Redirect("../AccesoDenegado.aspx", false);

                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MostrarDatos()
        {
          
            Usuario oRegistro = new Usuario();
            oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["id"]));            

            lblUsuario.Text = oRegistro.Username;
            

         
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Usuario oRegistro = new Usuario();
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["id"]));
               

                    oRegistro.Password = oUtil.Encrypt(txtPasswordNueva.Text);
                    oRegistro.Save();

                    string popupScript = "<script language='JavaScript'> alert('La nueva contraseña se ha guardado correctamente.'); </script>";
                    Page.RegisterStartupScript("PopupScript", popupScript);
                
            }
        }

       

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Usuario oRegistro = new Usuario();
            oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["id"]));

            if (oRegistro.Password != oUtil.Encrypt(txtPasswordActual.Text))            
                args.IsValid = false;                                         
            else
                args.IsValid = true;
        }

    

    }
}