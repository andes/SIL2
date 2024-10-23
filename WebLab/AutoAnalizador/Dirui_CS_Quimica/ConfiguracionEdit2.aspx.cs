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
using System.Data.SqlClient;
using Business;
using Business.Data.AutoAnalizador;
using NHibernate;
using NHibernate.Expression;
using Business.Data;

namespace WebLab.AutoAnalizador.Dirui_CS_Quimica
{
    public partial class ConfiguracionEdit2 : System.Web.UI.Page
    {
      
      
        Usuario oUser = new Usuario();
        //   Item oItem = new Item();

        protected void Page_PreInit(object sender, EventArgs e)
        {  
            if (Session["idUsuario"] != null)
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //     oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1, "IdEfector", oEfector);
            else
                Response.Redirect("../FinSesion.aspx", false);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Config. DIRUI");
                CargarCombos();
                CargarGrilla();
                if (oUser.IdEfector.IdEfector == 227)
                    btnGuardar.Visible = true;
                else btnGuardar.Visible = false;

            }
        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            string m_strSQL = @" SELECT     M.idDIRUIItem, I.codigo, I.nombre, M.idDIRUI, M.habilitado as Habilitado, M.prefijo
                                 FROM  LAB_DIRUIItem AS M 
                                 INNER JOIN LAB_Item AS I ON M.idItem = I.idItem Order by I.nombre ";

            DataSet Ds = new DataSet();
            //  SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            //   CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }

        private void CargarCombos()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "select idArea, nombre from Lab_Area where baja=0 and idtiposervicio=1 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre",connReady);

            CargarItem();

            m_ssql = null;
            oUtil = null;
        }


        private void CargarItem()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item 
                where baja=0 AND idEfector=idEfectorDerivacion and idCategoria=0 and idArea=" + ddlArea.SelectedValue +
                       " order by nombre";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarItem();
        }




        private void GuardarDetalleConfiguracion()
        {
            InccaItem oDetalle = new InccaItem();
            oDetalle.IdIncca= txtOrden.Text.Trim();
          //  oDetalle.Prefijo = txtPrefijo.Text.Trim();
            oDetalle.IdItem = int.Parse(ddlItem.SelectedValue);
            oDetalle.Habilitado = true;
            oDetalle.Save();
        }


        protected void btnGuardar_Click2(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string validacion = existe();
                if (validacion == "")
                {
                    lblMensajeValidacion.Text = "";
                    GuardarDetalleConfiguracion();
                    CargarGrilla();
                }
                else
                    lblMensajeValidacion.Text = validacion;
            }
        }

        private string existe()
        {
            //////////////////////////////////////////////////////////////////////////////////////////
            ///Verifica de que no exista un item para la combincacion orden y tipo de muestra
            //////////////////////////////////////////////////////////////////////////////////////////
            string hay = "";

            InccaItem oItem = new InccaItem();
            oItem = (InccaItem)oItem.Get(typeof(InccaItem), "IdItem", int.Parse(ddlItem.SelectedValue),"Prefijo", txtPrefijo.Text.Trim());
            if (oItem == null)
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(InccaItem));
                crit.Add(Expression.Eq("IdIncca",txtOrden.Text.Trim()));
                crit.Add(Expression.Eq("Prefijo", txtPrefijo.Text.Trim()));      
                IList detalle = crit.List();
                if (detalle.Count > 0)
                    hay = "Ya existe una vinculación para el ID de muestra seleccionado. Verifique.";
            }
            else
                hay = "Ya existe una configuración para el análisis seleccionado";

            return hay;
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[5].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

             
                    CheckBox chkStatus = (CheckBox)e.Row.Cells[4].Controls[1];
                if (oUser.IdEfector.IdEfector == 227)
                {
                    CmdEliminar.Visible = true;
                    chkStatus.Visible = true;

                }
                else
                {
                    CmdEliminar.Visible = false;
                    chkStatus.Enabled = false;
                }

            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                InccaItem oRegistro = new InccaItem();
                oRegistro = (InccaItem)oRegistro.Get(typeof(InccaItem), int.Parse(e.CommandArgument.ToString()));
                oRegistro.Delete();
                CargarGrilla();
            }
        }

        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            InccaItem oRegistro = new InccaItem();
            oRegistro = (InccaItem)oRegistro.Get(typeof(InccaItem), i_id);
            oRegistro.Habilitado = chkStatus.Checked;
            oRegistro.Save();


        }
        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PrincipalSysmex.aspx", false);
        }

    }
}
