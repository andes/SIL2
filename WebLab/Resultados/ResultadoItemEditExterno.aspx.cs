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
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Drawing;
using Business.Data;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Text;
using System.Net;
using System.IO;

namespace WebLab.Resultados
{
    public partial class ResultadoItemEditExterno : System.Web.UI.Page
    {
        string listavalidado = "";
        int suma1 = 0;
        Configuracion oCon = new Configuracion();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
 
            if (Session["idUsuario"] != null)
            {
                if (Request["idItem"] != null)
                {
                    LlenarTabla(Request["idItem"].ToString());
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    Inicializar();
                    if (Request["mostrarResumen"] != null)
                        MostrarResumen();
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }

        }

        private void MostrarResumen()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  select  resultadocar as Resultado, count (*) as Cantidad
   from LAB_DetalleProtocolo where idDetalleProtocolo in (" + Session["ListaValidado"].ToString() + ") group by resultadocar " ;
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumen.DataSource = Ds.Tables[0];
            gvResumen.DataBind();
            gvResumen.Visible = true;
            lblProcesado.Visible = true;

            // sisa
            lblMensajeSISA.Visible = false;
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (oCon.NotificarSISA) 
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                int i = 0;
                string ssql_Protocolo = " IdDetalleProtocolo in (Select IdDetalleProtocolo from LAB_DetalleProtocolo where IdDetalleProtocolo in (" + Session["ListaValidado"].ToString() + "))";
                crit.Add(Expression.Sql(ssql_Protocolo));
                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    foreach (DetalleProtocolo oDetalle in detalle)
                    {
                        if (oDetalle.IdProtocolo.IdCaracter != 2) // no se suben controles de alta
                        {
                            if ((oDetalle.IdProtocolo.IdPaciente.IdEstado == 3) && (oDetalle.IdItem.Codigo == oCon.CodigoCovid))
                            {
                                string res = oDetalle.ResultadoCar;
                                if (oDetalle.IdProtocolo.VerificarProtocoloAnterior(14))
                                {
                                    if (res.Length > 10)
                                    {
                                        if (res.Substring(0, 10) == "SE DETECTA")
                                        { if (ProcesaSISA(oDetalle, "SE DETECTA")) i = i + 1; }
                                    }
                                    if (res.Length > 13)
                                    {
                                        if (res.Substring(0, 13) == "NO SE DETECTA")
                                        { if (ProcesaSISA(oDetalle, "NO SE DETECTA")) i = i + 1; }
                                    }
                                }//oDetalle.IdProtocolo.Verificar
                            }//  if ((oDetalle.IdProtocolo.IdCasoS
                        }//  if (oDetalle.IdProtocolo.IdCaracter 
                    }//  foreach (DetalleP
                }//   if (detalle.
                if (i > 0)
                {
                    lblMensajeSISA.Text ="Se han generado "+ i.ToString() + " eventos nuevos en SISA";
                    lblMensajeSISA.Visible = true;
                }
            }//  if (oCon.NotificarSISA)



        }

        private bool ProcesaSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;

            try
            {
                if (oDetalle.IdProtocolo.IdCasoSISA == 0)
                {
                    generacaso = GenerarCasoSISA(oDetalle, res);

                }

                if (oDetalle.IdProtocolo.IdCasoSISA > 0)
                    GenerarMuestraSISA(oDetalle.IdProtocolo);

                if (oDetalle.IdeventomuestraSISA > 0)
                    GenerarResultadoSISA(oDetalle);


            }
            catch (Exception e)
            {
                generacaso = false;


            }
            return generacaso;

        }

        private bool GenerarCasoSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;
            string caracter = "";
            string idevento = "";
            string nombreevento = "";
            string idclasificacionmanual = "";
            string nombreclasificacionmanual = "";
            string idgrupoevento = "";
            string nombregrupoevento = "";
            bool seguir = true;
            string m_strSQL = "";

            try
            {



                if (res == "SE DETECTA")
                    m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1  ";
                else

                {
                    // si es contacto se sube: si es negativo como contacto y si es positivo como sospechoso.
                    if (oDetalle.IdProtocolo.IdCaracter == 4)  //contacto 
                        m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=4  ";
                    else
                    {
                        if (oDetalle.IdProtocolo.IdCaracter == 8)  //operativo detectar se sube como sospechoso
                            m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1  ";
                        else
                            seguir = false;
                    }
                }

                // nose notificò antes y es sospechoso o contacto


                if (seguir)
                {
                    DataSet Ds = new DataSet();
                    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    adapter.Fill(Ds);

                    DataTable dt = Ds.Tables[0];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        caracter = dt.Rows[i][1].ToString();
                        idevento = dt.Rows[i][2].ToString();
                        nombreevento = dt.Rows[i][3].ToString();
                        idclasificacionmanual = dt.Rows[i][4].ToString();
                        nombreclasificacionmanual = dt.Rows[i][5].ToString();
                        idgrupoevento = dt.Rows[i][6].ToString();
                        nombregrupoevento = dt.Rows[i][7].ToString();

                    }


                    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                    string URL = oCon.UrlServicioSISA;
                    string s_idestablecimiento = oCon.CodigoEstablecimientoSISA; // "14580562167000"
                    string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
                    string[] a = usersisa.Split(':');
                    string s_user = a[0].ToString();
                    string s_userpass = a[1].ToString();

                    string s_sexo = "";
                    switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                    {
                        case 1: s_sexo = "I"; break;
                        case 2: s_sexo = "F"; break;
                        case 3: s_sexo = "M"; break;
                    }
                    string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");

                    string fnpapel = oDetalle.IdProtocolo.FechaOrden.ToShortDateString().Replace("/", "-");


                    string numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();

                    string error = "";
                    //bool hayerror = false;

                    evento newevento = new evento
                    {
                        idTipodoc = "1",
                        nrodoc = numerodocumento,
                        sexo = s_sexo,
                        fechaNacimiento = fn,  //"05-06-1989",
                        idGrupoEvento = idgrupoevento,
                        idEvento = idevento, // "77",
                        idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                        fechaPapel = fnpapel, // "10-12-2019",
                        idClasificacionManualCaso = idclasificacionmanual, // "22"
                    };

                    AltaCaso caso = new AltaCaso
                    {
                        usuario = s_user,
                        clave = s_userpass,
                        altaEventoCasoNominal = newevento
                    };

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    string DATA = jsonSerializer.Serialize(caso);



                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                    client.BaseAddress = new System.Uri(URL);

                    System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");
                    HttpResponseMessage messge = client.PostAsync(URL, content).Result;
                    string description = string.Empty;
                    if (messge.IsSuccessStatusCode)
                    {
                        string result = messge.Content.ReadAsStringAsync().Result;
                        description = result;
                        RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(description);
                        if (respuesta_d.id_caso!="")
                        
                        { //  devolver el idcaso para guardar en la base de datos
                            string s_idcaso = respuesta_d.id_caso;

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            if (respuesta_d.resultado == "OK")
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, oDetalle.IdUsuarioValida);
                            else
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, oDetalle.IdUsuarioValida);
                            generacaso = true;

                            //generar muestra
                            GenerarMuestraSISA(oDetalle.IdProtocolo);




                        }
                        else
                        {
                            generacaso = false;
                            //hayerror = true;
                            error = respuesta_d.resultado;

                        }
                    }

                }

            }
            catch
            {
                generacaso = false;
                //lblError.Text = "Hubo algun problema al conectar al servicio SISA: " + e.InnerException.InnerException.Message.ToString() + ". Intente de nuevo o haga clic en Salir";
                //lblError.Visible = true;
                //btnSalir.Visible = true;
            }
            return generacaso;

        }
    
        private void GenerarMuestraSISA(Protocolo protocolo)

        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLMuestraSISA;


            bool generacaso = true;
            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if (idestablecimientotoma == "")
                //pongo por defecto laboratorio central
                idestablecimientotoma = "107093";


            ResultadoxNro.EventoMuestra newmuestra = new ResultadoxNro.EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso = protocolo.IdCasoSISA, // 2061287,
                idMuestra = 272,
                idtipoMuestra = 4,
                muestra = true
            };
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            string DATA = jsonSerializer.Serialize(newmuestra);


            byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

            HttpWebRequest request;
            request = WebRequest.Create(URL) as HttpWebRequest;
            request.Timeout = 10 * 1000;
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.ContentType = "application/json";
            request.Headers.Add("app_key", "b0fd61c3a08917cfd20491b24af6049e");
            request.Headers.Add("app_id", "22891c8f");

            try
            {

                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();


                if (body != "")
                {
                    ResultadoxNro.EventoMuestraResultado respuesta_d = jsonSerializer.Deserialize<ResultadoxNro.EventoMuestraResultado>(body);

                    if (respuesta_d.id != 1)
                    {
                        Item oItem = new Item();
                        oItem = (Item)oItem.Get(typeof(Item), "Codigo", oCon.CodigoCovid, "Baja", false);

                        //string trajomuestra = fila[3].ToString();
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                        crit.Add(Expression.Eq("IdProtocolo", protocolo));
                        crit.Add(Expression.Eq("IdItem", oItem));
                        IList listadetalle = crit.List();
                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {


                            oDetalle.IdeventomuestraSISA = respuesta_d.id;
                            oDetalle.Save();

                            oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + respuesta_d.id.ToString(), oDetalle.IdUsuarioValida);



                        } //for each
                    } //respuesta_o


                }// body

            }


            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

        }


        private void GenerarResultadoSISA(DetalleProtocolo oDetalle)

        {
            bool generacaso = false;
            int ideventomuestra = oDetalle.IdeventomuestraSISA;
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLResultadoSISA;

            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValida);

            string idestablecimientoresultado = oUser.IdEfector.CodigoSISA;
            if (idestablecimientoresultado == "")
                //pongo por defecto laboratorio central
                idestablecimientoresultado = "107093";

            try
            {
                int id_resultado_a_informar = 0;
                int idevento = 307; // sospechoso

                if (oDetalle.IdProtocolo.IdCaracter == 4) idevento = 309; // contacto
                // nose notificò antes y es sospechoso o contacto

                string res = oDetalle.ResultadoCar;


                if (res.Length > 10)
                {
                    if (res.Substring(0, 10) == "SE DETECTA")
                    { id_resultado_a_informar = 3; }
                }
                if (res.Length > 13)
                {
                    if (res.Substring(0, 13) == "NO SE DETECTA")
                    { id_resultado_a_informar = 4; }

                } // if res 

                if (id_resultado_a_informar != 0)
                {
                    string femision = oDetalle.FechaValida.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    string frecepcion = oDetalle.IdProtocolo.Fecha.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                    resultado newresultado = new resultado
                    { // resultado de dni: 31935346
                        derivada = false,
                        fechaEmisionResultado = femision, //"2020-09-14", //
                        fechaRecepcion = frecepcion, // "2020-09-13" 
                        idDerivacion = null, //1125675,//
                        idEstablecimiento =int.Parse( idestablecimientoresultado),  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                        idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                        idEventoMuestra = ideventomuestra,  // 2131682, // sale del excel
                        idPrueba = 1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = 727, // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
                        noApta = true,
                        valor = ""
                    };




                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    string DATA = jsonSerializer.Serialize(newresultado);


                    byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                    HttpWebRequest request;
                    request = WebRequest.Create(URL) as HttpWebRequest;
                    request.Timeout = 10 * 1000;
                    request.Method = "POST";
                    request.ContentLength = data.Length;
                    request.ContentType = "application/json";
                    request.Headers.Add("app_key", "8482d41353ecd747c271f2ec869345e4");
                    request.Headers.Add("app_id", "0e4fcbbf");



                    Stream postStream = request.GetRequestStream();
                    postStream.Write(data, 0, data.Length);

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();
                    if (body != "")
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oDetalle.IdUsuarioValida);
                        generacaso = true;
                    }

                }


            }
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                generacaso = false;

            }


        }


        public class resultado
        {
            public Boolean derivada { get; set; }
            public string fechaEmisionResultado { get; set; }
            public string fechaRecepcion { get; set; }
            public int? idDerivacion { get; set; }
            public int idEstablecimiento { get; set; }
            public int idEvento { get; set; }

            public int idEventoMuestra { get; set; }
            public int idPrueba { get; set; }
            public int idResultado { get; set; }
            public int idTipoPrueba { get; set; }
            public Boolean noApta { get; set; }
            public string valor { get; set; }


        }
        private void Inicializar()
        {
            hypRegresar.NavigateUrl = "ResultadoBExterno.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Carga&modo=" + Request["modo"].ToString();
            if (Request["idItem"] != null)
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
                lblItem.Text = oItem.Codigo + "  -  " + oItem.Nombre;
                //if (oItem.IdTipoResultado == 4)
                //    lblMensaje.Text = "Para ampliar la selección de carga de resultados acceder por Lista de Protocolos";
                //else lblMensaje.Text = "";
            }

            switch (Request["Operacion"].ToString())
            {
                //case "Carga":
                //    {                                                
                //        hypRegresar.NavigateUrl = "ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                //        lblTitulo.Text = "CARGA DE RESULTADOS";
                //        lnkMarcar.Visible = false;
                //        lnkDesmarcar.Visible = false;
                //        btnDesValidar.Visible = false;
                //    }
                //    break;             

                case "Valida":
                    {                        
                        hypRegresar.NavigateUrl = "ResultadoBExterno.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                        lblTitulo.Text = "VALIDACION DE RESULTADOS";
                        lblTitulo.CssClass = "mytituloRojo2";
                        btnGuardar.Text = "Validar";
                        btnValidarPendiente.Visible = false;
                        lnkMarcar.Visible = true;
                        lnkDesmarcar.Visible = true;
                        //btnDesValidar.Visible = true;                                         
                    }
                    break;
               
            }
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {

         //   Marcar(true);

        }

        

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
          //  Marcar(false);
        }

        private void LlenarTabla(string p)
        {
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), int.Parse(p));

            
            bool hiv= oItem.CodificaHiv;

            DataTable dt = getDataSet(oItem.IdItem.ToString());
            if (dt.Rows.Count > 0)
            {
                //lblCantidadRegistros.Text = dt.Rows.Count.ToString() + " protocolos encontrados ";

                TableRow objFila_TITULO = new TableRow();
                TableCell objCellProtocolo_TITULO = new TableCell();
                TableCell objCellFecha_TITULO = new TableCell();
                TableCell objCellPaciente_TITULO = new TableCell();
                TableCell objCellResultado_TITULO = new TableCell();
                TableCell objCellResultadoAnterior_TITULO = new TableCell();

                TableCell objCellReferencia_TITULO = new TableCell();
                TableCell objCellPersona_TITULO = new TableCell(); TableCell objCellValida_TITULO = new TableCell();

                TableCell objCellObservaciones_TITULO = new TableCell();

                Label lblTituloProtocolo = new Label();
                lblTituloProtocolo.Text = "PROTOCOLO";
                objCellProtocolo_TITULO.Controls.Add(lblTituloProtocolo);


                Label lblTituloFecha = new Label();
                lblTituloFecha.Text = "FECHA";
                objCellFecha_TITULO.Controls.Add(lblTituloFecha);

                Label lblTituloPaciente = new Label();
                lblTituloPaciente.Text = "PACIENTE";
                objCellPaciente_TITULO.Controls.Add(lblTituloPaciente);

                Label lblTituloResultado = new Label();
                lblTituloResultado.Text = "RESULTADO";
                objCellResultado_TITULO.Controls.Add(lblTituloResultado);                              
                
                Label lblTituloObservacionesResultado = new Label();
                lblTituloObservacionesResultado.Text = "OBS.";
                objCellObservaciones_TITULO.Controls.Add(lblTituloObservacionesResultado);

                Label lblResultadoAnterior = new Label();
                lblResultadoAnterior.Text = "R.ANTER.";
                objCellResultadoAnterior_TITULO.Controls.Add(lblResultadoAnterior);

                Label lblTituloReferencia = new Label();
                lblTituloReferencia.Text = "REFERENCIA|METODO";
                objCellReferencia_TITULO.Controls.Add(lblTituloReferencia);

                Label lblCargadoPor = new Label();
                lblCargadoPor.Text = "ESTADO";
                objCellPersona_TITULO.Controls.Add(lblCargadoPor);


                Label lblValida = new Label();

                if (Request["Operacion"].ToString() == "Valida")
                        lblValida.Text = "";
                
                objCellValida_TITULO.Controls.Add(lblValida);
                objFila_TITULO.Cells.Add(objCellProtocolo_TITULO);
                objFila_TITULO.Cells.Add(objCellFecha_TITULO);
                objFila_TITULO.Cells.Add(objCellPaciente_TITULO);
                objFila_TITULO.Cells.Add(objCellResultado_TITULO);
                
                if (Request["Operacion"].ToString() == "Valida") objFila_TITULO.Cells.Add(objCellResultadoAnterior_TITULO);
                objFila_TITULO.Cells.Add(objCellReferencia_TITULO);
                objFila_TITULO.Cells.Add(objCellPersona_TITULO);
                

                if (Request["Operacion"].ToString() == "Valida") 
                {                
                    objFila_TITULO.Cells.Add(objCellValida_TITULO);
                }

                objFila_TITULO.Cells.Add(objCellObservaciones_TITULO);
                objFila_TITULO.CssClass = "myLabelIzquierda";
                objFila_TITULO.BackColor = Color.Gainsboro; //.DarkBlue;// "#F2F2FF";

                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                tContenido.Controls.Add(objFila_TITULO);//.Rows.Add(objRow);    

                
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                
                    //decimal m_minimoReferencia = -1;
                    //decimal m_maximoReferencia = -1;
                    string s_valorReferencia = dt.Rows[i].ItemArray[9].ToString();
                    //string s_paciente = dt.Rows[i].ItemArray[15].ToString() + " - " + dt.Rows[i].ItemArray[0].ToString().ToUpper();                    
                    

                    //string s_fecha = dt.Rows[i].ItemArray[1].ToString();
                    
                    string s_idDetalleProtocolo = dt.Rows[i].ItemArray[3].ToString();
                    DetalleProtocolo oDetalle = new DetalleProtocolo();
                    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo),int.Parse( s_idDetalleProtocolo));

                    string s_idProtocolo = oDetalle.IdProtocolo.ToString();
                    string s_fecha = oDetalle.IdProtocolo.Fecha.ToShortDateString();
                   // dbo.NumeroProtocolo(P.idProtocolo) + case when P.numeroOrigen<>'' then '-' +P.numeroOrigen else '' end
                    
                    string s_numero = oDetalle.IdProtocolo.GetNumero() ; // dt.Rows[i].ItemArray[2].ToString();
                    string s_numeroOrigen=oDetalle.IdProtocolo.NumeroOrigen;
                    if ( s_numeroOrigen!="")   s_numero = s_numero+ "-"+ s_numeroOrigen;


                    string numerodocumento ="";
                    string apellido = "";
                    string nombre = "";
                    string s_paciente = "";
                    if (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0)
                    {
                        if (oDetalle.IdProtocolo.IdPaciente.IdEstado == 2) numerodocumento = "(Temporal)";
                        else numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();


                        apellido = oDetalle.IdProtocolo.IdPaciente.Apellido.ToUpper();
                        nombre = oDetalle.IdProtocolo.IdPaciente.Nombre.ToUpper();
                        s_paciente = "";
                  

                    if (hiv) //datosPaciente = " upper(P.sexo+substring(Pac.nombre,1,2)+SUBSTRING(Pac.apellido, 1,2)+REPLACE ( CONVERT(varchar(10), Pac.fechaNacimiento,103),'/','')) ";
                        s_paciente = oDetalle.IdProtocolo.getCodificaHiv(""); // oDetalle.IdProtocolo.IdPaciente.getSexo().Substring(0, 1) + nombre.Substring(0, 2) + apellido.Substring(0, 2) + oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");

                    else
                        if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio == 4)
                            s_paciente= oDetalle.IdProtocolo.getDatosParentesco();
                        else
                        s_paciente = numerodocumento.ToUpper() + " - " + apellido.ToUpper() + " " + nombre.ToUpper();

                    }
                    string s_embarazada = oDetalle.IdProtocolo.GetDiagnostico();
                    string m_formato0 = dt.Rows[i].ItemArray[4].ToString();
                    string m_formato1 = dt.Rows[i].ItemArray[5].ToString();
                    string m_formato2 = dt.Rows[i].ItemArray[6].ToString();
                    string m_formato3 = dt.Rows[i].ItemArray[7].ToString();
                    string m_formato4 = dt.Rows[i].ItemArray[8].ToString();
                    int estado= 0;
                    
                    if (oDetalle.IdUsuarioValida>0)
                        estado=2;

                    if (oDetalle.IdUsuarioPreValida > 0)
                        estado = 2;

                    if (oDetalle.IdUsuarioControl>0)
                        estado=2;
                    //if (dt.Rows[i].ItemArray[9].ToString() != "") m_minimoReferencia = decimal.Parse(dt.Rows[i].ItemArray[9].ToString().Replace(".", ","));
                    //if (dt.Rows[i].ItemArray[10].ToString() != "") m_maximoReferencia = decimal.Parse(dt.Rows[i].ItemArray[10].ToString().Replace(".", ","));
                    string m_observacionReferencia = dt.Rows[i].ItemArray[16].ToString();

                    string m_tipoValorReferencia = dt.Rows[i].ItemArray[12].ToString();
                    string m_metodo = dt.Rows[i].ItemArray[11].ToString();

                    //string s_referencia = dt.Rows[i].ItemArray[10].ToString();
                    string s_resultadoCar = dt.Rows[i].ItemArray[13].ToString();
                    string s_conResultado = dt.Rows[i].ItemArray[14].ToString();

                    if (s_conResultado == "False") { s_conResultado = "0"; }
                    else { s_conResultado = "1"; }


                    string s_Validado = "0";
                    string s_usuario = "";




                    if  (oDetalle.IdUsuarioValida>0)  
                    //if (dt.Rows[i].ItemArray[18].ToString() != "")//  usuario valida                    
                    {
                        Usuario oUser = new Usuario();
                        oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValida);

                        if (oUser.IdEfector == oCon.IdEfector) // falta validar por el externo
                        { s_usuario = "Derivado por : " + dt.Rows[i].ItemArray[18].ToString() + " " + oDetalle.FechaValida.ToShortDateString() + " " + oDetalle.FechaValida.ToShortTimeString();
                            s_Validado = "0";
                        }
                        else
                        {
                            s_usuario = "Val : " + dt.Rows[i].ItemArray[18].ToString() + " " + oDetalle.FechaValida.ToShortDateString() + " " + oDetalle.FechaValida.ToShortTimeString();
                            s_Validado = "2";
                        }
                    }
                    else
                    {
                        if (oDetalle.IdUsuarioPreValida > 0)
                        {
                            Usuario oUser = new Usuario();
                            oUser= (Usuario)oUser.Get(typeof(Usuario),  oDetalle.IdUsuarioPreValida);

                            s_usuario = "PreVal.: " + oUser.FirmaValidacion + " " + oDetalle.FechaPreValida.ToShortDateString() + " " + oDetalle.FechaPreValida.ToShortTimeString();
                            s_Validado = "2";
                        }
                        else
                        {
                            if (oDetalle.IdUsuarioControl > 0)
                            //   if (dt.Rows[i].ItemArray[19].ToString() != "")//  usuario control                    
                            {
                                s_usuario = "Ctrl: " + dt.Rows[i].ItemArray[19].ToString();
                                s_Validado = "1";
                            }
                            else
                            {
                                if (dt.Rows[i].ItemArray[17].ToString() != "")
                                    s_usuario = "Carg.: " + dt.Rows[i].ItemArray[17].ToString() + " " + oDetalle.FechaResultado.ToShortDateString() + " " + oDetalle.FechaResultado.ToShortTimeString();  /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();                                                                                                                                                                                                                              
                                else
                                    s_usuario = "";
                            }
                        }
                    }


                    TableRow objFila = new TableRow();
                    TableCell objCellProtocolo = new TableCell();
                    TableCell objCellFecha = new TableCell();
                    TableCell objCellPaciente = new TableCell();
                    TableCell objCellResultado = new TableCell();
                    TableCell objCellReferencia = new TableCell();
                    TableCell objCellValida = new TableCell();
                    TableCell objCellPersona = new TableCell();
                    TableCell objCellObservaciones = new TableCell();
                    TableCell objCellResultadoAnterior = new TableCell();

                    Label olblProtocolo = new Label();
                    olblProtocolo.Font.Name = "Arial";
                    olblProtocolo.Font.Size = FontUnit.Point(10);
                    olblProtocolo.Text = s_numero;
                    olblProtocolo.Font.Bold = true;
                    //olblProtocolo.ToolTip = "Haga clic aqui para ver mas información del protocolo";
                    //olblProtocolo.Attributes.Add("onClick", "javascript: protocoloView (" + s_idProtocolo + "); return false");
                    objCellProtocolo.BackColor = Color.Beige;
                    objCellProtocolo.Controls.Add(olblProtocolo);

                    Label olblFecha = new Label();                  
                    olblFecha.Text = s_fecha;
                    olblFecha.Font.Name = "Arial";
                    olblFecha.Font.Size = FontUnit.Point(8);
                    objCellFecha.Controls.Add(olblFecha);


                    Label olblPaciente = new Label();
                    olblPaciente.Font.Name = "Arial";
                    olblPaciente.Font.Size = FontUnit.Point(9);
                    olblPaciente.Text = s_paciente;

                    objCellPaciente.Controls.Add(olblPaciente);


                  // diagnsoticos saco por el momento para limpiar la pantalla
                    //if (s_embarazada != "")
                    //{
                    //    Label olblEmbarazo = new Label();
                    //    olblEmbarazo.Font.Name = "Arial";
                    //    olblEmbarazo.Font.Size = FontUnit.Point(7);
                    //    olblEmbarazo.ForeColor = Color.Red;
                    //    olblEmbarazo.Text = "&nbsp;" + s_embarazada;
                    //    objCellPaciente.Controls.Add(olblEmbarazo);
                    //}

                    if (Request["Operacion"].ToString() == "Valida") /// Solo en la validacion
                    {
                        ImageButton btnAddDiagnostico = new ImageButton();
                        btnAddDiagnostico.TabIndex = short.Parse("500");
                        //btnAddDetalle.AutoUpdateAfterCallBack = true;
                        btnAddDiagnostico.ID = "d" + s_idDetalleProtocolo;
                        btnAddDiagnostico.ToolTip = "Agregar/quitar Diagnostico del paciente.";
                        btnAddDiagnostico.ImageUrl = "~/App_Themes/default/images/add.png";
                        //btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                        btnAddDiagnostico.Attributes.Add("onClick", "javascript: editDiagnostico (" + oDetalle.IdProtocolo.IdProtocolo.ToString() + "); return false");
                        //objCellPaciente.Controls.Add(btnAddDiagnostico);
                        objCellProtocolo.Controls.Add(btnAddDiagnostico);
                    }
                    decimal x = 0;



                    switch (oItem.IdTipoResultado)
                    {//tipoResultado

                        case 4://Lista predefinida de resultados con seleccion multiple ( sin seleccion muktiple...jeje).
                            {
                                


                                TextBox txt1 = new TextBox();
                                txt1.ID = s_idDetalleProtocolo;
                                txt1.TabIndex = short.Parse(i + 1.ToString());
                                txt1.Text = s_resultadoCar;
                                txt1.TextMode = TextBoxMode.MultiLine;
                                txt1.Width = Unit.Percentage(95);
                                txt1.Rows = 2;
                                txt1.MaxLength = 200;
                                txt1.ToolTip = s_resultadoCar;
                                //txt1.CssClass = "myTexto";

                                ImageButton btnAddDetalle = new ImageButton();
                                btnAddDetalle.TabIndex = short.Parse("500");
                                //btnAddDetalle.AutoUpdateAfterCallBack = true;
                                btnAddDetalle.ID = "b" + s_idDetalleProtocolo;
                                btnAddDetalle.ToolTip = "Desplegar opciones";
                                btnAddDetalle.ImageUrl = "~/App_Themes/default/images/add.png";
                                //btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                btnAddDetalle.Attributes.Add("onClick", "javascript: PredefinidoSelect (" + oDetalle.IdDetalleProtocolo.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                //btnAddDetalle.Click += new ImageClickEventHandler(btnAddDetalle_Click);

                                if (s_Validado != "0")
                                {
                                    txt1.BackColor = Color.LightBlue;
                                    if (Request["Operacion"].ToString() == "Carga")
                                    {
                                        txt1.Enabled = false;// btnAddDetalle.Enabled = false; 
                                    }
                                }



                                objCellResultado.Controls.Add(txt1);
                                objCellResultado.Controls.Add(btnAddDetalle);

                              

                            } //fin case 4

                            break;

                        case 3://Lista predefinida de resultados
                            {
                                ///   Verifica si la determinacion tiene una lista predeterminada de resultados
                                ISession m_session = NHibernateHttpModule.CurrentSession;
                                ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                                crit.Add(Expression.Eq("IdItem", oItem));
                                crit.Add(Expression.Eq("Baja", false));

                                ///Si tiene resultados predeterminados muestra un combo
                                IList resultados = crit.List();
                                if (resultados.Count > 0)
                                {
                                    DropDownList ddl1 = new DropDownList();
                                    ddl1.ID = s_idDetalleProtocolo;

                                    ListItem ItemSeleccion = new ListItem();
                                    ItemSeleccion.Value = "0";
                                    ItemSeleccion.Text = "";
                                    ddl1.Items.Add(ItemSeleccion);

                             
                                    foreach (ResultadoItem oResultado in resultados)
                                    {
                                        ListItem Item = new ListItem();
                                        Item.Value = oResultado.IdResultadoItem.ToString();
                                        Item.Text = oResultado.Resultado;
                                        ddl1.Items.Add(Item);
                                    }
                                    if (s_conResultado == "0")// sin resultado
                                        ddl1.SelectedValue = oItem.IdResultadoPorDefecto.ToString();
                                    else
                                        ddl1.SelectedItem.Text = s_resultadoCar;

                                    if (s_Validado != "0")
                                    {
                                        ddl1.BackColor = Color.LightBlue;
                                        if (Request["Operacion"].ToString() == "Carga")
                                        { ddl1.Enabled = false; }
                                    }



                                    ddl1.SelectedIndexChanged += new EventHandler(ddl1_SelectedIndexChanged);
                                    objCellResultado.Controls.Add(ddl1);
                                }

                                /////////////////Resultado Anterior
                                if (Request["Operacion"].ToString() == "Valida")
                                {
                                    string resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                    if (resultadoAnterior != "")
                                    {
                                        Label olblResultadoAnterior = new Label();
                                        olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                        //olblResultadoAnterior.Font.Bold = true;
                                        olblResultadoAnterior.ForeColor = Color.Green;
                                        olblResultadoAnterior.Width = Unit.Pixel(20);
                                        olblResultadoAnterior.Text = resultadoAnterior;
                                        olblResultadoAnterior.ToolTip = "Haga clic aquí para ver más datos.";
                                        olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + ",790,400); return false");
                                        
                                        objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);

                                    }
                                }
                                //////////////////////
                                ////Otra forma de observacion
                                ImageButton btnObservacionDetalle2 = new ImageButton();
                                btnObservacionDetalle2.TabIndex = short.Parse("500");

                                btnObservacionDetalle2.ID = "Obs2|" + oDetalle.IdDetalleProtocolo.ToString(); // +"|" + m_estadoObservacion.ToString();//  m_idItem.ToString();

                                if (oDetalle.Observaciones != "")//tiene observaciones
                                {

                                    if (oDetalle.IdUsuarioValidaObservacion == 0)
                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                    else
                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                }
                                else
                                {

                                    btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                }

                                btnObservacionDetalle2.AlternateText = oDetalle.Observaciones;
                                //  btnObservacionDetalle2.ToolTip = "Observaciones para " + lbl1.Text.Replace("&nbsp;", "");
                                btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");

                                objCellObservaciones.Controls.Add(btnObservacionDetalle2);

                                ////////////////////                                        

                            } //fin case 3
                            break;

                        case 1: //numerico
                            {
                                string expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
                                switch (oItem.FormatoDecimal.ToString())
                                {
                                    case "0":
                                        {
                                            expresionControlDecimales = "[-+]?\\d*";
                                            x = decimal.Parse(m_formato0);
                                        }
                                        break;
                                    case "1":
                                        {
                                            expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,1}";
                                            x = decimal.Parse(m_formato1);
                                        } break;
                                    case "2":
                                        {
                                            expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
                                            x = decimal.Parse(m_formato2);
                                        } break;
                                    case "3":
                                        {
                                            expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,3}";
                                            x = decimal.Parse(m_formato3);
                                        } break;
                                    case "4":
                                        {
                                            expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,4}";
                                            x = decimal.Parse(m_formato4);
                                        } break;
                                }

                                TextBox txt1 = new TextBox();
                                txt1.ID = s_idDetalleProtocolo;
                                if (s_conResultado == "0")// sin resultado
                                    txt1.Text = oItem.ResultadoDefecto;
                                else
                                    txt1.Text = x.ToString(System.Globalization.CultureInfo.InvariantCulture);

                                //    txt1.Text = val.Replace(".", "");
                                txt1.Width = Unit.Pixel(60);
                                txt1.CssClass = "myTexto";
                                txt1.Attributes.Add("onkeypress", "javascript:return Enter(this, event)");

                                if (s_Validado != "0")
                                {
                                    txt1.BackColor = Color.LightBlue;
                                    if (Request["Operacion"].ToString() == "Carga")
                                    { txt1.Enabled = false; }
                                }



                                objCellResultado.Controls.Add(txt1);

                                RegularExpressionValidator oValidaNumero = new RegularExpressionValidator();
                                oValidaNumero.ValidationExpression = expresionControlDecimales;
                                oValidaNumero.ControlToValidate = txt1.ID;
                                oValidaNumero.Text = "Formato incorrecto";
                                oValidaNumero.ValidationGroup = "0";

                                objCellResultado.Controls.Add(oValidaNumero);

                                /////////////////Resultado Anterior
                                if (Request["Operacion"].ToString() == "Valida")
                                {
                                    string resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem,true);
                                    if (resultadoAnterior != "")
                                    {
                                        Label olblResultadoAnterior = new Label();
                                        olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                        //olblResultadoAnterior.Font.Bold = true;
                                        olblResultadoAnterior.ForeColor = Color.Green;
                                        olblResultadoAnterior.Width = Unit.Pixel(20);
                                        olblResultadoAnterior.Text = resultadoAnterior;
                                        olblResultadoAnterior.ToolTip = "Haga clic aquí para ver gráfico de evolución.";

                                        olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + ",790,420); return false");                                                                                                              
                                     
                                        objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                   
                                     
                                    }
                                }
                                //////////////////////


                                ////Otra forma de observacion
                                ImageButton btnObservacionDetalle2 = new ImageButton();
                                btnObservacionDetalle2.TabIndex = short.Parse("500");

                                btnObservacionDetalle2.ID = "Obs2|" + oDetalle.IdDetalleProtocolo.ToString();// +"|" + m_estadoObservacion.ToString();//  m_idItem.ToString();

                                if (oDetalle.Observaciones != "")//tiene observaciones
                                {

                                    if (oDetalle.IdUsuarioValidaObservacion == 0)
                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                    else
                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                }
                                else
                                {

                                    btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                }

                                btnObservacionDetalle2.AlternateText = oDetalle.Observaciones;
                              //  btnObservacionDetalle2.ToolTip = "Observaciones para " + lbl1.Text.Replace("&nbsp;", "");
                                btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");

                                objCellObservaciones.Controls.Add(btnObservacionDetalle2);

                                ////////////////////                                        

                                

                            }
                            break;
                        case 2: //texto
                            {
                                TextBox txt1 = new TextBox();
                                txt1.ID = s_idDetalleProtocolo;
                                if (s_conResultado == "0")// sin resultado
                                    txt1.Text = oItem.ResultadoDefecto;
                                else
                                {
                                    if (s_resultadoCar == "")
                                    {
                                        string resNum = oDetalle.ResultadoNum.ToString();
                                        if ((s_resultadoCar == "") && (oDetalle.Enviado == 2) && (oDetalle.IdUsuarioValida == 0) && (oDetalle.IdUsuarioResultado == 0)) // automatico
                                        {
                                            if (resNum != "") txt1.Text = resNum.Substring(0, resNum.Length - 2).Replace(",", ".");
                                        }
                                        else
                                            if (oDetalle.Enviado == 2) { if (oDetalle.Observaciones != "") txt1.Text = oDetalle.Observaciones; }
                                    }
                                    else
                                        txt1.Text = s_resultadoCar;
                                }
                                txt1.TextMode = TextBoxMode.MultiLine;
                                txt1.Width = Unit.Percentage(80);
                                txt1.Rows = 1;
                                txt1.MaxLength = 200;
                                txt1.CssClass = "myTexto";


                                if (s_Validado != "0")
                                {
                                    txt1.BackColor = Color.LightBlue;
                                    if (Request["Operacion"].ToString() == "Carga")
                                    { txt1.Enabled = false; }
                                }

                                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(txt1);

                                objCellResultado.Controls.Add(txt1);


                                /////////////////Resultado Anterior
                                if (Request["Operacion"].ToString() == "Valida")
                                {
                                    string resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                    if (resultadoAnterior != "")
                                    {
                                        Label olblResultadoAnterior = new Label();
                                        olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                        //olblResultadoAnterior.Font.Bold = true;
                                        olblResultadoAnterior.ForeColor = Color.Green;
                                        olblResultadoAnterior.Width = Unit.Pixel(20);
                                        olblResultadoAnterior.Text = resultadoAnterior;
                                        olblResultadoAnterior.ToolTip = "Haga clic aquí para ver más datos.";
                                        olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + ",790,400); return false");
                                   
                                        objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);

                                    }
                                }
                                //////////////////////

                            } // fin case 1
                            break;

                    }//fin swicth




                    Label lblValoresReferencia = new Label();

                    lblValoresReferencia.ID = "VR" + s_idDetalleProtocolo.ToString();
                    lblValoresReferencia.Font.Name = "Arial";
                    lblValoresReferencia.Font.Size = FontUnit.Point(8);
                    lblValoresReferencia.Font.Italic = true;


                    if (s_valorReferencia != "")
                    {
                        lblValoresReferencia.Text = s_valorReferencia;
                        if (m_metodo != "")
                            lblValoresReferencia.Text += " | " + m_metodo;
                    }
                    else
                    {
                        int pres = oDetalle.IdSubItem.GetPresentacionEfector(oDetalle.IdEfector);

                        lblValoresReferencia.Text = oDetalle.CalcularValoresReferencia(pres);
                    }

                 

                    objCellReferencia.Controls.Add(lblValoresReferencia);


                    Label lblPersona = new Label();
                    lblPersona.Text = s_usuario;
                    lblPersona.Font.Size = FontUnit.Point(7);
                    lblPersona.Font.Italic = true;
                    
                    if (s_Validado == "2") { lblPersona.ForeColor = Color.Blue; //VALIDADO
                        if (lblPersona.Text.Substring(0, 3).ToUpper() == "PRE")
                            lblPersona.ForeColor = Color.Red;
                    }
                    if (s_Validado == "1") lblPersona.ForeColor = Color.Green; ///CONTROLADO


                    if (Request["Operacion"].ToString() == "Valida") 
                    {
                        CheckBox chk1 = new CheckBox();
                        chk1.ID = "chk" + s_idDetalleProtocolo;
                        if ((estado == 2) && (Request["Operacion"].ToString() == "Carga")) //si esta validado y entro a controlar no puedo modificar
                        {
                            chk1.Visible = false;

                        }
                        objCellValida.Controls.Add(chk1);
                    }



                    objCellPersona.Controls.Add(lblPersona);
                   

                    ///Definir los anchos de las columnas
                    objCellProtocolo.Width = Unit.Percentage(10);
                    objCellFecha.Width = Unit.Percentage(5);
                    objCellPaciente.Width = Unit.Percentage(35);
                    objCellResultado.Width = Unit.Percentage(20);

                    objCellReferencia.Width = Unit.Percentage(20);
                    objCellPersona.Width = Unit.Percentage(10);



                    ///////////////////////
                    ///agrega a la fila cada una de las celdas

                    objFila.Cells.Add(objCellProtocolo);
                    objFila.Cells.Add(objCellFecha);
                    objFila.Cells.Add(objCellPaciente);
                    objFila.Cells.Add(objCellResultado);
                   
                    if (Request["Operacion"].ToString() == "Valida")  objFila.Cells.Add(objCellResultadoAnterior);                
                    objFila.Cells.Add(objCellReferencia);
                    //objFila.Cells.Add(objCellValoresReferencia);
                    objFila.Cells.Add(objCellPersona); if (Request["Operacion"].ToString() == "Valida") objFila.Cells.Add(objCellValida);
                    //objFila.Cells.Add(objCellValida);

                    objFila.Cells.Add(objCellObservaciones);

                    //////

                    Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                    //'añadimos la fila a la tabla
                    if (objFila != null)
                        tContenido.Controls.Add(objFila);//.Rows.Add(objRow);                                
                }
            }
           

        }


        protected void btnActualizarPracticas_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResultadoItemEditExterno.aspx?idServicio=" + Request["idServicio"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&Operacion=" + Request["Operacion"].ToString(), false);
            //Avanzar(0);
            //SetSelectedTab(TabIndex.DEFAULT);
            ////ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);



        }
        private DataTable getDataSet( string idItem)
        {
            string s_listaProtocolos = Session["Parametros"].ToString();
                              
            string m_strSQL = " SELECT '' as paciente, '' as fecha , " +
                              " DP.idProtocolo AS numero , DP.idDetalleProtocolo, " +
                              " DP.formato0, DP.formato1, DP.formato2, DP.formato3, DP.formato4, " +
                              " DP.ValorReferencia, '' as MaximoReferencia, DP.Metodo as metodo, '' as tipoValorReferencia, DP.resultadoCar, " +
                              " DP.conResultado, '' as numeroDocumento,  '' as observacionReferencia,  DP.userCarga, DP.userValida , DP.userControl " +                              
                              " FROM vta_LAB_Resultados AS DP " +
                              " WHERE  iditem= " + idItem + " and idProtocolo in (" + s_listaProtocolos + ")"+
                              " ORDER BY DP.iddetalleprotocolo" ;
                
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

        void ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl1 = (DropDownList)sender;
        }

        protected void cvValidaControles_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ValidaControles())
                args.IsValid = true;
            else
                args.IsValid = false;
        }

        private bool ValidaControles()
        {
            bool valida = true;
         //   string m_id = "";

       //     Label lbl;
            TextBox txt;
        //    DropDownList ddl;


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
                                                                    if (control5 is TextBox)
                                                                    {
                                                                        txt = (TextBox)control5;
                                                                        if (txt.Enabled)
                                                                        {
                                                                            if (Request["Operacion"].ToString() == "Valida")
                                                                            {
                                                                                if (estaTildado(txt.ID))
                                                                                {
                                                                                    if (txt.ID.Substring(0, 3) != "OBS")
                                                                                        valida = ValidarValor(txt.ID, txt.Text);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (txt.ID.Substring(0, 3) != "OBS") valida = ValidarValor(txt.ID, txt.Text);
                                                                            }
                                                                            if (!valida) { return false; }
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
            return valida;


        }

        private bool ValidarValor(string m_idControl, string valorItem)
        {
           bool control = true;
          
            

            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idControl));

            if ((oDetalle.IdSubItem.Multiplicador > 1) && (valorItem != ""))
            {
              //  valorItem = AplicarMultiplicador(m_idControl, oItem);

                decimal valorActual = Math.Round(oDetalle.ResultadoNum, oDetalle.IdSubItem.FormatoDecimal);
                valorItem = valorActual.ToString(System.Globalization.CultureInfo.InvariantCulture);
                Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(m_idControl);
                TextBox txt = txt = (TextBox)control1;
                if (txt != null)
                {
                    if (txt.Text != valorActual.ToString(System.Globalization.CultureInfo.InvariantCulture))  // si no tiene resultados 
                    {
                        decimal resultadoNumerico = decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture) * oDetalle.IdSubItem.Multiplicador;
                        txt.Text = resultadoNumerico.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        valorItem = txt.Text;
                    }
                }
            }


            if (oDetalle.AnalizarLimites(lblIdValorFueraLimite1.Text))
            {
                if (!oDetalle.IdSubItem.VerificaValoresMinimosMaximos(valorItem))
                {
                    cvValidaControles.ErrorMessage = "Error de valor fuera de límite en protocolo " + oDetalle.IdProtocolo.GetNumero();
                    lblIdValorFueraLimite.Text = oDetalle.IdDetalleProtocolo.ToString();
                    btnAceptarValorFueraLimite.Visible = true;
                    control = false; return control;
                }
            }

            return control;
        }

        protected void btnAceptarValorFueraLimite_Click(object sender, EventArgs e)
        {
            lblIdValorFueraLimite1.Text += "," + lblIdValorFueraLimite.Text;

        }



        protected void btnValidarPendiente_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Guardar(false);
                Response.Redirect("ResultadoItemEditExterno.aspx?idServicio=" + Request["idServicio"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&Operacion=" + Request["Operacion"].ToString()+"&mostrarResumen=1", false);
            }  
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Guardar(true);
                Response.Redirect("ResultadoItemEditExterno.aspx?idServicio=" + Request["idServicio"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&mostrarResumen=1", false);
            }  
        }

        private void Guardar(bool todo)
        {


         //   string m_id = "";

            TextBox txt;
            DropDownList ddl;

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

                                                                    if (control5 is TextBox)
                                                                    {
                                                                        txt = (TextBox)control5;
                                                                        if (txt.Enabled)
                                                                        {

                                                                            if (Request["Operacion"].ToString() == "Valida")
                                                                            {
                                                                                if (estaTildado(txt.ID))
                                                                                {
                                                                                    if (txt.ID.Substring(0, 3) != "OBS") GuardarResultado(txt.ID, txt.Text, todo);
                                                                                }
                                                                            }
                                                                            else{
                                                                                if (txt.ID.Substring(0, 3) != "OBS") GuardarResultado(txt.ID, txt.Text,todo);
                                                                            }
                                                                        }
                                                                    }

                                                                    if (control5 is DropDownList)
                                                                    {
                                                                        ddl = (DropDownList)control5;
                                                                        if (ddl.Enabled)
                                                                        {
                                                                            if (Request["Operacion"].ToString() == "Valida")
                                                                            {
                                                                                if (estaTildado(ddl.ID))
                                                                                {
                                                                                    GuardarResultado(ddl.ID, ddl.SelectedItem.Text, todo);
                                                                                }
                                                                            }
                                                                            else
                                                                                GuardarResultado(ddl.ID, ddl.SelectedItem.Text,todo);
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

         

          
        }

        private bool estaTildado(string m_idItem)
        {

            string nombre_control = "chk" + m_idItem;
            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            CheckBox chk = (CheckBox)control1;
            if (chk != null)
            {
                if (chk.Checked)
                {
                    if (Session["tildados"] == "")
                        Session["tildados"] = m_idItem;
                    else
                        Session["tildados"] += "," + m_idItem;
                    return true;
                }
                else return false;
            }
            else
                return false;
        }



        private void GuardarResultado(string m_idDetalleProtocolo, string valorItem, bool todo)
        {
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            if (!todo)
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdDetalleProtocolo",int.Parse(m_idDetalleProtocolo), "IdUsuarioValida",0);// crit.Add(Expression.Eq("IdUsuarioValida", 0));
            else
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));

            if (oDetalle!=null)
            {
        
                int tiporesultado = oDetalle.IdSubItem.IdTipoResultado;
                switch (tiporesultado)
                        {
                            case 1:// numerico         
                                if (valorItem != "")
                                {
                                    oDetalle.ResultadoNum = decimal.Parse(valorItem, System.Globalization.CultureInfo.InvariantCulture);
                                    oDetalle.FormatoValida = oDetalle.IdSubItem.FormatoDecimal;
                                    oDetalle.ConResultado = true;
                                }
                                else
                                {
                                    oDetalle.ResultadoNum = 0;
                                    oDetalle.ConResultado = false;
                                }
                                break;
                            default:
                                if (valorItem != "")
                                {
                                    oDetalle.ResultadoCar = valorItem;
                                    oDetalle.ConResultado = true;
                                }else
                                {
                                    oDetalle.ResultadoCar = "";
                                    oDetalle.ConResultado = false;
                                }
                                break;
                        }

                string m_metodo = "";
                string m_valorReferencia = "";
                string nombre_control = "VR" + oDetalle.IdDetalleProtocolo.ToString();
                Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
                Label valorRef = (Label)control1;


                if (valorRef != null)
                {
                    string[] arr = valorRef.Text.Split(("|").ToCharArray());
                    switch (arr.Length)
                    {
                        case 1: m_valorReferencia = arr[0].Trim().ToString(); break;
                        case 2:
                            {
                                m_valorReferencia = arr[0].Trim().ToString();
                                m_metodo = arr[1].Trim().ToString();
                            } break;
                    }
                    oDetalle.Metodo = m_metodo;
                    oDetalle.ValorReferencia = m_valorReferencia;
                }
                string operacion = Request["Operacion"].ToString();
                string s_unidadMedida = "";
                int i_unidadMedida = oDetalle.IdSubItem.IdUnidadMedida;
                if (i_unidadMedida > 0)
                {
                    UnidadMedida oUnidad = new UnidadMedida();
                    oUnidad = (UnidadMedida)oUnidad.Get(typeof(UnidadMedida), i_unidadMedida);
                    s_unidadMedida = oUnidad.Nombre;
                }

                oDetalle.UnidadMedida = s_unidadMedida;
                oDetalle.Metodo = m_metodo;
                oDetalle.ValorReferencia = m_valorReferencia;
                if (Request["Operacion"].ToString() == "Carga")
                {
                    if (oDetalle.ConResultado)
                        {
                            oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                            oDetalle.FechaResultado = DateTime.Now;
                        }
                }

                if ((Request["Operacion"].ToString() == "Valida") && (oDetalle.ConResultado))  //Validacion
                {
                    string res = valorItem;
                    if (valorItem.Length > 10)
                        res = valorItem.Substring(0, 10);


                    //if ((oDetalle.IdItem.Codigo == "9122") && (oDetalle.IdProtocolo.IdPaciente.IdPaciente>0) && (res == "SE DETECTA"))
                    //{
                    //    if (oCon.PreValida)
                    //    {
                    //        operacion = "Pre Valida";
                    //        oDetalle.IdUsuarioPreValida = int.Parse(Session["idUsuarioValida"].ToString());
                    //        oDetalle.FechaPreValida = DateTime.Now;
                    //    }
                    //    else
                    //    {
                    //        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                    //        oDetalle.FechaValida = DateTime.Now;
                    //    }

                    //}
                    //else
                    //{
                    if (oDetalle.Informable)
                    {
                        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                        oDetalle.FechaValida = DateTime.Now;
                    }
                    //}


                    if (listavalidado == "") listavalidado = oDetalle.IdDetalleProtocolo.ToString();
                    else listavalidado +=","+ oDetalle.IdDetalleProtocolo.ToString();
                    Session["ListaValidado"] = listavalidado;
                }
                

                oDetalle.Save();


                if (oDetalle.ConResultado)
                {
                    if (Request["Operacion"].ToString() != "Valida")
                        oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuario"].ToString()));
                    else
                    {
                        if (oDetalle.Informable)
                            oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuarioValida"].ToString()));
                    }
                }
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = oDetalle.IdProtocolo;

                if (Request["Operacion"].ToString() != "Valida")
                {
                    if (oProtocolo.Estado == 0)                    
                        oProtocolo.Estado = 1;                        //oProtocolo.Save();                    
                }
                else //Validacion
                {
                    if (oProtocolo.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(Session["idUsuarioValida"].ToString())))
                    {
                        oProtocolo.Estado = 2;  //validado total (cerrado);  

                        if (!oProtocolo.Notificarresultado)
                            oProtocolo.Estado = 3; //Acceso Restringido       
                    }
                    else
                        oProtocolo.Estado = 1;
                }
                oProtocolo.Save();
               
            }



        }

        protected void gvResumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               

                if (e.Row.Cells[1].Text != "&nbsp;") suma1 += int.Parse(e.Row.Cells[1].Text);

                


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "TOTAL PROTOCOLOS";
                e.Row.Cells[1].Text = suma1.ToString();
               

            }
        }

        protected void btnDesValidar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Desvalidar();
                Response.Redirect("ResultadoItemEditExterno.aspx?idServicio=" + Request["idServicio"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&Operacion=" + Request["Operacion"].ToString() , false);
            }
        }

        private void DesValidarResultado(string m_idDetalleProtocolo  )
        {
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdDetalleProtocolo", int.Parse(m_idDetalleProtocolo));// crit.Add(Expression.Eq("IdUsuarioValida", 0));
           

            if (oDetalle != null)
            {
                int usuarioActual = int.Parse(Session["idUsuarioValida"].ToString());           
                if ((oDetalle.IdUsuarioValida == 0) && (oDetalle.IdUsuarioPreValida > 0)) //Desvalida prevalidacion
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Des-PreValida", usuarioActual);
                        oDetalle.IdUsuarioPreValida = 0;                    
                        oDetalle.ResultadoCar = "";
                        oDetalle.ResultadoNum = 0;
                    
                        oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                        oDetalle.Save();
                    }
                if (oDetalle.IdUsuarioValida > 0) // desvalida validacion
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("DesValida", usuarioActual);
                        oDetalle.IdUsuarioValida = 0;
                        oDetalle.ResultadoCar = "";
                        oDetalle.ResultadoNum = 0;
                    oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        oDetalle.Save();
                    }               

            }
        }
        private void Desvalidar( )
        {
            
            TextBox txt;
            DropDownList ddl;

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

                                                                    if (control5 is TextBox)
                                                                    {
                                                                        txt = (TextBox)control5;
                                                                        if (txt.Enabled)
                                                                        {

                                                                            if (Request["Operacion"].ToString() == "Valida")
                                                                            {
                                                                                if (estaTildado(txt.ID))
                                                                                {
                                                                                    DesValidarResultado(txt.ID  );

                                                                                }
                                                                            }
                                                                           
                                                                        }
                                                                    }

                                                                    if (control5 is DropDownList)
                                                                    {
                                                                        ddl = (DropDownList)control5;
                                                                        if (ddl.Enabled)
                                                                        {
                                                                            if ((ddl.SelectedValue != "") && (Request["Operacion"].ToString() == "Valida"))
                                                                            {
                                                                                if (estaTildado(ddl.ID))
                                                                                {

                                                                                    DesValidarResultado(ddl.ID  );
                                                                                    //   GuardarReferenciaMetodoUnidadMedida(ddl.ID, oProtocolo);

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
                }
            }




        }
    }



}
