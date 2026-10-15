using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
namespace WebLab.Derivaciones
{
    public partial class DerivacionAnular : System.Web.UI.Page
    {
        public Usuario oUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUsuario = (Usuario)new Usuario().Get(typeof(Usuario), "IdUsuario", int.Parse(Session["idUsuario"].ToString()));

                if (!Page.IsPostBack)
                {
                    CargarListas();
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarListas()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura
            string m_ssql = "SELECT idMotivo, descripcion FROM LAB_DerivacionMotivoCancelacion WHERE baja = 0";
            string cacheKey = $"CAT_MOT_{oUsuario.IdEfector.IdEfector}";
            Business.Helpers.ComboCache.CargarCombo(ddlMotivoCancelacion, cacheKey, m_ssql, "idMotivo", "descripcion", connReady);

            ddlMotivoCancelacion.Items.Insert(0, new ListItem("--Seleccione--", "0"));

        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";
            if (Page.IsValid)
            {
                string[] listaIdDetalles = Request["Lista"].Split('|');

                foreach (string idDetalle in listaIdDetalles)
                {
                    if (idDetalle != "")
                    {
                        DetalleProtocolo oDetalle = (DetalleProtocolo)new DetalleProtocolo().Get(typeof(DetalleProtocolo), int.Parse(idDetalle));
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                        crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));

                        IList lista = crit.List();
                        if (lista.Count > 0)
                        {
                            string resultadoDerivacion = "";
                            if (oDetalle.ResultadoCar == "Pendiente de derivar") resultadoDerivacion = "No Derivado: " + ddlMotivoCancelacion.SelectedItem.Text;
                            else resultadoDerivacion = oDetalle.ResultadoCar.Replace(" - Pendiente de derivar", " - No Derivado: " + ddlMotivoCancelacion.SelectedItem.Text);

                            foreach (Derivacion oDeriva in lista)
                            {
                                oDeriva.Estado = 2; //No enviado
                                oDeriva.IdUsuarioRegistro = oUsuario.IdUsuario;
                                oDeriva.FechaRegistro = DateTime.Now;
                                oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                                oDeriva.Idlote = 0;
                                oDeriva.IdMotivoCancelacion = int.Parse(ddlMotivoCancelacion.SelectedItem.Value);
                                oDeriva.Observacion = txtObservacion.Text;
                                oDeriva.Save();
                            }

                            //Cambia valores del detalle del protocolo
                            oDetalle.ResultadoCar = resultadoDerivacion;
                            oDetalle.ConResultado = false;
                            oDetalle.IdUsuarioResultado = oUsuario.IdUsuario;
                            oDetalle.FechaResultado = DateTime.Now;
                            oDetalle.Save();


                            if (oDetalle.IdProtocolo.Estado < 2)
                            {
                                if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", oUsuario.IdUsuario))
                                    oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);
                                else
                                {
                                    if (oDetalle.IdProtocolo.EnProceso())
                                    {
                                        oDetalle.IdProtocolo.Estado = 1;//en proceso
                                    }
                                    else
                                        oDetalle.IdProtocolo.Estado = 0;
                                }
                                oDetalle.IdProtocolo.Save();
                            }

                            oDetalle.GrabarAuditoriaDetalleExtra("No Derivado", oUsuario.IdUsuario, "Motivo cancelacion: "+ddlMotivoCancelacion.SelectedItem.Text); //que se grabe el motivo de cancelacion
                        }
                    }
                }
                lblMensaje.Text = "Se guardaron los cambios";
            }
            
        }
    }
}