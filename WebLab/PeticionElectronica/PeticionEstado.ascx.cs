using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Business;
using System.Data;
using Business.Data.Laboratorio;

namespace WebLab.PeticionElectronica
{
    public partial class PeticionEstado1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                CargarGrilla();

        }
        private void CargarGrilla()
        {


            DataList1.DataSource = LeerDatos();
            DataList1.DataBind();
        }

        private object LeerDatos()
        {
            string m_strSQL = @" select P.idPeticion,P.fechaRegistro as fechaHoraRegistro , S.nombre as sector ,P.solicitante,
Pac.apellido +  ' ' + Pac.nombre as paciente
  from LAB_Peticion as P
inner join LAB_SectorServicio as S on P.idSector= S.idSectorServicio
inner join sys_paciente as Pac on Pac.idPaciente= P.idPaciente
 where idprotocolo=0";
            DataSet Ds = new DataSet();

            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);


            return Ds.Tables[0];


        }
        protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            HyperLink oHplInfo = (HyperLink)e.Item.FindControl("hplProtocoloEdit");
            if (oHplInfo != null)
            {
                string s_idMensaje = oHplInfo.NavigateUrl;

                Peticion oPeticion = new Peticion();
                oPeticion = (Peticion)oPeticion.Get(typeof(Peticion), int.Parse(s_idMensaje));

                //Response.Redirect("../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oPeticion.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oPeticion.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oPeticion.IdPeticion.ToString(), false);
                oHplInfo.NavigateUrl = "../Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oPeticion.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oPeticion.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oPeticion.IdPeticion.ToString();

               
            }
        }
        protected void imgAgregarMensaje_Click(object sender, ImageClickEventArgs e)
        {
            CargarGrilla();
        }
    }
}