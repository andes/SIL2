using Business.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab.Protocolos
{
    public partial class ObraSocialSel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          if (!Page.IsPostBack)
            { 
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));

          

            OSociales.setOS(oPaciente.IdObraSocial); //remplazar
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Paciente oPaciente = new Paciente();
            oPaciente = (Paciente)oPaciente.Get(typeof(Paciente), int.Parse(Request["idPaciente"].ToString()));
            oPaciente.IdObraSocial = OSociales.getObraSocial();
            oPaciente.Save();
            OSociales.setOS(oPaciente.IdObraSocial); //remplazar
        }
    }
}