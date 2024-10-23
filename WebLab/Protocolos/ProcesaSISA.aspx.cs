using System;
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
using System.Net.Http;

namespace WebLab.Protocolos
{
    public partial class ProcesaSISA : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request["Desde"] != null)

            {
                this.MasterPageFile = "~/Resultados/SitePE.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Protocolo protocolo = new Protocolo();
                protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));
                string idItem = protocolo.GenerarCasoSISA(); // se fija si hay algun item que tiene configurado notificacion a sisa
                if (idItem!="")
                {
                    MostrarDatosPaciente(protocolo, idItem);

                    // si la carga es en el laboratorio se graba solo el caso.
                    if (Request["Desde"] == null)
                        GenerarCasoSISA(protocolo);
                }
                else
                {
                    if (Request["Desde"] != null)

                    {

                        MostrarPacienteAntigeno(protocolo);
                        GenerarCasoSISA(protocolo);
                        Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                    }
                    else
                        Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                }

            }
        }

        
        private void MostrarPacienteAntigeno(Protocolo protocolo )
        {
            if (protocolo != null)
            {
                switch (protocolo.IdPaciente.IdSexo)
                {
                    case 1: lblSexo.Text = "I"; break;
                    case 2: lblSexo.Text = "F"; break;
                    case 3: lblSexo.Text = "M"; break;
                }

                lblDNI.Text = protocolo.IdPaciente.NumeroDocumento.ToString();
                lblApellido.Text = protocolo.IdPaciente.Apellido;
                lblNombre.Text = protocolo.IdPaciente.Nombre;
                lblFechaNacimiento.Text = protocolo.IdPaciente.FechaNacimiento.ToShortDateString();
                lblFechaPapel.Text = protocolo.FechaOrden.ToShortDateString();
                txtGrupoEvento.Value = "";

                string caracter = "";
                string idevento = "307";
                string nombreevento = "";
                string idclasificacionmanual = "";
                string nombreclasificacionmanual = "";
                string idgrupoevento = "113";
                string nombregrupoevento = "";

               //AJUSTE PARA INFORMAR A SISA TEST ANTIGENO COVID 19
                if (Request["codigo"] != null)
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", Request["codigo"].ToString(), "Baja", false);
                    if (oItem != null)
                    {
                        ISession m_session = NHibernateHttpModule.CurrentSession;

                        ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                        crit.Add(Expression.Eq("IdProtocolo", protocolo));
                        crit.Add(Expression.Eq("IdItem", oItem));
                        IList listadetalle = crit.List();

                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {
                            if (oDetalle.ResultadoCar == "REACTIVO")
                            {
                                idclasificacionmanual = "795";
                                nombreclasificacionmanual = "Caso Confirmado por Test de Antígeno en terreno (2019-nCoV)";
                                //idclasificacionmanual = "754";
                                //nombreclasificacionmanual = "Caso confirmado de nuevo coronavirus (2019-nCoV)";
                            }
                            if (oDetalle.ResultadoCar == "NO REACTIVO")
                            {
                                idclasificacionmanual = "752";
                                nombreclasificacionmanual = "Caso sospechosos de nuevo coronavirus (2019-nCoV)";
                            }
                        }

                        txtGrupoEvento.Value = idgrupoevento;
                        lblGrupoEvento.Text = nombregrupoevento;
                        txtEvento.Value = idevento;
                        lblEvento.Text = nombreevento;
                        txtClasificacionManualCaso.Value = idclasificacionmanual;
                        lblClasificacionManual.Text = nombreclasificacionmanual;

                    }

                }



            }

        }
 

        private void MostrarDatosPaciente(Protocolo protocolo, string idItem)
        {
            if (protocolo != null)
            {
                switch (protocolo.IdPaciente.IdSexo)
                {
                    case 1: lblSexo.Text = "I"; break;
                    case 2: lblSexo.Text = "F"; break;
                    case 3: lblSexo.Text = "M"; break;
                }

                lblDNI.Text = protocolo.IdPaciente.NumeroDocumento.ToString();
                lblApellido.Text = protocolo.IdPaciente.Apellido;
                lblNombre.Text = protocolo.IdPaciente.Nombre;
                lblFechaNacimiento.Text = protocolo.IdPaciente.FechaNacimiento.ToShortDateString();
                lblFechaPapel.Text = protocolo.FechaOrden.ToShortDateString();
                txtGrupoEvento.Value = "";

                string caracter = "";
                string idevento = "";
                string nombreevento = "";
                string idclasificacionmanual = "";
                string nombreclasificacionmanual = "";
                string idgrupoevento = "";
                string nombregrupoevento = "";

                if (Request["codigo"] == null)
                {
                    string m_strSQL = " select * from LAB_ConfiguracionSISA  where idCaracter=" + protocolo.IdCaracter.ToString() + " and idItem=" + idItem;



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

                        //if (protocolo.VerificaObligatoriedadFIS())
                        if (1 == 1)
                        // agregar un campo en lab_confuguracionsisa para saber si se notifica en el alta(por ej.contacto en covid solo si el resultado es positivo no se notifica en el alta
                        {
                            txtGrupoEvento.Value = idgrupoevento;
                            lblGrupoEvento.Text = nombregrupoevento;
                            txtEvento.Value = idevento;
                            lblEvento.Text = nombreevento;
                            txtClasificacionManualCaso.Value = idclasificacionmanual;
                            lblClasificacionManual.Text = nombreclasificacionmanual;
                            break;
                        }


                    }

                }
                /// AJUSTE PARA INFORMAR A SISA TEST ANTIGENO COVID 19
                if (Request["codigo"] != null)
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", Request["codigo"].ToString(), "Baja", false);
                    if (oItem != null)
                    {
                        ISession m_session = NHibernateHttpModule.CurrentSession;

                        ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                        crit.Add(Expression.Eq("IdProtocolo", protocolo));
                        crit.Add(Expression.Eq("IdItem", oItem));
                        IList listadetalle = crit.List();

                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {
                            if (oDetalle.ResultadoCar == "REACTIVO")
                            {
                                idclasificacionmanual = "795";
                                nombreclasificacionmanual = "Caso Confirmado por Test de Antígeno en terreno (2019-nCoV)";
                                //idclasificacionmanual = "754";
                                //nombreclasificacionmanual = "Caso confirmado de nuevo coronavirus (2019-nCoV)";
                            }
                            if (oDetalle.ResultadoCar == "NO REACTIVO")
                            {
                                idclasificacionmanual = "752";
                                nombreclasificacionmanual = "Caso sospechosos de nuevo coronavirus (2019-nCoV)";
                            }
                        }

                        txtGrupoEvento.Value = idgrupoevento;
                        lblGrupoEvento.Text = nombregrupoevento;
                        txtEvento.Value = idevento;
                        lblEvento.Text = nombreevento;
                        txtClasificacionManualCaso.Value = idclasificacionmanual;
                        lblClasificacionManual.Text = nombreclasificacionmanual;

                    }

                }



            }

        }




        public class RespuestaCaso
        {
            public string id_caso { get; set; }

            public string resultado { get; set; }

            public string description { get; set; }
        }
        public class AltaCaso
        {
            public string usuario { get; set; }
            public string clave { get; set; }
            public evento altaEventoCasoNominal { get; set; }

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

     
        private void GenerarCasoSISA(Protocolo protocolo)
        {

            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;
            try
            { 

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.UrlServicioSISA; 
            string s_idestablecimiento = oCon.CodigoEstablecimientoSISA; // "14580562167000"
            string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
            string[] a = usersisa.Split(':');
            string s_user = a[0].ToString();
            string s_userpass = a[1].ToString();
                
            string s_sexo = lblSexo.Text;
            string fn = lblFechaNacimiento.Text.Replace("/", "-");
            string s_idEvento = txtEvento.Value;
                string fnpapel = lblFechaPapel.Text.Replace("/", "-");
            string numerodocumento = lblDNI.Text;
            string clasificacionmanual = txtClasificacionManualCaso.Value;
                string idgrupoevento = txtGrupoEvento.Value;
                bool hayerror = false;
                string error = ""; 




                evento newevento = new evento
                {
                    idTipodoc = "1",
                    nrodoc = numerodocumento,
                    sexo = s_sexo,
                    fechaNacimiento = fn,  //"05-06-1989",
                    idGrupoEvento = idgrupoevento,
                    idEvento = s_idEvento, // "77",
                    idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                    fechaPapel = fnpapel, // "10-12-2019",
                    idClasificacionManualCaso = clasificacionmanual, // "22"
                };

                AltaCaso caso = new AltaCaso
            {

                    usuario = s_user, // "bcpintos",
                    clave = s_userpass, // "2398HH6RK6",
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
                            ///grabar a protocolo idCaso
                            //Protocolo protocolo = new Protocolo();
                            //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));
                            if (protocolo != null)
                            {
                                protocolo.IdCasoSISA = int.Parse(s_idcaso);
                                protocolo.Save();
                                if (respuesta_d.resultado == "OK")
                                    protocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                                else // ERROR_DATOS
                                    protocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));                                                  
                            }

                        }
                        else
                        {
                        hayerror = true;
                        error = respuesta_d.resultado + ": "+ respuesta_d.description;
                      
                    }
                    }

                //    if (hayerror)
                //{
                //    lblError.Text =   error + " .Intente de nuevo o haga clic en Salir";
                //    lblError.Visible = true;
                //    btnSalir.Visible = true;
                //}
                //else

                if (Request["Desde"] != null)

                {
                    Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                } else
                Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);


            }
            catch (WebException ex)
            {
             //   string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
             
                if (Request["Desde"] != null)

                {
                    Response.Redirect("DefaultEfector.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
                }
                else
                    Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);

                //lblError.Text = "Hubo algun problema al conectar al servicio SISA. Intente de nuevo o haga clic en Salir";
                //lblError.Visible = true;
                //btnSalir.Visible = true;
            }
        }





        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            //Protocolo protocolo = new Protocolo();
            //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));

            //if (protocolo != null)
            //{

            //    GenerarCasoSISA(protocolo);




            //}
        }



       

     


        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);


        }

        

    }
}

