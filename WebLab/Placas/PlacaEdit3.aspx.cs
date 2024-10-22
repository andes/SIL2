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
using Business.Data.Laboratorio;
using CrystalDecisions.Web;
using System.IO;
//using DalSic;
//using DalPadron;

namespace WebLab.Placas {
    public partial class PlacaEdit3 : System.Web.UI.Page {
        public Configuracion oC = new Configuracion();


        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
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
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack)
            {
                PreventingDoubleSubmit(btnGrabar);
                PreventingDoubleSubmit(btnCerrar);
                PreventingDoubleSubmit(btnImprimir);
                if (Session["idUsuario"] != null)
                {
                    CargarListas();
                    lblNro.Text = "Nueva Placa";
                    lblFecha.Text = DateTime.Now.ToShortDateString();
                    if (Request["placa"] != null)
                    {
                        if (Request["placa"].ToString() == "Promega")
                        {
                            lblEquipo.Text = "Promega"; pnlPromega.Visible = true; pnlAlplex.Visible = false; pnlPromega2.Visible = false;
                        }
                        if (Request["placa"].ToString() == "Alplex8")//8 virus
                        { lblEquipo.Text = "Alplex"; pnlPromega.Visible = false; pnlAlplex.Visible = true; pnlPromega2.Visible = false; }
                        if (Request["placa"].ToString() == "Promega2")
                        { lblEquipo.Text = "Promega-30M"; pnlPromega.Visible = false; pnlAlplex.Visible = false; pnlPromega2.Visible = true; }
                    }

                    if (Request["id"] != null)
                        MostrarDatosPlaca();
                   

                }
                else Response.Redirect("~/FinSesion.aspx", false);
            }
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);



            string m_ssql = "SELECT   distinct idusuario, apellido    as nombre FROM sys_usuario WHERE firmavalidacion like '%Bio%' and idEfector=" + oC.IdEfector.IdEfector.ToString()+ " and activo=1 order by nombre ";

            oUtil.CargarCombo(ddlOperador, m_ssql, "idusuario", "nombre");
            ddlOperador.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            oUtil.CargarCombo(ddlOperador0, m_ssql, "idusuario", "nombre");
            ddlOperador0.Items.Insert(0, new ListItem("--Seleccione--", "0"));

      
            m_ssql = null;
            oUtil = null;
        }
        private void MostrarDatosPlaca()
        {
            lblError.Visible = false;
            ///encabezado de la placa
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            if (Request["id"] != null) oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["id"].ToString()));
            lblNro.Text ="Nro: " + oRegistro.IdPlaca.ToString();

            lblFecha.Text = oRegistro.Fecha.ToShortDateString();
            lblEquipo.Text = oRegistro.Equipo;
           
            string[] arr = oRegistro.Operador.Split('-');
            if (arr.Length > 1)
            {
                ddlOperador.SelectedItem.Text = arr[0].ToString(); //"AB"
                ddlOperador0.SelectedItem.Text = arr[1].ToString(); // "2"
            }
            else
                ddlOperador.SelectedItem.Text = arr[0].ToString(); //"AB"


            if (oRegistro.Estado=="A")
            {
                btnImprimir.Visible = false;
            }
           
             if (oRegistro.Equipo == "Promega")
                {

                    pnlPromega.Visible = true; pnlAlplex.Visible = false; pnlPromega2.Visible = false;
            }
            if (oRegistro.Equipo == "Alplex8")
            { pnlPromega.Visible = false; pnlAlplex.Visible = true; pnlPromega2.Visible = false; }
            if (oRegistro.Equipo == "Promega-30M")
            { pnlPromega.Visible = false; pnlAlplex.Visible = false; pnlPromega2.Visible = true; }

            if (oRegistro.Estado=="C")
            { btnGrabar.Visible = false;
                btnImprimir.Visible = true;
                 
            }

             if (oRegistro.Baja)
            {
                btnGrabar.Visible = false;
                btnImprimir.Visible = false;
                btnCerrar.Visible = false;
            }

            if (Session["errores"] != null) {
                if (Session["errores"].ToString() != "")
                    lblError.Text = Session["errores"].ToString();
                lblError.Visible = true;
               
                    }

            ////
            ////Detalle de la placa

            DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
            crit.Add(Expression.Eq("IdPlaca", oRegistro.IdPlaca));



            IList items = crit.List();

            foreach (DetalleProtocoloPlaca oDet in items)
            {
                if (oDet.NamespaceID == "AB2") txt_AB_2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                 
                  //  if VerificaExistePlaca(oDet.IdDetalleProtocolo)txt_AB_2 : poner color o asterisco
                if (oDet.NamespaceID == "AB3") txt_AB_3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB4") txt_AB_4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB5") txt_AB_5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB6") txt_AB_6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB7") txt_AB_7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB8") txt_AB_8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB9") txt_AB_9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB10") txt_AB_10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB11") txt_AB_11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "AB12") txt_AB_12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "CD1") txt_CD_1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD2") txt_CD_2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD3") txt_CD_3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD4") txt_CD_4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD5") txt_CD_5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD6") txt_CD_6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD7") txt_CD_7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD8") txt_CD_8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD9") txt_CD_9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD10") txt_CD_10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD11") txt_CD_11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "CD12") txt_CD_12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);


                if (oDet.NamespaceID == "EF1") txt_EF_1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF2") txt_EF_2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF3") txt_EF_3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF4") txt_EF_4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF5") txt_EF_5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF6") txt_EF_6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF7") txt_EF_7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF8") txt_EF_8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF9") txt_EF_9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF10") txt_EF_10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF11") txt_EF_11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "EF12") txt_EF_12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);


                if (oDet.NamespaceID == "GH1") txt_GH_1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH2") txt_GH_2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH3") txt_GH_3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH4") txt_GH_4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH5") txt_GH_5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH6") txt_GH_6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH7") txt_GH_7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH8") txt_GH_8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH9") txt_GH_9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH10") txt_GH_10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH11") txt_GH_11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "GH12") txt_GH_12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                
                /// alplex 8
                if (oDet.NamespaceID == "A2") txtA2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A3") txtA3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A4") txtA4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A5") txtA5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A6") txtA6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A7") txtA7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A8") txtA8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A9") txtA9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A10") txtA10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A11") txtA11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "A12") txtA12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "B1") txtB1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B2") txtB2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B3") txtB3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B4") txtB4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B5") txtB5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B6") txtB6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B7") txtB7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B8") txtB8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B9") txtB9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B10") txtB10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B11") txtB11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "B12") txtB12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);


                if (oDet.NamespaceID == "C1") txtC1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C2") txtC2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C3") txtC3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C4") txtC4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C5") txtC5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C6") txtC6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C7") txtC7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C8") txtC8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C9") txtC9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C10") txtC10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C11") txtC11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "C12") txtC12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);


                if (oDet.NamespaceID == "D1") txtD1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D2") txtD2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D3") txtD3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D4") txtD4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D5") txtD5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D6") txtD6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D7") txtD7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D8") txtD8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D9") txtD9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D10") txtD10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D11") txtD11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D12") txtD12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "E1") txtE1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E2") txtE2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E3") txtE3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E4") txtE4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E5") txtE5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E6") txtE6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E7") txtE7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E8") txtE8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E9") txtE9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E10") txtE10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E11") txtE11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E12") txtE12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "F1") txtF1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F2") txtF2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F3") txtF3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F4") txtF4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F5") txtF5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F6") txtF6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F7") txtF7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F8") txtF8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F9") txtF9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F10") txtF10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F11") txtF11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "F12") txtF12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "G1") txtG1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G2") txtG2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G3") txtG3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G4") txtG4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G5") txtG5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G6") txtG6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G7") txtG7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G8") txtG8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G9") txtG9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G10") txtG10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G11") txtG11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "G12") txtG12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);


                if (oDet.NamespaceID == "H1") txtH1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H2") txtH2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H3") txtH3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H4") txtH4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H5") txtH5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H6") txtH6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H7") txtH7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H8") txtH8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H9") txtH9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H10") txtH10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H11") txtH11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "H12") txtH12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);


                /// promega2
                /// 
              //  if (oDet.NamespaceID == "ABC1") txt_ABC_1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC2") txt_ABC_2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC3") txt_ABC_3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC4") txt_ABC_4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC5") txt_ABC_5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC6") txt_ABC_6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC7") txt_ABC_7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC8") txt_ABC_8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC9") txt_ABC_9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC10") txt_ABC_10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC11") txt_ABC_11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "ABC12") txt_ABC_12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "D123") txt_D_123.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D456") txt_D_456.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D789") txt_D_789.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "D101112") txt_D_101112.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                if (oDet.NamespaceID == "E123") txt_E_123.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E456") txt_E_456.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E789") txt_E_789.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "E101112") txt_E_101112.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

             
                if (oDet.NamespaceID == "FGH1") txt_FGH_1.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH2") txt_FGH_2.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH3") txt_FGH_3.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH4") txt_FGH_4.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH5") txt_FGH_5.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH6") txt_FGH_6.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH7") txt_FGH_7.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH8") txt_FGH_8.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH9") txt_FGH_9.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH10") txt_FGH_10.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH11") txt_FGH_11.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);
                if (oDet.NamespaceID == "FGH12") txt_FGH_12.Value = getNumeroProtocolo(oDet.IdDetalleProtocolo);

                 
            }
        }

        private string getNumeroProtocolo(int idDetalleProtocolo)
        {
            try {
                Business.Data.Laboratorio.DetalleProtocolo oP = new Business.Data.Laboratorio.DetalleProtocolo();
                oP = (Business.Data.Laboratorio.DetalleProtocolo)oP.Get(typeof(Business.Data.Laboratorio.DetalleProtocolo), idDetalleProtocolo);
                if (oP != null)
                    return oP.IdProtocolo.Numero.ToString();
                else
                    return "";
            }
            catch { return ""; }



        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if ((ddlOperador.SelectedItem.Text == "--Seleccione--") || (ddlOperador0.SelectedItem.Text == "--Seleccione--"))
                {
                    lblError.Text = "Debe seleccionar Operadores";
                    lblError.Visible = true;
                }
                else
                {
                    Session["errores"] = "";
                    Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
                    if (Request["id"] != null) oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["id"].ToString()));
                    Guardar(oRegistro, "A");

                    if (Request["id"] != null)
                    {
                        oRegistro.GrabarAuditoria("Modifica Placa", int.Parse(Session["idUsuario"].ToString()), "");

                    }
                    else oRegistro.GrabarAuditoria("Genera Placa", int.Parse(Session["idUsuario"].ToString()), "");
                    // guardarValorCelda (idPlaca, txt_AB_1.value)
                    Response.Redirect("PlacaEdit3.aspx?id=" + oRegistro.IdPlaca.ToString());
                }

            }
        }

        private void Guardar(Placa oRegistro, string estado)
        {
           
            oRegistro.Operador = ddlOperador.SelectedItem.Text + " -" + ddlOperador0.SelectedItem.Text;
            oRegistro.Equipo = lblEquipo.Text;
            oRegistro.Fecha =DateTime.Parse( lblFecha.Text);

            if (Request["id"] == null)
            {
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            }
            oRegistro.Estado = estado;
            oRegistro.Save();
            if (estado=="C")            oRegistro.GrabarAuditoria("Graba y Cierra", int.Parse(Session["idUsuario"].ToString()), "");


            //Caro: hago que recorra las determinacoines del tipo de placa
            oRegistro.BorrarDetalle();
            string m_strSQL = @"select distinct idItem from [LAB_DetalleTipoPlaca] where [idTipoPlaca] =2";

                      DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                int iddet = int.Parse(Ds.Tables[0].Rows[i][0].ToString());

                //

                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), iddet);

                if (oRegistro.Equipo == "Promega")
                {
                  //  oRegistro.BorrarDetalle();
                    guardarvalorCelda(oRegistro, oItem, txt_AB_2.Value, txt_AB_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_3.Value, txt_AB_3.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_4.Value, txt_AB_4.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_5.Value, txt_AB_5.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_6.Value, txt_AB_6.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_7.Value, txt_AB_7.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_8.Value, txt_AB_8.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_9.Value, txt_AB_9.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_10.Value, txt_AB_10.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_11.Value, txt_AB_11.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_AB_12.Value, txt_AB_12.ID);

                    guardarvalorCelda(oRegistro, oItem, txt_CD_1.Value, txt_CD_1.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_2.Value, txt_CD_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_2.Value, txt_CD_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_3.Value, txt_CD_3.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_4.Value, txt_CD_4.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_5.Value, txt_CD_5.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_6.Value, txt_CD_6.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_7.Value, txt_CD_7.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_8.Value, txt_CD_8.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_9.Value, txt_CD_9.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_10.Value, txt_CD_10.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_11.Value, txt_CD_11.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_CD_12.Value, txt_CD_12.ID);

                    guardarvalorCelda(oRegistro, oItem, txt_EF_1.Value, txt_EF_1.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_2.Value, txt_EF_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_2.Value, txt_EF_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_3.Value, txt_EF_3.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_4.Value, txt_EF_4.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_5.Value, txt_EF_5.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_6.Value, txt_EF_6.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_7.Value, txt_EF_7.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_8.Value, txt_EF_8.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_9.Value, txt_EF_9.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_10.Value, txt_EF_10.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_11.Value, txt_EF_11.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_EF_12.Value, txt_EF_12.ID);


                    guardarvalorCelda(oRegistro, oItem, txt_GH_1.Value, txt_GH_1.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_2.Value, txt_GH_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_2.Value, txt_GH_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_3.Value, txt_GH_3.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_4.Value, txt_GH_4.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_5.Value, txt_GH_5.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_6.Value, txt_GH_6.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_7.Value, txt_GH_7.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_8.Value, txt_GH_8.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_9.Value, txt_GH_9.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_10.Value, txt_GH_10.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_11.Value, txt_GH_11.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_GH_12.Value, txt_GH_12.ID);

                }

                if (oRegistro.Equipo == "Promega-30M")
                {
                  //  oRegistro.BorrarDetalle();
                    //   guardarvalorCelda(oRegistro, oItem,txt_ABC_1.Value, txt_ABC_1.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_2.Value, txt_ABC_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_3.Value, txt_ABC_3.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_4.Value, txt_ABC_4.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_5.Value, txt_ABC_5.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_6.Value, txt_ABC_6.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_7.Value, txt_ABC_7.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_8.Value, txt_ABC_8.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_9.Value, txt_ABC_9.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_10.Value, txt_ABC_10.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_11.Value, txt_ABC_11.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_ABC_12.Value, txt_ABC_12.ID);

                    guardarvalorCelda(oRegistro, oItem, txt_D_123.Value, txt_D_123.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_D_456.Value, txt_D_456.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_D_789.Value, txt_D_789.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_D_101112.Value, txt_D_101112.ID);

                    guardarvalorCelda(oRegistro, oItem, txt_E_123.Value, txt_E_123.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_E_456.Value, txt_E_456.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_E_789.Value, txt_E_789.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_E_101112.Value, txt_E_101112.ID);

                    guardarvalorCelda(oRegistro, oItem, txt_FGH_1.Value, txt_FGH_1.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_2.Value, txt_FGH_2.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_3.Value, txt_FGH_3.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_4.Value, txt_FGH_4.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_5.Value, txt_FGH_5.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_6.Value, txt_FGH_6.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_7.Value, txt_FGH_7.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_8.Value, txt_FGH_8.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_9.Value, txt_FGH_9.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_10.Value, txt_FGH_10.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_11.Value, txt_FGH_11.ID);
                    guardarvalorCelda(oRegistro, oItem, txt_FGH_12.Value, txt_FGH_12.ID);



                }

                if (oRegistro.Equipo == "Alplex")
                {
                  //  oRegistro.BorrarDetalle();
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA2.Value, txtA2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA3.Value, txtA3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA4.Value, txtA4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA5.Value, txtA5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA6.Value, txtA6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA7.Value, txtA7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA8.Value, txtA8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA9.Value, txtA9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA10.Value, txtA10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA11.Value, txtA11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtA12.Value, txtA12.ID);

                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB1.Value, txtB1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB2.Value, txtB2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB3.Value, txtB3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB4.Value, txtB4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB5.Value, txtB5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB6.Value, txtB6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB7.Value, txtB7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB8.Value, txtB8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB9.Value, txtB9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB10.Value, txtB10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB11.Value, txtB11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtB12.Value, txtB12.ID);

                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC1.Value, txtC1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC2.Value, txtC2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC3.Value, txtC3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC4.Value, txtC4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC5.Value, txtC5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC6.Value, txtC6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC7.Value, txtC7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC8.Value, txtC8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC9.Value, txtC9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC10.Value, txtC10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC11.Value, txtC11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtC12.Value, txtC12.ID);

                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD1.Value, txtD1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD2.Value, txtD2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD3.Value, txtD3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD4.Value, txtD4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD5.Value, txtD5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD6.Value, txtD6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD7.Value, txtD7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD8.Value, txtD8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD9.Value, txtD9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD10.Value, txtD10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD11.Value, txtD11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtD12.Value, txtD12.ID);


                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE1.Value, txtE1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE2.Value, txtE2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE3.Value, txtE3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE4.Value, txtE4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE5.Value, txtE5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE6.Value, txtE6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE7.Value, txtE7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE8.Value, txtE8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE9.Value, txtE9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE10.Value, txtE10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE11.Value, txtE11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtE12.Value, txtE12.ID);

                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF1.Value, txtF1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF2.Value, txtF2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF3.Value, txtF3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF4.Value, txtF4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF5.Value, txtF5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF6.Value, txtF6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF7.Value, txtF7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF8.Value, txtF8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF9.Value, txtF9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF10.Value, txtF10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF11.Value, txtF11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtF12.Value, txtF12.ID);

                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG1.Value, txtG1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG2.Value, txtG2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG3.Value, txtG3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG4.Value, txtG4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG5.Value, txtG5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG6.Value, txtG6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG7.Value, txtG7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG8.Value, txtG8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG9.Value, txtG9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG10.Value, txtG10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG11.Value, txtG11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtG12.Value, txtG12.ID);


                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH1.Value, txtH1.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH2.Value, txtH2.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH3.Value, txtH3.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH4.Value, txtH4.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH5.Value, txtH5.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH6.Value, txtH6.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH7.Value, txtH7.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH8.Value, txtH8.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH9.Value, txtH9.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH10.Value, txtH10.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH11.Value, txtH11.ID);
                    guardarvalorCeldaAlplex(oRegistro, oItem, txtH12.Value, txtH12.ID);


                }
            }
            }

        private void guardarvalorCeldaAlplex(Placa oRegistro, Item oItem, string value, string iD)
        {

            try {

                if (value != "")
                {
                    Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                    oP = (Business.Data.Laboratorio.Protocolo)oP.Get(typeof(Business.Data.Laboratorio.Protocolo), "Numero", value);

                    if (oP != null)
                    {

                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                        crit.Add(Expression.Eq("IdProtocolo", oP));
                        crit.Add(Expression.Eq("IdSubItem", oItem));


                        IList items = crit.List();
                        if (items.Count > 0)
                        {
                            foreach (DetalleProtocolo oDet in items)
                            {
                                //  bool existenOtraPlaca = VerificaExistePlaca(oDet);
                                // if (!existenOtraPlaca)
                                if (oDet.IdUsuarioValida == 0)
                                {
                                    Business.Data.Laboratorio.DetalleProtocoloPlaca oDetPlaca = new Business.Data.Laboratorio.DetalleProtocoloPlaca();

                                    oDetPlaca.IdPlaca = oRegistro.IdPlaca;
                                    oDetPlaca.NamespaceID = iD.Replace("txt", "");
                                    oDetPlaca.IdDetalleProtocolo = oDet.IdDetalleProtocolo;

                                    oDetPlaca.Save();


                                    oDet.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Vincula a Placa Alplex", int.Parse(Session["idUsuario"].ToString()), "", oRegistro.IdPlaca.ToString());
                                }
                                else
                                { Session["errores"] += "- Protocolo " + value + " ya validado. No es posible incorporar a la placa."; }

                            }
                        }

                        else
                        { Session["errores"] += "- Protocolo " + value + "  no contiene Codigo " + oC.CodigoCovid; }
                    }
                    else
                    { Session["errores"] += "- Numero " + value + " no existe o no es válido"; }
                }
            }
            catch (Exception ex)
            {
                Session["errores"] += "-Numero " + value + " no existe o no es válido";
            }
        }

        private void guardarvalorCelda(Placa oRegistro, Item oItem, string value, string iD)
        {
            try {
                //levantar id=txt_AB_1 -- controlito
                string[] arr = iD.Split('_');
                string celda1 = arr[1].ToString(); //"AB"
                string celda2 = arr[2].ToString(); // "2"

                if (value != "")
                {
                    Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                    oP = (Business.Data.Laboratorio.Protocolo)oP.Get(typeof(Business.Data.Laboratorio.Protocolo), "Numero", value);

                    if (oP != null)
                    {

                        DetalleProtocolo oDetalle = new DetalleProtocolo();
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                        crit.Add(Expression.Eq("IdProtocolo", oP));
                        crit.Add(Expression.Eq("IdSubItem", oItem));


                        IList items = crit.List();
                        if (items.Count >= 0)
                        {
                            foreach (DetalleProtocolo oDet in items)
                            {
                                //bool existenOtraPlaca = VerificaExistePlaca(oDet);
                                //if (!existenOtraPlaca)
                                if (oDet.IdUsuarioValida == 0)
                                {
                                    Business.Data.Laboratorio.DetalleProtocoloPlaca oDetPlaca = new Business.Data.Laboratorio.DetalleProtocoloPlaca();

                                    oDetPlaca.IdPlaca = oRegistro.IdPlaca;
                                    oDetPlaca.NamespaceID = celda1 + celda2;
                                    oDetPlaca.IdDetalleProtocolo = oDet.IdDetalleProtocolo;

                                    oDetPlaca.Save();


                                    oDet.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Vincula a Placa Promega", int.Parse(Session["idUsuario"].ToString()), "", oRegistro.IdPlaca.ToString());
                                }
                                else
                                { Session["errores"] += "- Protocolo " + value + "  ya fue validado. No es posible incorporar en la placa"; }

                            }
                        }
                        else
                        { Session["errores"] += "- Protocolo " + value + "  no contiene Codigo " + oC.CodigoCovid; }

                    }
                    else
                    { Session["errores"] += "-Numero " + value + " no existe o no es válido"; }

                }

            }
            catch (Exception ex)
            {
                Session["errores"] += "-Numero " + value + " no existe o no es válido";
            }

              
             
        }

        private bool VerificaExistePlaca(DetalleProtocolo oDetP)
        {
            bool existe = false;
            
            DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
            crit.Add(Expression.Eq("IdDetalleProtocolo", oDetP.IdDetalleProtocolo));

            


            IList items = crit.List();
            foreach (DetalleProtocoloPlaca oDet in items)
            {
                

                Placa oRegistro = new  Placa();               
                oRegistro = (Placa)oRegistro.Get(typeof( Placa),"IdPlaca", oDet.IdPlaca, "Baja", false);
                if (oRegistro != null)
                {
                    if (oRegistro.IdPlaca.ToString() != Request["id"].ToString())
                    {
                        existe = true;
                        break;
                    }
                }
                
                  
                }
            return existe;
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            if (Request["id"] != null)
            {
                oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["id"].ToString()));
                Imprimir(oRegistro.IdPlaca.ToString());
            }

        }

        private void Imprimir(string id)
        {


            Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
            oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(id));
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            CrystalReportSource oCr = new CrystalReportSource();


            switch (oRegistro.Equipo)
            {
                case "Promega":

                    {
                        oCr.Report.FileName = "PlacaPromega.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
                case "Alplex":
                    {
                        oCr.Report.FileName = "PlacaAlplex.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
                case "Promega-30M":

                    {
                        oCr.Report.FileName = "PlacaPromega30M.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getCeldas());

                    }
                    break;
            }



                    oCr.DataBind();

                oRegistro.GrabarAuditoria("Imprime Placa", int.Parse(Session["idUsuario"].ToString()), "");
            string nombrearchivo = "Placa_" + oRegistro.IdPlaca.ToString() + "_" + oRegistro.Fecha.ToShortDateString().Replace("/", "");


            oCr.ReportDocument.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true,nombrearchivo );


        }

        protected void btnCerrar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if ((ddlOperador.SelectedItem.Text == "--Seleccione--") || (ddlOperador0.SelectedItem.Text == "--Seleccione--"))
                {
                    lblError.Text = "Debe seleccionar Operadores";
                    lblError.Visible = true;
                }
                else
                {
                    Session["errores"] = "";
                    Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
                    if (Request["id"] != null) oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["id"].ToString()));
                    Guardar(oRegistro, "C");
                 //   Imprimir(oRegistro.IdPlaca.ToString());
                    Response.Redirect("PlacaEdit2.aspx?id=" + oRegistro.IdPlaca.ToString());
                }
            }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            //ExportarTexto();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlacaList.aspx", false);
        }
        //private void ExportarTexto()
        //{
        //    Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
        //    if (Request["id"] != null) oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["id"].ToString()));



        //    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

        //    if (Directory.Exists(directorio))
        //    {
        //        string archivo = directorio + "\\LIMS003_384 wells.csv"; /// probablemente tiene que ser extension .ana (renombrar)

        //        using (StreamWriter streamwriter = new StreamWriter(archivo))
        //        {
        //            string s_nombreArchivo = "";
        //            string linea = "Plate Header,,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Filed,Data,,Instruction,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Version,1,,Do not modify this field.,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Plate Size,384,,Do not modify this field.,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Plate Type,BR White,,Allowed values (BR White,BR Clear),,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Scan Mode,SYBR/FAM Only,,""Allowed values (""SYBR / FAM Only"""," ""All Channels"""," ""FRET"")"",,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Run ID,,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Run Notes,,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Run Protocol,CFX_2stepAmp.prcl,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Data File,,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "TBD,,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Plate Data,,,,,,,,,,,,,,,,,,,,,,,,,";
        //            streamwriter.WriteLine(linea);
        //            linea = "Well,Ch1 Dye,Ch2 Dye,Ch3 Dye,Ch4 Dye,FRET,Sample Type,Sample Name,Ch1 Target Name,Ch2 Target Name,Ch3 Target Name,Ch4 Target Name,FRET Target Name,Biological Set Name,Replicate,Ch1 Quantity,Ch2 Quantity,Ch3 Quantity,Ch4 Quantity,FRET Quantity,Well Note,Ch1 Well Color,Ch2 Well Color,Ch3 Well Color,Ch4 Well Color,FRET Well Color";
        //            streamwriter.WriteLine(linea);


        //            DetalleProtocoloPlaca oDetalle = new DetalleProtocoloPlaca();
        //            ISession m_session = NHibernateHttpModule.CurrentSession;
        //            ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocoloPlaca));
        //            crit.Add(Expression.Eq("IdPlaca", oRegistro.IdPlaca));



        //            IList items = crit.List();

        //            foreach (DetalleProtocoloPlaca oDet in items)
        //            {
        //                if (oDet.NamespaceID == "A02")
        //                {
        //                    linea = "A02,FAM,,,,,Unknown," + getNumeroProtocolo (oDet.IdDetalleProtocolo)+ ",B.microti,,,,,,,,,,,,,-16776961,,,,";
        //                    streamwriter.WriteLine(linea);
        //                }

        //                if (oDet.NamespaceID == "A03")
        //                {
        //                    linea = "A03,FAM,,,,,Unknown,MA48725,B.microti,,,,,,,,,,,,,-10496";
        //                    streamwriter.WriteLine(linea);

        //                }
        //                if (oDet.NamespaceID == "A04")
        //                {
        //                    linea = "A04,FAM,,,,,Unknown,MA48725,B.microti,,,,,,,,,,,,,-10496";
        //                    streamwriter.WriteLine(linea);

        //                }
        //                if (oDet.NamespaceID == "A05")
        //                {
        //                    linea = "A04,FAM,,,,,Unknown,MA48725,B.microti,,,,,,,,,,,,,-10496";
        //                    streamwriter.WriteLine(linea);

        //                }
        //                if (oDet.NamespaceID == "A06")
        //                {
        //                    linea = "A04,FAM,,,,,Unknown,MA48725,B.microti,,,,,,,,,,,,,-10496";
        //                    streamwriter.WriteLine(linea);

        //                }
        //                if (oDet.NamespaceID == "A07")
        //                {
        //                    linea = "A04,FAM,,,,,Unknown,MA48725,B.microti,,,,,,,,,,,,,-10496";
        //                    streamwriter.WriteLine(linea);

        //                }
        //            }

        //            streamwriter.Close();

        //            string archivo_ruta = MapPath("TextFile.csv");
        //            Response.Clear();
        //            Response.Buffer = true;
        //            Response.ContentType = "text/plain";
        //            Response.AppendHeader("content-disposition", "attachment; filename=" + s_nombreArchivo + ".txt");
        //            Response.Clear();
        //            Response.WriteFile(archivo_ruta);
        //            Response.End();


        //        }
        //    }

        //    //            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(System.IO.File.Open
        //    //(MapPath("TextFile.txt"), System.IO.FileMode.OpenOrCreate));




        //}
    }


}


