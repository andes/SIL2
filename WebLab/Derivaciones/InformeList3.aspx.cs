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
        public CrystalReportSource oCr = new CrystalReportSource();
        Configuracion oCon = new Configuracion();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            else Response.Redirect("../FinSesion.aspx", false);

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
                    habilitarImprresion(); //solo para derivaciones anteriores a lote
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
            ddlMotivoCancelacion.SelectedValue = "0";
            ddlEstado.SelectedIndex = 2;
            ddlMotivoCancelacion.Enabled = false;

            //Observación
            Business.Data.Laboratorio.LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), Convert.ToInt32(Request["idLote"]));
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
            crit.Add(Expression.Eq("Idlote", lote.IdLoteDerivacion));
            IList lista = crit.List();
            if (lista.Count > 0)
                txtObservacion.Text = ((Business.Data.Laboratorio.Derivacion)lista[0]).Observacion;

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
            oUtil.CargarCombo(ddlMotivoCancelacion, m_ssql, "idMotivo", "descripcion", connReady);
            ddlMotivoCancelacion.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            
            
        }
        private void habilitarImprresion()
        {
            if (Convert.ToInt32(Request["Estado"]) == 1)
            {
                gvLista.Columns[11].Visible = true;
                lnkPDF.Visible = true;
                ddlMotivoCancelacion.Enabled = false;
            }
            else
            {
                gvLista.Columns[11].Visible = false;
                lnkPDF.Visible = false;
            }
        }
        private void limpiarForm()
        {
            txtObservacion.Text = string.Empty;
            ddlMotivoCancelacion.SelectedIndex = 0;
            ddlEstado.SelectedIndex = 0;
        }

        private void activarControles(bool valor)
        {
            btnGuardar.Enabled = valor;
            txtObservacion.Enabled = valor;
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
            int estado = Convert.ToInt16(Request["Estado"]);

            string m_strSQL = " SELECT  idDetalleProtocolo, estado, numero, convert(varchar(10), fecha,103) as fecha, dni, " +
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , " +
            " solicitante as especialista , isnull(idlote,0) as idLote , isnull(mot.descripcion,'') as motivo" +
            " FROM  " + s_vta_LAB +
            join +
            " WHERE ";

            if (Request["Tipo"] == "Alta")
            {
                m_strSQL += Request["Parametros"].ToString() + "  and estado = " + Request["Estado"].ToString();
                if (estado == 0 )//Pendiente de derivar , no tiene que tener lote asociado 
                {
                    m_strSQL += " and isnull(idlote,0) = 0 "; //Si se de alta un nuevo Lote, que no traiga determinaciones con lote
                }
                m_strSQL += " ORDER BY efectorDerivacion,numero ";


            }
            else
            {
                if (Request["Tipo"] == "Modifica")
                {

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
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
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

        protected bool HabilitarCheck(int estado, int idLote)
        {
            switch (estado)
            {
                case 0: //Pendiente de derivar
                    return true;
                case 1: //Enviado
                    if(idLote == 0) //Son enviados sin lote, corresponde a version anterior
                    {
                        return true; //se habilita el check para que puedan seleccionar al imprimir
                    }
                    else
                    {
                        return false;
                    }
                   
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
            if (Session["idUsuario"] != null)
            {
                //Se verifica que se hayan realizados cambios
                if (hdnDatosModificados.Value == "false")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(),"noHuboCambios", "alert('No hay cambios para guardar');", true) ;
                }
                else
                {
                    int idUsuario = int.Parse(Session["idUsuario"].ToString());
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), idUsuario);
                    Business.Data.Laboratorio.LoteDerivacion lote = new Business.Data.Laboratorio.LoteDerivacion();

                    if (int.Parse(ddlEstado.SelectedValue) == 2)
                    { 
                        GuardarDerivaciones(lote, oUser.IdUsuario); //con idLote=0

                        if (Request["Tipo"] == "Modifica")
                            lote.GrabarAuditoriaLoteDerivacion("Modifica", idUsuario);//Se guarda auditoria de modificacion de lote
                        
                        CargarGrilla();
                        limpiarForm();
                    }
                    else
                    {
                        if (Request["Tipo"] == "Alta")
                        {    //Genera Lote y cambia determinaciones
                            lote = GenerarLote(oUser.IdUsuario);
                        }
                        else
                        {
                            if (Request["Tipo"] == "Modifica")
                            {
                                lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), "IdLoteDerivacion", Request["idLote"]);
                                lote.GrabarAuditoriaLoteDerivacion("Modifica", idUsuario);//Se guarda auditoria de modificacion de lote
                            }
                        }
                        GuardarDerivaciones(lote, oUser.IdUsuario);
                        Response.Redirect("NuevoLote.aspx?Lote=" + lote.IdLoteDerivacion + "&Tipo=" + (Request["Tipo"]).ToString(), false);
                    }
                }
                
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        private Business.Data.Laboratorio.LoteDerivacion GenerarLote(int idUsuario)
        {
            Efector d_efector = new Efector();
            d_efector = (Efector)d_efector.Get(typeof(Efector), Convert.ToInt32(Request["Destino"]));

            Business.Data.Laboratorio.LoteDerivacion lote = new Business.Data.Laboratorio.LoteDerivacion();
            lote.IdEfectorDestino = d_efector;
            lote.IdEfectorOrigen = oUser.IdEfector;
            lote.IdUsuarioRegistro = idUsuario;
            lote.Estado = 1; //"CREADO" Segun tabla LAB_LoteDerivacionEstado
            lote.Save();

            //Se guarda auditoria de creacion de lote
            lote.GrabarAuditoriaLoteDerivacion(lote.descripcionEstadoLote(), idUsuario);// LAB-54 Sacar la palabra "Estado: xxxxx"
            return lote;
        }

        private void GuardarDerivaciones(Business.Data.Laboratorio.LoteDerivacion lote, int idUsuario)
        {
            if (Session["idUsuario"] != null)
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                    int estado = Convert.ToInt32(((Label)(row.Cells[0].FindControl("lbl_estado"))).Text);
                    bool chequeado = ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked;
                    int idLote = lote.IdLoteDerivacion;
                    //CASOS: Se evalua el estado anterior de las determinaciones

                    // 1 - Esta chequeado -> Se asocia al lote
                    if ((estado == 0 || estado == 2 || estado == 4) && chequeado)
                    {
                        ActualizarDetalleProtocolo(row, idLote);
                        continue; // ✅ La línea continue; en un foreach (o cualquier bucle) salta inmediatamente al siguiente ciclo de iteración, evitando que se siga ejecutando el resto del código dentro del bucle actual.
                    }

                    // 2 - No esta chequeado y tiene estado "Pendiente para enviar" (4) 
                    if (estado == 4 && !chequeado)
                    {
                       ActualizarDetalleProtocolo(row, idLote, 1);
                       continue;
                    }
                }
            }
            else 
                Response.Redirect("../FinSesion.aspx", false);
        }

        private void ActualizarDetalleProtocolo(GridViewRow row, int idLote = 0,  int desasociaLote = 0)
        {
            int idDetalle = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());
            string accion = Request["Tipo"].ToString();
            DetalleProtocolo oDetalle = (DetalleProtocolo)new DetalleProtocolo().Get(typeof(DetalleProtocolo), idDetalle);
            //int numeroProtocolo = oDetalle.IdProtocolo.Numero;

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
            crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));

            IList lista = crit.List();
            if (lista.Count > 0)
            {
                int estadoSeleccionado;
                string resultadoDerivacion;
                string observacion = txtObservacion.Text;
                int idUsuarioRegistro = oUser.IdUsuario;  //Convert.ToInt32(Session["idUsuario"]);
                int idUsuarioResultado = oUser.IdUsuario;
                DateTime fechaDeHoy = DateTime.Now;
                DateTime fechaDeHoyDetalle = DateTime.Now;
                int MotivoCancelacion = int.Parse(ddlMotivoCancelacion.SelectedItem.Value);
                bool conResultado = true;

                if (desasociaLote == 0)
                {
                    estadoSeleccionado = Convert.ToInt32(ddlEstado.SelectedValue);//Estado seleccionado => 2	No Enviado - 4  Pendiente para enviar
                    resultadoDerivacion = (estadoSeleccionado == 2) ? "No Derivado: " + ddlMotivoCancelacion.SelectedItem.Text : "Pendiente para enviar ";
                }
                else
                {
                    //Se desasocia del lote y se setean a vacio los valores correspondientes
                    estadoSeleccionado = 0;
                    resultadoDerivacion = "";
                    observacion = string.Empty;
                    idLote = 0;
                    idUsuarioResultado = 0;
                    fechaDeHoyDetalle = DateTime.Parse("01/01/1900");
                    conResultado = false;
                }


                foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                {
                    //Cambia valores de la derivacion
                    #region Derivacion
                    oDeriva.Estado = estadoSeleccionado;
                    oDeriva.Observacion = observacion;
                    oDeriva.IdUsuarioRegistro = idUsuarioRegistro;
                    oDeriva.FechaRegistro = fechaDeHoy;
                    oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                    oDeriva.Idlote = idLote;
                    oDeriva.IdMotivoCancelacion = MotivoCancelacion;
                    oDeriva.Save();
                    #endregion

                    //Cambia valores del detalle del protocolo
                    #region Detalle_Protocolo
                    oDetalle.ResultadoCar = resultadoDerivacion;
                    oDetalle.ConResultado = conResultado;
                    oDetalle.IdUsuarioResultado = idUsuarioResultado;
                    oDetalle.FechaResultado = fechaDeHoyDetalle;
                    oDetalle.Save();

                    #endregion

                    #region estado_protocolo
                    /*Actualiza estado de protocolo*/
                    if(oDetalle.IdProtocolo.Estado < 2)
                    {
                        if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", idUsuarioRegistro))
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

                    #endregion

                    #region Auditorias
                   
                    if (desasociaLote == 0)
                    {
                        //Auditoria: Para el alta
                        if (accion == "Alta" )
                        {
                            if(estadoSeleccionado == 2) //Se selecciono "No Enviado"
                            {
                                oDetalle.GrabarAuditoriaDetalleExtra("No Derivado", idUsuarioRegistro, resultadoDerivacion); //que se grabe el motivo de cancelacion
                            }
                            else //Se selecciono "Pendiente para enviar"
                            {
                                oDetalle.GrabarAuditoriaDetalleExtra(accion, idUsuarioRegistro, resultadoDerivacion + ": Lote  " + idLote);

                            }
                            continue;

                        }

                        //Auditoria:  Para la modificacion, si se selecciono "Pendiente para enviar", y antes tenian estado "Pendiente de derivar" o "No Enviado"
                        // Si ya tiene estado "Pendiente para derivar" no le hacemos auditoria
                        int estado = Convert.ToInt32(((Label)(row.Cells[0].FindControl("lbl_estado"))).Text);
                        if (accion == "Modifica" && estadoSeleccionado == 4 && (estado == 0 || estado == 2))
                        {
                            oDetalle.GrabarAuditoriaDetalleExtra(accion, idUsuarioRegistro, resultadoDerivacion + ": Lote  " + idLote);
                        }
                    }
                    else
                    {
                        //Auditoria: Se destildo del lote
                        oDetalle.GrabarAuditoriaDetalleExtra("Elimina", idUsuarioRegistro, "Eliminado del lote");
                    }
                        
                  
                    #endregion
                }
            }
        }
        #endregion

        protected void lnkPDF_Command(object sender, CommandEventArgs e)
        {
            //Para imprimir los PDF de las derivaciones anteriores a Lotes
            MostrarInforme("pdf");
        }
        private void MostrarInforme(string tipo)
        {
            if (Session["idUsuario"] != null)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);


                DataTable dt = GetDataSet(GenerarListaProtocolos(), tipo);

                if (dt.Rows.Count > 0)
                {
                    string informe = "../Informes/Derivacion.rpt";

                    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = oCon.EncabezadoLinea1;
                    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = oCon.EncabezadoLinea2;
                    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = oCon.EncabezadoLinea3;

                    oCr.Report.FileName = informe;
                    oCr.ReportDocument.SetDataSource(dt);
                    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                    oCr.DataBind();

                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF(oUser.IdEfector.IdEfector2.Trim() + "_Derivaciones");
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                }
            }
            else 
                Response.Redirect("../FinSesion.aspx", false);
        }

        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            MostrarInforme("pdf");
        }
        private DataTable GetDataSet(string s_lista, string s_donde)
        {
            string s_vta_LAB = "vta_LAB_Derivaciones";
            string s_iddetalle = "";
            if (s_donde != "pdf")
                s_iddetalle = "idDetalleProtocolo,";

            string m_strSQL = " SELECT " + s_iddetalle + " estado, numero, convert(varchar(10), fecha,103) as fecha, dni, " +
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , solicitante as especialista " +
            " FROM  " + s_vta_LAB + " WHERE " + Request["Parametros"].ToString();
           
            if (s_donde != "pdf")
            {
                m_strSQL += "  and estado= " + Request["Estado"].ToString();
            }
            else
                m_strSQL += "  and estado= 1";


            if (s_lista != "")
                m_strSQL += "  and idDetalleProtocolo in (" + s_lista + ")";

            m_strSQL += " ORDER BY efectorDerivacion,numero ";


            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        private string GenerarListaProtocolos()
        {
            string m_lista = "";
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    if (m_lista == "")
                        m_lista += gvLista.DataKeys[row.RowIndex].Value.ToString();
                    else
                        m_lista += "," + gvLista.DataKeys[row.RowIndex].Value.ToString();
                }
            }
            return m_lista;
        }
    }
}
