using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Resultados
{
    public partial class ResultadosPanel : System.Web.UI.Page
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {              


                if (VerificaPermisos("Historial de Resultados") >0)
                    Consulta.Visible = true;
                else
                    Consulta.Visible = false;

                if (VerificaPermisos("Resultados Histocompatibilidad") >0)
                    Histo.Visible = true;
                else
                    Histo.Visible = false;

                if (VerificaPermisos("Ingreso Test Antigeno") > 0)
                {
                    ingresoTest.Visible = true;
                    pnlAntigeno.Visible = true;
                }
                else
                {
                    pnlAntigeno.Visible = false;
                    ingresoTest.Visible = false;
                }



            }
            if (Request["Consulta"] != null)
                MostrarAccesos();

        }
        private void MostrarAccesos()
        {if (Session["idUsuario"] != null)
            {
                string m_strSQL = @" select top 10 dbo.NumeroProtocolo(idprotocolo) as Protocolo, fecha as [Fecha y Hora]  from lab_auditoriaprotocolo 
where idUsuario = " + Session["idUsuario"].ToString() + @"
order by idAuditoriaProtocolo desc";


                DataSet Ds1 = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds1);

                GridView1.DataSource = Ds1.Tables[0];
                GridView1.DataBind();
                GridView1.Visible = true;
            }
            else Response.Redirect(Page.ResolveUrl("~/FinSesion.aspx"), false);
        }

        private int VerificaPermisos(string sObjeto)
        {
            int i_permiso = 0;

            Utility oUtil = new Utility();
            i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
            return i_permiso;

        }


       
        protected void lnkGuardar_Click1(object sender, EventArgs e)
        {

        }

   

    }
}