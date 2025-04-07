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
                    if(Request["Tipo"] == "Alta") {
                        int estado = Convert.ToInt32(Request["Estado"]);
                        activarControles(estado == 0 || estado == 2);
                    } else {
                        if (Request["Tipo"] == "Modifica")
                            activarControles(true);
                            CargarParaModificacion();
                    }
                   
                } else {
                    Response.Redirect("../FinSesion.aspx", false);
                }
            }
        }


        #region carga
        private void CargarParaModificacion() {
            
            Business.Data.Laboratorio.LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion) lote.Get(typeof(LoteDerivacion), Convert.ToInt32(Request["idLote"]));
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
            crit.Add(Expression.Eq("Idlote", lote ));
            Business.Data.Laboratorio.Derivacion derivacion = (Derivacion) crit.UniqueResult();
            
            txtObservacion.Text = derivacion.Observacion;
            ddlEstado.SelectedIndex = 2;
        }
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
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , solicitante as especialista , isnull(idlote,0) as idLote " +
            " FROM  " + s_vta_LAB +
            " WHERE ";


            if (Request["Tipo"] == "Alta")
                m_strSQL += Request["Parametros"].ToString() +  "  and estado = " + Request["Estado"].ToString() + " ORDER BY efectorDerivacion,numero ";
            else {
                if (Request["Tipo"] == "Modifica") {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario) oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                    m_strSQL +=
                       "    (" +
                           "     (estado = 0 and idlote is null " +//Traer derivaciones pendientes por si se necesitan agregar 
                           "       and idEfectorDerivacion = " + Request["Destino"] + " and idEfector = " + oUser.IdEfector.IdEfector + ")   " +
                           "  or (estado = 4 and idLote= " + Request["idLote"] + ")" + //y ya cargadas en el lote por si se necesitan dejar nuevamente como pendiente
                              ")" +
                     " ORDER BY estado desc, efectorDerivacion,numero desc";
                }
                   
            }
            
           



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
                case 0: //Pendiente de derivar
                    return true;
                case 1: //Enviado
                    return false; //ya se envio el lote
                case 2: //No Enviado
                    return true;
                case 3: //Recibido
                    return false; 
                case 4: //Pendiente para enviar
                    return true; //ya tiene generado un lote, pero se puede editar
                default:
                    return false;
            }
        }

        protected bool HacerCheck(int estado) {
            if(estado == 4) {
                return true; //Dejar checkeados aquellos que ya estan en el lote
            } else {
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

            if (int.Parse(ddlEstado.SelectedValue) == 2) { //Cambia el estado a "No enviado"
                GuardarDerivaciones();
                CargarGrilla();
                limpiarForm();
            } else {
                Business.Data.Laboratorio.LoteDerivacion lote = new LoteDerivacion();
                if (Request["Tipo"] == "Alta") {    //Genera Lote y cambia determinaciones
                    lote = GenerarLote();
                } else {
                    if (Request["Tipo"] == "Modifica") {
                        lote = (LoteDerivacion) lote.Get(typeof(LoteDerivacion), "IdLoteDerivacion", Request["idLote"]);
                    }
                }
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
            lote.GrabarAuditoriaLoteDerivacion( lote.descripcionEstadoLote(), oUser.IdUsuario);// LAB-54 Sacar la palabra "Estado: xxxxx"
            return lote;
        }

        private void GuardarDerivaciones(Business.Data.Laboratorio.LoteDerivacion idLote = null) {
            foreach (GridViewRow row in gvLista.Rows) {
                CheckBox a = ((CheckBox) (row.Cells[0].FindControl("CheckBox1")));
                int estado = Convert.ToInt32(((Label) (row.Cells[0].FindControl("lbl_estado"))).Text);
                /* Casos:
                 * PARA ENVIAR
                * (1) GUARDA: Tiene estado "Pendiente de derivar" y fue checkeado -> Se asocia al lote
                * (2) MODIFICA: Tiene estado "Pendiente para enviar" y no tiene check -> Se debe desasociar el lote y borrar el resultado de derivacion
                * PARA NO ENVIAR
                * (3) GUARDA: Tiene estado "Pendiente de derivar" o "Pendiente para enviar", y fue checkeado 
                */
                if(idLote != null) {
                    if (estado == 0 && a.Checked) { // (1) Tiene estado "Pendiente de derivar" y fue checkeado -> Se asocia al lote
                        ActualizarDetalleProtocolo(row, idLote);
                    } else {
                        if (estado == 4 && !a.Checked) { //(2) Tiene estado "Pendiente para enviar" y no tiene check -> Se debe desasociar el lote y borrar el resultado de derivacion
                            ActualizarDetalleProtocolo(row, null, true);
                        }
                    }
                } else {
                    ActualizarDetalleProtocolo(row, idLote);
                }
            }
        }

        private void ActualizarDetalleProtocolo(GridViewRow row , Business.Data.Laboratorio.LoteDerivacion idLote, bool modifica = false) {
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

                    if (modifica) { //Cambia el resultado de LAB_DetalleProtocolo
                        DetalleProtocolo oDet = new DetalleProtocolo();
                        oDet = (DetalleProtocolo) oDet.Get(typeof(DetalleProtocolo), oDeriva.IdDetalleProtocolo.IdDetalleProtocolo);
                        //Inserta auditoria del detalle del protocolo
                        oDet.GrabarAuditoriaDetalleProtocolo("Modifica", oDet.IdUsuarioResultado);
                    }
                }
            }
        }
        #endregion

    }
}
