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
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.CasoFiliacion
{
    public partial class MedulaMenu : System.Web.UI.Page
    {

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            //   if (Request["id"] != null)
            {
                VerificaPermisos("Casos Histocompatibilidad");
                HttpContext Context;

                Context = HttpContext.Current;
                Context.Items.Add("idServicio", "3");
                Server.Transfer("CasoList.aspx");



            }
        }
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnNuevoCaso.Enabled = false;
                            btnNuevoCaso0.Visible = false;
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        

       


    

        protected void btnNuevoCaso_Click(object sender, EventArgs e)
        {
            Response.Redirect("CasoNewPrueba.aspx", false);
        }

        protected void btnNuevoCaso0_Click(object sender, EventArgs e)
        {
            Response.Redirect("CasoList.aspx?idServicio=3", false);
        }
    }
}
