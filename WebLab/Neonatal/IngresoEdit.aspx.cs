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
using Business.Data;
using NHibernate;
using NHibernate.Expression;
//using Business;

namespace WebLab.Neonatal
{
    public partial class IngresoEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Pesquisa Neonatal");
                if (Session["idUsuario"] == null)
                    Response.Redirect("../FinSesion.aspx", false);
                else
                {
                    CargarListas();
                    if (Request["idSolicitud"] != null)
                    {
                        mostrarSolicitud(Request["idSolicitud"].ToString());                        
                    }
                    else
                    {
                        mostrarPaciente(int.Parse(Request.QueryString["idPaciente"]));
                        gvLista.Visible = false;
                    }
                    
                }
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

        private void mostrarSolicitud(string p)
        {
           SolicitudScreening oRegistro = new SolicitudScreening();
           oRegistro = (SolicitudScreening)oRegistro.Get(typeof(SolicitudScreening), int.Parse(p));

            mostrarPaciente(oRegistro.IdPaciente.IdPaciente);
            ddlEfector.SelectedValue = oRegistro.IdEfectorSolicitante.ToString();
            ddlEspecialista.SelectedValue = oRegistro.IdMedicoSolicitante.ToString();
            txtApellidoMaterno.Text = oRegistro.ApellidoMaterno.ToUpper();  /// puede salir de los datos de la madre
            txtApellidoPaterno.Text = oRegistro.ApellidoPaterno.ToUpper();
            txtHoraNacimiento.Value = oRegistro.HoraNacimiento;
            txtEdadGestacional.Value = oRegistro.EdadGestacional.ToString();
            txtPeso.Value = oRegistro.Peso.ToString();
            if (oRegistro.Prematuro) rdbNacimiento.SelectedValue = "1"; else rdbNacimiento.SelectedValue = "0";
            if (oRegistro.PrimeraMuestra) rdbMuestra.SelectedValue = "1"; else rdbMuestra.SelectedValue = "0";
            txtFechaExtraccion.Value = oRegistro.FechaExtraccion.ToShortDateString();
            txtHoraExtraccion.Value = oRegistro.HoraExtraccion;
            if (oRegistro.IngestaLeche24Horas) rdbIngestaLeche.SelectedValue = "1"; else rdbIngestaLeche.SelectedValue = "0";
            rdbAlimentacion.SelectedValue = oRegistro.TipoAlimentacion.ToString();
            if (oRegistro.Antibiotico) rdbAntibiotico.SelectedValue = "1"; else rdbAntibiotico.SelectedValue = "0";
            if (oRegistro.Transfusion) rdbTransfusion.SelectedValue = "1"; else rdbTransfusion.SelectedValue = "0";
            if (oRegistro.Corticoides) rdbCorticoide.SelectedValue = "1"; else rdbCorticoide.SelectedValue = "0";
            if (oRegistro.Dopamina) rdbDopamina.SelectedValue = "1"; else rdbDopamina.SelectedValue = "0";
            if (oRegistro.EnfermedadTiroideaMaterna) rdbAntecedenteTiroidea.SelectedValue = "1"; else rdbAntecedenteTiroidea.SelectedValue = "0";
            txtAntecedenteMadre.Text = oRegistro.AntecedentesMaternos;
            CargarGrillaAlarmas();

        }

        private void CargarGrillaAlarmas()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }



        private object LeerDatos()
        {
            string m_strSQL = @" SELECT  descripcion as [Alarmas] FROM  LAB_AlarmaScreening WHERE idSolicitudScreening =" + Request["idSolicitud"].ToString();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        private void mostrarPaciente(int idPaciente)
        {

            Utility oUtil = new Utility();
            ///Muestro los datos del paciente 
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), idPaciente);
            lblPaciente.Text = oPaciente.NumeroDocumento.ToString() + " - " + oPaciente.Apellido + " " + oPaciente.Nombre;
            lblInfoContacto.Text = oPaciente.InformacionContacto;
            
            //SysPaciente paciente = new SysPaciente(idPaciente);
            //lblPaciente.Text = paciente.Apellido + " " + paciente.Nombre;
            lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();
            //lblInfoContacto.Text = paciente.InformacionContacto;
            txtApellidoPaterno.Text = oPaciente.Apellido;

            //if (paciente.SysParentescoRecords.Count > 0)
            //{
       
            //    SysParentesco par = paciente.SysParentescoRecords[0];
            //    txtApellidoMaterno.Text = par.Apellido;                                 
            //}
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();
            Configuracion oC = new Configuracion(); 
            oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);

            ////////////Carga de combos de Efector
            string m_ssql = "SELECT idEfector, nombre FROM sys_Efector order by nombre ";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.SelectedValue = oC.IdEfector.IdEfector.ToString();

            ////////////Carga de combos de Medicos Solicitantes
            m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional   ORDER BY apellido, nombre ";
            oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
            ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));

          //  SysUsuario oUser = new SysUsuario(Session["idUsuario"]);
          //  if (!oUser.IsNew)
          //  { 
            

          //  SysEfectorCollection t = new SubSonic.Select()
          //  .From(SysEfector.Schema)
          //  .Where(SysEfector.Columns.IdTipoEfector).IsEqualTo(2)
          ////  .Where(SysEfector.Columns.IdEfector).IsEqualTo(oUser.IdEfector)
            

          // .OrderAsc(SysEfector.Columns.Nombre)
          //  .ExecuteAsCollection<SysEfectorCollection>();


          //  ddlEfector.DataSource = t;
          //  ddlEfector.DataValueField = SysEfector.Columns.IdEfector;
          //  ddlEfector.DataTextField = SysEfector.Columns.Nombre;
          //  ddlEfector.DataBind();
          //  try
          //  {
          //      ddlEfector.SelectedValue = oUser.IdEfector.ToString();
          //  }
          //  catch { }
            
          //  }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { 
               


                SolicitudScreening oRegistro = new SolicitudScreening();
                if (Request["idSolicitud"] != null) oRegistro = (SolicitudScreening)oRegistro.Get(typeof(SolicitudScreening), int.Parse(Request["idSolicitud"].ToString()));
                Guardar(oRegistro);
            }
           
        }

        private void Guardar(SolicitudScreening oRegistro)
        {
           
            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString())); ;
            
            Efector oEfectorSolicitante = new Efector();
            if (Request["idSolicitud"] == null)
            {
                Paciente oPaciente = new Paciente();
                oRegistro.IdPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));
            }
            oRegistro.IdEfectorSolicitante = (Efector)oEfectorSolicitante.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
            oRegistro.IdMedicoSolicitante =int.Parse( ddlEspecialista.SelectedValue);
            oRegistro.IdEfector = oUser.IdEfector;
            oRegistro.ApellidoMaterno = txtApellidoMaterno.Text.ToUpper();  /// puede salir de los datos de la madre
            oRegistro.ApellidoPaterno = txtApellidoPaterno.Text.ToUpper();
            oRegistro.HoraNacimiento = txtHoraNacimiento.Value;
            oRegistro.EdadGestacional = int.Parse(txtEdadGestacional.Value);
            oRegistro.Peso = decimal.Parse(txtPeso.Value);

            if (rdbNacimiento.SelectedValue == "1") oRegistro.Prematuro = true;  else oRegistro.Prematuro = false;
            if (rdbMuestra.SelectedValue == "1") oRegistro.PrimeraMuestra = true;  else  oRegistro.PrimeraMuestra = false;

            oRegistro.FechaExtraccion = DateTime.Parse(txtFechaExtraccion.Value);
            oRegistro.HoraExtraccion = txtHoraExtraccion.Value;

            if (rdbIngestaLeche.SelectedValue == "1") oRegistro.IngestaLeche24Horas = true; else oRegistro.IngestaLeche24Horas = false;
            
            oRegistro.TipoAlimentacion = int.Parse(rdbAlimentacion.SelectedValue);

            if (rdbAntibiotico.SelectedValue == "1") oRegistro.Antibiotico = true; else oRegistro.Antibiotico = false;
            if (rdbTransfusion.SelectedValue == "1") oRegistro.Transfusion = true; else oRegistro.Transfusion = false;            
            if (rdbCorticoide.SelectedValue == "1") oRegistro.Corticoides = true; else oRegistro.Corticoides = false;            
            if (rdbDopamina.SelectedValue == "1") oRegistro.Dopamina = true; else oRegistro.Dopamina = false;            
            if (rdbAntecedenteTiroidea.SelectedValue == "1") oRegistro.EnfermedadTiroideaMaterna = true; else oRegistro.EnfermedadTiroideaMaterna = false;

            oRegistro.AntecedentesMaternos = txtAntecedenteMadre.Text;
            oRegistro.IdUsuarioRegistro = oUser;
            oRegistro.FechaRegistro =DateTime.Parse( DateTime.Now.ToShortDateString());
            oRegistro.Save();

            GuardarAlarmas(oRegistro);
            //GuardarProtocolo(oRegistro);

            Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&Operacion=Alta&idServicio=1&idSolicitudScreening="+oRegistro.IdSolicitudScreening.ToString());
          //  Response.Redirect("IngresoMensaje.aspx?id=" + oRegistro.IdSolicitudScreening.ToString(), false);



        }

      

        private void GuardarAlarmas(SolicitudScreening oSolicitud)
        {
            if (Request["idSolicitud"] != null)
            { ///Elimina Alarmas/// 
                EliminarAlarmas(oSolicitud);
            }
            string descripcionAlarma = "";
      

            double horasolaNac = double.Parse(txtHoraNacimiento.Value.Substring(0, 2));
            double minutosoloNac = double.Parse(txtHoraNacimiento.Value.Substring(3, 2));
            DateTime fechaHoraNacimiento = DateTime.Parse(lblFechaNacimiento.Text).AddHours(horasolaNac).AddMinutes(minutosoloNac);

            double horasolaExt = double.Parse(txtHoraExtraccion.Value.Substring(0, 2));
            double minutosoloExt = double.Parse(txtHoraExtraccion.Value.Substring(3, 2));
            DateTime fechaHoraExtraccion = DateTime.Parse(txtFechaExtraccion.Value).AddHours(horasolaExt).AddMinutes(minutosoloExt);


            TimeSpan diferencia = fechaHoraExtraccion - fechaHoraNacimiento;
            double diferenciasHoras = diferencia.TotalHours;

            if (diferenciasHoras<36)
            {
                descripcionAlarma = "La extracción se realizó antes de las 36 horas de vida.";
                GuardarDescripcionAlarma(descripcionAlarma, oSolicitud);
            }


            if (oSolicitud.EdadGestacional < 35)
            {
                descripcionAlarma = "Deberá repetirse la muestra a los 15 días: La edad gestacional es menor a 35 semanas.";
                GuardarDescripcionAlarma(descripcionAlarma, oSolicitud);
            }
            if (oSolicitud.Transfusion)
            {
                descripcionAlarma = "Deberá repetirse la muestra a los 7 días: Por terapia transfuncional.";
                GuardarDescripcionAlarma(descripcionAlarma, oSolicitud);
            }
            if (oSolicitud.Prematuro)
            {
                descripcionAlarma = "Deberá repetirse la muestra: Por prematuro.";
                GuardarDescripcionAlarma(descripcionAlarma, oSolicitud);
            }
            if (oSolicitud.IngestaLeche24Horas)
            {
                descripcionAlarma = "El RN tuvo ingesta de leche en las primeras 24 horas";
                GuardarDescripcionAlarma(descripcionAlarma, oSolicitud);
            }


        }

        private void EliminarAlarmas(SolicitudScreening oSolicitud)
        {           
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(AlarmaScreening));
            crit.Add(Expression.Eq("IdSolicitudScreening", oSolicitud));            

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (AlarmaScreening oDetalle in detalle)
                {
                    oDetalle.Delete();
                }

            }
        }

        private void GuardarDescripcionAlarma(string descripcionAlarma, SolicitudScreening oSolicitud)
        {
            
            AlarmaScreening oRegistro = new AlarmaScreening();
            oRegistro.IdSolicitudScreening = oSolicitud;
            oRegistro.IdEfector = oSolicitud.IdEfector;
            oRegistro.Descripcion = descripcionAlarma;
            oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
        }

        protected void cvValidacionFechaExtraccion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DateTime fechaExtraccion =DateTime.Parse( txtFechaExtraccion.Value);
            DateTime fechaActual = DateTime.Parse( DateTime.Now.ToShortDateString());
            DateTime fechaNacimiento = DateTime.Parse(lblFechaNacimiento.Text);

            ///Validaciones: 
           
            ///Fecha y hora de extracion <= fecha actual
            ///Fecha y hora de extracion > a la fecha de nacimiento
            ///Peso <9999 (4 digitos)
            ///////////////////////////////////

        
            
            if (fechaExtraccion > fechaActual)
            {
                cvValidacionFechaExtraccion.Text = "La fecha de extracción no puede ser mayor a la fecha actual. Verifique.";
                args.IsValid = false;
            }
            else
            {
                if (fechaNacimiento > fechaExtraccion)
                {
                    cvValidacionFechaExtraccion.Text = "La fecha de extracción no pueder ser anterior a la fecha de nacimiento. Verifique.";
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;
            }


        }

        protected void imgPdf_Click(object sender, ImageClickEventArgs e)
        {

        }

        
    }
}
