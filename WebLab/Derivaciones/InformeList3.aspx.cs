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

namespace WebLab.Derivaciones {
    public partial class InformeList3 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                if (Session["idUsuario"] != null) {
                    CargarListas();
                    CargarGrilla();
                    int estado = Convert.ToInt32(Request["Estado"]);
                    activarControles(estado == 0 || estado == 2);
                } else {
                    Response.Redirect("../FinSesion.aspx", false);
                }
            }
        }


        #region carga
        private void CargarListas() {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            // Estados de las derivaciones
            string m_ssql = "SELECT idEstado, descripcion FROM LAB_DerivacionEstado where baja=0 and idEstado in (2,4) ";
            oUtil.CargarCombo(ddlEstado, m_ssql, "idEstado", "descripcion", connReady);
            ddlEstado.Items.Insert(0, new ListItem("--Seleccione--", "0"));
        }
        private void limpiarForm() {
            txtObservacion.Text = string.Empty;
            ddlEstado.SelectedIndex = 0;
        }

        private void activarControles(bool valor) {
            btnGuardar.Enabled = valor;
            txtObservacion.Enabled = valor;
            lnkMarcar.Enabled = valor;
            lnkDesMarcar.Enabled = valor;
        }

        private void CargarGrilla() {
            gvLista.DataSource = GetDataSet();
            gvLista.DataBind();

            if (gvLista.Rows.Count <= 0) {
                activarControles(false);
            }

            CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";

        }

        public DataTable GetDataSet() {
            string s_vta_LAB = "vta_LAB_Derivaciones";

            string m_strSQL = " SELECT  idDetalleProtocolo, estado, numero, convert(varchar(10), fecha,103) as fecha, dni, " +
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , solicitante as especialista " +
            " FROM  " + s_vta_LAB +
            " WHERE " + Request["Parametros"].ToString() +
            "  and estado = " + Request["Estado"].ToString() +
            " ORDER BY efectorDerivacion,numero ";



            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];

        }

        protected string CargarImagenEstado(int estado) {
            switch (estado) {
                case 0:
                    return "~/App_Themes/default/images/pendiente.png";
                case 1:
                    return "~/App_Themes/default/images/enviado.png";
                case 2:
                    return "../App_Themes/default/images/block.png";
                case 4:
                    return "../App_Themes/default/images/reloj-de-arena.png";
                default:
                    return "";
                    //case 3 es Recibido! 16/01/2025
            }
        }

        protected bool HabilitarCheck(int estado) {
            switch (estado) {
                case 0:
                    return true;
                case 1:
                    return false; //ya se envio el lote
                case 2:
                    return true;
                case 3:
                    return false; //ya tiene generado un lote
                default:
                    return false;
            }
        }
        #endregion

        #region marcar
        protected void lnkMarcar_Click(object sender, EventArgs e) {
            MarcarSeleccionados(true);
        }

        private void MarcarSeleccionados(bool p) {
            foreach (GridViewRow row in gvLista.Rows) {
                CheckBox a = ((CheckBox) (row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox) (row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
            //PonerImagenes();
        }
        protected void lnkDesMarcar_Click(object sender, EventArgs e) {
            MarcarSeleccionados(false);
            //  PonerImagenes();
        }

        #endregion

        #region Guardar
        protected void btnGuardar_Click(object sender, EventArgs e) {

            if (int.Parse(ddlEstado.SelectedValue) == 2) {
                //Cambia el estado a "No enviado"
                GuardarDerivaciones();
                CargarGrilla();
                limpiarForm();
            } else {
                //Genera Lote y cambia determinaciones
                Business.Data.Laboratorio.LoteDerivacion lote = GenerarLote();
                GuardarDerivaciones(lote);
                Response.Redirect("NuevoLote.aspx?Lote=" + lote.IdLoteDerivacion, false);
            }

        }

        private Business.Data.Laboratorio.LoteDerivacion GenerarLote() {
            Usuario oUser = new Usuario();
            int idUsuario = int.Parse(Session["idUsuario"].ToString());
            oUser = (Usuario) oUser.Get(typeof(Usuario), idUsuario);

            Business.Data.Laboratorio.LoteDerivacion lote = new Business.Data.Laboratorio.LoteDerivacion {
                IdEfectorDestino = Convert.ToInt32(Request["Destino"]),
                IdEfectorOrigen = oUser.IdEfector.IdEfector,
                IdUsuarioRegistro = idUsuario,
                IdUsuarioRecepcion = null,
                Estado = 1 //"CREADO" Segun tabla LAB_LoteDerivacionEstado
            };
            lote.Save();

            //Se guarda auditoria de creacion de lote
            lote.GrabarAuditoriaLoteDerivacion("Estado: " + lote.descripcionEstadoLote(), oUser.IdUsuario);

            return lote;
        }

        private void GuardarDerivaciones(Business.Data.Laboratorio.LoteDerivacion idLote = null) {
            foreach (GridViewRow row in gvLista.Rows) {
                CheckBox a = ((CheckBox) (row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked) {
                    DetalleProtocolo oDetalle = new DetalleProtocolo();
                    oDetalle = (DetalleProtocolo) oDetalle.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                    crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));
                    IList lista = crit.List();
                    if (lista.Count > 0) {
                        foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista) {
                            oDeriva.Estado = int.Parse(ddlEstado.SelectedValue);
                            oDeriva.Observacion = txtObservacion.Text;
                            oDeriva.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                            oDeriva.FechaRegistro = DateTime.Now;
                            oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                            oDeriva.Idlote = idLote;
                            oDeriva.Save();
                        }
                    }
                }
            }
        }

        #endregion

    }
}
