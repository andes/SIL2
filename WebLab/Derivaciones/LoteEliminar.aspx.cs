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
using System.Text;

namespace WebLab.Derivaciones
{
    public partial class LoteEliminar : System.Web.UI.Page
    {
        private Random random = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null) {
                if (!Page.IsPostBack)
                {
                    SetToken();
                    PreventingDoubleSubmit(btnGuardar);
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
                // Baja de lote
                oLote = (LoteDerivacion)oLote.Get(typeof(LoteDerivacion), int.Parse(s_idCaso));
                oLote.Baja = true;
                oLote.Observacion = txtMotivoBaja.Text;
                oLote.Save();
                string accion = txtMotivoBaja.Text;

                if (accion.Length > 50)
                    accion = accion.Substring(0, 50);

                oLote.GrabarAuditoriaLoteDerivacion("Anula", int.Parse(Session["idUsuario"].ToString()),"Motivo baja" ,accion);

                //Libero derivaciones
                Usuario oUser = (Usuario) new Usuario().Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
                string ssql_Protocolo = @" IdLote=" + int.Parse(s_idCaso) +
                    " and {alias}.IdDetalleProtocolo " + // Traer la derivacion que tiene asociada un DetalleProtocolo
                    " in (Select IdDetalleProtocolo From LAB_DetalleProtocolo  where IdEfector=" + oUser.IdEfector.IdEfector + ")";
                crit.Add(Expression.Sql(ssql_Protocolo));


                IList lista = crit.List();
                foreach (Derivacion oDeriva in lista)
                {
                    oDeriva.Estado = 0;
                    oDeriva.Idlote = 0;
                    oDeriva.IdUsuarioRegistro = oUser.IdUsuario;
                    oDeriva.FechaRegistro = DateTime.Now;
                    oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                    oDeriva.IdMotivoCancelacion = 0;
                    oDeriva.Save();

                    DetalleProtocolo oDetalle = oDeriva.IdDetalleProtocolo;
                    //Derivacion comun
                    if(oDetalle.ResultadoCar == "Pendiente para enviar "){
                        oDetalle.ConResultado = false;
                        oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                        oDetalle.ResultadoCar = "Pendiente de derivar";
                        oDetalle.IdUsuarioResultado = oUser.IdUsuario;
                    }
                    else
                    { //Derivacion automatica (a apartir de un resultado predefinido) //no tiene que cambiar ConResultado , FechaResultado
                        oDetalle.ResultadoCar = oDetalle.ResultadoCar.Replace(" - Pendiente para enviar", " - Pendiente de derivar");
                    }
                   
                    oDetalle.Save();
                   
                    oDetalle.GrabarAuditoriaDetalleProtocolo("Desvincula del lote", oUser.IdUsuario);
                    /*Actualiza estado de protocolo*/
                    if (oDetalle.IdProtocolo.Estado < 2)
                    {
                        if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion",  oUser.IdUsuario))
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
                
                if (oLote.Baja) //inhabilitamos el btn Guardar para que no lo vuelvan a ejecutar repetidas veces
                    btnGuardar.Enabled = false;
            }

        } 
        
        
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && IsTokenValid())
            {
                Eliminar();
                estatus.Text = "El caso ha sido anulado";
            }
        }

        private void PreventingDoubleSubmit(Button button)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Procesando...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
        }

        private bool IsTokenValid()
        {
            bool result = double.Parse(hidToken.Value) == ((double)Session["NextToken"]);
            SetToken();
            return result;
        }

        private void SetToken()
        {
            double next = random.Next();
            hidToken.Value = next + "";
            Session["NextToken"] = next;
        }
    }
}