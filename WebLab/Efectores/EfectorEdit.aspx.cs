using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Business.Data;
using Business;
using Business.Data.Laboratorio;
using System.Collections;


namespace WebLab.Efectores
{
    public partial class EfectorEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Efector");
                CargarListas();
                if (Request["id"] != null)
                    MostrarDatos();
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
               // Utility oUtil = new Utility();
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
            Business.Data.Efector oArea = new Business.Data.Efector();
            oArea = (Business.Data.Efector)oArea.Get(typeof(Business.Data.Efector), int.Parse(Request["id"]));
            txtNombre.Text = oArea.Nombre;
            ddlZona.SelectedValue = oArea.IdZona.IdZona.ToString();
            if (oArea.IdTipoEfector == 2) chkPrivado.Checked = true;
            else chkPrivado.Checked = false;


        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string m_ssql = "select idZona,nombre  from Sys_Zona order by nombre";
            oUtil.CargarCombo(ddlZona, m_ssql, "idZona", "nombre");
            ddlZona.Items.Insert(0, new ListItem("Seleccione Zona", "0"));
            m_ssql = null;
            oUtil = null;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Efector oArea = new Business.Data.Efector();
                if (Request["id"] != null) oArea = (Business.Data.Efector)oArea.Get(typeof(Business.Data.Efector), int.Parse( Request["id"]));
                Guardar(oArea);

                if (Request["id"] != null)
                    Response.Redirect("EfectorList.aspx");
                else
                Response.Redirect("EfectorEdit.aspx");
            }
        }

       
        private void Guardar(Business.Data.Efector oRegistro)
        {
            
          
             
            //Efector oEfector = new Efector();

            Zona oC = new Zona();
            oC = (Zona)oC.Get(typeof(Zona), "IdZona", int.Parse(ddlZona.SelectedValue));

            oRegistro.Nombre = txtNombre.Text;
            if (chkPrivado.Checked)
                oRegistro.IdTipoEfector = 2;
            oRegistro.IdZona = oC;
             

            oRegistro.Save();                                  
    }
              
               
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("EfectorList.aspx");
        }

    
    }
}
