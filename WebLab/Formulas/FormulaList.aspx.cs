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
using Business.Data.Laboratorio;
using Business.Data;

namespace WebLab.Formulas
{
    public partial class FormulaList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Formulas y Controles");
                CargarGrilla();
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
                            btnAgregar.Visible = false;
                            btnAgregarControl.Visible = false;
                        } break;
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
            string m_strSQL = @" select F.idformula, I.nombre as item , F.formula + ' ...' as nombre, 
                 case when F.idtipoFormula=1 then 'Formula' else 'Control' end  as tipo 
                               from Lab_Formula F
                               inner join Lab_Item I on I.idItem= F.idItem
                               where F.baja=0 
                               order by I.nombre";
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

             
             
            return Ds.Tables[0];
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("FormulaEdit.aspx",false);
        }





        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                Redireccionar(e.CommandArgument);
            }
              
            if (e.CommandName == "Eliminar")
            {
                Eliminar(e.CommandArgument);
                CargarGrilla();
            }
        }

        private void Redireccionar(object p)
        {
            Formula oRegistro = new Formula();
            oRegistro = (Formula)oRegistro.Get(typeof(Formula), int.Parse(p.ToString()));
            if (oRegistro.IdTipoFormula==1)
                Response.Redirect("FormulaEdit.aspx?id=" + p.ToString());
            else
                Response.Redirect("ControlEdit.aspx?id=" + p.ToString());

        }


        private void Eliminar(object p)
        {
            Formula oRegistro = new Formula();
            oRegistro = (Formula)oRegistro.Get(typeof(Formula), int.Parse(p.ToString()));
            Usuario oUser = new Usuario();
            oRegistro.Baja = true;
           oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[4].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }
            }
        }

        protected void btnAgregarControl_Click(object sender, EventArgs e)
        {
            Response.Redirect("ControlEdit.aspx", false);
        }
    }
}
