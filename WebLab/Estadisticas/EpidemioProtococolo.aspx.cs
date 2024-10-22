using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace WebLab.Estadisticas
{
    public partial class EpidemioProtococolo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

              


                if (Request["protocolo"] != null)
                {
                    txtProtocolo.Text = Request["protocolo"].ToString();
                       CargarDatos(); }
            }
        }

        private void CargarDatos()
        {
            string a = Server.MapPath("") + "\\epi.xml";

            XDocument doc = XDocument.Load(a);
            foreach (XElement cell in doc.Element("ficha").Elements("antecedentes"))
            {
                if (cell.Element("protocolo").Value == Request["protocolo"].ToString())
                {
                    //cell.Element("protocolo").Value = "otro";
                    rdbAnteViajeZona.SelectedValue = cell.Element("Viaje_Z_Riesgo_14_dias_previos").Value;
                    txtAnteLugar.Text = cell.Element("Lugar").Value;
                    txtFechaViajeRetorno.Value = cell.Element("Fecha_ing").Value;
                    rdbAnteMedio.SelectedValue = cell.Element("Medio_trans").Value;

                    rdbAnteContactoIRA.SelectedValue = cell.Element("Contacto_con_personas_con_IRA_14_dias_previos").Value;
                    rdbAnteEntornoFliar.SelectedValue = cell.Element("Entorno_familiar_IRA").Value;
                    rdbAnteEntornoAsistencial.SelectedValue = cell.Element("Entorno_asistencial_IRA").Value;
                    txtanteOtroentorno.Text = cell.Element("Otro_entorno_IRA").Value;

                    rdbAnteContactoCovid.SelectedValue = cell.Element("Contacto_con_Confirmados_Covid").Value;
                    rdbAnteContactoCovidEntornoFliar.SelectedValue = cell.Element("Entorno_Covid_familiar").Value;
                    rdbAnteContactoCovidEntornoAsistencial.SelectedValue = cell.Element("Entorno_Covid_asistencial").Value;
                    txtAnteContactoCovidOtroEntorno.Text = cell.Element("Otro_entorno_Covid").Value;
                    txtFechaInicioSintomas.Value = cell.Element("Fecha_Inicio_sintomas").Value;
                    txtFechaPrimerConsulta.Value = cell.Element("Fecha_Primera_consulta").Value;
                    rdbInternacion.SelectedValue = cell.Element("Internacion").Value;

                    txtFechaInternacion.Value = cell.Element("Fecha_Internacion").Value;
                    rdbUTI.SelectedValue = cell.Element("UTI").Value;
                    txtFechainternacionUti.Value = cell.Element("Fecha_Int_UTI").Value;
                    rdbARM.SelectedValue = cell.Element("ARM").Value;
                    rdbDiagnosticoIngreso.SelectedValue = cell.Element("Dx_ingreso").Value;
                    txtOtrosDiagnostico.Text = cell.Element("Dx_Otros").Value;


                    rdbComorbilidades.SelectedValue = cell.Element("Comorbilidades").Value;
                    rdbComorbilidadEnfResp.SelectedValue = cell.Element("Com_Enf_Resp").Value; ;
                    rdbComorbilidadEnfNeuro.SelectedValue = cell.Element("Com_Enf_Neuro").Value; ;
                    rdbComorbilidadInmunosupresion.SelectedValue = cell.Element("Com_Inmunosupresion").Value;
                    rdbComorbilidadEnfCardio.SelectedValue = cell.Element("Com_Enf_cardio").Value;
                    rdbComorbilidadHTA.SelectedValue = cell.Element("Com_HTA").Value; ;
                    rdbComorbilidadRenal.SelectedValue = cell.Element("Com_Renal").Value; ;
                    rdbComorbilidadHepatica.SelectedValue = cell.Element("Com_Hepatica").Value; ;
                    rdbComorbilidadDBT.SelectedValue = cell.Element("Com_DBT").Value; ;
                    rdbComorbilidadDesnutricion.SelectedValue = cell.Element("Com_Desnutricion").Value; ;
                    rdbIMC_menor_30.SelectedValue = cell.Element("Com_IMC_Mayor_30").Value; ;
                    rdbComorbilidadPerinatales.SelectedValue = cell.Element("Com_Perinatales").Value; ;
                    rdbComorbilidadTtoAtbPrevio.SelectedValue = cell.Element("Com_Tto_Atb_Previo").Value; ;
                    txtFechaInicioComorbilidadAtbPrevio.Value = cell.Element("Com_Fecha_Tto_Atb_Previo").Value; ;
                    rdbComorbilidadTtoantiviralPrevio.SelectedValue = cell.Element("Com_Tto_antiviral_previo").Value; ;
                    txtFechaInicioComorbilidadAntiviralPrevio.Value = cell.Element("Com_Fecha_Tto_antiviral_previo").Value; ;
                    rdbComorbilidadAntigripal.SelectedValue = cell.Element("Com_Antigripal_2020").Value; ;


                    rdbSignosSintomatico.SelectedValue = cell.Element("SyS_Sintomatico").Value;
                    rdbSignosFiebre.SelectedValue = cell.Element("Sys_Fiebre_38_o_mas").Value; ;
                    rdbSignosRisnorrea.SelectedValue = cell.Element("Sys_Rinorrea").Value; ;
                    rdbSignosTos.SelectedValue = cell.Element("Sys_tos").Value; ;
                    rdbSignosDisnea.SelectedValue = cell.Element("Sys_Disnea").Value; ;
                    rdbSignosTaquipnea.SelectedValue = cell.Element("Sys_Taquipnea").Value; ;
                    rdbSignosDifResp.SelectedValue = cell.Element("Sys_Dif_Resp").Value; ;
                    rdbSignosAnosmia.SelectedValue = cell.Element("Sys_Anosmia").Value; ;
                    rdbSignosDisgeusia.SelectedValue = cell.Element("Sys_Disgeusia").Value; ;
                    rdbSignosMalestar.SelectedValue = cell.Element("Sys_Malestar_General").Value; ;
                    rdbSignosAltralgia.SelectedValue = cell.Element("Sys_Artralgias").Value; ;
                    rdbSignosMialgia.SelectedValue = cell.Element("Sys_Milagias").Value; ;
                    rdbSignosOdinofagia.SelectedValue = cell.Element("Sys_Odinofagia").Value; ;
                    rdbSignosOtalgia.SelectedValue = cell.Element("Sys_Otalgia").Value; ;
                    rdbSignosCefalea.SelectedValue = cell.Element("Sys_Cefalea").Value; ;

                    rdbSignosVomito.SelectedValue = cell.Element("Sys_Vomitos").Value; ;
                    rdbSignosDiarrea.SelectedValue = cell.Element("Sys_Diarrea").Value; ;
                    rdbSignosDolorAbdominal.SelectedValue = cell.Element("Sys_Dolor_abdominal").Value; ;
                    txtOtrosSintomas.Text = cell.Element("Sys_Otros_Sintomas").Value; ;
                    rdbSignosRx.SelectedValue = cell.Element("Sys_RX").Value; ;
                    txtSignosRxHallazgo.Text = cell.Element("Sys_RX_Hallazgo").Value; ;

                    txtLeucocitos.Text = cell.Element("Lab_Leucocitos").Value; ;
                    txtVSG.Text = cell.Element("Lab_VSG").Value; ;
                    txtLDH.Text = cell.Element("Lab_LDH").Value; ;
                    txtTGO.Text = cell.Element("Lab_TGO").Value; ;
                    txtFAL.Text = cell.Element("Lab_FAL").Value; ;
                    txtCPK.Text = cell.Element("Lab_CPK").Value; ;
                    txtTP.Text = cell.Element("Lab_TP").Value; ;
                    txtUrea.Text = cell.Element("Lab_Urea").Value; ;
                    txtCreatinina.Text = cell.Element("Lab_Creatinina").Value; ;
                    txtPCR.Text = cell.Element("Lab_PCR").Value; 
                    txtFechaPCR.Value = cell.Element("Diag_PCR_Fecha").Value; ;
                    txtTipoMuestra.Text = cell.Element("Diag_PCR_Tipo_muestra").Value; ;
                    txtResultado.Text = cell.Element("Diag_PCR_Resultado").Value; ;

                    rdbB2.SelectedValue= cell.Element("Tratamiento_B2").Value; ;
                    rdbO2.SelectedValue = cell.Element("Tratamiento_O2").Value; ;
                    rdbCorticoides.SelectedValue = cell.Element("Tratamiento_Corticoides").Value; ;
                    rdbATB.SelectedValue = cell.Element("Tratamiento_ATB").Value; ;
                    rdbAntiviral.SelectedValue = cell.Element("Tratamiento_Antiviral").Value; ;
                     txtOtrosTratamientos.Text= cell.Element("Tratamiento_Otros").Value; ;
                 
                    rdbComplicaciones.SelectedValue= cell.Element("Evolucion_Complicaciones").Value; ;
                    txtComplicaciones.Text = cell.Element("Evolucion_CualComplicacion").Value; ;
  rdbEstadoEgreso.SelectedValue = cell.Element("Evolucion_Egreso").Value; ;
                    txtFechaEgreso.Value= cell.Element("Evolucion_Fecha_Egreso").Value; ;
     


                }
            }

            //doc.Save(a);

             
              
            


                //lblCantidad.Text = GridView1.Rows.Count.ToString() + " presentaciones";

            }
       
 
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            /*********grabar uno nuevo*********************/
            if (Request["protocolo"] == null)
            {
                GrabarNuevo();
            }
           
            else
            { Modificar(); }

            Response.Redirect("EpidemioProtocoloList.aspx", false);

            //string Valor = ds.Tables["antecedentes"].Rows[0]["cedula"].ToString(); //Leemos el primer valor (Rows[0])

            ///*******Para Guardar un valor*******************/
            //ds.Tables["antecedentes"].Rows[i]["protocolo"] = txtProtocolo.Text;
            //ds.Tables["antecedentes"].Rows[i]["Viaje_Z_Riesgo_14_dias_previos"] = rdbAnteViajeZona.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Lugar"] = txtAnteLugar.Text;
            //ds.Tables["antecedentes"].Rows[i]["Fecha_ing"] = txtFechaViajeRetorno.Value;
            //ds.Tables["antecedentes"].Rows[i]["Medio_trans"] = rdbAnteMedio.SelectedValue;

            //ds.Tables["antecedentes"].Rows[i]["Contacto_con_personas_con_IRA_14_dias_previos"] = rdbAnteContactoIRA.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Entorno_familiar_IRA"] = rdbAnteEntornoFliar.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Entorno_asistencial_IRA"] = rdbAnteEntornoAsistencial.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Otro_entorno_IRA"] = txtanteOtroentorno.Text;

            //ds.Tables["antecedentes"].Rows[i]["Contacto_con_Confirmados_Covid"] = rdbAnteContactoCovid.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Entorno_Covid_familiar"] = rdbAnteContactoCovidEntornoFliar.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Entorno_Covid_asistencial"] = rdbAnteContactoCovidEntornoAsistencial.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Otro_entorno_Covid"] = txtAnteContactoCovidOtroEntorno.Text;
            //ds.Tables["antecedentes"].Rows[i]["Fecha_Inicio_sintomas"] = txtFechaInicioSintomas.Value;
            //ds.Tables["antecedentes"].Rows[i]["Fecha_Primera_consulta"] = txtFechaPrimerConsulta.Value;
            //ds.Tables["antecedentes"].Rows[i]["Internacion"] = rdbInternacion.SelectedValue;

            //ds.Tables["antecedentes"].Rows[i]["Fecha_Internacion"] = txtFechaInternacion.Value;
            //ds.Tables["antecedentes"].Rows[i]["UTI"] = rdbUTI.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Fecha_Int_UTI"] = txtFechainternacionUti.Value;
            //ds.Tables["antecedentes"].Rows[i]["ARM"] = rdbARM.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Dx_ingreso"] = rdbDiagnosticoIngreso.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Dx_Otros"] = txtOtrosDiagnostico.Text;
            

            //ds.Tables["antecedentes"].Rows[i]["Comorbilidades"] = rdbComorbilidades.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Enf_Resp"] = rdbComorbilidadEnfResp.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Enf_Neuro"] = rdbComorbilidadEnfNeuro.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Inmunosupresion"] = rdbComorbilidadInmunosupresion.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Enf_cardio"] = rdbComorbilidadEnfCardio.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_HTA"] = rdbComorbilidadHTA.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Renal"] = rdbComorbilidadRenal.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Hepatica"] = rdbComorbilidadHepatica.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_DBT"] = rdbComorbilidadDBT.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Desnutricion"] = rdbComorbilidadDesnutricion.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_IMC_Mayor_30"] = rdbIMC_menor_30.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Perinatales"] = rdbComorbilidadPerinatales.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Tto_Atb_Previo"] = rdbComorbilidadTtoAtbPrevio.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Fecha_Tto_Atb_Previo"] = txtFechaInicioComorbilidadAtbPrevio.Value;
            //ds.Tables["antecedentes"].Rows[i]["Com_Tto_antiviral_previo"] = rdbComorbilidadTtoantiviralPrevio.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Com_Fecha_Tto_antiviral_previo"] = txtFechaInicioComorbilidadAntiviralPrevio.Value;
            //ds.Tables["antecedentes"].Rows[i]["Com_Antigripal_2020"] = rdbComorbilidadAntigripal.SelectedValue;


            //ds.Tables["antecedentes"].Rows[i]["SyS_Sintomatico"] = rdbSignosSintomatico.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Fiebre_38_o_mas"] = rdbSignosFiebre.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Rinorrea"] = rdbSignosRisnorrea.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_tos"] = rdbSignosTos.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Disnea"] = rdbSignosDisnea.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Taquipnea"] = rdbSignosTaquipnea.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Dif_Resp"] = rdbSignosDifResp.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Anosmia"] = rdbSignosAnosmia.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Disgeusia"] = rdbSignosDisgeusia.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Malestar_General"] = rdbSignosMalestar.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Artralgias"] = rdbSignosAltralgia.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Milagias"] = rdbSignosMialgia.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Odinofagia"] = rdbSignosOdinofagia.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Otalgia"] = rdbSignosOtalgia.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Cefalea"] = rdbSignosCefalea.SelectedValue;

            //ds.Tables["antecedentes"].Rows[i]["Sys_Vomitos"] = rdbSignosVomito.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Diarrea"] = rdbSignosDiarrea.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Dolor_abdominal"] = rdbSignosDolorAbdominal.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_Otros_Sintomas"] = txtOtrosSintomas.Text;
            //ds.Tables["antecedentes"].Rows[i]["Sys_RX"] = rdbSignosRx.SelectedValue;
            //ds.Tables["antecedentes"].Rows[i]["Sys_RX_Hallazgo"] = txtSignosRxHallazgo.Text;

             

            //ds.WriteXml(a); // Guardamos las modificaciones
        }

        private void GrabarNuevo()
        {
            string a = Server.MapPath("") + "\\epi.xml";
            XDocument xDoc = XDocument.Load(a);
            
            var sub = new XElement("antecedentes",
                new  XElement( "protocolo", txtProtocolo.Text),
                           new XElement("Viaje_Z_Riesgo_14_dias_previos", rdbAnteViajeZona.SelectedValue),
                           new XElement("Lugar", txtAnteLugar.Text),
                           new XElement("Fecha_ing", txtFechaViajeRetorno.Value),
                            new XElement("Medio_trans", rdbAnteMedio.SelectedValue),

            new XElement("Contacto_con_personas_con_IRA_14_dias_previos", rdbAnteContactoIRA.SelectedValue),
            new XElement("Entorno_familiar_IRA", rdbAnteEntornoFliar.SelectedValue),
            new XElement("Entorno_asistencial_IRA", rdbAnteEntornoAsistencial.SelectedValue),
            new XElement("Otro_entorno_IRA", txtanteOtroentorno.Text),

            new XElement("Contacto_con_Confirmados_Covid", rdbAnteContactoCovid.SelectedValue),
            new XElement("Entorno_Covid_familiar", rdbAnteContactoCovidEntornoFliar.SelectedValue),
            new XElement("Entorno_Covid_asistencial", rdbAnteContactoCovidEntornoAsistencial.SelectedValue),
            new XElement("Otro_entorno_Covid", txtAnteContactoCovidOtroEntorno.Text),
            new XElement("Fecha_Inicio_sintomas", txtFechaInicioSintomas.Value),
            new XElement("Fecha_Primera_consulta", txtFechaPrimerConsulta.Value),
            new XElement("Internacion", rdbInternacion.SelectedValue),

            new XElement("Fecha_Internacion", txtFechaInternacion.Value),
            new XElement("UTI", rdbUTI.SelectedValue),
            new XElement("Fecha_Int_UTI", txtFechainternacionUti.Value),
            new XElement("ARM", rdbARM.SelectedValue),
            new XElement("Dx_ingreso", rdbDiagnosticoIngreso.SelectedValue),
            new XElement("Dx_Otros", txtOtrosDiagnostico.Text),


            new XElement("Comorbilidades", rdbComorbilidades.SelectedValue),
            new XElement("Com_Enf_Resp", rdbComorbilidadEnfResp.SelectedValue),
            new XElement("Com_Enf_Neuro", rdbComorbilidadEnfNeuro.SelectedValue),
            new XElement("Com_Inmunosupresion", rdbComorbilidadInmunosupresion.SelectedValue),
            new XElement("Com_Enf_cardio", rdbComorbilidadEnfCardio.SelectedValue),
            new XElement("Com_HTA", rdbComorbilidadHTA.SelectedValue),
            new XElement("Com_Renal", rdbComorbilidadRenal.SelectedValue),
            new XElement("Com_Hepatica", rdbComorbilidadHepatica.SelectedValue),
            new XElement("Com_DBT", rdbComorbilidadDBT.SelectedValue),
            new XElement("Com_Desnutricion", rdbComorbilidadDesnutricion.SelectedValue),
            new XElement("Com_IMC_Mayor_30", rdbIMC_menor_30.SelectedValue),
            new XElement("Com_Perinatales", rdbComorbilidadPerinatales.SelectedValue),
            new XElement("Com_Tto_Atb_Previo", rdbComorbilidadTtoAtbPrevio.SelectedValue),
            new XElement("Com_Fecha_Tto_Atb_Previo", txtFechaInicioComorbilidadAtbPrevio.Value),
            new XElement("Com_Tto_antiviral_previo", rdbComorbilidadTtoantiviralPrevio.SelectedValue),
            new XElement("Com_Fecha_Tto_antiviral_previo", txtFechaInicioComorbilidadAntiviralPrevio.Value),
            new XElement("Com_Antigripal_2020", rdbComorbilidadAntigripal.SelectedValue),


            new XElement("SyS_Sintomatico", rdbSignosSintomatico.SelectedValue),
            new XElement("Sys_Fiebre_38_o_mas", rdbSignosFiebre.SelectedValue),
            new XElement("Sys_Rinorrea", rdbSignosRisnorrea.SelectedValue),
            new XElement("Sys_tos", rdbSignosTos.SelectedValue),
            new XElement("Sys_Disnea", rdbSignosDisnea.SelectedValue),
            new XElement("Sys_Taquipnea", rdbSignosTaquipnea.SelectedValue),
            new XElement("Sys_Dif_Resp", rdbSignosDifResp.SelectedValue),
            new XElement("Sys_Anosmia", rdbSignosAnosmia.SelectedValue),
            new XElement("Sys_Disgeusia", rdbSignosDisgeusia.SelectedValue),
            new XElement("Sys_Malestar_General", rdbSignosMalestar.SelectedValue),
            new XElement("Sys_Artralgias", rdbSignosAltralgia.SelectedValue),
            new XElement("Sys_Milagias", rdbSignosMialgia.SelectedValue),
            new XElement("Sys_Odinofagia", rdbSignosOdinofagia.SelectedValue),
            new XElement("Sys_Otalgia", rdbSignosOtalgia.SelectedValue),
            new XElement("Sys_Cefalea", rdbSignosCefalea.SelectedValue),

            new XElement("Sys_Vomitos", rdbSignosVomito.SelectedValue),
            new XElement("Sys_Diarrea", rdbSignosDiarrea.SelectedValue),
            new XElement("Sys_Dolor_abdominal", rdbSignosDolorAbdominal.SelectedValue),
            new XElement("Sys_Otros_Sintomas", txtOtrosSintomas.Text),
            new XElement("Sys_RX", rdbSignosRx.SelectedValue),
            new XElement("Sys_RX_Hallazgo", txtSignosRxHallazgo.Text),

             new XElement("Lab_Leucocitos", txtLeucocitos.Text),
             new XElement("Lab_VSG", txtVSG.Text),
             new XElement("Lab_LDH", txtLDH.Text),
             new XElement("Lab_TGO", txtTGO.Text),
             new XElement("Lab_FAL", txtFAL.Text),
             new XElement("Lab_CPK", txtCPK.Text),

              new XElement("Lab_TP", txtTP.Text),
             new XElement("Lab_Urea", txtUrea.Text),
             new XElement("Lab_Creatinina", txtCreatinina.Text),
             new XElement("Lab_PCR", txtPCR.Text),
             new XElement("Diag_PCR_Fecha", txtFechaPCR.Value),
             new XElement("Diag_PCR_Tipo_muestra", txtTipoMuestra.Text),
             new XElement("Diag_PCR_Resultado", txtTipoMuestra.Text),


              new XElement("Tratamiento_B2", rdbB2.SelectedValue),
                 new XElement("Tratamiento_O2", rdbO2.SelectedValue),
            new XElement("Tratamiento_Corticoides", rdbCorticoides.SelectedValue),
                   new XElement("Tratamiento_ATB", rdbATB.SelectedValue),
          new XElement("Tratamiento_Antiviral", rdbAntiviral.SelectedValue),
                     new XElement("Tratamiento_Otros", txtOtrosTratamientos.Text),

  new XElement("Evolucion_Complicaciones", rdbComplicaciones.SelectedValue),
                 new XElement("Evolucion_CualComplicacion", txtComplicaciones.Text),
            new XElement("Evolucion_Egreso", rdbEstadoEgreso.SelectedValue),
                   new XElement("Evolucion_Fecha_Egreso", txtFechaEgreso.Value) 
       
                    
          




            );  

            xDoc.Root.Add(sub);
            xDoc.Save(a);
        }

        private void Modificar()
        {
            string a = Server.MapPath("") + "\\epi.xml";

            XDocument doc = XDocument.Load(a);
            foreach (XElement cell in doc.Element("ficha").Elements("antecedentes"))
            {
                if (cell.Element("protocolo").Value == Request["protocolo"].ToString())
                {
                    //cell.Element("protocolo").Value = "otro";
                    cell.Element("Viaje_Z_Riesgo_14_dias_previos").Value = rdbAnteViajeZona.SelectedValue;
                    cell.Element("Lugar").Value = txtAnteLugar.Text;

                    cell.Element("Fecha_ing").Value = txtFechaViajeRetorno.Value;
                    cell.Element("Medio_trans").Value = rdbAnteMedio.SelectedValue;

                    cell.Element("Contacto_con_personas_con_IRA_14_dias_previos").Value = rdbAnteContactoIRA.SelectedValue;
                    cell.Element("Entorno_familiar_IRA").Value = rdbAnteEntornoFliar.SelectedValue;
                    cell.Element("Entorno_asistencial_IRA").Value = rdbAnteEntornoAsistencial.SelectedValue;
                    cell.Element("Otro_entorno_IRA").Value = txtanteOtroentorno.Text;

                     cell.Element("Contacto_con_Confirmados_Covid").Value = rdbAnteContactoCovid.SelectedValue ;
                    cell.Element("Entorno_Covid_familiar").Value = rdbAnteContactoCovidEntornoFliar.SelectedValue;
                    cell.Element("Entorno_Covid_asistencial").Value=rdbAnteContactoCovidEntornoAsistencial.SelectedValue ;
                    cell.Element("Otro_entorno_Covid").Value=txtAnteContactoCovidOtroEntorno.Text ;
                    cell.Element("Fecha_Inicio_sintomas").Value=txtFechaInicioSintomas.Value ;
                    cell.Element("Fecha_Primera_consulta").Value=txtFechaPrimerConsulta.Value ;
                    cell.Element("Internacion").Value=rdbInternacion.SelectedValue ;

                    cell.Element("Fecha_Internacion").Value=txtFechaInternacion.Value ;
                    cell.Element("UTI").Value=rdbUTI.SelectedValue ;
                    cell.Element("Fecha_Int_UTI").Value=txtFechainternacionUti.Value ;
                    cell.Element("ARM").Value=rdbARM.SelectedValue ;
                    cell.Element("Dx_ingreso").Value=rdbDiagnosticoIngreso.SelectedValue ;
                    cell.Element("Dx_Otros").Value = txtOtrosDiagnostico.Text;


                    cell.Element("Comorbilidades").Value = rdbComorbilidades.SelectedValue;
                    cell.Element("Com_Enf_Resp").Value = rdbComorbilidadEnfResp.SelectedValue;
                    cell.Element("Com_Enf_Neuro").Value = rdbComorbilidadEnfNeuro.SelectedValue;
                    cell.Element("Com_Inmunosupresion").Value = rdbComorbilidadInmunosupresion.SelectedValue;
                    cell.Element("Com_Enf_cardio").Value = rdbComorbilidadEnfCardio.SelectedValue;
                    cell.Element("Com_HTA").Value = rdbComorbilidadHTA.SelectedValue;
                    cell.Element("Com_Renal").Value = rdbComorbilidadRenal.SelectedValue;
                    cell.Element("Com_Hepatica").Value = rdbComorbilidadHepatica.SelectedValue;
                    cell.Element("Com_DBT").Value = rdbComorbilidadDBT.SelectedValue;
                    cell.Element("Com_Desnutricion").Value = rdbComorbilidadDesnutricion.SelectedValue;
                    cell.Element("Com_IMC_Mayor_30").Value = rdbIMC_menor_30.SelectedValue;
                    cell.Element("Com_Perinatales").Value = rdbComorbilidadPerinatales.SelectedValue;
                    cell.Element("Com_Tto_Atb_Previo").Value = rdbComorbilidadTtoAtbPrevio.SelectedValue;
                    cell.Element("Com_Fecha_Tto_Atb_Previo").Value = txtFechaInicioComorbilidadAtbPrevio.Value;
                    cell.Element("Com_Tto_antiviral_previo").Value = rdbComorbilidadTtoantiviralPrevio.SelectedValue;
                    cell.Element("Com_Fecha_Tto_antiviral_previo").Value = txtFechaInicioComorbilidadAntiviralPrevio.Value;
                    cell.Element("Com_Antigripal_2020").Value = rdbComorbilidadAntigripal.SelectedValue;


                    cell.Element("SyS_Sintomatico").Value = rdbSignosSintomatico.SelectedValue;
                    cell.Element("Sys_Fiebre_38_o_mas").Value = rdbSignosFiebre.SelectedValue;
                    cell.Element("Sys_Rinorrea").Value = rdbSignosRisnorrea.SelectedValue;
                    cell.Element("Sys_tos").Value = rdbSignosTos.SelectedValue;
                    cell.Element("Sys_Disnea").Value = rdbSignosDisnea.SelectedValue;
                    cell.Element("Sys_Taquipnea").Value = rdbSignosTaquipnea.SelectedValue;
                    cell.Element("Sys_Dif_Resp").Value = rdbSignosDifResp.SelectedValue;
                    cell.Element("Sys_Anosmia").Value = rdbSignosAnosmia.SelectedValue;
                    cell.Element("Sys_Disgeusia").Value = rdbSignosDisgeusia.SelectedValue;
                    cell.Element("Sys_Malestar_General").Value = rdbSignosMalestar.SelectedValue;
                    cell.Element("Sys_Artralgias").Value = rdbSignosAltralgia.SelectedValue;
                    cell.Element("Sys_Milagias").Value = rdbSignosMialgia.SelectedValue;
                    cell.Element("Sys_Odinofagia").Value = rdbSignosOdinofagia.SelectedValue;
                    cell.Element("Sys_Otalgia").Value = rdbSignosOtalgia.SelectedValue;
                    cell.Element("Sys_Cefalea").Value = rdbSignosCefalea.SelectedValue;

                    cell.Element("Sys_Vomitos").Value=rdbSignosVomito.SelectedValue  ;
                    cell.Element("Sys_Diarrea").Value=rdbSignosDiarrea.SelectedValue  ;
                    cell.Element("Sys_Dolor_abdominal").Value=rdbSignosDolorAbdominal.SelectedValue ;
                    cell.Element("Sys_Otros_Sintomas").Value=txtOtrosSintomas.Text  ;
                    cell.Element("Sys_RX").Value = rdbSignosRx.SelectedValue;
                    cell.Element("Sys_RX_Hallazgo").Value = txtSignosRxHallazgo.Text;


                    cell.Element("Lab_Leucocitos").Value = txtLeucocitos.Text;
                    cell.Element("Lab_VSG").Value = txtVSG.Text;
                    cell.Element("Lab_LDH").Value=txtLDH.Text ; ;
                    cell.Element("Lab_TGO").Value=txtTGO.Text ; ;
                    cell.Element("Lab_FAL").Value=txtFAL.Text  ; ;
                    cell.Element("Lab_CPK").Value=txtCPK.Text ; ;
                    cell.Element("Lab_TP").Value=txtTP.Text  ; ;
                    cell.Element("Lab_Urea").Value=txtUrea.Text  ; ;
                    cell.Element("Lab_Creatinina").Value=txtCreatinina.Text ; ;
                    cell.Element("Lab_PCR").Value=txtPCR.Text  ;
                    cell.Element("Diag_PCR_Fecha").Value=txtFechaPCR.Value ; ;
                    cell.Element("Diag_PCR_Tipo_muestra").Value=txtTipoMuestra.Text ; ;
                    cell.Element("Diag_PCR_Resultado").Value=txtResultado.Text ; ;


                    cell.Element("Tratamiento_B2").Value = rdbB2.SelectedValue;
                    cell.Element("Tratamiento_O2").Value=rdbO2.SelectedValue ; ;
                    cell.Element("Tratamiento_Corticoides").Value=rdbCorticoides.SelectedValue; ;
                    cell.Element("Tratamiento_ATB").Value=rdbATB.SelectedValue; ;
                    cell.Element("Tratamiento_Antiviral").Value=rdbAntiviral.SelectedValue ; ;
                    cell.Element("Tratamiento_Otros").Value=txtOtrosTratamientos.Text  ; ;

                    cell.Element("Evolucion_Complicaciones").Value=rdbComplicaciones.SelectedValue ; ;
                    cell.Element("Evolucion_CualComplicacion").Value=txtComplicaciones.Text ; ;
                    cell.Element("Evolucion_Egreso").Value=rdbEstadoEgreso.SelectedValue ; ;
                    cell.Element("Evolucion_Fecha_Egreso").Value=txtFechaEgreso.Value ; ;


                    doc.Save(a);

                }

            }
      
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("EpidemioProtocoloList.aspx", false);

        }
    }
    }
