using Business;
using Business.Data;
using Business.Data.Facturacion;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class PresupuestoEdit : System.Web.UI.Page
    {
       
        DataTable dtDeterminaciones; //tabla para determinaciones
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { InicializarTablas();
                //VerificaPermisos("Hoja de Trabajo Edit");
                txtFecha.Value = DateTime.Now.ToShortDateString();
                CargarListas();
                btnImprimir.Visible = false;
                if (Request["id"] != null)
                {
                    MostrarDatos();
                }
               
            }

        }

        private void MostrarDatos()
        {
            bool facturado = false;
                        Presupuesto oRegistro = new Presupuesto();
            oRegistro = (Presupuesto)oRegistro.Get(typeof(Presupuesto), int.Parse(Request["id"].ToString()));

            nroPresupuesto.Text = "Nro. " + oRegistro.IdPresupuesto.ToString(); nroPresupuesto.Visible = true;
            txtFecha.Value = oRegistro.Fecha.ToShortDateString();
            txtEncabezado1.Text = oRegistro.Encabezado1;
            txtEncabezado2.Text = oRegistro.Encabezado2;
            txtPie.Text = oRegistro.Pie;
            txtNombrePresupuesto.Text = oRegistro.Nombre;
            ddlSectorServicio.SelectedValue = oRegistro.IdServicio.ToString();

            dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            DetallePresupuesto oDetalle = new DetallePresupuesto();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(DetallePresupuesto));
            crit.Add(Expression.Eq("IdPresupuesto", oRegistro));


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

                if (oDet.Cantidadprefacturado > 0)
                    facturado = true;
                dtDeterminaciones.Rows.Add(row);

            }
            Session.Add("Tabla1", dtDeterminaciones);
            gvLista.DataSource = dtDeterminaciones;
            gvLista.DataBind();
            CalcularTotal();
            btnImprimir.Visible = true;
          
            if (facturado)
            {
                btnAgregar.Visible = false;
                btnGuardar.Visible = false;
                lblEstado.Visible = true;
                lblEstado.Text = "Proceso de facturación iniciado. No es posible modificar.";
            }
            else
                lblEstado.Visible = false; 
             
            //oRegistro.GrabarAuditoria("Consulta Caso", int.Parse(Session["idUsuario"].ToString()),"");

        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = "select idNomencladorForense as idItem, nombre from LAB_NomencladorForense where baja=0  order by nombre";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;


            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            m_ssql = "SELECT  idSectorServicio,  nombre   as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";

            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("Seleccione", "0"));
           

            m_ssql = null;
            oUtil = null;
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
                    txtNombre.Enabled = true;
                    btnAgregar.Enabled = true;
                }
                else
                {
                    lblMensaje.Text = "El codigo " + txtCodigo.Text.ToUpper() + " no existe. ";
                    ddlItem.SelectedValue = "0";
                    txtCodigo.Text = "";
                    txtNombre.Text = "";
                    txtCantidad.Enabled = false;
                    txtNombre.Enabled = false;
                    btnAgregar.Enabled = false;
                    txtPrecio.Text = "";
                    txtCodigo.UpdateAfterCallBack = true;

                }

                ddlItem.UpdateAfterCallBack = true;
                txtNombre.UpdateAfterCallBack = true;
                txtCantidad.UpdateAfterCallBack = true;
                btnAgregar.UpdateAfterCallBack = true;
                txtPrecio.UpdateAfterCallBack = true;
                //   txtAncho.UpdateAfterCallBack = true;
                lblMensaje.UpdateAfterCallBack = true;
            }
            else
            {
                ddlItem.SelectedValue = "0";
                txtNombre.Text = "";
                txtCantidad.Text = "";
                txtPrecio.Text = "";
                ddlItem.UpdateAfterCallBack = true;
                txtNombre.UpdateAfterCallBack = true;
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
                int fila=gvLista.Rows.Count+1;
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
                row[2] = txtNombre.Text;
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

                txtCodigo.Text = ""; txtPrecio.Text = ""; txtNombre.Text = ""; txtCantidad.Text = "";
                ddlItem.SelectedValue = "0";
                txtPrecio.UpdateAfterCallBack = true;
                txtCodigo.UpdateAfterCallBack = true;
                txtNombre.UpdateAfterCallBack = true;
                ddlItem.UpdateAfterCallBack = true;
                txtCantidad.UpdateAfterCallBack = true;
                CalcularTotal();

            }
        }

        private void CalcularTotal()
        {
            decimal totalPresupuesto = 0;
        decimal cant=0; decimal precio;
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

                if (dtDeterminaciones.Rows[e.Row.RowIndex][8].ToString()!="0")
                {
                    string idp = dtDeterminaciones.Rows[e.Row.RowIndex][8].ToString();
                    DetallePresupuesto oDet = new DetallePresupuesto();
                    oDet = (DetallePresupuesto)oDet.Get(typeof(DetallePresupuesto), "IdDetallePresupuesto",int.Parse( idp));
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
        protected void cvAnalisis_ServerValidate(object source, ServerValidateEventArgs args)
        {

            //if (TxtDatos.Value == "") args.IsValid = false;
            //else


            //    args.IsValid = true;

        }
        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue != "0")
            {
                NomencladorForense oItem = new NomencladorForense();
                oItem = (NomencladorForense)oItem.Get(typeof(NomencladorForense), int.Parse(ddlItem.SelectedValue));
                txtCodigo.Text = oItem.Codigo;
               
                txtPrecio.Text = oItem.Precio.ToString();
                txtNombre.Enabled = true;
                txtCantidad.Enabled = true;
                btnAgregar.Enabled = true;
            }
            else
            {
                txtCodigo.Text = "";
               
                txtPrecio.Text = "";
                txtNombre.Text = "";
                txtCantidad.Text = "";
                btnAgregar.Enabled = false;

            }
          
            txtCodigo.UpdateAfterCallBack = true;
            txtPrecio.UpdateAfterCallBack = true;
            txtNombre.UpdateAfterCallBack = true;
            txtCantidad.UpdateAfterCallBack = true;
            btnAgregar.UpdateAfterCallBack = true;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Presupuesto oRegistro = new Presupuesto();
                if (Request["id"] != null) oRegistro = (Presupuesto)oRegistro.Get(typeof(Presupuesto), int.Parse(Request["id"].ToString()));
                Guardar(oRegistro);
                 Response.Redirect("PresupuestoList.aspx", false);
            }
        }

        private void Guardar(Presupuesto oRegistro)
        {
        
            oRegistro.IdServicio =int.Parse( ddlSectorServicio.SelectedValue);
            oRegistro.Fecha = Convert.ToDateTime(txtFecha.Value);
            oRegistro.Encabezado1 = txtEncabezado1.Text;
            oRegistro.Encabezado2 = txtEncabezado2.Text;
            oRegistro.Pie = txtPie.Text;
            oRegistro.Nombre = txtNombrePresupuesto.Text;
            ///////////////////////////////////
            oRegistro.IdUsuarioRegistro =  int.Parse(Session["idUsuario"].ToString()) ;
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();


            GuardarDetalle(oRegistro);
           
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
                oDetalle.Cantidad =int.Parse( dtDeterminaciones.Rows[i][3].ToString());
                oDetalle.Precio = decimal.Parse(dtDeterminaciones.Rows[i][4].ToString());
                oDetalle.Total = decimal.Parse(dtDeterminaciones.Rows[i][5].ToString());
                oDetalle.Prefacturado = false;
                oDetalle.Cantidadprefacturado = 0;
                oDetalle.TotalPrefactura = 0;
                oDetalle.IdFactura = 0;
                oDetalle.Save();

                //oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
            }

        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir(int.Parse(Request["id"].ToString()));

        }

        private void Imprimir(int p )
        {

            Presupuesto oRegistro = new Presupuesto();
            oRegistro = (Presupuesto)oRegistro.Get(typeof(Presupuesto), p);


            CrystalReportSource oCr = new CrystalReportSource();

           
                        oCr.Report.FileName = "presupuesto.rpt";
            oCr.ReportDocument.SetDataSource(oRegistro.getInforme() );
                       

            oCr.DataBind();
            //if (Desde.Value == "Carga")
            //    oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuario"].ToString()), "Resultado_" + oRegistro.Nombre.Trim());
            //else
            //    oRegistro.GrabarAuditoria("Imprime Resultado", int.Parse(Session["idUsuarioValida"].ToString()), "Resultado_" + oRegistro.Nombre.Trim());



            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Presupuesto_" + oRegistro.IdPresupuesto.ToString());


        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PresupuestoList.aspx", false);
        }
    }
}