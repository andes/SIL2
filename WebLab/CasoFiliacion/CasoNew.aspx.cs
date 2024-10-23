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
using System.Text.RegularExpressions;

namespace WebLab.CasoFiliacion
{
    public partial class CasoNew : System.Web.UI.Page
    {
        /*Creacion del caso forense: tipo 1 de filiacion, tipo 2 forense
        Accesible desde FiliacionMenu.aspx
        */
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
           
            {
                VerificaPermisos("Casos Forense");
                CargarListas();

            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,    nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));
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

                Session["idCaso"] = oRegistro.IdCasoFiliacion.ToString();
                if (oRegistro.IdTipoCaso==1) 
                    //caso de filiacion: sigue el circuito con identificacion de personas y consentimiento
                Response.Redirect("../Protocolos/Default2.aspx?idServicio=6&idUrgencia=0&idCaso="+ oRegistro.IdCasoFiliacion.ToString() + "&idTipoCaso=" + oRegistro.IdTipoCaso.ToString(), false);
                else 
                //caso forense: va directo a la carga de muestra no asociada a persona
                {

                    Response.Redirect("FacturacionForense/CasoPresupuesto.aspx?idCaso=" + oRegistro.IdCasoFiliacion.ToString()+"&Desde=NuevoCaso", false);
                    ///// Debe ir a Default2.aspx con la restriccion de
                    //Response.Redirect("../Protocolos/Default2.aspx?idServicio=6&idUrgencia=0&idCaso=" + oRegistro.IdCasoFiliacion.ToString()+"&idTipoCaso="+ oRegistro.IdTipoCaso.ToString(), false);
                    ///Response.Redirect("../Protocolos/ProtocoloEditForense.aspx?idPaciente=-1&Operacion=Alta&idServicio=6&idUrgencia=0&idCaso=" + oRegistro.IdCasoFiliacion.ToString(), false);
                }

            }
        }

        private void Guardar(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {


            Utility oUtil = new Utility();
            //Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
            Usuario oUser = new Usuario();
            oUser=(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            oRegistro.IdEfector = oUser.IdEfector;
            //string s_nombre= Regex.Replace(txtNombre.Text, @"[^0-9A-Za-z]", "", RegexOptions.None);
            //string s_nombre = Regex.Replace(txtNombre.Text, @"[^\w\s.!@$%^&*()\-\/]+", "");

            oRegistro.Nombre =oUtil.SacaComillas( oUtil.RemoverSignosAcentos(txtNombre.Text));

            oRegistro.IdTipoCaso = int.Parse(ddlTipoCaso.SelectedValue);
            oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Solicitante = ddlSectorServicio.SelectedItem.Text;
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
