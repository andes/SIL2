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
using Business.Data.Laboratorio;
using Business;
using System.Drawing;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Business.Data;
using CrystalDecisions.Web;
//using System.Web.UI.WebControls;

namespace WebLab.CasoFiliacion
{
    public partial class CasoListResultado : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                            VerificaPermisos("Resultados Histocompatibilidad");

                            //CargarGrillaMedula();
                       

                    }
                else Response.Redirect("../FinSesion.aspx", false);
            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private void CargarGrillaMedula()
        {
            gvLista.HeaderStyle.ForeColor = Color.Beige;
            gvLista.DataSource = LeerDatos("Extendido");
            gvLista.DataBind();
            //PonerImagenes();
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
                          
                            gvLista.Columns[5].Visible = false;
                        }
                        break;
                  

                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

     
   

        private object LeerDatos(string tipo)
        {
            string m_condicion = "";
            if (txtNumero.Text != "") m_condicion += " and C.IDCASOFILIACION =" + txtNumero.Text.Trim();
            if (txtNombre.Text != "") m_condicion += " and C.nombre like '%" + txtNombre.Text + "%'";
string m_strSQL ="";
            string m_orden = "";
            if (ddlTipo.SelectedValue == "0") m_orden = " ORDER BY C.IDCASOFILIACION desc";
            if (ddlTipo.SelectedValue == "1") m_orden = " ORDER BY C.IDCASOFILIACION asc";

            //if (tipo=="Extendido")
            m_strSQL = @"Select C.idCasoFiliacion, C.nombre,case when U.firmaValidacion='' then U.apellido + ' ' + U.nombre else U.firmaValidacion end  as usuario FROM lab_casofiliacion C
inner join Sys_Usuario U on U.idUsuario= C.idUsuarioValida
where idUsuarioValida>0 and idcasofiliacion  in (select idcasofiliacion from LAB_CasoFiliacionProtocolo C
inner join LAB_Protocolo P on C.idprotocolo = P.idprotocolo where P.idtiposervicio =3) and baja = 0" + m_condicion + m_orden;
            
                             
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

             
             
            return Ds.Tables[0];
        }


        

      

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
             
            Context.Items.Add("id", e.CommandArgument);
            

            if (e.CommandName != "Page")
            {
                switch (e.CommandName)
                {
                    
                    case "Resultados":
                        Server.Transfer("CasoResultadoHistoView.aspx");// Imprimir(e.CommandArgument.ToString()); 
                        break;
                
                }
            }
        }

        

      

    

        private void Imprimir(string id)
        {

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id));
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            CrystalReportSource oCr = new CrystalReportSource();
          
                oCr.Report.FileName = "ResultadoHisto.rpt";
            oCr.ReportDocument.SetDataSource(oRegistro.getResultadoHLA(oCon.IdHistocompatibilidad));
           

            oCr.DataBind();

            oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuario"].ToString()),"");

         
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Resultado_" + oRegistro.Nombre.Trim() + ".pdf");


        }
  

       

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString()));


         



                LinkButton CmdResultados = (LinkButton)e.Row.Cells[2].Controls[1];
                CmdResultados.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdResultados.CommandName = "Resultados";
                CmdResultados.ToolTip = "Resultados";
                 
                

            }  

        }

       

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text!="")
            { 
            gvLista.PageIndex = 0;
            CargarGrillaMedula();            
            CurrentPageLabel.Text = " ";
            }else
            CurrentPageLabel.Text = "Debe ingresar DNI del receptor o nombre";

        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLista_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
           
            
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;

            int currentPage = gvLista.PageIndex+1;


            CurrentPageLabel.Text = "Página " + currentPage.ToString() +
              " de " + gvLista.PageCount.ToString();
           

                CargarGrillaMedula();
            

           
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            
        }

       


        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            //MostrarInforme("Nomenclador");
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {   gvLista.PageIndex = 0;
            
                CargarGrillaMedula();
           

            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
          
                CargarGrillaMedula();
            
           
            CurrentPageLabel.Text = " ";
        }

        protected void lnkPdfReducido_Click(object sender, EventArgs e)
        {
            //MostrarInforme("Reducido");
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarArea();
            
            gvLista.PageIndex = 0;
          
                CargarGrillaMedula();
        

            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged1(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
            
                CargarGrillaMedula();
           

            CurrentPageLabel.Text = " ";
        }

        protected void gvLista_DataBound(object sender, EventArgs e)
        {

          

            
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
             

          
                      
        }
    }
}
