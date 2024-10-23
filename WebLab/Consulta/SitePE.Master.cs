using Business;
using Business.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Consulta
{
    public partial class SitePE : System.Web.UI.MasterPage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            if (Session["idUsuario"]== null)
              Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("../FinSesion.aspx", false);
                else
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    lblUsuario.Text = oUser.Apellido + " " + oUser.Nombre + " [" + oUser.IdEfector.Nombre + "]";
                }
               
            }
               

        }


        private int VerificaPermisos(string sObjeto)
        {
            int i_permiso = 0;

            Utility oUtil = new Utility();
            i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
            return i_permiso;

        }
    }
}