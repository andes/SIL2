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

namespace WebLab.Protocolos
{
    public partial class PesquisaNeonatal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public  void InicializarIngreso()
        {
            //CargarEfector(); 
            CargarItems();
            Paciente pac = new Paciente();
            pac = (Paciente)pac.Get(typeof(Paciente),int.Parse( Request["idPaciente"].ToString()));                       

            //Parentesco oDetalle = new Parentesco();
            ISession m_session2 = NHibernateHttpModule.CurrentSession;
            ICriteria crit2 = m_session2.CreateCriteria(typeof(Parentesco));
            crit2.Add(Expression.Eq("IdPaciente", pac));
            IList items2 = crit2.List();
            if (items2.Count > 0)
            {
                foreach (Parentesco oDet in items2)
                {
                    txtDniMaterno.Text = oDet.NumeroDocumento.ToString();
                    txtApellidoMaterno.Text = oDet.Apellido;
                    //lblApellidoPaterno.Text = pac.Apellido;
                    lblMedicoSolicitante.Visible = false;
                    txtNumeroTarjeta.Text = "";
                    txtNombreParentesco.Text = oDet.Nombre;
                    txtFechaNacimientoParentesco.Value = oDet.FechaNacimiento.ToShortDateString();
                }
            }
            else
            {
                txtDniMaterno.Text ="";
                txtApellidoMaterno.Text = "";
           //     lblApellidoPaterno.Text = "";
                lblMedicoSolicitante.Visible = false;
                txtNumeroTarjeta.Text = "";
                txtNombreParentesco.Text = "";
                txtFechaNacimientoParentesco.Value = "";
            }
        }

        //private void CargarEfector()
        //{
        //    Utility oUtil = new Utility();
        //    string  m_ssql = " SELECT idEfector,  nombre  FROM Sys_efector order by nombre ";
        //    oUtil.CargarCombo(ddlLugarControl, m_ssql, "idEfector", "nombre");
        //    ddlLugarControl.Items.Insert(0, new ListItem("--Seleccione--", "0"));     
        //}


        public void MostrarSolicitudScreeening(Business.Data.Laboratorio.SolicitudScreening oSolicitud)
        {
            CargarItems(); 
            //CargarEfector();
           txtHoraNacimiento.Value=   oSolicitud.HoraNacimiento ;
           txtEdadGestacional.Value = oSolicitud.EdadGestacional.ToString(); 
           txtPeso.Value = oSolicitud.Peso.ToString().Replace(",0000","");
           txtFechaExtraccion.Value = oSolicitud.FechaExtraccion.ToShortDateString();
           txtHoraExtraccion.Value = oSolicitud.HoraExtraccion;           
          
           rdbTipoAlimentacion.SelectedValue= oSolicitud.TipoAlimentacion;
           if (!oSolicitud.PrimeraMuestra) rdbPrimeraMuestra.SelectedValue = "0"; else rdbPrimeraMuestra.SelectedValue = "1";                    
           
            HabilitarMotivorepeticion();
           ddlMotivoRepeticion.SelectedValue = oSolicitud.MotivoRepeticion;

           if (!oSolicitud.IngestaLeche24Horas) rdbIngestaLeche24Horas.SelectedValue = "0"; else rdbIngestaLeche24Horas.SelectedValue = "1";
           if (!oSolicitud.Antibiotico) rdbAntibiotico.SelectedValue = "0"; else rdbAntibiotico.SelectedValue = "1";
           if (!oSolicitud.Transfusion) rdbTransfusion.SelectedValue = "0"; else rdbTransfusion.SelectedValue = "1";
           if (!oSolicitud.Corticoides) rdbCorticoide.SelectedValue = "0"; else rdbCorticoide.SelectedValue = "1";
           if (!oSolicitud.Dopamina) rdbDopamina.SelectedValue = "0"; else rdbDopamina.SelectedValue = "1";
           if (!oSolicitud.EnfermedadTiroideaMaterna) rdbEnfermedadTiroideaMaterna.SelectedValue = "0"; else rdbEnfermedadTiroideaMaterna.SelectedValue = "1";

           txtAntecedenteMaterno.Text = oSolicitud.AntecedentesMaternos;
           if (!oSolicitud.CorticoidesMaterno) rdbCorticoideMaterno.SelectedValue = "0"; else rdbCorticoideMaterno.SelectedValue = "1";

          txtNumeroTarjeta.Text = oSolicitud.NumeroTarjeta.ToString();
           lblMedicoSolicitante.Text = oSolicitud.MedicoSolicitante;
           txtApellidoMaterno.Text = oSolicitud.ApellidoMaterno;
           //lblApellidoPaterno.Text = oSolicitud.ApellidoPaterno;
           txtNombreParentesco.Text= oSolicitud.NombreParentesco;
           txtDniMaterno.Text = oSolicitud.NumerodocumentoParentesco.ToString();
           txtFechaNacimientoParentesco.Value=oSolicitud.FechaNacimientoParentesco.ToShortDateString();
            ddlLugarControl.SelectedValue= oSolicitud.IdLugarControl.ToString();

           
           
           ////////////////////////////

           DetalleProtocolo oDetalle = new DetalleProtocolo();
           ISession m_session = NHibernateHttpModule.CurrentSession;
           ICriteria crit = m_session.CreateCriteria(typeof(DetalleProtocolo));
           crit.Add(Expression.Eq("IdProtocolo", oSolicitud.IdProtocolo));
           crit.AddOrder(Order.Asc("IdDetalleProtocolo"));
           IList items = crit.List();
           foreach (DetalleProtocolo oDet in items)
           {
                for (int i = 0; i <chkItemPesquisa.Items.Count; i++)
                {
                    if (oDet.IdItem.Codigo == chkItemPesquisa.Items[i].Text)
                        chkItemPesquisa.Items[i].Selected = true;                                
               }
           }

        }

        public  void GuardarSolicitudScreeening(Business.Data.Laboratorio.Protocolo oRegistro, int i_idsolicitud, string s_operacion)
        {
            //SolicitudScreening oSolicitud = new SolicitudScreening();
            //if (i_idsolicitud>0)  oSolicitud = (SolicitudScreening)oSolicitud.Get(typeof(SolicitudScreening), i_idsolicitud);                       
            SolicitudScreening oSolicitud = new SolicitudScreening();
            if (s_operacion=="Modifica") oSolicitud = (SolicitudScreening)oSolicitud.Get(typeof(SolicitudScreening), "IdProtocolo", oRegistro); 
            
            oSolicitud.HoraNacimiento = txtHoraNacimiento.Value;            
            oSolicitud.EdadGestacional = int.Parse( txtEdadGestacional.Value);
            oSolicitud.Peso = int.Parse(txtPeso.Value);            
            oSolicitud.FechaExtraccion = DateTime.Parse(txtFechaExtraccion.Value.Trim());            
            oSolicitud.HoraExtraccion = txtHoraExtraccion.Value.Trim();
            oSolicitud.MotivoRepeticion = ddlMotivoRepeticion.SelectedValue;
            oSolicitud.TipoAlimentacion = rdbTipoAlimentacion.SelectedItem.Text;
            if (rdbPrimeraMuestra.SelectedValue.Trim() == "0") oSolicitud.PrimeraMuestra = false; else oSolicitud.PrimeraMuestra = true;                                       
            if (rdbIngestaLeche24Horas.SelectedValue.Trim() == "0") oSolicitud.IngestaLeche24Horas = false;else oSolicitud.IngestaLeche24Horas = true;                                  
            if (rdbAntibiotico.SelectedValue.Trim() == "0") oSolicitud.Antibiotico = false;else oSolicitud.Antibiotico = true;                        
            if (rdbTransfusion.SelectedValue.Trim() == "0") oSolicitud.Transfusion = false; else oSolicitud.Transfusion = true;            
            if (rdbCorticoide.SelectedValue.Trim() == "0") oSolicitud.Corticoides = false; else oSolicitud.Corticoides = true;            
            if (rdbDopamina.SelectedValue.Trim() == "0") oSolicitud.Dopamina = false; else oSolicitud.Dopamina = true;                        
            if (rdbEnfermedadTiroideaMaterna.SelectedValue.Trim() == "0") oSolicitud.EnfermedadTiroideaMaterna = false;  else oSolicitud.EnfermedadTiroideaMaterna = true;                        
            oSolicitud.AntecedentesMaternos =txtAntecedenteMaterno.Text.Trim();
            if (rdbCorticoideMaterno.SelectedValue.Trim() == "0") oSolicitud.CorticoidesMaterno = false;  else oSolicitud.CorticoidesMaterno = true;
            if (oRegistro.NumeroOrigen != "") oSolicitud.IdSolicitudScreeningOrigen = int.Parse(oRegistro.NumeroOrigen); else    oSolicitud.IdSolicitudScreeningOrigen = 0;
            oSolicitud.IdProtocolo = oRegistro;

            if (txtNumeroTarjeta.Text != "") oSolicitud.NumeroTarjeta = int.Parse(txtNumeroTarjeta.Text); else oSolicitud.NumeroTarjeta =0; 
            oSolicitud.MedicoSolicitante = lblMedicoSolicitante.Text;
            oSolicitud.ApellidoMaterno = txtApellidoMaterno.Text;
            oSolicitud.ApellidoPaterno = oRegistro.IdPaciente.Apellido;// lblApellidoPaterno.Text;
            oSolicitud.NombreParentesco = txtNombreParentesco.Text;
           if (txtDniMaterno.Text!="") oSolicitud.NumerodocumentoParentesco = int.Parse(txtDniMaterno.Text); else oSolicitud.NumerodocumentoParentesco =0;

            if (txtFechaNacimientoParentesco.Value!="")
            oSolicitud.FechaNacimientoParentesco = DateTime.Parse(txtFechaNacimientoParentesco.Value);
            else
            oSolicitud.FechaNacimientoParentesco = DateTime.Parse("01/01/1900");
            oSolicitud.IdLugarControl =int.Parse(ddlLugarControl.SelectedValue);

            if (hfFechaRegistro.Value != "")
                oSolicitud.FechaCargaOrigen = DateTime.Parse(hfFechaRegistro.Value);
            else oSolicitud.FechaCargaOrigen = oRegistro.FechaRegistro;

            if (hfFechaEnvio.Value != "")
                oSolicitud.FechaEnvioOrigen = DateTime.Parse(hfFechaEnvio.Value);
            else
                oSolicitud.FechaEnvioOrigen = oRegistro.FechaRegistro;
            
            oSolicitud.Save();

            oSolicitud.GuardarParentesco();

            GuardarAlarmas(oSolicitud);         

        }

       
        protected void cvNumeros_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Utility oUtil = new Utility();
            if (txtNumeroTarjeta.Text != "") { if (oUtil.EsEntero(txtNumeroTarjeta.Text)) args.IsValid = true; else args.IsValid = false; }
            else
                args.IsValid = true;
        }
        private void GuardarAlarmas(SolicitudScreening oSolicitud)
        {
            oSolicitud.EliminarAlarmas();
         

            string descripcionAlarma = "";////Alarma: (fecha/hora nacimiento-fecha/hora extraccion)<36 --> Generar alarma.                       

            double horasolaNac = double.Parse(oSolicitud.HoraNacimiento.Substring(0, 2));
            double minutosoloNac = double.Parse(oSolicitud.HoraNacimiento.Substring(3, 2));
            DateTime fechaHoraNacimiento = DateTime.Parse(oSolicitud.IdProtocolo.IdPaciente.FechaNacimiento.ToShortDateString()).AddHours(horasolaNac).AddMinutes(minutosoloNac);

            double horasolaExt = double.Parse(oSolicitud.HoraExtraccion.Substring(0, 2));
            double minutosoloExt = double.Parse(oSolicitud.HoraExtraccion.Substring(3, 2));
            DateTime fechaHoraExtraccion = DateTime.Parse(oSolicitud.FechaExtraccion.ToShortDateString()).AddHours(horasolaExt).AddMinutes(minutosoloExt);


            TimeSpan diferencia = fechaHoraExtraccion - fechaHoraNacimiento;
            double diferenciasHoras = diferencia.TotalHours;

            int s_idusuario = int.Parse(Session["idUsuario"].ToString());
            if (diferenciasHoras < 36)
            {
                descripcionAlarma = "La extracción se realizó antes de las 36 horas de vida.";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (oSolicitud.EdadGestacional < 35)/// es prematuro
            {
                descripcionAlarma = "Deberá repetirse la muestra a los 15 días: La edad gestacional es menor a 35 semanas.";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (oSolicitud.Transfusion)
            {
                descripcionAlarma = "Deberá repetirse la muestra a los 7 días: Por terapia transfuncional.";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (!oSolicitud.IngestaLeche24Horas)
            {
                descripcionAlarma = "El RN no tuvo ingesta de leche en las primeras 24 horas";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (oSolicitud.Antibiotico)
            {
                descripcionAlarma = "Deberá repetirse la muestra: Por ingesta de antibióticos";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (oSolicitud.Corticoides)
            {
                descripcionAlarma = "Deberá repetirse la muestra: Por ingesta de corticoides";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (oSolicitud.Dopamina)
            {
                descripcionAlarma = "Deberá repetirse la muestra: Por ingesta de dopamina";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }

            if (oSolicitud.TipoAlimentacion == "2")
            {
                descripcionAlarma = "Deberá repetirse la muestra: Al normalizar la alimentación";
                oSolicitud.GuardarDescripcionAlarma(descripcionAlarma, oSolicitud, s_idusuario);
            }
        }

      

       

        private void CargarItems()
        {
            Utility oUtil = new Utility();
            ///Carga del combo de determinaciones
            string m_ssql = " SELECT I.idItem as idItem,  I.codigo as nombre " +
                            " FROM Lab_item I  " +
                            " INNER JOIN Lab_area A ON A.idArea= I.idArea " +
                            " WHERE A.baja=0 and I.baja=0  and A.idtipoServicio= 4  order by I.codigo ";
            oUtil.CargarCheckBox(chkItemPesquisa, m_ssql, "idItem", "nombre");


            m_ssql = " SELECT idMotivoRepeticionScreening,  descripcion  FROM LAB_MotivoRepeticionScreening  ";
            oUtil.CargarCombo(ddlMotivoRepeticion, m_ssql, "idMotivoRepeticionScreening", "descripcion");
            ddlMotivoRepeticion.Items.Insert(0, new ListItem("--Seleccione--", "0"));
            
            m_ssql = " SELECT idEfector,  nombre  FROM Sys_efector order by nombre ";
            oUtil.CargarCombo(ddlLugarControl, m_ssql, "idEfector", "nombre");
            ddlLugarControl.Items.Insert(0, new ListItem("--Seleccione--", "0"));     
                             
            
        }
       


        public void CargarScreening(int p)
        {
            //CargarEfector(); 
            CargarItems();

            ////lee de la tabla temporal de screening descargados del servidor
                string m_strSQL = @" SELECT [horaNacimiento]
                ,[edadGestacional]
                ,[peso]
                ,case when [primeraMuestra]=0 then 'No' else 'Si' end as primeraMuestra
                ,[idMotivoRepeticion]
                ,[fechaExtraccion]
                ,[horaExtraccion]
                ,case when [ingestaLeche24Horas]=0 then 'No' else 'Si' end as ingestaLeche24Horas
                ,[tipoAlimentacion]
                ,case when [antibiotico]=0 then 'No' else 'Si' end as antibiotico
                ,case when [transfusion]=0 then 'No' else 'Si' end as transfusion
                ,case when [corticoides]=0 then 'No' else 'Si' end as corticoide
                ,case when [dopamina]=0 then 'No' else 'Si' end as dopamina
                ,case when [enfermedadTiroideaMaterna]=0 then 'No' else 'Si' end enfermedadTiroideaMaterna
                ,[antecedentesMaternos]
                ,case when [corticoidesMaterno]=0 then 'No' else 'Si' end as corticoidesMaterno,
                [numerodocumentoParentesco] as dniMaterno,
                [apellidoMaterno],
                [apellidoPaterno], '' as LugarControl,
                [medicoSolicitante],[numeroTarjeta], nombreParentesco, FechaNacimientoParentesco, LAB_TempSolicitudScreening.idLugarControl,
fechaRegistro as fechaCarga,fechaEnvio as fechaEnvio, analisis
                FROM  LAB_TempSolicitudScreening 
                
WHERE  idSolicitudScreening =" + p;
            //INNER JOIN SYS_EFECTOR ON sYS_EFECTOR.IDeFECTOR=LAB_TempSolicitudScreening.idLugarControl
                DataSet Ds = new DataSet();
                NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
                String strconn = oConf.GetProperty("hibernate.connection.connection_string");
                SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);
                
                da.Fill(Ds);

                txtHoraNacimiento.Value = Ds.Tables[0].Rows[0][0].ToString();
                txtEdadGestacional.Value = Ds.Tables[0].Rows[0][1].ToString();
                txtPeso.Value = Ds.Tables[0].Rows[0][2].ToString().Replace(",0000","") ;

                if (Ds.Tables[0].Rows[0][3].ToString() == "Si") rdbPrimeraMuestra.Text = "1"; else rdbPrimeraMuestra.Text = "0";
                HabilitarMotivorepeticion();
                ddlMotivoRepeticion.Text = Ds.Tables[0].Rows[0][4].ToString();
                txtFechaExtraccion.Value = Ds.Tables[0].Rows[0][5].ToString().Substring(0, 10);
                txtHoraExtraccion.Value = Ds.Tables[0].Rows[0][6].ToString(); ;
                if (Ds.Tables[0].Rows[0][7].ToString() == "Si") rdbIngestaLeche24Horas.SelectedValue = "1"; else rdbIngestaLeche24Horas.SelectedValue = "0"; 
                rdbTipoAlimentacion.Text = Ds.Tables[0].Rows[0][8].ToString();

                if (Ds.Tables[0].Rows[0][9].ToString() == "Si") rdbAntibiotico.SelectedValue = "1"; else rdbAntibiotico.SelectedValue = "0";
                if (Ds.Tables[0].Rows[0][10].ToString() == "Si") rdbTransfusion.SelectedValue = "1"; else rdbTransfusion.SelectedValue = "0";
                if (Ds.Tables[0].Rows[0][11].ToString() == "Si") rdbCorticoide.SelectedValue = "1"; else rdbCorticoide.SelectedValue = "0";
                if (Ds.Tables[0].Rows[0][12].ToString()=="Si") rdbDopamina.SelectedValue = "1"; else rdbDopamina.SelectedValue = "0";
                if (Ds.Tables[0].Rows[0][13].ToString() == "Si") rdbEnfermedadTiroideaMaterna.SelectedValue = "1"; else rdbEnfermedadTiroideaMaterna.SelectedValue = "0"; 
                txtAntecedenteMaterno.Text = Ds.Tables[0].Rows[0][14].ToString();
                if (Ds.Tables[0].Rows[0][15].ToString() == "Si") rdbCorticoideMaterno.SelectedValue = "1"; else rdbCorticoideMaterno.SelectedValue = "0";
                txtDniMaterno.Text = Ds.Tables[0].Rows[0][16].ToString();

                txtApellidoMaterno.Text = Ds.Tables[0].Rows[0][17].ToString();           

                ddlLugarControl.SelectedValue = Ds.Tables[0].Rows[0][24].ToString(); 
            

                lblMedicoSolicitante.Text = Ds.Tables[0].Rows[0][20].ToString();
                txtNumeroTarjeta.Text = Ds.Tables[0].Rows[0][21].ToString();
                txtNombreParentesco.Text = Ds.Tables[0].Rows[0][22].ToString();
                txtFechaNacimientoParentesco.Value =DateTime.Parse( Ds.Tables[0].Rows[0][23].ToString()).ToShortDateString();

                hfFechaRegistro.Value = Ds.Tables[0].Rows[0][25].ToString(); ///fecharegistro
                     hfFechaEnvio.Value = Ds.Tables[0].Rows[0][26].ToString(); ///fechaenvio
                                                                                           ///
                  string   s_analisis = Ds.Tables[0].Rows[0][27].ToString();
                     string[] tabla = s_analisis.Split('|');
                     for (int i = 0; i <= tabla.Length - 1; i++)
                     {
                         for (int j = 0; j < chkItemPesquisa.Items.Count; j++)
                         {
                             if (tabla[i].ToString() == chkItemPesquisa.Items[j].Text) chkItemPesquisa.Items[j].Selected = true;

                         }
                        

                     }
                ///poner en HiddenField o como sea Label fecha de carga y la fecha de envio del pedido
            //    CargarDetalleScreening(p);
        }



        protected void rdbSeleccionarItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbSeleccionarItem.SelectedValue == "1")
                MarcarTodas(true);
            else
                MarcarTodas(false);
            chkItemPesquisa.UpdateAfterCallBack = true;
        }

        private void MarcarTodas(bool p)
        {

            for (int i = 0; i < chkItemPesquisa.Items.Count; i++)
                chkItemPesquisa.Items[i].Selected = p;

        }

        //private void CargarDetalleScreening(int p)
        //{
        //    string m_strSQL = @" SELECT [codigo] FROM  [LAB_TempDetalleSolicitudScreening] WHERE  idSolicitudScreening =" + p;

        //    DataSet Ds = new DataSet();
        //    NHibernate.Cfg.Configuration oConf = new NHibernate.Cfg.Configuration();
        //    String strconn = oConf.GetProperty("hibernate.connection.connection_string");
        //    SqlDataAdapter da = new SqlDataAdapter(m_strSQL, strconn);

        //    da.Fill(Ds);

        //    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
        //    {
        //        for (int j = 0; j < chkItemPesquisa.Items.Count; j++)
        //        {
        //            if (Ds.Tables[0].Rows[i][0].ToString() == chkItemPesquisa.Items[j].Text) chkItemPesquisa.Items[j].Selected = true;

        //        }
        //    }
        
        //}

        protected void cvMotivoRepeticion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (rdbPrimeraMuestra.SelectedValue == "0")
            {
                if (ddlMotivoRepeticion.SelectedValue == "0") { args.IsValid = false; this.cvMotivoRepeticion.ErrorMessage = "Debe seleccionar un motivo de repetición"; }
                else args.IsValid = true;
            }
            else args.IsValid = true;
        }

        protected void rdbPrimeraMuestra_SelectedIndexChanged(object sender, EventArgs e)
        {
            HabilitarMotivorepeticion();
        }

        private void HabilitarMotivorepeticion()
        {
            ddlMotivoRepeticion.SelectedValue = "0";
            if (rdbPrimeraMuestra.SelectedValue == "0") ddlMotivoRepeticion.Enabled = true; else ddlMotivoRepeticion.Enabled = false;
            ddlMotivoRepeticion.UpdateAfterCallBack = true;
        }
    }
}