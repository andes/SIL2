using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Business.Data;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Drawing;
using System.Text;
using Business.Data.Laboratorio;
//using DalSic;
//using DalPadron;

namespace WebLab.Pacientes {
    public partial class PacienteEdit : System.Web.UI.Page {

       private void Page_PreInit(object sender, System.EventArgs e)
       {
        

       }
         

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {   
                if (Session["idUsuario"]!=null)   
                {
                    txtNumeroDocumento.Focus();                
                    txtFalta.Value = DateTime.Now.ToShortDateString();
                    CargarListas();
                    //traigo los datos del Paciente
                    if (Request.QueryString["id"] != "0")
                    {
                        MostrarDatosPaciente();
                    }
                }
                else Response.Redirect("~/FinSesion.aspx", false);
            }
        }

        protected void lnkValidarRenaper_Click(object sender, EventArgs e)
        {
            MostrarDatosPaciente();
        }
        private void MostrarDatosPaciente()
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);
            HFIdPaciente.Value = id.ToString();
            Paciente pac = new Paciente();
            pac = (Paciente)pac.Get(typeof(Paciente), id);

            //si no es nuevo entonces cargo los datos y los muestro
            if (pac != null)
            {

                txtApellido.Text = pac.Apellido;
                txtNombre.Text = pac.Nombre;
                ddlSexo.SelectedValue = pac.IdSexo.ToString();
                switch (pac.IdSexo )
                {
                    
                    case 2:   HFSexo.Value = "F"; break;
                    case 3:   HFSexo.Value = "M"; break;
                }
                HFNumeroDocumento.Value = pac.NumeroDocumento.ToString();
                ddlEstadoP.SelectedValue = pac.IdEstado.ToString();


                //Image1.ImageUrl =   string.Format("../imagen.ashx?idPaciente={0}&Renaper=1", pac.IdPaciente.ToString());

            
                


               if (ddlEstadoP.SelectedValue == "2")
                    txtNumeroDocumento.Enabled = false;
                txtNumeroDocumento.Text = pac.NumeroDocumento.ToString();
                txtFechaNac.Value = pac.FechaNacimiento.ToShortDateString();
            //    txtHC.Text = pac.HistoriaClinica.ToString();
                txtHC.Enabled = false;
                txtFalta.Value = pac.FechaAlta.ToShortDateString();
                txtFalta.Disabled = true;
                //NAcionalidad -> Pais
                ddlNacionalidad.SelectedValue = pac.IdPais.ToString();
                //Provincia -> lugar de nacimiento
                ddlProvincia.SelectedValue = pac.IdProvincia.ToString();

            
                txtCalle.Text = pac.Calle;
                txtNumero.Text = Convert.ToString(pac.Numero);
                txtPiso.Text = pac.Piso;
                txtDepartamento.Text = pac.Departamento;
                txtManzana.Text = pac.Manzana;
                ddlProvinciaDomicilio.SelectedValue = pac.IdProvinciaDomicilio.ToString();
                CargarDepartamentoDomicilio();
                //traigo la localidad correspondiente a la provincia seleccionada
                ddlDepartamento.SelectedValue = pac.IdDepartamento.ToString();
                CargarLocalidadDomicilio();

                ddlLocalidad.SelectedValue = pac.IdLocalidad.ToString();
                CargarBarrioDomicilio();

                //traigo el barrio correspondiente a la localidad seleccionada
                ddlBarrio.SelectedValue = pac.IdBarrio.ToString();
                txtContacto.Text = pac.InformacionContacto;
                txtReferencia.Text = pac.Referencia;
           
               

                //DateTime.Now() = pac.FechaUltimaActualizacion;
                pac.FechaUltimaActualizacion = DateTime.Now;

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Parentesco));
                crit.Add(Expression.Eq("IdPaciente", pac));                                
                IList detalle = crit.List();

                foreach (Parentesco oParen in detalle)
                {
                    txtNumeroP.Text = oParen.NumeroDocumento.ToString();
                    txtApellidoP.Text = oParen.Apellido;
                    txtNombreP.Text = oParen.Nombre;
                    txtNombreP.Text = oParen.NumeroDocumento.ToString();
                    txtFechaN.Value = oParen.FechaNacimiento.ToShortDateString();
                    //ddlProvinciaP.SelectedValue = oParen.IdProvincia.ToString();
                    //ddlNacionalidadP.SelectedValue = oParen.IdPais.ToString();
                    //ddlNIvelInstruccionP.SelectedValue = oParen.IdNivelInstruccion.ToString();
                    //ddlSituacionLaboralP.SelectedValue = oParen.IdSituacionLaboral.ToString();
                    ddlParentesco.SelectedValue = oParen.TipoParentesco.ToString();
                    //ddlProfesionP.SelectedValue = oParen.IdProfesion.ToString();
                    //    //guardo el idParentesco que estoy editando
                    lblIdParentesco.Text = oParen.IdParentesco.ToString();
                    break;

                }
                //us.IdUsuario = pac.IdUsuario;
                //us.IdEfector = pac.IdEfector;
                //traigo los datos del primer pariente que encuentra
                //if (pac.SysParentescoRecords.Count > 0)
                //{
                //    SysParentesco par = pac.SysParentescoRecords[0];                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
                //    //txtApellidoP.Text = pac.SysParentescoRecords[0].Apellido; Ó
                //    txtApellidoP.Text = par.Apellido;
                //    txtNombreP.Text = par.Nombre;
                //    ddlTipoDocP.SelectedValue = par.IdTipoDocumento.ToString();
                //    txtNumeroP.Text = par.NumeroDocumento.ToString();
                //    txtFechaN.Text = par.FechaNacimiento.ToShortDateString();

                //    ddlProvinciaP.SelectedValue = par.IdProvincia.ToString();
                //    //ddlProvinciaP_SelectedIndexChanged(null, null);
                //    ddlNacionalidadP.SelectedValue = par.IdPais.ToString();
                //    //ddlNacionalidadP_SelectedIndexChanged(null, null);

                //    ddlNIvelInstruccionP.SelectedValue = par.IdNivelInstruccion.ToString();
                //    ddlSituacionLaboralP.SelectedValue = par.IdSituacionLaboral.ToString();
                //    ddlParentesco.SelectedValue = par.TipoParentesco.ToString();
                //    ddlProfesionP.SelectedValue = par.IdProfesion.ToString();
                //    //guardo el idParentesco que estoy editando
                //    lblIdParentesco.Text = par.IdParentesco.ToString();

                //}
                //mostrarFoto(pac);

            }
            else
            {

                txtNumeroDocumento.Text = Request.QueryString["dni"];
                //if (Request.QueryString["llamada"] == "Labo")
                //{
                //    txtNumeroDocumento.Text = tbParamA.Value;
                //   // tbParamA.Visible = false;
                //}

            }
        }

       

        private void CargarListas()
        {            
            Utility oUtil = new Utility();

            ///Carga de combos de tipos de servicios
            string m_ssql = "select idProvincia, nombre from Sys_Provincia WHERE idPais= 54" ;
            oUtil.CargarCombo(ddlProvinciaDomicilio, m_ssql, "idProvincia", "nombre");
            ddlProvinciaDomicilio.SelectedValue = "139";

            CargarDepartamentoDomicilio();
            ///Carga de combos de estado
            m_ssql = "select idEstado, nombre from Sys_Estado ";
            oUtil.CargarCombo(ddlEstadoP, m_ssql, "idEstado", "nombre");

           

            ///Carga de combos de sexo
            m_ssql = "select idsexo, nombre from Sys_Sexo";
            oUtil.CargarCombo(ddlSexo, m_ssql, "idsexo", "nombre");
            ddlSexo.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            ///Carga de combos de EstadoCivil
            m_ssql = "select idEstadoCivil, nombre from Sys_EstadoCivil";
            oUtil.CargarCombo(ddlECivil, m_ssql, "idEstadoCivil", "nombre");

            ///Carga de combos de nacionalidad
            m_ssql = "select idPais, nombre from Sys_Pais";
            oUtil.CargarCombo(ddlNacionalidadP, m_ssql, "idPais", "nombre");
            ddlNacionalidadP.SelectedValue = "54";
            CargarProvinciasP();

            oUtil.CargarCombo(ddlNacionalidad, m_ssql, "idPais", "nombre");
            ddlNacionalidad.SelectedValue = "54";            
            CargarProvincias();


        }

        protected void btnGuardar_Click(object sender, EventArgs e) {
            
                switch (ddlEstadoP.SelectedValue)
                {
                    case "1":
                        Page.Validate("I");
                        break;
                    case "2":
                        Page.Validate("T");
                        break;
                    case "3":
                        Page.Validate("V");
                        break;
                    /* case "4":
                         Page.Validate("TBB");
                         break;*/
                }

                if (ddlMotivoNI.SelectedValue == "1") Page.Validate("TBB");

                if (Page.IsValid)
                {
                    if (DatosValidos(1))
                    {
                        Guardar();
                    }
                }
        }

        private void Guardar()
        {
            Utility oUtil = new Utility();

                Configuracion oC = new Configuracion(); 

            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
                          
            //instancio el usuario
            Usuario us = new Usuario();
            us=(Usuario) us.Get(typeof(Usuario),int.Parse( Session["idUsuario"].ToString()));            

            int id = Convert.ToInt32(Request.QueryString["id"]);
            //datos del Paciente           
            Paciente pac = new Paciente();
            if (id != 0) pac = (Paciente)pac.Get(typeof(Paciente), id);


            pac.IdEfector = oC.IdEfector; // us.IdEfector;
            pac.Apellido =oUtil.SacaComillas( txtApellido.Text.ToUpper());
            pac.Nombre =oUtil.SacaComillas( txtNombre.Text.ToUpper());

            pac.IdEstado = Convert.ToInt32(ddlEstadoP.SelectedValue);

            if (ddlEstadoP.SelectedValue == "2")
            {
                if (id == 0) // solo si es nuevo genera un numero
                {
                    pac.NumeroDocumento = pac.generarNumero();
                }
            }
            else
            {
                if (txtNumeroDocumento.Text != "") pac.NumeroDocumento = int.Parse(txtNumeroDocumento.Text);
            }

            
            //if (!string.IsNullOrEmpty(txtHC.Text))            
            //pac.HistoriaClinica = Convert.ToInt32(txtHC.Text);            
            //else            
            //pac.HistoriaClinica = 0;            

            pac.FechaAlta =Convert.ToDateTime( txtFalta.Value);
            pac.IdSexo = Convert.ToInt32(ddlSexo.SelectedValue);
            //valido que la fecha no se mayor a la actual
            if ((Convert.ToDateTime(txtFechaNac.Value) <= DateTime.Now) && (txtFechaNac.Value != null))
            {
            pac.FechaNacimiento = Convert.ToDateTime(txtFechaNac.Value);
            }
            pac.IdPais = Convert.ToInt32(ddlNacionalidad.SelectedValue);

            if (ddlProvincia.SelectedValue != "")
            {
            pac.IdProvincia = Convert.ToInt32(ddlProvincia.SelectedValue);
            }

         

            pac.Calle = txtCalle.Text.ToUpper();
            if (!string.IsNullOrEmpty(txtNumero.Text))
            {
            pac.Numero = Convert.ToInt32(txtNumero.Text);
            }
            else
            {
            pac.Numero = 0;
            }

            pac.Piso = txtPiso.Text;
            pac.Departamento = txtDepartamento.Text;
            pac.Manzana = txtManzana.Text.ToString();
            //if ()
            if (ddlProvinciaDomicilio.SelectedValue!="") pac.IdProvinciaDomicilio = Convert.ToInt32(ddlProvinciaDomicilio.SelectedValue);
            if (ddlDepartamento.SelectedValue != "") pac.IdDepartamento = Convert.ToInt32(ddlDepartamento.SelectedValue);
            if (ddlLocalidad.SelectedValue != "") pac.IdLocalidad = Convert.ToInt32(ddlLocalidad.SelectedValue);
            if (ddlBarrio.SelectedValue!="") pac.IdBarrio = Convert.ToInt32(ddlBarrio.SelectedValue);
            pac.Referencia =oUtil.SacaComillas( txtReferencia.Text.ToUpper());

            
            pac.InformacionContacto =oUtil.SacaComillas( txtContacto.Text);           
            pac.FechaDefuncion = Convert.ToDateTime("01/01/1900");           
            pac.IdUsuario = us.IdUsuario;                        
            pac.FechaUltimaActualizacion = DateTime.Now;
            pac.Save();
            //Guardo los datos del Parentesco. Traigo con lblbIdParentesco el idParentesco asociado al paciente

            //par = (Parentesco)par.Get(typeof(Parentesco),"IdPaciente", pac.IdPaciente);
            if ((txtApellidoP.Text != "") & (txtNombreP.Text != ""))
            {  
                Parentesco par = new Parentesco();
                 if (lblIdParentesco.Text != "")
                     par = (Parentesco)par.Get(typeof(Parentesco), "IdParentesco", int.Parse(lblIdParentesco.Text));


                par.Apellido =oUtil.SacaComillas( txtApellidoP.Text.ToUpper());
                par.Nombre = oUtil.SacaComillas(txtNombreP.Text.ToUpper());
                par.IdTipoDocumento = 1; // Convert.ToInt32(ddlTipoDocP.SelectedValue);
                par.IdPaciente = pac;
                if (ddlParentesco.SelectedValue != "")
                    par.TipoParentesco = ddlParentesco.SelectedValue;
                if (!string.IsNullOrEmpty(txtNumeroP.Text))
                    par.NumeroDocumento = Convert.ToInt32(txtNumeroP.Text);
                //par.NumeroDocumento = Convert.ToInt32(txtNumeroP.Text);
                if (txtFechaN.Value != "")
                    par.FechaNacimiento = Convert.ToDateTime(txtFechaN.Value);
                else
                    par.FechaNacimiento = Convert.ToDateTime("01/01/1900");

                //if (ddlProvinciaP.SelectedValue != "0")
                //    par.IdProvincia = Convert.ToInt32(ddlProvinciaP.SelectedValue);
                //    //par.IdProvincia = Convert.ToInt32(ddlProvinciaP.SelectedValue);
                //if (ddlNacionalidadP.SelectedValue != "0")
                //    par.IdPais = Convert.ToInt32(ddlNacionalidadP.SelectedValue);
              

                par.IdUsuario = us;

                //guardo la fecha actual de modificacion
                par.FechaModificacion = DateTime.Now;
                par.Save();
            }



            if (Request["llamada"] != null)
            {
                if (Request["llamada"] == "LaboTurno")
                    Response.Redirect("../Turnos/TurnosEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&Modifica=0");
                if (Request["llamada"] == "LaboProtocolo")
                {
                    if (Request["idProtocolo"] == null)
                    {
                        if (Request["idServicio"].ToString() == "6")
                            
                        Response.Redirect("../Protocolos/Consentimiento.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&idServicio=6");
                        else
                            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta");
                    }
                    else
                        Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + pac.IdPaciente.ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&Desde=" + Request["Desde"].ToString());
                }
            }
         


          
        }

      

        private bool DatosValidos(int id)
        {
            lblMensaje.Text = string.Empty;
            /*  if (txtHC.Text.Trim().Length > 0 && txtHC.Text != "0") {
                  //consulto en la tabla paciente si el numero de HClinica ingresado ya existe
                  DataTable dt = BuscarHClinica(id);
                  if (dt.Rows.Count > 0) {
                      lblMensaje.Text = "El número de Historia Clínica ingresado ya existe. <br/>";
                  }
              } */

            DateTime fec = Convert.ToDateTime("01/01/1900");
            DateTime.TryParse(txtFechaNac.Value, out fec);
            if ((fec > DateTime.Now) && (txtFechaNac.Value == ""))
            {
                //pac.FechaNacimiento = Convert.ToDateTime(txtFechaNac.Text);
                lblMensaje.Text = "Debe ingresar una fecha de nacimiento válida. <br/>";
            }

            //VerificarNumeroDoc();
            switch (ddlEstadoP.SelectedValue)
            {
                case "1":
                    Page.Validate("I");
                    break;
                case "2":
                    Page.Validate("T");
                    break;
                case "3":
                    Page.Validate("V");
                    break;
                /* case "4":
                     Page.Validate("TBB");
                     break; */
            }

            if (ddlMotivoNI.SelectedValue == "1") Page.Validate("TBB");

            // SI el estado es 1 o 3 Busco los posible duplicados, sino, no me interesa y no los busco
            if ((ddlEstadoP.SelectedValue == "1") || (ddlEstadoP.SelectedValue == "3"))
            {
                // Me fijo si ha llenado el nro de doc q es requerido en estos casos
                if (!string.IsNullOrEmpty(txtNumeroDocumento.Text.Trim()))
                {
                    //consulto en la tabla paciente si el tipo y numero de documento ingresado ya existe
                    IList dtd = BuscarNumeroDoc(id);
                    if (dtd.Count > 0)
                    {
                        lblMensaje.Text += "El número ingresado ya existe! <br/>";
                        txtNumeroDocumento.Focus();
                    }
                }
                else
                {
                    lblMensaje.Text += "El DNI es requerido! <br/>";
                    Page.Validate("I");
                    Label1.Text += "<br />Estado Identificado: " + Page.IsValid.ToString();
                }
            }
            if (lblMensaje.Text == string.Empty)
            {
                return true;
            }
            return false;
        }


        protected void ddlTipoDoc_SelectedIndexChanged(object sender, EventArgs e) {
            Boolean habilitado = true; // ddlTipoDocP.SelectedItem.Text != "SD";
            txtNumeroDocumento.Text = habilitado ? txtNumeroDocumento.Text : "";
            //pregunto si el tipo de doc es <> de SD e inhabilito el control
            txtNumeroDocumento.Enabled = habilitado;

        }

        protected void ddlNacionalidad_SelectedIndexChanged(object sender, EventArgs e) {
            //SubSonic.Select n = new SubSonic.Select();
            //n.From(DalSic.SysProvincium.Schema);
            //n.Where(SysProvincium.Columns.IdPais).IsEqualTo(ddlNacionalidad.SelectedValue);
            //n.Or(SysProvincium.Columns.IdProvincia).IsEqualTo(0);

            //ddlProvincia.DataSource = n.ExecuteTypedList<SysProvincium>();
            //ddlProvincia.DataBind();
        }

        protected void ddlNacionalidadP_SelectedIndexChanged(object sender, EventArgs e)
        {
         /*   SubSonic.Select n = new SubSonic.Select();
            n.From(DalSic.SysProvincium.Schema);
            n.Where(SysProvincium.Columns.IdPais).IsEqualTo(ddlNacionalidadP.SelectedValue);
            n.Or(SysProvincium.Columns.IdProvincia).IsEqualTo(0);

            ddlProvinciaP.DataSource = n.ExecuteTypedList<SysProvincium>();
            ddlProvinciaP.DataBind();*/


            CargarProvinciasP();
            ddlProvinciaP.UpdateAfterCallBack = true;
            Panel3.UpdateAfterCallBack = true;
        }

        private void CargarProvinciasP()
        {
            Utility oUtil = new Utility();

            if (ddlNacionalidadP.SelectedValue != "")
            {
                ///Carga de combos de tipos de servicios
                string m_ssql = "select idprovincia, nombre from Sys_provincia WHERE idpais= " + ddlNacionalidadP.SelectedValue;
                oUtil.CargarCombo(ddlProvinciaP, m_ssql, "idprovincia", "nombre");
                ddlProvinciaP.Items.Insert(0, new ListItem("Seleccione", "0"));
            }
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e) {
            // no nace nada
        }

        protected void ddlProvinciaP_SelectedIndexChanged(object sender, EventArgs e)
        {
            // no nace nada
        }

        protected void btnCancelar_Click(object sender, EventArgs e) {
            Response.Redirect("../Protocolos/Default.aspx?idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString(), false);

        }

        protected void ddlProvinciaDomicilio_SelectedIndexChanged(object sender, EventArgs e) {
            //Selecciono el item del combo Provincia y filtro las localidades de la provincia
            //SubSonic.Select p = new SubSonic.Select();
            //p.From(DalSic.SysLocalidad.Schema);
            //p.Where(SysLocalidad.Columns.IdProvincia).IsEqualTo(ddlProvinciaDomicilio.SelectedValue);
            //p.Or(SysLocalidad.Columns.IdLocalidad).IsEqualTo(0);

            //ddlLocalidad.DataSource = p.ExecuteTypedList<SysLocalidad>();
            //ddlLocalidad.DataBind();

            CargarDepartamentoDomicilio();
            ddlDepartamento.UpdateAfterCallBack = true;
            Panel2.UpdateAfterCallBack = true;
        }

        private void CargarDepartamentoDomicilio()
        {
            Utility oUtil = new Utility();

            if (ddlProvinciaDomicilio.SelectedValue != "")
            {
                ///Carga de combos de tipos de servicios
                string m_ssql = "select iddepartamento, nombre from Sys_departamento WHERE idProvincia= " + ddlProvinciaDomicilio.SelectedValue;
                oUtil.CargarCombo(ddlDepartamento, m_ssql, "iddepartamento", "nombre");
                ddlDepartamento.Items.Insert(0, new ListItem("Seleccione", "0"));
            }
        }

        protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarLocalidadDomicilio();
            ddlLocalidad.UpdateAfterCallBack = true;

            Panel2.UpdateAfterCallBack = true;
        }

        private void CargarLocalidadDomicilio()
        {
            Utility oUtil = new Utility();

            if (ddlDepartamento.SelectedValue != "")
            {
                ///Carga de combos de tipos de servicios
                string m_ssql = "select idLocalidad, nombre from Sys_localidad WHERE iddepartamento= " + ddlDepartamento.SelectedValue;
                oUtil.CargarCombo(ddlLocalidad, m_ssql, "idLocalidad", "nombre");
                ddlLocalidad.Items.Insert(0, new ListItem("Seleccione", "0"));
            }
        }

             protected void ddlLocalidad_SelectedIndexChanged(object sender, EventArgs e) 
             {
                 CargarBarrioDomicilio();

                 ddlBarrio.UpdateAfterCallBack = true;
                 Panel2.UpdateAfterCallBack = true;
        }

             private void CargarBarrioDomicilio()
             {
                 Utility oUtil = new Utility();

                 if (ddlLocalidad.SelectedValue != "")
                 {
                     ///Carga de combos de tipos de servicios
                     string m_ssql = "select idBarrio, nombre from Sys_Barrio WHERE idLocalidad= " + ddlLocalidad.SelectedValue;
                     oUtil.CargarCombo(ddlBarrio, m_ssql, "idBarrio", "nombre");
                     ddlBarrio.Items.Insert(0, new ListItem("Seleccione", "0"));
                 } 
             }

        protected void VerificarNumeroDoc() 
        {
            lblMensaje.Text = string.Empty;
            int id = Convert.ToInt32(Request.QueryString["id"]);

            //consulto en la tabla paciente si el tipo y numero de documento ingresado ya existe
            IList dtd = BuscarNumeroDoc(id);
            if (dtd.Count>0) {
                lblMensaje.Visible = true;
                
                lblMensaje.Text += "El número de DU ingresado ya existe. <br/>";
                lblMensaje.ForeColor = Color.Red;

            } else {
                //lblMensaje.Text += "El tipo y número de Documento es valido. <br/>";
                //lblMensaje.ForeColor = Color.Black;
                lblMensaje.Visible = false;
            }
        

        }

        protected void VerificarHClinica(object sender, EventArgs e)
        {
            //lblMensaje.Text = string.Empty;
            //int id = Convert.ToInt32(Request.QueryString["id"]);

            //consulto en la tabla paciente si la HClinica ingresada <>0 ya existe
            //DataTable dtd = BuscarHClinica(id);
            //if (dtd.Rows.Count > 0)
            //{
            //    lblMensaje.Text += "El número de Historia Clínica ingresado ya existe. <br/>";
            //    txtHC.Focus();   
            //}
            //else
            //{
            //    lblMensaje.Text += "El número de Historia Clínica es válido. <br/>";
            //}

        }

        private IList BuscarNumeroDoc(int id)
        {

            if (Request.QueryString["id"] != "0")
                id = int.Parse(Request.QueryString["id"]);

            ISession m_session = NHibernateHttpModule.CurrentSession;


            ICriteria crit = m_session.CreateCriteria(typeof(Paciente));

            crit.Add(Expression.Eq("IdEstado", ddlEstadoP.SelectedValue));
            crit.Add(Expression.Eq("NumeroDocumento", txtNumeroDocumento.Text));
            if (Request.QueryString["id"] != "0") crit.Add(Expression.Not(Expression.Eq("IdPaciente", id)));


            IList detalle = crit.List();

            return detalle;
        }

           protected void         ddlEstadoP_SelectedIndexChanged(object sender, EventArgs e)
        {
            Utility oUtil = new Utility();

            if (ddlEstadoP.SelectedValue == "2")
            {
                ///Carga de combos de motivo no identificación
                string m_ssql = "select idmotivoNI, nombre from Sys_MotivoNI ";
                oUtil.CargarCombo(ddlMotivoNI, m_ssql, "idmotivoNI", "nombre");
                ddlMotivoNI.Enabled = true;
                txtNumeroDocumento.Text = "";
                txtNumeroDocumento.Enabled = false;
                rfvDI.Enabled = false;
                lblMensaje.Text = "";
            }
            else
            {
                txtNumeroDocumento.Enabled = true;
                ddlMotivoNI.Items.Clear();
                ddlMotivoNI.Enabled = false;
                rfvDI.Enabled = true;                
            }


            Panel1.UpdateAfterCallBack = true;
        }

           //protected void txtNumeroDocumento_TextChanged(object sender, EventArgs e)
           //{
            
           //    Panel1.UpdateAfterCallBack = true;
           //}

           protected void ddlNacionalidad_SelectedIndexChanged1(object sender, EventArgs e)
           {
               CargarProvincias();
               ddlProvincia.UpdateAfterCallBack = true;
           }

           private void CargarProvincias()
           {
               Utility oUtil = new Utility();

               if (ddlNacionalidad.SelectedValue != "")
               {
                   ///Carga de combos de tipos de servicios
                   string m_ssql = "select idprovincia, nombre from Sys_provincia WHERE idpais= " + ddlNacionalidad.SelectedValue;
                   oUtil.CargarCombo(ddlProvincia, m_ssql, "idprovincia", "nombre");
                   ddlProvincia.Items.Insert(0, new ListItem("Seleccione", "0"));
               }
           }

        protected void btnGuardar0_Click(object sender, EventArgs e)
        {

        }



        //private DataTable BuscarHClinica(int id) {
        //    SubSonic.Select h = new SubSonic.Select();
        //    h.From(DalSic.SysPaciente.Schema);
        //    h.Where(SysPaciente.Columns.HistoriaClinica).IsEqualTo(txtHC.Text);
        //    h.And(SysPaciente.Columns.IdPaciente).IsNotEqualTo(id);
        //    DataTable dt = h.ExecuteDataSet().Tables[0];
        //    return dt;
        //}

    }


}


