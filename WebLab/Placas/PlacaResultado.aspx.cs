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
    public partial class PlacaResultado : System.Web.UI.Page {
        Configuracion oCon = new Configuracion();
        private void Page_PreInit(object sender, System.EventArgs e)
       {
       
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

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
                        MostrarDatosPlacaDesdeArchivo2();
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

            MostrarPanel(oRegistro);

            /// lista de detalles diferentes

            string m_strSQL = @"select distinct iddetalleprotocolo from LAB_DetalleProtocoloPlaca (nolock) where idPlaca =" + oRegistro.IdPlaca.ToString();

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
                    if (oRegistro.Equipo == "Promega-30M")
                    {
                        string letra = "";
                        string controlCeldaN1 = "";
                        string controlCeldaN2 = "";
                        string controlCeldaRP = "";


                        if (oDet.NamespaceID.Length > 3)
                        {
                            if ((oDet.NamespaceID.Substring(0, 1) == "D") || (oDet.NamespaceID.Substring(0, 1) == "E"))
                            { //"D123"
                                if (oDet.NamespaceID.Length < 6)
                                {
                                    celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 3);
                                    letra = oDet.NamespaceID.Substring(0, 1);
                                    n1 = oDet.NamespaceID.Substring(1, 1);
                                    n2 = oDet.NamespaceID.Substring(2, 1);
                                    rp = oDet.NamespaceID.Substring(3, 1);

                                }

                                if (oDet.NamespaceID.Length > 6)
                                {//"D101112"
                                    celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 6);
                                    letra = oDet.NamespaceID.Substring(0, 1);
                                    n1 = oDet.NamespaceID.Substring(1, 2);
                                    n2 = oDet.NamespaceID.Substring(3, 2);
                                    rp = oDet.NamespaceID.Substring(5, 2);
                                }

                                celditaN1 = letra + n1;
                                celditaN2 = letra + n2;
                                celditaRP = letra + rp;

                                // completa con ceros si es CFX
                                if ((HdModelo.Value == "CFX") && (n1.Length == 1))
                                {


                                    celditaN1 = letra + "0" + n1;
                                    celditaN2 = letra + "0" + n2;
                                    celditaRP = letra + "0" + rp;

                                }
                                if ((HdModelo.Value == "ABI") && (n1.Length == 1))
                                {
                                    controlCeldaN1 = letra + "0" + n1;
                                    controlCeldaN2 = letra + "0" + n2;
                                    controlCeldaRP = letra + "0" + rp;
                                }
                                else
                                {
                                    controlCeldaN1 = celditaN1;
                                    controlCeldaN2 = celditaN2;
                                    controlCeldaRP = celditaRP;
                                }

                            }
                            else
                            {
                                if (oDet.NamespaceID.Length == 4)
                                    celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 1);
                                if (oDet.NamespaceID.Length == 5)
                                    celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 2);


                                n1 = oDet.NamespaceID.Substring(0, 1);
                                n2 = oDet.NamespaceID.Substring(1, 1);
                                rp = oDet.NamespaceID.Substring(2, 1);
                                if (oDet.NamespaceID.Length == 4)
                                    nro = oDet.NamespaceID.Substring(3, 1);
                                if (oDet.NamespaceID.Length == 5)
                                    nro = oDet.NamespaceID.Substring(3, 2);

                                celditaN1 = n1 + nro;
                                celditaN2 = n2 + nro;
                                celditaRP = rp + nro;


                                // completa con ceros si es CFX                               
                                if ((HdModelo.Value == "CFX") && (nro.Length == 1))
                                {

                                    celditaN1 = n1 + "0" + nro;
                                    celditaN2 = n2 + "0" + nro;
                                    celditaRP = rp + "0" + nro;

                                }
                                if ((HdModelo.Value == "ABI") && (nro.Length == 1))
                                {
                                    controlCeldaN1 = n1 + "0" + nro;
                                    controlCeldaN2 = n2 + "0" + nro;
                                    controlCeldaRP = rp + "0" + nro;
                                }
                                else
                                {
                                    controlCeldaN1 = celditaN1;
                                    controlCeldaN2 = celditaN2;
                                    controlCeldaRP = celditaRP;
                                }
                            }

                            string[] arrct1 = GetCT(celditaN1, "Promega").Split((";").ToCharArray());
                            string lblctN1 = "lbl_" + controlCeldaN1;
                            Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN1);
                            Label lblN1 = (Label)controlN1;
                            lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                            lblN1.Visible = true;

                            string[] arrct2 = GetCT(celditaN2, "Promega").Split((";").ToCharArray());
                            string lblctN2 = "lbl_" + controlCeldaN2;
                            Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN2);
                            Label lblN2 = (Label)controlN2;
                            lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                            lblN2.Visible = true;

                            string[] arrct3 = GetCT(celditaRP, "Promega").Split((";").ToCharArray());
                            string lblctRP = "lbl_" + controlCeldaRP;
                            Control controlrp = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctRP);
                            Label lblRP = (Label)controlrp;
                            lblRP.Text = arrct3[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                            lblRP.Visible = true;


                            string lblprotocolo = "lbl_" + celda;

                            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblprotocolo);
                            Label txtNumero = (Label)control1;
                            string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                            txtNumero.Text = arr[0].ToString();

                            txtNumero.Visible = true;


                            string lbllugar = "lblLugar_" + celda;
                            string lblfIS = "lblFIS_" + celda;
                            Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lbllugar);
                            Label lblLugar = (Label)control2;
                            lblLugar.Text = arr[1].ToString();
                            lblLugar.Font.Bold = true;
                            lblLugar.Visible = true;

                            Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblfIS);
                            Label lblFIS = (Label)control3;
                            lblFIS.Text = arr[2].ToString();
                            if (!arr[2].Contains("SD"))
                                lblFIS.ForeColor = Color.Blue;
                            else
                                lblFIS.ForeColor = Color.Red;
                            lblFIS.Visible = true;


                            string res = "";

                            if ((lblN1.Text != "NaN") && (lblN2.Text != "NaN") && (lblRP.Text != "NaN"))
                            { res = "SE DETECTA"; }
                            else
                            {
                                if ((lblN1.Text == "NaN") && (lblN2.Text == "NaN") && (lblRP.Text != "NaN"))
                                    res = "NO SE DETECTA";
                                else
                                    res = "INDETERMINADO";
                            }

                            if (lblN1.Text == "NaN")
                                lblN1.Text = "";
                            if (lblN2.Text == "NaN")
                                lblN2.Text = "";
                            if (lblRP.Text == "NaN")
                                lblRP.Text = "";
                            string lblRes = "lblResultado_" + celda;
                            Control controlres = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblRes);
                            Label lblResultado = (Label)controlres;
                            lblResultado.Text = res;
                            lblResultado.Visible = true;


                            string chkvalida = "chkValida_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                            chkValida.Visible = true;

                            if (oDet.EstaValidado())
                                chkValida.Visible = false;


                            if (lblResultado.Text == "SE DETECTA")
                            {
                                lblN1.ForeColor = Color.Red;
                                lblN2.ForeColor = Color.Red;
                                lblRP.ForeColor = Color.Red;
                                lblResultado.ForeColor = Color.Red;
                                cantidadPos = cantidadPos + 1;
                            }
                            else
                          if (lblResultado.Text == "NO SE DETECTA")
                            {
                                lblN1.ForeColor = Color.Black;
                                lblN2.ForeColor = Color.Black;
                                lblRP.ForeColor = Color.Black;
                                lblResultado.ForeColor = Color.Black;
                                cantidadNega = cantidadNega + 1;
                            }
                            else

                            { // INDETERMINADO: NO SE VALIDA
                                lblResultado.ForeColor = Color.Blue;
                                cantidadInd = cantidadInd + 1;
                                chkValida.Visible = false;
                            }

                        }
                    }

                    if (oRegistro.Equipo == "Promega")
                    {

                        string controlCeldaN1 = "";
                        string controlCeldaN2 = "";
                        string controlCeldaRP = "";


                        if (oDet.NamespaceID.Length == 3)
                            celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 4)
                            celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 2);

                        n1 = oDet.NamespaceID.Substring(0, 1);
                        n2 = oDet.NamespaceID.Substring(1, 1);
                        //rp = oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 3)
                            nro = oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 4)
                            nro = oDet.NamespaceID.Substring(2, 2);


                        celditaN1 = n1 + nro;
                        celditaN2 = n2 + nro;


                        if (nro.Length == 1)
                        {
                            celditaN1 = n1 + "0" + nro;
                            celditaN2 = n2 + "0" + nro;
                            //celditaRP = rp + "0" + nro;
                        }

                        // completa con ceros si es CFX                               
                        if ((HdModelo.Value == "CFX") && (nro.Length == 1))
                        {

                            celditaN1 = n1 + "0" + nro;
                            celditaN2 = n2 + "0" + nro;
                            celditaRP = rp + "0" + nro;

                        }
                        if ((HdModelo.Value == "ABI") && (nro.Length == 1))
                        {

                            celditaN1 = n1 + nro;
                            celditaN2 = n2 + nro;

                            controlCeldaN1 = n1 + "0" + nro;
                            controlCeldaN2 = n2 + "0" + nro;
                            controlCeldaRP = rp + "0" + nro;
                        }
                        else
                        {
                            controlCeldaN1 = celditaN1;
                            controlCeldaN2 = celditaN2;
                            controlCeldaRP = celditaRP;
                        }


                        string[] arrct1 = GetCT(celditaN1, "Promega").Split((";").ToCharArray());
                        string lblctN1 = "lbl_P_" + controlCeldaN1;
                        Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN1);
                        Label lblN1 = (Label)controlN1;
                        lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                        lblN1.Visible = true;

                        string[] arrct2 = GetCT(celditaN2, "Promega").Split((";").ToCharArray());
                        string lblctN2 = "lbl_P_" + controlCeldaN2;
                        Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN2);
                        Label lblN2 = (Label)controlN2;
                        lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                        lblN2.Visible = true;


                        string lblprotocolo = "lbl_" + celda;

                        Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblprotocolo);
                        Label txtNumero = (Label)control1;
                        string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                        txtNumero.Text = arr[0].ToString();

                        txtNumero.Visible = true;


                        string lbllugar = "lblLugar_" + celda;
                        string lblfIS = "lblFIS_" + celda;
                        Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lbllugar);
                        Label lblLugar = (Label)control2;
                        lblLugar.Text = arr[1].ToString();
                        lblLugar.Font.Bold = true;
                        lblLugar.Visible = true;

                        Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblfIS);
                        Label lblFIS = (Label)control3;
                        lblFIS.Text = arr[2].ToString();
                        if (!arr[2].Contains("SD"))
                            lblFIS.ForeColor = Color.Blue;
                        else
                            lblFIS.ForeColor = Color.Red;
                        lblFIS.Visible = true;
                        string res = "";

                        if ((lblN1.Text != "NaN") && (lblN2.Text != "NaN"))
                        { res = "SE DETECTA"; }
                        else
                        {
                            if ((lblN1.Text == "NaN") && (lblN2.Text != "NaN"))
                                res = "NO SE DETECTA";
                            else
                                res = "INDETERMINADO";
                        }

                        if (lblN1.Text == "NaN")
                            lblN1.Text = "";
                        if (lblN2.Text == "NaN")
                            lblN2.Text = "";

                        string lblRes = "lblResultado_P_" + celda;
                        Control controlres = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblRes);
                        Label lblResultado = (Label)controlres;
                        lblResultado.Text = res;
                        lblResultado.Visible = true;

                        string chkvalida = "chkValida_P_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;
                        chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                        chkValida.Visible = true;

                        if (oDet.EstaValidado())
                            chkValida.Visible = false;


                        if (lblResultado.Text == "SE DETECTA")
                        {
                            lblN1.ForeColor = Color.Red;
                            lblN2.ForeColor = Color.Red;

                            lblResultado.ForeColor = Color.Red;
                            cantidadPos = cantidadPos + 1;
                        }
                        else
                      if (lblResultado.Text == "NO SE DETECTA")
                        {
                            lblN1.ForeColor = Color.Black;
                            lblN2.ForeColor = Color.Black;

                            lblResultado.ForeColor = Color.Black;
                            cantidadNega = cantidadNega + 1;
                        }
                        else

                        { // INDETERMINADO: NO SE VALIDA
                            lblResultado.ForeColor = Color.Blue;
                            cantidadInd = cantidadInd + 1;
                            chkValida.Visible = false;
                        }
                    }
                    /// alplex: desde aca


                    if (oRegistro.Equipo == "Alplex")
                    {
                        string celdaCT = ""; // es como viene en elarchivo

                        //celda=  es el control

                        if (oDet.NamespaceID.Length < 3)
                        {
                            celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                            celdaCT = celda;
                            if (HdModelo.Value == "ABI")
                                celdaCT = oDet.NamespaceID;
                        }
                        else
                        {
                            celda = oDet.NamespaceID;
                            celdaCT = celda;
                        }




                        string lblprotocolo = "lblProtocolo_" + celda;
                        string lbllugar = "lblLugar_" + celda;
                        string lblfIS = "lblFIS_" + celda;
                        string lblct = "lblCt1_" + celda;
                        string lblresultado = "lblResultado_" + celda;
                        //string lbldias = "lblDias_" + celda;
                        string pnl = celda;
                        Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblprotocolo);
                        Label lblNumero = (Label)control1;


                        string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());

                        lblNumero.Text = arr[0].ToString();

                        Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lbllugar);
                        Label lblLugar = (Label)control2;

                        lblLugar.Text = arr[1].ToString();
                        lblLugar.Font.Bold = true;

                        Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblfIS);
                        Label lblFIS = (Label)control3;

                        lblFIS.Text = arr[2].ToString();
                        if (!arr[2].Contains("SD"))
                            lblFIS.ForeColor = Color.Green;
                        else
                            lblFIS.ForeColor = Color.Red;

                        string[] arrct = GetCT(celdaCT, "Alplex").Split((";").ToCharArray());

                        Control control4 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblct);
                        Label lblCt1 = (Label)control4;

                        lblCt1.Text = arrct[0].ToString();
                        if (arrct.Length > 1)
                        {
                            string chkvalida = "chkValida_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                            chkValida.Visible = true;

                            Control control5 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblresultado);
                            Label lblResultado = (Label)control5;

                            lblResultado.Text = arrct[1].ToString();
                            if (lblResultado.Text == "SE DETECTA")
                            {
                                lblCt1.ForeColor = Color.Red; ;
                                lblResultado.ForeColor = Color.Red;
                                cantidadPos = cantidadPos + 1;
                            }
                            else
                            if (lblResultado.Text == "NO SE DETECTA")
                            {
                                lblCt1.ForeColor = Color.Black; ;
                                lblResultado.ForeColor = Color.Black;
                                cantidadNega = cantidadNega + 1;
                            }
                            else

                            { // INDETERMINADO: NO SE VALIDA
                                lblCt1.ForeColor = Color.Blue;
                                lblResultado.ForeColor = Color.Blue;
                                cantidadInd = cantidadInd + 1;

                                chkValida.Visible = false;


                            }

                            if (oDet.EstaValidado())
                                chkValida.Visible = false;

                            //lblResultado.ForeColor = Color.Red;
                        }





                        Control controlP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(pnl);
                        Panel pnlito = (Panel)controlP;
                        pnlito.Visible = true;
                    }
                    //}
                    //catch { }
                    break;
                }

            }


            lblPosi.Text = cantidadPos.ToString();
            lblNega.Text = cantidadNega.ToString();
            lblIndeterminado.Text = cantidadInd.ToString();
            lblTotal.Text = (cantidadPos + cantidadNega + cantidadInd).ToString();

        }

        private void MostrarDatosPlacaDesdeArchivo2() //para alpex multiples determinaciones
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(HdIdPlaca.Value));
            lblNro.Text = oRegistro.IdPlaca.ToString();

            MostrarPanel(oRegistro);

            /// lista de detalles diferentes

            string m_strSQL = @"select distinct iddetalleprotocolo from LAB_DetalleProtocoloPlaca (nolock) where idPlaca =" + oRegistro.IdPlaca.ToString();

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
                    if (oRegistro.Equipo == "Promega-30M")
                    {
                        string letra = "";
                        string controlCeldaN1 = "";
                        string controlCeldaN2 = "";
                        string controlCeldaRP = "";


                        if (oDet.NamespaceID.Length > 3)
                        {
                            if ((oDet.NamespaceID.Substring(0, 1) == "D") || (oDet.NamespaceID.Substring(0, 1) == "E"))
                            { //"D123"
                                if (oDet.NamespaceID.Length < 6)
                                {
                                    celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 3);
                                    letra = oDet.NamespaceID.Substring(0, 1);
                                    n1 = oDet.NamespaceID.Substring(1, 1);
                                    n2 = oDet.NamespaceID.Substring(2, 1);
                                    rp = oDet.NamespaceID.Substring(3, 1);

                                }

                                if (oDet.NamespaceID.Length > 6)
                                {//"D101112"
                                    celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 6);
                                    letra = oDet.NamespaceID.Substring(0, 1);
                                    n1 = oDet.NamespaceID.Substring(1, 2);
                                    n2 = oDet.NamespaceID.Substring(3, 2);
                                    rp = oDet.NamespaceID.Substring(5, 2);
                                }

                                celditaN1 = letra + n1;
                                celditaN2 = letra + n2;
                                celditaRP = letra + rp;

                                // completa con ceros si es CFX
                                if ((HdModelo.Value == "CFX") && (n1.Length == 1))
                                {


                                    celditaN1 = letra + "0" + n1;
                                    celditaN2 = letra + "0" + n2;
                                    celditaRP = letra + "0" + rp;

                                }
                                if ((HdModelo.Value == "ABI") && (n1.Length == 1))
                                {
                                    controlCeldaN1 = letra + "0" + n1;
                                    controlCeldaN2 = letra + "0" + n2;
                                    controlCeldaRP = letra + "0" + rp;
                                }
                                else
                                {
                                    controlCeldaN1 = celditaN1;
                                    controlCeldaN2 = celditaN2;
                                    controlCeldaRP = celditaRP;
                                }

                            }
                            else
                            {
                                if (oDet.NamespaceID.Length == 4)
                                    celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 1);
                                if (oDet.NamespaceID.Length == 5)
                                    celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 2);


                                n1 = oDet.NamespaceID.Substring(0, 1);
                                n2 = oDet.NamespaceID.Substring(1, 1);
                                rp = oDet.NamespaceID.Substring(2, 1);
                                if (oDet.NamespaceID.Length == 4)
                                    nro = oDet.NamespaceID.Substring(3, 1);
                                if (oDet.NamespaceID.Length == 5)
                                    nro = oDet.NamespaceID.Substring(3, 2);

                                celditaN1 = n1 + nro;
                                celditaN2 = n2 + nro;
                                celditaRP = rp + nro;


                                // completa con ceros si es CFX                               
                                if ((HdModelo.Value == "CFX") && (nro.Length == 1))
                                {

                                    celditaN1 = n1 + "0" + nro;
                                    celditaN2 = n2 + "0" + nro;
                                    celditaRP = rp + "0" + nro;

                                }
                                if ((HdModelo.Value == "ABI") && (nro.Length == 1))
                                {
                                    controlCeldaN1 = n1 + "0" + nro;
                                    controlCeldaN2 = n2 + "0" + nro;
                                    controlCeldaRP = rp + "0" + nro;
                                }
                                else
                                {
                                    controlCeldaN1 = celditaN1;
                                    controlCeldaN2 = celditaN2;
                                    controlCeldaRP = celditaRP;
                                }
                            }

                            string[] arrct1 = GetCT(celditaN1, "Promega").Split((";").ToCharArray());
                            string lblctN1 = "lbl_" + controlCeldaN1;
                            Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN1);
                            Label lblN1 = (Label)controlN1;
                            lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                            lblN1.Visible = true;

                            string[] arrct2 = GetCT(celditaN2, "Promega").Split((";").ToCharArray());
                            string lblctN2 = "lbl_" + controlCeldaN2;
                            Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN2);
                            Label lblN2 = (Label)controlN2;
                            lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                            lblN2.Visible = true;

                            string[] arrct3 = GetCT(celditaRP, "Promega").Split((";").ToCharArray());
                            string lblctRP = "lbl_" + controlCeldaRP;
                            Control controlrp = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctRP);
                            Label lblRP = (Label)controlrp;
                            lblRP.Text = arrct3[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                            lblRP.Visible = true;


                            string lblprotocolo = "lbl_" + celda;

                            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblprotocolo);
                            Label txtNumero = (Label)control1;
                            string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                            txtNumero.Text = arr[0].ToString();

                            txtNumero.Visible = true;


                            string lbllugar = "lblLugar_" + celda;
                            string lblfIS = "lblFIS_" + celda;
                            Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lbllugar);
                            Label lblLugar = (Label)control2;
                            lblLugar.Text = arr[1].ToString();
                            lblLugar.Font.Bold = true;
                            lblLugar.Visible = true;

                            Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblfIS);
                            Label lblFIS = (Label)control3;
                            lblFIS.Text = arr[2].ToString();
                            if (!arr[2].Contains("SD"))
                                lblFIS.ForeColor = Color.Blue;
                            else
                                lblFIS.ForeColor = Color.Red;
                            lblFIS.Visible = true;


                            string res = "";

                            if ((lblN1.Text != "NaN") && (lblN2.Text != "NaN") && (lblRP.Text != "NaN"))
                            { res = "SE DETECTA"; }
                            else
                            {
                                if ((lblN1.Text == "NaN") && (lblN2.Text == "NaN") && (lblRP.Text != "NaN"))
                                    res = "NO SE DETECTA";
                                else
                                    res = "INDETERMINADO";
                            }

                            if (lblN1.Text == "NaN")
                                lblN1.Text = "";
                            if (lblN2.Text == "NaN")
                                lblN2.Text = "";
                            if (lblRP.Text == "NaN")
                                lblRP.Text = "";
                            string lblRes = "lblResultado_" + celda;
                            Control controlres = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblRes);
                            Label lblResultado = (Label)controlres;
                            lblResultado.Text = res;
                            lblResultado.Visible = true;


                            string chkvalida = "chkValida_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                            chkValida.Visible = true;

                            if (oDet.EstaValidado())
                                chkValida.Visible = false;


                            if (lblResultado.Text == "SE DETECTA")
                            {
                                lblN1.ForeColor = Color.Red;
                                lblN2.ForeColor = Color.Red;
                                lblRP.ForeColor = Color.Red;
                                lblResultado.ForeColor = Color.Red;
                                cantidadPos = cantidadPos + 1;
                            }
                            else
                          if (lblResultado.Text == "NO SE DETECTA")
                            {
                                lblN1.ForeColor = Color.Black;
                                lblN2.ForeColor = Color.Black;
                                lblRP.ForeColor = Color.Black;
                                lblResultado.ForeColor = Color.Black;
                                cantidadNega = cantidadNega + 1;
                            }
                            else

                            { // INDETERMINADO: NO SE VALIDA
                                lblResultado.ForeColor = Color.Blue;
                                cantidadInd = cantidadInd + 1;
                                chkValida.Visible = false;
                            }

                        }
                    }

                    if (oRegistro.Equipo == "Promega")
                    {

                        string controlCeldaN1 = "";
                        string controlCeldaN2 = "";
                        string controlCeldaRP = "";


                        if (oDet.NamespaceID.Length == 3)
                            celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 4)
                            celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 2);

                        n1 = oDet.NamespaceID.Substring(0, 1);
                        n2 = oDet.NamespaceID.Substring(1, 1);
                        //rp = oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 3)
                            nro = oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 4)
                            nro = oDet.NamespaceID.Substring(2, 2);


                        celditaN1 = n1 + nro;
                        celditaN2 = n2 + nro;


                        if (nro.Length == 1)
                        {
                            celditaN1 = n1 + "0" + nro;
                            celditaN2 = n2 + "0" + nro;
                            //celditaRP = rp + "0" + nro;
                        }

                        // completa con ceros si es CFX                               
                        if ((HdModelo.Value == "CFX") && (nro.Length == 1))
                        {

                            celditaN1 = n1 + "0" + nro;
                            celditaN2 = n2 + "0" + nro;
                            celditaRP = rp + "0" + nro;

                        }
                        if ((HdModelo.Value == "ABI") && (nro.Length == 1))
                        {

                            celditaN1 = n1 + nro;
                            celditaN2 = n2 + nro;

                            controlCeldaN1 = n1 + "0" + nro;
                            controlCeldaN2 = n2 + "0" + nro;
                            controlCeldaRP = rp + "0" + nro;
                        }
                        else
                        {
                            controlCeldaN1 = celditaN1;
                            controlCeldaN2 = celditaN2;
                            controlCeldaRP = celditaRP;
                        }


                        string[] arrct1 = GetCT(celditaN1, "Promega").Split((";").ToCharArray());
                        string lblctN1 = "lbl_P_" + controlCeldaN1;
                        Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN1);
                        Label lblN1 = (Label)controlN1;
                        lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                        lblN1.Visible = true;

                        string[] arrct2 = GetCT(celditaN2, "Promega").Split((";").ToCharArray());
                        string lblctN2 = "lbl_P_" + controlCeldaN2;
                        Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN2);
                        Label lblN2 = (Label)controlN2;
                        lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("Unknown=", "");
                        lblN2.Visible = true;


                        string lblprotocolo = "lbl_" + celda;

                        Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblprotocolo);
                        Label txtNumero = (Label)control1;
                        string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                        txtNumero.Text = arr[0].ToString();

                        txtNumero.Visible = true;


                        string lbllugar = "lblLugar_" + celda;
                        string lblfIS = "lblFIS_" + celda;
                        Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lbllugar);
                        Label lblLugar = (Label)control2;
                        lblLugar.Text = arr[1].ToString();
                        lblLugar.Font.Bold = true;
                        lblLugar.Visible = true;

                        Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblfIS);
                        Label lblFIS = (Label)control3;
                        lblFIS.Text = arr[2].ToString();
                        if (!arr[2].Contains("SD"))
                            lblFIS.ForeColor = Color.Blue;
                        else
                            lblFIS.ForeColor = Color.Red;
                        lblFIS.Visible = true;
                        string res = "";

                        if ((lblN1.Text != "NaN") && (lblN2.Text != "NaN"))
                        { res = "SE DETECTA"; }
                        else
                        {
                            if ((lblN1.Text == "NaN") && (lblN2.Text != "NaN"))
                                res = "NO SE DETECTA";
                            else
                                res = "INDETERMINADO";
                        }

                        if (lblN1.Text == "NaN")
                            lblN1.Text = "";
                        if (lblN2.Text == "NaN")
                            lblN2.Text = "";

                        string lblRes = "lblResultado_P_" + celda;
                        Control controlres = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblRes);
                        Label lblResultado = (Label)controlres;
                        lblResultado.Text = res;
                        lblResultado.Visible = true;

                        string chkvalida = "chkValida_P_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;
                        chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                        chkValida.Visible = true;

                        if (oDet.EstaValidado())
                            chkValida.Visible = false;


                        if (lblResultado.Text == "SE DETECTA")
                        {
                            lblN1.ForeColor = Color.Red;
                            lblN2.ForeColor = Color.Red;

                            lblResultado.ForeColor = Color.Red;
                            cantidadPos = cantidadPos + 1;
                        }
                        else
                      if (lblResultado.Text == "NO SE DETECTA")
                        {
                            lblN1.ForeColor = Color.Black;
                            lblN2.ForeColor = Color.Black;

                            lblResultado.ForeColor = Color.Black;
                            cantidadNega = cantidadNega + 1;
                        }
                        else

                        { // INDETERMINADO: NO SE VALIDA
                            lblResultado.ForeColor = Color.Blue;
                            cantidadInd = cantidadInd + 1;
                            chkValida.Visible = false;
                        }
                    }
                    /// alplex: desde aca


                    if (oRegistro.Equipo.Substring(0,6) == "Alplex")//valido para alplex de 3 virus y de 7
                    {
                        string celdaCT = ""; // es como viene en elarchivo
                      
                        //celda=  es el control

                        if (oDet.NamespaceID.Length < 3)
                        { celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                            celdaCT = celda;
                            if (HdModelo.Value == "ABI")
                                celdaCT = oDet.NamespaceID;
                        }
                        else
                        { celda = oDet.NamespaceID;
                            celdaCT = celda;
                        }
                        
                    



                        string lblprotocolo = "lblProtocolo_" + celda;
                        string lbllugar = "lblLugar_" + celda;
                        string lblfIS = "lblFIS_" + celda;
                        string lblct = "lblCt1_" + celda;
                        string lblresultado = "lblResultado_" + celda;
                        //string lbldias = "lblDias_" + celda;
                        string pnl = celda;
                        Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblprotocolo);
                        Label lblNumero = (Label)control1;


                        string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());

                        lblNumero.Text = arr[0].ToString();

                        Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lbllugar);
                        Label lblLugar = (Label)control2;

                        lblLugar.Text = arr[1].ToString();
                        lblLugar.Font.Bold = true;

                        Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblfIS);
                        Label lblFIS = (Label)control3;

                        lblFIS.Text = arr[2].ToString();
                        if (!arr[2].Contains("SD"))
                            lblFIS.ForeColor = Color.Green;
                        else
                            lblFIS.ForeColor = Color.Red;
                        ///Caro
                        string[] arrct = GetCT(celdaCT, "Alplex",oDet.IdDetalleProtocolo, oDet.IdPlaca).Split((";").ToCharArray());

                        Control control4 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblct);
                        Label lblCt1 = (Label)control4;
                        if  (lblCt1.Text!="")
                            lblCt1.Text += "\n" + arrct[0].ToString();
                        else                        
                            lblCt1.Text  =  arrct[0].ToString();

                        if (arrct.Length > 1)
                        {
                            string chkvalida = "chkValida_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = lblNumero.Text; //numero de protocolo oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                            chkValida.Visible = true;

                            Control control5 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblresultado);
                            Label lblResultado = (Label)control5;
                            if  (lblResultado.Text=="")

                                lblResultado.Text = arrct[1].ToString();
                            else
                                 lblResultado.Text +="\n" + arrct[1].ToString();
                            if (lblResultado.Text.Contains ("=SE DET"))
                            {
                                lblCt1.ForeColor = Color.Red; ;
                                lblResultado.ForeColor = Color.Red;
                                cantidadPos = cantidadPos + 1;
                            }
                            else
                            if (lblResultado.Text.Contains("=NO SE DET"))
                            {
                                lblCt1.ForeColor = Color.Black; ;
                                lblResultado.ForeColor = Color.Black;
                                cantidadNega = cantidadNega + 1;
                            }
                            else

                            { // INDETERMINADO: NO SE VALIDA
                                lblCt1.ForeColor = Color.Blue;
                                lblResultado.ForeColor = Color.Blue;
                                cantidadInd = cantidadInd + 1;

                                chkValida.Visible = false;


                            }

                            if (oDet.EstaValidado())
                                chkValida.Visible = false;

                            //lblResultado.ForeColor = Color.Red;
                        }





                        Control controlP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(pnl);
                        Panel pnlito = (Panel)controlP;
                        pnlito.Visible = true;
                    }

               /*     if (oRegistro.Equipo == "Alplex7V")
                    {
                        string celdaCT = ""; // es como viene en elarchivo

                        //celda=  es el control

                        if (oDet.NamespaceID.Length < 3)
                        {
                            celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                            celdaCT = celda;
                            if (HdModelo.Value == "ABI")
                                celdaCT = oDet.NamespaceID;
                        }
                        else
                        {
                            celda = oDet.NamespaceID;
                            celdaCT = celda;
                        }





                        string lblprotocolo = "lblProtocolo_" + celda;
                        string lbllugar = "lblLugar_" + celda;
                        string lblfIS = "lblFIS_" + celda;
                        string lblct = "lblCt1_" + celda;
                        string lblresultado = "lblResultado_" + celda;
                        //string lbldias = "lblDias_" + celda;
                        string pnl = celda;
                        Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblprotocolo);
                        Label lblNumero = (Label)control1;


                        string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());

                        lblNumero.Text = arr[0].ToString();

                        Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lbllugar);
                        Label lblLugar = (Label)control2;

                        lblLugar.Text = arr[1].ToString();
                        lblLugar.Font.Bold = true;

                        Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblfIS);
                        Label lblFIS = (Label)control3;

                        lblFIS.Text = arr[2].ToString();
                        if (!arr[2].Contains("SD"))
                            lblFIS.ForeColor = Color.Green;
                        else
                            lblFIS.ForeColor = Color.Red;
                        ///Caro
                        string[] arrct = GetCT(celdaCT, "Alplex", oDet.IdDetalleProtocolo, oDet.IdPlaca).Split((";").ToCharArray());

                        Control control4 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblct);
                        Label lblCt1 = (Label)control4;
                        if (lblCt1.Text != "")
                            lblCt1.Text += "\n" + arrct[0].ToString();
                        else
                            lblCt1.Text = arrct[0].ToString();

                        if (arrct.Length > 1)
                        {
                            string chkvalida = "chkValida_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = lblNumero.Text; //numero de protocolo oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                            chkValida.Visible = true;

                            Control control5 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblresultado);
                            Label lblResultado = (Label)control5;
                            if (lblResultado.Text == "")

                                lblResultado.Text = arrct[1].ToString();
                            else
                                lblResultado.Text += "\n" + arrct[1].ToString();
                            if (lblResultado.Text.Contains("=SE DET"))
                            {
                                lblCt1.ForeColor = Color.Red; ;
                                lblResultado.ForeColor = Color.Red;
                                cantidadPos = cantidadPos + 1;
                            }
                            else
                            if (lblResultado.Text.Contains("=NO SE DET"))
                            {
                                lblCt1.ForeColor = Color.Black; ;
                                lblResultado.ForeColor = Color.Black;
                                cantidadNega = cantidadNega + 1;
                            }
                            else

                            { // INDETERMINADO: NO SE VALIDA
                                lblCt1.ForeColor = Color.Blue;
                                lblResultado.ForeColor = Color.Blue;
                                cantidadInd = cantidadInd + 1;

                                chkValida.Visible = false;


                            }

                            if (oDet.EstaValidado())
                                chkValida.Visible = false;

                            //lblResultado.ForeColor = Color.Red;
                        }





                        Control controlP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(pnl);
                        Panel pnlito = (Panel)controlP;
                        pnlito.Visible = true;
                    }*/
                    //}
                    //catch { }
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
                case "Alplex7V":
                    {
                        btnImprimir.Visible = true;
                        pnlPromega2.Visible = false;
                        pnlAlplex.Visible = true;
                        pnlPromega.Visible = false;
                        textoimprimir.Visible = false;
                        break;
                    }
                case "Promega":
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


            /// lista de detalles diferentes

            string m_strSQL =   @"select distinct iddetalleprotocolo from LAB_DetalleProtocoloPlaca (nolock) where idPlaca ="+ oRegistro.IdPlaca.ToString();

  int cantidadPos = 0; string res1 = "";
                int cantidadNega = 0;
                int cantidadInd = 0;
            string celdapivot = "";

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
                    string celditaRP = "";
                    try
                    {
                        if (oRegistro.Equipo == "Promega-30M")
                        {
                            string letra = "";

                            if (oDet.NamespaceID.Length > 3)
                            {
                                if ((oDet.NamespaceID.Substring(0, 1) == "D") || (oDet.NamespaceID.Substring(0, 1) == "E"))
                                { //"D123"
                                    if (oDet.NamespaceID.Length < 6)
                                    {
                                        celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 3);
                                        letra = oDet.NamespaceID.Substring(0, 1);
                                        n1 = oDet.NamespaceID.Substring(1, 1);
                                        n2 = oDet.NamespaceID.Substring(2, 1);
                                        rp = oDet.NamespaceID.Substring(3, 1);

                                    }

                                    if (oDet.NamespaceID.Length > 6)
                                    {//"D101112"
                                        celda = oDet.NamespaceID.Substring(0, 1) + "_" + oDet.NamespaceID.Substring(1, 6);
                                        letra = oDet.NamespaceID.Substring(0, 1);
                                        n1 = oDet.NamespaceID.Substring(1, 2);
                                        n2 = oDet.NamespaceID.Substring(3, 2);
                                        rp = oDet.NamespaceID.Substring(5, 2);
                                    }

                                    if (n1.Length == 1)
                                    {
                                        celditaN1 = letra + "0" + n1;
                                        celditaN2 = letra + "0" + n2;
                                        celditaRP = letra + "0" + rp;
                                    }
                                    else

                                    {
                                        celditaN1 = letra + n1;
                                        celditaN2 = letra + n2;
                                        celditaRP = letra + rp;
                                    }

                                }
                                else
                                {
                                    if (oDet.NamespaceID.Length == 4)
                                        celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 1);
                                    if (oDet.NamespaceID.Length == 5)
                                        celda = oDet.NamespaceID.Substring(0, 3) + "_" + oDet.NamespaceID.Substring(3, 2);

                                    n1 = oDet.NamespaceID.Substring(0, 1);
                                    n2 = oDet.NamespaceID.Substring(1, 1);
                                    rp = oDet.NamespaceID.Substring(2, 1);
                                    if (oDet.NamespaceID.Length == 4)
                                        nro = oDet.NamespaceID.Substring(3, 1);
                                    if (oDet.NamespaceID.Length == 5)
                                        nro = oDet.NamespaceID.Substring(3, 2);


                                    celditaN1 = n1 + nro;
                                    celditaN2 = n2 + nro;
                                    celditaRP = rp + nro;

                                    if (nro.Length == 1)
                                    {
                                        celditaN1 = n1 + "0" + nro;
                                        celditaN2 = n2 + "0" + nro;
                                        celditaRP = rp + "0" + nro;
                                    }
                                }



                                string lblprotocolo = "lbl_" + celda;

                                Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblprotocolo);
                                Label txtNumero = (Label)control1;
                                string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                                txtNumero.Text = arr[0].ToString();

                                txtNumero.Visible = true;

                                string s_idProtocolo = arr[4].ToString();

                                //    cambiar el getCT desde archivo por desde BD lab_detalleprotocolo 
                                string[] arrct1 = GetCTDetalleProtocolo(s_idProtocolo, "N1", "Promega").Split((";").ToCharArray());
                                string lblctN1 = "lbl_" + celditaN1;
                                Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN1);
                                Label lblN1 = (Label)controlN1;
                                lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("N1", "");
                                lblN1.Visible = true;

                                string[] arrct2 = GetCTDetalleProtocolo(s_idProtocolo, "N2", "Promega").Split((";").ToCharArray());
                                string lblctN2 = "lbl_" + celditaN2;
                                Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN2);
                                Label lblN2 = (Label)controlN2;
                                lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("N2", ""); ;
                                lblN2.Visible = true;

                                string[] arrct3 = GetCTDetalleProtocolo(s_idProtocolo, "RP", "Promega").Split((";").ToCharArray());
                                string lblctRP = "lbl_" + celditaRP;
                                Control controlrp = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctRP);
                                Label lblRP = (Label)controlrp;
                                lblRP.Text = arrct3[0].ToString().Replace("FAM=", "").Replace("RP", ""); ;
                                lblRP.Visible = true;



                                string lbllugar = "lblLugar_" + celda;
                                string lblfIS = "lblFIS_" + celda;
                                Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lbllugar);
                                Label lblLugar = (Label)control2;
                                lblLugar.Text = arr[1].ToString();
                                lblLugar.Font.Bold = true;
                                lblLugar.Visible = true;

                                Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblfIS);
                                Label lblFIS = (Label)control3;
                                lblFIS.Text = arr[2].ToString();

                                if (!arr[2].Contains("SD"))
                                    lblFIS.ForeColor = Color.Blue;
                                else
                                    lblFIS.ForeColor = Color.Red;
                                lblFIS.Visible = true;

                                string res = arr[5].ToString();

                              
 
                                string lblRes = "lblResultado_" + celda;
                                Control controlres = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblRes);
                                Label lblResultado = (Label)controlres;
                                lblResultado.Text = res;
                                lblResultado.Visible = true;

                                if (res.Trim().Length > 1)
                                {
                                    if (res.TrimStart().Substring(0, 10) == "SE DETECTA")
                                    {
                                        lblN1.ForeColor = Color.Red;
                                        lblN2.ForeColor = Color.Red;
                                        lblRP.ForeColor = Color.Red;
                                        lblResultado.ForeColor = Color.Red;
                                        lblResultado.Text = res.Substring(0, 10);
                                        
                                        cantidadPos = cantidadPos + 1;
                                    }

                                    if (res.TrimStart().Substring(0, 13) == "NO SE DETECTA")
                                    {
                                        lblN1.ForeColor = Color.Black;
                                        lblN2.ForeColor = Color.Black;
                                        lblRP.ForeColor = Color.Black;
                                        lblResultado.ForeColor = Color.Black;
                                        lblResultado.Text = res.Substring(0, 13);
                                        cantidadNega = cantidadNega + 1;
                                    }
                                }


                                string chkvalida = "chkValida_" + celda;
                                Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(chkvalida);
                                CheckBox chkValida = (CheckBox)control7;
                                chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                               
                                chkValida.Visible = false;


                            }




                        }

                        if (oRegistro.Equipo == "Promega")
                        {
                           


                            if (oDet.NamespaceID.Length == 3)
                                celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 1);
                            if (oDet.NamespaceID.Length == 4)
                                celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 2);

                            n1 = oDet.NamespaceID.Substring(0, 1);
                            n2 = oDet.NamespaceID.Substring(1, 1);
                            rp = oDet.NamespaceID.Substring(2, 1);
                            if (oDet.NamespaceID.Length == 3)
                                nro = oDet.NamespaceID.Substring(2, 1);
                            if (oDet.NamespaceID.Length == 4)
                                nro = oDet.NamespaceID.Substring(2, 2);


                            celditaN1 = n1 + nro;
                            celditaN2 = n2 + nro;
                            celditaRP = rp + nro;

                            if (nro.Length == 1)
                            {
                                celditaN1 = n1 + "0" + nro;
                                celditaN2 = n2 + "0" + nro;
                                celditaRP = rp + "0" + nro;
                            }




                            string lblprotocolo = "lbl_" + celda;

                            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblprotocolo);
                            Label txtNumero = (Label)control1;
                            string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());
                            txtNumero.Text = arr[0].ToString();

                            txtNumero.Visible = true;

                            string s_idProtocolo = arr[4].ToString();

                            //    cambiar el getCT desde archivo por desde BD lab_detalleprotocolo 
                            string[] arrct1 = GetCTDetalleProtocolo(s_idProtocolo, "N1", "Promega").Split((";").ToCharArray());
                            string lblctN1 = "lbl_P_" + celditaN1;
                            Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN1);
                            Label lblN1 = (Label)controlN1;
                            lblN1.Text = arrct1[0].ToString().Replace("FAM=", "").Replace("N1", "");
                            lblN1.Visible = true;

                            string[] arrct2 = GetCTDetalleProtocolo(s_idProtocolo, "RP", "Promega").Split((";").ToCharArray());
                            string lblctN2 = "lbl_P_" + celditaN2;
                            Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN2);
                            Label lblN2 = (Label)controlN2;
                            lblN2.Text = arrct2[0].ToString().Replace("FAM=", "").Replace("RP", "");
                            lblN2.Visible = true;

                        
                            string lbllugar = "lblLugar_" + celda;
                            string lblfIS = "lblFIS_" + celda;
                            Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lbllugar);
                            Label lblLugar = (Label)control2;
                            lblLugar.Text = arr[1].ToString();
                            lblLugar.Font.Bold = true;
                            lblLugar.Visible = true;

                            Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblfIS);
                            Label lblFIS = (Label)control3;
                            lblFIS.Text = arr[2].ToString();

                            if (!arr[2].Contains("SD"))
                                lblFIS.ForeColor = Color.Blue;
                            else
                                lblFIS.ForeColor = Color.Red;
                            lblFIS.Visible = true;

                            string res = arr[5].ToString();

                           
                            string lblRes = "lblResultado_P_" + celda;
                            Control controlres = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblRes);
                            Label lblResultado = (Label)controlres;
                            lblResultado.Text = res;
                            lblResultado.Visible = true;

                            if (res.Trim().Length > 1)
                            {
                                if (res.TrimStart().Substring(0, 10) == "SE DETECTA")
                                {
                                    lblN1.ForeColor = Color.Red;
                                    lblN2.ForeColor = Color.Red;

                                    lblResultado.ForeColor = Color.Red;
                                    lblResultado.Text = res.Substring(0, 10);
                                    cantidadPos = cantidadPos + 1;
                                }

                                if (res.TrimStart().Substring(0, 13) == "NO SE DETECTA")
                                {
                                    lblN1.ForeColor = Color.Black;
                                    lblN2.ForeColor = Color.Black;

                                    lblResultado.ForeColor = Color.Black;
                                    lblResultado.Text = res.Substring(0, 13);
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
                        if (oRegistro.Equipo == "Alplex")
                        {

                            if (oDet.NamespaceID.Length < 3)
                                celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                            else
                                celda = oDet.NamespaceID;
                            string lblprotocolo = "lblProtocolo_" + celda;
                            string lbllugar = "lblLugar_" + celda;
                            string lblfIS = "lblFIS_" + celda;
                            string lblct = "lblCt1_" + celda;
                            string lblresultado = "lblResultado_" + celda;
                            string lbldias = "lblDias_" + celda;
                            string pnl = celda;


                            Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblprotocolo);
                            Label lblNumero = (Label)control1;



                            string[] arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());



                            lblNumero.Text = arr[0].ToString();

                            Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lbllugar);
                            Label lblLugar = (Label)control2;

                            lblLugar.Text = arr[1].ToString();
                            lblLugar.Font.Bold = true;

                            Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblfIS);
                            Label lblFIS = (Label)control3;

                            lblFIS.Text = arr[2].ToString();
                            if (!arr[2].Contains("SD"))
                                lblFIS.ForeColor = Color.Green;
                            else
                                lblFIS.ForeColor = Color.Red;

                            string s_idProtocolo = arr[4].ToString();


                            string[] arrct = GetCTDetalleProtocolo(s_idProtocolo, "", "Alplex").Split((";").ToCharArray());



                            Control control4 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblct);
                            Label lblCt1 = (Label)control4;
                            lblCt1.Text = lblCt1.Text.Replace("Label", "");
                            lblCt1.Text = arrct[0].ToString();



                            string res = arr[5].ToString();
                            Control control5 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblresultado);
                            Label lblResultado = (Label)control5;
                            lblResultado.Text = lblResultado.Text.Replace("Label", "");

                            if (res.Length > 10)
                            {
                                if (res.TrimStart().Substring(0, 10) == "SE DETECTA")
                                {
                                    lblResultado.ForeColor = Color.Red;
                                    cantidadPos = cantidadPos + 1;
                                }
                                else
                                { cantidadNega = cantidadNega + 1; }

                                if (lblResultado.Text == "")

                                    lblResultado.Text = res;
                                else
                                    lblResultado.Text += "-" + res;


                            }




                            string chkvalida = "chkValida_" + celda;
                            Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                            CheckBox chkValida = (CheckBox)control7;
                            chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                            //chkValida.Visible = true;

                            //if (oDet.EstaValidado()) // o prevalidado
                            chkValida.Visible = false;

                            Control controlP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(pnl);
                            Panel pnlito = (Panel)controlP;
                            pnlito.Visible = true;
                        }
                        if (oRegistro.Equipo == "Alplex7V")
                        {
                            string res="";
                            if (oDet.NamespaceID.Length < 3)
                                celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                            else
                                celda = oDet.NamespaceID;
                            string lblprotocolo = "lblProtocolo_" + celda;
                            string lbllugar = "lblLugar_" + celda;
                            string lblfIS = "lblFIS_" + celda;
                            string lblct = "lblCt1_" + celda;
                            string lblresultado = "lblResultado_" + celda;
                            string lbldias = "lblDias_" + celda;
                            string pnl = celda;

                            if (celdapivot != celda)
                            {

                                cantidadPos = 0;
                                cantidadNega = 0;
                            }
                                  Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblprotocolo);
                                Label lblNumero = (Label)control1;

                                 string[]  arr = getNumeroProtocolo(oDet.IdDetalleProtocolo).Split((";").ToCharArray());

                                lblNumero.Text = arr[0].ToString();

                                Control control2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lbllugar);
                                Label lblLugar = (Label)control2;

                                lblLugar.Text = arr[1].ToString();
                                lblLugar.Font.Bold = true;

                                Control control3 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblfIS);
                                Label lblFIS = (Label)control3;

                                lblFIS.Text = arr[2].ToString();
                                if (!arr[2].Contains("SD"))
                                    lblFIS.ForeColor = Color.Green;
                                else
                                    lblFIS.ForeColor = Color.Red;

                                string s_idProtocolo = arr[4].ToString();


                                string[] arrct = GetCTDetalleProtocolo(s_idProtocolo, "", "Alplex7V").Split((";").ToCharArray());



                                Control control4 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblct);
                                Label lblCt1 = (Label)control4;
                                lblCt1.Text = lblCt1.Text.Replace("Label", "");
                                lblCt1.Text = arrct[0].ToString();


                                res  = arr[5].ToString().ToUpper();


                                 
                            
                                Control control5 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblresultado);
                                Label lblResultado = (Label)control5;
                                lblResultado.Text = lblResultado.Text.Replace("Label", "");
                            if (celdapivot != celda)
                            {
                                lblResultado.Text = "";
                                    }

                                  celdapivot = celda;
                           

                                if (res.Length > 10)
                                {

                                    if (res.TrimStart().Substring(0, 10) == "SE DETECTA")
                                    {
                                        lblResultado.ForeColor = Color.Red;
                                        cantidadPos = cantidadPos + 1;
                                        if (lblResultado.Text == "")
                                            lblResultado.Text = res;
                                        else
                                            lblResultado.Text += "-" + res;
                                    }
                                    else
                                    { cantidadNega = cantidadNega + 1; }
                                    /// si alguna es positivo: SE detecta- se muestra solo lo que se detecta.
                                    /// si es negativo todo: se muestra "no se detecta ningun virus"


                                    //if (lblResultado.Text == "")
                                    //    lblResultado.Text = res;
                                    //else
                                    //    lblResultado.Text += "-" + res;

                                }

                            if (cantidadNega == 7)
                                lblResultado.Text = "NO SE DETECTA NINGUN VIRUS";
                            else
                                lblResultado.Text += "";// ( Los demas negativos )";

                                string chkvalida = "chkValida_" + celda;
                                Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                                CheckBox chkValida = (CheckBox)control7;
                                chkValida.ToolTip = oDet.IdDetalleProtocolo.ToString(); // aca guardo el valor;

                                //chkValida.Visible = true;

                                //if (oDet.EstaValidado()) // o prevalidado
                                chkValida.Visible = false;

                                Control controlP = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(pnl);
                                Panel pnlito = (Panel)controlP;
                                pnlito.Visible = true;
                               
                        }
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

        private string GetCTDetalleProtocolo(string s_idProtocolo, string codigoitem, string tipo)
        {
           

            string m_strSQL = "";
            string ct = "";
            //int posi = 0;
            //int nega = 0;


            //            m_strSQL = @"select i.codigo, d.resultadoCar
            //from LAB_DetalleProtocolo d
            // inner join LAB_Item i on i.iditem = d.idSubItem
            // where idprotocolo =" + s_idProtocolo+ "  and i.codigo in ('"+ codigoitem + "')";

            m_strSQL = @"select i.codigo, d.resultadoCar
from LAB_DetalleProtocolo d (nolock)
 inner join LAB_Item i (nolock) on i.iditem = d.idSubItem
 where idprotocolo =" + s_idProtocolo + "  and i.codigo in (select distinct tipoCT from LAB_DetalleTipoPlaca)";
            if (tipo=="Alplex7V")
                m_strSQL = @"select i.codigo, d.resultadoCar
from LAB_DetalleProtocolo d (nolock)
 inner join LAB_Item i (nolock) on i.iditem = d.idSubItem
 where idprotocolo =" + s_idProtocolo + "  and i.codigo in (select distinct tipoCT from LAB_DetalleTipoPlaca where idTipoPlaca=3)";


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            if (tipo.Substring(0,6).ToUpper() == "ALPLEX")
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


                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)

                {
                   


                    if (ct == "")
                        ct = Ds.Tables[0].Rows[i][0].ToString() + "=" + Ds.Tables[0].Rows[i][1].ToString();
                    else
                        ct = ct + Ds.Tables[0].Rows[i][0].ToString() + " = " + Ds.Tables[0].Rows[i][1].ToString();

                }
          
            }


            return ct;
        }

      
        private string GetCT(string celda, string tipo)
        {

            string m_strSQL = "";
            string ct = "";
            int posi = 0;
            int nega = 0;

            m_strSQL = @"select tipoct ,  substring(valorct,1,4)     from LAB_ResultadoTemp(nolock) where   celda='" + celda + "'  and  idPlaca=" + Request["idPlaca"].ToString();

            
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
                    if ((resCt.ToUpper() != "NAN") && (resCt.ToUpper() != "UNDE") && (resCt.ToUpper() != "N/A"))
                        posi = posi + 1;

                    
                    if ((resCt.ToUpper() == "NAN") || (resCt.ToUpper() == "UNDE") || (resCt.ToUpper() != "N/A"))
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



        private string GetCT(string celda, string tipo, int idDetalleProtocolo, int idplaca)  //por cada determinacion
        {

            string m_strSQL = "";
            string ct = "";
            int posi = 0;
            int nega = 0;

            //m_strSQL = @"select tipoct ,  substring(valorct,1,4)     
            //    from LAB_ResultadoTemp where   celda='" + celda + "'  and  idPlaca=" + Request["idPlaca"].ToString();

            m_strSQL = @"
select distinct DTP.tipoCT, T.valorCT , DP.iditem
from LAB_DetalleProtocoloPlaca as DPP (nolock)
inner join lab_detalleprotocolo as DP (nolock) on DP.idDetalleProtocolo= DPP.idDetalleProtocolo

inner join LAB_ResultadoTEmp as T (nolock) on T.idPlaca= DPP.idPlaca 
inner join lab_detalletipoplaca as DTP (nolock) on DTP.iditem= DP.iditem and DTP.tipoCT=t.tipoct
where   DPP.idDetalleProtocolo= " + idDetalleProtocolo.ToString() + "  and DPP.idplaca=" + idplaca.ToString() + " and T.celda= '" + celda + "' order by DP.iditem ";


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
          //  string resultado = "";
            if (tipo == "Alplex")
            {
                string idDet = "";
                string nom = "";
                int cant = 0;
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    cant += 1;
                       idDet =Ds.Tables[0].Rows[i][2].ToString();
                     

                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), int.Parse(idDet));
                    if (oItem!=null) nom = oItem.Nombre;

                    string resCt = Ds.Tables[0].Rows[i][1].ToString();
                    if ((resCt.ToUpper() != "NAN") && (resCt.ToUpper() != "UNDE") && (resCt.ToUpper() != "N/A"))
                        posi = posi + 1;


                    if ((resCt.ToUpper() == "NAN") || (resCt.ToUpper() == "UNDE") || (resCt.ToUpper() == "N/A"))
                    {
                        nega = nega + 1;
                        resCt = "NN";
                    }

                    string tipoCT = Ds.Tables[0].Rows[i][0].ToString();
                    //if (tipoCT.Length >= 3)
                    //    tipoCT = tipoCT.Substring(0, 3);

                    if (ct == "")
                        ct = tipoCT + "=" + resCt;
                    else
                        ct = ct + "\n" + tipoCT + "=" + resCt;

                    

                }
                //  logica para Alplex
                if (cant > 0)
                {
                    ct = ct + ";" + nom;
                    if (posi == cant)
                        ct = ct + "=SE DETEC";
                    else
                    {
                        if ((cant > 1) && (posi < cant))
                        { ct = ct + "=NO SE DET"; }
                        else
                        {
                            if (nega == cant)
                                ct = ct + "=NO SE DET";
                            else
                                ct = ct + "=INDET";
                        }
                    }
                }



            }

            else //promega
            {


                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)

                {
                    if ((Ds.Tables[0].Rows[i][1].ToString() != "NaN") && (Ds.Tables[0].Rows[i][1].ToString() != "Unde"))
                        posi = posi + 1;


                    if ((Ds.Tables[0].Rows[i][1].ToString() == "NaN") || (Ds.Tables[0].Rows[i][1].ToString() == "Unde"))
                        nega = nega + 1;


                    if (ct == "")
                        ct = Ds.Tables[0].Rows[i][0].ToString() + "=" + Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");
                    else
                        ct = ct + Ds.Tables[0].Rows[i][0].ToString() + " = " + Ds.Tables[0].Rows[i][1].ToString().Replace("Unde", "NaN");

                }

            }


            return ct;
        }

        private string getDeterminacion(string tipoct)
        {
            string m_strSQL = "";
            //string ct = "";
            //int posi = 0;
            //int nega = 0;
            string codigo = "";
            m_strSQL = @"select iditem, codigo from lab_item a (nolock) where exists (
select 1 from lab_item b (nolock) where b.idItemReferencia=a.iditem and codigo='" + tipoct+"')";




            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                codigo = Ds.Tables[0].Rows[i][1].ToString(); break;
            }
            return codigo;
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

                                    if (line.Contains(";ID de Paciente;Pocillo;Nombre;Tipo;FAM;FAM;FAM;FAM;HEX;HEX;HEX;HEX;Cal Red 610;Cal Red 610;Cal Red 610;Cal Red 610;Quasar 670;Quasar 670;Quasar 670;Quasar 670;"))
                                    {
                                        if (lblEquipo.Text.ToUpper().Substring(0,6) == "ALPLEX")
                                            lineaDesde = 3;
                                        modelo = "ALPLEXMultiple";
                                    }
                                }


                                if (i >= lineaDesde) 
                                {
                                     if (lblEquipo.Text.ToUpper() == "ALPLEX")
                                            ProcesarLinea(line, modelo);
                                    if (lblEquipo.Text.ToUpper() == "ALPLEX7V")
                                        ProcesarLinea7V(line, modelo);
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

         //   string query = @"DELETE FROM [dbo].[LAB_ResultadoTemp] where idUsuario= "+ Session["idUsuarioValida"].ToString()+  " and [idPLACA]=" + Request["idPlaca"].ToString();
            string query = @"DELETE FROM [dbo].[LAB_ResultadoTemp] where   [idPLACA]=" + Request["idPlaca"].ToString();
            SqlCommand cmd = new SqlCommand(query, conn);





            int idres = Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void ProcesarLinea(string linea, string modelo)
        {
            try
            {
                bool grabar = true;

                if (modelo == "ALPLEXMultiple")
                {
                    string[] arr2 = linea.Split((";").ToCharArray());

                    if (arr2.Length >= 1)
                    {
                        string celda = arr2[2].ToString(); //pocillo

                        string escontrol = arr2[4].ToString(); // sample o control
                        if ((escontrol.Contains("PC")) || (escontrol.Contains("NC")))
                            grabar = false;

                        if (grabar)
                        {
                            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                            string valorCtFAM = arr2[6].ToString().Replace("\"","");  //valor FAM - covid
                            string query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "FAM" + "','" + valorCtFAM + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd = new SqlCommand(query, conn);

                            int idres = Convert.ToInt32(cmd.ExecuteScalar());


                            string valorCtRSV = arr2[8].ToString().Replace("\"", "");  //valor FAM - RSV
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "FAM-RSV" + "','" + valorCtRSV + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd1 = new SqlCommand(query, conn);
                            idres = Convert.ToInt32(cmd1.ExecuteScalar());

                            string valorCtHEX = arr2[10].ToString().Replace("\"", "");  //valor HEX - covid
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "HEX" + "','" + valorCtHEX + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd2 = new SqlCommand(query, conn);
                            idres = Convert.ToInt32(cmd2.ExecuteScalar());

                            string valorCtFLUB = arr2[12].ToString().Replace("\"","");  //valor HEX - Flu B
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "HEX-FLUB" + "','" + valorCtFLUB + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd3 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd3.ExecuteScalar());

                            string valorCtCAL = arr2[14].ToString().Replace("\"","");  //valor CAL red - covid
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "CAL" + "','" + valorCtCAL + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd4 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd4.ExecuteScalar());

                            string valorCtFLUA = arr2[16].ToString().Replace("\"","");  //valor CAL red - Flu A
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "CAL-FlUA" + "','" + valorCtFLUA + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd5 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd5.ExecuteScalar());

                            string valorCtQUA = arr2[18].ToString().Replace("\"","");  //valor Quasar - Covid

                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "QUA" + "','" + valorCtQUA + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd6 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd6.ExecuteScalar());
                            //usar el iditemde referencia para vincular el ct con la determinacion

                        }



                    }
                     
                      
                      
                    
                       
                    }
                else
                {

                    string[] arr = linea.Split((",").ToCharArray());
                 
                    if (arr.Length >= 1)
                    {


                        string celda = arr[0].ToString();
                        string tipoCt = arr[1].ToString();
                        string valorCt = arr[5].ToString();

                        if (modelo == "ABI")
                        {
                            tipoCt = arr[3].ToString();
                            valorCt = arr[4].ToString();
                            if (lblEquipo.Text.ToUpper().Substring(0,6) == "ALPLEX")
                            {
                                tipoCt = arr[2].ToString();

                            }
                        }


                        // para alplex
                        string escontrol = arr[3].ToString();

                        if ((escontrol.Contains("NTC")) || (escontrol.Contains("Ctrl")))
                            grabar = false;

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
           ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + tipoCt + "','" + valorCt + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd = new SqlCommand(query, conn);


                            int idres = Convert.ToInt32(cmd.ExecuteScalar());

                        }
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


        private void ProcesarLinea7V(string linea, string modelo)
        {
            try
            {
                bool grabar = true;

                if (modelo == "ALPLEXMultiple")
                {
                    string[] arr2 = linea.Split((";").ToCharArray());

                    if (arr2.Length >= 1)
                    {
                        string celda = arr2[2].ToString(); //pocillo

                        string escontrol = arr2[4].ToString(); // sample o control
                        if ((escontrol.Contains("PC")) || (escontrol.Contains("NC")))
                            grabar = false;

                        if (grabar)
                        {
                            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                            string valorCtFAM = arr2[6].ToString().Replace("\"", "");  //valor FAM -PIV4

                            string query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "FAM-PIV4" + "','" + valorCtFAM + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd = new SqlCommand(query, conn);

                            int idres = Convert.ToInt32(cmd.ExecuteScalar());


                            string valorCtRSV = arr2[8].ToString().Replace("\"", "");  //valor FAM - MPV
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "FAM-MPV" + "','" + valorCtRSV + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd1 = new SqlCommand(query, conn);
                            idres = Convert.ToInt32(cmd1.ExecuteScalar());

                            string valorCtHEX = arr2[10].ToString().Replace("\"", "");  //valor HEX - PIV2
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "HEX-PIV2" + "','" + valorCtHEX + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd2 = new SqlCommand(query, conn);
                            idres = Convert.ToInt32(cmd2.ExecuteScalar());

                            string valorCtFLUB = arr2[12].ToString().Replace("\"", "");  //valor HEX - PIV1
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "HEX-PIV1" + "','" + valorCtFLUB + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd3 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd3.ExecuteScalar());

                            string valorCtCAL = arr2[14].ToString().Replace("\"", "");  //valor CAL red - ADV
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "CAL-ADV" + "','" + valorCtCAL + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd4 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd4.ExecuteScalar());

                            string valorCtFLUA = arr2[16].ToString().Replace("\"", "");  //valor CAL red - HEV
                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "CAL-HEV" + "','" + valorCtFLUA + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd5 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd5.ExecuteScalar());

                            string valorCtQUA = arr2[18].ToString().Replace("\"", "");  //valor Quasar - PIV3

                            query = @"INSERT INTO [dbo].[LAB_ResultadoTEmp]([idPlaca]   ,[celda]    ,[tipoCT]   ,[valorCT],idUsuario    )
                            VALUES  ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + "QUA-PIV3" + "','" + valorCtQUA + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd6 = new SqlCommand(query, conn);

                            idres = Convert.ToInt32(cmd6.ExecuteScalar());
                            //usar el iditemde referencia para vincular el ct con la determinacion

                        }



                    }





                }
                else
                {

                    string[] arr = linea.Split((",").ToCharArray());

                    if (arr.Length >= 1)
                    {


                        string celda = arr[0].ToString();
                        string tipoCt = arr[1].ToString();
                        string valorCt = arr[5].ToString();

                        if (modelo == "ABI")
                        {
                            tipoCt = arr[3].ToString();
                            valorCt = arr[4].ToString();
                            if (lblEquipo.Text.ToUpper().Substring(0, 6) == "ALPLEX")
                            {
                                tipoCt = arr[2].ToString();

                            }
                        }


                        // para alplex
                        string escontrol = arr[3].ToString();

                        if ((escontrol.Contains("NTC")) || (escontrol.Contains("Ctrl")))
                            grabar = false;

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
           ( " + Request["idPlaca"].ToString() + ",'" + celda + "','" + tipoCt + "','" + valorCt + "' , " + Session["idUsuarioValida"].ToString() + "     )";
                            SqlCommand cmd = new SqlCommand(query, conn);


                            int idres = Convert.ToInt32(cmd.ExecuteScalar());

                        }
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
              

                Guardar(); Response.Redirect("PlacaResultado.aspx?Desde=Consulta&idPlaca=" +HdIdPlaca.Value);
              //  MostrarDatosPlaca();

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
                    string pnl = ""; string valorCT = "";
                    if (oRegistro.Equipo.Substring(0,6) == "Alplex")
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

                        string m_valor = chkValida.ID.Replace("chkValida", "lblResultado");

                        Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(m_valor);
                        Label oRes = (Label)control10;
                        resultado = oRes.Text;


                        string m_Celda = chkValida.ID.Replace("chkValida", "").Replace("_", "").Replace("P", "");
                        string lblctN1 = "lblCt1_" + m_Celda;
                        Control controlCt = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblctN1);
                        Label lblCt = (Label)controlCt;
                        valorCT = lblCt.Text;
                    }
                    //if (oRegistro.Equipo == "Alplex")
                    //{
                    //    pnl = "pnlAlplex";
                    //    if (oDet.NamespaceID.Length < 3)
                    //        celda = oDet.NamespaceID.Substring(0, 1) + "0" + oDet.NamespaceID.Substring(1, 1);
                    //    else
                    //        celda = oDet.NamespaceID;
                    //    string chkvalida = "chkValida_" + celda;
                    //    Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(chkvalida);
                    //    CheckBox chkValida = (CheckBox)control7;


                    //    m_id = chkValida.ToolTip;
                    //    m_idChk = chkValida.ID;

                    //    string m_valor = chkValida.ID.Replace("chkValida", "lblResultado");

                    //    Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(m_valor);
                    //    Label oRes = (Label)control10;
                    //    resultado = oRes.Text;


                    //    string m_Celda = chkValida.ID.Replace("chkValida", "").Replace("_", "").Replace("P", "");
                    //    string lblctN1 = "lblCt1_" + m_Celda;
                    //    Control controlCt = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblctN1);
                    //    Label lblCt = (Label)controlCt;
                    //      valorCT = lblCt.Text;
                    //}


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

                            string chkvalida = "chkValida_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;


                        m_id = chkValida.ToolTip; //detalleprotocolo
                        m_idChk = chkValida.ID; ///celdda ABC_123

                        string m_valor = chkValida.ID.Replace("chkValida", "lblResultado");

                        Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(m_valor);
                        Label oRes = (Label)control10;
                        resultado = oRes.Text;
                    }


                    if (oRegistro.Equipo == "Promega")
                    {
                        pnl = "pnlPromega";

                        if (oDet.NamespaceID.Length == 3)
                            celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 1);
                        if (oDet.NamespaceID.Length == 4)
                            celda = oDet.NamespaceID.Substring(0, 2) + "_" + oDet.NamespaceID.Substring(2, 2);

                      

                        string chkvalida = "chkValida_P_" + celda;
                        Control control7 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(chkvalida);
                        CheckBox chkValida = (CheckBox)control7;


                        m_id = chkValida.ToolTip; //detalleprotocolo
                        m_idChk = chkValida.ID; ///celdda ABC_123

                        string m_valor = chkValida.ID.Replace("chkValida_P_", "lblResultado_P_");

                        Control control10 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(m_valor);
                        Label oRes = (Label)control10;
                        resultado = oRes.Text;

                    }

                    if ((Request["Operacion"].ToString() == "Valida") || (Request["Operacion"].ToString() == "Control"))
                    {
                        if (estatildado(m_idChk, pnl))
                        {
                            //CARoct


                            Protocolo oProtocolo = new Protocolo();
                            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), "Numero", int.Parse(m_id));

                            valorCT = valorCT.Replace("Label", "");
                            resultado = resultado.Replace("Label", "");
                            // GuardarResultado(m_id, resultado);
                            GuardarResultado(oProtocolo, resultado, "Det");

                            GuardarResultado(oProtocolo, valorCT, "CT");

                            //GuardarResultadoCT(m_id, m_idChk, oRegistro.Equipo);
                            //  GuardarReferenciaMetodoUnidadMedida(txt.ID, oProtocolo);
                        }
                       
                    }


                }
                oRegistro.Estado = "V"; // validada la placa

                oRegistro.Save();
                oRegistro.GrabarAuditoria("Valida Placa", int.Parse(Session["idUsuarioValida"].ToString()), "");

            }
            catch (Exception ex)
            {
                lblEquipo.Text = ex.Message.ToString();
            }
        }

        private void GuardarResultado( Protocolo oProtocolo, string valorItem, string v)
        {

                //////////////////////////////////////////////////////////////////

            if    (oProtocolo != null) 
            {


                string[] arritem = valorItem.Trim().Split(("\n").ToCharArray());
                for (int i=0; i<arritem.Length; i++)
                {
                    string[] arrRes = arritem[i].ToString().Split(("=").ToCharArray());

                    if (arrRes[0].ToString().Trim() != "")
                    {
                        valorItem = arrRes[1].ToString().Trim();
                        Item oItem = new Item();
                        if (v == "CT")
                        {
                           
                            oItem = (Item)oItem.Get(typeof(Item), "Codigo", arrRes[0].ToString());
                        }
                        else
                        {
                          
                            oItem = (Item)oItem.Get(typeof(Item), "Nombre", arrRes[0].ToString());
                        }
                        if (oItem != null)
                        {
                            if (v == "CT")

                                GuardarResultaditoDetalleCT(oProtocolo, oItem, valorItem);
                             
                            else 


                                GrabarResultadito(oProtocolo, oItem, valorItem);
                            
                        }
                    }

                }
            }
        }

        private void GrabarResultadito(Protocolo oProtocolo, Item oItem, string valorItem)
        {
            string m_metodo = "";
            string m_valorReferencia = "";
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            crit.Add(Expression.Eq("IdSubItem", oItem));

          

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
                                if (valorItem == "SE DETEC")
                                    oDetalle.ResultadoCar = oDetalle.IdSubItem.GetResultado(valorItem);
                                if (valorItem == "NO SE DET")
                                    oDetalle.ResultadoCar = oDetalle.IdSubItem.GetResultado(valorItem);
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

                    string[] arr = oDetalle.CalcularValoresReferencia(pres).Split(("|").ToCharArray());
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

                            if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0) && (res == "SE DET"))// GENOMA DE COVID-19"))
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

        private void GuardarResultadoCT(string m_id, string m_Celda, string equipo)
        {
            string celda = "";
            string n1 = "";
            string n2 = "";
            string rp = "";
            string nro = "";
            string celditaN1 = "";
            string celditaN2 = "";
            string celditaRP = "";
            string letra = "";

            m_Celda = m_Celda.Replace("chkValida", "").Replace("_", "").Replace("P","");


            // Promega
            if (equipo == "Promega-30M")
            {
                if (m_Celda.Length > 3)
                {
                    if ((m_Celda.Substring(0, 1) == "D") || (m_Celda.Substring(0, 1) == "E"))
                    { //"D123"
                        if (m_Celda.Length < 6)
                        {
                            celda = m_Celda.Substring(0, 1) + "_" + m_Celda.Substring(1, 3);
                            letra = m_Celda.Substring(0, 1);
                            n1 = m_Celda.Substring(1, 1);
                            n2 = m_Celda.Substring(2, 1);
                            rp = m_Celda.Substring(3, 1);

                        }

                        if (m_Celda.Length > 6)
                        {//"D101112"
                            celda = m_Celda.Substring(0, 1) + "_" + m_Celda.Substring(1, 6);
                            letra = m_Celda.Substring(0, 1);
                            n1 = m_Celda.Substring(1, 2);
                            n2 = m_Celda.Substring(3, 2);
                            rp = m_Celda.Substring(5, 2);
                        }

                        if (n1.Length == 1)
                        {
                            celditaN1 = letra + "0" + n1;
                            celditaN2 = letra + "0" + n2;
                            celditaRP = letra + "0" + rp;
                        }
                        else

                        {
                            celditaN1 = letra + n1;
                            celditaN2 = letra + n2;
                            celditaRP = letra + rp;
                        }

                    }
                    else
                    {
                        if (m_Celda.Length == 4)
                            celda = m_Celda.Substring(0, 3) + "_" + m_Celda.Substring(3, 1);
                        if (m_Celda.Length == 5)
                            celda = m_Celda.Substring(0, 3) + "_" + m_Celda.Substring(3, 2);

                        n1 = m_Celda.Substring(0, 1);
                        n2 = m_Celda.Substring(1, 1);
                        rp = m_Celda.Substring(2, 1);
                        if (m_Celda.Length == 4)
                            nro = m_Celda.Substring(3, 1);
                        if (m_Celda.Length == 5)
                            nro = m_Celda.Substring(3, 2);


                        celditaN1 = n1 + nro;
                        celditaN2 = n2 + nro;
                        celditaRP = rp + nro;

                        if (nro.Length == 1)
                        {
                            celditaN1 = n1 + "0" + nro;
                            celditaN2 = n2 + "0" + nro;
                            celditaRP = rp + "0" + nro;
                        }
                    }
                } 

                        string lblctN1 = "lbl_" + celditaN1;
                        Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN1);
                        Label lblN1 = (Label)controlN1;

                        //lblN1.Text = arrct1[0].ToString().Replace("FAM=", "");
                        //lblN1.Visible = true;
                        GuardarResultadoDetalleCT(m_id, "N1", lblN1.Text);


                        string lblctN2 = "lbl_" + celditaN2;
                        Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctN2);
                        Label lblN2 = (Label)controlN2;
                        GuardarResultadoDetalleCT(m_id, "N2", lblN2.Text);


                        string lblctRP = "lbl_" + celditaRP;
                        Control controlrp = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega2").FindControl(lblctRP);
                        Label lblRP = (Label)controlrp;
                        GuardarResultadoDetalleCT(m_id, "RP", lblRP.Text);

                    }


            if (equipo == "Promega")
            {

                 


                if (m_Celda.Length == 3)
                            celda = m_Celda.Substring(0, 2) + "_" + m_Celda.Substring(2, 1);
                        if (m_Celda.Length == 4)
                            celda = m_Celda.Substring(0, 2) + "_" + m_Celda.Substring(2, 2);

                        n1 = m_Celda.Substring(0, 1);
                        n2 = m_Celda.Substring(1, 1);
                       
                        if (m_Celda.Length == 3)
                            nro = m_Celda.Substring(2, 1);
                        if (m_Celda.Length == 4)
                            nro = m_Celda.Substring(2, 2);


                        celditaN1 = n1 + nro;
                        celditaN2 = n2 + nro;
                        

                        if (nro.Length == 1)
                        {
                            celditaN1 = n1 + "0" + nro;
                            celditaN2 = n2 + "0" + nro;
                            
                        }
                    
                

                string lblctN1 = "lbl_P_" + celditaN1;
                Control controlN1 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN1);
                Label lblN1 = (Label)controlN1;

                //lblN1.Text = arrct1[0].ToString().Replace("FAM=", "");
                //lblN1.Visible = true;
                GuardarResultadoDetalleCT(m_id, "N1", lblN1.Text);


                string lblctN2 = "lbl_P_" + celditaN2;
                Control controlN2 = Master.FindControl("ContentPlaceHolder1").FindControl("pnlPromega").FindControl(lblctN2);
                Label lblN2 = (Label)controlN2;
                GuardarResultadoDetalleCT(m_id, "RP", lblN2.Text);


              

            }

            if (equipo == "Alplex")
                    {
                      // // if (m_Celda.Length < 3)
                            celda = m_Celda.Substring(0, 1) + "0" + m_Celda.Substring(1, 1);

                    string lblctN1 = "lblCt1_" + m_Celda;
                    Control controlCt = Master.FindControl("ContentPlaceHolder1").FindControl("pnlAlplex").FindControl(lblctN1);
                    Label lblCt = (Label)controlCt;

                    string[] arrCt=lblCt.Text.Split(("-").ToCharArray());

                    for (int i=0; i< arrCt.Length; i++) // deberia grabar 4 ct para alplex
                    {
                    string [] ctito = arrCt[i].ToString().Split(("=").ToCharArray());
                    string valorCT = ctito[1].ToString();
                    if (valorCT == "Nan.") valorCT = "NaN";

                    GuardarResultadoDetalleCT(m_id, ctito[0].ToString(), valorCT);
                    }

                    //m_idItem = arrIdItem[1].ToString();
                }

             
        }

        private void GuardarResultadoDetalleCT(string m_id, string nombreItem, string valor)
        {
            DetalleProtocolo oRegistro = new DetalleProtocolo();
            oRegistro = (DetalleProtocolo)oRegistro.Get(typeof(DetalleProtocolo), int.Parse(m_id));

            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), "Codigo", nombreItem);


            /// se fija si ya existe el CT, si no lo crea
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro.IdProtocolo));
            crit.Add(Expression.Eq("IdSubItem", oItem));
            IList detalle = crit.List();



            if (detalle.Count > 0)
            {
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



        private void GuardarResultaditoDetalleCT(Protocolo oRegistro, Item oItem, string valor)
        {
            //DetalleProtocolo oRegistro = new DetalleProtocolo();
            // oRegistro = (DetalleProtocolo)oRegistro.Get(typeof(DetalleProtocolo), int.Parse(m_id));

            //Item oItem = new Item();
            //oItem = (Item)oItem.Get(typeof(Item),"Codigo", nombreItem);


            /// se fija si ya existe el CT, si no lo crea
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro));
            crit.Add(Expression.Eq("IdSubItem", oItem));            
            IList detalle = crit.List();



            if (detalle.Count > 0)
            {
                foreach (DetalleProtocolo oDetalle1 in detalle)
                {

                    oDetalle1.IdProtocolo = oRegistro;
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
                oDetalle.IdProtocolo = oRegistro;
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
            //string nombre_control = "VR" + m_idItem;
            //Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            //Label valorRef = (Label)control1;




            /////busca unidad de medida
            //nombre_control = "UM" + m_idItem;
            //Control controlUMedida = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(nombre_control);
            //Label unMedida = (Label)controlUMedida;


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
                                        oDetalle.ResultadoCar = HdSeDetecta.Value;
                                    if (valorItem.ToUpper() == "NO SE DETECTA")
                                        oDetalle.ResultadoCar = HdNoSeDetecta.Value;
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

                        string[] arr =   oDetalle.CalcularValoresReferencia(pres).Split(("|").ToCharArray());
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

                                if ((oDetalle.IdItem.Codigo == oCon.CodigoCovid) && (oDetalle.IdProtocolo.IdPaciente.IdPaciente > 0) && (res == "SE DETECTA"))// GENOMA DE COVID-19"))
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
                  FROM    LAB_DetalleProtocolo d (nolock)
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
                case "Alplex7V":
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


