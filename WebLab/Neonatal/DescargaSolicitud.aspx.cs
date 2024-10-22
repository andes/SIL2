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
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using Business;
using NHibernate.Expression;

namespace WebLab.Neonatal
{
    public partial class DescargaSolicitud : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                VerificaPermisos("Descargar Derivaciones");
                if (Session["idUsuario"] == null)
                    Response.Redirect("../FinSesion.aspx", false);
            }
            
        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                      Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (fupSolicitud.FileName != "")
            {
                //arch.PostedFile.SaveAs("nuevo_nombre." + arch.PostedFile.FileName.Split('.')[1]);
                fupSolicitud.PostedFile.SaveAs(Server.MapPath("DatosImportados.txt"));

                string fic = Server.MapPath("DatosImportados.txt");
                string texto;

                System.IO.StreamReader sr = new System.IO.StreamReader(fic);
                texto = sr.ReadToEnd();
                sr.Close();

                int cantidad=Decodificar(texto);
                if (cantidad > 0)
                    Response.Redirect("SolicitudList.aspx", false);
                //else
                // response a pantalla que diga que no se descargaron solicitudes
                
                //this.fupLogo.SaveAs(Server.MapPath("~/Logo/" + oC.RutaLogo));                                   
            }
        }

        private int Decodificar(string texto)
        {
        //@PAC|114335^ABADIA RIVERO^PEDRO JULIAN^50242914^3^28/02/2010^1^-^1
        //@PAR|20121445^RIVERO^JUANA^12/12/1980
        //@SOL|114335^39^RIVERO^ABADIA RIVERO^12:30^35^3521,00^False^True^10/03/2012^12:00^True^0^False^False^False^False^False^^^13
        //@ALA|La extracción se realizó antes de las 36 horas de vida.^El RN tuvo ingesta de leche en las primeras 24 horas@PAC|114335^ABADIA RIVERO^PEDRO JULIAN^50242914^3^28/02/2010^1^-^1@PAR|20121445^RIVERO^JUANA^12/12/1980@SOL|114335^59^RIVERO^ABADIA RIVERO^12:00^30^2513,00^False^False^01/03/2012^12:00^False^1^False^False^False^False^False^cualquier antecedente de prueba.cualquier antecedente de prueba.cualquier antecedente de prueba.cualquier antecedente de prueba.cualquier antecedente de prueba.cualquier antecedente de prueba.^^14@ALA|La extracción se realizó antes de las 36 horas de vida.^Deberá repetirse la muestra a los 15 días: La edad gestacional es menor a 35 semanas.

            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString())); ;


            string[] arr = texto.Split(("@").ToCharArray());
            int idPaciente = 0; int idSolicitud = 0;
            int cantidadPacientes = 0;
            if (arr.Length > 0)
            {
                for (int j=0; j<arr.Length; j++)
                {
                string s_tipoSegmento = arr[j].ToString();
                string[] arrSegmento = s_tipoSegmento.Split(("|").ToCharArray());
             
                for (int i = 0; i < arrSegmento.Length; i++)
                {
                    switch (arrSegmento[i].Trim())
                    {
                        case "PAC":
                            idPaciente = 0;
                            Efector oEfector = new Efector();
                            cantidadPacientes += 1;  
                            string s_DatosPaciente = arrSegmento[1].ToString();
                            string[] arrDatosPaciente = s_DatosPaciente.Split(("^").ToCharArray());

                            Paciente oPaciente = new Paciente();
                            
                            oPaciente.Apellido=arrDatosPaciente[1].ToString();
                            oPaciente.Nombre=arrDatosPaciente[2].ToString();

                            int numeroDocumento = int.Parse(arrDatosPaciente[3].ToString());
                            int idestado = int.Parse(arrDatosPaciente[6].ToString());
                            idPaciente = existe(numeroDocumento,idestado);
                            if (idPaciente==0)
                            {
                                oPaciente.NumeroDocumento = numeroDocumento;
                                oPaciente.IdSexo = int.Parse(arrDatosPaciente[4].ToString());
                                oPaciente.FechaNacimiento = DateTime.Parse(arrDatosPaciente[5].ToString());
                                oPaciente.IdEstado = int.Parse(arrDatosPaciente[6].ToString());
                                oPaciente.InformacionContacto = arrDatosPaciente[7].ToString();
                                oPaciente.IdEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(arrDatosPaciente[8].ToString()));


                                oPaciente.FechaAlta = DateTime.Now;
                                oPaciente.FechaDefuncion = Convert.ToDateTime("01/01/1900");
                                oPaciente.FechaUltimaActualizacion = DateTime.Now;

                                oPaciente.HistoriaClinica = 0;
                                oPaciente.IdPais = 0;
                                oPaciente.IdProvincia = 0;
                                oPaciente.IdNivelInstruccion = 0;
                                oPaciente.IdSituacionLaboral = 0;
                                oPaciente.IdProfesion = 0;
                                oPaciente.IdOcupacion = 0;
                                oPaciente.IdLocalidad = 0;
                                oPaciente.IdBarrio = 0;
                                oPaciente.IdDepartamento = 0;
                                oPaciente.IdProvinciaDomicilio = 0;
                                oPaciente.IdObraSocial = 0;
                                oPaciente.IdUsuario = 2;
                                oPaciente.IdEstadoCivil = 0;

                                oPaciente.Save();
                                idPaciente = oPaciente.IdPaciente;
                            }
                            break;
                        case "PAR":
                            if (idPaciente != 0)
                            {
                                string s_DatosParentesco = arrSegmento[1].ToString();
                                string[] arrDatosParentesco = s_DatosParentesco.Split(("^").ToCharArray());

                                if (arrDatosParentesco.Length>2)
                                {                                                               
                                    Paciente oPac = new Paciente();                                    
                                    oPac = (Paciente)oPac.Get(typeof(Paciente), idPaciente);

                                    Parentesco par = new Parentesco();
                                    par.IdPaciente = oPac;
                                    par.NumeroDocumento = Convert.ToInt32(arrDatosParentesco[0].ToString());
                                    par.Apellido = arrDatosParentesco[1].ToString();
                                    par.Nombre = arrDatosParentesco[2].ToString().ToUpper();
                                    par.IdTipoDocumento = 1;
                                    par.FechaNacimiento = Convert.ToDateTime(arrDatosParentesco[3].ToString());
                                    par.IdPais = Convert.ToInt32(1);
                                    par.IdProvincia = 0;
                                    par.IdNivelInstruccion = 0;
                                    par.IdSituacionLaboral = 0;
                                    par.IdProfesion = 0;
                                    par.IdUsuario = oUser;                                    
                                    par.FechaModificacion = DateTime.Now;
                                    par.Save();
                                }
                            }
                            break;
                        case "SOL":
                            if (idPaciente != 0)
                            {
                                idSolicitud = 0;
                                string s_DatosSolicitud = arrSegmento[1].ToString();
                                string[] arrDatosSolicitud = s_DatosSolicitud.Split(("^").ToCharArray());
                                int numeroOrigen=int.Parse(arrDatosSolicitud[20].ToString());
                                if (!existe(numeroOrigen))
                                {
                                    SolicitudScreening oSolicitud = new SolicitudScreening();
                                    Efector oEfectorSolicitante = new Efector();
                                    Paciente oPac = new Paciente();
                                    oPac = (Paciente)oPac.Get(typeof(Paciente), idPaciente);
                                    oSolicitud.IdPaciente = oPac;

                                    oSolicitud.IdEfector = oUser.IdEfector;
                                    oSolicitud.IdEfectorSolicitante = (Efector)oEfectorSolicitante.Get(typeof(Efector), int.Parse(arrDatosSolicitud[1].ToString()));
                                    oSolicitud.ApellidoMaterno = arrDatosSolicitud[2].ToString();
                                    oSolicitud.ApellidoPaterno = arrDatosSolicitud[3].ToString();
                                    oSolicitud.HoraNacimiento = arrDatosSolicitud[4].ToString();
                                    oSolicitud.EdadGestacional = int.Parse(arrDatosSolicitud[5].ToString());
                                    oSolicitud.Peso = decimal.Parse(arrDatosSolicitud[6].ToString());

                                    if (arrDatosSolicitud[7].ToString() == "False") oSolicitud.Prematuro = false;
                                    else oSolicitud.Prematuro = true;

                                    if (arrDatosSolicitud[8].ToString() == "False") oSolicitud.PrimeraMuestra = false;
                                    else oSolicitud.PrimeraMuestra = true;

                                    oSolicitud.FechaExtraccion = DateTime.Parse(arrDatosSolicitud[9].ToString());
                                    oSolicitud.HoraExtraccion = arrDatosSolicitud[10].ToString();

                                    if (arrDatosSolicitud[11].ToString() == "False") oSolicitud.IngestaLeche24Horas = false;
                                    else oSolicitud.IngestaLeche24Horas = true;


                                    oSolicitud.TipoAlimentacion = int.Parse(arrDatosSolicitud[12].ToString());

                                    if (arrDatosSolicitud[13].ToString() == "False") oSolicitud.Antibiotico = false;
                                    else oSolicitud.Antibiotico = true;

                                    if (arrDatosSolicitud[14].ToString() == "False") oSolicitud.Transfusion = false;
                                    else oSolicitud.Transfusion = true;

                                    if (arrDatosSolicitud[15].ToString() == "False") oSolicitud.Corticoides = false;
                                    else oSolicitud.Corticoides = true;

                                    if (arrDatosSolicitud[16].ToString() == "False") oSolicitud.Dopamina = false;
                                    else oSolicitud.Dopamina = true;

                                    if (arrDatosSolicitud[17].ToString() == "False") oSolicitud.EnfermedadTiroideaMaterna = false;
                                    else oSolicitud.EnfermedadTiroideaMaterna = true;

                                    oSolicitud.AntecedentesMaternos = arrDatosSolicitud[18].ToString();

                                    oSolicitud.Observaciones = arrDatosSolicitud[19].ToString();
                                    oSolicitud.NumeroOrigen = int.Parse(arrDatosSolicitud[20].ToString());

                                    oSolicitud.FechaRegistro = DateTime.Parse(DateTime.Now.ToShortDateString());
                                    oSolicitud.IdUsuarioRegistro = oUser;
                                    oSolicitud.Save();
                                    idSolicitud = oSolicitud.IdSolicitudScreening;
                                }                    
                            }
                            break;
                      
                        case "ALA":
                            if (idSolicitud != 0)
                            {
                                string s_DatosAlarmas = arrSegmento[1].ToString();
                                string[] arrDatosAlarmas = s_DatosAlarmas.Split(("^").ToCharArray());

                                for (int k = 0; k < arrDatosAlarmas.Length; k++)
                                {                                    
                                    string s_descripcionAlarma = arrDatosAlarmas[k].ToString();
                                    if (s_descripcionAlarma.Trim()!="")
                                    {
                                        SolicitudScreening oSolicitud = new SolicitudScreening();
                                        oSolicitud = (SolicitudScreening)oSolicitud.Get(typeof(SolicitudScreening), idSolicitud); 

                                        AlarmaScreening oRegistro = new AlarmaScreening();
                                        oRegistro.IdSolicitudScreening = oSolicitud;
                                        oRegistro.IdEfector = oSolicitud.IdEfector;
                                        oRegistro.Descripcion = s_descripcionAlarma;
                                        oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                                        oRegistro.FechaRegistro = DateTime.Now;
                                        oRegistro.Save();
                                    }
                                }                               
                            }
                            break;                      
                    }
                }

               
            }
            }

            return cantidadPacientes;
        }

        private bool existe(int numeroOrigen)
        {            


            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(SolicitudScreening));
            crit.Add(Expression.Eq("NumeroOrigen",numeroOrigen));            
            IList detalle = crit.List();
            
                foreach (SolicitudScreening oDetalle in detalle)
                {
                    return true;
                }

            
            return false;
            
        }

        private int existe(int numeroDocumento, int idestado)
        {
            int idPaciente = 0;
        
          
            ISession m_session = NHibernateHttpModule.CurrentSession;


            ICriteria crit = m_session.CreateCriteria(typeof(Paciente));

            crit.Add(Expression.Eq("IdEstado", idestado));
            crit.Add(Expression.Eq("NumeroDocumento", numeroDocumento));          

            IList detalle = crit.List();
           
                foreach (Paciente oDetalle in detalle)
                {
                    idPaciente = oDetalle.IdPaciente; break;
                }

            
            return idPaciente;
            
        }
    }
}
