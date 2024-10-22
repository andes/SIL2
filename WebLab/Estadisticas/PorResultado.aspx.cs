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
using CrystalDecisions.Web;
using System.Data.SqlClient;
using Business.Data.Laboratorio;
using System.IO;
using CrystalDecisions.Shared;

namespace WebLab.Estadisticas
{
    public partial class PorResultado : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            oCr.ReportDocument.PrintOptions.PrinterName = oC.NombreImpresora;// System.Configuration.ConfigurationSettings.AppSettings["Impresora"]; 
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarListas();
                txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();

            }
        }




        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));

            CargarArea();
            CargarAnalisis();
            MostrarDescripcion();

       

            m_ssql = null;
            oUtil = null;
        }


        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea(); CargarAnalisis();

            
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "";
            if (ddlServicio.SelectedValue == "0")
                m_ssql = "select idArea, nombre from Lab_Area where baja=0  order by nombre";
            else
                m_ssql = "select idArea, nombre from Lab_Area where baja=0 and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));
            ddlArea.UpdateAfterCallBack = true;
            m_ssql = null;
            oUtil = null;

        }

        

        private object getListaAnalisis()
        {
            string listaAnalisis="";
            for (int i = 0; i < lstAnalisis.Items.Count; i++)
            {
                if (lstAnalisis.Items[i].Selected)
                {
                    if (listaAnalisis == "")
                        listaAnalisis = lstAnalisis.Items[i].Value;
                    else
                        listaAnalisis += ","+lstAnalisis.Items[i].Value;
                }
            }
            return listaAnalisis;
        }

        



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Request["Tipo"].ToString() == "MuestrasFaltantes")
                {
                    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

                    
                }
            }

        }

      

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx", false);
        }

        

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                Response.Redirect("GeneralReporte.aspx?idAnalisis=" + getListaAnalisis()+"&informe=PorResultado&tipo=" + rdbTipoInforme.SelectedValue + "&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value +  "&idTipoServicio=" + ddlServicio.SelectedValue + "&idArea=" + ddlArea.SelectedValue, false);
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            CargarAnalisis();
            
        }

        private void CargarAnalisis()
        {

            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = " SELECT     I.idItem as idItem,  I.descripcion as nombre " +
                         "  FROM         LAB_Item AS I " +
                         " INNER JOIN      LAB_Area AS A ON I.idArea = A.idArea" +
                         "  WHERE   I.baja=0 ";

            if (ddlServicio.SelectedValue != "0")
                m_ssql += " AND A.idtipoServicio="+ ddlServicio.SelectedValue;
            if (ddlArea.SelectedValue != "0")
                m_ssql += " AND I.idArea=" + ddlArea.SelectedValue;
            if (rdbTipoInforme.Items[0].Selected) //resultados predefinidos
                m_ssql += " AND (I.idTipoResultado = 3)";
            if (rdbTipoInforme.Items[1].Selected) //resultados numericos
                m_ssql += " AND (I.idTipoResultado = 1)";

            m_ssql += " order by I.nombre ";



            oUtil.CargarListBox(lstAnalisis, m_ssql, "idItem", "nombre");

            lstAnalisis.UpdateAfterCallBack = true;
            m_ssql = null;
            oUtil = null;


        }

      

        protected void rdbTipoInforme_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarAnalisis();
            MostrarDescripcion();
        }

        private void MostrarDescripcion()
        {
         

            if (rdbTipoInforme.SelectedValue == "0") lblDescripcion.Text = "Cantidad de Casos Encontrados: Muestra para los tipos de analisis, cuyos resultados son predefinidos, la cantidad de resultados obtenidos en cada caso.";
            if (rdbTipoInforme.SelectedValue == "1") lblDescripcion.Text = "Promedio de Resultados: Muestra, para los analisis cuyo resultados son numericos, el promedio de resultados.";
                 
            lblDescripcion.Visible = true;
            lblDescripcion.UpdateAfterCallBack = true;
        }

        protected void chkTodos_CheckedChanged(object sender, EventArgs e)
        {            

                Marcar(chkTodos.Checked);

        }

        private void Marcar(bool p)
        {
            
            for (int i = 0; i < lstAnalisis.Items.Count; i++)
            {
                lstAnalisis.Items[i].Selected=p;                
            }
            lstAnalisis.UpdateAfterCallBack = true;
            
        }
    }
}
