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
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
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
                    int estado = Convert.ToInt32(Request["Estado"]);

                    if (estado == 1 || estado == 3)
                    {
                        activarControles(true);
                    }
                    else
                    {
                        activarControles(false);
                    }

                    CargarGrilla();
                    CargarControles();
                }

            }
            else
            {
                Response.Redirect("../FinSesion.aspx", false);
            }
        }






        private DataTable GetData(string m_strSQL)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;//LAB-130 usar conexion principal no la de consulta
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

        private void CargarGrilla()
        {
            DataTable dt;
            int estado = Convert.ToInt32(Request["Estado"]);
            string parametros = Request["Parametros"].ToString();

            string m_strSQL = " SELECT idLoteDerivacion as numero, e.nombre as efectorderivacion, l.estado, l.idEfectorDestino as idEfectorDerivacion," +
                                " fechaRegistro, " +
                                " case when (fechaenvio = '1900-01-01 00:00:00.000' ) then null else fechaEnvio end as fechaEnvio, " +
                                "  l.observacion ,uEmi.username as usernameE, isnull(uRecep.username, '' )  as usernameR " +
                                " FROM LAB_LoteDerivacion l " +
                                " inner join Sys_Efector e on e.idEfector=l.idEfectorDestino " +
                                " inner join Sys_Usuario uEmi on uEmi.idUsuario = idUsuarioRegistro " +
                                " left join Sys_Usuario uRecep on uRecep.idUsuario = idUsuarioEnvio " +
                                " where " + parametros + " AND baja = 0  AND estado = " + estado +
                                " ORDER BY l.idEfectorDestino, idLoteDerivacion ";

            dt = GetData(m_strSQL);
             

            if (dt.Rows.Count > 0)
            {
                gvLista.DataSource = dt;
            }
            else
            {
                activarControles(false); //desactiva los controles porque no hay nada para derivar o cancelar
            }

            gvLista.DataBind();
            CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";
        }


        private void activarControles(bool valor)
        {
            btnGuardar.Enabled = valor;
            txtObservacion.Enabled = valor;
            ddlEstados.Enabled = valor;
            //rb_transportista.Enabled = true; //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
            ddlTransporte.Enabled = valor;
            lnkMarcar.Enabled = valor;
            lnkDesMarcar.Enabled = valor;
            txtFecha.Enabled = valor;
            txtHora.Enabled = valor;
        }

        private void CargarControles()
        {
            CargarEstados();
            CargarTransportistas();
            CargarFechaHoraActual();
        }
        private void CargarEstados()
        {  /////////////////Estados de lote /////////////////
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "SELECT idEstado, nombre FROM LAB_LoteDerivacionEstado where baja=0 and idEstado in (2,3) ";
            oUtil.CargarCombo(ddlEstados, m_ssql, "idEstado", "nombre", connReady);

        }


        private void CargarTransportistas()
        {
            ddlTransporte.Items.Add("-- SELECCIONE --");
            //Vanesa: por ahora esta hardcodeado los transportitas, hacer mejora que lea de la base de datos
            ddlTransporte.Items.Add("Público");
            ddlTransporte.Items.Add("Privado");


            //Utility oUtil = new Utility();
            //string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString;
            //string m_ssql = "";
            //oUtil.CargarCombo(ddl_Transporte, m_ssql, "idEstado", "nombre", connReady);
        }
        private void CargarFechaHoraActual()
        {
            DateTime miFecha = DateTime.UtcNow.AddHours(-3); //Hora estándar de Argentina	(UTC-03:00)
            txtFecha.Text = miFecha.Date.ToString("yyyy-MM-dd");
            txtHora.Text = miFecha.ToString("HH:mm");
            //Date1.Text = miFecha.Date.ToString("yyyy-MM-dd");
            //Time1.Text = miFecha.ToString("HH:mm");
        }

        protected string ObtenerImagenEstado(int estado)
        {
            //Estados de Lote
            switch (estado)
            {
                case 1:
                    return "../App_Themes/default/images/reloj-de-arena.png";
                case 2:
                    return "~/App_Themes/default/images/enviado.png";
                case 3:
                    return "../App_Themes/default/images/block.png";
                default:
                    return "";
            }
        }

        protected bool habilitarCheckBoxSegunEstado(int estado)
        {
            switch (estado)
            {
                case 1:
                    return true;
                case 2:
                    return false;
                case 3:
                    return true;
                default:
                    return false;
            }
        }

        protected string habilitarEditarSegunEstado(int estado)
        {
            if (estado == 1)
                return "~/App_Themes/default/images/editar.jpg";
            else
                return "";

        }
         

        #region Marca
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        protected void lnkDesMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }

        
        #endregion

        #region PDF



        protected void lnkPDF_Command(object sender, CommandEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                string idLote = (((System.Web.UI.WebControls.LinkButton)sender).CommandArgument).ToString();
                GenerarPDF(idLote);
            }
            else
            {
                Response.Redirect("../FinSesion.aspx", false);
            }

        }


        private void GenerarPDF(string idLote)
        {

            string m_strSQL = Business.Data.Laboratorio.LoteDerivacion.derivacionPDF(int.Parse(idLote));

            DataTable dt = GetData(m_strSQL);

            if (dt.Rows.Count > 0)
            {
                string informe = "../Informes/DerivacionLote.rpt";
                Configuracion oCon = new Configuracion();
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

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
                string nombrePDF = oUtil.CompletarNombrePDF("Derivaciones"+idLote);
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
            }
            else
                ScriptManager.RegisterStartupScript(this, GetType(), "mensajeOk", "alert('No se encontraron datos para el numero de lote ingresado');", true);
        }


        #endregion

        #region Entrega
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                int idLote = verificarDeterminaciones();
                if (idLote == 0) //No trajo ningun id Lote con error
                {
                    if (Guardar())
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "mensajeOk", "alert('✅ Se guardaron los cambios con exito');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "mensajeOk", "alert('❌ ERROR: No se pudo guardar los datos.\\n Revisá los campos e intentá nuevamente');", true);

                    }
                    CargarGrilla();
                    limpiarForm();
                }
                else
                { //Es el idLote con error
                    ScriptManager.RegisterStartupScript(this, GetType(), "mensajeError", "alert('No se puede derivar lote N° " + idLote + " no tiene determinaciones.\\nAgregue determinaciones o descarte el lote.');", true);
                }

            }
            else
            {
                Response.Redirect("../FinSesion.aspx", false);
            }

        }
        
        private int verificarDeterminaciones()
        {
            int tieneDeterminaciones = 0;
            //Verificamos que en lote no se hayan descartados todas sus determinaciones asi no se Deriva vacio
            if (Convert.ToInt32(ddlEstados.SelectedValue) == 2)
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked)
                    {
                        int idLote = Convert.ToInt32(row.Cells[2].Text);
                        DataTable dt =  GetData("select top 1 1 from vta_LAB_Derivaciones where idLote =" + idLote);
                        if(dt.Rows.Count ==  0)  
                            return  idLote;
                    }
                }
            }
            return tieneDeterminaciones;
        }

        private bool Guardar()
        {
            bool seGuardoEnBd = false;
            if(verificarDeterminaciones() == 0) //No trajo ningun id Lote con error
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked)
                    {

                        int idLote = Convert.ToInt32(row.Cells[2].Text);
                        int idUsuario = oUser.IdUsuario;
                        int estadoLote = Convert.ToInt32(ddlEstados.SelectedValue);
                        string resultadoDerivacion = estadoLote == 2 ? "Derivado: " + row.Cells[3].Text : "No Derivado. ";
                        string observacion = txtObservacion.Text + " " + (estadoLote == 1 ? ddlTransporte.SelectedValue : "");
                        LoteDerivacion lote = new LoteDerivacion();
                        lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), idLote);

                    //Se cambia el estado del lote LAB_LoteDerivacion
                    lote.Estado = estadoLote;
                    lote.Observacion = observacion;
                    lote.IdUsuarioEnvio = (estadoLote == 2) ? idUsuario : 0; 
                        //para Estado "Derivado" poner la fecha actual y para estado "Cancelado" no poner Fecha
                        string fecha_hora = txtFecha.Text + " " + txtHora.Text;
                    lote.FechaEnvio = (estadoLote == 2) ? Convert.ToDateTime(fecha_hora) : DateTime.Parse("01/01/1900");
                    lote.Save();
                   
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                    crit.Add(Expression.Eq("Idlote", lote.IdLoteDerivacion));
                    IList lista = crit.List();


                    
                     

                     

                            foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                            {
                                #region Derivacion 
                                //Cambia el estado de las derivaciones LAB_Derivacion 

                                /*
                                 Estado del lote LAB_LoteDerivacionEstado (representa el estado del lote, no de la derivacion)
                                 1 : Creado
                                 2 : Derivado
                                 3 : Cancelado

                                 Estado de la derivacion LAB_DerivacionEstado
                                 0 : Pendiente de derivar
                                 1 : Enviado
                                 2 : No Enviado
                                 3 : Recibido
                                 4 : Pendiente para enviar
                               */
                                oDeriva.Estado = (estadoLote == 2) ? 1 : 2;
                                oDeriva.Save();
                                #endregion


                            #region cambio_codificacion_a_derivacion
                            //Cambia el resultado de LAB_DetalleProtocolo
                            DetalleProtocolo oDet = oDeriva.IdDetalleProtocolo;
                            oDet.ResultadoCar = resultadoDerivacion + " "+observacion;
                            oDet.ConResultado = true;
                            oDet.IdUsuarioResultado = idUsuario;
                            oDet.FechaResultado = Convert.ToDateTime(fecha_hora);
                            oDet.Save();
                            //Inserta auditoria del detalle del protocolo
                            oDet.GrabarAuditoriaDetalleProtocolo("Graba", idUsuario);
                            #endregion
                        }
                   

                   
                    
                    //Inserta auditoria del lote
                    lote.GrabarAuditoriaLoteDerivacion(lote.descripcionEstadoLote(), idUsuario); // LAB-54 Sacar la palabra "Estado: xxxxx"
                    lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Observacion", txtObservacion.Text);

                        if (estadoLote == 2) //Si deriva indica con que transportista fue, y que fecha y hora se retiro
                        {      //   lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Transportista", rb_transportista.SelectedValue); //Vanesa: Cambio el radio button por un dropdownlist (asociado a tarea LAB-52)
                            lote.GrabarAuditoriaLoteDerivacion(resultadoDerivacion, idUsuario, "Transportista", ddlTransporte.SelectedValue);
                            DateTime f = new DateTime(Convert.ToInt16(txtFecha.Text.Substring(0, 4)), Convert.ToInt16(txtFecha.Text.Substring(5, 2)), Convert.ToInt16(txtFecha.Text.Substring(8, 2)));
                            lote.GrabarAuditoriaLoteDerivacion("Fecha y Hora retiro", idUsuario, "Fecha", f.ToString("dd/MM/yyyy")); //que las fechas tengan el mismo formato
                            lote.GrabarAuditoriaLoteDerivacion("Fecha y Hora retiro", idUsuario, "Hora", txtHora.Text);
                        }


                        seGuardoEnBd = true;
                    }
                }
            }
            else
            {

            }

            return seGuardoEnBd;
        }
        #endregion


        private void limpiarForm()
        {
            txtObservacion.Text = string.Empty;
            ddlEstados.SelectedIndex = 0;
        }

        #region Editar
        protected void lnkEdit_Command(object sender, CommandEventArgs e)
        {
            Response.Redirect("InformeList3.aspx?idLote=" + e.CommandArgument + "&Destino=" + e.CommandName + "&Tipo=Modifica" , false);
        }

        #endregion

      
    }
}
