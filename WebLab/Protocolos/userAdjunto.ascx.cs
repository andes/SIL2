using Business;
using Business.Data;
using Business.Data.Laboratorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Protocolos
{
    public partial class userAdjunto : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void setProtocolo(int idProtocolo)
        {

          
        }
        protected void subir_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean fileOK = false;
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath("") + "\\" + lblProtocolo.Text; // @"C:\Archivos de Usuario\";


                    if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

                    string archivo = directorio + "\\" + trepador.FileName;

                    string extension = System.IO.Path.GetExtension(archivo).ToLower();
                    String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg", ".pdf" };
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (extension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }

                    if (fileOK)
                    {
                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha guardado exitosamente.";

                        Protocolo oOs = new Protocolo();
                        oOs = (Protocolo)oOs.Get(typeof(Protocolo), int.Parse(hdnProtocolo.Value));

                        Usuario oUser = new Usuario();
                        oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                        oOs.GuardarAnexo(trepador.FileName, txtDescripcion.Text, ddlVisibilidad.SelectedValue, oUser);

                        CargarGrilla();
                    }
                    else
                        estatus.Text = "no se acepta el tipo de archivo. Verifique que tengan extension .gif,png,.jpeg,jpg, pdf";

                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }
        }

        public void CargarGrilla()
        {
            gvListaProducto.DataSource = LeerDatosProtocolos();
            gvListaProducto.DataBind();
        }

        private object LeerDatosProtocolos()
        {



            string m_strSQL = @"select idProtocoloAnexo, url, descripcion, case when visible=0 then 'No visible' else 'Visible' end as visible from lab_protocoloanexo
WHERE  idProtocolo=" + hdnProtocolo.Value;


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];

        }

        protected void gvListaProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvListaProducto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                //case "Visualizar":
                //    {
                //        ProtocoloAnexo oRegistro = new ProtocoloAnexo();
                //        oRegistro = (ProtocoloAnexo)oRegistro.Get(typeof(ProtocoloAnexo), int.Parse(e.CommandArgument.ToString()));
                //        string directorio = Server.MapPath("") + "\\" + oRegistro.IdProtocolo.GetNumero() + "\\" + oRegistro.Url;
                //        Response.Redirect(directorio);
                        
                //       break;
                //    }

                case "Eliminar":
                    {
                        Anular(e.CommandArgument);
                        // PintarReferencias();
                        break;
                    }


            }
        }

        private void Anular(object commandArgument)
        {
            ProtocoloAnexo oRegistro = new ProtocoloAnexo();
            oRegistro = (ProtocoloAnexo)oRegistro.Get(typeof(ProtocoloAnexo), int.Parse(commandArgument.ToString()));
            oRegistro.Delete();
            CargarGrilla();
        }

        protected void gvListaProducto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                 
                         HyperLink Download = ((HyperLink)(e.Row.FindControl("Download")));
                              

                      //Download.NavigateUrl = ("http://myServer" + Directorio);

                    ProtocoloAnexo oRegistro = new ProtocoloAnexo();
                    oRegistro = (ProtocoloAnexo)oRegistro.Get(typeof(ProtocoloAnexo), int.Parse(this.gvListaProducto.DataKeys[e.Row.RowIndex].Value.ToString()));
                    string directorio = Server.MapPath("") + "\\" + oRegistro.IdProtocolo.GetNumero() + "\\" + oRegistro.Url;

                    Download.NavigateUrl = (directorio);




                    ImageButton CmdEliminar = (ImageButton)e.Row.Cells[4].Controls[1];
                    CmdEliminar.CommandArgument = this.gvListaProducto.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Eliminar";
                    CmdEliminar.ToolTip = "Eliminar";



                }
            }
        }
    }
}