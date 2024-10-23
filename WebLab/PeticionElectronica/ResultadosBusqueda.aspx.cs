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
using NHibernate;
using NHibernate.Expression;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using Business.Data;

namespace WebLab.PeticionElectronica
{
    public partial class ResultadosBusqueda : System.Web.UI.Page
    {
        CrystalReportSource oCr = new CrystalReportSource();
        Configuracion oCon = new Configuracion();



        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;



        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)


            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                txtDni.Focus();
               

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


    

     

        private void CargarGrilla2()
        {


        }

        private void CargarGrilla()
        {
            Utility oUtil = new Utility();
            string str_condicion = ""; string str_condicionMadre = "";
          
       
            /////////////////////////////////////////////////////////////////////////////////////////

            if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value + "'";
            if (txtApellido.Text != "") str_condicion += " AND Pa.apellido like '%" + oUtil.SacaComillas(txtApellido.Text) + "%'";
            if (txtNombre.Text != "") str_condicion += " AND Pa.nombre like '%" + oUtil.SacaComillas(txtNombre.Text) + "%'";
       
 
            ///////////////////////////////Condicion para buscar por la madre/tutor///////////////             
            if ((txtDniMadre.Value != "") || (txtApellidoMadre.Text != "") || (txtNombreMadre.Text != ""))
            {
                str_condicionMadre = " and P.idPaciente in (Select idPaciente FROM  Sys_Parentesco WHERE  1=1 ";
                if (txtDniMadre.Value != "") str_condicionMadre += " AND numeroDocumento = '" + txtDniMadre.Value + "'";
                if (txtApellidoMadre.Text != "") str_condicionMadre += " AND apellido like '%" + oUtil.SacaComillas(txtApellidoMadre.Text) + "%'";
                if (txtNombreMadre.Text != "") str_condicionMadre += " AND nombre like '%" + oUtil.SacaComillas(txtNombreMadre.Text) + "%'";
                str_condicionMadre += " ) ";
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            string m_strSQL = " SELECT distinct P.idPaciente,case when Pa.idEstado=2 then '(s/dni)' else convert(varchar,Pa.numeroDocumento) end as numeroDocumento, Pa.apellido + ', ' + Pa.nombre as paciente, " +
                              " convert(varchar(10), Pa.fechaNacimiento,103) as fechaNacimiento " +
                              " FROM LAB_Protocolo P " +
                              " INNER JOIN Sys_Paciente Pa ON Pa.idPaciente= P.idPaciente " +
                              " INNER JOIN LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo " +
                              " left JOIN LAB_Peticion as Pe on Pe.idProtocolo= P.idProtocolo "+ /// restriccion de peticion
                              " WHERE P.baja=0 and P.idTipoServicio=3  " + str_condicion + str_condicionMadre +
                              " ORDER BY numeroDocumento, paciente";// and P.estado<3


            DataSet Ds1 = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1);
        

       

            gvLista.DataSource = Ds1.Tables[0];
            gvLista.DataBind();
            //gvListaProducto.Visible = false;
            //gvLista.Visible =true;
        }



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                CargarGrilla();

        }


        protected void lnkAmpliarFiltros_Click(object sender, EventArgs e)
        {
            if (lnkAmpliarFiltros.Text == "Ampliar filtros de búsqueda")
            {
                lnkAmpliarFiltros.Text = "Ocultar filtros adicionales";
                pnlParentesco.Visible = true;
            }
            else
            {
                lnkAmpliarFiltros.Text = "Ampliar filtros de búsqueda";
                pnlParentesco.Visible = false;
            }

            lnkAmpliarFiltros.UpdateAfterCallBack = true;
            pnlParentesco.UpdateAfterCallBack = true;
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Visualizar":
                    {
                        string m_parametro = " P.idPaciente=" + e.CommandArgument.ToString();
                    
                                Response.Redirect("../Resultados/Procesa.aspx?idServicio=3&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal&Tipo=PacienteValidado&master=1", false); break;
                        //}




                        break;
                    }

            }


        }
        private void Imprimir(int idProtocolo, string tipo)
        {
            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), idProtocolo);

                CrystalReportSource oCr = new CrystalReportSource();
                oCr.Report.FileName = "";
                oCr.CacheDuration = 10000;
                oCr.EnableCaching = true;

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

                if (oProtocolo.IdTipoServicio.IdTipoServicio != 3) //laboratorio o pesquisa neonatal
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
                    // oCr.Report.FileName = "../Informes/ResultadoSinOrden.rpt";
                }
                if (oProtocolo.IdTipoServicio.IdTipoServicio == 3) //microbiologia
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




                DataTable d = oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio,oCon);
                oCr.ReportDocument.SetDataSource(d);
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


                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, s_nombreProtocolo);

                oProtocolo.GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(Session["idUsuario"].ToString()));

                conn.Close();
            }

        }
        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Visualizar";
                CmdModificar.ToolTip = "Ver Historia Clínica";
            }

        }

        protected void cvDatosEntrada_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtDni.Value == "")
                if (txtApellido.Text == "")
                    if (txtNombre.Text == "")
                        if (txtDniMadre.Value == "")
                            if (txtApellidoMadre.Text == "")
                                if (txtNombreMadre.Text == "")
                                   
                                        args.IsValid = false;


                                    else
                                        args.IsValid = true;
                                else
                                    args.IsValid = true;
        }

        protected void rdbTipoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

      

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (oCon.TipoNumeracionProtocolo == 2)  //letras y numeros
            {
                args.IsValid = true;
            }
            else   ///solo numeros
            {


               
                    if (txtDni.Value != "") { if (oUtil.EsEntero(txtDni.Value)) args.IsValid = true; else args.IsValid = false; }
                    else
                        args.IsValid = true;
              
            }
        }

        protected void cvDNI_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            if (txtDni.Value != "")
            { if (oUtil.EsEntero(txtDni.Value)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

   

        protected void cvDNIMadre_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            if (txtDniMadre.Value != "")
            { if (oUtil.EsEntero(txtDniMadre.Value)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

        protected void lnkHistorial_Click(object sender, EventArgs e)
        {
            Response.Redirect("HistorialPorUsuario.aspx", false);
        }

     

    
    }
}

