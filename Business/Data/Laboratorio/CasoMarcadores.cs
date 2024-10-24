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
    public sealed class CasoMarcadores : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idcasomarcadores;
        private int m_orden;
        private int m_idprotocolo; 
		private int m_idcasofiliacion;
         
        private string m_marcador;
        private string m_allello1;
        private string m_allello2;
        private string m_allello3;
        private string m_allello4;
        private string m_allello5;
        private string m_allello6;
        private string m_allello7;
        private string m_allello8;
        private string m_allello9;
        private string m_allello10;
        private string m_ip;
        private string m_subitem;



        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public CasoMarcadores()
		{
            m_idcasomarcadores = 0;
            m_idprotocolo = 0;
            m_idcasofiliacion = 0;
            m_orden = 0;
              m_marcador = String.Empty;
            m_allello1 = String.Empty;
            m_allello2 = String.Empty;
            m_allello3 = String.Empty;
            m_allello4 = String.Empty;
            m_allello5 = String.Empty;
            m_allello6 = String.Empty;
            m_allello7 = String.Empty;
            m_allello8 = String.Empty;
            m_allello9 = String.Empty;
            m_allello10 = String.Empty;
            m_ip = String.Empty;
            m_subitem = String.Empty;




        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public CasoMarcadores(
			int idprotocolo,
            int idcasofiliacion, 
            int orden,
            string marcador,
            string allello1,
            string allello2,
            string allello3,
            string allello4,
            string allello5,
            string allello6,
            string allello7,
            string allello8,
            string allello9,
            string allello10,
            string ip,
            string subitem)
			: this()
		{
			m_idprotocolo = idprotocolo;
			m_idcasofiliacion = idcasofiliacion;
            m_orden = orden;

            m_marcador = marcador;
            m_allello1 = allello1;
            m_allello2 = allello2;
            m_allello3 = allello3;
            m_allello4 = allello4;
            m_allello5 = allello5;
            m_allello6 = allello6;
            m_allello7 = allello7;
            m_allello8 = allello8;
            m_allello9 = allello9;
            m_allello10 = allello10;
            m_ip = ip;
            m_subitem = subitem;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdCasoMarcadores
		{
			get { return m_idcasomarcadores; }
			set
			{
				m_isChanged |= (m_idcasomarcadores != value );
                m_idcasomarcadores = value;
			}

		}

        public int Orden
        {
            get { return m_orden; }
            set
            {
                m_isChanged |= (m_orden != value);
                m_orden = value;
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
		public int IdCasoFiliacion
        {
			get { return m_idcasofiliacion; }
			set
			{
				m_isChanged |= (m_idcasofiliacion != value );
                m_idcasofiliacion = value;
			}

		}


        public string Marcador
        {
            get { return m_marcador; }
            set
            {
                m_isChanged |= (m_marcador != value);
                m_marcador = value;
            }
        }

        public string Allello1
        {
            get { return m_allello1; }
            set
            {
                m_isChanged |= (m_allello1 != value);
                m_allello1 = value;
            }
        }
        public string Allello2
        {
            get { return m_allello2; }
            set
            {
                m_isChanged |= (m_allello2 != value);
                m_allello2 = value;
            }
        }
        public string Subitem
        {
            get { return m_subitem; }
            set
            {
                m_isChanged |= (m_subitem != value);
                m_subitem = value;
            }
        }
        public string Allello10
        {
            get { return m_allello10; }
            set
            {
                m_isChanged |= (m_allello10 != value);
                m_allello10 = value;
            }
        }
        public string Allello3
        {
            get { return m_allello3; }
            set
            {
                m_isChanged |= (m_allello3 != value);
                m_allello3 = value;
            }
        }
        public string Allello4
        {
            get { return m_allello4; }
            set
            {
                m_isChanged |= (m_allello4 != value);
                m_allello4 = value;
            }
        }
        public string Allello5
        {
            get { return m_allello5; }
            set
            {
                m_isChanged |= (m_allello5 != value);
                m_allello5 = value;
            }
        }

        public string Allello6
        {
            get { return m_allello6; }
            set
            {
                m_isChanged |= (m_allello6 != value);
                m_allello6 = value;
            }
        }

        public string Allello7
        {
            get { return m_allello7; }
            set
            {
                m_isChanged |= (m_allello7 != value);
                m_allello7 = value;
            }
        }

        public string Allello8
        {
            get { return m_allello8; }
            set
            {
                m_isChanged |= (m_allello8 != value);
                m_allello8 = value;
            }
        }

        public string Allello9
        {
            get { return m_allello9; }
            set
            {
                m_isChanged |= (m_allello9 != value);
                m_allello9 = value;
            }
        }
        public string Ip
        {
            get { return m_ip; }
            set
            {
                m_isChanged |= (m_ip != value);
                m_ip = value;
            }
        }


        /// <summary>
        /// 



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
