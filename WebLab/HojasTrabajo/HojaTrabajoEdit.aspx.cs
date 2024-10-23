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
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using System.Data.SqlClient;
using NHibernate.Expression;
using Business.Data;
using CrystalDecisions.Shared;
using System.IO;

namespace WebLab.HojasTrabajo
{
    public partial class HojaTrabajoEdit : System.Web.UI.Page
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Hoja de Trabajo Edit");
                CargarListas();
                if (Request["id"] != null)
                    MostrarDatos();
            }

        }

        private void MostrarDatos()
        {
            HojaTrabajo oRegistro = new HojaTrabajo();
            oRegistro = (HojaTrabajo)oRegistro.Get(typeof(HojaTrabajo), int.Parse(Request["id"].ToString()));

            ddlServicio.SelectedValue = oRegistro.IdArea.IdTipoServicio.IdTipoServicio.ToString();
            CargarArea();
            ddlArea.SelectedValue = oRegistro.IdArea.IdArea.ToString();
            HabilitarItem();
            txtCodigoHT.Text = oRegistro.Codigo;
            txtResponsable.Text = oRegistro.Responsable;
            ddlServicio.Enabled = false;
            ddlArea.Enabled = false;
           

 
                if (!oRegistro.TipoHoja)
                {
                    rdbFormatoHoja.Items[0].Selected = true;
                    rdbFormatoHoja.Items[1].Selected = false;
                }
                else
                    {
                    rdbFormatoHoja.Items[0].Selected = false;
                    rdbFormatoHoja.Items[1].Selected = true;
                }


            
            ddlAnchoColumnas.SelectedValue = oRegistro.FormatoAncho.ToString();

            ///oPCIONES DE IMPRESION DEL PROTOCOLO

            chkDatosProtocolo.Items[1].Selected=oRegistro.ImprimirPrioridad  ;
            chkDatosProtocolo.Items[2].Selected=oRegistro.ImprimirOrigen  ;
            chkDatosProtocolo.Items[3].Selected = oRegistro.ImprimirCorrelativo;
            chkDatosProtocolo.Items[4].Selected = oRegistro.ImprimirMedico;
            chkDatosProtocolo.Items[5].Selected = oRegistro.ImprimirMuestra;
            //////////////////////////

            ////opciones de impresion de los datos del paciente
            chkDatosPaciente.Items[0].Selected =oRegistro.ImprimirApellidoNombre  ;
          chkDatosPaciente.Items[1].Selected  = oRegistro.ImprimirEdad ;
            chkDatosPaciente.Items[2].Selected=oRegistro.ImprimirSexo  ;

            if (!oRegistro.ImprimirAntecedente)
                ddlImprimirAntecedente.SelectedValue = "0";
            else
                ddlImprimirAntecedente.SelectedValue = "1";

            if (!oRegistro.AgrupaFecha)
                ddlAgrupaFecha.SelectedValue = "0";
            else
                ddlAgrupaFecha.SelectedValue = "1";

            if (!oRegistro.ImprimirFechaHora)
                ddlImprimirFechaHora.SelectedValue = "0";
            else
                ddlImprimirFechaHora.SelectedValue = "1";

            txtInferiorDerecha.Text = oRegistro.TextoInferiorDerecha;
            txtInferiorIzquierda.Text = oRegistro.TextoInferiorIzquierda;

            txtCantidadLineaAdicional.Text = oRegistro.CantidadLineaAdicional.ToString();

               // dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                DetalleHojaTrabajo oDetalle = new DetalleHojaTrabajo();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleHojaTrabajo));
                crit.Add(Expression.Eq("IdHojaTrabajo", oRegistro));
                string sDatos = "";
                IList items = crit.List();
             //   string pivot = "";
                foreach (DetalleHojaTrabajo oDet in items)
                {
                    if (sDatos=="")
                        sDatos =  oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#" + oDet.TextoImprimir + "@";  
                    else
                    sDatos +=  oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#" + oDet.TextoImprimir + "@";  

                }

                TxtDatos.Value = sDatos;
            
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
                    case 1: { btnGuardar.Visible = false;  } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            
            CargarArea();
           
            m_ssql = null;
            oUtil = null;
        }
      

        private void CargarItem()
        {

            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select I.idItem, I.nombre from Lab_Item as I 
                 inner join lab_itemEfector as IE on IE.idItem= I.idItem and IE.idEfector= " + oUser.IdEfector.IdEfector.ToString() +
                 @" where I.baja=0 AND IE.idEfector=Ie.idEfectorDerivacion and I.idArea= " + ddlAreaDeterminacion.SelectedValue+ " and I.idCategoria=0 order by I.nombre";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;

            m_ssql = null;
            oUtil = null;
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "")
            {
                Item oItem = new Item();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                Area oArea = new Area();
                ICriteria crit = m_session.CreateCriteria(typeof(Item));

                crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
                crit.Add(Expression.Eq("Baja", false));
                crit.Add(Expression.Eq("IdCategoria", 0));
                //Control Multiefector
              //  crit.Add(Expression.Sql(" IdItem in (Select top 1 IdItem From LAb_ItemEfector where disponible=1 and idEfector=" +"  order by IdProtocolo desc)"));

                oItem = (Item)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlItem.SelectedValue = oItem.IdItem.ToString();
                    txtNombreAnalisis.Text = oItem.Nombre.ToString();
                    txtNombre.Text = oItem.Nombre.ToString();
      
                }
                else
                {
                    lblMensaje.Text = "El codigo "  + txtCodigo.Text.ToUpper() + " no existe. ";
                    ddlItem.SelectedValue = "0";
                    txtCodigo.Text = "";
                    txtNombre.Text = "";
                    txtNombreAnalisis.Text = "";
 txtCodigo.UpdateAfterCallBack = true;
                 
                }
               
                ddlItem.UpdateAfterCallBack = true;
                txtNombre.UpdateAfterCallBack = true;
                txtNombreAnalisis.UpdateAfterCallBack = true;
                //   txtAncho.UpdateAfterCallBack = true;
                lblMensaje.UpdateAfterCallBack = true;
            }
            else
            {
                ddlItem.SelectedValue = "0";
                txtNombre.Text = "";
                txtNombreAnalisis.Text = "";

                ddlItem.UpdateAfterCallBack = true;
                txtNombre.UpdateAfterCallBack = true;
                txtNombreAnalisis.UpdateAfterCallBack = true;
              //  lblMensaje.UpdateAfterCallBack = true;
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
                txtCodigo.Text = oItem.Codigo;
                txtNombre.Text = ddlItem.SelectedItem.Text;
                txtNombreAnalisis.Text = oItem.Nombre.ToString();
                //   txtAncho.Text = txtNombre.Text.Length.ToString();
             
                // txtAncho.UpdateAfterCallBack = true;
            }
            else
            {
                txtCodigo.Text = "";
                txtNombre.Text = "";
                txtNombreAnalisis.Text = "";
             
            }
            txtNombre.UpdateAfterCallBack = true;
            txtCodigo.UpdateAfterCallBack = true;
            txtNombreAnalisis.UpdateAfterCallBack = true;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HojaTrabajo oRegistro = new HojaTrabajo();
                if (Request["id"] != null) oRegistro = (HojaTrabajo)oRegistro.Get(typeof(HojaTrabajo), int.Parse(Request["id"].ToString()));
                Guardar(oRegistro);
                Response.Redirect("HTList.aspx", false);
            }
        }

        private void Guardar(HojaTrabajo oRegistro)
        {

            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));


            Area oArea = new Area();
            oArea = (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue));


            oRegistro.IdArea = oArea;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Codigo = txtCodigoHT.Text;
            oRegistro.Responsable = txtResponsable.Text;

            if (rdbFormatoHoja.Items[0].Selected) oRegistro.TipoHoja = false;
            else oRegistro.TipoHoja = true;


            ////Opciones de Impresión
            //if (rdbHojaTrabajo.Items[0].Selected) oRegistro.Formato = 0;
            //else oRegistro.Formato = 1;
            oRegistro.Formato = 0;
            oRegistro.FormatoAncho = int.Parse(ddlAnchoColumnas.SelectedValue);


            ///oPCIONES DE IMPRESION DEL PROTOCOLO

            oRegistro.ImprimirPrioridad = chkDatosProtocolo.Items[1].Selected;
            oRegistro.ImprimirOrigen = chkDatosProtocolo.Items[2].Selected;
            oRegistro.ImprimirCorrelativo = chkDatosProtocolo.Items[3].Selected;
            oRegistro.ImprimirMedico = chkDatosProtocolo.Items[4].Selected;
            oRegistro.ImprimirMuestra = chkDatosProtocolo.Items[5].Selected;
            //////////////////////////

            ////opciones de impresion de los datos del paciente
            oRegistro.ImprimirApellidoNombre= chkDatosPaciente.Items[0].Selected;
            oRegistro.ImprimirEdad= chkDatosPaciente.Items[1].Selected;
            oRegistro.ImprimirSexo= chkDatosPaciente.Items[2].Selected;
            ///////////////////////
            if (ddlImprimirAntecedente.SelectedValue=="0")
            oRegistro.ImprimirAntecedente=false;
            else
                oRegistro.ImprimirAntecedente = true;


            if (ddlAgrupaFecha.SelectedValue == "0")
                oRegistro.AgrupaFecha = false;
            else
                oRegistro.AgrupaFecha = true;


            if ( ddlImprimirFechaHora.SelectedValue == "0")
               oRegistro.ImprimirFechaHora=false;
            else
                oRegistro.ImprimirFechaHora = true;

            oRegistro.TextoInferiorDerecha =txtInferiorDerecha.Text ;
            oRegistro.TextoInferiorIzquierda=txtInferiorIzquierda.Text  ;

            oRegistro.CantidadLineaAdicional = int.Parse(txtCantidadLineaAdicional.Text);


            //if (rdbDatosPaciente.Items[0].Selected)  ///No imprime
            //    oRegistro.DatosPaciente = false;
            //else oRegistro.DatosPaciente = true;

            ///////////////////////////////////
            oRegistro.IdUsuarioRegistro =oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();


            GuardarDetalle(oRegistro);

             
        }

        private void GuardarDetalle(HojaTrabajo oRegistro)
        {
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleHojaTrabajo));
            crit.Add(Expression.Eq("IdHojaTrabajo", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetalleHojaTrabajo oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }


            string[] tabla = TxtDatos.Value.Split('@');

            for (int i = 0; i < tabla.Length - 1; i++)
            {
                DetalleHojaTrabajo oDetalle = new DetalleHojaTrabajo();

                string[] fila = tabla[i].Split('#');
                if (fila[0].ToString() != "")
                {
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), "Codigo", fila[0].ToString(), "Baja", false);
                    if (oItem != null)
                    {
                        oDetalle.IdHojaTrabajo = oRegistro;
                        oDetalle.IdEfector = oRegistro.IdEfector;

                        oDetalle.IdItem = oItem;
                        oDetalle.TextoImprimir = fila[1].ToString();

                        oDetalle.Save();
                    }
                }

            }
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
            
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            ///se agrega la opcion de no agrupar por fecha y la opcion de imprimir o no la muestra solo 
            ///si el servicio es microbiologia de pacientes y no pacientes

            if ((ddlServicio.SelectedValue == "3") || (ddlServicio.SelectedValue == "5"))
            {
                chkDatosProtocolo.Items[5].Enabled = true;
                ddlAgrupaFecha.Enabled = true;
            }
            else
            { 
                chkDatosProtocolo.Items[5].Enabled = false;
                ddlAgrupaFecha.Enabled = false;
            }
            ///Carga de combos de areas             
            string m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            oUtil.CargarCombo(ddlAreaDeterminacion, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Seleccione un Area", "0"));
            ddlAreaDeterminacion.Items.Insert(0, new ListItem("Seleccione un Area", "0"));
            ddlArea.UpdateAfterCallBack = true;
            ddlAreaDeterminacion.UpdateAfterCallBack = true;
            chkDatosProtocolo.UpdateAfterCallBack = true;
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarItem();
        }

        private void HabilitarItem()
        {
            if (ddlAreaDeterminacion.SelectedValue != "0")
            {
                CargarItem();
                txtCodigo.Enabled = true;
                ddlItem.Enabled = true;
                txtNombre.Enabled = true;

                txtCodigo.UpdateAfterCallBack = true;
                ddlItem.UpdateAfterCallBack = true;
                txtNombre.UpdateAfterCallBack = true;
            }
        }

        protected void cvAnalisis_ServerValidate(object source, ServerValidateEventArgs args)
        {
         
            if (TxtDatos.Value=="") args.IsValid = false;
            else
            
            
             args.IsValid = true;
            
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("HTList.aspx", false);
        }

      
    }
}
