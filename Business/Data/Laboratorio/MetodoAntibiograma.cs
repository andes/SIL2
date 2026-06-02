using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Data.Laboratorio
{
    [Serializable]
    public sealed class MetodoAntibiograma : Business.BaseDataAccess
    {
        #region variables

        private bool m_isChanged;
        private int m_idMetodoAntibiograma;
        private string m_codigo;
        private string m_nombre;
        private DateTime m_fechaRegistro;
        private Usuario m_idUsuarioRegistro;
        private bool m_baja;
        #endregion

        #region constructores

        public MetodoAntibiograma()
        {
            m_idMetodoAntibiograma = 0;
            m_codigo = string.Empty;
            m_nombre = string.Empty;
            m_fechaRegistro = DateTime.MinValue;
            m_idUsuarioRegistro = new Usuario();
            m_baja = false;
        }

        public MetodoAntibiograma(int idMetodoAntiobiograma, string codigo, string nombre, DateTime fechaRegistro, Usuario usuario, bool baja) : this ()
        {
            m_idMetodoAntibiograma = idMetodoAntiobiograma;
            m_codigo = codigo;
            m_nombre = nombre;
            m_fechaRegistro = fechaRegistro;
            m_idUsuarioRegistro = usuario;
            m_baja = baja;
        }
        #endregion

        #region funciones

        public int IdMetodoAntibiograma
        {
            get { return m_idMetodoAntibiograma; }
            set
            {
                m_isChanged |= (m_idMetodoAntibiograma != value);
                m_idMetodoAntibiograma = value;
            }

        }

        public string Codigo
        {
            get { return m_codigo; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for Codigo", value, "null");

                if (value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Codigo", value, value.ToString());

                m_isChanged |= (m_codigo != value); m_codigo = value;
            }
        }

        public string Nombre
        {
            get { return m_nombre; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for Nombre", value, "null");

                if (value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for Nombre", value, value.ToString());

                m_isChanged |= (m_nombre != value); m_nombre = value;
            }
        }

        public DateTime FechaRegistro
        {
            get { return m_fechaRegistro; }
            set
            {
                m_isChanged |= (m_fechaRegistro != value);
                m_fechaRegistro = value;
            }

        }

        public Usuario IdUsuarioRegistro
        {
            get { return m_idUsuarioRegistro; }
            set
            {
                m_isChanged |= (m_idUsuarioRegistro != value);
                m_idUsuarioRegistro = value;
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
        /// Returns whether or not the object has changed it's values.
        /// </summary>
        public bool IsChanged
        {
            get { return m_isChanged; }
        }

        #endregion
    }
}
