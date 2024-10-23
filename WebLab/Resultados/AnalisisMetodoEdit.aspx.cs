using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business;
using Business.Data.Laboratorio;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.Web;
using InfoSoftGlobal;
using Business.Data;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;
using NHibernate;
using NHibernate.Expression;
using System.Collections;

namespace WebLab.Resultados
{
    public partial class AnalisisMetodoEdit : System.Web.UI.Page
    {
      
        Protocolo oProtocolo = new Protocolo();
        Item oItem = new Item();

        public Usuario oUser = new Usuario();

        protected void Page_PreInit(object sender, EventArgs e)
        {
           
            if (Session["idUsuario"] != null)
            {
                oUser = (Usuario)oUser.Get(typeof(Usuario), int.Parse(Session["idUsuario"].ToString()));
                
            }

        }
       
        protected void Page_Load(object sender, EventArgs e)
        {

           if (!Page.IsPostBack)
            {                   
                
                   
                    oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
                    if (oItem != null)                     
                        lblItem.Text = oItem.Nombre;
                        
                     
              
                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                 
                    if (oProtocolo != null)
                    lblPaciente.Text = oProtocolo.IdPaciente.Apellido + " " + oProtocolo.IdPaciente.Nombre;

                CargarPresentaciones(oItem);
                  
                }                
            }

        private void CargarPresentaciones(                    Item oItem)
        {
            string m_strSQL = @"select distinct idItemPresentacion, presentacion
			 from LAB_ValorReferencia VR with (nolock)
			 inner join LAB_ItemPresentacion I with (nolock) on  I.idItemPresentacion= VR.idPresentacion
			 where VR.idefector=" + oUser.IdEfector.IdEfector.ToString()+"  and VR.iditem=" + oItem.IdItem.ToString();

           Utility oUtil = new Utility();                            
           oUtil.CargarCombo(ddlPresentacion, m_strSQL, "idItemPresentacion", "presentacion");
            ddlPresentacion.Items.Insert(0, new ListItem("--SELECCIONE --", "0"));


        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)           
            {
                ISession m_session = NHibernateHttpModule.CurrentSession;
                oItem = (Item)oItem.Get(typeof(Item), int.Parse(Request["idItem"].ToString()));
                if (oItem != null)

                {

                    oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));


                    ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
                    crit.Add(Expression.Eq("IdProtocolo", oProtocolo));
                    crit.Add(Expression.Eq("IdEfector", oUser.IdEfector));
                    crit.Add(Expression.Eq("IdSubItem", oItem));
                    IList listadetalle = crit.List();
                    foreach (DetalleProtocolo oDetalle in listadetalle)
                    {                                                   

                        ///Calculo de valor de referencia con la presentacion seleccionada
                        int pres = int.Parse(ddlPresentacion.SelectedValue);
                        string m_metodo = "";
                        string m_valorReferencia = "";
                        string valorRef = oDetalle.CalcularValoresReferencia(pres);
                        if (valorRef != "")
                        {
                            string[] arr = valorRef.Split(("|").ToCharArray());
                            switch (arr.Length)
                            {
                                case 1: m_valorReferencia = arr[0].ToString(); break;
                                case 2:
                                    {
                                        m_valorReferencia = arr[0].ToString();
                                        m_metodo = arr[1].ToString();
                                    }
                                    break;
                            }

                        }

                        oDetalle.Metodo = m_metodo;
                        oDetalle.ValorReferencia = m_valorReferencia;
                        oDetalle.Save();
                    }
                }
            }
        }
    }
}