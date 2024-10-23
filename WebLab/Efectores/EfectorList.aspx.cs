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

namespace WebLab.Efectores
{
    public partial class EfectorList : System.Web.UI.Page
    {
        Utility oUtil = new Utility(); public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {

            //MiltiEfector: Filtra para configuracion del efector del usuario

            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Efector");
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
            string m_strSQL = " select E.idEfector,   E.nombre, case when E.idtipoEfector=2 then 'Privado' else 'Publico' end as publico " +
                              " from Sys_efector E"+                                                          
                              " order by E.nombre";

  //          if (!oUser.Administrador)
  //          {
  //              m_strSQL = @"select E.idEfector,   E.nombre, case when E.idtipoEfector=2 then 'Privado' else 'Publico' end as publico   
  //, habilitado =isnull((select 1 from LAB_EfectorRelacionado R where idEfectorRel= E.idEfector and R.idEfector=" + oUser.IdEfector.IdEfector.ToString() + @"),0)
  //                       from Sys_efector E 								 
  //                                 order by nombre";
  //          }
            DataSet Ds = new DataSet();
            //     SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

             
             
            return Ds.Tables[0];
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("EfectorEdit.aspx");
        }

     

      

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                string MyURL;

                MyURL = "EfectorEdit.aspx?id=" +e.CommandArgument.ToString();
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
            Business.Data.Efector oRegistro = new Business.Data.Efector();
            oRegistro = (Business.Data.Efector)oRegistro.Get(typeof(Business.Data.Efector), int.Parse(p.ToString()));
            if (!oRegistro.tieneVinculados())
            {

                oRegistro.Delete();
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
