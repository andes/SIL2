using Business;
using Business.Data;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

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
                if (Session["idUsuario"] != null)
                {
                    RegistrarScriptConfirmacion();
                    VerificaPermisos("Usuarios");
                    CargarListas();
                    

                    if (Request["idEfector"] != null) ddlEfector.SelectedValue = Request["idEfector"].ToString();
                    if (Request["idPerfil"] != null) ddlPerfil.SelectedValue = Request["idPerfil"].ToString();
                    if (Request["tipoAutenticacion"] != null) ddlTipoAutenticacion.SelectedValue = Request["tipoAutenticacion"].ToString();
                    if (Request["nombre"] != null) txtNombre.Text = Request["nombre"].ToString();
                    if (Request["habilitados"] != null) ddlHabilitados.SelectedValue = Request["habilitados"].ToString();
                    if (Request["username"] != null) txtUsername.Text = Request["username"].ToString();
                    if (Request["apellido"] != null) txtApellido.Text = Request["apellido"].ToString();
                    if (Request["administrador"] != null) chbAdministrador.Checked = bool.Parse(Request["administrador"].ToString());

                    CargarGrilla();
                    if (oUser.IdEfector.IdEfector == 227) /// nivel central
                    {
                        btnAgregar.Visible = true;
                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
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
					  where E.idEfector=" + oUser.IdEfector.IdEfector.ToString() + " and exists (select 1 from Sys_UsuarioEfector u with (nolock)  where e.idefector= u.idefector) " +
                      " ORDER BY nombre ";
             
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            if   (nivelcentral)
               ddlEfector.Items.Insert(0, new ListItem("--Seleccione un efector--", "0"));

            m_ssql = @"SELECT idPerfil, nombre FROM Sys_Perfil with (nolock) 
                      where idperfil in (select idperfil from Sys_Usuario where activo=1 and
                    idefector=" + oUser.IdEfector.IdEfector.ToString() +" ) ORDER BY nombre";
            oUtil.CargarCombo(ddlPerfil, m_ssql, "idPerfil", "nombre");
            ddlPerfil.Items.Insert(0, new ListItem("Todos", "0"));

        }
        private void CargarGrilla()
        {
            gvLista.AutoGenerateColumns = false;
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            CurrentPageLabel.Text = "Página " + (gvLista.PageIndex+1) + " de " + gvLista.PageCount.ToString();
        }
        
        private DataTable LeerDatos(string tipo="")
        {
            string m_strSQL;
            if (tipo == "excel")
            {
                m_strSQL = @"SELECT U.apellido, U.nombre, username, CASE WHEN U.activo = 1 THEN 'Si' ELSE 'No' END AS activo,
                    firmaValidacion, email, U.telefono, CASE WHEN externo = 1 THEN 'Si' ELSE 'No' END AS externo,
                    P.nombre AS Perfil, E.nombre AS Efector,tipoAutenticacion as [Tipo Autenticacion] 
                    FROM Sys_Usuario U WITH (NOLOCK) 
                    INNER JOIN sys_perfil P WITH (NOLOCK) ON U.idPerfil = P.idPerfil 
                    INNER JOIN Sys_UsuarioEfector AS UE WITH (NOLOCK) ON UE.idUsuario = U.idUsuario 
                    INNER JOIN Sys_Efector AS E WITH (NOLOCK) ON E.idEfector = UE.idEfector ";
            }
            else
            {
                m_strSQL = @"SELECT U.idUsuario, U.apellido, U.nombre, U.username, P.nombre AS perfil, E.nombre as efector ,
                        case when U.activo=1 then 'Si' else 'No' end  as habilitado, U.activo as activo , tipoAutenticacion 
                        FROM Sys_Usuario U WITH(NOLOCK) 
                        INNER JOIN sys_perfil P WITH (NOLOCK) ON U.idPerfil = P.idPerfil 
                        INNER JOIN Sys_UsuarioEfector AS UE WITH (NOLOCK) ON UE.idUsuario = U.idUsuario 
                        INNER JOIN Sys_Efector AS E WITH (NOLOCK) ON E.idEfector = UE.idEfector ";
            }
                 
            string str_condicion = stringFiltros();


            //Poder ver en el listado de usuarios los usuarios externos del efector logueado
            if (oUser.IdEfector.IdEfector != 227)
            {
                string m_strSQLExterno = m_strSQL;
                string str_condicion_externo = str_condicion;
                //1- Primer parte del select se mantiene igual
                m_strSQL += str_condicion;

                //2-Segunda parte del select cambiamos las condiciones
                str_condicion_externo = str_condicion_externo.Replace(" and E.idEfector=" + ddlEfector.SelectedValue.ToString(), "");
                str_condicion_externo +=  " and  U.idEfectorDestino=" + oUser.IdEfector.IdEfector + " and U.idPerfil=15   order by username";
                m_strSQLExterno += str_condicion_externo;

                m_strSQL += @"
                    union 
                            "  + m_strSQLExterno;
            }
            else
                m_strSQL += str_condicion + " order by username";
            

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
           

            return Ds.Tables[0];
        }

        private string stringFiltros()
        {
            string str_condicion = " where username!='adminapi' ";

            if (ddlEfector.SelectedValue != "0")
                str_condicion += " and E.idEfector=" + ddlEfector.SelectedValue.ToString();

            if (ddlPerfil.SelectedValue != "0")
                str_condicion += " and P.idPerfil=" + ddlPerfil.SelectedValue.ToString();

            if (ddlTipoAutenticacion.SelectedValue != "0")
                str_condicion += " and tipoAutenticacion='" + ddlTipoAutenticacion.SelectedValue.ToString() + "'";

            if (ddlHabilitados.SelectedValue != "0")
                if (ddlHabilitados.SelectedValue == "1") str_condicion += " and U.activo=1";
                else str_condicion += " and U.activo=0";

            if (txtUsername.Text != "")
                str_condicion += " and U.username LIKE '%" + txtUsername.Text + "%'";

            if (txtNombre.Text != "")
                str_condicion += " and U.nombre LIKE '%" + txtNombre.Text + "%'";

            if (txtApellido.Text != "")
                str_condicion += " and U.apellido LIKE '%" + txtApellido.Text + "%'";

            if (chbAdministrador.Checked)
                str_condicion += " and U.administrador=1";

            return str_condicion;
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            string filtro = parametrosFiltros();
            Response.Redirect("UsuarioEdit.aspx" + "?" + filtro);
        }
        
        private string parametrosFiltros()
        {
            return @"idEfector=" + ddlEfector.SelectedValue.ToString()+ "&idPerfil=" + ddlPerfil.SelectedValue.ToString()+ "&tipoAutenticacion=" + ddlTipoAutenticacion.SelectedValue.ToString()
                + "&habilitados=" + ddlHabilitados.SelectedValue.ToString() + "&username=" + txtUsername.Text + "&nombre=" + txtNombre.Text + "&apellido=" + txtApellido.Text + "&administrador=" + chbAdministrador.Checked.ToString();
        }

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
           
            string filtro = parametrosFiltros();
            if (e.CommandName == "Modificar")
            {
                string MyURL;

                MyURL = "UsuarioEdit.aspx?id=" + e.CommandArgument.ToString() + "&" + filtro;
                Response.Redirect(MyURL);
            }
          
        }



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
                    chk.InputAttributes["onchange"] = "if(!PreguntoCambiarEstado(this)) { this.checked = !this.checked; return false; }";
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
                dataTableAExcel(LeerDatos("excel"),"ListaUsuarios" );

        }

        private DataTable LeerDatosExcel()
        {
            string m_strFiltro = "1=1 ";
            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (ddlEfector.SelectedValue != "0")
                m_strFiltro += " AND  E.idEfector=" + ddlEfector.SelectedValue.ToString();


            if (ddlPerfil.SelectedValue != "0")
                m_strFiltro += " and P.idPerfil=" + ddlPerfil.SelectedValue.ToString();
            if (ddlTipoAutenticacion.SelectedValue != "0")
                m_strFiltro += " and tipoAutenticacion='" + ddlTipoAutenticacion.SelectedValue.ToString() + "'";
            if (ddlHabilitados.SelectedValue != "0")
                if (ddlHabilitados.SelectedValue == "1") m_strFiltro += " and U.activo=1";
                else m_strFiltro += " and U.activo=0";

            if (txtUsername.Text != "")
                m_strFiltro += " and U.username LIKE '%" + txtUsername.Text + "%'";
            if (txtNombre.Text != "")
                m_strFiltro += " and U.nombre LIKE '%" + txtNombre.Text + "%'";
            if (txtApellido.Text != "")
                m_strFiltro += " and U.apellido LIKE '%" + txtApellido.Text + "%'";
            if (chbAdministrador.Checked)
                m_strFiltro += " and U.administrador=1";

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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

       

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                gvLista.PageIndex = e.NewPageIndex;
                int currentPage = gvLista.PageIndex + 1;
                CurrentPageLabel.Text = "Página " + currentPage.ToString() + " de " + gvLista.PageCount.ToString();
                CargarGrilla();
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        protected void ddlTipoAutenticacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlHabilitados_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void chbAdministrador_CheckedChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

       
        protected void gvLista_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                DataTable dt = LeerDatos();
                string sortDirection = GetSortDirection(e.SortExpression);
                dt.DefaultView.Sort = e.SortExpression + " " + sortDirection;
                gvLista.DataSource = dt;
                gvLista.DataBind();
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                        sortDirection = "DESC";
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;
            return sortDirection;
        }

    }
}
