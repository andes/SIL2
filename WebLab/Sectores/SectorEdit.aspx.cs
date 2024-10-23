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
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Sectores
{
    public partial class SectorEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Servicios");
       
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
            
           
            SectorServicio oRegistro = new SectorServicio();
            oRegistro = (SectorServicio)oRegistro.Get(typeof(SectorServicio), int.Parse(Request["id"]));
            txtNombre.Text = oRegistro.Nombre;
            txtAbreviatura.Text = oRegistro.Prefijo;
            

        }
       

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SectorServicio oRegistro = new SectorServicio();
                if (Request["id"] != null) oRegistro = (SectorServicio)oRegistro.Get(typeof(SectorServicio), int.Parse(Request["id"]));
                Guardar(oRegistro);

                if (Request["id"] != null)
                    Response.Redirect("SectorList.aspx");
                else
                    Response.Redirect("SectorEdit.aspx");
            }
        }


        private void Guardar(SectorServicio oRegistro)
        {

            Usuario oUser = new Usuario();

            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdEfector = oC.IdEfector;
            oRegistro.Prefijo = txtAbreviatura.Text;            
            oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("SectorList.aspx");
        }

        protected void cvLetra_ServerValidate(object source, ServerValidateEventArgs args)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(SectorServicio));
            crit.Add(Expression.Eq("Prefijo", txtAbreviatura.Text));
             crit.Add(Expression.Eq("Baja", false));


            ///valida si ya existe un servicio con la misma letra.
            IList lista = crit.List();

            if (lista.Count == 0) args.IsValid=true;
            else args.IsValid=false;
            
        }
    }
}
