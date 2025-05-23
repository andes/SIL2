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
	public sealed class Perfil: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idperfil; 
		
		private int m_idefector; 
		private string m_nombre; 
		private bool m_activo;
        private int m_idusuario; 
		private DateTime m_fechaactualizacion; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public Perfil()
		{
			m_idperfil = 0;

            m_idefector = 0; 
			m_nombre = String.Empty; 
			m_activo = false; 
			m_idusuario = 0; 
			m_fechaactualizacion = DateTime.MinValue; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public Perfil(
			int idefector, 
			string nombre, 
			bool activo, 
			int idusuario, 
			DateTime fechaactualizacion)
			: this()
		{
			m_idefector = idefector;
			m_nombre = nombre;
			m_activo = activo;
			m_idusuario = idusuario;
			m_fechaactualizacion = fechaactualizacion;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdPerfil
		{
			get { return m_idperfil; }
			set
			{
				m_isChanged |= ( m_idperfil != value ); 
				m_idperfil = value;
			}

		}
			
		


		/// <summary>
		/// 
		/// </summary>
		public int IdEfector
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
		public bool Activo
		{
			get { return m_activo; }
			set
			{
				m_isChanged |= ( m_activo != value ); 
				m_activo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdUsuario
		{
			get { return m_idusuario; }
			set
			{
				m_isChanged |= ( m_idusuario != value ); 
				m_idusuario = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaActualizacion
		{
			get { return m_fechaactualizacion; }
			set
			{
				m_isChanged |= ( m_fechaactualizacion != value ); 
				m_fechaactualizacion = value;
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
