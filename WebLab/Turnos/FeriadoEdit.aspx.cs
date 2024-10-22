using Business;
using Business.Data.Laboratorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Turnos
{
    public partial class FeriadoEdit1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cldTurno.SelectedDate = DateTime.Now;
                lblFecha.Text = DateTime.Now.ToShortDateString(); ActualizarGrilla();
            }
        }

        public void Mostrar()
        {
            //lblFecha.Text = DateTime.Now.ToShortDateString(); cldTurno.SelectedDate = DateTime.Now; 
            ActualizarGrilla();
        }

        protected void cldTurno_SelectionChanged(object sender, EventArgs e)
        {
            lblFecha.Text = cldTurno.SelectedDate.Date.ToShortDateString();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { Guardar(); }
        }

        private void Guardar()
        {
            Feriado oRegistro = new Feriado();
            oRegistro.Fecha = cldTurno.SelectedDate.Date;
            oRegistro.Save();
            ActualizarGrilla();
        }

        private void ActualizarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {

            string m_strSQL = @" SELECT    idFeriado, CONVERT(varchar(10), fecha, 103) AS fecha1

 FROM  Lab_Feriado with (nolock)  order by fecha";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            return Ds.Tables[0];
        }
        private void Anular(object p)
        {

            Feriado oRegistro = new Feriado();
            oRegistro = (Feriado)oRegistro.Get(typeof(Feriado), int.Parse(p.ToString()));
            oRegistro.Delete();
             ActualizarGrilla();

        }
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());


            string m_strSQL = " SELECT    idFeriado, fecha FROM  Lab_Feriado with (nolock)  WHERE fecha = '" + fecha.ToString("yyyyMMdd") + "'";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            if (Ds.Tables[0].Rows.Count > 0) args.IsValid = false;
            else
                args.IsValid = true;
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")

                Anular(e.CommandArgument);

        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {



                    ImageButton CmdEliminar = (ImageButton)e.Row.Cells[1].Controls[1];
                    CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Eliminar";
                    CmdEliminar.ToolTip = "Eliminar";
                }
            }
        }

    }
}