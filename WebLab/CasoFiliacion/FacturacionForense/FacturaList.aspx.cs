using Business;
using Business.Data.Facturacion;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.CasoFiliacion.FacturacionForense
{
    public partial class FacturaList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //InicializarTablas();
                //VerificaPermisos("Hoja de Trabajo Edit");
                CargarGrilla();
                //if (Request["id"] != null)
                //    MostrarDatos();

            }
        }


      

        protected void ddlCasoCerrado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        private void CargarGrilla()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            //PonerContadores();

        }




        private object LeerDatos()
        {
            string m_condicion = " ";
          
            if (ddlTipo.SelectedValue == "1") m_condicion = " and p.idfactura=0";
            if (ddlTipo.SelectedValue == "2") m_condicion = " and p.idfactura>0";


            string m_strSQL = @"select distinct F.idCasoFiliacion, convert(varchar,F.idCasoFiliacion)+' ' + F.nombre as nombre ,isnull(fac.numero,0) as factura, sum (P.totalPrefactura) as TotalPrefactura ,
U.apellido as usuario,fac.fechaRegistro
from lab_Casofiliacion F
  inner join LAB_CasoPresupuesto as C on C.idCasoFiliacion= F.idCasoFiliacion
inner join  LAB_detallePresupuesto  as P on P.idPresupuesto=  C.idPresupuesto and C.idCasoFiliacion= P.idCasoFiliacion
left join lab_factura   fac on fac.idfactura= P.idfactura and fac.baja=0 
left join sys_usuario u on u.idUsuario= fac.idUsuarioRegistro
  where prefacturado=1 " + m_condicion + " group by F.idCasoFiliacion,   F.nombre ,fac.numero , U.apellido  ,fac.fechaRegistro ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }

       
       

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                string[] arr = e.CommandArgument.ToString().Split((";").ToCharArray());

                if (e.CommandName == "Factura")

                {


                    Response.Redirect("Facturaedit.aspx?idCaso=" + arr[0].ToString() + "&Total="+ arr[1].ToString()  +"&Desde=FacturaList", false);
                }

                if (e.CommandName == "Anular")
                    EliminarFactura(arr[0].ToString());

            }
        }

        private void EliminarFactura(string idcaso)
        {
            Factura oRegistro = new Factura();
            oRegistro = (Factura)oRegistro.Get(typeof(Factura),"IdCasoFiliacion", int.Parse(idcaso));

            oRegistro.Baja = true;
            oRegistro.Save();

            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria critMarcadores = m_session.CreateCriteria(typeof(DetallePresupuesto));
            critMarcadores.Add(Expression.Eq("IdFactura",oRegistro.IdFactura));

            IList detalle = critMarcadores.List();
             
                foreach (DetallePresupuesto oDetalle in detalle)
                {
                oDetalle.IdFactura = 0;
                oDetalle.Save();
                }
            Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
            oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(idcaso));
            oCaso.GrabarAuditoria("Anula Factura", int.Parse(Session["IdUsuario"].ToString()), oRegistro.Numero);
            CargarGrilla();

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string total = e.Row.Cells[2].Text;
                Button CmdCaso = (Button)e.Row.Cells[6].Controls[1];
                CmdCaso.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString() + ";" + total;
                CmdCaso.CommandName = "Factura";
                CmdCaso.ToolTip = "Factura";

                Button CmdEliminar = (Button)e.Row.Cells[7].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString() + ";" + total;
                CmdEliminar.CommandName = "Anular";
                CmdEliminar.ToolTip = "Anular";

                string f = e.Row.Cells[3].Text;
                if (f == "0")
                    CmdEliminar.Visible = false;
            }
        }
    }
}