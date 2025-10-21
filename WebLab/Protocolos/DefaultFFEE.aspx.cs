﻿using System;
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
using System.Drawing;
using Business.Data;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using NHibernate;
using NHibernate.Expression;
using System.Text.RegularExpressions;

namespace WebLab.Protocolos
{
    public partial class DefaultFFEE : System.Web.UI.Page
    {
       
        public Configuracion oC = new Configuracion();

        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion),  "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!Page.IsPostBack)
            {

                if (Request["Numero"] != null)
                    lblMensajeOK.Text = "SE HA GENERADO EL PROTOCOLO NRO." + Request["Numero"].ToString();
               
                txtCodigo.Focus();

                if (Request["idServicio"] != null) {
                  
                    Session["idServicio"] = Request["idServicio"].ToString(); 

                }
                if (Request["idUrgencia"] != null)   Session["idUrgencia"] = Request["idUrgencia"].ToString();
                ///idUrgencia=1 La sesion la creo para que cuando se acceda a nuevo paciente no se pierda que se trata de una urgencia.
                //idUrgencia=2 para el modulo urgencia.

               

                if (Request["idUsuario"] != null) Session["idUsuario"] = Request["idUsuario"].ToString();

 

                switch (Session["idServicio"].ToString())
                { case "3": lblServicio.Text = "MICROBIOLOGIA"; break;
                    case "1": lblServicio.Text = "LABORATORIO"; break;
                    case "4": lblServicio.Text = "PESQUISA NEONATAL"; break;

                    case "6":
                        {
                            lblServicio.Text = "FORENSE";
                            pnlTitulo.Attributes.Add("class", "panel panel-success");
                            pnlTitulo2.Attributes.Add("class", "panel panel-success");
                                //gvLista.HeaderStyle.BackColor = System.Drawing.Color.LightGreen;
                                btnBuscar.CssClass = "btn btn-success";
                            //if (Session["idCaso"] == null)
                            //    Session["idCaso"] = "0";




                        }
                        break;


                }


                
                    lblTitulo.Text = "NUEVO PROTOCOLO";
                
                  

                    //if ((Request["idServicio"].ToString() == "6") && (Session["idCaso"] != null))
                        ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
                    //else
                    //{
                    //    ProtocoloList1.Visible = false;
                    //    btnFinalizarCaso.Visible = false;
                    //}



                   
               

           

            CargarEfectores();
            CargarTipoMuestra();
            CargarCaracteres();
          
                CargarImpresoras();
            if (Session["Etiquetadora"] != null) ddlImpresora.SelectedValue = Session["Etiquetadora"].ToString();
            }
        }

        private void CargarImpresoras()
        {
            
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            Utility oUtil = new Utility();
            string m_ssql =  "SELECT idImpresora, nombre FROM LAB_Impresora with (nolock) where idEfector=" + oUser.IdEfector.IdEfector.ToString() + " order by nombre";  //MultiEfector

            oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre", connReady);

            ddlImpresora.Items.Insert(0, new ListItem("Seleccione impresora", "0"));
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
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
            else Response.Redirect("../FinSesion.aspx", false);
        }
      

     
      
        public void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { ConectarWebService();}

            
        }

        private void ConectarWebService()
        {
           
                string s_codigo = ControlCodigo();
             try
            {
                if (s_codigo != "")
                {
                  //  Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                    string URL = ConfigurationManager.AppSettings["urlffeeandes"].ToString();
                    URL = URL + "&desde=2000-01-01T23:18:08.382Z&hasta=2050-01-01T23:18:11.407Z&totalOrganizaciones=true&identificadorPCR=" + s_codigo.ToUpper();

                    string s_token = ConfigurationManager.AppSettings["tokenffeeandes"].ToString();


                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    HttpWebRequest request;
                    request = WebRequest.Create(URL) as HttpWebRequest;
                    request.Timeout = 10 * 1000;
                    request.Method = "GET";

                    request.ContentType = "application/json";
                    request.Headers.Add("Authorization", s_token);


                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();
                    body = body.Replace("[", "").Replace("]", "");
                    if (body != "")
                    {
                        String bodyLimpio = body.Replace("'", "''");

                        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                        string query = @" INSERT INTO [dbo].[LAB_FichaElectronica]
                            ([body] ,  fechaRegistro,idusuarioregistro )
                            VALUES  ('" + bodyLimpio + @"', GETDATE() ,'" + oUser.IdUsuario.ToString() + @"')";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        int idres = Convert.ToInt32(cmd.ExecuteScalar());
                        try
                        {
                            FFEE respuesta_d = jsonSerializer.Deserialize<FFEE>(body);

                            if (respuesta_d.identificadorpcr == s_codigo)
                            {
                                string errores = Validacion(respuesta_d);
                                if (errores == "")
                                {
                                    ProcesaFicha(respuesta_d);
                                    Session["Etiquetadora"] = ddlImpresora.SelectedValue;
                                }
                                else
                                {
                                    lblMensaje.Text = errores;
                                    lblMensaje.Visible = true;
                                }

                            }

                        }
                        catch (Exception ex1)
                        {
                            lblMensaje.Text = "Error al procesar la ficha " + ex1.InnerException.ToString();
                            lblMensaje.Visible = true;
                        }


                    }
                    else
                    {
                        lblMensaje.Text = "Nro. de ficha no encontrada ";
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    lblMensaje.Text = "Error codigo invalido";
                    lblMensaje.Visible = true;
                }
            }
            catch (WebException ex)
            {
                lblMensaje.Text = "Error al conectarse a Andes" + ex.Message.ToString();
                lblMensaje.Visible = true;
            }
        }

        private string ControlCodigo()
        {
            string s_codigo = txtCodigo.Text;
            try {
               
                string result = string.Concat(s_codigo.Where(c => Char.IsDigit(c)));
                if (result == "")
                {
                    s_codigo = "";
                   
                }
                else
                    s_codigo = int.Parse(result).ToString();
               }
            catch (Exception ex) {
                lblMensaje.Text = "Error codigo ingresado:" + ex.Message.ToString();
                lblMensaje.Visible = true;
            }
            return s_codigo;
        }

        private string  Validacion(FFEE respuesta_d)
        {
            string s_usuario = oUser.IdUsuario.ToString();
            string idTipoMuestra = "";

            string errores = "";

            if (respuesta_d.organizacion != null)
            {
                if (BuscarEfector(respuesta_d.organizacion) == "0")
                {

                    errores += " No existe vinculacion entre la organizacion " + respuesta_d.organizacion + " y los efectores del sistema";
                }
            }
            else
                errores += " No es posible grabar sin datos de organizacion";

            if  (respuesta_d.tipomuestra == null)
            {
                idTipoMuestra = "18"; // por defecto hisopado
               // errores += " No es posible grabar sin tipo de muestra";
            }
            else
            {
                idTipoMuestra = BuscarMuestra(respuesta_d.tipomuestra);
                if (idTipoMuestra == "0")
                {
                    
                    errores += " No existe vinculacion entre la muestra informada " + respuesta_d.tipomuestra + " y las muestras del sistema";
                }
            }

            //if tipoform='UMA' entonces clasificacionmonitorio
            string idcarac="";
            if (respuesta_d.Tipo_ficha=="UMA") idcarac = "26";

            if ((respuesta_d.clasificacion != null) &&  (respuesta_d.Tipo_ficha != "UMA"))
            {
                  idcarac = BuscarCaracter(respuesta_d.clasificacion);
                if (idcarac == "0")
                {

                    errores += " No existe vinculacion entre la clasificacion " + respuesta_d.clasificacion + " y los caracteres del sistema";
                }
                if ((idcarac == "4") && (respuesta_d.Paciente_personalSalud=="NO") && (respuesta_d.Paciente_personalSeguridad=="NO") && (respuesta_d.Paciente_trabajaInstitucion=="NO" ))
                {

                    errores += " Ingreso rechazado. La ficha corresponde a Contacto Estrecho y No corresponden por algoritmo";
                }

            }
           
            if (idcarac=="")
                errores += " No es posible grabar sin datos de clasificacion";
            //if (BuscarEfector(respuesta_d.Paciente_documento.isn) == "0")
            //{
            //    valida = false;
            //errores += " El numero de documento " + respuesta_d.Paciente_documento + "no es un numero correcto";
            //}



            Paciente pac = new Paciente();
            int  id = pac.VerificaExistePaciente(respuesta_d.Paciente_estado, respuesta_d.Paciente_documento, respuesta_d.Paciente_apellido, respuesta_d.Paciente_nombre, respuesta_d.Paciente_sexo, respuesta_d.Paciente_fec_nacimiento,
                respuesta_d.direccioncaso,respuesta_d.localidadresidencia, s_usuario);
            if (id == -1)
            { // error warning deja grabar igual
                
                errores += " No es valido el documento para el estado del paciente: " + respuesta_d.Paciente_documento;
               
            }

            else
            {
                if ((int.Parse(idTipoMuestra) != 0) && (id>0))
                {
                    Paciente oPaciente = new Paciente();
                    oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), id);

                    string tieneingreso = oPaciente.GetFechaProtocolosRecientexEfector("3",  idTipoMuestra, oUser.IdEfector.IdEfector.ToString());
                    if (tieneingreso == DateTime.Now.ToShortDateString())
                        errores += " El paciente ya tiene ingresada la misma muestra en el día de la fecha. Verifique";
                     
                }
            }
             
                hdIdPaciente.Value = id.ToString();


            // datos requeridos del protocolo
            if ((respuesta_d.Fecha=="") || (respuesta_d.Fecha ==null))
            {  
                
                errores += " No es posible grabar sin fecha de ficha";
            }
            //if ((respuesta_d.fechamuestra == "")|| (respuesta_d.fechamuestra == null))
            //{
            //    respuesta_d.fechamuestra = respuesta_d.Fecha;
            //   //  errores += " No es posible grabar sin fecha de toma de muestra";
            //}
            //if ((respuesta_d.fechasintomas == "")|| (respuesta_d.fechasintomas == null))
            //{
                
            //    errores += " No es posible grabar sin fecha de inicio de sintomas";
            //}
           
           
           
            return errores;
        }

        private string BuscarCaracter(string clasificacion)
        {
            string s_idcaracter = "0";


            string[] tabla = hdCaracteres.Value.Split('@');

            /////Crea nuevamente los detalles.
            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                if (tabla[i].ToString() != "")
                {
                    string[] e = tabla[i].Split('#');
                    string org = e[1].ToString();
                    string efe = e[2].ToString();
                    if (org == clasificacion)
                    {
                        s_idcaracter = efe;
                        break;
                    }
                }
            }

            return s_idcaracter;
        }

        private void ProcesaFicha(FFEE respuesta_d)
        {
            // guarda Paciente
            Utility oUtil = new Utility();
            //instancio el usuario
            //Usuario us = new Usuario();
            //us = (Usuario)us.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));


           
            //datos del Paciente           
            Paciente pac = new Paciente();
          int  id =int.Parse( hdIdPaciente.Value);
            if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);

            if ((id != 0) && (pac.IdEstado == 3)) // validado por renaper
            { }
                else
            {
                if (respuesta_d.Paciente_estado == "validado")
                {
                    pac.IdEstado = 3;
                    pac.NumeroDocumento = int.Parse(respuesta_d.Paciente_documento);
                }
                // falta ver que hacer cuando es temporal (extranjero)
                else
                {
                    if ((respuesta_d.Paciente_estado == "temporal") && (respuesta_d.Paciente_documento.Length > 5))

                    {
                        pac.IdEstado = 1;
                        pac.NumeroDocumento = int.Parse(respuesta_d.Paciente_documento);
                    }
                    else
                    {
                        pac.IdEstado = 2;
                        pac.IdMotivoni = 0;
                        pac.NumeroDocumento = pac.generarNumero();
                        if (respuesta_d.Paciente_numeroIdentificacion != "Sin numero de identificacion")
                            pac.NumeroAdic = respuesta_d.Paciente_numeroIdentificacion;
                    }
                }


                pac.IdEfector = oC.IdEfector;   //us.IdEfector;
                pac.Apellido = oUtil.SacaComillas(respuesta_d.Paciente_apellido.Trim().ToUpper());
                pac.Nombre = oUtil.SacaComillas(respuesta_d.Paciente_nombre.Trim().ToUpper());

                pac.IdSexo = 1;
                pac.IdSexoLegal = 4;
                if (respuesta_d.Paciente_sexo.ToUpper() == "FEMENINO")
                {
                    pac.IdSexo = 2;
                    pac.IdSexoLegal = 2;
                }

                if (respuesta_d.Paciente_sexo.ToUpper() == "MASCULINO")
                {
                    pac.IdSexo = 3;
                    pac.IdSexoLegal = 3;
                }
           
                  
                
                pac.FechaAlta = DateTime.Now;
                //valido que la fecha no se mayor a la actual
                if ((Convert.ToDateTime(respuesta_d.Paciente_fec_nacimiento) <= DateTime.Now) && (respuesta_d.Paciente_fec_nacimiento != null))
                {
                    pac.FechaNacimiento = Convert.ToDateTime(respuesta_d.Paciente_fec_nacimiento);
                }

                pac.IdPais = -1;//;Convert.ToInt32(ddlNacionalidad.SelectedValue);
                pac.IdProvincia = -1;



            }

            if (respuesta_d.Paciente_genero != "")

            {
                pac.IdGenero = pac.getGeneroAndes(respuesta_d.Paciente_genero.ToUpper());
                pac.NombreAutopercibido = oUtil.SacaComillas(respuesta_d.Paciente_nombre_autopercibido);
            }

            string domi = "";

            if (respuesta_d.direccioncaso != null)

                domi = respuesta_d.direccioncaso.ToUpper();

            if (respuesta_d.barrioresidencia!=null)
                domi =  domi + " " + respuesta_d.barrioresidencia.ToUpper();

            if (domi.Length >= 50)
                domi = domi.Substring(0, 50);

            pac.Calle = domi;
            pac.Numero = 0;
            pac.Piso = "";
            pac.Departamento = "";
            pac.Manzana = "";




            //if (btnConfirmar.Text != "Actualizar")
            if (respuesta_d.telefonocaso!= null)
                pac.InformacionContacto = oUtil.SacaComillas(respuesta_d.telefonocaso);

            pac.FechaDefuncion = Convert.ToDateTime("01/01/1900");
            pac.IdUsuario = oUser.IdUsuario;
            pac.FechaUltimaActualizacion = DateTime.Now;
            pac.Save();
            

            //if (idEstado.Value == "3")
            //if ((respuesta_d.direccioncaso != null) && (respuesta_d.barrioresidencia != null) && (respuesta_d.localidadresidencia != null) )
                if ((respuesta_d.direccioncaso != null) && (respuesta_d.localidadresidencia != null))
                {
                if (respuesta_d.barrioresidencia == null)
                    respuesta_d.barrioresidencia = "";
                if (respuesta_d.direccioncaso.Length > 100)   respuesta_d.direccioncaso = respuesta_d.direccioncaso.Substring(0, 99);
                if (respuesta_d.barrioresidencia.Length > 100) respuesta_d.barrioresidencia = respuesta_d.barrioresidencia.Substring(0, 99);
                if (respuesta_d.localidadresidencia.Length > 100) respuesta_d.localidadresidencia = respuesta_d.localidadresidencia.Substring(0, 99);

                pac.GuardarDomicilio(DateTime.Today.ToShortDateString(), respuesta_d.direccioncaso, respuesta_d.barrioresidencia, respuesta_d.localidadresidencia, "", "", "");
              //  GuardarFoto(pac);
            }

            int numeroProtocolo= GuardarProtocolo(pac,respuesta_d);
            Session["Etiquetadora"] = ddlImpresora.SelectedValue;
            Response.Redirect("DefaultFFEE.aspx?idServicio=3&idUrgencia=0&Numero=" + numeroProtocolo.ToString());

        }

        private void GuardarDiagnosticos(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            

                    ProtocoloDiagnostico oDetalle = new ProtocoloDiagnostico();
                    oDetalle.IdProtocolo = oRegistro;
                    oDetalle.IdEfector = oRegistro.IdEfector;
                    if ((oRegistro.IdCaracter==1) || (oRegistro.IdCaracter == 26))
                        oDetalle.IdDiagnostico = 3562;
                    else
                        oDetalle.IdDiagnostico = 12458; // 999.7 sin registro
            oDetalle.Save();
                    
                

        }


        private int  GuardarProtocolo(Paciente oPaciente, FFEE respuesta_d)
        {
          Efector oEfector = new Efector();
       //     Usuario oUser = new Usuario();

            Protocolo oRegistro = new Protocolo();
            ObraSocial oObra = new ObraSocial();
            Origen oOrigen = new Origen();
            Prioridad oPri = new Prioridad();
            DateTime fecha = DateTime.Parse(respuesta_d.Fecha);

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.IdEfector = oC.IdEfector;
            SectorServicio oSector = new SectorServicio();
            oSector = (SectorServicio)oSector.Get(typeof(SectorServicio),  oC.IdSectorDefecto);
            oRegistro.IdSector = oSector;

            TipoServicio oServicio = new TipoServicio();
            oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(Session["idServicio"].ToString()));
            oRegistro.IdTipoServicio = oServicio;



            oRegistro.Numero = 0; // oRegistro.GenerarNumero();
            /*   oRegistro.NumeroDiario = oRegistro.GenerarNumeroDiario(fecha.ToString("yyyyMMdd"));
               oRegistro.PrefijoSector = oSector.Prefijo.Trim();
               oRegistro.NumeroSector = oRegistro.GenerarNumeroGrupo(oSector);
               oRegistro.NumeroTipoServicio = oRegistro.GenerarNumeroTipoServicio(oServicio);*/

            bool grabarincidenciafis = false;
            bool grabarincidenciafuc = false;

            oRegistro.FechaInicioSintomas = DateTime.Parse("01/01/1900");
            oRegistro.FechaUltimoContacto = DateTime.Parse("01/01/1900");
          
            string idCar = BuscarCaracter(respuesta_d.clasificacion);
            if (respuesta_d.Tipo_ficha == "UMA") idCar = "26";
            

            oRegistro.IdCaracter = int.Parse(idCar);

            if (idCar == "4") // contacto se guarda en fuc
            {
                if ((respuesta_d.fechasintomas == "") || (respuesta_d.fechasintomas == null)) grabarincidenciafuc = true;
                else oRegistro.FechaUltimoContacto  = DateTime.Parse(respuesta_d.fechasintomas);
            }
            else
            { if ((respuesta_d.fechasintomas == "") || (respuesta_d.fechasintomas == null))  grabarincidenciafis = true;
                else oRegistro.FechaInicioSintomas = DateTime.Parse(respuesta_d.fechasintomas);
            }



            //if (ddlCaracter.SelectedValue == "4")  //CONTACTO
            //{
            //    if (chkSinFUC.Checked) grabarincidenciafuc = true;
            //    else oRegistro.FechaUltimoContacto = DateTime.Parse(txtFechaFUC.Value);
            //    //GRABAR fUC
            //}
            oRegistro.Fecha =DateTime.Parse( DateTime.Now.ToShortDateString());
            oRegistro.FechaOrden = DateTime.Parse(respuesta_d.Fecha);
            if (respuesta_d.fechamuestra== null)
                oRegistro.FechaTomaMuestra = DateTime.Parse(respuesta_d.Fecha);
            else
               oRegistro.FechaTomaMuestra = DateTime.Parse(respuesta_d.fechamuestra);
            oRegistro.FechaRetiro = DateTime.Parse("01/01/1900"); //DateTime.Parse(txtFechaEntrega.Value);

            string idefectororganizacion = BuscarEfector(respuesta_d.organizacion);
            oRegistro.IdEfectorSolicitante = (Efector)oEfector.Get(typeof(Efector), int.Parse(idefectororganizacion));
            oRegistro.IdEspecialistaSolicitante = 0;


            if (respuesta_d.usuario != null)
            {
                oRegistro.MatriculaEspecialista = "9999";
                oRegistro.Especialista = respuesta_d.usuario;
            }
            else
            {
                oRegistro.MatriculaEspecialista = "0";
                oRegistro.Especialista = "No identificado";
            }
            

                ///Desde aca guarda los datos del paciente en Protocolo
                oRegistro.IdPaciente = oPaciente;


            oRegistro.Edad = 0; // oRegistro. int.Parse(lblEdad.Text);

            oRegistro.NumeroOrigen = "";
            oRegistro.NumeroOrigen2 = respuesta_d.identificadorpcr.Replace("HISOP00", "").Replace("HISOP0", "").Replace("HISOP", "");
 
                oRegistro.UnidadEdad = 0;  

            //oRegistro.Sexo = lblSexo.Text;

            switch (oPaciente.IdSexo)
            {
                case 1: oRegistro.Sexo = "I"; break;
                case 2: oRegistro.Sexo = "F"; break;
                case 3: oRegistro.Sexo = "M"; break;
            }


            oRegistro.Embarazada = "N";
            oRegistro.Sala = ""; // txtSala.Text;
            oRegistro.Cama = ""; // txtCama.Text;


            ObraSocial oObraSocial = new ObraSocial();
            oRegistro.IdObraSocial = (ObraSocial)oObraSocial.Get(typeof(ObraSocial), -1);


            string nombreOS = "";
            string codigoOS = "0";
            string[] os = GetPuco(oPaciente.NumeroDocumento).Split('&');
            if (os.Length > 1)
            {
                nombreOS = os[0].ToString();
                codigoOS = os[1].ToUpper();
               // CodOS.Value = codigoOS;
            }

            //lblObraSocial.Text = nombreOS; // oObraSocial.Nombre;

            oRegistro.NombreObraSocial = nombreOS;
            oRegistro.CodOs = int.Parse(codigoOS);
            int ID_ORIGEN = 12;  // no informado

            if (respuesta_d.requerimientocuidado != null)
            {
                if (respuesta_d.requerimientocuidado.ToUpper() == "AMBULATORIO")
                    ID_ORIGEN = 1;

                if (respuesta_d.requerimientocuidado.ToUpper().Contains("INTERNA") )
                    ID_ORIGEN = 2;
            }
            if (respuesta_d.Tipo_ficha == "UMA") ID_ORIGEN = 1; //AMBULATORIO


            oRegistro.Notificarresultado = true;
            oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), ID_ORIGEN);
            oRegistro.IdPrioridad = (Prioridad)oPri.Get(typeof(Prioridad), 1);
            if (respuesta_d.antigeno != null)
            {
                oRegistro.Observacion = "Antigeno:" + respuesta_d.antigeno;
                if (respuesta_d.antigeno == "Reactivo")
                { oRegistro.Notificarresultado = false; }
            }
            else
            {
                oRegistro.Notificarresultado = true;
            }
            oRegistro.ObservacionResultado = "";
            

            string ID_MUESTRA = "18"; // por defecto hisopado
            if (respuesta_d.tipomuestra != null)
            { ID_MUESTRA = BuscarMuestra(respuesta_d.tipomuestra); }
            oRegistro.IdMuestra = int.Parse(ID_MUESTRA);



            oRegistro.IdUsuarioRegistro = oUser; ///(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.IpCarga = "";
            oRegistro.Impres = ddlImpresora.SelectedValue;


           
            oRegistro.Estado = 0;
            oRegistro.Save();

            oRegistro.ActualizarNumeroDesdeID();


            string tabla  = BuscarDeterminaciones(respuesta_d.Tipo_ficha);
            GuardarDetalle(oRegistro, tabla );

            oRegistro.IdPaciente.ActualizarEdadProtocolo();

            // no grabo incidencias por que va a estar controlado el ingreso de fechas
            //if (grabarincidenciafis)
            //    oRegistro.GeneraIncidenciaAutomatica(46, int.Parse(Session["idUsuario"].ToString()));
            //if (grabarincidenciafuc)
            //    oRegistro.GeneraIncidenciaAutomatica(47, int.Parse(Session["idUsuario"].ToString()));
            //else
            GuardarDiagnosticos(oRegistro);


            //if ((!grabarincidenciafis) && (!grabarincidenciafuc))
            //    oRegistro.BorrarIncidenciasFISyFUC(int.Parse(Session["idUsuario"].ToString()));

            oRegistro.GrabarAuditoriaDetalleProtocolo("Graba desde FFEE", oUser.IdUsuario, respuesta_d.identificadorpcr, "");

            oRegistro.ImprimirCodigoBarras(ddlImpresora.SelectedItem.Text, oUser.IdUsuario);


            return oRegistro.Numero;
        }


        private void CargarEfectores()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = "select ltrim(idAndes), idsil from Rel_andes where tipo='Efector'";
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

            
            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                sTareas += "#" + ds.Tables["T"].Rows[i][0].ToString() + "#" + ds.Tables["T"].Rows[i][1].ToString()  + "@";
            }
            hdEfectores.Value = sTareas;

          
            m_ssql = null;
            oUtil = null;
        }

        private void CargarTipoMuestra()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = "select nombreAndes, idsil from Rel_andes where tipo='Tipo Muestra'";
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");


            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                sTareas += "#" + ds.Tables["T"].Rows[i][0].ToString() + "#" + ds.Tables["T"].Rows[i][1].ToString() + "@";
            }
            hdTipoMuestra.Value = sTareas;


            m_ssql = null;
            oUtil = null;
        }

        private void CargarCaracteres()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = "select nombreAndes, idsil from Rel_andes where tipo='Caracter'";
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");


            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                sTareas += "#" + ds.Tables["T"].Rows[i][0].ToString() + "#" + ds.Tables["T"].Rows[i][1].ToString() + "@";
            }
            hdCaracteres.Value = sTareas;


            m_ssql = null;
            oUtil = null;
        }

        private string BuscarEfector(string organizacion)
        {
            string s_idEfector="0";


            string[] tabla = hdEfectores.Value.Split('@');

            /////Crea nuevamente los detalles.
            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                string[] e = tabla[i].Split('#');
                string org = e[1].ToString();
                string efe = e[2].ToString();
                if (org == organizacion)
                {
                    s_idEfector = efe;
                    break;
                }
            }



            

            return s_idEfector;

        }
        private string BuscarMuestra(string muestra)
        {
            string s_idTipoMuestra = "0";


            string[] tabla = hdTipoMuestra.Value.Split('@');

            /////Crea nuevamente los detalles.
            for (int i = 0; i <= tabla.Length - 1; i++)
            {
                if (tabla[i].ToString() != "")
                {
                    string[] e = tabla[i].Split('#');
                    string and = e[1].ToString();
                    string sil = e[2].ToString();
                    if (and.ToUpper() == muestra.ToUpper())
                    {
                        s_idTipoMuestra = sil;
                        break;
                    }
                }
            }
            return s_idTipoMuestra;

        }

        private void GuardarDetalle(Business.Data.Laboratorio.Protocolo oRegistro, string Det)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;

          
            //int dias_espera = 0;
          //  string[] tabla = "1#9122#Si#False".Split('@');   // TxtDatosCargados.Value.Split('@');

            string[] tabla =  Det.Split('@');
            string recordar_practicas = "";

            for (int i = 0; i < tabla.Length; i++)
            {
                string[] fila = tabla[i].Split('#');


                string codigo = fila[1].ToString();


                if (recordar_practicas == "")
                    recordar_practicas = codigo + "#Si#False";
                else
                    recordar_practicas += ";" + codigo + "#Si#False";

                if (codigo != "")
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigo, "Baja", false);

                    //string trajomuestra = fila[3].ToString();

                    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                    crit.Add(Expression.Eq("IdProtocolo", oRegistro));
                    crit.Add(Expression.Eq("IdItem", oItem));
                    IList listadetalle = crit.List();
                    if (listadetalle.Count == 0)
                    { //// si no está lo agrego --- si ya está no hago nada


                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        //Item oItem = new Item();
                        oDetalle.IdProtocolo = oRegistro;
                        oDetalle.IdEfector = oRegistro.IdEfector;



                        oDetalle.IdItem = oItem; // (Item)oItem.Get(typeof(Item), "Codigo", codigo);
                        oDetalle.IdSubItem = oItem;
                        
                        oDetalle.TrajoMuestra = "Si";


                        oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                        oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                        oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                        oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                        oDetalle.Informable = oItem.Informable;
                       
                         

                        oDetalle.Save();
                        


                         GuardarDetallePractica(oDetalle);
                    }
                    else  //si ya esta actualizo si trajo muestra o no
                    {
                        foreach (DetalleProtocolo oDetalle in listadetalle)
                        {
                            //if (trajomuestra == "true")
                            //    oDetalle.TrajoMuestra = "No";
                            //else
                            oDetalle.TrajoMuestra = "Si";

                            oDetalle.Save();
                        }

                    }
                }
            }
 
            oRegistro.FechaRetiro = oRegistro.Fecha.AddDays(0);
            oRegistro.Estado = 0;




            oRegistro.Save();


        }

        private void GuardarDetallePractica(DetalleProtocolo oDet)
        {
            if (oDet.IdItem.IdEfector.IdEfector != oDet.IdItem.IdEfectorDerivacion.IdEfector) //Si es un item derivable no busca hijos y guarda directamente.
            {
                oDet.IdSubItem = oDet.IdItem;
                oDet.Save();
            }
            else
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                crit.Add(Expression.Eq("IdItemPractica", oDet.IdItem));            
                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    int i = 1;
                    foreach (PracticaDeterminacion oSubitem in detalle)
                    {
                        if (oSubitem.IdItemDeterminacion != 0)
                        {
                            Item oSItem = new Item();
                            oSItem = (Item)oSItem.Get(typeof(Item), oSubitem.IdItemDeterminacion);
                            if (i == 1)
                            {
                                oDet.IdSubItem = oSItem;
                                oDet.Save();
                            }
                            else
                            {
                                DetalleProtocolo oDetalle = new DetalleProtocolo();
                                oDetalle.IdProtocolo = oDet.IdProtocolo;
                                oDetalle.IdEfector = oDet.IdEfector;
                                oDetalle.IdItem = oDet.IdItem;
                                oDetalle.IdSubItem = oSItem;
                                oDetalle.TrajoMuestra = oDet.TrajoMuestra;
                                oDetalle.Informable = oSItem.Informable;


                                oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
                                oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                                oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                                oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                                oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                                oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                                oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                                oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");

                                oDetalle.Save();
                                oDetalle.GuardarValorReferencia();
                            }
                            i = i + 1;
                        }//fin if
                    }//fin foreach
                }
                else
                {
                    oDet.IdSubItem = oDet.IdItem;
                    oDet.Informable = oDet.IdSubItem.Informable;
                    oDet.Save();
                    oDet.GuardarValorReferencia();
                }//fin   if (detalle.Count > 0)  
            }



        }


        private string BuscarDeterminaciones(string Tipo_ficha)
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = "select nombresil  from Rel_andes where tipo='Determinacion' and [nombreAndes]='covid19'";
            if (Tipo_ficha == "UMA")
              m_ssql = "select nombresil  from Rel_andes where tipo='Determinacion' and [nombreAndes]='DerivacionOVR'"; //UMA
            else
                m_ssql = "select nombresil  from Rel_andes where tipo='Determinacion' and [nombreAndes]='covid19'";
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "T");

            m_ssql = null;
            oUtil = null;
            string sTareas = "";
            for (int i = 0; i < ds.Tables["T"].Rows.Count; i++)
            {
                //string[] tabla = "1#9122#Si#False".Split
                if (sTareas=="")
                    sTareas  = "1#" + ds.Tables["T"].Rows[i][0].ToString() + "#Si#False";
                else 
                sTareas += "@"+ (i+1).ToString() + "#"+ ds.Tables["T"].Rows[i][0].ToString() + "#Si#False";
            }
            return                 sTareas;


          
        }

        public string GetPuco(int numeroDocumento)
        {
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
            string codigo = "";
            string nombre = "";
            connetionString = ConfigurationManager.ConnectionStrings["Puco"].ConnectionString;

            ; // "Data Source=ServerName;Initial Catalog=DatabaseName;User ID=UserName;Password=Password";
            sql = "select S.nombre, S.cod_os as os from pd_puco P inner join obras_sociales S on S.cod_os=P.codigoOS where P.dni = " + numeroDocumento.ToString();

            connection = new SqlConnection(connetionString);
            try
            {
                SqlDataReader rdr = null;
                connection.Open();
                command = new SqlCommand(sql, connection);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {
                    nombre = rdr[0].ToString();
                    codigo = rdr[1].ToString();
                }
                if (nombre == "") nombre = "Sin obra social"; // sin obra social
                else nombre = nombre + "&" + codigo;
                command.Dispose();
                connection.Close();
            }
            catch (Exception ex)
            {
                nombre = "Sin obra social&0";
            }

            return nombre;
        }

        public class FFEE
        {
            
                   public string identificadorpcr { get; set; }
            public string Paciente_estado { get; set; }
            
            public string Paciente_nombre { get; set; }
            public string Paciente_apellido { get; set; }
            public string Paciente_documento { get; set; }
            public string Paciente_fec_nacimiento { get; set; }
            public string Paciente_sexo { get; set; }

            public string Fecha { get; set; } // feCHA DE ORDEN
            public string organizacion { get; set; } // EFECTOR
            public string telefonocaso { get; set; } // TELEFONO PACIENTE

            public string direccioncaso { get; set; } // DIRECCION PACIENTE

            public string localidadresidencia { get; set; } // localidadresidencia PACIENTE

            public string barrioresidencia { get; set; } // TELEFONO PACIENTE

            public string fechasintomas { get; set; } // TELEFONO PACIENTE

            public string requerimientocuidado { get; set; } // AMBULATORIO O IINTERNACION  
            public string casofallecido { get; set; } // SI ES SI SE INGRESA CARACTER= FALLECIDO
            public string fechamuestra { get; set; } // TELEFONO PACIENTE

            public string tipomuestra { get; set; } // TELEFONO PACIENTE

            public string antigeno { get; set; } // antigeno

            public string usuario { get; set; } // usuario-solicitante

            public string Paciente_numeroIdentificacion { get; set; } // numero de identificacion cuando es temporal

            public string clasificacion { get; set; } // clasificacion

            public string Paciente_genero { get; set; }

            public string Paciente_nombre_autopercibido { get; set; }

            public string Paciente_personalSalud { get; set; }

            public string Paciente_personalSeguridad { get; set; }

            public string Paciente_trabajaInstitucion { get; set; }


            public string Tipo_ficha { get; set; }


            public string derivacionovr { get; set; }

            



        }


        protected void lnkAmpliarFiltros_Click(object sender, EventArgs e)
        {
            //if (lnkAmpliarFiltros.Text == "Ampliar filtros de búsqueda")
            //{
            //    lnkAmpliarFiltros.Text = "Ocultar filtros adicionales";
            //    pnlParentesco.Visible = true;
            //}
            //else
            //{
            //    lnkAmpliarFiltros.Text = "Ampliar filtros de búsqueda";
            //    pnlParentesco.Visible = false;
            //}

            //lnkAmpliarFiltros.UpdateAfterCallBack = true;
            //pnlParentesco.UpdateAfterCallBack = true;
        }

        protected void cvFecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //try
            //{
            //    if (txtFechaNac.Value != "")
            //    {
            //        DateTime f1 = DateTime.Parse(txtFechaNac.Value);
            //        args.IsValid = true;
            //    }
            //    else                  
            //        args.IsValid = true;
            //}
            //catch (Exception ex)
            //{
            //    string exception = "";
            //    //while (ex != null)
            //    //{
            //        exception = ex.Message + "<br>";

            //    //} 
            //args.IsValid = false;
            //}
        }

        protected void btnFinalizarCaso_Click(object sender, EventArgs e)
        {
            CrystalReportSource oCr = new CrystalReportSource();
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));


            ParameterDiscreteValue nrocaso = new ParameterDiscreteValue();
            nrocaso.Value = oRegistro.IdCasoFiliacion.ToString();

            ParameterDiscreteValue nombre = new ParameterDiscreteValue();
            nombre.Value = oRegistro.IdCasoFiliacion.ToString() + " " + oRegistro.Nombre;


            oCr.Report.FileName = "..\\CasoFiliacion\\CaratulaFiliacion.rpt";
          
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(nrocaso);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(nombre);


            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Caratula");
        }

     

      

    }
}
