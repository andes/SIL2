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
    public partial class Derivados2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    if (Request["tipo"] == "informe")
                    {
                        lblTitulo.Text = "DERIVACIONES";
                        VerificaPermisos("Gestionar");
                    }
                    if (Request["tipo"] == "resultado")
                    {
                        lblTitulo.Text = "CARGA DE RESULTADOS DE DERIVACIONES";
                        VerificaPermisos("Resultados");
                        //Para los resultados debe estar el estado "Enviado" en la grilla
                    }
                    CargarListas();

                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    txtFechaDesde.Focus();
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
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
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdConfiguracion", 1, "IdEfector", oUser.IdEfector);

            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
            CargarArea();

            ///Carga de combos de Origen
            m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("Todos", "0"));

            ///Carga de combos de Prioridad
            m_ssql = "SELECT idPrioridad, nombre FROM LAB_Prioridad WHERE     (baja = 0)";
            oUtil.CargarCombo(ddlPrioridad, m_ssql, "idPrioridad", "nombre");
            ddlPrioridad.Items.Insert(0, new ListItem("Todos", "0"));


            m_ssql = "SELECT  E.idEfector, E.nombre " +
               " FROM  Sys_Efector AS E " +
               " where E.idEfector IN  (SELECT DISTINCT idEfectorDerivacion FROM   lab_itemEfector AS IE  WHERE Ie.disponible=1 and IE.idEfector<>Ie.idEfectorDerivacion and  IE.idEfector=" + oUser.IdEfector.IdEfector.ToString() + ")" +
               "    ORDER BY E.nombre";
            //oUtil.CargarListBox(lstEfectores, m_ssql, "idEfector", "nombre");
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("--Seleccione--", "0"));
            CargarItem();

            if (Request["tipo"] == "informe")
                CargarEstadoInforme();
            else
                CargarEstadoResultado();

            m_ssql = null;
            oUtil = null;
        }

        private void CargarEstadoInforme()
        {
            Utility oUtil = new Utility();
            string query_string = "SELECT idEstado,descripcion FROM LAB_DerivacionEstado where idEstado in (0,1,2,4)";
            oUtil.CargarRadioButton(rdbEstado, query_string, "idEstado", "descripcion");
            rdbEstado.SelectedIndex = 0;
        }

        private void CargarEstadoResultado()
        {
            Utility oUtil = new Utility();
            string query_string = "SELECT idEstado,descripcion FROM LAB_DerivacionEstado where idEstado in (0,1,2)";
            oUtil.CargarRadioButton(rdbEstado, query_string, "idEstado", "descripcion");
            rdbEstado.SelectedIndex = 0;
        }




        private void CargarArea()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = "";
            if (ddlServicio.SelectedValue != "0")
                m_ssql = "select idArea, nombre from Lab_Area where baja=0    and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            else
                m_ssql = "select idArea, nombre from Lab_Area where baja=0   order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));

            m_ssql = null;
            oUtil = null;

        }



        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            // if (Page.IsValid) MostrarInforme("PDF");
        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid) MostrarInforme("Imprimir");
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
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
                    string str_condicion = " 1= 1 AND fecha>='" + fecha1.ToString("yyyyMMdd") + "' and fecha<='" + fecha2.ToString("yyyyMMdd") + "'";

                    if (ddlOrigen.SelectedValue != "0")
                        str_condicion += " AND idOrigen = " + ddlOrigen.SelectedValue;
                    if (ddlPrioridad.SelectedValue != "0")
                        str_condicion += " AND idPrioridad = " + ddlPrioridad.SelectedValue;
                    if (ddlServicio.SelectedValue != "0")
                        str_condicion += " AND idTipoServicio = " + ddlServicio.SelectedValue;
                    if (ddlArea.SelectedValue != "0")
                        str_condicion += " AND idArea = " + ddlArea.SelectedValue;
                    if (ddlEfector.SelectedValue != "0")
                        str_condicion += " AND idEfectorDerivacion = " + ddlEfector.SelectedValue;
                    if (ddlItem.SelectedValue != "0")
                        str_condicion += " AND idItem = " + ddlItem.SelectedValue;
                    str_condicion += " AND idEfector= " + oUser.IdEfector.IdEfector.ToString();

                    verificaResultados(str_condicion);

                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {

            CargarItem();
            ddlItem.UpdateAfterCallBack = true;

        }

        private void CargarItem()
        {
            if (Session["idUsuario"] == null)
                Response.Redirect("logout.aspx", false);
            else
            {
                if (ddlEfector.SelectedValue != "0")
                {

                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));


                    Utility oUtil = new Utility();
                    string m_ssql = @" SELECT  i.idItem, nombre as determinacion FROM lab_item I
                                     inner join LAB_ItemEfector IE on IE.idItem= I.iditem
                                     WHERE baja=0 AND (ie.disponible = 1) and Ie.idEfectorDerivacion =" + ddlEfector.SelectedValue +
                                     "  and IE.idEfector= " + oUser.IdEfector.IdEfector.ToString() + " order by nombre";

                    //" SELECT  idItem, nombre as determinacion FROM lab_item WHERE baja=0 AND (disponible = 1) and idEfectorDerivacion =" + ddlEfector.SelectedValue + " order by nombre";
                    oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "determinacion");


                    ddlItem.Items.Insert(0, new ListItem("Todas", "0"));
                }
            }
        }
        private void verificaResultados(string str_condicion)
        {
            DataTable dt = GetDataSet(str_condicion);

            if (dt.Rows.Count > 0)
            {
                if (Request["tipo"] == "informe")
                    Response.Redirect("InformeList3.aspx?Parametros=" + str_condicion + "&Estado=" + rdbEstado.SelectedValue + "&Destino=" + ddlEfector.SelectedValue + "&Tipo=Alta" , false);
                else
                if (Request["tipo"] == "resultado")
                    Response.Redirect("../Derivaciones/ResultadoEdit.aspx?Parametros=" + str_condicion, false);
               
            }
            else
            {
                cv_botonBuscar.IsValid = false; //que de error sin enviar alert
            }


        }

        public DataTable GetDataSet(string parametros)
        {

            int estado = Convert.ToInt32(rdbEstado.SelectedValue);

            string m_strSQL = @" 
             SELECT  idDetalleProtocolo, estado, numero, convert(varchar(10), fecha,103) as fecha, dni, 
                 apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , 
                solicitante as especialista , isnull(idlote,0) as idLote , isnull(mot.descripcion,'') as motivo
             FROM  vta_LAB_Derivaciones vta left join LAB_DerivacionMotivoCancelacion mot on mot.idMotivo = vta.idMotivoCancelacion 
             WHERE " + parametros + "  and estado = " + estado;

            if(estado == 0) //Pendiente de derivar
                m_strSQL += " and idlote = 0 ";//No tiene que tener lote asociado
            
            
           
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

    }
}
