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
    public partial class Site1 : System.Web.UI.MasterPage {  
        
        protected void Page_Load(object sender, EventArgs e) 
        {
          //  Page.RegisterRedirectOnSessionEndScript();
            if (!Page.IsPostBack)
            {
                //if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")


                //{
                //    imgPrincipal.Visible = true;
                //    lnkCerrar.NavigateUrl = "FinSesion.aspx";
                   
                //}
                //else
                //{
                    //imgPrincipal.Visible = false;
                    lnkCerrar.NavigateUrl = "logout.aspx";
                //}
                    if (Session["idUsuario"] != null)
                {
                    lblFechaHora.Text = DateTime.Now.ToLongDateString().ToUpper() + " " + DateTime.Now.ToLongTimeString();
                    
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));//Session["idUsuario"].ToString());
                    lblUsuario.Text = oUser.Username + " - " + oUser.Nombre + " " + oUser.Apellido + " " + oUser.IdEfector.Nombre;                    

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



        //private DataTable CrearMenuSecundario(string user,string p)
        //{                        

        //    string m_strSQL = @"select  idMenu, objeto , idMenusuperior, urlmenu,permiso  
        //    from   LAB_MenuTempUsuario  with (nolock)
        //    WHERE   idMenusuperior = " + p + " and permiso > 0 and (idUsuario = " + user + ")  ORDER BY posicion ";
        //    DataSet Ds = new DataSet();
        //    //using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))  ///Performance: conexion de solo lectura)
        //    {                
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //        adapter.Fill(Ds);
        //        DataTable dtMenuItem = Ds.Tables[0];
        //        conn.Close();
        //        conn.Dispose();
        //        return dtMenuItem;

        //    }

        //}
        private string GetMenuCacheKey(Usuario oUser)
        {
            return $"Menu_{oUser.IdUsuario}_{oUser.IdEfector.IdEfector}";
        }


        private DataTable ObtenerMenu(Usuario oUser)
        {
            string sql = @"SELECT idMenu,
                          objeto,
                          idMenuSuperior,
                          urlMenu
                   FROM LAB_MenuTempUsuario WITH (NOLOCK)
                   WHERE idUsuario = @idUsuario
                     AND idEfector = @idEfector
                   ORDER BY level, posicion";

            DataTable dtMenu = new DataTable();

            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@idUsuario", oUser.IdUsuario);
                cmd.Parameters.AddWithValue("@idEfector", oUser.IdEfector.IdEfector);

                da.Fill(dtMenu);
            }

            return dtMenu;
        }
        //private DataTable ObtenerMenu(Usuario oUser)
        //{
        //    string cacheKey = GetMenuCacheKey(oUser);

        //    DataTable dtMenu = Cache[cacheKey] as DataTable;

        //    if (dtMenu != null)
        //        return dtMenu;

        //    string sql = @" SELECT idMenu,
        //       objeto,
        //       idMenuSuperior,
        //       urlMenu
        //FROM LAB_MenuTempUsuario WITH (NOLOCK)
        //WHERE idUsuario = " + oUser.IdUsuario + @"
        //  AND idEfector = " + oUser.IdEfector.IdEfector + @"
        //ORDER BY level, posicion";

        //    using (SqlConnection conn = new SqlConnection(
        //        ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))
        //    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
        //    {
        //        dtMenu = new DataTable();
        //        da.Fill(dtMenu);
        //    }

        //    Cache.Insert(
        //        cacheKey,
        //        dtMenu,
        //        null,
        //        DateTime.Now.AddMinutes(30),   // Expira a los 30 minutos
        //        System.Web.Caching.Cache.NoSlidingExpiration);

        //    return dtMenu;
        //}
        private void CrearMenu(Usuario oUser)
        {
            DataTable dtMenuItem = ObtenerMenu(oUser);
            string urlprincipal = "Principal.aspx";
            if (oUser.IdPerfil.IdPerfil==15)  ///administrativo externo
                urlprincipal = "PrincipalTurnos.aspx";
            mnuPrincipal.Items.Clear();

            MenuItem principal = new MenuItem
            {
                Value = "9999",
                Text = "Principal",
                NavigateUrl = urlprincipal/// "Principal.aspx"
            };

            mnuPrincipal.Items.Add(principal);

            foreach (DataRow dr in dtMenuItem.Select("idMenuSuperior = 0"))
            {
                MenuItem item = new MenuItem
                {
                    Value = dr["idMenu"].ToString(),
                    Text = CompletarTamanioItemMenuPrincipal(dr["objeto"].ToString()),
                    NavigateUrl = dr["urlMenu"].ToString() == "~/"
                        ? urlprincipal//"Principal.aspx"
                        : "~/" + dr["urlMenu"]
                };

                mnuPrincipal.Items.Add(item);

                AddMenuItem(ref item, dtMenuItem);

                if (item.ChildItems.Count == 0)
                    mnuPrincipal.Items.Remove(item);
            }
        }
        //private void CrearMenu_old(Usuario oUser)
        //{
            
        //    string m_strSQL = @"select  idMenu, objeto , idMenusuperior, urlmenu--,permiso, idusuario, idEfector 
        //        from LAB_MenuTempUsuario with (nolock)
        //        where idusuario=" + oUser.IdUsuario.ToString() + " and idEfector=" + oUser.IdEfector.IdEfector.ToString() +@" order by level, posicion";

        //    DataSet Ds = new DataSet();
        //    //using (                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
        //    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString))  ///Performance: conexion de solo lectura)
        //    {
        //        //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //        adapter.Fill(Ds);



        //        DataTable dtMenuItem = Ds.Tables[0];

        //        //lstViewMenu.DataSource = dtMenuItem;
        //        //lstViewMenu.DataBind();


        //        MenuItem mnuMenuItem1 = new MenuItem();
        //        mnuMenuItem1.Value = "9999";
        //        mnuMenuItem1.Text = "Principal";
        //        //mnuMenuItem.ImageUrl= drMenuItem.ItemArray[5].ToString();
        //        mnuMenuItem1.NavigateUrl = "Principal.aspx";
        //        mnuPrincipal.Items.Add(mnuMenuItem1);
        //        //    //agregamos el Ítem al menú


        //        foreach (DataRow drMenuItem in dtMenuItem.Rows)
        //        {
        //            if (drMenuItem.ItemArray[2].ToString() == "0") ///Crea los accesos superiores - nivel 0
        //            {
        //                MenuItem mnuMenuItem = new MenuItem();
        //                mnuMenuItem.Value = drMenuItem.ItemArray[0].ToString();
        //                mnuMenuItem.Text = CompletarTamanioItemMenuPrincipal(drMenuItem.ItemArray[1].ToString());
        //                if (drMenuItem.ItemArray[3].ToString().Trim() != "~/") mnuMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
        //                else mnuMenuItem.NavigateUrl = "Principal.aspx";

        //                //if (drMenuItem.ItemArray[1].ToString().Trim() == "Ayuda en linea") imgAyudaLinea.Visible = true;

        //                mnuPrincipal.Items.Add(mnuMenuItem);
        //                //    //hacemos un llamado al metodo recursivo encargado de generar el árbol del menú.
        //                AddMenuItem(ref mnuMenuItem, dtMenuItem);

        //                if (mnuMenuItem.ChildItems.Count == 0)// si el menu no tiene hijos se borra

        //                    mnuPrincipal.Items.Remove(mnuMenuItem);
        //            }
        //        }
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //}

        private void AddMenuItem(ref MenuItem mnuMenuItem, DataTable dtMenuItem)
        {
            //recorremos cada elemento del datatable para poder determinar cuales son elementos hijos
            //del menuitem dado pasado como parametro ByRef.

             //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            foreach (DataRow drMenuItem in dtMenuItem.Rows)
            {
                //if (drMenuItem.ItemArray[4].ToString() != "0")///verifica el permiso
                //{
                if (drMenuItem.ItemArray[2].ToString().Equals(mnuMenuItem.Value) && !drMenuItem.ItemArray[0].Equals(drMenuItem.ItemArray[2]))
                {

                    //if ((drMenuItem.ItemArray[1].ToString() == "Pacientes sin turno") && (!oCon.Turno))
                   //if (drMenuItem.ItemArray[1].ToString() == "Pacientes sin turno")
                   // {
                   //     MenuItem mnuNewMenuItem = new MenuItem();
                   //     mnuNewMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                   //     mnuNewMenuItem.Text = CompletarTamanioItemMenu("Carga de Protocolo"); // drMenuItem.ItemArray[1].ToString().Replace("Edit", "");
                   //     mnuNewMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
                        
                   //     mnuMenuItem.ChildItems.Add(mnuNewMenuItem);
                   //     AddMenuItem(ref mnuNewMenuItem, dtMenuItem);
                   // }
                    if ((drMenuItem.ItemArray[1].ToString().Contains("turno")) || (drMenuItem.ItemArray[1].ToString().Contains("Turno")) || (drMenuItem.ItemArray[1].ToString().Contains("Agenda")))
                    {
                        //if (oCon.Turno) // si es verdadero lo crea
                        //{
                            MenuItem mnuNewMenuItem = new MenuItem();
                            mnuNewMenuItem.Value = drMenuItem.ItemArray[0].ToString();
                            mnuNewMenuItem.Text = CompletarTamanioItemMenu(drMenuItem.ItemArray[1].ToString().Replace("Edit", ""));
                            mnuNewMenuItem.NavigateUrl = "~/" + drMenuItem.ItemArray[3].ToString();
                            mnuMenuItem.ChildItems.Add(mnuNewMenuItem);
                            AddMenuItem(ref mnuNewMenuItem, dtMenuItem);
                        //}
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
