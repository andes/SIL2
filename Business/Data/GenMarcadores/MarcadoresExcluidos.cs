/*
insert license info here
*/
using System;
using System.Collections;

namespace Business.Data.GenMarcadores
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
    public sealed class MarcadoresExcluidos : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;
        private int m_idmarcadoresexcluidos;

        private int m_idprotocolo;
        private int m_idpaciente;
         
        
        private int m_idusuarioregistro;
        private DateTime m_fecharegistro;
    
        
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public MarcadoresExcluidos()
		{
            m_idmarcadoresexcluidos = 0;
            m_idprotocolo = 0;
            m_idpaciente = 0;

             

            m_idusuarioregistro = 0;
            m_fecharegistro = DateTime.MinValue;



        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public MarcadoresExcluidos(
			int idprotocolo,
                 int idpaciente,
           
                int idusuarioregistro,
            DateTime fecharegistro )
			: this()
		{
			m_idprotocolo = idprotocolo;
            m_idpaciente = idpaciente;
 
         
            m_idusuarioregistro = idusuarioregistro;
            m_fecharegistro = fecharegistro;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdMarcadoresExcluidos
		{
			get { return m_idmarcadoresexcluidos; }
			set
			{
				m_isChanged |= (m_idmarcadoresexcluidos != value );
                m_idmarcadoresexcluidos = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdProtocolo
		{
			get { return m_idprotocolo; }
			set
			{
				m_isChanged |= ( m_idprotocolo != value ); 
				m_idprotocolo = value;
			}

		}

        /// <summary>
        /// 
        /// </summary>
        public int IdPaciente
        {
            get { return m_idpaciente; }
            set
            {
                m_isChanged |= (m_idpaciente != value);
                m_idpaciente = value;
            }

        }


        public int IdUsuarioRegistro
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