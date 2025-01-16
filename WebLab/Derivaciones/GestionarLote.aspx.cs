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
using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class GestionarLote : System.Web.UI.Page
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    lblTitulo.Text = "LOTES";
                      
                    CargarListas();

                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    txtFechaDesde.Focus();
                    
                }
                else Response.Redirect("../FinSesion.aspx", false);

            }
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
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarListas()

        {
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
           
            Utility oUtil = new Utility();

            string m_ssql = "SELECT  E.idEfector, E.nombre " +
               " FROM  Sys_Efector AS E " +
               " where E.idEfector IN  (SELECT DISTINCT idEfectorDerivacion FROM lab_itemEfector AS IE " +
               " WHERE Ie.disponible=1 and IE.idEfector<>Ie.idEfectorDerivacion and  IE.idEfector=" + oUser.IdEfector.IdEfector.ToString() +")" +
               " ORDER BY E.nombre";
            
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("--- TODOS ---", "0"));

            oUtil = new Utility();
            string query_string = "SELECT idEstado, nombre FROM LAB_LoteDerivacionEstado   where baja = 0 and idEstado in (1,2,3)";
                //"SELECT idEstado,descripcion FROM LAB_DerivacionEstado where idEstado in (1,2,3)";
            oUtil.CargarRadioButton(rdbEstado, query_string, "idEstado", "nombre");
            rdbEstado.SelectedIndex = 0;

        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["idUsuario"] != null)
                {
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                    string str_condicion = " 1= 1 AND fechaRegistro >='" + fecha1.ToString("yyyyMMdd") + " 00:00:00.000' and fechaRegistro <='" + fecha2.ToString("yyyyMMdd") + " 23:59:59.999'";

                    if (ddlEfector.SelectedValue != "0") str_condicion += " AND l.idEfectorDestino = " + ddlEfector.SelectedValue;
                    str_condicion += " AND idEfectorOrigen = " + oUser.IdEfector.IdEfector.ToString();

                    if (!string.IsNullOrWhiteSpace(tb_nrolote.Text)){
                        str_condicion += " AND idLoteDerivacion = " + tb_nrolote.Text;
                    }

                   Response.Redirect("InformeLote.aspx?Parametros=" + str_condicion + "&Estado=" + rdbEstado.SelectedValue, false);
                   
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
        }
    }
}
