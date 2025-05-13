using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Business.Data.Laboratorio {
    [Serializable]
    public sealed class DerivacionMotivoCancelacion : Business.BaseDataAccess {
        #region Private Members
        private int idMotivo;
        private string descripcion;
        private bool baja;
        #endregion

        #region Constructor
        public DerivacionMotivoCancelacion() {
            idMotivo = 0;
            descripcion = "";
            baja = false;
        }

        public DerivacionMotivoCancelacion(string _descripcion) {
           descripcion = _descripcion;
            baja = false;
        }
        #endregion

        #region Metodos
        public int IdMotivo{
            get { return idMotivo; }
            set
            {
                idMotivo = value;
            }
        }
        public string Descripcion {
            get { return descripcion; }
            set {
                descripcion = value;
            }
        }

        public bool Baja {
            get { return baja; }
            set {
                baja = value;
            }
        }
        #endregion
    }
}
