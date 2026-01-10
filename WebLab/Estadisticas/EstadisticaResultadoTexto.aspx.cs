using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebLab.Estadisticas
{
    public partial class EstadisticaResultadoTexto : System.Web.UI.Page
    {
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            else
                Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Inicializar();

            }
        }

        private void Inicializar()
        {
          
            txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            lblEfector.Text = oUser.IdEfector.Nombre.ToUpper();
            CargarListas();
            

        }
        protected void ddlServicio_SelectedIndexChanged1(object sender, EventArgs e)
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            string m_ssql = "SELECT idArea, nombre FROM LAB_Area with (nolock) where baja=0 and idtipoServicio=" + ddlServicio.SelectedValue + " order by nombre ";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);
            ddlArea.Items.Insert(0, new ListItem("--Todas--", "0"));
        }
        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedValue != "0") CargarPracticas();
        }

        private void CargarPracticas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

//            string m_ssql = @"select idItem, LAB_Item.nombre from LAB_Item  (nolock)
//where idArea=" + ddlArea.SelectedValue + " and LAB_Item.baja=0 and informable=1 and tipo='P' and idEfectorDerivacion=LAB_Item.idEfector order by LAB_Item.nombre ";


            string m_ssql                 = @"SELECT   I.idItem as idItem, I.nombre + ' - ' + I.codigo as nombre 
             FROM Lab_item I with (nolock) 
             inner join lab_itemEfector IE with (nolock) on IE.idItem= I.iditem and ie.idefector=" + oUser.IdEfector.IdEfector.ToString() +
             @"INNER JOIN Lab_area A with (nolock) ON A.idArea= I.idArea 
             where I.idArea=" + ddlArea.SelectedValue + @" and I.baja=0 and I.informable=1 and I.tipo='P' and I.idEfectorDerivacion=I.idEfector 
            order by  nombre ";



            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre", connReady);
                ddlItem.Items.Insert(0, new ListItem("--Todos--", "0"));
             
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            AgregarDeterminacion();
        }
        private void AgregarDeterminacion()
        {
            if (ddlItem.SelectedValue == "0")

            {
                if (ddlArea.SelectedValue != "0")
                {
                    Area oArea = new Area();
                    oArea = (Area)oArea.Get(typeof(Area), int.Parse(ddlArea.SelectedValue));
                    Item oDetalle = new Item();
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Item));
                    crit.Add(Expression.Eq("IdArea", oArea));
                    crit.Add(Expression.Eq("Baja", false));
                    crit.Add(Expression.Eq("Informable", true));
                    crit.AddOrder(Order.Asc("Nombre"));


                    IList items = crit.List();

                    foreach (Item oDet in items)
                    {
                        var item = new ListItem();
                        item.Value = oDet.IdItem.ToString();
                        item.Text = oDet.Nombre + "-" + oDet.Codigo;
                        lstItem.Items.Add(item);
                    }
                }
                else
                {
                    Item oDetalle = new Item();
                    ISession m_session = NHibernateHttpModule.CurrentSession;
                    ICriteria crit = m_session.CreateCriteria(typeof(Item));

                    crit.Add(Expression.Eq("Baja", false));
                    crit.Add(Expression.Eq("Informable", true));
                    crit.AddOrder(Order.Asc("Nombre"));

                    IList items = crit.List();
                    foreach (Item oDet in items)
                    {
                        var item = new ListItem();
                        item.Value = oDet.IdItem.ToString();
                        item.Text = oDet.Nombre + "-" + oDet.Codigo;
                        lstItem.Items.Add(item);
                    }
                }
            }
            else
            {
                Item oDet = new Item();
                oDet = (Item)oDet.Get(typeof(Item), int.Parse(ddlItem.SelectedValue));

                var item = new ListItem();
                item.Value = oDet.IdItem.ToString();
                item.Text = oDet.Nombre + "-" + oDet.Codigo;
                lstItem.Items.Add(item);
            }


            for (int i = 0; i < lstItem.Items.Count; i++)
            {
                lstItem.Items[i].Selected = true;
            }
            lstItem.UpdateAfterCallBack = true;
        }

        protected void btnSacarItem_Click(object sender, EventArgs e)
        {
            SacarDeterminacion();
        }
        private void SacarDeterminacion()
        {
            if (lstItem.SelectedValue != "")
            {
                lstItem.Items.Remove(lstItem.SelectedItem);
                lstItem.UpdateAfterCallBack = true;
            }
        }
        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio with (nolock) where baja=0 and idTipoServicio in (1,3)";            
           
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);

             

            m_ssql = "SELECT idArea, nombre FROM LAB_Area with (nolock) where baja=0    order by nombre ";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);
            ddlArea.Items.Insert(0, new ListItem("--Todas--", "0"));
             
             
            
            m_ssql = null;
            oUtil = null;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string m_listaCodigo = ""; string m_listaNombres = "";
            for (int i = 0; i < lstItem.Items.Count; i++)
            {
                if (lstItem.Items[i].Selected)
                {
                    Item oDet = new Item();
                    oDet = (Item)oDet.Get(typeof(Item), int.Parse(lstItem.Items[i].Value));

                    if (oDet != null)
                    {
                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria critItemEfector = m_session.CreateCriteria(typeof(ItemEfector));
                        critItemEfector.Add(Expression.Eq("IdItem", oDet));
                        critItemEfector.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                        IList detalle1 = critItemEfector.List();

                        foreach (ItemEfector oitemEfector in detalle1)
                        {

                            //ISession m_session = NHibernateHttpModule.CurrentSession;
                            ICriteria critItemPractica = m_session.CreateCriteria(typeof(PracticaDeterminacion));
                            critItemPractica.Add(Expression.Eq("IdItemPractica", oitemEfector.IdItem));
                            critItemPractica.Add(Expression.Eq("IdEfector", oUser.IdEfector));

                            IList detalle2 = critItemPractica.List();

                            if (detalle2.Count > 0)
                            {///Determinacion compuesta ==> se busca determinaciones asociadas para el informe
                                foreach (PracticaDeterminacion oitemPractica in detalle2)
                                {
                                    Item oDet2 = new Item();
                                    oDet2 = (Item)oDet2.Get(typeof(Item), oitemPractica.IdItemDeterminacion);


                                    if (oDet2 != null)
                                    {
                                        if (m_listaCodigo == "")
                                            m_listaCodigo = "'" + oDet2.Codigo + "'";
                                        else
                                            m_listaCodigo += ",'" + oDet2.Codigo + "'";
                                        if (m_listaNombres == "")
                                            m_listaNombres = "[" + oDet2.Nombre.Replace("[", "").Replace("]", "") + "]";
                                        else
                                            m_listaNombres += ",[" + oDet2.Nombre.Replace("[", "").Replace("]", "") + "]";
                                    }
                                }
                            }
                            else
                            {
                                ///Determinacion simple 
                                if (m_listaCodigo == "")
                                    m_listaCodigo = "'" + oitemEfector.IdItem.Codigo + "'";
                                else
                                    m_listaCodigo += ",'" + oitemEfector.IdItem.Codigo + "'";
                                if (m_listaNombres == "")
                                    m_listaNombres = "[" + oitemEfector.IdItem.Nombre.Replace("[","").Replace("]", "") + "]";
                                else
                                    m_listaNombres += ",[" + oitemEfector.IdItem.Nombre.Replace("[", "").Replace("]", "") + "]";
                            }

                        }

                    }

                }
            }

            if (m_listaNombres != "")
            {
                string m_strSQLCondicion = " 1=1 and b.baja=0 ";

                m_strSQLCondicion += " and b.idEfector=" + oUser.IdEfector.IdEfector.ToString();
                m_strSQLCondicion += " and DP.idusuariovalida>0";

                if (ddlTipoDatos.SelectedValue == "Paciente")
                {
                    m_strSQLCondicion += " and b.idPaciente>0 ";
                }

                if (ddlTipoDatos.SelectedValue == "No Paciente")
                {
                    m_strSQLCondicion += " and b.idPaciente=-1 ";
                }

                if (txtFechaDesde.Value != "")
                {
                    DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                    m_strSQLCondicion += " and b.fechaRegistro >= '" + fecha1.ToString("yyyyMMdd") + "'";
                }

                if (txtFechaHasta.Value != "")
                {
                    DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value).AddDays(1);
                    m_strSQLCondicion += " AND b.fechaRegistro  < '" + fecha2.ToString("yyyyMMdd") + "'";
                }

                string m_strSQL = @"with vta_ResultadoPivot_car1 as (

SELECT    b.numero AS numero, b.fecha, 
case when I.idtiporesultado=1 then convert(varchar,DP.resultadoNum) else
DP.resultadoCar end as resultadoCar, DP.idProtocolo,replace( replace(I.nombre,'[',''),']','') AS item 
FROM            dbo.LAB_DetalleProtocolo AS DP with (nolock)  
					  inner join LAB_Item I with (nolock) on I.iditem= DP.idsubitem
					   inner join lab_protocolo b with (nolock) on dp.idProtocolo = b.idProtocolo
WHERE   " + m_strSQLCondicion + @"   and codigo in (" + m_listaCodigo + @") and DP.idUsuarioValida>0 
)
SELECT Child.numero as Protocolo,  fecha," + m_listaNombres +
                @"FROM (
   SELECT    a.* FROM vta_ResultadoPivot_car1 a " + @"    
   )
  pvt PIVOT (max(resultadocar) FOR item IN (" + m_listaNombres + @"))  AS Child 
 ";

                DataSet Ds = new DataSet();
                
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);


                ExportarExcel(Ds.Tables[0]);
            }
            else
            {
                lblError.Text = "No hay datos para los filtros seleccionados";
                lblError.Visible = true;
            }

        }

        private void ExportarExcel(DataTable tabla)
        {
            try
            {
                lblError.Text = "";
                lblError.Visible = true;

                if (tabla.Rows.Count > 0)
                    {
                    Utility.ExportDataTableToXlsx(tabla, "Estadisticas_"+ oUser.IdEfector.Nombre+"_"+ DateTime.Now.ToShortDateString() );
                        //StringBuilder sb = new StringBuilder();
                        //StringWriter sw = new StringWriter(sb);
                        //HtmlTextWriter htw = new HtmlTextWriter(sw);
                        //Page pagina = new Page();
                        //HtmlForm form = new HtmlForm();
                        //GridView dg = new GridView();
                        //dg.EnableViewState = false;
                        //dg.DataSource = tabla;


                    //dg.DataBind();
                    //pagina.EnableEventValidation = false;
                    //pagina.DesignerInitialize();
                    //pagina.Controls.Add(form);
                    //form.Controls.Add(dg);
                    //pagina.RenderControl(htw);
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.ContentType = "application/vnd.ms-excel";
                    //Response.AddHeader("Content-Disposition", "attachment;filename=Estadisticas_"+ oUser.IdEfector.Nombre+"_"+ DateTime.Now.ToShortDateString() + ".xls");
                    //Response.Charset = "UTF-8";
                    //Response.ContentEncoding = Encoding.Default;
                    //Response.Write(sb.ToString());
                    //Response.End();
                }
               else
                {

                    lblError.Text = "No hay datos para los filtros seleccionados";
                    lblError.Visible = true;
                }
            }
            catch
            {

                lblError.Text = "Ha superado el límite para exportar datos. Comuniquese con el administrador.";
                lblError.Visible = true;
            }
        }
     
    }
}