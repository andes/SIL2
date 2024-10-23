using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data;
using System.Data;
using Business;
using System.Data.SqlClient;
using System.Collections;
using Business.Data.Laboratorio;
using System.Configuration;

namespace WebLab
{
    public partial class SiteTurnos : System.Web.UI.MasterPage {  
        
        protected void Page_Load(object sender, EventArgs e) 
        {
          //  Page.RegisterRedirectOnSessionEndScript();
            if (!Page.IsPostBack)
            {
                if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")


                {
                    imgPrincipal.Visible = true;
                    lnkCerrar.NavigateUrl = "FinSesion.aspx";
                   
                }
                else
                {
                    imgPrincipal.Visible = false;
                    lnkCerrar.NavigateUrl = "logout.aspx";
                }
                    if (Session["idUsuario"] != null)
                {
                    lblFechaHora.Text = DateTime.Now.ToLongDateString().ToUpper() + " " + DateTime.Now.ToLongTimeString();
                    
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));//Session["idUsuario"].ToString());
                    lblUsuario.Text = oUser.Username + " - " + oUser.Nombre + " " + oUser.Apellido + " " + oUser.IdEfector.Nombre;

                    //Efector oEfector = new Efector();
                    //oEfector = (Efector)oEfector.Get(int.Parse(oUser.IdEfector.ide));

                    lblEfector.Text ="             "+ oUser.IdEfector.Nombre;
                    CrearMenu(oUser);

                    if (Session["SIL"] != null)
                        lnkCambiarPass.Visible = true;
                    else
                        lnkCambiarPass.Visible = false;

                }
                else
                    Response.Redirect(Page.ResolveUrl("~/FinSesion.aspx"), false);
            }               
        }

     

        private DataTable CrearMenuSecundario(string user,string p)
        {
            string m_strSQL = " SELECT distinct M.idMenu, m.objeto , M.idMenusuperior, M.url,P.permiso  " +
                              " FROM         Sys_Menu AS M " +
                              " INNER JOIN   Sys_Modulo AS Mo ON M.idModulo = Mo.idModulo                               " +
                              " INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu " +
                              " INNER JOIN   Sys_Perfil AS Pf ON P.idPerfil = Pf.idPerfil " +
                              " INNER JOIN   Sys_Usuario AS U ON Pf.idPerfil = U.idPerfil " +
                              " WHERE M.esAccion=0 and M.idMenusuperior=" + p + " and P.permiso>0 and   (U.idUsuario = " + user + ")  AND (M.habilitado = 1) AND (M.idModulo = 2)" +
                              " ORDER BY M.posicion ";
            
            DataSet Ds = new DataSet();
            //using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))  ///Performance: conexion de solo lectura)
            {

                //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
                DataTable dtMenuItem = Ds.Tables[0];
                conn.Close();

                return dtMenuItem;

            }
           
        }

        private void CrearMenu(Usuario oUser)
        {
            //string m_strSQL = " SELECT  M.idMenu, m.objeto , M.idMenusuperior, M.url,P.permiso  " +
            //                  " FROM         Sys_Menu AS M " +
            //                  " INNER JOIN   Sys_Modulo AS Mo ON M.idModulo = Mo.idModulo                               " +
            //                  " INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu " +
            //                  " INNER JOIN   Sys_Perfil AS Pf ON P.idPerfil = Pf.idPerfil " +
            //                  " INNER JOIN   Sys_Usuario AS U ON Pf.idPerfil = U.idPerfil " +
            //                  " WHERE P.permiso>0 and   (U.idUsuario = " + p + ")  AND (M.habilitado = 1) AND (M.idModulo = 2)" +
            //                  " ORDER BY m.idmenusuperior, m.posicion";
            //                  //" ORDER BY M.idmenu ";



            string m_strSQL = @"  WITH TreeView(idmenu, objeto, idMenusuperior,url, permiso,posicion, Level ,idEfector )--Definimos nuestro CTE 
AS 
( 
    -- Definimos la raíz o miembro anclar 

	SELECT  M.idMenu, m.objeto ,M.idMenusuperior  as idMenusuperior, M.url ,P.permiso as permiso, m.posicion, m.idmenu as Level  ,U.idEfector  
                               FROM         Sys_Menu AS M 
                            --   INNER JOIN   Sys_Modulo AS Mo ON M.idModulo = Mo.idModulo                               
                               INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
                                INNER JOIN   Sys_Perfil AS Pf ON P.idPerfil = Pf.idPerfil 
                                INNER JOIN   Sys_Usuario AS U ON Pf.idPerfil = U.idPerfil 
                               WHERE  M.esAccion=0 and M.idMenusuperior=0  and P.permiso>0 and   (U.idUsuario = " + oUser.IdUsuario.ToString() + @")  AND (M.habilitado = 1) AND (M.idModulo = 2)
	union all
	SELECT M.idMenu, m.objeto ,M.idMenusuperior  as idMenusuperior, M.url ,P.permiso as permiso, m.posicion, 1 as Level  ,U.idEfector  
                               FROM         Sys_Menu AS M 
  INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
 INNER JOIN   Sys_Perfil AS Pf ON P.idPerfil = Pf.idPerfil 
                                INNER JOIN   Sys_Usuario AS U ON Pf.idPerfil = U.idPerfil 
	INNER JOIN TreeView tv on m.idMenusuperior = tv.idmenu 
    WHERE  M.esAccion=0 and P.permiso>0 and  (M.habilitado = 1) AND (M.idModulo = 2)  and   U.idUsuario = " + oUser.IdUsuario.ToString() + @"

 
)

SELECT idmenu, objeto, idMenusuperior,url, permiso,posicion, Level,P.idEfector  from TreeView P
where    idmenusuperior not in (  197 ,217)
union

SELECT idmenu, objeto, idMenusuperior,url, permiso,posicion, Level,P.idEfector  from TreeView P
where   exists (
select 1 from  lab_EfectorEquipo EE where EE.idefector=P.idEfector and EE.idmenuequipo = P.idmenu and  EE.habilitado=1
and P.idmenu= EE.idmenuequipo and idmenusuperior in (197 ,217) )
Order by level,posicion";

            if (oUser.IdEfector.IdEfector.ToString()=="227")
                m_strSQL = @" WITH TreeView(idmenu, objeto, idMenusuperior,url, permiso,posicion, Level,idEfector )--Definimos nuestro CTE 
AS 
( 
    -- Definimos la raíz o miembro anclar 

	SELECT  M.idMenu, m.objeto ,M.idMenusuperior  as idMenusuperior, M.url ,P.permiso as permiso, m.posicion, m.idmenu as Level ,U.idEfector 
                               FROM         Sys_Menu AS M 
                                                           
                               INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
                                INNER JOIN   Sys_Perfil AS Pf ON P.idPerfil = Pf.idPerfil 
                                INNER JOIN   Sys_Usuario AS U ON Pf.idPerfil = U.idPerfil 
                               WHERE  M.esAccion=0 and M.idMenu in (97,170,174,197,1286)  and M.habilitado=1
and P.permiso>0 and   (U.idUsuario = " + oUser.IdUsuario.ToString() + @") 
AND   (M.idModulo = 2)
 
	union all
	SELECT M.idMenu, m.objeto ,M.idMenusuperior  as idMenusuperior, M.url ,P.permiso as permiso, m.posicion, 1 as Level ,U.idEfector  
                               FROM         Sys_Menu AS M 
  INNER JOIN   Sys_Permiso AS P ON M.idMenu = P.idMenu 
 INNER JOIN   Sys_Perfil AS Pf ON P.idPerfil = Pf.idPerfil 
                                INNER JOIN   Sys_Usuario AS U ON Pf.idPerfil = U.idPerfil 
	INNER JOIN TreeView tv on m.idMenusuperior = tv.idmenu 
    WHERE  M.esAccion=0     and M.habilitado=1  AND (M.idModulo = 2)  and   U.idUsuario = " + oUser.IdUsuario.ToString() + @"

 
)
  SELECT distinct *from TreeView
Order by level, posicion

";

            DataSet Ds = new DataSet();
            //using (                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))  ///Performance: conexion de solo lectura)
            {
                //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);



                DataTable dtMenuItem = Ds.Tables[0];

                //lstViewMenu.DataSource = dtMenuItem;
                //lstViewMenu.DataBind();


                MenuItem mnuMenuItem1 = new MenuItem();
                mnuMenuItem1.Value = "9999";
                mnuMenuItem1.Text = "Principal";
                //mnuMenuItem.ImageUrl= drMenuItem.ItemArray[5].ToString();
                mnuMenuItem1.NavigateUrl = "PrincipalTurnos.aspx";
                mnuPrincipal.Items.Add(mnuMenuItem1);
                //    //agregamos el Ítem al menú


                foreach (DataRow drMenuItem in dtMenuItem.Rows)
                {
                    if (drMenuItem.ItemArray[2].ToString() == "0") ///Crea los accesos superiores - nivel 0
                    {
                        MenuItem mnuMenuItem = new MenuItem();
                        mnuMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                        mnuMenuItem.Text = CompletarTamanioItemMenuPrincipal(drMenuItem.ItemArray[1].ToString());
                        if (drMenuItem.ItemArray[3].ToString().Trim() != "~/") mnuMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
                        else mnuMenuItem.NavigateUrl = "PrincipalTurnos.aspx";

                        //if (drMenuItem.ItemArray[1].ToString().Trim() == "Ayuda en linea") imgAyudaLinea.Visible = true;

                        mnuPrincipal.Items.Add(mnuMenuItem);
                        //    //hacemos un llamado al metodo recursivo encargado de generar el árbol del menú.
                        AddMenuItem(ref mnuMenuItem, dtMenuItem);

                        if (mnuMenuItem.ChildItems.Count == 0)// si el menu no tiene hijos se borra

                            mnuPrincipal.Items.Remove(mnuMenuItem);
                    }
                }
                conn.Close();
            }
        }

        private void AddMenuItem(ref MenuItem mnuMenuItem, DataTable dtMenuItem)
        {
            //recorremos cada elemento del datatable para poder determinar cuales son elementos hijos
            //del menuitem dado pasado como parametro ByRef.

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            foreach (DataRow drMenuItem in dtMenuItem.Rows)
            {
                //if (drMenuItem.ItemArray[4].ToString() != "0")///verifica el permiso
                //{
                if (drMenuItem.ItemArray[2].ToString().Equals(mnuMenuItem.Value) && !drMenuItem.ItemArray[0].Equals(drMenuItem.ItemArray[2]))
                {

                    if ((drMenuItem.ItemArray[1].ToString() == "Pacientes sin turno") && (!oCon.Turno))
                    {
                        MenuItem mnuNewMenuItem = new MenuItem();
                        mnuNewMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                        mnuNewMenuItem.Text = CompletarTamanioItemMenu("Carga de Protocolo"); // drMenuItem.ItemArray[1].ToString().Replace("Edit", "");
                        mnuNewMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
                        
                        mnuMenuItem.ChildItems.Add(mnuNewMenuItem);
                        AddMenuItem(ref mnuNewMenuItem, dtMenuItem);
                    }
                    if ((drMenuItem.ItemArray[1].ToString().Contains("turno")) || (drMenuItem.ItemArray[1].ToString().Contains("Turno")) || (drMenuItem.ItemArray[1].ToString().Contains("Agenda")))
                    {
                        if (oCon.Turno) // si es verdadero lo crea
                        {
                            MenuItem mnuNewMenuItem = new MenuItem();
                            mnuNewMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                            mnuNewMenuItem.Text = CompletarTamanioItemMenu(drMenuItem.ItemArray[1].ToString().Replace("Edit", ""));
                            mnuNewMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
                            mnuMenuItem.ChildItems.Add(mnuNewMenuItem);
                            AddMenuItem(ref mnuNewMenuItem, dtMenuItem);
                        }
                    }
                    else
                    {
                        MenuItem mnuNewMenuItem = new MenuItem();
                        mnuNewMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                        if (drMenuItem.ItemArray[1].ToString().Trim() != "") mnuNewMenuItem.Text =CompletarTamanioItemMenu( drMenuItem.ItemArray[1].ToString().Replace("Edit", ""));

                            if (drMenuItem.ItemArray[3].ToString().Trim() == "~/") mnuNewMenuItem.NavigateUrl = "Principal.aspx";
                            else                             mnuNewMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
                        mnuMenuItem.ChildItems.Add(mnuNewMenuItem);
                        AddMenuItem(ref mnuNewMenuItem, dtMenuItem);
                    }


                    //}  
                }
            }

        }
        private string CompletarTamanioItemMenuPrincipal(string p)
        {

            int tamañotexto = p.Length;
            for (int i = tamañotexto; i <= 10; i++)
                p = p + "&nbsp;";


            return p;
        }
        private string CompletarTamanioItemMenu(string p)
        {

            int tamañotexto = p.Length;
            for (int i = tamañotexto; i <= 30; i++)
                p = p + "&nbsp;";


            return p;
        }
        protected void imgAyudaLinea_Click(object sender, ImageClickEventArgs e)
        {
            
            Response.Redirect("~/Help/Help_lis.html", false);
        }

      

    }
}
