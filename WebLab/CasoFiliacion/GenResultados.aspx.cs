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
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.Services;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Text.RegularExpressions;
using Business.Data.GenMarcadores;

namespace WebLab.CasoFiliacion
{
    public partial class GenResultados : System.Web.UI.Page
    {

    

        private enum TabIndex
        {
            DEFAULT = 1,
            ONE = 2,


            // you can as many as you want here
        }
        //private void SetSelectedTab(TabIndex tabIndex)
        //{
        //    HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)

            {


                VerificaPermisos("Casos Forense");
                CargarTablas();
            }


        }

        private void CargarTablas()
        {
            gvLista.DataSource = LeerProcesos();
            gvLista.DataBind();

           
        }

        private void GenerarFrecuencias()
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "GEN_CalculaFrecuenciasAlelicas";



            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);

            //return Ds.Tables[0];
        }

        private object LeerProcesos()
        {
            string m_strSQL = @"select P.idProceso, P.fechaCorte as fecha, count(*) as cantidad from GEN_ProcesoFrecuenciasProtocolos as PF
inner join gen_procesofrecuencias as P on P.idProceso=PF.idProceso
group by P.idProceso, P.fechaCorte ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            estatus.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
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
                    case 1:
                        {
                            btnGenerarProceso.Visible = false;
                            //btnAgregar.Visible = false;
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }








       
      
 
        

 

        protected void btnGenerarProceso_Click(object sender, EventArgs e)
        {

            GenerarFrecuencias();
            CargarTablas();


        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
             


                LinkButton CmdResultados = (LinkButton)e.Row.Cells[2].Controls[1];
                CmdResultados.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdResultados.CommandName = "Resultados";
                CmdResultados.ToolTip = "Resultados";



            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           


            if (e.CommandName != "Page")
            {
                switch (e.CommandName)
                {

                    case "Resultados":
                        { 

                        Context.Items.Add("idProceso", e.CommandArgument.ToString());
                          

                            Server.Transfer("GenResultadosFrecuencias.aspx");

                }
                        break;

                }
            }
        }
    }
     
}
