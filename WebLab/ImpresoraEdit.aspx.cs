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
using NHibernate;
using Business;
using NHibernate.Expression;
using Business.Data;

namespace WebLab
{
    public partial class ImpresoraEdit : System.Web.UI.Page
    {
       
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            if (Session["idUsuario"] != null)

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            else Response.Redirect("FinSesion.aspx", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Impresoras de Etiquetas");
                foreach (string MiImpresora in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    ddlImpresora.Items.Add(MiImpresora);                
                }
                MostrarImpresoras();
            }
        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("FinSesion.aspx", false);
            }
            else
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("AccesoDenegado.aspx", false); break;
                        case 1: btnGuardar.Visible = false; break;
                    }
                }
                else Response.Redirect("FinSesion.aspx", false);
            }
        }

        private void MostrarImpresoras()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;

            Impresora oRegistro = new Impresora();
            ICriteria crit2 = m_session.CreateCriteria(typeof(Impresora));
            crit2.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));
            IList listaImpresoras = crit2.List();

            foreach (Impresora oImpr in listaImpresoras)
            {
                ListItem oItem = new ListItem();
                oItem.Text = oImpr.Nombre;
                oItem.Value = oImpr.Nombre;
                lstImpresora.Items.Add(oItem);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //if (!VerificarExisteImpresoras())
            //{
            //    if (ddlImpresora.SelectedValue != "0")
            //    {
            //        ListItem oImpresora = new ListItem();
            //        oImpresora.Text = ddlImpresora.SelectedValue;
            //        oImpresora.Value = ddlImpresora.SelectedValue;
            //        lstImpresora.Items.Add(oImpresora);
            //    }

            //    if (txtNuevaImpresora.Text != "")
            //    {
            //        ListItem oImpresora2 = new ListItem();
            //        oImpresora2.Text = txtNuevaImpresora.Text;
            //        oImpresora2.Value = txtNuevaImpresora.Text;
            //        lstImpresora.Items.Add(oImpresora2);

            //    }
            //}
            //lstImpresora.UpdateAfterCallBack = true;
           
        }

        private bool VerificarExisteImpresoras()
        {
            bool existe = false;
            if (lstImpresora.Items.Count > 0)
            {
                /////Crea nuevamente los detalles.
                for (int i = 0; i < lstImpresora.Items.Count; i++)
                {
                    if (ddlImpresora.SelectedValue == lstImpresora.Items[i].Value)
                    {
                        existe = true; break;
                    }

                    if (txtNuevaImpresora.Text == lstImpresora.Items[i].Value)
                    {
                        existe = true; break;
                    }
                }
            }
            return existe;
        }

        protected void btnGuardarImpresora_Click(object sender, EventArgs e)
        {
            GuardarImpresoras();
            // if (lstImpresora.Items.Count > 0)
            //{
            //     ////borra las impresoras guardadas
            //    ISession m_session = NHibernateHttpModule.CurrentSession;
            //    ICriteria crit2 = m_session.CreateCriteria(typeof(Impresora));
            //    IList listaImpresoras = crit2.List();
            //    foreach (Impresora oImpr in listaImpresoras)
            //    {
            //        oImpr.Delete();
            //    }

            //    /////Crea nuevamente los detalles.
            //    for (int i = 0; i < lstImpresora.Items.Count; i++)
            //    {
            //         Impresora oRegistro = new Impresora();
            //         oRegistro.Nombre = lstImpresora.Items[i].Value;
            //         oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            //         oRegistro.FechaRegistro = DateTime.Now;
            //         oRegistro.Save();
            //    }
            //}

        }


        private void GuardarImpresoras()
        {
            //if (lstImpresora.Items.Count > 0)
            //{
            ////borra las impresoras guardadas
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session.CreateCriteria(typeof(Impresora));
            crit2.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));
            IList listaImpresoras = crit2.List();
            foreach (Impresora oImpr in listaImpresoras)
            {
                oImpr.Delete();
            }

            /////Crea nuevamente los detalles.
            for (int i = 0; i < lstImpresora.Items.Count; i++)
            {
                Impresora oRegistro = new Impresora();
                oRegistro.IdEfector = oUser.IdEfector.IdEfector;
                oRegistro.Nombre = lstImpresora.Items[i].Value;
                oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();
            }
            //}
        }

        protected void btnSacarImpresora_Click(object sender, ImageClickEventArgs e)
        {
           
            
        }

        protected void btnAgregarImpresora_Click(object sender, EventArgs e)
        {
            if (!VerificarExisteImpresoras())
            {
                if ((ddlImpresora.SelectedValue != "0") && (ddlImpresora.Enabled))
                {
                    ListItem oImpresora = new ListItem();
                    oImpresora.Text = ddlImpresora.SelectedValue;
                    oImpresora.Value = ddlImpresora.SelectedValue;
                    lstImpresora.Items.Add(oImpresora);
                }

                if (txtNuevaImpresora.Text != "")
                {
                    ListItem oImpresora2 = new ListItem();
                    oImpresora2.Text = txtNuevaImpresora.Text;
                    oImpresora2.Value = txtNuevaImpresora.Text;
                    lstImpresora.Items.Add(oImpresora2);

                }
            }
            lstImpresora.UpdateAfterCallBack = true;

        }

        protected void btnSacarImpresora_Click(object sender, EventArgs e)
        {
            if (lstImpresora.SelectedValue != "")
            {
                lstImpresora.Items.Remove(lstImpresora.SelectedItem);
            }


            lstImpresora.UpdateAfterCallBack = true;

        }
    }
}
