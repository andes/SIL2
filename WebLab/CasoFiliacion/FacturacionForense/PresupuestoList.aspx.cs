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
using Business.Data.Facturacion;
//using System.Web.UI.WebControls;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class PresupuestoList : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            HttpContext Context;
                Context = HttpContext.Current;
          
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Presupuestos");
                txtFechaDesde.Value = DateTime.Now.AddDays(-30).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                CargarListas();
                CargarGrilla();
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (Permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;

                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            

            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
           string m_ssql = "SELECT  idSectorServicio,  nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";

            oUtil.CargarCombo(ddlServicio, m_ssql, "idSectorServicio", "nombre");
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));


            m_ssql = null;
            oUtil = null;
        }
        //private string getEstado(int v)
        //{
        //    string m_strSQL="";
        //    if (idServicio.Value == "3")
        //    {
        //        m_strSQL = @"select count(*) as cantidad, 'Terminado' as estado from LAB_CasoFiliacion where idTipoCaso = 0 ";
        //    }
        //    else
        //    {
        //        m_strSQL = @"select count(*) as cantidad, 'Terminado' as estado from LAB_CasoFiliacion where 1=1 ";


        //        if (ddlTipoCaso.SelectedValue != "-1")
        //            m_strSQL += " and idTipoCaso =  " + ddlTipoCaso.SelectedValue;//tipo filiacion o forense
        //        else
        //            m_strSQL += " and idTipoCaso >0  ";
        //    }
        //    switch (v)
        //            { case 0:                        m_strSQL += " and baja=0 and idUsuarioValida=0 and idUsuarioCarga=0"; break;
        //            case 1: m_strSQL += " and baja=0 and idUsuarioValida=0 and idUsuarioCarga>0"; break;
        //            case 2: m_strSQL += " and baja=0 and idUsuarioValida>0"; break;
        //        }




        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);



        //    return Ds.Tables[0].Rows[0][0].ToString() ;
        //}



        //private void PonerContadores()
        //{
        //    lblNoProcesado.Text = getEstado(0);
        //    lblEnProceso.Text = getEstado(1);
        //    lblTerminado.Text = getEstado(2);
        //}

        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        //private void VerificaPermisos(string sObjeto)
        //{
        //    if (Session["s_permiso"] != null)
        //    {
        //        Utility oUtil = new Utility();
        //        Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
        //        switch (Permiso)
        //        {
        //            case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
        //            case 1:
        //                {
        //                    btnNuevo.Visible = false;
        //                    gvLista.Columns[5].Visible = false;
        //                }
        //                break;
        //            case 2: btnNuevo.Visible = true; break;

        //        }
        //    }
        //    else
        //        Response.Redirect("../FinSesion.aspx", false);

        //}

        private void CargarGrilla()
        {           
          
            gvLista.DataSource = LeerDatos("Extendido");
            gvLista.DataBind();
             //PonerContadores();
           
        }


   

        private object LeerDatos(string tipo)
        {
             string m_condicion = "  P.baja=0  ";
            //string m_condicionPersona = "";
           
          
             if (txtNumero.Text != "") m_condicion += " and P.idpresupuesto=" + txtNumero.Text.Trim();
             if (txtNombre.Text != "") m_condicion += " and P.nombre like '%" + txtNombre.Text + "%'";
            //if (txtDU.Text != "") m_condicionPersona += " and Pac.numerodocumento=" + txtDU.Text + " and Pac.idestado in (1,3)" ;

            //string m_strSQL ="";
             string m_orden = "";
            if (ddlTipo.SelectedValue == "0") m_orden = " ORDER BY P.idpresupuesto desc";
            if (ddlTipo.SelectedValue == "1") m_orden = " ORDER BY P.idpresupuesto  asc";
            
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                m_condicion += " AND P.fechaRegistro>= '" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
                m_condicion += " AND P.fechaRegistro<= '" + fecha2.ToString("yyyyMMdd") + "'";
            }


            //if (ddlEstado.SelectedValue == "-1")
            //    m_condicion += " and baja = 0 "; // solo los activos
            //if (ddlEstado.SelectedValue == "0")
            //    m_condicion += " and idusuariocarga= 0  and idusuariovalida=0 and baja = 0"; // no procesado

            //if (ddlEstado.SelectedValue == "1")
            //    m_condicion += " and idusuariocarga> 0  and idusuariovalida=0 and baja = 0"; // en proces o

            //if (ddlEstado.SelectedValue == "2")
            //    m_condicion += " and idusuariovalida>0 and baja = 0"; //Terminado
            //if (ddlEstado.SelectedValue == "3")
            //    m_condicion += " and baja = 1 "; // eliminados

             if (ddlServicio.SelectedValue != "0")
                 m_condicion += " and P.idServicio =  " + ddlServicio.SelectedValue;



            string   m_strSQL = @"select distinct  P.idPresupuesto,convert(varchar(10), fecha,103) as fecha ,P.nombre,  U.apellido as usuario, S.nombre as solicitante, P.fechaRegistro ,

case when (select count(*) from LAB_DetallePresupuesto where idPresupuesto=P.idPresupuesto and idfactura>0) >0 then 'Facturado'
else
	case when 
(select count(*) from LAB_DetallePresupuesto where idPresupuesto=P.idPresupuesto and prefacturado=1) >0 then 'PreFacturado'
else 

	case when isnull( C.idPresupuesto,0)=0 then 'Iniciado' else 'Con Caso Asignado' 
 
	end
end
end  as estado
 from LAB_Presupuesto as P   
inner join Sys_Usuario as U on P.idUsuarioRegistro= U.idUsuario
inner join lab_sectorServicio as S on S.idSectorServicio= P.idservicio
left join LAB_CasoPresupuesto as C on C.idpresupuesto= P.idPresupuesto
where " + m_condicion + m_orden;
                
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

             
             
            return Ds.Tables[0];
        }


        //        private DataTable LeerDatosHojaTrabajo()
        //        {
        //            string m_condicion = " 1 =1 ";
        //            string m_condicionPersona = "";

        //            if (txtNumero.Text != "") m_condicion += " and C.IDCASOFILIACION =" + txtNumero.Text.Trim();
        //            if (txtNombre.Text != "") m_condicion += " and C.nombre like '%" + txtNombre.Text + "%'";
        //            if (txtDU.Text != "") m_condicionPersona += " and Pac.numerodocumento=" + txtDU.Text + " and Pac.idestado in (1,3)";

        //            string m_strSQL = "";
        //            string m_orden = "";
        //            if (ddlTipo.SelectedValue == "0") m_orden = " ORDER BY C.IDCASOFILIACION desc";
        //            if (ddlTipo.SelectedValue == "1") m_orden = " ORDER BY C.IDCASOFILIACION asc";
        //            if (txtFechaDesde.Value != "")
        //            {
        //                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
        //                m_condicion += " AND C.fechaRegistro>= '" + fecha1.ToString("yyyyMMdd") + "'";
        //            }
        //            if (txtFechaHasta.Value != "")
        //            {
        //                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
        //                m_condicion += " AND C.fechaRegistro<= '" + fecha2.ToString("yyyyMMdd") + "'";
        //            }


        //            if (ddlEstado.SelectedValue == "-1")
        //                m_condicion += " and C.baja = 0 "; // solo los activos
        //            if (ddlEstado.SelectedValue == "0")
        //                m_condicion += " and C.idusuariocarga= 0  and C.idusuariovalida=0 and C.baja = 0"; // no procesado

        //            if (ddlEstado.SelectedValue == "1")
        //                m_condicion += " and C.idusuariocarga> 0  and C.idusuariovalida=0 and C.baja = 0"; // en proces o

        //            if (ddlEstado.SelectedValue == "2")
        //                m_condicion += " and C.idusuariovalida>0 and C.baja = 0"; //Terminado
        //            if (ddlEstado.SelectedValue == "3")
        //                m_condicion += " and C.baja = 1 "; // eliminados

        //            if (ddlTipoCaso.SelectedValue != "-1")
        //                m_condicion += " and c.idTipoCaso =  " + ddlTipoCaso.SelectedValue;//tipo filiacion o forense


        //                    m_strSQL = @"select C.idCasoFiliacion as numero, C.fechacarga as fecha, C.nombre as titulo, P.numero as codigo ,
        //   case when C.idtipoCaso = 2 then
        //     M.nombre +  ' '  +  P.descripcionProducto + '' +  case when Pa.idPaciente = -1 then '' else ' de ' + Pa.apellido + ' ' + Pa.nombre end
        //	else
        //    Pa.apellido + ' ' + Pa.nombre
        //    end
        //     as determinacion
        //from lab_protocolo P inner join
        //LAB_CasoFiliacionProtocolo as CP on CP.idProtocolo = P.idProtocolo inner join 
        //LAB_CasoFiliacion as C on C.idCasoFiliacion = CP.idCasoFiliacion  left JOIN 
        //Sys_Paciente Pa on Pa.idPaciente = P.idPaciente inner join
        //lab_muestra as M  on M.idmuestra= P.idmuestra
        //where idTipoCaso>0 and  " + m_condicion + m_orden;


        //            DataSet Ds = new DataSet();
        //            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //            SqlDataAdapter adapter = new SqlDataAdapter();
        //            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //            adapter.Fill(Ds);




        //            return Ds.Tables[0];
        //        }



        private void Imprimir(int p)
        {

            Presupuesto oRegistro = new Presupuesto();
            oRegistro = (Presupuesto)oRegistro.Get(typeof(Presupuesto), p);


            CrystalReportSource oCr = new CrystalReportSource();


            oCr.Report.FileName = "presupuesto.rpt";
            oCr.ReportDocument.SetDataSource(oRegistro.getInforme());


            oCr.DataBind();
            //if (Desde.Value == "Carga")
            //    oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuario"].ToString()), "Resultado_" + oRegistro.Nombre.Trim());
            //else
            //    oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuarioValida"].ToString()), "Resultado_" + oRegistro.Nombre.Trim());



            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Presupuesto_" + oRegistro.IdPresupuesto.ToString());


        }

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            //string m_parametroFiltro=  "&Nombre=" + txtNombre.Text ;


            if (e.CommandName != "Page")
            {
               
                switch (e.CommandName)
                {
                    case "Modificar":
                        Response.Redirect("Presupuestoedit.aspx?id=" + e.CommandArgument.ToString(), false);
                        break;
                    case "Casos":
                        Response.Redirect("Presupuestocaso.aspx?id=" + e.CommandArgument.ToString(), false);
                        break;

                    case "Descargar":
                        Imprimir(int.Parse(e.CommandArgument.ToString())); break;
                    case "Caratula":
                        ImprimirCaratula(e.CommandArgument.ToString()); break;
                    //    break;
                    //case "Auditoria":
                    //    ImprimirAuditoria(e.CommandArgument.ToString()); break;
                    ////    Response.Redirect("ItemDiagramacion.aspx?id=" + e.CommandArgument);

                    case "Prefactura":
                        //if (Request["idServicio"].ToString()=="6")
                        //Response.Redirect("CasoResultado.aspx?idServicio=6&id=" + e.CommandArgument.ToString() + "&Desde=Carga", false);
                        //else
                        //    Response.Redirect("CasoResultadoHisto.aspx?idServicio=3&id=" + e.CommandArgument.ToString() + "&Desde=Carga", false);

                      
                        break;
                    
                    
                    case "Eliminar":
                        {
                            Eliminar(e.CommandArgument);
                            
                                CargarGrilla();
                        }
                        break;

                   

                }
            }
        }

        //private void RedireccionarsegunTipo(string v, string m_parametroFiltro)
        //{
        //    Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
        //    oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(v));

        //    if (idServicio.Value == "6")
        //    {
        //        if (oRegistro.IdTipoCaso == 1) /// filiacion
        //        {
        //            Context.Items.Add("idServicio", idServicio.Value);
        //            Context.Items.Add("id", v);
        //            Context.Items.Add("parametros", m_parametroFiltro);
        //            Server.Transfer("CasoEdit.aspx");
        //        }
        //        else // forense o quimerismo
        //        {
        //            //string m_parametroFiltro = "&Nombre=" + txtNombre.Text;
        //            Context.Items.Add("idServicio", idServicio.Value);
        //            Context.Items.Add("id", v);
        //            Context.Items.Add("parametros", m_parametroFiltro);
        //            Server.Transfer("CasoForenseView.aspx");
        //        }
        //    }
        //    if (idServicio.Value == "3") // histo
        //    {
        //        Context.Items.Add("idServicio", idServicio.Value);
        //        Context.Items.Add("id", v);
        //        Context.Items.Add("parametros", m_parametroFiltro);
        //        Server.Transfer("CasoEdit.aspx");
        //    }
        //}

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
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;


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

       
        private void Eliminar(object idPresupuesto)
        { 


            Usuario oUser = new Usuario();
            Presupuesto oRegistro = new Presupuesto();
            oRegistro = (Presupuesto)oRegistro.Get(typeof(Presupuesto), int.Parse(idPresupuesto.ToString()));
           
             
            oRegistro.Baja = true;
            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();

           
                //estatus.Text = "No es posible eliminar. Tiene registros vinculados";
        }

       

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            Session.Contents.Remove("idUsuarioValida");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Presupuesto oRegistro = new Presupuesto();
                oRegistro = (Presupuesto)oRegistro.Get(typeof(Presupuesto), int.Parse(this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString()));
                LinkButton CmdCaso = (LinkButton)e.Row.Cells[7].Controls[1];
                CmdCaso.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdCaso.CommandName = "Casos";
                CmdCaso.ToolTip = "Casos";


                LinkButton CmdModificar = (LinkButton)e.Row.Cells[8].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modificar";

              

                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[9].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";


               
                LinkButton CmdResultados = (LinkButton)e.Row.Cells[10].Controls[1];
                CmdResultados.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdResultados.CommandName = "Descargar";
                CmdResultados.ToolTip = "Descargar";
          


                LinkButton CmdAuditoria = (LinkButton)e.Row.Cells[11].Controls[1];
                CmdAuditoria.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdAuditoria.CommandName = "Auditoria";
                CmdAuditoria.ToolTip = "Auditoria";
               

                LinkButton CmdCarga = (LinkButton)e.Row.Cells[12].Controls[1];
                CmdCarga.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdCarga.CommandName = "Prefactura";
                CmdCarga.ToolTip = "Prefactura";
 
 

                CmdEliminar.Visible = false;
                switch (e.Row.Cells[6].Text)
                {
                    case "Facturado": e.Row.Cells[6].BackColor = Color.Green; break;
                    case "PreFacturado": e.Row.Cells[6].BackColor = Color.Orange; break;
                    case "Con Caso Asignado": e.Row.Cells[6].BackColor = Color.Yellow; break;
                    case "Iniciado": { e.Row.Cells[6].BackColor = Color.Red;
                            CmdEliminar.Visible = true;
                        } break;
                }


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
            CargarGrilla();      
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
            
                CargarGrilla();

            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
           
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
            
                CargarGrilla();

            CurrentPageLabel.Text = " ";
        }

        protected void ddlArea_SelectedIndexChanged1(object sender, EventArgs e)
        { gvLista.PageIndex = 0;
           
                CargarGrilla();

            CurrentPageLabel.Text = " ";
        }

        protected void gvLista_DataBound(object sender, EventArgs e)
        {

          

            
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {

             

                     Response.Redirect("PresupuestoEdit.aspx" ,false);
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvLista.PageIndex = 0;
           
            CurrentPageLabel.Text = " ";
        }

        protected void btnHojaTrabajo_Click(object sender, EventArgs e)
        {
            //ImprimirHojaTrabajo();

        }

        //private void ImprimirHojaTrabajo()
        //{
        //    //Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
        //    //oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id));
        //    //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

        //    CrystalReportSource oCr = new CrystalReportSource();


        //    oCr.Report.FileName = "HojaTrabajoForense.rpt";
        //    oCr.ReportDocument.SetDataSource(LeerDatosHojaTrabajo());


        //    oCr.DataBind();

          
        //    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "HojaTrabajo");

        //}
    }
}
