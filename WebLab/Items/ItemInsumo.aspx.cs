using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using Business.Data.AutoAnalizador;
using System.Data;
using System.Data.SqlClient;
using Business;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data.Laboratorio;
using Business.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Drawing;

namespace WebLab.Items
{
    public partial class ItemInsumo : System.Web.UI.Page
    {
     //   private int cantidad = 0;
       // public string mensagitoInicial = ""; string mensagitoProcesa = "";

        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"]!= null)
            oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            else
                Response.Redirect("../FinSesion.aspx", false);


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                    VerificaPermisos("Auditoria de Insumo");


                CargarListas();
            }


        }

        private void CargarListas()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura


            Utility oUtil = new Utility();
            ///Carga de combos de servicios
            ///
            string m_ssql = "select idTipoServicio, nombre from Lab_TipoServicio (nolock) WHERE idTipoServicio<>5 and (baja = 0)";
            oUtil.CargarCombo(ddlServicio, m_ssql, "idTipoServicio", "nombre", connReady);
            ddlServicio.Items.Insert(0, new ListItem("Todos", "0"));
            CargarArea();


            if (oUser.IdEfector.IdEfector.ToString() == "227")
            {
                m_ssql = "select distinct E.idEfector, E.nombre  from sys_efector E (nolock) " +
                     " INNER JOIN lab_Configuracion C (nolock)  on C.idEfector=E.idEfector " +
                     "order by E.nombre";

                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
              //  ddlEfector.Items.Insert(0, new ListItem("Seleccione Efector", "0"));
            }
            else
            {
                m_ssql = "select  E.idEfector, E.nombre  from sys_efector E (nolock)   where E.idEfector= " + oUser.IdEfector.IdEfector.ToString();
                oUtil.CargarCombo(ddlEfector, m_ssql, "idEfector", "nombre", connReady);
            }

          

        }

        private void CargarArea()
        {
            string connReady = ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString; ///Performance: conexion de solo lectura

            Utility oUtil = new Utility();
            string m_ssql = "";
            if (ddlServicio.SelectedValue != "0")
                m_ssql = "select idArea, nombre from Lab_Area (nolock) where baja=0  and idTipoServicio=" + ddlServicio.SelectedValue + " order by nombre";
            else
                m_ssql = "select idArea, nombre from Lab_Area (nolock)  where baja=0  order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre", connReady);
            ddlArea.Items.Insert(0, new ListItem("Todas", "0"));
            ddlArea.UpdateAfterCallBack = true;

        }
        private void VerificaPermisos(string sObjeto)
        {
            if (Session["s_permiso"] != null)
            {
                Utility oUtil = new Utility();
                int i_permiso = oUtil.VerificaPermisos((ArrayList)Session["s_permiso"], sObjeto);
                switch (i_permiso)
                {
                    case 0: Response.Redirect("../AccesoDenegado.aspx", false); break;
                    case 1: Response.Redirect("../AccesoDenegado.aspx", false); break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void CargarGrilla()
        {

            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        //    PonerImagenes();
         
            PintarReferencias();
        }


        private DataTable LeerDatos()
        {
            string m_condicion = "";
          
            if (ddlArea.SelectedValue != "0") m_condicion += " and I.idArea=" + ddlArea.SelectedValue;

            m_condicion += " and Ie.idEfector=" + ddlEfector.SelectedValue;

            if (ddlEstado.SelectedValue != "T")
            {
                if (ddlEstado.SelectedValue == "D")
                    m_condicion += " and Ie.sininsumo=0";
                if (ddlEstado.SelectedValue == "S")
                    m_condicion += " and Ie.sininsumo=1";
                        }



            string m_strSQL = @" select ie.iditemefector, I.codigo, I.nombre, case when sininsumo=0 then 'Disponible' else 'Sin Insumo' end as estado, sininsumo
		from lab_itemefector IE (nolock) 
		inner join lab_item I (nolock)  on IE.iditem= I.iditem
		where 	Ie.disponible=1
        and Ie.idefector=Ie.idefectorderivacion
		and I.baja=0   " + m_condicion + " order by I.codigo ";


            DataSet Ds = new DataSet();
               SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            // 
             lblCantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " determinaciones encontrados";


            return Ds.Tables[0];
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArea();

          
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            PintarReferencias();
        } 
        private void PintarReferencias()
        {
            int celda = 4;
            foreach (GridViewRow row in gvLista.Rows)
            {           
                switch (row.Cells[celda].Text)
                {
                    case "True": //sin insumo
                        {
                            for (int i = 0;  i < gvLista.Columns.Count; i++)
                            { row.Cells[i].BackColor = Color.LightPink;
                            }
                             
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../App_Themes/default/images/rojo.gif";
                            hlnk.ToolTip = "Sin insumo";
                            row.Cells[celda].Controls.Add(hlnk);
                        }
                        break;
                    case "False": //disponible
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../App_Themes/default/images/verde.gif";
                            hlnk.ToolTip = "Disponible";
                            row.Cells[celda].Controls.Add(hlnk);
                        }
                        break;
                   
                }
            }
        }
        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            PintarReferencias();
        }

        private void MarcarSeleccionados(bool p)
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == !p)
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;
            }
        }

     
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
            CargarGrilla();
          
        }

        private void Guardar()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                  
                    ItemEfector oItem = new ItemEfector();
                    oItem = (ItemEfector)oItem.Get(typeof(ItemEfector), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));


                    if (oItem != null)
                    {

                        ISession m_session = NHibernateHttpModule.CurrentSession;
                        ICriteria crit = m_session.CreateCriteria(typeof(ItemEfector));
                        crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                        crit.Add(Expression.Eq("IdItem", oItem.IdItem));
                        IList items = crit.List();
                        foreach (ItemEfector oResultado in items)
                        {
                            if (oResultado.SinInsumo)
                            {
                                oResultado.SinInsumo = false;
                                oResultado.IdUsuarioRegistro = oUser;
                                oResultado.FechaRegistro = DateTime.Now;
                                oResultado.Save();
                                
                                oItem.IdItem.GrabarAuditoriaDetalleItem("Cambia estado", oUser, "Disponible", oUser.IdEfector.Nombre, "");
                            }
                            else
                            {
                                oResultado.SinInsumo = true;
                                oResultado.IdUsuarioRegistro = oUser;
                                oResultado.FechaRegistro = DateTime.Now;
                                oResultado.Save();
                                oItem.IdItem.GrabarAuditoriaDetalleItem("Cambia estado", oUser, "Sin Insumo", oUser.IdEfector.Nombre, "");

                                ActualizarProtocolos(oItem.IdItem);
                            }

                        } // foreach

                        
                    }
                }// chececk
            }// primero        
        }

        private void ActualizarProtocolos(Item  oItem)
        {
            string m_strSQL = @" 	select DP.iddetalleprotocolo
	from lab_detalleprotocolo DP (nolock)
	inner join       LAb_Protocolo P (nolock) on DP.idprotocolo=p.idprotocolo


                where P.baja=0 
                and P.estado<2 
                and P.IdEfector=" + oUser.IdEfector.IdEfector.ToString() + @" and P.fecha>getdate()-3
                and DP.idsubitem="+oItem.IdItem.ToString()+@" 				and DP.idusuariovalida=0				         ";


            DataSet Ds = new DataSet();
               SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
        //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            // 
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
                DetalleProtocolo detalle = new DetalleProtocolo();
                detalle = (DetalleProtocolo)detalle.Get(typeof(DetalleProtocolo), int.Parse(Ds.Tables[0].Rows[i][0].ToString()));
                if (detalle != null)
                {
                    detalle.GuardarSinInsumo();
                    ActualizaEstadoProtocolo(detalle.IdProtocolo);
                }
            }

            //string ssql_Protocolo = @" IdProtocolo in (Select LAb_Protocolo.IdProtocolo 
            //    From LAb_Protocolo 
            //    where LAb_Protocolo.baja=0 
            //    and LAb_Protocolo.estado<2 
            //    and lab_Protocolo.IdEfector=" + oUser.IdEfector.IdEfector.ToString() + " and Lab_Protocolo.fecha>getdate()-5 )";
            //critProtocolo.Add(Expression.Sql(ssql_Protocolo));
            //critProtocolo.Add(Expression.Eq("IdEfector", oUser.IdEfector));
            //critProtocolo.Add(Expression.Eq("IdSubItem", oItem));
            ////Protocolo oUltimoProtocolo = (Protocolo)critProtocolo.List;

            //IList detalle = critProtocolo.List();

            //foreach (DetalleProtocolo oResultado in detalle)
            //{
            //    if (oResultado.IdUsuarioValida==0)
            //        oResultado.GuardarSinInsumo();

            //}
        }

        private void ActualizaEstadoProtocolo(Protocolo oRegistro)
        {
            if (oRegistro.ValidadoTotal("Valida", int.Parse(oUser.IdUsuario.ToString())))
            {
                oRegistro.Estado = 2;  //validado total (cerrado);
                                       //oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                if (!oRegistro.Notificarresultado) // no se notifica a sips
                    oRegistro.Estado = 3; ///Accceso Restringido;
            }
            else
            {
                if (oRegistro.EnProceso())
                {
                    oRegistro.Estado = 1;//en proceso
                                         // oProtocolo.ActualizarResultados(Request["Operacion"].ToString(), int.Parse(Session["idUsuario"].ToString()));
                }
                else
                    oRegistro.Estado = 0;

            }




            oRegistro.Save();
        }

        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }
    }
}