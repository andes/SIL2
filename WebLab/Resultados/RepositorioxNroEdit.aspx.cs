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
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

namespace WebLab.Resultados
{
    public partial class RepositorioxNroEdit : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();
        public Configuracion oC = new Configuracion();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
        }


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

            ImprimirCaja();
            string m_strSQL = @"  select  idRepositorio as Caja, count (*) as [Cantidad Muestras]
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

        private void ImprimirCaja()
        {
              
            try
            {
                string idcaja = Request["idRepositorio"].ToString();

                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue numero= new ParameterDiscreteValue();
                numero.Value = idcaja.Replace(",00","");



                //   string informe = "";
                string m_reporte = "Caja_"+idcaja.ToString()+".pdf";
               oCr.Report.FileName = "../iNFORMES/Repositorio.rpt"; 

                 

                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(numero);
               
                oCr.DataBind();

                
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, m_reporte);
                 
            }
            catch
            { }

        
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
        private void Agregar( Protocolo oRegistro, string res)
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
                        row[0] = oRegistro.IdProtocolo.ToString();
                        row[1] = oRegistro.Numero.ToString();
                        row[2] = oRegistro.IdPaciente.NumeroDocumento.ToString();
                        row[3] = oRegistro.IdPaciente.Apellido + " " + oRegistro.IdPaciente.Nombre;

                    row[4] = res;
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
                        lblError.Text = "El protocolo ya fue ingresado a la lista";
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

                    Repositorio oRep = new Repositorio();
                oRep.IdUsuarioRegistro = oUser;
                oRep.FechaRegistro = DateTime.Now;
                oRep.Estado = "G"; //Generado
                oRep.FechaIngresoExtraccion = DateTime.Parse("01/01/1900");
                oRep.Save();

                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

                ///// GENERAR NUEVO REGISTRO EN LAB_REPOSITORIO

                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    string m_idProtocolo = dtDeterminaciones.Rows[i][0].ToString();
                    RepositorioProtocolo RepPro = new RepositorioProtocolo();
                    RepPro.IdRepositorio = oRep.IdRepositorio;
                    RepPro.IdProtocolo = int.Parse(m_idProtocolo);
                    RepPro.Save();

                    if (listavalidado == "") listavalidado = RepPro.IdProtocolo.ToString();
                    else listavalidado += "," + RepPro.IdProtocolo.ToString();
                    Session["ListaValidado"] = listavalidado;

                }

                /// imprimir codigo de barras de caja.
                Response.Redirect("RepositorioxNroEdit.aspx?mostrarResumen=1&idRepositorio="+ oRep.IdRepositorio.ToString(), false);
            }


        }

        private void GuardarResultado(string m_idDetalleProtocolo, string valorItem )
        {

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            //if (!todo)
            //    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdDetalleProtocolo", int.Parse(m_idDetalleProtocolo), "IdUsuarioValida", 0);// crit.Add(Expression.Eq("IdUsuarioValida", 0));
            //else
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(m_idDetalleProtocolo));

            if (oDetalle != null)
            {

                int tiporesultado = oDetalle.IdSubItem.IdTipoResultado;
                switch (tiporesultado)
                {
                    case 1:// numerico         
                        if (valorItem != "")
                        {
                            oDetalle.ResultadoNum = decimal.Parse(valorItem, System.Globalization.CultureInfo.InvariantCulture);
                            oDetalle.FormatoValida = oDetalle.IdSubItem.FormatoDecimal;
                            oDetalle.ConResultado = true;
                        }
                        else
                        {
                            oDetalle.ResultadoNum = 0;
                            oDetalle.ConResultado = false;
                        }
                        break;
                    default:
                        if (valorItem != "")
                        {
                            oDetalle.ResultadoCar = valorItem;
                            oDetalle.ConResultado = true;
                        }
                        else
                        {
                            oDetalle.ResultadoCar = "";
                            oDetalle.ConResultado = false;
                        }
                        break;
                }


             string valorRef = oDetalle.CalcularValoresReferencia();
                string m_metodo = "";
                string m_valorReferencia = "";
                //string nombre_control = "VR" + oDetalle.IdDetalleProtocolo.ToString();
                //Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
                //Label valorRef = (Label)control1;


                if (valorRef != null)
                {
                    string[] arr = valorRef.Split(("|").ToCharArray());
                    switch (arr.Length)
                    {
                        case 1: m_valorReferencia = arr[0].Trim().ToString(); break;
                        case 2:
                            {
                                m_valorReferencia = arr[0].Trim().ToString();
                                m_metodo = arr[1].Trim().ToString();
                            }
                            break;
                    }
                    oDetalle.Metodo = m_metodo;
                    oDetalle.ValorReferencia = m_valorReferencia;
                }
                string operacion = Request["Operacion"].ToString();
                string s_unidadMedida = "";
                int i_unidadMedida = oDetalle.IdSubItem.IdUnidadMedida;
                if (i_unidadMedida > 0)
                {
                    UnidadMedida oUnidad = new UnidadMedida();
                    oUnidad = (UnidadMedida)oUnidad.Get(typeof(UnidadMedida), i_unidadMedida);
                    s_unidadMedida = oUnidad.Nombre;
                }

                oDetalle.UnidadMedida = s_unidadMedida;
                //oDetalle.Metodo = m_metodo;
                //oDetalle.ValorReferencia = m_valorReferencia;
                if (Request["Operacion"].ToString() == "Carga")
                {
                    if (oDetalle.ConResultado)
                    {
                        oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                        oDetalle.FechaResultado = DateTime.Now;
                    }
                }

                if ((Request["Operacion"].ToString() == "Valida") && (oDetalle.ConResultado))  //Validacion
                {
                    string res = valorItem;
                    if (valorItem.Length > 10)
                        res = valorItem.Substring(0, 10);


                    if ((oDetalle.IdItem.Codigo == "9122") && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0) && (res == "SE DETECTA"))
                    {
                        if (oCon.PreValida)
                        {
                            operacion = "Pre Valida";
                            oDetalle.IdUsuarioPreValida = int.Parse(Session["idUsuarioValida"].ToString());
                            oDetalle.FechaPreValida = DateTime.Now;
                            oDetalle.IdUsuarioValida = 0;
                            oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        }
                        else
                        {
                            oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                            oDetalle.FechaValida = DateTime.Now;
                        }

                    }
                    else
                    {
                        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                        oDetalle.FechaValida = DateTime.Now;
                    }


                    if (listavalidado == "") listavalidado = oDetalle.IdDetalleProtocolo.ToString();
                    else listavalidado += "," + oDetalle.IdDetalleProtocolo.ToString();
                    Session["ListaValidado"] = listavalidado;
                }


                oDetalle.Save();


                if (oDetalle.ConResultado)
                {
                    if (Request["Operacion"].ToString() != "Valida")
                        oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuario"].ToString()));
                    else
                        oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuarioValida"].ToString()));
                }
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = oDetalle.IdProtocolo;

                if (Request["Operacion"].ToString() != "Valida")
                {
                    if (oProtocolo.Estado == 0)
                        oProtocolo.Estado = 1;                        //oProtocolo.Save();                    
                }
                else //Validacion
                {
                    if (oProtocolo.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(Session["idUsuarioValida"].ToString())))
                        oProtocolo.Estado = 2;  //validado total (cerrado);                    
                    else
                        oProtocolo.Estado = 1;
                }
                oProtocolo.Save();

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


            dtDeterminaciones.Columns.Add("idProtocolo"); 
            dtDeterminaciones.Columns.Add("numero");
            dtDeterminaciones.Columns.Add("dni");
            dtDeterminaciones.Columns.Add("paciente");
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
            //try
            //{
                Protocolo oRegistro = new Protocolo();
                oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), "Numero", int.Parse(txtNumero.Text));


            RepositorioProtocolo oDetalle = new RepositorioProtocolo();
          
            oDetalle = (RepositorioProtocolo)oDetalle.Get(typeof(RepositorioProtocolo), "IdProtocolo",  oRegistro.IdProtocolo);

            if (oDetalle != null)
            {
                if (oRegistro != null)
                {
                  //  string res = oRegistro.IdPaciente.GetProtocoloTestAntigenoReciente(20);

                    Agregar(oRegistro, res);

                    //if (res != "")
                    //{

                    //    lblError.Text = "El Paciente tiene un test de Antigeno Positivo previo. Verifique!! ";
                    //    lblError.Visible = true;
                    //    lblError.UpdateAfterCallBack = true;
                    //    btnAgregar.Visible = false;
                    //    txtNumero.Enabled = false;

                    //    btnAgregar.UpdateAfterCallBack = true;
                    //    txtNumero.UpdateAfterCallBack = true;
                    //    btnHabilitar.Visible = true;
                    //    btnHabilitar.UpdateAfterCallBack = true;
                    //}



                }
                else
                {
                    lblError.Text = "Numero no encontrado";
                    lblError.UpdateAfterCallBack = true;
                }
            }
            else
            {
                lblError.Text = "Protocolo ya incluido en otra caja.";
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

                lblCantidad.Text = dtDeterminaciones.Rows.Count.ToString() + " protocolos agregados";
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