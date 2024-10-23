<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Principal.aspx.cs" Inherits="WebLab.Principal" MasterPageFile="~/Site1.Master" %>

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
   
      </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    
 
  <ajx:toolkitscriptmanager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
    EnableScriptLocalization="true">
  </ajx:toolkitscriptmanager>
 
 <div align="left"  >
   
      
                 <div class="row" >
         <div class="col-sm-8" ID="divAlerta" runat="server">
                <div>
                    <div class="thumbnail">                        
                        <div class="caption">
                            <h3  class="btn btn-sq btn-danger" style="width:150px" > ALERTA</h3>
                            <p>   
                            <strong><asp:Label ID="lblAlerta" runat="server" Text=""></asp:Label> </strong></p>
                            
                                 

                                    
                        </div>

                    </div>
                </div>
            </div>
 
     </div>
          
        <div class="row" >
              
         
                <div class="col-sm-3" ID="pnlTurno" runat="server">
                <div>
                    <div class="thumbnail">
                        <img src="App_Themes/default/principal/images/Calendar.png"  height="32px" title="" border="0" class="icon_left" width="32px" />
                        <div class="caption">
                            <h3> Turnos</h3>
                            <p>    Genere desde esta opción los turnos programados.       </p>
                            

                                      <a href="Turnos/TurnoList.aspx?tipo=generacion"  target="_parent" > 

                                            <button type="button" class="btn btn-success" style="width:100px;"> Turno </button>
                                        </a>
                                    
                        </div>

                    </div>
                </div>
            </div>
                <div class="col-sm-3"  ID="pnlRecepcion" runat="server">
                <div>
                    <div class="thumbnail">

                        <div class="caption">
                            <h3>Recepción de Muestras</h3>
                            
                            <p> Generación de Protocolos </p>
                            <asp:Label ID="lblProximoProtocolo" runat="server" 
                               Text=" Próximo Número de Protocolo Disponible:" Font-Bold="True" 
                           Font-Italic="True"></asp:Label>
        
           <asp:Label ID="lblProximoProtocolo1" runat="server" Font-Bold="True"   Font-Italic="True"
                               Font-Size="11pt" ForeColor="#CC3300" Text="Label"></asp:Label>
        <br /> 
                       <asp:LinkButton ID="lnkUltimoNumeroSector" runat="server" 
                           Font-Bold="True"    Font-Italic="True"
                               Font-Size="11pt" ForeColor="#CC3300" 
                           onclick="lnkUltimoNumeroSector_Click" Visible="False">Ver</asp:LinkButton>
        <br />
                      <asp:Panel ID="pnlProtocolo" runat="server" Width="100%">
                      
                           
                           <a href="Protocolos/Default2.aspx?idServicio=3&idUrgencia=0" target="_parent" >Microbiología. </a>                        
                        </asp:Panel> 
          
                           <br />                   
                       <asp:Panel ID="pnlTurnoRecepcion" runat="server">
                           <a href="Turnos/TurnoList.aspx?tipo=recepcion" target="_parent">Pacientes con 
                           turnos.</a> Cargue desde acá los protocolos para los pacientes con turnos.<br />
                       </asp:Panel>

                         
             
                        </div>
                          
                    </div>
                </div>
            </div>
                <div class="col-sm-4" id="pnlSeguimiento" runat="server">
                   <uc1:seguimientoCovid runat="server" id="seguimientoCovid" />
                    
            </div>
             
               <div class="col-sm-4" id="Div1" runat="server">
                             <asp:GridView ID="gvProtocolosxEfector" runat="server" CssClass="table table-bordered bs-table"></asp:GridView>
                          
       </div> 
           
            
               <div class="col-sm-4" id="Div2" runat="server">
                  
            <asp:LinkButton Visible="true" Width="200px" Height="40px"  PostBackUrl="~/Capacita/Capacita.aspx" ID="LinkButton1" class="btn btn-sq btn-danger" runat="server">Capacitacion</asp:LinkButton>
                   <asp:LinkButton Visible="true" Width="200px" Height="40px"  PostBackUrl="~/LoginEfector.aspx" ID="lnkCambioEfector" class="btn btn-sq btn-primary" runat="server">Cambiar Efector</asp:LinkButton>
                   <br />
                   <br />
                  
                    <asp:GridView ID="gvProtocolosxSISA" runat="server" CssClass="table table-bordered bs-table" Width="250px"></asp:GridView>  
             </div>
         
              <div class="col-sm-4" id="mensajeria" runat="server"  style="width:305px;height:400px;overflow:scroll;overflow-x:hidden;" 
             align="right" > 
             <asp:ImageButton ID="imgAgregarMensaje" runat="server"  
                 ImageUrl="~/App_Themes/default/images/svn_added.png" 
                 onclick="imgAgregarMensaje_Click" ToolTip="Agregar Mensaje" />
             <br />
                                            <asp:DataList ID="DataList1" runat="server"
  onitemdatabound="DataList1_ItemDataBound"                                                 
width="290px" CellPadding="4" ForeColor="#333333" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
>
                                               
                                            
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                              
                                                <ItemStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                <SelectedItemStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                               
                                            
<HeaderTemplate>
    Mensajes Internos
   </HeaderTemplate>



                                                <HeaderStyle BackColor="Aquamarine" Font-Bold="True" ForeColor="#333333" />
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>                                                           
                                                            <td  class="myLabelIzquierda">                                                                
                                                              <b><%# DataBinder.Eval(Container.DataItem, "fechaHoraRegistro")%></b>    &nbsp;  &nbsp;  &nbsp;  &nbsp;
                                                                  <asp:HyperLink ID="hplMensajeEdit" NavigateUrl=<%# DataBinder.Eval(Container.DataItem, "idMensaje")%>  runat="server"><b>Eliminar</b></asp:HyperLink>                                                        
                                                            </td>
                                                        </tr>
                                                        <tr>                                                        
                                                            <td >
                                                           <b>   De:</b><%# DataBinder.Eval(Container.DataItem, "remitente")%></td>                                                             
                                                            
                                                        </tr>                                                       
                                                        <tr>
                                                        
                                                            <td >
                                                                
                                                           
                                                              <b>  Para:</b><b style="color: #FF0000; font-weight: bold"> <%# DataBinder.Eval(Container.DataItem, "destinatario")%> </b>
                                                            </td>
                                                            
                                                        </tr>
                                                               <tr>
                                                        
                                                            <td >
                                                                
                                                           
                                                                <b> Mensaje:</b><p> <%# DataBinder.Eval(Container.DataItem, "mensaje")%> </p>
                                                            </td>
                                                            
                                                        </tr>                                                                                                    
                                                    </table>
                                                </ItemTemplate>

                                            </asp:DataList></div>

            <%-- <div class="col-sm-4">
              <a href="Capacita/Capacita.aspx"  class="btn btn-sq btn-danger">
                <i class="fa fa-search fa-5x"></i><br/>
               CAPACITACION
            </a>
            </div>--%>
            </div>
               

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


