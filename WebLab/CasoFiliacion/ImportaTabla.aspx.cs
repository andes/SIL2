using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;

namespace WebLab.CasoFiliacion
{
    public partial class ImportaTabla : System.Web.UI.Page
    {
        bool cargar = false;
        string numeroProtocolo= "";
        string resultado = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Interface Fusion");
              
                //CargarListas();
                //CargarGrilla();                   

            }


        }

       
        private void VerificaPermisos(string sObjeto)
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
        protected void subir_Click(object sender, EventArgs e)
        {
            try
            {
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

                    if (Directory.Exists(directorio))
                    {
                        string archivo = directorio + "\\" + trepador.FileName;

                    
                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";

                       
                            ProcesarFichero();
                        
                           // CargarGrillaEspecificidad();
                       
                      
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString()+ " .Comuniquese con el administrador."; }
        }
        private void CargarGrillaEspecificidad()
        {





            string 
                m_strSQL = @"select idMindrayResultado, protocolo as numero, '' as item,
            M.descripcion as apellido, M.unidadmedida as dni, M.valorobtenido as transfusion 
            FROM            LAB_MindrayResultado AS M ";


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            gvListaEspecificidad.DataSource = Ds.Tables[0];
            gvListaEspecificidad.DataBind();
            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            gvListaEspecificidad.Visible = true;
          
            //PintarReferencias();
        }


        private void ProcesarFichero()
        {
            if (this.trepador.HasFile)
            {
                string filename = this.trepador.PostedFile.FileName;
                BorrarResultadosTemporales();
                int i = 1;
                if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                {
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            if (i != 1)
                                
                                    ProcesarLinea(line);
                               
                            i += 1;
                        }
                    }
                }
            }
        }
  
        private void BorrarResultadosTemporales()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(MindrayResultado));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (MindrayResultado oDetalle in detalle)
                {                    
                        oDetalle.Delete();
                }
            }
        }

        private void ProcesarLinea(string linea)
        {
            try
            {  Utility oUtil = new Utility();
                string[] arr = linea.Split(("\t").ToCharArray());
               

                if (arr.Length >= 5)
                {
                   
                    string  campo_cero= arr[0].ToString();
                    string campo_MFI = arr[1].ToString();
                    string campo_Bead= arr[2].ToString();
                    string campo_valor = arr[3].ToString();

                  


                            ///Se reutiliza la tabla MindrayResultado para volcar temporalmente los resultados del luminex
                            MindrayResultado oRegistro = new MindrayResultado();
                            oRegistro.Protocolo = campo_cero.Substring(0,5);
                           
                            oRegistro.FechaProtocolo = DateTime.Parse("01/01/1900");

                            oRegistro.IdItemMindray = 0;
                            oRegistro.Descripcion = campo_MFI;
                            oRegistro.UnidadMedida = campo_Bead;
                            oRegistro.ValorObtenido = campo_valor;
                            oRegistro.TipoValor = "";
                            oRegistro.FechaResultado = DateTime.Now;
                            oRegistro.FechaRegistro = DateTime.Now;
                            oRegistro.Estado = 0;
                            oRegistro.Save();
                       

                }
            }
            catch (Exception ex) {
                string exception = "";                
                   exception = ex.Message + "<br>";
                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema."+ exception;
            }
        }



     

        protected void btnGuardar_Click(object sender, EventArgs e)
        {


            //if (ddlTipoArchivo.SelectedIndex ==                0)
            //{
            //    Guardar1();
                
            //}
            //else

            //    Guardar2();

            

        }

    //    private void Guardar2()
    //    {

    //        ///hacer control de validacion como en guardar1
    //        string positivoFuerte_DR = "";
    //        string positivoFuerte_DQ = "";
    //        string positivoFuerte_DP = "";
    //        string positivoModerado_DR = "";
    //        string positivoModerado_DQ = "";
    //        string positivoModerado_DP = "";
    //        string positivoDebil_DR = "";
    //        string positivoDebil_DQ = "";
    //        string positivoDebil_DP = "";
    //        string negativo_DR = "";
    //        string negativo_DQ = "";
    //        string negativo_DP = "";
    //        int nroProtocolo = 0;
    //        int i = 0;
    //        int cantNegA = 0;
    //        int cantNegb = 0;
    //        int cantNegc = 0;


    //        bool sigue = true;
    //        nroProtocolo = int.Parse(idProtocolo.Value);
    //        //if (i == 0)
    //            sigue = BorrarResultadosLuminex2(nroProtocolo);

    //        if (sigue)
    //        { 
    //             foreach (GridViewRow row in gvListaEspecificidad.Rows)
    //            {
    //                    MindrayResultado oProtocolo = new MindrayResultado();
    //                    oProtocolo = (MindrayResultado)oProtocolo.Get(typeof(MindrayResultado), int.Parse(gvListaEspecificidad.DataKeys[row.RowIndex].Value.ToString()));

    //                    //Protocolo oP = new Protocolo();
    //                    //oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", oProtocolo.Protocolo);
    //                    //idProtocolo = oP.IdProtocolo;

    //                    double valor = double.Parse(oProtocolo.ValorObtenido);
    //                    string MFI = oProtocolo.Descripcion;
    //                    string Bead = oProtocolo.UnidadMedida;

    //                    ///extraer parte DPA

    //                    int startIndex = Bead.IndexOf("DPA");
    //                    int endIndex = Bead.LastIndexOf(",");
    //                    if (startIndex>-1)
    //                    { 
    //                    string parte = Bead.Substring(startIndex, endIndex + 1);
    //                    Bead = Bead.Replace(parte, "");
    //                    }
                  
    //                    i = i + 1;
    //                    if (valor > 7000) // positivo fuerte
    //                    {
    //                        if (MFI.Substring(0, 2) == "DR")
    //                            positivoFuerte_DR += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                        if (MFI.Substring(0, 2) == "DQ")
    //                            positivoFuerte_DQ += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                        if (MFI.Substring(0, 2) == "DP")
    //                            positivoFuerte_DP += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    }

    //                    if ((valor > 3000) && (valor <= 7000))// positivo moderado
    //                    {
    //                        if (MFI.Substring(0, 2) == "DR")
    //                            positivoModerado_DR += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                        if (MFI.Substring(0, 2) == "DQ")
    //                            positivoModerado_DQ += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                        if (MFI.Substring(0, 2) == "DP")
    //                            positivoModerado_DP += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    }

    //                    if ((valor >= 700) && (valor <= 3000))// positivo debil
    //                    {
    //                        if (MFI.Substring(0, 2) == "DR")
    //                            positivoDebil_DR += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                        if (MFI.Substring(0, 2) == "DQ")
    //                            positivoDebil_DQ += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                        if (MFI.Substring(0, 2) == "DP")
    //                            positivoDebil_DP += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    }
    //                    if ((valor < 700) && (valor > 0))// negativo
    //                    {

    //                        if (MFI.Substring(0, 2) == "DR")
    //                        {
    //                            if (cantNegA <= 5)
    //                            {
    //                                negativo_DR += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                                cantNegA += 1;
    //                            }
    //                        }
    //                        if (MFI.Substring(0, 2) == "DQ")
    //                        {
    //                            if (cantNegb <= 5)
    //                            {
    //                                negativo_DQ += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                                cantNegb += 1;
    //                            }
    //                        }
    //                        if (MFI.Substring(0, 2) == "DP")
    //                        {
    //                            if (cantNegc <= 5)
    //                            {
    //                                negativo_DP += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                                cantNegc += 1;
    //                            }
    //                        }


    //                    }
                    


    //            } // foreach

    //        ProtocoloLuminex oRegistro = new ProtocoloLuminex();
    //        oRegistro.IdProtocolo = nroProtocolo;
    //        oRegistro.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro.IdSubitem = "Anti HLA-DR";
    //        oRegistro.TipoValor = "Negativo";
    //        oRegistro.Valor = negativo_DR;
    //        oRegistro.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro.FechaResultado = DateTime.Now;
    //        oRegistro.IdUsuarioValida = 0;
    //     oRegistro.FechaValida=   DateTime.Parse("01/01/1900");

    //        oRegistro.Save();

    //        ProtocoloLuminex oRegistro11 = new ProtocoloLuminex();
    //        oRegistro11.IdProtocolo = nroProtocolo;
    //        oRegistro11.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro11.IdSubitem = "Anti HLA-DR";
    //        oRegistro11.TipoValor = "Positivo Debil";
    //        oRegistro11.Valor = positivoDebil_DR;
    //        oRegistro11.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro11.FechaResultado = DateTime.Now;
    //        oRegistro11.IdUsuarioValida = 0;
    //        oRegistro11.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro11.Save();

    //        ProtocoloLuminex oRegistro111 = new ProtocoloLuminex();
    //        oRegistro111.IdProtocolo = nroProtocolo;
    //        oRegistro111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro111.IdSubitem = "Anti HLA-DR";
    //        oRegistro111.TipoValor = "Positivo Moderado";
    //        oRegistro111.Valor = positivoModerado_DR;
    //        oRegistro111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro111.FechaResultado = DateTime.Now;
    //        oRegistro111.IdUsuarioValida = 0;
    //        oRegistro111.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro111.Save();

    //        ProtocoloLuminex oRegistro1111 = new ProtocoloLuminex();
    //        oRegistro1111.IdProtocolo = nroProtocolo;
    //        oRegistro1111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro1111.IdSubitem = "Anti HLA-DR";
    //        oRegistro1111.TipoValor = "Positivo Fuerte";
    //        oRegistro1111.Valor = positivoFuerte_DR;
    //        oRegistro1111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro1111.FechaResultado = DateTime.Now;
    //        oRegistro1111.IdUsuarioValida = 0;
    //        oRegistro1111.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro1111.Save();

    //        ProtocoloLuminex oRegistro12 = new ProtocoloLuminex();
    //        oRegistro12.IdProtocolo = nroProtocolo;
    //        oRegistro12.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro12.IdSubitem = "Anti HLA-DQ";
    //        oRegistro12.TipoValor = "Negativo";
    //        oRegistro12.Valor = negativo_DQ;
    //        oRegistro12.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro12.FechaResultado = DateTime.Now;
    //        oRegistro12.IdUsuarioValida = 0;
    //        oRegistro12.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro12.Save();

    //        ProtocoloLuminex oRegistro121 = new ProtocoloLuminex();
    //        oRegistro121.IdProtocolo = nroProtocolo;
    //        oRegistro121.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro121.IdSubitem = "Anti HLA-DQ";
    //        oRegistro121.TipoValor = "Positivo Debil";
    //        oRegistro121.Valor = positivoDebil_DQ;
    //        oRegistro121.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro121.FechaResultado = DateTime.Now;
    //        oRegistro121.IdUsuarioValida = 0;
    //        oRegistro121.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro121.Save();

    //        ProtocoloLuminex oRegistro1211 = new ProtocoloLuminex();
    //        oRegistro1211.IdProtocolo = nroProtocolo;
    //        oRegistro1211.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro1211.IdSubitem = "Anti HLA-DQ";
    //        oRegistro1211.TipoValor = "Positivo Moderado";
    //        oRegistro1211.Valor = positivoModerado_DQ;
    //        oRegistro1211.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro1211.FechaResultado = DateTime.Now;
    //        oRegistro1211.IdUsuarioValida = 0;
    //        oRegistro1211.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro1211.Save();


    //        ProtocoloLuminex oRegistro12111 = new ProtocoloLuminex();
    //        oRegistro12111.IdProtocolo = nroProtocolo;
    //        oRegistro12111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro12111.IdSubitem = "Anti HLA-DQ";
    //        oRegistro12111.TipoValor = "Positivo Fuerte";
    //        oRegistro12111.Valor = positivoFuerte_DQ;
    //        oRegistro12111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro12111.FechaResultado = DateTime.Now;
    //        oRegistro12111.IdUsuarioValida = 0;
    //        oRegistro12111.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro12111.Save();

    //        //// 

    //        ProtocoloLuminex oRegistro13 = new ProtocoloLuminex();
    //        oRegistro13.IdProtocolo = nroProtocolo;
    //        oRegistro13.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro13.IdSubitem = "Anti HLA-DP";
    //        oRegistro13.TipoValor = "Negativo";
    //        oRegistro13.Valor = negativo_DP;
    //        oRegistro13.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro13.FechaResultado = DateTime.Now;
    //        oRegistro13.IdUsuarioValida = 0;
    //        oRegistro13.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro13.Save();

    //        ProtocoloLuminex oRegistro131 = new ProtocoloLuminex();
    //        oRegistro131.IdProtocolo = nroProtocolo;
    //        oRegistro131.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro131.IdSubitem = "Anti HLA-DP";
    //        oRegistro131.TipoValor = "Positivo Debil";
    //        oRegistro131.Valor = positivoDebil_DP;
    //        oRegistro131.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro131.FechaResultado = DateTime.Now;
    //        oRegistro131.IdUsuarioValida = 0;
    //        oRegistro131.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro131.Save();

    //        ProtocoloLuminex oRegistro1311 = new ProtocoloLuminex();
    //        oRegistro1311.IdProtocolo = nroProtocolo;
    //        oRegistro1311.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro1311.IdSubitem = "Anti HLA-DP";
    //        oRegistro1311.TipoValor = "Positivo Moderado";
    //        oRegistro1311.Valor = positivoModerado_DP;
    //        oRegistro1311.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro1311.FechaResultado = DateTime.Now;
    //        oRegistro1311.IdUsuarioValida = 0;
    //        oRegistro1311.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro1311.Save();


    //        ProtocoloLuminex oRegistro13111 = new ProtocoloLuminex();
    //        oRegistro13111.IdProtocolo = nroProtocolo;
    //        oRegistro13111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //        oRegistro13111.IdSubitem = "Anti HLA-DP";
    //        oRegistro13111.TipoValor = "Positivo Fuerte";
    //        oRegistro13111.Valor = positivoFuerte_DP;
    //        oRegistro13111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //        oRegistro13111.FechaResultado = DateTime.Now;
    //        oRegistro13111.IdUsuarioValida = 0;
    //        oRegistro13111.FechaValida = DateTime.Parse("01/01/1900");
    //        oRegistro13111.Save();

    //        if (chkValida.Checked)
    //        {
    //            ISession m_session = NHibernateHttpModule.CurrentSession;
    //            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
    //            crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
    //            IList detalle = crit.List();
    //            if (detalle.Count > 0)
    //            {
    //                foreach (ProtocoloLuminex oDetalle in detalle)
    //                {
    //                    if ((oDetalle.IdSubitem == "Anti HLA-DR") || (oDetalle.IdSubitem == "Anti HLA-DQ") || (oDetalle.IdSubitem == "Anti HLA-DP"))
    //                    {
    //                        oDetalle.IdUsuarioValida = int.Parse(Session["idUsuario"].ToString());
    //                        oDetalle.FechaValida = DateTime.Now;
    //                        oDetalle.Save();
    //                    }
    //                }
    //            }
    //        }

    //        gvListaEspecificidad.Visible = false;
    //        tablaEspecificidad1.CargarGrillaProtocolo(nroProtocolo.ToString(),2);
    //        } // sigue=true
    //            else
    //            {
    //            estatus0.Text = "Ya se ha validado un informe para este protocolo. El mismo se descargará.";
    //            lblNro.Text = "PROTOCOLO NRO. " + nroProtocolo.ToString();
    //            lblNro.Visible = true;
    //            tablaEspecificidad1.CargarGrillaProtocolo(nroProtocolo.ToString(), 2);

    //        } // else sigue
        

    //}



 

    //    private void Guardar1()
    //    {
    //        string positivoFuerte_A = "";
    //        string positivoFuerte_B = "";
    //        string positivoFuerte_C = "";
    //        string positivoModerado_A = "";
    //        string positivoModerado_B = "";
    //        string positivoModerado_C = "";
    //        string positivoDebil_A = "";
    //        string positivoDebil_B = "";
    //        string positivoDebil_C = "";
    //        string negativo_A = "";
    //        string negativo_B = "";
    //        string negativo_C = "";
    //        int nroProtocolo = 0;
    //        int i = 0;
    //        int cantNegA = 0;
    //        int cantNegb = 0;
    //        int cantNegc = 0;
    //        bool sigue = true;
    //        nroProtocolo = int.Parse(idProtocolo.Value);
    //        if (i == 0)
    //            sigue = BorrarResultadosLuminex1(nroProtocolo);


    //        if (sigue)
    //        {


    //            foreach (GridViewRow row in gvListaEspecificidad.Rows)
    //            {
    //                MindrayResultado oProtocolo = new MindrayResultado();
    //                oProtocolo = (MindrayResultado)oProtocolo.Get(typeof(MindrayResultado), int.Parse(gvListaEspecificidad.DataKeys[row.RowIndex].Value.ToString()));

    //                //Protocolo oP = new Protocolo();
    //                //oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", oProtocolo.Protocolo);
    //                //idProtocolo = oP.IdProtocolo;

    //                double valor = double.Parse(oProtocolo.ValorObtenido);
    //                string MFI = oProtocolo.Descripcion;
    //                string Bead = oProtocolo.UnidadMedida;

    //                i = i + 1;
    //                if (valor > 7000) // positivo fuerte
    //                {
    //                    if (MFI.Substring(0, 1) == "A")
    //                        if (positivoFuerte_A == "")
    //                            positivoFuerte_A += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();

    //                    if (MFI.Substring(0, 1) == "B")

    //                        positivoFuerte_B += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    if (MFI.Substring(0, 1) == "C")
    //                        positivoFuerte_C += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                }

    //                if ((valor > 3000) && (valor <= 7000))// positivo moderado
    //                {
    //                    if (MFI.Substring(0, 1) == "A")
    //                        positivoModerado_A += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    if (MFI.Substring(0, 1) == "B")
    //                        positivoModerado_B += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    if (MFI.Substring(0, 1) == "C")
    //                        positivoModerado_C += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                }

    //                if ((valor >= 700) && (valor <= 3000))// positivo debil
    //                {
    //                    if (MFI.Substring(0, 1) == "A")
    //                        positivoDebil_A += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                    if (MFI.Substring(0, 1) == "B")
    //                        positivoDebil_B += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString(); ;
    //                    if (MFI.Substring(0, 1) == "C")
    //                        positivoDebil_C += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString(); ;
    //                }
    //                if ((valor < 700) && (valor > 0))// negativo
    //                {

    //                    if (MFI.Substring(0, 1) == "A")
    //                    {
    //                        if (cantNegA <= 5)
    //                        {
    //                            negativo_A += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                            cantNegA += 1;
    //                        }
    //                    }
    //                    if (MFI.Substring(0, 1) == "B")
    //                    {
    //                        if (cantNegb <= 5)
    //                        {
    //                            negativo_B += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                            cantNegb += 1;
    //                        }
    //                    }
    //                    if (MFI.Substring(0, 1) == "C")
    //                    {
    //                        if (cantNegc <= 5)
    //                        {
    //                            negativo_C += "(" + MFI + ") " + Bead + " MFI:" + valor.ToString() + Convert.ToChar(10).ToString();
    //                            cantNegc += 1;
    //                        }
    //                    }
    //                }// negativo
    //            } //foreach

    //                ProtocoloLuminex oRegistro = new ProtocoloLuminex();
    //                oRegistro.IdProtocolo = nroProtocolo;
    //                oRegistro.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro.IdSubitem = "Anti HLA-A";
    //                oRegistro.TipoValor = "Negativo";
    //                oRegistro.Valor = negativo_A;
    //                oRegistro.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro.FechaResultado = DateTime.Now;
    //                oRegistro.IdUsuarioValida = 0;
    //                oRegistro.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro.Save();

    //                ProtocoloLuminex oRegistro11 = new ProtocoloLuminex();
    //                oRegistro11.IdProtocolo = nroProtocolo;
    //                oRegistro11.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro11.IdSubitem = "Anti HLA-A";
    //                oRegistro11.TipoValor = "Positivo Debil";
    //                oRegistro11.Valor = positivoDebil_A;
    //                oRegistro11.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro11.FechaResultado = DateTime.Now;
    //                oRegistro11.IdUsuarioValida = 0;
    //                oRegistro11.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro11.Save();

    //                ProtocoloLuminex oRegistro111 = new ProtocoloLuminex();
    //                oRegistro111.IdProtocolo = nroProtocolo;
    //                oRegistro111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro111.IdSubitem = "Anti HLA-A";
    //                oRegistro111.TipoValor = "Positivo Moderado";
    //                oRegistro111.Valor = positivoModerado_A;
    //                oRegistro111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro111.FechaResultado = DateTime.Now;
    //                oRegistro111.IdUsuarioValida = 0;
    //                oRegistro111.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro111.Save();

    //                ProtocoloLuminex oRegistro1111 = new ProtocoloLuminex();
    //                oRegistro1111.IdProtocolo = nroProtocolo;
    //                oRegistro1111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro1111.IdSubitem = "Anti HLA-A";
    //                oRegistro1111.TipoValor = "Positivo Fuerte";
    //                oRegistro1111.Valor = positivoFuerte_A;
    //                oRegistro1111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro1111.FechaResultado = DateTime.Now;
    //                oRegistro1111.IdUsuarioValida = 0;
    //                oRegistro1111.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro1111.Save();

    //                ProtocoloLuminex oRegistro12 = new ProtocoloLuminex();
    //                oRegistro12.IdProtocolo = nroProtocolo;
    //                oRegistro12.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro12.IdSubitem = "Anti HLA-B";
    //                oRegistro12.TipoValor = "Negativo";
    //                oRegistro12.Valor = negativo_B;
    //                oRegistro12.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro12.FechaResultado = DateTime.Now;
    //                oRegistro12.IdUsuarioValida = 0;
    //                oRegistro12.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro12.Save();

    //                ProtocoloLuminex oRegistro121 = new ProtocoloLuminex();
    //                oRegistro121.IdProtocolo = nroProtocolo;
    //                oRegistro121.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro121.IdSubitem = "Anti HLA-B";
    //                oRegistro121.TipoValor = "Positivo Debil";
    //                oRegistro121.Valor = positivoDebil_B;
    //                oRegistro121.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro121.FechaResultado = DateTime.Now;
    //                oRegistro121.IdUsuarioValida = 0;
    //                oRegistro121.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro121.Save();

    //                ProtocoloLuminex oRegistro1211 = new ProtocoloLuminex();
    //                oRegistro1211.IdProtocolo = nroProtocolo;
    //                oRegistro1211.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro1211.IdSubitem = "Anti HLA-B";
    //                oRegistro1211.TipoValor = "Positivo Moderado";
    //                oRegistro1211.Valor = positivoModerado_B;
    //                oRegistro1211.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro1211.FechaResultado = DateTime.Now;
    //                oRegistro1211.IdUsuarioValida = 0;
    //                oRegistro1211.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro1211.Save();


    //                ProtocoloLuminex oRegistro12111 = new ProtocoloLuminex();
    //                oRegistro12111.IdProtocolo = nroProtocolo;
    //                oRegistro12111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro12111.IdSubitem = "Anti HLA-B";
    //                oRegistro12111.TipoValor = "Positivo Fuerte";
    //                oRegistro12111.Valor = positivoFuerte_B;
    //                oRegistro12111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro12111.FechaResultado = DateTime.Now;
    //                oRegistro12111.IdUsuarioValida = 0;
    //                oRegistro12111.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro12111.Save();

    //                //// C

    //                ProtocoloLuminex oRegistro13 = new ProtocoloLuminex();
    //                oRegistro13.IdProtocolo = nroProtocolo;
    //                oRegistro13.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro13.IdSubitem = "Anti HLA-C";
    //                oRegistro13.TipoValor = "Negativo";
    //                oRegistro13.Valor = negativo_C;
    //                oRegistro13.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro13.FechaResultado = DateTime.Now;
    //                oRegistro13.IdUsuarioValida = 0;
    //                oRegistro13.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro13.Save();

    //                ProtocoloLuminex oRegistro131 = new ProtocoloLuminex();
    //                oRegistro131.IdProtocolo = nroProtocolo;
    //                oRegistro131.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro131.IdSubitem = "Anti HLA-C";
    //                oRegistro131.TipoValor = "Positivo Debil";
    //                oRegistro131.Valor = positivoDebil_C;
    //                oRegistro131.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro131.FechaResultado = DateTime.Now;
    //                oRegistro131.IdUsuarioValida = 0;
    //                oRegistro131.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro131.Save();

    //                ProtocoloLuminex oRegistro1311 = new ProtocoloLuminex();
    //                oRegistro1311.IdProtocolo = nroProtocolo;
    //                oRegistro1311.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro1311.IdSubitem = "Anti HLA-C";
    //                oRegistro1311.TipoValor = "Positivo Moderado";
    //                oRegistro1311.Valor = positivoModerado_C;
    //                oRegistro1311.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro1311.FechaResultado = DateTime.Now;
    //                oRegistro1311.IdUsuarioValida = 0;
    //                oRegistro1311.FechaValida = DateTime.Parse("01/01/1900");
    //                oRegistro1311.Save();


    //                ProtocoloLuminex oRegistro13111 = new ProtocoloLuminex();
    //                oRegistro13111.IdProtocolo = nroProtocolo;
    //                oRegistro13111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
    //                oRegistro13111.IdSubitem = "Anti HLA-C";
    //                oRegistro13111.TipoValor = "Positivo Fuerte";
    //                oRegistro13111.Valor = positivoFuerte_C;
    //                oRegistro13111.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
    //                oRegistro13111.FechaResultado = DateTime.Now;
    //                oRegistro13111.IdUsuarioValida = 0;
    //                oRegistro13111.FechaValida = DateTime.Now;
    //                oRegistro13111.Save();



    //                if (chkValida.Checked)
    //                {
    //                    ISession m_session = NHibernateHttpModule.CurrentSession;
    //                    ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
    //                    crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
    //                    IList detalle = crit.List();
    //                    if (detalle.Count > 0)
    //                    {
    //                        foreach (ProtocoloLuminex oDetalle in detalle)
    //                        {
    //                            if ((oDetalle.IdSubitem == "Anti HLA-A") || (oDetalle.IdSubitem == "Anti HLA-B") || (oDetalle.IdSubitem == "Anti HLA-C"))
    //                            {
    //                                oDetalle.IdUsuarioValida = int.Parse(Session["idUsuario"].ToString());
    //                                oDetalle.FechaValida = DateTime.Now;
    //                                oDetalle.Save();
    //                            }
    //                        }
    //                    }
    //                }

    //                gvListaEspecificidad.Visible = false;
    //                lblNro.Text = "PROTOCOLO NRO. " + nroProtocolo.ToString();
    //                lblNro.Visible = true;
    //                tablaEspecificidad1.CargarGrillaProtocolo(nroProtocolo.ToString(), 1);
    //            } // sigue
    //            else
    //            {
    //                estatus0.Text = "Ya se ha validado un informe para este protocolo. El mismo se descargará.";
    //                lblNro.Text = "PROTOCOLO NRO. " + nroProtocolo.ToString();
    //                lblNro.Visible = true;
    //                tablaEspecificidad1.CargarGrillaProtocolo(nroProtocolo.ToString(), 1);

    //            } //else sigue
            


    //    }

        private bool BorrarResultadosLuminex1(int nroProtocolo)
        {
            bool borra = true;
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
            crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (ProtocoloLuminex oDetalle in detalle)
                {
                    if ((oDetalle.IdSubitem == "Anti HLA-A") || (oDetalle.IdSubitem == "Anti HLA-B") || (oDetalle.IdSubitem == "Anti HLA-C"))
                    { if (oDetalle.IdUsuarioValida == 0)
                            oDetalle.Delete();
                        else
                            borra = false; break;
                    }
                }
            }
            return borra;

        }
        private bool BorrarResultadosLuminex2(int nroProtocolo)
        {
            bool borra = true;
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
            crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (ProtocoloLuminex oDetalle in detalle)
                {
                    if ((oDetalle.IdSubitem == "Anti HLA-DR") || (oDetalle.IdSubitem == "Anti HLA-DQ") || (oDetalle.IdSubitem == "Anti HLA-DP"))
                    {
                        if (oDetalle.IdUsuarioValida == 0)
                            oDetalle.Delete();
                        else
                            borra = false; break;
                    }
                }
            }
            return borra;

        }

        protected void btnAdjuntar_Click(object sender, EventArgs e)
        {
            //Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            //oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo),"Numero", int.Parse(idProtocolo.Value));
            //if (oRegistro != null)
            //    Response.Redirect("../../Protocolos/ProtocoloAdjuntar.aspx?idProtocolo=" + oRegistro.IdProtocolo.ToString() + "&desde=protocolo&nombre="+ ddlTipoArchivo.SelectedItem.Text);
            //else
            //    estatus.Text = "No se ha encontrado un protocolo con el numero " + idProtocolo.Value ;

        }

    



    }
}