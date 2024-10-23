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
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.Services;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Text.RegularExpressions;
using Business.Data.GenMarcadores;

namespace WebLab.CasoFiliacion
{
    public partial class ImportarMarcadores : System.Web.UI.Page
    {

        DataTable dtDeterminaciones; //tabla para determinaciones


        private enum TabIndex
        {
            DEFAULT = 1,
            ONE = 2,
          

            // you can as many as you want here
        }
        //private void SetSelectedTab(TabIndex tabIndex)
        //{
        //    HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Casos Forense");
                CargarTablas();
            } 
        }

        //private void ValidacionUsuario()
        //{
        //    //////////////////Se controla quien es el usuario que está por validar////////////////
        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //    if ((oCon.AutenticaValidacion) && (Request["logIn"] == null)) Session["idUsuarioValida"] = null;
        //    if ((oCon.AutenticaValidacion) && (Session["idUsuarioValida"] == null))
        //    //    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);
        //    {
        //        //if ((Request["urgencia"] != null) && (oCon.AutenticaValidacion) && (Request["idUsuarioValida"] == null))
        //        string sredirect = "../Login.aspx?idServicio=6&Operacion=Valida&idCasoFiliacion=" + id.Value;
        //        if (Context.Items["Desde"].ToString() != null)
        //            sredirect += "&desde=" + Desde.Value;

        //        Response.Redirect(sredirect);
        //    }
        //    else
        //    {
            
        //        //Session["idUsuarioValida"] = Session["idUsuario"];
        //        btnGuardar.Visible = false;
        //        //btnValidar.Text = "Confirmar Validacion";
        //    }

        //    ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //}

        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnAgregar.Visible = false;
                            //btnAgregar.Visible = false;
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        //private void MostrarDatos()
        //{
        //    if (Desde.Value == "Carga")

        //        btnValidar.Visible = false;
        //    else
        //        btnGuardar.Visible = false;

        //    imgPdf.Visible = false;
        //    Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
        //    oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

        //    lblTipoCaso.Text = "FILIACION"; pnlMarcadoresFiliacion.Visible = true;
        //    if (oRegistro.IdTipoCaso == 2) //forense
        //    { txtMarcoEstudio.Text = ""; lblTipoCaso.Text = "FORENSE";
        //        pnlMarcadoresFiliacion.Visible = false;
        //    }


        //    lblTitulo.Text = oRegistro.IdCasoFiliacion.ToString();
        //    lblTitulo.Visible = true;
        //    lblNumero.Visible = true;
        //    txtNombre.Text = oRegistro.Nombre;


        //    if ((oRegistro.IdUsuarioValida == 0) && (oRegistro.IdUsuarioCarga > 0))
        //    {

        //        txtSolicitante.Text = oRegistro.Solicitante;
        //        txtAutos.Text = oRegistro.Autos;
        //        txtObjetivo.Text = oRegistro.Objetivo;
        //        //txtMuestra.Text = oRegistro.Muestra;
        //        txtResultado.Text = oRegistro.Resultado;
        //        txtConclusion.Text = oRegistro.Conclusion;
        //        txtMetodo.Text = oRegistro.Metodo;
        //        txtAmplificacion.Text = oRegistro.Amplificacion;
        //        txtAnalisis.Text = oRegistro.Analisis;
        //        txtSoftware.Text = oRegistro.Software;
        //        txtEstadistico.Text = oRegistro.Estadistico;
        //        txtMarcoEstudio.Text = oRegistro.Marcoestudio;
        //        txtBibliografia.Text = oRegistro.Bibliografia;
        //        txtCuantificacion.Text = oRegistro.Cuantificacion;
        //        txtProbabilidad.Text = oRegistro.Probabilidad;
        //        Usuario oUser = new Usuario();
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), oRegistro.IdUsuarioCarga);
        //        lblUsuario.Text = "Resultados cargados por " + oUser.Apellido + " " + oUser.Nombre + " - Fecha: " + oRegistro.FechaCarga.ToShortDateString() + " " + oRegistro.FechaCarga.ToShortTimeString();
        //        lblUsuario.Visible = true;
        //        lblUsuario.ForeColor = System.Drawing.Color.Black;
        //        btnBorrarTablaMarcadores.Visible = true;
        //    }


        //    if (oRegistro.IdUsuarioValida > 0)
        //    {
        //        txtSolicitante.Text = oRegistro.Solicitante;
        //        txtAutos.Text = oRegistro.Autos;
        //        txtObjetivo.Text = oRegistro.Objetivo;
        //        //txtMuestra.Text = oRegistro.Muestra;
        //        txtResultado.Text = oRegistro.Resultado;
        //        txtConclusion.Text = oRegistro.Conclusion;
        //        txtMetodo.Text = oRegistro.Metodo;
        //        txtAmplificacion.Text = oRegistro.Amplificacion;
        //        txtAnalisis.Text = oRegistro.Analisis;
        //        txtSoftware.Text = oRegistro.Software;
        //        txtEstadistico.Text = oRegistro.Estadistico;
        //        txtMarcoEstudio.Text = oRegistro.Marcoestudio;
        //        txtCuantificacion.Text = oRegistro.Cuantificacion;
        //        txtBibliografia.Text = oRegistro.Bibliografia;
        //        txtProbabilidad.Text = oRegistro.Probabilidad;

        //        Usuario oUser = new Usuario();
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), oRegistro.IdUsuarioValida);
        //        lblUsuario.Text = "Validado por " + oUser.Apellido + " " + oUser.Nombre + " - Fecha: " + oRegistro.FechaValida.ToShortDateString() + " " + oRegistro.FechaValida.ToShortTimeString();
        //        lblUsuario.Visible = true; lblUsuario.ForeColor = System.Drawing.Color.DarkRed;
        //        imgPdf.Visible = true;
        //        if (Desde.Value == "Carga")
        //        {
        //            btnGuardar.Visible = false;
        //            subir.Visible = false;
        //            subir0.Visible = false;
        //            //btnBorrarImg.Visible = false;
        //        }
        //    }

        //    ///cargar foto


        //    Image1.ImageUrl = string.Format("../imagen.ashx?id={0}", oRegistro.IdCasoFiliacion.ToString());
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //    string qry = "select imgResultado from lab_CasoFiliacion where idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString();
        //    SqlDataAdapter ad = new SqlDataAdapter(qry, conn);
        //    DataTable dt = new DataTable();
        //    ad.Fill(dt);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (dr["imgResultado"] != null)
        //        {
        //            btnBorrarImg.Visible = true;
        //            Image1.Visible = true;
        //        }
        //        else
        //        {
        //            btnBorrarImg.Visible = false;
        //            Image1.Visible = false;
        //        }
        //    }



        //    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
        //    CasoFiliacionProtocolo oDetalle = new CasoFiliacionProtocolo();
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
        //    crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));


        

        //    IList items = crit.List();            
        //    foreach (CasoFiliacionProtocolo oDet in items)
        //    {             
        //        DataRow row = dtDeterminaciones.NewRow();
        //        row[0] = oDet.IdProtocolo.Numero.ToString();
        //        row[1] = oDet.IdTipoParentesco.ToString() + " " + oDet.ObservacionParentesco;
        //        TipoParentesco oTP = new TipoParentesco();                oTP = (TipoParentesco)oTP.Get(typeof(TipoParentesco), oDet.IdTipoParentesco);
        //        row[2] = oTP.Nombre;
        //        string datosMuestra= oDet.IdProtocolo.Numero.ToString() + " " + oDet.IdProtocolo.IdPaciente.Apellido + " " + oDet.IdProtocolo.IdPaciente.Nombre;

                
        //       if (oDet.IdProtocolo.IdPaciente.IdPaciente==-1)
        //            datosMuestra += oDet.IdProtocolo.DescripcionProducto;
        //        else
        //        {
        //            if (oDet.IdProtocolo.IdPaciente.IdEstado == 1)
        //                datosMuestra += "- DNI: " + oDet.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
        //            else
        //                datosMuestra += "- HC: " + oDet.IdProtocolo.IdPaciente.NumeroDocumento.ToString();
        //        }


        //        row[3] = datosMuestra;
        //        row[4] = oDet.IdProtocolo.IdPaciente.IdPaciente.ToString();
        //        string directorio =  "..\\Protocolos\\"+oDet.IdProtocolo.Numero+"\\";
        //        row[5] =directorio+ oDet.IdProtocolo.getPrimerAdjuntoVisible();
        //        row[6] = "";
        //        dtDeterminaciones.Rows.Add(row);

        //    }
        //    Session.Add("Tabla1", dtDeterminaciones);
        //    gvLista.DataSource = dtDeterminaciones;
        //    gvLista.DataBind();
        //    CargarTablaResultados(2);
          



        //        oRegistro.GrabarAuditoria("Consulta Caso", int.Parse(Session["idUsuario"].ToString()),"");

        //}
        //private void InicializarTablas()
        //{
        //    ///Inicializa las sesiones para las tablas de diagnosticos y de determinaciones
        //    if (Session["Tabla1"] != null) Session["Tabla1"] = null;
        //    //if (Session["Tabla2"] != null) Session["Tabla2"] = null;

        //    dtDeterminaciones = new DataTable();


        //    dtDeterminaciones.Columns.Add("id"); // numero de protocolo
        //    dtDeterminaciones.Columns.Add("idtipoparentesco"); 
        //    dtDeterminaciones.Columns.Add("nombre"); //nombre del parentescto
        //    dtDeterminaciones.Columns.Add("protocolo"); // numero + apellido y nombre
        //    dtDeterminaciones.Columns.Add("idPaciente"); // foto
        //    dtDeterminaciones.Columns.Add("url");
        //    dtDeterminaciones.Columns.Add("eliminar");


        //    Session.Add("Tabla1", dtDeterminaciones);


        //}



        //protected void btnGuardar_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //       Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
        //        if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

        //        Guardar(oRegistro);

        //        if (Request["Desde"] != null)
        //        {
        //            if (Request["Desde"].ToString() == "Valida")
        //                Response.Redirect("FiliacionMenu.aspx");
        //        }
        //        else { 
        //        HttpContext Context;

        //        Context = HttpContext.Current;
        //        Context.Items.Add("idServicio", "6");
        //        Server.Transfer("CasoList.aspx");
        //        }
        //        //Response.Redirect("CasoList.aspx?idServicio=6", false);
        //    }
        //}

        //private void Guardar(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        //{
        //    Utility oUtil = new Utility();

        //    Usuario oUser = new Usuario();
        //    oRegistro.Solicitante = txtSolicitante.Text;
        //    oRegistro.Autos = txtAutos.Text;

        //    //string s_nombre = Regex.Replace(txtNombre.Text, @"[^0-9A-Za-z]", "", RegexOptions.None);
        //    oRegistro.Nombre = oUtil.SacaComillas(oUtil.RemoverSignosAcentos(txtNombre.Text));
        //    //oRegistro.Nombre = txtNombre.Text;
        //    oRegistro.Objetivo = txtObjetivo.Text;
        //    //oRegistro.Muestra = txtMuestra.Text;
        //    oRegistro.Resultado = txtResultado.Text;
        // //   oRegistro.Conclusion =   txtConclusion.Text;
        //    oRegistro.Metodo = txtMetodo.Text;
        //    oRegistro.Amplificacion = txtAmplificacion.Text;
        //    oRegistro.Analisis = txtAnalisis.Text;
        //    oRegistro.Software = txtSoftware.Text;
        //    oRegistro.Cuantificacion = txtCuantificacion.Text;
        //    oRegistro.Estadistico = txtEstadistico.Text;
        //    oRegistro.Marcoestudio = txtMarcoEstudio.Text;
        //    oRegistro.Bibliografia = txtBibliografia.Text;
        //    oRegistro.Probabilidad = txtProbabilidad.Text;


        //    /// GUARDA MARCADOR TOTAL            

        //    CasoMarcadores oMarca = new CasoMarcadores();
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit2 = m_session.CreateCriteria(typeof(CasoMarcadores));
        //    crit2.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
        //    crit2.Add(Expression.Eq("IdProtocolo", 0));
        //    crit2.Add(Expression.Eq("Marcador", "TOTAL"));

        //    IList itemsm = crit2.List();
        //    foreach (CasoMarcadores oM in itemsm)
        //    {
        //        oM.Ip = txTotalLR.Text;
        //        oM.Save();
        //    }
        //    ////
        //    /// 

        //    string s_desde = "Carga";
        //    if (Desde.Value == "Valida")
        //    {
                
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));

                
        //        s_desde = "Valida";
        //        oRegistro.FechaValida = DateTime.Now;
        //    oRegistro.IdUsuarioValida = oUser.IdUsuario;
               
        //    }

        //    else
        //    {
              
        //        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        //        oRegistro.FechaCarga = DateTime.Now;
        //        oRegistro.IdUsuarioCarga = oUser.IdUsuario;

        //    }
        //    oRegistro.Save();


        //    /// actualizar a mano la conclusion
        //   // SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //   // string query = @"update [dbo].[LAB_CasoFiliacion] set conclusion=" + txtConclusion.Text + " where idcasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString();          

        //   // SqlCommand cmd = new SqlCommand(query, conn);

          

        //   //int idcaso = Convert.ToInt32(cmd.ExecuteScalar());
        //   // //

        //    try
        //    { 
        //    ///grabaar imagen
        //    Int32 idconsenti;
        //        byte[] image = null;


        //        if (trepadorFoto.PostedFile.ContentLength > 0)
        //        {
        //            using (BinaryReader reader = new BinaryReader(trepadorFoto.PostedFile.InputStream))
        //            {
        //                image = reader.ReadBytes(trepadorFoto.PostedFile.ContentLength);
        //                string directorio = Server.MapPath("") + "..\\Consentimientos\\Fotos";
        //                if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);
        //                string archivo = directorio + "\\" + trepadorFoto.FileName;
        //                trepadorFoto.SaveAs(archivo);
        //            }
        //        }/*hasta aca trepador*/
        //        else
        //        {
        //            if (Session["CapturedImage"] != null)
        //            {
        //                //string path = Server.MapPath(Session["CapturedImage1"].ToString());
        //                string path = Server.MapPath(Session["CapturedImage"].ToString());
        //                // 2.
        //                // Get byte array of file.
        //                image = File.ReadAllBytes(path);
        //                // 3A.
        //            }

        //        }
        //        ///// separar conclusion de imagen en el update.


        //        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //        string query = @"update LAB_CasoFiliacion set   conclusion=@conclusion where  idCasoFiliacion= " + oRegistro.IdCasoFiliacion.ToString();
        //        SqlCommand cmd = new SqlCommand(query, conn);

             

        //        SqlParameter conclusionParam = cmd.Parameters.Add("@conclusion", System.Data.SqlDbType.VarChar);
        //        conclusionParam.Value = txtConclusion.Text;


        //        idconsenti = Convert.ToInt32(cmd.ExecuteScalar());


        //        SqlConnection conn1 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //        query = @"update LAB_CasoFiliacion set imgResultado=@imagen   where  idCasoFiliacion= " + oRegistro.IdCasoFiliacion.ToString();
        //        SqlCommand cmd1 = new SqlCommand(query, conn1);

        //        SqlParameter imageParam = cmd1.Parameters.Add("@imagen", System.Data.SqlDbType.Image);
        //        imageParam.Value = image;




        //        idconsenti = Convert.ToInt32(cmd1.ExecuteScalar());


        //        /// fin grabar imagen
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = "";
        //        //while (ex != null)
        //        //{
        //        exception = ex.Message + "<br>";

        //        //}
        //    }

        //    oRegistro.GrabarAuditoria(s_desde + " resultados", oUser.IdUsuario,"");
            



        //}

        //private void GuardarDetalle(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        //{
        //    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

        //    ///Eliminar los detalles para volverlos a crear            
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
        //    crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
        //    IList detalle = crit.List();
        //    if (detalle.Count > 0)
        //    {
        //        foreach (CasoFiliacionProtocolo oDetalle in detalle)
        //        {
        //            oDetalle.Delete();
        //        }
        //    }

        //    /////Crea nuevamente los detalles.
        //    for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
        //    {
        //        CasoFiliacionProtocolo oDetalle = new CasoFiliacionProtocolo();
        //        Protocolo oItem = new Protocolo();
        //        oDetalle.IdCasoFiliacion = oRegistro;
        //        oDetalle.IdTipoParentesco = int.Parse(dtDeterminaciones.Rows[i][1].ToString());
        //        oDetalle.IdProtocolo= (Protocolo)oItem.Get(typeof(Protocolo),"Numero", dtDeterminaciones.Rows[i][0].ToString());
              
        //        oDetalle.Save();                           
        //    }
         
        //}

     



        //protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Eliminar")
        //    {
        //        dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
        //        for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
        //        {
        //            if (dtDeterminaciones.Rows[i][0].ToString() == e.CommandArgument.ToString())
        //                dtDeterminaciones.Rows[i].Delete();
        //        }
        //        Session.Add("Tabla1", dtDeterminaciones);
        //        gvLista.DataSource = dtDeterminaciones;
        //        gvLista.DataBind();

              
        //    }
        //}

        protected void cvListaDeterminaciones_ServerValidate(object sender, EventArgs e)
        {

        }

        //protected void gvLista_RowDataBound1(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
              


        //        Image btnUrl = (Image)e.Row.FindControl("img");

        //        //Protocolo oRegistro1 = new Protocolo();
        //        //oRegistro1 = (Protocolo)oRegistro1.Get(typeof(Protocolo), int.Parse(hdnProtocolo.Value));
        //        //// oRegistro.IdProtocolo.GrabarAuditoriaProtocolo("Elimina Archivo: " + oRegistro.Url, int.Parse(Session["idUsuario"].ToString()));
        //        ////oRegistro1.Delete(); //CargarGrilla();

        //        //string directorio = oRegistro1.GetNumero() + "\\" + btnUrl.ImageUrl.ToString();
        //        string directorio = btnUrl.ImageUrl.ToString();
        //        string extension = System.IO.Path.GetExtension(directorio).ToLower();
        //        if (extension == ".pdf")

        //            btnUrl.ImageUrl = "..\\App_Themes\\default\\images\\pdfgrande.jpg";

        //    }
        //}

        protected void ddlServicio_SelectedIndexChanged()
        {

        }

       
        protected void cvListaDeterminaciones_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            if (Session["Tabla1"] != null)
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                if (dtDeterminaciones.Rows.Count == 0) args.IsValid = false;
                else args.IsValid = true;
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

       

     
        private void ProcesarFichero()
        {
         


            if (this.trepador.HasFile)
            {
                string filename = this.trepador.PostedFile.FileName;
                BorrarResultadosTemporales(1);
                int i = 1;
                if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                {
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            if (i != 1)

                                ProcesarLinea(line,1);

                            i += 1;
                        }
                    }
                }
            }
        }
        
        private void ProcesarLinea(string linea, int tipo)
        {
            try
            {
               

                Utility oUtil = new Utility();
                string[] arr = linea.Split(("\t").ToCharArray());


               
                    if (tipo == 1)
                    {
                    if (arr.Length >= 4)
                    {
                        string campo_cero = arr[0].ToString();
                        string campo_MFI = arr[1].ToString();
                        string campo_Bead = arr[2].ToString();
                        string campo_valor = arr[3].ToString();

                        Protocolo oP = new Protocolo();
                        oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", campo_cero.Substring(0, 5));

                        if (oP != null)
                        {    // carga directamente en la base nueva 

                            MarcadoresTemp oRegistro = new MarcadoresTemp();

                            oRegistro.IdProtocolo = oP.IdProtocolo;
                            oRegistro.IdPaciente = oP.IdPaciente.IdPaciente;

                            oRegistro.Marcador = campo_MFI;
                            oRegistro.Allello1 = campo_Bead;
                            oRegistro.Allello2 = campo_valor;

                            oRegistro.Save();
                        }

                    } //tipo ==1
                   

                }



               
                 
                 

            }
            catch (Exception ex)
            {
                string exception = "";
                exception = ex.Message + "<br>";
                estatus1.Text = "hay líneas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }
        

        private void BorrarResultadosTemporales(int tipo)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(MarcadoresTemp));          

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (MarcadoresTemp oDetalle in detalle)
                {                  
                            oDetalle.Delete();                  

                }
            }
        }
        protected void subir_Click(object sender, EventArgs e)
        {
               
            try
            {
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

                    if (Directory.Exists(directorio))
                    {
                        string archivo = directorio +   "\\" + trepador.FileName;


                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";


                        ProcesarFichero();

                         CargarTablaResultados(1);

  //SetSelectedTab(TabIndex.ONE);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }

          
        }

        private void CargarTablaResultados(int tipo)
        {
            gvTablaForense.DataSource = LeerDatos(tipo);
            gvTablaForense.DataBind();

           

        }

        private object LeerDatos(int tipo)
        {
            string columnas = "";
            string lista  = "";
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;

            string m_strSQL = @"  select distinct idprotocolo from [GEN_MarcadoresTemp]  ";
            
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                string s_idprotocolo = Ds.Tables[0].Rows[i][0].ToString();

                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idprotocolo));

                if (lista == "")
                    lista = s_idprotocolo;
                else
                    lista += ", " + s_idprotocolo;


                if (columnas == "")
                    columnas = "[" + oProtocolo.Numero.ToString() + "]";
                else
                    columnas += ",[" + oProtocolo.Numero.ToString() + "]";
            }

            m_strSQL = @" SELECT tipo as [ ], " + columnas + @"
 FROM(    
  SELECT isnull( P.numero,0) as numero, marcador as tipo , 
  allello1 +' - ' +   case when marcador<>'DYS391' then
  case when allello2='' then allello1 else allello2 end   
  else allello2 end  as y,  
  
  case marcador  when 'AMEL' then 1 when 'D3S1358' then 2 when 'D1S1656' then 3 when 'D2S441' 
  then 4 when 'D10S1248' then 5 when 'D13S317' then 6 when 'PENTA E' then 7 when 'D16S539' then 8 when 'D18S51' then 9 
  when 'D2S1338' then 10 when 'CSF1PO' then 11 when 'PENTA D' then 12 when 'TH01' then 13 when 'VWA' then 14 when 'D21S11' 
  then 15 when 'D7S820' then 16 when 'D5S818' then 17 when 'TPOX' then 18 when 'DYS391' then 19 when 'D8S1179' then 20 
  when 'D12S391' then 21 when 'D19S433' then 22 when 'FGA' then 23 when 'D22S1045' then 24 end as orden    
   FROM GEN_MarcadoresTemp    as M
				  left join LAB_Protocolo as P on P.idprotocolo= M.idprotocolo
				    where   M.idprotocolo in (" + lista + @") and M.idprotocolo not in (select idprotocolo from gen_marcadores))
   AS SourceTable PIVOT(max(y) 
   FOR numero IN(" + columnas + @" )) AS PivotTable order by orden";


            DataSet Ds1 = new DataSet();
           
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1);

            return Ds1.Tables[0];
        }

       

        //protected void btnBorrarTablaMarcadores_Click(object sender, EventArgs e)
        //{
        //    Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
        //    oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

        //    CasoMarcadores oMarca = new CasoMarcadores();
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit2 = m_session.CreateCriteria(typeof(CasoMarcadores));
        //    crit2.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
          


        //    IList itemsm = crit2.List();
        //    foreach (CasoMarcadores oM in itemsm)
        //    {
        //        oM.Delete();
        //    }
        //    CargarTablaResultados(2);

        //}
        private void CargarTablas()
        {
           

            gvBaseGen.DataSource = LeerGEN();
            gvBaseGen.DataBind();
        }

        private object LeerGEN()
        {
            string m_strSQL = @"select distinct   P.numero as [Protocolo], Pac.apellido + ' ' + Pac.nombre as Paciente, P.edad as Edad, P.sexo as Sexo
 from Gen_Marcadores  as CM
inner join LAB_Protocolo as P on p.idProtocolo= CM.idProtocolo
inner join Sys_Paciente as Pac on Pac.idPaciente= P.idPaciente   ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            estatus.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {

           

                    ISession m_session = NHibernateHttpModule.CurrentSession;

                    //verifica que ingrese solo el protocolo del caso
                    MarcadoresTemp oCasoMarcadores = new MarcadoresTemp();
                    ICriteria crit = m_session.CreateCriteria(typeof(MarcadoresTemp));
                  

                    IList detalle = crit.List();
                    foreach (MarcadoresTemp oDetalle in detalle)
                    {
                        Protocolo oProtocolo = new Protocolo();
                        oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), oDetalle.IdProtocolo);

                        /// se agrega a la base de frecuencias alelicas y a la base total de marcadores
                        /// 
                        CasoMarcadores oBase = new CasoMarcadores();
                        oBase.IdProtocolo = oDetalle.IdProtocolo;
                        oBase.IdCasoFiliacion = 0;              
                        oBase.Marcador = oDetalle.Marcador;
                        oBase.Allello1 = oDetalle.Allello1;
                        oBase.Allello2 = oDetalle.Allello2;
                        oBase.Save();

                        Marcadores oRegistro = new Marcadores();
                        oRegistro.IdProtocolo = oDetalle.IdProtocolo;                       
                        oRegistro.IdPaciente = oProtocolo.IdPaciente.IdPaciente;
                        oRegistro.Marcador = oDetalle.Marcador;
                        oRegistro.Allello1 = oDetalle.Allello1;
                        oRegistro.Allello2 = oDetalle.Allello2;
                        oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                        oRegistro.FechaRegistro = DateTime.Now;
                        oRegistro.Save();



                    } //for                
               

            CargarTablas();
        }
    }
     
}
