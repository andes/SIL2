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
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.Services;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Text.RegularExpressions;
using Business.Data.GenMarcadores;
using CrystalDecisions.CrystalReports.Engine;

namespace WebLab.CasoFiliacion
{
    public partial class CasoResultado3 : System.Web.UI.Page
    {
        public Configuracion oC = new Configuracion();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {


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

            {


                VerificaPermisos("Casos Forense");

                //InicializarTablas();
                CargarListas();
                imgPdf.Visible = false;

                if (Request["logIn"] != null)
                {
                    Desde.Value = Request["Desde"].ToString();
                    id.Value = Request["id"].ToString();

                }
                else
                {
                    id.Value = Context.Items["id"].ToString();
                    Desde.Value = Context.Items["Desde"].ToString();
                }

                if (Desde.Value == "Valida")
                { ValidacionUsuario(); }

                MostrarDatos();
            }
        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();

            string m_ssql = "SELECT idMetodoForense, nombre FROM LAB_MetodoForense where baja=0  order by nombre ";
            //oUtil.CargarCombo(ddlMetodoExtraccion, m_ssql, "nombre", "nombre");
            //ddlMetodoExtraccion.Items.Insert(0, new ListItem("", "0"));

            oUtil.CargarCheckBox(chkMetodoExtraccion, m_ssql, "idMetodoForense", "nombre");

            m_ssql = "SELECT idAmplificacion, nombre FROM LAB_Amplificacion where baja=0  order by nombre ";
            //oUtil.CargarCombo(ddlMetodoExtraccion, m_ssql, "nombre", "nombre");
            //ddlMetodoExtraccion.Items.Insert(0, new ListItem("", "0"));

            oUtil.CargarCheckBox(chkAmplificacion, m_ssql, "idAmplificacion", "nombre");

            m_ssql = "SELECT idTipoMarcador, nombre FROM LAB_TipoMarcador where baja=0  order by idTipoMarcador ";
            oUtil.CargarCombo(ddlTipoMarcador, m_ssql, "idTipoMarcador", "nombre");

            oUtil.CargarCombo(ddlTipoMarcadorArchivo, m_ssql, "idTipoMarcador", "nombre");

            m_ssql = null;
            oUtil = null;
        }
        private void ValidacionUsuario()
        {
            //////////////////Se controla quien es el usuario que está por validar////////////////
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if ((oCon.AutenticaValidacion) && (Request["logIn"] == null)) Session["idUsuarioValida"] = null;
            if ((oCon.AutenticaValidacion) && (Session["idUsuarioValida"] == null))
            //    Response.Redirect("../Login.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString(), false);
            {
                //if ((Request["urgencia"] != null) && (oCon.AutenticaValidacion) && (Request["idUsuarioValida"] == null))
                string sredirect = "../Login.aspx?idServicio=6&Operacion=Valida&idCasoFiliacion=" + id.Value;
                if (Context.Items["Desde"].ToString() != null)
                    sredirect += "&desde=" + Desde.Value;

                Response.Redirect(sredirect);
            }
            else
            {

                //Session["idUsuarioValida"] = Session["idUsuario"];
                btnGuardar.Visible = false;
                btnValidar.Text = "Confirmar Validacion";
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1:
                        {
                            btnGuardar.Visible = false;
                            //btnAgregar.Visible = false;
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MostrarDatos()
        {

            if (Desde.Value == "Carga")
            {
                lnkValidaEncabezado.Text = "Guarda Encabezado";
                ValidaMuestra.Text = "Guarda Muestras";
                ValidaResultado.Text = "Guarda Resultados";
                ValidaMetodo.Text = "Guarda Metodos";
                ValidaConclusiones.Text = "Guarda Conclusiones";
                ValidaBibliografia.Text = "Guarda Bibliografia";
                lnkValidarMarcadores.Text = "Guarda Marcadores";
                btnValidar.Visible = false;
            }
            else
                btnGuardar.Visible = false;

            imgPdf.Visible = false;
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            lblTitulo.Text = oRegistro.IdCasoFiliacion.ToString();
            lblTitulo.Visible = true;
            lblNumero.Visible = true;
            txtNombre.Text = oRegistro.Nombre;
            txtSolicitante.Text = oRegistro.Solicitante;
            if (oRegistro.IdTipoCaso == 1)
            {
                lblTipoCaso.Text = "FILIACION"; pnlMarcadoresFiliacion.Visible = true; tituloMarcadores.Visible = true;
                dlMuestras.DataSource = GetDatosMuestra(oRegistro); // dtDeterminaciones;
                dlMuestras.DataBind();
                dlMuestras.Visible = true;
                dlForense.Visible = false;
                dlQuimerismo.Visible = false;
                btnDescargarMarcador.Visible = false;
                ddlTipoMarcador.Visible = false;
                //btnDescargarMarcadoresY.Visible = false;
            }
            if (oRegistro.IdTipoCaso == 2)
            {
                txtMarcoEstudio.Text = "";
                lblTipoCaso.Text = "FORENSE";
                pnlMarcadoresFiliacion.Visible = false;
                groupTotalProbabilidad.Visible = false;
                btnDescargarMarcador.Visible = true;
                ddlTipoMarcador.Visible = true;
                //btnDescargarMarcadoresY.Visible = true;
                dlForense.DataSource = GetDatosMuestra(oRegistro); // dtDeterminaciones;
                dlForense.DataBind();
                dlForense.Visible = true;
                dlMuestras.Visible = false;
                dlQuimerismo.Visible = false;
                tituloMarcadores.Visible = true;

            }

            if (oRegistro.IdTipoCaso == 3)  //quimerismo
            {
                lblTipoCaso.Text = "QUIMERISMO"; pnlMarcadoresFiliacion.Visible = true; tituloMarcadores.Visible = true;
                groupAutos.Visible = false;
                btnDescargarMarcador.Visible = false;
                ddlTipoMarcador.Visible = false;
                //btnDescargarMarcadoresY.Visible = false;
                groupAnalisisEstadistico.Visible = false;
                groupMuestras.Visible = true;
                groupFechaTransplante.Visible = true;
                dlQuimerismo.DataSource = GetDatosMuestra(oRegistro); // dtDeterminaciones;
                dlQuimerismo.DataBind();
                dlQuimerismo.Visible = true;
                dlMuestras.Visible = false;
                dlForense.Visible = false;
                groupCuantificacion.Visible = false;
                groupMarcoEstudio.Visible = false;
                groupLimiteDeteccion.Visible = true;
                groupTotalProbabilidad.Visible = false;
                tituloBibliografia.Visible = false;
                tituloBaseFA.Visible = false;
                txtObjetivo.Text = "Estudio de quimerismo post transplante de medula ósea";

            }

            if ((oRegistro.IdUsuarioValida == 0) && (oRegistro.IdUsuarioCarga > 0))
            {

                txtSolicitante.Text = oRegistro.Solicitante;
                txtAutos.Text = oRegistro.Autos;
                txtObjetivo.Text = oRegistro.Objetivo;
                txtObservacionMuestras.Text = oRegistro.Muestra;
                txtResultado.Text = oRegistro.Resultado;
                txtConclusion.Text = oRegistro.Conclusion;
                 txtMetodoExtraccion.Text = oRegistro.Metodo;
                MostrarMetodo(oRegistro);
 
                 txtAmplificacion.Text = oRegistro.Amplificacion;
                MostrarAmpli(oRegistro);
                txtAnalisis.Text = oRegistro.Analisis;
                txtSoftware.Text = oRegistro.Software;
                txtEstadistico.Text = oRegistro.Estadistico;
                txtMarcoEstudio.Text = oRegistro.Marcoestudio;
                txtBibliografia.Text = oRegistro.Bibliografia;
                txtCuantificacion.Text = oRegistro.Cuantificacion;
                txtProbabilidad.Text = oRegistro.Probabilidad;
                Usuario oUser1 = new Usuario();
                oUser1 = (Usuario)oUser1.Get(typeof(Usuario), oRegistro.IdUsuarioCarga);
                lblUsuario.Text = "Resultados cargados por " + oUser1.Apellido + " " + oUser1.Nombre + " - Fecha: " + oRegistro.FechaCarga.ToShortDateString() + " " + oRegistro.FechaCarga.ToShortTimeString();
                lblUsuario.Visible = true;
                lblUsuario.ForeColor = System.Drawing.Color.Black;
                btnBorrarTablaMarcadores.Visible = true;
                txtLimiteDeteccion.Text = oRegistro.LimiteDeteccion;
                if (oRegistro.FechaTransplante.ToShortDateString()!="01/01/1900")
                txtFechaTransplante.Value = oRegistro.FechaTransplante.ToShortDateString();

            }


            if (oRegistro.IdUsuarioValida > 0)
            {
                txtSolicitante.Text = oRegistro.Solicitante;
                txtAutos.Text = oRegistro.Autos;
                txtObjetivo.Text = oRegistro.Objetivo;
                txtObservacionMuestras.Text = oRegistro.Muestra;
                txtResultado.Text = oRegistro.Resultado;
                txtConclusion.Text = oRegistro.Conclusion;
                txtMetodoExtraccion.Text = oRegistro.Metodo;
                MostrarMetodo(oRegistro);

                txtAmplificacion.Text = oRegistro.Amplificacion;
                MostrarAmpli(oRegistro);
                txtAnalisis.Text = oRegistro.Analisis;
                txtSoftware.Text = oRegistro.Software;
                txtEstadistico.Text = oRegistro.Estadistico;
                txtMarcoEstudio.Text = oRegistro.Marcoestudio;
                txtCuantificacion.Text = oRegistro.Cuantificacion;
                txtBibliografia.Text = oRegistro.Bibliografia;
                txtProbabilidad.Text = oRegistro.Probabilidad;
                txtLimiteDeteccion.Text = oRegistro.LimiteDeteccion;
                if (oRegistro.FechaTransplante.ToShortDateString() != "01/01/1900")
                    txtFechaTransplante.Value = oRegistro.FechaTransplante.ToShortDateString();
                Usuario oUser2 = new Usuario();
                oUser2 = (Usuario)oUser2.Get(typeof(Usuario), oRegistro.IdUsuarioValida);
                lblUsuario.Text = "Terminado por " + oUser2.Apellido + " " + oUser2.Nombre + " - Fecha: " + oRegistro.FechaValida.ToShortDateString() + " " + oRegistro.FechaValida.ToShortTimeString();
                lblUsuario.Visible = true; lblUsuario.ForeColor = System.Drawing.Color.DarkRed;
                imgPdf.Visible = true;
                if (Desde.Value == "Carga")
                {
                    btnGuardar.Visible = false;
                    subir.Visible = false;
                    subir0.Visible = false;



                    //btnBorrarImg.Visible = false;
                }

                lnkValidaEncabezado.Visible = false;
                ValidaMuestra.Visible = false;
                btnMuestras.Visible = false;
                ValidaBibliografia.Visible = false;
                ValidaConclusiones.Visible = false;
                ValidaResultado.Visible = false;
                lnkValidarMarcadores.Visible = false;
                btnValidar.Visible = false;
                imgPdfPreeliminar.Visible = false;
                btnGuardar.Visible = false;
                ValidaMetodo.Visible = false; // link de resultado


            }

            CargarTablaFA();
            ///cargar foto


            Image1.ImageUrl = string.Format("../imagen.ashx?id={0}", oRegistro.IdCasoFiliacion.ToString());
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string qry = "select imgResultado from lab_CasoFiliacion where idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString();
            SqlDataAdapter ad = new SqlDataAdapter(qry, conn);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["imgResultado"] != null)
                {
                    btnBorrarImg.Visible = true;
                    Image1.Visible = true;
                }
                else
                {
                    btnBorrarImg.Visible = false;
                    Image1.Visible = false;
                }
            }

            if ((oRegistro.IdUsuarioValida > 0) && (Desde.Value == "Carga"))
                btnBorrarImg.Visible = false; // borrar imagen oculta


            CargarTablaResultados(2);


            ActualizarWorkFlow(oRegistro);

            //oRegistro.GrabarAuditoria("Consulta Caso", int.Parse(Session["idUsuario"].ToString()),"");

        }

        private void MostrarMetodo(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
          


            //bool nohaynada = false;
            Utility oUtil = new Utility();
            string[] arr = oRegistro.Metodo.Split(("-").ToCharArray());
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < chkMetodoExtraccion.Items.Count; j++)
                {
                    if (arr[i].ToString() == chkMetodoExtraccion.Items[j].Text)
                    {
                        chkMetodoExtraccion.Items[j].Selected = true;
                        //nohaynada = true;
                    }
                }
            }

            


        }
        private void MostrarAmpli(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {



          
            Utility oUtil = new Utility();
            string[] arr = oRegistro.Amplificacion.Split(("-").ToCharArray());
            for (int i = 0; i < arr.Length; i++)
            {
                
                for (int j = 0; j < chkAmplificacion.Items.Count; j++)
                {
                    if (arr[i].ToString() == chkAmplificacion.Items[j].Text)
                    {
                        chkAmplificacion.Items[j].Selected = true;
                       
                    }
                }
            }

        
             


        }

        private object GetDatosMuestra(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
            string m_strSQL = "";
            if (oRegistro.IdTipoCaso == 1)
                m_strSQL = @"select P.idProtocolo, Pac.idPaciente,'" + Desde.Value + @"' as Desde,
Pac.nombre+ ' '+ Pac.apellido as Persona, case when Pac.idEstado=2 then 'Sin DNI' else 'DNI ' + convert(varchar,Pac.numerodocumento) end   as  numerodocumento , 
P.numero as Protocolo, 
Par.nombre + ' ' + CFP.observacionParentesco as Parentesco ,
 convert(varchar(10), P.fechatomamuestra,103) as fechatoma, S.nombre as lugar,
M.nombre as muestra
from  Lab_casofiliacion as CF
inner join  lab_casofiliacionprotocolo CFP on CF.idcasofiliacion= CFP.idcasofiliacion
inner join lab_protocolo P on P.idprotocolo = CFP.idprotocolo
inner join sys_paciente Pac on Pac.idPaciente= P.idpaciente 
inner join lab_parentesco as Par on Par.idParentesco=CFP.idtipoParentesco
inner join sys_efector as S on S.idefector= P.idefectorSolicitante
inner join lab_muestra as M on M.idmuestra= P.idMuestra 
where Pac.idPaciente<>-1 and cf.idcasofiliacion=" + oRegistro.IdCasoFiliacion.ToString() + @" order by P.idProtocolo    ";

            if (oRegistro.IdTipoCaso == 2)
                m_strSQL = @"select  p.idProtocolo as idProtocolo,'" + Desde.Value + @"' as Desde ,
Pac.apellido, Pac.nombre, 
P.numeroOrigen as  numeroOrigen , P.numero as Protocolo, 
 M.nombre as muestra, P.descripcionProducto     + case when Pac.idPaciente>-1 then ' de ' +  Pac.nombre + ' ' + Pac.apellido else '' end as muestraDetalle
from  Lab_casofiliacion as CF
inner join  lab_casofiliacionprotocolo CFP on CF.idcasofiliacion= CFP.idcasofiliacion
inner join lab_protocolo P on P.idprotocolo = CFP.idprotocolo
inner join sys_paciente Pac on Pac.idPaciente= P.idpaciente 
inner join lab_muestra as M on M.idmuestra= P.idMuestra 
where cf.idcasofiliacion= " + oRegistro.IdCasoFiliacion.ToString() + @" order by P.idProtocolo    ";

            if (oRegistro.IdTipoCaso == 3) // quimerismo
                m_strSQL = @"select  p.idProtocolo as idProtocolo,'" + Desde.Value + @"' as Desde ,
Pac.apellido, Pac.nombre, Par.nombre + ' ' + CFP.observacionParentesco as Parentesco ,
P.numeroOrigen as  numeroOrigen , P.numero as Protocolo, 
 M.nombre as muestra, P.descripcionProducto    as muestraDetalle,
     Pac.nombre + ' ' + Pac.apellido as Persona,
convert(varchar(10), P.fechatomamuestra,103) as fechatoma, S.nombre as lugar
from  Lab_casofiliacion as CF
inner join  lab_casofiliacionprotocolo CFP on CF.idcasofiliacion= CFP.idcasofiliacion
inner join lab_protocolo P on P.idprotocolo = CFP.idprotocolo
inner join sys_paciente Pac on Pac.idPaciente= P.idpaciente 
inner join lab_parentesco as Par on Par.idParentesco=CFP.idtipoParentesco
inner join sys_efector as S on S.idefector= P.idefectorSolicitante
inner join lab_muestra as M on M.idmuestra= P.idMuestra 
where cf.idcasofiliacion= " + oRegistro.IdCasoFiliacion.ToString() + @" order by P.idProtocolo    ";


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        private void ActualizarWorkFlow(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {

            AuditoriaCasoFiliacion oMarca = new AuditoriaCasoFiliacion();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session.CreateCriteria(typeof(AuditoriaCasoFiliacion));
            crit2.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));
            //crit2.Add(Expression.Eq("Accion", "Valida Muestra"));

            bool muestra = false;
            bool metodo = false;
            bool conclusiones = false;
            bool resultado = false;
            bool bibliografia = false;
            bool encabezado = false;
            bool marcador = false;

            IList itemsm = crit2.List();
            foreach (AuditoriaCasoFiliacion oM in itemsm)
            {
                if (oM.Accion == "Guarda Encabezado")
                {
                    lblUsuarioEncabezado.Text = "Cargado por: " + oM.GetUsuario();
                }
                if (oM.Accion == "Valida Encabezado")
                {
                    encabezado = true;
                    lblUsuarioEncabezado.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        lnkValidaEncabezado.Visible = false;
                }

                if (oM.Accion == "Guarda Muestras")
                    lblUsuarioMuestras.Text = "Cargado por: " + oM.GetUsuario();

                if (oM.Accion == "Valida Muestras")
                {
                    muestra = true;
                    lblUsuarioMuestras.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        ValidaMuestra.Visible = false;
                }
                if (oM.Accion == "DesValida Muestras")
                {
                    muestra = false;
                    lblUsuarioMuestras.Text = "";
                    if (Desde.Value == "Carga")
                        ValidaMuestra.Visible = true;
                }
                if (oM.Accion == "Guarda Metodos")
                    lblUsuarioMetodos.Text = "Cargado por: " + oM.GetUsuario();

                if (oM.Accion == "Valida Metodos")
                {
                    metodo = true;
                    lblUsuarioMetodos.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        ValidaMetodo.Visible = false;
                }

                if (oM.Accion == "Guarda Resultados")
                {
                    lblUsuarioResultados.Text = "Cargado por: " + oM.GetUsuario();
                }
                if (oM.Accion == "Valida Resultados")
                {
                    resultado = true;
                    lblUsuarioResultados.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        ValidaResultado.Visible = false;
                }
                ///

                if (oM.Accion == "Guarda Conclusiones")
                {
                    lblUsuarioConclusiones.Text = "Cargado por: " + oM.GetUsuario();
                }
                if (oM.Accion == "Valida Conclusiones")
                {
                    conclusiones = true;

                    lblUsuarioConclusiones.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        ValidaConclusiones.Visible = false;
                }
                if (oM.Accion == "Guarda Bibliografia")
                    lblUsuarioBibliografia.Text = "Cargado por: " + oM.GetUsuario();
                if (oM.Accion == "Valida Bibliografia")
                {
                    bibliografia = true;
                    lblUsuarioBibliografia.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        ValidaBibliografia.Visible = false;
                }
                if (oM.Accion == "Guarda Marcadores")
                {

                    lblUsuarioMarcadores.Text = oM.GetUsuario();
                }
                if (oM.Accion == "Valida Marcadores")
                {
                    marcador = true;
                    lblUsuarioMarcadores.Text = "Validado por: " + oM.GetUsuario();
                    if (Desde.Value == "Carga")
                        lnkValidarMarcadores.Visible = false;


                }
            }
            //imgPdf.Visible = false;
            if ((Desde.Value == "Valida") || (Desde.Value == "Revalida"))
            {
                switch (oRegistro.IdTipoCaso)
                {
                    case 3: //quimerismo
                        if ((muestra) && (metodo) && (resultado) && (conclusiones) && (encabezado) && (marcador))
                        {
                            btnValidar.Visible = true;

                        }
                        break;
                    case 1: // filiacion

                        if ((muestra) && (metodo) && (resultado) && (conclusiones) && (bibliografia) && (encabezado) && (marcador))
                        {
                            btnValidar.Visible = true;

                        }
                        break;
                    case 2: // forense

                        if ((muestra) && (metodo) && (resultado) && (conclusiones) && (bibliografia) && (encabezado) && (marcador))
                        {
                            btnValidar.Visible = true;

                        }
                        break;
                }
            }




            icomuestra.Visible = muestra;
            icometodo.Visible = metodo;
            icoresultado.Visible = resultado;
            icobibliografia.Visible = bibliografia;
            icoencabezado.Visible = encabezado;
            icomarcador.Visible = marcador;
            icoconclusiones.Visible = conclusiones;
            icobasefa.Visible = false;


        }

        private void CargarTablaFA()
        {

            GridView1.DataSource = LeerDatosFA();
            GridView1.DataBind();



        }

 


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

                Guardar(oRegistro, "");

                if (Request["Desde"] != null)
                {
                    if (Request["Desde"].ToString() == "Valida")
                        Response.Redirect("FiliacionMenu.aspx");
                }
                else
                {
                    HttpContext Context;

                    Context = HttpContext.Current;
                    Context.Items.Add("idServicio", "6");
                    Server.Transfer("CasoList.aspx");
                }
                //Response.Redirect("CasoList.aspx?idServicio=6", false);
            }
        }

        private void Guardar(Business.Data.Laboratorio.CasoFiliacion oRegistro, string seccion)
        {
            Utility oUtil = new Utility();

          
            //if (seccion == "Encabezado")
            //{
            oRegistro.Solicitante = txtSolicitante.Text;
            oRegistro.Autos = txtAutos.Text;

            //string s_nombre = Regex.Replace(txtNombre.Text, @"[^0-9A-Za-z]", "", RegexOptions.None);
            oRegistro.Nombre = oUtil.SacaComillas(oUtil.RemoverSignosAcentos(txtNombre.Text));
            //oRegistro.Nombre = txtNombre.Text;
            oRegistro.Objetivo = txtObjetivo.Text;

            oRegistro.Resultado = txtResultado.Text;

            oRegistro.Conclusion = txtConclusion.Text;
            //oRegistro.Metodo = txtMetodo.Text;
         //   oRegistro.Metodo = ddlMetodoExtraccion.SelectedItem.Text;
            oRegistro.Metodo = GuardaMetodo(oRegistro);

            oRegistro.Amplificacion = GuardaAmpli(oRegistro);  // txtAmplificacion.Text;
            oRegistro.Analisis = txtAnalisis.Text;
            oRegistro.Software = txtSoftware.Text;

            if (oRegistro.IdTipoCaso == 3) oRegistro.Muestra = txtObservacionMuestras.Text;

            //Para quimerismo no se guardan todos estos datos
            if (oRegistro.IdTipoCaso != 3) oRegistro.Cuantificacion = txtCuantificacion.Text;
            if (oRegistro.IdTipoCaso != 3) oRegistro.Estadistico = txtEstadistico.Text;
            if (oRegistro.IdTipoCaso != 3) oRegistro.Marcoestudio = txtMarcoEstudio.Text;
            if (oRegistro.IdTipoCaso != 3) oRegistro.Probabilidad = txtProbabilidad.Text;
            if (oRegistro.IdTipoCaso != 3) oRegistro.Bibliografia = txtBibliografia.Text;
            if (oRegistro.IdTipoCaso == 3)
            {
                oRegistro.LimiteDeteccion = txtLimiteDeteccion.Text;
                if (txtFechaTransplante.Value!="")

                oRegistro.FechaTransplante = DateTime.Parse(txtFechaTransplante.Value);
                else oRegistro.FechaTransplante = DateTime.Parse("1900-01-01");
            }


            /*{*/     /// GUARDA MARCADOR TOTAL            

            CasoMarcadores oMarca = new CasoMarcadores();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session.CreateCriteria(typeof(CasoMarcadores));
            crit2.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));
            crit2.Add(Expression.Eq("IdProtocolo", 0));
            crit2.Add(Expression.Eq("Marcador", "TOTAL"));

            IList itemsm = crit2.List();
            foreach (CasoMarcadores oM in itemsm)
            {
                oM.Ip = txTotalLR.Text;
                oM.Save();
            }
            ////
            /// 
            //}

            if (seccion == "Validado")
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));
                //s_desde = "Valida";
                oRegistro.FechaValida = DateTime.Now;
                oRegistro.IdUsuarioValida = oUser.IdUsuario;

            }
            else
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

                oRegistro.FechaCarga = DateTime.Now;
                oRegistro.IdUsuarioCarga = oUser.IdUsuario;

            }
            oRegistro.Save();
            //string s_desde = "Carga";
            //if (Desde.Value == "Valida")
            //{

            //    oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));


            //    s_desde = "Valida";
            //    oRegistro.FechaValida = DateTime.Now;
            //oRegistro.IdUsuarioValida = oUser.IdUsuario;

            //}




            /// actualizar a mano la conclusion
            // SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            // string query = @"update [dbo].[LAB_CasoFiliacion] set conclusion=" + txtConclusion.Text + " where idcasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString();          

            // SqlCommand cmd = new SqlCommand(query, conn);



            //int idcaso = Convert.ToInt32(cmd.ExecuteScalar());
            // //

            try
            {
                ///grabaar imagen
                Int32 idconsenti;
                byte[] image = null;


                if (trepadorFoto.PostedFile.ContentLength > 0)
                {
                    using (BinaryReader reader = new BinaryReader(trepadorFoto.PostedFile.InputStream))
                    {
                        image = reader.ReadBytes(trepadorFoto.PostedFile.ContentLength);
                        string directorio = Server.MapPath("") + "..\\Consentimientos\\Fotos";
                        if (!Directory.Exists(directorio)) Directory.CreateDirectory(directorio);
                        string archivo = directorio + "\\" + trepadorFoto.FileName;
                        trepadorFoto.SaveAs(archivo);
                    }
                }/*hasta aca trepador*/
                else
                {
                    if (Session["CapturedImage"] != null)
                    {
                        //string path = Server.MapPath(Session["CapturedImage1"].ToString());
                        string path = Server.MapPath(Session["CapturedImage"].ToString());
                        // 2.
                        // Get byte array of file.
                        image = File.ReadAllBytes(path);
                        // 3A.
                    }

                }
                ///// separar conclusion de imagen en el update.
                //if (seccion == "Resultado")
                //{
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                string query = @"update LAB_CasoFiliacion set   conclusion=@conclusion where  idCasoFiliacion= " + oRegistro.IdCasoFiliacion.ToString();
                SqlCommand cmd = new SqlCommand(query, conn);



                SqlParameter conclusionParam = cmd.Parameters.Add("@conclusion", System.Data.SqlDbType.VarChar);
                conclusionParam.Value = txtConclusion.Text;


                idconsenti = Convert.ToInt32(cmd.ExecuteScalar());


                SqlConnection conn1 = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

                query = @"update LAB_CasoFiliacion set imgResultado=@imagen   where  idCasoFiliacion= " + oRegistro.IdCasoFiliacion.ToString();
                SqlCommand cmd1 = new SqlCommand(query, conn1);

                SqlParameter imageParam = cmd1.Parameters.Add("@imagen", System.Data.SqlDbType.Image);
                imageParam.Value = image;




                idconsenti = Convert.ToInt32(cmd1.ExecuteScalar());

                //}
                /// fin grabar imagen
            }
            catch (Exception ex)
            {
                string exception = "";
                //while (ex != null)
                //{
                exception = ex.Message + "<br>";

                //}
            }


            //oRegistro.GrabarAuditoria(s_desde + " resultados", oUser.IdUsuario,"");






        }

        private string GuardaMetodo(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
            string metodo = "";
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionMetodo));
            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (CasoFiliacionMetodo oDetalle in detalle)
                {
                 //   oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Desvinculado de caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
                    oDetalle.Delete();

                }
            }

            /////Crea nuevamente los detalles.
            for (int i = 0; i < chkMetodoExtraccion.Items.Count; i++)
            {
                if (chkMetodoExtraccion.Items[i].Selected)
                {
                    CasoFiliacionMetodo oDetalle = new CasoFiliacionMetodo();
                  
                    oDetalle.IdCasoFiliacion = oRegistro;

                    oDetalle.IdMetodoForense = int.Parse(chkMetodoExtraccion.Items[i].Value);

                    oDetalle.Save();
                    if (metodo == "") metodo = chkMetodoExtraccion.Items[i].Text;
                    else metodo += "-"+chkMetodoExtraccion.Items[i].Text;
                }
                //oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
            }
            return metodo;

        }

        private string GuardaAmpli(Business.Data.Laboratorio.CasoFiliacion oRegistro)
        {
            string ampli = "";
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionAmplificacion));
            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (CasoFiliacionAmplificacion oDetalle in detalle)
                {
                    //   oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Desvinculado de caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
                    oDetalle.Delete();

                }
            }

            /////Crea nuevamente los detalles.
            for (int i = 0; i < chkAmplificacion.Items.Count; i++)
            {
                if (chkAmplificacion.Items[i].Selected)
                {
                    CasoFiliacionAmplificacion oDetalle = new CasoFiliacionAmplificacion();

                    oDetalle.IdCasoFiliacion = oRegistro;

                    oDetalle.IdAmplificacion = int.Parse(chkAmplificacion.Items[i].Value);

                    oDetalle.Save();
                    if (ampli == "") ampli = chkAmplificacion.Items[i].Text;
                    else ampli += "-" + chkAmplificacion.Items[i].Text;
                }
                //oDetalle.IdProtocolo.GrabarAuditoriaProtocolo("Vinculado a caso " + oDetalle.IdCasoFiliacion.IdCasoFiliacion.ToString(), int.Parse(Session["idUsuario"].ToString()));
            }
            return ampli;

        }

        protected void cvListaDeterminaciones_ServerValidate(object sender, EventArgs e)
        {

        }

        protected void gvLista_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image btnUrl = (Image)e.Row.FindControl("img");

               
                string directorio = btnUrl.ImageUrl.ToString();
                string extension = System.IO.Path.GetExtension(directorio).ToLower();
                if (extension == ".pdf")

                    btnUrl.ImageUrl = "..\\App_Themes\\default\\images\\pdfgrande.jpg";

            }
        }

        protected void ddlServicio_SelectedIndexChanged()
        {

        }


        protected void cvListaDeterminaciones_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            //if (Session["Tabla1"] != null)
            //{
            //    dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            //    if (dtDeterminaciones.Rows.Count == 0) args.IsValid = false;
            //    else args.IsValid = true;
            //}
            //else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            if (Request["Desde"] != null)
            {
                if (Request["Desde"].ToString() == "Valida")
                    Response.Redirect("FiliacionMenu.aspx");
            }
            else
            {
                HttpContext Context;

                Context = HttpContext.Current;

                Context.Items.Add("idServicio", "6");
                Server.Transfer("CasoList.aspx");
            }
        }

        protected void imgPdf_Click(object sender, EventArgs e)
        {
            Imprimir("Final");
        }

        private void Imprimir(string tipo)
        {

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));


            CrystalReportSource oCr = new CrystalReportSource();

            switch (oRegistro.IdTipoCaso)
            {
                case 1:
                    {
                        oCr.Report.FileName = "ResultadoFiliacion.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getResultado(tipo));
                        oCr.ReportDocument.Subreports[0].SetDataSource(oRegistro.getMarcadores());
                    }
                    break;
                case 2:
                    {
                        oCr.Report.FileName = "ResultadoForense.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getResultadoForense());
                        //oCr.ReportDocument.Subreports[0].SetDataSource(oRegistro.getMarcadores());
                    }
                    break;
                case 3:
                    {
                        oCr.Report.FileName = "ResultadoQuimerismo.rpt";
                        oCr.ReportDocument.SetDataSource(oRegistro.getResultado(tipo));
                        oCr.ReportDocument.Subreports[0].SetDataSource(oRegistro.getMarcadores());
                    }
                    break;

            }

            oCr.DataBind();
            //if (Desde.Value == "Carga")
            //    oRegistro.GrabarAuditoria("Imprime Resultado", oUser.IdUsuario, "Resultado_" + oRegistro.Nombre.Trim());
            //else
                oRegistro.GrabarAuditoria("Imprime Resultado", oUser.IdUsuario, "Resultado_" + oRegistro.Nombre.Trim());

          
             
             oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Resultado_" + oRegistro.Nombre.Trim());


        }

        protected void btnValidar_Click(object sender, EventArgs e)
        {
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "6");
            if (btnValidar.Text == "Confirmar Validacion")
            {
                if (Page.IsValid)
                {
                    Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                    if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(Request["id"]));

                    Guardar(oRegistro, "Validado");
                    //imgPdf.Visible = true;

                    Session.Contents.Remove("idUsuarioValida");

                    Server.Transfer("CasoList.aspx");

                    //Response.Redirect("CasoList.aspx?idServicio=6", false);
                }
            }
            else

            {
                Session.Contents.Remove("idUsuarioValida");
                Context.Items.Add("id", id.Value);
                Context.Items.Add("Desde", "Valida");
                Server.Transfer("CasoResultado3.aspx");

            }


        }

        protected void btnBorrarImg_Click(object sender, EventArgs e)
        {
            Int32 idconsenti;
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;

            string query = @"update LAB_CasoFiliacion set imgResultado=@Image  where idCasoFiliacion= " + oRegistro.IdCasoFiliacion.ToString();
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add("@Image", SqlDbType.VarBinary, -1);

            cmd.Parameters["@Image"].Value = DBNull.Value;


            idconsenti = Convert.ToInt32(cmd.ExecuteScalar());
            oRegistro.GrabarAuditoria("Borra imagen de resultados", int.Parse(Session["idUsuario"].ToString()), "");

            Image1.ImageUrl = "";
            Image1.UpdateAfterCallBack = true;
        }
        private void ProcesarFichero()
        {
            if (this.trepador.HasFile)
            {
                string filename = this.trepador.PostedFile.FileName;
                BorrarResultadosTemporales(1);
                int i = 1;
                if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                {
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            if (i != 1)

                                ProcesarLinea(line, 1);

                            i += 1;
                        }
                    }
                }
            }
        }
         
        private void ProcesarFichero2()
        {
            if (this.trepador2.HasFile)
            {
                string filename = this.trepador2.PostedFile.FileName;
                BorrarResultadosTemporales(2);
                int i = 1;
                if (filename.Substring(filename.LastIndexOf('.')).Trim().ToUpper() != ".EXE")
                {
                    string line;
                    StringBuilder log = new StringBuilder();
                    Stream stream = this.trepador2.FileContent;

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        while (!string.IsNullOrEmpty(line = sr.ReadLine()))
                        {
                            if (i != 1)

                                ProcesarLinea(line, 2);

                            i += 1;
                        }
                    }
                }
            }
        }
        private void ProcesarLinea(string linea, int tipo)
        {
            try
            {
                Business.Data.Laboratorio.CasoFiliacion oCaso = new Business.Data.Laboratorio.CasoFiliacion();
                oCaso = (Business.Data.Laboratorio.CasoFiliacion)oCaso.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

                TipoMarcador tipoMarcador = new TipoMarcador();
                tipoMarcador = (TipoMarcador)tipoMarcador.Get(typeof(TipoMarcador),  int.Parse( ddlTipoMarcadorArchivo.SelectedValue));



                Utility oUtil = new Utility();
                string[] arr = linea.Split(("\t").ToCharArray());



                if (tipo == 1)
                {
                    if (arr.Length >= 4)
                    {
                        string campo_cero = arr[0].ToString();
                        string campo_MFI = arr[1].ToString();
                        string campo_Bead = arr[2].ToString();
                        string campo_valor = arr[3].ToString();

                        string subitem = "";
                        string[] campo_cero_con_letra = campo_cero.Split((" ").ToCharArray());
                        if (campo_cero_con_letra.Length > 1)
                            if (campo_cero_con_letra[1].ToString().Length == 1)
                                subitem = campo_cero_con_letra[1].ToString();
                        //if (campo_cero_con_letra[2].ToString().Length > 49)
                        //    subitem +=" "+ campo_cero_con_letra[2].Substring(0, 48);
                        //else
                        //    subitem += " " + campo_cero_con_letra[2].ToString();

                        Protocolo oP = new Protocolo();
                        oP = (Protocolo)oP.Get(typeof(Protocolo), "Numero", campo_cero_con_letra[0].ToString());


                        //verifica que el protocolo pertenezca al caso
                        CasoFiliacionProtocolo oCasoPro = new CasoFiliacionProtocolo();
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));


                        crit.Add(Expression.Eq("IdCasoFiliacion", oCaso));
                        crit.Add(Expression.Eq("IdProtocolo", oP));

                        IList detalle = crit.List();
                        if (detalle.Count != 0)
                        //if (oP != null)
                        {
                           int orden= tipoMarcador.getOrden(campo_MFI);

                            if (orden > 0)
                            {
                                CasoMarcadores oRegistro = new CasoMarcadores();

                                oRegistro.IdProtocolo = oP.IdProtocolo;
                                oRegistro.IdCasoFiliacion = oCaso.IdCasoFiliacion;

                                oRegistro.Ip = "";

                                oRegistro.Marcador = campo_MFI;
                                oRegistro.Allello1 = campo_Bead;
                                oRegistro.Allello2 = campo_valor;
                                if (arr.Length >= 5)
                                    oRegistro.Allello3 = arr[4].ToString();
                                if (arr.Length >= 6)
                                    oRegistro.Allello4 = arr[5].ToString();
                                if (arr.Length >= 7)
                                    oRegistro.Allello5 = arr[6].ToString();
                                if (arr.Length >= 8)
                                    oRegistro.Allello6 = arr[7].ToString();
                                if (arr.Length >= 9)
                                    oRegistro.Allello7 = arr[8].ToString();
                                if (arr.Length >= 10)
                                    oRegistro.Allello8 = arr[9].ToString();
                                if (arr.Length >= 11)
                                    oRegistro.Allello9 = arr[10].ToString();
                                if (arr.Length >= 12)
                                    oRegistro.Allello10 = arr[11].ToString();
                                oRegistro.Subitem = subitem;
                                oRegistro.Orden = orden;
                                oRegistro.Save();
                            }
                        }
                    } //tipo ==1
                    //else
                    //{
                    //    int k;
                    //    k=arr.Length;
                    //}

                }



                if (tipo == 2)
                {
                    if (arr.Length >= 2)
                    {
                        string campo_MFI = arr[0].ToString();
                        string campo_Bead = arr[1].ToString();
                        if ((campo_MFI.Trim() != "SE33") && (campo_MFI.ToUpper().IndexOf("TOTAL") == -1))

                        {
                            int orden = tipoMarcador.getOrden(campo_MFI);
                            if (orden>0)
                            { 
                            ///Se reutiliza la tabla MindrayResultado para volcar temporalmente los resultados del luminex
                            CasoMarcadores oRegistro = new CasoMarcadores();

                            oRegistro.IdProtocolo = 0;
                            oRegistro.IdCasoFiliacion = oCaso.IdCasoFiliacion;

                            oRegistro.Ip = campo_Bead.Replace(".", ",");
                            oRegistro.Marcador = campo_MFI;
                            oRegistro.Allello1 = "";
                            oRegistro.Allello2 = "";
                            oRegistro.Allello3 = "";
                            oRegistro.Allello4 = "";
                            oRegistro.Allello5 = "";
                            oRegistro.Allello6 = "";
                            oRegistro.Allello7 = "";
                            oRegistro.Allello8 = "";
                            oRegistro.Allello9 = "";
                            oRegistro.Allello10 = "";
                            oRegistro.Subitem = "";
                                oRegistro.Orden = orden;
                            oRegistro.Save();
                            }
                        }
                        if (campo_MFI.ToUpper().IndexOf("TOTAL") != -1) // CONTIENE TOTAL
                        {
                            CasoMarcadores oRegistro = new CasoMarcadores();

                            oRegistro.IdProtocolo = 0;
                            oRegistro.IdCasoFiliacion = oCaso.IdCasoFiliacion;

                            double d;
                            string str = campo_Bead.ToString().Replace("+", "-");
                            if (Double.TryParse(str, out d)) // if done, then is a number
                            {


                                oRegistro.Ip = d.ToString();
                                oRegistro.Marcador = "TOTAL";
                                oRegistro.Allello1 = "";
                                oRegistro.Allello2 = "";
                                oRegistro.Allello3 = "";
                                oRegistro.Allello4 = "";
                                oRegistro.Allello5 = "";
                                oRegistro.Allello6 = "";
                                oRegistro.Allello7 = "";
                                oRegistro.Allello8 = "";
                                oRegistro.Allello9 = "";
                                oRegistro.Allello10 = "";
                                oRegistro.Subitem = "";
                                oRegistro.Orden = 999999;
                                oRegistro.Save();
                            }
                        }


                    }
                }// TIPO 2



            }
            catch (Exception ex)
            {
                string exception = "";
                exception = ex.Message + "<br>";
                estatus1.Text = "hay líneas del archivo que han sido ignoradas por no tener el formato esperado por el sistema." + exception;
            }
        }

       

        private void BorrarResultadosTemporales(int tipo)
        {
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            if (oRegistro.IdTipoCaso != 2) // para forense no borra; para filiacion y quimerismo si borra
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(CasoMarcadores));


                crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));


                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    foreach (CasoMarcadores oDetalle in detalle)
                    {
                        if (tipo == 1)
                        {
                            if (oDetalle.IdProtocolo > 0)
                                oDetalle.Delete();
                        }
                        else
                        {
                            if (oDetalle.IdProtocolo == 0)
                                oDetalle.Delete();
                        }

                    }
                }

                //if (Desde.Value == "Carga")
                //    oRegistro.GrabarAuditoria("Borra Marcadores", oUser.IdUsuario, "");
                //else
                    oRegistro.GrabarAuditoria("Borra Marcadores", oUser.IdUsuario, "");

            }
        }
        protected void subir_Click(object sender, EventArgs e)
        {

            try
            {
                if (trepador.HasFile)
                {
                    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

                    if (Directory.Exists(directorio))
                    {
                        string archivo = directorio + "\\" + trepador.FileName;


                        trepador.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";


                        ProcesarFichero();

                        CargarTablaResultados(1);

                        //SetSelectedTab(TabIndex.ONE);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }


        }
        private object LeerDatosFA()
        {

            string m_strSQL = @"select distinct P.idProtocolo, P.numero, Pac.apellido + ' ' + Pac.nombre as pacientep, P.edad, P.sexo , Pa.nombre as Parentesco, CFP.idCasoFiliacion as Caso
, case when GM.idProtocolo>=0 then 'En Base FA' else 
	case when GME.idProtocolo>=0 then 'Excluido' else 'Pendiente' end 
 end  as estado
 from LAB_CasoMarcadores  as CM 
inner join LAB_Protocolo as P on p.idProtocolo= CM.idProtocolo
inner join LAB_CasoFiliacionProtocolo CFP on CFP.idProtocolo=P.idprotocolo
inner join LAB_Parentesco as Pa on Pa.idParentesco= CFP.idTipoParentesco
inner join Sys_Paciente as Pac on Pac.idPaciente= P.idPaciente
left join gen_marcadores as GM on GM.idProtocolo=P.idProtocolo
left join gen_marcadoresexcluidos as GME on GME.idProtocolo= P.idProtocolo
where P.idPaciente<>-1 and    CFP.idCasoFiliacion = " + id.Value;

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        private void CargarTablaResultados(int tipo)
        {


            // muesra total LR
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            ISession m_session = NHibernateHttpModule.CurrentSession;
            if (oRegistro.IdTipoCaso != 2) // FILIACION o quimermismo
            {
                gvTablaForense.DataSource = LeerDatos(tipo);
                gvTablaForense.DataBind();


                ICriteria crit2 = m_session.CreateCriteria(typeof(CasoMarcadores));
                crit2.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));
                crit2.Add(Expression.Eq("IdProtocolo", 0));
                crit2.Add(Expression.Eq("Marcador", "TOTAL"));

                

                IList itemsm = crit2.List();
                foreach (CasoMarcadores oM in itemsm)
                {
                    txTotalLR.Text = oM.Ip;
                }
                //btnTablaPDF.Visible = false;
                gvTablaForense2.Visible = false;
                ////
            }
            if (oRegistro.IdTipoCaso == 2) // FORENSE
            {


                ICriteria crit3 = m_session.CreateCriteria(typeof(CasoMarcadores));
                crit3.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));
                IList listamarcadores = crit3.List();

                if (listamarcadores.Count > 0)
                {
                    btnDescargarMarcador.Visible = true;
                    //btnTablaPDF.Visible = true;
                    //btnDescargarMarcadoresY.Visible = true;
                }
                else
                {
                    btnDescargarMarcador.Visible = false;
                    //btnTablaPDF.Visible = false;
                    //btnDescargarMarcadoresY.Visible = false;
                }

                gvTablaForense.Visible = false; // tabla cruzada de filiacion

                gvTablaForense2.DataSource = LeerDatosForense(oRegistro.IdCasoFiliacion);
                gvTablaForense2.DataBind();




            }




        }

        private object LeerDatosForense(int idCasoFiliacion)
        {
            string m_strSQL = @" select  distinct convert (varchar, idprotocolo) +';'+ isnull (subitem, '') as idProtocoloSubitem, dbo.NumeroProtocolo (idprotocolo) as protocolo,  isnull (subitem, '') as subitem
from LAB_CasoMarcadores where idCasoFiliacion=" + idCasoFiliacion.ToString();
            
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        private object LeerDatos(int tipo)
        {

            string columnas = "";
            string lista, lista1 = "";
            ///Eliminar los detalles para volverlos a crear            
            ISession m_session = NHibernateHttpModule.CurrentSession;
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            ICriteria crit = m_session.CreateCriteria(typeof(CasoFiliacionProtocolo));
            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro));
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (CasoFiliacionProtocolo oDetalle in detalle)
                {
                    //Business.Data.Laboratorio.CasoMarcadores oMarca = new Business.Data.Laboratorio.CasoMarcadores();

                    ICriteria critMarcadores = m_session.CreateCriteria(typeof(CasoMarcadores));
                    critMarcadores.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));
                    critMarcadores.Add(Expression.Eq("IdProtocolo", oDetalle.IdProtocolo.IdProtocolo));
                    critMarcadores.Add(Expression.Eq("Marcador", "AMEL"));
                    IList detalleMarcadores = critMarcadores.List();
                    if (detalleMarcadores.Count > 0)
                    {
                        if (lista1 == "")
                            lista1 = "[" + oDetalle.IdProtocolo.IdProtocolo.ToString() + "]";
                        else
                            lista1 += ",[" + oDetalle.IdProtocolo.IdProtocolo.ToString() + "]";

                        if (columnas == "")
                            columnas = "[" + oDetalle.IdProtocolo.Numero.ToString() + "]";
                        else
                            columnas += ",[" + oDetalle.IdProtocolo.Numero.ToString() + "]";
                    }
                }
            }


            lista = lista1.Replace("[", "'").Replace("]", "'");

            string m_strSQL = @"  SELECT tipo as [ ], [0] as IP
FROM
 (SELECT isnull( P.numero,0) as numero, marcador as tipo ,
 case when IP<>'' then ip else
 allello1 +' - ' +   case when marcador<>'DYS391' then  
  case when allello2='' then allello1 else allello2 end 
       else allello2 end end as y,
	            orden        FROM LAB_CasoMarcadores    as M
				  left join LAB_Protocolo as P on P.idprotocolo= M.idprotocolo
				    where M.idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString() + @" and  M.idprotocolo  in (-1) 
				  ) as SourceTable PIVOT(max(y)     FOR numero IN([0] ))  AS PivotTable order by orden ";



            if (lista != "")
                m_strSQL = @" SELECT tipo as [ ], " + columnas + @"
 FROM(    
  SELECT isnull( P.numero,0) as numero, marcador as tipo , 
  allello1 +' - ' +   case when marcador<>'DYS391' then
  case when allello2='' then allello1 else allello2 end   
  else allello2 end  as y,   orden    
   FROM LAB_CasoMarcadores    as M
				  left join LAB_Protocolo as P on P.idprotocolo= M.idprotocolo
				    where M.idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString() + " and M.idprotocolo in (" + lista + @"))
   AS SourceTable PIVOT(max(y) 

   FOR numero IN(" + columnas + @" )) AS PivotTable order by orden";

            //if (tipo==2)
            if ((oRegistro.tieneMarcadorIP()) && (lista != ""))
                m_strSQL = @"  SELECT tipo as [ ], [0] as IP, " + columnas + @"  FROM
 (       SELECT isnull( P.numero,0) as numero, marcador as tipo ,
 case when IP<>'' then ip else
 allello1 +' - ' +   case when marcador<>'DYS391' then  
  case when allello2='' then allello1 else allello2 end 
       else allello2 end end as y,
	            orden        FROM LAB_CasoMarcadores    as M
				  left join LAB_Protocolo as P on P.idprotocolo= M.idprotocolo
				    where M.idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString() + " and  M.idprotocolo  in (" + lista + @",0) 
				  ) as SourceTable PIVOT(max(y)     FOR numero IN(" + columnas + @",[0] ))  AS PivotTable order by orden ";

            if ((oRegistro.tieneMarcadorIP()) && (lista == ""))
            {
                m_strSQL = @"  SELECT tipo as [ ], [0] as IP
FROM
 (       SELECT isnull( P.numero,0) as numero, marcador as tipo ,
 case when IP<>'' then ip else
 allello1 +' - ' +   case when marcador<>'DYS391' then  
  case when allello2='' then allello1 else allello2 end 
       else allello2 end end as y,
	           orden        FROM LAB_CasoMarcadores    as M
				  left join LAB_Protocolo as P on P.idprotocolo= M.idprotocolo
				    where M.idCasoFiliacion=" + oRegistro.IdCasoFiliacion.ToString() + @" and  M.idprotocolo  in (0) 
				  ) as SourceTable PIVOT(max(y)     FOR numero IN([0] ))  AS PivotTable order by orden ";

            }
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }

        protected void subir0_Click(object sender, EventArgs e)
        {
            try
            {
                if (trepador2.HasFile)
                {
                    string directorio = Server.MapPath(""); // @"C:\Archivos de Usuario\";

                    if (Directory.Exists(directorio))
                    {
                        string archivo = directorio + "\\" + trepador2.FileName;


                        trepador2.SaveAs(archivo);
                        estatus.Text = "El archivo se ha procesado exitosamente.";


                        ProcesarFichero2();

                        CargarTablaResultados(2);

                        //SetSelectedTab(TabIndex.ONE);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(
                           "El directorio en el servidor donde se suben los archivos no existe");
                    }
                }
            }
            catch (Exception ex) { estatus.Text = "ha ocurrido un error: " + ex.Message.ToString() + " .Comuniquese con el administrador."; }

        }

        protected void btnBorrarTablaMarcadores_Click(object sender, EventArgs e)
        {
            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

            CasoMarcadores oMarca = new CasoMarcadores();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session.CreateCriteria(typeof(CasoMarcadores));
            crit2.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));



            IList itemsm = crit2.List();
            foreach (CasoMarcadores oM in itemsm)
            {
                oM.Delete();
            }
            CargarTablaResultados(2);

        }

        protected void btnMuestras_Click(object sender, EventArgs e)
        {

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
            string m_parametroFiltro = "&Nombre=" + oRegistro.Nombre;

            if (oRegistro.IdTipoCaso == 1) /// filiacion
            {
                Context.Items.Add("idServicio", 6);
                Context.Items.Add("id", id.Value);
                Context.Items.Add("parametros", m_parametroFiltro);
                Context.Items.Add("Desde", Desde.Value);
                Server.Transfer("CasoEdit.aspx");
            }
            else // forense
            {
                //string m_parametroFiltro = "&Nombre=" + txtNombre.Text;
                Context.Items.Add("idServicio", 6);
                Context.Items.Add("id", id.Value);
                Context.Items.Add("parametros", m_parametroFiltro);
                Context.Items.Add("Desde", Desde.Value);
                Server.Transfer("CasoForenseView.aspx");
            }
        }

        protected void ValidaMuestra_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

                Guardar(oRegistro, "Muestras");
                //oRegistro.Muestra = txtObservacionMuestras.Text;
                //oRegistro.Save();

                if (Desde.Value == "Carga")
                {
                    oRegistro.GrabarAuditoria(ValidaMuestra.Text, int.Parse(Session["idUsuario"].ToString()), "");
                    Context.Items.Add("Desde", Desde.Value);
                }
                else
                {
                    oRegistro.GrabarAuditoria(ValidaMuestra.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");
                    Context.Items.Add("Desde", "Revalida");
                }

                Context.Items.Add("id", id.Value);


                Server.Transfer("CasoResultado3.aspx?");
            }


        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(GridView1.DataKeys[row.RowIndex].Value.ToString()));


                    ISession m_session = NHibernateHttpModule.CurrentSession;

                    //verifica que ingrese solo el protocolo del caso
                    CasoMarcadores oCasoMarcadores = new CasoMarcadores();
                    ICriteria crit = m_session.CreateCriteria(typeof(CasoMarcadores));
                    crit.Add(Expression.Eq("IdProtocolo", oProtocolo.IdProtocolo));

                    IList detalle = crit.List();
                    foreach (CasoMarcadores oDetalle in detalle)
                    {
                        Marcadores oRegistro = new Marcadores();

                        oRegistro.IdProtocolo = oDetalle.IdProtocolo;
                        oRegistro.IdPaciente = oProtocolo.IdPaciente.IdPaciente;
                        oRegistro.Marcador = oDetalle.Marcador;
                        oRegistro.Allello1 = oDetalle.Allello1;
                        oRegistro.Allello2 = oDetalle.Allello2;

                        oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                        oRegistro.FechaRegistro = DateTime.Now;

                        oRegistro.Save();
                    } //for                
                } // cheked
            }// for

            Context.Items.Add("id", id.Value);
            Context.Items.Add("Desde", "Valida");

            Server.Transfer("CasoResultado3.aspx?");

        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {

                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(GridView1.DataKeys[row.RowIndex].Value.ToString()));



                    MarcadoresExcluidos oRegistro = new MarcadoresExcluidos();

                    oRegistro.IdProtocolo = oProtocolo.IdProtocolo;
                    oRegistro.IdPaciente = oProtocolo.IdPaciente.IdPaciente;

                    oRegistro.IdUsuarioRegistro = int.Parse(Session["idUsuario"].ToString());
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();


                } // cheked
            }// for

            Context.Items.Add("id", id.Value);
            Context.Items.Add("Desde", "Valida");

            Server.Transfer("CasoResultado3.aspx?");
        }

        protected void Valida1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));



                Guardar(oRegistro, "Metodo");

                if (Desde.Value == "Carga")
                {

                    oRegistro.GrabarAuditoria(ValidaMetodo.Text, int.Parse(Session["idUsuario"].ToString()), "");
                    Context.Items.Add("Desde", Desde.Value);
                }
                else
                {
                    oRegistro.GrabarAuditoria(ValidaMetodo.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");
                    Context.Items.Add("Desde", "Revalida");
                }
                Context.Items.Add("id", id.Value);


                Server.Transfer("CasoResultado3.aspx?");
            }
        }

        protected void Valida2_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
                Guardar(oRegistro, "Resultado");
                Context.Items.Add("id", id.Value);

                if (Desde.Value == "Carga")
                {
                    Context.Items.Add("Desde", Desde.Value);
                    oRegistro.GrabarAuditoria(ValidaConclusiones.Text, int.Parse(Session["idUsuario"].ToString()), "");
                }
                else
                {
                    oRegistro.GrabarAuditoria(ValidaConclusiones.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");
                    Context.Items.Add("Desde", "Revalida");
                }

                Server.Transfer("CasoResultado3.aspx?");
            }

        }

        protected void Valida3_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
                Guardar(oRegistro, "Bibliografia");
                Context.Items.Add("id", id.Value);

                if (Desde.Value == "Carga")
                {
                    Context.Items.Add("Desde", Desde.Value);
                    oRegistro.GrabarAuditoria(ValidaBibliografia.Text, int.Parse(Session["idUsuario"].ToString()), "");
                }
                else
                {
                    oRegistro.GrabarAuditoria(ValidaBibliografia.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");


                    Context.Items.Add("Desde", "Revalida");
                }

                Server.Transfer("CasoResultado3.aspx?");
            }

        }

        protected void lnkValidaEncabezado_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));

                Guardar(oRegistro, "Encabezado");
                if (Desde.Value == "Carga")
                {
                    Context.Items.Add("Desde", Desde.Value);
                    oRegistro.GrabarAuditoria(lnkValidaEncabezado.Text, int.Parse(Session["idUsuario"].ToString()), "");
                }
                else
                {
                    Context.Items.Add("Desde", "Revalida");
                    oRegistro.GrabarAuditoria(lnkValidaEncabezado.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");
                }

                Context.Items.Add("id", id.Value);


                Server.Transfer("CasoResultado3.aspx?");
            }

        }

        protected void lnkValidarMarcadores_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
                Guardar(oRegistro, "Marcador");
                if (Desde.Value == "Carga")
                {
                    oRegistro.GrabarAuditoria(lnkValidarMarcadores.Text, int.Parse(Session["idUsuario"].ToString()), "");
                    Context.Items.Add("Desde", Desde.Value);
                }
                else
                {
                    Context.Items.Add("Desde", "Revalida");
                    oRegistro.GrabarAuditoria(lnkValidarMarcadores.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");
                }

                Context.Items.Add("id", id.Value);


                Server.Transfer("CasoResultado3.aspx?");
            }

        }

        protected void imgPdfPreeliminar_Click(object sender, EventArgs e)
        {
            Imprimir("Pre");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
                Guardar(oRegistro, "Resultados");
                if (Desde.Value == "Carga")
                {
                    oRegistro.GrabarAuditoria(ValidaResultado.Text, int.Parse(Session["idUsuario"].ToString()), "");
                    Context.Items.Add("Desde", Desde.Value);
                }
                else
                {
                    Context.Items.Add("Desde", "Revalida");
                    oRegistro.GrabarAuditoria(ValidaResultado.Text, int.Parse(Session["idUsuarioValida"].ToString()), "");
                }

                Context.Items.Add("id", id.Value);


                Server.Transfer("CasoResultado3.aspx?");
            }
        }
 

        protected void gvTablaForense2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = this.gvTablaForense2.DataKeys[e.Row.RowIndex].Value.ToString();

                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";

                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
                if (id.Value != "") oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));


                if (oRegistro.IdUsuarioValida > 0)
                    CmdEliminar.Visible = false;

            }

        }

        protected void gvTablaForense2_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "Eliminar")
            {


                EliminarMuestraTabla(e.CommandArgument);
                CargarTablaResultados(2);


            }
        }

        private void EliminarMuestraTabla(object commandArgument)
        {
            string[] arr = commandArgument.ToString().Split((";").ToCharArray());


            string idProtocolo = arr[0].ToString();
            string subitem = arr[1].ToString();

            Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));


            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(CasoMarcadores));


            crit.Add(Expression.Eq("IdCasoFiliacion", oRegistro.IdCasoFiliacion));
            crit.Add(Expression.Eq("IdProtocolo", int.Parse(idProtocolo)));
            crit.Add(Expression.Eq("Subitem", subitem));


            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (CasoMarcadores oDetalle in detalle)
                {

                    oDetalle.Delete();
                }


            }

            if (Desde.Value == "Carga")
                oRegistro.GrabarAuditoria("Borra Marcadores", int.Parse(Session["idUsuario"].ToString()), oRegistro.IdCasoFiliacion.ToString()+"-idProtocolo:"+idProtocolo+"-subitem:"+ subitem);
            else
                oRegistro.GrabarAuditoria("Borra Marcadores", int.Parse(Session["idUsuarioValida"].ToString()), oRegistro.IdCasoFiliacion.ToString() + "-idProtocolo:" + idProtocolo + "-subitem:" + subitem);




        }

        protected void btnDescargar_Click(object sender, EventArgs e)
        {
            string subitem="";

                Business.Data.Laboratorio.CasoFiliacion oRegistro = new Business.Data.Laboratorio.CasoFiliacion();
            oRegistro = (Business.Data.Laboratorio.CasoFiliacion)oRegistro.Get(typeof(Business.Data.Laboratorio.CasoFiliacion), int.Parse(id.Value));
            CrystalReportSource oCr = new CrystalReportSource();
            ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();

            encabezado1.Value = "Anexo Marcadores Caso Nro." + oRegistro.IdCasoFiliacion.ToString();

            ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();

            encabezado2.Value = ddlTipoMarcador.SelectedItem.Text;

            if (ddlTipoMarcador.SelectedItem.Text.IndexOf("Y") > -1)
                  subitem = "Y";


            string listaProtocolos = GetListaSeleccionados();



            oCr.Report.FileName = "TablaMarcadores.rpt";
            oCr.ReportDocument.SetDataSource(oRegistro.getTablaForense (ddlTipoMarcador.SelectedValue, subitem, listaProtocolos));
            oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);

            oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);


            oCr.DataBind();

            if (Desde.Value == "Carga")
                oRegistro.GrabarAuditoria("Imprime "+ ddlTipoMarcador.SelectedItem.Text, int.Parse(Session["idUsuario"].ToString()), "Marcadores_" + oRegistro.Nombre.Trim());
            else
                oRegistro.GrabarAuditoria("Imprime " + ddlTipoMarcador.SelectedItem.Text, int.Parse(Session["idUsuarioValida"].ToString()), "Marcadores_" + oRegistro.Nombre.Trim());

            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Marcadores_" + oRegistro.Nombre.Trim());




        }

        private string GetListaSeleccionados()
        {
            string lista = ""; 
            foreach (GridViewRow row in gvTablaForense2.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    string m_idProtocolo = gvTablaForense2.DataKeys[row.RowIndex].Value.ToString();
                    //convert(varchar, idprotocolo) + ';' + isnull(subitem, '')

                           string[] arr = m_idProtocolo.Split((";").ToCharArray());
                    
                        if (lista == "")
                            lista = arr[0].ToString();
                        else
                            lista = lista + "," + arr[0].ToString();

                }
            }

            return lista;
        }
    }
}