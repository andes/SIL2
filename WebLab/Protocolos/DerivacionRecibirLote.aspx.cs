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
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Protocolos {
    public partial class DerivacionRecibirLote : System.Web.UI.Page {

        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        #region Carga
        protected void Page_PreInit(object sender, EventArgs e) {
            if (Session["idUsuario"] != null) {
                oUser = (Usuario) oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion) oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            } else
                Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                VerificaPermisos("Derivacion");
                CargarEncabezado();
            } 
        }

        private void CargarEncabezado() {
            ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
            txtNumeroLote.Text = Request["idLote"];

            LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion) lote.Get(typeof(LoteDerivacion), Convert.ToInt32(Request["idLote"]));
            //lbl_FechaRegistro.Text = lote.FechaRegistro.Substring(0, 10);
            //fechaEnvio.Text = DateTime.Parse(lote.FechaEnvio.Substring(0, 10)).ToString("yyyy-MM-dd");
            hid_fechaEnvio.Value =  DateTime.Parse(lote.FechaEnvio.Substring(0, 10)).ToString("yyyy-MM-dd");
            lbl_fechaPermitida.Text = "Fecha envio: " + lote.FechaEnvio.Substring(0,10);
            Efector origen = new Efector();
            origen = (Efector) origen.Get(typeof(Efector), lote.IdEfectorOrigen);
            lb_efector.Text = origen.Nombre;
            
            CargarFechaHoraActual();

            //LAB-56 Cargo el transportista 
            List<AuditoriaLoteDerivacion> auditorias = AuditoriaLoteDerivacion.AuditoriasPorLote(Convert.ToInt32(Request["idLote"]));
            AuditoriaLoteDerivacion transporte = auditorias.FindLast(a => a.Analisis == "Transportista");
            lbl_transportista.Text = transporte.Valor;

            
        }
        private void CargarFechaHoraActual() {
            DateTime miFecha = DateTime.UtcNow.AddHours(-3); //Hora estándar de Argentina	(UTC-03:00)
            //txt_Fecha.Value = miFecha.Date.ToString("yyyy-MM-dd");
            txt_Hora.Value = miFecha.ToString("HH:mm");
            txtFecha.Text = miFecha.Date.ToString("yyyy-MM-dd");

            //LAB-74 Control de fecha: La fecha de ingreso del lote no puede ser anterior a la fecha de envio del lote
            rv_Fecha.MinimumValue = hid_fechaEnvio.Value;
            rv_Fecha.MaximumValue = txtFecha.Text; //Fecha Date today
            rv_Fecha.Text = "La fecha de recepcion no puede ser menor a la fecha de envio " + hid_fechaEnvio.Value;


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
        
         #endregion

        #region Guardar

        protected void btn_recibirLote_Click(object sender, EventArgs e) {
            //Cambiar estado al lote
            LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion) lote.Get(typeof(LoteDerivacion), Convert.ToInt32(Request["idLote"]));
            lote.Estado = 4;
            lote.IdUsuarioRecepcion = oUser.IdUsuario;
            lote.Save();

            //Generar Auditorias
            GenerarAuditorias(lote);
            btn_volver_Click(null, null);
        }

        private void GenerarAuditorias(LoteDerivacion lote) {
            string estado = lote.descripcionEstadoLote();

            lote.GrabarAuditoriaLoteDerivacion(estado, oUser.IdUsuario);  // LAB-54 Sacar la palabra "Estado: xxxxx"


            if (!string.IsNullOrEmpty(txt_Fecha.Value)) {
                DateTime f = new DateTime(Convert.ToInt16(txt_Fecha.Value.Substring(0, 4)), Convert.ToInt16(txt_Fecha.Value.Substring(5, 2)), Convert.ToInt16(txt_Fecha.Value.Substring(8, 2)));
                lote.GrabarAuditoriaLoteDerivacion(estado, oUser.IdUsuario, "Fecha Recibido", f.ToString("dd/MM/yyyy"));
            }

            //if (!string.IsNullOrEmpty(txt_Fecha.Value)) {
            //    DateTime f = new DateTime(Convert.ToInt16(txt_Fecha.Value.Substring(0, 4)), Convert.ToInt16(txt_Fecha.Value.Substring(5, 2)), Convert.ToInt16(txt_Fecha.Value.Substring(8, 2)));
            //    lote.GrabarAuditoriaLoteDerivacion(estado, oUser.IdUsuario, "Fecha Recibido", f.ToString("yyyy-MM-dd")); //Cambio formato de fecha asi tiene el mismo cuando se retira el lote
            //}

            if (!string.IsNullOrEmpty(txt_Hora.Value))
                lote.GrabarAuditoriaLoteDerivacion(estado, oUser.IdUsuario, "Hora Recibido", txt_Hora.Value);

            //if (!string.IsNullOrEmpty(txt_transportista.Text))
            //    lote.GrabarAuditoriaLoteDerivacion(estado, oUser.IdUsuario, "Transportista", txt_transportista.Text);

            if (!string.IsNullOrEmpty(txt_obs.Value))
                lote.GrabarAuditoriaLoteDerivacion(estado, oUser.IdUsuario, "Observacion", txt_obs.Value);
        }
        #endregion

        protected void btn_volver_Click(object sender, EventArgs e) {
            Response.Redirect("DerivacionMultiEfectorLote.aspx?idServicio=" + Request["idServicio"], false);
        }
    }
}