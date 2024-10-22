using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceNotificacion
{
    public class Protocolo
    {
        public string protocolo { get; set; }
        public string tipo_documento { get; set; }
        public string numero_documento { get; set; }
        public string sexo { get; set; }
        public DateTime fecha_nacimiento { get; set; }
        public DateTime fecha_pedido { get; set; }
        public DateTime fecha_toma_muestra { get; set; }
        public string establecimiento_toma_muestra { get; set; }
        public string resultado { get; set; }
        public DateTime fecha_resultado { get; set; }
    }
}
