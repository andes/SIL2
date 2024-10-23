using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;

using System.Data.SqlClient;

using System.Collections;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Xml;

namespace InterfaceNotificacion
{
    public partial class InterfaceSISA : Form
    {
        public InterfaceSISA()
        {
            InitializeComponent();
            lblCantidad.Text = "Cantidad de Eventos a Notificar: " + Api.Instance.GetCantidadSISAEVENTO();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label10.Text = DateTime.Now.ToLongTimeString();
            lblCantidad.Text = "Cantidad de Eventos a Notificar: " + Api.Instance.GetCantidadSISAEVENTO();
            BuscarProtocolo();
        }

        private void BuscarProtocolo()
        {
            try
            {
                Pendiente pendiente = Api.Instance.GetSISAEVENTO();
                if (pendiente != null)
                {
                    Protocolo protocolo = Api.Instance.GetProtocolo(pendiente.protocolo);

                    int tipo = 0;
                    bool seguir = false;
                    //variables para alta Evento
                    string idevento = "";
                    string idclasificacionmanual = "";
                    string idgrupoevento = "";
                    // variables para api muestra
                    string idMuestraSISA = "";
                    string idtipoMuestraSISA = "";
                    // variables para  api resultado
                    string idPruebaSISA = "";
                    string idTipoPruebaSISA = "";


                    //Levanta configuracion con los ID a informar segun el tipo
                    //1:PCR-Sospechoso - 2: PCR-Casos Especiales - 3: Antigeno Reactivo 4: Antigeno No Reactivo
                    using (DataSet dataset = new DataSet())
                    {
                        dataset.ReadXml("ConfigSISA.xml");
                        DataTable dt = dataset.Tables[0];
                        for (int i = 0; i <= dt.Rows.Count; i++)
                        {
                            tipo = int.Parse(dt.Rows[i][0].ToString());
                            if (tipo == pendiente.tipo)
                            {
                                idevento = dt.Rows[i][1].ToString();
                                idclasificacionmanual = dt.Rows[i][3].ToString();
                                idgrupoevento = dt.Rows[i][5].ToString();

                                idMuestraSISA = dt.Rows[i][7].ToString();
                                idtipoMuestraSISA = dt.Rows[i][8].ToString();

                                idPruebaSISA = dt.Rows[i][9].ToString();
                                idTipoPruebaSISA = dt.Rows[i][10].ToString();

                                seguir = true; break;
                            }
                        }
                    }

                    if (seguir)
                    {
                        string idEventoMuestraDevSISA = "";
                        string idCasoDevSISA = "";

                        if (pendiente.idevento == "")
                        {
                            idCasoDevSISA = GenerarEvento(protocolo.protocolo, protocolo.sexo, protocolo.fecha_nacimiento.ToShortDateString(),
                            protocolo.fecha_pedido.ToShortDateString(), protocolo.numero_documento, idevento, idclasificacionmanual, idgrupoevento);

                            if (idCasoDevSISA=="")
                                Api.Instance.ActualizarEventoPendienteTotal(protocolo.protocolo, "Error");
                            else
                            Api.Instance.ActualizarEventoPendiente(protocolo.protocolo, idCasoDevSISA);
                        }
                        else
                            idCasoDevSISA = pendiente.idevento;

                        // Solo si es PCR se informa muestra y resultado.
                        if (tipo <= 2) // "ANT-REACTIVO"
                        {
                            if (idCasoDevSISA != "")
                            {
                                if (pendiente.ideventomuestra == "")
                                {
                                    string idestablecimientotoma = GetIdEstablecimiento(protocolo.establecimiento_toma_muestra);
                                    idEventoMuestraDevSISA = GenerarMuestraSISA(protocolo.protocolo, protocolo.fecha_toma_muestra.ToShortDateString(),
                                        idestablecimientotoma, int.Parse(idCasoDevSISA), idMuestraSISA, idtipoMuestraSISA);

                                    Api.Instance.ActualizarMuestraPendiente(protocolo.protocolo, idEventoMuestraDevSISA);
                                }
                                else
                                    idEventoMuestraDevSISA = pendiente.ideventomuestra;


                                if (idEventoMuestraDevSISA != "")
                                {
                                    int idResultadoSISA = GetIdResultado(protocolo.resultado);
                                    if (idResultadoSISA > 0)
                                    {
                                        string resultadoDev = GenerarResultadoSISA(int.Parse(idEventoMuestraDevSISA), idPruebaSISA, idTipoPruebaSISA,
                                            idResultadoSISA, idevento, protocolo.fecha_resultado, protocolo.fecha_pedido);

                                        Api.Instance.ActualizarResultadoPendiente(protocolo.protocolo, resultadoDev);
                                    }
                                    else
                                    {
                                        Api.Instance.ActualizarResultadoPendiente(protocolo.protocolo, "Error");
                                        ELog.save(this, protocolo.protocolo + ":Resultado no informable para tipo de evento");
                                    }
                                }

                            }
                        }

                    }
                    else
                        ELog.save(this, protocolo.protocolo + ":No existe configuracion para notificar dicho protocolo");

                }
            }
            catch (Exception ex)
            { ELog.save(this,  ex); }


          
        }

        private int GetIdResultado(string resultado)
        {
            int id = 0;
            if (resultado.ToUpper() == "NO DETECTABLE")
                id= 4;
            if (resultado.ToUpper() == "DETECTABLE")
                id= 3;
            return id;
            
        }

        private string GetIdEstablecimiento(string establecimiento_toma_muestra)
        {
            string est = ConfigurationManager.AppSettings["IdEstablecimientoToma"];
            string[] arrSector = est.Split((";").ToCharArray());
            foreach (string item in arrSector)
            {
                string[] arrEst = item.Split((":").ToCharArray());
                if (arrEst[0].ToString() == establecimiento_toma_muestra)
                    return arrEst[1].ToString();
            }
            return null;
        }


       

        private string GenerarResultadoSISA( int idEventoMuestra, string idPruebaSISA, string idTipoPruebaSISA, int id_resultado_a_informar, string idEventoSISA, DateTime fechaEmision, DateTime fechaRecepcion)
            
        {

            System.Net.ServicePointManager.SecurityProtocol =
              System.Net.SecurityProtocolType.Tls12;
            string resultadoOK = "";
         
            string URL = ConfigurationManager.AppSettings["APIResultado"];
            string s_appKey = ConfigurationManager.AppSettings["app_key_Res"];
            string s_appId = ConfigurationManager.AppSettings["app_id_Res"];
            int idEst = int.Parse(ConfigurationManager.AppSettings["IdEstablecimiento"].ToString());


            try
            {
                
                int idevento = int.Parse(idEventoSISA); //  307; // sospechoso



                if (id_resultado_a_informar != 0)
                {
                    string femision = fechaEmision.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    string frecepcion = fechaRecepcion.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                    resultado newresultado = new resultado
                    { // resultado de dni: 31935346
                        derivada = false,
                        fechaEmisionResultado = femision, //"2020-09-14", //
                        fechaRecepcion = frecepcion, // "2020-09-13" 
                        idDerivacion = null, //1125675,//
                        idEstablecimiento = idEst,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                        idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                        idEventoMuestra = idEventoMuestra,  // 2131682, // sale del excel
                        idPrueba = int.Parse(idPruebaSISA), //1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = int.Parse(idTipoPruebaSISA), // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
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
                    request.Headers.Add("app_key", s_appKey);
                    request.Headers.Add("app_id", s_appId);



                    Stream postStream = request.GetRequestStream();
                    postStream.Write(data, 0, data.Length);

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();
                    if (body != "")
                    {
                        resultadoOK = "OK";
                        //    oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oDetalle.IdUsuarioValida);
                        ELog.save(this, "Se grabó para el idEventomuestra:" + idEventoMuestra + " el resultado ok");
                    }
                    else
                    {

                        resultadoOK = "Error";
                        ELog.save(this, "Error para el idEventomuestra:" + idEventoMuestra + " al grabar resultado");
                    }
                

                }
               

            }
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                ELog.save(this, "Error informando resultado " + mensaje);
                resultadoOK = mensaje;

            }

 return resultadoOK;
        }



        private string GenerarMuestraSISA(string  idprotocolo, string ftoma, string idestablecimientotoma, int idEventoCaso,
            string idMuestraSISA, string idtipoMuestraSISA )

        {


            System.Net.ServicePointManager.SecurityProtocol =
              System.Net.SecurityProtocolType.Tls12;


            string respuesta = "";
            
            string URL = ConfigurationManager.AppSettings["APIMuestra"];
            string s_appKey = ConfigurationManager.AppSettings["app_key_Muestra"];
            string s_appId = ConfigurationManager.AppSettings["app_id_Muestra"];

            
            ftoma= ftoma.Replace("/", "-");

            EventoMuestra newmuestra = new EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso = idEventoCaso, // 2061287,
                idMuestra = int.Parse(idMuestraSISA),
                idtipoMuestra = int.Parse(idtipoMuestraSISA),
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
            request.Headers.Add("app_key", s_appKey);
            request.Headers.Add("app_id", s_appId);

            try
            {

                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();


                if (body != "")
                {
                     EventoMuestra respuesta_d = jsonSerializer.Deserialize<EventoMuestra>(body);

                    if (respuesta_d.id != 1)
                    {
                        respuesta= respuesta_d.id.ToString();
                    } //respuesta_o


                }// body

            }


            catch (WebException ex)
            {
                
                ELog.save(this, ex);
            }
            return respuesta;
        }



        private string GenerarEvento(string idProtocolo, string s_sexo, string fnac, string fn_papel, string numerodocumento, string idevento , string  idclasificacionmanual , 
            string idgrupoevento  )
        {
           
           System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;

           
 string s_idcaso = "";
             
            try
            {
               

                string URL = ConfigurationManager.AppSettings["urlWS400"] ;  
                string s_idestablecimiento = ConfigurationManager.AppSettings["codigoEstablecimiento"]; 
                string usersisa = ConfigurationManager.AppSettings["usuarioSisa"];   
                string[] a = usersisa.Split(':');
                string s_user = a[0].ToString();
                string s_userpass = a[1].ToString();

                //string s_sexo = lblSexo.Text;
                string fn = fnac.Replace("/", "-");

                string fnpapel = fn_papel.Replace("/", "-");

                 

                string error = "";


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
                

                JavaScriptSerializer  jsonSerializer = new JavaScriptSerializer();

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
                         s_idcaso = respuesta_d.id_caso;
                        
                             if (respuesta_d.resultado == "OK")
                            ELog.save(this, idProtocolo+ ":" + "se ha generado un nuevo evento en sisa");
                      
                        else // ERROR_DATOS
                            ELog.save(this, idProtocolo+ ":" + respuesta_d.description);

                        if (s_idcaso == null)
                            s_idcaso = "";



                    }
                    else
                    {
                        error = respuesta_d.resultado + ": " + respuesta_d.description;
                        ELog.save(this, idProtocolo + ":" + error);

                    }
                }

               


            }
            catch (WebException ex)
            {
                ELog.save(this, ex);

            }

 return s_idcaso;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            timer1.Interval = 50000; // 5 segundos
            timer1.Start();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
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

public class RespuestaCaso
{
    public string id_caso { get; set; }

    public string resultado { get; set; }

    public string description { get; set; }
}
public class evento
{
    public string idTipodoc { get; set; }
    public string nrodoc { get; set; }
    public string sexo { get; set; }
    public string fechaNacimiento { get; set; }
    public string idGrupoEvento { get; set; }

    public string idEvento { get; set; }
    public string idEstablecimientoCarga { get; set; }
    public string fechaPapel { get; set; }
    public string idClasificacionManualCaso { get; set; }


}

public class AltaCaso
{
    public string usuario { get; set; }
    public string clave { get; set; }
    public evento altaEventoCasoNominal { get; set; }

}


public class EventoMuestra
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