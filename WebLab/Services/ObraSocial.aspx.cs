using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using Business.Data;
using NHibernate;
using NHibernate.Expression;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using System.IO;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Security.Principal;
using System.Text;

using fastJSON;


namespace WebLab.Services
{
    [Serializable]
    public class oshelper
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string sigla { get; set; }
        public string codigoNacion { get; set; }
    }

    public partial class ObraSocial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           if (IsPostBack) return;
            string consulta = String.IsNullOrEmpty(Request.QueryString["term"]) ? "" : Request.QueryString["term"];

            List<oshelper> lstOS = new List<oshelper>();

            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(Business.Data.ObraSocial));

            crit.Add(Expression.Or (Expression.InsensitiveLike("Nombre", String.Format("%{0}%", consulta.Replace('*', '%'))), Expression.InsensitiveLike("Sigla", String.Format("%{0}%", consulta.Replace('*', '%')))));
            
            
            IList oSociales = crit.List();
            
            foreach (Business.Data.ObraSocial OS in oSociales)
            {
                oshelper osh = new oshelper();
                osh.id = OS.IdObraSocial;
                osh.nombre = OS.Nombre;
                osh.sigla = OS.Sigla;
                //osh.codigoNacion = OS.CodigoNacion;
                lstOS.Add(osh);
            }
            if (lstOS.Count == 0)
            {
                lstOS.Add(new oshelper() { id = -666, nombre = "No hay resultados", sigla = "", codigoNacion = "" });
            }
            Response.Clear();
            Response.Write(JSON.Instance.ToJSON(lstOS));
            Response.Flush();
            Response.End();
        }
    }
}
