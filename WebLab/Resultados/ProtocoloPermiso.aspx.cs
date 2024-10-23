using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using System.Data.SqlClient;
using System.Data;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using System.Collections;
using NHibernate.Expression;
using System.Drawing;

namespace WebLab.Resultados
{
    public partial class ProtocoloPermiso : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            


            if (!Page.IsPostBack)
            {
             

                string s_idProtocolo = Request["idProtocolo"].ToString();
             

                Protocolo oProtocolo = new Protocolo();
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));
                lblProtocolo.Text = oProtocolo.GetNumero();

           


               

                CargarPerfil();
                CargarPermisos(oProtocolo);
               

            }
          
        }

        private void CargarPermisos(Protocolo oProtocolo)
        {
            string val = "";
            //////////////////////////////                              
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.ProtocoloPermiso));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            IList lista = crit.List();
            if (lista.Count > 0)
            {
              
                foreach (Business.Data.Laboratorio.ProtocoloPermiso oDiag in lista)
                {


                    if (val == "") val = oDiag.IdPerfil.ToString();
                    else val += ", " + oDiag.IdPerfil.ToString();



                }
            }

            if (val != "")
            {
                string m_strSQL = " select nombre from LAB_PerfilLaboratorio where idperfilvinculado in (" + val + ")";

                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dt = Ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < chkPerfiles.Items.Count; j++)
                    {
                        if (chkPerfiles.Items[j].Text == dt.Rows[i].ItemArray[0].ToString())
                        {
                            chkPerfiles.Items[j].Selected = true;
                        }
                    }
                }
            }
        }

        private void CargarPerfil()
        {
            Utility oUtil = new Utility();
            
            string m_ssql = @"select distinct nombre from LAB_PerfilLaboratorio ORDER BY  nombre";
            oUtil.CargarCheckBox(chkPerfiles, m_ssql, "nombre", "nombre");

          


        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Guardar();
            }
        }

        private void Guardar()
        {
            //if (Session["idServicio"] == null) Session["idServicio"] = 1;

            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));

            /// borra los permisos existentes
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.Laboratorio.ProtocoloPermiso));
            crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
            IList lista = crit.List();
            if (lista.Count > 0)
            {

                foreach (Business.Data.Laboratorio.ProtocoloPermiso oDiag in lista)
                {

                    oDiag.Delete();


                }
            }
            // genera nuevos permisos

            Usuario oUser = new Usuario();
           
                if (Session["idUsuarioValida"] != null) oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuarioValida"].ToString()));


                string val = "";
            for (int i = 0; i < chkPerfiles.Items.Count; i++)
            {
                if (chkPerfiles.Items[i].Selected)
                {
                    if (val == "") val = "'"+chkPerfiles.Items[i].Text+"'";
                    else val += ", '" + chkPerfiles.Items[i].Text + "'"; 
                }
            }

            if (val != "")
            {
                string m_strSQL = " SELECT idperfilvinculado from LAB_PerfilLaboratorio where nombre in (" + val + ")";



                DataSet Ds = new DataSet();
                SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                adapter.Fill(Ds);

                DataTable dt = Ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Business.Data.Laboratorio.ProtocoloPermiso oPermisoP = new Business.Data.Laboratorio.ProtocoloPermiso();
                    oPermisoP.IdProtocolo = oProtocolo;
                    oPermisoP.IdPerfil = int.Parse(dt.Rows[i].ItemArray[0].ToString());
                    oPermisoP.IdUsuarioRegistro = oUser;
                    oPermisoP.FechaRegistro = DateTime.Now;
                    oPermisoP.Save();

                }
            }

        }



        protected void btnGuardar_Click1(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                Guardar();
            }

        }
    }
}