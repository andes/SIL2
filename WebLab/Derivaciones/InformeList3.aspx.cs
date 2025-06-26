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
    public partial class InformeList3 : System.Web.UI.Page
    {
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    CargarListas();

                    if (Request["Tipo"] == "Alta")
                    {
                        int estado = Convert.ToInt32(Request["Estado"]);
                        activarControles(estado == 0 || estado == 2);
                        pnlNroLote.Visible = false;
                        HyperLink1.NavigateUrl = "~/Derivaciones/Derivados2.aspx?tipo=informe";
                    }
                    else
                    {
                        if (Request["Tipo"] == "Modifica")
                        {
                            activarControles(true);
                            CargarParaModificacion();
                            lblNroLote.Text = "NUMERO DE LOTE " + Convert.ToInt32(Request["idLote"]);
                            pnlNroLote.Visible = true;
                            HyperLink1.NavigateUrl = "~/Derivaciones/GestionarLote.aspx";
                        }

                    }
                    CargarGrilla();
                }
                else
                {
                    Response.Redirect("../FinSesion.aspx", false);
                }
            }
            
        }


        #region carga
        private void CargarParaModificacion()
        {
            ddl_motivoCancelacion.SelectedValue = "0";
            ddlEstado.SelectedIndex = 2;
            ddl_motivoCancelacion.Enabled = false;

            //Observación
            Business.Data.Laboratorio.LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), Convert.ToInt32(Request["idLote"]));
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
            crit.Add(Expression.Eq("Idlote", lote.IdLoteDerivacion));
            IList lista = crit.List();
            if (lista.Count > 0)
                txt_observacion.Text = ((Business.Data.Laboratorio.Derivacion)lista[0]).Observacion;

        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            // Estados de las derivaciones
            string m_ssql = "SELECT idEstado, descripcion FROM LAB_DerivacionEstado where baja=0 and idEstado in (2,4) ";
            oUtil.CargarCombo(ddlEstado, m_ssql, "idEstado", "descripcion", connReady);
            ddlEstado.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            oUtil = new Utility();
            //Motivos de cancelacion LAB-75
            m_ssql = "SELECT idMotivo, descripcion FROM LAB_DerivacionMotivoCancelacion WHERE baja = 0";
            oUtil.CargarCombo(ddl_motivoCancelacion, m_ssql, "idMotivo", "descripcion", connReady);
            ddl_motivoCancelacion.Items.Insert(0, new ListItem("--Seleccione--", "0"));
        }
        private void limpiarForm()
        {
            txt_observacion.Text = string.Empty;
            ddl_motivoCancelacion.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
        }

        private void activarControles(bool valor)
        {
            btnGuardar.Enabled = valor;
            txt_observacion.Enabled = valor;
            lnkMarcar.Enabled = valor;
            lnkDesMarcar.Enabled = valor;
            //ddl_motivoCancelacion.Enabled = valor;
            ddlEstado.Enabled = valor;
        }

        private void CargarGrilla()
        {
            gvLista.DataSource = GetDataSet();
            gvLista.DataBind();

            if (gvLista.Rows.Count <= 0)
            {
                activarControles(false);
            }

            CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";

        }

        public DataTable GetDataSet()
        {
            string s_vta_LAB = "vta_LAB_Derivaciones vta";
            string join = " left join LAB_DerivacionMotivoCancelacion mot on mot.idMotivo = vta.idMotivoCancelacion ";

            string m_strSQL = " SELECT  idDetalleProtocolo, estado, numero, convert(varchar(10), fecha,103) as fecha, dni, " +
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , " +
            " solicitante as especialista , isnull(idlote,0) as idLote , isnull(mot.descripcion,'') as motivo" +
            " FROM  " + s_vta_LAB +
            join +
            " WHERE ";

            if (Request["Tipo"] == "Alta")
                m_strSQL += Request["Parametros"].ToString() + "  and estado = " + Request["Estado"].ToString() + " ORDER BY efectorDerivacion,numero ";
            else
            {
                if (Request["Tipo"] == "Modifica")
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                    m_strSQL +=
                       "    (" +
                           "     (estado = 0 and isnull(idlote,0) = 0 " +//Traer derivaciones pendientes por si se necesitan agregar 
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

        protected string CargarImagenEstado(int estado)
        {
            switch (estado)
            {
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

        protected bool HabilitarCheck(int estado)
        {
            switch (estado)
            {
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

        protected bool HacerCheck(int estado)
        {
            if (Request["Tipo"] == "Modifica")
            {
                if (estado == 4)
                {
                    return true; //Dejar checkeados aquellos que ya estan en el lote
                }
                else
                {
                    return false;
                }
            }
            else
                return false;

        }


        #endregion

        #region marcar
        //protected void lnkMarcar_Click(object sender, EventArgs e) {
        //    MarcarSeleccionados(true);
        //}

        //private void MarcarSeleccionados(bool p) {
        //    foreach (GridViewRow row in gvLista.Rows) {
        //        CheckBox a = ((CheckBox) (row.Cells[0].FindControl("CheckBox1")));
        //        if (a.Checked == !p)
        //            ((CheckBox) (row.Cells[0].FindControl("CheckBox1"))).Checked = p;
        //    }
        //    //PonerImagenes();
        //}
        //protected void lnkDesMarcar_Click(object sender, EventArgs e) {
        //    MarcarSeleccionados(false);
        //    //  PonerImagenes();
        //}

        #endregion

        #region Guardar
        protected void btnGuardar_Click(object sender, EventArgs e)
        {

            if (int.Parse(ddlEstado.SelectedValue) == 2)
            { //Cambia el estado a "No enviado"
                GuardarDerivaciones();
                CargarGrilla();
                limpiarForm();
            }
            else
            {
                Business.Data.Laboratorio.LoteDerivacion lote = new LoteDerivacion();
                if (Request["Tipo"] == "Alta")
                {    //Genera Lote y cambia determinaciones
                    lote = GenerarLote();
                }
                else
                {
                    if (Request["Tipo"] == "Modifica")
                    {
                        lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), "IdLoteDerivacion", Request["idLote"]);
                    }
                }
                GuardarDerivaciones(lote);
                Response.Redirect("NuevoLote.aspx?Lote=" + lote.IdLoteDerivacion + "&Tipo=" + (Request["Tipo"]).ToString(), false);
            }

        }

        private Business.Data.Laboratorio.LoteDerivacion GenerarLote()
        {
            Usuario oUser = new Usuario();
            int idUsuario = int.Parse(Session["idUsuario"].ToString());
            oUser = (Usuario)oUser.Get(typeof(Usuario), idUsuario);

            Efector d_efector = new Efector();
            d_efector = (Efector)d_efector.Get(typeof(Efector), Convert.ToInt32(Request["Destino"]));

            Business.Data.Laboratorio.LoteDerivacion lote = new Business.Data.Laboratorio.LoteDerivacion();
            lote.IdEfectorDestino = d_efector;
            lote.IdEfectorOrigen = oUser.IdEfector;
            lote.IdUsuarioRegistro = idUsuario;
            lote.Estado = 1; //"CREADO" Segun tabla LAB_LoteDerivacionEstado
            lote.Save();

            //Se guarda auditoria de creacion de lote
            lote.GrabarAuditoriaLoteDerivacion(lote.descripcionEstadoLote(), oUser.IdUsuario);// LAB-54 Sacar la palabra "Estado: xxxxx"
            return lote;
        }

        private void GuardarDerivaciones(Business.Data.Laboratorio.LoteDerivacion idLote = null)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                foreach (GridViewRow row in gvLista.Rows)
                {
                    int estado = Convert.ToInt32(((Label)(row.Cells[0].FindControl("lbl_estado"))).Text);
                    bool chequeado = ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked;

                    //CASOS: Se evalua el estado anterior de las determinaciones

                    // 1 - Tiene estado "Pendiente de derivar" (0) y fue checkeado -> Se asocia al lote
                    if (estado == 0 && chequeado)
                    {
                        ActualizarDetalleProtocolo(row, idLote);
                        continue; // ✅ La línea continue; en un foreach (o cualquier bucle) salta inmediatamente al siguiente ciclo de iteración, evitando que se siga ejecutando el resto del código dentro del bucle actual.
                    }

                    // 2 - Tiene estado "Pendiente para enviar" (4) 
                    if (estado == 4)
                    {

                        if (!chequeado) // No tiene check -> Se debe desasociar el lote y borrar el resultado de derivacion
                            ActualizarDetalleProtocolo(row, null, true);
                        else
                            //Tiene check
                            ActualizarDetalleProtocolo(row, idLote, true);
                                                
                        idLote.GrabarAuditoriaLoteDerivacion("Modifica", oUser.IdUsuario);//Se guarda auditoria de modificacion de lote
                        continue;
                    }

                    // 3 - Estado "No enviado" y esta chequeado
                    if (estado == 2 && chequeado)
                    {
                        if (idLote != null)
                        {
                            ActualizarDetalleProtocolo(row, idLote, true);//Se debe asociar el lote generado y se debe guardar el historial
                        }
                        else
                        {
                            string motivo = ((Label)row.FindControl("lbl_motivo")).Text;
                            bool tieneMotivo = !string.IsNullOrEmpty(motivo); // -> Si tiene motivo es porque ya se ha guardado con anterioridad la derivacion
                            ActualizarDetalleProtocolo(row, null, tieneMotivo);
                        }

                        idLote.GrabarAuditoriaLoteDerivacion("Modifica", oUser.IdUsuario);//Se guarda auditoria de modificacion de lote
                        continue;
                    }

                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void ActualizarDetalleProtocolo(GridViewRow row, Business.Data.Laboratorio.LoteDerivacion idLote = null, bool modifica = false)
        {
            int idDetalle = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            DetalleProtocolo oDetalle = (DetalleProtocolo)new DetalleProtocolo().Get(typeof(DetalleProtocolo), idDetalle);
            //int numeroProtocolo = oDetalle.IdProtocolo.Numero;

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
            crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));

            IList lista = crit.List();
            if (lista.Count > 0)
            {
                int estadoSeleccionado = Convert.ToInt32(ddlEstado.SelectedValue);
                string observacion = txt_observacion.Text;
                int idUsuario = oUser.IdUsuario;  //Convert.ToInt32(Session["idUsuario"]);
                DateTime fechaHora = DateTime.Now;
                int MotivoCancelacion = int.Parse(ddl_motivoCancelacion.SelectedItem.Value);
                foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                {
                    //Cambia valores de la derivacion
                    #region Derivacion
                    oDeriva.Estado = estadoSeleccionado;
                    oDeriva.Observacion = observacion;
                    oDeriva.IdUsuarioRegistro = idUsuario;
                    oDeriva.FechaRegistro = fechaHora;
                    oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                    oDeriva.Idlote = idLote is null? 0 : idLote.IdLoteDerivacion;
                    oDeriva.IdMotivoCancelacion = MotivoCancelacion;
                    oDeriva.Save();
                    #endregion

                    //Cambia valores del detalle del protocolo
                    #region Detalle_Protocolo
                    //Estado seleccionado =>
                    // 2	No Enviado
                    // 4  Pendiente para enviar
                    string resultadoDerivacion = (estadoSeleccionado == 2) ? "No Derivado: " + ddl_motivoCancelacion.SelectedItem.Text : "Pendiente para enviar ";

                    oDetalle.ResultadoCar = resultadoDerivacion;
                    oDetalle.ConResultado = true;
                    oDetalle.IdUsuarioResultado = idUsuario;
                    oDetalle.FechaResultado = fechaHora;
                    oDetalle.Save();

                    #endregion

                    #region estado_protocolo
                    /*Actualiza estado de protocolo*/
                    if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", idUsuario))
                        oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);
                    else
                    {
                        if (oDetalle.IdProtocolo.EnProceso())
                        {
                            oDetalle.IdProtocolo.Estado = 1;//en proceso
                                                            // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                        }
                        else
                            oDetalle.IdProtocolo.Estado = 0;
                    }
                    oDetalle.IdProtocolo.Save();
                    #endregion

                    //Auditoria:
                    string accion = (modifica) ? "Modifica" : "Alta";
                    /*string motivo = (estadoSeleccionado == 2) ? ddl_motivoCancelacion.SelectedItem.Text : "";
                    accion = accion + ": " + motivo;*/
                    oDetalle.GrabarAuditoriaDetalleProtocolo(accion, idUsuario);
                }
            }
        }
        #endregion

    }
}
