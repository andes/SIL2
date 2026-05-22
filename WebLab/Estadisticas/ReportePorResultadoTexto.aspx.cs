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
    public partial class ReportePorResultadoTexto : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();


      
        //int suma1 = 0;
        //int grupo1 = 0; int grupo2 = 0; int grupo3 = 0; int grupo4=0; int grupo5=0; int grupo6=0; int grupo7=0; int grupo8=0; int grupo9=0; int grupo10=0;
        //int grupo11 = 0; int grupo12 = 0; int grupo13 = 0; int grupo14 = 0;
        //int masc = 0; int fem = 0; int ind = 0;
        
        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Session["idUsuario"] == null)
                Response.Redirect("logout.aspx", false);
            else
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oUser != null)
                    oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
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
                    CargarListas();
                    txtFechaDesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                }
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
        private void CargarListas()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "SELECT  idTipoServicio, nombre from LAB_TipoServicio where baja=0 and idtipoServicio<5 ORDER BY idTiposervicio";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            //ddlServicio.Items.Insert(0, new ListItem("--Seleccione--", "0"));
         //   ddlServicio.SelectedValue = "1";

            if (oUser.IdEfector.IdEfector == 227) // puede elegir el efector que quiere ver o todos
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E " +
                     " INNER JOIN lab_Configuracion C on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                ddlEfector.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }





            /////Carga de Diagnostico      
            ////if (oC.NomencladorDiagnostico==0)
            //    m_ssql = @"select distinct id, nombre from  sys_cie10 as S inner join lab_protocolodiagnostico as P on P.idDiagnostico = S.id order by S.Nombre";
            ////else
            ////    m_ssql = @"select distinct S.idDiagnostico as id, nombre from  lab_Diagnostico as S inner join lab_protocolodiagnostico as P on P.idDiagnostico = S.idDiagnostico order by S.Nombre";
            //oUtil.CargarListBox(lstDiag, m_ssql, "id", "nombre");
            //for (int i = 0; i < lstDiag.Items.Count; i++)
            //{
            //    lstDiag.Items[i].Selected = true;
            //}

            /////Carga de Sectores          
            //m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre  FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            //oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");
            //for (int i = 0; i < lstSector.Items.Count; i++)
            //{
            //    lstSector.Items[i].Selected = true;
            //}
            CargarAreas();

     //      CargarAnalisis();

            //m_ssql = "SELECT     idOrigen, nombre  AS nombre FROM         LAB_Origen  ORDER BY nombre";
            //oUtil.CargarCheckBox(ChckOrigen, m_ssql, "idOrigen", "nombre");
            //for (int i = 0; i < ChckOrigen.Items.Count; i++)
            //    ChckOrigen.Items[i].Selected = true;
            m_ssql = null;
            oUtil = null;

        }
        private void CargarAnalisis()
        {
            Utility oUtil = new Utility();
            string m_condicion = " and 1=1 "; string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            if (ddlArea.SelectedValue != "")
            {
                //if (ddlArea.SelectedValue != "0")
                m_condicion = " and idArea=" + ddlArea.SelectedValue;

                ///Carga de combos de tipos de servicios
                string m_ssql = @"SELECT     idItem, nombre + ' (' + codigo + ')' AS nombre  
              FROM         LAB_Item  a
              WHERE   disponible=1  AND (baja = 0)               " + m_condicion + //AND (tipo = 'P') 
                @" ORDER BY nombre";

                oUtil.CargarCombo(ddlAnalisis, m_ssql, "idItem", "nombre", connReady);
                ddlAnalisis.Items.Insert(0, new ListItem("--Seleccione--", "0"));


            m_ssql = null;
            }
            oUtil = null;
        }

        private void MostrarReporteGeneral()
        {
            DataTable dtCasos = GetDatosEstadistica("GV");
            gvEstadistica.DataSource = dtCasos;
            gvEstadistica.DataBind();
            

            lblAnalisis.Text = dtCasos.Rows.Count.ToString() + " registros encontrados";
            lblAnalisis.Visible = true;
        //    HFTipoMuestra.Value = getValoresGrilla();
            //if (gvEstadistica.Rows.Count == 0) Response.Redirect("SinDatos.aspx?Desde=ReportePorResultado.aspx", false);
            //else
            //{
            //    lblAnalisis.Text = ddlAnalisis.SelectedItem.Text;
            //    pnlResultado.Visible = true;
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



        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                //if (getListaOrigen() == "") { args.IsValid = false; cvFechas.ErrorMessage = "Seleccione un origen"; }
                //else { 


                if (txtFechaDesde.Value == "")
                    args.IsValid = false;
                else
                {
                    args.IsValid = true;
                    DateTime f1 = DateTime.Parse(txtFechaDesde.Value);
                    if (txtFechaHasta.Value == "")
                        args.IsValid = false;
                    else
                    {
                        DateTime f2 = DateTime.Parse(txtFechaHasta.Value);
                        args.IsValid = true;
                    }
                }
            //}
            }
            catch (Exception ex)
            {
                string exception = "";
                //while (ex != null)
                //{
                    exception = ex.Message + "<br>";

                //} 
            args.IsValid = false;
            }

        }


        private DataTable GetDatosEstadistica(string s_tipo)
        {
            DataSet Ds = new DataSet();
            //     SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_EstadisticaPorResultadosTexto]";



            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            /////

            cmd.Parameters.Add("@idArea", SqlDbType.NVarChar);
            cmd.Parameters["@idArea"].Value = int.Parse(ddlArea.SelectedValue);

            cmd.Parameters.Add("@idAnalisis", SqlDbType.NVarChar);
            cmd.Parameters["@idAnalisis"].Value = int.Parse(ddlAnalisis.SelectedValue);

            //cmd.Parameters.Add("@diagnostico", SqlDbType.NVarChar);
            //cmd.Parameters["@diagnostico"].Value = getListaDiagnostico();


            //cmd.Parameters.Add("@grupoEtareo", SqlDbType.Int);
            //cmd.Parameters["@grupoEtareo"].Value = int.Parse(ddlGrupoEtareo.SelectedValue);

            //cmd.Parameters.Add("@sexo", SqlDbType.Int);
            //cmd.Parameters["@sexo"].Value = int.Parse(ddlSexo.SelectedValue);

            //cmd.Parameters.Add("@idOrigen", SqlDbType.NVarChar);
            //cmd.Parameters["@idOrigen"].Value = getListaOrigen();

            //cmd.Parameters.Add("@idsector", SqlDbType.NVarChar);
            //cmd.Parameters["@idsector"].Value = getListaSector();


            cmd.Parameters.Add("@idEfector", SqlDbType.Int);
            cmd.Parameters["@idEfector"].Value = int.Parse(ddlEfector.SelectedValue);

            cmd.Parameters.Add("@texto", SqlDbType.VarChar);
            cmd.Parameters["@texto"].Value = texto.Text.Trim();

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            return Ds.Tables[0];

        }

        //private string getListaSector()
        //{
        //    string lista = "";
        //    for (int i = 0; i < this.lstSector.Items.Count; i++)
        //    {
        //        if (lstSector.Items[i].Selected)
        //        {
        //            if (lista == "")
        //                lista = lstSector.Items[i].Value;
        //            else
        //                lista += "," + lstSector.Items[i].Value;
        //        }

        //    }
        //    return lista;
        //}

        //protected void lnkMarcar_Click(object sender, EventArgs e)
        //{
        //    MarcarSeleccionados(true);
        //}
        //private void MarcarSeleccionados(bool p)
        //{
        //    for (int i = 0; i < ChckOrigen.Items.Count; i++)
        //        ChckOrigen.Items[i].Selected = p;

        //}

        //protected void lnkDesmarcar_Click(object sender, EventArgs e)
        //{
        //    MarcarSeleccionados(false);
        //}

        //private string getListaDiagnostico()
        //{

        //    string lista = "";string noselec = "";
        //    for (int i = 0; i < lstDiag.Items.Count; i++)
        //    {
        //        if (lstDiag.Items[i].Selected)
        //        {
        //            if (lista == "")
        //                lista = lstDiag.Items[i].Value;
        //            else
        //                lista += "," + lstDiag.Items[i].Value;
        //        }
        //        else
        //            noselec = "-1";

        //    }
        //    if (noselec == "")
        //        return noselec;
        //    else
        //    return lista;
        //}
        //private string getListaOrigen()
        //{

        //    string lista = "";
        //    for (int i = 0; i < this.ChckOrigen.Items.Count; i++)
        //    {
        //        if (ChckOrigen.Items[i].Selected)
        //        {
        //            if (lista == "")
        //                lista = ChckOrigen.Items[i].Value;
        //            else
        //                lista += "," + ChckOrigen.Items[i].Value;
        //        }

        //    }
        //    return lista;
        //}
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


                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    ImageButton CmdPdf = (ImageButton)e.Row.Cells[18].Controls[1];
                //    CmdPdf.CommandArgument = ddlAnalisis.SelectedValue + "~" + e.Row.Cells[0].Text; ///Codigo1 + ";" + codigo2
                //    CmdPdf.CommandName = "PDF";
                //    CmdPdf.ToolTip = "Ver Pacientes";

                //    ImageButton CmdExcel = (ImageButton)e.Row.Cells[19].Controls[1];
                //    CmdExcel.CommandArgument = ddlAnalisis.SelectedValue + "~" + e.Row.Cells[0].Text; ///Codigo1 + ";" + codigo2
                //    CmdExcel.CommandName = "EXCEL";
                //    CmdExcel.ToolTip = "Ver Pacientes";
              
                //    if (e.Row.Cells[1].Text != "&nbsp;") suma1 += int.Parse(e.Row.Cells[1].Text);
                    
                //    if (e.Row.Cells[2].Text != "&nbsp;") grupo1 += int.Parse(e.Row.Cells[2].Text);                    
                //    if (e.Row.Cells[3].Text != "&nbsp;") grupo2 += int.Parse(e.Row.Cells[3].Text);                    
                //    if (e.Row.Cells[4].Text != "&nbsp;") grupo3 += int.Parse(e.Row.Cells[4].Text);                    
                //    if (e.Row.Cells[5].Text != "&nbsp;") grupo4 += int.Parse(e.Row.Cells[5].Text);                    
                //    if (e.Row.Cells[6].Text != "&nbsp;") grupo5 += int.Parse(e.Row.Cells[6].Text);                    
                //    if (e.Row.Cells[7].Text != "&nbsp;") grupo6 += int.Parse(e.Row.Cells[7].Text);                    
                //    if (e.Row.Cells[8].Text != "&nbsp;") grupo7 += int.Parse(e.Row.Cells[8].Text);                    
                //    if (e.Row.Cells[9].Text != "&nbsp;") grupo8 += int.Parse(e.Row.Cells[9].Text);
                //    if (e.Row.Cells[10].Text != "&nbsp;") grupo9 += int.Parse(e.Row.Cells[10].Text);
                //    if (e.Row.Cells[11].Text != "&nbsp;") grupo10 += int.Parse(e.Row.Cells[11].Text);
                //if (e.Row.Cells[11].Text != "&nbsp;") grupo11 += int.Parse(e.Row.Cells[12].Text);
                //if (e.Row.Cells[11].Text != "&nbsp;") grupo12 += int.Parse(e.Row.Cells[13].Text);
                //if (e.Row.Cells[11].Text != "&nbsp;") grupo13 += int.Parse(e.Row.Cells[14].Text);
                ////if (e.Row.Cells[11].Text != "&nbsp;") grupo14 += int.Parse(e.Row.Cells[15].Text);


                //if (e.Row.Cells[12].Text != "&nbsp;") masc += int.Parse(e.Row.Cells[15].Text);
                //    if (e.Row.Cells[13].Text != "&nbsp;") fem += int.Parse(e.Row.Cells[16].Text);
                //    if (e.Row.Cells[14].Text != "&nbsp;") ind += int.Parse(e.Row.Cells[17].Text);
                    

                //}
                //if (e.Row.RowType == DataControlRowType.Footer)
                //{
                //    e.Row.Cells[0].Text = "TOTAL CASOS";
                //    e.Row.Cells[1].Text = suma1.ToString();
                //    e.Row.Cells[2].Text = grupo1.ToString();
                //    e.Row.Cells[3].Text = grupo2.ToString();
                //    e.Row.Cells[4].Text = grupo3.ToString();
                //    e.Row.Cells[5].Text = grupo4.ToString();
                //    e.Row.Cells[6].Text = grupo5.ToString();
                //    e.Row.Cells[7].Text = grupo6.ToString();
                //    e.Row.Cells[8].Text = grupo7.ToString();
                //    e.Row.Cells[9].Text = grupo8.ToString();
                //    e.Row.Cells[10].Text = grupo9.ToString();
                //    e.Row.Cells[11].Text = grupo10.ToString();
                //e.Row.Cells[12].Text = grupo11.ToString();
                //e.Row.Cells[13].Text = grupo12.ToString();
                //e.Row.Cells[14].Text = grupo13.ToString();
                ////e.Row.Cells[15].Text = grupo14.ToString();

                //e.Row.Cells[15].Text = masc.ToString();
                //    e.Row.Cells[16].Text = fem.ToString();
                //    e.Row.Cells[17].Text = ind.ToString();
                    
                //}

          
        }





        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

            ExportarExcel();

        }

        private void ExportarExcel()
        {
         DataTable tabla=   GetDatosEstadistica("GV");
            if (tabla.Rows.Count > 0)
            {
                Utility.ExportDataTableToXlsx(tabla, "Estadistica");
                //StringBuilder sb = new StringBuilder();
                //StringWriter sw = new StringWriter(sb);
                //HtmlTextWriter htw = new HtmlTextWriter(sw);
                //Page pagina = new Page();
                //HtmlForm form = new HtmlForm();
                //GridView dg = new GridView();
                //dg.EnableViewState = false;
                //dg.DataSource = tabla;
                //dg.DataBind();
                //pagina.EnableEventValidation = false;
                //pagina.DesignerInitialize();
                //pagina.Controls.Add(form);
                //form.Controls.Add(dg);
                //pagina.RenderControl(htw);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=Estadistica.xls");
                //Response.Charset = "UTF-8";
                //Response.ContentEncoding = Encoding.Default;
                //Response.Write(sb.ToString());
                //Response.End();
            }
        }

        protected void ddlAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvEstadistica.DataSource = GetDatosEstadistica("GV");
            gvEstadistica.DataBind();
        }

        protected void gvEstadistica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "PDF")
                InformePacientes(e.CommandArgument.ToString(),"PDF");
            if (e.CommandName == "EXCEL")
                InformePacientes(e.CommandArgument.ToString(),"EXCEL");

        }

        private void InformePacientes(string p, string reporte)
        {
          //  Utility oUtil = new Utility();
          //  string[] arr = p.ToString().Split(("~").ToCharArray());


          //  string m_analisis = arr[0].ToString();
          //  string m_resultado =oUtil.RemoverSignosAcentos( arr[1].ToString());

          //  if (reporte == "PDF")
          //  {
          //      ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();

          //      ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();

          //      ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
          //      if (ddlEfector.SelectedValue == "0")
          //      {



          //          //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
          //          encabezado1.Value = "Subsecretaria de Salud";

          //          //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
          //          encabezado2.Value = "Laboratorios";

          //          //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
          //          encabezado3.Value = "";
          //      }
          //      else
          //      {
          //          Efector oEfector = new Efector();
          //          oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));

          //          Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oEfector);

          //          //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
          //          encabezado1.Value = oCon.EncabezadoLinea1;

          //          //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
          //          encabezado2.Value = oCon.EncabezadoLinea2;

          //          //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
          //          encabezado3.Value = oCon.EncabezadoLinea3;


          //      }
          //      ////////////////////////////


          //      ParameterDiscreteValue titulo = new ParameterDiscreteValue();
          //  titulo.Value = "INFORME DE PACIENTES ";

          ////  if (rdbPaciente.SelectedValue == "1") titulo.Value = "INFORME DE PACIENTES EMBARAZADAS";
            
          //  //if (ddlGrupoEtareo.SelectedValue != "0") titulo.Value += "  Grupo Etareo: " + ddlGrupoEtareo.SelectedItem.Text;
          //  //if (ddlSexo.SelectedValue != "0") titulo.Value += " - Sexo: " + ddlSexo.SelectedItem.Text;

          //  ParameterDiscreteValue fechaDesde = new ParameterDiscreteValue();
          //  fechaDesde.Value = txtFechaDesde.Value;

          //  ParameterDiscreteValue fechaHasta = new ParameterDiscreteValue();
          //  fechaHasta.Value = txtFechaHasta.Value;


          //  oCr.Report.FileName = "Pacientes.rpt";
          //  oCr.ReportDocument.SetDataSource(GetDataPacientes(m_analisis, m_resultado, int.Parse(ddlEfector.SelectedValue), "PDF"));
          //  oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
          //  oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
          //  oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
          //  oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(titulo);
          //  oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(fechaDesde);
          //  oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(fechaHasta);
          //  oCr.DataBind();
                
          //      string nombrePDF = oUtil.CompletarNombrePDF("Pacientes");
          //      oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

              
             
          //  }
          //  if (reporte == "EXCEL")
          //  {
          //      StringBuilder sb = new StringBuilder();
          //      StringWriter sw = new StringWriter(sb);
          //      HtmlTextWriter htw = new HtmlTextWriter(sw);

          //      Page page = new Page();
          //      HtmlForm form = new HtmlForm();
          //      GridView dg = new GridView();
          //      dg.EnableViewState = false;
          //      dg.DataSource = GetDataPacientes(m_analisis, m_resultado, int.Parse(ddlEfector.SelectedValue), "EXCEL");
          //      dg.DataBind();
          //      // Deshabilitar la validación de eventos, sólo asp.net 2
          //      page.EnableEventValidation = false;

          //      // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
          //      page.DesignerInitialize();
          //      page.Controls.Add(form);
          //      form.Controls.Add(dg);
          //      page.RenderControl(htw);

          //      Response.Clear();
          //      Response.Buffer = true;
          //      Response.ContentType = "application/vnd.ms-excel";
          //      Response.AddHeader("Content-Disposition", "attachment;filename=DetallePacientes.xls");
          //      Response.Charset = "UTF-8";
          //      Response.ContentEncoding = Encoding.Default;
          //      Response.Write(sb.ToString());
          //      Response.End();
          //  }
        }

//        private DataTable GetDataPacientes(string m_analisis, string m_resultado, int i_idEfector, string m_tipo)
//        {
//            string m_strCondicion="";
//            string m_codigopaciente = " '' as codigoPaciente";

            
//            Item oItem = new Item();
//            oItem = (Item)oItem.Get(typeof(Item), int.Parse( ddlAnalisis.SelectedValue));
//            if (oItem != null)
//            {
//                if (oItem.CodificaHiv) m_codigopaciente = " [dbo].[BuscarPaciente] (Pa.idpaciente, 1) as codigoPaciente ";
//            }

//            string listadiag = getListaDiagnostico();
//            if (listadiag != "")
//                m_strCondicion += " and PD.iddiagnostico in ( " + listadiag +")";


//            m_strCondicion = " and P.idPaciente>-1";
//            if (i_idEfector > 0)
//                { m_strCondicion = " and P.idEfector="+ i_idEfector.ToString(); }

//            if (ddlGrupoEtareo.SelectedValue != "0")
//            {


//                if (ddlGrupoEtareo.SelectedValue == "1") m_strCondicion += " and (P.unidadEdad=1 and P.edad<6) or (P.unidadedad=2)";
//                if (ddlGrupoEtareo.SelectedValue == "2") m_strCondicion += " and P.unidadEdad=1 and P.edad>=6 and P.edad<12 ";
//                if (ddlGrupoEtareo.SelectedValue == "3") m_strCondicion += "  and P.edad >= 1 and P.edad <2 AND P.unidadedad = 0   ";
//                if (ddlGrupoEtareo.SelectedValue == "4") m_strCondicion += " and P.edad >= 2 AND P.edad <= 4 AND P.unidadedad = 0  ";
//                if (ddlGrupoEtareo.SelectedValue == "5") m_strCondicion += " and P.edad >= 5 AND P.edad <= 9 AND P.unidadedad = 0   ";
//                if (ddlGrupoEtareo.SelectedValue == "6") m_strCondicion += " and P.edad >= 10 AND P.edad <= 14 AND P.unidadedad = 0 ";
//                if (ddlGrupoEtareo.SelectedValue == "7") m_strCondicion += " and P.edad >= 15 AND P.edad <= 19 AND P.unidadedad = 0  ";
//                if (ddlGrupoEtareo.SelectedValue == "8") m_strCondicion += " and P.edad >= 20 AND P.edad <= 24 AND P.unidadedad = 0  ";
//                if (ddlGrupoEtareo.SelectedValue == "9") m_strCondicion += "  and P.edad >= 25 AND P.edad <= 34 AND P.unidadedad = 0   ";
//                if (ddlGrupoEtareo.SelectedValue == "10") m_strCondicion += " and P.edad >= 35 AND  P.edad <= 44 AND P.unidadedad = 0  ";
//                if (ddlGrupoEtareo.SelectedValue == "11") m_strCondicion += " and P.edad >= 45 AND P.edad <= 64 AND P.unidadedad = 0 ";
//                //if (ddlGrupoEtareo.SelectedValue == "12") m_strCondicion += " and P.edad >= 55 AND P.edad <= 64 AND P.unidadedad = 0  ";
//                if (ddlGrupoEtareo.SelectedValue == "12") m_strCondicion += " and P.edad >= 65 AND P.edad <= 74 AND P.unidadedad = 0   ";
//                if (ddlGrupoEtareo.SelectedValue == "13") m_strCondicion += " and P.edad >= 75 AND P.unidadedad = 0 ";
//            }

//            if (ddlSexo.SelectedValue != "0")
//            {
//                if (ddlSexo.SelectedValue == "1")
//                    m_strCondicion += " and P.sexo='F'";
//                if (ddlSexo.SelectedValue == "2")
//                    m_strCondicion += " and P.sexo='M' ";
//            }

//            m_strCondicion += " and P.idOrigen in (" + getListaOrigen() + ")";
//            m_strCondicion += " and P.idSector in (" + getListaSector() + ")";

//            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
//            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

//            string tablaDiag = "sys_cie10  as C10 on C10.id = Pd.iddiagnostico ";
//            //if (oC.NomencladorDiagnostico==1)
//            //    tablaDiag = "lab_diagnostico as C10 on C10.idDiagnostico = Pd.iddiagnostico";

 

//            string m_strSQL = @" SELECT  I.nombre AS ANALISIS, DP.resultadoCar AS RESULTADO, 
//p.fecharegistro as [Fecha Registro], 
//P.numero as [Protocolo], 
//p.numeroOrigen as [Origen],P.numeroorigen2 as [Hisopado],
//convert(varchar(100), e.nombre) as [Efector Procedencia],   
//ca.nombre as [Caracter],
//pac.Apellido , 
//pac.nombre as [Nombre],
//case when p.idpaciente = -1 then '' else case when pac.idEstado = 2 then 'SIN DNI'  else 'DNI' end end as [Tipo Doc.],
//case when p.idpaciente = -1 then 0 else case when pac.idEstado = 2 then 0 ELSE pac.numeroDocumento END end as [numeroDocumento],
//case when p.idpaciente = -1 then '' else convert(varchar(10), pac.fechaNacimiento, 103) end as [Fecha Nacimiento], 
//case when p.idpaciente = -1 then 0 else p.edad end as [Edad],
//case p.unidadEdad when 0 then 'años' when 1 then 'meses' when 2 then 'días' end as [amd],
//case when p.idpaciente = -1 then '' else p.sexo end as [Sexo],
//p.nombreObraSocial as [Obra Social],
//d.calle as [Calle Domicilio], d.barrio as [Barrio Domicilio],
// d.ciudad as [Ciudad Domicilio],  d.provincia as [Provincia Domicilio],d.pais as Pais,
// Pac.informacioncontacto as [Telefono],
//substring(O.nombre, 1, 3) as [Amb / Int.], 
//case when convert(varchar(10), p.fechaTomaMuestra, 103)= '01/01/1900' then ''
//else convert(varchar(10), p.fechaTomaMuestra, 103) end as [F.Toma Muestra],
//M.nombre as Muestra, 
//p.Especialista AS[Solicitante],
//dP.fechavalida as [F.Resultado],
//  isnull(c10.nombre, '') as diagnostico,
//   SS.nombre as Sector
//   FROM LAB_DetalleProtocolo AS DP
//   INNER JOIN LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo
//     left JOIN LAB_Muestra as M on M.idMuestra = p.idMuestra
//   INNER JOIN LAB_Item AS I ON DP.idSUBItem = I.idItem
//   INNER JOIN Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente
//   left join sys_Pacientedomicilio as D on d.idpaciente = Pac.idpaciente
//   left JOIN Lab_protocoloDiagnostico AS PD ON PD.idProtocolo = P.idProtocolo
//    left join " + tablaDiag+ @" 
//    INNER JOIN LAB_Origen as O on O.idOrigen = P.idOrigen
//     inner join lab_sectorservicio as SS on SS.idsectorservicio = P.idsector
//     inner JOIN [SYS_efector] e on  e.idefector = p.idEfectorSolicitante
//      left join lab_caracter as Ca on Ca.idCaracter = p.idcaracter " +
//                           " WHERE (I.idItem=" + m_analisis + ") AND (DP.resultadoCar = '" + m_resultado + "') AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "') and " +
//                           " ( DP.conresultado=1) " + m_strCondicion +
//                           " order by P.fecha  ";


//            if (m_tipo == "PDF")
//                m_strSQL = @" SELECT distinct I.nombre AS ANALISIS, DP.resultadoCar AS RESULTADO, 
//                        case when p.idpaciente=-1 then 0 else case when pa.idEstado = 2 then 0 ELSE pa.numeroDocumento END end as [numeroDocumento],
//  Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento, " +
//                            " 103) AS FECHANACIMIENTO,"+
//                            " d.calle + ' Barrio:' +  d.barrio + ' Ciudad: '+ d.ciudad   AS domicilio, CONVERT(varchar(10), P.fecha, 103) AS fecha,convert(varchar,P.edad) + case  P.unidadedad when 0 then 'A' when 1 then 'M' when 2 then 'D' end as edad, dbo.NumeroProtocolo(P.idProtocolo) as numero, Pa.informacionContacto as campo1,  "
//                            + m_codigopaciente +   
//                            " FROM LAB_DetalleProtocolo AS DP " +
//                            " INNER JOIN LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo " +
//                            " INNER JOIN LAB_Item AS I ON DP.idSUBItem = I.idItem " +
//                            " INNER JOIN Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente" +
//                            " left join sys_Pacientedomicilio as D on d.idpaciente = Pa.idpaciente "+
//                            " left JOIN Lab_protocoloDiagnostico AS PD ON PD.idProtocolo = P.idProtocolo " +
//                            " left join " + tablaDiag +
//                            " INNER JOIN LAB_Origen as O on O.idOrigen= P.idOrigen " +
//                            " inner join lab_sectorservicio as SS on SS.idsectorservicio= P.idsector " +
//                            " WHERE (I.idItem=" + m_analisis + ") AND (DP.resultadoCar = '" + m_resultado + "') AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "') and " +
//                            " ( DP.conresultado=1) " + m_strCondicion +  " order by numero  ";

//            DataSet Ds = new DataSet();
//            //       SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
//            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
//            adapter.Fill(Ds);


//            DataTable data = Ds.Tables[0];
//            return data;
//        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            MostrarReporteGeneral();
        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            MostrarPdf();
        }

        private void MostrarPdf()
        {
            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();

            if (oUser.IdEfector.IdEfector == 227)
            {
                Efector oCon = new Efector(); oCon = (Efector)oCon.Get(typeof(Efector), "IdEfector", oUser.IdEfector.IdEfector);



                //ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.Nombre;

                //ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = "";

                //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = "";
            }
            else

            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);


                encabezado1.Value = oCon.EncabezadoLinea1;


                encabezado2.Value = oCon.EncabezadoLinea2;


                encabezado3.Value = oCon.EncabezadoLinea3;
                ////////////////////////////
            }

            ParameterDiscreteValue titulo = new ParameterDiscreteValue();
            titulo.Value = "INFORME DE RESULTADOS ";

           // if (rdbPaciente.SelectedValue == "1") titulo.Value = "INFORME DE RESULTADOS (EMBARAZADAS)";
            //if (ddlGrupoEtareo.SelectedValue != "0") titulo.Value += "  Grupo Etareo: " + ddlGrupoEtareo.SelectedItem.Text;
            //if (ddlSexo.SelectedValue != "0") titulo.Value += " - Sexo: " + ddlSexo.SelectedItem.Text;

            ParameterDiscreteValue fechaDesde = new ParameterDiscreteValue();
            fechaDesde.Value = txtFechaDesde.Value;

            ParameterDiscreteValue fechaHasta = new ParameterDiscreteValue();
            fechaHasta.Value = txtFechaHasta.Value;


            oCr.Report.FileName = "ResultadoPredefinido2.rpt";
            oCr.ReportDocument.SetDataSource(GetDatosEstadistica("GV"));
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(titulo);
            oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(fechaDesde);
            oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(fechaHasta);
            oCr.DataBind();
            Utility oUtil = new Utility();
            string nombrePDF = oUtil.CompletarNombrePDF("EstadisticaResultados");
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

           // oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "reporteresultados");

           
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarAnalisis();
        }

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

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarAreas();
        }

        private void CargarAreas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            /// 
            string m_ssql = "SELECT  idArea, nombre from LAB_Area where baja=0 and idtipoServicio= " + ddlServicio.SelectedValue + " ORDER BY nombre";

            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("--Todas--", "0"));
            ddlAnalisis.Items.Insert(0, new ListItem("--Todos--", "0"));

        }
    }
}
