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
	public sealed class FusionItem: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idfusionitem; 
		private string m_idfusion; 
		private int m_iditem; 
		private bool m_habilitado; 		
		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public FusionItem()
		{
			m_idfusionitem = 0; 
			m_idfusion = String.Empty; 
			m_iditem = 0; 
			m_habilitado = false; 
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public FusionItem(
			string idfusion, 
			int iditem, 
			bool habilitado)
			: this()
		{
			m_idfusion = idfusion;
			m_iditem = iditem;
			m_habilitado = habilitado;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdFusionItem
		{
			get { return m_idfusionitem; }
			set
			{
				m_isChanged |= ( m_idfusionitem != value ); 
				m_idfusionitem = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
		public string IdFusion
		{
			get { return m_idfusion; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for IdCobas", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for IdCobas", value, value.ToString());
				
				m_isChanged |= (m_idfusion != value); m_idfusion = value;
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