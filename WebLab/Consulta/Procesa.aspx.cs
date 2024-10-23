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
using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using Business.Data;

namespace WebLab.Consulta
{
    public partial class Procesa : System.Web.UI.Page
    {    
        
        protected void Page_PreInit(object sender, EventArgs e)
        {
          
           

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 
                if (Session["idUsuario"] != null)
                {
                    
                            CargarGrilla();
                    
                
                    this.hypRegresar.NavigateUrl = "HistoriaClinicaFiltro.aspx";
                  
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
                
            }
            
            

        }

         

        private void CargarGrilla()
        {   
            string param=  Request["Parametros"].ToString();
           
            string ordenProtocolo =   " , P.numero ";
           

            string m_strSQL = " Select   P.idProtocolo " + ordenProtocolo +// +  // ,  dbo.NumeroProtocolo(P.idProtocolo) as numero, P.fecha 
                                      "  from LAB_ResultadoEncabezado P " +
                                        " WHERE   " + param +            " order by P.idprotocolo desc ";
            string conn = ConfigurationManager.ConnectionStrings["SIPS"].ConnectionString;

             
            SqlConnection connection = new SqlConnection(conn);

        
            DataSet Ds = new DataSet();
         //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, connection);
            adapter.Fill(Ds);


            string primerProtocolo = "";
             string m_listaProtocolo = "";
            if (Ds.Tables[0].Rows.Count > 0)
            {
                int cantidadRegistros = Ds.Tables[0].Rows.Count;
                if (cantidadRegistros <= 10000)
                {
                    for (int i = 0; i < cantidadRegistros; i++)
                    {


                        if (m_listaProtocolo == "")
                        {
                            m_listaProtocolo = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                            primerProtocolo = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
                        }
                        else
                            m_listaProtocolo += "," + Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    }
                 
                    Session["Parametros"] = m_listaProtocolo;
                    Session["idProtocolo"] = primerProtocolo;



                    Response.Redirect("Resultadoview.aspx?Index=0&Operacion=HC",false);
                  

                }
                else lblTitulo.Text = "La búsqueda ha superado el límite de procesamiento para la operación que desea realizar. Acote los filtros de búsqueda. Si cree que este mensaje es un error, póngase en contacto con el soporte del SIL.";
                }
            

            else
                lblTitulo.Text = "No se encontraron protocolos para los filtros ingresados";

        }



        //private int IdentificarUsuarioSSO()
        //{
        //    Salud.Security.SSO.SSOHelper.Authenticate();
        //    if (Salud.Security.SSO.SSOHelper.CurrentIdentity == null)
        //    {
        //        // 1.1. No lo está. Debe redirigir al sitio de SSO
        //        // Redirigir ...
        //        return 0;
        //        //pnlSinUsuario.Visible = true;
        //    }
        //    else
        //    {
        //        return Salud.Security.SSO.SSOHelper.CurrentIdentity.Id;

        //    }
        //}

        //private void CrearLogAcceso(int idUsuarioLogueado)
        //{
        //    LogAcceso RegistroAcceso = new LogAcceso();
        //    RegistroAcceso.IdUsuario = idUsuarioLogueado;
        //    RegistroAcceso.Fecha = DateTime.Now;
        //    RegistroAcceso.Save();
        //}

        //private void CrearPermisos(int p)
        //{
        //    string m_strSQL = " SELECT  M.objeto, P.permiso " +
        //                      " FROM Sys_Perfil " +
        //                      " INNER JOIN   Sys_Usuario AS U ON Sys_Perfil.idPerfil = U.idPerfil " +
        //                      " INNER JOIN   Sys_Permiso AS P ON Sys_Perfil.idPerfil = P.idPerfil " +
        //                      " INNER JOIN   Sys_Menu AS M ON P.idMenu = M.idMenu " +
        //                      " WHERE (M.habilitado = 1)  AND (M.idModulo = 2) and  (U.activo=1 )  AND (U.idUsuario =" + p.ToString() + ")";

        //   // using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
        //    //{
        //        DataSet Ds = new DataSet();
        //        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //        adapter.Fill(Ds);

        //        DataTable dtPermisos = Ds.Tables[0];
        //        ArrayList l_permisos;
        //        l_permisos = new ArrayList();
        //        foreach (DataRow dr in dtPermisos.Rows)
        //        {
        //            l_permisos.Add(dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString());
        //            Session["s_permiso"] = l_permisos;
        //        }
        //        //conn.Close();
        //    //}

        //}

//        private void IdentificarSSO()
//        {
//            ///////Simula Log In//////
//            //string sessionId = "admin";
//            //HttpContext.Current.User = new GenericPrincipal(new Salud.Security.SSO.SSOIdentity(new HttpCookie("SSO_AUTH_COOKIE", sessionId)), null);
//            ////////////////////////////            
//            int idUsuarioLogueado = IdentificarUsuarioSSO();
//            if (idUsuarioLogueado == 0)
//                Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Login.aspx", Request.Url.ToString());
//            else
//            {
//                Usuario oUser = new Usuario();
//                oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Salud.Security.SSO.SSOHelper.CurrentIdentity.Username);
//                if (oUser != null)
//                {
//                    idUsuarioLogueado = oUser.IdUsuario;

//                    string s_perfil = oUser.IdPerfil.Nombre;

//                    //object perfil = Salud.Security.SSO.SSOHelper.CurrentIdentity.GetSetting(s_perfil);
//                    //if (perfil == null) Salud.Security.SSO.SSOHelper.RedirectToErrorPage(403,0,"El usuario no tiene permisos de acceso al sistema de laboratorio. Comuniquese con el Administrador del Sistema.");
//                    //else
//                    //{    
//                    CrearLogAcceso(idUsuarioLogueado);
//                    CrearPermisos(idUsuarioLogueado);
//                    Session["idUsuario"] = idUsuarioLogueado;
////                    Response.Redirect("Principal.aspx", false);
//                }
//                else
//                    Response.Redirect("../AccesoDenegado.htm");
//            }

//        }


    }
}
