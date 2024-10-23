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

namespace WebLab.Protocolos
{
    public partial class ProtocoloEliminar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            { string s_iditem = Request["id"].ToString();
                Protocolo oProt = new Protocolo();
                oProt = (Protocolo)oProt.Get(typeof(Protocolo), int.Parse(s_iditem));

                lblTitulo.Text = "Nro." + oProt.Numero.ToString();
                if (oProt.Baja)
                {
                    estatus.Text = "El caso ha sido anulado. Ver auditoria";
                    estatus.Visible = true;

                    btnGuardar.Visible = false;
                }

            }

        }

        private void Eliminar()
        {
            string s_idCaso = Request["id"].ToString();

            Protocolo oProt = new Protocolo();
            if (s_idCaso != "")
            {

               
                    oProt = (Protocolo)oProt.Get(typeof(Protocolo), int.Parse(s_idCaso));
                    oProt.Baja = true;
                    oProt.Save();
                //oProt.GrabarAuditoriaProtocoloObs("Anula", int.Parse(Session["idUsuario"].ToString()),txtMotivoBaja.Text);
                string accion = txtMotivoBaja.Text;
                if (accion.Length > 50)
                    accion = accion.Substring(0, 50);
                oProt.GrabarAuditoriaProtocoloObs("Anula", int.Parse(Session["idUsuario"].ToString()), accion);


            }

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