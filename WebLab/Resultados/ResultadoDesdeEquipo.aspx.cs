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

namespace WebLab.Resultados
{
    public partial class ResultadoDesdeEquipo : System.Web.UI.Page
    {
        string listavalidado = "";
        //   string resultado = "";
        DataTable dtDeterminaciones; //tabla para determinaciones
        int fila = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 VerificaPermisos("Validacion");
              
                //Page.ClientScript.RegisterStartupScript(this, typeof(this.Page),  "ScriptDoFocus",   SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),  true);
                CargarListas();
                //InicializarTablas();
                if (Request["mostrarResumen"] != null)
                {
                    MostrarResumen();
                    //MostrarDetalle();
                   
                    btnValidar.Enabled = false;
                    
                    //gvLista.Visible = false;
                    fila = 0;
                }
                                

            }


        }
       
       

      
        private void MostrarResumen()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  select  resultadocar as Resultado, count (*) as Cantidad
   from LAB_DetalleProtocolo where idDetalleProtocolo in (" + Session["ListaValidado"].ToString() + ") group by resultadocar ";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumen.DataSource = Ds.Tables[0];
            gvResumen.DataBind();
            gvResumen.Visible = true;
            lblProcesado.Visible = true;
            // mostrar la grilla con lo procesado

          
           

            // sisa
            //lblMensajeSISA.Visible = false;
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
           
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


                    if (oCon.NotificarSISA)

                    {
                        if (oDetalle.IdProtocolo.IdCaracter != 2) // no se suben controles de alta
                        {
                            if ((oDetalle.IdProtocolo.IdCasoSISA == 0) && (oDetalle.IdProtocolo.IdPaciente.IdEstado == 3) && (oDetalle.IdItem.Codigo == "9122"))
                            {
                                string res = oDetalle.ResultadoCar;


                                if (res.Length > 10)
                                {
                                    if ((res.Substring(0, 10) == "SE DETECTA") && (oCon.PreValida == false))
                                    { if (GenerarSISA(oDetalle, "SE DETECTA")) i = i + 1; }

                                    if ((res.Substring(0, 13) == "NO SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter == 4))
                                    { if (GenerarSISA(oDetalle, "NO SE DETECTA")) i = i + 1; }

                                } // if res 
                            }//  if ((oDetalle.IdProtocolo.IdCasoS
                        }//  if (oDetalle.IdProtocolo.IdCaracter 
                    }//  foreach (DetalleP
                }//   if (detalle.
                if (i > 0)
                {
                    //lblMensajeSISA.Text = "Se han generado " + i.ToString() + " eventos nuevos en SISA";
                    //lblMensajeSISA.Visible = true;
                }
            }//  if (oCon.NotificarSISA)

           
            

        }
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            //PintarReferencias();
        }
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
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

        private bool GenerarSISA(DetalleProtocolo oDetalle, string res)
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
                        seguir = false;
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

                        //usuario = "mapaterniti",
                        //clave = "Alicia1938",
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

                        if (respuesta_d.resultado == "OK")
                        { //  devolver el idcaso para guardar en la base de datos
                            string s_idcaso = respuesta_d.id_caso;

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuarioValida"].ToString()));

                            generacaso = true;

                        }
                        else
                        {
                            generacaso = false;
                            //hayerror = true;
                            error = respuesta_d.resultado;

                        }
                    }

                    //if (hayerror)
                    //{
                    //    lblError.Text = "Ocurrio error: " + error + " al conectar al servicio SISA. Intente de nuevo o haga clic en Salir";
                    //    lblError.Visible = true;
                    //    btnSalir.Visible = true;
                    //}
                    //else
                    //    Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);
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
        private void CargarListas()
        {
            Item oItem = new Item();

            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
            if (oItem != null)
            {
                lblDeterminacion.Text = oItem.Nombre;
                lblCodigo.Text = oItem.Codigo;
                hiditem.Value = oItem.IdItem.ToString();
               

            }


            string m_strSQL = @"  select P.numero , Pa.numerodocumento as dni, 
Pa.apellido + '  ' + Pa.nombre  as Paciente, resultadoCar as resultado,
D.fechaenvio 
from lab_Protocolo P
inner join sys_paciente  Pa on Pa.idpaciente=P.idpaciente
inner join lab_detalleprotocolo D on D.idprotocolo= P.idprotocolo
 where P.baja=0 and  enviado=1 and idusuariovalida=0 and idusuarioprevalida=0 and idusuarioresultado=0  and d.idSubItem= " + hiditem.Value;
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
           
         


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
  


 

 
      
  
  
      

     

     

     


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int i = 0;
                foreach (GridViewRow row in gvLista.Rows)
                {

                   

                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {

                        string m_idDetalleProtocolo=gvLista.DataKeys[row.RowIndex].Value.ToString();
                        GuardarResultado(m_idDetalleProtocolo);



                    }//checked
                }// grid
                Response.Redirect("ResultadoDesdeEquipo.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString() + "&mostrarResumen=1", false);
            }


        }

        private void GuardarResultado(string m_idDetalleProtocolo  )
        {

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            //if (!todo)
            //    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdDetalleProtocolo", int.Parse(m_idDetalleProtocolo), "IdUsuarioValida", 0);// crit.Add(Expression.Eq("IdUsuarioValida", 0));
            //else
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));

            if (oDetalle != null)
            {

                //int tiporesultado = oDetalle.IdSubItem.IdTipoResultado;
                //switch (tiporesultado)
                //{
                //    case 1:// numerico         
                //        if (valorItem != "")
                //        {
                //            oDetalle.ResultadoNum = decimal.Parse(valorItem, System.Globalization.CultureInfo.InvariantCulture);
                //            oDetalle.FormatoValida = oDetalle.IdSubItem.FormatoDecimal;
                //            oDetalle.ConResultado = true;
                //        }
                //        else
                //        {
                //            oDetalle.ResultadoNum = 0;
                //            oDetalle.ConResultado = false;
                //        }
                //        break;
                //    default:
                //        if (valorItem != "")
                //        {
                //            oDetalle.ResultadoCar = valorItem;
                //            oDetalle.ConResultado = true;
                //        }
                //        else
                //        {
                //            oDetalle.ResultadoCar = "";
                //            oDetalle.ConResultado = false;
                //        }
                //        break;
                //}


             string valorRef = oDetalle.CalcularValoresReferencia();
                string m_metodo = "";
                string m_valorReferencia = "";
                //string nombre_control = "VR" + oDetalle.IdDetalleProtocolo.ToString();
                //Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
                //Label valorRef = (Label)control1;


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
                    string valorItem = oDetalle.ResultadoCar;
                    string res = valorItem;
                    if (valorItem.Length > 10)
                        res = valorItem.Substring(0, 10);


                    if ((oDetalle.IdItem.Codigo == "9122") && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0) && (res == "SE DETECTA"))
                    {
                        if (oCon.PreValida)
                        {
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


                oDetalle.Save();


                if (oDetalle.ConResultado)
                {
                    if (Request["Operacion"].ToString() != "Valida")
                        oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuario"].ToString()));
                    else
                        oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuarioValida"].ToString()));
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
                        oProtocolo.Estado = 2;  //validado total (cerrado);                    
                    else
                        oProtocolo.Estado = 1;
                }
                oProtocolo.Save();

            }



        }








      




    

    }
}