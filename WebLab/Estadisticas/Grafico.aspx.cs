using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InfoSoftGlobal;

namespace WebLab.Estadisticas
{
    public partial class Grafico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.Parse(Request["tipo"].ToString())!=2)

            FCLiteral.Text = mostrarGrafico(int.Parse(Request["tipo"].ToString()));
            else
                FCLiteral.Text = mostrarGrafico2(); ;
        }

        private string mostrarGrafico2()
        {
          //  string s_titulo = "";
          //  string s_tipo = "";
         //   string s_tipografico = "../FusionCharts/FCF_MSColumn3D.swf";

            string strXML = "<graph caption='Resistencia de Antibioticos' formatNumberScale='0' decimalPrecision='0'  showPercentageInLabel='1' >";    
            string strCategories = "<categories>";

    
            string strDataR = "<dataset seriesName='Resistente' color='9D080D'>";
            string strDataS = "<dataset seriesName='Sensible' color='F6BD0F'>";
            string strDataI = "<dataset seriesName='Intermedio' color='AFD8F8'>";

            string valorrecibido=Request["valores"].ToString();
            string[] arr = valorrecibido.Split(("=").ToCharArray());
            foreach (string item in arr)
            {
                string[] arr2 =item.ToString().Split((";").ToCharArray());
                if (arr2[0].ToString() != "")
                {
                    strCategories = strCategories + "<category name='" + arr2[0].ToString().Substring(0,5) + "' />";
                    if (arr2[1].ToString()=="")
                        strDataR = strDataR + "<set value='0' />";
                    else
                       strDataR = strDataR + "<set value='" + arr2[1] + "' />";

                    if (arr2[2].ToString() == "")
                        strDataS = strDataS + "<set value='0' />";
                    else
                        strDataS = strDataS + "<set value='" + arr2[2] + "' />";

                    if (arr2[3].ToString() == "")
                        strDataI = strDataI + "<set value='0' />";
                    else
                        strDataI = strDataI + "<set value='" + arr2[3] + "' />";
                }
            }

            //'Close <categories> element
            strCategories = strCategories + "</categories>";

            //'Close <dataset> elements
            strDataR= strDataR + "</dataset>";
            strDataS = strDataS + "</dataset>";
            strDataI = strDataI + "</dataset>";

            //'Assemble the entire XML now
            strXML = strXML + strCategories + strDataR + strDataS +strDataI +"</graph>";

            return FusionCharts.RenderChart("../FusionCharts/FCF_MSColumn3D.swf", "", strXML, "productSales", "800", "500", false, false);
        }

        private string mostrarGrafico(int p)
        {
            string s_titulo = "";
            string s_tipo = "";           
            string strXML = "";
            string strDetalle = "";

            //s_tipografico = "../FusionCharts/FCF_MSColumn3D.swf";

            s_titulo = "";// ddlAnalisis.SelectedItem.Text;
            if (p==0) s_tipo = "Casos por tipo de muestra";
            else
            { if (p == 3) s_tipo = "Casos por Resultados Obtenidos"; else        s_tipo = "Aislamientos"; }

            string s_ancho = "700";
            string s_alto= "400";

            string[] arr = Request["valores"].ToString().Split((";").ToCharArray());
            foreach (string item in arr)
            {
                strDetalle += "<set "+item+" />";
            }

            string s_tipografico = "../FusionCharts/FCF_Pie3D.swf";

            if (Request["tipoGrafico"] != null)
            {
                if (Request["tipoGrafico"].ToString() == "torta") 
                {
                    s_tipografico = "../FusionCharts/FCF_Pie3D.swf";
                    strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1' pieSliceDepth='30'  decimalPrecision='0' showNames='1'>";
                    strXML += strDetalle;
                    strXML += "</graph>";            
                }
                if (Request["tipoGrafico"].ToString() == "barra")
                {
                    s_ancho = "700";
                    s_alto = "350";
                    s_tipografico = "../FusionCharts/FCF_Column2D.swf";
                    strXML = "<graph caption='" + s_titulo + "' subCaption='" + s_tipo + "' showPercentageInLabel='1'  decimalPrecision='0' showNames='1' formatNumberScale='0'>";
                    strXML += strDetalle;
                    strXML += "</graph>";            
                }
            }               

            return FusionCharts.RenderChart(s_tipografico, p.ToString(), strXML, p.ToString(), s_ancho, s_alto, false, false);
        }

        
    }
}