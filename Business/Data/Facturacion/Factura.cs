/*
insert license info here
*/
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Business.Data.Facturacion
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
    public sealed class Factura : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

        private int m_idfactura;
        private int m_idcasofiliacion;
        private string m_numero;
        private decimal m_total;
     
        private int m_idusuarioregistro;
        private DateTime m_fecharegistro;
      private bool m_baja;
        
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public Factura()
		{
            m_idcasofiliacion = 0;
            
            m_total = 0;
            m_numero = String.Empty;

           m_baja = false;
            m_idusuarioregistro = 0;
            m_fecharegistro = DateTime.MinValue;
          
        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public Factura(
           
            string numero,
            decimal total,
            int idcasofiliacion,
            
           bool baja,
             int idusuarioregistro,
            DateTime fecharegistro
            
             )
			: this()
		{
			m_numero = numero;
            m_total = total;
            m_idcasofiliacion = idcasofiliacion;
         m_baja = baja;
           
            m_idusuarioregistro = idusuarioregistro;
        m_fecharegistro = DateTime.MinValue;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdFactura
        {
			get { return m_idfactura; }
			set
			{
				m_isChanged |= (m_idfactura != value );
                m_idfactura = value;
			}

		}
        public int IdCasoFiliacion
        {
            get { return m_idcasofiliacion; }
            set
            {
                m_isChanged |= (m_idcasofiliacion != value);
                m_idcasofiliacion = value;
            }

        }
        public string Numero
        {
            get { return m_numero; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for Nombre", value, "null");

                if (value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Nombre", value, value.ToString());

                m_isChanged |= (m_numero != value); m_numero = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Total
        {
            get { return m_total; }
            set
            {
                m_isChanged |= (m_total != value);
                m_total = value;
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
                m_isChanged |= (m_baja != value);
                m_baja = value;
            }

        }
        public int IdUsuarioRegistro
        {
            get { return m_idusuarioregistro; }
            set
            {
                m_isChanged |= (m_idusuarioregistro != value);
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
                m_isChanged |= (m_fecharegistro != value);
                m_fecharegistro = value;
            }

        }
 
        public bool IsChanged
		{
			get { return m_isChanged; }
		}


       

      

      
     
        #endregion
    }
}
