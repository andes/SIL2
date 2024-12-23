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
            if (!Page.IsPostBack){
                if (Convert.ToInt32(Request["Estado"]) == 3 || Convert.ToInt32(Request["Estado"]) == 2)
                {
                    activarControles();
                }
                CargarGrilla();
                CargarEstados();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null) {
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
            string m_strSQL = " SELECT idLoteDerivacion as numero, e.nombre as efectorderivacion, l.estado, l.idEfectorDestino as idEfectorDerivacion," +
                             " fechaRegistro, " +
                             " case when (fechaenvio = '1900-01-01 00:00:00.000' ) then null else fechaEnvio end as fechaEnvio, " +
                             "  l.observacion ,uEmi.username as usernameE, isnull(uRecep.username, '' )  as usernameR " +
                             " FROM LAB_LoteDerivacion l " +
                             " inner join Sys_Efector e on e.idEfector=l.idEfectorDestino " +
                             " inner join Sys_Usuario uEmi on uEmi.idUsuario = idUsuarioRegistro " +
                             " left join Sys_Usuario uRecep on uRecep.idUsuario = idUsuarioEnvio " +
                             " where " + Request["Parametros"].ToString() + " AND baja = 0  AND estado = " + Request["Estado"].ToString() +
                             " ORDER BY l.idEfectorDestino, idLoteDerivacion ";

            DataTable dt = GetData(m_strSQL);

            if (dt.Rows.Count > 0) {
                gvLista.DataSource = dt;
            }
            else{
                desactivarControles();
            }

            gvLista.DataBind(); 
            CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";
        }

        private void desactivarControles()
        {
            btnGuardar.Enabled = false;
            txtObservacion.Enabled = false;
            ddlEstados.Enabled = false;
            rb_transportista.Enabled = false;
            lnkMarcar.Enabled = false;
            lnkDesMarcar.Enabled = false;
        }

        private void activarControles()
        {
            btnGuardar.Enabled = true;
            txtObservacion.Enabled = true;
            ddlEstados.Enabled = true;
            rb_transportista.Enabled = true;
            lnkMarcar.Enabled = true;
            lnkDesMarcar.Enabled = true;
        }

        private void CargarEstados()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            /////////////////Estados de derivacion/////////////////

            string m_ssql = "SELECT idEstado, descripcion FROM LAB_DerivacionEstado where baja=0 and idEstado in (1,2) ";
            oUtil.CargarCombo(ddlEstados, m_ssql, "idEstado", "descripcion", connReady);
            ddlEstados.Items.Insert(0, new ListItem("--Seleccione--", "0"));
        }

        protected string ObtenerImagenEstado(int estado)
        {
            switch (estado)
            {
                case 1:return "~/App_Themes/default/images/enviado.png";
                case 2:return "../App_Themes/default/images/block.png";
                case 3:return "../App_Themes/default/images/reloj-de-arena.png";
                default: return ""; 
            }
        }

        protected bool cargarSegunEstado(int estado)
        {
           // return estado == 3 ? true :  false;
            switch (estado)
            {
                case 1: return false;
                case 2: return true;
                case 3: return true;
                default: return false;
            }
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
        //protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    int index = Convert.ToInt32(e.CommandArgument);
        //    string idLote= gvLista.Rows[index].Cells[2].Text;
        //    GenerarPDF(idLote);
        //}

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

        protected bool tieneLoteGenerado(int estado)
        {
            bool tiene = false;
            switch (estado){
                case 1: tiene = true; break;
                case 2: tiene = false; break;
                case 3: tiene = true; break;
            }

            return tiene;
        }
        #endregion

        #region Entrega
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarEstadoNuevo();
            CargarGrilla();
            limpiarForm();
        }
        private void GuardarEstadoNuevo()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked)
                {
                    string idLote = row.Cells[2].Text;
                    int idUsuario = int.Parse(Session["idUsuario"].ToString());
                    int estado = Convert.ToInt32(ddlEstados.SelectedValue);
                    string resultado =  Convert.ToInt32(ddlEstados.SelectedValue) == 1 ? "Derivado: " + row.Cells[3].Text : "No Derivado. ";
                    string observacion = txtObservacion.Text + " " + ( Convert.ToInt32(ddlEstados.SelectedValue) == 1 ? rb_transportista.SelectedValue : "");

                    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "LAB_LoteDerivacion_Envio";

                    cmd.Parameters.Add("@idLote", SqlDbType.Int);
                    cmd.Parameters["@idLote"].Value = idLote;

                    cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
                    cmd.Parameters["@idUsuario"].Value = idUsuario;

                    cmd.Parameters.Add("@resultado", SqlDbType.VarChar);
                    cmd.Parameters["@resultado"].Value = resultado;

                    cmd.Parameters.Add("@estado", SqlDbType.Int);
                    cmd.Parameters["@estado"].Value = estado;

                    cmd.Parameters.Add("@observacion", SqlDbType.VarChar);
                    cmd.Parameters["@observacion"].Value = observacion;

                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }

        }
        #endregion

        private bool hayLotesMarcados()
        {
            bool hayMarcado = false;
            int largo = gvLista.Rows.Count, i = 0;
            if (largo > 0)
            {
                GridViewRowCollection rows = gvLista.Rows;
                while (i < largo && !hayMarcado)
                {
                    GridViewRow row = rows[i];
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));

                    if (a.Checked)
                    {
                        hayMarcado = true; //hay al menos un item marcado
                    }
                    i++;
                }

            }
            return hayMarcado;
        }

        private void limpiarForm()
        {
            txtObservacion.Text = string.Empty;
            ddlEstados.SelectedIndex = 0;
        }

        
    }
}
