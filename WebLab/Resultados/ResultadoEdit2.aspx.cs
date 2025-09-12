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
using Business.Data.Laboratorio;
using System.Drawing;
using NHibernate;
using NHibernate.Expression;
using Business.Data;
using MathParser;
using CrystalDecisions.Shared;
using System.IO;
using CrystalDecisions.Web;
using System.Text.RegularExpressions;
using Business.Data.AutoAnalizador;
using System.Web.Script.Serialization;
using System.Text;
using System.Net;
using System.Net.Http;

namespace WebLab.Resultados
{
    public partial class ResultadoEdit2 : System.Web.UI.Page
    {
      
        CrystalReportSource oCr = new CrystalReportSource();
        Utility oUtil = new Utility();
        Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
    //    public Usuario oUserValida = new Usuario();
        DataTable dtProtocolo;

        bool desvalidar = false;
        string s_placa = "";
        private enum TabIndex
        {
            DEFAULT = 0,
            ONE = 1,
            TWO = 2,
            THREE = 3,
            CUARTO = 4,
            QUINTO = 5
            // you can as many as you want here
        }
        private void SetSelectedTab(TabIndex tabIndex)
        {
            HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                if( Session["idUsuarioValida"] != null) oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
                else
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion),  "IdEfector", oUser.IdEfector);

                if (oCon != null)
                {
                    oCr.Report.FileName = ""; oCr.CacheDuration = 0; oCr.EnableCaching = false;
                    

                    if (Request["idProtocolo"] != null)
                    {
                        if ((Session["idUsuario"] != null))// &&  (!Page.IsPostBack))
                        {
                       
                            LlenarTabla(Request["idProtocolo"].ToString());
                        }
                        else
                            Response.Redirect("../FinSesion.aspx", false);
                           
                    }
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    Inicializar();
                    CargarListas();
                   
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);                             
            }
        }


  
        private void Inicializar()
        {

            CargarGrilla();
       

            MuestraDatos(CurrentPageIndex.ToString());
            Session["tildados"] = "";
            switch (Request["Operacion"].ToString())
            {
                case "Carga":
                    {
                        btnMostrarResultados.Visible = false;
                        tieneInci.Visible = false;
                        inci.Visible = false;
                        btnMostrarResultados.Visible = false;
                        imgDiagnostico.Visible = false;
                        tituloAntecedente.Visible = false;
                        btnDesValidar.Visible = false;            
                        pnlAntecedentes.Visible = false;
                        chkCerrarSinResultados.Visible = false;
                        chkFormula.Checked = oCon.AplicarFormulaDefecto;
                        pnlHC.Visible = false;
                        pnlResultados.Visible = true;
                        hypRegresar.NavigateUrl = "ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                        lblTitulo.Text = "CARGA DE RESULTADOS";
                        pnlReferencia.Visible = false;
                        //imgImprimir.Visible = false;
                        imgPdf.Visible = false;
                        
                        //lnkMarcar2.Visible = false;
                        //lnkDesmarcar2.Visible = false;

                        lnkMarcar.Visible = false;
                        lnkDesmarcar.Visible = false;

                        lnkMarcarAislamiento.Visible = false;
                        lnkDesMarcarAislamiento.Visible = false;

                        rdbImprimir.Visible = false;
                    //    pnlImpresora.Visible = false;
                        btnRestringirAcceso.Visible = false;
                        VerificaPermisos("Carga");
                    }
                    break;
                case "Control":
                    {
                        tieneInci.Visible = false;
                        inci.Visible = false;
                        btnMostrarResultados.Visible = false;
                        imgDiagnostico.Visible = false;
                        btnDesValidar.Visible = false;            
                        chkCerrarSinResultados.Visible = false;
                        chkFormula.Checked = oCon.AplicarFormulaDefecto;
                        pnlHC.Visible = false;
                        pnlResultados.Visible = true;
                        hypRegresar.NavigateUrl = "ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                        lblTitulo.Text = "CONTROL DE RESULTADOS"; lblTitulo.ForeColor = Color.Green;
                        pnlReferencia.Visible = false;
                     //   imgImprimir.Visible = false;
                        imgPdf.Visible = false;

                        //lnkMarcar2.Visible = true;
                        //lnkDesmarcar2.Visible = true;


                        lnkMarcar.Visible = true;
                        lnkDesmarcar.Visible = true;
                        lnkMarcarAislamiento.Visible = false;
                        lnkDesMarcarAislamiento.Visible = false;
                        rdbImprimir.Visible = false;
                    //    pnlImpresora.Visible = false;
                        VerificaPermisos("Carga");
                    }
                    break;
              
                case "Valida":
                    {
                    //    btnMostrarResultados.Visible = true;
                        if (desvalidar) btnDesValidar.Visible = true;
                       
                        btnValidarPendiente.Visible = true;
                        btnValidarPendienteImprimir.Visible = true;
                        chkCerrarSinResultados.Visible = true;
                        chkFormula.Visible = false;
                        chkFormula.Checked = false;
                        lblFormula.Visible = false;

                        pnlHC.Visible = false;
                        pnlResultados.Visible = true;

                        if (Request["urgencia"] != null)
                        {
                            //////////////////Se controla quien es el usuario que está por validar////////////////
                            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                           
                                if ((oCon.AutenticaValidacion) && (Session["idUsuarioValida"] == null))
                                //    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);
                                {
                                    //if ((Request["urgencia"] != null) && (oCon.AutenticaValidacion) && (Request["idUsuarioValida"] == null))
                                    string sredirect = "../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString() + "&urgencia=1&idProtocolo=" + Request["idProtocolo"].ToString() + "&Parametros=" + Request["idProtocolo"].ToString();
                                    if (Request["desde"] != null)
                                        sredirect += "&desde=" + Request["desde"].ToString();

                                    Response.Redirect(sredirect, false);
                                }
                                else
                                {
                                    //if (Request["idUsuarioValida"] != null)
                                    //    Session["idUsuarioValida"] = Request["idUsuarioValida"];
                                    //else
                                   // Session["idUsuarioValida"] = Session["idUsuario"];
                                }
                            
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }

                        if ((Request["desde"]!=null)&&(Request["desde"]=="Urgencia"))
                            hypRegresar.NavigateUrl = "../Urgencia/UrgenciaList.aspx";
                        else
                            hypRegresar.NavigateUrl = "ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                        lblTitulo.Text = "VALIDACION DE RESULTADOS";
                        pnlReferencia.Visible = true;
                        //lblTitulo.CssClass = "mytituloRojo2";
                        btnGuardar.Text = "Validar";
                        btnValidarImprimir.Visible = true;
                        rdbImprimir.Visible = true;
                      //  imgImprimir.Visible = true;
                        imgPdf.Visible = true;
                        lnkMarcar.Visible = true;
                        lnkDesmarcar.Visible = true;
                        lnkMarcarAislamiento.Visible = true;
                        lnkDesMarcarAislamiento.Visible = true;
                        imgDiagnostico.Visible = true;
                        //lnkMarcar2.Visible = true;
                        //lnkDesmarcar2.Visible = true;

                    //    pnlImpresora.Visible = true;
                        VerificaPermisos("Validacion");
                      
                        // if (Request["Operacion"].ToString() == "Valida")
                      
                            if (Session["idUsuarioValida"] != null) oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
                           
                        //if (Request["urgencia"] != null)
                        //    btnGuardar.Visible = false;



                    }
                    break;
                case "HC":
                    {

                        btnMostrarResultados.Visible = false;
                        imgDiagnostico.Visible = false;
                      //  Panel1.ScrollBars = ScrollBars.None;                                        
                        pnlAntibiograma.Visible = false;
                        pnlMicroOrganismo.Visible = false;
                        
                        
                        //pnlMicroOrganismoHC.Visible = true;
                        pnlAntecedentes.Visible = false;
                        tituloObservaciones.Visible = false;

                        pnlObservacionProtocolo.Visible = false;
                    //    pnlDiagnostico.Visible = false;
                     //   tabDiagnostico.Visible = false;
                        tabAntecedente.Visible = false;
                        pnlHC.Visible = true;
                        //pnlResultados.Visible = false;

                        chkFormula.Visible = false;
                        btnAplicarFormula.Visible = false;
                        lblFormula.Visible = false;
                        if (Request["validado"].ToString() == "1")
                         lblTitulo.Text = "HISTORIA CLINICA";
                        else
                         lblTitulo.Text = "CONSULTA DE RESULTADOS"; 

                        pnlReferencia.Visible = true;
                        //lblTitulo.ForeColor = Color.Black;
                        btnGuardar.Visible = false;
                        btnValidarImprimir.Visible = false;
                        btnRestringirAcceso.Visible = false;
                        rdbImprimir.Visible = false;
                    
                        //imgPdf.Visible = true;
                        //imgImprimir.Visible = true;
                        //pnlImpresora.Visible = true;
                        btnDesValidar.Visible = false;            
                        lnkMarcar.Visible = false;
                        lnkDesmarcar.Visible = false;

                        if (Request["desde"] == "Urgencia")
                            hypRegresar.NavigateUrl = "../Urgencia/UrgenciaList.aspx";
                        else
                        {
                            if (Request["Desde"]!=null)
                            {
                                if (Request["Desde"].ToString() == "HistoriaClinicaFiltro")
                                    hypRegresar.NavigateUrl = "../Informes/historiaClinicafiltro.aspx?Tipo=PacienteCompleto";
                            }
                            else

                            hypRegresar.NavigateUrl = "ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                          

                        }


                    }
                    break;
            }
        }

        private void CargarListas()
        {
           //Utility oUtil = new Utility();        
           /////////////////Impresoras////////////////////////
           //string m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
           //oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
           //if (Session["Impresora"] != null) ddlImpresora.SelectedValue = Session["Impresora"].ToString();


           // if (ddlImpresora.Items.Count == 0)
           // {

           //     pnlImpresora.Visible = false;
                 

           // }
           // ///////////////Fin de Impresoras///////////////////

        }

        private void CargarListasAntibiogramas()
        {
    
            CargarAntibioticoPractica(ddlPracticaAtb.SelectedValue);
            
        }

      

        private void CargarAntibioticoPractica(string p)
        {
          
            Utility oUtil = new Utility();       
            ///Carga los germenes para la solapa Aislamientos
            string m_ssql = @" SELECT     CONVERT(varchar, PG.numeroAislamiento) + ' -  ' + G.nombre AS nombre, PG.idProtocoloGermen
FROM         LAB_Germen AS G (nolock) INNER JOIN
                      LAB_ProtocoloGermen AS PG (nolock) ON G.idGermen = PG.idGermen
WHERE     (PG.atb = 1) AND (G.baja = 0) AND (PG.idProtocolo = " + Request["idProtocolo"].ToString() + ") AND (PG.idItem = "+p+ ")   order by nombre";
         

            oUtil.CargarCombo(ddlGermen, m_ssql, "idProtocoloGermen", "nombre");
            ddlGermen.Items.Insert(0, new ListItem("--SELECCIONE AISLAMIENTO--", "0"));
        }


        protected void ddlPracticaAtb_SelectedIndexChanged(object sender, EventArgs e)
        {            
            CargarAntibioticoPractica(ddlPracticaAtb.SelectedValue);
            ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
            SetSelectedTab(TabIndex.CUARTO);
        }

        protected void ddlPerfilAntibiotico_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarListaAntibiotico();
            SetSelectedTab(TabIndex.CUARTO);
        }


        protected void ddlMetodoAntibiograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarListaAntibiogramas();
            SetSelectedTab(TabIndex.CUARTO);
        }


        private void CargarListasAislamientos()
        {
            Utility oUtil = new Utility();

            string m_ssql = @" SELECT DISTINCT DP.idItem, I.nombre FROM  LAB_DetalleProtocolo as DP (nolock)
                inner join lab_item as I (nolock) on I.iditem= DP.idItem WHERE  idProtocolo = " + Request["idProtocolo"].ToString();
            oUtil.CargarCombo(ddlPracticaAislamiento, m_ssql, "idItem", "nombre");
            
            //ddlPracticaAislamiento.Items.Insert(0, new ListItem("--SELECCIONE PRACTICA--", "0"));


            ddlPracticaAtb.DataTextField = "nombre";
            ddlPracticaAtb.DataValueField = "idItem";
            ddlPracticaAtb.DataSource = ddlPracticaAislamiento.DataSource;
            ddlPracticaAtb.DataBind();

           
            

            ///Carga los germenes para la solapa Aislamientos
             m_ssql = " SELECT   idGermen, nombre +  ' ' + codigo as nombre FROM LAB_Germen (nolock) " +
            " where baja=0 and idGermen not in (Select distinct idGermen from  LAB_Antibiograma  where idProtocolo=" + Request["idProtocolo"].ToString() + ")" +
            " order by nombre";

            oUtil.CargarCombo(ddlAislamiento, m_ssql, "idGermen", "nombre");
            ddlAislamiento.Items.Insert(0, new ListItem("--SELECCIONE MICROORGANISMO--", "0"));

            CargarPerfilAntibiotico();
           
        }

        protected void rdbMetodologiaAntibiograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CargarListaAntibiotico();
            //SetSelectedTab(TabIndex.CUARTO);
        }
        private void CargarPerfilAntibiotico()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            ///Carga los perfiles de  Antibioticos
            string m_ssql = @" SELECT DISTINCT PA.idPerfilAntibiotico, PA.nombre
FROM         LAB_PerfilAntibiotico AS PA (nolock) INNER JOIN
                      LAB_DetallePerfilAntibiotico AS DPA (nolock) ON PA.idPerfilAntibiotico = DPA.idPerfilAntibiotico INNER JOIN
                      LAB_Antibiotico AS A (nolock) ON DPA.idAntibiotico = A.idAntibiotico
WHERE     (PA.baja = 0)
ORDER BY PA.nombre";
            oUtil.CargarCombo(ddlPerfilAntibiotico, m_ssql, "idPerfilAntibiotico", "nombre", connReady);
            //ddlPerfilAntibiotico.Items.Insert(0, new ListItem("--SELECCIONE PERFIL ANTIBIOTICOS--", "0"));
            ddlPerfilAntibiotico.Items.Insert(0, new ListItem("--TODOS LOS ANTIBIOTICOS--", "0"));
            //////////////////////////////                              
        }


        protected void txtCodigoMicroorganismo_TextChanged(object sender, EventArgs e)
        {

            if (txtCodigoMicroorganismo.Text != "")
            {
                Germen oGermen = new Germen();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Germen));
                crit.Add(Expression.Eq("Codigo", txtCodigoMicroorganismo.Text));
                crit.Add(Expression.Eq("Baja", false));

                try
                {
                    oGermen = (Germen)crit.UniqueResult();
                    if (oGermen != null)
                    {
                        ddlAislamiento.SelectedValue = oGermen.IdGermen.ToString();

                    }
                    else
                    {
                        //lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no existe. ";
                        //ddlItem.SelectedValue = "0";
                        //txtCodigo.Text = "";

                        //txtCodigo.UpdateAfterCallBack = true;

                    }
                }
                catch { ddlAislamiento.SelectedValue = "0"; }

                ddlAislamiento.UpdateAfterCallBack = true;

             //   lblMensaje.UpdateAfterCallBack = true;
            }
            else
            {
                ddlAislamiento.SelectedValue = "0";

                ddlAislamiento.UpdateAfterCallBack = true;

            }
        }
        private void CargarListaAntibiotico()
        {
            Utility oUtil = new Utility();
            ///Carga los antibioticos para la solapa Antibiograma

            string m_ssql = "";
            if (ddlPerfilAntibiotico.SelectedValue == "0") ///Todos los antibioticos
            {
                m_ssql = " SELECT idAntibiotico , descripcion FROM LAB_Antibiotico where baja=0 order by descripcion ";
            }
            else
            {
                m_ssql = @" SELECT DISTINCT A.idAntibiotico, A.descripcion
FROM         LAB_PerfilAntibiotico AS PA INNER JOIN
                      LAB_DetallePerfilAntibiotico AS DPA ON PA.idPerfilAntibiotico = DPA.idPerfilAntibiotico INNER JOIN
                      LAB_Antibiotico AS A ON DPA.idAntibiotico = A.idAntibiotico
WHERE     (PA.idPerfilAntibiotico = " + ddlPerfilAntibiotico.SelectedValue + ")  ORDER BY A.descripcion";
            }
            //AND (A.idMetodologia = "+ rdbMetodologiaAntibiograma.SelectedValue+")
            DataSet Ds = new DataSet();
            //     SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_ssql, conn);
            adapter.Fill(Ds);

            gvAntiobiograma.DataSource = Ds;
            gvAntiobiograma.DataBind();
            //////////////////////////////
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
                    case 1:
                        {
                            if (sObjeto=="Carga") btnGuardar.Visible = false;
                            if (sObjeto == "Validacion")
                            {
                                btnGuardar.Visible = false;
                                btnValidarImprimir.Visible = false;
                            }
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }




       
        private void CargarGrilla()
        {
           
            
                string m_strSQL = " Select distinct P.idProtocolo, " +
                    " P.numero as numero," +
                    //              " 0 as numero," +
                                  " convert(varchar(10),P.fecha,103) as fecha"+// ,P.estado , P.fecha as fecha1," +
                              //    " , prefijosector, numerosector , numerodiario, numeroTipoServicio " +
                                  " from Lab_Protocolo P with (nolock)" + // +str_condicion;
                    //" INNER JOIN Lab_Configuracion Con on Con.idEfector= P.idEfector " +
                                  " WHERE P.idProtocolo in (" + Session["Parametros"].ToString() + ")";

        
            if (Request["Operacion"].ToString() == "HC")
                {
                 //   if (Request["Tipo"] != null)
                        m_strSQL += " order by P.idProtocolo desc "; // desde el mas reciente al mas antiguo
                   // else
                     //   m_strSQL += " order by P.idProtocolo "; // desde el mas reciente al mas antiguo
                    //m_strSQL += " order by P.idProtocolo desc "; // desde el mas reciente al mas antiguo.
                }
                else
                {
                    //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
                    //if (oC.TipoNumeracionProtocolo == 0)
                        m_strSQL += " order by  numero ";
                    //if (oC.TipoNumeracionProtocolo == 1)
                    //    m_strSQL += " order by  numerodiario ";
                    //if (oC.TipoNumeracionProtocolo == 2)
                    //    m_strSQL += " order by prefijosector, numerosector ";
                    //if (oC.TipoNumeracionProtocolo == 3)
                    //    m_strSQL += " order by numeroTipoServicio ";
                }


                DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
                gvLista.DataSource = Ds.Tables[0];
                gvLista.DataBind();
               // dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);



                if (Ds.Tables[0].Rows.Count > 0)
                {
                    dtProtocolo = Ds.Tables[0];
                    int ultimafila = Ds.Tables[0].Rows.Count - 1;
                 //   CurrentPageIndex = int.Parse(Request["idProtocolo"].ToString());
                    CurrentIndexGrilla = int.Parse(Request["Index"].ToString());
                   CurrentPageIndex = int.Parse(Ds.Tables[0].Rows[CurrentIndexGrilla].ItemArray[0].ToString());
                    UltimaPageIndex = ultimafila; // int.Parse(Ds.Tables[0].Rows[ultimafila].ItemArray[0].ToString());
                }
                int cantidad = Ds.Tables[0].Rows.Count;
                if (cantidad == 1) {lnkAnterior.Visible = false; lnkPosterior.Visible = false;}
                lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados"; 
                Session.Add("Tabla1", dtProtocolo);
            
            
            
        }


        //private void CargarGrillaNuevo()
        //{
        //    ////Metodo que carga la grilla de Protocolos
        //    int ultimafila = 0;
        //    //if (Session["Tabla1"] == null)
        //    //{

        //        string m_strSQL = " Select distinct P.idProtocolo, " +
        //            " P.numero as numero," +
        //            //              " 0 as numero," +
        //                          " convert(varchar(10),P.fecha,103) as fecha" +// ,P.estado , P.fecha as fecha1," +
        //            //" prefijosector, numerosector , numerodiario" +
        //                          " from Lab_Protocolo P " + // +str_condicion;
        //            //" INNER JOIN Lab_Configuracion Con on Con.idEfector= P.idEfector " +
        //                          " WHERE P.idProtocolo in (" + Session["Parametros"].ToString() + ")";
          
        //        if (Request["Operacion"].ToString() == "HC")
        //            m_strSQL += " order by P.idProtocolo desc "; // desde el mas reciente al mas antiguo.
        //        else
        //        {
        //            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
        //            if (oC.TipoNumeracionProtocolo == 0)
        //                m_strSQL += " order by  idProtocolo ";
        //            if (oC.TipoNumeracionProtocolo == 1)
        //                m_strSQL += " order by  numerodiario ";
        //            if (oC.TipoNumeracionProtocolo == 2)
        //                m_strSQL += " order by prefijosector, numerosector ";
        //            if (oC.TipoNumeracionProtocolo == 3)
        //                m_strSQL += " order by numeroTipoServicio ";
        //        }


        //        DataSet Ds = new DataSet();
        //        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //        SqlDataAdapter adapter = new SqlDataAdapter();
        //        adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //        adapter.Fill(Ds);
        //        gvLista.DataSource = Ds.Tables[0];
        //        gvLista.DataBind();
        //        //dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
                


        //        if (Ds.Tables[0].Rows.Count > 0)
        //        {
        //            dtProtocolo = Ds.Tables[0];
        //            ultimafila = Ds.Tables[0].Rows.Count - 1;

        //        }
              
        //    CurrentPageIndex = int.Parse(Request["idProtocolo"].ToString());
        //    CurrentIndexGrilla = int.Parse(Request["Index"].ToString());
        //    ////CurrentPageIndex = int.Parse(Ds.Tables[0].Rows[CurrentIndexGrilla].ItemArray[0].ToString());
        //    UltimaPageIndex = ultimafila; // int.Parse(Ds.Tables[0].Rows[ultimafila].ItemArray[0].ToString());
        //    lblCantidadRegistros.Text =dtProtocolo.Rows.Count.ToString() + " protocolos encontrados";

         
        //}


    

        private int CurrentPageIndex /* Guardamos el indice de página actual */
        {
            get { return ViewState["CurrentPageIndex"] == null ? 0 : int.Parse(ViewState["CurrentPageIndex"].ToString()); }
            set { ViewState["CurrentPageIndex"] = value; }
        }

        private int CurrentIndexGrilla /* Guardamos el indice de página actual */
        {
            get { return ViewState["CurrentIndexGrilla"] == null ? 0 : int.Parse(ViewState["CurrentIndexGrilla"].ToString()); }
            set { ViewState["CurrentIndexGrilla"] = value; }
        }

        private int UltimaPageIndex /* Guardamos el indice de página actual */
        {
            get { return ViewState["UltimaPageIndex"] == null ? 0 : int.Parse(ViewState["UltimaPageIndex"].ToString()); }
            set { ViewState["UltimaPageIndex"] = value; }
        }

        private void MuestraDatos(string p)
        {
            HFOperacion.Value = Request["Operacion"].ToString();
            ///Muestra los datos de encabezado para el protocolo seleccionado
            CargarListasObservaciones("gral");

            //Actualiza los datos de los objetos : alta o modificacion .                                        
            Protocolo oRegistro = new Protocolo();
            oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(p));

            lblServicio.Text = oRegistro.IdTipoServicio.Nombre.ToUpper();
            //if (Request["Operacion"].ToString() == "HC") { oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString())); }
            if (oRegistro.tieneAdjuntoProtocolo())
            {   spanadjunto.Visible = true; }
            else
            {   spanadjunto.Visible = false; }
            if (Request["Operacion"].ToString() == "Valida") imgAdjunto.Visible = true;

            HFIdProtocolo.Value = CurrentPageIndex.ToString();
            if ((oRegistro.IdTipoServicio.IdTipoServicio == 1) || (oRegistro.IdTipoServicio.IdTipoServicio == 6))//Laboratorio o forense (no se muestran solapas de ais y ATB
            {
                pnlMicroOrganismo.Visible = false;                
                pnlAntibiograma.Visible = false;
                tituloAntibiograma.Visible = false;
                tituloMicroOrganismo.Visible = false;
                hplPesquisa.Visible = false;
            }

            if (oRegistro.IdTipoServicio.IdTipoServicio == 5) //microbiologia de no pacientes
            {
                pnlPaciente.Visible = false;
                pnlProducto.Visible = true;
                divAntecedentes.Visible = false;
                lblTituloFechaTomaMuestra.Visible = false;
                btnMostrarResultados.Visible = false;
            }
            if (oRegistro.IdTipoServicio.IdTipoServicio == 4) //Pesquisa
            {
                SolicitudScreening oSolicitud = new SolicitudScreening();
                oSolicitud = (Business.Data.Laboratorio.SolicitudScreening)oSolicitud.Get(typeof(Business.Data.Laboratorio.SolicitudScreening), "IdProtocolo", oRegistro);

                if (Request["Operacion"].ToString() == "Valida")
                {
                    if (oSolicitud != null)
                    {
                        int i_alarmas = oSolicitud.GetCantidadAlarmas();
                        imgPesquisa.ToolTip = "Tiene " + i_alarmas.ToString() + " alarmas";
                        if (i_alarmas > 0) imgPesquisa.Visible = true; else imgPesquisa.Visible = false;
                        imgPesquisa.Attributes.Add("onClick", "javascript: PesquisaNeonatalView (" + oRegistro.IdProtocolo.ToString() + ",650,280); return false");

                        hplPesquisa.Visible = true;
                        hplPesquisa.Attributes.Add("onClick", "javascript: PesquisaNeonatalView (" + oRegistro.IdProtocolo.ToString() + ",650,280); return false");
                    }
                }
                pnlMicroOrganismo.Visible = false;
                pnlAntibiograma.Visible = false;
                tituloAntibiograma.Visible = false;
                tituloMicroOrganismo.Visible = false;
                
                //SolicitudScreening oTarjeta = new SolicitudScreening();
                //oTarjeta = (SolicitudScreening)oTarjeta.Get(typeof(SolicitudScreening), "IdProtocolo", oRegistro);
                if (oSolicitud != null)
                {
                    if (Request["Operacion"].ToString() == "HC")
                        lblMuestra.Text = "Tarjeta Nro.: " + oSolicitud.NumeroTarjeta.ToString();
                    else
                        lblMuestra.Text = " Nro.: " + oSolicitud.NumeroTarjeta.ToString();
                }                    
                lblServicio.Text = "P. NEONATAL";
                
            }
            

                if ((oRegistro.IdTipoServicio.IdTipoServicio == 3)|| (oRegistro.IdTipoServicio.IdTipoServicio == 5)) //Microbiologia
            {
                hplPesquisa.Visible = false;
                if (Request["Operacion"].ToString() == "Valida")
                {
                    btnEditarAntibiograma.Visible=false;
                    btnAgregarAntibiogramaValidado.Visible = true;

                    chkWhonet.Visible = true;
                    ProtocoloWhonet oRegistroWhonet = new ProtocoloWhonet();
                    oRegistroWhonet = (ProtocoloWhonet)oRegistroWhonet.Get(typeof(ProtocoloWhonet), "IdProtocolo", oRegistro);
                    if (oRegistroWhonet != null) chkWhonet.Checked = true;
                    else chkWhonet.Checked = false;
                }

                lblPlaca.Text = s_placa;
                CargarListasAislamientos();
                CargarListaAntibiotico();
                CargarListasAntibiogramas();
                

                if (oRegistro.IdMuestra > 0)
                {
                    Muestra oMuestra = new Muestra();
                    oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), oRegistro.IdMuestra);
                    if (oMuestra!=null)
                    lblMuestra.Text = " Tipo de Muestra: " + oMuestra.Nombre; 
                    
                }
              
                if (oRegistro.IdConservacion > 0)
                {
                    Conservacion oConserva = new Conservacion();
                    oConserva = (Conservacion)oConserva.Get(typeof(Conservacion), oRegistro.IdConservacion);
                    if (oConserva!=null)
                    lblConservacion.Text = oConserva.Descripcion;
                }
                if (oRegistro.IdCaracter > 0)
                {
                    lblCovid.Visible = true;
                    Caracter oCaracter = new Caracter();
                    oCaracter = (Caracter)oCaracter.Get(typeof(Caracter), oRegistro.IdCaracter);
                    if (oCaracter!= null)
                    lblCovid.Text = "Clasificación Covid-19: " + oCaracter.Nombre;
                    if (oRegistro.FechaInicioSintomas.Year != 1900)
                        lblCovid.Text = lblCovid.Text + " - FIS: " + oRegistro.FechaInicioSintomas.ToShortDateString();
                    if (oRegistro.FechaUltimoContacto.Year != 1900)
                        lblCovid.Text = lblCovid.Text + " - FUC: " + oRegistro.FechaUltimoContacto.ToShortDateString();
                }

                pnlMicroOrganismo.Visible = true;
                pnlAntibiograma.Visible = true;
                tituloMicroOrganismo.Visible = true;
                tituloAntibiograma.Visible = true;
                 
                CargarGrillaAislamientos();
                ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
                if (Request["Operacion"].ToString() == "HC")
                {
                    pnlMicroOrganismo.Enabled = false;
                    pnlMicroorganismoHC.Visible = true;
                }
             

                
            }



            //tituloCalidad.Visible = true;
            //this.IncidenciaEdit1.MostrarDatosdelProtocolo(oRegistro.IdProtocolo);

            if (oRegistro.getIncidencias() > 0)
            {
                inci.Visible = true;
                tieneInci.Visible = true;
            }
            else
            {
                inci.Visible = false;
                tieneInci.Visible = false;
            }
            switch (oRegistro.Estado)
            {
                case 0:
                    {
                        if (Request["Operacion"].ToString() == "Carga")  btnActualizarPracticasCarga.Visible = true;// en la carga de edita si el protocolo no tiene validaciones
                        imgEstado.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                        lblEstado1.CssClass = "label label-danger";
                        lblEstado1.Text = "NO PROCESADO";
                    //    imgImprimir.Visible = false;
                      //  pnlImpresora.Visible = false;
                        imgPdf.Visible = false; btnRestringirAcceso.Visible = false;
                    }
                    break;
                case 1:
                    { imgEstado.ImageUrl = "~/App_Themes/default/images/amarillo.gif";
                        lblEstado1.CssClass = "label label-warning";
                        lblEstado1.Text = "EN PROCESO";


                        btnRestringirAcceso.Visible = false;
                    }
                    
                    break;
                case 2:
                    {
                        imgEstado.ImageUrl = "~/App_Themes/default/images/verde.gif";
                        lblEstado1.CssClass = "label label-success";
                        lblEstado1.Text = "TERMINADO"; btnRestringirAcceso.Visible = true;
                        if (Request["Operacion"].ToString() == "Carga")
                        {
                     
                            //btnGuardar.Enabled = false;
                            btnGuardar.Visible = false;
                            chkFormula.Enabled = false;
                            btnAplicarFormula.Enabled = false;
                            ////OCULTAR OPCIONES PARA CREAR O ELIMINAR ANTIBIOGRAMAS

                            btnGuardarAislamientos.Visible = false;
                            btnAgregarGermen.Visible = false;
                            
                            btnAgregarAntibiograma.Visible = false;
                            btnEliminarAntibiograma.Visible = false;
                            ddlAntibiograma.Visible = false;
                          
                        }
                   
                    } break;

                case 3: // restringido
                    {
                        imgEstado.ImageUrl = "~/App_Themes/default/images/lock.png";
                        lblEstado1.CssClass = "label label-success";
                        lblEstado1.Text = "ACCESO RESTRINGIDO";
                        if (Request["Operacion"].ToString() == "Carga")
                        {
                       
                            //btnGuardar.Enabled = false;
                            btnGuardar.Visible = false;
                            chkFormula.Enabled = false;
                            btnAplicarFormula.Enabled = false;
                            ////OCULTAR OPCIONES PARA CREAR O ELIMINAR ANTIBIOGRAMAS

                            btnGuardarAislamientos.Visible = false;
                            btnAgregarGermen.Visible = false;

                            btnAgregarAntibiograma.Visible = false;
                            btnEliminarAntibiograma.Visible = false;
                            ddlAntibiograma.Visible = false;

                        }
                        if (oRegistro.IdTipoServicio.IdTipoServicio!= 6)
                        {
                            btnRestringirAcceso.Visible = true;
                            btnRestringirAcceso.Text = "Ver Restricción";
                            btnDeshacerRestringirAcceso.Visible = true;
                        }
                        else
                        {
                            btnRestringirAcceso.Visible = false;
                            btnDeshacerRestringirAcceso.Visible = false;
                        }

                    
                    }
                    break;

            }
            btnRestringirAcceso.Visible = oCon.HabilitaNoPublicacion;

            ///si tiene aislamientos o antibiogramas no se puede editar practicas
            if ((aisl.Visible) || (anti.Visible)) btnActualizarPracticasCarga.Visible = false;
            ///////////////////////////

            lblUsuario.Text =  oRegistro.IdUsuarioRegistro.Apellido;
            lblUsuario1.Text =   oRegistro.IdUsuarioRegistro.Apellido;

            lblFechaRegistro.Text = oRegistro.FechaRegistro.ToShortDateString();
            lblFechaRegistro1.Text = oRegistro.FechaRegistro.ToShortDateString();

            int len = oRegistro.FechaRegistro.ToString().Length - 11;
            lblHoraRegistro.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);
            lblHoraRegistro1.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);

            lblFecha.Text = oRegistro.Fecha.ToShortDateString();
            lblFecha1.Text = oRegistro.Fecha.ToShortDateString();

            if (oRegistro.FechaTomaMuestra.ToShortDateString() == "01/01/1900")
                lblFechaTomaMuestra.Text = "";
            else lblFechaTomaMuestra.Text = oRegistro.FechaTomaMuestra.ToShortDateString();
            lblFechaTomaMuestra1.Text = lblFechaTomaMuestra.Text;

            lblProtocolo.Text =   oRegistro.GetNumero().ToString();
            lblProtocolo1.Text = lblProtocolo.Text;

            //hplProtocolo.NavigateUrl = "../Protocolos/ProtocoloEdit2.aspx?idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString()+ "&Operacion=Modifica&idProtocolo=" +oRegistro.IdProtocolo.ToString();

           
                lblOrigen.Text = oRegistro.IdOrigen.Nombre;
             

            lblSolicitante.Text = oRegistro.IdEfectorSolicitante.Nombre;
            lblSolicitante2.Text = oRegistro.IdEfectorSolicitante.Nombre;

            lblObservacion.Text = oRegistro.Observacion;
            lblObservacion1.Text = oRegistro.Observacion;
            if (oRegistro.MatriculaEspecialista != "-1")
                lblMedico.Text = oRegistro.Especialista + " MP:" + oRegistro.MatriculaEspecialista;
          

            lblPrioridad.Text =  oRegistro.IdPrioridad.Nombre;
            if (oRegistro.IdPrioridad.Nombre == "URGENTE")
            {
                lblPrioridad.ForeColor = Color.Red;
                lblPrioridad.Font.Bold = true;
            }

            //if (oCon.IdSectorDefecto == 0)
            //{
               
            
            lblSector.Text = oRegistro.IdSector.Nombre;
            lblSector1.Text = lblSector.Text;
            //}
            if (oRegistro.Sala != "") lblSector.Text += " Sala: " + oRegistro.Sala;
            if (oRegistro.Cama != "") lblSector.Text += " Cama: " + oRegistro.Cama;

            lblDescripcionProducto.Text = oRegistro.DescripcionProducto;
            ///Datos del Paciente            
            if (oRegistro.IdPaciente.IdEstado==2) lblDni.Text = "(Sin DU Temporal)";            
            else lblDni.Text = oRegistro.IdPaciente.NumeroDocumento.ToString();
            

            if (oRegistro.IdTipoServicio.IdTipoServicio == 4)
                lblPaciente.Text = oRegistro.getDatosParentesco();
            else
                lblPaciente.Text = oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();
            /////fin de datos del paciente


            lblSexo.Text = oRegistro.IdPaciente.getSexo();
            lblFechaNacimiento.Text = oRegistro.IdPaciente.FechaNacimiento.ToShortDateString();
            lblEdad.Text = oRegistro.Edad.ToString();
            switch (oRegistro.UnidadEdad)
            {
                case 0: lblEdad.Text += " años"; break;
                case 1: lblEdad.Text += " meses"; break;
                case 2: lblEdad.Text += " días"; break;                
            }


      

            lblNumeroOrigen.Text = oRegistro.NumeroOrigen;
            lblNumeroOrigen1.Text = lblNumeroOrigen.Text;

            /////Observaciones en el ingreso de protocolo
            if (Request["Operacion"].ToString() != "HC")
            {

                if (oRegistro.Observacion.Trim() != "")
                {
                    pnlObservaciones.Visible = true;
                    lblObservacion.Text = oRegistro.Observacion;
                    lblObservacion1.Text = oRegistro.Observacion;

                }
                else
                {
                    pnlObservaciones.Visible = false;
                    lblObservacion1.Text = "";
                }
            }
            else
            {
                if (Request["validado"] == "0")
                { }
                else
                {
                    pnlObservaciones.Visible = false;
                    lblObservacion1.Text = "";
                }
            }

            ////////////////////////////////////////
            string embarazada="";
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            IList lista = crit.List();
            if (lista.Count > 0)
            {
                foreach (ProtocoloDiagnostico oDiag in lista)
                {
                    Cie10 oD = new Cie10();
                    oD = (Cie10)oD.Get(typeof(Cie10), oDiag.IdDiagnostico);
                    if  ( lblDiagnostico.Text=="") lblDiagnostico.Text =  oD.Nombre;
                    else    lblDiagnostico.Text += " - " + oD.Nombre;

                    if (oD.Codigo=="Z32.1") embarazada="E";
                }
            }

            //oRegistro.IdPaciente.getCodificaHiv(); //
            lblCodigoPaciente.Text = oRegistro.getCodificaHiv(embarazada); //lblSexo.Text.Substring(0, 1) + " " + oRegistro.IdPaciente.Nombre.Substring(0, 2) + oRegistro.IdPaciente.Apellido.Substring(0, 2) + " " + lblFechaNacimiento.Text.Replace("/", "") + embarazada;
            //lblCodigoPaciente.Text = lblCodigoPaciente.Text.ToUpper();
            
            ///Observaciones de Resultados al pie 
            if (oRegistro.ObservacionResultado != "")
                {
                lblObservacionResultado.Visible = true;
                lblObservacionResultado.Text =" Observaciones: " + oRegistro.ObservacionResultado;
                }
            if (Request["Operacion"].ToString() != "HC")
            {
                if (oRegistro.ObservacionResultado != "")
                {
                    txtObservacion.Text = oRegistro.ObservacionResultado;
                    //pnlObsGeneral.Visible = true;
                    //btnObservaciones.Enabled = false;
                }
                else
                {
                    //pnlObsGeneral.Visible = false;
                    //btnObservaciones.Enabled = true;
                }

            }
          


            //if (Request["desde"].ToString() != "Urgencia")
            //{ 
            if (Request["desde"] != null)
            { 
            if (Request["desde"].ToString() != "Urgencia")
            {
              //  if (esHemoterapia())
              if (oUser.IdArea>0)
                {
                    btnActualizarPracticas.Enabled = false;
                    btnActualizarPracticasCarga.Enabled = false;
                }
            }
                }
          

        }

        //private int esdeArea()
        //{
        //    Usuario oUser = new Usuario();

        //    if (Request["Operacion"].ToString() == "Valida")
        //        return oUserValida.IdArea;
        //    else
        //        return oUser.IdArea;

           

        //}

        //private bool esHemoterapia()
        //{
        //    Usuario oUser = new Usuario();

        //    if (Request["Operacion"].ToString() == "Valida")


        //        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
        //    else
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
        //    return oUser.esHemoterapia();


        //}


        private void LlenarTabla(string p)
        {
            //bool hayAntecedente = false;
            string m_strSQL = " 1=1 ";
                
                //" SELECT grupo,item, iditem, resultadoNum, ResultadoCar, observaciones,idCategoria," +
                //              " idTipoResultado, UnidadMedida, Estado, Metodo, valorReferencia, '' as MaximoReferencia, '' as observacionReferencia ," +
                //              " userCarga, trajoMuestra ,'' as tipoValorReferencia, conresultado, " +
                //              " formatoDecimal,  formato0,  formato1, formato2, formato3,  formato4 , resultadoDefecto, userControl, iddetalleProtocolo, codificaHiv, userValida,estadoObservacion " +                              
                //              " FROM vta_LAB_Resultados " +
                //              " WHERE   idProtocolo = " + p;
                              

            if (Request["idArea"].ToString() != "0")   m_strSQL += " and idArea in (" + Request["idArea"].ToString()+")";


            //////////////////control de hemoterapia            
            //if (Request["desde"] != null)
            //{
            //    if (Request["desde"].ToString() != "Urgencia")
            //    { if (esHemoterapia()) m_strSQL += " and idItem in (select iditem from lab_item where codigo like '433%')"; }
            //}
            ////////////////////

            if (Request["Operacion"].ToString() == "HC")
            {
                if (Request["validado"].ToString() == "1") { m_strSQL += " and estado=2 "; }
                m_strSQL += " order by ordenArea, orden, idDetalleProtocolo ";  //orden de impresion

             

            }
            else
            { 
                if (oCon.OrdenCargaResultado)/// orden de impresion
                    m_strSQL += " order by ordenArea, orden, idDetalleProtocolo ";  //orden de impresion
                else
                    m_strSQL += " order by  idDetalleProtocolo ";  //orden de impresion
                
            }



            //DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            //adapter.Fill(Ds);


            /*nuevo con SP*/

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
             
                cmd.CommandText = "[LAB_Resultados]";


            cmd.Parameters.Add("@idProtocolo", SqlDbType.Int);
            cmd.Parameters["@idProtocolo"].Value = int.Parse(p);


            cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
            cmd.Parameters["@FiltroBusqueda"].Value = m_strSQL;

            //cmd.Parameters.Add("@desde", SqlDbType.Int);
            //cmd.Parameters["@desde"].Value = 0;

            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);

            /*fin de nuevo*/

            string s= System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;

    
            string m_titulo= "";
            string m_hijo = "";
            string m_nombre = "";
 


            TableRow objFila_TITULO = new TableRow();
            TableCell objCellAnalisis_TITULO = new TableCell();
            TableCell objCellResultado_TITULO = new TableCell();
            TableCell objCellResultadoAnterior_TITULO = new TableCell();
            TableCell objCellUnMedida_TITULO = new TableCell();
            TableCell objCellValoresReferencia_TITULO = new TableCell();
            TableCell objCellValida_TITULO = new TableCell();
            TableCell objCellPersona_TITULO = new TableCell();
            TableCell objCellObservaciones_TITULO = new TableCell();
            TableCell objCellImagenes_TITULO = new TableCell();



            Label lblAnalisis = new Label();
            lblAnalisis.Text = "ANALISIS";
            objCellAnalisis_TITULO.Controls.Add(lblAnalisis);
               

            Label lblResultado = new Label();
            lblResultado.Text = "RESULTADO";            
            objCellResultado_TITULO.Controls.Add(lblResultado);
            

            Label lblResultadoAnterior = new Label();
            lblResultadoAnterior.Text = "R.ANTER.";
            objCellResultadoAnterior_TITULO.Controls.Add(lblResultadoAnterior);
            

            if (Request["Operacion"].ToString() != "HC")
            {
                Label lblUM = new Label();
                lblUM.Text = "UM";
                objCellUnMedida_TITULO.Controls.Add(lblUM);
            }

            Label lblVR = new Label();
            lblVR.Text = "VR|METODO";           
            objCellValoresReferencia_TITULO.Controls.Add(lblVR);

            


            Label lblValida = new Label();
            if ((Request["Operacion"].ToString() == "Carga") || (Request["Operacion"].ToString() == "HC")) lblValida.Text = "";
            else
            {   if (Request["Operacion"].ToString() == "Valida")  lblValida.Text = "VAL";
                if (Request["Operacion"].ToString() == "Control") lblValida.Text = "CTRL";
            }
            objCellValida_TITULO.Controls.Add(lblValida);

            Label lblCargadoPor = new Label();
            if ((Request["Operacion"].ToString() == "HC")&&(Request["validado"].ToString() == "1"))
            {
                lblCargadoPor.Text = "VALIDADO POR";
//                Panel1.ScrollBars = ScrollBars.None;
            }
            else
                lblCargadoPor.Text = "ESTADO";
            
            objCellPersona_TITULO.Controls.Add(lblCargadoPor);


            /////observaciones
            //if (Request["Operacion"].ToString() == "Valida")
            //{
            if (Request["Operacion"].ToString() != "HC")
            {
                Label lblObservaciones = new Label();
                lblObservaciones.Text = "OBS.";               
                //    else
                //        lblObservaciones.Text = "";
                objCellObservaciones_TITULO.Controls.Add(lblObservaciones);

                objCellObservaciones_TITULO.Width = Unit.Pixel(60);
            }
            //}             
            if (Request["Operacion"].ToString() != "HC")
            {
                Label lblImagenes = new Label();
                lblImagenes.Text = "IMG";
              
                objCellImagenes_TITULO.Controls.Add(lblImagenes);
                objCellImagenes_TITULO.Width = Unit.Pixel(60);
            }
            //}           

            objFila_TITULO.Cells.Add(objCellAnalisis_TITULO);
            objFila_TITULO.Cells.Add(objCellResultado_TITULO);

          


            if ((Request["Operacion"].ToString() == "Valida")||  (Request["Operacion"].ToString() == "Control")) objFila_TITULO.Cells.Add(objCellResultadoAnterior_TITULO);

            if (Request["Operacion"].ToString() != "HC") objFila_TITULO.Cells.Add(objCellUnMedida_TITULO);
            
            objFila_TITULO.Cells.Add(objCellValoresReferencia_TITULO);

            if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
            {
                //objFila_TITULO.Cells.Add(objCellResultadoAnterior_TITULO);
                objFila_TITULO.Cells.Add(objCellValida_TITULO);
            }


            objFila_TITULO.Cells.Add(objCellPersona_TITULO);
            objFila_TITULO.Cells.Add(objCellObservaciones_TITULO);
            if (Request["Operacion"].ToString() != "HC") objFila_TITULO.Cells.Add(objCellImagenes_TITULO);


            objFila_TITULO.CssClass = "myLabelIzquierda";
            objFila_TITULO.BackColor = Color.Gainsboro;

            
            
            Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

            //'añadimos la fila a la tabla
            if (objFila_TITULO != null)   tContenido.Controls.Add(objFila_TITULO);//.Rows.Add(objRow);    

                     

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {  
                //decimal m_minimoReferencia=-1;
                //decimal m_maximoReferencia=-1;
                bool algovalidado = false;
                

                string valorReferencia = Ds.Tables[0].Rows[i].ItemArray[11].ToString();
                int m_idItem = int.Parse(Ds.Tables[0].Rows[i].ItemArray[2].ToString());
                string unMedida = Ds.Tables[0].Rows[i].ItemArray[8].ToString();
                string Observaciones = Ds.Tables[0].Rows[i].ItemArray[5].ToString();
                int tiporesultado = (int.Parse(Ds.Tables[0].Rows[i].ItemArray[7].ToString()));
                int tipodeterminacion = int.Parse(Ds.Tables[0].Rows[i].ItemArray[6].ToString());
              
                int estado = int.Parse(Ds.Tables[0].Rows[i].ItemArray[9].ToString());
                if (estado == 2) algovalidado = true;

                string m_metodo = Ds.Tables[0].Rows[i].ItemArray[10].ToString();
           
                string m_observacionReferencia =Ds.Tables[0].Rows[i].ItemArray[13].ToString();
                string m_usuarioCarga = Ds.Tables[0].Rows[i].ItemArray[14].ToString();
                string m_trajoMuestra = Ds.Tables[0].Rows[i].ItemArray[15].ToString();
                string m_tipoValorReferencia = Ds.Tables[0].Rows[i].ItemArray[16].ToString();
                string m_conResultado = Ds.Tables[0].Rows[i].ItemArray[17].ToString();
                string m_formatoDecimal= Ds.Tables[0].Rows[i].ItemArray[18].ToString();
                string m_formato0 = Ds.Tables[0].Rows[i].ItemArray[19].ToString();
                string m_formato1 = Ds.Tables[0].Rows[i].ItemArray[20].ToString();
                string m_formato2 = Ds.Tables[0].Rows[i].ItemArray[21].ToString();
                string m_formato3 = Ds.Tables[0].Rows[i].ItemArray[22].ToString();
                string m_formato4 = Ds.Tables[0].Rows[i].ItemArray[23].ToString();
                string m_resultadoDefecto = Ds.Tables[0].Rows[i].ItemArray[24].ToString();
                string m_usuariocontrol = Ds.Tables[0].Rows[i].ItemArray[25].ToString();
                string m_usuariovalida = Ds.Tables[0].Rows[i].ItemArray[28].ToString();
                int i_iddetalleProtocolo= int.Parse( Ds.Tables[0].Rows[i].ItemArray[26].ToString());
                string m_codificaPaciente = Ds.Tables[0].Rows[i].ItemArray[27].ToString();

                string m_estadoObservacion = Ds.Tables[0].Rows[i].ItemArray[29].ToString();

                if (m_codificaPaciente == "True")
                {
                    lblPaciente.Visible = false;
                    lblCodigoPaciente.Visible = true;
                }

                m_hijo = Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                m_titulo = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                
                TableRow objFila = new TableRow();
                TableCell objCellAnalisis = new TableCell();
                TableCell objCellResultado = new TableCell();
                TableCell objCellResultadoAnterior = new TableCell();
                TableCell objCellUnMedida = new TableCell();
                TableCell objCellValoresReferencia = new TableCell();
                TableCell objCellValida = new TableCell();
                TableCell objCellPersona = new TableCell();
                TableCell objCellObservaciones = new TableCell();
                TableCell objCellImagenes = new TableCell();
                objCellResultadoAnterior.Width= Unit.Pixel(80);
                objCellUnMedida.Width = Unit.Pixel(60);
                objCellImagenes.Width = Unit.Pixel(60);
                objCellObservaciones.Width = Unit.Pixel(60);

                decimal x = 0;
               

                if ((m_hijo != m_titulo)&& (m_nombre!=m_titulo)) ///poner titulo de la practica
                {                    
                        TableRow objRow = new TableRow();
                        TableCell objCell = new TableCell();
                        Label lbl0 = new Label();
                        lbl0.Text = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                        lbl0.TabIndex = short.Parse("500");
                        lbl0.Font.Bold = true;

                        Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(lbl0);
                        objCell.Controls.Add(lbl0);
                        if (Request["Operacion"].ToString() == "HC")
                            objCell.ColumnSpan =6;
                        else
                            objCell.ColumnSpan=8;
                        
                        objRow.Cells.Add(objCell);                      
                        objRow.CssClass = "myLabelIzquierda";
                        tContenido.Controls.Add(objRow);
                      
                        m_nombre = m_titulo;                                                        
                }
                
                  

                Label lbl1 = new Label();
                if (m_hijo == m_titulo) lbl1.Text =m_hijo; 
                else  lbl1.Text = "&nbsp;&nbsp;&nbsp;" + m_hijo; 

                lbl1.TabIndex = short.Parse("500");
                lbl1.ForeColor = Color.Black;
                lbl1.Font.Size = FontUnit.Point(9);


                DetalleProtocolo oDetalle = new DetalleProtocolo();
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);
                Item oItem = new Item();
                oItem = oDetalle.IdSubItem; // (Item)oItem.Get(typeof(Item), m_idItem);


                if (tipodeterminacion != 0)
                {
                    TableRow objRowTitulo = new TableRow();
                    TableCell objCellTitulo = new TableCell();
                    lbl1.Font.Bold = true;
                    lbl1.Font.Italic = true;
                    if (Request["Operacion"].ToString() == "HC")
                        objCellTitulo.ColumnSpan = 6;
                    else
                        objCellTitulo.ColumnSpan = 8;

                    objCellTitulo.Controls.Add(lbl1);
                    objRowTitulo.Cells.Add(objCellTitulo);
                    objRowTitulo.CssClass = "myLabelIzquierda";
                    tContenido.Controls.Add(objRowTitulo);

                    /// puede ser tambien derivable el titulo 
                    //if  (oDetalle.esDerivado()) //(oItem.esDerivado(oCon.IdEfector))
                    //{
                        Label lblDerivacion = new Label();
                        lblDerivacion.Font.Italic = true;
                        lblDerivacion.TabIndex = short.Parse("500");
                        //Verifica el estado de la derivacion
                        string estadoDerivacion = "";
                        Derivacion oDeriva = new Derivacion();
                        oDeriva = (Derivacion)oDeriva.Get(typeof(Derivacion), "IdDetalleProtocolo", oDetalle);
                        if (oDeriva != null)  /// esta pendiente
                        {
                        //    estadoDerivacion = "Pendiente de Derivacion";
                        //    lblDerivacion.ForeColor = Color.Red;
                        //}
                        //else
                        //{
                        /* if (oDeriva.Estado == 0) /// pendiente                            
                         {
                             estadoDerivacion = "Pendiente de Derivacion";
                             lblDerivacion.ForeColor = Color.Red;
                         }
                         if (oDeriva.Estado == 1) /// enviado
                             estadoDerivacion = oDetalle.ResultadoCar; //  "Derivado: " + oItem.GetEfectorDerivacion(oCon.IdEfector);
                         if (oDeriva.Estado == 2) /// no enviado
                             estadoDerivacion = oDetalle.ResultadoCar; //" No Derivado. " + oDeriva.Observacion;

                         if (oDeriva.Estado == 4) // --> Pendiente de Derivar
                          {
                             estadoDerivacion = oDetalle.ResultadoCar;
                             lblDerivacion.ForeColor = Color.Red;
                         }
                        */
                            estadoDerivacion = oDetalle.ResultadoCar; //Para todos los estados
                            lblDerivacion.Font.Bold = true;

                            if (oDeriva.Resultado != "")
                                estadoDerivacion += " - Resultado Informado: " + oDeriva.Resultado;
                        //}

                        lblDerivacion.Text = estadoDerivacion;
                        objCellResultado.ColumnSpan = 1;
                        objCellResultado.Controls.Add(lblDerivacion);
                       }
                    if (m_trajoMuestra == "No")
                    {
                        Label lblSinMuestra = new Label();
                        lblSinMuestra.TabIndex = short.Parse("500");
                        lblSinMuestra.Text = "Sin Muestra";// +oItem.IdEfectorDerivacion.Nombre; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                        lblSinMuestra.Font.Italic = true;
                        lblSinMuestra.ForeColor = Color.Blue;
                        //     objCellResultado.ColumnSpan = 5;
                        objCellResultado.Controls.Add(lblSinMuestra);
                    }


                }
                else
                {
                    objCellAnalisis.Controls.Add(lbl1);

                     

                    if ((oDetalle.IdUsuarioPreValida>0) && (oDetalle.IdUsuarioValida==0))
                            { estado = 2; }
                    //     string m_idSuperItem = oDetalle.IdItem.IdItem.ToString();

                     
                  string   s_placaaux = oDetalle.getPlaca();
                    if (s_placaaux != "")
                    {
                        if (s_placa == "")
                            s_placa = s_placaaux;
                        else
                            s_placa += "-" + s_placaaux;
                    }



                    ///Antes de mostrar el control verifica  si está derivado                    
                    //if (oItem.IdEfectorDerivacion != oItem.IdEfector) //es derivado
                    //   if (oItem.esDerivado(oCon.IdEfector))
                    //if (oDetalle.esDerivado()) //(oItem.esDerivado(oCon.IdEfector))
                    //{
                        Label lblDerivacion = new Label();
                        lblDerivacion.Font.Italic = true;
                        lblDerivacion.TabIndex = short.Parse("500");
                        //Verifica el estado de la derivacion
                        string estadoDerivacion = "";
                        Derivacion oDeriva = new Derivacion();
                        oDeriva = (Derivacion)oDeriva.Get(typeof(Derivacion), "IdDetalleProtocolo", oDetalle);
                        if (oDeriva != null)  /// esta pendiente
                        {
                            //    estadoDerivacion = "Pendiente de Derivacion";
                            //    lblDerivacion.ForeColor = Color.Red;
                            //}
                            //else
                            //{
                            /*if (oDeriva.Estado == 0) /// pendiente                            
                            {
                                estadoDerivacion = oDetalle.ResultadoCar; //"Pendiente de Derivacion";
                                lblDerivacion.ForeColor = Color.Red;
                            }
                            if (oDeriva.Estado == 1) /// enviado
                                estadoDerivacion = oDetalle.ResultadoCar; //"Derivado: " + oItem.GetEfectorDerivacion(oCon.IdEfector);
                            if (oDeriva.Estado == 2) /// no enviado
                                estadoDerivacion = oDetalle.ResultadoCar; // " No Derivado. " + oDeriva.Observacion;
                            if (oDeriva.Estado == 4) // --> Pendiente de Derivar
                            {
                                estadoDerivacion = oDetalle.ResultadoCar;
                                lblDerivacion.ForeColor = Color.Red;
                            }*/
                            estadoDerivacion = oDetalle.ResultadoCar; //Para todos los estados
                            lblDerivacion.Font.Bold = true;

                                if (oDeriva.Resultado != "")
                                    estadoDerivacion += " - Resultado Informado: " + oDeriva.Resultado; 
                       

                            lblDerivacion.Text = estadoDerivacion;
                            objCellResultado.ColumnSpan = 1;
                            objCellResultado.Controls.Add(lblDerivacion);
                        }
                    //}
                    else
                    {//No es derivado                     
                        if (m_trajoMuestra == "No")
                        {
                            Label lblSinMuestra = new Label();
                            lblSinMuestra.TabIndex = short.Parse("500");
                            lblSinMuestra.Text = "Sin Muestra";// +oItem.IdEfectorDerivacion.Nombre; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            lblSinMuestra.Font.Italic = true;
                            lblSinMuestra.ForeColor = Color.Blue;
                            //     objCellResultado.ColumnSpan = 5;
                            objCellResultado.Controls.Add(lblSinMuestra);
                        }
                        else
                        {
                            if (tipodeterminacion == 0)// si es una determinacion simple
                            { string s_determinacion = oDetalle.IdSubItem.IdItem.ToString();
                                string s_idPaciente = oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString();
                                switch (tiporesultado)//tipoResultado
                                {
                                


                                    case 4://Checklistbox
                                        {
                                            if (Request["Operacion"].ToString() != "HC")
                                            {
                                                ///   Verifica si la determinacion tiene una lista predeterminada de resultados
                                                ISession m_session = NHibernateHttpModule.CurrentSession;
                                                ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                                                crit.Add(Expression.Eq("IdItem", oItem));
                                                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                                                crit.Add(Expression.Eq("Baja", false));
                                              ///  crit.AddOrder(Order.Asc("Resultado")); ///El orden lo define el usuario
                                                IList resultados = crit.List();
                                                if (resultados.Count > 0)
                                                {
                                                    Anthem.CheckBoxList chk1 = new Anthem.CheckBoxList();
                                                    //CheckBoxList chk1 = new CheckBoxList();
                                                    chk1.Visible = false;
                                                    chk1.AutoCallBack = true;
                                                    chk1.RepeatColumns = 2;
                                                    chk1.Width = Unit.Pixel(300);
                                                    chk1.Attributes.Add("ScrollBars", "Horizontal");
                                                    chk1.ID = "c" + m_idItem.ToString();
                                                    //chk1.ID = "c" + m_idSuperItem +";"+m_idItem.ToString(); 
                                                    chk1.TabIndex = short.Parse(i + 1.ToString());

                                                    foreach (ResultadoItem oResultado in resultados)
                                                    {
                                                        ListItem Item = new ListItem();
                                                        Item.Value = oResultado.IdResultadoItem.ToString();
                                                        Item.Text = oResultado.Resultado;
                                                        chk1.Items.Add(Item);
                                                        if (oResultado.ResultadoDefecto)
                                                            m_resultadoDefecto = oResultado.Resultado;
                                                    }

                                                    chk1.AutoPostBack = true;
                                                    chk1.SelectedIndexChanged += new EventHandler(chk1_SelectedIndexChanged);

                                                     
                                                    chk1.CssClass = "myList";


                                                    Anthem.TextBox txt1 = new Anthem.TextBox();
                                                    //TextBox txt1 = new TextBox();

                                                    txt1.ID = m_idItem.ToString();
                                                    txt1.ReadOnly = true;// no es posible editar como texto, sino que agregar /quitar desde las opciones
                                                    //txt1.ID =m_idSuperItem +";"+ m_idItem.ToString();
                                                    txt1.TabIndex = short.Parse(i + 1.ToString());
                                                    txt1.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                                                    txt1.TextMode = TextBoxMode.MultiLine;
                                                    txt1.Width = Unit.Percentage(95);
                                                    txt1.Rows = 2;
                                                    txt1.MaxLength = 200;
                                                    txt1.CssClass = "form-control input-sm"; //mytexto
                                                    txt1.ToolTip = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                                                     if (m_conResultado == "False") txt1.Text = m_resultadoDefecto;
                                                    ////////////////////
                                                    if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                                    {
                                                        if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio!=5)
                                                        {

                                                            //Caro Performance: se comenta por defecto la busqueda del resultado anterior 

                                                            string resultadoAnterior = "";// oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                        
                                                        //        resultadoAnterior =   oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                        //    if (resultadoAnterior != "")
                                                        //{
                                                            //hayAntecedente = true;
                                                            Label olblResultadoAnterior = new Label();
                                                            olblResultadoAnterior.TabIndex = short.Parse("500");
                                                            olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                            olblResultadoAnterior.ToolTip = "Haga clic aquí para ver más datos.";
                                                                olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                            olblResultadoAnterior.ForeColor = Color.Green;
                                                            olblResultadoAnterior.Visible = false;
                                                            olblResultadoAnterior.ID = "ResAnterior"+ s_determinacion;
                                                            //   olblResultadoAnterior.Width = Unit.Pixel(20);
                                                            olblResultadoAnterior.Text = resultadoAnterior;
                                                            olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + s_determinacion + "," + s_idPaciente + ",790,420); return false");
                                                            objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                        //}
                                                        }
                                                    }

                                                    //Boton de desplegar y/o ocultar las opciones predefinidas para elegir.
                                                    Anthem.ImageButton btnAddDetalle = new Anthem.ImageButton();
                                                    //ImageButton btnAddDetalle = new ImageButton();
                                                    btnAddDetalle.TabIndex = short.Parse("500");
                                                    //btnAddDetalle.AutoUpdateAfterCallBack = true;

                                                    btnAddDetalle.ID = "b" + m_idItem.ToString();
                                                    //btnAddDetalle.ID = "b" + m_idSuperItem+";"+ m_idItem.ToString();
                                                    btnAddDetalle.ToolTip = "Desplegar opciones";
                                                    btnAddDetalle.ImageUrl = "~/App_Themes/default/images/add.png";
                                                    //btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                    //btnAddDetalle.Attributes.Add("onClick", "javascript: PredefinidoSelect (" + oDetalle.IdDetalleProtocolo.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                    btnAddDetalle.Click += new ImageClickEventHandler(btnAddDetalle_Click);

                                                if ((estado > 0) && (Request["Operacion"].ToString() == "Carga")) //si esta controlado o validado pinta la celda
                                                    {
                                                    btnAddDetalle.Enabled=false;
                                                  //      chk1.BackColor = Color.GhostWhite;
                                                    //    chk1.Enabled = false;
                                                        txt1.Enabled = false;
                                                    }

                                                    if ((estado == 2) && (Request["Operacion"].ToString() == "Control")) //si esta validado y entro a controlar no puedo modificar
                                                    {
                                                    //    chk1.BackColor = Color.GhostWhite;
                                                      //  chk1.Enabled = false;
                                                        btnAddDetalle.Enabled=false;
                                                        txt1.Enabled = false;
                                                    }




                                                    objCellResultado.Controls.Add(txt1);
                                                    objCellResultado.Controls.Add(chk1);

                                                    objCellResultado.Controls.Add(btnAddDetalle);
                                                    ////Habilitacion de observaciones
                                                    ImageButton btnObservacionDetalle2 = new ImageButton();
                                                    btnObservacionDetalle2.TabIndex = short.Parse("500");
                                                    btnObservacionDetalle2.ID = "OBS" + i_iddetalleProtocolo.ToString();
                                                    //btnObservacionDetalle2.ID = "Obs2|" + oDetalle.IdDetalleProtocolo.ToString() + "|" + m_estadoObservacion.ToString();//  m_idItem.ToString();

                                                    if (oDetalle.Observaciones != "")//tiene observaciones
                                                    {

                                                        if (oDetalle.IdUsuarioValidaObservacion == 0)
                                                            btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                        else
                                                            btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                    }
                                                    else
                                                    {

                                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                    }

                                                    btnObservacionDetalle2.AlternateText = oDetalle.Observaciones;
                                                    btnObservacionDetalle2.ToolTip = "Observaciones para " + lbl1.Text.Replace("&nbsp;", "");


                                                    btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");

                                                    objCellObservaciones.Controls.Add(btnObservacionDetalle2);
                                                    ////////////////////                                        
                                                    ///IMAGENES ADJUNTAS
                                                    if (Request["Operacion"].ToString() != "HC")
                                                    {
                                                        ImageButton btnImagen = new ImageButton();
                                                        btnImagen.TabIndex = short.Parse("500");
                                                        btnImagen.ID = "IMG" + i_iddetalleProtocolo.ToString(); // Caro Performance


                                                        if (oDetalle.tieneAdjuntoNoVisible())//tiene observaciones
                                                        {
                                                            btnImagen.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                            btnImagen.ToolTip = "Adjunto no imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                        else
                                                        {
                                                            if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                            {
                                                                btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                                btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                            }
                                                            else
                                                            {
                                                                btnImagen.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                                btnImagen.ToolTip = "Subir adjunto  para " + lbl1.Text.Replace("&nbsp;", "");
                                                            }
                                                        }




                                                        btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                        objCellImagenes.Controls.Add(btnImagen);
                                                    }
                                                    // fin de imagenes adjuntas
                                                    ////////////////////////////////////////////////////////////////////                                              
                                                }
                                            }
                                            else
                                            {
                                                Label olbl = new Label();
                                                olbl.Font.Name = "Courier";
                                                olbl.Font.Size = FontUnit.Point(9);
                                                if (m_conResultado == "False")
                                                    olbl.Text = "";
                                                else
                                                    olbl.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                                               
                                                if (oDetalle.Observaciones != "")
                                                {
                                                    if (olbl.Text == "")
                                                        olbl.Text = oDetalle.Observaciones;
                                                    else
                                                        olbl.Text += Environment.NewLine + oDetalle.Observaciones;
                                                }
                                                objCellResultado.Controls.Add(olbl);
                                            }
                                        } //fin case 
                                        break;

                                    case 3://Lista predefinida de resultados
                                        {
                                            if (Request["Operacion"].ToString() != "HC")
                                            {
                                                ///Verifica si la determinacion tiene una lista predeterminada de resultados
                                                ISession m_session = NHibernateHttpModule.CurrentSession;
                                                ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                                                crit.Add(Expression.Eq("IdItem", oItem));
                                                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                                                crit.Add(Expression.Eq("Baja", false));
                                               /// crit.AddOrder(Order.Asc("Resultado"));  //el orden lo define el usuario 
                                                IList resultados = crit.List();
                                                if (resultados.Count > 0)
                                                {
                                                    DropDownList ddl1 = new DropDownList();
                                                    ddl1.Width = Unit.Pixel(200);
                                                    ddl1.ID = m_idItem.ToString();
                                                    //ddl1.ID = m_idSuperItem + ";" + m_idItem.ToString();
                                                    ddl1.TabIndex = short.Parse(i + 1.ToString());

                                                    ListItem ItemSeleccion = new ListItem();
                                                    ItemSeleccion.Value = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                                                    ItemSeleccion.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                                                    ddl1.Items.Add(ItemSeleccion);

                                                    foreach (ResultadoItem oResultado in resultados)
                                                    {
                                                        ListItem Item = new ListItem();
                                                        Item.Value = oResultado.IdResultadoItem.ToString();
                                                        Item.Text = oResultado.Resultado;
                                                        ddl1.Items.Add(Item);
                                                        if (oResultado.ResultadoDefecto)
                                                            m_resultadoDefecto = oResultado.IdResultadoItem.ToString();
                                                    }

                                                    if ((m_resultadoDefecto != "") && (m_conResultado == "False"))
                                                    {
                                                        ddl1.SelectedValue = m_resultadoDefecto;
                                                    }

                                                    ddl1.SelectedIndexChanged += new EventHandler(ddl1_SelectedIndexChanged);
                                                    ddl1.Attributes.Add("onkeypress", "javascript:return Enter(this, event)");

                                                    ///estad=0 Carga
                                                    ///estado=1 control
                                                    ///estado=2 valida
                                                    if ((estado > 0) && (Request["Operacion"].ToString() == "Carga")) //si esta controlado o validado pinta la celda
                                                    {
                                                        ddl1.BackColor = Color.GhostWhite;
                                                        ddl1.Enabled = false;
                                                    }

                                                    if ((estado == 2) && (Request["Operacion"].ToString() == "Control")) //si esta validado y entro a controlar no puedo modificar
                                                    {
                                                        ddl1.BackColor = Color.GhostWhite;
                                                        ddl1.Enabled = false;
                                                    }
                                                    ddl1.CssClass = "myList";
                                                    objCellResultado.Controls.Add(ddl1);
                                                    ////////////////////
                                                    if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                                    {
                                                        if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio != 5)
                                                        {


                                                            //Caro Performance: se comenta por defecto la busqueda del resultado anterior
                                                            string resultadoAnterior = "";// oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                            //if (chkMostrarResultadosAnteriores.Checked)
                                                            //    resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                            //if (resultadoAnterior != "")
                                                            //{
                                                                //hayAntecedente = true;
                                                                Label olblResultadoAnterior = new Label();
                                                                olblResultadoAnterior.TabIndex = short.Parse("500");
                                                                olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                                olblResultadoAnterior.ToolTip = "Haga clic aquí para ver más datos.";
                                                                //  olblResultadoAnterior.Font.Bold = true;
                                                                olblResultadoAnterior.ForeColor = Color.Green;
                                                                olblResultadoAnterior.Visible = false;
                                                                olblResultadoAnterior.ID = "ResAnterior" + s_determinacion;
                                                                olblResultadoAnterior.Width = Unit.Pixel(20);
                                                                olblResultadoAnterior.Text = resultadoAnterior;
                                                                olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + s_determinacion + "," + s_idPaciente + ",790,420); return false");
                                                                objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                            //}
                                                        }
                                                    }

                                               


                                                    ////Otra forma de observacion
                                                    ImageButton btnObservacionDetalle2 = new ImageButton();
                                                    btnObservacionDetalle2.TabIndex = short.Parse("500");
                                                    btnObservacionDetalle2.ID = "OBS" + i_iddetalleProtocolo.ToString();
                                                    //btnObservacionDetalle2.ID = "Obs2|" + oDetalle.IdDetalleProtocolo.ToString() + "|" + m_estadoObservacion.ToString();//  m_idItem.ToString();

                                                    if (oDetalle.Observaciones != "")//tiene observaciones
                                                    {

                                                        if (oDetalle.IdUsuarioValidaObservacion == 0)
                                                            btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                        else
                                                            btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                    }
                                                    else
                                                    {

                                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                    }

                                                    btnObservacionDetalle2.AlternateText = oDetalle.Observaciones;
                                                    btnObservacionDetalle2.ToolTip = "Observaciones para " + lbl1.Text.Replace("&nbsp;", "");
                                                    
                                                    
                                                    btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() +",'"+Request["Operacion"].ToString()+"'); return false");

                                                    objCellObservaciones.Controls.Add(btnObservacionDetalle2);

                                                    ////////////////////                                        
                                                    ///IMAGENES ADJUNTAS
                                                    if (Request["Operacion"].ToString() != "HC")
                                                    { 
                                                    ImageButton btnImagen = new ImageButton();
                                                    btnImagen.TabIndex = short.Parse("500");
                                                    btnImagen.ID = "IMG" + i_iddetalleProtocolo.ToString();


                                                    if (oDetalle.tieneAdjuntoNoVisible())//tiene observaciones
                                                    {
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                        btnImagen.ToolTip = "Adjunto no imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                    }
                                                    else
                                                    { 
                                                        if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                        { 
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                        btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                        else
                                                        { 
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                        btnImagen.ToolTip = "Subir adjunto  para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                    }                                              
                                                    
                                                    


                                                    btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                    objCellImagenes.Controls.Add(btnImagen);
                                                    }
                                                    // fin de imagenes adjuntas


                                                }
                                            }
                                            else
                                            { ///HC
                                                Label olbl = new Label();
                                                olbl.Font.Name = "Courier";
                                                olbl.Font.Size = FontUnit.Point(9);
                                                if (m_conResultado == "False")
                                                    olbl.Text = "";
                                                else
                                                    olbl.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();

                                                objCellResultado.Controls.Add(olbl);
                                                if (oDetalle.Observaciones != "")
                                                {
                                                    if (olbl.Text == "")
                                                        olbl.Text = oDetalle.Observaciones;
                                                    else
                                                        olbl.Text += Environment.NewLine + oDetalle.Observaciones;
                                                }
                                            }
                                        } //fin case 3
                                        break;

                                    case 1: //numerico
                                        {
                                            string expresionControlDecimales = oUtil.ExpresionFormato(int.Parse(m_formatoDecimal));
                                            //string expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
                                            switch (m_formatoDecimal)
                                            {
                                                case "0":
                                                    {
                                                        //expresionControlDecimales = "[-+]?\\d*";
                                                     //   expresionControlDecimales = "[-+]?\\d+";
                                                        x = decimal.Parse(m_formato0);
                                                    }
                                                    break;
                                                case "1":
                                                    {
                                                        //expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,1}";
                                                      //  expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,1}";
                                                        x = decimal.Parse(m_formato1);
                                                    }
                                                    break;
                                                case "2":
                                                    {
                                                        //                                                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
                                                     //   expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,2}";
                                                        x = decimal.Parse(m_formato2);
                                                    }
                                                    break;
                                                case "3":
                                                    {
                                                        //expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,3}";
                                                      //  expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,3}";
                                                        x = decimal.Parse(m_formato3);
                                                    }
                                                    break;
                                                case "4":
                                                    {
                                                        //expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,4}";
                                                     //   expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,4}";
                                                        x = decimal.Parse(m_formato4);
                                                    }
                                                    break;
                                            }

                                            if (Request["Operacion"].ToString() != "HC")
                                            {
                                                objCellResultado.CssClass = "CeldaContenedor";
                                                objCellResultado.Width = Unit.Pixel(150);
                                                TextBox txt1 = new TextBox();
                                                txt1.ID = m_idItem.ToString();
                                                //txt1.ID = m_idSuperItem + ";" + m_idItem.ToString();
                                                txt1.Attributes.Add("onkeypress", "javascript:return Enter(this, event)");
                                                txt1.TabIndex = short.Parse(i + 1.ToString());
                                                if (m_conResultado == "True") txt1.Text = x.ToString(System.Globalization.CultureInfo.InvariantCulture); 
                                                if (m_conResultado == "False") txt1.Text = m_resultadoDefecto;
                                                txt1.Width = Unit.Pixel(60);
                                              
                                                txt1.CssClass = "form-control input-sm";


                                                if (Request["Operacion"].ToString() != "Carga") //validacion
                                                {
                                                    // if (VerificaValorReferencia(m_minimoReferencia, m_maximoReferencia, x, m_tipoValorReferencia))
                                                    
                                                    if (oDetalle.VerificaValorReferencia(x))
                                                        txt1.ForeColor = Color.Black;
                                                    else
                                                    {
                                                        if (m_conResultado == "True") txt1.ForeColor = Color.Red;
                                                        else txt1.ForeColor = Color.Black;
                                                    }
                                                }

                                               

                                                //  objCellResultado.Controls.Add(imgAceptaValor);
                                                objCellResultado.Controls.Add(txt1);


                                                ///etiqueta de unidad de medida
                                                Label olblUM = new Label();
                                                     olblUM.ID = "UM" + m_idItem.ToString();
                                                olblUM.Font.Size = FontUnit.Point(7);
                                                olblUM.Text = unMedida;
                                                objCellResultado.Controls.Add(olblUM);
                                                olblUM.Visible = false;
                                                //////////////////////////////////////////////////////

                                             
                                                if (oItem.TieneFormula()) //Si el item se calcula por formula se muestra inhabilitado
                                                {
                                                    chkFormula.Visible = true;
                                                    lblFormula.Visible = true;
                                                    btnAplicarFormula.Visible = true;
                                                   
                                                 


                                                    /// seleccionar la formula a calcular
                                                    CheckBox ochkFormula = new CheckBox();
                                                    ochkFormula.Checked = true;                                                    
                                                    ochkFormula.ID = "F" + m_idItem.ToString();
                                                    ochkFormula.Text = "F(x)";
                                                    ochkFormula.ForeColor = Color.Red;
                                                    ochkFormula.Font.Size = FontUnit.Point(7);                                                    
                                                    ochkFormula.ToolTip = "Marcar si quiere calcular la fórmula";

                                                    if ((estado > 0) && (Request["Operacion"].ToString() == "Carga")) //si esta controlado o validado pinta la celda
                                                    ochkFormula.Enabled = false;                                                    

                                                    if ((estado == 2) && (Request["Operacion"].ToString() == "Control")) //si esta validado y entro a controlar no p
                                                    ochkFormula.Enabled = false;

                                                    objCellResultado.Controls.Add(ochkFormula);
                                                    
                                                }
                                                if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                                {
                                                    if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio != 5)
                                                    {
                                                        ///Caro Performance: sacar busqueda de resultado anterior en una version simplificada
                                                        string resultadoAnterior = "";// oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                        //if (chkMostrarResultadosAnteriores.Checked)
                                                        //    resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                        //if (resultadoAnterior != "")
                                                        //{
                                                            //hayAntecedente = true;
                                                            Label olblResultadoAnterior = new Label();
                                                            olblResultadoAnterior.TabIndex = short.Parse("500");
                                                            olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                            olblResultadoAnterior.ToolTip = "Haga clic aquí para ver gráfico de evolución";
                                                        //  olblResultadoAnterior.Font.Bold = true;
                                                        olblResultadoAnterior.Visible = false;
                                                        olblResultadoAnterior.ID = "ResAnterior" + s_determinacion;
                                                        olblResultadoAnterior.ForeColor = Color.Green;
                                                            olblResultadoAnterior.Width = Unit.Pixel(20);
                                                            olblResultadoAnterior.Text = resultadoAnterior;
                                                            olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + s_determinacion + "," + s_idPaciente + ",790,420); return false");
                                                            objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                        //}
                                                    }
                                                }

                                                RegularExpressionValidator oValidaNumero = new RegularExpressionValidator();
                                                oValidaNumero.ValidationExpression = expresionControlDecimales;
                                                oValidaNumero.ControlToValidate = txt1.ID;
                                                oValidaNumero.Text = "Formato incorrecto";
                                                oValidaNumero.ValidationGroup = "0";
                                                objCellResultado.Controls.Add(oValidaNumero);

                                     
                                                ////Otra forma de observacion
                                                ImageButton    btnObservacionDetalle2 = new ImageButton();
                                                btnObservacionDetalle2.TabIndex = short.Parse("500");
                                                if (oDetalle.Observaciones != "")//tiene observaciones
                                                {

                                                    if (oDetalle.IdUsuarioValidaObservacion == 0)
                                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                    else
                                                        btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                }
                                                else
                                                {

                                                    btnObservacionDetalle2.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                }
                                                btnObservacionDetalle2.ID = "OBS" + i_iddetalleProtocolo.ToString();
                                                btnObservacionDetalle2.AlternateText = oDetalle.Observaciones;
                                                btnObservacionDetalle2.ToolTip = "Observaciones para " + lbl1.Text.Replace("&nbsp;", "");
                                                btnObservacionDetalle2.Attributes.Add("onClick", "javascript: ObservacionEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString()   +",'"+Request["Operacion"].ToString()+"'); return false");
                                                 
                                                objCellObservaciones.Controls.Add(btnObservacionDetalle2);
                                                
                                                ////////////////////                                        



                                                if ((estado > 0) && (Request["Operacion"].ToString() == "Carga")) //si esta controlado o validado pinta la celda
                                                {
                                                    txt1.BackColor = Color.GhostWhite;
                                                    txt1.Enabled = false;
                                                }

                                                if ((estado == 2) && (Request["Operacion"].ToString() == "Control")) //si esta validado y entro a controlar no puedo modificar
                                                {
                                                    txt1.BackColor = Color.GhostWhite;
                                                    txt1.Enabled = false;
                                                }

                                                ////////////////////                                        
                                                ///IMAGENES ADJUNTAS
                                                if (Request["Operacion"].ToString() != "HC")
                                                {
                                                    ImageButton btnImagen = new ImageButton();
                                                    btnImagen.TabIndex = short.Parse("500");
                                                    btnImagen.ID = "IMG" + i_iddetalleProtocolo.ToString();



                                                    ///Caro Performance: buscar una sola vez cuantos adjuntos visibles y no visbles tiene
                                                    if (oDetalle.tieneAdjuntoNoVisible())//tiene observaciones
                                                    {
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                        btnImagen.ToolTip = "Adjunto no imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                    }
                                                    else
                                                    {
                                                        if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                        {
                                                            btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                            btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                        else
                                                        {
                                                            btnImagen.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                            btnImagen.ToolTip = "Subir adjunto  para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                    }




                                                    btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                    objCellImagenes.Controls.Add(btnImagen);
                                                }
                                                // fin de imagenes adjuntas
                                            }
                                            else
                                            {
                                                Label olbl = new Label();
                                                olbl.Font.Name = "Courier";
                                                olbl.Font.Size = FontUnit.Point(9);
                                                if (m_conResultado == "False")
                                                    olbl.Text = "";
                                                else
                                                    olbl.Text = x.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + unMedida;

                                                //                                                if (VerificaValorReferencia(m_minimoReferencia, m_maximoReferencia, x, m_tipoValorReferencia))
                                                if (oDetalle.VerificaValorReferencia(x))
                                                    olbl.ForeColor = Color.Black;
                                                else
                                                {
                                                    olbl.ForeColor = Color.Red;
                                                    pnlReferencia.Visible = true;
                                                }


                                                if (oDetalle.Observaciones != "")
                                                {
                                                    if (olbl.Text == "")
                                                        olbl.Text = oDetalle.Observaciones;
                                                    else
                                                        olbl.Text += Environment.NewLine + oDetalle.Observaciones;
                                                    //objCellResultado.Controls.Add(olblObservaciones);
                                                }
                                                objCellResultado.Controls.Add(olbl);
                                            }

                                        } // fin case 1
                                        break;
                                    case 2: //texto
                                        {
                                            if (Request["Operacion"].ToString() != "HC")
                                            {
                                                string resObs = Ds.Tables[0].Rows[i].ItemArray[5].ToString(); 
                                                string resCar = Ds.Tables[0].Rows[i].ItemArray[4].ToString(); 
                                                string resNum = Ds.Tables[0].Rows[i].ItemArray[3].ToString();
                                                TextBox txt1 = new TextBox();
                                                txt1.ID = m_idItem.ToString();
                                              //  txt1.ID = m_idSuperItem + ";" + m_idItem.ToString();
                                               // txt1.Attributes.Add("onkeypress", "javascript:return Enter(this, event)");
                                                txt1.TabIndex = short.Parse(i + 1.ToString());
                                                txt1.Text = resCar;
                                                if ((resCar == "") &&(oDetalle.Enviado == 2) && (estado != 1)) // automatico
                                                {
                                                    if (resNum != "") txt1.Text = resNum.Substring(0,resNum.Length - 2).Replace(",",".");
                                                    if (oDetalle.Enviado == 2) { if (resObs != "") txt1.Text = resObs; }
                                                }
                                                txt1.TextMode = TextBoxMode.MultiLine;
                                                txt1.Width = Unit.Pixel(200);
                                                txt1.Rows = 1;
                                                txt1.MaxLength = 200;
                                                txt1.CssClass = "form-control input-sm";

                                                if (m_conResultado == "False") txt1.Text = m_resultadoDefecto;

                                                if ((estado > 0) && (Request["Operacion"].ToString() == "Carga")) //si esta controlado o validado pinta la celda
                                                {
                                                    txt1.BackColor = Color.GhostWhite;
                                                    txt1.Enabled = false;
                                                }

                                                if ((estado == 2) && (Request["Operacion"].ToString() == "Control")) //si esta validado y entro a controlar no puedo modificar
                                                {
                                                    txt1.BackColor = Color.GhostWhite;
                                                    txt1.Enabled = false;
                                                }

                                                if (Request["Operacion"].ToString() != "Carga") //validacion
                                                {
                                                    if (EsNumerico(txt1.Text))
                                                    {
                                                        decimal x1 = decimal.Parse(txt1.Text.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                                        // if (VerificaValorReferencia(m_minimoReferencia, m_maximoReferencia, x, m_tipoValorReferencia))
                                                        if (oDetalle.VerificaValorReferencia(x1))
                                                            txt1.ForeColor = Color.Black;
                                                        else
                                                        {
                                                            if (m_conResultado == "True") txt1.ForeColor = Color.Red;
                                                            else txt1.ForeColor = Color.Black;
                                                        }
                                                    }
                                                }

                                                objCellResultado.Controls.Add(txt1);

                                                ///etiqueta de unidad de medida
                                                Label olblUM = new Label();
                                                olblUM.ID = "UM" + m_idItem.ToString();
                                                olblUM.Font.Size = FontUnit.Point(7);
                                                olblUM.Text = unMedida;
                                                objCellResultado.Controls.Add(olblUM);
                                                olblUM.Visible = false;
                                                //////////////////////////////////////////////////////
                                                ////////////////////                                        
                                                ///IMAGENES ADJUNTAS
                                                if (Request["Operacion"].ToString() != "HC")
                                                {
                                                    ImageButton btnImagen = new ImageButton();
                                                    btnImagen.TabIndex = short.Parse("500");
                                                    btnImagen.ID = "IMG" + i_iddetalleProtocolo.ToString();


                                                    if (oDetalle.tieneAdjuntoNoVisible())//tiene observaciones
                                                    {
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_cargado.png";
                                                        btnImagen.ToolTip = "Adjunto no imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                    }
                                                    else
                                                    {
                                                        if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                        {
                                                            btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                            btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                        else
                                                        {
                                                            btnImagen.ImageUrl = "~/App_Themes/default/images/obs_normal.png";
                                                            btnImagen.ToolTip = "Subir adjunto  para " + lbl1.Text.Replace("&nbsp;", "");
                                                        }
                                                    }




                                                    btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + i_iddetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                    objCellImagenes.Controls.Add(btnImagen);
                                                }
                                                // fin de imagenes adjuntas
                                            }
                                            else
                                            {

                                                Label olbl = new Label();
                                                olbl.Font.Name = "Courier";
                                                olbl.Font.Size = FontUnit.Point(9);
                                                if (m_conResultado == "False")
                                                    olbl.Text = "";
                                                else
                                                    olbl.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();
                                                objCellResultado.Controls.Add(olbl);
                                            }

                                            ////////////////////
                                            if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                            {
                                                if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio != 5)
                                                {

                                                    ///Caro Performance: sacar busqueda en modo simplificado
                                                    string resultadoAnterior = ""; // oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                    //if (chkMostrarResultadosAnteriores.Checked)
                                                    //    resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, true);
                                                    //if (resultadoAnterior != "")
                                                    //{
                                                        //hayAntecedente = true;
                                                        Label olblResultadoAnterior = new Label();
                                                        olblResultadoAnterior.TabIndex = short.Parse("500");
                                                        olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                        olblResultadoAnterior.ToolTip = "Haga clic aquí para ver más datos.";
                                                        //  olblResultadoAnterior.Font.Bold = true;
                                                        olblResultadoAnterior.ForeColor = Color.Green;
                                                    olblResultadoAnterior.Visible = false;
                                                    olblResultadoAnterior.ID = "ResAnterior" + s_determinacion;
                                                    olblResultadoAnterior.Width = Unit.Pixel(20);
                                                        olblResultadoAnterior.Text = resultadoAnterior;
                                                        olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteAnalisisView (" + s_determinacion + "," + s_idPaciente + ",790,420); return false");
                                                        objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                    //}
                                                }
                                            }

                                        } // fin case 
                                        break;

                                }//fin swicth


                                Label lblPersona = new Label();
                                lblPersona.TabIndex = short.Parse("500");

                                switch (estado)
                                {
                                    case 0: //cargado
                                        {
                                            if (m_usuarioCarga != "")
                                            {
                                                lblPersona.Text = "Carg.: " + m_usuarioCarga + " "+ oDetalle.FechaResultado.ToShortDateString() + " " + oDetalle.FechaResultado.ToShortTimeString();  /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();                                                                                                                                                                                                                              
                                                lblPersona.ForeColor = Color.Black;
                                            }
                                            else
                                            {
                                                if ((oDetalle.Enviado == 2) && (estado != 1))
                                                {
                                                    lblPersona.Text = "AUTOMÁTICO: " + oDetalle.FechaResultado.ToShortDateString() + " " + oDetalle.FechaResultado.ToShortTimeString();
                                                    lblPersona.ForeColor = Color.Red;
                                                }
                                            }
                                        }
                                        break;
                                    case 1: //controlado
                                        {
                                            if (m_usuariocontrol != "")
                                            {
                                                lblPersona.Text = "Ctrl.: " + m_usuariocontrol + " " + oDetalle.FechaControl.ToShortDateString() + " " + oDetalle.FechaControl.ToShortTimeString();
                                                lblPersona.ForeColor = Color.Green;
                                            }
                                        }
                                        break;
                                    case 2: //validado
                                        {
                                            if (Request["Operacion"].ToString() == "Valida")
                                            {
                                                desvalidar = true;
                                                //if (oDetalle.IdUsuarioValida == int.Parse(Session["idUsuarioValida"].ToString())) desvalidar = true;
                                                //if (oDetalle.IdUsuarioPreValida == int.Parse(Session["idUsuarioValida"].ToString())) desvalidar = true;
                                            }

                                            if ((m_usuariovalida == "") && (oDetalle.IdUsuarioValidaObservacion > 0))
                                            {
                                                Usuario oUser = new Usuario();
                                                oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValidaObservacion);
                                                if (oUser.FirmaValidacion == "") m_usuariovalida = oUser.Apellido + " " + oUser.Nombre;  else m_usuariovalida = oUser.FirmaValidacion;
                                            }
                                           
                                            lblPersona.Text = "Val.: " + m_usuariovalida + " " + oDetalle.FechaValida.ToShortDateString() + " " + oDetalle.FechaValida.ToShortTimeString(); ; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();                                                                                                                                                                                                                              
                                            lblPersona.ForeColor = Color.Blue;

                                            if ((oDetalle.IdUsuarioPreValida > 0) && (oDetalle.IdUsuarioValida== 0))
                                            {
                                                Usuario oUser = new Usuario();
                                                oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioPreValida);

                                                lblPersona.Text = "PreVal.: " + oUser.FirmaValidacion + " " + oDetalle.FechaPreValida.ToShortDateString() + " " + oDetalle.FechaPreValida.ToShortTimeString();
                                                lblPersona.ForeColor = Color.Red;
                                            }


                                        }
                                        break;
                                }





                                /// 
                                lblPersona.Font.Size = FontUnit.Point(6);
                                //lblPersona.Font.Italic = true;

                                if (oDetalle.IdUsuarioImpresion > 0)
                                {
                                    System.Web.UI.WebControls.Image imgImp = new System.Web.UI.WebControls.Image();
                                    imgImp.ImageUrl = "~/App_Themes/default/images/impreso.jpg";
                                    objCellPersona.Controls.Add(imgImp);
                                }
                                objCellPersona.Controls.Add(lblPersona);

                                if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                {
                                    CheckBox chk1 = new CheckBox();
                                    chk1.ID = "chk" + Ds.Tables[0].Rows[i].ItemArray[2].ToString();
                                    if ((estado == 2) && (Request["Operacion"].ToString() == "Control")) //si esta validado y entro a controlar no puedo modificar
                                    {
                                        chk1.Visible = false;

                                    }
                                    objCellValida.Controls.Add(chk1);
                                }

                                if (Request["Operacion"].ToString() != "HC")
                                {

                                    Label lblUMedida = new Label();

                                    //lblUMedida.ID = "UM"+ m_idItem.ToString();
                                    lblUMedida.Font.Italic = true;
                                    lblUMedida.Font.Size = FontUnit.Point(8);

                                    lblUMedida.Text = unMedida;


                                    objCellUnMedida.Controls.Add(lblUMedida);

                                }


                                ///Caro Performance: analizar de grabar valor de referencia al grabar el protocolo-detalle y aca solo mostrar
                                ///importante: hacer un update a los estado menor a 2  de los valores de ref por script
                                ///
                                Label lblValoresReferencia = new Label();
                                lblValoresReferencia.ID = "VR" + m_idItem.ToString();
                                //lblValoresReferencia.ID = "VR"  +m_idSuperItem + ";" + m_idItem.ToString();
                                lblValoresReferencia.Font.Italic = true;
                                lblValoresReferencia.Font.Size = FontUnit.Point(8);
                                if (valorReferencia != "")
                                {// muestra el valor guardado 
                                    lblValoresReferencia.Text = valorReferencia;
                                    if (m_metodo != "")
                                        // lblValoresReferencia.Text += " |Método:" + m_metodo;
                                        lblValoresReferencia.Text += " |" + m_metodo;
                                }
                                //else
                                //    lblValoresReferencia.Text = oDetalle.CalcularValoresReferencia(); ///Caro Performance: el valor de referencia se guarda en la carga de protocolo
                                ///CARO: posibilidad de cambiar valor de refrencia - presentacion solo en validacion o control
                                if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                {
                                    ///if (oDetalle.) si tiene mas de una presentacion
                                    lblValoresReferencia.Attributes.Add("onClick", "javascript:  AnalisisMetodoEdit (" + m_idItem + "," + oDetalle.IdProtocolo.IdProtocolo.ToString() + ",790,420); return false");
                                    lblValoresReferencia.ForeColor = Color.Blue;
                                }
                                objCellValoresReferencia.Controls.Add(lblValoresReferencia);
                            }
                        }
                    }
                }


                if ((Request["Operacion"].ToString() == "Carga") && (algovalidado==true)) // si es carga y tiene algo validado
                {
                    pnlMicroOrganismo.Enabled = false;
                    pnlAntibiograma.Enabled = false;

                }

                if (Request["idServicio"].ToString()!="6")
                    { 
                if ((Request["Operacion"].ToString() == "Valida") && (algovalidado == true)) // si es validacion y tiene algo validado
                    btnRestringirAcceso.Visible = true;
                }
                //else
                //    btnRestringirAcceso.Visible = false;


                ///agrega a la fila cada una de las celdas
                objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);
                    if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control")) objFila.Cells.Add(objCellResultadoAnterior);                
                    if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellUnMedida);

                    objFila.Cells.Add(objCellValoresReferencia);

                    if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))  objFila.Cells.Add(objCellValida);
                    
                    objFila.Cells.Add(objCellPersona);
                    if (Request["Operacion"].ToString() != "HC")  objFila.Cells.Add(objCellObservaciones);

                //////
                if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellImagenes);
                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                    //'añadimos la fila a la tabla
                    if (objFila!=null)
                    tContenido.Controls.Add(objFila);//.Rows.Add(objRow);                                
            }
        }


        protected bool EsNumerico(string val)
        {
            bool match;
            //regula expression to match numeric values
            string pattern = "(^[-+]?\\d+(,?\\d*)*\\.?\\d*([Ee][-+]\\d*)?$)|(^[-+]?\\d?(,?\\d*)*\\.\\d+([Ee][-+]\\d*)?$)";
            //generate new Regulsr Exoression eith the pattern and a couple RegExOptions
            Regex regEx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            //tereny expresson to see if we have a match or not
            match = regEx.Match(val).Success ? true : false;
            //return the match value (true or false)
            return match;
        }      
        private void CargarListasObservaciones(string tipo)
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            string m_ssql = @" SELECT idObservacionResultado , codigo  AS descripcion 
                                FROM   LAB_ObservacionResultado where idTipoServicio=" +Request["idServicio"].ToString()+" and  baja=0 order by codigo " ;
          

            if (tipo == "gral")
            {
                oUtil.CargarCombo(ddlObsCodificadaGeneral, m_ssql, "idObservacionResultado", "descripcion", connReady);
                ddlObsCodificadaGeneral.Items.Insert(0, new ListItem("", "0"));
                ddlObsCodificadaGeneral.UpdateAfterCallBack = true;
            }
            

        }

      



        protected void btnAddDetalle_Click(object sender, EventArgs e)
        {
            Anthem.ImageButton btn1 = (Anthem.ImageButton)sender;


            //////Guarda las observaciones asociadas a un resultado numerico
            string nombre_control = btn1.ID.Replace("b", "c");
            Control txt2 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            if (txt2 != null)
            {
                Anthem.CheckBoxList txtObservacion = (Anthem.CheckBoxList)txt2;
                if (txtObservacion.Visible)
                {                    
                    txtObservacion.Visible = false;
                }
                else
                    txtObservacion.Visible = true;

                txtObservacion.UpdateAfterCallBack = true;
            }

        }

        void chk1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList chk1 = (CheckBoxList)sender;

            string val ="";
            for (int i = 0; i < chk1.Items.Count; i++)
            {
                if (chk1.Items[i].Selected)
                {
                    if (val == "") val = chk1.Items[i].Text;
                    else val += "\n" + chk1.Items[i].Text;//salto de linea
                }
            }
            //////Guarda las observaciones asociadas a un resultado numerico
            string nombre_control = chk1.ID.Replace("c","");
            Control txt2 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            if (txt2 != null)
            {
                Anthem.TextBox txtObservacion = (Anthem.TextBox)txt2;                
                txtObservacion.Text = val;
                txtObservacion.UpdateAfterCallBack = true;
            }

       


        }
   

        protected void ddlAntibiograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarAtb();
            SetSelectedTab(TabIndex.CUARTO);
        }

        private void MostrarAtb()
        {
            if ((ddlAntibiograma.SelectedValue != "0") && (ddlAntibiograma.SelectedValue != ""))
            {
                string[] ais = ddlAntibiograma.SelectedValue.Split(("-").ToCharArray());
                HFIdMetodo.Value = ddlMetodoAntibiograma.SelectedValue;
                HFIdGermen.Value = ais[1].ToString();
                HFIdProtocolo.Value = CurrentPageIndex.ToString();
                HFIdItem.Value = ddlPracticaAtb.SelectedValue;
                HFNumeroAislamiento.Value = ais[0].ToString();
                btnEliminarAntibiograma.Enabled = true;
                btnEditarAntibiograma.Enabled = true;
                if (Request["Operacion"].ToString() == "Valida") { btnValidarAntibiograma.Visible = true; btnValidarTodosAntibiogramas.Visible = true;  btnValidarTodosAntibiogramasPendientes.Visible = true; }
                else { btnValidarAntibiograma.Visible = false; btnValidarTodosAntibiogramas.Visible = false; btnValidarTodosAntibiogramasPendientes.Visible = false; }

            }
        }

        void ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl1 = (DropDownList)sender;
        }

        protected void btnDesValidar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                DesValidar       (oProtocolo);

                string sredirect = "";

                if ((Request["desde"] != null) && (Request["desde"] == "Urgencia"))
                {
                    sredirect = "../Urgencia/UrgenciaList.aspx";
                    Response.Redirect(sredirect);
                }
                else
                    if (Request["urgencia"] != null)
                    {
                        sredirect = "ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Valida&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&urgencia=1&validado=" + Request["validado"].ToString() + "&modo=Normal";
                        Response.Redirect(sredirect);
                    }
                    else


                        //if (sredirect=="")
                        Avanzar(0);
            }
            
        }


        protected void btnValidarPendienteImprimir_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                Guardar(oProtocolo, true, false);
                //Imprimir(oProtocolo, "I");
                Imprimir(oProtocolo, "PDF");
                string sredirect="";

                if ((Request["desde"] != null) && (Request["desde"] == "Urgencia"))
                {
                    sredirect = "../Urgencia/UrgenciaList.aspx";
                    Response.Redirect(sredirect);
                }
                else
                if (Request["urgencia"] != null)
                {
                    sredirect = "ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Valida&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&urgencia=1&validado=" + Request["validado"].ToString() + "&modo=Normal";
                    Response.Redirect(sredirect);    
                }
                else
                
                    
                //if (sredirect=="")
                    Avanzar(1);
                //else
                  //  Response.Redirect(sredirect, false);                    
            }
            
        }

        
        protected void btnValidarPendiente_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                Guardar(oProtocolo, false, false);

                string sredirect="";

                if ((Request["desde"] != null) && (Request["desde"] == "Urgencia"))
                {
                    sredirect = "../Urgencia/UrgenciaList.aspx";
                    Response.Redirect(sredirect);
                }
                else
                if (Request["urgencia"] != null)
                {
                    sredirect = "ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Valida&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&urgencia=1&validado=" + Request["validado"].ToString() + "&modo=Normal";
                    Response.Redirect(sredirect);    
                }
                else
                
                    
                //if (sredirect=="")
                    Avanzar(1);
                //else
                  //  Response.Redirect(sredirect, false);                    
            }
            
        }

//        protected void btnEliminarIncidencia_Click(object sender, EventArgs e)
//        {
//            Protocolo oProtocolo = new Protocolo();
//            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();

//            this.IncidenciaEdit1.EliminarProtocoloIncidencia(oProtocolo);
//            Avanzar(0);
//            SetSelectedTab(TabIndex.ONE);
//}
        

//        protected void btnGuardarIncidencia_Click(object sender, EventArgs e)
//        {
//            Protocolo oProtocolo = new Protocolo();
//            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
//            //if (oProtocolo != null)
//            //{
//                this.IncidenciaEdit1.GuardarProtocoloIncidencia(oProtocolo);
//                Avanzar(0);
//                SetSelectedTab(TabIndex.ONE);
//            //}
//        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                Guardar(oProtocolo, false,true);

                string sredirect="";

                if ((Request["desde"] != null) && (Request["desde"] == "Urgencia"))
                {
                    sredirect = "../Urgencia/UrgenciaList.aspx";
                    Response.Redirect(sredirect);
                }
                else
                if (Request["urgencia"] != null)
                {
                    sredirect = "ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Valida&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&urgencia=1&validado=" + Request["validado"].ToString() + "&modo=Normal";
                    Response.Redirect(sredirect);    
                }
                else
                
                    
                //if (sredirect=="")
                    Avanzar(1);
                //else
                  //  Response.Redirect(sredirect, false);                    
            }
            
        }

        private void Imprimir(Protocolo oProtocolo, string tipo)
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {
                Configuracion oCon = new Configuracion();
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oProtocolo.IdEfector);

                string parametroPaciente = "";
                string parametroProtocolo = "";


                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();

                ParameterDiscreteValue ImprimirHojasSeparadas = new ParameterDiscreteValue();

                ParameterDiscreteValue tipoNumeracion = new ParameterDiscreteValue();
                tipoNumeracion.Value = oCon.TipoNumeracionProtocolo;

                ///////Redefinir el tipo de firma electronica (Serían dos reportes distintos)
                ParameterDiscreteValue conPie = new ParameterDiscreteValue();
                ParameterDiscreteValue conLogo = new ParameterDiscreteValue();
                if (oCon.RutaLogo != "") conLogo.Value = true;
                else
                    conLogo.Value = false;

                if ((oProtocolo.IdTipoServicio.IdTipoServicio == 1) || (oProtocolo.IdTipoServicio.IdTipoServicio == 4)) //laboratorio o pesquisa neonatal.
                {
                    encabezado1.Value = oCon.EncabezadoLinea1;
                    encabezado2.Value = oCon.EncabezadoLinea2;
                    encabezado3.Value = oCon.EncabezadoLinea3;


                    if (oCon.ResultadoEdad) parametroPaciente = "1"; else parametroPaciente = "0";
                    if (oCon.ResultadoFNacimiento) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoSexo) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoDNI) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoHC) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoDomicilio) parametroPaciente += "1"; else parametroPaciente += "0";


                    if (oCon.ResultadoNumeroRegistro) parametroProtocolo = "1"; else parametroProtocolo = "0";
                    if (oCon.ResultadoFechaEntrega) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoSector) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoSolicitante) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoOrigen) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoPrioridad) parametroProtocolo += "1"; else parametroProtocolo += "0";



                    ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultado;

                    conPie.Value = oCon.FirmaElectronicaLaboratorio.ToString();

                    if (oCon.OrdenImpresionLaboratorio)
                    {
                        if (oCon.TipoHojaImpresionResultado == "A4") oCr.Report.FileName = "../Informes/ResultadoSinOrden.rpt";
                        else oCr.Report.FileName = "../Informes/ResultadoSinOrdenA5.rpt";
                    }
                    else
                    {
                        if (oCon.TipoHojaImpresionResultado == "A4") oCr.Report.FileName = "../Informes/Resultado.rpt";
                        else oCr.Report.FileName = "../Informes/ResultadoA5.rpt";
                    }
                }
                if ((oProtocolo.IdTipoServicio.IdTipoServicio == 3) || (oProtocolo.IdTipoServicio.IdTipoServicio == 5) || (oProtocolo.IdTipoServicio.IdTipoServicio == 6)) //microbilogia de pacientes y de no pacientes o forense
                {
                    encabezado1.Value = oCon.EncabezadoLinea1Microbiologia;
                    encabezado2.Value = oCon.EncabezadoLinea2Microbiologia;
                    encabezado3.Value = oCon.EncabezadoLinea3Microbiologia;


                    if (oCon.ResultadoEdadMicrobiologia) parametroPaciente = "1"; else parametroPaciente = "0";
                    if (oCon.ResultadoFNacimientoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoSexoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoDNIMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoHCMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                    if (oCon.ResultadoDomicilioMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";


                    if (oCon.ResultadoNumeroRegistroMicrobiologia) parametroProtocolo = "1"; else parametroProtocolo = "0";
                    if (oCon.ResultadoFechaEntregaMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoSectorMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoSolicitanteMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoOrigenMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                    if (oCon.ResultadoPrioridadMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";

                    ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultadoMicrobiologia;

                    conPie.Value = oCon.FirmaElectronicaMicrobiologia.ToString();


                    if (oCon.TipoHojaImpresionResultadoMicrobiologia == "A4")
                    {
                        if ((oProtocolo.IdTipoServicio.IdTipoServicio == 3) || (oProtocolo.IdTipoServicio.IdTipoServicio == 6))
                        {
                            /// porner ejemplo de tabla
                            /// 

                            oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                        }
                        else
                            oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaNoPacientes.rpt";
                    }
                    else
                    {
                        if ((oProtocolo.IdTipoServicio.IdTipoServicio == 3) || (oProtocolo.IdTipoServicio.IdTipoServicio == 6))
                            oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaA5.rpt";
                        else
                            oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaNoPacientesA5.rpt";
                    }

                }



                ParameterDiscreteValue datosPaciente = new ParameterDiscreteValue();
                datosPaciente.Value = parametroPaciente;

                ParameterDiscreteValue datosProtocolo = new ParameterDiscreteValue();
                datosProtocolo.Value = parametroProtocolo;



                string m_filtro = " WHERE idProtocolo =" + oProtocolo.IdProtocolo;



                if ((oProtocolo.IdTipoServicio.IdTipoServicio == 1) || (oProtocolo.IdTipoServicio.IdTipoServicio == 4)) /// laboratorio o pesquisa neo.
                {

                    if (Request["idArea"].ToString() != "0") m_filtro += " and idArea in (" + Request["idArea"].ToString() + ")";
                    if (Request["Operacion"].ToString() != "Valida")
                    {
                        if (Request["validado"].ToString() == "1") m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 or trajomuestra='No' )";
                    }

                    if (Request["Operacion"].ToString() == "HC")
                    {
                        m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0  or   idUsuarioDerivacion>0 or trajomuestra='No')";
                    }

                    if (Request["Operacion"].ToString() == "Valida")
                    {
                        //    m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )"; // los validados hasta ahora
                        if ((rdbImprimir.SelectedValue == "0") && (Session["tildados"].ToString() != ""))// solo los marcados                
                            m_filtro += " and idSubItem in (" + Session["tildados"] + ")";
                        if (Session["tildados"].ToString() == "")
                            m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 or   idUsuarioDerivacion>0 or trajomuestra='No')";

                    }

                }


                //Caro Performance: ver de no usar la vta_LAB_ImprimirResultados y cambiar a SP --grbar la auditoria en el SP
                oCr.ReportDocument.SetDataSource(oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio, oCon));

                oProtocolo.getQRResultados(oCon);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(conLogo);
                oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(datosPaciente);
                oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(ImprimirHojasSeparadas);
                oCr.ReportDocument.ParameterFields[6].CurrentValues.Add(tipoNumeracion);
                oCr.ReportDocument.ParameterFields[7].CurrentValues.Add(conPie);
                oCr.ReportDocument.ParameterFields[8].CurrentValues.Add(datosProtocolo);

                oCr.DataBind();


                string s_nombreProtocolo = oProtocolo.Numero.ToString();
                /*switch (oCon.TipoNumeracionProtocolo)
                {
                    case 0: s_nombreProtocolo = oProtocolo.Numero.ToString(); break;
                    case 1: s_nombreProtocolo = oProtocolo.NumeroDiario.ToString(); break;
                    case 2: s_nombreProtocolo = oProtocolo.PrefijoSector+ oProtocolo.NumeroSector.ToString(); break;
                    case 3: s_nombreProtocolo = oProtocolo.NumeroTipoServicio.ToString(); break;
                }
                */


                try
                {
                    if (Session["idUsuario"] != null)
                    {
                        Utility oUtil = new Utility();
                        string nombrePDF = oUtil.CompletarNombrePDF(s_nombreProtocolo);

                        oProtocolo.GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(oUser.IdUsuario.ToString()));
                        oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

                    }
                    else Response.Redirect("../FinSesion.aspx", false);
                }
                catch (Exception ex)
                {
                    string exception = "";
                    //while (ex != null)
                    //{
                    exception = ex.Message + "<br>";

                    //} 
                    string popupScript = "<script language='JavaScript'> alert('No se pudo descargar el  informe. Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                    Page.RegisterStartupScript("PopupScript", popupScript);
                }
                //  }

                conn.Close();
            }

        }


    
    protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                if (this.oCr.ReportDocument != null)
                {
                    this.oCr.ReportDocument.Close();
                    this.oCr.ReportDocument.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DesValidar(Protocolo oProtocolo)
        {
            string m_id = "";
            TextBox txt;
            DropDownList ddl;

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

                                                                         
                                                                            m_id = txt.Text;

                                                                            if (Request["Operacion"].ToString() == "Valida") 
                                                                            {
                                                                                if (estaTildado(txt.ID))
                                                                                {

                                                                                DesValidarResultado(txt.ID, txt.Text, oProtocolo, false);
                                                                                    //  GuardarReferenciaMetodoUnidadMedida(txt.ID, oProtocolo);
                                                                                }
                                                                            }
                                                                           
                                                                            //}

                                                                            //}

                                                                        }
                                                                    }

                                                                    if (control5 is DropDownList)
                                                                    {
                                                                        ddl = (DropDownList)control5;
                                                                        if (ddl.Enabled)
                                                                        {
                                                                            if ((ddl.SelectedValue != "") && (Request["Operacion"].ToString() == "Valida") )
                                                                                {
                                                                                    if (estaTildado(ddl.ID))
                                                                                        {

                                                                                        DesValidarResultado(ddl.ID, ddl.SelectedItem.Text, oProtocolo, false);
                                                                                        //   GuardarReferenciaMetodoUnidadMedida(ddl.ID, oProtocolo);

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
            
               
            if (oProtocolo.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString())))
            {
                oProtocolo.Estado = 2;  //validado total (cerrado);
                                        //oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                if (!oProtocolo.Notificarresultado) // no se notifica a sips
                    oProtocolo.Estado = 3; ///Accceso Restringido;
            }
            else
            {
                if (oProtocolo.EnProceso())
                {
                    oProtocolo.Estado = 1;//en proceso
                    // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                }
                else
                    oProtocolo.Estado = 0;
                
            }

           
            oProtocolo.Save();
        }

        private void DesValidarResultado(string m_idItem, string valorItem, Protocolo oProtocolo, bool marcarImpresion)
        {                      
            int usuarioActual = int.Parse(oUser.IdUsuario.ToString());
            //////////////////////////////////////////////////////////////////
            Item oItem = new Item();
            if (valorItem != "Seleccione")
            {
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(m_idItem));
                int tiporesultado = oItem.IdTipoResultado;

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                crit.Add(Expression.Eq("IdSubItem", oItem));
                crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
             
                //crit.Add(Expression.Eq("IdUsuarioValida", usuarioActual));//Solo guarda resultados que no han sido validados
               
                IList detalle = crit.List();
                foreach (DetalleProtocolo oDetalle in detalle)
                {
                       if ((oDetalle.IdUsuarioValida==0 )&& (oDetalle.IdUsuarioPreValida>0)) //Desvalida prevalidacion
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Des-PreValida", usuarioActual);
                        oDetalle.IdUsuarioPreValida = 0;
                    //    oDetalle.ResultadoCar = String.Empty; ;
                   //     oDetalle.ResultadoNum = 0;
                   //     oDetalle.IdUsuarioResultado = 0;
                        oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");
                        oDetalle.Save();
                    }
                    if (oDetalle.IdUsuarioValida > 0) // desvalida validacion
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("DesValida", usuarioActual);
                        oDetalle.IdUsuarioValida = 0;
                  //      oDetalle.ResultadoCar = String.Empty; ;
                   //     oDetalle.ResultadoNum = 0;
                    //    oDetalle.IdUsuarioResultado = 0;
                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                        oDetalle.Save();
                    }                                      
                }
                
            }
        }
        private void Guardar(Protocolo oProtocolo, bool imprimir, bool todo)
        {
            if (Request["Operacion"].ToString() == "Carga")
            { if (Session["idUsuario"] == null) Response.Redirect("../FinSesion.aspx", false); }

            if (Request["Operacion"].ToString() == "Valida")   //Validacion
            { if (Session["idUsuarioValida"] == null) Response.Redirect("../FinSesion.aspx", false); }


            string m_id = "";
            TextBox txt;
            DropDownList ddl;
            GridView gd;

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

                                                                            //if (txt.ID.Substring(0, 1) != "O")
                                                                            //{
                                                                            //if (txt.Text != "")
                                                                            //{
                                                                            m_id = txt.Text;

                                                                            if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                                                            {
                                                                                if (estaTildado(txt.ID))
                                                                                {

                                                                                    GuardarResultado(txt.ID, txt.Text, oProtocolo, imprimir, todo);
                                                                                    //  GuardarReferenciaMetodoUnidadMedida(txt.ID, oProtocolo);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                GuardarResultado(txt.ID, txt.Text, oProtocolo, imprimir, todo);
                                                                                //  GuardarReferenciaMetodoUnidadMedida(txt.ID, oProtocolo);
                                                                            }

                                                                            //}

                                                                            //}

                                                                        }
                                                                    }

                                                                    if (control5 is DropDownList)
                                                                    {
                                                                        ddl = (DropDownList)control5;
                                                                        if (ddl.Enabled)
                                                                        {
                                                                            if (ddl.SelectedValue != "")
                                                                                if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                                                                {
                                                                                    if (estaTildado(ddl.ID))
                                                                                    {

                                                                                        GuardarResultado(ddl.ID, ddl.SelectedItem.Text, oProtocolo, imprimir, todo);
                                                                                        //   GuardarReferenciaMetodoUnidadMedida(ddl.ID, oProtocolo);

                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    GuardarResultado(ddl.ID, ddl.SelectedItem.Text, oProtocolo, imprimir, todo);
                                                                                    //      GuardarReferenciaMetodoUnidadMedida(ddl.ID, oProtocolo);
                                                                                }
                                                                        }
                                                                    }// termina control 5

                                                                    if (control5 is GridView)
                                                                    {
                                                                        gd = (GridView)control5;
                                                                        if (gd.Enabled)
                                                                        {

                                                                            if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                                                                            {
                                                                                if (estaTildado(gd.ID))
                                                                                {
                                                                                    ProtocoloLuminex oFusion = new ProtocoloLuminex();
                                                                                    oFusion.Guardar(gd.ID, oProtocolo.Numero, int.Parse(Session["idUsuarioValida"].ToString()));
                                                                                    GuardarResultado(gd.ID, "Luminex", oProtocolo, imprimir, todo);
                                                                                    //   GuardarReferenciaMetodoUnidadMedida(ddl.ID, oProtocolo);

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

                if (chkWhonet.Visible) oProtocolo.GuardarWhonet(chkWhonet.Checked);
                if ((chkFormula.Checked) && (chkFormula.Visible))
                {
                    oProtocolo.CalcularFormulas(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()), false);
                }
                //oProtocolo.ObservacionResultado = txtObservacion.Text;
                oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                if (Request["Operacion"].ToString() == "Carga")
                {
                    if (oProtocolo.EnProceso())
                    {
                        oProtocolo.Estado = 1;//en proceso
                                              // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                    }
                }
                else
                {
                    if (oProtocolo.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString())))
                    {
                        oProtocolo.Estado = 2;  //validado total (cerrado);
                                                //oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));

                        if (!oProtocolo.Notificarresultado)
                            oProtocolo.Estado = 3; //Acceso Restringido
                    }
                    else
                    {
                        if (oProtocolo.EnProceso())
                        {
                            oProtocolo.Estado = 1;//en proceso
                                                  // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                        }
                    }
                }

                if ((Request["Operacion"].ToString() == "Valida") && (chkCerrarSinResultados.Checked))
                {
                    oProtocolo.Estado = 2;
                    //if (oProtocolo.IdTipoServicio.IdTipoServicio==3) oProtocolo.exportarDatos();
                    oProtocolo.GrabarAuditoriaProtocolo("Terminado", int.Parse(Session["idUsuario"].ToString())); // agrego auditoria de cierre de protocolo
                    if (!oProtocolo.Notificarresultado)
                        oProtocolo.Estado = 3; //Acceso Restringido
                }

                oProtocolo.Save();
            }
        }


        
        
        private void GuardarResultado(string m_idItem, string valorItem , Protocolo oProtocolo, bool marcarImpresion, bool todo)
        {
            Utility oUtil = new Utility();
            string m_metodo = "";
            string m_valorReferencia = "";
            string nombre_control = "VR" + m_idItem;
            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            Label valorRef = (Label)control1;


            ///busca unidad de medida
            nombre_control = "UM" + m_idItem;
            Control controlUMedida = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            Label unMedida = (Label)controlUMedida;


            //////////////////////////////////////////////////////////////////
            Item oItem = new Item();
            if (valorItem != "Seleccione")
            {
                //string[] arrIdItem = m_idItem.Split((";").ToCharArray());
                //m_idItem = arrIdItem[1].ToString();

                oItem = (Item)oItem.Get(typeof(Item), int.Parse(m_idItem));
                int tiporesultado = oItem.IdTipoResultado;

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                crit.Add(Expression.Eq("IdSubItem", oItem));
                crit.Add(Expression.Eq("IdProtocolo", oProtocolo));

                
                //  crit.Add(Expression.Eq("IdEfector", oProtocolo.IdEfector));
                if (!todo) crit.Add(Expression.Eq("IdUsuarioValida", 0));
                 
                if (Request["Operacion"].ToString() == "Carga") crit.Add(Expression.Eq("IdUsuarioValida",0));//Solo guarda resultados que no han sido validados
                if (Request["Operacion"].ToString() == "Control") crit.Add(Expression.Eq("IdUsuarioValida", 0));//Solo guarda resultados que no han sido validados

                IList detalle = crit.List();
               
                if (detalle.Count > 0)
                {
                    foreach (DetalleProtocolo oDetalle in detalle)
                    {
                        switch (tiporesultado)
                        {
                            case 1:// numerico         
                                if ((valorItem.Trim() != "") &&  (oUtil.EsNumerico(valorItem)))

                                {
                                    oDetalle.ResultadoNum = decimal.Parse(valorItem, System.Globalization.CultureInfo.InvariantCulture);
                                    oDetalle.FormatoValida = oItem.FormatoDecimal;
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


                        /////////////////////////////////////////////////////////////////////////////////
                        if ((valorRef != null) || (unMedida != null))
                        {
                            if (valorRef != null)
                            {
                                string[] arr = valorRef.Text.Split(("|").ToCharArray());
                                switch (arr.Length)
                                {
                                    case 1: m_valorReferencia = arr[0].ToString(); break;
                                    case 2:
                                        {
                                            m_valorReferencia = arr[0].ToString();
                                            m_metodo = arr[1].ToString();
                                        } break;
                                }
                                oDetalle.Metodo = m_metodo;
                                oDetalle.ValorReferencia = m_valorReferencia;
                            }

                            if (unMedida != null) oDetalle.UnidadMedida = unMedida.Text;
                        }
                        ///////////////////////////
                    

                     

                        if (Request["Operacion"].ToString() == "Carga")
                        {
                            if (oDetalle.ConResultado)
                            {
                                oDetalle.IdUsuarioResultado = int.Parse(oUser.IdUsuario.ToString());
                                oDetalle.FechaResultado = DateTime.Now;
                            }
                            oDetalle.Save();
                            if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString()));
                        }

                        if ((Request["Operacion"].ToString() == "Valida") && (!oDetalle.Informable))   //Validacion
                        {

                           

                            if (oDetalle.ConResultado)
                            {

                                string res = valorItem;


                                oDetalle.IdUsuarioResultado = int.Parse(oUser.IdUsuario.ToString());
                                oDetalle.FechaResultado = DateTime.Now;



                                oDetalle.Save();
                                if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo("Carga", int.Parse(oUser.IdUsuario.ToString()));
                            }
                        }
                            if ((Request["Operacion"].ToString() == "Valida") &&   (oDetalle.Informable))   //Validacion
                        {  string operacion = "Valida";

                                if (oDetalle.ConResultado)
                            {

                                string res = valorItem;
                                if (valorItem.Length > 10)
                                    res = valorItem.Substring(0, 10);

                                if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente>0) &&  (res == "SE DETECTA"))// GENOMA DE COVID-19"))
                                {
                                  
                                    if (oCon.PreValida)
                                    {
                                        operacion = "PreValida";
                                        oDetalle.IdUsuarioPreValida = int.Parse(oUser.IdUsuario.ToString());
                                        oDetalle.FechaPreValida = DateTime.Now;
                                        oDetalle.IdUsuarioValida = 0;
                                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                                    }
                                    else
                                    {
                                        oDetalle.IdUsuarioValida = int.Parse(oUser.IdUsuario.ToString());
                                        oDetalle.FechaValida = DateTime.Now;

                                        if (marcarImpresion)
                                        {
                                            oDetalle.IdUsuarioImpresion = int.Parse(oUser.IdUsuario.ToString());
                                            oDetalle.FechaImpresion = DateTime.Now;
                                        }

                                        Notificar(oDetalle);

                                    }
                                }
                                else
                                {
                                    
                                        oDetalle.IdUsuarioValida = int.Parse(oUser.IdUsuario.ToString());
                                    oDetalle.FechaValida = DateTime.Now;

                                    if (marcarImpresion)
                                    {
                                        oDetalle.IdUsuarioImpresion = int.Parse(oUser.IdUsuario.ToString());
                                        oDetalle.FechaImpresion = DateTime.Now;
                                    }

                                 //   if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0))
                                        Notificar(oDetalle);

                                }
                                oDetalle.Save();
                                if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(oUser.IdUsuario.ToString()));
                            }


                           
                        }  
                          if (Request["Operacion"].ToString() == "Control")   //Control
                        {
                            //if (estaTildado(m_idItem) && (oDetalle.ConResultado))
                            if (oDetalle.ConResultado)
                            {
                                oDetalle.IdUsuarioControl = int.Parse(oUser.IdUsuario.ToString()); 
                                oDetalle.FechaControl = DateTime.Now;
                                oDetalle.Save();
                                if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString()));
                            }
                        }                                                                         
                    }                 
                }   
            }                       
        }

        private void Notificar(DetalleProtocolo oDetalle)
        {
            //bool notificasisa = oDetalle.NotificarSisa();

            if ((oDetalle.IdUsuarioValida > 0)  && (oDetalle.IdProtocolo.Notificarresultado) )
            {                
                if ((oCon.NotificaAndes) && (oDetalle.IdItem.Codigo == oCon.CodigoCovid)) // solo para covid
                    GenerarNotificacionAndes(oDetalle);
                //Caro Performance: Ver que no notifique a andes


                if (oCon.NotificarSISA)///esta marca es para notificar sisa automatico
                {
                    string idItem = oDetalle.IdProtocolo.GenerarCasoSISA(); // se fija si hay algun item que tiene configurado notificacion a sisa
                    if (idItem != "")
                    {
                      //  int i = 0;
                        //if (oDetalle.IdProtocolo.IdCaracter != 2) // no se suben controles de alta
                        //{
                            //if ((oDetalle.IdProtocolo.IdPaciente.IdEstado != 2))
                            //{
                                string res = oDetalle.ResultadoCar;
                                ProcesaSISA(oDetalle, res);
                                //if (oDetalle.IdProtocolo.VerificarProtocoloAnterior(14))
                                //{
                        //if (res.Length > 10)
                        //            {
                        //                if ((res.Substring(0, 10) == "SE DETECTA") && (oCon.PreValida == false))
                        //                { if (ProcesaSISA(oDetalle, "SE DETECTA")) i = i + 1; }
                        //            }
                        //            if (res.Length > 13)
                        //            {
                        //                if (res.Substring(0, 13) == "NO SE DETECTA")
                        //                { if (ProcesaSISA(oDetalle, "NO SE DETECTA")) i = i + 1; }
                        //            }
                              /*  }// oDetalle.IdProtocolo.VerificarPr*/

                            //}//  if ((oDetalle.IdProtocolo.IdPacie
                      /*  }// if (oDetalle.IdProtocolo.IdCaracter != 2*/
                    }//    if (oCon.NotificarSISA)
                }
            }
        }

        private void GenerarNotificacionAndes(DetalleProtocolo oDetalle)
        {


            try
            {




                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                string URL = ConfigurationManager.AppSettings["urlnotifiacionandes"].ToString();
                string s_token = ConfigurationManager.AppSettings["tokennotifiacionandes"].ToString();
                string s_sexo = "";
                switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                {

                    case 2: s_sexo = "femenino"; break;
                    case 3: s_sexo = "masculino"; break;
                }
                string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToString("dd/MM/yyyy");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");
                string fs = oDetalle.IdProtocolo.FechaOrden.ToString("dd/MM/yyyy");
                string numerodoc = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                if (oDetalle.IdProtocolo.IdPaciente.IdEstado == 2) // temporal
                    numerodoc = "0";

                string error = "";
                string firma = "";
                Usuario oUs = new Usuario(); oUs = (Usuario)oUs.Get(typeof(Usuario), oDetalle.IdUsuarioValida);
                if (oUs != null)

                    firma = oUs.FirmaValidacion;

                NotificacionPaciente newevento = new NotificacionPaciente
                {
                    nombre = oDetalle.IdProtocolo.IdPaciente.Nombre,
                    apellido = oDetalle.IdProtocolo.IdPaciente.Apellido,
                    documento = numerodoc,
                    sexo = s_sexo,
                    fechaNacimiento = DateTime.Parse("01-01-1900"),
                    telefono = oDetalle.IdProtocolo.IdPaciente.InformacionContacto,
                    protocolo = oDetalle.IdProtocolo.Numero.ToString(),
                    resultado = oDetalle.ResultadoCar,
                    fechaSolicitud = DateTime.Parse("01-01-1753"),
                    validador = firma
                };

                const string Comillas = "\"";

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                string DATA = jsonSerializer.Serialize(newevento);
                DATA = DATA.Replace("\"\\/Date(", "").Replace(")\\/\"", "").Replace("-2208978000000", "&&");

                DATA = DATA.Replace("&&", Comillas + fn + Comillas);


                DATA = DATA.Replace("\"\\/Date(", "").Replace(")\\/\"", "").Replace("-6847794000000", "||");

                DATA = DATA.Replace("||", Comillas + fs + Comillas);



                byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                HttpWebRequest request;
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.Timeout = 10 * 1000;
                request.Method = "POST";
                request.ContentLength = data.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", s_token);




                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();
                if (body != "")
                {


                    oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Notificacion Andes ", oDetalle.IdUsuarioValida);

                }



            }
            catch (Exception e)
            {
                //string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }


        }
        private bool ProcesaSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;

            try
            {
                if (oDetalle.IdProtocolo.IdCasoSISA == 0)
                    generacaso =    GenerarCasoSISA_V2(oDetalle, res);



                string m_strSQL = @"SELECT  distinct idDetalleProtocolo,  S.idMuestra as IdMuestraSISA,	  S.idTipoMuestra as idTipoMuestraSISA, s.idPrueba as idPruebaSISA, s.idTipoPrueba as idTipoPruebaSISA,  
                ds.idResultadoSISA,S.idEvento
                  FROM    LAB_DetalleProtocolo d (nolock)
                   inner join LAB_ConfiguracionSISA S (nolock) on S.idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + @" and s.idItem= d.idSubItem
                   inner join LAB_ConfiguracionSISADetalle DS (nolock) on DS.idItem=d.idSubItem  and resultadocar= ds.resultado
                    where d.idProtocolo= " + oDetalle.IdProtocolo.IdProtocolo.ToString();



                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
                string idDetalleProtocolo;
                string idMuestra;
                string idTipoMuestra;
                string idPrueba;
                string idTipoPrueba;
                string idResultadoSISA;
                string idEvento;

                DataTable dt = Ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    idDetalleProtocolo = dt.Rows[i][0].ToString();
                    idMuestra = dt.Rows[i][1].ToString();
                    idTipoMuestra = dt.Rows[i][2].ToString();
                    idPrueba = dt.Rows[i][3].ToString();
                    idTipoPrueba = dt.Rows[i][4].ToString();
                    idResultadoSISA = dt.Rows[i][5].ToString();
                    idEvento = dt.Rows[i][6].ToString();


                    if ((oDetalle.IdProtocolo.IdCasoSISA > 0) && (oDetalle.IdeventomuestraSISA == 0))
                        GenerarMuestraSISA(oDetalle.IdProtocolo, idMuestra, idTipoMuestra, idDetalleProtocolo);

                    if (oDetalle.IdeventomuestraSISA > 0)
                        GenerarResultadoSISA(oDetalle, idPrueba, idTipoPrueba, idResultadoSISA, idEvento);

                    break;
                }

            }
            catch (Exception e)
            {
                generacaso = false;


            }
            return generacaso;

        }
       
        private void GenerarMuestraSISA(Protocolo protocolo, string idMuestraSISA, string idtipoMuestraSISA, string idDetalleProtocolo)

        {
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;
            string URL = oCon.URLMuestraSISA;


             
            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if ((idestablecimientotoma == "") || (idestablecimientotoma == "0"))
                //pongo por defecto laboratorio central
                idestablecimientotoma = "107093";


            ResultadoxNro.EventoMuestra newmuestra = new ResultadoxNro.EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso = protocolo.IdCasoSISA, // 2061287,
                idMuestra = int.Parse(idMuestraSISA),
                idtipoMuestra = int.Parse(idtipoMuestraSISA),
                muestra = true
            };
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            string DATA = jsonSerializer.Serialize(newmuestra);


            byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

            HttpWebRequest request;
            request = WebRequest.Create(URL) as HttpWebRequest;
            request.Timeout = 10 * 1000;
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.ContentType = "application/json";
            request.Headers.Add("app_key", "b0fd61c3a08917cfd20491b24af6049e");
            request.Headers.Add("app_id", "22891c8f");

            try
            {

                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();


                if (body != "")
                {
                    ResultadoxNro.EventoMuestraResultado respuesta_d = jsonSerializer.Deserialize<ResultadoxNro.EventoMuestraResultado>(body);

                    if (respuesta_d.id != 1)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(idDetalleProtocolo));

                        if (oDetalle != null)
                        { 

                            oDetalle.IdeventomuestraSISA = respuesta_d.id;
                            oDetalle.Save();

                            oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + respuesta_d.id.ToString(), oDetalle.IdUsuarioValida);



                        } //for each
                    } //respuesta_o


                }// body

            }


            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

        }
        //private bool GenerarCasoSISA(DetalleProtocolo oDetalle, string res)
        //{
        //    System.Net.ServicePointManager.SecurityProtocol =
        //        System.Net.SecurityProtocolType.Tls12;
        //    bool generacaso = false;
        //    string caracter = "";
        //    string idevento = "";
        //    string nombreevento = "";
        //    string idclasificacionmanual = "";
        //    string nombreclasificacionmanual = "";
        //    string idgrupoevento = "";
        //    string nombregrupoevento = "";
        //    bool seguir = false;
        //    string m_strSQL = "";

        //    try
        //    {
        //        m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=  " + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem=" + oDetalle.IdSubItem.IdItem.ToString();
        //        if ((res == "SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter != 2) && (oCon.CodigoCovid == oDetalle.IdSubItem.Codigo))
        //        {    /// si es positivo y no es controlo de alta se genera un caso sospechoso solo para covid

        //            m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1  and idItem=" + oDetalle.IdSubItem.IdItem.ToString();
        //        }




        //        // nose notificò antes y es sospechoso o contacto



        //            DataSet Ds = new DataSet();
        //            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //            SqlDataAdapter adapter = new SqlDataAdapter();
        //            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //            adapter.Fill(Ds);

        //            DataTable dt = Ds.Tables[0];

        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                caracter = dt.Rows[i][1].ToString();
        //                idevento = dt.Rows[i][2].ToString();
        //                nombreevento = dt.Rows[i][3].ToString();
        //                idclasificacionmanual = dt.Rows[i][4].ToString();
        //                nombreclasificacionmanual = dt.Rows[i][5].ToString();
        //                idgrupoevento = dt.Rows[i][6].ToString();
        //                nombregrupoevento = dt.Rows[i][7].ToString();
        //            seguir = true;
        //            break;

        //            }

        //        if (seguir)
        //        {
        //            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //            string URL = oCon.UrlServicioSISA;
        //            string s_idestablecimiento = oCon.CodigoEstablecimientoSISA; // "14580562167000"
        //            string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
        //            string[] a = usersisa.Split(':');
        //            string s_user = a[0].ToString();
        //            string s_userpass = a[1].ToString();

        //            string s_sexo = "";
        //            switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
        //            {
        //                case 1: s_sexo = "I"; break;
        //                case 2: s_sexo = "F"; break;
        //                case 3: s_sexo = "M"; break;
        //            }
        //            string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");

        //            string fnpapel = oDetalle.IdProtocolo.FechaOrden.ToShortDateString().Replace("/", "-");


        //            string numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();

        //            string error = "";
        //            bool hayerror = false;

        //            evento newevento = new evento
        //            {
        //                idTipodoc = "1",
        //                nrodoc = numerodocumento,
        //                sexo = s_sexo,
        //                fechaNacimiento = fn,  //"05-06-1989",
        //                idGrupoEvento = idgrupoevento,
        //                idEvento = idevento, // "77",
        //                idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
        //                fechaPapel = fnpapel, // "10-12-2019",
        //                idClasificacionManualCaso = idclasificacionmanual, // "22"
        //            };

        //            AltaCaso caso = new AltaCaso
        //            {

        //                usuario = s_user, // "bcpintos",
        //                clave = s_userpass, // "2398HH6RK6",
        //                altaEventoCasoNominal = newevento
        //            };

        //            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        //            string DATA = jsonSerializer.Serialize(caso);



        //            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        //            client.BaseAddress = new System.Uri(URL);

        //            System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");
        //            HttpResponseMessage messge = client.PostAsync(URL, content).Result;
        //            string description = string.Empty;
        //            if (messge.IsSuccessStatusCode)
        //            {
        //                string result = messge.Content.ReadAsStringAsync().Result;
        //                description = result;
        //                RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(description);
        //                if (respuesta_d.id_caso!="" )                        
        //                { //  devolver el idcaso para guardar en la base de datos
        //                    string s_idcaso = respuesta_d.id_caso;

        //                    oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
        //                    oDetalle.IdProtocolo.Save();
        //                    if (respuesta_d.resultado == "OK")
        //                        oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(oUser.IdUsuario.ToString()));
        //                    else
        //                        oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(oUser.IdUsuario.ToString()));
        //                    generacaso = true;

        //                }
        //                else
        //                {
        //                    generacaso = false;
        //                    hayerror = true;
        //                    error = respuesta_d.resultado;

        //                }
        //            }


        //        }

        //    }
        //    catch
        //    {
        //        generacaso = false;


        //    }
        //    return generacaso;

        //}


        private bool GenerarCasoSISA_V2(DetalleProtocolo oDetalle, string res)
        {
            /*Version 2*/
            System.Net.ServicePointManager.SecurityProtocol =
             System.Net.SecurityProtocolType.Tls12;

            bool generacaso = false;
            string caracter = "";
            string idevento = "";
            string nombreevento = "";
            string idclasificacionmanual = "";
            string nombreclasificacionmanual = "";
            string idgrupoevento = "";
            string nombregrupoevento = "";
            bool seguir = false;
            string m_strSQL = "";
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            try
            {
                // query levanta todos los que se generan segun el caracter
                m_strSQL = @" select * from LAB_ConfiguracionSISA where  idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem= " + oDetalle.IdSubItem.IdItem.ToString();
                // si es contacto se sube==>si es negativo como contacto y si es positivo como sospechoso.
                if ((res == "SE DETECTA") && (oDetalle.IdProtocolo.IdCaracter == 4) && (oCon.CodigoCovid == oDetalle.IdSubItem.Codigo))
                {
                    m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1 and idItem= " + oDetalle.IdSubItem.IdItem.ToString();
                }


                m_strSQL += @"  and fechavigenciadesde<=convert(date,convert(varchar,getdate(),112)) 
and ( fechavigenciahasta  >convert(date,convert(varchar,getdate(),112)) or convert(varchar, fechavigenciahasta, 103) = '01/01/1900')
                                and (idorigen=0 or idOrigen=" + oDetalle.IdProtocolo.IdOrigen.IdOrigen.ToString() + ")";

                //Control de efector solicitante
                //Monitoreo de SARS COV - 2 y OVR en ambulatorios ==> solo aplica para Hospital Heller
                //Demas eventos para todos los efectores solicitantes.
                m_strSQL += @" and (idefectorsolicitante=0 or 
                                idefectorsolicitante in (" + oDetalle.IdProtocolo.IdEfectorSolicitante.IdEfector.ToString() + "))";

                //control de embarzada=s /N
                m_strSQL += @"  and soloEmbarazada='" + oDetalle.IdProtocolo.Embarazada.ToString() + "'";
                ///control de edades
                m_strSQL += @" and (" + oDetalle.IdProtocolo.Edad + " between edadDesde and edadHasta and " + oDetalle.IdProtocolo.UnidadEdad + " = 0) ";




                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dt = Ds.Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    caracter = dt.Rows[i][1].ToString();
                    idevento = dt.Rows[i][2].ToString();
                   HdidEventoSISA.Value = idevento;
                    nombreevento = dt.Rows[i][3].ToString();
                    idclasificacionmanual = dt.Rows[i][4].ToString();
                    nombreclasificacionmanual = dt.Rows[i][5].ToString();
                    idgrupoevento = dt.Rows[i][6].ToString();
                    nombregrupoevento = dt.Rows[i][7].ToString();
                    seguir = true;
                    break;
                }

                if (seguir)
                {

                    string URL = oCon.UrlServicioSISA; //"https://ws400-qa.sisa.msal.gov.ar/snvsCasoNominal/v2/snvsCasoNominal"; //
                    string s_idestablecimiento = oCon.CodigoEstablecimientoSISA; // "14580562167000"                    
                    string s_user = "e56f25eb"; // a[0].ToString();"PruebasWSQA_SNVS_ID"; //
                    string s_userpass = "64a16ba3bedbae19e9010e3184fa9926"; // a[1].ToString();"PruebasWSQA_SNVS_KEY"; //

                    string s_sexo = "";
                    switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                    {
                        case 1: s_sexo = "I"; break;
                        case 2: s_sexo = "F"; break;
                        case 3: s_sexo = "M"; break;
                    }
                    string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");
                    string fnpapel = oDetalle.IdProtocolo.FechaOrden.ToShortDateString().Replace("/", "-");
                    string s_numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
                    string error = "";
                    string s_apellido = oDetalle.IdProtocolo.IdPaciente.Apellido;
                    if (s_apellido.Length >= 100) s_apellido = s_apellido.Substring(0, 99);
                    string s_nombre = oDetalle.IdProtocolo.IdPaciente.Nombre;
                    if (s_nombre.Length >= 100) s_nombre = s_nombre.Substring(0, 99);
                    string s_calle = oDetalle.IdProtocolo.IdPaciente.Calle;
                    if (s_calle.Length >= 200) s_nombre = s_calle.Substring(0, 199);
                    string s_telefono = oDetalle.IdProtocolo.IdPaciente.InformacionContacto;
                    if (s_telefono.Length < 7)
                        s_telefono = "sindatos";
                    string s_tipodocumento = "1";
                    if (oDetalle.IdProtocolo.IdPaciente.IdEstado == 2)
                    {
                        s_tipodocumento = "3"; //indocumentado
                        s_numerodocumento = "";
                    }
                    string seDeclaraPuebloIndigena = "No";
                    if (oDetalle.IdProtocolo.IdPaciente.SeDeclaraAborigen)
                        seDeclaraPuebloIndigena = "Si";

                    string s_mail = null;
                    if (oDetalle.IdProtocolo.IdPaciente.Mail != "")
                        s_mail = oDetalle.IdProtocolo.IdPaciente.Mail;
                    //bool hayerror = false;
                    domicilio newdomicilio = new domicilio
                    {
                        calle = s_calle,
                        idDepartamento = null,
                        idLocalidad = null,
                        idProvincia = null,
                        idPais = null,


                    };
                    personaACargo newpersonaaCargo = new personaACargo
                    {
                        tipoDocumento = null,
                        numeroDocumento = null,
                        vinculo = null,

                    };
                    ciudadano newciudadano = new ciudadano
                    {
                        apellido = s_apellido,
                        nombre = s_nombre,
                        tipoDocumento = s_tipodocumento, // 1: DNI- 2: PASAPorte - 3: Indocumentado
                        numeroDocumento = s_numerodocumento,
                        fechaNacimiento = fn,
                        sexo = s_sexo,
                        paisEmisionTipoDocumento = "200",
                        seDeclaraPuebloIndigena = seDeclaraPuebloIndigena,
                        domicilio = newdomicilio,
                        telefono = s_telefono,
                        mail = s_mail,
                        personaACargo = newpersonaaCargo


                    };

                    eventoCasoNominal newevento = new eventoCasoNominal
                    {
                        fechaPapel = fnpapel, // "10-12-2019",                        
                        idGrupoEvento = idgrupoevento,
                        idEvento = idevento, // "77",                      
                        idClasificacionManualCaso = idclasificacionmanual, // "22"
                        idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                    };

                    AltaCasoV2 caso = new AltaCasoV2
                    {
                        ciudadano = newciudadano,
                        eventoCasoNominal = newevento
                    };

                    string DATA = jsonSerializer.Serialize(caso);

                    byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                    HttpWebRequest request;
                    request = WebRequest.Create(URL) as HttpWebRequest;
                    request.Timeout = 10 * 1000;
                    request.Method = "POST";
                    request.ContentLength = data.Length;
                    request.ContentType = "application/json";
                    request.Headers.Add("APP_ID", s_user);
                    request.Headers.Add("APP_KEY", s_userpass);


                    Stream postStream = request.GetRequestStream();
                    postStream.Write(data, 0, data.Length);

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();


                    if (body != "")
                    {
                        //string result = messge.Content.ReadAsStringAsync().Result;
                        //description = result;
                        RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(body);

                        if (respuesta_d.id_caso != "")
                        { //  devolver el idcaso para guardar en la base de datos
                            string s_idcaso = respuesta_d.id_caso;
                            ///grabar a protocolo idCaso
                            //Protocolo protocolo = new Protocolo();
                            //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            if (respuesta_d.resultado == "OK")
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                            else // ERROR_DATOS
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                        }
                        else
                        {
                            generacaso = false;
                            //hayerror = true;
                            error = respuesta_d.resultado;
                        }
                    }

                }

            }

            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                RespuestaCaso respuesta_error = jsonSerializer.Deserialize<RespuestaCaso>(mensaje);
                if (respuesta_error.id_caso != "")
                { //  devolver el idcaso para guardar en la base de datos
                    string s_idcaso = respuesta_error.id_caso;
                    ///grabar a protocolo idCaso
                    //Protocolo protocolo = new Protocolo();
                    //protocolo = (Protocolo)protocolo.Get(typeof(Protocolo), int.Parse(Request["idP"].ToString()));

                    oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                    oDetalle.IdProtocolo.Save();
                    if (respuesta_error.resultado == "OK")
                        oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                    else // ERROR_DATOS
                        oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuario"].ToString()));
                }
                else
                    generacaso = false;
            }
            return generacaso;

        }


        //private void GenerarResultadoSISA(DetalleProtocolo oDetalle)
        private void GenerarResultadoSISA(DetalleProtocolo oDetalle, string idPruebaSISA, string idTipoPruebaSISA, string idResultadoSISA, string idEventoSISA)

        {
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12;

            int ideventomuestra = oDetalle.IdeventomuestraSISA;
           
            string URL = oCon.URLResultadoSISA;


            try
            {
                int id_resultado_a_informar = int.Parse(idResultadoSISA); // 0;
                int idevento = int.Parse(idEventoSISA); //  307; // sospechoso

              

                if (id_resultado_a_informar != 0)
                {
                    string femision = oDetalle.FechaValida.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    string frecepcion = oDetalle.IdProtocolo.Fecha.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                    resultado newresultado = new resultado
                    { // resultado de dni: 31935346
                        derivada = false,
                        fechaEmisionResultado = femision, //"2020-09-14", //
                        fechaRecepcion = frecepcion, // "2020-09-13" 
                        idDerivacion = null, //1125675,//
                        idEstablecimiento = 107093,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                        idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                        idEventoMuestra = ideventomuestra,  // 2131682, // sale del excel
                        idPrueba = int.Parse(idPruebaSISA), //1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = int.Parse(idTipoPruebaSISA), // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
                        noApta = true,
                        valor = ""
                    };




                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    string DATA = jsonSerializer.Serialize(newresultado);


                    byte[] data = UTF8Encoding.UTF8.GetBytes(DATA);

                    HttpWebRequest request;
                    request = WebRequest.Create(URL) as HttpWebRequest;
                    request.Timeout = 10 * 1000;
                    request.Method = "POST";
                    request.ContentLength = data.Length;
                    request.ContentType = "application/json";
                    request.Headers.Add("app_key", "8482d41353ecd747c271f2ec869345e4");
                    request.Headers.Add("app_id", "0e4fcbbf");



                    Stream postStream = request.GetRequestStream();
                    postStream.Write(data, 0, data.Length);

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();
                    if (body != "")
                    {
                        oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", oDetalle.IdUsuarioValida);
                     }

                }


            }
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                 

            }


        }
         

        private bool estaTildado(string m_idItem)
        {
         
            
          
            string nombre_control = "chk" + m_idItem;
            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            CheckBox chk = (CheckBox)control1;
            if (chk != null)
            {
                if (chk.Checked)
                {
                    if (Session["tildados"] == "")
                        Session["tildados"] = m_idItem;
                    else
                        Session["tildados"] += "," + m_idItem;
                    return true;
                }
                else return false;
            }
            else
                return false;
        }

   

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ingresar")
            {                       
              
               dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
               if (dtProtocolo != null)
               {
                   for (int i = 0; i < dtProtocolo.Rows.Count; i++)
                   {
                       // dtProtocolo.Rows[i].Delete();
                       if (dtProtocolo.Rows[i][0].ToString() == e.CommandArgument.ToString()) CurrentIndexGrilla = i;
                   }
                   CurrentPageIndex = int.Parse(e.CommandArgument.ToString());

                   Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + e.CommandArgument + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
               }
               else
                   Response.Redirect("../FinSesion.aspx", false);                             
            }

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i_idProtocolo= int.Parse(gvLista.DataKeys[e.Row.RowIndex].Value.ToString());
                //Protocolo oProtocolo= new Protocolo();
                //oProtocolo= (Protocolo) oProtocolo.Get(typeof(Protocolo), i_idProtocolo);

                //e.Row.Cells[0].Text = oProtocolo.GetNumero();
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = i_idProtocolo.ToString();
                CmdModificar.CommandName = "Ingresar";                          
            }


        }

        protected void cvValidaControles_ServerValidate(object source, ServerValidateEventArgs args)
        {                
            if ( (ValidaControlesSuperior()) && (ValidaControlesInferior()) )
                args.IsValid=true;
            else
                args.IsValid=false;
        }


        private bool ValidaControlesSuperior()
        {
            //Control a nivel de IdItem (Analisis compuestos de nivel superior: formula leucocitaira, etc)
            bool control = false;
            int idItem_Pivot = 0;

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));//int.Parse

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
           // crit.Add(Expression.Eq("IdEfector", oProtocolo.IdEfector));

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetalleProtocolo oDetalle in detalle)
                {

                  if(  idItem_Pivot != oDetalle.IdItem.IdItem)
                  {                      
                    if (estaVisibleControl(oDetalle.IdItem.IdArea.IdArea.ToString()))
                    {                                                                                  
                        ///Para cada uno de los items del protocolo busca si tiene formula de control asociada.       
                        ICriteria critFormula = m_session.CreateCriteria(typeof(Formula));
                        critFormula.Add(Expression.Eq("IdItem", oDetalle.IdItem));
                        critFormula.Add(Expression.Eq("IdTipoFormula", 2));
                        critFormula.Add(Expression.Eq("Baja", false));

                        IList listaformulas = critFormula.List();
                        if (listaformulas.Count > 0)
                        {
                            //control = false;
                            foreach (Formula oFormula in listaformulas)
                            {
                                if (oFormula != null) //Si el item tiene control se calcula
                                {
                                    cvValidaControles.ErrorMessage = "Error de validación en " + oFormula.IdItem.Nombre;
                                    decimal formula1, formula2 = 0;
                                    string valor1 = ReemplazarValores(oFormula.ContenidoFormula, oDetalle, "control");
                                    if (valor1 == "NA") { control = false; break; }
                                    if (valor1 == "NC") { control = true; break; }
                                    if ((valor1 != "NA") &&(valor1 != "NC"))
                                    {
                                        formula1 = decimal.Parse(valor1, System.Globalization.CultureInfo.InvariantCulture);
                                        string valor2 = ReemplazarValores(oFormula.FormulaControl, oDetalle, "control");
                                        if (valor2 == "NA"){ control = false; break;}
                                        if (valor2 == "NC") { control = true; break; }
                                        if ((valor2 != "NA")&&(valor2 != "NC"))
                                        {
                                            formula2 = decimal.Parse(valor2, System.Globalization.CultureInfo.InvariantCulture);

                                            ///Calculo de veracidad del control
                                            switch (oFormula.Operacion)
                                            {
                                                case 1: //igual
                                                    if (formula1 == formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 2://no es igual
                                                    if (formula1 != formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 3: ///mayor que
                                                    if (formula1 > formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 4: ///mayor o igual que
                                                    if (formula1 >= formula2) control = true;
                                                    else control = false;
                                                    break;

                                                case 5: ///menor que
                                                    if (formula1 < formula2) control = true;
                                                    else control = false;
                                                    break;

                                                case 6: ///menor o igual que
                                                    if (formula1 <= formula2) control = true;
                                                    else control = false;
                                                    break;
                                            } //fin swicth                                     
                                        }/// fin if valor 2
                                    }//fin if valor1                                                                                   
                                }//fin if formula!=null
                                if (!control) break;
                            }// fin del foreach de formula
                        }//fin del si hay una lista de formulas
                        else { control = true;  }

                    } // fin de verificaciíon de contorles visibles por area
                    else
                    { control = true; }
                    //}// fin id controol==true 
                }
                    idItem_Pivot = oDetalle.IdItem.IdItem;
                    if (!control) break;
                   
                }//fin del foreach
            }//fin primer if
            return control;
        }
        private bool ValidaControlesInferior()
        {
            Utility oUtil = new Utility();

            //Control a nivel de SubIdItem (Analisis compuestos de nivel inferior: metamielocitos, etc)
            bool control=true;

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse( Request["idProtocolo"].ToString()));//int.Parse

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));             
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
          //  crit.Add(Expression.Eq("IdEfector", oProtocolo.IdEfector));

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetalleProtocolo oDetalle in detalle)
                { 
                    
                    if (estaVisibleControl(oDetalle.IdSubItem.IdArea.IdArea.ToString()))
                    {                 
                        
                        if (oDetalle.IdSubItem.IdEfector.IdEfector == oDetalle.IdSubItem.IdEfectorDerivacion.IdEfector)
                    {
                        
                        if ((oDetalle.IdSubItem.IdTipoResultado == 1)&& (oDetalle.TrajoMuestra=="Si"))
                        {
                            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(oDetalle.IdSubItem.IdItem.ToString());
                            TextBox txt = txt = (TextBox)control1;
                            if (txt!= null) 
                            {
                                if (txt.Enabled)
                                {
                                    ///Verifica si el item tiene valor minimos y maximos de validación
                                    ///
                                    if ((txt.Text.Trim() != "") &&       (oUtil.EsNumerico(txt.Text.Trim())))
                                        {
                                        if (oDetalle.IdSubItem.Multiplicador > 1)
                                        {
                                            decimal valorActual = Math.Round(oDetalle.ResultadoNum, oDetalle.IdSubItem.FormatoDecimal);

                                            if (txt.Text != valorActual.ToString(System.Globalization.CultureInfo.InvariantCulture))  // si no tiene resultados 
                                            {
                                                if (txt.Text.Length != valorActual.ToString(System.Globalization.CultureInfo.InvariantCulture).Length)  // si no tiene resultados 
                                                {
                                                    decimal resultadoNumerico = decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture) * oDetalle.IdSubItem.Multiplicador;
                                                    txt.Text = resultadoNumerico.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                                }

                                            }
                                        }

                                        //AplicarMultiplicador(txt.ID, oDetalle.IdSubItem.Multiplicador); 

                                        if (oDetalle.AnalizarLimites(lblIdValorFueraLimite1.Text))
                                        {
                                                if (Request["Operacion"].ToString() != "Carga")
                                                {
                                                    if (estaTildado(oDetalle.IdSubItem.IdItem.ToString()))
                                                    {
                                                        if (oDetalle.ResultadoNum != decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture))
                                                        {
                                                            if (!oDetalle.IdSubItem.VerificaValoresMinimosMaximos(txt.Text))
                                                            {
                                                                cvValidaControles.ErrorMessage = "Error de valor fuera de límite en " + oDetalle.IdSubItem.Nombre;
                                                                lblIdValorFueraLimite.Text = oDetalle.IdDetalleProtocolo.ToString();
                                                                btnAceptarValorFueraLimite.Visible = true;
                                                                control = false; break;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                   
                                                
                                                    if ((txt.Text != "") && (oDetalle.ResultadoNum != decimal.Parse(txt.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture)))
                                                    {
                                                        if (!oDetalle.IdSubItem.VerificaValoresMinimosMaximos(txt.Text))
                                                        {
                                                            cvValidaControles.ErrorMessage = "Error de valor fuera de límite en " + oDetalle.IdSubItem.Nombre;
                                                            lblIdValorFueraLimite.Text = oDetalle.IdDetalleProtocolo.ToString();
                                                            btnAceptarValorFueraLimite.Visible = true;
                                                            control = false; break;
                                                        }
                                                    }
                                                }
                                        }
                                    }
                                }
                            }
                            //if (!VerificaValoresMinimosMaximos(oDetalle.IdSubItem, txt.Text)) { cvValidaControles.ErrorMessage = "Error de validación en " + oDetalle.IdSubItem.Nombre; control = false; break; }
                        }
                    }

                                            
                    ///Para cada uno de los items del protocolo busca si tiene formula de control asociada.       
                    ICriteria critFormula = m_session.CreateCriteria(typeof(Formula));
                    critFormula.Add(Expression.Eq("IdItem", oDetalle.IdSubItem));
                    critFormula.Add(Expression.Eq("IdTipoFormula", 2));
                    critFormula.Add(Expression.Eq("Baja", false));

                        IList listaformulas = critFormula.List();
                        if (listaformulas.Count > 0)
                        {
                            foreach (Formula oFormula in listaformulas)
                            {
                                if (oFormula != null) //Si el item tiene control se calcula
                                {
                                    cvValidaControles.ErrorMessage = "Error de validación en " + oFormula.IdItem.Nombre;
                                    decimal formula1, formula2 = 0;
                                    string valor1 = ReemplazarValores(oFormula.ContenidoFormula, oDetalle,"control");
                                    if (valor1 == "NA") { control = false; break; }
                                    if (valor1 == "NC") { control = true; break; }
                                    if ((valor1 != "NA") && (valor1 != "NC"))                              
                                    {
                                        formula1 = decimal.Parse(valor1, System.Globalization.CultureInfo.InvariantCulture);
                                        string valor2 = ReemplazarValores(oFormula.FormulaControl, oDetalle,"control");
                                        if (valor2 == "NA") { control = false; break; }
                                        if (valor2 == "NC") { control = true; break; }
                                        if ((valor2 != "NA") && (valor2 != "NC"))
                                        {
                                            formula2 = decimal.Parse(valor2, System.Globalization.CultureInfo.InvariantCulture);

                                            ///Calculo de veracidad del control
                                            switch (oFormula.Operacion)
                                            {
                                                case 1: //igual
                                                    if (formula1 == formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 2://no es igual
                                                    if (formula1 != formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 3: ///mayor que
                                                    if (formula1 > formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 4: ///mayor o igual que
                                                    if (formula1 >= formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 5: ///menor que
                                                    if (formula1 < formula2) control = true;
                                                    else control = false;
                                                    break;
                                                case 6: ///menor o igual que
                                                    if (formula1 <= formula2) control = true;
                                                    else control = false;
                                                    break;
                                            } //fin swicth                                     
                                        }/// fin if valor 2
                                    }//fin if valor1                                                                                   
                                }//fin if formula!=null
                                if (!control) break;
                            }// fin del foreach de formula
                        }//fin del si hay una lista de formulas
                    }                    // fin de verificaciíon de contorles visibles por area
                    else
                    { control = true; }

                        //}// fin id controol==true
                  if (!control) break;
                }//fin del foreach
            }//fin primer if
            return control;            
        }

        private bool AnalizarLimites(string p)
        {
            throw new NotImplementedException();
        }

        private bool estaVisibleControl(string idarea)
        {
            bool visible=true;
            if (Request["idArea"].ToString() == "0")
                visible = true;
            else
            {
                if (idarea == Request["idArea"].ToString())
                    visible = true;
                else
                    visible = false;
            }
            return visible;

        }

        

        private string ReemplazarValores(string p, DetalleProtocolo oDetalle, string tipoControl)
        {
            string devolver = "NA";
            string formulacalculada = p;
            string aux = p.Replace("+", ";");
            aux = aux.Replace("-", ";");
            aux = aux.Replace("/", ";");
            aux = aux.Replace("*", ";");

            //if (estaTildado(oDetalle.IdSubItem.IdItem.ToString()))
            //{
                string[] arr = aux.Split((";").ToCharArray());


            ////verifricar el estado de todos
                bool calcula = false; 
                foreach (string m in arr)
                {
                    if (m.Substring(0, 1) == "[")
                    {
                        string codigoDet = m.Replace("[", "");
                        codigoDet = codigoDet.Replace("]", "");

                        if (tienevalor(codigoDet, tipoControl))
                        {
                            calcula = true; break;
                        }

                    }
                    else
                    {
                        calcula = true; break;
                    }
                  
                }

            //////////
            if (!calcula) { devolver = "NC";  }/// si estan vacio devuelve NC: no contiene valores

            if (calcula)
            {
                foreach (string m in arr)
                {
                    if (m.Substring(0, 1) == "[")
                    {
                        string codigoDet = m.Replace("[", "");
                        codigoDet = codigoDet.Replace("]", "");
                        decimal valorEncontrado = BuscarResultadoItem(codigoDet, tipoControl);
                        if (valorEncontrado == -11111)
                        {
                            devolver = "NA"; break;
                        }
                        else
                            //if (valorEncontrado == -99999)
                            //{
                            //    devolver = "NA";
                            //    break;
                            //}else
                            formulacalculada = formulacalculada.Replace("[" + codigoDet + "]", valorEncontrado.ToString());
                    }
                    else
                        formulacalculada = m;

                }
                Parser parser = new Parser();
                double resultado = 0;
                if (parser.Evaluate(formulacalculada))
                {
                    resultado = parser.Result;
                    devolver = resultado.ToString();
                }
            }////fin si calcula
            return devolver;            

        }

        private bool tienevalor(string codigoDet, string tipoControl)
        {
            bool vacio = true;
            //decimal valor = 0;
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigoDet, "Baja", false);

            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(oItem.IdItem.ToString());
            TextBox txt;
            if (control1 is TextBox)
            {
                txt = (TextBox)control1;

                if (Request["Operacion"].ToString() != "Carga")
                {
                    if (!estaTildado(oItem.IdItem.ToString()))
                    {                      
                     vacio = false; 
                    }


                }
                else
                {
                    if (txt.Text == "")
                        vacio = false;
                }
            }
            
            return vacio;
        }




        private decimal BuscarResultadoItem(string codigoDet, string tipoControl)
        {

            decimal valor = 0;
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigoDet,"Baja",false);

            if (oItem != null)
            {
                Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(oItem.IdItem.ToString());
                TextBox txt;
                if (control1 is TextBox)
                {
                    txt = (TextBox)control1;
                    //               oItem.FormatoDecimal               

                    if (Request["Operacion"].ToString() == "Carga")
                    {

                        if (txt.Text != "")
                        {
                            decimal resultadoNumerico = decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture) * oItem.Multiplicador;
                            valor = resultadoNumerico;// decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                            valor = -11111;



                    }

                    if (Request["Operacion"].ToString() != "Carga")
                    {
                        if (tipoControl == "control")
                        {
                            if (estaTildado(oItem.IdItem.ToString()))
                            {
                                if (txt.Text != "")
                                {
                                    decimal resultadoNumerico = decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture) * oItem.Multiplicador;
                                    valor = resultadoNumerico; // decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture);
                                }
                            }
                            else
                                valor = -11111;
                        }

                        if (tipoControl == "formula")
                        {
                            //if (estaTildado(oItem.IdItem.ToString()))
                            //{
                            if (txt.Text != "")
                            {
                                decimal resultadoNumerico = decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture) * oItem.Multiplicador;
                                valor = resultadoNumerico; // decimal.Parse(txt.Text, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            //}
                            else
                                valor = -11111;
                        }
                    }


                }
            }
            return valor;
        }


     

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
          
        }

        private void Avanzar(int avance)
        {
            //try
            //{

            if (Session["Tabla1"] != null)
           {
                if (CurrentIndexGrilla <= UltimaPageIndex)
                {
                    if (avance == 1)
                    {
                        if (CurrentIndexGrilla < UltimaPageIndex) CurrentIndexGrilla += 1;  //avanza
                    }
                    else  //retrocede                        
                    {
                        if (avance != 0)                        
                        CurrentIndexGrilla = CurrentIndexGrilla - 1;  //retrocede
                    }
                    if (CurrentIndexGrilla > -1)
                    {
                        dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
                        CurrentPageIndex = int.Parse(dtProtocolo.Rows[CurrentIndexGrilla][0].ToString());

                        string s_url = "ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString();
                        if (Request["Desde"] != null) s_url = s_url + "&Desde=" + Request["Desde"].ToString();
                        Response.Redirect(s_url, false);
                    }
                }
                else
                    //if (Request["Operacion"].ToString() == "HC")
                    //    Response.Redirect("../Informes/HistoriaClinicaFiltro.aspx?Tipo=Paciente", false);
                    //else
                        Response.Redirect("ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);

            }
            //else Response.Redirect("../FinSesion.aspx", false);                             


             
        }

        protected void lnkPosterior_Click(object sender, EventArgs e)
        {
            Avanzar(1);
        }

        protected void lnkAnterior_Click(object sender, EventArgs e)
        {
            Avanzar(-1);
        }


        protected void lnkAuditoria_Click(object sender, EventArgs e)
        {
           
        }

    

       

        protected void btnValidarImprimir_Click(object sender, EventArgs e)
        {
             if (Page.IsValid)
                {

                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                    Guardar(oProtocolo,true, true);
                    Imprimir(oProtocolo, "I");
                    string sredirect = "";

                    if ((Request["desde"] != null) && (Request["desde"] == "Urgencia"))
                    {
                        sredirect = "../Urgencia/UrgenciaList.aspx";
                        Response.Redirect(sredirect);
                    }
                    else
                        if (Request["urgencia"] != null)
                        {
                            sredirect = "ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=Valida&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&urgencia=1&validado=" + Request["validado"].ToString() + "&modo=Normal";
                            Response.Redirect(sredirect);
                        }
                        else


                            //if (sredirect=="")
                            Avanzar(1);    
                 //Avanzar(1);

                }
          
        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            Imprimir(oProtocolo, "PDF");
        }

      
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {

         //   Marcar(true);
         //   SetSelectedTab(TabIndex.DEFAULT);

        }

        
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
        //    Marcar(false);
          //  SetSelectedTab(TabIndex.DEFAULT);
        }

        

        protected void btnGuardarObsCodificadaGral_Click(object sender, EventArgs e)
        {
            //if (Session["IdUsuario"] != null)
            if (oUser.IdUsuario>0)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                oProtocolo.ObservacionResultado = txtObservacion.Text;
                oProtocolo.Save();

                oProtocolo.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString()), "OBSERVACION RESULTADO", txtObservacion.Text);
                lblObservacionResultado.Text = txtObservacion.Text;
                SetSelectedTab(TabIndex.ONE);
            }
            else Response.Redirect("../FinSesion.aspx", false);

            //string popupScript = "<script language='JavaScript'> alert('Las observaciones se guardaron correctamente')</script>";
            //Page.RegisterClientScriptBlock("PopupScript", popupScript);

            
            //CurrentPageIndex = oProtocolo.IdProtocolo;
            //if (Request["urgencia"] != null)
            //    Response.Redirect("../resultados/ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&Parametros=" + oProtocolo.IdProtocolo.ToString() + "&idArea=" + Request["idArea"].ToString() + "&urgencia=1&modo=Normal&validado=" + Request["validado"].ToString());

            //else
            //    Response.Redirect("../resultados/ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&Parametros=" + oProtocolo.IdProtocolo.ToString() + "&idArea=" + Request["idArea"].ToString() + "&modo=Normal&validado=" + Request["validado"].ToString());                            



        }
        
        protected void btnBorrarObsCodificadaGral_Click(object sender, EventArgs e)
        {
           txtObservacion.Text = "";            
           txtObservacion.UpdateAfterCallBack = true;
        }


        
        protected void btnAgregarObsCodificadaGral_Click(object sender, EventArgs e)
        {
            if (ddlObsCodificadaGeneral.Text != "")
            {
                ObservacionResultado oRegistro = new ObservacionResultado();
                oRegistro = (ObservacionResultado)oRegistro.Get(typeof(ObservacionResultado), int.Parse(ddlObsCodificadaGeneral.SelectedValue));

                txtObservacion.Text = txtObservacion.Text + " " + oRegistro.Nombre;
                txtObservacion.UpdateAfterCallBack = true;
            }
        }
        
        
        

        protected void btnAceptarValorFueraLimite_Click(object sender, EventArgs e)
        {
           lblIdValorFueraLimite1.Text += ","+ lblIdValorFueraLimite.Text;
          
        }


        protected void btnAplicarFormula_Click(object sender, EventArgs e)
        {
            //if ((chkFormula.Checked) && (chkFormula.Visible))
            //{

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();

            AplicarFormulas(oProtocolo);
             chkFormula.Checked = false;
            //}
        }


        public void MostrarResAnterior(Protocolo oProtocolo)
        {
            bool haydatos = false;
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            //crit.Add(Expression.Eq("IdEfector", oProtocolo.IdEfector));

            IList lista = crit.List();

            //   string valor = "NA";

            if (lista.Count > 0)
            {
                foreach (DetalleProtocolo oDet in lista)
                {
                   


                    string s_iditemcito = "ResAnterior"+oDet.IdSubItem.IdItem.ToString();
                        Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(s_iditemcito.ToString());
                    Label olblResultadoAnterior = (Label)control1;

                        if (olblResultadoAnterior != null) 
                        {

                              string  resultadoAnterior = oDet.BuscarResultadoAnterior(oDet.IdSubItem, oDet.IdItem, true);
                        if (resultadoAnterior != "")
                        {
                            olblResultadoAnterior.Text = resultadoAnterior;

                            olblResultadoAnterior.Visible = true;
                            haydatos = true;
                        }
                    }
                    }///foreach
                 
            }
            if (!haydatos)
            {
                lblSinResultados.Visible = true;
                lblSinResultados.Text = "No se encontraron datos anteriores para las determinaciones de este protocolo";
            }
        }


        public void AplicarFormulas(Protocolo oProtocolo)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            //crit.Add(Expression.Eq("IdEfector", oProtocolo.IdEfector));

            IList lista = crit.List();

         //   string valor = "NA";
           
            if (lista.Count > 0)
            {
                foreach (DetalleProtocolo oDet in lista)
                {
                    if (oDet.IdSubItem.IdTipoResultado==1)  // solo para controles numericos
                    {
                    int s_iditemcito=oDet.IdSubItem.IdItem;
                    Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(s_iditemcito.ToString());
                    TextBox txtFormula = (TextBox)control1;

                    if ((txtFormula != null) && (estatildadaformula(s_iditemcito)))
                    {
                        
                        ICriteria critFormula = m_session.CreateCriteria(typeof(Formula));
                        critFormula.Add(Expression.Eq("IdItem", oDet.IdSubItem));
                        critFormula.Add(Expression.Eq("IdTipoFormula", 1));
                        critFormula.Add(Expression.Eq("Baja", false));
                        IList lista2 = critFormula.List();
                        foreach (Formula oFormula in lista2)
                        {
                            bool sicalcula = false;
                            //////Aplica filtro para el sexo y edad del paciente///////////////////                     
                            if ((oDet.IdProtocolo.Sexo == oFormula.Sexo) || (oFormula.Sexo == "I")) // coincide el sexo del paciente o el sexo del valor de referencia es Indistinto
                            {
                                    bool cumpleCondicion = true;
                                    if (oFormula.CondicionDeterminacion != "")
                                        cumpleCondicion =  VerificarCondicionFormula(oFormula);
                                    if (cumpleCondicion)
                                    {
                                        int idraza = oFormula.IdRaza;
                                        int edadDesde = oFormula.EdadDesde;
                                        int edadHasta = oFormula.EdadHasta;
                                        int unidadEdad = oFormula.UnidadEdad;
                                        if (unidadEdad == -1)
                                            sicalcula = true;
                                        else
                                        {
                                            if ((unidadEdad == oDet.IdProtocolo.UnidadEdad) && (edadDesde <= oDet.IdProtocolo.Edad) && (edadHasta >= oDet.IdProtocolo.Edad))
                                                sicalcula = true;
                                        }

                                        if (idraza == 1)  // si es afro
                                        {
                                            if (idraza == oDet.IdProtocolo.IdPaciente.IdRaza)
                                                sicalcula = true;
                                            else
                                                sicalcula = false;
                                        }
                                        if (idraza == 0)
                                        {    if ( oDet.IdProtocolo.IdPaciente.IdRaza!=1)
                                                sicalcula = true;
                                            else
                                                sicalcula = false;
                                              }
                                    }
                                }
                            ////////////////////////////////////////////////////////////////////////////////
                            if (sicalcula)
                            {
                            string formulacalculada = oFormula.ContenidoFormula.ToUpper().TrimEnd();
                            string aux = oFormula.ContenidoFormula.Replace("+", ";").ToUpper().TrimEnd();
                            aux = aux.Replace("-", ";");
                            aux = aux.Replace("/", ";");
                            aux = aux.Replace("\\", ";");
                            aux = aux.Replace("*", ";");
                            aux = aux.Replace("(", ";");
                            aux = aux.Replace(")", ";");
                            aux = aux.Replace("!", ";");
                            aux = aux.Replace("^", ";");
                            aux = aux.ToLower().Replace("mod", ";");
                            aux = aux.ToLower().Replace("min", ";");
                            aux = aux.ToLower().Replace("max", ";");
                            aux = aux.ToLower().Replace("sin", ";");
                            aux = aux.ToLower().Replace("cos", ";");
                            aux = aux.ToLower().Replace("tan", ";");
                            aux = aux.ToLower().Replace("atan", ";");
                            aux = aux.ToLower().Replace("abs", ";");
                            aux = aux.ToLower().Replace("log", ";");
                            aux = aux.ToLower().Replace("ceil", ";");
                            aux = aux.ToLower().Replace("int", ";");
                            aux = aux.ToLower().Replace("frac", ";");
                            aux = aux.ToLower().Replace("sqr", ";");
                            aux = aux.ToUpper().Replace("EDAD", "EDAD");
                            string[] arr = aux.ToUpper().Split((";").ToCharArray());
                            bool sinvalor = false;
                            foreach (string m in arr)
                            {
                                if (m.Length > 0)
                                {
                                    if (m.Substring(0, 1) == "[")
                                    {
                                        string codigoDet = m.Replace("[", "");
                                        codigoDet = codigoDet.Replace("]", "");

                                        decimal valorEncontrado = 0;
                                        if (codigoDet.ToUpper().TrimEnd() == "EDAD")
                                            valorEncontrado =decimal.Parse( oDet.IdProtocolo.Edad.ToString());
                                        else

                                            valorEncontrado = BuscarResultadoItem(codigoDet, "formula");
                                        //if (valorEncontrado == -99999) break;
                                        //else
                                        if (valorEncontrado == -11111)
                                        {
                                            sinvalor = true;
                                            break;
                                        }
                                        formulacalculada = formulacalculada.Replace("[" + codigoDet.ToUpper().TrimEnd() + "]", valorEncontrado.ToString());
                                        formulacalculada = formulacalculada.Replace(".",","); ///reemplazo puntos por comas en los numeros decimales; para aplicar la funcion Evaluate.                                                                                                                       
                                    }
                                }
                            }

                            
                            if (!sinvalor)
                            {                                
                                Parser p = new Parser();                                
                                double resultado = 0;
                                if (p.Evaluate(formulacalculada))
                                {                                    
                                    resultado = p.Result;
                                    //Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(oDet.IdSubItem.IdItem.ToString());
                                    //TextBox txtFormula = (TextBox)control1;
                                    //txtFormula.Text= resultado.ToString();
                                    try
                                    {                                        
                                        decimal x = 0;
                                        x = Math.Round(decimal.Parse(resultado.ToString()), oDet.IdSubItem.FormatoDecimal);
                                        // x = decimal.Parse(resultado.ToString());

                                        txtFormula.Text = x.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    }
                                    catch { txtFormula.Text = ""; }


                                }
                            }
                            else
                                txtFormula.Text = "";

                            //valor = resultado.ToString();
                        }
                            } /// si es control de tipo numerico
                      }
                    }///foreach
                }
            }
            //return valor;


            //string valorFormula= GetValorFormula(oDetalle.IdSubItem);
            //if (valorFormula!="NA")
            //oDetalle.ResultadoNum = decimal.Parse( valorFormula);

        }

       
                public bool VerificarCondicionFormula(Formula oFormula)
        {
            string formulacalculada = oFormula.CondicionDeterminacion;
            string aux = oFormula.CondicionDeterminacion.Replace("+", ";");
            aux = aux.Replace("-", ";");
            aux = aux.Replace("/", ";");
            aux = aux.Replace("\\", ";");
            aux = aux.Replace("*", ";");
            aux = aux.Replace("(", ";");
            aux = aux.Replace(")", ";");
            aux = aux.Replace("!", ";");
            aux = aux.Replace("^", ";");
            aux = aux.ToLower().Replace("mod", ";");
            aux = aux.ToLower().Replace("min", ";");
            aux = aux.ToLower().Replace("max", ";");
            aux = aux.ToLower().Replace("sin", ";");
            aux = aux.ToLower().Replace("cos", ";");
            aux = aux.ToLower().Replace("tan", ";");
            aux = aux.ToLower().Replace("atan", ";");
            aux = aux.ToLower().Replace("abs", ";");
            aux = aux.ToLower().Replace("log", ";");
            aux = aux.ToLower().Replace("ceil", ";");
            aux = aux.ToLower().Replace("int", ";");
            aux = aux.ToLower().Replace("frac", ";");
            aux = aux.ToLower().Replace("sqr", ";");
            aux = aux.ToLower().Replace(">=", ";");
            aux = aux.ToLower().Replace(">", ";");
            aux = aux.ToLower().Replace("<=", ";");
            aux = aux.ToLower().Replace("<", ";");
            aux = aux.ToLower().Replace("=", ";");

            string[] arr = aux.Split((";").ToCharArray());
            foreach (string m in arr)
            {
                if (m.Length > 0)
                {
                    if (m.Substring(0, 1) == "[")
                    {
                        string codigoDet = m.Replace("[", "");
                        codigoDet = codigoDet.Replace("]", "");
                        decimal valorEncontrado = BuscarResultadoItem(codigoDet,   "formula");
                        if (valorEncontrado == -11111) break;
                        else
                        {
                            formulacalculada = formulacalculada.Replace("[" + codigoDet.ToUpper() + "]", valorEncontrado.ToString());
                            formulacalculada = formulacalculada.Replace(".", ","); ///reemplazo puntos por comas en los numeros decimales; para aplicar la funcion Evaluate.                                                                                                                       
                        }
                    }
                }
                //else
                //    formulacalculada = m;

            }

            bool ok = false;
            try
            {
                string[] arr1 = formulacalculada.Split((",").ToCharArray());
                if (formulacalculada.Contains(">="))
                {
                    arr1 = formulacalculada.Split((">=").ToCharArray());
                    if (double.Parse(arr1[0].ToString()) >= double.Parse(arr1[2].ToString())) ok = true;
                }
                if ((formulacalculada.Contains(">")) && (!ok))
                {
                    arr1 = formulacalculada.Split((">").ToCharArray());
                    if (double.Parse(arr1[0].ToString()) > double.Parse(arr1[1].ToString())) ok = true;
                }
                if ((formulacalculada.Contains("<=")) && (!ok))
                {
                    arr1 = formulacalculada.Split(("<=").ToCharArray());
                    if (double.Parse(arr1[0].ToString()) <= double.Parse(arr1[2].ToString())) ok = true;
                }
                if ((formulacalculada.Contains("<")) && (!ok))
                {
                    arr1 = formulacalculada.Split(("<").ToCharArray());
                    if (double.Parse(arr1[0].ToString()) < double.Parse(arr1[1].ToString())) ok = true;
                }
                if ((formulacalculada.Contains("=")) && (!ok))
                {
                    arr1 = formulacalculada.Split(("=").ToCharArray());
                    if (double.Parse(arr1[0].ToString()) == double.Parse(arr1[1].ToString())) ok = true;
                }
            }
            catch (Exception ex)
            {
                string h = ex.Message.ToString();
                ok = false; }
            return ok;
        }
    

        private bool estatildadaformula(int s_iditemcito)
        {
            bool ok = false;
            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl("F"+s_iditemcito.ToString());
            CheckBox ochkformula = (CheckBox)control1;

            if (ochkformula != null)
            {
                if (ochkformula.Enabled) /// solo si está habilitado 
                    ok = ochkformula.Checked;
            }

            return ok;
        }

        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {              
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            Imprimir(oProtocolo, "I");       
        }


        protected void btnGuardarAislamiento_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                 if (Session["idUsuario"] != null)
                {
                    for (int i = 0; i <gvAislamientos.Rows.Count; i++)
                    {                   
                        ProtocoloGermen oRegistro = new ProtocoloGermen();
                        oRegistro = (ProtocoloGermen)oRegistro.Get(typeof(ProtocoloGermen), int.Parse(gvAislamientos.DataKeys[i].Value.ToString()));

                        CheckBox chkAtb = (CheckBox)gvAislamientos.Rows[i].FindControl("chkAtb");
                        if (chkAtb != null) oRegistro.Atb = chkAtb.Checked;

                        TextBox txtObservaciones= (TextBox)gvAislamientos.Rows[i].FindControl("txtObservacionesAislamiento");
                        if (txtObservaciones != null)
                        {
                            oRegistro.Observaciones = txtObservaciones.Text;
                        }
                        oRegistro.IdUsuarioRegistro = int.Parse(oUser.IdUsuario.ToString());
                        oRegistro.FechaRegistro = DateTime.Now;
                        if (Request["Operacion"].ToString() != "Valida") oRegistro.FechaValida = DateTime.Parse("01/01/1900");
                        if (Request["Operacion"].ToString() == "Valida")
                        {
                            CheckBox chkValida = (CheckBox)gvAislamientos.Rows[i].FindControl("chkValida");
                            if (chkValida != null)
                            {
                                if (chkValida.Checked)
                                {
                                    oRegistro.FechaValida = DateTime.Now;
                                    oRegistro.IdUsuarioValida = int.Parse(oUser.IdUsuario.ToString());
                                  oRegistro.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Valida", int.Parse(oUser.IdUsuario.ToString()), "Aislamiento", oRegistro.IdGermen.Nombre);
                                }
                            }

                        }
                        oRegistro.Save();
                        if ((oRegistro.IdProtocolo.Estado == 0) && (oRegistro.IdProtocolo.Estado !=2))
                        {
                            oRegistro.IdProtocolo.Estado = 1;
                            oRegistro.IdProtocolo.Save();
                        }


                     

                    }
                    CargarGrillaAislamientos();
                    CargarListasAntibiogramas();
                }
                 else Response.Redirect("../FinSesion.aspx", false);
               
            }
            SetSelectedTab(TabIndex.QUINTO);
        }


        protected void btnAgregarGermen_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (oUser.IdUsuario>0)// != null)
                {
                    GuardarAislamiento();
                    CargarGrillaAislamientos();
                    SetSelectedTab(TabIndex.QUINTO);
                }
                else Response.Redirect("../FinSesion.aspx", false);
               
            }

        }

        private void GuardarAislamiento()
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);
            //////////////
            Germen oGermen = new Germen();
            oGermen = (Germen)oGermen.Get(typeof(Germen), int.Parse(ddlAislamiento.SelectedValue));

            ////revisa cuantos aislamientos hay para numerar el siguiente
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloGermen));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));            
            IList lista = crit.List();
            int numeroAislamiento= lista.Count+1;                
            /////////////////////////////////

            ProtocoloGermen oRegistro = new ProtocoloGermen();
            oRegistro.IdProtocolo = oProtocolo;
            oRegistro.NumeroAislamiento = numeroAislamiento;
            oRegistro.IdItem = int.Parse(ddlPracticaAislamiento.SelectedValue);
            oRegistro.IdGermen = oGermen;
            oRegistro.IdUsuarioRegistro = int.Parse(oUser.IdUsuario.ToString());
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.FechaValida = DateTime.Parse("01/01/1900");
            
            oRegistro.Save();

                oProtocolo.GrabarAuditoriaDetalleProtocolo("Alta", int.Parse(oUser.IdUsuario.ToString()), "Aislamiento", oRegistro.IdGermen.Nombre);
            //}
            //else
            //{
            //    string popupScript = "<script language='JavaScript'> alert('El aislamiento ya fue agregado')</script>";
            //    Page.RegisterClientScriptBlock("PopupScript", popupScript);

            //}

            


            
        }


        private void CargarGrillaAislamientos()
        {
            ////Metodo que carga la grilla de Protocolos

            string m_strSQL = @" SELECT PG.numeroAislamiento, I.nombre as item, G.nombre AS germen, PG.atb, PG.observaciones, PG.idProtocoloGermen
                        FROM         LAB_ProtocoloGermen AS PG 
                        INNER JOIN                      LAB_Germen AS G ON PG.idGermen = G.idGermen 
INNER JOIN LAB_Item AS I ON I.idItem= PG.idItem
WHERE   PG.baja=0 and  PG.idProtocolo = " + CurrentPageIndex;

            if (Request["Operacion"].ToString() == "HC")
            {
                m_strSQL = @" SELECT PG.idProtocoloGermen, I.nombre as [Deter.], PG.numeroAislamiento as [Nro. Cepa], G.nombre AS [Aislamiento], PG.atb as [ATB], PG.observaciones as [Observaciones], '' as Estado
                        FROM         LAB_ProtocoloGermen AS PG 
                        INNER JOIN                      LAB_Germen AS G ON PG.idGermen = G.idGermen 
INNER JOIN LAB_Item AS I ON I.idItem= PG.idItem
WHERE   PG.baja=0 and  PG.idProtocolo = " + CurrentPageIndex;
            }

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
           

        

            if (Request["Operacion"].ToString() == "HC")
            {
                gvAislamientosHC.DataSource = Ds.Tables[0];
                gvAislamientosHC.DataBind();
            }
            else
            {
                gvAislamientos.DataSource = Ds.Tables[0];
                gvAislamientos.DataBind();
              
            }
            if (Ds.Tables[0].Rows.Count > 0)
                aisl.Visible = true;
        }


        protected void btnAgregarAntibiograma_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GuardarAntibiograma(false);
                
                ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
                CargarAntibioticoPractica(ddlPracticaAtb.SelectedValue);
                CargarListaAntibiotico();
                SetSelectedTab(TabIndex.CUARTO);
                //Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&Parametros=" + Request["Parametros"].ToString() + "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
            //    ActualizarVistaAntibiograma();
            }
            
        }

        private void ActualizarVistaAntibiograma(string i)
        {
            CargarListaAntibiogramas();
            

            ///////////////////////////////////////////////////////////////////////////////////////////
            //int cantidadAntibiogramas=ddlAntibiograma.Items.Count - 1;
            //lblCantidadAntibiograma.Text = " *" + cantidadAntibiogramas.ToString();

            //DataSet Ds1 = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //adapter.SelectCommand = new SqlCommand(m_ssql, conn);
            //adapter.Fill(Ds1);           
            //gvAntibiogramaValida.DataSource = Ds1.Tables[0];
            //gvAntibiogramaValida.DataBind();
            /////////////////////////////////////////////////////////////////////////////////////////


            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.CommandText = "Lab_GetAntibiograma";


            //Protocolo oProtocolo = new Protocolo();
            //oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);


            cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idProtocolo"].Value = CurrentPageIndex.ToString();// oProtocolo.IdProtocolo.ToString();

            cmd.Parameters.Add("@idItem", SqlDbType.NVarChar);
            cmd.Parameters["@idItem"].Value = i;


         
        
            cmd.Connection = conn;


            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            
            //gvAntiBiograma2.DataSource = Ds.Tables[0];
            //gvAntiBiograma2.DataBind();
            //gvAntiBiograma2.Visible = true;


            if (Request["Operacion"].ToString() == "HC")
            {
                pnlAntibiograma.Visible = false;
                gvAntiBiogramaHC.Visible = true;
                gvAntiBiogramaHC.DataSource = Ds.Tables[0];
                gvAntiBiogramaHC.DataBind();

                if ((gvAntiBiogramaHC.Rows.Count == 1) && (gvAntiBiogramaHC.Columns.Count == 0))
                {
                    tituloAntibiograma.Visible = false;
                    pnlAntibiogramaHC.Visible = false;
                }
                else
                {
                    tituloAntibiograma.Visible = true;
                    pnlAntibiogramaHC.Visible = true;
                    anti.Visible = true; 
                }
            }
            else
            {
                //if (oProtocolo.IdTipoServicio.IdTipoServicio == 3)
                //{
                    pnlAntibiograma.Visible = true;
              //      pnlAntibiogramaHC.Visible = false;
                    gvAntiBiograma2.DataSource = Ds.Tables[0];
                    gvAntiBiograma2.DataBind();

                 
                    gvAntiBiograma2.Visible = true;
                //    gvAntiBiogramaHC.Visible = false;

                    if ((Ds.Tables[0].Rows.Count == 1) && (Ds.Tables[0].Columns.Count == 1)) gvAntiBiograma2.Visible = false;
                    else { anti.Visible = true; }
                   // CargarListasAntibiogramas();
              //  }
            }


           

           
        }

        private void CargarListaAntibiogramas()
        {
            Utility oUtil = new Utility();
            string m_ssql = " SELECT distinct CONVERT(varchar, ANTI.numeroAislamiento) + '-' + convert(varchar,G.idGermen) AS idGermen,CONVERT(varchar, ANTI.numeroAislamiento) + ' - ' + G.nombre AS nombre " +
                            " FROM LAB_Antibiograma AS ANTI " +
                            " INNER JOIN LAB_Germen AS G ON ANTI.idGermen = G.idGermen " +
                            " where ANTI.idProtocolo=" + CurrentPageIndex + " and ANTI.IdItem=" + ddlPracticaAtb.SelectedValue + " and ANTI.idMetodologia= " + ddlMetodoAntibiograma.SelectedValue;

            oUtil.CargarCombo(ddlAntibiograma, m_ssql, "idGermen", "nombre");
            MostrarAtb();
            //ddlAntibiograma.Items.Insert(0, new ListItem("--SELECCIONE ANTIBIOGRAMA--", "0"));

        }


        protected void gvAislamientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                EliminarMicroorganismos(e.CommandArgument.ToString());
                CargarGrillaAislamientos();

                SetSelectedTab(TabIndex.QUINTO);

            }
        }

        private void EliminarMicroorganismos(string p)
        {
            ProtocoloGermen oRegistro = new ProtocoloGermen();
            oRegistro = (ProtocoloGermen)oRegistro.Get(typeof(ProtocoloGermen), int.Parse(p));



            if (!tieneAntibiograma(oRegistro.IdProtocolo, oRegistro.IdGermen, oRegistro.NumeroAislamiento))
            {
             

                oRegistro.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(oUser.IdUsuario.ToString()), "Aislamiento", oRegistro.IdGermen.Nombre + " Nro. cepa:" + oRegistro.NumeroAislamiento.ToString());
                oRegistro.Delete();
                //}
            }
            else
            {
                string popupScript = "<script language='JavaScript'> alert('No es posible eliminar el aislamiento por que tiene un antibiograma asociado.'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }

        }

        private bool tieneAntibiograma(Protocolo p, Germen p_2, int p_3)
        {         
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Antibiograma));
            crit.Add(Expression.Eq("IdProtocolo", p));
            crit.Add(Expression.Eq("IdGermen", p_2));
            crit.Add(Expression.Eq("NumeroAislamiento", p_3));
            IList lista = crit.List();
            if (lista.Count > 0)
                return true;
            else
                return false;
        }


        protected void gvAislamientos_RowDataBound1(object sender, GridViewRowEventArgs e)
        {

            if (Request["Operacion"] == "Carga")
            {
                e.Row.Cells[5].Visible = false;
                
                }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {                
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[7].Controls[1];
                CmdEliminar.CommandArgument = gvAislamientos.DataKeys[e.Row.RowIndex].Value.ToString();

                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

              

                ProtocoloGermen oGermen = new ProtocoloGermen();
                oGermen = (ProtocoloGermen)oGermen.Get(typeof(ProtocoloGermen), int.Parse(gvAislamientos.DataKeys[e.Row.RowIndex].Value.ToString()));

                Usuario oUser = new Usuario();
                // if (Request["Operacion"].ToString() == "Valida")
                if (oGermen.IdUsuarioValida > 0)
                {
                    oUser = (Usuario)oUser.Get(typeof(Usuario), oGermen.IdUsuarioValida);

                    e.Row.Cells[6].Text.ToString();
                    e.Row.Cells[6].ForeColor = Color.Blue;

                    e.Row.Cells[6].Text = "Validado por " + oUser.FirmaValidacion + " " + oGermen.FechaValida.ToShortDateString() +" "+ oGermen.FechaValida.ToShortTimeString();
                }
                else
                {
                    if (oGermen.IdUsuarioRegistro > 0)
                    {
                        oUser = (Usuario)oUser.Get(typeof(Usuario), oGermen.IdUsuarioRegistro);

                        e.Row.Cells[6].Text.ToString();
                        e.Row.Cells[6].ForeColor = Color.Black;
                        e.Row.Cells[6].Text ="Cargado por " + oUser.Apellido + " " + oUser.Nombre + " " + oGermen.FechaRegistro.ToShortDateString() + " " + oGermen.FechaRegistro.ToShortTimeString();
                    }
                    else
                        if (oGermen.IdUsuarioRegistro == 0)
                    {
                      

                        e.Row.Cells[6].Text.ToString();
                        e.Row.Cells[6].ForeColor = Color.Red;
                        e.Row.Cells[6].Text = "AUTOMATICO " + oGermen.FechaRegistro.ToShortDateString() + " " + oGermen.FechaRegistro.ToShortTimeString();
                    }
                }

                e.Row.Cells[6].Font.Size = FontUnit.Point(7);
            }
        }

    
       

        private void GuardarAntibiograma(bool valida)
        {            

            //diferenciar guardar  de validar
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);
            
            ProtocoloGermen oGermen = new ProtocoloGermen();
            oGermen = (ProtocoloGermen)oGermen.Get(typeof(ProtocoloGermen), int.Parse(ddlGermen.SelectedValue));

            if (noexistetb(oProtocolo, oGermen))
            {

            

                for (int i = 0; i < gvAntiobiograma.Rows.Count; i++)
                {
                    DropDownList ddl = (DropDownList)gvAntiobiograma.Rows[i].FindControl("ddlAntibiotico");
                    TextBox txth = (TextBox)gvAntiobiograma.Rows[i].FindControl("txtHalo");


                    if ((ddl.SelectedValue != "") || (txth.Text != ""))
                    {
                        Antibiograma oRegistro = new Antibiograma();
                        oRegistro.NumeroAislamiento = oGermen.NumeroAislamiento;
                        oRegistro.IdProtocolo = oProtocolo;
                        oRegistro.IdEfector = oProtocolo.IdEfector;
                        oRegistro.IdGermen = oGermen.IdGermen;
                        oRegistro.IdItem = int.Parse(ddlPracticaAtb.SelectedValue);

                        Antibiotico oAntibiotico = new Antibiotico();
                        oAntibiotico = (Antibiotico)oAntibiotico.Get(typeof(Antibiotico), int.Parse(gvAntiobiograma.DataKeys[i].Value.ToString()));
                        oRegistro.IdAntibiotico = oAntibiotico;


                        oRegistro.IdMetodologia = int.Parse(rdbMetodologiaAntibiograma.SelectedValue);
                        oRegistro.Resultado = ddl.SelectedValue;

                        //TextBox txth = (TextBox)gvAntiobiograma.Rows[i].FindControl("txtHalo");
                        //if (txth != null)
                        //{ 
                        if (txth.Text != "") oRegistro.Valor = txth.Text;
                        //}

                        oRegistro.IdUsuarioRegistro = oUser.IdUsuario;
                        oRegistro.FechaRegistro = DateTime.Now;
                        oRegistro.FechaValida = DateTime.Parse("01/01/1900");
                        //    if (Request["Operacion"].ToString() == "Valida")
                        if (valida)
                        {
                            oRegistro.IdUsuarioValida = oUser.IdUsuario;
                            oRegistro.FechaValida = DateTime.Now;
                        }
                        oRegistro.Save();

                        oProtocolo.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(oUser.IdUsuario.ToString()), "ATB " + oRegistro.NumeroAislamiento.ToString() + " " + oRegistro.IdGermen.Nombre + " (" + rdbMetodologiaAntibiograma.SelectedItem.Text + ") - " + oRegistro.IdAntibiotico.Descripcion, oRegistro.Resultado);


                    }
                }
            }
            else
            {
                string popupScript = "<script language='JavaScript'> alert('No se pudo agregar; modifique el ATB ya creado'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
            }
            
          
               
        }

        private bool noexistetb(Protocolo oProtocolo, ProtocoloGermen oGermen)
        {
            ////validacion si existe la combinacion protocolo+germen+ microorg+ metolodoliga+ antibiotico
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Antibiograma));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            crit.Add(Expression.Eq("IdGermen", oGermen.IdGermen));
            crit.Add(Expression.Eq("IdItem", int.Parse(ddlPracticaAtb.SelectedValue)));
            crit.Add(Expression.Eq("IdMetodologia", int.Parse(rdbMetodologiaAntibiograma.SelectedValue)));
            crit.Add(Expression.Eq("NumeroAislamiento", oGermen.NumeroAislamiento));

            IList lista = crit.List();
            if (lista.Count == 0)
                return true;
            else
                return false;
        }

        protected void gvAntiBiograma2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int idGermen= int.Parse( gvAntiBiograma2.DataKeys[gvAntiBiograma2.SelectedIndex].Value);
            //ModificarAntibiograma(idGermen);
        }

        private void ModificarAntibiograma(int idGermen)
        {
            throw new NotImplementedException();
        }

        protected void btnEliminarAntibiograma_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            if (ddlAntibiograma.SelectedValue!="0")
            {
                EliminarAntibiograma();
                
                ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);

                SetSelectedTab(TabIndex.CUARTO);
            }
          
        }
        
        protected void btnActualizarPracticas_Click(object sender, EventArgs e)
        {

           // this.EnableViewState = false;
           // Avanzar(0);
           //SetSelectedTab(TabIndex.ONE);
           // this.EnableViewState = true;
            //ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);


           
        }
        protected void btnActualizarATB_Click(object sender, EventArgs e)
        {
            ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);

            SetSelectedTab(TabIndex.CUARTO);

        }

        protected void btnEditarAntibiograma_Click(object sender, EventArgs e)
        {
          

            ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);

            SetSelectedTab(TabIndex.CUARTO);

        }
        protected void btnValidarAntibiograma_Click(object sender, EventArgs e)
        {
         
            ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);

            SetSelectedTab(TabIndex.CUARTO);

        }
        private void EliminarAntibiograma()
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);

            string[] ais = ddlAntibiograma.SelectedValue.Split(("-").ToCharArray());
            string s_idgermen = ais[1].ToString();
            string s_numeroaislamiento = ais[0].ToString();
            
            Germen oGermen = new Germen();
            oGermen = (Germen)oGermen.Get(typeof(Germen), int.Parse(s_idgermen));


            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Antibiograma));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            crit.Add(Expression.Eq("IdGermen", oGermen));
            crit.Add(Expression.Eq("IdItem",int.Parse(ddlPracticaAtb.SelectedValue)));
            crit.Add(Expression.Eq("IdMetodologia", int.Parse(ddlMetodoAntibiograma.SelectedValue)));
            crit.Add(Expression.Eq("NumeroAislamiento", int.Parse(s_numeroaislamiento)));
            
            IList lista = crit.List();
            if (lista.Count > 0)
            {
                foreach (Antibiograma oRegistro in lista)
                {
                    //if ((Request["Operacion"] == "Carga") && (oRegistro.IdUsuarioValida == 0))
                    //{
                        oRegistro.Delete();
                        oRegistro.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(oUser.IdUsuario.ToString()), "ATB: " + oRegistro.IdGermen.Nombre + "(" + ddlMetodoAntibiograma.SelectedItem.Value + ") - " + oRegistro.IdAntibiotico.Descripcion, oRegistro.Resultado);
                    //}
                }
            }
        }

        protected void gvAntiBiograma2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string hola=e.Row.Cells[i].Text.ToString();
                    if (hola.IndexOf("SII") != -1)
                    {// contienen un SIII validado
                        e.Row.Cells[i].ForeColor = Color.Blue;
                        e.Row.Cells[i].Text = hola.Replace("SII", "");
                    }

                    if (hola.IndexOf("NOO") != -1)
                    {// contienen un SIII validado
                        e.Row.Cells[i].ForeColor = Color.Black;
                        e.Row.Cells[i].Text = hola.Replace("NOO", "");
                    }

                    if (hola.IndexOf("EQUIPO") != -1)
                    {// contienen un SIII validado
                        e.Row.Cells[i].ForeColor = Color.Red;
                        e.Row.Cells[i].Text = hola.Replace("EQUIPO", "");
                    }

                }
                  
            }

        }

        protected void btnActualizarAntibiograma_Click(object sender, EventArgs e)
        {
            ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
            SetSelectedTab(TabIndex.CUARTO);
        }

        protected void gvAntiBiogramaHC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string hola = e.Row.Cells[i].Text.ToString();
                    if (hola.IndexOf("SII") != -1)
                    {// contienen un SIII validado
                        e.Row.Cells[i].ForeColor = Color.Blue;
                        e.Row.Cells[i].Text = hola.Replace("SII", "");
                    }

                    if (hola.IndexOf("NOO") != -1)
                    {// contienen un SIII validado
                        e.Row.Cells[i].ForeColor = Color.Black;
                        e.Row.Cells[i].Text = hola.Replace("NOO", "");
                    }

                    if (hola.IndexOf("EQUIPO") != -1)
                    {// contienen un SIII validado
                        e.Row.Cells[i].ForeColor = Color.Red;
                        e.Row.Cells[i].Text = hola.Replace("EQUIPO", "");
                    }

                }

            }
        }

        protected void btnAgregarAntibiogramaValidado_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GuardarAntibiograma(true);

                ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
                CargarAntibioticoPractica(ddlPracticaAtb.SelectedValue);
                CargarListaAntibiotico();
                SetSelectedTab(TabIndex.CUARTO);
                //Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&Parametros=" + Request["Parametros"].ToString() + "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
                //    ActualizarVistaAntibiograma();
            }
        }

        protected void lnkMarcarAislamiento_Click(object sender, EventArgs e)
        {
            MarcarAislamientosSeleccionados(true);
            
        }
        private void MarcarAislamientosSeleccionados(bool p)
        {
            
            foreach (GridViewRow row in gvAislamientos.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[5].FindControl("chkValida")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[5].FindControl("chkValida"))).Checked = p;
            }
            SetSelectedTab(TabIndex.QUINTO);
        }

        protected void lnkDesMarcarAislamiento_Click(object sender, EventArgs e)
        {
            MarcarAislamientosSeleccionados(false);
        }

        protected void btnValidarTodosAntibiogramas_Click(object sender, EventArgs e)
        {
            ValidarTodosATB(true);
            ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
            SetSelectedTab(TabIndex.CUARTO);
        }

        private void ValidarTodosATB(bool completo)
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);

            oProtocolo.ValidarTodoslosAtb(int.Parse(ddlPracticaAtb.SelectedValue), int.Parse(oUser.IdUsuario.ToString()), completo);
           
        }

        protected void btnValidarTodosAntibiogramasPendientes_Click(object sender, EventArgs e)
        {
            ValidarTodosATB(false); ActualizarVistaAntibiograma(ddlPracticaAtb.SelectedValue);
            SetSelectedTab(TabIndex.CUARTO);
        }

        protected void gvAislamientosHC_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            
                ProtocoloGermen oGermen = new ProtocoloGermen();
                oGermen = (ProtocoloGermen)oGermen.Get(typeof(ProtocoloGermen), int.Parse(gvAislamientosHC.DataKeys[e.Row.RowIndex].Value.ToString()));

                Usuario oUser = new Usuario();
                // if (Request["Operacion"].ToString() == "Valida")
                if (oGermen.IdUsuarioValida > 0)
                {
                    oUser = (Usuario)oUser.Get(typeof(Usuario), oGermen.IdUsuarioValida);

                    e.Row.Cells[6].Text.ToString();
                    e.Row.Cells[6].ForeColor = Color.Blue;

                    e.Row.Cells[6].Text = "Validado por " + oUser.FirmaValidacion + " " + oGermen.FechaValida.ToShortDateString() + " " + oGermen.FechaValida.ToShortTimeString();
                }
                else
                {
                    if (oGermen.IdUsuarioRegistro > 0)
                    {
                        oUser = (Usuario)oUser.Get(typeof(Usuario), oGermen.IdUsuarioRegistro);

                        e.Row.Cells[6].Text.ToString();
                        e.Row.Cells[6].ForeColor = Color.Black;
                        e.Row.Cells[6].Text = "Cargado por " + oUser.Apellido + " " + oUser.Nombre + " " + oGermen.FechaRegistro.ToShortDateString() + " " + oGermen.FechaRegistro.ToShortTimeString();
                    }
                    else
                        if (oGermen.IdUsuarioRegistro == 0)
                    {


                        e.Row.Cells[6].Text.ToString();
                        e.Row.Cells[6].ForeColor = Color.Red;
                        e.Row.Cells[6].Text = "AUTOMATICO " + oGermen.FechaRegistro.ToShortDateString() + " " + oGermen.FechaRegistro.ToShortTimeString();
                    }
                }

                e.Row.Cells[6].Font.Size = FontUnit.Point(7);
            }
        }
        protected void btnArchivos_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Protocolos/ProtocoloAdjuntar.aspx?idProtocolo=" + Request["idProtocolo"].ToString() + "&desde=protocolo");
        }
        protected void imgPdf_Click(object sender, EventArgs e)
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            Imprimir(oProtocolo, "PDF");

        }

        protected void btnRestringirAcceso_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { 
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();                      
                oProtocolo.Estado = 3; ///Accceso Restringido;
                oProtocolo.Save();
                oProtocolo.GrabarAuditoriaProtocolo("Acceso Restringido", int.Parse(oUser.IdUsuario.ToString()));
                Avanzar(0);
            }

        }

        protected void btnDeshacerRestringirAcceso_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
                oProtocolo.Estado = 1; ///vuelve a en proceso y se debe validar nuevamente!!!
                oProtocolo.Save();
                oProtocolo.GrabarAuditoriaProtocolo("Deshace Acceso Restringido", int.Parse(oUser.IdUsuario.ToString()));

                /// borra los permisos existentes
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.ProtocoloPermiso));
                crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
                IList lista = crit.List();                
                    foreach (Business.Data.Laboratorio.ProtocoloPermiso oDiag in lista)
                    {
                        oDiag.Delete();
                    }               
                ////

                Avanzar(0);
            }

        }

   

        protected void btnMostrarResultados_Click(object sender, EventArgs e)
        {

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            if (oProtocolo!= null)
            MostrarResAnterior(oProtocolo);
            //chkFormula.Checked = false;

            
        }
    }
}
