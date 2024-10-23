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
using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;
using CrystalDecisions.Web;

namespace WebLab.Derivaciones
{
    public partial class Derivados : System.Web.UI.Page
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 if (Request["tipo"] == "informe")
                {
                    lblTitulo.Text = "DERIVACIONES";
                    VerificaPermisos("Gestionar");
                }
                if (Request["tipo"] == "resultado")
                {
                    lblTitulo.Text = "CARGA DE RESULTADOS DE DERIVACIONES";
                    VerificaPermisos("Resultados");
                }
                CargarListas();
                txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();

               
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
  CargarArea();

            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("Todos", "0"));

            ///Carga de combos de Prioridad
            m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad WHERE     (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre");
            ddlPrioridad.Items.Insert(0, new ListItem("Todos", "0"));


            m_ssql = "SELECT TOP (100) PERCENT E.idEfector, E.nombre " +
               " FROM  dbo.Sys_Efector AS E " +
               " INNER JOIN    dbo.LAB_Configuracion AS C ON E.idEfector <> C.idEfector" +
               " WHERE  (E.idEfector IN" +
                        " (SELECT DISTINCT idEfectorDerivacion FROM  dbo.LAB_Item AS I    WHERE      (baja = 0)))" +
               "    ORDER BY E.nombre";
            oUtil.CargarListBox(lstEfectores, m_ssql, "idEfector", "nombre");
           
          

            m_ssql = null;
            oUtil = null;
        }

    


       

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "";
            if (ddlServicio.SelectedValue!="0")
 m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            else
                m_ssql = "select idArea, nombre from Lab_Area where baja=0   order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));

            m_ssql = null;
            oUtil = null;
           

        }

       

        private string ListaEfectores()
        {
            string m_lista = "";
            for (int i=0;i< lstEfectores.Items.Count;i++)
            {
                if (lstEfectores.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstEfectores.Items[i].Value;
                    else
                        m_lista +=","+ lstEfectores.Items[i].Value;
                }

            }
            return m_lista;
        }



        protected void lnkPDF_Click(object sender, EventArgs e)
        {
           // if (Page.IsValid) MostrarInforme("PDF");
        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid) MostrarInforme("Imprimir");
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        private void MarcarSeleccionados(bool p)
        {
            for (int i = 0; i <lstEfectores.Items.Count; i++)
            {
                lstEfectores.Items[i].Selected = p;
            }


        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
            string str_condicion = " 1= 1 AND fecha>='" + fecha1.ToString("yyyyMMdd") + "' and fecha<='" + fecha2.ToString("yyyyMMdd") + "'";

            //if (txtProtocoloDesde.Value != "") str_condicion += " AND P.numero >= " + txtProtocoloDesde.Value;
            //if (txtProtocoloHasta.Value != "") str_condicion += " AND P.numero <= " + txtProtocoloHasta.Value;
            if (ddlOrigen.SelectedValue != "0") str_condicion += " AND idOrigen = " + ddlOrigen.SelectedValue;
            if (ddlPrioridad.SelectedValue != "0") str_condicion += " AND idPrioridad = " + ddlPrioridad.SelectedValue;
            if (ddlServicio.SelectedValue != "0") str_condicion += " AND idTipoServicio = " + ddlServicio.SelectedValue;
            if (ddlArea.SelectedValue != "0") str_condicion += " AND idArea = " + ddlArea.SelectedValue;

            string m_lista = ListaEfectores();
            if (m_lista != "")
                str_condicion += " AND idEfector in ( " + m_lista + ")";

            if (Request["tipo"]=="informe")
                Response.Redirect("InformeList.aspx?Parametros=" + str_condicion, false);
            if (Request["tipo"] == "resultado")
                Response.Redirect("../Derivaciones/ResultadoEdit.aspx?Parametros=" + str_condicion, false);
        }
      
    }
}
