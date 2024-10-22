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

namespace WebLab.Resultados
{
    public partial class PredefinidoSelect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            {
                txtObservacionAnalisis.Focus();
                string s_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
                DetalleProtocolo oDetalle = new DetalleProtocolo();
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(s_idDetalleProtocolo));

                if (oDetalle.IdSubItem.IdItem != oDetalle.IdItem.IdItem)
                {
                    lblObservacionAnalisis.Text = oDetalle.IdItem.Nombre;
                    lblObservacionAnalisis.Text += " " + oDetalle.IdSubItem.Nombre;
                }
                else
                    lblObservacionAnalisis.Text = oDetalle.IdSubItem.Nombre;

                lblProtocolo.Text = oDetalle.IdProtocolo.GetNumero();
                
                
                //Utility oUtil = new Utility();

                //string m_ssql = @" select resultado from LAB_ResultadoItem where idItem=" + s_iditem + " and baja=0 order by idResultadoItem ";
            
                //oUtil.CargarCombo(ddlObservacionesCodificadas, m_ssql, "resultado", "resultado");
                //ddlObservacionesCodificadas.Items.Insert(0, new ListItem("", "0"));

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                crit.Add(Expression.Eq("IdItem", oDetalle.IdSubItem));                
                IList resultados = crit.List();
                
                foreach (ResultadoItem oResultado in resultados)
                {
                    ListItem Item = new ListItem();
                    Item.Value = oResultado.IdResultadoItem.ToString();
                    Item.Text = oResultado.Resultado;
                    chk1.Items.Add(Item);
                }
              //      chk1.Height = Unit.Pixel(200);
                    //chk1.Attributes.Add("ScrollBars", "Horizontal");
                    //chk1.AutoUpdateAfterCallBack = true;
                                               
                
                


                txtObservacionAnalisis.Text = oDetalle.ResultadoCar;


                if (oDetalle.IdUsuarioValidaObservacion > 0)
                {
                    if (Request["Operacion"].ToString() != "Valida")
                    {
                    //    btnValidarObservacionAnalisis.Visible = false;
                        btnGuardarObservacionAnalisis.Visible = false;

                    }

                }
                
               // string op = Request["Operacion"].ToString();
                //if (op != "Valida") btnValidarObservacionAnalisis.Visible = false;
                
            //lblUsuarioObservacionAnalisis.Text= 
                
            }
        }

      

        protected void btnAgregarObsCodificada_Click(object sender, EventArgs e)
        {
            //if (ddlObservacionesCodificadas.Text != "")
            //{
            //    txtObservacionAnalisis.Text +=ddlObservacionesCodificadas.SelectedValue;               
            //}
            //txtObservacionAnalisis.Focus();
            //txtObservacionAnalisis.UpdateAfterCallBack = true;
        }
            
        protected void btnGuardarObservacionesAnalisis_Click(object sender, EventArgs e)
        {
            GuardarObservacionesDetalle();
          
               
        }
       

        protected void btnValidarObservacionesAnalisis_Click(object sender, EventArgs e)
        {
            ValidarObservacionesDetalle();
        }

         private void GuardarObservacionesDetalle()
        {
            string m_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));
            if (oDetalle != null)
            {
                oDetalle.ResultadoCar = txtObservacionAnalisis.Text;
                
                oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                oDetalle.FechaResultado = DateTime.Now;
                oDetalle.ConResultado = true;
                oDetalle.GrabarAuditoriaDetalleProtocolo("Carga", oDetalle.IdUsuarioResultado);
                
                oDetalle.Save();
                if (oDetalle.IdProtocolo.Estado == 0)
                {
                    oDetalle.IdProtocolo.Estado = 1;
                    oDetalle.IdProtocolo.Save();
                }
               // pnlObservacionesDetalle.Visible = false;
            } //pnlObservacionesDetalle.UpdateAfterCallBack = true;

        }
       

        private void ValidarObservacionesDetalle()
        {
            //string op = Request["Operacion"].ToString();
            string m_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));
            if (oDetalle != null)
            {
                oDetalle.ResultadoCar = txtObservacionAnalisis.Text;
                oDetalle.IdUsuarioValida = int.Parse(Session["idUsuario"].ToString());
                oDetalle.FechaValida= DateTime.Now;
                oDetalle.ConResultado = true;
                
             
                oDetalle.Save();
                if (oDetalle.IdProtocolo.Estado == 0)
                {
                    oDetalle.IdProtocolo.Estado = 1;
                    oDetalle.IdProtocolo.Save();
                }
                oDetalle.GrabarAuditoriaDetalleProtocolo("Valida", oDetalle.IdUsuarioValida);
                //pnlObservacionesDetalle.Visible = false;
            }// pnlObservacionesDetalle.UpdateAfterCallBack = true;

        }

        protected void chk1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string val = "";
            for (int i = 0; i < chk1.Items.Count; i++)
            {
                if (chk1.Items[i].Selected)
                {
                    if (val == "") val = chk1.Items[i].Text;
                    else val += ", " + chk1.Items[i].Text;
                }
            }
            txtObservacionAnalisis.Text = val;
                txtObservacionAnalisis.UpdateAfterCallBack = true;
            
        }

        


          
        }


       
    
}