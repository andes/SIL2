using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data;
using System.Data;
using System.Data.SqlClient;

namespace WebLab.Calidad
{
    public partial class IncidenciaRecepcionEdit : System.Web.UI.Page
    {
        
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        { 
            if (Session["idUsuario"] != null)
            {
                //oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Incidencias PreRecepcion");
                txtFecha.Value = DateTime.Now.ToShortDateString();
                lblTitulo.Text = "NUEVA INCIDENCIA";
                pnlIncidencia.Visible = false;
                CargarListas();
                //if (Request["tipoIncidencia"] == "1")
                //{
                //    TreeView1.Visible = true;
                //    //TreeView2.Visible = false;
                //}
                //else
                //{
                //    TreeView1.Visible = false;
                //   // TreeView2.Visible = true;
                //}
                if (Request["id"] != null)
                {
                   // if (Request["tipoIncidencia"] == "1")
                        MostrarDatos(Request["id"].ToString());
                   // else
                     //   MostrarDatosdelProtocolo(Request["id"].ToString());
                }
            }

        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

          

            ////////////Carga de combos de Efector
           string m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfectorOrigen, m_ssql, "idEfector", "nombre");
            ddlEfectorOrigen.SelectedValue = oCon.IdEfector.IdEfector.ToString();
        }
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

      
        //private void MostrarDatosdelProtocolo(string p)
        //{

        //    ////guardar fecha y hora del registro
        //    TreeView1.Visible = false;
        //    lblTitulo.Text = "INCIDENCIA";
        //    pnlIncidencia.Visible = true;
        //    Usuario oUser = new Usuario();
        //    Protocolo oRegistro = new Protocolo();
        //    oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(Request["id"].ToString()));
        //    lblNumero.Text = "Protocolo Nro." + oRegistro.GetNumero();
        //    //lblFecha.Text = oRegistro.Fecha.ToShortDateString() + " " + oRegistro.Fecha.ToShortTimeString();

        //    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
        //    lblUsuario.Text = oUser.Apellido + " " + oUser.Nombre;


        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloIncidenciaCalidad));
        //    crit.Add(Expression.Eq("IdProtocolo", oRegistro.IdProtocolo));



        //    IList items = crit.List();

        //    foreach (ProtocoloIncidenciaCalidad oDet in items)
        //    {
        //        for (int i = 0; i < TreeView2.Nodes.Count; i++)
        //        {

        //            if (TreeView2.Nodes[i].Value == oDet.IdIncidenciaCalidad.ToString())
        //            {
        //                TreeView2.Nodes[i].Checked = true;
        //            }
        //            for (int j = 0; j < TreeView2.Nodes[i].ChildNodes.Count; j++)
        //            {
        //                if (TreeView2.Nodes[i].ChildNodes[j].Value == oDet.IdIncidenciaCalidad.ToString())
        //                {
        //                    TreeView2.Nodes[i].ChildNodes[j].Checked = true;
        //                    TreeView2.Nodes[i].ExpandAll();
        //                }
        //            }

        //        }


        //    }


        //}

        private void MostrarDatos(string p)
        {
            //TreeView2.Visible = false;
            lblTitulo.Text = "MODIFICACION INCIDENCIA";
            pnlIncidencia.Visible = true;
            Usuario oUser = new Usuario();
            IndicenciaRecepcion oRegistro = new IndicenciaRecepcion();
            oRegistro = (IndicenciaRecepcion)oRegistro.Get(typeof(IndicenciaRecepcion), int.Parse(Request["id"].ToString()));
            lblNumero.Text= oRegistro.IdIndicenciaRecepcion.ToString();
            lblFecha.Text = oRegistro.FechaRegistro.ToShortDateString() + " " + oRegistro.FechaRegistro.ToShortTimeString();
            txtFecha.Value = oRegistro.Fecha.ToShortDateString();
            ddlEfectorOrigen.SelectedValue = oRegistro.IdEfectorOrigen.ToString();
            txtNumeroOrigen.Text = oRegistro.NumeroOrigen;
            txtObservaciones.Text = oRegistro.Observaciones;
            //oUser= (Usuario)oUser.Get(typeof(Usuario), oRegistro.IdUsuario);
            lblUsuario.Text = oUser.Apellido + " " + oUser.Nombre                ;

            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleIncidenciaRecepcion));
            crit.Add(Expression.Eq("IdIndicenciaRecepcion", oRegistro.IdIndicenciaRecepcion));
            

           
            IList items = crit.List();

            foreach (DetalleIncidenciaRecepcion oDet in items)
            {
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    
                    if (TreeView1.Nodes[i].Value == oDet.IdIncidenciaCalidad.ToString())
                    {
                        TreeView1.Nodes[i].Checked= true;
                    }
                    for (int j = 0; j < TreeView1.Nodes[i].ChildNodes.Count; j++)
                    {
                        if (TreeView1.Nodes[i].ChildNodes[j].Value == oDet.IdIncidenciaCalidad.ToString())
                        {
                            TreeView1.Nodes[i].ChildNodes[j].Checked = true;
                            TreeView1.Nodes[i].ExpandAll();
                        }
                    }
                    
                }

                
            }

        }

       

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //if (Request["tipoIncidencia"] == "1")
                //{
                    IndicenciaRecepcion oReg = new IndicenciaRecepcion();
                    if (Request["id"] != null)
                        oReg = (IndicenciaRecepcion)oReg.Get(typeof(IndicenciaRecepcion), int.Parse(Request["id"].ToString()));

                    Guardar(oReg);
                    Response.Redirect("IncidenciaRecepcionList.aspx", false);
               // }
                //else
                //{
                //    ProtocoloIncidenciaCalidad oReg = new ProtocoloIncidenciaCalidad();
                //    if (Request["id"] != null)
                //        oReg = (ProtocoloIncidenciaCalidad)oReg.Get(typeof(ProtocoloIncidenciaCalidad), int.Parse(Request["id"].ToString()));

                //    GuardarProtocoloIncidencia(oReg);
                //}

            }
         
             

            
        }

        private void Guardar(IndicenciaRecepcion oRegistro)
        {
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
         //   IndicenciaRecepcion oRegistro = new IndicenciaRecepcion();
            oRegistro.Fecha =DateTime.Parse( txtFecha.Value);
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.IdEfector = oCon.IdEfector.IdEfector;
            oRegistro.IdUsuario = int.Parse(Session["idUsuario"].ToString());
            oRegistro.IdEfectorOrigen =int.Parse( ddlEfectorOrigen.SelectedValue);
            oRegistro.NumeroOrigen = txtNumeroOrigen.Text;
            oRegistro.Observaciones = txtObservaciones.Text;
            oRegistro.Save();

                //// borra los detalles para el existente y los crea de nuevo
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleIncidenciaRecepcion));
                crit.Add(Expression.Eq("IdIndicenciaRecepcion", oRegistro.IdIndicenciaRecepcion));
                IList items = crit.List();

                foreach (DetalleIncidenciaRecepcion oDet in items)
                {
                    oDet.Delete();
                }



                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    if (TreeView1.Nodes[i].Checked)
                    {
                        DetalleIncidenciaRecepcion oDet = new DetalleIncidenciaRecepcion();
                        oDet.IdIndicenciaRecepcion = oRegistro.IdIndicenciaRecepcion;
                        oDet.IdIncidenciaCalidad = int.Parse(TreeView1.Nodes[i].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
                        oDet.Save();
                    }
                    for (int j = 0; j < TreeView1.Nodes[i].ChildNodes.Count; j++)
                    {
                        if (TreeView1.Nodes[i].ChildNodes[j].Checked)
                        {
                            DetalleIncidenciaRecepcion oDet = new DetalleIncidenciaRecepcion();
                            oDet.IdIndicenciaRecepcion = oRegistro.IdIndicenciaRecepcion;
                            oDet.IdIncidenciaCalidad = int.Parse(TreeView1.Nodes[i].ChildNodes[j].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
                            oDet.Save();
                        }                       
                    }
                }



            //    for (int i = 0; i < TreeView1.Nodes.Count; i++)
            //{
            //    //if (TreeView1.CheckedNodes[i].Selected)
            //    if (TreeView1.Nodes[i].Checked)
            //    {
            //        DetalleIncidenciaRecepcion oDet = new DetalleIncidenciaRecepcion();
            //        oDet.IdIndicenciaRecepcion = oRegistro.IdIndicenciaRecepcion;
            //        oDet.IdIncidenciaCalidad =  int.Parse(TreeView1.Nodes[i].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
            //        oDet.Save();

            //    }
            //}
            

            //for (int i = 0; i < chkIncidencia.Items.Count; i++)
            //{
            //    if (chkIncidencia.Items[i].Selected)
            //    {
            //        DetalleIncidenciaRecepcion oDet = new DetalleIncidenciaRecepcion();
            //        oDet.IdIndicenciaRecepcion = oRegistro.IdIndicenciaRecepcion;
            //        oDet.IdIncidenciaCalidad = int.Parse(chkIncidencia.Items[i].Value);
            //        oDet.Save();

            //    }
            //}

            //for (int i = 0; i < chkIncidencia0.Items.Count; i++)
            //{
            //    if (chkIncidencia0.Items[i].Selected)
            //    {
            //        DetalleIncidenciaRecepcion oDet = new DetalleIncidenciaRecepcion();
            //        oDet.IdIndicenciaRecepcion = oRegistro.IdIndicenciaRecepcion;
            //        oDet.IdIncidenciaCalidad = int.Parse(chkIncidencia0.Items[i].Value);
            //        oDet.Save();

            //    }
            //}

        }

        //private void GuardarProtocoloIncidencia(ProtocoloIncidenciaCalidad oRegistro)
        //{
            

        //    //// borra los detalles para el existente y los crea de nuevo
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloIncidenciaCalidad));
        //    crit.Add(Expression.Eq("IdProtocolo",int.Parse(Request["id"].ToString())));
        //    IList items = crit.List();

        //    foreach (ProtocoloIncidenciaCalidad oDet in items)
        //    {
        //        oDet.Delete();
        //    }



        //    for (int i = 0; i < TreeView2.CheckedNodes.Count; i++)
        //    {
        //         Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
         

        //        ProtocoloIncidenciaCalidad oDet = new ProtocoloIncidenciaCalidad();
        //        oDet.IdProtocolo = int.Parse(Request["id"].ToString());
        //        oDet.IdEfector = oC.IdEfector.IdEfector;
        //        oDet.IdIncidenciaCalidad = int.Parse(TreeView2.CheckedNodes[i].Value);
        //        oDet.Save();

        //    }


            
        //}

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;

            if (TreeView1.CheckedNodes.Count>0)                
                        args.IsValid = true; 

                


            

        }

        protected void lnkRegresar_Click1(object sender, EventArgs e)
        {
            Response.Redirect("IncidenciaRecepcionList.aspx", false);
        }
    }
}