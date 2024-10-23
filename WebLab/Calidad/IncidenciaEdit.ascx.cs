using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NHibernate;
using Business;
using Business.Data.Laboratorio;
using NHibernate.Expression;
using System.Collections;

namespace WebLab.Calidad
{
    public partial class IncidenciaEdit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {        

        }

        public void MostrarDatosdelProtocolo(int p)
        {
                      
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloIncidenciaCalidad));
            crit.Add(Expression.Eq("IdProtocolo", p));

            IList items = crit.List();

            foreach (ProtocoloIncidenciaCalidad oDet in items)
            {
                for (int i = 0; i < TreeView2.Nodes.Count; i++)
                {

                    if (TreeView2.Nodes[i].Value == oDet.IdIncidenciaCalidad.ToString())
                    {
                        TreeView2.Nodes[i].Checked = true;
                        
                    }
                    for (int j = 0; j < TreeView2.Nodes[i].ChildNodes.Count; j++)
                    {
                        if (TreeView2.Nodes[i].ChildNodes[j].Value == oDet.IdIncidenciaCalidad.ToString())
                        {
                            TreeView2.Nodes[i].ChildNodes[j].Checked = true;
                            TreeView2.Nodes[i].ExpandAll();
                        }
                    }
                }
            }
        }
              

        public void GuardarProtocoloIncidencia(Protocolo oRegistro)
        {
            //// borra los detalles para el existente y los crea de nuevo
            EliminarProtocoloIncidencia(oRegistro);
            for (int i = 0; i < TreeView2.Nodes.Count; i++)
            {
                if (TreeView2.Nodes[i].Checked)
                {
                    ProtocoloIncidenciaCalidad oDet = new ProtocoloIncidenciaCalidad();
                    oDet.IdProtocolo = oRegistro.IdProtocolo;
                   oDet.IdEfector = oRegistro.IdEfector.IdEfector;
                       
                    oDet.IdIncidenciaCalidad = int.Parse(TreeView2.Nodes[i].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
                    oDet.Save();
                    oRegistro.GrabarAuditoriaDetalleProtocolo("Registra", int.Parse(Session["idUsuario"].ToString()), "Incidencia", TreeView2.Nodes[i].Text);
                }
                for (int j = 0; j < TreeView2.Nodes[i].ChildNodes.Count; j++)
                {
                    if (TreeView2.Nodes[i].ChildNodes[j].Checked)
                    {
                        ProtocoloIncidenciaCalidad oDet = new ProtocoloIncidenciaCalidad();
                        oDet.IdProtocolo = oRegistro.IdProtocolo;
                        oDet.IdEfector = oRegistro.IdEfector.IdEfector;
                        oDet.IdIncidenciaCalidad = int.Parse(TreeView2.Nodes[i].ChildNodes[j].Value); // int.Parse(TreeView1.CheckedNodes[i].Value);
                        oDet.Save();
                        oRegistro.GrabarAuditoriaDetalleProtocolo("Registra", int.Parse(Session["idUsuario"].ToString()), "Incidencia", TreeView2.Nodes[i].ChildNodes[j].Text);
                    }
                }
            }
               

        }

 
 

        public void EliminarProtocoloIncidencia(Protocolo oRegistro)
        {
            //// borra los detalles para el existente y los crea de nuevo
            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloIncidenciaCalidad));
            crit.Add(Expression.Eq("IdProtocolo", oRegistro.IdProtocolo));
            IList items = crit.List();

            foreach (ProtocoloIncidenciaCalidad oDet in items)
            {
                IncidenciaCalidad oInci = new IncidenciaCalidad();
                oInci = (IncidenciaCalidad)oInci.Get(typeof(IncidenciaCalidad), oDet.IdIncidenciaCalidad);
                if (oInci != null) oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Incidencia", oInci.Nombre);

                oDet.Delete();
            }

        }
    }
}