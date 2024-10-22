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
using System.Web.UI.HtmlControls;

namespace WebLab.Resultados
{
    public partial class ResultadosaSisa : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                 VerificaPermisos("Resultados a SISA");
          

                //CargarListas();
                //CargarGrilla();                   

            }


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
        protected void subir_Click(object sender, EventArgs e)
        {
            try
            {
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

                    if (Directory.Exists(directorio))
                    {
                        string archivo = directorio + "\\" + trepador.FileName;

                    
                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";                       
                            ProcesarFichero();                     


                            CargarGrilla();
                      
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString()+ " .Comuniquese con el administrador."; }
        }
   

        private void CargarGrilla()
        {          

                string m_strSQL = @" SELECT  distinct iddetalleprotocolo,
      [idevento]
      ,[dni]
      ,A.[nombre]
      ,0 as [idDerivacion]
      ,0 as [idEventoMuestra]
	  , d.resultadoCar,
	  A.fechaToma
  FROM [dbo].[LAB_Temp_ResultadoSISA] a
  

  INNER JOIN LAB_Protocolo C ON c.idCasoSISA= a.idevento 
  inner join LAB_DetalleProtocolo d on d.idProtocolo= c.idProtocolo 
  inner join lab_item i on i.idItem= d.idItem and i.codigo='9122'
  where resultadocar<>'' and d.idusuariovalida>0   
  and resultadocar IN ('NO SE DETECTA GENOMA DE SARS-CoV-2', 'SE DETECTA GENOMA DE SARS-CoV-2')



  ";

          if   (ddlOrigen.SelectedValue=="1") //derivaciones sin responder
                m_strSQL = @" SELECT  distinct   iddetalleprotocolo,
      [idevento]
      ,[dni]
      ,A.[nombre]
      ,idDerivacion as [idDerivacion]
      ,idEventoMuestra as [idEventoMuestra]
	  , d.resultadoCar,
	  A.fechaToma
  FROM [dbo].[LAB_Temp_ResultadoSISA] a
  
  inner join sys_paciente Pa on Pa.numeroDocumento= replace( replace( A.dni,'F',''),'M','') and Pa.idestado=3
  INNER JOIN LAB_Protocolo C ON C.idPaciente= Pa.idPaciente  and A.fechaToma=C.fechaTomaMuestra  
  
  inner join LAB_DetalleProtocolo d on d.idProtocolo= c.idProtocolo 
  inner join lab_item i on i.idItem= d.idItem and i.codigo='9122'
  where   d.idusuariovalida>0    and C.baja=0 and d.ideventomuestrasisa=0  
      
  and resultadocar IN ('NO SE DETECTA GENOMA DE SARS-CoV-2', 'SE DETECTA GENOMA DE SARS-CoV-2')


  ";



            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

           

            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
           

        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            //PintarReferencias();
        }
 

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            //PintarReferencias();
        }

        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }

        private void ProcesarFichero()
        {
            try {
                if (this.trepador.HasFile)
                {
                    string filename = this.trepador.PostedFile.FileName;
                    BorrarResultadosTemporales();
                    int i = 1;
                    if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                    {
                        string line;
                        StringBuilder log = new StringBuilder();
                        Stream stream = this.trepador.FileContent;
                        string extension = filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper();


                        using (StreamReader sr = new StreamReader(stream))
                        {
                            while (i <= 3000)//((!string.IsNullOrEmpty(line = sr.ReadLine()) && (i > 10)))

                            {

                                line = sr.ReadLine();

                                if (ddlOrigen.SelectedValue == "1") //deirvaciones sin responder
                                    ProcesarLinea(line);
                                    if (ddlOrigen.SelectedValue == "2") //eventos desde el sil
                                        ProcesarLineadesdeAPI(line);  
                                i += 1;
                            }
                        }
                    }
                }
            }

            catch { }
        }

        private void ProcesarLineadesdeAPI(string linea)
        {
            try
            {
                string[] arr = linea.Split((";").ToCharArray());


                if (arr.Length >= 1)
                {


                    string idevento = arr[0].ToString();
                    string documento = arr[1].ToString(); // Replace ("DNI","").Replace("IND","").Trim();
                     
                        string nombre = arr[2].ToString();
                        string idDerivacion = arr[3].ToString();
                        string idEventoMuestra = arr[4].ToString();
                        documento = arr[1].ToString().Replace("DNI", "").Trim();
                        DateTime fechatoma = DateTime.Parse(arr[5].ToString().Trim());
                        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                        string query = @"
INSERT INTO [dbo].[LAB_Temp_ResultadoSISA]
           ([idevento]
           ,[dni]
           ,[nombre]
           ,[idDerivacion]
           ,[idEventoMuestra]
           ,idUsuario
            ,fechatoma
            )
     VALUES
           ( " + idevento + ",'" + documento + "','" + nombre + "'," + idDerivacion + "," + idEventoMuestra + "," + Session["idUsuario"].ToString() + ",'" + fechatoma + "' )";
                        SqlCommand cmd = new SqlCommand(query, conn);


                        int idres = Convert.ToInt32(cmd.ExecuteScalar());

                    

                }

            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

                // estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }
        private void BorrarResultadosTemporales()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_Temp_ResultadoSISA] where idusuario=" + Session["idUsuario"].ToString();
            SqlCommand cmd = new SqlCommand(query, conn);


            int idres = Convert.ToInt32(cmd.ExecuteScalar());

        }


        private void BorrarLineaTemporales(string idderivacion, string ideventomuestra)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_Temp_ResultadoSISA] where [idDerivacion]=" + idderivacion+" and [idEventoMuestra]=" + ideventomuestra;
            SqlCommand cmd = new SqlCommand(query, conn);

         



            int idres = Convert.ToInt32(cmd.ExecuteScalar());

        }
        private void BorrarLineaTemporalesdesdeAPI(string ideventocaso)
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_Temp_ResultadoSISA] where [idEvento]=" + ideventocaso;
            SqlCommand cmd = new SqlCommand(query, conn);





            int idres = Convert.ToInt32(cmd.ExecuteScalar());

        }




        private void ProcesarLinea(string linea)
        {
            try
            {
                string[] arr = linea.Split((";").ToCharArray());
              

                if (arr.Length >= 1)
                {
                   

                    string idevento= arr[0].ToString();
                    string documento = arr[1].ToString().Substring(0, 3); // Replace ("DNI","").Replace("IND","").Trim();
                    if (documento == "DNI")
                    {
                        string nombre = arr[2].ToString();
                        string idDerivacion = arr[9].ToString();
                        string idEventoMuestra = arr[15].ToString();
                        documento= arr[1].ToString().Replace ("DNI","").Trim();
                        documento = documento.Replace("F", "").Replace("M", "").Trim();
                        DateTime fechatoma = DateTime.Parse(arr[14].ToString().Trim());

                      string ftoma=  fechatoma.ToString("yyyy-MM-dd");
                        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                        string query = @"
INSERT INTO [dbo].[LAB_Temp_ResultadoSISA]
           ([idevento]
           ,[dni]
           ,[nombre]
           ,[idDerivacion]
           ,[idEventoMuestra]
           ,idUsuario
            ,fechatoma
            )
     VALUES
           ( " + idevento + ",'" + documento + "','" + nombre + "'," + idDerivacion + "," + idEventoMuestra + "," + Session["idUsuario"].ToString() +",'" + ftoma + "' )";
                        SqlCommand cmd = new SqlCommand(query, conn);


                        int idres = Convert.ToInt32(cmd.ExecuteScalar());

                    }

                }

            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

               // estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ddlOrigen.SelectedValue == "1") //derivaciones sin responder

            {
                SubirSISA();

            }
            if (ddlOrigen.SelectedValue == "2") //eventos desde sil
                SubirSISAdesdeAPI();

        }

        private bool ProcesaSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;

            try
            {
                if (oDetalle.IdProtocolo.IdCasoSISA == 0)
                {
                    generacaso = GenerarCasoSISA(oDetalle, res);

                }

                if ((oDetalle.IdProtocolo.IdCasoSISA > 0) && (oDetalle.IdeventomuestraSISA==0))
                    GenerarMuestraSISA(oDetalle.IdProtocolo);

                if (oDetalle.IdeventomuestraSISA > 0)
                    GenerarResultadoSISA(oDetalle);


            }
            catch (Exception e)
            {
                generacaso = false;


            }
            return generacaso;

        }
        private bool GenerarCasoSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;
            string caracter = "";
            string idevento = "";
            string nombreevento = "";
            string idclasificacionmanual = "";
            string nombreclasificacionmanual = "";
            string idgrupoevento = "";
            string nombregrupoevento = "";
            bool seguir = true;
            string m_strSQL = "";

            try
            {



                if (res == "SE DETECTA")
                    m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1  ";
                else

                {
                    // si es contacto se sube: si es negativo como contacto y si es positivo como sospechoso.
                    if (oDetalle.IdProtocolo.IdCaracter == 4)  //contacto 
                        m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=  " + oDetalle.IdProtocolo.IdCaracter.ToString();
                    else
                    {
                        if (oDetalle.IdProtocolo.IdCaracter == 8)  //operativo detectar se sube como sospechoso
                            m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1  ";
                        else
                            seguir = false;
                    }
                }

                // nose notificò antes y es sospechoso o contacto


                if (seguir)
                {
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
                        nombreevento = dt.Rows[i][3].ToString();
                        idclasificacionmanual = dt.Rows[i][4].ToString();
                        nombreclasificacionmanual = dt.Rows[i][5].ToString();
                        idgrupoevento = dt.Rows[i][6].ToString();
                        nombregrupoevento = dt.Rows[i][7].ToString();

                    }


                    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                    string URL = oCon.UrlServicioSISA;
                    string s_idestablecimiento = oCon.CodigoEstablecimientoSISA; // "14580562167000"
                    string usersisa = ConfigurationManager.AppSettings["usuarioSisa"].ToString();
                    string[] a = usersisa.Split(':');
                    string s_user = a[0].ToString();
                    string s_userpass = a[1].ToString();

                    string s_sexo = "";
                    switch (oDetalle.IdProtocolo.IdPaciente.IdSexo)
                    {
                        case 1: s_sexo = "I"; break;
                        case 2: s_sexo = "F"; break;
                        case 3: s_sexo = "M"; break;
                    }
                    string fn = oDetalle.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "-");

                    string fnpapel = oDetalle.IdProtocolo.FechaOrden.ToShortDateString().Replace("/", "-");


                    string numerodocumento = oDetalle.IdProtocolo.IdPaciente.NumeroDocumento.ToString();

                    string error = "";
                    //bool hayerror = false;

                    evento newevento = new evento
                    {
                        idTipodoc = "1",
                        nrodoc = numerodocumento,
                        sexo = s_sexo,
                        fechaNacimiento = fn,  //"05-06-1989",
                        idGrupoEvento = idgrupoevento,
                        idEvento = idevento, // "77",
                        idEstablecimientoCarga = s_idestablecimiento, //prod: "51580352167442",
                        fechaPapel = fnpapel, // "10-12-2019",
                        idClasificacionManualCaso = idclasificacionmanual, // "22"
                    };

                    AltaCaso caso = new AltaCaso
                    {
                        usuario = s_user,
                        clave = s_userpass,
                        altaEventoCasoNominal = newevento
                    };

                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                    string DATA = jsonSerializer.Serialize(caso);



                    System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
                    client.BaseAddress = new System.Uri(URL);

                    System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");
                    HttpResponseMessage messge = client.PostAsync(URL, content).Result;
                    string description = string.Empty;
                    if (messge.IsSuccessStatusCode)
                    {
                        string result = messge.Content.ReadAsStringAsync().Result;
                        description = result;
                        RespuestaCaso respuesta_d = jsonSerializer.Deserialize<RespuestaCaso>(description);

                        if (respuesta_d.resultado == "OK")
                        { //  devolver el idcaso para guardar en la base de datos
                            string s_idcaso = respuesta_d.id_caso;

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuarioValida"].ToString()));

                            generacaso = true;

                            //generar muestra
                            GenerarMuestraSISA(oDetalle.IdProtocolo);




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
            catch
            {
                generacaso = false;
                //lblError.Text = "Hubo algun problema al conectar al servicio SISA: " + e.InnerException.InnerException.Message.ToString() + ". Intente de nuevo o haga clic en Salir";
                //lblError.Visible = true;
                //btnSalir.Visible = true;
            }
            return generacaso;

        }

        private void GenerarMuestraSISA(Protocolo protocolo)

        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLMuestraSISA;


            bool generacaso = true;
            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if ((idestablecimientotoma == "")|| (idestablecimientotoma == "0"))
                //pongo por defecto laboratorio central
                idestablecimientotoma = "107093";
           

            ResultadoxNro.EventoMuestra newmuestra = new ResultadoxNro.EventoMuestra
            {
                adecuada = true,
                aislamiento = false,
                fechaToma = ftoma, // "2020-08-23",
                idEstablecimientoToma = int.Parse(idestablecimientotoma),  // 140618, // sacar del efector  solicitante
                idEventoCaso = protocolo.IdCasoSISA, // 2061287,
                idMuestra = 272,
                idtipoMuestra = 4,
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
                        Item oItem = new Item();
                        oItem = (Item)oItem.Get(typeof(Item), "Codigo", oCon.CodigoCovid, "Baja", false);

                        //string trajomuestra = fila[3].ToString();
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                        crit.Add(Expression.Eq("IdProtocolo", protocolo));
                        crit.Add(Expression.Eq("IdItem", oItem));
                        IList listadetalle = crit.List();
                        foreach (DetalleProtocolo oDetalle in listadetalle)
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


        private void GenerarResultadoSISA(DetalleProtocolo oDetalle)

        {
            bool generacaso = false;
            int ideventomuestra = oDetalle.IdeventomuestraSISA;
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLResultadoSISA;


            try
            {
                int id_resultado_a_informar = 0;
                int idevento = 307; // sospechoso

                if (oDetalle.IdProtocolo.IdCaracter == 4) idevento = 309; // contacto
                // nose notificò antes y es sospechoso o contacto

                string res = oDetalle.ResultadoCar;


                if (res.Length > 10)
                {
                    if (res.Substring(0, 10) == "SE DETECTA")
                    { id_resultado_a_informar = 3; }

                    if (res.Substring(0, 13) == "NO SE DETECTA")
                    { id_resultado_a_informar = 4; }

                } // if res 

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
                        idPrueba = 1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = 727, // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
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
                        generacaso = true;
                    }

                }


            }
            catch (WebException ex)
            {
                string mensaje = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                generacaso = false;

            }


        }


        private void SubirSISAdesdeAPI()
        {
            int i = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {

                   string res = gvLista.Rows[row.RowIndex].Cells[6].Text;
                //string idderivacion = gvLista.Rows[row.RowIndex].Cells[4].Text;
                string ideventomuestra = gvLista.Rows[row.RowIndex].Cells[1].Text;
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
                    oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    if (oDetalleProtocolo != null)
                    {
                       
                             
                            if (res.Length > 10)
                            {
                                if (res.Substring(0, 10) == "SE DETECTA")  
                                { if (ProcesaSISA(oDetalleProtocolo, "SE DETECTA")) i = i + 1; }

                                if (res.Substring(0, 13) == "NO SE DETECTA")
                                { if (ProcesaSISA(oDetalleProtocolo, "NO SE DETECTA")) i = i + 1; }
                            }
                           
                        
                        BorrarLineaTemporalesdesdeAPI(ideventomuestra);
                  }
                   

                }//checked
            }// grid


            CargarGrilla();
            estatus.Text = "se han informado " + i.ToString() + " resultados";
            btnDescargarExcelControl.Visible = true;
            //GenerarResultadoSISA(  );

        }
        private void SubirSISA()
        {
            int i = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {

            //    string res = gvLista.Rows[row.RowIndex].Cells[6].Text;
                string idderivacion = gvLista.Rows[row.RowIndex].Cells[4].Text;
                string ideventomuestra = gvLista.Rows[row.RowIndex].Cells[5].Text;
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    DetalleProtocolo oDetalleProtocolo = new DetalleProtocolo();
                    oDetalleProtocolo = (DetalleProtocolo)oDetalleProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
                    if (GenerarResultadoSISA(oDetalleProtocolo, idderivacion, ideventomuestra))
                    {
                        i = i + 1;
                      //  oDetalleProtocolo.IdProtocolo.GrabarAuditoriaDetalleProtocolo(" Resultado a SISA", int.Parse(Session["idUsuario"].ToString()), "", oDetalleProtocolo.ResultadoCar);
                        BorrarLineaTemporales(idderivacion, ideventomuestra);
                    }


                }//checked
            }// grid


            CargarGrilla();
            estatus.Text = "se han informado " + i.ToString() + " resultados";
            //GenerarResultadoSISA(  );

        }

         private bool GenerarResultadoSISA(DetalleProtocolo oDetalle,  string idderivacion, string ideventomuestra)
     
        {
            bool generacaso = false;

            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string URL = oCon.URLResultadoSISA;



            try
            {
                int id_resultado_a_informar = 0;

                // nose notificò antes y es sospechoso o contacto
             
                int idevento = 307; // sospechoso

                if (oDetalle.IdProtocolo.IdCaracter == 4) idevento = 309; // contacto
                // nose notificò antes y es sospechoso o contacto


                string res = oDetalle.ResultadoCar;


                if (res.Length > 10)
                {
                    if (res.Substring(0, 10) == "SE DETECTA")
                    { id_resultado_a_informar = 3; }

                    if (res.Substring(0, 13) == "NO SE DETECTA")
                    { id_resultado_a_informar = 4; }

                } // if res 

                if (id_resultado_a_informar != 0)
                {
                     string femision = oDetalle.FechaValida.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

                    string frecepcion = oDetalle.IdProtocolo.Fecha.ToString("yyyy-MM-dd");//ToShortDateString("yyyy/MM/dd").Replace("/", "-");


                  



                    resultado newresultado = new resultado
                    { // resultado de dni: 31935346
                        derivada = true,
                        fechaEmisionResultado =  femision, //"2020-09-14", //
                        fechaRecepcion = frecepcion, // "2020-09-13" 
                        idDerivacion = int.Parse(idderivacion), //1125675,//
                        idEstablecimiento = 107093,  //int.Parse( s_idestablecimiento), //prod: "51580352167442",
                        idEvento = idevento, // sospechoso: 307 y 309 contacto.. idem a la tabla de configuracion sisa
                        idEventoMuestra = int.Parse(ideventomuestra),  // 2131682, // sale del excel
                        idPrueba = 1076,  // RT-PCR en tiempo real para agregar en la tabla de configuracion sisa
                        idResultado = id_resultado_a_informar,// 4, // 4: no detectable; 3: detectable
                        idTipoPrueba = 727, // Genoma viral SARS-CoV-2  para agregar en la tabla de configuracion sisa
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
                    string a_apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("8482d41353ecd747c271f2ec869345e4"));
                    string a_apiId = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("0e4fcbbf"));
                    request.Headers.Add("app_key", "8482d41353ecd747c271f2ec869345e4");
                    request.Headers.Add("app_id", "0e4fcbbf");



                    Stream postStream = request.GetRequestStream();
                    postStream.Write(data, 0, data.Length);

                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string body = reader.ReadToEnd();
                    if (body != "")
                    {
                        oDetalle.IdeventomuestraSISA = int.Parse(ideventomuestra);
                        oDetalle.Save();

                        oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Muestra SISA " + ideventomuestra,int.Parse( Session["idUsuario"].ToString()));



                        oDetalle.GrabarAuditoriaDetalleProtocolo("Genera Resultado en SISA", int.Parse(Session["idUsuario"].ToString()));
                        generacaso = true;
                    }

                }
                 

            }
            catch (Exception e)
            {
                generacaso = false;
                    


                //lblError.Visible = true;
                //btnSalir.Visible = true;
            }
            return generacaso;

        }


    

        private void BorrarResultadosLuminex(int nroProtocolo)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
            crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (ProtocoloLuminex oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }

        }
         
        public class resultado
        {
            public Boolean derivada { get; set; }
            public string fechaEmisionResultado { get; set; }
            public string fechaRecepcion { get; set; }
            public int? idDerivacion { get; set; }
            public int idEstablecimiento { get; set; }
            public int idEvento { get; set; }

            public int idEventoMuestra { get; set; }
            public int idPrueba { get; set; }
            public int idResultado { get; set; }
            public int idTipoPrueba { get; set; }
            public Boolean noApta { get; set; }
            public string valor { get; set; }


        }

        protected void btnDescargarExcelControl_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }
        private DataTable MostrarDatos()
        {
            DateTime fecha1 = DateTime.Now;
           

            string m_strSQL = @" select P.numero, P.fecha, pa.numeroDocumento as dni,  pa.apellido, pa.nombre, d.resultadoCar from 
LAB_AuditoriaProtocolo A
inner join LAB_Protocolo P on P.idProtocolo= A.idProtocolo
inner join Sys_Paciente pa on pa.idPaciente= P.idPaciente
inner join LAB_DetalleProtocolo d on d.idProtocolo= P.idProtocolo
 where A.idUsuario="+ Session["idUsuario"].ToString() +"  and A.fecha>='"+fecha1.ToString("yyyyMMdd")+"' and accion='Genera Resultado en SISA'  ";           


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
          
            return Ds.Tables[0];
        }
        private void ExportarExcel()
        {
            try
            {
                DataTable tabla = MostrarDatos();
                if (tabla.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    StringWriter sw = new StringWriter(sb);
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    Page pagina = new Page();
                    HtmlForm form = new HtmlForm();
                    GridView dg = new GridView();
                    dg.EnableViewState = false;
                    dg.DataSource = tabla;
                
                    dg.DataBind();
                    pagina.EnableEventValidation = false;
                    pagina.DesignerInitialize();
                    pagina.Controls.Add(form);
                    form.Controls.Add(dg);
                    pagina.RenderControl(htw);
                    Response.Clear();
                    Response.Buffer = true;
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=ControlActualizacionSISA_" + DateTime.Now.ToShortDateString() + ".xls");
                    Response.Charset = "UTF-8";
                    Response.ContentEncoding = Encoding.Default;
                    Response.Write(sb.ToString());
                    Response.End();
                }
            }

            catch
            {

               
            }




        }
    }
}