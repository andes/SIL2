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
using Business.Data;
using System.Text.RegularExpressions;
using System.Configuration;

namespace WebLab.AutoAnalizador
{
    public partial class ImportaDatos : System.Web.UI.Page
    {
        private int cantidad = 0; public string mensagitoInicial = ""; string mensagitoProcesa = "";

        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"]!= null)
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            else
                Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                switch (Request["Equipo"].ToString())
                {                  

                    case "Gen5":
                        {
                            VerificaPermisos("Gen5 - Recepción de datos");
                            lblEquipo.Text = "GEN5 / ENDOCRINOLOGIA";
                        }
                        break;

                    case "Metrolab":
                        {
                            VerificaPermisos("Metrolab - Recepción de datos");
                            lblEquipo.Text = "METROLAB- CM 250&200";
                        }
                        break;
                    case "Incca":
                        {
                            VerificaPermisos("Incca - Recepción de datos");
                            lblEquipo.Text = "INCAA - Diconex";
                        }
                        break;
                    case "SysmexKX21N":
                        {
                            VerificaPermisos("Sysmex Kx21N- Recepción de datos");
                            lblEquipo.Text = "Sysmex - Contador Hematologico";
                        }
                        break;
                    case "SysmexXP300":
                        {
                            VerificaPermisos("Sysmex XP300- Recepción de datos");
                            lblEquipo.Text = "Sysmex - Contador Hematologico";
                        }
                        break;
                    case "Counter":
                        {
                            VerificaPermisos("Counter - Recepción de datos");
                            lblEquipo.Text = "Counter - Contador Hematologico";
                        }
                        break;

                }
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
                        MarcarSeleccionados(true);
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
                        DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_IncorporarResultados]";

            cmd.Parameters.Add("@idEfector", SqlDbType.Int);
            cmd.Parameters["@idEfector"].Value = oUser.IdEfector.IdEfector;

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados";
            PintarReferencias();
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            PintarReferencias();
        }
        private void PintarReferencias()
        {      
            foreach (GridViewRow row in gvLista.Rows)
            {           
                switch (row.Cells[8].Text)
                {
                    case "0": //sin enviar
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../../App_Themes/default/images/rojo.gif";
                            hlnk.ToolTip = "Sin procesar";
                            row.Cells[8].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //enviado
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../../App_Themes/default/images/amarillo.gif";
                            hlnk.ToolTip = "En proceso";
                            row.Cells[8].Controls.Add(hlnk);
                        }
                        break;
                   
                }
            }
        }
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            PintarReferencias();
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
            if (this.trepador.HasFile)
            {
                string filename = this.trepador.PostedFile.FileName;
                BorrarResultadosTemporales();

                //if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                //{
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                                switch (Request["Equipo"].ToString())
                                {

                         
                                    case "Incca":
                                        {
                                            ProcesarLineaIncca(line);
                                        }
                                        break;
                                    case "SysmexKX21N":
                                        {
                                    ProcesarLineaSysmexKX121N(line);
                                        }
                                        break;
                            case "SysmexXP300":
                                {
                                    ProcesarLineaSysmexXP300(line);
                                }
                                break;

                            case "Counter":
                                {
                                    ProcesarLineaCounter(line);
                                }
                                break;

                            default: ProcesarLinea(line); break;
                                }

                        
                       
                        }
                    }
                //}
            }
        }

        private void ProcesarLineaCounter(string mensagito)
        {
            //Grabar en la tabla temporal para mostrar al usuario los registros a incorporar
            try
            {

                Utility oUtil = new Utility();

         


                int v = mensagito.IndexOf("D", 0);
                int caracterInicio = Convert.ToInt32(Encoding.ASCII.GetBytes(mensagito.Substring(0, 1))[0]);// comienzo de trama caracter 2

                //{
                if  (caracterInicio==2) //(mensagito.IndexOf("D", 0) >= 0)
                {
                    mensagitoProcesa = "";
                    mensagitoInicial = mensagito; //*nuevo guardo el pedacito de mensaje inicial
                    //if (caracterInicio != 2)
                    //{
                    //    mensagitoProcesa = ""; // le pone caracter de inicio

                    //}
                }
                //   else
                //     mensagito = mensagito.Substring(1, mensagito.Length-1);
                //}l
               int caracterfin =  Convert.ToInt32(Encoding.ASCII.GetBytes(mensagito.Substring(mensagito.Length - 1, 1))[0]);// tiene que ser igual a 26
                mensagitoProcesa += mensagito;

                ////*nuevo cuando vienen separados el inicio y el fni
                int LARGO = mensagitoProcesa.Length; ///120 caracteres del mensaje de datos de resultados (info de calidad viene en 99 caracteres)
                //if ((LARGO < 121) && (caracterfin == 3)) // terminó pero no cumple con el largo
                //{
                //    mensagitoProcesa = mensagitoInicial + mensagitoProcesa;
                //}
                //////

                //LARGO = mensagitoProcesa.Length; ///120 caracteres del mensaje de datos de resultados (info de calidad viene en 99 caracteres)
                //if ((LARGO == 121) && (caracterfin == 3)) // FORMATO K21N: completa hasta 131
                //{
                //    string pri = mensagitoProcesa.Substring(0, 13);
                //    string seg = mensagitoProcesa.Substring(13, 108);
                //    mensagitoProcesa = pri + "          " + seg;
                //}
                //LARGO = mensagitoProcesa.Length;
                //if (LARGO == 131)  
                if ((LARGO > 100)  && (caracterfin == 26))
                {

                    SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                    string query = @"INSERT INTO Temp_Mensaje   (mensaje, fechaRegistro, idEfector) 
                VALUES           ( '" + mensagitoProcesa + "', getdate()," +oUser.IdEfector.IdEfector.ToString()+ ")";
                    SqlCommand cmd = new SqlCommand(query, conn2);
                    int idres2 = Convert.ToInt32(cmd.ExecuteScalar());


                    /////Decofificar mensagito k1000 por posicion de ocupacion
                    //string codeInicio = mensagitoProcesa.Substring(1, 1);
                    //if (codeInicio.ToUpper() == "D") //Datos de resultado
                    //{
                        //string code1 = mensagitoProcesa.Substring(2, 1);
                        //string code2 = mensagitoProcesa.Substring(3, 1);
                        //string code3 = mensagito.Substring(3, 2);
                        string anio = mensagitoProcesa.Substring(27, 4);//counter
                        string mes = mensagitoProcesa.Substring(23, 2);//counter
                        string dia = mensagitoProcesa.Substring(25, 2);//counter
                        //string code4 = mensagitoProcesa.Substring(12, 1);
                        //string f = mensagitoProcesa.Substring(13, 12);

                        string numeroprotocolo = oUtil.quitaCerosIzquierda(mensagitoProcesa.Substring(12, 10)).TrimStart().TrimEnd();//counter

                        //string code_PDA = mensagitoProcesa.Substring(23, 6);
                        //string code_rdw = mensagitoProcesa.Substring(29, 1);

                        String[] arr = new String[19]; /// Son 19 parametros los que envia el equipo

                        string WBC = mensagitoProcesa.Substring(35, 4);//Counter WBC ###.# ==> 10.2
                        WBC = WBC.Substring(0, 3) + "." + WBC.Substring(3, 1);
                        arr[0] = "WBC|" + oUtil.quitaCerosIzquierda(WBC);

                        string LymphNumeral = mensagitoProcesa.Substring(39, 4);//Counter  0026--Lymph ###.# ==>2.6                    
                        LymphNumeral = LymphNumeral.Substring(0, 3) + "." + LymphNumeral.Substring(3, 1);  
                        arr[1] = "Lymph#|" + oUtil.quitaCerosIzquierda(LymphNumeral);

                        string MidNumeral = mensagitoProcesa.Substring(43, 4);//Counter  0005--Mid ###.# ==> 0.5                   
                        MidNumeral = MidNumeral.Substring(0, 3) + "." + MidNumeral.Substring(3, 1);
                        arr[2] = "Mid#|" + oUtil.quitaCerosIzquierda(MidNumeral);

                        string GranNumeral = mensagitoProcesa.Substring(47, 4);//Counter  --Gran ###.# ==> 7.1                
                        GranNumeral = GranNumeral.Substring(0, 3) + "." + GranNumeral.Substring(3, 1);
                        arr[3] = "Gran#|" + oUtil.quitaCerosIzquierda(GranNumeral);

                        string LymphPorc = mensagitoProcesa.Substring(51, 3);//Counter  --Lymph% ##.# ==>25.7               
                        LymphPorc = LymphPorc.Substring(0, 2) + "." + LymphPorc.Substring(2, 1);
                        arr[4] = "Lymph%|" + oUtil.quitaCerosIzquierda(LymphPorc);

                        string MidPorc = mensagitoProcesa.Substring(54, 3);//Counter 050 --Mid% ##.# ==>5          
                        MidPorc = MidPorc.Substring(0, 2) + "." + MidPorc.Substring(2, 1);
                        arr[5] = "Mid%|" + oUtil.quitaCerosIzquierda(MidPorc);

                        string GranPorc = mensagitoProcesa.Substring(57, 3);//Counter 050 --Gran% ##.# ==>69.3        
                        GranPorc = GranPorc.Substring(0, 2) + "." + GranPorc.Substring(2, 1);
                        arr[6] = "Gran%|" + oUtil.quitaCerosIzquierda(GranPorc);

                       string RBC = mensagitoProcesa.Substring(60, 3);//Counter 424 --RBC ##.# ==> 4.24 --se informa asi desde el hospi
                        RBC = RBC.Substring(0, 1) + "." + RBC.Substring(1, 2); //RBC[x10 ^ 6 / uL]
                        arr[7] = "RBC|" + oUtil.quitaCerosIzquierda(RBC);
                    
                        string HGB = mensagitoProcesa.Substring(63, 3);//Counter 123 ----HGB ### ==> 12.3
                        HGB = HGB.Substring(0, 2) + "." + HGB.Substring(2, 1);
                        arr[8] = "HGB|" + oUtil.quitaCerosIzquierda(HGB);

                        string MCHC = mensagitoProcesa.Substring(66, 4);//Counter   0340--MCHC ####=>34 -- se informa
                        MCHC = MCHC.Substring(0, 3) + "." + MCHC.Substring(3,1);
                        arr[9] = "MCHC|" + oUtil.quitaCerosIzquierda(MCHC);

                    string MCV = mensagitoProcesa.Substring(70, 4);// 0852 --MCV ###.# ==> 85.2
                    MCV = MCV.Substring(0, 3) + "." + MCV.Substring(3, 1);
                    arr[10] = "MCV|" + oUtil.quitaCerosIzquierda(MCV);

                    string MCH = mensagitoProcesa.Substring(74, 4);//0290  -- MCH ###.# ==> 29
                    MCH = MCH.Substring(0, 3) + "." + MCH.Substring(3, 1);
                    arr[11] = "MCH|" + oUtil.quitaCerosIzquierda(MCH);


                    string RDWCV = mensagitoProcesa.Substring(78, 3);//129 -- RDW-CV%  ##.# ==> 12.9
                    RDWCV = RDWCV.Substring(0, 2) + "." + RDWCV.Substring(2, 1);
                    arr[12] = "RDW-CV|" + oUtil.quitaCerosIzquierda(RDWCV);


                    string HCT = mensagitoProcesa.Substring(81, 3);//361  --HCT% ##.# ==> 36.1
                    HCT = HCT.Substring(0, 2) + "." + HCT.Substring(2, 1);
                        arr[13] = "HCT|" + oUtil.quitaCerosIzquierda(HCT);   
                     

                        string PLT = mensagitoProcesa.Substring(84, 4); //0343  --PLT #### ==> 343
                        PLT = PLT.Substring(0, 4);
                        arr[14] = "PLT|" + oUtil.quitaCerosIzquierda(PLT);


                    string MPV = mensagitoProcesa.Substring(88, 3);//085 --MPV ##.# ==> 8.5
                    MPV = MPV.Substring(0, 2) + "." + MPV.Substring(2, 1);
                    arr[15] = "MPV|" + oUtil.quitaCerosIzquierda(MPV);

                    string PDW = mensagitoProcesa.Substring(91, 3);//155-- PDW ##.# ==> 15.5
                    PDW = PDW.Substring(0, 2) + "." + PDW.Substring(2, 1);
                    arr[16] = "PDW|" + oUtil.quitaCerosIzquierda(PDW);

                    string PCTPorc = mensagitoProcesa.Substring(94, 3);//  291--PCT % .### =>0.291
                    PCTPorc =  "0." + PCTPorc.Substring(0, 3);
                    arr[17] = "PCT%|" + oUtil.quitaCerosIzquierda(PCTPorc);

                    string RDWSD = mensagitoProcesa.Substring(97, 4);//0419 --RDW-SD  ###.# ==> 41.9
                    RDWSD = RDWSD.Substring(0, 3) + "." + RDWSD.Substring(3, 1);                   
                    arr[18] = "RDW-SD|" + oUtil.quitaCerosIzquierda(RDWSD);

                    //string WSCR = mensagitoProcesa.Substring(75, 5);//W-SCR [%]
                    //    WSCR = WSCR.Substring(0, 3) + "." + WSCR.Substring(3, 2);///le saco el centesimo para que la suma de 100. (3,1)
                    //    arr[8] = "W-SCR%|" + oUtil.quitaCerosIzquierda(WSCR);

                    //    string WMCR = mensagitoProcesa.Substring(80, 5);//W-MCR [%]
                    //    WMCR = WMCR.Substring(0, 3) + "." + WMCR.Substring(3, 2);///le saco el centesimo para que la suma de 100.
                    //    arr[9] = "W-MCR%|" + oUtil.quitaCerosIzquierda(WMCR);

                    //    string WLCR = mensagitoProcesa.Substring(85, 5);//W-LCR [%]
                    //    WLCR = WLCR.Substring(0, 3) + "." + WLCR.Substring(3, 2);///le saco el centesimo para que la suma de 100.
                    //    arr[10] = "W-LCR%|" + oUtil.quitaCerosIzquierda(WLCR);

                    //    string W_SCC = mensagitoProcesa.Substring(90, 5);                  //W-SCC
                    //    W_SCC = W_SCC.Substring(0, 3) + "." + W_SCC.Substring(3, 2);
                    //    arr[11] = "W-SCC|" + oUtil.quitaCerosIzquierda(W_SCC);

                    //    string W_MCC = mensagitoProcesa.Substring(95, 5);//W-MCC
                    //    W_MCC = W_MCC.Substring(0, 3) + "." + W_MCC.Substring(3, 2);
                    //    arr[12] = "W-MCC|" + oUtil.quitaCerosIzquierda(W_MCC);

                    //    string W_LCC = mensagitoProcesa.Substring(100, 5);//W-LCC
                    //    W_LCC = W_LCC.Substring(0, 3) + "." + W_LCC.Substring(3, 2);
                    //    arr[13] = "W-LCC|" + oUtil.quitaCerosIzquierda(W_LCC);  
                        

                        //string PLCR = mensagitoProcesa.Substring(125, 5);//P-LCR
                        //PLCR = PLCR.Substring(0, 3) + "." + PLCR.Substring(3, 2);
                        //arr[17] = "P-LCR|" + oUtil.quitaCerosIzquierda(PLCR);
                        ///Fin de decodificación de mensagito
                        ///Fin de decodificación de mensagito 
                        /// 
                        foreach (string r in arr)
                        {
                            if (r.Trim() != "")
                            {
                                string s_idItemA = "";
                                string[] arrCampo = r.Split(("|").ToCharArray());
                                string s_Prueba = arrCampo[0].ToString(); ///idItem
                                string s_Resultado = arrCampo[1].ToString().Trim(); ///Valor del item
                                if (oUtil.EsNumerico(s_Resultado)) ///Solo guarda numeros
                                {
                                    Grabar(numeroprotocolo, s_Prueba, s_Resultado, "");

                                }
                            }
                        }






                    //}
                }

            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }

        private void ProcesarLineaSysmexXP300(string mensagito)
        {
            //Grabar en la tabla temporal para mostrar al usuario los registros a incorporar
            try
            {

                Utility oUtil = new Utility();

                string mensagitoProcesa = "";
             

                int v = mensagito.IndexOf("D", 0);
                int caracterInicio = Convert.ToInt32(Encoding.ASCII.GetBytes(mensagito.Substring(0, 1))[0]);// comienzo de trama caracter 2

                //{
                if (mensagito.IndexOf("D", 0) >= 0)
                {
                    mensagitoProcesa = "";
                    mensagitoInicial = mensagito; //*nuevo guardo el pedacito de mensaje inicial
                    if (caracterInicio != 2)
                    {
                        mensagitoProcesa = ""; // le pone caracter de inicio
                    
                    }
                }
                //   else
                //     mensagito = mensagito.Substring(1, mensagito.Length-1);
                //}l
                int caracterfin = Convert.ToInt32(Encoding.ASCII.GetBytes(mensagito.Substring(mensagito.Length - 1, 1))[0]);// tiene que ser igual a 3
                mensagitoProcesa += mensagito;

                //*nuevo cuando vienen separados el inicio y el fni
                int LARGO = mensagitoProcesa.Length; ///120 caracteres del mensaje de datos de resultados (info de calidad viene en 99 caracteres)
                if ((LARGO < 121) && (caracterfin == 3)) // terminó pero no cumple con el largo
                {
                    mensagitoProcesa = mensagitoInicial + mensagitoProcesa;
                }
                ////

                LARGO = mensagitoProcesa.Length; ///120 caracteres del mensaje de datos de resultados (info de calidad viene en 99 caracteres)
                if ((LARGO == 121) && (caracterfin == 3)) // FORMATO K21N: completa hasta 131
                {
                    string pri = mensagitoProcesa.Substring(0, 13);
                    string seg = mensagitoProcesa.Substring(13, 108);
                    mensagitoProcesa = pri + "          " + seg;
                }
                LARGO = mensagitoProcesa.Length;
                if (LARGO == 131)  // FORMATO K21N
                {
                     GrabarMensajeTemp(mensagitoProcesa);
                    //SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                    //string query = @"INSERT INTO Temp_Mensaje   (mensaje, fechaRegistro)     
                    //    VALUES           ( '" + mensagitoProcesa + "', getdate() )";
                    //SqlCommand cmd = new SqlCommand(query, conn2);
                    //int idres2 = Convert.ToInt32(cmd.ExecuteScalar());


                    /////Decofificar mensagito k1000 por posicion de ocupacion
                    string codeInicio = mensagitoProcesa.Substring(1, 1);
                    if (codeInicio.ToUpper() == "D") //Datos de resultado
                    {
                        string code1 = mensagitoProcesa.Substring(2, 1);
                        string code2 = mensagitoProcesa.Substring(3, 1);
                        //string code3 = mensagito.Substring(3, 2);
                        string anio = mensagitoProcesa.Substring(4, 4);
                        string mes = mensagitoProcesa.Substring(8, 2);
                        string dia = mensagitoProcesa.Substring(10, 2);
                        string code4 = mensagitoProcesa.Substring(12, 1);
                        string f = mensagitoProcesa.Substring(13, 12);
                        string numeroprotocolo = oUtil.quitaCerosIzquierda(mensagitoProcesa.Substring(14, 14)).TrimStart().TrimEnd();//.Substring(12, 6)); //
                        string code_PDA = mensagitoProcesa.Substring(23, 6);
                        string code_rdw = mensagitoProcesa.Substring(29, 1);

                        String[] arr = new String[19]; /// Son 18 parametros los que envia el equipo

                        string WBC = mensagitoProcesa.Substring(35, 5);//WBC [x10^2/uL]
                        WBC = WBC.Substring(0, 3) + "." + WBC.Substring(3, 2);
                        arr[0] = "WBC|" + oUtil.quitaCerosIzquierda(WBC);

                        string RBC = mensagitoProcesa.Substring(40, 5);//RBC [x10^4/uL]
                       //  RBC = RBC.Substring(0, 3) + "." + RBC.Substring(3, 2);//RBC [x10^4/uL]
                        RBC = RBC.Substring(0, 2) + "." + RBC.Substring(2, 3); //RBC[x10 ^ 6 / uL]
                        arr[1] = "RBC|" + oUtil.quitaCerosIzquierda(RBC);

                        string HGB = mensagitoProcesa.Substring(45, 5);//HGB [g/dL]
                        HGB = HGB.Substring(0, 3) + "." + HGB.Substring(3, 2);
                        arr[2] = "HGB|" + oUtil.quitaCerosIzquierda(HGB);

                        string HCT = mensagitoProcesa.Substring(50, 5);//HCT [%]
                        HCT = HCT.Substring(0, 3) + "." + HCT.Substring(3, 2);
                        arr[3] = "HCT|" + oUtil.quitaCerosIzquierda(HCT);

                        string MCV = mensagitoProcesa.Substring(55, 5);// MCV[fL]
                        MCV = MCV.Substring(0, 3) + "." + MCV.Substring(3, 2);
                        arr[4] = "MCV|" + oUtil.quitaCerosIzquierda(MCV);

                        string MCH = mensagitoProcesa.Substring(60, 5);//MCH [pg]
                        MCH = MCH.Substring(0, 3) + "." + MCH.Substring(3, 2);
                        arr[5] = "MCH|" + oUtil.quitaCerosIzquierda(MCH);

                        string MCHC = mensagitoProcesa.Substring(65, 5);//MCHC [g/dL]
                        MCHC = MCHC.Substring(0, 3) + "." + MCHC.Substring(3, 2);
                        arr[6] = "MCHC|" + oUtil.quitaCerosIzquierda(MCHC);

                        string PLT = mensagitoProcesa.Substring(70, 5); //PLT[x10 ^ 4 / uL]
                        PLT = PLT.Substring(0, 4);
                        arr[7] = "PLT|" + oUtil.quitaCerosIzquierda(PLT);

                        string WSCR = mensagitoProcesa.Substring(75, 5);//W-SCR [%]
                        WSCR = WSCR.Substring(0, 3) + "." + WSCR.Substring(3, 2);///le saco el centesimo para que la suma de 100. (3,1)
                        arr[8] = "W-SCR%|" + oUtil.quitaCerosIzquierda(WSCR);

                        string WMCR = mensagitoProcesa.Substring(80, 5);//W-MCR [%]
                        WMCR = WMCR.Substring(0, 3) + "." + WMCR.Substring(3, 2);///le saco el centesimo para que la suma de 100.
                        arr[9] = "W-MCR%|" + oUtil.quitaCerosIzquierda(WMCR);

                        string WLCR = mensagitoProcesa.Substring(85, 5);//W-LCR [%]
                        WLCR = WLCR.Substring(0, 3) + "." + WLCR.Substring(3, 2);///le saco el centesimo para que la suma de 100.
                        arr[10] = "W-LCR%|" + oUtil.quitaCerosIzquierda(WLCR);

                        string W_SCC = mensagitoProcesa.Substring(90, 5);                  //W-SCC
                        W_SCC = W_SCC.Substring(0, 3) + "." + W_SCC.Substring(3, 2);
                        arr[11] = "W-SCC|" + oUtil.quitaCerosIzquierda(W_SCC);

                        string W_MCC = mensagitoProcesa.Substring(95, 5);//W-MCC
                        W_MCC = W_MCC.Substring(0, 3) + "." + W_MCC.Substring(3, 2);
                        arr[12] = "W-MCC|" + oUtil.quitaCerosIzquierda(W_MCC);

                        string W_LCC = mensagitoProcesa.Substring(100, 5);//W-LCC
                        W_LCC = W_LCC.Substring(0, 3) + "." + W_LCC.Substring(3, 2);
                        arr[13] = "W-LCC|" + oUtil.quitaCerosIzquierda(W_LCC);

                        string RDWSD = mensagitoProcesa.Substring(105, 5);//RDW-SD
                        RDWSD = RDWSD.Substring(0, 3) + "." + RDWSD.Substring(3, 2);
                        //arr[14] = "RDW-SD/CV|" + oUtil.quitaCerosIzquierda(RDW);
                        arr[14] = "RDW-SD|" + oUtil.quitaCerosIzquierda(RDWSD);


                        string RDWCV = mensagitoProcesa.Substring(110, 5);//RDW-CV
                        RDWCV = RDWCV.Substring(0, 3) + "." + RDWCV.Substring(3, 2);
                        arr[15] = "RDW-CV|" + oUtil.quitaCerosIzquierda(RDWCV);

                        string PDW = mensagitoProcesa.Substring(115, 5);//PDW
                        PDW = PDW.Substring(0, 3) + "." + PDW.Substring(3, 2);
                        arr[16] = "PDW|" + oUtil.quitaCerosIzquierda(PDW);

                        string MPV = mensagitoProcesa.Substring(120, 5);//MPV
                        MPV = MPV.Substring(0, 3) + "." + MPV.Substring(3, 2);
                        arr[17] = "MPV|" + oUtil.quitaCerosIzquierda(MPV);

                        string PLCR = mensagitoProcesa.Substring(125, 5);//P-LCR
                        PLCR = PLCR.Substring(0, 3) + "." + PLCR.Substring(3, 2);
                        arr[18] = "P-LCR|" + oUtil.quitaCerosIzquierda(PLCR);
                        ///Fin de decodificación de mensagito
                        ///Fin de decodificación de mensagito 
                        /// 
                        foreach (string r in arr)
                        {
                            if (r.Trim() != "")
                            {
                              
                                string[] arrCampo = r.Split(("|").ToCharArray());
                                string s_Prueba = arrCampo[0].ToString(); ///idItem
                                string s_Resultado = arrCampo[1].ToString().Trim(); ///Valor del item
                                if (oUtil.EsNumerico(s_Resultado)) ///Solo guarda numeros
                                {
                                    Grabar(numeroprotocolo, s_Prueba, s_Resultado, "");

                                }
                            }
                        }






                    }
                }

            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }

        private void GrabarMensajeTemp(string mensagitoProcesa)
        {
            try {
                SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                string query = @"INSERT INTO Temp_Mensaje   (mensaje, fechaRegistro, idEfector)     
                    VALUES           ( '" + mensagitoProcesa + "', getdate() ,"+oUser.IdEfector.IdEfector.ToString()+")";
                SqlCommand cmd = new SqlCommand(query, conn2);
                int idres2 = Convert.ToInt32(cmd.ExecuteScalar());

            }
            catch (Exception ex)
            {
                string exception = "";

             
            }
        }

        private void ProcesarLineaSysmexKX121N(string mensagito)
        {
            //Grabar en la tabla temporal para mostrar al usuario los registros a incorporar
            try
            {

                Utility oUtil = new Utility();

                string mensagitoProcesa = "";

                int v = mensagito.IndexOf("D", 0);
                int caracterInicio = Convert.ToInt32(Encoding.ASCII.GetBytes(mensagito.Substring(0, 1))[0]);// comienzo de trama caracter 2

                //{
                if (mensagito.IndexOf("D", 0) >= 0)
                {
                    mensagitoProcesa = "";
                    if (caracterInicio != 2)
                        mensagitoProcesa = ""; // le pone caracter de inicio
                }
                //   else
                //     mensagito = mensagito.Substring(1, mensagito.Length-1);
                //}l
                int caracterfin = Convert.ToInt32(Encoding.ASCII.GetBytes(mensagito.Substring(mensagito.Length - 1, 1))[0]);// tiene que ser igual a 3
                mensagitoProcesa += mensagito;

            
                int LARGO = mensagitoProcesa.Length; ///120 caracteres del mensaje de datos de resultados (info de calidad viene en 99 caracteres)
                if (LARGO == 121)
                {
                    /////Decofificar mensagito k1000 por posicion de ocupacion
                    string codeInicio = mensagitoProcesa.Substring(1, 1);
                    if (codeInicio.ToUpper() == "D") //Datos de resultado
                    {
                        GrabarMensajeTemp(mensagitoProcesa);
                        string unidadMedida = "";
                        string code1 = mensagitoProcesa.Substring(2, 1);
                        string code2 = mensagitoProcesa.Substring(3, 1);
                        //string code3 = mensagito.Substring(3, 2);
                        string anio = mensagitoProcesa.Substring(4, 2);
                        string mes = mensagitoProcesa.Substring(6, 2);
                        string dia = mensagitoProcesa.Substring(8, 2);
                        string code4 = mensagitoProcesa.Substring(10, 1);
                        string f = mensagitoProcesa.Substring(11, 12);
                        string numeroprotocolo = Convert.ToInt32(mensagitoProcesa.Substring(11, 12)).ToString();
                        string code_PDA = mensagitoProcesa.Substring(23, 6);
                        string code_rdw = mensagitoProcesa.Substring(29, 1);

                        String[] arr = new String[18]; /// Son 18 parametros los que envia el equipo

                        string WBC = mensagitoProcesa.Substring(30, 5);
                        WBC = WBC.Substring(0, 3) + "." + WBC.Substring(3, 2);
                        arr[0] = "WBC|" + oUtil.quitaCerosIzquierda(WBC);

                   

                        string RBC = mensagitoProcesa.Substring(35, 5);
                        RBC = RBC.Substring(0, 2) + "." + RBC.Substring(2, 3);
                        arr[1] = "RBC|" + oUtil.quitaCerosIzquierda(RBC);



                                   string HGB = mensagitoProcesa.Substring(40, 5);
                                   HGB = HGB.Substring(0, 3) + "." + HGB.Substring(3, 2);
                                   arr[2] = "HGB|" + oUtil.quitaCerosIzquierda(HGB);

                                   string HCT = mensagitoProcesa.Substring(45, 5);
                                   HCT = HCT.Substring(0, 3) + "." + HCT.Substring(3, 2);
                                   arr[3] = "HCT|" + oUtil.quitaCerosIzquierda(HCT);

                                   string MCV = mensagitoProcesa.Substring(50, 5);
                                   MCV = MCV.Substring(0, 3) + "." + MCV.Substring(3, 2);
                                   arr[4] = "MCV|" + oUtil.quitaCerosIzquierda(MCV);

                                   string MCH = mensagitoProcesa.Substring(55, 5);
                                   MCH = MCH.Substring(0, 3) + "." + MCH.Substring(3, 2);
                                   arr[5] = "MCH|" + oUtil.quitaCerosIzquierda(MCH);

                                   string MCHC = mensagitoProcesa.Substring(60, 5);
                                   MCHC = MCHC.Substring(0, 3) + "." + MCHC.Substring(3, 2);
                                   arr[6] = "MCHC|" + oUtil.quitaCerosIzquierda(MCHC);

                                   string PLT = mensagitoProcesa.Substring(65, 5);
                                   PLT = PLT.Substring(0, 4);
                                   arr[7] = "PLT|" + oUtil.quitaCerosIzquierda(PLT);

                                   string LYM_PORC = mensagitoProcesa.Substring(70, 5);
                                   LYM_PORC = LYM_PORC.Substring(0, 3) + "." + LYM_PORC.Substring(3, 2);///le saco el centesimo para que la suma de 100. (3,1)
                                   arr[8] = "LYM%|" + oUtil.quitaCerosIzquierda(LYM_PORC);

                                   string MXD_PORC = mensagitoProcesa.Substring(75, 5);
                                   MXD_PORC = MXD_PORC.Substring(0, 3) + "." + MXD_PORC.Substring(3, 2);///le saco el centesimo para que la suma de 100.
                                   arr[9] = "MXD%|" + oUtil.quitaCerosIzquierda(MXD_PORC);

                                   string NEUT_PORC = mensagitoProcesa.Substring(80, 5);
                                   NEUT_PORC = NEUT_PORC.Substring(0, 3) + "." + NEUT_PORC.Substring(3, 2);///le saco el centesimo para que la suma de 100.
                                   arr[10] = "NEUT%|" + oUtil.quitaCerosIzquierda(NEUT_PORC);

                                   string LYM_ABS = mensagitoProcesa.Substring(85, 5);
                                   LYM_ABS = LYM_ABS.Substring(0, 3) + "." + LYM_ABS.Substring(3, 2);
                                   arr[11] = "LYM#|" + oUtil.quitaCerosIzquierda(LYM_ABS);

                                   string MXD_ABS = mensagitoProcesa.Substring(90, 5);
                                   MXD_ABS = MXD_ABS.Substring(0, 3) + "." + MXD_ABS.Substring(3, 2);
                                   arr[12] = "MXD#|" + oUtil.quitaCerosIzquierda(MXD_ABS);

                                   string NEUT_ABS = mensagitoProcesa.Substring(95, 5);
                                   NEUT_ABS = NEUT_ABS.Substring(0, 3) + "." + NEUT_ABS.Substring(3, 2);
                                   arr[13] = "NEUT#|" + oUtil.quitaCerosIzquierda(NEUT_ABS);

                                   string RDW = mensagitoProcesa.Substring(100, 5);
                                   RDW = RDW.Substring(0, 3) + "." + RDW.Substring(3, 2);
                                   arr[14] = "RDW-SD/CV|" + oUtil.quitaCerosIzquierda(RDW);

                                   string PDW = mensagitoProcesa.Substring(105, 5);
                                   PDW = PDW.Substring(0, 3) + "." + PDW.Substring(3, 2);
                                   arr[15] = "PDW|" + oUtil.quitaCerosIzquierda(PDW);

                                   string MPV = mensagitoProcesa.Substring(110, 5);
                                   MPV = MPV.Substring(0, 3) + "." + MPV.Substring(3, 2);
                                   arr[16] = "MPV|" + oUtil.quitaCerosIzquierda(MPV);

                                   string PLCR = mensagitoProcesa.Substring(115, 5);
                                   PLCR = PLCR.Substring(0, 3) + "." + PLCR.Substring(3, 2);
                                   arr[17] = "P-LCR|" + oUtil.quitaCerosIzquierda(PLCR);
                        ///Fin de decodificación de mensagito 
                        /// 
                        foreach (string r in arr)
                        {
                            if (r.Trim() != "")
                            {
                                string s_idItemA = "";
                                string[] arrCampo = r.Split(("|").ToCharArray());
                                string s_Prueba = arrCampo[0].ToString(); ///idItem
                                string s_Resultado = arrCampo[1].ToString().Trim(); ///Valor del item
                                if (oUtil.EsNumerico(s_Resultado)) ///Solo guarda numeros
                                {
                                    Grabar(numeroprotocolo, s_Prueba, s_Resultado, unidadMedida);

                                }
                            }
                        }






                    }
                }
          
            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }
        private void ProcesarLineaIncca(string line)
        {
            try
            {
                //8: protocolo
                //    11: resultado
                //    2: determinaciones
                string[] arr = line.Split((",").ToCharArray());
                string numeroprotocolo = ""; //int cantidadPracticas = 0;
                if (arr.Length >= 12)
                {

                    string codigoPractica = "";
                    string resultado = "";
                    string unidadMedida = "";
                    numeroprotocolo = arr[8].ToString().Replace("\"","");
                    codigoPractica = arr[2].ToString().Replace("\"", "");
                  string  resultadoAux = arr[11].ToString().Replace("\"", "").Replace("Resultado:","");
                    resultadoAux = resultadoAux.TrimEnd().TrimStart();
 

                   
                    resultado = Regex.Replace(resultadoAux, @"[^\d.]", "");  //saco los alfanumericos
                    unidadMedida = Regex.Replace(resultadoAux, @"[\d.]", "");  //saco los alfanumericos

                    try
                            {
                               
                                Grabar(numeroprotocolo, codigoPractica, resultado, unidadMedida);
                            }
                            catch (Exception ex)
                            {
                                //string exception = "";
                                //while (ex != null)
                                //{
                                //    exception = ex.Message + "<br>";

                                //}
                            }
                      
                }
            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }

        private void BorrarResultadosTemporales()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(MindrayResultado));
            crit.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector)); // borra por idefector
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (MindrayResultado oDetalle in detalle)
                {                    
                        oDetalle.Delete();
                }
            }
        }

        private void ProcesarLinea(string linea)
        {
            try
            {
                string[] arr = linea.Split((";").ToCharArray());
                string numeroprotocolo = ""; int cantidadPracticas = 0;
                if (arr.Length >= 12)
                {
                    string codigoPractica = "";
                    string resultado = "";
                    string unidadMedida = "";
                    numeroprotocolo = arr[0].ToString().TrimEnd();
                    //string[] arrprotocolo = numeroprotocolo.Split(("-").ToArray());
                    //if (arrprotocolo.Length >= 1)
                    if (1 == 1)
                    {
                        //numeroprotocolo = arrprotocolo[0].ToString();

                        cantidadPracticas = int.Parse(arr[9].ToString());
                        if (cantidadPracticas > 0)
                        {
                            for (int j = 0; j < cantidadPracticas; j++)
                            {
                                try
                                {
                                    int m = 9 + (j * 3); ///Cada tres posiciones esta un nuevo analisiS

                                    codigoPractica = arr[m + 1].ToString();
                                    resultado = arr[m + 2].ToString();
                                    unidadMedida = arr[m + 3].ToString();
                                    Grabar(numeroprotocolo, codigoPractica, resultado, unidadMedida);
                                }
                                catch (Exception ex)
                                {
                                    //string exception = "";
                                    //while (ex != null)
                                    //{
                                    //    exception = ex.Message + "<br>";

                                    //}
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string exception = "";
                
                   exception = ex.Message + "<br>";

                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema."+ exception;
            }
        }

        private void Grabar(string numeroprotocolo, string codigoPractica, string resultado, string unidadMedida)
        {
            ///Se reutiliza la tabla MindrayResultado para volcar temporalmente los resultados del metrolab
            MindrayResultado oRegistro = new MindrayResultado();
            oRegistro.Protocolo = numeroprotocolo.Trim();
            oRegistro.FechaProtocolo = DateTime.Now;
            oRegistro.IdItemMindray = 0;
            oRegistro.Descripcion = codigoPractica.Trim();
            oRegistro.UnidadMedida = unidadMedida.Trim();
            oRegistro.ValorObtenido = resultado.Trim();
            oRegistro.TipoValor = "";
            oRegistro.FechaResultado = DateTime.Now;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Estado = 0;
            oRegistro.IdEfector = oUser.IdEfector.IdEfector;
            oRegistro.Save();

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
            Response.Redirect("Mensaje.aspx?Equipo="+ Request["Equipo"].ToString() + "&Cantidad="+ cantidad.ToString(), false);
            // mostrar mensaje que se han guardado los datos
        }

        private void Guardar()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    string s_prefijo = "";
                    cantidad += 1;
                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    string numero =  row.Cells[1].Text.Trim();
                    string numeroAux = row.Cells[1].Text.Trim();
                    ///
                    string[] arr = numero.Split(("-").ToCharArray());
                    if (arr.Length > 1)// tiene prefijo
                    {
                        numero = arr[0].ToString();
                        s_prefijo = arr[1].ToString();
                    }
            
                    ///

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(MindrayResultado));
                    crit.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));
                    crit.Add(Expression.Eq("Protocolo", numeroAux));
                    IList items = crit.List();
                    foreach (MindrayResultado oResultado in items)
                    {
                        if (Request["equipo"].ToString() == "Metrolab")
                        {
                            guardarMetrolab(oResultado, s_prefijo, m_session, oProtocolo);
                        }
                        if (Request["equipo"].ToString() == "Gen5")
                        {
                            //guardarGen5(oResultado, s_prefijo, m_session, oProtocolo);
                        }
                        if (Request["equipo"].ToString() == "Incca")
                        {
                            guardarIncca(oResultado, s_prefijo, m_session, oProtocolo);
                        }

                        if (Request["equipo"].ToString() == "SysmexKX21N")
                        {
                            guardarSysmexKx21N(oResultado, s_prefijo, m_session, oProtocolo);  //sysmex k21n y xp300 es lo mismo
                        }

                        if (Request["equipo"].ToString() == "SysmexXP300")
                        {
                            guardarSysmexXP300(oResultado, s_prefijo, m_session, oProtocolo);  //sysmex k21n y xp300 es lo mismo
                        }

                        if (Request["equipo"].ToString() == "Counter")
                        {
                            guardarCounter(oResultado, s_prefijo, m_session, oProtocolo);  //counter Añelo
                        }
                    } // foreach
                }// chececk
            }// primero
        
    }


        private void guardarCounter(MindrayResultado oResultado, string s_prefijo, ISession m_session, Protocolo oProtocolo)
        {/// busco el item en el lis 
            try
            {
                ICriteria crit2 = m_session.CreateCriteria(typeof(Business.Data.AutoAnalizador.CounterItem));
                crit2.Add(Expression.Eq("IdCounter", oResultado.Descripcion));
                //crit2.Add(Expression.Eq("Prefijo", s_prefijo));
                crit2.Add(Expression.Eq("Habilitado", true));
                CounterItem oItem = (CounterItem)crit2.UniqueResult();

                if (oItem != null)
                {
                    int IdItemLIS = oItem.IdItem; // id item en el lis
                    Item oItemLIS = new Item();
                    oItemLIS = (Item)oItemLIS.Get(typeof(Item), IdItemLIS);
                    string valorObtenido = oResultado.ValorObtenido;
                    if (oItemLIS != null)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oProtocolo, "IdSubItem", oItemLIS);
                        if (oDetalle != null)
                        {
                            if (oDetalle.IdUsuarioValida == 0)// si no fue validado
                            {
                                if (oItemLIS.IdTipoResultado == 1) //Si es numero
                                {
                                    decimal s_ItemNum = decimal.Parse(valorObtenido.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                    oDetalle.ResultadoNum = s_ItemNum;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Counter", int.Parse(Session["idUsuario"].ToString()));
                                }
                                else //Si es texto
                                {
                                    oDetalle.ResultadoCar = valorObtenido;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Counter", int.Parse(Session["idUsuario"].ToString()));
                                }
                                if (oProtocolo.Estado == 0)
                                {
                                    oProtocolo.Estado = 1;
                                    oProtocolo.Save();
                                }
                            } // fin if idusuario validado
                        } // fin odetalle null  
                    }
                }
            }
            catch
            { }
        }


        private void guardarIncca(MindrayResultado oResultado, string s_prefijo, ISession m_session, Protocolo oProtocolo)
        {/// busco el item en el lis 
            try
            {
                ICriteria crit2 = m_session.CreateCriteria(typeof(InccaItem));
                crit2.Add(Expression.Eq("IdIncca", oResultado.Descripcion));
                crit2.Add(Expression.Eq("Prefijo", s_prefijo));
                crit2.Add(Expression.Eq("Habilitado", true));
                InccaItem oItem = (InccaItem)crit2.UniqueResult();

                if (oItem != null)
                {
                    int IdItemLIS = oItem.IdItem; // id item en el lis
                    Item oItemLIS = new Item();
                    oItemLIS = (Item)oItemLIS.Get(typeof(Item), IdItemLIS);
                    string valorObtenido = oResultado.ValorObtenido;

                    DetalleProtocolo oDetalle = new DetalleProtocolo();
                    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oProtocolo, "IdSubItem", oItemLIS);
                    if (oDetalle != null)
                    {
                        if (oDetalle.IdUsuarioValida == 0)// si no fue validado
                        {
                            if (oItemLIS.IdTipoResultado == 1) //Si es numero
                            {
                                decimal s_ItemNum = decimal.Parse(valorObtenido.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                oDetalle.ResultadoNum = s_ItemNum;
                                oDetalle.Enviado = 2;
                                oDetalle.ConResultado = true;
                                oDetalle.FechaResultado = DateTime.Now;
                                oDetalle.Save();
                                oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Incca", int.Parse(Session["idUsuario"].ToString()));
                            }
                            else //Si es texto
                            {
                                oDetalle.ResultadoCar = valorObtenido;
                                oDetalle.Enviado = 2;
                                oDetalle.ConResultado = true;
                                oDetalle.FechaResultado = DateTime.Now;
                                oDetalle.Save();
                                oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Incca", int.Parse(Session["idUsuario"].ToString()));
                            }
                            if (oProtocolo.Estado == 0)
                            {
                                oProtocolo.Estado = 1;
                                oProtocolo.Save();
                            }
                        } // fin if idusuario validado
                    } // fin odetalle null  
                }
            }
            catch
            { }
        }
        private void guardarSysmexKx21N(MindrayResultado oResultado, string s_prefijo, ISession m_session, Protocolo oProtocolo)
        {/// busco el item en el lis 
            try
            {
                ICriteria crit2 = m_session.CreateCriteria(typeof(Business.Data.AutoAnalizador.SysmexItemkx21n));
                crit2.Add(Expression.Eq("IdSysmex", oResultado.Descripcion));
                //crit2.Add(Expression.Eq("Prefijo", s_prefijo));
                crit2.Add(Expression.Eq("Habilitado", true));
                SysmexItemkx21n oItem = (SysmexItemkx21n)crit2.UniqueResult();

                if (oItem != null)
                {
                    int IdItemLIS = oItem.IdItem; // id item en el lis
                    Item oItemLIS = new Item();
                    oItemLIS = (Item)oItemLIS.Get(typeof(Item), IdItemLIS);
                    string valorObtenido = oResultado.ValorObtenido;
                    if (oItemLIS != null)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oProtocolo, "IdSubItem", oItemLIS);
                        if (oDetalle != null)
                        {
                            if (oDetalle.IdUsuarioValida == 0)// si no fue validado
                            {
                                if (oItemLIS.IdTipoResultado == 1) //Si es numero
                                {
                                    decimal s_ItemNum = decimal.Parse(valorObtenido.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                    oDetalle.ResultadoNum = s_ItemNum;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Sysmex", int.Parse(Session["idUsuario"].ToString()));
                                }
                                else //Si es texto
                                {
                                    oDetalle.ResultadoCar = valorObtenido;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Sysmex", int.Parse(Session["idUsuario"].ToString()));
                                }
                                if (oProtocolo.Estado == 0)
                                {
                                    oProtocolo.Estado = 1;
                                    oProtocolo.Save();
                                }
                            } // fin if idusuario validado
                        } // fin odetalle null  
                    }
                }
            }
            catch
            { }
        }
        private void guardarSysmexXP300(MindrayResultado oResultado, string s_prefijo, ISession m_session, Protocolo oProtocolo)
        {/// busco el item en el lis 
            try
            {
                ICriteria crit2 = m_session.CreateCriteria(typeof(Business.Data.AutoAnalizador.SysmexItemXP300));
                crit2.Add(Expression.Eq("IdSysmex", oResultado.Descripcion));
                //crit2.Add(Expression.Eq("Prefijo", s_prefijo));
                crit2.Add(Expression.Eq("Habilitado", true));
                SysmexItemXP300 oItem = (SysmexItemXP300)crit2.UniqueResult();

                if (oItem != null)
                {
                    int IdItemLIS = oItem.IdItem; // id item en el lis
                    Item oItemLIS = new Item();
                    oItemLIS = (Item)oItemLIS.Get(typeof(Item), IdItemLIS);
                    string valorObtenido = oResultado.ValorObtenido;
                    if (oItemLIS != null)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oProtocolo, "IdSubItem", oItemLIS);
                        if (oDetalle != null)
                        {
                            if (oDetalle.IdUsuarioValida == 0)// si no fue validado
                            {
                                if (oItemLIS.IdTipoResultado == 1) //Si es numero
                                {
                                    decimal s_ItemNum = decimal.Parse(valorObtenido.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                    oDetalle.ResultadoNum = s_ItemNum;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Sysmex", int.Parse(Session["idUsuario"].ToString()));
                                }
                                else //Si es texto
                                {
                                    oDetalle.ResultadoCar = valorObtenido;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Sysmex", int.Parse(Session["idUsuario"].ToString()));
                                }
                                if (oProtocolo.Estado == 0)
                                {
                                    oProtocolo.Estado = 1;
                                    oProtocolo.Save();
                                }
                            } // fin if idusuario validado
                        } // fin odetalle null  
                    }
                }
            }
            catch
            { }
        }

        private void guardarMetrolab(MindrayResultado oResultado, string s_prefijo, ISession m_session, Protocolo oProtocolo)
        {
            try
            {
                /// busco el item en el lis                    
                ICriteria crit2 = m_session.CreateCriteria(typeof(MetrolabItem));
                crit2.Add(Expression.Eq("IdMetrolab", oResultado.Descripcion));
                crit2.Add(Expression.Eq("Prefijo", s_prefijo));
                crit2.Add(Expression.Eq("Habilitado", true));
                MetrolabItem oItem = (MetrolabItem)crit2.UniqueResult();

                if (oItem != null)
                {
                    int IdItemLIS = oItem.IdItem; // id item en el lis
                    Item oItemLIS = new Item();
                    oItemLIS = (Item)oItemLIS.Get(typeof(Item), IdItemLIS);
                    string valorObtenido = oResultado.ValorObtenido;
                    if (oItemLIS != null)
                    {
                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oProtocolo, "IdSubItem", oItemLIS);
                        if (oDetalle != null)
                        {
                            if (oDetalle.IdUsuarioValida == 0)// si no fue validado
                            {
                                if (oItemLIS.IdTipoResultado == 1) //Si es numero
                                {
                                    decimal s_ItemNum = decimal.Parse(valorObtenido.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                    oDetalle.ResultadoNum = s_ItemNum;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Metrolab", int.Parse(Session["idUsuario"].ToString()));
                                }
                                else //Si es texto
                                {
                                    oDetalle.ResultadoCar = valorObtenido;
                                    oDetalle.Enviado = 2;
                                    oDetalle.ConResultado = true;
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.Save();
                                    oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Metrolab", int.Parse(Session["idUsuario"].ToString()));
                                }
                                if (oProtocolo.Estado == 0)
                                {
                                    oProtocolo.Estado = 1;
                                    oProtocolo.Save();
                                }
                            } // fin if idusuario validado
                        } // fin odetalle null  
                    }
                }
            }
            catch
            { }
        }
    }
}