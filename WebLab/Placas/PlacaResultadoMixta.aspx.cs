using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Business.Data;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Drawing;
using System.Text;
using System.IO;
using Business.Data.Laboratorio;
using System.Web.UI.HtmlControls;
using System.Net;
using WebLab.Resultados;
using System.Web.Script.Serialization;
using System.Net.Http;
using CrystalDecisions.Web;
//using DalSic;
//using DalPadron;

namespace WebLab.Placas {
    public partial class PlacaResultadoMixta : System.Web.UI.Page {
    
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            //MiltiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);


        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
             
                if (Request["idPlaca"] != null)
                {    //PreventingDoubleSubmit(btnValidar);
                    HdIdPlaca.Value = Request["idPlaca"].ToString();

                    hdProcesaArchivo.Value = "N";


                    if (Request["Desde"] == "Valida")
                    { ValidacionUsuario(); }

                    Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
                        oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(HdIdPlaca.Value));
                    lblEquipo.Text = oRegistro.Equipo;
                    lblNro.Text = oRegistro.IdPlaca.ToString();

                    MostrarDatosPlaca();

                    if (Request["Desde"] == "Consulta")
                    {
                        lbltituloCSV.Visible = false;
                        btnValidar.Visible = false;
                        trepador.Visible = false;
                        subir.Visible = false;
                        marcar.Visible = false;
                        desmarcar.Visible = false;
                        
                    }
                }
                 
            }
        }
        private void PreventingDoubleSubmit(Button button)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == ' ') { ");
            sb.Append("var oldPage_IsValid = Page_IsValid; var oldPage_BlockSubmit = Page_BlockSubmit;");
            sb.Append("if (Page_ClientValidate('" + button.ValidationGroup + "') == false) {");
            sb.Append(" Page_IsValid = oldPage_IsValid; Page_BlockSubmit = oldPage_BlockSubmit; return false; }} ");
            sb.Append("this.value = 'Procesando...';");
            sb.Append("this.disabled = true;");
            sb.Append(ClientScript.GetPostBackEventReference(button, null) + ";");
            sb.Append("return true;");

            string submit_Button_onclick_js = sb.ToString();
            button.Attributes.Add("onclick", submit_Button_onclick_js);
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
                        ProcesarFichero();
                        MostrarDatosPlacaDesdeArchivo();
                        hdProcesaArchivo.Value = "S";
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) {
                //estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; 
            }
        }


        private void MostrarDatosPlacaDesdeArchivo()
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(HdIdPlaca.Value));
            lblNro.Text = oRegistro.IdPlaca.ToString();
            MostrarControlesPlaca();
            MostrarPanel(oRegistro);

            /// lista de detalles diferentes

            string m_strSQL = @"select distinct iddetalleprotocolo from LAB_DetalleProtocoloPlaca where idPlaca =" + oRegistro.IdPlaca.ToString();

            int cantidadPos = 0;
            int cantidadNega = 0;
            int cantidadInd = 0;



            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {

                int iddet = int.Parse(Ds.Tables[0].Rows[i][0].ToString());
                DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
                crit.Add(Expression.Eq("IdDetalleProtocolo", iddet));
                crit.Add(Expression.Eq("IdPlaca", oRegistro.IdPlaca));




                IList items = crit.List();

                foreach (DetalleProtocoloPlaca oDet in items)
                {
                    /// promega: pendiente
                    string celda = "";
                    string n1 = "";
                    string n2 = "";
                    string rp = "";
                    string nro = "";
                    string celditaN1 = "";
                    string celditaN2 = "";
                    string celditaRP = "";
                    //try
                    //{
                 

                    if (oRegistro.Equipo == "PromegaM")  //mixto
                    {

                        string controlCelda = "";
                        string controlCeldaN2 = "";
                        string controlCeldaRP = "";
                        string controlProtocolo = "";

                        string valorRP = "";

                        if ((oDet.NamespaceID.Substring(0, 1) == "A") ||
                               (oDet.NamespaceID.Substring(0, 1) == "B") ||
                               (oDet.NamespaceID.Substring(0, 1) == "C"))
                        {
                            controlCeldaRP = "D";
                            if (oDet.NamespaceID.Length == 2) 
                                controlCeldaRP = controlCeldaRP + oDet.NamespaceID.Substring(1, 1);

                            if (oDet.NamespaceID.Length == 3)
                                controlCeldaRP = controlCeldaRP + oDet.NamespaceID.Substring(1, 2);  
                        }

                        if ((oDet.NamespaceID.Substring(0, 1) == "E") ||
                              (oDet.NamespaceID.Substring(0, 1) == "F") ||
                              (oDet.NamespaceID.Substring(0, 1) == "G"))
                        {                            
                            controlCeldaRP = "H";
                            if (oDet.NamespaceID.Length == 2)      
                                controlCeldaRP = controlCeldaRP + oDet.NamespaceID.Substring(1, 1);

                            if (oDet.NamespaceID.Length == 3)                         
                                controlCeldaRP = controlCeldaRP + oDet.NamespaceID.Substring(1, 2);
                           
                        }
                        string[] arrctRP = GetCTMixto(controlCeldaRP, "", "Promega").Split((";").ToCharArray());
                        string lblctRP = "lbl_P_" + controlCeldaRP;
                        Control controlRP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctRP);
                        Label lblRP = (Label)controlRP;
                        lblRP.Text = arrctRP[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                        lblRP.Visible = true;
                        if ((lblRP.Text == "Unde") || (lblRP.Text.ToUpper() == "NAN"))
                            valorRP = "INDETERMINADO";

                        //if (oDet.NamespaceID.Length == 3)
                        //    celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 1);
                        //if (oDet.NamespaceID.Length == 4)
                        //    celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 2);
                        if ((oDet.NamespaceID.Substring(0, 1) == "A")|| (oDet.NamespaceID.Substring(0, 1) == "E"))
                        {
                            if ((oDet.NamespaceID.Substring(0, 1) == "A")||
                                (oDet.NamespaceID.Substring(0, 1) == "B") ||
                                (oDet.NamespaceID.Substring(0, 1) == "C"))
                            { controlProtocolo = "ABCD";
                                controlCeldaRP = "D";

                            }


                            if ((oDet.NamespaceID.Substring(0, 1) == "E") ||
                                (oDet.NamespaceID.Substring(0, 1) == "F") ||
                                (oDet.NamespaceID.Substring(0, 1) == "G"))
                            {
                                controlProtocolo = "EFGH";
                                controlCeldaRP = "H";
                            }


                            if (oDet.NamespaceID.Length == 2)
                            {
                                
                                controlProtocolo = controlProtocolo + "_" + oDet.NamespaceID.Substring(1, 1);
                                //controlCeldaRP = controlCeldaRP   + oDet.NamespaceID.Substring(1, 1);

                            }

                            if (oDet.NamespaceID.Length == 3)
                            {
                               
                                controlProtocolo = controlProtocolo + "_" + oDet.NamespaceID.Substring(1, 2);
                                //controlCeldaRP = controlCeldaRP  + oDet.NamespaceID.Substring(1, 2);
                            }

                           


                            string lblprotocolo = "lbl_" + controlProtocolo;
                            string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());

                            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblprotocolo);
                            Label txtNumero = (Label)control1;
                          
                            txtNumero.Text = arr[0].ToString();
                            txtNumero.Visible = true;


                            string lbllugar = "lblLugar_" + controlProtocolo;

                            Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lbllugar);
                            Label lblLugar = (Label)control2;
                            lblLugar.Text = arr[1].ToString();
                            lblLugar.Font.Bold = true;
                            lblLugar.Visible = true;

                            string lblfIS = "lblFIS_" + controlProtocolo;

                            Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblfIS);
                            Label lblFIS = (Label)control3;
                            lblFIS.Text = arr[2].ToString();
                            if (!arr[2].Contains("SD"))
                                lblFIS.ForeColor = Color.Blue;
                            else
                                lblFIS.ForeColor = Color.Red;
                            lblFIS.Visible = true;

                          




                        }


                        if (oDet.NamespaceID.Length == 2)
                        {
                            celda = oDet.NamespaceID.Substring(0, 1) + oDet.NamespaceID.Substring(1, 1);
                            
                        }

                        if (oDet.NamespaceID.Length == 3)
                        {
                            celda = oDet.NamespaceID.Substring(0, 1) + oDet.NamespaceID.Substring(1, 2);
                          
                        }

                      
                       
                       
                        string[] arrct2 = GetCTMixto(celda,valorRP, "Promega").Split((";").ToCharArray());
                        string lblctN2 = "lbl_P_" + celda;
                        Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN2);
                        Label lblN2 = (Label)controlN2;
                        lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                        lblN2.Visible = true;
                        if (lblN2.Text == "INDETERMINADO")
                        {
                         //   lblN2.Text = "INDETERMINADO";

                          

                            if ((oDet.NamespaceID.Substring(0, 1) != "D") && (oDet.NamespaceID.Substring(0, 1) != "H"))
                            {
                                string chkvalida = "chkValida_P_" + celda;
                                Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                                CheckBox chkValida = (CheckBox)control7;
                                chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;
                                chkValida.Visible = false;
                            }
                        }
                            
                        
                        else
                        { 

                            string lblctRes = "lbl_P_" + celda + "Res";
                            Control controlRes = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctRes);
                            Label lblRes = (Label)controlRes;
                            decimal result;
                            if (decimal.TryParse(lblN2.Text, out result))
                            {
                                lblRes.Text = "SE DETECTA";
                                lblRes.ForeColor = Color.Red;
                            }

                            else
                                lblRes.Text = "NO SE DETECTA";
                            lblRes.Visible = true;

                            if ((oDet.NamespaceID.Substring(0, 1) != "D") && (oDet.NamespaceID.Substring(0, 1) != "H"))
                            {
                                string chkvalida = "chkValida_P_" + celda;
                                Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                                CheckBox chkValida = (CheckBox)control7;
                                chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;
                                chkValida.Visible = true;
                            }

                        }
                       
                         
                    }
                   
                    break;
                }
              
            }


            lblPosi.Text = cantidadPos.ToString();
            lblNega.Text = cantidadNega.ToString();
            lblIndeterminado.Text = cantidadInd.ToString();
            lblTotal.Text = (cantidadPos + cantidadNega + cantidadInd).ToString();

        }

        private void MostrarPanel(Placa oRegistro)
        {
            switch (oRegistro.Equipo)
            {
                case "Promega-30M":
                    {
                        btnImprimir.Visible = false;
                        pnlPromega2.Visible = true;
                        pnlAlplex.Visible = false;
                        pnlPromega.Visible = false;
                        break;
                    }

                case "Alplex":
                    {
                        btnImprimir.Visible = true;
                        pnlPromega2.Visible = false;
                        pnlAlplex.Visible = true;
                        pnlPromega.Visible = false;
                        textoimprimir.Visible = false;
                        break;
                    }
                case "PromegaM":
                    {
                        btnImprimir.Visible = false;
                        pnlPromega2.Visible = false;
                        pnlAlplex.Visible = false;
                        pnlPromega.Visible = true;
                        break;
                    }
            }
        }

        private void MostrarDatosPlaca()
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(HdIdPlaca.Value));
            lblNro.Text = oRegistro.IdPlaca.ToString();

            MostrarPanel(oRegistro);
            MostrarControlesPlaca();

            /// lista de detalles diferentes

            string m_strSQL =   @"select distinct iddetalleprotocolo from LAB_DetalleProtocoloPlaca where idPlaca ="+ oRegistro.IdPlaca.ToString();

  int cantidadPos = 0;
                int cantidadNega = 0;
                int cantidadInd = 0;


            hdProcesaArchivo.Value = "N";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                 
                int iddet =int.Parse( Ds.Tables[0].Rows[i][0].ToString());
                DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
                crit.Add(Expression.Eq("IdDetalleProtocolo", iddet));
                crit.Add(Expression.Eq("IdPlaca", oRegistro.IdPlaca));


                IList items = crit.List();

                foreach (DetalleProtocoloPlaca oDet in items)
                {                    /// promega: pendiente
                    string celda = "";
                    string n1 = "";
                    string n2 = "";
                    string rp = "";
                    string nro = "";
                    string celditaN1 = "";
                    string celditaN2 = "";
                    string controlCeldaRP = "";
                    string controlProtocolo = "";
                    try
                    {
                       

                        if (oRegistro.Equipo == "PromegaM")
                        {
                            
                                if ((oDet.NamespaceID.Substring(0, 1) == "A") ||
                                    (oDet.NamespaceID.Substring(0, 1) == "B") ||
                                    (oDet.NamespaceID.Substring(0, 1) == "C"))
                                {
                                    controlProtocolo = "ABCD";
                                controlCeldaRP = "D";

                                }


                                if ((oDet.NamespaceID.Substring(0, 1) == "E") ||
                                    (oDet.NamespaceID.Substring(0, 1) == "F") ||
                                    (oDet.NamespaceID.Substring(0, 1) == "G"))
                                {
                                    controlProtocolo = "EFGH";
                                    controlCeldaRP = "H";
                                }


                                if (oDet.NamespaceID.Length == 2)
                                {

                                    controlProtocolo = controlProtocolo + "_" + oDet.NamespaceID.Substring(1, 1);
                                    controlCeldaRP = controlCeldaRP  + oDet.NamespaceID.Substring(1, 1);

                                }

                                if (oDet.NamespaceID.Length == 3)
                                {

                                    controlProtocolo = controlProtocolo + "_" + oDet.NamespaceID.Substring(1, 2);
                                 controlCeldaRP = controlCeldaRP   + oDet.NamespaceID.Substring(1, 2);
                            }


                            celda = oDet.NamespaceID;

                                string lblprotocolo = "lbl_" + controlProtocolo;
                                string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());

                                Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblprotocolo);
                                Label txtNumero = (Label)control1;

                                txtNumero.Text = arr[0].ToString();
                                txtNumero.Visible = true;


                                string lbllugar = "lblLugar_" + controlProtocolo;

                                Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lbllugar);
                                Label lblLugar = (Label)control2;
                                lblLugar.Text = arr[1].ToString();
                                lblLugar.Font.Bold = true;
                                lblLugar.Visible = true;

                                string lblfIS = "lblFIS_" + controlProtocolo;

                                Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblfIS);
                                Label lblFIS = (Label)control3;
                                lblFIS.Text = arr[2].ToString();
                                if (!arr[2].Contains("SD"))
                                    lblFIS.ForeColor = Color.Blue;
                                else
                                    lblFIS.ForeColor = Color.Red;
                                lblFIS.Visible = true;

                                                        

                            string s_idProtocolo = arr[4].ToString();
                            string nombreCT = celda.Substring(0, 1);
                            if ((nombreCT == "C") || (nombreCT == "G")) nombreCT = "N1";
                            if  (nombreCT == "E")   nombreCT = "A";
                            if (nombreCT == "F") nombreCT = "B";
                            if ((nombreCT == "D") || (nombreCT == "H")) nombreCT = "RP";

                            //    cambiar el getCT desde archivo por desde BD lab_detalleprotocolo 
                            string[] arrct1 = GetCTDetalleProtocolo(s_idProtocolo, nombreCT, "Promega",0).Split((";").ToCharArray());
                            string lblctN1 = "lbl_P_" + celda;
                            Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN1);
                            Label lblN1 = (Label)controlN1;
                            lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("N1", "");
                            lblN1.Visible = true;

                            string[] arrct2 = GetCTDetalleProtocolo(s_idProtocolo, "", "Promega", oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                            string lblctN2 = "lbl_P_" + celda +"Res";
                            Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN2);
                            Label lblN2 = (Label)controlN2;
                            lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("RP", "");
                            lblN2.Visible = true;

                            string[] arrct3 = GetCTDetalleProtocolo(s_idProtocolo, "RP", "Promega", 0).Split((";").ToCharArray());
                            string lblctRP = "lbl_P_" + controlCeldaRP;
                            Control controlrp = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctRP);
                            Label lblRP = (Label)controlrp;
                            lblRP.Text = arrct3[0].ToString().Replace("FAM=", "").Replace("RP", ""); ;
                            lblRP.Visible = true;

                            if (!arr[2].Contains("SD"))
                                lblFIS.ForeColor = Color.Blue;
                            else
                                lblFIS.ForeColor = Color.Red;
                            lblFIS.Visible = true;

                            string res = arr[5].ToString().ToUpper();

                          

                            if (lblN2.Text.Length > 1)
                            {
                                if (res.TrimStart().Substring(0, 9) == "SE DETECT")
                                {
                                    lblN1.ForeColor = Color.Red;
                                    lblN2.ForeColor = Color.Red;
                                     
                                    cantidadPos = cantidadPos + 1;
                                }

                                if (res.TrimStart().Substring(0, 12) == "NO SE DETECT")
                                {
                                    lblN1.ForeColor = Color.Black;
                                    lblN2.ForeColor = Color.Black; 
                                    cantidadNega = cantidadNega + 1;
                                }
                            }


                            string chkvalida = "chkValida_P_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;
 
                            chkValida.Visible = false;                       




                        }

                        /// alplex: desde aca

                    
                    }
                    catch { }
 break;
                } // foreach

            } // for int
            lblPosi.Text = cantidadPos.ToString();
                lblNega.Text = cantidadNega.ToString();
                lblIndeterminado.Text = cantidadInd.ToString();
                lblTotal.Text = (cantidadPos + cantidadNega + cantidadInd).ToString();
            
        }


        private void MostrarControlesPlaca()
        {      
            string m_strSQL = @"select celda, substring(valorCT,0,5), substring(tipoCT,0,5) from LAB_PlacaControl where idPlaca =" + HdIdPlaca.Value;       


            hdProcesaArchivo.Value = "N";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                string celda = Ds.Tables[0].Rows[i][0].ToString();
                string valorCT= Ds.Tables[0].Rows[i][1].ToString();             
                string tipoCT = Ds.Tables[0].Rows[i][2].ToString();
                string lblprotocolo = "lbl_P_" + celda;
                            
                 Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblprotocolo);
                 Label txtNumero = (Label)control1;
                 txtNumero.Text = tipoCT  + "="+ valorCT ;
                 txtNumero.Visible = true;
                 txtNumero.ForeColor = Color.Red;     

            }  
           
        }

        private string GetCTDetalleProtocolo(string s_idProtocolo, string codigoitem, string tipo, int idDetalleProtocolo)
        {
           

            string m_strSQL = "";
            string ct = "";
            //int posi = 0;
            //int nega = 0;

           
            m_strSQL = @"select   d.resultadoCar
from LAB_DetalleProtocolo d
 inner join LAB_Item i on i.iditem = d.idSubItem
 where idprotocolo =" + s_idProtocolo+ "  and i.codigo in ('"+ codigoitem + "')";
            if (idDetalleProtocolo > 0)
                m_strSQL = @"select resultadoCar--substring(resultadoCar,0,13)
from LAB_DetalleProtocolo   
 where  idDetalleProtocolo =" + idDetalleProtocolo.ToString();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            if (tipo == "Alplex")
            {
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    string resCT = Ds.Tables[0].Rows[i][1].ToString();
                    if (resCT == "NaN")
                        resCT = "NaN.";

                    if (ct == "")
                        ct = Ds.Tables[0].Rows[i][0].ToString() + "=" + resCT;
                    else
                        ct = ct + "-" + Ds.Tables[0].Rows[i][0].ToString() + "=" + resCT;

                }
               
            }

            else //promega
            {


               
                   
 ct =   Ds.Tables[0].Rows[0][0].ToString();

               
          
            }


            return ct;
        }

      
        private string GetCT(string celda, string tipo)
        {

            string m_strSQL = "";
            string ct = "";
            int posi = 0;
            int nega = 0;

            m_strSQL = @"select tipoct ,  substring(valorct,1,4)     from LAB_ResultadoTemp where   celda='" + celda + "'  and  idPlaca=" + Request["idPlaca"].ToString();




            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            if (tipo == "Alplex")
            {             

                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    string resCt = Ds.Tables[0].Rows[i][1].ToString();
                    if ((resCt.ToUpper() != "NAN") && (resCt.ToUpper() != "UNDE"))
                        posi = posi + 1;

                    
                    if ((resCt.ToUpper() == "NAN") || (resCt.ToUpper() == "UNDE"))
                    {
                        nega = nega + 1;
                        resCt = "NaN.";
                    }

                    string tipoCT = Ds.Tables[0].Rows[i][0].ToString();
                    if (tipoCT.Length >= 3)
                        tipoCT = tipoCT.Substring(0, 3);

                    if (ct == "")
                        ct = tipoCT + "=" + resCt;
                    else
                        ct =ct+ "-"+ tipoCT + "=" + resCt;

                }
              //  logica para Alplex

                if (posi == 4)
                    ct = ct + ";SE DETECTA";
                else
                {
                    if ((posi == 1) && (nega == 3))
                        ct = ct + ";NO SE DETECTA";
                    else
                        ct = ct + ";INDETERMINADO";
                }
            }

            else //promega
            {
               

                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)

                {
                    if ((Ds.Tables[0].Rows[i][1].ToString() != "NaN") && (Ds.Tables[0].Rows[i][1].ToString() != "Unde"))
                        posi = posi + 1;


                    if ((Ds.Tables[0].Rows[i][1].ToString() == "NaN")|| (Ds.Tables[0].Rows[i][1].ToString() == "Unde"))
                        nega = nega + 1;


                    if (ct == "")
                        ct = Ds.Tables[0].Rows[i][0].ToString() + "=" + Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");
                    else
                        ct = ct + Ds.Tables[0].Rows[i][0].ToString() + " = " + Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");

                }
              
            }
            

            return ct;
        }

        private string GetCTMixto(string celda, string  valorRP, string tipo)
        {

            string m_strSQL = "";
            string ct = "";
            int posi = 0;
            int nega = 0;


            if (valorRP != "")
                ct = valorRP;
            else
            {
                m_strSQL = @"select tipoct ,  substring(valorct,1,4)     from LAB_ResultadoTemp 
                    where   celda='" + celda + "'  and  idPlaca=" + Request["idPlaca"].ToString();


                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
                if (tipo == "Alplex")
                {

                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        string resCt = Ds.Tables[0].Rows[i][1].ToString();
                        if ((resCt.ToUpper() != "NAN") && (resCt.ToUpper() != "UNDE"))
                            posi = posi + 1;


                        if ((resCt.ToUpper() == "NAN") || (resCt.ToUpper() == "UNDE"))
                        {
                            nega = nega + 1;
                            resCt = "NaN.";
                        }

                        //string tipoCT = Ds.Tables[0].Rows[i][0].ToString();
                        //if (tipoCT.Length >= 3)
                        //    tipoCT = tipoCT.Substring(0, 3);

                        //if (ct == "")
                        //    ct = tipoCT + "=" + resCt;
                        //else
                        //    ct = ct + "-" + tipoCT + "=" + resCt;

                    }
                    //  logica para Alplex

                  
                }

                else //promega
                {
                    //Mixto

                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)

                    {
                        if ((Ds.Tables[0].Rows[i][1].ToString() != "NaN") && (Ds.Tables[0].Rows[i][1].ToString() != "Unde"))
                            posi = posi + 1;


                        if ((Ds.Tables[0].Rows[i][1].ToString() == "NaN") || (Ds.Tables[0].Rows[i][1].ToString() == "Unde"))
                            nega = nega + 1;


                        //if (ct == "")
                        //    ct = Ds.Tables[0].Rows[i][0].ToString() + "=" + Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");
                        //else
                        //    ct = ct + Ds.Tables[0].Rows[i][0].ToString() + " = " + Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");

                        ct =   Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");

                    }

                }

            }
            return ct;
        }
        private string getNumeroProtocolo(int idDetalleProtocolo)
        {
            try
            {

                Utility oUtil = new Utility();
                string datosProtocolo = "";
                Business.Data.Laboratorio.DetalleProtocolo oP = new Business.Data.Laboratorio.DetalleProtocolo();
                oP = (Business.Data.Laboratorio.DetalleProtocolo)oP.Get(typeof(Business.Data.Laboratorio.DetalleProtocolo), idDetalleProtocolo);
                if (oP != null)
                {
                    string efector = oP.IdProtocolo.IdEfectorSolicitante.Nombre.ToUpper().Replace("HOSPITAL", "").Replace("H.", "").Replace("El ", "").Replace("LA ", "").Replace("C.S.","");
                    if (efector.Length > 12)
                        efector = efector.Substring(0, 12);
                    

                    datosProtocolo = oP.IdProtocolo.Numero.ToString() + ";" + efector;

                    
                    if (oP.IdProtocolo.FechaInicioSintomas.Year != 1900)

                    {
                        datosProtocolo = datosProtocolo + ";FIS:" + oP.IdProtocolo.FechaInicioSintomas.ToShortDateString();
                      

                    }
                    else
                        datosProtocolo = datosProtocolo + ";FIS:SD";

                    if (oP.IdProtocolo.FechaUltimoContacto.Year != 1900)

                    {
                        datosProtocolo = datosProtocolo + ";FUC:" + oP.IdProtocolo.FechaInicioSintomas.ToShortDateString();


                    }
                    else
                        datosProtocolo = datosProtocolo + ";FUC:SD";

                    datosProtocolo += ";" + oP.IdProtocolo.IdProtocolo.ToString();
                    datosProtocolo += "; " + oP.ResultadoCar;

                }
                return datosProtocolo;
            }
            catch { return ""; }



        }
        private void ProcesarFichero()
        {
            try
            {
                int lineaDesde = 0;
                string modelo = "";
                if (this.trepador.HasFile)
                {
                    string filename = this.trepador.PostedFile.FileName;
                   
                    BorrarResultadosTemporales(); BorrarControles();
                    int i = 1;
                    if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                    {
                        string line;
                        StringBuilder log = new StringBuilder();
                        Stream stream = this.trepador.FileContent;
                        string extension = filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper();


                        using (StreamReader sr = new StreamReader(stream))
                        {
                            while (i <= 1000)//((!string.IsNullOrEmpty(line = sr.ReadLine()) && (i > 10)))

                            { 
                                    line = sr.ReadLine();

                                if (i == 1) // analizo la primer linea para ver si es ABI o CFX
                                {
                                    if (line.Contains("File Name")) // es cfx y se lee desde linea 21
                                    {
                                        lineaDesde = 21;
                                        modelo = "CFX";
                                        HdModelo.Value = "CFX";
                                    }
                                    if (line.Contains("Document Name")) // es ABI y se lee desde linea 26
                                    {
                                        if (lblEquipo.Text.ToUpper()=="ALPLEX")
                                            lineaDesde = 27;
                                        else
                                            lineaDesde = 26;
                                        modelo = "ABI";
                                        HdModelo.Value = "ABI";
                                    }
                                }


                                if (i >= lineaDesde) 
                                {
                                    ProcesarLinea(line, modelo);
                                }
                              
                                i += 1;
                            }
                        }
                    }
                }
            }

            catch { }
        }

        private void BorrarResultadosTemporales()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"DELETE FROM [dbo].[LAB_ResultadoTemp] where idUsuario= "+ Session["idUsuarioValida"].ToString()+  " and [idPLACA]=" + Request["idPlaca"].ToString();
            SqlCommand cmd = new SqlCommand(query, conn);


            int idres = Convert.ToInt32(cmd.ExecuteScalar());
           


        }
        private void BorrarControles()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            string query = @"DELETE FROM LAB_PlacaControl where   [idPLACA]=" + Request["idPlaca"].ToString();
            SqlCommand cmd = new SqlCommand(query, conn);


            int idres = Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void ProcesarLinea(string linea, string modelo)
        {
            try
            {
                string[] arr = linea.Split((",").ToCharArray());
                bool grabar = true;

                if (arr.Length >= 1)
                {


                    string celda = arr[0].ToString();
                    string tipoCt = arr[1].ToString();
                    string valorCt = arr[5].ToString();

                    if (modelo=="ABI")
                    {
                          tipoCt = arr[3].ToString();
                          valorCt = arr[4].ToString();
                        if (lblEquipo.Text.ToUpper() == "ALPLEX")
                        {
                            tipoCt = arr[2].ToString();
                            
                        }
                    }

                   
                        // para alplex
                        string escontrol = arr[3].ToString();
 if (!celda.Contains("10"))
                            celda = celda.Replace("0", "");
                    if ((escontrol.Contains("NTC")) || (escontrol.Contains("Standar")) || (escontrol.Contains("Pos Ctrl")))
                    {
                       

                         grabar = false;
                        SqlConnection conn2 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                        string query = @"INSERT INTO [dbo].[LAB_PlacaControl]
           ([idPlaca]
           ,[celda]
           ,[tipoCT]
           ,[valorCT]
           ,[idUsuario]
           ,[fechaRegistro])
     VALUES
           ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + tipoCt + "','" + valorCt + "' , " + Session["idUsuarioValida"].ToString() + ", getdate()     )";
                        SqlCommand cmd = new SqlCommand(query, conn2);


                        int idres2 = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (grabar)
                    {
                        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                        string query = @"
INSERT INTO [dbo].[LAB_ResultadoTEmp]
           ([idPlaca]
           ,[celda]
           ,[tipoCT]
           ,[valorCT]
,idUsuario
            )
     VALUES
           ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + tipoCt + "','" + valorCt + "' , "+ Session["idUsuarioValida"].ToString()+ "     )";
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

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
              

                Guardar(  );
                MostrarDatosPlaca();

            }
         }


        private void ValidacionUsuario()
        {
            //////////////////Se controla quien es el usuario que está por validar////////////////
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if ((oCon.AutenticaValidacion) && (Request["logIn"] == null)) Session["idUsuarioValida"] = null;
            if ((oCon.AutenticaValidacion) && (Session["idUsuarioValida"] == null))
            //    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);
            {
                //if ((Request["urgencia"] != null) && (oCon.AutenticaValidacion) && (Request["idUsuarioValida"] == null))
                string sredirect = "../Login.aspx?idServicio=3&Operacion=Valida&idPlaca="+ Request["idPlaca"].ToString(); // + id.Value;
                if (Request["Desde"].ToString() != null)
                    sredirect += "&desde=Valida";// + Desde.Value;

                Response.Redirect(sredirect);
            }
            else
            {

                //Session["idUsuarioValida"] = Session["idUsuario"];
              //  btnValidar.Visible = false;
             //   btnValidar.Text = "Confirmar Validacion";
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        private void Guardar( )
        {
            if (Request["Operacion"].ToString() == "Carga")
            { if (Session["idUsuario"] == null) Response.Redirect("../FinSesion.aspx", false); }

            if (Request["Operacion"].ToString() == "Valida")   //Validacion
            { if (Session["idUsuarioValida"] == null) Response.Redirect("../FinSesion.aspx", false); }


            string m_id = "";
          
            try
            {

                Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
                if (Request["idPlaca"] != null) oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["idPlaca"].ToString()));

                DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
                crit.Add(Expression.Eq("IdPlaca", oRegistro.IdPlaca));



                IList items = crit.List();

                foreach (DetalleProtocoloPlaca oDet in items)
                {
                    string celda = "";
                    string m_idChk = "";
                    string resultado = "";
                    string valorCT = "";
                    string nombreCT = "";

                    string pnl = "";
                    if (oRegistro.Equipo == "Alplex")
                    {
                        pnl = "pnlAlplex";
                        if (oDet.NamespaceID.Length < 3)
                            celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                        else
                            celda = oDet.NamespaceID;
                        string chkvalida = "chkValida_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;


                        m_id = chkValida.ToolTip;
                        m_idChk = chkValida.ID;

                        string m_valor = chkValida.ID.Replace("chkValida_P_", "lbl_P_");
                        m_valor = m_valor.Replace("Res", "");
                        Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(m_valor);
                        Label oRes = (Label)control10;
                        resultado = oRes.Text;
                    }


                    if (oRegistro.Equipo == "Promega-30M")
                    {
                        pnl = "pnlPromega2";

                        if (oDet.NamespaceID.Length == 4)
                            celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 1);
                        if (oDet.NamespaceID.Length == 5)
                            celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 2);

                        if ((oDet.NamespaceID.Substring(0, 1) == "D") || (oDet.NamespaceID.Substring(0, 1) == "E"))
                        { //"D123"
                            if (oDet.NamespaceID.Length < 6)
                            {
                                celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 3);
                              
                            }
                            if (oDet.NamespaceID.Length > 6)
                            {//"D101112"
                                celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 6);
                                
                            }
                        }

                            string chkvalida = "chkValida_P_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;


                        m_id = chkValida.ToolTip; //detalleprotocolo
                        m_idChk = chkValida.ID; ///celdda ABC_123

                         
                        string m_valor = chkValida.ID.Replace("chkValida_P_", "lbl_P_");
                        m_valor = m_valor.Replace("Res", "");

                        Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(m_valor);
                        Label oRes = (Label)control10;
                        resultado = oRes.Text;
                    }


                    if (oRegistro.Equipo == "PromegaM")
                    {
                        pnl = "pnlPromega";

                        celda = oDet.NamespaceID;
                        //if (oDet.NamespaceID.Length == 3)
                        //    celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 1);
                        //if (oDet.NamespaceID.Length == 4)
                        //    celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 2);

                      

                        string chkvalida = "chkValida_P_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;


                        m_id = chkValida.ToolTip; //detalleprotocolo
                        m_idChk = chkValida.ID; ///celdda ABC_123
                        nombreCT = chkValida.ID.Replace("chkValida_P_","");
                        string m_valor = chkValida.ID.Replace("chkValida_P_", "lbl_P_");
                    
                        Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(m_valor);
                        Label oResCT = (Label)control10;
                        valorCT = oResCT.Text;

                        //Guardo valor RP
                        string m_valorRP = "";
                        if ((celda.Substring(0, 1) == "A") || (celda.Substring(0, 1) == "B") || (celda.Substring(0, 1) == "C"))
                        {
                              m_valorRP = chkValida.ID.Replace("chkValida_P_", "lbl_P_D").Replace("A","").Replace("B","").Replace("C","");

                          
                        }
                        if ((celda.Substring(0, 1) == "E") || (celda.Substring(0, 1) == "F") || (celda.Substring(0, 1) == "G"))
                        {
                              m_valorRP = chkValida.ID.Replace("chkValida_P_", "lbl_P_H").Replace("E", "").Replace("F", "").Replace("G", "");

                        }
                        if (m_valorRP != "")
                        {
                            Control controlRP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(m_valorRP);
                            Label oResCTRP = (Label)controlRP;
                            string valorCTRP = oResCTRP.Text;                            
                            GuardarResultadoDetalleCT(m_id, "RP", valorCTRP);
                        }
                        // fin Valor RP

                        m_valor = m_valor + "Res";
                        Control control11 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(m_valor);
                        Label oRes = (Label)control11;
                        resultado = oRes.Text;

                     // Ver como guardar RP
                            //string lblctRP = "lbl_P_" + m_valor;
                            //Control controlrp = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctRP);
                            //Label lblRP = (Label)controlrp;
                            //GuardarResultadoDetalleCT(m_id, "RP", lblRP.Text);
                         
                    }

                    if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                    {
                        if (estatildado(m_idChk, pnl))
                        {

                            GuardarResultado(m_id, resultado);

                            nombreCT = nombreCT.Substring(0, 1);

                            //nombreCT ==> A Flu A- B Flu B- C N1
                            //E Flu A - F Flu B - G N1 
                            if (nombreCT == "E")
                                nombreCT = "A";
                            if (nombreCT == "F")
                                nombreCT = "B";

                            if ((nombreCT == "C") || (nombreCT == "G"))
                                nombreCT = "N1";

                           

                            GuardarResultadoDetalleCT(m_id, nombreCT, valorCT);

                          
                            //  GuardarResultadoCT(m_id, valorCT, oRegistro.Equipo);
                            //  GuardarReferenciaMetodoUnidadMedida(txt.ID, oProtocolo);
                        }
                       
                    }


                }
                oRegistro.Estado = "V"; // validada la placa

                oRegistro.Save();
                oRegistro.GrabarAuditoria("Valida Placa", int.Parse(Session["idUsuarioValida"].ToString()), "");

            }
            catch
            { }
        }

   


    private void GuardarResultadoDetalleCT(string m_id, string nombreItem, string valor)
        {
            DetalleProtocolo oRegistro = new DetalleProtocolo();
             oRegistro = (DetalleProtocolo)oRegistro.Get(typeof(DetalleProtocolo), int.Parse(m_id));

            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item),"Codigo", nombreItem);


            /// se fija si ya existe el CT, si no lo crea
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro.IdProtocolo));
            crit.Add(Expression.Eq("IdSubItem", oItem));            
            IList detalle = crit.List();



            if (detalle.Count > 0)
            { ///delete 

                foreach (DetalleProtocolo oDetalle1 in detalle)
                {

                    oDetalle1.IdProtocolo = oRegistro.IdProtocolo;
                    oDetalle1.IdEfector = oRegistro.IdEfector;
                    oDetalle1.IdItem = oItem;
                    oDetalle1.IdSubItem = oItem;
                    oDetalle1.TrajoMuestra = "Si";
                    oDetalle1.Informable = oItem.Informable;
                    oDetalle1.ResultadoCar = valor;
                    oDetalle1.ConResultado = true;

                    if (oItem.Informable)
                    {
                        oDetalle1.FechaValida = DateTime.Now;


                        oDetalle1.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                    }
                    else
                    {
                        oDetalle1.FechaValida = DateTime.Parse("01/01/1900");


                        oDetalle1.IdUsuarioValida = 0;
                    }
                    oDetalle1.FechaResultado = DateTime.Parse("01/01/1900");
                    oDetalle1.FechaValida = DateTime.Now;
                    oDetalle1.FechaControl = DateTime.Parse("01/01/1900");
                    oDetalle1.FechaImpresion = DateTime.Parse("01/01/1900");
                    oDetalle1.FechaEnvio = DateTime.Parse("01/01/1900");
                    oDetalle1.FechaObservacion = DateTime.Parse("01/01/1900");
                    oDetalle1.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                    oDetalle1.FechaPreValida = DateTime.Parse("01/01/1900");


                    oDetalle1.Save();
                    oDetalle1.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuarioValida"].ToString()));
                }

            }
            else
            {
                DetalleProtocolo oDetalle = new DetalleProtocolo();
                oDetalle.IdProtocolo = oRegistro.IdProtocolo;
                oDetalle.IdEfector = oRegistro.IdEfector;
                oDetalle.IdItem = oItem;
                oDetalle.IdSubItem = oItem;
                oDetalle.TrajoMuestra = "Si";
                oDetalle.Informable = oItem.Informable;
                oDetalle.ResultadoCar = valor;
                oDetalle.ConResultado = true;



                oDetalle.FechaResultado = DateTime.Parse("01/01/1900");
             
                oDetalle.FechaControl = DateTime.Parse("01/01/1900");
                oDetalle.FechaImpresion = DateTime.Parse("01/01/1900");
                oDetalle.FechaEnvio = DateTime.Parse("01/01/1900");
                oDetalle.FechaObservacion = DateTime.Parse("01/01/1900");
                oDetalle.FechaValidaObservacion = DateTime.Parse("01/01/1900");
                oDetalle.FechaPreValida = DateTime.Parse("01/01/1900");

 
                if (oItem.Informable)
                {
                    oDetalle.FechaValida = DateTime.Now;


                    oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                }
                else
                {
                    oDetalle.FechaValida = DateTime.Parse("01/01/1900");


                    oDetalle.IdUsuarioValida = 0;
                }

                oDetalle.Save();
                oDetalle.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuarioValida"].ToString()));
            }
            

        }

        private bool estatildado(string s_iditemcito, string pnl)
        {
            bool ok = false;
            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl(pnl).FindControl(s_iditemcito);
            CheckBox ochkformula = (CheckBox)control1;

            if (ochkformula != null)
            {
                if (ochkformula.Enabled) /// solo si está habilitado 
                    ok = ochkformula.Checked;
            }

            return ok;
        }

        private void GuardarResultado(string m_idDetalleProtocolo, string valorItem ) //, Protocolo oProtocolo, bool marcarImpresion, bool todo)
        {
            string m_metodo = "";
            string m_valorReferencia = "";
           

            //////////////////////////////////////////////////////////////////
            Item oItem = new Item();
            if (valorItem != "Seleccione")
            {
                //string[] arrIdItem = m_idItem.Split((";").ToCharArray());
                //m_idItem = arrIdItem[1].ToString();



                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                crit.Add(Expression.Eq("IdDetalleProtocolo", int.Parse(m_idDetalleProtocolo)));
                //crit.Add(Expression.Eq("IdUsuarioValida", 0));




                if (Request["Operacion"].ToString() == "Carga") crit.Add(Expression.Eq("IdUsuarioValida", 0));//Solo guarda resultados que no han sido validados
                if (Request["Operacion"].ToString() == "Control") crit.Add(Expression.Eq("IdUsuarioValida", 0));//Solo guarda resultados que no han sido validados

                IList detalle = crit.List();


                
                if (detalle.Count > 0)
                {
                    foreach (DetalleProtocolo oDetalle in detalle)
                    {

                       


                        switch (oDetalle.IdSubItem.IdTipoResultado)
                        {
                            case 1:// numerico         
                                if (valorItem != "")
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
                                    if (valorItem.ToUpper() == "SE DETECTA")
                                    {
                                        valorItem = "SE DETECT";
                                        oDetalle.ResultadoCar = oDetalle.IdSubItem.GetResultado(valorItem);
                                    }
                                    if (valorItem.ToUpper() == "NO SE DETECTA")
                                    {
                                      valorItem = "NO SE DETECT";
                                        oDetalle.ResultadoCar = oDetalle.IdSubItem.GetResultado(valorItem);
                                    }
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
                        //if ((valorRef != null) || (unMedida != null))
                        //{
                        //    if (valorRef != null)
                        //    {
                        int pres = oDetalle.IdSubItem.GetPresentacionEfector(oDetalle.IdEfector);

                        string[] arr =  oDetalle.CalcularValoresReferencia(pres).Split(("|").ToCharArray());
                        switch (arr.Length)
                        {
                            case 1: m_valorReferencia = arr[0].ToString(); break;
                            case 2:
                                {
                                    m_valorReferencia = arr[0].ToString();
                                    m_metodo = arr[1].ToString();
                                }
                                break;
                        }
                        oDetalle.Metodo = m_metodo;
                        oDetalle.ValorReferencia = m_valorReferencia;
                        //    }

                         //oDetalle.UnidadMedida = oDetalle.IdSubItem.getMetodoReferencia
                        //}
                        ///////////////////////////




                        if (Request["Operacion"].ToString() == "Carga")
                        {
                            if (oDetalle.ConResultado)
                            {
                                oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                                oDetalle.FechaResultado = DateTime.Now;
                            }
                            oDetalle.Save();
                            if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                        }
                        if ((Request["Operacion"].ToString() == "Valida") && (oDetalle.Informable))   //Validacion
                        {
                            string operacion = "Valida";

                            if (oDetalle.ConResultado)
                            {

                                string res = valorItem;
                                if (valorItem.Length > 10)
                                    res = valorItem.Substring(0, 10);

                                if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0) && (res.ToUpper() == "SE DETECTA"))// GENOMA DE COVID-19"))
                                {

                                    if (oCon.PreValida)
                                    {

                                        operacion = "PreValida";
                                        oDetalle.IdUsuarioPreValida = int.Parse(Session["idUsuarioValida"].ToString());
                                        oDetalle.FechaPreValida = DateTime.Now;
                                        oDetalle.IdUsuarioValida = 0;
                                        oDetalle.FechaValida = DateTime.Parse("01/01/1900");
                                    }
                                    else
                                    {
                                        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                                        oDetalle.FechaValida = DateTime.Now;


                                        Notificar(oDetalle);

                                    }
                                }
                                else
                                {

                                    oDetalle.IdUsuarioValida = int.Parse(Session["idUsuarioValida"].ToString());
                                    oDetalle.FechaValida = DateTime.Now;

                                    //if (marcarImpresion)
                                    //{
                                    //    oDetalle.IdUsuarioImpresion = int.Parse(Session["idUsuarioValida"].ToString());
                                    //    oDetalle.FechaImpresion = DateTime.Now;
                                    //}

                                    if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0))
                                        Notificar(oDetalle);

                                }
                                oDetalle.Save();
                                if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo(operacion, int.Parse(Session["idUsuarioValida"].ToString()));
                            }



                        }
                        if (Request["Operacion"].ToString() == "Control")   //Control
                        {
                            //if (estaTildado(m_idItem) && (oDetalle.ConResultado))
                            if (oDetalle.ConResultado)
                            {
                                oDetalle.IdUsuarioControl = int.Parse(Session["idUsuario"].ToString());
                                oDetalle.FechaControl = DateTime.Now;
                                oDetalle.Save();
                                if (oDetalle.ConResultado) oDetalle.GrabarAuditoriaDetalleProtocolo(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                            }
                        }

                        if (oDetalle.IdProtocolo.ValidadoTotal(Request["Operacion"].ToString(), int.Parse(Session["idUsuarioValida"].ToString())))
                        {
                            oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);             

                            if (!oDetalle.IdProtocolo.Notificarresultado)
                                oDetalle.IdProtocolo.Estado = 3; //Acceso Restringido    
                        }
                        else
                            oDetalle.IdProtocolo.Estado = 1;

                        oDetalle.IdProtocolo.Save();


                }




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
        private void Notificar(DetalleProtocolo oDetalle)
        {
            //bool notificasisa = oDetalle.NotificarSisa();

            if ((oDetalle.IdUsuarioValida > 0) && (oDetalle.IdProtocolo.Notificarresultado))
            {
                if ((oCon.NotificaAndes) && (oDetalle.IdItem.Codigo == oCon.CodigoCovid)) // solo para covid
                    GenerarNotificacionAndes(oDetalle);


                if (oCon.NotificarSISA)
                {
                    string idItem = oDetalle.IdProtocolo.GenerarCasoSISA(); // se fija si hay algun item que tiene configurado notificacion a sisa
                    if (idItem != "")
                    {
                        int i = 0;
                        //if (oDetalle.IdProtocolo.IdCaracter != 2) // no se suben controles de alta
                        //{
                        //if ((oDetalle.IdProtocolo.IdPaciente.IdEstado != 2))
                        //{
                        string res = oDetalle.ResultadoCar;

                        //if (oDetalle.IdProtocolo.VerificarProtocoloAnterior(14))
                        //{
                        if (res.Length > 10)
                        {
                            if ((res.Substring(0, 10) == "SE DETECTA") && (oCon.PreValida == false))
                            { if (ProcesaSISA(oDetalle, "SE DETECTA")) i = i + 1; }
                        }
                        if (res.Length > 13)
                        {
                            if (res.Substring(0, 13) == "NO SE DETECTA")
                            { if (ProcesaSISA(oDetalle, "NO SE DETECTA")) i = i + 1; }
                        }
                        /*  }// oDetalle.IdProtocolo.VerificarPr*/

                        //}//  if ((oDetalle.IdProtocolo.IdPacie
                        /*  }// if (oDetalle.IdProtocolo.IdCaracter != 2*/
                    }//    if (oCon.NotificarSISA)
                }
            }
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


                if (res == "SE DETECTA") // si es contacto y es positivo se sube como sospechoso
                    m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=1  and idItem=" + oDetalle.IdSubItem.IdItem.ToString();
                else
                {

                    m_strSQL = " select * from LAB_ConfiguracionSISA where idCaracter=  " + oDetalle.IdProtocolo.IdCaracter.ToString() + " and idItem=" + oDetalle.IdSubItem.IdItem.ToString();
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
                    bool hayerror = false;

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

                        usuario = s_user, // "bcpintos",
                        clave = s_userpass, // "2398HH6RK6",
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
                        if (respuesta_d.id_caso != "")
                        { //  devolver el idcaso para guardar en la base de datos
                            string s_idcaso = respuesta_d.id_caso;

                            oDetalle.IdProtocolo.IdCasoSISA = int.Parse(s_idcaso);
                            oDetalle.IdProtocolo.Save();
                            if (respuesta_d.resultado == "OK")
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Genera Caso SISA " + s_idcaso, int.Parse(Session["idUsuarioValida"].ToString()));
                            else
                                oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Actualiza Caso SISA " + s_idcaso, int.Parse(Session["idUsuarioValida"].ToString()));
                            generacaso = true;

                        }
                        else
                        {
                            generacaso = false;
                            hayerror = true;
                            error = respuesta_d.resultado;

                        }
                    }


                }

            }
            catch
            {
                generacaso = false;


            }
            return generacaso;

        }
        private bool ProcesaSISA(DetalleProtocolo oDetalle, string res)
        {
            bool generacaso = false;

            try
            {
                if (oDetalle.IdProtocolo.IdCasoSISA == 0)
                    generacaso = GenerarCasoSISA(oDetalle, res);





                string m_strSQL = @"SELECT  distinct idDetalleProtocolo,  S.idMuestra as IdMuestraSISA,	  S.idTipoMuestra as idTipoMuestraSISA, s.idPrueba as idPruebaSISA, s.idTipoPrueba as idTipoPruebaSISA,  
                ds.idResultadoSISA,S.idEvento
                  FROM    LAB_DetalleProtocolo d
                   inner join LAB_ConfiguracionSISA S on S.idCaracter=" + oDetalle.IdProtocolo.IdCaracter.ToString() + @" and s.idItem= d.idSubItem
                   inner join LAB_ConfiguracionSISADetalle DS on DS.idItem=d.idSubItem  and resultadocar= ds.resultado
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
            string URL = oCon.URLMuestraSISA;



            string ftoma = protocolo.FechaTomaMuestra.ToString("yyyy-MM-dd");//.ToShortDateString("yyyy/MM/dd").Replace("/", "-");

            string idestablecimientotoma = protocolo.IdEfectorSolicitante.CodigoSISA;
            if (idestablecimientotoma == "")
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
        private void GenerarResultadoSISA(DetalleProtocolo oDetalle, string idPruebaSISA, string idTipoPruebaSISA, string idResultadoSISA, string idEventoSISA)

        {

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

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaList.aspx", false);

        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir(HdIdPlaca.Value);  

        }
        private void Imprimir(string id)
        {


            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(id));

            CrystalReportSource oCr = new CrystalReportSource();


            switch (oRegistro.Equipo)
            {
                case "Promega":

                    {
                        oCr.Report.FileName = "PlacaPromegaRes.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldasResultado());

                    }
                    break;
                case "Alplex":
                    {
                        oCr.Report.FileName = "PlacaAlplexRes.rpt";
                        if (hdProcesaArchivo.Value == "S")
                            oCr.ReportDocument.SetDataSource(oRegistro.getCeldasArchivo());
                        else
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldasResultado());

                    }
                    break;
                case "Promega-30M":

                    {
                        oCr.Report.FileName = "PlacaPromega30MRes.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldasResultado());

                    }
                    break;
            }



            oCr.DataBind();

            oRegistro.GrabarAuditoria("Imprime Placa", int.Parse(Session["idUsuario"].ToString()), "");
            string nombrearchivo = "Placa_" + oRegistro.IdPlaca.ToString() + "_" + oRegistro.Fecha.ToShortDateString().Replace("/", "");


            oCr.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, nombrearchivo);


        }
        //private DataTable BuscarHClinica(int id) {
        //    SubSonic.Select h = new SubSonic.Select();
        //    h.From(DalSic.SysPaciente.Schema);
        //    h.Where(SysPaciente.Columns.HistoriaClinica).IsEqualTo(txtHC.Text);
        //    h.And(SysPaciente.Columns.IdPaciente).IsNotEqualTo(id);
        //    DataTable dt = h.ExecuteDataSet().Tables[0];
        //    return dt;
        //}

    }


}


