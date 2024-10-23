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
using Business.Data;
using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Protocolos
{
    public partial class UnificarPaciente : System.Web.UI.Page
    { 
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            //MiltiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                VerificaPermisos("Unificar Pacientes");

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)            
                MostrarPaciente();
            
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
            {
                if (Session["s_permiso"] != null)
                {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                    switch (i_permiso)
                    {
                        case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                        case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    }
                }
                else Response.Redirect("../FinSesion.aspx", false);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        private void MostrarPaciente()
        {
            ///muestra solo el primer paciente con ese numero de documento.
            int i = 0;
            int dni = int.Parse(txtDni.Value);
           ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Paciente));
                crit.Add(Expression.Eq("NumeroDocumento", dni));
              crit.Add(Expression.Eq("IdEstado", 3)); 
                IList lst = crit.List();
              

            foreach (Paciente oPaciente in lst)
            {
                if (i == 0) 
                {
                    //lblEstado.Text = "VALIDADO";
                    //lblEstado.ForeColor = Color.Red;
                    lblidPaciente.Text = oPaciente.IdPaciente.ToString();
                    lblDU.Text = oPaciente.NumeroDocumento.ToString();
                    lblApellido.Text = oPaciente.Apellido;
                    lblNombre.Text = oPaciente.Nombre;
                    lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();
                    switch (oPaciente.IdSexo)
                    {
                        case 1: lblSexo.Text = "Sin definir"; break;
                        case 2: lblSexo.Text = "Femenino"; break;
                        case 3: lblSexo.Text = "Masculino"; break;
                    }
                    pnlPaciente.Visible = true;
                    pnlSinDatosPaciente.Visible = false;
                }
                i += 1;
            }
            /////////////////////////////////////////////////////
            // si no encontró un paciente validado busca a un identificado
            /////////////////////////////////////////////////////
            //  if (lst.Count == 0)
            if (i == 0)
            {
               //( ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit2 = m_session.CreateCriteria(typeof(Paciente));
                crit2.Add(Expression.Eq("NumeroDocumento", dni));
                crit2.Add(Expression.Eq("IdEstado", 1));
                IList lst2 = crit2.List();

                foreach (Paciente oPaciente in lst2)
                {
                    if (i == 0)
                    {
                        //lblEstado.Text = "IDENTIFICADO";
                        //lblEstado.ForeColor = Color.Green;
                        lblidPaciente.Text = oPaciente.IdPaciente.ToString();
                        lblDU.Text = oPaciente.NumeroDocumento.ToString();
                        lblApellido.Text = oPaciente.Apellido;
                        lblNombre.Text = oPaciente.Nombre;
                        lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();
                        switch (oPaciente.IdSexo)
                        {
                            case 1: lblSexo.Text = "Sin definir"; break;
                            case 2: lblSexo.Text = "Femenino"; break;
                            case 3: lblSexo.Text = "Masculino"; break;
                        }
                        pnlPaciente.Visible = true;
                        pnlSinDatosPaciente.Visible = false;
                    }
                    i += 1;
                }
            }

            if (i == 0)
            {
                pnlPaciente.Visible = false;
                pnlSinDatosPaciente.Visible = true;
            }
        }

        //private void MostrarPaciente2()
        //{
        //    Paciente oPaciente = new Paciente();
        //    oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), "NumeroDocumento", int.Parse(txtDni.Value));
        //    if (oPaciente != null)
        //    {
        //        if (oPaciente.IdEstado!=2)// si no es temporal lo muestra
        //        { 
        //            lblidPaciente.Text = oPaciente.IdPaciente.ToString();
        //        lblDU.Text = oPaciente.NumeroDocumento.ToString();
        //        lblApellido.Text = oPaciente.Apellido;
        //        lblNombre.Text = oPaciente.Nombre;
        //        lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();
        //      //  lblHC.Text = oPaciente.HistoriaClinica.ToString();
        //        switch (oPaciente.IdSexo)
        //        {
        //            case 1: lblSexo.Text = "Sin definir"; break;
        //            case 2: lblSexo.Text = "Femenino"; break;
        //            case 3: lblSexo.Text = "Masculino"; break;
        //        }

        //        Utility oUtil = new Utility();
        //        string[] edad = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento,DateTime.Now).Split(';');
        //        lblEdad.Text = edad[0].ToString() + " " + edad[1].ToUpper();
        //        pnlPaciente.Visible = true;
        //        pnlSinDatosPaciente.Visible = false;
        //        }
        //        else
        //        {
        //            pnlPaciente.Visible = false;
        //            pnlSinDatosPaciente.Visible = true;
        //        }
        //    }
        //    else
        //    {
        //        pnlPaciente.Visible = false;
        //        pnlSinDatosPaciente.Visible = true;
        //    }

        //}

        protected void btnBuscar0_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }
        
        private void CargarGrilla()
        {   
            try
            {
                gvLista.DataSource = LeerDatosProtocolos();
                gvLista.DataBind();
            }
            catch (Exception ex)
            {
                pnlSinDatos.Visible = true;
                pnlProtocolos.Visible = false;
                string exception = "";
                while (ex != null)
                {
                    exception = ex.Message + "<br>";

                }
            }            
            
        }

        private object LeerDatosProtocolos()
        {            
            string str_condicion = " WHERE P.baja=0 and P.idEfector="+oUser.IdEfector.IdEfector.ToString();

            if (oUser.IdEfector.IdEfector == 227)  //Es administrador puede unificar toda la base
                str_condicion = " WHERE P.baja=0 ";

            switch (ddlFiltro.SelectedValue)
            {                
                case "1": str_condicion += " and Pa.numeroDocumento=" + txtFiltro.Text; break;
                case "2": str_condicion += " and Pa.apellido like '%" + txtFiltro.Text + "%'"; break;
                case "3": str_condicion += " and Pa.nombre like '%" + txtFiltro.Text + "%'"; break;
                case "4":   str_condicion += " AND P.numero = " + int.Parse(txtFiltro.Text);                    break;
                    //{
                    ////Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                    ////switch (oCon.TipoNumeracionProtocolo)
                    ////    {
                    ////    case 0: // busqueda con autonumerico                        
                    //         str_condicion += " AND P.numero = " + int.Parse(txtFiltro.Text);                            
                    //         break;
                    //    case 1: //busqueda con numero diario
                    //         str_condicion += " AND P.numeroDiario = " + int.Parse(txtFiltro.Text);                                                
                    //        break;
                    //    case 2: //busqueda con numero de grupo    
                    //        str_condicion += " AND P.numeroSector = " + int.Parse(txtFiltro.Text);                                                   
                    //        break;
                    //    }
                    //} break;
            }
            
            string m_strSQL = @" SELECT  P.numero as numero, Pa.apellido + ' ' + Pa.nombre AS paciente,P.idProtocolo ,
                               case when Pa.idestado=2 then 0 else Pa.numeroDocumento end as dni, P.idPaciente as idPaciente ,E.nombre as efector 
                       FROM         dbo.LAB_Protocolo with (nolock) AS P 
                       INNER JOIN   dbo.Sys_Paciente with (nolock) AS Pa ON Pa.idPaciente = P.idPaciente 
                        inner join Sys_efector with (nolock) E on E.idEfector= P.idEfector " +  str_condicion;  
            
            DataSet Ds = new DataSet();
            //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            if (Ds.Tables[0].Rows.Count == 0)
            {
                pnlSinDatos.Visible = true;
                pnlProtocolos.Visible = false;
            }
            else
            {
                pnlProtocolos.Visible = true;
                pnlSinDatos.Visible = false;
            }
            return Ds.Tables[0];
            
        }

        protected void btnMover_Click(object sender, EventArgs e)
        {
            if (haySeleccion())
            {                
                MoverProtocolos();
               
                string popupScript = "<script language='JavaScript'> alert('Los protocolos se reasignaron al paciente principal')</script>";
                Page.RegisterClientScriptBlock("PopupScript", popupScript);
                CargarGrilla();         
            }
            else
            {
                string popupImprimir = "<script language='JavaScript'> alert('Debe seleccionar al menos un protocolo para asignarle paciente.') </script>";
                Page.RegisterStartupScript("PopupScriptImprimir", popupImprimir);
            }           
        }

        private bool haySeleccion()
        {
            bool hay = false;
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    hay = true; break;
                }
            }
            return hay;
        }


        private void MoverProtocolos()
        {
            if (lblidPaciente.Text != "")
            {
                foreach (GridViewRow row in gvLista.Rows)
                {
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {
                        ActualizarProtocolo(gvLista.DataKeys[row.RowIndex].Value.ToString());
                    }
                }
            }            
        }

        private void ActualizarProtocolo(string idProtocolo)
        {
            try
            {
                Utility oUtil = new Utility();
                Protocolo oRegistro = new Protocolo();
                oRegistro = (Protocolo)oRegistro.Get(typeof(Protocolo), int.Parse(idProtocolo));

                string pacienteAnterior = oRegistro.IdPaciente.NumeroDocumento.ToString();
                Usuario oUser = new Usuario();
                Paciente oPaciente = new Paciente();

                oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(lblidPaciente.Text));
                oRegistro.IdPaciente = oPaciente;
                string nuevoPaciente = oPaciente.NumeroDocumento.ToString();

                string[] edad = oUtil.DiferenciaFechas(oPaciente.FechaNacimiento, oRegistro.Fecha).Split(';');
                oRegistro.Edad = int.Parse(edad[0].ToString());

                switch (edad[1].ToUpper().Trim())
                {
                    case "A": oRegistro.UnidadEdad = 0; break;
                    case "M": oRegistro.UnidadEdad = 1; break;
                    case "D": oRegistro.UnidadEdad = 2; break;
                }
                
                switch (oPaciente.IdSexo)
                {
                    case 1: oRegistro.Sexo = "I"; break;
                    case 2: oRegistro.Sexo = "F"; break;
                    case 3: oRegistro.Sexo = "M"; break;
                }

                oRegistro.Save();
                oRegistro.GrabarAuditoriaProtocolo("Reasigna " + pacienteAnterior + " por " + nuevoPaciente, oUser.IdUsuario);
            }
            catch (Exception ex)
            {
                string exception = "";
                while (ex != null)
                {
                    exception = ex.Message + "<br>";

                }
                string popupScript = "<script language='JavaScript'> alert('No se ha podido reasignar. Verifique con el Administrador del Sistema"+exception+"')</script>";
                Page.RegisterClientScriptBlock("PopupScript", popupScript);
            }
        }

        protected void btnBorrar_Click(object sender, EventArgs e)
        {
            txtDni.Value = "";
            pnlPaciente.Visible = false;
            pnlSinDatosPaciente.Visible = false;
            lblidPaciente.Text = "";
        }

        protected void btnBorrar0_Click(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
            pnlProtocolos.Visible = false;
            pnlSinDatos.Visible = false;
            ddlFiltro.SelectedValue = "1";
        }

       
        }
    
}
