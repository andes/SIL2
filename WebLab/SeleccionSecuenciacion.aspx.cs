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
    public partial class SeleccionSecuenciacion : System.Web.UI.Page
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

                    

                        txtFechaDesde.Value = DateTime.Now.AddDays(-1).ToShortDateString();
                        txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                        
                        CargarGrilla();


                     

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
        private DataTable MostrarDatos(Item oItem )
        {
            // d.calle as [Calle], d.numero as [Nro.],d.departamento as [Depto], d.cpostal as [CP], d.barrio as Barrio,d.municipio as Municipio,
            string m_strSQLCondicion = " ";
            //DateTime fecha = DateTime.Parse(txtFechaDesde.Value);
           

            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
               
                    m_strSQLCondicion += " and P.fecharegistro  >= '" + fecha1.ToString("yyyyMMdd") + "'";

            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
          
                    m_strSQLCondicion += " AND P.fecharegistro  < '" + fecha2.ToString("yyyyMMdd") + "'";
                
            }


            string m_strSQL = @"  
select convert(varchar(10),P.fecha,103) as [Fecha Registro],P.numero as [Protocolo],
E.nombre as	[Efector Proc.],	pc.apellido as Apellido, pc.nombre as	Nombre, 
case when pc.idestado<>2 then 'DNI' else 'Sin dni' end as [Tipo Doc],
case when pc.idestado<>2 then numeroDocumento else 0 end as NroDoc,
convert(varchar(10),pc.fechanacimiento,103) as	[Fecha Nac.], P.edad as	Edad, p.sexo as	Sexo,
--pc.	Ciudad Domicilio	,
convert(varchar(10),p.fechatomamuestra,103)  as [F.Toma Muestra],	O.nombre as [Int/Amb.],
ca.nombre as 	[Carácter], convert(varchar(10),P.fechainiciosintomas,103)  as	[FIS], convert(varchar(10),p.fechaultimocontacto,103)  as	[FUC],
case when  datediff (dd,p.fechainiciosintomas,p.fechatomamuestra)<=7 then 'CRUDO' ELSE 'MAXWELL' END AS MetodoExtraccion , 
 (select top 1 resultadocar from lab_detalleprotocolo where idProtocolo=P.idprotocolo and (idsubitem =3175 or idsubitem =3183 or idsubitem =3179)) as [FAM (E, N1)],
  (select top 1 resultadocar from lab_detalleprotocolo where idProtocolo=P.idprotocolo and (idsubitem =3177 or idsubitem =3185)) as [QUASAR 670 (N)],
(select top 1 resultadocar from lab_detalleprotocolo where idProtocolo=P.idprotocolo and (idsubitem =3176 or idsubitem =3182))  as [CAL RED (RDRP)]
from 
Lab_protocolo as P
inner join  sys_efector E on E.idefector=P.idefectorsolicitante
inner join sys_paciente as Pc on Pc.idpaciente= P.idpaciente
inner JOIN Lab_Origen O on O.idOrigen= p.idOrigen
 left join lab_caracter as Ca on Ca.idCaracter= p.idcaracter
 
inner JOIN LAB_DetalleProtocolo DP ON DP.idProtocolo = p.idProtocolo
inner JOIN LAB_ITEM IT ON IT.idItem = DP.idsubitem 
 
 where   IT.iditem= " + oItem.IdItem.ToString() + m_strSQLCondicion + @"
and  P.baja=0   and DP.resultadoCar like 'SE DETECTA%' and DP.idusuariovalida>0
order by p.fecharegistro ";






            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            lblCantidad.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
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
                crit.Add(Expression.Eq("Codigo", oCon.CodigoCovid));
                //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
                Item oItem = (Item)crit.UniqueResult();

                if (oItem != null)
                {
                    DataTable tabla = MostrarDatos(oItem);
                    if (tabla.Rows.Count > 0)
                    {
                        Utility.ExportDataTableToXlsx(tabla, "SeleccionSecuenciacion_" + DateTime.Now.ToShortDateString());
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
                        //Response.AddHeader("Content-Disposition", "attachment;filename=SeleccionSecuenciacion_" + DateTime.Now.ToShortDateString() + ".xls");
                        //Response.Charset = "UTF-8";
                        //Response.ContentEncoding = Encoding.Default;
                        //Response.Write(sb.ToString());
                        //Response.End();
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
            crit.Add(Expression.Eq("Codigo", oCon.CodigoCovid));
            //crit.Add(Expression.Eq("IdSector", oProtocoloActual.IdSector));
            Item oItem = (Item)crit.UniqueResult();

            if (oItem != null)
            {
                DataTable dtMuestras = MostrarDatos(oItem);

                gvResumen.DataSource = dtMuestras;
                
                gvResumen.DataBind();
                gvResumen.Visible = true;
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        
        }

        protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    string res = "";
            //   string v = e.Row.Cells[2].Text;
            //    e.Row.Cells[20].Font.Bold = false;
                
            //   res = v;

              
            //        if (res != "")
            //            for (int i = 0; i <= 21; i++)
            //            {
            //                e.Row.Cells[i].Font.Bold = true;
            //            }
               

            //}

        }
    }
  


}