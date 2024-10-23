using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data;
using System.Drawing;

namespace WebLab.Resultados
{
    public partial class ProtocoloView : System.Web.UI.Page
    {
      
        Protocolo oProtocolo = new Protocolo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                MuestraDatos(Request["idProtocolo"].ToString());

                
                
            }
        }

        private void MuestraDatos(string p)
        {
            Protocolo oRegistro = new Protocolo();
            oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(p));


            
            if (oRegistro.IdTipoServicio.IdTipoServicio == 3) //Microbiologia
            {
            
                if (oRegistro.IdMuestra > 0)
                {
                    Muestra oMuestra = new Muestra();
                    oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), oRegistro.IdMuestra);

            //        lblMuestra.Text = "Tipo de Muestra: " + oMuestra.Nombre;
                }
              
            }

         


            switch (oRegistro.Estado)
            {
                case 0:
                    {
                        
                        imgEstado.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                        
                    }
                    break;
                case 1: imgEstado.ImageUrl = "~/App_Themes/default/images/amarillo.gif"; break;
                case 2:
                    {
                        imgEstado.ImageUrl = "~/App_Themes/default/images/verde.gif";

                        
                    } break;

            }

            lblUsuario.Text = oRegistro.IdUsuarioRegistro.Username;
            lblFechaRegistro.Text = oRegistro.FechaRegistro.ToShortDateString();
            int len = oRegistro.FechaRegistro.ToString().Length - 11;
            lblHoraRegistro.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);
            lblFecha.Text = oRegistro.Fecha.ToShortDateString();
            lblProtocolo.Text = oRegistro.GetNumero().ToString();

            

            if (oRegistro.IdEfector == oRegistro.IdEfectorSolicitante)
                lblOrigen.Text = oRegistro.IdOrigen.Nombre;
            else
                lblOrigen.Text = oRegistro.IdEfectorSolicitante.Nombre;


            lblMedico.Text = "";
            if ((oRegistro.IdEspecialistaSolicitante > 0) && (oRegistro.IdEfectorSolicitante == oRegistro.IdEfector))
            {
                try
                {
                    Profesional oMedico = new Profesional();
                    oMedico = (Profesional)oMedico.Get(typeof(Profesional), oRegistro.IdEspecialistaSolicitante);
                    if (oMedico != null)
                        lblMedico.Text = oMedico.Apellido + " " + oMedico.Nombre;
                }
                catch (Exception ex)
                {
                    string exception = "";
                    while (ex != null)
                    {
                        exception = ex.Message + "<br>";

                    }
                }
            }
            else
                lblMedico.Text = "";

            lblPrioridad.Text = oRegistro.IdPrioridad.Nombre;
            if (oRegistro.IdPrioridad.Nombre == "URGENTE")
            {
                lblPrioridad.ForeColor = Color.Red;
                lblPrioridad.Font.Bold = true;
            }

            lblSector.Text = oRegistro.IdSector.Nombre;
            if (oRegistro.Sala != "") lblSector.Text += " Sala: " + oRegistro.Sala;
            if (oRegistro.Cama != "") lblSector.Text += " Cama: " + oRegistro.Cama;


            ///Datos del Paciente            
            if (oRegistro.IdPaciente.IdEstado == 2)  lblDni.Text = "(Sin DU Temporal)";
            else lblDni.Text = oRegistro.IdPaciente.NumeroDocumento.ToString();
            
            lblPaciente.Text = oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();

            lblSexo.Text = oRegistro.IdPaciente.getSexo();
            lblFechaNacimiento.Text = oRegistro.IdPaciente.FechaNacimiento.ToShortDateString();
            lblEdad.Text = oRegistro.Edad.ToString();
            switch (oRegistro.UnidadEdad)
            {
                case 0: lblEdad.Text += " años"; break;
                case 1: lblEdad.Text += " meses"; break;
                case 2: lblEdad.Text += " días"; break;
            }


            //if (oRegistro.HoraNacimiento!="") lblDatosScreening.Text = "Hora Nac.:" + oRegistro.HoraNacimiento + " -  Peso Nac.:" + oRegistro.PesoNacimiento.ToString() + "(gr.) - Sem. Gest:" + oRegistro.SemanaGestacion.ToString();
            //else lblDatosScreening.Text = "";


            lblNumeroOrigen.Text = oRegistro.NumeroOrigen;

            /////Observaciones en el ingreso de protocolo
           
                    pnlObservaciones.Visible = true;
                    lblObservacion.Text = oRegistro.Observacion;
           

            ////////////////////////////////////////
            string embarazada = "";
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            IList lista = crit.List();
            if (lista.Count > 0)
            {
                foreach (ProtocoloDiagnostico oDiag in lista)
                {
                    Cie10 oD = new Cie10();
                    oD = (Cie10)oD.Get(typeof(Cie10), oDiag.IdDiagnostico);
                    if (lblDiagnostico.Text == "") lblDiagnostico.Text = oD.Nombre;
                    else lblDiagnostico.Text += " - " + oD.Nombre;

                    if (oD.Codigo == "Z32.1") embarazada = "E";
                }
            }

            //oRegistro.IdPaciente.getCodificaHiv(); //
            lblCodigoPaciente.Text = oRegistro.getCodificaHiv(embarazada); //lblSexo.Text.Substring(0, 1) + " " + oRegistro.IdPaciente.Nombre.Substring(0, 2) + oRegistro.IdPaciente.Apellido.Substring(0, 2) + " " + lblFechaNacimiento.Text.Replace("/", "") + embarazada;
            //lblCodigoPaciente.Text = lblCodigoPaciente.Text.ToUpper();

            ///Observaciones de Resultados al pie 

            if (oRegistro.IdTipoServicio.IdTipoServicio == 4) PesquisaNeonatal1.Visible = true;
            else PesquisaNeonatal1.Visible = false;


            lblMuestra.Text = "";
            if (oRegistro.IdTipoServicio.IdTipoServicio == 3)
            {
                Muestra oMuestra = new Muestra();
                oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), oRegistro.IdMuestra);
                if (oMuestra != null) lblMuestra.Text = oMuestra.Nombre;
            }
            //SolicitudScreening oSolicitud = new SolicitudScreening();
            //oSolicitud = (Business.Data.Laboratorio.SolicitudScreening)oSolicitud.Get(typeof(Business.Data.Laboratorio.SolicitudScreening), "IdProtocolo", oRegistro);
            //if (oSolicitud != null) PesquisaNeonatal1.Visible = false;

        }
        

    }
}

