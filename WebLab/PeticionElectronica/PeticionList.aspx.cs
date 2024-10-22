using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Drawing;

namespace WebLab.PeticionElectronica
{
    public partial class PeticionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Peticiones");
               
                //Session["idUrgencia"] = "0";
                txtFechaDesde.Value = DateTime.Now.AddDays(-1).ToShortDateString();
                txtFechaHasta.Value = DateTime.Now.ToShortDateString();
                CargarListas();
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

                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();         
            ///Carga de combos de Origen
            string m_ssql = "SELECT  idOrigen, nombre FROM LAB_Origen WHERE (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("-- Todos --", "0"));            

            ///Carga de Sectores
            m_ssql = "SELECT idSectorServicio,  nombre  + ' - ' + prefijo as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("-- Todos --", "0"));

            m_ssql = null;
            oUtil = null;
        }

        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();     
        }

        private object LeerDatos()
        {
            string str_condicion = " where 1=1  ";

            DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
            DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1); 
            
        
            if (ddlSectorServicio.SelectedValue != "0") str_condicion += " AND PE.idSector = " + ddlSectorServicio.SelectedValue;
            if (txtFechaDesde.Value != "") str_condicion += " AND PE.fecha>= '" + fecha1.ToString("yyyyMMdd") + "'"; //" AND convert(datetime,convert(varchar(10),PE.fecha,103))>= convert(datetime,convert(varchar(10),  '" + fecha1.ToString("yyyyMMdd") + "',103)) ";
            if (txtFechaHasta.Value != "") str_condicion += " AND PE.fecha<= '" + fecha2.ToString("yyyyMMdd") + "'";//" AND convert(datetime,convert(varchar(10),PE.fecha,103))<= convert(datetime,convert(varchar(10),  '" + fecha2.ToString("yyyyMMdd") + "',103)) ";
            if (txtNro.Text != "") str_condicion += " And PE.idPeticion='" + txtNro.Text + "'";
            if (ddlOrigen.SelectedValue != "0") str_condicion += " AND PE.idOrigen = " + ddlOrigen.SelectedValue;
            if (txtDni.Value != "") str_condicion += " AND Pac.numeroDocumento = '" + txtDni.Value + "'";
            if (txtApellido.Text != "") str_condicion += " AND Pac.apellido like '%" + txtApellido.Text.TrimEnd() + "%'";
            if (txtNombre.Text != "") str_condicion += " AND Pac.nombre like '%" + txtNombre.Text.TrimEnd() + "%'";
            switch (ddlEstado.SelectedValue)
            {
                case "-1": str_condicion += " AND PE.baja=0 "; break;
                case "0": str_condicion += " AND PE.idProtocolo=0 AND PE.baja=0 "; break;
                case "1": str_condicion += " AND PE.idProtocolo>0 AND PE.baja=0 "; break;
                case "2": str_condicion += " AND PE.baja=1 "; break;
            }
            //if (ddlEstado.SelectedValue == "0") str_condicion += " AND PE.idProtocolo=0 AND PE.baja=0 ";
            //if (ddlEstado.SelectedValue == "1") str_condicion += " AND PE.idProtocolo>0 AND PE.baja=0 ";
            //if (ddlEstado.SelectedValue == "2") str_condicion += " AND PE.baja=1 ";                 
          
            string m_strSQL = @"SELECT PE.idPeticion, Pac.numeroDocumento, Pac.apellido, Pac.nombre, CASE WHEN PE.idProtocolo > 0 THEN dbo.NumeroProtocolo(P.idProtocolo) 
           ELSE '-' END AS Protocolo, CONVERT(VARCHAR(10),PE.fecha,103) AS fecha, 
		       dbo.LAB_GetEstadoPeticion(PE.idPeticion) as estado, O.nombre AS origen, S.nombre as sector, PE.observaciones,
			  case when Pe.idEfector=S.idEfector then   Pro.apellido + ' ' + Pro.nombre else Ef.nombre end as solicitante, U.username as usuario 
			        FROM LAB_Peticion AS PE INNER JOIN 
					          Sys_Paciente AS Pac ON PE.idPaciente = Pac.idPaciente  INNER JOIN 
							       LAB_SectorServicio AS S ON PE.idSector = S.idSectorServicio INNER JOIN 
								      LAB_Origen AS O ON PE.idOrigen = O.idOrigen LEFT JOIN 
									      LAB_Protocolo AS P ON PE.idProtocolo = P.idProtocolo LEFT JOIN 
										    Sys_Profesional as Pro on Pro.idProfesional= PE.idSolicitante inner join 
											sys_efector as Ef on Ef.idEfector=Pe.idefector
											      inner join sys_usuario as U on U.idusuario=PE.idusuarioregistro" + str_condicion + " order by PE.idPeticion desc";
    
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);           
            return Ds.Tables[0];
        }
        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtNro.Text != "") { if (oUtil.EsEntero(txtNro.Text)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }
        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
         
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string idPeticion=  this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    //string s_detalle = getDetallePeticion(idPeticion);

                    //e.Row.Cells[8].Text = s_detalle;
                    //ImageButton CmdModificar = (ImageButton)e.Row.Cells[11].Controls[1];
                    //CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    //CmdModificar.CommandName = "Visualizar";
                    //CmdModificar.ToolTip = "Modificar";              
                   
                    ImageButton CmdEliminar = (ImageButton)e.Row.Cells[14].Controls[1];
                    CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Eliminar";
                    CmdEliminar.ToolTip = "Eliminar";

                    ImageButton CmdProtocoloNuevo = (ImageButton)e.Row.Cells[15].Controls[1];
                    CmdProtocoloNuevo.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdProtocoloNuevo.CommandName = "Protocolo";
                    CmdProtocoloNuevo.ToolTip = "Protocolo";

                Button btnVincular = (Button)e.Row.Cells[16].FindControl("Vincular");

                btnVincular.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                btnVincular.CommandName = "Vincular";
                btnVincular.ToolTip = "Vincular";
                
                //string estado = getEstadoProtocolo(idPeticion);
                //e.Row.Cells[12].Text = estado;
                CmdEliminar.Visible = false;
                    CmdProtocoloNuevo.Visible = false;

                    string estado=e.Row.Cells[12].Text;
                string numeroprotocolo= e.Row.Cells[13].Text;

                if (numeroprotocolo != "-")
                    btnVincular.Visible = false;
                else
                    btnVincular.Visible = true;

                if (estado.ToUpper().IndexOf("ENVIADA") != -1) 
                    {
                        CmdEliminar.Visible = true;
                        CmdProtocoloNuevo.Visible = true;
                    }
                    if (estado.ToUpper().IndexOf("RECIBIDA") != -1) e.Row.Cells[12].ForeColor = Color.Red;
                    if (estado.ToUpper().IndexOf("TERMINADO") != -1) e.Row.Cells[12].ForeColor = Color.Green;
                    if (estado.ToUpper().IndexOf("PROCESO") != -1) e.Row.Cells[12].ForeColor = Color.Orange;
                    if (estado.ToUpper().IndexOf("ELIMINAD") != -1)  e.Row.Cells[12].ForeColor = Color.Blue;                   
                       
                
                   
                  
                }              
            
        }

        private string getEstadoProtocolo(string idPeticion)
        {
            string dev = "Recibida";
            Peticion oRegistro = new Peticion();
            oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(idPeticion));
            if (oRegistro.IdProtocolo > 0)
            {
                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), oRegistro.IdProtocolo);
                switch (oProtocolo.Estado)
                { case 1:dev = "En proceso"; break; case 2:dev = "Terminado"; break; default: dev = "Recibida"; break; }
            }
            else { if (oRegistro.Baja) dev = "Eliminada"; }
            return dev;
        }

        private string getDetallePeticion(string idPeticion)
        {
            string dev = ""; int i = 0;
            Peticion oRegistro = new Peticion();
            oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(idPeticion));

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(PeticionItem));
            crit.Add(Expression.Eq("IdPeticion", oRegistro));
            IList items = crit.List();
            foreach (PeticionItem oDet in items)
            {
                i += 1;
                if (dev == "")                                    
                    dev = oDet.IdItem.Nombre;                
                else                                
                    dev = dev +" - " + oDet.IdItem.Nombre ;                
            }
            return i.ToString()+": "+ dev;
        }


        private void Anular(Business.Data.Laboratorio.Peticion oRegistro)
        {
           
            oRegistro.Baja = true;
            oRegistro.Save();
            CargarGrilla();
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Business.Data.Laboratorio.Peticion oRegistro = new Business.Data.Laboratorio.Peticion();
            oRegistro = (Business.Data.Laboratorio.Peticion)oRegistro.Get(typeof(Business.Data.Laboratorio.Peticion), int.Parse(e.CommandArgument.ToString()));
            switch (e.CommandName)
            {
                case "Modificar":
                    {                   
                        Response.Redirect("PeticionEdit.aspx?idPeticion=" + oRegistro.IdPeticion.ToString()+ "&idPaciente="+oRegistro.IdPaciente.IdPaciente.ToString() +"&idTipoServicio="+oRegistro.IdTipoServicio.IdTipoServicio.ToString() +"&Modifica=1");
                    }
                    break;                
                case "Eliminar":
                    Anular(oRegistro);                    
                    break;
                case "Protocolo":
                    {
                        
                     
                        Response.Redirect("VerificaPaciente.aspx?idPeticion=" + oRegistro.IdPeticion.ToString(), false);
                       
                    }
                    break;
                case "Vincular":
                    {


                        Response.Redirect("VincularProtocolo.aspx?idPeticion=" + oRegistro.IdPeticion.ToString(), false);

                    }
                    break;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            CargarGrilla();
        }
    }
}