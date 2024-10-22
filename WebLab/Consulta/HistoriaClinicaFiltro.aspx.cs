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
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Consulta
{
  
    public partial class HistoriaClinicaFiltro : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
           
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtDni.Focus(); //CargarServicio();
                //switch (Request["Tipo"].ToString())
                //{
                //    case "PacienteValidado":
                //        {
                       //     VerificaPermisos("Historial de Resultados");
                            //pnlAnalisis.Visible = false;
                            lblTitulo.Text = "HISTORIAL DE RESULTADOS";
                            //rvAnalisis.Enabled = false;
                //        } break;
                //    case "PacienteCompleto":
                //        {
                //            VerificaPermisos("Historial de Visitas");
                //            pnlAnalisis.Visible = false;
                //            lblTitulo.Text = "HISTORIAL DE VISITAS";
                //            rvAnalisis.Enabled = false;
                //        } break;

                //    case "PacienteForense":
                //        {
                //            VerificaPermisos("Historial de Resultados");
                //            pnlAnalisis.Visible = false;
                //            lblTitulo.Text = "HISTORIAL DE RESULTADOS";
                //            rvAnalisis.Enabled = false;

                           
                //                pnlTitulo.Attributes.Add("class", "panel panel-success");
                //                gvLista.HeaderStyle.BackColor = System.Drawing.Color.Green;
                //                btnBuscar.CssClass = "btn btn-success";
                               


                            
                //        }
                //        break;
                //    case "Analisis":
                //        {
                //            VerificaPermisos("Historial Por Analisis");
                //            pnlAnalisis.Visible = true;
                //            lblTitulo.Text = "HISTORIAL POR ANALISIS";
                //            rvAnalisis.Enabled = true;
                //            CargarArea();
                //            CargarItem();
                //        }
                //        break;
                //}
                
               
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


       
        //private void CargarArea()
        //{ 
        //    Utility oUtil = new Utility();            
        //    string m_ssql = "SELECT A.idArea , TS.nombre + ' - ' + A.nombre AS nombre FROM LAB_Area AS A INNER JOIN LAB_TipoServicio AS TS ON A.idTipoServicio = TS.idTipoServicio WHERE (A.baja = 0) and  A.idtiposervicio<>6 order by TS.nombre, A.nombre ";//AND tipo='P'
        //    oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
        //    ddlArea.Items.Insert(0, new ListItem("-- Todas --", "0"));            
        //    m_ssql = null;
        //    oUtil = null;
        //}

     
        private void CargarGrilla2()
        {
            Utility oUtil = new Utility();
            //string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            string sql = null;
          
            string connetionString = ConfigurationManager.ConnectionStrings["SIPS"].ConnectionString;
            string str_condicion = "";
            if (txtDni.Value != "") str_condicion += " AND  P.numeroDocumento = '" + txtDni.Value.Trim() + "'";
            if (txtApellido.Text != "") str_condicion += " AND P.apellido like '%" + oUtil.SacaComillas(txtApellido.Text) + "%'";
            if (txtNombre.Text != "") str_condicion += " AND P.nombre like '%" + oUtil.SacaComillas(txtNombre.Text) + "%'";
            //     str_condicion = str_condicion + " and (EFECTORSOLICITANTE like '%ALLEN%' OR EFECTORSOLICITANTE like '%CIPOLL%') ";
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                str_condicion +=  " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                str_condicion += " AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
            }
            str_condicion += " AND sector = 'DERIVACION' AND ORIGEN = 'RIO NEGRO'";

            sql = @"SELECT distinct   P.numeroDocumento  as numeroDocumento, P.apellido + '  ' + P.nombre as paciente,  
                                    convert(varchar(10), P.fechaNacimiento,103) as fechaNacimiento  
                                    FROM LAB_ResultadoEncabezado P (nolock)                                  
                                    WHERE P.idEfector=205  " + str_condicion + @" ORDER BY   paciente";
            connection = new SqlConnection(connetionString);
            
                DataSet Ds1 = new DataSet();
                
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(sql, connection);
                adapter.Fill(Ds1);
                gvLista.DataSource = Ds1.Tables[0];
                gvLista.DataBind();

            

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                CargarGrilla2();

        }


   

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Utility oUtil = new Utility();
            switch (e.CommandName)
            {
                case "Visualizar":
                    {
                        string m_parametro = " P.numerodocumento=" + e.CommandArgument.ToString();
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


                        if (txtApellido.Text != "") m_parametro += " AND P.apellido like '%" + oUtil.SacaComillas(txtApellido.Text) + "%'";
                        if (txtNombre.Text != "") m_parametro += " AND P.nombre like '%" + oUtil.SacaComillas(txtNombre.Text) + "%'";

                        m_parametro += " AND sector = 'DERIVACION' AND ORIGEN = 'RIO NEGRO'";

                      

                        Response.Redirect("Procesa.aspx?Operacion=HC&Parametros=" + m_parametro , false);
                                   

                        
                
             

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
                       
                                            args.IsValid = false;                                
                    

                else
                    args.IsValid = true;
            else
                args.IsValid = true;        
        }

        protected void rdbTipoConsulta_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            
        }

        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtCodigo.Text != "")
        //    {
        //        Item oItem = new Item();
        //        ISession m_session = NHibernateHttpModule.CurrentSession;
        //        ICriteria crit = m_session.CreateCriteria(typeof(Item));
        //        crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
        //        crit.Add(Expression.Eq("Baja", false));
        //        crit.Add(Expression.Eq("IdCategoria", 0));

        //        if (ddlArea.SelectedValue != "0")
        //        {
        //            Area oArea = new Area();
        //            crit.Add(Expression.Eq("IdArea", (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue))));
        //        }
          
        //        oItem = (Item)crit.UniqueResult();
        //        if (oItem != null)
        //        {
        //            ddlItem.SelectedValue = oItem.IdItem.ToString();                  
        //        }
        //        else
        //        {
        //            lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no existe. ";
        //            ddlItem.SelectedValue = "0";
        //            txtCodigo.Text = "";                
        //            txtCodigo.UpdateAfterCallBack = true;
                    
        //        }

        //        ddlItem.UpdateAfterCallBack = true;              
        //        lblMensaje.UpdateAfterCallBack = true;
        //    }
        //    else
        //    {
        //        ddlItem.SelectedValue = "0";            
        //        ddlItem.UpdateAfterCallBack = true;                
        //    }
        //}

        //protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlItem.SelectedValue != "0")
        //    {
        //        Item oItem = new Item();
        //        oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));
        //        txtCodigo.Text = oItem.Codigo;
              
        //    }
        //    else
        //    {
        //        txtCodigo.Text = "";
            

        //    }
         
        //    txtCodigo.UpdateAfterCallBack = true;
          
        //}

        //protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        //{
        ////    CargarItem();
        //}

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Utility oUtil = new Utility();
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            //if (oCon.TipoNumeracionProtocolo == 2)  //letras y numeros
            //{
            //    args.IsValid = true;
            //}
            //else   ///solo numeros
            //{
            //    if (ddlNumero.SelectedValue == "Protocolo")
            //    {
            //        if (txtProtocolo.Text != "") { if (oUtil.EsEntero(txtProtocolo.Text)) args.IsValid = true; else args.IsValid = false; }
            //        else
            //            args.IsValid = true;
            //    }
            //    else
            //    {
            //        if (ddlNumero.SelectedValue == "Tarjeta")
            //        {
            //            if (txtProtocolo.Text != "") { if (oUtil.EsEntero(txtProtocolo.Text)) args.IsValid = true; else args.IsValid = false; }
            //            else
            //                args.IsValid = true;
            //        }
            //        else
            //            args.IsValid = true;
            //    }
            //}
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

        //protected void cvDNIMadre_ServerValidate(object source, ServerValidateEventArgs args)
        //{
        //    Utility oUtil = new Utility();
        //    if (txtDniMadre.Value != "")
        //    { if (oUtil.EsEntero(txtDniMadre.Value)) args.IsValid = true; else args.IsValid = false; }
        //    else
        //        args.IsValid = true;
        //}

        protected void lnkHistorial_Click(object sender, EventArgs e)
        {
            Response.Redirect("HistorialPorUsuario.aspx", false);
        }

        //protected void gvListaProducto_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        ImageButton CmdModificar = (ImageButton)e.Row.Cells[4].Controls[1];
        //        CmdModificar.CommandArgument = this.gvListaProducto.DataKeys[e.Row.RowIndex].Value.ToString();
        //        CmdModificar.CommandName = "Visualizar";
        //        CmdModificar.ToolTip = "Ver Protocolo";
        //    }


        //}

        protected void gvListaProducto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName== "Visualizar")
            {
                
                        string m_parametro = " P.idProtocolo=" + e.CommandArgument.ToString();
                        

                        switch (Request["Tipo"].ToString())
                        {
                            //case "Analisis":                              //por analisis
                            //    Response.Redirect("HistoriaClinica.aspx?idPaciente=" + e.CommandArgument.ToString() + "&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value + "&idAnalisis=" + ddlItem.SelectedValue); break;
                            //case "PacienteCompleto":
                            //    Response.Redirect("../Resultados/Procesa.aspx?idServicio=5&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=0&modo=Normal&Desde=HistoriaClinicaFiltro&Tipo=PacienteCompleto", false); break;
                            case "PacienteValidado":
                                Response.Redirect("../Resultados/Procesa.aspx?idServicio=5&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal&Tipo=PacienteValidado", false); break;
                        }

                
            }


        }
    }
}
