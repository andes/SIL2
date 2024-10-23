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
using Business.Data;
using Business;

namespace WebLab.Profesionales
{
    public partial class ProfesionalEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Médico Solicitante");             
                if (Request["id"] != null)
                    MostrarDatos();
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
      
        private void MostrarDatos()
        {
            Profesional oRegistro = new Profesional();
            oRegistro = (Profesional)oRegistro.Get(typeof(Profesional), int.Parse(Request["id"]));
            txtApellido.Text = oRegistro.Apellido;
            txtNombre.Text = oRegistro.Nombre;
            txtNumeroDocumento.Value = oRegistro.NumeroDocumento.ToString();
            txtMatricula.Text = oRegistro.Matricula;
            
            chkActivo.Checked = oRegistro.Activo;
            
        }




        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }


        private void Guardar(Profesional oRegistro)
        {
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            
            oRegistro.IdEfector =oUser.IdEfector.IdEfector;
           

            oRegistro.Apellido = txtApellido.Text;
            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdTipoDocumento=1;
            if (txtNumeroDocumento.Value.Trim()!="")
                oRegistro.NumeroDocumento=int.Parse( txtNumeroDocumento.Value);
            else
                oRegistro.NumeroDocumento=0;

            oRegistro.Matricula = txtMatricula.Text;
            oRegistro.Activo = chkActivo.Checked;
            oRegistro.IdUsuario = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaModificacion = DateTime.Now;

            oRegistro.Save();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProfesionalList.aspx");
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Profesional oRegistro = new Profesional();
                if (Request["id"] != null) oRegistro = (Profesional)oRegistro.Get(typeof(Profesional), int.Parse(Request["id"]));
                Guardar(oRegistro);

                if (Request["id"] != null)
                    Response.Redirect("ProfesionalList.aspx", false);
                else
                    Response.Redirect("ProfesionalEdit.aspx", false);
            }
        }
    }
}
