using Business;
using Business.Data;
using Business.Data.Laboratorio;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
namespace WebLab
{
    public partial class CondicionUso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarListas();
            }

        }

        private void CargarListas()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection; ///Performance: conexion de solo lectura

            string m_strSQL = @"  Select TOP 1 * FROM LAB_CondicionUso order by idCondicionUso desc ";
            DataSet Ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            conn.Close();
            conn.Dispose();

            if (Ds.Tables[0].Rows.Count > 0)
                litCondicionesUso.Text = Ds.Tables[0].Rows[0]["texto"].ToString();


        }


        protected void btn_aceptarTerminosCondiciones_Click(object sender, EventArgs e)
        {
            if (Session["usuarioPendienteAceptacion"] == null)
            {
                Response.Redirect("Logout.aspx", true);
                return;
            }

            int i_idusuario = int.Parse(Session["usuarioPendienteAceptacion"].ToString());
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), i_idusuario);

            if (oUser != null)
            {
                Session["usuarioPendienteAceptacion"] = null;

                //Actualizar la fecha en la bd
                oUser.FechaAceptaTerminosCondiciones = DateTime.Now;
                oUser.Save();
                //Actualizo acceso en log
                LogAccesoTerminosCondiciones RegistroAcceso = new LogAccesoTerminosCondiciones();
                RegistroAcceso.IdUsuario = oUser.IdUsuario;
                RegistroAcceso.Fecha = DateTime.Now;
                RegistroAcceso.Save();

                Session["usuarioContinuar"] = oUser.IdUsuario;
                Response.Redirect("~/Login.aspx", false);
            }
            else
            {
                Response.Redirect("Logout.aspx", true);
            }
        }
    }
}