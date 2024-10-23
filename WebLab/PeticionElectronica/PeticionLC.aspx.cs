using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.PeticionElectronica
{
    public partial class PeticionLC : System.Web.UI.Page
    {

        private Random random = new Random();
        private static int TEST = 0;

        private bool IsTokenValid()
        {
            bool result = double.Parse(hidToken.Value) == ((double)Session["NextToken"]);
            SetToken();
            return result;
        }

        private void SetToken()
        {
            double next = random.Next();
            hidToken.Value = next + "";
            Session["NextToken"] = next;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ( Session["idEfectorSolicitante"]!=null)
                { 
                    SetToken();
                    txtNumeroDocumento.Focus();
                    txtFecha.Value= DateTime.Now.ToShortDateString();
                    CargarListas();
                    if (Request["idPeticion"] != null)
                    {
                        MostrarPeticion(Request["idPeticion"].ToString());                  

                    }
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }

        }

        private void MostrarPeticion(string p)
        {
            Peticion Pe = new Peticion();
            Pe = (Peticion)Pe.Get(int.Parse(p));
            idPeticion.Text = "PETICION NRO. " + Pe.IdPeticion.ToString();
            idPeticion.Visible = true;
            if (Pe.IdPaciente.IdEstado != 2)  //identificado
                txtNumeroDocumento.Text = Pe.IdPaciente.NumeroDocumento.ToString();
            else
            {
                rdbRN.SelectedValue = "Si"; rdbRN.Enabled = false;
                txtNumeroDocumento.Text = "";
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Parentesco));
                crit.Add(Expression.Eq("IdPaciente", Pe.IdPaciente));
                IList detalle = crit.List();

                foreach (Parentesco oParen in detalle)
                {
                    txtNumeroP.Text = oParen.NumeroDocumento.ToString();
                    txtApellidoP.Text = oParen.Apellido;
                    txtNombreP.Text = oParen.Nombre;
                    break;
                }

            }
            hidPaciente.Text = Pe.IdPaciente.IdPaciente.ToString();
            txtApellido.Text = Pe.Apellido;
            txtNombre.Text = Pe.Nombre;
            txtFechaNac.Text = Pe.FechaNacimiento.ToShortDateString();

            ddlSexo.SelectedValue = Pe.IdPaciente.IdSexo.ToString();

            txtNumeroDocumento.Enabled = false;
            txtNumeroP.Enabled = true;
            txtApellido.Enabled = true;
            txtApellidoP.Enabled = true;
            txtNombre.Enabled = true;
            txtNombreP.Enabled = true;
            txtFechaNac.Enabled = true;
            ddlSexo.Enabled = true;

            txtNumeroDocumento.UpdateAfterCallBack = false;
            txtNumeroP.UpdateAfterCallBack = true;
            txtApellido.UpdateAfterCallBack = true;
            txtApellidoP.UpdateAfterCallBack = true;
            txtNombre.UpdateAfterCallBack = true;
            txtNombreP.UpdateAfterCallBack = true;
            txtFechaNac.UpdateAfterCallBack = true;
            ddlSexo.UpdateAfterCallBack = true;

            txtFecha.Value = Pe.Fecha.ToShortDateString();
            rdbOrigen.SelectedValue = Pe.IdOrigen.IdOrigen.ToString();
            
            txtObservaciones.Text = Pe.Observaciones;
            OSociales.setOS(Pe.IdObraSocial);

            //MostrarAdjunto(Pe);




        }

        //private void MostrarAdjunto(Peticion pe)
        //{

        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(PeticionAnexo));
        //    crit.Add(Expression.Eq("IdPeticion", pe));
            
        //    IList detalle = crit.List();
        //    int i = 1;
        //    foreach (PeticionAnexo oParen in detalle)
        //    {
        //        if (i == 1) {
        //            Anexo1.Text = oParen.Url;  Anexo1.Visible = true; FileUpload1.Enabled = false;Anexo1.NavigateUrl= pe.IdPeticion.ToString() + "\\" + oParen.Url;
        //            btnAnulaAnexo1.CommandArgument = oParen.IdPeticionAnexo.ToString();
        //            btnAnulaAnexo1.Visible = true;
        //        }
        //        if (i == 2) { Anexo2.Text = oParen.Url; Anexo2.Visible = true; Anexo2.NavigateUrl = pe.IdPeticion.ToString() + "\\" + oParen.Url; FileUpload2.Enabled = false;
        //            btnAnulaAnexo2.Visible = true;
        //        }
        //        if (i == 3) { Anexo3.Text = oParen.Url; Anexo3.Visible = true; Anexo3.NavigateUrl = pe.IdPeticion.ToString() + "\\" + oParen.Url; FileUpload3.Enabled = false;
        //            btnAnulaAnexo3.Visible = true;
        //        }

        //        i = i + 1;
        //    }
        //}

        private void CargarListas()
        {
            try
            {
                Utility oUtil = new Utility();

                Efector oEfector = new Efector();
                oEfector = (Efector)oEfector.Get(int.Parse(Session["idEfectorSolicitante"].ToString()));
                lblEfectorSolicitante.Text = oEfector.Nombre;

                ///Carga de combos de sexo
                string m_ssql = "select idsexo, nombre from Sys_Sexo where idsexo>1";
                oUtil.CargarCombo(ddlSexo, m_ssql, "idsexo", "nombre");
                ddlSexo.Items.Insert(0, new ListItem("--Seleccione--", "0"));
            }
            catch (Exception ex)
            { Response.Redirect("../AccesoDenegado.htm"); }

        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MostrarDatosPaciente();
        }

        private void MostrarDatosPaciente()
        {
            try
            {
                if (txtNumeroDocumento.Text != "")
                {
                    Paciente pac = new Paciente();
                    pac = (Paciente)pac.Get(typeof(Paciente), "NumeroDocumento", int.Parse(txtNumeroDocumento.Text));

                    //si no es nuevo entonces cargo los datos y los muestro
                    if (pac != null)
                    {
                        VerificaPeticionCurso(pac);

                        hidPaciente.Text = pac.IdPaciente.ToString();
                        txtApellido.Text = pac.Apellido;
                        txtNombre.Text = pac.Nombre;
                        ddlSexo.SelectedValue = pac.IdSexo.ToString();
                        //ddlEstadoP.SelectedValue = pac.IdEstado.ToString();

                        if (rdbRN.SelectedValue == "1")

                        {
                            rfvDni.Enabled = false;

                            txtNumeroDocumento.Enabled = false;
                            lblApellidoMadre.Visible = false;
                            lblDniMadre.Visible = false;
                            lblNombreMadre.Visible = false;
                        }
                        txtNumeroDocumento.Text = pac.NumeroDocumento.ToString();
                        txtFechaNac.Text = pac.FechaNacimiento.ToShortDateString();


                        
                        txtApellido.Enabled = true;
                        txtNombre.Enabled = true;
                        ddlSexo.Enabled = true;
                        txtApellidoP.Enabled = true;
                        txtNombreP.Enabled = true;
                        txtNumeroP.Enabled = true;
                        txtFechaNac.Enabled = true;

                    }
                    else
                    {
                        rfvDni.Enabled = true;
                        txtApellido.Text = "";
                        txtNombre.Text = "";
                        ddlSexo.SelectedValue = "0";
                        txtApellidoP.Text = "";
                        txtNombreP.Text = "";
                        txtNumeroP.Text = "";
                        txtFechaNac.Text = "";

                        txtApellido.Enabled = true;
                        txtNombre.Enabled = true;
                        ddlSexo.Enabled = true;
                        txtApellidoP.Enabled = true;
                        txtNombreP.Enabled = true;
                        txtNumeroP.Enabled = true;
                        txtFechaNac.Enabled = true;

                        lblApellidoMadre.Visible = true;
                        lblDniMadre.Visible = true;
                        lblNombreMadre.Visible = true;

                        lblMensaje.Text = "No se encontró paciente con ese numero de documento. Complete los datos requeridos.";
                        hidPaciente.Text = "";

                    }
                    lblApellidoMadre.UpdateAfterCallBack = true;
                    lblDniMadre.UpdateAfterCallBack = true;
                    lblNombreMadre.UpdateAfterCallBack = true;
                    rfvDni.UpdateAfterCallBack = true;
                 
                    lblMensaje2.UpdateAfterCallBack = true;
                    lblMensaje.AutoUpdateAfterCallBack = true;
                    txtApellido.AutoUpdateAfterCallBack = true;
                    txtNombre.AutoUpdateAfterCallBack = true;
                    ddlSexo.AutoUpdateAfterCallBack = true;
                    txtApellidoP.AutoUpdateAfterCallBack = true;
                    txtNombreP.AutoUpdateAfterCallBack = true;
                    txtNumeroP.AutoUpdateAfterCallBack = true;
                    txtFechaNac.AutoUpdateAfterCallBack = true;
                    hidPaciente.AutoUpdateAfterCallBack = true;

                }
            }
            catch {
                lblMensaje.Text ="El valor ingresado no es correcto";
                lblMensaje.AutoUpdateAfterCallBack = true;
            }
        }

        private void VerificaPeticionCurso(Paciente pac)
        {
            Efector oEfector = new Efector();


            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Session["idEfectorSolicitante"].ToString()));

            //busca peticiones en el efector en la ultimos 7 dias
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Peticion));
            crit.Add(Expression.Eq("IdPaciente", pac));
            crit.Add(Expression.Eq("IdEfector", oEfector));
            crit.Add(Expression.Ge("Fecha", DateTime.Now.AddDays(-7)));
            lblMensaje2.Visible = true;
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {// hay una peticion en curso para el paciente en los ultimos 7 dias. Si el pedido es diferente al ya solicitado continue con la carga.
                lblMensaje2.Text = "Hay una petición en curso para el paciente en los últimos 7 dias. Si el pedido es diferente al ya solicitado continue con la carga.";

            }
            else
                lblMensaje2.Text = "";
            lblMensaje2.UpdateAfterCallBack = true;

        }

        protected void lnkGuardar_Click(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if (Page.IsValid)
                    GuardarPeticion();
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void GuardarPeticion()
        {
           
            if (IsTokenValid())
            {
                Peticion oRegistro = new Peticion();

                if (Request["idPeticion"] != null)
                    oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));

                TEST++;
                
                Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
                //Actualiza los datos de los objetos : alta o modificacion .       
                Usuario oUser = new Usuario();
                TipoServicio oServicio = new TipoServicio();
                Paciente oPaciente = new Paciente();
                if (hidPaciente.Text == "")
                {
                    hidPaciente.Text = NuevoPaciente();
                }
               

                    oRegistro.Apellido = txtApellido.Text;
                    oRegistro.Nombre = txtNombre.Text;
                    oRegistro.IdSexo = int.Parse(ddlSexo.SelectedValue);
                    oRegistro.FechaNacimiento = DateTime.Parse(txtFechaNac.Text);

                
                Origen oOrigen = new Origen();
                SectorServicio oSector = new SectorServicio();
                Efector oEfector = new Efector();


                oRegistro.IdEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Session["idEfectorSolicitante"].ToString()));
                
                oRegistro.IdTipoServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), 3); ///microbiologia



                oRegistro.Fecha = DateTime.Parse( txtFecha.Value.ToString());
                oRegistro.Hora = DateTime.Now.ToShortTimeString();

                ///Desde aca guarda los datos del paciente en Turno
                oRegistro.IdPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(hidPaciente.Text));
                oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), int.Parse(rdbOrigen.SelectedValue)); ///ambulatorio
                oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.IdMuestra = 0; // int.Parse(ddlMuestra.SelectedValue);
                oRegistro.Observaciones = txtObservaciones.Text;

                oRegistro.IdObraSocial =   OSociales.getObraSocial();
                oRegistro.IdSector = (SectorServicio)oSector.Get(typeof(SectorServicio), 55); /// el id de Privado
                oRegistro.IdSolicitante = 0; // int.Parse(ddlEspecialista.SelectedValue); // lblSolicitante.Text; /// analizar la posibilidad de vincular al idUsuario
                oRegistro.Sala = "";// txtSala.Text;
                oRegistro.Cama = ""; //txtCama.Text;

                oRegistro.Save();

                //GuardarAnexo1(oRegistro);
                //GuardarAnexo2(oRegistro);
                //GuardarAnexo3(oRegistro);
                
                Response.Redirect("PeticionMensaje.aspx?id=" + oRegistro.IdPeticion.ToString());


            }
            else
            { //doble submit
            }
            }
            
        
       

        private string NuevoPaciente()
        {
            Utility oUtil = new Utility();
            //instancio el usuario
            Usuario us = new Usuario();
            us = (Usuario)us.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            int id = Convert.ToInt32(Request.QueryString["id"]);
            //datos del Paciente           
            Paciente pac = new Paciente();
           


            pac.IdEfector = us.IdEfector;
            pac.Apellido = oUtil.SacaComillas(txtApellido.Text.ToUpper());
            pac.Nombre = oUtil.SacaComillas(txtNombre.Text.ToUpper());


            if ((rdbRN.SelectedValue == "1") && (txtNumeroDocumento.Enabled == false))
            {
                pac.IdEstado = 2;
                pac.NumeroDocumento = pac.generarNumero();
            }
            else
            {
                pac.NumeroDocumento = Convert.ToInt32(txtNumeroDocumento.Text);
                pac.IdEstado = 1;
            }




            pac.FechaAlta = DateTime.Now;
            pac.IdSexo = Convert.ToInt32(ddlSexo.SelectedValue);
           

                pac.FechaNacimiento = Convert.ToDateTime(txtFechaNac.Text);


            pac.IdPais = -1; // Convert.ToInt32(ddlNacionalidad.SelectedValue);

            
            
            pac.Calle = "";
           
               
           

            pac.Piso = "";
            pac.Departamento = "";
            pac.Manzana = "";
            //if ()
            

            pac.Referencia = "";


            pac.InformacionContacto = "";
            pac.FechaDefuncion = Convert.ToDateTime("01/01/1900");
            pac.IdUsuario = us.IdUsuario;
            pac.FechaUltimaActualizacion = DateTime.Now;
            pac.Save();
            //Guardo los datos del Parentesco. Traigo con lblbIdParentesco el idParentesco asociado al paciente

            //par = (Parentesco)par.Get(typeof(Parentesco),"IdPaciente", pac.IdPaciente);
            if ((txtApellidoP.Text != "") & (txtNombreP.Text != ""))
            {
                Parentesco par = new Parentesco();
           

                par.Apellido = oUtil.SacaComillas(txtApellidoP.Text.ToUpper());
                par.Nombre = oUtil.SacaComillas(txtNombreP.Text.ToUpper());
                par.IdTipoDocumento = 1; // Convert.ToInt32(ddlTipoDocP.SelectedValue);
                par.IdPaciente = pac;
             
                    par.TipoParentesco = "Madre";
                if (!string.IsNullOrEmpty(txtNumeroP.Text))
                    par.NumeroDocumento = Convert.ToInt32(txtNumeroP.Text);
                //par.NumeroDocumento = Convert.ToInt32(txtNumeroP.Text);
              

                    par.FechaNacimiento = Convert.ToDateTime("01/01/1900");


                par.IdUsuario = us;

                //guardo la fecha actual de modificacion
                par.FechaModificacion = DateTime.Now;
                par.Save();
            }



            return pac.IdPaciente.ToString();
        }

        private bool noexiste(Peticion oOs, string fileName)
        {
            /// armar un peticionadjunto
            ProtocoloAnexo r = new ProtocoloAnexo();
            r = (ProtocoloAnexo)r.Get(typeof(ProtocoloAnexo), "IdProtocolo", oOs, "Url", fileName);
            if (r != null)
                return true;
            else
                return false;
        }
        //protected void GuardarAnexo1(Peticion oOs)
        //{
        //    try
        //    {
               

        //        Boolean fileOK = false;
        //        if (FileUpload1.HasFile)
        //        {
        //            string directorio = Server.MapPath("") + "\\" + oOs.IdPeticion.ToString();


        //            if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

        //            string archivo = directorio + "\\" + FileUpload1.FileName;

        //            string extension = System.IO.Path.GetExtension(archivo).ToLower();
        //            String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".pdf" };
        //            for (int i = 0; i < allowedExtensions.Length; i++)
        //            {
        //                if (extension == allowedExtensions[i])
        //                {
        //                    fileOK = true;
        //                }
        //            }

        //            if (fileOK)
        //            {



        //                if (!noexiste(oOs, FileUpload1.FileName))
        //                {
        //                    FileUpload1.SaveAs(archivo);
        //                    Usuario oUser = new Usuario();
        //                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        //                    oOs.GuardarAnexo(FileUpload1.FileName, oUser);


        //                }
        //                else
        //                    estatus.Text = "ya existe un archivo con ese nombre para la peticion";

        //            }
        //            else
        //                estatus.Text = "no se acepta el tipo de archivo. Verifique que tengan extension .gif,png,.jpeg,jpg, pdf";

        //        }
        //    }
        //    catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }
        //}
        //protected void GuardarAnexo2(Peticion oOs)
        //{
        //    try
        //    {


        //        Boolean fileOK = false;
        //        if (FileUpload2.HasFile)
        //        {
        //            string directorio = Server.MapPath("") + "\\" + oOs.IdPeticion.ToString();


        //            if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

        //            string archivo = directorio + "\\" + FileUpload2.FileName;

        //            string extension = System.IO.Path.GetExtension(archivo).ToLower();
        //            String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".pdf" };
        //            for (int i = 0; i < allowedExtensions.Length; i++)
        //            {
        //                if (extension == allowedExtensions[i])
        //                {
        //                    fileOK = true;
        //                }
        //            }

        //            if (fileOK)
        //            {



        //                if (!noexiste(oOs, FileUpload2.FileName))
        //                {
        //                    FileUpload2.SaveAs(archivo);

        //                    Usuario oUser = new Usuario();
        //                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        //                    oOs.GuardarAnexo(FileUpload2.FileName, oUser);

        //                }
        //                else
        //                    estatus.Text = "ya existe un archivo con ese nombre para la peticion";

        //            }
        //            else
        //                estatus.Text = "no se acepta el tipo de archivo. Verifique que tengan extension .gif,png,.jpeg,jpg, pdf";

        //        }
        //    }
        //    catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }
        //}

        //protected void GuardarAnexo3(Peticion oOs)
        //{
        //    try
        //    {


        //        Boolean fileOK = false;
        //        if (FileUpload3.HasFile)
        //        {
        //            string directorio = Server.MapPath("") + "\\" + oOs.IdPeticion.ToString();


        //            if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

        //            string archivo = directorio + "\\" + FileUpload3.FileName;

        //            string extension = System.IO.Path.GetExtension(archivo).ToLower();
        //            String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".pdf" };
        //            for (int i = 0; i < allowedExtensions.Length; i++)
        //            {
        //                if (extension == allowedExtensions[i])
        //                {
        //                    fileOK = true;
        //                }
        //            }

        //            if (fileOK)
        //            {



        //                if (!noexiste(oOs, FileUpload3.FileName))
        //                {
        //                    FileUpload3.SaveAs(archivo);
        //                    Usuario oUser = new Usuario();
        //                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        //                    oOs.GuardarAnexo(FileUpload3.FileName, oUser);
        //                }
        //                else
        //                    estatus.Text = "ya existe un archivo con ese nombre para la peticion";

        //            }
        //            else
        //                estatus.Text = "no se acepta el tipo de archivo. Verifique que tengan extension .gif,png,.jpeg,jpg, pdf";

        //        }
        //    }
        //    catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }
        //}

        protected void rdbRN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbRN.SelectedValue == "1")
            {
                lblMensaje.Text = "Deberá completar adicionalmente datos de la madre";
                txtNumeroDocumento.Text = "";
                txtNumeroDocumento.Enabled = false;
                lnkBuscar.Enabled = false;
                rfvDni.Enabled = true;
                rfvDniMadre.Enabled = true;
                rfvApellidoMadre.Enabled = true;
                rfvNombreMadre.Enabled = true;
                txtApellido.Enabled = true;
                txtNombre.Enabled = true;
                ddlSexo.Enabled = true;
                txtApellidoP.Enabled = true;
                txtNombreP.Enabled = true;
                txtNumeroP.Enabled = true;
                rfvDni.Enabled = false;

                txtNumeroDocumento.Enabled = false;
                lblApellidoMadre.Visible = false;
                lblDniMadre.Visible = false;
                lblNombreMadre.Visible = false;
            }
            else
            {
                lnkBuscar.Enabled = true;
                lblMensaje.Text = "";
                rfvDni.Enabled = false;
                txtNumeroDocumento.Enabled = true;
                rfvDniMadre.Enabled = false;
                rfvApellidoMadre.Enabled = false;
                rfvNombreMadre.Enabled = false;

                rfvDni.Enabled = true;

              
                lblApellidoMadre.Visible = true;
                lblDniMadre.Visible = true;
                lblNombreMadre.Visible = true;
            }
            lnkBuscar.AutoUpdateAfterCallBack = false;
            txtNumeroDocumento.AutoUpdateAfterCallBack = true;
            lblMensaje.AutoUpdateAfterCallBack = true;
            rfvDniMadre.UpdateAfterCallBack = true;
            rfvDni.UpdateAfterCallBack = true;
            rfvApellidoMadre.UpdateAfterCallBack = true;
            rfvNombreMadre.UpdateAfterCallBack = true;
            txtApellido.AutoUpdateAfterCallBack = true;
            txtNombre.AutoUpdateAfterCallBack = true;
            ddlSexo.AutoUpdateAfterCallBack = true;
            txtApellidoP.AutoUpdateAfterCallBack = true;
            txtNombreP.AutoUpdateAfterCallBack = true;
            txtNumeroP.AutoUpdateAfterCallBack = true;



            rfvDni.AutoUpdateAfterCallBack = true;


            lblApellidoMadre.AutoUpdateAfterCallBack = true;
            lblDniMadre.AutoUpdateAfterCallBack = true;
            lblNombreMadre.AutoUpdateAfterCallBack = true;

        }

        protected void txtNumeroDocumento_TextChanged(EventArgs e)
        {
            MostrarDatosPaciente();
        }

        protected void lnkBuscar_Click(object sender, EventArgs e)
        {
            MostrarDatosPaciente();
        }

        protected void cvValidacionInput_ServerValidate(object source, ServerValidateEventArgs args)
        {
            ///Validacion de la fecha
            try {
                if (txtFecha.Value == "")
                {
                    args.IsValid = false;
                    this.cvValidacionInput.ErrorMessage = "Debe ingresar la fecha de la peticion";
                    return;
                }
                else
                {

                    if (DateTime.Parse(txtFecha.Value) > DateTime.Now)
                    {

                        args.IsValid = false;
                        this.cvValidacionInput.ErrorMessage = "La fecha de la peticion no puede ser superior a la fecha actual";
                        return;
                    }
                    else
                    {
                        if (DateTime.Parse(txtFechaNac.Text) > DateTime.Parse(txtFecha.Value))
                        {

                            args.IsValid = false;
                            this.cvValidacionInput.ErrorMessage = "La fecha de la peticion no puede ser menor a la fecha de nacimiento";
                            return;
                        }
                        else
                            args.IsValid = true;
                    }
                }
            }
            catch  
            {
                args.IsValid = false;
                this.cvValidacionInput.ErrorMessage = "Fecha inválida";
            }


        }

        //protected void btnAnulaAnexo1_Click(object sender, EventArgs e)
        //{
        //    PeticionAnexo oRegistro1 = new PeticionAnexo();
        //    oRegistro1 = (PeticionAnexo)oRegistro1.Get(typeof(PeticionAnexo), int.Parse(btnAnulaAnexo1.CommandArgument.ToString()));
        //    oRegistro1.Delete();
        //    Response.Redirect("PeticionLC.aspx?idPeticion=" + oRegistro1.IdPeticion.IdPeticion.ToString());
            

        //}
    }
}