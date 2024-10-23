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
using CrystalDecisions.Shared;
using Business;
using CrystalDecisions.Web;
using Business.Data.Laboratorio;
using System.IO;
using System.Data.SqlClient;
using System.Text;

namespace WebLab.Informes
{
    public partial class AuditoriaUsuario : System.Web.UI.Page
    {
        CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            //oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Session["idUsuario"] != null)
                {
                    Inicializar();

                }
                else
                    Response.Redirect("../FinSesion.aspx", false);

            }
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private void Inicializar()
        {
            CargarListas();

          
                    lblTitulo.Text = "AUDITORIA DE ABM DE USUARIOS";
                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios
            string m_ssql = "select idusuario, apellido + ' ' +nombre  as nombre from sys_usuario   order by apellido, nombre";
            oUtil.CargarCombo(ddlUsuarioModificado, m_ssql, "idusuario", "nombre");
            oUtil.CargarCombo(ddlUsuarioABM, m_ssql, "idusuario", "nombre");

            ddlUsuarioModificado.Items.Insert(0, new ListItem("--Todos--", "0"));
            ddlUsuarioABM.Items.Insert(0, new ListItem("--Todos--", "0"));

           
        }

        protected void btnControlProtocolo_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                ImprimirAuditoria();
         

        }

        private void ImprimirAuditoria()
        {

            DataTable dtAuditoria = GetDataSetAuditoria();
              if (dtAuditoria.Columns.Count > 2)
            {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            
            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;


            oCr.Report.FileName = "AuditoriaUsuarioABM.rpt";
            oCr.ReportDocument.SetDataSource(dtAuditoria);
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();

                //oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Auditoria" + txtProtocolo.Text);

 

            }
              else
              {
                  string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                  Page.RegisterStartupScript("PopupScript", popupScript);
              }

        }

        private DataTable GetDataSetAuditoria()
        {
            string m_strSQL = "";

                string m_strCondicion = " 1 = 1 ";

            //  SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            

            if (ddlUsuarioABM.SelectedValue != "0")
                    m_strCondicion = " and A.idUsuarioRegistro=" + ddlUsuarioABM.SelectedValue;
            if (ddlUsuarioABM.SelectedValue != "0")
                m_strCondicion = m_strCondicion+ " and A.idUsuario=" + ddlUsuarioModificado.SelectedValue;


            m_strSQL = @" 
SELECT U.apellido usuarioABM,  A.fecha AS fecha, A.hora, A.accion, A.username as usuariomodificado, U1.apellido as apellidomodificado
    FROM         lab_AuditoriaUsuario AS A   
     inner JOIN Sys_Usuario AS U ON A.idUsuarioRegistro = U.idUsuario
    inner JOIN Sys_Usuario AS U1 ON A.idUsuario = U1.idUsuario
    where "+ m_strCondicion+"  order by A.idAuditoriausuario";
                
                DataSet Ds1 = new DataSet();            
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds1, "auditoria");


                DataTable data = Ds1.Tables[0];
                return data;
             
            
        }


      


        protected void btnControlAcceso_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                

                ImprimirAuditoria();
               
            }

         

        }

   
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "") args.IsValid = false;
                else args.IsValid = true;

        }
//        private void ImprimirAuditoriaAcceso()
//        {

//            DataTable dtAuditoria = GetDataSetAuditoriaAcceso();
//            if (dtAuditoria.Columns.Count > 0)
//            {
//                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

//                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
//                encabezado1.Value = oCon.EncabezadoLinea1;

//                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
//                encabezado2.Value = oCon.EncabezadoLinea2;

//                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
//                encabezado3.Value = oCon.EncabezadoLinea3;


//                oCr.Report.FileName = "AuditoriaAcceso.rpt";
//                oCr.ReportDocument.SetDataSource(dtAuditoria);
//                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
//                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
//                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
//                oCr.DataBind();


//                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "AuditoriaAcceso");

          

//            }
//            else
//            {
//                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
//                Page.RegisterStartupScript("PopupScript", popupScript);
//            }

//        }
//        private DataTable GetDataSetAuditoriaAcciones()
//        {
//            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
//            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

//            string m_strCondicion = " Where 1=1 ";
//            if (txtFechaDesde.Value != "") m_strCondicion += " and convert(varchar(10),A.fecha,112)   >= '" + fecha1.ToString("yyyyMMdd") + "'  ";
//            if (txtFechaHasta.Value != "") m_strCondicion += " AND  convert(varchar(10),A.fecha,112)  <= '" + fecha2.ToString("yyyyMMdd") + "'  ";
//            if (ddlUsuarioABM.SelectedValue != "0")
//                m_strCondicion += " and A.idUsuario=" + ddlUsuarioABM.SelectedValue;
//            string m_strSQL = @" select U.username + ' ' + U.apellido + ' ' + U.nombre as username, A.fecha, A.hora, A.accion,   P.numero, Pac.apellido, Pac.nombre, A.analisis, A.valor,A.valoranterior 
//from LAB_AuditoriaProtocolo  as A
//inner join LAB_Protocolo as P on P.idprotocolo = A.idProtocolo
//inner join Sys_Paciente as Pac on Pac.idpaciente = P.idPaciente
//inner join Sys_Usuario as U on u.idusuario = A.idusuario
//" + m_strCondicion +
//" order by A.fecha desc ";


//            DataSet Ds = new DataSet();
//            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
//            adapter.Fill(Ds, "auditoria");


//            DataTable data = Ds.Tables[0];
//            return data;
//        }
//        private DataTable GetDataSetAuditoriaAcceso()
//        {  
//            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
//            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

//            string m_strCondicion = " Where 1=1 ";
//            if (txtFechaDesde.Value != "") m_strCondicion += " and convert(varchar(10),fecha,112)   >= '" + fecha1.ToString("yyyyMMdd") + "'  ";
//            if (txtFechaHasta.Value != "") m_strCondicion += " AND  convert(varchar(10),fecha,112)  <= '" + fecha2.ToString("yyyyMMdd") + "'  ";
//            if (ddlUsuarioABM.SelectedValue != "0")
//                m_strCondicion += " and U.idUsuario=" + ddlUsuarioABM.SelectedValue;
//            string m_strSQL = " SELECT   U.apellido + ' ' + U.nombre as username , LA.fecha AS fecha " +
//" FROM         LAB_LogAcceso AS LA INNER JOIN Sys_Usuario AS U ON LA.idUsuario = U.idUsuario" + m_strCondicion +
//" ORDER BY LA.IDLOGACCESO ";


//            DataSet Ds = new DataSet();
//            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
//            adapter.Fill(Ds, "auditoria");


//            DataTable data = Ds.Tables[0];
//            return data;
//        }
    }
}
