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
    public sealed class ItemEfector : Business.BaseDataAccess
	{

		#region Private Members
		private bool m_isChanged;
        private int m_iditemEfector;
       
        private Item m_iditem; 
		private Efector m_idefector; 
		 
		private Efector m_idefectorderivacion; 
		 
        private bool m_disponible;
        private Usuario m_idusuarioregistro;
        private DateTime m_fecharegistro;
        private bool m_informable;
        private bool m_sininsumo;
        private int m_idpresentacion;
        private String m_resultadoDefecto;


        #endregion

        #region Default ( Empty ) Class Constuctor
        /// <summary>
        /// default constructor
        /// </summary>
        public ItemEfector()
		{
            m_iditemEfector = 0;
            m_iditem = new Item();
            m_idefector = new Efector(); 
		 
            m_idefectorderivacion = new Efector(); 
		 
            m_disponible = true;
           
            m_informable = true;

            m_sininsumo = false;
            m_idpresentacion = 0;
            m_resultadoDefecto= String.Empty;
            //m_isscreening = false;
        }
		#endregion // End of Default ( Empty ) Class Constuctor

		#region Required Fields Only Constructor
		/// <summary>
		/// required (not null) fields only constructor
		/// </summary>
		public ItemEfector(
            Item iditem,
			Efector idefector, 
			 
			Efector idefectorderivacion, 
			 
            
            bool disponible,
			 
			Usuario idusuarioregistro, 
			DateTime fecharegistro,
            bool informable ,
             bool   sininsumo ,
             int  idpresentacion,
             string resultadoDefecto
            )
			: this()
		{
            m_iditem  = iditem;
			m_idefector = idefector;
			 
			m_idefectorderivacion = idefectorderivacion;
		 

            m_disponible = disponible;
			 
			m_idusuarioregistro = idusuarioregistro;
			m_fecharegistro = fecharegistro;
            m_informable = informable;
            m_sininsumo = sininsumo;
            m_idpresentacion = idpresentacion;
            m_resultadoDefecto = resultadoDefecto;
        }
		#endregion // End Required Fields Only Constructor

		#region Public Properties
			
		/// <summary>
		/// 
		/// </summary>
		public int IdItemEfector
		{
			get { return m_iditemEfector; }
			set
			{
				m_isChanged |= (m_iditemEfector != value );
                m_iditemEfector = value;
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
        public Efector IdEfector
		{
			get { return m_idefector; }
			set
			{
				m_isChanged |= ( m_idefector != value ); 
				m_idefector = value;
			}

		}


        public string ResultadoDefecto
        {
            get { return m_resultadoDefecto; }

            set
            {
                if (value == null)
                    throw new ArgumentOutOfRangeException("Null value not allowed for m_resultadoDefecto", value, "null");

                if (value.Length > 200)
                    throw new ArgumentOutOfRangeException("Invalid value for m_resultadoDefecto", value, value.ToString());

                m_isChanged |= (m_resultadoDefecto != value); m_resultadoDefecto = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>


        /// <summary>
        /// 
        /// </summary>
        public Efector IdEfectorDerivacion
		{
			get { return m_idefectorderivacion; }
			set
			{
				m_isChanged |= ( m_idefectorderivacion != value ); 
				m_idefectorderivacion = value;
			}

		}
			
		/// <summary>
		/// 
	 
        public bool Informable
        {
            get { return m_informable; }
            set
            {
                m_isChanged |= (m_informable != value);
                m_informable = value;
            }

        }

        public bool Disponible
        {
            get { return m_disponible; }
            set
            {
                m_isChanged |= (m_disponible != value);
                m_disponible = value;
            }

        }

        public bool SinInsumo
        {
            get { return m_sininsumo; }
            set
            {
                m_isChanged |= (m_sininsumo != value);
                m_sininsumo = value;
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
        public int IdPresentacionDefecto 
        {
            get { return m_idpresentacion; }
            set
            {
                m_isChanged |= (m_idpresentacion != value);
                m_idpresentacion = value;
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
				
		#endregion 

        #region Public Metodo


        //public string BuscarRecomendaciones()
        //{
        //    string recomendaciones = "";
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(ItemRecomendacion));
        //    crit.Add(Expression.Eq("IdItem", this));
        //    IList detalle = crit.List();
        //    if (detalle.Count > 0)
        //    {
        //        foreach (ItemRecomendacion oDetalle in detalle)
        //        {
        //            if (recomendaciones == "")
        //                recomendaciones = oDetalle.IdRecomendacion.Descripcion;
        //            else
        //                recomendaciones += "\r" + oDetalle.IdRecomendacion.Descripcion;
        //        }
        //    }
        //    return recomendaciones;
        //}

        public bool TieneFormula()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Formula));
            crit.Add(Expression.Eq("IdItem", this));
            crit.Add(Expression.Eq("IdTipoFormula", 1));
            crit.Add(Expression.Eq("Baja", false));

            IList detalle = crit.List();
            if (detalle.Count > 0)
                return true;
            else
                return false;
        }


        public bool VerificaValoresMinimosMaximos( string valorControl)
        {
            bool devolver = true;
            if (valorControl !="")
            {
                try
                {
                    if (this.IdItem.IdTipoResultado == 1) //solo para los numericos
                    {
                        decimal m_minimo = this.IdItem.ValorMinimo;
                        decimal m_maximo = this.IdItem.ValorMaximo;
                        if ((m_minimo != -1) && (m_maximo != -1))  //si tiene valores minimos y maximos predefinidos
                        {
                            ////Control control1 = Master.FindControl("ContentPlaceHolder1").FindControl("Panel1").FindControl(oItem.IdItem.ToString());
                            ////TextBox txt = txt = (TextBox)control1;
                            decimal valor = decimal.Parse(valorControl, System.Globalization.CultureInfo.InvariantCulture);
                            if (valor < m_minimo)
                                devolver = false; 
                            if (valor > m_maximo)
                                devolver = false;
                        }
                    }
                }
                catch { devolver = false; }
            }
            else  devolver=true;
            return devolver;
        }

        
        //public string GetValorFormula(Protocolo oProtocolo)
        //{
        //    string valor = "NA";
        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(Formula));
        //    crit.Add(Expression.Eq("IdItem", this));
        //    crit.Add(Expression.Eq("IdEfector", this.IdEfector));
        //    crit.Add(Expression.Eq("Baja", false));

        //    IList lista = crit.List();
        //    if (lista.Count > 0)
        //    {
        //        foreach (Formula oFormula in lista)
        //        {
        //            //string[] field = moje.ToString().Split(("/").ToCharArray(), StringSplitOptions);
        //            string formulacalculada = oFormula.ContenidoFormula;
        //            string aux = oFormula.ContenidoFormula.Replace("+", ";");
        //            aux = aux.Replace("-", ";");
        //            aux = aux.Replace("/", ";");
        //            aux = aux.Replace("*", ";");


        //            string[] arr = aux.Split((";").ToCharArray());

        //            foreach (string m in arr)
        //            {
        //                string codigoDet = m.Replace("[", "");
        //                codigoDet = codigoDet.Replace("]", "");
        //                decimal valorEncontrado = BuscarResultadoItem(codigoDet, oProtocolo);
        //                formulacalculada = formulacalculada.Replace("[" + codigoDet + "]", valorEncontrado.ToString());

        //            }

        //            //      valor =System.Web.UI.s formulacalculada.

        //            Parser p = new Parser();
        //            double resultado = 0;
        //            if (p.Evaluate(formulacalculada))
        //                resultado = p.Result;

        //            valor = resultado.ToString();
        //        }
        //    }
        //    return valor;


        //}

        //public decimal BuscarResultadoItem(string codigoDet, Protocolo oPr)
        //{

        //    decimal valor = 0;
        //    Item oItem = new Item();
        //    oItem = (Item)oItem.Get(typeof(Item), "Codigo", codigoDet);

        //    ISession m_session = NHibernateHttpModule.CurrentSession;
        //    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
        //    crit.Add(Expression.Eq("IdItem", oItem));
        //    crit.Add(Expression.Eq("IdProtocolo", oPr));
        //    crit.Add(Expression.Eq("IdEfector", oPr.IdEfector));
        //    //crit.Add(Expression.Eq("Baja", false));
        //    IList lista = crit.List();
        //    if (lista.Count > 0)
        //    {
        //        foreach (DetalleProtocolo oDet in lista)
        //        {
        //            valor = oDet.ResultadoNum;
        //        }
        //    }
        //    return valor;
        //}



        #endregion



    }
}
