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

namespace WebLab.Items
{
    public partial class ItemGrupo : System.Web.UI.Page
    {

        DataTable dtDeterminaciones; //tabla para determinaciones
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            //   if (Request["id"] != null)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Rutinas");
                    InicializarTablas();
                    CargarListas();
                    if (Request["id"] != null)
                        MostrarDatos();

                }
                else Response.Redirect("../FinSesion.aspx", false);

                
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
            Rutina oRegistro = new Rutina();
            oRegistro = (Rutina)oRegistro.Get(typeof(Rutina), int.Parse(Request["id"]));

            txtNombre.Text = oRegistro.Nombre;
            ddlServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
            chkPeticionElectronica.Checked = oRegistro.PeticionElectronica;

            HabilitarDeterminaciones();
            ///Agregar a la tabla las determinaciones para mostrarlas en el gridview                             
            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            DetalleRutina oDetalle = new DetalleRutina();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleRutina));
            crit.Add(Expression.Eq("IdRutina", oRegistro));

            IList items = crit.List();            
            foreach (DetalleRutina oDet in items)
            {             
                    DataRow row = dtDeterminaciones.NewRow();
                    row[0] = oDet.IdItem.IdItem.ToString();
                    row[1] = oDet.IdItem.Nombre;                    
                    row[2] = "";
                    dtDeterminaciones.Rows.Add(row);
            
            }
            Session.Add("Tabla1", dtDeterminaciones);
            gvLista.DataSource = dtDeterminaciones;
            gvLista.DataBind();

        }
        private void InicializarTablas()
        {
            ///Inicializa las sesiones para las tablas de diagnosticos y de determinaciones
            if (Session["Tabla1"] != null) Session["Tabla1"] = null;
            //if (Session["Tabla2"] != null) Session["Tabla2"] = null;

            dtDeterminaciones = new DataTable();


            dtDeterminaciones.Columns.Add("id");
            dtDeterminaciones.Columns.Add("nombre");            
            dtDeterminaciones.Columns.Add("eliminar");


            Session.Add("Tabla1", dtDeterminaciones);


        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlServicio.Items.Insert(0, new ListItem("Seleccione un servicio", "0"));

          //  HabilitarDeterminaciones();
            m_ssql = null;
            oUtil = null;
        }

    

        private void CargarItem()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            Utility oUtil = new Utility();   ///Carga de combos de Areas         
            string m_ssql = @"select I.idItem, I.nombre from Lab_item I (nolock)
                 INNER JOIN lAB_AREA a (nolock) on A.idArea= I.idArea
             where A.baja=0 and  I.baja=0  AND (I.tipo= 'P') ";

            if (ddlServicio.SelectedValue != "5")
                m_ssql += " and A.idTipoServicio= " + ddlServicio.SelectedValue;
            
            m_ssql+= " order by I.nombre"; 

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione un item", "0"));
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Rutina oRegistro = new Rutina();
                if (Request["id"] != null) oRegistro = (Rutina)oRegistro.Get(typeof(Rutina), int.Parse(Request["id"]));
                Guardar(oRegistro);
                Response.Redirect("RutinaList.aspx", false);
            }
        }

        private void Guardar(Rutina oRegistro)
        {
          
            //Item oItemGrupo = new Item();
            TipoServicio oServicio = new TipoServicio();
            //Efector oEfector = new Efector();
            Usuario oUser = new Usuario();
            oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.IdEfector = oUser.IdEfector;

            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdTipoServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(ddlServicio.SelectedValue));
            oRegistro.IdArea = 0;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.PeticionElectronica = chkPeticionElectronica.Checked;
            oRegistro.Save();        
            GuardarDetalle(oRegistro);
            Response.Redirect("RutinaEdit.aspx", false);


        }

        private void GuardarDetalle(Rutina oRegistro)
        {
            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleRutina));
            crit.Add(Expression.Eq("IdRutina", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetalleRutina oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }

            /////Crea nuevamente los detalles.
            for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
            {
                DetalleRutina oDetalle = new DetalleRutina();
                Item oItem = new Item();
                oDetalle.IdRutina = oRegistro;
                oDetalle.IdEfector = oRegistro.IdEfector;
                oDetalle.IdItem = (Item)oItem.Get(typeof(Item), int.Parse(dtDeterminaciones.Rows[i][0].ToString()));
                oDetalle.Save();                           
            }
         
        }

        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {
            ///Si encuentra el codigo ingresado muestra el item en el combo
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", txtCodigo.Text, "Baja", false);
            if (oItem != null)
            {
                if ((oItem.IdArea.IdTipoServicio.IdTipoServicio == int.Parse(ddlServicio.SelectedValue)) || (int.Parse(ddlServicio.SelectedValue)==5 ))
                { if (oItem.Tipo=="P")   ddlItem.SelectedValue = oItem.IdItem.ToString();
                }
            }

            ddlItem.UpdateAfterCallBack = true;
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ///////Con la selección del item se muestra el codigo
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
            if (oItem != null)
            {
                txtCodigo.Text = oItem.Codigo;
                txtCodigo.UpdateAfterCallBack = true;
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
            if (ddlItem.SelectedValue != "0")
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                //Primero verifica que no exista el item en la lista
                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    if (ddlItem.SelectedValue == dtDeterminaciones.Rows[i][0].ToString())
                        existe = true;
                }
                if (!existe)
                {
                    DataRow row = dtDeterminaciones.NewRow();
                    row[0] = ddlItem.SelectedValue;
                    row[1] = ddlItem.SelectedItem.Text;                    
                    row[2] = "";
                    dtDeterminaciones.Rows.Add(row);

                    Session.Add("Tabla1", dtDeterminaciones);
                    gvLista.DataSource = dtDeterminaciones;
                    gvLista.DataBind();
                }

                gvLista.UpdateAfterCallBack = true;

                txtCodigo.Text = "";
                ddlItem.SelectedValue = "0";
                txtCodigo.UpdateAfterCallBack = true;
                ddlItem.UpdateAfterCallBack = true;

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
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[1].Controls[1];
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

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarDeterminaciones();
        }

        private void HabilitarDeterminaciones()
        {
            if (ddlServicio.SelectedValue != "0")
            {
                ddlServicio.Enabled = false;
                txtCodigo.Enabled = true;
                ddlItem.Enabled = true;

                CargarItem();
                ddlServicio.UpdateAfterCallBack = true;
                txtCodigo.UpdateAfterCallBack = true;
                ddlItem.UpdateAfterCallBack = true;


            }
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

    }
}
