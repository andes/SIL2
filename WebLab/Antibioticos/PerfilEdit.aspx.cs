using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using NHibernate;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Collections;
using Business.Data;

namespace WebLab.Antibioticos
{
    public partial class PerfilEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {

                    VerificaPermisos("Perfil Antibioticos");
                    CargarAntibioticos();
                    if (Request["id"] != null)
                        MostrarDatos();
                }
                else Response.Redirect("../FinSesion.aspx", false);
                
            }
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
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        private void MostrarDatos()
        {
            PerfilAntibiotico oRegistro = new PerfilAntibiotico();
         oRegistro = (PerfilAntibiotico)oRegistro.Get(typeof(PerfilAntibiotico), int.Parse(Request["id"]));
         txtNombre.Text = oRegistro.Nombre;

         ISession m_session = NHibernateHttpModule.CurrentSession;
         ICriteria crit = m_session.CreateCriteria(typeof(DetallePerfilAntibiotico));
         crit.Add(Expression.Eq("IdPerfilAntibiotico", oRegistro.IdPerfilAntibiotico));
         IList detalle = crit.List();
         if (detalle.Count > 0)
         {
             foreach (DetallePerfilAntibiotico oDetalle in detalle)
             {
                 ListItem ItemSeleccion = new ListItem();
                 ItemSeleccion.Value = oDetalle.IdAntibiotico.ToString();

                 Antibiotico oAnti= new Antibiotico();
                 oAnti = (Antibiotico)oAnti.Get(typeof(Antibiotico), oDetalle.IdAntibiotico);
                 ItemSeleccion.Text = oAnti.Descripcion + " - " + oAnti.NombreCorto;

                 lstAntibioticoFinal.Items.Add(ItemSeleccion);

             }

         }
        }

        private void CargarAntibioticos()
        {
            Utility oUtil = new Utility();

            
            //  btnGuardarImprimir.Visible = oC.GeneraComprobanteProtocolo;
            lstAntibiotico.Items.Clear();

            ///Carga de combos de tipos de servicios
            string m_ssql = "SELECT     idAntibiotico , descripcion + ' - ' + nombreCorto AS nombre FROM  LAB_Antibiotico ORDER BY descripcion";
            oUtil.CargarListBox(lstAntibiotico, m_ssql, "idAntibiotico", "nombre");            
        }

        protected void btnAgregarDiagnostico_Click(object sender, EventArgs e)
        {


            AgregarAntibiotico();
        }

        private void AgregarAntibiotico()
        {
            bool agregar=true;
            if (lstAntibioticoFinal.Items.Count > 0)
            {
                /////Crea nuevamente los detalles.
                for (int i = 0; i < lstAntibioticoFinal.Items.Count; i++)
                {
                    if (lstAntibiotico.SelectedItem.Value == lstAntibioticoFinal.Items[i].Value)
                    { agregar = false; break; }
                }
            }

            if (agregar)
            {
                if (lstAntibiotico.SelectedValue != "")
                {
                    lstAntibioticoFinal.Items.Add(lstAntibiotico.SelectedItem);

                }
                lstAntibioticoFinal.UpdateAfterCallBack = true;
            }
        }

        protected void btnSacarDiagnostico_Click(object sender, ImageClickEventArgs e)
        {
            SacarAntibiotico();
        }

        private void SacarAntibiotico()
        {
            if (lstAntibioticoFinal.SelectedValue != "")
            {
                lstAntibioticoFinal.Items.Remove(lstAntibioticoFinal.SelectedItem);
                lstAntibioticoFinal.UpdateAfterCallBack = true;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                PerfilAntibiotico oRegistro = new PerfilAntibiotico();
                if (Request["id"] != null) oRegistro = (PerfilAntibiotico)oRegistro.Get(typeof(PerfilAntibiotico), int.Parse(Request["id"]));
                Guardar(oRegistro);
                Response.Redirect("PerfilList.aspx", false);
            }
        }

        private void Guardar(PerfilAntibiotico oRegistro)
        {

            oRegistro.Nombre = txtNombre.Text;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            oRegistro.Save();
         
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetallePerfilAntibiotico));
            crit.Add(Expression.Eq("IdPerfilAntibiotico", oRegistro.IdPerfilAntibiotico));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetallePerfilAntibiotico oDetalle in detalle)
                {                   
                    oDetalle.Delete();                   
                }
            }

            ///Busca en la lista de diagnosticos buscados
            if (lstAntibioticoFinal.Items.Count > 0)
            {
                /////Crea nuevamente los detalles.
                for (int i = 0; i < lstAntibioticoFinal.Items.Count; i++)
                {
                    DetallePerfilAntibiotico oDetalle = new DetallePerfilAntibiotico();
                    oDetalle.IdPerfilAntibiotico = oRegistro.IdPerfilAntibiotico;
                    oDetalle.IdAntibiotico = int.Parse(lstAntibioticoFinal.Items[i].Value);                 
                    oDetalle.Save();
                }
            }


        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PerfilList.aspx");
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (lstAntibioticoFinal.Items.Count == 0)
                args.IsValid = false;
            else
                args.IsValid = true;

        }

    }
}