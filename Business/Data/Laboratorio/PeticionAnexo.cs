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
    public sealed class PeticionAnexo : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idpeticionanexo; 
		private Peticion m_idpeticion; 
		
		private string m_url;
        
        private Usuario m_idusuarioregistro;
        private DateTime m_fecharegistro;
        
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public PeticionAnexo()
		{
			m_idpeticionanexo = 0; 
			m_idpeticion = new Peticion(); 
			 
            m_url= String.Empty;
         
            m_idusuarioregistro = new Usuario();
            m_fecharegistro = DateTime.MinValue;
            
        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public PeticionAnexo(
			Peticion idpeticion, 
		 
            string url
           )
			: this()
		{
            m_idpeticion = idpeticion;
		 
            m_url = url;
             
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdPeticionAnexo
		{
			get { return m_idpeticionanexo; }
			set
			{
				m_isChanged |= (m_idpeticionanexo != value );
                m_idpeticionanexo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public Peticion IdPeticion
		{
			get { return m_idpeticion; }
			set
			{
				m_isChanged |= (m_idpeticion != value );
                m_idpeticion = value;
			}

		}
			
		/// <summary>
		/// 
	


        public string Url
        {
            get { return m_url; }

            set
            {
                if (value != null && value.Length > 500)
                    throw new ArgumentOutOfRangeException("Invalid value for m_url", value, value.ToString());

                m_isChanged |= (m_url != value); m_url = value;
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
