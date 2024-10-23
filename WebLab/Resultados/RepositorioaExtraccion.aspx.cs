using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.Configuration;
using Business.Data;

namespace WebLab.Resultados
{
    public partial class RepositorioaExtraccion : System.Web.UI.Page
    {
        string listavalidado = "";
        //   string resultado = "";
        DataTable dtDeterminaciones; //tabla para determinaciones
        int fila = 0;
        protected void Page_Load(object sender, EventArgs e)
        {  
            if (!Page.IsPostBack)
            {
              //   VerificaPermisos("Validacion");
                txtNumero.Attributes.Add("onkeypress", "return clickButton(event,'" + btnAgregar.ClientID + "')");
                txtNumero.Focus();
                //Page.ClientScript.RegisterStartupScript(this, typeof(this.Page),  "ScriptDoFocus",   SCRIPT_DOFOCUS.Replace("REQUEST_LASTFOCUS", Request["__LASTFOCUS"]),  true);
                CargarListas();
                InicializarTablas();
                if (Request["mostrarResumen"] != null)
                {
                    MostrarResumen();
                    MostrarDetalle();
                    //ddlResultado.Enabled = false;
                    txtNumero.Enabled = false;
                    btnAgregar.Enabled = false;
                    btnValidar.Enabled = false;
                    btnComenzar.Visible = true;
                    gvLista.Visible = false;
                    fila = 0;
                }
                                

            }


        }
       
       

        private void MostrarDetalle()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  
select P.numero as Protocolo, Pa.apellido + '  ' + Pa.nombre  as Paciente 
from lab_Protocolo P
inner join sys_paciente  Pa on Pa.idpaciente=P.idpaciente
inner join lab_repositorioprotocolo D on D.idprotocolo= P.idprotocolo
 
 where  P.idProtocolo in (" + Session["ListaValidado"].ToString() + ")  order by P.numero ";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumeDetalle.DataSource = Ds.Tables[0];
            gvResumeDetalle.DataBind();

            if (Ds.Tables[0].Rows.Count > 0)
            {
                gvResumeDetalle.Visible = true;
               
            }
            else
                gvResumeDetalle.Visible = false;

            


        }
        private void MostrarResumen()
        {  ////Metodo que carga la grilla de Protocolos
            string m_strSQL = @"  select  idRepositorio as Caja, count (*) as Cantidad
   from LAB_repositorioProtocolo where idProtocolo in (" + Session["ListaValidado"].ToString() + ") group by idRepositorio ";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            gvResumen.DataSource = Ds.Tables[0];
            gvResumen.DataBind();
            gvResumen.Visible = true;
            lblProcesado.Visible = true;
            // mostrar la grilla con lo procesado

          
            




        }


 
  
  


 

      
        private void CargarListas()
        {
//            Item oItem = new Item();

//            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
//            if (oItem != null)
//            {
//                lblDeterminacion.Text = oItem.Nombre;
//                lblCodigo.Text = oItem.Codigo;
//                hiditem.Value = oItem.IdItem.ToString();
//                Utility oUtil = new Utility();
//                string m_ssql = @"select distinct  resultadocar as resultado from LAB_detalleprotocolo
//where iditem in (select iditem from lab_item where codigo = '" + lblCodigo.Text + "') order by resultadocar";

//                oUtil.CargarCombo(ddlResultado, m_ssql, "resultado", "resultado");
//                ddlResultado.Items.Insert(0, new ListItem("--Seleccione--", "0"));
//            }


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
                    case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void Agregar( Repositorio oRegistro)
        {
            ///Agregar a la tabla lOS PROTOCOLOS para mostrarlas en el gridview
            try { 
            bool existe = false;
               
                    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                    fila = (int)(Session["orden"]);
                    //Primero verifica que no exista el item en la lista
                    for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                    {
                        if (txtNumero.Text.Trim() == dtDeterminaciones.Rows[i][1].ToString())
                            existe = true;
                    }
                    if (!existe)
                    {
                        Session["orden"] = int.Parse(Session["orden"].ToString()) + 1;
                        //fila  = fila+1;

                        DataRow row = dtDeterminaciones.NewRow();
                    row[0] = oRegistro.IdRepositorio.ToString();
                    row[1] = oRegistro.IdRepositorio.ToString();
                    row[2] = oRegistro.FechaRegistro.ToShortDateString();
                    row[3] = oRegistro.IdUsuarioRegistro.Apellido + " " + oRegistro.IdUsuarioRegistro.Nombre;
                       
                        row[4] = "";
                        row[5] = Session["orden"].ToString();
                        dtDeterminaciones.Rows.Add(row);

                      
                        dtDeterminaciones.DefaultView.Sort = " orden desc";
                        dtDeterminaciones = dtDeterminaciones.DefaultView.ToTable();

                        Session.Add("Tabla1", dtDeterminaciones);
                        gvLista.DataSource = dtDeterminaciones;
                        gvLista.DataBind();
                        gvLista.Visible = true;
                        lblError.Text = "";
                        lblCantidad.Text = dtDeterminaciones.Rows.Count.ToString()  + " protocolos agregados";
                    }
                    else
                    {
                        lblError.Text = "El repositorio ya fue ingresado a la lista";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                   btnAgregar.UpdateAfterCallBack = true;
                    gvLista.UpdateAfterCallBack = true;

                    lblCantidad.UpdateAfterCallBack = true;
                    txtNumero.Text = ""; lblError.UpdateAfterCallBack = true;
                    txtNumero.Focus();
                    txtNumero.UpdateAfterCallBack = true;
                   

              

           
                  }
                catch (Exception e)
                { }

        }

 

 
      
  
  
      

     

     

     


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                Usuario oUser = new Usuario();
               
                    oUser = (Usuario)oUser.Get(typeof(Usuario),int.Parse(Session["idUsuario"].ToString()));

               

                
              

                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

                ///// GENERAR NUEVO REGISTRO EN LAB_REPOSITORIO

                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    string m_id  = dtDeterminaciones.Rows[i][0].ToString();
                    Repositorio oRep = new Repositorio();
                    oRep = (Repositorio)oRep.Get(typeof(Repositorio), int.Parse(m_id));
                    oRep.IdUsuarioIngresoExtraccion = oUser.IdUsuario;
                    oRep.FechaIngresoExtraccion = DateTime.Now;
                    oRep.Estado = "I"; //Generado
                    oRep.Save();




                    if (listavalidado == "") listavalidado = oRep.IdRepositorio.ToString();
                    else listavalidado += "," + oRep.IdRepositorio.ToString();
                    Session["ListaValidado"] = listavalidado;

                }

                /// imprimir codigo de barras de caja.
                Response.Redirect("RepositorioxNroEdit.aspx?mostrarResumen=1", false);
            }


        }

     






        private void InicializarTablas()
        {
            ///Inicializa las sesiones para las tablas de diagnosticos y de determinaciones
            if (Session["Tabla1"] != null)
            {
                Session["Tabla1"] = null;
                Session["orden"] = null;
            }


          

            //if (Session["Tabla2"] != null) Session["Tabla2"] = null;

            dtDeterminaciones = new DataTable();


            dtDeterminaciones.Columns.Add("IdRepositorio"); 
            dtDeterminaciones.Columns.Add("numero");
            dtDeterminaciones.Columns.Add("Fecha");
            dtDeterminaciones.Columns.Add("usaurio");
                       dtDeterminaciones.Columns.Add("eliminar");
            dtDeterminaciones.Columns.Add("orden");


            Session.Add("Tabla1", dtDeterminaciones);
            Session.Add("orden", 0);

          

        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            VerificayAgrega();

            txtNumero.Focus();
            txtNumero.AutoUpdateAfterCallBack = true;


        }

        private void VerificayAgrega()
        {
            try
            {
                Repositorio oRegistro = new Repositorio();
                oRegistro = (Repositorio)oRegistro.Get(typeof(Repositorio), "IdRepositorio", int.Parse(txtNumero.Text));

               

                if (oRegistro != null)
                {
                    

                    Agregar(oRegistro);

    

                   
                }
                else
                    lblError.Text = "Numero no encontrado";
                lblError.UpdateAfterCallBack = true;
            }
            catch
            {
                lblError.Text = "Numero no encontrado";
                lblError.UpdateAfterCallBack = true;
            }

        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    if (dtDeterminaciones.Rows[i][0].ToString() == e.CommandArgument.ToString())
                        dtDeterminaciones.Rows[i].Delete();
                }


                dtDeterminaciones.DefaultView.Sort = " orden desc";
                dtDeterminaciones = dtDeterminaciones.DefaultView.ToTable();

                Session.Add("Tabla1", dtDeterminaciones);
                gvLista.DataSource = dtDeterminaciones;
                gvLista.DataBind();
                gvLista.UpdateAfterCallBack = true;

                lblCantidad.Text = dtDeterminaciones.Rows.Count.ToString() + " repositorios agregados";
                lblCantidad.UpdateAfterCallBack = true;
                //Session.Add("Tabla1", dtDeterminaciones);
                //gvLista.DataSource = dtDeterminaciones;
                //gvLista.DataBind();


            }
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                

                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = dtDeterminaciones.Rows[e.Row.RowIndex][0].ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

              

            }
        }

        protected void txtNumero_TextChanged(object sender, EventArgs e)
        {
            //if (txtNumero.Text.Length>=5)
            //{
            //    VerificayAgrega();
            //}
        }

        protected void btnComenzar_Click(object sender, EventArgs e)
        {
            gvResumeDetalle.Visible = false;
          
            gvResumen.Visible = false;
            lblProcesado.Visible = false;
            gvLista.Visible = true;
            
            txtNumero.Enabled = true;
            btnAgregar.Enabled = true;
            btnValidar.Enabled = true;
        }

        protected void btnHabilitar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";  lblError.Visible = true;
            lblError.UpdateAfterCallBack = true;
            btnAgregar.Visible = true;
            txtNumero.Enabled = true;
                                btnAgregar.UpdateAfterCallBack = true;
            txtNumero.UpdateAfterCallBack = true;
            btnHabilitar.Visible = false;
            btnHabilitar.UpdateAfterCallBack = true;
        }
    }
}