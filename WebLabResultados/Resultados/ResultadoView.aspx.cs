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
//using Business;
//using Business.Data.Laboratorio;
using System.Drawing;
//using NHibernate;
//using NHibernate.Expression;
//using Business.Data;
using MathParser;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
//using System.Collections.Generic;
//using Business.Data.AutoAnalizador;

namespace WebLabResultados.Resultados
{
    public partial class ResultadoView : System.Web.UI.Page
    {
      
        //CrystalReportSource oCr = new CrystalReportSource();
        bool hayAntecedente = false;
     
        DataTable dtProtocolo;
        


     
        //protected void Page_UnLoad(object sender, EventArgs e)
        //{  
        //    oCr.Dispose();
        //}


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Business.Utility oUtil = new Business.Utility();
                string param = Request["de"].ToString();  //va a produccion www.saludnqn.gob.ar/ressil/default.aspx?de=228n396277 ==>encriptado
                                                          //string param = oUtil.Encrypt("228n396277");
                                                          //Request["EF"]
                                                          //      Request["idProtocolo"];

                param = param.Replace(' ', '+').Replace('-', '+').Replace('_', '/').PadRight(4 * ((param.Length + 3) / 4), '=');
                string deco = oUtil.DecryptarNet(param, "SIL", 256);
                string[] ar = deco.Split('n');
                string idEfe = ar[0];
                string idpro = ar[1];
                string Efector = idEfe;//Context.Items["EF"].ToString();

                Session["idProtocolo"] = idpro; // Context.Items["idProtocolo"].ToString();
                //Desde.Value = Context.Items["Desde"].ToString();

                if (Session["idProtocolo"] != null)

                {


                    //Protocolo oRegistro = new Protocolo();
                    //oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Session["idProtocolo"].ToString()));

                   
                        Inicializar();
                  
                 
                    LlenarTabla(Session["idProtocolo"].ToString(),Efector);
                            LlenarTablaATB(Session["idProtocolo"].ToString(), Efector);

                    

                }
               
             
                                            
                else
                    Response.Redirect("../FinSesion.aspx", false);                             
            }
        }


  
        private void Inicializar()
        {           
            
           
            //pnlAntecedentes.Visible = false;
                    
            pnlHC.Visible = true;
            //if (Session["validado"].ToString() == "1")
            //    lblTitulo.Text = "HISTORIAL DE RESULTADOS";
            //else
                //lblTitulo.Text = "CONSULTA DE RESULTADOS"; 

            //pnlReferencia.Visible = true;

            MuestraDatos( );

           


        }









        //private void CargarGrilla()
        //{
        //    ////Metodo que carga la grilla de Protocolos

        //    string m_strSQL = " Select distinct P.idProtocolo, " +
        //                      " P.numero as numero," +
        //                      " convert(varchar(10),P.fecha,103) as fecha,P.estado , P.fecha as fecha1," +
        //                      " prefijosector, numerosector , numerodiario" +
        //                      " from Lab_Protocolo P " + // +str_condicion;                          
        //                      " WHERE P.idProtocolo in (" + Session["Parametros"].ToString() + ")";

        //    if (Request["Operacion"].ToString() == "HC")
        //        m_strSQL += " order by idProtocolo desc "; // desde el mas reciente al mas antiguo.
        //    else
        //    {
        //        Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
        //        if (oC.TipoNumeracionProtocolo == 0) m_strSQL += " order by  idProtocolo ";
        //        if (oC.TipoNumeracionProtocolo == 1) m_strSQL += " order by  numerodiario ";
        //        if (oC.TipoNumeracionProtocolo == 2) m_strSQL += " order by prefijosector, numerosector ";
        //    }


        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);
        //    gvLista.DataSource = Ds.Tables[0];
        //    gvLista.DataBind();
        //    dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);


        //    if (Ds.Tables[0].Rows.Count > 0)
        //    {
        //        dtProtocolo = Ds.Tables[0];
        //        int ultimafila = Ds.Tables[0].Rows.Count - 1;
        //        CurrentPageIndex = int.Parse(Session["idProtocolo"].ToString());
        //        CurrentIndexGrilla = int.Parse(Request["Index"].ToString());
        //        //CurrentPageIndex = int.Parse(Ds.Tables[0].Rows[CurrentIndexGrilla].ItemArray[0].ToString());
        //        UltimaPageIndex = ultimafila; // int.Parse(Ds.Tables[0].Rows[ultimafila].ItemArray[0].ToString());
        //    }

        //    lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados";
        //    Session.Add("Tabla1", dtProtocolo);


        //}







        private void MuestraDatos()
        {
            Business.Utility oUtil = new Business.Utility();
            string param = Request["de"].ToString();  //va a produccion www.saludnqn.gob.ar/ressil/default.aspx?de=228n396277 ==>encriptado
                                                      //string param = oUtil.Encrypt("228n396277");
                                                      //Request["EF"]
                                                      //      Request["idProtocolo"];

            param = param.Replace(' ', '+').Replace('-', '+').Replace('_', '/').PadRight(4 * ((param.Length + 3) / 4), '=');
            string deco = oUtil.DecryptarNet(param, "SIL", 256);
            string[] ar = deco.Split('n');
            string idEfe = ar[0];
            string idpro = ar[1];
            string Efector = idEfe;//Context.Items["EF"].ToString();


            SqlConnection connection;
            //string Efector = Context.Items["EF"].ToString();

            string connetionString = ConfigurationManager.ConnectionStrings[Efector].ConnectionString;

            connection = new SqlConnection(connetionString);

            DataSet Ds = new DataSet();

            string sql = @"select P.idtipoServicio, M.nombre as muestra, idCasoSISA as numerosisa, P.observacionesResultados as ObservacionResultado,
C.nombre as caracter, T.nombre as tipoServicio, P.estado, convert(varchar(10),P.fecha,103) as fecha, P.numero,P.Especialista, E.nombre as solicitante,
P.idPaciente, Pa.idEstado, Pa.numeroDocumento, Pa.apellido, Pa.nombre, P.sexo, convert(varchar(10),Pa.fechaNacimiento,103) as fechaNacimiento, P.edad, P.unidadEdad	 ,   Con.encabezadoLinea1
from lab_protocolo P
left join lab_muestra M on M.idmuestra= P.idmuestra
left join LAB_Caracter C on C.idCaracter= P.idCaracter
inner join LAB_TipoServicio T on T.idTipoServicio= P.idTipoServicio
inner join Sys_Efector E on E.idEfector= P.idEfectorSolicitante
inner join Sys_Paciente Pa on Pa.idPaciente= P.idPaciente
inner join lab_configuracion as Con on Con.idEfector= P.idEfector
where	 idProtocolo=" + Session["idProtocolo"].ToString();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(sql, connection);
            adapter.Fill(Ds);

            ///Muestra los datos de encabezado para el protocolo seleccionado
            ////CargarListasObservaciones("gral");
            //Protocolo oRegistro = new Protocolo();
            //oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Session["idProtocolo"].ToString()));
            HFIdProtocolo.Value = Session["idProtocolo"].ToString(); // p;
                                                                     //Actualiza los datos de los objetos : alta o modificacion .                                        
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                //decimal m_minimoReferencia=-1;
                //decimal m_maximoReferencia=-1;
                string idTipoServicio = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                string muestra = Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                string casoSISA = Ds.Tables[0].Rows[i].ItemArray[2].ToString();
                string ObservacionResultado = Ds.Tables[0].Rows[i].ItemArray[3].ToString();
                string caracter = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                string tipoServicio = Ds.Tables[0].Rows[i].ItemArray[5].ToString();
                int estado = int.Parse(Ds.Tables[0].Rows[i].ItemArray[6].ToString());

                string s_fecha = Ds.Tables[0].Rows[i].ItemArray[7].ToString();
                string numero = Ds.Tables[0].Rows[i].ItemArray[8].ToString();
                string Especialista = Ds.Tables[0].Rows[i].ItemArray[9].ToString();

                string solicitante = Ds.Tables[0].Rows[i].ItemArray[10].ToString();

            

                if ((idTipoServicio == "3") || (idTipoServicio == "5")) //Microbiologia
                {

                    lblMuestra.Text = "Tipo de Muestra: " + muestra;


                }

                if (casoSISA != "0")
                    lblNroSISA.Text = "Nro. SISA:" + casoSISA;
                else
                    lblNroSISA.Visible = false;

                if (ObservacionResultado != "")
                    lblObservacionResultado.Text = ObservacionResultado;
                if (caracter != "")
                {
                    lblCovid.Visible = true;

                    lblCovid.Text = "Clasificación Covid-19: " + caracter;
                }


                //lblServicio.Text = tipoServicio;



                //Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);
                Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl("tContenido");
                Table tablaContenido = (Table)control1;
                int cantidadFilas = 0;
                if (tablaContenido != null)
                {
                    cantidadFilas = tablaContenido.Rows.Count;
                    if ((cantidadFilas == 1) || (estado == 0))
                    {

                        Panel1.Visible = false;
                        pnlReferencia.Visible = false;

                        lblEstadoProtocolo.Visible = true;
                        lblEstadoProtocolo.Text = " PROTOCOLO EN PROCESO";


                    }

                }


                //lblUsuario.Text = oRegistro.IdUsuarioRegistro.Apellido;
                //lblFechaRegistro.Text = oRegistro.FechaRegistro.ToShortDateString() + " " + oRegistro.FechaRegistro.ToShortTimeString();
                //int len = oRegistro.FechaRegistro.ToString().Length - 11;
                //lblHoraRegistro.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);
                lblFecha.Text = s_fecha;
                lblProtocolo.Text = numero;
                lblMedico.Text = Especialista;

                lblSolicitante.Text = solicitante;


                string idPaciente = Ds.Tables[0].Rows[i].ItemArray[11].ToString();
                string idPacEstado = Ds.Tables[0].Rows[i].ItemArray[12].ToString();
                string numeroDocumento = Ds.Tables[0].Rows[i].ItemArray[13].ToString();
                string apellido = Ds.Tables[0].Rows[i].ItemArray[14].ToString();
                string nombre = Ds.Tables[0].Rows[i].ItemArray[15].ToString();
                string sexo = Ds.Tables[0].Rows[i].ItemArray[16].ToString();
                string fechaNacimiento = Ds.Tables[0].Rows[i].ItemArray[17].ToString();
                string edad = Ds.Tables[0].Rows[i].ItemArray[18].ToString();
                string unidadEdad = Ds.Tables[0].Rows[i].ItemArray[19].ToString();

                string laboratorio = Ds.Tables[0].Rows[i].ItemArray[20].ToString();
                lblEfector.Text = laboratorio;


                HFIdPaciente.Value = idPaciente;

                ///Datos del Paciente       
                if (idPacEstado == "2")
                {
                    lblDni.Text = "(Sin DU Temporal)  ";

                }
                else lblDni.Text = numeroDocumento;

                lblPaciente.Text = apellido.ToUpper() + " " + nombre.ToUpper();

                lblSexo.Text = sexo; // ver si se puede recuperar
                lblFechaNacimiento.Text = fechaNacimiento;
                lblEdad.Text = edad;
                switch (int.Parse(unidadEdad))
                {
                    case 0: lblEdad.Text += " años"; break;
                    case 1: lblEdad.Text += " meses"; break;
                    case 2: lblEdad.Text += " días"; break;
                }


                //// ver como hacer
                //lblCodigoPaciente.Text = oRegistro.getCodificaHiv(embarazada); // lblSexo.Text.Substring(0, 1) + oRegistro.IdPaciente.Nombre.Substring(0, 2) + oRegistro.IdPaciente.Apellido.Substring(0, 2) + lblFechaNacimiento.Text.Replace("/", "") + embarazada;            
                
                if (nombre.Length >= 2) nombre = nombre.Substring(0, 2);
                else nombre = nombre + " ";

                if (apellido.Length >= 2) apellido = apellido.Substring(0, 2);
                else apellido = apellido + " ";
                 
                lblCodigoPaciente.Text = sexo + nombre + apellido +fechaNacimiento.Replace("/", "") ;
                
                //lblPedidoOriginal.Text = oRegistro.GetPracticasPedidas();



            }
        }




        private void LlenarTabla(string p, string Efector)
        {
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;         
           
          
            SqlConnection conn;

            string connetionString = ConfigurationManager.ConnectionStrings[Efector].ConnectionString;
            conn = new SqlConnection(connetionString);


            DataSet Ds = new DataSet();            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "LAB_ResultadoView";

            cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idProtocolo"].Value = p;
            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);

          
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
            lblValida.Text = "";
            //if ((Request["Operacion"].ToString() == "Carga") || (Request["Operacion"].ToString() == "HC")) lblValida.Text = "";
            //    else
            //    {
            //        if (Request["Operacion"].ToString() == "Valida") lblValida.Text = "VAL";
            //        if (Request["Operacion"].ToString() == "Control") lblValida.Text = "CTRL";
            //    }
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

                //objFila_TITULO.CssClass = "myLabelIzquierda";
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
                    if (Ds.Tables[0].Rows[i].ItemArray[4].ToString() != "")
                        tiporesultado = 2;
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
                        //objRow.CssClass = "myLabelIzquierda";
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

                    //DetalleProtocolo oDetalle = new DetalleProtocolo();
                    //oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);

                    //Item oItem = new Item();
                    //oItem = (Item)oItem.Get(typeof(Item), m_idItem);

                    //DetalleProtocolo oDetalle = new DetalleProtocolo();
                    //oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);


                  //  bool es_Bacteriologia = false;
                    string observacionesDetalle = "";
                    ///Antes de mostrar el control verifica  si está derivado 
                    /// 

                    //Caro: Ver los dos campos de observaciones
                    //observacionesDetalle = oDetalle.Observaciones;
                    //m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                    //Derivacion oDeriva = new Derivacion();
                    //oDeriva = (Derivacion)oDeriva.Get(typeof(Derivacion), "IdDetalleProtocolo", oDetalle);
                    //if (oDeriva != null)  /// esta pendiente

                    ////if (oItem.IdEfectorDerivacion != oItem.IdEfector) //es derivado
                    //{
                    //    Label lblDerivacion = new Label();
                    //    lblDerivacion.Font.Italic = true;
                    //    lblDerivacion.TabIndex = short.Parse("500");
                    //    //Verifica el estado de la derivacion
                    //    string estadoDerivacion = "";

                    //    //if (i_iddetalleProtocolo != 0)
                    //    //{
                    //    //    DetalleProtocolo oDetalle = new DetalleProtocolo();
                    //    //    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);



                    //    //    observacionesDetalle = oDetalle.Observaciones;
                    //    //    m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                    //    //    Derivacion oDeriva = new Derivacion();
                    //    //    oDeriva = (Derivacion)oDeriva.Get(typeof(Derivacion), "IdDetalleProtocolo", oDetalle);
                    //    //    if (oDeriva == null)  /// esta pendiente
                    //    //    {
                    //    //        estadoDerivacion = "Pendiente de Derivacion";
                    //    //        lblDerivacion.ForeColor = Color.Red;
                    //    //    }
                    //    //    else
                    //    //    {
                    //    if (oDeriva.Estado == 0) /// pendiente                            
                    //    {
                    //        estadoDerivacion = "Pendiente de Derivacion";
                    //        lblDerivacion.ForeColor = Color.Red;
                    //    }
                    //    if (oDeriva.Estado == 1) /// enviado
                    //        estadoDerivacion = oDetalle.ResultadoCar; //  "Derivado: " + oItem.GetEfectorDerivacion(oCon.IdEfector);
                    //    if (oDeriva.Estado == 2) /// no enviado
                    //        estadoDerivacion = oDetalle.ResultadoCar; //" No Derivado. " + oDeriva.Observacion;
                    //    lblDerivacion.Font.Bold = true;

                    //    if (oDeriva.Resultado != "")
                    //        estadoDerivacion += " - Resultado Informado: " + oDeriva.Resultado;

                    //    //}

                    ////}
                    //    lblDerivacion.Text = estadoDerivacion;

                    //    objCellResultado.ColumnSpan = 1;
                    //    objCellResultado.Controls.Add(lblDerivacion);
                    //}
                    if (1 == 2)
                    { }
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
                            ImageButton btnImagen = new ImageButton();
                            if (tipodeterminacion == 0) // si es una determinacion simple
                            {
                                switch (tiporesultado)
                                {//tipoResultado

                                    //case 5://fusion
                                    //    {
                                    //        //if (Request["Operacion"].ToString() != "HC")
                                    //        if (m_conResultado != "False")
                                    //        {
                                    //            //    DetalleProtocolo oDetalle = new DetalleProtocolo();
                                    //            //    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);

                                    //            Anthem.GridView Gd1 = new Anthem.GridView();
                                    //            Gd1.ID = m_idItem.ToString();
                                    //            ProtocoloLuminex oFusion = new ProtocoloLuminex();

                                    //            Gd1.DataSource = oFusion.LeerDatosProtocolos(oDetalle.IdProtocolo.Numero.ToString(), oDetalle.IdItem.IdItem);
                                    //            Gd1.DataBind();
                                    //            objCellResultado.Controls.Add(Gd1);
                                    //        }
                                    //    }
                                    //    break;
                                    case 1: //numerico
                                        {
                                            bool imgAdj = false;
                                            Label lblValorCritico = new Label();
                                            switch (m_formatoDecimal)
                                            {
                                                case "0": x = decimal.Parse(m_formato0); break;
                                                case "1": x = decimal.Parse(m_formato1); break;
                                                case "2": x = decimal.Parse(m_formato2); break;
                                                case "3": x = decimal.Parse(m_formato3); break;
                                                case "4": x = decimal.Parse(m_formato4); break;
                                            }

                                            Label olbl = new Label();
                                            olbl.Font.Bold = true;
                                            olbl.Font.Size = FontUnit.Point(9);
                                            if (m_conResultado == "False")
                                                olbl.Text = "";
                                            else
                                                olbl.Text = x.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + unMedida;

                                            if (i_iddetalleProtocolo != 0)
                                            {
                                                if (i_iddetalleProtocolo != 9999999)
                                                {
                                                    //DetalleProtocolo oDetalle = new DetalleProtocolo();
                                                    //oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);

                                                    if (Observaciones != "")
                                                    {
                                                        if (m_conResultado == "False")
                                                        {
                                                            olbl.Text = Observaciones;
                                                            if (m_usuariovalida == "")
                                                            {
                                                                ///Caro: ver que hacer con esto
                                                                //Usuario oUser = new Usuario();
                                                                //oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValidaObservacion);
                                                                //if (oUser.FirmaValidacion == "") m_usuariovalida = oUser.Apellido + " " + oUser.Nombre;
                                                                //else m_usuariovalida = oUser.FirmaValidacion;

                                                                //m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                                                            }
                                                        }
                                                        else
                                                            olbl.Text = olbl.Text + Observaciones;
                                                    }


                                                    
                                                    //// if (VerificaValorReferencia(m_minimoReferencia, m_maximoReferencia, x, m_tipoValorReferencia))
                                                    //if (oDetalle.VerificaValorReferencia(x))
                                                    //    olbl.ForeColor = Color.Black;
                                                    //else
                                                    //{
                                                    //    olbl.ForeColor = Color.Red;
                                                    //    pnlReferencia.Visible = true;
                                                    //}

                                                    //if (oDetalle.VerificaValoresLimites(x))
                                                    //{
                                                    //    // verifica si tiene valores fuera de limite: es un valor critico

                                                    //    lblValorCritico.TabIndex = short.Parse("500");
                                                    //    lblValorCritico.Text = " VALOR CRITICO";
                                                    //    lblValorCritico.Font.Bold = true;
                                                    //    lblValorCritico.ForeColor = Color.OrangeRed;
                                                    //    //     objCellResultado.ColumnSpan = 5;

                                                    //}

                                                    ///IMAGENES ADJUNTAS                                              




                                                    //if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                    //{
                                                    //    imgAdj = true;
                                                    //    btnImagen.TabIndex = short.Parse("500");
                                                    //    btnImagen.ID = "IMG" + oDetalle.IdDetalleProtocolo.ToString();
                                                    //    btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";

                                                    //    btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");


                                                    //    btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'HC'); return false");

                                                    //}

                                                    // fin de imagenes adjuntas



                                                }

                                            }
                                            objCellResultado.Controls.Add(olbl);
                                            if (imgAdj) objCellResultado.Controls.Add(btnImagen);
                                            ///etiqueta de unidad de medida
                                            Label olblUM = new Label();


                                            //     olblUM.ID = "UM" + m_idItem.ToString();
                                            olblUM.Font.Size = FontUnit.Point(7);
                                            olblUM.Text = unMedida;





                                            objCellResultado.Controls.Add(olblUM);
                                            olblUM.Visible = false;
                                            objCellResultado.Controls.Add(lblValorCritico);

                                            //////////////////
                                        } // fin case 1
                                        break;
                                    default: //texto
                                        {

                                            Label olbl = new Label();
                                            olbl.Font.Bold = true;
                                            olbl.Font.Size = FontUnit.Point(9);
                                            if (m_conResultado == "0")
                                                olbl.Text = "";
                                            else
                                                olbl.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();

                                            if (Observaciones != "")
                                            {
                                                if (olbl.Text == "")
                                                    olbl.Text += Observaciones;
                                                else
                                                    olbl.Text += Environment.NewLine + " " + Observaciones;

                                            }

                                            ///IMAGENES ADJUNTAS                                              





                                            // fin de imagenes adjuntas

                                           objCellResultado.Controls.Add(olbl);

                                            //if ((i_iddetalleProtocolo != 0))
                                            //{
                                            //    if (i_iddetalleProtocolo != 9999999)
                                            //    {
                                            //        //DetalleProtocolo oDetalle = new DetalleProtocolo();
                                            //        //oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);

                                            //        m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                                                   
                                            //        if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                            //        {
                                            //            //   ImageButton btnImagen = new ImageButton();
                                            //            btnImagen.TabIndex = short.Parse("500");
                                            //            btnImagen.ID = "IMG" + oDetalle.IdDetalleProtocolo.ToString();
                                            //            btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";

                                            //            btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");


                                            //            btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'HC'); return false");
                                            //            objCellResultado.Controls.Add(btnImagen);
                                            //        }
                                            //    }
                                            //}

                                        } // fin case 1
                                        break;


                                }//fin swicth



                                Label lblPersona = new Label();
                                //    lblPersona.TabIndex = short.Parse("500");
                                lblPersona.Text = m_usuariovalida; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();      



                                /// 
                                lblPersona.Font.Size = FontUnit.Point(7);
                                lblPersona.Font.Italic = true;
                                lblPersona.Text = m_usuariovalida;

                                objCellPersona.Controls.Add(lblPersona);


                            }


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
                    objCellAnalisis.Width = Unit.Percentage(100);
                    objCellResultado.Width = Unit.Percentage(100);
                    objCellValoresReferencia.Width = Unit.Percentage(100);
                    //            objCellValida.Width = Unit.Percentage(5);
                    objCellPersona.Width = Unit.Percentage(100);



                    ///////////////////////
                    ///agrega a la fila cada una de las celdas
                    objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);
                    
                    //if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellUnMedida);

                    objFila.Cells.Add(objCellValoresReferencia);

                    //if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control")) objFila.Cells.Add(objCellValida);

                    objFila.Cells.Add(objCellPersona); 
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



        private void LlenarTablaATB(string p, string Efector)
        {
            SqlConnection conn;

            string connetionString = ConfigurationManager.ConnectionStrings[Efector].ConnectionString;
            conn = new SqlConnection(connetionString);


            DataSet Ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "LAB_ResultadoViewATB";

            cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idProtocolo"].Value = p;
            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);


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

            int tablas = Ds.Tables.Count;
            if (tablas > 0)
            {


               


                objFila_TITULO.Cells.Add(objCellAnalisis_TITULO);
                objFila_TITULO.Cells.Add(objCellResultado_TITULO);




                objFila_TITULO.Cells.Add(objCellValoresReferencia_TITULO);


                objFila_TITULO.Cells.Add(objCellPersona_TITULO);
                objFila_TITULO.Cells.Add(objCellResultadoAnterior_TITULO);

                objFila_TITULO.CssClass = "myLabelIzquierda";
                objFila_TITULO.BackColor = Color.Gainsboro;

                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                //'añadimos la fila a la tabla
                if (objFila_TITULO != null) tContenido.Controls.Add(objFila_TITULO);//.Rows.Add(objRow);    

                string pivot_Area = "";


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
                    string m_area = "ATB";//Ds.Tables[0].Rows[i].ItemArray[30].ToString();

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
                      
                            objCell.ColumnSpan = 6;
                      

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


                    if (tipodeterminacion == 0) // si es una determinacion simple
                    {


                        Label olbl = new Label();
                        olbl.Font.Bold = true;
                        olbl.Font.Size = FontUnit.Point(9);
                        if (m_conResultado == "0")
                            olbl.Text = "";
                        else
                            olbl.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();

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
                        lblPersona.Text = m_usuariovalida;
                        objCellPersona.Controls.Add(lblPersona);
                        
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

                    /////Definir los anchos de las columnas
                    //objCellAnalisis.Width = Unit.Percentage(30);
                    //objCellResultado.Width = Unit.Percentage(30);
                    //objCellValoresReferencia.Width = Unit.Percentage(20);
                    ////            objCellValida.Width = Unit.Percentage(5);
                    //objCellPersona.Width = Unit.Percentage(20);

                    ///////////////////////
                    ///agrega a la fila cada una de las celdas
                    objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);
                    //if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellUnMedida);
                    objFila.Cells.Add(objCellValoresReferencia);

                    //if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control")) objFila.Cells.Add(objCellValida);

                    objFila.Cells.Add(objCellPersona);

               //     objFila.Cells.Add(objCellResultadoAnterior);
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

     
    


      
       
 

    
       

   


        //private string getDetalleProtocolo(string idProtocolo)
        //{
        //    string dev = ""; int i = 0;
        //    Protocolo oRegistro = new Protocolo();
        //    oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(idProtocolo));

        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
        //    crit.Add(Expression.Eq("IdProtocolo", oRegistro));
        //    IList items = crit.List();
        //    foreach (DetalleProtocolo oDet in items)
        //    {
        //        i += 1;
        //        if (dev == "")
        //            dev = oDet.IdItem.Nombre;
        //        else
        //        {
        //            if (dev.IndexOf(oDet.IdItem.Nombre) == -1)
        //                dev = dev + " - " + oDet.IdItem.Nombre;
        //        }
        //    }
        //    //return i.ToString() + ": " + dev;
        //    return  dev;
        //}

        protected void btnArchivos_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Protocolos/ProtocoloAdjuntar.aspx?idProtocolo=" + Session["idProtocolo"].ToString()+"&desde=resultado");
        }

     


      

    }
}
