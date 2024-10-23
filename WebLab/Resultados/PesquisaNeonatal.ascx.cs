using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Business;
using Business.Data;
using Business.Data.Laboratorio;
using NHibernate;
using NHibernate.Expression;
using System.Collections;
using System.Drawing;

namespace WebLab.Resultados
{
    public partial class PesquisaNeonatal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
            
                if (Request["idProtocolo"] != null)
                {
                    Business.Data.Laboratorio.Protocolo oRegistro = new Business.Data.Laboratorio.Protocolo();
                    oRegistro = (Business.Data.Laboratorio.Protocolo)oRegistro.Get(typeof(Business.Data.Laboratorio.Protocolo), int.Parse(Request["idProtocolo"].ToString())); 

                    SolicitudScreening oSolicitud = new SolicitudScreening();
                    oSolicitud = (Business.Data.Laboratorio.SolicitudScreening)oSolicitud.Get(typeof(Business.Data.Laboratorio.SolicitudScreening), "IdProtocolo", oRegistro);

                    if (oSolicitud!=null)
                    MostrarSolicitudScreeening(oSolicitud);

                //    CargarItems();
                //    CargarScreening(int.Parse(Request["idSolicitudScreening"].ToString()));
                //    CargarAlarmas(int.Parse(Request["idSolicitudScreening"].ToString()));
                
                }
            }
        }


        public void MostrarSolicitudScreeening(Business.Data.Laboratorio.SolicitudScreening oSolicitud)
        {
            this.

          // CargarItems();
           lblHoraNacimiento.Text = oSolicitud.HoraNacimiento;
           lblEdadGestacional.Text = oSolicitud.EdadGestacional.ToString();
           lblPeso.Text = oSolicitud.Peso.ToString().Replace(",0000", "");
            lblFechaHoraExtraccion.Text = oSolicitud.FechaExtraccion.ToShortDateString()+ " " + oSolicitud.HoraExtraccion;

            
            switch (oSolicitud.MotivoRepeticion)
            {
                case "0": lblMotivoRepeticion.Text = ""; break;
                case "1": lblMotivoRepeticion.Text = "Mala Muestra"; break;
                case "2": lblMotivoRepeticion.Text = "Prematuro"; break;
                case "3": lblMotivoRepeticion.Text = "Patología"; break;
                case "4": lblMotivoRepeticion.Text = "Ingesta Leche < 24 hs"; break;
                case "5": lblMotivoRepeticion.Text = "Antibióticos"; break;
                case "6": lblMotivoRepeticion.Text = "Transfusión"; break;
                case "7": lblMotivoRepeticion.Text = "Corticoides"; break;
                case "8": lblMotivoRepeticion.Text = "Dopamina"; break;
            }
           

           lblTipoAlimentacion.Text = oSolicitud.TipoAlimentacion;
           if (!oSolicitud.PrimeraMuestra) lblPrimeraMuestra.Text = "No"; else lblPrimeraMuestra.Text = "Si";
           if (!oSolicitud.IngestaLeche24Horas) lblIngestaLeche.Text = "No"; else lblIngestaLeche.Text = "Si";
           if (!oSolicitud.Antibiotico) lblAntibiotico.Text = "No"; else { lblAntibiotico.Text = "Si"; lblAntibiotico.ForeColor = Color.Red; }
           if (!oSolicitud.Transfusion) lblTransfusion.Text = "No"; else { lblTransfusion.Text = "Si"; lblTransfusion.ForeColor = Color.Red; }
           if (!oSolicitud.Corticoides) lblCorticoides.Text = "No"; else { lblCorticoides.Text = "Si"; lblCorticoides.ForeColor = Color.Red; }
           if (!oSolicitud.Dopamina) lblDopamina.Text = "No"; else { lblDopamina.Text = "Si"; lblDopamina.ForeColor = Color.Red; }
           if (!oSolicitud.EnfermedadTiroideaMaterna) lblEnfermedadTiroidea.Text = "No"; else { lblEnfermedadTiroidea.Text = "Si"; lblEnfermedadTiroidea.ForeColor = Color.Red; }

           lblAntecedentesMaterno.Text = oSolicitud.AntecedentesMaternos;
           if (!oSolicitud.CorticoidesMaterno) lblCorticoidesMaterno.Text= "No"; else lblCorticoidesMaterno.Text = "Si";

           lblNumeroTarjeta.Text = oSolicitud.NumeroTarjeta.ToString();
           lblMedicoSolicitante.Text = oSolicitud.MedicoSolicitante;
           lblApellidoMaterno.Text = oSolicitud.ApellidoMaterno;
           lblApellidoPaterno.Text = oSolicitud.ApellidoPaterno;
           lblNombreParentesco.Text= oSolicitud.NombreParentesco;
           lblDniMaterno.Text = oSolicitud.NumerodocumentoParentesco.ToString();
           lblFechaNacimientoParentesco.Text=oSolicitud.FechaNacimientoParentesco.ToShortDateString();
           lblIdLugarControl.Text = oSolicitud.IdLugarControl.ToString();

           Efector oLugarControl = new Efector();
           oLugarControl = (Efector)oLugarControl.Get(typeof(Efector), oSolicitud.IdLugarControl);
           lblLugarControl.Text = oLugarControl.Nombre;

         

            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///mostrar alarmas
           string m_strSQL = @" SELECT descripcion FROM   LAB_SolicitudScreeningAlarma WHERE  idSolicitudScreening =" + oSolicitud.IdSolicitudScreening.ToString();
           DataSet Ds = new DataSet();
           NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
           String strconn = oConf.GetProperty("hibernate.connection.connection_string");
           SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);
           da.Fill(Ds);
           gvAlarmas.DataSource = Ds.Tables[0];
           gvAlarmas.DataBind();
          
        }

//        public  void GuardarSolicitudScreeening(Business.Data.Laboratorio.Protocolo oRegistro, int i_idsolicitud)
//        {
//            SolicitudScreening oSolicitud = new SolicitudScreening();
            
//            oSolicitud.HoraNacimiento = txtHoraNacimiento.Value;            
//            oSolicitud.EdadGestacional = int.Parse( txtEdadGestacional.Value);
//            oSolicitud.Peso = int.Parse(txtPeso.Value);            
//            oSolicitud.FechaExtraccion = DateTime.Parse(txtFechaExtraccion.Value.Trim());            
//            oSolicitud.HoraExtraccion = txtHoraExtraccion.Value.Trim();
//            oSolicitud.MotivoRepeticion = ddlMotivoRepeticion.SelectedValue;
//            oSolicitud.TipoAlimentacion = rdbTipoAlimentacion.SelectedItem.Text;
//            if (rdbPrimeraMuestra.SelectedValue.Trim() == "0") oSolicitud.PrimeraMuestra = false; else oSolicitud.PrimeraMuestra = true;                                       
//            if (rdbIngestaLeche24Horas.SelectedValue.Trim() == "0") oSolicitud.IngestaLeche24Horas = false;else oSolicitud.IngestaLeche24Horas = true;                                  
//            if (rdbAntibiotico.SelectedValue.Trim() == "0") oSolicitud.Antibiotico = false;else oSolicitud.Antibiotico = true;                        
//            if (rdbTransfusion.SelectedValue.Trim() == "0") oSolicitud.Transfusion = false; else oSolicitud.Transfusion = true;            
//            if (rdbCorticoide.SelectedValue.Trim() == "0") oSolicitud.Corticoides = false; else oSolicitud.Corticoides = true;            
//            if (rdbDopamina.SelectedValue.Trim() == "0") oSolicitud.Dopamina = false; else oSolicitud.Dopamina = true;                        
//            if (rdbEnfermedadTiroideaMaterna.SelectedValue.Trim() == "0") oSolicitud.EnfermedadTiroideaMaterna = false;  else oSolicitud.EnfermedadTiroideaMaterna = true;                        
//            oSolicitud.AntecedentesMaternos =txtAntecedenteMaterno.Text.Trim();
//            if (rdbCorticoideMaterno.SelectedValue.Trim() == "0") oSolicitud.CorticoidesMaterno = false;  else oSolicitud.CorticoidesMaterno = true;            
//            oSolicitud.IdSolicitudScreeningOrigen = i_idsolicitud;
//            oSolicitud.IdProtocolo = oRegistro;

//            oSolicitud.NumeroTarjeta =int.Parse( lblNumeroTarjeta.Text);
//            oSolicitud.MedicoSolicitante = lblMedicoSolicitante.Text;
//            oSolicitud.ApellidoMaterno = lblApellidoMaterno.Text;
//            oSolicitud.ApellidoPaterno = lblApellidoPaterno.Text;
//            oSolicitud.NombreParentesco = lblNombreParentesco.Text;
//            oSolicitud.NumerodocumentoParentesco = int.Parse(lblDniMaterno.Text);
//            oSolicitud.FechaNacimientoParentesco = DateTime.Parse(lblFechaNacimientoParentesco.Text);
//            oSolicitud.IdLugarControl =int.Parse( lblIdLugarControl.Text);
    



//            oSolicitud.Save();
            
//            string m_strSQL = @" SELECT descripcion FROM  LAB_TempAlarmaScreening WHERE  idSolicitudScreening =" + i_idsolicitud.ToString();
//            DataSet Ds = new DataSet();
//            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
//            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
//            SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);
//            da.Fill(Ds);
//            for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
//            {
//                SolicitudScreeningAlarma oAlarma = new SolicitudScreeningAlarma();
//                oAlarma.IdSolicitudScreening = oSolicitud;
//                oAlarma.Descripcion = Ds.Tables[0].Rows[i][0].ToString();
//                oAlarma.Save();
//            }

//        }
//        private void CargarItems()
//        {
//            Utility oUtil = new Utility();
//            ///Carga del combo de determinaciones
//            string m_ssql = " SELECT I.idItem as idItem,  I.codigo as nombre " +                           
//                            " FROM Lab_item I  " +
//                            " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
//                            " WHERE A.baja=0 and I.baja=0  and A.idtipoServicio= 4  order by I.nombre ";
//            oUtil.CargarCheckBox(chkItemPesquisa, m_ssql, "idItem", "nombre");

            
//            m_ssql = " SELECT idMotivoRepeticionScreening,  descripcion  FROM LAB_MotivoRepeticionScreening  ";
//            oUtil.CargarCombo(ddlMotivoRepeticion, m_ssql, "idMotivoRepeticionScreening", "descripcion");
//            ddlMotivoRepeticion.Items.Insert(0, new ListItem("----", "0"));                        
            
//        }
       

//        private void CargarAlarmas(int p)
//        {            
//            string m_strSQL = @" SELECT descripcion FROM  LAB_TempAlarmaScreening WHERE  idSolicitudScreening =" + p;

//            DataSet Ds = new DataSet();
//            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
//            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
//            SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);
//            da.Fill(Ds);
//            gvAlarmas.DataSource = Ds.Tables[0];
//            gvAlarmas.DataBind();

//        }

//        private void CargarScreening(int p)
//        {
//            ////lee de la tabla temporal de screening descargados del servidor
//                string m_strSQL = @" SELECT [horaNacimiento]
//                ,[edadGestacional]
//                ,[peso]
//                ,case when [primeraMuestra]=0 then 'No' else 'Si' end as primeraMuestra
//                ,[MotivoRepeticion]
//                ,[fechaExtraccion]
//                ,[horaExtraccion]
//                ,case when [ingestaLeche24Horas]=0 then 'No' else 'Si' end as ingestaLeche24Horas
//                ,[tipoAlimentacion]
//                ,case when [antibiotico]=0 then 'No' else 'Si' end as antibiotico
//                ,case when [transfusion]=0 then 'No' else 'Si' end as transfusion
//                ,case when [corticoides]=0 then 'No' else 'Si' end as corticoide
//                ,case when [dopamina]=0 then 'No' else 'Si' end as dopamina
//                ,case when [enfermedadTiroideaMaterna]=0 then 'No' else 'Si' end enfermedadTiroideaMaterna
//                ,[antecedentesMaternos]
//                ,case when [corticoidesMaterno]=0 then 'No' else 'Si' end as corticoidesMaterno,
//                [numerodocumentoParentesco] as dniMaterno,
//                [apellidoMaterno],
//                [apellidoPaterno],[idLugarControl],
//                [medicoSolicitante],[numeroTarjeta], nombreParentesco, FechaNacimientoParentesco
//                FROM  LAB_TempSolicitudScreening WHERE  idSolicitudScreening =" + p;

//                DataSet Ds = new DataSet();
//                NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
//                String strconn = oConf.GetProperty("hibernate.connection.connection_string");
//                SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);
//                da.Fill(Ds);
             
//                txtHoraNacimiento.Value = Ds.Tables[0].Rows[0][0].ToString();
//                txtEdadGestacional.Value = Ds.Tables[0].Rows[0][1].ToString();
//                txtPeso.Value = Ds.Tables[0].Rows[0][2].ToString().Replace(",0000","") ;

//                if (Ds.Tables[0].Rows[0][3].ToString() == "Si") rdbPrimeraMuestra.Text = "1"; else rdbPrimeraMuestra.Text = "0";
//                ddlMotivoRepeticion.Text = Ds.Tables[0].Rows[0][4].ToString();
//                txtFechaExtraccion.Value = Ds.Tables[0].Rows[0][5].ToString().Substring(0, 10);
//                txtHoraExtraccion.Value = Ds.Tables[0].Rows[0][6].ToString(); ;
//                if (Ds.Tables[0].Rows[0][7].ToString() == "Si") rdbIngestaLeche24Horas.SelectedValue = "1"; else rdbIngestaLeche24Horas.SelectedValue = "0"; 
//                rdbTipoAlimentacion.Text = Ds.Tables[0].Rows[0][8].ToString();

//                if (Ds.Tables[0].Rows[0][9].ToString() == "Si") rdbAntibiotico.SelectedValue = "1"; else rdbAntibiotico.SelectedValue = "0";
//                if (Ds.Tables[0].Rows[0][10].ToString() == "Si") rdbTransfusion.SelectedValue = "1"; else rdbTransfusion.SelectedValue = "0";
//                if (Ds.Tables[0].Rows[0][11].ToString() == "Si") rdbCorticoide.SelectedValue = "1"; else rdbCorticoide.SelectedValue = "0";
//                if (Ds.Tables[0].Rows[0][12].ToString()=="Si") rdbDopamina.SelectedValue = "1"; else rdbDopamina.SelectedValue = "0";
//                if (Ds.Tables[0].Rows[0][13].ToString() == "Si") rdbEnfermedadTiroideaMaterna.SelectedValue = "1"; else rdbEnfermedadTiroideaMaterna.SelectedValue = "0"; 
//                txtAntecedenteMaterno.Text = Ds.Tables[0].Rows[0][14].ToString();
//                if (Ds.Tables[0].Rows[0][15].ToString() == "Si") rdbCorticoideMaterno.SelectedValue = "1"; else rdbCorticoideMaterno.SelectedValue = "0";
//                lblDniMaterno.Text = Ds.Tables[0].Rows[0][16].ToString();

//                lblApellidoMaterno.Text = Ds.Tables[0].Rows[0][17].ToString();
//                lblApellidoPaterno.Text = Ds.Tables[0].Rows[0][18].ToString();

//                Efector oLugarControl = new Efector();
//                oLugarControl = (Efector)oLugarControl.Get(typeof(Efector),int.Parse(Ds.Tables[0].Rows[0][19].ToString()));
//                lblIdLugarControl.Text = oLugarControl.IdEfector.ToString();
//                lblLugarControl.Text = oLugarControl.Nombre;

//                lblMedicoSolicitante.Text = Ds.Tables[0].Rows[0][20].ToString();
//                lblNumeroTarjeta.Text = Ds.Tables[0].Rows[0][21].ToString();


//                lblNombreParentesco.Text = Ds.Tables[0].Rows[0][22].ToString();

//                lblFechaNacimientoParentesco.Text = Ds.Tables[0].Rows[0][23].ToString();



//                CargarDetalleScreening(p);
//        }

//        private void CargarDetalleScreening(int p)
//        {
//            string m_strSQL = @" SELECT [codigo] FROM  [LAB_TempDetalleSolicitudScreening] WHERE  idSolicitudScreening =" + p;
//            DataSet Ds = new DataSet();
//            NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
//            String strconn = oConf.GetProperty("hibernate.connection.connection_string");
//            SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);
//            da.Fill(Ds);

//            for (int i = 0; i <= Ds.Tables[0].Rows.Count; i++)
//            {
//                if (chkItemPesquisa.Items.Count > i)
//                {
//                    if (Ds.Tables[0].Rows[0][i].ToString() == chkItemPesquisa.Items[i].Text) chkItemPesquisa.Items[i].Selected = true;
//                }
//            }
        
//        }
    }
}