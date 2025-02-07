using System;
using System.Collections;
using System.Collections.Generic; 
using System.Text;
using NHibernate;
using NHibernate.Collection;

namespace Business.Data.Laboratorio  {
    [Serializable]
    public sealed class LoteDerivacionEstado : Business.BaseDataAccess
    {

        
        private int idEstado;
        private string nombre;
        private bool baja;

        public LoteDerivacionEstado() {
            idEstado = 0;
            nombre = "";
            baja = false;
        }

        #region metodos publicos
     
        public bool Baja {
            get { return baja; }
            set { baja = value; }
        }
        
        public int IdEstado {
            get { return idEstado; }
            set { idEstado = value; }
        }

        public string Nombre {
            get { return nombre; }
            set { nombre = value; }
        }
        #endregion

        public static List<LoteDerivacionEstado> estados() {
            List<LoteDerivacionEstado> estados = new List<LoteDerivacionEstado>();
            try {
                ISession session = NHibernateHttpModule.CurrentSession;
                IList lista = session.CreateQuery("from LoteDerivacionEstado where baja = 0").List();
               
                foreach (LoteDerivacionEstado item in lista) {
                    estados.Add(item);
                }
            } catch(Exception) {

            }
          
            

            return estados;
        }
    }
}
