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
using Business.Data;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Usuarios
{
    public partial class PerfilList : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Perfil");
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
            string m_strSQL = @"SELECT  idPerfil, nombre , activo FROM Sys_Perfil with (nolock) order by nombre";
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
            Response.Redirect("PerfilEdit.aspx");
        }





        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Permisos")
            {
                string MyURL;

                MyURL = "PermisoEdit.aspx?id=" + e.CommandArgument.ToString();
                Response.Redirect(MyURL);
            }

            if (e.CommandName == "Modificar")
            {
                string MyURL;

                MyURL = "PerfilEdit.aspx?id=" + e.CommandArgument.ToString();
                Response.Redirect(MyURL);
            }
            // Response.Redirect("AreaEdit.aspx?id=" + e.CommandArgument);
            //if (e.CommandName == "Eliminar")
            //{
            //    Eliminar(e.CommandArgument);
               
            //}
        }


        //private void Eliminar(object p)
        //{
        //    Perfil oRegistro = new Perfil();
        //    oRegistro = (Perfil)oRegistro.Get(typeof(Perfil), int.Parse(p.ToString()));

        //    ///Buscar usuarios que esten ligados al perfil
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(Usuario));
        //    crit.Add(Expression.Eq("IdPerfil", oRegistro));
        //    IList detalle = crit.List();
        //    if (detalle.Count > 0)
        //    {
        //        string popupScript = "<script language='JavaScript'> alert('No es posible eliminar el perfil. Tiene usuarios vinculados.'); </script>";
        //        Page.RegisterStartupScript("PopupScript", popupScript);

        //    }
        //    ///////////////
        //    else
        //    {

        //        Usuario oUser = new Usuario();
        //        oRegistro.ba
        //        oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
        //        oRegistro.FechaRegistro = DateTime.Now;
        //        oRegistro.Save();
        //        CargarGrilla();
        //    }
        //}

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdPermiso = (ImageButton)e.Row.Cells[1].Controls[1];
                CmdPermiso.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdPermiso.CommandName = "Permisos";

                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                //ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                //CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                //CmdEliminar.CommandName = "Eliminar";

                if (Permiso == 1)
                {
                 //   CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }
            }
        }
    }
}
