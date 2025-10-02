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
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Data.SqlClient;
using Business.Data.Laboratorio;
using CrystalDecisions.Web;
using System.Text;
using Business.Data;

namespace WebLab.Informes
{
    public partial class Informe : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            //oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Session["idUsuario"] != null)
            {
                //oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Inicializar();
                txtProtocoloDesde.Focus();                                                       
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
        private void MarcarSectoresSeleccionados(bool p)
        {
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                lstSector.Items[i].Selected = p;
            }



        }
        
        private void Inicializar()
        {   
            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            CargarListas();

            switch (Request["Tipo"].ToString())
            {
                case "HojaTrabajo":
                    {
                        VerificaPermisos("Hoja de Trabajo");
                        lblTitulo.Text = "HOJA DE TRABAJO";
                        pnlEtiquetaCodigoBarras.Visible = false;
                        pnlHojaTrabajo.Visible = true;
                        pnlAnalisisFueraHT.Visible = false;
                        pnlHojaTrabajoResultado.Visible = true;
                        chkDesdeUltimoListado.Visible = true;
                        //pnlHojaTrabajo.Visible = true;
                        //btnBuscar.Visible = false;
                        txtLoteDesde.Visible = false;
                        txtLoteHasta.Visible = false;
                        lblLoteDesde.Visible = false;   
                        lblLoteHasta.Visible = false;
                        CargarGrilla();

                    } break;
                case "AnalisisFueraHT":
                    {
                        VerificaPermisos("HT a Demanda");
                        lblTitulo.Text = "HOJA DE TRABAJO A DEMANDA";
                        pnlEtiquetaCodigoBarras.Visible =false;
                        pnlHojaTrabajo.Visible = false;
                        pnlHojaTrabajoResultado.Visible = false;
                        chkDesdeUltimoListado.Visible = false;
                        pnlAnalisisFueraHT.Visible = true;
                    //     CargarAnalisisFueraHT();
                    //pnlHojaTrabajo.Visible = false;
                    //   btnBuscar.Visible = true;
                        txtLoteDesde.Visible = false;
                        txtLoteHasta.Visible = false;
                        lblLoteDesde.Visible = false;
                        lblLoteHasta.Visible = false;
                }
                    break;

                case "CodigoBarras":
                    {
                        VerificaPermisos("Etiqueta Codigo Barras");
                        lblTitulo.Text = "IMPRESION DE ETIQUETAS DE CODIGO DE BARRAS DE DERIVACIONES";
                        pnlEtiquetaCodigoBarras.Visible = true;
                        pnlHojaTrabajo.Visible = false;
                        chkDesdeUltimoListado.Visible = false;
                        pnlHojaTrabajoResultado.Visible = false;
                        pnlAnalisisFueraHT.Visible = false;
                        rdbEstadoAnalisis.Visible = false; tituloEstado.Visible = false;
                        txtLoteDesde.Visible = true; 
                        txtLoteHasta.Visible = true;

                    }
                    break;
            }                        
          
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarListas()
        {
            try
            {
                Utility oUtil = new Utility();
                string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

                ///Carga de combos de tipos de servicios
                string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE (baja = 0)";
                if (Request["Tipo"].ToString() == "HojaTrabajo") // solo los servicios que tienen hoja de trabajo
                    m_ssql = m_ssql + " and idTipoServicio in (select distinct A.idTipoServicio from lab_hojatrabajo as H inner join LAB_Area as A on A.idarea = H.idarea where A.baja=0 )";
                oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);

                lblCaracterCovid.Visible = false;
                chkCaracterCovid.Visible = false;
                m_ssql = "SELECT idCaracter,  nombre as nombre  FROM LAB_Caracter (nolock) ";
                oUtil.CargarCheckBox(chkCaracterCovid, m_ssql, "idCaracter", "nombre");
                for (int i = 0; i < chkCaracterCovid.Items.Count; i++)
                {
                   // lblCaracterCovid.Visible = true;
                    chkCaracterCovid.Items[i].Selected = true;
                }

                if ((chkCaracterCovid.Items.Count > 1) && (oUser.IdEfector.IdEfector == 228))/// particularidad para labo central: ver com oparametrizar
                {
                    lblCaracterCovid.Visible = true;
                    chkCaracterCovid.Visible = true;
                }
                ///Carga de combos de Origen
                m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen (nolock)  WHERE (baja = 0)";
                oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre", connReady);
                ddlOrigen.Items.Insert(0, new ListItem("-- Todos --", "0"));

                ///Carga de combos de Prioridad
                m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad (nolock)  WHERE (baja = 0)";
                oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre", connReady);
                ddlPrioridad.Items.Insert(0, new ListItem("-- Todos --", "0"));


                  m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora with (nolock) where idEfector=" + oUser.IdEfector.IdEfector.ToString() + " order by nombre";  //MultiEfector

                oUtil.CargarCombo(ddlImpresoraCB, m_ssql, "nombre", "nombre", connReady);

                ddlImpresora.Items.Insert(0, new ListItem("Seleccione impresora", "0"));


                if (Request["Tipo"].ToString() == "HojaTrabajo") ddlPrioridad.SelectedValue = "1";//rutina

                ///Carga de Sectores
                m_ssql = "SELECT idSectorServicio, prefijo + ' - ' + nombre as nombre  FROM LAB_SectorServicio (nolock)  WHERE (baja = 0) order by nombre";
                oUtil.CargarListBox(lstSector, m_ssql, "idSectorServicio", "nombre");

                for (int i = 0; i < lstSector.Items.Count; i++)
                {
                    lstSector.Items[i].Selected = true;
                }
                //////////////////

                ///Carga de combos de areas
                CargarArea();

                ///Carga de Efectores solicitantes
                m_ssql = "SELECT idEfector, nombre FROM sys_Efector (nolock)  order by nombre ";
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
                ddlEfector.Items.Insert(0, new ListItem("-- Todos --", "0"));
                //ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();

                if (pnlAnalisisFueraHT.Visible)
                {
                    pnlAnalisisFueraHT.Visible = true;
                    CargarAnalisisFueraHT();
                }

                m_ssql = "SELECT  E.idEfector, E.nombre " +
             " FROM  Sys_Efector AS E " +
             " where E.idEfector IN  (SELECT DISTINCT idEfectorDerivacion FROM   lab_itemEfector AS IE  WHERE Ie.disponible=1 and IE.idEfector<>Ie.idEfectorDerivacion and  IE.idEfector=" + oUser.IdEfector.IdEfector.ToString() + ")" +
             "    ORDER BY E.nombre";
                //oUtil.CargarListBox(lstEfectores, m_ssql, "idEfector", "nombre");
                oUtil.CargarCombo(ddlEfectorDestino, m_ssql, "idEfector", "nombre");
                ddlEfectorDestino.Items.Insert(0, new ListItem("--Seleccione--", "0"));
                ///////////////Impresoras////////////////////////

                /*En multieFEctor esta parte de codigo no va porque refiere a impresora - no etiquetadora*/

                if (oCon.esMultiEFector())
                {
                    pnlImpresora.Visible = false;
                    lnkImprimir.Visible = false;
                    lnkImprimirAnalisisFueraHT.Visible = false;
                    // ddlImpresora.Visible = false;
                    //lnkImprimir.Visible = false;
                }
                else
                {
                    m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora (nolock) where idEfector=" + oUser.IdEfector.IdEfector.ToString() + " order by nombre";  
                    oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre", connReady);
                    if (Request["Tipo"].ToString() == "CodigoBarras")
                    { if (Session["Etiquetadora"] != null) ddlImpresora.SelectedValue = Session["Etiquetadora"].ToString(); }
                    else
                        if (Session["Impresora"] != null) ddlImpresora.SelectedValue = Session["Impresora"].ToString();

                    if (ddlImpresora.Items.Count == 0)
                    {
                        pnlImpresora.Visible = false;
                        lnkImprimir.Visible = false;
                        lnkImprimirAnalisisFueraHT.Visible = false;
                        // ddlImpresora.Visible = false;
                        //lnkImprimir.Visible = false;
                    }
                }


                /////////////////////////////////////////////

                m_ssql = null;
                oUtil = null;
            }
            catch (Exception ex)
            { }
        }

        private void CargarAnalisisFueraHT()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_condicion = "";
            if (ddlArea.SelectedValue != "0") m_condicion += " and I.idArea= " + ddlArea.SelectedValue;
            if ((ddlServicio.SelectedValue != "0")&& (ddlServicio.SelectedValue != "")) m_condicion += " and A.idTipoServicio= " + ddlServicio.SelectedValue;


            string m_ssql = @"select I.idItem,  I.nombre+ '  [' + I.codigo + ']' as nombre from lab_item I with (nolock)
inner join LAB_ItemEfector IE  with (nolock) on i.idItem = ie.idItem
inner join lab_area A  with (nolock) on A.idArea = I.idArea
where Ie.idEfector = " + oUser.IdEfector.IdEfector.ToString()+ @"  and i.baja = 0
and ie.disponible=1
and ie.idEfector= ie.idEfectorDerivacion " + m_condicion+ @" order by I.nombre ";
            oUtil.CargarListBox(lstAnalisis, m_ssql, "idItem", "nombre", connReady);   

            
        }


        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
            if (pnlHojaTrabajo.Visible)
            CargarGrilla();

            if (pnlAnalisisFueraHT.Visible)
                CargarAnalisisFueraHT();
            if (ddlServicio.SelectedValue == "3")
            { btnSeleccionarTipoMuestra.Visible = true;
            btnSeleccionarTipoMuestra.UpdateAfterCallBack = true;
            }
            else
            {
                btnSeleccionarTipoMuestra.Visible = false;
                lstMuestra.Visible = false;
                lstMuestra.UpdateAfterCallBack = true;
                btnSeleccionarTipoMuestra.UpdateAfterCallBack = true;
            }
            
            //ddlArea.UpdateAfterCallBack = true;
        }

        private void CargarTipoMuestra()
        {
            Utility oUtil = new Utility();

            string m_ssql = "SELECT idMuestra, nombre + ' - ' + codigo as nombre FROM LAB_Muestra with (nolock)   where baja=0  order by nombre ";
            
            oUtil.CargarListBox(lstMuestra, m_ssql, "idMuestra", "nombre");
            for (int i = 0; i < lstMuestra.Items.Count; i++)
            {
                lstMuestra.Items[i].Selected = true;
            }
            lstMuestra.Visible = true;
           

        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            string m_stroCondicion ="";
            if (Request["Tipo"].ToString() == "CodigoBarras")   m_stroCondicion = " and imprimeCodigoBarra=1 ";
            if (ddlServicio.SelectedValue!="")  m_stroCondicion += "and idTipoServicio = " + ddlServicio.SelectedValue;
            string m_ssql = "select idArea, nombre from Lab_Area (nolock)  where baja=0  " + m_stroCondicion +" order by nombre";            
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);

            if (Request["Tipo"].ToString()=="CodigoBarras")  ddlArea.Items.Insert(0, new ListItem("--ETIQUETA GENERAL--", "0"));                    
            else  ddlArea.Items.Insert(0, new ListItem("Todas", "0"));                    
            
            m_ssql = null;
            oUtil = null;
           
        }

        private void MostrarInforme(string tipo, DataTable dt)
        {
            
            //Aca se deberá consultar los parametros para mostrar una hoja de trabajo u otra
            //this.CrystalReportSource1.Report.FileName = "HTrabajo2.rpt";

            try
            {

                //Configuracion oCon = new Configuracion();              oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                 
                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;

             //   string informe = "";
                string m_reporte = "Reporte.pdf";
                switch (Request["Tipo"].ToString())
                {
                    case "HojaTrabajo":
                        {
                            /*            if (oCon.TipoHojaTrabajo == 0) informe = "HTrabajo.rpt";
                                        if (oCon.TipoHojaTrabajo == 1) informe = "HTrabajo2.rpt"; 
                             * */

                            Business.Data.Laboratorio.Area oArea = new Business.Data.Laboratorio.Area();
                            oArea = (Business.Data.Laboratorio.Area)oArea.Get(typeof(Business.Data.Laboratorio.Area), int.Parse(ddlArea.SelectedValue));

                            HojaTrabajo oRegistro = new HojaTrabajo();
                            oRegistro = (HojaTrabajo)oRegistro.Get(typeof(HojaTrabajo), "IdArea", oArea);
                            if (oRegistro.Formato == 0)
                            {
                                switch (oRegistro.FormatoAncho)
                                {
                                    case 0: oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo1.rpt"; break;
                                    case 1: oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo2.rpt"; break;
                                    case 2: oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo3.rpt"; break;
                                    case 3: oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo4.rpt"; break;
                                } 
                                m_reporte = oRegistro.Codigo+ ".pdf";
                            }
                            if (oRegistro.Formato == 1)
                            {
                                switch (oRegistro.FormatoAncho)
                                {
                                    case 0: oCr.Report.FileName = "../iNFORMES/HojasdeTrabajo/HTrabajoAnalisis1.rpt"; break;
                                    case 1: oCr.Report.FileName = "../iNFORMES/HojasdeTrabajo/HTrabajoAnalisis2.rpt"; break;
                                    case 2: oCr.Report.FileName = "../iNFORMES/HojasdeTrabajo/HTrabajoAnalisis3.rpt"; break;
                                }               
                                m_reporte = oRegistro.Codigo+ ".pdf";
                            }

                            
                            oCr.ReportDocument.SetDataSource(dt);
                        } break;
                    case "MuestrasFaltantes": lblTitulo.Text = "MUESTRAS FALTANTES"; break;
                }




                //            this.CrystalReportSource1.ReportDocument.SetDataSource(GetDataSet_HojaTrabajo());

                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                if (tipo == "Imprimir")//imprimir
                {
                    try
                    {
                        Session["Impresora"] = ddlImpresora.SelectedValue;
                        oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
                        oCr.ReportDocument.PrintToPrinter(1, true, 1, 100);
                    }
                    catch (Exception ex)
                    {
                        string exception = "";
                        //while (ex != null)
                        //{
                        //    exception = ex.Message + "<br>";

                        //}
                        string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Impresora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                        Page.RegisterStartupScript("PopupScript", popupScript);
                    }
                }
                else
                {
                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF(m_reporte);
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                    //MemoryStream oStream; // using System.IO
                    //oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("Content-Disposition", "attachment;filename=" + m_reporte);

                    //Response.BinaryWrite(oStream.ToArray());
                    //Response.End();
                }
            }
            catch
            { }

        }

        
        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Pdf")
            {
                if (Page.IsValid)
                    VistaPreeliminar(e.CommandArgument, "PDF");
            }


            if (e.CommandName == "Imprimir")
            {
                if (Page.IsValid)
                    VistaPreeliminar(e.CommandArgument, "Impresora");
            }

            if (e.CommandName == "Excel")
            {
                if (Page.IsValid)
                    VistaPreeliminar(e.CommandArgument, "Excel");
            }
        }

     

        
        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }

        }
        private void VistaPreeliminar(object p, string accion)
        {
            HojaTrabajo oRegistro = new HojaTrabajo();
            oRegistro = (HojaTrabajo)oRegistro.Get(typeof(HojaTrabajo), int.Parse(p.ToString()));
            if (oRegistro != null)
            {
                string m_reporte = "";
                m_reporte = oRegistro.Codigo;


                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF(m_reporte);


                DataTable dt = new DataTable();
                dt = GetDataSet_HojaTrabajo(p);


                if (dt.Rows.Count == 0)
                {
                    if (accion == "PDF")
                    {
                        string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para la hoja de trabajo seleccionada'); </script>";
                        Page.RegisterStartupScript("PopupScript", popupScript);
                    }

                }
                else
                {
                    if (accion == "Excel")
                        ExportarExcel(dt, nombrePDF);
                    else
                    {

                        //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                        ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                        encabezado1.Value = oCon.EncabezadoLinea1;

                        ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                        encabezado2.Value = oCon.EncabezadoLinea2;

                        ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                        encabezado3.Value = oCon.EncabezadoLinea3;

                        ParameterDiscreteValue imprimirFechaHora = new ParameterDiscreteValue();
                        imprimirFechaHora.Value = oRegistro.ImprimirFechaHora;


                        string nombre_reporte = "";
                        if (oRegistro.Formato == 0) ///unico formato
                        {

                            switch (oRegistro.FormatoAncho)
                            {
                                case 0:
                                    {
                                        if (oCon.TipoNumeracionProtocolo == 2)
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra1";
                                        else
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo1";
                                        //oCr.Report.FileName = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo1.rpt"; 
                                    }
                                    break;
                                case 1:
                                    {
                                        if (oCon.TipoNumeracionProtocolo == 2)
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra2";
                                        else
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo2";
                                    }
                                    break;
                                case 2:
                                    {
                                        if (oCon.TipoNumeracionProtocolo == 2)
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra3";
                                        else
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo3";
                                    }
                                    break;
                                case 3://texto corto con numero de fila
                                    {
                                        if (oCon.TipoNumeracionProtocolo == 2)
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocoloLetra3";
                                        else
                                            nombre_reporte = "../iNFORMES/HojasdeTRabajo/HTrabajoProtocolo4";
                                    }
                                    break;
                            }
                        }
                        if (ddlServicio.SelectedValue == "3")  //microbiolgoia
                            nombre_reporte = nombre_reporte + "Microbiologia";

                        if (ddlServicio.SelectedValue == "4")  //microbiolgoia
                        {
                            nombre_reporte = nombre_reporte + "Pesquisa";
                            if (txtFechaDesde.Value == txtFechaHasta.Value)
                                encabezado3.Value = txtFechaDesde.Value;
                            else
                                encabezado3.Value = txtFechaDesde.Value + " - " + txtFechaHasta.Value;
                        }

                        if (!oRegistro.TipoHoja) // 0: Horizontal
                            nombre_reporte = nombre_reporte + "Horizontal";

                        if (!oRegistro.AgrupaFecha) // sin agrupar por fecha
                            nombre_reporte = nombre_reporte + "SA";

                        nombre_reporte = nombre_reporte + ".rpt";
                        oCr.Report.FileName = nombre_reporte;


                        oCr.ReportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;
                        oCr.ReportDocument.SetDataSource(dt);

                        oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                        oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                        oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                        oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(imprimirFechaHora);
                        oCr.DataBind();

                    }
                    if (accion == "PDF")
                    {

                        oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

                        //oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF+".pdf");


                    }
                    else
                    {
                        if (accion == "Impresora")
                        {
                            try
                            {
                                Session["Impresora"] = ddlImpresora.SelectedValue.ToString();
                                oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue; // ConfigurationSettings.AppSettings["Impresora"]; 
                                oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);
                            }
                            catch (Exception ex)
                            {
                                string exception = "";
                                //while (ex != null)
                                //{
                                exception = ex.Message + "<br>";

                                //}
                                string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Impresora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                                Page.RegisterStartupScript("PopupScript", popupScript);
                            }
                        }
                    }
                }
            }
        }

        private void ExportarExcel(DataTable dt, string nom)
        {
            
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Page pagina = new Page();
                HtmlForm form = new HtmlForm();
                GridView dg = new GridView();
                dg.EnableViewState = false;
                dg.DataSource = dt;
                dg.DataBind();
                pagina.EnableEventValidation = false;
                pagina.DesignerInitialize();
                pagina.Controls.Add(form);
                form.Controls.Add(dg);
                pagina.RenderControl(htw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + nom + ".xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.Write(sb.ToString());
                Response.End();
            
        }
     

     


        private DataTable GetDataSet_HojaTrabajo(object p)
        {

            string s_store = "LAB_GeneraHT";
            DataSet Ds = new DataSet();
                   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
           // SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            // Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (rdbHojaConResultados.SelectedValue == "0")
               s_store = "LAB_GeneraHT";
            else                           
               s_store = "LAB_GeneraHTconResultados";
              
            
            if (ddlServicio.SelectedValue=="3") s_store = s_store + "Microbiologia";
            if (ddlServicio.SelectedValue=="4") s_store = s_store + "Pesquisa";

            cmd.CommandText= s_store;
            /////Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            /////


            ///////Parametro hoja de trabajo
            cmd.Parameters.Add("@idHojaTrabajo", SqlDbType.NVarChar); 
            cmd.Parameters["@idHojaTrabajo"].Value = p.ToString();        

            ///////Parametro @idEfectorSolicitante
            cmd.Parameters.Add("@idEfectorSolicitante", SqlDbType.NVarChar); 
            cmd.Parameters["@idEfectorSolicitante"].Value = ddlEfector.SelectedValue;

            ///////Parametro @@idOrigen
            cmd.Parameters.Add("@idOrigen", SqlDbType.NVarChar); 
            cmd.Parameters["@idOrigen"].Value = ddlOrigen.SelectedValue;

            ///////Parametro @@@idPrioridad
            cmd.Parameters.Add("@idPrioridad", SqlDbType.NVarChar); 
            cmd.Parameters["@idPrioridad"].Value = ddlPrioridad.SelectedValue;
            
            
            ///////Parametro @@@@idSector        

            cmd.Parameters.Add("@idSector", SqlDbType.NVarChar);
            cmd.Parameters["@idSector"].Value = getListaSectores(true);  // getListaSectores();// ddlSectorServicio.SelectedValue;


            ///////Parametro @@@@idSector
            cmd.Parameters.Add("@estado", SqlDbType.NVarChar);
            cmd.Parameters["@estado"].Value = rdbEstadoAnalisis.SelectedValue;

            ///////Parametro @@@@numeroDesde
            cmd.Parameters.Add("@numeroDesde", SqlDbType.NVarChar);
            cmd.Parameters["@numeroDesde"].Value = txtProtocoloDesde.Value;


            ///////Parametro @@@@numeroDesde
            cmd.Parameters.Add("@numeroHasta", SqlDbType.NVarChar);
            cmd.Parameters["@numeroHasta"].Value = txtProtocoloHasta.Value;


            cmd.Parameters.Add("@desdeUltimoNumero", SqlDbType.Int);
            if (chkDesdeUltimoListado.Checked)
                cmd.Parameters["@desdeUltimoNumero"].Value = 1;
            else
                cmd.Parameters["@desdeUltimoNumero"].Value = 0;


            cmd.Parameters.Add("@tipoMuestra", SqlDbType.NVarChar);
            if (ddlServicio.SelectedValue == "3")
             

                cmd.Parameters["@tipoMuestra"].Value = getListaMuestra();
             
            else
                cmd.Parameters["@tipoMuestra"].Value = "0";

            cmd.Parameters.Add("@idCaracter", SqlDbType.NVarChar);
            cmd.Parameters["@idCaracter"].Value = getListaCaracter(true);

            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            conn.Close();
            return Ds.Tables[0];

        }

        private object getListaCaracter(bool v)
        {
            string m_lista = "";
            bool todasseleccionadas = true;
            for (int i = 0; i < chkCaracterCovid.Items.Count; i++)
            {
                if (chkCaracterCovid.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = chkCaracterCovid.Items[i].Value;
                    else
                        m_lista += "," + chkCaracterCovid.Items[i].Value;
                }
                else todasseleccionadas = false;

            }
            if (  (todasseleccionadas)) m_lista = "";
            return m_lista;
        }

        protected void btnSeleccionarTipoMuestra_Click(object sender, EventArgs e)
        {
            if (lstMuestra.Visible) { lstMuestra.Visible = false; btnSeleccionarTipoMuestra.Text = "Elegir Tipo de muestra"; }
            else
            {
                btnSeleccionarTipoMuestra.Text = "Ocultar Tipo de Muestra";
                CargarTipoMuestra();
            }
            btnSeleccionarTipoMuestra.UpdateAfterCallBack = true;
            lstMuestra.UpdateAfterCallBack = true;
        }

        private string getListaMuestra()
        {
            string m_lista = "";
            for (int i = 0; i < lstMuestra.Items.Count; i++)
            {
                if (lstMuestra.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstMuestra.Items[i].Value;
                    else
                        m_lista += "," + lstMuestra.Items[i].Value;
                }
            }
            return m_lista;
        }

        //private object getListaSectores()
        //{
        //    string m_lista = "";
        //    for (int i = 0; i <lstSector.Items.Count; i++)
        //    {
        //        if (lstSector.Items[i].Selected)
        //        {
        //            if (m_lista == "")
        //                m_lista = lstSector.Items[i].Value;
        //            else
        //                m_lista += "," + lstSector.Items[i].Value;
        //        }
        //    }
        //    return m_lista;
        //}

        private string getListaSectores(bool filtro)
        {
            string m_lista = "";
            bool todasseleccionadas = true;
            for (int i = 0; i < lstSector.Items.Count; i++)
            {
                if (lstSector.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstSector.Items[i].Value;
                    else
                        m_lista += "," + lstSector.Items[i].Value;
                }
                else todasseleccionadas = false;

            }
            if ((filtro) && (todasseleccionadas)) m_lista = "";
            return m_lista;
        }

        private DataTable GetDataSet_AnalisisFueraHT()
        {
            DataSet Ds = new DataSet();
               SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
         //   SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.CommandText = "LAB_AnalisisFueraHT";


            /////Parametros de fechas           
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);
            cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            /////


            ///////Parametro hoja de trabajo



            ///////Parametro @idEfectorSolicitante
            cmd.Parameters.Add("@idEfectorSolicitante", SqlDbType.NVarChar);
            cmd.Parameters["@idEfectorSolicitante"].Value = ddlEfector.SelectedValue;

            ///////Parametro @@idOrigen
            cmd.Parameters.Add("@idOrigen", SqlDbType.NVarChar);
            cmd.Parameters["@idOrigen"].Value = ddlOrigen.SelectedValue;

            ///////Parametro @@@idPrioridad
            cmd.Parameters.Add("@idPrioridad", SqlDbType.NVarChar);
            cmd.Parameters["@idPrioridad"].Value = ddlPrioridad.SelectedValue;


            ///////Parametro @@@@idSector
            cmd.Parameters.Add("@idSector", SqlDbType.NVarChar);
            cmd.Parameters["@idSector"].Value = getListaSectores(true); // ddlSectorServicio.SelectedValue;


            ///////Parametro @@@@idSector
            cmd.Parameters.Add("@listaItem", SqlDbType.NVarChar);
            cmd.Parameters["@listaItem"].Value = getListaAnalisis();


            ///////Parametro @@@@numeroDesde
            cmd.Parameters.Add("@numeroDesde", SqlDbType.NVarChar);
            cmd.Parameters["@numeroDesde"].Value = txtProtocoloDesde.Value;


            ///////Parametro @@@@numeroDesde
            cmd.Parameters.Add("@numeroHasta", SqlDbType.NVarChar);
            cmd.Parameters["@numeroHasta"].Value = txtProtocoloHasta.Value;


            ///////Parametro @@@@numeroDesde
            cmd.Parameters.Add("@estado", SqlDbType.NVarChar);
            cmd.Parameters["@estado"].Value = rdbEstadoAnalisis.SelectedValue;


            ///////Parametro @@@@numeroDesde
            cmd.Parameters.Add("@idEfector", SqlDbType.NVarChar);
            cmd.Parameters["@idEfector"].Value = oUser.IdEfector.IdEfector.ToString();

            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            conn.Close();
            return Ds.Tables[0];
        }

        private object getListaAnalisis()
        {
            string m_lista = "";
            for (int i = 0; i < lstAnalisis.Items.Count; i++)
            {
                if (lstAnalisis.Items[i].Selected)
                {
                    if (m_lista == "")
                        m_lista = lstAnalisis.Items[i].Value;
                    else
                        m_lista += "," + lstAnalisis.Items[i].Value;
                }

            }
            return m_lista;
        }


        private void VistaPreeliminar_AnalisisFueraHT( string accion)
        {

            DataTable dt = new DataTable();
            dt = GetDataSet_AnalisisFueraHT();

            if (dt.Rows.Count == 0)
            {               
                    string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para los filtros de busqueda seleccionados'); </script>";
                    Page.RegisterStartupScript("PopupScript", popupScript);               
            }
            else
            {               

                //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;

                oCr.Report.FileName = "../iNFORMES/AnalisisFueraHT.rpt";                
                oCr.ReportDocument.SetDataSource(dt);
                
              
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();


                if (accion == "PDF")
                {
                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF("HTaDemanda");
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                       
                }
                else
                {
                    try
                    {
                        Session["Impresora"] = ddlImpresora.SelectedValue;
                        oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
                        oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        string exception = "";
                        //while (ex != null)
                        //{
                            exception = ex.Message + "<br>";

                        //}
                        string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Impresora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                        Page.RegisterStartupScript("PopupScript", popupScript);
                    }
                }
            }





        }
        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                ImageButton CmdPdf = (ImageButton)e.Row.Cells[4].Controls[1];
                CmdPdf.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdPdf.CommandName = "Pdf";
                CmdPdf.ToolTip = "Vista Preeliminar";

                ImageButton CmdAdicional = (ImageButton)e.Row.Cells[5].Controls[1];
                CmdAdicional.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdAdicional.CommandName = "Imprimir";
                CmdAdicional.ToolTip = "Imprimir";

                ImageButton CmdExcel = (ImageButton)e.Row.Cells[6].Controls[1];
                CmdExcel.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdExcel.CommandName = "Excel";
                CmdExcel.ToolTip = "Excel";

             
                if (pnlImpresora.Visible == false)
                {
                    CmdAdicional.Visible = false;
                }
            }
        }

        private void CargarGrilla()
        {
          //  gvLista.AutoGenerateColumns = false;
         
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            MarcarSeleccionados(false);
        }

        private object LeerDatos()
        {
            string m_condicion = " and HT.idEfector =  " + oUser.IdEfector.IdEfector.ToString();
            if (ddlArea.SelectedValue != "0") m_condicion += " and HT.idArea= " + ddlArea.SelectedValue;
            if ((ddlServicio.SelectedValue != "0")&& (ddlServicio.SelectedValue != "")) m_condicion += " and A.idTipoServicio= " + ddlServicio.SelectedValue;

            string m_strSQL = @" SELECT HT.idHojaTrabajo AS idHojaTrabajo, A.nombre AS area, TS.nombre AS servicio, HT.codigo AS codigo 
                                FROM  LAB_HojaTrabajo AS HT (nolock) INNER JOIN 
                                LAB_Area AS A (nolock) ON HT.idArea = A.idArea INNER JOIN  
                                LAB_TipoServicio AS TS (nolock) ON A.idTipoServicio = TS.idTipoServicio 
                                WHERE     (HT.baja = 0) AND (A.baja = 0) " + m_condicion +
                                @" order by A.nombre";
            DataSet Ds = new DataSet();
            //      SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            conn.Close();
            return Ds.Tables[0];
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                CargarGrilla();
               
            }

        }

        protected void lnkPDF_Click(object sender, EventArgs e)
        {
         
        }

        
        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    VistaPreeliminar(gvLista.DataKeys[row.RowIndex].Value.ToString(), "Impresora");                    
                }
            }
            

         
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else                            
            if (txtFechaHasta.Value == "") args.IsValid = false;
            else args.IsValid = true;

        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlHojaTrabajo.Visible)
            CargarGrilla();
            if (pnlAnalisisFueraHT.Visible)
            CargarAnalisisFueraHT(); 
          //  gvLista.UpdateAfterCallBack = true;
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        protected void lnkImprimirAnalisisFueraHT_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                VistaPreeliminar_AnalisisFueraHT ("Impresora");    
            }
        }

        protected void lnkPDFAnalisisFueraHT_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                VistaPreeliminar_AnalisisFueraHT("PDF");
            }
        }

        protected void lnkDesmarcarAnalisis_Click(object sender, EventArgs e)
        {
            MarcarSeleccionadosAnalisis(false);
        }

        private void MarcarSeleccionadosAnalisis(bool p)
        {
            for (int i = 0; i < lstAnalisis.Items.Count; i++)
            {
                lstAnalisis.Items[i].Selected = p;
            }
                
                
        }

        protected void lnkMarcarAnalisis_Click(object sender, EventArgs e)
        {
            MarcarSeleccionadosAnalisis(true);
        }

        protected void btnImprimirEtiqueta_Click(object sender, EventArgs e)
        {
            //  GenerarEtiquetas();
            if (Page.IsValid)            
            {
                DataTable dt = new DataTable();
                dt = GetDataSet_Etiquetas();
                foreach (DataRow item in dt.Rows)
                {

                    int reg_idProtocolo = int.Parse(item[0].ToString());
                    Business.Data.Laboratorio.Protocolo oProt = new Business.Data.Laboratorio.Protocolo();
                    oProt = (Business.Data.Laboratorio.Protocolo)oProt.Get(typeof(Business.Data.Laboratorio.Protocolo), reg_idProtocolo);

                    ///Imprimir codigo de barras.


                    ImprimirCodigoBarrasAreas(oProt, ddlArea.SelectedValue, ddlImpresoraCB.SelectedItem.Text);

                }
            }
           

        }
        private void ImprimirCodigoBarrasAreas(Protocolo oProt, string s_listaAreas, string impresora)
        {

            string[] tabla = s_listaAreas.Split(',');


            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                string s_area = tabla[i].ToUpper();
                if (s_area == "0")
                    s_area = "-1";
                if (s_area == "-1")
                    oProt.GrabarAuditoriaProtocolo("Imprime Etiqueta General", oUser.IdUsuario);
                else
                {
                    Business.Data.Laboratorio.Area oArea = new Business.Data.Laboratorio.Area();
                    oArea = (Business.Data.Laboratorio.Area)oArea.Get(typeof(Business.Data.Laboratorio.Area), int.Parse(s_area));
                    string s_narea = oArea.Nombre;
                    if (oArea.Nombre.Length > 25)
                        s_narea = oArea.Nombre.Substring(0, 25);

                    oProt.GrabarAuditoriaProtocolo("Imprime Etiqueta " + s_narea, oUser.IdUsuario);
                }


                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                string query = @" INSERT INTO LAB_ProtocoloEtiqueta
                    (idProtocolo ,idEfector  ,[idArea]  ,[idItem]      ,[impresora],fechaRegistro )
     VALUES   ( " + oProt.IdProtocolo.ToString() + "," + oUser.IdEfector.IdEfector.ToString() + "," + s_area + ",0,'" + impresora + "' , getdate()    )";
                SqlCommand cmd = new SqlCommand(query, conn);


                int idres = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void GenerarEtiquetas()
        {

            TipoServicio oTipoServicio= new TipoServicio();
            oTipoServicio = (TipoServicio)oTipoServicio.Get(typeof(TipoServicio), int.Parse(ddlServicio.SelectedValue));                           

            DataTable dt = new DataTable();
            dt = GetDataSet_Etiquetas();

            if (dt.Rows.Count == 0)
            {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para los filtros de busqueda seleccionados'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }
            else
            {
                ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra(); oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipoServicio);
                string sFuenteBarCode = oConBarra.Fuente;
                bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
                bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
                bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
                bool imprimeProtocoloNumeroOrigen=oConBarra.ProtocoloNumeroOrigen;
                bool imprimePacienteNumeroDocumento= oConBarra.PacienteNumeroDocumento;
                bool imprimePacienteApellido = oConBarra.PacienteApellido;
                bool imprimePacienteSexo = oConBarra.PacienteSexo;
                bool imprimePacienteEdad = oConBarra.PacienteEdad;


                foreach (DataRow item in dt.Rows)
                {                  

                    //DataRow reg = tabla.NewRow();
                    int reg_idProtocolo =int.Parse( item[0].ToString());
                    Business.Data.Laboratorio.Protocolo oProt = new Business.Data.Laboratorio.Protocolo();
                    oProt = (Business.Data.Laboratorio.Protocolo)oProt.Get(typeof(Business.Data.Laboratorio.Protocolo), reg_idProtocolo);
                    //reg_idArea"] = item[1].ToString();
                    string reg_numero = item[2].ToString();
                    string reg_area = item[3].ToString();
                    string reg_Fecha = item[4].ToString().Substring(0,10);
                    string reg_Origen = item[5].ToString();
                    string reg_Sector= item[6].ToString();
                    string reg_NumeroOrigen = item[7].ToString();
                    string reg_NumeroDocumento = oProt.IdPaciente.getNumeroImprimir();
                    string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);
                    string reg_sexo = item[10].ToString();
                    string reg_edad = item[11].ToString();
                    //tabla.Rows.Add(reg);
                    //tabla.AcceptChanges();

                    string reg_apellido="";
                    if (reg_codificaHIV == "FALSE")
                        reg_apellido = oProt.IdPaciente.Apellido + " " + oProt.IdPaciente.Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
                    else
                        reg_apellido = reg_sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");

                    if  (!imprimeProtocoloFecha) reg_Fecha = "          ";
                    if (!imprimeProtocoloOrigen) reg_Origen = "          ";
                    if (!imprimeProtocoloSector) reg_Sector = "   ";
                    if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
                    if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
                    if (!imprimePacienteApellido) reg_apellido = "";
                    if (!imprimePacienteSexo) reg_sexo = " ";
                    if (!imprimePacienteEdad) reg_edad = "   ";
                 

                    Business.Etiqueta ticket = new Business.Etiqueta();
                    ticket.TipoEtiqueta = oCon.TipoEtiqueta;
                    if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);

                    ticket.AddHeaderLine(reg_apellido.ToUpper());
                    ticket.AddSubHeaderLine(reg_sexo + " " +reg_edad + " "+ reg_NumeroDocumento + " " + reg_Fecha);
                    if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector)) ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);
                    if (reg_area != "") ticket.AddSubHeaderLineNegrita(reg_area);

                    // falta pasar por parametro la fuente de codigo de barras
                    ticket.AddCodigoBarras( reg_numero,sFuenteBarCode);
                    ticket.AddFooterLine(reg_numero );
                    
                    
                    Session["Etiquetadora"] = ddlImpresora.SelectedValue;
                    //oCr.ReportDocument.PrintOptions.PrinterName = Session["Etiquetadora"].ToString();// ConfigurationSettings.AppSettings["Impresora"]; 
                    try
                    {
                        ticket.PrintTicket(ddlImpresora.SelectedValue, oConBarra.Fuente);
                    }
                    catch (Exception ex)
                    {
                        string exception = "";
                     
                            exception = ex.Message + "<br>";

                     
                        string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Etiquetadora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                        Page.RegisterStartupScript("PopupScript", popupScript);
                        break;
                    }
                    
                }
            }
        }


        private DataTable GetDataSet_Etiquetas()
        {

            string m_strSQL = "";
            ///////////////establecer los filtros.
            string s_condicion = "";
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            s_condicion = " and A.idTipoServicio=" + ddlServicio.SelectedValue;
            s_condicion += " and P.idEfector=" + oUser.IdEfector.IdEfector.ToString();
            s_condicion += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "' AND P.Fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
            s_condicion += " and ((De.estado=4 and L.estado=1) or (De.estado =1 and L.estado =2))"; //Si esta Pendiente de Enviar la determinacion el lote esta creado, Si se derivo el lote la determinacion pasa a Enviada


            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            //if (txtProtocoloDesde.Value != "") s_condicion += " And numero>=" +(txtProtocoloDesde.Value);
            //if (txtProtocoloHasta.Value != "") s_condicion += " AND numero<=" +(txtProtocoloHasta.Value);

            if (txtProtocoloDesde.Value != "")
            {
		        switch (oCon.TipoNumeracionProtocolo )
                {
                    case 0:	s_condicion += " and P.numero>=" +(txtProtocoloDesde.Value); break;
                    //case 1:	s_condicion += " and numeroDiario>=" +(txtProtocoloDesde.Value); break;
                    //case 2:	s_condicion += " and numeroSector>=" +(txtProtocoloDesde.Value); break;
                    //case 3:	s_condicion += " and numeroTipoServicio>=" +(txtProtocoloDesde.Value); break;
                }
            }


            if (txtProtocoloHasta.Value != "")
            { 
                switch (oCon.TipoNumeracionProtocolo )
                {
                    case 0 : s_condicion += " and P.numero<="  +(txtProtocoloHasta.Value);break;
              /*      case 1 : s_condicion += " and numeroDiario<="  +(txtProtocoloHasta.Value);break;
                    case 2 : s_condicion += " and numeroSector<="  +(txtProtocoloHasta.Value); break;
                    case 3 : s_condicion += " and numeroTipoServicio<=" + (txtProtocoloHasta.Value); break;
                    */
                }
            }

            
            if (ddlEfector.SelectedValue != "0") s_condicion += " AND P.idEfectorSolicitante=" + ddlEfector.SelectedValue;
            if (ddlOrigen.SelectedValue != "0") s_condicion += " AND P.idOrigen=" + ddlOrigen.SelectedValue;
            if (ddlPrioridad.SelectedValue != "0") s_condicion += " AND P.idPrioridad=" + ddlPrioridad.SelectedValue;

       //     if (rdbEstadoAnalisis.SelectedValue == "1") s_condicion += " AND conResultado=0 ";

            string s_sectores = getListaSectores(true);
            if (s_sectores!="")   s_condicion += " AND P.idSector in (" + getListaSectores(true) + ")";

            if (ddlServicio.SelectedValue == "3")
            {
                string listaM = getListaMuestra();
                if (listaM!="") s_condicion += " AND P.idMuestra in (" + listaM + ")";
            }
            if (ddlEfectorDestino.SelectedValue != "0") s_condicion += " AND De.idEfectorDerivacion = " + ddlEfectorDestino.SelectedValue;
            ////////////////////////

            if (ddlArea.SelectedValue != "0")
            {
                //s_condicion += " AND idDestinoEtiqueta=" + ddlArea.SelectedValue;
                
                s_condicion += " AND I.idArea=" + ddlArea.SelectedValue + "and DP.trajoMuestra='Si' ";

                //m_strSQL = @" SELECT distinct [idProtocolo] ,[idArea] ,[numeroP],[area] ,[Fecha],[Origen] ,[Sector],[NumeroOrigen],[NumeroDocumento],[apellido],[Sexo],[edad]
                //FROM vta_LAB_GeneraCodigoBarras
                //WHERE   " + s_condicion;

//                m_strSQL = @"SELECT distinct P.idProtocolo
//                    FROM         dbo.LAB_Protocolo AS P with (nolock) INNER JOIN
//                      dbo.LAB_DetalleProtocolo AS DP with (nolock) ON P.idProtocolo = DP.idProtocolo INNER JOIN
//    	          LAB_Derivacion De with (nolock) on  De.idDetalleProtocolo= DP.idDetalleProtocolo inner join
//                     dbo.LAB_Item AS I with (nolock) ON DP.idItem = I.idItem  
//--              inner join        dbo.LAB_Area AS A with (nolock) ON I.idArea = A.idArea INNER JOIN
//--                      dbo.LAB_Origen AS O with (nolock) ON P.idOrigen = O.idOrigen INNER JOIN
//--                      dbo.LAB_SectorServicio AS SS with (nolock) ON P.idSector = SS.idSectorServicio Left JOIN
//	--				  lab_muestra as M with (nolock) on M.idMuestra= I.idMuestra
//Where P.baja=0      " + s_condicion;
            }
            //            if (ddlArea.SelectedValue=="0")
            //            {
            //                //m_strSQL += @"   SELECT [idProtocolo],[idArea],[numeroP],[area],[Fecha],[Origen],[Sector],[NumeroOrigen],[NumeroDocumento]      ,[apellido]      ,[Sexo]      ,[edad]
            //                //FROM vta_LAB_GeneraCodigoBarrasGeneral                    
            //                //WHERE   " + s_condicion;

            //                m_strSQL = @" SELECT DISTINCT                       P.idProtocolo
            //FROM         dbo.LAB_Protocolo AS P with (nolock) 
            //        --INNER JOIN                      dbo.LAB_Origen AS O with (nolock) ON P.idOrigen = O.idOrigen INNER JOIN
            //          --            dbo.LAB_SectorServicio AS SS with (nolock) ON P.idSector = SS.idSectorServicio 
            //        INNER JOIN     dbo.LAB_DetalleProtocolo AS DP with (nolock) ON P.idProtocolo = DP.idProtocolo INNER JOIN
            //    	       LAB_Derivacion De with (nolock) on  De.idDetalleProtocolo= DP.idDetalleProtocolo
            //        inner join   dbo.LAB_Item AS I with (nolock) ON DP.idItem = I.idItem  
            //--      inner join        dbo.LAB_Area AS A with (nolock) ON I.idArea = A.idArea 
            // where P.baja=0   " + s_condicion;

            //            }


            if (txtLoteDesde.Value != "")
            {
                s_condicion += " and L.idLoteDerivacion>=" + (txtLoteDesde.Value);
            }

            if(txtLoteHasta.Value != "")
            {
                s_condicion += " and L.idLoteDerivacion<=" + (txtLoteHasta.Value);
            }

                m_strSQL = @" SELECT DISTINCT      P.idProtocolo
        FROM         dbo.LAB_Protocolo AS P with (nolock)        
        INNER JOIN     dbo.LAB_DetalleProtocolo AS DP with (nolock) ON P.idProtocolo = DP.idProtocolo 
        INNER JOIN    LAB_Derivacion De with (nolock) on  De.idDetalleProtocolo= DP.idDetalleProtocolo
        inner join   dbo.LAB_Item AS I with (nolock) ON DP.idItem = I.idItem  
        inner join        dbo.LAB_Area AS A with (nolock) ON I.idArea = A.idArea 
        inner join      LAB_LoteDerivacion L with (nolock) ON De.idLote = L.idLoteDerivacion
        where P.baja=0   " + s_condicion;

            //      m_strSQL += " ORDER BY area";


            DataSet Ds = new DataSet();
            //      SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds, "resultado");

            
            DataTable data = Ds.Tables[0];
            conn.Close();
            return data;
        
        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtProtocoloDesde.Value != "")
            {
                if (oUtil.EsEntero(txtProtocoloDesde.Value))
                {
                    if (txtProtocoloHasta.Value != "")
                    {
                        if (oUtil.EsEntero(txtProtocoloHasta.Value)) args.IsValid = true; else args.IsValid = false;
                    }
                }
                else args.IsValid = false;
            }
            else
            {
                if (txtProtocoloHasta.Value != "")
                {
                    if (oUtil.EsEntero(txtProtocoloHasta.Value)) args.IsValid = true; else args.IsValid = false;
                }
                else args.IsValid = true;
            }

            
        }

        protected void lnkDesmarcarSectores_Click(object sender, EventArgs e)
        {
            MarcarSectoresSeleccionados(false);
        }

        protected void lnkMarcarSectores_Click(object sender, EventArgs e)
        {
            MarcarSectoresSeleccionados(true);
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    }

