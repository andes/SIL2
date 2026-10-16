using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Data.SqlClient;
using System.Data;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Drawing;

namespace WebLab.Derivaciones
{
    public partial class LoteEliminar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null) {
                if (!Page.IsPostBack)
                {
                    string s_iditem = Request["id"].ToString();
                    LoteDerivacion oLote = new LoteDerivacion();
                    oLote = (LoteDerivacion)oLote.Get(typeof(LoteDerivacion), int.Parse(s_iditem));

                    lblTitulo.Text = "Nro. Lote " + oLote.IdLoteDerivacion.ToString();
                    if (oLote.Baja)
                    {
                        estatus.Text = "El caso ha sido anulado. Ver auditoria";
                        estatus.Visible = true;
                        btnGuardar.Text = "Recuperar";
                        btnGuardar.Visible = false;
                    }
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        private void Eliminar()
        {
            string s_idCaso = Request["id"].ToString();

            LoteDerivacion oLote = new LoteDerivacion();
            if (s_idCaso != "")
            {
                oLote = (LoteDerivacion)oLote.Get(typeof(LoteDerivacion), int.Parse(s_idCaso));
                oLote.Baja = true;
                oLote.Observacion = txtMotivoBaja.Text;
                oLote.Save();
                string accion = txtMotivoBaja.Text;

                if (accion.Length > 50)
                    accion = accion.Substring(0, 50);

                oLote.GrabarAuditoriaLoteDerivacion("Anula", int.Parse(Session["idUsuario"].ToString()),"Motivo baja" ,accion);

                //Libero derivaciones

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
                crit.Add(Expression.Eq("Idlote", int.Parse(s_idCaso)));
                IList lista = crit.List();
                foreach (Derivacion oDeriva in lista)
                {
                    oDeriva.Estado = 0;
                    oDeriva.Idlote = 0;
                    oDeriva.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                    oDeriva.FechaRegistro = DateTime.Now;
                    oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                    oDeriva.IdMotivoCancelacion = 0;

                    DetalleProtocolo oDetalle = oDeriva.IdDetalleProtocolo;
                    oDetalle.ResultadoCar = oDetalle.ResultadoCar.Replace(" - Pendiente de derivar", "");
                    oDetalle.ConResultado = false;
                    oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                    oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                    oDetalle.GrabarAuditoriaDetalleProtocolo("Desvincula del lote", int.Parse(Session["idUsuario"].ToString()));
                    /*Actualiza estado de protocolo*/
                    if (oDetalle.IdProtocolo.Estado < 2)
                    {
                        if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", int.Parse(Session["idUsuario"].ToString())))
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
                }
                
            }

        } 
        
        
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Eliminar();
                estatus.Text = "El caso ha sido anulado";
            }
        }
    }
}