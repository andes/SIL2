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
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using System.IO;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.Drawing;

namespace WebLab.Items
{
    public partial class ItemEdit2 : System.Web.UI.Page
    {

        CrystalReportSource oCr = new CrystalReportSource();
        Configuracion oC = new Configuracion();
        Usuario oUser = new Usuario();
        //   Item oItem = new Item();
        private enum TabIndex
        {
            DEFAULT = 0,
            ONE = 1,
            TWO = 2,
            THREE = 3,
            CUARTO = 4,
            QUINTO = 5,
            SECTO = 6,
            SEPTIMO = 7, OCTAVO = 8
            // you can as many as you want here
        }
        private void SetSelectedTab(TabIndex tabIndex)
        {
            HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            //oC = (Configuracion)oC.Get(typeof(Configuracion), 1);
            if (Session["idUsuario"] != null)
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //     oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1, "IdEfector", oEfector);
            else
                Response.Redirect("../FinSesion.aspx", false);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                    
                    VerificaPermisos("Analisis");
                    CargarListas();
                    if (Request["id"] != null)
                    {
                        Item oItem = new Item();
                        oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                        if (oItem != null)
                        { //Se optimiza accesos a la bd, se buscan datos segun condiciones
                            MostrarDatos(oItem); HabilitarDatos();
                            if (oItem.IdTipoResultado != 0)
                                MostrarDatosValoresReferencia();
                            if ((oItem.Tipo=="P") && (oItem.IdTipoResultado ==0))
                            MostrarDatosDiagrama();
                            if (oItem.IdTipoResultado >= 2) //si no es numerico muestra los predefinidos
                                MostrarDatosResultadosPredefinidos(oItem);
                            MostrarDatosRecomendaciones();
                        }
                    }
                    if (Request["idEfector"].ToString() == "227")
                    {
                        txtEfector.Text = "ADMINISTRADOR";
                        btnNuevo.Visible = true;
                        btnAgregarPresentacion.Visible = true;
                        btnAgregarMuestra.Visible = true;
                        chkEtiquetaAdicional.Enabled = true;
                    }
                    else
                    {
                        Efector oEfector = new Efector();
                        oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));
                        if (oEfector != null)
                        {
                            txtEfector.Text = oEfector.Nombre;
                            btnNuevo.Visible = false;
                            pnlDiagrama.Enabled = false;
                            pnlPredefinidos.Enabled = false;
                            btnAgregarPresentacion.Visible = false;
                            btnAgregarMuestra.Visible = false;
                            btnGuardarRPDefecto.Enabled = true;
                            chkEtiquetaAdicional.Enabled = false; ///el efector no puede cambiar por que es la misma configuracion para todos
                        }
                    }
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
            }
        }

        private void HabilitarDatos()
        {
            if (Request["idEfector"].ToString() != "227")

            {

                txtCodigo.Enabled = false;
                txtNombre.Enabled = false;
                txtDescripcion.Enabled = false;
                txtDuracion.Disabled = true;
                ddlServicio.Enabled = false;
                ddlArea.Enabled = false;
                rdbTipo.Enabled = false;
                rdbCategoria.Enabled = false;
                ddlTipoResultado.Enabled = false;
                ddlDecimales.Enabled = false;
                ddlMultiplicador.Enabled = false;
                txtValorMinimo.Enabled = false;
                txtValorMaximo.Enabled = false;
                ddlCaracter.Enabled = false;
                chkCodificaHiv.Enabled = false;
                ddlUnidadMedida.Enabled = false;
                /*Las recomendaciones no se pueden modificar.*/
                ddlRecomendacion.Enabled = false;
                btnAgregarRecomendacion.Enabled = false;
                gvListaRecomendacion.Enabled = false;
                chkEtiquetaAdicional.Enabled = false;


                Efector oEfector = new Efector();
                oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));
                if (oEfector != null)
                    MostrarDatosEfector(oEfector);

                // no puede modificar diagrama.
                btnGuardarDiagrama.Visible = false;
                // no puede modificar resultados predefinidos
                btnGuardarRP.Visible = false;
                ///no se puede recplicar a los demas efectores
                btnAplicarEfectores.Visible = false;
                btnAplicarEfectoresNP.Visible = false;

                btnAgregarPresentacion.Visible = true;

                pnlNuevoVR.Visible = false; ///El efector no puede cargar valores de referencia. Solo puede elegir presentacion.
                PnlVREfector.Visible = true;
                gdPresentacion.Enabled = false;
            }


        }

        private void MostrarDatosEfector(Efector oEfector)
        {
            Item oItemPrincipal = new Item();
            oItemPrincipal = (Item)oItemPrincipal.Get(typeof(Item), int.Parse(Request["id"].ToString()));


            ItemEfector oItem = new ItemEfector();
            oItem = (ItemEfector)oItem.Get(typeof(ItemEfector), "IdItem", oItemPrincipal, "IdEfector", oEfector);

            if (oItem != null)
            {
                //      txtRecomendaciones.Text=oItem.Recomendacion;
                if (oItem.IdEfectorDerivacion != oItem.IdEfector)
                {
                    ddlDerivable.SelectedValue = "1";
                    HabilitarDerivador();
                    ddlEfector.SelectedValue = oItem.IdEfectorDerivacion.IdEfector.ToString();
                }
                else ddlDerivable.SelectedValue = "0";



                if (oItem.Disponible) ddlDisponible.SelectedValue = "1";
                else ddlDisponible.SelectedValue = "0";

                if (oItem.SinInsumo)
                {
                    lblSininsumo.Text = "SIN INSUMO";
                    lblSininsumo.ForeColor = Color.Red;
                }
                else
                {
                    lblSininsumo.Text = "DISPONIBLE-CON INSUMO";
                    lblSininsumo.ForeColor = Color.Green;
                }


                if (oItem.Informable) ddlInformable.SelectedValue = "1";
                else ddlInformable.SelectedValue = "0";

                ddlPresentacionEfectorDefecto.SelectedValue = oItem.IdPresentacionDefecto.ToString();

                MostrarDatosValoresReferencia();
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (this.oCr.ReportDocument != null)
            {
                this.oCr.ReportDocument.Close();
                this.oCr.ReportDocument.Dispose();
            }
        }

        ///************************************Inicio de Recomendaciones para el pacientes        ********************//
        private void MostrarDatosRecomendaciones()
        {
            ddlRecomendacion.SelectedValue = "0";
            gvListaRecomendacion.AutoGenerateColumns = false;
            gvListaRecomendacion.DataSource = LeerDatosRecomendacion();
            gvListaRecomendacion.DataBind();
        }

        private object LeerDatosRecomendacion()
        {
            string m_strSQL = @" SELECT IR.idItemRecomendacion, R.descripcion as recomendacion 
                              FROM LAB_ItemRecomendacion IR with (nolock)
                              INNER JOIN LAB_Recomendacion R with (nolock) ON R.idRecomendacion=IR.idRecomendacion 
                               WHERE IR.idItem=" + Request["id"].ToString();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }

        protected void btnAgregarRecomendacion_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                AgregarRecomendacion();
                MostrarDatosRecomendaciones();

            }

        }

        private void AgregarRecomendacion()
        {
            ItemRecomendacion oRegistro = new ItemRecomendacion();
            Item oItem = new Item();

            Recomendacion oRec = new Recomendacion();
            
            oRegistro.IdItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
            oRegistro.IdRecomendacion = (Recomendacion)oRec.Get(typeof(Recomendacion), int.Parse(ddlRecomendacion.SelectedValue));
            oRegistro.Save();


        }

        protected void gvListaRecomendacion_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                EliminarItemRecomendacion(e.CommandArgument);
                MostrarDatosRecomendaciones();

            }
        }

        private void EliminarItemRecomendacion(object idItem)
        {
            ItemRecomendacion oRegistro = new ItemRecomendacion();
            oRegistro = (ItemRecomendacion)oRegistro.Get(typeof(ItemRecomendacion), int.Parse(idItem.ToString()));
            oRegistro.Delete();
        }

        protected void gvListaRecomendacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[1].Controls[1];
                CmdEliminar.CommandArgument = this.gvListaRecomendacion.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                }
            }
        }

        ///**************************************Fin de REcomendaciones para el paciente********************************************//
        ///


        ///******************************************Inicio de Resultados Predefinidos***************************************************//
        private void MostrarDatosResultadosPredefinidos(Item oItem)
        {
         
            if (oItem != null)
            {
                //lblItem.Text = oItem.Codigo + " - " + oItem.Nombre;
                Efector oEfector = new Efector();
                oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));
                if (oEfector != null)
                {
                    CargarListasRPDefecto();


                    // dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
                    ResultadoItem oDetalle = new ResultadoItem();
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                    crit.Add(Expression.Eq("IdItem", oItem));
                    crit.Add(Expression.Eq("Baja", false));
                    crit.Add(Expression.Eq("IdEfector", oEfector));

                    string sDatos = "";
                    IList items = crit.List();

                    foreach (ResultadoItem oDet in items)
                    {
                        string efe = "0";
                        if (oDet.IdEfectorDeriva > 0)
                        {
                            Efector oEfectorDer = new Efector();
                            oEfectorDer = (Efector)oEfectorDer.Get(typeof(Efector), oDet.IdEfectorDeriva);
                            efe = oEfectorDer.Nombre + "-" + oEfectorDer.IdEfector.ToString();
                        }

                        if (sDatos == "")
                            sDatos = oDet.Resultado + "#" + efe + "@";
                        else
                            sDatos += oDet.Resultado + "#" + efe + "@";

                        if (oDet.ResultadoDefecto)
                            ddlResultadoPorDefecto.SelectedValue = oDet.IdResultadoItem.ToString();
                    }

                    TxtDatosResultados.Value = sDatos;
                    // LeerDatosRP();
                    //CargarListasRPDefecto();
                    //ddlResultadoPorDefecto.SelectedValue = oItem.IdResultadoPorDefecto.ToString();
                }
            }

        }


        private void CargarGrillaRP()
        {
            txtNombreRP.Text = "";
            //gvLista.AutoGenerateColumns = false;
            //gvLista.DataSource = LeerDatos();
            //gvLista.DataBind();
        }

        //private object LeerDatosRP()
        //{
        //    string m_strSQL = " SELECT idResultadoItem, resultado" +
        //                      " FROM LAB_ResultadoItem " +
        //                      " WHERE (baja = 0) and idItem=" + Request["id"].ToString() +
        //                      " ORDER BY idResultadoItem"; // SE PONE EL ORDEN EN QUE SE FUE AGREGANDO

        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);



        //    return Ds.Tables[0];
        //}

        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        Guardar();
        //       // CargarGrilla();
        //    }
        //}

        private void GuardarRP()
        {
            if (Request["id"] != null)
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                Efector oEfector = new Efector();
                oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));



                Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                ///Borrar los existentes 
                ResultadoItem oDetalle = new ResultadoItem();
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                crit.Add(Expression.Eq("IdItem", oItem));
                crit.Add(Expression.Eq("IdEfector", oEfector));

                //   string sDatos = "";
                IList items = crit.List();

                foreach (ResultadoItem oDet in items)
                {
                    oDet.Delete();
                }

                ///Guardar nuevamente
                string[] tabla = TxtDatosResultados.Value.Split('@');


                for (int i = 0; i < tabla.Length - 1; i++)
                {
                    ResultadoItem oRegistro = new ResultadoItem();
                    string resultado = "";
                    string efector = "0";
                    string[] det = tabla[i].ToString().Split('#');

                    if (det.Length > 1)
                    {
                        resultado = det[0].ToString();
                        string[] e = det[1].ToString().Split('-');
                        if (e.Length > 1)
                            efector = e[1].ToString();
                    }
                    else resultado = det[0].ToString();

                    oRegistro.IdEfector = oEfector;
                    oRegistro.IdItem = oItem;
                    oRegistro.Resultado = resultado;
                    oRegistro.IdEfectorDeriva = int.Parse(efector);

                    oRegistro.IdUsuarioRegistro = oUser;
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();
                    oItem.GrabarAuditoriaDetalleItem("Guarda", oUser, "Resultado PreDefinido", resultado, "");
                }

                GuardarRPTodoslosEfectores(oItem);
                oItem.GrabarAuditoriaDetalleItem("Guarda", oUser, "Resultado PreDefinido", "aplica a todos los efectores", "");
            }
            ////            
        }

        protected void btnGuardarRP_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                GuardarRP();

                CargarListasRPDefecto();
                /// Response.Redirect("ItemEdit2.aspx", false);                

            }
        }

        private void GuardarRPTodoslosEfectores(Item oItem)
        {

            Efector oEfectorPrincipal = new Efector();
            oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {

                ResultadoItem oDetalle = new ResultadoItem();

                ICriteria crit2 = m_session.CreateCriteria(typeof(ResultadoItem));
                crit2.Add(Expression.Eq("IdItem", oItem));
                crit2.Add(Expression.Eq("IdEfector", olabo.IdEfector));

                // borra si existiesen
                IList items2 = crit2.List();

                foreach (ResultadoItem oDet in items2)
                {
                    oDet.Delete();
                }

                //copia los resultados configurados en nivel central==> oEfectorPrincipal

                ICriteria critNew = m_session.CreateCriteria(typeof(ResultadoItem));
                critNew.Add(Expression.Eq("IdItem", oItem));
                critNew.Add(Expression.Eq("IdEfector", oEfectorPrincipal));

                //   string sDatos = "";
                IList itemsNew = critNew.List();

                foreach (ResultadoItem oDetNew in itemsNew)
                {

                    ResultadoItem oRegistro = new ResultadoItem();

                    oRegistro.IdEfector = olabo.IdEfector;
                    oRegistro.IdItem = oItem;
                    oRegistro.Resultado = oDetNew.Resultado;
                    oRegistro.IdEfectorDeriva = oDetNew.IdEfectorDeriva;

                    oRegistro.IdUsuarioRegistro = oUser;
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();
                }

            }
        }

        private void CargarListasRPDefecto()
        {
            Utility oUtil = new Utility();
            string m_ssql = @"select idResultadoItem, resultado  as nombre
from Lab_ResultadoItem with (nolock) where baja=0 and idItem= " + Request["id"].ToString() + " and  idEfector=" + Request["idEfector"].ToString() + " order by idResultadoItem";
            oUtil.CargarCombo(ddlResultadoPorDefecto, m_ssql, "idResultadoItem", "nombre");
            ddlResultadoPorDefecto.Items.Insert(0, new ListItem("               ", "0"));
            ddlResultadoPorDefecto.UpdateAfterCallBack = true;
        }
        //////////////////////////////*****************///Fin de Resultados Predefinidos*************************************//
        ///



        /// <summary>
        ///******************************************************* inicio de  Diagrama//////********************************//
        /// </summary>
        private void MostrarDatosDiagrama()
        {
            txtCodigoDiagrama.Text = "";
            //  ddlItemDiagrama.SelectedValue = "0";
            txtNombreDiagrama.Text = "";
            //  txtTitulo.Text = "";

            //gvListaDiagrama.AutoGenerateColumns = false;
            //gvListaDiagrama.DataSource = LeerDatosDiagrama();
            //gvListaDiagrama.DataBind();
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
            //lblItem.Text = oItem.Codigo + " - " + oItem.Nombre;

            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));


            // dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            ResultadoItem oDetalle = new ResultadoItem();
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
            crit.Add(Expression.Eq("IdItemPractica", oItem));
            crit.Add(Expression.Eq("IdEfector", oEfector));
            crit.Add(Expression.Eq("Orden", 1));

            string sDatos = "";
            IList items = crit.List();

            foreach (PracticaDeterminacion oDet in items)
            {
                Item oItemDeterminacion = new Item();
                oItemDeterminacion = (Item)oItemDeterminacion.Get(typeof(Item), oDet.IdItemDeterminacion);

                if (sDatos == "")
                    sDatos = oItemDeterminacion.Codigo + "#" + oDet.Titulo + "@";
                else
                    sDatos += oItemDeterminacion.Codigo + "#" + oDet.Titulo + "@";

            }

            TxtDatosDiagrama.Value = sDatos;
            //LeerDatosRP();
            //CargarListasRPDefecto();
            //ddlResultadoPorDefecto.SelectedValue = oItem.IdResultadoPorDefecto.ToString();



        }


        private void GuardarDiagrama()
        {
            if (Request["id"] != null)
            {
                Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                ///Borrar los existentes 
                EliminarDiagrama(oItem);


                ///Guardar nuevamente
                string[] tabla = TxtDatosDiagrama.Value.Split('@');


                for (int i = 0; i < tabla.Length - 1; i++)
                {
                    string[] item = tabla[i].ToString().Split('#');

                    AgregarItemDiagrama(item[0].ToString(), item[1].ToString());

                }

                GuardarDiagramaTodoslosEfectores(oItem);
            }
            ////            
        }

        private void GuardarDiagramaTodoslosEfectores(Item oItem)
        {
            try
            {
                Efector oEfectorPrincipal = new Efector();
                oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

                /// recorre para los efectores configurados en lab_configuracion
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

                IList items = crit.List();

                foreach (Configuracion olabo in items)
                {


                    ICriteria crit2 = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                    crit2.Add(Expression.Eq("IdItemPractica", oItem));
                    crit2.Add(Expression.Eq("IdEfector", olabo.IdEfector));

                    // borra si existiesen
                    IList items2 = crit2.List();

                    foreach (PracticaDeterminacion oDet in items2)
                    {
                        oDet.Delete();
                    }

                    //copia los resultados configurados en nivel central==> oEfectorPrincipal

                    ICriteria critNew = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                    critNew.Add(Expression.Eq("IdItemPractica", oItem));
                    critNew.Add(Expression.Eq("IdEfector", oEfectorPrincipal));

                    //   string sDatos = "";
                    IList itemsNew = critNew.List();

                    foreach (PracticaDeterminacion oDetNew in itemsNew)
                    {

                        PracticaDeterminacion oRegistro = new PracticaDeterminacion();

                        oRegistro.IdEfector = olabo.IdEfector;
                        oRegistro.IdItemPractica = oItem;
                        oRegistro.IdItemDeterminacion = oDetNew.IdItemDeterminacion;
                        oRegistro.Titulo = oDetNew.Titulo;
                        oRegistro.Orden = oDetNew.Orden;
                        oRegistro.FormatoImpresion = oDetNew.FormatoImpresion;

                        oRegistro.IdUsuarioRegistro = oUser;
                        oRegistro.FechaRegistro = DateTime.Now;

                        oRegistro.Save();
                    }


                }
            }
            catch (Exception ex)
            { }
        }

        //private object LeerDatosDiagrama()
        //{
        //    string m_strSQL = " SELECT PD.idPracticaDeterminacion as idDiagrama, " +
        //                      " CASE WHEN PD.iditemdeterminacion = 0 THEN PD.titulo ELSE I.codigo + ' - ' + I.nombre END AS nombre," +
        //                      " PD.titulo as textoimprimir" +
        //                      " FROM LAB_PracticaDeterminacion AS PD " +
        //                      " left JOIN LAB_Item AS I ON PD.idItemDeterminacion = I.idItem" +
        //                      " WHERE PD.iditempractica=" + Request["id"].ToString() +
        //                      " ORDER BY PD.idPracticaDeterminacion"; // SE PONE EL ORDEN EN QUE SE FUE AGREGANDO

        //    DataSet Ds = new DataSet();
        //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlDataAdapter adapter = new SqlDataAdapter();
        //    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
        //    adapter.Fill(Ds);



        //    return Ds.Tables[0];
        //}





        //protected void btnAgregarItemDiagrama_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        AgregarItemDiagrama();
        //        MostrarDatosDiagrama();

        //    }
        //}

        private void AgregarItemDiagrama(string codigo, string titulo)
        {
            //PracticaDeterminacion oRegistro = new PracticaDeterminacion();
            Item oItemPractica = new Item();
            Usuario oUser = new Usuario();


            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);


            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            oItemPractica = (Item)oItemPractica.Get(typeof(Item), int.Parse(Request["id"].ToString()));



            //if (noExiste(oItemPractica, oC.IdEfector))
            //{
            Item oI = new Item();
            oI = (Item)oI.Get(typeof(Item), "Codigo", codigo, "Baja", false);

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));

            crit.Add(Expression.Eq("IdItemPractica", oI));
            crit.Add(Expression.Eq("IdEfector", oEfector));

            IList lista = crit.List();
            if (lista.Count > 0)
            {

                if (titulo.Length > 99) titulo = titulo.Substring(0, 99);
                PracticaDeterminacion oRegistro = new PracticaDeterminacion();
                oRegistro.IdEfector = oEfector; // oC.IdEfector;
                oRegistro.IdItemPractica = oItemPractica;

                oRegistro.IdItemDeterminacion = oI.IdItem;
                oRegistro.Titulo = titulo;
                oRegistro.Orden = 1;
                oRegistro.FormatoImpresion = "";
                oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();
                oItemPractica.GrabarAuditoriaDetalleItem("Agrega", oUser, "Diagrama", titulo, "");

                foreach (PracticaDeterminacion oDet in lista)
                {


                    PracticaDeterminacion oR = new PracticaDeterminacion();
                    oR.IdEfector = oEfector; // oC.IdEfector;
                    oR.IdItemPractica = oItemPractica;
                    oR.IdItemDeterminacion = oDet.IdItemDeterminacion;
                    oR.Titulo = oDet.Titulo;
                    oR.Orden = 0;
                    oR.FormatoImpresion = "";
                    oR.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oR.FechaRegistro = DateTime.Now;
                    oItemPractica.GrabarAuditoriaDetalleItem("Agrega", oUser, "Diagrama", titulo, "");

                    oR.Save();
                }
            }
            else
            {

                PracticaDeterminacion oR = new PracticaDeterminacion();
                oR.IdEfector = oEfector; // oC.IdEfector;
                oR.IdItemPractica = oItemPractica;

                oR.IdItemDeterminacion = oI.IdItem;
                oR.Titulo = titulo;
                oR.Orden = 1;
                oR.FormatoImpresion = "";
                oR.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oR.FechaRegistro = DateTime.Now;
                oR.Save();
                oItemPractica.GrabarAuditoriaDetalleItem("Agrega", oUser, "Diagrama", titulo, "");

            }

        }

        protected void btnGuardarDiagrama_Click(object sender, EventArgs e)
        {
            GuardarDiagrama();
            string m_parametroFiltro = "&Codigo=" + Request["Codigo"].ToString() + "&Nombre=" + Request["Nombre"].ToString() + "&Servicio=" + Request["Servicio"].ToString() +
    "&Area=" + Request["Area"].ToString() + "&Orden=" + Request["Orden"].ToString() + "&idEfector=" + Request["idEfector"].ToString();
            Response.Redirect("ItemEdit2.aspx?id=" + Request["id"].ToString() + m_parametroFiltro, true);


        }




        protected void btnAgregarTitulo_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //  AgregarItem("T");
                MostrarDatosDiagrama();
            }
        }



        private void MostrarInforme()
        {
            oCr.Report.FileName = "../Informes/Diagrama.rpt";
            oCr.ReportDocument.SetDataSource(GetDataSet());
            oCr.DataBind();
            oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Diagrama.pdf");



        }

        private DataTable GetDataSet()
        {

            string m_strSQL = " SELECT P.nombre AS practica, D.titulo AS nombre," +
                              " CASE WHEN I.idCategoria = 1 THEN 'Si' ELSE 'No' END AS esTitulo " +
                              " FROM LAB_PracticaDeterminacion AS D " +
                              " INNER JOIN LAB_Item AS P ON D.idItemPractica = P.idItem " +
                              " INNER JOIN lAB_iTEM AS i ON I.IDITEM= d.IDITEMDETERMINACION " +
                              " WHERE D.iditempractica=" + Request["id"].ToString() +
                              " ORDER BY D.idPracticaDeterminacion"; // SE PONE EL ORDEN EN QUE SE FUE AGREGANDO

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];

        }



        protected void ddlItemDiagrama_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItemDiagrama.SelectedValue != "0")
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItemDiagrama.SelectedValue));
                if (oItem != null)
                {
                    //    ddlItem.SelectedValue = oItem.IdItem.ToString();
                    txtCodigoDiagrama.Text = oItem.Codigo;
                    txtNombreDiagrama.Text = oItem.Descripcion;
                }

                txtCodigoDiagrama.UpdateAfterCallBack = true;
                txtNombreDiagrama.UpdateAfterCallBack = true;
            }
        }

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            MostrarInforme();
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ItemList.aspx", false);
        }

        /// <summary>
        /// *************************************************find e Diagrama*****************************************************///
        /// </summary>
        /// 

        ///-******************************************Inicio Valores de REferencia **************************************************///

        private void MostrarDatosValoresReferencia()
        {
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
            //lblItem.Text = oItem.Codigo + " - " + oItem.Nombre;
            lblDecimales.Text = oItem.FormatoDecimal.ToString();
            lblDecimales.Visible = false;


            HabilitarControlFormatoVR();

            CargarGrillaVR();
            CargarGrillaVR_NoPac();


        }





        private void HabilitarControlFormatoVR()
        {
            string expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
            switch (lblDecimales.Text)
            {
                case "0": /// entero
                    {
                        expresionControlDecimales = "[-+]?\\d*";
                        revValorMinimo.Text = "Sólo se admite numero entero";
                        revValorMaximo.Text = "Sólo se admite numero entero";
                        revValorMinimo.ErrorMessage = "Valor mínimo sólo admite numero entero";
                        revValorMaximo.ErrorMessage = "Valor máximo sólo admite numero entero";
                    }
                    break;
                case "1":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,1}";
                        revValorMinimo.Text = "Verifique formato #.#";
                        revValorMaximo.Text = "Verifique formato #.#";
                        revValorMinimo.ErrorMessage = "Valor mínimo sólo admite numero con formato #.#";
                        revValorMaximo.ErrorMessage = "Valor máximo sólo admite numero con formato #.#";
                    }
                    break;
                case "2":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
                        revValorMinimo.Text = "Verifique formato #.##";
                        revValorMaximo.Text = "Verifique formato #.##";
                        revValorMinimo.ErrorMessage = "Valor mínimo sólo admite numero con formato #.##";
                        revValorMaximo.ErrorMessage = "Valor máximo sólo admite numero con formato #.##";
                    }
                    break;
                case "3":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,3}";
                        revValorMinimo.Text = "Verifique formato #.###";
                        revValorMaximo.Text = "Verifique formato #.###";
                        revValorMinimo.ErrorMessage = "Valor mínimo sólo admite numero con formato #.###";
                        revValorMaximo.ErrorMessage = "Valor máximo sólo admite numero con formato #.###";
                    }
                    break;
                case "4":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,4}";
                        revValorMinimo.Text = "Verifique formato #.####";
                        revValorMaximo.Text = "Verifique formato #.####";
                        revValorMinimo.ErrorMessage = "Valor mínimo sólo admite numero con formato #.####";
                        revValorMaximo.ErrorMessage = "Valor máximo sólo admite numero con formato #.####";
                    }
                    break;
            }

            revValorMinimo.ValidationExpression = expresionControlDecimales;
            revValorMaximo.ValidationExpression = expresionControlDecimales;
            revValorMinimo.UpdateAfterCallBack = true;
            revValorMaximo.UpdateAfterCallBack = true;
        }

        protected void rdbRango_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbRango.Items[0].Selected)
            {
                txtValorMinimo.Enabled = true;
                txtValorMaximo.Enabled = true;

            }
            if (rdbRango.Items[1].Selected)
            {
                txtValorMinimo.Enabled = true;
                txtValorMaximo.Enabled = false;
                //txtValorMaximo.BackColor = Color.Gainsboro;
                txtValorMaximo.Text = "";
            }
            if (rdbRango.Items[2].Selected)
            {
                txtValorMaximo.Enabled = true;
                txtValorMinimo.Enabled = false;
                txtValorMinimo.Text = "";
            }

            if (rdbRango.Items[3].Selected)
            {
                txtValorMinimo.Enabled = false;
                txtValorMaximo.Enabled = false;
                txtValorMinimo.Text = "";
                txtValorMaximo.Text = "";
            }
            else
                HabilitarControlFormato();


            txtValorMinimo.UpdateAfterCallBack = true;
            txtValorMaximo.UpdateAfterCallBack = true;


        }

        protected void cvValores_ServerValidate(object source, ServerValidateEventArgs args)
        {

            if (rdbRango.Items[0].Selected)  //Rango
            {
                cvValores.ErrorMessage = "Debe ingresar un valor minimo y un valor maximo";
                if ((txtValorMaximoVR.Text == "") && (txtValorMinimoVR.Text == ""))
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }
            if (rdbRango.Items[1].Selected) // Limite Inferior
            {
                cvValores.ErrorMessage = "Debe ingresar un valor minimo";
                if (txtValorMinimoVR.Text == "")
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }
            if (rdbRango.Items[2].Selected) //Limite Superior
            {
                cvValores.ErrorMessage = "Debe ingresar un valor máximo";
                if (txtValorMaximoVR.Text == "")
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }

            if (rdbRango.Items[3].Selected)///solo observaciones
            {
                cvValores.ErrorMessage = "Debe ingresar una observación";
                if (txtObservaciones.Text == "")
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }

        }

        protected void gvListaVR_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                EliminarItemVR(int.Parse(e.CommandArgument.ToString()));
                CargarGrillaVR();
                //SetSelectedTab(TabIndex.TWO);
            }
        }

        private void CargarGrillaVR()
        {
            gvListaVR.AutoGenerateColumns = false;
            gvListaVR.DataSource = LeerDatosVR("P");
            gvListaVR.DataBind();
            gvListaVR.UpdateAfterCallBack = true;
        }

        private void CargarGrillaVR_NoPac()
        {
            gvListaVRNP.AutoGenerateColumns = false;
            gvListaVRNP.DataSource = LeerDatosVR("NP");
            gvListaVRNP.DataBind();
        }

        private object LeerDatosVR(string tipo)
        {
            string m_strSQL = " SELECT VR.idValorReferencia, VR.sexo, CONVERT(varchar(5), VR.edadDesde) + '-' + CONVERT(varchar(5), VR.edadHasta) AS edad, " +
                              " CASE VR.unidadEdad when 0 then 'Años' when 1 then 'Meses' when 2 then 'Dias' end as unidadEdad, M.nombre AS metodo,  " +
                              " CASE WHEN tipovalor = 0 OR tipovalor = 1 THEN  " +
                              " CASE I.formatoDecimal " +
                              " WHEN 0 THEN cast (CAST(vr.valorminimo AS int) as varchar) " +
                              " WHEN 1 THEN cast(CAST(vr.valorminimo AS decimal(18, 1)) as varchar)" +
                              " WHEN 2 THEN cast(CAST(vr.valorminimo AS decimal(18, 2)) as varchar) " +
                              " WHEN 3 THEN cast(CAST(vr.valorminimo AS decimal(18, 3)) as varchar) " +
                              " WHEN 4 THEN cast(CAST(vr.valorminimo AS decimal(18, 4)) as varchar) " +
                              " END ELSE ' - ' END AS minimo, " +
                              " CASE WHEN tipovalor = 0 OR  tipovalor = 2 THEN  " +
                              " CASE I.formatoDecimal " +
                              " WHEN 0 THEN cast (CAST(vr.valormaximo AS int) as varchar) " +
                              " WHEN 1 THEN cast(CAST(vr.valormaximo AS decimal(18, 1)) as varchar) " +
                              " WHEN 2 THEN cast(CAST(vr.valormaximo AS decimal(18, 2)) as varchar) " +
                              " WHEN 3 THEN cast(CAST(vr.valormaximo AS decimal(18, 3)) as varchar) " +
                              " WHEN 4 THEN cast(CAST(vr.valormaximo AS decimal(18, 4)) as varchar) " +
                              " END ELSE ' - ' END AS maximo, VR.observacion, P.presentacion+'-'+ME.nombre  as presentacion " +
                              " FROM  LAB_ValorReferencia AS VR " +
                              " INNER JOIN  LAB_Item AS I ON VR.idItem = I.idItem " +
                              " LEFT OUTER JOIN  LAB_Metodo AS M ON VR.idMetodo = M.idMetodo" +
                              " LEFT OUTER JOIN  LAB_ItemPresentacion AS P ON P.idItemPresentacion = VR.idPresentacion " +
                                " LEFT OUTER JOIN  LAB_MarcaEquipo ME  ON P.idMarca = ME.idMarcaEquipo " +
                              " where VR.iditem=" + Request["id"].ToString() + " and VR.idEfector=" + Request["idEfector"].ToString();

            if (tipo == "NP") ///no paciente

                m_strSQL = @" SELECT VR.idValorRefNoPaciente as idValorReferencia,  M.nombre AS metodo,  
                               CASE WHEN tipovalor = 0 OR tipovalor = 1 THEN  
                               CASE I.formatoDecimal 
                                WHEN 0 THEN cast (CAST(vr.valorminimo AS int) as varchar) 
                               WHEN 1 THEN cast(CAST(vr.valorminimo AS decimal(18, 1)) as varchar)
                               WHEN 2 THEN cast(CAST(vr.valorminimo AS decimal(18, 2)) as varchar) 
                               WHEN 3 THEN cast(CAST(vr.valorminimo AS decimal(18, 3)) as varchar) 
                               WHEN 4 THEN cast(CAST(vr.valorminimo AS decimal(18, 4)) as varchar) 
                               END ELSE ' - ' END AS minimo, 
                               CASE WHEN tipovalor = 0 OR  tipovalor = 2 THEN  
                               CASE I.formatoDecimal 
                               WHEN 0 THEN cast (CAST(vr.valormaximo AS int) as varchar) 
                               WHEN 1 THEN cast(CAST(vr.valormaximo AS decimal(18, 1)) as varchar) 
                               WHEN 2 THEN cast(CAST(vr.valormaximo AS decimal(18, 2)) as varchar) 
                               WHEN 3 THEN cast(CAST(vr.valormaximo AS decimal(18, 3)) as varchar) 
                               WHEN 4 THEN cast(CAST(vr.valormaximo AS decimal(18, 4)) as varchar) 
                               END ELSE ' - ' END AS maximo, VR.observacion , P.presentacion+'-'+ME.nombre  as presentacion
                               FROM dbo.LAB_ValorRefNoPaciente AS VR with (nolock)
                               INNER JOIN  dbo.LAB_Item AS I (nolock) ON VR.idItem = I.idItem 
                               LEFT OUTER JOIN dbo.LAB_Metodo AS M (nolock) ON VR.idMetodo = M.idMetodo
                                LEFT OUTER JOIN LAB_ItemPresentacion AS P (nolock) ON P.idItemPresentacion = VR.idPresentacion 
                                LEFT OUTER JOIN  LAB_MarcaEquipo ME  ON P.idMarca = ME.idMarcaEquipo 
                               where VR.iditem=" + Request["id"].ToString() + " and VR.idEfector=" + Request["idEfector"].ToString();
            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            if ((tipo != "NP") && (Ds.Tables[0].Rows.Count == 0) && (btnAplicarEfectores.Visible))
                btnAplicarEfectores.Visible = false;

            if ((tipo == "NP") && (Ds.Tables[0].Rows.Count == 0) && (btnAplicarEfectoresNP.Visible))
                btnAplicarEfectoresNP.Visible = false;

            return Ds.Tables[0];
        }

        private void EliminarItemVR(int idValorReferencia)
        {


            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            Item oItemPrincipal = new Item();
            oItemPrincipal = (Item)oItemPrincipal.Get(typeof(Item), int.Parse(Request["id"].ToString()));



            //ValorReferencia oRegistro = new ValorReferencia();
            //    oRegistro = (ValorReferencia)oRegistro.Get(typeof(ValorReferencia), "IdValorReferencia", idValorReferencia, "IdItem", oItemPrincipal, "IdEfector", oEfector);
            //    oRegistro.Delete();

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ValorReferencia));
            crit.Add(Expression.Eq("IdValorReferencia", idValorReferencia));
            crit.Add(Expression.Eq("IdItem", oItemPrincipal));
            crit.Add(Expression.Eq("IdEfector", oEfector));

            IList lista = crit.List();
            string valor = "";
            foreach (ValorReferencia oRegistro in lista) // deberia devolver uno solo
            {
                valor = oRegistro.IdEfector.Nombre + "-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() + "-EH:" + oRegistro.EdadHasta.ToString() + "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();

                oRegistro.Delete();
            }


            oItemPrincipal.GrabarAuditoriaDetalleItem("Elimina", oUser, "VR", valor, "");

        }

        private void EliminarItemVR_NP(int idValorReferencia)
        {


            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            Item oItemPrincipal = new Item();
            oItemPrincipal = (Item)oItemPrincipal.Get(typeof(Item), int.Parse(Request["id"].ToString()));



            //ValorReferencia oRegistro = new ValorReferencia();
            //    oRegistro = (ValorReferencia)oRegistro.Get(typeof(ValorReferencia), "IdValorReferencia", idValorReferencia, "IdItem", oItemPrincipal, "IdEfector", oEfector);
            //    oRegistro.Delete();

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ValorReferenciaNoPac));
            crit.Add(Expression.Eq("IdValorRefNoPaciente", idValorReferencia));
            crit.Add(Expression.Eq("IdItem", oItemPrincipal));
            crit.Add(Expression.Eq("IdEfector", oEfector));

            IList lista = crit.List();
            string valor = "";
            foreach (ValorReferenciaNoPac oRegistro in lista) // deberia devolver uno solo
            {
                valor = oRegistro.IdEfector.Nombre +
                    // "-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() + "-EH:" + oRegistro.EdadHasta.ToString() +
                    "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();

                oRegistro.Delete();
            }


            oItemPrincipal.GrabarAuditoriaDetalleItem("Elimina", oUser, "VR_NP", valor, "");

        }

        protected void gvListaVR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[7].Controls[1];
                CmdEliminar.CommandArgument = this.gvListaVR.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                }
            }
        }

        private void GuardarVR()
        {
            ValorReferencia oRegistro = new ValorReferencia();
            //            Efector oEfector = new Efector();
            Usuario oUser = new Usuario();
            Item oItem = new Item();

            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            oRegistro.IdEfector = oEfector;
            oRegistro.IdItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString())); ;
            oRegistro.Sexo = ddlSexo.SelectedValue;
            oRegistro.TodasEdades = true;
            oRegistro.EdadDesde = int.Parse(txtEdadDesde.Value);
            oRegistro.EdadHasta = int.Parse(txtEdadHasta.Value);
            oRegistro.UnidadEdad = int.Parse(ddlUnidadEdad.SelectedValue);
            oRegistro.IdMetodo = int.Parse(ddlMetodo.SelectedValue);

            if (rdbRango.Items[0].Selected) oRegistro.TipoValor = 0;
            if (rdbRango.Items[1].Selected) oRegistro.TipoValor = 1;
            if (rdbRango.Items[2].Selected) oRegistro.TipoValor = 2;
            if (rdbRango.Items[3].Selected) oRegistro.TipoValor = 3;

            if (txtValorMinimoVR.Text != "") oRegistro.ValorMinimo = decimal.Parse(txtValorMinimoVR.Text, System.Globalization.CultureInfo.InvariantCulture);
            if (txtValorMaximoVR.Text != "") oRegistro.ValorMaximo = decimal.Parse(txtValorMaximoVR.Text, System.Globalization.CultureInfo.InvariantCulture);

            oRegistro.Observacion = this.txtObservaciones.Text;
            oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.IdPresentacion = int.Parse(ddlPresentacionItem.SelectedValue);
            oRegistro.Save();

            string valor = oRegistro.IdEfector.Nombre + "-" + oRegistro.Sexo + "-" + oRegistro.EdadDesde.ToString() + "-" + oRegistro.EdadHasta.ToString() + "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
            oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VR", valor, "");
            GuardarVRTodoslosEfectores(oRegistro.IdItem);

        }


        private void GuardarVR_NoPac()
        {
            ValorReferenciaNoPac oRegistro = new ValorReferenciaNoPac();
            //            Efector oEfector = new Efector();
            Usuario oUser = new Usuario();
            Item oItem = new Item();

            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            oRegistro.IdEfector = oEfector;
            oRegistro.IdItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString())); ;
            /*     oRegistro.Sexo = ddlSexo.SelectedValue;
                 oRegistro.TodasEdades = true;
                 oRegistro.EdadDesde = int.Parse(txtEdadDesde.Value);
                 oRegistro.EdadHasta = int.Parse(txtEdadHasta.Value);
                 oRegistro.UnidadEdad = int.Parse(ddlUnidadEdad.SelectedValue);*/
            oRegistro.IdMetodo = int.Parse(ddlMetodo.SelectedValue);
            oRegistro.IdPresentacion = int.Parse(ddlPresentacionItem.SelectedValue);

            if (rdbRango.Items[0].Selected) oRegistro.TipoValor = 0;
            if (rdbRango.Items[1].Selected) oRegistro.TipoValor = 1;
            if (rdbRango.Items[2].Selected) oRegistro.TipoValor = 2;
            if (rdbRango.Items[3].Selected) oRegistro.TipoValor = 3;

            if (txtValorMinimoVR.Text != "") oRegistro.ValorMinimo = decimal.Parse(txtValorMinimoVR.Text, System.Globalization.CultureInfo.InvariantCulture);
            if (txtValorMaximoVR.Text != "") oRegistro.ValorMaximo = decimal.Parse(txtValorMaximoVR.Text, System.Globalization.CultureInfo.InvariantCulture);

            oRegistro.Observacion = this.txtObservaciones.Text;
            oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;

            oRegistro.Save();

            string valor = oRegistro.IdEfector.Nombre +
                /*"-" + oRegistro.Sexo + "-" + oRegistro.EdadDesde.ToString() + "-" + oRegistro.EdadHasta.ToString() + */
                "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
            oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VR", valor, "");
            GuardarVRTodoslosEfectores_NoPac(oRegistro.IdItem);

        }
        private void GuardarVRTodoslosEfectores(Item oItem)
        {


            Efector oEfectorPrincipal = new Efector();
            oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {
                bool haydatos = olabo.hayvalorreferencia(oItem);



                if (!haydatos)
                {
                    ValorReferencia oRegistro = new ValorReferencia();
                    //            Efector oEfector = new Efector();
                    Usuario oUser = new Usuario();


                    //Efector oEfector = new Efector();
                    //oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

                    oRegistro.IdEfector = olabo.IdEfector; // oEfector;
                    oRegistro.IdItem = oItem; // (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString())); ;
                    oRegistro.Sexo = ddlSexo.SelectedValue;
                    oRegistro.TodasEdades = true;
                    oRegistro.EdadDesde = int.Parse(txtEdadDesde.Value);
                    oRegistro.EdadHasta = int.Parse(txtEdadHasta.Value);
                    oRegistro.UnidadEdad = int.Parse(ddlUnidadEdad.SelectedValue);
                    oRegistro.IdMetodo = int.Parse(ddlMetodo.SelectedValue);

                    if (rdbRango.Items[0].Selected) oRegistro.TipoValor = 0;
                    if (rdbRango.Items[1].Selected) oRegistro.TipoValor = 1;
                    if (rdbRango.Items[2].Selected) oRegistro.TipoValor = 2;
                    if (rdbRango.Items[3].Selected) oRegistro.TipoValor = 3;

                    if (txtValorMinimoVR.Text != "") oRegistro.ValorMinimo = decimal.Parse(txtValorMinimoVR.Text, System.Globalization.CultureInfo.InvariantCulture);
                    if (txtValorMaximoVR.Text != "") oRegistro.ValorMaximo = decimal.Parse(txtValorMaximoVR.Text, System.Globalization.CultureInfo.InvariantCulture);

                    oRegistro.Observacion = this.txtObservaciones.Text;
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();

                    string valor = oRegistro.IdEfector.Nombre + "-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() + "-EH:" + oRegistro.EdadHasta.ToString() + "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VR", valor, "");
                }
            }
        }



        private void GuardarVRTodoslosEfectores_NoPac(Item oItem)
        {


            Efector oEfectorPrincipal = new Efector();
            oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {
                bool haydatos = olabo.hayvalorreferencia(oItem);



                if (!haydatos)
                {
                    ValorReferenciaNoPac oRegistro = new ValorReferenciaNoPac();
                    //            Efector oEfector = new Efector();
                    Usuario oUser = new Usuario();


                    //Efector oEfector = new Efector();
                    //oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

                    oRegistro.IdEfector = olabo.IdEfector; // oEfector;
                    oRegistro.IdItem = oItem; // (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString())); ;
                                              /*    oRegistro.Sexo = ddlSexo.SelectedValue;
                                                  oRegistro.TodasEdades = true;
                                                  oRegistro.EdadDesde = int.Parse(txtEdadDesde.Value);
                                                  oRegistro.EdadHasta = int.Parse(txtEdadHasta.Value);
                                                  oRegistro.UnidadEdad = int.Parse(ddlUnidadEdad.SelectedValue);*/
                    oRegistro.IdMetodo = int.Parse(ddlMetodo.SelectedValue);

                    if (rdbRango.Items[0].Selected) oRegistro.TipoValor = 0;
                    if (rdbRango.Items[1].Selected) oRegistro.TipoValor = 1;
                    if (rdbRango.Items[2].Selected) oRegistro.TipoValor = 2;
                    if (rdbRango.Items[3].Selected) oRegistro.TipoValor = 3;

                    if (txtValorMinimoVR.Text != "") oRegistro.ValorMinimo = decimal.Parse(txtValorMinimoVR.Text, System.Globalization.CultureInfo.InvariantCulture);
                    if (txtValorMaximoVR.Text != "") oRegistro.ValorMaximo = decimal.Parse(txtValorMaximoVR.Text, System.Globalization.CultureInfo.InvariantCulture);

                    oRegistro.Observacion = this.txtObservaciones.Text;
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();

                    string valor = oRegistro.IdEfector.Nombre + /*"-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() + "-EH:" + oRegistro.EdadHasta.ToString()                        +*/
                        "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VRNP", valor, "");
                }
            }
        }


        private void ReplicarVRTodoslosEfectores(Item oItem)
        {
            Efector oEfectorPrincipal = new Efector();
            oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {

                ResultadoItem oDetalle = new ResultadoItem();

                ICriteria crit2 = m_session.CreateCriteria(typeof(ValorReferencia));
                crit2.Add(Expression.Eq("IdItem", oItem));
                crit2.Add(Expression.Eq("IdEfector", olabo.IdEfector));

                // borra si existiesen
                IList items2 = crit2.List();

                foreach (ValorReferencia oDet in items2)
                {
                    oDet.Delete();
                }

                //copia los resultados configurados en nivel central==> oEfectorPrincipal

                ICriteria critNew = m_session.CreateCriteria(typeof(ValorReferencia));
                critNew.Add(Expression.Eq("IdItem", oItem));
                critNew.Add(Expression.Eq("IdEfector", oEfectorPrincipal));

                //   string sDatos = "";
                IList itemsNew = critNew.List();

                foreach (ValorReferencia oDetNew in itemsNew)
                {

                    ValorReferencia oRegistro = new ValorReferencia();

                    oRegistro.IdEfector = olabo.IdEfector; // oEfector;
                    oRegistro.IdItem = oItem;
                    oRegistro.Sexo = oDetNew.Sexo;
                    oRegistro.TodasEdades = oDetNew.TodasEdades;
                    oRegistro.EdadDesde = oDetNew.EdadDesde;
                    oRegistro.EdadHasta = oDetNew.EdadHasta;
                    oRegistro.UnidadEdad = oDetNew.UnidadEdad;
                    oRegistro.IdMetodo = oDetNew.IdMetodo;

                    oRegistro.TipoValor = oDetNew.TipoValor;
                    oRegistro.ValorMinimo = oDetNew.ValorMinimo;
                    oRegistro.ValorMaximo = oDetNew.ValorMaximo;

                    oRegistro.Observacion = oDetNew.Observacion;
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.IdPresentacion = oDetNew.IdPresentacion;
                    oRegistro.Save();

                    string valor = oRegistro.IdEfector.Nombre + "-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() + "-EH:" + oRegistro.EdadHasta.ToString() + "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VR", valor, "");


                }

            }
        }

        private void ReplicarVRTodoslosEfectores_NP(Item oItem)
        {
            Efector oEfectorPrincipal = new Efector();
            oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {

                ResultadoItem oDetalle = new ResultadoItem();

                ICriteria crit2 = m_session.CreateCriteria(typeof(ValorReferenciaNoPac));
                crit2.Add(Expression.Eq("IdItem", oItem));
                crit2.Add(Expression.Eq("IdEfector", olabo.IdEfector));

                // borra si existiesen
                IList items2 = crit2.List();

                foreach (ValorReferenciaNoPac oDet in items2)
                {
                    oDet.Delete();
                }

                //copia los resultados configurados en nivel central==> oEfectorPrincipal

                ICriteria critNew = m_session.CreateCriteria(typeof(ValorReferenciaNoPac));
                critNew.Add(Expression.Eq("IdItem", oItem));
                critNew.Add(Expression.Eq("IdEfector", oEfectorPrincipal));

                //   string sDatos = "";
                IList itemsNew = critNew.List();

                foreach (ValorReferenciaNoPac oDetNew in itemsNew)
                {

                    ValorReferenciaNoPac oRegistro = new ValorReferenciaNoPac();

                    oRegistro.IdEfector = olabo.IdEfector; // oEfector;
                    oRegistro.IdItem = oItem;
                    //oRegistro.Sexo = oDetNew.Sexo;
                    //oRegistro.TodasEdades = oDetNew.TodasEdades;
                    //oRegistro.EdadDesde = oDetNew.EdadDesde;
                    //oRegistro.EdadHasta = oDetNew.EdadHasta;
                    //oRegistro.UnidadEdad = oDetNew.UnidadEdad;
                    oRegistro.IdMetodo = oDetNew.IdMetodo;

                    oRegistro.TipoValor = oDetNew.TipoValor;
                    oRegistro.ValorMinimo = oDetNew.ValorMinimo;
                    oRegistro.ValorMaximo = oDetNew.ValorMaximo;
                    oRegistro.IdPresentacion = oDetNew.IdPresentacion;
                    oRegistro.Observacion = oDetNew.Observacion;
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();

                    string valor = oRegistro.IdEfector.Nombre +
                        //"-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() +
                        //"-EH:" + oRegistro.EdadHasta.ToString() +
                        "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VR NP", valor, "");


                }

            }
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////Fin de Valores de Referencia /*******************************************/
        /// </summary>

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
                            btGuardarVR.Visible = false;
                            // btnAgregarItemDiagrama.Visible = false;
                            btnAgregarRecomendacion.Visible = false;
                            btnGuardarRP.Visible = false;
                            btnGuardarRPDefecto.Visible = false;
                            btnNuevo.Visible = false;
                        }
                        break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }


        private void MostrarDatos(Item oItem)
        {
            // Muestra datos del item principal
           
            //Item oRegistro = new Item();
            //oRegistro = (Item)oRegistro.Get(typeof(Item), int.Parse(Request["id"].ToString()));
            lblItemVR.Text = oItem.Codigo + " - " + oItem.Nombre;
            lblItemDiagrama.Text = oItem.Codigo + " - " + oItem.Nombre;
            lblItemRP.Text = oItem.Codigo + " - " + oItem.Nombre;
            lblItemRecomendacion.Text = oItem.Codigo + " - " + oItem.Nombre;
            lblItemHiv.Text = oItem.Codigo + " - " + oItem.Nombre;

            txtCodigo.Text = oItem.Codigo.ToUpper();
            txtNombre.Text = oItem.Nombre;
            txtDescripcion.Text = oItem.Descripcion;
            ddlServicio.SelectedValue = oItem.IdArea.IdTipoServicio.IdTipoServicio.ToString();
            CargarArea(); ddlArea.SelectedValue = oItem.IdArea.IdArea.ToString();

            if (oItem.IdArea.IdTipoServicio.IdTipoServicio == 3)
            {
                //   tabMuestra.Visible = true;
                btnAgregarMuestra.Visible = true;
                

            }
            else
            {
                //    tabMuestra.Visible = false;
                btnAgregarMuestra.Visible = false;
                
            }

            if (oItem.Tipo == "P") rdbTipo.Items[0].Selected = true;
            else rdbTipo.Items[1].Selected = true;

            //      txtRecomendaciones.Text=oItem.Recomendacion;

            if (Request["idEfector"].ToString() == "227")
            {
                if (oItem.IdEfectorDerivacion != oItem.IdEfector)
                {
                    ddlDerivable.SelectedValue = "1";
                    HabilitarDerivador();
                    ddlEfector.SelectedValue = oItem.IdEfectorDerivacion.IdEfector.ToString();
                }
                else ddlDerivable.SelectedValue = "0";
            }
            else
            {
                ItemEfector oItemEfector = new ItemEfector();
                oItemEfector = (ItemEfector)oItemEfector.Get(typeof(ItemEfector), "IdItem", oItem, "IdEfector", oUser.IdEfector);
                if (oItemEfector != null)
                {
                    if (oItemEfector.IdEfectorDerivacion != oUser.IdEfector)
                    {
                        ddlDerivable.SelectedValue = "1";
                        HabilitarDerivador();
                        ddlEfector.SelectedValue = oItemEfector.IdEfectorDerivacion.IdEfector.ToString();
                    }
                    else ddlDerivable.SelectedValue = "0";
               
                if ( oItemEfector.IdItem.IdTipoResultado <3 )  //numerico o texto (1,2)
                txtValorDefecto.Text = oItemEfector.ResultadoDefecto;
                }
            }

            if (oItem.RequiereMuestra == "S")
            {
                rdbRequiereMuestra.Items[0].Selected = true;
                rdbRequiereMuestra.Items[1].Selected = false;
            }
            else
            {
                rdbRequiereMuestra.Items[0].Selected = false;
                rdbRequiereMuestra.Items[1].Selected = true;
            }

            ddlUnidadMedida.SelectedValue = oItem.IdUnidadMedida.ToString();

            ddlTipoMuestra.SelectedValue = oItem.IdMuestra.ToString();
            if (oItem.IdCategoria == 0) rdbCategoria.Items[0].Selected = true;
            if (oItem.IdCategoria == 1) rdbCategoria.Items[1].Selected = true;

            HabilitarTipoResultados();
            ddlTipoResultado.SelectedValue = oItem.IdTipoResultado.ToString();
            HabilitarValorDefecto();
            ddlDecimales.SelectedValue = oItem.FormatoDecimal.ToString();
            ddlMultiplicador.SelectedValue = oItem.Multiplicador.ToString();
            HabilitarDecimales();

            if ((oItem.IdTipoResultado == 1) || (oItem.IdTipoResultado == 2))
                txtValorDefecto.Text = oItem.ResultadoDefecto;
            else
            {
                txtValorDefecto.Text = "";
                txtValorDefecto.Enabled = false;
            }
            

            if ((oItem.ValorMinimo != -1) && (oItem.ValorMaximo != -1))
            {
                string formato = Formato(ddlDecimales.SelectedValue);
                /*txtValorMinimo.Text = oItem.ValorMinimo.ToString(System.Globalization.CultureInfo.InvariantCulture);
                txtValorMaximo.Text = oItem.ValorMaximo.ToString(System.Globalization.CultureInfo.InvariantCulture);*/
                decimal v1 = decimal.Parse(oItem.ValorMinimo.ToString(formato));
                decimal v2 = decimal.Parse(oItem.ValorMaximo.ToString(formato));
                txtValorMinimo.Text = v1.ToString(System.Globalization.CultureInfo.InvariantCulture);
                txtValorMaximo.Text = v2.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            HabilitarControlFormato();

            txtDuracion.Value = oItem.Duracion.ToString();

            ddlItemReferencia.SelectedValue = oItem.IdItemReferencia.ToString(); MostrarCodigoItemReferencia();




            txtCodigoNomenclador.Text = oItem.CodigoNomenclador;
            MostrarItemNomenclador();

            if (oItem.IdTipoResultado == 0)  /// compuesta
            {
                pnlVR.Enabled = false;
                pnlDiagrama.Enabled = true;
                //    pnlVerDiagrama.Enabled = true;
                pnlPredefinidos.Enabled = false;
            }
            else
            {
                pnlVR.Enabled = true;
                pnlDiagrama.Enabled = false;
                // pnlVerDiagrama.Enabled = false;
                if ((oItem.IdTipoResultado == 3) || (oItem.IdTipoResultado == 4))  ///resultados predefinidos (selección simple o multiple)
                    pnlPredefinidos.Enabled = true;
                else
                    pnlPredefinidos.Enabled = false;
            }
            if (oItem.Tipo == "P") /// solo si el practica
                pnlRecomendacion.Enabled = true;
            else
                pnlRecomendacion.Enabled = false;



            ///codifica HIV: Solapa Mas Opciones
            chkCodificaHiv.Checked = oItem.CodificaHiv;
            txtLimite.Value = oItem.LimiteTurnosDia.ToString();
            chkEtiquetaAdicional.Checked = oItem.EtiquetaAdicional;
            ///
            /////screening neonatal
            //  chkIsScreening.Checked=oItem.IsScreeening ;


            if (oItem.Disponible) ddlDisponible.SelectedValue = "1";
            else ddlDisponible.SelectedValue = "0";


            if (oItem.Informable) ddlInformable.SelectedValue = "1";
            else ddlInformable.SelectedValue = "0";

            if (oItem.RequiereCaracter) ddlCaracter.SelectedValue = "1";
            else ddlCaracter.SelectedValue = "0";

            pnlVR.UpdateAfterCallBack = true;
            pnlDiagrama.UpdateAfterCallBack = true;
            //   pnlVerDiagrama.UpdateAfterCallBack = true;
            pnlPredefinidos.UpdateAfterCallBack = true;
            pnlRecomendacion.UpdateAfterCallBack = true;
            txtValorDefecto.UpdateAfterCallBack = true;

            chkImprimeMuestra.Checked = oItem.ImprimeMuestra;

            lnkRegresar.Visible = true;

            lblSininsumo.Text = "No aplica nivel Central";
            CargarGrillaPresentaciones();
            CargarGrillaMuestras();

            CargarGrillaAutoanalizadores();

        }

       
        private void CargarGrillaAutoanalizadores()
        {
            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;          

            cmd.CommandText = "lab_GetEquipoItem";           

            cmd.Parameters.Add("@idItemBuscar", SqlDbType.Int);
            cmd.Parameters["@idItemBuscar"].Value = Request["id"].ToString();              
            cmd.Connection = conn;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);


            gvAutoAnalizadores.DataSource = Ds.Tables[0];
            gvAutoAnalizadores.DataBind();
          ///  lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            conn.Close();
            //}
        }
        private void CargarGrillaMuestras()
        {
            gvMuestraItem.AutoGenerateColumns = false;
            gvMuestraItem.DataSource = LeerDatosMuestraItem("P");
            gvMuestraItem.DataBind();
            gvMuestraItem.UpdateAfterCallBack = true;

        }

        private object LeerDatosMuestraItem(string v)
        {
            string m_strSQL = @" select iditemMuestra, M.nombre as muestra
 from LAB_ItemMuestra I
 inner join Lab_muestra M on I.idmuestra = M.idMuestra
 where I.iditem =  " + Request["id"].ToString();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];
        }


        private void HabilitarValorDefecto()
        {
            if (ddlTipoResultado.SelectedValue == "1")
            {
                txtValorDefecto.Enabled = true;
                txtValorDefecto.Width = Unit.Pixel(76);

            }
            if (ddlTipoResultado.SelectedValue == "2")
            {
                txtValorDefecto.Enabled = true;
                txtValorDefecto.Width = Unit.Pixel(250);
            }
            txtValorDefecto.UpdateAfterCallBack = true;

        }

        private string Formato(string p)
        {
            string aux = "";
            switch (p)
            {
                case "0": aux = "0"; break;
                case "1": aux = "0.0"; break;
                case "2": aux = "0.00"; break;
                case "3": aux = "0.000"; break;
                case "4": aux = "0.0000"; break;
            }
            return aux;
        }

        private void HabilitarTipoResultados()
        {
            if (rdbCategoria.Items[0].Selected)//Simple
            {
                ddlTipoResultado.Enabled = true;
                rvTipoResultado.Enabled = true;
                ddlDecimales.Enabled = true;
                txtValorMinimo.Enabled = true;
                txtValorMaximo.Enabled = true;
            }

            else //Compuesta
            {
                ddlTipoResultado.SelectedValue = "0";
                ddlTipoResultado.Enabled = false;
                ddlDecimales.Enabled = false;
                rvTipoResultado.Enabled = false;
                txtValorMinimo.Text = "";
                txtValorMaximo.Text = "";
                txtValorMinimo.Enabled = false;
                txtValorMaximo.Enabled = false;
            }
            ddlTipoResultado.UpdateAfterCallBack = true;
            ddlDecimales.UpdateAfterCallBack = true;
            rvTipoResultado.UpdateAfterCallBack = true;
            txtValorMinimo.UpdateAfterCallBack = true;
            txtValorMaximo.UpdateAfterCallBack = true;

        }


        private void CargarListas()
        {
            pnlVR.Enabled = false;
            pnlDiagrama.Enabled = false;
            //    pnlVerDiagrama.Enabled = false;
            pnlPredefinidos.Enabled = false;
            pnlRecomendacion.Enabled = false;

            pnlVR.UpdateAfterCallBack = true;
            pnlDiagrama.UpdateAfterCallBack = true;
            //   pnlVerDiagrama.UpdateAfterCallBack = true;
            pnlPredefinidos.UpdateAfterCallBack = true;
            pnlRecomendacion.UpdateAfterCallBack = true;

            Utility oUtil = new Utility();
            ///Carga de combos de Areas
            ///
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio WHERE idtiposervicio<>5 and  (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre");
            CargarArea();



            m_ssql = @"select distinct E.nombre, E.nombre+'-' +convert(varchar,E.idEfector)  as idEfector from sys_efector E  
                  INNER JOIN lab_Configuracion C on C.idEfector<>E.idEfector  
                order by E.nombre";



            oUtil.CargarCombo(ddlEfectorItemDeriva, m_ssql, "idEfector", "nombre");
            ddlEfectorItemDeriva.Items.Insert(0, new ListItem("Seleccione Efector", "0"));
            ddlEfectorItemDeriva.Enabled = true;
            ddlEfectorItemDeriva.UpdateAfterCallBack = true;


            ///Carga de combos de Unidad de Medida
            m_ssql = "SELECT idUnidadMedida, nombre FROM LAB_UnidadMedida where baja=0 order by nombre";
            oUtil.CargarCombo(ddlUnidadMedida, m_ssql, "idUnidadMedida", "nombre");
            ddlUnidadMedida.Items.Insert(0, new ListItem("No Aplica", "0"));


            ///Carga de combos de Item de Referencia
            m_ssql = "SELECT I.idItem, I.nombre FROM LAB_Item I " +
                "INNER JOIN lab_AREA A ON I.idArea=A.idArea " +
                "where A.baja=0 and I.baja=0";
            oUtil.CargarCombo(ddlItemReferencia, m_ssql, "idItem", "nombre");
            ddlItemReferencia.Items.Insert(0, new ListItem("No Aplica", "0"));


            ///Carga de combos del Nomenclador
            m_ssql = "SELECT capitulo as codigo, descripcion as descrip FROM LAB_Nomenclador order by descripcion";
            oUtil.CargarCombo(ddlItemNomenclador, m_ssql, "codigo", "descrip");
            ddlItemNomenclador.Items.Insert(0, new ListItem("No Aplica", "0"));

            //Utility oUtil = new Utility();
            /////Carga de combos de Areas
            m_ssql = "select idMetodo, nombre from Lab_metodo  where baja=0 order by nombre";
            oUtil.CargarCombo(ddlMetodo, m_ssql, "idMetodo", "nombre");
            ddlMetodo.Items.Insert(0, new ListItem("No Aplica", "0"));

            if (Request["id"] != null)
            {
                m_ssql =
                    " SELECT idItem, nombre FROM LAB_Item AS I " +
" WHERE     (baja = 0) AND (idItem <> " + Request["id"].ToString() + ") " +
" AND (idArea IN  (SELECT a.idArea   FROM LAB_Area AS a INNER JOIN LAB_Item AS I ON I.idArea = a.idArea" +
                            " WHERE      (I.idItem = " + Request["id"].ToString() + ")))ORDER BY nombre";


                //"select I.idItem, I.nombre from Lab_item I" +
                //    " INNER join LAB_Area A on A.idArea= I.idarea "+
                //    " where I.baja=0 and I.idItem<>" + Request["id"].ToString() + " order by I.nombre";
                oUtil.CargarCombo(ddlItemDiagrama, m_ssql, "idItem", "nombre");
                ddlItemDiagrama.Items.Insert(0, new ListItem("Seleccione Item a agregar", "0"));
            }


            m_ssql = "select idRecomendacion, nombre  from Lab_Recomendacion where baja = 0 order by nombre";
            oUtil.CargarCombo(ddlRecomendacion, m_ssql, "idRecomendacion", "nombre");
            ddlRecomendacion.Items.Insert(0, new ListItem("Seleccione Recomendacion", "0"));


            txtLimite.Value = "0";
            //     lnkRegresar.Visible = false;


            ///Carga de combos de Tipo de muestra---Ver como nombrar
            m_ssql = "select idMuestra, codigo + '-' + nombre as nombre from lab_muestra where baja=0  order by nombre ";
            oUtil.CargarCombo(ddlTipoMuestra, m_ssql, "idMuestra", "nombre");
            ddlTipoMuestra.Items.Insert(0, new ListItem("No Aplica", "0"));

            oUtil.CargarCombo(ddlMuestra1, m_ssql, "idMuestra", "nombre");
            ddlMuestra1.Items.Insert(0, new ListItem("Seleccione", "0"));


            ///Carga de combos de ddlMarca
            m_ssql = "SELECT idMarcaEquipo, nombre FROM LAB_MArcaEquipo order by nombre";
            oUtil.CargarCombo(ddlMarca, m_ssql, "idMarcaEquipo", "nombre");
            ddlMarca.Items.Insert(0, new ListItem("No Aplica", "0"));
            if (Request["id"] != null)
            {
                ///Carga de combos de ddlPresentacionItem
                m_ssql = @"SELECT iditempresentacion, codigo + '-' + presentacion as nombre FROM LAB_ItemPresentacion 
                    where   baja=0 and iditem=" + Request["id"].ToString() + " order by nombre";
                oUtil.CargarCombo(ddlPresentacionItem, m_ssql, "iditempresentacion", "nombre");
                ddlPresentacionItem.Items.Insert(0, new ListItem("No Aplica", "0"));

                oUtil.CargarCombo(ddlPresentacionEfector, m_ssql, "iditempresentacion", "nombre");
                ddlPresentacionEfector.Items.Insert(0, new ListItem("Seleccione", "0"));

                oUtil.CargarCombo(ddlPresentacionEfectorDefecto, m_ssql, "iditempresentacion", "nombre");




            }


            m_ssql = null;
            oUtil = null;
        }

        private void CargarArea()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select idArea, nombre from Lab_Area where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");
            ddlArea.Items.Insert(0, new ListItem("Seleccione Area", "0"));
            ddlArea.UpdateAfterCallBack = true;

        }
        protected void btnGuardarVR_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ddlTipoServicio.SelectedValue == "P")
                {
                    GuardarVR();
                    CargarGrillaVR();
                }

                else
                {
                    GuardarVR_NoPac();
                    CargarGrillaVR_NoPac();
                }

                SetSelectedTab(TabIndex.TWO);

            }
        }

        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void ddlDerivable_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarDerivador();

        }

        private void HabilitarDerivador()
        {
            if (ddlDerivable.SelectedValue.ToString() == "1")//Si
                CargarEfector();
            else
            {
                ddlEfector.Enabled = false;
                rvEfector.Enabled = false;
            }
            ddlEfector.UpdateAfterCallBack = true;
            rvEfector.UpdateAfterCallBack = true;
        }

        private void CargarEfector()
        {
            Utility oUtil = new Utility();

            string m_ssql = "select E.idEfector, E.nombre from sys_efector E " +
                " INNER JOIN lab_Configuracion C on C.idEfector<>E.idEfector where C.idEfector= " + Request["idEfector"].ToString() +
                " order by E.nombre";
            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            ddlEfector.Items.Insert(0, new ListItem("Seleccione Efector", "0"));
            ddlEfector.Enabled = true;
            rvEfector.Enabled = true;


            m_ssql = null;
            oUtil = null;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (Session["idUsuario"] != null)
                {
                    string m_accion = "Alta";

                Item oReg = new Item();
                if (Request["id"] != null)
                {
                    oReg = (Item)oReg.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                    m_accion = "Modifica";
                }
                Guardar(oReg, m_accion);

                if (Request["id"] == null)  // es nueva determinacion==> debe replicar en LAB_ItemEfector
                {
                    GuardarNuevoTodosEfectores(oReg);
                }

                string m_parametroFiltro = "&Codigo=" + Request["Codigo"].ToString() + "&Nombre=" + Request["Nombre"].ToString() + "&Servicio=" + Request["Servicio"].ToString() +
"&Area=" + Request["Area"].ToString() + "&Orden=" + Request["Orden"].ToString() + "&idEfector=" + Request["idEfector"].ToString();

                if (Request["id"] != null) //Modificacion
                {
                    // Response.Redirect ("javascript:history.go(-3);");
                    //   Response.Redirect("ItemList.aspx", false);
                    string popupScript = "<script language='JavaScript'> alert('Los datos se guardaron correctamente'); </script>";
                    Page.RegisterStartupScript("PopupScript", popupScript);
                    Response.Redirect("ItemEdit2.aspx?id=" + oReg.IdItem + m_parametroFiltro, false);
                }
                else //Nuevo
                {

                        Response.Redirect("ItemEdit2.aspx?id=" + oReg.IdItem + m_parametroFiltro, false);
                    }

                }
            }
        
            else
                Response.Redirect("../FinSesion.aspx", false);
        }

        private void GuardarNuevoTodosEfectores(Item oReg)
        {

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {
                ItemEfector oRegistro = new ItemEfector();
                oRegistro.IdItem = oReg;
                oRegistro.IdEfector = olabo.IdEfector;
                oRegistro.IdEfectorDerivacion = olabo.IdEfector;
                oRegistro.Disponible = oReg.Disponible;
                oRegistro.Informable = oReg.Informable;
                oRegistro.IdUsuarioRegistro = oReg.IdUsuarioRegistro;
                oRegistro.FechaRegistro = DateTime.Now;
                oRegistro.Save();

            }

        }

        private void Guardar(Item oRegistro, string m_accion)
        {
            Area oArea = new Area();
            Efector oEfector = new Efector();
            Usuario oUser = new Usuario();

             oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));

            if (oEfector.IdEfector == 227)
            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            {
                if (txtValorMinimo.Text == "") txtValorMinimo.Text = "-1";
                if (txtValorMaximo.Text == "") txtValorMaximo.Text = "-1";

                ///auditoria
                if (oRegistro.Descripcion != txtDescripcion.Text)
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Descripcion", txtDescripcion.Text, oRegistro.Descripcion);
                if (oRegistro.IdArea.IdArea != int.Parse(ddlArea.SelectedValue))
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Area", ddlArea.SelectedItem.Text, oRegistro.IdArea.Nombre);
                if (oRegistro.IdUnidadMedida != int.Parse(ddlUnidadMedida.SelectedValue))
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Un. Medida", ddlUnidadMedida.SelectedItem.Text, "");


                //oRegistro.IdTipoResultado = int.Parse(ddlTipoResultado.SelectedValue);
                if (oRegistro.IdTipoResultado != int.Parse(ddlTipoResultado.SelectedValue))
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, " Tipo Res", ddlTipoResultado.SelectedItem.Text, oRegistro.IdTipoResultado.ToString());


                oRegistro.FormatoDecimal = int.Parse(ddlDecimales.SelectedValue);
                if (oRegistro.FormatoDecimal != int.Parse(ddlDecimales.SelectedValue))
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Formato Decimal", ddlDecimales.SelectedItem.Text, oRegistro.FormatoDecimal.ToString());

                if (oRegistro.ValorMinimo != decimal.Parse(txtValorMinimo.Text, System.Globalization.CultureInfo.InvariantCulture))
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Valor Minimo", txtValorMinimo.Text, oRegistro.ValorMinimo.ToString());
                if (oRegistro.ValorMaximo != decimal.Parse(txtValorMaximo.Text, System.Globalization.CultureInfo.InvariantCulture))
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Valor Maximo", txtValorMaximo.Text, oRegistro.ValorMinimo.ToString());



                if (oRegistro.CodigoNomenclador != txtCodigoNomenclador.Text)
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Cod. Nom.", txtCodigoNomenclador.Text, oRegistro.CodigoNomenclador);

                if (oRegistro.ImprimeMuestra != chkImprimeMuestra.Checked)
                    oRegistro.GrabarAuditoriaDetalleItem(m_accion, oUser, "Imprime Muestra", chkImprimeMuestra.Checked.ToString(), oRegistro.ImprimeMuestra.ToString());


                //fin auditoria

                oRegistro.IdEfector = oEfector; // oC.IdEfector;
                oRegistro.Codigo = txtCodigo.Text.ToUpper();
                oRegistro.Nombre = txtNombre.Text;
                oRegistro.Descripcion = txtDescripcion.Text;
                oRegistro.IdArea = (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue));

                if (rdbTipo.Items[0].Selected) oRegistro.Tipo = "P";
                else oRegistro.Tipo = "D";


                if (ddlDerivable.SelectedValue == "1")
                    oRegistro.IdEfectorDerivacion = (Efector)oEfector.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                else
                    oRegistro.IdEfectorDerivacion = oRegistro.IdEfector;

                oRegistro.RequiereMuestra = rdbRequiereMuestra.SelectedItem.Value;
                oRegistro.IdUnidadMedida = int.Parse(ddlUnidadMedida.SelectedValue);


                if (rdbCategoria.Items[0].Selected) //Simple
                {
                    oRegistro.IdCategoria = 0;
                    // EliminarDiagrama(oRegistro);
                }
                if (rdbCategoria.Items[1].Selected) oRegistro.IdCategoria = 1; //Compuesta

                oRegistro.IdTipoResultado = int.Parse(ddlTipoResultado.SelectedValue);
                oRegistro.FormatoDecimal = int.Parse(ddlDecimales.SelectedValue);

                oRegistro.ValorMinimo = decimal.Parse(txtValorMinimo.Text, System.Globalization.CultureInfo.InvariantCulture);
                oRegistro.ValorMaximo = decimal.Parse(txtValorMaximo.Text, System.Globalization.CultureInfo.InvariantCulture);

                oRegistro.Multiplicador = int.Parse(ddlMultiplicador.SelectedValue);
                oRegistro.ResultadoDefecto = txtValorDefecto.Text;
                oRegistro.LimiteTurnosDia = 0;
                //// si no es de tipo predefinido pone el idResultadoDefecto=0 dado que este campo se usa sólo para los predefinidos simples
                if (int.Parse(ddlTipoResultado.SelectedValue) != 3) oRegistro.IdResultadoPorDefecto = 0;


                oRegistro.Duracion = int.Parse(txtDuracion.Value.ToString());

                if (ddlItemReferencia.SelectedValue != "0") oRegistro.IdItemReferencia = int.Parse(ddlItemReferencia.SelectedValue);

                oRegistro.IdMuestra = int.Parse(ddlTipoMuestra.SelectedValue);

                oRegistro.IdUsuarioRegistro = oUser;
                oRegistro.FechaRegistro = DateTime.Now;

                oRegistro.CodigoNomenclador = txtCodigoNomenclador.Text;
                if (ddlDisponible.SelectedValue == "1")
                    oRegistro.Disponible = true;
                else oRegistro.Disponible = false;

                if (ddlInformable.SelectedValue == "1")
                    oRegistro.Informable = true;
                else oRegistro.Informable = false;


                if (ddlCaracter.SelectedValue == "1")
                    oRegistro.RequiereCaracter = true;
                else oRegistro.RequiereCaracter = false;


                oRegistro.ImprimeMuestra = chkImprimeMuestra.Checked;




                oRegistro.Save();
                ///Si es simple y tenia diagrama se borra el mismo
                if (oRegistro.IdCategoria == 0) EliminarDiagrama(oRegistro);
            }

            else
            {

                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(ItemEfector));
                crit.Add(Expression.Eq("IdItem", oRegistro));
                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));

                IList lista = crit.List();

                foreach (ItemEfector oReg in lista) // deberia devolver uno solo
                {     ///auditoria
                    if (ddlEfector.SelectedValue != "")
                    {
                        if (oReg.IdEfectorDerivacion.IdEfector != int.Parse(ddlEfector.SelectedValue))
                            oRegistro.GrabarAuditoriaDetalleItem("Modifica ", oUser, "Derivacion", ddlEfector.SelectedItem.Text, oReg.IdEfectorDerivacion.Nombre);
                    }
                    if ((oReg.Disponible) && (ddlDisponible.SelectedValue != "1"))
                        oRegistro.GrabarAuditoriaDetalleItem("Modifica ", oUser, "Disponible", ddlDisponible.SelectedItem.Text, oReg.Disponible.ToString());
                    if ((!oReg.Disponible) && (ddlDisponible.SelectedValue != "0"))
                        oRegistro.GrabarAuditoriaDetalleItem("Modifica ", oUser, "Disponible", ddlDisponible.SelectedItem.Text, oReg.Disponible.ToString());
                    if ((oReg.Informable) && (ddlInformable.SelectedValue != "1"))
                        oRegistro.GrabarAuditoriaDetalleItem("Modifica ", oUser, "Informable", ddlDisponible.SelectedItem.Text, oReg.Disponible.ToString());
                    if ((!oReg.Informable) && (ddlInformable.SelectedValue != "0"))
                        oRegistro.GrabarAuditoriaDetalleItem("Modifica", oUser, "Informable", ddlDisponible.SelectedItem.Text, oReg.Disponible.ToString());

                    if (oRegistro.ResultadoDefecto != txtValorDefecto.Text)
                        oRegistro.GrabarAuditoriaDetalleItem("Modifica ", oUser, "Valor Defecto", txtValorDefecto.Text, oRegistro.ResultadoDefecto);
                    
                    //fin auditoria

                    if (ddlDerivable.SelectedValue == "1")
                    {
                        Efector oEfectorDer = new Efector();
                        oReg.IdEfectorDerivacion = (Efector)oEfectorDer.Get(typeof(Efector), int.Parse(ddlEfector.SelectedValue));
                    }
                    else
                        oReg.IdEfectorDerivacion = oReg.IdEfector;

                    if (ddlDisponible.SelectedValue == "1")
                        oReg.Disponible = true;
                    else oReg.Disponible = false;

                    if (ddlInformable.SelectedValue == "1")
                        oReg.Informable = true;
                    else oReg.Informable = false;
                    oReg.IdUsuarioRegistro = oUser;//(Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oReg.FechaRegistro = DateTime.Now;
                    oReg.ResultadoDefecto = txtValorDefecto.Text;

                    oReg.Save();


                }


            }
        }

        private void EliminarDiagrama(Item oRegistro)
        {
            Efector oEfector = new Efector();
            oEfector = (Efector)oEfector.Get(typeof(Efector), int.Parse(Request["idEfector"].ToString()));


            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(PracticaDeterminacion));
            crit.Add(Expression.Eq("IdItemPractica", oRegistro));
            crit.Add(Expression.Eq("IdEfector", oEfector));

            IList lista = crit.List();

            foreach (PracticaDeterminacion oItem in lista)
            {
                oItem.Delete();
            }
            oRegistro.GrabarAuditoriaDetalleItem("Elimina", oUser, "Diagrama", "Por cambio de valor Compuesta", "");
        }


        private void EliminarVREfectores(Item oRegistro)
        {

            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));

            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {


                ICriteria crit2 = m_session.CreateCriteria(typeof(ValorReferencia));
                crit2.Add(Expression.Eq("IdItem", oRegistro));
                crit2.Add(Expression.Eq("IdEfector", olabo.IdEfector));

                // borra si existiesen
                IList items2 = crit2.List();

                foreach (ValorReferencia oDet in items2)
                {
                    oDet.Delete();
                }
                oRegistro.GrabarAuditoriaDetalleItem("Elimina", oUser, "VR", "Por cambio del Administrador", "");
            }
        }




        protected void cvCodigo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Verifica que no exista un item con el codigo ingresado

            ISession m_session = NHibernateHttpModule.CurrentSession;

            ICriteria crit = m_session.CreateCriteria(typeof(Item));

            crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
            crit.Add(Expression.Eq("Baja", false));


            IList lista = crit.List();
            if (lista.Count == 0)
                args.IsValid = true;
            else
            {

                foreach (Item oItem in lista)
                {
                    if (Request["id"] != null)
                    {
                        if (int.Parse(Request["id"].ToString()) == oItem.IdItem)
                            args.IsValid = true;
                        else
                            args.IsValid = false;
                    }
                    else
                        args.IsValid = false;
                }
            }

        }

        protected void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (txtDescripcion.Text == "")
            {
                txtDescripcion.Text = txtNombre.Text;
                txtDescripcion.UpdateAfterCallBack = true;
            }
        }

        protected void txtCodigoDiagrama_TextChanged(object sender, EventArgs e)
        {
            if (txtCodigoDiagrama.Text != "")
            {
                Item oItem = new Item();
                ISession m_session = NHibernateHttpModule.CurrentSession;

                ICriteria crit = m_session.CreateCriteria(typeof(Item));

                crit.Add(Expression.Eq("Codigo", txtCodigoDiagrama.Text));
                crit.Add(Expression.Eq("Baja", false));
                //   crit.Add(Expression.Eq("Tipo", "D"));

                oItem = (Item)crit.UniqueResult();
                if (oItem != null)
                {
                    try
                    {
                        ddlItemDiagrama.SelectedValue = oItem.IdItem.ToString();
                        txtNombreDiagrama.Text = oItem.Descripcion;
                        lblMensajeDiagrama.Text = "";
                    }
                    catch
                    {
                        lblMensajeDiagrama.Text = "El codigo ingresado no existe dentro del area";
                        ddlItemDiagrama.SelectedValue = "0";
                        txtNombreDiagrama.Text = "";
                    }
                }
                else
                {
                    lblMensajeDiagrama.Text = "El codigo ingresado no existe";
                    ddlItemDiagrama.SelectedValue = "0";
                    txtNombreDiagrama.Text = "";
                }

                ddlItemDiagrama.UpdateAfterCallBack = true;
                txtNombreDiagrama.UpdateAfterCallBack = true;
                lblMensajeDiagrama.UpdateAfterCallBack = true;
            }
        }

        protected void rdbCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarTipoResultados();
        }

        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {
            MostrarItemNomenclador();
        }

        private void MostrarItemReferencia()
        {
            ///Si encuentra el codigo ingresado muestra el item en el combo
            ///
            try
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Item));
                crit.Add(Expression.Eq("Codigo", txtCodigoReferencia.Text));
                crit.Add(Expression.Eq("Baja", false));
                //IList detalle = crit.List();

                Item oItem = (Item)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlItemReferencia.SelectedValue = oItem.IdItem.ToString();
                    // lblValorNomenclador.Text = oItem.ValMod.ToString();
                    //lblMensaje.Visible = false;
                }
                else
                {
                    ddlItemReferencia.SelectedValue = "0";
                    //lblValorNomenclador.Text = "";
                    //if (txtCodigoNomenclador.Text != "")
                    //  lblMensaje.Visible = true;
                }
            }
            catch
            {

            }
            ddlItemReferencia.UpdateAfterCallBack = true;
            //lblValorNomenclador.UpdateAfterCallBack = true;
            //lblMensaje.UpdateAfterCallBack = true;
        }

        private void MostrarItemNomenclador()
        {
            ///Si encuentra el codigo ingresado muestra el item en el combo
            ///
            try
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(Nomenclador));
                crit.Add(Expression.Eq("Codigo", txtCodigoNomenclador.Text));
                //IList detalle = crit.List();

                Nomenclador oItem = (Nomenclador)crit.UniqueResult();
                if (oItem != null)
                {
                    ddlItemNomenclador.SelectedValue = oItem.Codigo;
                    lblValorNomenclador.Text = oItem.Ug.ToString();//.ValMod.ToString();
               //     lblFactorProduccion.Text = oItem.FactorProduccion.ToString();
                    lblMensaje.Visible = false;
                }
                else
                {
                    ddlItemNomenclador.SelectedValue = "0";
                    lblValorNomenclador.Text = "";
                //    lblFactorProduccion.Text = "";
                    if (txtCodigoNomenclador.Text != "")
                        lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
            ddlItemNomenclador.UpdateAfterCallBack = true;
            lblValorNomenclador.UpdateAfterCallBack = true;
       //     lblFactorProduccion.UpdateAfterCallBack = true;
            lblMensaje.UpdateAfterCallBack = true;
        }



        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        { ///////Con la selección del item se muestra el codigo
            if (ddlItemNomenclador.SelectedValue != "0")
            {
                Nomenclador oItem = new Nomenclador();
                oItem = (Nomenclador)oItem.Get(typeof(Nomenclador), ddlItemNomenclador.SelectedValue);
                txtCodigoNomenclador.Text = oItem.Codigo;
                lblValorNomenclador.Text = oItem.Ug.ToString();
             //   lblFactorProduccion.Text = oItem.FactorProduccion.ToString();
            }
            else
            {
                txtCodigoNomenclador.Text = "";
                lblValorNomenclador.Text = "";
           //     lblFactorProduccion.Text = "";

            }
            txtCodigoNomenclador.UpdateAfterCallBack = true;
            lblValorNomenclador.UpdateAfterCallBack = true;
      //      lblFactorProduccion.UpdateAfterCallBack = true;

        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();
        }

        protected void txtCodigoReferencia_TextChanged(object sender, EventArgs e)
        {
            MostrarItemReferencia();
        }

        protected void ddlItemReferencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarCodigoItemReferencia();
        }

        private void MostrarCodigoItemReferencia()
        {
            if (ddlItemReferencia.SelectedValue != "0")
            {
                Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(ddlItemReferencia.SelectedValue));
                txtCodigoReferencia.Text = oItem.Codigo;
            }
            //lblValorNomenclador.Text = oItem.ValMod.ToString();
            txtCodigoReferencia.UpdateAfterCallBack = true;
            //lblValorNomenclador.UpdateAfterCallBack = true;
        }

        protected void ddlTipoResultado_SelectedIndexChanged(object sender, EventArgs e)
        {

            HabilitarDecimales();
        }

        private void HabilitarDecimales()
        {
            if (ddlTipoResultado.SelectedValue == "1")
            {
                ddlDecimales.Enabled = true;
                ddlMultiplicador.Enabled = true;
                txtValorMinimo.Enabled = true;
                txtValorMaximo.Enabled = true;
                txtValorDefecto.Width = Unit.Pixel(76);

            }
            else
            {
                ddlDecimales.Enabled = false;
                ddlMultiplicador.Enabled = false;
                txtValorMinimo.Text = "";
                txtValorMaximo.Text = "";
                txtValorMinimo.Enabled = false;
                txtValorMaximo.Enabled = false;
                txtValorDefecto.Width = Unit.Pixel(250);
                if (ddlTipoResultado.SelectedValue == "3")  //predefinidos
                {
                    txtValorDefecto.Enabled = false;
                }

            }

            ddlDecimales.UpdateAfterCallBack = true;
            ddlMultiplicador.UpdateAfterCallBack = true;
            txtValorMinimo.UpdateAfterCallBack = true;
            txtValorMaximo.UpdateAfterCallBack = true;
            txtValorDefecto.UpdateAfterCallBack = true;

        }

        protected void ddlDecimales_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarControlFormato();
        }

        private void HabilitarControlFormato()
        {
            string expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
            switch (ddlDecimales.SelectedValue)
            {
                case "0": /// entero
                    {
                        expresionControlDecimales = "[-+]?\\d*";
                    }
                    break;
                case "1":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,1}";
                    }
                    break;
                case "2":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,2}";
                    }
                    break;
                case "3":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,3}";
                    }
                    break;
                case "4":
                    {
                        expresionControlDecimales = "[-+]?\\d*\\.?\\,?\\d{0,4}";
                    }
                    break;
            }

            revValorMinimo.ValidationExpression = expresionControlDecimales;
            revValorMaximo.ValidationExpression = expresionControlDecimales;

            revValorMinimo.UpdateAfterCallBack = true;
            revValorMaximo.UpdateAfterCallBack = true;
            if (ddlTipoResultado.SelectedValue == "1") //numerico
            {
                revValorDefecto.ValidationExpression = expresionControlDecimales;
                revValorDefecto.Enabled = true;

            }
            else
            {
                revValorDefecto.Enabled = false;

            }

            revValorDefecto.UpdateAfterCallBack = true;
        }



        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;

            ICriteria crit = m_session.CreateCriteria(typeof(Item));

            crit.Add(Expression.Eq("Codigo", txtCodigo.Text));
            crit.Add(Expression.Eq("Baja", false));


            IList lista = crit.List();
            if (lista.Count != 0)
            {
                lblMensajeCodigo.Text = "El codigo " + txtCodigo.Text + " ya existe. Verifique.";
                lblMensajeCodigo.Visible = true;
                txtCodigo.Text = "";
            }
            else
                lblMensajeCodigo.Visible = false;

            txtCodigo.UpdateAfterCallBack = true;
            lblMensajeCodigo.UpdateAfterCallBack = true;
        }

        protected void lnkRegresar_Click1(object sender, EventArgs e)
        {
            string m_parametroFiltro = "?Codigo=" + Request["Codigo"].ToString() + "&Nombre=" + Request["Nombre"].ToString() + "&Servicio=" + Request["Servicio"].ToString() +
         "&Area=" + Request["Area"].ToString() + "&Orden=" + Request["Orden"].ToString();

            Response.Redirect("ItemList.aspx" + m_parametroFiltro, false);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            string m_parametroFiltro = "?Codigo=" + Request["Codigo"].ToString() + "&Nombre=" + Request["Nombre"].ToString() + "&Servicio=" + Request["Servicio"].ToString() +
            "&Area=" + Request["Area"].ToString() + "&Orden=" + Request["Orden"].ToString() + "&idEfector=" + Request["idEfector"].ToString();
            Response.Redirect("ItemEdit2.aspx" + m_parametroFiltro, false);

        }

        protected void btnGuardarRPDefecto_Click(object sender, EventArgs e)
        {
            GuardarRPDefectoEfector();
        }

        private void GuardarRPDefectoEfector()
        {
            Item oItem = new Item();
            oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
            if (oItem != null)
            {
                //oItem.ResultadoDefecto = ddlResultadoPorDefecto.SelectedItem.Text;

                //oItem.IdResultadoPorDefecto = int.Parse(ddlResultadoPorDefecto.SelectedValue);
                //oItem.Save();


                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(ResultadoItem));
                crit.Add(Expression.Eq("IdItem", oItem));
                crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                crit.Add(Expression.Eq("Baja", false));
                //   crit.Add(Expression.Eq("Resultado", ddlResultadoPorDefecto.SelectedItem.Text));
                /// crit.AddOrder(Order.Asc("Resultado"));  //el orden lo define el usuario 
                IList resultados = crit.List();
                foreach (ResultadoItem oResultado in resultados)
                {
                    if (ddlResultadoPorDefecto.SelectedItem.Text == oResultado.Resultado)
                    {
                        oResultado.ResultadoDefecto = true;
                        oItem.GrabarAuditoriaDetalleItem("Graba Resultado", oUser, "Resultado Defecto", ddlResultadoPorDefecto.SelectedItem.Text, "");
                    }
                    else
                        oResultado.ResultadoDefecto = false;
                    oResultado.Save();
                    
                }
                lblMensajeRpD.Text = "El resultado por defecto ha sido guardado";
                lblMensajeRpD.Visible = true;
                lblMensajeRpD.UpdateAfterCallBack = true;
            }
        }

        protected void btnGuardarHIV_Click(object sender, EventArgs e)
        {
            string valor = "";
            if (Request["id"] != null)
            {
                
                  Item oItem = new Item();
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                if (oItem != null)
                {
                    if (oItem.EtiquetaAdicional != chkEtiquetaAdicional.Checked)
                    { 
                            if (chkEtiquetaAdicional.Checked)
                            valor += "Cambiar Marca a imprimir Etiqueta Adicional";
                            else
                            valor += "Cambiar Marca a NO imprimir  Etiqueta Adicional";
                    }

                    if (oItem.CodificaHiv != chkCodificaHiv.Checked)
                    {
                        if (chkCodificaHiv.Checked)
                            valor = "Cambiar Marca a Codificar Datos de Paciente";
                        else
                            valor = "Cambiar Marca a NO Codificar Datos de Paciente";
                    }
                    oItem.CodificaHiv = chkCodificaHiv.Checked;
                    oItem.LimiteTurnosDia = int.Parse(txtLimite.Value);  ///pasar a dato por efector
                    oItem.EtiquetaAdicional = chkEtiquetaAdicional.Checked;
                    //oItem.IsScreeening = chkIsScreening.Checked;
                    oItem.Save();
                    
                
                    oItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "Mas Opciones", valor, "");
                }
            }
        }

        protected void btnAuditoria_Click(object sender, EventArgs e)
        {

            if (Request["id"] != null)
            {

                DataTable dtAuditoria = GetDataSetAuditoria();
                if (dtAuditoria.Columns.Count > 0)
                {

                    ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                    encabezado1.Value = "Auditoria de Determinaciones";

                    ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                    encabezado2.Value = "Sistema de Auditoria del SIL";

                    ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                    encabezado3.Value = "Auditoria de Determinaciones";

                    oCr.Report.FileName = "../Informes/AuditoriaProtocolo.rpt";
                    oCr.ReportDocument.SetDataSource(dtAuditoria);
                    oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                    oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                    oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                    oCr.DataBind();

                    Utility oUtil = new Utility();
                    string nombrePDF = oUtil.CompletarNombrePDF("Auditoria_" + txtCodigo.Text);
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
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            
            string m_strSQL = @"  SELECT  P.codigo  AS numero, a.apellidousuario as username, A.fecha AS fecha, A.hora, A.accion, A.analisis, A.valor, A.valorAnterior
            FROM LAB_AuditoriaItem  AS A  with (nolock)
    inner join  LAB_Item  P with (nolock)  on P.iditem = A.iditem
    where P.iditem = " + Request["id"].ToString() + @" ORDER BY A.idAuditoriaitem";

            DataSet Ds1 = new DataSet();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds1, "auditoria");

            DataTable data = Ds1.Tables[0];
            return data;

        }

        protected void btnAplicarEfectores_Click(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {

                Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                if (oItem != null)
                    ReplicarVRTodoslosEfectores(oItem);

                lblMensaje3.Visible = true;
                lblMensaje3.Text = "Se aplicaron los cambios en los efectores";
                lblMensaje3.UpdateAfterCallBack = true;
            }
            ////            
        }

        protected void ddlTipoServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoServicio.SelectedValue == "P")///paciente
            {
                ddlSexo.SelectedValue = "0";
                ddlSexo.Enabled = true;

                txtEdadDesde.Value = "";
                txtEdadHasta.Value = "";
                txtEdadDesde.Disabled = false;
                txtEdadHasta.Disabled = false;
                ddlUnidadEdad.Enabled = true;
                rfvEdadDesde.Enabled = true;
                rfvEdadHasta.Enabled = true;
            }

            else
            {
                ddlSexo.SelectedValue = "I";
                ddlSexo.Enabled = false;

                txtEdadDesde.Value = "";
                txtEdadHasta.Value = "";
                txtEdadDesde.Disabled = true;
                txtEdadHasta.Disabled = true;
                ddlUnidadEdad.Enabled = false;
                rfvEdadDesde.Enabled = false;
                rfvEdadHasta.Enabled = false;
            }
            ddlSexo.UpdateAfterCallBack = true;
            ddlUnidadEdad.UpdateAfterCallBack = true;
            rfvEdadDesde.UpdateAfterCallBack = true;
            rfvEdadHasta.UpdateAfterCallBack = true;

        }

        protected void gvListaVRNP_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarVR")
            {
                EliminarItemVR_NP(int.Parse(e.CommandArgument.ToString()));
                CargarGrillaVR_NoPac();
                SetSelectedTab(TabIndex.TWO);
            }
        }

        protected void gvListaVRNP_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[5].Controls[1];

                CmdEliminar.CommandArgument = this.gvListaVRNP.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "EliminarVR";
                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                }
            }
        }

        protected void btnAplicarEfectoresNP_Click(object sender, EventArgs e)
        {
            if (Request["id"] != null)
            {

                Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                if (oItem != null)
                    ReplicarVRTodoslosEfectores_NP(oItem);

                lblMensaje2.Visible = true;
                lblMensaje2.Text = "Se aplicaron los cambios en los efectores";
                lblMensaje2.UpdateAfterCallBack = true;
            }
        }

        protected void gvListaVRNP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAgregarPresentacion_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                if (oItem != null)
                {
                    ItemPresentacion oItemP = new ItemPresentacion();
                    oItemP.IdItem = oItem;
                    oItemP.IdMarca = int.Parse(ddlMarca.SelectedValue);
                    oItemP.Codigo = txtCodigoPresentacion.Text;
                    oItemP.Presentacion = txtPresentacion.Text;
                    oItemP.IdUsuarioRegistro = oUser;
                    oItemP.FechaRegistro = DateTime.Now;

                    oItemP.Save();
                    oItem.GrabarAuditoriaDetalleItem("Guarda", oUser, "Presentacion", txtPresentacion.Text, "");
                    oItem.Save();
                }
                CargarGrillaPresentaciones();

            }
        }

        private void CargarGrillaPresentaciones()
        {
            gdPresentacion.AutoGenerateColumns = false;
            gdPresentacion.DataSource = LeerDatosPresentaciones("P");
            gdPresentacion.DataBind();
            gdPresentacion.UpdateAfterCallBack = true;

        }

        private object LeerDatosPresentaciones(string v)
        {
            string m_strSQL = @" select iditempresentacion, M.nombre as marca , I.presentacion, I.codigo 
 from LAB_ItemPresentacion I with (nolock)
 inner join LAB_MarcaEquipo M with (nolock) on I.idmarca = M.idMarcaEquipo
 where I.baja=0 and iditem =  " + Request["id"].ToString();


            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            //if ((tipo != "NP") && (Ds.Tables[0].Rows.Count == 0) && (btnAplicarEfectores.Visible))
            //    btnAplicarEfectores.Visible = false;

            //if ((tipo == "NP") && (Ds.Tables[0].Rows.Count == 0) && (btnAplicarEfectoresNP.Visible))
            //    btnAplicarEfectoresNP.Visible = false;

            return Ds.Tables[0];
        }

        protected void gdPresentacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {

                EliminarItemPresentacion(e.CommandArgument);
                CargarGrillaPresentaciones();

            }
        }

        private void EliminarItemPresentacion(object idItem)
        {
            ItemPresentacion oRegistro = new ItemPresentacion();
            oRegistro = (ItemPresentacion)oRegistro.Get(typeof(ItemPresentacion), int.Parse(idItem.ToString()));
            if (!oRegistro.tieneDatosVinculados())
            {
                if (oRegistro != null)
                {
                    oRegistro.Baja = true;
                    oRegistro.IdUsuarioRegistro = oUser;
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.Save();
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Elimina", oUser, "Presentacion", txtPresentacion.Text, "");
                }
            }
            else
            {
                lblMensagePresentacion.Visible = true;
                lblMensagePresentacion.Text = "No es posible eliminar está vinculado a Valores de Referencia";
                lblMensagePresentacion.UpdateAfterCallBack = true;
            }

        }

        protected void gdPresentacion_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = this.gdPresentacion.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                }
                if (Request["idEfector"].ToString() == "227")
                
                    CmdEliminar.Visible = true;
                 
                else
                    CmdEliminar.Visible = false;
            }
        }

        protected void gdPresentacion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAgregarVREfector_Click(object sender, EventArgs e)
        {

            if (ddlPresentacionEfector.SelectedValue != "0")
                if (Request["id"] != null)
                {

                    Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                    if (oItem != null)
                        ReplicarPresentacionenEfector(oItem);
                    CargarGrillaVR();
                    gvListaVR.UpdateAfterCallBack = true;

                    /*pone siempre el ultimo como defecto*/
                    int pres = int.Parse(ddlPresentacionEfector.SelectedValue);
                    GuardarPresentacionporDefecto(pres);

                    lblMensaje3.Visible = true;
                    lblMensaje3.Text = "Se aplicaron los cambios.";
                    lblMensaje3.UpdateAfterCallBack = true;
                }

        }

        private void ReplicarPresentacionenEfector(Item oItem)
        {
            Efector oEfectorPrincipal = new Efector();
            oEfectorPrincipal = (Efector)oEfectorPrincipal.Get(typeof(Efector), 227);


            /// recorre para los efectores configurados en lab_configuracion
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Configuracion));
            crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
            IList items = crit.List();

            foreach (Configuracion olabo in items)
            {


                ICriteria crit2 = m_session.CreateCriteria(typeof(ValorReferencia));
                crit2.Add(Expression.Eq("IdItem", oItem));
                crit2.Add(Expression.Eq("IdEfector", olabo.IdEfector));
                crit2.Add(Expression.Eq("IdPresentacion", int.Parse(ddlPresentacionEfector.SelectedValue)));

                // borra si existiesen
                IList items2 = crit2.List();

                foreach (ValorReferencia oDet in items2)
                {
                    oDet.Delete();
                }

                //copia los resultados configurados en nivel central==> oEfectorPrincipal

                ICriteria critNew = m_session.CreateCriteria(typeof(ValorReferencia));
                critNew.Add(Expression.Eq("IdItem", oItem));
                critNew.Add(Expression.Eq("IdEfector", oEfectorPrincipal));
                critNew.Add(Expression.Eq("IdPresentacion", int.Parse(ddlPresentacionEfector.SelectedValue)));

                //   string sDatos = "";
                IList itemsNew = critNew.List();

                foreach (ValorReferencia oDetNew in itemsNew)
                {

                    ValorReferencia oRegistro = new ValorReferencia();

                    oRegistro.IdEfector = olabo.IdEfector; // oEfector;
                    oRegistro.IdItem = oItem;
                    oRegistro.Sexo = oDetNew.Sexo;
                    oRegistro.TodasEdades = oDetNew.TodasEdades;
                    oRegistro.EdadDesde = oDetNew.EdadDesde;
                    oRegistro.EdadHasta = oDetNew.EdadHasta;
                    oRegistro.UnidadEdad = oDetNew.UnidadEdad;
                    oRegistro.IdMetodo = oDetNew.IdMetodo;

                    oRegistro.TipoValor = oDetNew.TipoValor;
                    oRegistro.ValorMinimo = oDetNew.ValorMinimo;
                    oRegistro.ValorMaximo = oDetNew.ValorMaximo;

                    oRegistro.Observacion = oDetNew.Observacion;
                    oRegistro.IdUsuarioRegistro = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                    oRegistro.FechaRegistro = DateTime.Now;

                    oRegistro.IdPresentacion = oDetNew.IdPresentacion;

                    oRegistro.Save();

                    string valor = oRegistro.IdEfector.Nombre + "-Sexo:" + oRegistro.Sexo + "-ED:" + oRegistro.EdadDesde.ToString() + "-EH:" + oRegistro.EdadHasta.ToString() + "-ValorMin:" + oRegistro.ValorMinimo.ToString() + "-ValorMax:" + oRegistro.ValorMaximo.ToString();
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Guardar", oUser, "VR", valor, "");


                }

            }
        }

        protected void btnPresentacionDefecto_Click(object sender, EventArgs e)
        { int pres = int.Parse(ddlPresentacionEfectorDefecto.SelectedValue);
            GuardarPresentacionporDefecto(pres);

        }

        private void GuardarPresentacionporDefecto(int pres)
        {
            Item oItemPrincipal = new Item();
            oItemPrincipal = (Item)oItemPrincipal.Get(typeof(Item), int.Parse(Request["id"].ToString()));


            ItemEfector oItem = new ItemEfector();
            oItem = (ItemEfector)oItem.Get(typeof(ItemEfector), "IdItem", oItemPrincipal, "IdEfector", oUser.IdEfector);

            if (oItem != null)
            {
                oItem.IdPresentacionDefecto = pres;
                oItem.Save();

            }
        }

        protected void btnAgregarMuestra_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Item oItem = new Item(); oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["id"].ToString()));
                if (oItem != null)
                {
                    if (!oItem.VerificaMuestrasRepetida(int.Parse(ddlMuestra1.SelectedValue)))
                    {
                        ItemMuestra oItemP = new ItemMuestra();
                        oItemP.IdItem = oItem;
                        oItemP.IdMuestra = int.Parse(ddlMuestra1.SelectedValue);
                        oItemP.IdUsuarioRegistro = oUser;
                        oItemP.FechaRegistro = DateTime.Now;

                        oItemP.Save();
                        oItem.GrabarAuditoriaDetalleItem("Guarda", oUser, "Muestra", ddlMuestra1.SelectedItem.Text, "");
                        oItem.Save();
                    }
                    else
                    {
                        lblMensajeMuestra.Visible = true;
                        lblMensajeMuestra.Text = "Ya existe la muestra para la determinacion";
                        lblMensajeMuestra.UpdateAfterCallBack = true;
                    }
                }
                CargarGrillaMuestras();


                
            }
        }

        protected void gvMuestraItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[1].Controls[1];
                CmdEliminar.CommandArgument = this.gvMuestraItem.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                }
                if (Request["idEfector"].ToString() == "227")

                    CmdEliminar.Visible = true;

                else
                    CmdEliminar.Visible = false;
            }

        }

        protected void gvMuestraItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                EliminarItemMuestra(int.Parse(e.CommandArgument.ToString()));
                CargarGrillaMuestras();
                SetSelectedTab(TabIndex.OCTAVO);
            }
        }

        private void EliminarItemMuestra(int v)
        {
            ItemMuestra oRegistro = new ItemMuestra();
            oRegistro = (ItemMuestra)oRegistro.Get(typeof(ItemMuestra), v);
            if (oRegistro != null)
            {
                Muestra oM = new Muestra();
                oM = (Muestra)oM.Get(typeof(Muestra), oRegistro.IdMuestra);
                if (oM != null)
                    oRegistro.IdItem.GrabarAuditoriaDetalleItem("Elimina", oUser, "Muestra", oM.Nombre, "");

                oRegistro.Delete();




        }
        }
    }
}
     

