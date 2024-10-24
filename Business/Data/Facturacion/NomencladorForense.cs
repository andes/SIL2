/*
insert license info here
*/
using System;
using System.Collections;
using NHibernate;
using NHibernate.Expression;
using MathParser;

namespace Business.Data.Facturacion
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
    public sealed class NomencladorForense : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idnomencladorforense; 
	 
		private string m_codigo; 
		private string m_nombre; 
	 
        private decimal m_precio;
       
		private bool m_baja; 
		private Usuario m_idusuarioregistro; 
		private DateTime m_fecharegistro;
       

		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public NomencladorForense()
		{ 
			 
			m_codigo = String.Empty; 
			m_nombre = String.Empty; 
		 
            m_precio =0;
           

			m_baja = false; 
			m_idusuarioregistro = new Usuario(); 
			m_fecharegistro = DateTime.MinValue;
           
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public NomencladorForense(
 
			string codigo, 
			string nombre, 
		 
            decimal precio,
           
			bool baja, 
			Usuario idusuarioregistro, 
			DateTime fecharegistro)
			: this()
		{
			 
			m_codigo = codigo;
			m_nombre = nombre;
			m_precio = precio;
		 
			m_baja = baja;
			m_idusuarioregistro = idusuarioregistro;
			m_fecharegistro = fecharegistro;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdNomencladorForense
        {
			get { return m_idnomencladorforense; }
			set
			{
				m_isChanged |= (m_idnomencladorforense != value );
                m_idnomencladorforense = value;
			}

		}
			
		/// <summary>
		/// 
		/// </summary>
	 
			
		/// <summary>
		/// 
		/// </summary>
		public string Codigo
		{
			get { return m_codigo; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for Codigo", value, "null");
				
				if(  value.Length > 50)
					throw new ArgumentOutOfRangeException("Invalid value for Codigo", value, value.ToString());
				
				m_isChanged |= (m_codigo != value); m_codigo = value;
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
				
				if(  value.Length > 500)
					throw new ArgumentOutOfRangeException("Invalid value for Nombre", value, value.ToString());
				
				m_isChanged |= (m_nombre != value); m_nombre = value;
			}
		}

        public bool noexistecodigo(string cod)
        {
            NomencladorForense oRegistro = new NomencladorForense();
            oRegistro = (NomencladorForense)oRegistro.Get(typeof(NomencladorForense),"Codigo",cod);
            if (oRegistro != null)
                return true;
            else
                 return false;


        }

        public decimal Precio
        {
            get { return m_precio; }
            set
            {
                m_isChanged |= (m_precio != value);
                m_precio = value;
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

        #region Public Metodo

 

   

       

        #endregion


       
    }
}
