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
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System.IO;

namespace WebLab.Resultados
{
    public partial class IncidenciaProtocolo : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                Protocolo oProtocolo = new Protocolo();
               string s_idProtocolo = Request["idProtocolo"].ToString();

                oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(s_idProtocolo));//int.Parse(Request["idProtocolo"].ToString()));r();
                Label1.Text = "Protocolo Nro. " + oProtocolo.Numero.ToString();
            
                this.IncidenciaEdit1.MostrarDatosdelProtocolo(oProtocolo.IdProtocolo);
            }

        }

        protected void btnEliminarIncidencia_Click(object sender, EventArgs e)
        {
            string s_idProtocolo = Request["idProtocolo"].ToString();
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));

            this.IncidenciaEdit1.EliminarProtocoloIncidencia(oProtocolo);
          
        }


        protected void btnGuardarIncidencia_Click(object sender, EventArgs e)
        {
            Protocolo oProtocolo = new Protocolo();
            oProtocolo = (Protocolo)oProtocolo.Get(typeof(Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            if (oProtocolo != null)
            {
            this.IncidenciaEdit1.GuardarProtocoloIncidencia(oProtocolo);
           
            }
        }
        //protected void Page_Unload(object sender, EventArgs e)
        //{
        //    if (this.oCr.ReportDocument != null)
        //    {
        //        this.oCr.ReportDocument.Close();
        //        this.oCr.ReportDocument.Dispose();
        //    }
        //}
   


    }
}