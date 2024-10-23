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
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;

namespace WebLab.CasoFiliacion
{
    public partial class FiliacionMenu : System.Web.UI.Page
    {
        /*Menu principal que lleva a nuevo caso y a lista de casos*/
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
          
            {
            
                VerificaPermisos("Casos Forense");
                btnNuevoCaso.Visible = VerificaPermisosObjeto("Crear Caso");
                Session["idCaso"] = "0";

            }
        }


        private bool VerificaPermisosObjeto(string v)
        {
            bool i = false;

            Utility oUtil = new Utility();
            Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], v);
            switch (Permiso)
            {
                case 0: i = false; break;
                case 1:
                    i = true; break;
                case 2: i = true; break;

            }

            return i;

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
                    case 1:
                        {
                            btnNuevoCaso.Enabled = false;
                            btnNuevoCaso0.Visible = false;
                        } break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        

       


    

        protected void btnNuevoCaso_Click(object sender, EventArgs e)
        {
            string casos = HayCasosVacios();
            if (casos!="")
            {
                lblMensaje.Text = "Los siguientes casos fueron generados sin muestras: " + casos + ". Reutilice algunos de esos, antes de continuar generando nuevos casos.";
                lblMensaje.Visible = true;
            }
            else

            {
                HttpContext Context;

                Context = HttpContext.Current;

                Server.Transfer("CasoNew.aspx");
            }

        }

        private string  HayCasosVacios()
        {
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            string casos = "";

            string m_strSQL = @"select idCasoFiliacion from LAB_CasoFiliacion where baja=0 and idCasoFiliacion not in( select idCasoFiliacion from LAB_CasoFiliacionProtocolo)";

            DataSet Ds = new DataSet();

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                if (casos == "")
                    casos = Ds.Tables[0].Rows[i][0].ToString();
                else

                    casos +=","+ Ds.Tables[0].Rows[i][0].ToString();
            }
            return casos;
            //if (Ds.Tables[0].Rows.Count > 0)
               
            //    return true;
            //else return false;
        }

        protected void btnNuevoCaso0_Click(object sender, EventArgs e)
        {
            HttpContext Context;

            Context = HttpContext.Current;
            Context.Items.Add("idServicio", "6");
            Server.Transfer("CasoList.aspx");
           

        }

     
    }
}
