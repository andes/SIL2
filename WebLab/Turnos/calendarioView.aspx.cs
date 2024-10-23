using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using Business;
using System.Collections;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Data;
using System.Data.SqlClient;
using Business.Data;
using System.Configuration;

namespace WebLab.Turnos
{
    public partial class calendarioView : System.Web.UI.Page
    {

      
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
          //  else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        { if (!Page.IsPostBack)
            VerificarAgenda();

        }

        private void VerificarAgenda()
        {
             String []Dia={"Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado"};

             if (Request["idItem"].ToString() != "0")
             {
                 Item oRegistro = new Item();
                 oRegistro = (Item)oRegistro.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
                 item.Text = oRegistro.Nombre;
             }

            

            string m_ssqlItem = " AND A.IdItem=" + Request["idItem"].ToString();
             m_ssqlItem += " AND A.IdTipoServicio=" + Request["idTipoServicio"].ToString();

            //m_ssqlItem += " AND A.IdEfector=" +oUser.IdEfector.IdEfector.ToString();


            m_ssqlItem += "  and A.IdEfectorSolicitante = " + oUser.IdEfector.IdEfector.ToString();

              DateTime fecha = DateTime.Now.AddDays(1);// ..Parse(cldTurno.SelectedDate.ToShortDateString());

            ISession m_session = NHibernateHttpModule.CurrentSession;
            string m_ssql = " FROM Agenda A WHERE A.Baja=0 " + m_ssqlItem; //AND A.IdTipoServicio=" + ddlTipoServicio.SelectedValue +
                            //" AND  (A.FechaDesde<='" + fecha.ToString("yyyyMMdd") + "' or  A.FechaDesde<='" + fecha.ToString("yyyyMMdd") + "')" +
                            //" AND  A.FechaHasta>='" + fecha.ToString("yyyyMMdd") + "'" ;

            IQuery q = m_session.CreateQuery(m_ssql);

            List<CalendarioTurnos> resultadosList = new List<CalendarioTurnos>();
            IList lista = q.List();
            if (lista.Count > 0)
            {
                foreach (Agenda oAgenda in lista)
                {
                    DateTime fechaDesde =oAgenda.FechaDesde; if (fechaDesde < fecha) fechaDesde = fecha;
                    DateTime fechaHasta = oAgenda.FechaHasta;

                    ICriteria crit = m_session.CreateCriteria(typeof(AgendaDia));
                    crit.Add(Expression.Eq("IdAgenda", oAgenda));
                    

                    IList listaDias = crit.List();
                    if (listaDias.Count > 0)
                    {
                        foreach (AgendaDia oAgendaDia in listaDias)
                        {

                            DateTime fechaDesdeaux = fechaDesde;
                            DateTime fechaHastaaux = fechaHasta;
                            while (fechaDesdeaux <= fechaHastaaux)
                            {

                                int dia = (int) fechaDesdeaux.DayOfWeek; //bool result = false;
                                if (dia == oAgendaDia.Dia)
                                {
                                    int cantidadTurnosdados = int.Parse(CalcularTurnoDisponible( fechaDesdeaux));
                                    
                                    int cantidadTurnos = oAgendaDia.LimiteTurnos;
                                    int cantidadTurnosDisponibles = cantidadTurnos - cantidadTurnosdados;
                                    if (cantidadTurnosDisponibles > 0)
                                    {
                                        CalendarioTurnos oCalendario = new CalendarioTurnos();
                                        oCalendario.Fecha = fechaDesdeaux;
                                        oCalendario.CantidadTurnosDisponibles = cantidadTurnosDisponibles;
                                        oCalendario.Dia = Dia[dia];                                        
                                        resultadosList.Add(oCalendario);
                                    }
                                }
                                fechaDesdeaux = fechaDesdeaux.AddDays(1);
                            }
                         

                        }
                      
                    }
                    //else
                    //    result = false;
                }
            }
     //       resultadosList = from CalendarioTurnos in resultadosList orderby CalendarioTurnos.Fecha ascending select CalendarioTurnos;


            resultadosList = (from n in resultadosList
                           orderby  GetDynamicSortProperty(n, "Fecha") ascending
                           select n).ToList();


            gv.DataSource = resultadosList;
            gv.DataBind();
            //return result;
        }
        public object GetDynamicSortProperty(object item, string propName)
        {
            //Use reflection to get order type
            return item.GetType().GetProperty(propName).GetValue(item, null);
        }

        public class CalendarioTurnos
        {
            public string Dia { get; set; }
            public DateTime Fecha { get; set; }
            
            public int CantidadTurnosDisponibles { get; set; }
        //    public int CantidadTurnosDados {get; set; }
       

            
        }
        private string CalcularTurnoDisponible( DateTime fecha)
        {
          //  DateTime fecha = DateTime.Parse(cldTurno.SelectedDate.ToShortDateString());
            string m_strSQL = @" SELECT idTurno AS idturno, hora FROM LAB_Turno AS T (nolock)
                               WHERE (T.baja = 0) and T.idEfector="+oUser.IdEfector.IdEfector.ToString()+
                                      @" and T.idEfectorSolicitante= " + oCon.IdEfector.IdEfector.ToString() +

                               "          AND T.fecha='" + fecha.ToString("yyyyMMdd") + "'  AND T.IdItem=" + Request["idItem"].ToString() +" ORDER BY idturno DESC ";
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

          
         m_strSQL=  Ds.Tables[0].Rows.Count.ToString();

         return m_strSQL;



        }
    }
}