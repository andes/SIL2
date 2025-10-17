using System; 
using System.Collections.Generic; 
using System.Text; 


namespace Business.Data.Laboratorio  {
    [Serializable]
    public sealed class LoteDerivacion : Business.BaseDataAccess
    {

        private int idLoteDerivacion;
        private Efector idEfectorOrigen;
        private Efector idEfectorDestino;
        private int estado;
        private DateTime fechaRegistro;
        private DateTime fechaEnvio;
        private DateTime fechaingreso;
        private bool baja;
        private int idUsuarioRegistro;
        private int idusuarioreceptor;
        private int idusuarioEnvio;

        private string observacion;


        public LoteDerivacion() {
            idLoteDerivacion = 0;
            idEfectorOrigen = new Efector();
            idEfectorDestino = new Efector();
            estado = 0;
            fechaRegistro = DateTime.Now;
            fechaEnvio = DateTime.Parse("01/01/1900"); //DateTime.MinValue;
            fechaingreso = DateTime.Parse("01/01/1900");
            baja = false;
            idUsuarioRegistro = 0;
            idusuarioreceptor = 0;
            idusuarioEnvio = 0;
            observacion = string.Empty;

        }

        #region metodos publicos
        public int IdLoteDerivacion {
            get { return this.idLoteDerivacion; }
            set { this.idLoteDerivacion = value; }
        }
        public Efector IdEfectorOrigen { 
            get { return idEfectorOrigen; }
            set { idEfectorOrigen = value; } 
        }
        public Efector IdEfectorDestino {
            get { return idEfectorDestino; }
            set { idEfectorDestino = value; }
        }
        public int Estado {
            get { return estado; }
            set { estado = value; } 
        }
        public DateTime FechaRegistro {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }
        public DateTime FechaEnvio {
            get { return fechaEnvio; }
            set { fechaEnvio = value; }
        }
        public DateTime  FechaIngreso {
            get { return fechaingreso; }
            set { fechaingreso = value; } 
        }
        public bool Baja {
            get { return baja; }
            set { baja = value; }
        }
        public int IdUsuarioRegistro {
            get { return idUsuarioRegistro; }
            set { idUsuarioRegistro = value; } 
        }
        public int IdUsuarioRecepcion {
            get { return idusuarioreceptor; }
            set { idusuarioreceptor = value; }
        }
        public int IdUsuarioEnvio { 
            
            get { return idusuarioEnvio; }
            set { idusuarioEnvio = value; }
            }


        public string Observacion {
            get { return this.observacion;  }
            set { this.observacion = value; }
        }

        #endregion

        public static string derivacionPDF(int idLote)
        {
            string m_strSQL = @"SELECT  numero, convert(varchar(10), fecha,103) as fecha, dni, determinacion,  
             apellido + ' '+ nombre as paciente,  efectorderivacion,  fechaNacimiento as edad, unidadEdad, sexo,   
             solicitante as especialista, idLote , 
               CASE  WHEN(len(idLote) < 9)  
               THEN  '00000' + CONVERT(VARCHAR, idLote)  
             ELSE CONVERT(VARCHAR, idLote )  
              END as idLoteString ,  
              idTipoServicio 
             FROM vta_LAB_Derivaciones WHERE  idLote= "+ idLote + " ORDER BY efectorDerivacion,numero ";

            return m_strSQL;     
        }

        public void GrabarAuditoriaLoteDerivacion(string m_accion, int m_idusuario, string m_observacion="", string valorNuevo="") {
            AuditoriaLoteDerivacion oRegistro = new AuditoriaLoteDerivacion();
            oRegistro.Accion = m_accion;
            oRegistro.Analisis = m_observacion;
            oRegistro.Valor = valorNuevo;
            oRegistro.Fecha = DateTime.Now;
            oRegistro.Hora = DateTime.Now.ToLongTimeString();
            oRegistro.IdLote = this.IdLoteDerivacion;
            oRegistro.IdUsuario = m_idusuario;
            oRegistro.Save();
        }
        public void GrabarAuditoriaLoteDerivacion(string m_accion, int m_idusuario, string m_observacion , string valorNuevo , string valorAnterior) {
            AuditoriaLoteDerivacion oRegistro = new AuditoriaLoteDerivacion();
            oRegistro.Accion = m_accion;
            oRegistro.Analisis = m_observacion;
            oRegistro.Valor = valorNuevo;
            oRegistro.ValorAnterior = valorAnterior;
            oRegistro.Fecha = DateTime.Now;
            oRegistro.Hora = DateTime.Now.ToLongTimeString();
            oRegistro.IdLote = this.IdLoteDerivacion;
            oRegistro.IdUsuario = m_idusuario;
            oRegistro.Save();
        }

        public string descripcionEstadoLote() {
            List<LoteDerivacionEstado> estados = LoteDerivacionEstado.estados();
            LoteDerivacionEstado estado = estados.Find(x => x.IdEstado == this.estado);
            return estado.Nombre;
        }

        public bool HayDerivacionesPendientes() {
            List<Derivacion> dList = Derivacion.DerivacionesByLote(this.IdLoteDerivacion);
            dList = dList.FindAll(x => x.IdProtocoloDerivacion == 0 && x.Estado == 1);

            if (dList.Count > 0)
                return true;
            else
                return false;
            
        }

        
        //public bool IngresoProtocolo() {
        //    List<Derivacion> derivaciones = Derivacion.DerivacionesByLote(this.IdLoteDerivacion);
            
        //    derivaciones.co
        //}
    }
}
