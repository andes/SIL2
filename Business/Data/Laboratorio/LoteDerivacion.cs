using System; 
using System.Collections.Generic; 
using System.Text; 


namespace Business.Data.Laboratorio  {
    [Serializable]
    public sealed class LoteDerivacion : Business.BaseDataAccess
    {

        private int idLoteDerivacion;
        private int idEfectorOrigen;
        private int idEfectorDestino;
        private int estado;
        private string fechaRegistro;
        private string fechaEnvio;
        private string fechaingreso;
        private bool baja;
        private System.Nullable<int> idUsuarioRegistro;
        private System.Nullable<int> idusuarioreceptor;
        private System.Nullable<int> idusuarioEnvio;

        private string observacion;


        public LoteDerivacion() {
            idLoteDerivacion = 0;
            idEfectorOrigen = 0;
            idEfectorDestino = 0;
            estado = 0;
            fechaRegistro = DateTime.Now.ToString();
            fechaEnvio = new DateTime(1900,1,1).ToString();
            fechaingreso = new DateTime(1900,1,1).ToString();
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
        public int IdEfectorOrigen { 
            get { return idEfectorOrigen; }
            set { idEfectorOrigen = value; } 
        }
        public int IdEfectorDestino {
            get { return idEfectorDestino; }
            set { idEfectorDestino = value; }
        }
        public int Estado {
            get { return estado; }
            set { estado = value; } 
        }
        public string FechaRegistro {
            get { return fechaRegistro; }
            set { fechaRegistro = value; }
        }
        public string FechaEnvio {
            get { return fechaEnvio; }
            set { fechaEnvio = value; }
        }
        public string FechaIngreso {
            get { return fechaingreso; }
            set { fechaingreso = value; } 
        }
        public bool Baja {
            get { return baja; }
            set { baja = value; }
        }
        public System.Nullable<int> IdUsuarioRegistro {
            get { return idUsuarioRegistro; }
            set { idUsuarioRegistro = value; } 
        }
        public System.Nullable<int> IdUsuarioRecepcion {
            get { return idusuarioreceptor; }
            set { idusuarioreceptor = value; }
        }
        public System.Nullable<int>  IdUsuarioEnvio { 
            get => idusuarioEnvio; 
            set => idusuarioEnvio = value;
        }


        public string Observacion {
            get { return this.observacion;  }
            set { this.observacion = value; }
        }

        #endregion

        public static string derivacionPDF(int idLote)
        {
            string m_strSQL = " SELECT  numero, convert(varchar(10), fecha,103) as fecha, dni, determinacion, " +
            " apellido + ' '+ nombre as paciente,  efectorderivacion,  fechaNacimiento as edad, unidadEdad, sexo,  " +
            " solicitante as especialista, idLote ," +
            "   CASE  WHEN(len(idLote) < 9) " +
            "   THEN  '00000' + CONVERT(VARCHAR, idLote) " +
            " ELSE CONVERT(VARCHAR, idLote ) " +
            "  END as idLoteString " +
            " FROM vta_LAB_Derivaciones WHERE  idLote=" + idLote + " ORDER BY efectorDerivacion,numero ";

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
    }
}
