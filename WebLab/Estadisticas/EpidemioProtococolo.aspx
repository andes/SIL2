<%@ Page Title="" Language="C#" MasterPageFile="~/Epi1.Master" AutoEventWireup="true" CodeBehind="EpidemioProtococolo.aspx.cs" Inherits="WebLab.Estadisticas.EpidemioProtococolo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  



  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript"> 
     

	$(function() {
		$("#<%=txtFechaViajeRetorno.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
		$("#<%=txtFechaInicioSintomas.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});
$(function() {
		$("#<%=txtFechaPCR.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});


          $(function() {
		$("#<%=txtFechaEgreso.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

          
          
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
    
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    <div align="left" style="width: 1500px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h2 >Ficha Clinica</h2>
                       
                         <asp:Button ID="Button1" CssClass="btn btn-primary" Width="100px" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                        <asp:Button ID="Button2" CssClass="btn btn-primary" Width="100px" runat="server" Text="Regresar" OnClick="btnRegresar_Click"  />
                     
                        </div>

				<div class="panel-body">

                     <div class="form-group" >
                         <h4>Protocolo:</h4>
                         <asp:TextBox ID="txtProtocolo" runat="server"></asp:TextBox>
                        

                         </div>

                <div class="panel panel-default">
                    <div class="panel-heading">
     <h3>Antecedentes Epidemiológicos</h3>
                        </div>

				<div class="panel-body">
                       
                       
                    <div class="form-group" >
                         <h4>Viaje a Zona de riesgo los ultimos 14 días:
                             </h4>
                        </div>
                         <div class="form-group" > <h4>
                             <asp:RadioButtonList ID="rdbAnteViajeZona" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                             </asp:RadioButtonList></h4>
                             </div>

                      <div class="form-group" >
                         <h4>Lugar: <asp:TextBox ID="txtAnteLugar" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                          </div>
                    <br />
                          <div class="form-group" >
                         <h4>Fecha del viaje (retorno):  <input id="txtFechaViajeRetorno" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  />
                             </h4>
                             </div>
                               <div class="form-group" >
                    <h4>     <asp:RadioButtonList ID="rdbAnteMedio" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Avión</asp:ListItem>
                             <asp:ListItem>Buque</asp:ListItem>
                             <asp:ListItem>Omnibus</asp:ListItem>
                         </asp:RadioButtonList></h4>

                         </div>
                          
                         <hr />
                         <div class="form-group" >
                         <h4>Contacto con personas con IRA en los ultimos 14 días
                             </h4>
                             </div>
                              <div class="form-group" >
                         <asp:RadioButtonList ID="rdbAnteContactoIRA" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                                  </div>
                    <br />
                    <div class="form-group" >
                             <h4>Entorno Familiar:</h4>
                        </div>
                         <div class="form-group" >
                            <h4>   <asp:RadioButtonList ID="rdbAnteEntornoFliar" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList></h4>
                             </div>
                         <div class="form-group" >
                             <h4>Entorno Asistencial:</h4>
                        </div>
                         <div class="form-group" >
                            <h4>     <asp:RadioButtonList ID="rdbAnteEntornoAsistencial" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList> </h4>
                             </div>
                    <br />
                      <div class="form-group" >
                         <h4>Otros: <asp:TextBox ID="txtanteOtroentorno" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                          </div>
                        
                       

                        

                     
                         <hr />
                            <div class="form-group" >
                         <h4>Contacto con caso probable o confirmado de Covid-19 en los ultimos 14 días
                             </h4>
                                </div>
                                  <div class="form-group" >
                        <h4>  <asp:RadioButtonList ID="rdbAnteContactoCovid" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList></h4>
                                     </div>
                    <br />
                                  <div class="form-group" >
                             <h4>Entorno Familiar:
                                 </h4>
                                </div>
                                  <div class="form-group" >
                          <h4>     <asp:RadioButtonList ID="rdbAnteContactoCovidEntornoFliar" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList></h4>
                                        </div>
                    <div class="form-group" >
                             <h4>Entorno Asistencial:
                                  </h4>
                                </div>
                                  <div class="form-group" >
                             <h4> <asp:RadioButtonList ID="rdbAnteContactoCovidEntornoAsistencial" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList></h4>
                                      </div>
                    <br />
                       <div class="form-group" >   <h4>Otros: </h4>
                           </div>
                     <div class="form-group" >
                           <asp:TextBox ID="txtAnteContactoCovidOtroEntorno" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                         </div> 
                        
                       

                         </div>
                     </div> 

               


         <div class="panel panel-default">
                    <div class="panel-heading">
       <h3>Información Clínica</h3>
                        </div>

				<div class="panel-body">
                         
                       
                        <div class="form-group" >
                            <h4>Fecha de inicio de síntomas:
                             <input id="txtFechaInicioSintomas" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  /></h4>
                        </div>
                     <div class="form-group" >

                            <h4>Fecha de Primera consulta:
                       <input id="txtFechaPrimerConsulta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  /></h4>
                         </div>
                    <br />
                      <div class="form-group" >
                                <h4>Internación</h4>
                          </div>
                      <div class="form-group" >
                        <h4>  <asp:RadioButtonList ID="rdbInternacion" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList></h4>
                          </div>
                       <div class="form-group" >
                               <h4>Fecha de internación:
                             <input id="txtFechaInternacion" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  /></h4>
                        </div>
                    <br />
                             <div class="form-group" >
                                 <h4>UTI</h4>
                                 </div>
                       <div class="form-group" >
                       <h4>  <asp:RadioButtonList ID="rdbUTI" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList></h4>
                           </div>
                     <div class="form-group" >
                               <h4>Fecha de internación de UTI:
                      <input id="txtFechainternacionUti" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  />
</h4>
                         </div>
                    <div class="form-group" >
                                <h4>Requerimiento de ARM</h4>
                         </div>
                       <div class="form-group" >
                         <asp:RadioButtonList ID="rdbARM" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </div>
                    <br />
                      <div class="form-group" >
                                  <h4>Diagnósticos de Ingreso</h4>
                          </div>
                       <div class="form-group" >
                         <asp:RadioButtonList ID="rdbDiagnosticoIngreso" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>ETI</asp:ListItem>
                             <asp:ListItem>Bronquiolitis</asp:ListItem>
                             <asp:ListItem>Neumonía focal</asp:ListItem>
                             <asp:ListItem>Neumonía multifocal</asp:ListItem>
                             <asp:ListItem>Neumonía con derrame</asp:ListItem>
                         </asp:RadioButtonList>
                           </div>
                    <br /> 
                    <div class="form-group" >
                                 Otros:
                              </div>
                       <div class="form-group" >   
                                 <asp:TextBox ID="txtOtrosDiagnostico" runat="server"></asp:TextBox></h4>
                                 </div>
                     
                     

                  


                    </div>
             </div>

                      <div class="panel panel-default">
                    <div class="panel-heading">
       <h2>Comorbilidades</h2>
                        </div>

				<div class="panel-body">

                      <div class="form-group" >
                            <h4>Comorbilidades:
                         <asp:RadioButtonList ID="rdbComorbilidades" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>

                    <div class="form-group" >
                            <h4>&nbsp;Enf Resp:
                         <asp:RadioButtonList ID="rdbComorbilidadEnfResp" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>

                    <div class="form-group" >
                            <h4>Enf Neuro:
                         <asp:RadioButtonList ID="rdbComorbilidadEnfNeuro" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Inmunosupresion:
                         <asp:RadioButtonList ID="rdbComorbilidadInmunosupresion" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Enf cardio:
                         <asp:RadioButtonList ID="rdbComorbilidadEnfCardio" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>HTA:
                         <asp:RadioButtonList ID="rdbComorbilidadHTA" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Renal:
                         <asp:RadioButtonList ID="rdbComorbilidadRenal" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Hepatica:
                         <asp:RadioButtonList ID="rdbComorbilidadHepatica" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>DBT:
                         <asp:RadioButtonList ID="rdbComorbilidadDBT" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Desnutricion:
                         <asp:RadioButtonList ID="rdbComorbilidadDesnutricion" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>&nbsp;IMC&gt;30:
                         <asp:RadioButtonList ID="rdbIMC_menor_30" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                       <div class="form-group" >
                            <h4>&nbsp;Perinatales:
                         <asp:RadioButtonList ID="rdbComorbilidadPerinatales" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>   <div class="form-group" >
                            <h4>Tratamiento Antibiótico Previo:
                         <asp:RadioButtonList ID="rdbComorbilidadTtoAtbPrevio" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                     
                            <h4>Fecha Inicio Tto Atb Previo:
                        <input id="txtFechaInicioComorbilidadAtbPrevio" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  />
                           </h4>
                    </div>   <div class="form-group" >
                            <h4>tratamiento antiviral previo:
                         <asp:RadioButtonList ID="rdbComorbilidadTtoantiviralPrevio" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    
                         
                            <h4>Fecha Inicio tratamiento antiviral previo:
                                <input id="txtFechaInicioComorbilidadAntiviralPrevio" runat="server" class="form-control input-sm" maxlength="10" onblur="valFecha(this)" onkeyup="mascara(this,'/',patron,true)" style="width: 100px" tabindex="4" title="Ingrese la fecha de fin" type="text" /></h4>
                    </div>

                     <div class="form-group" >
                            <h4>Vacunación antigripal 2020:
                         <asp:RadioButtonList ID="rdbComorbilidadAntigripal" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div> 
                          </div>
       </div>



                    <div class="panel panel-default">
                    <div class="panel-heading">
       <h2>Signos y Sintomas</h2>
                        </div>

				<div class="panel-body">

                      <div class="form-group" >
                            <h4>Sintomatico:
                         <asp:RadioButtonList ID="rdbSignosSintomatico" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>

                    <div class="form-group" >
                            <h4>&nbsp;Fiebre 38 o >:
                         <asp:RadioButtonList ID="rdbSignosFiebre" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>

                    <div class="form-group" >
                            <h4>Rinorrea:
                         <asp:RadioButtonList ID="rdbSignosRisnorrea" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>tos:
                         <asp:RadioButtonList ID="rdbSignosTos" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Disnea:
                         <asp:RadioButtonList ID="rdbSignosDisnea" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Taquipnea:
                         <asp:RadioButtonList ID="rdbSignosTaquipnea" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Dif Resp:
                         <asp:RadioButtonList ID="rdbSignosDifResp" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Anosmia:
                         <asp:RadioButtonList ID="rdbSignosAnosmia" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Disgeusia:
                         <asp:RadioButtonList ID="rdbSignosDisgeusia" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Malestar General:
                         <asp:RadioButtonList ID="rdbSignosMalestar" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <div class="form-group" >
                            <h4>Artralgias:
                         <asp:RadioButtonList ID="rdbSignosAltralgia" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>
                    <br />
                       <div class="form-group" >
                            <h4>Milagias:
                         <asp:RadioButtonList ID="rdbSignosMialgia" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    </div>   <div class="form-group" >
                            <h4>Odinofagia:
                         <asp:RadioButtonList ID="rdbSignosOdinofagia" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                      
                    </div>   <div class="form-group" >
                            <h4>Otalgia:
                         <asp:RadioButtonList ID="rdbSignosOtalgia" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                    
                         

                    </div>

                     <div class="form-group" >
                             Cefalea:
                         
                         <asp:RadioButtonList ID="rdbSignosCefalea" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                            
                    </div> 

                      <div class="form-group" >
                            Vómitos:  
                         <asp:RadioButtonList ID="rdbSignosVomito" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           
                    </div> 
                      <div class="form-group" >
                           Diarrea:
                         
                         <asp:RadioButtonList ID="rdbSignosDiarrea" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                          
                    </div> 
                      <div class="form-group"   >
                           Dolor abdominal:
                        
                         <asp:RadioButtonList ID="rdbSignosDolorAbdominal" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                       </div>    
                    <br />
                      <div class="form-group" >
                          <h4>Otros: <asp:TextBox ID="txtOtrosSintomas" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                    </div> 
                    <br />
                     <div class="form-group"   >
                          RX:
                          </div> 
                      <div class="form-group" >
                         <asp:RadioButtonList ID="rdbSignosRx" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                       </div>    
                      <div class="form-group" >
                          <h4>Hallazgo: <asp:TextBox ID="txtSignosRxHallazgo" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                    </div> 
                    <br />

                      <div class="form-group" >
                          <h4>Laboratorio</h4>
                          <h4>Leucocitos: <asp:TextBox ID="txtLeucocitos" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>VSG: <asp:TextBox ID="txtVSG" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>LDH: <asp:TextBox ID="txtLDH" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>TGO: <asp:TextBox ID="txtTGO" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>FAL: <asp:TextBox ID="txtFAL" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>CPK: <asp:TextBox ID="txtCPK" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>TP: <asp:TextBox ID="txtTP" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>Urea: <asp:TextBox ID="txtUrea" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>Creatinina: <asp:TextBox ID="txtCreatinina" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                          <h4>PCR Cuantitativa: <asp:TextBox ID="txtPCR" runat="server" class="form-control input-sm" Width="100px"></asp:TextBox></h4>
                         
                          
                    </div> 

                          </div>
       </div>


                       <div class="panel panel-default">
                    <div class="panel-heading">
       <h2>Diagnostico</h2>
                        </div>

				<div class="panel-body">

                       <div class="form-group" >
                            <h4>Fecha:
                       
                                <input id="txtFechaPCR" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  />
                           </h4>
                    </div> 
                     <div class="form-group" >
                            <h4>Tipo de Muestra:
                       <asp:TextBox ID="txtTipoMuestra" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox>

                           </h4>
                    </div> 
                    <div class="form-group" >
                            <h4>Resultado:
                       <asp:TextBox ID="txtResultado" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox>

                           </h4>
                    </div> 
                    </div>
                           </div>

                       <div class="panel panel-default">
                    <div class="panel-heading">
       <h2>Tratamiento</h2>
                        </div>

				<div class="panel-body">

                       <div class="form-group" >
                            
                          <h4>B2: 
                         <asp:RadioButtonList ID="rdbB2" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>O2: 
                         <asp:RadioButtonList ID="rdbO2" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>Corticoides: 
                         <asp:RadioButtonList ID="rdbCorticoides" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>ATB:  
                         <asp:RadioButtonList ID="rdbATB" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>Antiviral:   
                         <asp:RadioButtonList ID="rdbAntiviral" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>Otros: <asp:TextBox ID="txtOtrosTratamientos" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                          

                          
                    </div> 
                  
                    </div>
                           </div>

                    <div class="panel panel-default">
                    <div class="panel-heading">
       <h2>Evolución Clínica</h2>
                        </div>

				<div class="panel-body">

                       <div class="form-group" >
                            
                          <h4>Complicaciones:    
                         <asp:RadioButtonList ID="rdbComplicaciones" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Si</asp:ListItem>
                             <asp:ListItem>No</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>Cuales Complicaciones?: <asp:TextBox ID="txtComplicaciones" runat="server" class="form-control input-sm" Width="400px"></asp:TextBox></h4>
                          <h4>Estado de Egreso:     
                         <asp:RadioButtonList ID="rdbEstadoEgreso" runat="server" RepeatDirection="Horizontal">
                             <asp:ListItem>Alta Recuperado</asp:ListItem>
                             <asp:ListItem>Alta No Recuperado</asp:ListItem>
                             <asp:ListItem>Muerte</asp:ListItem>
                             <asp:ListItem>Derivado/desconocido</asp:ListItem>
                         </asp:RadioButtonList>
                           </h4>
                          <h4>Fecha de Egreso: 
                              <input id="txtFechaEgreso" runat="server" class="form-control input-sm" maxlength="10" onblur="valFecha(this)" onkeyup="mascara(this,'/',patron,true)" style="width: 100px" tabindex="4" title="Ingrese la fecha de fin" type="text" /></h4>
                         
                          
                    </div> 
                  
                    </div>
                           </div>
                 <asp:Button ID="btnGuardar" CssClass="btn btn-primary" Width="100px" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                        <asp:Button ID="btnRegresar" CssClass="btn btn-primary" Width="100px" runat="server" Text="Regresar" OnClick="btnRegresar_Click"  />
                        
        </div>
    </div>
    </div>
</asp:Content>
