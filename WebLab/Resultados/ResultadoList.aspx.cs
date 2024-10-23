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
    public partial class ResultadoList : System.Web.UI.Page
    {                 
        
      
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


  
        private void Inicializar()
        {           
            CargarGrilla();
           
          
                lblTitulo.Text = "HISTORIAL DE RESULTADOS";
         

            
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

        
            string str_condicion = " P.Baja=0 AND P.idPrioridad = 2";
           
            


                                
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.CommandText = "[LAB_ListaProtocolo]";

            cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
            cmd.Parameters["@FiltroBusqueda"].Value = str_condicion;

            cmd.Parameters.Add("@TipoLista", SqlDbType.Int);
            cmd.Parameters["@TipoLista"].Value = 4;  /// tipo de reporte de urgencias

            cmd.Parameters.Add("@idArea", SqlDbType.Int);
            cmd.Parameters["@idArea"].Value = 0;

            cmd.Parameters.Add("@idItem", SqlDbType.Int);
            cmd.Parameters["@idItem"].Value = 0;
            
            cmd.Connection = conn;
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            ////////////////////////////////////////////////////////////////////////////////
            
            //////Metodo que carga la grilla de Protocolos

            //string m_strSQL = " Select distinct P.idProtocolo, " +
            //                  " dbo.NumeroProtocolo(P.idProtocolo) as numero," +
            //                  " convert(varchar(10),P.fecha,103) as fecha,P.estado , P.fecha as fecha1," +
            //                  " prefijosector, numerosector , numerodiario" +
            //                  " from Lab_Protocolo P " + // +str_condicion;
            //                  //" INNER JOIN Lab_Configuracion Con on Con.idEfector= P.idEfector " +
            //                  " WHERE P.idProtocolo in (" + Session["Parametros"].ToString() + ")";


            //if (Request["Operacion"].ToString() == "HC")
            //    m_strSQL += " order by idProtocolo desc "; // desde el mas reciente al mas antiguo.
            //else
            //{
            //    Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            //    if (oC.TipoNumeracionProtocolo == 0)
            //        m_strSQL += " order by  idProtocolo ";
            //    if (oC.TipoNumeracionProtocolo == 1)
            //        m_strSQL += " order by  numerodiario ";
            //    if (oC.TipoNumeracionProtocolo == 2)
            //        m_strSQL += " order by prefijosector, numerosector ";
            //}
    

            //DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //SqlDataAdapter adapter = new SqlDataAdapter();
            //adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            //adapter.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
           

            if (Ds.Tables[0].Rows.Count > 0)
            {
                dtProtocolo = Ds.Tables[0];
                int ultimafila = Ds.Tables[0].Rows.Count - 1;
                CurrentPageIndex = int.Parse(Request["idProtocolo"].ToString());
                CurrentIndexGrilla = int.Parse(Request["Index"].ToString());
                //CurrentPageIndex = int.Parse(Ds.Tables[0].Rows[CurrentIndexGrilla].ItemArray[0].ToString());
                UltimaPageIndex = ultimafila; // int.Parse(Ds.Tables[0].Rows[ultimafila].ItemArray[0].ToString());
            }

            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados";
            Session.Add("Tabla1", dtProtocolo);
           
            
        }


    

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
            ///Muestra los datos de encabezado para el protocolo seleccionado
            //CargarListasObservaciones("gral");

            //Actualiza los datos de los objetos : alta o modificacion .                                        
            Protocolo oRegistro = new Protocolo();
            oRegistro = (Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(p));
            oRegistro.GrabarAuditoriaProtocolo("Consulta", int.Parse(Session["idUsuario"].ToString()));

         
            if (oRegistro.IdTipoServicio.IdTipoServicio == 3) //Microbiologia
            {                           
                if (oRegistro.IdMuestra > 0)
                {
                    Muestra oMuestra = new Muestra();
                    oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), oRegistro.IdMuestra);

                    lblMuestra.Text = "Tipo de Muestra: " + oMuestra.Nombre;
                }                             
            }

            lblServicio.Text = oRegistro.IdTipoServicio.Nombre.ToUpper();

            //Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);
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

            
            lblUsuario.Text = oRegistro.IdUsuarioRegistro.Username;
            lblFechaRegistro.Text = oRegistro.FechaRegistro.ToShortDateString();
            int len = oRegistro.FechaRegistro.ToString().Length - 11;
            lblHoraRegistro.Text = oRegistro.FechaRegistro.ToString().Substring(11, oRegistro.FechaRegistro.ToString().Length - 11);
            lblFecha.Text = oRegistro.Fecha.ToShortDateString();
            lblProtocolo.Text =   oRegistro.GetNumero().ToString();  
          
            //hplProtocolo.NavigateUrl = "../Protocolos/ProtocoloEdit2.aspx?idServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString()+ "&Operacion=Modifica&idProtocolo=" +oRegistro.IdProtocolo.ToString();
            
            if (oRegistro.IdEfector == oRegistro.IdEfectorSolicitante)
                lblOrigen.Text = oRegistro.IdOrigen.Nombre;
            else
                lblOrigen.Text = oRegistro.IdEfectorSolicitante.Nombre;

            if ((oRegistro.IdEspecialistaSolicitante > 0) && (oRegistro.IdEfectorSolicitante == oRegistro.IdEfector))
            {
                try
                {
                    Profesional oMedico = new Profesional();
                    oMedico = (Profesional)oMedico.Get(typeof(Profesional), oRegistro.IdEspecialistaSolicitante);
                    if (oMedico != null)
                        lblMedico.Text = oMedico.Apellido + " " + oMedico.Nombre;
                }
                catch (Exception ex) { lblMedico.Text = ""; }
            }
            else
                lblMedico.Text = "";
            lblPrioridad.Text =  oRegistro.IdPrioridad.Nombre;
            if (oRegistro.IdPrioridad.Nombre == "URGENTE")
            {
                lblPrioridad.ForeColor = Color.Red;
                lblPrioridad.Font.Bold = true;
            }

            lblSector.Text = oRegistro.IdSector.Nombre;
            if (oRegistro.Sala != "") lblSector.Text += " Sala: " + oRegistro.Sala;
            if (oRegistro.Cama != "") lblSector.Text += " Cama: " + oRegistro.Cama;

            HFIdPaciente.Value = oRegistro.IdPaciente.IdPaciente.ToString();

            ///Datos del Paciente       
            if (oRegistro.IdPaciente.IdEstado == 1) lblDni.Text = oRegistro.IdPaciente.NumeroDocumento.ToString();
            else lblDni.Text = "(Sin DU Temporal)";            
            lblPaciente.Text = oRegistro.IdPaciente.Apellido.ToUpper() + " " + oRegistro.IdPaciente.Nombre.ToUpper();
            
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
                    else            lblDiagnostico.Text += " - " + oD.Nombre;
                    if (oD.Codigo=="Z32.1") embarazada="E";

                }
            }

            lblCodigoPaciente.Text = oRegistro.getCodificaHiv(embarazada); // lblSexo.Text.Substring(0, 1) + oRegistro.IdPaciente.Nombre.Substring(0, 2) + oRegistro.IdPaciente.Apellido.Substring(0, 2) + lblFechaNacimiento.Text.Replace("/", "") + embarazada;            
            ///Observaciones de Resultados al pie 
            if (oRegistro.ObservacionResultado != "")
            {
                lblObservacionResultado.Visible = true;
                lblObservacionResultado.Text =" Observaciones: " + oRegistro.ObservacionResultado;
            }


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

            //int cantidadResultadosValidados = Ds.Tables[0].Rows.Count;
            //if (cantidadResultadosValidados > 0)
            
            //{
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


                    decimal x = 0;



                    if (m_area != pivot_Area) ///poner titulo del area
                    {
                        TableRow objRow = new TableRow();
                        TableCell objCell = new TableCell();
                        Label lbl0 = new Label();
                        //lbl0.ForeColor = Color
                        lbl0.Text = m_area.ToUpper();
                        lbl0.TabIndex = short.Parse("500");
                        lbl0.Font.Bold = true;


                        Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(lbl0);
                        objCell.Controls.Add(lbl0);
                        //if (Request["Operacion"].ToString() == "HC")
                        //    objCell.ColumnSpan = 8;
                        //else
                        objCell.ColumnSpan = 8;

                        objRow.BackColor = Color.Beige;
                        objRow.HorizontalAlign = HorizontalAlign.Center;
                        objRow.Cells.Add(objCell);
                        //         objRow.CssClass = "myLabelIzquierda";
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


                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), m_idItem);

                    //DetalleProtocolo oDetalle = new DetalleProtocolo();
                    //oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);


                    bool es_Bacteriologia = false;
                    string observacionesDetalle = "";
                    ///Antes de mostrar el control verifica  si está derivado                    
                    if (oItem.IdEfectorDerivacion != oItem.IdEfector) //es derivado
                    {
                        Label lblDerivacion = new Label();
                        lblDerivacion.Font.Italic = true;
                        lblDerivacion.TabIndex = short.Parse("500");
                        //Verifica el estado de la derivacion
                        string estadoDerivacion = "";

                        if (i_iddetalleProtocolo != 0)
                        {
                            DetalleProtocolo oDetalle = new DetalleProtocolo();
                            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);

                            observacionesDetalle = oDetalle.Observaciones;

                            Derivacion oDeriva = new Derivacion();
                            oDeriva = (Derivacion)oDeriva.Get(typeof(Derivacion), "IdDetalleProtocolo", oDetalle);
                            if (oDeriva == null)  /// esta pendiente
                            {
                                estadoDerivacion = "Pendiente de Derivacion";
                                lblDerivacion.ForeColor = Color.Red;
                            }
                            else
                            {
                                if (oDeriva.Estado == 0) /// pendiente                            
                                {
                                    estadoDerivacion = "Pendiente de Derivacion";
                                    lblDerivacion.ForeColor = Color.Red;
                                }
                                if (oDeriva.Estado == 1) /// enviado
                                    estadoDerivacion = "Derivado: " + oItem.IdEfectorDerivacion.Nombre;
                                if (oDeriva.Estado == 2) /// no enviado
                                    estadoDerivacion = " No Derivado. " + oDeriva.Observacion;
                                lblDerivacion.Font.Bold = true;
                            }

                        }
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
                            if (tipodeterminacion == 0) // si es una determinacion simple
                            {
                                switch (tiporesultado)
                                {//tipoResultado
                                    case 1: //numerico
                                        {                                            
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

                                            if (i_iddetalleProtocolo != 0)
                                            {
                                                DetalleProtocolo oDetalle = new DetalleProtocolo();
                                                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), i_iddetalleProtocolo);



                                            if (Observaciones != "")
                                            {

                                                if (m_conResultado == "False")
                                                {
                                                    olbl.Text = Observaciones;
                                                    if (m_usuariovalida == "")
                                                    {
                                                        Usuario oUser = new Usuario();
                                                        oUser = (Usuario)oUser.Get(typeof(Usuario), oDetalle.IdUsuarioValidaObservacion);
                                                        if (oUser.FirmaValidacion == "") m_usuariovalida = oUser.Apellido + " " + oUser.Nombre; else m_usuariovalida = oUser.FirmaValidacion;
                                                    }
                                                }
                                                else
                                                    olbl.Text = olbl.Text + Observaciones;

                                            }



                                                
                                                string resultadoAnterior = oDetalle.BuscarResultadoAnterior(oDetalle.IdSubItem, oDetalle.IdItem,false);
                                                if (resultadoAnterior != "")
                                                {
                                                    hayAntecedente = true;
                                                    Label olblResultadoAnterior = new Label();
                                                    olblResultadoAnterior.TabIndex = short.Parse("500");
                                                    olblResultadoAnterior.Font.Size = FontUnit.Point(8);
                                                    //olblResultadoAnterior.CssClass = "myLink";
                                                    olblResultadoAnterior.Attributes.Add("onClick", "javascript: AntecedenteView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + "); return false");
                                                    olblResultadoAnterior.ToolTip = "Haga clic aqui para ver grafico de evolución";
                                                    //olblResultadoAnterior.ForeColor = Color.Green;
                                                    olblResultadoAnterior.Width = Unit.Pixel(20);
                                                    olblResultadoAnterior.Text = resultadoAnterior;

                                                    objCellResultadoAnterior.Controls.Add(olblResultadoAnterior);
                                                    //Button oB = new Button();
                                                    //oB.Text = "R.ANT";
                                                    //oB.OnClientClick = "javascript: AntecedenteView (" + oDetalle.IdSubItem.IdItem.ToString() + "," + oDetalle.IdProtocolo.IdPaciente.IdPaciente.ToString() + "); return false";
                                                    //objCellResultadoAnterior.Controls.Add(oB);
                                                }

                                               
                                                // if (VerificaValorReferencia(m_minimoReferencia, m_maximoReferencia, x, m_tipoValorReferencia))
                                                if (oDetalle.VerificaValorReferencia(x))
                                                    olbl.ForeColor = Color.Black;
                                                else
                                                    olbl.ForeColor = Color.Red;

                                            }
                                            objCellResultado.Controls.Add(olbl);
                                            ///etiqueta de unidad de medida
                                            Label olblUM = new Label();
                                            

                                            //     olblUM.ID = "UM" + m_idItem.ToString();
                                            olblUM.Font.Size = FontUnit.Point(7);
                                            olblUM.Text = unMedida;

                                            
                                           
                             
                                                
                                            objCellResultado.Controls.Add(olblUM);
                                            olblUM.Visible = false;

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

                                        } // fin case 1
                                        break;


                                }//fin swicth



                                Label lblPersona = new Label();
                                //    lblPersona.TabIndex = short.Parse("500");
                                lblPersona.Text = m_usuariovalida; /// Ds.Tables[0].Rows[i].ItemArray[1].ToString();      



                                /// 
                                lblPersona.Font.Size = FontUnit.Point(7);
                                lblPersona.Font.Italic = true;
                                lblPersona.Text = m_usuariovalida;

                                objCellPersona.Controls.Add(lblPersona);

                                
                            }


                            Label lblValoresReferencia = new Label();

                            //     lblValoresReferencia.ID = "VR" + m_idItem.ToString();
                            lblValoresReferencia.Font.Italic = true;
                            lblValoresReferencia.Font.Size = FontUnit.Point(8);
                            if (valorReferencia != "")
                            {// muestra el valor guardado 
                                lblValoresReferencia.Text = valorReferencia;
                                if (m_metodo != "")
                                    // lblValoresReferencia.Text += " |Método:" + m_metodo;
                                    lblValoresReferencia.Text += Environment.NewLine + m_metodo;
                            }
                            //else
                            //    lblValoresReferencia.Text = oDetalle.CalcularValoresReferencia();                                                                                      

                            objCellValoresReferencia.Controls.Add(lblValoresReferencia);
                        }
                    }


                    ///Definir los anchos de las columnas
                    objCellAnalisis.Width = Unit.Percentage(30);
                    objCellResultado.Width = Unit.Percentage(30);
                    objCellValoresReferencia.Width = Unit.Percentage(20);
                    //            objCellValida.Width = Unit.Percentage(5);
                    objCellPersona.Width = Unit.Percentage(20);



                    ///////////////////////
                    ///agrega a la fila cada una de las celdas
                    objFila.Cells.Add(objCellAnalisis);
                    objFila.Cells.Add(objCellResultado);
                    
                    //if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellUnMedida);

                    objFila.Cells.Add(objCellValoresReferencia);

                    //if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control")) objFila.Cells.Add(objCellValida);

                    objFila.Cells.Add(objCellPersona);
                    objFila.Cells.Add(objCellResultadoAnterior);
                    //if (Request["Operacion"].ToString() != "HC") objFila.Cells.Add(objCellObservaciones);

                    //////
                    Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").Controls.Add(tContenido);

                    //'añadimos la fila a la tabla
                    if (objFila != null)
                        tContenido.Controls.Add(objFila);//.Rows.Add(objRow);                                
                }
            //}
        }



        private void Imprimir(Protocolo oProtocolo, string tipo)
        {

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

            if (oProtocolo.IdTipoServicio.IdTipoServicio == 1) //laboratorio
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
            if (oProtocolo.IdTipoServicio.IdTipoServicio == 3) //microbilogia
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
                    oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                else
                    oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaA5.rpt";
            }
                 
                  
            
            ParameterDiscreteValue datosPaciente = new ParameterDiscreteValue();
            datosPaciente.Value = parametroPaciente;                     

            ParameterDiscreteValue datosProtocolo = new ParameterDiscreteValue();
            datosProtocolo.Value = parametroProtocolo;



            string m_filtro = " WHERE idProtocolo =" + oProtocolo.IdProtocolo;

            if (Request["idArea"].ToString() != "0") m_filtro += " and idArea=" + Request["idArea"].ToString();

            if (oProtocolo.IdTipoServicio.IdTipoServicio == 1)
            {
                //if (Request["Operacion"].ToString() != "Valida")
                //{
                //    if (Request["validado"].ToString() == "1") m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";
                //}

                if (Request["Operacion"].ToString() == "HC")
                {
                    m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";
                }

                
            }

            oCr.ReportDocument.SetDataSource(oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio));
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
                case 2: s_nombreProtocolo = oProtocolo.PrefijoSector+ oProtocolo.NumeroSector.ToString(); break;
                case 3: s_nombreProtocolo = oProtocolo.NumeroTipoServicio.ToString(); break;
            }

            if (tipo != "PDF")
            {
                oProtocolo.GrabarAuditoriaProtocolo("Imprime Resultados", int.Parse(Session["idUsuario"].ToString()));
                Session["Impresora"] = ddlImpresora.SelectedValue;

                oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;        
                oCr.ReportDocument.PrintToPrinter(1, false, 0,0);
                
                oProtocolo.Impreso = true;
                oProtocolo.Save();
            }
            else
            {
                oProtocolo.GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(Session["idUsuario"].ToString()));
                MemoryStream oStream; // using System.IO
                oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + s_nombreProtocolo + ".pdf");

                Response.BinaryWrite(oStream.ToArray());
                Response.End();
            }

        }

      
        
        

   

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Ingresar")
            {                                            
               dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
                  for (int i = 0; i < dtProtocolo.Rows.Count; i++)
                {
                   // dtProtocolo.Rows[i].Delete();
                    if (dtProtocolo.Rows[i][0].ToString() == e.CommandArgument.ToString())  CurrentIndexGrilla = i;
                }                
                CurrentPageIndex = int.Parse( e.CommandArgument.ToString());

                Response.Redirect("ResultadoView.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + e.CommandArgument + "&Index=" + CurrentIndexGrilla +  "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
            }

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdModificar.CommandArgument = gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Ingresar";                          
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
                        CurrentIndexGrilla = CurrentIndexGrilla - 1;  //retrocede

                    if (CurrentIndexGrilla > -1)
                    {
                        dtProtocolo = (System.Data.DataTable)(Session["Tabla1"]);
                        CurrentPageIndex = int.Parse(dtProtocolo.Rows[CurrentIndexGrilla][0].ToString());

                        Response.Redirect("ResultadoView.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + CurrentPageIndex + "&Index=" + CurrentIndexGrilla + "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
                    }
                }
                else
                    if (Request["Operacion"].ToString() == "HC")
                        Response.Redirect("../Informes/HistoriaClinicaFiltro.aspx?Tipo=Paciente", false);
                    else
                        Response.Redirect("ResultadoBusqueda.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);

            }
            else Response.Redirect("../FinSesion.aspx", false);                             


             
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

    

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            Imprimir(oProtocolo, "PDF");
        }

      
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {

            Marcar(true);

        }

        private void Marcar(bool p)
        {

            CheckBox chk;
             

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

                                                                    if (control5 is CheckBox)
                                                                    {
                                                                        chk = (CheckBox)control5;
                                                                        chk.Checked = p;
                                                                        
                                                                    
                                                                    
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

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            Marcar(false);
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
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), CurrentPageIndex);//int.Parse(Request["idProtocolo"].ToString()));r();
            Imprimir(oProtocolo, "I");       
        }

        protected void btnPeticion_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PeticionElectronica/PeticionEdit.aspx?idPaciente=" + HFIdPaciente.Value + "&Modifica=0&idTipoServicio=1&master=1", false);
        }


     
     

      
     
    }
}
