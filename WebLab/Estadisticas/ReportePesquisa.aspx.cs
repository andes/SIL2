using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;
using InfoSoftGlobal;
using System.Text;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections;

namespace WebLab.Estadisticas
{
    public partial class ReportePesquisa : System.Web.UI.Page
    {
        int suma1 = 0; int suma2 = 0; int suma3 = 0; int suma4 = 0; int suma5 = 0; int suma6 = 0; //int suma7 = 0; int suma8 = 0; int suma9 = 0; int suma10 = 0;
        int grupo1 = 0; int grupo2 = 0; int grupo3 = 0; //int grupo4 = 0; int grupo5 = 0; int grupo6 = 0; int grupo7 = 0; int grupo8 = 0; int grupo9 = 0; int grupo10 = 0;
        int gv31 = 0; int gv32 = 0; int gv33 = 0; int gv34 = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("De Pesquisa Neonatal");
                txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();     
            }
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ddlResultado.Visible)
                {
                    if (ddlResultado.SelectedValue != "0")
                        MostrarPorResultado();
                }
                else
                    MostrarSegunVariables();
            }
            
        }
         
        private DataTable GetDatosEstadistica(int s_tipo, string s_item)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "LAB_EstadisticasPesquisa";

            cmd.Parameters.Add("@tipo", SqlDbType.Int);
            cmd.Parameters["@tipo"].Value = s_tipo;

            DateTime fecha1 = DateTime.Parse("01/01/1900");  DateTime fecha2 = DateTime.Now;
            
            if (txtFechaDesde.Value != "")                
             fecha1 = DateTime.Parse(txtFechaDesde.Value);
            if (txtFechaHasta.Value!="")
             fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");


            cmd.Parameters.Add("@item", SqlDbType.NVarChar);
            cmd.Parameters["@item"].Value = s_item;

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            return Ds.Tables[0];

        }

        protected void gv1_DataBound(object sender, EventArgs e)
        {

        }

        protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") grupo1 += int.Parse(e.Row.Cells[1].Text);
                if (e.Row.Cells[2].Text != "&nbsp;") grupo2 += int.Parse(e.Row.Cells[2].Text);
                if (e.Row.Cells[3].Text != "&nbsp;") grupo3 += int.Parse(e.Row.Cells[3].Text);                

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = grupo1.ToString();
                e.Row.Cells[2].Text = grupo2.ToString();
                e.Row.Cells[3].Text = grupo3.ToString();                
            }
        }

        protected void gv2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") suma1 += int.Parse(e.Row.Cells[1].Text);
                if (e.Row.Cells[2].Text != "&nbsp;") suma2 += int.Parse(e.Row.Cells[2].Text);
                if (e.Row.Cells[3].Text != "&nbsp;") suma3 += int.Parse(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;") suma4 += int.Parse(e.Row.Cells[4].Text);
                if (e.Row.Cells[5].Text != "&nbsp;") suma5 += int.Parse(e.Row.Cells[5].Text);
                if (e.Row.Cells[6].Text != "&nbsp;") suma6 += int.Parse(e.Row.Cells[6].Text);


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma1.ToString();
                e.Row.Cells[2].Text = suma2.ToString();
                e.Row.Cells[3].Text = suma3.ToString();
                e.Row.Cells[4].Text = suma4.ToString();
                e.Row.Cells[5].Text = suma5.ToString();
                e.Row.Cells[6].Text = suma6.ToString();
            }
        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel1(ddlVariable.SelectedValue);
        }       

        protected void imgExcel0_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel2(ddlVariable.SelectedValue);

        }
        
        private void ExportarExcel1(string nombreArchivo)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gv1.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gv1);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=PesquisaMuestras.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
        private void ExportarExcel2(string nombreArchivo)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gv2.EnableViewState = false;
            
            page.EnableEventValidation = false;
            
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gv2);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename="+nombreArchivo+".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
        private void ExportarExcel3(string nombreArchivo)
        {
            if (ddlResultado.SelectedValue != "0")
                nombreArchivo += "_" + ddlResultado.SelectedValue;
           

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gv3.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gv3);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename="+nombreArchivo+".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void ddlVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            /////datos de resultados por analisis 
            //int tipoRreporte = 2;
            MostrarSegunVariables();
        }

        private void MostrarSegunVariables()
        {

            pnlGeneral.Visible = true;
            muestraPesquisadas.Visible = false;
            muestraporpatologia.Visible = false;
            ddlResultado.Visible = false;
            DataTable dt3 = new DataTable();
            switch (ddlVariable.SelectedValue)           
            {
                case "Muestras Pesquisadas":
                    {
                        pnlGeneral.Visible = false;
                        muestraPesquisadas.Visible = true;
                        DataTable dt1 = GetDatosEstadistica(0,"");
                        gv1.DataSource = dt1;
                        gv1.DataBind();
                        if (dt1.Rows.Count > 0)
                        {
                            Literal1.Text = mostrarGrafico("Muestras recibidas");
                            Literal2.Text = mostrarGrafico1(dt1,"Muestras recibidas por efector");
                        }
                    } break;
                case "Muestras por Patologia":
                    {
                        pnlGeneral.Visible = false;
                        muestraporpatologia.Visible = true;
                        DataTable dt2 = GetDatosEstadistica(1, "");
                        gv2.DataSource = dt2;
                        gv2.DataBind();
                        
                    }break;
                case "Repeticiones":   dt3 = GetDatosEstadistica(2,""); break;                           
                case "Tipo de Alimentacion": dt3 = GetDatosEstadistica(3,"");break;
                case "Antibiotico":  dt3 = GetDatosEstadistica(4,""); break;
                case "Ingesta Leche 24hs.":  dt3 = GetDatosEstadistica(5,"");break;            
                case "Transfusion":  dt3 = GetDatosEstadistica(6,"");break;
                case  "Corticoides":dt3 = GetDatosEstadistica(7,""); break;
                case "Dopamina": dt3 = GetDatosEstadistica(8,""); break;
                case "Corticoides Materno": dt3 = GetDatosEstadistica(9,""); break;
                case "Resultado":
                    {
                        Literal3.Visible = false;
                        Literal4.Visible = false;
                        Literal5.Visible = false; CargarResultados();
                    } break;
            }  
            gv3.DataSource = dt3;
            gv3.DataBind();
            if (dt3.Rows.Count > 0)
            {
                 if (ddlVariable.SelectedValue!= "Resultado")
                    {
                        Literal3.Visible = true;
                        Literal4.Visible = true;
                        Literal5.Visible = true; 
                    
                    Literal3.Text = mostrarGrafico3(dt3, ddlVariable.SelectedValue);
                    Literal4.Text = mostrarGrafico4(dt3, ddlVariable.SelectedValue,"220","200");
                    Literal5.Text = mostrarGrafico5(dt3,"Distribución por Sexo");
                    }
            }
            
            
        }

        private void CargarResultados()
        {
            ddlResultado.Visible = true;                     
            Utility oUtil = new Utility();            
            string m_ssql = @"select  I.nombre as nombre from lab_item  as I inner join lab_area  as A on A.idArea= I.idArea where a.idTipoServicio=4 and I.idtiporesultado=2 and I.baja=0 ";
            oUtil.CargarCombo(ddlResultado, m_ssql, "nombre", "nombre");
            ddlResultado.Items.Insert(0, new ListItem("--Seleccione Práctica--", "0"));                                                       
            m_ssql = null;
            oUtil = null;       
        }

        private string mostrarGrafico(string s_titulo)// (DataTable dt1)
        {         
            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf";                        
            string strXML =  "<graph caption='" + s_titulo + "' subCaption='' showPercentageInLabel='1' pieSliceDepth='5'  decimalPrecision='0' showNames='1'>";
            strXML += "<set name='Primera Muestra' value='" +  grupo1.ToString() + "' />";
            strXML += "<set name='Repeticion' value='" + grupo2.ToString() + "' />";              
            strXML += "</graph>";
            return FusionCharts.RenderChart(s_tipografico, "", strXML, "pesquisa", "400", "200", false, false);
        }
        private string mostrarGrafico1(DataTable dt1, string s_titulo)
        {
            string s_tipografico = "../FusionCharts/FCF_Column3D.swf";            
            string strXML =  "<graph caption='" + s_titulo + "' subCaption='' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";
            for (int i = 0; i < dt1.Rows.Count; i++)
                strXML += "<set name='" + dt1.Rows[i][0].ToString().Substring(2, 4).Replace("\r\n", "") + "' value='" + dt1.Rows[i][3].ToString() + "' />";                                        
            strXML += "</graph>";
            return FusionCharts.RenderChart(s_tipografico, "2", strXML, "pesquisa2", "400", "300", false, false);
        }        
        private string mostrarGrafico4(DataTable dt1, string s_titulo, string ancho, string alto)
        {
            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf";
            string strXML = "<graph caption='" + s_titulo + "' subCaption='' showPercentageInLabel='1' pieSliceDepth='5'  decimalPrecision='0' showNames='1'>";                       
            for (int i = 0; i < dt1.Rows.Count; i++)  strXML += "<set name='" + dt1.Rows[i][0].ToString().Replace("\r\n","") + "' value='" + dt1.Rows[i][4].ToString() + "' />";                            
            strXML += "</graph>";
            return FusionCharts.RenderChart(s_tipografico, "", strXML, "pesquisa", ancho,alto, false, false);
        }   
        private string mostrarGrafico3(DataTable dt1,  string s_titulo)
        {                     

            string strXML = "<graph caption='" + s_titulo + "' formatNumberScale='0' decimalPrecision='0'  showPercentageInLabel='1' >";
            string strCategories = "<categories>";

            string strDataR = "<dataset seriesName='Femenino' color='9D080D'>";
            string strDataS = "<dataset seriesName='Masculino' color='F6BD0F'>";
            string strDataI = "<dataset seriesName='Indeterminado' color='AFD8F8'>";

             for (int i = 0; i < dt1.Rows.Count; i++)                
             {
                 strCategories = strCategories + "<category name='" + dt1.Rows[i][0].ToString().Replace("\r\n", "") + "' />";
                if (dt1.Rows[i][1].ToString() == "") strDataR = strDataR + "<set value='0' />";
                else   strDataR = strDataR + "<set value='" + dt1.Rows[i][1].ToString() + "' />";

                if (dt1.Rows[i][2].ToString() == "")  strDataS = strDataS + "<set value='0' />";
                else   strDataS = strDataS + "<set value='" + dt1.Rows[i][2].ToString() + "' />";

                if (dt1.Rows[i][3].ToString() == "") strDataI = strDataI + "<set value='0' />";
                else   strDataI = strDataI + "<set value='" + dt1.Rows[i][3].ToString() + "' />";               
            }

            //'Close <categories> element
            strCategories = strCategories + "</categories>";

            //'Close <dataset> elements
            strDataR = strDataR + "</dataset>";
            strDataS = strDataS + "</dataset>";
            strDataI = strDataI + "</dataset>";

            //'Assemble the entire XML now
            strXML = strXML + strCategories + strDataR + strDataS + strDataI + "</graph>";

            return FusionCharts.RenderChart("../FusionCharts/FCF_MSColumn3D.swf", "", strXML, "productSales", "400", "400", false, false);
        }
        private string mostrarGrafico5(DataTable dt1, string p)
        {
            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf";
            string strXML =  "<graph caption='" + p + "' subCaption='' showPercentageInLabel='1' pieSliceDepth='8'  decimalPrecision='0' showNames='1'>";
            if (dt1.Rows.Count > 0)
            {
                strXML += "<set name='Fem.' value='" + gv31 + "' />";
                strXML += "<set name='Mas.' value='" + gv32 + "' />";
                strXML += "<set name='Ind.' value='" + gv33 + "' />";
            }
            strXML += "</graph>";
            return FusionCharts.RenderChart(s_tipografico, "", strXML, "pesquisaSexo", "200", "200", false, false);
        }

        protected void gv3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") gv31 += int.Parse(e.Row.Cells[1].Text);
                if (e.Row.Cells[2].Text != "&nbsp;") gv32 += int.Parse(e.Row.Cells[2].Text);
                if (e.Row.Cells[3].Text != "&nbsp;") gv33 += int.Parse(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;") gv34 += int.Parse(e.Row.Cells[4].Text);                
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text =gv31.ToString();
                e.Row.Cells[2].Text = gv32.ToString();
                e.Row.Cells[3].Text = gv33.ToString();
                e.Row.Cells[4].Text = gv34.ToString();                
            }
        }

        protected void gv3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void imgExcel1_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel3(ddlVariable.SelectedValue);
        }

        protected void ddlResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarPorResultado();
            

        }

        private void MostrarPorResultado()
        {
            DataTable dt3 = GetDatosEstadistica(10, ddlResultado.SelectedValue);
            gv3.DataSource = dt3;
            gv3.DataBind();

            if (dt3.Rows.Count > 0)
            {
                Literal3.Visible = false;
                Literal5.Visible = false;
                Literal4.Visible = true;                
                Literal4.Text = mostrarGrafico4(dt3, ddlResultado.SelectedValue, "600","400");
                
            }
        }

        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
            }
            catch
            {
                args.IsValid = false;
                cvFechas.ErrorMessage = "Fechas inválidas";
            }
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;

        }

        protected void imgExcelDetallePacientes_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource =  GetDatosEstadistica(11, "");
            dg.DataBind();

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(dg);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=PacientesPesquisaNeonatal.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}