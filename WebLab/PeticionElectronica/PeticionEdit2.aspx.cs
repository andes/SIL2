using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Business;
using System.Data.SqlClient;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Web.UI.HtmlControls;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using System.Drawing;

namespace WebLab.PeticionElectronica
{
    public partial class PeticionEdit2 : System.Web.UI.Page
    {
        CrystalReportSource oCr = new CrystalReportSource();
        private Random random = new Random();
        private static int TEST = 0;

        private bool IsTokenValid()
        {
            bool result = double.Parse(hidToken.Value) == ((double)Session["NextToken"]);
            SetToken();
            return result;
        }

        private void SetToken()
        {
            double next = random.Next();
            hidToken.Value = next + "";
            Session["NextToken"] = next;
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {            
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            if (Request["master"] != null)
                Page.MasterPageFile = "~/Site1.master";
            else
                Page.MasterPageFile = "~/Site2.master";           
        }

        protected void rdbTipoPlanilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbTipoPlanilla.SelectedValue == "1") { CargarAreasLaboratorio(); ddlRutina.Enabled = false; }
            else { CargarRutinasLaboratorio(); ddlRutina.Enabled = true; }
        }
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    VerificaPermisos("PeticionEdit");
                    SetToken();
                    Usuario oUser = new Usuario();
                    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    lblSolicitante.Text = oUser.Apellido + " " + oUser.Nombre;
                    CargarListas();
                    //    MostrarPeticion();
                    MostrarPaciente();
                    lblFecha.Text = DateTime.Now.ToShortDateString();
                    lblHora.Text = DateTime.Now.ToShortTimeString();
                    //CargarAreasLaboratorio();
                    CargarRutinasLaboratorio();
                    CargarAreasMicrobiologia();
                    CargarGrilla();
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);       
               
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
                    case 0: {
                        if (Request["master"] != null) Response.Redirect("../AccesoDenegado.aspx?master=1", false);
                        else  Response.Redirect("../AccesoDenegado.aspx", false); 
                    } break;
                    case 1: ///Solo lectura
                        {
                            btnGuardarEnviar.Visible = false;
                            btnGuardarMicrobiologia.Visible = false;
                        } break;

                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();                     

            ///Carga de grupos de numeración solo si el tipo de numeración es 2: por Grupos
            string m_ssql = "SELECT  idSectorServicio,  nombre  as nombre FROM LAB_SectorServicio WHERE (baja = 0) order by nombre";
            oUtil.CargarCombo(ddlSectorServicio, m_ssql, "idSectorServicio", "nombre");
            ddlSectorServicio.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            ///Carga de combos de Origen
            m_ssql = " SELECT  idOrigen, nombre FROM LAB_Origen WHERE idOrigen<5 and (baja = 0)";
            oUtil.CargarCombo(ddlOrigen, m_ssql, "idOrigen", "nombre");
            ddlOrigen.Items.Insert(0, new ListItem("--Seleccione--", "0"));

            m_ssql = " SELECT  idRutina, nombre FROM LAB_Rutina WHERE (baja = 0) AND (idTipoServicio = 1) order by nombre";
            oUtil.CargarCombo(ddlRutina, m_ssql, "idRutina", "nombre");
            ddlRutina.Items.Insert(0, new ListItem("--Todos los perfiles--", "0"));            

            m_ssql = "SELECT idMuestra, nombre + ' - ' + codigo as nombre FROM LAB_Muestra";            
            m_ssql += " where baja=0 ";
            m_ssql += " order by nombre ";

            oUtil.CargarCombo(ddlMuestra, m_ssql, "idMuestra", "nombre");
            ddlMuestra.Items.Insert(0, new ListItem("--Seleccione Muestra--", "0"));
            //rvMuestra.Enabled = true;


         //   if (Request["Operacion"].ToString() == "Carga")///muestra solo los activos
            m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional WHERE activo=1 ORDER BY apellido, nombre ";
           // else
             //   m_ssql = "SELECT idProfesional, apellido + ' ' + nombre AS nombre FROM Sys_Profesional   ORDER BY apellido, nombre ";
            oUtil.CargarCombo(ddlEspecialista, m_ssql, "idProfesional", "nombre");
            ddlEspecialista.Items.Insert(0, new ListItem("No identificado", "0"));



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
            ///
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_GetPeticionesPaciente]";
          
            cmd.Parameters.Add("@idPaciente", SqlDbType.Int);
            cmd.Parameters["@idPaciente"].Value = int.Parse(Request["idPaciente"].ToString());

            cmd.Parameters.Add("@idPeticion", SqlDbType.Int);
            cmd.Parameters["@idPeticion"].Value = 0; // int.Parse(Request["idPaciente"].ToString());       

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
           

            return Ds.Tables[0];
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string idPeticion = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();


                ImageButton CmdModificar = (ImageButton)e.Row.Cells[10].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Imprimir";
                CmdModificar.ToolTip = "Imprimir";

                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[11].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

                //ImageButton CmdPeticionNueva = (ImageButton)e.Row.Cells[10].Controls[1];
                //CmdPeticionNueva.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                //CmdPeticionNueva.CommandName = "Peticion";
                //CmdPeticionNueva.ToolTip = "Peticion";

                //System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                if (e.Row.Cells[8].Text != "-")                //simple
                {
                    string estado = getEstadoProtocolo(idPeticion);
                    e.Row.Cells[9].Text = estado;
                    switch (estado)
                    {
                        case "En proceso":  e.Row.Cells[9].ForeColor = Color.YellowGreen; break;
                        case "Terminado": e.Row.Cells[9].ForeColor = Color.Green; break;
                        case "Recibida": e.Row.Cells[9].ForeColor = Color.Red; break;
                    }
                    //hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                    //e.Row.Cells[0].Controls.Add(hlnk);
                    CmdEliminar.Visible = false;
                    
                }
                else
                {
                    //Image hlnk = new Image();
                    //hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                    //e.Row.Cells[0].Controls.Add(hlnk);
                    CmdEliminar.Visible = true;
                    
                }
            }

        }

        private string getEstadoProtocolo(string idPeticion)
        {
            string dev = "Recibida";
            Peticion oRegistro = new Peticion();
            oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(idPeticion));

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), oRegistro.IdProtocolo);
            switch (oProtocolo.Estado)
            { case 1:dev = "En proceso"; break; case 2:dev = "Terminado"; break; default: dev = "Recibida"; break; }

            return dev;
        }
        private void Anular(Business.Data.Laboratorio.Peticion oRegistro)
        {

            oRegistro.Baja = true;
            oRegistro.Save();
            if (Request["master"] != null)
                Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&master=1", false);
            else
                Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString(), false);
            //CargarGrilla();
        }
        private void ImprimirResultado(Protocolo oProtocolo, string tipo)
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            string parametroPaciente = "";
            string parametroProtocolo = "";


            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();

            ParameterDiscreteValue ImprimirHojasSeparadas = new ParameterDiscreteValue();


            ParameterDiscreteValue tipoNumeracion = new ParameterDiscreteValue();
            tipoNumeracion.Value = oCon.TipoNumeracionProtocolo;


            ///////Redefinir el tipo de firma electronica (Serían dos reportes distintos)
            ParameterDiscreteValue conPie = new ParameterDiscreteValue();

            ParameterDiscreteValue conLogo = new ParameterDiscreteValue();
            if (oCon.RutaLogo != "")
                conLogo.Value = true;
            else
                conLogo.Value = false;

            if (oProtocolo.IdTipoServicio.IdTipoServicio == 1) //laboratorio
            {
                encabezado1.Value = oCon.EncabezadoLinea1;
                encabezado2.Value = oCon.EncabezadoLinea2;
                encabezado3.Value = oCon.EncabezadoLinea3;


                if (oCon.ResultadoEdad) parametroPaciente = "1"; else parametroPaciente = "0";
                if (oCon.ResultadoFNacimiento) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoSexo) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDNI) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoHC) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDomicilio) parametroPaciente += "1"; else parametroPaciente += "0";


                if (oCon.ResultadoNumeroRegistro) parametroProtocolo = "1"; else parametroProtocolo = "0";
                if (oCon.ResultadoFechaEntrega) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSector) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSolicitante) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoOrigen) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoPrioridad) parametroProtocolo += "1"; else parametroProtocolo += "0";



                ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultado;

                conPie.Value = oCon.FirmaElectronicaLaboratorio.ToString();

                if (oCon.OrdenImpresionLaboratorio)
                {
                    if (oCon.TipoHojaImpresionResultado == "A4") oCr.Report.FileName = "../Informes/ResultadoSinOrden.rpt";
                    else oCr.Report.FileName = "../Informes/ResultadoSinOrdenA5.rpt";
                }
                else
                {
                    if (oCon.TipoHojaImpresionResultado == "A4") oCr.Report.FileName = "../Informes/Resultado.rpt";
                    else oCr.Report.FileName = "../Informes/ResultadoA5.rpt";
                }
            }
            if (oProtocolo.IdTipoServicio.IdTipoServicio == 3) //microbilogia
            {
                encabezado1.Value = oCon.EncabezadoLinea1Microbiologia;
                encabezado2.Value = oCon.EncabezadoLinea2Microbiologia;
                encabezado3.Value = oCon.EncabezadoLinea3Microbiologia;


                if (oCon.ResultadoEdadMicrobiologia) parametroPaciente = "1"; else parametroPaciente = "0";
                if (oCon.ResultadoFNacimientoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoSexoMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDNIMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoHCMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";
                if (oCon.ResultadoDomicilioMicrobiologia) parametroPaciente += "1"; else parametroPaciente += "0";


                if (oCon.ResultadoNumeroRegistroMicrobiologia) parametroProtocolo = "1"; else parametroProtocolo = "0";
                if (oCon.ResultadoFechaEntregaMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSectorMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoSolicitanteMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoOrigenMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";
                if (oCon.ResultadoPrioridadMicrobiologia) parametroProtocolo += "1"; else parametroProtocolo += "0";

                ImprimirHojasSeparadas.Value = oCon.TipoImpresionResultadoMicrobiologia;

                conPie.Value = oCon.FirmaElectronicaMicrobiologia.ToString();

                if (oCon.TipoHojaImpresionResultadoMicrobiologia == "A4")
                    oCr.Report.FileName = "../Informes/ResultadoMicrobiologia.rpt";
                else
                    oCr.Report.FileName = "../Informes/ResultadoMicrobiologiaA5.rpt";
            }



            ParameterDiscreteValue datosPaciente = new ParameterDiscreteValue();
            datosPaciente.Value = parametroPaciente;

            ParameterDiscreteValue datosProtocolo = new ParameterDiscreteValue();
            datosProtocolo.Value = parametroProtocolo;



            string m_filtro = " WHERE idProtocolo =" + oProtocolo.IdProtocolo;

       //     if (Request["idArea"].ToString() != "0") m_filtro += " and idArea=" + Request["idArea"].ToString();

            if (oProtocolo.IdTipoServicio.IdTipoServicio == 1)
            {
                //if (Request["Operacion"].ToString() != "Valida")
                //{
                //    if (Request["validado"].ToString() == "1") m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";
                //}

                //if (Request["Operacion"].ToString() == "HC")
                //{
                    m_filtro += " and (idusuariovalida> 0 or idUsuarioValidaObservacion>0 )";
                //}


            }

            oCr.ReportDocument.SetDataSource(oProtocolo.GetDataSet("Resultados", m_filtro, oProtocolo.IdTipoServicio.IdTipoServicio));
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
            oCr.ReportDocument.ParameterFields[3].CurrentValues.Add(conLogo);
            oCr.ReportDocument.ParameterFields[4].CurrentValues.Add(datosPaciente);
            oCr.ReportDocument.ParameterFields[5].CurrentValues.Add(ImprimirHojasSeparadas);
            oCr.ReportDocument.ParameterFields[6].CurrentValues.Add(tipoNumeracion);
            oCr.ReportDocument.ParameterFields[7].CurrentValues.Add(conPie);
            oCr.ReportDocument.ParameterFields[8].CurrentValues.Add(datosProtocolo);






            oCr.DataBind();

            string s_nombreProtocolo = "";
            switch (oCon.TipoNumeracionProtocolo)
            {
                case 0: s_nombreProtocolo = oProtocolo.Numero.ToString(); break;
                case 1: s_nombreProtocolo = oProtocolo.NumeroDiario.ToString(); break;
                case 2: s_nombreProtocolo = oProtocolo.PrefijoSector + oProtocolo.NumeroSector.ToString(); break;
                case 3: s_nombreProtocolo = oProtocolo.NumeroTipoServicio.ToString(); break;
            }

            //if (tipo != "PDF")
            //{
            //    oProtocolo.GrabarAuditoriaProtocolo("Imprime Resultados", int.Parse(Session["idUsuario"].ToString()));
            //    Session["Impresora"] = ddlImpresora.SelectedValue;

            //    oCr.ReportDocument.PrintOptions.PrinterName = ddlImpresora.SelectedValue;
            //    oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);

            //    oProtocolo.Impreso = true;
            //    oProtocolo.Save();
            //}
            //else
            //{
                oProtocolo.GrabarAuditoriaProtocolo("Genera PDF Resultados", int.Parse(Session["idUsuario"].ToString()));
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, s_nombreProtocolo + ".pdf");
            //MemoryStream oStream; // using System.IO
            //oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment;filename=" + s_nombreProtocolo + ".pdf");

            //Response.BinaryWrite(oStream.ToArray());
            //Response.End();
            //}

        }
        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Peticion oRegistro = new Peticion();
            oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(e.CommandArgument.ToString()));
            switch (e.CommandName)
            {
                case "Imprimir":
                    if (oRegistro.IdProtocolo==0)
                    Imprimir(oRegistro); 
                    else
                    { 
                        Protocolo oProtocolo = new Protocolo();
                        oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo),oRegistro.IdProtocolo);
                        if (oProtocolo.Estado > 0) ImprimirResultado(oProtocolo, "Pdf");
                        else
                        {  string popupScript = "<script language='JavaScript'> alert('El protocolo aún no tiene resultados'); </script>";Page.RegisterStartupScript("PopupScript", popupScript);
                        }
                    }
                    break;
                //    {
                //        Response.Redirect("PeticionEdit.aspx?idPeticion=" + oRegistro.IdPeticion.ToString() + "&idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&idTipoServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&Modifica=1");
                //    }
                //    break;
                case "Eliminar":
                    Anular(oRegistro);
                    break;
                //case "Peticion":
                //    {

                //        Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&idTipoServicio=" + oRegistro.IdTipoServicio.IdTipoServicio.ToString() + "&Modifica=0");
                //    }
                //    break;

            }
        }
        //private void MostrarPeticion()
        //{

        //    if (Request["Modifica"].ToString() == "1")
        //    {
        //        Peticion oRegistro = new Peticion();
        //        oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));
        //   //     lblNuevaPeticion.Visible = false;
        //        lblNumeroPeticion.Text ="Pedido Nro.:"+ oRegistro.IdPeticion.ToString();
        //        lblFecha.Text = oRegistro.Fecha.ToShortDateString();
        //        lblHora.Text = oRegistro.Hora;
        //        //txtObservaciones.Text = oRegistro.Observaciones;

        //        ddlOrigen.SelectedValue = oRegistro.IdOrigen.IdOrigen.ToString();

        //        if (oRegistro.IdProtocolo > 0)
        //        {
        //            DataList1.Enabled = false;
        //            lblEstado.Text = "RECIBIDA";
        //            btnGuardar.Visible = false;
        //            btnGuardarEnviar.Visible = false;
        //        }
        //        else
        //        {
        //            if (oRegistro.Enviada)
        //            {
        //                lblEstado.Text = "ENVIADA"; btnGuardar.Visible = false; btnGuardarEnviar.Visible = false; DataList1.Enabled = false;
        //            }
        //            else
        //            {
        //                lblEstado.Text = "PENDIENTE";
        //            }
        //        }


        //        ISession m_session = NHibernateHttpModule.CurrentSession;
        //        ICriteria crit = m_session.CreateCriteria(typeof(PeticionItem));
        //        crit.Add(Expression.Eq("IdPeticion", oRegistro));
        //        IList items = crit.List();
        //        foreach (PeticionItem oDet in items)
        //        {
        //            if (txtSeleccion.Text == "")
        //            {
        //                lblIdSeleccion.Text = oDet.IdItem.IdItem.ToString();
        //                txtSeleccion.Text = oDet.IdItem.Nombre + Environment.NewLine;
        //            }
        //            else
        //            {
        //                lblIdSeleccion.Text += ";" + oDet.IdItem.IdItem.ToString();
        //                txtSeleccion.Text = txtSeleccion.Text + oDet.IdItem.Nombre + Environment.NewLine;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //lblNuevaPeticion.Visible = true;
        //        lblNumeroPeticion.Text = "";
        //        lblEstado.Text  ="";
        //        lblFecha.Text = DateTime.Now.ToShortDateString();
        //        lblHora.Text = DateTime.Now.ToShortTimeString();
        //    }
        //}

      

        private void CargarPracticasUrgencias()
        {
        //    chkAnalisisUrgentes.DataSource = LeerAnalisis("25");/// 25 es el id de urgencias
        //    chkAnalisisUrgentes.DataValueField = "idItem";
        //    chkAnalisisUrgentes.DataTextField = "nombre";
        //    chkAnalisisUrgentes.DataBind();
            // MostrarCantidades();        
        }

        private void CargarAreasLaboratorio()
        {
            DataList1.DataSource = LeerAreas(1);
            DataList1.DataBind();


             
        }
        private void CargarAreasMicrobiologia()
        {
        


            datalistMicrobiologia.DataSource = LeerAreas(3);
            datalistMicrobiologia.DataBind();
            // MostrarCantidades();
        }


        private void CargarRutinasLaboratorio()
        {
            DataList1.DataSource = LeerRutinas();
            DataList1.DataBind();



        }

        private object LeerRutinas()
        {
            
            string m_strSQL = @"  select idRutina as idArea, nombre as area from lab_rutina where idTipoServicio=1 and  baja=0 order by nombre ";            
            if (ddlRutina.SelectedValue!="0")
                m_strSQL = @"  select idRutina as idArea, nombre as area from lab_rutina where idRutina="+ddlRutina.SelectedValue +" and idTipoServicio=1 and  baja=0 order by nombre ";            
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        private void MostrarPaciente()
        {
            ///Muestro los datos del paciente 
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));
            if (oPaciente.IdEstado == 1)
                lblDU.Text = oPaciente.NumeroDocumento.ToString();// + " - " + oPaciente.Apellido + " " + oPaciente.Nombre;
            else
                lblDU.Text = "(Sin DU Temporal)"; // +oPaciente.Apellido + " " + oPaciente.Nombre;

            lblApellido.Text = oPaciente.Apellido.ToUpper();
            lblNombre.Text = oPaciente.Nombre.ToUpper();
            //lblIdPaciente.Text = oPaciente.IdPaciente.ToString();
            //ddlObraSocial.SelectedValue = oPaciente.IdObraSocial.ToString();
            lblFechaNacimiento.Text = oPaciente.FechaNacimiento.ToShortDateString();
            switch (oPaciente.IdSexo)
            {
                case 1: lblSexo.Text = "Indeterminado"; break;
                case 2: lblSexo.Text = "Femenino"; break;
                case 3: lblSexo.Text = "Masculino"; break;
            }
        
            //////Busca turnos con inasistencia
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Peticion));
            crit.Add(Expression.Eq("IdPaciente", oPaciente));
            crit.Add(Expression.Eq("IdProtocolo", 0));
            crit.Add(Expression.Le("Fecha", DateTime.Now));
            crit.Add(Expression.Le("Baja",false));

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                lblAlerta.Visible = true;
                lblAlerta.Text = "El paciente tiene " + detalle.Count.ToString() + " peticiones sin generar protocolo";
            }
            else lblAlerta.Visible = false;            
            ///// fin de busqueda

        }

        private object LeerAreas(int idServicio)
        {
            string m_strSQL = @"  SELECT distinct lab_area.idArea, lab_area.nombre as area FROM lab_area inner join lab_item on Lab_item.idArea= lab_area.idarea            where lab_area.baja=0 
and lab_area.idArea<>13 
and lab_item.idEfector=lab_item.idEfectorDerivacion and lab_area.idTipoServicio=" + idServicio.ToString() +" order by lab_area.nombre ";
//            and lab_area.idArea<>25


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }


        //protected void dataLisUrgencias_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    CheckBoxList oList = (CheckBoxList)e.Item.FindControl("chkAnalisis");

        //    if (oList != null)
        //    {              
        //        //oList.DataValueField = "idItem";
        //        //oList.DataTextField = "nombre";
        //        //oList.DataBind();
        //        oList.SelectedIndexChanged += new EventHandler(chkAnalisis_SelectedIndexChanged);
        //    }
        //}


        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            CheckBoxList oList = (CheckBoxList)e.Item.FindControl("chkAnalisis");

            if (oList != null)
            {
                Label oLabelArea = (Label)e.Item.FindControl("lblArea");
              
                oLabelArea.Width = Unit.Percentage(100);

                oLabelArea.Text = "&nbsp;&nbsp;" + oLabelArea.Text.ToUpper();
                
                Label oLabel = (Label)e.Item.FindControl("lblIdArea");
                if (rdbTipoPlanilla.SelectedValue == "1")
                {
                    oLabelArea.ForeColor = Color.White;
                    oLabelArea.BackColor = Color.IndianRed;

                    oList.DataSource = LeerAnalisis(oLabel.Text, 1);
                }
                else
                {
              //      oLabelArea.ForeColor = Color.Gray;
                //    oLabelArea.BackColor = Color.LightSteelBlue;
                    oList.DataSource = LeerAnalisis(oLabel.Text, 0);
                }
                oList.DataValueField = "idItem";
                oList.DataTextField="nombre";
                oList.DataBind();
                oList.SelectedIndexChanged += new EventHandler(chkAnalisis_SelectedIndexChanged);
            }
        }
        protected void datalistMicrobiologia_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            CheckBoxList oList = (CheckBoxList)e.Item.FindControl("chkAnalisisMicrobiologia");

            if (oList != null)
            {
                Label oLabel = (Label)e.Item.FindControl("lblIdArea");
                
                //Label oLabelArea = (Label)e.Item.FindControl("lblArea");
                //oLabelArea.Width = Unit.Percentage(100);
                //oLabelArea.ForeColor = Color.White;
                //oLabelArea.Text = oLabelArea.Text.ToUpper();
                //oLabelArea.BackColor = Color.MidnightBlue;

                oList.DataSource = LeerAnalisis(oLabel.Text,1);
                oList.DataValueField = "idItem";
                oList.DataTextField = "nombre";
                oList.DataBind();
               // oList.SelectedIndexChanged += new EventHandler(chkAnalisis_SelectedIndexChanged);
            }
        }
        private object LeerAnalisis(string p, int tipo)
        {
            string m_strSQL = " SELECT '"+p+"' + '-'+convert(varchar, idItem) as idItem, nombre " +
            " FROM        LAB_item " +
            " WHERE idEfector=idEfectorDerivacion and  (tipo = 'P') AND (baja = 0) and IDAREA=" + p + " order by nombre ";

            if (tipo==0) //rutinas
             m_strSQL = @" select '"+p+"' + '-'+convert(varchar, I.idItem) as idItem, I.nombre  from lab_detallerutina as DR"+
             " inner join lab_item as I on I.iditem= DR.iditem" +
             " where idrutina=" + p;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

     

        protected void chkAnalisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtSeleccion.Text = ""; lblIdSeleccion.Text = "";
            SeleccionarPractica();
           
        }

        private void SeleccionarPractica()
        {
            lstSeleccion.Items.Clear();
            lblIdSeleccion.Text = "";
            if (Page.Master != null)
            {
                foreach (Control control in Page.Master.Controls)
                {
                    if (control is HtmlForm)
                    {
                        foreach (Control controlform in control.Controls)
                        {
                            if (controlform is ContentPlaceHolder)
                            {
                                foreach (Control control1 in controlform.Controls)
                                {
                                    if (control1 is DataList)
                                        if (control1.ID == "DataList1")
                                        {
                                            foreach (Control control3 in control1.Controls)
                                            {

                                                if (control3 is DataListItem)
                                                    foreach (Control control4 in control3.Controls)
                                                    {
                                                        if (control4 is Anthem.CheckBoxList)
                                                        {
                                                            Anthem.CheckBoxList oList = (Anthem.CheckBoxList)control4;
                                                            if (oList != null)
                                                            {
                                                                for (int i = 0; i < oList.Items.Count; i++)
                                                                {
                                                                    if (oList.Items[i].Selected)
                                                                    {
                                                                        if (lblIdSeleccion.Text == "")
                                                                        {
                                                                            lblIdSeleccion.Text = oList.Items[i].Value;
                                                                            //txtSeleccion.Text = oList.Items[i].Text;//+ Environment.NewLine;
                                                                            ListItem Item = new ListItem();
                                                                            Item.Value = oList.Items[i].Value;
                                                                            Item.Text = oList.Items[i].Text;
                                                                            Item.Selected = true;
                                                                            lstSeleccion.Items.Add(Item);
                                                                        }
                                                                        else
                                                                        {

                                                                            if (lblIdSeleccion.Text.IndexOf(oList.Items[i].Value) == -1)
                                                                            {
                                                                                lblIdSeleccion.Text += ";" + oList.Items[i].Value;
                                                                                //txtSeleccion.Text = txtSeleccion.Text + "-" + oList.Items[i].Text;// +Environment.NewLine;
                                                                                ListItem Item = new ListItem();
                                                                                Item.Value = oList.Items[i].Value;
                                                                                Item.Text = oList.Items[i].Text;
                                                                                Item.Selected = true;
                                                                                lstSeleccion.Items.Add(Item);
                                                                            }
                                                                        }
                                                                        //txtSeleccion.Text = txtSeleccion.Text + " - " + lblSeleccionUrgencias.Text;
                                                                        oList.Items[i].Attributes.Add("style", "background-color: yellow;");

                                                                    }
                                                                    else
                                                                    {
                                                                        if (lblIdSeleccion.Text.IndexOf(oList.Items[i].Value) != -1)
                                                                        {
                                                                            //   txtSeleccion.Text = txtSeleccion.Text.Replace(oList.Items[i].Text, "");
                                                                            lblIdSeleccion.Text = lblIdSeleccion.Text.Replace(oList.Items[i].Value, "");
                                                                            ListItem Item = new ListItem();
                                                                            Item.Value = oList.Items[i].Value;
                                                                            Item.Text = oList.Items[i].Text;
                                                                            lstSeleccion.Items.Remove(Item);

                                                                        }
                                                                        oList.Items[i].Attributes.Add("style", "background-color: white;");
                                                                    }
                                                                    oList.UpdateAfterCallBack = true;

                                                                }
                                                            }
                                                        }
                                                    }
                                            }
                                        }
                                }
                            }
                        }
                    }
                }
            }
            lstSeleccion.UpdateAfterCallBack = true;
            lblIdSeleccion.UpdateAfterCallBack = true;
            //txtSeleccion.UpdateAfterCallBack = true;
        }
        protected void chkAnalisisMicrobiologia_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtSeleccionMicrobiologia.Text = ""; lblIdSeleccionMicrobiologia.Text = "";
         
            if (Page.Master != null) 
            { 
                foreach (Control control in Page.Master.Controls) 
                { 
                    if (control is HtmlForm)
                    { 
                        foreach (Control controlform in control.Controls) 
                        {
                            if (controlform is ContentPlaceHolder)
                            {
                                foreach (Control control1 in controlform.Controls)
                                {
                                    if (control1 is DataList)
                                        if (control1.ID == "datalistMicrobiologia")
                                        {
                                            foreach (Control control3 in control1.Controls)
                                            {

                                                if (control3 is DataListItem)
                                                    foreach (Control control4 in control3.Controls)
                                                    {
                                                        if (control4 is Anthem.CheckBoxList)
                                                        {
                                                            Anthem.CheckBoxList oList = (Anthem.CheckBoxList)control4;
                                                            if (oList != null)
                                                            {

                                                                for (int i = 0; i < oList.Items.Count; i++)
                                                                {
                                                                    if (oList.Items[i].Selected)
                                                                    {
                                                                        if (txtSeleccionMicrobiologia.Text == "")
                                                                        {
                                                                            lblIdSeleccionMicrobiologia.Text = oList.Items[i].Value;
                                                                            txtSeleccionMicrobiologia.Text = oList.Items[i].Text;//+ Environment.NewLine;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (lblIdSeleccionMicrobiologia.Text.IndexOf(oList.Items[i].Value) == -1)
                                                                            {
                                                                                lblIdSeleccionMicrobiologia.Text += ";" + oList.Items[i].Value;
                                                                                txtSeleccionMicrobiologia.Text = txtSeleccionMicrobiologia.Text + "-" + oList.Items[i].Text;// +Environment.NewLine;
                                                                            }
                                                                        }

                                                                        oList.Items[i].Attributes.Add("style", "background-color: yellow;");
                                                                        //oList.UpdateAfterCallBack = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (lblIdSeleccionMicrobiologia.Text.IndexOf(oList.Items[i].Value) != -1)
                                                                        {
                                                                            txtSeleccionMicrobiologia.Text= txtSeleccionMicrobiologia.Text.Replace(oList.Items[i].Text,"");
                                                                            lblIdSeleccionMicrobiologia.Text = lblIdSeleccionMicrobiologia.Text.Replace(oList.Items[i].Value, "");
                                                                        }
                                                                        oList.Items[i].Attributes.Add("style", "background-color: white;");
                                                                    }
                                                                    oList.UpdateAfterCallBack = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                            }
                                        }
                                }
                            }
                        }
                    }
                }
            }            
            lblIdSeleccionMicrobiologia.UpdateAfterCallBack = true;
            txtSeleccionMicrobiologia.UpdateAfterCallBack = true;
        }

    
        protected void imgBorrarSeleccionMicrobiologia_Click(object sender, EventArgs e)
        {
            txtSeleccionMicrobiologia.Text = "";
            lblIdSeleccionMicrobiologia.Text = "";


            if (Page.Master != null)
            {
                foreach (Control control in Page.Master.Controls)
                {
                    if (control is HtmlForm)
                    {
                        foreach (Control controlform in control.Controls)
                        {
                            if (controlform is ContentPlaceHolder)
                            {
                                foreach (Control control1 in controlform.Controls)
                                {
                                    if (control1 is DataList)
                                        if (control1.ID == "datalistMicrobiologia")
                                        {
                                            foreach (Control control3 in control1.Controls)
                                            {

                                                if (control3 is DataListItem)
                                                    foreach (Control control4 in control3.Controls)
                                                    {
                                                        if (control4 is Anthem.CheckBoxList)
                                                        {
                                                            Anthem.CheckBoxList oList = (Anthem.CheckBoxList)control4;
                                                            if (oList != null)
                                                            {

                                                                for (int i = 0; i < oList.Items.Count; i++)
                                                                {
                                                                    
                                                                    if (oList.Items[i].Selected)
                                                                    {
                                                                        oList.Items[i].Selected = false;                                                                  
                                                                        oList.Items[i].Attributes.Add("style", "background-color: white;");
                                                                        oList.UpdateAfterCallBack = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                            }
                                        }
                                }
                            }
                        }
                    }
                }
            }
            lblIdSeleccionMicrobiologia.UpdateAfterCallBack = true;
            txtSeleccionMicrobiologia.UpdateAfterCallBack = true;
            //poner selected=false todos los chkanalisis del datalist
        }


        
        protected void imgBorrarSeleccionLaboratorio_Click(object sender, EventArgs e)
        {
   

            for (int i = 0; i < lstSeleccion.Items.Count; i++)
            {
                if (lstSeleccion.Items[i].Selected)
                {                 
                    lblIdSeleccion.Text = lblIdSeleccion.Text.Replace(lstSeleccion.Items[i].Value, "");                                             

                    if (Page.Master != null)
                    {
                        foreach (Control control in Page.Master.Controls)
                        {
                            if (control is HtmlForm)
                            {
                                foreach (Control controlform in control.Controls)
                                {
                                    if (controlform is ContentPlaceHolder)
                                    {
                                        foreach (Control control1 in controlform.Controls)
                                        {
                                            if (control1 is DataList)
                                                if (control1.ID == "DataList1")
                                                {
                                                    foreach (Control control3 in control1.Controls)
                                                    {
                                                        if (control3 is DataListItem)
                                                            foreach (Control control4 in control3.Controls)
                                                            {
                                                                if (control4 is Anthem.CheckBoxList)
                                                                {
                                                                    Anthem.CheckBoxList oList = (Anthem.CheckBoxList)control4;
                                                                    if (oList != null)
                                                                    {
                                                                        for (int j = 0; j < oList.Items.Count; j++)
                                                                        {
                                                                            if (oList.Items[j].Value==lstSeleccion.Items[i].Value)
                                                                           {
                                                                                oList.Items[j].Selected = false;
                                                                                oList.Items[j].Attributes.Add("style", "background-color: white;");
                                                                                oList.UpdateAfterCallBack = true;
                                                                           }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                    }
                                                }
                                        }
                                    }
                                }
                            }
                        }
                    } // fin de if master.
         //       lstSeleccion.Items.Remove(lstSeleccion.Items[i]);
                }//if seleccionado                
            }///for
            
            lblIdSeleccion.UpdateAfterCallBack = true;
            lstSeleccion.UpdateAfterCallBack = true;
            SeleccionarPractica();

            //poner selected=false todos los chkanalisis del datalist
        }

        
        protected void btnCargarPlanilla_Click(object sender, EventArgs e)
        {
            if (rdbTipoPlanilla.SelectedValue == "1") CargarAreasLaboratorio();
            else CargarRutinasLaboratorio();
           
        }

        
      

        private void Guardar(Peticion oRegistro, int servicio)
        {
             if (IsTokenValid())
            {
                TEST++;
                Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
                //Actualiza los datos de los objetos : alta o modificacion .       
                Usuario oUser = new Usuario();
                TipoServicio oServicio = new TipoServicio();
                Paciente oPaciente = new Paciente();
                Origen oOrigen = new Origen();
                SectorServicio oSector = new SectorServicio();

                bool urgencia = false;
                oRegistro.IdEfector = oC.IdEfector;
                if (servicio == 2) /// urgencias
                {
                    urgencia = true;
                    oRegistro.IdTipoServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), 1);
                }
                else
                    oRegistro.IdTipoServicio = (TipoServicio)oServicio.Get(typeof(TipoServicio), servicio);

                oRegistro.Fecha = DateTime.Now;
                oRegistro.Hora = DateTime.Now.ToShortTimeString();

                ///Desde aca guarda los datos del paciente en Turno
                oRegistro.IdPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));
                oRegistro.IdOrigen = (Origen)oOrigen.Get(typeof(Origen), int.Parse(ddlOrigen.SelectedValue));
                oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.IdMuestra =int.Parse( ddlMuestra.SelectedValue);
                oRegistro.Observaciones = txtObservaciones.Text;


                oRegistro.IdSector = (SectorServicio)oSector.Get(typeof(SectorServicio), int.Parse(ddlSectorServicio.SelectedValue));
                oRegistro.IdSolicitante =int.Parse(ddlEspecialista.SelectedValue); // lblSolicitante.Text; /// analizar la posibilidad de vincular al idUsuario
                oRegistro.Sala = txtSala.Text;
                oRegistro.Cama = txtCama.Text;

                oRegistro.Save();            

                GuardarDetalle(oRegistro, urgencia);

            
            }
             else
             { //doble submit
             }
        }

        private void GuardarDetalle(Peticion oRegistro, bool urgencia)
        {
            //  Bind();
            //dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);

            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(PeticionItem));
            crit.Add(Expression.Eq("IdPeticion", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (PeticionItem oDetalle in detalle)
                {
                    oDetalle.Delete();
                }
            }


            string[] tabla;
            //if (urgencia)
            //    tabla = lblSeleccionUrgencias.Text.Split(';');
            //else
            //{
                if (oRegistro.IdTipoServicio.IdTipoServicio == 1)
                    tabla = lblIdSeleccion.Text.Split(';');
                else
                    tabla = lblIdSeleccionMicrobiologia.Text.Split(';');
            //}
            ///////Crea nuevamente los detalles.
            for (int i = 0; i < tabla.Length; i++)
            {                
                string codigo1 = tabla[i].ToString();
                if (codigo1.Length > 1)
                {
                    string[] codigo2 = codigo1.Split('-');

                    string codigo = codigo2[1].ToString();
                    //else
                    if (codigo != "")
                    {
                        Item oItem = new Item();
                        oItem = (Item)oItem.Get(typeof(Item), int.Parse(codigo));

                        //ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria critRec = m_session.CreateCriteria(typeof(ItemRecomendacion));
                        critRec.Add(Expression.Eq("IdItem", oItem));
                        IList listaRecomendacion = critRec.List();
                        if (listaRecomendacion.Count > 0)
                        {
                            foreach (ItemRecomendacion oRec in listaRecomendacion)
                            {
                                PeticionItem oDetalle = new PeticionItem();
                                oDetalle.IdPeticion = oRegistro;
                                oDetalle.IdEfector = oRegistro.IdEfector;
                                oDetalle.IdItem = oItem;
                                ///buscar recomdaciones asociadas al item                    
                                oDetalle.Recomendacion = oRec.IdRecomendacion.Descripcion;
                                oDetalle.Save();
                            }
                        }
                        else /// si no hay ninguna recomendacion se guarda vacia
                        {
                            PeticionItem oDetalle = new PeticionItem();
                            oDetalle.IdPeticion = oRegistro;
                            oDetalle.IdEfector = oRegistro.IdEfector;
                            oDetalle.IdItem = oItem;
                            ///buscar recomdaciones asociadas al item                    
                            oDetalle.Recomendacion = "";
                            oDetalle.Save();
                        }
                    }
                }
            }
            //  Response.Redirect("TurnoList.aspx", false);
        }

   

        protected void btnGuardarEnviar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Peticion oRegistro = new Peticion();
                //if (Request["Modifica"].ToString() == "1")  oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));
                Guardar(oRegistro,1);
                oRegistro.Enviada = true;
                oRegistro.Save();
                if (Request["master"] != null)
                Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&master=1", false);
                else
                Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString(), false);

            }
        }

        private void Imprimir(Peticion oProt)
        {                    
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
            encabezado1.Value = oCon.EncabezadoLinea1;

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
            encabezado2.Value = oCon.EncabezadoLinea2;

            ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
            encabezado3.Value = oCon.EncabezadoLinea3;            

            oCr.Report.FileName = "PeticionE.rpt";          
            
            oCr.ReportDocument.SetDataSource(oProt.GetDataSetComprobante());            
            //oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
            //oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
            //oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);            
            oCr.DataBind();
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Peticion" + oProt.IdPeticion.ToString().Replace(".", "") + ".pdf");

            //MemoryStream oStream; // using System.IO
            //oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            ////Response.CacheControl = "Private";
            //Response.Buffer = true;
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment;filename=Peticion"+oProt.IdPeticion.ToString().Replace(".","")+".pdf");
            //Response.BinaryWrite(oStream.ToArray());
            //Response.End();

        }

        //private void Imprimir(Peticion oProt)
        //{

        //    ///////////////
        //    Business.Reporte ticket = new Business.Reporte();

        //    string textoAdicional = "";
        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //    //ConfiguracionCodigoBarra oConBarra = new ConfiguracionCodigoBarra(); oConBarra = (ConfiguracionCodigoBarra)oConBarra.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oProt.IdTipoServicio);

        //    //string sFuenteBarCode = oConBarra.Fuente;
        //    string reg_numero = oProt.IdPeticion.ToString().Replace(".","");

       
        //    DataTable dt = oProt.GetDataSetComprobante();
        //    string analisis = dt.Rows[0][7].ToString();
        //    string s_hiv = dt.Rows[0][13].ToString();
        //    string paciente = "";
        //    if (s_hiv != "False") paciente = oProt.IdPaciente.getSexo().Substring(0,1)
        //        + oProt.IdPaciente.Nombre.Substring(0, 2) + oProt.IdPaciente.Apellido.Substring(0, 2) + oProt.IdPaciente.FechaNacimiento.ToShortDateString().Replace("/", "");
        //    else paciente = oProt.IdPaciente.Apellido.ToUpper() + " " + oProt.IdPaciente.Nombre.ToUpper();

        //    ticket.AddHeaderLine("LABORATORIO " + oCon.EncabezadoLinea1);
        //    ticket.AddSubHeaderLine("_____________________________________________________________________________________________");
        //    ticket.AddSubHeaderLine("PETICION Nro. " + reg_numero + "         Fecha: " + oProt.Fecha.ToShortDateString() ; //+ "              Fecha de Entrega: " + oProt.FechaRetiro.ToShortDateString());
        //    ticket.AddSubHeaderLine(" ");
        //    ticket.AddSubHeaderLine(paciente.ToUpper() + "                       DU:" + oProt.IdPaciente.NumeroDocumento.ToString() + "      Fecha de Nacimiento:" + oProt.IdPaciente.FechaNacimiento.ToShortDateString() + "      SEXO:" + oProt.IdPaciente.getSexo());



        //    ticket.AddSubHeaderLine("_____________________________________________________________________________________________");

        //    int largo = analisis.Length;
        //    int cantidadFilas = largo / 90;
        //    if (cantidadFilas >= 0)
        //    {
        //        ticket.AddSubHeaderLine("PRACTICAS SOLICITADAS");
        //        for (int i = 1; i <= cantidadFilas; i++)
        //        {
        //            int l = i * 90;
        //            analisis = analisis.Insert(l, "&");

        //        }
        //        string[] tabla = analisis.Split('&');

        //        /////Crea nuevamente los detalles.
        //        for (int i = 0; i <= tabla.Length - 1; i++)
        //        {
        //            ticket.AddSubHeaderLine("     " + tabla[i].ToUpper());
        //        }

        //    }
        //    ticket.AddSubHeaderLine("_____________________________________________________________________________________________");

        //    //ticket.AddCodigoBarras(reg_numero, sFuenteBarCode);
        //    //ticket.AddFooterLine(reg_numero);

        //    //ticket.AddFooterLine("******************************" + textoAdicional);



        //    //Session["Impresora"] = ddlImpresora.SelectedValue;

        //    //ticket.PrintTicket(ddlImpresora.SelectedValue, oConBarra.Fuente);
        //    /////fin de impresion de archivos
        //}

        //protected void btnGuardarUrgencias_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        Peticion oRegistro = new Peticion();
        //      //  if (Request["Modifica"].ToString() == "1") oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));
        //        Guardar(oRegistro, 2);
        //        oRegistro.Enviada = true;
        //        oRegistro.Save();
        //        CargarGrilla();
        //    }
        //}
        
        protected void btnGuardarMicrobiologia_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //if (VerificarPedidosAnterioresMicrobiologia(int.Parse(Request["idPaciente"].ToString()))=="")
                //{
                    Peticion oRegistro = new Peticion();
                    //if (Request["Modifica"].ToString() == "1") oRegistro = (Peticion)oRegistro.Get(typeof(Peticion), int.Parse(Request["idPeticion"].ToString()));
                    Guardar(oRegistro, 3);
                    oRegistro.Enviada = true;
                    oRegistro.Save();
                    //CargarGrilla();
                    if (Request["master"] != null)
                        Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString() + "&master=1", false);
                    else
                        Response.Redirect("PeticionEdit.aspx?idPaciente=" + oRegistro.IdPaciente.IdPaciente.ToString(), false);
                //}

            }
        }

        private string VerificarPedidosAnterioresMicrobiologia(int p)
        {
            string mensaje="";
            Paciente oPaciente = new Paciente();
            oPaciente=  (Paciente)oPaciente.Get(typeof(Paciente), p);
            string[] tabla;        
            
            tabla = lblIdSeleccionMicrobiologia.Text.Split(';');

            for (int i = 0; i < tabla.Length; i++)
            {
                string codigo1 = tabla[i].ToString();
                if (codigo1.Length > 1)
                {
                    string[] codigo2 = codigo1.Split('-');

                    string codigo = codigo2[1].ToString();
                    //else
                    if (codigo != "")
                    {
                        Item oItem = new Item();
                        oItem = (Item)oItem.Get(typeof(Item), int.Parse(codigo));
                                
                        mensaje=  BuscarResultadoAnterior(oItem, oPaciente);
                        /////
                        if (mensaje != "")
                            mensaje = "Ya se ha realizado un pedido para " + oItem.Nombre + ":" + mensaje;
                    
                    }
                }
            }
            return mensaje;
        }

        public string BuscarResultadoAnterior(Item itemprincipal,  Paciente oPaciente )
        {            
            string s_resultadoAnterior = "";
            ISession m_session = NHibernateHttpModule.CurrentSession;           
            ICriteria critProtocolo = m_session.CreateCriteria(typeof(Protocolo));

            string ssql_Protocolo = " IdProtocolo in (Select top 10 LAb_Protocolo.IdProtocolo From LAb_Protocolo where LAb_Protocolo.idPaciente=" + oPaciente.IdPaciente.ToString() + " and LAb_Protocolo.fecha>=getdate()-30 order by LAb_Protocolo.IdProtocolo desc )";
            critProtocolo.Add(Expression.Sql(ssql_Protocolo));            

            IList detalle = critProtocolo.List();

            if (detalle.Count > 0)
            {
                foreach (Protocolo oDetalle in detalle)                
                {
                    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                    crit.Add(Expression.Eq("IdItem", itemprincipal));
                    //crit.Add(Expression.Eq("IdSubItem", subitem));
                    crit.Add(Expression.Eq("IdProtocolo", oDetalle));
                    IList oUltimoResultado = crit.List();
                    //DetalleProtocolo oUltimoResultado = (DetalleProtocolo)crit.UniqueResult();

                    //if (oUltimoResultado != null)
                    if (oUltimoResultado.Count>0)
                    {

                        s_resultadoAnterior ="Protocolo Nro. " + oDetalle.GetNumero() + " - "  + oDetalle.Fecha.ToShortDateString();
                            break;
                    
                    }
                }
            }
            return s_resultadoAnterior;

        }


       

        protected void ddlRutina_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarRutinasLaboratorio();
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string mensaje=VerificarPedidosAnterioresMicrobiologia(int.Parse(Request["idPaciente"].ToString()));
            if (mensaje == "")
            {
                args.IsValid = true; chkIgnorarValidacionMicrobiologia.Visible = false;
            }
            else
            {
                args.IsValid = false; chkIgnorarValidacionMicrobiologia.Visible = true;
                CustomValidator1.ErrorMessage = mensaje;
                return;
            }

        }

        protected void chkIgnorarValidacionMicrobiologia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIgnorarValidacionMicrobiologia.Checked)
                CustomValidator1.Enabled = false;
            else
                CustomValidator1.Enabled = true;

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}