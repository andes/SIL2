/*
insert license info here
*/
using System;
using System.Collections;
using NHibernate;
using NHibernate.Expression;

namespace Business.Data.Laboratorio
{
	/// <summary>
	///	Generated by MyGeneration using the NHibernate Object Mapping template
	/// </summary>
	[Serializable]
	public sealed class SolicitudScreening: Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;

		private int m_idsolicitudscreening;
        private int m_idsolicitudscreeningorigen; 		
		private Protocolo m_idprotocolo;        
        
        private int m_numerotarjeta;          
    private string m_medicoSolicitante;
    private string m_apellidoMaterno;
    private string m_apellidoPaterno;
    private string m_nombreParentesco;
    private int m_numerodocumentoParentesco;
    private DateTime m_fechaNacimientoParentesco;
    private int m_idLugarControl;



		private string m_horanacimiento; 
		private int m_edadgestacional; 
		private decimal m_peso;        
        private bool m_primeramuestra;
        private string  m_motivorepeticion; 
		private DateTime m_fechaextraccion; 
		private string m_horaextraccion; 
		private bool m_ingestaleche24horas; 

		private string m_tipoalimentacion; 
		private bool m_antibiotico; 
		private bool m_transfusion; 
		private bool m_corticoides; 
		private bool m_dopamina; 
		private bool m_enfermedadtiroideamaterna; 
		private string m_antecedentesmaternos;
        private bool m_corticoidesmaterno; 
		
        private DateTime m_fechacargaorigen;
        private DateTime m_fechaenvioorigen;

		#endregion

		#region Default ( Empty ) Class Constuctor
		/// <summary>
		/// default constructor
		/// </summary>
		public SolicitudScreening()
		{
			m_idsolicitudscreening = 0;
            m_idsolicitudscreeningorigen = 0;
            m_idprotocolo = new Protocolo(); 			

            m_numerotarjeta=0;          
            m_medicoSolicitante=String.Empty;
            m_apellidoMaterno=String.Empty;
            m_apellidoPaterno=String.Empty;
            m_nombreParentesco=String.Empty;
            m_numerodocumentoParentesco=0;
            m_fechaNacimientoParentesco=DateTime.MinValue;
            m_idLugarControl=0;

            m_horanacimiento = String.Empty; 
			m_edadgestacional = 0; 
			m_peso = 0; 			
			m_primeramuestra = false; 
			m_fechaextraccion = DateTime.MinValue; 
			m_horaextraccion = String.Empty; 
			m_ingestaleche24horas = false;
            m_tipoalimentacion = String.Empty;  
			m_antibiotico = false; 
			m_transfusion = false; 
			m_corticoides = false; 
			m_dopamina = false; 
			m_enfermedadtiroideamaterna = false; 
			m_antecedentesmaternos = String.Empty;
            m_corticoidesmaterno = false;

            m_fechacargaorigen = DateTime.MinValue;
            m_fechaenvioorigen = DateTime.MinValue;
		}
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public SolicitudScreening(			
			Protocolo idprotocolo,
            int idsolicitudscreeningorigen,

            int numerotarjeta,
            string medicoSolicitante,
            string apellidoMaterno,
            string apellidoPaterno,
            string nombreParentesco,
            int numerodocumentoParentesco,
            DateTime fechaNacimientoParentesco,
            int idLugarControl,



			string horanacimiento, 
			int edadgestacional, 
			decimal peso, 
			bool prematuro, 
			bool primeramuestra, 
			DateTime fechaextraccion, 
			string horaextraccion, 
			bool ingestaleche24horas, 
			string tipoalimentacion, 
			bool antibiotico, 
			bool transfusion, 
			bool corticoides, 
			bool dopamina, 
			bool enfermedadtiroideamaterna, 
			string antecedentesmaternos, 
			bool corticoidesmaterno )
			: this()
		{
			
			m_idprotocolo = idprotocolo;
            m_idsolicitudscreeningorigen = idsolicitudscreeningorigen;            
			
            m_numerotarjeta= numerotarjeta;
            m_medicoSolicitante=medicoSolicitante;
            m_apellidoMaterno= apellidoMaterno;
            m_apellidoPaterno=apellidoPaterno;
            m_nombreParentesco=nombreParentesco;
            m_numerodocumentoParentesco=numerodocumentoParentesco;
            m_fechaNacimientoParentesco = fechaNacimientoParentesco;
            m_idLugarControl = idLugarControl;


			m_horanacimiento = horanacimiento;
			m_edadgestacional = edadgestacional;
			m_peso = peso;			
			m_primeramuestra = primeramuestra;
			m_fechaextraccion = fechaextraccion;
			m_horaextraccion = horaextraccion;
			m_ingestaleche24horas = ingestaleche24horas;
			m_tipoalimentacion = tipoalimentacion;
			m_antibiotico = antibiotico;
			m_transfusion = transfusion;
			m_corticoides = corticoides;
			m_dopamina = dopamina;
			m_enfermedadtiroideamaterna = enfermedadtiroideamaterna;
			m_antecedentesmaternos = antecedentesmaternos;
            m_corticoidesmaterno = corticoidesmaterno;
		}
		#endregion // End Required Fields Only Constructor

		#region Public Properties			
	
		public int IdSolicitudScreening
		{
			get { return m_idsolicitudscreening; }
			set
			{
				m_isChanged |= ( m_idsolicitudscreening != value ); 
				m_idsolicitudscreening = value;
			}
		}

        public int IdSolicitudScreeningOrigen
        {
            get { return m_idsolicitudscreeningorigen; }
            set
            {
                m_isChanged |= (m_idsolicitudscreeningorigen != value);
                m_idsolicitudscreeningorigen = value;
            }
        }		
		public Protocolo IdProtocolo
		{
			get { return m_idprotocolo; }
			set
			{
				m_isChanged |= ( m_idprotocolo != value ); 
				m_idprotocolo = value;
			}
		}

        public int NumeroTarjeta
        {
            get { return m_numerotarjeta; }
            set
            {
                m_isChanged |= (m_numerotarjeta != value);
                m_numerotarjeta = value;
            }
        }




        public String MedicoSolicitante
        {
            get { return m_medicoSolicitante; }
            set
            {
                m_isChanged |= (m_medicoSolicitante != value);
                m_medicoSolicitante = value;
            }
        }


        public String ApellidoMaterno
        {
            get { return m_apellidoMaterno; }
            set
            {
                m_isChanged |= (m_apellidoMaterno != value);
                m_apellidoMaterno = value;
            }
        }

        public String ApellidoPaterno
        {
            get { return m_apellidoPaterno; }
            set
            {
                m_isChanged |= (m_apellidoPaterno != value);
                m_apellidoPaterno = value;
            }
        }


        public String NombreParentesco
        {
            get { return m_nombreParentesco; }
            set
            {
                m_isChanged |= (m_nombreParentesco != value);
                m_nombreParentesco = value;
            }
        }


        public int NumerodocumentoParentesco
		{
			get { return m_numerodocumentoParentesco; }
			set
			{
                m_isChanged |= (m_numerodocumentoParentesco != value);
                m_numerodocumentoParentesco = value;
			}
		}


        public DateTime FechaNacimientoParentesco
		{
			get { return m_fechaNacimientoParentesco; }
			set
			{
                m_isChanged |= (m_fechaNacimientoParentesco != value);
                m_fechaNacimientoParentesco = value;
			}

		}

        public int IdLugarControl
		{
			get { return m_idLugarControl; }
			set
			{
                m_isChanged |= (m_idLugarControl != value);
                m_idLugarControl = value;
			}
		}
        


        /// <summary>
        /// ///////////////////////////
        /// </summary>

        public String MotivoRepeticion
        {
            get { return m_motivorepeticion; }
            set
            {
                m_isChanged |= (m_motivorepeticion != value);
                m_motivorepeticion = value;
            }
        }		
      
		public string HoraNacimiento
		{
			get { return m_horanacimiento; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for HoraNacimiento", value, "null");
				
				if(  value.Length > 5)
					throw new ArgumentOutOfRangeException("Invalid value for HoraNacimiento", value, value.ToString());
				
				m_isChanged |= (m_horanacimiento != value); m_horanacimiento = value;
			}
		}		
		public int EdadGestacional
		{
			get { return m_edadgestacional; }
			set
			{
				m_isChanged |= ( m_edadgestacional != value ); 
				m_edadgestacional = value;
			}

		}			
		public decimal Peso
		{
			get { return m_peso; }
			set
			{
				m_isChanged |= ( m_peso != value ); 
				m_peso = value;
			}

		}		
		public bool PrimeraMuestra
		{
			get { return m_primeramuestra; }
			set
			{
				m_isChanged |= ( m_primeramuestra != value ); 
				m_primeramuestra = value;
			}

		}		
		public DateTime FechaExtraccion
		{
			get { return m_fechaextraccion; }
			set
			{
				m_isChanged |= ( m_fechaextraccion != value ); 
				m_fechaextraccion = value;
			}

		}				
		public string HoraExtraccion
		{
			get { return m_horaextraccion; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for HoraExtraccion", value, "null");
				
				if(  value.Length > 5)
					throw new ArgumentOutOfRangeException("Invalid value for HoraExtraccion", value, value.ToString());
				
				m_isChanged |= (m_horaextraccion != value); m_horaextraccion = value;
			}
		}			
		public bool IngestaLeche24Horas
		{
			get { return m_ingestaleche24horas; }
			set
			{
				m_isChanged |= ( m_ingestaleche24horas != value ); 
				m_ingestaleche24horas = value;
			}

		}					
		public String TipoAlimentacion
		{
			get { return m_tipoalimentacion; }
			set
			{
				m_isChanged |= ( m_tipoalimentacion != value ); 
				m_tipoalimentacion = value;
			}

		}				
		public bool Antibiotico
		{
			get { return m_antibiotico; }
			set
			{
				m_isChanged |= ( m_antibiotico != value ); 
				m_antibiotico = value;
			}

		}			
		public bool Transfusion
		{
			get { return m_transfusion; }
			set
			{
				m_isChanged |= ( m_transfusion != value ); 
				m_transfusion = value;
			}

		}			
		public bool Corticoides
		{
			get { return m_corticoides; }
			set
			{
				m_isChanged |= ( m_corticoides != value ); 
				m_corticoides = value;
			}

		}				
		public bool Dopamina
		{
			get { return m_dopamina; }
			set
			{
				m_isChanged |= ( m_dopamina != value ); 
				m_dopamina = value;
			}

		}					
		public bool EnfermedadTiroideaMaterna
		{
			get { return m_enfermedadtiroideamaterna; }
			set
			{
				m_isChanged |= ( m_enfermedadtiroideamaterna != value ); 
				m_enfermedadtiroideamaterna = value;
			}

		}				
		public string AntecedentesMaternos
		{
			get { return m_antecedentesmaternos; }

			set	
			{	
				if( value == null )
					throw new ArgumentOutOfRangeException("Null value not allowed for AntecedentesMaternos", value, "null");
				
				if(  value.Length > 1073741823)
					throw new ArgumentOutOfRangeException("Invalid value for AntecedentesMaternos", value, value.ToString());
				
				m_isChanged |= (m_antecedentesmaternos != value); m_antecedentesmaternos = value;
			}
		}        
        public bool CorticoidesMaterno
        {
            get { return m_corticoidesmaterno; }
            set
            {
                m_isChanged |= (m_corticoidesmaterno != value);
                m_corticoidesmaterno = value;
            }

        }

        public DateTime FechaCargaOrigen
        {
            get { return m_fechacargaorigen; }
            set
            {
                m_isChanged |= (m_fechacargaorigen != value);
                m_fechacargaorigen = value;
            }

        }
        public DateTime FechaEnvioOrigen
        {
            get { return m_fechaenvioorigen; }
            set
            {
                m_isChanged |= (m_fechaenvioorigen != value);
                m_fechaenvioorigen = value;
            }

        }


        public int GetCantidadAlarmas()
        {
            int i = 0;
            AlarmaScreening oDetalle = new AlarmaScreening();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(AlarmaScreening));
            crit.Add(Expression.Eq("IdSolicitudScreening", this));
            IList items = crit.List();
            i=items.Count;
            return i;
        }
        /// <summary>
		/// Returns whether or not the object has changed it's values.
		/// </summary>
		public bool IsChanged
		{
			get { return m_isChanged; }
		}
				
		#endregion 
	
        public void EliminarAlarmas()
        {
            
           AlarmaScreening oDetalle = new AlarmaScreening();
           ISession m_session = NHibernateHttpModule.CurrentSession;
           ICriteria crit = m_session.CreateCriteria(typeof(AlarmaScreening));
           crit.Add(Expression.Eq("IdSolicitudScreening", this));           
           IList items = crit.List();
           foreach (AlarmaScreening oDet in items)
           {
               oDet.Delete();
           }
        
        }

        public void GuardarDescripcionAlarma(string descripcionAlarma, SolicitudScreening oSolicitud, int user)
        {  
            AlarmaScreening oRegistro = new AlarmaScreening();
            oRegistro.IdSolicitudScreening = this;
            oRegistro.Descripcion = descripcionAlarma;
            oRegistro.IdUsuarioRegistro =user;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
       
        }

        public void GuardarParentesco()
        {
            //Parentesco oDetalle = new Parentesco();
            ISession m_session2 = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session2.CreateCriteria(typeof(Parentesco));
            crit2.Add(Expression.Eq("IdPaciente", this.IdProtocolo.IdPaciente));
            IList items2 = crit2.List();
       
                foreach (Parentesco oDet in items2)
                {
                    oDet.Delete();
                }
     
                Parentesco par = new Parentesco();

                par.Apellido = this.ApellidoMaterno;
                par.Nombre = this.NombreParentesco;
                par.IdTipoDocumento = 1; // Convert.ToInt32(ddlTipoDocP.SelectedValue);
                par.IdPaciente = this.IdProtocolo.IdPaciente;
                par.TipoParentesco = "Madre";
                par.NumeroDocumento = this.NumerodocumentoParentesco;


                par.FechaNacimiento = this.FechaNacimientoParentesco;

                //par.IdProvincia = -1;
                //par.IdPais = 54;
                par.IdUsuario = this.IdProtocolo.IdUsuarioRegistro;

                //guardo la fecha actual de modificacion
                par.FechaModificacion = DateTime.Now;
                par.Save();
        

        }
    }
}