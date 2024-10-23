using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Text;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace WebLab.Protocolos
{
    public partial class DerivacionScreening : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Recepción de Derivaciones"); PreventingDoubleSubmit(btnBuscar);
              //  Session["idServicio"] = "4";
                //CargarListas(); 
                //CargarGrillaProtocolo(); 
                ProtocoloList1.CargarGrillaProtocolo("4");
                txtNumeroProtocolo.Focus(); }

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
        //private void CargarGrillaProtocolo()
        //{

        //    DataList1.DataSource = LeerDatosProtocolos();
        //    DataList1.DataBind();
        //}
        private object LeerDatosProtocolos()
        {
            ////mostrar solo ultimas tarjetas ingresadas; con dato del numero de tarjeta, efector solicitante.

            string str_condicion = " WHERE P.baja=0 and P.idTipoServicio=4"; // +Session["idServicio"].ToString();
            //DateTime fecha1 = DateTime.Today;
            //if (Request["urgencia"] != null)
            //{
            //    str_condicion += " and P.idPrioridad=2 ";
            //}

            string m_strSQL = " SELECT   TOP (10) dbo.NumeroProtocolo (P.idProtocolo) AS numero, Pa.apellido + ' ' + Pa.nombre AS paciente, U.username ,P.idProtocolo ,P.idTipoServicio," +
            " Pa.numeroDocumento, P.idPaciente as idPaciente, P.fechaRegistro , S.idSolicitudScreeningOrigen as numerosolicitud, E.nombre as efector " +
            " FROM        dbo.LAB_Protocolo AS P " +
            " INNER JOIN  dbo.Sys_Paciente AS Pa ON Pa.idPaciente = P.idPaciente " +
            " INNER JOIN  dbo.Sys_Usuario AS U ON U.idUsuario = P.idUsuarioRegistro " +
            " inner JOIN  dbo.LAB_SolicitudScreening  AS S ON S.idProtocolo = P.idProtocolo " +
            " inner JOIN  dbo.Sys_Efector  AS E ON E.idefector = P.idEfectorSolicitante " +
            str_condicion +
            " order by P.idProtocolo desc ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }


        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            HyperLink oHplInfo = (HyperLink)e.Item.FindControl("hplProtocoloEdit");
            if (oHplInfo != null)
            {
                string idProtocolo = oHplInfo.NavigateUrl;
                oHplInfo.NavigateUrl = "ProtocoloEditPesquisa.aspx?idServicio=4&Desde=Derivacion&Operacion=Modifica&idProtocolo=" + idProtocolo;                
            }
        }
       

        private void PreventingDoubleSubmit(Button button)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Conectando...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) { Response.Redirect("DerivacionProcesa.aspx?protocolo=" + txtNumeroProtocolo.Text+"&isScreening=1", false); }

        }
    }
}