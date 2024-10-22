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
using Business.Data.AutoAnalizador;
using System.IO;
using NHibernate;
using Business;
using NHibernate.Expression;
using Business.Data.Laboratorio;
using Business.Data;
using System.Text;

namespace WebLab.AutoAnalizador
{
    public partial class EnvioMensaje : System.Web.UI.Page
    {
        Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else
                Response.Redirect("../FinSesion.aspx", false);



        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.lblMensaje.Text = "Se han enviado " + Request["Cantidad"].ToString() + " muestras al equipo " + Request["Equipo"].ToString();
                if (Request["Equipo"].ToString() == "Metrolab")
                {
                    this.lblMensaje.Text = "Se han procesado " + Request["Cantidad"].ToString() + " protocolos.";
                    btnDescargarArchivo.Visible = true;
                }
                if (Request["Equipo"].ToString() == "Incca")
                {
                    this.lblMensaje.Text = "Se han procesado " + Request["Cantidad"].ToString() + " protocolos.";
                    btnDescargarArchivo.Visible = true;
                }
            }

        }

        private void GenerarArchivoFormatoMetrolabExportar()
        {
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

            if (Directory.Exists(directorio))
            {
                string archivo = directorio + "\\Muestras.ana"; /// probablemente tiene que ser extension .ana (renombrar)

                 using (StreamWriter sw = new StreamWriter(archivo, false, Encoding.UTF8))
               // using (StreamWriter sw = new StreamWriter(archivo))
                {
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloEnvio));
                    crit.Add(Expression.Eq("Equipo", "Metrolab"));
                    crit.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));
                    
                    IList detalle = crit.List();
                    if (detalle.Count > 0)
                    {
                        string linea = "";
                     
                        foreach (ProtocoloEnvio oDetalle in detalle)
                        {
                            string nroProtocolo = GetNumero(oDetalle.NumeroProtocolo); Protocolo oProtocolo = new Protocolo();
                            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), "Numero", int.Parse(nroProtocolo));
                            if (oProtocolo.IdEfector == oUser.IdEfector)
                            {
                                string[] datosPaciente = oDetalle.Paciente.Split((";").ToCharArray());

                                string dni = datosPaciente[0].ToString();
                                string nombre = datosPaciente[1].ToString().PadRight(30, ' ');
                                if (nombre.Length > 30) nombre = nombre.Substring(0, 30);
                                int cantidadPruebas = oDetalle.Iditem.Split((";").ToCharArray()).Length;

                                //linea formato metrolab= numeroprotocolo (hasta 10c);N(1c);nombrePaciente(hasta 30c);documento(hasta 12c);edad(3c);sexo(M,F o blanco);cantidadPruebas (1 o 2c);pruebas
                                linea = oDetalle.NumeroProtocolo + ";N;" + nombre + ";" + dni + ";" + oDetalle.AnioNacimiento.PadLeft(3, '0') + ";" + oDetalle.Sexo + ";" + cantidadPruebas.ToString() + ";" + oDetalle.Iditem;
                                sw.Write(linea);
                                sw.Write("\r\n");//retorno de carro y avance de linea
                            }
                        }
                    }
                }              
                DescargarArchivo(archivo);


              
            }
        }

        private string GetNumero(string numeroProtocolo)
        {
            Utility oUtil = new Utility();
            if (oUtil.EsNumerico(numeroProtocolo))
                return numeroProtocolo;
            else
            {
                string[] datosPaciente = numeroProtocolo.Split(("-").ToCharArray());

                return datosPaciente[0].ToString();

            }
        }

        private void DescargarArchivo(string archivo)
        {
         
            System.IO.FileInfo toDownload = new System.IO.FileInfo(archivo);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
            HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(archivo);
            HttpContext.Current.Response.End();
            ////////////////////////////////////////////////////////////////////////////
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProtocoloBusqueda2.aspx?Equipo=" + Request["Equipo"].ToString(), false);
        }

        protected void btnDescargarArchivo_Click(object sender, EventArgs e)
        {
            if (Request["Equipo"].ToString() == "Metrolab")
            {
                GenerarArchivoFormatoMetrolabExportar();
            }
            if (Request["Equipo"].ToString() == "Incca")
            {
                GenerarArchivoFormatoinccaExportar();
            }
           
        }

        private void GenerarArchivoFormatoinccaExportar()
        {
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

            if (Directory.Exists(directorio))
            {
                string archivo = directorio + "\\PacientesIncca.csv"; /// probablemente tiene que ser extension .ana (renombrar)

                //  using (StreamWriter sw = new StreamWriter(archivo))
                using (StreamWriter sw = new StreamWriter(archivo, false, Encoding.UTF8))
                {
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloEnvio));
                    crit.Add(Expression.Eq("Equipo", "Incca"));
                    crit.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));

                    IList detalle = crit.List();
                    if (detalle.Count > 0)
                    {
                        string linea = "";
                        foreach (ProtocoloEnvio oDetalle in detalle)
                        {
                            string nroProtocolo = GetNumero(oDetalle.NumeroProtocolo); Protocolo oProtocolo = new Protocolo();
                            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), "Numero", int.Parse(nroProtocolo));
                            if (oProtocolo.IdEfector == oUser.IdEfector)
                            {
                                string[] datosPaciente = oDetalle.Paciente.Split((";").ToCharArray());

                                string dni = datosPaciente[0].ToString();
                                string apellido = datosPaciente[1].ToString().PadRight(30, ' ');
                                if (apellido.Length > 30) apellido = apellido.Substring(0, 30);


                                string nombre = datosPaciente[2].ToString().PadRight(30, ' ');
                                if (nombre.Length > 30) nombre = nombre.Substring(0, 30);
                              
                                int cantidadPruebas = oDetalle.Iditem.Split((";").ToCharArray()).Length;

                                string tipo = "1";
                                if (oDetalle.Urgente == "N") tipo = "0";

                                nombre = nombre.Replace(",", "").Trim();

                                apellido = apellido.Replace(",", "").Trim();  

                                string cama = oDetalle.SectorSolicitante.Trim();
                                //linea formato incca= 
                                //PAC,protocolo,Nombre del Paciente,Apellido delPaciente,Edad,SEXO *,Doctor,Cama,TIPO *,Metodo1,Metodo2,...,MetodoN

                                //lina formato incca=Software versions 2.6.1 or higher.
                                //PAC, ID, Patient name, Patient surname, Age, SEX*, Doctor, Bed, STAT*,PID, comment, Method1, Method2, ..., MethodN
                                linea = "PAC,"+oDetalle.NumeroProtocolo + "," + nombre + "," + apellido+","+ oDetalle.AnioNacimiento + "," + oDetalle.Sexo +
                                    ","+oDetalle.MedicoSolicitante.Replace(",", "")  +","+ cama + ","+ tipo+","+ dni+",," +   oDetalle.Iditem;
                                sw.Write(linea);
                                sw.Write("\r\n");//retorno de carro y avance de linea
                            }
                        }
                    }
                }
                DescargarArchivo(archivo);



            }
        }
    }
}
