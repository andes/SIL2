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

namespace WebLab.Derivaciones
{
    public partial class LoteEliminar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                string s_iditem = Request["id"].ToString();
                LoteDerivacion oLote = new LoteDerivacion();
                oLote = (LoteDerivacion)oLote.Get(typeof(LoteDerivacion), int.Parse(s_iditem));

                lblTitulo.Text = "Nro. Lote " + oLote.IdLoteDerivacion.ToString();
                if (oLote.Baja)
                {
                    if (Request["accion"] != null && Request["accion"].ToString() == "recupera")
                    {
                        lblMotivo.Text = "Motivo de recuperación";
                        btnGuardar.Text = "Recuperar";
                    }
                    else
                    {
                        estatus.Text = "El caso ha sido anulado. Ver auditoria";
                        estatus.Visible = true;
                        btnGuardar.Text = "Recuperar";
                        btnGuardar.Visible = false;
                    }
                  
                }
            }

        }

        private void Eliminar()
        {
            string s_idCaso = Request["id"].ToString();

            LoteDerivacion oLote = new LoteDerivacion();
            if (s_idCaso != "")
            {
                oLote = (LoteDerivacion)oLote.Get(typeof(LoteDerivacion), int.Parse(s_idCaso));
                oLote.Baja = true;
                oLote.Save();
                string accion = txtMotivoBaja.Text;

                if (accion.Length > 50)
                    accion = accion.Substring(0, 50);

                oLote.GrabarAuditoriaLoteDerivacion("Anula", int.Parse(Session["idUsuario"].ToString()),"Motivo baja" ,accion);

                //FALTA --> Liberar derivaciones
            }

        } 
        
        private void Recuperar()
        {
            string s_idCaso = Request["id"].ToString();

            LoteDerivacion oLote = new LoteDerivacion();
            if (s_idCaso != "")
            {
                oLote = (LoteDerivacion)oLote.Get(typeof(LoteDerivacion), int.Parse(s_idCaso));
                oLote.Baja = false;
                oLote.Save();
                string accion = txtMotivoBaja.Text;
                if (accion.Length > 50)
                    accion = accion.Substring(0, 50);

                oLote.GrabarAuditoriaLoteDerivacion("Recupera", int.Parse(Session["idUsuario"].ToString()),"Motivo recupera", accion);
            }

        }
        
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Request["accion"] != null && Request["accion"].ToString() == "recupera")
                {
                    Recuperar();
                    estatus.Text = "El caso ha sido recuperado";
                }
                else
                {
                    Eliminar();
                    estatus.Text = "El caso ha sido anulado";
                }
            }
        }
    }
}