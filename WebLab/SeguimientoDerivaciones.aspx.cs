using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data.Laboratorio;
using Business.Data;
using System.Data;
using System.Data.SqlClient;
using Business;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Drawing;
using System.Configuration;

namespace WebLab
{
    public partial class SeguimientoDerivaciones : System.Web.UI.Page
    {
        Configuracion oC = new Configuracion();
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
                VerificaPermisos("Seguimiento");
                if (Session["idUsuario"] != null)
                {
                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    CargarListas();
                    DataTable dtMuestras = MostrarDatos();

                    GridResultados.DataSource = dtMuestras;
                    GridResultados.DataBind();
                    GridResultados.Visible = true;
                        
                    


                }
                else Response.Redirect("FinSesion.aspx", false);
            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "SELECT idCaracter, nombre   FROM LAB_Caracter with (nolock)";
            oUtil.CargarCombo(ddlCaracter, m_ssql, "idCaracter", "nombre", connReady);
            ddlCaracter.Items.Insert(0, new ListItem("Todos", "0"));


            m_ssql = @" SELECT DISTINCT dp.resultadocar AS resultado
FROM LAB_detalleprotocolo dp with (nolock)
inner JOIN lab_item li with (nolock) ON dp.idsubitem = li.iditem and li.baja=0
WHERE li.codigo = '" + oC.CodigoCovid + @"'
  AND dp.idEfector = " + oUser.IdEfector.IdEfector.ToString() + @" ORDER BY dp.resultadocar";
//            m_ssql = @"select distinct  resultadocar as resultado from LAB_detalleprotocolo
//where iditem in (select iditem from lab_item where codigo = '" + oC.CodigoCovid + @"')  and idEfector = "+ oUser.IdEfector.IdEfector.ToString() +" order by resultadocar";

            oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado", connReady);
            ddlResultado.Items.Insert(0, new ListItem("Todos", "0"));

            m_ssql = @"select e.idEfector, e.nombre from sys_Efector  e

inner join LAB_ResultadoItem as R on R.idEfectorDeriva =e.idEfector
";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));

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
                if (Permiso!=2)
                {
                  Response.Redirect("AccesoDenegado.aspx", false); 
                    
                }
            }
            else Response.Redirect("FinSesion.aspx", false);
        }
        private DataTable MostrarDatos( )
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = " and 1=1 ";
            string m_strSQLCondicionEfector1 = "";
            string m_strSQLCondicionEfector2 = "";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);


            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
               
                    m_strSQLCondicion += " AND dp.fechavalida >= '" + fecha1.ToString("yyyyMMdd") + "'";
             
            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
               
                    m_strSQLCondicion += " AND dp.fechavalida < '" + fecha2.ToString("yyyyMMdd") + "'";
              
            }


            if (ddlCaracter.SelectedValue != "0")
                m_strSQLCondicion += " AND ir.idCaracter=" + ddlCaracter.SelectedValue;

            //if ((int.Parse(rdbOpcion.SelectedValue) == 2) || (int.Parse(rdbOpcion.SelectedValue) == 4))  //pacientes positivos
            //    m_strSQLCondicion += " and dp.resultadoCar like 'SE DETECTA%'  and Dp.idUsuarioValida>0";

            if (ddlResultado.SelectedValue != "0")
            {
                m_strSQLCondicion += " and dp.resultadoCar='" + ddlResultado.SelectedValue + "'";
              
            }
            if (ddlEfector.SelectedValue != "0")
            {
                m_strSQLCondicionEfector1 += "  and r.idEfectorDeriva=" + ddlEfector.SelectedValue;
                m_strSQLCondicionEfector2 += "  and E.idefector=  " + ddlEfector.SelectedValue;

            }
       //     Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);


            string m_strSQL = @"  
Select P.fecharegistro as fecha, P.numero , Pa.apellido, Pa.nombre, DP.fechaValida as [Fecha Derivacion], DP.resultadocar as resultadocar , null as   [Fecha Validacion], '' as Validador
from Lab_Protocolo P
inner join sys_paciente Pa on Pa.idpaciente= P.idpaciente
INNER JOIN lAB_DetalleProtocolo DP on DP.idProtocolo = P.idProtocolo
INNER JOIN LAB_iTEM I on i.iditem = dp.idsubitem
inner join LAB_ResultadoItem as R on R.resultado = dp.resultadoCar and  r.idEfectorDeriva>0
WHERE P.baja = 0  " + m_strSQLCondicion + m_strSQLCondicionEfector1 + @"
and I.codigo='" + oC.CodigoCovid + @"'
union


Select  P.fecharegistro as fecha,P.numero , Pa.apellido, Pa.nombre,  A.fecha as [Fecha Derivacion],  DP.resultadocar , dp.fechavalida as   [Fecha Validacion], u.firmavalidacion as Validador
from Lab_Protocolo P
INNER JOIN lAB_DetalleProtocolo DP on DP.idProtocolo = P.idProtocolo
inner join sys_paciente Pa on Pa.idpaciente= P.idpaciente
INNER JOIN LAB_iTEM I on i.iditem = dp.idsubitem
inner join sys_usuario U on U.idusuario= DP.idusuarioValida
inner join sys_efector E on E.idefector=  U.idefector  AND e.IDeFECTOR<>" + oC.IdEfector.IdEfector.ToString()+ @"
inner join LAB_AuditoriaProtocolo as A on A.idprotocolo= P.idprotocolo and A.fecha in (select max (fecha) from LAB_AuditoriaProtocolo where accion ='Valida' and valor like   '%derivada%' and idProtocolo =P.idprotocolo)
WHERE P.baja = 0  " + m_strSQLCondicion + m_strSQLCondicionEfector2 + @"
and I.codigo='" + oC.CodigoCovid + @"'";


          

          

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            lblCantidad.Text= Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            return Ds.Tables[0];
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ExportarExcel( );

        }

        private void ExportarExcel()
        {
            try {
                DataTable tabla = MostrarDatos();
                if (tabla.Rows.Count > 0)
                {
                    Utility.ExportDataTableToXlsx(tabla, "CodVid_Derivaciones_" + DateTime.Now.ToShortDateString() );
                    //StringBuilder sb = new StringBuilder();
                    //StringWriter sw = new StringWriter(sb);
                    //HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //Page pagina = new Page();
                    //HtmlForm form = new HtmlForm();
                    //GridView dg = new GridView();
                    //dg.EnableViewState = false;
                    //dg.DataSource = tabla;

                    //dg.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);


                    //dg.DataBind();
                    //pagina.EnableEventValidation = false;
                    //pagina.DesignerInitialize();
                    //pagina.Controls.Add(form);
                    //form.Controls.Add(dg);
                    //pagina.RenderControl(htw);
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.ContentType = "application/vnd.ms-excel";
                    //Response.AddHeader("Content-Disposition", "attachment;filename=CodVid_Derivaciones_" + DateTime.Now.ToShortDateString() + ".xls");
                    //Response.Charset = "UTF-8";
                    //Response.ContentEncoding = Encoding.Default;
                    //Response.Write(sb.ToString());
                    //Response.End();
                }
            }
            catch
            {

                lblError.Text = "Ha superado el límite para exportar datos. Comuniquese con el administrador.";
                lblError.Visible = true;
            }
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable dtMuestras = MostrarDatos();

            GridResultados.DataSource = dtMuestras;
            GridResultados.DataBind();
            GridResultados.Visible = true;



        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        
        }

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string res = "";
                string v = e.Row.Cells[5].Text;
                e.Row.Cells[5].Font.Bold = true;
                if (v.Length > 10)
                    res = v.Substring(0, 10);
                else res = v;

                //  if ((v == "SE DETECTA GENOMA DE COVID-19") || (v == "SE DETECTA GENOMA DE SARS-COV2"))
                if (res == "SE DETECTA")
                {
                    for (int i = 0; i <= 7; i++)
                    {
                        e.Row.Cells[i].ForeColor = Color.Red;
                        e.Row.Cells[i].Font.Bold = true;
                    }
                }
                else
                {
                    if (res != "PENDIENTE")
                        for (int i = 0; i <= 7; i++)
                        {
                            e.Row.Cells[i].Font.Bold = true;
                        }
                }

            }

        }
    }
  


}