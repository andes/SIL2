using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Drawing;

namespace WebLab.PeticionElectronica
{
    public partial class VincularProtocolo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Peticiones");

                Peticion oRegistro = new Peticion();
                oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));
                lblPeticion.Text = oRegistro.IdPeticion.ToString() ;
                lblPaciente.Text =   oRegistro.Apellido + " " + oRegistro.Nombre;
                if (oRegistro.IdProtocolo == 0)
                    lblProtocolo.Text = "Sin Protocolo";
                else
                {
                    Protocolo oP = new Protocolo();
                    oP = (Protocolo)oP.Get(typeof(Protocolo), oRegistro.IdProtocolo);
                    lblProtocolo.Text = oP.GetNumero();
                }
                
                     
            }
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

                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


    
        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtNro.Text != "") { if (oUtil.EsEntero(txtNro.Text)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }
     



        protected void txtNro_TextChanged(object sender, EventArgs e)
        {
            Protocolo oP = new Protocolo();
                       oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", int.Parse(txtNro.Text), "Baja", false );
            if (oP != null)
            {
                Peticion oRegistro = new Peticion();
                oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), "IdProtocolo",oP.IdProtocolo);
                if (oRegistro!= null)
                { 
                    if (oRegistro.IdPeticion != int.Parse(Request["idPeticion"].ToString()))
                    {
                        status.Text = "Protocolo asignado a otra peticion nro. " + oRegistro.IdPeticion.ToString();
                        btnGuardar.Enabled = false;
                    }
                    else
                    { 
                        status.Text = "Numero de protocolo valido:" + oP.IdPaciente.Apellido + " " + oP.IdPaciente.Nombre; 

                    btnGuardar.Enabled = true;
                    }
                }
                else
                {
                    status.Text = "Numero de protocolo valido" + oP.IdPaciente.Apellido + " " + oP.IdPaciente.Nombre;

                    btnGuardar.Enabled = true;
                }
            }
            else
            {
                status.Text = "Numero de protocolo invalido";
                btnGuardar.Enabled = false;
            }
            status.Visible = true;
            status.UpdateAfterCallBack = true;
            btnGuardar.UpdateAfterCallBack = true;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { 
                Peticion oRegistro = new Peticion();
                oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));
                Protocolo oP = new Protocolo();
                oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", int.Parse(txtNro.Text), "Baja", false);

                oRegistro.IdProtocolo = oP.IdProtocolo;
                oRegistro.Save();
                Response.Redirect("PeticionList.aspx", true);
            }
        }
    }
}