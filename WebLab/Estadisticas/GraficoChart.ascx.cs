using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace WebLab.Estadisticas
{
    public partial class GraficoChart : System.Web.UI.UserControl
    {
        /// <summary>
		/// LabelsJson es un valor obligatorio 
		/// </summary>
        public string LabelsJson { get; set; } //Obligatorio. 
        /// <summary>
        /// DatosJson es un valor obligatorio 
        /// </summary>
        public string DatosJson { get; set; } //Obligatorio. Recibe resultados numericos
        /// <summary>
        /// DatosStringJson si se envian valores en texto
        /// </summary>
        public string DatosStringJson { get; set; } //recibe resultados de otro tipo
        /// <summary>
        /// TipoGrafico es un valor obligatorio 
        /// </summary>
        public string TipoGrafico { get; set; } //Obligatorio. Tipo Pie, bar, line
        /// <summary>
        /// TituloJson es un valor obligatorio 
        /// </summary>
        public string TituloJson { get; set; } //Obligatorio
        /// <summary>
        ///  
        /// </summary>
        public string Subtitulo { get; set; }
        /// <summary>
        /// Valor minimo del eje Y
        /// </summary>
        public string minimo { get; set; } //valor minimo de eje Y
        /// <summary>
        /// 
        /// </summary>
        public string tituloX { get; set; } //titulo de eje X
        /// <summary>
        /// 
        /// </summary>
        public string tituloY { get; set; }  //titulo de eje Y
        /// <summary>
        /// Controla si se muestran los labels en la leyenda (true por defecto)
        /// </summary>
        public string MostrarLabels { get; set; }

        /* 
         * ASP.NET WebForms está llamando al Render del control dos veces durante 
         * el ciclo de vida de la página(algo común con master pages, dependiendo 
         * de cómo se estructura el árbol de controles). *
         */

        /// <summary>
        /// Banderas para que se ejecute una sola vez DecodeLabels
        /// </summary>
        private string labelsDecodificadas;
        private bool seDecodificaronLabels;

        /// <summary>
        /// DecodeLabels: Retorna LabelsJson con entidades HTML decodificadas si MostrarLabels es true
        /// Ej: Si recibo "Orina, obtenci&#243;n desconocido" lo transformo a "Orina, obtención desconocido"
        /// </summary>
        public string LabelsJsonDecoded
        {
            get
            {
                if (!seDecodificaronLabels)
                {
                    labelsDecodificadas = DecodeLabels();
                    seDecodificaronLabels = true;
                }
                return labelsDecodificadas;
            }
        }

        private string DecodeLabels()
        {
            if (!string.IsNullOrEmpty(LabelsJson))
            {
                bool mostrar = string.IsNullOrEmpty(MostrarLabels) ||
                               MostrarLabels.Equals("true", StringComparison.OrdinalIgnoreCase);
                if (mostrar)
                {
                    var js = new JavaScriptSerializer();
                    var labels = js.Deserialize<List<string>>(LabelsJson);
                    if (labels != null)
                    {
                        labels = labels.Select(l => HttpUtility.HtmlDecode(l)).ToList();
                        return js.Serialize(labels);
                    }
                }
            }
            return LabelsJson;
        }
    }
}
