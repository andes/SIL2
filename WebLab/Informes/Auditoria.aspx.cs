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
using Business.Data;

namespace WebLab.Informes {
    public partial class Auditoria : System.Web.UI.Page {
        CrystalReportSource oCr = new CrystalReportSource();
        Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e) {
            //oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Session["idUsuario"] != null) {
                oUser = (Usuario) oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion) oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            } else
                Response.Redirect("../FinSesion.aspx", false);



        }
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {

                if (Session["idUsuario"] != null) {
                    Inicializar();

                } else
                    Response.Redirect("../FinSesion.aspx", false);

            }
        }
        protected void Page_Unload(object sender, EventArgs e) {
            if (this.oCr.ReportDocument != null) {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private void Inicializar() {
            CargarListas();

            switch (Request["tipo"].ToString()) {
                case "controlAcceso": {
                    lblTitulo.Text = "AUDITORIA DE ACCESOS";
                    txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    pnlControlProtocolo.Visible = false;
                    pnlControlAcceso.Visible = true;
                    pnControlLote.Visible = false;

                }
                break;
                case "controlProtocolo": {
                    ddlTipoServicio.Visible = false;
                    if (oC != null) {
                        if (oC.TipoNumeracionProtocolo == 3)
                            ddlTipoServicio.Visible = true;
                        else
                            ddlTipoServicio.Visible = false;
                    }
                    lblTitulo.Text = "AUDITORIA DE PROTOCOLO";
                    pnlControlProtocolo.Visible = true;
                    pnlControlAcceso.Visible = false;
                    pnControlLote.Visible = false;


                }
                break;

                case "controlLote": {

                    ddlTipoServicio.Visible = false;
                    lblTitulo.Text = "AUDITORIA DE LOTE";
                    pnlControlProtocolo.Visible = false;
                    pnlControlAcceso.Visible = false;
                    pnControlLote.Visible = true;
                }
                break;
            }
        }
        private void CargarListas() {
            Utility oUtil = new Utility();
            ///Carga de combos de tipos de servicios

            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = @"	select idusuario, apellido + ' ' +nombre  as nombre 
                    from sys_usuario u with (nolock)
                    where activo = 1
                    and exists (select 1 from sys_usuarioefector e (nolock) where e.idusuario = u.idusuario and e.idEfector = " + oUser.IdEfector.IdEfector.ToString() + @") order by apellido, nombre";
            if (oUser.Administrador) {
                m_ssql = @"	select idusuario, apellido + ' ' +nombre  as nombre  from sys_usuario u with (nolock) where activo = 1 order by apellido, nombre";
            }
            oUtil.CargarCombo(ddlUsuario, m_ssql, "idusuario", "nombre", connReady);
            oUtil.CargarCombo(ddlUsuario2, m_ssql, "idusuario", "nombre", connReady);
            oUtil.CargarCombo(ddlUsuario3, m_ssql, "idusuario", "nombre", connReady);

            ddlUsuario.Items.Insert(0, new ListItem("--Todos--", "0"));
            ddlUsuario2.Items.Insert(0, new ListItem("--Todos--", "0"));
            ddlUsuario3.Items.Insert(0, new ListItem("--Todos--", "0"));

        }

        #region Protocolo
        protected void btnControlProtocolo_Click(object sender, EventArgs e) {
            if (Page.IsValid) {
               ImprimirAuditoria();
            }
        }

        private void ImprimirAuditoria() {


            DataTable dtAuditoria = GetDataSetAuditoriaProtocolo();


            if (dtAuditoria.Columns.Count > 2) {
                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                if (oC != null) {

                    encabezado1.Value = oC.EncabezadoLinea1;
                    encabezado2.Value = oC.EncabezadoLinea2;
                } else {

                    encabezado1.Value = oUser.IdEfector.Nombre;
                    encabezado2.Value = oUser.IdEfector.Domicilio;
                }
                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = "Auditoria de Protocolo";

                oCr.Report.FileName = "AuditoriaProtocolo.rpt";
                oCr.ReportDocument.SetDataSource(dtAuditoria);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("Auditoria" + txtProtocolo.Text);
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);




            } else {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }

        }

        private DataTable GetDataSetAuditoriaProtocolo() {
            string m_strSQL = "";

            string m_strCondicion = "";

            //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();

            if (!oUser.Administrador) {
                m_strCondicion = " and P.idefector=" + oUser.IdEfector.IdEfector.ToString();
            }

            if (ddlUsuario.SelectedValue != "0")
                m_strCondicion += " and U.idusuario=" + ddlUsuario.SelectedValue;

             m_strSQL = @" SELECT  P.numero  AS numero,isnull(U.apellido,'Automatico')  as username, A.fecha AS fecha, A.hora, A.accion, A.analisis, A.valor, A.valorAnterior
                            FROM         LAB_AuditoriaProtocolo AS A with (nolock)
                             left JOIN Sys_Usuario AS U with (nolock) ON A.idUsuario = U.idUsuario
                            inner join  lab_protocolo P  with (nolock) on P.idprotocolo= A.idprotocolo
                            where  P.numero=" + txtProtocolo.Text.Trim() + m_strCondicion + " ORDER BY A.idAuditoriaProtocolo";

            DataSet Ds1 = new DataSet();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1, "auditoria");


            DataTable data = Ds1.Tables[0];
            return data;


        }

        #endregion

        #region Acceso
        protected void btnControlAcceso_Click(object sender, EventArgs e) {
            if (Page.IsValid) {
                if (rdbTipoAuditoria.SelectedValue == "1")

                    ImprimirAuditoriaAcceso();
                if (rdbTipoAuditoria.SelectedValue == "2") {
                    if (ddlUsuario2.SelectedValue != "0") { ImprimirAuditoriaAcciones(); } else {
                        lblMensaje.Text = "Debe seleccionar un usuario";
                        lblMensaje.Visible = true;
                    }
                }
            }



        }

        private void ImprimirAuditoriaAcciones() {
            DataTable dtAuditoria = GetDataSetAuditoriaAcciones();
            if (dtAuditoria.Columns.Count > 0) {

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();

                if (oC != null) {
                    encabezado1.Value = oC.EncabezadoLinea1;
                    encabezado2.Value = oC.EncabezadoLinea2;
                    encabezado3.Value = oC.EncabezadoLinea3;
                } else {
                    encabezado1.Value = oUser.IdEfector.Nombre;
                    encabezado2.Value = oUser.IdEfector.Domicilio;
                    encabezado3.Value = "";
                }


                oCr.Report.FileName = "AuditoriaAcciones.rpt";
                oCr.ReportDocument.SetDataSource(dtAuditoria);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("AuditoriaAcceso");
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);



            } else {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args) {
            if (txtFechaDesde.Value == "")
                args.IsValid = false;
            else
                if (txtFechaHasta.Value == "")
                args.IsValid = false;
            else
                args.IsValid = true;

        }
        private void ImprimirAuditoriaAcceso() {

            DataTable dtAuditoria = GetDataSetAuditoriaAcceso();
            if (dtAuditoria.Columns.Count > 0) {

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();

                if (oC != null) {
                    encabezado1.Value = oC.EncabezadoLinea1;
                    encabezado2.Value = oC.EncabezadoLinea2;
                    encabezado3.Value = oC.EncabezadoLinea3;
                } else {
                    encabezado1.Value = oUser.IdEfector.Nombre;
                    encabezado2.Value = oUser.IdEfector.Domicilio;
                    encabezado3.Value = "";
                }

                oCr.Report.FileName = "AuditoriaAcceso.rpt";
                oCr.ReportDocument.SetDataSource(dtAuditoria);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("AuditoriaAcceso");
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);



            } else {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }

        }
        private DataTable GetDataSetAuditoriaAcciones() {
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            string m_strCondicion = " Where 1=1 ";
            if (!oUser.Administrador) {
                m_strCondicion += " and P.idEfector=" + oUser.IdEfector.IdEfector.ToString();
            }

            if (txtFechaDesde.Value != "")
                m_strCondicion += " and convert(varchar(10),A.fecha,112)   >= '" + fecha1.ToString("yyyyMMdd") + "'  ";
            if (txtFechaHasta.Value != "")
                m_strCondicion += " AND  convert(varchar(10),A.fecha,112)  <= '" + fecha2.ToString("yyyyMMdd") + "'  ";
            if (ddlUsuario2.SelectedValue != "0")
                m_strCondicion += " and A.idUsuario=" + ddlUsuario2.SelectedValue;
            string m_strSQL = @"  select       U.username  +' ' + U.apellido  +' ' + U.nombre as username ,
                               A.fecha, 
                               A.hora, 
                               A.accion  ,   
                               P.numero,
                               Pac.apellido, Pac.nombre,
                              A.analisis, A.valor,A.valoranterior 
                            from LAB_AuditoriaProtocolo A with (nolock)
                             inner join LAB_Protocolo as P with (nolock) on P.idprotocolo = A.idProtocolo
                             inner join Sys_Paciente as Pac with (nolock) on Pac.idpaciente = P.idPaciente
                             inner join Sys_Usuario as U  with (nolock) on u.idusuario = A.idusuario
                            " + m_strCondicion +
                            " order by A.fecha desc ";


            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds, "auditoria");


            DataTable data = Ds.Tables[0];
            return data;
        }
        private DataTable GetDataSetAuditoriaAcceso() {
            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);

            string m_strCondicion = " Where 1=1 ";


            if (txtFechaDesde.Value != "")
                m_strCondicion += " and convert(varchar(10),fecha,112)   >= '" + fecha1.ToString("yyyyMMdd") + "'  ";
            if (txtFechaHasta.Value != "")
                m_strCondicion += " AND  convert(varchar(10),fecha,112)  <= '" + fecha2.ToString("yyyyMMdd") + "'  ";
            if (ddlUsuario2.SelectedValue != "0")
                m_strCondicion += " and U.idUsuario=" + ddlUsuario2.SelectedValue;
            string m_strSQL = " SELECT   U.apellido + ' ' + U.nombre as username , LA.fecha AS fecha " +
                                " FROM         LAB_LogAcceso AS LA with (nolock) INNER JOIN Sys_Usuario AS U with (nolock) ON LA.idUsuario = U.idUsuario" + m_strCondicion +
                                " ORDER BY LA.IDLOGACCESO ";


            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds, "auditoria");


            DataTable data = Ds.Tables[0];
            return data;
        }

        #endregion

        #region Lote
        private DataTable GetDataSetAuditoraLote() {
            string m_strSQL;
            string m_strCondicion = "";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();

            if (!oUser.Administrador) {
                m_strCondicion = " and L.idEfectorDestino = " + oUser.IdEfector.IdEfector.ToString();
            }

            if (ddlUsuario3.SelectedValue != "0")
                m_strCondicion += " and U.idusuario = " + ddlUsuario.SelectedValue;

            m_strSQL = @" SELECT  L.idLoteDerivacion  AS numero,isnull(U.apellido,'Automatico')  as username, A.fecha AS fecha, A.hora, A.accion, A.analisis, A.valor, A.valorAnterior
                            FROM LAB_AuditoriaLote AS A with (nolock)
                            left JOIN Sys_Usuario AS U with (nolock) ON A.idUsuario = U.idUsuario
                            inner join  LAB_LoteDerivacion L  with (nolock) on L.idLoteDerivacion= A.idLote
                            where  L.idLoteDerivacion = " + txt_lote.Text.Trim() + m_strCondicion + " ORDER BY A.idAuditoriaLote";

            DataSet Ds1 = new DataSet();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1, "auditoria");


            DataTable data = Ds1.Tables[0];
            return data;
        }

        protected void btn_informeLote_Click(object sender, EventArgs e) {
            if (Page.IsValid) {
                ImprimirAuditoriaLote();
            }
        }
        private void ImprimirAuditoriaLote() {
            DataTable dtAuditoria = GetDataSetAuditoraLote();
            if (dtAuditoria.Rows.Count > 0) {
                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                if (oC != null) {
                    encabezado1.Value = oC.EncabezadoLinea1;
                    encabezado2.Value = oC.EncabezadoLinea2;
                } else {

                    encabezado1.Value = oUser.IdEfector.Nombre;
                    encabezado2.Value = oUser.IdEfector.Domicilio;
                }
                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue {
                    Value = "Auditoria de Lote"
                };

                oCr.Report.FileName = "AuditoriaLote.rpt";
                oCr.ReportDocument.SetDataSource(dtAuditoria);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF("Auditoria_Lote_" + txt_lote.Text);
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);




            } else {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de lote ingresado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }
        }

        #endregion
    }
}
