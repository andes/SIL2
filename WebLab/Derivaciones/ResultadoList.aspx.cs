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

namespace WebLab.Derivaciones
{
    public partial class ResultadoList : System.Web.UI.Page
    {
     

       

        protected void Page_Load(object sender, EventArgs e)
        {
          
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
            string str_condicion = " 1=1 ";

       
            if (txtDni.Value != "") str_condicion += " AND dni= '" + txtDni.Value + "'";
            if (txtApellido.Text != "") str_condicion += " AND apellido like '%" + txtApellido.Text.TrimEnd() + "%'";
            if (txtNombre.Text != "") str_condicion += " AND nombre like '%" + txtNombre.Text.TrimEnd() + "%'";
           

            /////////////
            ///Si no fue enviado muestra la observacion sino el resultado
          string m_strSQL = " SELECT estadoDerivacion , numero , convert(varchar(10),fecha,103) as fecha, dni, apellido + ' ' + nombre as paciente, determinacion, efectorDerivacion, " +
                            " case when estadoDerivacion=1 then resultado else observacion end as resultado "+
                            " FROM [vta_LAB_DerivacionesEnviadas] " +
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


        

    }
}
