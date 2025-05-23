/*
insert license info here
*/
using System;
using System.Collections;

namespace Business.Data.AutoAnalizador
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
	public sealed class MiuraItem: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idmiuraitem; 
		private string m_idmiura; 
		private int m_iditem;
        private string m_dilucion;
		private string m_prefijo; 
		private bool m_habilitado; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public MiuraItem()
		{
			m_idmiuraitem = 0; 
			m_idmiura = String.Empty; 
			m_iditem = 0;
            m_dilucion = String.Empty;
			m_prefijo = String.Empty; 
			m_habilitado = false; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public MiuraItem(
			string idmiura, 
			int iditem, 
            string dilucion,
			string prefijo, 
			bool habilitado)
			: this()
		{
			m_idmiura = idmiura;
			m_iditem = iditem;
            m_dilucion = dilucion;
			m_prefijo = prefijo;
			m_habilitado = habilitado;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdMiuraItem
		{
			get { return m_idmiuraitem; }
			set
			{
				m_isChanged |= ( m_idmiuraitem != value ); 
				m_idmiuraitem = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string IdMiura
		{
			get { return m_idmiura; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for IdMiura", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for IdMiura", value, value.ToString());
				
				m_isChanged |= (m_idmiura != value); m_idmiura = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdItem
		{
			get { return m_iditem; }
			set
			{
				m_isChanged |= ( m_iditem != value ); 
				m_iditem = value;
			}

		}
        /// <summary>
        /// 
        /// </summary>
        public string Dilucion
        {
            get { return m_dilucion; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for m_dilucion", value, "null");

                if (value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for m_dilucion", value, value.ToString());

                m_isChanged |= (m_dilucion != value); m_dilucion = value;
            }
        }	
		/// <summary>
		/// 
		/// </summary>
		public string Prefijo
		{
			get { return m_prefijo; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Prefijo", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Prefijo", value, value.ToString());
				
				m_isChanged |= (m_prefijo != value); m_prefijo = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public bool Habilitado
		{
			get { return m_habilitado; }
			set
			{
				m_isChanged |= ( m_habilitado != value ); 
				m_habilitado = value;
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
