using Business;
using Business.Data.Facturacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data.Laboratorio;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class PrefacturaEdit : System.Web.UI.Page
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
                    Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                    oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(lblCaso.Text));

                   
                    lblNombre.Text = oCaso.Nombre;
                    estatus.Text = "";
                    CargarGrilla();
                }

                //    MostrarDatos();

            }
        }

         
        private void CargarGrilla()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            CalcularTotal();
            

        }




        private object LeerDatos()
        {



            string m_strSQL = @"select D.idDetallePresupuesto, N.codigo, D.descripcion as nombre, 
D.cantidadprefacturado,D.cantidad, D.precio, d.total,  d.prefacturado, D.totalPrefactura, isnull(D.idfactura ,0 ) as estado 
from LAB_DetallePresupuesto d 
inner join LAB_Presupuesto as P on P.idPresupuesto= d.idPresupuesto 
inner join  LAB_CasoPresupuesto c on c.idPresupuesto=d.idPresupuesto
inner join LAB_NomencladorForense N on N.idNomencladorForense= D.idNomencladorForense
where C.idCasoFiliacion= " + lblCaso.Text + @" and (d.idcasofiliacion=" + lblCaso.Text + @" or d.idcasofiliacion=0) ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GuardarPrefactura();

            }

        }

        private void GuardarPrefactura()
        {
            if (validaDatos())
            {
                Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(lblCaso.Text));
                bool prefact = false;
           
                foreach (GridViewRow row in gvLista.Rows)
                {

                    TextBox t = ((TextBox)(row.Cells[3].FindControl("txtCantidadPrefactura")));
                    int cantidadp = int.Parse(t.Text);
                    if (cantidadp >= 0)
                    {



                        Business.Data.Facturacion.DetallePresupuesto oP = new Business.Data.Facturacion.DetallePresupuesto();
                        oP = (Business.Data.Facturacion.DetallePresupuesto)oP.Get(typeof(Business.Data.Facturacion.DetallePresupuesto), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
                        decimal totalp = decimal.Parse(cantidadp.ToString()) * oP.Precio;
                        oP.Cantidadprefacturado = cantidadp;
                        oP.Prefacturado = true;
                        oP.TotalPrefactura = totalp;
                        oP.IdCasoFiliacion = oCaso.IdCasoFiliacion;
                        oP.Save();
                        prefact = true;
                    }//checked
                }// grid

               

                if (prefact )
                    oCaso.GrabarAuditoria("Prefacturado", int.Parse(Session["idUsuario"].ToString()),txtTotal.Text);


                estatus.Text = "se han guardado correctamente";
                CargarGrilla();
                
            }else
            estatus.Text = "error en la cantidades ingresadas";
        }

       

        private bool validaDatos()
        {
            bool b = true;
            foreach (GridViewRow row in gvLista.Rows)
            {
                TextBox t = ((TextBox)(row.Cells[3].FindControl("txtCantidadPrefactura")));
                int cantidadp = int.Parse(t.Text);
                int cantidad = int.Parse( row.Cells[2].Text.ToString());
                // la cantidad a facturar no puede ser mayor a lo presupuestado.
                if (cantidadp > cantidad)
                {
                    b = false;
                    break;
                }//checked
            }// grid

            return b;
        }

        protected void btnCalcularTotal_Click(object sender, EventArgs e)
        {
            CalcularTotal();
        }

        private void CalcularTotal()
        {
            decimal totalPresupuesto = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {
                TextBox txtCantidad = (TextBox)row.Cells[3].Controls[1];

                decimal cant = decimal.Parse(txtCantidad.Text);

                decimal v = decimal.Parse(row.Cells[4].Text) * cant;

                totalPresupuesto = totalPresupuesto + v;
            }
            txtTotal.Text = totalPresupuesto.ToString();
            txtTotal.UpdateAfterCallBack = true;
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
         
        }



        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DetallePresupuesto oRegistro = new DetallePresupuesto();
                oRegistro = (DetallePresupuesto)oRegistro.Get(typeof(DetallePresupuesto), int.Parse(this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString()));
                if (oRegistro.IdCasoFiliacion == int.Parse(lblCaso.Text))
                {
                    if (oRegistro.Cantidadprefacturado > 0)
                    {
                        for (int i = 0; i < e.Row.Cells.Count; i++)
                        {

                            e.Row.Cells[i].Font.Bold = true;
                            e.Row.Cells[i].BackColor = System.Drawing.Color.LightGray;

                            //#f7ecdc
                        }

                        if (oRegistro.IdFactura > 0)
                        {
                            TextBox txtCantidad = (TextBox)e.Row.Cells[3].Controls[1];

                            txtCantidad.Enabled = false;
                            Factura oFactura = new Factura();
                            oFactura = (Factura)oFactura.Get(typeof(Factura), oRegistro.IdFactura);
                            e.Row.Cells[7].Text = oFactura.Numero;

                            btnGuardar.Visible = false;
                            btnCalcularTotal.Visible = false;
                        }
                        else { e.Row.Cells[7].Text = "Sin Facturar"; }
                    }

                } //if cas
                 
            }// if
        }//

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", 6);
            Server.Transfer("../CasoList.aspx");
        }
    }
}