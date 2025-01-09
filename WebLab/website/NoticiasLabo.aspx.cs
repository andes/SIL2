using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.website
{
    public partial class NoticiasLabo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                this.BindItemsList();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            CargarGrilla();
            //GridView1.SetPageIndex(0);
        }
        private int CurrentPage
        {
            get
            {
                object objPage = ViewState["_CurrentPage"];
                int _CurrentPage = 0;
                if (objPage == null)
                {
                    _CurrentPage = 0;
                }
                else
                {
                    _CurrentPage = (int)objPage;
                }
                return _CurrentPage;
            }
            set { ViewState["_CurrentPage"] = value; }
        }
        private int fistIndex
        {
            get
            {

                int _FirstIndex = 0;
                if (ViewState["_FirstIndex"] == null)
                {
                    _FirstIndex = 0;
                }
                else
                {
                    _FirstIndex = Convert.ToInt32(ViewState["_FirstIndex"]);
                }
                return _FirstIndex;
            }
            set { ViewState["_FirstIndex"] = value; }
        }
        private int lastIndex
        {
            get
            {

                int _LastIndex = 0;
                if (ViewState["_LastIndex"] == null)
                {
                    _LastIndex = 0;
                }
                else
                {
                    _LastIndex = Convert.ToInt32(ViewState["_LastIndex"]);
                }
                return _LastIndex;
            }
            set { ViewState["_LastIndex"] = value; }
        }


        private DataTable CargarGrilla()
        {


            
            using (DataSet dataset = new DataSet())
            {
                dataset.ReadXml(Server.MapPath("noticias.xml"));
                //GridView1.PageSize = 6; 

                

                DataTable dt = dataset.Tables[0];
                return dt;
                //GridView1.DataSource = dt;     
                //GridView1.DataBind();
        
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
            //GridView1.PageIndex = e.NewPageIndex;

            //int currentPage = GridView1.PageIndex + 1;
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
            //GridView1.SetPageIndex(0);
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
        

            CargarGrilla();
       //     GridView1.SetPageIndex(0);

        }

        PagedDataSource _PageDataSource = new PagedDataSource();
        private void BindItemsList()
        {

            DataTable dataTable = CargarGrilla();
            _PageDataSource.DataSource = dataTable.DefaultView;
            _PageDataSource.AllowPaging = true;
            _PageDataSource.PageSize =8;
            _PageDataSource.CurrentPageIndex = CurrentPage;
            ViewState["TotalPages"] = _PageDataSource.PageCount;

            this.lblPageInfo.Text = "Página " + (CurrentPage + 1) + " de " + _PageDataSource.PageCount;
            this.lbtnPrevious.Enabled = !_PageDataSource.IsFirstPage;
            this.lbtnNext.Enabled = !_PageDataSource.IsLastPage;
            this.lbtnFirst.Enabled = !_PageDataSource.IsFirstPage;
            this.lbtnLast.Enabled = !_PageDataSource.IsLastPage;

            this.GridView1.DataSource = _PageDataSource;
            this.GridView1.DataBind();
            this.doPaging();
        }

        private void doPaging()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");

            fistIndex = CurrentPage - 5;

            if (CurrentPage > 5)
            {
                lastIndex = CurrentPage + 5;
            }
            else
            {
                lastIndex = 10;
            }
            if (lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
            {
                lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
                fistIndex = lastIndex - 10;
            }

            if (fistIndex < 0)
            {
                fistIndex = 0;
            }

            for (int i = fistIndex; i < lastIndex; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            this.dlPaging.DataSource = dt;
            this.dlPaging.DataBind();
        }

        protected void dlPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("Paging"))
            {
                CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
                this.BindItemsList();
            }
        }

        protected void dlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
            if (lnkbtnPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkbtnPage.Enabled = false;
                lnkbtnPage.Style.Add("font-size", "14px");
                lnkbtnPage.Font.Bold = true;

            }
        }

        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            this.BindItemsList();
        }

        protected void lbtnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            this.BindItemsList();

        }

        protected void lbtnLast_Click(object sender, EventArgs e)
        {
            CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
            this.BindItemsList();
        }

        protected void lbtnFirst_Click(object sender, EventArgs e)
        {
            CurrentPage = 0;
            this.BindItemsList();
        }
    }
}
