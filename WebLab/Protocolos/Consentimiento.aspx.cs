using Business;
using Business.Data;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Protocolos
{
    public partial class Consentimiento : System.Web.UI.Page
    {
        public CrystalReportSource oCr = new CrystalReportSource();


        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;

        }


        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
      

        [WebMethod(EnableSession = true)]
        public static string GetCapturedImage()
        {
            string url = HttpContext.Current.Session["CapturedImage"].ToString();
            HttpContext.Current.Session["CapturedImage"] = null;
            return url;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Request.InputStream.Length > 0)
                {
                    using (StreamReader reader = new StreamReader(Request.InputStream))
                    {
                        string hexString = Server.UrlEncode(reader.ReadToEnd());
                        string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
                        string imagePath = string.Format("~/Protocolos/Webcam_Plugin/{0}.png", imageName);
                        string url1 = Server.MapPath(imagePath);
                        File.WriteAllBytes(Server.MapPath(imagePath), ConvertHexToBytes(hexString));
                        Session["CapturedImage"] = ResolveUrl(imagePath);
                        //Session["CapturedImage1"] = ResolveUrl(imagePath);
                    }
                }



                Paciente oP = new Paciente();
                oP = (Paciente)oP.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));
                //hdnProtocolo.Value = oP.IdProtocolo.ToString();
                txtFecha.Value = DateTime.Now.ToShortDateString();
                txtLugar.Text = "Neuquén";
                if (oP != null)
                {
                    lblCasoNro.Text = Session["idCaso"].ToString();
                    lblApellido.Text = oP.Apellido;
                    lblNombre.Text = oP.Nombre;
                    lblFechaNacimiento.Text = oP.FechaNacimiento.ToShortDateString();
                    lblSexo.Text = oP.getSexo();
                        if (Request["idTipoCaso"].ToString() != "3") //quimerismo: no requiere control de que protocolos en los ultimos 7 dias
                        
                        {
                    /// verifica si el paciente tiene protocolos en los ultimos 7 dias
                    string tieneingreso = oP.GetProtocolosReciente(7,Request["idServicio"].ToString());

                    if (tieneingreso != "")
                    {
                        lblAlertaProtocolo.Text = " La persona tiene un protocolo ingresado: " + tieneingreso;
                        lblAlertaProtocolo.Visible = true;
                        lnkProtocolo.Visible = false;
                        lnkBuscar.Visible = false;
                        lnkBuscar0.Visible = false;
                        btnRegresar.Visible = true;

                    }

                    }
                }
            }
        }

        protected void subir_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            GenerarConsentimiento("WORD");
        }

        private void GenerarConsentimiento(string v)
        {
            try
            {
                Int32 idconsenti;
                byte[] image = null;

                if (1 == 1)
                {
                    if (trepadorFoto.PostedFile.ContentLength > 0)
                    {
                        using (BinaryReader reader = new BinaryReader(trepadorFoto.PostedFile.InputStream))


                        {


                            image = reader.ReadBytes(trepadorFoto.PostedFile.ContentLength);
                            string directorio = Server.MapPath("") + "\\Consentimientos\\Fotos";


                            if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);

                            string archivo = directorio + "\\" + trepadorFoto.FileName;


                            trepadorFoto.SaveAs(archivo);

                        }
                    }/*hasta aca trepador*/
                    else
                    { 


                    //string path = Server.MapPath(Session["CapturedImage1"].ToString());
                    string path = Server.MapPath(Session["CapturedImage"].ToString());

                    // 2.
                    // Get byte array of file.
                      image = File.ReadAllBytes(path);

                    // 3A.

                }


                        SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                        string query = @"INSERT INTO [dbo].[LAB_Consentimiento]
           ([fecha]
           ,[fechaRegistro]
           ,[idUsuarioRegistro]
           ,[idPaciente]
           ,[baja]
           ,[imgFoto]
       
           ,[lugar])
     VALUES
           (GETDATE()
           ,GETDATE()
           ,"+ Session["idUsuario"].ToString()+"," + Request["idPaciente"].ToString() + ",0,@imagen ,@nombre)            SELECT CAST(scope_identity() AS int)";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@nombre", txtLugar.Text);


                        SqlParameter imageParam = cmd.Parameters.Add("@imagen", System.Data.SqlDbType.Image);
                      
                            imageParam.Value = image;
                        

                        idconsenti = Convert.ToInt32(cmd.ExecuteScalar());

                    if (Session["idCaso"].ToString() != "0")
                    {
                        Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();

                        oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Session["idCaso"].ToString()));
                        oRegistro.GrabarAuditoria("Genera Consentimiento en " + v, int.Parse(Session["idUsuario"].ToString()), lblApellido.Text + " " + lblNombre.Text);

                    }

                    string m_strSQL = @" select P.apellido, 
case when idestado=2 then 'Sin DNI' else convert(varchar,P.numerodocumento)   end as numerodocumento, 
C.fecha, P.nombre, P.fechaNacimiento, C.lugar as sector,
P.informacionContacto as observaciones, C.imgFoto as imagen from sys_paciente  as P
inner join lab_consentimiento as C on C.idpaciente = P.idpaciente where C.idConsentimiento=" + idconsenti.ToString();

                        DataSet Ds = new DataSet();

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                        adapter.Fill(Ds);


                        oCr.Report.FileName = "../Informes/Consentimiento.rpt";
                        oCr.ReportDocument.SetDataSource(Ds.Tables[0]);



                        oCr.DataBind();

                        if (v=="WORD")
                        oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.WordForWindows, Response, true, "Consentimiento_" + lblApellido.Text);
                        else oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Consentimiento_" + lblApellido.Text);



                 
                }
               //}

            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }

        }

        private void Descargar(object commandArgument)
        {
            ProtocoloAnexo oRegistro1 = new ProtocoloAnexo();
            oRegistro1 = (ProtocoloAnexo)oRegistro1.Get(typeof(ProtocoloAnexo), int.Parse(commandArgument.ToString()));
            // oRegistro.IdProtocolo.GrabarAuditoriaProtocolo("Elimina Archivo: " + oRegistro.Url, int.Parse(Session["idUsuario"].ToString()));
            //oRegistro1.Delete(); //CargarGrilla();

            string directorio = oRegistro1.IdProtocolo.GetNumero() + "\\" + oRegistro1.Url;
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

        protected void lnkProtocolo_Click(object sender, EventArgs e)
        {
            if ( Request["idServicio"].ToString()=="6")
                Response.Redirect("ProtocoloEditForense.aspx?idPaciente=" + Request["idPaciente"].ToString() + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString() + "&Urgencia=0");
            else
            Response.Redirect("ProtocoloEdit2.aspx?idPaciente=" + Request["idPaciente"].ToString() + "&Operacion=Alta&idServicio="+  Request["idServicio"].ToString()+"&Urgencia=0");
        }

        protected void lnkBuscar0_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                GenerarConsentimiento("PDF");
        }

        protected void lnkOmitirConsentimiento_Click(object sender, EventArgs e)
        {

            Response.Redirect("ProtocoloEditForense.aspx?idPaciente=" + Request["idPaciente"].ToString() + "&Operacion=Alta&idServicio=" + Request["idServicio"].ToString() + "&Urgencia=0");
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
        
            Response.Redirect("Default2.aspx?idServicio=" + Session["idServicio"].ToString() + "&idUrgencia=0", false);//; break;

        }





        //}












    }
}
    
