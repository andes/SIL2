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

namespace WebLab.Protocolos {
    public partial class DerivacionMultiEfectorLote : System.Web.UI.Page {

        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        #region Carga
        protected void Page_PreInit(object sender, EventArgs e) {

            //MultiEfector: Filtra para configuracion del efector del usuario 
            if (Session["idUsuario"] != null) {
                oUser = (Usuario) oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oC = (Configuracion) oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            } else
                Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e) {
            
            if (!Page.IsPostBack) {
               
                VerificaPermisos("Derivacion");
                ProtocoloList1.CargarGrillaProtocolo(Request["idServicio"].ToString());

                if (Session["idLote"] != null) {
                    txtNumeroLote.Text =  Convert.ToString(Session["idLote"]);
                    btnBuscar_Click(null, null);
                }
                txtNumeroLote.Focus();
               
            }

            //if (Request.Form["__EVENTTARGET"] == "btnMiBoton") {
            //    resetearForm();
            //}
            //else {
            //    resetearForm();
            //}


        }
        private void VerificaPermisos(string sObjeto) {
            if (Session["idUsuario"] != null) {
                if (Session["s_permiso"] != null) {
                    Utility oUtil = new Utility();
                    int i_permiso = oUtil.VerificaPermisos((ArrayList) Session["s_permiso"], sObjeto);
                    switch (i_permiso) {
                        case 0:
                            Response.Redirect("../AccesoDenegado.aspx", false);
                            break;
                        case 1:
                            Response.Redirect("../AccesoDenegado.aspx", false);
                            break;
                    }
                } else
                    Response.Redirect("../FinSesion.aspx", false);
            } else
                Response.Redirect("../FinSesion.aspx", false);
        }
        private void resetearForm() {
            gvProtocolosDerivados.DataSource = null;
            gvProtocolosDerivados.EmptyDataText = "";
            gvProtocolosDerivados.DataBind();
            div_controlLote.Attributes["class"] = "form-group";
            lbl_errorEfectorOrigen.Visible = false;
            lbl_cantidadRegistros.Text = "";
            lbl_estadoLote.Text = "";
            btn_recibirLote.Enabled = false;
            lbl_efectorOrigen.Text = "";
        }
        protected bool NoIngresado(int estado) {
            bool tiene = true;

            switch (estado) {
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
        protected void btnBuscar_Click(object sender, EventArgs e) {
            resetearForm();
            LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion) lote.Get(typeof(LoteDerivacion), Convert.ToInt32(txtNumeroLote.Text));

            if (efectorCorrecto(lote)) { //El efector destino es el efector logueado

                CargarControladores(lote);

                //Guardo en sesion el número de Lote para que al volver de cargar el protocolo se recarguen los datos
                if (Session["idLote"] != null) {
                    Session["idLote"] = Convert.ToInt32(txtNumeroLote.Text);
                } else {
                    Session.Add("idLote", Convert.ToInt32(txtNumeroLote.Text));
                }

            }
        }
        private void CargarControladores(LoteDerivacion lote) {
            //Si el lote es Derivado se habilita el botón para recibirlo
            if (lote.Estado == 2) {
                btn_recibirLote.Enabled = true;
            }
            
            //Cargo el estado 
            lbl_estadoLote.Text = lote.descripcionEstadoLote();

            //Cargo el efector de Origen
            Efector efectorOrigen = new Efector();
            efectorOrigen = (Efector) efectorOrigen.Get(typeof(Efector), "IdEfector", lote.IdEfectorOrigen);
            lbl_efectorOrigen.Text = efectorOrigen.Nombre;

            //Cargo grilla de protocolos para ingresar
            DataTable dt = LeerDatosProtocolosDerivados();
            if (dt.Rows.Count > 0) {
                gvProtocolosDerivados.DataSource = dt;
                lbl_cantidadRegistros.Text = "Cantidad de registros encontrados " + dt.Rows.Count;
            } else {
                gvProtocolosDerivados.DataSource = null;
                gvProtocolosDerivados.Visible = true; //asi  sale el cartel de grilla vacia
            }
            if (gvProtocolosDerivados.Rows.Count == 0) {
                //Si no trajo datos verifico el estado del lote
                if (lote.Estado != 6) {
                    gvProtocolosDerivados.EmptyDataText = "No se encontraron protocolos para el lote ingresado ";
                } else {
                    //Si esta el lote esta completo muestro otro mensaje de la grilla
                    gvProtocolosDerivados.EmptyDataText = "Ya se ingresaron todos los protocolos del lote ";
                }
            }
            gvProtocolosDerivados.DataBind();
        }
       
        private bool efectorCorrecto(LoteDerivacion lote) {
            try {
                //Verifico que el efector de Destino sea el que se tenga que ingresar
                if (lote.IdEfectorDestino == oUser.IdEfector.IdEfector) {
                    gvProtocolosDerivados.Visible = true;
                    return true;
                } else {
                    Efector e = new Efector();
                    e = (Efector) e.Get(typeof(Efector), lote.IdEfectorDestino);
                    lbl_errorEfectorOrigen.Visible = true;
                    lbl_errorEfectorOrigen.Text = "El lote con efector Destino '" + e.Nombre + "' no coincide con Efector del usuario '" + oC.IdEfector.Nombre + "'";
                    div_controlLote.Attributes["class"] = "form-group has-error";
                    gvProtocolosDerivados.Visible = false; //para que no salga el cartel de grilla vacia
                    return false;
                }
                

            } catch(Exception excep) {

               // if(excep.Message.Contains(""))
                return false; //Cuando da error idlote inexistente que devuelva falso
            }
        }

        

        private DataTable LeerDatosProtocolosDerivados() {

            string m_strSQL =
                @"select convert(varchar(10),P.fecha,103) as fecha, P.numero, P.idPaciente  as idPaciente, DE.descripcion as EstadoDerivacion , 
                P.idProtocolo , L.idEfectorDestino , ef.nombre , Pa.nombre + ' ' + Pa.apellido as paciente
                from LAB_Derivacion D
                inner join LAB_DetalleProtocolo as Det on Det.idDetalleProtocolo = D.idDetalleProtocolo
                inner join LAB_Protocolo as P on P.idProtocolo = det.idProtocolo
                inner join LAB_DerivacionEstado as DE on DE.idEstado = D.estado
                inner join LAB_LoteDerivacion L on L.idLoteDerivacion = D.idLote
                inner join Sys_Efector ef on ef.idEfector = l.idEfectorDestino
                inner join Sys_Paciente Pa on Pa.idPaciente = P.idPaciente
                where P.baja = 0
                and L.estado in (2, 4, 5)
                and D.estado=1
                and D.idLote = " + txtNumeroLote.Text + @" 
                group by P.fecha, P.numero, P.idPaciente, DE.descripcion,  P.idProtocolo ,
                L.idEfectorDestino , ef.nombre ,  Pa.nombre + ' ' + Pa.apellido ";


            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter {
                SelectCommand = new SqlCommand(m_strSQL, conn)
            };
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }
        private DataTable TraerItemsDerivadosProtocolo() {
            ////// ---------------------->Buscar las derivaciones que no han sido ingresadas
            //el protocolo me da los protocolos detalles
            //los protocolos detalles me dan las derivaciones
            //la derivacion debe estar enviada
            //la derivacion debe tener el mismo lote que el ingresado (no todos los analisis pueden haber sido enviados con el mismo lote)

            string m_strSQL =
                @" select  STRING_AGG(Det.idSubItem ,' | ') as pivote , count(*) as cantidad
                    from LAB_Derivacion D
                    inner join LAB_DetalleProtocolo as Det on Det.idDetalleProtocolo = D.idDetalleProtocolo
                    inner join LAB_Protocolo as P on P.idProtocolo = det.idProtocolo
                    inner join LAB_DerivacionEstado as LE on LE.idEstado = D.estado
                    inner join LAB_LoteDerivacion L on L.idLoteDerivacion = D.idLote
                    where P.baja = 0
                    and D.estado in (1) ---------------------- Buscar las derivaciones que no han sido ingresadas
                    and L.idLoteDerivacion = " + txtNumeroLote.Text;


            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter {
                SelectCommand = new SqlCommand(m_strSQL, conn)
            };
            adapter.Fill(Ds);
            return Ds.Tables[0];
        }

        protected bool HabilitarIngreso() {
            bool puedeIngresarProtocolo = false;

            LoteDerivacion lote = new LoteDerivacion();
            lote = (LoteDerivacion) lote.Get(typeof(LoteDerivacion), Convert.ToInt32(txtNumeroLote.Text));

            /* Estados del Lote
             idEstado	nombre
                    1	Creado -> NO permitir cargar protocolo
                    2	Derivado -> NO permitir cargar protocolo
                    3	Cancelado -> NO permitir cargar protocolo
                    4	Recibido ->  permitir cargar protocolo
                    5	Ingresado ->  permitir cargar protocolo
                    6	Completado ->  No hay más protocolos para cargar
             
             */
            if (lote.Estado == 5 || lote.Estado == 4) {
                    puedeIngresarProtocolo = true;
            }
            return puedeIngresarProtocolo;
        }
        #endregion

        #region NuevoLote
        protected void lnkIngresoProtocolo_Command(object sender, CommandEventArgs e) {
            int idProtocolo = Convert.ToInt32(e.CommandArgument);
            int idPaciente = Convert.ToInt32(e.CommandName);
            GenerarNuevoProtocolo(idProtocolo, idPaciente);
        }
        private void GenerarNuevoProtocolo(int idProtocoloOrigen, int idPaciente) {

            string pivot, m_numero, s_idServicio;

            Protocolo p = new Protocolo();
            p = (Protocolo) p.Get(typeof(Protocolo), idProtocoloOrigen);

            s_idServicio = p.IdTipoServicio.IdTipoServicio.ToString();
            m_numero = p.Numero.ToString();

            DataTable dt = TraerItemsDerivadosProtocolo();
            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][1]) > 0) {
                pivot = Convert.ToString(dt.Rows[0][0]);
                Response.Redirect("ProtocoloEdit2.aspx?idEfectorSolicitante=" + p.IdEfector.IdEfector +
                     "&numeroProtocolo=" + m_numero +
                     "&idServicio=" + s_idServicio +
                     "&idLote=" + Session["idLote"] +
                     "&idPaciente=" + idPaciente +
                     "&Operacion=AltaDerivacionMultiEfectorLote&analisis=" + pivot, false);
            }
        }

        #endregion

        #region RecibirLote
        protected void btn_recibirLote_Click(object sender, EventArgs e) {
            Response.Redirect("DerivacionRecibirLote.aspx?idLote=" + txtNumeroLote.Text + "&idServicio="+ Request["idServicio"], false);
        }
        #endregion

        protected void txtNumeroLote_TextChanged(object sender, EventArgs e) {
            //Si cambia el numero de lote, que vuelva a realizar la busqueda para que refresque los datos de busqueda
            btnBuscar_Click(null, null);
        }

        
    }
}