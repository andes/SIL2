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

namespace WebLab
{
    public partial class SeguimientoResultadosSemanal : System.Web.UI.Page
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
                    txtFechaDesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    //CargarListas();
                    DataTable dtMuestras = MostrarDatos();

                    GridResultados.DataSource = dtMuestras;
                    GridResultados.DataBind();
                    GridResultados.Visible = true;
                        
                    


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
        private DataTable MostrarDatos( )
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = "";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);
           

            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                
                    m_strSQLCondicion += " AND dp.fechavalida >= '" + fecha1.ToString("yyyyMMdd") + "'";
              
            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
                 
                    m_strSQLCondicion += " AND dp.fechavalida <= '" + fecha2.ToString("yyyyMMdd") + "'";
               
            }

             
              m_strSQLCondicion += @" and dp.idUsuarioValida>0 AND dp.resultadoCar NOT like '%SIN MUESTRA%' 
             AND dp.resultadoCar NOT like '%MUESTRA DERIVADA%'";
           
                string m_strSQL = @"  select    caracter AS CLASIFICACION, total AS [TOTAL MUESTRAS], positivos AS POSITIVOS
from (
select   
 
     ca.idCaracter as orden,
ca.nombre as [Caracter], 
count (*)  as total, (select        

count (*)  as positivos
 from		
LAB_protocolo as IR 
 left join lab_caracter as Ca1 on Ca1.idCaracter= IR.idcaracter 
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
inner JOIN LAB_ITEM IT ON IT.idItem = DP.idsubitem and IT.codigo='" + oCon.CodigoCovid + @"' 
where  
(IT.codigo= '" + oCon.CodigoCovid + @"' ) and IR.idPaciente>0  " + m_strSQLCondicion + @"
and dp.resultadoCar like 'SE DETECTA%'
and  ir.baja=0  and ca.nombre=ca1.nombre  
 ) as positivos
 

 from		
LAB_protocolo as IR
 
 left join lab_caracter as Ca on Ca.idCaracter= IR.idcaracter
 
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
inner JOIN LAB_ITEM IT ON IT.idItem = DP.idsubitem and IT.codigo='" + oCon.CodigoCovid + @"'  
 
where  
(IT.codigo= '" + oCon.CodigoCovid + @"' )  and IR.idPaciente>0  " + m_strSQLCondicion + @"
and  ir.baja=0 
group by   
ca.nombre,  ca.idCaracter 

union

select   
  9999 AS orden, 'TOTAL DE MUESTRAS' as [Caracter], 
count (*)  as total, (select        

count (*)  as positivos
 from		
LAB_protocolo as IR  
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
inner JOIN LAB_ITEM IT ON IT.idItem = DP.idsubitem and IT.codigo='9122'  
where  
(IT.codigo= '" + oCon.CodigoCovid + @"' ) and IR.idPaciente>0   " + m_strSQLCondicion + @"
and dp.resultadoCar like 'SE DETECTA%'
and  ir.baja=0    
 ) as positivos
 

 from		
LAB_protocolo as IR 
 
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
inner JOIN LAB_ITEM IT ON IT.idItem = DP.idsubitem and IT.codigo='" + oCon.CodigoCovid + @"'  
 
where  
(IT.codigo= '" + oCon.CodigoCovid + @"' )  and IR.idPaciente>0  " + m_strSQLCondicion + @"
and  ir.baja=0 
 ) x

 

  ";


           


          

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
            try {
                DataTable tabla = MostrarDatos();
                if (tabla.Rows.Count > 0)
                {
                    Utility.ExportDataTableToXlsx(tabla, "CodVid_Resumen_" + DateTime.Now.ToShortDateString());
                    //StringBuilder sb = new StringBuilder();
                    //StringWriter sw = new StringWriter(sb);
                    //HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //Page pagina = new Page();
                    //HtmlForm form = new HtmlForm();
                    //GridView dg = new GridView();
                    //dg.EnableViewState = false;
                    //dg.DataSource = tabla;
                    //dg.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound1);
                    //dg.DataBind();
                    //Utility.GenerarColumnasGrid(dg, dg.DataSource as DataTable);

                    //pagina.EnableEventValidation = false;
                    //pagina.DesignerInitialize();
                    //pagina.Controls.Add(form);
                    //form.Controls.Add(dg);
                    //pagina.RenderControl(htw);
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.ContentType = "application/vnd.ms-excel";
                    //Response.AddHeader("Content-Disposition", "attachment;filename=CodVid_Resumen_" + DateTime.Now.ToShortDateString() + ".xls");
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
                string res =  e.Row.Cells[0].Text;
               

                //  if ((v == "SE DETECTA GENOMA DE COVID-19") || (v == "SE DETECTA GENOMA DE SARS-COV2"))
                if (res== "TOTAL DE MUESTRAS")
                    {
                    for (int i = 0; i <= 2; i++)
                    {
                        
                        e.Row.Cells[i].Font.Bold = true;
                    }
                }
               

            }

        }
    }
  


}