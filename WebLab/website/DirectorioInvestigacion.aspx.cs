using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.website
{
    public partial class DirectorioInvestigacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                CargarGrilla();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            GridView1.SetPageIndex(0);
        }

     

        private void CargarGrilla()
        {
            Label1.Text = "";
            using (DataSet dataset = new DataSet())
            {
                dataset.ReadXml(Server.MapPath("presentaciones.xml"));
                if (ddlTipo.SelectedValue == "Papers") GridView1.PageSize = 6; else GridView1.PageSize = 5;

                string filtro= " tipo like '%" + ddlTipo.SelectedValue + "%'";

                DataTable dt = dataset.Tables[0];
        
                if (txtNombre.Text.Trim() != "")                    
                        filtro += " and title like '%" + txtNombre.Text.Trim() + "%'";
              

                //if (ddlOrden.SelectedValue== "Titulo")
                //    if (filtro=="")
                //    filtro = "anio like '%" + ddlAnio.SelectedValue + "%'";
                //else
                //        filtro+= " and anio like '%" + ddlAnio.SelectedValue + "%'";

                dt.DefaultView.RowFilter = filtro;
                if (ddlOrden.SelectedValue == "Titulo")  dt.DefaultView.Sort = "title asc";
                else dt.DefaultView.Sort = "anio desc";

                GridView1.DataSource = dt;     
                GridView1.DataBind();
                if (ddlTipo.SelectedValue== "Presentaciones a Congresos")
                Label1.Text= "66 presentaciones";
                if (ddlTipo.SelectedValue == "Proyectos de Investigación")
                    Label1.Text = "10 proyectos";
                if (ddlTipo.SelectedValue == "Publicaciones en Revistas Científicas")
                    Label1.Text = "18 publicaciones";
                if (ddlTipo.SelectedValue == "Capítulos de Libros")
                    Label1.Text = "1 registro";
                //lblCantidad.Text = GridView1.Rows.Count.ToString() + " presentaciones";

            }
        }

        public string GenerateContent(string v)
        {

            if (v == "")
                return "";
            else
            { 
                string[] arr = v.Split(("|").ToCharArray());
           
                    
                    return arr[0].Trim().ToString();
            }

        }

        public string Generateurl(string v)
        {
            return "DirectorioInvestigacion2.aspx?nombre=" + v;
            //if (v == "")
            //    return "";
            //else
            //{
            //    string[] arr = v.Split(("|").ToCharArray());


            //    return  arr[1].Trim().ToString();
            //}


        }
        public bool Generatevisible(string v)
        {
            if (v == "")
                return false;
            else
                return true;


        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;

            int currentPage = GridView1.PageIndex + 1;
            //CurrentPageLabel.Text = "Página " + currentPage.ToString() + " de " + GridView1.PageCount.ToString();
            CargarGrilla();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //HyperLink CmdAnular = (HyperLink)e.Row.Cells[0].FindControl("hplFicha");
            //if (CmdAnular != null)
            //    if (CmdAnular.Text != "") CmdAnular.Text = "algitototttt";


        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
            GridView1.SetPageIndex(0);
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
        

            CargarGrilla();
            GridView1.SetPageIndex(0);

        }
    }
}
