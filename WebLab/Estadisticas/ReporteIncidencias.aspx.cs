using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;
using System.Collections;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using InfoSoftGlobal;
using Business.Data.Laboratorio;
using Business.Data;
using System.Configuration;

namespace WebLab.Estadisticas
{
    public partial class ReporteIncidencias : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        //public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();

        int suma1 = 0; int suma2 = 0;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //oCr.CacheDuration = 0;
            //oCr.EnableCaching = false;
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
                    VerificaPermisos("De Incidencias");
                    CargarListas();
                    txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    pnlIncidencia.Visible = false; pnlMensaje.Visible = false;
                }
             //   Cargargrilla();
            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "";

            if (oUser.IdEfector.IdEfector == 227) // puede elegir el efector que quiere ver o todos
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E " +
                     " INNER JOIN lab_Configuracion C on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
                ddlEfector.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            }

           
            m_ssql = null;
            oUtil = null;
        }

        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

      

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    //case 1: btnNuevo.Visible = false; break;
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }



        private void Cargargrilla()
        {
            DataTable dt1 = LeerIncidencias("0");
            
            GridView1.DataSource = dt1; 
            GridView1.DataBind();

            if (dt1.Rows.Count > 0)
            {
                //Literal1.Text = mostrarGrafico(dt1, "", "grafico1");
             //  Literal2.Text = mostrarGrafico1(dt1, "Muestras recibidas por efector");
            }
            DataTable dt2 = LeerIncidencias("1");
            gvProtocolos.DataSource = dt2;
            gvProtocolos.DataBind();
            if (dt2.Rows.Count > 0)
            {
                //Literal2.Text = mostrarGrafico(dt2, "", "grafico2");
                //  Literal2.Text = mostrarGrafico1(dt1, "Muestras recibidas por efector");
            }
            if ((GridView1.Rows.Count > 0) || (gvProtocolos.Rows.Count > 0))
            {
                pnlIncidencia.Visible = true;
                pnlMensaje.Visible = false;
            }
            else
            {
                pnlIncidencia.Visible = false;
                pnlMensaje.Visible = true;
            }
        }
        private string mostrarGrafico(DataTable dt1, string s_titulo, string id)
        {
            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf"; 
            string strXML = "<graph caption='" + s_titulo + "' subCaption='' showPercentageInLabel='1' pieSliceDepth='20'  decimalPrecision='0' showNames='1'>";
            for (int i = 0; i < dt1.Rows.Count; i++)
                strXML += "<set name='" + dt1.Rows[i][0].ToString().Replace("\r\n", "") + "' value='" + dt1.Rows[i][1].ToString() + "' />";//.Substring(2, 4)
            strXML += "</graph>";
            return FusionCharts.RenderChart(s_tipografico, "2", strXML, id, "500", "300", false, false);
        }     
        private DataTable LeerIncidencias(string s_tipo)
        {
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "LAB_EstadisticasIncidencias";

            cmd.Parameters.Add("@tipo", SqlDbType.Int);
            cmd.Parameters["@tipo"].Value = s_tipo;

            DateTime fecha1 = DateTime.Parse("01/01/1900"); DateTime fecha2 = DateTime.Now;

            if (txtFechaDesde.Value != "")
                fecha1 = DateTime.Parse(txtFechaDesde.Value);
            if (txtFechaHasta.Value != "")
                fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.AddDays(1).ToString("yyyyMMdd");

            cmd.Parameters.Add("@idEfectorProtocolo", SqlDbType.Int);
            cmd.Parameters["@idEfectorProtocolo"].Value = int.Parse(ddlEfector.SelectedValue);


            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            return Ds.Tables[0];

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Cargargrilla();
        }

        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel("PreRecepcion");
        }

        private void ExportarExcel(string nombreArchivo)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView1.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(GridView1);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreArchivo + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void imgExcel0_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel2("deProtocolos");
        }

        private void ExportarExcel2(string p)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            gvProtocolos.EnableViewState = false;

            // Deshabilitar la validación de eventos, sólo asp.net 2
            page.EnableEventValidation = false;

            // Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            page.DesignerInitialize();
            page.Controls.Add(form);
            form.Controls.Add(gvProtocolos);
            page.RenderControl(htw);

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + p + ".xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") suma1 += int.Parse(e.Row.Cells[1].Text);
                

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma1.ToString();
                
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[1].Text != "&nbsp;") suma2 += int.Parse(e.Row.Cells[1].Text);

              
                    //ImageButton CmdExcel = (ImageButton)e.Row.Cells[2].Controls[1];
                    //CmdExcel.CommandArgument =  this.GridView2.DataKeys[e.Row.RowIndex].Value.ToString();
                    //CmdExcel.CommandName = "EXCEL";
                    //CmdExcel.ToolTip = "Ver Pacientes";







                
//select   dbo.NumeroProtocolo(P.idprotocolo), P.fecha, Pac.apellido, Pac.nombre,
//O.nombre as origen, S.nombre as sector, M.nombre as muestra, U.apellido+ ' ' + U.nombre as usuario,
//medico.solicitante as solicitante, E.nombre as efector
//from		 LAB_protocolo as P
//inner join LAB_ProtocoloIncidenciaCalidad as I on I.idProtocolo= P.idprotocolo
//INNER JOIN   dbo.Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente

//left join LAB_muestra as M on M.idmuestra= P.idmuestra
//   INNER JOIN Lab_Origen O on O.idOrigen= P.idOrigen
//   INNER JOIN LAB_SectorServicio S on S.idSectorServicio= P.idSector
//   inner join Sys_Usuario as U on U.idUsuario =P.idUsuarioRegistro
//   inner join vta_LAB_SolicitanteProtocolo as Medico on Medico.idProtocolo= P.idProtocolo
//   inner join Sys_Efector as E on E.idEfector= P.idEfectorSolicitante
//   where I.idIncidenciaCalidad=10
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL CASOS";
                e.Row.Cells[1].Text = suma2.ToString();

            }
        }

        protected void btnDetalleProtocolos_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();
            GridView dg = new GridView();
            dg.EnableViewState = false;
            dg.DataSource = LeerIncidencias("2");
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
            Response.AddHeader("Content-Disposition", "attachment;filename=ProtocolosIncidencias.xls");
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Response.Write(sb.ToString());
            Response.End();
        }
    }
}