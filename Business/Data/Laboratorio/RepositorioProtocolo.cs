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
	public sealed class RepositorioProtocolo : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idrepositorioprotocolo ; 
		private int m_idprotocolo; 
		private int m_idrepositorio; 
	 
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public RepositorioProtocolo()
		{
			m_idrepositorioprotocolo = 0; 
			m_idprotocolo = 0;
            m_idrepositorio = 0; 
			 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public RepositorioProtocolo(
			int idprotocolo, 
			int idrepositorio )
			: this()
		{
			m_idprotocolo = idprotocolo;
            m_idrepositorio = idrepositorio;
			
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdRepositorioProtocolo
        {
			get { return m_idrepositorioprotocolo; }
			set
			{
				m_isChanged |= (m_idrepositorioprotocolo != value );
                m_idrepositorioprotocolo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdProtocolo
		{
			get { return m_idprotocolo; }
			set
			{
				m_isChanged |= ( m_idprotocolo != value ); 
				m_idprotocolo = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public int IdRepositorio
		{
			get { return m_idrepositorio; }
			set
			{
				m_isChanged |= (m_idrepositorio != value );
                m_idrepositorio = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		//public int IdIncidenciaCalidad
		//{
		//	get { return m_idincidenciacalidad; }
		//	set
		//	{
		//		m_isChanged |= ( m_idincidenciacalidad != value ); 
		//		m_idincidenciacalidad = value;
		//	}

		//}
			
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
