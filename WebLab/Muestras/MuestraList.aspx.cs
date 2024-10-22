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

namespace WebLab.Muestras
{
    public partial class MuestraList : System.Web.UI.Page
    {
        Utility oUtil = new Utility(); public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {

            //MiltiEfector: Filtra para configuracion del efector del usuario
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Tipo de Muestra");
                CargarGrilla(); 
                 btnAgregar.Visible = oUser.Administrador;
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
            string m_strSQL = " select idMuestra,codigo,nombre,case when baja=1 then 0 else 1 end as habilitado" +
                              " from Lab_Muestra" +                              
                           //   " where baja=0 " +
                              " order by nombre";

            if (oUser.IdEfector.IdEfector!=227)
            {
                m_strSQL = @"select M.idMuestra,codigo,nombre, case when  E.idMuestra=M.idMuestra then 1 else 0 end as habilitado 
                                 from Lab_Muestra M
								 left join lab_muestraefector as E on E.idMuestra=M.idMuestra
								 and E.idefector=" + oUser.IdEfector.IdEfector.ToString() + @"
                            where baja=0  
                                   order by nombre";
            }
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
            Response.Redirect("MuestraEdit.aspx");
        }





        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                string MyURL;

                MyURL = "MuestraEdit.aspx?id=" + e.CommandArgument.ToString();

                Response.Redirect(Server.UrlPathEncode(MyURL));
            }

               // Response.Redirect("MetodoEdit.aspx?id=" + e.CommandArgument);
            //if (e.CommandName == "Eliminar")
            //{
            //    Eliminar(e.CommandArgument);
            //    CargarGrilla();
            //}
        }
        protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkStatus = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkStatus.NamingContainer;

            int i_id = int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString());

            if ((oUser.Administrador )&& (oUser.IdEfector.IdEfector==227))
            {
                Muestra oRegistro = new Muestra();
                oRegistro = (Muestra)oRegistro.Get(typeof(Muestra), i_id);
                oRegistro.Baja = !(chkStatus.Checked);
                oRegistro.Save();
            }
            else
            {
                if (!chkStatus.Checked)
                {


                    Muestra oM = new Muestra();
                    oM = (Muestra)oM.Get(typeof(Muestra), i_id);
                    Efector oEfector = new Efector();
                    oEfector = (Efector)oEfector.Get(typeof(Efector), oUser.IdEfector.IdEfector);
                    if ((oM != null) && (oEfector != null))
                    {
                        MuestraEfector oRegistro = new MuestraEfector();
                        oRegistro = (MuestraEfector)oRegistro.Get(typeof(MuestraEfector), "IdMuestra", oM, "IdEfector", oEfector);
                        if (oRegistro != null)
                            oRegistro.Delete();
                    }
                }
                else
                {
                    Muestra oRegistro = new Muestra();
                    oRegistro = (Muestra)oRegistro.Get(typeof(Muestra), i_id);
                    if (oRegistro != null)
                    {
                        MuestraEfector oRegistro2 = new MuestraEfector();

                        oRegistro2.IdEfector = oUser.IdEfector;
                        oRegistro2.IdMuestra = oRegistro;
                        oRegistro2.Save();
                    }
                }
            }


        }

        private void Eliminar(object p)
        {
            Muestra oRegistro = new Muestra();
            oRegistro = (Muestra)oRegistro.Get(typeof(Muestra), int.Parse(p.ToString()));
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
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                CheckBox CmdEliminar = (CheckBox)e.Row.Cells[3].Controls[1];
                //CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                //CmdEliminar.CommandName = "Eliminar";


                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }

                CmdModificar.Visible = oUser.Administrador;
            }
        }
    }
}
