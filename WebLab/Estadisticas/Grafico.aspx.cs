using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using InfoSoftGlobal;
using System.Web.Script.Serialization;

namespace WebLab.Estadisticas
{
    public partial class Grafico : System.Web.UI.Page
    {
        public string LabelsJson { get; set; }
        public string DatosJson { get; set; }
        public string TipoGrafico { get; set; }
        public string TituloJson { get; set; }
        public string TooltipsJson { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mostrarGraficoNuevo(int.Parse(Request["tipo"].ToString()));
            }
        }

        // ======================================================
        // mostrarGraficoNuevo
        // ======================================================
        // Esta función prepara la información que será utilizada
        // por Chart.js para renderizar gráficos estadísticos.
        //
        // Flujo general:
        // 1. Define el título del gráfico según el parámetro recibido.
        // 2. Determina el tipo de gráfico (bar o pie).
        // 3. Lee los datos enviados en Request["valores"].
        // 4. Separa labels y cantidades.
        // 5. Serializa los datos a JSON para JavaScript.
        // ======================================================


        private void mostrarGraficoNuevo(int p)
        {
            string tipo = "";
            string s_tipo = "";

            List<string> labels = new List<string>();
            List<int> datos = new List<int>();

            // Tooltips personalizados
            //List<string> tooltips = new List<string>();

            switch (p)
            {
                case 0: s_tipo = "Casos por tipo de muestra";  break;
                case 1: s_tipo = "Aislamientos"; break;
                case 2: s_tipo = "Resistencia en ATB"; break;
                case 3: s_tipo = "Casos por Resultados Obtenidos"; break;
                case 4: s_tipo = "Casos de Mecanismos de Resistencia"; break;
            }
           

            // Tipo de gráfico
            if (Request["tipoGrafico"] != null)
            {
                if (Request["tipoGrafico"] == "barra")
                    tipo = "bar";
                else
                    tipo = "pie";
            }

            // Lectura de datos
            if (!string.IsNullOrEmpty(Request["valores"]))
            {
                string[] arr = Request["valores"].Split(';');

                foreach (string item in arr)
                {
                    string[] items = item.Split('|');

                    string label = items[0];
                    int valor = int.Parse(items[1]);

                    labels.Add(label);
                    datos.Add(valor);
                }
            }


            var js = new JavaScriptSerializer();

            LabelsJson = js.Serialize(labels);
            DatosJson = js.Serialize(datos);
            //TooltipsJson = js.Serialize(tooltips);

            TipoGrafico = js.Serialize(tipo);
            TituloJson = js.Serialize(s_tipo);
        }
    }
}