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
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;

namespace WebLab
{
    public partial class CodigoBarrasEdit : System.Web.UI.Page
    {

        private enum TabIndex
        {
            DEFAULT = 0,
            ONE = 1,        
        }
        private void SetSelectedTab(TabIndex tabIndex)
        {
            HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                MostrarDatosLaboratorio();
                MostrarDatosMicrobiologia();

            }
        }

        private void MostrarDatosLaboratorio()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 1);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo);

            if (oRegistro == null)
            {
                chkImprimeCodigoBarrasLaboratorio.Checked = false;
                pnlLaboratorio.Enabled = false;
            }
            else
            {
                chkImprimeCodigoBarrasLaboratorio.Checked = oRegistro.Habilitado;
                pnlLaboratorio.Enabled = chkImprimeCodigoBarrasLaboratorio.Checked;
                ddlFuente.SelectedValue = oRegistro.Fuente;
                chkProtocolo.Items[1].Selected = oRegistro.ProtocoloFecha;
                chkProtocolo.Items[2].Selected = oRegistro.ProtocoloOrigen;

                chkProtocolo.Items[3].Selected = oRegistro.ProtocoloSector;
                chkProtocolo.Items[4].Selected = oRegistro.ProtocoloNumeroOrigen;

                chkPaciente.Items[0].Selected = oRegistro.PacienteApellido;
                chkPaciente.Items[1].Selected = oRegistro.PacienteSexo;
                chkPaciente.Items[2].Selected = oRegistro.PacienteEdad;
                chkPaciente.Items[3].Selected = oRegistro.PacienteNumeroDocumento; 



            }
        }

        private void MostrarDatosMicrobiologia()
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 3);

            ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
            oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo);

            if (oRegistro == null)
            {
                chkImprimeCodigoBarrasMicrobiologia.Checked = false;
                pnlMicrobiologia.Enabled = false;
            }
            else
            {
                chkImprimeCodigoBarrasMicrobiologia.Checked = oRegistro.Habilitado;
                pnlMicrobiologia.Enabled = chkImprimeCodigoBarrasMicrobiologia.Checked;
                rdbFuente2.SelectedValue = oRegistro.Fuente;
                chkProtocolo2.Items[1].Selected = oRegistro.ProtocoloFecha;
                chkProtocolo2.Items[2].Selected = oRegistro.ProtocoloOrigen;

                chkProtocolo2.Items[3].Selected = oRegistro.ProtocoloSector;
                chkProtocolo2.Items[4].Selected = oRegistro.ProtocoloNumeroOrigen;

                chkPaciente2.Items[0].Selected = oRegistro.PacienteApellido;
                chkPaciente2.Items[1].Selected = oRegistro.PacienteSexo;
                chkPaciente2.Items[2].Selected = oRegistro.PacienteEdad;
                chkPaciente2.Items[3].Selected = oRegistro.PacienteNumeroDocumento; 



            }
        }

       

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                TipoServicio oTipo = new TipoServicio();
                oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 1);

                ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
                oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo);
                if (oRegistro == null)
                {
                    ConfiguracionCodigoBarra oRegistroNew = new ConfiguracionCodigoBarra();
                    Guardar(oRegistroNew);
                }
                else
                    Guardar(oRegistro);
                }
        }


        private void Guardar(ConfiguracionCodigoBarra oRegistro)
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 1);

            oRegistro.Habilitado = chkImprimeCodigoBarrasLaboratorio.Checked;
            oRegistro.IdTipoServicio = oTipo;
            oRegistro.Fuente = ddlFuente.SelectedValue;
            oRegistro.ProtocoloFecha = chkProtocolo.Items[1].Selected;
            oRegistro.ProtocoloOrigen = chkProtocolo.Items[2].Selected;
            oRegistro.ProtocoloSector = chkProtocolo.Items[3].Selected;
            oRegistro.ProtocoloNumeroOrigen = chkProtocolo.Items[4].Selected;

            oRegistro.PacienteApellido = chkPaciente.Items[0].Selected;
            oRegistro.PacienteSexo= chkPaciente.Items[1].Selected;
            oRegistro.PacienteEdad = chkPaciente.Items[2].Selected;
            oRegistro.PacienteNumeroDocumento = chkPaciente.Items[3].Selected;
            
            oRegistro.Save();

           
        }

        protected void chkImprimeCodigoBarrasLaboratorio_CheckedChanged(object sender, EventArgs e)
        {
            
             pnlLaboratorio.Enabled = chkImprimeCodigoBarrasLaboratorio.Checked;
             pnlLaboratorio.UpdateAfterCallBack = true;
             SetSelectedTab(TabIndex.DEFAULT);
        }

        protected void chkImprimeCodigoBarrasMicrobiologia_CheckedChanged(object sender, EventArgs e)
        {

            pnlMicrobiologia.Enabled = chkImprimeCodigoBarrasMicrobiologia.Checked;
            pnlMicrobiologia.UpdateAfterCallBack = true;
            SetSelectedTab(TabIndex.ONE);
        }

        protected void btnGuardar2_Command(object sender, CommandEventArgs e)
        {

        }

        protected void btnGuardar2_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                TipoServicio oTipo = new TipoServicio();
                oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 3);

                ConfiguracionCodigoBarra oRegistro = new ConfiguracionCodigoBarra();
                oRegistro = (ConfiguracionCodigoBarra)oRegistro.Get(typeof(ConfiguracionCodigoBarra), "IdTipoServicio", oTipo);

               if (oRegistro == null)
                {
                    ConfiguracionCodigoBarra oRegistroNew = new ConfiguracionCodigoBarra();
                    GuardarMicrobiologia(oRegistroNew);
                }
                else
                    GuardarMicrobiologia(oRegistro);

               

            }

        }

        private void GuardarMicrobiologia(ConfiguracionCodigoBarra oRegistro)
        {
            TipoServicio oTipo = new TipoServicio();
            oTipo = (TipoServicio)oTipo.Get(typeof(TipoServicio), 3);

            oRegistro.Habilitado =chkImprimeCodigoBarrasMicrobiologia.Checked;

            oRegistro.IdTipoServicio = oTipo;
            oRegistro.Fuente =rdbFuente2.SelectedValue;
            oRegistro.ProtocoloFecha = chkProtocolo2.Items[1].Selected;
            oRegistro.ProtocoloOrigen = chkProtocolo2.Items[2].Selected;
            oRegistro.ProtocoloSector = chkProtocolo2.Items[3].Selected;
            oRegistro.ProtocoloNumeroOrigen = chkProtocolo2.Items[4].Selected;

            oRegistro.PacienteApellido = chkPaciente2.Items[0].Selected;
            oRegistro.PacienteSexo = chkPaciente2.Items[1].Selected;
            oRegistro.PacienteEdad = chkPaciente2.Items[2].Selected;
            oRegistro.PacienteNumeroDocumento = chkPaciente2.Items[3].Selected;


            oRegistro.Save();

           
        }
    }
}
