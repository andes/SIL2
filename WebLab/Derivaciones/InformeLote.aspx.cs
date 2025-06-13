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
using Business.Data.Laboratorio;
using System.Data.SqlClient;
using Business;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO;
using NHibernate;
using NHibernate.Expression;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class InformeLote : System.Web.UI.Page
    {

        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
        }

      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if (!Page.IsPostBack)
                {
                    int estado = Convert.ToInt32(Request["Estado"]);

                    if (estado == 1 || estado == 3)
                    {
                        activarControles(true);
                    }
                    else
                    {
                        activarControles(false);
                    }

                    CargarGrilla();
                    CargarControles();
                }

            }
            else
            {
                Response.Redirect("../FinSesion.aspx", false);
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


        #region Carga


        private DataTable GetData(string m_strSQL)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

        private void CargarGrilla()
        {
            int estado = Convert.ToInt32(Request["Estado"]);
            string parametros = Request["Parametros"].ToString();

            string m_strSQL = " SELECT idLoteDerivacion as numero, e.nombre as efectorderivacion, l.estado, l.idEfectorDestino as idEfectorDerivacion," +
                             " fechaRegistro, " +
                             " case when (fechaenvio = '1900-01-01 00:00:00.000' ) then null else fechaEnvio end as fechaEnvio, " +
                             "  l.observacion ,uEmi.username as usernameE, isnull(uRecep.username, '' )  as usernameR " +
                             " FROM LAB_LoteDerivacion l " +
                             " inner join Sys_Efector e on e.idEfector=l.idEfectorDestino " +
                             " inner join Sys_Usuario uEmi on uEmi.idUsuario = idUsuarioRegistro " +
                             " left join Sys_Usuario uRecep on uRecep.idUsuario = idUsuarioEnvio " +
                             " where " + parametros + " AND baja = 0  AND estado = " + estado +
                             " ORDER BY l.idEfectorDestino, idLoteDerivacion ";

            DataTable dt = GetData(m_strSQL);

            if (dt.Rows.Count > 0)
            {
                gvLista.DataSource = dt;
            }
            else
            {
                activarControles(false); //desactiva los controles porque no hay nada para derivar o cancelar
            }

            gvLista.DataBind();
            CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";
        }

        //private void desactivarControles()
        //{
        //    btnGuardar.Enabled = false;
        //    txtObservacion.Enabled = false;
        //    ddlEstados.Enabled = false;
        //    //rb_transportista.Enabled = false; //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
        //    ddl_Transporte.Enabled = false;
        //    lnkMarcar.Enabled = false;
        //    lnkDesMarcar.Enabled = false;
        //}

        private void activarControles(bool valor)
        {
            btnGuardar.Enabled = valor;
            txtObservacion.Enabled = valor;
            ddlEstados.Enabled = valor;
            //rb_transportista.Enabled = true; //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
            ddl_Transporte.Enabled = valor;
            lnkMarcar.Enabled = valor;
            lnkDesMarcar.Enabled = valor;
            txt_Fecha.Enabled = valor;
            txt_Hora.Enabled = valor;
        }

        private void CargarControles()
        {
            CargarEstados();
            CargarTransportistas();
            CargarFechaHoraActual();
        }
        private void CargarEstados()
        {  /////////////////Estados de lote /////////////////
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "SELECT idEstado, nombre FROM LAB_LoteDerivacionEstado where baja=0 and idEstado in (2,3) ";
            oUtil.CargarCombo(ddlEstados, m_ssql, "idEstado", "nombre", connReady);

        }


        private void CargarTransportistas()
        {
            ddl_Transporte.Items.Add("-- SELECCIONE --");
            //Vanesa: por ahora esta hardcodeado los transportitas, hacer mejora que lea de la base de datos
            ddl_Transporte.Items.Add("Público");
            ddl_Transporte.Items.Add("Privado");


            //Utility oUtil = new Utility();
            //string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;
            //string m_ssql = "";
            //oUtil.CargarCombo(ddl_Transporte, m_ssql, "idEstado", "nombre", connReady);
        }
        private void CargarFechaHoraActual()
        {
            DateTime miFecha = DateTime.UtcNow.AddHours(-3); //Hora estándar de Argentina	(UTC-03:00)
            txt_Fecha.Text = miFecha.Date.ToString("yyyy-MM-dd");
            txt_Hora.Text = miFecha.ToString("HH:mm");
            //Date1.Text = miFecha.Date.ToString("yyyy-MM-dd");
            //Time1.Text = miFecha.ToString("HH:mm");
        }

        protected string ObtenerImagenEstado(int estado)
        {
            //Estados de Lote
            switch (estado)
            {
                case 1:
                    return "../App_Themes/default/images/reloj-de-arena.png";
                case 2:
                    return "~/App_Themes/default/images/enviado.png";
                case 3:
                    return "../App_Themes/default/images/block.png";
                default:
                    return "";
            }
        }

        protected bool habilitarCheckBoxSegunEstado(int estado)
        {
            switch (estado)
            {
                case 1:
                    return true;
                case 2:
                    return false;
                case 3:
                    return true;
                default:
                    return false;
            }
        }

        protected string habilitarEditarSegunEstado(int estado)
        {
            if (estado == 1)
                return "~/App_Themes/default/images/editar.jpg";
            else
                return "";

        }


        #endregion

        #region Marca
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        protected void lnkDesMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            /* DataTable dt = new DataTable();

             if (Session["ListaSeleccionados"] == null){

                 dt.Columns.Add("numero");
                 dt.Columns.Add("efectorderivacion");
                 dt.Columns.Add("estado");
                 dt.Columns.Add("fechaAlta");
                 dt.Columns.Add("usernameE");
                 dt.Columns.Add("usernameR");
                 dt.Columns.Add("idEfectorDerivacion");

                 cargarRow(sender, dt);
             }
             else
             {
                 dt = (DataTable) Session["ListaSeleccionados"];
                 if (((System.Web.UI.WebControls.CheckBox)sender).Checked){
                     cargarRow(sender, dt);
                 }
                 else{
                     int index = ((GridViewRow)((System.Web.UI.Control)sender).BindingContainer).DataItemIndex;
                     string idLote = gvLista.Rows[index].Cells[2].Text;
                     DataRow[] borrar = dt.Select("numero = " + idLote);

                     if(borrar.Length > 0)
                        dt.Rows.Remove(borrar[0]);
                 }
             }

             Session["ListaSeleccionados"] = dt;*/
        }

        //private void cargarRow(object sender, DataTable dt)
        //{
        //    int index = ((GridViewRow)((System.Web.UI.Control)sender).BindingContainer).DataItemIndex;

        //    string idLote = gvLista.Rows[index].Cells[2].Text;
        //    string efectorderivacion = gvLista.Rows[index].Cells[3].Text;
        //    string usernameE = gvLista.Rows[index].Cells[4].Text;
        //    string usernameR = gvLista.Rows[index].Cells[5].Text;
        //    string fechaAlta = gvLista.Rows[index].Cells[6].Text;
        //    string idEfectorDerivacion = gvLista.Rows[index].Cells[8].Text;
        //    DataRow dr = dt.NewRow();
        //    dr["numero"] = idLote;
        //    dr["efectorderivacion"] = efectorderivacion;
        //    dr["estado"] = Request["Estado"].ToString();
        //    dr["fechaAlta"] = fechaAlta;
        //    dr["usernameE"] = usernameE;
        //    dr["usernameR"] = usernameR;
        //    dr["idEfectorDerivacion"] = idEfectorDerivacion;
        //    dt.Rows.Add(dr);
        //}
        #endregion

        #region PDF



        protected void lnkPDF_Command(object sender, CommandEventArgs e)
        {
            string idLote = (((System.Web.UI.WebControls.LinkButton)sender).CommandArgument).ToString();
            GenerarPDF(idLote);
        }


        private void GenerarPDF(string idLote)
        {

            string m_strSQL = Business.Data.Laboratorio.LoteDerivacion.derivacionPDF(int.Parse(idLote));

            DataTable dt = GetData(m_strSQL);

            if (dt.Rows.Count > 0)
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                string informe = "../Informes/DerivacionLote.rpt";
                Configuracion oCon = new Configuracion();
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;

                oCr.Report.FileName = informe;
                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("Derivaciones");
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
            }
        }


        #endregion

        #region Entrega
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                Guardar();
                CargarGrilla();
                limpiarForm();
            }
            else
            {
                Response.Redirect("../FinSesion.aspx", false);
            }

        }
        //private void GuardarEstadoNuevo()
        //{
        //    foreach (GridViewRow row in gvLista.Rows)
        //    {
        //        CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
        //        if (a.Checked)
        //        {
        //            string idLote = row.Cells[2].Text;
        //            int idUsuario = int.Parse(Session["idUsuario"].ToString());
        //            int estado = Convert.ToInt32(ddlEstados.SelectedValue);
        //            string resultado =  Convert.ToInt32(ddlEstados.SelectedValue) == 1 ? "Derivado: " + row.Cells[3].Text : "No Derivado. ";
        //            string observacion = txtObservacion.Text + " " + ( Convert.ToInt32(ddlEstados.SelectedValue) == 1 ? rb_transportista.SelectedValue : "");

        //            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "LAB_LoteDerivacion_Envio";

        //            cmd.Parameters.Add("@idLote", SqlDbType.Int);
        //            cmd.Parameters["@idLote"].Value = idLote;

        //            cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
        //            cmd.Parameters["@idUsuario"].Value = idUsuario;

        //            cmd.Parameters.Add("@resultado", SqlDbType.VarChar);
        //            cmd.Parameters["@resultado"].Value = resultado;

        //            cmd.Parameters.Add("@estado", SqlDbType.Int);
        //            cmd.Parameters["@estado"].Value = estado;

        //            cmd.Parameters.Add("@observacion", SqlDbType.VarChar);
        //            cmd.Parameters["@observacion"].Value = observacion;

        //            cmd.Connection = conn;
        //            cmd.ExecuteNonQuery();
        //        }
        //    }

        //}

        private void Guardar()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked)
                {

                    int idLote = Convert.ToInt32(row.Cells[2].Text);
                    int idUsuario = int.Parse(Session["idUsuario"].ToString());
                    int estadoLote = Convert.ToInt32(ddlEstados.SelectedValue);
                    string resultadoDerivacion = estadoLote == 2 ? "Derivado: " + row.Cells[3].Text : "No Derivado. ";
                    //string observacion = txtObservacion.Text + " " + (estadoLote == 1 ? rb_transportista.SelectedValue : ""); //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
                    string observacion = txtObservacion.Text + " " + (estadoLote == 1 ? ddl_Transporte.SelectedValue : "");
                    LoteDerivacion lote = new LoteDerivacion();
                    lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), idLote);

                    //Se cambia el estado del lote LAB_LoteDerivacion
                    lote.Estado = estadoLote;
                    lote.Observacion = observacion;
                    lote.IdUsuarioEnvio = idUsuario;
                    //para Estado "Derivado" poner la fecha actual y para estado "Cancelado" no poner Fecha
                    // lote.FechaEnvio = (estadoLote == 2) ? DateTime.Now.ToString() : "";

                    string fecha_hora = txt_Fecha.Text + " " + txt_Hora.Text;
                    lote.FechaEnvio = Convert.ToDateTime(fecha_hora);

                    //Inserta auditoria del lote
                    lote.GrabarAuditoriaLoteDerivacion("Estado: " + lote.descripcionEstadoLote(), idUsuario);
                    lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Observacion", txtObservacion.Text);
                    lote.GrabarAuditoriaLoteDerivacion("Fecha y Hora retiro", idUsuario, "Fecha", txt_Fecha.Text);
                    lote.GrabarAuditoriaLoteDerivacion("Fecha y Hora retiro", idUsuario, "Hora", txt_Hora.Text);

                    if (estadoLote == 2)  //Si deriva indica con que transportista fue
                        //   lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Transportista", rb_transportista.SelectedValue); //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
                        lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Transportista", ddl_Transporte.SelectedValue);

                    lote.Save();

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                    crit.Add(Expression.Eq("Idlote", lote));
                    IList lista = crit.List();
                    if (lista.Count > 0)
                    {

                        foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                        {
                            #region Derivacion 
                            //Cambia el estado de las derivaciones LAB_Derivacion 

                            /*
                             Estado del lote LAB_LoteDerivacionEstado (representa el estado del lote, no de la derivacion)
                             1 : Creado
                             2 : Derivado
                             3 : Cancelado

                             Estado de la derivacion LAB_DerivacionEstado
                             0 : Pendiente de derivar
                             1 : Enviado
                             2 : No Enviado
                             3 : Recibido
                             4 : Pendiente para enviar
                           */
                            oDeriva.Estado = (estadoLote == 2) ? 1 : 2;
                            oDeriva.Save();
                            #endregion


                            #region cambio_codificacion_a_derivacion
                            //Cambia el resultado de LAB_DetalleProtocolo
                            //DetalleProtocolo oDet = new DetalleProtocolo();
                            //oDet = (DetalleProtocolo) oDet.Get(typeof(DetalleProtocolo), oDeriva.IdDetalleProtocolo.IdDetalleProtocolo);
                            //oDet.ResultadoCar = resultadoDerivacion;
                            //oDet.ConResultado = true;
                            //oDet.IdUsuarioResultado = idUsuario;
                            ////oDet.FechaResultado = DateTime.Now;
                            //oDet.FechaResultado = Convert.ToDateTime(fecha_hora);
                            //oDet.Save();
                            ////Inserta auditoria del detalle del protocolo
                            //oDet.GrabarAuditoriaDetalleProtocolo("Graba", idUsuario);
                            #endregion
                        }
                    }

                    //Se cambia el estado del lote LAB_LoteDerivacion
                    lote.Estado = estadoLote;
                    lote.Observacion = observacion;
                    lote.IdUsuarioEnvio = idUsuario;
                    //para Estado "Derivado" poner la fecha actual y para estado "Cancelado" no poner Fecha
                    lote.FechaEnvio = (estadoLote == 2) ? DateTime.Now : DateTime.MinValue;

                    //Inserta auditoria del lote
                    lote.GrabarAuditoriaLoteDerivacion("Estado: " + lote.descripcionEstadoLote(), idUsuario);
                    lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Observacion", txtObservacion.Text);
                    if (estadoLote == 2)  //Si deriva indica con que transportista fue
                        //   lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Transportista", rb_transportista.SelectedValue); //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
                        lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Transportista", ddl_Transporte.SelectedValue);

                }
            }
        }
        #endregion


        private void limpiarForm()
        {
            txtObservacion.Text = string.Empty;
            ddlEstados.SelectedIndex = 0;
        }

        #region Editar
        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("InformeList3.aspx?idLote=" + e.CommandArgument + "&Destino=" + e.CommandName + "&Tipo=Modifica" , false);
        }

        #endregion

      
    }
}
