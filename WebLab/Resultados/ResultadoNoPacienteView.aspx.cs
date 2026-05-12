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

namespace WebLab.Resultados
{
    public partial class ResultadoNoPacienteView : System.Web.UI.Page
    {
      
        CrystalReportSource oCr = new CrystalReportSource();
      
        Configuracion oCon = new Configuracion(); 
       
        public Usuario oUser = new Usuario();

      

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;


            if (Request["master"]!=null)
                Page.MasterPageFile = "~/Site2.master";

            if (Request["idProtocolo"] != null)
            {
                if (Session["idUsuario"] != null)         
                {
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
               
                    LlenarTabla(Request["idProtocolo"].ToString()); 
                    CargarListas();
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);                   
            }       
        }


    
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {            
                if (Session["idUsuario"] != null)                
                    Inicializar();                                      
                else
                    Response.Redirect("../FinSesion.aspx", false);                             
            }
        }


        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private void Inicializar()
        {           
            
           
            pnlAntecedentes.Visible = false;                    
            pnlHC.Visible = true;            
            pnlReferencia.Visible = true;
            MuestraDatos(Request["idProtocolo"].ToString());
            if (Request["desde"] == "Urgencia")
                hypRegresar.NavigateUrl = "../Urgencia/UrgenciaList.aspx";
            else
            {
                if (Request["validado"] == "0") ///protocolo completo
                {
                    hypRegresar.NavigateUrl = "../Informes/HistoriaClinicaFiltro.aspx?Tipo=PacienteCompleto";
                    VerificaPermisos("Historial de Visitas");
                }
                else  ///solo validado para acceso externos
                {
                    imgImprimir.Visible = false;
                    pnlImpresora.Visible = false;
                    hypRegresar.NavigateUrl = "../Informes/HistoriaClinicaFiltro.aspx?Tipo=PacienteValidado";
                    VerificaPermisos("Historial de Resultados");
                }
            }


            
        }

        private void CargarListas()
        {
           Utility oUtil = new Utility();
            
           ///////////////Impresoras////////////////////////

           string m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
            oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
            if (Session["Impresora"] != null) ddlImpresora.SelectedValue = Session["Impresora"].ToString();
            
           ///////////////Fin de Impresoras///////////////////
                       
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


 

        private void MuestraDatos(string p)
        {
            ///Muestra los datos de encabezado para el protocolo seleccionado
            //CargarListasObservaciones("gral");

            //Actualiza los datos de los objetos : alta o modificacion .                                        
            Protocolo oRegistro = new Protocolo();
            oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(p));
            HFIdProtocolo.Value = p;
            oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));
            if (oRegistro.tieneAdjuntoProtocoloVisible())
            { imgAdjunto.Visible = true; spanadjunto.Visible = true; }
            else
            { imgAdjunto.Visible = false; spanadjunto.Visible = false; }

            if (oRegistro.IdMuestra > 0)
                {
                    Muestra oMuestra = new Muestra();
                    oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), oRegistro.IdMuestra);

                    lblMuestra.Text = oMuestra.Nombre;
                }

            lblConservacion.Text = "";
            if (oRegistro.IdConservacion> 0)
            {
                Conservacion oConservacion = new Conservacion();
                oConservacion = (Conservacion)oConservacion.Get(typeof(Conservacion), oRegistro.IdConservacion);
                lblConservacion.Text = oConservacion.Descripcion;
            }
           
            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl("tContenido");
            Table tablaContenido = (Table)control1;
            int cantidadFilas =0;
            if (tablaContenido != null)
            {
                cantidadFilas = tablaContenido.Rows.Count;
                if ((cantidadFilas==1) || (oRegistro.Estado == 0))
                {                 
                    imgImprimir.Visible = false;
                    pnlImpresora.Visible = false;
                    imgPdf.Visible = false;
                    /// protocolo sin procesar no muestra panel con resultados
                    Panel1.Visible = false;
                    pnlReferencia.Visible = false;

                    lblEstadoProtocolo.Visible = true;
                    lblEstadoProtocolo.Text = " PROTOCOLO EN PROCESO";
                }
                    
            }
            lblDescripcion.Text = oRegistro.DescripcionProducto;
            
            lblUsuario.Text = oRegistro.IdUsuarioRegistro.Username;
            lblFechaRegistro.Text = oRegistro.FechaRegistro.ToShortDateString();
            int len = oRegistro.FechaRegistro.ToString().Length - 11;
            lblHoraRegistro.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);
            lblFecha.Text = oRegistro.Fecha.ToShortDateString();
            lblProtocolo.Text =   oRegistro.GetNumero().ToString();  
          
          

            lblSector.Text = oRegistro.IdSector.Nombre;
            if (oRegistro.Sala != "") lblSector.Text += " Sala: " + oRegistro.Sala;
            if (oRegistro.Cama != "") lblSector.Text += " Cama: " + oRegistro.Cama;

            
            lblNumeroOrigen.Text = oRegistro.NumeroOrigen;        
          

            lblPedidoOriginal.Text = oRegistro.GetPracticasPedidas();

        }

       


        private void LlenarTabla(string p)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;           
            DataSet Ds = new DataSet();            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "LAB_ResultadoView";

            cmd.Parameters.Add("@idProtocolo", SqlDbType.NVarChar);
            cmd.Parameters["@idProtocolo"].Value = p;
            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);

           
                string s = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;


                string m_titulo = "";
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
                    lblUM.Text = "U.M";
                    objCellUnMedida_TITULO.Controls.Add(lblUM);
                }

                Label lblVR = new Label();
                lblVR.Text = "VR-METODO";
                objCellValoresReferencia_TITULO.Controls.Add(lblVR);




                Label lblValida = new Label();
                if ((Request["Operacion"].ToString() == "Carga") || (Request["Operacion"].ToString() == "HC")) lblValida.Text = "";
                else
                {
                    if (Request["Operacion"].ToString() == "Valida") lblValida.Text = "VAL";
                    if (Request["Operacion"].ToString() == "Control") lblValida.Text = "CTRL";
                }
                objCellValida_TITULO.Controls.Add(lblValida);

                Label lblCargadoPor = new Label();
                if ((Request["Operacion"].ToString() == "HC") && (Request["validado"].ToString() == "1"))                
                    lblCargadoPor.Text = "VALIDADO POR";                    
                else
                    lblCargadoPor.Text = "ESTADO";

                objCellPersona_TITULO.Controls.Add(lblCargadoPor);                             
                objFila_TITULO.Cells.Add(objCellAnalisis_TITULO);
                objFila_TITULO.Cells.Add(objCellResultado_TITULO);                            
                objFila_TITULO.Cells.Add(objCellValoresReferencia_TITULO);                
                objFila_TITULO.Cells.Add(objCellPersona_TITULO);
                objFila_TITULO.Cells.Add(objCellResultadoAnterior_TITULO);

                objFila_TITULO.CssClass = "myLabelIzquierda";
                objFila_TITULO.BackColor = Color.Gainsboro;

                Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                //'añadimos la fila a la tabla
                if (objFila_TITULO != null) tContenido.Controls.Add(objFila_TITULO);//.Rows.Add(objRow);    

                string pivot_Area = "";

                int tablas = Ds.Tables.Count;
                if (tablas > 0)
                { 
                ////Caro PF: Cache de resultado item
               
                ISession session = NHibernateHttpModule.CurrentSession;             
                ///CARO PF: traer detalleprotocolos y derivaciones antes del for para no ir a la base N veces
                var idsDetallesList = Ds.Tables[0].AsEnumerable()
                .Select(r => r.Field<int>("idDetalleProtocolo"))
                .ToList();

                var detallesList = session.CreateCriteria(typeof(DetalleProtocolo))
                .Add(Expression.In("IdDetalleProtocolo", idsDetallesList.ToArray()))
                .List()
                .Cast<DetalleProtocolo>()
                .ToList();

                var detallesDict = detallesList
                    .ToDictionary(d => d.IdDetalleProtocolo);

                // Traer todas las Derivaciones juntas
                var derivacionesList = session.CreateCriteria(typeof(Derivacion))
                    .CreateAlias("IdDetalleProtocolo", "dp")
                    .Add(Expression.In("dp.IdDetalleProtocolo", idsDetallesList.ToArray()))
                    .List()
                    .Cast<Derivacion>()
                    .ToList();

                var derivacionesDict = derivacionesList
                    .ToDictionary(d => d.IdDetalleProtocolo.IdDetalleProtocolo);
                /// CARO PF : fin 
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    //decimal m_minimoReferencia=-1;
                    //decimal m_maximoReferencia=-1;
                    string valorReferencia = Ds.Tables[0].Rows[i].ItemArray[11].ToString();
                    int m_idItem = int.Parse(Ds.Tables[0].Rows[i].ItemArray[2].ToString());
                    string unMedida = Ds.Tables[0].Rows[i].ItemArray[8].ToString();
                    string Observaciones = Ds.Tables[0].Rows[i].ItemArray[5].ToString();
                    int tiporesultado = (int.Parse(Ds.Tables[0].Rows[i].ItemArray[7].ToString()));
                    int tipodeterminacion = int.Parse(Ds.Tables[0].Rows[i].ItemArray[6].ToString());
                    int estado = int.Parse(Ds.Tables[0].Rows[i].ItemArray[9].ToString());
                    string m_metodo = Ds.Tables[0].Rows[i].ItemArray[10].ToString();

                    string m_observacionReferencia = Ds.Tables[0].Rows[i].ItemArray[13].ToString();
                    string m_usuarioCarga = Ds.Tables[0].Rows[i].ItemArray[14].ToString();
                    string m_trajoMuestra = Ds.Tables[0].Rows[i].ItemArray[15].ToString();
                    string m_tipoValorReferencia = Ds.Tables[0].Rows[i].ItemArray[16].ToString();
                    string m_conResultado = Ds.Tables[0].Rows[i].ItemArray[17].ToString();
                    string m_formatoDecimal = Ds.Tables[0].Rows[i].ItemArray[18].ToString();
                    string m_formato0 = Ds.Tables[0].Rows[i].ItemArray[19].ToString();
                    string m_formato1 = Ds.Tables[0].Rows[i].ItemArray[20].ToString();
                    string m_formato2 = Ds.Tables[0].Rows[i].ItemArray[21].ToString();
                    string m_formato3 = Ds.Tables[0].Rows[i].ItemArray[22].ToString();
                    string m_formato4 = Ds.Tables[0].Rows[i].ItemArray[23].ToString();
                    string m_resultadoDefecto = Ds.Tables[0].Rows[i].ItemArray[24].ToString();
                    string m_usuariocontrol = Ds.Tables[0].Rows[i].ItemArray[25].ToString();
                    string m_usuariovalida = Ds.Tables[0].Rows[i].ItemArray[28].ToString();
                    int i_iddetalleProtocolo = int.Parse(Ds.Tables[0].Rows[i].ItemArray[26].ToString());
                    string m_codificaPaciente = Ds.Tables[0].Rows[i].ItemArray[27].ToString();

                    string m_estadoObservacion = Ds.Tables[0].Rows[i].ItemArray[29].ToString();
                    string m_area = Ds.Tables[0].Rows[i].ItemArray[30].ToString();

                 

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


                    decimal x = 0;



                    if (m_area != pivot_Area) ///poner titulo del area
                    {
                        TableRow objRow = new TableRow();
                        TableCell objCell = new TableCell();
                        Label lbl0 = new Label();                        
                        lbl0.Text = m_area.ToUpper();
                        lbl0.TabIndex = short.Parse("500");
                        lbl0.Font.Bold = true;


                        Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(lbl0);
                        objCell.Controls.Add(lbl0);                        
                        objCell.ColumnSpan = 8;

                        objRow.BackColor = Color.Beige;
                        objRow.HorizontalAlign = HorizontalAlign.Center;
                        objRow.Cells.Add(objCell);
                        
                        tContenido.Controls.Add(objRow);

                        pivot_Area = m_area;
                    }

                    if ((m_hijo != m_titulo) && (m_nombre != m_titulo)) ///poner titulo de la practica
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
                            objCell.ColumnSpan = 6;
                        else
                            objCell.ColumnSpan = 8;

                        objRow.Cells.Add(objCell);
                        objRow.CssClass = "myLabelIzquierda";
                        tContenido.Controls.Add(objRow);

                        m_nombre = m_titulo;
                        if (m_trajoMuestra == "No")
                        {
                            Label lblSinMuestra = new Label();
                            lblSinMuestra.TabIndex = short.Parse("500");
                            lblSinMuestra.Text = "Sin Muestra";// +oItem.IdEfectorDerivacion.Nombre; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();
                            lblSinMuestra.Font.Italic = true;
                            lblSinMuestra.ForeColor = Color.Blue;                        
                            objCellResultado.Controls.Add(lblSinMuestra);
                        }
                    }
                    Label lbl1 = new Label();
                    if (m_hijo == m_titulo) lbl1.Text = m_hijo;
                    else lbl1.Text = "&nbsp;&nbsp;&nbsp;" + m_hijo;



                    lbl1.TabIndex = short.Parse("500");
                    lbl1.ForeColor = Color.Black;
                    lbl1.Font.Size = FontUnit.Point(9);
                    if (tipodeterminacion != 0)
                    {
                        lbl1.Font.Bold = true;
                        lbl1.Font.Italic = true;
                        objCellAnalisis.ColumnSpan = 1;
                    }

                    objCellAnalisis.Controls.Add(lbl1);


  					DetalleProtocolo oDetalle;
                    detallesDict.TryGetValue(i_iddetalleProtocolo, out oDetalle);

                    Derivacion oDeriva;
                    derivacionesDict.TryGetValue(i_iddetalleProtocolo, out oDeriva);
                    //fin 
                    Item oItem = new Item();
                    oItem = oDetalle.IdSubItem; // (Item)oItem.Get(typeof(Item), m_idItem);

                     
                    ///Antes de mostrar el control verifica  si está derivado                    
                    if (oDeriva != null)  /// esta pendiente     
                    {
                        Label lblDerivacion = new Label();
                        lblDerivacion.Font.Italic = true;
                        lblDerivacion.TabIndex = short.Parse("500");                        
                        string estadoDerivacion = "";                   
                        estadoDerivacion = oDetalle.ResultadoCar;
                        if (oDeriva.Resultado != "")
                            estadoDerivacion += " - Resultado Informado: " + oDeriva.Resultado;                        
                        lblDerivacion.Text = estadoDerivacion;                        
                        objCellResultado.ColumnSpan = 1;
                        objCellResultado.Controls.Add(lblDerivacion);
                    }
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
                            ImageButton btnImagen = new ImageButton();
                            if (tipodeterminacion == 0) // si es una determinacion simple
                            {
                                switch (tiporesultado)
                                {//tipoResultado
                                    case 1: //numerico
                                        {
                                            Label lblValorCritico = new Label();
                                            switch (m_formatoDecimal)
                                            {
                                                case "0": x = decimal.Parse(m_formato0);break;
                                                case "1": x = decimal.Parse(m_formato1);break;
                                                case "2": x = decimal.Parse(m_formato2);break;
                                                case "3": x = decimal.Parse(m_formato3);break;
                                                case "4": x = decimal.Parse(m_formato4);break;
                                            }

                                            Label olbl = new Label();
                                            olbl.Font.Bold = true;
                                            olbl.Font.Size = FontUnit.Point(9);
                                            if (m_conResultado == "False")
                                                olbl.Text = "";
                                            else
                                                olbl.Text = x.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + unMedida;

                                            if (oDetalle != null) 
                                            {
                                                                                        
                                                    m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                                                    if (Observaciones != "")
                                                    {
                                                        if (m_conResultado == "False")
                                                        {
                                                            olbl.Text = Observaciones;
                                                            if (m_usuariovalida == "")
                                                            {
                                                                Usuario oUser = new Usuario();
                                                                oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValidaObservacion);
                                                                if (oUser.FirmaValidacion == "") m_usuariovalida = oUser.Apellido + " " + oUser.Nombre;
                                                                else m_usuariovalida = oUser.FirmaValidacion;

                                                                m_usuariovalida +=" " + oDetalle.FechaValida.ToShortDateString();
                                                            }
                                                        }
                                                        else
                                                            olbl.Text = olbl.Text + Observaciones;
                                                    }


                                                    if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio!=5)
                                                    { 
                                                        string resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, false);
                                                        if (resultadoAnterior != "")
                                                        {
                                                          //  hayAntecedente = true;
                                                            Label olblResultadoAnterior = new Label();
                                                            olblResultadoAnterior.TabIndex = short.Parse("500");
                                                            olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                            olblResultadoAnterior.CssClass = "myLittleLink";
                                                            olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + ",790,440); return false");
                                                            olblResultadoAnterior.ToolTip = "Haga clic aqui para ver gráfico de evolución";                                                        
                                                            olblResultadoAnterior.Width = Unit.Pixel(20);
                                                            olblResultadoAnterior.Text = resultadoAnterior;

                                                     
                                                            objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                   
                                                        }

                                                    }
                                                    
                                                    if (oDetalle.VerificaValorReferencia(x))
                                                        olbl.ForeColor = Color.Black;
                                                    else
                                                        olbl.ForeColor = Color.Red;

                                                    if (oDetalle.VerificaValoresLimites(x))
                                                    {
                                                        // verifica si tiene valores fuera de limite: es un valor critico
                                                     
                                                        lblValorCritico.TabIndex = short.Parse("500");
                                                        lblValorCritico.Text = " VALOR CRITICO";
                                                        lblValorCritico.Font.Bold = true;
                                                        lblValorCritico.ForeColor = Color.OrangeRed;                                                        
                                                       
                                                    }                                                    
                                                    if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                    {

                                                        btnImagen.TabIndex = short.Parse("500");
                                                        btnImagen.ID = "IMG" + oDetalle.IdDetalleProtocolo.ToString();
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";
                                                        btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");
                                                        btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                    }
                                                    else
                                                        btnImagen.Visible = false;                                                   

                                            }
                                            objCellResultado.Controls.Add(olbl);
                                            objCellResultado.Controls.Add(btnImagen);
                                            ///etiqueta de unidad de medida
                                            Label olblUM = new Label();
                                            olblUM.Font.Size = FontUnit.Point(7);
                                            olblUM.Text = unMedida;                                                                                                                 
                                                
                                            objCellResultado.Controls.Add(olblUM);
                                            olblUM.Visible = false;
                                            objCellResultado.Controls.Add(lblValorCritico);
                                            //////////////////
                                        } // fin case 1
                                        break;
                                    default: //texto
                                        {

                                            Label olbl = new Label();
                                            olbl.Font.Bold = true;
                                            olbl.Font.Size = FontUnit.Point(9);
                                            if (m_conResultado == "0")
                                                olbl.Text = "";
                                            else
                                                olbl.Text = Ds.Tables[0].Rows[i].ItemArray[4].ToString();

                                            if (Observaciones != "")
                                            {
                                                if (olbl.Text == "")
                                                    olbl.Text += Observaciones;
                                                else
                                                    olbl.Text += Environment.NewLine + " " + Observaciones;

                                            }

                                            objCellResultado.Controls.Add(olbl);
                                            if (oDetalle != null)
                                            {
                                              
                                                    m_usuariovalida += " " + oDetalle.FechaValida.ToShortDateString();
                                                    if (oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio != 5)
                                                    {
                                                        string resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem, false);
                                                        if (resultadoAnterior != "")
                                                        {
                                                     //       hayAntecedente = true;
                                                            Label olblResultadoAnterior = new Label();
                                                            olblResultadoAnterior.TabIndex = short.Parse("500");
                                                            olblResultadoAnterior.Font.Size = FontUnit.Point(8);                                                            
                                                            olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + ",790,300); return false");
                                                            olblResultadoAnterior.ToolTip = "Haga clic aqui para ver más datos.";                                                            
                                                            olblResultadoAnterior.Width = Unit.Pixel(20);
                                                            olblResultadoAnterior.Text = resultadoAnterior;
                                                            objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                            
                                                        }
                                                    }

                                                    if (oDetalle.tieneAdjuntoVisible())//tiene observaciones
                                                    {
                                                        //   ImageButton btnImagen = new ImageButton();
                                                        btnImagen.TabIndex = short.Parse("500");
                                                        btnImagen.ID = "IMG" + oDetalle.IdDetalleProtocolo.ToString();
                                                        btnImagen.ImageUrl = "~/App_Themes/default/images/obs_validado.png";

                                                        btnImagen.ToolTip = "Adjunto imprimible para " + lbl1.Text.Replace("&nbsp;", "");


                                                        btnImagen.Attributes.Add("onClick", "javascript: AdjuntoEdit (" + oDetalle.IdDetalleProtocolo.ToString() + "," + oDetalle.IdProtocolo.IdTipoServicio.IdTipoServicio.ToString() + ",'" + Request["Operacion"].ToString() + "'); return false");
                                                        objCellResultado.Controls.Add(btnImagen);
                                                    }
                                                }
                                             

                                        } // fin case 1
                                        break;
                                }//fin swicth


                                Label lblPersona = new Label();                                
                                lblPersona.Text = m_usuariovalida; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();      
                                lblPersona.Font.Size = FontUnit.Point(7);
                                lblPersona.Font.Italic = true;
                                lblPersona.Text = m_usuariovalida;
                                objCellPersona.Controls.Add(lblPersona);                                
                            }


                            Label lblValoresReferencia = new Label();                            
                            lblValoresReferencia.Font.Italic = true;
                            lblValoresReferencia.Font.Size = FontUnit.Point(8);
                            if (valorReferencia != "")
                            {// muestra el valor guardado 
                                lblValoresReferencia.Text = valorReferencia;
                                if (m_metodo != "")                                    
                                    lblValoresReferencia.Text += Environment.NewLine + m_metodo;
                            }                            

                            objCellValoresReferencia.Controls.Add(lblValoresReferencia);
                        }
                    }


                    ///Definir los anchos de las columnas
                    objCellAnalisis.Width = Unit.Percentage(30);
                    objCellResultado.Width = Unit.Percentage(30);
                    objCellValoresReferencia.Width = Unit.Percentage(20);                    
                    objCellPersona.Width = Unit.Percentage(20);
                    ///////////////////////
                    ///agrega a la fila cada una de las celdas
                    objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);                                        

                    objFila.Cells.Add(objCellValoresReferencia);                    

                    objFila.Cells.Add(objCellPersona);

                    objFila.Cells.Add(objCellResultadoAnterior);
                    

                    //////
                    Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                    //'añadimos la fila a la tabla
                    if (objFila != null)
                        tContenido.Controls.Add(objFila);//.Rows.Add(objRow);                                
                }
                }
            //}
        }



        private void Imprimir(Protocolo oProtocolo, string tipo)
        {
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oProtocolo.IdEfector);
            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {

                  CrystalReportSource oCr = new CrystalReportSource();
            oCr.Report.FileName = "";
            oCr.CacheDuration =10000;
            oCr.EnableCaching = true;

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
            if (oCon.RutaLogo != "")
                conLogo.Value = true;
            else
                conLogo.Value = false;



                if (oProtocolo.IdTipoServicio.IdTipoServicio == 5) // no pacientes
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
                        if (oProtocolo.IdTipoServicio.IdTipoServicio == 3)
                            oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                        else
                            oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaNoPacientes.rpt";
                    }
                    else
                    {
                        if (oProtocolo.IdTipoServicio.IdTipoServicio == 3)
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

            //if (Request["idArea"].ToString() != "0") m_filtro += " and idArea=" + Request["idArea"].ToString();

            
             

            DataTable d = oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio,oCon);
            oCr.ReportDocument.SetDataSource(d);
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
         

            string s_nombreProtocolo = "";
            switch (oCon.TipoNumeracionProtocolo)
            {
                case 0: s_nombreProtocolo = oProtocolo.Numero.ToString(); break;
                case 1: s_nombreProtocolo = oProtocolo.NumeroDiario.ToString(); break;
                case 2: s_nombreProtocolo = oProtocolo.PrefijoSector + oProtocolo.NumeroSector.ToString(); break;
                case 3: s_nombreProtocolo = oProtocolo.NumeroTipoServicio.ToString(); break;
            }

            if (tipo != "PDF")
            {
                try
                {
                    oProtocolo.GrabarAuditoriaProtocolo("Imprime Resultados", int.Parse(Session["idUsuario"].ToString()));
                    Session["Impresora"] = ddlImpresora.SelectedValue;

                    oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
                    oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);

                    oProtocolo.Impreso = true;
                    oProtocolo.Save();
                }
                catch (Exception ex)
                {
                    string exception = "";
                    //while (ex != null)
                    //{
                    //    exception = ex.Message + "<br>";

                    //}
                    string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Impresora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                    Page.RegisterStartupScript("PopupScript", popupScript);
                }
            }
            else
                {

                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF(s_nombreProtocolo);
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                    
                    oProtocolo.GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(Session["idUsuario"].ToString()));
            }
            conn.Close();
            }

        }


    
        protected void cvValidaControles_ServerValidate(object source, ServerValidateEventArgs args)
        {                
            //if ( (ValidaControlesSuperior()) && (ValidaControlesInferior()) )
            //    args.IsValid=true;
            //else
            //    args.IsValid=false;
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

     
     

       

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                Imprimir(oProtocolo, "PDF");
            }
            else
                Response.Redirect("../FinSesion.aspx", false);      
          
        }

       
        protected void btnVerAntecendente_Click(object sender, EventArgs e)
        {
            //if (ddlItem.SelectedValue != "0")
            //{
            //    Protocolo oProtocolo = new Protocolo();
            //    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            //    CargarGrillaAntecedentes(oProtocolo);
            //    SetSelectedTab(TabIndex.THREE);
            //}
        }

 

    
       

        protected void imgImprimir_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                Imprimir(oProtocolo, "I");       
            }
            else
                Response.Redirect("../FinSesion.aspx", false);      
            
        }

        protected void btnPeticion_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PeticionElectronica/PeticionEdit.aspx?idPaciente=" + HFIdPaciente.Value + "&master=1", false);
        }


       
        protected void imgPdf_Click1(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                Imprimir(oProtocolo, "PDF");
            }
            else
                Response.Redirect("../FinSesion.aspx", false);

        }
    }
}
