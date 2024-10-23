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
    public partial class GenResultadosFrecuencias : System.Web.UI.Page
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
                lblProceso.Text = Context.Items["idProceso"].ToString();
                CargarListas();
                CargarTablas("");            
             
               
            }


        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select distinct marcador from GEN_Frecuencias where idproceso="+lblProceso.Text+" order by marcador";

            oUtil.CargarCombo(ddlMarcador, m_ssql, "marcador", "marcador");
            ddlMarcador.Items.Insert(0, new ListItem("Todos", ""));
        }

        private void CargarTablas(string marcador)
        {
            gvLista.DataSource = LeerFrecuencia(marcador);
            gvLista.DataBind();
            if (marcador != "")
                gvLista.Width = Unit.Percentage(20);



        }

      

        private object LeerFrecuencia(string marcador)
        {
            string proceso = lblProceso.Text;
           
            string m_strSQL = @"
select Allello as Alelo,   [CSF1PO], [D10S1248],[D12S391],[D13S317], [D16S539],[D1S1656], [D18S51],[D21S11],[D2S1338],[D2S441],[D3S1358],[D5S818],
[D8S1179],[DYS391],[FGA],[Penta D],[Penta E],[TH01],[TPOX],[vWA]
	from ( 
	SELECT   marcador, allello, round(frecuencia,5) as frecuencia
	FROM    gen_frecuencias
	
	where idProceso="+ proceso + @"
     ) V PIVOT ( max(frecuencia) FOR marcador IN (
	  [CSF1PO], [D10S1248],[D12S391],[D13S317], [D16S539],[D1S1656], [D18S51],[D21S11],[D2S1338],[D2S441],[D3S1358],[D5S818],
[D8S1179],[DYS391],[FGA],[Penta D],[Penta E],[TH01],[TPOX],[vWA]
	 
	 ) ) as PT

 order by convert(float, allello)

";
            if (marcador!="")
                m_strSQL = @"select Allello as Alelo,   ["+ marcador + @"] 
	from ( 	SELECT   marcador, allello, round(frecuencia,5) as frecuencia
	FROM    gen_frecuencias
	where idProceso=" + proceso + @"
     ) V PIVOT ( max(frecuencia) FOR marcador IN (
	  ["+marcador + @"] 	 
	 ) ) as PT
 where isnull([" + marcador + @"] ,-1)<>-1
 order by convert(float, allello)";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
           

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
                            btnDescargar.Visible = false;
                            //btnAgregar.Visible = false;
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void ddlMarcador_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarTablas(ddlMarcador.SelectedValue);
        }
    }
     
}
