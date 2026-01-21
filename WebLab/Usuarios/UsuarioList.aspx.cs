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
using System.Text;
using System.IO;
using Business.Data;

namespace WebLab.Usuarios
{
    public partial class UsuarioList : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                //  oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!Page.IsPostBack)
            {
                RegistrarScriptConfirmacion();
                VerificaPermisos("Usuarios");
                CargarListas();
                CargarGrilla();
                if (oUser.IdEfector.IdEfector == 227) /// nivel central
                {
                    btnAgregar.Visible = true;
                }
            }
        }
        private void RegistrarScriptConfirmacion()
        {
            string script = @"function PreguntoCambiarEstado() { 
                        return confirm('¿Está seguro de cambiar estado?'); 
                      }";

            ScriptManager.RegisterClientScriptBlock(
                this,
                this.GetType(),
                "PreguntoCambiarEstado",
                script,
                true
            );
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
            Utility oUtil = new Utility(); bool nivelcentral = false;
            string m_ssql;
            if (oUser.IdEfector.IdEfector == 227) /// nivel central
            {
                nivelcentral = true;
                m_ssql = @"   SELECT idEfector, nombre FROM Sys_Efector e with (nolock) 
					  where exists (select 1 from Sys_UsuarioEfector u with (nolock)  where e.idefector= u.idefector) ORDER BY nombre ";
            }
            else
                m_ssql = @"   SELECT idEfector, nombre FROM Sys_Efector e with (nolock) 
					  where E.idEfector=" + oUser.IdEfector.IdEfector.ToString() + " and exists (select 1 from Sys_UsuarioEfector u with (nolock)  where e.idefector= u.idefector) ORDER BY nombre ";
             
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            if   (nivelcentral)
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
case when U.activo=1 then 'Si' else 'No' end  as habilitado, U.activo as activo , tipoAutenticacion
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

                ImageButton CmdModificar = (ImageButton)e.Row.Cells[7].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                CheckBox chk = (CheckBox)e.Row.FindControl("chkStatus");
                if (chk != null)
                {
                    chk.InputAttributes["onchange"] =
                "if(!PreguntoCambiarEstado(this)) { this.checked = !this.checked; return false; }";
                }

                if (Permiso == 1)
                {
                    CmdModificar.ToolTip = "Consultar";
                }
                if (oUser.IdEfector.IdEfector != 227) /// nivel central
                {
                    CmdModificar.Visible = false;
                }
            }
        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            string accion = "";
            if (chkStatus.Checked) accion = "Habilita"; else accion = "Dehabilita";



            Usuario oRegistro = new Usuario();
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), i_id);



            if (oRegistro != null)
            {
                oRegistro.Activo = chkStatus.Checked;

                oRegistro.Save();

                oUser.GrabaAuditoria(accion, oRegistro.IdUsuario, oRegistro.Username);
            }
            CargarGrilla();

        }
        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                dataTableAExcel(LeerDatosExcel(),"ListaUsuarios" );
          
        }

        private DataTable LeerDatosExcel()
        {
            string m_strFiltro = "";
            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (ddlEfector.SelectedValue != "0")
                m_strFiltro = "   E.idEfector=" + ddlEfector.SelectedValue.ToString();



            cmd.CommandText = "[LAB_ListaUsuarios]";

            cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
            cmd.Parameters["@FiltroBusqueda"].Value = m_strFiltro;


            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            //////////


            return Ds.Tables[0];
        }

        private void dataTableAExcel(DataTable tabla, string nombreArchivo)
        {
            if (tabla.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Page pagina = new Page();
                HtmlForm form = new HtmlForm();
                GridView dg = new GridView();
                dg.EnableViewState = false;
                dg.DataSource = tabla;
                dg.DataBind();
                pagina.EnableEventValidation = false;
                pagina.DesignerInitialize();
                pagina.Controls.Add(form);
                form.Controls.Add(dg);
                pagina.RenderControl(htw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreArchivo + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            }
        }
    }
}
