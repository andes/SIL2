
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.SessionState;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Business;
using Business.Data;
using Business.Data.Laboratorio;

namespace WebLab
{
    public class DescargarPdf : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["idUsuario"] == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            string informe = context.Request.QueryString["informe"];
            string consulta = context.Request.QueryString["consulta"];
            
            ReportDocument reporte = new ReportDocument();

            try
            {
                DataSet datos = PrepararInforme(context, consulta);

                if (datos.Tables.Count == 0 || datos.Tables[0].Rows.Count == 0)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("No hay datos para generar el informe.");
                    return;
                }

                string ruta = context.Server.MapPath("~/Informes/" + informe);
                reporte.Load(ruta);
                reporte.SetDataSource(datos.Tables[0]);

                // Parámetros específicos del informe.
                ConfigurarParametros(context, reporte, consulta);

                using (Stream stream = reporte.ExportToStream( ExportFormatType.PortableDocFormat))
                using (MemoryStream memoria = new MemoryStream())
                {
                    stream.CopyTo(memoria);

                    context.Response.Clear();
                    context.Response.ContentType = "application/pdf";
                    context.Response.AddHeader( "Content-Disposition", "attachment; filename=" + Path.GetFileNameWithoutExtension(informe) + ".pdf");
                    context.Response.BinaryWrite(memoria.ToArray());
                    context.Response.Flush();
                }
            }
            catch (Exception)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.Write("No se pudo generar el informe.");
            }
            finally
            {
                reporte.Close();
                reporte.Dispose();
            }
        }

        private DataSet PrepararInforme(HttpContext context, string consulta)
        {
            DataSet datos = new DataSet();
            string sql;

            switch (consulta)
            {
                case "derivacionPDF":
                    int idLote;
                    if (!int.TryParse(context.Request.QueryString["idLote"], out idLote))
                        throw new Exception("idLote inválido.");

                    sql = LoteDerivacion.derivacionPDF(idLote);
                    break;

                default:
                    throw new Exception("Consulta no permitida.");
            }

            SqlConnection conexion =
                (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conexion))
            {
                adapter.Fill(datos);
            }

            return datos;
        }

        private void ConfigurarParametros(HttpContext context, ReportDocument reporte, string consulta)
        {
            Usuario usuario = (Usuario)new Usuario().Get(typeof(Usuario), int.Parse(context.Session["idUsuario"].ToString()));

            Configuracion configuracion = (Configuracion)new Configuracion().Get( typeof(Configuracion), "IdEfector", usuario.IdEfector);

            switch (consulta)
            {
                case "derivacionPDF":
                    int efectorOrigen;
                    if (!int.TryParse( context.Request.QueryString["efectorOrigen"], out efectorOrigen))
                        throw new Exception("Efector inválido.");
                    

                    if (usuario.IdEfector.IdEfector == 227)
                    {
                        Efector efector = (Efector)new Efector().Get( typeof(Efector),  "IdEfector", efectorOrigen);
                        configuracion = (Configuracion)new Configuracion().Get( typeof(Configuracion), "IdEfector",  efector);
                    }

                    reporte.SetParameterValue(0, configuracion.EncabezadoLinea1);
                    reporte.SetParameterValue(1, configuracion.EncabezadoLinea2);
                    reporte.SetParameterValue(2, configuracion.EncabezadoLinea3);
                    break;

                default:
                    throw new Exception("Configuración no permitida.");
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}