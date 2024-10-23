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
using Business.Data.Laboratorio;
using Business;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Usuarios
{
    public partial class PerfilEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Perfil");
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
                    case 1:btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void MostrarDatos()
        {
            Perfil oRegistro = new Perfil();
            oRegistro = (Perfil)oRegistro.Get(typeof(Perfil), int.Parse(Request["id"]));
            if (oRegistro != null)
            {
                txtNombre.Text = oRegistro.Nombre;
                chkActivo.Checked = oRegistro.Activo;
            }
        }




        protected void btnGuardar_Click(object sender, EventArgs e)
        {
          
        }


        private void Guardar(Perfil oRegistro)
        {


            Usuario oUser = new Usuario();
            //Efector oEfector = new Efector();

            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdEfector = oC.IdEfector.IdEfector;
            oRegistro.Activo = chkActivo.Checked;
            oRegistro.IdUsuario =  int.Parse(Session["idUsuario"].ToString());                        
            oRegistro.FechaActualizacion= DateTime.Now;

            oRegistro.Save();
            if (Request["id"] == null) ///solo si es nuevo
            {
                GuardaPermisoSoloLectura(oRegistro);///asigna permisos a todo solo de lectura
                Response.Redirect("PermisoEdit.aspx?id=" + oRegistro.IdPerfil.ToString(), false);///lleva a editar los permisos.
            }
            else            
                Response.Redirect("PerfilList.aspx", false);
            

        }

        private void GuardaPermisoSoloLectura(Perfil oRegistro)
        {
            ///Si existe algun permiso, boora todo para el pefil.
            Permiso oItem = new Permiso();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Permiso));
            crit.Add(Expression.Eq("IdPerfil", oRegistro));

            IList permisosexistentes= crit.List();
            foreach (Permiso oDias in permisosexistentes)
            {
                oDias.Delete();
            }


            ///Asigna permisos de solo lectura para el perfil recien creado.
            MenuSistema oMenu = new MenuSistema();
            
            ICriteria crit1 = m_session.CreateCriteria(typeof(MenuSistema));
            crit1.Add(Expression.Eq("IdModulo", 2));

            IList listamenu = crit1.List();
            foreach (MenuSistema oMenu1 in listamenu)
            {
                Permiso oPermiso= new Permiso();
                oPermiso.IdEfector = oRegistro.IdEfector;
                oPermiso.IdPerfil = oRegistro;
                oPermiso.IdMenu = oMenu1.IdMenu;
                oPermiso.PermisoAcceso = "1";
                oPermiso.Save();
            }


        }

      


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PerfilList.aspx");
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Perfil oRegistro = new Perfil();
                if (Request["id"] != null) oRegistro = (Perfil)oRegistro.Get(typeof(Perfil), int.Parse(Request["id"]));
                Guardar(oRegistro);

                
            }
        }
    }
}
