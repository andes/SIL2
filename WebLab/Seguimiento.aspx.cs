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
using System.Configuration;

namespace WebLab
{
    public partial class Seguimiento : System.Web.UI.Page
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
                        CargarListas();
                        DataTable dtMuestras = MostrarDatos(oItem);
                        if ((int.Parse(rdbOpcion.SelectedValue) == 1) || (int.Parse(rdbOpcion.SelectedValue) == 2))   //pacientes diferentes
                        {
                            gvPacientes.DataSource = dtMuestras;
                            gvPacientes.DataBind();
                            gvPacientes.Visible = true;
                            GridView1.Visible = false;
                        }
                        else
                        {

                            GridView1.DataSource = dtMuestras;
                            GridView1.DataBind();
                            gvPacientes.Visible = false;
                            GridView1.Visible = true;
                        }
                    }
                }
                else Response.Redirect("FinSesion.aspx", false);
            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "SELECT idCaracter, nombre   FROM LAB_Caracter with (nolock) order by nombre ";
            oUtil.CargarCombo(ddlCaracter, m_ssql, "idCaracter", "nombre", connReady);
            ddlCaracter.Items.Insert(0, new ListItem("Todos", "0"));

            //            m_ssql = @"select distinct  resultadocar as resultado from LAB_detalleprotocolo
            //where iditem in (select iditem from lab_item where codigo = '" + oCon.CodigoCovid + @"' )
            //order by resultadocar";
            //            oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado");
            //            ddlResultado.Items.Insert(0, new ListItem("Todos", "0"));

            //m_ssql = "SELECT idItem, nombre   FROM LAB_Item where codigo in ('9001', '9002','9122') ";
            m_ssql = @" SELECT idItem, nombre   FROM LAB_Item with (nolock)
                        INNER JOIN LAB_Param ON PARSTR = CODIGO 
                        where idParam = 2
                        AND BAJA = 0 
                        order by nombre ";//LAB_Param contiene los codidos para el seguimiento diario
            oUtil.CargarCheckBox (chkItem, m_ssql, "idItem", "nombre", connReady);
            for (int i = 0; i < chkItem.Items.Count; i++)
            {
                chkItem.Items[i].Selected = true;
            }
            CargarResultados();

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
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private DataTable MostrarDatos( Item oItem)
        {
             

                 // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
                string m_strSQLCondicion = " 1=1 ";

            m_strSQLCondicion = " and Ir.idEfector=" + oUser.IdEfector.IdEfector.ToString();
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);
            if (txtFechaDesde.Value != "")
                {
                    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                    m_strSQLCondicion += " AND ir.fecharegistro >= '" + fecha1.ToString("yyyyMMdd") + "'";

                }

                if (txtFechaHasta.Value != "")
                {
                    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
                    m_strSQLCondicion += " AND ir.fecharegistro < '" + fecha2.ToString("yyyyMMdd") + "'";

                }
                if (ddlCaracter.SelectedValue != "0")
                    m_strSQLCondicion += " AND ir.idCaracter=" + ddlCaracter.SelectedValue;

                if ((int.Parse(rdbOpcion.SelectedValue) == 2) || (int.Parse(rdbOpcion.SelectedValue) == 4))  //pacientes positivos
                    m_strSQLCondicion += " and dp.resultadoCar like 'SE DETECT%'  and Dp.idUsuarioValida>0";

                if (ddlResultado.SelectedValue != "0")
                    m_strSQLCondicion += " and dp.resultadoCar='" + ddlResultado.SelectedValue + "'  and Dp.idUsuarioValida>0";


                if (int.Parse(rdbOpcion.SelectedValue) == 3) // pendiente de resultado
                    m_strSQLCondicion += " and dp.idUsuarioValida=0 ";
                if (int.Parse(rdbOpcion.SelectedValue) == 5) // muestra con resultado/procesado
                    m_strSQLCondicion += @" and dp.idUsuarioValida>0 AND dp.resultadoCar NOT like '%SIN MUESTRA%' 
AND dp.resultadoCar NOT like '%MUESTRA DERIVADA%'";

            string m_strSQL = @"  select  ir.fecharegistro as [Fecha Registro], 
IR.numero as [Protocolo], 
IR.numeroOrigen as [Origen],
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
 Pac.informacioncontacto as [Telefono],
substring(O.nombre,1,3) as [Amb/Int.], 
case when convert(varchar(10), IR.fechaTomaMuestra, 103)='01/01/1900' then '' 
else convert(varchar(10), IR.fechaTomaMuestra, 103) end as [F. Toma Muestra],
M.nombre as Muestra, IR.numeroOrigen2,
IR.Especialista AS [Solicitante] ,
dP.fechavalida as [F. Resultado],
case when  Dp.idUsuarioValida>0 then upper(dp.resultadoCar)  else 'EN PROCESO' end   AS 'Resultado' ,
case when  Dp.idUsuarioValida>0 then upper(ltrim(dp.observaciones + ' '+ ir.observacionesResultados)) else '' end as Observaciones
 from		
LAB_protocolo as IR with (nolock)
inner JOIN   Sys_Paciente AS Pac with (nolock) ON IR.idPaciente = Pac.idPaciente 
inner JOIN Lab_Origen O with (nolock) on O.idOrigen= IR.idOrigen
inner JOIN LAB_SectorServicio S with (nolock) on S.idSectorServicio= IR.idSector
INNER JOIN LAB_Muestra as M with (nolock) on M.idMuestra= IR.idMuestra
 left join lab_caracter as Ca with (nolock) on Ca.idCaracter= IR.idcaracter
inner JOIN [SYS_efector] e with (nolock) on  e.idefector = ir.idEfectorSolicitante
inner JOIN LAB_DetalleProtocolo DP with (nolock) ON DP.idProtocolo = IR.idProtocolo
    
where  DP.idsubitem in ( " + GetDeterminaciones() +") "+ m_strSQLCondicion + @"
and  ir.baja=0  
order by ir.numero
";


                if ((int.Parse(rdbOpcion.SelectedValue) == 1) || (int.Parse(rdbOpcion.SelectedValue) == 2))   //pacientes diferentes
                    m_strSQL = @" select [Tipo Doc.],  [Nro. Documento], Apellido,   [Nombre],  [Fecha Nacimiento], [Sexo], count (*) as Cantidad
from (" + m_strSQL + @" AND IR.IDPACIENTE>-1)x
group by  [Tipo Doc.],  [Nro. Documento], Apellido,   [Nombre],
  [Fecha Nacimiento], [Sexo]";


                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
                lblCantidad.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
                return Ds.Tables[0];
            
        }

        private string GetDeterminaciones()
        {
            string lista = "";
            for (int i = 0; i < chkItem.Items.Count; i++)
            {
                if (chkItem.Items[i].Selected)
                {

                    if (lista == "") lista = chkItem.Items[i].Value;
                    else lista += "," + chkItem.Items[i].Value;
                }
                //oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
            }
            return lista;
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
                        if ((int.Parse(rdbOpcion.SelectedValue) == 0) || (int.Parse(rdbOpcion.SelectedValue) >= 3))   //pacientes diferentes
                        {
                            dg.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);
                        }
                        dg.DataBind();
                        pagina.EnableEventValidation = false;
                        pagina.DesignerInitialize();
                        pagina.Controls.Add(form);
                        form.Controls.Add(dg);
                        pagina.RenderControl(htw);
                        Response.Clear();
                        Response.Buffer = true;
                        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", "attachment;filename=Seguimiento_" + DateTime.Now.ToShortDateString() + ".xls");
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

        protected void rdbOpcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Item));
            crit.Add(Expression.Eq("Codigo", oC.CodigoCovid));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Item oItem = (Item)crit.UniqueResult();

            if (oItem != null)
            {
                DataTable dtMuestras = MostrarDatos(oItem);
                if ((int.Parse(rdbOpcion.SelectedValue) == 1) || (int.Parse(rdbOpcion.SelectedValue) == 2))   //pacientes diferentes
                {
                    gvPacientes.DataSource = dtMuestras;
                    gvPacientes.DataBind();
                    gvPacientes.Visible = true;
                    GridView1.Visible = false;
                }
                else
                {

                    GridView1.DataSource = dtMuestras;
                    GridView1.DataBind();
                    gvPacientes.Visible = false;
                    GridView1.Visible = true;
                }

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
                if ((int.Parse(rdbOpcion.SelectedValue) == 1) || (int.Parse(rdbOpcion.SelectedValue) == 2))   //pacientes diferentes
                {
                    gvPacientes.DataSource = dtMuestras;
                    gvPacientes.DataBind();
                    gvPacientes.Visible = true;
                    GridView1.Visible = false;
                }
                else
                {

                    GridView1.DataSource = dtMuestras;
                    GridView1.DataBind();
                    gvPacientes.Visible = false;
                    GridView1.Visible = true;
                }
            }

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
                if (e.Row.RowType == DataControlRowType.DataRow)
            {

                string res = "";
                
              

                string v = e.Row.Cells[21].Text;
                e.Row.Cells[21].Font.Bold = true;

                if (v.Length > 10)
                    res = v.Substring(0,9);
                else res = v;

                if (res== "SE DETECT")
                {
                    for (int i = 0; i <= 22; i++)
                    {
                        e.Row.Cells[i].ForeColor = Color.Red;
                        e.Row.Cells[i].Font.Bold = true;
                    }
                }
                else
                {
                    if (res != "EN PROCESO")
                        for (int i = 0; i <= 22; i++)
                        {
                            e.Row.Cells[i].Font.Bold = true;
                        }
                }

            }
        }

        protected void chkItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarResultados();
        }

        private void CargarResultados()
        {
            Utility oUtil = new Utility();
            string m_ssql = @"select distinct  resultadocar as resultado from LAB_detalleprotocolo with (nolock)
where iditem in (" + GetDeterminaciones() + @")
and idEfector="+ oUser.IdEfector.IdEfector.ToString() +" order by resultadocar";
            oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado");
            ddlResultado.Items.Insert(0, new ListItem("Todos", "0"));
        }
    }
  


}