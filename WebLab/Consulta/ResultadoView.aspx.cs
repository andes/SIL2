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
using System.Drawing;
using NHibernate;
using NHibernate.Expression;
using Business.Data;
using MathParser;
using CrystalDecisions.Shared;
using System.IO;
using CrystalDecisions.Web;
using System.Net;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Business.Data.AutoAnalizador;

namespace WebLab.Consulta
{
    public partial class ResultadoView : System.Web.UI.Page
    {
      
        CrystalReportSource oCr = new CrystalReportSource();
        bool hayAntecedente = false;
        Configuracion oCon = new Configuracion(); 
        DataTable dtProtocolo;
        


        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;

           


           if (Session["idProtocolo"] != null)
             
                {
                if (Session["idUsuario"] != null)         
                {

                    //Protocolo oRegistro = new Protocolo();
                    //oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Session["idProtocolo"].ToString()));

                    //bool acceso = oRegistro.VerificaPermisodeAcceso(int.Parse(Session["idUsuario"].ToString()));
                    //if (acceso)
                    //{
                        LlenarTabla(Session["idProtocolo"].ToString());
                        //CargarListas();
                    //}
                    //else
                    //{     //acceso bloqueado
                    //    //panelResultados.Visible = false;
                    //    //lblAccesoRestringido.Visible = true;
                    //}

                }
                else
                    Response.Redirect("../FinSesion.aspx", false);                   
            }       
        }

        //protected void Page_UnLoad(object sender, EventArgs e)
        //{  
        //    oCr.Dispose();
        //}


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {            
                if (Session["idUsuario"] != null)                
                    Inicializar();
             
                                            
                else
                    Response.Redirect("../FinSesion.aspx", false);                             
            }
        }


  
        private void Inicializar()
        {           
            CargarGrilla();
           
            pnlAntecedentes.Visible = false;
                    
            pnlHC.Visible = true;
            //if (Session["validado"].ToString() == "1")
                lblTitulo.Text = "HISTORIAL DE RESULTADOS";
            //else
            //    lblTitulo.Text = "CONSULTA DE RESULTADOS"; 

            //pnlReferencia.Visible = true;

            MuestraDatos(CurrentPageIndex.ToString());
            hypRegresar.NavigateUrl = "HistoriaClinicaFiltro.aspx?Tipo=PacienteValidado";
            //if (Request["master"] != null) // es peticion electronic
            //{
            //    hypRegresar.Visible = false; imgImprimir.Visible = false;
            //    pnlImpresora.Visible = false; lblTitulo.Visible = false; }
            //else
            //{
            //    //if (Request["desde"] == "Urgencia")
            //    //    hypRegresar.NavigateUrl = "../Urgencia/UrgenciaList.aspx";
            //    //else
            //    //{
            //        if (Session["validado"].ToString() == "0") ///protocolo completo
            //        {
            //            hypRegresar.NavigateUrl = "../Informes/HistoriaClinicaFiltro.aspx?Tipo=PacienteCompleto";
            //            VerificaPermisos("Historial de Visitas");
            //        }
            //        else  ///solo validado para acceso externos
            //        {
            //            imgImprimir.Visible = false;
            //            pnlImpresora.Visible = false;
            //            hypRegresar.NavigateUrl = "../Informes/HistoriaClinicaFiltro.aspx?Tipo=PacienteValidado";
            //            VerificaPermisos("Historial de Resultados");
            //        }
            //    //}
            //}



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
                    
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }



       
        private void CargarGrilla()
        {
            ////Metodo que carga la grilla de Protocolos
            string connetionString = ConfigurationManager.ConnectionStrings["SIPS"].ConnectionString;
            string m_strSQL = " Select distinct  idProtocolo, " +
                              "  numero as numero," +
                              " convert(varchar(10), fecha,103) as fecha, 2 as estado ,  fecha as fecha1," +
                              " '' prefijosector, 0 as numerosector , 0 as numerodiario" +
                              " from  LAB_ResultadoEncabezado " + // +str_condicion;                          
                              " WHERE idEfector=205 and idProtocolo in (" + Session["Parametros"].ToString() + ")";
            
            if (Request["Operacion"].ToString() == "HC")
                m_strSQL += " order by idProtocolo desc "; // desde el mas reciente al mas antiguo.


            SqlConnection conn = new SqlConnection(connetionString);

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
           

            if (Ds.Tables[0].Rows.Count > 0)
            {
                dtProtocolo = Ds.Tables[0];
                int ultimafila = Ds.Tables[0].Rows.Count - 1;
                CurrentPageIndex = int.Parse(Session["idProtocolo"].ToString());
                CurrentIndexGrilla = int.Parse(Request["Index"].ToString());
                //CurrentPageIndex = int.Parse(Ds.Tables[0].Rows[CurrentIndexGrilla].ItemArray[0].ToString());
                UltimaPageIndex = ultimafila; // int.Parse(Ds.Tables[0].Rows[ultimafila].ItemArray[0].ToString());
            }

            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados";
            Session.Add("Tabla1", dtProtocolo);
           
            
        }


    

        private int CurrentPageIndex /* Guardamos el indice de página actual */
        {
            get { return ViewState["CurrentPageIndex"] == null ? 0 : int.Parse(ViewState["CurrentPageIndex"].ToString()); }
            set { ViewState["CurrentPageIndex"] = value; }
        }

        private int CurrentIndexGrilla /* Guardamos el indice de página actual */
        {
            get { return ViewState["CurrentIndexGrilla"] == null ? 0 : int.Parse(ViewState["CurrentIndexGrilla"].ToString()); }
            set { ViewState["CurrentIndexGrilla"] = value; }
        }

        private int UltimaPageIndex /* Guardamos el indice de página actual */
        {
            get { return ViewState["UltimaPageIndex"] == null ? 0 : int.Parse(ViewState["UltimaPageIndex"].ToString()); }
            set { ViewState["UltimaPageIndex"] = value; }
        }

        private void MuestraDatos(string p)
        {
            SqlConnection connection;
            SqlCommand command;
            string connetionString = ConfigurationManager.ConnectionStrings["SIPS"].ConnectionString;
            connection = new SqlConnection(connetionString);

            DataSet Ds = new DataSet();

            string sql = @"select fecha, numero, origen, solicitante,
 sala, cama, numerodocumento, apellido,nombre, sexo, fechaNacimiento, edad, unidadedad, 
 hiv, tipoMuestra,  ObservacionesResultados , embarazo
 from  LAB_ResultadoEncabezado 
where  idefector=  205 
and    idProtocolo=" + Session["idProtocolo"].ToString();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(sql, connection);
            adapter.Fill(Ds);
            

            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //DataSet Ds = new DataSet();
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "LAB_ResultadoSIPS";

            //cmd.Parameters.Add("@idEfector", SqlDbType.NVarChar);
            //cmd.Parameters["@idEfector"].Value = 205;

            //cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            //cmd.Parameters["@idProtocolo"].Value = int.Parse(Session["idProtocolo"].ToString());

            //cmd.Parameters.Add("@tipo", SqlDbType.NVarChar);
            //cmd.Parameters["@tipo"].Value = "encabezado";
            //cmd.Connection = conn;

            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            //da.Fill(Ds);
            ///Muestra los datos de encabezado para el protocolo seleccionado
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                //decimal m_minimoReferencia=-1;
                //decimal m_maximoReferencia=-1;
                string m_fecha = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                string numero = Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                string origen = Ds.Tables[0].Rows[i].ItemArray[2].ToString();
                string solicitante = Ds.Tables[0].Rows[i].ItemArray[3].ToString();

                string sala = Ds.Tables[0].Rows[i].ItemArray[4].ToString();

                string cama = Ds.Tables[0].Rows[i].ItemArray[5].ToString();
                string numerodocumento = Ds.Tables[0].Rows[i].ItemArray[6].ToString();
                string apellido = Ds.Tables[0].Rows[i].ItemArray[7].ToString();
                string nombre = Ds.Tables[0].Rows[i].ItemArray[8].ToString();
                string sexo = Ds.Tables[0].Rows[i].ItemArray[9].ToString();
                string fechaNacimiento = Ds.Tables[0].Rows[i].ItemArray[10].ToString();
                string edad = Ds.Tables[0].Rows[i].ItemArray[11].ToString();
                string unidadedad = Ds.Tables[0].Rows[i].ItemArray[12].ToString();
                string hiv = Ds.Tables[0].Rows[i].ItemArray[13].ToString();
                string tipoMuestra = Ds.Tables[0].Rows[i].ItemArray[14].ToString();
                string ObservacionesResultados = Ds.Tables[0].Rows[i].ItemArray[15].ToString();
                string embarazada = Ds.Tables[0].Rows[i].ItemArray[16].ToString();

                //HFIdProtocolo.Value = p;
                //Actualiza los datos de los objetos : alta o modificacion .                                        

                // oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));

                GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()), int.Parse(Session["idProtocolo"].ToString()), 205);

                if (tipoMuestra != "")
                {

                    lblMuestra.Text = "Tipo de Muestra: " + tipoMuestra;

                }




                //lblNroSISA.Visible = false;


                lblObservacionResultado.Text = ObservacionesResultados;



                lblServicio.Text = "LABORATORIO";







                //lblUsuario.Visible = false;
                lblFechaRegistro.Visible = false;
                //int len = oRegistro.FechaRegistro.ToString().Length - 11;
                //lblHoraRegistro.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);
                lblFecha.Text = m_fecha;
                lblProtocolo.Text = numero;

                //hplProtocolo.NavigateUrl = "../Protocolos/ProtocoloEdit2.aspx?idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString()+ "&Operacion=Modifica&idProtocolo=" +oRegistro.IdProtocolo.ToString();

                //if (oRegistro.IdEfector == oRegistro.IdEfectorSolicitante)
                lblOrigen.Text = origen;
                //else
                lblSolicitante.Text = solicitante;

                //  lblMedico.Text = oRegistro.Especialista + " MP:" + oRegistro.MatriculaEspecialista;

                //lblPrioridad.Text = oRegistro.IdPrioridad.Nombre;
                //    if (oRegistro.IdPrioridad.Nombre == "URGENTE")
                //    {
                //        lblPrioridad.ForeColor = Color.Red;
                //        lblPrioridad.Font.Bold = true;
                //    }

                //lblSector.Text = oRegistro.IdSector.Nombre;
                if (sala != "") lblOrigen.Text += " Sala: " + sala;
                if (cama != "") lblOrigen.Text += " Cama: " + cama;

                //   HFIdPaciente.Value = oRegistro.IdPaciente.IdPaciente.ToString();

                ///Datos del Paciente       
                lblDni.Text = numerodocumento;

                logoRenaper.Visible = false; spanRenaper.Visible = false;

                lblPaciente.Text = apellido.ToUpper() + " " + nombre.ToUpper();

                lblSexo.Text = sexo;
                lblFechaNacimiento.Text = fechaNacimiento;
                lblEdad.Text = edad + " " +unidadedad;

                //    lblNumeroOrigen.Text = oRegistro.NumeroOrigen;
                lblNumeroOrigen.Visible = false;
                ////////////////////////////////////////
                
                //ISession m_session = NHibernateHttpModule.CurrentSession;
                //ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
                //crit.Add(Expression.Eq("IdProtocolo", oRegistro));
                //IList lista = crit.List();
                //if (lista.Count > 0)
                //{
                //    foreach (ProtocoloDiagnostico oDiag in lista)
                //    {
                //        Cie10 oD = new Cie10();
                //        oD = (Cie10)oD.Get(typeof(Cie10), oDiag.IdDiagnostico);
                //        if (lblDiagnostico.Text == "") lblDiagnostico.Text = oD.Nombre;
                //        else lblDiagnostico.Text += " - " + oD.Nombre;
                //        if (oD.Codigo == "Z32.1") embarazada = "E";

                //    }
                //}

                if (hiv.ToUpper() == "TRUE")
                    lblPaciente.Text = lblSexo.Text.Substring(0, 1) + nombre.Substring(0, 2) + apellido.Substring(0, 2) + lblFechaNacimiento.Text.Replace("/", "") + embarazada;
                lblPaciente.Text = lblPaciente.Text.ToUpper();

                //     lblPedidoOriginal.Text = oRegistro.GetPracticasPedidas();



            }

        }


        private void LlenarTabla(string p)
        {
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;           
            //DataSet Ds = new DataSet();            
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "LAB_ResultadoSIPS";

            //cmd.Parameters.Add("@idEfector", SqlDbType.NVarChar);
            //cmd.Parameters["@idEfector"].Value = 205;

            //cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            //cmd.Parameters["@idProtocolo"].Value = p;

            //cmd.Parameters.Add("@tipo", SqlDbType.NVarChar);
            //cmd.Parameters["@tipo"].Value = "resultado";
            //cmd.Connection = conn;

            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            //da.Fill(Ds);


            SqlConnection connection;
           
            string connetionString = ConfigurationManager.ConnectionStrings["SIPS"].ConnectionString;
            connection = new SqlConnection(connetionString);

            DataSet Ds = new DataSet();

            string sql = @"select  grupo, item, 0 as iditem , resultado as resultadonum,resultado as resultadocar, observaciones, 0 as idcategoria,
0 as idtipoResultado, unidad as unidadMedida, 2 as Estado, metodo , valorreferencia,
'' as MaximoReferencia, 	'' as observacionReferencia , '' as usercarga, 'S' as trajomuestra,
'' as tipoValorReferencia,  conresultado, 
0	formatoDecimal, 0 formato0, 0 formato1, 0 formato2, 0 formato3,0  formato4 , 
'' resultadoDefecto, '' userControl,
    iddetalleProtocolo, 1 codificaHiv, profesional_val as userValida, 1 estadoObservacion, area, orden  
	 
from  dbo.LAB_ResultadoDetalle
where  idefector=  205
and    idProtocolo=" + Session["idProtocolo"].ToString();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(sql, connection);
            adapter.Fill(Ds);


            //int cantidadResultadosValidados = Ds.Tables[0].Rows.Count;
            //if (cantidadResultadosValidados > 0)

            //{
            string s = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;


                string m_titulo = "";
                string m_hijo = "";
                string m_nombre = "";



                TableRow objFila_TITULO = new TableRow();
                TableCell objCellAnalisis_TITULO = new TableCell();
                TableCell objCellResultado_TITULO = new TableCell();
                TableCell objCellResultadoAnterior_TITULO = new TableCell();
                TableCell objCellUnMedida_TITULO = new TableCell();
                TableCell objCellValoresReferencia_TITULO = new TableCell();
                TableCell objCellValida_TITULO = new TableCell();
                TableCell objCellPersona_TITULO = new TableCell();
                TableCell objCellObservaciones_TITULO = new TableCell();




                Label lblAnalisis = new Label();
                lblAnalisis.Text = "ANALISIS";
                objCellAnalisis_TITULO.Controls.Add(lblAnalisis);


                Label lblResultado = new Label();
                lblResultado.Text = "RESULTADO";
                objCellResultado_TITULO.Controls.Add(lblResultado);


                //Label lblResultadoAnterior = new Label();
                //lblResultadoAnterior.Text = "R.ANTER.";
            
                //objCellResultadoAnterior_TITULO.Controls.Add(lblResultadoAnterior);


                //if (Request["Operacion"].ToString() != "HC")
                //{
                //    Label lblUM = new Label();
                //    lblUM.Text = "U.M";
                //    objCellUnMedida_TITULO.Controls.Add(lblUM);
                //}

                Label lblVR = new Label();
                lblVR.Text = "VR-METODO";
                objCellValoresReferencia_TITULO.Controls.Add(lblVR);




                Label lblValida = new Label();
                if ((Request["Operacion"].ToString() == "Carga") || (Request["Operacion"].ToString() == "HC")) lblValida.Text = "";
                else
                {
                    if (Request["Operacion"].ToString() == "Valida") lblValida.Text = "VAL";
                    if (Request["Operacion"].ToString() == "Control") lblValida.Text = "CTRL";
                }
                objCellValida_TITULO.Controls.Add(lblValida);

                Label lblCargadoPor = new Label();
                //if ((Request["Operacion"].ToString() == "HC") && (Session["validado"].ToString() == "1"))
                //{
                    lblCargadoPor.Text = "VALIDADO POR";
                    //                Panel1.ScrollBars = ScrollBars.None;
                //}
                //else
                //    lblCargadoPor.Text = "ESTADO";

                objCellPersona_TITULO.Controls.Add(lblCargadoPor);


                /////observaciones
                //if (Request["Operacion"].ToString() == "Valida")
                //{
                

                objFila_TITULO.Cells.Add(objCellAnalisis_TITULO);
                objFila_TITULO.Cells.Add(objCellResultado_TITULO);
            

                

                objFila_TITULO.Cells.Add(objCellValoresReferencia_TITULO);

                
                objFila_TITULO.Cells.Add(objCellPersona_TITULO);
                //objFila_TITULO.Cells.Add(objCellResultadoAnterior_TITULO);

                objFila_TITULO.CssClass = "myLabelIzquierda";
                objFila_TITULO.BackColor = Color.Gainsboro;

                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                //'añadimos la fila a la tabla
                if (objFila_TITULO != null) tContenido.Controls.Add(objFila_TITULO);//.Rows.Add(objRow);    

                string pivot_Area = "";

                int tablas = Ds.Tables.Count;
                if (tablas > 0)
                { 
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    //decimal m_minimoReferencia=-1;
                    //decimal m_maximoReferencia=-1;
                    string valorReferencia = Ds.Tables[0].Rows[i].ItemArray[11].ToString();
                    int m_idItem = int.Parse(Ds.Tables[0].Rows[i].ItemArray[2].ToString());
                    string unMedida = Ds.Tables[0].Rows[i].ItemArray[8].ToString();
                    string Observaciones = Ds.Tables[0].Rows[i].ItemArray[5].ToString();
                    int tiporesultado = (int.Parse(Ds.Tables[0].Rows[i].ItemArray[7].ToString()));
                    int tipodeterminacion = int.Parse(Ds.Tables[0].Rows[i].ItemArray[6].ToString());
                    int estado = int.Parse(Ds.Tables[0].Rows[i].ItemArray[9].ToString());
                    string m_metodo = Ds.Tables[0].Rows[i].ItemArray[10].ToString();

                    string m_observacionReferencia = Ds.Tables[0].Rows[i].ItemArray[13].ToString();
                    string m_usuarioCarga = Ds.Tables[0].Rows[i].ItemArray[14].ToString();
                    string m_trajoMuestra = Ds.Tables[0].Rows[i].ItemArray[15].ToString();
                    string m_tipoValorReferencia = Ds.Tables[0].Rows[i].ItemArray[16].ToString();
                    string m_conResultado = Ds.Tables[0].Rows[i].ItemArray[17].ToString();
                    string m_formatoDecimal = Ds.Tables[0].Rows[i].ItemArray[18].ToString();
                    string m_formato0 = Ds.Tables[0].Rows[i].ItemArray[19].ToString();
                    string m_formato1 = Ds.Tables[0].Rows[i].ItemArray[20].ToString();
                    string m_formato2 = Ds.Tables[0].Rows[i].ItemArray[21].ToString();
                    string m_formato3 = Ds.Tables[0].Rows[i].ItemArray[22].ToString();
                    string m_formato4 = Ds.Tables[0].Rows[i].ItemArray[23].ToString();
                    string m_resultadoDefecto = Ds.Tables[0].Rows[i].ItemArray[24].ToString();
                    string m_usuariocontrol = Ds.Tables[0].Rows[i].ItemArray[25].ToString();
                    string m_usuariovalida = Ds.Tables[0].Rows[i].ItemArray[28].ToString();
                    int i_iddetalleProtocolo = int.Parse(Ds.Tables[0].Rows[i].ItemArray[26].ToString());
                    string m_codificaPaciente = Ds.Tables[0].Rows[i].ItemArray[27].ToString();

                    string m_estadoObservacion = Ds.Tables[0].Rows[i].ItemArray[29].ToString();
                    string m_area = Ds.Tables[0].Rows[i].ItemArray[30].ToString();

                    if (m_codificaPaciente == "True")
                    {
                        lblPaciente.Visible = false;
                        lblCodigoPaciente.Visible = true;
                    }


                    m_hijo = Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                    m_titulo = Ds.Tables[0].Rows[i].ItemArray[0].ToString();

                    TableRow objFila = new TableRow();
                    TableCell objCellAnalisis = new TableCell();
                    TableCell objCellResultado = new TableCell();
                    TableCell objCellResultadoAnterior = new TableCell();
                    TableCell objCellUnMedida = new TableCell();
                    TableCell objCellValoresReferencia = new TableCell();
                    TableCell objCellValida = new TableCell();
                    TableCell objCellPersona = new TableCell();
                    TableCell objCellObservaciones = new TableCell();


                    decimal x = 0;



                    if (m_area != pivot_Area) ///poner titulo del area
                    {
                        TableRow objRow = new TableRow();
                        TableCell objCell = new TableCell();
                        Label lbl0 = new Label();
                        //lbl0.ForeColor = Color
                        lbl0.Text = m_area.ToUpper();
                        lbl0.TabIndex = short.Parse("500");
                        lbl0.Font.Bold = true;


                        Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(lbl0);
                        objCell.Controls.Add(lbl0);
                        //if (Request["Operacion"].ToString() == "HC")
                        //    objCell.ColumnSpan = 8;
                        //else
                        objCell.ColumnSpan = 8;

                        objRow.BackColor = Color.Beige;
                        objRow.HorizontalAlign = HorizontalAlign.Center;
                        objRow.Cells.Add(objCell);
                        //         objRow.CssClass = "myLabelIzquierda";
                        tContenido.Controls.Add(objRow);

                        pivot_Area = m_area;
                    }

                    if ((m_hijo != m_titulo) && (m_nombre != m_titulo)) ///poner titulo de la practica
                    {
                        TableRow objRow = new TableRow();
                        TableCell objCell = new TableCell();
                        Label lbl0 = new Label();

                        lbl0.Text = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                        lbl0.TabIndex = short.Parse("500");
                        lbl0.Font.Bold = true;

                        Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(lbl0);
                        objCell.Controls.Add(lbl0);
                        //if (Request["Operacion"].ToString() == "HC")
                            objCell.ColumnSpan = 6;
                        //else
                        //    objCell.ColumnSpan = 8;

                        objRow.Cells.Add(objCell);
                        objRow.CssClass = "myLabelIzquierda";
                        tContenido.Controls.Add(objRow);

                        m_nombre = m_titulo;
                    }



                    Label lbl1 = new Label();
                    if (m_hijo == m_titulo) lbl1.Text = m_hijo;
                    else lbl1.Text = "&nbsp;&nbsp;&nbsp;" + m_hijo;



                    lbl1.TabIndex = short.Parse("500");
                    lbl1.ForeColor = Color.Black;
                    lbl1.Font.Size = FontUnit.Point(9);
                    if (tipodeterminacion != 0)
                    {
                        lbl1.Font.Bold = true;
                        lbl1.Font.Italic = true;
                        objCellAnalisis.ColumnSpan = 1;
                    }

                    objCellAnalisis.Controls.Add(lbl1);


                    //Item oItem = new Item();
                    //oItem = (Item)oItem.Get(typeof(Item), m_idItem);

                    //DetalleProtocolo oDetalle = new DetalleProtocolo();
                    //oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);


                  //  bool es_Bacteriologia = false;
                    string observacionesDetalle = "";
                    ///Antes de mostrar el control verifica  si está derivado                    
                    if (1==0) //oItem.IdEfectorDerivacion != oItem.IdEfector) //es derivado
                    {
                        //Label lblDerivacion = new Label();
                        //lblDerivacion.Font.Italic = true;
                        //lblDerivacion.TabIndex = short.Parse("500");
                        ////Verifica el estado de la derivacion
                        //string estadoDerivacion = "";

                        //if (i_iddetalleProtocolo != 0)
                        //{
                        //    DetalleProtocolo oDetalle = new DetalleProtocolo();
                        //    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);

                           

                        //    observacionesDetalle = oDetalle.Observaciones;
                        //    m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                        //    Derivacion oDeriva = new Derivacion();
                        //    oDeriva = (Derivacion)oDeriva.Get(typeof(Derivacion), "IdDetalleProtocolo", oDetalle);
                        //    if (oDeriva == null)  /// esta pendiente
                        //    {
                        //        estadoDerivacion = "Pendiente de Derivacion";
                        //        lblDerivacion.ForeColor = Color.Red;
                        //    }
                        //    else
                        //    {
                        //        if (oDeriva.Estado == 0) /// pendiente                            
                        //        {
                        //            estadoDerivacion = "Pendiente de Derivacion";
                        //            lblDerivacion.ForeColor = Color.Red;
                        //        }
                        //        if (oDeriva.Estado == 1) /// enviado
                        //            estadoDerivacion = "Derivado: " + oItem.IdEfectorDerivacion.Nombre;
                        //        if (oDeriva.Estado == 2) /// no enviado
                        //            estadoDerivacion = " No Derivado. " + oDeriva.Observacion;
                        //        lblDerivacion.Font.Bold = true;

                        //        if (oDeriva.Resultado!="")                                                           
                        //            estadoDerivacion += " - Resultado Informado: " + oDeriva.Resultado; 

                        //    }

                        //}
                        //lblDerivacion.Text = estadoDerivacion;
                        
                        //objCellResultado.ColumnSpan = 1;
                        //objCellResultado.Controls.Add(lblDerivacion);
                    }
                    else
                    {//No es derivado                     
                        if (m_trajoMuestra == "No")
                        {
                            Label lblSinMuestra = new Label();
                            lblSinMuestra.TabIndex = short.Parse("500");
                            lblSinMuestra.Text = "Sin Muestra";// +oItem.IdEfectorDerivacion.Nombre; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            lblSinMuestra.Font.Italic = true;
                            lblSinMuestra.ForeColor = Color.Blue;
                            //     objCellResultado.ColumnSpan = 5;
                            objCellResultado.Controls.Add(lblSinMuestra);
                        }
                        else
                        {
                            //ImageButton btnImagen = new ImageButton();
                            //if (tipodeterminacion == 0) // si es una determinacion simple
                            //{
                                Label olbl = new Label();
                                olbl.Font.Bold = true;
                                olbl.Font.Size = FontUnit.Point(9);
                                //if (m_conResultado == "0")
                                //    olbl.Text = "";
                                //else
                                    olbl.Text = Ds.Tables[0].Rows[i].ItemArray[3].ToString() + " "+ unMedida; 

                                if (Observaciones != "")
                                {
                                    if (olbl.Text == "")
                                        olbl.Text += Observaciones;
                                    else
                                        olbl.Text += Environment.NewLine + " " + Observaciones;

                                }
                            objCellResultado.Controls.Add(olbl);






                            Label lblPersona = new Label();
                                //    lblPersona.TabIndex = short.Parse("500");
                                lblPersona.Text = m_usuariovalida; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();      



                                /// 
                                lblPersona.Font.Size = FontUnit.Point(7);
                                lblPersona.Font.Italic = true;
                                lblPersona.Text = m_usuariovalida ;

                                objCellPersona.Controls.Add(lblPersona);

                                
                            //}


                            Label lblValoresReferencia = new Label();

                            //     lblValoresReferencia.ID = "VR" + m_idItem.ToString();
                            lblValoresReferencia.Font.Italic = true;
                            lblValoresReferencia.Font.Size = FontUnit.Point(8);
                            if (valorReferencia != "")
                            {// muestra el valor guardado 
                                lblValoresReferencia.Text = valorReferencia;
                                if (m_metodo != "")
                                    // lblValoresReferencia.Text += " |Método:" + m_metodo;
                                    lblValoresReferencia.Text += Environment.NewLine + m_metodo;
                            }
                            //else
                            //    lblValoresReferencia.Text = oDetalle.CalcularValoresReferencia();                                                                                      

                            objCellValoresReferencia.Controls.Add(lblValoresReferencia);
                        }
                    }


                    ///Definir los anchos de las columnas
                    objCellAnalisis.Width = Unit.Percentage(30);
                    objCellResultado.Width = Unit.Percentage(30);
                    objCellValoresReferencia.Width = Unit.Percentage(20);
                    //            objCellValida.Width = Unit.Percentage(5);
                    objCellPersona.Width = Unit.Percentage(20);



                    ///////////////////////
                    ///agrega a la fila cada una de las celdas
                    objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);
                    
                    //if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellUnMedida);

                    objFila.Cells.Add(objCellValoresReferencia);

                    //if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control")) objFila.Cells.Add(objCellValida);

                    objFila.Cells.Add(objCellPersona);

                    //objFila.Cells.Add(objCellResultadoAnterior);
                    //if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellObservaciones);

                    //////
                    Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                    //'añadimos la fila a la tabla
                    if (objFila != null)
                        tContenido.Controls.Add(objFila);//.Rows.Add(objRow);                                
                }
                }
            //}
        }



        private void Imprimir(int idProtocolo, string tipo)
        {
             

                  CrystalReportSource oCr = new CrystalReportSource();
            oCr.Report.FileName = "";
            oCr.CacheDuration =10000;
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
            
                conLogo.Value = false;


                encabezado1.Value = "HOSPITAL DR. CASTRO RENDON";
                encabezado2.Value = "SERVICIO DE LABORATORIO";
                encabezado3.Value = "";

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
                oCr.Report.FileName = "../Informes/Resultado.rpt";
                
            
            ParameterDiscreteValue datosPaciente = new ParameterDiscreteValue();
            datosPaciente.Value = parametroPaciente;                     

            ParameterDiscreteValue datosProtocolo = new ParameterDiscreteValue();
            datosProtocolo.Value = parametroProtocolo;

            string m_filtro = " WHERE E.idEfector=205  and E.idProtocolo =" + idProtocolo.ToString();

            //if (HidArea.Value != "0") m_filtro += " and idArea=" + HidArea.Value;

        



            DataTable d = GetDatosImprimir(m_filtro);// oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio);
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

            Utility oUtil = new Utility();
            string s_nombreProtocolo = lblProtocolo.Text.Trim();
            string nombrePDF = oUtil.CompletarNombrePDF(s_nombreProtocolo);
        
            GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(Session["idUsuario"].ToString()), idProtocolo, 205);
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

              

           

        }

        private void GrabarAuditoriaProtocolo(string accion,  int usuario, int idprotocolo, int idEfector)
        {
            SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"
        INSERT INTO [dbo].[LAB_AuditoriaProtocoloSIPS]
           ([idProtocolo]
           ,[idEfector]
           ,[fecha]        
           ,[accion]
           ,[idUsuario])   
     VALUES
           ( " + idprotocolo.ToString() + "," + idEfector.ToString() + ",getdate(),'" + accion + "'," + usuario.ToString() + " )";
            SqlCommand cmd = new SqlCommand(query, conn2);


            int idres2 = Convert.ToInt32(cmd.ExecuteScalar());
        
    }

        private DataTable GetDatosImprimir(string m_filtro)
        {
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //DataSet Ds = new DataSet();
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "LAB_ResultadoSIPS";

            //cmd.Parameters.Add("@idEfector", SqlDbType.NVarChar);
            //cmd.Parameters["@idEfector"].Value = 205;

            //cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            //cmd.Parameters["@idProtocolo"].Value = int.Parse(Session["idProtocolo"].ToString());

            //cmd.Parameters.Add("@tipo", SqlDbType.NVarChar);
            //cmd.Parameters["@tipo"].Value = "impreso";
            //cmd.Connection = conn;

            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            //da.Fill(Ds, "resultado");
             
            //DataTable data = Ds.Tables[0];
            //return data;



            SqlConnection connection;
            SqlCommand command;
            string connetionString = ConfigurationManager.ConnectionStrings["SIPS"].ConnectionString;
            connection = new SqlConnection(connetionString);

            DataSet Ds = new DataSet();

            string sql = @"SELECT  e.idProtocolo, 2 as estado, codigo, orden, apellido, nombre, edad, fechaNacimiento, sexo, numeroDocumento, fecha, domicilio, HC, prioridad, origen, area, 
                      grupo, item, d.resultado as resultadoCar, 0 as resultadoNum, observaciones, esTitulo, derivado, unidad,  e.fecha fechaEntrega, 
					  e.numero, e.hiv, metodo, valorReferencia, 
                      idDetalleProtocolo, 'SI'  muestra, case when solicitante is null then 'No informado' else solicitante end as solicitante, conresultado, 
					  0 as formatoDecimal,
					  2 as idTipoResultado, 
                    '0' AS formato0, 
                      '0' AS formato1, 
                    '0' AS formato2, 
                    '0' AS formato3, 
                    '0' AS  formato4 , sector, sala, cama,d.profesional_val as firmante, embarazo , 
					case unidadEdad 
						when 'Años' then 0
						when 'Meses' then 1
						when 'Dias' then 2
						end as unidadEdad,e.observacionesResultados as  observacionDetalle  
                     FROM   dbo.LAB_ResultadoEncabezado e
					 inner join   dbo.LAB_Resultadodetalle d on  e.idprotocolo= d.idprotocolo and e.idefector= d.idefector
					 where  e.idefector= 205 
and    e.idProtocolo=" + Session["idProtocolo"].ToString() + @"					 ORDER BY idProtocolo, ordenArea, orden, orden1";
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(sql, connection);
            adapter.Fill(Ds, "resultado");

            DataTable data = Ds.Tables[0];
            return data;



        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ingresar")
            {                                            
               dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
                if (dtProtocolo != null)
                {
                    for (int i = 0; i < dtProtocolo.Rows.Count; i++)
                    {
                        // dtProtocolo.Rows[i].Delete();
                        if (dtProtocolo.Rows[i][0].ToString() == e.CommandArgument.ToString()) CurrentIndexGrilla = i;
                    }
                    CurrentPageIndex = int.Parse(e.CommandArgument.ToString());
                    string s_peticion = "";

                    //if (Request["master"] != null) // es peticion electronic                   
                    //    s_peticion = "&master="+ Request["master"].ToString();

                    Session["idProtocolo"] = e.CommandArgument.ToString();

                     Response.Redirect("ResultadoView.aspx?idServicio=0&Operacion=" + Request["Operacion"].ToString() + "&Index=" + CurrentIndexGrilla     + s_peticion, false);
               }
               else
                      Response.Redirect("../FinSesion.aspx", false);   
            }

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Ingresar";


                string idProtocolo= this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                string s_detalle = e.Row.Cells[0].Text + ": " + getDetalleProtocolo(idProtocolo);

                e.Row.Cells[0].ToolTip = s_detalle;
                e.Row.Cells[1].ToolTip = s_detalle;

                CmdModificar.ToolTip = s_detalle;
            }


        }

        protected void cvValidaControles_ServerValidate(object source, ServerValidateEventArgs args)
        {                
            //if ( (ValidaControlesSuperior()) && (ValidaControlesInferior()) )
            //    args.IsValid=true;
            //else
            //    args.IsValid=false;
        }


     

        private bool AnalizarLimites(string p)
        {
            throw new NotImplementedException();
        }

        private bool estaVisibleControl(string idarea)
        {
            bool visible=true;
            //if (HidArea.Value == "0")
            //    visible = true;
            //else
            //{
            //    if (idarea == HidArea.Value)
            //        visible = true;
            //    else
            //        visible = false;
            //}
            return visible;

        }

     
     

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
          
        }

        private void Avanzar(int avance)
        {
            //try
            //{

            if (Session["Tabla1"] != null)
            {
                if (CurrentIndexGrilla <= UltimaPageIndex)
                {
                    if (avance == 1)
                    {
                        if (CurrentIndexGrilla < UltimaPageIndex) CurrentIndexGrilla += 1;  //avanza
                    }
                    else  //retrocede                        
                        CurrentIndexGrilla = CurrentIndexGrilla - 1;  //retrocede

                    if (CurrentIndexGrilla > -1)
                    {
                        dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
                        CurrentPageIndex = int.Parse(dtProtocolo.Rows[CurrentIndexGrilla][0].ToString());
                        string s_peticion = "";
                        if (Request["master"] != null) // es peticion electronic
                            s_peticion = "&master="+ Request["master"].ToString();

                        Session["idProtocolo"] = CurrentPageIndex.ToString();
                        Response.Redirect("ResultadoView.aspx?Operacion=" + Request["Operacion"].ToString() +  "&Index=" + CurrentIndexGrilla     + s_peticion, false);
                    }
                }
                else
                    if (Request["Operacion"].ToString() == "HC")
                        Response.Redirect("../Informes/HistoriaClinicaFiltro.aspx?Tipo=Paciente", false);
                    else
                        Response.Redirect("ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);

            }
            else Response.Redirect("../FinSesion.aspx", false);                             


             
        }

        protected void lnkPosterior_Click(object sender, EventArgs e)
        {
            Avanzar(1);
        }

        protected void lnkAnterior_Click(object sender, EventArgs e)
        {
            Avanzar(-1);
        }


        protected void lnkAuditoria_Click(object sender, EventArgs e)
        {
           
        }

    


      
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {

            Marcar(true);

        }

        private void Marcar(bool p)
        {

            CheckBox chk;
             

            if (Page.Master != null)
            {
                foreach (Control control in Page.Master.Controls)
                {
                    if (control is HtmlForm)
                    {
                        foreach (Control controlform in control.Controls)
                        {
                            if (controlform is ContentPlaceHolder)
                            {
                                foreach (Control control1 in controlform.Controls)
                                {
                                    if (control1 is Panel)
                                        foreach (Control control2 in control1.Controls)
                                        {
                                            if (control2 is Table)
                                                foreach (Control control3 in control2.Controls)
                                                {

                                                    if (control3 is TableRow)
                                                        foreach (Control control4 in control3.Controls)
                                                        {

                                                            if (control4 is TableCell)
                                                                foreach (Control control5 in control4.Controls)
                                                                {

                                                                    if (control5 is CheckBox)
                                                                    {
                                                                        chk = (CheckBox)control5;
                                                                        chk.Checked = p;
                                                                        
                                                                    
                                                                    
                                                                    }

                                                                    
                                                                }
                                                        }
                                                }
                                        }
                                }
                            }
                        }
                    }
                }
            }

        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            Marcar(false);
        }

        protected void btnVerAntecendente_Click(object sender, EventArgs e)
        {
            //if (ddlItem.SelectedValue != "0")
            //{
            //    Protocolo oProtocolo = new Protocolo();
            //    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            //    CargarGrillaAntecedentes(oProtocolo);
            //    SetSelectedTab(TabIndex.THREE);
            //}
        }

 

    
       

        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                //Protocolo oProtocolo = new Protocolo();
                //oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                Imprimir(CurrentPageIndex, "I");       
            }
            else
                Response.Redirect("../FinSesion.aspx", false);      
            
        }

        protected void btnPeticion_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PeticionElectronica/PeticionEdit.aspx?idPaciente=" + HFIdPaciente.Value + "&master=1", false);
        }


        private string getDetalleProtocolo(string idProtocolo)
        {
            string dev = ""; int i = 0;
            //Protocolo oRegistro = new Protocolo();
            //oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(idProtocolo));

            //ISession m_session = NHibernateHttpModule.CurrentSession;
            //ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            //crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            //IList items = crit.List();
            //foreach (DetalleProtocolo oDet in items)
            //{
            //    i += 1;
            //    if (dev == "")
            //        dev = oDet.IdItem.Nombre;
            //    else
            //    {
            //        if (dev.IndexOf(oDet.IdItem.Nombre) == -1)
            //            dev = dev + " - " + oDet.IdItem.Nombre;
            //    }
            //}
            ////return i.ToString() + ": " + dev;
            return  dev;
        }

        protected void btnArchivos_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Protocolos/ProtocoloAdjuntar.aspx?idProtocolo=" + Session["idProtocolo"].ToString()+"&desde=resultado");
        }

        protected void imgPdf_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                //Protocolo oProtocolo = new Protocolo();
                //oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                Imprimir(CurrentPageIndex, "PDF");
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        protected void btnMasResultados_Click(object sender, EventArgs e)
        {
          
                Response.Redirect("http://www.saludnqn.gob.ar/sips/laboratorio/Resultados/ProtocoloList.aspx?id=" + lblDni.Text);
            
        }
    }
}
