using Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.PeticionElectronica
{
    public partial class VerificaPaciente : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["idPeticion"]!= null)
                {
                    Business.Data.Laboratorio.Peticion oRegistro = new Business.Data.Laboratorio.Peticion();
                    oRegistro = (Business.Data.Laboratorio.Peticion)oRegistro.Get(typeof(Business.Data.Laboratorio.Peticion), int.Parse(Request["idPeticion"].ToString()));

                    Paciente opac = new Paciente();
                    opac = (Paciente)opac.Get(typeof(Paciente), oRegistro.IdPaciente.IdPaciente);
                    opac.IdObraSocial = oRegistro.IdObraSocial;
                    opac.Save();

                    if (opac.IdEstado == 2)  /// es temporal
                        Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oRegistro.IdPeticion.ToString(), false);
                    else // es identificado con dni                      
                    //// ser compara los datos ingresados en la peticion con los datos de sys_paciente 
                    //// si hay diferencias las muestra y queda en el usuario decidir si actualiza o no en Sys_paciente
                    ////si no hay diferencia sigue el protocolo
                    {
                        int i_apellido = String.Compare(oRegistro.Apellido, opac.Apellido);
                        int i_nombre = string.Compare(oRegistro.Nombre, opac.Nombre);

                        if ((i_apellido != 0) || (i_nombre != 0))
                        {
                            string datospaciente= opac.Apellido + " " + opac.Nombre + " - Fecha de Nac."+opac.FechaNacimiento.ToShortDateString();
                            string datospeticion = oRegistro.Apellido + " " + oRegistro.Nombre + " - Fecha de Nac." + oRegistro.FechaNacimiento.ToShortDateString();
                            ///Existe otro paciente con el mismo dni.
                            lblMensaje.Text = "ALERTA. El DNI ya existe con los siguientes datos.Verifique diferencias." + "\r\n" ;
                            lblMensaje.Text += "Datos en SIL: " + datospaciente + "\r\n";
                            lblMensaje.Text += "Datos en Peticion: " + datospeticion + "\r\n";

                            lblMensaje.Rows =3;
                           
                        }
                        else
                            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oRegistro.IdPeticion.ToString(), false);

                    }





                }
            }
           
        }

    

        protected void btnIgnorar_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.Peticion oRegistro = new Business.Data.Laboratorio.Peticion();
            oRegistro = (Business.Data.Laboratorio.Peticion)oRegistro.Get(typeof(Business.Data.Laboratorio.Peticion), int.Parse(Request["idPeticion"].ToString()));
            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oRegistro.IdPeticion.ToString(), false);

        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.Peticion oRegistro = new Business.Data.Laboratorio.Peticion();
            oRegistro = (Business.Data.Laboratorio.Peticion)oRegistro.Get(typeof(Business.Data.Laboratorio.Peticion), int.Parse(Request["idPeticion"].ToString()));

            Paciente opac = new Paciente();
            opac = (Paciente)opac.Get(typeof(Paciente), oRegistro.IdPaciente.IdPaciente);
            opac.Apellido = oRegistro.Apellido;
            opac.Nombre = oRegistro.Nombre;
            opac.FechaNacimiento = oRegistro.FechaNacimiento;
            opac.IdObraSocial = oRegistro.IdObraSocial;
            opac.Save();

            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oRegistro.IdPeticion.ToString(), false);

        }
    }
}