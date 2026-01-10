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
using InfoSoftGlobal;

namespace WebLab.Estadisticas
{
    public partial class ReporteForense : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        int suma1 = 0; int suma2=0; int suma3 = 0; int suma4 = 0;
       


        private enum TabIndex
        {
            DEFAULT = 0,
            ONE = 1,
            TWO = 2,
            THREE = 3,
            CUARTO = 4,
            QUINTO = 5
            // you can as many as you want here
        }
        private void SetSelectedTab(TabIndex tabIndex)
        {
            HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               VerificaPermisos("Estadistica Forense");

                txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();
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

  

     


        private void MostrarReporteGeneral()
        {
            DataTable dtCasos = MostrarDatos("Casos");

            //DataTable dtMicroorganismo = MostrarDatos("Aislamiento");

            gvCasos.DataSource = dtCasos;
            gvCasos.DataBind();

            DataTable dtTipoMuestra = MostrarDatos("Tipo de Muestra");

             
            gvTipoMuestra.DataSource = dtTipoMuestra;
            gvTipoMuestra.DataBind();

           

             DataTable dtOrigen = MostrarDatos("Origen Filiacion"); 
             gvSolicitante.DataSource = dtOrigen;
             gvSolicitante.DataBind();

            DataTable dtOrigen1 = MostrarDatos("Origen Forense");
            gvOrigenFiliacion.DataSource = dtOrigen1;
            gvOrigenFiliacion.DataBind();

            DataTable dtMuestras = MostrarDatos("Muestras");
            gvMuestras.DataSource = dtMuestras;
            gvMuestras.DataBind();

            DataTable dtParentesco = MostrarDatos("Vinculo");
            gvParentesco.DataSource = dtParentesco;
            gvParentesco.DataBind();

            DataTable dtLugarExtraccion = MostrarDatos("Lugar");
            gvLugarExtraccion.DataSource = dtLugarExtraccion;
            gvLugarExtraccion.DataBind();


            //HFTipoMuestra.Value = getValoresTipoMuestra();

            ////////////////////////Solapa microorganismos
            //ddlTipoMuestra.DataTextField = "Tipo Muestra";
            //ddlTipoMuestra.DataValueField = "idMuestra";

            //ddlTipoMuestra.DataSource = dtTipoMuestra;
            //ddlTipoMuestra.DataBind();
            //ddlTipoMuestra.Items.Insert(0, new ListItem("--Todas--", "0"));

            //gvMicroorganismos.DataSource = dtMicroorganismo;
            //gvMicroorganismos.DataBind();

            //HFMicroorganismo.Value = getValoresMicroorganismos();
            //gvMicroorganismos.Visible = true;
            //lblFiltroMicroorganismo.Text = "Tipo de Muestra: " + ddlTipoMuestra.SelectedItem.Text + " - ATB: " + ddlATB.SelectedValue;


        }




        private string getValoresTipoMuestra()
        {
            string s_valores = "";
           
                for (int i = 0; i < gvCasos.Rows.Count; i++)
                {
                    string s_nombre = gvCasos.Rows[i].Cells[0].Text.Replace(";", "");
                    s_nombre = s_nombre.Replace("&#", "");
                    if (s_valores=="")
                        s_valores = "name='" + s_nombre + "' value='" + gvCasos.Rows[i].Cells[1].Text + "'";
                    else
                        s_valores += ";" + "name='" + s_nombre + "' value='" + gvCasos.Rows[i].Cells[1].Text + "'";
                }
                
            return  s_valores;
        }




        protected void lnkPdf_Click(object sender, EventArgs e)
        {

        }




          private DataTable MostrarDatos(string s_tipo)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "LAB_EstadisticaGeneticaForense";

            int tipoM = 0;
           

            int idsubitem=0;

        

            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            ///////


            //cmd.Parameters.Add("@idAnalisis", SqlDbType.Int);
            //cmd.Parameters["@idAnalisis"].Value = int.Parse(ddlAnalisis.SelectedValue);

        



            cmd.Parameters.Add("@tipoReporte", SqlDbType.NVarChar);
            cmd.Parameters["@tipoReporte"].Value = s_tipo;


            cmd.Parameters.Add("@idTipoMuestra", SqlDbType.Int);
            cmd.Parameters["@idTipoMuestra"].Value = tipoM;



            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);

            return Ds.Tables[0];
            
            

        }




        private string mostrarGrafico(int p)
        {
            string s_titulo="";
            string s_tipo="";
            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf";
            DataTable dt = new DataTable();
            //dt = (DataTable)gvTipoMuestra.DataSource;
            string strXML = "";
            string ancho = "500";
            if (p == 0)
            {
                //    s_tipografico = "../FusionCharts/FCF_Pie3D.swf";
                s_titulo = "No Pacientes";
                s_tipo = "Casos por tipo de muestra";
                 strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";

                if (gvCasos.Rows.Count > 0)
                {
                    for (int i = 0; i < gvCasos.Rows.Count; i++)
                    {
                        strXML += "<set name='" + gvCasos.Rows[i].Cells[0].Text + "' value='" + gvCasos.Rows[i].Cells[1].Text + "' />";
                    }
                }
                strXML += "</graph>";
            }

            if (p == 1)
            {

                ancho = "1000";

            //    s_tipografico = "../FusionCharts/FCF_Column2D.swf";

                ////s_titulo = ddlAnalisis.SelectedItem.Text +" " +  ddlTipoMuestra.SelectedItem.Text;
                //s_tipo = "Casos por Aislamiento";
                //strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";

                //if (gvMicroorganismos.Rows.Count > 0)
                //{
                //    for (int i = 0; i < gvMicroorganismos.Rows.Count; i++)
                //    {
                //        strXML += "<set name='" + gvMicroorganismos.Rows[i].Cells[0].Text.Substring(0,5) + "' value='" + gvMicroorganismos.Rows[i].Cells[1].Text + "' />";
                //    }
                //}
                //strXML += "</graph>";
            }
            //else
            //    Response.Redirect("SinDatos.aspx", false);


          

            return FusionCharts.RenderChart(s_tipografico, p.ToString(), strXML, "Sales"+p.ToString(), ancho, "200", false, false);
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

        





        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

            ExportarExcelTipoMuestra();

        }

        private void ExportarExcelTipoMuestra()
        {
            Utility.ExportGridViewToExcel(gvCasos, "NoPaciente_TipoMuestra");
            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Page page = new Page();
            //HtmlForm form = new HtmlForm();
            //gvCasos.EnableViewState = false;

            //// Deshabilitar la validación de eventos, sólo asp.net 2
            //page.EnableEventValidation = false;

            //// Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            //page.DesignerInitialize();
            //page.Controls.Add(form);
            //form.Controls.Add(gvCasos);
            //page.RenderControl(htw);

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename=NoPaciente_TipoMuestra.xls");
            //Response.Charset = "UTF-8";
            //Response.ContentEncoding = Encoding.Default;
            //Response.Write(sb.ToString());
            //Response.End();
        }

      

        protected void gvEstadistica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Pacientes")
            //    InformePacientes(e.CommandArgument.ToString());

        }

 
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            MostrarReporteGeneral();
            pnlResultados.Visible = true;
            
        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
         //   MostrarPdf();
        }

        protected void imgExcel0_Click(object sender, ImageClickEventArgs e)
        {
            //ExportarExcelMicroorganismos();
        }

        

        protected void imgExcel1_Click(object sender, ImageClickEventArgs e)
        {
            //ExportarExcelAntibioticos();
        }

        

        //protected void imgExcel2_Click(object sender, ImageClickEventArgs e)
        //{
        //    ExportarExcelResultados();
        //}

        //private void ExportarExcelResultados()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter sw = new StringWriter(sb);
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    Page page = new Page();
        //    HtmlForm form = new HtmlForm();
        //    gvResultado.EnableViewState = false;

        //    // Deshabilitar la validación de eventos, sólo asp.net 2
        //    page.EnableEventValidation = false;

        //    // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
        //    page.DesignerInitialize();
        //    page.Controls.Add(form);
        //    form.Controls.Add(gvResultado);
        //    page.RenderControl(htw);

        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.AddHeader("Content-Disposition", "attachment;filename=_Resultado.xls");
        //    Response.Charset = "UTF-8";
        //    Response.ContentEncoding = Encoding.Default;
        //    Response.Write(sb.ToString());
        //    Response.End();
        //}

        protected void gvTipoMuestra_RowDataBound(object sender, GridViewRowEventArgs e)
        {



            if (e.Row.RowType == DataControlRowType.Header)
            {
                suma1 = 0;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                if (e.Row.Cells[1].Text != "&nbsp;") suma1 += int.Parse(e.Row.Cells[1].Text);






            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma1.ToString();
                lblCantidadCasos.Text = "Total de casos: " + suma1.ToString();
                lblCantidadCasos.Visible = true;
            }
        }


        protected void gvMuestras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                suma2 = 0;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                if (e.Row.Cells[1].Text != "&nbsp;") suma2 += int.Parse(e.Row.Cells[1].Text);






            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
              lblCantidadMuestras.Text = "Total de muestras: " + suma2.ToString();
                lblCantidadMuestras.Visible = true;
            }



        }

        protected void gvMicroorganismos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
          

         
        }


        protected void gvAntibiotico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }

        
        
        
   

        protected void gvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
           
                //for (int i = 1; i <= 16; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
            
        }

        //protected void btnBuscarAislamiento_Click(object sender, EventArgs e)
        //{
           
        //    DataTable dt = MostrarDatos("Resultados");

        //    gvResultado.DataSource =dt;
        //    gvResultado.DataBind();
          
            

        //    SetSelectedTab(TabIndex.TWO);
            
        //}



     

        protected void btnVerParametro_Click(object sender, EventArgs e)
        {
            MostrarDatos("Parametro");
        }

     

        

      


        protected void gvResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        private void InformePacientes(string p)
        {
          
        }



      
          

        protected void gvMicroorganismosATB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              
                for (int i = 1; i <=4; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
            
        }

        protected void gvSolicitante_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                suma3 = 0;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") suma3 += int.Parse(e.Row.Cells[1].Text);






            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma3.ToString();
              


            }
        }

        protected void gvOrigenFiliacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                suma4 = 0;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") suma4 += int.Parse(e.Row.Cells[1].Text);






            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma4.ToString();



            }
        }

        //protected void btnBuscarAislamiento2_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = MostrarDatos("Aislamiento");

        //    gvMicroorganismos.DataSource = dt;
        //    gvMicroorganismos.DataBind();
        //    //HFMicroorganismo.Value = getValoresMicroorganismos();
        //    SetSelectedTab(TabIndex.THREE);
        //}
    }
}
