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
using System.Data.SqlClient;
using Business;
using System.Security.Principal;


namespace WebLab.PeticionElectronica
{
    public partial class Default : System.Web.UI.Page
    {       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

             
                if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")  IdentificarSSO();
                else IdentificarSGH();                
            }          
        }

        private void IdentificarSGH()
        {
         
            int idUsuarioLogueado = 2; // Request["idUsuario"].ToString();
            if (Request["idUsuario"] != null) idUsuarioLogueado =int.Parse( Request["idUsuario"].ToString());

            CrearLogAcceso(idUsuarioLogueado);
            CrearPermisos(idUsuarioLogueado);
            Session["idUsuario"] = idUsuarioLogueado;
            RedireccionarPeticion();
        
        }

        private void IdentificarSSO()
        {
          
            int idUsuarioLogueado = IdentificarUsuarioSSO();
            if (idUsuarioLogueado == 0)
                Salud.Security.SSO.SSOHelper.RedirectToSSOPage("../Login.aspx", Request.Url.ToString());
            else
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Salud.Security.SSO.SSOHelper.CurrentIdentity.Username);
                if (oUser != null)
                {
                    idUsuarioLogueado = oUser.IdUsuario;                                   
                    CrearLogAcceso(idUsuarioLogueado);
                    CrearPermisos(idUsuarioLogueado);
                    Session["idUsuario"] = idUsuarioLogueado;
                    RedireccionarPeticion();                    
                }
                else
                    Response.Redirect("../AccesoDenegado.htm");
            }
                
        }

        private void RedireccionarPeticion()
        {
            string idPaciente = "1";
            string idOrigen = "3";
            string diag = "";
            if (Request["idOrigen"] != null) idOrigen = Request["idOrigen"].ToString();
            if (Request["idPaciente"] != null) idPaciente = Request["idPaciente"].ToString();
            if (Request["Diagnostico"] != null) diag = Request["Diagnostico"].ToString();   

            Response.Redirect("PeticionEdit.aspx?idPaciente=" + idPaciente + "&idOrigen=" + idOrigen +"&Diagnostico="+diag, false);          
        }

        private int IdentificarUsuarioSSO()
        {
            Salud.Security.SSO.SSOHelper.Authenticate();
            if (Salud.Security.SSO.SSOHelper.CurrentIdentity == null)
            {
                // 1.1. No lo está. Debe redirigir al sitio de SSO
                // Redirigir ...
                return 0;
                //pnlSinUsuario.Visible = true;
            }
            else
            {
                return Salud.Security.SSO.SSOHelper.CurrentIdentity.Id;

            }
        }
       

        private void CrearLogAcceso(int idUsuarioLogueado)
        {
            LogAcceso RegistroAcceso = new LogAcceso();
            RegistroAcceso.IdUsuario = idUsuarioLogueado;
            RegistroAcceso.Fecha = DateTime.Now;
            RegistroAcceso.Save();
        }

        private void CrearPermisos(int p)
        {
            string m_strSQL = " SELECT  M.objeto, P.permiso " +
                              " FROM Sys_Perfil " +
                              " INNER JOIN   Sys_Usuario AS U ON Sys_Perfil.idPerfil = U.idPerfil " +
                              " INNER JOIN   Sys_Permiso AS P ON Sys_Perfil.idPerfil = P.idPerfil " +
                              " INNER JOIN   Sys_Menu AS M ON P.idMenu = M.idMenu " +
                              " WHERE   (M.habilitado = 1)  AND (M.idModulo = 2) and  (U.activo=1 )  AND (U.idUsuario =" + p.ToString() + ")";           

            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {
                DataSet Ds = new DataSet();
                //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];
                ArrayList l_permisos = new ArrayList();
                foreach (DataRow dr in dtPermisos.Rows)
                {
                    l_permisos.Add(dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString());
                    Session["s_permiso"] = l_permisos;       
                }
              //  conn.Close();
            }
            
        }

      
    }
}
