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
    public partial class Default2 : System.Web.UI.Page
    {

        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {

            //MiltiEfector: Filtra para configuracion del efector del usuario 
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
               

                string s_pagina = "ProtocoloEdit2.aspx";
                if (Request["idServicio"].ToString() == "4") s_pagina = "ProtocoloEditPesquisa.aspx";
                if (Request["idServicio"].ToString() == "6")
                {
                    if (Request["idCaso"]!=null) 
                    {
                        Session["idCaso"] = Request["idCaso"].ToString(); // "0";
                        btnFinalizarCaso.Visible = true;
                        ProtocoloList1.Visible = false;
                    }
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

 

                switch (Request["idServicio"].ToString())
                { case "3": lblServicio.Text = "MICROBIOLOGIA"; break;
                    case "1": lblServicio.Text = "LABORATORIO"; break;
                    case "4": lblServicio.Text = "PESQUISA NEONATAL"; break;

                    case "6":
                        {
                            lblServicio.Text = "FORENSE";
                            pnlTitulo.Attributes.Add("class", "panel panel-success");
                            pnlTitulo2.Attributes.Add("class", "panel panel-success");
                                //gvLista.HeaderStyle.BackColor = System.Drawing.Color.LightGreen;
                                btnBuscar.CssClass = "btn btn-success";
                            //if (Session["idCaso"] == null)
                            //    Session["idCaso"] = "0";




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

                    //hplNuevoPaciente.NavigateUrl += "&idProtocolo=" + Request["idProtocolo"].ToString()+ "&Desde=" + Request["Desde"].ToString(); ;
                    if (Request["PacienteRetorno"] != null)
                        Response.Redirect(s_pagina + "?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString());
                }
                else
                {
                    lblTitulo.Text = "NUEVO PROTOCOLO";
                    if (Request["idServicio"].ToString() == "6")
                    {
                        btnSinPersona.Visible = true;
                        lblTitulo.Text = "Identificación de Muestra Genética Forense";

                        if (Session["idCaso"] != null)
                        {

                            if (Session["idCaso"].ToString() != "0")
                            {
                                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));


                                lblTitulo.Text = "Paso 2: Identificación de Persona  - Caso Nro. " + Session["idCaso"].ToString();
                                if (oRegistro.IdTipoCaso == 1)
                                {
                                    btnSinPersona.Visible = false;

                                    lblTituloLista.Text = "Muestras del Caso de Filiación";
                                }
                                if (oRegistro.IdTipoCaso == 2)
                                {
                                    btnSinPersona.Visible = true;
                                    lblTituloLista.Text = "Muestras del Caso de Pericia Forense";
                                }
                                if (oRegistro.IdTipoCaso == 3)
                                {
                                    btnSinPersona.Visible = false;
                                    lblTituloLista.Text = "Muestras del Caso de Quimerismo";
                                }
                            }

                        }
                        else
                        {
                            btnSinPersona.Visible = true;
                            lblTitulo.Text = "Identificación de Muestra Genética Forense";
                        }
                    }

                    //if ((Request["idServicio"].ToString() == "6") && (Session["idCaso"] != null))
                        ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
                    //else
                    //{
                    //    ProtocoloList1.Visible = false;
                    //    btnFinalizarCaso.Visible = false;
                    //}



                    if (Request["PacienteRetorno"] != null)
                    {                      
                            Response.Redirect(s_pagina+"?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Operacion=Alta",false);
                    }

                    if (Request["idServicio"].ToString() != "6")
                    {
                     //   Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                        if (oC.PeticionElectronica) PeticionList.CargarPeticiones();
                    }
                }

              

            }
            

        }
        protected void btnSeleccionarPaciente_Click(object sender, EventArgs e)
        {
            //lblPaciente.Visible = true;
            //lblPaciente.Text = "Se ha seleccionado el paciente con ID " + hfPaciente.Value;
            //RedireccionarProtocolo(hfPaciente.Value);
            // Buscar paciente en base del hospital y lo agrega en la base del laboratorio y redirecciona
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }
        protected void btnSeleccionarPacienteInternado_Click(object sender, EventArgs e)
        {
            //lblPaciente.Visible = true;
            //lblPaciente.Text = "Se ha seleccionado el paciente con ID " + hfPaciente.Value;
            //RedireccionarProtocolo(hfPaciente.Value);
        }

        private void RedireccionarProtocolo(string p)
        { 
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
        //private String TicketUsuario = "";
        //private String TicketSistema = "";
        //private String operador = "";

        ////este codigo debe representar a la aplicación cliente
        ////se usa el usuario "USUARIO-TEST" preparado para realizar pruebas
        //String SistemaCliente = "SISTEMA_LAB_CENTRAL";
        //String PasswordSistemaCliente = "Gogo**885";


        private void CargarGrillaAmpliada()
        {
           
                gvLista.DataSource = LeerDatosAdicionales();
                gvLista.DataBind();


            if (gvLista.Rows.Count > 0)
            {

                lblMensaje.Text = "Se muestran los primeros 10 coincidencias encontradas. Para acotar su busqueda ingrese mas filtros.";
                lblMensaje.Visible = true;

            }
            else lblMensaje.Visible = false;












        }
        private void CargarGrilla ()
        {
            string m_sexo = "F";
            if (ddlSexo.SelectedValue == "2")
                m_sexo = "F";

            if (ddlSexo.SelectedValue == "3")
                m_sexo = "M";
            if (ddlSexo.SelectedValue == "4")
                m_sexo = "X";



            if ((ddlTipo.SelectedValue == "DNI") && (txtDni.Value != ""))
            {
                gvLista.DataSource = LeerDatos();
                gvLista.DataBind();


                if (gvLista.Rows.Count == 0)
                {
                    lblMensaje.Visible = false;


                    Response.Redirect("ProcesaRenaper.aspx?Tipo=" + ddlTipo.SelectedValue + "&dni=" + txtDni.Value + "&sexo=" + m_sexo + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString());


                }
                else
                    lblMensaje.Visible = true;
            }


            if ((ddlTipo.SelectedValue == "T"))
            { //temporal
                if ((txtDni.Value != "") || (txtNumeroAdicional.Text != ""))
                {
                    gvLista.DataSource = LeerDatos();
                    gvLista.DataBind();


                    if (gvLista.Rows.Count == 0)
                    {
                        lblMensaje.Visible = false;


                        Response.Redirect("ProcesaRenaperT.aspx?Tipo=" + ddlTipo.SelectedValue + "&dni=" + txtDni.Value + "&sexo=" + m_sexo + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString());


                    }
                    else
                        lblMensaje.Visible = true;
                }

                else

                    Response.Redirect("ProcesaRenaperT.aspx?Tipo=" + ddlTipo.SelectedValue + "&dni=" + txtDni.Value + "&sexo=" + m_sexo + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString());
            }







            if ((ddlTipo.SelectedValue == "DNI") && (txtDni.Value == ""))
            {
                lblMensaje.Text = "Debe ingresar un numero de documento";
                lblMensaje.Visible = true;

            }



        }


        private object LeerDatos()
        {
            Utility oUtil = new Utility();
            string m_strSQL = ""; string str_condicionMadre = "";
            string str_condicion = "";

            //if (Request["PacienteRetorno"] != null) str_condicion += " AND Pa.idPaciente = " + Request["PacienteRetorno"].ToString();


            if (ddlTipo.SelectedValue == "DNI")
            {
                str_condicion += " and Pa.idEstado in (1,3)";
                if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value.Trim() + "'";

            }
            else
            {
                str_condicion += " and Pa.idEstado in (2)"; // temporal
                if (txtDni.Value != "") str_condicion += " AND (Pa.numeroDocumento = '" + txtDni.Value.Trim() + "')";
                if (txtNumeroAdicional.Text != "")  str_condicion +=" and (Pa.numeroAdic like '%" + txtNumeroAdicional.Text.Trim() + @"%'  
                                                                        or Pa.idPaciente in (select idPaciente from sys_parentesco (nolock) where numeroDocumento="+txtNumeroAdicional.Text.Trim()+"))";
            }

            m_strSQL = " SELECT top 10 Pa.idPaciente,  case when Pa.idestado=2 then convert(varchar,Pa.numeroAdic) + '-T' else convert(varchar,Pa.numeroDocumento)  end as dni,Pa.apellido+ ' ' + Pa.nombre as paciente, convert(varchar(10),Pa.fechaNacimiento,103) as fechaNacimiento, " +
                        " case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo, " +
                        " case Pa.idestado when 1 then 'IDENTIFICADO' when 2 then 'TEMPORAL' when 3 then 'VALIDADO' end as estado " +
                        " FROM Sys_Paciente Pa (nolock) " +
                        " WHERE 1=1 " + str_condicion + str_condicionMadre +
                        " order by paciente";
      
            DataSet Ds = new DataSet();
            ///SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


           
            return Ds.Tables[0];
        }


        private object LeerDatosAdicionales()
        {
            Utility oUtil = new Utility();
            string m_strSQL = ""; 
            string str_condicion = "";

           

            if (txtApellido.Text != "") str_condicion += " AND (Pa.apellido like '%" + txtApellido.Text + "%')";
            if (txtNombre.Text != "") str_condicion += " AND (Pa.nombre like '%" + txtNombre.Text + "%')";
            if (txtDniMadre.Value != "") str_condicion += " and  exists (select 1 from sys_parentesco Par with (nolock) where Pa.idpaciente=Par.idPaciente and numeroDocumento=" + txtDniMadre.Value + "))";
            if (txtApellidoMadre.Text != "") str_condicion += " and  exists (select 1 from sys_parentesco Par with (nolock) where Pa.idpaciente=Par.idPaciente and apellido=" + txtApellidoMadre.Text + "))";
            if (txtNombreMadre.Text != "") str_condicion += " and  exists (select 1 from sys_parentesco Par with (nolock) where Pa.idpaciente=Par.idPaciente and apellido=" + txtNombreMadre.Text + "))";

            m_strSQL = " SELECT top 10  Pa.idPaciente,  case when Pa.idestado=2 then convert(varchar,Pa.numeroAdic) + '-T' else convert(varchar,Pa.numeroDocumento)  end as dni,Pa.apellido+ ' ' + Pa.nombre as paciente, convert(varchar(10),Pa.fechaNacimiento,103) as fechaNacimiento, " +
                        " case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo, " +
                        " case Pa.idestado when 1 then 'IDENTIFICADO' when 2 then 'TEMPORAL' when 3 then 'VALIDADO' end as estado " +
                        " FROM Sys_Paciente Pa (nolock) " +
                        " WHERE 1=1 " + str_condicion + 
                        " order by paciente";

            DataSet Ds = new DataSet();
            ///SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
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

                Button CmdProtocolo = (Button)e.Row.Cells[6].FindControl("Protocolo");
                CmdProtocolo.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdProtocolo.CommandName = "Protocolo";
                if (Request["Operacion"] == "Modifica")
                    CmdProtocolo.ToolTip = "Asignar Paciente";
                else
                    CmdProtocolo.ToolTip = "Nuevo Protocolo";




                Button btnEditar = (Button)e.Row.Cells[6].FindControl("Consentimiento");
                btnEditar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                btnEditar.CommandName = "Consentimiento";
                btnEditar.ToolTip = "Consentimiento";

                if ((Request["idServicio"].ToString() == "6") && (oC.HabilitaConsentimientoMicrobiologia))
                {

                    if (Session["idCaso"] != null)
                    {
                        if (Session["idCaso"].ToString() != "0")
                        {
                            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));

                            if (oRegistro.IdTipoCaso == 1)// filiacion
                            {
                                btnEditar.Visible = true;
                                CmdProtocolo.Visible = false;

                            }
                            if ((oRegistro.IdTipoCaso == 2) || (oRegistro.IdTipoCaso == 3)) //forense o quimerismo
                            {
                                btnEditar.Visible = false; // consentimiento oculto
                                CmdProtocolo.Visible = true;
                            }
                        }
                        else
                        {
                            btnEditar.Visible = true;
                            CmdProtocolo.Visible = false;
                        }
                    }


                      
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
                                Response.Redirect(s_pagina + "?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio=1&Urgencia=" + Request["urgencia"].ToString());
                            else
                            {
                                // primero que valide renaper si es identificado y dsps al protocolo
                                Paciente oPac = new Paciente();
                                oPac = (Paciente)oPac.Get(typeof(Paciente), int.Parse(e.CommandArgument.ToString()));
                                if ((oPac.IdEstado == 1) && (oC.ConectaRenaper)) //identificado
                                {
                                    string m_sexo = "";
                                    switch (oPac.IdSexo)
                                    {
                                        case 1: m_sexo = "I"; break;
                                        case 2: m_sexo = "F"; break;
                                        case 3: m_sexo = "M"; break;
                                    }
                                    Response.Redirect("ProcesaRenaper.aspx?id=" + oPac.IdPaciente.ToString() + "&Tipo=" + ddlTipo.SelectedValue + "&dni=" + oPac.NumeroDocumento.ToString() + "&sexo=" + m_sexo + "&llamada=LaboProtocolo&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString());
                                }
                                else
                                    Response.Redirect(s_pagina + "?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString() + nrocaso);
                            }


                                //Response.Redirect(s_pagina + "?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString() + nrocaso);
                        }
                        else
                        {
                          
                            Response.Redirect(s_pagina + "?Desde=" + Request["Desde"].ToString() + "&idPaciente=" + e.CommandArgument + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString() + "&idServicio=" + Session["idServicio"].ToString());
                        }

                    }
                    break;

                case "Consentimiento":
                    {
                        string s_pagina = "Consentimiento.aspx?idPaciente=" + e.CommandArgument.ToString() + "&idTipoCaso="+  Request["idTipoCaso"].ToString() + "&idServicio=" + Request["idServicio"].ToString();
                        Response.Redirect(s_pagina, false);
                        break;
                    }
                case "Modificar":

                  
                    //string urlPaciente = ConfigurationManager.AppSettings["urlPaciente"].ToString().Replace("id=0", "id=" + e.CommandArgument);
                    //Response.Redirect(urlPaciente + "&llamada=LaboProtocolo&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);

                    Response.Redirect("../Pacientes/PacienteEdit2.aspx?Tipo=DNI&sexo=I&dni=0&id=" + e.CommandArgument.ToString()+"&llamada=LaboProtocolo&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);


                    break;


            }
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { CargarGrilla();}

            
        }

        protected void cvDatosEntrada_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtDni.Value == "")
            {
                 
                if (ddlSexo.SelectedValue == "")

                    args.IsValid = false;
                else
                    args.IsValid = true;
            }
            else
                args.IsValid = true;                             
        }

       

        protected void btnNuevoPaciente_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Pacientes/PacienteEdit.aspx?id=0", false);
        }

        protected void lnkAmpliarFiltros_Click(object sender, EventArgs e)
        {
            btnBuscarMas.Visible = false;
            if (lnkAmpliarFiltros.Text == "Ampliar filtros de búsqueda")
            {
                lnkAmpliarFiltros.Text = "Ocultar filtros adicionales";
                pnlParentesco.Visible = true;
                btnBuscarMas.Visible = true;
            }
            else
            {
                lnkAmpliarFiltros.Text = "Ampliar filtros de búsqueda";
                pnlParentesco.Visible = false;
                btnBuscarMas.Visible = false;
            }

            //lnkAmpliarFiltros.UpdateAfterCallBack = true;
            //pnlParentesco.UpdateAfterCallBack = true;
           // btnBuscarMas.UpdateAfterCallBack = true;
        }

        protected void cvFecha_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //try
            //{
            //    if (txtFechaNac.Value != "")
            //    {
            //        DateTime f1 = DateTime.Parse(txtFechaNac.Value);
            //        args.IsValid = true;
            //    }
            //    else                  
            //        args.IsValid = true;
            //}
            //catch (Exception ex)
            //{
            //    string exception = "";
            //    //while (ex != null)
            //    //{
            //        exception = ex.Message + "<br>";

            //    //} 
            //args.IsValid = false;
            //}
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

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipo.SelectedValue == "T")
            {
                txtDni.Value = "";
                ddlSexo.SelectedValue = "0";
                txtNumeroAdicional.Enabled = true;
                txtNumeroAdicional.Text = "";
                
            }
            else
            {
                //txtDni.Value = "";
                //ddlSexo.SelectedValue = "0";
                txtNumeroAdicional.Enabled = false;
                txtNumeroAdicional.Text = "";

            }

        }

        protected void btnSinPersona_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("../Protocolos/ProtocoloEditForense.aspx?idPaciente=-1&Operacion=Alta&idServicio=6&idUrgencia=0&idCaso=" + Session["idCaso"].ToString(), false);

        }

        protected void btnBuscarMas_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if ((txtApellido.Text == "") && (txtNombre.Text == "") && (txtDniMadre.Value == "") && (txtApellidoMadre.Text == "") && (txtNombreMadre.Text == ""))
                {
                    lblMensaje.Text = "Debe ingresar al menos un filtro adicional de busqueda";
                    lblMensaje.Visible = true;
                }
                else
                    CargarGrillaAmpliada();
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
    }
}
