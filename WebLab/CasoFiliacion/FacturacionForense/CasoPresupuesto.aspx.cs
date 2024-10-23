using Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class CasoPresupuesto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["idCaso"] != null)
                {
                    Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                    oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Request["idCaso"].ToString()));

                    lblCaso.Text = Request["idCaso"].ToString();
                    lblNombre.Text = oCaso.Nombre;
                    if (oCaso.Prefacturado())
                    { btnGuardar.Visible = false;
                       
                    }
                    
                       
                }
                CargarGrilla();
            }
        }
        private void CargarGrilla()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            //PonerContadores();

        }
      


       

        private object LeerDatos()
        { 
            string m_strSQL = @"SELECT P.idPresupuesto, P.fecha,P.nombre AS nombre, CASE WHEN C.idCasoFiliacion ="+Request["idCaso"].ToString()+ @" THEN 1 ELSE 0 END AS caso
                         FROM LAB_Presupuesto AS P 
                         LEFT OUTER JOIN  LAB_CasoPresupuesto AS C ON C.idPresupuesto = P.idPresupuesto
						 LEFT OUTER JOIN  LAB_Factura as F on F.idcasofiliacion= C.idCasoFiliacion 
						 where P.baja=0 and (C.idCasoFiliacion="+ Request["idCaso"].ToString() + @" or isnull(F.idFactura,0)=0 )";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }
        public void BorrarPresupuestos(Business.Data.Laboratorio.CasoFiliacion oCaso)
        {
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Facturacion.CasoPresupuesto));
            crit.Add(Expression.Eq("IdCasoFiliacion", oCaso));
            IList detalle = crit.List();

            foreach (Business.Data.Facturacion.CasoPresupuesto oDetalle in detalle)
            {
                oDetalle.Delete();
            }

        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Request["idCaso"].ToString()));
            BorrarPresupuestos(oCaso);

            
           
            foreach (GridViewRow row in gvLista.Rows)
            {

           
                CheckBox a = ((CheckBox)(row.Cells[3].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                                      
                    Business.Data.Facturacion.Presupuesto oP = new Business.Data.Facturacion.Presupuesto();
                    oP = (Business.Data.Facturacion.Presupuesto)oP.Get(typeof(Business.Data.Facturacion.Presupuesto), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    Business.Data.Facturacion.CasoPresupuesto oPro = new Business.Data.Facturacion.CasoPresupuesto();


                    oPro.IdPresupuesto = oP;
                    oPro.IdCasoFiliacion = oCaso;
                    oPro.Save();

                    oCaso.GrabarAuditoria("Vincula a presupuesto", int.Parse(Session["idUsuario"].ToString()), oP.IdPresupuesto.ToString());

                }//checked
            }// grid


            /// Debe ir a Default2.aspx con la restriccion de
            if (Request["Desde"].ToString()=="NuevoCaso")
            Response.Redirect("../../Protocolos/Default2.aspx?idServicio=6&idUrgencia=0&idCaso=" + oCaso.IdCasoFiliacion.ToString() + "&idTipoCaso=" + oCaso.IdTipoCaso.ToString(), false);
            if (Request["Desde"].ToString() == "Lista")
            {
                Context.Items.Add("idServicio",6);
                Server.Transfer("../CasoList.aspx");
            }
            //estatus.Text = "se han guardado " + i.ToString() + " registros";

        }

      

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }
    }
}