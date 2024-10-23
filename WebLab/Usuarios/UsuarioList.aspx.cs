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

namespace WebLab.Usuarios
{
    public partial class UsuarioList : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Usuarios");
                CargarListas();
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
        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string m_ssql  = @"   SELECT idEfector, nombre FROM Sys_Efector e (nolock) 
					  where exists (select 1 from Sys_UsuarioEfector u (nolock)  where e.idefector= u.idefector) ORDER BY nombre ";

            
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
               ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));
         
        }
        private void CargarGrilla()
        {
            gvLista.AutoGenerateColumns = false;
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            string m_strSQL = @"SELECT     U.idUsuario, U.apellido, U.nombre, U.username, P.nombre AS perfil, E.nombre as efector ,
case when U.activo=1 then 'Si' else 'No' end  as habilitado
FROM         Sys_Usuario AS U (nolock) INNER JOIN
                      Sys_Perfil AS P (nolock)  ON U.idPerfil = P.idPerfil inner join
					  Sys_UsuarioEfector UE (nolock)  on Ue.idusuario= U.idUsuario inner join
					  sys_efector as E (nolock) on Ue.idEfector= E.idEfector";

            if (ddlEfector.SelectedValue != "0")
                m_strSQL += " where E.idEfector=" + ddlEfector.SelectedValue.ToString();

            m_strSQL += " order by username";

        

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
            Response.Redirect("UsuarioEdit.aspx");
        }





        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
           

            if (e.CommandName == "Modificar")
            {
                string MyURL;

                MyURL = "UsuarioEdit.aspx?id=" + e.CommandArgument.ToString();
                Response.Redirect(MyURL);
            }
            // Response.Redirect("AreaEdit.aspx?id=" + e.CommandArgument);
          
        }


        //private void Eliminar(object p)
        //{
        //    Perfil oRegistro = new Perfil();
        //    oRegistro = (Perfil)oRegistro.Get(typeof(Perfil), int.Parse(p.ToString()));
        //    Usuario oUser = new Usuario();
        //    oRegistro.Baja = true;
        //    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
        //    oRegistro.FechaRegistro = DateTime.Now;
        //    oRegistro.Save();
        //}

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
             
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[6].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                
                if (Permiso == 1)
                {                
                    CmdModificar.ToolTip = "Consultar";
                }
            }
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}
