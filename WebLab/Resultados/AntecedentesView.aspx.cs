using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Web;

namespace WebLab.Resultados
{
    public partial class AntecedentesView : System.Web.UI.Page
    {
      
        Protocolo oProtocolo = new Protocolo();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                string s_idProtocolo = Request["idProtocolo"].ToString();

                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));//int.Parse(Request["idProtocolo"].ToString()));r();

                lblPaciente.Text = oProtocolo.IdPaciente.Apellido + " " + oProtocolo.IdPaciente.Nombre;
                CargarListas();
            }

        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();

            ///////////////////Carga los analisis para la solapa Antecedentes/////////////////
            string m_ssql = " SELECT DISTINCT I.idItem, I.nombre AS nombre" +
                            " FROM LAB_DetalleProtocolo AS DP " +
                            " INNER JOIN   LAB_Protocolo AS P ON DP.idProtocolo = P.idProtocolo" +
                            " INNER JOIN   LAB_Item AS I ON DP.idItem = I.idItem" +
                            " WHERE     P.idProtocolo = " + Request["idProtocolo"].ToString() +
                            " ORDER BY I.nombre ";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("--Seleccione un análisis--", "0"));
            //////////////////////////////////////////////////////////////////////////////////          
        }

        protected void btnVerAntecendente_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string s_idProtocolo = Request["idProtocolo"].ToString();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));//int.Parse(Request["idProtocolo"].ToString()));r();
                CargarGrillaAntecedentes(oProtocolo);
              
            }
        }

        private void CargarGrillaAntecedentes(Protocolo oProtocolo)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.CommandText = "LAB_AntecedentesAnalisis2";


            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = "";
            cmd.Parameters["@fechaHasta"].Value = oProtocolo.Fecha.ToString("yyyyMMdd");

            cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idProtocolo"].Value = oProtocolo.IdProtocolo.ToString();

            cmd.Parameters.Add("@idAnalisis", SqlDbType.NVarChar);
            cmd.Parameters["@idAnalisis"].Value = ddlItem.SelectedValue;

            cmd.Parameters.Add("@idPaciente", SqlDbType.Int);
            cmd.Parameters["@idPaciente"].Value = oProtocolo.IdPaciente.IdPaciente;

            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            gvAntecedente.DataSource = Ds.Tables[0];
            gvAntecedente.DataBind();
            //  gvAntecedente.UpdateAfterCallBack = true;


        }
    }
}