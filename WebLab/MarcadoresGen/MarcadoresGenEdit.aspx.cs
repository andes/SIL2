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

namespace WebLab.MarcadoresGen
{
    public partial class MarcadoresGenEdit : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {


            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Marcadores");
           
                if (Request["id"] != null)
                    MostrarDatos();
            }

        }

        private void MostrarDatos()
        {
            TipoMarcador oRegistro = new TipoMarcador();
            oRegistro = (TipoMarcador)oRegistro.Get(typeof(TipoMarcador), int.Parse(Request["id"].ToString()));

            txtCodigo.Text = oRegistro.Nombre;
           
  

               // dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                DetalleTipoMarcador oDetalle = new DetalleTipoMarcador();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleTipoMarcador));
                crit.Add(Expression.Eq("IdTipoMarcador", oRegistro));
                string sDatos = "";
                IList items = crit.List();
             //   string pivot = "";
                foreach (DetalleTipoMarcador oDet in items)
                {
                    if (sDatos=="")
                        sDatos =  oDet.Nombre + "#" + oDet.Nombre + "@";  
                    else
                    sDatos += oDet.Nombre + "#" + oDet.Nombre + "@";

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

 


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                TipoMarcador oRegistro = new TipoMarcador();
                if (Request["id"] != null) oRegistro = (TipoMarcador)oRegistro.Get(typeof(TipoMarcador), int.Parse(Request["id"].ToString()));
                Guardar(oRegistro);
                Response.Redirect("MarcadoresGenList.aspx", false);
            }
        }

        private void Guardar(TipoMarcador oRegistro)
        {
            //Usuario oUser = new Usuario();

            oRegistro.Nombre = txtCodigo.Text;

            oRegistro.IdUsuarioRegistro = oUser; // (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
            
            GuardarDetalle(oRegistro);
        }

        private void GuardarDetalle(TipoMarcador oRegistro)
        {
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleTipoMarcador));
            crit.Add(Expression.Eq("IdTipoMarcador", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetalleTipoMarcador oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }


            string[] tabla = TxtDatos.Value.Split('@');

            for (int i = 0; i < tabla.Length - 1; i++)
            {
                DetalleTipoMarcador oDetalle = new DetalleTipoMarcador();

                string[] fila = tabla[i].Split('#');
                
                

                oDetalle.IdTipoMarcador = oRegistro;
              
                oDetalle.Nombre= fila[1].ToString();

                oDetalle.Save();

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
            Response.Redirect("MarcadoresGenList.aspx", false);
        }

      
    }
}
