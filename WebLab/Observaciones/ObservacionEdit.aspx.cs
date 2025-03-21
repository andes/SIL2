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
using Business.Data;

namespace WebLab.Observaciones
{
    public partial class ObservacionEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
               

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("Observaciones");
                CargarListas();
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
                //      Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();   ///Carga de combos de Areas        
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura



            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlTipoServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlTipoServicio.Items.Insert(0, new ListItem("--Seleccione--", "0"));


            m_ssql = null;
            oUtil = null;
        }



        private void MostrarDatos()
        {

            ObservacionResultado oRegistro = new ObservacionResultado();
            oRegistro = (ObservacionResultado)oRegistro.Get(typeof(ObservacionResultado), int.Parse(Request["id"]));
            ddlTipoServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
            txtNombre.Text = oRegistro.Nombre;
            txtAbreviatura.Text = oRegistro.Codigo;


        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                ObservacionResultado oRegistro = new ObservacionResultado();
                if (Request["id"] != null) oRegistro = (ObservacionResultado)oRegistro.Get(typeof(ObservacionResultado), int.Parse(Request["id"]));
                Guardar(oRegistro);

                if (Request["id"] != null)
                    Response.Redirect("ObservacionList.aspx");
                else
                    Response.Redirect("ObservacionEdit.aspx");
            }
        }


        private void Guardar(ObservacionResultado oRegistro)
        {
            TipoServicio oTipo = new TipoServicio();
          
            if (txtNombre.Text.Length>1000)
                oRegistro.Nombre = txtNombre.Text.Substring(1,999);
            else
            oRegistro.Nombre = txtNombre.Text;
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.IdTipoServicio = (TipoServicio)oTipo.Get(typeof(TipoServicio), int.Parse(ddlTipoServicio.SelectedValue));
            oRegistro.Codigo = txtAbreviatura.Text;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ObservacionList.aspx");
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtNombre.Text.Length>1000)
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "el largo del texto supera el limite de caracteres. Verifique.";
                return;

            }
        }
    }
}
