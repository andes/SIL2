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
    public partial class EditarListaLote : System.Web.UI.Page
    {
        public Usuario oUsuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUsuario = (Usuario)new Usuario().Get(typeof(Usuario), "IdUsuario", int.Parse(Session["idUsuario"].ToString()));

                if (!Page.IsPostBack)
                {
                    HFIdEfectorDerivacion.Value = Request["idEfectorDerivacion"].ToString();
                    CargarListas();
                    
                    txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarListas()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura
            string cacheKey = "";

            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            cacheKey = "CAT_TipoServicio";
            Business.Helpers.ComboCache.CargarCombo(ddlServicio, cacheKey, m_ssql, "idTipoServicio", "nombre", connReady);

            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
            CargarArea();
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "";
            if (ddlServicio.SelectedValue != "0")
                m_ssql = "select idArea, nombre from Lab_Area where baja=0    and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            else
                m_ssql = "select idArea, nombre from Lab_Area where baja=0   order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));
        }

        private void CargarGrilla()
        {
            if (Page.IsValid)
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

                string str_condicion = " vta.fecha >= '" + fecha1.ToString("yyyyMMdd") + "' AND vta.fecha <= '" + fecha2.ToString("yyyyMMdd") + "' " +
                    " AND vta.idEfectorDerivacion = " + HFIdEfectorDerivacion.Value  +
                    " AND vta.idEfector=" + oUsuario.IdEfector.IdEfector  +
                    " AND vta.estado = 0 "  +
                    " AND vta.idlote = 0 ";

                if (ddlServicio.SelectedValue != "0")
                    str_condicion += " AND vta.idTipoServicio = " + ddlServicio.SelectedValue;
                if (ddlArea.SelectedValue != "0")
                    str_condicion += " AND vta.idArea = " + ddlArea.SelectedValue;


                string m_strSQL = " SELECT " +
                  " STUFF(( " +
                  "     SELECT '|' + CAST(v2.idDetalleProtocolo AS varchar(20)) " + //si tengo varios iddetalle del mismo iditem los agrupo
                  "     FROM vta_LAB_Derivaciones v2 " +
                  "     WHERE v2.idProtocolo = vta.idProtocolo " +
                  "       AND v2.idItem = vta.idItem " +
                  "     FOR XML PATH('') " +
                  " ), 1, 1, '') AS idDetalleProtocolo, " +
                  "  numero, convert(varchar(10), fecha,103) as fecha, dni, " +
                  " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username " +
                  " FROM  vta_LAB_Derivaciones vta " +
                  " WHERE " + str_condicion +
                  " GROUP BY   vta.idProtocolo, vta.idItem, vta.numero,  vta.fecha,  vta.dni, vta.apellido, vta.nombre, vta.determinacion, vta.efectorderivacion,  vta.username "+
                  " ORDER BY numero ";


                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
               

                gvLista.DataSource = Ds.Tables[0];
                gvLista.DataBind();

            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = "";

            if (Page.IsValid)
            {
                int idLote = int.Parse(Request["id"]);
                int idUsuarioRegistro = oUsuario.IdUsuario;

                foreach (GridViewRow row in gvLista.Rows)
                {
                    if (((CheckBox)(row.Cells[0].FindControl("chkSel"))).Checked)
                    {
                        //Para los casos donde un analisis compuesto tiene mas de una determinacion simple con derivacion automatica, "desarmo" el pipe
                        string[] idDetalles = gvLista.DataKeys[row.RowIndex].Value.ToString().Split('|');
                        foreach (string idDetalleProtocolo in idDetalles)
                        {
                            DetalleProtocolo oDetalle = (DetalleProtocolo)new DetalleProtocolo().Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));

                            ISession m_session = NHibernateHttpModule.CurrentSession;
                            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                            crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));

                            IList lista = crit.List();

                            if (lista.Count > 0)
                            {
                                //verificamos resultado predefinido para cambiar el valor del resultadoCar correctamente
                                if (oDetalle.ResultadoCar != "Pendiente de derivar") oDetalle.ResultadoCar = oDetalle.ResultadoCar.Replace(" - Pendiente de derivar", "");
                                else oDetalle.ResultadoCar = "Pendiente para enviar ";

                                oDetalle.ConResultado = true;
                                oDetalle.IdUsuarioResultado = idUsuarioRegistro;
                                oDetalle.FechaResultado = DateTime.Now;
                                oDetalle.Save();

                                foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                                {
                                    oDeriva.Estado = 4; // 4  Pendiente para enviar
                                    oDeriva.IdUsuarioRegistro = idUsuarioRegistro;
                                    oDeriva.FechaRegistro = DateTime.Now;
                                    oDeriva.Idlote = idLote; //asociamos al lote del request
                                    oDeriva.Save();
                                }

                                /*Actualiza estado de protocolo*/
                                if (oDetalle.IdProtocolo.Estado < 2)
                                {
                                    if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", idUsuarioRegistro))
                                        oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);
                                    else
                                    {
                                        if (oDetalle.IdProtocolo.EnProceso())
                                            oDetalle.IdProtocolo.Estado = 1;//en proceso
                                        else
                                            oDetalle.IdProtocolo.Estado = 0;
                                    }
                                    oDetalle.IdProtocolo.Save();
                                }

                                //Auditoria
                                oDetalle.GrabarAuditoriaDetalleExtra("Modifica", idUsuarioRegistro, "Pendiente para enviar: Lote " + idLote);
                            }
                        }
                    }


                }

                lblMensaje.Text = "Determinaciones agregadas al lote";
                CargarGrilla();
            }
        }
        

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
        }

        protected void lnkDesMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        private void MarcarSeleccionados(bool p)
        {

            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("chkSel")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("chkSel"))).Checked = p;
            }

        }

        protected void cvGeneral_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;

            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox chk = row.FindControl("chkSel") as CheckBox;

                if (chk != null && chk.Checked)
                {
                    args.IsValid = true;
                    break;
                }
            }
            cvGeneral.ErrorMessage = "*Seleccione una fila";
        }

       
    }
}