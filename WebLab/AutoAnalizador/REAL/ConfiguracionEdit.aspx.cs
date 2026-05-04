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

namespace WebLab.AutoAnalizador.REAL
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
                VerificaPermisos("Config. REAL");
                CargarCombos();
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
        {
            string m_strSQL = @" SELECT     M.idrealitem, I.codigo, I.nombre, M.idreal, M.habilitado as Habilitado
                                 FROM  lab_realitem AS M 
                                 INNER JOIN LAB_Item AS I ON M.idItem = I.idItem Order by I.nombre ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            //   CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }
        private DataTable LeerDatosExcel()
        {
            string m_strSQL = @" SELECT   I.codigo as [Codigo SIL], I.nombre as [Descripcion], M.idreal as [Codigo Real], M.habilitado as Habilitado
                                 FROM  lab_realitem AS M 
                                 INNER JOIN LAB_Item AS I ON M.idItem = I.idItem Order by I.nombre ";

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
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "select idArea, nombre from Lab_Area where baja=0 and idtiposervicio=3 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);

            

            CargarItem();
            //ddlArea.Items.Insert(0, new ListItem("Seleccione Area", "0"));


            m_ssql = null;
            oUtil = null;
        }






        private void GuardarDetalleConfiguracion()
        {
            RealItem oDetalle = new RealItem();
            oDetalle.IdReal = txtIDEquipo.Text;
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

            RealItem oItem = new RealItem();
            oItem = (RealItem)oItem.Get(typeof(RealItem), "IdItem", int.Parse(ddlItem.SelectedValue));
            if (oItem == null)
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(RealItem));
                crit.Add(Expression.Eq("IdReal", txtIDEquipo.Text));                
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

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[4].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";


                CheckBox chkStatus = (CheckBox)e.Row.Cells[3].Controls[1];
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
                RealItem oRegistro = new RealItem();
                oRegistro = (RealItem)oRegistro.Get(typeof(RealItem), int.Parse(e.CommandArgument.ToString()));
                oRegistro.Delete();

                CargarGrilla();

            }

        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            RealItem oRegistro = new RealItem();
            oRegistro = (RealItem)oRegistro.Get(typeof(RealItem), i_id);
            oRegistro.Habilitado = chkStatus.Checked;
            oRegistro.Save();


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
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item I
                where baja=0 AND idArea=" + ddlArea.SelectedValue +
                       " order by nombre";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                dataTableAExcel(LeerDatosExcel(), "REAL_SIL");
        }


        private void dataTableAExcel(DataTable tabla, string nombreArchivo)
        {
            if (tabla.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Page pagina = new Page();
                HtmlForm form = new HtmlForm();
                GridView dg = new GridView();
                dg.EnableViewState = false;
                dg.DataSource = tabla;
                dg.DataBind();
                pagina.EnableEventValidation = false;
                pagina.DesignerInitialize();
                pagina.Controls.Add(form);
                form.Controls.Add(dg);
                pagina.RenderControl(htw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
