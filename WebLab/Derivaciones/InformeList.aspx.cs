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

namespace WebLab.Derivaciones
{
    public partial class InformeList : System.Web.UI.Page
    {

        public CrystalReportSource oCr = new CrystalReportSource();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            oCr.Report.FileName = "";
            oCr.CacheDuration = 0;
            oCr.EnableCaching = false;
            //Configuracion oC = new Configuracion();
            //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
           
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarItem();
                CargarGrilla();
              
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
            string s_iddetalle = "";
            if (s_donde != "pdf")                
                s_iddetalle = "idDetalleProtocolo,";

            string m_strSQL = " SELECT "+s_iddetalle+" estado, numero, convert(varchar(10), fecha,103) as fecha, dni, "+
            " apellido + ' '+ nombre as paciente, determinacion, efectorderivacion, username, fechaNacimiento as edad, unidadEdad, sexo, observacion , solicitante as especialista " +
            " FROM dbo.vta_LAB_Derivaciones " +
                              " WHERE " + Request["Parametros"].ToString() ;
            if (ddlItem.SelectedValue != "0")
                m_strSQL += "  and idItem= " + ddlItem.SelectedValue;

            if (ddlEfector.SelectedValue != "0")
                m_strSQL += "  and idEfector= " + ddlEfector.SelectedValue;

            if (ddlEstadoFiltro.SelectedValue != "-1")
                m_strSQL += "  and estado= " + ddlEstadoFiltro.SelectedValue;

            if (s_lista!="")
                m_strSQL += "  and idDetalleProtocolo in (" + s_lista +")";

            m_strSQL += " ORDER BY efectorDerivacion,numero ";



            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];

        }


        private void CargarItem()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de areas
            string m_ssql = " SELECT DISTINCT idItem, determinacion FROM vta_LAB_Derivaciones " +
                            " WHERE  " + Request["Parametros"].ToString()+ " ORDER BY determinacion";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "determinacion");
            ddlItem.Items.Insert(0, new ListItem("Todas", "0"));


            m_ssql = " SELECT DISTINCT idEfector, efectorDerivacion FROM vta_LAB_Derivaciones " +
                     " WHERE   " + Request["Parametros"].ToString();

            oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "efectorDerivacion");
            ddlEfector.Items.Insert(0, new ListItem("Todos", "0"));
  m_ssql = null;
            oUtil = null;


        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {

            CargarGrilla();
            
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
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
                            hlnk.ToolTip = "Protocolo Impreso";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //enviado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/enviado.png";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;
                    case "2": //enviado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/block.png";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;

                }

            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            GuardarDerivaciones();
            CargarGrilla();
        }

        private void GuardarDerivaciones()
        {
           
          
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    DetalleProtocolo oDetalle= new DetalleProtocolo();
                    oDetalle= (DetalleProtocolo)oDetalle.Get(typeof(DetalleProtocolo),int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));

                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof( Business.Data.Laboratorio.Derivacion));
                    crit.Add(Expression.Eq("IdDetalleProtocolo",oDetalle));
                   // crit.Add(Expression.Eq("Baja", false));


                    ///Si tiene resultados predeterminados muestra un combo
                    IList lista = crit.List();

                      if (lista.Count > 0)
                      {
                          foreach (Business.Data.Laboratorio.Derivacion oDeriva in lista)
                          {
                              oDeriva.Delete();
                          }
                      }

                   Business.Data.Laboratorio.Derivacion oRegistro = new Business.Data.Laboratorio.Derivacion();
                   oRegistro.IdDetalleProtocolo = oDetalle;
                   oRegistro.Estado = int.Parse(ddlEstado.SelectedValue);
                   oRegistro.Observacion = txtObservacion.Text;
                   oRegistro.IdUsuarioRegistro =  int.Parse(Session["idUsuario"].ToString());
                   oRegistro.FechaRegistro = DateTime.Now;
                   oRegistro.FechaResultado = DateTime.Parse("01/01/1900");
                   oRegistro.Save();             
                
                }
            }
            
        
        }

        protected void ddlEstadoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }


        private void MostrarInforme(string tipo)
        {
            DataTable dt = new DataTable();
            dt = GetDataSet(GenerarListaProtocolos(), "pdf");

            if (dt.Rows.Count > 0)
            {


                //Aca se deberá consultar los parametros para mostrar una hoja de trabajo u otra
                //this.oCr.Report.FileName = "HTrabajo2.rpt";
                string informe = "../Informes/Derivacion.rpt";
                Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);

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

                if (tipo == "Imprimir")//imprimir
                {
                    //oCr.ReportDocument.PrintOptions.PrinterName = oCon.NombreImpresora;
                    //oCr.ReportDocument.PrintToPrinter(1, false, 0, 0);
                }
                else
                {

                    oCr.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Derivaciones.pdf");

                    //MemoryStream oStream; // using System.IO
                    //oStream = (MemoryStream)oCr.ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.ContentType = "application/pdf";

                    //Response.AddHeader("Content-Disposition", "attachment;filename=Derivaciones.pdf");

                    //Response.BinaryWrite(oStream.ToArray());
                    ////    string popupImprimir = "<script language='JavaScript'> windows.print(); </script>";
                    ////Page.RegisterStartupScript("PopupScriptImprimir", popupImprimir);

                    //Response.End();



                }
            }

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            MostrarInforme("pdf");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            MostrarInforme("Imprimir");
        }

        protected void lnkPDF_Click(object sender, EventArgs e)
        {
            MostrarInforme("pdf");

        }

        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            MostrarInforme("Imprimir");
        }

    }
}
