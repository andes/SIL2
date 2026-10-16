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

        #region Inicializar
        private void Inicializar()
        {
            LoteDerivacion oLote;
            Efector oEfector;
            switch (Request["Tipo"])
            {
                case "Alta":
                    pnlNroLote.Visible = false;

                    lblTitulo.Text = "NUEVO LOTE";
                    lblSubTitulo.Visible = true;
                   
                    HyperLink1.NavigateUrl = "~/Derivaciones/Derivados2.aspx?tipo=informe";
                    
                    gvLista.Visible = true;
                    gvListaEdit.Visible = false;
                    break;

                case "Modifica":
                     oLote = (LoteDerivacion)new LoteDerivacion().Get(typeof(LoteDerivacion), "IdLoteDerivacion", int.Parse(Request["idLote"].ToString()));
                     oEfector = (Efector)new Efector().Get(typeof(Efector), "IdEfector", oLote.IdEfectorDestino.IdEfector);
                    
                    pnlNroLote.Visible = true;

                    lblTitulo.Text = "MODIFICACION DE LOTE ";
                    lblNroLote.Text = oLote.IdLoteDerivacion.ToString();
                    lblEstadoLote.Text = "ESTADO: " + oLote.descripcionEstadoLote();
                    lblSubTitulo.Text = "Efector Destino: " + oEfector.Nombre;
                    lblSubTitulo.Visible = true;
                  
                    HyperLink1.NavigateUrl = "~/Derivaciones/LoteList.aspx?Parametros=" + Request["Parametros"].ToString();
                    
                    btnAgregarDeterminaciones.Visible = (oLote.Estado == 1);
                    btnGuardar.Visible = false;
                    btnNoEnviado.Visible = false;
                    btnExcluir.Visible = true;

                    HFIdLote.Value = Request["idLote"].ToString();
                    HFIdEfectorDerivacion.Value = oLote.IdEfectorDestino.IdEfector.ToString();
                    
                    gvLista.Visible = false;
                    gvListaEdit.Visible = true;
                  
                  
                    break;

                case "Consulta":

                     oLote = (LoteDerivacion)new LoteDerivacion().Get(typeof(LoteDerivacion), "IdLoteDerivacion", int.Parse(Request["idLote"].ToString()));
                     oEfector = (Efector)new Efector().Get(typeof(Efector), "IdEfector", oLote.IdEfectorDestino.IdEfector);

                    pnlNroLote.Visible = true;

                    lblTitulo.Text = "CONSULTA DE LOTE ";
                    lblNroLote.Text = oLote.IdLoteDerivacion.ToString();
                    lblSubTitulo.Text = "Efector Destino: " + oEfector.Nombre;
                    lblSubTitulo.Visible = true;
                   
                    HyperLink1.NavigateUrl = "~/Derivaciones/LoteList.aspx?Parametros=" + Request["Parametros"].ToString();
                    
                    btnAgregarDeterminaciones.Visible = false;
                    btnGuardar.Visible = false;
                    btnNoEnviado.Visible = false;
                    btnExcluir.Visible = false;

                    gvLista.Visible = false;
                    gvListaEdit.Visible = true;
                    gvListaEdit.Columns[0].Visible = false;

                    linkSeleccionar.Visible = false;

                    break;
            }
           
        }
        private void CargarGrilla()
        {
            if (Request["Tipo"] == "Alta")
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

                    if (Request["Estado"] == "-1") // estado pendiente de derivar y No enviado
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
                case "Consulta":
                    m_strSQL += " FROM  vta_LAB_Derivaciones vta ";
                    m_strSQL += " WHERE   idLote= " + Request["idLote"];
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
        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[11].Controls[1];
              
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

        #endregion

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
            GridView gv; string control = "";
            if (Request["Tipo"] == "Alta")
            {
                gv = gvLista;
                control = "CheckBox1";
            }
            else
            {
                gv = gvListaEdit;
                control = "chkSel";
            }

            foreach (GridViewRow row in gv.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl(control)));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl(control))).Checked = p;
            }

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

                                if (estado == 0)
                                {
                                    if (oDetalle.ResultadoCar != "Pendiente de derivar")
                                        resultadoCar = oDetalle.ResultadoCar.Replace(" - Pendiente de derivar", "");
                                }
                                else if (estado == 2)
                                {
                                    if (!oDetalle.ResultadoCar.StartsWith("No Derivado:"))
                                    {
                                        int fin = oDetalle.ResultadoCar.IndexOf(" - No Derivado:");
                                        if (fin > 0)
                                            resultadoCar = oDetalle.ResultadoCar.Substring(0, fin);
                                    }
                                }

                                resultadoDerivacion = resultadoCar == ""
                                    ? "Pendiente para enviar "
                                    : resultadoCar + " - Pendiente para enviar";

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
                                    oDeriva.IdMotivoCancelacion = 0; 
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


        //Validaciones
        #region Validaciones
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
        protected void cvNoEnviado_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Determinaciones que no estan en un lote
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

        protected void cvExcluir_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Determinaciones que que estan en un lote

            args.IsValid = false;
            HFListaDetalles.Value = "";
            int cantSeleccionados = 0;

            foreach (GridViewRow row in gvListaEdit.Rows)
            {
                CheckBox chk = row.FindControl("chkSel") as CheckBox;

                if (chk != null && chk.Checked)
                {
                    cantSeleccionados++;

                    string idDetalle = gvListaEdit.DataKeys[row.RowIndex].Value.ToString();

                    if (HFListaDetalles.Value == "")
                        HFListaDetalles.Value = idDetalle;
                    else
                        HFListaDetalles.Value += "|" + idDetalle;
                }
            }

            // Validacion para saber si selecciono filas
            if (cantSeleccionados == 0)
            {
                HFListaDetalles.Value = "";
                args.IsValid = false;
                cvExcluir.ErrorMessage = "*Seleccione una fila";
                return;
            }

            // Valida que el lote no quede vacio
            if (cantSeleccionados == gvListaEdit.Rows.Count)
            {
                HFListaDetalles.Value = "";
                args.IsValid = false;
                cvExcluir.ErrorMessage = "*El lote debe contener al menos una determinación. No puede quedar vacío";
                return;
            }

            args.IsValid = true;
        }
        #endregion

    }
}
