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
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;


namespace WebLab.HojasTrabajo
{
    public partial class HTList : System.Web.UI.Page
    {
       public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();
       
       protected void Page_PreInit(object sender, EventArgs e)
       { 
           oCr.Report.FileName = "";           
           oCr.CacheDuration=0;
           oCr.EnableCaching=false;
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Hoja de Trabajo Edit");
                    CargarLista();
                    CargarGrilla();
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
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
        private void CargarLista()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura
                                                                                                        ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio  WHERE (baja = 0) and idTipoServicio<5";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
            CargarArea();
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
                    case 1: btnAgregar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        private void CargarGrilla()
        {
           // gvLista.AutoGenerateColumns = false;
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {

           
            string m_condicion = "  and  HT.idEfector=" + oUser.IdEfector.IdEfector.ToString();
            if (ddlArea.SelectedValue != "0")   m_condicion += " and HT.idArea= " + ddlArea.SelectedValue;
            if (ddlServicio.SelectedValue != "0") m_condicion += " and A.idTipoServicio= " + ddlServicio.SelectedValue;

            string m_strSQL = @" SELECT HT.idHojaTrabajo AS idHojaTrabajo, A.nombre AS area, TS.nombre AS servicio, HT.codigo AS codigo
                                FROM LAB_HojaTrabajo AS HT 
                                INNER JOIN  LAB_Area AS A ON HT.idArea = A.idArea 
                                INNER JOIN LAB_TipoServicio AS TS ON A.idTipoServicio = TS.idTipoServicio
                               WHERE     (HT.baja = 0) AND (A.baja = 0) " + m_condicion+      " order by A.nombre";
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("HojaTrabajoEdit.aspx");
        }





        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
                Response.Redirect("HojaTrabajoEdit.aspx?id=" + e.CommandArgument);
            if (e.CommandName == "Eliminar")
            {
                Eliminar(e.CommandArgument);
                CargarGrilla();
            }
            if (e.CommandName == "Pdf")
            {

                Imprimir(e.CommandArgument);
            }
        }

        private void Imprimir(object p)
        {

            HojaTrabajo oRegistro = new HojaTrabajo();
            oRegistro = (HojaTrabajo)oRegistro.Get(typeof(HojaTrabajo), int.Parse(p.ToString()));

            Configuracion oCon = new Configuracion();
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2 + " Vista Preeliminar - No es generacion desde protocolos";

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;

            ParameterDiscreteValue imprimirFechaHora = new ParameterDiscreteValue();
            imprimirFechaHora.Value = oRegistro.ImprimirFechaHora;

            string nombre_reporte = "";
            string m_reporte = "Reporte.pdf";
         
            if (oRegistro.Formato == 0)
            {
                Utility oUtil = new Utility();
               
                m_reporte = oUtil.CompletarNombrePDF(oRegistro.Codigo + "_VistaPreeliminar");
                
                switch (oRegistro.FormatoAncho)
                {
                    //case 0: 
                    //    oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo1.rpt"; break;
                    //case 1: oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo2.rpt"; break;
                    //case 2: oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo3.rpt"; break;

                    case 0:
                        {
                            if (oCon.TipoNumeracionProtocolo == 2)
                              nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra1";
                            else
                                nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo1";
                        }
                        break;
                    case 1:
                        {
                            if (oCon.TipoNumeracionProtocolo == 2)
                        nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra2";
                            else
                                nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo2";
                        } break;
                    case 2:
                        {
                            if (oCon.TipoNumeracionProtocolo == 2)
                                nombre_reporte= "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra3";
                            else
                                nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo3";
                        } break;

                    case 3://texto corto con numero de fila
                        {
                            if (oCon.TipoNumeracionProtocolo == 2)
                                nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra3";
                            else
                                nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo4";
                        } break;
                }                              
            }
            //if (oRegistro.Formato == 1)
            //{
            //    m_reporte = oRegistro.Codigo ;

            //    switch (oRegistro.FormatoAncho)
            //    {
            //        case 0: oCr.Report.FileName = "../iNFORMES/HojasdeTrabajo/HTrabajoAnalisis1.rpt"; break;
            //        case 1: oCr.Report.FileName = "../iNFORMES/HojasdeTrabajo/HTrabajoAnalisis2.rpt"; break;
            //        case 2: oCr.Report.FileName = "../iNFORMES/HojasdeTrabajo/HTrabajoAnalisis3.rpt"; break;
            //    }
               
            //}

            if (oRegistro.IdArea.IdTipoServicio.IdTipoServicio ==3)  //microbiolgoia
                nombre_reporte = nombre_reporte + "Microbiologia";

            if (!oRegistro.TipoHoja) // 0: Horizontal
                nombre_reporte = nombre_reporte + "Horizontal";

            if (!oRegistro.AgrupaFecha) // sin agrupar por fecha
                nombre_reporte = nombre_reporte + "SA";

            nombre_reporte = nombre_reporte + ".rpt";


            oCr.Report.FileName = nombre_reporte;
            oCr.ReportDocument.SetDataSource(oRegistro.GetDataSet_HojaTrabajoPreeliminar());
        
            //if (!oRegistro.TipoHoja) // 0: Horizontal
            //    oCr.ReportDocument.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            //else //1: vertical
            //    oCr.ReportDocument.PrintOptions.PaperOrientation = PaperOrientation.Portrait;

            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(imprimirFechaHora);
     

            oCr.DataBind();

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, m_reporte );
             

        }

        private void Eliminar(object p)
        {
            HojaTrabajo oRegistro = new HojaTrabajo();
            oRegistro = (HojaTrabajo)oRegistro.Get(typeof(HojaTrabajo), int.Parse(p.ToString()));
           // Usuario oUser = new Usuario();
            oRegistro.Baja = true;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

                ImageButton CmdPdf = (ImageButton)e.Row.Cells[4].Controls[1];
                CmdPdf.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdPdf.CommandName = "Pdf";
                CmdPdf.ToolTip = "Vista Preeliminar";
                
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[5].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }
            }
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        { 
        

          CargarGrilla();    CargarArea();
           
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas            
            string m_ssql ="";
          if (ddlServicio.SelectedValue!="0")
 m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
          else
              m_ssql = "select idArea, nombre from Lab_Area where baja=0   order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));

            m_ssql = null;
            oUtil = null;  
        //    ddlArea.UpdateAfterCallBack = true;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}
