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
using System.Data.SqlClient;
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;


namespace WebLab.MarcadoresGen
{
    public partial class MarcadoresGenList : System.Web.UI.Page
    {
       public CrystalReportSource oCr = new CrystalReportSource();
        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {


            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                //oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }
       // protected void Page_PreInit(object sender, EventArgs e)
       //{ 
       //    oCr.Report.FileName = "";           
       //    oCr.CacheDuration=0;
       //    oCr.EnableCaching=false;           
       //}
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                        
                VerificaPermisos("Marcadores");
              
                CargarGrilla();
            }
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

        private void CargarGrilla()
        {
           // gvLista.AutoGenerateColumns = false;
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            string m_condicion = "  ";


            string m_strSQL = @" SELECT idTipoMarcador AS idTipoMarcador,  nombre AS nombre
                                 FROM LAB_TipoMarcador  
                                WHERE   baja = 0  " + m_condicion;
                               
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);



            return Ds.Tables[0];
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("MarcadoresGenEdit.aspx");
        }





        protected void gvLista_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
                Response.Redirect("MarcadoresGenEdit.aspx?id=" + e.CommandArgument);
            if (e.CommandName == "Eliminar")
            {
                Eliminar(e.CommandArgument);
                CargarGrilla();
            }
            
        }

    
        private void Eliminar(object p)
        {
            TipoMarcador oRegistro = new TipoMarcador();
            oRegistro = (TipoMarcador)oRegistro.Get(typeof(TipoMarcador), int.Parse(p.ToString()));
            //Usuario oUser = new Usuario();
            oRegistro.Baja = true;
            oRegistro.IdUsuarioRegistro = oUser; // (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oRegistro.FechaRegistro = DateTime.Now;
            oRegistro.Save();
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdModificar = (ImageButton)e.Row.Cells[1].Controls[1];
                CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdModificar.CommandName = "Modificar";

             
                
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[2].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";

                if (Permiso == 1)
                {
                    CmdEliminar.Visible = false;
                    CmdModificar.ToolTip = "Consultar";
                }
            }
        }

      

     

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}
