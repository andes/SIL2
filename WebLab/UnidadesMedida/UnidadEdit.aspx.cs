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
using Business.Data.Laboratorio;
using Business.Data;
using Business;

namespace WebLab.UnidadesMedida
{
    public partial class UnidadEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Un. Medida");
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
            UnidadMedida oRegistro = new UnidadMedida();
            oRegistro = (UnidadMedida)oRegistro.Get(typeof(UnidadMedida), int.Parse(Request["id"]));
            txtNombre.Text = oRegistro.Nombre;
        }




        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                UnidadMedida oRegistro = new UnidadMedida();
                if (Request["id"] != null) oRegistro = (UnidadMedida)oRegistro.Get(typeof(UnidadMedida), int.Parse(Request["id"]));
                Guardar(oRegistro);

                if (Request["id"] != null)
                    Response.Redirect("UnidadList.aspx", false);
                else
                    Response.Redirect("UnidadEdit.aspx", false);
            }
        }


        private void Guardar(UnidadMedida oRegistro)
        {


            Usuario oUser = new Usuario();
            oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //Efector oEfector = new Efector();

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("UnidadList.aspx");
        }
    }
}
