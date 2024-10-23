using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;

namespace WebLab.Informes
{
    public partial class PacientesList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            }
        }

      
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) CargarGrilla();

        }

        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();

        }

        private object LeerDatos()
        {
            string str_condicion = ""; 
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                str_condicion += " AND P.Fecha>='" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                str_condicion += " AND P.fecha<='" + fecha2.ToString("yyyyMMdd") + "'";
            }

            //if (txtDni.Value != "") str_condicion += " AND Pa.numeroDocumento = '" + txtDni.Value + "'";
            //if (txtApellido.Text != "") str_condicion += " AND Pa.apellido like '%" + txtApellido.Text + "%'";
            //if (txtNombre.Text != "") str_condicion += " AND Pa.nombre like '%" + txtNombre.Text + "%'";
            /////////////////////////////////////////////////////////////////////////////////////////


            str_condicion += " and P.estado>0 AND (DP.idUsuarioValida > 0) ";
            str_condicion += " and P.IDPrioridad=2"; /// urgente

            if (rdbOrigen.SelectedValue!="0")
            str_condicion += " and P.IDoRIGEN="+ rdbOrigen.SelectedValue;
            
            //if (Request["Tipo"].ToString() == "Analisis")
            //{
            //    str_condicion += "  AND (DP.idUsuarioValida > 0) ";
            //    if (ddlItem.SelectedValue != "0")
            //        str_condicion += "  AND DP.idsubitem=" + ddlItem.SelectedValue;
            //}

            /////////////////////////////////////////////////////////////////////////////////////////

            ///////////////////////////////Condicion para buscar por la madre/tutor///////////////             
            //if ((txtDniMadre.Value != "") || (txtApellidoMadre.Text != "") || (txtNombreMadre.Text != ""))
            //{
            //    str_condicionMadre = " and P.idPaciente in (Select idPaciente FROM  Sys_Parentesco WHERE  1=1 ";
            //    if (txtDniMadre.Value != "") str_condicionMadre += " AND numeroDocumento = '" + txtDniMadre.Value + "'";
            //    if (txtApellido.Text != "") str_condicionMadre += " AND apellido like '%" + txtApellidoMadre.Text + "%'";
            //    if (txtNombre.Text != "") str_condicionMadre += " AND nombre like '%" + txtNombreMadre.Text + "%'";
            //    str_condicionMadre += " ) ";
            //}
            /////////////////////////////////////////////////////////////////////////////////////////
            string m_strSQL = " SELECT distinct P.idPaciente,case when Pa.idEstado=1 then Pa.numeroDocumento else 'TEMPORAL' END AS numeroDocumento, Pa.apellido + ', ' + Pa.nombre as paciente, " +
                              " convert(varchar(10), Pa.fechaNacimiento,103) as fechaNacimiento, case Pa.idSexo when 1 then 'Ind' when 2 then 'Fem' when 3 then 'Masc' end as sexo " +
                              " FROM LAB_Protocolo P " +
                              " INNER JOIN Sys_Paciente Pa ON Pa.idPaciente= P.idPaciente " +
                              " INNER JOIN LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo " +
                              " WHERE P.baja=0 " + str_condicion +
                              " ORDER BY paciente";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            return Ds.Tables[0];
        }


        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modificar":
                    {
                        string m_parametro = " P.idPaciente=" + e.CommandArgument.ToString();
                        //if (Request["Tipo"].ToString() == "PacienteValidado") 
                            m_parametro += " and P.estado>0 AND (DP.idUsuarioValida > 0) ";

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


                        //switch (Request["Tipo"].ToString())
                        //{
                        //    case "Analisis":                              //por analisis
                        //        Response.Redirect("HistoriaClinica.aspx?idPaciente=" + e.CommandArgument.ToString() + "&fechaDesde=" + txtFechaDesde.Value + "&fechaHasta=" + txtFechaHasta.Value + "&idAnalisis=" + ddlItem.SelectedValue); break;
                        //    case "PacienteCompleto":
                        //        Response.Redirect("../Resultados/Procesa.aspx?idServicio=1&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=0&modo=Normal", false); break;
                        //    case "PacienteValidado":
                                Response.Redirect("../Resultados/Procesa.aspx?idServicio=1&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal", false); 
                        //break;
                        //}




                        break;
                    }

            }

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[4].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Ver Historia Clínica";
            }

        }
    }
}