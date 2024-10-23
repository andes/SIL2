/*
insert license info here
*/
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using NHibernate;
using NHibernate.Expression;
using MathParser;
using System.IO;

namespace Business.Data.Laboratorio
{
    [Serializable]
    public sealed class DetalleProtocoloPlaca : Business.BaseDataAccess
    {

        #region Private Members
        private bool m_isChanged;

        private int m_idDetalleProtocoloPlaca;
        private int m_idPlaca;
        private int m_idDetalleProtocolo;
        private string m_NamespaceID;


        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public DetalleProtocoloPlaca()
        {
            
            m_idPlaca = 0;
            m_idDetalleProtocolo = 0;
            m_NamespaceID = String.Empty;

        }

        #endregion // End of Default ( Empty ) Class Constuctor

        #region Required Fields Only Constructor
        /// <summary>
        /// required (not null) fields only constructor
        /// </summary>
        public DetalleProtocoloPlaca(
            int idDetalleProtocoloPlaca,
            int idPlaca,
            int idDetalleProtocolo,
            string NamespaceID)
                : this()
        {
            m_idDetalleProtocoloPlaca = idDetalleProtocoloPlaca;
            m_idPlaca = idPlaca;
            m_idDetalleProtocolo = idDetalleProtocolo;
            m_NamespaceID = NamespaceID;

        }



        #endregion // End Required Fields Only Constructor

        #region Public Properties

        public int IdDetalleProtocoloPlaca
        {
            get { return m_idDetalleProtocoloPlaca; }
            set
            {
                m_isChanged |= (m_idDetalleProtocoloPlaca != value);
                m_idDetalleProtocoloPlaca = value;
            }

        }

        public int IdPlaca
        {
            get { return m_idPlaca; }
            set
            {
                m_isChanged |= (m_idPlaca != value);
                m_idPlaca = value;
            }

        }

        public int IdDetalleProtocolo
        {
            get { return m_idDetalleProtocolo; }
            set
            {
                m_isChanged |= (m_idDetalleProtocolo != value);
                m_idDetalleProtocolo = value;
            }

        }

        public String NamespaceID
        {
            get { return m_NamespaceID; }
            set
            {
                m_isChanged |= (m_NamespaceID != value);
                m_NamespaceID = value;
            }

        }

        public bool IsChanged
        {
            get { return m_isChanged; }
        }

        public bool EstaValidado()
        {
            bool validado = false;
            DetalleProtocolo oDetalle = new DetalleProtocolo();
            oDetalle = (Business.Data.Laboratorio.DetalleProtocolo)oDetalle.Get(typeof(Business.Data.Laboratorio.DetalleProtocolo),this.IdDetalleProtocolo);
            if (oDetalle != null)
            {
                if (oDetalle.IdUsuarioValida > 0)
                    validado = true;
                else
                {                  
                    if (oDetalle.IdUsuarioPreValida > 0)
                        validado = true;
                    else
                        validado = false;
                }
            }
            return validado;

        }
    }
}

        #endregion


