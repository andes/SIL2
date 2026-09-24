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
using System.Data.SqlClient;
using Business.Data;

namespace WebLab.Derivaciones
{
    public partial class ResultadoList : System.Web.UI.Page
    {

        private Usuario oUsuario = new Usuario();
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["idUsuario"] != null)
            {
                oUsuario = (Usuario)new Usuario().Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                if (!IsPostBack)
                {
                    VerificaPermisos("Consultas"); 
                    txtFechaDesde.Value = DateTime.Now.AddDays(-7).ToShortDateString();
                    txtFechaHasta.Value = DateTime.Now.ToShortDateString();

                    CargarListas();
                }
            }
            else
                Response.Redirect("../FinSesion.aspx", false);
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

                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

     



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
           // CurrentPageLabel.Text = " ";

        }
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos(0);

            gvLista.DataBind();
            PintarReferencias();


        }

        private DataTable LeerDatos(int tipo)
        {
            string str_condicion = " 1=1 AND idEfector=" + oUsuario.IdEfector.IdEfector;

       
            if (txtDni.Value != "") str_condicion += " AND dni= '" + txtDni.Value + "'";
            if (txtApellido.Text != "") str_condicion += " AND apellido like '%" + txtApellido.Text.TrimEnd() + "%'";
            if (txtNombre.Text != "") str_condicion += " AND nombre like '%" + txtNombre.Text.TrimEnd() + "%'";
            if (txtFechaDesde.Value != "")
            {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                str_condicion += " AND fecha >= '" + fecha1.ToString("yyyyMMdd") + "'";
            }

            if (txtFechaHasta.Value != "")
            {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                fecha2 = fecha2.AddDays(1);
                str_condicion += " AND fecha <= '" + fecha2.ToString("yyyyMMdd") + "'";
            }

            if (ddlEfectorDestino.SelectedValue != "0")
                str_condicion += " AND idEfectorDerivacion=" + ddlEfectorDestino.SelectedValue;
            /////////////
                ///Si no fue enviado muestra la observacion sino el resultado
            string m_strSQL = " SELECT estadoDerivacion , numero , idlote, convert(varchar(10),fecha,103) as fecha, dni, apellido + ' ' + nombre as paciente, determinacion, efectorDerivacion, " +
                            " case when estadoDerivacion=1 then resultado else observacion end as resultado   "+
                            " FROM [vta_LAB_Derivaciones] " +
                            " WHERE  " + str_condicion + 
                            " ORDER BY convert(datetime,fecha) desc ";     

            DataSet Ds = new DataSet();
            //SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";
            return Ds.Tables[0];
            //////////


           
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        private void Imprimir(object p, string p_2)
        {

         
        }


     

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }

        private void PintarReferencias()
        {



            foreach (GridViewRow row in gvLista.Rows)
            {
                switch (row.Cells[0].Text.Trim())
                {
                    case "&nbsp;": ///pendiente
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/pendiente.png";
                            row.Cells[0].Controls.Add(hlnk);
                        }
                        break;
                    case "0": ///pendiente
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/pendiente.png";
                            row.Cells[0].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //enviado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/enviado.png";
                            row.Cells[0].Controls.Add(hlnk);
                        }
                        break;
                    case "2": //no enviado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/block.png";
                            row.Cells[0].Controls.Add(hlnk);
                        }
                        break;
                    case "3": //Recibido
                        {
                            Label hlnk = new Label();
                            hlnk.CssClass = "glyphicon glyphicon-inbox";
                            row.Cells[0].Controls.Add(hlnk);
                        }
                        break;
                    case "4": //Pendiente de envio
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/reloj-de-arena.png";
                            row.Cells[0].Controls.Add(hlnk);
                        }
                        break;
                }

               

            }

        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click1(object sender, EventArgs e)
        {
            CargarGrilla();
        }


        private void CargarListas()
        {
            Utility oUtil = new Utility();
            string m_ssql = "SELECT  E.idEfector, E.nombre " +
               " FROM  Sys_Efector AS E " +
               " where E.idEfector IN " +
               " (SELECT DISTINCT idEfectorDerivacion FROM   lab_itemEfector AS IE  " +
               " WHERE Ie.disponible=1 and IE.idEfector<>Ie.idEfectorDerivacion and  IE.idEfector=" + oUsuario.IdEfector.IdEfector.ToString() + ")" +
               //19.08.2026 Agregamos los efectores de derivacion de los resultados predefinidos
               @" UNION
                    SELECT E.idEfector, E.nombre
                    FROM  Sys_Efector AS E
                    where E.idEfector IN ( SELECT DISTINCT idEfectorDeriva FROM  
                        LAB_ResultadoItem AS RI WHERE RI.baja= 0  
                        and RI.idEfector<> RI.idEfectorDeriva and RI.idEfector= " + oUsuario.IdEfector.IdEfector.ToString() + " ) " +
               "    ORDER BY E.nombre";
            oUtil.CargarCombo(ddlEfectorDestino, m_ssql, "idEfector", "nombre");
            ddlEfectorDestino.Items.Insert(0, new ListItem("--TODOS--", "0"));
        }

        protected void gvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLista.PageIndex = e.NewPageIndex;
            CargarGrilla();
        }
    }
}
