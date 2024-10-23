using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace InterfaceNotificacion
{
    public class ELog
    {
        public static void save(object obj, Exception ex)
        {
            string fecha = System.DateTime.Now.ToString("yyyyMMdd");
            string hora = System.DateTime.Now.ToString("HH:mm:ss");
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + "\\logInterfaceSISA_" + fecha + ".txt"))
            {
                StackTrace stacktrace = new StackTrace();
                file.WriteLine(obj.GetType().FullName + " " + hora);
                file.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + ex.Message);
                file.WriteLine("");

            }
        }

        public static void save(object obj, string ex)
        {
            string fecha = System.DateTime.Now.ToString("yyyyMMdd");
            string hora = System.DateTime.Now.ToString("HH:mm:ss");
            

            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string nombreArchivo = path + "\\logInterfaceSISA_" + fecha + ".txt";


            if (File.Exists(nombreArchivo))
            {
                System.Collections.Generic.List<string> nuevo = new System.Collections.Generic.List<string>();
                nuevo.Add(hora+ " " +ex);
                System.IO.File.WriteAllLines(nombreArchivo, nuevo);
            }
            else
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(nombreArchivo))
            {
                StackTrace stacktrace = new StackTrace();
                file.WriteLine(obj.GetType().FullName + " " + hora);
                file.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + ex);
                file.WriteLine("");

            }


             
        }
    }
}
