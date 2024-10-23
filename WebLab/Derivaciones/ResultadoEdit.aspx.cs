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
using System.Drawing;
using Business.Data.Laboratorio;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class ResultadoEdit : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
          
                LlenarTabla();
          
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null) CargarItem();
                else Response.Redirect("../FinSesion.aspx", false);
            }

        }

     
        private void CargarItem()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = " SELECT DISTINCT idItem, determinacion FROM vta_LAB_DerivacionesEnviadas " +
                            " WHERE  " + Request["Parametros"].ToString() + " ORDER BY determinacion";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "determinacion");
            ddlItem.Items.Insert(0, new ListItem("Todas", "0"));


            //m_ssql = " SELECT DISTINCT idEfector, efectorDerivacion FROM vta_LAB_Derivaciones " +
            //         " WHERE   " + Request["Parametros"].ToString();

            //oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "efectorDerivacion");
            //ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));


            if (Request["idItem"] != null)
                ddlItem.SelectedValue = Request["idItem"].ToString();

            if (Request["idEfector"] != null)
                ddlEfector.SelectedValue = Request["idEfector"].ToString();

            if (Request["conResultado"] != null)
                ddlEstado.SelectedValue = Request["conResultado"].ToString();
            m_ssql = null;
            oUtil = null;


        }

        private void LlenarTabla()
        {
            //Item oItem = new Item();
            //oItem = (Item)oItem.Get(typeof(Item), int.Parse(p));

            // lblItem.Text = oItem.Codigo + " - "+  oItem.Nombre;
        

            DataTable dt = getDataSet();
            if (dt.Rows.Count > 0)
            {

                TableRow objFila_TITULO = new TableRow();
                TableCell objCellProtocolo_TITULO = new TableCell();
                TableCell objCellFecha_TITULO = new TableCell();
                TableCell objCellPaciente_TITULO = new TableCell();
                TableCell objCellResultado_TITULO = new TableCell();
                TableCell objCellAnalisis_TITULO = new TableCell();
                TableCell objCellPersona_TITULO = new TableCell();



                Label lblTituloProtocolo = new Label();
                lblTituloProtocolo.Text = "PROTOCOLO";
                objCellProtocolo_TITULO.Controls.Add(lblTituloProtocolo);


                Label lblTituloFecha = new Label();
                lblTituloFecha.Text = "FECHA";
                objCellFecha_TITULO.Controls.Add(lblTituloFecha);

                Label lblTituloPaciente = new Label();
                lblTituloPaciente.Text = "PACIENTE";
                objCellPaciente_TITULO.Controls.Add(lblTituloPaciente);

               
                Label lblTituloAnalisis= new Label();
                lblTituloAnalisis.Text = "ANALISIS";
                objCellAnalisis_TITULO.Controls.Add(lblTituloAnalisis);

                Label lblTituloResultado = new Label();
                lblTituloResultado.Text = "RESULTADO";
                objCellResultado_TITULO.Controls.Add(lblTituloResultado);


                Label lblCargadoPor = new Label();
                lblCargadoPor.Text = "CARGADO POR";
                objCellPersona_TITULO.Controls.Add(lblCargadoPor);

                objFila_TITULO.Cells.Add(objCellProtocolo_TITULO);
                objFila_TITULO.Cells.Add(objCellFecha_TITULO);
                objFila_TITULO.Cells.Add(objCellPaciente_TITULO);
                objFila_TITULO.Cells.Add(objCellAnalisis_TITULO);
                objFila_TITULO.Cells.Add(objCellResultado_TITULO);
              
                objFila_TITULO.Cells.Add(objCellPersona_TITULO);

                objFila_TITULO.CssClass = "myLabelIzquierda";
                objFila_TITULO.BackColor = Color.Beige; //.DarkBlue;// "#F2F2FF";

                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                tContenido.Controls.Add(objFila_TITULO);//.Rows.Add(objRow);    


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string s_paciente = dt.Rows[i].ItemArray[2].ToString() ;
                    string s_fecha = dt.Rows[i].ItemArray[1].ToString();
                    string s_numero = dt.Rows[i].ItemArray[0].ToString();
                    string s_analisis = dt.Rows[i].ItemArray[3].ToString();
                    string s_idDetalleProtocolo = dt.Rows[i].ItemArray[5].ToString();
                    string s_resultado = dt.Rows[i].ItemArray[6].ToString();
                    string s_idUsuario = dt.Rows[i].ItemArray[7].ToString();
                    string s_fechaResultado = dt.Rows[i].ItemArray[8].ToString();  
                    

                    TableRow objFila = new TableRow();
                    TableCell objCellProtocolo = new TableCell();
                    TableCell objCellFecha = new TableCell();
                    TableCell objCellPaciente = new TableCell();
                    TableCell objCellResultado = new TableCell();
                    TableCell objCellAnalisis = new TableCell();

                    TableCell objCellPersona = new TableCell();


                    Label olblProtocolo = new Label();
                    olblProtocolo.Text = s_numero;
                    olblProtocolo.Font.Bold = true;

                    objCellProtocolo.BackColor = Color.Beige;
                    objCellProtocolo.Controls.Add(olblProtocolo);

                    Label olblFecha = new Label();
                    olblFecha.Text = s_fecha;
                    objCellFecha.Controls.Add(olblFecha);


                    Label olblPaciente = new Label();
                    olblPaciente.Text = s_paciente;
                    objCellPaciente.Controls.Add(olblPaciente);

                    Label olblAnalisis = new Label();
                    olblAnalisis.Text = s_analisis;
                    objCellAnalisis.Controls.Add(olblAnalisis);


                                      
                    TextBox txt1 = new TextBox();
                    txt1.ID = s_idDetalleProtocolo;

                        txt1.Text = s_resultado;
                    txt1.TextMode = TextBoxMode.MultiLine;
                    txt1.Width = Unit.Percentage(80);
                    txt1.Rows = 2;
                    txt1.MaxLength = 200;
                    txt1.CssClass = "myTexto";

               

                    Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(txt1);

                    objCellResultado.Controls.Add(txt1);

                




                  Label lblPersona = new Label();
                    

                    if (s_idUsuario!="0")
                    {
                        Usuario oUser = new Usuario();
                        oUser = (Usuario)oUser.Get(typeof(Usuario), "IdUsuario", int.Parse( s_idUsuario));
                        lblPersona.Text = oUser.Username + " - " + s_fechaResultado;
                    }
                    lblPersona.Font.Size = FontUnit.Point(7);
                    lblPersona.Font.Italic = true;
                   

                    objCellPersona.Controls.Add(lblPersona);

                    ///Definir los anchos de las columnas
                    objCellProtocolo.Width = Unit.Percentage(10);
                    objCellFecha.Width = Unit.Percentage(10);
                    objCellPaciente.Width = Unit.Percentage(20);                 
                    objCellAnalisis.Width = Unit.Percentage(20);
                    objCellResultado.Width = Unit.Percentage(40);
                    objCellPersona.Width = Unit.Percentage(20);



                    ///////////////////////
                    ///agrega a la fila cada una de las celdas

                    objFila.Cells.Add(objCellProtocolo);
                    objFila.Cells.Add(objCellFecha);
                    objFila.Cells.Add(objCellPaciente);                    
                    objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);
                    //objFila.Cells.Add(objCellValoresReferencia);
                    objFila.Cells.Add(objCellPersona);
                    //objFila.Cells.Add(objCellValida);

                    //////

                    Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                    //'añadimos la fila a la tabla
                    if (objFila != null)
                        tContenido.Controls.Add(objFila);//.Rows.Add(objRow);                                
                }
            }


        }

        private DataTable getDataSet()
        {
            string s_condicion = Request["Parametros"].ToString()+ "  and estado=1 ";

            if (Request["idItem"] != null)
            {
                if (Request["idItem"].ToString() !="0")
                    s_condicion += " and iditem= " + Request["idItem"].ToString();
            }
            //if (Request["idEfector"] != null)
            //{
            //    if (Request["idEfector"].ToString() != "0")
            //        s_condicion += " and idEfector= " + Request["idEfector"].ToString();
            //}
            if (Request["estado"] != null)
            {
                if (Request["estado"].ToString() != "-1")
                    s_condicion += " and estado= " + Request["estado"].ToString();
            }
            if (Request["conResultado"] == null) s_condicion += " and idUsuarioResultado =0";
            else
            {
                if (Request["conResultado"].ToString() != "-1")
                {
                    if (Request["conResultado"].ToString() == "0")
                        s_condicion += " and idUsuarioResultado =0";
                    else
                        s_condicion += " and idUsuarioResultado >0";
                }
            }

            string m_strSQL = " SELECT numero, convert(varchar(10),fecha,103) as fecha,convert(varchar,dni) + ' - ' + apellido + ' ' + nombre  AS PACIENTE, " +
                              " determinacion, efectorDerivacion, IDDETALLEprotocolo, RESULTADO, IDusuarioResultado, fechaResultado FROM [vta_LAB_DerivacionesEnviadas]" +                              
                              " WHERE  " +s_condicion+ 
                              " ORDER BY NUMERO ";     

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

        void ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl1 = (DropDownList)sender;
        }

      
       

       


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Guardar();
                Response.Redirect("Derivados2.aspx?tipo=resultado", false);
                //Response.Redirect("ResultadoEdit.aspx?Parametros=" + Request["Parametros"].ToString() + "&idItem=" + Request["idItem"].ToString() , false);
            }
        }

        private void Guardar()
        {


            string m_id = "";

            TextBox txt;
           // DropDownList ddl;

            if (Page.Master != null)
            {
                foreach (Control control in Page.Master.Controls)
                {
                    if (control is HtmlForm)
                    {
                        foreach (Control controlform in control.Controls)
                        {
                            if (controlform is ContentPlaceHolder)
                            {
                                foreach (Control control1 in controlform.Controls)
                                {
                                    if (control1 is Panel)
                                        foreach (Control control2 in control1.Controls)
                                        {
                                            if (control2 is Table)
                                                foreach (Control control3 in control2.Controls)
                                                {

                                                    if (control3 is TableRow)
                                                        foreach (Control control4 in control3.Controls)
                                                        {

                                                            if (control4 is TableCell)
                                                                foreach (Control control5 in control4.Controls)
                                                                {

                                                                    if (control5 is TextBox)
                                                                    {
                                                                        txt = (TextBox)control5;
                                                                        if (txt.Enabled)
                                                                        {

                                                                            if (txt.Text != "")
                                                                            {
                                                                                m_id = txt.Text;
                                                                                GuardarResultado(txt.ID, txt.Text);
                                                                            }
                                                                        }
                                                                    }

                                                                   
                                                                }
                                                        }
                                                }
                                        }
                                }
                            }
                        }
                    }
                }
            }




        }


        private void GuardarResultado(string m_idDetalleProtocolo, string valorItem)
        {
            DetalleProtocolo oDet = new DetalleProtocolo();
            oDet = (DetalleProtocolo)oDet.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));
            ////////////
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Derivacion));
            crit.Add(Expression.Eq("IdDetalleProtocolo", oDet));
            Derivacion oDetalle = (Derivacion)crit.UniqueResult();                       
            if (oDetalle == null)
            {               
                oDetalle.IdDetalleProtocolo = oDet;
                oDetalle.Estado = 2;
                oDetalle.FechaRegistro = DateTime.Now;
                oDetalle.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            }
            if (valorItem != "")
            {                             
                oDetalle.Resultado =valorItem;
                oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                oDetalle.FechaResultado = DateTime.Now;                                                
                oDetalle.Save();                
            }
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  Response.Redirect("ResultadoEdit.aspx?Parametros=" + Request["Parametros"].ToString() + "&idItem=" + ddlItem.SelectedValue + "&idEfector=" + ddlEfector.SelectedValue+"&conResultado="+ ddlEstado.SelectedValue );
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    Response.Redirect("ResultadoEdit.aspx?Parametros=" + Request["Parametros"].ToString() + "&idItem=" + ddlItem.SelectedValue + "&idEfector=" + ddlEfector.SelectedValue + "&conResultado=" + ddlEstado.SelectedValue);
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Response.Redirect("ResultadoEdit.aspx?Parametros=" + Request["Parametros"].ToString() + "&idItem=" + ddlItem.SelectedValue + "&idEfector=" + ddlEfector.SelectedValue + "&conResultado=" + ddlEstado.SelectedValue);

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ResultadoEdit.aspx?Parametros=" + Request["Parametros"].ToString() + "&idItem=" + ddlItem.SelectedValue + "&idEfector=" + ddlEfector.SelectedValue + "&conResultado=" + ddlEstado.SelectedValue);
        }

       
    }
}
