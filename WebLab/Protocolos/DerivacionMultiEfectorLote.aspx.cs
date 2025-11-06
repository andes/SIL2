using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Text;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using Business.Data;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Protocolos
{
    public partial class DerivacionMultiEfectorLote : System.Web.UI.Page
    {

        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        #region Carga
        protected void Page_PreInit(object sender, EventArgs e)
        {

            //MultiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {

                VerificaPermisos("Derivacion");
                if (Request["idServicio"].ToString() != "5")
                    ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());
                else
                    pnlTitulo.Visible = false;

                if (Request["idLote"] != null)
                {
                    txtNumeroLote.Text = Convert.ToString(Request["idLote"]);
                    btnBuscar_Click(null, null);
                }
                txtNumeroLote.Focus();

            }
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
                        case 0:
                            Response.Redirect("../AccesoDenegado.aspx", false);
                            break;
                        case 1:
                            Response.Redirect("../AccesoDenegado.aspx", false);
                            break;
                    }
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
        }
        private void resetearForm()
        {
            gvProtocolosDerivados.DataSource = null;
            gvProtocolosDerivados.EmptyDataText = "";
            gvProtocolosDerivados.DataBind();
            divControlLote.Attributes["class"] = "form-group";
            lblErrorEfectorOrigen.Visible = false;
            lblCantidadRegistros.Text = "";
            lblEstadoLote.Text = "";
            btnRecibirLote.Enabled = false;
            lblEfectorOrigen.Text = "";
        }
        protected bool NoIngresado(int estado)
        {
            bool tiene = true;

            switch (estado)
            {
                case 1:
                    tiene = true;
                    break;
                case 2:
                    tiene = false;
                    break;
                case 3:
                    tiene = true;
                    break;
            }
            return tiene;
        }
        #endregion

        #region Buscar
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                resetearForm();
                LoteDerivacion lote = new LoteDerivacion();
                lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), Convert.ToInt32(txtNumeroLote.Text));

                if (efectorCorrecto(lote))
                { //El efector destino es el efector logueado
                    CargarControladores(lote);
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "mensajeError", "alert('Número de lote inexistente.');", true);
            }

        }
        private void CargarControladores(LoteDerivacion lote)
        {
            //Si el lote es Derivado se habilita el botón para recibirlo
            if (lote.Estado == 2)
            {
                btnRecibirLote.Enabled = true;
            }

            //Cargo el estado 
            lblEstadoLote.Text = lote.descripcionEstadoLote();

            //Cargo el efector de Origen
            Efector efectorOrigen = new Efector();
            efectorOrigen = (Efector)efectorOrigen.Get(typeof(Efector), "IdEfector", lote.IdEfectorOrigen.IdEfector);
            lblEfectorOrigen.Text = efectorOrigen.Nombre;

            //Cargo grilla de protocolos para ingresar
            DataTable dt = LeerDatosProtocolosDerivados();
            if (dt.Rows.Count > 0)
            {
                gvProtocolosDerivados.DataSource = dt;
                lblCantidadRegistros.Text = "Cantidad de registros encontrados " + dt.Rows.Count;
            }
            else
            {
                gvProtocolosDerivados.DataSource = null;
                gvProtocolosDerivados.Visible = true; //asi  sale el cartel de grilla vacia "EmptyDataText"

                //Si no trajo datos verifico el estado del lote
                gvProtocolosDerivados.EmptyDataRowStyle.ForeColor = System.Drawing.Color.Red;
                switch (lote.Estado)
                {
                    case 1:
                        gvProtocolosDerivados.EmptyDataText = "No se puede recepcionar lote, todavia no se ha derivado."; break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        gvProtocolosDerivados.EmptyDataText = "No se encontraron protocolos para el lote ingresado.";

                        break;
                    case 6:  //Si esta el lote esta completo muestro otro mensaje de la grilla
                        gvProtocolosDerivados.EmptyDataText = "Ya se ingresaron todos los protocolos del lote.";
                        gvProtocolosDerivados.EmptyDataRowStyle.ForeColor = System.Drawing.Color.Black;
                        break;
                }
            }
            gvProtocolosDerivados.DataBind();
        }

        private bool efectorCorrecto(LoteDerivacion lote)
        {
            try
            {
                //Verifico que el efector de Destino sea el que se tenga que ingresar
                if (lote.IdEfectorDestino.IdEfector == oUser.IdEfector.IdEfector)
                {
                    gvProtocolosDerivados.Visible = true;
                    return true;
                }
                else
                {
                    Efector e = new Efector();
                    e = (Efector)e.Get(typeof(Efector), lote.IdEfectorDestino.IdEfector);
                    lblErrorEfectorOrigen.Visible = true;
                    lblErrorEfectorOrigen.Text = "El lote no corresponde al efector del usuario '" + oC.IdEfector.Nombre + "'";
                    divControlLote.Attributes["class"] = "form-group has-error";
                    gvProtocolosDerivados.Visible = false; //para que no salga el cartel de grilla vacia
                    return false;
                }


            }
            catch (Exception excep)
            {

                // if(excep.Message.Contains(""))
                return false; //Cuando da error idlote inexistente que devuelva falso
            }
        }



        private DataTable LeerDatosProtocolosDerivados()
        {

            string m_strSQL =
                @"select convert(varchar(10),P.fecha,103) as fecha, P.numero, P.idPaciente  as idPaciente, DE.descripcion as EstadoDerivacion , 
                P.idProtocolo , L.idEfectorDestino , ef.nombre ,
                case when P.idPaciente > 0 then
				 Pa.nombre + ' ' + Pa.apellido 
				 else M.nombre+' - '+ P.descripcionProducto end as paciente
                from LAB_Derivacion D
                    inner join LAB_DetalleProtocolo as Det with (nolock) on Det.idDetalleProtocolo = D.idDetalleProtocolo
                    inner join LAB_Protocolo as P with (nolock) on P.idProtocolo = det.idProtocolo
                    inner join LAB_DerivacionEstado as DE with (nolock) on DE.idEstado = D.estado
                    inner join LAB_LoteDerivacion L with (nolock) on L.idLoteDerivacion = D.idLote
                    inner join Sys_Efector ef with (nolock) on ef.idEfector = l.idEfectorDestino
                    inner join Sys_Paciente Pa with (nolock) on Pa.idPaciente = P.idPaciente
                    left join LAB_Muestra AS M with (nolock) ON P.idMuestra = M.idMuestra AND M.baja = 0
                where P.baja = 0
                    and L.estado in (2, 4, 5)
                    and D.estado=1
                    and D.idLote = " + txtNumeroLote.Text + @" 
                group by P.fecha, P.numero, P.idPaciente, DE.descripcion,  P.idProtocolo ,
                L.idEfectorDestino , ef.nombre ,  Pa.nombre + ' ' + Pa.apellido ,P.descripcionProducto,M.nombre
                ORDER BY numero";


            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(m_strSQL, conn)
            };
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
       

        protected bool HabilitarIngreso()
        {
            bool puedeIngresarProtocolo = false;

            LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion)lote.Get(typeof(LoteDerivacion), Convert.ToInt32(txtNumeroLote.Text));

            /* Estados del Lote
             idEstado	nombre
                    1	Creado -> NO permitir cargar protocolo
                    2	Derivado -> NO permitir cargar protocolo
                    3	Cancelado -> NO permitir cargar protocolo
                    4	Recibido ->  permitir cargar protocolo
                    5	Ingresado ->  permitir cargar protocolo
                    6	Completado ->  No hay más protocolos para cargar
             
             */
            if (lote.Estado == 5 || lote.Estado == 4)
            {
                puedeIngresarProtocolo = true;
            }
            return puedeIngresarProtocolo;
        }
        #endregion

        #region NuevoLote
        protected void lnkIngresoProtocolo_Command(object sender, CommandEventArgs e)
        {
            int idProtocolo = Convert.ToInt32(e.CommandArgument);
            int idPaciente = Convert.ToInt32(e.CommandName);
            GenerarNuevoProtocolo(idProtocolo, idPaciente);
        }
        private void GenerarNuevoProtocolo(int idProtocoloOrigen, int idPaciente)
        {

            string  s_idServicio, idLote;

            Protocolo p = new Protocolo();
            p = (Protocolo)p.Get(typeof(Protocolo), idProtocoloOrigen);

            s_idServicio = p.IdTipoServicio.IdTipoServicio.ToString();
            idLote = txtNumeroLote.Text;
           
            if(idPaciente > 0)
            {
                Response.Redirect("ProtocoloEdit2.aspx?idEfectorSolicitante=" + p.IdEfector.IdEfector +
                    "&idProtocolo=" + idProtocoloOrigen +
                    "&idServicio=" + s_idServicio +
                    "&idLote=" + idLote +
                    "&idPaciente=" + idPaciente +
                     "&Operacion=AltaDerivacionMultiEfectorLote", false);
            }
            else
            {
                //Es Muestra No Pacientes
                Response.Redirect("ProtocoloProductoEdit.aspx?idEfectorSolicitante=" + p.IdEfector.IdEfector +
                   "&idProtocolo=" + idProtocoloOrigen +
                   "&idServicio=" + s_idServicio +
                   "&idLote=" + idLote +
                   "&Desde=AltaDerivacionMultiEfectorLote" +
                   "&Operacion=AltaDerivacionMultiEfectorLote" , false);
            }
           
        }

        #endregion

        #region RecibirLote
        protected void btn_recibirLote_Click(object sender, EventArgs e)
        {
            Response.Redirect("DerivacionRecibirLote.aspx?idLote=" + txtNumeroLote.Text + "&idServicio=" + Request["idServicio"], false);
        }
        #endregion

        protected void txtNumeroLote_TextChanged(object sender, EventArgs e)
        {
            //Si cambia el numero de lote, que vuelva a realizar la busqueda para que refresque los datos de busqueda
            btnBuscar_Click(null, null);
        }


    }
}