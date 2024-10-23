using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InfoSoftGlobal;
using System.Data;
using System.Data.SqlClient;
using Business;

namespace WebLab.Estadisticas
{
    public partial class distribucionServicio : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            FCLiteral.Text = mostrarGrafico(); ;
        }

        private string mostrarGrafico()
        {
            string s_titulo = "";
            string s_tipo = "";
            string strXML = "";
            string strDetalle = "";

            //s_tipografico = "../FusionCharts/FCF_MSColumn3D.swf";

            //s_titulo = "";// ddlAnalisis.SelectedItem.Text;
            //if (p == 0) s_tipo = "Casos por tipo de muestra";
            //else
            //{ if (p == 3) s_tipo = "Casos por Resultados Obtenidos"; else        s_tipo = "Aislamientos"; }

            string s_ancho = "700";
            string s_alto = "400";

            DataTable dt = new DataTable();
            dt = GetDatosEstadistica();



        //    string strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='10'  decimalPrecision='0' showNames='1'>";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strDetalle += "<set name='" + dt.Rows[i][0].ToString() + "' value='" + dt.Rows[i][1].ToString() + "' />";
                }
            }


            //string[] arr = Request["valores"].ToString().Split((";").ToCharArray());
            //foreach (string item in arr)
            //{
            //    strDetalle += "<set " + item + " />";
            //}

            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf";

        
               
                    strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='30'  decimalPrecision='0' showNames='1'>";
                    strXML += strDetalle;
                    strXML += "</graph>";
                
            
            return FusionCharts.RenderChart(s_tipografico, "", strXML, "1", s_ancho, s_alto, false, false);
        }

        private DataTable GetDatosEstadistica()
        {
            string m_strSQL = @" select S.nombre, COUNT(*) as cantidad
 from LAB_Protocolo as P
 inner join LAB_TipoServicio  as S on S.idTipoServicio= P.idTipoServicio
 where fecha>='20140401' --and idTipoServicio=4
 group by  S.nombre
 order by cantidad desc";
            DataSet Ds = new DataSet();

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            return Ds.Tables[0];     
            
        }
    }
}