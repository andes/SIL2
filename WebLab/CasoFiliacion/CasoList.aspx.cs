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
using System.Data.SqlClient;
using Business.Data.Laboratorio;
using Business;
using System.Drawing;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Business.Data;
using CrystalDecisions.Web;
//using System.Web.UI.WebControls;

namespace WebLab.CasoFiliacion
{
    public partial class CasoList : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            HttpContext Context;
                Context = HttpContext.Current;

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

                
                if (Context.Items.Contains("idServicio"))
                {


                    idServicio.Value = Context.Items["idServicio"].ToString();


                   
                    if (Session["idUsuario"] != null)
                    {
                        if (Request["Nombre"] != null) txtNombre.Text = Request["Nombre"].ToString();

                        if (Request["Orden"] != null) ddlTipo.SelectedValue = Request["Orden"].ToString();
                        txtFechaDesde.Value = DateTime.Now.AddDays(-60).ToShortDateString();
                        txtFechaHasta.Value = DateTime.Now.AddDays(1).ToShortDateString();

                        if (Context.Items["idServicio"].ToString() == "3")
                            {
                            btnHojaTrabajo.Visible = false;
                            lbltitulo.Text = "CASOS HISTOCOMPATIBILIDAD";
                            lblTipoForense.Visible = false;
                            ddlTipoCaso.Visible = false;
                                VerificaPermisos("Casos Histocompatibilidad");

                                CargarGrillaMedula();  btnNuevo.Visible = true;
                            }
                       
                            else
                            {
                            btnHojaTrabajo.Visible = true;
                            lbltitulo.Text = "CASOS FORENSE-FILIACION";
                                lblTipoForense.Visible = true;
                                ddlTipoCaso.Visible = true;
                               
                            VerificaPermisos("Casos Forense");
                            
                                CargarGrilla();btnNuevo.Visible = false;
                            }
                        ////oculto columnas segun permisos
                        gvLista.Columns[5].Visible = VerificaPermisosObjeto("Modificar Caso");
                        gvLista.Columns[7].Visible = VerificaPermisosObjeto("Imprimir Resultado");

                        gvLista.Columns[8].Visible = VerificaPermisosObjeto("Auditoria Caso");

                        gvLista.Columns[9].Visible = VerificaPermisosObjeto("Carga Resultados");
                        gvLista.Columns[10].Visible = VerificaPermisosObjeto("Valida Resultados");
                         

                        ////

                    }
                    else Response.Redirect("../FinSesion.aspx", false);
                }

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
        private string getEstado(int v)
        {
            string m_strSQL="";
            if (idServicio.Value == "3")            
                m_strSQL = @"select count(*) as cantidad, 'Terminado' as estado from LAB_CasoFiliacion where idTipoCaso = 0 ";            
            else
            {
                m_strSQL = @"select count(*) as cantidad, 'Terminado' as estado from LAB_CasoFiliacion where 1=1 ";


                if (ddlTipoCaso.SelectedValue != "-1")
                    m_strSQL += " and idTipoCaso =  " + ddlTipoCaso.SelectedValue;//tipo filiacion o forense
                else
                    m_strSQL += " and idTipoCaso >0  ";
            }
            switch (v)
                    { case 0:   m_strSQL += " and baja=0 and idUsuarioValida=0 and idUsuarioCarga=0"; break;
                    case 1: m_strSQL += " and baja=0 and idUsuarioValida=0 and idUsuarioCarga>0"; break;
                    case 2: m_strSQL += " and baja=0 and idUsuarioValida>0"; break;
                }
           



            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            if (Ds.Tables[0].Rows.Count > 0)

                return Ds.Tables[0].Rows[0][0].ToString();
            else
                return "0";
        }

        private void CargarGrillaMedula()
        {
            if (idServicio.Value=="3")            gvLista.HeaderStyle.BackColor = Color.Gray;

            gvLista.DataSource = LeerDatos("Extendido");
            gvLista.DataBind();
            PonerContadores();
        }

        private void PonerContadores()
        {
            lblNoProcesado.Text = getEstado(0);
            lblEnProceso.Text = getEstado(1);
            lblTerminado.Text = getEstado(2);
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
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnNuevo.Visible = false;
                            gvLista.Columns[5].Visible = false;
                        }
                        break;
                    case 2: btnNuevo.Visible = true; break;

                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

        private void CargarGrilla()
        {           
          
            gvLista.DataSource = LeerDatos("Extendido");
            gvLista.DataBind();
             PonerContadores();
           
        }


   

        private object LeerDatos(string tipo)
        {
            string m_condicion = " 1=1 ";
            string m_condicionPersona = "";
           
          
            if (txtNumero.Text != "") m_condicion += " and IDCASOFILIACION =" + txtNumero.Text.Trim();
            if (txtNombre.Text != "") m_condicion += " and nombre like '%" + txtNombre.Text + "%'";
            if (txtDU.Text != "") m_condicionPersona += " and Pac.numerodocumento=" + txtDU.Text + " and Pac.idestado in (1,3)" ;

            string m_strSQL ="";
            string m_orden = "";
            if (ddlTipo.SelectedValue == "0") m_orden = " ORDER BY IDCASOFILIACION desc";
            if (ddlTipo.SelectedValue == "1") m_orden = " ORDER BY IDCASOFILIACION asc";
            if (ddlTipo.SelectedValue == "2") m_orden = " ORDER BY dateadd(dd,30,fecharegistro ) ";
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                m_condicion += " AND fechaRegistro>= '" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                m_condicion += " AND fechaRegistro<= '" + fecha2.ToString("yyyyMMdd") + "'";
            }


            if (ddlEstado.SelectedValue == "-1")
                m_condicion += " and baja = 0 "; // solo los activos
            if (ddlEstado.SelectedValue == "0")
                m_condicion += " and idusuariocarga= 0  and idusuariovalida=0 and baja = 0"; // no procesado

            if (ddlEstado.SelectedValue == "1")
                m_condicion += " and idusuariocarga> 0  and idusuariovalida=0 and baja = 0"; // en proces o

            if (ddlEstado.SelectedValue == "2")
                m_condicion += " and idusuariovalida>0 and baja = 0"; //Terminado
            if (ddlEstado.SelectedValue == "3")
                m_condicion += " and baja = 1 "; // eliminados

            if (ddlTipoCaso.SelectedValue != "-1")
                m_condicion += " and idTipoCaso =  " + ddlTipoCaso.SelectedValue;//tipo filiacion o forense

             if (idServicio.Value=="3") // histo
            {
                if (m_condicionPersona != "")
                { 
                    m_strSQL = @" select *,case idtipocaso when 0 then 'Histocompatibilidad' when 1 then 'Filiacion' when 2 then 'Forense' when 3 then 'Quimerismo' end as tipo ,
dateadd(day, 30,fechaRegistro) as fechavencimiento then FROM lab_casofiliacion where  idTipoCaso=0 and idcasofiliacion in
(select idcasofiliacion from LAB_CasoFiliacionProtocolo C
inner join LAB_Protocolo P on C.idprotocolo = P.idprotocolo 
inner join Sys_Paciente Pac on Pac.idPaciente= P.idPaciente
where P.idEfector="+ oUser.IdEfector.IdEfector.ToString() +" and  P.idtiposervicio = " + idServicio.Value + m_condicionPersona + ") and " + m_condicion + m_orden;
                }
                else
                    m_strSQL = @" select *, case idtipocaso when 0 then 'Histocompatibilidad' when 1 then 'Filiacion' when 2 then 'Forense' when 3 then 'Quimerismo' end as tipo ,
dateadd(day, 30,fechaRegistro) as fechavencimiento  FROM lab_casofiliacion where idTipoCaso=0 and  " + m_condicion + m_orden;
            }
                else
            { 
            if (m_condicionPersona != "")
            m_strSQL = @" select *, case idtipocaso when 0 then 'Histocompatibilidad' when 1 then 'Filiacion' when 2 then 'Forense' when 3 then 'Quimerismo' end as tipo ,
dateadd(day, 30,fechaRegistro) as fechavencimiento  FROM lab_casofiliacion where  idTipoCaso>0 and idcasofiliacion in
(select idcasofiliacion from LAB_CasoFiliacionProtocolo C
inner join LAB_Protocolo P on C.idprotocolo = P.idprotocolo 
inner join Sys_Paciente Pac on Pac.idPaciente= P.idPaciente
where P.idEfector=" + oUser.IdEfector.IdEfector.ToString() + " and  P.idtiposervicio = " + idServicio.Value + m_condicionPersona + ") and " + m_condicion + m_orden;
            else
                m_strSQL = @" select *, 
case idtipocaso when 0 then 'Histocompatibilidad' when 1 then 'Filiacion' when 2 then 'Forense' when 3 then 'Quimerismo' end as tipo ,
dateadd(day, 30,fechaRegistro) as fechavencimiento
FROM lab_casofiliacion where  idTipoCaso>0 and  " + m_condicion + m_orden;
            }

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

             
             
            return Ds.Tables[0];
        }


        private DataTable LeerDatosHojaTrabajo()
        {
            string m_condicion = " 1=1 ";
            string m_condicionPersona = "";
           
            if (txtNumero.Text != "") m_condicion += " and C.IDCASOFILIACION =" + txtNumero.Text.Trim();
            if (txtNombre.Text != "") m_condicion += " and C.nombre like '%" + txtNombre.Text + "%'";
            if (txtDU.Text != "") m_condicionPersona += " and Pac.numerodocumento=" + txtDU.Text + " and Pac.idestado in (1,3)";

            string m_strSQL = "";
            string m_orden = "";
            if (ddlTipo.SelectedValue == "0") m_orden = " ORDER BY C.IDCASOFILIACION desc";
            if (ddlTipo.SelectedValue == "1") m_orden = " ORDER BY C.IDCASOFILIACION asc";
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                m_condicion += " AND C.fechaRegistro>= '" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                m_condicion += " AND C.fechaRegistro<= '" + fecha2.ToString("yyyyMMdd") + "'";
            }


            if (ddlEstado.SelectedValue == "-1")
                m_condicion += " and C.baja = 0 "; // solo los activos
            if (ddlEstado.SelectedValue == "0")
                m_condicion += " and C.idusuariocarga= 0  and C.idusuariovalida=0 and C.baja = 0"; // no procesado

            if (ddlEstado.SelectedValue == "1")
                m_condicion += " and C.idusuariocarga> 0  and C.idusuariovalida=0 and C.baja = 0"; // en proces o

            if (ddlEstado.SelectedValue == "2")
                m_condicion += " and C.idusuariovalida>0 and C.baja = 0"; //Terminado
            if (ddlEstado.SelectedValue == "3")
                m_condicion += " and C.baja = 1 "; // eliminados

            if (ddlTipoCaso.SelectedValue != "-1")
                m_condicion += " and c.idTipoCaso =  " + ddlTipoCaso.SelectedValue;//tipo filiacion o forense

         
                    m_strSQL = @"select C.idCasoFiliacion as numero, C.fechacarga as fecha, C.nombre as titulo, P.numero as codigo ,
   case when C.idtipoCaso = 2 then
     M.nombre +  ' '  +  P.descripcionProducto + '' +  case when Pa.idPaciente = -1 then '' else ' de ' + Pa.apellido + ' ' + Pa.nombre end
	else
    Pa.apellido + ' ' + Pa.nombre
    end
     as determinacion
from lab_protocolo P inner join
LAB_CasoFiliacionProtocolo as CP on CP.idProtocolo = P.idProtocolo inner join 
LAB_CasoFiliacion as C on C.idCasoFiliacion = CP.idCasoFiliacion  left JOIN 
Sys_Paciente Pa on Pa.idPaciente = P.idPaciente inner join
lab_muestra as M  on M.idmuestra= P.idmuestra
where P.idEfector=" + oUser.IdEfector.IdEfector.ToString() + " and idTipoCaso>0 and  " + m_condicion + m_orden;
             

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);




            return Ds.Tables[0];
        }




        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            string m_parametroFiltro=  "&Nombre=" + txtNombre.Text ;
           

            if (e.CommandName != "Page")
            {
               
                switch (e.CommandName)
                {
                    case "Modificar":
                        RedireccionarsegunTipo(e.CommandArgument.ToString(), m_parametroFiltro);break;
                    case "Prefactura":
                        {

                            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(e.CommandArgument.ToString()));
                            if (oCaso.IdTipoCaso == 2)
                                Response.Redirect("FacturacionForense/PrefacturaEdit.aspx?idCaso=" + e.CommandArgument.ToString()+"&Desde=CasoList");
                            if (oCaso.IdTipoCaso == 1)

                                Response.Redirect("FacturacionForense/FacturaFiliacionEdit.aspx?idCaso=" + e.CommandArgument.ToString() + "&Desde=CasoList");


                            
                        }
                        break;

                        

                    case "Factura":
                        {
                            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(e.CommandArgument.ToString()));
                            if (oCaso.IdTipoCaso == 2)
                                Response.Redirect("FacturacionForense/FacturaEdit.aspx?idCaso=" + e.CommandArgument.ToString() + "&Desde=CasoList");
                            if (oCaso.IdTipoCaso == 1)

                                Response.Redirect("FacturacionForense/FacturaFiliacionEdit.aspx?idCaso=" + e.CommandArgument.ToString() + "&Desde=CasoList");
                        }
                        break;

                    case "Resultados":
                        Imprimir(e.CommandArgument.ToString()); break;
                    case "Caratula":
                        ImprimirCaratula(e.CommandArgument.ToString()); break;
                    //    break;
                    case "Auditoria":
                        ImprimirAuditoria(e.CommandArgument.ToString()); break;
                    //    Response.Redirect("ItemDiagramacion.aspx?id=" + e.CommandArgument);

                    case "Carga":
                        //if (Request["idServicio"].ToString()=="6")
                        //Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + e.CommandArgument.ToString() + "&Desde=Carga", false);
                        //else
                        //    Response.Redirect("CasoResultadoHisto.aspx?idServicio=3&id=" + e.CommandArgument.ToString() + "&Desde=Carga", false);

                        Context.Items.Add("logIn", "0");

                        Context.Items.Add("id", e.CommandArgument.ToString());
                        Context.Items.Add("Desde", "Carga");
                        if (idServicio.Value == "6")
                        {
                            Server.Transfer("CasoResultado3.aspx");
                        }

                        if (idServicio.Value == "3")
                        {
                            Server.Transfer("CasoResultadoHisto.aspx");
                        }
                            break;
                    //    break;fili
                    case "Valida":
                        //if (Request["idServicio"].ToString() == "6")
                        //    Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + e.CommandArgument.ToString()+"&Desde=Valida", false);
                        //else
                        //    Response.Redirect("CasoResultadoHisto.aspx?idServicio=3&id=" + e.CommandArgument.ToString() + "&Desde=Valida", false);
                        Context.Items.Add("id", e.CommandArgument.ToString());
                        Context.Items.Add("Desde", "Valida");
                        if (idServicio.Value == "6")

                        {
                            Server.Transfer("CasoResultado3.aspx?");
                        }

                        if (idServicio.Value == "3")
                        {
                            Server.Transfer("CasoResultadoHisto.aspx");
                        }


                        break;
                    //case "Recomendacion":
                    //    Response.Redirect("ItemRecomendaciones.aspx?id=" + e.CommandArgument);
                    //    break;
                    case "Eliminar":
                        { 
                        Eliminar(e.CommandArgument);
                            if (Context.Items["idServicio"].ToString() == "3")
                                CargarGrillaMedula();
                            else
                                CargarGrilla();
                        }
                        break;

                    case "BaseFA":
                        //if (Request["idServicio"].ToString() == "6")
                        //    Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + e.CommandArgument.ToString()+"&Desde=Valida", false);
                        //else
                        //    Response.Redirect("CasoResultadoHisto.aspx?idServicio=3&id=" + e.CommandArgument.ToString() + "&Desde=Valida", false);
                        Context.Items.Add("id", e.CommandArgument.ToString());
                       
                        if (idServicio.Value == "6")

                        {
                            Server.Transfer("GenMarcadoresEdit.aspx?");
                        }
                         


                        break;
                }
            }
        }

        private void RedireccionarsegunTipo(string v, string m_parametroFiltro)
        {
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(v));

            if (idServicio.Value == "6")
            {
                if (oRegistro.IdTipoCaso == 1) /// filiacion
                {
                    Context.Items.Add("idServicio", idServicio.Value);
                    Context.Items.Add("id", v);
                    Context.Items.Add("parametros", m_parametroFiltro);
                    Server.Transfer("CasoEdit.aspx");
                }
                else // forense o quimerismo
                {
                    //string m_parametroFiltro = "&Nombre=" + txtNombre.Text;
                    Context.Items.Add("idServicio", idServicio.Value);
                    Context.Items.Add("id", v);
                    Context.Items.Add("parametros", m_parametroFiltro);
                    Server.Transfer("CasoForenseView.aspx");
                }
            }
            if (idServicio.Value == "3") // histo
            {
                Context.Items.Add("idServicio", idServicio.Value);
                Context.Items.Add("id", v);
                Context.Items.Add("parametros", m_parametroFiltro);
                Server.Transfer("CasoEdit.aspx");
            }
        }

        private void ImprimirCaratula(string v)
        {
            CrystalReportSource oCrCaratula = new CrystalReportSource();
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(v));

            if (oRegistro != null)
            {
                ParameterDiscreteValue nrocaso = new ParameterDiscreteValue();
                nrocaso.Value = oRegistro.IdCasoFiliacion.ToString();

                ParameterDiscreteValue nombre = new ParameterDiscreteValue();
                nombre.Value = oRegistro.Nombre;


                oCrCaratula.Report.FileName = "CaratulaFiliacion.rpt";

                oCrCaratula.ReportDocument.ParameterFields[0].CurrentValues.Add(nrocaso);
                oCrCaratula.ReportDocument.ParameterFields[1].CurrentValues.Add(nombre);


                oCrCaratula.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Caratula");
            }
        }

        private DataTable GetDataSetAuditoria(string sidCaso)
        {


            
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
        


            string m_strSQL = "SELECT "+ sidCaso + " AS numero,U.apellido  as username, A.fecha AS fecha, A.hora, '' as analisis, A.accion,  A.valor, A.valorAnterior  FROM LAB_AuditoriaCasoFiliacion AS A INNER JOIN Sys_Usuario AS U ON A.idUsuario = U.idUsuario    where A.idCasoFiliacion = "+ sidCaso+" ORDER BY A.idAuditoriaCasoFiliacion";

                DataSet Ds1 = new DataSet();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds1, "auditoria");


                DataTable data = Ds1.Tables[0];
                return data;
          


        }

        private void ImprimirAuditoria(string v)
        {
            DataTable dtAuditoria = GetDataSetAuditoria(v);
            if (dtAuditoria.Columns.Count > 2)
            {
              
                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oC.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oC.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oC.EncabezadoLinea3;


                oCr.Report.FileName = "..\\Informes\\AuditoriaCasoFiliacion.rpt";
                oCr.ReportDocument.SetDataSource(dtAuditoria);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Auditoria_Caso" +v);



            }
            else
            {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }

        }

        private void Imprimir(string id)
        {


            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id));
       
            CrystalReportSource oCr = new CrystalReportSource();

            if (idServicio.Value == "6")
            {
                

            switch (oRegistro.IdTipoCaso )
            {
                case 1:
            {
                oCr.Report.FileName = "ResultadoFiliacion.rpt";
                oCr.ReportDocument.SetDataSource(oRegistro.getResultado("Final"));
                oCr.ReportDocument.Subreports[0].SetDataSource(oRegistro.getMarcadores());
            }
                    break;
                case 2:
            {
                oCr.Report.FileName = "ResultadoForense.rpt";
                oCr.ReportDocument.SetDataSource(oRegistro.getResultadoForense());
                //oCr.ReportDocument.Subreports[0].SetDataSource(oRegistro.getMarcadores());
            }
                    break;
                case 3:
                    {
                        oCr.Report.FileName = "ResultadoQuimerismo.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getResultado("Final"));
                        oCr.ReportDocument.Subreports[0].SetDataSource(oRegistro.getMarcadores());
                    }
                    break;

            }
           


                
            }
            else
            { 
                oCr.Report.FileName = "ResultadoHisto.rpt";
            oCr.ReportDocument.SetDataSource(oRegistro.getResultadoHLA(oC.IdHistocompatibilidad));
            }

            oCr.DataBind();

            oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuario"].ToString()),"");

         

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Resultado_" + oRegistro.Nombre.Trim()  );


        }
        private void Eliminar(object idItem)
        { 


           // Usuario oUser = new Usuario();
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(idItem.ToString()));
           
            if (oRegistro.IdUsuarioCarga==0)
            { 
            oRegistro.Baja = true;
                oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();

            oRegistro.GrabarAuditoria("Elimina Caso",oUser.IdUsuario,"");
            }
            else
                estatus.Text = "No es posible eliminar. Tiene registros vinculados";
        }

       

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            Session.Contents.Remove("idUsuarioValida");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString()));


                LinkButton CmdModificar = (LinkButton)e.Row.Cells[5].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modificar";

              

                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[6].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";


               
                LinkButton CmdResultados = (LinkButton)e.Row.Cells[7].Controls[1];
                CmdResultados.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdResultados.CommandName = "Resultados";
                CmdResultados.ToolTip = "Resultados";
          


                LinkButton CmdAuditoria = (LinkButton)e.Row.Cells[8].Controls[1];
                CmdAuditoria.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdAuditoria.CommandName = "Auditoria";
                CmdAuditoria.ToolTip = "Auditoria";
               

                LinkButton CmdCarga = (LinkButton)e.Row.Cells[9].Controls[1];
                CmdCarga.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdCarga.CommandName = "Carga";
                CmdCarga.ToolTip = "Carga";
               
                LinkButton CmdValida = (LinkButton)e.Row.Cells[10].Controls[1];
                CmdValida.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdValida.CommandName = "Valida";
                CmdValida.ToolTip = "Valida";

              
                LinkButton CmdCaratula = (LinkButton)e.Row.Cells[11].Controls[1];
                CmdCaratula.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdCaratula.CommandName = "Caratula";
                CmdCaratula.ToolTip = "Caratula";
                e.Row.Cells[0].Font.Bold = true;

                LinkButton CmdPrefactura = (LinkButton)e.Row.Cells[12].Controls[1];
                CmdPrefactura.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdPrefactura.CommandName = "Prefactura";
                CmdPrefactura.ToolTip = "Prefactura";

              


                //LinkButton CmdBaseFA = (LinkButton)e.Row.Cells[10].Controls[1];
                //CmdBaseFA.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                //CmdBaseFA.CommandName = "BaseFA";
                //CmdBaseFA.ToolTip = "BaseFA";

                if (idServicio.Value == "3")
                {
                    //CmdBaseFA.Visible = false;
                    CmdCaratula.Visible = false;
                }

                    if (Permiso == 1)
                {
                    //CmdBaseFA.Visible = false;
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                   
                }

                if (oRegistro.IdTipoCaso == 3) //si es quimerismo no saca caratula
                    CmdCaratula.Visible = false;

                if (oRegistro.IdTipoCaso == 1) //si es quimerismo no saca caratula
                {
                    CmdPrefactura.Visible = true;
                   
                }
                  
                 


                if (oRegistro.Baja) //esta dado de baja
                {
                    //CmdBaseFA.Visible = false;
                    CmdEliminar.Visible = false;
                    CmdModificar.Text = "Consultar"; CmdModificar.Visible = VerificaPermisosObjeto("Modificar Caso");
                    CmdCaratula.Visible = false;
                    CmdCarga.Visible = false;
                    CmdResultados.Visible = false;
                    //CmdAuditoria.Visible = true;
                    CmdAuditoria.Visible = VerificaPermisosObjeto("Auditoria Caso");
                    CmdValida.Visible = false;
                    Label CountryNameLabel = (Label)e.Row.FindControl("lblNumero");
                    CountryNameLabel.CssClass = "label label-default";

                }
                else
                {
                    //Se verifica la facturacion de filiacion antes de que sea validado 
                    if (oRegistro.IdTipoCaso == 1) //filiacion
                    {
                        CmdPrefactura.Visible = true;
                        CmdPrefactura.CommandName = "Factura";
                        CmdPrefactura.ToolTip = "Factura";
                        string nro = oRegistro.getFactura();
                        if (nro != "")
                        {
                            CmdPrefactura.Text = "FC: " + nro;
                            CmdPrefactura.CommandName = "Factura";
                            CmdPrefactura.ToolTip = "Factura";
                            CmdPrefactura.CssClass = "label label-success";
                        }
                    }

                    if (oRegistro.IdUsuarioValida > 0)
                    {
                        //CmdResultados.Visible = true;
                        CmdResultados.Visible = VerificaPermisosObjeto("Imprimir Resultado");
                        

                        Label CountryNameLabel = (Label)e.Row.FindControl("lblNumero");
                        CountryNameLabel.CssClass = "label label-success";

                        CmdModificar.Text = "Consultar";
                        CmdEliminar.Visible = false;
                        if (oRegistro.IdTipoCaso == 2) //forense o filiacion
                        {
                           

                            CmdPrefactura.Visible = true;
                            //si tiene factura se
                         string nro=   oRegistro.getFactura();

                            if (nro != "")
                            {
                                CmdPrefactura.Text = "FC: " + nro;
                                CmdPrefactura.CommandName = "Factura";
                                CmdPrefactura.ToolTip = "Factura";
                                CmdPrefactura.CssClass = "label label-success";
                            }

                            else
                            { if (oRegistro.Prefacturado())
                                {
                                  
                                    CmdPrefactura.CssClass = "label label-warning";
                                }
                            }

                        }

                       


                    }
                    else
                    {
                        CmdAuditoria.Visible = VerificaPermisosObjeto("Auditoria Caso");
                        CmdCarga.Visible = VerificaPermisosObjeto("Carga Resultados");
                        CmdValida.Visible = VerificaPermisosObjeto("Valida Resultados");
                        if (oRegistro.IdTipoCaso == 2) //forense
                        {
                            if (!oRegistro.tienePresupuesto()) // si no tiene presupuesto no puede validar.
                                CmdValida.Visible = false;
                        }

                      
                        //else
                        //    CmdPrefactura.Visible = false;
                        CmdModificar.Visible = VerificaPermisosObjeto("Modificar Caso");
                        CmdEliminar.Visible = true;
                        //e.Row.Cells[0].BackColor = Color.Red;// pendiente
                        Label CountryNameLabel = (Label)e.Row.FindControl("lblNumero");
                        CountryNameLabel.CssClass = "label label-danger";
                        //e.Row.Cells[0].CssClass = "label label-danger";
                        if (oRegistro.IdUsuarioCarga > 0)
                        {
                            Label CountryNameLabel1 = (Label)e.Row.FindControl("lblNumero");
                            CountryNameLabel1.CssClass = "label label-warning";
                        }
                        //e.Row.Cells[0].CssClass = "label label-warning";
                        //e.Row.Cells[0].BackColor = Color.Yellow;// en proceso.
                    }

                 
                }


                //CmdModificar.Visible = VerificaPermisosObjeto("Modificar Caso");
                //CmdResultados.Visible = VerificaPermisosObjeto("Imprimir Resultado");
                //CmdAuditoria.Visible = VerificaPermisosObjeto("Auditoria Caso");
                //CmdCarga.Visible = VerificaPermisosObjeto("Carga Resultados");

                //CmdValida.Visible = VerificaPermisosObjeto("Valida Resultados");

              gvLista.DataKeys[e.Row.RowIndex].Value.ToString();

                

            }  

        }

        private bool VerificaPermisosObjeto(string v)
        { bool i = false;
           
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], v);
                switch (Permiso)
                {
                    case 0: i= false; break;
                    case 1:
                        i=true; break;
                case 2: i= true; break;

            }

            return i;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            gvLista.PageIndex = 0;
            CargarGrillaMedula();            
            CurrentPageLabel.Text = " ";
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLista_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
           
            
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;

            int currentPage = gvLista.PageIndex+1;


            CurrentPageLabel.Text = "Página " + currentPage.ToString() +
              " de " + gvLista.PageCount.ToString();
            if (idServicio.Value == "3")

                CargarGrillaMedula();
            else
                CargarGrilla();

           
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            
        }

       


        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            //MostrarInforme("Nomenclador");
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {   gvLista.PageIndex = 0;
            if (idServicio.Value == "3")
                CargarGrillaMedula();
            else
                CargarGrilla();

            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
            if (idServicio.Value == "3")
                CargarGrillaMedula();
            else
                CargarGrilla();
           
            CurrentPageLabel.Text = " ";
        }

        protected void lnkPdfReducido_Click(object sender, EventArgs e)
        {
            //MostrarInforme("Reducido");
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarArea();
            
            gvLista.PageIndex = 0;
            if (idServicio.Value == "3")
                CargarGrillaMedula();
            else
                CargarGrilla();

            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged1(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
            if (idServicio.Value == "3")
                CargarGrillaMedula();
            else
                CargarGrilla();

            CurrentPageLabel.Text = " ";
        }

        protected void gvLista_DataBound(object sender, EventArgs e)
        {

          

            
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {

            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "3");
            Server.Transfer("CasoNewPrueba.aspx");

                        //Response.Redirect("CasoNewPrueba.aspx" ,false);
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvLista.PageIndex = 0;
            CargarGrillaMedula();
            CurrentPageLabel.Text = " ";
        }

        protected void btnHojaTrabajo_Click(object sender, EventArgs e)
        {
            ImprimirHojaTrabajo();

        }

        private void ImprimirHojaTrabajo()
        {
            //Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            //oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id));
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            CrystalReportSource oCr = new CrystalReportSource();


            oCr.Report.FileName = "HojaTrabajoForense.rpt";
            oCr.ReportDocument.SetDataSource(LeerDatosHojaTrabajo());


            oCr.DataBind();

          
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "HojaTrabajo");

        }
    }
}
