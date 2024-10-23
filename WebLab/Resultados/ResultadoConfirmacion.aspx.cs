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
using NHibernate;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Drawing;
using Business.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Text;
using System.Net;
using System.IO;

namespace WebLab.Resultados
{
    public partial class ResultadoConfirmacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                CargarPagina();
            }
        }


        private void MostrarResumen()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  select RESULTADOCAR AS RESULTADO , COUNT (*) AS [CANTIDAD TOTAL SIN CONFIRMAR]
from lab_detalleprotocolo a with (nolock)
where idusuariovalida=0 and 
idusuarioprevalida>0
GROUP BY resultadoCar";
            DataSet Ds = new DataSet();
         //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumen.DataSource = Ds.Tables[0];
            gvResumen.DataBind();

            if (Ds.Tables[0].Rows.Count > 0)
                btnValidarPendiente.Visible = true;
            else
                btnValidarPendiente.Visible = false;




        }
        private void MostrarDetalle()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  
select D.iddetalleprotocolo, P.numero, Pa.apellido + '  ' + Pa.nombre  as paciente from lab_Protocolo P with (nolock)
inner join sys_paciente  Pa with (nolock) on Pa.idpaciente=P.idpaciente
inner join lab_detalleprotocolo D with (nolock) on D.idprotocolo= P.idprotocolo
 where  idusuariovalida=0 and 
idusuarioprevalida>0";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();

            if (Ds.Tables[0].Rows.Count > 0)
                btnValidarPendiente.Visible = true;
            else
                btnValidarPendiente.Visible = false;




        }
        private void CargarPagina()
        {

            MarcarSeleccionados(true);

            hplCambiarContrasenia.NavigateUrl = "";

            Usuario oUserValida = new Usuario();
            oUserValida = (Usuario)oUserValida.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
            lblUsuarioValida.Text = "Usuario Autenticado para validar: " + oUserValida.Apellido + " " + oUserValida.Nombre;
            hplCambiarContrasenia.NavigateUrl = "../Usuarios/PasswordEdit.aspx?id=" + oUserValida.IdUsuario.ToString();
            hplCambiarContrasenia.Visible = true;
            divValidacion.Visible = true;
            //////////////////fin ///////////////////////////////////////////

            //VerificaPermisos("Validacion");
            MostrarResumen();
            MostrarDetalle();
            hplCambiarContrasenia.Visible = false;

            MarcarSeleccionados(true);







        }





        private void VerificaPermisos(string sObjeto)
        {
            //if (Request["Operacion"].ToString() == "Valida") { if (Session["idUsuarioValida"] == null) Response.Redirect("../FinSesion.aspx"); }
            //if (Session["idUsuario"] != null)
            //{
            //    if (Session["s_permiso"] != null)
            //    {
            //        Utility oUtil = new Utility();
            //        int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
            //        if (i_permiso==0)  Response.Redirect("../AccesoDenegado.aspx", false); 

            //    }
            //    else Response.Redirect("../FinSesion.aspx", false);
            //}
            //  else Response.Redirect("../FinSesion.aspx", false);
        }















        protected void lnkLimpiar_Click(object sender, EventArgs e)
        {
            Session["Validacion"] = null;
            Session["Resultados"] = null;

            CargarPagina();

        }



        protected void btnValidarPendiente_Click(object sender, EventArgs e)
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            int i = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {

             
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {



                    ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                    string i_id =   gvLista.DataKeys[row.RowIndex].Value.ToString();
                    

                    string ssql_Protocolo = " IdDetalleProtocolo in (Select IdDetalleProtocolo from LAB_DetalleProtocolo where idusuariovalida=0 and idusuarioprevalida>0 and IdDetalleProtocolo="+i_id+" )";
                    crit.Add(Expression.Sql(ssql_Protocolo));
                    IList detalle = crit.List();
                    if (detalle.Count > 0)
                    {
                        foreach (DetalleProtocolo oDetalle in detalle)
                        {
                            oDetalle.IdUsuarioValida = oDetalle.IdUsuarioPreValida;
                            oDetalle.FechaValida = oDetalle.FechaPreValida;
                            oDetalle.Save();
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Confirma Validacion", int.Parse(Session["idUsuarioValida"].ToString()));



                            if (oDetalle.IdProtocolo.ValidadoTotal("Valida", int.Parse(Session["idUsuarioValida"].ToString())))
                            {
                                oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);
                                                                  //oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                            }
                            else
                            {
                                if (oDetalle.IdProtocolo.EnProceso())
                                {
                                    oDetalle.IdProtocolo.Estado = 1;//en proceso
                                                                    // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                                }
                            } // fin   if (oDetalle.IdProtocolo.V
                            oDetalle.IdProtocolo.Save();
                            if (oCon.NotificarSISA  ) // no se suben controles de alta

                            {
                                //if ((oDetalle.IdProtocolo.IdPaciente.IdEstado == 3) && (oDetalle.IdItem.Codigo == "9122"))
                                //{
                                    string res = oDetalle.ResultadoCar;
                                // if (oDetalle.IdProtocolo.VerificarProtocoloAnterior(14))  // controla que no tenga protocolo en los ultimos 14 dias
                                string idItem = oDetalle.IdProtocolo.GenerarCasoSISA(); // se fija si hay algun item que tiene configurado notificacion a sisa
                                if (idItem != "")
                                {

                                        if (res.Length > 10)
                                        {
                                            if (res.Substring(0, 10) == "SE DETECTA")
                                            {
                                                if (ProcesaSISA(oDetalle, "SE DETECTA")) i = i + 1;

                                            }

                                      
                                        } // if res 
                                    }
                                //}//  if ((oDetalle.IdProtocolo.IdCasoS                   
                            }// if (oCon.NotificarSISA)


                            if ((oCon.NotificaAndes) && (oDetalle.IdUsuarioValida>0))

                                GenerarNotificacionAndes(oDetalle);

                        }// fin  foreach (DetalleProtocolo oDetalle in detalle)

                    } // fin   if (detalle.Cou
                } //   if (a.Checked == true)
            } //   foreach (GridViewRow r
            lblTitulo0.Text = "Los resultados se validaron correctamente.";

        }


        private void GenerarNotificacionAndes(DetalleProtocolo oDetalle)
        {


            try
            {




                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                string URL = ConfigurationManager.AppSettings["urlnotifiacionandes"].ToString();
                string s_token = ConfigurationManager.AppSettings["tokennotifiacionandes"].ToString();
                string s_sexo = "";
                switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                {

                    case 2: s_sexo = "femenino"; break;
                    case 3: s_sexo = "masculino"; break;
                }
                string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToString("dd/MM/yyyy");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");
                string fs = oDetalle.IdProtocolo.FechaOrden.ToString("dd/MM/yyyy");
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
                    documento = numerodoc,
                    sexo = s_sexo,
                    fechaNacimiento = DateTime.Parse("01-01-1900"),
                    telefono = oDetalle.IdProtocolo.IdPaciente.InformacionContacto,
                    protocolo = oDetalle.IdProtocolo.Numero.ToString(),
                    resultado = oDetalle.ResultadoCar,
                    fechaSolicitud = DateTime.Parse("01-01-1753"),
                    validador = firma
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
                //string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }


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
                string m_strSQL = @"SELECT  distinct idDetalleProtocolo,  S.idMuestra as IdMuestraSISA,	  S.idTipoMuestra as idTipoMuestraSISA, s.idPrueba as idPruebaSISA, s.idTipoPrueba as idTipoPruebaSISA,  
                ds.idResultadoSISA,S.idEvento
                  FROM    LAB_DetalleProtocolo d with (nolock)
                   inner join LAB_ConfiguracionSISA S with (nolock) on S.idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + @" and s.idItem= d.idSubItem
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
                string idPrueba;
                string idTipoPrueba;
                string idResultadoSISA;
                string idEvento;

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
                        GenerarMuestraSISA(oDetalle.IdProtocolo, idMuestra, idTipoMuestra, idDetalleProtocolo);

                    if (oDetalle.IdeventomuestraSISA > 0)
                        GenerarResultadoSISA(oDetalle, idPrueba, idTipoPrueba, idResultadoSISA, idEvento);

                    break;
                }


            }
            catch (Exception e)
            {
                generacaso = false;


            }
            return generacaso;

        }
        public void GenerarMuestraSISA(Protocolo protocolo, string idMuestraSISA, string idtipoMuestraSISA, string idDetalleProtocolo)

        {
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLMuestraSISA;


            bool generacaso = true;
            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if (idestablecimientotoma == "")
                //pongo por defecto laboratorio central
                idestablecimientotoma = "107093";


            EventoMuestra newmuestra = new EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso = protocolo.IdCasoSISA, // 2061287,
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
                    EventoMuestra respuesta_d = jsonSerializer.Deserialize<EventoMuestra>(body);

                    if (respuesta_d.id != 1)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));

                        if (oDetalle != null)
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
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
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
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
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

    

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            //PintarReferencias();
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            //PintarReferencias();
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
            bool seguir = true;
            string m_strSQL = "";

            try
            {


                // solo se notifican los positivos por confirmacion

                if (res == "SE DETECTA") // se sube todo como sospechoso
                    m_strSQL = " select top 1 * from LAB_ConfiguracionSISA where idCaracter=1 and idItem= " + oDetalle.IdItem.IdItem.ToString();
                else             
                   seguir = false;                   
                

                // no se notificò antes y es sospechoso o contacto


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
                        break;
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
                

            }
            return generacaso;

        }
       
    }

    public class RespuestaCaso
    {
        public string id_caso { get; set; }

        public string resultado { get; set; }
    }

    public class NotificacionPaciente
    { 
        public string  nombre { get; set; }

        public string apellido { get; set; }
        public string documento { get; set; }
        public string sexo { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public string telefono { get; set; }

        public string protocolo { get; set; }


        public string resultado { get; set; }
        public DateTime fechaSolicitud { get; set; }
        public string validador { get; set; }


    }
    public class AltaCaso
    {
        public string usuario { get; set; }
        public string clave { get; set; }
        public evento altaEventoCasoNominal { get; set; }

    }

    public class AltaCasoV2
    {
        public ciudadano ciudadano { get; set; }

        public eventoCasoNominal eventoCasoNominal  { get; set; }

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
    public class ciudadano
    {
        public string apellido { get; set; } // hasta 100
        public string nombre { get; set; } // hasta 100
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }

        public string fechaNacimiento { get; set; }
        public string sexo { get; set; }

        public string paisEmisionTipoDocumento { get; set; }
        public string seDeclaraPuebloIndigena { get; set; }

        public domicilio domicilio { get; set; }
        public string telefono { get; set; } // entre 7 y 50
        public string mail { get; set; }// entre 5 y 100

        public personaACargo personaACargo { get; set; }
    }
    public class domicilio
    {
        public string calle { get; set; } // hasta 200
        public string idDepartamento { get; set; } // hasta 100
        public string idLocalidad { get; set; }
        public string idPais { get; set; }

        public string idProvincia { get; set; }
      
    }
    public class personaACargo
    {
        public string tipoDocumento { get; set; } // hasta 200
        public string numeroDocumento { get; set; } // hasta 100
        public string vinculo { get; set; }
         

    }
    public class eventoCasoNominal
    {
        public string fechaPapel { get; set; }
        public string idGrupoEvento { get; set; }

        public string idEvento { get; set; }
       
      
        public string idClasificacionManualCaso { get; set; }
        public string idEstablecimientoCarga { get; set; }


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
}
    

