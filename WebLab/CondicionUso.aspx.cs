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
                Inicializar();
                CargarListas();
            }

        }

        private void Inicializar()
        {
            if (Request["Tipo"] == "consulta")
            {
                
                lblTitulo.Text = "Mi última aceptación";
                cbAceptaTerminos.Visible = false;
                btnAceptarTerminosCondiciones.Visible = false;
                hlRegresar.Visible = true;
                pnlMiAceptacion.Visible = true;

            }
        }
        private void CargarListas()
        {
            string m_strSQL = "";
            if (Request["Tipo"] == "alta")
            {
                m_strSQL = @"  Select TOP 1 * FROM LAB_CondicionUso order by idCondicionUso desc ";
            }
            else
            {
                //Consulta última aceptación.
                if (Session["idUsuario"] != null)
                {
                    m_strSQL = @" select top 1 texto, version, l.idCondicionUso, fecha
                                    from LAB_LogAccesoTerminosCondiciones l
                                    inner join LAB_CondicionUso cod on cod.idCondicionUso = l.idCondicionUso
                                    where idUsuario = " + int.Parse(Session["idUsuario"].ToString()) +
                                " order by fecha desc ";
                }
                
            }



            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection; ///Performance: conexion de solo lectura
            DataSet Ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            conn.Close();
            conn.Dispose();

            if (Ds.Tables[0].Rows.Count > 0)
            {
                litCondicionesUso.Text = Ds.Tables[0].Rows[0]["texto"].ToString();
                if (Request["Tipo"] == "alta")
                {
                    HFIdCondicion.Value = Ds.Tables[0].Rows[0]["idCondicionUso"].ToString();
                } else if (Request["Tipo"] == "consulta") {
                    lblVersion.Text = Ds.Tables[0].Rows[0]["version"].ToString();
                    lblFechaAceptacion.Text = DateTime.Parse(Ds.Tables[0].Rows[0]["fecha"].ToString()).ToShortDateString();
                }
            }


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
                RegistroAcceso.IdCondicionUso = int.Parse(HFIdCondicion.Value);
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