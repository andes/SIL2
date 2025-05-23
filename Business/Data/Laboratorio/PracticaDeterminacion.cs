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
    public sealed class PracticaDeterminacion : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idpracticadeterminacion; 
		private Efector m_idefector; 
		private Item m_iditempractica; 
		private int m_iditemdeterminacion; 
		private string m_titulo; 
		private int m_orden; 
		private string m_formatoimpresion; 
		private Usuario m_idusuarioregistro; 
		private DateTime m_fecharegistro; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public PracticaDeterminacion()
		{
			m_idpracticadeterminacion = 0; 
			m_idefector = new Efector(); 
			m_iditempractica = new Item(); 
			m_iditemdeterminacion = 0; 
			m_titulo = String.Empty; 
			m_orden = 0; 
			m_formatoimpresion = String.Empty; 
			m_idusuarioregistro = new Usuario(); 
			m_fecharegistro = DateTime.MinValue; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public PracticaDeterminacion(
			Efector idefector, 
			Item iditempractica, 
			int iditemdeterminacion, 
			int orden, 
			Usuario idusuarioregistro, 
			DateTime fecharegistro)
			: this()
		{
			m_idefector = idefector;
			m_iditempractica = iditempractica;
			m_iditemdeterminacion = iditemdeterminacion;
			m_titulo = String.Empty;
			m_orden = orden;
			m_formatoimpresion = String.Empty;
			m_idusuarioregistro = idusuarioregistro;
			m_fecharegistro = fecharegistro;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdPracticaDeterminacion
		{
			get { return m_idpracticadeterminacion; }
			set
			{
				m_isChanged |= ( m_idpracticadeterminacion != value ); 
				m_idpracticadeterminacion = value;
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
		public Item IdItemPractica
		{
			get { return m_iditempractica; }
			set
			{
				m_isChanged |= ( m_iditempractica != value ); 
				m_iditempractica = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdItemDeterminacion
		{
			get { return m_iditemdeterminacion; }
			set
			{
				m_isChanged |= ( m_iditemdeterminacion != value ); 
				m_iditemdeterminacion = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Titulo
		{
			get { return m_titulo; }

			set	
			{	
				if(  value != null &&  value.Length > 100)
					throw new ArgumentOutOfRangeException("Invalid value for Titulo", value, value.ToString());
				
				m_isChanged |= (m_titulo != value); m_titulo = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public int Orden
		{
			get { return m_orden; }
			set
			{
				m_isChanged |= ( m_orden != value ); 
				m_orden = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string FormatoImpresion
		{
			get { return m_formatoimpresion; }

			set	
			{	
				if(  value != null &&  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for FormatoImpresion", value, value.ToString());
				
				m_isChanged |= (m_formatoimpresion != value); m_formatoimpresion = value;
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
		/// Returns whether or not the object has changed it's values.
		/// </summary>
		public bool IsChanged
		{
			get { return m_isChanged; }
		}
				
		#endregion 
	}
}
