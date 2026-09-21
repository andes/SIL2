using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data.Laboratorio;
using System.Data.SqlClient;
using Business;
using CrystalDecisions.Web;
using NHibernate;
using NHibernate.Expression;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class InformeList4 : System.Web.UI.Page
    {
        public Usuario oUser = new Usuario();
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                if (!Page.IsPostBack)
                {
                    Inicializar();
                    CargarGrilla();
                }
            }
            else 
                Response.Redirect("../FinSesion.aspx", false);
        }

        private void Inicializar()
        {
            if (Request["Tipo"] == "Alta")
            {
                lblTitulo.Text = "NUEVO LOTE";
                lblSubTitulo.Visible = true;
                pnlNroLote.Visible = false;
                HyperLink1.NavigateUrl = "~/Derivaciones/Derivados2.aspx?tipo=informe";
                gvLista.Visible = true;
                gvListaEdit.Visible = false;
            }
            else
            {
                if (Request["Tipo"] == "Modifica")
                {
                    lblTitulo.Text = "MODIFICACION DE LOTE ";
                    LoteDerivacion oLote = (LoteDerivacion)new LoteDerivacion().Get(typeof(LoteDerivacion), "IdLoteDerivacion", int.Parse(Request["idLote"].ToString()));

                    lblNroLote.Text = oLote.IdLoteDerivacion.ToString();
                    pnlNroLote.Visible = true;
                    Efector oEfector = (Efector)new Efector().Get(typeof(Efector), "IdEfector", int.Parse(Request["Destino"].ToString()));
                    lblSubTitulo.Text = "Efector Destino: " + oEfector.Nombre;
                    lblSubTitulo.Visible = true;
                    HyperLink1.NavigateUrl = "~/Derivaciones/LoteList.aspx?Parametros=" + Request["Parametros"].ToString();
                    btnAgregarDeterminaciones.Visible = (oLote.Estado == 1);
                    btnGuardar.Visible = false;
                    btnNoEnviado.Visible = false;
                    HFIdLote.Value = Request["idLote"].ToString();
                    HFIdEfectorDerivacion.Value = Request["Destino"].ToString();
                    gvLista.Visible = false;
                    gvListaEdit.Visible = true;
                }
            }
        }
        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[12].Controls[1];
              
                CmdEliminar.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";

                int estado = Convert.ToInt32(((Label)(e.Row.Cells[0].FindControl("lbl_estado"))).Text);
                if (Request["Tipo"] == "Modifica" || estado != 0) //Solo se puede eliminar con estado 0
                {
                    CmdEliminar.Visible = false;
                }
                
            }
        }
        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;

                if (row == null)
                    return;
                
                Eliminar(e.CommandArgument);
                CargarGrilla();

            }
        }
        protected bool HacerCheck(int estado)
        {
            if (Request["Tipo"] == "Modifica")
            {
                if (estado == 4) return true; //Dejar checkeados aquellos que ya estan en el lote
                else return false;
            }
            else
                return false;

        }
        protected void gvListaEdit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("CheckBox1");
                if (chk != null)
                {
                    chk.InputAttributes["onchange"] = "if(!PreguntoCambiarEstado(this)) { this.checked = !this.checked; return false; }";
                }
                
            }
        }

        protected void chkSel_CheckedChanged(object sender, EventArgs e)
        {
            //si es estado 4 pasar a estado 0

        }
        private void Eliminar(object detalle)
        {
            string[] idDetalles = detalle.ToString().Split('|');

            foreach (string idDetalleProtocolo in idDetalles)
            {
                DetalleProtocolo oDetalle = (DetalleProtocolo)new DetalleProtocolo().Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));
                
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));
                object oDerivacion = crit.UniqueResult();


                if (oDerivacion != null) {
                    oDetalle.GrabarAuditoriaDetalleProtocolo("Elimina Derivado", oUser.IdUsuario);
                    oDetalle.ResultadoCar = oDetalle.ResultadoCar.Replace(" - Pendiente de derivar", ""); 
                    oDetalle.Save();
                    ((Derivacion)(oDerivacion)).Delete();
                }
                
            }
            
        }
        
        private void CargarGrilla()
        {
            if(Request["Tipo"] == "Alta")
            {
                gvLista.DataSource = GetDataSet();
                gvLista.DataBind();
                CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";
            }
            else
            {
                gvListaEdit.DataSource = GetDataSet();
                gvListaEdit.DataBind();
                CantidadRegistros.Text = gvListaEdit.Rows.Count.ToString() + " registros encontrados";
            }

        }

        public DataTable GetDataSet()
        {
            //20.08.2026 Derivacion automatica
            //Para los casos donde un analisis compuesto tiene mas de una determinacion simple con derivacion automatica
            // agrupo con pipe los idDetalles a derivar pero solo muestro 1 derivacion 
            string m_strSQL = " SELECT " +
                 " STUFF(( " +
                 "     SELECT '|' + CAST(v2.idDetalleProtocolo AS varchar(20)) " + //si tengo varios iddetalle del mismo iditem los agrupo
                 "     FROM vta_LAB_Derivaciones v2 " +
                 "     WHERE v2.idProtocolo = vta.idProtocolo " +
                 "       AND v2.idItem = vta.idItem " +
                 "     FOR XML PATH('') " +
                 " ), 1, 1, '') AS idDetalleProtocolo, " +
                 " estado, numero, convert(varchar(10), fecha,103) as fecha, dni, " +
                 " apellido + ' '+ nombre as paciente, determinacion,  username, " +
                 " fechaNacimiento as edad, unidadEdad, sexo, " +
                 " solicitante as especialista, isnull(idlote,0) as idLote ";

            switch (Request["Tipo"])
            {

                  case "Alta":
                    m_strSQL += "   , observacion, efectorderivacion, isnull(mot.descripcion,'') as motivo ";
                    m_strSQL += " FROM  vta_LAB_Derivaciones vta ";
                    m_strSQL += " LEFT JOIN LAB_DerivacionMotivoCancelacion mot on mot.idMotivo = vta.idMotivoCancelacion ";
                    m_strSQL += " WHERE " + Request["Parametros"].ToString(); 

                    if(Request["Estado"] == "-1" ) // estado pendiente de derivar y No enviado
                         m_strSQL += " AND estado IN (0,2) ";
                    else
                        m_strSQL += " AND estado IN ( " + Request["Estado"] + " ) ";

                    m_strSQL += " and isnull(idlote,0) = 0 "; //que traiga derivaciones sin lote

                    m_strSQL += @" GROUP BY
                                    vta.idProtocolo, vta.idItem, vta.estado, vta.numero,  vta.fecha,  vta.dni, vta.apellido, vta.nombre, vta.determinacion, vta.efectorderivacion,
                                    vta.username, vta.fechaNacimiento, vta.unidadEdad,  vta.sexo, vta.observacion, vta.solicitante, vta.idlote,
                                    mot.descripcion ";
                    m_strSQL += " ORDER BY numero ";
                    break;

                case "Modifica":
                    m_strSQL += " FROM  vta_LAB_Derivaciones vta ";
                    m_strSQL += " WHERE   idLote= " + Request["idLote"] ; 
                    m_strSQL += @" GROUP BY
                                vta.idProtocolo, vta.idItem, vta.estado, vta.numero,  vta.fecha,  vta.dni, vta.apellido, vta.nombre, vta.determinacion,
                                vta.username, vta.fechaNacimiento, vta.unidadEdad,  vta.sexo,  vta.solicitante, vta.idlote";
                    m_strSQL += " ORDER BY numero ";
                    break;

            }


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["idUsuario"] != null)
                {
                    LoteDerivacion lote = new LoteDerivacion();
                    if (Request["Tipo"] == "Alta")
                    {
                        Efector d_efector = new Efector();
                        d_efector = (Efector)d_efector.Get(typeof(Efector), Convert.ToInt32(Request["Destino"]));
                        lote.IdEfectorDestino = d_efector;
                        lote.IdEfectorOrigen = oUser.IdEfector;
                        lote.IdUsuarioRegistro = oUser.IdUsuario;
                        lote.Estado = 1; //"CREADO" Segun tabla LAB_LoteDerivacionEstado
                        lote.Save();
                        lote.GrabarAuditoriaLoteDerivacion("Creado", oUser.IdUsuario);
                    } 
                    else
                    {
                        if (Request["Tipo"] == "Modifica")
                        {
                            lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), "IdLoteDerivacion", Request["idLote"]);
                            lote.GrabarAuditoriaLoteDerivacion("Modifica", oUser.IdUsuario);//Se guarda auditoria de modificacion de lote
                        }
                    }
                    Guardar(lote);
                    Response.Redirect("NuevoLote.aspx?Lote=" + lote.IdLoteDerivacion + "&Tipo=" + (Request["Tipo"]).ToString(), false);
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
            
        }
        private void Guardar(LoteDerivacion lote)
        {
            if (Session["idUsuario"] != null)
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                    if (((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked)
                    {
                        int estado = Convert.ToInt32(((Label)(row.Cells[0].FindControl("lbl_estado"))).Text);

                        string[] idDetalles = gvLista.DataKeys[row.RowIndex].Value.ToString().Split('|');//20.08.2026 Para los casos donde un analisis compuesto tiene mas de una determinacion simple con derivacion automatica, "desarmo" el pipe
                        foreach (string idDetalleProtocolo in idDetalles)
                        {
                            DetalleProtocolo oDetalle = (DetalleProtocolo)new DetalleProtocolo().Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));
                            string accion = Request["Tipo"].ToString();

                            ISession m_session = NHibernateHttpModule.CurrentSession;
                            ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
                            crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));

                            IList lista = crit.List();
                            if (lista.Count > 0)
                            {
                                string resultadoDerivacion = "", resultadoCar = "";

                                switch (estado)  //19.08.2026 estado de derivacion 1 enviado y 3 recibido no pasan por esta pantalla
                                {
                                    case 0: //Si ResultadoCar es igual a "Pendiente de derivar" es derivacion comun ino es derivacion automatica
                                        if (oDetalle.ResultadoCar != "Pendiente de derivar") resultadoCar = oDetalle.ResultadoCar.Replace(" - Pendiente de derivar", "");
                                        break;
                                    case 2:  //Si el ResultadoCar empieza con 'No derivado' es derivacion comun  sino es automatica
                                        if (!oDetalle.ResultadoCar.StartsWith("No Derivado:"))
                                        {
                                            int fin = oDetalle.ResultadoCar.IndexOf(" - No Derivado:");
                                            if (fin > 0) resultadoCar = oDetalle.ResultadoCar.Substring(0, fin);
                                        }
                                        break;
                                }
                                if (resultadoCar == "") //Es derivacion comun, piso resultadoCar
                                    resultadoDerivacion = "Pendiente para enviar ";
                                else // Es derivacion automatica: Agrego los nuevos valores al resultadoCar
                                {
                                    resultadoDerivacion = resultadoCar + " - Pendiente para enviar";
                                }

                                oDetalle.ResultadoCar = resultadoDerivacion;
                                oDetalle.ConResultado = true;
                                oDetalle.IdUsuarioResultado = oUser.IdUsuario;
                                oDetalle.FechaResultado = DateTime.Now;
                                oDetalle.Save();


                                foreach (Derivacion oDeriva in lista)
                                {
                                    oDeriva.Estado = 4;
                                    oDeriva.IdUsuarioRegistro = oUser.IdUsuario;
                                    oDeriva.FechaRegistro = DateTime.Now;
                                    oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                                    oDeriva.Idlote = lote.IdLoteDerivacion;
                                    oDeriva.Save();
                                }
                                
                                /*Actualiza estado de protocolo*/
                                if (oDetalle.IdProtocolo.Estado < 2)
                                {
                                    if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", oUser.IdUsuario))
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
                                }

                                //Auditoria de Alta
                                if (accion == "Alta")
                                {
                                    oDetalle.GrabarAuditoriaDetalleExtra(accion, oUser.IdUsuario, "Pendiente para enviar: Lote " + lote.IdLoteDerivacion);
                                }
                            }
                        }
                    }
                }
            }
            else 
                Response.Redirect("../FinSesion.aspx", false);
        }
		
        protected void btnAgregarDeterminaciones_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void cvGeneral_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox chk = row.FindControl("CheckBox1") as CheckBox;

                if (chk != null && chk.Checked)
                {
                    args.IsValid = true;
                    break;
                }
            }
            cvGeneral.ErrorMessage = "*Seleccione una fila";
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
            GridView gv;
            if (Request["Tipo"] == "Alta")
                gv = gvLista;
            else 
                gv = gvListaEdit;

            foreach (GridViewRow row in gv.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("chkSel")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("chkSel"))).Checked = p;
            }

        }

        protected void btnNoEnviado_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string listaIdDetalle = HFListaDetalles.Value;

                string script = "NoEnviarDeterminaciones('" + listaIdDetalle + "');";

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "NoEnviarDeterminaciones",
                    script,
                    true);
            }
        }


        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }
        protected void cvNoEnviado_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            HFListaDetalles.Value = "";
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox chk = row.FindControl("CheckBox1") as CheckBox;
                if (chk != null && chk.Checked)
                {
                    args.IsValid = true;
                    string idDetalle = gvLista.DataKeys[row.RowIndex].Value.ToString();
                    if (HFListaDetalles.Value == "")
                        HFListaDetalles.Value = idDetalle;
                    else
                        HFListaDetalles.Value = HFListaDetalles.Value + "|" + idDetalle;
                }
            }
            cvNoEnviado.ErrorMessage = "*Seleccione una fila";
        }

       
    }
}
