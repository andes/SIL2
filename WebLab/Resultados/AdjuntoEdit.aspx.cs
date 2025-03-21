using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Data.SqlClient;
using System.Data;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.IO;
using System.Configuration;

namespace WebLab.Resultados
{
    public partial class AdjuntoEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!Page.IsPostBack)
            {

                string s_idDetalleProtocolo = Request["idDetalleProtocolo"].ToString();
                DetalleProtocolo oDetalle = new DetalleProtocolo();
                oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(s_idDetalleProtocolo));
                //    lblObservacionAnalisis.Text = oDetalle.IdProtocolo.GetNumero() + " - " + oDetalle.IdSubItem.Nombre;
                lblObservacionAnalisis.Text = oDetalle.IdProtocolo.Numero.ToString() + " - " + oDetalle.IdSubItem.Nombre;
                hdnidDetalleProtocolo.Value = s_idDetalleProtocolo;
                CargarGrilla();

                if (Request["operacion"] == "HC")
                    pnlbody.Visible = false;

            }
        }

        protected void subir_Click(object sender, EventArgs e)
        {
            try
            {
                DetalleProtocolo oOs = new DetalleProtocolo();
                oOs = (DetalleProtocolo)oOs.Get(typeof(DetalleProtocolo), int.Parse(hdnidDetalleProtocolo.Value));

                Boolean fileOK = false;
                if (trepador.HasFile)
                {
                    //    string directorio = Server.MapPath("") + "\\" + oOs.IdProtocolo.GetNumero() + "\\" + hdnidDetalleProtocolo.Value; // @"C:\Archivos de Usuario\";
                    string directorio = Server.MapPath("") + "\\" + oOs.IdProtocolo.Numero.ToString() + "\\" + hdnidDetalleProtocolo.Value; // @"C:\Archivos de Usuario\";


                    if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

                    string archivo = directorio + "\\" + trepador.FileName;

                    string extension = System.IO.Path.GetExtension(archivo).ToLower();
                    String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" }; //solamente imagen
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (extension == allowedExtensions[i])
                        {
                            fileOK = true;
                        }
                    }

                    if (fileOK)
                    {



                        if (!noexiste(oOs, trepador.FileName))
                        {
                            trepador.SaveAs(archivo);
                            estatus.Text = "El archivo se ha guardado exitosamente.";
                            Usuario oUser = new Usuario();
                            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                            oOs.IdProtocolo.GuardarAnexo(trepador.FileName, txtDescripcion.Text, ddlVisibilidad.SelectedValue, oUser, oOs.IdDetalleProtocolo);

                            CargarGrilla();
                        }
                        else
                            estatus.Text = "ya existe un archivo con ese nombre para el protocolo";

                    }
                    else
                        estatus.Text = "no se acepta el tipo de archivo. Verifique que tengan extension .gif,png,.jpeg,jpg";

                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }
        }
        private bool noexiste(DetalleProtocolo oOs, string fileName)
        {

            ProtocoloAnexo r = new ProtocoloAnexo();
            r = (ProtocoloAnexo)r.Get(typeof(ProtocoloAnexo), "IdDetalleProtocolo", oOs.IdDetalleProtocolo);
            if (r != null)
                return true;
            else
                return false;
        }
        private void CargarGrilla()
        {
            DTlist.RepeatColumns = 3;
            DTlist.DataSource = LeerDatosProtocolos();
            DTlist.DataBind();
            txtDescripcion.Text = "";
            //PintarReferencias();



        }

        private object LeerDatosProtocolos()
        {

            string m_strSQL = @"select a.idProtocoloAnexo, convert(varchar,P.numero) + '\'+convert(varchar,a.idDetalleProtocolo)+ '\'+ url as url , a.descripcion, case when visible=0 then 'No visible' else 'Visible' end as visible,
U.apellido + ' - ' + convert(varchar(10),a.fechaRegistro,103) as auditoria  
from lab_protocoloanexo a with (nolock)
inner join lab_protocolo P with (nolock) on P.idprotocolo=  a.idprotocolo
inner join sys_usuario as U with (nolock) on U.idusuario= a.idusuarioregistro
WHERE  a.idDetalleProtocolo=" + hdnidDetalleProtocolo.Value;




            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);





            return Ds.Tables[0];

        }

        protected void DTlist_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
                Anular(e.CommandArgument);

            if (e.CommandName == "Descargar")
                Descargar(e.CommandArgument);
        }

        private void Descargar(object commandArgument)
        {
            ProtocoloAnexo oRegistro1 = new ProtocoloAnexo();
            oRegistro1 = (ProtocoloAnexo)oRegistro1.Get(typeof(ProtocoloAnexo), int.Parse(commandArgument.ToString()));
            // oRegistro.IdProtocolo.GrabarAuditoriaProtocolo("Elimina Archivo: " + oRegistro.Url, int.Parse(Session["idUsuario"].ToString()));
            //oRegistro1.Delete(); //CargarGrilla();

            string directorio = oRegistro1.IdProtocolo.Numero.ToString() + "\\" + hdnidDetalleProtocolo.Value + "\\" + oRegistro1.Url;
            string extension = System.IO.Path.GetExtension(directorio).ToLower();


            switch (extension)
            {
                case ".png":
                    Response.ContentType = "image/png";
                    break;
                case ".gif":
                    Response.ContentType = "image/gif";
                    break;
                case ".jpeg":
                    Response.ContentType = "image/jpeg";
                    break;
                case ".jpg":
                    Response.ContentType = "image/jpg";
                    break;

            }



            Response.AppendHeader("Content-Disposition", "attachment; filename=" + oRegistro1.Url);


            Response.TransmitFile(Server.MapPath(directorio));
            Response.End();
            oRegistro1.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Descarga Adjunto", int.Parse(Session["idUsuario"].ToString()), oRegistro1.Url, "");


        }

        private void Anular(object commandArgument)
        {
            ProtocoloAnexo oRegistro1 = new ProtocoloAnexo();
            oRegistro1 = (ProtocoloAnexo)oRegistro1.Get(typeof(ProtocoloAnexo), int.Parse(commandArgument.ToString()));
            if (oRegistro1 != null)
            {

                oRegistro1.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Elimina Adjunto", int.Parse(Session["idUsuario"].ToString()), oRegistro1.Url, "");

                oRegistro1.Delete();
                CargarGrilla();
            }
        }

        protected void DTlist_ItemDataBound(object sender, DataListItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item ||
          e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)(e.Item.DataItem);

                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("Eliminar");
                Button btnDescargar = (Button)e.Item.FindControl("Descargar");
                //if (Request["desde"].ToString() == "resultado")
                //    btnEliminar.Visible = false;
                //else
                  btnEliminar.Visible = true;

                if (Request["operacion"] == "HC")
                {
                    btnEliminar.Visible = false;
                    btnDescargar.Visible = false;
                }


             

            }
        }
    }
}