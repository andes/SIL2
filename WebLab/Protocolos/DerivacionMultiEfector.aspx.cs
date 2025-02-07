using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Text;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using Business.Data;

namespace WebLab.Protocolos {
    public partial class DerivacionMultiEfector : System.Web.UI.Page {

        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e) {

            //MiltiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null) {
                oUser = (Usuario) oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion) oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            } else
                Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                VerificaPermisos("Derivacion");
                //PreventingDoubleSubmit(btnBuscar);
                CargarListas();
                //CargarGrillaProtocolo(); 
                ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
                Session["idServicio"] = Request["idServicio"].ToString();
                txtNumeroProtocolo.Focus();
                Session["VariosLotes"] = null;
            }

        }
        private void VerificaPermisos(string sObjeto) {
            if (Session["idUsuario"] != null) {
                if (Session["s_permiso"] != null) {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList) Session["s_permiso"], sObjeto);
                    switch (i_permiso) {
                        case 0:
                            Response.Redirect("../AccesoDenegado.aspx", false);
                            break;
                        case 1:
                            Response.Redirect("../AccesoDenegado.aspx", false);
                            break;
                    }
                } else
                    Response.Redirect("../FinSesion.aspx", false);
            } else
                Response.Redirect("../FinSesion.aspx", false);
        }
        //private void CargarGrillaProtocolo()
        //{

        //    DataList1.DataSource = LeerDatosProtocolos();
        //    DataList1.DataBind();
        //}
        private object LeerDatosProtocolos() {
            string str_condicion = " WHERE P.baja=0 and P.idTipoServicio=" + Request["idServicio"].ToString(); // +Session["idServicio"].ToString();
            DateTime fecha1 = DateTime.Today;
            if (Request["urgencia"] != null) {
                str_condicion += " and P.idPrioridad=2 ";
            }

            string m_strSQL = " SELECT   TOP (10) P.numero AS numero, Pa.apellido + ' ' + Pa.nombre AS paciente, U.username ,P.idProtocolo ,P.idTipoServicio," +
            " Pa.numeroDocumento, P.idPaciente as idPaciente, P.fechaRegistro , TP.nombre as muestra " +
            " FROM         dbo.LAB_Protocolo AS P " +
            " INNER JOIN   dbo.Sys_Paciente AS Pa ON Pa.idPaciente = P.idPaciente " +
            " INNER JOIN   dbo.Sys_Usuario AS U ON U.idUsuario = P.idUsuarioRegistro " +
            " LEFT JOIN  dbo.LAB_Muestra AS TP ON TP.idMuestra = P.idMuestra " +
            str_condicion +
            " order by P.idProtocolo desc ";

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }


        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e) {
            HyperLink oHplInfo = (HyperLink) e.Item.FindControl("hplProtocoloEdit");
            if (oHplInfo != null) {
                string idProtocolo = oHplInfo.NavigateUrl;
                oHplInfo.NavigateUrl = "ProtocoloEdit2.aspx?idServicio=1&Desde=Derivacion&Operacion=Modifica&idProtocolo=" + idProtocolo;

                Label oMuestra = (Label) e.Item.FindControl("lblTipoMuestra");
                //if (Request["idServicio"].ToString() == "1")
                //{

                oMuestra.Visible = false;
                HyperLink oHplBacteriologia = (HyperLink) e.Item.FindControl("lnkMicrobiologia");
                if (oHplBacteriologia != null) {
                    oHplBacteriologia.Visible = true;
                    Label oIdPaciente = (Label) e.Item.FindControl("lblidPaciente");
                    oHplBacteriologia.NavigateUrl = "ProtocoloEdit2.aspx?idPaciente=" + oIdPaciente.Text + "&Operacion=Alta&idServicio=3&Urgencia=0";
                }
                //}
                //else
                //    oMuestra.Visible = true;
            }
        }
        private void CargarListas() {
            Utility oUtil = new Utility();

            //    string s_listaEfectores = ConfigurationManager.AppSettings["efectoresRPD"].ToString();
            ///Carga de combos de Efector del multiEfector
            string m_ssql = @"SELECT idEfector, nombre FROM sys_Efector WHERE idEfector<> " + oUser.IdEfector.IdEfector.ToString() + @"
                               and idEfector in (select idEfector from lab_configuracion) 
                               and idEfector in (select idEfector from LAB_ItemEfector where idEfectorDerivacion=" + oUser.IdEfector.IdEfector.ToString() + ") ORDER BY nombre ";

            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("-- Seleccione Efector --", "0"));

            if (Request["idEfectorSolicitante"] != null)
                ddlEfector.SelectedValue = Request["idEfectorSolicitante"].ToString();


            m_ssql = null;
            oUtil = null;
        }

        private void PreventingDoubleSubmit(Button button) {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Conectando...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
        }

        protected void btnBuscar_Click(object sender, EventArgs e) {
            // if (Page.IsValid) { Response.Redirect("DerivacionProcesa.aspx?idEfector=" + ddlEfector.SelectedValue + "&protocolo=" + txtNumeroProtocolo.Text + "&idServicio="+ Request["idServicio"].ToString(), false); }//+"&isScreening=0"

            gvProtocolosDerivados.DataSource = LeerDatosProtocolosDerivados();
            gvProtocolosDerivados.DataBind();
            MarcarSeleccionados(true);

        }
        protected void lnkMarcar_Click(object sender, EventArgs e) {
            MarcarSeleccionados(true);
            // PintarReferencias();
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e) {
            MarcarSeleccionados(false);
            //    PintarReferencias();
        }

        private void MarcarSeleccionados(bool p) {
            foreach (GridViewRow row in gvProtocolosDerivados.Rows) {
                CheckBox a = ((CheckBox) (row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox) (row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }

        private object LeerDatosProtocolosDerivados() {


            string m_strSQL = @" select D.idDerivacion,I.nombre as Determinacion, convert(varchar(10),P.fecha,103) as fecha , P.numero, Pac.apellido + ' ' + Pac.nombre  as paciente 
                                from LAB_Derivacion D
                                inner
                                join LAB_DetalleProtocolo as Det on Det.idDetalleProtocolo = D.idDetalleProtocolo
                                inner
                                join LAB_Protocolo as P on P.idProtocolo = det.idProtocolo
                                inner
                                join Sys_Paciente as Pac on Pac.idPaciente = P.idPaciente
                                inner
                                join lab_item  as I on I.iditem = det.idSubItem
                                where d.idEfectorDerivacion =" + oUser.IdEfector.IdEfector.ToString() + @"-- - destino
                                and P.numero=" + txtNumeroProtocolo.Text.Trim() + @"                 and P.idEfector =" + ddlEfector.SelectedValue.ToString() + @"--origen
                                and P.baja = 0
                                and D.estado = 1 ";


            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];

        }

        protected void btnEnviar_Click(object sender, EventArgs e) {
            GenerarProtocolo();
        }


        private void GenerarProtocolo() {

            string s_idPaciente = "";
            string pivot = "";
            string m_idDerivacion = "";
            string m_numeroMuestra = pivot;
            string m_numero = "";
            string s_idServicio = "";
            HashSet<string> lotes = new HashSet<string>();

            int cantidad = 1;
            foreach (GridViewRow row in gvProtocolosDerivados.Rows) {
                CheckBox a = ((CheckBox) (row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true) {
                    m_numero = row.Cells[1].Text;
                    m_idDerivacion = gvProtocolosDerivados.DataKeys[row.RowIndex].Value.ToString();
                    Business.Data.Laboratorio.Derivacion oDerivacion = new Business.Data.Laboratorio.Derivacion();
                    oDerivacion = (Business.Data.Laboratorio.Derivacion) oDerivacion.Get(typeof(Business.Data.Laboratorio.Derivacion), int.Parse(m_idDerivacion));
                    if (oDerivacion != null) {
                        if (cantidad == 1) {
                            s_idServicio = oDerivacion.IdDetalleProtocolo.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString();
                            s_idPaciente = oDerivacion.IdDetalleProtocolo.IdProtocolo.IdPaciente.IdPaciente.ToString();
                            pivot = oDerivacion.IdDetalleProtocolo.IdItem.IdItem.ToString();
                        } else {
                            pivot += "|" + oDerivacion.IdDetalleProtocolo.IdItem.IdItem.ToString();
                        }
                        if (oDerivacion.Idlote != null) {
                            lotes.Add(oDerivacion.Idlote.IdLoteDerivacion.ToString());
                        }
                    }
                    cantidad += 1;
                }
            }

            CargarLotes(lotes);

            if ((pivot != "") && (m_numero != "") && (cantidad >= 1)) {
                Response.Redirect("ProtocoloEdit2.aspx?idEfectorSolicitante=" + ddlEfector.SelectedValue + "&numeroProtocolo=" + m_numero + "&idServicio=" + s_idServicio + "&idPaciente=" + s_idPaciente + "&Operacion=AltaDerivacionMultiEfector&analisis=" + pivot, false);
            }
        }

        private void CargarLotes(HashSet<string> lotes) {
            if (Session["VariosLotes"] != null) {
                Session["VariosLotes"] = lotes;
            } else {
                Session.Add("VariosLotes", lotes);
            }
        }
    }
}