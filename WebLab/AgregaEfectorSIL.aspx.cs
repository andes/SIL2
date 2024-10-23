using Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
    public partial class AgregaEfectorSIL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Utility oUtil = new Utility();

                
                string     m_ssql = "select  E.idEfector, E.nombre  from sys_efector E order by nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
 
                m_ssql = null;
                oUtil = null;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "LAB_AgregaNuevoLaboratorio";        

          


            cmd.Parameters.Add("@idEfector", SqlDbType.Int);
            cmd.Parameters["@idEfector"].Value =  ddlEfector.SelectedValue ;
           

            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            //////////


            //(count(*)/@RegistrosPorPagina)+1 

            lblMensaje.Text = Ds.Tables[0].Rows.Count.ToString() + " determinaciones impactadas";
        }

        protected void btnInicializa_Click(object sender, EventArgs e)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "LAB_InicializaLaboratorioMultiEfector";


             

            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            //////////


            //(count(*)/@RegistrosPorPagina)+1 

            lblMensaje.Text = Ds.Tables[0].Rows.Count.ToString() + " determinaciones inicializadas";

        }
    }
}