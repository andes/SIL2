using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.Configuration;
using Business.Data;

namespace WebLab.Resultados
{
    public partial class ResultadoxNro : System.Web.UI.Page
    {
        string listavalidado = "";
        //   string resultado = "";
        DataTable dtDeterminaciones; //tabla para determinaciones
        int fila = 0;

      
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        { 
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
                 VerificaPermisos("Validacion");
                txtNumero.Attributes.Add("onkeypress", "return clickButton(event,'" + btnAgregar.ClientID + "')");
                txtNumero.Focus();
                //Page.ClientScript.RegisterStartupScript(this, typeof(this.Page),  "ScriptDoFocus",   SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),  true);
                CargarListas();
                InicializarTablas();
                if (Request["mostrarResumen"] != null)
                {
                    if (Session["ListaValidado"] != null)
                    {
                        MostrarResumen();
                        MostrarDetalle();
                        ddlResultado.Enabled = false;
                        txtNumero.Enabled = false;
                        btnAgregar.Enabled = false;
                        btnValidar.Enabled = false;
                        btnComenzar.Visible = true;
                        gvLista.Visible = false;
                        fila = 0;
                    }
                }
                                

            }


        }
       
       

        private void MostrarDetalle()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  
select P.numero as Protocolo, Pa.apellido + '  ' + Pa.nombre  as Paciente, resultadoCar as [Resultado Informado],
case when D.idusuariovalida>0 then 'Val: ' + U.firmaValidacion else  
case when D.idusuarioprevalida>0 then 'PreVal: ' + U1.firmaValidacion else 
    case when D.idusuarioresultado>0 then 'Cargado: ' + U2.apellido + ' ' + U2.nombre else  ''
 end   

 end   
end as Usuario
from lab_Protocolo P with (nolock)
inner join sys_paciente  Pa with (nolock) on Pa.idpaciente=P.idpaciente
inner join lab_detalleprotocolo D with (nolock) on D.idprotocolo= P.idprotocolo
left join sys_usuario U with (nolock) on U.idusuario= D.idusuariovalida
left join sys_usuario U1 with (nolock) on U1.idusuario= D.idusuarioprevalida
left join sys_usuario U2 with (nolock) on U2.idusuario= D.idusuarioresultado
 where  D.idDetalleProtocolo in (" + Session["ListaValidado"].ToString() + ")  order by P.numero ";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumeDetalle.DataSource = Ds.Tables[0];
            gvResumeDetalle.DataBind();

            if (Ds.Tables[0].Rows.Count > 0)
            {
                gvResumeDetalle.Visible = true;
               
            }
            else
                gvResumeDetalle.Visible = false;

            


        }
        private void MostrarResumen()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  select  resultadocar as Resultado, count (*) as Cantidad
   from LAB_DetalleProtocolo with (nolock) where idDetalleProtocolo in (" + Session["ListaValidado"].ToString() + ") group by resultadocar ";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumen.DataSource = Ds.Tables[0];
            gvResumen.DataBind();
            gvResumen.Visible = true;

            if (Request["Operacion"] == "Carga")
                lblProcesado.Text = "Registrado como cargado en este paso:";
            else
                lblProcesado.Text = "Registrado como validado en este paso:";
            lblProcesado.Visible = true;



            // sisa
            //lblMensajeSISA.Visible = false;
        
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

                    // fin de llenar
                    if ((oDetalle.IdUsuarioValida > 0) && (oDetalle.IdProtocolo.Notificarresultado))// solo si està validado
                    { 
                        if ((oCon.NotificaAndes) && (oDetalle.IdItem.Codigo == oCon.CodigoCovid))
                        {
                       
                            GenerarNotificacionAndes(oDetalle);

                        }

                        if (oCon.NotificarSISA) 

                        {
                            //if (oDetalle.IdProtocolo.IdCaracter != 2) // no se suben controles de alta
                            //{
                                //if ((oDetalle.IdProtocolo.IdPaciente.IdEstado != 2) ) // 
                                //{
                                    string res = oDetalle.ResultadoCar;

                                    //if (oDetalle.IdProtocolo.VerificarProtocoloAnterior(14))
                                    //{
                                    string idItem = oDetalle.IdProtocolo.GenerarCasoSISA(); // se fija si hay algun item que tiene configurado notificacion a sisa
                                    if (idItem != "")
                                    //if (oDetalle.IdSubItem.Codigo==oCon.CodigoCovid)
                                    {
                                        if (res.Length > 10)
                                        {
                                            if ((res.Substring(0, 10) == "SE DETECTA"))
                                            { if (ProcesaSISA(oDetalle, "SE DETECTA")) i = i + 1; }
                                        }
                                        if (res.Length > 13)
                                        {
                                            if (res.Substring(0, 13) == "NO SE DETECTA")
                                            { if (ProcesaSISA(oDetalle, "NO SE DETECTA")) i = i + 1; }
                                        }
                                    }
                                    //}// oDetalle.IdProtocolo.VerificarPr

                                  //}//  if ((oDetalle.IdProtocolo.IdPacie
                        //}// if (oDetalle.IdProtocolo.IdCaracter != 2
                         }//    if (oCon.NotificarSISA)
                }//                foreach (DetalleProtoco
                }// si esta validado
                 
            }//     if (detalle.Count > 0)




        }

        private void GenerarNotificacionAndes(DetalleProtocolo oDetalle)
        {


            try
            {

                 
                string URL = ConfigurationManager.AppSettings["urlnotifiacionandes"].ToString();
                string s_token = ConfigurationManager.AppSettings["tokennotifiacionandes"].ToString();
                string s_sexo = "";
                switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                {

                    case 2: s_sexo = "femenino"; break;
                    case 3: s_sexo = "masculino"; break;
                }
                string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToString("dd/MM/yyyy");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");
                string fs= oDetalle.IdProtocolo.FechaOrden.ToString("dd/MM/yyyy");
                string numerodoc = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                if (oDetalle.IdProtocolo.IdPaciente.IdEstado == 2) // temporal
                    numerodoc = "0";

                string error = "";
                string firma = "";
                Usuario oUs = new Usuario(); oUs = (Usuario)oUs.Get(typeof(Usuario), oDetalle.IdUsuarioValida);
                if (oUs != null)

                    firma = oUs.FirmaValidacion;

                NotificacionPaciente newevento = new NotificacionPaciente
                {
                    nombre = oDetalle.IdProtocolo.IdPaciente.Nombre,
                    apellido = oDetalle.IdProtocolo.IdPaciente.Apellido,
                    documento =numerodoc,
                    sexo = s_sexo,
                    fechaNacimiento = DateTime.Parse("01-01-1900"),
                    telefono = oDetalle.IdProtocolo.IdPaciente.InformacionContacto,
                    protocolo = oDetalle.IdProtocolo.Numero.ToString(),
                    resultado = oDetalle.ResultadoCar,
                    fechaSolicitud = DateTime.Parse("01-01-1753"),
                    validador= firma
                };

                const string Comillas = "\"";

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                string DATA = jsonSerializer.Serialize(newevento);
                DATA = DATA.Replace("\"\\/Date(", "").Replace(")\\/\"", "").Replace("-2208978000000", "&&");

                DATA = DATA.Replace("&&", Comillas + fn + Comillas);


                DATA = DATA.Replace("\"\\/Date(", "").Replace(")\\/\"", "").Replace("-6847794000000", "||");

                DATA = DATA.Replace("||", Comillas + fs + Comillas);
                



                byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                HttpWebRequest request;
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.Timeout = 10 * 1000;
                request.Method = "POST";
                request.ContentLength = data.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", s_token);




                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();
                if (body != "")
                {


                    oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Notificacion Andes ", oDetalle.IdUsuarioValida);

                }



            }
            catch (Exception e)
            {
               // string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }


        }
        private bool ProcesaSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;

            try
            {
                if (oDetalle.IdProtocolo.IdCasoSISA == 0)
                          generacaso = GenerarCasoSISA(oDetalle, res);                      
                

                string m_strSQL = @"SELECT  distinct idDetalleProtocolo,  S.idMuestra as IdMuestraSISA,	  S.idTipoMuestra as idTipoMuestraSISA, s.idPrueba as idPruebaSISA, s.idTipoPrueba as idTipoPruebaSISA,  
                ds.idResultadoSISA,S.idEvento
                  FROM    LAB_DetalleProtocolo d with (nolock)
                   inner join LAB_ConfiguracionSISA S with (nolock) on S.idCaracter=" +  oDetalle.IdProtocolo.IdCaracter.ToString()+ @" and s.idItem= d.idSubItem
                   inner join LAB_ConfiguracionSISADetalle DS with (nolock) on DS.idItem=d.idSubItem  and resultadocar= ds.resultado
                    where d.idProtocolo= " + oDetalle.IdProtocolo.IdProtocolo.ToString();



                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                string idDetalleProtocolo;
                string idMuestra;
                string idTipoMuestra;
                string  idPrueba ;
                string idTipoPrueba;
                string idResultadoSISA ;
                string  idEvento ;
 
                DataTable dt = Ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    idDetalleProtocolo = dt.Rows[i][0].ToString();
                    idMuestra = dt.Rows[i][1].ToString();
                    idTipoMuestra = dt.Rows[i][2].ToString();
                    idPrueba = dt.Rows[i][3].ToString();
                    idTipoPrueba = dt.Rows[i][4].ToString();
                    idResultadoSISA = dt.Rows[i][5].ToString();
                    idEvento = dt.Rows[i][6].ToString();


                    if ((oDetalle.IdProtocolo.IdCasoSISA > 0) && (oDetalle.IdeventomuestraSISA == 0))
                        GenerarMuestraSISA(oDetalle.IdProtocolo,  idMuestra ,  idTipoMuestra, idDetalleProtocolo);

                    if (oDetalle.IdeventomuestraSISA > 0)
                        GenerarResultadoSISA(oDetalle,    idPrueba  , idTipoPrueba , idResultadoSISA,  idEvento );

                    break;
                }

            }
            catch (Exception e)
            {
                generacaso = false;
               

            }
            return generacaso;

        }

        private bool GenerarCasoSISA(DetalleProtocolo oDetalle, string res)
        {
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;
            bool generacaso = false;
            string caracter = "";
            string idevento = "";
            string nombreevento = "";
            string idclasificacionmanual = "";
            string nombreclasificacionmanual = "";
            string idgrupoevento = "";
            string nombregrupoevento = "";
            bool seguir = false;
            string m_strSQL = "";
         //   Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            try
            {

                m_strSQL = " select * from LAB_ConfiguracionSISA with (nolock) where idCaracter=  " + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem=" + oDetalle.IdSubItem.IdItem.ToString();
                if ((res == "SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter != 2) && (oCon.CodigoCovid == oDetalle.IdSubItem.Codigo))
                {
                    /// si es positivo y no es controlo de alta se genera un caso sospechoso solo para covid

                    m_strSQL = " select * from LAB_ConfiguracionSISA with (nolock) where idCaracter=1  and idItem=" + oDetalle.IdSubItem.IdItem.ToString();
                }   
               
               


                // nose notificò antes y es sospechoso o contacto



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
                        seguir = true;
                        break;
                    }

                if (seguir)
                {
                  
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

                        if (respuesta_d.id_caso != "")
                        { //  devolver el idcaso para guardar en la base de datos
                            string s_idcaso = respuesta_d.id_caso;

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            if (respuesta_d.resultado == "OK")
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, oDetalle.IdUsuarioValida);
                            else // ERROR_DATOS
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, oDetalle.IdUsuarioValida);                            

                            
                    



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
        public class EventoMuestra
        {
            public Boolean adecuada { get; set; }
            public Boolean aislamiento { get; set; }
            public string fechaToma { get; set; }
       //     public int id { get; set; }
            public int idEstablecimientoToma { get; set; }

            public int idEventoCaso { get; set; }
            public int idMuestra { get; set; }
            public int idtipoMuestra { get; set; }
            public Boolean muestra { get; set; }


        }


        public class EventoMuestraResultado
        {
            public Boolean adecuada { get; set; }
            public Boolean aislamiento { get; set; }
            public string fechaToma { get; set; }
            public int id { get; set; }
            public int idEstablecimientoToma { get; set; }

            public int idEventoCaso { get; set; }
            public int idMuestra { get; set; }
            public int idtipoMuestra { get; set; }
            public Boolean muestra { get; set; }


        }
        public void GenerarMuestraSISA(Protocolo protocolo, string idMuestraSISA, string idtipoMuestraSISA, string idDetalleProtocolo)

        {
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

           
            string URL = oCon.URLMuestraSISA;


            bool generacaso = true;
            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if ((idestablecimientotoma == "")|| (idestablecimientotoma == "0"))
                //pongo por defecto laboratorio central
                idestablecimientotoma = "107093";


            EventoMuestra newmuestra = new EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso =   protocolo.IdCasoSISA, // 2061287,
                idMuestra = int.Parse(idMuestraSISA),
                idtipoMuestra =int.Parse( idtipoMuestraSISA),
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
                    EventoMuestraResultado respuesta_d = jsonSerializer.Deserialize<EventoMuestraResultado>(body);

                    if (respuesta_d.id != 1)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));
 
                        if (oDetalle!= null)
                        { 

                            oDetalle.IdeventomuestraSISA = respuesta_d.id;
                            oDetalle.Save();

                            oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + respuesta_d.id.ToString(), oDetalle.IdUsuarioValida);
                     


                        } //if
                    } //respuesta_o


                }// body

            }


            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

        }


        private void GenerarResultadoSISA(DetalleProtocolo oDetalle, string idPruebaSISA, string idTipoPruebaSISA, string idResultadoSISA, string idEventoSISA)

        {
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;


            int ideventomuestra = oDetalle.IdeventomuestraSISA;
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLResultadoSISA;


            try
            {
                int id_resultado_a_informar = int.Parse(idResultadoSISA); // 0;
                int idevento = int.Parse(idEventoSISA); //  307; // sospechoso

             

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
                        idEstablecimiento = 107093,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                        idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                        idEventoMuestra = ideventomuestra,  // 2131682, // sale del excel
                        idPrueba = int.Parse(idPruebaSISA), //1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado =   id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = int.Parse(idTipoPruebaSISA), //727, // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
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
                        
                    }

                }


            }
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
              
 
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
        private void CargarListas()
        {
            Usuario oUser = new Usuario();

            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            Item oItem = new Item();

            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
            if (oItem != null)
            {
                lblDeterminacion.Text = oItem.Nombre;
                lblCodigo.Text = oItem.Codigo;
                hiditem.Value = oItem.IdItem.ToString();
                Utility oUtil = new Utility();
                string m_ssql = @"select   resultado from LAB_ResultadoItem with (nolock) where iditem =" + oItem.IdItem.ToString()+ " and idEfector= "+oUser.IdEfector.IdEfector.ToString() +" order by resultado";

                oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado");
                ddlResultado.Items.Insert(0, new ListItem("--Seleccione--", "0"));
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
                    case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void Agregar(DetalleProtocolo oRegistro)
        {
            ///Agregar a la tabla lOS PROTOCOLOS para mostrarlas en el gridview
            try { 
            bool existe = false;
                if (ddlResultado.SelectedValue != "0")
                {
                    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                    fila = (int)(Session["orden"]);
                    //Primero verifica que no exista el item en la lista
                    for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                    {
                        if (txtNumero.Text.Trim() == dtDeterminaciones.Rows[i][1].ToString())
                            existe = true;
                    }
                    if (!existe)
                    {
                        Session["orden"] = int.Parse(Session["orden"].ToString()) + 1;
                        //fila  = fila+1;

                        DataRow row = dtDeterminaciones.NewRow();
                        row[0] = oRegistro.IdDetalleProtocolo.ToString();
                        row[1] = oRegistro.IdProtocolo.Numero.ToString();
                        row[2] = oRegistro.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                        row[3] = oRegistro.IdProtocolo.IdPaciente.Apellido + " " + oRegistro.IdProtocolo.IdPaciente.Nombre;
                        row[4] = ddlResultado.SelectedItem.Text;
                        row[5] = oRegistro.ResultadoCar;
                        row[6] = "";
                        row[7] = Session["orden"].ToString();
                        dtDeterminaciones.Rows.Add(row);

                      
                        dtDeterminaciones.DefaultView.Sort = " orden desc";
                        dtDeterminaciones = dtDeterminaciones.DefaultView.ToTable();

                        Session.Add("Tabla1", dtDeterminaciones);
                        gvLista.DataSource = dtDeterminaciones;
                        gvLista.DataBind();
                        gvLista.Visible = true;
                        lblError.Text = "";
                        lblCantidad.Text = dtDeterminaciones.Rows.Count.ToString()  + " protocolos agregados";
                    }
                    else
                    {
                        lblError.Text = "El protocolo ya fue ingresado a la lista";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                   btnAgregar.UpdateAfterCallBack = true;
                    gvLista.UpdateAfterCallBack = true;

                    lblCantidad.UpdateAfterCallBack = true;
                    txtNumero.Text = ""; lblError.UpdateAfterCallBack = true;
                    txtNumero.Focus();
                    txtNumero.UpdateAfterCallBack = true;
                   

              

            }
                else
                {
                    lblError.Text = "DEBE SELECCIONAR UN RESULTADO A INFORMAR ";
                    lblError.UpdateAfterCallBack = true;
                }
                  }
                catch (Exception e)
                { }

        }

 

 
      
  
  
      

     

     

     


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["Tabla1"] != null)
                {
                    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);


                    for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                    {
                        string m_idDetalleProtocolo = dtDeterminaciones.Rows[i][0].ToString();
                        string valorItem = dtDeterminaciones.Rows[i][4].ToString();
                        GuardarResultado(m_idDetalleProtocolo, valorItem, true);

                    }
                    Response.Redirect("ResultadoxNro.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Valida&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&mostrarResumen=1", false);
                }
            }


        }

        private void GuardarResultado(string m_idDetalleProtocolo, string valorItem, bool valida )
        {

           
            DetalleProtocolo oDetalle = new DetalleProtocolo();
           
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));

            if (oDetalle != null)
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
                        }
                        else
                        {
                            oDetalle.ResultadoCar = "";
                            oDetalle.ConResultado = false;
                        }
                        break;
                }

                int pres = oDetalle.IdSubItem.GetPresentacionEfector(oDetalle.IdEfector);

                string valorRef = oDetalle.CalcularValoresReferencia(pres);
                string m_metodo = "";
                string m_valorReferencia = "";
                

                if (valorRef != null)
                {
                    string[] arr = valorRef.Split(("|").ToCharArray());
                    switch (arr.Length)
                    {
                        case 1: m_valorReferencia = arr[0].Trim().ToString(); break;
                        case 2:
                            {
                                m_valorReferencia = arr[0].Trim().ToString();
                                m_metodo = arr[1].Trim().ToString();
                            }
                            break;
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
                //oDetalle.Metodo = m_metodo;
                //oDetalle.ValorReferencia = m_valorReferencia;
                bool grabar = true;
                string s_operacion = "Valida";
                if (!valida) s_operacion = "Carga";

                if (s_operacion == "Carga")  // puede cargar mientras no tenga un resultado validado.
                {if (oDetalle.IdUsuarioValida == 0)
                    {
                        if (oDetalle.ConResultado)
                        {
                            oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                            oDetalle.FechaResultado = DateTime.Now;
                        }
                        if (listavalidado == "") listavalidado = oDetalle.IdDetalleProtocolo.ToString();
                        else listavalidado += "," + oDetalle.IdDetalleProtocolo.ToString();
                        Session["ListaValidado"] = listavalidado;

                    }
                    else grabar = false;
                }

                if ((s_operacion == "Valida") && (oDetalle.ConResultado))  //Validacion
                {
                    string res = valorItem;
                    if (valorItem.Length > 10)
                        res = valorItem.Substring(0, 10);


                    if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0) && (res == "SE DETECTA"))
                    {
                        if (oCon.PreValida)
                        {
                            s_operacion = "Pre Valida";
                            operacion = "Pre Valida";
                            oDetalle.IdUsuarioPreValida = int.Parse(Session["idUsuarioValida"].ToString());
                            oDetalle.FechaPreValida = DateTime.Now;
                            oDetalle.IdUsuarioValida = 0;
                            oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        }
                        else
                        {
                            oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                            oDetalle.FechaValida = DateTime.Now;
                        }

                    }
                    else
                    {
                        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                        oDetalle.FechaValida = DateTime.Now;
                    }


                    if (listavalidado == "") listavalidado = oDetalle.IdDetalleProtocolo.ToString();
                    else listavalidado += "," + oDetalle.IdDetalleProtocolo.ToString();
                    Session["ListaValidado"] = listavalidado;
                }

                if (grabar)
                {
                    oDetalle.Save();


                    if (oDetalle.ConResultado)
                    
                            oDetalle.GrabarAuditoriaDetalleProtocolo(s_operacion, int.Parse(Session["idUsuarioValida"].ToString()));
                     
                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = oDetalle.IdProtocolo;

                    if (s_operacion != "Valida")
                    {
                        if (oProtocolo.Estado == 0)
                            oProtocolo.Estado = 1;                        //oProtocolo.Save();                    
                    }
                    else //Validacion
                    {
                        if (oProtocolo.ValidadoTotal(s_operacion, int.Parse(Session["idUsuarioValida"].ToString())))
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



        }








        private void InicializarTablas()
        {
            ///Inicializa las sesiones para las tablas de diagnosticos y de determinaciones
            if (Session["Tabla1"] != null)
            {
                Session["Tabla1"] = null;
                Session["orden"] = null;
            }

          
            //if (Session["Tabla2"] != null) Session["Tabla2"] = null;

            dtDeterminaciones = new DataTable();


            dtDeterminaciones.Columns.Add("idDetalleProtocolo"); 
            dtDeterminaciones.Columns.Add("numero");
            dtDeterminaciones.Columns.Add("dni");
            dtDeterminaciones.Columns.Add("paciente");
            dtDeterminaciones.Columns.Add("resultado"); // resultado nuevo
            dtDeterminaciones.Columns.Add("resultadoAnt"); // valor anterior a reemplazar
            dtDeterminaciones.Columns.Add("eliminar");
            dtDeterminaciones.Columns.Add("orden");


            Session.Add("Tabla1", dtDeterminaciones);
            Session.Add("orden", 0);


        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            VerificayAgrega();

            txtNumero.Focus();
            txtNumero.AutoUpdateAfterCallBack = true;


        }

        private void VerificayAgrega()
        {
            try
            {
                //Usuario oUser = new Usuario();
                //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));


                Protocolo oRegistro = new Protocolo();
                oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), "Numero", int.Parse(txtNumero.Text), "IdEfector",oUser.IdEfector );

                Item oItem = new Item();

                oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));

                if (oRegistro != null)
                {
                    DetalleProtocolo oDProtocolo = new DetalleProtocolo();
                    oDProtocolo = (DetalleProtocolo)oDProtocolo.Get(typeof(DetalleProtocolo), "IdProtocolo", oRegistro, "IdSubItem", oItem);

                  
                    string res = oDProtocolo.ResultadoCar;

                    Agregar(oDProtocolo);

                    if (res.Length > 10)
                    {
                        if (res.Substring(0, 10) == "SE DETECTA")
                        {
                            if (res != ddlResultado.SelectedValue)
                            {
                                lblError.Text = "Esta pisando un resultado positivo. Está seguro de cambiarlo?. Haga clic en Habilitar Ingreso para aceptarlo o corregirlo.";
                                lblError.Visible = true;
                                lblError.UpdateAfterCallBack = true;
                                btnAgregar.Visible = false;
                                txtNumero.Enabled = false;

                                btnAgregar.UpdateAfterCallBack = true;
                                txtNumero.UpdateAfterCallBack = true;
                                btnHabilitar.Visible = true;
                                btnHabilitar.UpdateAfterCallBack = true;
                            }

    }


                     }

                   
                }
                else
                    lblError.Text = "Numero no encontrado";
                lblError.UpdateAfterCallBack = true;
            }
            catch
            {
                lblError.Text = "Numero no encontrado";
                lblError.UpdateAfterCallBack = true;
            }

        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    if (dtDeterminaciones.Rows[i][0].ToString() == e.CommandArgument.ToString())
                        dtDeterminaciones.Rows[i].Delete();
                }


                dtDeterminaciones.DefaultView.Sort = " orden desc";
                dtDeterminaciones = dtDeterminaciones.DefaultView.ToTable();

                Session.Add("Tabla1", dtDeterminaciones);
                gvLista.DataSource = dtDeterminaciones;
                gvLista.DataBind();
                gvLista.UpdateAfterCallBack = true;

                lblCantidad.Text = dtDeterminaciones.Rows.Count.ToString() + " protocolos agregados";
                lblCantidad.UpdateAfterCallBack = true;
                //Session.Add("Tabla1", dtDeterminaciones);
                //gvLista.DataSource = dtDeterminaciones;
                //gvLista.DataBind();


            }
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[5].Controls[1];
                CmdEliminar.CommandArgument = dtDeterminaciones.Rows[e.Row.RowIndex][0].ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

              

            }
        }

        protected void txtNumero_TextChanged(object sender, EventArgs e)
        {
            //if (txtNumero.Text.Length>=5)
            //{
            //    VerificayAgrega();
            //}
        }

        protected void btnComenzar_Click(object sender, EventArgs e)
        {
            gvResumeDetalle.Visible = false;
            ddlResultado.Enabled = true;
            gvResumen.Visible = false;
            lblProcesado.Visible = false;
            gvLista.Visible = true;
            
            txtNumero.Enabled = true;
            btnAgregar.Enabled = true;
            btnValidar.Enabled = true;
        }

        protected void btnHabilitar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";  lblError.Visible = true;
            lblError.UpdateAfterCallBack = true;
            btnAgregar.Visible = true;
            txtNumero.Enabled = true;
                                btnAgregar.UpdateAfterCallBack = true;
            txtNumero.UpdateAfterCallBack = true;
            btnHabilitar.Visible = false;
            btnHabilitar.UpdateAfterCallBack = true;
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["Tabla1"] != null)
                {
                    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);


                    for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                    {
                        string m_idDetalleProtocolo = dtDeterminaciones.Rows[i][0].ToString();
                        string valorItem = dtDeterminaciones.Rows[i][4].ToString();
                        GuardarResultado(m_idDetalleProtocolo, valorItem, false);

                    }
                    Response.Redirect("ResultadoxNro.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Carga&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&mostrarResumen=1", false);
                }
            }


        }
    }
}