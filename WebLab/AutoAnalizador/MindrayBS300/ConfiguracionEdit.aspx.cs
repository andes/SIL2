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
using NHibernate.Expression;
using NHibernate;
using Business.Data;
using System.Data.SqlClient;
using Business.Data.AutoAnalizador;

namespace WebLab.AutoAnalizador.MindrayBS300
{
    public partial class ConfiguracionEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Config. Mindray BS-300");
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
            string m_strSQL = @" SELECT   M.idMindrayItem, I.codigo, I.nombre, M.tipoMuestra, M.idMindray, M.prefijo, M.Habilitado as Habilitado
                                 FROM  LAB_MindrayItem AS M (nolock)
                                 INNER JOIN LAB_Item AS I  (nolock) ON M.idItem = I.idItem Order by M.idMindray, M.tipoMuestra ";

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

            string m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0 and idtiposervicio=1 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");

            CargarItem();

            m_ssql = null;
            oUtil = null;
       }


        private void CargarItem()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item (nolock)
                where baja=0 
                AND idEfector=idEfectorDerivacion 
                and idCategoria=0 and idArea=" + ddlArea.SelectedValue +
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
            MindrayItem oDetalle = new MindrayItem();
            oDetalle.IdMindray =int.Parse( txtOrden.Text);
            oDetalle.IdItem = int.Parse( ddlItem.SelectedValue);
            oDetalle.TipoMuestra = rdbMuestra.SelectedValue;
            oDetalle.Prefijo = txtPrefijo.Text;
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

        private string  existe()
        {   
            //////////////////////////////////////////////////////////////////////////////////////////
            ///Verifica de que no exista un item para la combincacion orden y tipo de muestra
            //////////////////////////////////////////////////////////////////////////////////////////
            string hay = "";

            MindrayItem oItem = new MindrayItem();
            oItem= (MindrayItem)oItem.Get(typeof(MindrayItem), "IdItem",int.Parse(ddlItem.SelectedValue));
            if (oItem == null)
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(MindrayItem));
                crit.Add(Expression.Eq("IdMindray", int.Parse(txtOrden.Text)));
            //    crit.Add(Expression.Eq("IdItem", int.Parse(ddlItem.SelectedValue)));
                crit.Add(Expression.Eq("TipoMuestra", rdbMuestra.SelectedValue));
                crit.Add(Expression.Eq("Prefijo", txtPrefijo.Text));

                IList detalle = crit.List();
                if (detalle.Count > 0)
                    hay = "Ya existe una vinculación para el número de orden y tipo de muestra seleccionado. Verifique.";
            }
            else
                hay = "Ya existe una configuración para el análisis seleccionado";

            return hay;
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[6].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";             
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {          
            if (e.CommandName=="Eliminar")
            {
                MindrayItem oRegistro = new MindrayItem();
                oRegistro = (MindrayItem)oRegistro.Get(typeof(MindrayItem), int.Parse(e.CommandArgument.ToString()));
                oRegistro.Delete();                        
                CargarGrilla();                        
            }            
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PrincipalMindray.aspx", false);
        }


        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;
          
            int i_id= int.Parse( gvLista.DataKeys[row.RowIndex].Value.ToString());
            
            MindrayItem oRegistro = new MindrayItem();
            oRegistro = (MindrayItem)oRegistro.Get(typeof(MindrayItem), i_id);
            oRegistro.Habilitado = chkStatus.Checked;
            oRegistro.Save();                    
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)

        {
           
        }

    }
}
