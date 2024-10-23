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

namespace WebLab.Metodos
{
    public partial class MetodoEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Metodo");

       

             

                if (Request["id"] != null)
                    MostrarDatos();
            }
        }


        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
            //    Utility oUtil = new Utility();
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
            Metodo oRegistro = new Metodo();
            oRegistro = (Metodo)oRegistro.Get(typeof(Metodo), int.Parse( Request["id"].ToString()));
            txtNombre.Text = oRegistro.Nombre;            
        }
       



        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Metodo oRegistro = new Metodo();
                if (Request["id"] != null) oRegistro = (Metodo)oRegistro.Get(typeof(Metodo), int.Parse(Request["id"].ToString()));                
                Guardar(oRegistro);

                if (Request["id"] != null)
                    Response.Redirect("MetodoList.aspx",false);
                else
                    Response.Redirect("MetodoEdit.aspx",false);
            }
        }


        private void Guardar(Metodo oRegistro)
        {

           
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
          


            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdEfector = oUser.IdEfector; // (Efector)oEfector.Get(typeof(Efector), "IdEfector", 5);            
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("MetodoList.aspx");
        }
    }
}
