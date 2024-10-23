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
using System.Data.SqlClient;
using Business.Data.Laboratorio;
using Business.Data;

namespace WebLab.Observaciones
{
    public partial class ObservacionList : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Observaciones");
                    CargarListas();
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
        private void CargarListas()
        {
            Utility oUtil = new Utility();   ///Carga de combos de Areas         
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlTipoServicio, m_ssql, "idTipoServicio", "nombre");
            ddlTipoServicio.Items.Insert(0, new ListItem("Todos", "0"));


            m_ssql = null;
            oUtil = null;
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                // Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnAgregar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        private void CargarGrilla()
        {
            gvLista.AutoGenerateColumns = false;
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            string m_condicion = "";
            if (ddlTipoServicio.SelectedValue != "0") m_condicion += " and O.idTipoServicio=" + ddlTipoServicio.SelectedValue;    
            string m_strSQL = " SELECT idObservacionResultado as idObservacion, O.codigo , O.nombre, TS.nombre as tipoServicio " +
                              " FROM LAB_ObservacionResultado as O " +
                              " INNER JOIN lab_tiposervicio  as TS on TS.idTipoServicio= O.idtipoServicio "+
                              " where O.baja=0 " + m_condicion +
                              " order by O.nombre";
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
            Response.Redirect("ObservacionEdit.aspx");
        }
        
        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {               
                string MyURL = "ObservacionEdit.aspx?id=" + e.CommandArgument.ToString();
                Response.Redirect(MyURL);
            }
            if (e.CommandName == "Eliminar")
            {
                Eliminar(e.CommandArgument);
                CargarGrilla();
            }
        }


        private void Eliminar(object p)
        {
            ObservacionResultado oRegistro = new ObservacionResultado();
            oRegistro = (ObservacionResultado)oRegistro.Get(typeof(ObservacionResultado), int.Parse(p.ToString()));
            if (oRegistro != null)
            {
                Usuario oUser = new Usuario();
                oRegistro.Baja = true;
                oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();
            }
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

        protected void ddlTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
