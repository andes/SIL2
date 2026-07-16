using Business;
using Business.Data;
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

namespace WebLab
{
    public partial class LoginEfector : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuarioAux"] != null)
            {
                if (!Page.IsPostBack)
                {

                    Usuario oRegistro = new Usuario();
                    oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuarioAux"].ToString()));
                    lblUsuario.Text = oRegistro.Username;
                    lblNombre.Text = oRegistro.Nombre + " " + oRegistro.Apellido;          
                    CargarListas(oRegistro);
                }
            }
            else Response.Redirect("FinSesion.aspx", false);

       
        }

         
        private void CargarListas(Usuario oRegistro)
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            Utility oUtil = new Utility();

            //            string m_ssql = @"  SELECT idEfector, nombre FROM Sys_Efector (nolock)
            //where idEfector in (select idEfector from sys_UsuarioEfector (nolock) where activo=1 and idUsuario=" + oRegistro.IdUsuario.ToString()+ @") 
            //--and  idEfector in (select idEfector from lab_configuracion (nolock) ) 
            //ORDER BY nombre ";

            //Usuario Tercera Parte:Se carga en el combo el efector, idarea, perfil y efector destino con el que puede ingresar el usuario

            string m_ssql = @" Select
                                 CAST(UE.idEfector AS varchar(10)) + '|' + 
                                 CAST(UE.idarea AS varchar(10)) + '|' +
                                 CAST(UE.idPerfil AS varchar(10)) + '|' + 
                                 CAST(UE.idEfectorDestino AS varchar(10))    AS idEfector , 
                                 E.nombre 
                                FROM sys_UsuarioEfector  UE with (nolock)
                                inner join sys_efector  E with (nolock) on E.idefector= UE.idefector
                                where UE.activo=1 and UE.idUsuario=" + oRegistro.IdUsuario.ToString() + @" 
                                ORDER BY E.nombre ";


            if (oRegistro.Administrador)              
                m_ssql = @"  SELECT    CAST(idEfector AS varchar(10)) + '|0|2|' + CAST(idEfector AS varchar(10)) AS idEfector,    nombre
                            FROM Sys_Efector WITH(NOLOCK)
                            WHERE idEfector IN  (    SELECT idEfector   FROM lab_configuracion WITH(NOLOCK)    
                    UNION    SELECT 227) ORDER BY nombre";



            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            

            if (ddlEfector.Items.Count == 0)
                Response.Redirect("AccesoDenegado.aspx?mensaje=Efector no Habilitado. Verifique con el Administrador.", false);
            else
            {
                if (ddlEfector.Items.Count > 1)
                    ddlEfector.Items.Insert(0, new ListItem("--Seleccione un efector--", "0"));
                else
                { ///si tiene un solo efector se selecciona e ingresa
                    SeleccionarEfector(oRegistro);                  
                }
            }

            
        }

        private void SeleccionarEfector(Usuario oRegistro)
        {            
            string[] datos = ddlEfector.SelectedValue.Split('|');

            int idEfector = int.Parse(datos[0]);
            int idArea = int.Parse(datos[1]);
            int idPerfil = int.Parse(datos[2]);
            int idEfectorDestino = int.Parse(datos[3]);

            // Se actualiza Sys_Usuario con el efector, idarea, perfil y efector destino seleccionados.            
            // Sys_Usuario.IdEfector e IdPerfil.            
            oRegistro.IdEfector = new Efector { IdEfector = idEfector };
            oRegistro.IdArea = idArea;
            oRegistro.IdPerfil = new Perfil { IdPerfil = idPerfil };
            oRegistro.IdEfectorDestino = new Efector { IdEfector = idEfectorDestino };
            oRegistro.Save();            

            Session["idUsuario"] = oRegistro.IdUsuario.ToString();
            Response.Redirect("Default.aspx", false);
            return;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ddlEfector.SelectedValue != "0")
                {
                    if (Session["idUsuarioAux"] != null)
                    {
                        Usuario oRegistro = new Usuario();
                        oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Session["idUsuarioAux"].ToString()));
                        SeleccionarEfector( oRegistro);                     
                    }
                    else Response.Redirect("FinSesion.aspx", false);
                }
            }
        }
    }
}
