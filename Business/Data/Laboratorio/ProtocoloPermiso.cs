/*
insert license info here
*/
using System;
using System.Collections;

namespace Business.Data.Laboratorio
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
    public sealed class ProtocoloPermiso : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idprotocolopermiso; 
		private Protocolo m_idprotocolo; 
		private int m_idperfil; 
		 
        private Usuario m_idusuarioregistro;
        private DateTime m_fecharegistro;
     
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public ProtocoloPermiso()
		{
            m_idprotocolopermiso = 0; 
			m_idprotocolo = new Protocolo();
            m_idperfil = 0;

            m_idusuarioregistro = new Usuario();
            m_fecharegistro = DateTime.MinValue;
            
        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public ProtocoloPermiso(
			Protocolo idprotocolo, 
			int idperfil
			
            )
			: this()
		{
			m_idprotocolo = idprotocolo;
			m_idperfil = idperfil;
			
          
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdProtocoloPermiso
		{
			get { return m_idprotocolopermiso; }
			set
			{
				m_isChanged |= (m_idprotocolopermiso != value );
                m_idprotocolopermiso = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public Protocolo IdProtocolo
		{
			get { return m_idprotocolo; }
			set
			{
				m_isChanged |= ( m_idprotocolo != value ); 
				m_idprotocolo = value;
			}

		}

        public int IdPerfil
        {
            get { return m_idperfil; }
            set
            {
                m_isChanged |= (m_idperfil != value);
                m_idperfil = value;
            }

        }
        public Usuario IdUsuarioRegistro
        {
            get { return m_idusuarioregistro; }
            set
            {
                m_isChanged |= (m_idusuarioregistro != value);
                m_idusuarioregistro = value;
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
                m_isChanged |= (m_fecharegistro != value);
                m_fecharegistro = value;
            }

        }

     
        /// <summary>
        /// Returns whether or not the object has changed it's values.
        /// </summary>
        public bool IsChanged
		{
			get { return m_isChanged; }
		}
				
		#endregion 
	}
}
