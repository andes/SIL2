using Business;
using Business.Data.Facturacion;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;


namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class FacturaEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //InicializarTablas();
                //VerificaPermisos("Hoja de Trabajo Edit");


                if (Request["idCaso"] != null)
                {
                    lblCaso.Text = Request["idCaso"].ToString();
                    if (Request["total"]!= null)
                    lblTotal.Text = Request["total"].ToString();
                    CargarGrilla();
                    MostrarDatos();
                }

            }

        }

        private void MostrarDatos()
        {
            Factura oRegistro= new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura),"IdCasoFiliacion", int.Parse(lblCaso.Text),"Baja",false);
            if (oRegistro != null)
            {
                txtNumeroFactura.Text = oRegistro.Numero;

                lblTotal.Text = oRegistro.Total.ToString();
                btnImprimir.Visible = false;

            }
            else
                btnImprimir.Visible = false;
        }

        private void CargarGrilla()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
          

        }




        private object LeerDatos()
        {
            string m_strSQL = @"select D.idDetallePresupuesto, N.codigo, D.descripcion as nombre, 
 D.cantidadprefacturado as cantidad, D.precio,    D.totalPrefactura  as total 
from LAB_DetallePresupuesto d 
inner join LAB_Presupuesto as P on P.idPresupuesto= d.idPresupuesto 
inner join  LAB_CasoPresupuesto c on c.idPresupuesto=d.idPresupuesto
inner join LAB_NomencladorForense N on N.idNomencladorForense= D.idNomencladorForense
where  prefacturado=1 and c.idCasoFiliacion=" + lblCaso.Text +@" and  d.idcasofiliacion=" + lblCaso.Text;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Factura oRegistro = new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura), "IdCasoFiliacion", int.Parse(lblCaso.Text),"Baja",false);
            if (oRegistro != null)
                GrabarFactura(oRegistro,"Modifica");
            else
            { Factura oRegistroNew = new Factura();
                GrabarFactura(oRegistroNew, "Alta");
            }


            if (Request["Desde"].ToString()=="FacturaList")
            Response.Redirect("FacturaList.aspx", false);
            else
            {
                Context.Items.Add("idServicio", 6);
                Server.Transfer("../CasoList.aspx");
            }

        }

        private void GrabarFactura(Factura oRegistro, string accion)
        {
            oRegistro.IdCasoFiliacion = int.Parse(lblCaso.Text);
            oRegistro.Total = decimal.Parse(lblTotal.Text);
            oRegistro.Numero = txtNumeroFactura.Text;
            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Baja = false;
            oRegistro.Save();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"  update LAB_detallePresupuesto set idfactura=" + oRegistro.IdFactura.ToString() +
                @" where prefacturado=1 and idfactura=0 and idCasofiliacion=" + oRegistro.IdCasoFiliacion.ToString() +
                @" and idpresupuesto in  (select idPresupuesto from LAB_CasoPresupuesto where idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString() + @")";

            SqlCommand cmd = new SqlCommand(query, conn);
            int idcaso = Convert.ToInt32(cmd.ExecuteScalar());


            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(lblCaso.Text));

            oCaso.GrabarAuditoria(accion+ " Factura", int.Parse(Session["idUsuario"].ToString()), oRegistro.Numero);

           // imprimirFactura(oRegistro);

        }

        private void imprimirFactura(Factura oRegistro)
        {
            //string sourceFile = Server.MapPath("") + "\\factura.xls";
            //string destinationFile = Server.MapPath("") + "\\facturacopia.xls";
            //try
            //{
            //    File.Copy(sourceFile, destinationFile, true);
            //}
            //catch (IOException iox)
            //{
            //    Console.WriteLine(iox.Message);
            //}

            //Microsoft.Office.Interop.Excel.Application ExApp;
            //ExApp = new Microsoft.Office.Interop.Excel.Application();
            //Microsoft.Office.Interop.Excel._Workbook oWBook;
            //Microsoft.Office.Interop.Excel._Worksheet oSheet;
        
            //oWBook = ExApp.Workbooks.Open(Server.MapPath("") + "\\facturacopia.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWBook.ActiveSheet;
            //oSheet.Cells[10, 5] = "MINISTERIO PUBLICO FISCAL";
            //oSheet.Cells[12, 5] = "";
            //ISession m_session = NHibernateHttpModule.CurrentSession;
            //ICriteria crit = m_session.CreateCriteria(typeof(DetallePresupuesto));
            //crit.Add(Expression.Eq("IdFactura", oRegistro.IdFactura));
            //IList detalle = crit.List();
            //int  linea = 22;

            //int i = 1;
            //oSheet.Cells[6, 17] = "  "+oRegistro.FechaRegistro.ToShortDateString().Substring(0,2)+"            "+ oRegistro.FechaRegistro.ToShortDateString().Substring(3, 2) + "        "+ oRegistro.FechaRegistro.ToShortDateString().Substring(6, 4);
            //oSheet.Cells[14, 17] = "";
            //foreach (DetallePresupuesto oDetalle in detalle)
            //{
            //    oSheet.Cells[20, 4] = oDetalle.IdPresupuesto.Nombre;
            //    oSheet.Cells[linea, 3] = i.ToString();
            //    oSheet.Cells[linea, 4] = oDetalle.Descripcion;
            //    oSheet.Cells[linea, 17] = oDetalle.Cantidadprefacturado.ToString();
            //    oSheet.Cells[linea, 18] = oDetalle.TotalPrefactura.ToString();

            //    i = i + 1;
            //    linea = linea + 2;
            //}
            //Moneda oMoneda = new Moneda();

            ////primer parametro es la cantidad en string
            ////segundo parametro es si queremos que sea mayuscula
            ////tercer parametro la moneda
            //string resultado = oMoneda.Convertir(oRegistro.Total.ToString(), true, "PESOS");
            //oSheet.Cells[50, 5] = resultado.Replace("M.N.", ".------");
            //oSheet.Cells[50, 18] = oRegistro.Total.ToString();

            //ExApp.Visible = false;
            //ExApp.UserControl = true;
            //oWBook.Save();
            //ExApp.ActiveWorkbook.Close(true, "excelfile", Type.Missing);
            //ExApp.Quit();
            //ExApp = null;
            //DescargarDocumento(Server.MapPath("") + "\\facturacopia.xls", oRegistro.Numero+".xls");
            
        }

        private void DescargarDocumento(String ruta, string nombre)
        {
            try
            {
                
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "xls";
              
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombre);
                HttpContext.Current.Response.TransmitFile(ruta);
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                 
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Factura oRegistro = new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura), "IdCasoFiliacion", int.Parse(lblCaso.Text), "Baja", false);
            if (oRegistro != null)
                imprimirFactura(oRegistro);

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        { if (Request["Desde"]== "CasoList")
                Response.Redirect("../CasoList.aspx", false);
            if (Request["Desde"] == "FacturaList")
                
            Response.Redirect("FacturaList.aspx", false);
        }
    }
}