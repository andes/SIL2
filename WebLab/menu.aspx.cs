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
    public partial class menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) CargarListas();

        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();


            ///Carga de combos de estado
            string m_ssql = "select idMenu, objeto from sys_menu where idmenusuperior=0 or url='/' order by idMenu";
            oUtil.CargarCombo(ddlIdMenuSuperior, m_ssql, "idMenu", "objeto");
            ddlIdMenuSuperior.Items.Insert(0, new ListItem("--Es Menu Superior--", "0"));


            ////////////Carga de combos de sexo biologico
            m_ssql = "SELECT idPerfil,convert(varchar, idPerfil) + ' - ' + nombre as nombre from sys_perfil  ";
            oUtil.CargarCombo(ddlPerfil, m_ssql, "idPerfil", "nombre");
            
            m_ssql = null;
            oUtil = null;


        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Configuracion oC = new Configuracion();

            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);


            MenuSistema oRegistro = new MenuSistema();
            oRegistro.Objeto = txtObjeto.Text;
            oRegistro.Url = txtURL.Text;
            oRegistro.IdMenuSuperior =int.Parse( ddlIdMenuSuperior.SelectedValue);
            oRegistro.Posicion =int.Parse( txtOrden.Text);
            oRegistro.Icono = "";
            oRegistro.Habilitado = true;
            oRegistro.FechaCreacion = DateTime.Today;
            oRegistro.IdUsuarioCreacion = 2;
            oRegistro.FechaModificacion = DateTime.Today;
            oRegistro.IdUsuarioModificacion = 2;
            oRegistro.IdModulo = 2;
            oRegistro.EsAccion = false;
            oRegistro.Save();

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Perfil));
            
            IList detalle = crit.List();

            foreach (Perfil oPerfil in detalle)
            {
                Permiso oPermiso = new Permiso();
                oPermiso.IdEfector = oC.IdEfector.IdEfector;
                oPermiso.IdPerfil = oPerfil;

                oPermiso.IdMenu = oRegistro.IdMenu;
                if (txtPerfilPermiso.Text.Contains(oPerfil.IdPerfil.ToString()))
                    oPermiso.PermisoAcceso = "2";
                else
                    oPermiso.PermisoAcceso = "0";

                oPermiso.Save();
            }
                
           


        }
    }
}