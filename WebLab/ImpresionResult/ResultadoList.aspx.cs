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
using System.Data.SqlClient;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Business.Data.Laboratorio;
using CrystalDecisions.Web;
using Business.Data;

namespace WebLab.ImpresionResult
{
    public partial class ProtocoloList : System.Web.UI.Page
    {

        Configuracion oCon = new Configuracion();
        public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            //MiltiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);


        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Impresión");
                chkAnalisisResultados.Visible = false;
                chkAnalisisValidados.Visible = false;
                CargarGrilla();
                MarcarSeleccionados(false);
               // CargarListas();
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
        //private void CargarListas()
        //{
        //    Utility oUtil = new Utility();

        //    ///////////////Impresoras////////////////////////
        //    string m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
        //    oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
        //    if (Session["Impresora"] != null) ddlImpresora.SelectedValue = Session["Impresora"].ToString();
        //    ///////////////Fin de Impresoras///////////////////

        //    if (ddlImpresora.Items.Count == 0)
        //    {


        //        ddlImpresora.Visible = false;
        //        lnkImprimir.Visible = false;


        //    }
        //}
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        lnkMarcarImpresos.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MarcarSeleccionados(bool p)
        {

            if (gvLista.Visible)
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == !p)
                        ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
                }
            }
            else
            { 
                foreach (GridViewRow row in gvListaProducto.Rows)
                {
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == !p)
                        ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
                }
        }
         

    }
                
        private void CargarGrilla()
        {

            if (Request["idTipoServicio"].ToString() != "5")
            {
                
                gvLista.DataSource = LeerDatos();
                gvLista.DataBind();
                gvLista.Visible = true;
                gvListaProducto.Visible = false;
                PonerImagenes();
            }
            else
            {
                gvListaProducto.DataSource = LeerDatos();
                gvListaProducto.DataBind();
                gvLista.Visible = false;
                gvListaProducto.Visible = true;
                PonerImagenesNoProducto();
            }
        }

        private object LeerDatos()
        {
            string str_condicion = " P.baja=0 and P.idEfector="+ oUser.IdEfector.IdEfector.ToString();
            string str_orden = " ORDER BY ";
            DateTime fecha1 = DateTime.Parse(Request["FechaDesde"].ToString());
            DateTime fecha2 = DateTime.Parse(Request["FechaHasta"].ToString());
            str_condicion += " AND P.fecha>= '" + fecha1.ToString("yyyyMMdd") + "'";
            str_condicion += " AND P.fecha<= '" + fecha2.ToString("yyyyMMdd") + "'";

            TipoServicio oServicio= new TipoServicio();
            oServicio= (TipoServicio) oServicio.Get(typeof(TipoServicio),int.Parse(Request["idTipoServicio"].ToString()));
            lblServicio.Text = oServicio.Nombre.ToUpper();


            //if (Request["idTipoServicio"].ToString() != "0")
              str_condicion += " AND P.idTipoServicio = " + Request["idTipoServicio"].ToString();

            //Configuracion oCon = new Configuracion();oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            switch (oCon.TipoNumeracionProtocolo)
            {
                case 0:// busqueda con autonumerico
                    {
                        if (Request["ProtocoloDesde"].ToString() != "") str_condicion += " AND P.numero >= " + int.Parse(Request["ProtocoloDesde"].ToString());
                        if (Request["ProtocoloHasta"].ToString() != "") str_condicion += " AND P.numero <= " + int.Parse(Request["ProtocoloHasta"].ToString());
                        if (Request["ProtocoloRango"].ToString() != "") str_condicion += " AND P.numero in (" + Request["ProtocoloRango"].ToString() +")";
                        str_orden += " numero ";
                    } break;
                case 1: //busqueda con numero diario
                    {
                        if (Request["ProtocoloDesde"].ToString() != "") str_condicion += " AND P.numeroDiario >= " + int.Parse(Request["ProtocoloDesde"].ToString());
                        if (Request["ProtocoloHasta"].ToString() != "") str_condicion += " AND P.numeroDiario <= " + int.Parse(Request["ProtocoloHasta"].ToString());
                        if (Request["ProtocoloRango"].ToString() != "") str_condicion += " AND P.numeroDiario in (" + Request["ProtocoloRango"].ToString() + ")";
                        str_orden += " fecha, numerodiario  ";
                    } break;
                case 2: //busqueda con numero por sector
                    {
                        if (Request["ProtocoloDesde"].ToString() != "") str_condicion += " AND P.numeroSector >= " + int.Parse(Request["ProtocoloDesde"].ToString());
                        if (Request["ProtocoloHasta"].ToString() != "") str_condicion += " AND P.numeroSector <= " + int.Parse(Request["ProtocoloHasta"].ToString());
                        if (Request["ProtocoloRango"].ToString() != "") str_condicion += " AND P.numeroSector in (" + Request["ProtocoloRango"].ToString() + ")";
                        str_orden += "  numerosector  ";
                    } break;
            }

           

            if (Request["idOrigen"].ToString() != "0") str_condicion += " AND P.idOrigen = " + Request["idOrigen"].ToString();
            if (Request["idSectorServicio"].ToString() != "0") str_condicion += " AND P.idSector in (" + Request["idSectorServicio"].ToString() +")";
            if (Request["idArea"].ToString() != "0") str_condicion += " AND I.idArea in ( " + Request["idArea"].ToString()+")";
            if (Request["idEfectorSolicitante"].ToString() != "0") str_condicion += " AND P.idEfectorSolicitante = " + Request["idEfectorSolicitante"].ToString();
            if (Request["idPrioridad"].ToString() != "0") str_condicion += " AND P.idPrioridad = " + Request["idPrioridad"].ToString();
            if (Request["Impreso"].ToString() != "2") str_condicion += " AND P.impreso = " + Request["Impreso"].ToString();
            if (Request["Estado"].ToString() != "3") str_condicion += " AND P.estado = " + Request["Estado"].ToString();

            string m_strSQL = "";
            if (Request["idTipoServicio"].ToString() != "5")
             m_strSQL = @" SELECT   P.idProtocolo, 
                          P.numero  as numero,  CONVERT(varchar(10),P.fecha,103) as fecha, 
                                CASE 
                                    WHEN Pa.idestado = 2  THEN CAST(Pa.numeroAdic AS varchar(20))
                                ELSE     CAST(Pa.numeroDocumento AS varchar(20))
                                END  as dni ,Pa.apellido+ ' ' + Pa.nombre as paciente,
                            O.nombre as origen, Pri.nombre as prioridad, SS.nombre as sector,P.estado, P.impreso 
                           FROM Lab_Protocolo P (nolock)
                           INNER JOIN Lab_Origen O (nolock) on O.idOrigen= P.idOrigen 
                             INNER JOIN Lab_Prioridad Pri (nolock) on Pri.idPrioridad= P.idPrioridad 
                             INNER JOIN Sys_Paciente Pa (nolock) on Pa.idPaciente= P.idPaciente                               
                             INNER JOIN LAB_SectorServicio SS (nolock) ON P.idSector= SS.idSectorServicio                               
                                WHERE " + str_condicion  +
                                @"	 AND EXISTS (
                                    SELECT 1
                                    FROM LAB_DetalleProtocolo DP WITH (NOLOCK)  
                                    WHERE DP.idProtocolo = P.idProtocolo
                              )"+                                str_orden;
            else
                m_strSQL = @" SELECT DISTINCT  P.idProtocolo, P.numero  as numero,
                              CONVERT(varchar(10),P.fecha,103) as fecha,  M.nombre as muestra,  C.descripcion as conservacion,
                              O.nombre as origen,                                Pri.nombre as prioridad, SS.nombre as sector,P.estado, P.impreso 
                              FROM Lab_Protocolo P (nolock)
                              INNER JOIN Lab_Origen O (nolock) on O.idOrigen= P.idOrigen
                              INNER JOIN Lab_Prioridad Pri (nolock) on Pri.idPrioridad= P.idPrioridad
                            INNER JOIN Lab_muestra M (nolock) on M.idmuestra = P.idmuestra
                               left join lab_conservacion C on C.idconservacion = P.idconservacion                                
                                 INNER JOIN LAB_SectorServicio SS  ON P.idSector= SS.idSectorServicio                                
                               WHERE " + str_condicion  +str_orden;

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
             
            return Ds.Tables[0];
        }

     

        private void Imprimir(string tipo)
        {
            string m_lista = GenerarListaProtocolos();
           

        

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
          

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            ParameterDiscreteValue ImprimirHojasSeparadas = new ParameterDiscreteValue();
   

            ParameterDiscreteValue conLogo = new ParameterDiscreteValue();
            if (oCon.RutaLogo!="")
                conLogo.Value = true;
            else
                conLogo.Value = false;


            string parametroPaciente="";
          


            string parametroProtocolo = "";
          

        
       

            ParameterDiscreteValue tipoNumeracion = new ParameterDiscreteValue();
            tipoNumeracion.Value = oCon.TipoNumeracionProtocolo;

            ///////Redefinir el tipo de firma electronica (Serían dos reportes distintos)
            ParameterDiscreteValue conPie = new ParameterDiscreteValue();

          
            //ParameterDiscreteValue soloAnalisisResultado = new ParameterDiscreteValue();
            //soloAnalisisResultado.Value = chkAnalisisResultados.Checked;


            if ((Request["idTipoServicio"].ToString() == "1")|| (Request["idTipoServicio"].ToString() == "4"))//laboratorio o pesquisa
            {
                encabezado1.Value = oCon.EncabezadoLinea1;
                encabezado2.Value = oCon.EncabezadoLinea2;
                encabezado3.Value = oCon.EncabezadoLinea3;

                if (oCon.ResultadoEdad) parametroPaciente = "1"; else parametroPaciente = "0";
                if (oCon.ResultadoFNacimiento) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoSexo) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDNI) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoHC) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDomicilio) parametroPaciente += "1"; else parametroPaciente += "0";


                if (oCon.ResultadoNumeroRegistro) parametroProtocolo = "1"; else parametroProtocolo = "0";
                if (oCon.ResultadoFechaEntrega) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSector) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSolicitante) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoOrigen) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoPrioridad) parametroProtocolo += "1"; else parametroProtocolo += "0";

                conPie.Value = oCon.FirmaElectronicaLaboratorio.ToString();

                ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultado;


                if (oCon.OrdenImpresionLaboratorio)
                {
                    if (oCon.TipoHojaImpresionResultado == "A4")
                        oCr.Report.FileName = "../Informes/ResultadoSinOrden.rpt";
                    else
                        oCr.Report.FileName = "../Informes/ResultadoSinOrdenA5.rpt";
                }
                else
                {
                    if (oCon.TipoHojaImpresionResultado == "A4")
                        oCr.Report.FileName = "../Informes/Resultado.rpt";
                    else
                        oCr.Report.FileName = "../Informes/ResultadoA5.rpt";

                }
            }

         
            if ((Request["idTipoServicio"].ToString() == "3")|| (Request["idTipoServicio"].ToString() == "5"))//microbiologia o no pacientes
            {
                encabezado1.Value = oCon.EncabezadoLinea1Microbiologia;
                encabezado2.Value = oCon.EncabezadoLinea2Microbiologia;
                encabezado3.Value = oCon.EncabezadoLinea3Microbiologia;

                if (oCon.ResultadoEdadMicrobiologia) parametroPaciente = "1"; else parametroPaciente = "0";
                if (oCon.ResultadoFNacimientoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoSexoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDNIMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoHCMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDomicilioMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";



                if (oCon.ResultadoNumeroRegistroMicrobiologia) parametroProtocolo = "1"; else parametroProtocolo = "0";
                if (oCon.ResultadoFechaEntregaMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSectorMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSolicitanteMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoOrigenMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoPrioridadMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";

                conPie.Value = oCon.FirmaElectronicaMicrobiologia.ToString();


                ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultadoMicrobiologia;
                if (oCon.TipoHojaImpresionResultadoMicrobiologia == "A4")
                {    //oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                    if (Request["idTipoServicio"].ToString() == "3")
                        oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                    else
                        oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaNoPacientes.rpt";
                }
                else
                //oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaA5.rpt";
                {
                    if (Request["idTipoServicio"].ToString() == "3")
                        oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaA5.rpt";
                    else
                        oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaNoPacientesA5.rpt";
                }
            }

            ParameterDiscreteValue datosPaciente = new ParameterDiscreteValue();
            datosPaciente.Value = parametroPaciente;

            ParameterDiscreteValue datosProtocolo = new ParameterDiscreteValue();
            datosProtocolo.Value = parametroProtocolo;


            oCr.ReportDocument.SetDataSource(GenerarSetDatos(m_lista, chkAnalisisResultados.Checked, chkAnalisisValidados.Checked));
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(conLogo);
            oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(datosPaciente);
            oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(ImprimirHojasSeparadas);
            oCr.ReportDocument.ParameterFields[6].CurrentValues.Add(tipoNumeracion);
            oCr.ReportDocument.ParameterFields[7].CurrentValues.Add(conPie);
            oCr.ReportDocument.ParameterFields[8].CurrentValues.Add(datosProtocolo);
   

            oCr.DataBind();
            
            //if (tipo != "PDF")
            //{
            //    try
            //    {
            //        Session["Impresora"] = ddlImpresora.SelectedValue;
            //        oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
            //        oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);
            //    }
            //    catch (Exception ex)
            //    {
            //        string exception = "";
            //        //while (ex != null)
            //        //{
            //            exception = ex.Message + "<br>";

            //        //}
            //        string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Impresora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
            //        Page.RegisterStartupScript("PopupScript", popupScript);
            //    }
            //}
            //else
            //{
                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("Resultados");

                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
               
            //}

        }

        private string GenerarListaProtocolos()
        {
            string m_lista = "";
            if (gvLista.Visible)
            {
                foreach (GridViewRow row in gvLista.Rows)
                {

                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {
                        if (m_lista == "")
                            m_lista += gvLista.DataKeys[row.RowIndex].Value.ToString();
                        else
                            m_lista += "," + gvLista.DataKeys[row.RowIndex].Value.ToString();
                    }
                }
            }
            else
            {
                foreach (GridViewRow row in gvListaProducto.Rows)
                {

                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {
                        if (m_lista == "")
                            m_lista += gvListaProducto.DataKeys[row.RowIndex].Value.ToString();
                        else
                            m_lista += "," + gvListaProducto.DataKeys[row.RowIndex].Value.ToString();
                    }
                }
            }
            return m_lista;
        }

        private DataTable GenerarSetDatos(string m_lista, bool soloConResultados, bool soloValidados)
        {
            //Configuracion oCon = new Configuracion();
            //oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            int i_tipoServicio = int.Parse(Request["idTipoServicio"].ToString());

            string m_condicion = " WHERE idProtocolo in (" + m_lista + ")";
            string m_strOrden = "";
            //if (soloConResultados) m_condicion += " and conresultado=1 ";
            //if (soloValidados) m_condicion += " and idusuariovalida>0 ";

            if (Request["idArea"].ToString() != "0") m_condicion += " and idArea in (" + Request["idArea"].ToString() +")";

   


            m_condicion = m_condicion + m_strOrden;

            DataTable data = new DataTable();
            Protocolo oP = new Protocolo();

            data = oP.GetDataSet("Resultados", m_condicion,i_tipoServicio,oCon);



            return data;
        }

        

        protected void btnMarcarSel_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        protected void btnDesmarcarSel_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
         //   Imprimir();
        }

        protected void btnMarcarImpresos_Click(object sender, EventArgs e)
        {
            MarcarImpresos();
        }

        private void MarcarImpresos()
        {
            if (gvLista.Visible)
            {
                foreach (GridViewRow row in gvLista.Rows)
                {

                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {
                        Protocolo oProtocolo = new Protocolo();
                        oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
                        oProtocolo.Impreso = true;
                        oProtocolo.Save();
                        oProtocolo.GrabarAuditoriaProtocolo("Imprime", oUser.IdUsuario);

                    }
                }
            }
            else
            {
                foreach (GridViewRow row in gvListaProducto.Rows)
                {

                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {
                        Protocolo oProtocolo = new Protocolo();
                        oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvListaProducto.DataKeys[row.RowIndex].Value.ToString()));
                        oProtocolo.Impreso = true;
                        oProtocolo.Save();
                        oProtocolo.GrabarAuditoriaProtocolo("Imprime", oUser.IdUsuario);

                    }
                }
            }
            CargarGrilla();            
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {                                   
        }

        private void PonerImagenes()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {                
                    switch (row.Cells[9].Text)
                    {
                        case "0": ///Abierto
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                                row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                        case "1": //en proceso
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/amarillo.gif";
                               row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                        case "2": //terminado
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                                row.Cells[9].Controls.Add(hlnk);
                            }
                            break;

                    case "3": //bloqueado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/lock.png";
                            row.Cells[9].Controls.Add(hlnk);
                        }
                        break;
                }

                    switch (row.Cells[1].Text)
                    {
                        case "True":
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/impreso.jpg";
                                hlnk.ToolTip = "Protocolo Impreso";
                               row.Cells[1].Controls.Add(hlnk);
                            }
                            break;
                        case "False":
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/transparente.jpg";
                               row.Cells[1].Controls.Add(hlnk);
                            }
                            break;

                    }                

            }
        }

        private void PonerImagenesNoProducto()
        {
            foreach (GridViewRow row in gvListaProducto.Rows)
            {
                switch (row.Cells[9].Text)
                {
                    case "0": ///Abierto
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                            row.Cells[9].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //en proceso
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/amarillo.gif";
                            row.Cells[9].Controls.Add(hlnk);
                        }
                        break;
                    case "2": //terminado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                            row.Cells[9].Controls.Add(hlnk);
                        }
                        break;
                }

                switch (row.Cells[1].Text)
                {
                    case "True":
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/impreso.jpg";
                            hlnk.ToolTip = "Protocolo Impreso";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;
                    case "False":
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/transparente.jpg";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;

                }

            }
        }


        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            if  (GenerarListaProtocolos()!="")
                Imprimir("PDF");
            PonerImagenes();
        }

     /*   protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            if (GenerarListaProtocolos() != "")
            {
                //Imprimir("I");
                ImprimirdeaUno();
                //MarcarImpresos();
            }
            CargarGrilla();
         //   PonerImagenes();

        }*/

        private void ImprimirdeaUno()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    try
                    {
                        Protocolo oProtocolo = new Protocolo();
                        oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
                        ImprimirProtocolo(oProtocolo, "I");
                        oProtocolo.Impreso = true;
                        oProtocolo.Save();
                        //oProtocolo.GrabarAuditoriaProtocolo("Imprime", int.Parse(Session["idUsuario"].ToString()));
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
                        break;
                    }
                }
            }       
        }


        private void ImprimirProtocolo(Protocolo oProtocolo, string tipo)
        {

            string parametroPaciente = "";
            string parametroProtocolo = "";


            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();

            ParameterDiscreteValue ImprimirHojasSeparadas = new ParameterDiscreteValue();


            ParameterDiscreteValue tipoNumeracion = new ParameterDiscreteValue();
            tipoNumeracion.Value = oCon.TipoNumeracionProtocolo;


            ///////Redefinir el tipo de firma electronica (Serían dos reportes distintos)
            ParameterDiscreteValue conPie = new ParameterDiscreteValue();




            ParameterDiscreteValue conLogo = new ParameterDiscreteValue();
            if (oCon.RutaLogo != "")
                conLogo.Value = true;
            else
                conLogo.Value = false;

            if (oProtocolo.IdTipoServicio.IdTipoServicio == 1) //laboratorio
            {
                encabezado1.Value = oCon.EncabezadoLinea1;
                encabezado2.Value = oCon.EncabezadoLinea2;
                encabezado3.Value = oCon.EncabezadoLinea3;


                if (oCon.ResultadoEdad) parametroPaciente = "1"; else parametroPaciente = "0";
                if (oCon.ResultadoFNacimiento) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoSexo) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDNI) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoHC) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDomicilio) parametroPaciente += "1"; else parametroPaciente += "0";


                if (oCon.ResultadoNumeroRegistro) parametroProtocolo = "1"; else parametroProtocolo = "0";
                if (oCon.ResultadoFechaEntrega) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSector) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSolicitante) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoOrigen) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoPrioridad) parametroProtocolo += "1"; else parametroProtocolo += "0";



                ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultado;

                conPie.Value = oCon.FirmaElectronicaLaboratorio.ToString();

                if (oCon.OrdenImpresionLaboratorio)
                {
                    if (oCon.TipoHojaImpresionResultado == "A4") oCr.Report.FileName = "../Informes/ResultadoSinOrden.rpt";
                    else oCr.Report.FileName = "../Informes/ResultadoSinOrdenA5.rpt";
                }
                else
                {
                    if (oCon.TipoHojaImpresionResultado == "A4") oCr.Report.FileName = "../Informes/Resultado.rpt";
                    else oCr.Report.FileName = "../Informes/ResultadoA5.rpt";
                }
            }
            if (oProtocolo.IdTipoServicio.IdTipoServicio == 3) //microbilogia
            {
                encabezado1.Value = oCon.EncabezadoLinea1Microbiologia;
                encabezado2.Value = oCon.EncabezadoLinea2Microbiologia;
                encabezado3.Value = oCon.EncabezadoLinea3Microbiologia;


                if (oCon.ResultadoEdadMicrobiologia) parametroPaciente = "1"; else parametroPaciente = "0";
                if (oCon.ResultadoFNacimientoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoSexoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDNIMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoHCMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDomicilioMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";


                if (oCon.ResultadoNumeroRegistroMicrobiologia) parametroProtocolo = "1"; else parametroProtocolo = "0";
                if (oCon.ResultadoFechaEntregaMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSectorMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSolicitanteMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoOrigenMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoPrioridadMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";

                ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultadoMicrobiologia;

                conPie.Value = oCon.FirmaElectronicaMicrobiologia.ToString();

                if (oCon.TipoHojaImpresionResultadoMicrobiologia == "A4")
                    oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                else
                    oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaA5.rpt";
            }



            ParameterDiscreteValue datosPaciente = new ParameterDiscreteValue();
            datosPaciente.Value = parametroPaciente;

            ParameterDiscreteValue datosProtocolo = new ParameterDiscreteValue();
            datosProtocolo.Value = parametroProtocolo;



            string m_filtro = " WHERE idProtocolo =" + oProtocolo.IdProtocolo;



            if (oProtocolo.IdTipoServicio.IdTipoServicio == 1)
            {

                if (Request["idArea"].ToString() != "0") m_filtro += " and idArea=" + Request["idArea"].ToString();
                //if (Request["Operacion"].ToString() != "Valida")
                //{
                //    if (Request["validado"].ToString() == "1") m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";
                //}

                //if (Request["Operacion"].ToString() == "HC")
                //{
                    m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";
                //}

                //if (Request["Operacion"].ToString() == "Valida")
                //{
                //    //    m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )"; // los validados hasta ahora
                //    if ((rdbImprimir.SelectedValue == "0") && (Session["tildados"].ToString() != ""))// solo los marcados                
                //        m_filtro += " and idSubItem in (" + Session["tildados"] + ")";
                //    if (Session["tildados"].ToString() == "") m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";

                //}

            }

            oCr.ReportDocument.SetDataSource(oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio,oCon));
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(conLogo);
            oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(datosPaciente);
            oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(ImprimirHojasSeparadas);
            oCr.ReportDocument.ParameterFields[6].CurrentValues.Add(tipoNumeracion);
            oCr.ReportDocument.ParameterFields[7].CurrentValues.Add(conPie);
            oCr.ReportDocument.ParameterFields[8].CurrentValues.Add(datosProtocolo);






            oCr.DataBind();

            string s_nombreProtocolo = "";
            switch (oCon.TipoNumeracionProtocolo)
            {
                case 0: s_nombreProtocolo = oProtocolo.Numero.ToString(); break;
                case 1: s_nombreProtocolo = oProtocolo.NumeroDiario.ToString(); break;
                case 2: s_nombreProtocolo = oProtocolo.PrefijoSector + oProtocolo.NumeroSector.ToString(); break;
                case 3: s_nombreProtocolo = oProtocolo.NumeroTipoServicio.ToString(); break;
            }

            //if (tipo != "PDF")
            //{
           //     oProtocolo.GrabarAuditoriaProtocolo("Imprime Resultados", int.Parse(Session["idUsuario"].ToString()));
            //    Session["Impresora"] = ddlImpresora.SelectedValue;

             //   oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
               // oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);

                //oProtocolo.Impreso = true;
                //oProtocolo.Save();
            //}
            //else
            //{
            //    oProtocolo.GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(Session["idUsuario"].ToString()));
            //    MemoryStream oStream; // using System.IO
            //    oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.ContentType = "application/pdf";
            //    Response.AddHeader("Content-Disposition", "attachment;filename=" + s_nombreProtocolo + ".pdf");

            //    Response.BinaryWrite(oStream.ToArray());
            //    Response.End();
            //}

        }
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            if (gvLista.Visible) PonerImagenes();
            else PonerImagenesNoProducto();
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            if (gvLista.Visible) PonerImagenes();
            else PonerImagenesNoProducto();
        }

        protected void lnkMarcarImpresos_Click(object sender, EventArgs e)
        {
            if (GenerarListaProtocolos() != "")
                MarcarImpresos();
            if (gvLista.Visible) PonerImagenes();
            else PonerImagenesNoProducto();
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;
            CargarGrilla();
           
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImprimirBusqueda.aspx?idServicio=" + Request["idTipoServicio"].ToString() + "&modo=" + Request["modo"].ToString(), false);
        }

        protected void gvListaProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvListaProducto.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void gvListaProducto_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}
