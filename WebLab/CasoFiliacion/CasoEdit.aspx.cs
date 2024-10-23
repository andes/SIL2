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
using System.Text.RegularExpressions;

namespace WebLab.CasoFiliacion
{
    public partial class CasoEdit : System.Web.UI.Page
    {

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
       
            {            
                if (Context.Items.Contains("idServicio"))
                {
                    id.Value = Context.Items["id"].ToString();
                    idServicio.Value = Context.Items["idServicio"].ToString();
                    if (Context.Items["idServicio"].ToString() == "3")
                        VerificaPermisos("Casos Histocompatibilidad");
                    if (Context.Items["idServicio"].ToString() == "6")

                        VerificaPermisos("Casos Forense");
                }
                InicializarTablas();
                CargarListas();
                btnResultados.Visible = false;
                btnResultados0.Visible = false;
                if (Context.Items.Contains("id"))
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
                    case 1:
                        {
                            btnGuardar.Visible = false;
                            btnAgregar.Visible = false;
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MostrarDatos()
        {
            string s_idCaso = Context.Items["id"].ToString();

            if (idServicio.Value == "3")
            {
                Label1.Text = "Caso de Histocompatibilidad";

                btnAgregar0.Visible = false;
                btnResultados.Visible = false;
                btnResultados0.Visible = false;
            }
            else
                Label1.Text = "Caso de Filiación";


            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(s_idCaso));

            if (oRegistro.IdTipoCaso == 2)
            {
                HttpContext Context;

                Context = HttpContext.Current;
                Context.Items.Add("idServicio", "6");
                Context.Items.Add("id", oRegistro.IdCasoFiliacion.ToString());
                Server.Transfer("CasoForenseView.aspx");

            }
            //btnResultados.Visible = true;

            if (oRegistro.IdUsuarioValida > 0)
            {
                btnAgregar.Visible = false;
                btnAgregar0.Visible = false;
                  btnResultados0.Visible = false;
                btnEliminar.Visible = false;// si está validado no puede anular.
                btnGuardar.Visible = false; // si está validado no puede modificar.

                gvLista.Columns[2].Visible = false;

                if (VerificaPermisosObjeto("Valida Resultados"))
                {
                    btnGuardar.Visible = true; // si está validado no puede modificar.

                    gvLista.Columns[2].Visible = true;
                    btnAgregar.Visible = true;
                    btnAgregar0.Visible = true;
                    btnResultados0.Visible = true;
                    btnEliminar.Visible = true;// si está validado no puede anular.
                    btnGuardar.Visible = true; // si está validado no puede modificar.

                }
                //if (int.Parse(Session["idUsuario"].ToString()) == oRegistro.IdUsuarioValida) // solopuede modificar el validador.
                //    btnGuardar.Visible = true;

            }
            else
            {
                btnResultados0.Visible = true;
                btnEliminar.Visible = true;// 
            }

           
            lblTitulo.Text = oRegistro.IdCasoFiliacion.ToString();
            lblTitulo.Visible = true;
            lblNumero.Visible = true;
            txtNombre.Text = oRegistro.Nombre;

            if (oRegistro.Baja)
            {
                Business.Data.Usuario oUser = new Business.Data.Usuario();
                oUser = (Business.Data.Usuario)oUser.Get(typeof(Business.Data.Usuario), oRegistro.IdUsuarioRegistro);

                estatus.Text = "Anulado " + oRegistro.MotivoBaja + " " + oRegistro.FechaRegistro.ToShortDateString() + " " + oUser.Apellido+ " "+ oUser.Nombre;
                btnAgregar.Enabled = false;
                txtCodigo.Enabled = false;
                btnAgregar0.Enabled = false;
                btnEliminar.Visible = false;
                btnGuardar.Visible = false;
                btnResultados.Visible = false;
                btnResultados0.Visible = false;
                    }

            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            CasoFiliacionProtocolo oDetalle = new CasoFiliacionProtocolo();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
        

            IList items = crit.List();            
            foreach (CasoFiliacionProtocolo oDet in items)
            {
                DataRow row = dtDeterminaciones.NewRow();
                row[0] = oDet.IdProtocolo.Numero.ToString();
                row[1] = oDet.IdTipoParentesco.ToString();
                TipoParentesco oTP = new TipoParentesco(); oTP = (TipoParentesco)oTP.Get(typeof(TipoParentesco), oDet.IdTipoParentesco);
                row[2] = oTP.Nombre;
                row[3] = oDet.IdProtocolo.Numero.ToString() + " " + oDet.IdProtocolo.IdPaciente.Apellido + " " + oDet.IdProtocolo.IdPaciente.Nombre;
                row[4] = "";
                dtDeterminaciones.Rows.Add(row);

            }
            Session.Add("Tabla1", dtDeterminaciones);
            gvLista.DataSource = dtDeterminaciones;
            gvLista.DataBind();



        }


        private bool VerificaPermisosObjeto(string v)
        {
            bool i = false;

            Utility oUtil = new Utility();
            Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], v);
            switch (Permiso)
            {
                case 0: i = false; break;
                case 1:
                    i = true; break;
                case 2: i = true; break;

            }

            return i;

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


        private void CargarListas()
        {
            Utility oUtil = new Utility();


            string m_ssql = "select idParentesco, nombre from Lab_Parentesco (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlParentesco, m_ssql, "idParentesco", "nombre");
            ddlParentesco.Items.Insert(0, new ListItem("Seleccione un parentesco", "0"));

            lblTitulo.Visible = false;
            lblNumero.Visible = false;
            m_ssql = null;
            oUtil = null;
        }

    

     


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
               Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
               if (id.Value!="") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse (id.Value));
                 Guardar(oRegistro);

                if (Context.Items["Desde"] != null)
                {
                    Context.Items.Add("Desde", Context.Items["Desde"].ToString());
                    Context.Items.Add("idServicio", idServicio.Value);
                    Server.Transfer("CasoResultado3.aspx");
                }
                else
                {
                    Context.Items.Add("idServicio", idServicio.Value);
                    Server.Transfer("CasoList.aspx");
                }
                //Response.Redirect("CasoList.aspx?idServicio="+ Context.Items["idServicio"].ToString(), false);
            }
        }

        private void Guardar(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
          
       
            Usuario oUser = new Usuario();
            oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Nombre = txtNombre.Text;
            //Regex.Replace(txtNombre.Text, @"[^0-9A-Za-z]", "", RegexOptions.None);

            if (id.Value == "")
            {// solo se graba en el alta
                oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
                oRegistro.FechaRegistro = DateTime.Now;
            }

            if ((oRegistro.IdUsuarioCarga == 0) && (oRegistro.IdUsuarioValida == 0))
            {
                oRegistro.Objetivo = "";
                oRegistro.Muestra = "";
                oRegistro.Resultado = "";
                oRegistro.Conclusion = "";
                oRegistro.Metodo = "";
                oRegistro.Amplificacion = "";
                oRegistro.Analisis = "";
                oRegistro.Software = "";
                oRegistro.Analisis = "";
                oRegistro.Marcoestudio = "";

                oRegistro.FechaCarga = DateTime.Parse("01/01/1900");
                oRegistro.IdUsuarioCarga = 0;
                oRegistro.FechaValida = DateTime.Parse("01/01/1900");
                oRegistro.IdUsuarioValida = 0;
            }
            oRegistro.Save();        
            GuardarDetalle(oRegistro);
           
         oRegistro.GrabarAuditoria("Modifica datos de caso", oUser.IdUsuario, "");



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
                    oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Desvinculado de caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
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

                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));                   
            }
         
        }

        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {
            ///Si encuentra el codigo ingresado muestra el item en el combo
            try {
                //Protocolo oItem = new Protocolo();
                //oItem = (Protocolo)oItem.Get(typeof(Protocolo), "Numero", txtCodigo.Text, "Baja", false, "IdEfector", oUser.IdEfector);

               
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Protocolo));
                crit.Add(Expression.Eq("Numero", txtCodigo.Text));
                crit.Add(Expression.Eq("Baja", false));
                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));

                Protocolo oItem = (Protocolo)crit.UniqueResult();               

                if (oItem != null)
                { 


                    //if (Context.Items["idServicio"].ToString() == "6")
                    if (idServicio.Value == "6")
                    {
                        if (oItem.IdTipoServicio.IdTipoServicio == 6)
                        {
                            lblPaciente.Text = oItem.IdPaciente.Apellido + " " + oItem.IdPaciente.Nombre;
                            lblPaciente.ForeColor = System.Drawing.Color.Black;
                            btnAgregar.Enabled = true;
                        }
                        else
                        {
                            lblPaciente.Text = "error numero de protocolo. Verifique";
                            lblPaciente.ForeColor = System.Drawing.Color.Red;
                            btnAgregar.Enabled = false;
                        }
                    }

                    if (idServicio.Value == "3")
                    //if (Context.Items["idServicio"].ToString() == "3")//medula
                    {
                        if ((oItem.IdTipoServicio.IdTipoServicio == 3) && (oItem.Estado >= 2)) // controla que el protocolo esté terminado con resultados.
                        {
                            lblPaciente.Text = oItem.IdPaciente.Apellido + " " + oItem.IdPaciente.Nombre;
                            lblPaciente.ForeColor = System.Drawing.Color.Black;
                            btnAgregar.Enabled = true;
                        }
                        else
                        {
                            lblPaciente.Text = "error número de protocolo o protocolo no terminado. Verifique";
                            lblPaciente.ForeColor = System.Drawing.Color.Red;
                            btnAgregar.Enabled = false;
                        }
                    }

                }
                btnAgregar.UpdateAfterCallBack = true;
                lblPaciente.UpdateAfterCallBack = true;
            }
            catch (Exception ex)
            {
                string exception = "";
              
                exception = ex.Message + "<br>";
                lblPaciente.Text = "error número de protocolo. Verifique";

            }
        }

      


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarDeterminaciones();
        }

        private void AgregarDeterminaciones()
        {
            ///Agregar a la tabla las determinaciones para mostrarlas en el gridview

            bool existe = false;
            if (ddlParentesco.SelectedValue != "0")
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                //Primero verifica que no exista el item en la lista
                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    if (txtCodigo.Text.Trim() == dtDeterminaciones.Rows[i][0].ToString())
                        existe = true;
                }
                if (!existe)
                {
                    DataRow row = dtDeterminaciones.NewRow();
                    row[0] = txtCodigo.Text.Trim(); // la clave numero de protocolo: nopuede repetirse
                    row[1] = ddlParentesco.SelectedValue;
                    row[2] = ddlParentesco.SelectedItem.Text;
                    row[3] = txtCodigo.Text + " - " + lblPaciente.Text;
                    row[4] = "";
                    dtDeterminaciones.Rows.Add(row);

                    Session.Add("Tabla1", dtDeterminaciones);
                    gvLista.DataSource = dtDeterminaciones;
                    gvLista.DataBind();
                }
                else
                {
                    lblPaciente.Text = "El protocolo ya fue ingresado a la lista";
                    lblPaciente.ForeColor = System.Drawing.Color.Red;
                }
                btnAgregar.Enabled = false; btnAgregar.UpdateAfterCallBack = true;
                gvLista.UpdateAfterCallBack = true;

                txtCodigo.Text = ""; lblPaciente.Text = "";
                ddlParentesco.SelectedValue = "0";
                lblPaciente.UpdateAfterCallBack = true;
                txtCodigo.UpdateAfterCallBack = true;
                ddlParentesco.UpdateAfterCallBack = true;

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
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdEliminar.CommandArgument = dtDeterminaciones.Rows[e.Row.RowIndex][0].ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                }

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
            Context.Items.Add("idServicio", idServicio.Value);

            Server.Transfer("CasoList.aspx");
            //Response.Redirect("CasoList.aspx?idServicio=" + Request["idServicio"].ToString(), false);
        }

        protected void btnResultados_Click(object sender, EventArgs e)
        {
            Context.Items.Add("idServicio", idServicio.Value);
            Server.Transfer("CasoResultado3.aspx");
            //Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + Request["id"].ToString() , false);
        }
        
        protected void btnResultados0_Click(object sender, EventArgs e)
        {
            Context.Items.Add("id", id.Value);
            Context.Items.Add("idServicio", idServicio.Value);
            Context.Items.Add("Desde", "Carga");
            Server.Transfer("CasoResultado3.aspx");
            //Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + Request["id"].ToString() + "&Desde=Carga", false);
        }

        protected void btnAgregar0_Click(object sender, EventArgs e)
        {
            Session["idCaso"] = id.Value;

            //Response.Redirect("../Protocolos/Default2.aspx?idServicio=6&idUrgencia=0&idCaso=" + id.Value , false);

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            if (oRegistro != null)
            {
                /// Debe ir a Default2.aspx con la restriccion de
                Response.Redirect("../Protocolos/Default2.aspx?idServicio=6&idUrgencia=0&idCaso=" + id.Value + "&idTipoCaso=" + oRegistro.IdTipoCaso.ToString(), false);
                ///Response.Redirect("../Protocolos/ProtocoloEditForense.aspx?idPaciente=-1&Operacion=Alta&idServicio=6&idUrgencia=0&idCaso=" + oRegistro.IdCasoFiliacion.ToString(), false);
            }
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {

        }

       
    }
}
