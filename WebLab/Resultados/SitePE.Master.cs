using Business;
using Business.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Resultados
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

                if (!Page.IsPostBack)
                {


                    if (VerificaPermisos("Historial de Resultados") > 0)
                        consulta.Visible = true;
                    else
                        consulta.Visible = false;

                    if (VerificaPermisos("Resultados Histocompatibilidad") > 0)
                        histo.Visible = true;
                    else
                        histo.Visible = false;


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