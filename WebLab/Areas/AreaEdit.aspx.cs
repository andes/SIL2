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


namespace WebLab.Areas
{
    public partial class AreaEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Area");
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
            Area oArea = new Area();
            oArea = (Area)oArea.Get(typeof(Area), int.Parse( Request["id"]));
            txtNombre.Text = oArea.Nombre;
            ddlTipoServicio.SelectedValue = oArea.IdTipoServicio.IdTipoServicio.ToString();
            
         
   chkImprimeCodigoBarras.Checked = oArea.ImprimeCodigoBarra;

            ConfiguracionCodigoBarra oConfiguracion = new ConfiguracionCodigoBarra();
            oConfiguracion = (ConfiguracionCodigoBarra)oConfiguracion.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oArea.IdTipoServicio);
            if (oConfiguracion == null)
            {
                chkImprimeCodigoBarras.Enabled = false;
                chkImprimeCodigoBarras.Checked = false;
            }
            else
            {
                chkImprimeCodigoBarras.Enabled = oConfiguracion.Habilitado;
                if (!oConfiguracion.Habilitado )chkImprimeCodigoBarras.Checked = oConfiguracion.Habilitado;
            }

          
        }
        private void CargarListas()
        {            
        Utility oUtil = new Utility();
       
        string m_ssql = "select idTipoServicio,nombre  from Lab_TipoServicio with (nolock) where idtiposervicio<>5 and baja = 0";
            oUtil.CargarCombo(ddlTipoServicio, m_ssql, "idTipoServicio", "nombre");
            ddlTipoServicio.Items.Insert(0, new ListItem("Seleccione Tipo de Servicio", "0"));
        m_ssql = null;
        oUtil = null;   
        }

        

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Area oArea = new Area();
                if (Request["id"] != null) oArea = (Area)oArea.Get(typeof(Area), int.Parse( Request["id"]));
                Guardar(oArea);

                if (Request["id"] != null)
                    Response.Redirect("AreaList.aspx");
                else
                Response.Redirect("AreaEdit.aspx");
            }
        }

       
        private void Guardar(Area oRegistro)
        {
            
            TipoServicio oTipo = new TipoServicio();
            Usuario oUser= new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //Efector oEfector = new Efector();

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.IdTipoServicio = (TipoServicio)oTipo.Get(typeof(TipoServicio), int.Parse(ddlTipoServicio.SelectedValue));
            oRegistro.ImprimeCodigoBarra = chkImprimeCodigoBarras.Checked; 
        //    oRegistro.IdUsuarioResponsable = (Usuario)oUser.Get(typeof(Usuario), int.Parse(ddlResponsable.SelectedValue));
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();                                  
    }
              
               
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListaArea.aspx");
        }

        protected void ddlTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {  
            TipoServicio oTipo = new TipoServicio();
             oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), int.Parse(ddlTipoServicio.SelectedValue));

            ConfiguracionCodigoBarra oConfiguracion=  new ConfiguracionCodigoBarra();
            oConfiguracion = (ConfiguracionCodigoBarra)oConfiguracion.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo);
            if (oConfiguracion == null)
                chkImprimeCodigoBarras.Enabled = false;
            else
                chkImprimeCodigoBarras.Enabled = oConfiguracion.Habilitado;
            


        }
    }
}
