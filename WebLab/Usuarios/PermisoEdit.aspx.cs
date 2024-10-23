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
using System.Data.SqlClient;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Usuarios
{
    public partial class PermisoEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Perfiles");
               
                if (Request["id"]!=null)
                {
                    Perfil oPerfil= new Perfil();
                    oPerfil= (Perfil)oPerfil.Get(typeof(Perfil), int.Parse(Request["id"].ToString()));
                    lblPerfil.Text = oPerfil.Nombre;


                    CrearMenuPrincipal(oPerfil.IdPerfil.ToString());
                    lblModulo.Text = "Seleccione módulo a configurar los permisos de acceso.";
                    if (Request["idMenuPrincipal"] != null) CrearMenuSecundario(Request["idMenuPrincipal"].ToString());
                    
                }


               
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
                    case 1:btnGuardarPermisos.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void CrearMenuSecundario(string p)
        {
            MenuSistema oMenu = new MenuSistema();
            oMenu = (MenuSistema)oMenu.Get(typeof(MenuSistema), int.Parse(p));

            lblModulo.Text ="Módulo seleccionado: "+ oMenu.Objeto;


            string idperfil = Request["id"].ToString();
            //string m_strSQL = " SELECT distinct M.idMenu, m.objeto , P.permiso  " +
            //                  " FROM         Sys_Menu AS M " +
            //                  " INNER JOIN   Sys_Modulo AS Mo ON M.idModulo = Mo.idModulo                               " +
            //                  " INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu " +
            //                  " WHERE M.idMenusuperior=" + p + "  AND (P.idPerfil = " + idperfil + ")  and (M.habilitado = 1) AND (M.idModulo = 2)" +
            //                  " ORDER BY M.idmenu ";


            string m_strSQL = @" 
WITH TreeView(idmenu, objeto, Level, permiso, idMenusuperior,posicion)--Definimos nuestro CTE 
AS 
( 
    -- Definimos la raíz o miembro anclar 

	SELECT  M.idMenu, case when m.esaccion=1 then m.objeto+'**' else  m.objeto end  , 0 as Level, P.permiso as permiso,M.idMenusuperior  as idMenusuperior, m.posicion
                               FROM         Sys_Menu AS M 
                               INNER JOIN   Sys_Modulo AS Mo ON M.idModulo = Mo.idModulo                               
                               INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
                               WHERE M.idMenusuperior=" + p + "  AND (P.idPerfil = " + idperfil + ")  and (M.habilitado = 1) AND (M.idModulo = 2) "+
    @" union all
	SELECT  M.idMenu,case when m.esaccion=1 then m.objeto+'**' else  m.objeto end   as objeto ,1 as Level, P.permiso as permiso ,M.idMenusuperior as idMenusuperior, m.posicion
                               FROM         Sys_Menu AS M 
  INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
	INNER JOIN TreeView tv on m.idMenusuperior = tv.idmenu 
    WHERE  (P.idPerfil = " + idperfil + ")  and (M.habilitado = 1) AND (M.idModulo = 2) " +

 
@")

SELECT * from TreeView 
Order by posicion";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvPermisoMenu.DataSource = Ds.Tables[0];
            gvPermisoMenu.DataBind();
        }


        private void CrearMenuPrincipal(string idperfil)
        {
            string m_strSQL = @" SELECT DISTINCT M.idMenu, M.objeto, M.idMenuSuperior, M.url
FROM         Sys_Menu AS M INNER JOIN
                      Sys_Modulo AS Mo ON M.idModulo = Mo.idModulo INNER JOIN
                      Sys_Permiso AS P ON M.idMenu = P.idMenu
WHERE     (M.idMenuSuperior = 0) AND (M.habilitado = 1) AND (M.idModulo = 2)  ORDER BY M.idMenu";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            string primermenuprincipal = "";
            int i = 0;
            DataTable dtMenuItem = Ds.Tables[0];
            foreach (DataRow drMenuItem in dtMenuItem.Rows)
            {
                if (drMenuItem.ItemArray[2].ToString() == "0") ///Crea los accesos superiores - nivel 0
                {
                
                    MenuItem mnuMenuItem = new MenuItem();
                    mnuMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                    mnuMenuItem.Text = drMenuItem.ItemArray[1].ToString();
                    mnuMenuItem.NavigateUrl = "PermisoEdit.aspx?id="+idperfil+"&idMenuPrincipal="+drMenuItem.ItemArray[0].ToString();
                    if (i==0)
                        primermenuprincipal = drMenuItem.ItemArray[0].ToString();


                    mnuPrincipal.Items.Add(mnuMenuItem);
                   
                    i += 1;
                }
            }

           // Response.Redirect("PermisoEdit.aspx?id=" + idperfil + "&idMenuPrincipal=" + primermenuprincipal, false);

        }

        protected void mnuPrincipal_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

        protected void gvPermisoMenu_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                RadioButtonList chkListMyCheckBoxList = (RadioButtonList)(e.Row.FindControl("chkPermiso"));

                string per = e.Row.Cells[2].Text;

                chkListMyCheckBoxList.SelectedValue = per;
            }
            
        }

        protected void btnGuardarPermisos_Click(object sender, EventArgs e)
        {
            GuardarPermisos();
        }

        private void GuardarPermisos()
        {
            Perfil oPerfil = new Perfil();
            oPerfil = (Perfil)oPerfil.Get(typeof(Perfil), int.Parse(Request["id"].ToString()));

            ISession m_session = NHibernateHttpModule.CurrentSession;
            foreach (GridViewRow grd_Row in gvPermisoMenu.Rows)
            {
                Permiso oPermiso = new Permiso();
                ICriteria crit = m_session.CreateCriteria(typeof(Permiso));
                crit.Add(Expression.Eq("IdPerfil", oPerfil));
                crit.Add(Expression.Eq("IdMenu",int.Parse( gvPermisoMenu.DataKeys[grd_Row.RowIndex].Value.ToString())));

                oPermiso =(Permiso) crit.UniqueResult();
                if (oPermiso != null)
                {
                    RadioButtonList chkListMyCheckBoxList = (RadioButtonList)(grd_Row.FindControl("chkPermiso"));
                    oPermiso.PermisoAcceso = chkListMyCheckBoxList.SelectedValue;
                    oPermiso.Save();

                }
            }


        }

        protected void gvPermisoMenu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
