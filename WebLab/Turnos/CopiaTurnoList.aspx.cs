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
    public partial class CopiaTurnoList : System.Web.UI.Page
    {
       protected string DiasNoHabiles = "";
       protected DateTime fechaDesde= new DateTime();
       protected DateTime fechaHasta = new DateTime();


       public CrystalReportSource oCr = new CrystalReportSource();

       public Utility oUtil = new Utility();
       protected void Page_PreInit(object sender, EventArgs e)
       {
           oCr.Report.FileName = "";
           oCr.CacheDuration = 0;
           oCr.EnableCaching = false;
           //Configuracion oCon = new Configuracion(); 
           //oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
           //oCr.ReportDocument.PrintOptions.PrinterName = oCon.NombreImpresora;// ConfigurationSettings.AppSettings["Impresora"]; 
       }

        protected void Page_Load(object sender, EventArgs e)
        {
            //List DiasHabiles= new List
            if (!Page.IsPostBack)
            {              
                if (Request["tipo"]!=null)
                    Session["tipo"] = Request["tipo"].ToString();

                if (Session["tipo"].ToString() == "recepcion")
                {
                    VerificaPermisos("Pacientes con turno");
                    pnlDerecho.BackColor = Color.White;                    
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
                    VerificaPermisos("Asignacion de turnos");
                    lblTitulo.Text = "TURNOS";
                    cldTurno.SelectedDate = DateTime.Now.AddDays(1);
                    cldTurno.VisibleDate = DateTime.Now.AddDays(1);
                    cldTurno.TodaysDate = DateTime.Now.AddDays(1);              
                }
                CargarListas();
                //VerificarAgenda();
                //IdentificarDiasNoHabiles();
               
                Actualizar();
               
                
          //  if (Session["tipo"].ToString() == "recepcion") btnNuevo.Visible = false;
                
                    
            }

        }

        private void MostrarUltimoProtocolo()
        {
            if (Request["ultimoProtocolo"] != null)
            {
                lblUltimoProtocolo.Text= "Protocolo Generado: " ;
               Protocolo oRegistro = new Protocolo();
                oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(Request["ultimoProtocolo"].ToString()));
                //Configuracion oC = new Configuracion();
                //oC = (Configuracion)oC.Get(typeof(Configuracion), 1);
                //if (oC.TipoNumeracionProtocolo == 2)
                //    lblUltimoProtocolo.Text += oRegistro.PrefijoSector;
                lblUltimoProtocolo.Text +=  oRegistro.GetNumero().ToString() + " " + oRegistro.IdPaciente.Apellido + " " + oRegistro.IdPaciente.Nombre;
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

                lblMensaje.Text = "No se ha programado una agenda para la fecha y servicio seleccionados";
                btnNuevo.Visible = false;
                lblMensaje.Visible = true;
                lblLimiteTurnos.Visible = false;
                lblTurnosDados.Visible = false;
                lblTurnosDisponibles.Visible = false;
                lblHorario.Visible = false;
            }
            else
            {
                lblMensaje.Visible = false;
                lblLimiteTurnos.Visible = true;
                lblTurnosDados.Visible = true;
                lblTurnosDisponibles.Visible =true;
                lblHorario.Visible = true;
                
            }
        }

        private bool VerificarAgenda()
        {
            lblTipoServicio.Text = "Tipo de Servicio: " +ddlTipoServicio.SelectedItem.Text;
            lblFecha.Text =cldTurno.SelectedDate.ToLongDateString().ToUpper();//.ToShortDateString();
            
            int dia=  (int)          cldTurno.SelectedDate.DayOfWeek;
            bool result=false;

            DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());  
                        

            ISession m_session = NHibernateHttpModule.CurrentSession;
            string m_ssql = " FROM Agenda A WHERE A.Baja=0 AND A.IdTipoServicio=" + ddlTipoServicio.SelectedValue +
                           " AND  A.FechaDesde<='" + fecha.ToString("yyyyMMdd") + "'" +
                           " AND  A.FechaHasta>='" + fecha.ToString("yyyyMMdd") + "'";
            
            IQuery q=          m_session.CreateQuery( m_ssql);


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
                            lblHoraTurno.Text = CalcularHorarioDisponible(oAgendaDia.TipoTurno,oAgendaDia.Frecuencia, oAgendaDia.HoraDesde);
                            if (oAgendaDia.LimiteTurnos == 0)
                            {
                                lblLimiteTurnos.Text = "Sin límite de turnos";
                                lblTurnosDisponibles.Text ="-";
                            }
                            else
                            {
                                lblLimiteTurnos.Text = oAgendaDia.LimiteTurnos.ToString();
                                int turnos_dados= int.Parse(lblTurnosDados.Text);
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
            
            return result;
        }

        private string CalcularHorarioDisponible(int tipo, int f, string horadesde)
        {
            DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());           
            string m_strSQL = " SELECT  idTurno AS idturno, hora FROM LAB_Turno AS T " +
                              "  WHERE (T.baja = 0) AND T.fecha='" + fecha.ToString("yyyyMMdd") + "'" +
                              " AND T.idTipoServicio=" + ddlTipoServicio.SelectedValue +
                              " ORDER BY idturno DESC ";
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


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select idTipoServicio,nombre  from Lab_TipoServicio where baja = 0";
            oUtil.CargarCombo(ddlTipoServicio, m_ssql, "idTipoServicio", "nombre");


          
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


            string m_strSQL = " SELECT     T.idTurno AS idturno, P.numeroDocumento AS numeroDocumento, P.apellido AS apellido, P.nombre AS nombre, " +
                            " case when     T.baja = 1 then 'Turno Eliminado' else case when  T.idProtocolo>0 then dbo.NumeroProtocolo(Pro.idProtocolo) else '-' end end as Protocolo,convert(varchar(10), t.FECHA,103) as fecha  ,S.nombre as servicio " +
                            " , U.username as usuario,  T.fechaRegistro, P.informacionContacto " +
                              "  FROM         LAB_Turno AS T " +
                              " INNER JOIN  Sys_Paciente AS P ON T.idPaciente = P.idPaciente " +
                              " Left join LAB_Protocolo Pro on Pro.idProtocolo = T.idProtocolo " +
                              " INNER JOIN   LAB_TipoServicio as S ON T.idTipoServicio = S.idTipoServicio"+
                              "   INNER JOIN     Sys_Usuario AS U ON T.idUsuarioRegistro = U.idUsuario" +

                              "  WHERE  T.fecha='" + fecha.ToString("yyyyMMdd") + "'"+
                              " AND T.idTipoServicio="+ ddlTipoServicio.SelectedValue + m_Condicion;
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            // 
            // 
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
            
            TipoServicio oServicio= new TipoServicio();
            oServicio= (TipoServicio) oServicio.Get(typeof(TipoServicio),int.Parse(ddlTipoServicio.SelectedValue));

            ICriteria crit = m_session.CreateCriteria(typeof(Turno));
            crit.Add(Expression.Eq("Fecha", cldTurno.SelectedDate));
            crit.Add(Expression.Eq("IdTipoServicio",oServicio));
            crit.Add(Expression.Eq("Baja", false));

            IList listaTurnosDados = crit.List();
            if (listaTurnosDados.Count >= int.Parse(lblLimiteTurnos.Text))
                return false;
            else
                return true;                    
        }

        protected void cldTurno_SelectionChanged(object sender, EventArgs e)
        {
            Actualizar();                     
        }

        private void Actualizar()
        {
         //   IdentificarDiasNoHabiles();
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
                    ImageButton CmdModificar = (ImageButton)e.Row.Cells[5].Controls[1];
                    CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdModificar.CommandName = "Modificar";
                    CmdModificar.ToolTip = "Modificar";

                    ImageButton CmdImprimir = (ImageButton)e.Row.Cells[6].Controls[1];
                    CmdImprimir.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdImprimir.CommandName = "Imprimir";
                    CmdImprimir.ToolTip = "Imprimir";

                    
                    ImageButton CmdEliminar = (ImageButton)e.Row.Cells[7].Controls[1];
                    CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Eliminar";
                    CmdEliminar.ToolTip = "Eliminar";
                    
                  
                    ImageButton CmdProtocolo = (ImageButton)e.Row.Cells[8].Controls[1];
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
                    if ((e.Row.Cells[4].Text == "-") || (e.Row.Cells[4].Text == "Turno Eliminado"))                //simple                    
                    {                        
                        //Image hlnk = new Image();
                        hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                        e.Row.Cells[0].Controls.Add(hlnk);

                        if (e.Row.Cells[4].Text == "Turno Eliminado")
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
                Configuracion oCon = new Configuracion();oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);              
                e.Row.Cells[6].Visible = oCon.GeneraComprobanteTurno;
                
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modificar": Response.Redirect("TurnoEdit2.aspx?idTurno=" + e.CommandArgument + "&Modifica=1"); break;
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
            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idServicio=" + ddlTipoServicio.SelectedValue + "&idPaciente=" + oRegistro.IdPaciente.IdPaciente + "&Operacion=AltaTurno&idTurno=" + p);
        }
        private void Anular(object p)
        {
            if (Session["idUsuario"] != null)
            {
                Usuario oUser = new Usuario();
                Turno oRegistro = new Turno();
                oRegistro = (Turno)oRegistro.Get(typeof(Turno), int.Parse(p.ToString()));
                oRegistro.Baja = true;
                oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();
                Actualizar();
            }
          
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void Imprimir(object p)
        {
            //Aca se deberá consultar los parametros para mostrar una hoja de trabajo u otra
            //this.CrystalReportSource1.Report.FileName = "HTrabajo2.rpt";
            string informe = "../Informes/Turno.rpt";
            Configuracion oCon = new Configuracion();              oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

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

            //if (rdbImprimir.Items[0].Selected == true)//imprimir
          //   oCr.ReportDocument.PrintToPrinter(1, false, 0,0);
            //else
            //{
            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=Turno.pdf");

            Response.BinaryWrite(oStream.ToArray());
            Response.End();
            //}
        }

        private void PintarReferencias()
        {
            for (int j = 0; j < gvLista.Rows.Count; j++)
            {
                switch (gvLista.Rows[j].Cells[3].Text)
                {
                    case "Si":///Abierto
                        for (int i = 0; i < gvLista.Columns.Count; i++)
                        {
                            gvLista.Rows[j].Cells[i].Style["background"] = "#ffffdf";
                        }                                                
                        break;
                    case "No":///Con Resultados
                        for (int i = 0; i < gvLista.Columns.Count; i++)
                        {
                            gvLista.Rows[j].Cells[i].Style["background"] = "#ffffff";
                        }
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
          //  string[] arr = DiasNoHabiles.Split((";").ToCharArray());
          ////  e.Cell.BackColor = System.Drawing.Color.LightYellow;

          //  e.Day.IsSelectable = true;
          //  if (DiasNoHabiles != "")
          //  {
          //      foreach (string m in arr)
          //      {
          //          int diaCalendario = (int)e.Day.Date.DayOfWeek;
          //          int diaNoHabil = int.Parse(m);
          //          if (diaCalendario == diaNoHabil)
          //          {
          //              e.Day.IsSelectable = false;
          //              e.Cell.Enabled = false;
          //              e.Cell.BackColor = System.Drawing.Color.Gainsboro;
                        
          //          }
          //      }
          //  }      

          //  if ((e.Day.Date>=fechaDesde) && (e.Day.Date<=fechaHasta))
          //      e.Day.IsSelectable = true;
          //  else
          //  {
          //              e.Day.IsSelectable = false;
          //              e.Cell.Enabled = false;
          //              e.Cell.BackColor = System.Drawing.Color.Gainsboro;
                
          //  }
        
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

        protected void IdentificarDiasNoHabiles()
        {
            DateTime firstDate = new DateTime(cldTurno.VisibleDate.Year,        cldTurno.VisibleDate.Month, 1);
            DateTime lastDate = GetFirstDayOfNextMonth();

            string m_strSQL = " SELECT DISTINCT  AD.Dia, A.fechaDesde, A.fechaHasta " +
                         " FROM LAB_AgendaDia as AD " +
                        " JOIN LAB_Agenda as A ON A.IdAgenda= Ad.IdAgenda " +
                        " WHERE A.Baja=0 AND A.IdTipoServicio=" + ddlTipoServicio.SelectedValue;// +
                        //" AND A.fechaDesde<='" + firstDate.ToString("yyyyMMdd") + "'";// and A.fechaHasta>='" + lastDate.ToString("yyyyMMdd") + "'"; 
                        
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

          
            
            if (Ds.Tables[0].Rows.Count > 0)
            {
                //fechaDesde= DateTime.Parse(Ds.Tables[0].Rows[0][1].ToString());
                //fechaHasta = DateTime.Parse(Ds.Tables[0].Rows[0][2].ToString());

                for (int dia = 1; dia <= 7; dia++)
                {
                    bool noes = false;
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        int diaHabil = int.Parse(Ds.Tables[0].Rows[i][0].ToString());
                        if (dia == diaHabil)
                        {
                            noes = true;
                            break;
                        }
                    }
                  
                     if (!noes)
                         if (DiasNoHabiles == "")
                             DiasNoHabiles = dia.ToString();
                         else
                             DiasNoHabiles += ";" + dia.ToString();
                }
            }

            DiasNoHabiles = DiasNoHabiles.Replace("7", "0");

        }

        protected void cldTurno_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            Actualizar();

        }

        protected void lnkProtocolo_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Protocolos/Default.aspx?idServicio=1&idUrgencia=0", false);
        }

        protected void lnkPlanilla_Click(object sender, EventArgs e)
        {
            ImprimirPlanilla();
        }

        private void ImprimirPlanilla()
        {
           
  
            string informe = "../Informes/PlanillaTurno.rpt";
            Configuracion oCon = new Configuracion();              oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;

         
            oCr.Report.FileName = informe;
            oCr.ReportDocument.SetDataSource(LeerDatos());
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();

          
            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=PlanillaTurno.pdf");

            Response.BinaryWrite(oStream.ToArray());
            Response.End();
         
       
        }

        protected void lnkPlanillaDetallada_Click(object sender, EventArgs e)
        {
            ImprimirPlanillaDetallada();

        }
        private void ImprimirPlanillaDetallada()
        {


            string informe = "../Informes/PlanillaDetalladaTurno.rpt";
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;


            oCr.Report.FileName = informe;
            oCr.ReportDocument.SetDataSource(GetDataSetPlanillaDetallada());
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.DataBind();


            MemoryStream oStream; // using System.IO
            oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=PlanillaDetalladaTurno.pdf");

            Response.BinaryWrite(oStream.ToArray());
            Response.End();


        }

     

        public DataTable GetDataSetPlanillaDetallada()
        {
            //string m_strSQL = " SELECT  * from vta_LAB_ImprimirTurno " +
            //                  " WHERE idTurno=" + IdTurno;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

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


    }
}
