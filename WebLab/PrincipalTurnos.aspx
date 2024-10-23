<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrincipalTurnos.aspx.cs" Inherits="WebLab.PrincipalTurnos" MasterPageFile="~/SiteTurnos.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajx" %>
<%@ Register Src="~/PeticionList.ascx" TagPrefix="uc1" TagName="PeticionList" %>
<%@ Register Src="~/seguimientoCovid.ascx" TagPrefix="uc1" TagName="seguimientoCovid" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server"> 


 <link rel="stylesheet" href="bootstrap/3.3.7/bootstrap.min.css" />
<script src="bootstrap/3.3.7/jquery.min.js"></script>
<script src="bootstrap/3.3.7/bootstrap.min.js"></script>
    <link rel="shortcut icon" href="website/website/images/icolabo.ico">
 <%-- <link rel="stylesheet" href="website/style.css">--%>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
    <link rel="stylesheet" type="text/css" href="App_Themes/default/principal/style.css" />
 
      </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    
 
  <ajx:toolkitscriptmanager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
    EnableScriptLocalization="true">
  </ajx:toolkitscriptmanager>
 
 <div align="left"  >
   
      
         
      
               

      <%-- <div class="row" >
        
                <div class="col-sm-3" ID="pnlHojaTrabajo" runat="server">
                <div>
                    <div class="thumbnail">
                    

                        <div class="caption">
                            <h3> <a href="Informes/Informe.aspx?Tipo=HojaTrabajo">Hojas de Trabajo</a></h3>
                            <p>     Genere desde acá las hojas de trabajo para las áreas de su laboratorio.     </p>
                            

                                      <a href="Informes/Informe.aspx?Tipo=HojaTrabajo"> 

                                            <button type="button" class="btn btn-success"> HT </button>
                                        </a>
                                    
                        </div>

                    </div>
                </div>
            </div>
              
                       
   <div class="col-sm-3" ID="pnlCargaResultado" runat="server">
                <div>
                    <div class="thumbnail">
                    

                        <div class="caption">
                            <h3>  Carga de resultados de Microbiología </h3>
                            <p>     </p>
                            
                            <a href="Resultados/ResultadoBusqueda.aspx?idServicio=3&Operacion=Carga&modo=Normal">
                                   <button type="button" class="btn btn-success"> HT </button> </a>
                                    
                        </div>

                    </div>
                </div>
            </div>
              
           <div class="col-sm-3" ID="pnlValidacion" runat="server">
                <div>
                    <div class="thumbnail">
                    

                        <div class="caption">
                            <h3> Validación de Resultados</h3>
                            <p>     </p>
                            
                           <a href="Resultados/ResultadoBusqueda.aspx?idServicio=3&Operacion=Valida&modo=Normal">
                                   <button type="button" class="btn btn-success">   </button> </a>
                                    
                        </div>

                    </div>
                </div>
            </div>
                     
       </div>
                --%>            
        
                

            
   
    
			 
              
				
		<%--	<asp:Panel ID="pnlImpresion" runat="server"> 
				<div class="services_block">
					<img src="App_Themes/default/principal/images/Printersettings.png" alt="" width="32" height="32" border="0" class="icon_left" title="" />
                    <div class="services_details">
                     <h3 class="Estilo1"><a href="ImpresionResult/ImprimirBusqueda.aspx?idServicio=1&modo=Normal" target="_parent">Impresión de Resultados</a></h3>
                     <p>
                   
                                       </p>     
                                          <a href="ImpresionResult/ImprimirBusqueda.aspx?idServicio=3&modo=Normal" title="Acceso directo a Microbiologia">Microbiología.</a>                 
                    </div>												
				
              </div>	      <br />
      	 </asp:Panel>--%>
      		<%--<asp:Panel ID="pnlUrgencia" runat="server">
                 <div class="services_block">
                     <img border="0" class="icon_left" height="32px" 
                         src="App_Themes/default/principal/images/urgencia.png" title="" width="32px" /><div 
                         class="services_details">
                         <h3>
                             <a 
                                 href="Protocolos/Default.aspx?idServicio=1&idUrgencia=1">Modulo de Urgencias</a>
                                
                                 </h3>
                         <div align="left">
                             Desde acá podrá cargar el protocolo, sus resultados, validar e imprimir de forma 
                             rápida en un solo paso.</div>
                     </div>
                 </div>
                 <br />
     </asp:Panel>--%>
          
   
     <div class="card-deck">

				<%--      <div class="card bg-light mb-12" style="max-width: 38rem;" ID="pnlResultados" runat="server">
  <div class="card-header">
       <img border="0" class="icon_left" height="32px" 
                         src="App_Themes/default/principal/images/resultados.png" title="" width="32px" />
      
      <a 
                                 href="Informes/historiaClinicafiltro.aspx?Tipo=PacienteValidado">Consulta de Resultados</a>

  </div>
  <div class="card-body text-info">
      
    <h5 class="card-title">     
        
    </h5>
    <p class="card-text">   Historial de Resultados: Consulte para un paciente los resultados obtenidos en el laboratorio.<hr /> <b>
                     
                          <h3>   Para consultar resultados en la RED PROVINCIAL DE LABORATORIOS hacer clic <a href="http://www.saludnqn.gob.ar/Sips/" target="_blank">aquí</a></h3></b>
                            
                             &nbsp; &nbsp; &nbsp; <em>Si tiene algun inconveniente para acceder a este link consultar con su soporte informático local.</em> </p>

  </div>
</div>--%>

      
                  
                 
                 <br />
           

     <div class="card bg-light mb-3" style="max-width: 18rem;" ID="pnlNuevoUsuario" runat="server">
  <div class="card-header">
       <img border="0" class="icon_left" height="32px" 
                         src="App_Themes/default/principal/images/userLogin.jpg" title="" width="32px" />
      <a 
                                 href="login.aspx">Nuevo Usuario</a>

    
      
  </div>
  <div class="card-body text-info">
      
    <h5 class="card-title">     
        
    </h5>
    <p class="card-text">  
           Cierra sesión actual y permite abrir nueva sesión de usuario.
    </p>

  </div>
</div>
               
                  
                 
     <%--<asp:Panel ID="pnlSivila" runat="server">
      <div class="services_block">
            <img border="0" class="icon_left" 
                         src="App_Themes/default//images/snvs.PNG" title=""  />
                 <div 
                         class="services_details">
                         <h3>
                             <a 
                                 href="Estadisticas/ReportePorResultadoSivila.aspx">Exportación de datos para SIVILA</a></h3>
                         <div align="left">
                           Generación de archivos automaticos para el envío de datos a SIVILA.
                             </div>
                     </div>
                 </div> 
         </asp:Panel>--%>
   
     <uc1:PeticionList runat="server" id="PeticionList1" />
       
         <br /> 



                                             <br />
                                            
 
  
<br />&nbsp;
</div>
    
 
 <%--<div class="myLabelIzquierda"> 
 <table>
 <tr>
 <td rowspan="2"> &nbsp;&nbsp;<a href="install_flashplayer11x32_mssa_aih.exe"><img title="Haga clic aquí para descargar instalador" src="App_Themes/default/images/adobeFp.jpg" /></a></td>
 <td>Para acceder a gráficos generados por este sistema requiere instalar Adobe Flash Player.</td>
 </tr>
 <tr>
 <td colspan="2"><a href="install_flashplayer11x32_mssa_aih.exe">Descargar</a>

     </td>
 </tr>
 </table>
 
<br />&nbsp;
</div>--%>
    
                                                                                       
       </div>
</asp:Content>


