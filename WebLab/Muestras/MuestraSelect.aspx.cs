using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data.Laboratorio;
using NHibernate;
using Business;
using NHibernate.Expression;
using System.Collections;

namespace WebLab.Muestras
{
    public partial class MuestraSelect : System.Web.UI.Page
    {
        public string lista_muestras = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                CargarListaTipoMuestra();

        }

        private void CargarListaTipoMuestra()
        {
            Utility oUtil = new Utility();
            
            string m_ssql = "SELECT  idMuestra, nombre from lab_muestra  WHERE (baja = 0) order by nombre";
            oUtil.CargarListBox(lstDiagnosticos, m_ssql, "idMuestra", "nombre");
            

        }

        protected void btnAgregarDiagnostico_Click(object sender, EventArgs e)
        {


            AgregarDiagnostico();
        }
        protected void btnAgregarDiagnostico_Click1(object sender, ImageClickEventArgs e)
        {
            AgregarDiagnostico();
        }

        protected void btnSacarDiagnostico_Click(object sender, ImageClickEventArgs e)
        {
            SacarDiagnostico();
        }

        private void AgregarDiagnostico()
        {

            if (lstDiagnosticos.SelectedValue != "")
            {
                lstDiagnosticosFinal.Items.Add(lstDiagnosticos.SelectedItem);
                lstDiagnosticosFinal.UpdateAfterCallBack = true;
                if (lista_muestras == "")
                    lista_muestras = lstDiagnosticosFinal.SelectedValue;
                else
                    lista_muestras ="&"+ lstDiagnosticosFinal.SelectedValue;

                Session["muestras"] = lista_muestras;
            }
        }


        private void SacarDiagnostico()
        {
            if (lstDiagnosticosFinal.SelectedValue != "")
            {
                lstDiagnosticosFinal.Items.Remove(lstDiagnosticosFinal.SelectedItem);
                lstDiagnosticosFinal.UpdateAfterCallBack = true;
                if (lista_muestras == "")
                    lista_muestras = lstDiagnosticosFinal.SelectedValue;
                else
                    lista_muestras = "&" + lstDiagnosticosFinal.SelectedValue;
                Session["muestras"] = lista_muestras;
            }
        }



        protected void btnBusquedaDiagnostico_Click(object sender, EventArgs e)
        {

            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {
                Muestra       oDiagnostico = new Muestra();
                oDiagnostico = (Muestra)oDiagnostico.Get(typeof(Muestra), "Codigo", txtCodigoDiagnostico.Text);
                if (oDiagnostico != null)
                {
                    ListItem oDia = new ListItem();
                    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    oDia.Value = oDiagnostico.IdMuestra.ToString();
                    lstDiagnosticos.Items.Add(oDia);

                }
                else
                    lstDiagnosticos.Items.Clear();
            }

            if (txtNombreDiagnostico.Text != "")
            {
                lstDiagnosticos.Items.Clear();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                //Muestra oDiagnostico = new Muestra();
                ICriteria crit = m_session.CreateCriteria(typeof(Muestra));
                crit.Add (Expression.Like("Nombre",txtNombreDiagnostico.Text));
                //crit.Add(Expression.Sql("Nombre like '%" + txtNombreDiagnostico.Text + "%' order by Nombre"));

                IList items = crit.List();

                foreach (Muestra oDiagnostico in items)
                {
                    ListItem oDia = new ListItem();
                    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    oDia.Value = oDiagnostico.IdMuestra.ToString();
                    lstDiagnosticos.Items.Add(oDia);
                }


            }
            lstDiagnosticos.UpdateAfterCallBack = true;




        }
    }
}