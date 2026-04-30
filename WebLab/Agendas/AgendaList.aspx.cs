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

namespace WebLab.Agendas
{
    public partial class AgendaList : System.Web.UI.Page
    {
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
              
                    VerificaPermisos("Agenda");
                    
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
            gvLista.AutoGenerateColumns = false;
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            //Usuario oUser = new Usuario();
            //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
          
            string m_condicion = " and A.idEfector = "  + oUser.IdEfector.IdEfector.ToString();
            if (ddlTipoServicio.SelectedValue != "0") m_condicion += " and A.idTipoServicio=" + ddlTipoServicio.SelectedValue;
            if (ddlEfectorSolicitante.SelectedValue != "0") m_condicion += " and A.idEfectorSolicitante=" + ddlEfectorSolicitante.SelectedValue;

            string m_strSQL = @" select top 20 A.idAgenda, T.nombre,  (I.codigo +'-' +I.nombre) as item, convert(varchar(10),A.fechaDesde,103) as fechaDesde,
                                        convert(varchar(10),A.fechaHasta,103) as fechaHasta ,E.nombre as efector,  U.apellido as usuario, 
                                        A.fechaRegistro,D.dias,  D.limiteTurnos
                              from Lab_TipoServicio T  (nolock) 
                                 INNER JOIN lAB_aGENDA A (nolock) on A.idTipoServicio= T.idTipoServicio 
                                 LEFT JOIN lab_item I (nolock)  on A.iditem=i.iditem  
                                 inner join sys_Efector E (nolock) on E.idEfector=A.idEfectorSolicitante
                                 inner join sys_usuario U (nolock)  on U.idUsuario= A.idUsuarioRegistro
                                    INNER JOIN (
                                        SELECT 
                                            D1.idAgenda,

                                            STUFF((
                                                SELECT ', ' + 
			                                    case 
			                                    when D2.dia = 1 then 'Lunes' 
			                                    when D2.dia = 2 then 'Martes'
			                                    when D2.dia = 3 then 'Miercoles' 
			                                    when D2.dia = 4 then 'Jueves' 
			                                    when D2.dia = 5 then 'Viernes' 
			                                    when D2.dia = 6 then 'Sabado' 
			                                    when D2.dia = 7 then 'Domingo' 
			
			                                    end
                                                FROM LAB_AgendaDia D2 (NOLOCK)
                                                WHERE D2.idAgenda = D1.idAgenda
                                                ORDER BY D2.dia
                                                FOR XML PATH('')
                                            ), 1, 1, '') AS dias,

                                            MAX(D1.limiteTurnos) AS limiteTurnos

                                        FROM LAB_AgendaDia D1 (NOLOCK)
                                        GROUP BY D1.idAgenda
                                    ) D
                                        ON D.idAgenda = A.idAgenda
                                                               where A.baja=0 " + m_condicion +"   order by A.idAgenda desc ";

            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
         //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

             
             
            return Ds.Tables[0];
        }



        private void CargarListas()
        {
            Utility oUtil = new Utility();   ///Carga de combos de Areas         
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE idtipoServicio<4 and (baja = 0)";
            oUtil.CargarCombo(ddlTipoServicio, m_ssql, "idTipoServicio", "nombre");
            ddlTipoServicio.Items.Insert(0, new ListItem("Todos", "0"));

            ////////////Carga de combos de Efector
            m_ssql = @"SELECT idEfector, nombre FROM sys_Efector  E (nolock) 
                    where idEfector in  (select distinct R.idefectorRel from LAB_EfectorRelacionado R (nolock) 
				                    inner join LAB_Agenda D  (nolock) ON D.idEfectorSolicitante=R.idefectorRel and D.idEfector=R.idEfector
				                    where D.baja =0 and  R.idefector = " + oUser.IdEfector.IdEfector.ToString() +
	                    ") or (E.idEfector= " + oUser.IdEfector.IdEfector.ToString() + @") order by nombre ";
           
            oUtil.CargarCombo(ddlEfectorSolicitante, m_ssql, "idEfector", "nombre");
            if(ddlEfectorSolicitante.Items.Count > 1)
                ddlEfectorSolicitante.Items.Insert(0, new ListItem("Todos", "0"));

            m_ssql = null;
            oUtil = null;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AgendaEdit.aspx", false);
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[9].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip="Modifica";

                 ImageButton CmdEliminar = (ImageButton)e.Row.Cells[10].Controls[1];
                    CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    if (Permiso == 1)
                    {
                        CmdEliminar.Visible = false;
                        CmdModificar.ToolTip = "Consultar";
                    }

                    CmdEliminar.CommandName = "Eliminar";
                    CmdEliminar.ToolTip = "Eliminar";
                
            }  
        }

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Modificar")
                Response.Redirect("AgendaEdit.aspx?id=" + e.CommandArgument);
            if (e.CommandName == "Eliminar")
            {
                if (Permiso == 2)
                {

                    Eliminar(e.CommandArgument);
                    CargarGrilla();
                }
            }
        }


        private void Eliminar(object p)
        {
            Agenda oRegistro = new Agenda();
            oRegistro = (Agenda)oRegistro.Get(typeof(Agenda), int.Parse(p.ToString()));
            if (oRegistro != null)
            { 
            oRegistro.Baja = true;
                oRegistro.IdUsuarioRegistro = oUser; // (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
        }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEfectorSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}
