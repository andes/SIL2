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
using Business.Data;
using InfoSoftGlobal;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO;

namespace WebLab.Informes
{
    public partial class HistoriaClinica : System.Web.UI.Page
    {
        Paciente oPaciente = new Paciente();

        public CrystalReportSource oCr = new CrystalReportSource();
        

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;

        }  

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                   
                DataTable dt = new DataTable();
                dt = LlenarDatos();
                if (dt.Rows.Count > 0)
                { 
                    MostrarPaciente();
                    gvHistorico.DataSource = dt;
                    gvHistorico.DataBind();
                    Item oItem = new Item();
                    oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idAnalisis"].ToString()));
                    lblAnalisis.Text = oItem.Codigo + " - " + oItem.Nombre;
                    if (oItem.IdTipoResultado == 1) 
                    { if (coincideUnidadMedida(dt))
                        FCLiteral.Text = CreateChart(dt);
                    }
                }
                else
                 Response.Redirect("SinDatos.aspx", false);
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
        private bool coincideUnidadMedida(DataTable dt)
        {bool hay=true;
              string unidad ="";
              if (dt.Rows.Count > 0)
              {
                  for (int i = 0; i < dt.Rows.Count; i++)
                  {
                      if (i == 0)
                          unidad = dt.Rows[i][6].ToString();
                      else
                      {
                          if (unidad != dt.Rows[i][6].ToString())
                              hay = false; break;
                      }
                  }
              }
              return hay;
                        
        }

        private string CreateChart(DataTable dt)
        {

            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idAnalisis"].ToString()));

            string valorminimo = Math.Round(oItem.ValorMinimo, 0).ToString();
            string strXML = "<graph caption='" + lblAnalisis.Text + "' subcaption='' xAxisName='Protocolo' yAxisMinValue='" + valorminimo + "' yAxisName='Resultado' decimalPrecision='2' formatNumberScale='1' showNames='1' " +
                " showValues='0' showAlternateHGridColor='1'  AlternateHGridColor='ff5904' divLineColor='ff5904' divLineAlpha='20' alternateHGridAlpha='5'>";

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strXML += "<set name='" + dt.Rows[i][2].ToString() + "' value='" + dt.Rows[i][4].ToString().Replace(",",".") + "' hoverText='" + dt.Rows[i][2].ToString() + "' />";
                }
            }        

            strXML += "</graph>";
            return FusionCharts.RenderChart("../FusionCharts/FCF_Line.swf", "", strXML, "Sales", "600", "250", false, false);

        }

        private void MostrarPaciente()
        {
       
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente),int.Parse( Request["idPaciente"].ToString()));
            if (oPaciente.IdEstado==2)
                lblNumeroDocumento.Text = "Sin DNI (temporal)";
            else
            lblNumeroDocumento.Text = oPaciente.NumeroDocumento.ToString();
             
              
            lblPaciente.Text =  oPaciente.Apellido + " " + oPaciente.Nombre;


            lblSexo.Text =  oPaciente.getSexo() ;
            lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();
            lblContacto.Text = oPaciente.InformacionContacto;         
        }                   
      
        private DataTable LlenarDatos()
        {               
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            
            cmd.CommandText = "LAB_HistoricoAnalisis";
            cmd.Parameters.Add("@fechaDesde", SqlDbType.NVarChar);
            cmd.Parameters.Add("@fechaHasta", SqlDbType.NVarChar);

            if (Request["fechaDesde"].ToString() != "")
            {
               DateTime fecha1 = DateTime.Parse(Request["fechaDesde"].ToString());
               cmd.Parameters["@fechaDesde"].Value = fecha1.ToString("yyyyMMdd");
            }
            else
               cmd.Parameters["@fechaDesde"].Value ="";

            if (Request["fechaHasta"].ToString() != "")
            {
               DateTime fecha2 = DateTime.Parse(Request["fechaHasta"].ToString());
               cmd.Parameters["@fechaHasta"].Value = fecha2.ToString("yyyyMMdd");
            }
            else
               cmd.Parameters["@fechaHasta"].Value = "";                      

            cmd.Parameters.Add("@idAnalisis", SqlDbType.NVarChar);
            cmd.Parameters["@idAnalisis"].Value = Request["idAnalisis"].ToString();       

            cmd.Parameters.Add("@idPaciente", SqlDbType.NVarChar);
            cmd.Parameters["@idPaciente"].Value = Request["idPaciente"].ToString();

            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            return Ds.Tables[0];
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("HistoriaClinicaFiltro.aspx?Tipo=Analisis", false);
        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {
            ExportarPDF();
        }

        private void ExportarPDF()
        {
            DataTable dt = new DataTable();
            dt =LlenarDatos();
            if (dt.Rows.Count > 0)
            {
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;

                ParameterDiscreteValue encabezado4 = new ParameterDiscreteValue();
                encabezado4.Value = lblAnalisis.Text;

                ParameterDiscreteValue encabezado5 = new ParameterDiscreteValue();
                encabezado5.Value = lblNumeroDocumento.Text;

                ParameterDiscreteValue encabezado6 = new ParameterDiscreteValue();
                encabezado6.Value = lblPaciente.Text;

                ParameterDiscreteValue encabezado7 = new ParameterDiscreteValue();
                encabezado7.Value = lblFechaNacimiento.Text;

                ParameterDiscreteValue encabezado8 = new ParameterDiscreteValue();
                encabezado8.Value = lblSexo.Text;

                            
                Item oItem = new Item();
                oItem= (Item) oItem.Get(typeof(Item),int.Parse( Request["idAnalisis"].ToString()));
                if (oItem.IdTipoResultado==1)             
                    oCr.Report.FileName = "HistoricoAnalisis.rpt";                
                else
                    oCr.Report.FileName = "HistoricoAnalisisNoNumerico.rpt";   
                
                ParameterDiscreteValue subTitulo = new ParameterDiscreteValue();
                subTitulo.Value = Request["fechaDesde"].ToString() + " - " + Request["fechaHasta"].ToString();
                
                if ((Request["fechaDesde"].ToString() != "") && (Request["fechaHasta"].ToString() != ""))
                    subTitulo.Value = Request["fechaDesde"].ToString() + " - " + Request["fechaHasta"].ToString();
                else
                    subTitulo.Value = "";
                             

                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(encabezado4);
                oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(subTitulo);
                oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(encabezado5);
                oCr.ReportDocument.ParameterFields[6].CurrentValues.Add(encabezado6);
                oCr.ReportDocument.ParameterFields[7].CurrentValues.Add(encabezado7);
                oCr.ReportDocument.ParameterFields[8].CurrentValues.Add(encabezado8);
                
           
                oCr.DataBind();

                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Historial_Resultados.pdf");
                //MemoryStream oStream; // using System.IO
                //oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("Content-Disposition", "attachment;filename=Historial_Resultados.pdf");

                //Response.BinaryWrite(oStream.ToArray());
                //Response.End();                       
            }
            else
                Response.Redirect("SinDatos.aspx", false);
        }

        protected void gvHistorico_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idAnalisis"].ToString()));
                if (oItem.IdTipoResultado == 1)
                {
                    e.Row.Cells[3].Visible = true;
                    e.Row.Cells[4].Visible = false;
                }
                else
                {
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = true;

                }
            }
            catch { }

        }
    }
}
