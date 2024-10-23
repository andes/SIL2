using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;
using Business.Data;

namespace WebLab.Services
{
    public partial class ObrasSociales : System.Web.UI.UserControl
    {
        public bool Requerido { get; set; }
        //private static bool tieneOS { get; set; }
        public string ValidationGroup { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //tieneOS = false;
            boolRequerido.Value = Requerido.ToString().ToLower();
            cvValidar.ValidationGroup = ValidationGroup;
            cvValidar.Enabled = (Requerido != true ? false : true);
        }

        public int getObraSocial()
        {
            int idO;
            if (Int32.TryParse(codigoOS.Value, out idO))
            {
                //tieneOS = true;
                ActivarRequerido(false);
                return idO;
            }
            else
            {
                if (Requerido) ActivarRequerido(true);
                //tieneOS = false;
                return -1;
            }
        }

        private void ActivarRequerido(bool Activa)
        {
            lblMensajeError.Style.Add("visibility", (Activa == true ? "visible" : "collapse"));
        }

        public void setOS(int idOso)
        {
                       Business.Data.ObraSocial oOs = new Business.Data.ObraSocial();
            oOs=(Business.Data.ObraSocial)oOs.Get(typeof(Business.Data.ObraSocial), idOso); 
            
            if (oOs!=null)
            {
                idOS.Value = oOs.ToString();
                lblNombre.Text = oOs.Nombre;
                lblSigla.Text = oOs.Sigla;
                //lblCodigoNacion.Text = oOs.Nombre;
                codigoOS.Value = oOs.IdObraSocial.ToString();

                if (idOso < 0 && Requerido) ActivarRequerido(true);
            }
            else
            {
                //tieneOS = false;
                lblNombre.Text = "La Obra Social seteada es incorrecta";
            }
        }
    }
}