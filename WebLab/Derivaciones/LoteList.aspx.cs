using Business;
using Business.Data;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebLab.Derivaciones
{
    public partial class LoteList : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oCr.Report.FileName = "";
                oCr.CacheDuration = 0;
                oCr.EnableCaching = false;
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if (!IsPostBack)
                {
                    lblTitulo.Text = "LISTA DE LOTES";
                    habilitaOrdenarEfector();
                    CargarFiltros();
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }
        private void habilitaOrdenarEfector()
        {
            //solo ordeno por efector origen si es subsecretaria
            string expresion = "";
            if (oUser.IdEfector.IdEfector == 227)
                expresion = "efectorOrigen";

            gvLista.Columns[2].SortExpression = expresion;
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        #region Validaciones
        protected void cvValidar_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Session["idUsuario"] != null)
            {
                DateTime fecha1 = new DateTime(), fecha2 = new DateTime();

                try
                {
                    fecha1 = DateTime.Parse(txtFechaDesde.Value);
                    fecha2 = DateTime.Parse(txtFechaHasta.Value);
                }
                catch
                {
                    args.IsValid = false;
                    cvValidar.ErrorMessage = "Fechas inválidas";
                }

                if (txtFechaDesde.Value == "")
                {
                    args.IsValid = false;
                    cvValidar.ErrorMessage = "Fechas de inicio y de fin";
                }
                else
                {
                    if (txtFechaHasta.Value == "")
                    {
                        args.IsValid = false;
                        cvValidar.ErrorMessage = "Fechas de inicio y de fin";
                    }
                    else
                    {
                        if (fecha1.CompareTo(fecha2) <= 0)
                            args.IsValid = true;
                        else
                        {
                            cvValidar.ErrorMessage = "Fechas Desde no puede ser mayor a Fecha Hasta";
                            args.IsValid = false;
                        }
                    }
                }

                if (txtLoteDesde.Text != "" && txtLoteHasta.Text != "" && (Convert.ToInt32(txtLoteDesde.Text) > Convert.ToInt32(txtLoteHasta.Text)))
                {
                    cvValidar.ErrorMessage = "Lote Desde no puede ser mayor a Lote Hasta";
                    args.IsValid = false;
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

        #endregion

        #region Inicializar
        private string consultaEfectorDestino(int efectorOrigen = 0)
        {
            string consulta = @"select distinct E.idEfector, E.nombre  
                    from sys_efector E (nolock) 
                    INNER JOIN LAB_LoteDerivacion l (nolock) on l.idEfectorDestino = E.idEfector 
                    where l.baja=0 ";

            if (efectorOrigen == 0)
                return consulta + " order by E.nombre";
            else
                return consulta + "  and L.idEfectorOrigen= " + efectorOrigen + " ORDER BY E.nombre";
        }
      

        private void CargarFiltros()
        {
            if (Session["idUsuario"] != null)
            {
                Utility oUtil = new Utility();
                string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;
                string msql;

                //Fechas
                txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                //Estados de lotes
                msql = "Select idEstado, nombre  from LAB_LoteDerivacionEstado where baja = 0";
                oUtil.CargarCombo(ddlEstado, msql, "idEstado", "nombre", connReady);
                ddlEstado.Items.Insert(0, new ListItem("--TODOS--", "0"));

                //Efector origen y destino
                if (oUser.IdEfector.IdEfector == 227) //SUBSECRETARIA DE SALUD
                {  //Opción todos solo para nivel de subsecretaria de salud
                    msql = @"select distinct E.idEfector, E.nombre  
                              from sys_efector E (nolock) 
                              INNER JOIN LAB_LoteDerivacion l (nolock) on l.idEfectorOrigen = E.idEfector  
                              where l.baja=0
                            order by E.nombre";

                    oUtil.CargarCombo(ddlEfectorOrigen, msql, "idEfector", "nombre", connReady);
                    ddlEfectorOrigen.Items.Insert(0, new ListItem("--TODOS--", "0"));

                    msql = consultaEfectorDestino();
                    oUtil.CargarCombo(ddlEfectorDestino, msql, "idEfector", "nombre", connReady);
                    ddlEfectorDestino.Items.Insert(0, new ListItem("--TODOS--", "0"));
                }
                else
                {   //ORIGEN: Si es efector no subsecretaria de salud solo el efector del usuario logueado.
                    msql = "select  E.idEfector, E.nombre  from sys_efector E (nolock)  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                    oUtil.CargarCombo(ddlEfectorOrigen, msql, "idEfector", "nombre", connReady);
                    //DESTINO: Si es efector no subsecretaria de salud solo los efectores a los que el efector origen puede derivar

                    msql = consultaEfectorDestino(oUser.IdEfector.IdEfector);

                    oUtil.CargarCombo(ddlEfectorDestino, msql, "idEfector", "nombre", connReady);
                    ddlEfectorDestino.Items.Insert(0, new ListItem("--TODOS--", "0"));
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }
        private void CargarGrilla()
        {
            if (Session["idUsuario"] != null)
            {
                gvLista.DataSource = GenerarGrilla();
                gvLista.DataBind();
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        private DataTable GenerarGrilla()
        {
            string str_condicion = " L.baja = 0 ";

            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                str_condicion += " AND l.fechaRegistro>= '" + fecha1.ToString("yyyyMMdd") + "'";
            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                str_condicion += " AND l.fechaRegistro<= '" + fecha2.ToString("yyyyMMdd") + "'";
            }

            if (txtLoteDesde.Text != "")
                str_condicion += " AND L.idLoteDerivacion >= " + int.Parse(txtLoteDesde.Text);
            if (txtLoteHasta.Text != "")
                str_condicion += " AND L.idLoteDerivacion <= " + int.Parse(txtLoteHasta.Text);

            if (ddlEfectorOrigen.SelectedValue != "0")
                str_condicion += " AND L.idEfectorOrigen = " + ddlEfectorOrigen.SelectedValue;

            if (ddlEfectorDestino.SelectedValue != "0")
                str_condicion += " AND L.idEfectorDestino = " + ddlEfectorDestino.SelectedValue;

            if (ddlEstado.SelectedValue != "0")
                str_condicion += " AND L.estado = " + ddlEstado.SelectedValue;

            string str_sql =  @"select idLoteDerivacion as numero, fechaRegistro,  efOrigen.nombre as efectorOrigen, efOrigen.idEfector as idEfectorOrigen,
                                 efDestino.nombre as efectorDestino, E.nombre AS estado, isnull(U.apellido, 'Automatico') as username
                                 from LAB_LoteDerivacion L (nolock)
                                                INNER JOIN LAB_LoteDerivacionEstado E(nolock) ON E.idEstado = L.estado
                                                INNER JOIN Sys_Efector efOrigen (nolock) ON efOrigen.idEfector = L.idEfectorOrigen
                                                INNER JOIN Sys_Efector efDestino (nolock) ON efDestino.idEfector = L.idEfectorDestino
                                                INNER JOIN Sys_Usuario U (nolock) ON U.idUsuario = L.idUsuarioRegistro
                                                where " + str_condicion;
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(str_sql, conn);
            adapter.Fill(Ds);

            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            ViewState["Datos"] = Ds.Tables[0];
            return Ds.Tables[0];
        }

        protected void ddlEfectorOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEfectorDestino.Items.Clear();
            int efectorOrigen = Convert.ToInt32(ddlEfectorOrigen.SelectedValue);
            string msql;
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;
            Utility oUtil = new Utility();
            if (efectorOrigen != 0)
            {
                msql = consultaEfectorDestino(efectorOrigen);
                oUtil.CargarCombo(ddlEfectorDestino, msql, "idEfector", "nombre", connReady);
                ddlEfectorDestino.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }
            else
            {
                msql = consultaEfectorDestino();
                oUtil.CargarCombo(ddlEfectorDestino, msql, "idEfector", "nombre", connReady);
                ddlEfectorDestino.Items.Insert(0, new ListItem("--TODOS--", "0"));
            }

        }
        #endregion

        #region Buscar
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                gvLista.DataSource = null;

                if (Page.IsValid)
                    CargarGrilla();
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        #endregion

        #region Impresiones
        protected void lnkPDFAuditoria_Command(object sender, CommandEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                int idLote = Convert.ToInt32(((System.Web.UI.WebControls.LinkButton)sender).CommandArgument);
                string m_strSQL, m_strCondicion = "";

                if (!oUser.Administrador)
                {
                    m_strCondicion = " and L.idEfectorDestino = " + oUser.IdEfector.IdEfector.ToString();
                }

                m_strSQL = @" SELECT  L.idLoteDerivacion  AS numero,isnull(U.apellido,'Automatico')  as username, A.fecha AS fecha, A.hora, A.accion, A.analisis, A.valor, A.valorAnterior
                            FROM LAB_AuditoriaLote AS A with (nolock)
                            left JOIN Sys_Usuario AS U with (nolock) ON A.idUsuario = U.idUsuario
                            inner join  LAB_LoteDerivacion L  with (nolock) on L.idLoteDerivacion= A.idLote
                            where  L.idLoteDerivacion = " + idLote + m_strCondicion + " ORDER BY A.idAuditoriaLote";

                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet Ds1 = new DataSet();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds1, "auditoria");
                DataTable data = Ds1.Tables[0];

                if (data.Rows.Count > 0)
                {
                    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();

                    if (oUser.IdEfector.IdEfector == 227)
                    {
                        //tengo que cargar la configuracion del efector Origen
                        int efectorOrigen = Convert.ToInt32(((LinkButton)sender).CommandName);
                        oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", efectorOrigen);
                    }


                    if (oC != null)
                    {
                        encabezado1.Value = oC.EncabezadoLinea1;
                        encabezado2.Value = oC.EncabezadoLinea2;
                    }
                    else
                    {
                        encabezado1.Value = oUser.IdEfector.Nombre;
                        encabezado2.Value = oUser.IdEfector.Domicilio;
                    }
                    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue
                    {
                        Value = "Auditoria de Lote"
                    };

                    oCr.Report.FileName = "../Informes/AuditoriaLote.rpt";
                    oCr.ReportDocument.SetDataSource(data);
                    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                    oCr.DataBind();

                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF("Auditoria_Lote_" + idLote);
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                }
                else
                {
                    string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de lote ingresado.'); </script>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "PopupScript", popupScript, true);
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        protected void lnkPDFImprimir_Command(object sender, CommandEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                int idLote = Convert.ToInt32((((System.Web.UI.WebControls.LinkButton)sender).CommandArgument));
                string m_strSQL = Business.Data.Laboratorio.LoteDerivacion.derivacionPDF(idLote);

                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string informe = "../Informes/DerivacionLote.rpt";

                    if (oUser.IdEfector.IdEfector == 227)
                    {
                        //tengo que cargar la configuracion del efector Origen
                        int efectorOrigen = Convert.ToInt32(((LinkButton)sender).CommandName);
                        oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", efectorOrigen);
                    }

                    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = oC.EncabezadoLinea1;
                    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = oC.EncabezadoLinea2;
                    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = oC.EncabezadoLinea3;

                    oCr.Report.FileName = informe;
                    oCr.ReportDocument.SetDataSource(Ds.Tables[0]);
                    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                    oCr.DataBind();

                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF("Derivaciones_" + idLote);
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
        }

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if (Page.IsValid)
                {
                    if (gvLista.Rows.Count != 0)
                    {
                        DataTable tabla = ViewState["Datos"] as DataTable; 
                       
                        if (tabla.Rows.Count > 0)
                        {
                            tabla.Columns.Remove("idEfectorOrigen");
                            tabla.Columns["numero"].ColumnName = "Nro.";
                            tabla.Columns["fechaRegistro"].ColumnName = "Fecha Generación";
                            tabla.Columns["efectorOrigen"].ColumnName = "Efector Origen";
                            tabla.Columns["efectorDestino"].ColumnName = "Efector Destino";
                            tabla.Columns["estado"].ColumnName = "Estado";
                            tabla.Columns["username"].ColumnName = "Usuario Generación";

                            StringBuilder sb = new StringBuilder();
                            StringWriter sw = new StringWriter(sb);
                            HtmlTextWriter htw = new HtmlTextWriter(sw);
                            HtmlForm form = new HtmlForm();
                            GridView dg = new GridView();
                            dg.EnableViewState = false;
                            dg.DataSource = tabla;
                            dg.DataBind();
                            Page pagina = new Page();
                            pagina.EnableEventValidation = false;
                            pagina.DesignerInitialize();
                            pagina.Controls.Add(form);
                            form.Controls.Add(dg);
                            pagina.RenderControl(htw);
                            Utility oUtil = new Utility();
                            string nombrXLS = oUtil.CompletarNombrePDF("Lotes");
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = "application/vnd.ms-excel";
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + nombrXLS + ".xls");
                            Response.Charset = "UTF-8";
                            Response.ContentEncoding = Encoding.Default;
                            Response.Write(sb.ToString());
                            Response.End();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "PopupScript3", "alert('No hay datos para exportar.');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "PopupScript2", "alert('No hay datos para exportar.');", true);
                    }

                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        #endregion

        #region gvLista
        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                gvLista.PageIndex = e.NewPageIndex;
                int currentPage = gvLista.PageIndex + 1;
                CurrentPageLabel.Text = "Página " + currentPage.ToString() + " de " + gvLista.PageCount.ToString();
                CargarGrilla();
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        protected void gvLista_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                DataTable dt = ViewState["Datos"] as DataTable;
                string sortDirection = GetSortDirection(e.SortExpression);
                dt.DefaultView.Sort = e.SortExpression + " " + sortDirection;
                gvLista.DataSource = dt;
                gvLista.DataBind();
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                        sortDirection = "DESC";
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;
            return sortDirection;
        }



        #endregion


    }
}