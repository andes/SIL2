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

namespace WebLab.Estadisticas
{
    public partial class ResultadoDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            VerificaPermisos("Por Resultados");
        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                        case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void imgResultadoPredefinido_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ReportePorREsultado.aspx", false);
        }

        protected void imgResultadoNumerico_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ReportePorREsultadoNum.aspx", false);
        }
    }
}
