using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using System.Configuration;
using Business.Data.AutoAnalizador;
using System.Data.SqlClient;
using NHibernate.Expression;
using System.Collections;
using Business.Data;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace ImprimeLocal
{
    public partial class Form1MultiEfector : Form
    {
        delegate void SetTextCallback(string text);
        public Form1MultiEfector()
        {
            InitializeComponent();

            foreach (String strPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)

            {
                comboBoximp.Items.Add(strPrinter);

            }
            string miValor = ConfigurationManager.AppSettings["impresora"];
            string cantidadLaboratorio = ConfigurationManager.AppSettings["cantidadLaboratorio"];
            string cantidadMicrobiologia = ConfigurationManager.AppSettings["cantidadMicrobiologia"];
            string cantidadNoPaciente = ConfigurationManager.AppSettings["cantidadNoPaciente"];
            string cantidadForense = ConfigurationManager.AppSettings["cantidadForense"];
            lblIdEfector.Text = ConfigurationManager.AppSettings["idEfector"].ToString();


            comboBoximp.Text = miValor;
            nupLaboratorio.Value = decimal.Parse(cantidadLaboratorio);
            nupMicrobiologia.Value = decimal.Parse(cantidadMicrobiologia);
            nupNoPaciente.Value = decimal.Parse(cantidadNoPaciente);
            nupForense.Value = decimal.Parse(cantidadForense);


            if (chkModoAutomatico.Checked)
            {
                SetText("Iniciando proceso");
                //label11.Visible = true;
                //label10.Visible = true;
                //   BorrarColaImpresion();
                textBox1.Enabled = false;
                btnImprimir.Enabled = false;
                timer1.Enabled = true;
                timer1.Interval = 5000; // 5 segundos
                timer1.Start();
            }
            //    chkModoAutomatico.Checked = true;
            /*

            textBox1.Text = buscarUltimoProtocolo(int.Parse(lblIdEfector.Text));

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["numeroProtocolo"].Value = textBox1.Text;
            config.Save(ConfigurationSaveMode.Modified);

            Utility oUtil = new Utility();
            string m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            ddlEfector.DisplayMember = "nombre";
            ddlEfector.ValueMember = "idEfector";
            
            ddlEfector.DataSource = ds.Tables["t"];
            //ddlEfector.DataBind();
            da.Dispose();
            ds.Dispose();
            */


        }

        private string buscarUltimoProtocolo(int i_idEfector)
        {
                 Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                int numerito = oP.GenerarNumeroMultiEfector(i_idEfector) - 1;

                return numerito.ToString();

       


        }

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (txtmensajes.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else txtmensajes.Text = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + text; ;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label10.Text = DateTime.Now.ToLongTimeString();
            //   BuscarProtocolo2();
            //  BuscarProtocoloADemanda();


            BuscarProtocoloconAPI(); //CARO

        }

        private void BuscarProtocoloconAPI()
        {
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol =
                //  System.Net.SecurityProtocolType.Ssl3;
                SetText("buscando etiquetas pendientes de imprimir");
                //   TraerEtiquetasAPI();
                ////llamar al SP :LAB_GetGeneraEtiqueta con parametros de entrada idEfector e Impresora y de salida idProtocolo
                /// cuando esta la tabla con los datos de la etiqueta llamar a una consulta GetEtiquetaaImprimir con idParametro idProtocolo de la llamada anterior
                string s_impre = this.comboBoximp.Text;

                string URL = ConfigurationManager.AppSettings["urlAPILaboratorio"].ToString();
                URL = URL + "executeSP?nombre=LAB_GetGeneraEtiqueta&parametros=" + lblIdEfector.Text + "|" + s_impre;



                string s_token = ConfigurationManager.AppSettings["tokenAPI"].ToString();


                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                HttpWebRequest request;                           
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.Timeout = -1;// 10 * 1000;
                request.Method = "GET";// "GET";
              
                //request.ContentLength = 0;
                //request.KeepAlive = true;
                request.ContentType = "application/json";

                request.Headers.Add("Authorization", "Bearer "+s_token);
                //("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKV1RTZXJ2aWNlc0FjY2Vzc1Rva2VuIiwianRpIjoiNWIzYWI1ZTYtOWY0YS00ZWQ4LWE2ZGMtYzVkYzUyM2Q5NTUxIiwiaWF0IjoiMDQvMDMvMjAyNCAyMjoyMjoxMyIsInVzdWFyaW8iOiI5NTA5NjkyMyIsImV4cCI6MjAyNTEyMzczMywiaXNzIjoid3d3LnNhbHVkbnFuLmdvYi5hciIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NzExOC8ifQ.soeKWGRfiytiABg2rKKPY1r-SXZ0CjrYoWaY2cFfM8I");
             
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(response.GetResponseStream());
                string body = reader.ReadToEnd();
                response.Close();
                body = body.Replace("[", "").Replace("]", "");
                if (body != "")
                {
                    SetText("encontró etiquetas pendientes de imprimir");
                    TraerEtiquetasAPI();
                }
                else
                    SetText("NO encontró etiquetas pendientes de imprimir");

                //else
                //  return "no enontrado";
            }
            catch (WebException ex)
            {
                SetText("Error al conectarse a " + ex.Message.ToString());

            }
        }

        private void TraerEtiquetasAPI()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol =
           System.Net.SecurityProtocolType.Tls12;

                string URL = ConfigurationManager.AppSettings["urlAPILaboratorio"].ToString();
                URL = URL + "GetDBdata?nombre=GetEtiquetaImpresa&parametros=idEfector:" + lblIdEfector.Text;
                string s_token = ConfigurationManager.AppSettings["tokenAPI"].ToString();
                //    string s_token = ConfigurationManager.AppSettings["tokenffeeandes"].ToString();


                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

                HttpWebRequest request;
                request = WebRequest.Create(URL) as HttpWebRequest;
                request.Timeout = 10 * 1000;
                request.Method = "GET";

                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + s_token);


                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(response.GetResponseStream());


                string s = reader.ReadToEnd();
                if (s != "0")
                {

                    //List<ProtocoloEdit2.ProfesionalMatriculado> pro = jsonSerializer.Deserialize<List<ProtocoloEdit2.ProfesionalMatriculado>>(s);

                    DataTable t = GetJSONToDataTableUsingMethod(s);
                 
                    foreach (DataRow item in t.Rows)
                    {
                      
                        Business.Etiqueta ticket = new Business.Etiqueta();

                        ///Desde acá impresion de archivos
                        string reg_area = item[4].ToString();
                        string reg_numero = item[3].ToString();

                       
                       //     reg_area = oMuestra.Nombre; //item[3].ToString();
                        

                        string reg_Fecha =  item[5].ToString(); // .Subsring(0, 10);
                                                                           //string dia = reg_Fecha.Substring(0, 2);
                                                                           //string mes = reg_Fecha.Substring(3, 2);
                                                                           //string anio = reg_Fecha.Substring(8, 2);
                                                                           //reg_Fecha = dia + "-" + mes + "-" + anio;
                        string reg_Origen = item[6].ToString();
                        string reg_Sector = item[7].ToString();
                        string reg_NumeroOrigen = item[8].ToString();
                        string reg_NumeroDocumento = item[9].ToString();
                        string reg_apellido = item[10].ToString();
                        string reg_sexo = item[11].ToString();
                        string reg_edad = item[12].ToString();
                        string reg_FIS = item[14].ToString();
                        string reg_numeroOrigen2 = reg_FIS;
                        string sTipoEtiqueta = item[15].ToString();
                        string sFuenteBarCode = item[16].ToString();

                        ticket.TipoEtiqueta = sTipoEtiqueta;
                        if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);


                        ticket.AddHeaderLine(reg_apellido.ToUpper());
                        ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha + " " + reg_FIS);
                       /* if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector))
                        {
                            if (reg_numeroOrigen2 == "")
                                ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);
                            else
                                ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen + "/HIS:" + reg_numeroOrigen2);
                        }*/

                        if (reg_area != "")
                        {
                            if (reg_numeroOrigen2 == "")
                                ticket.AddSubHeaderLineNegrita(reg_area + " - " + reg_NumeroOrigen);
                            else

                                ticket.AddSubHeaderLineNegrita(reg_area + " - " + reg_NumeroOrigen + "/HIS:" + reg_numeroOrigen2);
                        }
                        //ticket.AddSubHeaderLine(reg_area);

                        ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
                        ticket.AddFooterLine(reg_numero);
                        

                        System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        string equipo = comboBoximp.Text;   //config.AppSettings.Settings["impresora"].Value;


                        ticket.PrintTicket2(equipo, sFuenteBarCode, sTipoEtiqueta);
                        SetText("Etiqueta " + reg_numero + " enviada a impresora");
                    }
                }
              
            }
            catch (Exception EX)
            {
              //  SetText("Sin etiquetas para imprimir");
            }
        }

        public static DataTable GetJSONToDataTableUsingMethod(string JSONData)
        {
            DataTable dtUsingMethodReturn = new DataTable();
            string[] jsonStringArray = Regex.Split(JSONData.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx).Replace("\"", "").Trim();
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dtUsingMethodReturn.Columns.Add(AddColumnName);
            }
            foreach (string strJSONarr in jsonStringArray)
            {
                string[] RowData = Regex.Split(strJSONarr.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dtUsingMethodReturn.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx).Replace("\"", "").Trim();
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dtUsingMethodReturn.Rows.Add(nr);
            }
            return dtUsingMethodReturn;
        }

        public class FormatoEtiqueta
        {
            public int idProtocoloEtiquetaImpresa { get; set; }
            public int idProtocolo { get; set; }
            public int idArea { get; set; }

            public string numeroP { get; set; }
            public string area { get; set; }

            public string fecha { get; set; }
            public string Origen { get; set; }
            public string Sector { get; set; }
            public string NumeroOrigen { get; set; }
            public string NumeroDocumento { get; set; }

            public string Apellido { get; set; } // feCHA DE ORDEN
            public string Sexo { get; set; } // EFECTOR
            public string edad { get; set; } // TELEFONO PACIENTE

            public string pacientecodificado { get; set; } // DIRECCION PACIENTE

            public string NumeroOrigen2 { get; set; } // localidadresidencia PACIENTE

            public string tipoEtiqueta { get; set; } // TELEFONO PACIENTE

            public string FuenteBarCode { get; set; } // TELEFONO PACIENTE

            




        }




        //       private void BuscarProtocolo2()
        //       {
        //           // impresion en forma automatica
        //           try
        //               {
        //           Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //               string s_impre = this.comboBoximp.Text;
        //               int numeroguardado = int.Parse(config.AppSettings.Settings["numeroProtocolo"].Value);

        //               int numeroobtenido = int.Parse(config.AppSettings.Settings["numeroProtocolo"].Value)  + 1;
        //textBox1.Text = numeroobtenido.ToString();
        //           Protocolo oProt = new Protocolo();
        //               oProt = (Protocolo)oProt.Get(typeof(Protocolo), "Numero", numeroobtenido);
        //           if (oProt != null)
        //           {
        //                   if (oProt.Impres.Trim() == s_impre.Trim()) // si la impresora coincide con la del protocolo se imprime 
        //                   {
        //                       decimal cantEtiquetas = 1;
        //                       if (oProt.IdPaciente.IdPaciente == -1)
        //                       {
        //                           //if (oProt.IdTipoServicio.IdTipoServicio == 7) // bro
        //                           //    ImprimirCodigoBarrasBromatologia(oProt);
        //                           //else
        //                           //{
        //                               if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
        //                               if (oProt.IdTipoServicio.IdTipoServicio == 5) cantEtiquetas = decimal.Parse(nupNoPaciente.Value.ToString());

        //                               for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
        //                               { ImprimirCodigoBarrasNoPaciente(oProt); }
        //                           //}
        //                       }
        //                       else
        //                       {
        //                           if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
        //                           if (oProt.IdTipoServicio.IdTipoServicio == 3) cantEtiquetas = decimal.Parse(nupMicrobiologia.Value.ToString());
        //                           if (oProt.IdTipoServicio.IdTipoServicio == 1) cantEtiquetas = decimal.Parse(nupLaboratorio.Value.ToString());


        //                           for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
        //                           { ImprimirCodigoBarras(oProt,0); }


        //                       }                  


        //                   }
        //                   config.AppSettings.Settings["numeroProtocolo"].Value = numeroobtenido.ToString();
        //                   config.Save(ConfigurationSaveMode.Modified);
        //               }
        //           else
        //           {
        //               lblError.Text = "Numero aun no cargado.";
        //               lblError.Visible = true;
        //                   BuscarProtocoloADemanda();
        //           }
        //           }
        //           catch
        //           {
        //               timer1.Stop();
        //               chkModoAutomatico.Checked = false;
        //               lblError.Text = "Error de impresion. Comuniquese con el administrador.";
        //               lblError.Visible = true;
        //           }
        //       }



        private void BuscarProtocoloADemanda()
        {
            // impresion en forma automatica
            try
            {
                string m_strSQL = @" select  * from LAB_ProtocoloEtiqueta where idEfector= "+ lblIdEfector.Text;


                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds, "etiquetas");
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p < Ds.Tables[0].Rows.Count; p++)
                    {


                        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        string s_impre = this.comboBoximp.Text;
                        int numeroguardado = int.Parse(config.AppSettings.Settings["numeroProtocolo"].Value);

                        int idProtocolo = int.Parse(Ds.Tables[0].Rows[p][1].ToString());
                        int idAreaImprimir = int.Parse(Ds.Tables[0].Rows[p][2].ToString());
                        string impresoraProtocolo = Ds.Tables[0].Rows[p][4].ToString();
                        //textBox1.Text = numeroobtenido.ToString();
                        Protocolo oProt = new Protocolo();
                        oProt = (Protocolo)oProt.Get(typeof(Protocolo), idProtocolo);
                        if (oProt != null)
                        {
                            if (impresoraProtocolo == s_impre.Trim()) // si la impresora coincide con la del protocolo se imprime 
                            {
                                decimal cantEtiquetas = 1;
                                if (oProt.IdPaciente.IdPaciente == -1)
                                {
                                    //if (oProt.IdTipoServicio.IdTipoServicio == 7) // bro
                                    //    ImprimirCodigoBarrasBromatologia(oProt);
                                    //else
                                    //{
                                    if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
                                    if (oProt.IdTipoServicio.IdTipoServicio == 5) cantEtiquetas = decimal.Parse(nupNoPaciente.Value.ToString());

                                    for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
                                    { ImprimirCodigoBarrasNoPaciente(oProt); }
                                    //}
                                }
                                else
                                {
                                    if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
                                    if (oProt.IdTipoServicio.IdTipoServicio == 3) cantEtiquetas = decimal.Parse(nupMicrobiologia.Value.ToString());
                                    if (oProt.IdTipoServicio.IdTipoServicio == 1) cantEtiquetas = decimal.Parse(nupLaboratorio.Value.ToString());


                                    for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
                                    { ImprimirCodigoBarras(oProt, idAreaImprimir); }


                                }

                                MarcarImpreso(idProtocolo, idAreaImprimir);


                            }
                            //config.AppSettings.Settings["numeroProtocolo"].Value = numeroobtenido.ToString();
                            //config.Save(ConfigurationSaveMode.Modified);
                        }
                        else
                        {
                            lblError.Text = "Numero aun no cargado.";
                            lblError.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //timer1.Stop();
                //chkModoAutomatico.Checked = false;
                lblError.Text = ex.Message; // "Error de impresion. Comuniquese con el administrador.";
                lblError.Visible = true;
            }
        }

        private void MarcarImpreso(int idProtocolo, int idAreaImprimir)
        {

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"delete from LAB_ProtocoloEtiqueta where  idProtocolo= " + idProtocolo.ToString() + " and idArea= " + idAreaImprimir.ToString();
            SqlCommand cmd = new SqlCommand(query, conn);

             

           int idconsenti = Convert.ToInt32(cmd.ExecuteScalar());

           
        }
        //private void BorrarColaImpresion()
        //{
        //    string s_impre = this.comboBoximp.Text;

        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

        //    string query = @"delete from LAB_ProtocoloEtiqueta where idEfector="+ lblIdEfector.Text+ " and  impresora= '" + s_impre + "'";
        //    SqlCommand cmd = new SqlCommand(query, conn);



        //    int idconsenti = Convert.ToInt32(cmd.ExecuteScalar());

        //    //string m_strSQL = @" select  * from LAB_ProtocoloEtiqueta";


        //    //DataSet Ds = new DataSet();
        //    //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    //SqlDataAdapter adapter = new SqlDataAdapter();
        //    //adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    //adapter.Fill(Ds, "etiquetas");
        //    //lblError.Text = "Etiquetas por imprimir: " + Ds.Tables[0].Rows.Count.ToString(); 




        //}



        private void ImprimirCodigoBarras(Protocolo oProt, int idAreaImprimir)
        {
            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(lblIdEfector.Text));


            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oEfector);

            //   Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), 1);

            //    oProt.GrabarAuditoriaProtocolo("Imprime Código de Barras", oProt.IdUsuarioRegistro.IdUsuario);
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);


            TipoServicio oTipo = new TipoServicio(); oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), oProt.IdTipoServicio.IdTipoServicio);
            ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();
            if (oTipo.IdTipoServicio == 6)
            {
                TipoServicio oTipo3 = new TipoServicio(); oTipo3 = (TipoServicio)oTipo3.Get(typeof(TipoServicio), 3);
                oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo3, "IdEfector", oEfector);
            }
            else
                oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo, "IdEfector", oEfector);

            string s_listaAreas = getListaAreas(oTipo);
            if (idAreaImprimir != 0) 
                s_listaAreas = idAreaImprimir.ToString();
            bool adicionalGeneral = false;

            if (idAreaImprimir == -1) adicionalGeneral = true;
            //oConBarra.Fuente = "Code39";
            string sFuenteBarCode = "CCode39";
            bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
            bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
            bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
            bool imprimeProtocoloNumeroOrigen = oConBarra.ProtocoloNumeroOrigen;
            bool imprimePacienteNumeroDocumento = oConBarra.PacienteNumeroDocumento;
            bool imprimePacienteApellido = oConBarra.PacienteApellido;
            bool imprimePacienteSexo = oConBarra.PacienteSexo;
            bool imprimePacienteEdad = oConBarra.PacienteEdad;
           


        

            DataTable Dt = new DataTable();
            Dt = (DataTable)oProt.GetDataSetCodigoBarras("Protocolo", s_listaAreas, oProt.IdTipoServicio.IdTipoServicio, adicionalGeneral);
            foreach (DataRow item in Dt.Rows)
            {
                ///Desde acá impresion de archivos
                string reg_area =  item[3].ToString();
                string reg_numero = item[2].ToString();

                if (oProt.IdMuestra > 0)
                {
                    Muestra oMuestra = new Muestra();
                    oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), "IdMuestra", oProt.IdMuestra);
                    reg_area = oMuestra.Nombre; //item[3].ToString();
                }
                 
                string reg_Fecha = oProt.Fecha.ToShortDateString();//item[4].ToString(); // .Subsring(0, 10);
                //string dia = reg_Fecha.Substring(0, 2);
                //string mes = reg_Fecha.Substring(3, 2);
                //string anio = reg_Fecha.Substring(8, 2);
                //reg_Fecha = dia + "-" + mes + "-" + anio;
                string reg_Origen = item[5].ToString();
                string reg_Sector = item[6].ToString();
                string reg_NumeroOrigen = item[7].ToString();
                string reg_NumeroDocumento = oProt.IdPaciente.getNumeroImprimir();



                string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);

                string reg_apellido = "";
             
                    if (reg_codificaHIV == "FALSE")
                        reg_apellido = oProt.IdPaciente.Apellido + " " + oProt.IdPaciente.Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
                    else
                        reg_apellido = oProt.Sexo + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");

                //reg_apellido = item[12].ToString().ToUpper();
                string reg_sexo = item[10].ToString();
                string reg_edad = item[11].ToString();
                string reg_numeroOrigen2 = item[13].ToString();
               reg_numeroOrigen2=  reg_numeroOrigen2.Replace("HISOP00", "").Replace("HISOP0", "").Replace("HISOP", "");
                //tabla.Rows.Add(reg);
                //tabla.AcceptChanges();
                // AGREGO FECHA DE INICIO DE SINTOMAS Y METODO DE EXTRACCION SEGUN LOS DIAS

                string reg_FIS = "";

                if ((oProt.IdEfector.IdEfector == 228) && (oProt.IdTipoServicio.IdTipoServicio==3))
                {
                    if (oProt.FechaInicioSintomas.Year != 1900)
                    {
                        //  reg_FIS = " FIS:" + oProt.FechaInicioSintomas.ToShortDateString();
                        //  dia = reg_FIS.Substring(0, 2);
                        //  mes = reg_FIS.Substring(3, 2);
                        //  anio = reg_FIS.Substring(8, 2);
                        //reg_FIS = " FIS:" +  dia + "-" + mes + "-" + anio;

                        TimeSpan diffResult = oProt.FechaTomaMuestra - oProt.FechaInicioSintomas;
                        if (diffResult.Days <= 7)
                            reg_FIS = "***CRUDO****";
                        else
                            reg_FIS = "- MAXWELL";
                    }
                    else
                        reg_FIS = "- SIN FIS";
                    // fin calculo de FIS
               
                    // CONTROL DE ALTA SE PROCESA MAXWELL
                    if (oProt.IdCaracter==2)
                        reg_FIS = "- MAXWELL";
                }


                if (!imprimeProtocoloFecha) reg_Fecha = "          ";
                if (!imprimeProtocoloOrigen) reg_Origen = "          ";
                if (!imprimeProtocoloSector) reg_Sector = "   ";
                if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
                if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
                if (!imprimePacienteApellido) reg_apellido = "";
                if (!imprimePacienteSexo) reg_sexo = " ";
                if (!imprimePacienteEdad) reg_edad = "   ";
                //ParameterDiscreteValue fuenteCodigoBarras = new ParameterDiscreteValue(); fuenteCodigoBarras.Value = oConBarra.Fuente;


                Business.Etiqueta ticket = new Business.Etiqueta();
                ticket.TipoEtiqueta = oC.TipoEtiqueta;
                if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);


                ticket.AddHeaderLine(reg_apellido.ToUpper());
                ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha + " " + reg_FIS);
                if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector))
                { if (reg_numeroOrigen2=="")
                        ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen );
                    else
                                            ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen + "/HIS:" + reg_numeroOrigen2);
                }

                if (reg_area != "")
                {
                    if (reg_numeroOrigen2 == "")
                        ticket.AddSubHeaderLineNegrita(reg_area + " - " + reg_NumeroOrigen );
                    else

                        ticket.AddSubHeaderLineNegrita(reg_area + " - " + reg_NumeroOrigen + "/HIS:" + reg_numeroOrigen2);
                }
                //ticket.AddSubHeaderLine(reg_area);

                ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
                ticket.AddFooterLine(reg_numero);

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string equipo = comboBoximp.Text;   //config.AppSettings.Settings["impresora"].Value;
           

                ticket.PrintTicket2(equipo, sFuenteBarCode, oC.TipoEtiqueta);
                /////fin de impresion de archivos
            }

        }

        private string getListaAreas(TipoServicio oTipo)
        {
            string s_practicas = "";
            Area oDetalle = new Area();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Area));
            crit.Add(Expression.Eq("ImprimeCodigoBarra", true));
            crit.Add(Expression.Eq("IdTipoServicio", oTipo));
            

            IList items = crit.List();
            string pivot = "";
            string sDatos = "";
            foreach (Area oDet in items)
            {
                if (pivot != oDet.IdArea.ToString())
                {
                    if (sDatos == "")
                        sDatos = oDet.IdArea.ToString();
                    else
                        sDatos += ", " + oDet.IdArea.ToString();
                    //sDatos += "#" + oDet.IdItem.Codigo + "#" + oDet.IdItem.Nombre + "#" + oDet.TrajoMuestra + "@";                   
                    pivot = oDet.IdArea.ToString();
                }

            }

            s_practicas = sDatos;
            return s_practicas;
        }

        private void button1_Click(object sender, EventArgs e)
        {


            try
            {
                Protocolo oProt = new Protocolo();
                oProt = (Protocolo)oProt.Get(typeof(Protocolo), "Numero", Convert.ToInt32(textBox1.Text));

                decimal cantEtiquetas = 1;
                if (oProt.IdPaciente.IdPaciente == -1)
                {
                    //if ((oProt.IdTipoServicio.IdTipoServicio == 6) || (oProt.IdTipoServicio.IdTipoServicio == 3))
                    //{
                    if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
                    if (oProt.IdTipoServicio.IdTipoServicio == 5) cantEtiquetas = decimal.Parse(nupNoPaciente.Value.ToString());

                    for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
                    { ImprimirCodigoBarrasNoPaciente(oProt); }
                    //}
                    //if (oProt.IdTipoServicio.IdTipoServicio == 7)  // bromatologia
                    //{ ImprimirCodigoBarrasBromatologia(oProt); }
                }
                else
                {
                    if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
                    if (oProt.IdTipoServicio.IdTipoServicio == 3) cantEtiquetas = decimal.Parse(nupMicrobiologia.Value.ToString());
                    if (oProt.IdTipoServicio.IdTipoServicio == 1) cantEtiquetas = decimal.Parse(nupLaboratorio.Value.ToString());



                    for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
                    { ImprimirCodigoBarras(oProt, 0); }

                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: verifique numero ingresado incorrecto o comuniquese con el administrador del sistema";
                lblError.Visible = true;
            }

        }
//        private void ImprimirCodigoBarrasBromatologia(Protocolo oProt)
//        {
//            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), 1);

//            //    oProt.GrabarAuditoriaProtocolo("Imprime Código de Barras", oProt.IdUsuarioRegistro.IdUsuario);
//            TipoServicio oTipo = new TipoServicio(); oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 7);

//            ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();
//            oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo);

//            string s_listaAreas = getListaAreas(oTipo);
//            //oConBarra.Fuente = "Code39";
//            string sFuenteBarCode = "CCode39";
//            bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
//            bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
//            bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
//            bool imprimeProtocoloNumeroOrigen = oConBarra.ProtocoloNumeroOrigen;
//            bool imprimePacienteNumeroDocumento = oConBarra.PacienteNumeroDocumento;
//            bool imprimePacienteApellido = oConBarra.PacienteApellido;
//            bool imprimePacienteSexo = oConBarra.PacienteSexo;
//            bool imprimePacienteEdad = oConBarra.PacienteEdad;
//            bool adicionalGeneral = false;
//            //if (nupNoPaciente.Value == 2) adicionalGeneral = true;
//            //    if (s_listaAreas.Substring(0, 1) == "0") adicionalGeneral = true;

//            DataTable Dt = new DataTable();
//            Dt =  GetDataSetCodigoBarrasBromatologia( oProt.IdProtocolo, s_listaAreas, 3, adicionalGeneral);





//            foreach (DataRow item in Dt.Rows)
//            {
//                ///Desde acá impresion de archivos
//                string reg_numero = item[2].ToString();

                
//                string reg_area = item[2].ToString();
//                string s_conservacion = "";
               

//                string reg_Fecha = item[4].ToString();//.Substring(0, 10);
//                string reg_Origen = item[5].ToString();
//                string reg_Sector = item[6].ToString();
//                string reg_NumeroOrigen = item[7].ToString();
//                string reg_NumeroDocumento = oProt.NumeroOrigen.ToString();



               

//                string reg_apellido = oProt.DescripcionProducto;
                 
//                //tabla.Rows.Add(reg);
//                //tabla.AcceptChanges();


//                if (!imprimeProtocoloFecha) reg_Fecha = "          ";
//                if (!imprimeProtocoloOrigen) reg_Origen = "          ";
//                if (!imprimeProtocoloSector) reg_Sector = "   ";
//                if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
//                if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
//                if (!imprimePacienteApellido) reg_apellido = "";
               
//                //ParameterDiscreteValue fuenteCodigoBarras = new ParameterDiscreteValue(); fuenteCodigoBarras.Value = oConBarra.Fuente;


//                Business.Etiqueta ticket = new Business.Etiqueta();
//                ticket.TipoEtiqueta = oC.TipoEtiqueta;
//                if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);


//                ticket.AddHeaderLine(reg_apellido.ToUpper());
//                ticket.AddSubHeaderLine(  reg_NumeroDocumento + " " + reg_Fecha.Substring(0, 10));
//                if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector)) ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);
//                if (reg_area != "") ticket.AddSubHeaderLineNegrita(reg_area + "-" + s_conservacion);
//                //ticket.AddSubHeaderLine(reg_area);

//                ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
//                ticket.AddFooterLine(reg_numero); // + "  " + reg_NumeroOrigen);

//                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
//                string equipo = comboBoximp.Text;   //config.AppSettings.Settings["impresora"].Value;

//                ticket.PrintTicket2(equipo, sFuenteBarCode, oC.TipoEtiqueta);

//                /////fin de impresion de archivos
//            }

//        }

//        private DataTable GetDataSetCodigoBarrasBromatologia(int idProtocolo, string s_listaAreas, int v, bool adicionalGeneral)
//        {
//            string m_strSQL = @" SELECT DISTINCT 
//                      P.idProtocolo, I.idArea,  P.numero AS numeroP, P.idEfectorSolicitante, P.idOrigen, P.idPrioridad, P.idSector, P.idTipoServicio, 
//                      CASE WHEN I.etiquetaAdicional = 1 THEN substring (I.nombre,0,20) ELSE substring(A.nombre,0,20) END AS area, P.fecha,   P.numeroOrigen, P.numeroOrigen2,
//                      0 as numeroDocumento, I.codificaHiv as apellido , P.sexo, CONVERT(varchar, P.edad) 
//                      + '' + CASE P.unidadEdad WHEN 0 THEN 'a' WHEN 1 THEN 'm' WHEN 2 THEN 'd' END AS edad, DP.conResultado, P.numero, P.numeroDiario, P.numeroTipoServicio, 
//                      P.numeroSector, '' AS pacientecodificado,  P.idMuestra,
//					  C.idCaso as NumeroInforme, P.numeroControl as numeroControl, C.Elaborador, M.descripcion as motivo
//FROM     
//dbo.LAB_Protocolo AS P INNER JOIN
//BRO_CasoProtocolo Cp on cp.idProtocolo= P.idprotocolo  inner join
//BRO_Caso C on c.idCaso= cp.idCaso inner join
//LAB_MotivoAnalisis as M on M.idMotivoAnalisis= C.idMotivoAnalisis inner join

//                      dbo.LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo INNER JOIN
//                      dbo.LAB_Item AS I ON DP.idSubItem = I.idItem INNER JOIN
//                      dbo.LAB_Area AS A ON I.idArea = A.idArea  
//Where P.baja=0 and P.idprotocolo="+ idProtocolo.ToString();


//            DataSet Ds = new DataSet();
//            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
//            adapter.Fill(Ds, "resultado");


//            DataTable data = Ds.Tables[0];
//            return data;
//        }

        private void ImprimirCodigoBarrasNoPaciente(Protocolo oProt)
        {
            Efector oEfector = new Efector();

            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(lblIdEfector.Text));


            Configuracion oC = new Configuracion();
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oEfector);
            //  Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), 1);

            //    oProt.GrabarAuditoriaProtocolo("Imprime Código de Barras", oProt.IdUsuarioRegistro.IdUsuario);
            TipoServicio  oTipo = new TipoServicio(); oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 3);

            ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();
            oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo, "IdEfector", oEfector);

            string s_listaAreas = getListaAreas(oTipo);
            //oConBarra.Fuente = "Code39";
            string sFuenteBarCode = "CCode39";
            bool imprimeProtocoloFecha = oConBarra.ProtocoloFecha;
            bool imprimeProtocoloOrigen = oConBarra.ProtocoloOrigen;
            bool imprimeProtocoloSector = oConBarra.ProtocoloSector;
            bool imprimeProtocoloNumeroOrigen = oConBarra.ProtocoloNumeroOrigen;
            bool imprimePacienteNumeroDocumento = oConBarra.PacienteNumeroDocumento;
            bool imprimePacienteApellido = oConBarra.PacienteApellido;
            bool imprimePacienteSexo = oConBarra.PacienteSexo;
            bool imprimePacienteEdad = oConBarra.PacienteEdad;
            bool adicionalGeneral = false;
            //if (nupNoPaciente.Value == 2) adicionalGeneral = true;
            //    if (s_listaAreas.Substring(0, 1) == "0") adicionalGeneral = true;

            DataTable Dt = new DataTable();
            Dt = (DataTable)oProt.GetDataSetCodigoBarras("Protocolo", s_listaAreas, 3, adicionalGeneral);

          



            foreach (DataRow item in Dt.Rows)
            {
                ///Desde acá impresion de archivos
                string reg_numero = item[2].ToString();

                Muestra oMuestra = new Muestra();
                oMuestra = (Muestra)oMuestra.Get(typeof(Muestra), "IdMuestra", oProt.IdMuestra);
                string reg_area = oMuestra.Nombre ; //item[3].ToString();
                string s_conservacion = "";
                if (oProt.IdConservacion > 0)
                {
                    Conservacion oConservacion = new Conservacion();
                    oConservacion = (Conservacion)oConservacion.Get(typeof(Conservacion), "IdConservacion", oProt.IdConservacion);

                      s_conservacion = oConservacion.Descripcion;
                }

                string reg_Fecha = item[4].ToString();//.Substring(0, 10);
                string reg_Origen = item[5].ToString();
                string reg_Sector = item[6].ToString();
                string reg_NumeroOrigen = item[7].ToString();
                string reg_NumeroDocumento = oProt.NumeroOrigen.ToString();



                string reg_codificaHIV = item[9].ToString().ToUpper(); //.Substring(0,32-reg_NumeroOrigen.Length);

                string reg_apellido = oProt.DescripcionProducto;
                 if (reg_apellido.Length>30)

                reg_apellido = reg_apellido.Substring(0,30) ; //.IdMuestra.IdPaciente.Apellido + " " + oProt.IdPaciente.Nombre;//  .Substring(0,20); SUBSTRING(Pac.apellido + ' ' + Pac.nombre, 0, 20) ELSE upper(P.sexo + substring(Pac.nombre, 1, 2) 
                
                string reg_sexo = item[10].ToString();
                string reg_edad = item[11].ToString();
                //tabla.Rows.Add(reg);
                //tabla.AcceptChanges();


                if (!imprimeProtocoloFecha) reg_Fecha = "          ";
                if (!imprimeProtocoloOrigen) reg_Origen = "          ";
                if (!imprimeProtocoloSector) reg_Sector = "   ";
                if (!imprimeProtocoloNumeroOrigen) reg_NumeroOrigen = "     ";
                if (!imprimePacienteNumeroDocumento) reg_NumeroDocumento = "        ";
                if (!imprimePacienteApellido) reg_apellido = "";
                if (!imprimePacienteSexo) reg_sexo = " ";
                if (!imprimePacienteEdad) reg_edad = "   ";
                //ParameterDiscreteValue fuenteCodigoBarras = new ParameterDiscreteValue(); fuenteCodigoBarras.Value = oConBarra.Fuente;


                Business.Etiqueta ticket = new Business.Etiqueta();
                ticket.TipoEtiqueta = oC.TipoEtiqueta;
                if (reg_Origen.Length > 11) reg_Origen = reg_Origen.Substring(0, 10);


                ticket.AddHeaderLine(reg_apellido.ToUpper());
                ticket.AddSubHeaderLine(reg_sexo + " " + reg_edad + " " + reg_NumeroDocumento + " " + reg_Fecha.Substring(0,10));
                if ((imprimeProtocoloOrigen) || (imprimeProtocoloSector)) ticket.AddSubHeaderLine(reg_Origen + "  " + reg_NumeroOrigen);
                if (reg_area != "") ticket.AddSubHeaderLineNegrita(reg_area  +"-" + s_conservacion);
                //ticket.AddSubHeaderLine(reg_area);

                ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
                ticket.AddFooterLine(reg_numero); // + "  " + reg_NumeroOrigen);

                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                string equipo = comboBoximp.Text;   //config.AppSettings.Settings["impresora"].Value;
             
                ticket.PrintTicket2(equipo, sFuenteBarCode, oC.TipoEtiqueta);
               
                /////fin de impresion de archivos
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ini= int.Parse(txtDesde.Text);
            int fin= int.Parse(txtHasta.Text);
            for (int i= ini; i<= fin; i++)
            { 
            MindrayResultado oProtocolo = new MindrayResultado();
            oProtocolo = (MindrayResultado)oProtocolo.Get(typeof(MindrayResultado), "Protocolo", i);
                if (oProtocolo!=null)
            ImprimirEtiquetaSeroteca(oProtocolo.Protocolo, oProtocolo.FechaProtocolo, oProtocolo.Descripcion, oProtocolo.UnidadMedida, oProtocolo.ValorObtenido);
            }
        }

        private void ImprimirEtiquetaSeroteca(string numero, DateTime fechatranfusion, string apellido, string dni, string tranfusion)
        {
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
            Business.Etiqueta ticket = new Business.Etiqueta();
            ticket.TipoEtiqueta = "5x2.5";

            string sFuenteBarCode = "Code39";

            ticket.AddHeaderLine(numero + "    " + dni);
            ticket.AddSubHeaderLine(apellido.ToUpper());
            if (fechatranfusion.ToShortDateString() != "01/01/1900")
                ticket.AddSubHeaderLine(fechatranfusion.ToShortDateString() + "    " + tranfusion);
            else
                ticket.AddSubHeaderLine("");
            ticket.AddSubHeaderLineNegrita("");
            ticket.AddSubHeaderLine("");

            //// falta pasar por parametro la fuente de codigo de barras
            ticket.AddCodigoBarras("", "");
            ticket.AddFooterLine("");

            //TipoServicio oTipoServicio = new TipoServicio();
            //oTipoServicio = (TipoServicio)oTipoServicio.Get(typeof(TipoServicio), 3);

           ticket.PrintTicket(comboBoximp.Text , sFuenteBarCode);
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void chkImpresora_CheckedChanged(object sender, EventArgs e)
        {
            if (chkImpresora.Checked)
            { Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["impresora"].Value = comboBoximp.Text;
config.Save(ConfigurationSaveMode.Modified);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["cantidadLaboratorio"].Value = nupLaboratorio.Value.ToString();
            config.AppSettings.Settings["cantidadMicrobiologia"].Value = nupMicrobiologia.Value.ToString();
                config.AppSettings.Settings["cantidadNoPaciente"].Value = nupNoPaciente.Value.ToString();
                config.AppSettings.Settings["cantidadForense"].Value = nupForense.Value.ToString();
                config.Save(ConfigurationSaveMode.Modified);

           

        }

        private void chkModoAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            if (chkModoAutomatico.Checked)
            {
                //Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    BuscarProtocoloconAPI();

                //textBox1.Text = buscarUltimoProtocolo(int.Parse(lblIdEfector.Text));
                //config.AppSettings.Settings["numeroProtocolo"].Value = textBox1.Text;
                //config.Save(ConfigurationSaveMode.Modified);
          SetText("Iniciando proceso");
                //label11.Visible = true;
                //label10.Visible = true;
             //   BorrarColaImpresion();
                textBox1.Enabled = false;
                btnImprimir.Enabled = false;
                timer1.Enabled = true;
                timer1.Interval = 5000; // 5 segundos
                timer1.Start();
                //Borrar cola de impresion
              
                //InitTimer();
            }
            else
            {
                label11.Visible = false;
                label10.Visible = false;
                textBox1.Text = "";
                textBox1.Enabled = true;
                btnImprimir.Enabled = true;
                timer1.Enabled = false;
               


            }
        }

        private void btnImprimirEfector_Click(object sender, EventArgs e)
        {
            if (ddlEfector.SelectedValue.ToString() != "0")
            {
                if (txtDesdeEfector.Text != "")
                {
                    ImprimirLoteEfector();
                }
                else
                {
                    lblError.Text = "Debe ingresar un numero desde";
                    lblError.Visible = true;
                }
            }
            else
            {
                lblError.Text = "Debe seleccionar un efector";
                lblError.Visible = true;
            }
           
        }

        private void ImprimirLoteEfector()
        {
            Utility oUtil = new Utility();
            string m_ssql = @"select  idprotocolo 
from lab_protocolo p
inner join sys_paciente pa on pa.idpaciente = p.idpaciente
where p.baja=0 and p.estado=0 and p.idEfectorsolicitante = " + ddlEfector.SelectedValue.ToString() + @" AND p.numero >=" + txtDesdeEfector.Text+@" order by  pa.apellido ";

            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(m_ssql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");

            if (ds.Tables["t"].Rows.Count == 0)
            {
                lblError.Text = "No se encontraron datos para el efector y numero ingresado";
                lblError.Visible = true;
            }
            for (int m = 0; m < ds.Tables["t"].Rows.Count; m++)
            {
                Protocolo oProt = new Protocolo();
                oProt = (Protocolo)oProt.Get(typeof(Protocolo), int.Parse(ds.Tables["t"].Rows[m][0].ToString()));
                if (oProt != null)
                {

                    decimal cantEtiquetas = 1;
                    if (oProt.IdPaciente.IdPaciente == -1)
                    {
                        if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
                        if (oProt.IdTipoServicio.IdTipoServicio == 3) cantEtiquetas = decimal.Parse(nupNoPaciente.Value.ToString());

                        for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
                        { ImprimirCodigoBarrasNoPaciente(oProt); }
                    }
                    else
                    {
                        if (oProt.IdTipoServicio.IdTipoServicio == 6) cantEtiquetas = decimal.Parse(nupForense.Value.ToString());
                        if (oProt.IdTipoServicio.IdTipoServicio == 3) cantEtiquetas = decimal.Parse(nupMicrobiologia.Value.ToString());
                        if (oProt.IdTipoServicio.IdTipoServicio == 1) cantEtiquetas = decimal.Parse(nupLaboratorio.Value.ToString());


                        for (int i = 1; i <= decimal.ToInt32(cantEtiquetas); i++)
                        { ImprimirCodigoBarras(oProt,0); }


                    }



                }
                else
                {
                    lblError.Text = "Numero aun no cargado.";
                    lblError.Visible = true;
                }
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        //private void timer2_Tick(object sender, EventArgs e)
        //{
        //    BuscarProtocoloADemanda();
        //}

        //private void timer2_Tick(object sender, EventArgs e)
        //{
        //    //ConfirmarValidacion();
        //}

        //private void chkConfirmacionValidacion_CheckedChanged(object sender, EventArgs e)
        //{
        //    //if (chkConfirmacionValidacion.Checked)
        //    //{


        //    //    timer2.Enabled = true;
        //    //    timer2.Interval = 400000;  // 6 minutos
        //    //    timer2.Start();
        //    //    //InitTimer();
        //    //}
        //    //else
        //    //{

        //    //    timer2.Enabled = false;

        //    //}
        //}
    }
}
