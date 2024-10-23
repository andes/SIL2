using Business;
using Business.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.PeticionElectronica
{
    public partial class SitePE : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                lblUsuario.Text = oUser.Apellido + " " + oUser.Nombre;

                if (VerificaPermisos("Validacion Externo") == 0)
                    lblAccesoValidacion.Visible = false;

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