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
using NHibernate;
using NHibernate.Expression;

namespace WebLab.CasoFiliacion
{
    public partial class CasoNewPrueba : System.Web.UI.Page
    {
        /*Caso nnuevo de histocompatibilidad
        Accesible desde CasoList (lista de casos de histocompatibilidad)
        */
       
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
         

            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                      {

                HttpContext Context;
                Context = HttpContext.Current;
                if (Context.Items.Contains("idServicio"))
                    if ( Context.Items["idServicio"].ToString()=="3") 
                VerificaPermisos("Casos Histocompatibilidad");
            }
        }
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnGuardar.Visible = false;
                           
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

    


     


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
               Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                if (Request["id"] != null) oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Request["id"]));
                Guardar(oRegistro);


                //Response.Redirect("CasoEdit.aspx?idServicio=3&idUrgencia=0&id="+ oRegistro.IdCasoFiliacion.ToString(), false);
                HttpContext Context;

                Context = HttpContext.Current;
                Context.Items.Add("idServicio", "3");
                Context.Items.Add("id", oRegistro.IdCasoFiliacion.ToString());
                Server.Transfer("CasoEdit.aspx");
            }
        }

        private void Guardar(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
          
       
          
            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.Nombre = txtNombre.Text;

            
            oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Objetivo = "";
            oRegistro.Muestra = "";
            oRegistro.Resultado = "";
            oRegistro.Conclusion = "";
            oRegistro.Metodo = "";
            oRegistro.Amplificacion = "";
            oRegistro.Analisis = "";
            oRegistro.Software = "";
            oRegistro.Analisis = "";
            oRegistro.Marcoestudio = "";

            oRegistro.FechaCarga = DateTime.Parse("01/01/1900");
            oRegistro.IdUsuarioCarga = 0;
            oRegistro.FechaValida = DateTime.Parse("01/01/1900");
            oRegistro.IdUsuarioValida = 0;
            oRegistro.FechaTransplante = DateTime.Parse("01/01/1900");
            oRegistro.Save();

            oRegistro.GrabarAuditoria("Graba", oUser.IdUsuario, "");

                   
        //     GuardarDetalle(oRegistro);




    }

       




      


    }
}
