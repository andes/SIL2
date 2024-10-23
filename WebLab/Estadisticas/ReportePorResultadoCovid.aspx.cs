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
using System.Data.SqlClient;
using Business;
using System.Text;
using System.IO;
using Business.Data;
using CrystalDecisions.Shared;
using Business.Data.Laboratorio;

namespace WebLab.Estadisticas
{
    public partial class ReportePorResultadoCovid : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public CrystalReportSource oCr = new CrystalReportSource();
        int suma1 = 0;
        int grupo1 = 0; int grupo2 = 0; int grupo3 = 0; int grupo4=0; int grupo5=0; int grupo6=0; int grupo7=0; int grupo8=0; int grupo9=0; int grupo10=0;
        int grupo11 = 0; int grupo12 = 0; int grupo13 = 0;  
        int masc = 0; int fem = 0;

        int suma1_0 = 0;
        int grupo1_0 = 0; int grupo2_0 = 0; int grupo3_0 = 0; int grupo4_0 = 0; int grupo5_0 = 0; int grupo6_0 = 0; int grupo7_0 = 0; int grupo8_0 = 0;
        int grupo9_0 = 0; int grupo10_0 = 0;
        int grupo11_0 = 0; int grupo12_0 = 0; int grupo13_0 = 0;
        int masc_0 = 0; int fem_0 = 0;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false; oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtFechaDesde.Value = "01/03/2020"; // DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                MostrarReporteGeneral();
                MostrarGrillaResultados(0);
            }
        }


        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }


        private void MostrarReporteGeneral()
        {
            gvEstadistica0.DataSource = GetDatosEstadistica(0,1);
            gvEstadistica0.DataBind();

           
            //HFTipoMuestra.Value = getValoresGrilla();
            //if (gvEstadistica.Rows.Count == 0) Response.Redirect("SinDatos.aspx?Desde=ReportePorResultado.aspx", false);
            //else
            //{

            pnlResultado.Visible = true;
            //}
        }

        private string getValoresGrilla()
        {
            string s_valores = "";

            for (int i = 0; i < gvEstadistica.Rows.Count; i++)
            {
                string s_nombre = gvEstadistica.Rows[i].Cells[0].Text.Replace(";", "");
                s_nombre = s_nombre.Replace("&#", "");
                if (s_valores == "")
                    s_valores = "name='" + s_nombre + "' value='" + gvEstadistica.Rows[i].Cells[1].Text + "'";
                else
                    s_valores += ";" + "name='" + s_nombre + "' value='" + gvEstadistica.Rows[i].Cells[1].Text + "'";
            }

            return s_valores;
        }



        protected void lnkPdf_Click(object sender, EventArgs e)
        {

        }



  

        private DataTable GetDatosEstadistica( int idcaracter, int grilla)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_EstadisticaSeguimiento]";



            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
            

           
            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.AddDays(1).ToString("yyyyMMdd");

            cmd.Parameters.Add("@criterioFecha", SqlDbType.NVarChar);
            cmd.Parameters["@criterioFecha"].Value = ddlCriterio.SelectedValue;
            /////
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", "9122");

            cmd.Parameters.Add("@idAnalisis", SqlDbType.NVarChar);
            cmd.Parameters["@idAnalisis"].Value = oItem.IdItem.ToString();
            
            cmd.Parameters.Add("@diagnostico", SqlDbType.NVarChar);
            cmd.Parameters["@diagnostico"].Value = "";


            cmd.Parameters.Add("@grupoEtareo", SqlDbType.Int);
            cmd.Parameters["@grupoEtareo"].Value = 0;

            cmd.Parameters.Add("@sexo", SqlDbType.Int);
            cmd.Parameters["@sexo"].Value =0;

            cmd.Parameters.Add("@idOrigen", SqlDbType.NVarChar);
            cmd.Parameters["@idOrigen"].Value = "";

            cmd.Parameters.Add("@idsector", SqlDbType.NVarChar);
            cmd.Parameters["@idsector"].Value = "";
            cmd.Parameters.Add("@idcaracter", SqlDbType.Int);
           
                cmd.Parameters["@idcaracter"].Value =idcaracter;
           




            cmd.Parameters.Add("@grilla", SqlDbType.Int);
            cmd.Parameters["@grilla"].Value = grilla;
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            return Ds.Tables[0];

        }

      

         
        protected void lnkExcel_Click1(object sender, EventArgs e)
        {


        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {

        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            if (Request["informe"].ToString() == "General")
                Response.Redirect("Filtro.aspx", false);
            else
                Response.Redirect("PorResultado.aspx", false);

        }

        protected void gvEstadistica_RowDataBound(object sender, GridViewRowEventArgs e)
        {


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton CmdPdf = (ImageButton)e.Row.Cells[17].Controls[1];
                    //CmdPdf.CommandArgument = ddlAnalisis.SelectedValue + "~" + e.Row.Cells[0].Text; ///Codigo1 + ";" + codigo2
                    CmdPdf.CommandName = "PDF";
                    CmdPdf.ToolTip = "Ver Pacientes";

                    ImageButton CmdExcel = (ImageButton)e.Row.Cells[18].Controls[1];
                  //  CmdExcel.CommandArgument = ddlAnalisis.SelectedValue + "~" + e.Row.Cells[0].Text; ///Codigo1 + ";" + codigo2
                    CmdExcel.CommandName = "EXCEL";
                    CmdExcel.ToolTip = "Ver Pacientes";
              
                    if (e.Row.Cells[1].Text != "&nbsp;") suma1 += int.Parse(e.Row.Cells[1].Text);
                    
                    if (e.Row.Cells[2].Text != "&nbsp;") grupo1 += int.Parse(e.Row.Cells[2].Text);                    
                    if (e.Row.Cells[3].Text != "&nbsp;") grupo2 += int.Parse(e.Row.Cells[3].Text);                    
                    if (e.Row.Cells[4].Text != "&nbsp;") grupo3 += int.Parse(e.Row.Cells[4].Text);                    
                    if (e.Row.Cells[5].Text != "&nbsp;") grupo4 += int.Parse(e.Row.Cells[5].Text);                    
                    if (e.Row.Cells[6].Text != "&nbsp;") grupo5 += int.Parse(e.Row.Cells[6].Text);                    
                    if (e.Row.Cells[7].Text != "&nbsp;") grupo6 += int.Parse(e.Row.Cells[7].Text);                    
                    if (e.Row.Cells[8].Text != "&nbsp;") grupo7 += int.Parse(e.Row.Cells[8].Text);                    
                    if (e.Row.Cells[9].Text != "&nbsp;") grupo8 += int.Parse(e.Row.Cells[9].Text);
                    if (e.Row.Cells[10].Text != "&nbsp;") grupo9 += int.Parse(e.Row.Cells[10].Text);
                    if (e.Row.Cells[11].Text != "&nbsp;") grupo10 += int.Parse(e.Row.Cells[11].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo11 += int.Parse(e.Row.Cells[12].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo12 += int.Parse(e.Row.Cells[13].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo13 += int.Parse(e.Row.Cells[14].Text);
                //if (e.Row.Cells[11].Text != "&nbsp;") grupo14 += int.Parse(e.Row.Cells[15].Text);


                if (e.Row.Cells[12].Text != "&nbsp;") masc += int.Parse(e.Row.Cells[15].Text);
                    if (e.Row.Cells[13].Text != "&nbsp;") fem += int.Parse(e.Row.Cells[16].Text);
                    //if (e.Row.Cells[14].Text != "&nbsp;") ind += int.Parse(e.Row.Cells[17].Text);
                    

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Cells[0].Text = "TOTAL CASOS";
                    e.Row.Cells[1].Text = suma1.ToString();
                    e.Row.Cells[2].Text = grupo1.ToString();
                    e.Row.Cells[3].Text = grupo2.ToString();
                    e.Row.Cells[4].Text = grupo3.ToString();
                    e.Row.Cells[5].Text = grupo4.ToString();
                    e.Row.Cells[6].Text = grupo5.ToString();
                    e.Row.Cells[7].Text = grupo6.ToString();
                    e.Row.Cells[8].Text = grupo7.ToString();
                    e.Row.Cells[9].Text = grupo8.ToString();
                    e.Row.Cells[10].Text = grupo9.ToString();
                    e.Row.Cells[11].Text = grupo10.ToString();
                e.Row.Cells[12].Text = grupo11.ToString();
                e.Row.Cells[13].Text = grupo12.ToString();
                e.Row.Cells[14].Text = grupo13.ToString();
                //e.Row.Cells[15].Text = grupo14.ToString();

                e.Row.Cells[15].Text = masc.ToString();
                    e.Row.Cells[16].Text = fem.ToString();
                    //e.Row.Cells[17].Text = ind.ToString();
                    
                }

          
        }





        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

            ExportarExcelTabla2();

        }

        private void ExportarExcelTabla2()
        {
            DataTable tabla = GetDatosEstadistica(0, 2);
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
                Response.AddHeader("Content-Disposition", "attachment;filename=COVID_RESULTADOS.xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }
        private void ExportarExcel()
        {
         DataTable tabla= GetDatosEstadistica(0, 1);
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
                Response.AddHeader("Content-Disposition", "attachment;filename=COVID_RESULTADOS.xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }

        //protected void ddlAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    gvEstadistica.DataSource = GetDatosEstadistica("GV");
        //    gvEstadistica.DataBind();
        //}

        protected void gvEstadistica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "PDF")
            //    InformePacientes(e.CommandArgument.ToString(),"PDF");
            //if (e.CommandName == "EXCEL")
            //    InformePacientes(e.CommandArgument.ToString(),"EXCEL");

        }

        //private void InformePacientes(string p, string reporte)
        //{
        //    Utility oUtil = new Utility();
        //    string[] arr = p.ToString().Split(("~").ToCharArray());


        //    string m_analisis = arr[0].ToString();
        //    string m_resultado =oUtil.RemoverSignosAcentos( arr[1].ToString());

        //    if (reporte == "PDF")
        //    {
        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

        //    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
        //    encabezado1.Value = oCon.EncabezadoLinea1;

        //    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
        //    encabezado2.Value = oCon.EncabezadoLinea2;

        //    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
        //    encabezado3.Value = oCon.EncabezadoLinea3;
        //    ////////////////////////////
      

        //    ParameterDiscreteValue titulo = new ParameterDiscreteValue();
        //    titulo.Value = "INFORME DE PACIENTES ";

        //  //  if (rdbPaciente.SelectedValue == "1") titulo.Value = "INFORME DE PACIENTES EMBARAZADAS";
            
        //    if (ddlGrupoEtareo.SelectedValue != "0") titulo.Value += "  Grupo Etareo: " + ddlGrupoEtareo.SelectedItem.Text;
        //    if (ddlSexo.SelectedValue != "0") titulo.Value += " - Sexo: " + ddlSexo.SelectedItem.Text;

        //    ParameterDiscreteValue fechaDesde = new ParameterDiscreteValue();
        //    fechaDesde.Value = txtFechaDesde.Value;

        //    ParameterDiscreteValue fechaHasta = new ParameterDiscreteValue();
        //    fechaHasta.Value = txtFechaHasta.Value;


        //    oCr.Report.FileName = "Pacientes.rpt";
        //    oCr.ReportDocument.SetDataSource(GetDataPacientes(m_analisis, m_resultado, "PDF"));
        //    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
        //    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
        //    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
        //    oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(titulo);
        //    oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(fechaDesde);
        //    oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(fechaHasta);
        //    oCr.DataBind();

        //        oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Pacientes");

             
        //    }
        //    if (reporte == "EXCEL")
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        StringWriter sw = new StringWriter(sb);
        //        HtmlTextWriter htw = new HtmlTextWriter(sw);

        //        Page page = new Page();
        //        HtmlForm form = new HtmlForm();
        //        GridView dg = new GridView();
        //        dg.EnableViewState = false;
        //        dg.DataSource = GetDataPacientes(m_analisis, m_resultado, "EXCEL");
        //        dg.DataBind();
        //        // Deshabilitar la validación de eventos, sólo asp.net 2
        //        page.EnableEventValidation = false;

        //        // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
        //        page.DesignerInitialize();
        //        page.Controls.Add(form);
        //        form.Controls.Add(dg);
        //        page.RenderControl(htw);

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.AddHeader("Content-Disposition", "attachment;filename=DetallePacientes.xls");
        //        Response.Charset = "UTF-8";
        //        Response.ContentEncoding = Encoding.Default;
        //        Response.Write(sb.ToString());
        //        Response.End();
        //    }
        //}

        //private DataTable GetDataPacientes(string m_analisis, string m_resultado, string m_tipo)
        //{
        //    string m_strCondicion="";
        //    string m_codigopaciente = " '' as codigoPaciente";

        //    Item oItem = new Item();
        //    oItem = (Item)oItem.Get(typeof(Item), int.Parse( ddlAnalisis.SelectedValue));
        //    if (oItem != null)
        //    {
        //        if (oItem.CodificaHiv) m_codigopaciente = " [dbo].[BuscarPaciente] (Pa.idpaciente, 1) as codigoPaciente ";
        //    }

        //    string listadiag = getListaDiagnostico();
        //    if (listadiag != "")
        //        m_strCondicion += " and PD.iddiagnostico in ( " + listadiag +")";


        //    m_strCondicion = " and P.idPaciente>-1";
        //    if (ddlGrupoEtareo.SelectedValue != "0")
        //    {


        //        if (ddlGrupoEtareo.SelectedValue == "1") m_strCondicion += " and (P.unidadEdad=1 and P.edad<6) or (P.unidadedad=2)";
        //        if (ddlGrupoEtareo.SelectedValue == "2") m_strCondicion += " and P.unidadEdad=1 and P.edad>=6 and P.edad<12 ";
        //        if (ddlGrupoEtareo.SelectedValue == "3") m_strCondicion += "  and P.edad >= 1 and P.edad <2 AND P.unidadedad = 0   ";
        //        if (ddlGrupoEtareo.SelectedValue == "4") m_strCondicion += " and P.edad >= 2 AND P.edad <= 4 AND P.unidadedad = 0  ";
        //        if (ddlGrupoEtareo.SelectedValue == "5") m_strCondicion += " and P.edad >= 5 AND P.edad <= 9 AND P.unidadedad = 0   ";
        //        if (ddlGrupoEtareo.SelectedValue == "6") m_strCondicion += " and P.edad >= 10 AND P.edad <= 14 AND P.unidadedad = 0 ";
        //        if (ddlGrupoEtareo.SelectedValue == "7") m_strCondicion += " and P.edad >= 15 AND P.edad <= 19 AND P.unidadedad = 0  ";
        //        if (ddlGrupoEtareo.SelectedValue == "8") m_strCondicion += " and P.edad >= 20 AND P.edad <= 24 AND P.unidadedad = 0  ";
        //        if (ddlGrupoEtareo.SelectedValue == "9") m_strCondicion += "  and P.edad >= 25 AND P.edad <= 34 AND P.unidadedad = 0   ";
        //        if (ddlGrupoEtareo.SelectedValue == "10") m_strCondicion += " and P.edad >= 35 AND  P.edad <= 44 AND P.unidadedad = 0  ";
        //        if (ddlGrupoEtareo.SelectedValue == "11") m_strCondicion += " and P.edad >= 45 AND P.edad <= 64 AND P.unidadedad = 0 ";
        //        //if (ddlGrupoEtareo.SelectedValue == "12") m_strCondicion += " and P.edad >= 55 AND P.edad <= 64 AND P.unidadedad = 0  ";
        //        if (ddlGrupoEtareo.SelectedValue == "12") m_strCondicion += " and P.edad >= 65 AND P.edad <= 74 AND P.unidadedad = 0   ";
        //        if (ddlGrupoEtareo.SelectedValue == "13") m_strCondicion += " and P.edad >= 75 AND P.unidadedad = 0 ";
        //    }

        //    if (ddlSexo.SelectedValue != "0")
        //    {
        //        if (ddlSexo.SelectedValue == "1")
        //            m_strCondicion += " and P.sexo='F'";
        //        if (ddlSexo.SelectedValue == "2")
        //            m_strCondicion += " and P.sexo='M' ";
        //    }

        //    m_strCondicion += " and P.idOrigen in (" + getListaOrigen() + ")";
        //    m_strCondicion += " and P.idSector in (" + getListaSector() + ")";

        //    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
        //    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

        //    string tablaDiag = "sys_cie10  as C10 on C10.id = Pd.iddiagnostico ";
        //    if (oC.NomencladorDiagnostico==1)
        //        tablaDiag = "lab_diagnostico as C10 on C10.idDiagnostico = Pd.iddiagnostico";


        //    string m_strSQL = " SELECT I.nombre AS ANALISIS, DP.resultadoCar AS RESULTADO, Pa.numeroDocumento, Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento, " +
        //                    " 103) AS FECHANACIMIENTO, Pa.referencia AS domicilio, CONVERT(varchar(10), P.fecha, 103) AS fecha,convert(varchar,P.edad) + case  P.unidadedad when 0 then 'A' when 1 then 'M' when 2 then 'D' end as edad, dbo.NumeroProtocolo(P.idProtocolo) as numero, " 
        //                    + m_codigopaciente + ", O.nombre as Origen, isnull(c10.nombre,'') as diagnostico, SS.nombre as Sector" +
        //                    " FROM LAB_DetalleProtocolo AS DP " +
        //                    " INNER JOIN LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo " +
        //                    " INNER JOIN LAB_Item AS I ON DP.idSUBItem = I.idItem " +
        //                    " INNER JOIN Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente" +
        //                    " left JOIN Lab_protocoloDiagnostico AS PD ON PD.idProtocolo = P.idProtocolo " +
        //                    " left join "+ tablaDiag  +
        //                    " INNER JOIN LAB_Origen as O on O.idOrigen= P.idOrigen " +
        //                    " inner join lab_sectorservicio as SS on SS.idsectorservicio= P.idsector "+
        //                    " WHERE (I.idItem=" + m_analisis + ") AND (DP.resultadoCar = '" + m_resultado + "') AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "') and " +
        //                    " ( DP.conresultado=1) " + m_strCondicion +
        //                    " order by P.fecha  ";


        //    if (m_tipo == "PDF")
        //        m_strSQL = " SELECT distinct I.nombre AS ANALISIS, DP.resultadoCar AS RESULTADO, Pa.numeroDocumento, Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento, " +
        //                    " 103) AS FECHANACIMIENTO, Pa.referencia AS domicilio, CONVERT(varchar(10), P.fecha, 103) AS fecha,convert(varchar,P.edad) + case  P.unidadedad when 0 then 'A' when 1 then 'M' when 2 then 'D' end as edad, dbo.NumeroProtocolo(P.idProtocolo) as numero, "
        //                    + m_codigopaciente + ", O.nombre as Origen , SS.nombre as Sector" +
        //                    " FROM LAB_DetalleProtocolo AS DP " +
        //                    " INNER JOIN LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo " +
        //                    " INNER JOIN LAB_Item AS I ON DP.idSUBItem = I.idItem " +
        //                    " INNER JOIN Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente" +
        //                    " left JOIN Lab_protocoloDiagnostico AS PD ON PD.idProtocolo = P.idProtocolo " +
        //                    " left join " + tablaDiag +
        //                    " INNER JOIN LAB_Origen as O on O.idOrigen= P.idOrigen " +
        //                    " inner join lab_sectorservicio as SS on SS.idsectorservicio= P.idsector " +
        //                    " WHERE (I.idItem=" + m_analisis + ") AND (DP.resultadoCar = '" + m_resultado + "') AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "') and " +
        //                    " ( DP.conresultado=1) " + m_strCondicion +  " order by numero  ";

        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);


        //    DataTable data = Ds.Tables[0];
        //    return data;
        //}

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            MostrarReporteGeneral();
        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            //MostrarPdf();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MostrarReporteGeneral();
            MostrarGrillaResultados(0);
        }

        protected void gvEstadistica0_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               Button CmdPdf = (Button)e.Row.Cells[17].Controls[1];
                CmdPdf.CommandArgument = this.gvEstadistica0.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdPdf.CommandName = "Filtrar";
                CmdPdf.ToolTip = "Ver Resultados";

                if (e.Row.Cells[1].Text != "&nbsp;") suma1_0 += int.Parse(e.Row.Cells[1].Text);

                if (e.Row.Cells[2].Text != "&nbsp;") grupo1_0 += int.Parse(e.Row.Cells[2].Text);
                if (e.Row.Cells[3].Text != "&nbsp;") grupo2_0 += int.Parse(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;") grupo3_0 += int.Parse(e.Row.Cells[4].Text);
                if (e.Row.Cells[5].Text != "&nbsp;") grupo4_0 += int.Parse(e.Row.Cells[5].Text);
                if (e.Row.Cells[6].Text != "&nbsp;") grupo5_0 += int.Parse(e.Row.Cells[6].Text);
                if (e.Row.Cells[7].Text != "&nbsp;") grupo6_0 += int.Parse(e.Row.Cells[7].Text);
                if (e.Row.Cells[8].Text != "&nbsp;") grupo7_0 += int.Parse(e.Row.Cells[8].Text);
                if (e.Row.Cells[9].Text != "&nbsp;") grupo8_0 += int.Parse(e.Row.Cells[9].Text);
                if (e.Row.Cells[10].Text != "&nbsp;") grupo9_0 += int.Parse(e.Row.Cells[10].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo10_0 += int.Parse(e.Row.Cells[11].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo11_0 += int.Parse(e.Row.Cells[12].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo12_0 += int.Parse(e.Row.Cells[13].Text);
                if (e.Row.Cells[11].Text != "&nbsp;") grupo13_0 += int.Parse(e.Row.Cells[14].Text);
                //if (e.Row.Cells[11].Text != "&nbsp;") grupo14 += int.Parse(e.Row.Cells[15].Text);


                if (e.Row.Cells[12].Text != "&nbsp;") masc_0 += int.Parse(e.Row.Cells[15].Text);
                if (e.Row.Cells[13].Text != "&nbsp;") fem_0 += int.Parse(e.Row.Cells[16].Text);
                


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma1_0.ToString();
                e.Row.Cells[2].Text = grupo1_0.ToString();
                e.Row.Cells[3].Text = grupo2_0.ToString();
                e.Row.Cells[4].Text = grupo3_0.ToString();
                e.Row.Cells[5].Text = grupo4_0.ToString();
                e.Row.Cells[6].Text = grupo5_0.ToString();
                e.Row.Cells[7].Text = grupo6_0.ToString();
                e.Row.Cells[8].Text = grupo7_0.ToString();
                e.Row.Cells[9].Text = grupo8_0.ToString();
                e.Row.Cells[10].Text = grupo9_0.ToString();
                e.Row.Cells[11].Text = grupo10_0.ToString();
                e.Row.Cells[12].Text = grupo11_0.ToString();
                e.Row.Cells[13].Text = grupo12_0.ToString();
                e.Row.Cells[14].Text = grupo13_0.ToString();
                //e.Row.Cells[15].Text = grupo14.ToString();

                e.Row.Cells[15].Text = masc_0.ToString();
                e.Row.Cells[16].Text = fem_0.ToString();
                //e.Row.Cells[17].Text = ind.ToString();

            }

        
    }

        protected void gvEstadistica0_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Filtrar")
            {
                MostrarGrillaResultados(int.Parse(e.CommandArgument.ToString()));
                    for (int i = 0; i < this.gvEstadistica0.Rows.Count; i++)
                {

                    if (this.gvEstadistica0.DataKeys[i].Value.ToString() == e.CommandArgument.ToString())


                        this.gvEstadistica0.Rows[i].BackColor = System.Drawing.Color.LightGray;
                    else
                        this.gvEstadistica0.Rows[i].BackColor = System.Drawing.Color.White;

                    
                }
            }


         
            //if (e.CommandName == "EXCEL")
            //    InformePacientes(e.CommandArgument.ToString(),"EXCEL");

        }

        private void MostrarGrillaResultados(int idcaracter)
        {
            gvEstadistica.DataSource = GetDatosEstadistica( idcaracter, 2);
            gvEstadistica.DataBind();
        }

        protected void imgExcel0_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcelTabla2();

        }

        //private void MostrarPdf()
        //{



        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

        //    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
        //    encabezado1.Value = oCon.EncabezadoLinea1;

        //    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
        //    encabezado2.Value = oCon.EncabezadoLinea2;

        //    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
        //    encabezado3.Value = oCon.EncabezadoLinea3;
        //    ////////////////////////////


        //    ParameterDiscreteValue titulo = new ParameterDiscreteValue();
        //    titulo.Value = "INFORME DE RESULTADOS ";

        //   // if (rdbPaciente.SelectedValue == "1") titulo.Value = "INFORME DE RESULTADOS (EMBARAZADAS)";
        //    if (ddlGrupoEtareo.SelectedValue != "0") titulo.Value += "  Grupo Etareo: " + ddlGrupoEtareo.SelectedItem.Text;
        //    if (ddlSexo.SelectedValue != "0") titulo.Value += " - Sexo: " + ddlSexo.SelectedItem.Text;

        //    ParameterDiscreteValue fechaDesde = new ParameterDiscreteValue();
        //    fechaDesde.Value = txtFechaDesde.Value;

        //    ParameterDiscreteValue fechaHasta = new ParameterDiscreteValue();
        //    fechaHasta.Value = txtFechaHasta.Value;


        //    oCr.Report.FileName = "ResultadoPredefinido2.rpt";
        //    oCr.ReportDocument.SetDataSource(GetDatosEstadistica("GV"));
        //    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
        //    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
        //    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
        //    oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(titulo);
        //    oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(fechaDesde);
        //    oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(fechaHasta);
        //    oCr.DataBind();

        //    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "reporteresultados");


        //}

        //protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CargarAnalisis();
        //}

        //private void MarcarSectoresSeleccionados(bool p)
        //{
        //    for (int i = 0; i < lstDiag.Items.Count; i++)
        //    {
        //        lstDiag.Items[i].Selected = p;
        //    }



        //}
        //protected void lnkMarcarSectores_Click(object sender, EventArgs e)
        //{
        //    MarcarSectoresSeleccionados(true);

        //}

        //protected void lnkDesmarcarSectores_Click(object sender, EventArgs e)
        //{
        //    MarcarSectoresSeleccionados(false);

        //}

        //protected void lnkMarcarSector_Click(object sender, EventArgs e)
        //{
        //    MarcarSectores(true);

        //}

        //private void MarcarSectores (bool p)
        //{
        //    for (int i = 0; i < lstSector.Items.Count; i++)
        //    {
        //        lstSector.Items[i].Selected = p;
        //    }



        //}

        //protected void lnkDesmarcarSector_Click(object sender, EventArgs e)
        //{
        //    MarcarSectores(false);
        //}

        //protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    CargarAreas();
        //}

        //private void CargarAreas()
        //{
        //    Utility oUtil = new Utility();
        //    ///Carga de combos de tipos de servicios
        //    /// 
        //    string               m_ssql = "SELECT  idArea, nombre from LAB_Area where baja=0 and idtipoServicio= "+ddlServicio.SelectedValue +" ORDER BY nombre";

        //    oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
        //    ddlArea.Items.Insert(0, new ListItem("--Seleccione--", "0"));

        //}
    }
}
