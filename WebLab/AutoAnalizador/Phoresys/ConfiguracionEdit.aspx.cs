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

namespace WebLab.AutoAnalizador.Phoresys
{
    public partial class ConfiguracionEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Config. Phoresys Hidrasys");
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

        private object LeerDatos()
        {
            string m_strSQL = @" SELECT     M.idPhoresysItem, I.codigo, I.nombre, M.idPhoresys, M.habilitado as Habilitado
                                 FROM  LAB_PhoresysItem AS M 
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

            string m_ssql = "select idArea, nombre from Lab_Area where baja=0 and idtiposervicio=1 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");

            CargarItem();

            m_ssql = null;
            oUtil = null;
        }


        private void CargarItem()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item 
                where baja=0 AND idEfector=idEfectorDerivacion and idCategoria=0 and idArea=" + ddlArea.SelectedValue +
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
            PhoresysItem oDetalle = new PhoresysItem();
            oDetalle.IdPhoresys = ddlItemEquipo.SelectedValue;
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

            PhoresysItem oItem = new PhoresysItem();
            oItem = (PhoresysItem)oItem.Get(typeof(PhoresysItem), "IdItem", int.Parse(ddlItem.SelectedValue));
            if (oItem == null)
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(PhoresysItem));
                crit.Add(Expression.Eq("IdPhoresys", ddlItemEquipo.SelectedValue));                
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
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                PhoresysItem oRegistro = new PhoresysItem();
                oRegistro = (PhoresysItem)oRegistro.Get(typeof(PhoresysItem), int.Parse(e.CommandArgument.ToString()));
                oRegistro.Delete();
                CargarGrilla();
            }

        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            PhoresysItem oRegistro = new PhoresysItem();
            oRegistro = (PhoresysItem)oRegistro.Get(typeof(PhoresysItem), i_id);
            oRegistro.Habilitado = chkStatus.Checked;
            oRegistro.Save();


        }
        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PrincipalSysmex.aspx", false);
        }

    }
}
