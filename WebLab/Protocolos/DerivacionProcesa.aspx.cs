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

namespace WebLab.Protocolos
{
    public partial class DerivacionProcesa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {                    
                    if (Request["isScreening"] != null)
                    {
                        VerificaPermisos("Recepción de Derivaciones");
                        ProcesaScreening();
                    }
                    else
                    {
                        VerificaPermisos("Derivacion");
                        string s_idEfector = Request["idEfector"].ToString();

                        if (s_idEfector != "221")
                            ProcesarDatos(Request["idServicio"].ToString());
                        else
                            ProcesarDatosHeller(Request["idServicio"].ToString());
                    }
                }
                catch (Exception ex)
                {
                   lblMensaje.Text = "Ha ocurrido un error en el procesamiento:" + Environment.NewLine + "'" + ex.Message + "'." + Environment.NewLine + "Comuniquese con el Administrador del Sistema para verificar la conectividad. " + Environment.NewLine + "Si el problema persiste ingrese los datos del protocolo de la forma tradicional.";
                    lblMensaje.Rows = 8;
                    
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s_protocolo"></param>
        /// <returns></returns>
        private DataTable LeerDatosScreening(string s_protocolo)
        {
            btnGenerarPaciente.Visible = false;
            hplRegresar.NavigateUrl = "DerivacionScreening.aspx"; //?idEfector=" + s_idEfector;
            //hfidEfector.Value = s_idEfector;

            string s_urlWFC = ConfigurationManager.AppSettings["WSLaboratorio"].ToString();
            string s_url = s_urlWFC + "/GetProtocoloDerivadoScreening?numeroprotocolo=" + s_protocolo;                        

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s_url);
            HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Stream st = ws1.GetResponseStream();
            StreamReader sr = new StreamReader(st);

            string s = sr.ReadToEnd();
            if (s != "0")
            {
                int inicio = s.IndexOf("[");
                int fin = s.IndexOf("]") + 1;
                string aux = s.Substring(inicio, fin - inicio);

                List<ProtocoloDerivacionScreening> protocolo_d = jsonSerializer.Deserialize<List<ProtocoloDerivacionScreening>>(aux);

                if (protocolo_d.Count() > 0)
                {
                    SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    string s_insert = @"INSERT INTO LAB_TempSolicitudScreening  ([idSolicitudScreening]
           ,[idPaciente]
           ,[idEfectorSolicitante]
           ,[numeroTarjeta]
           ,[medicoSolicitante]
           ,[apellidoMaterno]
           ,[apellidoPaterno]
           ,[numerodocumentoParentesco]
           ,[nombreParentesco]
           ,[fechaNacimientoParentesco]
           ,[horaNacimiento]
           ,[edadGestacional]
           ,[peso]
           ,[primeraMuestra]
           ,[idMotivoRepeticion]
           ,[fechaExtraccion]
           ,[horaExtraccion]
           ,[ingestaLeche24Horas]
           ,[tipoAlimentacion]
           ,[antibiotico]
           ,[transfusion]
           ,[corticoides]
           ,[dopamina]
           ,[enfermedadTiroideaMaterna]
           ,[antecedentesMaternos]
           ,[corticoidesMaterno]
           ,[fechaRegistro]
           ,[fechaEnvio]
           ,[idLugarControl]
           ,[numeroDocumento]
           ,[apellido]
           ,[nombre]
           ,[fechaNacimiento]
           ,[idSexo]
           ,[informacionContacto]
           ,[analisis])
     VALUES
           (" + s_protocolo + ",0," + protocolo_d[0].idEfectorSolicitante.ToString() + "," +
                 protocolo_d[0].numeroTarjeta + ",'" + protocolo_d[0].medicoSolicitante + "','" +
                protocolo_d[0].ApellidosParentesco + "','" +
                protocolo_d[0].Apellidos + "'," +
                protocolo_d[0].DocumentoParentesco.ToString() + ",'" +
                protocolo_d[0].NombresParentesco + "','" +
                DateTime.Parse(protocolo_d[0].FechaNacimientoParentesco.ToString()).ToString("yyyyMMdd") + "','" +
                protocolo_d[0].horaNacimiento + "'," +
                protocolo_d[0].edadGestacional.ToString() + "," +
                protocolo_d[0].peso.ToString() + ",'" +
                protocolo_d[0].primeraMuestra.ToString() + "'," +
                protocolo_d[0].idMotivoRepeticion.ToString() + ",'" +
                DateTime.Parse(protocolo_d[0].fechaExtraccion.ToString()).ToString("yyyyMMdd") + "','" +
                protocolo_d[0].horaExtraccion + "','" +
                protocolo_d[0].ingestaLeche24Horas + "','" +
                protocolo_d[0].tipoAlimentacion + "','" +
                protocolo_d[0].antibiotico + "','" +
                protocolo_d[0].transfusion + "','" +
                protocolo_d[0].corticoide + "','" +
                protocolo_d[0].dopamina + "','" +
                protocolo_d[0].enfermedadTiroideaMaterna + "','" +
                protocolo_d[0].antecedentesMaternos + "','" +
                protocolo_d[0].corticoidesMaterno + "','" +
                DateTime.Parse(protocolo_d[0].fechaCarga.ToString()).ToString("yyyyMMdd") + "','" +
                DateTime.Parse(protocolo_d[0].fechaEnvio.ToString()).ToString("yyyyMMdd") + "'," +
                protocolo_d[0].idLugarControl.ToString() + "," +
                protocolo_d[0].Documento.ToString() + ",'" +
                protocolo_d[0].Apellidos + "','" +
                protocolo_d[0].Nombres + "','" +
                DateTime.Parse(protocolo_d[0].FechaNacimiento.ToString()).ToString("yyyyMMdd") + "'," +
                protocolo_d[0].Sexo + ",'" +
                protocolo_d[0].InformacionContacto + "','" + protocolo_d[0].CodigoAnalisis + "')";

                cmd.CommandText = s_insert;
                cmd.Connection = conn2;
              
                cmd.ExecuteNonQuery();
                }
            }
          
            ////lee de la tabla temporal de screening descargados del servidor
            string m_strSQL = @" SELECT apellido, nombre, fechaNacimiento,idSexo, numeroDocumento,  apellidoMaterno,nombreParentesco,fechaNacimientoParentesco,numerodocumentoParentesco, numeroTarjeta, fechaRegistro as fecha, [informacionContacto],[idSolicitudScreening], 
            case when numeroDocumento =0 then 2 else 1 end as [idEstado], idEfectorSolicitante, analisis
            FROM         LAB_TempSolicitudScreening WHERE     idSolicitudScreening =" + s_protocolo;
            
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }
    

        private void ProcesaScreening()
        {
            gvLista.Visible = false;
            btnGenerarPaciente.Visible = false;
            //string s_idEfector = Request["idEfector"].ToString();
            string s_protocolo = Request["protocolo"].ToString();
            hplRegresar.NavigateUrl = "DerivacionScreening.aspx"; //?idEfector=" + s_idEfector;
         
            DataTable dt = LeerDatosScreening( s_protocolo);

            if (dt.Rows.Count > 0)
            {
                Utility oUtil = new Utility();
                ///crear objeto Paciente
                string s_apellido = dt.Rows[0][0].ToString(); s_apellido = oUtil.SacaComillas(s_apellido); hfapellido.Value = s_apellido;
                string s_nombre = dt.Rows[0][1].ToString(); s_nombre = oUtil.SacaComillas(s_nombre); hfnombre.Value = s_nombre;
                string s_fechanac = dt.Rows[0][2].ToString();    hffechaNacimiento.Value = s_fechanac;
                string s_sexo = dt.Rows[0][3].ToString();
                switch (dt.Rows[0][3].ToString()) { case "2": s_sexo = "F"; break; case "3": s_sexo ="M"; break; case "1": s_sexo = "I"; break; }
                int s_numeroDocumento = int.Parse(dt.Rows[0][4].ToString());
                hfsexo.Value = s_sexo;
                ///crear objeto Parentesco del Paciente
                string s_apellidoParentesco = oUtil.SacaComillas(dt.Rows[0][5].ToString());
                hfapellidoParentesco.Value = s_apellidoParentesco;

                string s_nombreParentesco = oUtil.SacaComillas(dt.Rows[0][6].ToString());
                hfnombreParentesco.Value = s_nombreParentesco;

                string s_fechanacParentesco = dt.Rows[0][7].ToString();
                hffechaNacimientoParentesco.Value = s_fechanacParentesco;

            //    string s_tipoParentesco = "MADRE";

                int s_numeroDocumentoParentesco = int.Parse(dt.Rows[0][8].ToString());
                hfnumeroDocumentoParentesco.Value = s_numeroDocumentoParentesco.ToString();
                /////numero de tarjeta.
                string s_numeroProtocolo = dt.Rows[0][12].ToString(); hfnumeroProtocolo.Value = s_numeroProtocolo;                
                string s_fechaProtocolo = dt.Rows[0][10].ToString().Replace("-", "/"); hffechaProtocolo.Value = DateTime.Parse(s_fechaProtocolo).ToString("dd/MM/yyyy");

                string s_informacionContacto = oUtil.SacaComillas(dt.Rows[0][11].ToString()); hfinformacionContacto.Value = s_informacionContacto;
                string s_idSolicitudScreening = dt.Rows[0][12].ToString();

                string s_idEstadoPaciente = dt.Rows[0][13].ToString();
                string s_idEfector = dt.Rows[0][14].ToString();
                hfidEfector.Value = s_idEfector;

                string s_analisis = dt.Rows[0][15].ToString();// getAnalisisScreening(s_idSolicitudScreening);           
                hfanalisis.Value = s_analisis;

                string s_sala = ""; hfsala.Value = s_sala;
                string s_cama = ""; hfcama.Value = s_cama;

                string s_calle = "";
                //if (protocolo_d[0].Calle != null) s_calle = protocolo_d[0].Calle;

                int s_numeroDomicilio = 0;
                //if (protocolo_d[0].NumeroDomicilio != null) s_numeroDomicilio = protocolo_d[0].NumeroDomicilio;

                string s_referencia = ""; hfreferencia.Value = s_referencia;                      
                string s_idDiagnostico = "0";  hfdiagnostico.Value = s_idDiagnostico;

                if (ExisteNumeroScreening(s_numeroProtocolo))  
                {
                    lblMensaje.Text = "El Nro. de Solicitud " + s_numeroProtocolo.ToUpper() + " ya fué ingresado.";
                    lblMensaje.Rows = 2;
                }
                else
                {
                    if ((s_idEstadoPaciente == "1") && (s_numeroDocumento>0))
                    {
                        Paciente oPaciente = new Paciente();
                        oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "NumeroDocumento", s_numeroDocumento, "IdEstado", 1);
                        if (oPaciente != null)
                        {
                            string apellidoExistente = oPaciente.Apellido.ToUpper(); string nombreExistente = oPaciente.Nombre.ToUpper();
                            int i_apellido = String.Compare(s_apellido, apellidoExistente);
                            int i_nombre = string.Compare(s_nombre, nombreExistente);
                            if ((i_apellido < 0) || (i_nombre < 0))
                            {
                                lblMensaje.Text = "El DNI ya existe con los siguientes datos."; lblMensaje.Rows = 1;
                                gvLista.DataSource = BuscarPacientes(s_numeroDocumento);
                                gvLista.DataBind();
                                gvLista.Visible = true;
                                btnGenerarPaciente.Visible = true;
                            }
                            else
                            {
                                //Actualiza datos del paciente con los datos del origen
                                oPaciente.Apellido = s_apellido;
                                oPaciente.Nombre = s_nombre;

                                string d3 = s_fechanac.Substring(0, 10);
                                string D1 = DateTime.Parse(d3).ToShortDateString();
                                string D2 = DateTime.Parse("01/01/1900").ToShortDateString();
                                if (D1 != D2) oPaciente.FechaNacimiento = DateTime.Parse(d3);
                                switch (s_sexo) { case "F": oPaciente.IdSexo = 2; break; case "M": oPaciente.IdSexo = 3; break; case "I": oPaciente.IdSexo = 1; break; }
                                oPaciente.Calle = s_calle;
                                oPaciente.Numero = s_numeroDomicilio;
                                oPaciente.Referencia = s_referencia;
                                oPaciente.InformacionContacto = s_informacionContacto;
                                oPaciente.Save();
                                if (oPaciente.IdPaciente != 0)
                                    Response.Redirect("ProtocoloEditPesquisa.aspx?idServicio=4&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value + "&idSolicitudScreening=" + s_idSolicitudScreening, false);

                            }
                        }  /// if paciente!=null
                        else
                        {
                          
                                oPaciente = GenerarPaciente(s_numeroDocumento, s_apellido, s_nombre, s_fechanac.Substring(0, 10), s_sexo, s_calle, s_numeroDomicilio, s_referencia, s_informacionContacto);
                                if (oPaciente.IdPaciente == 0) { lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema. Verifique con personal de TICs."; lblMensaje.Rows = 2; }
                                else
                                    Response.Redirect("ProtocoloEditPesquisa.aspx?idServicio=4&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value + "&idSolicitudScreening=" + s_idSolicitudScreening, false);
                            
                        }  /// else if paciente!=null
                    }

                    else /// paciente con estado=2 ; hay que cargarlo.
                    {                          
                          
                               //// buscar con el numero de dni parentesco si tiene bebes asignados
                               DataTable dtParentesco= BuscarParentesco(s_numeroDocumentoParentesco);
                               if (dtParentesco.Rows.Count > 0)
                               {
                                   lblMensaje.Text = "Se encontraron los siguientes hijos asociados al numero de dni de parentesco: "; lblMensaje.Rows = 2;
                                   gvLista.DataSource = dtParentesco;
                                   gvLista.DataBind();
                                   gvLista.Visible = true;
                                   btnGenerarPaciente.Visible = true;
                               }
                               else
                               {




                                   s_numeroDocumento = 0;
                                   Paciente oPaciente = new Paciente();
                                   oPaciente = GenerarPaciente(s_numeroDocumento, s_apellido, s_nombre, s_fechanac.Substring(0, 10), s_sexo, s_calle, s_numeroDomicilio, s_referencia, s_informacionContacto);
                                   if (oPaciente.IdPaciente == 0) { lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema. Verifique con personal de TICs."; lblMensaje.Rows = 2; }
                                   else
                                       Response.Redirect("ProtocoloEditPesquisa.aspx?idServicio=4&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value + "&idSolicitudScreening=" + s_idSolicitudScreening, false);
                               }
                    }
                }
            }
            else
            {
                lblMensaje.Text = "No se encontró solicitud pendiente de recibir con el número ingresado." + Environment.NewLine + "Verificar con el Laboratorio Derivante.";
                lblMensaje.Rows = 3;
            }
               
        
        }

        private DataTable BuscarParentesco(int s_numeroDocumentoParentesco)
        {
            string m_strSQL = @" select idPaciente, numeroDocumento, apellido, nombre, convert(varchar(10),fechaNacimiento,103) as fechaNacimiento from sys_paciente where idpaciente in (select idpaciente from sys_parentesco where numerodocumento=" + s_numeroDocumentoParentesco.ToString() + ")";
//Select idPaciente, numeroDocumento, apellido, nombre, convert(varchar(10),fechaNacimiento,103) as fechaNacimiento from Sys_PAciente WHERE numeroDocumento=" + s_numeroDocumento.ToString();
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        private bool ExisteNumeroScreening(string s_numeroProtocolo)
        {
            ///
            ///verificar si el numero de screening esta en lab_protocolo.idprotocolo= lab_solicitudscreening.idprotocolo where lab_protocolo.numeroorigen=idsolictuscreening
            ///
            bool existe = false;                        
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(SolicitudScreening));
            crit.Add(Expression.Eq("IdSolicitudScreeningOrigen",  s_numeroProtocolo));          
            IList detalle = crit.List();
            foreach (SolicitudScreening odet in detalle)
            {
                if (!odet.IdProtocolo.Baja) { existe = true; break; }
                else existe = false;
            }            
            return existe;
        }
    

        private void ProcesarDatosHeller(string s_idServicio)
        {
            gvLista.Visible = false;
            btnGenerarPaciente.Visible = false;
            string s_idEfector = Request["idEfector"].ToString();
            string s_protocolo = Request["protocolo"].ToString();
            hplRegresar.NavigateUrl = "Derivacion.aspx?idEfector=" + s_idEfector +"&idServicio="+ s_idServicio;
            hfidEfector.Value = s_idEfector;

            string s_urlWFC = ConfigurationManager.AppSettings["Efector" + s_idEfector].ToString();            
            string s_url = s_urlWFC + "?idpet=" + s_protocolo;         

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s_url);
            HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            Stream st = ws1.GetResponseStream();
            StreamReader sr = new StreamReader(st);

            string s = sr.ReadToEnd();
            if (s != "")
            {
                //int inicio = s.IndexOf("{");
                //int fin = s.IndexOf("}") + 1;
                //string aux = s.Substring(inicio, fin - inicio);

                try
                {                
                string aux ="["+ s+"]";              
                List<ProtocoloDerivacion> protocolo_d = jsonSerializer.Deserialize<List<ProtocoloDerivacion>>(aux);            

                if (protocolo_d.Count > 0)
                {
                    Utility oUtil = new Utility();
                    ///crear objeto Paciente
                    string s_apellido = "";
                    if (protocolo_d[0].Apellidos != null) s_apellido = protocolo_d[0].Apellidos; else s_apellido = protocolo_d[0].Apellido;
                    s_apellido = oUtil.SacaComillas(s_apellido); hfapellido.Value = s_apellido;

                    string s_nombre = "";
                    if (protocolo_d[0].Nombres != null) s_nombre = protocolo_d[0].Nombres; else s_nombre = protocolo_d[0].Nombre;
                    s_nombre = oUtil.SacaComillas(s_nombre); hfnombre.Value = s_nombre;

                    string s_fechanac = "";
                    if (protocolo_d[0].FechaNacimiento != null) s_fechanac = protocolo_d[0].FechaNacimiento; else s_fechanac = protocolo_d[0].FechaNac.Replace("-", "/");
                    hffechaNacimiento.Value = s_fechanac;

                    string s_sexo = protocolo_d[0].Sexo; hfsexo.Value = s_sexo;
                    int s_numeroDocumento = protocolo_d[0].Documento;

                    ///crear objeto Parentesco del Paciente
                    string s_apellidoParentesco = "";
                    if (protocolo_d[0].ApellidosParentesco != null) s_apellidoParentesco = oUtil.SacaComillas(protocolo_d[0].ApellidosParentesco);

                    string s_nombreParentesco = "";
                    if (protocolo_d[0].NombresParentesco != null) s_nombreParentesco = oUtil.SacaComillas(protocolo_d[0].NombresParentesco);

                    string s_fechanacParentesco = "";
                    if (protocolo_d[0].FechaNacimientoParentesco != null) s_fechanacParentesco = protocolo_d[0].FechaNacimientoParentesco;

                    string s_tipoParentesco = protocolo_d[0].TipoParentesco;

                    int s_numeroDocumentoParentesco = 0;
                    if (protocolo_d[0].DocumentoParentesco != null) s_numeroDocumentoParentesco = protocolo_d[0].DocumentoParentesco;
                    ///Lllamar a web services del hospital con los datos del paciente y esperar ID de Pacient
                    ///
                    ///crear objeto Protocolo
                    string s_numeroProtocolo = protocolo_d[0].NumeroProtocolo; hfnumeroProtocolo.Value = s_numeroProtocolo;
                    //hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value);                
                    string s_fechaProtocolo = protocolo_d[0].FechaProtocolo.Replace("-", "/"); hffechaProtocolo.Value = DateTime.Parse(s_fechaProtocolo).ToString("dd/MM/yyyy");

                    string s_analisis = "";
                    if (protocolo_d[0].CodigoAnalisis != null) s_analisis = protocolo_d[0].CodigoAnalisis; else s_analisis = protocolo_d[0].Codigos;
                    hfanalisis.Value = s_analisis;

                    string s_sala = "";
                    if (protocolo_d[0].Sala != null)
                    { s_sala = oUtil.SacaComillas(protocolo_d[0].Sala); hfsala.Value = s_sala; }

                    string s_cama = "";
                    if (protocolo_d[0].Cama != null)
                    {
                       s_cama= oUtil.SacaComillas(protocolo_d[0].Cama); hfcama.Value = s_cama;
                    }

                    string s_calle = "";
                    if (protocolo_d[0].Calle != null) s_calle = oUtil.SacaComillas( protocolo_d[0].Calle);

                    int s_numeroDomicilio = 0;
                    if (protocolo_d[0].NumeroDomicilio != null) s_numeroDomicilio = protocolo_d[0].NumeroDomicilio;

                    string s_referencia = "";
                    if (protocolo_d[0].Referencia != null)
                    { s_referencia = oUtil.SacaComillas(protocolo_d[0].Referencia); }
                    hfreferencia.Value = s_referencia;

                    string s_informacionContacto = "";
                    if (protocolo_d[0].InformacionContacto != null)
                    { s_informacionContacto = oUtil.SacaComillas(protocolo_d[0].InformacionContacto); }
                    hfinformacionContacto.Value = s_informacionContacto;


                    string s_idDiagnostico = "0";
                    if (protocolo_d[0].DiagnosticoEmbarazo != null) { s_idDiagnostico = protocolo_d[0].DiagnosticoEmbarazo; }
                    hfdiagnostico.Value = s_idDiagnostico;

                    if (ExisteNumeroOrigen(int.Parse(s_idEfector), s_numeroProtocolo))
                    {
                        lblMensaje.Text = "El Nro. de protocolo " + s_numeroProtocolo.ToUpper() + " ya fué ingresado para el efector derivante seleccionado.";
                        lblMensaje.Rows = 2;
                    }
                    else
                    {
                        //   s_numeroDocumento = 3900558;
                        Paciente oPaciente = new Paciente();
                        oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "NumeroDocumento", s_numeroDocumento, "IdEstado", 1);
                        if (oPaciente != null)
                        {
                            string apellidoExistente = oPaciente.Apellido.ToUpper();
                            string nombreExistente = oPaciente.Nombre.ToUpper();

                            int i_apellido = String.Compare(s_apellido, apellidoExistente);
                            int i_nombre = string.Compare(s_nombre, nombreExistente);

                            if ((i_apellido < 0) || (i_nombre < 0))
                            {
                                ///Existe otro paciente con el mismo dni.
                                lblMensaje.Text = "El DNI ya existe con los siguientes datos."; lblMensaje.Rows = 1;
                                ///poner link para seleccionar paciente y redirect a ProtocoloEdit2.aspx
                                gvLista.DataSource = BuscarPacientes(s_numeroDocumento);
                                gvLista.DataBind();

                                gvLista.Visible = true;
                                btnGenerarPaciente.Visible = true;
                                //oPaciente = GenerarPaciente(s_numeroDocumento, s_apellido, s_nombre, s_fechanac, s_sexo,  s_calle ,  s_numeroDomicilio,  s_referencia, s_informacionContacto);
                                //if (oPaciente == null){ lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema";lblMensaje.Rows = 2;}
                            }
                            else
                            {
                                //Actualiza datos del paciente con los datos del origen
                                oPaciente.Apellido = s_apellido;
                                oPaciente.Nombre = s_nombre;

                                string d3 = s_fechanac.Substring(0, 10);
                                string D1 = DateTime.Parse(d3).ToShortDateString();
                                string D2 = DateTime.Parse("01/01/1900").ToShortDateString();

                                if (D1 != D2) oPaciente.FechaNacimiento = DateTime.Parse(d3);


                                //if (DateTime.Parse(s_fechanac.Substring(0,10)).ToString("MM/dd/yyyy") != DateTime.Parse("01/01/1900").ToString("MM/dd/yyyy"))
                                //    oPaciente.FechaNacimiento = DateTime.Parse(s_fechanac);

                                switch (s_sexo) { case "F": oPaciente.IdSexo = 2; break; case "M": oPaciente.IdSexo = 3; break; case "I": oPaciente.IdSexo = 1; break; }
                                oPaciente.Calle = s_calle;
                                oPaciente.Numero = s_numeroDomicilio;
                                oPaciente.Referencia = s_referencia;
                                oPaciente.InformacionContacto = s_informacionContacto;
                                oPaciente.Save();
                                if (oPaciente.IdPaciente != 0)
                                {
                                    Response.Redirect("ProtocoloEdit2.aspx?idServicio=1&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value, false);
                                }
                            }
                        }
                        else
                        {
                            oPaciente = GenerarPaciente(s_numeroDocumento, s_apellido, s_nombre, s_fechanac.Substring(0, 10), s_sexo, s_calle, s_numeroDomicilio, s_referencia, s_informacionContacto);
                            if (oPaciente.IdPaciente == 0) { lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema"; lblMensaje.Rows = 2; }
                            else
                                Response.Redirect("ProtocoloEdit2.aspx?idServicio=1&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value, false);
                        }
                    }

                }///fin if
              
                else
                {
                    lblMensaje.Text = "No se encontraron datos para el número ingresado." + Environment.NewLine + "Verificar con el Laboratorio Derivante.";
                    lblMensaje.Rows = 3;
                }
                }///try
                catch (Exception ex)
                {
                    //string exception = "";
                    //while (ex != null)
                    //{
                    //    exception = ex.Message + "<br>";

                    //}
                    lblMensaje.Text = "No se encontraron datos para el número ingresado." + Environment.NewLine + "Verificar con el Laboratorio Derivante.";
                    lblMensaje.Rows = 3;
                }
            }
            else
            {
                lblMensaje.Text = "No se encontraron datos para el número ingresado." + Environment.NewLine + "Verificar con el Laboratorio Derivante.";
                lblMensaje.Rows = 3;
            }
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


        private void ProcesarDatos(string s_idServicio)
        {
            gvLista.Visible = false;
            btnGenerarPaciente.Visible = false;
            string s_idEfector = Request["idEfector"].ToString();
            string s_protocolo = Request["protocolo"].ToString();
            hplRegresar.NavigateUrl = "Derivacion.aspx?idEfector=" + s_idEfector +"&idServicio="+ s_idServicio;
            hfidEfector.Value = s_idEfector;

            string s_urlWFC = ConfigurationManager.AppSettings["Efector" + s_idEfector].ToString();
            string s_url = "http://" + s_urlWFC + "/WCFLaboratorio/wsLaboratorio.asmx/GetProtocoloDerivado?numeroprotocolo=" + s_protocolo;

            if (s_idEfector == "221")
                s_url = s_urlWFC + "?idpet=" + s_protocolo;

            //if (Request["isScreening"] == "1") 
            //{
            //    s_url = "http://10.1.232.8/WSLaboratorio/wsLaboratorio.asmx/GetProtocoloDerivado?idEfector=1&numeroprotocolo=" + s_protocolo; 
            //}

            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(s_url);           
            HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();         
            Stream st = ws1.GetResponseStream();
            StreamReader sr = new StreamReader(st);

            string s = sr.ReadToEnd();
            if (s != "0")
            {
                int inicio = s.IndexOf("[");
                int fin = s.IndexOf("]") + 1;
                string aux = s.Substring(inicio, fin - inicio);

                List<ProtocoloDerivacion> protocolo_d = jsonSerializer.Deserialize<List<ProtocoloDerivacion>>(aux);

                if (protocolo_d.Count > 0)
                {
                    Utility oUtil = new Utility();                    
                    ///crear objeto Paciente
                    string s_apellido = "";
                    if (protocolo_d[0].Apellidos != null) s_apellido = protocolo_d[0].Apellidos; else s_apellido = protocolo_d[0].Apellido;
                    s_apellido = oUtil.SacaComillas(s_apellido); hfapellido.Value = s_apellido;

                    string s_nombre = "";
                    if (protocolo_d[0].Nombres != null) s_nombre = protocolo_d[0].Nombres; else s_nombre = protocolo_d[0].Nombre;
                    s_nombre = oUtil.SacaComillas(s_nombre); hfnombre.Value = s_nombre;

                    string s_fechanac = "";
                    if (protocolo_d[0].FechaNacimiento != null) s_fechanac = protocolo_d[0].FechaNacimiento; else s_fechanac = protocolo_d[0].FechaNac.Replace("-", "/");
                    hffechaNacimiento.Value = s_fechanac;

                    string s_sexo = protocolo_d[0].Sexo; hfsexo.Value = s_sexo;
                    
                    int s_numeroDocumento = protocolo_d[0].Documento;

                    ///crear objeto Parentesco del Paciente
                    string s_apellidoParentesco = "";
                    if (protocolo_d[0].ApellidosParentesco != null) s_apellidoParentesco = oUtil.SacaComillas(protocolo_d[0].ApellidosParentesco);

                    string s_nombreParentesco = "";
                    if (protocolo_d[0].NombresParentesco != null) s_nombreParentesco = oUtil.SacaComillas(protocolo_d[0].NombresParentesco);

                    string s_fechanacParentesco = "";
                    if (protocolo_d[0].FechaNacimientoParentesco != null) s_fechanacParentesco = protocolo_d[0].FechaNacimientoParentesco;

                    string s_tipoParentesco = protocolo_d[0].TipoParentesco;

                    int s_numeroDocumentoParentesco = 0;
                    if (protocolo_d[0].DocumentoParentesco != null) s_numeroDocumentoParentesco = protocolo_d[0].DocumentoParentesco;
                    ///Lllamar a web services del hospital con los datos del paciente y esperar ID de Pacient
                    ///
                    ///crear objeto Protocolo
                    string s_numeroProtocolo = protocolo_d[0].NumeroProtocolo; hfnumeroProtocolo.Value = s_numeroProtocolo;
                    //hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value);                
                    string s_fechaProtocolo = protocolo_d[0].FechaProtocolo.Replace("-", "/"); hffechaProtocolo.Value = DateTime.Parse(s_fechaProtocolo).ToString("dd/MM/yyyy");

                    string s_analisis = "";
                    if (protocolo_d[0].CodigoAnalisis != null) s_analisis = protocolo_d[0].CodigoAnalisis; else s_analisis = protocolo_d[0].Codigos;
                    hfanalisis.Value = s_analisis;

                    string s_sala = "";
                    if (protocolo_d[0].Sala != null)   s_sala=   oUtil.SacaComillas(protocolo_d[0].Sala); 
                    hfsala.Value = s_sala;
                    
                    string s_cama = "";
                    if (protocolo_d[0].Cama != null)  s_cama=oUtil.SacaComillas(protocolo_d[0].Cama); 
                    hfcama.Value = s_cama;
                    

                    string s_calle = "";
                    if (protocolo_d[0].Calle != null) s_calle =oUtil.SacaComillas(  protocolo_d[0].Calle);

                    int s_numeroDomicilio = 0;
                    if (protocolo_d[0].NumeroDomicilio != null) s_numeroDomicilio = protocolo_d[0].NumeroDomicilio;

                    string s_referencia = "";
                    if (protocolo_d[0].Referencia != null)
                    { s_referencia = oUtil.SacaComillas(protocolo_d[0].Referencia); }
                    hfreferencia.Value = s_referencia;

                    string s_informacionContacto = "";
                    if (protocolo_d[0].InformacionContacto != null)
                    { s_informacionContacto = oUtil.SacaComillas(protocolo_d[0].InformacionContacto); }
                    hfinformacionContacto.Value = s_informacionContacto;


                    string s_idDiagnostico = "0";
                    if (protocolo_d[0].DiagnosticoEmbarazo != null) { s_idDiagnostico = protocolo_d[0].DiagnosticoEmbarazo; }
                    hfdiagnostico.Value = s_idDiagnostico;

                    if (ExisteNumeroOrigen(int.Parse(s_idEfector), s_numeroProtocolo))
                    {
                        lblMensaje.Text = "El Nro. de protocolo " + s_numeroProtocolo.ToUpper() + " ya fué ingresado para el efector derivante seleccionado.";
                        lblMensaje.Rows = 2;
                    }
                    else
                    {
                        //   s_numeroDocumento = 3900558;
                        Paciente oPaciente = new Paciente();
                        //oPaciente.Save();
                        oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "NumeroDocumento", s_numeroDocumento, "IdEstado", 1);
                        if (oPaciente != null)
                        {
                            string apellidoExistente = oPaciente.Apellido.ToUpper();
                            string nombreExistente = oPaciente.Nombre.ToUpper();

                            int i_apellido = String.Compare(s_apellido, apellidoExistente);
                            int i_nombre = string.Compare(s_nombre, nombreExistente);
               
                            if ((i_apellido < 0) || (i_nombre < 0))
                            {
                                ///Existe otro paciente con el mismo dni.
                                lblMensaje.Text = "El DNI ya existe con los siguientes datos."; lblMensaje.Rows = 1;
                                ///poner link para seleccionar paciente y redirect a ProtocoloEdit2.aspx
                                gvLista.DataSource = BuscarPacientes(s_numeroDocumento);
                                gvLista.DataBind();

                                gvLista.Visible = true;
                                btnGenerarPaciente.Visible = true;
                                //oPaciente = GenerarPaciente(s_numeroDocumento, s_apellido, s_nombre, s_fechanac, s_sexo,  s_calle ,  s_numeroDomicilio,  s_referencia, s_informacionContacto);
                                //if (oPaciente == null){ lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema";lblMensaje.Rows = 2;}
                            }
                            else
                            {
                                //Actualiza datos del paciente con los datos del origen
                                oPaciente.Apellido = s_apellido;
                                oPaciente.Nombre = s_nombre;

                                string d3 = s_fechanac.Substring(0, 10);
                                string D1 = DateTime.Parse(d3).ToShortDateString();
                                string D2 = DateTime.Parse("01/01/1900").ToShortDateString();

                                if (D1 != D2)  oPaciente.FechaNacimiento = DateTime.Parse(d3);


                                //if (DateTime.Parse(s_fechanac.Substring(0,10)).ToString("MM/dd/yyyy") != DateTime.Parse("01/01/1900").ToString("MM/dd/yyyy"))
                                //    oPaciente.FechaNacimiento = DateTime.Parse(s_fechanac);

                                switch (s_sexo) { case "F": oPaciente.IdSexo = 2; break; case "M": oPaciente.IdSexo = 3; break; case "I": oPaciente.IdSexo = 1; break; }
                                oPaciente.Calle = s_calle;
                                oPaciente.Numero = s_numeroDomicilio;
                                oPaciente.Referencia = s_referencia;
                                oPaciente.InformacionContacto = s_informacionContacto;
                                oPaciente.Save();
                                if (oPaciente.IdPaciente != 0)
                                {
                                    Response.Redirect("ProtocoloEdit2.aspx?idServicio="+s_idServicio+"&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value, false);
                                }
                            }
                        }
                        else
                        {
                            oPaciente = GenerarPaciente(s_numeroDocumento, s_apellido, s_nombre, s_fechanac.Substring(0, 10), s_sexo, s_calle, s_numeroDomicilio, s_referencia, s_informacionContacto);
                            if (oPaciente.IdPaciente == 0) { lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema"; lblMensaje.Rows = 2; }
                            else
                                Response.Redirect("ProtocoloEdit2.aspx?idServicio="+ s_idServicio + "&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value,false);
                        }
                    }

                }///fin if
                else
                {
                    lblMensaje.Text = "No se encontraron datos para el número ingresado." + Environment.NewLine + "Verificar con el Laboratorio Derivante.";
                    lblMensaje.Rows = 3;
                }
            }
            else
            {
                lblMensaje.Text = "No se encontraron datos para el número ingresado." + Environment.NewLine + "Verificar con el Laboratorio Derivante.";
                lblMensaje.Rows = 3;
            }

        }


     

        private DataTable BuscarPacientes(int s_numeroDocumento)
        {
            string m_strSQL = @" Select idPaciente, numeroDocumento, apellido, nombre, convert(varchar(10),fechaNacimiento,103) as fechaNacimiento from Sys_PAciente WHERE numeroDocumento=" + s_numeroDocumento.ToString();          
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            
            return Ds.Tables[0];
        }


        private Paciente GenerarPaciente(int s_numeroDocumento, string s_apellido, string s_nombre, 
            string s_fechanac, string s_sexo, string s_calle ,int  s_numeroDomicilio, string s_referencia,
            string s_informacionContacto)
        {
            //instancio el usuario
            Usuario us = new Usuario();
            us = (Usuario)us.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            Paciente oPaciente= new Paciente();
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (oCon.IdEfector.IdEfector!= 205)//if (oCon.IdEfector.IdEfector == 205)//
            {
                ///CREA UN NUEVO PACIENTE EN EL SIL DIRECTAMENTE                            
                oPaciente.IdEfector = us.IdEfector;
                oPaciente.Apellido = s_apellido.ToUpper();
                oPaciente.Nombre = s_nombre.ToUpper();
                if (s_numeroDocumento == 0) { oPaciente.IdEstado = 2; oPaciente.NumeroDocumento = 0; }
                else { oPaciente.IdEstado = 1; oPaciente.NumeroDocumento = s_numeroDocumento; }

                oPaciente.FechaAlta = DateTime.Now;
                oPaciente.FechaNacimiento = DateTime.Parse(s_fechanac);
                switch (s_sexo) { case "F": oPaciente.IdSexo = 2; break; case "M": oPaciente.IdSexo = 3; break; case "I": oPaciente.IdSexo = 1; break; }
                oPaciente.IdPais = 54;
                oPaciente.IdProvincia = 0;
                oPaciente.Calle = s_calle;
                oPaciente.Numero = s_numeroDomicilio;
                oPaciente.Piso = "";
                oPaciente.Manzana = "";
                oPaciente.Referencia = s_referencia;

                oPaciente.InformacionContacto = s_informacionContacto;
                oPaciente.FechaDefuncion = Convert.ToDateTime("01/01/1900");
                oPaciente.IdUsuario = us.IdUsuario;
                oPaciente.FechaUltimaActualizacion = DateTime.Now;
                oPaciente.Save();
                ///////////////////////////////

                CargarParentesco(oPaciente, us);
            }
            else
            { 
                ///////Llama a web service del hospital neuquen y devuelve paciente.
                ///////  https://wwwdev.hospitalneuquen.org.ar/dotnet/Historias/Services/WebService.asmx/CrearHistoriaClinica
                //////////////////
                int i_idSexo= 1;
                switch (s_sexo) { case "F": i_idSexo = 2; break; case "M":i_idSexo = 1; break; case "I": i_idSexo = 3; break; }
                string s_urlWFC = ConfigurationManager.AppSettings["WSPacienteHNQN"].ToString();
                string s_fechanac1= DateTime.Parse(s_fechanac.Substring(0,10)).ToString("MM/dd/yyyy");


                /////datos del parentesco
                    string s_numeroDocumentoParentesco = hfnumeroDocumentoParentesco.Value;
                    if (s_numeroDocumentoParentesco == "") s_numeroDocumentoParentesco = "0";

                    string s_apellidoParentesco = hfapellidoParentesco.Value;
                    if (s_apellidoParentesco.Length > 50) s_apellidoParentesco = s_apellidoParentesco.Substring(0, 49);
                    //else s_apellidoParentesco = ".";

                    string s_nombreParentesco = hfnombreParentesco.Value;
                    if (s_nombreParentesco.Length > 50) s_nombreParentesco = s_nombreParentesco.Substring(0, 49);
                    //else s_nombreParentesco = ".";

                    string s_fechanacParentesco = "";
                    if (hffechaNacimientoParentesco.Value != "")
                    {
                        string d3 = hffechaNacimientoParentesco.Value.Substring(0, 10);
                        s_fechanacParentesco = DateTime.Parse(d3).ToString("MM/dd/yyyy");                    
                    }
                    else s_fechanacParentesco = Convert.ToDateTime("01/01/1900").ToString("MM/dd/yyyy");
                    //int i_idSexoParentesco = 2;
                ///////

                //string s_url = s_urlWFC + "?documento=" + s_numeroDocumento.ToString() + "&apellido=" + s_apellido.Replace("-", "") + "&nombres=" + s_nombre.Replace("-", "") + "&fechaNacimiento=" + s_fechanac1 + "&sexo=" + i_idSexo;
                string s_url = s_urlWFC + "?documento=" + s_numeroDocumento.ToString() + "&apellido=" + s_apellido.Trim().Replace("-", "") + "&nombres=" + s_nombre.Trim().Replace("-", "") + "&fechaNacimiento=" + s_fechanac1 + "&sexo=" + i_idSexo + "&madre_documento=" + s_numeroDocumentoParentesco + "&madre_apellido=" + s_apellidoParentesco.Trim().Replace("-", "") + "&madre_Nombres=" + s_nombreParentesco.Trim().Replace("-", "");
            
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(s_url);                              
                HttpWebResponse ws1 = (HttpWebResponse)request.GetResponse();
        
                Stream st = ws1.GetResponseStream();
                StreamReader sr = new StreamReader(st);
                string s = sr.ReadToEnd();

                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(s);
                string resultado = doc.DocumentElement.InnerText; 
                
                int p = int.Parse(resultado);
                if (p == -1)
                {
                    lblMensaje.Text = "El paciente ya existe en la base de datos del hospital. Consulte con personal de TIC.";
                    lblMensaje.Rows = 2;
                }
                else
                {
                    oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "HistoriaClinica", p);
                  // CargarParentesco(oPaciente, us);///preguntar si esto sigue
                }

            }
            return oPaciente;

        }

        private void CargarParentesco(Paciente oPaciente, Usuario us)
        {

            if ((hfapellidoParentesco.Value != "") & (hfnombreParentesco.Value != ""))
            {
                Parentesco par = new Parentesco();

                par.Apellido = hfapellidoParentesco.Value.ToUpper();
                par.Nombre = hfnombreParentesco.Value.ToUpper();
                par.IdTipoDocumento = 1; // Convert.ToInt32(ddlTipoDocP.SelectedValue);
                par.IdPaciente = oPaciente;
                par.TipoParentesco = "Madre";
                par.NumeroDocumento = Convert.ToInt32(hfnumeroDocumentoParentesco.Value);


                if (hffechaNacimientoParentesco.Value != "") {
                    string d3 = hffechaNacimientoParentesco.Value.Substring(0, 10);                
                    par.FechaNacimiento = DateTime.Parse(d3);
                }
                else par.FechaNacimiento = Convert.ToDateTime("01/01/1900");                     

                //par.IdProvincia = -1;
                //par.IdPais = 54;        
                par.IdUsuario = us;

                //guardo la fecha actual de modificacion
                par.FechaModificacion = DateTime.Now;
                par.Save();
            }
        }

        private bool ExisteNumeroOrigen(int p, string s_numeroProtocolo)
        {
            bool existe=false;
            Efector oEfector = new Efector();  oEfector = (Efector)oEfector.Get(typeof(Efector), p);

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Protocolo));
            crit.Add(Expression.Eq("IdEfectorSolicitante", oEfector));
            crit.Add(Expression.Eq("NumeroOrigen", s_numeroProtocolo));
            crit.Add(Expression.Eq("Baja", false));
            IList detalle = crit.List();

            if (detalle.Count > 0)  existe = true;            
            return existe;
        }
  
        private Paciente WebServiceHospital(int s_numeroDocumento, string s_apellido, string s_nombre, string s_fechanac, string s_sexo)
        {       
            int p=0;// historiaclinica
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "HistoriaClinica", p);
            return oPaciente;
        }

        public class ProtocoloDerivacion
        {
            public int Documento { get; set; }
            public string Apellidos { get; set; }public string Apellido { get; set; }
            public string Nombres { get; set; }public string Nombre { get; set; }
            public string FechaNacimiento { get; set; } public string FechaNac { get; set; }
            public string Sexo { get; set; }
            //public string Efector { get; set; }
            //public int idTipoServicio { get; set; }
            public string NumeroProtocolo { get; set; }
            public string FechaProtocolo { get; set; }
            public string CodigoAnalisis { get; set; } // los codigos de analisis separados por "|"
            public string Codigos { get; set; } // los codigos de analisis separados por "|"
            public string Sala { get; set; }
            public string Cama { get; set; }
            public int TipoServicio { get; set; }
            public string Calle { get; set; }
            public int NumeroDomicilio { get; set; }
            public string Referencia { get; set; }
            public string InformacionContacto { get; set; }

            public int DocumentoParentesco { get; set; }
            public string ApellidosParentesco { get; set; }
            public string NombresParentesco { get; set; }
            public string FechaNacimientoParentesco { get; set; }
            public string TipoParentesco { get; set; }
            public string DiagnosticoEmbarazo { get; set; }
            //public int idProtocolo { get; set; }
            //public int idEfector { get; set; }

        }

        public class ProtocoloDerivacionScreening
            {
                public int Documento { get; set; }
                public string Apellidos { get; set; }public string Apellido { get; set; }
                public string Nombres { get; set; }public string Nombre { get; set; }
                public string FechaNacimiento { get; set; } public string FechaNac { get; set; }
                public string Sexo { get; set; }
                //public string Efector { get; set; }
                //public int idTipoServicio { get; set; }
                public string NumeroProtocolo { get; set; }
                public string FechaProtocolo { get; set; }
                public string CodigoAnalisis { get; set; } // los codigos de analisis separados por "|"
                public string Codigos { get; set; } // los codigos de analisis separados por "|"
                public string Sala { get; set; }
                public string Cama { get; set; }
                public int TipoServicio { get; set; }
                public string Calle { get; set; }
                public int NumeroDomicilio { get; set; }
                public string Referencia { get; set; }
                public string InformacionContacto { get; set; }

                public int DocumentoParentesco { get; set; }
                public string ApellidosParentesco { get; set; }
                public string NombresParentesco { get; set; }
                public string FechaNacimientoParentesco { get; set; }
                public string TipoParentesco { get; set; }
                public string DiagnosticoEmbarazo { get; set; }
                /// <summary>
                /// /varaiables screening
                /// </summary>
                public int idEfectorSolicitante { get; set; }
                public string horaNacimiento { get; set; }
                public int edadGestacional { get; set; }
                public int peso { get; set; }
                public string primeraMuestra { get; set; }
                public int idMotivoRepeticion { get; set; }
                public string fechaExtraccion { get; set; }
                public string horaExtraccion { get; set; }
                public string ingestaLeche24Horas { get; set; }
                public string tipoAlimentacion { get; set; }
                public string antibiotico { get; set; }
                public string transfusion { get; set; }
                public string corticoide { get; set; }
                public string dopamina { get; set; }
                public string enfermedadTiroideaMaterna { get; set; }
                public string antecedentesMaternos { get; set; }
                public string corticoidesMaterno { get; set; }
                public string medicoSolicitante { get; set; }
                public string numeroTarjeta { get; set; }
                public int idLugarControl { get; set; }
                public string fechaCarga { get; set; }
                public string fechaEnvio { get; set; }



                //public int idProtocolo { get; set; }
                //public int idEfector { get; set; }

            }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Seleccionar")
            {
                int s_idPaciente =int.Parse( e.CommandArgument.ToString());
                ActualizarDatosPaciente(s_idPaciente);
                if (Request["isScreening"] != null)
                    Response.Redirect("ProtocoloEditPesquisa.aspx?idServicio=4&idPaciente=" + s_idPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value + "&idSolicitudScreening=" + Request["protocolo"].ToString(), false);
                else
                    Response.Redirect("ProtocoloEdit2.aspx?idServicio="+Request["idServicio"].ToString()+"&idPaciente=" + s_idPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value);


                //Response.Redirect("ProtocoloEdit2.aspx?idServicio=1&idPaciente=" + s_idPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value);
            }
            
        }

        private void ActualizarDatosPaciente(int s_idPaciente)
        {
            Paciente oPaciente = new Paciente();                     
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), s_idPaciente);

            oPaciente.Apellido = hfapellido.Value;
            oPaciente.Nombre = hfnombre.Value;

            string d3 = hffechaNacimiento.Value.Substring(0, 10);
            string D1 = DateTime.Parse(d3).ToShortDateString();
            string D2 = DateTime.Parse("01/01/1900").ToShortDateString();

            if (D1 !=D2) oPaciente.FechaNacimiento = DateTime.Parse(d3);
            
            switch (hfsexo.Value) { case "F": oPaciente.IdSexo = 2; break; case "M": oPaciente.IdSexo = 3; break; case "I": oPaciente.IdSexo = 1; break; }
            oPaciente.Referencia = hfreferencia.Value;
            oPaciente.InformacionContacto = hfinformacionContacto.Value;
            oPaciente.Save();

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton CmdSeleccionar = (LinkButton)e.Row.Cells[4].Controls[1];
                CmdSeleccionar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdSeleccionar.CommandName = "Seleccionar";
                CmdSeleccionar.ToolTip = "Seleccionar Paciente";
            }
        }

        protected void btnGenerarPaciente_Click(object sender, EventArgs e)
        {
            Paciente oPaciente = new Paciente();
            oPaciente = GenerarPaciente(0, hfapellido.Value, hfnombre.Value, hffechaNacimiento.Value, hfsexo.Value,"", 0,hfreferencia.Value,hfinformacionContacto.Value);
            if (oPaciente.IdPaciente == 0) { lblMensaje.Text = "El paciente no fue registrado en la base de datos del sistema"; lblMensaje.Rows = 2; }
            else
            {
                if (Request["isScreening"] != null)
                    Response.Redirect("ProtocoloEditPesquisa.aspx?idServicio=4&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value + "&idSolicitudScreening=" + Request["protocolo"].ToString(), false);
                else
                    Response.Redirect("ProtocoloEdit2.aspx?idServicio="+Request["idServicio"].ToString()+"&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=AltaDerivacion&numeroOrigen=" + hfnumeroProtocolo.Value + "&fechaOrden=" + hffechaProtocolo.Value + "&idEfectorSolicitante=" + hfidEfector.Value + "&analisis=" + hfanalisis.Value + "&sala=" + hfsala.Value + "&cama=" + hfcama.Value + "&diagnostico=" + hfdiagnostico.Value);
            }

        }
    }
}