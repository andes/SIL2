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
using Business.Data;
using Business;
using Business.Data.Facturacion;

namespace WebLab.NomencladorF
{
    public partial class NomencladorForenseEdit : System.Web.UI.Page
    {
        Utility oUtil = new Utility();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Nomenclador Forense");

                if (Request["id"] != null)
                    MostrarDatos();
            }
        }


        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
            //    Utility oUtil = new Utility();
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
            NomencladorForense oRegistro = new NomencladorForense();
            oRegistro = (NomencladorForense)oRegistro.Get(typeof(NomencladorForense), int.Parse( Request["id"].ToString()));
            txtCodigo.Text = oRegistro.Codigo;
            txtNombre.Text = oRegistro.Nombre;
            txtPrecio.Text = oRegistro.Precio.ToString();      
        }
       



        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                NomencladorForense oRegistro = new NomencladorForense();
                if (Request["id"] != null) oRegistro = (NomencladorForense)oRegistro.Get(typeof(NomencladorForense), int.Parse(Request["id"].ToString()));                
                Guardar(oRegistro);

              
            }
        }


        private void Guardar(NomencladorForense oRegistro)
        {

            if (!oRegistro.noexistecodigo(txtCodigo.Text))
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                oRegistro.Codigo = txtCodigo.Text;

                oRegistro.Nombre = txtNombre.Text;
                oRegistro.Precio = decimal.Parse(txtPrecio.Text); // (Efector)oEfector.Get(typeof(Efector), "IdEfector", 5);            
                oRegistro.IdUsuarioRegistro = oUser;
                oRegistro.FechaRegistro = DateTime.Now;

                oRegistro.Save();
                if (Request["id"] != null)
                    Response.Redirect("NomencladorForenseList.aspx", false);
                else
                    Response.Redirect("NomencladorForenseEdit.aspx", false);
            }
            else
            {       lblMensaje.Text = "Codigo existente";
            lblMensaje.Visible = true;
        }
        }


        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("NomencladorForenseList.aspx");
        }
    }
}
