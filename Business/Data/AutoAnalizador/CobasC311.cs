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
        private string m_itemCobas;
        private int m_idItemCobas;
        private int m_idItemSil;
        private string m_prefijo;
        private string m_tipoMuestra;
        private bool m_habilitado;
        private string m_codigoSil;
        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public CobasC311()
        {
            m_id = 0;
            m_itemCobas = string.Empty;
            m_idItemCobas = 0;
            m_idItemSil = 0;
            m_prefijo = string.Empty;
            m_tipoMuestra = string.Empty;
            m_habilitado = false;
            m_codigoSil = string.Empty;
        }
        #endregion // End of Default ( Empty ) Class Constuctor

        #region Required Fields Only Constructor
        /// <summary>
        /// required (not null) fields only constructor
        /// </summary>
        public CobasC311(
            string itemCobas,
            int idItemCobas,
            int idItemSil,
            string tipoMuestra,
            string Prefijo,
            bool habilitado,
            string codigoSil)
            : this()
        {
            m_itemCobas = itemCobas;
            m_idItemCobas = idItemCobas;
            m_idItemSil = idItemSil;
            m_tipoMuestra = tipoMuestra;
            m_prefijo = Prefijo;
            m_habilitado = habilitado;
            m_codigoSil = codigoSil;
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
        public int IdItemCobas
        {
            get { return m_idItemCobas; }

            set
            {
                m_isChanged |= (m_idItemCobas != value);
                m_idItemCobas = value;

            }
        }

        public string ItemCobas
        {
            get { return m_itemCobas; }
            set
            {
                m_isChanged |= (m_itemCobas != value);
                m_itemCobas = value;
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        public int IdItemSil
        {
            get { return m_idItemSil; }
            set
            {
                m_isChanged |= (m_idItemSil != value);
                m_idItemSil = value;
            }

        }

        public string TipoMuestra
        {
            get { return m_tipoMuestra; }
            set
            {
                m_isChanged |= (m_tipoMuestra != value);
                m_tipoMuestra = value;
            }
        }

            public string Prefijo
        {
            get { return m_prefijo; }
            set
            {
                m_isChanged |= (m_prefijo != value);
                m_prefijo = value;
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

        public string CodigoSil
        {
            get { return m_codigoSil; }
            set
            {
                m_isChanged |= (m_codigoSil != value);
                m_codigoSil = value;
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
