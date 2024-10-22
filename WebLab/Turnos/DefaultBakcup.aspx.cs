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
using Business.Data;

namespace WebLab.Turnos
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtDni.Focus();
                if (Request["idUsuario"] != null) Session["idUsuario"] = Request["idUsuario"].ToString();
               //if (ConfigurationManager.AppSettings["urlPaciente"].ToString() != "0")
                  
               //     HyperLink1.NavigateUrl = ConfigurationManager.AppSettings["urlPaciente"].ToString() + "&llamada=LaboTurno"; 
               // //}
               // else
               //     HyperLink1.NavigateUrl = "../../sips/Paciente/PacienteEdit.aspx?id=0&llamada=LaboTurno";
                if (Request["PacienteRetorno"] != null)
                    Response.Redirect("TurnosEdit2.aspx?idPaciente=" + Request["PacienteRetorno"].ToString() + "&Modifica=0",false);
                
            }
        }

        //protected void btnSeleccionarPaciente_Click(object sender, EventArgs e)
        //{
        //    //lblPaciente.Visible = true;
        //    //lblPaciente.Text = "Se ha seleccionado el paciente con ID " + hfPaciente.Value;
        //    RedireccionarProtocolo(hfPaciente.Value);
        //}

        private void RedireccionarProtocolo(string p)
        {
        //    ///inserta el paciente en la tabla Sys_Paciente            
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "LAB_AgregaPacienteHospital";


        //    cmd.Parameters.Add("@historiaClinica", SqlDbType.NVarChar);
        //    cmd.Parameters["@historiaClinica"].Value = p.ToString();

        //    cmd.Connection = conn;

        //    SqlDataAdapter da = new SqlDataAdapter(cmd);

        //    cmd.ExecuteNonQuery();
        //    /// Busca el paciente recien insertado y continua proceso
            
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "HistoriaClinica", p);
            if (oPaciente != null)
            {

                Response.Redirect("TurnosEdit2.aspx?idPaciente=" + oPaciente.IdPaciente.ToString() + "&Modifica=0", false);
                //if (Request["Operacion"] != "Modifica")
                //{
                //    if (Request["urgencia"] != null)
                //        Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=Alta&idServicio=1&Urgencia=" + Request["urgencia"].ToString());
                //    else
                //        Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString());
                //}
                //else
                //    Response.Redirect("ProtocoloEdit2.aspx?Desde=" + Request["Desde"].ToString() + "&idPaciente=" + oPaciente.IdPaciente.ToString() + "&Operacion=Modifica&idProtocolo=" + Request["idProtocolo"].ToString());
            }
        }
        private void CargarGrilla()
        {
            string m_sexo = "F";
            if (ddlSexo.SelectedValue == "2")
                m_sexo = "F";
            else
                m_sexo = "M";




            if ((ddlTipo.SelectedValue == "DNI") && (txtDni.Value != ""))
            {
                gvLista.DataSource = LeerDatos();
                gvLista.DataBind();


                if (gvLista.Rows.Count == 0)
                {
                    lblMensaje.Visible = false;


                    Response.Redirect("../Protocolos/ProcesaRenaper.aspx?Tipo=" + ddlTipo.SelectedValue + "&dni=" + txtDni.Value + "&sexo=" + m_sexo + "&llamada=LaboTurno&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0"); // + Session["idUrgencia"].ToString());


                }
                else
                    lblMensaje.Visible = true;
            }


            if ((ddlTipo.SelectedValue == "T"))
            { //temporal
                if (txtDni.Value != "")
                {
                    gvLista.DataSource = LeerDatos();
                    gvLista.DataBind();


                    if (gvLista.Rows.Count == 0)
                    {
                        lblMensaje.Visible = false;


                        Response.Redirect("../Protocolos/ProcesaRenaperT.aspx?Tipo=" + ddlTipo.SelectedValue + "&dni=" + txtDni.Value + "&sexo=" + m_sexo + "&llamada=LaboTurno&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0" );// + Session["idUrgencia"].ToString());


                    }
                    else
                        lblMensaje.Visible = true;
                }

                else

                    Response.Redirect("../Protocolos/ProcesaRenaperT.aspx?Tipo=" + ddlTipo.SelectedValue + "&dni=" + txtDni.Value + "&sexo=" + m_sexo + "&llamada=LaboTurno&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0"); // + Session["idUrgencia"].ToString());
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


            if (ddlTipo.SelectedValue == "DNI")
            {
                str_condicion += " and Pa.idEstado in (1,3)";
                if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value.Trim() + "'";

            }
            else
             
            {
                str_condicion += " and Pa.idEstado in (2)"; // temporal
                if (txtDni.Value != "") str_condicion += " AND (Pa.numeroDocumento = '" + txtDni.Value.Trim() + "')";                
                if (txtNumeroAdicional.Text != "") str_condicion += " and (Pa.numeroAdic like '%" + txtNumeroAdicional.Text.Trim() + @"%'  
                                                                        or Pa.idPaciente in (select idPaciente from sys_parentesco with (nolock) where numeroDocumento=" + txtNumeroAdicional.Text.Trim() + "))";
            }


            m_strSQL = @" SELECT Pa.idPaciente, convert(varchar,Pa.numeroDocumento) + case when Pa.idestado=2 then '-T' else '' end as dni,
                        Pa.apellido+ ' ' + Pa.nombre as paciente, convert(varchar(10),Pa.fechaNacimiento,103) as fechaNacimiento, 
                         case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo, 
                         case Pa.idestado when 1 then 'IDENTIFICADO' when 2 then 'TEMPORAL' when 3 then 'VALIDADO' end as estado 
                         FROM Sys_Paciente Pa with (nolock) 
                         WHERE 1=1 " + str_condicion + str_condicionMadre + @" order by paciente";

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
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
                CmdModificar.ToolTip = "Modificar datos del paciente";

                Button CmdTurno = (Button)e.Row.Cells[6].Controls[1];
                CmdTurno.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdTurno.CommandName = "Turno";
                CmdTurno.ToolTip = "Ingresar Turno";
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modificar":
                    //Response.Redirect("//"+ ConfigurationSettings.AppSettings["server"]+"/principal/Paciente/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboTurno&idUsuario=" + Session["idUsuario"].ToString() , false);
                    // Response.Redirect("../../sips/Paciente/PacienteEdit.aspx?id=" + e.CommandArgument + "&llamada=LaboTurno&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);                        
                    //string urlPaciente = ConfigurationManager.AppSettings["urlPaciente"].ToString().Replace("id=0", "id=" + e.CommandArgument);
                    // Response.Redirect(urlPaciente+"&llamada=LaboTurno&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);  
                    Response.Redirect("../Pacientes/PacienteEdit2.aspx?Tipo=DNI&sexo=I&dni=0&id=" + e.CommandArgument.ToString() + "&llamada=LaboTurno&idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0" , false);

                    break;


                case "Turno":
                    Response.Redirect("TurnosEdit2.aspx?idPaciente=" + e.CommandArgument + "&Modifica=0");
                    break;

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            CargarGrilla();
        }

       

        protected void cvDatosEntrada_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (txtDni.Value == "")
                if (ddlSexo.SelectedValue == "")

                    args.IsValid = false;
                else
                    args.IsValid = true;
            else
                args.IsValid = true;



        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvLista.PageIndex = e.NewPageIndex;
            CargarGrilla();
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
        }
    }
}
