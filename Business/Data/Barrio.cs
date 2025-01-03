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
    public sealed class Barrio : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idbarrio; 
		private string m_nombre; 
		private Localidad m_idlocalidad; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Barrio()
		{
			m_idbarrio = 0; 
			m_nombre = String.Empty; 
			m_idlocalidad = new Localidad(); 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public Barrio(
			string nombre, 
			Localidad idlocalidad)
			: this()
		{
			m_nombre = nombre;
			m_idlocalidad = idlocalidad;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdBarrio
		{
			get { return m_idbarrio; }
			set
			{
				m_isChanged |= ( m_idbarrio != value ); 
				m_idbarrio = value;
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
				
				if(  value.Length > 100)
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
		/// Returns whether or not the object has changed it's values.
		/// </summary>
		public bool IsChanged
		{
			get { return m_isChanged; }
		}
				
		#endregion 

		
			}
}
