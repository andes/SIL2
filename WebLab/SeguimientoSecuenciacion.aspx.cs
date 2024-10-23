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
using NHibernate;
using NHibernate.Expression;

namespace WebLab
{
    public partial class SeguimientoSecuenciacion : System.Web.UI.Page
    {
        Configuracion oCon = new Configuracion();



        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Seguimiento");
                if (Session["idUsuario"] != null)
                {

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Item));
                    crit.Add(Expression.Eq("Codigo", "9999"));
                    //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
                    Item oItem = (Item)crit.UniqueResult();

                    if (oItem != null)
                    {

                        txtFechaDesde.Value = DateTime.Now.AddMonths(-1).ToShortDateString();
                        txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                        CargarListas(oItem);
                        CargarGrilla();


                    }

                }
                else Response.Redirect("FinSesion.aspx", false);
            }
        }

        private void CargarListas(Item oItem)
        {
            Utility oUtil = new Utility();
            string    m_ssql = "SELECT idCaracter, nombre   FROM LAB_Caracter ";
            oUtil.CargarCombo(ddlCaracter, m_ssql, "idCaracter", "nombre");
            ddlCaracter.Items.Insert(0, new ListItem("Todos", "0"));

            m_ssql = @"select distinct  resultadocar as resultado from LAB_detalleprotocolo
where idSUBitem= " + oItem.IdItem.ToString() +@" order by resultadocar";
            oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado");
            ddlResultado.Items.Insert(0, new ListItem("Todos", "0"));
             
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
        private DataTable MostrarDatos(Item oItem )
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = " ";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);
           

            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
               
                    m_strSQLCondicion += " and ir.fecharegistro  >= '" + fecha1.ToString("yyyyMMdd") + "'";

            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
          
                    m_strSQLCondicion += " AND ir.fecharegistro  < '" + fecha2.ToString("yyyyMMdd") + "'";
                
            }


            if (ddlCaracter.SelectedValue != "0")
                m_strSQLCondicion += " AND ir.idCaracter=" + ddlCaracter.SelectedValue;

            //if ((int.Parse(rdbOpcion.SelectedValue) == 2) || (int.Parse(rdbOpcion.SelectedValue) == 4))  //pacientes positivos
            //    m_strSQLCondicion += " and dp.resultadoCar like 'SE DETECTA%'  and Dp.idUsuarioValida>0";

            if (ddlResultado.SelectedValue != "0")
            {
                m_strSQLCondicion += " and dp.resultadoCar='" + ddlResultado.SelectedValue + "'";
              
            }


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.CommandText = "[LAB_VigilanciaEpidemio]";

            cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
            cmd.Parameters["@FiltroBusqueda"].Value = m_strSQLCondicion;

            cmd.Parameters.Add("@idItem", SqlDbType.Int);
            cmd.Parameters["@idItem"].Value = oItem.IdItem;

             

            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            lblCantidad.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }
        private DataTable MostrarResumen(Item oItem)
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = " ";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);


            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);

                m_strSQLCondicion += " and ir.fecharegistro  >= '" + fecha1.ToString("yyyyMMdd") + "'";

            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);

                m_strSQLCondicion += " AND ir.fecharegistro  < '" + fecha2.ToString("yyyyMMdd") + "'";

            }


            if (ddlCaracter.SelectedValue != "0")
                m_strSQLCondicion += " AND ir.idCaracter=" + ddlCaracter.SelectedValue;

            //if ((int.Parse(rdbOpcion.SelectedValue) == 2) || (int.Parse(rdbOpcion.SelectedValue) == 4))  //pacientes positivos
            //    m_strSQLCondicion += " and dp.resultadoCar like 'SE DETECTA%'  and Dp.idUsuarioValida>0";

            if (ddlResultado.SelectedValue != "0")            
                m_strSQLCondicion += " and dp.resultadoCar='" + ddlResultado.SelectedValue + "'";

           


            string m_strSQL = @" select  case when dp.resultadoCar='' then 'PENDIENTE' ELSE dp.resultadoCar end as [Linaje de SARS-CoV-2],count (*) as Cantidad 
 from	LAB_protocolo as IR 
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo

where ir.baja=0 and DP.idsubitem=" + oItem.IdItem.ToString()  + m_strSQLCondicion + @"
group by  dp.resultadoCar
order by cantidad  desc ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
           
            return Ds.Tables[0];
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ExportarExcel( );

        }

        private void ExportarExcel()
        {
            try
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Item));
                crit.Add(Expression.Eq("Codigo", "9999"));
                //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
                Item oItem = (Item)crit.UniqueResult();

                if (oItem != null)
                {
                    DataTable tabla = MostrarDatos(oItem);
                    if (tabla.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        StringWriter sw = new StringWriter(sb);
                        HtmlTextWriter htw = new HtmlTextWriter(sw);
                        Page pagina = new Page();
                        HtmlForm form = new HtmlForm();
                        GridView dg = new GridView();
                        dg.EnableViewState = false;
                        dg.DataSource = tabla;

                        dg.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);


                        dg.DataBind();
                        pagina.EnableEventValidation = false;
                        pagina.DesignerInitialize();
                        pagina.Controls.Add(form);
                        form.Controls.Add(dg);
                        pagina.RenderControl(htw);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", "attachment;filename=VigilanciaGenomica_" + DateTime.Now.ToShortDateString() + ".xls");
                        Response.Charset = "UTF-8";
                        Response.ContentEncoding = Encoding.Default;
                        Response.Write(sb.ToString());
                        Response.End();
                    }
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
            CargarGrilla();



        }

        private void CargarGrilla()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Item));
            crit.Add(Expression.Eq("Codigo", "9999"));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Item oItem = (Item)crit.UniqueResult();

            if (oItem != null)
            {
                DataTable dtMuestras = MostrarDatos(oItem);

                GridResultados.DataSource = dtMuestras;
                GridResultados.DataBind();
                GridResultados.Visible = true;


                DataTable dtResumen = MostrarResumen(oItem);

                gvResumen.DataSource = dtResumen;
                gvResumen.DataBind();
                gvResumen.Visible = true;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        
        }

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string res = "";
               string v = e.Row.Cells[2].Text;
                e.Row.Cells[20].Font.Bold = false;
                
               res = v;

              
                    if (res != "")
                        for (int i = 0; i <= 21; i++)
                        {
                            e.Row.Cells[i].Font.Bold = true;
                        }
               

            }

        }
    }
  


}