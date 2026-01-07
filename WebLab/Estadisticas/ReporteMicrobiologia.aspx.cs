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
    public partial class ReporteMicrobiologia : System.Web.UI.Page
    {
       
        public Configuracion oC = new Configuracion();
        public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();

        int suma1 = 0;
        int grupo1 = 0; int grupo2 = 0; int grupo3 = 0; int grupo4=0; int grupo5=0; int grupo6=0; int grupo7=0; int grupo8=0; int grupo9=0; int grupo10=0;
        int grupo11 = 0; int grupo12 = 0; int grupo13 = 0; int grupo14 = 0;
        int masc = 0; int fem = 0; int ind = 0; int emb = 0;


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
                    VerificaPermisos("De Microbiologia");
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
        private void CargarListas()
        {
            Utility oUtil = new Utility(); string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de tipos de servicios
            string m_ssql =  "SELECT     I.idItem, I.nombre + ' (' + I.codigo + ')' AS nombre " +
            " FROM         LAB_Item as I" +
            " INNER JOIN LAB_Area  as A ON A.idArea= I.idArea  " +
            " WHERE   I.disponible=1 AND (I.idEfectorDerivacion = I.idEfector) AND (I.baja = 0) and A.idTipoServicio=3 AND (tipo = 'P')  " +
            " ORDER BY I.nombre";

            oUtil.CargarCombo(ddlAnalisis, m_ssql, "idItem", "nombre", connReady);
            ddlAnalisis.Items.Insert(0, new ListItem("--Seleccione--", "0"));

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

            m_ssql = "SELECT     idOrigen, nombre  AS nombre FROM         LAB_Origen where  baja = 0  ORDER BY nombre";
            oUtil.CargarCheckBox(ChckOrigen, m_ssql, "idOrigen", "nombre");
            for (int i = 0; i < ChckOrigen.Items.Count; i++)
                ChckOrigen.Items[i].Selected = true;
            m_ssql = null;
            oUtil = null;
        }

        private void MostrarReporteGeneral()
        {
            DataTable dtTipoMuestra = MostrarDatos("Tipo de Muestra");
            DataTable dtMicroorganismo = MostrarDatos("Aislamiento");
            DataTable dtResultado = MostrarDatos("Resultado");
            
            gvTipoMuestra.DataSource =dtTipoMuestra;
            gvTipoMuestra.DataBind();

            HFTipoMuestra.Value = getValoresTipoMuestra();

            //////////////////////Solapa microorganismos
            ddlTipoMuestra.DataTextField = "Tipo Muestra";
            ddlTipoMuestra.DataValueField = "idMuestra";

            ddlTipoMuestra.DataSource = dtTipoMuestra;
            ddlTipoMuestra.DataBind();
            ddlTipoMuestra.Items.Insert(0, new ListItem("--Todas--", "0"));

            gvMicroorganismos.DataSource = dtMicroorganismo;
            gvMicroorganismos.DataBind();

            HFMicroorganismo.Value = getValoresMicroorganismos();
            gvMicroorganismos.Visible = true;
            lblFiltroMicroorganismo.Text = "Tipo de Muestra: " + ddlTipoMuestra.SelectedItem.Text + " - ATB: " + ddlATB.SelectedValue;

    
            ///llena los tipo de muestra de la grilla principal
                ddlTipoMuestraAntibioticos.DataTextField = "Tipo Muestra";
                ddlTipoMuestraAntibioticos.DataValueField = "idMuestra";
                ddlTipoMuestraAntibioticos.DataSource =dtTipoMuestra;
                ddlTipoMuestraAntibioticos.DataBind();
                ddlTipoMuestraAntibioticos.Items.Insert(0, new ListItem("--Todas--", "0"));

           
                ddlTipoMuestraMecanismo.DataTextField = "Tipo Muestra";
            ddlTipoMuestraMecanismo.DataValueField = "idMuestra";
            ddlTipoMuestraMecanismo.DataSource = dtTipoMuestra;
            ddlTipoMuestraMecanismo.DataBind();
            ddlTipoMuestraMecanismo.Items.Insert(0, new ListItem("--Todas--", "0"));


            ddlMicroorganismosAntibioticos.DataTextField = "Microorganismo";
                ddlMicroorganismosAntibioticos.DataValueField = "idGermen";
                ddlMicroorganismosAntibioticos.DataSource =dtMicroorganismo;
                ddlMicroorganismosAntibioticos.DataBind();
                ddlMicroorganismosAntibioticos.Items.Insert(0, new ListItem("--Todos--", "0"));

            
            


            ///////////////////
            gvResultado.DataSource = dtResultado;
            gvResultado.DataBind();


            gvMicroorganismosATB.Visible = false;
            gvAntibioticoResistencia.Visible = false;

          



        if (gvTipoMuestra.Rows.Count == 0) Response.Redirect("SinDatos.aspx?Desde=ReporteMicrobiologia.aspx", false);
            else
            {
                lblAnalisis.Text = ddlAnalisis.SelectedItem.Text;
                pnlResultado.Visible = true;
                SetSelectedTab(TabIndex.ONE);
            }
        }

        private string getValoresMicroorganismos()
        {
            string s_valores = "";

            for (int i = 0; i < gvMicroorganismos.Rows.Count; i++)
            {
                string s_nombre = gvMicroorganismos.Rows[i].Cells[0].Text.Replace(";", "");
                s_nombre = s_nombre.Replace("&#", "");
                    
                if (s_valores == "")

                    s_valores = "name='" + s_nombre + "' value='" + gvMicroorganismos.Rows[i].Cells[1].Text + "'";
                else
                    s_valores += ";" + "name='" + s_nombre + "' value='" + gvMicroorganismos.Rows[i].Cells[1].Text + "'";
            }
            
            return s_valores;
        }

        private string getValoresTipoMuestra()
        {
            string s_valores = "";
           
                for (int i = 0; i < gvTipoMuestra.Rows.Count; i++)
                {
                    string s_nombre = gvTipoMuestra.Rows[i].Cells[0].Text.Replace(";", "");
                    s_nombre = s_nombre.Replace("&#", "");
                    if (s_valores=="")
                        s_valores = "name='" + s_nombre + "' value='" + gvTipoMuestra.Rows[i].Cells[1].Text + "'";
                    else
                        s_valores += ";" + "name='" + s_nombre + "' value='" + gvTipoMuestra.Rows[i].Cells[1].Text + "'";
                }
                
            return  s_valores;
        }




        protected void lnkPdf_Click(object sender, EventArgs e)
        {

        }




          private DataTable MostrarDatos(string s_tipo)
        {
            DataSet Ds = new DataSet();
            //  SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_EstadisticaMicrobiologia]";

            int tipoM = 0;
            int conATB=0;
            if (s_tipo=="Aislamiento")            
            {
                if (ddlTipoMuestra.SelectedValue != "") tipoM= int.Parse(ddlTipoMuestra.SelectedValue);
                if (ddlATB.SelectedValue != "Todos")
                {
                    if (ddlATB.SelectedValue == "Con ATB") conATB = 1;  ///si 
                    else conATB = 2;//no
                }
            }

            if (s_tipo == "Mecanismo")
                
            { if (ddlTipoMuestraMecanismo.SelectedValue != "") tipoM = int.Parse(ddlTipoMuestraMecanismo.SelectedValue); }


            if (s_tipo == "Antibiotico")
            { if (ddlTipoMuestraAntibioticos.SelectedValue != "") tipoM = int.Parse(ddlTipoMuestraAntibioticos.SelectedValue); }

            int tipoGermen = 0;
            if (s_tipo == "Antibiotico") { if (ddlMicroorganismosAntibioticos.SelectedValue != "") tipoGermen = int.Parse(ddlMicroorganismosAntibioticos.SelectedValue); }

            
            int tipoAntibiotico = 0;
            if (s_tipo == "Resistencia") {
                if (ddlTipoMuestraAntibioticos.SelectedValue != "") tipoM = int.Parse(ddlTipoMuestraAntibioticos.SelectedValue); 
                if (ddlMicroorganismosAntibioticos.SelectedValue != "") tipoGermen = int.Parse(ddlMicroorganismosAntibioticos.SelectedValue); 
                if (hdfidAntibiotico.Value != "") tipoAntibiotico = int.Parse(hdfidAntibiotico.Value); }
            
            
            int idsubitem=0;

            //if (s_tipo == "Parametro")
            //{

            //    if (ddlSubItem.SelectedValue != "") idsubitem = int.Parse(ddlSubItem.SelectedValue);
            //}
            ///Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            ///////


            cmd.Parameters.Add("@idAnalisis", SqlDbType.Int);
            cmd.Parameters["@idAnalisis"].Value =int.Parse( ddlAnalisis.SelectedValue);

            cmd.Parameters.Add("@idOrigen", SqlDbType.NVarChar);
            cmd.Parameters["@idOrigen"].Value =getListaOrigen();

            cmd.Parameters.Add("@idTipoMuestra", SqlDbType.Int);
            cmd.Parameters["@idTipoMuestra"].Value = tipoM;


            cmd.Parameters.Add("@idGermen", SqlDbType.Int);
            cmd.Parameters["@idGermen"].Value = tipoGermen;

            cmd.Parameters.Add("@idAntibiotico", SqlDbType.Int);
            cmd.Parameters["@idAntibiotico"].Value = tipoAntibiotico;

            cmd.Parameters.Add("@idsubitem", SqlDbType.Int);
            cmd.Parameters["@idsubitem"].Value = idsubitem;
            
            
            cmd.Parameters.Add("@tipoReporte", SqlDbType.NVarChar);
            cmd.Parameters["@tipoReporte"].Value = s_tipo;

            cmd.Parameters.Add("@conATB", SqlDbType.Int);
            cmd.Parameters["@conATB"].Value = conATB;

            cmd.Parameters.Add("@idEfectorProtocolo", SqlDbType.Int);
            cmd.Parameters["@idEfectorProtocolo"].Value = int.Parse(ddlEfector.SelectedValue);




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
                s_titulo = ddlAnalisis.SelectedItem.Text;
                s_tipo = "Casos por tipo de muestra";
                 strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";

                if (gvTipoMuestra.Rows.Count > 0)
                {
                    for (int i = 0; i < gvTipoMuestra.Rows.Count; i++)
                    {
                        strXML += "<set name='" + gvTipoMuestra.Rows[i].Cells[0].Text + "' value='" + gvTipoMuestra.Rows[i].Cells[1].Text + "' />";
                    }
                }
                strXML += "</graph>";
            }

            if (p == 1)
            {

                ancho = "1000";

            //    s_tipografico = "../FusionCharts/FCF_Column2D.swf";

                s_titulo = ddlAnalisis.SelectedItem.Text +" " +  ddlTipoMuestra.SelectedItem.Text;
                s_tipo = "Casos por Aislamiento";
                strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";

                if (gvMicroorganismos.Rows.Count > 0)
                {
                    for (int i = 0; i < gvMicroorganismos.Rows.Count; i++)
                    {
                        strXML += "<set name='" + gvMicroorganismos.Rows[i].Cells[0].Text.Substring(0,5) + "' value='" + gvMicroorganismos.Rows[i].Cells[1].Text + "' />";
                    }
                }
                strXML += "</graph>";
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

        protected void gvEstadistica_RowDataBound(object sender, GridViewRowEventArgs e)
        {


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton CmdProtocolo = (ImageButton)e.Row.Cells[15].Controls[1];
                    CmdProtocolo.CommandArgument = ddlAnalisis.SelectedValue +"~" + e.Row.Cells[0].Text; ///Codigo1 + ";" + codigo2
                    CmdProtocolo.CommandName = "Pacientes";
                    CmdProtocolo.ToolTip = "Ver Pacientes";
              
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

                    if (e.Row.Cells[12].Text != "&nbsp;") grupo11 += int.Parse(e.Row.Cells[12].Text);
                    if (e.Row.Cells[13].Text != "&nbsp;") grupo12 += int.Parse(e.Row.Cells[13].Text);
                    if (e.Row.Cells[14].Text != "&nbsp;") grupo13 += int.Parse(e.Row.Cells[14].Text);
                //if (e.Row.Cells[15].Text != "&nbsp;") grupo14 += int.Parse(e.Row.Cells[15].Text);
                if (e.Row.Cells[16].Text != "&nbsp;") masc += int.Parse(e.Row.Cells[15].Text);
                if (e.Row.Cells[17].Text != "&nbsp;") fem += int.Parse(e.Row.Cells[16].Text);
                if (e.Row.Cells[18].Text != "&nbsp;") ind += int.Parse(e.Row.Cells[17].Text);


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
                    e.Row.Cells[17].Text = ind.ToString();
                    
                }

          
        }





        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

            ExportarExcelTipoMuestra();

        }

        private void ExportarExcelTipoMuestra()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gvTipoMuestra.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gvTipoMuestra);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_TipoMuestra.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void ddlAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    gvTipoMuestra.DataSource = GetDatosEstadistica("GV");
        //    gvTipoMuestra.DataBind();
        }

        protected void gvEstadistica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Pacientes")
            //    InformePacientes(e.CommandArgument.ToString());

        }

 
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            MostrarReporteGeneral();
        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
         //   MostrarPdf();
        }

        protected void imgExcel0_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcelMicroorganismos();
        }

        private void ExportarExcelMicroorganismos()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gvMicroorganismos.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gvMicroorganismos);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Microorganismos.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void imgExcel1_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcelAntibioticos();
        }

        private void ExportarExcelAntibioticos()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gvAntibiotico.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gvAntibiotico);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Antibiotico.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void imgExcel2_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcelResultados();
        }

        private void ExportarExcelResultados()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gvResultado.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gvResultado);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Resultado.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void gvTipoMuestra_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                suma1 = 0;
                grupo1 = 0; grupo2 = 0; grupo3 = 0; grupo4 = 0; grupo5 = 0; grupo6 = 0; grupo7 = 0; grupo8 = 0; grupo9 = 0; grupo10 = 0;
                grupo11 = 0; grupo12 = 0; grupo13 = 0;
                masc = 0; fem = 0; ind = 0; emb = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              

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
                if (e.Row.Cells[12].Text != "&nbsp;") grupo11 += int.Parse(e.Row.Cells[12].Text);
                if (e.Row.Cells[13].Text != "&nbsp;") grupo12 += int.Parse(e.Row.Cells[13].Text);
                //if (e.Row.Cells[14].Text != "&nbsp;") grupo13 += int.Parse(e.Row.Cells[14].Text);

                if (e.Row.Cells[14].Text != "&nbsp;") masc += int.Parse(e.Row.Cells[14].Text);
                if (e.Row.Cells[15].Text != "&nbsp;") fem += int.Parse(e.Row.Cells[15].Text);
                if (e.Row.Cells[16].Text != "&nbsp;") emb += int.Parse(e.Row.Cells[16].Text);
                if (e.Row.Cells[17].Text != "&nbsp;") ind += int.Parse(e.Row.Cells[17].Text);


                


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
                //e.Row.Cells[14].Text = grupo13.ToString();
                e.Row.Cells[14].Text = masc.ToString();
                e.Row.Cells[15].Text = fem.ToString();
                e.Row.Cells[16].Text = emb.ToString();
                e.Row.Cells[17].Text = ind.ToString();

            }
            for (int i = 1; i <= 18; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";

          
        }

        protected void gvMicroorganismos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton CmdModificar = (LinkButton)e.Row.Cells[19].Controls[1];
                CmdModificar.CommandArgument = gvMicroorganismos.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Resistencia";
                CmdModificar.ToolTip = "Resistencia";
                for (int i = 1; i <= 18; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
           
            

         
        }


        protected void gvAntibiotico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Resistencia")
            {
                hdfidAntibiotico.Value= e.CommandArgument.ToString();
               
               // MostrarDatos("Resistencia");


                DataTable dt = MostrarDatos("Resistencia");
                gvAntibioticoResistencia.DataSource =dt;
                gvAntibioticoResistencia.DataBind();
                gvAntibioticoResistencia.Visible = true;
                
                    Antibiotico oAnti = new Antibiotico();
                    oAnti = (Antibiotico)oAnti.Get(typeof(Antibiotico),int.Parse( hdfidAntibiotico.Value));

                    string seleccion = "Tipo de Muestra:" + ddlTipoMuestraAntibioticos.SelectedItem.Text + " - " + " Microorganismo: " + ddlMicroorganismosAntibioticos.SelectedItem.Text;
                    lblResistenciaAntibiotico.Text = seleccion + " - Resistencia de " + oAnti.Descripcion;
                    lblResistenciaAntibiotico.Visible = true;
                

                

            }
        }

        
        
        
        protected void gvAntibiotico_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                 suma1 = 0;
                 grupo1 = 0; grupo2 = 0; grupo3 = 0; grupo4 = 0;  grupo5 = 0; grupo6 = 0;  grupo7 = 0; grupo8 = 0; grupo9 = 0; grupo10 = 0;
                 masc = 0; fem = 0;  ind = 0;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               LinkButton CmdModificar = (LinkButton)e.Row.Cells[19].Controls[1];
               CmdModificar.CommandArgument = gvAntibiotico.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Resistencia";
                CmdModificar.ToolTip = "Resistencia";
                for (int i = 1; i <= 19; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
            
         
            
        }

        protected void gvResultado_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
           
                for (int i = 1; i <= 19; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
            
        }

        protected void btnBuscarAislamiento_Click(object sender, EventArgs e)
        {
            //MostrarDatos("Aislamiento");

            DataTable dt = MostrarDatos("Aislamiento");

            gvMicroorganismos.DataSource =dt;          
            gvMicroorganismos.DataBind();
            HFMicroorganismo.Value = getValoresMicroorganismos();
            //gvMicroorganismos.Visible = true;

            gvMicroorganismosATB.Visible = false;
            lblFiltroMicroorganismoATB.Visible = false;
            btnGraficoResistencia.Visible = false;
            lblFiltroMicroorganismo.Text = "Tipo de Muestra: " + ddlTipoMuestra.SelectedItem.Text + " - ATB: " + ddlATB.SelectedValue;

          //  gvMicroorganismos.UpdateAfterCallBack = true;
          //  lblFiltroMicroorganismo.UpdateAfterCallBack = true;
            

            SetSelectedTab(TabIndex.TWO);
            
        }

        protected void btnBuscarAntibioticos_Click(object sender, EventArgs e)
        {
         


            

            DataTable dt = MostrarDatos("Antibiotico");


            gvAntibiotico.DataSource = dt;
            gvAntibiotico.DataBind();
            lblFiltroAntibiotico.Text = "Tipo de Muestra: " + ddlTipoMuestraAntibioticos.SelectedItem.Text + " - Aislamiento: " + ddlMicroorganismosAntibioticos.SelectedItem.Text;
            gvAntibioticoResistencia.Visible = false;
        
            SetSelectedTab(TabIndex.THREE);

        }

     

        protected void btnVerParametro_Click(object sender, EventArgs e)
        {
            MostrarDatos("Parametro");
        }

     

        protected void gvAntibioticoResistencia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    suma1 = 0;
                    grupo1 = 0; grupo2 = 0; grupo3 = 0; grupo4 = 0; grupo5 = 0; grupo6 = 0; grupo7 = 0; grupo8 = 0; grupo9 = 0; grupo10 = 0;
                    masc = 0; fem = 0; ind = 0;
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


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

                    if (e.Row.Cells[12].Text != "&nbsp;") masc += int.Parse(e.Row.Cells[12].Text);
                    if (e.Row.Cells[13].Text != "&nbsp;") fem += int.Parse(e.Row.Cells[13].Text);
                    if (e.Row.Cells[14].Text != "&nbsp;") ind += int.Parse(e.Row.Cells[14].Text);


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
                    e.Row.Cells[12].Text = masc.ToString();
                    e.Row.Cells[13].Text = fem.ToString();
                    e.Row.Cells[14].Text = ind.ToString();

                }
                for (int i = 1; i <= 14; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
            catch (Exception ex)
            {
                //string exception = "";
                //while (ex != null)
                //{
                //    exception = ex.Message + "<br>";

                //}
            }
        }

        protected void btnDescargarDetallePacientes_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = GetDataPacientes("General");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Pacientes.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void gvResultado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Pacientes")
            //    InformePacientes(e.CommandArgument.ToString());

        }

        private void InformePacientes(string p)
        {
          
        }

        private DataTable GetDataPacientes(string tipo)
        {
            string m_strCondicion = " and P.baja=0 ";

          

            
            //    if (ddlSexo.SelectedValue == "1")
                    m_strCondicion += " and P.idOrigen in (" + getListaOrigen() +")";
            if (ddlEfector.SelectedValue != "0")
                m_strCondicion += " and P.idEfector=" + ddlEfector.SelectedValue;
            

            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            string m_strSQL = "";


            if (tipo == "General")
            {      m_strSQL = @"SELECT distinct
P.numero as [Numero Protocolo], CONVERT(varchar(10), P.fecha, 103) AS fecha,M.nombre AS [Tipo de Muestra], 
case when Pa.idestado=2 then 0 else Pa.numeroDocumento end as [Nro. Documento], 
Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento,  103) AS FECHANACIMIENTO, Pa.referencia AS domicilio, 
P.edad  , case P.unidadEdad when 0 then 'años' when 1 then 'meses' when 2 then 'días' end tipoEdad ,P.sexo ,
case when PD.iddiagnostico is null then 'No' else 'Si' end as embarazada

FROM LAB_DetalleProtocolo AS DP  
INNER JOIN LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo  
INNER JOIN Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente 
inner join lab_muestra as M on M.idmuestra=P.idmuestra
left JOIN vta_LAB_Embarazadas AS PD ON PD.idProtocolo = P.idProtocolo  
WHERE dp.idItem=" + ddlAnalisis.SelectedValue + " AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "')  and P.idtiposervicio=3 and P.estado=2" + m_strCondicion;
        }
            if (tipo == "Resultado")
            {
                m_strSQL = @" SELECT  P.numero as [Numero Protocolo], CONVERT(varchar(10), P.fecha, 103) AS fecha, 
case when Pa.idestado=2 then 0 else Pa.numeroDocumento end as [Nro. Documento], 
Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento,  103) AS FECHANACIMIENTO, 
Pa.referencia AS domicilio, P.edad  , case P.unidadEdad when 0 then 'años' when 1 then 'meses' when 2 then 'días' end tipoEdad,P.sexo ,
case when PD.iddiagnostico is null then 'No' else 'Si' end as embarazada, I1.nombre AS ANALISIS, DP.resultadoCar AS RESULTADO  

FROM LAB_DetalleProtocolo AS DP  
INNER JOIN LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo  
INNER JOIN LAB_Item AS I ON DP.idItem = I.idItem  
inner join lab_item as I1 on Dp.idsubitem= I1.iditem
INNER JOIN Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente 
left JOIN vta_LAB_Embarazadas AS PD ON PD.idProtocolo = P.idProtocolo  

WHERE dp.idItem=" + ddlAnalisis.SelectedValue + " AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "')  and P.idtiposervicio=3 " +
   " and I1.idtiporesultado=3 and resultadocar<>'' and idusuariovalida>0   " + m_strCondicion + " order by P.idprotocolo ,P.fecha  ";

            }

            if (tipo == "Aislamiento")
            {
                m_strSQL = @"SELECT DISTINCT 
                     P.numero AS [Numero Protocolo], CONVERT(varchar(10), P.fecha, 103) AS fecha, M.nombre AS [Tipo de Muestra], 
                      Pa.numeroDocumento AS [Nro. Documento], Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento, 103) AS FECHANACIMIENTO, 
                      Pa.referencia AS domicilio, P.edad,
 case P.unidadEdad when 0 then 'años' when 1 then 'meses' when 2 then 'días' end tipoEdad, P.sexo, case when PD.iddiagnostico is null then 'No' else 'Si' end as embarazada,
 AIS.nombre AS aislamiento, case when AIS.atb =1 then 'Si'  else 'No' end as atb
FROM         LAB_Protocolo AS P INNER JOIN
                      Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente INNER JOIN
                      LAB_Muestra AS M ON M.idMuestra = P.idMuestra INNER JOIN
                      vta_LAB_Aislamiento AS AIS ON P.idProtocolo = AIS.idProtocolo LEFT OUTER JOIN
                      vta_LAB_Embarazadas AS PD ON PD.idProtocolo = P.idProtocolo
WHERE ais.idItem=" + ddlAnalisis.SelectedValue + " AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "')  and P.idtiposervicio=3 "  + m_strCondicion; // +" order by P.idprotocolo ,P.fecha  ";

            }


            if (tipo == "ATB")
            {
                m_strSQL = @"SELECT DISTINCT 
                     P.numero AS [Numero Protocolo], CONVERT(varchar(10), P.fecha, 103) AS fecha, M.nombre AS [Tipo de Muestra], 
                      Pa.numeroDocumento AS [Nro. Documento], Pa.apellido, Pa.nombre, CONVERT(VARCHAR(10), Pa.fechaNacimiento, 103) AS FECHANACIMIENTO, 
                      Pa.referencia AS domicilio, P.edad, CASE P.unidadEdad WHEN 0 THEN 'años' WHEN 1 THEN 'meses' WHEN 2 THEN 'días' END AS tipoEdad, P.sexo,
                      CASE WHEN PD.iddiagnostico IS NULL THEN 'No' ELSE 'Si' END AS embarazada, ATB.germen AS [Mricroorganismo], ATB.antibiotico , ATB.resultado as [Resistencia]
FROM         LAB_Protocolo AS P INNER JOIN
                      Sys_Paciente AS Pa ON P.idPaciente = Pa.idPaciente INNER JOIN
                      LAB_Muestra AS M ON M.idMuestra = P.idMuestra INNER JOIN
                      vta_LAB_Antibiograma AS ATB ON P.idProtocolo = ATB.idProtocolo LEFT OUTER JOIN
                      vta_LAB_Embarazadas AS PD ON PD.idProtocolo = P.idProtocolo
WHERE ATB.idItem=" + ddlAnalisis.SelectedValue + " AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "')  and P.idtiposervicio=3 " + m_strCondicion;// +" order by P.idprotocolo ,P.fecha  ";

            }


            if (tipo == "Mecanismo")
            {
                m_strSQL = @"	;WITH Mecanismos AS (
    SELECT 
        aM.idProtocolo,
        aM.idItem,
        aM.idGermen,
        aM.idMetodologia,
        Mecanismo = STUFF((
            SELECT DISTINCT ' + ' + M2.nombre
            FROM lab_ProtocoloAtbMecanismo aM2
            INNER JOIN LAB_MecanismoResistencia M2 
                ON M2.idMecanismoResistencia = aM2.idMecanismoResistencia
            WHERE aM2.idProtocolo = aM.idProtocolo
              AND aM2.idItem = aM.idItem
              AND aM2.idGermen = aM.idGermen
              AND aM2.idMetodologia = aM.idMetodologia
            FOR XML PATH(''), TYPE
        ).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
    FROM lab_ProtocoloAtbMecanismo aM
)
SELECT DISTINCT 
    P.numero AS [Numero Protocolo],
    CONVERT(varchar(10), P.fecha, 103) AS fecha,
    M.nombre AS [Tipo de Muestra],
    Pa.numeroDocumento AS [Nro. Documento],
    Pa.apellido,
    Pa.nombre,
    CONVERT(VARCHAR(10), Pa.fechaNacimiento, 103) AS FECHANACIMIENTO,
    Pa.referencia AS domicilio,
    P.edad,
    CASE P.unidadEdad WHEN 0 THEN 'años' WHEN 1 THEN 'meses' WHEN 2 THEN 'días' END AS tipoEdad,
    P.sexo,
    CASE WHEN PD.iddiagnostico IS NULL THEN 'No' ELSE 'Si' END AS embarazada,
    Mec.Mecanismo
FROM LAB_Protocolo P
INNER JOIN Sys_Paciente Pa ON P.idPaciente = Pa.idPaciente
INNER JOIN LAB_Muestra M ON M.idMuestra = P.idMuestra
INNER JOIN LAB_Antibiograma ANT ON P.idProtocolo = ANT.idProtocolo
INNER JOIN lab_ProtocoloAtbMecanismo aM 
    ON aM.idProtocolo = ANT.idProtocolo
    AND aM.idItem = ANT.idItem
    AND aM.idGermen = ANT.idGermen
    AND aM.idMetodologia = ANT.idMetodologia
INNER JOIN Mecanismos Mec
    ON Mec.idProtocolo = aM.idProtocolo
    AND Mec.idItem = aM.idItem
    AND Mec.idGermen = aM.idGermen
    AND Mec.idMetodologia = aM.idMetodologia
LEFT JOIN vta_LAB_Embarazadas PD ON PD.idProtocolo = P.idProtocolo
WHERE ANT.idItem=" + ddlAnalisis.SelectedValue + " AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + "')  and P.idtiposervicio=3 " + m_strCondicion;// +" order by P.idprotocolo ,P.fecha  ";

            }

            DataSet Ds = new DataSet();
            //     SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            DataTable data = Ds.Tables[0];
            return data;
        }

      

        protected void imgExcelResultadoPacientes_Click1(object sender, ImageClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = GetDataPacientes("Resultado");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text+ "_Resultados.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
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
            dg.DataSource = GetDataPacientes("General");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Pacientes.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void imgExcelDetallePacientesAislamientos_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = GetDataPacientes("Aislamiento");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text.Trim() + "_Pacientes.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void imgExcelDetalleAtb_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = GetDataPacientes("ATB");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();

        }

        protected void gvMicroorganismos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Resistencia")
            {
                try
                {
                    int idGermen = int.Parse(e.CommandArgument.ToString());

                    // MostrarDatos("Resistencia");

                    Germen oAnti = new Germen();
                    oAnti = (Germen)oAnti.Get(typeof(Germen), idGermen);
                    //  DataTable dt = MostrarDatos("Resistencia");
                    gvMicroorganismosATB.DataSource = getATB(oAnti.Nombre);
                    gvMicroorganismosATB.DataBind();
                    gvMicroorganismosATB.Visible = true;
                    lblFiltroMicroorganismoATB.Visible = true;
                    btnGraficoResistencia.Visible = true;

                    HFResistencia.Value = getValoresResistencia();


                    //string seleccion = "Tipo de Muestra:" + ddlTipoMuestraAntibioticos.SelectedItem.Text + " - " + " Microorganismo: " + ddlMicroorganismosAntibioticos.SelectedItem.Text;
                    lblFiltroMicroorganismoATB.Text = oAnti.Nombre;
                    //lblResistenciaAntibiotico.Visible = true;

                    //gvMicroorganismosATB.UpdateAfterCallBack = true;
                    //lblFiltroMicroorganismoATB.UpdateAfterCallBack = true;
                }
                catch (Exception ex)
                {
                    string exception = "";
                    while (ex != null)
                    {
                        exception = ex.Message + "<br>";

                    }
                }
                SetSelectedTab(TabIndex.TWO);

            }
        }

        private string getValoresResistencia()
        {
            string s_valores = "";
            

            for (int i = 0; i < gvMicroorganismosATB.Rows.Count; i++)
            {
                string s_nombre = gvMicroorganismosATB.Rows[i].Cells[0].Text.Replace(";", "");
                s_nombre = s_nombre.Replace("&#", "");
              
                if (s_valores == "")
                    s_valores =s_nombre + ";" + gvMicroorganismosATB.Rows[i].Cells[1].Text + ";"+gvMicroorganismosATB.Rows[i].Cells[2].Text +";"+ gvMicroorganismosATB.Rows[i].Cells[3].Text;
                else
                    s_valores += "=" + s_nombre + ";" + gvMicroorganismosATB.Rows[i].Cells[1].Text + ";" + gvMicroorganismosATB.Rows[i].Cells[2].Text + ";" + gvMicroorganismosATB.Rows[i].Cells[3].Text;
            }
            s_valores = s_valores.Replace("&#", "");
            return s_valores;
        }

        private DataTable getATB(string s_germen)
        {

            string m_strCondicion = " and P.baja=0 ";

          
            m_strCondicion += " and P.idOrigen in (" + getListaOrigen() + ")";
            if (ddlEfector.SelectedValue != "0")
                m_strCondicion += " and P.idEfector=" + ddlEfector.SelectedValue;

            if (ddlTipoMuestra.SelectedValue != "0") m_strCondicion += " and P.idMuestra=" + ddlTipoMuestra.SelectedValue;
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            string                 m_strSQL = @"SELECT Child.* FROM (
SELECT  
  Antibiotico ,sensibilidad
FROM             vta_LAB_Antibiograma A INNER JOIN
                      LAB_Protocolo P ON A.idProtocolo = P.idProtocolo
where A.resultado<>'' and  A.germen like '%" + s_germen +"%' and A.idItem=" + ddlAnalisis.SelectedValue + 
@" AND (P.fecha >= '" + fecha1.ToString("yyyyMMdd") + "') AND (P.fecha <= '" + fecha2.ToString("yyyyMMdd") + 
@"')  and P.idtiposervicio=3 and P.estado=2" + m_strCondicion + //; germen='Escherichia coli' and P.fecha>='20130101'
")  pvt PIVOT (count(sensibilidad) FOR sensibilidad IN ([Resistente],[Sensible],[Intermedio],[No Probado],[Apto para Sinergia],[Sensibilidad Disminuida],[Sin Reactivo]))  AS Child order by antibiotico";
 
                              


            DataSet Ds = new DataSet();
            //      SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            DataTable data = Ds.Tables[0];
            return data;
        }

        protected void gvMicroorganismosATB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              
                for (int i = 1; i <=4; i++) if (e.Row.Cells[i].Text == "0") e.Row.Cells[i].Text = "";
            }
            
        }

        protected void btnBuscarMecanismo_Click(object sender, EventArgs e)
        {
            DataTable dt = MostrarDatos("Mecanismo");
            gvTipoMecanismo.DataSource = dt;
            gvTipoMecanismo.DataBind();            
            gvTipoMecanismo.Visible = true;
            SetSelectedTab(TabIndex.QUINTO);
        }

        protected void imgExcelMecanismo_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcelMecanismo();
        }

        private void ExportarExcelMecanismo()
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();

            gvTipoMecanismo.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gvTipoMecanismo);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Mecanismo.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void imgPacientesMecanismo_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = GetDataPacientes("Mecanismo");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ddlAnalisis.SelectedItem.Text + "_Mecanismo.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}
