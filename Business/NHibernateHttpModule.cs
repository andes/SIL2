using System;
using System.IO;
using System.Collections;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using Business;

namespace Business
{
	/// <summary>
	/// Summary description for NHibernateHttpModule.
	/// </summary>
    public class NHibernateHttpModule : IHttpModule
	{ 
		// this is only used if not running in HttpModule mode
		protected static ISessionFactory m_factory;
		
		// this is only used if not running in HttpModule mode
		private static ISession m_session;

		private static readonly string KEY_NHIBERNATE_FACTORY = "NHibernateSessionFactory"; 
		private static readonly string KEY_NHIBERNATE_SESSION = "NHibernateSession"; 

		public void Init(HttpApplication context) 
		{ 
			context.BeginRequest += new EventHandler(context_BeginRequest);
			context.EndRequest += new EventHandler(context_EndRequest); 
		} 
 
		public void Dispose() 
		{ 
			if (m_session!=null)
			{
				m_session.Close();
				m_session.Dispose();
			}

			if (m_factory!=null)
			{
				m_factory.Close();
			}
		} 
 
		private void context_BeginRequest (object sender, EventArgs e) 
		{ 
			HttpApplication application = (HttpApplication)sender; 
			HttpContext context = application.Context; 

			context.Items[KEY_NHIBERNATE_SESSION] = CreateSession(); 
		} 
 
		private void context_EndRequest(object sender, EventArgs e) 
		{ 
			HttpApplication application = (HttpApplication)sender; 
			HttpContext context = application.Context; 
 
			ISession session = context.Items[KEY_NHIBERNATE_SESSION] as ISession; 
			if (session != null) 
			{ 
				try 
				{ 
					session.Flush(); 
					session.Close(); 
				} 
				catch {} 
			} 
 
			context.Items[KEY_NHIBERNATE_SESSION] = null; 
		} 

		
		public static ISessionFactory CreateSessionFactory()
		{
			Configuration config;
			ISessionFactory factory;
			HttpContext currentContext = HttpContext.Current; 
			cAppConfig oApp;
			Utility oUtil = new Utility();
			
			config = new NHibernate.Cfg.Configuration();			

			if (HttpContext.Current == null)
			{
				oApp = new cAppConfig(ConfigFileType.AppConfig) ;	
				config.SetProperty("hibernate.connection.connection_string", oApp.GetValue("hibernate.connection.connection_string"));
			}
			else
			{
				oApp = new cAppConfig(ConfigFileType.WebConfig) ;
			//	string strDesEnc  = oUtil.Decrypt(oApp.GetValue("hibernate.connection.connection_string"));
                config.SetProperty("hibernate.connection.connection_string", oApp.GetValue("hibernate.connection.connection_string"));
			}

    
			if (config == null)
			{
				throw new InvalidOperationException("NHibernate configuration is null.");
			}

			if (HttpContext.Current==null)
				config.SetProperty("hibernate.connection.connection_string",oApp.GetValue("hibernate.connection.connection_string"));
			
			config.AddAssembly("Business");
			factory = config.BuildSessionFactory();
			
			if (currentContext != null) currentContext.Items[KEY_NHIBERNATE_FACTORY] = factory; 
			
			if (factory==null)
				throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");
			else
				return factory;
		}

		public static ISessionFactory CreateSessionFactory(IDictionary dir)
		{
			Configuration config;
			ISessionFactory factory;
			HttpContext currentContext = HttpContext.Current; 
	
			config = new Configuration();
			
			if (config==null) throw new InvalidOperationException("NHibernate configuration is null.");
            			
			config.AddAssembly("Business");
			factory = config.BuildSessionFactory();
			
			if (currentContext != null)
				currentContext.Items[KEY_NHIBERNATE_FACTORY] = factory; 
			
			if (factory==null)
				throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");
			else
				return factory;
		}

		public static ISessionFactory CurrentFactory
		{
			get
			{
				if (HttpContext.Current==null)
				{
					// Corriendo sin HttpContext (Modo No-web)
					if (m_factory!=null)
						return m_factory;
					else
					{
						m_factory = CreateSessionFactory(); 
						return m_factory;
					}
				}
				else
				{
					// Corriendo con HttpContext (Modo Web)
					HttpContext currentContext = HttpContext.Current; 
				
					ISessionFactory factory = currentContext.Application[KEY_NHIBERNATE_FACTORY] as ISessionFactory;
 
					if (factory == null) 
					{ 
						factory = CreateSessionFactory(); 
						currentContext.Application[KEY_NHIBERNATE_FACTORY] = factory; 
					} 
 
					return factory; 
				}
			}
		}

		public static ISession CreateSession()
		{
			ISessionFactory factory;
			ISession session;

			factory = NHibernateHttpModule.CurrentFactory;
			
			if (factory==null) throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");

			session = factory.OpenSession();
			session.FlushMode = FlushMode.Auto;

			if (session==null) throw new InvalidOperationException("Call to factory.OpenSession() returned null.");

			return session;
		}

		public static ISession CreateSession(string strSource)
		{
			if (! File.Exists(strSource)) throw new Exception("La Base de Datos Original fue Removida") ;
			string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strSource;	
			
			Configuration config = new Configuration();	
			config.SetProperty("hibernate.connection.connection_string",connString);		
			config.AddAssembly("Business");

			return config.BuildSessionFactory().OpenSession();
		}

		public static ISession CurrentSession 
		{ 
			get 
			{ 
				if (HttpContext.Current==null)
				{
					// Corriendo sin HttpContext (Modo No-web)
					if (m_session!=null)
					{
						if (! m_session.IsConnected) m_session.Reconnect();
						m_session.FlushMode = FlushMode.Auto;
						return m_session;
					}
					else
					{
						m_session = CreateSession(); 
						m_session.FlushMode = FlushMode.Auto;

						return m_session;
					}
				}
				else
				{
					// Corriendo con HttpContext (Modo Web)
					HttpContext currentContext = HttpContext.Current; 
				
					ISession session = currentContext.Items[KEY_NHIBERNATE_SESSION] as ISession; 
 
					if (session == null) 
					{ 
						session = CreateSession(); 
						currentContext.Items[KEY_NHIBERNATE_SESSION] = session; 
					} 
 
					return session; 
				}
			} 
		} 

			
		public static void ResetSessionFactory()
		{
			Configuration config;
			HttpContext currentContext = HttpContext.Current; 
			cAppConfig oApp = new cAppConfig(ConfigFileType.AppConfig) ;
	
			config = new NHibernate.Cfg.Configuration();
			
			config.SetProperty("hibernate.connection.connection_string", oApp.GetValue("hibernate.connection.connection_string"));
			config.AddAssembly("Business");
						
			NHibernateHttpModule.m_factory = config.BuildSessionFactory();
			NHibernateHttpModule.m_session = NHibernateHttpModule.m_factory.OpenSession();
					
			if (NHibernateHttpModule.m_factory == null)
				throw new InvalidOperationException("Call to Configuration.BuildSessionFactory() returned null.");
		}

		public static void CloseSession()
		{
			if (m_session != null)
				if (m_session.IsOpen) m_session.Close();
		}
		
	} 

}