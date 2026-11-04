
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Configuration;
using NHibernate;
using Business;

namespace WebLab.Resultados
{
    public partial class ProtocoloTiempos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int idProtocolo;

                if (!int.TryParse(Request.QueryString["idProtocolo"], out idProtocolo)
                    || idProtocolo <= 0)
                {
                    MostrarError("El protocolo indicado no es válido.");
                    return;
                }

                CargarTiemposProtocolo(idProtocolo);
            }
        }

        private void CargarTiemposProtocolo(int idProtocolo)
        {
            try
            {
                SqlConnection conn =
                    (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                DateTime fechaIngreso;
                DateTime? fechaFin;
                int estado;

                if (!CargarDatosGenerales(
                    conn,
                    idProtocolo,
                    out fechaIngreso,
                    out fechaFin,
                    out estado))
                {
                    MostrarError("No se encontró el protocolo.");
                    return;
                }

                lblEstado.Text = estado == 2
                    ? "Terminado"
                    : "En proceso";

                lblFechaIngreso.Text =
                    fechaIngreso.ToString("dd/MM/yyyy HH:mm");

                if (fechaFin.HasValue)
                {
                    lblFechaFin.Text =
                        fechaFin.Value.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    lblFechaFin.Text = estado == 2
                        ? "Sin fecha de cierre registrada"
                        : "Pendiente";
                }

                DateTime fechaHasta = fechaFin.HasValue
                    ? fechaFin.Value
                    : DateTime.Now;

                lblTiempoTotal.Text =
                    FormatearTiempo(fechaIngreso, fechaHasta);

                CargarAreas(conn, idProtocolo, fechaIngreso);
            }
            catch (Exception ex)
            {
                MostrarError(
                    "No fue posible consultar los tiempos del protocolo. " +
                    Server.HtmlEncode(ex.Message));
            }
        }

        private bool CargarDatosGenerales(
            SqlConnection conn,
            int idProtocolo,
            out DateTime fechaIngreso,
            out DateTime? fechaFin,
            out int estado)
        {
            fechaIngreso = DateTime.MinValue;
            fechaFin = null;
            estado = 0;

            string sql = @"
                SELECT
                    P.numero,
                    P.fechaRegistro AS fechaIngreso, P.fechaFinalizacion,
                    P.estado
                FROM dbo.LAB_Protocolo P with (nolock)
                WHERE P.idProtocolo = @idProtocolo";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@idProtocolo", SqlDbType.Int)
                    .Value = idProtocolo;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return false;

                    lblNumero.Text =
                        Server.HtmlEncode(Convert.ToString(dr["numero"]));

                    fechaIngreso =
                        Convert.ToDateTime(dr["fechaIngreso"]);

                    DateTime fechaFinalizacion =
      Convert.ToDateTime(dr["fechaFinalizacion"]);

                    fechaFin = fechaFinalizacion > new DateTime(1900, 1, 1)
                        ? (DateTime?)fechaFinalizacion
                        : null;

                    estado = Convert.ToInt32(dr["estado"]);
                }
            }

            if (fechaFin.HasValue)
            {
                lblTiempoTotal.Text =
                    FormatearTiempo(fechaIngreso, fechaFin.Value);
            }
            else if (estado <= 1)
            {
                lblTiempoTotal.Text =
                    FormatearTiempo(fechaIngreso, DateTime.Now);
            }
            else
            {
                lblTiempoTotal.Text = "No disponible";
            }

            return true;
        }

       
        private void CargarAreas(
            SqlConnection conn,
            int idProtocolo,
            DateTime fechaIngreso)
        {
            string sql = @"
                SELECT
                    A.nombre AS nombreArea,
                    PAE.fechaTermina,
                    PAE.idUsuarioTermina
                FROM dbo.LAB_ProtocoloAreaEstado PAE
                INNER JOIN dbo.LAB_Area A
                    ON A.idArea = PAE.idArea
                WHERE PAE.idProtocolo = @idProtocolo
                  AND PAE.estado = 'T'
                  AND PAE.fechaTermina IS NOT NULL
                ORDER BY PAE.fechaTermina";

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@idProtocolo", SqlDbType.Int)
                    .Value = idProtocolo;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            dt.Columns.Add("tiempo", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                DateTime fechaTermina =
                    Convert.ToDateTime(row["fechaTermina"]);

                row["tiempo"] =
                    FormatearTiempo(fechaIngreso, fechaTermina);
            }

            pnlAreas.Visible = dt.Rows.Count > 0;

            if (dt.Rows.Count > 0)
            {
                gvAreas.DataSource = dt;
                gvAreas.DataBind();
            }
        }

        private string FormatearTiempo(DateTime inicio, DateTime fin)
        {
            if (fin < inicio)
                return "No disponible";

            TimeSpan tiempo = fin.Subtract(inicio);

            if (tiempo.Days > 0)
            {
                return string.Format(
                    "{0} d, {1} h, {2} min",
                    tiempo.Days,
                    tiempo.Hours,
                    tiempo.Minutes);
            }

            if (tiempo.Hours > 0)
            {
                return string.Format(
                    "{0} h, {1} min",
                    tiempo.Hours,
                    tiempo.Minutes);
            }

            return string.Format(
                "{0} min",
                tiempo.Minutes);
        }

        private void MostrarError(string mensaje)
        {
            lblEstado.Text = mensaje;
            lblFechaIngreso.Text = "-";
            lblFechaFin.Text = "-";
            lblTiempoTotal.Text = "-";
            pnlAreas.Visible = false;
        }
    }
}
