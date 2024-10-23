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
using System.Data.SqlClient;
using NHibernate;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Data.SqlTypes;
using CrystalDecisions.Shared;
using System.IO;
using System.Drawing;
using NHibernate.Collection;
using CrystalDecisions.Web;
using Business.Data;

namespace WebLab.Turnos
{
    public partial class TurnoList : System.Web.UI.Page
    {
       protected string DiasNoHabiles = "";
       protected DateTime fechaDesde= new DateTime();
       protected DateTime fechaHasta = new DateTime();
       public Configuracion oCon = new Configuracion();
        public  Usuario oUser = new Usuario();


       
       public CrystalReportSource oCr = new CrystalReportSource();

       public Utility oUtil = new Utility();
       protected void Page_PreInit(object sender, EventArgs e)
       {
           oCr.Report.FileName = "";
           oCr.CacheDuration = 0;
           oCr.EnableCaching = false;
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oUser.IdPerfil.IdPerfil == 15)
                {
                    oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfectorDestino);
                    this.MasterPageFile = "../SiteTurnos.Master";
                }
                else
                {
                    oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
                    this.MasterPageFile = "../Site1.Master";
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {      
            if (!Page.IsPostBack)
            {              
                if (Request["tipo"]!=null)
                    Session["tipo"] = Request["tipo"].ToString();

                if (Session["tipo"].ToString() == "recepcion")
                {
                    VerificaPermisos("Pacientes con turno");
                    //pnlDerecho.BackColor = Color.White;                    
                    lblTitulo.Text = "PLANILLA DIARIA" ;
                    lblSubTitulo.Text = "Recepción de Pacientes con Turno";
                    lblSubTitulo.Visible = true;
                    cldTurno.SelectedDate = DateTime.Now;
                    cldTurno.TodaysDate = DateTime.Now;
                    cldTurno.VisibleDate = DateTime.Now;
                    MostrarUltimoProtocolo();
                  //  btnNuevo.Visible = true;
                }
                else
                {
                    string labo = "";
                    if (oUser.IdPerfil.IdPerfil == 15)
                    {
                        labo = oUser.IdEfectorDestino.Nombre;
                        lnkProtocolo.Visible = false; /// no es posible registrar pacientes sin turno
                    }

                    else
                        labo = oUser.IdEfector.Nombre;
                    VerificaPermisos("Asignacion de turnos");
                    lblTitulo.Text = "TURNOS PARA " + labo.ToUpper();
                    cldTurno.SelectedDate = DateTime.Now.AddDays(1);
                    cldTurno.VisibleDate = DateTime.Now.AddDays(1);
                    cldTurno.TodaysDate = DateTime.Now.AddDays(1);

                    imgServicioView.Visible = true;
                    imgServicioView.Attributes.Add("onClick", "javascript: CalendarioView (" + ddlTipoServicio.SelectedValue + "," + ddlItem.SelectedValue + "); return false");
     
                }
                CargarListas();
                //VerificarAgenda();
                //IdentificarDiasNoHabiles();
               
                
                Actualizar();
               
                
          //  if (Session["tipo"].ToString() == "recepcion") btnNuevo.Visible = false;
                
                    
            }

        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }
        private void MostrarUltimoProtocolo()
        {
            if (Request["ultimoProtocolo"] != null)
            {
                lblUltimoProtocolo.Text= "Protocolo Generado: " ;
                Protocolo oRegistro = new Protocolo();
                oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(Request["ultimoProtocolo"].ToString()));             
                if (oRegistro!= null)
                    lblUltimoProtocolo.Text +=  oRegistro.Numero.ToString() + " " + oRegistro.IdPaciente.Apellido + " " + oRegistro.IdPaciente.Nombre;

                
            }

        }


        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {            
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (Permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                        case 1: btnNuevo.Visible = false; break;
                    }
                }
                else Response.Redirect("../AccesoDenegado.aspx", false);
            }          
            else Response.Redirect("../FinSesion.aspx", false);

        }

        private void CargarTurnos()
        {
            
            if (!VerificarAgenda())
            {
              //  string popupScript = "<script language='JavaScript'> alert('No se ha programado una agenda para la fecha y servicio seleccionados'); </script>";
//                Page.RegisterStartupScript("PopupScript", popupScript);

                lblMensaje.Text = "No se ha programado una agenda para la fecha y servicio/practica seleccionados";
                btnNuevo.Visible = false;
                lblMensaje.Visible = true;
                lblLimiteTurnos.Visible = false;
                lblTurnosDados.Visible = false;
                lblTurnosDisponibles.Visible = false;
                lblHorario.Visible = false;
            }
            else
            {
                if (noEsFeriado())
                {
                    btnNuevo.Visible = true;
                    lblMensaje.Visible = false;
                    lblLimiteTurnos.Visible = true;
                    lblTurnosDados.Visible = true;
                    lblTurnosDisponibles.Visible = true;
                    lblHorario.Visible = true;
                }
                else
                {
                    lblMensaje.Text = "La fecha seleccionada es un día feriado.";
                    btnNuevo.Visible = false;
                    lblMensaje.Visible = true;
                    lblLimiteTurnos.Visible = false;
                    lblTurnosDados.Visible = false;
                    lblTurnosDisponibles.Visible = false;
                    lblHorario.Visible = false;
                }
                
            }
        }

        private bool noEsFeriado()
        {
        

        
            lblFecha.Text = cldTurno.SelectedDate.ToLongDateString().ToUpper();//.ToShortDateString();

            int dia = (int)cldTurno.SelectedDate.DayOfWeek; bool result = false;

            DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());

            ISession m_session = NHibernateHttpModule.CurrentSession;
            string m_ssql = " FROM Feriado A WHERE A.Fecha='" + fecha.ToString("yyyyMMdd") + "'";

            IQuery q = m_session.CreateQuery(m_ssql);


            IList lista = q.List();
            if (lista.Count > 0)
              
                        result = false;
                
            
            else
                result = true;

            return result;
        }

        private bool VerificarAgenda()
        {bool result = false;
            if (ddlTipoServicio.SelectedValue != "")
            {
                string s_item = ""; string m_ssqlItem = "";
                if (ddlItem.SelectedValue != "0") s_item = " - " + ddlItem.SelectedItem.Text;
                m_ssqlItem = " AND A.IdItem=" + ddlItem.SelectedValue;

                lblTipoServicio.Text = ddlTipoServicio.SelectedItem.Text + s_item;
                lblFecha.Text = cldTurno.SelectedDate.ToLongDateString().ToUpper();//.ToShortDateString();

                int dia = (int)cldTurno.SelectedDate.DayOfWeek; 

                DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());

                ISession m_session = NHibernateHttpModule.CurrentSession;
                string m_ssql = "";
                if (oUser.IdPerfil.IdPerfil == 15)
                    m_ssql = " FROM Agenda A WHERE A.Baja=0 AND A.IdTipoServicio=" + ddlTipoServicio.SelectedValue +
                                " and IdEfectorSolicitante = " + oUser.IdEfector.IdEfector.ToString()+
                               " AND  A.FechaDesde<='" + fecha.ToString("yyyyMMdd") + "'" +
                               " AND  A.FechaHasta>='" + fecha.ToString("yyyyMMdd") + "'" + m_ssqlItem;

                else
                    m_ssql = " FROM Agenda A WHERE A.Baja=0 AND A.IdTipoServicio=" + ddlTipoServicio.SelectedValue +
                                " and IdEfector = " + oCon.IdEfector.IdEfector.ToString() +
                                " and IdEfectorSolicitante = " + oCon.IdEfector.IdEfector.ToString() +
                               " AND  A.FechaDesde<='" + fecha.ToString("yyyyMMdd") + "'" +
                               " AND  A.FechaHasta>='" + fecha.ToString("yyyyMMdd") + "'" + m_ssqlItem;

                IQuery q = m_session.CreateQuery(m_ssql);


                IList lista = q.List();
                if (lista.Count > 0)
                {
                    foreach (Agenda oAgenda in lista)
                    {
                        fechaDesde = oAgenda.FechaDesde;
                        fechaHasta = oAgenda.FechaHasta;
                        ICriteria crit = m_session.CreateCriteria(typeof(AgendaDia));
                        crit.Add(Expression.Eq("IdAgenda", oAgenda));
                        crit.Add(Expression.Eq("Dia", dia));

                        IList listaDias = crit.List();
                        if (listaDias.Count > 0)
                        {
                            foreach (AgendaDia oAgendaDia in listaDias)
                            {
                                result = true;
                                lblHorario.Text = "Horario de Atención: " + oAgendaDia.HoraDesde + " - " + oAgendaDia.HoraHasta;
                                lblHoraTurno.Text = CalcularHorarioDisponible(oAgendaDia.TipoTurno, oAgendaDia.Frecuencia, oAgendaDia.HoraDesde);
                                if (oAgendaDia.LimiteTurnos == 0)
                                {
                                    lblLimiteTurnos.Text = "Sin límite de turnos";
                                    lblTurnosDisponibles.Text = "0";
                                }
                                else
                                {
                                    lblLimiteTurnos.Text = oAgendaDia.LimiteTurnos.ToString();
                                    int turnos_dados = int.Parse(lblTurnosDados.Text);
                                    lblTurnosDisponibles.Text = (oAgendaDia.LimiteTurnos - turnos_dados).ToString();
                                }

                            }
                            break;
                        }
                        else
                            result = false;
                    }
                }
                else
                    result = false;
            }
            else result = false;

            return result;
        }

        private string CalcularHorarioDisponible(int tipo, int f, string horadesde)
        {
            DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());
            string m_strSQL = "";
            if (oUser.IdPerfil.IdPerfil==15)
                m_strSQL = " SELECT idTurno AS idturno, hora FROM LAB_Turno AS T  (nolock) " +
                             " WHERE (T.baja = 0) AND T.fecha='" + fecha.ToString("yyyyMMdd") + "'" +
                             " and T.idEfectorSolicitante= " + oUser.IdEfector.IdEfector.ToString() +
                             " AND T.idTipoServicio=" + ddlTipoServicio.SelectedValue + " AND T.IdItem=" + ddlItem.SelectedValue + " ORDER BY idturno DESC ";
            else
             m_strSQL = @" SELECT idTurno AS idturno, hora 
                            FROM LAB_Turno AS T  (nolock)
                               WHERE (T.baja = 0) AND T.fecha='" + fecha.ToString("yyyyMMdd") + @"'
                               and T.idEfector= " + oCon.IdEfector.IdEfector.ToString() +
                                @" and T.idEfectorSolicitante= " + oCon.IdEfector.IdEfector.ToString() +
                             @" AND T.idTipoServicio=" + ddlTipoServicio.SelectedValue + " AND T.IdItem=" + ddlItem.SelectedValue + " ORDER BY idturno DESC ";
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            string m_Hora=horadesde;
            lblTurnosDados.Text = Ds.Tables[0].Rows.Count.ToString();

            if (tipo == 1)
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    string aux = Ds.Tables[0].Rows[0][1].ToString();
                    DateTime hora = DateTime.Parse(aux).AddMinutes(f);
                    m_Hora = hora.ToShortTimeString();

                }
            }
                return m_Hora;



        }
        //private void CargarEfector()
        //{
        //    string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


        //    Utility oUtil = new Utility();
        //    string m_ssql = "SELECT idEfector, nombre FROM sys_Efector  E (nolock) where  ";

        //    //if (Request["Operacion"].ToString() != "Modifica")  //alta
        //    m_ssql += " where exists (select 1 from LAB_EfectorRelacionado R (nolock) where E.idEfector = R.idEfectorRel and R.idefector = " + oUser.IdEfector.IdEfector.ToString() +
        //        ")  or (E.idEfector=" + oCon.IdEfector.IdEfector.ToString() + @" )";

        //    m_ssql += " order by nombre ";
        //    oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
        //    ddlEfector.SelectedValue = oCon.IdEfector.IdEfector.ToString();
        //}
        private void CargarListas()
        {

        Utility oUtil = new Utility();

            string m_filtro = "and idEfectorSolicitante= " + oUser.IdEfector.IdEfector.ToString(); //si es generacion solo puede ver agendas de su efector
            if (Request["tipo"] == "recepcion")  //los turnos que asiganron todos
                m_filtro = "and A.idEfector = " + oCon.IdEfector.IdEfector.ToString();


            string m_ssql = @"select idTipoServicio,nombre  from Lab_TipoServicio 
where  idtipoServicio IN (SELECT idTipoServicio from lab_agenda A where baja=0 "+ m_filtro + ") and baja = 0";
            oUtil.CargarCombo(ddlTipoServicio, m_ssql, "idTipoServicio", "nombre");
            if (Session["idServicio"] != null) ddlTipoServicio.SelectedValue = Session["idServicio"].ToString();


            if (oUser.IdPerfil.IdPerfil == 15)
            {
                m_ssql = @"select idEfector, nombre 
							  from sys_Efector where idEfector in (select idEfectorSolicitante from LAB_Agenda A where baja=0 and A.idEfectorSolicitante = " + oUser.IdEfector.IdEfector.ToString() + ") order by nombre";
                oUtil.CargarCombo(ddlEfectorSolicitante, m_ssql, "idEfector", "nombre");
                ddlEfectorSolicitante.SelectedValue = oUser.IdEfector.IdEfector.ToString();
                ddlEfectorSolicitante.Enabled = false;
            }
            else
            {
                m_ssql = @"select idEfector, nombre 
							  from sys_Efector where idEfector in (select idEfectorSolicitante from LAB_Agenda A where baja=0 and A.idEfector = " + oUser.IdEfector.IdEfector.ToString() + ") order by nombre";
                oUtil.CargarCombo(ddlEfectorSolicitante, m_ssql, "idEfector", "nombre");
                ddlEfectorSolicitante.Items.Insert(0, new ListItem("--Todos--", "0"));
            }


            m_ssql = @"SELECT I.idItem, I.nombre FROM  LAB_Agenda A 
INNER JOIN LAB_Item I ON A.idItem = I.idItem where A.baja=0 and I.baja=0  " + m_filtro; //and A.fechaDesde>='" + fecha.ToString("yyyyMMdd") + "'";
            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("--Seleccione práctica--", "0"));
            if (Session["idItem"] != null) ddlItem.SelectedValue = Session["idItem"].ToString();

            m_ssql = null;
            oUtil = null;
        }

        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            if (gvLista.Rows.Count > 0)
            {
                lnkPlanilla.Visible = true;
                lnkPlanillaDetallada.Visible = true;
            }
            else
            {
                lnkPlanilla.Visible = false;
                lnkPlanillaDetallada.Visible =false;
            }

        }

        private object LeerDatos()
        {        
            
            DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());        
            string m_Condicion="";
            if (txtPaciente.Text!="")
            {
                if (rdbBusqueda.Items[0].Selected)//DNI
                    m_Condicion  = " and P.numeroDocumento=" + txtPaciente.Text ;
                if (rdbBusqueda.Items[1].Selected)//Apellido
                    m_Condicion  = " and P.apellido like '%" + txtPaciente.Text+"%'" ;
                if (rdbBusqueda.Items[2].Selected)//Apellido
                    m_Condicion  = " and P.nombre like '%" + txtPaciente.Text+"%'" ;
            }

            //<asp:ListItem Selected="True">Turnos Activos</asp:ListItem>
            //      <asp:ListItem Value="Con Protocolo">Con Protocolo</asp:ListItem>
            //      <asp:ListItem>Sin Protocolo</asp:ListItem>
            //      <asp:ListItem>Turnos Eliminados</asp:ListItem>
            switch (ddlEstadoTurno.SelectedValue)
            {
                case "Turnos Activos": m_Condicion += " and T.baja=0"; break;
                case "Con Protocolo": m_Condicion += " and T.idProtocolo>0"; break;
                case "Sin Protocolo": m_Condicion += " and T.idProtocolo=0"; break;
                case "Turnos Eliminados": m_Condicion += " and T.baja=1"; break;
            }
            //if (ddlItem.SelectedValue != "0") 
            m_Condicion +=  " AND T.IdItem=" + ddlItem.SelectedValue ;
            if (ddlEfectorSolicitante.SelectedValue!="0")
                m_Condicion += " AND T.IdEfectorSolicitante=" + ddlEfectorSolicitante.SelectedValue;

            if (oUser.IdPerfil.IdPerfil==15)
                m_Condicion += " and    T.idEfectorSolicitante= " + oUser.IdEfector.IdEfector.ToString();   //MultiEfector         
            else
                  m_Condicion += " and    T.idEfector= " + oCon.IdEfector.IdEfector.ToString();   //MultiEfector         
            if (ddlTipoServicio.SelectedValue != "")
                m_Condicion += " AND T.idTipoServicio=" + ddlTipoServicio.SelectedValue;
            
              
           string m_strSQL = @" SELECT  T.idTurno AS idturno, T.hora, case when P.idestado=2 then 'temp:'+ convert (varchar,P.numeroDocumento)  else convert(varchar, P.numeroDocumento) end AS numeroDocumento, P.apellido AS apellido, P.nombre AS nombre,  
                             case when     T.baja = 1 then 'Turno Eliminado' else case when  T.idProtocolo>0 then   convert(varchar,Pro.numero) else '-' end end as Protocolo,convert(varchar(10), t.FECHA,103) as fecha  ,S.nombre as servicio  
                            , U.username as usuario,  T.fechaRegistro, P.informacionContacto ,E.nombre as efector
                                FROM         LAB_Turno AS T  (nolock)
                              INNER JOIN  Sys_Paciente AS P (nolock)  ON T.idPaciente = P.idPaciente
                               Left join LAB_Protocolo Pro (nolock)  on Pro.idProtocolo = T.idProtocolo 
             INNER JOIN   LAB_TipoServicio as S (nolock)  ON T.idTipoServicio = S.idTipoServicio 
                               INNER JOIN     Sys_Usuario AS U (nolock)  ON T.idUsuarioRegistro = U.idUsuario 
 inner join sys_Efector E (nolock) on E.idEfector=T.idEfectorSolicitante
                              WHERE  T.fecha='" + fecha.ToString("yyyyMMdd") + "'"+                              m_Condicion + " ORDER BY T.idTurno ";

            //and Pro.idEfector=" + oCon.IdEfector.IdEfector.ToString() +
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
             
            return Ds.Tables[0];
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            if (VerificarDisponibilidad())
            {
                Session["Turno_Fecha"] = cldTurno.SelectedDate;
                Session["Turno_IdTipoServicio"] = ddlTipoServicio.SelectedValue;
                Session["idServicio"] = ddlTipoServicio.SelectedValue;
                Session["Turno_Hora"] = lblHoraTurno.Text;
                Session["idItem"] = ddlItem.SelectedValue;
                Response.Redirect("Default.aspx", false);
            }
            else
            {
                  string popupScript = "<script language='JavaScript'> alert('Se ha alcanzado el límite de turnos dados')</script>";
                  Page.RegisterClientScriptBlock("PopupScript", popupScript);
            }

        }

        private bool VerificarDisponibilidad()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;

            if (ddlTipoServicio.SelectedValue != "")
            {
                TipoServicio oServicio = new TipoServicio();
                oServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), int.Parse(ddlTipoServicio.SelectedValue));

                ICriteria crit = m_session.CreateCriteria(typeof(Turno));
                crit.Add(Expression.Eq("Fecha", cldTurno.SelectedDate));
                crit.Add(Expression.Eq("IdTipoServicio", oServicio));
               
               if (oUser.IdPerfil.IdPerfil==15)
                    crit.Add(Expression.Eq("IdEfectorSolicitante", oUser.IdEfector));
                else
                    crit.Add(Expression.Eq("IdEfector", oUser.IdEfector)); 

                crit.Add(Expression.Eq("IdItem", int.Parse(ddlItem.SelectedValue)));

                crit.Add(Expression.Eq("Baja", false));

                IList listaTurnosDados = crit.List();
                if (listaTurnosDados.Count >= int.Parse(lblLimiteTurnos.Text))
                    return false;
                else
                    return true;
            }
            else
                return false;               
        }

        protected void cldTurno_SelectionChanged(object sender, EventArgs e)
        {
            Actualizar();                     
        }

        private void Actualizar()
        {
            //Session["tipo"] = Request["tipo"];
            if (Session["tipo"].ToString() != "recepcion") ///Sólo para asignacion de turnos
            {
                if (ddlItem.SelectedValue != "0") { imgCalendarioView.Visible = true; imgCalendarioView.Attributes.Add("onClick", "javascript: CalendarioView (" + ddlTipoServicio.SelectedValue + "," + ddlItem.SelectedValue + "); return false"); }
                else imgCalendarioView.Visible = false;

                imgServicioView.Attributes.Add("onClick", "javascript: CalendarioView (" + ddlTipoServicio.SelectedValue + ",0); return false");
            }

            CargarTurnos();
            CargarGrilla();
            PintarReferencias(); int turno_dispo =int.Parse( lblTurnosDisponibles.Text);
            if (cldTurno.SelectedDate.Date < DateTime.Now.Date)
                btnNuevo.Visible = false;
            else
                if (lblMensaje.Visible)
                    btnNuevo.Visible = false;
                else
                    if (turno_dispo<=0)
                        btnNuevo.Visible = false;
                    else
                        if (Permiso == 1) btnNuevo.Visible = false;
                        else btnNuevo.Visible = true;


            Session["Turno_Fecha"] = cldTurno.SelectedDate;
            Session["Turno_IdTipoServicio"] = ddlTipoServicio.SelectedValue;
            Session["Turno_Hora"] = lblHoraTurno.Text;
        }

        protected void ddlTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Actualizar();
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton CmdModificar = (LinkButton)e.Row.Cells[7].Controls[1];
                    CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdModificar.CommandName = "Modificar";
                    CmdModificar.ToolTip = "Modificar";

                    ImageButton CmdImprimir = (ImageButton)e.Row.Cells[8].Controls[1];
                    CmdImprimir.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdImprimir.CommandName = "Imprimir";
                    CmdImprimir.ToolTip = "Imprimir";


                    LinkButton CmdEliminar = (LinkButton)e.Row.Cells[9].Controls[1];
                    CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Eliminar";
                    CmdEliminar.ToolTip = "Eliminar";
                    
                  
                    ImageButton CmdProtocolo = (ImageButton)e.Row.Cells[10].Controls[1];
                    CmdProtocolo.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdProtocolo.CommandName = "Protocolo";
                    CmdProtocolo.ToolTip = "Protocolo";


                    if (Request["tipo"] != null) Session["tipo"] = Request["tipo"];
                    if (Session["tipo"].ToString() == "generacion")
                    { CmdProtocolo.Visible = false; }

                    if (Permiso == 1)
                    {
                        CmdEliminar.Visible = false;
                        CmdModificar.ToolTip = "Consultar";
                    }

                    System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                    if ((e.Row.Cells[6].Text == "-") || (e.Row.Cells[6].Text == "Turno Eliminado"))                //simple                    
                    {                        
                        //Image hlnk = new Image();
                        hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                        e.Row.Cells[0].Controls.Add(hlnk);

                        if (e.Row.Cells[6].Text == "Turno Eliminado")
                        {
                            CmdEliminar.Visible = false;
                            CmdProtocolo.Visible = false;
                            CmdModificar.Visible = false;
                        }
                    }
                    else
                    {
                        CmdModificar.Visible = false;
                        CmdImprimir.Visible = false;
                        CmdEliminar.Visible = false;
                        CmdProtocolo.Visible = false;
                       
                        hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                        e.Row.Cells[0].Controls.Add(hlnk);                        
                    }                                                    
                }
             //   Configuracion oCon = new Configuracion();oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);              
                e.Row.Cells[7].Visible = oCon.GeneraComprobanteTurno;
                
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modificar": Response.Redirect("TurnosEdit2.aspx?idTurno=" + e.CommandArgument + "&Modifica=1"); break;
                case "Imprimir": Imprimir( e.CommandArgument);break;
                case "Eliminar":
                    if (Permiso == 2)
                    {
                        Anular(e.CommandArgument);                     
                    }
                   break;
                case "Protocolo":
                    RedireccionarProtocolo(e.CommandArgument.ToString());
                    break;
            }    
        }

        private void RedireccionarProtocolo(string p)
        {
            Turno oRegistro = new Turno();
            oRegistro = (Turno)oRegistro.Get(typeof(Turno), int.Parse(p));
            if  (oRegistro.IdTipoServicio.IdTipoServicio==6)
                Response.Redirect("../Protocolos/ProtocoloEditForense.aspx?idServicio=" + ddlTipoServicio.SelectedValue + "&idPaciente=" + oRegistro.IdPaciente.IdPaciente + "&Operacion=AltaTurno&idTurno=" + p);
            else

            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idServicio=" + ddlTipoServicio.SelectedValue + "&idPaciente=" + oRegistro.IdPaciente.IdPaciente + "&Operacion=AltaTurno&idTurno=" + p);
        }
        private void Anular(object p)
        {
          /*  if (Session["idUsuario"] != null)
            {*/
              //  Usuario oUser = new Usuario();
                Turno oRegistro = new Turno();
                oRegistro = (Turno)oRegistro.Get(typeof(Turno), int.Parse(p.ToString()));
                oRegistro.Baja = true;
            oRegistro.IdUsuarioRegistro = oUser;
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();
                
                Actualizar();
         /*   }
          
            else Response.Redirect("../FinSesion.aspx", false);*/
        }


        private void Imprimir(object p)
        {
            //Aca se deberá consultar los parametros para mostrar una hoja de trabajo u otra
            //this.CrystalReportSource1.Report.FileName = "HTrabajo2.rpt";
            string informe = "../Informes/Turno.rpt";
            //Configuracion oCon = new Configuracion();   oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;

            Turno oTurno = new Turno();
            oTurno= (Turno)oTurno.Get(typeof(Turno),int.Parse(p.ToString()));


            oCr.Report.FileName = informe;
            oCr.ReportDocument.SetDataSource(oTurno.GetDataSet());
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();
 
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Turno");
          
        }

        private void PintarReferencias()
        {
            for (int j = 0; j < gvLista.Rows.Count; j++)
            {
                switch (gvLista.Rows[j].Cells[3].Text)
                {
                    case "Si":///Abierto
                        for (int i = 0; i < gvLista.Columns.Count; i++)                        
                            gvLista.Rows[j].Cells[i].Style["background"] = "#ffffdf";                                                                        
                        break;
                    case "No":///Con Resultados
                        for (int i = 0; i < gvLista.Columns.Count; i++)                        
                            gvLista.Rows[j].Cells[i].Style["background"] = "#ffffff";                        
                        break;               
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            Actualizar();
        }

        protected void cldTurno_DayRender(object sender, DayRenderEventArgs e)
        {
            //if ((e.Day.Date.Day < DateTime.Now.Day) || (e.Day.Date.Month < DateTime.Now.Month))
            //    e.Day.IsSelectable = false;

        
        
        }


        protected DateTime GetFirstDayOfNextMonth()
        {
            int monthNumber, yearNumber;
            if (cldTurno.VisibleDate.Month == 12)
            {
                monthNumber = 1;
                yearNumber = cldTurno.VisibleDate.Year + 1;
            }
            else
            {
                monthNumber = cldTurno.VisibleDate.Month + 1;
                yearNumber = cldTurno.VisibleDate.Year;
            }
            DateTime lastDate = new DateTime(yearNumber, monthNumber, 1);
            return lastDate;
        }

        //protected void IdentificarDiasNoHabiles()
        //{
        //    DateTime firstDate = new DateTime(cldTurno.VisibleDate.Year,        cldTurno.VisibleDate.Month, 1);
        //    DateTime lastDate = GetFirstDayOfNextMonth();

        //    string m_strSQL = " SELECT DISTINCT  AD.Dia, A.fechaDesde, A.fechaHasta " +
        //                 " FROM LAB_AgendaDia as AD " +
        //                " JOIN LAB_Agenda as A ON A.IdAgenda= Ad.IdAgenda " +
        //                " WHERE A.Baja=0 AND A.IdTipoServicio=" + ddlTipoServicio.SelectedValue;// +
        //                //" AND A.fechaDesde<='" + firstDate.ToString("yyyyMMdd") + "'";// and A.fechaHasta>='" + lastDate.ToString("yyyyMMdd") + "'"; 
                        
        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);

          
            
        //    if (Ds.Tables[0].Rows.Count > 0)
        //    {
        //        //fechaDesde= DateTime.Parse(Ds.Tables[0].Rows[0][1].ToString());
        //        //fechaHasta = DateTime.Parse(Ds.Tables[0].Rows[0][2].ToString());

        //        for (int dia = 1; dia <= 7; dia++)
        //        {
        //            bool noes = false;
        //            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        //            {

        //                int diaHabil = int.Parse(Ds.Tables[0].Rows[i][0].ToString());
        //                if (dia == diaHabil)
        //                {
        //                    noes = true;
        //                    break;
        //                }
        //            }
                  
        //             if (!noes)
        //                 if (DiasNoHabiles == "")
        //                     DiasNoHabiles = dia.ToString();
        //                 else
        //                     DiasNoHabiles += ";" + dia.ToString();
        //        }
        //    }

        //    DiasNoHabiles = DiasNoHabiles.Replace("7", "0");

        //}

        protected void cldTurno_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Actualizar();

        }

        protected void lnkProtocolo_Click(object sender, EventArgs e)
        {
            string idServicio = "1";
            if ((ddlTipoServicio.SelectedValue != "") && (ddlTipoServicio.SelectedValue != "0"))
                idServicio = ddlTipoServicio.SelectedValue;

            Response.Redirect("../Protocolos/Default2.aspx?idServicio="+ idServicio + "&idUrgencia=0", false);
        }

        protected void lnkPlanilla_Click(object sender, EventArgs e)
        {
            ImprimirPlanilla();
        }

        private void ImprimirPlanilla()
        {             
            string informe = "../Informes/PlanillaTurno.rpt";
            string s_item = "";
            if (ddlItem.SelectedValue != "0") s_item = ddlItem.SelectedItem.Text;
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue(); encabezado1.Value = oCon.EncabezadoLinea1;
            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue(); encabezado2.Value = oCon.EncabezadoLinea2;
            //ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue(); encabezado3.Value = oCon.EncabezadoLinea3;
            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue(); encabezado3.Value = s_item;
         
            oCr.Report.FileName = informe;
            oCr.ReportDocument.SetDataSource(LeerDatos());
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();
            Utility oUtil = new Utility();
            string nombrePDF = oUtil.CompletarNombrePDF("PlanillaTurno");
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

          


        }

        protected void lnkPlanillaDetallada_Click(object sender, EventArgs e)
        {
            ImprimirPlanillaDetallada();

        }
        private void ImprimirPlanillaDetallada()
        {

            string s_item = "";
            if (ddlItem.SelectedValue != "0") s_item = ddlItem.SelectedItem.Text;
            string informe = "../Informes/PlanillaDetalladaTurno.rpt";
            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = s_item; // oCon.EncabezadoLinea3;


            oCr.Report.FileName = informe;
            oCr.ReportDocument.SetDataSource(GetDataSetPlanillaDetallada());
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();
            Utility oUtil = new Utility();
            string nombrePDF = oUtil.CompletarNombrePDF("PlanillaDetalladaTurno");
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);

           


        }

     

        public DataTable GetDataSetPlanillaDetallada()
        {
            //string m_strSQL = " SELECT  * from vta_LAB_ImprimirTurno " +
            //                  " WHERE idTurno=" + IdTurno;

            DataSet Ds = new DataSet();
        //       SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
           SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_ImprimirTurno]";

            cmd.Parameters.Add("@idTurno", SqlDbType.NVarChar);
            cmd.Parameters["@idTurno"].Value = getListaTurno();

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);



            //SqlDataAdapter adapter = new SqlDataAdapter();
            //adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            //adapter.Fill(Ds);

            // conn.Close();
            //   adapter.Dispose();
            return Ds.Tables[0];
        }

        private string getListaTurno()
        {
            string s_lista = "";
            for (int i = 0; i < gvLista.Rows.Count; i++)
            {
                if (s_lista=="")
                s_lista =  this.gvLista.DataKeys[i].Value.ToString();
                else
                    s_lista += ", " + this.gvLista.DataKeys[i].Value.ToString();
            }
            return s_lista;
        }

        protected void ddlEstadoTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            Actualizar();
        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (rdbBusqueda.Items[0].Selected)//DNI            
            {
                if (txtPaciente.Text != "") { if (oUtil.EsEntero(txtPaciente.Text)) args.IsValid = true; else args.IsValid = false; }
                else
                    args.IsValid = true;
            }
            else
                args.IsValid = true;
        }

        protected void ddlEfectorSolicitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            Actualizar();
        }
    }
}
