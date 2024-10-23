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

namespace WebLab.Protocolos
{
    public partial class ProtocoloMensaje : System.Web.UI.Page
    {
        


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                error.Visible = false;
                altaMuestra.Visible = false;
                if (Request["error"] != null)
                {
                    error.Visible = true;
                    altaMuestra.Visible = false;
                    lblError.Text = "Se ha producido un error al generar el protocolo. Verifique de no tener pestañas duplicadas del sistema o contactese con el Administrador";
                }
                else
                {
                    altaMuestra.Visible = true;
                    Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                    oP = (Business.Data.Laboratorio.Protocolo)oP.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["id"].ToString()));
                    lblTitulo.Text = oP.GetNumero();
                    Business.Data.Laboratorio.Muestra oM = new Business.Data.Laboratorio.Muestra();
                    oM = (Business.Data.Laboratorio.Muestra)oM.Get(typeof(Business.Data.Laboratorio.Muestra), oP.IdMuestra);
                    lblDescripcion.Text = oM.Nombre.ToUpper() + " " + oP.DescripcionProducto;
                }

            }
            
        }

        



        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
            oP = (Business.Data.Laboratorio.Protocolo)oP.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["id"].ToString()));
            if ((oP.IdTipoServicio.IdTipoServicio==3) || (oP.IdTipoServicio.IdTipoServicio == 5))
            Response.Redirect("ProtocoloProductoEdit.aspx?Operacion=Alta", false);
            if (oP.IdTipoServicio.IdTipoServicio == 6)
                Response.Redirect("Default2.aspx?idServicio=6&idUrgencia=0&idCaso=0", false);

        }
    }
}
