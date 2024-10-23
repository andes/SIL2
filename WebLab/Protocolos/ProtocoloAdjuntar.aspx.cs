using Business;
using Business.Data;
using Business.Data.Laboratorio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Protocolos
{
    public partial class ProtocoloAdjuntar : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Request["desde"].ToString() == "protocolo")
                Page.MasterPageFile = "~/Site1.master";
            if (Request["desde"].ToString() == "valida")
                Page.MasterPageFile = "~/Site2.master";
            if (Request["desde"].ToString() == "resultado")
                Page.MasterPageFile = "~/Site2.master";

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                oP = (Business.Data.Laboratorio.Protocolo)oP.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                hdnProtocolo.Value = oP.IdProtocolo.ToString();

            if (oP != null)
            {
                lblProtocolo.Text = oP.GetNumero();
                if (oP.IdTipoServicio.IdTipoServicio != 5)
                    lblProtocolo.Text = lblProtocolo.Text + " " + oP.IdPaciente.Apellido + " " + oP.IdPaciente.Nombre;

                CargarGrilla();
                    if (Request["desde"].ToString() == "resultado")
                        datos.Visible = false;
                    if (Request["nombre"] != null)
                        txtDescripcion.Text = Request["nombre"].ToString();
                    if (Request["desde"] == "valida")
                        btnRegresar.Visible = false;
            }
            }
        }

            protected void subir_Click(object sender, EventArgs e)
        {
            try
            {
                Protocolo oOs = new Protocolo();
                oOs = (Protocolo)oOs.Get(typeof(Protocolo), int.Parse(hdnProtocolo.Value));

                Boolean fileOK = false;
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath("") + "\\" + oOs.GetNumero(); // @"C:\Archivos de Usuario\";


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
                        

                       
                        if (!noexiste(oOs, trepador.FileName))
                        {
                            trepador.SaveAs(archivo);
                            estatus.Text = "El archivo se ha guardado exitosamente.";
                            Usuario oUser = new Usuario();
                            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                            oOs.GuardarAnexo(trepador.FileName, txtDescripcion.Text, ddlVisibilidad.SelectedValue, oUser, 0);

                            CargarGrilla();
                        }
                        else
                            estatus.Text = "ya existe un archivo con ese nombre para el protocolo";

                    }
                    else
                        estatus.Text = "no se acepta el tipo de archivo. Verifique que tengan extension .gif,png,.jpeg,jpg, pdf";

                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }
        }

        private bool noexiste(Protocolo oOs, string fileName)
        {

            ProtocoloAnexo r = new ProtocoloAnexo();
            r = (ProtocoloAnexo)r.Get(typeof(ProtocoloAnexo),"IdProtocolo", oOs, "Url", fileName);
            if (r != null)
                return true;
            else
                return false;
        }
        public class Imagen
        {
            public string Url { get; set; }
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

string m_strSQL = @"select PA.idProtocoloAnexo,convert(varchar,P.numero )+ '\'+ Pa.url as url , Pa.descripcion, case when visible=0 then 'No visible' else 'Visible' end as visible,
U.apellido + ' - ' + convert(varchar(10),PA.fechaRegistro,103) as auditoria  
from lab_protocoloanexo PA
inner join sys_usuario as U on U.idusuario= PA.idusuarioregistro
inner join lab_protocolo as P on P.idprotocolo= pa.idprotocolo
WHERE Pa.iddetalleprotocolo=0 and  Pa.idProtocolo=" + hdnProtocolo.Value;

            if (Request["desde"].ToString() == "resultado")
                m_strSQL = @"select pa.idProtocoloAnexo, convert(varchar,P.numero ) + '\'+ pa.url as url , Pa.descripcion, case when visible=0 then 'No visible' else 'Visible' end as visible ,
U.apellido + ' - ' + convert(varchar(10),pa.fechaRegistro,103) as auditoria 
from lab_protocoloanexo as PA
inner join sys_usuario as U on U.idusuario= pa.idusuarioregistro
inner join lab_protocolo as P on P.idprotocolo= pa.idprotocolo
WHERE pa.iddetalleprotocolo=0 and  visible=1 and  pa.idProtocolo=" + hdnProtocolo.Value;


            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            //List<Imagen> lista = new List<Imagen>();
            //Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
            //oP = (Business.Data.Laboratorio.Protocolo)oP.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            //for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            //{
            //   string ruta= oP.GetNumero() + "\\" + Ds.Tables[0].Rows[i][1].ToString();
            //   lista.Add(new Imagen { Url = ruta });
                
            //}
          

            //DTlist.DataSource = Ds.Tables[0]; // lista;
            //    DTlist.DataBind();
            

                return Ds.Tables[0];

        }

        protected void gvListaProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        //protected void gvListaProducto_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //   if (e.CommandName==                "Eliminar")
                    
        //                Anular(e.CommandArgument);
        //    if (e.CommandName == "Descargar")
        //        Descargar(e.CommandArgument);

           

        //}

        private void Descargar(object commandArgument)
        {
            ProtocoloAnexo oRegistro1 = new ProtocoloAnexo();
            oRegistro1 = (ProtocoloAnexo)oRegistro1.Get(typeof(ProtocoloAnexo), int.Parse(commandArgument.ToString()));
            // oRegistro.IdProtocolo.GrabarAuditoriaProtocolo("Elimina Archivo: " + oRegistro.Url, int.Parse(Session["idUsuario"].ToString()));
            //oRegistro1.Delete(); //CargarGrilla();

            string directorio = oRegistro1.IdProtocolo.Numero.ToString() + "\\" + oRegistro1.Url;
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
                case ".pdf":
                    Response.ContentType = "application/pdf"; break;
            }



            Response.AppendHeader("Content-Disposition", "attachment; filename=" + oRegistro1.Url);


            Response.TransmitFile(Server.MapPath(directorio));
            Response.End();
            oRegistro1.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Descarga Adjunto", int.Parse(Session["idUsuario"].ToString()), oRegistro1.Url, "");


        }

      


        //}
        private void Anular(object commandArgument)
        {
            
            ProtocoloAnexo oRegistro1 = new ProtocoloAnexo();
            oRegistro1 = (ProtocoloAnexo)oRegistro1.Get(typeof(ProtocoloAnexo), int.Parse(commandArgument.ToString()));



            oRegistro1.IdProtocolo.GrabarAuditoriaDetalleProtocolo("Elimina Adjunto", int.Parse(Session["idUsuario"].ToString()), oRegistro1.Url, "");

            oRegistro1.Delete();
            CargarGrilla();

         

            

         
         

           
        }

    

        protected void DTlist_ItemDataBound(object sender, DataListItemEventArgs e)
        {



            if (e.Item.ItemType == ListItemType.Item ||
          e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)(e.Item.DataItem);

                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("Eliminar");

                if (Request["desde"].ToString() == "resultado")
                    btnEliminar.Visible = false;
                else
                    btnEliminar.Visible = true;

                Image btnUrl = (Image)e.Item.FindControl("img");

                //Protocolo oRegistro1 = new Protocolo();
                //oRegistro1 = (Protocolo)oRegistro1.Get(typeof(Protocolo), int.Parse(hdnProtocolo.Value));
                //// oRegistro.IdProtocolo.GrabarAuditoriaProtocolo("Elimina Archivo: " + oRegistro.Url, int.Parse(Session["idUsuario"].ToString()));
                ////oRegistro1.Delete(); //CargarGrilla();

                //string directorio = oRegistro1.GetNumero() + "\\" + btnUrl.ImageUrl.ToString();
                string directorio =  btnUrl.ImageUrl.ToString();
                string extension = System.IO.Path.GetExtension(directorio).ToLower();
                if (extension == ".pdf")

                    btnUrl.ImageUrl = "..\\App_Themes\\default\\images\\pdfgrande.jpg";

            }
        }

        protected void DTlist_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
                Anular(e.CommandArgument);

            if (e.CommandName == "Descargar")
                Descargar              (e.CommandArgument);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            if (Request["desde"] == "protocolo")
                Response.Redirect("ProtocoloList.aspx?idServicio=" + Request["idServicio"].ToString() + "&Tipo=" + Request["tipo"].ToString());
        }
    }
}
    
