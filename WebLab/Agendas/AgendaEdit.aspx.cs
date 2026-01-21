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

namespace WebLab.Agendas
{
    public partial class AgendaEdit  : System.Web.UI.Page
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
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Agenda");
                CargarListas();
                txtFechaDesde.Value = DateTime.Now.ToShortDateString();
                txtLimite.Value = "0";
                if (Request["id"] != null)
                    MostrarDatos();
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
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }


        private void MostrarDatos()
        {
            Agenda oRegistro = new Agenda();
            oRegistro = (Agenda)oRegistro.Get(typeof(Agenda), int.Parse(Request["id"]));
            if (oRegistro != null)
            {
                ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.IdEfector.ToString();
                cboTipoServicio.SelectedValue = oRegistro.IdTipoServicio.IdTipoServicio.ToString();
                txtFechaDesde.Value = oRegistro.FechaDesde.ToShortDateString();
                txtFechaHasta.Value = oRegistro.FechaHasta.ToShortDateString();
                ddlItem.SelectedValue = oRegistro.IdItem.ToString();

                AgendaDia oItem = new AgendaDia();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(AgendaDia));
                crit.Add(Expression.Eq("IdAgenda", oRegistro));

                IList items = crit.List();
                foreach (AgendaDia oDia in items)
                {
                    int i = oDia.Dia;
                    cklDias.Items[i - 1].Selected = true;
                    txtLimite.Value = oDia.LimiteTurnos.ToString();
                    txtHoraDesde.Value = oDia.HoraDesde.ToString();
                    txtHoraHasta.Value = oDia.HoraHasta.ToString();
                    if (oDia.TipoTurno == 0)
                        rdbHorarioTurno.Items[0].Selected = true;
                    else
                        rdbHorarioTurno.Items[1].Selected = true;

                    if (oDia.Frecuencia != 0) txtFrecuenciaTurno.Value = oDia.Frecuencia.ToString(); ;
                }
            }

        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "select idTipoServicio,nombre  from Lab_TipoServicio (nolock) where idtiposervicio<>5 and baja = 0";
            oUtil.CargarCombo(cboTipoServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            CargarItems();

            ////////////Carga de combos de Efector
            m_ssql = "SELECT idEfector, nombre FROM sys_Efector  E (nolock) ";

            //if (Request["Operacion"].ToString() != "Modifica")  //alta
            m_ssql += " where exists (select 1 from LAB_EfectorRelacionado R (nolock) where E.idEfector = R.idEfectorRel and R.idefector = " + oUser.IdEfector.IdEfector.ToString() +
                ")  or (E.idEfector=" + oCon.IdEfector.IdEfector.ToString() + @" )";

            m_ssql += " order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            ddlEfector.SelectedValue = oCon.IdEfector.IdEfector.ToString();

            m_ssql = null;
            oUtil = null;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Agenda oRegistro = new Agenda();
                if (Request["id"] != null)
                {
                    oRegistro = (Agenda)oRegistro.Get(typeof(Agenda), int.Parse(Request["id"]));
                }
                    Guardar(oRegistro);
                Response.Redirect("AgendaList.aspx", false);
            }
    
        }


        private void Guardar(Agenda oRegistro)
        {
           
            TipoServicio oTipoServicio = new TipoServicio();
            Efector oEfectorSolicitante = new Efector();
           

            ///Se guarda el encabezado////
            oRegistro.IdTipoServicio= (TipoServicio) oTipoServicio.Get (typeof(TipoServicio), int.Parse(cboTipoServicio.SelectedValue));
            oRegistro.IdEfectorSolicitante= (Efector)oEfectorSolicitante.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
            oRegistro.FechaDesde = DateTime.Parse(txtFechaDesde.Value);
            oRegistro.FechaHasta = DateTime.Parse(txtFechaHasta.Value);
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.IdItem = int.Parse(ddlItem.SelectedValue);
            oRegistro.Save();             

            ///Si existe detalle para la agenda se elimina
            AgendaDia oItem = new AgendaDia();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(AgendaDia));
            crit.Add(Expression.Eq("IdAgenda", oRegistro));

            IList items = crit.List();
            foreach (AgendaDia oDias in items)
            {
                oDias.Delete();
            }
            
            ///

            ///Se guarda el detalle por dia//         
            for (int i=0; i<cklDias.Items.Count;i++)
            {
                if (cklDias.Items[i].Selected)
                {
                    AgendaDia oDia = new AgendaDia();
                    oDia.IdAgenda = oRegistro;
                    oDia.IdEfector = oRegistro.IdEfector;
                    oDia.Dia = i + 1;
                    oDia.LimiteTurnos = int.Parse(txtLimite.Value);
                    oDia.HoraDesde = txtHoraDesde.Value;
                    oDia.HoraHasta = txtHoraHasta.Value;
                    if (rdbHorarioTurno.Items[0].Selected)  oDia.TipoTurno = 0;
                    else   oDia.TipoTurno = 1;
                    if (txtFrecuenciaTurno.Value!="")  oDia.Frecuencia = int.Parse( txtFrecuenciaTurno.Value);

                    oDia.Save();
                }
            }
            ///////////////

           
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {

        }

        protected void ValidaCheckBox(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            for (int i = 0; i<this.cklDias.Items.Count; i++)
            {
                if (cklDias.Items[i].Selected)
                {
                    args.IsValid= true;                    
                }
            }
        }

        protected void rdbTipoDias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbTipoDias.Items[0].Selected)//Todos los días
                for (int i = 0; i < 7; i++)
                {
                    cklDias.Items[i].Selected = true;
                }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    cklDias.Items[i].Selected = true;
                }
                for (int i = 5; i < 7; i++)
                {
                    cklDias.Items[i].Selected = false;
                }
            }
            cklDias.UpdateAfterCallBack = true;

        }

        protected void rdbHorarioTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbHorarioTurno.Items[1].Selected)//Con frecuencia
                rfvFrecuencia.Enabled = true;
            else
                rfvFrecuencia.Enabled = false;
            rfvFrecuencia.UpdateAfterCallBack = true;
        }

        protected void cboTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarItems();
        }

        private void CargarItems()
        {
            //Usuario oUser = new Usuario();
            //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            if (oUser != null)
            {
                string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

                Utility oUtil = new Utility();
                //Carga del combo de determinaciones
                string m_ssql = "SELECT I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre FROM Lab_item I  " +
                        " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
                        " inner join lab_itemEfector as Ie on Ie.idItem= I.idItem and Ie.idEfector= " + oUser.IdEfector.IdEfector.ToString() +
                        " where A.baja=0 and I.baja=0 and IE.disponible =1 and A.idtipoServicio= " + cboTipoServicio.SelectedValue +
                        " AND (I.tipo= 'P') order by I.nombre ";
                oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
                ddlItem.Items.Insert(0, new ListItem("--SIN SELECCIONAR--", "0"));
            }

        }

        protected void customValidadorGeneral_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (oUser != null)
            {
                TipoServicio tp = new TipoServicio();
                tp = (TipoServicio)tp.Get(typeof(TipoServicio), "IdTipoServicio", cboTipoServicio.SelectedValue);
                DateTime fDesde = Convert.ToDateTime(txtFechaDesde.Value);
                DateTime fHasta = Convert.ToDateTime(txtFechaHasta.Value);

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Agenda));
                crit.Add(Expression.Eq("IdEfector", oCon.IdEfector));
                crit.Add(Expression.Eq("IdTipoServicio", tp));
                crit.Add(Expression.Eq("IdItem", ddlItem.SelectedValue));
                crit.Add(Expression.Le("FechaDesde", fHasta));
                crit.Add(Expression.Ge("FechaHasta", fDesde));

                if (Request["id"] != null) //Sacar el id de la agenda si es modificacion
                    crit.Add(Expression.Not(Expression.Eq("IdAgenda", int.Parse(Request["id"]))));

                IList items = crit.List();

                if (items.Count > 0)
                {
                    args.IsValid = false;
                    this.customValidadorGeneral.ErrorMessage = "Existe superposición de fechas con otra agenda";
                }
            }
        }
    }
}
