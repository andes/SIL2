using System;
using System.Collections;
using System.Configuration;
using System.Security.Policy;
using System.Xml;
using System.Web;
using System.Windows.Forms;

namespace Business
{	

	public enum ConfigFileType
	{
		WebConfig,
		AppConfig,
	}

	public class cAppConfig : System.Configuration.AppSettingsReader
	{
		public cAppConfig(ConfigFileType tipoArchivo)
		{
			this._configType = tipoArchivo;
		}

		public string docName = string.Empty;
		
		private ConfigFileType _configType;
		public ConfigFileType ConfigType 
		{
			get 
			{
				return _configType;
			}
			set 
			{
				_configType = value;
			}
		}
	
		public bool SetValue(string key, string value)
		{
			XmlDocument cfgdoc = new XmlDocument();
			loadConfigDoc(cfgdoc);
			XmlNode node = cfgdoc.SelectSingleNode("//nhibernate");

			if (node == null) throw new System.InvalidOperationException("No se encontró la seccion appSettings");
			
			try 
			{
				XmlElement addElem = ((XmlElement)(node.SelectSingleNode("//add[@key='" + key + "']")));
				if (!(addElem == null)) 
				{
					addElem.SetAttribute("value", value);
				} 
				else 
				{
					XmlElement enTry = cfgdoc.CreateElement("add");
					enTry.SetAttribute("key", key);
					enTry.SetAttribute("value", value);
					node.AppendChild(enTry);
				}
				saveConfigDoc(cfgdoc, docName);
				return true;
			} 
			catch 
			{
				return false;
			}
		}
		

		public bool SetValue(string nodo, string key, string value)
		{
			XmlDocument cfgdoc = new XmlDocument();
			loadConfigDoc(cfgdoc);
			XmlNode node = cfgdoc.SelectSingleNode("//" + nodo);

			if (node == null) throw new System.InvalidOperationException("No se encontró la seccion appSettings");
			
			try 
			{
				XmlElement addElem = ((XmlElement)(node.SelectSingleNode("//add[@key='" + key + "']")));
				if (!(addElem == null)) 
				{
					addElem.SetAttribute("value", value);
				} 
				else 
				{
					XmlElement enTry = cfgdoc.CreateElement("add");
					enTry.SetAttribute("key", key);
					enTry.SetAttribute("value", value);
					node.AppendChild(enTry);
				}
				saveConfigDoc(cfgdoc, docName);
				return true;
			} 
			catch 
			{
				return false;
			}
		}

		public string GetValue(string key)
		{
			string rValue = "";
			XmlDocument cfgdoc = new XmlDocument();
			loadConfigDoc(cfgdoc);
			XmlNode node = cfgdoc.SelectSingleNode("//nhibernate");
			if (node == null) throw new System.InvalidOperationException("NHibernate section not found");
			
			try 
			{
				XmlElement addElem = ((XmlElement)(node.SelectSingleNode("//add[@key='" + key + "']")));

				if (!(addElem == null)) rValue = addElem.Attributes[1].InnerText;
			} 
			catch 
			{
			}

			return rValue;
		}

		public string GetValue(string nodo, string key)
		{
			string rValue = "";
			XmlDocument cfgdoc = new XmlDocument();
			loadConfigDoc(cfgdoc);
			XmlNode node = cfgdoc.SelectSingleNode("//" + nodo);
			if (node == null) throw new System.InvalidOperationException(nodo + " section not found");
			
			try 
			{
				XmlElement addElem = ((XmlElement)(node.SelectSingleNode("//add[@key='" + key + "']")));

				if (!(addElem == null)) rValue = addElem.Attributes[1].InnerText;
			} 
			catch 
			{
			}

			return rValue;
		}

		private void saveConfigDoc(XmlDocument cfgDoc, string cfgDocPath)
		{
			try 
			{
				XmlTextWriter writer = new XmlTextWriter(cfgDocPath, null);
				writer.Formatting = Formatting.Indented;
				cfgDoc.WriteTo(writer);
				writer.Flush();
				writer.Close();
				return;
			} 
			catch 
			{
				throw;
			}
		}

		private XmlDocument loadConfigDoc(XmlDocument cfgDoc)
		{
			System.Reflection.Assembly Asm;
			Asm = System.Reflection.Assembly.GetEntryAssembly();
			docName = string.Empty;

            if (ConfigType == ConfigFileType.AppConfig)
                docName = Application.ExecutablePath + ".config";
            else
                docName = cAppConfig.GetApplicationWebConfig();

            try
            {
                cfgDoc.Load(docName);
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException("No se pudo cargar el archivo de configuracion");
            }
			return cfgDoc;
		}

		public static string GetApplicationWebConfig()
		{   
			string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
			if(APP_PATH == "/") 
				APP_PATH = "/";
			else if(!APP_PATH.EndsWith(@"/")) 
				APP_PATH += @"/";

			string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
			if(!it.EndsWith(@"\"))
				it += @"\";

			return it + "web.config";
		}

		public static string GetApplicationRootDir()
		{   
			string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
			if(APP_PATH == "/") 
				APP_PATH = "/";
			else if(!APP_PATH.EndsWith(@"/")) 
				APP_PATH += @"/";

			string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
			if(!it.EndsWith(@"\"))
				it += @"\";

			return it;
		}

	}
}
