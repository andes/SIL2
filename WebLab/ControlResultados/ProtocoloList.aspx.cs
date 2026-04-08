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
using System.Data.SqlClient;

namespace WebLab.ControlResultados
{
    public partial class ProtocoloList : System.Web.UI.Page
    {
      

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                VerificaPermisos("Calcular Formulas");        
                CargarGrilla();
                MarcarSeleccionados(false);
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
                    //case 1:
                    //    lnkMarcarImpresos.Visible = false; break;
                }
            }
            else Response.Redirect("../FinSesion.aspx", false);
        }

        private void MarcarSeleccionados(bool p)
        {           
            foreach (GridViewRow row in gvLista.Rows )
            {
                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));         
                if (a.Checked==!p)                
                    ((CheckBox)(row.Cells[0].FindControl("CheckBox1"))).Checked = p;                
            }
            
        }

        
        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos();
            gvLista.DataBind();
            PonerImagenes();
        }

        private object LeerDatos()
        {
            /*Filtra los protocolos en proceso: por que debe tener valores cargados para aplicar las formulas*/
            /*  Filtra los protocolos con analisis con formulas a calcular sin resultados*/
            

            //string m_strSQL = @" SELECT    P.idProtocolo, P.numero as numero,  cONVERT(varchar(10),P.fecha,103) as fecha,                               
            //                 CASE 
            //                    WHEN Pa.idestado = 2                                     THEN CAST(Pa.numeroAdic AS varchar(20))
            //                    ELSE         CAST(Pa.numeroDocumento AS varchar(20))
            //                    END AS dni,
            //                    Pa.apellido+ ' ' + Pa.nombre as paciente,
            //                   O.nombre as origen, Pri.nombre as prioridad, SS.nombre as sector,P.estado, P.impreso 
            //                   FROM Lab_Protocolo P with (nolock)
            //                   INNER JOIN Lab_Origen O with (nolock) on O.idOrigen= P.idOrigen
            //                   INNER JOIN Lab_Prioridad Pri with (nolock) on Pri.idPrioridad= P.idPrioridad
            //                   INNER JOIN Sys_Paciente Pa with (nolock) on Pa.idPaciente= P.idPaciente                              
            //                   INNER JOIN LAB_SectorServicio SS with (nolock) ON P.idSector= SS.idSectorServicio 
            //                   INNER JOIN    LAB_DetalleProtocolo AS DP with (nolock) ON P.idProtocolo = DP.idProtocolo
            //                   INNER JOIN   LAB_Item AS I with (nolock) ON DP.idSubItem = I.idItem
            //                   INNER JOIN  LAB_Formula AS F with (nolock) ON I.idItem = F.idItem
            //                   WHERE  (F.idTipoFormula = 1) AND (P.estado =1) AND (DP.conResultado = 0) AND " + Request["Parametros"].ToString(); // +str_orden;

            /*Se reformula sql con mejor preformance para no mostrar protocolos duplicados */
            string m_strSQL = @" SELECT    P.idProtocolo, P.numero as numero,  cONVERT(varchar(10),P.fecha,103) as fecha,                               
                                CASE 
                                    WHEN Pa.idestado = 2  THEN CAST(Pa.numeroAdic AS varchar(20))
                                ELSE     CAST(Pa.numeroDocumento AS varchar(20))
                                END AS dni,
                                 LTRIM(RTRIM(ISNULL(Pa.apellido,'') + ' ' + ISNULL(Pa.nombre,''))) AS paciente,
                               O.nombre as origen, Pri.nombre as prioridad, SS.nombre as sector,P.estado, P.impreso 
                             FROM Lab_Protocolo P WITH (NOLOCK)
                            INNER JOIN Lab_Origen O WITH (NOLOCK) ON O.idOrigen = P.idOrigen
                            INNER JOIN Lab_Prioridad Pri WITH (NOLOCK) ON Pri.idPrioridad = P.idPrioridad
                            INNER JOIN Sys_Paciente Pa WITH (NOLOCK) ON Pa.idPaciente = P.idPaciente
                            INNER JOIN LAB_SectorServicio SS WITH (NOLOCK) ON P.idSector = SS.idSectorServicio
                               WHERE  (P.estado =1) AND " + Request["Parametros"].ToString()+
                               @"   AND EXISTS ( SELECT 1
        FROM LAB_DetalleProtocolo DP WITH (NOLOCK)
     
        INNER JOIN LAB_Formula F WITH (NOLOCK) ON DP.idSubItem = F.idItem
        WHERE DP.idProtocolo = P.idProtocolo and P.idEfector = DP.idEfector
          AND DP.conResultado = 0
          AND F.idTipoFormula = 1 )
                order by P.numero "                               ; 

            
            DataSet Ds = new DataSet();
            //    SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);

            CantidadRegistros.Text = Ds.Tables[0].Rows.Count.ToString() + " protocolos con fórmulas pendientes de calcular ";
             
            return Ds.Tables[0];
        }

     

       

        private string GenerarListaProtocolos()
        {
            string m_lista = "";
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    if (m_lista == "")
                        m_lista += gvLista.DataKeys[row.RowIndex].Value.ToString();
                    else
                        m_lista += "," + gvLista.DataKeys[row.RowIndex].Value.ToString();
                }
            }
            return m_lista;
        }

        

        protected void gvLista_RowDataBound(object sender, GridViewRowEventArgs e)
        {                                   
        }

        private void PonerImagenes()
        {
            foreach (GridViewRow row in gvLista.Rows)
            {                
                    switch (row.Cells[9].Text)
                    {
                        case "0": ///Abierto
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/rojo.gif";
                                row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                        case "1": //en proceso
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/amarillo.gif";
                               row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                        case "2": //terminado
                            {
                                Image hlnk = new Image();
                                hlnk.ImageUrl = "~/App_Themes/default/images/verde.gif";
                                row.Cells[9].Controls.Add(hlnk);
                            }
                            break;
                    }

                     

            }
        }



      

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            PonerImagenes();
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            PonerImagenes();
        }

       

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("ControlPlanilla.aspx?tipo=formula",false);            

        }

        protected void btnCalcularFormula_Click(object sender, EventArgs e)
        {
            int i_idUsuario = int.Parse(Session["idUsuario"].ToString());
            foreach (GridViewRow row in gvLista.Rows)
            {

                CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                if (a.Checked == true)
                {
                    Protocolo oProtocolo = new Protocolo();
                    oProtocolo= (Protocolo) oProtocolo.Get(typeof(Protocolo),int.Parse( gvLista.DataKeys[row.RowIndex].Value.ToString()));
                    oProtocolo.CalcularFormulas("Carga",i_idUsuario,true);
                }
            }
            string popupScript = "<script language='JavaScript'> alert('Las fórmulas se calcularon correctamente. Si alguno de los protocolos no fue cargado con todos los analisis necesarios para el calculo de formula, los analisis con formula no se calcularan. Verifique sus datos.')</script>";
            Page.RegisterClientScriptBlock("PopupScript", popupScript);
            CargarGrilla();

        }
    }
   
}
