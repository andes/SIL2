﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Business.Data;
using NHibernate;
using Business.Data.Laboratorio; 
using NHibernate.Expression;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Xml;
using System.ServiceModel;
using System.Net.Mail;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace WebLab.Protocolos
{
    public partial class ProcesaRenaperT : System.Web.UI.Page
    {
        
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);


            if (Request["master"] == null)
                Page.MasterPageFile = "~/Site1.master";
            else
                Page.MasterPageFile = "~/Site2.master";

            if (Request["llamada"] != null)
            {
                if (Request["llamada"] == "LaboEfector")


                    this.MasterPageFile = "~/Resultados/SitePE.master";
            }



        }

        


    
//        private bool  ConectarRenaperXRoad ()
//        {
//            bool ok = false;
//            try
//            {
//                //imgAndes.Visible = false;
//                //imgRenaper.Visible = false; lblFechaDomicilio.Visible = false;
//                 GrabarLogAcceso("RENAPER", Request["dni"].ToString());

//                long nrodocumento = long.Parse(Request["dni"].ToString());
//                string sexo = Request["sexo"].ToString();

//                string rutaCert = ConfigurationManager.AppSettings["RutaCert"].ToString();              
//                string BaseUrl = ConfigurationManager.AppSettings["BaseUrlXroad"].ToString();
//                string Serv = "GP-RENAPER/WS_RENAPER_DOCUMENTO/";
//                string clie = ConfigurationManager.AppSettings["ClienteXroad"].ToString();
//                string param = nrodocumento.ToString() + "/" + sexo.ToUpper();
//                string host = BaseUrl + Serv + param;

//                ServicePointManager.Expect100Continue = true;
//                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
//                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
//                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

//                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(host);
                 
//                //certificado
//                X509Certificate certificate = new X509Certificate(rutaCert, "", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet
//                  | X509KeyStorageFlags.PersistKeySet);

//                req.ClientCertificates = new X509CertificateCollection() { certificate };
//                req.ContentType = "application/json";
//                req.AllowAutoRedirect = true;
//                req.Timeout = 10 * 1000;
//                req.Method = "GET";
//                req.Headers.Add("X-Road-Client", clie);

           
//                ResultadoRenaperModel resultado2;
              
                
//                using (WebResponse response = req.GetResponse())
//                {
//                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

//                    using (Stream strReader = response.GetResponseStream())
//                    {
                        
//                        using (StreamReader objReader = new StreamReader(strReader))
//                        {
                            
//                            string responseBody = objReader.ReadToEnd();

//                            if (!responseBody.Contains("error") )
//                            {
//                                resultado2 = jsonSerializer.Deserialize<ResultadoRenaperModel>(responseBody);
//                                PersonaRenaperModel persona_d = resultado2.data;

//                                if (resultado2.resultado.ToUpper() == "CORRECTO")
//                                {
//                             //       lblValidador.Visible = true;
//                               //     lblValidador.Text = "Paciente VALIDADO POR RENAPER";
//                                    ok = true;
//                                    txtDNI.Text = Request["dni"].ToString();
//                                    txtApellido.Text = persona_d.apellido.ToUpper();
//                                    txtNombre.Text = persona_d.nombres.ToUpper();
//                                    txtFechaNacimiento.Value = persona_d.fecha_nacimiento;
//                                    txtCalle.Value = persona_d.calle + " " + persona_d.numero;
//                                 //   txtCuil.Value = persona_d.cuil;
//                                    if (persona_d.ciudad == "") txtCiudad.Value = "SIN DATOS";
//                                    else txtCiudad.Value = persona_d.ciudad;

//                                    if (persona_d.provincia == "") txtProvincia.Value = "SIN DATOS";
//                                    else txtProvincia.Value = persona_d.provincia;
//                                    if (persona_d.pais == "") txtPais.Value = "SIN DATOS";
//                                    else txtPais.Value = persona_d.pais;
//                                    if (persona_d.codigo_postal == "") txtCodigoPostal.Value = "SIN DATOS";
//                                    else txtCodigoPostal.Value = persona_d.codigo_postal;
//                                    if (persona_d.monoblock == "") txtBarrio.Value = "SIN DATOS";
//                                    else
//                                        txtBarrio.Value = persona_d.monoblock;
//                                    fallecimiento.Text = persona_d.mensaje_fallecido;
//                                    fechaDomicilio.Text = persona_d.emision;

//                                    if (fechaDomicilio.Text == "")
//                                        lblFechaDomicilio.Visible = false;
//                                    else
//                                        lblFechaDomicilio.Visible = true;
//                                    if (Request["sexo"].ToString() == "F") { txtSexo.Value = "FEMENINO"; ddlSexo.SelectedValue = "2"; }
//                                    if (Request["sexo"].ToString() == "M") { txtSexo.Value = "MASCULINO"; ddlSexo.SelectedValue = "3"; }
//                                    if (Request["sexo"].ToString() == "X") { txtSexo.Value = "X"; ddlSexo.SelectedValue = "0"; }
//                                    idEstado.Value = "3"; // validado con renaper
//                                                          // System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
//                                                          //string foti= codificador.GetString(persona_d.foto);
//                                                          //  Image1.ImageUrl = persona_d.foto;


//                                    /// traer al paciente si no es nuevo, es modificacion
//                                    int id = Convert.ToInt32(Request.QueryString["id"]);
//                                    //datos del Paciente           
//                                    Paciente pac = new Paciente();
//                                    if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);
//                                    txtTelefono.Value = pac.InformacionContacto;
//                                    /// 

//                            //        imgRenaper.Visible = true;
//                                }
//                                else
//                                    ok = false;
//                            }
//                            else
//                                ok = false;

//                        }
//                        response.Close();
//                    }
//                    //   lstRetorno.Add(resultado);
//                }              
                

//            }
//            catch (WebException ex)
//{
//                ok = false;
//string mensaje = ex.ToString();

//}
//            //}
//            //catch (Exception ex)
//            //{
//            //    ok = false;
//            //    string mensaje = ex.Message.ToString();
//            //}

//            return ok;

//        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();


            ////////////Carga de combos de sexo biologico
            string m_ssql = "SELECT idSexo, nombre FROM sys_Sexo (nolock) order by nombre ";
            oUtil.CargarCombo(ddlSexo, m_ssql, "idSexo", "nombre");
            ddlSexo.Items.Insert(0, new ListItem("--Seleccione--", "0"));
            if (txtSexo.Value == "F") ddlSexo.SelectedValue = "2";
            if (txtSexo.Value == "M") ddlSexo.SelectedValue = "3";
            if (txtSexo.Value == "X")
                ddlSexo.SelectedValue = "0";

            //m_ssql = "SELECT idGenero, nombre FROM sys_Genero (nolock) order by nombre ";
            //oUtil.CargarCombo(ddlGenero, m_ssql, "idGenero", "nombre");
            //ddlGenero.Items.Insert(0, new ListItem("----", "0"));
            m_ssql = null;
            oUtil = null;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool conOK = false;
                CargarListas();

                if (Request["master"] != null)
                    btnConfirmar.Text = "Actualizar";
                else btnConfirmar.Visible = true;

                if (Request["Tipo"].ToString() == "T")
                {
                    HabilitaTemporal();

                }
     //           else
     //           {
                   
     //               if ( oCon.ConectaRenaper  )                   
     ////  conOK= SolicitarServicio();
     //   conOK =ConectarRenaperXRoad();
                    
     //                   if ((oCon.ConectaMPI) && (!conOK))
     //                       conOK = SolicitarServicioMPI();
                         
     //                   if (!conOK)
     //                       HabilitaCargaManual();
                     

     //           }


            }
        }

        //private bool SolicitarServicioMPI()
        //{
        //    bool ok = false;
        //    GrabarLogAcceso("MPI", Request["dni"].ToString());
        //    try
        //    {

        //    //    imgRenaper.Visible = false;
        //      //  imgAndes.Visible = false;
        //        string alfinApellido = ""; string alfinnombre = ""; string alfinSexo = ""; string alfinfechanac = "";
        //        string alfincalle = "SIN DATOS"; string alfinciudad = "SIN DATOS"; ; string alfinprovincia = "SIN DATOS"; string alfincodigopostal = "";
        //        string alfinpais = ""; string alfinTel = "SIN DATOS"; string alfincuil = "";
               
        //        System.Net.ServicePointManager.SecurityProtocol =
        //       System.Net.SecurityProtocolType.Tls12;

        //        string Servicio = oCon.UrlMPI; // "https://fhir.andes.gob.ar/4_0_0/patient/?identifier=http://www.renaper.gob.ar|";// oCon.UrlRenaper;

        //        string parametros = Request["dni"].ToString();


        //        //   string urlCompleta = urlServidorXroadLocal + "/" + Servicio + "/" + parametros;
        //        string urlCompleta = Servicio + parametros;

        //        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        //        string accessToken = oCon.TokenMPI; // @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjYyYWIyMzQwMmJmODlkOWM5MDE0ZTU1NCIsImFwcCI6eyJpZCI6IjYyYWIyMDc1MzVjNmMxNThmYzgyODNmMiIsIm5vbWJyZSI6IkxhYm9yYXRvcmlvLWNlbnRyYWwtbXBpIn0sIm9yZ2FuaXphY2lvbiI6eyJpZCI6IjU5MzgwMTUzZGI4ZTkwZmU0NjAyZWMwMiIsIm5vbWJyZSI6IkxhYm9yYXRvcmlvLWNlbnRyYWwtbXBpIn0sInBlcm1pc29zIjpbInVzZXIvUHJhY3RpdGlvbmVyLnJlYWQiLCJ1c2VyL1BhdGllbnQucmVhZCJdLCJhY2NvdW50X2lkIjpudWxsLCJ0eXBlIjoiYXBwLXRva2VuIiwiaWF0IjoxNjU1MzgyODQ4fQ.7xEkCx87mSJjQYBTOnGF_LoHd5EPRWfluNXf5SLHiZY";


        //        HttpWebRequest request;
        //        request = WebRequest.Create(urlCompleta) as HttpWebRequest;
        //        request.Timeout = -1;
        //        request.Method = "GET";
        //        //  request.ContentLength = data.Length;
        //        request.ContentType = "application/fhir+json; charset=utf-8";
        //        //   request.Headers.Add("Authorization", s_headerValorRenaper);

        //        request.Headers.Add("Authorization", "Bearer " + accessToken);
        //        //Stream postStream = request.GetRequestStream();
        //        //postStream.Write(data, 0, data.Length);

        //        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //        StreamReader reader = new StreamReader(response.GetResponseStream());
        //        string body = reader.ReadToEnd();



        //        if (body != "")
        //        {
        //            int len = body.Length;
        //            body = body.Substring(1, len - 2);

        //            ok = true;
        //            //string result = messge.Content.ReadAsStringAsync().Result;
        //            //description = result;
        //            if (body != "")
        //            {
        //                body = body.Replace("°", "").Replace("-", "");
        //                Root respuesta_Paciente = jsonSerializer.Deserialize<Root>(body);
        //                if (respuesta_Paciente.resourceType == "Patient")//&& respuesta_Paciente.active
        //                {
        //              //      lblValidador.Visible = true;
        //                //    lblValidador.Text = "Paciente VALIDADO POR MPI-ANDES";


        //                    List<Identifier> iden = respuesta_Paciente.identifier;
        //                    foreach (Identifier item in iden)
        //                    {
        //                        string sys = item.system;
        //                        if (sys == "http://www.renaper.gob.ar/cuil")
        //                            alfincuil = item.value;
        //                    }

        //                    List<Name> nombres = respuesta_Paciente.name;
        //                    foreach (Name item in nombres)
        //                    {
        //                        List<string> apellido = item.family;
        //                        foreach (string apel in apellido)
        //                        {
        //                            alfinApellido = alfinApellido + " " + apel.ToString();

        //                        }
        //                        List<string> nombre = item.given;
        //                        foreach (string nom in nombre)
        //                        {
        //                            alfinnombre = alfinnombre + "  " + nom.ToString();

        //                        }
        //                    }
        //                    alfinSexo = respuesta_Paciente.gender.ToUpper();
        //                    alfinfechanac = respuesta_Paciente.birthDate;

        //                    string anio, mes, dia;
        //                    if (!alfinfechanac.Contains("/"))
        //                    { anio = alfinfechanac.Substring(0, 4);
        //                        mes = alfinfechanac.Substring(4, 2);
        //                        dia = alfinfechanac.Substring(6, 2);

        //                        alfinfechanac = anio + "/" + mes + "/" + dia;

        //                            }
        //                    if (respuesta_Paciente.address != null)
        //                    {
        //                        alfinciudad = "";
        //                        alfinprovincia = "";
        //                        alfincodigopostal = "";
        //                        alfinpais = "";
        //                        List<Address> direccion = respuesta_Paciente.address;
        //                        foreach (Address dir in direccion)
        //                        {
        //                            if (dir.line != null)
        //                            {
        //                                List<string> calle = dir.line;
        //                                foreach (string nom in calle)
        //                                {
        //                                    alfincalle = nom.ToUpper();

        //                                }

        //                                alfinciudad = dir.city.ToUpper();
        //                                alfinprovincia = dir.state.ToUpper();
        //                                alfincodigopostal = dir.postalCode.ToUpper();
        //                                alfinpais = dir.country.ToUpper();
        //                            }
        //                        }
        //                    }

        //                    if (respuesta_Paciente.telecom != null)
        //                    {
        //                        alfinTel = "";
        //                        List<Telecom> telefono = respuesta_Paciente.telecom;
        //                        foreach (Telecom tel in telefono)
        //                        {
        //                            if (tel.system == "phone")
        //                                alfinTel = alfinTel + "  " + tel.value;
        //                        }
        //                    }

        //                    txtDNI.Text = Request["dni"].ToString();
        //                    txtApellido.Text = alfinApellido.ToUpper().TrimStart().TrimEnd();
        //                    txtNombre.Text = alfinnombre.ToUpper().TrimStart().TrimEnd();
        //                    txtFechaNacimiento.Value = alfinfechanac;
        //                    txtCalle.Value = alfincalle;
        //              //      txtCuil.Value = alfincuil;
        //                    if (alfinciudad == "") txtCiudad.Value = "SIN DATOS";
        //                    else txtCiudad.Value = alfinciudad.Trim();

        //                    if (alfinprovincia == "") txtProvincia.Value = "SIN DATOS";
        //                    else txtProvincia.Value = alfinprovincia.Trim();
        //                    if (alfinpais == "") txtPais.Value = "SIN DATOS";
        //                    else txtPais.Value = alfinpais.Trim();
        //                    if (alfincodigopostal == "") txtCodigoPostal.Value = "SIN DATOS";
        //                    else txtCodigoPostal.Value = alfincodigopostal.Trim();
        //                    txtBarrio.Value = "SIN DATOS";

        //                    //fallecimiento.Text = persona_d.mensaf;
        //                    //fechaDomicilio.Text = persona_d.EMISION;
        //                    if (alfinSexo == "FEMALE") { txtSexo.Value = "FEMENINO"; ddlSexo.SelectedValue = "2"; }
        //                    if (alfinSexo == "MALE") { txtSexo.Value = "MASCULINO"; ddlSexo.SelectedValue = "3"; }
        //                    if ((alfinSexo != "FEMALE") && (alfinSexo != "MALE")) { txtSexo.Value = "X"; ddlSexo.SelectedValue = "0"; }
        //                    idEstado.Value = "3"; // validado con renaper
        //                                          // System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
        //                                          //string foti= codificador.GetString(persona_d.foto);
        //                    //Image1.Visible = false;



        //                    txtTelefono.Value = alfinTel.TrimStart().TrimEnd();
        //               //     imgAndes.Visible = true;
        //                    //Identifier respuesta_Paciente = jsonSerializer.Deserialize<Identifier>(body);
        //                }//if patienet
        //            } // body
        //            else
        //            {

        //                lblMensaje.Text = "No se encontraron datos para el numero de documento de la persona en MPI-ANDES.";
        //                lblMensaje.Visible = true;
        //                idEstado.Value = "1";

        //           //     lblValidador.Visible = false;
        //                ok = false;

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMensaje.Text = "Hubo un error para recuperar datos para el numero de documento de la persona en MPI-ANDES.";
        //        lblMensaje.Visible = true;
        //        idEstado.Value = "1";
        //    //    lblValidador.Visible = false;
        //        ok = false;
        //        ok = false;
        //    }
        //    return ok;
        //}
    
        private void MostrarDatosPaciente()
        {

            Paciente pac = new Paciente();
            pac = (Paciente)pac.Get(typeof(Paciente), int.Parse(Request["id"].ToString()));
            if (pac != null)
            {
                switch (pac.IdSexo)
                {
                    case 1: txtSexo.Value = "X"; break;
                    case 2: txtSexo.Value = "F"; break;
                    case 3: txtSexo.Value = "M"; break;
                }

                txtDNI.Text = pac.NumeroDocumento.ToString();
                txtApellido.Text = pac.Apellido;
                txtNombre.Text = pac.Nombre;
                txtFechaNacimiento.Value = pac.FechaNacimiento.ToShortDateString();
                //txtCalle.Value = "-";
                //txtBarrio.Value = "-";
                //txtCiudad.Value = "-";
                //txtCodigoPostal.Value = "-";
                //txtPais.Value = "-";
                //txtProvincia.Value = "-";
                txtNumeroAdic.Value = pac.NumeroAdic;


            }

        }
        private void HabilitaCargaManual()
        {
      //      imgRenaper.Visible = false;
            lblTemporal.Text = "Servicio de Renaper no disponible";
            txtDNI.Text = Request["dni"].ToString();
            idEstado.Value = "1"; // identificado
            txtApellido.Enabled = true;
            txtNombre.Enabled = true;
            txtFechaNacimiento.Disabled = false;
            txtCalle.Disabled = false;
        //    txtCuil.Disabled = false;
            //txtCiudad.Disabled = false;
            //txtProvincia.Disabled = false;
            //txtPais.Disabled = false;
            //txtCodigoPostal.Disabled = false;
            //txtBarrio.Disabled = false;
            fallecimiento.Visible = false;
            txtSexo.Value = "X";
            if (Request["sexo"].ToString() == "F")
                txtSexo.Value = "FEMENINO";
            if (Request["sexo"].ToString() == "M")
                txtSexo.Value = "MASCULINO";
             
        }

        private void HabilitaTemporal()
        {
            idEstado.Value = "2"; // temporal
        //    imgRenaper.Visible = false;
            grupoDNI.Visible = false;
            lblTemporal.Visible = true;
            txtApellido.Enabled = true;
            txtNombre.Enabled = true;
            txtFechaNacimiento.Disabled = false;
            txtCalle.Disabled = false;
          //  txtCuil.Disabled = false;
            //txtCiudad.Disabled = true;
            //txtProvincia.Disabled = true;
            //txtPais.Disabled = true;
            //txtCodigoPostal.Disabled = true;
            //txtBarrio.Disabled = true;
            fallecimiento.Visible = false; txtSexo.Value = "X";
            
            if (Request["sexo"].ToString() == "F") { txtSexo.Value = "FEMENINO"; ddlSexo.SelectedValue = "2"; }
            if (Request["sexo"].ToString() == "M") { txtSexo.Value = "MASCULINO"; ddlSexo.SelectedValue = "3"; }
            if (Request["sexo"].ToString() == "X") { txtSexo.Value = "X"; ddlSexo.SelectedValue = "0"; }


            divNumeroAdicional.Visible = true;
            
            Utility oUtil = new Utility();
            ///Carga de motivo no identificación
            string m_ssql = "select idmotivoNI, nombre from Sys_MotivoNI (nolock) ";
            oUtil.CargarCombo(ddlMotivoNI, m_ssql, "idmotivoNI", "nombre");
            ddlMotivoNI.Items.Insert(0, new ListItem("--Seleccione motivo--", "0"));
            ddlMotivoNI.Enabled = true;
            rvMotivo.Enabled = true;

            m_ssql = "select idParentesco, nombre from LAB_Parentesco (nolock) ";
            oUtil.CargarCombo(ddlTipoParentesco, m_ssql, "idParentesco", "nombre");
            ddlTipoParentesco.Items.Insert(0, new ListItem("--Seleccione parentesco--", "0"));
            ddlTipoParentesco.Enabled = true;
            rvTipoParentesco.Enabled = true;



        }

        //private bool SolicitarServicio()
        //{
        //    bool ok = false;
        //    string mensaje = "";
        //    try
        //    {
        //        //imgAndes.Visible = false;
        //        //imgRenaper.Visible = false; lblFechaDomicilio.Visible = false;
        //        GrabarLogAcceso("RENAPER", Request["dni"].ToString());
        //        string Servicio = oCon.UrlRenaper;
              
        //        string parametros = Request["dni"].ToString()+"/"+ Request["sexo"].ToString(); ;
        //        BasicHttpBinding binding = new BasicHttpBinding();

        //        // Use double the default value
        //        binding.MaxReceivedMessageSize = 65536 * 2;


        //        //   string urlCompleta = urlServidorXroadLocal + "/" + Servicio + "/" + parametros;
        //        string urlCompleta =  Servicio + "/" + parametros;

        //        var request = (HttpWebRequest)WebRequest.Create(urlCompleta);

                
        //        string headerRenaper = ConfigurationManager.AppSettings["headerRenaper"].ToString();
        //        string[] a = headerRenaper.Split(':');
        //        string s_headerRenaper = a[0].ToString();
        //        string s_headerValorRenaper = a[1].ToString();

        //        request.Headers.Add(s_headerRenaper, s_headerValorRenaper);


        //        var response = (HttpWebResponse)request.GetResponse();
                
        //       string responseString="";
        //        using (var stream = response.GetResponseStream())
        //        {
        //            using (var reader = new StreamReader(stream))
        //            {
                      
        //                responseString = reader.ReadToEnd();

                         
        //            }
        //        }

        //       if (responseString !="")
        //          {
        //               mensaje = "el servicio se resolvió con exito";
                    
        //                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
        //                 resRenaper res = jsonSerializer.Deserialize<resRenaper>(responseString);


        //            //        //el resultado viene en 5 array de bytes

        //            //        //transformo el array a string y lo muestro
        //            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();


        //          //  String Resultado1 = enc.GetString(res.resultado1);

                    
        //                 string s = responseString;
                        
        //                 if (s != "0")
        //                {
        //                Persona persona_d = res.resultado1; // jsonSerializer.Deserialize<Persona>(s);
        //                   if (persona_d.nroError == "2") //persona no encontrada
        //                     {
        //                         txtDNI.Enabled = true;
        //                         lblMensaje.Text = "No se encontraron datos para el numero de doc. y sexo de la persona en Renaper. Verifique.";
        //                         lblMensaje.Visible = true;
        //                        idEstado.Value = "1";

        //                        if (Request["id"] != null)
        //                         {
        //                              Continuar();
                                

        //                    }


        //                }
        //                else
        //                {
        //                    if ((persona_d.apellido == "") && (persona_d.nombres == ""))
        //                    {
        //                        lblMensaje.Text = "No se encontraron datos para el numero y sexo de la persona en Renaper. Verifique.";
        //                        idEstado.Value = "1";
        //                    }
        //                    else
        //                    {
        //                        ok = true;
        //                        txtDNI.Text = Request["dni"].ToString();
        //                        txtApellido.Text = persona_d.apellido;
        //                        txtNombre.Text = persona_d.nombres;
        //                        txtFechaNacimiento.Value = persona_d.fechaNacimiento;
        //                        txtCalle.Value = persona_d.calle + " " + persona_d.numero;
        //            //            txtCuil.Value = persona_d.cuil;
        //                        if (persona_d.ciudad == "") txtCiudad.Value = "SIN DATOS";
        //                        else txtCiudad.Value = persona_d.ciudad;

        //                        if (persona_d.provincia == "") txtProvincia.Value = "SIN DATOS";
        //                        else txtProvincia.Value = persona_d.provincia;
        //                        if (persona_d.pais == "") txtPais.Value = "SIN DATOS";
        //                        else txtPais.Value = persona_d.pais;
        //                        if (persona_d.cpostal == "") txtCodigoPostal.Value = "SIN DATOS";
        //                        else txtCodigoPostal.Value = persona_d.cpostal;
        //                        if (persona_d.monoblock == "") txtBarrio.Value = "SIN DATOS";
        //                        else
        //                            txtBarrio.Value = persona_d.monoblock;
        //                        fallecimiento.Text = persona_d.mensaf;
        //                        fechaDomicilio.Text = persona_d.EMISION;

        //                        if (fechaDomicilio.Text == "")
        //                            lblFechaDomicilio.Visible = false;
        //                        else
        //                            lblFechaDomicilio.Visible = true;
        //                        if (Request["sexo"].ToString() == "F") { txtSexo.Value = "FEMENINO"; ddlSexo.SelectedValue = "2"; }
        //                         if (Request["sexo"].ToString() == "M") { txtSexo.Value = "MASCULINO"; ddlSexo.SelectedValue = "3"; }
        //                        if (Request["sexo"].ToString() == "X") { txtSexo.Value = "X"; ddlSexo.SelectedValue = "0"; }
        //                        idEstado.Value = "3"; // validado con renaper
        //                                              // System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
        //                                              //string foti= codificador.GetString(persona_d.foto);
        //                        //Image1.ImageUrl = persona_d.foto;


        //                        /// traer al paciente si no es nuevo, es modificacion
        //                         int id = Convert.ToInt32(Request.QueryString["id"]);
        //                        //datos del Paciente           
        //                        Paciente pac = new Paciente();
        //                        if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);
        //                        txtTelefono.Value = pac.InformacionContacto;
        //                        /// 

        //                     //   imgRenaper.Visible = true;

        //                    }
                       
        //            }//res
        //        }

        //        else
        //        {
        //                lblMensaje.Text = "El servicio de Renaper no está activo. Ingrese los datos del paciente manualmente. Reporte al Administrador.";
        //            lblMensaje.Visible = true;

        //            idEstado.Value = "1";
        //            HabilitaCargaManual();
        //                if (Request["id"] != null)
        //                {
        //                    Continuar();


        //                }
        //            }


        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!oCon.ConectaMPI)
        //        {
        //            lblMensaje.Text = "Ha ocurrido un error al conectarse a RENAPER. Reporte el problema al Administrador.";
        //            lblMensaje.Visible = true;
        //            idEstado.Value = "1";
        //            HabilitaCargaManual();
        //            if (Request["id"] != null)
        //            {
        //                Continuar();


        //            }
        //        }
        //    }
        //    return ok;
        //}

//        private void GrabarLogAcceso(string servicio, string dni)
//        {
//            try
//            {

//                SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

//                string query = @"
//INSERT INTO LAB_LogAccesoServicio 
//           ([numerodocumento]
//           ,[servicio]
//           ,[idUsuario]
//           ,[fecha])
//     VALUES
//           ('" + dni + "','" + servicio + "'," + Session["idUsuario"].ToString() + ", getdate()     )";

//                SqlCommand cmd = new SqlCommand(query, conn2);


//                int idres2 = Convert.ToInt32(cmd.ExecuteScalar());
//            }
//            catch (Exception ex)
//            { }
//        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (validadatos())
                {
                    if (validamail())
                    {
                        Utility oUtil = new Utility();
                        //instancio el usuario
                        Usuario us = new Usuario();
                        us = (Usuario)us.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                        int id = Convert.ToInt32(Request.QueryString["id"]);
                        //datos del Paciente           
                        Paciente pac = new Paciente();
                        if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);


                        pac.IdEfector = oCon.IdEfector; // us.IdEfector;
                        pac.Apellido = oUtil.SacaComillas(txtApellido.Text.ToUpper());
                        pac.Nombre = oUtil.SacaComillas(txtNombre.Text.ToUpper());

                        if (fechaDomicilio.Text == "") idEstado.Value = "1";

                        if (Request["Tipo"].ToString() == "T")
                        {
                            pac.IdEstado = 2;
                            pac.IdMotivoni = int.Parse(ddlMotivoNI.SelectedValue);
                            pac.NumeroDocumento = pac.generarNumero();
                        }
                        else
                        {
                            pac.IdEstado = int.Parse(idEstado.Value);

                            pac.NumeroDocumento = int.Parse(txtDNI.Text);
                        }


                        if (btnConfirmar.Text != "Actualizar")
                            pac.FechaAlta = DateTime.Now;

                        pac.IdSexo = 1;
                        /// Sexo legal
                        if (Request["sexo"].ToString() == "F")
                            pac.IdSexoLegal = 2;
                        if (Request["sexo"].ToString() == "M")
                            pac.IdSexoLegal = 3;
                        if (Request["sexo"].ToString() == "X")
                            pac.IdSexoLegal = 4;

                        // sexo biologico
                        pac.IdSexo = int.Parse(ddlSexo.SelectedValue);

                        // genero
                        //pac.IdGenero = int.Parse(ddlGenero.SelectedValue);
                        //pac.NombreAutopercibido = txtNombreAutopercibido.Value;
                        //valido que la fecha no se mayor a la actual
                        if ((Convert.ToDateTime(txtFechaNacimiento.Value) <= DateTime.Now) && (txtFechaNacimiento.Value != null))
                        {
                            pac.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Value);
                        }

                        pac.IdPais = -1;//;Convert.ToInt32(ddlNacionalidad.SelectedValue);
                        pac.IdProvincia = -1;


                        string domi = txtCalle.Value.ToUpper() ;
                        if (domi.Length >= 50)
                            domi = domi.Substring(0, 50);

                        pac.Calle = domi;

                        pac.Numero = 0;




                        pac.Piso = "";
                        pac.Departamento = "";
                        pac.Manzana = "";


                        //if (btnConfirmar.Text != "Actualizar")
                        pac.InformacionContacto = oUtil.SacaComillas(txtTelefono.Value);
                        pac.FechaDefuncion = Convert.ToDateTime("01/01/1900");
                        pac.IdUsuario = us.IdUsuario;
                        pac.FechaUltimaActualizacion = DateTime.Now;
                        pac.NumeroAdic = txtNumeroAdic.Value;
                        pac.Mail = txtMail.Value;
                        ddlRaza.SelectedValue = pac.IdRaza.ToString();


                        if (ddlAborigen.SelectedValue == "0")
                            pac.SeDeclaraAborigen = false;
                        else
                            pac.SeDeclaraAborigen = true;


                        pac.Save();


                        //if (idEstado.Value == "3")
                        if ((txtCalle.Value != "") )//&& (txtBarrio.Value != "") && (txtCiudad.Value != "") && (txtProvincia.Value != "") && (txtPais.Value != "") && (txtCodigoPostal.Value != ""))
                        {
                            if (fechaDomicilio.Text == "")
                                fechaDomicilio.Text = DateTime.Now.ToShortDateString();
                            pac.GuardarDomicilio(fechaDomicilio.Text, txtCalle.Value, "", "", "", "", "");
                            //GuardarFoto(pac);
                        }

                        GuardarParentesco(pac);



                        if (Request["llamada"] != null)
                        {


                            if (Request["llamada"] == "LaboTurno")
                                Response.Redirect("../Turnos/TurnosEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&Modifica=0");
                            if (Request["llamada"] == "LaboEfector")
                            {
                                if (Request["idProtocolo"] == null)
                                    Response.Redirect("../Protocolos/ProtocoloEditEfector.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&llamada=LaboEfector&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta");

                                else
                                    Response.Redirect("../Protocolos/ProtocoloEditEfector.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&llamada=LaboEfector&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString());
                            }
                            if (Request["llamada"] == "LaboProtocolo")
                            {
                                if (Request["idProtocolo"] == null)
                                {
                                    if (Request["idServicio"].ToString() == "6")
                                    {
                                        if (Session["idCaso"].ToString() != "0")
                                        {
                                            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                                            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));
                                            if (oCaso.IdTipoCaso == 1) //filiacion

                                                Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&idServicio=6");
                                            if ((oCaso.IdTipoCaso == 2) || (oCaso.IdTipoCaso == 3)) //forense o quimerismo

                                                Response.Redirect("ProtocoloEditForense.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&Operacion=Alta&idServicio=6&idCaso=" + Session["idCaso"].ToString());
                                        }

                                        else // muestra sin caso, pedir consentimiento.
                                        {
                                            Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&idServicio=6");
                                        }


                                    }
                                    else
                                        Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta");
                                }
                                else
                                    Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString());
                            }
                        }
                    }
                    else
                    {
                        lblMensaje.Text = "Email inválido";
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    lblMensaje.Text = "hay errores en los datos. Corrija caracteres especiales � o verifique datos faltantes o erroneos como fechas(formato dd/mm/aaaa).";
                    lblMensaje.Visible = true;
                }
            }
        }

        private void GuardarParentesco(Paciente pac)
        {
            //par = (Parentesco)par.Get(typeof(Parentesco),"IdPaciente", pac.IdPaciente);
            if (( txtApellidoParentesco  .Text != "") & (txtNombreParentesco.Text != ""))
            {
                Utility oUtil = new Utility();

                Parentesco par = new Parentesco();               
          //      par = (Parentesco)par.Get(typeof(Parentesco), "IdPaciente",pac);


                par.Apellido = oUtil.SacaComillas(txtApellidoParentesco.Text.ToUpper());
                par.Nombre = oUtil.SacaComillas(txtNombreParentesco.Text.ToUpper());
                par.IdTipoDocumento = 1; // Convert.ToInt32(ddlTipoDocP.SelectedValue);
                par.IdPaciente = pac;
                
                if (ddlTipoParentesco.SelectedValue != "")
                    par.TipoParentesco = ddlTipoParentesco.SelectedItem.Value;
                if (!string.IsNullOrEmpty(txtDNIParentesco.Text))
                    par.NumeroDocumento = Convert.ToInt32(txtDNIParentesco.Text);
                //par.NumeroDocumento = Convert.ToInt32(txtNumeroP.Text);
                //if (txtFechaN.Value != "")
                //    par.FechaNacimiento = Convert.ToDateTime(txtFechaN.Value);
                //else
                    par.FechaNacimiento = Convert.ToDateTime("01/01/1900");
               
               
                par.IdUsuario = oUser;
                //guardo la fecha actual de modificacion
                par.FechaModificacion = DateTime.Now;
                par.Save();
            }
        }

        private bool validadatos()
        {
            bool g=true;          
            if (txtApellido.Text.Contains("\uFFFD"))  g = false;
            if (txtNombre.Text.Contains("\uFFFD")) g = false;
            if (txtCalle.Value.Contains("\uFFFD")) g = false;
            //if (txtCiudad.Value.Contains("\uFFFD")) g = false;
            //if (txtProvincia.Value.Contains("\uFFFD")) g = false;
            //if (txtPais.Value.Contains("\uFFFD")) g = false;
            //if (txtBarrio.Value.Contains("\uFFFD")) g = false;
            if (ddlSexo.SelectedValue == "0") g = false;
            DateTime fechaNacimiento;
            if (DateTime.TryParse(txtFechaNacimiento.Value, out fechaNacimiento))
            {

                if (DateTime.Now < fechaNacimiento)
                    g = false;
                 
            }
            else
                g = false;
            return g;
        }

        //private void GuardarFoto(Paciente pac)
        //{
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //    string query = @"update [dbo].[Sys_Paciente] set 
        //   [Foto] = @imagen where idPaciente=" + pac.IdPaciente;;
        //    SqlCommand cmd = new SqlCommand(query, conn);

        //    byte[] image =  Encoding.UTF8.GetBytes(Image1.ImageUrl);


        //    SqlParameter imageParam = cmd.Parameters.Add("@imagen", System.Data.SqlDbType.Image);

        //    imageParam.Value = image;


        //   int idconsenti = Convert.ToInt32(cmd.ExecuteScalar());
        //}

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            Continuar();
        }
        private bool validamail()
        {
            try
            {
                if (txtMail.Value != "")
                {
                    MailAddress m = new MailAddress(txtMail.Value);
                    return true;
                }
                else return true;
            }
            catch (FormatException)
            {
                return false;


            }

        }
        private void Continuar()
        {
            if (Request["llamada"] != null)
            {

                if (Request["llamada"] == "LaboEfector")
                {
                    if (Request["idProtocolo"] == null)
                        

                    Response.Redirect("../Protocolos/ProtocoloEditEfector.aspx?idPaciente=" + Request["id"].ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta");

                    else
                        
                    Response.Redirect("../Protocolos/ProtocoloEditEfector.aspx?idPaciente=" + Request["id"].ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString());
                }

                if (Request["llamada"] == "LaboProtocolo")
                {
                    if (Request["idProtocolo"] == null)
                    {
                        if (Request["idServicio"].ToString() == "6")
                        {
                            if (Session["idCaso"].ToString() != "0")
                            {
                                Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                                oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));
                                if (oCaso.IdTipoCaso == 1) //filiacion

                                    Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + Request["id"].ToString() + "&idServicio=6");
                                if ((oCaso.IdTipoCaso == 2) || (oCaso.IdTipoCaso == 3)) //forense o quimerismo

                                    Response.Redirect("ProtocoloEditForense.aspx?idPaciente=" + Request["id"].ToString() + "&Operacion=Alta&idServicio=6&idCaso=" + Session["idCaso"].ToString());
                            }

                            else // muestra sin caso, pedir consentimiento.
                            {
                                Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + Request["id"].ToString() + "&idServicio=6");
                            }


                        }
                        else
                            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + Request["id"].ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta");
                    }
                    else
                        Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + Request["id"].ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString());
                }
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {

}

       
        protected void ddlMotivoNI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMotivoNI.SelectedValue == "1") //recien nacido
            {
                pnlSexoBiologico.Visible = false;

                pnlParentesco.Visible = true;
                grupoDNIParentesco.Visible = true;
                rfvDNIParentesco.Enabled = true;
                rfvApellidoParentesco.Enabled = true;
                rfvNombreParentesco.Enabled = true;
                grupoapellidoParentesco.Visible = true;
                gruponombreParentesco.Visible = true;
                grupoListaParentesco.Visible = true;
                grupoTipoParentesco.Visible = true;
                grupoFechaNacParentesco.Visible = true;


            }

            if (ddlMotivoNI.SelectedValue == "2") //extranjero sin parentesco
            {
                pnlParentesco.Visible = true;
                pnlSexoBiologico.Visible = true;
                grupoDNIParentesco.Visible = false;
                rfvDNIParentesco.Enabled = false;
                rfvApellidoParentesco.Enabled = false;
                rfvNombreParentesco.Enabled = false;
                grupoapellidoParentesco.Visible = false;
                gruponombreParentesco.Visible = false;
                grupoListaParentesco.Visible = false;
                grupoTipoParentesco.Visible = false;
                grupoFechaNacParentesco.Visible = false;


            }

            if (ddlMotivoNI.SelectedValue == "3") //NN con parentesco opcional
            {
                pnlSexoBiologico.Visible = true;
                pnlParentesco.Visible = true;
                grupoDNIParentesco.Visible = true;
                rfvDNIParentesco.Enabled = false;
                rfvApellidoParentesco.Enabled = false;
                rfvNombreParentesco.Enabled = false;
                grupoapellidoParentesco.Visible = true;
                gruponombreParentesco.Visible = true;
                grupoListaParentesco.Visible = true;
                grupoTipoParentesco.Visible = true;
                grupoFechaNacParentesco.Visible = true;


            }

        }

        protected void btnConfirmarParentesco_Click(object sender, EventArgs e)
        {
         
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            VerificarPaciente();
        }

        private void VerificarPaciente()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session.CreateCriteria(typeof(Paciente));
            crit2.Add(Expression.Eq("NumeroDocumento", int.Parse(txtDNIParentesco.Text)));

            IList pacEncontrados = crit2.List();


            foreach (Paciente oPac in pacEncontrados)
            {
                txtApellidoParentesco.Text = oPac.Apellido;
                txtNombreParentesco.Text = oPac.Nombre;
                txtfechaNacParentesco.Text = oPac.FechaNacimiento.ToShortDateString(); ;
            }
            txtNombreParentesco.UpdateAfterCallBack = true;
            txtApellidoParentesco.UpdateAfterCallBack = true;
            txtfechaNacParentesco.UpdateAfterCallBack = true;

        }

        /// fin de continuar
    }
}