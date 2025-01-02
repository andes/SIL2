using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.website
{
    public partial class DirectorioInvestigacion2 : System.Web.UI.Page
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
            using (DataSet dataset = new DataSet())
            {
                dataset.ReadXml(Server.MapPath("presentaciones.xml"));
                //if (ddlTipo.SelectedValue == "Papers") GridView1.PageSize = 6; else GridView1.PageSize = 5;

               

                DataTable dt = dataset.Tables[0];
        
                      
                string        filtro = "  title like '%" + Request["nombre"].ToString() + "%'";
              

                //if (ddlOrden.SelectedValue== "Titulo")
                //    if (filtro=="")
                //    filtro = "anio like '%" + ddlAnio.SelectedValue + "%'";
                //else
                //        filtro+= " and anio like '%" + ddlAnio.SelectedValue + "%'";

                dt.DefaultView.RowFilter = filtro;
              
                

                GridView1.DataSource = dt;     
                GridView1.DataBind();
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
            if (v == "")
                return "";
            else
            {
                string[] arr = v.Split(("|").ToCharArray());


                return  arr[1].Trim().ToString();
            }


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
