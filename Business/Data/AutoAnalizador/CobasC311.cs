using System;
using System.Collections;

namespace Business.Data.AutoAnalizador
{
    [Serializable]
    public sealed class CobasC311 : Business.BaseDataAccess
    {

        #region Private Members
        private bool m_isChanged;
        private int m_id;
        private string m_idItemCobas;
        private string m_idItemSil;
        private bool m_habilitado;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public CobasC311()
        {
            m_id = 0;
            m_idItemCobas = string.Empty;
            m_idItemSil = string.Empty;
            m_habilitado = false;
        }
        #endregion // End of Default ( Empty ) Class Constuctor

        #region Required Fields Only Constructor
        /// <summary>
        /// required (not null) fields only constructor
        /// </summary>
        public CobasC311(
            string idItemCobas,
            string idItemSil,
            bool habilitado)
            : this()
        {
            m_idItemCobas = idItemCobas;
            m_idItemSil = idItemSil;
            m_habilitado = habilitado;
        }
        #endregion // End Required Fields Only Constructor

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set
            {
                m_isChanged |= (m_id != value);
                m_id = value;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string IdItemCobas
        {
            get { return m_idItemCobas; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for Idbastec", value, "null");

                if (value.Length <= 0)
                    throw new ArgumentOutOfRangeException("Invalid value for idCobas", value, value.ToString());
                
                m_isChanged |= (m_idItemCobas != value); m_idItemCobas = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IdItemSil
        {
            get { return m_idItemSil; }
            set
            {
                m_isChanged |= (m_idItemSil != value);
                m_idItemSil = value;
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
                m_isChanged |= (m_habilitado != value);
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
