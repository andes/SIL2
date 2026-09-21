using Business;
using Business.Data;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
                    VerificaPermisos("Lista de Lotes");
                    Inicializar();
                    CargarListas();
                    RecuperarSesion();
                    CargarGrilla();
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

        private void Inicializar()
        {
            lblTitulo.Text = "LISTA DE LOTES";

            //solo ordeno por efector origen si es subsecretaria
            string expresion = "";
            if (oUser.IdEfector.IdEfector == 227)
                expresion = "efectorOrigen";

            gvLista.Columns[2].SortExpression = expresion;
        }

        private void RecuperarSesion() { 
            if(Request["Parametros"] != null)
            {
                string str_condicion = Request["Parametros"].ToString();
                if(str_condicion.Contains("AND l.fechaRegistro>= "))
                { 
                    string fecha = ObtenerParametro("AND l.fechaRegistro>= ", str_condicion);
                    txtFechaDesde.Value =  DateTime.ParseExact( fecha, "yyyyMMdd", CultureInfo.InvariantCulture ).ToShortDateString();
                }

                if (str_condicion.Contains("l.fechaRegistro<="))
                {
                    string fecha = ObtenerParametro(" AND l.fechaRegistro<= '", str_condicion);
                    txtFechaHasta.Value = DateTime.ParseExact(fecha, "yyyyMMdd", CultureInfo.InvariantCulture).ToShortDateString();

                }
                if (str_condicion.Contains("AND L.idLoteDerivacion >="))
                    txtLoteDesde.Text = ObtenerParametro(" AND L.idLoteDerivacion >= ",str_condicion);

                if (str_condicion.Contains("AND L.idLoteDerivacion <= "))
                    txtLoteHasta.Text = ObtenerParametro("AND L.idLoteDerivacion <= ", str_condicion);

                if (str_condicion.Contains(" AND L.idEfectorOrigen = "))
                    ddlEfectorOrigen.SelectedValue = ObtenerParametro(" AND L.idEfectorOrigen = ", str_condicion);

                if (str_condicion.Contains(" AND L.idEfectorDestino = "))
                    ddlEfectorDestino.SelectedValue  = ObtenerParametro(" AND L.idEfectorDestino = ", str_condicion);


                if (str_condicion.Contains(" AND L.estado  IN ("))
                {
                    string condicion = " AND L.estado  IN (";
                    int inicio = str_condicion.IndexOf(condicion) + condicion.Length;
                    int fin = str_condicion.IndexOf(")", inicio);

                    string idEstados = str_condicion.Substring(inicio, fin - inicio).Trim();

                    string[] estados = idEstados.Split(',');

                    for (int i = 0; i < chkEstados.Items.Count; i++)
                    {
                        chkEstados.Items[i].Selected = false;

                        for (int j = 0; j < estados.Length; j++)
                        {
                            if (chkEstados.Items[i].Value == estados[j].Trim())
                            {
                                chkEstados.Items[i].Selected = true;
                                break;
                            }
                        }
                    }
                }

            }

        }

        private string ObtenerParametro(string condicion, string str_condicion)
        {
            int inicio = str_condicion.IndexOf(condicion);

            if (inicio < 0)  return "";

            inicio += condicion.Length;

            int fin = str_condicion.IndexOf("AND", inicio);

            if (fin < 0) fin = str_condicion.Length;

            return str_condicion.Substring(inicio, fin - inicio)
                                 .Trim()
                                 .Trim('\'');
        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (Permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;

                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
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


        private void CargarListas()
        {
            if (Session["idUsuario"] != null)
            {
                Utility oUtil = new Utility();
                string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;
                string msql;

                //Fechas
                txtFechaDesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                //Estados de lotes
                msql = "Select idEstado, nombre  from LAB_LoteDerivacionEstado where baja = 0";
                oUtil.CargarCheckBox(chkEstados, msql, "idEstado", "nombre", connReady);
                chkEstados.Items.Insert(0, new ListItem("TODOS", "0"));
                chkEstados.SelectedIndex = 1;

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

        private string Parametros()
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
                fecha2 = fecha2.AddDays(1);
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


            if (!chkEstados.Items[0].Selected)
            {
                string idEstados = "";
                for (int i = 0; i < chkEstados.Items.Count; i++)
                {
                    if (chkEstados.Items[i].Selected)
                    {
                        if(idEstados == "")
                            idEstados += chkEstados.Items[i].Value;
                        else
                            idEstados += ","+ chkEstados.Items[i].Value;
                    }
                }

                str_condicion += " AND L.estado  IN ( " + idEstados + " )";
            } 

            return str_condicion;
        }
        private DataTable GenerarGrilla()
        {
            string str_condicion = Parametros();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_ListaLotes]";
            cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
            cmd.Parameters.Add("@orden", SqlDbType.NVarChar);
            cmd.Parameters["@FiltroBusqueda"].Value = str_condicion;
            cmd.Parameters["@orden"].Value = ddlOrden.SelectedValue;
            cmd.Connection = conn;


            adapter = new SqlDataAdapter(cmd);
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
        private void PDFAuditoria(int idLote, int efectorOrigen)
        {
            if (Session["idUsuario"] != null)
            {
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
                        Efector ef = new Efector();
                        ef = (Efector)ef.Get(typeof(Efector), "IdEfector", efectorOrigen);
                        oC = new Configuracion();
                        oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", ef);
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
        private void PDFControl(int idLote, int efectorOrigen)
        {
            if (Session["idUsuario"] != null)
            {
                string m_strSQL = LoteDerivacion.derivacionPDF(idLote);

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
                        Efector ef = new Efector();
                        ef = (Efector)ef.Get(typeof(Efector), "IdEfector", efectorOrigen);
                        oC = new Configuracion();
                        oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", ef);
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "mensajeOk", "alert('No se encontraron datos para el numero de lote ingresado');", true);

            }
            else  Response.Redirect("../FinSesion.aspx", false);
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
                            tabla.Columns["fechaRegistro"].ColumnName = "Fecha";
                            tabla.Columns["efectorOrigen"].ColumnName = "Efector Origen";
                            tabla.Columns["efectorDestino"].ColumnName = "Efector Destino";
                            tabla.Columns["estado"].ColumnName = "Estado";
                            tabla.Columns["username"].ColumnName = "Usuario Gen.";
                            tabla.Columns["fechaGeneracion"].ColumnName = "Fecha Gen.";
                            tabla.Columns["fechaIngreso"].ColumnName = "Fecha Ing.";
                            tabla.Columns["fechaEnvio"].ColumnName = "Fecha Envio";

                            Utility oUtil = new Utility();
                            Utility.ExportDataTableToXlsx(tabla, oUtil.CompletarNombrePDF("Lotes"));
                      
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

        //protected void gvLista_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    if (Session["idUsuario"] != null)
        //    {
        //        DataTable dt = ViewState["Datos"] as DataTable;
        //        string sortDirection = GetSortDirection(e.SortExpression);
        //        dt.DefaultView.Sort = e.SortExpression + " " + sortDirection;
        //        gvLista.DataSource = dt;
        //        gvLista.DataBind();
        //    }
        //    else
        //        Response.Redirect("../FinSesion.aspx", false);
        //}
        //private string GetSortDirection(string column)
        //{
        //    string sortDirection = "ASC";
        //    string sortExpression = ViewState["SortExpression"] as string;

        //    if (sortExpression != null)
        //    {
        //        if (sortExpression == column)
        //        {
        //            string lastDirection = ViewState["SortDirection"] as string;
        //            if ((lastDirection != null) && (lastDirection == "ASC"))
        //                sortDirection = "DESC";
        //        }
        //    }

        //    ViewState["SortDirection"] = sortDirection;
        //    ViewState["SortExpression"] = column;
        //    return sortDirection;
        //}

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton CmdModificar = (LinkButton)e.Row.Cells[10].Controls[1];
                CmdModificar.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                
                LinkButton CmdCambiarEstado = (LinkButton)e.Row.Cells[11].Controls[1];
                CmdCambiarEstado.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdCambiarEstado.CommandName = "CambiarEstado";


                LinkButton CmdAuditoria = (LinkButton)e.Row.Cells[12].Controls[1];
                CmdAuditoria.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdAuditoria.CommandName = "Auditoria";

                LinkButton CmdPDFControl = (LinkButton)e.Row.Cells[13].Controls[1];
                CmdPDFControl.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdPDFControl.CommandName = "PDFControl";

                string estado = e.Row.Cells[5].Text;
                LoteDerivacionEstado oEstado = (LoteDerivacionEstado) new LoteDerivacionEstado().Get(typeof(LoteDerivacionEstado), "Nombre", estado);
                if (oEstado.IdEstado == 1 || oEstado.IdEstado == 3)
                     CmdCambiarEstado.Visible = true; 
                else
                     CmdCambiarEstado.Visible = false; 

                if (oEstado.IdEstado == 1 || oEstado.IdEstado == 3)
                    CmdPDFControl.Visible = false;

            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string str_condicion = Parametros();

            GridViewRow fila = (GridViewRow)((Control)e.CommandSource).NamingContainer;
            int idEfectorOrigen = Convert.ToInt32(gvLista.Rows[fila.RowIndex].Cells[3].Text);

            switch (e.CommandName)
            {
                case "Modificar":
                        Response.Redirect("InformeList4.aspx?idLote=" + e.CommandArgument + "&Destino=" + idEfectorOrigen + "&Tipo=Modifica&Parametros=" + str_condicion, false); 

                    break;
                case "CambiarEstado":
                    {
                        string script = "CambiarEstado('" + e.CommandArgument + "' , '"+ idEfectorOrigen + "');";

                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "CambiarEstado",
                            script,
                            true);
                    }
                     break;
                case "Auditoria":
                        PDFAuditoria(int.Parse(e.CommandArgument.ToString()), idEfectorOrigen);
                    break;
                case "PDFControl":
                        PDFControl(int.Parse(e.CommandArgument.ToString()), idEfectorOrigen);
                     break;
               
            }
        }


        #endregion


        protected void chkEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkEstados.Items[0].Selected)
            {
                for (int i = 0; i < chkEstados.Items.Count; i++)
                {
                    chkEstados.Items[i].Selected = true;
                }
                chkEstados.Items[0].Selected = false;
            }
            else
            {
                chkEstados.Items[0].Selected = false;
            }
        }

       
    }
}