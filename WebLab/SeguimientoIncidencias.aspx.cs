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
    public partial class SeguimientoIncidencias : System.Web.UI.Page
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

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Item));
                    crit.Add(Expression.Eq("Codigo", oC.CodigoCovid));
                    //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
                    Item oItem = (Item)crit.UniqueResult();

                    if (oItem != null)
                    {

                        txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                        txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                             DataTable dtMuestras = MostrarDatos(oItem);

                        GridIncidencias.DataSource = dtMuestras;
                        GridIncidencias.DataBind();
                        GridIncidencias.Visible = true;


                    }

                }
                else Response.Redirect("FinSesion.aspx", false);
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
                if (Permiso!=2)
                {
                  Response.Redirect("AccesoDenegado.aspx", false); 
                    
                }
            }
            else Response.Redirect("FinSesion.aspx", false);
        }
        private DataTable MostrarDatos( Item oItem)
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = "1=1";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                m_strSQLCondicion += " AND  fecha  >= '" + fecha1.ToString("yyyyMMdd") + "'";
                
            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
                m_strSQLCondicion += " AND  fecha  < '" + fecha2.ToString("yyyyMMdd") + "'";

            }
            if (ddlTipo.SelectedValue != "0")
                m_strSQLCondicion += " AND Tipo  like '%" + ddlTipo.SelectedValue+ "%'";
  

            string m_strSQL = @" select  numeroorigen as [Nro. Hisopado] ,  efector as [Efector],  indicencia as [Incidencia],  observaciones,  fecha ,   tipo as [Tipo],  protocolo 
from
(
select   i.numeroorigen , e.nombre as efector, c.nombre as indicencia, i.observaciones, i.fecha ,  'Prerecepcion' as tipo, '' as protocolo 
from LAB_IndicenciaRecepcion as i
inner join sys_efector as e on e.idefector= i.idefectororigen
inner join LAB_DetalleIncidenciaRecepcion as d on i.idIndicenciaRecepcion= d.idIndicenciaRecepcion
inner join LAB_IncidenciaCalidad as C on d.idIncidenciaCalidad= C.idIncidenciaCalidad
where I.idEfector="+oUser.IdEfector.IdEfector.ToString()+@"
union
select  P.numeroorigen2 as numeroorigen, e.nombre as efector, c.nombre as incidencia , p.observacion, P.fecha,'De Protocolo:', P.numero as Protocolo
from lab_protocolo p
inner join LAB_ProtocoloIncidenciaCalidad as i on i.idProtocolo= p.idprotocolo
inner join LAB_IncidenciaCalidad as C on i.idIncidenciaCalidad= C.idIncidenciaCalidad
inner join  sys_efector as e on e.idefector= p.idefectorsolicitante
inner join lab_detalleprotocolo as D on D.idprotocolo= P.idprotocolo 
where  D.idsubitem =" + oItem.IdItem.ToString() + @"  
 and P.idEfector=" + oUser.IdEfector.IdEfector.ToString() + @"
)x
where " + m_strSQLCondicion+" order by fecha";

           
               

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
            try
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Item));
                crit.Add(Expression.Eq("Codigo", oC.CodigoCovid));
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
                        Response.AddHeader("Content-Disposition", "attachment;filename=CodVid_Incidencias_" + DateTime.Now.ToShortDateString() + ".xls");
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

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Item));
            crit.Add(Expression.Eq("Codigo", oC.CodigoCovid));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Item oItem = (Item)crit.UniqueResult();

            if (oItem != null)
            {

                DataTable dtMuestras = MostrarDatos(oItem);

                GridIncidencias.DataSource = dtMuestras;
                GridIncidencias.DataBind();
                GridIncidencias.Visible = true;


            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        
        }

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{

            //    string v = e.Row.Cells[22].Text;
            //    e.Row.Cells[22].Font.Bold = true;


            //    if (v == "SE DETECTA GENOMA DE COVID-19")
            //    {
            //        for (int i = 0; i <= 23; i++)
            //        {
            //            e.Row.Cells[i].ForeColor = Color.Red;
            //            e.Row.Cells[i].Font.Bold = true;
            //        }
            //    }
            //    else
            //    {
            //        if (v != "PENDIENTE")
            //            for (int i = 0; i <= 23; i++)
            //            {
            //                e.Row.Cells[i].Font.Bold = true;
            //            }
            //    }

            //}

        }



        //
    }
  


}