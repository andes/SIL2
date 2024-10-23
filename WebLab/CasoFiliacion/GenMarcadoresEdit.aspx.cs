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
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.Services;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Text.RegularExpressions;
using Business.Data.GenMarcadores;

namespace WebLab.CasoFiliacion
{
    public partial class GenMarcadoresEdit : System.Web.UI.Page
    {

    

        private enum TabIndex
        {
            DEFAULT = 1,
            ONE = 2,


            // you can as many as you want here
        }
        //private void SetSelectedTab(TabIndex tabIndex)
        //{
        //    HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)

            {


                VerificaPermisos("Casos Forense");
                CargarTablas();
            }


        }

        private void CargarTablas()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();

            gvBaseGen.DataSource = LeerGEN();
            gvBaseGen.DataBind();
        }

        private object LeerGEN()
        {
            string m_strSQL = @"select distinct P.numero as [Protocolo], Pac.apellido + ' ' + Pac.nombre as Paciente, P.edad as Edad, P.sexo as Sexo
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
                            btnAgregar.Visible = false;
                            //btnAgregar.Visible = false;
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }








       
      
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);

        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);

        }
        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }
       

        private object LeerDatos()       {

            string strcondicion = "  1=1";
            if (Context.Items["id"] != null)
                strcondicion = "  CFP.idCasoFiliacion=" + Context.Items["id"].ToString();

            string m_strSQL = @"select distinct P.idProtocolo, P.numero, Pac.apellido + ' ' + Pac.nombre as pacientep, P.edad, P.sexo , Pa.nombre as Parentesco, CFP.idCasoFiliacion as Caso, 'Pendiente' as Estado
 from LAB_CasoMarcadores  as CM
 
inner join LAB_Protocolo as P on p.idProtocolo= CM.idProtocolo
inner join LAB_CasoFiliacionProtocolo CFP on CFP.idProtocolo=P.idprotocolo
inner join LAB_Parentesco as Pa on Pa.idParentesco= CFP.idTipoParentesco
inner join Sys_Paciente as Pac on Pac.idPaciente= P.idPaciente where P.idPaciente<>-1 and P.idProtocolo not in (select idProtocolo from gen_marcadores)  
and P.idProtocolo not in (select idProtocolo from gen_marcadoresexcluidos) and " + strcondicion ;


            if (chkExcluidos.Checked)
                m_strSQL += @" union 
select distinct P.idProtocolo, P.numero, Pac.apellido + ' ' + Pac.nombre as pacientep, P.edad, P.sexo , Pa.nombre as Parentesco, CFP.idCasoFiliacion as Caso,  'Excluido' as Estado
 from LAB_CasoMarcadores  as CM
 
inner join LAB_Protocolo as P on p.idProtocolo= CM.idProtocolo
inner join LAB_CasoFiliacionProtocolo CFP on CFP.idProtocolo=P.idprotocolo
inner join LAB_Parentesco as Pa on Pa.idParentesco= CFP.idTipoParentesco
inner join Sys_Paciente as Pac on Pac.idPaciente= P.idPaciente where  P.idProtocolo in (select idProtocolo from gen_marcadoresexcluidos)
and P.idProtocolo not in (select idProtocolo from gen_marcadores) and " + strcondicion ;

                DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        


        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));


                    ISession m_session = NHibernateHttpModule.CurrentSession;

                    //verifica que ingrese solo el protocolo del caso
                    CasoMarcadores oCasoMarcadores = new CasoMarcadores();
                    ICriteria crit = m_session.CreateCriteria(typeof(CasoMarcadores));
                    crit.Add(Expression.Eq("IdProtocolo", oProtocolo.IdProtocolo));

                    IList detalle = crit.List();
                    foreach (CasoMarcadores oDetalle in detalle)
                    {
                        Marcadores oRegistro = new Marcadores();

                        oRegistro.IdProtocolo = oDetalle.IdProtocolo;
                        oRegistro.IdPaciente = oProtocolo.IdPaciente.IdPaciente;
                        oRegistro.Marcador = oDetalle.Marcador;
                        oRegistro.Allello1 = oDetalle.Allello1;
                        oRegistro.Allello2 = oDetalle.Allello2;

                        oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                        oRegistro.FechaRegistro = DateTime.Now;

                        oRegistro.Save();
                    } //for                
                } // cheked
            }// for

            CargarTablas();
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "onMouseOver('" + (e.Row.RowIndex + 1) + "')";
                e.Row.Attributes["onmouseout"] = "onMouseOut('" + (e.Row.RowIndex + 1) + "')";
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));



                        MarcadoresExcluidos oRegistro = new MarcadoresExcluidos();

                        oRegistro.IdProtocolo = oProtocolo.IdProtocolo;
                        oRegistro.IdPaciente = oProtocolo.IdPaciente.IdPaciente;

                    oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                    oRegistro.FechaRegistro = DateTime.Now;

                        oRegistro.Save();
                   
                             
                } // cheked
            }// for

            CargarTablas();
        }

        protected void chkExcluidos_CheckedChanged(object sender, EventArgs e)
        {
            
                CargarTablas();

        }
    }
     
}
