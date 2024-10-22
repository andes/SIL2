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
using Business.Data;
using System.Data.SqlClient;
using Business.Data.Laboratorio;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections.Generic;

namespace WebLab
{
    public partial class login : System.Web.UI.UserControl
    {
       
      

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["idUsuario"] = null;
            //Session["idUsuarioValida"] = null;
            this.Login1.Focus();
            //if (Session["SIL"] != null)
            //    Response.Redirect("Logout.aspx", false);

            //else
            //{
                if (Request["Operacion"] == null)
                {
                if (Session["SIL"] != null)
                    Response.Redirect("Logout.aspx", false);

                else
                {
                    if (ConfigurationManager.AppSettings["tipoAutenticacion"].ToString() == "SSO")


                        Salud.Security.SSO.SSOHelper.RedirectToSSOPage("Logout.aspx?relogin=1", "login.aspx");
                }
                }
            //}
            //else 

        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            Utility oUtil = new Utility();
            string m_password = oUtil.Encrypt(Login1.Password);

            Usuario oUser = new Usuario();
            oUser = (Usuario)oUser.Get(typeof(Usuario), "Username", Login1.UserName, "Password", m_password);
          
            if (oUser != null)
            {
                if ((oUser.Activo)&&(oUser.IdPerfil.Activo))
                {
                    Session["idUsuarioValida"] = null;

                    if (Request["Operacion"] == null)
                    {//////////////nuevo login
                      
                        Response.Redirect("Default.aspx?IdUsuario=" + oUser.IdUsuario.ToString(), false);
                    }
                    else
                    {///////////////validacion


                        if (Request["idCasoFiliacion"] != null)
                        {


                            if ((Request["idServicio"].ToString() == "6") && (VerificarSiTienePermisodeValidar(oUser.Username, "/CasoFiliacion/CasoResultado.aspx")))
                            {
                                //HttpContext Context;
                                //Context = HttpContext.Current;
                                //Context.Items.Add("id", Request["idCasoFiliacion"].ToString());
                                Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                //Context.Items.Add("Desde", "Valida");
                                //Server.Transfer("~/CasoFiliacion/CasoResultado.aspx");

                                Response.Redirect("~/casoFiliacion/CasoResultado3.aspx?id=" + Request["idCasoFiliacion"].ToString() + "&Desde=Valida&logIn=1");


                            }
                            if ((Request["idServicio"].ToString() == "3") && (VerificarSiTienePermisodeValidar(oUser.Username, "/CasoFiliacion/CasoResultadoHisto.aspx")))
                            {
                                //HttpContext Context2;
                                //Context2 = HttpContext.Current;
                                //Context2.Items.Add("id", Request["idCasoFiliacion"].ToString());
                                Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                //Context2.Items.Add("Desde", "Valida");
                                //Server.Transfer("~/CasoFiliacion/CasoResultadoHisto.aspx");

                                Response.Redirect("~/CasoFiliacion/CasoResultadoHisto.aspx?id=" + Request["idCasoFiliacion"].ToString() + "&Desde=Valida&logIn=1", false);

                            }




                        }
                        else
                        {
                            if (Request["idPlaca"] != null)
                            {
                                if (Request["idServicio"].ToString() == "3") //&& (VerificarSiTienePermisodeValidar(oUser.Username, "/CasoFiliacion/CasoResultadoHisto.aspx")))
                                {
                                    //HttpContext Context2;
                                    //Context2 = HttpContext.Current;
                                    //Context2.Items.Add("id", Request["idCasoFiliacion"].ToString());
                                    Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                    //Context2.Items.Add("Desde", "Valida");
                                    //Server.Transfer("~/CasoFiliacion/CasoResultadoHisto.aspx");
                                    Business.Data.Laboratorio.Placa oRegistro = new Business.Data.Laboratorio.Placa();
                                    oRegistro = (Business.Data.Laboratorio.Placa)oRegistro.Get(typeof(Business.Data.Laboratorio.Placa), int.Parse(Request["idPlaca"].ToString()));
                                    if (oRegistro.Equipo=="PromegaM")
                                        Response.Redirect("~/Placas/PlacaResultadoMixta.aspx?idPlaca=" + Request["idPlaca"].ToString() + "&Desde=Valida&logIn=1&Operacion=Valida", false);

                                    else
                                        Response.Redirect("~/Placas/PlacaResultado.aspx?idPlaca=" + Request["idPlaca"].ToString() + "&Desde=Valida&logIn=1&Operacion=Valida", false);


                                }
                            }
                            else
                            {

                                string idServicio = Request["idServicio"].ToString();
                                string operacion = Request["Operacion"].ToString();
                                string modo = Request["modo"].ToString();
                                Configuracion oC = new Configuracion();
                                oC = (Configuracion)oC.Get(typeof(Configuracion), "IdEfector", oUser.IdEfector);
                                if (oUser.IdEfector != oC.IdEfector)
                                {
                                    if (VerificarSiTienePermisodeValidar(oUser.Username, "/Resultados/ResultadoBExterno.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo))
                                    {
                                        Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                        Response.Redirect("~/Resultados/ResultadoBExterno.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo + "&logIn=1", false);// + "&idUsuarioValida=" + oUser.IdUsuario, false);
                                    }
                                    else
                                    {
                                        e.Authenticated = false;
                                        Login1.FailureText = "El usuario no tiene permisos para validar.";
                                    }

                                }
                                else
                                {
                                    if (VerificarSiTienePermisodeValidar(oUser.Username, "/Resultados/ResultadoBusqueda.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo))
                                    {
                                        Session["idUsuarioValida"] = oUser.IdUsuario.ToString();
                                        if (Request["urgencia"] != null)
                                        {

                                            string sredirect = "~/resultados/ResultadoEdit2.aspx?idServicio=1&Operacion=Valida&idProtocolo=" + Request["idProtocolo"].ToString() + "&Index=0&Parametros=" + Request["idProtocolo"].ToString() + "&idArea=0&urgencia=1&validado=0&modo=Urgencia"; //&idUsuarioValida=" + oUser.IdUsuario.ToString();
                                            if (Request["desde"] != null)
                                                sredirect += "&desde=" + Request["desde"].ToString();
                                            Response.Redirect(sredirect, false);
                                            //    Response.Redirect("~/Resultados/ResultadoBusqueda.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo + "&idUsuarioValida=" + oUser.IdUsuario, false);
                                        }
                                        else
                                            Response.Redirect("~/Resultados/ResultadoBusqueda.aspx?idServicio=" + idServicio + "&Operacion=" + operacion + "&modo=" + modo + "&logIn=1", false);// + "&idUsuarioValida=" + oUser.IdUsuario, false);
                                    }
                                    else
                                    {
                                        e.Authenticated = false;
                                        Login1.FailureText = "El usuario no tiene permisos para validar.";
                                    }
                                }
                            }
                        }
                        
                    }
                }
                else
                {
                    e.Authenticated = false;
                    Login1.FailureText = "El usuario no está activo.";
                }
            }
            else
            {
                e.Authenticated = false;
                Login1.FailureText = "El usuario y/o contraseña no son correctos.";
            }
        }


     
   
        private bool VerificarSiTienePermisodeValidar(string user, string m_url)
        {
            bool b_permiso = false;
            string m_strSQL = @" SELECT   P.permiso, M.objeto, M.url, U.username
            FROM         Sys_Menu AS M INNER JOIN
            Sys_Permiso AS P ON M.idMenu = P.idMenu INNER JOIN
            Sys_Usuario AS U ON P.idPerfil = U.idPerfil
            WHERE     (M.url = '" + m_url + "') AND (U.username = '" + user + "') AND (P.permiso = 2) and  (U.activo=1 ) ";

            using (SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection)
            {
                DataSet Ds = new DataSet();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dtPermisos = Ds.Tables[0];

                if (dtPermisos.Rows.Count > 0) b_permiso = true;
                conn.Close();
                return b_permiso;
            }
        }
    }
}