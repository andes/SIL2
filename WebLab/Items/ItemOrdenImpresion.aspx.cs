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

namespace WebLab.Items
{
    public partial class ItemOrdenImpresion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Orden de impresion");
                CargarListas();              
            }

        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnGuardarAnalisis.Visible = false;
                            btnGuardarArea.Visible = false;
                            imgBajarAnalisis.Visible = false;
                            imgBajarArea.Visible = false;
                            imgSubirAnalisis.Visible = false;
                            imgSubirArea.Visible = false;
                            
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void CargarListas()
        {
         
            Utility oUtil = new Utility();
            ///Carga de combos de Areas
            ///
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            CargarArea();


            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (ddlServicio.SelectedValue == "1")
            {
                if (oCon.OrdenImpresionLaboratorio)
                    rdbOrdenImpresion.SelectedValue = "1";//segun ingreso del protocolo
                else
                    rdbOrdenImpresion.SelectedValue = "0";//segun orden predeterminado
            }
            if (ddlServicio.SelectedValue == "3")
            {
                if (oCon.OrdenImpresionMicrobiologia)
                    rdbOrdenImpresion.SelectedValue = "1";//segun ingreso del protocolo
                else
                    rdbOrdenImpresion.SelectedValue = "0";//segun orden predeterminado
            }
            m_ssql = null;
            oUtil = null;
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Seleccione Area", "0"));
            ddlArea.UpdateAfterCallBack = true;

            m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by ordenImpresion";
            oUtil.CargarListBox(lstArea, m_ssql, "idArea", "nombre");
            lstArea.UpdateAfterCallBack = true;

        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            CargarAnalisis();
        }

        private void CargarAnalisis()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select idItem, nombre + ' [' + codigo + ']' as nombre from Lab_Item where baja=0 and tipo='P' " +
                " and idArea=" + ddlArea.SelectedValue + " order by ordenImpresion, nombre";
            oUtil.CargarListBox(lstAnalisis,m_ssql,"idItem","nombre");
            lstAnalisis.UpdateAfterCallBack=true;
            
            
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
        }

        protected void btnSubir_Click(object sender, EventArgs e)
        {
          

        }

        protected void btnBajar_Click(object sender, EventArgs e)
        {
           
        }

        private void BajarAnalisis()
        {
            if (lstAnalisis.SelectedItem != null)
            {
                //Item seleccionado
                ListItem item = lstAnalisis.SelectedItem;

                //indice del item seleccionado
                int indice = lstAnalisis.Items.IndexOf(lstAnalisis.SelectedItem);

                if (indice + 1 <= lstAnalisis.Items.Count - 1)
                {
                    lstAnalisis.SelectedIndex = -1; //Quita la seleccion del item
                    lstAnalisis.Items.Insert(indice + 2, item);
                    lstAnalisis.Items.RemoveAt(indice);
                    lstAnalisis.SelectedIndex = indice + 1;
                }
            }
            lstAnalisis.UpdateAfterCallBack = true;
        }

        private void BajarAreas()
        {
            if (lstArea.SelectedItem != null)
            {
                //Item seleccionado
                ListItem item = lstArea.SelectedItem;

                //indice del item seleccionado
                int indice = lstArea.Items.IndexOf(lstArea.SelectedItem);

                if (indice + 1 <= lstArea.Items.Count - 1)
                {
                    lstArea.SelectedIndex = -1; //Quita la seleccion del item
                    lstArea.Items.Insert(indice + 2, item);
                    lstArea.Items.RemoveAt(indice);
                    lstArea.SelectedIndex = indice + 1;
                }
            }
            lstArea.UpdateAfterCallBack = true;
        }
       

        private void SubirAnalisis()
        {
            if (lstAnalisis.SelectedItem != null)
            {
                //Item seleccionado
                ListItem item = lstAnalisis.SelectedItem;

                //indice del item seleccionado
                int indice = lstAnalisis.Items.IndexOf(lstAnalisis.SelectedItem);

                if (indice - 1 >= 0)
                {
                    lstAnalisis.SelectedIndex = -1; //Quita la seleccion del item
                    lstAnalisis.Items.Insert(indice - 1, item);
                    lstAnalisis.Items.RemoveAt(indice + 1);
                    lstAnalisis.SelectedIndex = indice - 1;
                }
            }
            lstAnalisis.UpdateAfterCallBack = true;
        }


        private void SubirAreas()
        {
            if (lstArea.SelectedItem != null)
            {
                //Item seleccionado
                ListItem item = lstArea.SelectedItem;

                //indice del item seleccionado
                int indice = lstArea.Items.IndexOf(lstArea.SelectedItem);

                if (indice - 1 >= 0)
                {
                    lstArea.SelectedIndex = -1; //Quita la seleccion del item
                    lstArea.Items.Insert(indice - 1, item);
                    lstArea.Items.RemoveAt(indice + 1);
                    lstArea.SelectedIndex = indice - 1;
                }
            }
            lstArea.UpdateAfterCallBack = true;
        }


        private void GuardarOrdenAnalisis()
        {
            for (int i = 0; i < lstAnalisis.Items.Count; i++)
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(lstAnalisis.Items[i].Value));
                oItem.OrdenImpresion = i+1;
                oItem.Save();
            }
            string popupScript = "<script language='JavaScript'> alert('Se ha actualizado el orden de impresión'); </script>";
            Page.RegisterStartupScript("PopupScript", popupScript);
        }

        protected void btnGuardarAnalisis_Click(object sender, EventArgs e)
        {
 GuardarOrdenAnalisis();
        }

        protected void imgBajarAnalisis_Click(object sender, ImageClickEventArgs e)
        {
BajarAnalisis();
        }

        protected void imgSubirAnalisis_Click(object sender, ImageClickEventArgs e)
        {
            SubirAnalisis();
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarAnalisis();

        }

        protected void imgSubirArea_Click(object sender, ImageClickEventArgs e)
        {
            SubirAreas();
        }

        protected void imgBajarArea_Click(object sender, ImageClickEventArgs e)
        {
            BajarAreas();
        }

        protected void btnGuardarArea_Click(object sender, EventArgs e)
        {
            GuardarOrdenAreas();
        }

        private void GuardarOrdenAreas()
        {
            for (int i = 0; i < lstArea.Items.Count; i++)
            {
                Area oArea = new Area();
                oArea = (Area)oArea.Get(typeof(Area), int.Parse(lstArea.Items[i].Value));
                oArea.OrdenImpresion = i + 1;
                oArea.Save();
            }
            string popupScript = "<script language='JavaScript'> alert('Se ha actualizado el orden de impresión'); </script>";
            Page.RegisterStartupScript("PopupScript", popupScript);
        }

        protected void ddlServicio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            MostrarOrdenImpresion();
            CargarArea();
            CargarAnalisis();
        }

        private void MostrarOrdenImpresion()
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (ddlServicio.SelectedValue == "1")
            {
                if (oCon.OrdenImpresionLaboratorio)
                    rdbOrdenImpresion.SelectedValue = "1";//segun ingreso del protocolo
                else
                    rdbOrdenImpresion.SelectedValue = "0";//segun orden predeterminado
            }
            if (ddlServicio.SelectedValue == "3")
            {
                if (oCon.OrdenImpresionMicrobiologia)
                    rdbOrdenImpresion.SelectedValue = "1";//segun ingreso del protocolo
                else
                    rdbOrdenImpresion.SelectedValue = "0";//segun orden predeterminado
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarTipoOrdenImpresion();
        }

        private void GuardarTipoOrdenImpresion()
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (ddlServicio.SelectedValue == "1")
            {
                if (rdbOrdenImpresion.SelectedValue == "1")
                    oCon.OrdenImpresionLaboratorio = true;
                else
                    oCon.OrdenImpresionLaboratorio = false;
            }

            if (ddlServicio.SelectedValue == "3")
            {
                if (rdbOrdenImpresion.SelectedValue == "1")
                    oCon.OrdenImpresionMicrobiologia = true;
                else
                    oCon.OrdenImpresionMicrobiologia = false;
            }

            oCon.Save();
        }
    }
}
