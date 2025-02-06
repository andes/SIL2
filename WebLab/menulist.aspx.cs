using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;

using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
    public partial class menulist : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            //    autenticarUsuario();
                Cargar();
            }

        }

       

        private void Cargar()
        {
            gvProtocolosxEfector.DataSource = LeerDatos();
            gvProtocolosxEfector.DataBind();
        }

        private object LeerDatos()
        {
            Utility oUtil = new Utility();
            string m_ssql = "select * from sys_menu where idmodulo=2";
            if (DropDownList1.SelectedValue== "sys_menu")            
             m_ssql = "select * from sys_menu where idmodulo=2";
            if (DropDownList1.SelectedValue == "temp_mensaje")
                m_ssql = "select * from temp_mensaje ";

            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_ssql, conn);
            adapter.Fill(Ds);

            return Ds.Tables[0];


        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cargar();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}