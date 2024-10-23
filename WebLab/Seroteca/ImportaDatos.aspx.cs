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
using System.Configuration;

namespace WebLab.Seroteca
{
    public partial class ImportaDatos : System.Web.UI.Page
    {
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            else
                Response.Redirect("../FinSesion.aspx", false);


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Seroteca");
                CargarListas();
                CargarGrilla();                   

            }


        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///////////////Impresoras////////////////////////

          string  m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora where idEfector= " + oUser.IdEfector.IdEfector.ToString();
            oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
           
 if (Session["Etiquetadora"] != null) ddlImpresora.SelectedValue = Session["Etiquetadora"].ToString();         


            /////////////////////////////////////////////

            m_ssql = null;
            oUtil = null;
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

        private void CargarGrilla()
        {
        


            string m_strSQL = @"select idMindrayResultado, protocolo, protocolo as numero, convert(varchar(10),fechaprotocolo,103) as fecha, 
descripcion as apellido, unidadmedida as dni, valorobtenido as transfusion 
from [LAB_MindrayResultado] where idEfector=" + oUser.IdEfector.IdEfector.ToString();
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

           

            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            //PintarReferencias();
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            //PintarReferencias();
        }
        //private void PintarReferencias()
        //{      
        //    foreach (GridViewRow row in gvLista.Rows)
        //    {           
        //        switch (row.Cells[8].Text)
        //        {
        //            case "0": //sin enviar
        //                {
        //                    System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
        //                    hlnk.ImageUrl = "../../App_Themes/default/images/rojo.gif";
        //                    hlnk.ToolTip = "Sin procesar";
        //                    row.Cells[8].Controls.Add(hlnk);
        //                }
        //                break;
        //            case "1": //enviado
        //                {
        //                    System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
        //                    hlnk.ImageUrl = "../../App_Themes/default/images/amarillo.gif";
        //                    hlnk.ToolTip = "En proceso";
        //                    row.Cells[8].Controls.Add(hlnk);
        //                }
        //                break;
                   
        //        }
        //    }
        //}
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

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        { 
                            if (i!=1)                         
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
            crit.Add(Expression.Eq("IdEfector", oUser.IdEfector.IdEfector)); // borra por idefector
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
            {
                string[] arr = linea.Split((";").ToCharArray());
               

                if (arr.Length >= 5)
                {
                   

                   string numero = arr[0].ToString();
                    string apellidonombre= arr[1].ToString();
                    string dni = arr[2].ToString();
                    string fechatranfusion= arr[3].ToString();
                    string tranfusion= arr[4].ToString();

                    ///Se reutiliza la tabla MindrayResultado para volcar temporalmente los resultados del metrolab
                    MindrayResultado oRegistro = new MindrayResultado();
                    oRegistro.Protocolo = numero;
                    if (fechatranfusion=="")
                        oRegistro.FechaProtocolo = DateTime.Parse("01/01/1900");
                    else
                    oRegistro.FechaProtocolo = DateTime.Parse(fechatranfusion);
                    oRegistro.IdEfector = oUser.IdEfector.IdEfector;
                    oRegistro.IdItemMindray = 0;
                    oRegistro.Descripcion = apellidonombre;
                    oRegistro.UnidadMedida = dni;
                    oRegistro.ValorObtenido = tranfusion;
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
            Guardar();
      //      Response.Redirect("Mensaje.aspx?Cantidad="+ cantidad.ToString(), false);
            // mostrar mensaje que se han guardado los datos
        }

        private void Guardar()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                  

                    MindrayResultado oProtocolo = new MindrayResultado();
                    oProtocolo = (MindrayResultado)oProtocolo.Get(typeof(MindrayResultado), "Protocolo", int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
                    ImprimirEtiqueta(oProtocolo.Protocolo, oProtocolo.FechaProtocolo, oProtocolo.Descripcion, oProtocolo.UnidadMedida, oProtocolo.ValorObtenido);


                }// chececk
            }// primero
        
    }

        private void ImprimirEtiqueta( string numero, DateTime fechatranfusion, string apellido, string dni, string tranfusion)
        {
            Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
            Business.Etiqueta ticket = new Business.Etiqueta();
            ticket.TipoEtiqueta = oC.TipoEtiqueta;



            ticket.AddHeaderLine(numero + "    " + dni);
            ticket.AddSubHeaderLine(apellido.ToUpper());
            if (fechatranfusion.ToShortDateString() != "01/01/1900")
                ticket.AddSubHeaderLine(fechatranfusion.ToShortDateString() + "    " + tranfusion);
            else
                ticket.AddSubHeaderLine("");
            ticket.AddSubHeaderLineNegrita("");
            ticket.AddSubHeaderLine("");

            //// falta pasar por parametro la fuente de codigo de barras
            ticket.AddCodigoBarras("", "");
            ticket.AddFooterLine("");

            TipoServicio oTipoServicio = new TipoServicio();
            oTipoServicio = (TipoServicio)oTipoServicio.Get(typeof(TipoServicio), 3);

            ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra();
            oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipoServicio);
            Session["Etiquetadora"] = ddlImpresora.SelectedValue;
            //oCr.ReportDocument.PrintOptions.PrinterName = Session["Etiquetadora"].ToString();// ConfigurationSettings.AppSettings["Impresora"]; 
            try
            {
                ticket.PrintTicket(ddlImpresora.SelectedValue, oConBarra.Fuente);
            }
            catch (Exception ex)
            {
                string exception = "";
                //while (ex != null)
                //{
                exception = ex.Message + "<br>";

                //}
                string popupScript = "<script language='JavaScript'> alert('No se pudo imprimir en la impresora " + Session["Etiquetadora"].ToString() + ". Si el problema persiste consulte con soporte técnico." + exception + "'); </script>";
                Page.RegisterStartupScript("PopupScript", popupScript);
                

            }
        }

    

    }
}