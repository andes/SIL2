using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Informes
{
    public partial class ResultadosporServicio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

             if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    Inicializar();
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
        }


        private void Inicializar()
        {
           
            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
           
            CargarListas();
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();

       

      

            ///Carga de Sectores
          string  m_ssql = "SELECT idSectorServicio,  nombre  + ' - ' + prefijo as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("-- Seleccione --", "0"));

          

            //
            m_ssql = null;
            oUtil = null;
        }

        protected void btnBuscarControl_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            Response.Redirect("../Resultados/ResultadoViewList.aspx?idSector=" + ddlSectorServicio.SelectedValue + "&fecha=" + txtFechaDesde.Value+"&validado=1&Operacion=HC");
        }
    }
}