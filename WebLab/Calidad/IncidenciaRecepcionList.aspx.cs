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

namespace WebLab.Calidad
{
    public partial class IncidenciaRecepcionList : System.Web.UI.Page
    {
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                //oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
               
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    
                    VerificaPermisos("Incidencias PreRecepcion");
                    txtFechaDesde.Value = DateTime.Now.AddDays(-1).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    CargarListas();
                    CargarGrilla();
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
          
            string m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfectorOrigen, m_ssql, "idEfector", "nombre");
            ddlEfectorOrigen.Items.Insert(0, new ListItem("Todos", "0"));
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
                    case 1: btnNuevo.Visible = false; break;
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

        private void CargarGrilla()
        {           
          
            gvLista.DataSource = LeerDatos("Extendido");
            gvLista.DataBind();

            CurrentPageLabel.Text = gvLista.Rows.Count.ToString() + " registros encontrados";
        }


    

        private object LeerDatos(string tipo)
        {
            string str_condicion = " ";
            str_condicion += " AND R.idEfector =" + oUser.IdEfector.IdEfector.ToString();
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            if (txtFechaDesde.Value != "") str_condicion += " AND cast(convert(varchar(10),R.fecha,112) as datetime)>= '" + fecha1.ToString("yyyyMMdd") + "'";
            if (txtFechaHasta.Value != "") str_condicion += " AND cast(convert(varchar(10),R.fecha,112) as datetime)<= '" + fecha2.ToString("yyyyMMdd") + "'";
            if (ddlEfectorOrigen.SelectedValue != "0") str_condicion += " AND R.idEfectorOrigen=" + ddlEfectorOrigen.SelectedValue;

            string m_strSQL = @" SELECT R.idIndicenciaRecepcion,convert(varchar(10), R.fecha, 103) as fecha, U.apellido + ' ' + U.nombre AS USUARIO, E.nombre as Efector, R.numeroOrigen, R.Observaciones 
                                 FROM   LAB_IndicenciaRecepcion as R 
                                 INNER JOIN Sys_Usuario  as U ON R.idUsuario = U.idUsuario
                                 inner join Sys_Efector as E on E.idEfector= R.idEfectorOrigen
                                 where R.baja=0 " + str_condicion +"  ORDER BY R.FECHA";              
                             
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }




        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
            }
            catch
            {
                args.IsValid = false;
                cvFechas.ErrorMessage = "Fechas inválidas";
            }
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;

        }
      

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName != "Page")
            {
                switch (e.CommandName)
                {
                    case "Modificar":
                        Response.Redirect("IncidenciaRecepcionEdit.aspx?id=" + e.CommandArgument.ToString());
                        break;
                    //case "VR":
                    //    Response.Redirect("ItemValorReferencia.aspx?id=" + e.CommandArgument);
                    //    break;
                    //case "Diagramacion":
                    //    Response.Redirect("ItemDiagramacion.aspx?id=" + e.CommandArgument);
                    //    break;
                    //case "Resultado":
                    //    Response.Redirect("ItemResultado2.aspx?id=" + e.CommandArgument);
                    //    break;
                    //case "Recomendacion":
                    //    Response.Redirect("ItemRecomendaciones.aspx?id=" + e.CommandArgument);
                    //    break;
                    case "Eliminar":
                        Eliminar(e.CommandArgument.ToString());
                        CargarGrilla();
                        break;
                }
            }
        }

        private void Eliminar(object idItem)
        { 
            Usuario oUser = new Usuario();
            IndicenciaRecepcion oRegistro = new IndicenciaRecepcion();
            oRegistro = (IndicenciaRecepcion)oRegistro.Get(typeof(IndicenciaRecepcion), int.Parse(idItem.ToString()));
           
            oRegistro.Baja = true;
            oRegistro.IdUsuario = oUser.IdUsuario;
            

            oRegistro.Save();
        }

       

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                


                ImageButton CmdModificar = (ImageButton)e.Row.Cells[6].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modificar";

           

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[7].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }

             

           
            }  

        }

       

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvLista.PageIndex = 0;
            CargarGrilla();            
            CurrentPageLabel.Text = " ";
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

            CargarGrilla();

           
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            
        }

        private void MostrarInforme(string tipo)
        {

           
         



        }

        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            MostrarInforme("Nomenclador");
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {   gvLista.PageIndex = 0;
            CargarGrilla();
         
            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
            CargarGrilla();
           
            CurrentPageLabel.Text = " ";
        }

        protected void lnkPdfReducido_Click(object sender, EventArgs e)
        {
            MostrarInforme("Reducido");
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
            gvLista.PageIndex = 0;
            CargarGrilla();
            
            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged1(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
            CargarGrilla();
           
            CurrentPageLabel.Text = " ";
        }

        protected void gvLista_DataBound(object sender, EventArgs e)
        {

          

            
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            
          
                        Response.Redirect("IncidenciaRecepcionEdit.aspx",false);
        }
    }
}
