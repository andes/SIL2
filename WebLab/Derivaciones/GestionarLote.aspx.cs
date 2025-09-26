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
        public Usuario oUser = new Usuario();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (!Page.IsPostBack)
                {
                    VerificaPermisos("Gestionar Lotes");
                    //Session["GrillaGestionLotes"] = null; --> no se usa
                    lblTitulo.Text = "LOTES";
                      
                    CargarListas();

                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    txtFechaDesde.Focus();
                }
            }
                else Response.Redirect("../FinSesion.aspx", false);
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0:
                        Response.Redirect("../AccesoDenegado.aspx", false);
                        break;
                        //case 1: btn .Visible = false; break;
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarListas()
        {
            
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
             if (Session["idUsuario"] != null)
             {
                if (Page.IsValid)
                {
                    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                    string str_condicion = " 1= 1 AND fechaRegistro >='" + fecha1.ToString("yyyyMMdd") + " 00:00:00.000' and fechaRegistro <='" + fecha2.ToString("yyyyMMdd") + " 23:59:59.999'";

                    if (ddlEfector.SelectedValue != "0") str_condicion += " AND l.idEfectorDestino = " + ddlEfector.SelectedValue;
                    str_condicion += " AND idEfectorOrigen = " + oUser.IdEfector.IdEfector.ToString();

                    if (!string.IsNullOrWhiteSpace(tb_nrolote.Text))
                    {
                        str_condicion += " AND idLoteDerivacion = " + tb_nrolote.Text;
                    }

                    verificaResultados(Convert.ToInt32(rdbEstado.SelectedValue), str_condicion);
                  
                   
                }
                
             }
              else 
                Response.Redirect("../FinSesion.aspx", false);
        }

        

        protected void cv_nroLote_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                LoteDerivacion lote = new LoteDerivacion();
                lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), Convert.ToInt32(args.Value));
                args.IsValid = true;
            }
            catch (Exception)
            {
                args.IsValid = false;
            }
        }

        private void verificaResultados(int estado, string parametros)
        {
            string m_strSQL = " SELECT idLoteDerivacion as numero, e.nombre as efectorderivacion, l.estado, l.idEfectorDestino as idEfectorDerivacion," +
                             " fechaRegistro, " +
                             " case when (fechaenvio = '1900-01-01 00:00:00.000' ) then null else fechaEnvio end as fechaEnvio, " +
                             "  l.observacion ,uEmi.username as usernameE, isnull(uRecep.username, '' )  as usernameR " +
                             " FROM LAB_LoteDerivacion l " +
                             " inner join Sys_Efector e on e.idEfector=l.idEfectorDestino " +
                             " inner join Sys_Usuario uEmi on uEmi.idUsuario = idUsuarioRegistro " +
                             " left join Sys_Usuario uRecep on uRecep.idUsuario = idUsuarioEnvio " +
                             " where " + parametros + " AND baja = 0  AND estado = " + estado +
                             " ORDER BY l.idEfectorDestino, idLoteDerivacion ";

            DataTable dt = GetData(m_strSQL);

            if (dt.Rows.Count > 0)
            {
                Response.Redirect("InformeLote.aspx?Parametros=" + parametros + "&Estado=" + estado, false);
            }
            else
            {
                cv_botonBuscar.IsValid = false; //que de error sin enviar alert
            }

            
        }
        private DataTable GetData(string m_strSQL)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;//LAB-130 usar conexion principal no la de consulta
            SqlDataAdapter adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(m_strSQL, conn)
            };
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

    }
}
