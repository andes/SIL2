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
using System.Data.SqlClient;
using Business;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO;
using NHibernate;
using NHibernate.Expression;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class InformeList2 : System.Web.UI.Page
    {

        public CrystalReportSource oCr = new CrystalReportSource();
        Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
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
                if (Session["idUsuario"] != null)
                {
                    //  CargarListas();
                    //if (Request["Estado"].ToString() == "0"){   CargarItem();}
                    //else ddlItem.Enabled=false;

                    CargarGrilla();
                }
                else Response.Redirect("../FinSesion.aspx", false);
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
        //private void CargarListas()
        //{
        //    Utility oUtil = new Utility();

        //    /////////////////Impresoras////////////////////////
        //    //string m_ssql = "SELECT idImpresora, nombre FROM LAB_Impresora ";
        //    //oUtil.CargarCombo(ddlImpresora, m_ssql, "nombre", "nombre");
        //    //if (Session["Impresora"] != null) ddlImpresora.SelectedValue = Session["Impresora"].ToString();
        //    /////////////////Fin de Impresoras///////////////////

        //}
        private void CargarGrilla()
        {
            gvLista.DataSource = GetDataSet("","");
            gvLista.DataBind();

            PonerImagenes();
            CantidadRegistros.Text = gvLista.Rows.Count.ToString() + " registros encontrados";

        }

        private string GenerarListaProtocolos()
        {
            string m_lista = "";
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    if (m_lista == "")
                        m_lista += gvLista.DataKeys[row.RowIndex].Value.ToString();
                    else
                        m_lista += "," + gvLista.DataKeys[row.RowIndex].Value.ToString();
                }
            }
            return m_lista;
        }
        private DataTable GetDataSet(string s_lista, string s_donde)
        {
            string s_vta_LAB = "vta_LAB_Derivaciones";
            //string s_vta_LAB="vta_LAB_DerivacionesPendientes";
            //if (s_donde == "pdf") s_vta_LAB = "vta_LAB_DerivacionesEnviadas";

            //else
            //{
            //    if (Request["Estado"].ToString() == "0") s_vta_LAB = "vta_LAB_DerivacionesPendientes";
            //    if ((Request["Estado"].ToString() == "1") || (Request["Estado"].ToString() == "2")) s_vta_LAB = "vta_LAB_DerivacionesEnviadas";
            //}

            string s_iddetalle = "";
            if (s_donde != "pdf")                
                s_iddetalle = "idDetalleProtocolo,";

            string m_strSQL = " SELECT "+s_iddetalle+" estado, numero, convert(varchar(10), fecha,103) as fecha, dni, "+
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , solicitante as especialista " +
            " FROM  " +s_vta_LAB+ " WHERE " + Request["Parametros"].ToString() ;
            //if (ddlItem.SelectedValue != "0")
            //    if (ddlItem.SelectedValue != "")
            //    m_strSQL += "  and idItem= " + ddlItem.SelectedValue;
            if (s_donde != "pdf")
            {
               // if ((Request["Estado"].ToString() == "1") || (Request["Estado"].ToString() == "2"))
                    m_strSQL += "  and estado= " + Request["Estado"].ToString();
            }
            else
                m_strSQL += "  and estado= 1";

            //if (ddlEfector.SelectedValue != "0")
            //    m_strSQL += "  and idEfector= " + ddlEfector.SelectedValue;

            //if (ddlEstadoFiltro.SelectedValue != "-1")
            //    m_strSQL += "  and estado= " + ddlEstadoFiltro.SelectedValue;

            if (s_lista!="")
                m_strSQL += "  and idDetalleProtocolo in (" + s_lista +")";

            m_strSQL += " ORDER BY efectorDerivacion,numero ";



            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];

        }


  //      private void CargarItem()
  //      {
  //          string s_vta_LAB = "vta_LAB_DerivacionesPendientes";
  //          if (Request["Estado"].ToString() == "0") s_vta_LAB = "vta_LAB_DerivacionesPendientes";
  //          if (Request["Estado"].ToString() == "1") s_vta_LAB = "vta_LAB_DerivacionesEnviadas";


  //          Utility oUtil = new Utility();
  //          ///Carga de combos de areas
  //          string m_ssql = " SELECT DISTINCT idItem, determinacion FROM  " + s_vta_LAB + " WHERE  " + Request["Parametros"].ToString()+ " ORDER BY determinacion";

  //          oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "determinacion");
  //          ddlItem.Items.Insert(0, new ListItem("Todas", "0"));


  //          //m_ssql = " SELECT DISTINCT idEfector, efectorDerivacion FROM vta_LAB_Derivaciones " +
  //          //         " WHERE   " + Request["Parametros"].ToString();

  //          //oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "efectorDerivacion");
  //          //ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));
  //m_ssql = null;
  //          oUtil = null;


  //      }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {          
            CargarGrilla();            
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  CargarGrilla();
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);           
        }


        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
            PonerImagenes();
        }



        protected void lnkDesMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
          //  PonerImagenes();
        }

        private void PonerImagenes()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {


                switch (row.Cells[1].Text)
                {
                    case "0":
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/pendiente.png";
                            hlnk.ToolTip = "Pendiente de derivar";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //enviado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/enviado.png";
                            hlnk.ToolTip = "Enviado";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;
                    case "2": //enviado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/block.png";
                            hlnk.ToolTip = "No enviado";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;

                }

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            //GuardarDerivaciones();
            GuardarDerivaciones2();
            CargarGrilla();
        }

        //private void GuardarDerivaciones()
        //{
           
          
        //    foreach (GridViewRow row in gvLista.Rows)
        //    {

        //        CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
        //        if (a.Checked == true)
        //        {
        //            DetalleProtocolo oDetalle= new DetalleProtocolo();
        //            oDetalle= (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo),int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

        //            ISession m_session = NHibernateHttpModule.CurrentSession;
        //            ICriteria crit = m_session.CreateCriteria(typeof( Business.Data.Laboratorio.Derivacion));
        //            crit.Add(Expression.Eq("IdDetalleProtocolo",oDetalle));
        //           // crit.Add(Expression.Eq("Baja", false));


        //            ///Si tiene resultados predeterminados muestra un combo
        //            IList lista = crit.List();

        //              if (lista.Count > 0)
        //              {
        //                  foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
        //                  {
        //                      oDeriva.Delete();
        //                  }
        //              }

        //           Business.Data.Laboratorio.Derivacion oRegistro = new Business.Data.Laboratorio.Derivacion();
        //           oRegistro.IdDetalleProtocolo = oDetalle;
        //           oRegistro.Estado = int.Parse(ddlEstado.SelectedValue);
        //           oRegistro.Observacion = txtObservacion.Text;
        //            oRegistro.IdUsuarioRegistro = oUser.IdUsuario;// int.Parse(Session["idUsuario"].ToString());
        //           oRegistro.FechaRegistro = DateTime.Now;
        //           oRegistro.FechaResultado = DateTime.Parse("01/01/1900");
        //           oRegistro.Save();

        //            // nuevo: se graba el lugar a donde se derivó en ese momento.

        //            //Usuario oUser = new Usuario();
        //            //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));

        //            if (ddlEstado.SelectedValue == "0") /// pendiente 
        //                oDetalle.ResultadoCar = "Pendiente de Derivacion";                       
        //            if (ddlEstado.SelectedValue == "1") /// enviado
        //                oDetalle.ResultadoCar = "Derivado: " + oDetalle.IdItem.GetEfectorDerivacion(oUser.IdEfector);
        //            if (ddlEstado.SelectedValue == "2") /// no enviado
        //                oDetalle.ResultadoCar  = " No Derivado. " + txtObservacion.Text;

        //            oDetalle.Save();
        //            oDetalle.GrabarAuditoriaDetalleProtocolo("Graba", oUser.IdUsuario);

        //            /// fin de grabar en resultado la derivacion
        //        }
        //    }
            
        
        //}


        private void GuardarDerivaciones2()
        {

            if (Session["idUsuario"] != null)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

           
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    DetalleProtocolo oDetalle = new DetalleProtocolo();
                    oDetalle = (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.Derivacion));
                    crit.Add(Expression.Eq("IdDetalleProtocolo", oDetalle));
                    // crit.Add(Expression.Eq("Baja", false));


                    ///Si tiene resultados predeterminados muestra un combo
                    IList lista = crit.List();

                    if (lista.Count > 0)
                    {
                        foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                        {
                            //oDeriva.Delete();

                            oDeriva.Estado = int.Parse(ddlEstado.SelectedValue);
                            oDeriva.Observacion = txtObservacion.Text;
                            oDeriva.IdUsuarioRegistro = oUser.IdUsuario;// int.Parse(Session["idUsuario"].ToString());
                            oDeriva.FechaRegistro = DateTime.Now;
                            oDeriva.FechaResultado = DateTime.Parse("01/01/1900");
                            oDeriva.Save();


                           

                            if (ddlEstado.SelectedValue == "0") /// pendiente 
                                oDetalle.ResultadoCar = "Pendiente de Derivacion";
                            if (ddlEstado.SelectedValue == "1") /// enviado
                                oDetalle.ResultadoCar = "Derivado: " + oDeriva.IdEfectorDerivacion.Nombre;
                            if (ddlEstado.SelectedValue == "2") /// no enviado
                                oDetalle.ResultadoCar = " No Derivado. " + txtObservacion.Text;

                            oDetalle.ConResultado = true;
                            oDetalle.IdUsuarioResultado = oUser.IdUsuario;//int.Parse(Session["idUsuario"].ToString());
                            oDetalle.FechaResultado = DateTime.Now;
                            oDetalle.Save();
                            oDetalle.GrabarAuditoriaDetalleProtocolo("Graba", oUser.IdUsuario);

                            /*Actualiza estado de protocolo*/
                            if (oDetalle.IdProtocolo.ValidadoTotal("Derivacion", oUser.IdUsuario))
                                oDetalle.IdProtocolo.Estado = 2;  //validado total (cerrado);
                            else
                            {
                                if (oDetalle.IdProtocolo.EnProceso())
                                {
                                    oDetalle.IdProtocolo.Estado = 1;//en proceso
                                                                    // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                                }
                                else
                                    oDetalle.IdProtocolo.Estado = 0;
                            }
                            oDetalle.IdProtocolo.Save();

                            

                        }
                    }

                    
                }
            }

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void ddlEstadoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }


        private void MostrarInforme(string tipo)
        {
            if (Session["idUsuario"] != null)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

           
            DataTable dt = new DataTable();
            dt = GetDataSet(GenerarListaProtocolos(), "pdf");

            if (dt.Rows.Count > 0)
            {
                //Usuario oUser = new Usuario();
                //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            

                //Aca se deberá consultar los parametros para mostrar una hoja de trabajo u otra
                //this.oCr.Report.FileName = "HTrabajo2.rpt";
                string informe = "../Informes/Derivacion.rpt";
            //    Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

                ParameterDiscreteValue encabezado1 = new ParameterDiscreteValue();
                encabezado1.Value = oCon.EncabezadoLinea1;

                ParameterDiscreteValue encabezado2 = new ParameterDiscreteValue();
                encabezado2.Value = oCon.EncabezadoLinea2;

                ParameterDiscreteValue encabezado3 = new ParameterDiscreteValue();
                encabezado3.Value = oCon.EncabezadoLinea3;


                //if (oCon.TipoHojaTrabajo == 0) informe = "HTrabajo.rpt";
                //if (oCon.TipoHojaTrabajo == 1) informe = "HTrabajo2.rpt";
                oCr.Report.FileName = informe;
                oCr.ReportDocument.SetDataSource(dt);
                oCr.ReportDocument.ParameterFields[0].CurrentValues.Add(encabezado1);
                oCr.ReportDocument.ParameterFields[1].CurrentValues.Add(encabezado2);
                oCr.ReportDocument.ParameterFields[2].CurrentValues.Add(encabezado3);
                oCr.DataBind();

                Utility oUtil = new Utility();
                string nombrePDF = oUtil.CompletarNombrePDF(oUser.IdEfector.IdEfector2.Trim()+"_Derivaciones");
                oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, nombrePDF);
                 



                
            }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            MostrarInforme("pdf");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            MostrarInforme("Imprimir");
            PonerImagenes();
        }

        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            MostrarInforme("pdf");

        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            MostrarInforme("Imprimir");
            PonerImagenes();
        }

    }
}
