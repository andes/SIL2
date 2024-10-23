using System;
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
using Business.Data.Laboratorio;
using System.IO;
using System.Drawing;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;

namespace WebLab.PeticionElectronica
{
    public partial class PeticionMensaje : System.Web.UI.Page
    {
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Business.Data.Laboratorio.Peticion oP = new Business.Data.Laboratorio.Peticion();
                oP= (Business.Data.Laboratorio.Peticion)oP.Get(typeof(Business.Data.Laboratorio.Peticion),int.Parse(Request["id"].ToString()));
                lblTitulo.Text = oP.IdPeticion.ToString();
              
                lblDescripcion.Text =oP.Apellido + "  " +oP.Nombre;

            }
            
        }

        



        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PeticionLC.aspx", false);
            
        }
    }
}
