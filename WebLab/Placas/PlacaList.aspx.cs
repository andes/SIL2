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
using Business.Data.Laboratorio;
using Business;
using System.Drawing;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Business.Data;
using CrystalDecisions.Web;
using System.Text;
//using System.Web.UI.WebControls;

namespace WebLab.Placas
{
    public partial class PlacaList : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();

        public Usuario oUser = new Usuario();

        public Configuracion oC = new Configuracion();

 
        private void PreventingDoubleSubmit(Button button)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Procesando...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
       
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            HttpContext Context;
            Context = HttpContext.Current;

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
                PreventingDoubleSubmit(btnBuscar);

                    if (Session["idUsuario"] != null)
                    {
                        
                        txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                        txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                        
                            VerificaPermisos("Placas");

                            CargarGrilla ();  
                        

                    }
                    else Response.Redirect("../FinSesion.aspx", false);
                 

            }
        }
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }
        private bool VerificaPermisos(string v)
        {
            bool i = false;

            Utility oUtil = new Utility();
            Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], v);
            switch (Permiso)
            {
                case 0: i = false; break;
                case 1:
                    i = true; break;
                case 2: i = true; break;

            }

            return i;

        }


        private void CargarGrilla ()
        {
           
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
             PonerContadores();
            PintarReferencias();
        }
        private void PonerContadores()
        {
            lblAbierta.Text = getEstado("A");
            lblCerrada.Text = getEstado("C");
            
        }

        private string getEstado(string estado)
        {
            string m_strSQL  = @"select count(*) as cantidad,  estado from LAB_Placa  with (nolock) where 1=1 ";
           
            switch (estado)
            {
                case "A": m_strSQL += " and baja=0 and estado='A'"; break;
                case "C": m_strSQL += " and baja=0 and estado='C'"; break;
                case "B": m_strSQL += " and baja=1  "; break;
            }
            m_strSQL  = m_strSQL + " group by estado";



            DataSet Ds = new DataSet();

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            if (Ds.Tables[0].Rows.Count > 0)

                return Ds.Tables[0].Rows[0][0].ToString();
            else
                return "0";
        }
        private object LeerDatos( )
        {
            string m_condicion = " 1=1 ";        

                        if (txtNumero.Text != "") m_condicion += " and IDPlaca =" + txtNumero.Text.Trim();
          

            string m_strSQL = "";
            string m_orden = "";
            if (ddlTipo.SelectedValue == "0") m_orden = " ORDER BY IDPlaca desc";
            if (ddlTipo.SelectedValue == "1") m_orden = " ORDER BY IDPlaca asc";
            if (ddlTipo.SelectedValue == "2") m_orden = " ORDER BY dateadd(dd,30,fecharegistro ) ";
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                m_condicion += " AND fechaRegistro>= '" + fecha1.ToString("yyyyMMdd") + "'";
            }
            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                m_condicion += " AND fechaRegistro<= '" + fecha2.AddDays(1).ToString("yyyyMMdd") + "'";
            }


            if (ddlEstado.SelectedValue == "-1")
                m_condicion += " and baja = 0 "; // solo los activos
            else
            {

                if (ddlEstado.SelectedValue == "B")
                    m_condicion += " and baja = 1 "; // eliminados
                else
                    m_condicion += " and estado='" + ddlEstado.SelectedValue + "' and baja = 0";
            }



            if (ddlEquipo.SelectedValue != "-1")
                m_condicion += " and P.equipo =  '" + ddlEquipo.SelectedValue +"'";//tipo alplex o promega

            
             m_strSQL = @"  select idPlaca, P.operador, P.equipo as tipo,  U.username as usuario, P.fecharegistro  ,
 ( select count(*) from (select distinct idprotocolo
from LAB_DetalleProtocolo  with (nolock)
where idDetalleProtocolo in 
(select idDetalleProtocolo from LAB_DetalleProtocoloPlaca   with (nolock) where  idplaca=P.idPlaca))x)  as cantidad, P.estado
FROM lab_Placa P  
inner join Sys_usuario U  with (nolock) on U.idusuario= P.idusuarioregistro
where   " + m_condicion + m_orden;
               

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }
        private void PintarReferencias()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                switch (row.Cells[12].Text)
                {
                    case "V": //sin enviar
                        {
                            for (int i=0; i<row.Cells.Count; i++)
                            {
                                row.Cells[i].BackColor = Color.GreenYellow;

                            }
                             
                        }
                        break;
                  

                }
            }
        }

        protected void btnNuevoPromega_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaEdit.aspx?placa=Promega", false);
        }

        protected void btnNuevoAlplex_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaEdit2.aspx?placa=Alplex", false);
        }
        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvLista_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {


        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            Session.Contents.Remove("idUsuarioValida");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
                oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString()));



                LinkButton CmdImprimir = (LinkButton)e.Row.Cells[6].Controls[1];
                CmdImprimir.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdImprimir.CommandName = "Imprimir";
                CmdImprimir.ToolTip = "Imprimir";


                LinkButton CmdModificar = (LinkButton)e.Row.Cells[7].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";
                CmdModificar.ToolTip = "Modificar";



                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[8].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";



               


                LinkButton CmdAuditoria = (LinkButton)e.Row.Cells[9].Controls[1];
                CmdAuditoria.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdAuditoria.CommandName = "Auditoria";
                CmdAuditoria.ToolTip = "Auditoria";


         
                LinkButton CmdValida = (LinkButton)e.Row.Cells[10].Controls[1];
                CmdValida.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdValida.CommandName = "Valida";
                CmdValida.ToolTip = "Valida";


                LinkButton CmdConsulta = (LinkButton)e.Row.Cells[11].Controls[1];
                CmdConsulta.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdConsulta.CommandName = "Consultar";
                CmdConsulta.ToolTip = "Consultar";


                e.Row.Cells[0].Font.Bold = true;

              
                 

                

                //if (Permiso == 1)
                //{
                //    //CmdBaseFA.Visible = false;
                //    CmdEliminar.Visible = false;
                //    CmdModificar.ToolTip = "Consultar";

                //}

               




                if (oRegistro.Baja) //esta dado de baja
                {

                    //CmdBaseFA.Visible = false;
                    CmdEliminar.Visible = false;
                    CmdModificar.Text = "Consultar";
                    //CmdModificar.Visible = VerificaPermisosObjeto("Modificar Caso");
                  
                    //CmdAuditoria.Visible = true;
                  //  CmdAuditoria.Visible = VerificaPermisosObjeto("Auditoria Caso");
                    CmdValida.Visible = false;
                    Label CountryNameLabel = (Label)e.Row.FindControl("lblNumero");
                    CountryNameLabel.CssClass = "label label-default";

                }

                if (oRegistro.Estado=="A")

                {
                    Label CountryNameLabel = (Label)e.Row.FindControl("lblNumero");
                    CountryNameLabel.CssClass = "label label-warning";
                }
                if (oRegistro.Estado == "C")

                {
                    Label CountryNameLabel = (Label)e.Row.FindControl("lblNumero");
                    CountryNameLabel.CssClass = "label label-success";
                }




                gvLista.DataKeys[e.Row.RowIndex].Value.ToString();



            }

        }

        private DataTable GetDataSetAuditoria(string sidCaso)
        {



            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();



            string m_strSQL = "SELECT " + sidCaso + @" AS numero,U.apellido  as username, A.fecha AS fecha, A.hora, '' as analisis, A.accion,  A.valor, A.valorAnterior  
                FROM LAB_AuditoriaPlaca AS A  with (nolock)
                INNER JOIN Sys_Usuario AS U  with (nolock) ON A.idUsuario = U.idUsuario    
            where A.idPlaca = " + sidCaso + " ORDER BY A.idAuditoriaPlaca";

            DataSet Ds1 = new DataSet();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1, "auditoria");


            DataTable data = Ds1.Tables[0];
            return data;



        }

        private void ImprimirAuditoria(string v)
        {
            DataTable dtAuditoria = GetDataSetAuditoria(v);
            if (dtAuditoria.Columns.Count > 2)
            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;


                oCr.Report.FileName = "AuditoriaPlaca.rpt";
                oCr.ReportDocument.SetDataSource(dtAuditoria);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Auditoria_Placa_" + v);



            }
            else
            {
                string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }

        }


        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;

            int currentPage = gvLista.PageIndex + 1;


            CurrentPageLabel.Text = "Página " + currentPage.ToString() +
              " de " + gvLista.PageCount.ToString();
           

                CargarGrilla();
           


        }

        protected void btnNuevoAlplex_Click1(object sender, EventArgs e)
        {

        }

        protected void btnNuevoPromega_Click1(object sender, EventArgs e)
        {

        }

        protected void gvLista_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {         
            if (e.CommandName != "Page")
            {
                switch (e.CommandName)
                {
                    case "Imprimir":
                        Imprimir ( e.CommandArgument.ToString()); break;
                    case "Modificar":
                        Modificar(e.CommandArgument.ToString()); break;                      
                   
                    case "Auditoria":
                        ImprimirAuditoria(e.CommandArgument.ToString());                        break;                    
                    case "Eliminar": 
                        {
                            Eliminar(e.CommandArgument);                           
                                CargarGrilla();
                        }
                        break;
                    case "Valida":
                        Validar(e.CommandArgument.ToString()); break;
                    case "Consultar":
                        Consultar(e.CommandArgument.ToString()); break;
                }
            }

        }

        private void Consultar(string v)
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(v));
            if (oRegistro.Equipo == "PromegaM")

                Response.Redirect("PlacaResultadoMixta.aspx?Desde=Consulta&idPlaca=" + v); 
            else
                Response.Redirect("PlacaResultado.aspx?Desde=Consulta&idPlaca=" + v);
        }

        private void Validar(string v)
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(v));
            if (oRegistro.Equipo == "PromegaM")
                Response.Redirect("PlacaResultadoMixta.aspx?Desde=Valida&idPlaca=" + v);  
            else
                Response.Redirect("PlacaResultado.aspx?Desde=Valida&idPlaca=" + v);
        }

        private void Modificar(string v)
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(v));
            if (oRegistro.Equipo== "PromegaM")
                Response.Redirect("PlacaMixtaEdit.aspx?id=" + v);
            else
             Response.Redirect("PlacaEdit2.aspx?id=" + v);  
        }

        private void Imprimir(string id)
        {


            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(id));
            
            CrystalReportSource oCr = new CrystalReportSource();


            switch (oRegistro.Equipo)
            {
                case "PromegaM":

                    {
                        oCr.Report.FileName = "PlacaPromega.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
                case "Promega":

                    {
                        oCr.Report.FileName = "PlacaPromega.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
                case "Alplex":
                    {
                        oCr.Report.FileName = "PlacaAlplex.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
                case "Alplex7V":
                    {
                        oCr.Report.FileName = "PlacaAlplex.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
                case "Promega-30M":

                    {
                        oCr.Report.FileName = "PlacaPromega30M.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
            }



            oCr.DataBind();

            oRegistro.GrabarAuditoria("Imprime Placa", int.Parse(Session["idUsuario"].ToString()), "");
            string nombrearchivo = "Placa_" + oRegistro.IdPlaca.ToString() + "_" + oRegistro.Fecha.ToShortDateString().Replace("/", "");


            oCr.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, nombrearchivo);


        }

        private void Eliminar(object idItem)
        {          
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(idItem.ToString()));

            
                oRegistro.Baja = true;
              

                oRegistro.Save();

                 oRegistro.GrabarAuditoria("Elimina", int.Parse(Session["idUsuario"].ToString()), "");
             
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void btnNuevoPromega2_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaEdit.aspx?placa=Promega2", false);
        }

        protected void btnNuevoMixta_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaMixtaEdit.aspx?placa=PromegaM", false);
        }

        protected void btnNuevoAlplex8V_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaEdit2.aspx?placa=Alplex7V", false);
        }
    }


}


