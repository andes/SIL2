/*
insert license info here
*/
using System;
using System.Collections;
using NHibernate;
using NHibernate.Expression;
using MathParser;

namespace Business.Data.Laboratorio
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
    public sealed class ItemPresentacion : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;
        private int m_iditemPresentacion;
       
        private Item m_iditem;
        private int m_idMarca;
        private string m_codigo;
        private string m_Presentacion; 
		 
        private bool m_baja;
        private Usuario m_idusuarioregistro;
        private DateTime m_fecharegistro;
        

        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public ItemPresentacion()
		{
            m_iditemPresentacion = 0;
            m_iditem = new Item();
            m_idMarca = 0;
            m_codigo = "";
            m_Presentacion = "";
            m_baja = false;
            

            //m_isscreening = false;
        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public ItemPresentacion(
            Item iditem,
			 int idmarca,
             string codigo,
             string presentacion,
			Usuario idusuarioregistro, 
			DateTime fecharegistro,
            bool baja
            )
			: this()
		{
            m_iditem  = iditem;
			m_idMarca = idmarca;
            m_codigo = codigo;
			m_Presentacion = presentacion;
		 	 
			m_idusuarioregistro = idusuarioregistro;
			m_fecharegistro = fecharegistro;
            m_baja = baja;
        }
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdItemPresentacion
		{
			get { return m_iditemPresentacion; }
			set
			{
				m_isChanged |= (m_iditemPresentacion != value );
                m_iditemPresentacion = value;
			}

		}

        public Item IdItem 
        {
            get { return m_iditem; }
            set
            {
                m_isChanged |= (m_iditem != value);
                m_iditem = value;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public int IdMarca
		{
			get { return m_idMarca; }
			set
			{
				m_isChanged |= (m_idMarca != value );
                m_idMarca = value;
			}

		}

        /// <summary>
        /// 
        /// </summary>


        public string Codigo
        {
            get { return m_codigo; }
            set
            {
                m_isChanged |= (m_codigo != value);
                m_codigo = value;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public string Presentacion
		{
			get { return m_Presentacion; }
			set
			{
				m_isChanged |= (m_Presentacion != value );
                m_Presentacion = value;
			}

		}


        public bool Baja
        {
            get { return m_baja; }
            set
            {
                m_isChanged |= (m_baja != value);
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
        /// 
        /// </summary>
       

		/// <summary>
		/// Returns whether or not the object has changed it's values.
		/// </summary>
		public bool IsChanged
		{
			get { return m_isChanged; }
		}

        public bool tieneDatosVinculados()
        {
            ICriteria crit2 = m_session.CreateCriteria(typeof(ValorReferencia));                        
            crit2.Add(Expression.Eq("IdPresentacion", this.IdItemPresentacion));            
            IList items2 = crit2.List();
            if (items2.Count > 0)
                return true;
            else
            {
                ICriteria crit3 = m_session.CreateCriteria(typeof(ValorReferenciaNoPac));
                crit3.Add(Expression.Eq("IdPresentacion", this.IdItemPresentacion));
                IList items3 = crit3.List();
                if (items3.Count > 0)
                    return true;
                else
                return false;

            }
        }

        #endregion

        #region Public Metodo





        #endregion



    }
}
