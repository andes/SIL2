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

namespace WebLab.Recomendaciones
{
    public partial class RecomendacionEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Recomendaciones");
                    if (Request["id"] != null)
                        MostrarDatos();

                }
                else Response.Redirect("../FinSesion.aspx", false);
                
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
            Recomendacion oRegistro = new Recomendacion();
            oRegistro = (Recomendacion)oRegistro.Get(typeof(Recomendacion), int.Parse(Request["id"]));

            txtNombre.Text = oRegistro.Nombre;
            txtDescripcion.Text = oRegistro.Descripcion;            

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

            Recomendacion oRegistro = new Recomendacion();

            if (Request["id"] != null) oRegistro = (Recomendacion)oRegistro.Get(typeof(Recomendacion), int.Parse(Request["id"]));
            Guardar(oRegistro);

            if (Request["id"] != null)
                Response.Redirect("RecomendacionList.aspx",false);
            else
                 Response.Redirect("RecomendacionEdit.aspx", false);


            
             
            }

        }

        private void Guardar(Recomendacion oRegistro)
        {
            //Efector oEfector = new Efector();
            Usuario oUser = new Usuario();
            oUser= (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);


            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Nombre = txtNombre.Text;
            oRegistro.Descripcion = txtDescripcion.Text;
            oRegistro.IdUsuarioRegistro = oUser; //(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();            
        }
    }
}
