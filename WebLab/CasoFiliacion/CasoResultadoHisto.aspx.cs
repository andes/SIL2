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
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;

namespace WebLab.CasoFiliacion
{
    public partial class CasoResultadoHisto : System.Web.UI.Page
    {     //////////////////Se controla quien es el usuario que está por validar////////////////
      
        DataTable dtDeterminaciones; //tabla para determinaciones 
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {


            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            //   if (Request["id"] != null)
            {
                VerificaPermisos("Casos Histocompatibilidad");

                InicializarTablas();
                imgPdf.Visible = false;

               
                    if (Request["Desde"] != null)
                    {
                        Desde.Value = Request["Desde"].ToString();
                        id.Value = Request["id"].ToString();

                    }
                    else
                    { 
                    id.Value = Context.Items["id"].ToString();

                    Desde.Value = Context.Items["Desde"].ToString();
                    }
                    if (Desde.Value == "Valida")
                        ValidacionUsuario();
                    MostrarDatos();
                }
            

        }

        private void ValidacionUsuario()
        {
       

            if ((oC.AutenticaValidacion) && (Session["idUsuarioValida"] == null))
            //    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);
            {
                //if ((Request["urgencia"] != null) && (oCon.AutenticaValidacion) && (Request["idUsuarioValida"] == null))
                string sredirect = "../Login.aspx?idServicio=3&Operacion=Valida&idCasoFiliacion=" + id.Value;
                if (Desde.Value != null)
                    sredirect += "&desde=" + Desde.Value;

                Response.Redirect(sredirect, false);
            }
            else
            {
                //if (Request["idUsuarioValida"] != null)
                //    Session["idUsuarioValida"] = Request["idUsuarioValida"];
                //else
                Session["idUsuarioValida"] = Session["idUsuario"];
                btnGuardar.Visible = false;
                btnValidar.Text = "Confirmar Validacion";
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnGuardar.Visible = false;
                            //btnAgregar.Visible = false;
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MostrarDatos()
        {
            if (Desde.Value == "Carga")
                btnValidar.Visible = false;
            else
                btnGuardar.Visible = false;

            imgPdf.Visible = false;
         Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            lblTitulo.Text = oRegistro.IdCasoFiliacion.ToString();
            lblTitulo.Visible = true;
            lblNumero.Visible = true;
            txtNombre.Text = oRegistro.Nombre;

         
            if ((oRegistro.IdUsuarioValida == 0) && (oRegistro.IdUsuarioCarga > 0))
            {
                txtSolicitante.Text = oRegistro.Solicitante;
                //txtAutos.Text = oRegistro.Autos;
                //txtObjetivo.Text = oRegistro.Objetivo;
                ////txtMuestra.Text = oRegistro.Muestra;
                //txtResultado.Text = oRegistro.Resultado;
                txtConclusion.Text = oRegistro.Conclusion;
                txtMetodo.Text = oRegistro.Metodo;
                //txtAmplificacion.Text = oRegistro.Amplificacion;
                //txtAnalisis.Text = oRegistro.Analisis;
                //txtSoftware.Text = oRegistro.Software;
                //txtEstadistico.Text = oRegistro.Estadistico;
                txtObservacion.Text = oRegistro.Marcoestudio;
                //txtBibliografia.Text = oRegistro.Bibliografia;
                Usuario oUser1 = new Usuario();
                oUser1 = (Usuario)oUser1.Get(typeof(Usuario), oRegistro.IdUsuarioCarga);
                lblUsuario.Text = "Resultados cargados por " + oUser1.Apellido + " " + oUser1.Nombre + " - Fecha: " + oRegistro.FechaCarga.ToShortDateString() + " " + oRegistro.FechaCarga.ToShortTimeString();
                lblUsuario.Visible = true;
                lblUsuario.ForeColor = System.Drawing.Color.Black;
            }


            if (oRegistro.IdUsuarioValida > 0)
            {
                txtSolicitante.Text = oRegistro.Solicitante;
                //txtAutos.Text = oRegistro.Autos;
                //txtObjetivo.Text = oRegistro.Objetivo;
                //txtMuestra.Text = oRegistro.Muestra;
                //txtResultado.Text = oRegistro.Resultado;
                txtConclusion.Text = oRegistro.Conclusion;
                txtMetodo.Text = oRegistro.Metodo;
                //txtAmplificacion.Text = oRegistro.Amplificacion;
                //txtAnalisis.Text = oRegistro.Analisis;
                //txtSoftware.Text = oRegistro.Software;
                //txtEstadistico.Text = oRegistro.Estadistico;
                txtObservacion.Text = oRegistro.Marcoestudio;

                Usuario oUser1 = new Usuario();
                oUser1 = (Usuario)oUser1.Get(typeof(Usuario), oRegistro.IdUsuarioCarga);
                lblUsuario.Text = "Validado por " + oUser1.Apellido + " " + oUser1.Nombre + " - Fecha: " + oRegistro.FechaValida.ToShortDateString() + " " + oRegistro.FechaValida.ToShortTimeString();
                lblUsuario.Visible = true; lblUsuario.ForeColor = System.Drawing.Color.DarkRed;
                imgPdf.Visible = true;
                if (Desde.Value == "Carga")
                    btnGuardar.Visible = false;

            }
            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            CasoFiliacionProtocolo oDetalle = new CasoFiliacionProtocolo();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));


            string listaProto = "";

            IList items = crit.List();            
            foreach (CasoFiliacionProtocolo oDet in items)
            {             
                    DataRow row = dtDeterminaciones.NewRow();
                    row[0] = oDet.IdProtocolo.Numero.ToString();
                row[1] = oDet.IdTipoParentesco.ToString() + " " + oDet.ObservacionParentesco;
                TipoParentesco oTP = new TipoParentesco();                oTP = (TipoParentesco)oTP.Get(typeof(TipoParentesco), oDet.IdTipoParentesco);
                row[2] = oTP.Nombre;
                string datosMuestra= oDet.IdProtocolo.Numero.ToString() + " " + oDet.IdProtocolo.IdPaciente.Apellido + " " + oDet.IdProtocolo.IdPaciente.Nombre;
                if (oDet.IdProtocolo.IdPaciente.IdEstado==2)                  
                    datosMuestra += "- HC: " + oDet.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                else
                    datosMuestra += "- DNI: " + oDet.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
               
                row[3] = datosMuestra;
                
                row[4] = "";
                    dtDeterminaciones.Rows.Add(row);
                if (listaProto == "") listaProto = oDet.IdProtocolo.IdProtocolo.ToString();
                else listaProto += ","+ oDet.IdProtocolo.IdProtocolo.ToString();

            }
            Session.Add("Tabla1", dtDeterminaciones);
            gvLista.DataSource = dtDeterminaciones;
            gvLista.DataBind();
            if (listaProto!="")
            MostrarResultados(oRegistro,listaProto);
           

            oRegistro.GrabarAuditoria("Consulta Caso",oUser.IdUsuario,"");
            txtObservacion.UpdateAfterCallBack = true;
        }

        private void MostrarResultados(Business.Data.Laboratorio.CasoFiliacion oCaso, string lista)
        {
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            gvResultado.DataSource = getResultadosCruzados(oC.IdHistocompatibilidad, lista);
              
            gvResultado.DataBind();
            
        }

        private object getResultadosCruzados(int idHistocompatibilidad, string lista)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "Lab_TablaCruzadahla";

            cmd.Parameters.Add("@ListaProtocolos", SqlDbType.NVarChar);
            cmd.Parameters["@ListaProtocolos"].Value = lista;

          

            cmd.Parameters.Add("@IdHojaTrabajo", SqlDbType.NVarChar);
            cmd.Parameters["@IdHojaTrabajo"].Value = idHistocompatibilidad.ToString();            



            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            return Ds.Tables[0];
        }

        private void InicializarTablas()
        {
            Utility oUtil = new Utility();

            string m_ssql = @" SELECT idObservacionResultado , codigo  AS descripcion 
                                FROM   LAB_ObservacionResultado where idTipoServicio=3 and  baja=0 order by codigo ";


            oUtil.CargarCombo(ddlObsCodificadaGeneral, m_ssql, "idObservacionResultado", "descripcion");
            ddlObsCodificadaGeneral.Items.Insert(0, new ListItem("", "0"));
            ddlObsCodificadaGeneral.UpdateAfterCallBack = true;

            ///Inicializa las sesiones para las tablas de diagnosticos y de determinaciones
            if (Session["Tabla1"] != null) Session["Tabla1"] = null;
            //if (Session["Tabla2"] != null) Session["Tabla2"] = null;

            dtDeterminaciones = new DataTable();


            dtDeterminaciones.Columns.Add("id"); // numero de protocolo
            dtDeterminaciones.Columns.Add("idtipoparentesco"); 
            dtDeterminaciones.Columns.Add("nombre"); //nombre del parentescto
            dtDeterminaciones.Columns.Add("protocolo"); // numero + apellido y nombre
            dtDeterminaciones.Columns.Add("eliminar");


            Session.Add("Tabla1", dtDeterminaciones);


        }



        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
               Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

                Guardar(oRegistro);

                Session.Contents.Remove("idUsuarioValida");
                HttpContext Context;

                Context = HttpContext.Current;
                Context.Items.Add("idServicio", "3");
                Server.Transfer("CasoList.aspx");

                //Response.Redirect("CasoList.aspx?idServicio=6", false);
            }
        }

        private void Guardar(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
          
       
            //Usuario oUser = new Usuario();
            //oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.Solicitante = txtSolicitante.Text;
            //oRegistro.Autos = txtAutos.Text;

            oRegistro.Nombre = txtNombre.Text;
            //oRegistro.Objetivo = txtObjetivo.Text;
            ////oRegistro.Muestra = txtMuestra.Text;
            //oRegistro.Resultado = txtResultado.Text;
            oRegistro.Conclusion = txtConclusion.Text;
            oRegistro.Metodo = txtMetodo.Text;
            //oRegistro.Amplificacion = txtAmplificacion.Text;
            //oRegistro.Analisis = txtAnalisis.Text;
            //oRegistro.Software = txtSoftware.Text;

            //oRegistro.Estadistico = txtEstadistico.Text;
            oRegistro.Marcoestudio = txtObservacion.Text;
            //oRegistro.Bibliografia = txtBibliografia.Text;
            
            string s_desde = "Carga";
            if (Desde.Value == "Valida")
            {
                s_desde = "Valida";
                oRegistro.FechaValida = DateTime.Now;
            oRegistro.IdUsuarioValida = oUser.IdUsuario;
               
            }

            else
            {
                oRegistro.FechaCarga = DateTime.Now;
                oRegistro.IdUsuarioCarga = oUser.IdUsuario;

            }
            oRegistro.Save();




            oRegistro.GrabarAuditoria(s_desde + " resultados", oUser.IdUsuario,"");
            



        }

        private void GuardarDetalle(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (CasoFiliacionProtocolo oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }

            /////Crea nuevamente los detalles.
            for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
            {
                CasoFiliacionProtocolo oDetalle = new CasoFiliacionProtocolo();
                Protocolo oItem = new Protocolo();
                oDetalle.IdCasoFiliacion = oRegistro;
                oDetalle.IdTipoParentesco = int.Parse(dtDeterminaciones.Rows[i][1].ToString());
                oDetalle.IdProtocolo= (Protocolo)oItem.Get(typeof(Protocolo),"Numero", dtDeterminaciones.Rows[i][0].ToString());
              
                oDetalle.Save();                           
            }
         
        }

     



        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    if (dtDeterminaciones.Rows[i][0].ToString() == e.CommandArgument.ToString())
                        dtDeterminaciones.Rows[i].Delete();
                }
                Session.Add("Tabla1", dtDeterminaciones);
                gvLista.DataSource = dtDeterminaciones;
                gvLista.DataBind();

              
            }
        }

        protected void cvListaDeterminaciones_ServerValidate(object sender, EventArgs e)
        {

        }

        protected void gvLista_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                //ImageButton CmdEliminar = (ImageButton)e.Row.Cells[2].Controls[1];
                //CmdEliminar.CommandArgument = dtDeterminaciones.Rows[e.Row.RowIndex][0].ToString();
                //CmdEliminar.CommandName = "Eliminar";
                //CmdEliminar.ToolTip = "Eliminar";

                //if (Permiso == 1)
                //{
                //    CmdEliminar.Visible = false;
                //}

            }
        }

        protected void ddlServicio_SelectedIndexChanged()
        {

        }

       
        protected void cvListaDeterminaciones_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            if (Session["Tabla1"] != null)
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                if (dtDeterminaciones.Rows.Count == 0) args.IsValid = false;
                else args.IsValid = true;
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Session.Contents.Remove("idUsuarioValida");
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "3");
            Server.Transfer("CasoList.aspx");
        }

        protected void imgPdf_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void Imprimir()
        {

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            CrystalReportSource oCr = new CrystalReportSource();

            oCr.Report.FileName = "ResultadoHisto.rpt";


            oCr.ReportDocument.SetDataSource(oRegistro.getResultadoHLA(oCon.IdHistocompatibilidad));


            oCr.DataBind();
            oRegistro.GrabarAuditoria("Imprime Resultado", oUser.IdUsuario, "Resultado_" + oRegistro.Nombre.Trim() );

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Resultado_" + oRegistro.Nombre.Trim() );


        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {  HttpContext Context;

                    Context = HttpContext.Current;
                    Context.Items.Add("idServicio", "3");
            if (btnValidar.Text == "Confirmar Validacion")
            {
                if (Page.IsValid)
                {
                    Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                    if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

                    Guardar(oRegistro);

                    Session.Contents.Remove("idUsuarioValida");
                    Server.Transfer("CasoList.aspx");

                    //Response.Redirect("CasoList.aspx?idServicio=6", false);
                }
            }
            else

            {
                Context.Items.Add("id", id.Value);
                Context.Items.Add("Desde", "Valida");
                Server.Transfer("CasoResultado.aspx");

                //Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + Request["id"].ToString() + "&Desde=Valida", false);
            }


        }

        protected void btnAgregarObsCodificadaGral_Click(object sender, EventArgs e)
        {
            if (ddlObsCodificadaGeneral.Text != "")
            {
                ObservacionResultado oRegistro = new ObservacionResultado();
                oRegistro = (ObservacionResultado)oRegistro.Get(typeof(ObservacionResultado), int.Parse(ddlObsCodificadaGeneral.SelectedValue));

                txtObservacion.Text = txtObservacion.Text + " " + oRegistro.Nombre;
                txtObservacion.UpdateAfterCallBack = true;
            }
        }
    }
}
