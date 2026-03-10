using Business;
using Business.Data;
using Business.Data.Laboratorio;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NHibernate.Expression;

namespace WebLab.Usuarios
{
    public partial class UsuarioEdit : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        CrystalReportSource oCr = new CrystalReportSource();




        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            oC = (Configuracion)oC.Get(typeof(Configuracion), 1); // "IdEfector", oUser.IdEfector);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Usuarios");
                CargarListas();
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
        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string m_ssql = @" SELECT idPerfil, nombre FROM Sys_Perfil (nolock) WHERE (activo = 1) ORDER BY nombre ";

            oUtil.CargarCombo(ddlPerfil, m_ssql, "idPerfil", "nombre");
            ddlPerfil.Items.Insert(0, new ListItem("Seleccione un perfil", "0"));

            m_ssql = @" SELECT idEfector, nombre FROM Sys_Efector  (nolock) ORDER BY nombre ";
            oUtil.CargarListBox(ddlEfector3, m_ssql, "idEfector", "nombre");
            ddlEfector3.Items.Insert(0, new ListItem("Seleccione un efector", "0"));
            ddlEfector3.Items.FindByValue("0").Selected = true; 

            m_ssql = @" SELECT idArea, nombre FROM LAB_Area  (nolock) where baja=0  ORDER BY nombre ";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));

            if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")
            {
                chkRequiereContrasenia.Checked = false;
                chkRequiereContrasenia.Visible = false;
            }

            
        }

        private void MostrarDatos()
        {
            Usuario oRegistro = new Usuario();
            oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["id"]));
            txtApellido.Text = oRegistro.Apellido;
            txtNombre.Text = oRegistro.Nombre;
            txtFirmaValidacion.Text = oRegistro.FirmaValidacion;
            //txtMatricula.Text = oRegistro.Matricula;
            txtUsername.Text = oRegistro.Username;
            chkRequiereContrasenia.Checked = oRegistro.RequiereCambioPass;
            txtPassword.Enabled = false;
            chkActivo.Checked = oRegistro.Activo;
            ddlPerfil.SelectedValue = oRegistro.IdPerfil.IdPerfil.ToString();
            //ddlEfector.SelectedValue = oRegistro.IdEfector.IdEfector.ToString();
            //ddlEfector.Enabled = false;
            if (ddlPerfil.SelectedValue == "15")
            {
                CargarEfectorLabo();
                ddlEfectorDestino.SelectedValue = oRegistro.IdEfectorDestino.IdEfector.ToString();
            }

            ddlArea.SelectedValue = oRegistro.IdArea.ToString();
            chkExterno.Checked = oRegistro.Externo;
            rfvPassword.Enabled = false;



            email.Value = oRegistro.Email;
            txtTelefono.Text = oRegistro.Telefono;
            btnBlanquear.Visible = true;

            chkAdministrador.Checked = oRegistro.Administrador;
            habilitarAdministrador();
            Usuario oAuditor = new Usuario();
            oAuditor = (Usuario)oAuditor.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oAuditor.GrabaAuditoria("Consulta", oRegistro.IdUsuario, oRegistro.Username);

            // MostrarEfectores(); //lo llamo en habilitarAdministrador para que se muestre el efector del admin

            ddlTipoAutenticacion.SelectedValue =  oRegistro.TipoAutenticacion.Trim();
           
            habilitarPorAutenticacion();
        }

        private void habilitarAdministrador()
        {
            if (chkAdministrador.Checked)
            {
                ddlArea.Enabled = false;
                ddlPerfil.Enabled = false;
                ddlPerfil.SelectedValue = "2";
                btnAgregarEfector.Enabled = false;
                lstEfectoresFinal.Enabled = false;
                ddlEfector3.Enabled = false;
                btnSacarEfector.Enabled = false;
                lstEfectoresFinal.Items.Clear();
                lstEfectoresFinal.Items.Add( new ListItem("SUBSECRETARIA DE SALUD", "227"));
                //Cuando uso Select para que se refleje el cambio en el combo se debe hacer por javascript
                ScriptManager.RegisterStartupScript( this, this.GetType(), "adminSelect2", "setAdministradorEfector(true);",  true);
            }
            else
            {
                ddlArea.Enabled = true;
                ddlPerfil.Enabled = true;
                btnAgregarEfector.Enabled = true;
                ddlEfector3.Enabled = true;
                lstEfectoresFinal.Enabled = true;
                btnSacarEfector.Enabled = true;
                MostrarEfectores();
               
                //Cuando uso Select para que se refleje el cambio en el combo se debe hacer por javascript
                ScriptManager.RegisterStartupScript(this, this.GetType(), "adminSelect2", "setAdministradorEfector(false);", true);
            }

            ddlArea.UpdateAfterCallBack = true;
            ddlPerfil.UpdateAfterCallBack = true;
            btnAgregarEfector.UpdateAfterCallBack = true;
            btnSacarEfector.UpdateAfterCallBack = true;
            lstEfectoresFinal.UpdateAfterCallBack = true;
        }

        

        //protected void btnGuardar_Click(object sender, EventArgs e)
        //{

        //}


        private void Guardar(Usuario oRegistro)
        {
            string accion = "Crea";
            if (oRegistro != null)
                accion = "Modifica";

            Perfil oPerfil = new Perfil();
            oPerfil = (Perfil)oPerfil.Get(typeof(Perfil), int.Parse(ddlPerfil.SelectedValue));

            Efector oEfector = new Efector();
            
            int idEfector = int.Parse(lstEfectoresFinal.Items[0].Value); //Tomo el primero de la lista
            oEfector = (Efector)oEfector.Get(typeof(Efector), idEfector);

            Efector oEfectorDestino = new Efector();
            if ((ddlEfectorDestino.SelectedValue != "0") && (ddlEfectorDestino.SelectedValue != ""))
                oEfectorDestino = (Efector)oEfectorDestino.Get(typeof(Efector), int.Parse(ddlEfectorDestino.SelectedValue));
            else
                oEfectorDestino = oEfector;

            oRegistro.IdEfector = oEfector;
            oRegistro.IdEfectorDestino = oEfectorDestino;
            oRegistro.IdPerfil = oPerfil;
            oRegistro.IdArea = int.Parse(ddlArea.SelectedValue);

            oRegistro.Apellido = txtApellido.Text;
            oRegistro.Nombre = txtNombre.Text;
            oRegistro.Legajo = "";
            oRegistro.FirmaValidacion = txtFirmaValidacion.Text;
            //oRegistro.Matricula=txtMatricula.Text;
            oRegistro.Administrador = chkAdministrador.Checked;
            oRegistro.Username = txtUsername.Text;

            oRegistro.Email = email.Value;
            oRegistro.Telefono = txtTelefono.Text;


            if (Request["id"] == null) //no se modifica contraseña
            {
                Utility oUtil = new Utility();

                string m_password = oUtil.Encrypt(txtPassword.Text);

                if (ddlTipoAutenticacion.SelectedValue == "ONELOGIN")
                {
                    m_password = oUtil.Encrypt(txtUsername.Text); //La contraseña de ONELOGIN es el username
                }
                oRegistro.Password = m_password;
            }

            

            if (oRegistro != null)
            {
                if ((oRegistro.Activo == true) && (chkActivo.Checked == false))
                    accion = "Inhabilita";
                if ((oRegistro.Activo == false) && (chkActivo.Checked == true))
                    accion = "Habilita";

                if (oRegistro.IdPerfil != oPerfil)
                    accion = "Cambia Perfil";

            }

            oRegistro.Activo = chkActivo.Checked;
            oRegistro.Externo = chkExterno.Checked;
            oRegistro.RequiereCambioPass = chkRequiereContrasenia.Checked;
            oRegistro.IdUsuarioActualizacion = int.Parse(Session["idUsuario"].ToString());
            oRegistro.FechaActualizacion = DateTime.Now;
            oRegistro.TipoAutenticacion = ddlTipoAutenticacion.SelectedValue;
            oRegistro.Save();


            Usuario oAuditor = new Usuario();
            oAuditor = (Usuario)oAuditor.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            oAuditor.GrabaAuditoria(accion, oRegistro.IdUsuario, oRegistro.Username);

            GuardarEfectores(oRegistro);


        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("UsuarioList.aspx");
        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                Usuario oRegistro = new Usuario();
                if (Request["id"] != null)
                {
                    oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["id"]));

                }

                Guardar(oRegistro);

                //if (Request["id"] != null)
                //    Response.Redirect("UsuarioList.aspx", false);
                //else
                //    Response.Redirect("UsuarioEdit.aspx", false);

                Response.Redirect("UsuarioList.aspx", false);
            }
        }

        protected void chkAdministrador_CheckedChanged(object sender, EventArgs e)
        {
            habilitarAdministrador();
        }

       

        //protected void btnAgregarEfector_Click(object sender, EventArgs e)
        //{

        //    if (Page.IsValid)
        //    {
        //        AgregarEfector();
        //        MostrarEfectores();

        //    }
        
        //}


       

        private DataTable LeerDatosEfector()
        {
            string m_strSQL = @" SELECT IR.idUsuarioEfector, R.nombre as nombre , R.idEfector
                               FROM Sys_UsuarioEfector IR (nolock) 
                              INNER JOIN sys_efector R (nolock) ON R.idEfector=IR.idEfector 
                               WHERE IR.idUsuario=" + Request["id"].ToString();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }

        //protected void gvListaEfector_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {



        //        ImageButton CmdEliminar = (ImageButton)e.Row.Cells[1].Controls[1];
        //        CmdEliminar.CommandArgument = this.gvListaEfector.DataKeys[e.Row.RowIndex].Value.ToString();
        //        CmdEliminar.CommandName = "Eliminar";
        //        CmdEliminar.ToolTip = "Eliminar";
        //    }
        //}

        //protected void gvListaEfector_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName != "Page")
        //    {
        //        switch (e.CommandName)
        //        {

        //            case "Eliminar":
        //                Eliminar(e.CommandArgument);
        //                MostrarEfectores();
        //                break;
        //        }
        //    }
        //}

       

        protected void btnAuditoria_Click(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {

                DataTable dtAuditoria = GetDataSetAuditoria();
                if (dtAuditoria.Columns.Count > 2)
                {

                    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = "Auditoria de Usuario";

                    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = "Sistema de Auditoria del SIL";

                    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = "Auditoria de Usuario";


                    oCr.Report.FileName = "../Informes/AuditoriaProtocolo.rpt";
                    oCr.ReportDocument.SetDataSource(dtAuditoria);
                    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                    oCr.DataBind();

                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF("Auditoria_" + txtUsername.Text);
                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);




                }
                else
                {
                    string popupScript = "<script language='JavaScript'> alert('No se encontraron datos para el numero de protocolo ingresado.'); </script>";
                    Page.RegisterStartupScript("PopupScript", popupScript);
                }
            }
        }

        private DataTable GetDataSetAuditoria()
        {
            string m_strSQL = "";

            string m_strCondicion = "";

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();

            //if (!oUser.Administrador)
            //{
            //    m_strCondicion = " and P.idefector=" + oUser.IdEfector.IdEfector.ToString();
            //}


            m_strSQL = @"  SELECT  A.username  AS numero, P.apellido as username, A.fecha AS fecha, A.hora, A.accion, '' as analisis, '' as valor, '' as valorAnterior
    FROM         	 LAB_Auditoriausuario AS A (nolock)   
    inner join  sys_usuario P (nolock) on P.idusuario= A.idusuarioregistro
    where  A.idusuario= " + Request["id"].ToString() + @" ORDER BY A.idAuditoriausuario";

            DataSet Ds1 = new DataSet();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1, "auditoria");


            DataTable data = Ds1.Tables[0];
            return data;


        }

        protected void btnBlanquear_Click(object sender, EventArgs e)
        {
            Usuario oRegistro = new Usuario();
            if (Request["id"] != null)
            {
                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), int.Parse(Request["id"]));


                Utility oUtil = new Utility();
                string m_password = oUtil.Encrypt(oRegistro.Username);
                oRegistro.Password = m_password;
                oRegistro.Save();
                oRegistro.GrabaAuditoria("Blanquea Contraseña", int.Parse(Session["idUsuario"].ToString()), oRegistro.Username);
            }

        }

        protected void customValidacionGeneral_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (Request["id"] == null) // alta
            {

                Usuario oRegistro = new Usuario();

                oRegistro = (Usuario)oRegistro.Get(typeof(Usuario), "Username", txtUsername.Text.Trim());
                if (oRegistro != null)
                {
                    args.IsValid = false;

                    return;

                }

            }
        }

        protected void ddlPerfil_SelectedIndexChanged(object sender, EventArgs e)
        {
            rvEfectorDestino.Enabled = false;
            ddlEfectorDestino.Visible = false;
            if (ddlPerfil.SelectedValue == "15")
            {
                CargarEfectorLabo();
            }
        }

        private void CargarEfectorLabo()
        {
            Utility oUtil = new Utility();



            string m_ssql = @" SELECT idEfector, nombre FROM Sys_Efector (nolock) where  idEfector in (select idEfector from lab_configuracion (nolock) ) ORDER BY nombre ";
            oUtil.CargarCombo(ddlEfectorDestino, m_ssql, "idEfector", "nombre");
            //    ddlEfector.Items.Insert(0, new ListItem("Seleccione un efector", "0"));

            ddlEfectorDestino.Items.Insert(0, new ListItem("Seleccione Laboratorio", "0"));
            ddlEfectorDestino.UpdateAfterCallBack = true;
            ddlEfectorDestino.Visible = true;
            rvEfectorDestino.Enabled = true;
            rvEfectorDestino.UpdateAfterCallBack = true;
        }

        protected void customValidacionGeneral0_ServerValidate(object source, ServerValidateEventArgs args)
        {
            
                if (txtUsername.Text.Trim().Length<6)                
                {
                    args.IsValid = false;
                    if (ddlTipoAutenticacion.SelectedValue == "SIL")
                        customValidacionGeneral0.ErrorMessage = "Usuario debe contener al menos 6 caracteres(letras o numeros)";
                    else
                    customValidacionGeneral0.ErrorMessage = "Usuario debe contener al menos 6 numeros";
                    
                    return;
                }

            
        }
        
        private void habilitarPorAutenticacion()
        {
            if (ddlTipoAutenticacion.SelectedValue == "ONELOGIN")
            {
                //No se habilita “Requiere nueva contraseña al ingresar”
                chkRequiereContrasenia.Enabled = false;
                chkRequiereContrasenia.Checked = false;
                //No se habilita “exclusivo Río Negro”
                chkExterno.Enabled = false;
                //Por defecto la contraseña es el username
                txtPassword.Enabled = false;
                rfvPassword.Enabled = false;
            }
            else
            {
                chkRequiereContrasenia.Enabled = true;
                chkExterno.Enabled = true;

                //Se puede poner una contraseña
                if (Request["id"] == null){ txtPassword.Enabled = true; rfvPassword.Enabled = true; }
                else
                    txtPassword.Enabled = false;
            }


        }
        protected void ddlTipoAutenticacion_SelectedIndexChanged(object sender, EventArgs e)
        {
          
           habilitarPorAutenticacion();
            
        }


        protected void customValidacionGeneral1_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            if (ddlTipoAutenticacion.SelectedValue == "ONELOGIN")
            {
                //El username debe ser un numero (numero de documento) no debe admitir letras ni caracteres especiales.

                bool validate = Regex.IsMatch(txtUsername.Text, @"^\d+$");
                if (!validate)
                {
                    args.IsValid = false;
                    return;
                }
            }
        }

        protected void customValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlTipoAutenticacion.SelectedValue == "ONELOGIN" && chkExterno.Checked)
            {
                customValidatorExterno.ErrorMessage = "Usuario ONLOGIN no puede ser “Exclusivo Río Negro”";
                args.IsValid = false;
                return;

            }
        }

       

       

        protected void customValidatorEfector_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (lstEfectoresFinal.Items.Count == 0)
            {
                args.IsValid = false;
                this.customValidatorEfector.ErrorMessage = "Debe ingresar al menos un efector para el usuario";
                return;
            }
        }



        #region Efectores

        protected void btnAgregarEfector_Click(object sender, EventArgs e)
        {
            AgregarEfector();
        }
      
        private void AgregarEfector()
        {
            lblMensajeEfector.Visible = false;
            if (ddlEfector3.SelectedValue != "0")
            {
                bool agrego = true;
               ListItem efectorEncontrado = lstEfectoresFinal.Items.FindByValue(ddlEfector3.SelectedItem.Value); //Verifica si ya fue agregado el efctor

                if (efectorEncontrado != null)
                {
                    agrego = false;
                }

                if (agrego)
                {
                    lstEfectoresFinal.Items.Add(ddlEfector3.SelectedItem);
                    lstEfectoresFinal.UpdateAfterCallBack = true;
                }
                else
                {
                    lblMensajeEfector.Visible = true;
                    lblMensajeEfector.Text = "Alerta: Efector ya ingresado para el usuario.";

                }
            }
            lblMensajeEfector.UpdateAfterCallBack = true;
        }


        protected void btnSacarEfector_Click(object sender, ImageClickEventArgs e)
        {
            if (lstEfectoresFinal.SelectedValue != "")
            {
                EliminarEfector(lstEfectoresFinal.SelectedValue);
            }
        }

        private void GuardarEfectores(Business.Data.Usuario oUsuario)
        {
            ///Eliminar los efectores y volverlos a crear
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(UsuarioEfector));
            crit.Add(Expression.Eq("IdUsuario", oUsuario));
            IList lista = crit.List();
            

            foreach (UsuarioEfector oUsuarioEfector in lista)
            {
                oUsuarioEfector.Delete();
            }

           
            if (lstEfectoresFinal.Items.Count > 0)
            {
                /////Crea nuevamente los efectores
                for (int i = 0; i < lstEfectoresFinal.Items.Count; i++)
                {

                    UsuarioEfector oRegistro = new UsuarioEfector();
                    int idEfector = int.Parse(lstEfectoresFinal.Items[i].Value);
                    Efector oEfector = new Efector();
                    oEfector = (Efector)oEfector.Get(typeof(Efector), idEfector);
                    oRegistro.IdUsuario = oUsuario;
                    oRegistro.IdEfector = oEfector;
                    oRegistro.Activo = true;
                    oRegistro.Save();


                    Usuario oAuditor = new Usuario();
                    oAuditor = (Usuario)oAuditor.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                    oAuditor.GrabaAuditoria("Vincula " + oEfector.Nombre, oRegistro.IdUsuario.IdUsuario, oRegistro.IdUsuario.Username);
                }
            }
        }

        private void MostrarEfectores()
        {
            //ddlEfector2.SelectedValue = "0";

            //gvListaEfector.AutoGenerateColumns = false;
            //gvListaEfector.DataSource = LeerDatosEfector();
            //gvListaEfector.DataBind();
            //gvListaEfector.UpdateAfterCallBack = true;
            lstEfectoresFinal.Items.Clear();
            if (Request["id"]  != null)
            {
                DataTable dt = LeerDatosEfector();
                
                foreach (DataRow item in dt.Rows)
                {
                    ListItem listItem = new ListItem( item[1].ToString(), item[2].ToString()); //Texto R.nombre as nombre , Value R.idEfector 
                    lstEfectoresFinal.Items.Add(listItem);
                }
            }
           
        }


        private void EliminarEfector(object idEfector)
        {
            bool puedeEliminar = true;

            lblMensajeEfector.Visible = false;
            if (lstEfectoresFinal.Items.Count > 1)
            {
                if (Request["id"] != null)
                {
                    int idUsuario = int.Parse(Request["id"].ToString());
                    Usuario usuario = new Usuario();
                    usuario = (Usuario)usuario.Get(typeof(Usuario), idUsuario);

                    Efector ef = new Efector();
                    ef = (Efector)ef.Get(typeof(Efector), int.Parse(idEfector.ToString()));
                    
                    UsuarioEfector oRegistro = new UsuarioEfector();
                    oRegistro = (UsuarioEfector)oRegistro.Get(typeof(UsuarioEfector), "IdEfector", ef, "IdUsuario", usuario);

                    if (oRegistro != null) //si esta en base lo elimina sino no hace nada
                    {
                        string s_efector = oRegistro.IdEfector.Nombre;
                        string s_username = oRegistro.IdUsuario.Username;
                        oRegistro.Delete();
                        /////Auditoria
                        Usuario oAuditor = new Usuario();
                        oAuditor = (Usuario)oAuditor.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                        oAuditor.GrabaAuditoria("DesVincula " + s_efector.TrimStart().TrimEnd(), idUsuario, s_username);
                    }
                }
                    

            }
            else
            {
                lblMensajeEfector.Visible = true;
                lblMensajeEfector.Text = "Alerta: Debe tener al menos 1 efector asociado";
                lblMensajeEfector.UpdateAfterCallBack = true;
                puedeEliminar = false;
            }
                
            
            if (puedeEliminar)
            {
                lstEfectoresFinal.Items.Remove(lstEfectoresFinal.SelectedItem);
                lstEfectoresFinal.UpdateAfterCallBack = true;
            }
        }
        #endregion


    }
}
