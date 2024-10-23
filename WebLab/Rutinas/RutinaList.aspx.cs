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

namespace WebLab.Items
{
    public partial class ItemRutinasList : System.Web.UI.Page
    {
       
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            //MiltiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Rutinas");
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
                Utility oUtil = new Utility();
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
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            string m_strSQL = " select R.idRutina, S.nombre as servicio , R.nombre as nombre" +
                              " from Lab_Rutina R with (nolock) " +
                              " INNER JOIN Lab_TipoServicio S with (nolock) on S.idTipoServicio= R.idTipoServicio " +
                              " where R.baja=0 and R.idEfector=" +oUser.IdEfector.IdEfector.ToString() +" and S.nombre <> 'Bromatologia' " +
                              " order by S.idTipoServicio, R.nombre";
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
            Response.Redirect("RutinaEdit.aspx", false);
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modifica";

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }
            }
        }

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
                Response.Redirect("RutinaEdit.aspx?id=" + e.CommandArgument);
            if (e.CommandName == "Eliminar")
            {
                Eliminar(e.CommandArgument);
                CargarGrilla();
            }
        }
        private void Eliminar(object p)
        {
            Rutina oRegistro = new Rutina();
            oRegistro = (Rutina)oRegistro.Get(typeof(Rutina), int.Parse(p.ToString()));
            Usuario oUser = new Usuario();
            oRegistro.Baja = true;
           oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
        }
    }
}
