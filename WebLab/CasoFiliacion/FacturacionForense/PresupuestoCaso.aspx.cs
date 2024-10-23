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
using Business.Data.Facturacion;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class PresupuestoCaso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["id"] != null)
                {
                    Presupuesto oPresupuesto = new Presupuesto();
                    oPresupuesto = (Presupuesto)oPresupuesto.Get(typeof(Presupuesto), int.Parse(Request["id"].ToString()));

                    lblPresupuesto.Text = oPresupuesto.IdPresupuesto.ToString();
                    lblNombre.Text = oPresupuesto.Nombre;
                    CargarGrilla();
                }
               
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
             
             

            string m_strSQL = @"SELECT   C.idCasoFiliacion,convert(varchar(10), C.fechaRegistro,103) as fecha, C.nombre  , F.numero as factura
FROM            LAB_CasoFiliacion AS C inner  JOIN
                         LAB_CasoPresupuesto AS P ON C.idCasoFiliacion	 = P.idCasoFiliacion
						 left join LAB_Factura as F on F.idCasoFiliacion= C.idCasoFiliacion
						 where P.idPresupuesto=  " + lblPresupuesto.Text;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }
    

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("PresupuestoList.aspx", false);
        }
    }
}