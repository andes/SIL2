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
using Business.Data.Laboratorio;
using Business;
using Business.Data;
using NHibernate;
using NHibernate.Expression;

namespace WebLab.Efectores
{
    public partial class EfectorLisRel : System.Web.UI.Page
    {
        Utility oUtil = new Utility(); public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {

            //MiltiEfector: Filtra para configuracion del efector del usuario
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["idUsuario"] != null)
                {
                     VerificaPermisos("Efectores Vinculados");  
                    CargarListas();
                    Buscar();
                    CargarGrilla();

                 
                }
                else Response.Redirect("../FinSesion.aspx", false);
               
            }
        }

        private void CargarListas()
        {
            Utility oUtil = new Utility();

            ///Carga de Sectores
            string m_ssql = "";

         

            ///Carga de hojas de trabajo para histocomptaibilidad
            m_ssql = " select idZona, nombre from Sys_Zona ";
            oUtil.CargarCombo(ddlZona, m_ssql, "idZona", "nombre");
            ddlZona.Items.Insert(0, new ListItem("Todas", "0"));

            if (oUser.IdEfector.IdEfector.ToString() == "227")
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E " +
                     " INNER JOIN lab_Configuracion C on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");

            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E  where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre");
            }

          
            m_ssql = null;
            oUtil = null;
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
             //   Utility oUtil = new Utility();
                Permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (Permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: btnGuardar.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);

        }

        //private void CargarGrilla()
        //{           
        //    //gvLista.AutoGenerateColumns = false;
        //    //gvLista.DataSource = LeerDatos();
        //    //gvLista.DataBind();
        //}

        private void CargarGrilla()
        {
            //string m_strSQL = " select E.idEfector,   E.nombre, case when E.idtipoEfector=2 then 'Privado' else 'Publico' end as publico " +
            //                  " from Sys_efector E"+                                                          
            //                  " order by E.nombre";

            //if (!oUser.Administrador)
            //{
            string m_strSQL = @"select E.idEfector,   E.nombre 
                         from Sys_efector E 	
where  exists (select 1 from LAB_EfectorRelacionado R where idEfectorRel= E.idEfector and R.idEfector=" + ddlEfector.SelectedValue + @")   order by nombre";
            //}
            oUtil.CargarListBox(lstEfectorVinculado, m_strSQL, "idEfector", "nombre");
            lstEfectorVinculado.UpdateAfterCallBack = true;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (lstEfectorVinculado.Items.Count > 0)
            {
                Efector oEfector = new Efector();
                oEfector = (Efector)oEfector.Get(typeof(Efector), "IdEfector", int.Parse(ddlEfector.SelectedValue));

                ///Eliminar los detalles y volverlos a crear
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(EfectorRel));
                crit.Add(Expression.Eq("IdEfector", oEfector));
                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    foreach (EfectorRel oDetalle in detalle)
                    {
                        oDetalle.Delete();
                    }

                }


                if (lstEfectorVinculado.Items.Count > 0)
                {
                    /////Crea nuevamente los detalles.
                    for (int i = 0; i < lstEfectorVinculado.Items.Count; i++)
                    {
                        Efector oEfectorRel = new Efector();
                        oEfectorRel = (Efector)oEfectorRel.Get(typeof(Efector), "IdEfector", int.Parse(lstEfectorVinculado.Items[i].Value));


                        EfectorRel oDetalle = new EfectorRel();
                        oDetalle.IdEfector = oEfector;
                        oDetalle.IdEfectorRel = oEfectorRel;

                        oDetalle.Save();

                    }
                }
                estatus.Text = "Datos guardados";
                estatus.Visible = true;
                estatus.UpdateAfterCallBack = true;
            }
            else
            {
                estatus.Text = "Debe cargar al menos un efector vinculado";
                estatus.Visible = true;
                estatus.UpdateAfterCallBack = true;
            }
        }

     

       
       

        protected void ddlZona_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEfector_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void Buscar()
        {
            string m_strCondicion = " where 1=1";
            string m_strSQL = @"select E.idEfector,   E.nombre as nombre 
   
                         from Sys_efector E ";
            if (ddlZona.SelectedValue != "0")
                m_strCondicion += @" and E.idzona=" + ddlZona.SelectedValue;
            if (txtNombre.Text != "")
                m_strCondicion += @" and upper(E.nombre) like '%" + txtNombre.Text.ToUpper() + "%'";
            m_strSQL = m_strSQL + m_strCondicion + @"  order by nombre";
            

            oUtil.CargarListBox(lstEfector, m_strSQL, "idEfector", "nombre");
            lstEfector.UpdateAfterCallBack = true;
        }

        

        protected void btnSacarTodos_Click(object sender, EventArgs e)
        {
            lstEfectorVinculado.Items.Clear();
            //for (int i = 0; i < lstEfectorVinculado.Items.Count; i++)
            //{
               
            //    lstEfectorVinculado.Items.Remove(lstEfectorVinculado.Items[i]);

            //}
            lstEfectorVinculado.UpdateAfterCallBack = true;
        }

        protected void btnAgregarEfector_Click(object sender, EventArgs e)
        {
            if (lstEfector.SelectedItem.Value != "")
            {
                bool esta = false;
                for (int i = 0; i < lstEfectorVinculado.Items.Count; i++)
                {

                    if (lstEfectorVinculado.Items[i].Value == lstEfector.SelectedItem.Value)
                    { esta = true; break; }
                }
                if (!esta)
                {
                    lstEfectorVinculado.Items.Add(lstEfector.SelectedItem);
                    lstEfectorVinculado.UpdateAfterCallBack = true;
                    estatus.Text = "";
                    estatus.Visible = false;
                    estatus.UpdateAfterCallBack = true;
                }
                else
                {
                    estatus.Text = "El efector ya fue vinculado";
                    estatus.Visible = true;
                    estatus.UpdateAfterCallBack = true;
                }
            }
        }

        protected void btnSacarEfector_Click(object sender, EventArgs e)
        {
            if (lstEfectorVinculado.SelectedValue != "")
            {
                lstEfectorVinculado.Items.Remove(lstEfectorVinculado.SelectedItem);
                lstEfectorVinculado.UpdateAfterCallBack = true;
            }
        }
    }
}
