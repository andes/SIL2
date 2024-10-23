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
using NHibernate;
using Business;
using Business.Data.Laboratorio;
using NHibernate.Expression;

namespace WebLab
{
    public partial class ProximoNumeroList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //if (Request["tipo"].ToString()=="2")
                //InicializarTablaSector();
                //else
                //    InicializarTablaTipoServicio();

                switch (int.Parse(Request["tipo"].ToString()))
                {

                    case 0:  ///autonumerico
                        {
                            Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                            lblProximoProtocolo1.Text = oP.GenerarNumero().ToString();
                            GridView1.Visible = false;
                        }

                        break;
                    case 1:  //numero diario
                        {
                            Business.Data.Laboratorio.Protocolo oP = new Business.Data.Laboratorio.Protocolo();
                            lblProximoProtocolo1.Text = oP.GenerarNumeroDiario(DateTime.Now.ToString("yyyyMMdd")).ToString();
                            GridView1.Visible = false;
                        }

                        break;
                    case 2:
                        { InicializarTablaSector(); lblProximoProtocolo1.Visible = false; } break;
                    case 3: { InicializarTablaTipoServicio(); lblProximoProtocolo1.Visible = false;
                }
                break;

                }
            }

        }

        private void InicializarTablaTipoServicio()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Tipo Servicio");            
            dt.Columns.Add("Próximo Número");


            ISession m_session = NHibernateHttpModule.CurrentSession;
            ICriteria crit = m_session.CreateCriteria(typeof(TipoServicio));
            crit.Add(Expression.Eq("Baja", false));

            IList detalle = crit.List();
            if (detalle.Count > 0)
            {
                foreach (TipoServicio oSector in detalle)
                {
                    Protocolo oProtocolo = new Protocolo();


                    DataRow row = dt.NewRow();
                    row[0] = oSector.Nombre;                 
                    row[1] = oProtocolo.GenerarNumeroTipoServicio(oSector).ToString();
                    dt.Rows.Add(row);
                }
            }


            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        private void InicializarTablaSector()
        {
            
           DataTable dt = new DataTable();

           dt.Columns.Add("Sector/Servicio");
           dt.Columns.Add("Prefijo");
           dt.Columns.Add("Próximo Número");


           ISession m_session = NHibernateHttpModule.CurrentSession;
           ICriteria crit = m_session.CreateCriteria(typeof(SectorServicio));
           crit.Add(Expression.Eq("Baja", false));
           crit.AddOrder(Order.Asc("Nombre"));
           IList detalle = crit.List();
           if (detalle.Count > 0)
           {
               foreach (SectorServicio oSector in detalle)
               {
                   Protocolo oProtocolo = new Protocolo();
                   

                   DataRow row = dt.NewRow();
                   row[0] = oSector.Nombre;
                   row[1] = oSector.Prefijo;
                   row[2] = oProtocolo.GenerarNumeroGrupo(oSector).ToString(); 
                   dt.Rows.Add(row);
               }
           }

          
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void lnkRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Principal.aspx", false);
        }


    }
}
