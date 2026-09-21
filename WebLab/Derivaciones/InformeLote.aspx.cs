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
        public Usuario oUser = new Usuario();
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oCr.Report.FileName = "";
                oCr.CacheDuration = 0;
                oCr.EnableCaching = false;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                if (!Page.IsPostBack)
                {
                    Inicializar();
                    CargarListas();
                }
            }
            else
            {
                Response.Redirect("../FinSesion.aspx", false);
            }
        }


        private void Inicializar()
        {
            lblTitulo.Text = "LOTE NUMERO "+ Request["idLote"];
            Efector oEfector = (Efector) new Efector().Get(typeof(Efector), "IdEfector",int.Parse(Request["Destino"].ToString()));
            lblSubtitulo.Text = "EFECTOR DESTINO: " +oEfector.Nombre;
        }
        
        private void CargarListas()
        {
            //Fecha y Hora de retiro del lote
            DateTime miFecha = DateTime.UtcNow.AddHours(-3); //Hora estándar de Argentina	(UTC-03:00)
            txtFecha.Text = miFecha.Date.ToString("yyyy-MM-dd");
            txtHora.Text = miFecha.ToString("HH:mm");
            /////////////////Estados de lote /////////////////
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura
            string m_ssql = "SELECT idEstado, nombre FROM LAB_LoteDerivacionEstado where baja=0 and idEstado in (2,3) ";
            string cacheKey = "CAT_EstadosLote";
            Business.Helpers.ComboCache.CargarCombo(ddlEstados, cacheKey, m_ssql, "idEstado", "nombre", connReady);
            // Transportista
            ddlTransporte.Items.Add(new ListItem("-- SELECCIONE --","0"));
            ddlTransporte.Items.Add(new ListItem("Público","1"));
            ddlTransporte.Items.Add(new ListItem("Privado", "2"));
        }

        #region Guardar
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            if (Page.IsValid)
            {
                Guardar();
                btnGuardar.Enabled = false;
                lblMensaje.Text = "Se guardaron los cambios correctamente";
            }
        }
        
      

        private void Guardar()
        {
            int idUsuario = oUser.IdUsuario;
            int estadoLote = Convert.ToInt32(ddlEstados.SelectedValue);
            string resultadoDerivacion = (estadoLote == 2) ? "Derivado: " + Request["Destino"] : "No Derivado. ";
            string observacion = txtObservacion.Text + " " + (estadoLote == 1 ? ddlTransporte.SelectedValue : "");
            string resultadoAuditoria = resultadoDerivacion + " " + observacion;
            
            LoteDerivacion lote = (LoteDerivacion)new LoteDerivacion().Get(typeof(LoteDerivacion), int.Parse(Request["idLote"]));
            lote.Estado = estadoLote;
            lote.Observacion = observacion;
            lote.IdUsuarioEnvio = (estadoLote == 2) ? idUsuario : 0; 
            string fecha_hora = txtFecha.Text + " " + txtHora.Text;
            DateTime fechaResultado = (estadoLote == 2) ? Convert.ToDateTime(fecha_hora) : DateTime.Parse("01/01/1900"); //para Estado "Derivado" poner la fecha actual y para estado "Cancelado" no poner Fecha
            lote.FechaEnvio = fechaResultado;
            lote.Save();
                   
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
            string ssql_Protocolo = @" IdLote=" + lote.IdLoteDerivacion + 
                " and {alias}.IdDetalleProtocolo " +
                " in (Select IdDetalleProtocolo From LAB_DetalleProtocolo  where IdEfector=" + oUser.IdEfector.IdEfector + ")";
            crit.Add(Expression.Sql(ssql_Protocolo));
            IList lista = crit.List();
                        
            foreach (Derivacion oDeriva in lista)
            {
                int estadoAnterior = oDeriva.Estado;
                oDeriva.Estado = (estadoLote == 2) ? 1 : 2;
                oDeriva.Save();


                //Cambia el resultado de LAB_DetalleProtocolo
                DetalleProtocolo oDet = oDeriva.IdDetalleProtocolo;

                string aux_resultadoCar="";
                //Si el resultado anterior era 'pendiente de derivar'
                if (estadoAnterior == 4 && oDet.ResultadoCar != "Pendiente para enviar ")   aux_resultadoCar = oDet.ResultadoCar.Replace("- Pendiente para enviar", "");

                //Si el resultado anterior era 'No derivado.'
                if(estadoAnterior == 2 && oDet.ResultadoCar != "No Derivado. ") aux_resultadoCar = oDet.ResultadoCar.Replace("- No Derivado.", "");

                oDet.ResultadoCar = (aux_resultadoCar != "") ? aux_resultadoCar + " - " + resultadoDerivacion  : resultadoDerivacion;
                oDet.ConResultado = true;
                oDet.IdUsuarioResultado = idUsuario;
                oDet.FechaResultado = fechaResultado;
                oDet.Save();

                //Inserta auditoria del detalle del protocolo
                oDet.GrabarAuditoriaDetalleProtocolo("Graba", idUsuario);
            }
                    
            //Inserta auditoria del lote
            lote.GrabarAuditoriaLoteDerivacion(lote.descripcionEstadoLote(), idUsuario); 
            lote.GrabarAuditoriaLoteDerivacion(resultadoAuditoria, idUsuario, "Observacion", txtObservacion.Text);

            if (estadoLote == 2) //Si deriva indica con que transportista fue, y que fecha y hora se retiro
            {
                lote.GrabarAuditoriaLoteDerivacion(resultadoAuditoria, idUsuario, "Transportista", ddlTransporte.SelectedValue);
                DateTime f = new DateTime(Convert.ToInt16(txtFecha.Text.Substring(0, 4)), Convert.ToInt16(txtFecha.Text.Substring(5, 2)), Convert.ToInt16(txtFecha.Text.Substring(8, 2)));
                lote.GrabarAuditoriaLoteDerivacion("Fecha y Hora retiro", idUsuario, "Fecha", f.ToString("dd/MM/yyyy")); //que las fechas tengan el mismo formato
                lote.GrabarAuditoriaLoteDerivacion("Fecha y Hora retiro", idUsuario, "Hora", txtHora.Text);
            }
        }
        #endregion



        protected void cvGeneral_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;

            if (ddlEstados.SelectedValue == "2" )
            {
                if (ddlTransporte.SelectedValue == "0")
                {
                    args.IsValid = false;
                    cvGeneral.ErrorMessage = "* Seleccionar transporte";
                    return;
                }
                
            }
            else
            {
                if(txtObservacion.Text == "")
                {
                    args.IsValid = false;
                    cvGeneral.ErrorMessage = "* Indicar en Observacion el motivo de Descarte";
                    return;
                }
            }

        }

        protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlEstados.SelectedValue != "0")
            {
                if(ddlEstados.SelectedValue == "2") //Derivado
                {
                    ddlTransporte.Enabled = true;
                    txtFecha.Enabled = true;
                    rfvFecha.Enabled = true;
                    txtHora.Enabled = true;
                    rfvHora.Enabled = true;
                }
                else //Descartado
                {
                    ddlTransporte.Enabled = false;
                    txtFecha.Enabled = false;
                    rfvFecha.Enabled = false;
                    txtHora.Enabled = false;
                    rfvHora.Enabled = false;
                }

            }
        }
    }
}
