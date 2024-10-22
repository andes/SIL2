using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Drawing;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;

namespace WebLab.PeticionElectronica
{
    public partial class PeticionLCList : System.Web.UI.Page
    {
  

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                { 
                //VerificaPermisos("Peticion");
                txtFechaDesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                CargarGrilla();
                }
                else Response.Redirect("../FinSesion.aspx", false);
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

                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


      

        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();     
        }

        private object LeerDatos()
        {
            string str_condicion = " where 1=1  ";

            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);

            str_condicion += " and PE.idUsuarioRegistro=" + Session["idUsuario"].ToString(); //solo  peticiones cargadas por mi.
            //if (ddlSectorServicio.SelectedValue != "0") str_condicion += " AND PE.idSector = " + ddlSectorServicio.SelectedValue;
            if (txtFechaDesde.Value != "") str_condicion += " AND PE.fecha>= '" + fecha1.ToString("yyyyMMdd") + "'"; //" AND convert(datetime,convert(varchar(10),PE.fecha,103))>= convert(datetime,convert(varchar(10),  '" + fecha1.ToString("yyyyMMdd") + "',103)) ";
            if (txtFechaHasta.Value != "") str_condicion += " AND PE.fecha<= '" + fecha2.ToString("yyyyMMdd") + "'";//" AND convert(datetime,convert(varchar(10),PE.fecha,103))<= convert(datetime,convert(varchar(10),  '" + fecha2.ToString("yyyyMMdd") + "',103)) ";
            if (txtNro.Text != "") str_condicion += " And PE.idPeticion='" + txtNro.Text + "'";
            //if (ddlOrigen.SelectedValue != "0") str_condicion += " AND PE.idOrigen = " + ddlOrigen.SelectedValue;
            if (txtDni.Value != "") str_condicion += " AND Pac.numeroDocumento = '" + txtDni.Value + "'";
            if (txtApellido.Text != "") str_condicion += " AND Pac.apellido like '%" + txtApellido.Text.TrimEnd() + "%'";
            if (txtNombre.Text != "") str_condicion += " AND Pac.nombre like '%" + txtNombre.Text.TrimEnd() + "%'";
            switch (ddlEstado.SelectedValue)
            {
                case "-1": str_condicion += " AND PE.baja=0 "; break;
                case "0": str_condicion += " AND PE.idProtocolo=0 AND PE.baja=0 "; break;
                case "1": str_condicion += " AND PE.idProtocolo>0 AND PE.baja=0 "; break;
                case "2": str_condicion += " AND P.estado=2 AND PE.baja=0 "; break; ///con resultados terminados
                case "3": str_condicion += " AND PE.baja=1 "; break;
            }
            //if (ddlEstado.SelectedValue == "0") str_condicion += " AND PE.idProtocolo=0 AND PE.baja=0 ";
            //if (ddlEstado.SelectedValue == "1") str_condicion += " AND PE.idProtocolo>0 AND PE.baja=0 ";
            //if (ddlEstado.SelectedValue == "2") str_condicion += " AND PE.baja=1 ";                 
          
            string m_strSQL = @"SELECT PE.idPeticion, Pac.numeroDocumento, pE.apellido, PE.nombre, CASE WHEN PE.idProtocolo > 0 THEN dbo.NumeroProtocolo(P.idProtocolo) 
            ELSE '-' END AS Protocolo, convert(varchar(10), PE.fecha,103) AS fecha, 
       dbo.LAB_GetEstadoPeticion(PE.idPeticion) as estado, O.nombre AS origen, S.nombre as sector, PE.observaciones,
            Pro.apellido + ' ' + Pro.nombre as solicitante, U.username as usuario
            FROM LAB_Peticion AS PE INNER JOIN
            Sys_Paciente AS Pac ON PE.idPaciente = Pac.idPaciente  INNER JOIN
            LAB_SectorServicio AS S ON PE.idSector = S.idSectorServicio INNER JOIN
            LAB_Origen AS O ON PE.idOrigen = O.idOrigen LEFT JOIN
            LAB_Protocolo AS P ON PE.idProtocolo = P.idProtocolo LEFT JOIN
            Sys_Profesional as Pro on Pro.idProfesional= PE.idSolicitante
            inner join sys_usuario as U on U.idusuario=PE.idusuarioregistro
            " + str_condicion + " order by PE.idPeticion desc";
    
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);           
            return Ds.Tables[0];
        }
        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtNro.Text != "") { if (oUtil.EsEntero(txtNro.Text)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }
        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblEstado = (Label)e.Row.Cells[0].Controls[1];


            


                Button CmdAnular = (Button)e.Row.Cells[8].FindControl("btnAnular");
                
                CmdAnular.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdAnular.CommandName = "Eliminar";
                CmdAnular.ToolTip = "Anular la Peticion";
              



                Button btnEditar = (Button)e.Row.Cells[8].FindControl("btnEditar");

                btnEditar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                btnEditar.CommandName = "Editar";
                btnEditar.ToolTip = "Editar la Peticion";
               



                Button btnResultados = (Button)e.Row.Cells[8].FindControl("btnResultados");

                btnResultados.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                btnResultados.CommandName = "Resultado";
                btnResultados.ToolTip = "Visualizar Resultados";
                btnResultados.Visible = false;


                string estado = lblEstado.Text;

                if (estado.ToUpper().IndexOf("ENVIADA") != -1)
                {
                    CmdAnular.Visible = true;
                    btnEditar.Visible = true;
                    lblEstado.CssClass = "label label-warning";

                }
                else
                {
                    CmdAnular.Visible = false;
                    btnEditar.Visible = false;
                }
                if (lblEstado.Text.ToUpper().IndexOf("RECIBIDA") != -1)
                {
                    lblEstado.CssClass = "label label-primary";

                }
                if (estado.ToUpper().IndexOf("TERMINADO") != -1)
                {
                   
                    lblEstado.CssClass = "label label-success";
                    btnResultados.Visible = true;
                }
                if (estado.ToUpper().IndexOf("EN PROCESO") != -1) lblEstado.CssClass = "label label-info";

                if (estado.ToUpper().IndexOf("ELIMINADA") != -1) lblEstado.CssClass = "label label-danger";




            }

        }

        private string getEstadoProtocolo(string idPeticion)
        {
            string dev = "Recibida";
            Peticion oRegistro = new Peticion();
            oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(idPeticion));
            if (oRegistro.IdProtocolo > 0)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), oRegistro.IdProtocolo);
                switch (oProtocolo.Estado)
                { case 1:dev = "En proceso"; break; case 2:dev = "Terminado"; break; default: dev = "Recibida"; break; }
            }
            else { if (oRegistro.Baja) dev = "Eliminada"; }
            return dev;
        }

        


        private void Anular(Business.Data.Laboratorio.Peticion oRegistro)
        {
           
            oRegistro.Baja = true;
            
            oRegistro.Save();
            
            CargarGrilla();
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {

           


            Business.Data.Laboratorio.Peticion oRegistro = new Business.Data.Laboratorio.Peticion();
            oRegistro = (Business.Data.Laboratorio.Peticion)oRegistro.Get(typeof(Business.Data.Laboratorio.Peticion), int.Parse(e.CommandArgument.ToString()));
            switch (e.CommandName)
            {
                case "Editar":
                    {
                        Response.Redirect("PeticionLC.aspx?idPeticion=" + oRegistro.IdPeticion.ToString(), false);
                    }
                    break;
                case "Eliminar":
                    Anular(oRegistro);
                    break;
                case "Resultado":
                    {
                        string m_parametro = " P.idProtocolo=" + oRegistro.IdProtocolo.ToString();

                        Response.Redirect("../Resultados/Procesa.aspx?idServicio=3&ModoCarga=LP&Operacion=HC&Parametros=" + m_parametro + "&idArea=0&idHojaTrabajo=0&validado=1&modo=Normal&Tipo=PacienteValidado&master=1", false);
                        break;



                    }
            }
        }

  


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
         

            CargarGrilla();
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            gvLista.PageIndex = e.NewPageIndex;

            int currentPage = gvLista.PageIndex + 1;
            CurrentPageLabel.Text = "Página " + currentPage.ToString() + " de " + gvLista.PageCount.ToString();
            CargarGrilla();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
          

                CargarGrilla();
        }
    }
}