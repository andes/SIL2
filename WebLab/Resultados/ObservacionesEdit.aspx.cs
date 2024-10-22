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
    public partial class ObservacionesEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            {
                txtObservacionAnalisis.Focus();
                string s_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
                DetalleProtocolo oDetalle = new DetalleProtocolo();
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(s_idDetalleProtocolo));
                lblObservacionAnalisis.Text = oDetalle.IdSubItem.Nombre;
                
                Utility oUtil = new Utility();
          
                string m_ssql = @" SELECT  idObservacionResultado , codigo  AS descripcion 
                                FROM   LAB_ObservacionResultado with (nolock) where idTipoServicio=" + Request["idTipoServicio"].ToString()+" and  baja=0 order by codigo " ;


                oUtil.CargarCombo(ddlObservacionesCodificadas, m_ssql, "idObservacionResultado", "descripcion");
                ddlObservacionesCodificadas.Items.Insert(0, new ListItem("", "0"));

                txtObservacionAnalisis.Text=oDetalle.Observaciones;

                if (txtObservacionAnalisis.Text != "") btnBorrarGuardarObservacionAnalisis.Enabled = true;
                else btnBorrarGuardarObservacionAnalisis.Enabled = false;

                if (oDetalle.IdUsuarioValidaObservacion > 0)
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValidaObservacion);
                    lblUsuarioObservacionAnalisis.Text += " Validado por: " + oUser.Apellido + " " + oUser.Nombre + " " + oDetalle.FechaValidaObservacion.ToString();
                    lblUsuarioObservacionAnalisis.Visible = true;
                    if (Request["Operacion"].ToString() != "Valida")
                    {
                        btnValidarObservacionAnalisis.Visible = false;
                        btnGuardarObservacionAnalisis.Visible = false;
                        btnBorrarGuardarObservacionAnalisis.Visible = false;
                    }

                }
                else
                {
                    if (oDetalle.IdUsuarioObservacion > 0)
                    {
                        Usuario oUser = new Usuario();
                        oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioObservacion);
                        lblUsuarioObservacionAnalisis.Text = " Cargado por: " + oUser.Apellido + " " + oUser.Nombre + " " + oDetalle.FechaObservacion.ToString();
                        lblUsuarioObservacionAnalisis.Visible = true;

                    }
                }
                string op = Request["Operacion"].ToString();
                if (op != "Valida") btnValidarObservacionAnalisis.Visible = false;
                
            //lblUsuarioObservacionAnalisis.Text= 
                
            }
        }

        protected void btnBorrarObservacionesAnalisis_Click(object sender, EventArgs e)
        {
           

            txtObservacionAnalisis.Text = "";
            txtObservacionAnalisis.UpdateAfterCallBack = true;

        }

        protected void btnAgregarObsCodificada_Click(object sender, EventArgs e)
        {
            if (ddlObservacionesCodificadas.Text != "")
            {
                ObservacionResultado oRegistro = new ObservacionResultado();
                 oRegistro = (ObservacionResultado)oRegistro.Get(typeof(ObservacionResultado), int.Parse( ddlObservacionesCodificadas.SelectedValue));

                 txtObservacionAnalisis.Text += oRegistro.Nombre;
                   
            }
            txtObservacionAnalisis.UpdateAfterCallBack = true;
        }
            protected void btnGuardarObservacionesAnalisis_Click(object sender, EventArgs e)
        {
            GuardarObservacionesDetalle();
          
               
        }
        protected void btnUsuarioObservacionesAnalisis_Click(object sender, EventArgs e)
        {
          VerUsuarioObservacionesDetalle();
               
        }

        private void VerUsuarioObservacionesDetalle()
        {

            lblUsuarioObservacionAnalisis.Text = "";
            string m_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));
            if (oDetalle != null)
            
            {
                if (oDetalle.IdUsuarioObservacion>0)
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario),oDetalle.IdUsuarioObservacion);
                    lblUsuarioObservacionAnalisis.Text = " Cargado por: " + oUser.Apellido + " " + oUser.Nombre + " " + oDetalle.FechaObservacion.ToString();
                }
                if (oDetalle.IdUsuarioValidaObservacion> 0)
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValidaObservacion);
                    lblUsuarioObservacionAnalisis.Text += " Validado por: " + oUser.Apellido +" " + oUser.Nombre + " " + oDetalle.FechaValidaObservacion.ToString();
                }
                //lblUsuarioObservacionAnalisis.UpdateAfterCallBack = true;
            } 
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
                oDetalle.Observaciones = txtObservacionAnalisis.Text;
                if (Request["Operacion"].ToString() == "Valida")
                    oDetalle.IdUsuarioObservacion = int.Parse(Session["idUsuarioValida"].ToString());
                else
                    oDetalle.IdUsuarioObservacion = int.Parse(Session["idUsuario"].ToString());
                oDetalle.FechaObservacion = DateTime.Now;
                //oDetalle.ConResultado = true;
                oDetalle.GrabarAuditoriaDetalleObservacionesProtocolo("Carga", oDetalle.IdUsuarioObservacion);
                
                oDetalle.Save();
               // pnlObservacionesDetalle.Visible = false;
            } //pnlObservacionesDetalle.UpdateAfterCallBack = true;

        }
       

        private void ValidarObservacionesDetalle()
        {
            if (Session["idUsuarioValida"] != null)
            {
                string op = Request["Operacion"].ToString();
                string m_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
                DetalleProtocolo oDetalle = new DetalleProtocolo();
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));
                if (oDetalle != null)
                {
                    oDetalle.Observaciones = txtObservacionAnalisis.Text;
                    oDetalle.IdUsuarioValidaObservacion = int.Parse(Session["idUsuarioValida"].ToString());
                    oDetalle.FechaValidaObservacion = DateTime.Now;

                    if (oDetalle.ConResultado == false)
                    {
                        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                        oDetalle.FechaValida  = DateTime.Now;
                    }

                    //   oDetalle.ConResultado = true;
                    oDetalle.Save();
                  
                    oDetalle.GrabarAuditoriaDetalleObservacionesProtocolo(op, oDetalle.IdUsuarioObservacion);


                    //Actualiza estado del protocolo

                    if (Request["Operacion"].ToString() != "Valida")
                    {
                        if (oDetalle.IdProtocolo.Estado == 0)
                            oDetalle.IdProtocolo.Estado = 1;                        //oProtocolo.Save();                    
                    }
                    else //Validacion
                    {
                        if (oDetalle.IdProtocolo.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(Session["idUsuarioValida"].ToString())))
                        {
                            oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);   

                            if (!oDetalle.IdProtocolo.Notificarresultado)
                                oDetalle.IdProtocolo.Estado = 3; //Acceso Restringido 
                        }
                        else
                            oDetalle.IdProtocolo.Estado = 1;
                    }
                    oDetalle.IdProtocolo.Save();
                    //Fin Actualiza estado del protocolo
                    //pnlObservacionesDetalle.Visible = false;
                }// pnlObservacionesDetalle.UpdateAfterCallBack = true;
            }
            else
                    Response.Redirect("../FinSesion.aspx", false);
        }

        protected void btnBorrarGuardarObservacionAnalisis_Click(object sender, EventArgs e)
        {
            string m_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));
            if (oDetalle != null)
            {
                oDetalle.Observaciones = "";
                if (oDetalle.IdUsuarioValidaObservacion > 0)
                {
                    oDetalle.IdUsuarioValidaObservacion = 0;
                    oDetalle.FechaValidaObservacion = DateTime.Now;
                }



                oDetalle.IdUsuarioObservacion = int.Parse(Session["idUsuario"].ToString()); oDetalle.FechaObservacion = DateTime.Now;
                    
                

                //   oDetalle.ConResultado = true;
                oDetalle.Save();
                
                oDetalle.GrabarAuditoriaDetalleObservacionesProtocolo("Borra", oDetalle.IdUsuarioObservacion);
                //pnlObservacionesDetalle.Visible = false;
            }

        }



          
        }


       
    
}