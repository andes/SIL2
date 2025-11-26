using System;
using System.Security.Cryptography;
using System.Configuration;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;
//using Sql.Data

using System.Security.Cryptography;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;

namespace Business
{
	/// <summary>
	/// Summary description for Utility.
	/// </summary>
	public class Utility
	{
		private PDSAEncryptionType mbytEncryptionType;
		private string mstrOriginalString;
		private string mstrEncryptedString;
		private SymmetricAlgorithm mCSP;

		public enum PDSAEncryptionType : byte
		{
			DES,
			RC2,
			Rijndael,
			TripleDES
		}

		#region " Constructores "

		public Utility()
		{
			mbytEncryptionType = PDSAEncryptionType.DES;

			this.SetEncryptor();
		}


        public string EncryptarNet(string Data, string Password, int Bits)
        {
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(Data);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x0, 0x1, 0x2, 0x1C, 0x1D, 0x1E, 0x3, 0x4, 0x5, 0xF, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });
            if (Bits == 128)
            {
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (Bits == 192)
            {
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (Bits == 256)
            {
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else
            {
                return String.Concat(Bits);
            }

        }

        public   string quitaCerosIzquierda(string valor)
        {
            for (int j = 0; j < valor.Length; j++)
            {
                if (valor.Substring(j, 1) != "0")
                {
                    valor = valor.Substring(j, valor.Length - j);
                    break;
                }
            }
            return valor;
        }
        private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }
        private byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }
        public string DecryptarNet(string Data, string Password, int Bits)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(Data);
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password, new byte[] { 0x0, 0x1, 0x2, 0x1C, 0x1D, 0x1E, 0x3, 0x4, 0x5, 0xF, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });
                if (Bits == 128)
                {
                    byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                    return System.Text.Encoding.Unicode.GetString(decryptedData);
                }
                else if (Bits == 192)
                {
                    byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                    return System.Text.Encoding.Unicode.GetString(decryptedData);
                }
                else if (Bits == 256)
                {
                    byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                    return System.Text.Encoding.Unicode.GetString(decryptedData);
                }
                else
                {
                    return String.Concat(Bits);
                }
            }
            catch (Exception ex)
            {
                return String.Concat(Bits);
            }
        }

        public Utility(PDSAEncryptionType EncryptionType)
		{
			mbytEncryptionType = EncryptionType;

			this.SetEncryptor();
		}
        public string CompletarNombrePDF(string v)
        {
            return v + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
        }
       
        public Utility(PDSAEncryptionType EncryptionType, string OriginalString)
		{
			mbytEncryptionType = EncryptionType;
			mstrOriginalString = OriginalString;

			this.SetEncryptor();
		}

		#endregion

		#region " Metodos Anteriores "
        
		public  string EncryptAnterior(string strPass)
		{
			long lngChr;
			string strBuff="";
			string strInput = ConfigurationSettings.AppSettings["encKey"];
			strPass = strPass.ToUpper();

			if(strPass.Length != 0) 
			{
				for(int intCnt=0;intCnt<strInput.Length;intCnt++)
				{
					int intStart = intCnt % strPass.Length; lngChr = Convert.ToInt64(strInput.Substring(intCnt,1)[0]); // [0] for to avoid "" from the input, e.g., "S" ==> 'S'
					// To avoid index is not found exception
					if(intStart == (strPass.Length -1))
						lngChr = lngChr + Convert.ToInt64(strPass.Substring(intStart,1)[0]); // [0] for to avoid "" from the input, e.g., "S" ==> 'S'
					else
						lngChr = lngChr + Convert.ToInt64(strPass.Substring(intStart+1,1)[0]); // [0] for to avoid "" from the input, e.g., "S" ==> 'S' 

					strBuff = strBuff + (char)(lngChr & Convert.ToInt64("0xFF", 16));// AND wif 0xFF
				}
			}
			else
				strBuff = strInput;

			return strBuff;
		}

        public object getParametro(string val, string para)
        {
            string par = "";
          
          
            string[] arr = val.Split(("&").ToCharArray());
            foreach (string item in arr)
            {
                string[] s_control = item.Split(("=").ToCharArray());

                if (s_control[0].ToString() == para)
                    par = s_control[1].ToString(); break;

            }
            return par;
        }
        public  string DecryptAnterior(string strPass) 
		{
			string strBuff="";
			string strInput = ConfigurationSettings.AppSettings["encKey"];
			long lngChr; strPass = strPass.ToUpper();
			if(strPass.Length != 0)
			{
				for(int intCnt=0;intCnt<strInput.Length;intCnt++)
				{ 
					int intStart = intCnt % strPass.Length;

					lngChr = Convert.ToInt64(strInput.Substring(intCnt,1)[0]);// [0] for to avoid "" from the input, e.g., "S" ==> 'S'

					// To avoid index is not found exception
					if(intStart == (strPass.Length -1))
						lngChr = lngChr - Convert.ToInt64(strPass.Substring(intStart,1)[0]);// [0] for to avoid "" from the input, e.g., "S" ==> 'S'
					else
						lngChr = lngChr - Convert.ToInt64(strPass.Substring(intStart+1,1)[0]); // [0] for to avoid "" from the input, e.g., "S" ==> 'S' 

					strBuff = strBuff + (char)(lngChr & Convert.ToInt64("0xFF", 16));// AND wif 0xFF

				}

			}
			else

				strBuff = strInput;

			return strBuff; 
		}

      

        #endregion

        #region " Propiedades Publicas "

        public PDSAEncryptionType EncryptionType
		{
			get {return mbytEncryptionType;}
			set
			{
				if (mbytEncryptionType != value) 
				{
					mbytEncryptionType = value;
					mstrOriginalString = String.Empty;
					mstrEncryptedString = String.Empty;

					this.SetEncryptor();
				}
			}
		}

		public SymmetricAlgorithm CryptoProvider
		{
			get {return mCSP;}
			set {mCSP = value;}
		}

		public string OriginalString
		{
			get {return mstrOriginalString;}
			set {mstrOriginalString = value;}
		}

		public string EncryptedString
		{
			get {return mstrEncryptedString;}
			set {mstrEncryptedString = value;}
		}

		public byte[] key
		{
			get {return mCSP.Key;}
			set {mCSP.Key = value;}
		}

		public string KeyString
		{
			get {return Convert.ToBase64String(mCSP.Key);}
			set {mCSP.Key = Encoding.UTF8.GetBytes(value);}
		}

		public byte[] IV
		{
			get {return mCSP.IV;}
			set {mCSP.IV = value;}
		}

		public string IVString
		{
			get {return Convert.ToBase64String(mCSP.IV);}
			set {mCSP.IV = Encoding.UTF8.GetBytes(value);}
		}

		#endregion

		#region " Metodos de Encriptacion "

		public string Encrypt()
		{
			ICryptoTransform ct;
			MemoryStream ms;
			CryptoStream cs;
			byte[] byt;

			ct = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

			byt = Encoding.UTF8.GetBytes(mstrOriginalString);

			ms = new MemoryStream();
			cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
			cs.Write(byt, 0, byt.Length);
			cs.FlushFinalBlock();
			cs.Close();

			mstrEncryptedString = Convert.ToBase64String(ms.ToArray());

			return mstrEncryptedString;
		}

		public string Encrypt(string OriginalString)
		{
			mstrOriginalString = OriginalString;
      
			return this.Encrypt();
		}

		public string Encrypt(string OriginalString, PDSAEncryptionType EncryptionType)
		{
			mstrOriginalString = OriginalString;
			mbytEncryptionType = EncryptionType;

			return this.Encrypt();
		}

		#endregion

		#region " Metodos de Desencriptacion "

		public string Decrypt()
		{
			ICryptoTransform ct;
			MemoryStream ms;
			CryptoStream cs;
			byte[] byt;

			ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

			byt = Convert.FromBase64String(mstrEncryptedString);

			ms = new MemoryStream();
			cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
			cs.Write(byt, 0, byt.Length);
			cs.FlushFinalBlock();
			cs.Close();

			mstrOriginalString = Encoding.UTF8.GetString(ms.ToArray());

			return mstrOriginalString;
		}

		public string Decrypt(string EncryptedString)
		{
			mstrEncryptedString = EncryptedString;
           // return mstrEncryptedString;
			return this.Decrypt();
		}

		public string Decrypt(string EncryptedString, PDSAEncryptionType EncryptionType)
		{
			mstrEncryptedString = EncryptedString;
			mbytEncryptionType = EncryptionType;

			return this.Decrypt();
		}

		#endregion

		#region " Metodo SetEncryptor() "

		private void SetEncryptor()
		{
			switch(mbytEncryptionType)
			{
				case PDSAEncryptionType.DES:
					mCSP = new DESCryptoServiceProvider();
					break;
				case PDSAEncryptionType.RC2:
					mCSP = new RC2CryptoServiceProvider();
					break;
				case PDSAEncryptionType.Rijndael:
					mCSP = new RijndaelManaged();
					break;
				case PDSAEncryptionType.TripleDES:
					mCSP = new TripleDESCryptoServiceProvider();
					break;
			}
      
			// Generate Key
			this.GenerateKey();

			// Generate IV
			this.GenerateIV();
		}
		#endregion

		#region " Metodos Publicos Varios "

		public byte[] GenerateKey()
		{
			try
			{
				int x = 0;
				char[] chars = ConfigurationSettings.AppSettings["encKey"].ToCharArray();
				byte[] bits = new byte[chars.Length];
				foreach (char c in chars)
				{
					bits[x] = Convert.ToByte(c);
					x += 1;
				}
										  
				mCSP.Key = bits;

				return mCSP.Key;
			}
			catch (Exception ex)
			{
				if (ex.GetType().Name.Equals("ArgumentException"))
					throw new Exception("La clave encKey debe tener 8 digitos"); 
				else
					throw new Exception("Clave encKey en Archivo de Configuracion no implementada"); 
			}
		}

		public byte[] GenerateIV()
		{
			try
			{
				int x = 0;
				char[] chars = ConfigurationSettings.AppSettings["encKey"].ToCharArray();
				byte[] bits = new byte[chars.Length];
				foreach (char c in chars)
				{
					bits[x] = Convert.ToByte(c);
					x += 1;
				}
										  
				mCSP.IV = bits;

				return mCSP.IV;
			}
			catch (Exception ex)
			{
				if (ex.GetType().Name.Equals("ArgumentException"))
					throw new Exception("La clave encKey debe tener 8 digitos"); 
				else
					throw new Exception("Clave encKey en Archivo de Configuracion no implementada"); 
			}
		}

		#endregion

        #region " Metodos Carga Componentes "


      

        public void CargarCombo(DropDownList Combo, String strSql, String CampoId, String CampoDetalle)
        {
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            Combo.DataTextField = CampoDetalle;
            Combo.DataValueField = CampoId;
            Combo.DataSource = ds.Tables["t"];
            Combo.DataBind();
            da.Dispose();
            ds.Dispose();
         
        }

        public void CargarCombo(DropDownList Combo, String strSql, String CampoId, String CampoDetalle, string strconn)
        {
            //NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            //String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            Combo.DataTextField = CampoDetalle;
            Combo.DataValueField = CampoId;
            Combo.DataSource = ds.Tables["t"];
            Combo.DataBind();
            da.Dispose();
            ds.Dispose();

        }

        public void CargarCombo(DropDownList Combo, IList lista, String CampoId, String CampoDetalle)
        {
            Combo.DataTextField = CampoDetalle;
            Combo.DataValueField = CampoId;
            Combo.DataSource = lista;
            Combo.DataBind();           
        }

        public void CargarCheckBox(CheckBoxList Checks, String strSql, String CampoId, String CampoDetalle)
        {
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            Checks.DataTextField = CampoDetalle;
            Checks.DataValueField = CampoId;
            Checks.DataSource = ds.Tables["t"];
            Checks.DataBind();

        }

        public void CargarCheckBox(CheckBoxList Checks, String strSql, String CampoId, String CampoDetalle, string strconn)
        { 
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            Checks.DataTextField = CampoDetalle;
            Checks.DataValueField = CampoId;
            Checks.DataSource = ds.Tables["t"];
            Checks.DataBind();

        }
        public void CargarListBox(ListBox Lista, String strSql, String CampoId, String CampoDetalle,  string strconn)
        {
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
          
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            Lista.DataTextField = CampoDetalle;
            Lista.DataValueField = CampoId;
            Lista.DataSource = ds.Tables["t"];
            Lista.DataBind();

        }

        public void CargarListBox(ListBox Lista, String strSql, String CampoId, String CampoDetalle )
        {
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            Lista.DataTextField = CampoDetalle;
            Lista.DataValueField = CampoId;
            Lista.DataSource = ds.Tables["t"];
            Lista.DataBind();

        }


        #endregion

        public SqlDataReader getDataReader(String strSql)
        {
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strConn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlConnection myConnection = new SqlConnection(strConn);
            SqlCommand myCommand = new SqlCommand(strSql, myConnection);
            myConnection.Open();
            SqlDataReader dr = myCommand.ExecuteReader();
            return dr;
        }
           

        public DataTable getDataSet(String strSql, bool conColu)
        {       
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da2 = new SqlDataAdapter(strSql, strconn);
            DataSet ds2 = new DataSet();            
            da2.Fill(ds2, "t");
            if (conColu == true)
            {
                DataColumn vModifica = new DataColumn();
                vModifica.DefaultValue = "";
                DataColumn vElimina = new DataColumn();
                vElimina.DefaultValue = "";
                ds2.Tables["t"].Columns.Add(vModifica);
                ds2.Tables["t"].Columns.Add(vElimina);
            }
            return ds2.Tables[0];
        }
        #region " Otros Métodos "

        public bool EsNumerico(string val)
        {
            bool match;
            //regula expression to match numeric values
            string pattern = "(^[-+]?\\d+(,?\\d*)*\\.?\\d*([Ee][-+]\\d*)?$)|(^[-+]?\\d?(,?\\d*)*\\.\\d+([Ee][-+]\\d*)?$)";
            //generate new Regulsr Exoression eith the pattern and a couple RegExOptions
            Regex regEx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            //tereny expresson to see if we have a match or not
            match = regEx.Match(val).Success ? true : false;
            //return the match value (true or false)
            return match;
        }
        public bool EsEntero(string val)
        {
            bool match;
            //regula expression to match numeric values
            string pattern = "(^\\d*$)";
            //generate new Regulsr Exoression eith the pattern and a couple RegExOptions
            Regex regEx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            //tereny expresson to see if we have a match or not
            match = regEx.Match(val).Success ? true : false;
            //return the match value (true or false)
            return match;
        }   

        public string SacaComillas(string cadena)
        {
            string Caracter;
            string Comillas = "''";
            string Blanco = "";
            string cadenaLimpia = cadena.Replace(Comillas, Blanco);
            Comillas = "'";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Comillas, Blanco);
            Caracter = "*";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = "/";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = "\\";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = ":";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = "?";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = "<";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = ">";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);
            Caracter = "|";
            Blanco = "";
            cadenaLimpia = cadenaLimpia.Replace(Caracter, Blanco);

            cadenaLimpia = RemoverSignosAcentos(cadenaLimpia);

            return cadenaLimpia;
        }

        public int VerificaPermisos(ArrayList lista, string m_Objeto)
        {
            int per = 0;          
            foreach (string item in lista)
            {
                string[] arr = item.Split((":").ToCharArray());
                if (arr[0] == m_Objeto)
                {
                    per = int.Parse(arr[1]);
                    break;
                }
            }
            return per;
        }


        /// con ñ
        //private const string ConSignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇ";
        //private const string SinSignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUcC";

        /// sin ñ
        private const string ConSignos = "áàäéèëíìïóòöúùuÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇ±";
        private const string SinSignos = "aaaeeeiiiooouuuAAAEEEIIIOOOUUUcCn";



        public  string RemoverSignosAcentos(string texto)
        {
            var textoSinAcentos = string.Empty;

            foreach (var caracter in texto)
            {
                var indexConAcento = ConSignos.IndexOf(caracter);
                if (indexConAcento > -1)
                    textoSinAcentos = textoSinAcentos + (SinSignos.Substring(indexConAcento, 1));
                else
                    textoSinAcentos = textoSinAcentos + (caracter);
            }
            return textoSinAcentos;
        }


        public bool bisiesto(int anno)
  {
      bool resultado;
      //Comprobamos la regla general.
      //Si anno es divisible por 4, es decir, si el
      //resto de la división entre 4 es 0...
      if (anno % 4 == 0)
      {
          //Si es divisible por 4, ahora toca comprobar
          //la excepción
          if (
               (anno % 100 == 0) &&  //Si es divisible por 100
               (anno % 400 != 0)     //y no por 400
             )
          {
              resultado = false; //entonces no es bisiesto
          }
          else
          {
              resultado = true; //No cumple la excepción.
              //Lo dejamos como bisiesto por ser divisible por 4
          }
      }
      else //Si no cumple la regla general
      {
          //No es bisiesto.
          resultado = false;
      }
      return resultado;
} 
        public string Formato(string p)
        {
            string aux = "";
            switch (p)
            {
                case "0": aux = "0"; break;
                case "1": aux = "0.0"; break;
                case "2": aux = "0.00"; break;
                case "3": aux = "0.000"; break;
                case "4": aux = "0.0000"; break;
            }
            return aux;
        }

        public string ExpresionFormato(int p)
        {
            string expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,2}";
            switch (p)
            {
                case 0:
                    {
                        expresionControlDecimales = "[-+]?\\d+";
                    }
                    break;
                case 1:
                    {
                        expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,1}";

                    }
                    break;
                case 2:
                    {
                        expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,2}";

                    }
                    break;
                case 3:
                    {
                        expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,3}";

                    }
                    break;
                case 4:
                    {
                        expresionControlDecimales = "[-+]?\\d+\\.?\\,?\\d{0,4}";

                    }
                    break;
            }
            return expresionControlDecimales;
        }
        public string DiferenciaFechas(DateTime dn, DateTime fechaProtocolo)
        { ///calculo de fechas teniendo el cuenta los dias de los meses            

            DateTime da = fechaProtocolo; // DateTime.Now;
            int  anos =  da.Year - dn.Year; // calculamos años 
            int meses = da.Month - dn.Month; // calculamos meses 
            int dias =  da.Day - dn.Day; // calculamos días 

            //ajuste de posible negativo en $días 
            if (dias < 0) 
            { 
                //--$meses; 
                int dias_mes_anterior=0;
                //ahora hay que sumar a $dias los dias que tiene el mes anterior de la fecha actual 
                switch (da.Month) { 
                case 1:     dias_mes_anterior=31; break; 
                case 2:     dias_mes_anterior=31; break; 
                case 3:  
                    if (bisiesto(da.Year )) 
                    { 
                        dias_mes_anterior=29; break; 
                    } else { 
                        dias_mes_anterior=28; break; 
                    } 
                case 4:     dias_mes_anterior=31; break; 
                case 5:     dias_mes_anterior=30; break; 
                case 6:     dias_mes_anterior=31; break; 
                case 7:     dias_mes_anterior=30; break; 
                case 8:     dias_mes_anterior=31; break; 
                case 9:     dias_mes_anterior=31; break; 
                case 10:    dias_mes_anterior=30; break; 
                case 11:    dias_mes_anterior=31; break; 
                case 12:    dias_mes_anterior=30; break; 
                } 
                dias=dias + dias_mes_anterior;
                meses = meses - 1;
            }
           
             
            //ajuste de posible negativo en $meses 
            if (meses < 0)
            {   //--$anos; 
                meses = meses + 12;
            }
            
            string edad = "1;D";
            if (anos > 0)
            {
                if ((da.Month < dn.Month)&&(anos==1))
                {
                    if (meses > 0) edad = meses.ToString() + ";M";
                    else
                        if (dias > 0) edad = dias.ToString() + ";D";
                }
                else
                {
                    if (da.Month < dn.Month) anos=anos-1;
                    edad = anos.ToString() + ";A";
                }
            }
            else
            {
                if (meses> 0) edad = meses.ToString() + ";M";
                else
                    if (dias > 0) edad = dias.ToString() + ";D";
            }
            return edad;
        }

        public void CargarRadioButton(RadioButtonList buttons, String strSql, String CampoId, String CampoDetalle)
        {
            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
            SqlDataAdapter da = new SqlDataAdapter(strSql, strconn);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            buttons.DataTextField = CampoDetalle;
            buttons.DataValueField = CampoId;
            buttons.DataSource = ds.Tables["t"];
            buttons.DataBind();
        }



        #endregion

        #region Excel

        public static void ExportDataTableToXlsx(DataTable dataTable, string filename)
        {
            // ⚠️ Si usas EPPlus v5.x o superior, descomenta esta línea:
            // OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                HttpContext.Current.Response.Write("No hay datos para exportar.");
                HttpContext.Current.Response.End();
                return;
            }

            HttpResponse response = HttpContext.Current.Response;

            // Define el color de fondo por defecto si no se proporciona
            //    --azul-neuquen: #2b3e4c;
            Color finalBackColor = ColorTranslator.FromHtml("#2b3e4c"); //azul-neuquen

            using (ExcelPackage package = new ExcelPackage())
            {
                // Crear una nueva hoja de trabajo
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(filename);

                // Cargar la DataTable en la hoja de trabajo. 'true' incluye los encabezados.
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                // --- FORMATEAR COLUMNAS FECHA ---
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    if (dataTable.Columns[col].DataType == typeof(DateTime))
                    {
                        // EPPlus usa 1-based index → sumar 1
                        int excelCol = col + 1;

                        // aplicar formato "dd/MM/yyyy"
                        worksheet.Column(excelCol).Style.Numberformat.Format = "dd/MM/yyyy";

                        // opcional: si querés fecha+hora
                        // worksheet.Column(excelCol).Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                    }
                }

                // --- APLICAR ESTILO AL ENCABEZADO ---
                int rowCount = dataTable.Rows.Count;
                int colCount = dataTable.Columns.Count;

                // Rango del encabezado: Desde A1 hasta el final de la primera fila
                using (var range = worksheet.Cells[1, 1, 1, colCount])
                {
                    ApplyHeaderStyle(range, finalBackColor);
                }

                // Autoajusta las columnas
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // --- CONFIGURAR RESPUESTA HTTP ---
                response.Clear();
                response.Buffer = true;
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                string fullFilename = filename.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ? filename : filename + ".xlsx";
                response.AddHeader("Content-Disposition", $"attachment; filename=\"{fullFilename}\"");

                // Escribe el paquete de Excel directamente al flujo de salida
                package.SaveAs(response.OutputStream);

                response.Flush();
                response.End();
            }
        }
       
        private static void ApplyHeaderStyle(ExcelRange range, Color backColor)
        {
            range.Style.Font.Bold = true;
            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(backColor);
            range.Style.Font.Color.SetColor(Color.White); // Color de fuente blanco (opcional)
        }

        public static void ExportGridViewToExcel(GridView grid, string nombreArchivo)
        {
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add(nombreArchivo);

                int fila = 1;
                int col = 1;

                // ================================
                // 1) Escribir encabezados
                // ================================

               
                foreach (DataControlField column in grid.Columns)
                {

                    Color encabezado = grid.HeaderStyle.BackColor;
                    Color fontColor = grid.HeaderStyle.ForeColor;

                    ws.Cells[fila, col].Value = column.HeaderText;

                    // Formato encabezado
                    //Color backgroundColor =  ColorTranslator.FromHtml("#2b3e4c"); //Azul Neuquen
                    ws.Cells[fila, col].Style.Font.Size = 9;
                    ws.Cells[fila, col].Style.Font.Bold = true;
                    ws.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(encabezado);
                    ws.Cells[fila, col].Style.Font.Color.SetColor(fontColor);
                    ws.Cells[fila, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    col++;
                }

                fila++;

                // ================================
                // 2) Escribir filas + colores del GridView
                // ================================
                foreach (GridViewRow row in grid.Rows)
                {
                    col = 1;

                    foreach (TableCell cell in row.Cells)
                    {
                        // (2) Decodificar texto HTML
                        var texto = HttpUtility.HtmlDecode(cell.Text);
                        
                        // (1) Detectar si es número
                        double numero;
                        bool esNumero = double.TryParse(
                            texto,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out numero
                        );

                        if (esNumero)
                        {
                            ws.Cells[fila, col].Value = numero;
                        }
                        else
                        {
                            ws.Cells[fila, col].Value = texto;
                        }

                        // Aplicar colores si existen
                        if (cell.BackColor != Color.Empty)
                        {
                            ws.Cells[fila, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[fila, col].Style.Fill.BackgroundColor.SetColor(cell.BackColor);
                        }

                        if (cell.ForeColor != Color.Empty)
                        {
                            ws.Cells[fila, col].Style.Font.Color.SetColor(cell.ForeColor);
                        }

                        ws.Cells[fila, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        ws.Cells[fila, col].Style.Font.Size = 9;
                        col++;
                    }

                    fila++;
                }

                // Autoajustar columnas
                ws.Cells[1, 1, fila - 1, grid.Columns.Count].AutoFitColumns();

                // ================================
                // 3) Descargar archivo
                // ================================
                var response = HttpContext.Current.Response;

                response.Clear();
                response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                response.AddHeader("content-disposition", $"attachment; filename={nombreArchivo}.xlsx");

                response.BinaryWrite(package.GetAsByteArray());
                response.End();
            }
        }
        #endregion
    }
}