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

namespace WebLab.AutoAnalizador.CobasC311Config
{
    public partial class ConfiguracionEdit : System.Web.UI.Page
    {
        Usuario oUser = new Usuario();
      
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
                 VerificaPermisos("Config. CobasC311");
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

            /*  string m_strSQL = @" SELECT  C.id, I.codigo, I.nombre, C.idItemCobas, C.idItemSil,C.prefijo,C.tipoMuestra, C.habilitado as Habilitado
                                   FROM  LAB_CobasC311 AS C with (nolock)
                                   INNER JOIN LAB_Item AS I with (nolock) ON C.idItemSil = I.idItem Order by I.nombre ";
                                   */

            string m_strSQL = @" SELECT  C.id, I.codigo, I.nombre, C.ItemCobas, C.idItemSil,C.prefijo,C.tipoMuestra, C.habilitado as Habilitado
                                 FROM  LAB_CobasC311 AS C with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) ON C.idItemSil = I.idItem Order by I.nombre ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            //   CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }

        private void CargarCombos()
        {
            Utility oUtil = new Utility();

            string m_ssql = "select idArea, nombre from Lab_Area with (nolock) where baja=0 and idtiposervicio=1 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");

            CargarItem();

            //m_ssql = null;
            //oUtil = null;

            //Llamo a cargar el combo Item Equipo (cargo los datos de ROCHE)
            m_ssql = "select test,codigo from LAB_CobasC311Determinaciones order by test";
            oUtil.CargarCombo(ddlItemEquipo, m_ssql, "codigo", "test");
            ddlItemEquipo.Items.Insert(0, new ListItem("Seleccione", "0"));
            ddlItemEquipo.UpdateAfterCallBack = true;

        }

        private void CargarItem()
        {

            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item   with (nolock)
                where baja=0 and idCategoria=0 and idArea=" + ddlArea.SelectedValue +
                       " order by nombre";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarItem();
        }
        private void GuardarDetalleConfiguracion()
        {
            
             CobasC311 oDetalle = new  CobasC311();

            oDetalle.ItemCobas = ddlItemEquipo.SelectedItem.Text;
            oDetalle.IdItemCobas = int.Parse(ddlItemEquipo.SelectedValue);
            oDetalle.IdItemSil = int.Parse(ddlItem.SelectedValue);
            oDetalle.Prefijo = this.ddlPrefijo.SelectedValue;
            oDetalle.TipoMuestra = this.ddlTipoMuestra.SelectedItem.ToString();
            oDetalle.Habilitado = true;
         //   oDetalle.CodigoSil = this.ddlItem.SelectedItem.Text.Split('-')[1];
            
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
            ///Verifica que no exista un item para la combinacion orden y tipo de muestra
            //////////////////////////////////////////////////////////////////////////////////////////
            string hay = "";

            CobasC311 oItem = new CobasC311();
            oItem = (CobasC311)oItem.Get(typeof(CobasC311), "IdItemSil", int.Parse(ddlItem.SelectedValue));
            if (oItem == null)
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(CobasC311));
                crit.Add(Expression.Eq("IdItemCobas", Int32.Parse(ddlItemEquipo.SelectedValue)));
                crit.Add(Expression.Eq("TipoMuestra", ddlTipoMuestra.SelectedItem.ToString()));
                crit.Add(Expression.Eq("Prefijo", ddlPrefijo.SelectedValue.ToString()));
                
                IList detalle = crit.List();
                if (detalle.Count > 0)
                    //hay = "Ya existe una vinculación para el ID de muestra seleccionado. Verifique.";
                    hay = "Ya existe esa configuración en la grilla para el ID, tipo y prefijo de muestra";
            }
            else
                hay = "Ya existe una configuración para el análisis seleccionado";

            return hay;
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[7].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";
                CheckBox chkStatus = (CheckBox)e.Row.Cells[6].Controls[1];
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
                CobasC311 oRegistro = new CobasC311();
                oRegistro = (CobasC311)oRegistro.Get(typeof(CobasC311), int.Parse(e.CommandArgument.ToString()));
                oRegistro.Delete();

                CargarGrilla();

            }

        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            CobasC311 oRegistro = new CobasC311();
            oRegistro = (CobasC311)oRegistro.Get(typeof(CobasC311), i_id);
            oRegistro.Habilitado = chkStatus.Checked;
            oRegistro.Save();


        }
     

    }

}