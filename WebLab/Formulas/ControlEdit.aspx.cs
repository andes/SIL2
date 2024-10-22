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
using NHibernate.Expression;
using NHibernate;

namespace WebLab.Formulas
{
    public partial class ControlEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Formulas y Controles");
                CargarListas();
                if (Request["id"] != null)
                    MostrarDatos();
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
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void MostrarDatos()
        {
            Formula oRegistro = new Formula();
            oRegistro = (Formula)oRegistro.Get(typeof(Formula), int.Parse(Request["id"]));
            //txtNombre.Text = oRegistro.Nombre;
            txtFormula.Text = oRegistro.ContenidoFormula;
            ddlOperacion.SelectedValue = oRegistro.Operacion.ToString();
            txtFormulaControl.Text = oRegistro.FormulaControl;
            
            
            ddlItem.SelectedValue = oRegistro.IdItem.IdItem.ToString();
            txtCodigo.Text = oRegistro.IdItem.Codigo;
        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string m_ssql = "select idItem, nombre from Lab_Item where baja=0 and idTipoResultado<=1 order by nombre";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione un analisis", "0"));

            
            m_ssql = null;
            oUtil = null;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Formula oRegistro = new Formula();
                if (Request["id"] != null) oRegistro = (Formula)oRegistro.Get(typeof(Formula), int.Parse(Request["id"]));
                Guardar(oRegistro);
                Response.Redirect("FormulaList.aspx", false);
            }



        }

        private void Guardar(Formula oRegistro)
        {


            Item oItem = new Item();
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //Efector oEfector = new Efector();

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);


            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.IdItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
            //oRegistro.Nombre = txtNombre.Text;

            oRegistro.ContenidoFormula = txtFormula.Text.Replace("\r\n", ""); 
            oRegistro.Operacion =int.Parse( ddlOperacion.SelectedValue);
            oRegistro.FormulaControl = txtFormulaControl.Text.Replace("\r\n", ""); 
            oRegistro.IdTipoFormula = 2;

            oRegistro.Sexo = "I";
            oRegistro.EdadDesde = 0;
            oRegistro.EdadHasta = 120;
            oRegistro.UnidadEdad = -1;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();

        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "")
            {
                Item oItem = new Item();
                ISession m_session = NHibernateHttpModule.CurrentSession;
          
                ICriteria crit = m_session.CreateCriteria(typeof(Item));

                crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
                crit.Add(Expression.Eq("Baja", false));
                crit.Add(Expression.Between("IdTipoResultado", 0,1));
              

                oItem = (Item)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlItem.SelectedValue = oItem.IdItem.ToString();
              
                }
                else
                {
                    lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no existe. ";
                    ddlItem.SelectedValue = "0";
                    txtCodigo.Text = "";
                 
                    txtCodigo.UpdateAfterCallBack = true;
                 
                }

                ddlItem.UpdateAfterCallBack = true;
              
                lblMensaje.UpdateAfterCallBack = true;
            }
            else
            {
                ddlItem.SelectedValue = "0";
            
                ddlItem.UpdateAfterCallBack = true;
               
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
                txtCodigo.Text = oItem.Codigo;
            
            }
            else
            {
                txtCodigo.Text = "";
         
            }
        
            txtCodigo.UpdateAfterCallBack = true;
          
        }
    }
}
