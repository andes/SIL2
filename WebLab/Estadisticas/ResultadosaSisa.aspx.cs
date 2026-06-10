 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.Configuration;
using Business.Data;
using System.Web.UI.HtmlControls;

namespace WebLab.Estadisticas
{
    public partial class ResultadosaSisa : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {


            if (Session["idUsuario"] == null)
                Response.Redirect("../logout.aspx", false);
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
                    Response.Redirect("../logout.aspx", false);
                else
                {
                    VerificaPermisos("Resultados a SISA");
                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    lblMensaje.Visible = false;
                    CargarListas();                 
                }
            }


        }


        private void VerificaPermisos(string sObjeto)
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



        private void CargarListas()
        {
            Utility oUtil = new Utility(); string m_ssql = ""; string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            if (oUser.IdEfector.IdEfector.ToString() == "227")
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E " +
                     " INNER JOIN lab_Configuracion C on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));

            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }



            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            m_ssql = @"SELECT distinct i.idItem, i.nombre FROM LAB_item i (nolock)
            inner join LAB_ConfiguracionSISADetalle c (nolock) on c.idItem = i.idItem
            order by i.nombre ";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("Todos", "0"));

            /*   m_ssql = @"SELECT distinct c.idEvento, c.nombreEvento
                       FROM   LAB_ConfiguracionSISA c with (nolock)  
                       order by c.nombreEvento";

               oUtil.CargarCombo(ddlItem, m_ssql, "idEvento", "nombreEvento", connReady);
               ddlItem.Items.Insert(0, new ListItem("Seleccione", "0"));
               */
            m_ssql = null;
            oUtil = null;
        }


        protected void btnDescargarExcelControl_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }



        private void ExportarExcel()
        {
            Utility.ExportGridViewToExcel(gvEstadistica, "Enviados_por_Dia");

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            CargarEstadistica();
        }

        private void CargarEstadistica()
        {
            DataSet Ds = new DataSet();

            SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString);

            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "LAB_EstadisticaEnviosSISA";

            cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value =
                DateTime.Parse(txtFechaDesde.Value);

            cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value =
                DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@idEfector", SqlDbType.Int).Value =
                int.Parse(ddlEfector.SelectedValue);

            cmd.Parameters.Add("@idItem", SqlDbType.Int).Value =
                int.Parse(ddlItem.SelectedValue);

            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);

            gvEstadistica.DataSource = Ds.Tables[0];
            gvEstadistica.DataBind();
            gvEstadistica.Columns[1].Visible = (ddlEfector.SelectedValue == "0");
        }

        protected void btnDescargarDetalle_Click(object sender, EventArgs e)
        {
            ExportarDetalleExcel();
        }

        private DataTable MostrarDetalle()
        {
            DataSet Ds = new DataSet();

            SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString);

            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "LAB_EstadisticaEnviosSISADetalle";

            cmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value =
                DateTime.Parse(txtFechaDesde.Value);

            cmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value =
                DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@idEfector", SqlDbType.Int).Value =
                int.Parse(ddlEfector.SelectedValue);

            cmd.Parameters.Add("@idItem", SqlDbType.Int).Value =
                int.Parse(ddlItem.SelectedValue);

            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);

            return Ds.Tables[0];
        }

        private void ExportarDetalleExcel()
        {
            ///   Utility.ExportGridViewToExcel(gvTipoMuestra, ddlAnalisis.SelectedItem.Text + "_TipoMuestra");
            /// 

            DataTable tabla = MostrarDetalle();
            Utility.ExportDataTableToXlsx(tabla, "DetalleEnviadosResultados");
         

        }

        int totalEnviados = 0;
        int totalPacientes = 0;
        int totalProtocolos = 0;

        protected void gvEstadistica_RowDataBound(
object sender,
GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totalEnviados += Convert.ToInt32(
                    DataBinder.Eval(e.Row.DataItem, "cantidadEnviados"));

                totalPacientes += Convert.ToInt32(
                    DataBinder.Eval(e.Row.DataItem, "pacientesUnicos"));

                totalProtocolos += Convert.ToInt32(
                    DataBinder.Eval(e.Row.DataItem, "protocolos"));
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL";

                e.Row.Cells[0].Font.Bold = true;

                e.Row.Cells[4].Text = totalEnviados.ToString("N0");

                e.Row.Cells[5].Text = totalPacientes.ToString("N0");

                e.Row.Cells[6].Text = totalProtocolos.ToString("N0");

                e.Row.Cells[4].Font.Bold = true;
                e.Row.Cells[5].Font.Bold = true;
                e.Row.Cells[6].Font.Bold = true;
            }

        }

        protected void cvValidaDatos_ServerValidate(object source, ServerValidateEventArgs args)
        {


            ///Validacion de la fecha de protocolo
            if (txtFechaDesde.Value == "")
            {
                // TxtDatos.Value = "";
                args.IsValid = false;
                this.cvValidaDatos.ErrorMessage = "Debe ingresar la fecha desde";
                return;
            }
            else
            {

                if (DateTime.Parse(txtFechaDesde.Value) > DateTime.Now)
                {
                    //TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidaDatos.ErrorMessage = "La fecha desde no puede ser superior a la fecha actual";
                    return;
                }
                else
                    args.IsValid = true;
            }





            ///Validacion de la fecha hasta

            if (txtFechaHasta.Value == "")
            {
                // TxtDatos.Value = "";
                args.IsValid = false;
                this.cvValidaDatos.ErrorMessage = "Debe ingresar la fecha hasta";
                return;
            }
            else
            {
                if (DateTime.Parse(txtFechaHasta.Value) > DateTime.Now)
                {
                    // TxtDatos.Value = "";
                    args.IsValid = false;
                    this.cvValidaDatos.ErrorMessage = "La fecha hasta no puede ser superior a la fecha actual";
                    return;
                }
                else
                {
                    if (DateTime.Parse(txtFechaDesde.Value) > DateTime.Parse(txtFechaHasta.Value))
                    {
                        //TxtDatos.Value = "";
                        args.IsValid = false;
                        this.cvValidaDatos.ErrorMessage = "La fecha desde no puede ser superior a la fecha hasta";
                        return;
                    }
                    else
                        args.IsValid = true;
                }
            }

        }
    }
    
}