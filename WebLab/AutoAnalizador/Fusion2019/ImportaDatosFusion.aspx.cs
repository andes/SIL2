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
using Business.Data;

namespace WebLab.AutoAnalizador.Fusion2019
{
    public partial class ImportaDatosFusion : System.Web.UI.Page
    {
        bool cargar = false;
        string numeroProtocolo= "";
        string resultado = "";

        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Interface Fusion");
          

                //CargarListas();
                //CargarGrilla();                   

            }


        }

        //private void CargarListas()
        //{
        //    Utility oUtil = new Utility();
        //    ///////////////Impresoras////////////////////////

        //    string m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
        //    oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");

        //    if (Session["Etiquetadora"] != null) ddlImpresora.SelectedValue = Session["Etiquetadora"].ToString();


        //    /////////////////////////////////////////////

        //    m_ssql = null;
        //    oUtil = null;
        //}
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
                        


                            CargarGrilla();
                      
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
        //private void CargarGrillaEspecificidad()
        //{





        //    string 
        //        m_strSQL = @"select idMindrayResultado, protocolo as numero, '' as item,
        //    M.descripcion as apellido, M.unidadmedida as dni, M.valorobtenido as transfusion 
        //    FROM            LAB_MindrayResultado AS M ";


        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);


        //    gvListaEspecificidad.DataSource = Ds.Tables[0];
        //    gvListaEspecificidad.DataBind();
        //    lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
        //    gvListaEspecificidad.Visible = true;
        //    gvLista.Visible = false;
        //    //PintarReferencias();
        //}
        private void CargarGrilla()
        {          

                string m_strSQL = @"select idMindrayResultado, protocolo as numero, I.nombre AS item,
M.descripcion as apellido, M.unidadmedida as dni, M.valorobtenido as transfusion 
FROM            LAB_MindrayResultado AS M INNER JOIN
                         LAB_Item AS I ON I.idItem = M.idItemMindray
 where M.idEfector="+oUser.IdEfector.IdEfector.ToString ()+" and protocolo not in (select numero from lab_protocolo where estado=2)  order by numero ";



            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

           

            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            gvListaEspecificidad.Visible = false;
            gvLista.Visible = true;
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            //PintarReferencias();
        }
       
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            //PintarReferencias();
        }

        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
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
                    string extension = filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper();
                   

                        using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            if (i != 1)
                            //    if (ddlTipoArchivo.SelectedValue != "Transplante")
                            //    {
                            //        ProcesarLinea(line);
                            //    }
                            //else
                                    ProcesarLineaTransplante(line);
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

        //private void ProcesarLinea(string linea)
        //{
        //    try
        //    {
        //        string[] arr = linea.Split((";").ToCharArray());
               

        //        if (arr.Length >= 5)
        //        {
                   
        //            string  campo_cero= arr[0].ToString();
        //            string campo_MFI = arr[1].ToString();
        //            string campo_Bead= arr[2].ToString();
        //            string campo_valor = arr[3].ToString();



        //            if ((campo_cero == "Sample ID:") && (campo_MFI != "") && (cargar==false))
        //            {
        //                 numeroProtocolo = campo_MFI;
        //                cargar = true;
        //            }
        //            if (cargar)
        //            {
        //                if (campo_cero.Substring(0, 1) == "0")
        //                {


        //                    ///Se reutiliza la tabla MindrayResultado para volcar temporalmente los resultados del luminex
        //                    MindrayResultado oRegistro = new MindrayResultado();
        //                    if (!existe(numeroProtocolo,))
        //                    {

        //                        oRegistro.Protocolo = numeroProtocolo;
        //                        oRegistro.FechaProtocolo = DateTime.Parse("01/01/1900");

        //                        oRegistro.IdItemMindray = 0;
        //                        oRegistro.Descripcion = campo_MFI;
        //                        oRegistro.UnidadMedida = campo_Bead;
        //                        oRegistro.ValorObtenido = campo_valor;
        //                        oRegistro.TipoValor = "";
        //                        oRegistro.FechaResultado = DateTime.Now;
        //                        oRegistro.FechaRegistro = DateTime.Now;
        //                        oRegistro.Estado = 0;
        //                        oRegistro.Save();
        //                    }
        //                }

        //            }


        //        }
        //    }
        //    catch (Exception ex) {
        //        string exception = "";
                
        //           exception = ex.Message + "<br>";

        //        estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema."+ exception;
        //    }
        //}

        private bool existe(string nroProtocolo, int nroItem, string resul)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(MindrayResultado));
            crit.Add(Expression.Eq("Protocolo", nroProtocolo));
            crit.Add(Expression.Eq("IdItemMindray", nroItem));
            crit.Add(Expression.Eq("Descripcion", resul));
            crit.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector));

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                return true;
            }
            else
                return false;
        }

        private void ProcesarLineaTransplante(string linea)
        {
            try
            {
                string[] arr = linea.Split((";").ToCharArray());


                if (arr.Length >= 1)
                {
                    int iditem = 0;

                    string campo_cero = arr[0].ToString();

                    Utility oUtil = new Utility();

                    if ((cargar) && (numeroProtocolo != ""))
                    {
                        //string c1= campo_cero.Replace('"', ' ').Replace("\n", ";").Trim();
                        //string[] arrc1 = c1.Split((";").ToCharArray());
                        //for (int i=0; i<arrc1.Length;i++)
                        if (campo_cero == "Sample Date:")
                            campo_cero = arr[2].ToString();

                        resultado = "";
                        campo_cero = campo_cero.Replace('"', ' ');

                        

                        string letras = campo_cero.Trim().Substring(0, 3);

                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(FusionItem));
                        IList detalle = crit.List();
                        if (detalle.Count > 0)
                        {
                            foreach (FusionItem oDetalle in detalle)
                            {
                                int pos = letras.IndexOf(oDetalle.IdFusion, 0);
                                if ((pos==0) && (oDetalle.Habilitado))
                                //if ((letras == oDetalle.IdFusion) && (oDetalle.Habilitado))
                                {
                                    iditem = oDetalle.IdItem; //HLA-A Luminex
                                    resultado = campo_cero.Replace("$", "");
                                }

                            }
                        }




                        if (resultado != "")
                        {
                            if (!existe(numeroProtocolo, iditem, resultado))
                            {
                                MindrayResultado oRegistro = new MindrayResultado();
                                oRegistro.Protocolo = numeroProtocolo;
                                oRegistro.FechaProtocolo = DateTime.Parse("01/01/1900");
                                oRegistro.IdEfector = oUser.IdEfector.IdEfector;
                                oRegistro.IdItemMindray = iditem;
                                oRegistro.Descripcion = resultado;
                                oRegistro.UnidadMedida = "";
                                oRegistro.ValorObtenido = "";
                                oRegistro.TipoValor = "";
                                oRegistro.FechaResultado = DateTime.Now;
                                oRegistro.FechaRegistro = DateTime.Now;
                                oRegistro.Estado = 0;
                                oRegistro.Save();


                                // update de detalleprotocolo set resultadocar=Descripcion where numeroprotocolo= numeroprotocolo and iditem= iditem and idusuauriocarga=0 

                                cargar = false;
                            }
                        }

                    }
                    if (oUtil.EsNumerico(campo_cero))
                    {

                        numeroProtocolo = campo_cero;
                        cargar = true;
                    }
                }

            }
            catch (Exception ex)
            {
                string exception = "";

                exception = ex.Message + "<br>";

                estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {


            //if (ddlTipoArchivo.SelectedValue!= "Transplante")
                
            //    Guardar();
            //else
                GuardarTransplante();
      
        }

        private void GuardarTransplante()
        {
            int i = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {

                string re = ((TextBox)(gvLista.Rows[row.RowIndex]).Cells[3].Controls[1]).Text;
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    MindrayResultado oProtocolo = new MindrayResultado();
                    oProtocolo = (MindrayResultado)oProtocolo.Get(typeof(MindrayResultado), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    Protocolo oPro = new Protocolo();
                    oPro = (Protocolo)oPro.Get(typeof(Protocolo), "Numero", oProtocolo.Protocolo);

                    if (oPro != null) // si existe el protocolo
                    {
                        if (oPro.Estado < 2)
                        {
                            Item oItem = new Item();
                            oItem = (Item)oItem.Get(typeof(Item), "IdItem", oProtocolo.IdItemMindray);

                            DetalleProtocolo oDetalle = new DetalleProtocolo();
                            oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oPro, "IdSubItem", oItem);

                            if (oDetalle != null) // si contiene el resultado
                            {
                                if ((oDetalle.IdUsuarioResultado == 0) &&
                                    (oDetalle.IdUsuarioValida == 0))
                                {
                                    oDetalle.ResultadoCar = re; // oProtocolo.Descripcion;
                                    oDetalle.IdUsuarioResultado = int.Parse(Session["idUsuario"].ToString());
                                    oDetalle.FechaResultado = DateTime.Now;
                                    oDetalle.ConResultado = true;
                                    oDetalle.Save();
                                    i += 1;

                                }
                            }
                        }
                        if (oPro.EnProceso()) //actualiza estado protocolo
                        {
                            oPro.Estado = 1;
                            oPro.Save();

                        }
                    }//opro

                }//checked
            }// grid



            estatus.Text = "se han guardado " + i.ToString() + " registros";


        }
        //private void Guardar()
        //{
        //    string positivoFuerte_A = "";
        //    string positivoFuerte_B = "";
        //    string positivoFuerte_C = "";
        //    string positivoModerado_A = "";
        //    string positivoModerado_B = "";
        //    string positivoModerado_C = "";
        //    string positivoDebil_A = "";
        //    string positivoDebil_B = "";
        //    string positivoDebil_C = "";
        //    string negativo_A = "";
        //    string negativo_B = "";
        //    string negativo_C = "";
        //    int nroProtocolo = 0;
        //    int i = 0;
           
        //    foreach (GridViewRow row in gvListaEspecificidad.Rows)
        //    {
        //        MindrayResultado oProtocolo = new MindrayResultado();
        //        oProtocolo = (MindrayResultado)oProtocolo.Get(typeof(MindrayResultado), int.Parse(gvListaEspecificidad.DataKeys[row.RowIndex].Value.ToString()));

        //        //Protocolo oP = new Protocolo();
        //        //oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", oProtocolo.Protocolo);
        //        //idProtocolo = oP.IdProtocolo;

        //        double valor = double.Parse(oProtocolo.ValorObtenido);
        //        string MFI = oProtocolo.Descripcion;
        //        string Bead = oProtocolo.UnidadMedida;
        //         nroProtocolo = int.Parse(oProtocolo.Protocolo);
        //        if (i==0)
        //        BorrarResultadosLuminex(nroProtocolo);
        //        i = i + 1;
        //        if (valor > 7000) // positivo fuerte
        //        {
        //            if (MFI.Substring(0, 1) == "A")
        //                positivoFuerte_A += "("+MFI + ") " + Bead + System.Environment.NewLine; //+ "[" + valor.ToString() +"]";
        //            if (MFI.Substring(0, 1) == "B")
        //                positivoFuerte_B += "(" + MFI + ") " + Bead+ System.Environment.NewLine; //+ "[" + valor.ToString() +"]";
        //            if (MFI.Substring(0, 1) == "C")
        //                positivoFuerte_C += "(" + MFI + ") " + Bead+ System.Environment.NewLine; //+ "[" + valor.ToString() +"]";
        //        }

        //        if ((valor > 3000) && (valor <= 7000))// positivo moderado
        //        {
        //            if (MFI.Substring(0, 1) == "A")
        //                positivoModerado_A += "(" + MFI + ") " + Bead + System.Environment.NewLine; //+ "[" + valor.ToString() +"]";
        //            if (MFI.Substring(0, 1) == "B")
        //                positivoModerado_B += "(" + MFI + ") " + Bead + System.Environment.NewLine; //+ "[" + valor.ToString() +"]";
        //            if (MFI.Substring(0, 1) == "C")
        //                positivoModerado_C += "(" + MFI + ") " + Bead + System.Environment.NewLine; //+ "[" + valor.ToString() +"]";
        //        }

        //        if ((valor >= 700) && (valor <= 3000))// positivo debil
        //        {
        //            if (MFI.Substring(0, 1) == "A")
        //                positivoDebil_A += MFI + " " + Bead; //+ "[" + valor.ToString() + "]";
        //            if (MFI.Substring(0, 1) == "B")
        //                positivoDebil_B += MFI + " " + Bead; //+ "[" + valor.ToString() + "]";
        //            if (MFI.Substring(0, 1) == "C")
        //                positivoDebil_C += MFI + " " + Bead; //+ "[" + valor.ToString() + "]";
        //        }
        //        if (valor < 700)// negativo
        //        {
        //            if (MFI.Substring(0, 1) == "A")
        //                negativo_A += MFI + " " + Bead; //+ "[" + valor.ToString() + "]";
        //            if (MFI.Substring(0, 1) == "B")
        //                negativo_B += MFI + " " + Bead; //+ "[" + valor.ToString() + "]";
        //            if (MFI.Substring(0, 1) == "C")
        //                negativo_C += MFI + " " + Bead; //+ "[" + valor.ToString() + "]";
        //        }
        //        // ImprimirEtiqueta(oProtocolo.Protocolo, oProtocolo.FechaProtocolo, oProtocolo.Descripcion, oProtocolo.UnidadMedida, oProtocolo.ValorObtenido);

        //    }// primero


        //    ProtocoloLuminex oRegistro = new ProtocoloLuminex();
        //    oRegistro.IdProtocolo = nroProtocolo;
        //    oRegistro.IdItem =int.Parse( ddlTipoArchivo.SelectedValue);
        //    oRegistro.IdSubitem = "Anti HLA-A";
        //    oRegistro.TipoValor = "Negativo";
        //    oRegistro.Valor = negativo_A;
        //    oRegistro.Save();

        //    ProtocoloLuminex oRegistro11 = new ProtocoloLuminex();
        //    oRegistro11.IdProtocolo = nroProtocolo;
        //    oRegistro11.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro11.IdSubitem = "Anti HLA-A";
        //    oRegistro11.TipoValor = "Positivo Debil";
        //    oRegistro11.Valor = positivoDebil_A;
        //    oRegistro11.Save();

        //    ProtocoloLuminex oRegistro111 = new ProtocoloLuminex();
        //    oRegistro111.IdProtocolo = nroProtocolo;
        //    oRegistro111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro111.IdSubitem = "Anti HLA-A";
        //    oRegistro111.TipoValor = "Positivo Moderado";
        //    oRegistro111.Valor = positivoModerado_A;
        //    oRegistro111.Save();

        //    ProtocoloLuminex oRegistro1111 = new ProtocoloLuminex();
        //    oRegistro1111.IdProtocolo = nroProtocolo;
        //    oRegistro1111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro1111.IdSubitem = "Anti HLA-A"; 
        //    oRegistro1111.TipoValor = "Positivo Fuerte";
        //    oRegistro1111.Valor = positivoFuerte_A;           
        //    oRegistro1111.Save();

        //    ProtocoloLuminex oRegistro12 = new ProtocoloLuminex();
        //    oRegistro12.IdProtocolo = nroProtocolo;
        //    oRegistro12.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro12.IdSubitem = "Anti HLA-B";
        //    oRegistro12.TipoValor = "Negativo";
        //    oRegistro12.Valor = negativo_B;
        //    oRegistro12.Save();

        //    ProtocoloLuminex oRegistro121 = new ProtocoloLuminex();
        //    oRegistro121.IdProtocolo = nroProtocolo;
        //    oRegistro121.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro121.IdSubitem = "Anti HLA-B";
        //    oRegistro121.TipoValor = "Positivo Debil";
        //    oRegistro121.Valor= positivoDebil_B;
        //    oRegistro121.Save();

        //    ProtocoloLuminex oRegistro1211 = new ProtocoloLuminex();
        //    oRegistro1211.IdProtocolo = nroProtocolo;
        //    oRegistro1211.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro1211.IdSubitem = "Anti HLA-B";
        //    oRegistro1211.TipoValor = "Positivo Moderado";
        //    oRegistro1211.Valor= positivoModerado_B;
        //    oRegistro1211.Save();


        //    ProtocoloLuminex oRegistro12111 = new ProtocoloLuminex();
        //    oRegistro12111.IdProtocolo = nroProtocolo;
        //    oRegistro12111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro12111.IdSubitem = "Anti HLA-B";
        //    oRegistro12111.TipoValor = "Positivo Fuerte";
        //    oRegistro12111.Valor= positivoFuerte_B;
        //    oRegistro12111.Save();

        //    //// C

        //    ProtocoloLuminex oRegistro13 = new ProtocoloLuminex();
        //    oRegistro13.IdProtocolo = nroProtocolo;
        //    oRegistro13.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro13.IdSubitem = "Anti HLA-C";
        //    oRegistro13.TipoValor = "Negativo";
        //    oRegistro13.Valor = negativo_C;
        //    oRegistro13.Save();

        //    ProtocoloLuminex oRegistro131 = new ProtocoloLuminex();
        //    oRegistro131.IdProtocolo = nroProtocolo;
        //    oRegistro131.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro131.IdSubitem = "Anti HLA-C";
        //    oRegistro131.TipoValor = "Positivo Debil";
        //    oRegistro131.Valor = positivoDebil_C;
        //    oRegistro131.Save();

        //    ProtocoloLuminex oRegistro1311 = new ProtocoloLuminex();
        //    oRegistro1311.IdProtocolo = nroProtocolo;
        //    oRegistro1311.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro1311.IdSubitem = "Anti HLA-C";
        //    oRegistro1311.TipoValor = "Positivo Moderado";
        //    oRegistro1311.Valor = positivoModerado_C;
        //    oRegistro1311.Save();


        //    ProtocoloLuminex oRegistro13111 = new ProtocoloLuminex();
        //    oRegistro13111.IdProtocolo = nroProtocolo;
        //    oRegistro13111.IdItem = int.Parse(ddlTipoArchivo.SelectedValue);
        //    oRegistro13111.IdSubitem = "Anti HLA-C";
        //    oRegistro13111.TipoValor = "Positivo Fuerte";
        //    oRegistro13111.Valor = positivoFuerte_C;
        //    oRegistro13111.Save();


        //    //ProtocoloLuminex oRegistro2 = new ProtocoloLuminex();
        //    //oRegistro2.IdProtocolo = nroProtocolo;
        //    //oRegistro2.IdItem = 1;
        //    //oRegistro2.IdSubitem = "Anti HLA-C";
        //    //oRegistro2.ValorNegativo = negativo_C;
        //    //oRegistro2.ValorPositivoDebil = positivoDebil_C;
        //    //oRegistro2.ValorPositivoModerado = positivoModerado_C;
        //    //oRegistro2.ValorPositivoFuerte = positivoFuerte_C;
        //    //oRegistro2.Save();




        //}

        private void BorrarResultadosLuminex(int nroProtocolo)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloLuminex));
            crit.Add(Expression.Eq("IdProtocolo", nroProtocolo));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (ProtocoloLuminex oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }

        }

        //private void ImprimirEtiqueta( string numero, DateTime fechatranfusion, string apellido, string dni, string tranfusion)
        //{
        //    Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
        //    Business.Etiqueta ticket = new Business.Etiqueta();
        //    ticket.TipoEtiqueta = oC.TipoEtiqueta;



        //    ticket.AddHeaderLine(numero + "    " + dni);
        //    ticket.AddSubHeaderLine(apellido.ToUpper());
        //    if (fechatranfusion.ToShortDateString() != "01/01/1900")
        //        ticket.AddSubHeaderLine(fechatranfusion.ToShortDateString() + "    " + tranfusion);
        //    else
        //        ticket.AddSubHeaderLine("");
        //    ticket.AddSubHeaderLineNegrita("");
        //    ticket.AddSubHeaderLine("");

        //    //// falta pasar por parametro la fuente de codigo de barras
        //    ticket.AddCodigoBarras("", "");
        //    ticket.AddFooterLine("");

        //    TipoServicio oTipoServicio = new TipoServicio();
        //    oTipoServicio = (TipoServicio)oTipoServicio.Get(typeof(TipoServicio), 3);

        //    ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();
        //    oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipoServicio);
        //    Session["Etiquetadora"] = ddlTipoArchivo.SelectedValue;
        //    //oCr.ReportDocument.PrintOptions.PrinterName = Session["Etiquetadora"].ToString();// ConfigurationSettings.AppSettings["Impresora"]; 
        //    try
        //    {
        //        ticket.PrintTicket(ddlTipoArchivo.SelectedValue, oConBarra.Fuente);
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = "";
        //        //while (ex != null)
        //        //{
        //        exception = ex.Message + "<br>";

        //        //}
        //        string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Etiquetadora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
        //        Page.RegisterStartupScript("PopupScript", popupScript);


        //    }
        //}



    }
}