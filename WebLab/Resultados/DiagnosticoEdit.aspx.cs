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
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using Business.Data;
using System.Configuration;

namespace WebLab.Resultados
{
    public partial class DiagnosticoEdit : System.Web.UI.Page
    {
        private Random
            random = new Random();

        private static int
            TEST = 0;

        private bool IsTokenValid()
        {
            bool result = double.Parse(hidToken.Value) == ((double)Session["NextToken"]);
            SetToken();
            return result;
        }

        private void SetToken()
        {
            double next = random.Next();
            hidToken.Value = next + "";
            Session["NextToken"] = next;
        }
      
        Protocolo oProtocolo = new Protocolo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetToken();
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
                if (oRegistro != null)
                {
                    lblProtocolo.Text = oRegistro.Numero.ToString() + " " + oRegistro.IdPaciente.Apellido + " " + oRegistro.IdPaciente.Nombre;
                    //    CargarListas(oRegistro);
                    MuestraDatos(oRegistro);
                }

                
                
            }
        }

        private void MuestraDatos(Business.Data.Laboratorio.Protocolo oRegistro)
        {
         
            ///Agregar a la tabla las determinaciones para mostrarlas en el gridview                             
            //dtDeterminaciones = (System.Data.DataTable)(Session["Tabla1"]);
            //DetalleProtocolo oDetalle = new DetalleProtocolo();
            //ISession m_session = NHibernateHttpModule.CurrentSession;

            /////Agregar a la tabla las diagnosticos para mostrarlas en el gridview                             
            ////   dtDiagnosticos = (System.Data.DataTable)(Session["Tabla2"]);
            //ProtocoloDiagnostico oDiagnostico = new ProtocoloDiagnostico();
            //ICriteria crit2 = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
            //crit2.Add(Expression.Eq("IdProtocolo", oRegistro));

            //IList diagnosticos = crit2.List();

            //foreach (ProtocoloDiagnostico oDiag in diagnosticos)
            //{
            //    Cie10 oCie10 = new Cie10();
            //    oCie10 = (Cie10)oCie10.Get(typeof(Cie10), oDiag.IdDiagnostico);

            //    ListItem oDia = new ListItem();
            //    oDia.Text = oCie10.Codigo + " - " + oCie10.Nombre;
            //    oDia.Value = oCie10.Id.ToString();
            //    lstDiagnosticosFinal.Items.Add(oDia);
            //}

            string m_strSQL = @"select c.id, c.codigo + ' -' + c.nombre 
from sys_cie10 c with (nolock)
inner join LAB_ProtocoloDiagnostico pd with (nolock) on c.id = pd.idDiagnostico
where pd.idProtocolo=" + oRegistro.IdProtocolo.ToString();

            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
            adapter.Fill(Ds);
            lstDiagnosticosFinal.Items.Clear();  
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {
               

                ListItem oDia = new ListItem();
                oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                lstDiagnosticosFinal.Items.Add(oDia);


            }


        }


        private void BuscarCodigoDiagnostico()
        {
            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {



                //if (oC.NomencladorDiagnostico == 0) /// Cie10
                //{
                    string m_strSQL = @"select id, codigo + ' -' + nombre from sys_cie10 with (nolock) where CODIGO like '%" + txtCodigoDiagnostico.Text.Trim() + "%'";

                    DataSet Ds = new DataSet();
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    adapter.Fill(Ds);
                    lstDiagnosticos.Items.Clear();
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        ListItem oDia = new ListItem();
                        oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                        oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                        lstDiagnosticos.Items.Add(oDia);


                    }







                //}
                //else /// diagnostico propio
                //{
                //    DiagnosticoP oDiagnostico = new DiagnosticoP();
                //    oDiagnostico = (DiagnosticoP)oDiagnostico.Get(typeof(DiagnosticoP), "Codigo", txtCodigoDiagnostico.Text);
                //    if (oDiagnostico != null)
                //    {
                //        ListItem oDia = new ListItem();
                //        oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                //        oDia.Value = oDiagnostico.IdDiagnostico.ToString();
                //        lstDiagnosticos.Items.Add(oDia);

                //    }
                //    else
                //        lstDiagnosticos.Items.Clear();
                //}

            }
            lstDiagnosticos.UpdateAfterCallBack = true;

        }

        protected void btnBusquedaDiagnostico_Click(object sender, EventArgs e)
        {

            lstDiagnosticos.Items.Clear();
            if (txtCodigoDiagnostico.Text != "")
            {
                BuscarCodigoDiagnostico();
                //Cie10 oDiagnostico = new Cie10();
                //oDiagnostico = (Cie10)oDiagnostico.Get(typeof(Cie10), "Codigo", txtCodigoDiagnostico.Text);
                //if (oDiagnostico != null)
                //{
                //    ListItem oDia = new ListItem();
                //    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                //    oDia.Value = oDiagnostico.Id.ToString();
                //    lstDiagnosticos.Items.Add(oDia);

                //}
                //else
                //    lstDiagnosticos.Items.Clear();
            }

            if (txtNombreDiagnostico.Text != "")
            {
                BuscarNombreDiagnostico();
                //lstDiagnosticos.Items.Clear();
                //ISession m_session = NHibernateHttpModule.CurrentSession;
                //ICriteria crit = m_session.CreateCriteria(typeof(Cie10));
                //crit.Add(Expression.Sql(" Nombre like '%" + txtNombreDiagnostico.Text + "%' order by Nombre"));

                //IList items = crit.List();

                //foreach (Cie10 oDiagnostico in items)
                //{
                //    ListItem oDia = new ListItem();
                //    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                //    oDia.Value = oDiagnostico.Id.ToString();
                //    lstDiagnosticos.Items.Add(oDia);
                //}


            }
            lstDiagnosticos.UpdateAfterCallBack = true;




        }

        private void BuscarNombreDiagnostico()
        {
            lstDiagnosticos.Items.Clear();
            if (txtNombreDiagnostico.Text != "")
            {

               // ISession m_session = NHibernateHttpModule.CurrentSession;
                //if (oC.NomencladorDiagnostico == 0)
                //{
                    //ICriteria crit = m_session.CreateCriteria(typeof(Cie10));
                    //crit.Add(Expression.Sql(" Nombre like '%" + txtNombreDiagnostico.Text + "%' order by Nombre"));

                    //IList items = crit.List();

                    //foreach (Cie10 oDiagnostico in items)
                    //{
                    //    ListItem oDia = new ListItem();
                    //    oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                    //    oDia.Value = oDiagnostico.Id.ToString();
                    //    lstDiagnosticos.Items.Add(oDia);
                    //}


                    string m_strSQL = @"select id, codigo + ' -' + nombre from sys_cie10 with (nolock) where Nombre like '%" + txtNombreDiagnostico.Text.Trim() + "%' order by Nombre";

                    DataSet Ds = new DataSet();
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(m_strSQL, conn);
                    adapter.Fill(Ds);
                    lstDiagnosticos.Items.Clear();
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        ListItem oDia = new ListItem();
                        oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                        oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                        lstDiagnosticos.Items.Add(oDia);


                    }


                    lstDiagnosticos.UpdateAfterCallBack = true;
                //}

                //else //nomenclador propio
                //{
                //    ICriteria crit1 = m_session.CreateCriteria(typeof(DiagnosticoP));

                //    crit1.Add(Expression.InsensitiveLike("Nombre", txtNombreDiagnostico.Text, MatchMode.Anywhere));
                //    crit1.Add(Expression.Eq("Baja", false));
                //    IList items = crit1.List();

                //    foreach (DiagnosticoP oDiagnostico in items)
                //    {
                //        ListItem oDia = new ListItem();
                //        oDia.Text = oDiagnostico.Codigo + " - " + oDiagnostico.Nombre;
                //        oDia.Value = oDiagnostico.IdDiagnostico.ToString();
                //        lstDiagnosticos.Items.Add(oDia);
                //    }

                //    lstDiagnosticos.UpdateAfterCallBack = true;
                //}

            }
        }

        private void CargarDiagnosticosFrecuentes()
        {
            //            Utility oUtil = new Utility();


            //            //  btnGuardarImprimir.Visible = oC.GeneraComprobanteProtocolo;
            //            lstDiagnosticos.Items.Clear();

                        Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                         oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));
            //            if (oRegistro != null)
            //            {
            //                string m_ssql = @"SELECT top 20 ID, Codigo + ' - ' + Nombre as nombre, count (*)  cantidad
            //FROM Sys_CIE10 c
            //inner join LAB_ProtocoloDiagnostico p on c.id = p.idDiagnostico
            //where p.idEfector=" + oRegistro.IdEfector.IdEfector.ToString() + @"
            //group by id, codigo, nombre
            //ORDER BY cantidad desc";
            //                oUtil.CargarListBox(lstDiagnosticos, m_ssql, "id", "nombre");
            //            }
            //            lstDiagnosticos.UpdateAfterCallBack = true;
            Utility oUtil = new Utility();


            //  btnGuardarImprimir.Visible = oC.GeneraComprobanteProtocolo;
            lstDiagnosticos.Items.Clear();

            string m_ssql = @"SELECT top 20 ID, Codigo + ' - ' + Nombre as nombre, count (*)  cantidad
FROM Sys_CIE10 c with (nolock)
inner join LAB_ProtocoloDiagnostico p with (nolock) on c.id = p.idDiagnostico
where p.idEfector=" + oRegistro.IdEfector.IdEfector.ToString() + @"
group by id, codigo, nombre
ORDER BY cantidad desc";



            DataSet Ds = new DataSet();
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SIL_ReadOnly"].ConnectionString); ///Performance: conexion de solo lectura
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = new SqlCommand(m_ssql, conn);
            adapter.Fill(Ds);
            lstDiagnosticos.Items.Clear();
            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
            {

                ListItem oDia = new ListItem();
                oDia.Text = Ds.Tables[0].Rows[i][1].ToString();
                oDia.Value = Ds.Tables[0].Rows[i][0].ToString();
                
                lstDiagnosticos.Items.Add(oDia);
                


            }

            //if (oC.NomencladorDiagnostico==1) //prppio
            //    m_ssql = "SELECT idDiagnostico as ID, Codigo + ' - ' + Nombre as nombre FROM lab_Diagnostico WHERE (idDiagnostico IN (SELECT DISTINCT idDiagnostico FROM LAB_ProtocoloDiagnostico)) ORDER BY Nombre";
            //oUtil.CargarListBox(lstDiagnosticos, m_ssql, "id", "nombre");
            lstDiagnosticos.UpdateAfterCallBack = true;


        }

        protected void btnBusquedaFrecuente_Click(object sender, EventArgs e)
        {
            CargarDiagnosticosFrecuentes();
        }







       
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
                          
        }

        private void GuardarDiagnosticos(Business.Data.Laboratorio.Protocolo oRegistro)
        {
            if (IsTokenValid())
            {
                TEST++;
                string embarazada = ""; string accion = "Graba";
                //   dtDiagnosticos = (System.Data.DataTable)(Session["Tabla2"]);
                ///Eliminar los detalles y volverlos a crear
                ISession m_session = NHibernateHttpModule.CurrentSession;
                ICriteria crit = m_session.CreateCriteria(typeof(ProtocoloDiagnostico));
                crit.Add(Expression.Eq("IdProtocolo", oRegistro));
                IList detalle = crit.List();
                if (detalle.Count > 0)
                {
                    foreach (ProtocoloDiagnostico oDetalle in detalle)
                    {
                       /* Cie10 oCie10 = new Cie10(oDetalle.IdDiagnostico);
                        string s_diag_1 = oCie10.Nombre;*/
                        oDetalle.Delete();
                        accion = "Cambia";
                        ///oRegistro.GrabarAuditoriaDetalleProtocolo("Elimina", int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", s_diag_1);
                    }
                }


                ///Busca en la lista de diagnosticos buscados
                if (lstDiagnosticosFinal.Items.Count > 0)
                {
                    /////Crea nuevamente los detalles.
                    for (int i = 0; i < lstDiagnosticosFinal.Items.Count; i++)
                    {
                        ProtocoloDiagnostico oDetalle = new ProtocoloDiagnostico();
                        oDetalle.IdProtocolo = oRegistro;
                        oDetalle.IdEfector = oRegistro.IdEfector;
                        oDetalle.IdDiagnostico = int.Parse(lstDiagnosticosFinal.Items[i].Value);
                        string s_diag = lstDiagnosticosFinal.Items[i].Text;
                        oDetalle.Save();
                        oRegistro.GrabarAuditoriaDetalleProtocolo(accion, int.Parse(Session["idUsuario"].ToString()), "Diagnóstico", s_diag);
                        embarazada = oDetalle.EsEmbarazada();


                    }
                }
                if (embarazada == "E")
                {
                    oRegistro.Embarazada = "S";
                    oRegistro.Save();
                }

            }

        }


        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click1(object sender, EventArgs e)
        {
            if (Page.IsValid)
            { ///Verifica si se trata de un alta o modificacion de protocolo               
                Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString()));

                GuardarDiagnosticos(oRegistro);
            }
        }



        protected void btnAgregarDiagnostico_Click(object sender, EventArgs e)
        {


            AgregarDiagnostico();
        }
        protected void btnAgregarDiagnostico_Click1(object sender, ImageClickEventArgs e)
        {
            AgregarDiagnostico();
        }

        protected void btnSacarDiagnostico_Click(object sender, ImageClickEventArgs e)
        {
            SacarDiagnostico();
        }

        private void AgregarDiagnostico()
        {
            lblMensajeDiagnostico.Visible = false;
            if (lstDiagnosticos.SelectedValue != "")
            {
                bool agrego = true;
                /////Verifica si ya fue agregado el diagnostico
                for (int i = 0; i < lstDiagnosticosFinal.Items.Count; i++)
                {

                    if (lstDiagnosticosFinal.Items[i].Value == lstDiagnosticos.SelectedItem.Value)
                    {
                        agrego = false; break;
                    }
                }


                if (agrego)
                {
                    lstDiagnosticosFinal.Items.Add(lstDiagnosticos.SelectedItem);
                    lstDiagnosticosFinal.UpdateAfterCallBack = true;
                }
                else
                {
                    lblMensajeDiagnostico.Visible = true;
                    lblMensajeDiagnostico.Text = "Alerta: Diagnostico ya ingresado para el paciente.";

                }
            }
            lblMensajeDiagnostico.UpdateAfterCallBack = true;
        }


        private void SacarDiagnostico()
        {
            if (lstDiagnosticosFinal.SelectedValue != "")
            {
                lstDiagnosticosFinal.Items.Remove(lstDiagnosticosFinal.SelectedItem);
                lstDiagnosticosFinal.UpdateAfterCallBack = true;
            }
        }




        protected void txtCodigo_TextChanged1(object sender, EventArgs e)
        {

        }

      


    }
}

