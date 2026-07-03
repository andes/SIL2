using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;
using Business.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Drawing;

namespace WebLab.Estadisticas
{
    public partial class  ReporteInsumo : System.Web.UI.Page
    {
     //   private int cantidad = 0;
       // public string mensagitoInicial = ""; string mensagitoProcesa = "";

        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"]!= null)
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            else
                Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                    VerificaPermisos("Auditoria de Insumo");


                CargarListas();
                 
            }


        }
     
        private void CargarListas()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            Utility oUtil = new Utility();
            ///Carga de combos de servicios
            ///
            //string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE idTipoServicio<>5 and (baja = 0)";
            //oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            //ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
            //CargarArea();


            //if (oUser.IdEfector.IdEfector.ToString() == "227")
            //{
            //    m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E (nolock) " +
            //         " INNER JOIN lab_Configuracion C (nolock)  on C.idEfector=E.idEfector " +
            //         "order by E.nombre";

            //    oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            //  //  ddlEfector.Items.Insert(0, new ListItem("Seleccione Efector", "0"));
            //}
            //else
            //{
            string m_ssql = "select  E.idEfector, E.nombre  from sys_efector E (nolock)   where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            //}

          

        }

        //private void CargarArea()
        //{
        //    string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

        //    Utility oUtil = new Utility();
        //    string m_ssql = "";
        //    if (ddlServicio.SelectedValue != "0")
        //        m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
        //    else
        //        m_ssql = "select idArea, nombre from Lab_Area (nolock)  where baja=0  order by nombre";
        //    oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);
        //    ddlArea.Items.Insert(0, new ListItem("Todas", "0"));
        //    ddlArea.UpdateAfterCallBack = true;

        //}
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

      

      
       

        
        

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }
        private void CargarGrilla()
        {
            string m_condicion = "";
            if (ddlEfector.SelectedValue != "0")
            {
                m_condicion = " AND efector='" + ddlEfector.SelectedItem.Text + "'";
            }
            string m_sql = @"
 WITH Estados AS
(
    SELECT
        idAuditoriaItem,
      codigo, nombre,
        valor AS Efector,
        fecha,
        analisis,
        LEAD(analisis) OVER (
            PARTITION BY it.idItem, valor
            ORDER BY fecha
        ) AS EstadoSiguiente,
        LEAD(fecha) OVER (
            PARTITION BY  it.idItem, valor
            ORDER BY fecha
        ) AS FechaFin
    FROM LAB_AuditoriaItem it
	 inner join [LAB_Item] i on it.iditem = i.iditem and tipo = 'P'
    WHERE 1=1---idItem = 2393
      AND accion = 'Cambia estado'
      AND analisis IN ('Sin Insumo','Disponible')
)
SELECT
      codigo, nombre,
    Efector,
    fecha AS InicioSinInsumo,
    FechaFin AS FinSinInsumo,
    DATEDIFF(DAY, fecha, ISNULL(FechaFin,GETDATE())) AS DiasSinInsumo
FROM Estados
WHERE analisis='Sin Insumo'
" + m_condicion + @"
and  DATEDIFF(DAY, fecha, ISNULL(FechaFin,GETDATE()))>0
ORDER BY codigo, fecha;
";


            DataTable dt = new DataTable();
            SqlConnection conn =
                (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;


            SqlDataAdapter da = new SqlDataAdapter(m_sql, conn);


            da.Fill(dt);


            gvSinInsumo.DataSource = dt;
            gvSinInsumo.DataBind();


            lblCantidadRegistros.Text =
                dt.Rows.Count.ToString() +
                " registros encontrados";

        }

    }
} 