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

using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using System.Drawing;

namespace WebLab.Neonatal
{
    public partial class SolicitudList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                VerificaPermisos("Pesquisa Neonatal");
                CargarListas();
                   txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                    // llenarCombos(us);
                    CargarGrilla();
                 
                }
            
            

        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                      Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnNueva.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
          //  Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            string m_ssql = "SELECT idZona, nombre FROM sys_zona where idzona>1";
            oUtil.CargarCombo(ddlZona, m_ssql, "idZona", "nombre");
            ddlZona.Items.Insert(0, new ListItem("Todas", "0"));


            ////////////////////////////////////Carga de combos de Efector: Solo Hospitales////////////////////////////////////
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector WHERE (complejidad > 2) AND (complejidad < 7) order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));

           



            m_ssql = null;
            oUtil = null;
        }
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            PintarReferencias();
        }



        private object LeerDatos()
        {
            string m_strCondicion = " WHERE 1=1";

            if (txtFechaDesde.Value != "")
            {
                DateTime fini = Convert.ToDateTime(txtFechaDesde.Value);
                m_strCondicion += " and S.fechaRegistro>='" + fini.ToString("yyyyMMdd") + "'";
            }

            if (txtFechaHasta.Value != "")
            {
                DateTime ffin = Convert.ToDateTime(txtFechaHasta.Value);
                m_strCondicion += " and S.fechaRegistro<='" + ffin.ToString("yyyyMMdd") + "'";
            }

            
            //{
            if (ddlZona.SelectedValue!="0")                    m_strCondicion += " and E.idZona=" + ddlZona.SelectedValue;
            //}
            //else
            if (ddlEfector.SelectedValue!="0")                    m_strCondicion += " and S.idEfectorSolicitante=" + ddlEfector.SelectedValue;

      //      if (txtNumeroSolicitud.Value != "") m_strCondicion += " and S.idSolicitudScreening=" + txtNumeroSolicitud.Value;

            string m_strSQL = @"  SELECT S.idSolicitudScreening AS Numero, E.nombre AS Efector, Pac.numeroDocumento AS [DNI RN], S.apellidoPaterno AS [Apellido Paterno], 
                                  Pac.fechaNacimiento AS [Fecha Nac.], S.horaNacimiento AS [Hora Nac.], Par.numeroDocumento AS [DNI Madre], S.fechaRegistro AS [Fecha Registro], 
                                  U.username AS Usuario, Par.apellido AS [Apellido Materno], dbo.numeroProtocolo(Pro.idProtocolo) as Protocolo, 
                                  CASE WHEN S.numeroOrigen=0 THEN '' ELSE S.numeroOrigen end as numeroOrigen 
                                  FROM         LAB_SolicitudScreening AS S INNER JOIN
                                  Sys_Efector AS E ON S.idEfectorSolicitante = E.idEfector INNER JOIN
                                  Sys_Paciente AS Pac ON S.idPaciente = Pac.idPaciente INNER JOIN
                                  Sys_Usuario AS U ON S.idUsuarioRegistro = U.idUsuario LEFT OUTER JOIN
                                  Lab_Protocolo AS Pro ON S.idProtocolo = Pro.idProtocolo LEFT OUTER JOIN
                                  Sys_Parentesco AS Par ON Pac.idPaciente = Par.idPaciente
                                  " + m_strCondicion;


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);         

            return Ds.Tables[0];
        }

        

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlZona_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    CargarEfectores();
        }

        private void PintarReferencias()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                if (row.Cells[11].Text != "&nbsp;")
                {
                    for (int j = 0; j < gvLista.Columns.Count; j++)
                    { row.Cells[j].BackColor = Color.LightYellow; }
                }               
            }
        }


        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar") Response.Redirect("IngresoEdit.aspx?idSolicitud=" + e.CommandArgument, false);
            if (e.CommandName == "Protocolo")
            {
         
                SolicitudScreening oRegistro = new SolicitudScreening();
                oRegistro = (SolicitudScreening)oRegistro.Get(typeof(SolicitudScreening), int.Parse(e.CommandArgument.ToString()));
                int idProtocolo = oRegistro.IdProtocolo;
                if (idProtocolo==0)///alta de protocolo
                    Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&Operacion=Alta&idServicio=1&idSolicitudScreening=" + oRegistro.IdSolicitudScreening.ToString());
                else/// modificar protocolo
                    Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idServicio=1&Operacion=Modifica&idProtocolo=" + idProtocolo.ToString() + "&idSolicitudScreening=" + oRegistro.IdSolicitudScreening.ToString());
            }
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdSolicitud= (ImageButton)e.Row.Cells[12].Controls[1];
                CmdSolicitud.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdSolicitud.CommandName = "Editar";
                CmdSolicitud.ToolTip = "Editar";

                ImageButton CmdProtocolo= (ImageButton)e.Row.Cells[13].Controls[1];
                CmdProtocolo.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString() ;
                CmdProtocolo.CommandName = "Protocolo";
                CmdProtocolo.ToolTip = "Protocolo";
            }
        }

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx", false);
        }
    }
}
