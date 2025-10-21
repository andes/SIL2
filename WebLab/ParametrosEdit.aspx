<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParametrosEdit.aspx.cs" Inherits="WebLab.ParametrosEdit" MasterPageFile="~/Site1.Master" %>
   <%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>



<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
<link href="script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
    <%--    <li><a href="#tab7" >Impresoras</a></li>--%>
<script src="script/jquery.min.js" type="text/javascript"></script>  
<script src="script/jquery-ui.min.js" type="text/javascript"></script> 
<script type="text/javascript" src="script/ValidaFecha.js"></script>

<script type="text/javascript">  

$(function() {

$("#tabContainer").tabs();
var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();

$("#tabContainer").tabs({ selected: currTab });
});


</script> 
    
    
    
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
       
    <asp:HiddenField runat="server" ID="HFCurrTabIndex"   /> 
    <div align="center" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">CONFIGURACION DEL SISTEMA
                        </h3>
                        </div>

       	<div class="panel-body">	
             <table>
        <tr>
            <td colspan="2">
               
           <strong>Efector:</strong>     <asp:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" AutoPostBack="True" OnSelectedIndexChanged="ddlEfector_SelectedIndexChanged">
                            </asp:DropDownList>


            </td>
            
        </tr>
        <tr>
            <td colspan="2">
                <hr /></td>
            
        </tr>
         <tr>
            <td colspan="2">
            
             <div id="tabContainer" style="border: 0px solid #C0C0C0">  
             <ul class="myLabel">
                <li><a href="#tab0">General</a></li>   
                <li><a href="#tab1">Protocolos</a></li>    
                <li><a href="#tab3">Turnos</a></li>
                <li><a href="#tab2">Calendario</a></li>        
                <li><a href="#tab4">Carga/Valid.Resultados</a></li>
                <li><a href="#tab5">Laboratorio General</a></li>
                <li><a href="#tab6">Microbiología</a></li>
                    <%--  <asp:ListItem Value="2">Imprimir por cada determinación</asp:ListItem>--%>
                
               <%-- <li><a href="#tab8">Imagen de Impresión</a></li> No se aplica logo en multiefector.. Todos los labo informan sin logo--%>
                
                <li><a href="#tab9">Codigo de Barras</a></li>     
                <li><a href="#tab10">Genética Forense</a></li> 
                   <li><a href="#tab11">Diagnóstico Presuntivo</a></li> 
                  <li><a href="#tab12">Histocompatibilidad</a></li>                
                     <li><a href="#tab13">Covid-19</a></li>    
                   <li><a href="#tab14">Interoperabilidad SISA</a></li>    
                 <li><a href="#tab15">Etiquetas Areas</a></li>    
            </ul>

                 

 <div id="tab0" class="tab_content" style="border: 1px solid #C0C0C0">
     <table style="width:1000px;">
          <tr>
             <td   style="vertical-align: top"   class="myLabelIzquierdaGde">
               N° de REFES</td>
               <td  >
                   <asp:TextBox ID="lblRefes" class="form-control input-sm" runat="server" Width="400" Enabled="false"></asp:TextBox>
                  
                 
             </td>
         </tr>
         <tr>
             <td   style="vertical-align: top"   class="myLabelIzquierdaGde">
              Region Sanitaria</td>
                <td  >
                     
                    <asp:DropDownList ID="ddlRegion" class="form-control input-sm" runat="server" Enabled="false"></asp:DropDownList>
                 
                 
             </td>
         </tr>

         <tr>
             <td   style="vertical-align: top"  class="myLabelIzquierdaGde">
              ID unico</td>
           <td  >
                 <asp:TextBox ID="lblIdEfector2" runat="server" class="form-control input-sm" Width="80" Enabled="false"></asp:TextBox>
                

                 
             </td>
         </tr>
           <tr>
             <td   style="vertical-align: top"   class="myLabelIzquierdaGde">
               Nombre del jefe</td>
                 <td  >
                                      <asp:TextBox ID="lblJefe" class="form-control input-sm" runat="server" Width="400" Enabled="false"></asp:TextBox>
                 
                 
             </td>
         </tr>
           <tr>
             <td   style="vertical-align: top"   class="myLabelIzquierdaGde">
                Correo de la jefatura</td>
             <td  > 
                                      <asp:TextBox ID="lblCorreoJefe" class="form-control input-sm" runat="server" Width="500" Enabled="false"></asp:TextBox>
                 
                 
             </td>
         </tr>

        <tr>
             <td   style="vertical-align: top" colspan="2" class="myLabelIzquierdaGde">
                </td>
             <td class="style3">
                 &nbsp;
             </td>
         </tr>
         <tr>
             <td   style="vertical-align: top" colspan="2" class="myLabelIzquierdaGde">
                 Configuración de Accesos Directos de Pantalla Principal</td>
             <td class="style3">
                 &nbsp;
             </td>
         </tr>
         <tr>
             <td class="myLabelDerechaGde" style="vertical-align: top" colspan="2">
                <asp:CheckBoxList ID="chkAccesoPrincipal" runat="server">
                     <asp:ListItem Value="0">Turnos</asp:ListItem>
                     <asp:ListItem Value="1">Recepción</asp:ListItem>
                     <asp:ListItem Value="2">Impresion de Hojas de Trabajo</asp:ListItem>
                     <asp:ListItem Value="3">Carga de Resultados</asp:ListItem>
                     <asp:ListItem Value="4">Validacion</asp:ListItem>
                     <asp:ListItem Value="5">Impresión de Resultados</asp:ListItem>
                     <asp:ListItem Value="6">Modulo de Urgencia</asp:ListItem>
                     <asp:ListItem Value="7">Consulta de Resultados</asp:ListItem>
                 </asp:CheckBoxList>
         
             </td>
             <td class="style3">
                 &nbsp;</td>
         </tr>
        
         <tr>
             <td class="myLabelIzquierda" colspan="2">
                 La visualización de los accesos directos queda sujeta a los permisos del 
                 usuario.&nbsp;              </td>
             <td>
                 &nbsp;
             </td>
         </tr>
      
       

         </table>
 </div>
    <div id="tab1" class="tab_content" style="border: 1px solid #C0C0C0">
                                <table style="width:1000px;">
                                    <tr>
                                        <td style="vertical-align: top">
                                           <b  class="myLabelIzquierda"> Tipo de Numeración de Protocolos:</b>
                                               </td>
                                            <td class="myLabelDerechaGde">
                                            <asp:RadioButtonList ID="rdbTipoNumeracionProtocolo" runat="server" 
                                                CssClass="myLabelDerechaGde" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0">Autonumérica única</asp:ListItem>
                                                <asp:ListItem Value="1">Por día</asp:ListItem>
                                                <asp:ListItem Value="2">Por servicio/sector</asp:ListItem>
                                                <asp:ListItem Value="3">Autonumérica diferenciada por Tipo de Servicio</asp:ListItem>
                                            </asp:RadioButtonList>
                                             
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabel" colspan="2">
                                     <asp:Label ID="lblMensajeNumeracion" runat="server" Font-Italic="False" 
                                                ForeColor="#CC3300" 
                                                Text="No es posible modificar el tipo de numeración; ya que hay protocolos cargados" 
                                                Visible="False" CssClass="myLabelDerechaGde"></asp:Label></tr>
                        
                       

                        
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="2">
                                            <hr /></td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="2">
                                            Carga de Protocolos</td>
                                    </tr>

                                       <tr>
                                        <td class="myLabelIzquierda">
                                            Validar con Renaper:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlRenaper" runat="server"  class="form-control input-sm" Width="100px"
                                                
                                                >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtUrlRenaper" Width="500px" runat="server" class="form-control input-sm"></asp:TextBox>

                                        </td>
                                             <tr>
                                        <td class="myLabelIzquierda">
                                            Validar con MPI-Andes:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlAndes" runat="server"  class="form-control input-sm" Width="100px"
                                                
                                                >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtUrlAndes" Width="500px" runat="server" class="form-control input-sm"></asp:TextBox>

                                        </td>
                                    </tr>
                                        <tr>
                                        <td class="myLabelIzquierda">
                                            Médico Obligatorio:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlMedicoObligatorio" runat="server"  class="form-control input-sm" Width="100px"
                                                
                                                >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtUrlMatriculacion" Width="500px" runat="server" class="form-control input-sm"></asp:TextBox>

                                        </td>
                                    </tr>
                        
                                        <tr>
                                        <td class="myLabelIzquierda">
                                           Diagnostico Obligatorio:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlDiagnostico" runat="server"  class="form-control input-sm" Width="100px"
                                                
                                                >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Recordar el último Origen cargado:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlRecordarOrigenProtocolo" runat="server"  class="form-control input-sm" Width="100px"
                                                
                                                onselectedindexchanged="ddlRecordarOrigenProtocolo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                              <tr>
                                        <td class="myLabelIzquierda">
                                            Mostrar notificación de ingreso del paciente:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlVerificaIngreso" runat="server"  class="form-control input-sm" Width="100px"
                                                
                                               >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                            Si elige SI, se mostrará aviso si el paciente tuvo ingreso en los ultimos 7 dias.
                                        </td>
                                    </tr>
                          <tr>
                                        <td class="myLabelIzquierda">
                                          Sector por defecto:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlSectorDefecto" runat="server"   class="form-control input-sm" >
                                             
                                            </asp:DropDownList>
                                            Si elige un sector por defecto el mismo se grabará en todos los protocolos y se ocultará su ingreso.

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Recordar el último Sector cargado:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlRecordarSectorProtocolo" runat="server"   class="form-control input-sm"
                                               Width="100px"
                                                onselectedindexchanged="ddlRecordarOrigenProtocolo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Fecha de Orden:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlFechaOrden" runat="server"    class="form-control input-sm"   Width="150px"                                    
                                                >
                                                <asp:ListItem Value="0">Sin Dato</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="1">Fecha Actual</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Fecha Toma Muestra:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlFechaTomaMuestra" runat="server" Width="150px" class="form-control input-sm">
                                                <asp:ListItem Value="0" >Sin Dato</asp:ListItem>
                                                <asp:ListItem Value="1" Selected="True">Fecha Actual</asp:ListItem>
                                                
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                                                                        Origen por defecto (Módulo Urgencias):</td>
                                        <td class="myLabelDerechaGde">
                                            
         <asp:DropDownList ID="ddlOrigenUrgencia" runat="server" Width="200px"  class="form-control input-sm">
         </asp:DropDownList>
                                       </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Sector por defecto (Módulo Urgencias):</td>
                                        <td class="myLabelDerechaGde">
                                            
         <asp:DropDownList ID="ddlSectorUrgencia" runat="server"  class="form-control input-sm">
         </asp:DropDownList>
                                     </td>


                                    </tr>
                          <tr>
                                        <td class="myLabelIzquierda">
                                            Origenes habilitados:</td>
                                        <td class="myLabelDerechaGde">
                                            <asp:CheckBoxList ID="chkOrigen" runat="server" RepeatDirection="Vertical"></asp:CheckBoxList>
         

                                     </td>
                              </tr>
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="2">
                                           <hr /></td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Habilita modificación de Protocolos Terminados:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlModificaProtocoloTerminado" runat="server" Width="100px" class="form-control input-sm"
                                              
                                                onselectedindexchanged="ddlRecordarOrigenProtocolo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Habilita eliminación de Protocolos:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlEliminaProtocoloTerminado" runat="server" Width="100px" class="form-control input-sm"
                                               
                                                onselectedindexchanged="ddlRecordarOrigenProtocolo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Habilita No publicacion de Protocolo</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlHabilitaPublicacionProtocolo" runat="server" Width="100px" class="form-control input-sm"
                                               
                                                onselectedindexchanged="ddlRecordarOrigenProtocolo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="2">
                                           <hr /></td>
                                    </tr>
                        
                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Tamaño máximo de la lista de Protocolos:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            Mostrar
                                            
                                            <asp:DropDownList ID="ddlPaginadoProtocolo" runat="server" Width="100px" class="form-control input-sm"
                                             
                                                onselectedindexchanged="ddlRecordarOrigenProtocolo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="25">25</asp:ListItem>
                                                <asp:ListItem Value="50">50</asp:ListItem>
                                                <asp:ListItem>100</asp:ListItem>
                                            </asp:DropDownList>
                                        &nbsp;protocolos por página

                                        </td>
                                    </tr>
                                           <tr>
                                                <td class="myLabelIzquierda">
                                           Tipo Orden de la lista de Protocolos por Defecto:</td>
                                        <td class="myLabelIzquierda">
                                                <asp:DropDownList ID="ddlOrdenProtocolos" runat="server">
                                                                   <asp:ListItem Selected="True" Value="Asc">Ascendente</asp:ListItem>
                                                                   <asp:ListItem Value="Desc">Descendente</asp:ListItem>
                                                               </asp:DropDownList>
                                            </td>
                                               </tr>
                                           
                        
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="2">
                                           <hr /></td>
                                    </tr>
                        
            

                                    <tr>
                                        <td class="myLabelIzquierda">
                                            Formato por defecto del
                                            Listado Ordenado: </td>
                                        <td>
                                            
                                            <asp:RadioButtonList ID="rdbTipoListaProtocolo" runat="server" CssClass="myLabelDerechaGde"
                                                RepeatDirection="Horizontal" >
                                                <asp:ListItem Value="0" Selected="True">Formato Reducido (Nombre)</asp:ListItem>
                                                    <asp:ListItem Value="2">Formato Reducido (Codigo)</asp:ListItem>
                                              <%--  <asp:ListItem Value="1">Formato Extendido</asp:ListItem>--%>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                        
                                   
                                   
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="2">
                                            <hr /></td>
                                    </tr>
                        
                                   
                                   
                                    </table>
                            </div>
                       
    <div id="tab3" class="tab_content" style="border: 1px solid #C0C0C0">
                              <table  >
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top">
                                            ¿Trabaja con turnos:?</td>
                                        <td class="myLabelIzquierda" style="vertical-align: top" 
                                            rowspan="2">
                                            
                                            <asp:DropDownList ID="ddlTurno" runat="server"  class="form-control input-sm">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top">
                                            (Habilita en la opcion de menu el acceso a generar turnos)</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top" colspan="2">
                                           <hr /></td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top">
                                            Habilita generación de comprobante de Turnos:</td>
                                        <td class="myLabelIzquierda" style="vertical-align: top" rowspan="3">
                                            <asp:DropDownList ID="ddlTurnoComprobante" runat="server"  class="form-control input-sm">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top">
                                            (Muestra la opción de imprimir un comprobante del turno <br /> despues de guardado el 
                                            mismo)</td>
                                    </tr>
                                    <tr>
                                        <td class="style17" style="vertical-align: top">
                                           </td>
                                    </tr>
                                   <tr>
                                        <td class="myLabel" style="vertical-align: top" colspan="2">
                                           <hr /></td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda">
                                      <img src="App_Themes/default/images/new.png" />      Envía mensaje de texto al momento de cancelar turno:</td>
                                           <td class="myLabelIzquierda" style="vertical-align: top">
                                                   <asp:DropDownList ID="ddlSmsCancelaTurno" runat="server"  class="form-control input-sm" >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td class="myLabel" style="vertical-align: top" colspan="2">
                                           <hr /></td>
                                    </tr>
                                    <tr>
                                        <td >
                                            <asp:Button ID="btnFeriado" runat="server"  OnClientClick="editFeriado(); return false;" Text="Configurar Feriados" 
                                                 ToolTip="Configurar Feriados" Width="200px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                       
    <div id="tab2" class="tab_content" style="border: 1px solid #C0C0C0">
                               <table style="width:1000px;">
                                    <tr class="myLabelIzquierda">
                                      <td class="myLabelIzquierda">
                                            Días para la entrega de Resultados:</td>
                                        <td>
                                            <anthem:RadioButtonList ID="rdbDiasEspera" runat="server" 
                                                onselectedindexchanged="rdbDiasEspera_SelectedIndexChanged" 
                                                RepeatDirection="Horizontal" Width="100%" CssClass="myLabel">
                                                <Items>
                                                    <asp:ListItem Selected="True" Value="0">Calcular segun la duración de los 
                                                    analisis</asp:ListItem>
                                                    <asp:ListItem Value="1">Valor Predeterminado</asp:ListItem>
                                                </Items>
                                            </anthem:RadioButtonList>
                                       
                                    </tr>
                                   <tr>
                                        <td class="myLabelIzquierda">
                                            Días de espera predeterminado:</td>
                                        <td class="myLabelIzquierda">
                                            <input id="txtDiasEntrega" runat="server"  class="form-control input-sm" maxlength="5" 
                                                onblur="valNumero(this)" size="5"  type="text" /><anthem:RequiredFieldValidator 
                                                ID="rfvDiasEspera" runat="server" ControlToValidate="rdbDiasEspera" 
                                                Enabled="False" ErrorMessage="*">*</anthem:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top" colspan="2">
                                           <hr /></td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top">
                                            Calendario de Entrega:</td>
                                        <td>
                            <anthem:RadioButtonList ID="rdbTipoDias" runat="server" AutoCallBack="True" 
                                onselectedindexchanged="rdbTipoDias_SelectedIndexChanged" 
                                RepeatDirection="Horizontal" CssClass="myLabelDerechaGde">
                                <Items>
                                    <asp:ListItem Value="0">Todos los días</asp:ListItem>
                                    <asp:ListItem Value="1">Días habiles</asp:ListItem>
                                </Items>
                            </anthem:RadioButtonList>
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top">
                                            &nbsp;</td>
                                        <td>
                            <anthem:CheckBoxList ID="cklDias" runat="server" RepeatColumns="5" 
                                RepeatDirection="Horizontal" CssClass="myLabelDerechaGde">
                                <Items>
                                    <asp:ListItem Value="1">Lunes</asp:ListItem>
                                    <asp:ListItem Value="2">Martes</asp:ListItem>
                                    <asp:ListItem Value="3">Miercoles</asp:ListItem>
                                    <asp:ListItem Value="4">Jueves</asp:ListItem>
                                    <asp:ListItem Value="5">Viernes</asp:ListItem>
                                    <asp:ListItem Value="6">Sabado</asp:ListItem>
                                    <asp:ListItem Value="0">Domingo</asp:ListItem>
                                </Items>
                            </anthem:CheckBoxList>
                                        
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        
                        
                        
                        
                        
                 
    <div id="tab4" class="tab_content" style="border: 1px solid #C0C0C0">
                             <table style="width:1000px;">
                                    <tr>
                                        <td  style="width: 368px" >
                                      <label>      Carga y Validación de Resultados por Defecto:</label></td>
                                        <td style="width: 4px">
                                            &nbsp;</td>
                                        <td class="myLabelIzquierda" style="width: 603px">
                                            <asp:RadioButtonList ID="rdbCargaResultados" runat="server" 
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="0">Por lista de Protocolos</asp:ListItem>
                                                <asp:ListItem Value="1">Por Hoja de Trabajo</asp:ListItem>
                                                <asp:ListItem Value="2">Por Análisis</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                   
                                    <tr>
                                        <td class="myLabelIzquierdaGde" colspan="3" >
                                            <hr /></td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 368px" >
                                            Orden de carga/validación de resultados:</td>
                                        <td style="width: 4px">
                                            
                                            &nbsp;</td>
                                        <td class="myLabelIzquierda" rowspan="2" style="width: 603px">
                                            
                                              <asp:RadioButtonList ID="rdbOrdenCargaResultados" 
                                                  runat="server">
                                                <asp:ListItem Selected="True" Value="0">Orden de carga en la recepcion del 
                                                  paciente</asp:ListItem>
                                                <asp:ListItem Value="1">Orden de impresion de resultados</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="width: 368px" >
                                            (Solo aplicable para la carga y validación de resultados
                                            <br />
                                             por Lista de Protocolos)</td>
                                        <td style="width: 4px">
                                            
                                            &nbsp;</td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" colspan="3" >
                                           <hr /></td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 368px" >
                                            ¿Aplicar Fórmula por Defecto?:</td>
                                        <td style="width: 4px">
                                            
                                            &nbsp;</td>
                                        <td class="myLabelIzquierda" style="width: 603px">
                                            
                                            <asp:DropDownList ID="ddlAplicaFormula" runat="server"  class="form-control input-sm" Width="200px">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="width: 368px" >
                                            (Aplicable solo para la carga y validación de resultados por Lista de Protocolos.
                                            Despues de guardar aplica las formulas)</td>
                                        <td style="width: 4px">
                                            
                                            &nbsp;</td>
                                        <td style="width: 603px">
                                            
                                            &nbsp;</td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" colspan="3" >
                                           <hr /></td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 368px" >
                                            <b class="mytituloRojo">Firma Electrónica:</b> <br />¿Requiere nueva autenticación para validar?</td>
                                        <td style="width: 4px">
                                            
                                            &nbsp;</td>
                                        <td class="myLabelIzquierda" style="width: 603px">
                                            
                                            <asp:DropDownList ID="ddlAutenticaValidacion" runat="server"  class="form-control input-sm" Width="200px">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="width: 368px" >
                                            (Solicitará nuevamente que el usuario se autentique<br />
&nbsp;al ingresar el módulo de validación)</td>
                                        <td style="width: 4px">
                                            
                                            &nbsp;</td>
                                        <td style="width: 603px">
                                            
                                            &nbsp;</td>
                                        <td style="width: 7px"  >
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td   style="vertical-align: top" colspan="4">
                                           <hr /></td>
                                    </tr>
                               
                                    </table>
                            </div>
                        
                        
                        
                   
    <div id="tab5" class="tab_content" style="border: 1px solid #C0C0C0">
    <table width="100%">
                                 
                  
                 
               </table>
                                <table style="width:100%;">
                                    <tr>
                                        <td class="myLabelIzquierda" colspan="2">
                                           Comprobante para el Paciente<hr /></td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            Habilita generación de comprobante de Protocolo:</td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:DropDownList ID="ddlProtocoloComprobante" runat="server"   class="form-control input-sm"                                               >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList> 
                                         
                                        </td>
                                    </tr>
                                 
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            Texto Adicional al Pie:</td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:TextBox ID="txtTextoAdicionalComprobante" runat="server"   class="form-control input-sm"  
                                                Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                 
                                    
                                    <tr>
                                        <td  colspan="2">
                                           <br /> &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" colspan="2">
                                            Impresión de Resultados<hr /></td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            Encabezado en linea 1: </td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:TextBox ID="txtEncabezado1" runat="server" class="form-control input-sm"  
                                                Width="300px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            Encabezado en linea 2:</td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:TextBox ID="txtEncabezado2" runat="server" class="form-control input-sm"   
                                                Width="300px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            Encabezado en linea 3:</td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:TextBox ID="txtEncabezado3" runat="server" class="form-control input-sm"  
                                                Width="300px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                  
                                           
                                        <td class="myLabelIzquierda" style="vertical-align: top; width: 328px;">
                                            Tamaño de la Hoja de Resultados:</td> 
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlTipoHojaImpresionResultados" runat="server" class="form-control input-sm"   >
                                              <asp:ListItem Value="A4">A4</asp:ListItem>
                                                <asp:ListItem Value="A5">A5</asp:ListItem>
                                            </asp:DropDownList>  
                                                        </td>
                                    </tr>
                                   
                                       <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top; width: 328px;">
                                            Impresión de Resultados:</td>
                                        <td rowspan="2" style="vertical-align: top">
                                          <asp:RadioButtonList CssClass="myLabelDerechaGde" ID="rdbTipoImpresionResultado" runat="server" 
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="1">Imprimir en Hojas Separadas</asp:ListItem>
                                                <asp:ListItem Value="0">Imprimir a continuación</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                      
                                       <tr>
                                  <td class="myLabel" style="vertical-align: top; width: 328px;">
                                            Opción solo aplicable para Tamaño de Papel A4</td>
                                            </tr>
                                    <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top" colspan="2">
                                           </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            Datos del Protocolo a imprimir:</td>
                                        <td  rowspan="2" class="style8" style="width: 652px">
                                            <asp:CheckBoxList ID="chkDatosProtocoloImprimir" runat="server" CssClass="myLabelDerechaGde" 
                                                RepeatColumns="3">
                                                <asp:ListItem Value="0">Nro. Registro</asp:ListItem>                                                                                           
                                                <asp:ListItem Value="1">Fecha de Entrega</asp:ListItem> 
                                                    <asp:ListItem Value="2">Sector</asp:ListItem>
                                                <asp:ListItem Value="3">Solicitante</asp:ListItem>
                                                <asp:ListItem Value="4">Origen</asp:ListItem>
                                                <asp:ListItem Value="5">Prioridad</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top; width: 328px;">
                                            (Seleccionar los datos opcionales a imprimir del protocolo 
                                            <br />
                                            en la hoja de resultados)</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top" colspan="2">
                                           </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top; width: 328px;">
                                            Datos del Paciente a imprimir:</td>
                                        <td rowspan="2">
                                            <asp:CheckBoxList ID="chkDatosPacienteImprimir" runat="server" CssClass="myLabelDerechaGde" 
                                                RepeatColumns="4">
                                                <asp:ListItem Enabled="False" Selected="True" Value="0">Apellido y Nombres</asp:ListItem>                                                                                           
                                                <asp:ListItem Value="1">DNI</asp:ListItem> 
                                                    <asp:ListItem Value="2" Enabled="False">Nro. HC (sin uso)</asp:ListItem>
                                                <asp:ListItem Value="3">Edad</asp:ListItem>
                                                <asp:ListItem Value="4">Fecha Nacimiento</asp:ListItem>
                                                <asp:ListItem Value="5">Sexo</asp:ListItem>
                                                <asp:ListItem Value="6">Domicilio</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top; width: 328px;">
                                            (Seleccionar los datos opcionales a imprimir del paciente 
                                            <br />
                                            en la hoja de resultados)</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top" colspan="2">
                                          </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top; width: 328px;">
                                          Firma Electrónica:</td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlImprimePieResultados" runat="server" class="form-control input-sm"  Width="200px"
                                            >
                                                <asp:ListItem Selected="True" Value="0">No usar</asp:ListItem>
                                               
                                                <asp:ListItem Value="2">Imprimir por cada determinación</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top; width: 328px;">
                                            (Se imprimirá el apellido y nombre de la/s persona/s que validaron los resultados)</td>
                                        <td class="myLabel" style="vertical-align: top;">
                                            *Imprimir por cada determinación: solo disponible en formato A4</td>
                                    </tr>
                                   
                            
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top; width: 328px;">
                                          ¿Activa alerta Petición Electrónica?:</td>
                                        <td class="myLabel" style="vertical-align: top;">
                                            <asp:DropDownList ID="ddlPeticionElectronica" runat="server" class="form-control input-sm"  Width="100px"
                                            >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                                              
                                </table>
                                
                                
                            </div>
            <div id="tab6" class="tab_content" style="border: 1px solid #C0C0C0">
            
            
          
                                <table style="width:100%;">

                                  
                                  



                                  

                                    <tr>
                                        <td   style="width: 328px">
                                            <h6>  Habilita generación de comprobante de Protocolo:</h6></td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:DropDownList ID="ddlProtocoloComprobanteMicrobiologia" runat="server"   Width="100px"
                                         class="form-control input-sm"     >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%--  <asp:ListItem Value="2">Imprimir por cada determinación</asp:ListItem>--%>
                                    <tr>
                                        <td   style="width: 328px">
                                         <h6>   Texto Adicional al Pie de Comprobante:</h6></td>
                                        <td class="myLabelIzquierda"  style="width: 652px">
                                            <asp:TextBox ID="txtTextoAdicionalComprobanteMicrobiologia" runat="server"  class="form-control input-sm"  
                                                Width="500px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                      
                                    <tr>
                                        <td class="myLabelIzquierda" style="width: 328px">
                                            &nbsp;</td>
                                        <td style="width: 652px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td  colspan="2">
                                         <h4>   Impresión de Resultados</h4><hr /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 328px">
                                       <h6>     Encabezado en linea 1: </h6> </td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:TextBox ID="txtEncabezado1Microbiologia" runat="server" class="form-control input-sm"  
                                                Width="300px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td   style="width: 328px">
                                      <h6>     Encabezado en linea 2:</h6></td>
                                        <td class="myLabelIzquierda"  style="width: 652px">
                                            <asp:TextBox ID="txtEncabezado2Microbiologia" runat="server" class="form-control input-sm"  
                                                Width="300px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="width: 328px">
                                      <h6>      Encabezado en linea 3:</h6></td>
                                        <td class="myLabelIzquierda"  style="width: 652px">
                                            <asp:TextBox ID="txtEncabezado3Microbiologia" runat="server" class="form-control input-sm"  
                                                Width="300px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                  
                                            <tr>
                                        <td   style="vertical-align: top; width: 328px;">
                                        <h6>  Tamaño de la Hoja de Resultados:</h6></td> 
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlTipoHojaImpresionResultadosMicrobiologia" runat="server" class="form-control input-sm"   Width="100px"
                                             >
                                                <asp:ListItem Value="A4">A4</asp:ListItem>
                                                <asp:ListItem Value="A5">A5</asp:ListItem>
                                            </asp:DropDownList>  
                                                        </td>
                                    </tr>
                                     
                                       <tr>
                                        <td   style="vertical-align: top; width: 328px;">
                                       <h6>         Impresión de Resultados:</h6></td>
                                        <td   style="vertical-align: top">
                                          <asp:RadioButtonList CssClass="myLabelDerechaGde" ID="rdbTipoImpresionResultadoMicrobiologia" runat="server" 
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="1">Imprimir en Hojas Separadas</asp:ListItem>
                                                <asp:ListItem Value="0">Imprimir a continuación</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                   
                                       <tr>
                                  <td class="myLabel" style="vertical-align: top; width: 328px;">
                                       </td>
                                            <td class="myLabel" style="vertical-align: top; width: 328px;">
                                                Opción solo aplicable para Tamaño de Papel A4
                                           </td>
                                            </tr>
                                    <tr>
                                        <td  style="width: 328px">
                                        <h6>    Datos del Protocolo a imprimir:</h6></td>
                                        <td    >
                                            <asp:CheckBoxList ID="chkDatosProtocoloImprimirMicrobiologia" runat="server" CssClass="myLabelDerechaGde" 
                                                RepeatColumns="6">
                                                <asp:ListItem Value="0">Nro. Registro</asp:ListItem>                                                                                           
                                                <asp:ListItem Value="1">Fecha de Entrega</asp:ListItem> 
                                                    <asp:ListItem Value="2">Sector</asp:ListItem>
                                                <asp:ListItem Value="3">Solicitante</asp:ListItem>
                                                <asp:ListItem Value="4">Origen</asp:ListItem>
                                                <asp:ListItem Value="5">Prioridad</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td   style="vertical-align: top; width: 328px;">
                                         <h6>  Datos del Paciente a imprimir:</h6> </td>
                                        <td rowspan="2">
                                            <asp:CheckBoxList ID="chkDatosPacienteImprimirMicrobiologia" runat="server" CssClass="myLabelDerechaGde" 
                                                RepeatColumns="4">
                                                <asp:ListItem Enabled="False" Selected="True" Value="0">Apellido y Nombres</asp:ListItem>                                                                                           
                                                <asp:ListItem Value="1">DNI</asp:ListItem> 
                                                    <asp:ListItem Value="2" Enabled="False">Numero HC (sin uso)</asp:ListItem>
                                                <asp:ListItem Value="3">Edad</asp:ListItem>
                                                <asp:ListItem Value="4">Fecha Nacimiento</asp:ListItem>
                                                <asp:ListItem Value="5">Sexo</asp:ListItem>
                                                <asp:ListItem Value="6">Domicilio</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                   

                                    <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top" colspan="2">
                                          </td>
                                    </tr>
                                    <tr>
                                        <td   style="vertical-align: top; width: 328px;">
                                       <h6>   Firma Electrónica:</h6></td>
                                        <td class="myLabelIzquierda">
                                            
                                            <asp:DropDownList ID="ddlImprimePieResultadosMicrobiologia" runat="server" class="form-control input-sm"  Width="200px"
                                                >
                                                <asp:ListItem Selected="True" Value="0">No usar</asp:ListItem>
                                                <asp:ListItem Value="1">Imprimir al Pie</asp:ListItem>
                                              <%--  <asp:ListItem Value="2">Imprimir por cada determinación</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="vertical-align: top; width: 328px;">
                                            &nbsp;</td>
                                        <td class="myLabel">
                                            (Se imprimirá el apellido y nombre de la/s persona/s que validaron los resultados)</td>
                                    </tr>
                              
                            
                                </table>
                            </div>
            <div id="tab10" class="tab_content" style="border: 1px solid #C0C0C0">
            
            
          
                                <table style="width:100%;">

                                  
                                <tr>
                                        <td   >
                                         <h6>  Habilita generación de consentimiento en la recepcion de muestras:</h6></td>
                                        <td class="myLabelIzquierda"  >
                                         <asp:DropDownList ID="ddlConsentimientoMicrobiologia" runat="server" Width="100px"
                                           class="form-control input-sm"   >
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>  <img src="App_Themes/default/images/new.png" />  
                                        </td>
                                    </tr>



                            
                                </table>
                            </div>
                 <div id="tab12" class="tab_content" style="border: 1px solid #C0C0C0">
            
            
          
                                <table style="width:100%;">

                                  
                                <tr>
                                        <td   >
                                         <h6>  Seleccione la hoja de trabajo con los items </h6>
                                            <h6>  a incluir en el informe de resultados:</h6></td>
                                        <td class="myLabelIzquierda"  >
                                         <asp:DropDownList ID="ddlHisto" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                            </asp:DropDownList>  <img src="App_Themes/default/images/new.png" />  
                                        </td>
                                    </tr>



                            
                                </table>
                            </div>
                    <div id="tab11" class="tab_content" style="border: 1px solid #C0C0C0">
            
            
          
                                <table style="width:100%;">

                                  
                                <tr>
                                        <td   >
                                         <h6>  Nomenclador de Diagnósticos a utilizar:</h6></td>
                                        <td class="myLabelIzquierda"  >
                                         <asp:DropDownList ID="ddlNomencladorDiagnostico" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                                <asp:ListItem Selected="True" Value="0">Nomenclador Internacional Cie10</asp:ListItem>
                                                <asp:ListItem Value="1">Nomenclador Propio</asp:ListItem>
                                            </asp:DropDownList>  <img src="App_Themes/default/images/new.png" />  
                                        </td>
                                    </tr>



                            
                                </table>
                            </div>
     <div id="tab9"  class="tab_content" style="border: 1px solid #C0C0C0; width: 1000px;"> 

 


<table style="width:90%">
      
       <tr>
                        <td class="myLabelIzquierda" colspan="2">
                            Tipo de Etiqueta:<anthem:DropDownList ID="ddlTipoEtiqueta" runat="server"  class="form-control input-sm" Width="240px" OnSelectedIndexChanged="ddlTipoEtiqueta_SelectedIndexChanged" AutoCallBack="True"                                                
                                            >
            <asp:ListItem Value="5x2.5">5x2.5 (cm)</asp:ListItem>
            <asp:ListItem Value="8x2.5">8x2.5 (cm)/doble</asp:ListItem>
                                              
                                            </anthem:DropDownList></td>
                    </tr>
                    
       <tr>
                        <td class="auto-style1" colspan="2">
                            <anthem:Image ID="imgEtiqueta5"  Visible="false" runat="server" ImageUrl="~/App_Themes/default/images/barCodeEjemplo.png" />
     <anthem:Image ID="imgEtiqueta8" runat="server" Visible="false" ImageUrl="~/App_Themes/default/images/barCodeEjemplo8x2.5.png" />
                        </td>
                    </tr>
                    
       <tr>
                        <td class="style6">
                            &nbsp;</td>
                        <td align="left" class="style8" >
                            &nbsp;</td>
                    </tr>
                    
       <tr>
                        <td >
                            SERVICIO LABORATORIO GENERAL</td>
                        <td   align="left"   >
                            <anthem:CheckBox ID="chkImprimeCodigoBarrasLaboratorio" runat="server"  class="form-control input-sm" Text=" Genera Código de Barras"  
                                oncheckedchanged="chkImprimeCodigoBarrasLaboratorio_CheckedChanged" 
                           
                                AutoCallBack="True" />
                        </td>
                    </tr>
                    
                    <tr>
                    <td colspan="2">
                    <anthem:Panel ID="pnlLaboratorio" Enabled="false" runat="server">
                    <table>
                                        <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 156px;">
                            Fuente de código de barras:</td>
                        <td class="myLabelIzquierda" align="left">
                            <asp:RadioButtonList ID="ddlFuente" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="CCode39">Code 39</asp:ListItem>
                                <asp:ListItem Value="Ccode39M43" Enabled="false">Code 39 Módulo 43</asp:ListItem>
                          <%--      <asp:ListItem Value="EAN-13">EAN-13</asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </td> 
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del protocolo a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkProtocolo" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="False" Selected="True">Numero de Protocolo</asp:ListItem>
                                <asp:ListItem>Fecha</asp:ListItem>
                                <asp:ListItem>Origen</asp:ListItem>
                                <asp:ListItem>Sector</asp:ListItem>
                                <asp:ListItem>Nro. Origen</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del paciente a incluir:</td>
                        <td class="myLabel" align="left">
                            <asp:CheckBoxList ID="chkPaciente" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Apellido y Nombre</asp:ListItem>
                                <asp:ListItem>Sexo</asp:ListItem>
                                <asp:ListItem>Edad</asp:ListItem>
                                <asp:ListItem>Nro. Documento</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
 
 
                    </table>
                    </anthem:Panel>
                    </td>
                    </tr>
    
     <tr>
                    <td colspan="2">
                        <br />
                        <br />
                        </td>
         </tr>
          <tr>
                        <td  style="vertical-align: top; ">
                            SERVICIO MICROBIOLOGIA</td>
                        <td align="left" class="style11"  >
                            <anthem:CheckBox ID="chkImprimeCodigoBarrasMicrobiologia" runat="server" class="form-control input-sm" Text=" Genera Código de Barras"  
                             
                                oncheckedchanged="chkImprimeCodigoBarrasMicrobiologia_CheckedChanged" 
                                AutoCallBack="True" />
                        </td>
                    </tr>
                    
                    <tr>
                    <td colspan="2">
                    <anthem:Panel ID="pnlMicrobiologia" Enabled="false" runat="server">
                    <table>
                                        <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 156px;">
                            Fuente de código de barras:</td>
                        <td class="myLabel" align="left">
                            <asp:RadioButtonList ID="rdbFuente2" runat="server" 
                                RepeatDirection="Horizontal">
                               <asp:ListItem Value="CCode39">Code 39</asp:ListItem>
                                <asp:ListItem Value="Ccode39M43" Enabled="false">Code 39 Módulo 43</asp:ListItem>
                          <%--      <asp:ListItem Value="EAN-13">EAN-13</asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </td>
                                            <td class="myLabel" rowspan="3">
                                            </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del protocolo a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkProtocolo2" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="False" Selected="True">Numero de Protocolo</asp:ListItem>
                                <asp:ListItem>Fecha</asp:ListItem>
                                <asp:ListItem>Origen</asp:ListItem>
                                <asp:ListItem>Sector</asp:ListItem>
                                <asp:ListItem>Nro. Origen</asp:ListItem>
                                <asp:ListItem>Tipo de Muestra</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del paciente a incluir:</td>
                        <td class="myLabel" align="left">
                            <asp:CheckBoxList ID="chkPaciente2" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Apellido y Nombre</asp:ListItem>
                                <asp:ListItem>Sexo</asp:ListItem>
                                <asp:ListItem>Edad</asp:ListItem>
                                <asp:ListItem>Nro. Documento</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
 
 
                    </table>
                    </anthem:Panel>
                    </td>
                    </tr>
     
  
      <tr>
                    <td colspan="2">
                        <br />
                        <br />
                        </td>
         </tr>
          <tr>
                        <td  style="vertical-align: top; ">
                            SERVICIO PESQUISA NEONATAL</td>
                        <td align="left" class="style13" >
                            <anthem:CheckBox ID="chkImprimeCodigoBarrasPesquisa" runat="server" class="form-control input-sm" Text=" Genera Código de Barras"  
                             
                                oncheckedchanged="chkImprimeCodigoBarrasPesquisa_CheckedChanged" 
                                AutoCallBack="True" />
                        </td>
                    </tr>
                    
                    <tr>
                    <td colspan="2">
                    <anthem:Panel ID="pnlPesquisa" Enabled="false" runat="server">
                    <table>
                                        <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 156px;">
                            Fuente de código d e barras:</td>
                        <td class="myLabel" align="left">
                            <asp:RadioButtonList ID="rdbFuente3" runat="server" 
                                RepeatDirection="Horizontal">
                               <asp:ListItem Value="CCode39">Code 39</asp:ListItem>
                                <asp:ListItem Value="Ccode39M43"  Enabled="false">Code 39 Módulo 43</asp:ListItem>
                          <%--      <asp:ListItem Value="EAN-13">EAN-13</asp:ListItem>--%>
                            </asp:RadioButtonList>
                        </td>
                                            <td class="myLabel" rowspan="3">
                                            </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del protocolo a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkProtocolo3" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="False" Selected="True">Numero de Protocolo</asp:ListItem>
                                <asp:ListItem>Fecha</asp:ListItem>
                                <asp:ListItem>Origen</asp:ListItem>
                                <asp:ListItem>Sector</asp:ListItem>
                                <asp:ListItem>Nro. Origen</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del paciente a incluir:</td>
                        <td class="myLabel" align="left">
                            <asp:CheckBoxList ID="chkPaciente3" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Apellido y Nombre</asp:ListItem>
                                <asp:ListItem>Sexo</asp:ListItem>
                                <asp:ListItem>Edad</asp:ListItem>
                                <asp:ListItem>Nro. Documento</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
 
 
                    </table>
                    </anthem:Panel>
                    </td>
                    </tr>
     
   
</table>
     
 
   
     </div>

                     <div id="tab15"  class="tab_content" style="border: 1px solid #C0C0C0; width: 1000px;"> 

 


<table style="width:90%">
      
       <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top">
                           Areas que imprime el el alta de Protocolo:</td>

                        <td class="myLabelIzquierda" >
                            <asp:CheckBoxList ID="chkAreas" runat="server"></asp:CheckBoxList>
                             </td>
                    </tr>
                    
       
              
     
   
</table>
     
 
   
     </div>
                 <%--      <asp:ListItem Value="EAN-13">EAN-13</asp:ListItem>--%>
    <%-- <div id="tab8" class="tab_content" style="border: 1px solid #C0C0C0; width: 1000px;">
     <table>
       <tr>
                                        <td class="myLabelIzquierdaGde" style="vertical-align: top" colspan="3">
                                         </td>
                                    </tr>
                                    <tr>
                                        <td class="myLabelIzquierda" style="vertical-align: top; width: 328px;">
                                            Logo de Impresión de Resultados:</td>
                                        <td class="myLabelIzquierda" style="width: 652px">
                                            <asp:FileUpload ID="fupLogo" runat="server" CssClass="myTexto" Width="250px" 
                                                ondatabinding="fupLogo_DataBinding" />                                                                                      
                                           
                                        </td>
                                        <td   rowspan="3" class="style7" style="width: 6px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="myLabel" style="vertical-align: top; width: 328px;">
                                            La imagen deberá ser de formato PNG.<br />
                                            El tamaño recomendado de la imagen es de 2.54x2.54 cm.<br />
                                            Esta imagen se imprimirá en el margen superior derecho del encabezado.</td>
                                        <td style="width: 652px">
                                          <div  style="width:150px;height:100pt;overflow:scroll;border:1px solid #808080;" title="Imagen Logo"> 
                                          
                                            <asp:Image ID="Image1" runat="server" ImageUrl="" Visible="false" />
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                    <td colspan="2">
                                    <hr />
                                     <asp:CheckBox ID="chkBorrarImagen" runat="server" CssClass="myLabelDerechaGde" 
                                                Text="Borrar Imagen" />
                                    </td>
                                    </tr>
     </table>
     </div>--%>
               
                   <div id="tab13" class="tab_content" style="border: 1px solid #C0C0C0">
             
                       <asp:Panel ID="tabCovid" runat="server">
            <h3>Panel covid </h3>
                                <table style="width:100%;">

                                     <tr>
                                        <td   >
                                         <h6> CODIGO COVID: </h6>
                                            </td>
                                        <td    >
                                            <asp:TextBox ID="txtCodigoCovid" runat="server"  class="form-control input-sm" Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td    >
                                        <h6>    Utiliza Pre Validacion y Confirmación Posterior:</h6></td>
                                       
                                        <td  >
                                            
                                            <asp:DropDownList ID="ddlPreValidacion" runat="server"  class="form-control input-sm" Width="200px">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                      
                                    </tr>
                                      <tr>
                                        <td   >
                                         <h6>  Notificar a Andes: </h6>
                                            </td>
                                        <td    >
                                         <asp:DropDownList ID="ddlNotificaAndes" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                             <asp:ListItem Value="1">SI</asp:ListItem>
                                             <asp:ListItem Value="0">NO</asp:ListItem>
                                            </asp:DropDownList>  <img src="App_Themes/default/images/new.png" />  
                                        </td>
                                    </tr>

     
                                  
                                  
                                <tr>
                                        <td   >
                                         <h6>  Habilita ingreso de Caracter: </h6>
                                            </td>
                                        <td  >
                                         <asp:DropDownList ID="ddlCaracter" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                             <asp:ListItem Value="1">SI</asp:ListItem>
                                             <asp:ListItem Value="0">NO</asp:ListItem>
                                            </asp:DropDownList>  <img src="App_Themes/default/images/new.png" />  
                                        </td>
                                    </tr>



                            
                                <tr>
                                        <td   >
                                          <h6>  Habilita ingreso de Nro. de hisopado:</h6>
                                            </td>
                                        <td    >
                                         <asp:DropDownList ID="ddlCaracter0" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                             <asp:ListItem Value="1">SI</asp:ListItem>
                                             <asp:ListItem Value="0">NO</asp:ListItem>
                                            </asp:DropDownList>    
                                        </td>
                                    </tr>



                            
                                <tr>
                                        <td   >
                                       <h6>     Filtrar por Caracter en hoja de trabajo?</h6></td>
                                        <td    >
                                         <asp:DropDownList ID="ddlCaracter1" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                             <asp:ListItem Value="1">SI</asp:ListItem>
                                             <asp:ListItem Value="0">NO</asp:ListItem>
                                            </asp:DropDownList>    
                                        </td>
                                    </tr>



                            
                                <tr>
                                        <td   >
                                         <h6>   Filtrar por Caracter en Resultados?</h6></td>
                                        <td   >
                                         <asp:DropDownList ID="ddlCaracter2" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                             <asp:ListItem Value="1">SI</asp:ListItem>
                                             <asp:ListItem Value="0">NO</asp:ListItem>
                                            </asp:DropDownList>    
                                        </td>
                                    </tr>
                                       <tr>
                                        <td   >
                                        <h6>    Obligatoriedad de FIS segun Caracter</h6>
                                           
                                        </td>
                                        <td    >
                                            <asp:CheckBoxList ID="chkFISCaracter" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"></asp:CheckBoxList>
                                             <br />
                                            <h6>Se notificaran a SISA como "Sospechosos"</h6>

                                        </td>
                                    </tr>



                            
                                </table>

                           </asp:Panel>
                            </div>


                 <div id="tab14" class="tab_content" style="border: 1px solid #C0C0C0">
                      <h3>SISA</h3>
                      <table style="width:100%;">
                           <tr>
                                        <td   >
                                         <h6>  Notificar a SISA - Automatico: </h6>
                                            </td>
                                        <td   >
                                         <asp:DropDownList ID="ddlNotificarSISA" runat="server" Width="250px"
                                           class="form-control input-sm"   >
                                             <asp:ListItem Value="1">SI</asp:ListItem>
                                             <asp:ListItem Value="0">NO</asp:ListItem>
                                            </asp:DropDownList>   
                                        </td>
                                    </tr>
                                     <tr>
                                        <td   >
                                         <h6>  Url Servicio Nuevo Evento: </h6>
                                            </td>
                                        <td    >
                                            <asp:TextBox ID="txtUrlServicioSISA" runat="server"  class="form-control input-sm" Width="600px"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td   >
                                         <h6>  Url Servicio Muestra: </h6>
                                            </td>
                                        <td   >
                                            <asp:TextBox ID="txtUrlMuestraSISA" runat="server"  class="form-control input-sm" Width="600px"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td   >
                                         <h6>  Url Servicio Resultado: </h6>
                                            </td>
                                        <td   >
                                            <asp:TextBox ID="txtUrlResultadoSISA" runat="server"  class="form-control input-sm" Width="600px"></asp:TextBox>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td   >
                                         <h6>  Codigo Establecimiento: </h6>
                                            </td>
                                        <td    >
                                            <asp:TextBox ID="txtCodigoEstablecimientoSISA" runat="server"  class="form-control input-sm" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>

                          </table>
                     </div>
                        </div> 
                      </td>     


         </tr>
                      
            

            
      
        </table>
          </div>

        	<div class="panel-footer">	
                                               
            <asp:Button ID="btnReinializacion" runat="server" Width="150px" CssClass="btn btn-info" Text="Mantenimiento" Visible="false"  onclick="btnReinializacion_Click" />
                                               
            <asp:Button ID="btnGuardar" runat="server" Width="90px" CssClass="btn btn-info" Text="Actualizar" onclick="btnGuardar_Click" />
                </div>
       </div>
        </div>
      <script src="script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

                      function editFeriado() {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }


        var $this = $(this);
        $('<iframe src="Turnos/FeriadoEdit.aspx" />').dialog({
            title: 'Feriados',
            autoOpen: true,
            width:480,
            height: 400,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(500);
    }
        
   
    
    </script>

    
    
    
    

    
</asp:Content>
