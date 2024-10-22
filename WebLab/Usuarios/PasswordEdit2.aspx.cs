using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data;
using Business;
using System.Collections;

namespace WebLab.Usuarios
{
    public partial class PasswordEdit2 : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        Usuario oRegistro = new Usuario();
     
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request["idUsuario"] != null)
            {
                Page.MasterPageFile = "~/Site2.master";
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["idUsuario"].ToString()));
                if (oRegistro.Externo)
                    Page.MasterPageFile = "~/Site2.master";

            }

            if (Session["idUsuario"] != null)
            {
               
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oRegistro.Externo)
                    Page.MasterPageFile = "~/Site2.master";
            }

       }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
           
             //   if (Request["id"] != null)
                    MostrarDatos();
            }
        }

        //private void VerificaPermisos(string sObjeto)
        //{
        //    if (Session["idUsuarioValida"] == null) Response.Redirect("../FinSesion.aspx");
        //    if (Session["idUsuario"] != null)
        //    {
        //        if (Session["s_permiso"] != null)
        //        {
        //            Utility oUtil = new Utility();
        //            int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
        //            if (i_permiso == 0) Response.Redirect("../AccesoDenegado.aspx", false);

        //        }
        //        else Response.Redirect("../FinSesion.aspx", false);
        //    }
        //    else Response.Redirect("../FinSesion.aspx", false);
        //}

        private void MostrarDatos()
        {
            if (Session["idUsuario"] != null)
            {
                Usuario oRegistro = new Usuario();
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                lblUsuario.Text = oRegistro.Username;
            }
            if (Request["idUsuario"] != null)
            {
                Usuario oRegistro = new Usuario();
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["idUsuario"].ToString()));

                lblUsuario.Text = oRegistro.Username;
            }



        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { Usuario oRegistro = new Usuario();
                if (Session["idUsuario"] != null)
                {
                   
                    oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                }

                if (Request["idUsuario"] != null)
                {
                 
                    oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["idUsuario"].ToString()));
                }
                    oRegistro.Password = oUtil.Encrypt(txtPasswordNueva.Text);
                oRegistro.RequiereCambioPass = false;
                    oRegistro.Save();
                Session["idUsuario"] = oRegistro.IdUsuario.ToString();

                lblMensajeOk.Text = "La nueva contraseña se ha guardado correctamente.Haga clic en Continuar";

                    //string popupScript = "<script language='JavaScript'> alert('La nueva contraseña se ha guardado correctamente.'); </script>";
                    //Page.RegisterStartupScript("PopupScript", popupScript);

            }
        }

       

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Usuario oRegistro = new Usuario();
            if (Session["idUsuario"] != null)
            
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            if (Request["idUsuario"] != null)
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["idUsuario"].ToString()));


            if (oRegistro.Password != oUtil.Encrypt(txtPasswordActual.Text))            
                args.IsValid = false;                                         
            else
                args.IsValid = true;
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
                lblMensajeOk.Text = "No es posible Continuar si no cambia la contraseña";
            else
            {
                Usuario oRegistro = new Usuario();
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oRegistro.Externo)                 

                    Response.Redirect("~/Consulta/Historiaclinicafiltro.aspx", false);
                else
                    Response.Redirect("../Default.aspx", false);
                
            }

        }
    }
}