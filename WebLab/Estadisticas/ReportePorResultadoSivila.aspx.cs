using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using System.Data.SqlClient;
using Business;
using System.Text;
using System.IO;
using Business.Data;
using CrystalDecisions.Shared;
using Business.Data.Laboratorio;
using NHibernate;

namespace WebLab.Estadisticas
{
    public partial class ReportePorResultadoSivila : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Exportacion para SIVILA");
                ddlAnio14.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1422.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1858.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1859.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1909.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1927.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1928.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1929.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1930.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1931.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1932.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1933.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1934.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1935.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1936.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1937.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio1938.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio4.SelectedValue = DateTime.Now.Year.ToString();
                ddlAnio_33_1321.SelectedValue = DateTime.Now.Year.ToString();
                MostrarDatos();
                //guardar los codigos asociados a sivila: tabla: grupo, subgrupo, etiologia, codigosil
            }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                        case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void MostrarDatos()
        {
            TextBox txt; 
            Label lbl;
            RelSivilaSil oItem = new RelSivilaSil();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(RelSivilaSil));
          

            IList items = crit.List();
            foreach (RelSivilaSil oCodigo in items)
            {
                string txt1="txtCodigo_" + oCodigo.IdSivilaGrupo.ToString() + "_" + oCodigo.IdSivilaEtiologia.ToString();
                string txt2 = "txtSemana_" + oCodigo.IdSivilaGrupo.ToString() + "_" + oCodigo.IdSivilaEtiologia.ToString();              
                string lbl1 = "lblCantidad_" + oCodigo.IdSivilaGrupo.ToString() + "_" + oCodigo.IdSivilaEtiologia.ToString();

                 if (Page.Master != null)
            {
                foreach (Control control in Page.Master.Controls)
                {
                    if (control is HtmlForm)
                    {
                        foreach (Control controlform in control.Controls)
                        {
                            if (controlform is ContentPlaceHolder)
                            {
                                foreach (Control control2 in controlform.Controls)
                                {                                                                        

                                    if (control2 is Label)
                                    {
                                        lbl = (Label)control2;
                                        if (lbl.ID == lbl1) lbl.Text = oCodigo.CantidadRegistros.ToString();
                                    }
                                                                    if (control2 is TextBox)
                                                                    {
                                                                        txt = (TextBox)control2;
                                                                        if (txt.ID == txt1) txt.Text = oCodigo.CodigoSil;
                                                                        if (txt.ID == txt2) txt.Text = oCodigo.Semana;
                                                                    }
                                                                }
                                     
                                }
                            }
                        }
                    }
                }
                 
                
            }


          

        }
               
        



        protected void imgExcel_Click(object sender, ImageClickEventArgs e)
        {

            //ExportarExcel();

        }

        
        protected void btnDescargar4_Click(object sender, EventArgs e)
        {
            //vih-hombres: 
         
            int anio = int.Parse(ddlAnio4.SelectedValue);
            string semana = listarSemanas( txtSemana_4_1353.Text);
            string codigo = txtCodigo_4_1353.Text;
            string sexo = "M"; //Sexo: Masculino, Femenino, Embrazadas
            DataTable dt = GetDatosSivila(4, 0, 1353, anio, semana, codigo, sexo, txtSemana_4_1353.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt,"VIHhombres");
        }

        public string listarSemanas(string sem)
        {
            string res="";
            string[] arr = sem.Split(("-").ToCharArray());
            if (arr.Count()> 1)
            {
                try
                {
                    string desde = arr[0].ToString();
                    string[] arrDesde = desde.Split((",").ToCharArray());
                    int aux1 = arrDesde.Count() - 1;
                    int desde1 = int.Parse(arrDesde[aux1].ToString());
                    ////
                    res = res + desde;
                    //int desde =int.Parse( arr[0].ToString());

                    string hasta = arr[1].ToString();
                    string[] arrHasta = hasta.Split((",").ToCharArray());
                    //int aux2 = arrHasta.Count() - 1;
                    int hasta1 = int.Parse(arrHasta[0].ToString());

                    for (int i = desde1; i <= hasta1; i++)
                    {
                        if (res == "")
                            res = i.ToString();
                        else
                            res = res + "," + i.ToString();
                    }

                    res = res + "," + hasta;
                }
                catch
                { }
            }
            else res = sem;

            return res;
        }

        //public string listarCodigos(string cod)
        //{
        //    string res="";
        //    string[] arr = cod.Split((",").ToCharArray());
           
        //        for (int i = 0; i < arr.Count(); i++)
        //        {
        //            if (res == "")
        //                res = "'" + arr[i].ToString() + "'";
        //            else
        //                res = res + "," + "'" + arr[i].ToString() + "'";
        //        }
            
        //    return res;
        //}

       

        protected void btnDescargar14_Click(object sender, EventArgs e)
        {   //vih-mujeres: 

            int anio = int.Parse(ddlAnio14.SelectedValue);
            string semana =listarSemanas( txtSemana_14_1353.Text);
            
            string codigo =txtCodigo_14_1353.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(14, 0, 1353, anio, semana, codigo, sexo, txtSemana_14_1353.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt,"VIHmujeres");


        }

        private DataTable GetDatosSivila(int grupo, int subgrupo, int etiologia, int anio, string semana, string codigo, string sexo, string semanaOriginal)
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_EstadisticasSivila]";

            
            int id_establecimiento =int.Parse( ConfigurationManager.AppSettings["codigoEfectorSivila"].ToString());
            string usuario_sivila = ConfigurationManager.AppSettings["usuarioSivila"].ToString();



            /*
             * store nuevo: parametros int grupo, int subgrupo, int etiologia, int anio, string semana, string codigo, string sexo, int id_establecimiento, string usuario_sivila
             */
             
            cmd.Parameters.Add("@grupo", SqlDbType.Int);  cmd.Parameters["@grupo"].Value = grupo;
            cmd.Parameters.Add("@subgrupo", SqlDbType.Int);  cmd.Parameters["@subgrupo"].Value = subgrupo;
            cmd.Parameters.Add("@etiologia", SqlDbType.Int);  cmd.Parameters["@etiologia"].Value = etiologia;
            cmd.Parameters.Add("@anio", SqlDbType.Int);  cmd.Parameters["@anio"].Value = anio;
            cmd.Parameters.Add("@semana", SqlDbType.NVarChar); cmd.Parameters["@semana"].Value =semana;
            cmd.Parameters.Add("@codigo", SqlDbType.NVarChar); cmd.Parameters["@codigo"].Value = codigo;
            cmd.Parameters.Add("@sexo", SqlDbType.NVarChar); cmd.Parameters["@sexo"].Value = sexo;
            cmd.Parameters.Add("@id_establecimiento", SqlDbType.Int); cmd.Parameters["@id_establecimiento"].Value = id_establecimiento;
            cmd.Parameters.Add("@usuario_sivila", SqlDbType.NVarChar); cmd.Parameters["@usuario_sivila"].Value = usuario_sivila;

             cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            GuardarRelacionSilSivila(grupo, etiologia, codigo, semanaOriginal, Ds.Tables[0].Rows.Count);
            return Ds.Tables[0];
                    

        }

        private void GuardarRelacionSilSivila(int grupo, int etiologia, string codigo, string semana, int cantidad)
        {
            RelSivilaSil oConfiguracion = new RelSivilaSil();
            oConfiguracion = (RelSivilaSil)oConfiguracion.Get(typeof(RelSivilaSil), "IdSivilaGrupo", grupo, "IdSivilaEtiologia", etiologia);
            if ( oConfiguracion!=null)
            oConfiguracion.Delete();

            RelSivilaSil oRegistro = new RelSivilaSil();
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            oRegistro.IdSivilaGrupo = grupo;
            oRegistro.IdSivilaEtiologia = etiologia;
            oRegistro.CodigoSil = codigo;
            oRegistro.Semana = semana;
            oRegistro.CantidadRegistros = cantidad;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();    
        }


        private void GenerarArchivo(DataTable dt, string nombrearchivo)
        {
            string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";
          int cantidadRegistros=dt.Rows.Count;
            if (Directory.Exists(directorio))
            {
                string archivo = directorio + "\\" + nombrearchivo + ".txt"; /// probablemente tiene que ser extension .ana (renombrar)

                using (StreamWriter sw = new StreamWriter(archivo))
                {
                    string linea = "";
                    for (int i = 0; i < cantidadRegistros; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (linea == "")
                                linea = dt.Rows[i][j].ToString();
                            else
                                linea += "," + dt.Rows[i][j].ToString();
                        }
                        sw.Write(linea);
                        sw.Write("\r\n");//retorno de carro y avance de linea
                        linea = "";
                    }


                }              
                DescargarArchivo(archivo);
               
            }
        }

        private void DescargarArchivo(string archivo)        {

            System.IO.FileInfo toDownload = new System.IO.FileInfo(archivo);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
            HttpContext.Current.Response.AddHeader("Content-Length", toDownload.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(archivo);
            HttpContext.Current.Response.End();
            ////////////////////////////////////////////////////////////////////////////
        }

        protected void btnDescargar1858_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1858.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1858.Text);

            string codigo =  txtCodigo_18_1858.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1858, anio, semana, codigo, sexo, txtSemana_18_1858.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_rubeolaig");
        }

        protected void btnDescargar1859_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1859.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1859.Text);

            string codigo = txtCodigo_18_1859.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1859, anio, semana, codigo, sexo, txtSemana_18_1859.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_estreptococo");
        }

        protected void btnDescargar1422_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1422.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1422.Text);

            string codigo = txtCodigo_18_1422.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1422, anio, semana, codigo, sexo, txtSemana_18_1422.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_estreptococo");
        }

        protected void btnDescargar1909_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1909.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1909.Text);

            string codigo = txtCodigo_18_1909.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1909, anio, semana, codigo, sexo, txtSemana_18_1909.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_chagas");
        }

        protected void btnDescargar1927_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1927.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1927.Text);

            string codigo = txtCodigo_18_1927.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1927, anio, semana, codigo, sexo, txtSemana_18_1927.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_sifilis_1927");
        }

        protected void btnDescargar1928_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1928.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1928.Text);

            string codigo = txtCodigo_18_1928.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1928, anio, semana, codigo, sexo, txtSemana_18_1928.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_sifilis_1928");

        }

        protected void btnDescargar1929_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1929.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1929.Text);

            string codigo = txtCodigo_18_1929.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1929, anio, semana, codigo, sexo, txtSemana_18_1929.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_vih_1929");
        }

        protected void btnDescargar1930_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1930.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1930.Text);

            string codigo = txtCodigo_18_1930.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1930, anio, semana, codigo, sexo, txtSemana_18_1930.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_hepatitis_1930");

        }

        protected void btnDescargar1931_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1931.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1931.Text);

            string codigo = txtCodigo_18_1931.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1931, anio, semana, codigo, sexo, txtSemana_18_1931.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_toxo_igg");

        }

        protected void btnDescargar1932_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1932.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1932.Text);

            string codigo = txtCodigo_18_1932.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1932, anio, semana, codigo, sexo, txtSemana_18_1932.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_toxo_igm");


        }

        protected void btnDescargar1933_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1933.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1933.Text);

            string codigo = txtCodigo_18_1933.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1933, anio, semana, codigo, sexo, txtSemana_18_1933.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_hiv_tamizaje");
        }

        protected void btnDescargar1936_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1936.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1936.Text);

            string codigo = txtCodigo_18_1936.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1936, anio, semana, codigo, sexo, txtSemana_18_1936.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_hiv_test_parto");

        }

        protected void btnDescargar1934_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1934.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1934.Text);

            string codigo = txtCodigo_18_1934.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1934, anio, semana, codigo, sexo, txtSemana_18_1934.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_hiv_test_rapido");

        }

        protected void btnDescargar1935_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1935.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1935.Text);

            string codigo = txtCodigo_18_1935.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1935, anio, semana, codigo, sexo, txtSemana_18_1935.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_hiv_tamizaje_parto");


        }

        protected void btnDescargar1937_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1937.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1937.Text);

            string codigo = txtCodigo_18_1937.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1937, anio, semana, codigo, sexo, txtSemana_18_1937.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_hiv_confirm_parto");
        }

        protected void btnDescargar1938_Click(object sender, EventArgs e)
        {
            int anio = int.Parse(ddlAnio1938.SelectedValue);
            string semana = listarSemanas(txtSemana_18_1938.Text);

            string codigo = txtCodigo_18_1938.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(18, 6, 1938, anio, semana, codigo, sexo, txtSemana_18_1938.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "Embarazadas_brucelosis");

        }

        protected void btnDescargar1321_Click(object sender, EventArgs e)
        {
            
            int anio = int.Parse(ddlAnio_33_1321.SelectedValue);
            string semana = listarSemanas(txtSemana_33_1321.Text);

            string codigo = txtCodigo_33_1321.Text;
            string sexo = "F"; //Sexo: Masculino, Femenino, Embarazadas
            DataTable dt = GetDatosSivila(33, 0, 1321, anio, semana, codigo, sexo, txtSemana_33_1321.Text);
            //dt convertir en archivo cvs y se descarga
            GenerarArchivo(dt, "chagas");
        }


    }
}
