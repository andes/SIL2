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
using CrystalDecisions.Web;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;
using Business;
using System.Text;
using Business.Data;
//using OfficeOpenXml;

namespace WebLab.Estadisticas
{
    public partial class Produccion : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Session["idUsuario"] == null)
                Response.Redirect("logout.aspx", false);
            else
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            
            
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("logout.aspx", false);
                else
                {
              ///      PreventingDoubleSubmit(btnGenerar);
                    VerificaPermisos("De Produccion");
                    txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                    CargarListas();
                }
            }
        }
        //private void PreventingDoubleSubmit(Button button)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
        //    sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
        //    sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
        //    sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
        //    sb.Append("this.value = 'Processing...';");
        //    sb.Append("this.disabled = true;");
        //    sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
        //    sb.Append("return true;");

        //    string submit_Button_onclick_js = sb.ToString();
        //    button.Attributes.Add("onclick", submit_Button_onclick_js);
        //}
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
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de tipos de servicios
            string m_ssql = "SELECT   idOrigen, nombre  AS nombre FROM  LAB_Origen with (nolock) where baja=0 ORDER BY nombre";
            oUtil.CargarCheckBox(ChckOrigen, m_ssql, "idOrigen", "nombre");
            for (int i = 0; i < ChckOrigen.Items.Count; i++)
                ChckOrigen.Items[i].Selected = true;



            if (oUser.IdEfector.IdEfector == 227) // puede elegir el efector que quiere ver o todos
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E (nolock) " +
                     " INNER JOIN lab_Configuracion C (nolock) on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                ddlEfector.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E (nolock) where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }


            m_ssql = "SELECT     idArea, nombre  AS nombre FROM   LAB_Area (nolock) where baja=0  ORDER BY nombre";
            oUtil.CargarCheckBox(ChckArea, m_ssql, "idArea", "nombre");
            for (int i = 0; i < ChckArea.Items.Count; i++)
                ChckArea.Items[i].Selected = true;


            ///Carga de Sectores          
            m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre  FROM LAB_SectorServicio (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");
            for (int i = 0; i < lstSector.Items.Count; i++)            
                lstSector.Items[i].Selected = true;


            Habilitaacciones();

                        m_ssql = null;
            oUtil = null;
        }
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if ((ddlAgrupadoEfector.Enabled) && (ddlAgrupadoEfector.SelectedValue == "S"))
                    ImprimirReporteAgrupado();  ///tabla cruzada con los efectores
                else
                {
                    if (rblFormato.SelectedValue=="PDF")
                           ImprimirReporte();
                    else
                        dataTableAExcel(GetDataSet(), "Produccion");
                }
              
            } 
        }

        private void ImprimirReporteAgrupado()
        {
            DataTable dt   = GetDataSet();

              gvEstadistica.DataSource = dt;
                gvEstadistica.DataBind();

                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);

                Page page = new Page();
                HtmlForm form = new HtmlForm();
                gvEstadistica.EnableViewState = false;

                // Deshabilitar la validación de eventos, sólo asp.net 2
                page.EnableEventValidation = false;

                // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
                page.DesignerInitialize();
                page.Controls.Add(form);
                form.Controls.Add(gvEstadistica);
                page.RenderControl(htw);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=estadisticaProduccionTotal.xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End(); 
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //using (var package = new ExcelPackage())
            //{
            //    // Agregar una hoja de trabajo
            //    var worksheet = package.Workbook.Worksheets.Add("MiHoja");
            //    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
               

            //    // Guardar el archivo
            //    var file = new FileInfo("miarchivo.xlsx");
            //    package.SaveAs(file);
            //}


        }

        private void ImprimirReporte()
        {
            try
            {

                string informe = "Produccion2.rpt";

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                if (ddlEfector.SelectedValue == "0")
                {

                    //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = "Subsecretaria de Salud";

                    //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = "Laboratorios";

                    //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = "";
                }
                else
                {
                    Efector oEfector = new Efector();
                    oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oEfector);

                    encabezado1.Value = oCon.EncabezadoLinea1;
                    encabezado2.Value = oCon.EncabezadoLinea2;
                    encabezado3.Value = oCon.EncabezadoLinea3;


                }

                ParameterDiscreteValue rangeFechas = new ParameterDiscreteValue();
                rangeFechas.Value = txtFechaDesde.Value + " - " + txtFechaHasta.Value;

                oCr.Report.FileName = informe;
                oCr.ReportDocument.SetDataSource(GetDataSet());
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(rangeFechas);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = "ProduccionLaboratorio";
                if (ddlEfector.SelectedValue != "0")
                    nombrePDF = nombrePDF + "_" + ddlEfector.SelectedItem.Text;

                nombrePDF = oUtil.CompletarNombrePDF(nombrePDF);
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
            }
            catch (Exception ex)
            {
            }


        }
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
            {
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else
                { if (diferenciamayorunanio(DateTime.Parse(txtFechaDesde.Value), DateTime.Parse(txtFechaHasta.Value)) > 1)
                    {
                        CustomValidator1.Text = "No es posible generar información para mas de 1 año. Verifique.";                       
                        args.IsValid = false;
                    }
                    else args.IsValid = true; }
            }
        }

        private double diferenciamayorunanio(DateTime desde, DateTime hasta)
        {
            double dif = 0;
               TimeSpan diferencia = hasta - desde;

            // 365.2425 días es la duración media de un año gregoriano (considerando años bisiestos)
            dif=diferencia.TotalDays / 365.2425;
            return dif;


            //int años = hasta.Year - desde.Year;

            //// Si todavía no ha llegado el aniversario este año, restar 1
            //if (hasta.Month < desde.Month || (hasta.Month == desde.Month && hasta.Day < desde.Day))
            //{
            //    años--;
            //}

            //return años;
        }

        public DataTable GetDataSet()
        {       
            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_EstadisticaProduccion]";

            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");

            cmd.Parameters.Add("@idArea", SqlDbType.NVarChar);
            cmd.Parameters["@idArea"].Value = getListaAreas();

            cmd.Parameters.Add("@idOrigen", SqlDbType.NVarChar);
            cmd.Parameters["@idOrigen"].Value = getListaOrigen();

            cmd.Parameters.Add("@idSector", SqlDbType.NVarChar);
            cmd.Parameters["@idSector"].Value = getListaSectores();


            cmd.Parameters.Add("@idEfector", SqlDbType.Int);
            cmd.Parameters["@idEfector"].Value = int.Parse(ddlEfector.SelectedValue);

            cmd.Parameters.Add("@agrupado", SqlDbType.Bit);
            if (ddlAgrupadoEfector.SelectedValue == "S")
                cmd.Parameters["@agrupado"].Value = 1;  ///tabla cruzada con efectores
            else
                cmd.Parameters["@agrupado"].Value = 0;  ///agrupado por efectores

            cmd.Parameters.Add("@salida", SqlDbType.VarChar);
            cmd.Parameters["@salida"].Value = rblFormato.SelectedValue;
            //cmd.Parameters.Add("@derivacion", SqlDbType.Bit);
            //if (ddlDerivaciones.SelectedValue == "S")
            //    cmd.Parameters["@derivacion"].Value = 1;  ///tabla cruzada con efectores
            //else
            //    cmd.Parameters["@derivacion"].Value = 0;  ///agrupado por efectores

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);

            return Ds.Tables[0];
        }
        private object getListaSectores()
        {
            string m_lista = "";
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                if (lstSector.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstSector.Items[i].Value;
                    else
                        m_lista += "," + lstSector.Items[i].Value;
                }

            }
            return m_lista;
        }

        private string getListaOrigen()
        {
            
            string lista = "";
            for (int i = 0; i < this.ChckOrigen.Items.Count; i++)
            {
                if (ChckOrigen.Items[i].Selected)
                {
                    if (lista == "")
                        lista = ChckOrigen.Items[i].Value;
                    else
                        lista += "," + ChckOrigen.Items[i].Value;
                }

            }
            return lista;
        }

        private string getListaAreas()
        {
            string lista = "";
            for (int i = 0; i < this.ChckArea.Items.Count; i++)
            {
                if (ChckArea.Items[i].Selected)
                {
                    if (lista == "")
                        lista = ChckArea.Items[i].Value;
                    else
                        lista += ","+ChckArea.Items[i].Value;
                }

            }
            return lista;
        }

        //protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        dataTableAExcel(GetDataSet(), "ProduccionLaboratorio");
        //    }
        //}
        //private void dataTableAExcel(DataTable tabla, string nombreArchivo)
        //{
        //    if (tabla == null || tabla.Rows.Count == 0)
        //        return;

        //    // Si usas EPPlus 5+, descomenta esta línea:
        //    // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    using (ExcelPackage pck = new ExcelPackage())
        //    {
        //        var hoja = pck.Workbook.Worksheets.Add("Datos");

        //        // Cargar los datos desde la DataTable (true = incluye encabezados)
        //        hoja.Cells["A1"].LoadFromDataTable(tabla, true);

        //        // Ajustar el ancho de columnas
        //        hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

        //        // Crear archivo en memoria y enviarlo como respuesta
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            pck.SaveAs(ms);
        //            ms.Position = 0;

        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //            string nombreFinal = $"{nombreArchivo}_{oUser.IdEfector.IdEfector2}.xlsx";
        //            Response.AddHeader("content-disposition", "attachment;filename=" + nombreFinal);
        //            Response.BinaryWrite(ms.ToArray());
        //            Response.End();
        //        }
        //    }
        //}
        private void dataTableAExcel(DataTable tabla, string nombreArchivo)
        {
            if (tabla.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Page pagina = new Page();
                HtmlForm form = new HtmlForm();
                GridView dg = new GridView();
                dg.EnableViewState = false;
                dg.DataSource = tabla;
                dg.DataBind();
                pagina.EnableEventValidation = false;
                pagina.DesignerInitialize();
                pagina.Controls.Add(form);
                form.Controls.Add(dg);
                pagina.RenderControl(htw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreArchivo + "_" + oUser.IdEfector.IdEfector2 + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }
        //private void dataTableAExcel(DataTable tabla, string nombreArchivo)
        //{
        //    if (tabla == null || tabla.Rows.Count == 0)
        //        return;

        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        // Agregar la DataTable como hoja
        //        var hoja = wb.Worksheets.Add(tabla, "Datos");

        //        // Opcional: ajustar automáticamente el ancho de columnas
        //        hoja.Columns().AdjustToContents();

        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            wb.SaveAs(stream);
        //            stream.Position = 0;

        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.Charset = "";
        //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //            // Agregar nombre con sufijo del efector
        //            string nombreCompleto = $"{nombreArchivo}_{oUser.IdEfector.IdEfector2}.xlsx";
        //            Response.AddHeader("content-disposition", "attachment;filename=" + nombreCompleto);

        //            Response.BinaryWrite(stream.ToArray());
        //            Response.End();
        //        }
        //    }
        //}

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }
        private void MarcarSeleccionados(bool p)
        {
            for (int i = 0; i < ChckOrigen.Items.Count; i++)
                ChckOrigen.Items[i].Selected = p;

        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            MarcarAreasSeleccionados(true);
        }

        private void MarcarAreasSeleccionados(bool p)
        {
            for (int i = 0; i < ChckArea.Items.Count; i++)
                ChckArea.Items[i].Selected = p;

        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            MarcarAreasSeleccionados(false);
        }

        protected void cvOrigen_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool hay = false;
            for (int i = 0; i < ChckOrigen.Items.Count; i++)
            { if (ChckOrigen.Items[i].Selected) { hay = true; break; } }

           
                args.IsValid = hay;
                
        }

        protected void cvOrigen0_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool hay = false;
            for (int i = 0; i < ChckArea.Items.Count; i++)
            { if (ChckArea.Items[i].Selected) { hay = true; break; } }


            args.IsValid = hay;
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Habilitaacciones();
            
        }

        private void Habilitaacciones()
        {
            ddlAgrupadoEfector.Enabled = false;
            if (ddlEfector.SelectedValue == "0")
            {
                ddlAgrupadoEfector.Enabled = true;
                if (ddlAgrupadoEfector.SelectedValue == "S")
                {
                    rblFormato.SelectedValue = "Excel";
                    rblFormato.Enabled = false;
                }
                else
                    rblFormato.Enabled = true;
            }
            else
            {
                ddlAgrupadoEfector.SelectedValue = "N";
                rblFormato.Enabled = true;
            }

            ddlAgrupadoEfector.UpdateAfterCallBack = true;
            rblFormato.UpdateAfterCallBack = true;
        }

        protected void ddlAgrupadoEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Habilitaacciones();
        }
    }
}
