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
using Business.Data.AutoAnalizador;
using System.Drawing;
using NHibernate;

namespace WebLab.AutoAnalizador
{
    public partial class ProtocoloEnviar : System.Web.UI.Page
    {
        int cantidadProtocolos = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                switch (Request["Equipo"].ToString())
                {
                    case "SysmexXS1000":
                        {
                            VerificaPermisos("Sysmex XS1000i");
                            lblTituloEquipo.Text = "Sysmex XS1000i".ToUpper();
                        } break;
                    case "SysmexXT1800":
                        {
                            VerificaPermisos("Sysmex XT1800i");
                            lblTituloEquipo.Text = "Sysmex XT1800i".ToUpper();
                        } break;
                    case "Mindray":
                        {
                            VerificaPermisos("Mindray BS-300");
                            lblTituloEquipo.Text = "Mindray BS-300".ToUpper();
                        } break;

                    case "Stago": 
                        {
                            VerificaPermisos("STA Compact");
                            lblTituloEquipo.Text = "STA Compact".ToUpper();
                        } break;
                }
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
        private void CargarComboSysmexXS1000()
        {
            
            Utility oUtil = new Utility();

            string s_condicion = " P.baja = 0 and DP.idUsuarioValida=0 and " + Request["Parametros"].ToString();


            string m_strSQL = " select distinct  P.idprotocolo, dbo.NumeroProtocolo(P.idProtocolo) AS numero from LAB_Protocolo as  P " +
                "  INNER JOIN                      LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo" +
                " INNER JOIN                      LAB_SysmexItem AS MI ON DP.idSubItem = MI.idItem  " +
                " INNER JOIN                      LAB_Item ON DP.idSubItem = LAB_Item.idItem    " +
                " where " + s_condicion + " ORDER BY numero ";


            oUtil.CargarCombo(ddlProtocolo, m_strSQL, "idProtocolo", "numero");
            ddlProtocolo.Items.Insert(0, new ListItem("Todos", "0"));
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


        private void CargarGrilla()
        {
            gvLista.DataSource = LeerDatos("0");
            gvLista.DataBind();
            ddlProtocolo.Visible = false;
            //if (Request["Equipo"].ToString() == "Mindray")                CargarComboMindray();
            //if (Request["Equipo"].ToString() == "SysmexXS1000")                CargarComboSysmexXS1000();
            //if (Request["Equipo"].ToString() == "SysmexXT1800") CargarComboSysmexXT1800();
            //if (Request["Equipo"].ToString() == "Stago") CargarComboStago();

            PintarReferencias();
            lblCantidadProtocolos.Text = cantidadProtocolos.ToString() + " protocolos encontrados";
            MarcarSeleccionados(false);
        }

        private void CargarComboSysmexXT1800()
        {
            //Utility oUtil = new Utility();

            //string s_condicion = " P.baja = 0 and DP.idUsuarioValida=0 and " + Request["Parametros"].ToString();


            //string m_strSQL = " select distinct  P.idprotocolo, dbo.NumeroProtocolo(P.idProtocolo) AS numero from LAB_Protocolo as  P " +
            //    "  INNER JOIN                      LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo" +
            //    " INNER JOIN                      LAB_SysmexItemxt1800 AS MI ON DP.idSubItem = MI.idItem  " +
            //    " INNER JOIN                      LAB_Item ON DP.idSubItem = LAB_Item.idItem    " +
            //    " where " + s_condicion + " ORDER BY numero ";


            //oUtil.CargarCombo(ddlProtocolo, m_strSQL, "idProtocolo", "numero");
            //ddlProtocolo.Items.Insert(0, new ListItem("Todos", "0"));
        }

        private void CargarComboMindray()
        {
          //  string cantidadProtocolos = Request["Cantidad"].ToString();
            string m_prefijo = "";
            if (Request["Prefijo"].ToString() != "Rutina")
                m_prefijo = Request["Prefijo"].ToString();
           Utility oUtil = new Utility();

           string s_condicion = " P.baja = 0 and idUsuarioValida=0 and " + Request["Parametros"].ToString() + " and MI.tipoMuestra='" + Request["tipoMuestra"].ToString() + "' and MI.prefijo='"+m_prefijo +"'";


           string m_strSQL = " select distinct  P.idprotocolo, dbo.NumeroProtocolo(P.idProtocolo) AS numero from LAB_Protocolo as  P " +
               "  INNER JOIN                      LAB_DetalleProtocolo AS DP ON P.idProtocolo = DP.idProtocolo" +
               " INNER JOIN                      LAB_MindrayItem AS MI ON DP.idSubItem = MI.idItem  " +
               " INNER JOIN                      LAB_Item ON DP.idSubItem = LAB_Item.idItem    " +
               " where "+ s_condicion +" ORDER BY numero "; 


           oUtil.CargarCombo(ddlProtocolo, m_strSQL, "idProtocolo", "numero");
           ddlProtocolo.Items.Insert(0, new ListItem("Todos", "0"));

         
        }

        private void PintarReferencias()
        {

            string numeroPivot = "";

            foreach (GridViewRow row in gvLista.Rows)
            {

                if (numeroPivot != row.Cells[0].Text)
                {
                    for (int j = 0; j < gvLista.Columns.Count; j++)
                    { row.Cells[j].BackColor = Color.Beige; }

                    cantidadProtocolos += 1;  
                }
                else
                    for (int j =1; j <= 2; j++)
                    {
                      
                        row.Cells[j].Text = ""; }

              numeroPivot = row.Cells[0].Text;

                switch (row.Cells[6].Text)
                {
                 
                    case "0": //sin enviar
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../App_Themes/default/images/amarillo.gif";
                            row.Cells[6].Controls.Add(hlnk);
                        }
                        break;
                    case "1": //enviado
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../App_Themes/default/images/verde.gif";
                            row.Cells[6].Controls.Add(hlnk);
                        }
                        break;
                    case "2": //enviado
                        {
                            System.Web.UI.WebControls.Image hlnk = new System.Web.UI.WebControls.Image();
                            hlnk.ImageUrl = "../App_Themes/default/images/verde.gif";
                            row.Cells[6].Controls.Add(hlnk);
                        }
                        break;
                }
                
            }

        }
      
        private object LeerDatos(string s_idProtocolo)
        {
            string s_condicion= Request["Parametros"].ToString();
            if (s_idProtocolo != "0") s_condicion += " and P.idProtocolo=" + s_idProtocolo;
            string m_prefijo = "";
            if (Request["Prefijo"].ToString() != "Rutina")
                m_prefijo = Request["Prefijo"].ToString();

            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[LAB_GeneraProtocolosEnvio]";


            cmd.Parameters.Add("@Equipo", SqlDbType.NVarChar);
            cmd.Parameters["@Equipo"].Value = Request["Equipo"].ToString();

            cmd.Parameters.Add("@Param", SqlDbType.NVarChar);
            cmd.Parameters["@Param"].Value = s_condicion;

            //cmd.Parameters.Add("@Cantidad", SqlDbType.NVarChar);
            //cmd.Parameters["@Cantidad"].Value = Request["Cantidad"].ToString();
          
            cmd.Parameters.Add("@TipoMuestra", SqlDbType.NVarChar);
            cmd.Parameters["@TipoMuestra"].Value = Request["tipoMuestra"].ToString();

            cmd.Parameters.Add("@Prefijo", SqlDbType.NVarChar);
            cmd.Parameters["@Prefijo"].Value =m_prefijo;
                    

            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);
            return Ds.Tables[0];

   
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            EnviarDatos();
       
            //CargarGrilla();
        }

        private void EnviarDatos()
        {
                 int IdMuestra=int.Parse( Request["IDMuestra"].ToString())-1;
                 string m_Prefijo = Request["Prefijo"].ToString();
                 string m_Equipo = Request["Equipo"].ToString();
                LimpiarTablaTemporal();
                string pivot = "";
                string pivotTipoMuestra = "";
                string m_numeroMuestra = pivot;
                string m_listaItem = ""; ;
                string m_paciente = "";
                string m_numero = "";
                string m_muestra = "";
                string m_anioNacimiento = "";
                string m_sexoPaciente = "";
                string m_urgente = "N";
                string m_sectorSolicitante = "";
                string m_listaFinal = "";

                int cantidad = 0;
                foreach (GridViewRow row in gvLista.Rows)
                {            
                    CheckBox a = ((CheckBox)(row.Cells[0].FindControl("CheckBox1")));
                    if (a.Checked == true)
                    {
                        pivot=m_numeroMuestra;
                        pivotTipoMuestra = m_muestra;

                        m_numero = row.Cells[0].Text; // gvLista.DataKeys[row.RowIndex].Value.ToString();                  
                        m_muestra = row.Cells[4].Text; // gvLista.DataKeys[row.RowIndex].Value.ToString();  

                        if (m_Equipo == "Mindray")
                        {
                            if (m_Prefijo.Trim() != "Rutina")
                                m_numeroMuestra = m_numero + "-" + m_muestra.Substring(0, 1).ToUpper() + "-" + m_Prefijo.ToUpper();
                            else
                                m_numeroMuestra = m_numero + "-" + m_muestra.Substring(0, 1).ToUpper();
                        }
                        //if ((m_Equipo == "SysmexXS1000")|| (m_Equipo == "SysmexXT1800"))
                        if (m_Equipo != "Mindray")
                            m_numeroMuestra = m_numero.ToUpper();                        
                       
                        
                        bool grabar = false;
                        if (pivot != m_numeroMuestra)
                        {
                             cantidad += 1;                              
                             m_listaFinal = m_listaItem;
                             m_listaItem = "";
                             if (pivot != "")
                             {
                                 IdMuestra += 1;
                                 grabar = true;
                             }
                        }
                        
                        if (grabar)
                        {
                             //////INSERTAR LOS ANALISIS EN LA TABLA TEMPORAL LAB_MINDRAYPROTOCOLO
                             ProtocoloEnvio oRegistro = new ProtocoloEnvio();
                             oRegistro.IdMuestra = IdMuestra;
                             oRegistro.NumeroProtocolo = pivot;
                             oRegistro.Iditem = m_listaFinal;
                             oRegistro.Paciente = m_paciente;
                             oRegistro.AnioNacimiento = m_anioNacimiento;
                             oRegistro.Sexo = m_sexoPaciente;
                             oRegistro.SectorSolicitante = m_sectorSolicitante;
                             oRegistro.MedicoSolicitante = "";
                             oRegistro.TipoMuestra = pivotTipoMuestra;
                             oRegistro.Urgente = m_urgente;
                             oRegistro.Equipo = m_Equipo;
                             oRegistro.Save();
                             ////////////////////////////////////////////
                        }

                        string m_idDetalleProtocolo = gvLista.DataKeys[row.RowIndex].Value.ToString();
                                               
                        
                        DetalleProtocolo oDetProtocolo = new DetalleProtocolo();
                        oDetProtocolo = (DetalleProtocolo)oDetProtocolo.Get(typeof(DetalleProtocolo), int.Parse(gvLista.DataKeys[row.RowIndex].Value.ToString()));
                        oDetProtocolo.Enviado = 1;
                        oDetProtocolo.IdUsuarioEnvio = int.Parse(Session["idUsuario"].ToString());
                        oDetProtocolo.FechaEnvio = DateTime.Now;

                        oDetProtocolo.Save();
                        if (m_Equipo == "Mindray") m_paciente = oDetProtocolo.IdProtocolo.IdPaciente.Apellido + " " + oDetProtocolo.IdProtocolo.IdPaciente.Nombre;
                        if (m_Equipo != "Mindray") m_paciente = oDetProtocolo.IdProtocolo.IdPaciente.NumeroDocumento.ToString() + " - " + oDetProtocolo.IdProtocolo.IdPaciente.Apellido + " " + oDetProtocolo.IdProtocolo.IdPaciente.Nombre;
                        //if ((m_Equipo == "SysmexXS1000") || (m_Equipo == "SysmexXT1800")) m_paciente = oDetProtocolo.IdProtocolo.IdPaciente.NumeroDocumento.ToString() + " - " + oDetProtocolo.IdProtocolo.IdPaciente.Apellido + " " + oDetProtocolo.IdProtocolo.IdPaciente.Nombre;

                        if (m_Equipo == "Mindray") m_anioNacimiento = oDetProtocolo.IdProtocolo.IdPaciente.FechaNacimiento.Year.ToString();
                        if (m_Equipo != "Mindray") m_anioNacimiento = oDetProtocolo.IdProtocolo.IdPaciente.FechaNacimiento.ToString("yyyyMMdd");
                        //if ((m_Equipo == "SysmexXS1000") || (m_Equipo == "SysmexXT1800")) m_anioNacimiento = oDetProtocolo.IdProtocolo.IdPaciente.FechaNacimiento.ToString("yyyyMMdd");

                        m_sexoPaciente = oDetProtocolo.IdProtocolo.Sexo; if (m_sexoPaciente=="I") m_sexoPaciente="O";                    

                        m_urgente ="N";
                        if (oDetProtocolo.IdProtocolo.IdPrioridad.IdPrioridad==2)  m_urgente ="Y";

                        m_sectorSolicitante = oDetProtocolo.IdProtocolo.IdSector.Nombre;                    

                        int i_idItem = oDetProtocolo.IdSubItem.IdItem;                  
                        
                        ////busca en el analisis en el mindray para sacar tipo de muestra y numero de analisis 
                        if (m_Equipo == "Mindray")
                        {                                                    
                            MindrayItem oItemMindray = new MindrayItem();
                            oItemMindray = (MindrayItem)oItemMindray.Get(typeof(MindrayItem), "IdItem", i_idItem);

                            if (oItemMindray != null)
                            {
                                if (m_listaItem == "")
                                    m_listaItem = oItemMindray.IdMindray.ToString();
                                else
                                    m_listaItem +="|"+ oItemMindray.IdMindray.ToString();
                            }
                       }
                        ////busca en el analisis en el sysmex para sacar identificación de analisis 


                        if (m_Equipo == "Stago")
                        {
                            StaCompactItem oItemEquipo = new StaCompactItem();
                            oItemEquipo = (StaCompactItem)oItemEquipo.Get(typeof(StaCompactItem), "IdItem", i_idItem);
                            if (oItemEquipo != null)
                            {
                                if (m_listaItem == "")
                                    m_listaItem = oItemEquipo.IdstaCompac.ToString();
                                else
                                    m_listaItem += "|" + oItemEquipo.IdstaCompac.ToString();

                                
                            }
                        }

                        if (m_Equipo == "SysmexXS1000")
                       {
                            SysmexItemXS1000 oItemSysmex = new SysmexItemXS1000();
                            oItemSysmex = (SysmexItemXS1000)oItemSysmex.Get(typeof(SysmexItemXS1000), "IdItem", i_idItem);
                            if (oItemSysmex != null)
                            {
                                 if (m_listaItem == "")
                                     m_listaItem = oItemSysmex.IdSysmex.ToString();
                                 else
                                     m_listaItem += "|" + oItemSysmex.IdSysmex.ToString();

                                 //switch (oItemSysmex.IdSysmex)
                                 //{
                                 //    case "NEUT%": m_listaItem += "|NEUT#"; break;
                                 //    case "LYMPH%": m_listaItem += "|LYMPH#"; break;
                                 //    case "MONO%": m_listaItem += "|MONO#"; break;
                                 //    case "EO%": m_listaItem += "|EO#"; break;
                                 //    case "BASO%": m_listaItem += "|BASO#"; break;
                                 //}
                            }
                       }

                         if  (m_Equipo == "SysmexXT1800")
                         {

                             SysmexItemxt1800 oItemSysmex = new SysmexItemxt1800();
                             oItemSysmex = (SysmexItemxt1800)oItemSysmex.Get(typeof(SysmexItemxt1800), "IdItem", i_idItem);
                             if (oItemSysmex != null)
                             {
                                 if (m_listaItem == "")
                                     m_listaItem = oItemSysmex.IdSysmex.ToString();
                                 else
                                     m_listaItem += "|" + oItemSysmex.IdSysmex.ToString();

                                 //switch (oItemSysmex.IdSysmex)
                                 //{
                                 //    case "NEUT%": m_listaItem += "|NEUT#"; break;
                                 //    case "LYMPH%": m_listaItem += "|LYMPH#"; break;
                                 //    case "MONO%": m_listaItem += "|MONO#"; break;
                                 //    case "EO%": m_listaItem += "|EO#"; break;
                                 //    case "BASO%": m_listaItem += "|BASO#"; break;
                                 //}
                             }
                         }
                    }
            }

            if (m_numeroMuestra!= "")
            //if ((pivot == m_numeroMuestra)&&(pivot !=""))
            {
               IdMuestra += 1;
               //////INSERTAR LOS ANALISIS EN LA TABLA TEMPORAL LAB_MINDRAYPROTOCOLO
               ProtocoloEnvio oRegistro = new ProtocoloEnvio();
               oRegistro.NumeroProtocolo = m_numeroMuestra;
               oRegistro.IdMuestra = IdMuestra;
               oRegistro.Iditem = m_listaItem;
               oRegistro.Paciente = m_paciente;
               oRegistro.AnioNacimiento = m_anioNacimiento;
               oRegistro.Sexo = m_sexoPaciente;
               oRegistro.SectorSolicitante = m_sectorSolicitante;
               oRegistro.MedicoSolicitante = "";
               oRegistro.TipoMuestra = m_muestra;
               oRegistro.Urgente = m_urgente;
               oRegistro.Equipo = m_Equipo;
               oRegistro.Save();
              
                ////////////////////////////////////////////
            }
            Response.Redirect("EnvioMensaje.aspx?Cantidad=" + cantidad.ToString() + "&Equipo=" + Request["Equipo"].ToString(), false);
        }

        private void LimpiarTablaTemporal()
        {
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloEnvio));            
            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (ProtocoloEnvio oDetalle in detalle)
                {                                                                         
                    if (oDetalle.Equipo==Request["Equipo"].ToString())
                    oDetalle.Delete();
                }

            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGrilla(); 
            
        }

        protected void lnkMarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(true);
            PintarReferencias();
        }

        protected void lnkDesmarcar_Click(object sender, EventArgs e)
        {
            MarcarSeleccionados(false);
            PintarReferencias();
        }

        protected void ddlProtocolo_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            gvLista.DataSource = LeerDatos(ddlProtocolo.SelectedValue);
            gvLista.DataBind();                    

            PintarReferencias();
            lblCantidadProtocolos.Text = cantidadProtocolos.ToString() + " protocolos encontrados";
            MarcarSeleccionados(true);
        }
        

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            
            Response.Redirect("ProtocoloBusqueda.aspx?Equipo=" + Request["Equipo"].ToString(), false);
        }
    }
}
