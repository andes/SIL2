using System;
using System.Web.UI;

namespace WebLab.Estadisticas
{
    public partial class GraficoChart : System.Web.UI.UserControl
    {
        public string LabelsJson { get; set; } //Obligatorio. 
        public string DatosJson { get; set; } //Obligatorio. Recibe resultados numericos
        public string DatosStringJson { get; set; } //recibe resultados de otro tipo
        public string TipoGrafico { get; set; } //Obligatorio. Tipo Pie, bar, line
        public string TituloJson { get; set; } //Obligatorio
        public string Subtitulo { get; set; }
        public string minimo { get; set; } //valor minimo de eje Y
        public string tituloX { get; set; } //titulo de eje X
        public string tituloY { get; set; }  //titulo de eje Y
    }
}
