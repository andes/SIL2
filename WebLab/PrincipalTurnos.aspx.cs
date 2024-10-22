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
using Business.Data.Laboratorio;
using Business;
using System.Data.SqlClient;
using Business.Data;
using System.IO;
using System.Drawing;

namespace WebLab
{
    public partial class PrincipalTurnos : System.Web.UI.Page
    {
      
   
        protected void Page_Load(object sender, EventArgs e)
        {
           
             if (!Page.IsPostBack)
            {  if (Session["s_permiso"] == null)Response.Redirect("FinSesion.aspx");
   Configuracion oCon = new Configuracion();
      Usuario oUser = new Usuario();
                  Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
               
              
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (oUser!=null)
                    if (oUser.IdEfector.IdEfector != 227)                    
                        oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
                else oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
                //if (oCon.NroProtocolo == -1)
                //{
                //    int i = calcularComodinDifProtocolo();
                //    Actualizarcomodin(i);
                //}

          

                //    else
                //    {
                //        if (VerificaPermisos("Pacientes con turno") == 0)
                //            pnlTurnoRecepcion.Visible = false;
                //        else
                //            pnlTurnoRecepcion.Visible = oCon.PrincipalRecepcion;

                //        if (VerificaPermisos("Pacientes sin turno") == 0) 
                //            pnlProtocolo.Visible = false;
                //        else
                //            pnlProtocolo.Visible = oCon.PrincipalRecepcion;


                //    }



                //Si no tiene permisos directamente no se muestra
                //Si tiene permisos y si está habilitado el acceso directo se muestra
                //if (VerificaPermisos("Hoja de Trabajo") == 0)
                //    pnlHojaTrabajo.Visible = false;
                //else
                //    pnlHojaTrabajo.Visible = oCon.PrincipalImpresionHT;


                //if (VerificaPermisos("Carga") == 0) pnlCargaResultado.Visible = false;
                //else pnlCargaResultado.Visible = oCon.PrincipalCargaResultados;

                //if (VerificaPermisos("Validacion") == 0)   pnlValidacion.Visible = false;
                //else pnlValidacion.Visible = oCon.PrincipalValidacion;

                //if (VerificaPermisos("Impresión") == 0)                    pnlImpresion.Visible = false;
                //else pnlImpresion.Visible = oCon.PrincipalImpresionResultados;


                // ///Para que el usuario acceda al modulo urgencias deberá tener permisos a la carga de protocolo,
                // ///carga de resultados y validacion.
                // ///

                //if ((VerificaPermisos("Pacientes sin turno") == 2)&& (VerificaPermisos("Carga") == 2)&&(VerificaPermisos("Validacion") == 2)) ///Urgencias
                //    pnlUrgencia.Visible =  oCon.PrincipalUrgencias;
                //else 
                //pnlUrgencia.Visible = false;



                // ///Agregar tambien el acceso a evolucion por analisis e historial (solapa antecedentes de resultados)
                //if (VerificaPermisos("Historial de Resultados") == 0) pnlResultados.Visible = false;
                //else pnlResultados.Visible = oCon.PrincipalResultados;

                //if (VerificaPermisos("Exportacion para SIVILA") == 0) pnlSivila.Visible = false;
                //else pnlSivila.Visible = true;
 
                
            }

        }

    

        private string GetAlerta(Configuracion oCon)
        {
            string s_alerta = "";
            if (oCon.NotificarSISA == false)   //si la notificacion no es automatica desde resultado
            {
                DataSet Ds = new DataSet();
                //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;

                string str_condicion = " and C.idEfector=" + oCon.IdEfector.IdEfector.ToString();
               
                    
                    cmd.CommandText = "[LAB_ResultadosASisa]";

                    cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
                    cmd.Parameters["@FiltroBusqueda"].Value = str_condicion;

                    cmd.Parameters.Add("@idItem", SqlDbType.Int);
                    cmd.Parameters["@idItem"].Value = 0; //todos

                    cmd.Parameters.Add("@Estado", SqlDbType.Int);
                    cmd.Parameters["@Estado"].Value = 0;//pendiente



                    cmd.Connection = conn;


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(Ds);

                     
                    

                    if  (Ds.Tables[0].Rows.Count>0)
                s_alerta = "Hay " + Ds.Tables[0].Rows.Count.ToString() + " eventos pendientes de notificar a SISA. Notifique desde el menu Informes - Resultados a SISA";

            }
            return s_alerta;

    }

    private void Actualizarcomodin(int dif)
        { 

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter da = new SqlDataAdapter();
            string m_ssql = @" update LAB_Configuracion set nroProtocolo=" + dif.ToString();
            SqlCommand cmdUpdate = new SqlCommand(m_ssql, conn);
            da.InsertCommand = cmdUpdate;
            da.InsertCommand.ExecuteNonQuery();
        }

        private int calcularComodinDifProtocolo()
        {
            int dif = 0;
            string m_strSQL = "select top 1 idprotocolo - numero as dif from lab_protocolo order by idprotocolo desc";
            DataSet Ds = new DataSet();

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            if (Ds.Tables[0].Rows.Count > 0)
                dif =int.Parse( Ds.Tables[0].Rows[0][0].ToString());
            else

                dif = 0;
            return dif;
        }

        //protected void UpdateTimer_Tick(object sender, EventArgs e)
        //{
        //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
        //    if (oCon.PeticionElectronica)
        //    {
        //        lblActualizacion.Text = "Datos actualizados al: " + DateTime.Now.ToLongTimeString();
        //        CargarPeticiones();
        //    }

        //}


        //private void CargarPeticiones()
        //{
        //    DataList2.DataSource = LeerPeticiones();
        //    DataList2.DataBind();
        //}

        //private object LeerPeticiones()
        //{
        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.CommandText = "[LAB_GetPeticionesPendientes]";


        //    cmd.Connection = conn;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(Ds);


        //    return Ds.Tables[0];


        //}

        private object LeerProtocolosxEfector()
        {
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_ProtocolosxEfector]";


            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);


            return Ds.Tables[0];


        }
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            HyperLink oHplInfo = (HyperLink)e.Item.FindControl("hplMensajeEdit");
            if (oHplInfo != null)
            {
                string s_idMensaje = oHplInfo.NavigateUrl;
                oHplInfo.NavigateUrl = "MensajeEdit.aspx?idMensaje=" + s_idMensaje + "&Operacion=Eliminar";

                MensajeInterno oMensaje = new MensajeInterno();
                oMensaje = (MensajeInterno)oMensaje.Get(typeof(MensajeInterno), int.Parse(s_idMensaje));
                if (oMensaje.IdUsuarioRegistro.ToString() == Session["idUsuario"].ToString())
                    oHplInfo.Visible = true;
                else
                    oHplInfo.Visible = false;
            }
        }


        //protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    HyperLink oHplInfo = (HyperLink)e.Item.FindControl("hplPeticionEdit");
        //    if (oHplInfo != null)
        //    {
        //        string s_idPeticion = oHplInfo.NavigateUrl;
        //        Peticion oMensaje = new Peticion();
        //        oMensaje = (Peticion)oMensaje.Get(typeof(Peticion), int.Parse(s_idPeticion));
               
        //        oHplInfo.NavigateUrl = "Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oMensaje.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oMensaje.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oMensaje.IdPeticion.ToString();

              
        //    }
        //}
        //private void MostrarMensajes()
        //{
        //    BorrarMensajes();
        //    mensajeria.Visible = true;
        //  //  CargarGrilla();
        //}

        private void BorrarMensajes()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter da = new SqlDataAdapter();
            string m_ssql = @" Delete from  LAB_Mensaje where fechaHoraRegistro <'" + DateTime.Now.AddDays(-5).ToString("yyyyMMdd") + "'"; 
            SqlCommand cmdUpdate = new SqlCommand(m_ssql, conn);
            da.InsertCommand = cmdUpdate;
            da.InsertCommand.ExecuteNonQuery();
                                                          
        }
           private void CargarGrilla()
        {

 

        }

        private object LeerDatos()
        {
            string m_strSQL = " SELECT     TOP (10) idMensaje, fechaHoraRegistro, mensaje, destinatario, remitente " +
            " FROM         dbo.LAB_Mensaje ORDER BY IDMENSAJE DESC";
            DataSet Ds = new DataSet();
                
                 SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);
                

                return Ds.Tables[0];     
            
           
        }

        private int VerificaPermisos(string sObjeto)
        {
            int i_permiso = 0;
            
                Utility oUtil = new Utility();
                 i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                return i_permiso;    
          
        }

        protected void lnkUltimoNumeroSector_Click(object sender, EventArgs e)
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            Response.Redirect("ProximoNumeroList.aspx?tipo="+ oCon.TipoNumeracionProtocolo.ToString(), false);
        }

        protected void imgAgregarMensaje_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("MensajeEdit.aspx?Operacion=Alta", false);
           
        }

        protected void btnSeguimiento_Click(object sender, EventArgs e)
        {
            Response.Redirect("Seguimiento.aspx", false);
        }
    }
}
