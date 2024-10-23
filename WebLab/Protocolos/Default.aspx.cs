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
using System.Drawing;
using Business.Data;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

namespace WebLab.Protocolos
{
    public partial class Default : System.Web.UI.Page
    {
       
        public Configuracion oC = new Configuracion();


        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!Page.IsPostBack)
            {
                string s_pagina = "ProtocoloEdit2.aspx";
                if (Request["idServicio"].ToString() == "4") s_pagina = "ProtocoloEditPesquisa.aspx";
                if (Request["idServicio"].ToString() == "6")
                {
                    btnFinalizarCaso.Visible = true;
                    //lblTitulo.Text="Paso 2: Identificacion de Persona  - Caso Nro. "+ Request["idCaso"].ToString();
                    //pnlTitulo.Visible = false;
                       s_pagina = "ProtocoloEditForense.aspx";
                }
                txtDni.Focus();

                if (Request["idServicio"] != null) {
                     if (Request["idServicio"].ToString() =="1")VerificaPermisos("Pacientes sin turno");
                     if (Request["idServicio"].ToString() == "3") VerificaPermisos("Recepción de Muestras");
                     if (Request["idServicio"].ToString() == "4") VerificaPermisos("Recepción de Pacientes");
                    if (Request["idServicio"].ToString() == "6") VerificaPermisos("Recepción de Muestras");
                    Session["idServicio"] = Request["idServicio"].ToString(); 

                }
                if (Request["idUrgencia"] != null)   Session["idUrgencia"] = Request["idUrgencia"].ToString();
                ///idUrgencia=1 La sesion la creo para que cuando se acceda a nuevo paciente no se pierda que se trata de una urgencia.
                //idUrgencia=2 para el modulo urgencia.

                if (Session["idUrgencia"].ToString() != "0") imgUrgencia.Visible = true;  else imgUrgencia.Visible = false;
                if (Request["idUsuario"] != null) Session["idUsuario"] = Request["idUsuario"].ToString();


               if (ConfigurationManager.AppSettings["urlPaciente"].ToString() != "0")                    
                //{
                    
                //    //string s_urlLabo = ConfigurationManager.AppSettings["urlLabo"].ToString();
                //    //string s_urlAlta = s_urlLabo + "Protocolos/ProtocoloEdit2.aspx?idPaciente=IdPaciente&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString() + "&Operacion=Alta";
                   hplNuevoPaciente.NavigateUrl = ConfigurationManager.AppSettings["urlPaciente"].ToString() + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(); 
                //}
                else
                    hplNuevoPaciente.NavigateUrl = "../../sips/Paciente/PacienteEdit.aspx?id=0&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString();


                switch (Session["idServicio"].ToString())
                { case "3": lblServicio.Text = "MICROBIOLOGIA"; break;
                    case "1": lblServicio.Text = "LABORATORIO"; break;
                    case "4": lblServicio.Text = "PESQUISA NEONATAL"; break;

                    case "6":
                        {
                            lblServicio.Text = "FORENSE";
                            pnlTitulo.Attributes.Add("class", "panel panel-success");
                            pnlTitulo2.Attributes.Add("class", "panel panel-success");
                                gvLista.HeaderStyle.BackColor = System.Drawing.Color.LightGreen;
                                btnBuscar.CssClass = "btn btn-success";
                              



                           
                        }
                        break;


                }


                if (Request["Operacion"] == "Modifica")
                {
                    lblTitulo.Text = "ACTUALIZACION PROTOCOLO";
                    pnlTitulo.Visible = false;
                    ProtocoloList1.Visible = false;
                    //hplNuevoPaciente.Visible = false;
                    //pnlProtocolos.Visible = false;                    

                    hplNuevoPaciente.NavigateUrl += "&idProtocolo=" + Request["idProtocolo"].ToString()+ "&Desde=" + Request["Desde"].ToString(); ;
                    if (Request["PacienteRetorno"] != null)
                        Response.Redirect(s_pagina + "?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString());
                }
                else
                { lblTitulo.Text = "NUEVO PROTOCOLO";
                    if (Request["idServicio"].ToString() == "6")
                    {
                        if (Request["idCaso"]!= null)
                            Session["idCaso"] = Request["idCaso"].ToString();
                        lblTitulo.Text = "Paso 2: Identificacion de Persona  - Caso Nro. " + Session["idCaso"].ToString();
                        lblTituloLista.Text = "Protocolos del Caso de Filiacion";

                    }
                   
                    ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
                    //hplNuevoPaciente.Visible = true;
                    //   CargarGrillaProtocolo();
                
                    //if (Session["idServicio"].ToString() == "3")//microbiologia
                    //    lblServicio.Text = "MICROBIOLOGIA";
                    //if (Session["idServicio"].ToString() == "1")
                    //    lblServicio.Text = "LABORATORIO";
                    //if (Session["idServicio"].ToString() == "4")
                    //    lblServicio.Text = "PESQUISA NEONATAL";

                    if (Request["PacienteRetorno"] != null)
                    {                      
                            Response.Redirect(s_pagina+"?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Operacion=Alta",false);
                    }

                    if (Session["idServicio"].ToString() != "6")
                    {
                        Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                        if (oCon.PeticionElectronica) PeticionList.CargarPeticiones();
                    }
                }

              

            }
            

        }
        protected void btnSeleccionarPaciente_Click(object sender, EventArgs e)
        {
            //lblPaciente.Visible = true;
            //lblPaciente.Text = "Se ha seleccionado el paciente con ID " + hfPaciente.Value;
            RedireccionarProtocolo(hfPaciente.Value);
            // Buscar paciente en base del hospital y lo agrega en la base del laboratorio y redirecciona
        }


        protected void btnSeleccionarPacienteInternado_Click(object sender, EventArgs e)
        {
            //lblPaciente.Visible = true;
            //lblPaciente.Text = "Se ha seleccionado el paciente con ID " + hfPaciente.Value;
            RedireccionarProtocolo(hfPaciente.Value);
        }

        private void RedireccionarProtocolo(string p)
        {


            /////inserta el paciente en la tabla Sys_Paciente            
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "LAB_AgregaPacienteHospital";


            //cmd.Parameters.Add("@historiaClinica", SqlDbType.NVarChar);
            //cmd.Parameters["@historiaClinica"].Value = p.ToString();

            //cmd.Connection = conn;

            //SqlDataAdapter da = new SqlDataAdapter(cmd);

            //cmd.ExecuteNonQuery();
            ///// Busca el paciente recien insertado y continua proceso
            string s_pagina = "ProtocoloEdit2.aspx"; string nrocaso = "";
            if (Request["idServicio"].ToString() == "4") s_pagina = "ProtocoloEditPesquisa.aspx";

            if (Request["idServicio"].ToString() == "6")
            {
                s_pagina = "ProtocoloEditForense.aspx";
                 nrocaso ="&idCaso="+ Session["idCaso"].ToString();
            }

            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "HistoriaClinica",int.Parse( p));
            if (oPaciente != null)
            {
                if (Request["Operacion"] != "Modifica")
                {
                    if (Request["urgencia"] != null)
                        Response.Redirect(s_pagina+"?idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=Alta&idServicio=1&Urgencia=" + Request["urgencia"].ToString());
                    else
                        Response.Redirect(s_pagina + "?idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString()+ nrocaso);
                }
                else
                    Response.Redirect(s_pagina+"?Desde=" + Request["Desde"].ToString() + "&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString());



            }
        }


        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            string s_idServicio = Request["idServicio"].ToString();
            HyperLink oHplInfo = (HyperLink)e.Item.FindControl("hplProtocoloEdit");
            if (oHplInfo != null)
            {
                string s_pagina="ProtocoloEdit2.aspx";
                if (s_idServicio == "4")                
                    s_pagina = "ProtocoloEditPesquisa.aspx";
                if (s_idServicio == "6")
                    s_pagina = "ProtocoloEditForense.aspx";


                string idProtocolo = oHplInfo.NavigateUrl;
                oHplInfo.NavigateUrl =s_pagina +"?idServicio=" +s_idServicio + "&Desde=Default&Operacion=Modifica&idProtocolo="+ idProtocolo ;
                HyperLink oHplNuevoLaboratorio = (HyperLink)e.Item.FindControl("lnkNuevoProtocolo");
                        if (oHplNuevoLaboratorio != null)
                        {
                            if (s_idServicio== "4") oHplNuevoLaboratorio.Visible = false;
                            else
                            {
                                oHplNuevoLaboratorio.Visible = true;
                                Label oIdPaciente = (Label)e.Item.FindControl("lblidPaciente");
                                oHplNuevoLaboratorio.NavigateUrl = s_pagina + "?idPaciente=" + oIdPaciente.Text + "&Operacion=Alta&idServicio="+s_idServicio+"&Urgencia=0";
                            }
                        }


                Label oMuestra = (Label)e.Item.FindControl("lblTipoMuestra");
                if (Request["idServicio"].ToString() != "3")///laboratorio o pesquisa
                {
                      
                        oMuestra.Visible = false;
                        HyperLink oHplBacteriologia = (HyperLink)e.Item.FindControl("lnkMicrobiologia");
                        if (oHplBacteriologia != null)
                        {
                            if (Request["idServicio"].ToString() == "4") oHplBacteriologia.Visible = false;
                            else
                            {
                            oHplBacteriologia.Visible = true;
                            Label oIdPaciente= (Label)e.Item.FindControl("lblidPaciente");                                                                                 
                            oHplBacteriologia.NavigateUrl = s_pagina+ "?idPaciente="+oIdPaciente.Text+"&Operacion=Alta&idServicio=3&Urgencia=0";
                            }
                        }

                       



                }                    
                else
                    oMuestra.Visible = true;
            }
        }


        //private void CargarGrillaProtocolo()
        //{
       
        //    DataList1.DataSource = LeerDatosProtocolos();
        //    DataList1.DataBind();
        //}
        

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                        case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarGrilla()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            //CargarGrillaProtocolo();
        }

        //private object LeerDatosProtocolos()
        //{
        //    if (Session["idServicio"] == null) Session["idServicio"] = 1;

        //    string str_condicion = " WHERE P.baja=0 and P.idTipoServicio=" + Session["idServicio"].ToString();
        //    DateTime fecha1 = DateTime.Today;
        //    if (Request["urgencia"] != null)
        //    {
        //        str_condicion += " and P.idPrioridad=2 ";
        //    }

        //    string m_strSQL = " SELECT  TOP (10) dbo.NumeroProtocolo (idProtocolo) AS numero, Pa.apellido + ' ' + Pa.nombre AS paciente, U.username ,P.idProtocolo ,P.idTipoServicio," +
        //    " Pa.numeroDocumento, P.idPaciente as idPaciente, P.fechaRegistro , TP.nombre as muestra " +
        //    " FROM         dbo.LAB_Protocolo AS P " +
        //    " INNER JOIN   dbo.Sys_Paciente AS Pa ON Pa.idPaciente = P.idPaciente " +
        //    " INNER JOIN   dbo.Sys_Usuario AS U ON U.idUsuario = P.idUsuarioRegistro " +
        //    " LEFT JOIN  dbo.LAB_Muestra AS TP ON TP.idMuestra = P.idMuestra " +
        //    str_condicion +
        //    " order by P.idProtocolo desc ";

        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);

        //    return Ds.Tables[0];
            
        //}

        private object LeerDatos()
        {
            Utility oUtil = new Utility();
            string m_strSQL = ""; string str_condicionMadre = "";
            string str_condicion = "";

            if (Request["PacienteRetorno"] != null) str_condicion += " AND Pa.idPaciente = " + Request["PacienteRetorno"].ToString();
            if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value.Trim() + "'";
            if (txtApellido.Text != "") str_condicion += " AND Pa.apellido like '%" +oUtil.SacaComillas( txtApellido.Text.TrimEnd()) + "%'";
            if (txtNombre.Text != "") str_condicion += " AND Pa.nombre like '%" +oUtil.SacaComillas( txtNombre.Text.TrimEnd()) + "%'";
            if (txtFechaNac.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaNac.Value);
                str_condicion += " AND Pa.fechaNacimiento='" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (ddlSexo.SelectedValue != "0")str_condicion += " AND Pa.idSexo=" + ddlSexo.SelectedValue;
            ///////////////////////////////Condicion para buscar por la madre/tutor///////////////             
            if ((txtDniMadre.Value != "") || (txtApellidoMadre.Text != "") || (txtNombreMadre.Text != ""))
            {
                str_condicionMadre = " and Pa.idPaciente in (Select idPaciente FROM  Sys_Parentesco WHERE  1=1 ";
                if (txtDniMadre.Value != "") str_condicionMadre += " AND numeroDocumento = '" + txtDniMadre.Value.Trim() + "'";
                if (txtApellidoMadre.Text != "") str_condicionMadre += " AND apellido like '%" + oUtil.SacaComillas( txtApellidoMadre.Text) + "%'";
                if (txtNombreMadre.Text != "") str_condicionMadre += " AND nombre like '%" + oUtil.SacaComillas(txtNombreMadre.Text) + "%'";
                str_condicionMadre += " ) ";
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            m_strSQL = " SELECT Pa.idPaciente, Pa.numeroDocumento as dni,Pa.apellido+ ' ' + Pa.nombre as paciente, convert(varchar(10),Pa.fechaNacimiento,103) as fechaNacimiento, " +
                        " case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo " +
                        " FROM Sys_Paciente Pa " +
                        " WHERE Pa.idEstado<4 " + str_condicion + str_condicionMadre +
                        " order by paciente";
      
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[0].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modificar datos paciente";

                Button CmdProtocolo = (Button)e.Row.Cells[5].FindControl("Protocolo");
                CmdProtocolo.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdProtocolo.CommandName = "Protocolo";
                if (Request["Operacion"] == "Modifica")
                    CmdProtocolo.ToolTip = "Asignar Paciente";
                else
                CmdProtocolo.ToolTip = "Nuevo Protocolo";


               

                 Button btnEditar = (Button)e.Row.Cells[5].FindControl("Consentimiento");
                btnEditar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                btnEditar.CommandName = "Consentimiento";
                btnEditar.ToolTip = "Consentimiento";

                if ((Request["idServicio"].ToString() == "6") && (oC.HabilitaConsentimientoMicrobiologia))
                {
                    btnEditar.Visible = true;
                    CmdProtocolo.Visible = false;
                }
                else
                    btnEditar.Visible = false;

                if (Request["Operacion"] == "Modifica")
                {
                    CmdProtocolo.Text = "Asignar"; // asigna paciente a protocolo
                    btnEditar.Visible = false;
                }

            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Protocolo":
                    {
                        string s_pagina = "ProtocoloEdit2.aspx"; string nrocaso = "";
                        if (Request["idServicio"].ToString() == "4") s_pagina = "ProtocoloEditPesquisa.aspx";
                        if (Request["idServicio"].ToString() == "6")
                        {
                            nrocaso = "&idCaso=" + Session["idCaso"].ToString();
                            s_pagina = "ProtocoloEditForense.aspx";
                        }

                        if (Request["Operacion"] != "Modifica")
                        {
                            //VerificarSiTienePeticion (reenvia a elegir la peticion a descargar)
                            if (Request["urgencia"] != null)
                                Response.Redirect(s_pagina+"?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio=1&Urgencia="+ Request["urgencia"].ToString());
                            else
                                Response.Redirect(s_pagina + "?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString()+nrocaso);
                        }
                        else
                            Response.Redirect(s_pagina+"?Desde=" + Request["Desde"].ToString() + "&idPaciente=" + e.CommandArgument + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() +"&idServicio=" + Session["idServicio"].ToString() );

                    }
                    break;

                case "Consentimiento":
                    {
                        string s_pagina = "Consentimiento.aspx?idPaciente=" + e.CommandArgument.ToString() + "&idServicio=" + Request["idServicio"].ToString();
                        Response.Redirect(s_pagina, false);
                        break;
                    }
                case "Modificar":                                  

                    //if (ConfigurationManager.AppSettings["urlPaciente"].ToString() != "0")
                    //{
                    //    Response.Redirect("../Pacientes/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboProtocolo&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);                        
                    //    //string s_urlLabo = ConfigurationManager.AppSettings["urlLabo"].ToString();
                    //    //string s_urlAlta =s_urlLabo+  "Protocolos/ProtocoloEdit2.aspx?idPaciente=IdPaciente&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString() + "&Operacion=Alta";
                    //    //Response.Redirect(ConfigurationManager.AppSettings["urlPaciente"].ToString() + "/modifica/?idPaciente=" + e.CommandArgument + "&url=" + s_urlAlta, false);
                    //}
                    //else
                    string urlPaciente=ConfigurationManager.AppSettings["urlPaciente"].ToString().Replace("id=0", "id=" + e.CommandArgument);
                    Response.Redirect(urlPaciente+"&llamada=LaboProtocolo&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                        //Response.Redirect("../Pacientes/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboProtocolo&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);                        
                        
                    //    if (                    //    ConfigurationSettings.AppSettings["PacIntegrado"].ToString() == "1")
                    //    Response.Redirect("//" + ConfigurationSettings.AppSettings["server"] + "/principal/Paciente/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                    //else
                    //    Response.Redirect("../Pacientes/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);                        

                    
                           
                           break;


            }
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
          if (Page.IsValid)
            CargarGrilla();
        }

        protected void cvDatosEntrada_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtDni.Value == "")
                if (txtApellido.Text == "")
                    if (txtNombre.Text == "")
                        if (txtDniMadre.Value == "")
                            if (txtApellidoMadre.Text == "")
                                if (txtNombreMadre.Text == "")
                                    args.IsValid = false;
                                else
                                    args.IsValid = true;
                            else
                                args.IsValid = true;                             
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Pacientes/PacienteEdit.aspx?id=0", false);
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
                string exception = "";
                //while (ex != null)
                //{
                    exception = ex.Message + "<br>";

                //} 
            args.IsValid = false;
            }
        }

        protected void btnFinalizarCaso_Click(object sender, EventArgs e)
        {
            CrystalReportSource oCr = new CrystalReportSource();
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));


            ParameterDiscreteValue nrocaso = new ParameterDiscreteValue();
            nrocaso.Value = oRegistro.IdCasoFiliacion.ToString();

            ParameterDiscreteValue nombre = new ParameterDiscreteValue();
            nombre.Value = oRegistro.IdCasoFiliacion.ToString() + " " + oRegistro.Nombre;


            oCr.Report.FileName = "..\\CasoFiliacion\\CaratulaFiliacion.rpt";
          
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(nrocaso);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(nombre);


            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Caratula");
        }
    }
}
