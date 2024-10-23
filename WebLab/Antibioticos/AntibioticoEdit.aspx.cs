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
using Business;
using Business.Data;

namespace WebLab.Antibioticos
{
    public partial class AntibioticoEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();

        

     //   Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //    oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Antibioticos");
       
                if (Request["id"] != null)
                    MostrarDatos();
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
          //      Utility oUtil = new Utility();
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


            Antibiotico oRegistro = new Antibiotico();
            oRegistro = (Antibiotico)oRegistro.Get(typeof(Antibiotico), int.Parse(Request["id"]));
            txtNombre.Text = oRegistro.Descripcion;
            txtAbreviatura.Text = oRegistro.NombreCorto;
            

        }
       

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Antibiotico oRegistro = new Antibiotico();
                if (Request["id"] != null) oRegistro = (Antibiotico)oRegistro.Get(typeof(Antibiotico), int.Parse(Request["id"]));
                Guardar(oRegistro);

                if (Request["id"] != null)
                    Response.Redirect("AntibioticoList.aspx");
                else
                    Response.Redirect("AntibioticoEdit.aspx");
            }
        }


        private void Guardar(Antibiotico oRegistro)
        {

        /*    Usuario oUser = new Usuario();

            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);*/

            oRegistro.Descripcion = txtNombre.Text;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.NombreCorto = txtAbreviatura.Text;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AntibioticoList.aspx");
        }
    }
}
