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
using Business.Data.Laboratorio;
using Business;
using Business.Data;

namespace WebLab.Areas
{
    public partial class AreaList : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Area");
                CargarGrilla();
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
             //   Utility oUtil = new Utility();
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
            string m_strSQL = @" select A.idArea, T.nombre as tipoServicio, A.nombre  
                                from Lab_Area A with (nolock)
                                inner join Lab_TipoServicio T with (nolock) on T.idTipoServicio= A.idtipoServicio 
                                where A.baja=0  
                                order by A.nombre";
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
            Response.Redirect("AreaEdit.aspx");
        }

     

      

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                string MyURL;

                MyURL = "AreaEdit.aspx?id=" +e.CommandArgument.ToString();
                Response.Redirect(MyURL);
            }
           // Response.Redirect("AreaEdit.aspx?id=" + e.CommandArgument);
           if (e.CommandName == "Eliminar")
           {
               Eliminar(e.CommandArgument);
               CargarGrilla();
           }
        }


        private void Eliminar(object p)
        {
            Area oRegistro = new Area();
            oRegistro = (Area)oRegistro.Get(typeof(Area), int.Parse(p.ToString()));
            if (!oRegistro.tieneVinculados())
            {
                Usuario oUser = new Usuario();
                oRegistro.Baja = true;
                oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();
                estatus.Text = "";
            }
            else
                estatus.Text = "No es posible eliminar. Tiene registros vinculados";
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }
            }  
        }
    }
}
