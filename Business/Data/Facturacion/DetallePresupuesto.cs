/*
insert license info here
*/
using System;
using System.Collections;
using Business.Data.Laboratorio;

namespace Business.Data.Facturacion
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
	public sealed class DetallePresupuesto : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_iddetallepresupuesto; 
		private Presupuesto m_idpresupuesto;
        private int m_idnomencladorforense;

        private string m_descripcion;
        private int m_cantidad;
        private int m_cantidadprefacturado;
        private int m_idfactura;
        private bool m_prefacturado;
        private decimal m_precio;
        private decimal m_total;
        private decimal m_totalprefactura;
        private int m_idcasofiliacion;
        //private int m_ancho;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public DetallePresupuesto()
		{
            m_iddetallepresupuesto = 0;
            m_idpresupuesto = new Presupuesto();
            m_cantidadprefacturado = 0;
        m_idfactura = 0;
          m_prefacturado = false;
        m_descripcion = String.Empty;
            m_totalprefactura = 0;
            m_idcasofiliacion = 0;
            //m_ancho = 0;
        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public DetallePresupuesto(
			Presupuesto idpresupuesto,
            int  cantidadprefacturado,
            int  idcasofiliacion,
         int  idfactura,
        bool  prefacturado,
        string descripcion)
			: this()
		{
			m_idpresupuesto = idpresupuesto;
             m_cantidadprefacturado=cantidadprefacturado;
          m_idfactura=idfactura;
        m_prefacturado=prefacturado;
        m_descripcion = descripcion;
            m_idcasofiliacion = idcasofiliacion;
            //m_ancho = ancho;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdDetallePresupuesto
		{
			get { return m_iddetallepresupuesto; }
			set
			{
				m_isChanged |= (m_iddetallepresupuesto != value );
                m_iddetallepresupuesto = value;
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

        /// <summary>
        /// 
        /// </summary>
        public Presupuesto IdPresupuesto
        {
			get { return m_idpresupuesto; }
			set
			{
				m_isChanged |= (m_idpresupuesto != value );
                m_idpresupuesto = value;
			}

		}
			
		
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


        public int IdFactura
        {
            get { return m_idfactura; }
            set
            {
                m_isChanged |= (m_idfactura != value);
                m_idfactura = value;
            }

        }

        public int Cantidadprefacturado
        {
            get { return m_cantidadprefacturado; }
            set
            {
                m_isChanged |= (m_cantidadprefacturado != value);
                m_cantidadprefacturado = value;
            }

        }

        public bool Prefacturado
        {
            get { return m_prefacturado; }
            set
            {
                m_isChanged |= (m_prefacturado != value);
                m_prefacturado = value;
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
					throw new ArgumentOutOfRangeException("Null value not allowed for TextoImprimir", value, "null");
				
				if(  value.Length > 4000)
					throw new ArgumentOutOfRangeException("Invalid value for TextoImprimir", value, value.ToString());
				
				m_isChanged |= (m_descripcion != value); m_descripcion = value;
			}
		}


        public int Cantidad
        {
            get { return m_cantidad; }
            set
            {
                m_isChanged |= (m_cantidad != value);
                m_cantidad = value;
            }

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
        public decimal Total
        {
            get { return m_total; }
            set
            {
                m_isChanged |= (m_total != value);
                m_total = value;
            }

        }

        public decimal TotalPrefactura
        {
            get { return m_totalprefactura; }
            set
            {
                m_isChanged |= (m_totalprefactura != value);
                m_totalprefactura = value;
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