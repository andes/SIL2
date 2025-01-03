﻿using System;
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
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Informes
{
  
    public partial class HistoriaClinicaFiltro : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request["Desde"]!= null)
           
            {
                this.MasterPageFile = "~/Resultados/SitePE.master";
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtDni.Focus(); CargarServicio();
                switch (Request["Tipo"].ToString())
                {
                    case "PacienteValidado":
                        {
                            VerificaPermisos("Historial de Resultados");
                            pnlAnalisis.Visible = false;
                            lblTitulo.Text = "HISTORIAL DE RESULTADOS";
                            rvAnalisis.Enabled = false;
                        } break;
                    case "PacienteCompleto":
                        {
                            VerificaPermisos("Historial de Visitas");
                            pnlAnalisis.Visible = false;
                            lblTitulo.Text = "HISTORIAL DE VISITAS";
                            rvAnalisis.Enabled = false;
                        } break;

                    case "PacienteForense":
                        {
                            VerificaPermisos("Historial de Resultados");
                            pnlAnalisis.Visible = false;
                            lblTitulo.Text = "HISTORIAL DE RESULTADOS";
                            rvAnalisis.Enabled = false;

                           
                                pnlTitulo.Attributes.Add("class", "panel panel-success");
                                gvLista.HeaderStyle.BackColor = System.Drawing.Color.Green;
                                btnBuscar.CssClass = "btn btn-success";
                               


                            
                        }
                        break;
                    case "Analisis":
                        {
                            VerificaPermisos("Historial Por Analisis");
                            pnlAnalisis.Visible = true;
                            lblTitulo.Text = "HISTORIAL POR ANALISIS";
                            rvAnalisis.Enabled = true;
                            CargarArea();
                            CargarItem();
                        }
                        break;
                }
                
               
            }
        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarServicio()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio with (nolock) where baja=0 and idtiposervicio<>6 order by nombre ";
            if (Request["Tipo"].ToString()=="PacienteForense")
                m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio with (nolock) where baja=0 and  idtiposervicio=6 order by nombre ";


            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            if (Request["Tipo"].ToString() != "PacienteForense") ddlServicio.Items.Insert(0, new ListItem("-- Todos --", "0"));            

        }
        private void CargarArea()
        { 
            Utility oUtil = new Utility();            
            string m_ssql = "SELECT A.idArea , TS.nombre + ' - ' + A.nombre AS nombre FROM LAB_Area AS A INNER JOIN LAB_TipoServicio AS TS ON A.idTipoServicio = TS.idTipoServicio WHERE (A.baja = 0) and  A.idtiposervicio<>6 order by TS.nombre, A.nombre ";//AND tipo='P'
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("-- Todas --", "0"));            
            m_ssql = null;
            oUtil = null;
        }

        private void CargarItem()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_strCondicion ="";            
            if (ddlArea.SelectedValue != "0")
                m_strCondicion = " and idArea= " + ddlArea.SelectedValue;
            string m_ssql = "select idItem, nombre + ' ['+ codigo+ ']' as nombre from Lab_Item where baja=0  and idEfector=idEfectorDerivacion   and idCategoria=0 and idtiporesultado>=1 " + m_strCondicion + " order by nombre";//AND tipo='P'
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;

            m_ssql = null;
            oUtil = null;
        }
        private void CargarGrilla2()
        {
            
            
        }

        private void CargarGrilla()
        {
            Utility oUtil = new Utility();
            string str_condicion = "";   string str_condicionMadre = "";
            if (txtFechaDesde.Value!="")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                str_condicion += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") +"'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                str_condicion += " AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
            }
            if (ddlNumero.SelectedValue == "Protocolo")
            {
                //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                //switch (oCon.TipoNumeracionProtocolo)
                //{
                //    case 0: // busqueda con autonumerico
                        if (txtProtocolo.Text != "") str_condicion += " AND P.numero='" + txtProtocolo.Text + "'"; //break;
                //    case 1: //busqueda con numero diario
                //        if (txtProtocolo.Text != "") str_condicion += " AND P.numeroDiario='" + txtProtocolo.Text + "'"; break;
                //    case 2: //busqueda con numero de grupo
                //        if (txtProtocolo.Text != "") str_condicion += " AND prefijoSector + CONVERT(varchar, numeroSector)='" + txtProtocolo.Text + "'"; break;
                //    case 3:
                //        if (txtProtocolo.Text != "") str_condicion += " AND P.numeroTipoServicio='" + txtProtocolo.Text + "'"; break;
                //}
            }
            if (ddlNumero.SelectedValue == "Origen")
            {
                if (txtProtocolo.Text != "")
                    str_condicion += " AND P.numeroOrigen='" + txtProtocolo.Text + "'";
            }




                if (txtDni.Value != "") str_condicion += " AND  Pa.idEstado <> 2 and Pa.numeroDocumento = '" + txtDni.Value + "'";
                if (txtApellido.Text != "") str_condicion += " AND Pa.apellido like '%" + oUtil.SacaComillas(txtApellido.Text) + "%'";
                if (txtNombre.Text != "") str_condicion += " AND Pa.nombre like '%" + oUtil.SacaComillas(txtNombre.Text) + "%'";
                if (txtFechaNac.Value != "")
                {
                    DateTime fecha1 = DateTime.Parse(txtFechaNac.Value);
                    str_condicion += " AND Pa.fechaNacimiento='" + fecha1.ToString("yyyyMMdd") + "'";
                }
                if (ddlSexo.SelectedValue != "0") str_condicion += " AND Pa.idSexo=" + ddlSexo.SelectedValue;
            if (txtNumeroAdic.Value != "") str_condicion += " AND Pa.numeroAdic like '%" + txtNumeroAdic.Value + "%'";
            /////////////////////////////////////////////////////////////////////////////////////////

            if (Request["Tipo"].ToString() == "PacienteForense") str_condicion += " and P.idTipoServicio=6";
            if ((Request["Tipo"].ToString() == "PacienteValidado")|| (Request["Tipo"].ToString() == "PacienteCompleto")) str_condicion += " and P.idTipoServicio!=6";

            if (Request["Tipo"].ToString() == "Analisis")
                {
                    str_condicion += "  AND (DP.idUsuarioValida > 0) ";
                    if (ddlItem.SelectedValue != "0")
                        str_condicion += "  AND DP.idsubitem=" + ddlItem.SelectedValue;
                }

                /////////////////////////////////////////////////////////////////////////////////////////

                ///////////////////////////////Condicion para buscar por la madre/tutor///////////////             
                if ((txtDniMadre.Value != "") || (txtApellidoMadre.Text != "") || (txtNombreMadre.Text != ""))
                {
                    str_condicionMadre = " and P.idPaciente in (Select idPaciente FROM  Sys_Parentesco WHERE  1=1 ";
                    if (txtDniMadre.Value != "") str_condicionMadre += " AND numeroDocumento = '" + txtDniMadre.Value + "'";
                    if (txtApellidoMadre.Text != "") str_condicionMadre += " AND apellido like '%" + oUtil.SacaComillas(txtApellidoMadre.Text) + "%'";
                    if (txtNombreMadre.Text != "") str_condicionMadre += " AND nombre like '%" + oUtil.SacaComillas(txtNombreMadre.Text) + "%'";
                    str_condicionMadre += " ) ";
                }
                /////////////////////////////////////////////////////////////////////////////////////////
                string m_strSQL = @" SELECT distinct P.idPaciente, case when Pa.idEstado=2 then Pa.numeroAdic+'(s/dni)'  else convert(varchar,Pa.numeroDocumento) end as numeroDocumento, Pa.apellido + ', ' + Pa.nombre as paciente, 
                                   convert(varchar(10), Pa.fechaNacimiento,103) as fechaNacimiento 
                                   FROM LAB_Protocolo P with (nolock)
                                   INNER JOIN Sys_Paciente Pa  with  (nolock) ON Pa.idPaciente= P.idPaciente 
                                   INNER JOIN LAB_DetalleProtocolo AS DP  with (nolock) ON P.idProtocolo = DP.idProtocolo 
                                   WHERE P.baja=0 and P.idTipoServicio<>5 " + str_condicion + str_condicionMadre +  @" ORDER BY   paciente";

                if ((ddlNumero.SelectedValue == "Tarjeta") && (txtProtocolo.Text != ""))
                {
                    if (ddlNumero.SelectedValue == "Tarjeta") str_condicion += " AND S.numeroTarjeta='" + txtProtocolo.Text + "'";
                    /////////////////////////////////////////////////////////////////////////////////////////
                    m_strSQL = @" SELECT distinct P.idPaciente, Pa.numeroDocumento, Pa.apellido + ', ' + Pa.nombre as paciente, 
                                      convert(varchar(10), Pa.fechaNacimiento,103) as fechaNacimiento 
                                      FROM LAB_Protocolo P  with  (nolock)
                                      INNER JOIN Sys_Paciente Pa  with  (nolock) ON Pa.idPaciente= P.idPaciente 
                                      INNER JOIN LAB_DetalleProtocolo AS DP  with (nolock) ON P.idProtocolo = DP.idProtocolo 
                                      left join [LAB_SolicitudScreening] as S  with (nolock) on S.idProtocolo= P.idprotocolo 
                                      WHERE P.baja=0 and P.idTipoServicio<5 " + str_condicion + str_condicionMadre + @" ORDER BY Pa.numeroDocumento, paciente";
                }
                DataSet Ds1 = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds1);
                if (Ds1.Tables[0].Rows.Count == 0)
                {
                if (Request["Tipo"].ToString() != "Analisis") //analisis es solo para pacientes
                {
                    try { 
                    //aca hago la busqueda primero dentro de las muestras de no pacientes, con el numero de protocolo o de origen, si no encuentro sigo con la busqueda de pacientes
                    m_strSQL = @" SELECT distinct  P.idProtocolo, P.numero, Pa.nombre + ', ' + P.descripcionProducto as producto,  P.numeroOrigen as numeroOrigen, 
                                       convert(varchar(10), P.fecha, 103) as fechaProtocolo  
                                       FROM LAB_Protocolo P (nolock)
                                       INNER JOIN LAB_Muestra Pa (nolock) ON Pa.idMuestra = P.idMuestra 
                                      WHERE P.idtipoServicio=5 and P.baja=0 " + str_condicion;

                    DataSet Ds = new DataSet();
                    //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                    //SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    adapter.Fill(Ds);


                    gvListaProducto.DataSource = Ds.Tables[0];
                    gvListaProducto.DataBind();
                    gvLista.Visible = false;
                    gvListaProducto.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        gvLista.DataSource = Ds1.Tables[0];
                        gvLista.DataBind();
                        gvListaProducto.Visible = false;
                        gvLista.Visible = true;
                    }
                }
                

                }
                else {

                    gvLista.DataSource = Ds1.Tables[0];
                    gvLista.DataBind();
                gvListaProducto.Visible = false;
                gvLista.Visible =true;
            }

                
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                CargarGrilla();

        }


        protected void lnkAmpliarFiltros_Click(object sender, EventArgs e)
        {
            if (lnkAmpliarFiltros.Text == "Ampliar filtros de búsqueda")
            {
                lnkAmpliarFiltros.Text = "Ocultar filtros adicionales";
                pnlParentesco.Visible = true;
            }
            else
            {
                lnkAmpliarFiltros.Text = "Ampliar filtros de búsqueda";
                pnlParentesco.Visible = false;
            }

            lnkAmpliarFiltros.UpdateAfterCallBack = true;
            pnlParentesco.UpdateAfterCallBack = true;
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Visualizar":
                    {
                        string m_parametro = " P.idPaciente=" + e.CommandArgument.ToString();
                        //if (Request["Tipo"].ToString() == "PacienteValidado") m_parametro += " and P.estado>0 AND (DP.idUsuarioValida > 0) ";                        

                        if (txtFechaDesde.Value != "")
                        {
                            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                            m_parametro += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "'";
                        }
                        if (txtFechaHasta.Value != "")
                        {
                            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                            m_parametro += " AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
                        }

                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        if (ddlNumero.SelectedValue == "Protocolo")
                        {
                            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                            switch (oCon.TipoNumeracionProtocolo)
                            {
                                case 0: // busqueda con autonumerico
                                    if (txtProtocolo.Text != "")
                                        m_parametro += " AND P.numero='" + txtProtocolo.Text + "'";
                                    break;
                                case 1: //busqueda con numero diario
                                    if (txtProtocolo.Text != "") m_parametro += " AND P.numeroDiario='" + txtProtocolo.Text + "'"; break;
                                case 2: //busqueda con numero de grupo
                                    if (txtProtocolo.Text != "") m_parametro += " AND prefijoSector + CONVERT(varchar, numeroSector)='" + txtProtocolo.Text + "'"; break;
                                case 3:
                                    if (txtProtocolo.Text != "") m_parametro += " AND P.numeroTipoServicio='" + txtProtocolo.Text + "'"; break;
                            }
                        }
                        if (ddlNumero.SelectedValue == "Origen") { if (txtProtocolo.Text != "") m_parametro += " AND P.numeroOrigen='" + txtProtocolo.Text + "'"; }
                        if (ddlNumero.SelectedValue == "Tarjeta") { if (txtProtocolo.Text != "") m_parametro += " AND S.numeroTarjeta='" + txtProtocolo.Text + "'"; }
                        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        switch (Request["Tipo"].ToString() )
                        {
                            case "Analisis":                              //por analisis
                            Response.Redirect("HistoriaClinica.aspx?idPaciente=" + e.CommandArgument.ToString() + "&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value + "&idAnalisis=" + ddlItem.SelectedValue); break;
                            case "PacienteCompleto":
                            Response.Redirect("../Resultados/Procesa.aspx?idServicio="+ddlServicio.SelectedValue+"&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=0&modo=Normal&Desde=HistoriaClinicaFiltro&Tipo=PacienteCompleto", false);break;
                            case "PacienteForense":
                                Response.Redirect("../Resultados/Procesa.aspx?idServicio=" + ddlServicio.SelectedValue + "&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=0&modo=Normal&Desde=HistoriaClinicaFiltro&Tipo=PacienteForense", false); break;
                            case "PacienteValidado":
                                {
                                  


                                    if (Request["Desde"] != null)
                                        Response.Redirect("../Resultados/Procesa.aspx?idServicio=" + ddlServicio.SelectedValue + "&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal&Tipo=PacienteValidado&master=2", false);
                                    else
                                        Response.Redirect("../Resultados/Procesa.aspx?idServicio=" + ddlServicio.SelectedValue + "&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal&Tipo=PacienteValidado", false);
                                    break;
                                }
                        }
                        
                
             

                        break;
                    }
              
            }           

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Visualizar";
                CmdModificar.ToolTip = "Ver Historia Clínica";
            }

        }

        protected void cvDatosEntrada_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtDni.Value == "")
                if (txtApellido.Text == "")
                    if (txtNombre.Text == "")                       
                        if (txtDniMadre.Value == "")                            
                            if (txtApellidoMadre.Text == "")                                
                                if (txtNombreMadre.Text == "")
                                    if (txtProtocolo.Text=="")
                                        if (txtNumeroAdic.Value == "")
                                            args.IsValid = false;                                
                    

                else
                    args.IsValid = true;
            else
                args.IsValid = true;        
        }

        protected void rdbTipoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "")
            {
                Item oItem = new Item();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Item));
                crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
                crit.Add(Expression.Eq("Baja", false));
                crit.Add(Expression.Eq("IdCategoria", 0));

                if (ddlArea.SelectedValue != "0")
                {
                    Area oArea = new Area();
                    crit.Add(Expression.Eq("IdArea", (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue))));
                }
          //      crit.Add(Expression.Eq("Tipo", "P"));
                
                //  crit.Add(Expression.Eq("IdArea", (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue))));

                oItem = (Item)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlItem.SelectedValue = oItem.IdItem.ToString();                  
                }
                else
                {
                    lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no existe. ";
                    ddlItem.SelectedValue = "0";
                    txtCodigo.Text = "";                
                    txtCodigo.UpdateAfterCallBack = true;
                    
                }

                ddlItem.UpdateAfterCallBack = true;              
                lblMensaje.UpdateAfterCallBack = true;
            }
            else
            {
                ddlItem.SelectedValue = "0";            
                ddlItem.UpdateAfterCallBack = true;                
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
                txtCodigo.Text = oItem.Codigo;
              
            }
            else
            {
                txtCodigo.Text = "";
            

            }
         
            txtCodigo.UpdateAfterCallBack = true;
          
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarItem();
        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (oCon.TipoNumeracionProtocolo == 2)  //letras y numeros
            {
                args.IsValid = true;
            }
            else   ///solo numeros
            {
                if (ddlNumero.SelectedValue == "Protocolo")
                {
                    if (txtProtocolo.Text != "") { if (oUtil.EsEntero(txtProtocolo.Text)) args.IsValid = true; else args.IsValid = false; }
                    else
                        args.IsValid = true;
                }
                else
                {
                    if (ddlNumero.SelectedValue == "Tarjeta")
                    {
                        if (txtProtocolo.Text != "") { if (oUtil.EsEntero(txtProtocolo.Text)) args.IsValid = true; else args.IsValid = false; }
                        else
                            args.IsValid = true;
                    }
                    else
                        args.IsValid = true;
                }
            }
        }

        protected void cvDNI_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            if (txtDni.Value != "") 
            { if (oUtil.EsEntero(txtDni.Value)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

        protected void cvFecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (txtFechaNac.Value != "")
                {
                    DateTime f1 = DateTime.Parse(txtFechaNac.Value);
                    args.IsValid = true;
                }
                else
                    args.IsValid = true;
            }
            catch (Exception ex)
            {
                
                args.IsValid = false;
            }
        }

        protected void cvDNIMadre_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            if (txtDniMadre.Value != "")
            { if (oUtil.EsEntero(txtDniMadre.Value)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

        protected void lnkHistorial_Click(object sender, EventArgs e)
        {
            Response.Redirect("HistorialPorUsuario.aspx", false);
        }

        protected void gvListaProducto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[4].Controls[1];
                CmdModificar.CommandArgument = this.gvListaProducto.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Visualizar";
                CmdModificar.ToolTip = "Ver Protocolo";
            }


        }

        protected void gvListaProducto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "Visualizar")
            {
                
                        string m_parametro = " P.idProtocolo=" + e.CommandArgument.ToString();
                        

                        switch (Request["Tipo"].ToString())
                        {
                            case "Analisis":                              //por analisis
                                Response.Redirect("HistoriaClinica.aspx?idPaciente=" + e.CommandArgument.ToString() + "&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value + "&idAnalisis=" + ddlItem.SelectedValue); break;
                            case "PacienteCompleto":
                                Response.Redirect("../Resultados/Procesa.aspx?idServicio=5&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=0&modo=Normal&Desde=HistoriaClinicaFiltro&Tipo=PacienteCompleto", false); break;
                            case "PacienteValidado":
                                Response.Redirect("../Resultados/Procesa.aspx?idServicio=5&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal&Tipo=PacienteValidado", false); break;
                        }

                
            }


        }
    }
}
