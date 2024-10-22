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
    public partial class SeguimientoTestAntigenos : System.Web.UI.Page
    {
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
            string    m_ssql = "SELECT idCaracter, nombre   FROM LAB_Caracter ";
            oUtil.CargarCombo(ddlCaracter, m_ssql, "idCaracter", "nombre");
            ddlCaracter.Items.Insert(0, new ListItem("Todos", "0"));

            m_ssql = @"select distinct  resultadocar as resultado from LAB_detalleprotocolo
where iditem in (select iditem from lab_item where codigo = '9124')
order by resultadocar";
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
        private DataTable MostrarDatos( )
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = "";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);
           

            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                //if (ddlTipo.SelectedValue == "Validado")
                    m_strSQLCondicion += " AND dp.fechavalida >= '" + fecha1.ToString("yyyyMMdd") + "'";
                //if (ddlTipo.SelectedValue == "Prevalidado")
                //    m_strSQLCondicion += " AND dp.fechaPreValida >= '" + fecha1.ToString("yyyyMMdd") + "'";

            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
                //if (ddlTipo.SelectedValue == "Validado")
                    m_strSQLCondicion += " AND dp.fechavalida < '" + fecha2.ToString("yyyyMMdd") + "'";
                //if (ddlTipo.SelectedValue == "Prevalidado")
                //    m_strSQLCondicion += " AND dp.fechaPrevalida < '" + fecha2.ToString("yyyyMMdd") + "'";

            }


            if (ddlCaracter.SelectedValue != "0")
                m_strSQLCondicion += " AND ir.idCaracter=" + ddlCaracter.SelectedValue;

            //if ((int.Parse(rdbOpcion.SelectedValue) == 2) || (int.Parse(rdbOpcion.SelectedValue) == 4))  //pacientes positivos
            //    m_strSQLCondicion += " and dp.resultadoCar like 'SE DETECTA%'  and Dp.idUsuarioValida>0";

            if (ddlResultado.SelectedValue != "0")
            {
                m_strSQLCondicion += " and dp.resultadoCar='" + ddlResultado.SelectedValue + "'";
              
            }
            //if (ddlTipo.SelectedValue == "Validado")
            //    m_strSQLCondicion += "  and Dp.idUsuarioValida>0";
            //if (ddlTipo.SelectedValue == "Prevalidado")
            //    m_strSQLCondicion += "  and Dp.idUsuarioPreValida>0   and Dp.idUsuarioValida=0";



            //            if (int.Parse(rdbOpcion.SelectedValue) == 3) // pendiente de resultado
            //                m_strSQLCondicion += " and dp.idUsuarioValida=0 ";
            //            if (int.Parse(rdbOpcion.SelectedValue) == 5) // muestra con resultado/procesado
            //                m_strSQLCondicion += @" and dp.idUsuarioValida>0 AND dp.resultadoCar NOT like '%SIN MUESTRA%' 
            //AND dp.resultadoCar NOT like '%MUESTRA DERIVADA%'";
          
                string m_strSQL = @"  select  
ir.fecharegistro as [Fecha Registro], 
IR.numero as [Protocolo], 
IR.numeroOrigen as [Origen], IR.numeroOrigen2 as [Hisopado],
convert(varchar(100),e.nombre) as [Efector Procedencia],   
ca.nombre as [Caracter],
pac.Apellido , 
pac.nombre as [Nombre],
case when ir.idpaciente=-1 then '' else case when pac.idEstado = 2 then 'SIN DNI'  else 'DNI' end end as [Tipo Doc.],
case when ir.idpaciente=-1 then 0 else case when pac.idEstado = 2 then 0 ELSE pac.numeroDocumento END end as [Nro. Documento],
case when ir.idpaciente=-1 then '' else convert(varchar(10),pac.fechaNacimiento,103) end as [Fecha Nacimiento], 
case when ir.idpaciente=-1 then 0 else IR.edad end as [Edad],
case IR.unidadEdad when 0 then 'años' when 1 then 'meses' when 2 then 'días' end as [amd],
case when ir.idpaciente=-1 then '' else IR.sexo end as [Sexo],
IR.nombreObraSocial as [Obra Social],
-- d.ciudad as [Ciudad Domicilio],  d.provincia as [Provincia Domicilio],d.pais as Pais,
substring(O.nombre,1,3) as [Amb/Int.], 
case when convert(varchar(10), IR.fechaTomaMuestra, 103)='01/01/1900' then '' 
else convert(varchar(10), IR.fechaTomaMuestra, 103) end as [F. Toma Muestra],
M.nombre as Muestra,
IR.Especialista AS [Solicitante] ,
dP.fechavalida as [F. Resultado],
case when  Dp.idUsuarioValida>0 then upper(dp.resultadoCar)  else 'PENDIENTE' end   AS 'Test Antigeno CoVid-19' ,
upper(ltrim(dp.observaciones + ' '+ ir.observacionesResultados)) as Observaciones

 from		
LAB_protocolo as IR
inner JOIN   Sys_Paciente AS Pac ON IR.idPaciente = Pac.idPaciente
--left join sys_Pacientedomicilio as D on d.idpaciente = Pac.idpaciente 
left JOIN Lab_Origen O on O.idOrigen= IR.idOrigen
left JOIN LAB_SectorServicio S on S.idSectorServicio= IR.idSector
INNER JOIN LAB_Muestra as M on M.idMuestra= IR.idMuestra
 left join lab_caracter as Ca on Ca.idCaracter= IR.idcaracter
inner JOIN [SYS_efector] e on  e.idefector = ir.idEfectorSolicitante
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = IR.idProtocolo
inner JOIN LAB_ITEM IT ON IT.idItem = DP.iditem and IT.codigo='9124' 
 
where  
(IT.codigo= '9124')   " + m_strSQLCondicion +@"
and  ir.baja=0  ";


          

          

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
                    Response.AddHeader("Content-Disposition", "attachment;filename=testAntigeno_" + DateTime.Now.ToShortDateString() + ".xls");
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;
                    Response.Write(sb.ToString());
                    Response.End();
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
               string v = e.Row.Cells[20].Text;
                e.Row.Cells[20].Font.Bold = true;
                if (v.Length > 10)
                    res = v.Substring(0, 10);
                else res = v;

                //  if ((v == "SE DETECTA GENOMA DE COVID-19") || (v == "SE DETECTA GENOMA DE SARS-COV2"))
                if (res== "REACTIVO")
                    {
                    for (int i = 0; i <= 21; i++)
                    {
                        e.Row.Cells[i].ForeColor = Color.Red;
                        e.Row.Cells[i].Font.Bold = true;
                    }
                }
                else
                {
                    if (res != "NO REACTIVO")
                        for (int i = 0; i <= 21; i++)
                        {
                            e.Row.Cells[i].Font.Bold = true;
                        }
                }

            }

        }
    }
  


}