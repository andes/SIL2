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

namespace WebLab.Neonatal
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!Page.IsPostBack)
            {


                VerificaPermisos("Pesquisa Neonatal");
            
                if (Request["idServicio"] != null)   Session["idServicio"] = Request["idServicio"].ToString();

                if (Request["idUrgencia"] != null)   Session["idUrgencia"] = Request["idUrgencia"].ToString();
                ///idUrgencia=1 La sesion la creo para que cuando se acceda a nuevo paciente no se pierda que se trata de una urgencia.
                //idUrgencia=2 para el modulo urgencia.

                //if (Session["idUrgencia"].ToString() != "0") imgUrgencia.Visible = true;
                //else imgUrgencia.Visible = false;


                if (Request["idUsuario"] != null) Session["idUsuario"] = Request["idUsuario"].ToString();

                //if (ConfigurationSettings.AppSettings["PacIntegrado"].ToString() == "1")
                //    HyperLink1.NavigateUrl = "//"+ ConfigurationSettings.AppSettings["server"]+"/principal/Paciente/PacienteEdit.aspx?id=0&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString();
                //else
                    //HyperLink1.NavigateUrl = "../Pacientes/PacienteEdit.aspx?id=0&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString();

                //if (Request["Operacion"] == "Modifica")
                //{
                //    lblTitulo.Text = "ACTUALIZACION PROTOCOLO";
                //    if (Session["idServicio"].ToString() == "3")//microbiologia
                //        lblServicio.Text = "MICROBIOLOGIA";
                //    else
                //        lblServicio.Text = "LABORATORIO";
                //    pnlProtocolos.Visible = false;                    
                //    if (Request["PacienteRetorno"] != null)
                //        Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Operacion=Modifica&idProtocolo="+ Request["idProtocolo"].ToString());
                //}
                //else
                //{
                //    lblTitulo.Text = "NUEVO PROTOCOLO";
                //    CargarGrillaProtocolo();

                //    if (Session["idServicio"].ToString() == "3")//microbiologia
                //        lblServicio.Text = "MICROBIOLOGIA";
                //    else
                //        lblServicio.Text = "LABORATORIO";

                //    if (Request["PacienteRetorno"] != null)
                //    {                      
                //            Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Operacion=Alta",false);
                //    }
                //}
            
              
            }
            

        }

        private void CargarGrillaProtocolo()
        {
       
            //DataList1.DataSource = LeerDatosProtocolos();
            //DataList1.DataBind();
        }
        

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
           // CargarGrillaProtocolo();
        }

        //private object LeerDatosProtocolos()
        //{
        //    string str_condicion = " WHERE P.baja=0  and P.idTipoServicio=" + Session["idServicio"].ToString();
        //    DateTime fecha1 = DateTime.Today;
        //    if (Request["urgencia"] != null)
        //    {
        //        str_condicion += " and P.idPrioridad=2 ";
        //    }

        //    string m_strSQL = " SELECT     TOP (10) dbo.NumeroProtocolo (idProtocolo) AS numero, Pa.apellido + ' ' + Pa.nombre AS paciente, U.username ,P.idProtocolo ,P.idTipoServicio," +
        //    " Pa.numeroDocumento, P.idPaciente as idPaciente, P.fechaRegistro  " +
        //    " FROM         dbo.LAB_Protocolo AS P " +
        //    " INNER JOIN   dbo.Sys_Paciente AS Pa ON Pa.idPaciente = P.idPaciente " +
        //    " INNER JOIN   dbo.Sys_Usuario AS U ON U.idUsuario = P.idUsuarioRegistro " +
        //    " INNER JOIN  dbo.LAB_Configuracion AS C ON P.idEfector = C.idEfector " + 
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
            //string m_strSQL = "";
            //string str_condicion = "";

            //if (Request["PacienteRetorno"] != null) str_condicion += " AND Pa.idPaciente = " + Request["PacienteRetorno"].ToString();           
            //if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value + "'";
            //if (txtApellido.Text != "") str_condicion += " AND Pa.apellido like '%" + txtApellido.Text.TrimEnd() + "%'";
            //if (txtNombre.Text != "") str_condicion += " AND Pa.nombre like '%" + txtNombre.Text.TrimEnd() + "%'";

            //m_strSQL = " SELECT Pa.idPaciente,Pa.numeroDocumento as dni,Pa.apellido+ ' ' + Pa.nombre as paciente, convert(varchar(10),Pa.fechaNacimiento,103) as fechaNacimiento, " +
            //            " case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo " +
            //            " FROM Sys_Paciente Pa " +
            //            " WHERE Pa.idEstado<4 " + str_condicion+
            //            " order by paciente";

            string str_condicion = ""; string str_condicionMadre = "";
            //if (txtFechaDesde.Value != "")
            //{
            //    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            //    str_condicion += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "'";
            //}
            //if (txtFechaHasta.Value != "")
            //{
            //    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
            //    str_condicion += " AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
            //}

            if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value + "'";
            if (txtApellido.Text != "") str_condicion += " AND Pa.apellido like '%" + txtApellido.Text + "%'";
            if (txtNombre.Text != "") str_condicion += " AND Pa.nombre like '%" + txtNombre.Text + "%'";

            ///////////////////////////////Condicion para buscar por la madre/tutor///////////////             
            if ((txtDniMadre.Value != "") || (txtApellidoMadre.Text != "") || (txtNombreMadre.Text != ""))
            {
                str_condicionMadre = " and P.idPaciente in (Select idPaciente FROM  Sys_Parentesco WHERE  1=1 ";
                if (txtDniMadre.Value != "") str_condicionMadre += " AND numeroDocumento = '" + txtDniMadre.Value + "'";
                if (txtApellido.Text != "") str_condicionMadre += " AND apellido like '%" + txtApellidoMadre.Text + "%'";
                if (txtNombre.Text != "") str_condicionMadre += " AND nombre like '%" + txtNombreMadre.Text + "%'";
                str_condicionMadre += " ) ";
            }
            /////////////////////////////////////////////////////////////////////////////////////////

          //  if (Request["Tipo"].ToString() == "PacienteValidado") str_condicion += " and P.estado>0 AND (DP.idUsuarioValida > 0) ";

            /////////////////////////////////////////////////////////////////////////////////////////

            string m_strSQL = " SELECT distinct P.idPaciente, Pa.numeroDocumento as dni, Pa.apellido + ', ' + Pa.nombre as paciente, " +
                              " convert(varchar(10), Pa.fechaNacimiento,103) as fechaNacimiento, case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo " +
                              " FROM LAB_Protocolo P " +
                              " INNER JOIN Sys_Paciente Pa ON Pa.idPaciente= P.idPaciente " +                           
                              " WHERE P.baja=0 " + str_condicion + str_condicionMadre +
                              " ORDER BY Pa.numeroDocumento, paciente";
      
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

                ImageButton CmdProtocolo = (ImageButton)e.Row.Cells[5].Controls[1];
                CmdProtocolo.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdProtocolo.CommandName = "Screening";
                if (Request["Operacion"] == "Modifica")
                    CmdProtocolo.ToolTip = "Asignar Paciente";
                else
                CmdProtocolo.ToolTip = "Nuevo Protocolo";
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Screening":
                    {
                        //if (Request["Operacion"] != "Modifica")
                        //{
                        //    if (Request["urgencia"] != null)
                        //        Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio=1&Urgencia="+ Request["urgencia"].ToString());
                        //    else
                        //        Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + e.CommandArgument + "&Operacion=Alta&idServicio="+ Request["idServicio"].ToString());
                        //}
                        //else
                            Response.Redirect("IngresoEdit.aspx?idPaciente=" + e.CommandArgument );

                    }
                    break;
                case "Modificar":
                    if (ConfigurationSettings.AppSettings["PacIntegrado"].ToString() == "1")
                        Response.Redirect("//" + ConfigurationSettings.AppSettings["server"] + "/principal/Paciente/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);
                    else
                        Response.Redirect("../Pacientes/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=" + Session["idUrgencia"].ToString(), false);                        

                    
                           //HyperLink1.NavigateUrl = "//" + ConfigurationSettings.AppSettings["server"] + "/principal/Paciente/PacienteEdit.aspx?id=0&llamada=LaboProtocolo&idUsuario=" + Session["idUsuario"].ToString() + "&idServicio=" + Request["idServicio"].ToString() + "&idUrgencia=" + Request["idUrgencia"].ToString();
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

       
    }
}
