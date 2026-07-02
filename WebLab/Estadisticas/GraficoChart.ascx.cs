using System;
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
    }
}
