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
using Business.Data.Laboratorio;
using Business.Data;

namespace WebLab.Urgencia 
{
    public partial class UrgenciaList : System.Web.UI.Page
    {
        public Configuracion oCon = new Configuracion();
        public Usuario oUser = new Usuario();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session["idUsuario"] != null)
            {

                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);

            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                Inicializar();

              
            }
        }


        private void Inicializar()
        {
            VerificaPermisos("Control de Urgencias");
            Session["idUsuarioValida"] = null;
            //txtFechaDesde.Value = DateTime.Now.ToShortDateString();
            //txtFechaHasta.Value = DateTime.Now.ToShortDateString();
            CargarGrilla();
            txtNumero.Focus();
         
            if (oCon.PeticionElectronica) PeticionList.CargarPeticiones();



        }

        private int Permiso /*el permiso */
        {
            get { return ViewState["Permiso"] == null ? 0 : int.Parse(ViewState["Permiso"].ToString()); }
            set { ViewState["Permiso"] = value; }
        }

        private void VerificaPermisos(string sObjeto)
        {
            if (Session["idUsuario"] != null)
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
            else Response.Redirect("../FinSesion.aspx", false);
        }
       

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { CargarGrilla(); }
            else
            PintarReferencias();

        }
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();

            gvLista.DataBind();
            PintarReferencias();

        }

        private DataTable LeerDatos()
        {
            //Usuario oUser = new Usuario();
            //Configuracion oCon = new Configuracion();

            //oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
            //oCon = (Configuracion)oCon.Get(typeof(Configuracion),  "IdEfector", oUser.IdEfector);

            string str_condicion = " P.Baja=0 AND P.idPrioridad = 2 and P.idEfector= " + oCon.IdEfector.IdEfector.ToString();
           
            


            if (txtFechaDesde.Value != "") {
                DateTime fecha1 = DateTime.Parse(txtFechaDesde.Value);
                str_condicion += " AND P.fecha>= '" + fecha1.ToString("yyyyMMdd") + "'"; }
            if (txtFechaHasta.Value != "") {
                DateTime fecha2 = DateTime.Parse(txtFechaHasta.Value);
                str_condicion += " AND P.fecha<= '" + fecha2.ToString("yyyyMMdd") + "'"; }
           
            if (ddlEstado.SelectedValue != "-1") str_condicion += " AND P.estado=" + ddlEstado.SelectedValue;
            if (txtNumero.Text != "") str_condicion += " and P.numero='" + txtNumero.Text + "'";
                                
            DataSet Ds = new DataSet();
            //  SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            cmd.CommandText = "[LAB_ListaProtocolo]";

            cmd.Parameters.Add("@FiltroBusqueda", SqlDbType.NVarChar);
            cmd.Parameters["@FiltroBusqueda"].Value = str_condicion;

            cmd.Parameters.Add("@TipoLista", SqlDbType.Int);
            cmd.Parameters["@TipoLista"].Value = 4;  /// tipo de reporte de urgencias

            cmd.Parameters.Add("@idArea", SqlDbType.Int);
            cmd.Parameters["@idArea"].Value = 0;

            cmd.Parameters.Add("@idItem", SqlDbType.Int);
            cmd.Parameters["@idItem"].Value = 0;

            cmd.Parameters.Add("@tipoorden", SqlDbType.NVarChar);          
                cmd.Parameters["@tipoorden"].Value = "DESC";

            cmd.Connection = conn;
            
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(Ds);
            ////////////////////////////////////////////////////////////////////////////////
            
            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() ;



            return Ds.Tables[0];
        }

        private void PintarReferencias()
        {
            int i_noprocesado = 0;
            int i_enproceso = 0;
            int i_terminado = 0;
            int i_impreso = 0;
            foreach (GridViewRow row in gvLista.Rows)
            {
                switch (row.Cells[0].Text)
                {
                    case "0": ///Abierto
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                            row.Cells[0].Controls.Add(hlnk);
                            i_noprocesado += 1;
                        }
                        break;
                    case "1": //en proceso
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/amarillo.gif";
                            row.Cells[0].Controls.Add(hlnk);
                            i_enproceso += 1;
                        }
                        break;
                    case "2": //terminado
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                            row.Cells[0].Controls.Add(hlnk);
                            i_terminado += 1;
                        }
                        break;
                }

                switch (row.Cells[1].Text)
                {
                    case "True":
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/impreso.jpg";
                            hlnk.ToolTip = "Protocolo Impreso";
                            row.Cells[1].Controls.Add(hlnk);
                            i_impreso += 1;
                        }
                        break;
                    case "False":
                        {
                            Image hlnk = new Image();
                            hlnk.ImageUrl = "~/App_Themes/default/images/transparente.jpg";
                            row.Cells[1].Controls.Add(hlnk);
                        }
                        break;

                }
                

            }
            lblNoProcesado.Text = i_noprocesado.ToString();
                lblEnProceso.Text = i_enproceso.ToString();
                lblTerminado.Text = i_terminado.ToString();
                lblImpreso.Text = i_impreso.ToString();

        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Modificar":
                    {                        
                        Session["ListaProtocolo"] = e.CommandArgument.ToString();
                        Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idServicio=1&Operacion=Modifica&idProtocolo=" + e.CommandArgument.ToString() + "&Desde=Urgencia");
                        break;
                    }

                case "Resultado":
                    {                        
                        Session["Parametros"] = e.CommandArgument.ToString();
                        Response.Redirect("../Resultados/ResultadoEdit2.aspx?idServicio=1&Operacion=HC&idProtocolo=" + e.CommandArgument.ToString() + "&Index=0&idArea=0&modo=Urgencia&desde=Urgencia&urgencia=1&validado=0", false);                         
                        break;
                    }


                case "Valida":
                    {
                        Session["Parametros"] = e.CommandArgument.ToString();
                        Response.Redirect("../Resultados/ResultadoEdit2.aspx?idServicio=1&Operacion=Valida&idProtocolo=" + e.CommandArgument.ToString() + "&Index=0&idArea=0&modo=Urgencia&desde=Urgencia&urgencia=1", false);
                        break;
                    }              

                case "Anular":
                    {
                        Anular(e.CommandArgument);                      
                        break;
                    }
            }    
        }
   

        private void Anular(object p)
        {
            Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
            oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(p.ToString()));

            //Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            //oCon = (Configuracion)oCon.Get(typeof(Configuracion), "IdEfector", oRegistro.IdEfector );

            if (oRegistro.Estado == 2)
            {
                if (oCon.EliminarProtocoloTerminado)
                {
                    oRegistro.Baja = true;
                    oRegistro.Save();
                    oRegistro.GrabarAuditoriaProtocolo("Elimina Protocolo", oUser.IdUsuario);  
                    //CargarGrilla();
                }
                else
                {
                    string popupScript = "<script language='JavaScript'> alert('No es posible eliminar un protocolo terminado.')</script>";
                    Page.RegisterClientScriptBlock("PopupScript", popupScript);
                }
            }
            else
            {
                oRegistro.Baja = true;
                oRegistro.Save();
                oRegistro.GrabarAuditoriaProtocolo("Elimina Protocolo", oUser.IdUsuario);  
                //CargarGrilla();
            }
            CargarGrilla();
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton CmdModificar = (ImageButton)e.Row.Cells[11].Controls[1];
                    CmdModificar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdModificar.CommandName = "Modificar";
                    CmdModificar.ToolTip = "Modificar";


                 

                    ImageButton CmdEliminar = (ImageButton)e.Row.Cells[12].Controls[1];
                    CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdEliminar.CommandName = "Anular";
                    CmdEliminar.ToolTip = "Anular";


                    ImageButton CmdResultado = (ImageButton)e.Row.Cells[13].Controls[1];
                    CmdResultado.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdResultado.CommandName = "Resultado";
                    CmdResultado.ToolTip = "Resultados";


                    ImageButton CmdValida = (ImageButton)e.Row.Cells[14].Controls[1];
                    CmdValida.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                    CmdValida.CommandName = "Valida";
                    CmdValida.ToolTip = "Valida";


                    if (Permiso == 1)
                    {
                        CmdEliminar.Visible = false;
                        CmdModificar.ToolTip = "Consultar";
                      
                    }

                    


                    if (oCon.EliminarProtocoloTerminado) CmdEliminar.Visible = true;
                    else
                        CmdEliminar.Visible = false;
                }



            }
              





            }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Protocolos/Default.aspx?urgencia=1", false);
        }

        protected void ddlEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrilla();
        }

        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();

            if (txtNumero.Text != "") { if (oUtil.EsEntero(txtNumero.Text)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }

        protected void cvFechas_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try{
            if (txtFechaDesde.Value != "")
            {                DateTime f1 = DateTime.Parse(txtFechaDesde.Value);   
                args.IsValid=true;
            }
            else
                if (txtFechaHasta.Value != "")
                {
                    DateTime f2 = DateTime.Parse(txtFechaHasta.Value); args.IsValid = true;
                }
                else args.IsValid = true;
        }
            catch(Exception ex)
        {
            string exception = "";
            while (ex != null)
            {
                exception = ex.Message + "<br>";

            } args.IsValid = false;
        }

        }  
        
} 
}
