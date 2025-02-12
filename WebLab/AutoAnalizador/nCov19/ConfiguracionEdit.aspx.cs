﻿using System;
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
using NHibernate.Expression;
using NHibernate;
using Business.Data;
using System.Data.SqlClient;
using Business.Data.AutoAnalizador;

namespace WebLab.AutoAnalizador.nCov19
{
    public partial class ConfiguracionEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Interface Covid");
                CargarCombos();
                CargarGrilla();                
            }
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
                    //case 1: btn .Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
        }

        private object LeerDatos()
        {
            string m_strSQL = @" SELECT  distinct   M.idResultadoItemNcov, I.codigo, R.resultado, M.resultadoEquipo
                                 FROM  LAB_ResultadoItemNcov AS M 
                                inner join lab_resultadoitem R on R.idresultadoitem= M.idresultadoitem
                                 INNER JOIN LAB_Item AS I ON R.idItem = I.idItem   ";

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            //   CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " registros encontrados";

            return Ds.Tables[0];
        }

        private void CargarCombos()
        {
            Utility oUtil = new Utility();

            string m_ssql = "select idArea, nombre from Lab_Area where baja=0 and idtiposervicio=3 order by nombre";
            oUtil.CargarCombo(ddlArea, m_ssql, "idArea", "nombre");

            CargarItem();

            m_ssql = null;
            oUtil = null;
       }


        private void CargarItem()
        {
            Utility oUtil = new Utility();
            ///Carga de combos de Item sin el item que se está configurando y solo las determinaciones simples
            string m_ssql = @"select idItem, nombre + ' - ' + codigo as nombre from Lab_Item 
                where baja=0 AND idEfector=idEfectorDerivacion and   idArea=" + ddlArea.SelectedValue +
                       " order by nombre";

            oUtil.CargarCombo(ddlItem, m_ssql, "idItem", "nombre");
            ddlItem.Items.Insert(0, new ListItem("Seleccione Item", "0"));
            ddlItem.UpdateAfterCallBack = true;
        }


        protected void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarItem();
        }


        private void GuardarDetalleConfiguracion()
        {
            ResultadoItemNcov        oDetalle = new ResultadoItemNcov();
            oDetalle.IdResultadoItem = int.Parse(ddlItemResultado.SelectedValue);
            oDetalle.ResultadoEquipo = txtResultado.Text;
            

            oDetalle.Save();  
        }

       
        protected void btnGuardar_Click2(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string validacion = existe();
                if (validacion == "")
                {
                    lblMensajeValidacion.Text = "";
                    GuardarDetalleConfiguracion();
                    CargarGrilla();
                }
                else
                    lblMensajeValidacion.Text = validacion;
            }
        }

        private string  existe()
        {   
            //////////////////////////////////////////////////////////////////////////////////////////
            ///Verifica de que no exista un item para la combincacion orden y tipo de muestra
            //////////////////////////////////////////////////////////////////////////////////////////
            string hay = "";

            ResultadoItemNcov oItem = new ResultadoItemNcov();
            oItem= (ResultadoItemNcov)oItem.Get(typeof(ResultadoItemNcov), "IdResultadoItem",int.Parse(ddlItemResultado.SelectedValue));
            if (oItem != null)            
                    hay = "Ya existe una vinculación para el resultado seleccionado. Verifique.";
       
            

            return hay;
        }

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton CmdEliminar = (ImageButton)e.Row.Cells[3].Controls[1];
                CmdEliminar.CommandArgument = this.gvLista.DataKeys[e.Row.RowIndex].Value.ToString();
                CmdEliminar.CommandName = "Eliminar";
                CmdEliminar.ToolTip = "Eliminar";             
            }
        }

        protected void gvLista_RowCommand(object sender, GridViewCommandEventArgs e)
        {          
            if (e.CommandName=="Eliminar")
            {
                ResultadoItemNcov oRegistro = new ResultadoItemNcov();
                oRegistro = (ResultadoItemNcov)oRegistro.Get(typeof(ResultadoItemNcov), int.Parse(e.CommandArgument.ToString()));
                oRegistro.Delete();                        
                CargarGrilla();                        
            }            
        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            //Response.Redirect("../PrincipalMindray.aspx", false);
        }


        //protected void chkStatus_OnCheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox chkStatus = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)chkStatus.NamingContainer;
          
        //    int i_id= int.Parse( gvLista.DataKeys[row.RowIndex].Value.ToString());
            
        //    MindrayItem oRegistro = new MindrayItem();
        //    oRegistro = (MindrayItem)oRegistro.Get(typeof(MindrayItem), i_id);
        //    oRegistro.Habilitado = chkStatus.Checked;
        //    oRegistro.Save();                    
        //}

        protected void gvLista_SelectedIndexChanged(object sender, EventArgs e)

        {
           
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarResultadosItem();
        }

        private void CargarResultadosItem()
        {
            Utility oUtil = new Utility();
          
            string m_ssql = @"select idResultadoItem,   resultado from Lab_ResultadoItem 
                where  idItem=" + ddlItem.SelectedValue;

            oUtil.CargarCombo(ddlItemResultado, m_ssql, "idResultadoItem", "resultado");
            ddlItemResultado.Items.Insert(0, new ListItem("Seleccione Resultado", "0"));
            ddlItemResultado.UpdateAfterCallBack = true;
        }
    }
}
