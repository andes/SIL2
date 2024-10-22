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

namespace WebLab.AutoAnalizador.Metrolab
{
    public partial class ImportaDatos : System.Web.UI.Page
    {
        private int cantidad = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) VerificaPermisos("Metrolab - Recepción de datos");
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

                        //if (File.Exists(archivo))
                        //{
                        //    // ya existe un archivo con el mismo nombre en el directorio,
                        //    // así que hay hacer algo al respecto (p.ej. renombrar el que 
                        //    // está en el servidor o asignarle otro nombre al que se está 
                        //    // subiendo), de lo contrario el archivo en el servidor será 
                        //    // sobreescrito
                        //}
                        //else
                        //{
                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";

                        // 
                        // TODO: código para procesar el archivo va aquí...
                        //
                        // }
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
//            ////Metodo que carga la grilla de Protocolos
//            ///Ve como traer los protocolos con prefijo.
//            string m_strSQL = @" SELECT  P.idProtocolo, dbo.NumeroProtocolo(P.idProtocolo) AS numero, convert(varchar(10),P.fecha,103) as fecha, 
//		case when Pac.idEstado=1 then Pac.numerodocumento else '' end as numeroDocumento, Pac.apellido + ' ' + Pac.nombre AS Paciente   , O.nombre as Origen,
//convert (varchar (10),P.edad) + case P.unidadedad when 0 then ' años ' when 1 then ' meses ' when 2 then ' días ' end as edad,
//P.sexo as sexo, P.estado
//		FROM         LAB_Protocolo AS P  
//		inner join LAB_Origen AS O on O.idOrigen= P.idOrigen 
//		INNER JOIN   Sys_Paciente AS Pac ON P.idPaciente = Pac.idPaciente  
//WHERE P.estado<2 and   (dbo.NumeroProtocolo(P.idProtocolo) IN
//                          (SELECT DISTINCT protocolo
//                            FROM          LAB_MindrayResultado))
//";


//            DataSet Ds = new DataSet();
//            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
//            SqlDataAdapter adapter = new SqlDataAdapter();
//            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
//            adapter.Fill(Ds);
//            gvLista.DataSource = Ds.Tables[0];
//            gvLista.DataBind();
//            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados";
//            PintarReferencias();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_IncorporarResultadosMetrolab]";


            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            gvLista.DataSource = Ds.Tables[0];
            gvLista.DataBind();
            lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos encontrados";
            PintarReferencias();
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            PintarReferencias();
        }
        private void PintarReferencias()
        {      
            foreach (GridViewRow row in gvLista.Rows)
            {           
                switch (row.Cells[8].Text)
                {
                    case "0": //sin enviar
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../../App_Themes/default/images/rojo.gif";
                            hlnk.ToolTip = "Sin procesar";
                            row.Cells[8].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //enviado
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../../App_Themes/default/images/amarillo.gif";
                            hlnk.ToolTip = "En proceso";
                            row.Cells[8].Controls.Add(hlnk);
                        }
                        break;
                    //case "2": //enviado
                    //    {
                    //        System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                    //        hlnk.ImageUrl = "../App_Themes/default/images/verde.gif";
                    //        row.Cells[6].Controls.Add(hlnk);
                    //    }
                    //    break;
                }
            }
        }
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            PintarReferencias();
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

                if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                {
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {                          
                            ProcesarLinea(line);
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
            {
                string[] arr = linea.Split((";").ToCharArray());
                string numeroprotocolo = ""; int cantidadPracticas = 0;
                if (arr.Length >= 12)
                {
                    string codigoPractica = "";
                    string resultado = "";
                    string unidadMedida = "";
                    numeroprotocolo = arr[0].ToString();
                    cantidadPracticas = int.Parse(arr[9].ToString());
                    if (cantidadPracticas > 0)
                    {
                        for (int j = 0; j < cantidadPracticas; j++)
                        {
                            try
                            {
                                int m = 9 + (j * 3); ///Cada tres posiciones esta un nuevo analisiS

                                codigoPractica = arr[m + 1].ToString();
                                resultado = arr[m + 2].ToString();
                                unidadMedida = arr[m + 3].ToString();
                                Grabar(numeroprotocolo, codigoPractica, resultado, unidadMedida);
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "hay lineas del archivo que han sido ignoradas por no tener el formato esperado por el sistema."; }
        }

        private void Grabar(string numeroprotocolo, string codigoPractica, string resultado, string unidadMedida)
        {
            ///Se reutiliza la tabla MindrayResultado para volcar temporalmente los resultados del metrolab
            MindrayResultado oRegistro = new MindrayResultado();
            oRegistro.Protocolo = numeroprotocolo.Trim();
            oRegistro.FechaProtocolo = DateTime.Now;
            oRegistro.IdItemMindray = 0;
            oRegistro.Descripcion = codigoPractica.Trim();
            oRegistro.UnidadMedida = unidadMedida.Trim();
            oRegistro.ValorObtenido = resultado.Trim();
            oRegistro.TipoValor = "";
            oRegistro.FechaResultado = DateTime.Now;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Estado = 0;
            oRegistro.Save();

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
            Response.Redirect("Mensaje.aspx?Cantidad="+ cantidad.ToString(), false);
            // mostrar mensaje que se han guardado los datos
        }

        private void Guardar()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    string s_prefijo = "";
                    cantidad += 1;
                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    string numero =  row.Cells[1].Text.Trim();
                    string numeroAux = row.Cells[1].Text.Trim();
                    ///
                    string[] arr = numero.Split(("-").ToCharArray());
                    if (arr.Length > 1)// tiene prefijo
                    {
                        numero = arr[0].ToString();
                        s_prefijo = arr[1].ToString();
                    }
            
                    ///

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(MindrayResultado));
                    crit.Add(Expression.Eq("Protocolo", numeroAux));
                    IList items = crit.List();
                    foreach (MindrayResultado oResultado in items)
                    {
                        /// busco el item en el lis                    
                        ICriteria crit2 = m_session.CreateCriteria(typeof(MetrolabItem));
                        crit2.Add(Expression.Eq("IdMetrolab", oResultado.Descripcion));
                        crit2.Add(Expression.Eq("Prefijo", s_prefijo));
                        crit2.Add(Expression.Eq("Habilitado",true));                        
                        MetrolabItem oItem =(MetrolabItem) crit2.UniqueResult();
                        //oItem = (MetrolabItem)oItem.Get(typeof(MetrolabItem), "IdMetrolab", oResultado.Descripcion, "Habilitado",true);
                        if (oItem != null)
                        {
                          
                                int IdItemLIS = oItem.IdItem; // id item en el lis
                                Item oItemLIS = new Item();
                                oItemLIS = (Item)oItemLIS.Get(typeof(Item), IdItemLIS);
                                string valorObtenido = oResultado.ValorObtenido;

                                DetalleProtocolo oDetalle = new DetalleProtocolo();
                                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), "IdProtocolo", oProtocolo, "IdSubItem", oItemLIS);
                                if (oDetalle != null)
                                {
                                    if (oDetalle.IdUsuarioValida == 0)// si no fue validado
                                    {
                                        if (oItemLIS.IdTipoResultado == 1) //Si es numero
                                        {
                                            decimal s_ItemNum = decimal.Parse(valorObtenido.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                                            oDetalle.ResultadoNum = s_ItemNum;
                                            oDetalle.Enviado = 2;
                                            oDetalle.ConResultado = true;
                                            oDetalle.FechaResultado = DateTime.Now;
                                            oDetalle.Save();
                                            oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Metrolab", int.Parse(Session["idUsuario"].ToString()));
                                        }
                                        else //Si es texto
                                        {
                                            oDetalle.ResultadoCar = valorObtenido;
                                            oDetalle.Enviado = 2;
                                            oDetalle.ConResultado = true;
                                            oDetalle.FechaResultado = DateTime.Now;
                                            oDetalle.Save();
                                            oDetalle.GrabarAuditoriaDetalleProtocolo("Automático Metrolab", int.Parse(Session["idUsuario"].ToString()));
                                        }
                                        if (oProtocolo.Estado == 0)
                                        {
                                            oProtocolo.Estado = 1;
                                            oProtocolo.Save();
                                        }
                                    } // fin if idusuario validado
                                } // fin odetalle null  
                         
                        }// item nnll
                    } // foreach
                }// chececk
            }// primero
        }
    }


}