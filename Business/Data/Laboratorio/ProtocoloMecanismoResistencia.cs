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
    public sealed class ProtocoloMecanismoResistencia : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idprotocolomecanismoresistencia; 
		private ProtocoloGermen m_idprotocologermen; 
		private MecanismoResistencia m_idmecanismoresistencia; 
		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public ProtocoloMecanismoResistencia()
		{
			m_idprotocolomecanismoresistencia = 0; 
			m_idprotocologermen = new ProtocoloGermen(); 
			m_idmecanismoresistencia = new MecanismoResistencia(); 
			
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
        public ProtocoloMecanismoResistencia(
			ProtocoloGermen idprotocologermen, 
			MecanismoResistencia idmecanismoresistencia 
			)
			: this()
		{
			m_idprotocologermen = idprotocologermen;
			m_idmecanismoresistencia = idmecanismoresistencia;
			
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdProtocoloMecanismoResistencia
		{
			get { return m_idprotocolomecanismoresistencia; }
			set
			{
                m_isChanged |= (m_idprotocolomecanismoresistencia != value);
                m_idprotocolomecanismoresistencia = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public ProtocoloGermen IdProtocoloGermen
		{
			get { return m_idprotocologermen; }
			set
			{
                m_isChanged |= (m_idprotocologermen != value);
                m_idprotocologermen = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public MecanismoResistencia IdMecanismoResistencia
		{
			get { return m_idmecanismoresistencia ; }
			set
			{
                m_isChanged |= (m_idmecanismoresistencia != value);
                m_idmecanismoresistencia = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		
			
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