using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Data.SqlClient;
using System.Data;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Drawing;

namespace WebLab.CasoFiliacion
{
    public partial class CasoEliminar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            {
                string s_iditem = Request["idCaso"].ToString();

                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(s_iditem));
                lblTitulo.Text= "Caso nro."+oRegistro.IdCasoFiliacion.ToString();

            }

        }

        private void Eliminar()
        {
            string s_idCaso = Request["idCaso"].ToString();


          
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(s_idCaso));

       if (oRegistro.IdUsuarioValida==0)
            { 
                oRegistro.Baja = true;
                oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.MotivoBaja = txtMotivoBaja.Text;
                oRegistro.Save();

                oRegistro.GrabarAuditoria("Elimina Caso", int.Parse(Session["idUsuario"].ToString()), txtMotivoBaja.Text);
            }
       else
                estatus.Text = "El caso ha sido validado y no es posible eliminar";

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Eliminar();
                estatus.Text = "El caso ha sido anulado";
            }
        }
    }
}