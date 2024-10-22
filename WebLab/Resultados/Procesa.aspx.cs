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

namespace WebLab.Resultados
{
    public partial class WebForm1 : System.Web.UI.Page
    {    
        
        protected void Page_PreInit(object sender, EventArgs e)
        {
          
           

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO") IdentificarSSO();
                
                if (Session["idUsuario"] != null)
                {
                    if (Request["ModoCarga"].ToString() == "ANDE")
                        Nuevaforma();
                    else
                    {
                        //if (Request["ModoCarga"].ToString() == "ANDA")
                        //    Nuevaforma2();
                        //else
                            CargarGrilla();
                    }
               
                    this.hypRegresar.NavigateUrl = "ResultadoBusqueda.aspx?idServicio="+ Request["idServicio"].ToString() +"&Operacion=" + Request["Operacion"].ToString() + "&modo=" + Request["modo"].ToString();
                    if ((Request["validado"].ToString() == "1") && (Request["Operacion"].ToString() == "HC"))
                        this.hypRegresar.NavigateUrl = "../Informes/historiaClinicafiltro.aspx?Tipo=PacienteValidado";
                    if ((Request["validado"].ToString() == "0") && (Request["Operacion"].ToString() == "HC"))
                        this.hypRegresar.NavigateUrl = "../Informes/historiaClinicafiltro.aspx?Tipo=PacienteCompleto";
                }
                else
                    Response.Redirect("../FinSesion.aspx", false);
                
            }
            
            

        }

        private void Nuevaforma()
        {
            if (Request["idItem"] != "0")
                Response.Redirect("ResultadoxNro.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString(), false);
            else
                lblTitulo.Text = "Debe seleccionar una Determinacion a informar";
        }

        //private void Nuevaforma2()
        //{
        //    if (Request["idItem"] != "0")
        //        Response.Redirect("ResultadoDesdeEquipo.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString(), false);
        //    else
        //        lblTitulo.Text = "Debe seleccionar una Determinacion a informar";
        //    //lblTitulo.Text = "En construccion";
        //}

        private void CargarGrilla()
        {   
            string param=  Request["Parametros"].ToString();
            param = param.Replace("prefijoSector", "prefijoSector +");
          int tar= param.IndexOf("S.numeroTarjeta");





            string ordenProtocolo = "";
            //Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            //if (oC.TipoNumeracionProtocolo == 0)
                //m_strSQL += " order by  numero ";
                ordenProtocolo += ", P.numero ";
            //if (oC.TipoNumeracionProtocolo == 1)
            //    ordenProtocolo += " ,  P.numerodiario ";
            //if (oC.TipoNumeracionProtocolo == 2)
            //    ordenProtocolo += ",P.prefijosector, P.numerosector ";
            //if (oC.TipoNumeracionProtocolo == 3)
            //    ordenProtocolo += ",P.numeroTipoServicio ";


            string m_strSQL = " Select distinct  P.idProtocolo " + ordenProtocolo +// +  // ,  dbo.NumeroProtocolo(P.idProtocolo) as numero, P.fecha 
                                      "  from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                        " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                        //" INNER JOIN LAB_iTEM I on i.iditem= dp.idsubitem " +
                                        " WHERE P.baja=0 and " + param; // Request["Parametros"].ToString();

            if (tar > -1)
            {
                m_strSQL = " Select distinct   P.idProtocolo " + ordenProtocolo +  // ,  dbo.NumeroProtocolo(P.idProtocolo) as numero, P.fecha 
                             "  from Lab_Protocolo P with (nolock) " + // +str_condicion;
                               " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                             " left join [LAB_SolicitudScreening] as S with (nolock) on S.idProtocolo= P.idprotocolo " +
                               " WHERE P.baja=0 and " + param;//Request["Parametros"].ToString();
            }

            if (Request["dni"]!=null){/// si la busqueda es po paciente
                if (Request["dni"].ToString() != "")
                {
                     string numeroDocumento= Request["dni"].ToString();
                     m_strSQL = " Select  distinct  P.idProtocolo " + ordenProtocolo + //,  dbo.NumeroProtocolo(P.idProtocolo) as numero, P.fecha 
                                " from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                                " INNER JOIN SYS_Paciente Pac with (nolock) ON P.idPaciente= Pac.idPaciente" +
                                " WHERE P.baja=0 and " + /*Request["Parametros"].ToString()*/ param + " and Pac.idEstado<>2 and Pac.numeroDocumento=" + numeroDocumento;

                     if (tar > -1)
                     {
                         m_strSQL = " Select distinct   P.idProtocolo " + ordenProtocolo + //,  dbo.NumeroProtocolo(P.idProtocolo) as numero, P.fecha 
                                  " from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                  " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                                  " left join [LAB_SolicitudScreening] as S with (nolock) on S.idProtocolo= P.idprotocolo " +
                                  " INNER JOIN SYS_Paciente Pac with (nolock) ON P.idPaciente= Pac.idPaciente" +
                                  " WHERE P.baja=0 and " +param /* Request["Parametros"].ToString()*/ + " and Pac.idEstado<>2 and Pac.numeroDocumento=" + numeroDocumento;
                     }
                }
            }

            if ((Request["ModoCarga"].ToString() == "HT") || (Request["ModoCarga"].ToString() == "AN"))
            {
                m_strSQL = " Select  distinct P.idProtocolo " + ordenProtocolo +
                                 " from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                 " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                                 " WHERE P.baja=0 and " + param;//Request["Parametros"].ToString();
                
                if (tar > -1)/// si busca una tarjeta hace join con SolicitudScreening
                {
                    m_strSQL = " Select distinct  P.idProtocolo " + ordenProtocolo +
                                   " from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                   " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                                   " left join [LAB_SolicitudScreening] as S with (nolock) on S.idProtocolo= P.idprotocolo " +
                                   " WHERE P.baja=0 and " + param; // Request["Parametros"].ToString();
                }

                if (Request["dni"] != null)
                {/// si la busqueda es po paciente
                    if (Request["dni"].ToString() != "")
                    {
                        string numeroDocumento = Request["dni"].ToString();
                        m_strSQL = " Select distinct   P.idProtocolo " + ordenProtocolo +
                                   " from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                   " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                                   //" left join [LAB_SolicitudScreening] as S on S.idProtocolo= P.idprotocolo " +
                                   " INNER JOIN SYS_Paciente Pac with (nolock) ON P.idPaciente= Pac.idPaciente" +
                                   " WHERE P.baja=0 and " +param /* Request["Parametros"].ToString()*/ + " and Pac.idEstado<>2 and Pac.numeroDocumento=" + numeroDocumento;


                        if (tar > -1)/// si busca una tarjeta hace join con SolicitudScreening
                        {
                            m_strSQL = " Select   distinct P.idProtocolo " + ordenProtocolo +
                                    " from Lab_Protocolo P with (nolock) " + // +str_condicion;
                                    " INNER JOIN lAB_DetalleProtocolo DP with (nolock) on DP.idProtocolo= P.idProtocolo " +
                                    " left join [LAB_SolicitudScreening] as S with (nolock) on S.idProtocolo= P.idprotocolo " +
                                    " INNER JOIN SYS_Paciente Pac with (nolock) ON P.idPaciente= Pac.idPaciente" +
                                    " WHERE P.baja=0 and " + param /* Request["Parametros"].ToString()*/ + " and Pac.idEstado<>2 and Pac.numeroDocumento=" + numeroDocumento;
                        }
                    }
                }
            }          

            if (Request["idServicio"].ToString() != "0") m_strSQL = m_strSQL + " and P.idtipoServicio=" + Request["idServicio"].ToString();
            //if ((Request["Operacion"].ToString() == "Carga") && (Request["ModoCarga"].ToString() == "HT"))
            if  (Request["ModoCarga"].ToString() == "HT")
            {
                m_strSQL += " and DP.idsubItem in (select iditem from LAB_DetalleHojaTrabajo with (nolock) where idHojaTrabajo=" + Request["idHojaTrabajo"].ToString() + ")";
            }
            
         //   if ((Request["Operacion"].ToString() == "Carga") && (Request["ModoCarga"].ToString() == "AN"))
            if  (Request["ModoCarga"].ToString() == "AN")
            {
                m_strSQL += " and DP.idsubItem=" + Request["idItem"].ToString();             
            }

            string masterPE = "";
            if (Request["master"] != null) /// si es desde PE solo los protocolos pedidos por la web
            {
                if (Request["master"] == "1") //pedidos de externos
                {
                    masterPE = "&master=1";
                    //m_strSQL += " and P.estado<3"; // no muestra los protocolos bloqueados
                }
                else //pedidos de consultas internos
                {
                    masterPE = "&master=2";
                  
                }
                //m_strSQL += " and P.idProtocolo in (select idProtocolo from LAB_Peticion where baja=0)";
            }

            if (Request["Operacion"].ToString() == "HC")
            {
                //if ((Request["Operacion"].ToString() == "HC") && (Request["validado"] == "0"))  // solo se visualiza lo del efector
                //    m_strSQL = m_strSQL + " and P.idefector=" + oCon.IdEfector.IdEfector.ToString();


                if (Request["Tipo"] != null)
                {
                    if (Request["Tipo"].ToString() == "PacienteForense")
                        m_strSQL += " and P.idTipoServicio=6";  // en el caso de forense no permitir la consulta de resultados
                    else
                        m_strSQL += " and P.idTipoServicio<>6";  // en el caso de forense no permitir la consulta de resultados
                }
                    m_strSQL += " order by P.idProtocolo desc "; // desde el mas reciente al mas antiguo

                }
            else

                m_strSQL += Request["orden"].ToString();
            //    else
            //    {
            //       //   Configuracion oC = new Configuracion(); oC = (Configuracion)oC.Get(typeof(Configuracion), "IdConfiguracion", 1);
            //            if (oC.TipoNumeracionProtocolo == 0)
            //                //m_strSQL += " order by  numero ";
            //                m_strSQL += " order by  P.numero ";
            //            if (oC.TipoNumeracionProtocolo == 1)
            //                m_strSQL += " order by  P.numerodiario ";
            //            if (oC.TipoNumeracionProtocolo == 2)
            //                m_strSQL += " order by P.prefijosector, P.numerosector ";
            //            if (oC.TipoNumeracionProtocolo == 3)
            //                m_strSQL += " order by P.numeroTipoServicio ";
            //}







            DataSet Ds = new DataSet();
            //   SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            Session["MostrarAnterior"] = "0";
            string m_listaProtocolo = "";
            if (Ds.Tables[0].Rows.Count > 0)
            {
                int cantidadRegistros = Ds.Tables[0].Rows.Count;
                if (cantidadRegistros <= 5000)
                {
                    for (int i = 0; i < cantidadRegistros; i++)
                    {
                        if (m_listaProtocolo == "")
                            m_listaProtocolo = Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                        else
                            m_listaProtocolo += "," + Ds.Tables[0].Rows[i].ItemArray[0].ToString();
                    }
                    string primerProtocolo = Ds.Tables[0].Rows[0].ItemArray[0].ToString();
                    Session["Parametros"] = m_listaProtocolo;

                    switch (Request["Operacion"].ToString())
                    {
                        case "Carga":
                            {
                                switch (Request["ModoCarga"].ToString())
                                {
                                    case "LP":  ///Lista de Protocolos
                                        //                                    Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&Parametros=" + m_listaProtocolo + "&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString()); break;
                                        Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString()); break;
                                    case "HT": ///Por hoja de trabajo
                                        {
                                            if (Request["control"] != null)
                                            {
                                                Response.Redirect("ResultadoHTEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idArea=" + Request["idArea"].ToString() + "&idHojaTrabajo=" + Request["idHojaTrabajo"].ToString() + "&control=1", false); break;
                                            }
                                            else
                                            {
                                                Response.Redirect("ResultadoHTEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idArea=" + Request["idArea"].ToString() + "&idHojaTrabajo=" + Request["idHojaTrabajo"].ToString(), false); break;
                                            }
                                        }
                                    case "AN":
                                        Response.Redirect("ResultadoItemEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString(), false); break;
                                    case "ANDA":
                                        Response.Redirect("ResultadoItemEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString(), false); break;
                                }
                            } break;

                        case "Control": ////Control
                            switch (Request["ModoCarga"].ToString())
                            {
                                case "LP":  ///Lista de Protocolos
                                    Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false); break;
                                case "HT": ///Por hoja de trabajo
                                    {

                                        Response.Redirect("ResultadoHTEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idArea=" + Request["idArea"].ToString() + "&idHojaTrabajo=" + Request["idHojaTrabajo"].ToString() + "&control=1", false); break;

                                    }
                            }
                            break;
                        case "Valida": ////Validación
                            switch (Request["ModoCarga"].ToString())
                            {
                                case "LP":  ///Lista de Protocolos
                                    Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false); break;
                                case "AN":
                                    Response.Redirect("ResultadoItemEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString(), false); break;
                                case "HT": ///Por hoja de trabajo
                                    {

                                        Response.Redirect("ResultadoHTEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idArea=" + Request["idArea"].ToString() + "&idHojaTrabajo=" + Request["idHojaTrabajo"].ToString() + "&control=1", false); break;

                                    }
                                case "ANDA":
                                    Response.Redirect("ResultadoItemEdit.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idItem=" + Request["idItem"].ToString() + "&modo=" + Request["modo"].ToString(), false); break;
                            }
                            break;
                        case "HC": //Historia Clinica
                            if (Request["validado"].ToString() == "0")
                                if (Request["Desde"] != null)
                                    Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString() + "&Desde=" + Request["Desde"].ToString(), false);
                                else

                                    Response.Redirect("ResultadoEdit2.aspx?idServicio=" + Request["idServicio"].ToString() + "&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
                            else
                            {
                               

                                string g = Request["idServicio"].ToString();
                                if (Request["idServicio"].ToString() == "5")
                                    Response.Redirect("ResultadoNoPacienteview.aspx?idServicio=0&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString(), false);
                                else
                                {

                                    Session["idProtocolo"] = primerProtocolo;
                                    Session["validado"] = Request["validado"].ToString();
                                    Session["modo"] = Request["modo"].ToString();





                                    Response.Redirect("Resultadoview.aspx?Index=0&Operacion=HC"+ masterPE,false);

                                   

                                    //Response.Redirect("Resultadoview.aspx?idServicio=0&Operacion=" + Request["Operacion"].ToString() + "&idProtocolo=" + primerProtocolo + "&Index=0&idArea=" + Request["idArea"].ToString() + "&validado=" + Request["validado"].ToString() + "&modo=" + Request["modo"].ToString() + masterPE, false);
                                }
                            }
                            break;
                    }
                    }
                else lblTitulo.Text = "La búsqueda ha superado el límite de procesamiento para la operación que desea realizar. Acote los filtros de búsqueda. Si cree que este mensaje es un error, póngase en contacto con el soporte del SIL.";
                }
            

            else
                lblTitulo.Text = "No se encontraron protocolos para los filtros ingresados";

        }



        private int IdentificarUsuarioSSO()
        {
            Salud.Security.SSO.SSOHelper.Authenticate();
            if (Salud.Security.SSO.SSOHelper.CurrentIdentity == null)
            {
                // 1.1. No lo está. Debe redirigir al sitio de SSO
                // Redirigir ...
                return 0;
                //pnlSinUsuario.Visible = true;
            }
            else
            {
                return Salud.Security.SSO.SSOHelper.CurrentIdentity.Id;

            }
        }

        private void CrearLogAcceso(int idUsuarioLogueado)
        {
            LogAcceso RegistroAcceso = new LogAcceso();
            RegistroAcceso.IdUsuario = idUsuarioLogueado;
            RegistroAcceso.Fecha = DateTime.Now;
            RegistroAcceso.Save();
        }

        private void CrearPermisos(int p)
        {
            string m_strSQL = " SELECT  M.objeto, P.permiso " +
                              " FROM Sys_Perfil " +
                              " INNER JOIN   Sys_Usuario AS U ON Sys_Perfil.idPerfil = U.idPerfil " +
                              " INNER JOIN   Sys_Permiso AS P ON Sys_Perfil.idPerfil = P.idPerfil " +
                              " INNER JOIN   Sys_Menu AS M ON P.idMenu = M.idMenu " +
                              " WHERE (M.habilitado = 1)  AND (M.idModulo = 2) and  (U.activo=1 )  AND (U.idUsuario =" + p.ToString() + ")";

           // using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            //{
                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];
                ArrayList l_permisos;
                l_permisos = new ArrayList();
                foreach (DataRow dr in dtPermisos.Rows)
                {
                    l_permisos.Add(dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString());
                    Session["s_permiso"] = l_permisos;
                }
                //conn.Close();
            //}

        }

        private void IdentificarSSO()
        {
            ///////Simula Log In//////
            //string sessionId = "admin";
            //HttpContext.Current.User = new GenericPrincipal(new Salud.Security.SSO.SSOIdentity(new HttpCookie("SSO_AUTH_COOKIE", sessionId)), null);
            ////////////////////////////            
            int idUsuarioLogueado = IdentificarUsuarioSSO();
            if (idUsuarioLogueado == 0)
                Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Login.aspx", Request.Url.ToString());
            else
            {
                Usuario oUser = new Usuario();
                oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Salud.Security.SSO.SSOHelper.CurrentIdentity.Username);
                if (oUser != null)
                {
                    idUsuarioLogueado = oUser.IdUsuario;

                    string s_perfil = oUser.IdPerfil.Nombre;

                    //object perfil = Salud.Security.SSO.SSOHelper.CurrentIdentity.GetSetting(s_perfil);
                    //if (perfil == null) Salud.Security.SSO.SSOHelper.RedirectToErrorPage(403,0,"El usuario no tiene permisos de acceso al sistema de laboratorio. Comuniquese con el Administrador del Sistema.");
                    //else
                    //{    
                    CrearLogAcceso(idUsuarioLogueado);
                    CrearPermisos(idUsuarioLogueado);
                    Session["idUsuario"] = idUsuarioLogueado;
//                    Response.Redirect("Principal.aspx", false);
                }
                else
                    Response.Redirect("../AccesoDenegado.htm");
            }

        }


    }
}
