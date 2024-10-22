using Business;
using Business.Data.Laboratorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebLab
{
    /// <summary>
    /// Summary description for imagen
    /// </summary>
    public class imagen : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string m_strSQL;

            if (context.Request["idPaciente"] != null)
            { 
              


                m_strSQL = " SELECT imgFoto FROM LAB_Consentimiento WHERE idPaciente=" + context.Request["idPaciente"];
            }
            else
                m_strSQL = " SELECT imgResultado as imgFoto FROM LAB_CasoFiliacion WHERE idCasoFiliacion=" + context.Request["id"];

            if (context.Request["Renaper"] != null)
                m_strSQL = " SELECT foto as imgFoto FROM sys_paciente WHERE idPaciente=" + context.Request["idPaciente"];
            

            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {
                DataSet Ds = new DataSet();              
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];
                 if (Ds.Tables[0].Rows.Count>0)
                { 
                    byte[] imageBuffer = (byte[])Ds.Tables[0].Rows[0]["imgFoto"];
                    context.Response.ContentType = "image/jpg";
                    context.Response.BinaryWrite(imageBuffer);
                }
                conn.Close();
            }                    

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}