using Business;
using Business.Data.Laboratorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebLab
{
    public partial class PeticionList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //divPeticion.Visible = true;
            //CargarPeticiones();
        }

        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {
            Configuracion oCon = new Configuracion(); oCon = (Configuracion)oCon.Get(typeof(Configuracion), 1);
            if (oCon.PeticionElectronica)
            {
                lblActualizacion.Text = "Datos actualizados al: " + DateTime.Now.ToLongTimeString();
                CargarPeticiones();
            }

        }


        public void CargarPeticiones()
        {
            divPeticion.Visible = true;
            DataList2.DataSource = LeerPeticiones();
            DataList2.DataBind();
        }

        private object LeerPeticiones()
        {
            DataSet Ds = new DataSet();
            SqlConnection conn = (SqlConnection)NHibernateHttpModule.CurrentSession.Connection;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "[LAB_GetPeticionesPendientes]";


            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(Ds);

            int cantidad= Ds.Tables[0].Rows.Count;
            if (cantidad==0) lblCantidad.Text = "";
            if (cantidad == 1) lblCantidad.Text = cantidad.ToString()+" PETICION";
            if (cantidad > 1) lblCantidad.Text = cantidad.ToString() + " PETICIONES";

            return Ds.Tables[0];


        }

        protected void DataList2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            HyperLink oHplInfo = (HyperLink)e.Item.FindControl("hplPeticionEdit");
            if (oHplInfo != null)
            {
                string s_idPeticion = oHplInfo.NavigateUrl;
                Peticion oMensaje = new Peticion();
                oMensaje = (Peticion)oMensaje.Get(typeof(Peticion), int.Parse(s_idPeticion));

                oHplInfo.NavigateUrl = "Protocolos/ProtocoloEdit2.aspx?idPaciente=" + oMensaje.IdPaciente.IdPaciente.ToString() + "&Operacion=AltaPeticion&idServicio=" + oMensaje.IdTipoServicio.IdTipoServicio.ToString() + "&idPeticion=" + oMensaje.IdPeticion.ToString();


            }
        }
    }
}