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
    public partial class CasoResultadoHistoView : System.Web.UI.Page
    {     //////////////////Se controla quien es el usuario que está por validar////////////////
      
        DataTable dtDeterminaciones; //tabla para determinaciones 
        Configuracion oCon = new Configuracion();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            //   if (Request["id"] != null)
            {
                //VerificaPermisos("Casos Histocompatibilidad");

                InicializarTablas();
                imgPdf.Visible = false;

               
                

                        id.Value = Context.Items["id"].ToString();



                MostrarDatos();
                }
            

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
                  
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MostrarDatos()
        {
           

            imgPdf.Visible = false;
         Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            lblTitulo.Text = oRegistro.IdCasoFiliacion.ToString();
            lblTitulo.Visible = true;
            lblNumero.Visible = true;
            lblNombre.Text = oRegistro.Nombre;

         
            if ((oRegistro.IdUsuarioValida == 0) && (oRegistro.IdUsuarioCarga > 0))
            {
                lblEntidad.Text = oRegistro.Solicitante;
               
                lblConclusion.Text = oRegistro.Conclusion;
                lblMetodo.Text = oRegistro.Metodo;
               
                lblObservaciones.Text = oRegistro.Marcoestudio;
                
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), oRegistro.IdUsuarioCarga);
                lblUsuario.Text = "Resultados cargados por " + oUser.Apellido + " " + oUser.Nombre + " - Fecha: " + oRegistro.FechaCarga.ToShortDateString() + " " + oRegistro.FechaCarga.ToShortTimeString();
                lblUsuario.Visible = true;
                lblUsuario.ForeColor = System.Drawing.Color.Black;
            }


            if (oRegistro.IdUsuarioValida > 0)
            {
                lblEntidad.Text = oRegistro.Solicitante;
               
                lblConclusion.Text = oRegistro.Conclusion;
                lblMetodo.Text = oRegistro.Metodo;
                
                lblObservaciones.Text = oRegistro.Marcoestudio;

                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), oRegistro.IdUsuarioValida);
                lblUsuario.Text = "Validado por " + oUser.Apellido + " " + oUser.Nombre + " - Fecha: " + oRegistro.FechaValida.ToShortDateString() + " " + oRegistro.FechaValida.ToShortTimeString();
                lblUsuario.Visible = true; lblUsuario.ForeColor = System.Drawing.Color.DarkRed;
                imgPdf.Visible = true;
                

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
                if (oDet.IdProtocolo.IdPaciente.IdEstado==2) // temporal
                    
                    datosMuestra += "- Temp: " + oDet.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
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
            MostrarResultados(oRegistro,listaProto);
           

            oRegistro.GrabarAuditoria("Consulta Caso", int.Parse(Session["idUsuario"].ToString()),"");
           
        }

        private void MostrarResultados(Business.Data.Laboratorio.CasoFiliacion oCaso, string lista)
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            gvResultado.DataSource = getResultadosCruzados(oCon.IdHistocompatibilidad, lista);
              
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
            //Context.Items.Add("idServicio", "3");
            Server.Transfer("CasoListResultado.aspx");
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
            oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuario"].ToString()), "Resultado_" + oRegistro.Nombre.Trim() );

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Resultado_" + oRegistro.Nombre.Trim() );


        }

      
    }
}
