using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using Business.Data.GenMarcadores;

namespace WebLab.CasoFiliacion
{
    public partial class BaseGenMenu : System.Web.UI.Page
    {
        /*Menu principal que lleva a nuevo caso y a lista de casos*/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)

            {
                VerificaPermisos("Casos Forense");

                CargarTablas();

            }
        }
        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnImportar.Enabled = false;
                            btnImportar.Visible = false;
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }



        protected void btnImportar_Click(object sender, EventArgs e)
        {
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "6");
            Server.Transfer("GenMarcadoresEdit.aspx");


        }

        private void CargarTablas()
        {


            gvBaseGen.DataSource = LeerGEN();
            gvBaseGen.DataBind();
        }

        private object LeerGEN()
        {
            string m_strSQL = @"select distinct P.idProtocolo,  P.numero, Pac.apellido + ' ' + Pac.nombre as paciente, P.edad, P.sexo
 from Gen_Marcadores  as CM
inner join LAB_Protocolo as P on p.idProtocolo= CM.idProtocolo
inner join Sys_Paciente as Pac on Pac.idPaciente= P.idPaciente   ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            estatus.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }

        protected void btnCargarTablas_Click(object sender, EventArgs e)
        {
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "6");
            Server.Transfer("ImportarMarcadores.aspx");
        }

        protected void btnResultados_Click(object sender, EventArgs e)
        {
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "6");
            Server.Transfer("GenResultados.aspx");
        }

        protected void gvBaseGen_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Anular":
                    {
                        Anular(e.CommandArgument);
                        // PintarReferencias();
                        break;
                    }
            }
        }

        private void Anular(object commandArgument)
        {
            //Protocolo oRegistro = new Protocolo();
            //oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(commandArgument.ToString()));

            int idprot = int.Parse(commandArgument.ToString());

            if (!ProtocoloIncluidoenProceso(idprot))
            {


                ISession m_session = NHibernateHttpModule.CurrentSession;

                //verifica que ingrese solo el protocolo del caso
                Marcadores oCasoMarcadores = new Marcadores();
                ICriteria crit = m_session.CreateCriteria(typeof(Marcadores));
                crit.Add(Expression.Eq("IdProtocolo", idprot));

                IList detalle = crit.List();
                foreach (Marcadores oDetalle in detalle)
                {
                    oDetalle.Delete();
                } //for    
                CargarTablas();
            }
            else
                estatus.Text = " No es posible eliminar. Esta vinculado a procesos";

        }

        public bool ProtocoloIncluidoenProceso(int id)
        {

            string m_strSQL = @"select *  from GEN_ProcesoFrecuenciasProtocolos where idProtocolo=  " +id.ToString();

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            if (Ds.Tables[0].Rows.Count > 0)
                return true;
            else return false;
        }


        protected void gvBaseGen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {



                if (e.Row.RowType == DataControlRowType.DataRow)
                {



                    LinkButton CmdEliminar = (LinkButton)e.Row.Cells[4].Controls[1];
                    CmdEliminar.CommandArgument = this.gvBaseGen.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Anular";
                    CmdEliminar.ToolTip = "Anular";



                    if (Permiso == 1)
                    {
                        CmdEliminar.Visible = false;

                    }




                }


            }
        }
    }
}
