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
using System.Net.Mail;

namespace WebLab.Pacientes
{
    public partial class PacienteEdit2 : System.Web.UI.Page
    {
        private String TicketUsuario = "";
        //private String TicketSistema = "";
        private String operador = "";

        //este codigo debe representar a la aplicación cliente
        //se usa el usuario "USUARIO-TEST" preparado para realizar pruebas
        String SistemaCliente = ConfigurationManager.AppSettings["SistemaCliente"].ToString(); //"SISTEMA_LAB_CENTRAL";
        String PasswordSistemaCliente = ConfigurationManager.AppSettings["PasswordSistemaCliente"].ToString();//"Gogo**885";

        Configuracion oCon = new Configuracion();

        

        

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            if (Request["master"] == null)
                Page.MasterPageFile = "~/Site1.master";
            else
                Page.MasterPageFile = "~/Site2.master";


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarListas();
                if (Request["Sexo"].ToString() == "I") ///
                {
                    MostrarDatosPaciente();
                }
                else
                {
                    if (Request["master"] != null)
                        btnConfirmar.Text = "Actualizar";
                    else btnConfirmar.Visible = true;

                    if (Request["Tipo"].ToString() == "T")
                    {
                        HabilitaTemporal();

                    }
                    else
                    {
                        try
                        {
                            //IntAutenticacion.AutenticacionClient cliente =
                            // new IntAutenticacion.AutenticacionClient();

                            //TicketUsuario = cliente.LoginPecas(SistemaCliente, PasswordSistemaCliente);

                            //if (TicketUsuario != "")
                            //{
                            if (oCon.ConectaRenaper)
                            {



                                SolicitarServicio();

                            }
                        }
                        catch (Exception ex)
                        {
                            lblMensaje.Text = ex.Message.ToString();
                            lblMensaje.Visible = true;
                            if (Request["id"] != null)
                                MostrarDatosPaciente();
                            //carga manual de afiliado
                            HabilitaCargaManual();
                            // string redirectPaciente = ConfigurationManager.AppSettings["urlPaciente"].ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString();
                            // Response.Redirect(redirectPaciente, false);
                            //mensaje = "ocurrió una exception al solicitar el servicio:" + ex.Message;
                        }
                    }

                }
            }
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga de combos de estado
            string m_ssql = "select idEstado, nombre from Sys_Estado with (nolock)  where idestado!=3 ";
            oUtil.CargarCombo(ddlEstadoP, m_ssql, "idEstado", "nombre", connReady);             


            ////////////Carga de combos de sexo biologico
            m_ssql = "SELECT idSexo, nombre FROM sys_Sexo with (nolock)  order by nombre ";
            oUtil.CargarCombo(ddlSexo, m_ssql, "idSexo", "nombre", connReady);
            ddlSexo.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            m_ssql = "SELECT idSexoLegal, nombre FROM sys_SexoLegal with (nolock)  order by nombre ";
            oUtil.CargarCombo(ddlSexoLegal, m_ssql, "idSexoLegal", "nombre", connReady);
            ddlSexoLegal.Items.Insert(0, new ListItem("--Seleccione--", "0"));


            m_ssql = "SELECT idGenero, nombre FROM sys_Genero with (nolock)  order by nombre ";
            oUtil.CargarCombo(ddlGenero, m_ssql, "idGenero", "nombre", connReady);
            ddlGenero.Items.Insert(0, new ListItem("----", "0"));

            m_ssql = null;
            oUtil = null;
        

    }
    private void MostrarDatosPaciente()
        {

            Paciente pac = new Paciente();
            pac = (Paciente)pac.Get(typeof(Paciente), int.Parse(Request["id"].ToString()));
            Image1.Visible = false;
            HFIdPaciente.Value = pac.IdPaciente.ToString();
            HFNumeroDocumento.Value = pac.NumeroDocumento.ToString();
            switch (pac.IdSexoLegal)
            {
                case 1: ddlSexoLegal.SelectedValue = "1"; break;
                case 2: ddlSexoLegal.SelectedValue="2"; HFSexo.Value = "F"; break;
                case 3: ddlSexoLegal.SelectedValue = "3"; HFSexo.Value = "M"; break;
            }
            ddlSexo.SelectedValue = pac.IdSexo.ToString();
            ddlGenero.SelectedValue = pac.IdGenero.ToString();
            txtNombreAutopercibido.Value = pac.NombreAutopercibido;

            btnValidarRenaper.Visible = false;
            if (pac.IdEstado != 2) //temporal
            {
                txtDNI.Text = pac.NumeroDocumento.ToString();
                
             
                if (pac.IdEstado == 1)
                {
                    btnValidarRenaper.Visible = true;
                    pnlRenaper.Visible = false;
                }
                if (pac.IdEstado == 3)
                {
                    btnValidarRenaper.Visible = true;
                    pnlRenaper.Visible = true;
                    fechaDomicilio.Visible = true;
                }
            }
            else
            {
                MuestraTemporal();
                ddlMotivoNI.SelectedValue = pac.IdMotivoni.ToString();
                lblTemporal.Text ="Temporal: " + pac.NumeroDocumento.ToString();
                txtDNI.Text = "";
            }

            txtNumeroAdic.Value = pac.NumeroAdic;
            idEstado.Value = pac.IdEstado.ToString();
            txtApellido.Text = pac.Apellido;
            txtNombre.Text = pac.Nombre;
            txtFechaNacimiento.Value = pac.FechaNacimiento.ToShortDateString() ;
            ddlEstadoP.SelectedValue = pac.IdEstado.ToString();
            txtTelefono.Value = pac.InformacionContacto;
         
             
            txtMail.Value = pac.Mail;
            ddlRaza.SelectedValue = pac.IdRaza.ToString();
            if (pac.SeDeclaraAborigen) ddlAborigen.SelectedValue = "1"; else ddlAborigen.SelectedValue = "0";

            if (pac.IdEstado == 3)
            {
                ddlEstadoP.Items.Clear();
                ddlEstadoP.Items.Insert(0, new ListItem("Validado", "3"));
                ddlEstadoP.Enabled = false;
             }
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DomicilioPaciente));
                crit.Add(Expression.Eq("IdPaciente", pac));
                IList detalle = crit.List();

                foreach (DomicilioPaciente oDom in detalle)
                {
                    fechaDomicilio.Text = oDom.FechaDomicilio.ToShortDateString();
                    txtCalle.Value = oDom.Calle;
                    //txtCuil.Value= pac
                    txtCiudad.Value = oDom.Ciudad;
                    txtProvincia.Value = oDom.Provincia;
                    txtPais.Value = oDom.Pais;
                    txtCodigoPostal.Value = oDom.Cpostal;
                    txtBarrio.Value = oDom.Barrio;
                }
            gvParentesco.DataSource = MostrarParentesco(pac); 
            gvParentesco.DataBind();
          
            //mostrarFoto(pac);

            //fallecimiento.Visible = false;
            //if (Request["sexo"].ToString() == "F")
            //    txtSexo.Value = "FEMENINO";
            //else txtSexo.Value = "MASCULINO";
        }

        private DataTable MostrarParentesco(Paciente pac)
        {
              
            string m_strSQL = "";
             
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();

           

            m_strSQL = @" select P.numeroDocumento as [Documento], p.apellido as [Apellido], p.nombre as [Nombre], Pa.nombre as Vinculo
from sys_parentesco P (nolock)
inner join LAB_Parentesco Pa (nolock) on P.tipoparentesco= Pa.idParentesco
where P.idPaciente=" + pac.IdPaciente.ToString();

            DataSet Ds1 = new DataSet();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1, "par");


            DataTable data = Ds1.Tables[0];
            return data;

 

    }

    private void MuestraTemporal()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura



            ///Carga de combos de motivo no identificación
            string m_ssql = "select idmotivoNI, nombre from Sys_MotivoNI ";
                oUtil.CargarCombo(ddlMotivoNI, m_ssql, "idmotivoNI", "nombre", connReady);  
                ddlMotivoNI.Items.Insert(0, new ListItem("--Seleccione motivo--", "0"));
                ddlMotivoNI.Enabled = true;
                txtDNI.Text = "";
                txtDNI.Enabled = false;
                //rfvDI.Enabled = false;
                lblMensaje.Text = "";
                rvMotivo.Enabled = true;
                rfvDocumento.Enabled = false;
            divNumeroAdicional.Visible = true;
            rfvDocumento.Enabled = false; 



        }

        //private void mostrarFoto(Paciente pac)
        //{

        //    string m_strSQL = " SELECT foto2 as imgFoto FROM sys_paciente WHERE idPaciente=" + pac.IdPaciente.ToString();


        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //    DataSet Ds = new DataSet();
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);

        //    DataTable dtPermisos = Ds.Tables[0];
        //    if (Ds.Tables[0].Rows.Count > 0)
        //    {

        //        //byte[] image = Encoding.UTF8.GetBytes(Image1.ImageUrl);
        //        byte[] imageBuffer = Encoding.UTF8.GetBytes(Ds.Tables[0].Rows[0]["imgFoto"].ToString());

        //        string imageBase64 = Convert.ToBase64String(imageBuffer);
        //        Image1.ImageUrl = imageBase64;
        //    }


        //}
        private void HabilitaCargaManual()
        {
           
            lblTemporal.Text = "Servicio de Renaper no disponible";
            txtDNI.Text = Request["dni"].ToString();
            idEstado.Value = "1"; // identificado
            txtApellido.Enabled = true;
            txtNombre.Enabled = true;
            txtFechaNacimiento.Disabled = false;
            txtCalle.Disabled = false;
            txtCuil.Disabled = false;
            txtCiudad.Disabled = false;
            txtProvincia.Disabled = false;
            txtPais.Disabled = false;
            txtCodigoPostal.Disabled = false;
            txtBarrio.Disabled = false;
            fallecimiento.Visible = false;
            //if (Request["sexo"].ToString() == "F")
            //    txtSexo.Value = "FEMENINO";
            //else txtSexo.Value = "MASCULINO";
        }

        private void HabilitaTemporal()
        {
            idEstado.Value = "2"; // temporal
         
            grupoDNI.Visible = false;
            lblTemporal.Visible = true;
            txtApellido.Enabled = true;
            txtNombre.Enabled = true;
            txtFechaNacimiento.Disabled = false;
            txtCalle.Disabled = false;
            txtCuil.Disabled = false;
            txtCiudad.Disabled = true;
            txtProvincia.Disabled = true;
            txtPais.Disabled = true;
            txtCodigoPostal.Disabled = true;
            txtBarrio.Disabled = true;
            fallecimiento.Visible = false;
            divNumeroAdicional.Visible = true;

        }

        

        private void SolicitarServicio()
        {
            string mensaje = "";
            try
            {
                 
                string Servicio = oCon.UrlRenaper;

                string parametros = Request["dni"].ToString() + "/" + Request["sexo"].ToString(); ;



                //   string urlCompleta = urlServidorXroadLocal + "/" + Servicio + "/" + parametros;
                string urlCompleta = Servicio + "/" + parametros;

                var request = (HttpWebRequest)WebRequest.Create(urlCompleta);

                string headerRenaper = ConfigurationManager.AppSettings["headerRenaper"].ToString();
                string[] a = headerRenaper.Split(':');
                string s_headerRenaper = a[0].ToString();
                string s_headerValorRenaper = a[1].ToString();

                request.Headers.Add(s_headerRenaper, s_headerValorRenaper);


                var response = (HttpWebResponse)request.GetResponse();

                string responseString = "";
                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        responseString = reader.ReadToEnd();


                    }
                }

                if (responseString != "")
                {
                    mensaje = "el servicio se resolvió con exito";

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    resRenaper res = jsonSerializer.Deserialize<resRenaper>(responseString);


                    //        //el resultado viene en 5 array de bytes

                    //        //transformo el array a string y lo muestro
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();


                    //  String Resultado1 = enc.GetString(res.resultado1);


                    string s = responseString;

                    
                        if (s != "0")
                        {
                        Persona persona_d = res.resultado1;  
                            if (persona_d.nroError == "2") //persona no encontrada
                            {
                                txtDNI.Enabled = true;
                                lblMensaje.Text = "No se encontraron datos para el numero de doc. y sexo de la persona en Renaper. Verifique.";
                                lblMensaje.Visible = true;
                                idEstado.Value = "1";
                            }
                            else
                            {
                                if ((persona_d.apellido == "") && (persona_d.nombres == ""))
                                {
                                    lblMensaje.Text = "No se encontraron datos para el numero y sexo de la persona en Renaper. Verifique.";
                                    idEstado.Value = "1";
                                }
                                else
                                {
                                    ddlEstadoP.SelectedValue = "3";
                                    ddlMotivoNI.SelectedValue = "0";
                                    txtDNI.Text = Request["dni"].ToString();
                                    txtApellido.Text = persona_d.apellido;
                                    txtNombre.Text = persona_d.nombres;
                                    txtFechaNacimiento.Value = persona_d.fechaNacimiento;
                                    txtCalle.Value = persona_d.calle + " " + persona_d.numero;
                                    txtCuil.Value = persona_d.cuil;
                                    if (persona_d.ciudad == "") txtCiudad.Value = "SIN DATOS";
                                    else txtCiudad.Value = persona_d.ciudad;

                                    if (persona_d.provincia == "") txtProvincia.Value = "SIN DATOS";
                                    else txtProvincia.Value = persona_d.provincia;
                                    if (persona_d.pais == "") txtPais.Value = "SIN DATOS";
                                    else txtPais.Value = persona_d.pais;
                                    if (persona_d.cpostal == "") txtCodigoPostal.Value = "SIN DATOS";
                                    else txtCodigoPostal.Value = persona_d.cpostal;
                                    if (persona_d.monoblock == "") txtBarrio.Value = "SIN DATOS";
                                    else
                                        txtBarrio.Value = persona_d.monoblock;
                                    fallecimiento.Text = persona_d.mensaf;
                                    fechaDomicilio.Text = persona_d.EMISION;
                                if (Request["sexo"].ToString() == "F")
                                {
                                    ddlSexo.SelectedValue = "2";
                                    ddlSexoLegal.SelectedValue = "2";
                                }
                                else
                                {
                                    ddlSexo.SelectedValue = "3";
                                    ddlSexoLegal.SelectedValue = "3";
                                }
                                    idEstado.Value = "3"; // validado con renaper
                                                          // System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
                                                          //string foti= codificador.GetString(persona_d.foto);
                                    Image1.ImageUrl = persona_d.foto;
                                    pnlRenaper.Visible = true;
                                    Image1.Visible = true;
                                /// traer al paciente si no es nuevo, es modificacion
                                int id = Convert.ToInt32(Request.QueryString["id"]);
                                //datos del Paciente           
                                Paciente pac = new Paciente();
                                if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);
                                txtTelefono.Value = pac.InformacionContacto;
                                HFIdPaciente.Value = id.ToString();
                            }
                             
                        }//res
                    }

                    else
                    {
                        lblMensaje.Text = "hubo algún error al solicitar el servicio: " + res.resultado1.mensaf.ToString();
                        lblMensaje.Visible = true;
                      
                        idEstado.Value = "1";
                        if (Request["id"] != null)
                            MostrarDatosPaciente();
                        HabilitaCargaManual();
                    }


                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "hubo algún error al solicitar el servicio: " + ex.Message;
                lblMensaje.Visible = true;
                idEstado.Value = "1";
                if (Request["id"] != null)
                    MostrarDatosPaciente();
                HabilitaCargaManual();
            }
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (validadatos())
                {
                    if (validamail())
                    {
                        Configuracion oC = new Configuracion();

                        oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);

                        Utility oUtil = new Utility();
                        //instancio el usuario
                        Usuario us = new Usuario();
                        us = (Usuario)us.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                        int id = Convert.ToInt32(Request.QueryString["id"]);
                        //datos del Paciente           
                        Paciente pac = new Paciente();
                        if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);


                        pac.IdEfector = oC.IdEfector; // us.IdEfector;
                        pac.Apellido = oUtil.SacaComillas(txtApellido.Text.ToUpper());
                        pac.Nombre = oUtil.SacaComillas(txtNombre.Text.ToUpper());



                        if (int.Parse(ddlEstadoP.SelectedValue) == 2)
                        {
                            pac.IdEstado = 2;
                            pac.IdMotivoni = int.Parse(ddlMotivoNI.SelectedValue);
                            if (id == 0)
                                pac.NumeroDocumento = pac.generarNumero();
                        }
                        else
                        {
                            if (idEstado.Value == "3")
                                pac.IdEstado = 3;
                            else
                                pac.IdEstado = int.Parse(ddlEstadoP.SelectedValue);
                            btnValidarRenaper.Visible = true;
                            pac.NumeroDocumento = int.Parse(txtDNI.Text);
                        }


                        if (btnConfirmar.Text != "Actualizar")
                            pac.FechaAlta = DateTime.Now;

                        bool actualizarSexoProtocolo = false;
                        //if (Request["sexo"].ToString() == "F")
                        if (id != 0)
                        {
                            if (pac.IdSexo != int.Parse(ddlSexo.SelectedValue))
                                actualizarSexoProtocolo = true;

                        }



                        pac.IdSexo = int.Parse(ddlSexo.SelectedValue);
                        pac.IdSexoLegal = int.Parse(ddlSexoLegal.SelectedValue);
                        pac.IdGenero = int.Parse(ddlGenero.SelectedValue);
                        pac.NombreAutopercibido = txtNombreAutopercibido.Value;
                        //else
                        //    pac.IdSexo = 3;
                        //valido que la fecha no se mayor a la actual

                        bool actualizarEdadProtocolo = false;
                        //if (Request["sexo"].ToString() == "F")
                        if (id != 0)
                        {
                            if (pac.FechaNacimiento != Convert.ToDateTime(txtFechaNacimiento.Value))
                                actualizarEdadProtocolo = true;

                        }

                        if ((Convert.ToDateTime(txtFechaNacimiento.Value) <= DateTime.Now) && (txtFechaNacimiento.Value != null))
                        {
                            pac.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Value);
                        }

                        pac.IdPais = -1;//;Convert.ToInt32(ddlNacionalidad.SelectedValue);
                        pac.IdProvincia = -1;


                        string domi = txtCalle.Value.ToUpper() + " " + txtBarrio.Value;
                        if (domi.Length >= 50)
                            domi = domi.Substring(0, 50);

                        pac.Calle = domi;

                        pac.Numero = 0;




                        pac.Piso = "";
                        pac.Departamento = "";
                        pac.Manzana = "";

                        pac.InformacionContacto = oUtil.SacaComillas(txtTelefono.Value);
                        if (btnConfirmar.Text != "Actualizar")

                            pac.FechaDefuncion = Convert.ToDateTime("01/01/1900");
                        pac.IdUsuario = us.IdUsuario;
                        pac.FechaUltimaActualizacion = DateTime.Now;
                        pac.NumeroAdic = txtNumeroAdic.Value;

                        pac.Mail = txtMail.Value;

                        pac.IdRaza = int.Parse(ddlRaza.SelectedValue.ToString());

                        if (ddlAborigen.SelectedValue == "0")
                            pac.SeDeclaraAborigen = false;
                        else
                            pac.SeDeclaraAborigen = true;


                        pac.Save();

                        if (idEstado.Value == "3")
                        {
                            if (fechaDomicilio.Text!="")
                            pac.GuardarDomicilio(fechaDomicilio.Text, txtCalle.Value, txtBarrio.Value, txtCiudad.Value.Replace("_", " "), txtProvincia.Value.Replace("_", " "), txtPais.Value, txtCodigoPostal.Value);
                            //GuardarFoto(pac);
                        }

                        if (actualizarSexoProtocolo)
                            pac.ActualizarSexoProtocolo();

                        if (actualizarEdadProtocolo)
                            pac.ActualizarEdadProtocolo();

                        if (Request["llamada"] != null)
                        {
                            if (Request["llamada"] == "LaboTurno")
                                Response.Redirect("../Turnos/TurnosEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&Modifica=0");
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
        private bool validadatos()
        {
            bool g=true;          
            if (txtApellido.Text.Contains("\uFFFD"))  g = false;
            if (txtNombre.Text.Contains("\uFFFD")) g = false;
            if (txtCalle.Value.Contains("\uFFFD")) g = false;
            if (txtCiudad.Value.Contains("\uFFFD")) g = false;
            if (txtProvincia.Value.Contains("\uFFFD")) g = false;
            if (txtPais.Value.Contains("\uFFFD")) g = false;
            if (txtBarrio.Value.Contains("\uFFFD")) g = false;
            if (ddlSexo.SelectedValue=="0") g = false;
            if (ddlSexoLegal.SelectedValue == "0") g = false;
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
        //   [Foto2] = @imagen where idPaciente=" + pac.IdPaciente;;
        //    SqlCommand cmd = new SqlCommand(query, conn);

        //    byte[] image =  Encoding.UTF8.GetBytes(Image1.ImageUrl);


        //    SqlParameter imageParam = cmd.Parameters.Add("@imagen", System.Data.SqlDbType.Image);

        //    imageParam.Value = image;

        //    cmd.ExecuteNonQuery();
        //}

        protected void ddlEstadoP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEstadoP.SelectedValue == "2")
            {
                MuestraTemporal();
            }
            else
            {
                rfvDocumento.Enabled = true;
                rvMotivo.Enabled = false;
                txtDNI.Enabled = true;
                divNumeroAdicional.Visible = false;
                txtDNI.Focus();
                
            }
        }

        protected void lnkValidarRenaper_Click(object sender, EventArgs e)
        {
            Response.Redirect("PacienteEdit2.aspx?Tipo=DNI&sexo="+HFSexo.Value+"&dni="+HFNumeroDocumento.Value+"&id=" + HFIdPaciente.Value + "&llamada=LaboProtocolo&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);


        }

        protected void ddlMotivoNI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMotivoNI.SelectedValue == "2") // extrnajero
                rfvPais.Enabled = true;
            else
                rfvPais.Enabled = false;
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (Request["llamada"] != null)
            {
                if (Request["llamada"] == "LaboTurno")
                    Response.Redirect("../Turnos/TurnosEdit2.aspx?idPaciente=" + HFIdPaciente.Value + "&Modifica=0");
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

                                    Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + HFIdPaciente.Value + "&idServicio=6");
                                if ((oCaso.IdTipoCaso == 2) || (oCaso.IdTipoCaso == 3)) //forense o quimerismo

                                    Response.Redirect("ProtocoloEditForense.aspx?idPaciente=" + HFIdPaciente.Value + "&Operacion=Alta&idServicio=6&idCaso=" + Session["idCaso"].ToString());
                            }

                            else // muestra sin caso, pedir consentimiento.
                            {
                                Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + HFIdPaciente.Value + "&idServicio=6");
                            }


                        }
                        else
                            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + HFIdPaciente.Value + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta");
                    }
                    else
                        Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + HFIdPaciente.Value + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString());
                }
            }
        }

        private bool validamail()
        {
            try {
                if (txtMail.Value != "")
                {
                    MailAddress m = new MailAddress(txtMail.Value);
                    return true;
                }
                else return true;
            } catch (FormatException) {
                return false;
                

            }

        }
    }
}

