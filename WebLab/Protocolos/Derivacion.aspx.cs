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
    public partial class Derivacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) { VerificaPermisos("Derivacion");
                //PreventingDoubleSubmit(btnBuscar);
                CargarListas(); 
                //CargarGrillaProtocolo(); 
            ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
                txtNumeroProtocolo.Focus();
            }

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
            string str_condicion = " WHERE P.baja=0 and P.idTipoServicio="+ Request["idServicio"].ToString(); // +Session["idServicio"].ToString();
            DateTime fecha1 = DateTime.Today;
            if (Request["urgencia"] != null)
            {
                str_condicion += " and P.idPrioridad=2 ";
            }

            string m_strSQL = " SELECT   TOP (10) dbo.NumeroProtocolo (idProtocolo) AS numero, Pa.apellido + ' ' + Pa.nombre AS paciente, U.username ,P.idProtocolo ,P.idTipoServicio," +
            " Pa.numeroDocumento, P.idPaciente as idPaciente, P.fechaRegistro , TP.nombre as muestra " +
            " FROM         dbo.LAB_Protocolo AS P " +
            " INNER JOIN   dbo.Sys_Paciente AS Pa ON Pa.idPaciente = P.idPaciente " +
            " INNER JOIN   dbo.Sys_Usuario AS U ON U.idUsuario = P.idUsuarioRegistro " +
            " LEFT JOIN  dbo.LAB_Muestra AS TP ON TP.idMuestra = P.idMuestra " +
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
                  oHplInfo.NavigateUrl = "ProtocoloEdit2.aspx?idServicio=1&Desde=Derivacion&Operacion=Modifica&idProtocolo=" + idProtocolo;

                Label oMuestra = (Label)e.Item.FindControl("lblTipoMuestra");
                //if (Request["idServicio"].ToString() == "1")
                //{

                    oMuestra.Visible = false;
                    HyperLink oHplBacteriologia = (HyperLink)e.Item.FindControl("lnkMicrobiologia");
                    if (oHplBacteriologia != null)
                    {
                        oHplBacteriologia.Visible = true;
                        Label oIdPaciente = (Label)e.Item.FindControl("lblidPaciente");
                        oHplBacteriologia.NavigateUrl = "ProtocoloEdit2.aspx?idPaciente=" + oIdPaciente.Text + "&Operacion=Alta&idServicio=3&Urgencia=0";
                    }
                //}
                //else
                //    oMuestra.Visible = true;
            }
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string s_listaEfectores = ConfigurationManager.AppSettings["efectoresRPD"].ToString();
            ///Carga de combos de Efector
            string m_ssql = "SELECT idEfector, nombre FROM sys_Efector WHERE idEfector in (" + s_listaEfectores + ") ORDER BY nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("-- Seleccione Efector --", "0"));

            if (Request["idEfector"] != null)
                ddlEfector.SelectedValue = Request["idEfector"].ToString();
           
        
            m_ssql = null;
            oUtil = null;
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
            if (Page.IsValid) { Response.Redirect("DerivacionProcesa.aspx?idEfector=" + ddlEfector.SelectedValue + "&protocolo=" + txtNumeroProtocolo.Text + "&idServicio="+ Request["idServicio"].ToString(), false); }//+"&isScreening=0"

        }
    }
}