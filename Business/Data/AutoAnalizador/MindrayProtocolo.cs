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
	public sealed class MindrayProtocolo: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idmindrayprotocolo; 
		private int m_iddetalleprotocolo; 
		private string m_numeroprotocolo; 
		private DateTime m_fechaprotocolo; 
		private string m_tipomuestra; 
		private int m_iditemmindray; 
		private string m_paciente; 
		private DateTime m_fechanacimiento; 
		private string m_sexo; 
		private string m_sectorsolicitante; 
		private bool m_urgente; 
		private bool m_estado; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public MindrayProtocolo()
		{
			m_idmindrayprotocolo = 0; 
			m_iddetalleprotocolo = 0; 
			m_numeroprotocolo = String.Empty; 
			m_fechaprotocolo = DateTime.MinValue; 
			m_tipomuestra = String.Empty; 
			m_iditemmindray = 0; 
			m_paciente = String.Empty; 
			m_fechanacimiento = DateTime.MinValue; 
			m_sexo = String.Empty; 
			m_sectorsolicitante = String.Empty; 
			m_urgente = false; 
			m_estado = false; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public MindrayProtocolo(
			int iddetalleprotocolo, 
			string numeroprotocolo, 
			DateTime fechaprotocolo, 
			string tipomuestra, 
			int iditemmindray, 
			string paciente, 
			DateTime fechanacimiento, 
			string sexo, 
			string sectorsolicitante, 
			bool urgente, 
			bool estado)
			: this()
		{
			m_iddetalleprotocolo = iddetalleprotocolo;
			m_numeroprotocolo = numeroprotocolo;
			m_fechaprotocolo = fechaprotocolo;
			m_tipomuestra = tipomuestra;
			m_iditemmindray = iditemmindray;
			m_paciente = paciente;
			m_fechanacimiento = fechanacimiento;
			m_sexo = sexo;
			m_sectorsolicitante = sectorsolicitante;
			m_urgente = urgente;
			m_estado = estado;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdMindrayProtocolo
		{
			get { return m_idmindrayprotocolo; }
			set
			{
				m_isChanged |= ( m_idmindrayprotocolo != value ); 
				m_idmindrayprotocolo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IddetalleProtocolo
		{
			get { return m_iddetalleprotocolo; }
			set
			{
				m_isChanged |= ( m_iddetalleprotocolo != value ); 
				m_iddetalleprotocolo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string NumeroProtocolo
		{
			get { return m_numeroprotocolo; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for NumeroProtocolo", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for NumeroProtocolo", value, value.ToString());
				
				m_isChanged |= (m_numeroprotocolo != value); m_numeroprotocolo = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaProtocolo
		{
			get { return m_fechaprotocolo; }
			set
			{
				m_isChanged |= ( m_fechaprotocolo != value ); 
				m_fechaprotocolo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string TipoMuestra
		{
			get { return m_tipomuestra; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for TipoMuestra", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for TipoMuestra", value, value.ToString());
				
				m_isChanged |= (m_tipomuestra != value); m_tipomuestra = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IditemMindray
		{
			get { return m_iditemmindray; }
			set
			{
				m_isChanged |= ( m_iditemmindray != value ); 
				m_iditemmindray = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Paciente
		{
			get { return m_paciente; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Paciente", value, "null");
				
				if(  value.Length > 200)
					throw new ArgumentOutOfRangeException("Invalid value for Paciente", value, value.ToString());
				
				m_isChanged |= (m_paciente != value); m_paciente = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public DateTime FechaNacimiento
		{
			get { return m_fechanacimiento; }
			set
			{
				m_isChanged |= ( m_fechanacimiento != value ); 
				m_fechanacimiento = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string Sexo
		{
			get { return m_sexo; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Sexo", value, "null");
				
				if(  value.Length > 1)
					throw new ArgumentOutOfRangeException("Invalid value for Sexo", value, value.ToString());
				
				m_isChanged |= (m_sexo != value); m_sexo = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public string SectorSolicitante
		{
			get { return m_sectorsolicitante; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for SectorSolicitante", value, "null");
				
				if(  value.Length > 150)
					throw new ArgumentOutOfRangeException("Invalid value for SectorSolicitante", value, value.ToString());
				
				m_isChanged |= (m_sectorsolicitante != value); m_sectorsolicitante = value;
			}
		}
			
		/// <summary>
		/// 
		/// </summary>
		public bool Urgente
		{
			get { return m_urgente; }
			set
			{
				m_isChanged |= ( m_urgente != value ); 
				m_urgente = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public bool Estado
		{
			get { return m_estado; }
			set
			{
				m_isChanged |= ( m_estado != value ); 
				m_estado = value;
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
