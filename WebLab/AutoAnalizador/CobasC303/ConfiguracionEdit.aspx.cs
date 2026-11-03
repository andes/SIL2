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
using System.Text;
using System.IO;
using Business.Data;

namespace WebLab.AutoAnalizador.CobasC303
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
                VerificaPermisos("Config. CobasC303-C311-E411");
                CargarListas();
                CargarGrilla();
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

        private DataTable LeerDatos()
        { string m_strSQL = "";
            if (ddlEquipo.SelectedValue== "CobasC303")
              m_strSQL = @" SELECT     M.idCobasC303Item as id, I.codigo, I.nombre, M.ItemCobas as idEquipo, M.habilitado as Habilitado
                                 FROM  LAB_CobasC303Item AS M with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) ON M.idItemsil = I.idItem Order by I.nombre ";


            if (ddlEquipo.SelectedValue == "CobasC311")
                  m_strSQL = @" SELECT     M.idCobasC311Item as id, I.codigo, I.nombre, M.ItemCobas as idEquipo, M.habilitado as Habilitado
                                 FROM  LAB_CobasC311Item AS M with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) ON M.idItemsil = I.idItem Order by I.nombre ";


            if (ddlEquipo.SelectedValue == "CobasE411")
                m_strSQL = @" SELECT     M.idCobasE411Item as id, I.codigo, I.nombre, M.ItemCobas as idEquipo, M.habilitado as Habilitado
                                 FROM  LAB_CobasE411Item AS M with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) ON M.idItemsil = I.idItem Order by I.nombre ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        private DataTable LeerDatosExcel()
        {
            string m_strSQL = "";
            if (ddlEquipo.SelectedValue == "CobasC303")
                  m_strSQL = @" SELECT   I.codigo as [Codigo SIL], I.nombre as [Descripcion],  
                                 case when M.habilitado=1 then 'Si' else 'No' end as Habilitado
                                 FROM  LAB_CobasC303Item AS M with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) 
                                    ON M.idItemsil = I.idItem Order by I.nombre ";

            if (ddlEquipo.SelectedValue == "CobasC311")
                m_strSQL = @" SELECT   I.codigo as [Codigo SIL], I.nombre as [Descripcion],  
                                 case when M.habilitado=1 then 'Si' else 'No' end as Habilitado
                                 FROM  LAB_CobasC311Item AS M with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) 
                                    ON M.idItemsil = I.idItem Order by I.nombre ";

            if (ddlEquipo.SelectedValue == "CobasE411")
                m_strSQL = @" SELECT   I.codigo as [Codigo SIL], I.nombre as [Descripcion],  
                                 case when M.habilitado=1 then 'Si' else 'No' end as Habilitado
                                 FROM  LAB_CobasE411Item AS M with (nolock)
                                 INNER JOIN LAB_Item AS I with (nolock) 
                                    ON M.idItemsil = I.idItem Order by I.nombre ";


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            
            return Ds.Tables[0];
        }

        private void CargarListas()
        {

            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "select idArea, nombre from Lab_Area with (nolock)  where baja=0  order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);            
            CargarItem();            
            m_ssql = null;
            oUtil = null;
        }
        
        private void GuardarDetalleConfiguracion()
        {
            if (ddlEquipo.SelectedValue == "CobasC303")
            {
                CobasC303Item oDetalle = new CobasC303Item();
                oDetalle.ItemCobas = ddlItem.SelectedValue;
                oDetalle.IdItemSil = int.Parse(ddlItem.SelectedValue);
                oDetalle.Habilitado = true;
                oDetalle.Save();
            }
            if (ddlEquipo.SelectedValue == "CobasC311")
            {
                CobasC311Item oDetalle = new CobasC311Item();
                oDetalle.ItemCobas = ddlItem.SelectedValue;
                oDetalle.IdItemSil = int.Parse(ddlItem.SelectedValue);
                oDetalle.Habilitado = true;
                oDetalle.Save();
            }
            if (ddlEquipo.SelectedValue == "CobasE411")
            {
                CobasE411Item oDetalle = new CobasE411Item();
                oDetalle.ItemCobas = ddlItem.SelectedValue;
                oDetalle.IdItemSil = int.Parse(ddlItem.SelectedValue);
                oDetalle.Habilitado = true;
                oDetalle.Save();
            }
            //   if (ddlEquipo.SelectedValue == "CobasC311")
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
            string hay = "";
            if (ddlEquipo.SelectedValue == "CobasC303")
            {   CobasC303Item oItem = new CobasC303Item();
            oItem = (CobasC303Item)oItem.Get(typeof(CobasC303Item), "IdItemSil", int.Parse(ddlItem.SelectedValue));
            if (oItem != null)                            
                hay = "Ya existe una configuración para el análisis seleccionado";
                }
            if (ddlEquipo.SelectedValue == "CobasC311")
            {
                CobasC311Item oItem = new CobasC311Item();
                oItem = (CobasC311Item)oItem.Get(typeof(CobasC311Item), "IdItemSil", int.Parse(ddlItem.SelectedValue));
                if (oItem != null)
                    hay = "Ya existe una configuración para el análisis seleccionado";
            }

            if (ddlEquipo.SelectedValue == "CobasE411")
            {
                CobasE411Item oItem = new CobasE411Item();
                oItem = (CobasE411Item)oItem.Get(typeof(CobasE411Item), "IdItemSil", int.Parse(ddlItem.SelectedValue));
                if (oItem != null)
                    hay = "Ya existe una configuración para el análisis seleccionado";
            }
            return hay;
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";
                CheckBox chkStatus = (CheckBox)e.Row.Cells[2].Controls[1];
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
                if (ddlEquipo.SelectedValue == "CobasC303")
                {
                    CobasC303Item oRegistro = new CobasC303Item();
                    oRegistro = (CobasC303Item)oRegistro.Get(typeof(CobasC303Item), int.Parse(e.CommandArgument.ToString()));
                    oRegistro.Delete();
                }
                if (ddlEquipo.SelectedValue == "CobasC311")
                {
                    CobasC311Item oRegistro = new CobasC311Item();
                    oRegistro = (CobasC311Item)oRegistro.Get(typeof(CobasC311Item), int.Parse(e.CommandArgument.ToString()));
                    oRegistro.Delete();
                }

                if (ddlEquipo.SelectedValue == "CobasE411")
                {
                    CobasE411Item oRegistro = new CobasE411Item();
                    oRegistro = (CobasE411Item)oRegistro.Get(typeof(CobasE411Item), int.Parse(e.CommandArgument.ToString()));
                    oRegistro.Delete();
                }
                CargarGrilla();
            }

        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());
            if (ddlEquipo.SelectedValue == "CobasC303")
            {
                CobasC303Item oRegistro = new CobasC303Item();
                oRegistro = (CobasC303Item)oRegistro.Get(typeof(CobasC303Item), i_id);
                oRegistro.Habilitado = chkStatus.Checked;
                oRegistro.Save();
            }

            if (ddlEquipo.SelectedValue == "CobasC311")
            {
                CobasC311Item oRegistro = new CobasC311Item();
                oRegistro = (CobasC311Item)oRegistro.Get(typeof(CobasC311Item), i_id);
                oRegistro.Habilitado = chkStatus.Checked;
                oRegistro.Save();
            }
            if (ddlEquipo.SelectedValue == "CobasE411")
            {
                CobasE411Item oRegistro = new CobasE411Item();
                oRegistro = (CobasE411Item)oRegistro.Get(typeof(CobasE411Item), i_id);
                oRegistro.Habilitado = chkStatus.Checked;
                oRegistro.Save();
            }
        }
        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
        //    Response.Redirect("../PrincipalSysmex.aspx", false);
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarItem();
        }

        private void CargarItem()
        {           
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item I with (nolock)
                where baja=0 AND idArea=" + ddlArea.SelectedValue +
                       " order by nombre";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable tabla = LeerDatosExcel();
            if (ddlEquipo.SelectedValue == "CobasC303")
                Utility.ExportDataTableToXlsx(tabla, "CobasC303_SIL");
            if (ddlEquipo.SelectedValue == "CobasC311")
                Utility.ExportDataTableToXlsx(tabla, "CobasC311_SIL");
            if (ddlEquipo.SelectedValue == "CobasE411")
                Utility.ExportDataTableToXlsx(tabla, "CobasE411_SIL");

        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}
