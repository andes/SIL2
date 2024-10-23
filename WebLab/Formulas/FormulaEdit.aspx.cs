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

namespace WebLab.Formulas
{
    public partial class FormulaEdit : System.Web.UI.Page
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
            ddlItem.SelectedValue = oRegistro.IdItem.IdItem.ToString();
            txtCodigo.Text = oRegistro.IdItem.Codigo;

            ddlSexo.SelectedValue = oRegistro.Sexo ;
            txtEdadDesde.Value = oRegistro.EdadDesde.ToString();
            txtEdadHasta.Value = oRegistro.EdadHasta.ToString();
            ddlUnidadEdad.SelectedValue=oRegistro.UnidadEdad.ToString();
            txtFormulaCondicion.Text = oRegistro.CondicionDeterminacion;
            ddlRaza.SelectedValue = oRegistro.IdRaza.ToString();
        }
        

        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string m_ssql = "select idItem, nombre from Lab_Item where baja=0 and idTipoResultado=1 order by nombre";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione un analisis", "0"));

            m_ssql = null;
            oUtil = null;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {   if (Page.IsValid)
            {            
            Formula oRegistro = new Formula();
            if (Request["id"] != null)
                oRegistro = (Formula)oRegistro.Get(typeof(Formula), int.Parse(Request["id"]));
            else
            {
                string popupScript = "<script language='JavaScript'> alert('Verifique que los codigos de analisis esten entre corchetes para poder diferenciar los codigos de los valores numericos.')</script>";
                Page.RegisterClientScriptBlock("PopupScript", popupScript);
            }

            Guardar(oRegistro);
            Response.Redirect("FormulaList.aspx", false);
            }

            
          
        }

        private void Guardar(Formula oRegistro)
        {
            

            Item oItem = new Item();
            Usuario oUser= new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
          

            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.IdItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
            //oRegistro.Nombre = txtNombre.Text;
            oRegistro.ContenidoFormula = txtFormula.Text.Replace("\r\n", "");
            oRegistro.IdTipoFormula = 1;

            oRegistro.Sexo = ddlSexo.SelectedValue;
            
            if (int.Parse(ddlUnidadEdad.SelectedValue)==-1)
            {
                oRegistro.EdadDesde =0;
                oRegistro.EdadHasta = 120;
            }
            else{
            oRegistro.EdadDesde = int.Parse(txtEdadDesde.Value);
            oRegistro.EdadHasta = int.Parse(txtEdadHasta.Value);
            }
            oRegistro.UnidadEdad = int.Parse(ddlUnidadEdad.SelectedValue);
            oRegistro.CondicionDeterminacion = txtFormulaCondicion.Text;
            oRegistro.IdRaza = int.Parse(ddlRaza.SelectedValue);
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
                crit.Add(Expression.Eq("IdTipoResultado", 1));


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

        protected void ddlUnidadEdad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlUnidadEdad.SelectedValue != "-1")
            {
                txtEdadDesde.Disabled = false;
                txtEdadHasta.Disabled = false;   
            
            }
            else
            {
                txtEdadDesde.Disabled = true;
                txtEdadHasta.Disabled = true;             
            }

        }
    }
}
