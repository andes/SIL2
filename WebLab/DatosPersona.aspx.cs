using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
    public partial class DatosPersona : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //HttpContext CurrContext = HttpContext.Current;
                //string username = CurrContext.Items["Documento"].ToString();
                //string contra = CurrContext.Items["Clave"].ToString();
                //string Apellido = CurrContext.Items["Apellido"].ToString();
                //string Nombre = CurrContext.Items["Nombre"].ToString();


                string username = Session["Documento"].ToString()  ;
                string contra = Session["Clave"].ToString();
                string Apellido = Session["Apellido"].ToString();
                string Nombre = Session["Nombre"].ToString();
             //   string Nombre = Session["Titulo"] = pro[i].profesiones[0].titulo;


                txtApellido.Text = Apellido;
                txtNombre.Text = Nombre;
                txtDocumento.Text = username;
                
            }
           
        }
       
        

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            /*CurrContext.Items.Add("Documento", username);
            CurrContext.Items.Add("Clave", Login1.Password);
            CurrContext.Items.Add("Apellido", pro[i].apellido);
            CurrContext.Items.Add("Nombre", pro[i].nombre);
            CurrContext.Items.Add("Titulo", pro[i].profesiones[0].titulo);
            */

            /* HttpContext CurrContext = HttpContext.Current;
             string username=CurrContext.Items["Documento"].ToString() ;
             string contra=CurrContext.Items["Clave"].ToString();
             string Apellido = CurrContext.Items["Apellido"].ToString();
             string Nombre = CurrContext.Items["Nombre"].ToString();
             */

            string username = Session["Documento"].ToString();
            string contra = Session["Clave"].ToString();
            string Apellido = Session["Apellido"].ToString();
            string Nombre = Session["Nombre"].ToString();
            /*Graba el usuario en sys_usuario con el perfil de consulta*/

            string accion = "Graba desde Onlogin";


            Perfil oPerfil = new Perfil();
            oPerfil = (Perfil)oPerfil.Get(typeof(Perfil),  18);  ///ver perfil de consulta

            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), 227);  //subsecretaria

            Efector oEfectorDestino = new Efector();
             oEfectorDestino = oEfector;
            Usuario oRegistro = new Usuario();
            oRegistro.IdEfector = oEfector;
            oRegistro.IdEfectorDestino = oEfectorDestino;
            oRegistro.IdPerfil = oPerfil;
            oRegistro.IdArea = 0; //int.Parse(ddlArea.SelectedValue);

            oRegistro.Apellido = Apellido;
            oRegistro.Nombre = txtNombre.Text;
            oRegistro.Legajo = "";
            oRegistro.FirmaValidacion = "";
            //oRegistro.Matricula=txtMatricula.Text;
            oRegistro.Administrador = false;
            oRegistro.Username = username;

            oRegistro.Email = "";
            oRegistro.Telefono = "";

            oRegistro.Activo = true;
            if (accion != "Modifica") //no se modifica contrasñea
            {
                Utility oUtil = new Utility();
                string m_password = oUtil.Encrypt(contra);
                oRegistro.Password = m_password;
            }

           

            
            oRegistro.Externo = false;
            oRegistro.RequiereCambioPass = false;
            oRegistro.IdUsuarioActualizacion = 2; // Adminsitrador int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaActualizacion = DateTime.Now;

            oRegistro.Save();

            //if (Request["id"] == null) // NUEVO USUARIO
            //{
                UsuarioEfector oUsuarioEfector = new UsuarioEfector();
                oUsuarioEfector.IdUsuario = oRegistro;
                oUsuarioEfector.IdEfector = oEfector;
                oUsuarioEfector.Activo = true;
                oUsuarioEfector.Save();
            //}

            Usuario oAuditor = new Usuario();
            oAuditor = (Usuario)oAuditor.Get(typeof(Usuario), 2);
         
            oAuditor.GrabaAuditoria(accion, oRegistro.IdUsuario, oRegistro.Username);

            /*lleva al sistema*/
            Session["idUsuario"] = oRegistro.IdUsuario.ToString();
            Session["idUsuarioAux"] = oRegistro.IdUsuario.ToString();
            Response.Redirect("LoginEfector.aspx", false);
            //Response.Redirect("Default.aspx", false);

        }
    }
}