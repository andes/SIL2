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
using Business.Data.Laboratorio;

namespace WebLab
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            else
                Response.Redirect("FinSesion.aspx", false);
                //Configuracion oC = new Configuracion();
                //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
                //if (oUser.IdEfector != oC.IdEfector) // es externo
                //{
                //    this.MasterPageFile = "~/PeticionElectronica/SitePE.master";

                //}

            }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Session["idUsuario"] != null)
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                    //Configuracion oC = new Configuracion();
                    //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
                    //if (oUser.IdEfector != oC.IdEfector) // es externo
                    //{
                     
                    //    pnlTitulo.Attributes.Add("class", "panel panel-success");
                    //}

                    if (Request["Operacion"] == null)
                    //////////nuevo login
                    {                       
                        lblSubtitulo.Text = "";                        
                    }
                    else
                    ///////////////validacion
                    {
                        if (Request["idServicio"].ToString() == "6")
                        {
                            pnlTitulo.Attributes.Add("class", "panel panel-success");
                          
                        }
                        lblSubtitulo.Text = "Usted está ingresando a validación de resultados. Ingrese su usuario y contraseña de firma electrónica.";
                    }     
                }
                else
                    Response.Redirect("FinSesion.aspx", false);

            }
            
        }

       
    }
}
