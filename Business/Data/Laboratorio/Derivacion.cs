/*
insert license info here
*/
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;

namespace Business.Data.Laboratorio
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
	public sealed class Derivacion: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idderivacion; 
		private DetalleProtocolo m_iddetalleprotocolo; 
		private DateTime m_fecharegistro; 
		private int m_idusuarioregistro; 
		private int m_estado; 
		private string m_observacion;
        private string m_resultado;
        private int m_idusuarioresultado;
        private DateTime m_fecharesultado;
        private Efector m_idEfector;
        private int idLote; //Cambio ya que al crear una Derivacion con idlote=0 despues NHibernate da error  'No row with the given identifier exists'
		private int idProtocoloDerivacion;
		private int idMotivoCancelacion;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Derivacion()
		{
			m_idderivacion = 0; 
			m_iddetalleprotocolo = new DetalleProtocolo(); 
			m_fecharegistro = DateTime.MinValue; 
			m_idusuarioregistro = 0; 
			m_estado = 0; 
			m_observacion = String.Empty;
            m_resultado = String.Empty;
            m_idusuarioresultado = 0;
            m_fecharesultado = DateTime.MinValue;
            m_idEfector = new Efector();
            idLote = 0;
            idProtocoloDerivacion = 0;
			idMotivoCancelacion = 0;

		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public Derivacion(
			DetalleProtocolo iddetalleprotocolo, 
			DateTime fecharegistro, 
			int idusuarioregistro, 
			int estado, 
			string observacion,
            string resultado,
            int idusuarioresultado, 
			DateTime fecharesultado,
            Efector idEfector)
			: this()
		{
			m_iddetalleprotocolo = iddetalleprotocolo;
			m_fecharegistro = fecharegistro;
			m_idusuarioregistro = idusuarioregistro;
			m_estado = estado;
			m_observacion = observacion;
            m_resultado = resultado;
            m_idusuarioresultado = idusuarioresultado;
            m_fecharesultado = fecharesultado;
            m_idEfector = idEfector;
            idLote = 0;
			idMotivoCancelacion = 0;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdDerivacion
		{
			get { return m_idderivacion; }
			set
			{
				m_isChanged |= ( m_idderivacion != value ); 
				m_idderivacion = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
        public DetalleProtocolo IdDetalleProtocolo
		{
			get { return m_iddetalleprotocolo; }
			set
			{
				m_isChanged |= ( m_iddetalleprotocolo != value ); 
				m_iddetalleprotocolo = value;
			}

		}

        public Efector IdEfectorDerivacion
        {
            get { return m_idEfector; }
            set
            {
                m_isChanged |= (m_idEfector != value);
                m_idEfector = value;
            }

        }

       

        /// <summary>
        /// 
        /// </summary>
        public DateTime FechaRegistro
		{
			get { return m_fecharegistro; }
			set
			{
				m_isChanged |= ( m_fecharegistro != value ); 
				m_fecharegistro = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdUsuarioRegistro
		{
			get { return m_idusuarioregistro; }
			set
			{
				m_isChanged |= ( m_idusuarioregistro != value ); 
				m_idusuarioregistro = value;
			}

		}
			
		/// <summary>
		/// 0: Pendiente - 1: Enviado - 2: No enviado
		/// </summary>
		public int Estado
		{
			get { return m_estado; }
			set
			{
				m_isChanged |= ( m_estado != value ); 
				m_estado = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Observacion
		{
			get { return m_observacion; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Motivo", value, "null");
				
				if(  value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Motivo", value, value.ToString());
				
				m_isChanged |= (m_observacion != value); m_observacion = value;
			}
		}

        public string Resultado
        {
            get { return m_resultado; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for m_resultado", value, "null");

                if (value.Length > 500)
                    throw new ArgumentOutOfRangeException("Invalid value for m_resultado", value, value.ToString());

                m_isChanged |= (m_resultado != value); m_resultado = value;
            }
        }
        public int IdUsuarioResultado
        {
            get { return m_idusuarioresultado; }
            set
            {
                m_isChanged |= (m_idusuarioresultado != value);
                m_idusuarioresultado = value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FechaResultado
        {
            get { return m_fecharesultado; }
            set
            {
                m_isChanged |= (m_fecharesultado != value);
                m_fecharesultado = value;
            }

        }
		/// <summary>
		/// Returns whether or not the object has changed it's values.
		/// </summary>
		public bool IsChanged
		{
			get { return m_isChanged; }
		}


        public int Idlote {
            get { return idLote; }
            set {
                m_isChanged |= (idLote != value);
                idLote = value;
            }
        }

        public int IdProtocoloDerivacion {
            get {
                return idProtocoloDerivacion;
            }
            set {
                idProtocoloDerivacion = value;
            }
        }

		public int IdMotivoCancelacion {
            get {
				return idMotivoCancelacion;
            }
            set {
				idMotivoCancelacion = value;
            }
        }
        #endregion

        public static List<Derivacion> DerivacionesByLote(int idLote) {
            List<Derivacion> derivaciones = new List<Derivacion>();
            try {
                ISession session = NHibernateHttpModule.CurrentSession;
                IList lista = session.CreateQuery("from Derivacion where idLote="+idLote).List();

                foreach (Derivacion item in lista) {
                    derivaciones.Add(item);
                }
            } catch (Exception) {

            }
            return derivaciones;
        }

       
    }
}
