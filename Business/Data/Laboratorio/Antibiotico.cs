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
	public sealed class Antibiotico: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idantibiotico; 
		private Efector m_idefector; 
		private string m_nombrecorto; 
		private string m_descripcion; 
		private Usuario m_idusuarioregistro; 
		private DateTime m_fecharegistro; 
		private bool m_baja; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Antibiotico()
		{
			m_idantibiotico = 0; 
			m_idefector = new Efector(); 
			m_nombrecorto = String.Empty; 
			m_descripcion = String.Empty; 
			m_idusuarioregistro = new Usuario(); 
			m_fecharegistro = DateTime.MinValue; 
			m_baja = false; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public Antibiotico(
			Efector idefector, 
			string nombrecorto, 
			string descripcion, 
			Usuario idusuarioregistro, 
			DateTime fecharegistro, 
			bool baja)
			: this()
		{
			m_idefector = idefector;
			m_nombrecorto = nombrecorto;
			m_descripcion = descripcion;
			m_idusuarioregistro = idusuarioregistro;
			m_fecharegistro = fecharegistro;
			m_baja = baja;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdAntibiotico
		{
			get { return m_idantibiotico; }
			set
			{
				m_isChanged |= ( m_idantibiotico != value ); 
				m_idantibiotico = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public Efector IdEfector
		{
			get { return m_idefector; }
			set
			{
				m_isChanged |= ( m_idefector != value ); 
				m_idefector = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string NombreCorto
		{
			get { return m_nombrecorto; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for NombreCorto", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for NombreCorto", value, value.ToString());
				
				m_isChanged |= (m_nombrecorto != value); m_nombrecorto = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Descripcion
		{
			get { return m_descripcion; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Descripcion", value, "null");
				
				if(  value.Length > 250)
					throw new ArgumentOutOfRangeException("Invalid value for Descripcion", value, value.ToString());
				
				m_isChanged |= (m_descripcion != value); m_descripcion = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public Usuario IdUsuarioRegistro
		{
			get { return m_idusuarioregistro; }
			set
			{
				m_isChanged |= ( m_idusuarioregistro != value ); 
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
				m_isChanged |= ( m_fecharegistro != value ); 
				m_fecharegistro = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public bool Baja
		{
			get { return m_baja; }
			set
			{
				m_isChanged |= ( m_baja != value ); 
				m_baja = value;
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
