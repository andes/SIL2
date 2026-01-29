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
using Business;
using System.Data.SqlClient;
using System.IO;
using Business.Data.Laboratorio;

using System.Text;

namespace WebLab.Protocolos
{
    public partial class ProtocoloExport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                
                VerificaPermisos("Exportacion de datos");
                CargarGrilla();
                MarcarSeleccionados(false);
                if (Request["idServicio"].ToString() == "3") lblServicio.Text = "MICROBIOLOGIA";
                else lblServicio.Text = "LABORATORIO";
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
                    case 1:
                        {imgExportarExcel.Visible = false;
                        imgExportarTexto.Visible = false;
                        }break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MarcarSeleccionados(bool p)
        {           
            foreach (GridViewRow row in gvLista.Rows )
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)                
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;                                   
            }            
        }
                
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            PonerImagenes();
        }

        private object LeerDatos()
        {
            string str_condicion =  " P.idProtocolo  in (" + Session["ListaProtocolo"].ToString() + ")";


            string m_strSQL = @" SELECT DISTINCT  P.idProtocolo, P.numero  as numero,
                               CONVERT(varchar(10),P.fecha,103) as fecha,  case when Pa.idestado <> 2 then Pa.numeroDocumento else 0 end as dni,Pa.apellido+ ' ' + Pa.nombre as paciente,
                               O.nombre as origen, Pri.nombre as prioridad, SS.nombre as sector,P.estado, P.impreso 
                               FROM Lab_Protocolo P
                               INNER JOIN Lab_Origen O on O.idOrigen= P.idOrigen
                               INNER JOIN Lab_Prioridad Pri on Pri.idPrioridad= P.idPrioridad
                               INNER JOIN Sys_Paciente Pa on Pa.idPaciente= P.idPaciente 
                               INNER JOIN LAB_SectorServicio SS  ON P.idSector= SS.idSectorServicio ";
                              if (Request["Whonet"].ToString()=="True")
                                  m_strSQL += " INNER JOIN LAB_ProtocoloWhonet as PW on PW.idProtocolo= P.idProtocolo " ;

                              m_strSQL += " WHERE " + str_condicion + " order by P.idProtocolo"; // +str_orden;
                              
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
             
            return Ds.Tables[0];
        }

     

       

        private string GenerarListaProtocolos()
        {
            string m_lista = "0";
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    if (m_lista == "0")
                        m_lista += gvLista.DataKeys[row.RowIndex].Value.ToString();
                    else
                        m_lista += "," + gvLista.DataKeys[row.RowIndex].Value.ToString();
                }
            }
            return m_lista;
        }

        private DataTable GenerarSetDatos()
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_ExportacionDatosWhonet]";
            
            cmd.Parameters.Add("@idServicio", SqlDbType.NVarChar);
            cmd.Parameters["@idServicio"].Value = Request["idServicio"].ToString();

            cmd.Parameters.Add("@ListaProtocolos", SqlDbType.NVarChar);
            cmd.Parameters["@ListaProtocolos"].Value = GenerarListaProtocolos();

            cmd.Connection = conn;
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            
            return Ds.Tables[0];
        }

        

        protected void btnMarcarSel_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
        }

        protected void btnDesmarcarSel_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
         //   Imprimir();
        }

       

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {                                   
        }

        private void PonerImagenes()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {                
                    switch (row.Cells[9].Text)
                    {
                        case "0": ///Abierto
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                                row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                        case "1": //en proceso
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/amarillo.gif";
                               row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                        case "2": //terminado
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                                row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                    }

            }
        }


  
        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            PonerImagenes();
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            PonerImagenes();
        }

      

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;
            CargarGrilla();
           
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProtocoloList.aspx?idServicio=1&tipo=Exportacion", false);
        }

        protected void imgExportar_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel();
        }

        private void ExportarTexto()
        {
            DataTable dt = GenerarSetDatos();

            

            string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

            if (Directory.Exists(directorio))
            {
                string archivo = directorio + "\\TextFile.txt"; /// probablemente tiene que ser extension .ana (renombrar)

                using (StreamWriter streamwriter = new StreamWriter(archivo))
                {
                    string s_nombreArchivo = ""; string linea = "";
                    if (Request["idServicio"].ToString() == "3")
                    {
                        s_nombreArchivo = "Microbiologia_" + DateTime.Now.ToShortDateString().Replace("/", "");
                        linea = "NUMERO;FECHA;ORIGEN;CODIGO MUESTRA;DESCRIPCION MUESTRA;DNI;APELLIDO;NOMBRE;FECHA NAC.;SEXO;MATERIAL;CODIGO MICROORGANISMO;DESCRIPCION MICROORGANISMO";
                    }
                    else
                    {
                        s_nombreArchivo = "Laboratorio_" + DateTime.Now.ToShortDateString().Replace("/", "");
                        linea = "NUMERO;FECHA;ORIGEN;PRIORIDAD;DNI;APELLIDO;NOMBRE;FECHA NAC.;SEXO";
                    }





                    streamwriter.WriteLine(linea);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        linea = "";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (linea == "")
                                linea = dt.Rows[i][j].ToString().Replace(";", "");
                            else
                                linea += ";" + dt.Rows[i][j].ToString().Replace(";", "");
                        }
                        streamwriter.WriteLine(linea);
                    }
                    streamwriter.Close();

                    string archivo_ruta = MapPath("TextFile.txt");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "text/plain";
                    Response.AppendHeader("content-disposition", "attachment; filename=" + s_nombreArchivo + ".txt");
                    Response.Clear();
                    Response.WriteFile(archivo_ruta);
                    Response.End();


                }
            }

//            System.IO.StreamWriter streamwriter = new System.IO.StreamWriter(System.IO.File.Open
//(MapPath("TextFile.txt"), System.IO.FileMode.OpenOrCreate));

            

            
        }
        private void ExportarExcel()
        {
            string s_nombreArchivo = "";
            if (Request["idServicio"].ToString() == "3") s_nombreArchivo = "Microbiologia_"+DateTime.Now.ToShortDateString().Replace("/","");
            else s_nombreArchivo = "Laboratorio_" + DateTime.Now.ToShortDateString().Replace("/", "");

            Utility.ExportDataTableToXlsx(GenerarSetDatos(), s_nombreArchivo);
            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //HtmlTextWriter htw = new HtmlTextWriter(sw);

            //Page page = new Page();
            //HtmlForm form = new HtmlForm();
            //GridView dg = new GridView();
            //dg.EnableViewState = false;
            //dg.DataSource = GenerarSetDatos();
            //dg.DataBind();

            //// Deshabilitar la validación de eventos, sólo asp.net 2
            //page.EnableEventValidation = false;

            //// Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD.
            //page.DesignerInitialize();
            //page.Controls.Add(form);
            //form.Controls.Add(dg);
            //page.RenderControl(htw);

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.AddHeader("Content-Disposition", "attachment;filename="+s_nombreArchivo+".xls");
            //Response.Charset = "UTF-8";
            //Response.ContentEncoding = Encoding.Default;
            //Response.Write(sb.ToString());
            //Response.End();
        }

        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            ExportarExcel();
        }

        protected void imgExportarTexto_Click(object sender, ImageClickEventArgs e)
        {
            ExportarTexto();
        }

        protected void imgExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportarExcel();
        }

    }
}
