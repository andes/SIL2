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
using System.Web.UI.WebControls;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class FacturaFiliacionEdit : System.Web.UI.Page
    {
        DataTable dtDeterminaciones; //tabla para determinaciones
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InicializarTablas();
                //VerificaPermisos("Hoja de Trabajo Edit");


                if (Request["idCaso"] != null)
                {
                    lblCaso.Text = Request["idCaso"].ToString();
                    if (Request["total"] != null)
                        txtTotal.Text = Request["total"].ToString();
                    CargarListas();
                    //CargarGrilla();
                    MostrarDatos();
                }

            }

        }
        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                NomencladorForense oItem = new NomencladorForense();
                oItem = (NomencladorForense)oItem.Get(typeof(NomencladorForense), int.Parse(ddlItem.SelectedValue));
                txtCodigo.Text = oItem.Codigo;

                txtPrecio.Text = oItem.Precio.ToString();
                ddlProtocoloFiliacion.Enabled = true;
                txtCantidad.Enabled = true;
                btnAgregar.Enabled = true;
            }
            else
            {
                txtCodigo.Text = "";

                txtPrecio.Text = "";
                ddlProtocoloFiliacion.SelectedValue = "0";
                txtCantidad.Text = "";
                btnAgregar.Enabled = false;

            }

            txtCodigo.UpdateAfterCallBack = true;
            txtPrecio.UpdateAfterCallBack = true;
            ddlProtocoloFiliacion.UpdateAfterCallBack = true;
            txtCantidad.UpdateAfterCallBack = true;
            btnAgregar.UpdateAfterCallBack = true;

        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = "select idNomencladorForense as idItem, nombre from LAB_NomencladorForense where baja=0  order by nombre";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;

            m_ssql = @"select P.idprotocolo, Pac.apellido + ' ' + Pac.nombre as nombre from LAB_CasoFiliacionProtocolo as CF
inner join LAB_Protocolo as P on P.idProtocolo = CF.idProtocolo
inner join Sys_Paciente as Pac on Pac.idPaciente = P.idPaciente
 where CF.idCasoFiliacion = " + lblCaso.Text;
            oUtil.CargarCombo(ddlProtocoloFiliacion, m_ssql, "idprotocolo", "nombre");
            ddlProtocoloFiliacion.Items.Insert(0, new ListItem("Seleccione Muestra", "0"));







            m_ssql = null;
            oUtil = null;
        }
        private void MostrarDatos()
        {
            Factura oRegistro = new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura), "IdCasoFiliacion", int.Parse(lblCaso.Text), "Baja", false);
            if (oRegistro != null)
            {
                txtNumeroFactura.Text = oRegistro.Numero;


                txtTotal.Text = oRegistro.Total.ToString();
                btnImprimir.Visible = false;

                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                DetallePresupuesto oDetalle = new DetallePresupuesto();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(DetallePresupuesto));
                crit.Add(Expression.Eq("IdCasoFiliacion", int.Parse(lblCaso.Text)));


                IList items = crit.List();
                int i = 0;
                foreach (DetallePresupuesto oDet in items)
                {
                    i = i + 1;
                    NomencladorForense oN = new NomencladorForense();
                    oN = (NomencladorForense)oN.Get(typeof(NomencladorForense), "IdNomencladorForense", oDet.IdNomencladorForense);

                    DataRow row = dtDeterminaciones.NewRow();
                    row[0] = oDet.IdNomencladorForense.ToString();
                    row[1] = oN.Codigo;

                    row[2] = oDet.Descripcion;
                    row[3] = oDet.Cantidad.ToString();
                    row[4] = oDet.Precio.ToString();
                    row[5] = oDet.Total.ToString();
                    row[7] = i.ToString();
                    row[8] = oDet.IdDetallePresupuesto.ToString();
                    dtDeterminaciones.Rows.Add(row);

                }
                Session.Add("Tabla1", dtDeterminaciones);
                gvLista.DataSource = dtDeterminaciones;
                gvLista.DataBind();
                CalcularTotal();
                btnImprimir.Visible = true;




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
where  prefacturado=1 and c.idCasoFiliacion=" + lblCaso.Text + @" and  d.idcasofiliacion=" + lblCaso.Text;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }
        private void GuardarPresupuesto(Presupuesto oRegistro)
        {

            oRegistro.IdServicio = 0;
            oRegistro.Fecha = DateTime.Today;
            oRegistro.Encabezado1 = "";
            oRegistro.Encabezado2 = "";
            oRegistro.Pie = "";
            oRegistro.Nombre = "Caso Filiacion nro. " + lblCaso.Text;
            ///////////////////////////////////
            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();


            GuardarDetalle(oRegistro);
            GuardarVinculacion(oRegistro);



        }

        private void GuardarVinculacion(Presupuesto oRegistro)
        {
            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(lblCaso.Text));

            Business.Data.Facturacion.CasoPresupuesto oPro = new Business.Data.Facturacion.CasoPresupuesto();


            oPro.IdPresupuesto = oRegistro;
            oPro.IdCasoFiliacion = oCaso;
            oPro.Save();

            oCaso.GrabarAuditoria("Vincula a presupuesto", int.Parse(Session["idUsuario"].ToString()), oRegistro.IdPresupuesto.ToString());

        }

        private void GuardarDetalle(Presupuesto oRegistro)
        {
            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetallePresupuesto));
            crit.Add(Expression.Eq("IdPresupuesto", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (DetallePresupuesto oDetalle in detalle)
                {
                    //oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Desvinculado de caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
                    oDetalle.Delete();

                }
            }

            /////Crea nuevamente los detalles.
            for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
            {
                DetallePresupuesto oDetalle = new DetallePresupuesto();


                oDetalle.IdPresupuesto = oRegistro;
                oDetalle.IdNomencladorForense = int.Parse(dtDeterminaciones.Rows[i][0].ToString());
                oDetalle.Descripcion = dtDeterminaciones.Rows[i][2].ToString();
                oDetalle.Cantidad = int.Parse(dtDeterminaciones.Rows[i][3].ToString());
                oDetalle.Precio = decimal.Parse(dtDeterminaciones.Rows[i][4].ToString());
                oDetalle.Total = decimal.Parse(dtDeterminaciones.Rows[i][5].ToString());
                oDetalle.Prefacturado = true;
                oDetalle.Cantidadprefacturado = int.Parse(dtDeterminaciones.Rows[i][3].ToString());
                oDetalle.TotalPrefactura = decimal.Parse(dtDeterminaciones.Rows[i][5].ToString());
                oDetalle.IdFactura = 0;
                oDetalle.IdCasoFiliacion = int.Parse(lblCaso.Text);
                oDetalle.Save();

                //oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Presupuesto oPre = new Presupuesto();


            GuardarPresupuesto(oPre);

            Factura oRegistro = new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura), "IdCasoFiliacion", int.Parse(lblCaso.Text), "Baja", false);
            if (oRegistro != null)
                GrabarFactura(oRegistro, "Modifica");
            else
            {
                Factura oRegistroNew = new Factura();
                GrabarFactura(oRegistroNew, "Alta");
            }


            if (Request["Desde"].ToString() == "FacturaList")
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
            oRegistro.Total = decimal.Parse(txtTotal.Text);
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

            oCaso.GrabarAuditoria(accion + " Factura", int.Parse(Session["idUsuario"].ToString()), oRegistro.Numero);

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
            //int linea = 22;

            //int i = 1;
            //oSheet.Cells[6, 17] = "  " + oRegistro.FechaRegistro.ToShortDateString().Substring(0, 2) + "            " + oRegistro.FechaRegistro.ToShortDateString().Substring(3, 2) + "        " + oRegistro.FechaRegistro.ToShortDateString().Substring(6, 4);
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
            //DescargarDocumento(Server.MapPath("") + "\\facturacopia.xls", oRegistro.Numero + ".xls");

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
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigo.Text != "")
            {
                NomencladorForense oItem = new NomencladorForense();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(NomencladorForense));
                crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
                crit.Add(Expression.Eq("Baja", false));


                oItem = (NomencladorForense)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlItem.SelectedValue = oItem.IdNomencladorForense.ToString();

                    txtPrecio.Text = oItem.Precio.ToString();
                    txtCantidad.Enabled = true;
                    ddlProtocoloFiliacion.Enabled = true;
                    btnAgregar.Enabled = true;
                }
                else
                {
                    lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no existe. ";
                    ddlItem.SelectedValue = "0";
                    txtCodigo.Text = "";
                    ddlProtocoloFiliacion.SelectedValue = "0";
                    txtCantidad.Enabled = false;
                    ddlProtocoloFiliacion.Enabled = false;
                    btnAgregar.Enabled = false;
                    txtPrecio.Text = "";
                    txtCodigo.UpdateAfterCallBack = true;

                }

                ddlItem.UpdateAfterCallBack = true;
                ddlProtocoloFiliacion.UpdateAfterCallBack = true;
                txtCantidad.UpdateAfterCallBack = true;
                btnAgregar.UpdateAfterCallBack = true;
                txtPrecio.UpdateAfterCallBack = true;
                //   txtAncho.UpdateAfterCallBack = true;
                lblMensaje.UpdateAfterCallBack = true;
            }
            else
            {
                ddlItem.SelectedValue = "0";
                ddlProtocoloFiliacion.SelectedValue = "0";
                txtCantidad.Text = "";
                txtPrecio.Text = "";
                ddlItem.UpdateAfterCallBack = true;
                ddlProtocoloFiliacion.UpdateAfterCallBack = true;
                btnAgregar.UpdateAfterCallBack = true;
                txtPrecio.UpdateAfterCallBack = true;
                //  lblMensaje.UpdateAfterCallBack = true;
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Agregar();
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                {
                    if (dtDeterminaciones.Rows[i][7].ToString() == e.CommandArgument.ToString())
                    {
                        dtDeterminaciones.Rows[i].Delete();
                    }
                }
                Session.Add("Tabla1", dtDeterminaciones);
                ReNumerar();
                gvLista.DataSource = dtDeterminaciones;
                gvLista.DataBind();
                gvLista.UpdateAfterCallBack = true;

                CalcularTotal();

            }
        }

        private void ReNumerar()
        {
            /////Crea nuevamente los detalles.
            for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
            {
                int nrofila = i + 1;
                dtDeterminaciones.Rows[i][7] = nrofila.ToString();

            }
        }

        private void InicializarTablas()
        {
            ///Inicializa las sesiones para las tablas de diagnosticos y de determinaciones
            if (Session["Tabla1"] != null) Session["Tabla1"] = null;

            dtDeterminaciones = new DataTable();


            dtDeterminaciones.Columns.Add("idNomenclador");

            dtDeterminaciones.Columns.Add("codigo");
            dtDeterminaciones.Columns.Add("nombre");
            dtDeterminaciones.Columns.Add("cantidad");
            dtDeterminaciones.Columns.Add("precio");
            dtDeterminaciones.Columns.Add("total");
            dtDeterminaciones.Columns.Add("eliminar");
            dtDeterminaciones.Columns.Add("fila");
            dtDeterminaciones.Columns.Add("iddetallepresupuesto");


            Session.Add("Tabla1", dtDeterminaciones);


        }
        private void Agregar()
        {
            ///Agregar a la tabla las determinaciones para mostrarlas en el gridview


            if (ddlItem.SelectedValue != "0")
            {
                int fila = gvLista.Rows.Count + 1;
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                //Primero verifica que no exista el item en la lista
                //for (int i = 0; i < dtDeterminaciones.Rows.Count; i++)
                //{
                //    if (txtCodigo.Text.Trim() == dtDeterminaciones.Rows[i][0].ToString())
                //        existe = true;
                //}
                //if (!existe)
                //{
                DataRow row = dtDeterminaciones.NewRow();
                //dtDeterminaciones.Columns.Add("idNomenclador");
                //dtDeterminaciones.Columns.Add("codigo");
                //dtDeterminaciones.Columns.Add("nombre");
                //dtDeterminaciones.Columns.Add("cantidad");
                //dtDeterminaciones.Columns.Add("precio");
                //dtDeterminaciones.Columns.Add("total");
                decimal total = decimal.Parse(txtPrecio.Text) * decimal.Parse(txtCantidad.Text);
                row[0] = ddlItem.SelectedValue;
                row[1] = txtCodigo.Text;
                row[2] = ddlProtocoloFiliacion.SelectedItem.Text;
                row[3] = txtCantidad.Text;
                row[4] = txtPrecio.Text;
                row[5] = total.ToString();
                row[7] = fila.ToString();
                row[8] = "0";
                dtDeterminaciones.Rows.Add(row);

                Session.Add("Tabla1", dtDeterminaciones);
                gvLista.DataSource = dtDeterminaciones;
                gvLista.DataBind();
                //}
                //else
                //{
                //    lblPaciente.Text = "El protocolo ya fue ingresado a la lista";
                //    lblPaciente.ForeColor = System.Drawing.Color.Red;
                //}
                btnAgregar.Enabled = false; btnAgregar.UpdateAfterCallBack = true;
                gvLista.UpdateAfterCallBack = true;

                txtCodigo.Text = ""; txtPrecio.Text = ""; ddlProtocoloFiliacion.SelectedValue = "0"; txtCantidad.Text = "";
                ddlItem.SelectedValue = "0";
                txtPrecio.UpdateAfterCallBack = true;
                txtCodigo.UpdateAfterCallBack = true;
                ddlProtocoloFiliacion.UpdateAfterCallBack = true;
                ddlItem.UpdateAfterCallBack = true;
                txtCantidad.UpdateAfterCallBack = true;
                CalcularTotal();

            }
        }

        private void CalcularTotal()
        {
            decimal totalPresupuesto = 0;
            decimal cant = 0; decimal precio;
            foreach (GridViewRow row in gvLista.Rows)
            {



                cant = decimal.Parse(row.Cells[3].Text);

                precio = decimal.Parse(row.Cells[4].Text);

                totalPresupuesto = totalPresupuesto + (cant * precio);
            }

            txtTotal.Text = totalPresupuesto.ToString();
            txtTotal.UpdateAfterCallBack = true;
        }
        protected void gvLista_RowDataBound1(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                LinkButton CmdEliminar = (LinkButton)e.Row.Cells[6].Controls[1];
                CmdEliminar.CommandArgument = dtDeterminaciones.Rows[e.Row.RowIndex][7].ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

                if (dtDeterminaciones.Rows[e.Row.RowIndex][8].ToString() != "0")
                {
                    string idp = dtDeterminaciones.Rows[e.Row.RowIndex][8].ToString();
                    DetallePresupuesto oDet = new DetallePresupuesto();
                    oDet = (DetallePresupuesto)oDet.Get(typeof(DetallePresupuesto), "IdDetallePresupuesto", int.Parse(idp));
                    if (oDet.Prefacturado)
                    {
                        CmdEliminar.Visible = false;

                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].Font.Bold = true;
                            e.Row.Cells[i].BackColor = System.Drawing.Color.LightGray;
                            //#f7ecdc
                        }
                    }
                }

                //if (Permiso == 1)
                //{
                //    CmdEliminar.Visible = false;
                //}

            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Factura oRegistro = new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura), "IdCasoFiliacion", int.Parse(lblCaso.Text), "Baja", false);
            if (oRegistro != null)
                imprimirFactura(oRegistro);

        }

        protected void btnAnular_Click(object sender, EventArgs e)
        {
            if (Request["idCaso"] != null)
            {
                lblCaso.Text = Request["idCaso"].ToString();
                AnularFacturaFiliacion();

            }
        }

        private void AnularFacturaFiliacion()
        {
            int i = 0;
            ISession m_session = NHibernateHttpModule.CurrentSession;

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(lblCaso.Text));

            /// Borra vinculacion del caso con el presupuesto
            Business.Data.Facturacion.CasoPresupuesto oDetalle1 = new Business.Data.Facturacion.CasoPresupuesto();
            ICriteria crit1 = m_session.CreateCriteria(typeof(Business.Data.Facturacion.CasoPresupuesto));
            crit1.Add(Expression.Eq("IdCasoFiliacion", oRegistro));


            IList items1 = crit1.List();

            foreach (Business.Data.Facturacion.CasoPresupuesto oDet in items1)
            {

                oDet.Delete();
            }


            /// Borra  presupuesto y detallepresupuesto

            int idp = 0;
            DetallePresupuesto oDetalle = new DetallePresupuesto();
            ICriteria crit = m_session.CreateCriteria(typeof(DetallePresupuesto));
            crit.Add(Expression.Eq("IdCasoFiliacion", int.Parse(lblCaso.Text)));
            IList items = crit.List();
            foreach (DetallePresupuesto oDet in items)
            {
                idp = oDet.IdPresupuesto.IdPresupuesto;
                oDet.Delete();
            }

            /// presupeusto
            Business.Data.Facturacion.Presupuesto OP= new Business.Data.Facturacion.Presupuesto();
            OP = (Business.Data.Facturacion.Presupuesto)OP.Get(typeof(Business.Data.Facturacion.Presupuesto), idp);
            if (OP != null)
                OP.Delete();

            /// borro la factura


            Factura oFactura = new Factura();
            oFactura = (Factura)oFactura.Get(typeof(Factura), "IdCasoFiliacion", int.Parse(lblCaso.Text));
            oFactura.Delete();

        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            if (Request["Desde"] == "CasoList")
                Response.Redirect("../CasoList.aspx", false);
            if (Request["Desde"] == "FacturaList")

                Response.Redirect("FacturaList.aspx", false);
        }
    }
}