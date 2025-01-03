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
	public sealed class AuditoriaUsuario : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

        private int m_idauditoriausuario; 
       
        private int m_idusuario; 
		private DateTime m_fecha; 
		private string m_hora; 
		private string m_accion; 
		private string m_username; 
		 
		private int m_idusuarioregistro; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public AuditoriaUsuario()
		{
            m_idauditoriausuario = 0;
            m_idusuario = 0; 
			m_fecha = DateTime.MinValue; 
			m_hora = String.Empty; 
			m_accion = String.Empty;
            m_username = String.Empty;
            m_idusuarioregistro = 0; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public AuditoriaUsuario(
			int idusuario, 
			DateTime fecha, 
			string hora, 
			string accion, 
			string username, 
			 
			int idusuarioregistro)
			: this()
		{
            m_idusuario = idusuario;
			m_fecha = fecha;
			m_hora = hora;
			m_accion = accion;
			m_username = username;

            m_idusuarioregistro = idusuarioregistro;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
        public int IdAuditoriaUsuario
		{
            get { return m_idauditoriausuario; }
			set
			{
                m_isChanged |= (m_idauditoriausuario != value);
                m_idauditoriausuario = value;
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
				m_isChanged |= (m_idusuario != value );
                m_idusuario = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public DateTime Fecha
		{
			get { return m_fecha; }
			set
			{
				m_isChanged |= ( m_fecha != value ); 
				m_fecha = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Hora
		{
			get { return m_hora; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Hora", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Hora", value, value.ToString());
				
				m_isChanged |= (m_hora != value); m_hora = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Accion
		{
			get { return m_accion; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Accion", value, "null");
				
				if(  value.Length > 400)
					throw new ArgumentOutOfRangeException("Invalid value for Accion", value, value.ToString());
				
				m_isChanged |= (m_accion != value); m_accion = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Username
		{
			get { return m_username; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for m_username", value, "null");
				
				if(  value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for m_username", value, value.ToString());
				
				m_isChanged |= (m_username != value); m_username = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
	 
		/// <summary>
		/// 
		/// </summary>
		public int IdUsuarioRegistro
		{
			get { return m_idusuarioregistro; }
			set
			{
				m_isChanged |= (m_idusuarioregistro != value );
                m_idusuarioregistro = value;
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
