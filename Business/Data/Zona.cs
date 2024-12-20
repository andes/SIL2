/*
insert license info here
*/
using System;
using System.Collections;

namespace Business.Data
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
    public sealed class Zona : Business.BaseDataAccess
    {

		#region Private Members
		private bool m_isChanged;

		private int m_idzona; 
		private string m_nombre; 
		private Localidad m_idlocalidad; 
		private string m_responsable; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Zona()
		{
			m_idzona = 0; 
			m_nombre = String.Empty; 
			m_idlocalidad = new Localidad(); 
			m_responsable = String.Empty; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public Zona(
			string nombre, 
			Localidad idlocalidad, 
			string responsable)
			: this()
		{
			m_nombre = nombre;
			m_idlocalidad = idlocalidad;
			m_responsable = responsable;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdZona
		{
			get { return m_idzona; }
			set
			{
				m_isChanged |= ( m_idzona != value ); 
				m_idzona = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Nombre
		{
			get { return m_nombre; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Nombre", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Nombre", value, value.ToString());
				
				m_isChanged |= (m_nombre != value); m_nombre = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public Localidad IdLocalidad
		{
			get { return m_idlocalidad; }
			set
			{
				m_isChanged |= ( m_idlocalidad != value ); 
				m_idlocalidad = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Responsable
		{
			get { return m_responsable; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Responsable", value, "null");
				
				if(  value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Responsable", value, value.ToString());
				
				m_isChanged |= (m_responsable != value); m_responsable = value;
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
