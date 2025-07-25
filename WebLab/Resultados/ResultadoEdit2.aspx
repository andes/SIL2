<%@ Page  Language="C#"   AutoEventWireup="true" CodeBehind="ResultadoEdit2.aspx.cs" Inherits="WebLab.Resultados.ResultadoEdit2" EnableEventValidation="true"  MasterPageFile="~/Site1.Master"%>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<%--<%@ Register src="../Calidad/IncidenciaEdit.ascx" tagname="IncidenciaEdit" tagprefix="uc1" %>
--%>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 
    

                   <script type="text/javascript">                     
                                      

  
  

             $(function() {
                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                        $("#tabContainer").tabs({ selected: currTab });
             });

                    function Enter(field, event) {
                        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
                        if (keyCode == 13) {
                            var i;
                            for (i = 0; i < field.form.elements.length; i++)
                                if (field == field.form.elements[i])
                                    break;
                            i = (i + 1) % field.form.elements.length;
                            field.form.elements[i].focus();
                            return false;
                        }
                        else
                            return true;

                    }
             
             function enterToTab(pEvent) {////ie
                if (window.event.keyCode == 13  )
                {                           
                    window.event.keyCode = 9;
                }
            }            
    </script>   
       <style type="text/css">
#CeldaContenedor > * {
    display: inline-block; /* Hace que los controles se alineen en una sola línea */
    float: left; /* Alinea los controles a la izquierda */
}
#CeldaContenedor {
    text-align: left; /* Alinea todo el contenido a la izquierda */
}
</style>
    </asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
     
<div align="left" style="width: 1400px">
     
                 <table  width="98%"    align="center" cellpadding="0" cellspacing="0" style="vertical-align: top">
                     <tr>
                        <td colspan="3">
                        
  <h5> <asp:Label ID="lblTitulo" runat="server" Text="Carga de Resultados" 
                                ForeColor="#2B7EBD"></asp:Label><small>  &nbsp;    <asp:Label ID="lblServicio" runat="server" 
                                       Text="Label"></asp:Label></small></h5>  <input id="HdidEventoSISA" type="hidden" runat="server" />
             
                                                    </td>
					</tr>
                    				
					
					
					
						<tr>
						 <td   align="center" style="vertical-align: top">
                            <table style="width:180px;" cellpadding="0" cellspacing="0">
                              
                                <tr>
                                    <td align="center">
                                         <div class="panel panel-primary">
  <div class="panel-heading"> <asp:Label ID="lblCantidadRegistros" runat="server" Text="Label"></asp:Label>
                                             </div>
  <div class="panel-body">
                                        <div  style="width:100%;height:350pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                                            <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                            DataKeyNames="idProtocolo" onrowcommand="gvLista_RowCommand" 
                                            onrowdatabound="gvLista_RowDataBound" CellPadding="3" 
                                            HorizontalAlign="Left"  CssClass="table table-bordered bs-table" 
                                            BorderWidth="1px" GridLines="None" Font-Bold="False"  >
                                            
                                            <Columns>
                                            <asp:BoundField DataField="numero" HeaderText="Protocolo" />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:ImageButton ID="Pdf" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                                            CommandName="Pdf" />
                                            </ItemTemplate>

                                            <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="estado" Visible="False" />

                                            </Columns>
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" 
                                            Font-Names="Arial" Font-Size="8pt" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
                                            </asp:GridView>
                                     </div>
      </div>

                                              <div class="panel-footer">
                                                   <asp:HyperLink ID="hypRegresar" AccessKey="R" ToolTip="Alt+Shift+R" runat="server" >Regresar</asp:HyperLink>
                                             </div>
                                             </div>
                                        <asp:ImageButton ImageUrl="~/App_Themes/default/images/actualizar.gif"  ID="btnActualizar"  runat="server"  ToolTip="Ctrl+F4"
                                   onclick="btnActualizarPracticas_Click"
                        ></asp:ImageButton>    
                                        </td>
                                </tr>
                            </table>
                           
                        </td>				
					
					
						
						
						<td   align="left" style="vertical-align: top">
                            &nbsp;&nbsp;</td>
                            
						<td style="vertical-align: top"> 
                            
                             <table cellpadding="0" cellspacing="0">
                             
							<tr>
						
						<td align="left"  style="vertical-align: top" colspan="3">  
                            <ul class="pagination">
                                     <li>   
                                <asp:LinkButton  AccessKey="<" ID="lnkAnterior" runat="server"  
                                onclick="lnkAnterior_Click">&lt;Anterior</asp:LinkButton>
                                          </li>
                                                               <li> <asp:LinkButton 
                                ID="lnkPosterior" runat="server" 
                                onclick="lnkPosterior_Click">Posterior&gt;</asp:LinkButton></li>
                                                               </ul>
						 
                            <asp:Panel ID="pnlPaciente" runat="server">
                                       <div class="panel panel-default" >
  <div class="panel-heading"> Datos del protocolo </div>
  <div class="panel-body">
						<table align="left"  >
      						
					
					
					<tr>
							<td class="myLabelIzquierda" width="150px">
                                DU:</td>
						<td  width="200px">
                            <asp:Label ID="lblDni" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="9pt"></asp:Label>
                        </td>
						<td class="myLabelIzquierda" width="100px">
                            Protocolo:</td>
						<td 
                            align="left"   width="150px"  >
                            <asp:Image ID="imgEstado" runat="server" 
                                ImageUrl="~/App_Themes/default/images/amarillo.gif" Width="12px" Height="12px" />
                              
                            <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="12pt" ></asp:Label>
                            </td>
                      	<td class="myLabelIzquierda">
                           Fecha:</td>
                        <td     align="left"  width="130px" >   
                                     <asp:Label Font-Bold="True" 
                                Font-Size="9pt" ID="lblFecha" runat="server" Text="Label"></asp:Label>    </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierda">
                                Paciente:</td>
						<td >
                            <asp:Label ID="lblPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="9pt"></asp:Label>
                            <asp:Label ID="lblCodigoPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="9pt" Visible="False"></asp:Label>
                        </td>
						<td class="myLabelIzquierda">
                            Nro. Origen:</td>
						<td 
                            align="left"   >
                            <asp:Label ID="lblNumeroOrigen" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                            </td>
                      	<td class="myLabelIzquierda">
                            Toma Muestra:</td>
                        <td   align="left">   
                                     <asp:Label Font-Bold="True" 
                                Font-Size="9pt" ID="lblFechaTomaMuestra" runat="server" Text="Label"></asp:Label>    </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierda">
                            Sexo:</td>
						<td class="auto-style1"  >
                            <asp:Label ID="lblSexo" runat="server" Text="Label" CssClass="myLabel" ></asp:Label>
                        </td>
						<td class="myLabelIzquierda">
                            Origen:
                            </td>
						<td 
                            align="left" colspan="3"   >
                            <asp:Label ID="lblOrigen" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                            <asp:Label ID="lblSector" runat="server" CssClass="myLabel" Text=""></asp:Label>
                        </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierda">
                                Fecha Nac. - Edad:</td>
						<td class="auto-style1"  >
                            <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                        &nbsp;<asp:Label ID="lblEdad" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>   
                        </td>
						<td class="myLabelIzquierda" >
                            Prioridad:
                            </td>
						<td   
                            align="left" >
                            <asp:Label ID="lblPrioridad" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                        </td>
                    	<td class="myLabelIzquierda"> Usuario:</td>
                        <td    align="left"> <asp:Label  ID="lblUsuario" runat="server" Text="Label"></asp:Label>                     
                       
                        </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierda">
                                Solicitante:</td>
						<td colspan="3">
                           
                            <asp:Label ID="lblSolicitante" runat="server" CssClass="myLabel" Text="Label"></asp:Label>
                            &nbsp;<asp:Label ID="lblMedico" runat="server" CssClass="myLabel" Text="Label"></asp:Label>
                        </td>
                       	<td class="myLabelIzquierda">     
                               Registro:</td>
                       <td   align="left">     
                                <asp:Label  ID="lblFechaRegistro" runat="server" Text="Label" CssClass="myLabel"></asp:Label>                
                                 <asp:Label  ID="lblHoraRegistro" runat="server" Text="Label" CssClass="myLabel"></asp:Label>    
                        </td>
					</tr>
						
							
						
					
					
					<tr>
					    <td class="myLabelIzquierda">Diagnósticos:<asp:ImageButton ID="imgDiagnostico" runat="server" ImageUrl="~/App_Themes/default/images/add.png" onclick="btnActualizarPracticas_Click" OnClientClick="editDiagnostico(); return false;" ToolTip="Agregar o quitar Diagnósticos" Visible="false" />
                        </td>                        
						<td colspan="3">
                            <asp:Label ID="lblDiagnostico" runat="server" Text="" Font-Bold="False" 
                                Font-Size="9pt" ></asp:Label>                                                                                                               
                                
                        </td>
                        
					    <td class="myLabelIzquierda" colspan="2">
                            <asp:Label ID="lblCovid" runat="server" Visible="false" CssClass="myLabelRojo"></asp:Label>
                        </td>
                        
					</tr>
						
						
						<tr>
						<td class="myLabelIzquierda">Observaciones Internas:</td>
						<td colspan="5"  >
                            <asp:Panel ID="pnlObservaciones" runat="server">
                            <asp:Label ID="lblObservacion" runat="server" Text="Label"  CssClass="mytituloRojo"></asp:Label> 
                            </asp:Panel>
						   
                            </td>
					</tr>
                            
					        <tr>
                                <td class="myLabelIzquierda">Placa Nro:</td>
                                <td colspan="5">
                                    <asp:Label ID="lblPlaca" runat="server" CssClass="myLabel" Text=""></asp:Label>
                                </td>
                            </tr>
                            
					</table>
      </div>
                                           </div>
                                </asp:Panel>
                            

                            <asp:Panel ID="pnlProducto" runat="server" Visible="false">
                           
                           <div class="panel panel-default">
  <div class="panel-heading"> Datos del protocolo </div>
  <div class="panel-body">
						<table " width="100%">
      						
					
					
					<tr>
						<td class="auto-style4">
                            Numero:</td>
						<td 
                            align="left" class="auto-style3"  >
                            
                               <asp:Label ID="lblProtocolo1" runat="server" Font-Bold="True" Font-Size="10pt" Text="Label"></asp:Label>
                              
                         
                            </td>
                      	<td>
                                 </td>
                        <td     align="right">   
                                 <asp:Label ID="lblEstado1" runat="server" Font-Bold="True" Font-Size="8pt" Text="Label"></asp:Label></td>
					</tr>
						
					
					
					       
                            <tr>
                                <td class="auto-style4">Descripcion:</td>
                                <td align="left" class="auto-style3">
                                    <asp:Label ID="lblDescripcionProducto" runat="server" CssClass="myLabelIzquierda" Font-Bold="True" Font-Size="9pt" Text="Label"></asp:Label>
                                </td>
                                <td>Fecha</td>
                                <td align="left"  class="myLabelIzquierda">
                                    <asp:Label ID="lblFecha1" runat="server" Font-Bold="True" Font-Size="9pt" Text="Label"></asp:Label>
                                </td>
                            </tr>
						
					
					
					       
                            <tr>
                                <td class="auto-style4" >Muestra:</td>
                                <td align="left" class="auto-style3" >
                                    <asp:Label ID="lblConservacion" runat="server" CssClass="myLabelIzquierda" Font-Bold="True" Font-Size="9pt" Text="Label"></asp:Label>
                                </td>
                                <td >  <asp:Label ID="lblTituloFechaTomaMuestra" runat="server" Text="F. das Muestra"  CssClass="myLabelIzquierda"></asp:Label></td>
                                <td align="left" class="auto-style1">
                                    <asp:Label ID="lblFechaTomaMuestra1" runat="server" CssClass="myLabelIzquierda" Text="Label"></asp:Label>
                                </td>
                            </tr>
						
					
					
					        <tr>
                                <td class="auto-style4">Nro. de Origen:</td>
                                <td align="left" class="auto-style3">
                                    <asp:Label ID="lblNumeroOrigen1" runat="server" CssClass="myLabelIzquierda" Text="Label"></asp:Label>
                                </td>
                                <td>Usuario</td>
                                <td align="left" class="auto-style1">
                                    <asp:Label ID="lblUsuario1" runat="server" CssClass="myLabelIzquierda" Text="Label"></asp:Label>
                                </td>
                            </tr>
						
					
					
					<tr>
						<td class="auto-style4" >
                            Solicitante-Sector:
                            </td>
						<td class="auto-style3" 
                            >
                            <asp:Label ID="lblSolicitante2" runat="server" CssClass="myLabelIzquierda"  Text=""></asp:Label>
                            &nbsp;<asp:Label ID="lblSector1" runat="server" CssClass="myLabelIzquierda" Text=""></asp:Label>
                        </td>
                      	<td >
                            Fecha de Registro:</td>
                        <td   align="left" class="auto-style1">   
                                   <asp:Label ID="lblFechaRegistro1" runat="server" CssClass="myLabelIzquierda" Text="Label"></asp:Label>
                                   <asp:Label ID="lblHoraRegistro1" runat="server" CssClass="myLabelIzquierda" Text="Label"></asp:Label>
                                       
                                    </td>
					</tr>
						
					
					
					<tr>
						<td class="auto-style4" >
                            Observaciones Internas:</td>
						<td   
                            align="left" class="auto-style3"  >
                            <asp:Label ID="lblObservacion1" runat="server" CssClass="mytituloRojo" Text="Label"></asp:Label>
                            </td>
                    	<td > &nbsp;</td>
                        <td    align="left" class="auto-style1"> 
                            &nbsp;</td>
					</tr>
						
					
					
					</table>
      </div>
                                    <span id="spanadjunto" runat="server" class="label label-danger">
                            <asp:Label ID="lblAdjunto" runat="server" Text="El protocolo tiene archivos adjuntos"></asp:Label>

                                     
                            </span>

                                
                               </div>
                                </asp:Panel>
						</td>
						</tr>
				
					<tr>
						
						<td    colspan="3">  
						
						    
<asp:HiddenField runat="server" ID="HFCurrTabIndex" /> 

						    
<asp:HiddenField runat="server" ID="HFIdItem" /> 
<asp:HiddenField runat="server" ID="HFIdGermen" /> 
<asp:HiddenField runat="server" ID="HFIdMetodo" /> 
<asp:HiddenField runat="server" ID="HFIdProtocolo" /> 
<asp:HiddenField runat="server" ID="HFNumeroAislamiento" /> 
                            <asp:HiddenField runat="server" ID="HFOperacion" /> 

                           
                            <div id="tabContainer">  
                             <asp:Panel ID="pnlResultados" runat="server" > 
                             <ul>
    <li><a href="#tab1"><b>Análisis</b></a></li>  
    <%--<li id="tituloCalidad" runat="server"><a  href="#tab6">Incidencia<img alt="tiene incidencias" runat="server" id="inci" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></a></li>--%>
    <li id="tituloMicroOrganismo" runat="server"><a href="#tab5"><b>Aislamientos&nbsp;<img alt="tiene antibiogramas" runat="server" id="aisl" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></b></a></li>          
    <li id="tituloAntibiograma" runat="server"><a href="#tab4"><b>Antibiogramas&nbsp;<img alt="tiene antibiogramas" runat="server" id="anti" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></b></a></li>          
    <li id="tituloObservaciones" runat="server"><a href="#tab2" >Observaciones</a></li>
      <li id="tituloAntecedente" runat="server"><a href="#tab3" id="tabAntecedente" runat="server">Más opciones</a></li>
    
                                                
    
  
</ul>                          


  
                            </asp:Panel>
                            
                            <asp:Panel ID="pnlHC" runat="server" > 
                      <%--     <ul>
    <li><a href="#tab1">Resultados</a></li>    
  <%--    <li><a href="#tab2">Diagnósticos</a></li>      
  
</ul>                       --%>           
                            </asp:Panel>
						 

        <div onkeydown="enterToTab(event)"  id="tab1" >   
        
        <table width="90%">
        	<tr>						
						<td>   <asp:Button ID="btnMostrarResultados" runat="server"
                                Text="Mostrar Resultados Anteriores"  AccessKey="M"
                                onclick="btnMostrarResultados_Click" Width="250px" TabIndex="600" />
                            <br />
                            <asp:Label ID="lblSinResultados" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                              <a runat="server" id="tieneInci" href="#tab6">Tiene Incidencia<img alt="tiene incidencias" runat="server" id="inci" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></a>   
                            <asp:HyperLink ID="hplPesquisa" Visible="false" CssClass="myLink2" runat="server">Ver Tarjeta</asp:HyperLink>
                            
                            <asp:Image ID="imgPesquisa" Visible="false" runat="server" ImageUrl="~/App_Themes/default/images/pendiente.png" />                                       				
                            <asp:Label CssClass="myLabelIzquierdaGde" ID="lblMuestra" runat="server" ></asp:Label>            
                            <asp:CustomValidator ID="cvValidaControles" runat="server"  ErrorMessage="CustomValidator" Font-Size="9pt" onservervalidate="cvValidaControles_ServerValidate"   ValidationGroup="0" Font-Bold="True"></asp:CustomValidator> &nbsp; &nbsp;
                            <asp:Button ID="btnAceptarValorFueraLimite" Width="220px" CssClass="myButtonRojo"  onclick="btnAceptarValorFueraLimite_Click" runat="server" Text="Aceptar valor fuera de límite" Visible="false" />  
                            <asp:Label ID="lblIdValorFueraLimite" Visible="false" runat="server" Text="0"></asp:Label>
                            <asp:Label ID="lblIdValorFueraLimite1" Visible="false" runat="server" Text="0"></asp:Label> 
                                                      
                        </td>
						
						<td align="right" > 
                       
                            
                                                                 
                       
                            
                       <asp:Button ID="btnActualizarPracticasCarga" Visible="false" ToolTip="" runat="server" Text="Editar Practicas"  Width="150px"   
                       OnClientClick="ProtocoloEditCarga(); return false;" />
                       
                       <INPUT TYPE="button" accesskey="m"  title="Alt+Shift+M"  runat="server" name="marcar" id="lnkMarcar" value="Marcar todos" onClick="seleccionar_todo()" style="font-size: 11px; color: #333333; text-decoration: underline; font-family: Arial, Helvetica, sans-serif; font-weight: bold;" class="myLabelIzquierda">
                       <INPUT TYPE="button" accesskey="z" title="Alt+Shift+Z" runat="server" name="marcar" id="lnkDesmarcar"  value="Desmarcar todos" onClick="desmarcar_todo()" style="font-size: 11px; color: #333333; text-decoration: underline; font-family: Arial, Helvetica, sans-serif; font-weight: bold;" class="myLabelIzquierda">
						     
                                      </td>
						
					</tr>
        	<tr>
						
						<td  colspan="2" style="vertical-align: top">   
                          <div id="imprimir">    

						     <asp:Panel ID="Panel1" runat="server" > 
                                                                                                                                                                						 
                                               <asp:Table ID="tContenido"   enableviewstate="true" CssClass="CeldaContenedor"
                                                   Runat="server" CellPadding="0" CellSpacing="0"  
                                                   Width="100%" GridLines="Both" BorderColor="#CCCCCC"
                                                   ></asp:Table>                                                                                                                                     
                                     
                                           </asp:Panel>
                           </div>

                                <asp:Label ID="lblObservacionResultado" runat="server" Font-Names="Courier New" 
                                Font-Size="9pt" ForeColor="Black" Visible="False"></asp:Label>
                          
                        
						</td>
						
					</tr>
            <%--   <br />

                                        <asp:CheckBox ID="chkInfo" runat="server" Text="Info Adicional"  />--%>
                                                                    
                                                                    
                                                                    
                                                                    <tr>
                                                                      <td colspan="2"  align="right" style="vertical-align: top">
                                                                 
                                                                    <asp:Button ID="btnAplicarFormula" runat="server" CssClass="btn btn-danger"
                                Text="F(x) Calcular Fórmulas"  AccessKey="F" ToolTip="Alt+Shift+F"
                                onclick="btnAplicarFormula_Click" Visible="False" Width="200px" TabIndex="600" /> <br />
                            <asp:CheckBox ID="chkFormula" runat="server" Checked="True" CssClass="myLabel" 
                                Text="Calcular fórmulas al guardar" Visible="False" 
                                ToolTip="Recalcula y sobreescribe formulas al guardar" TabIndex="600" />
                            <asp:Label ID="lblFormula" runat="server" Font-Bold="True" ForeColor="Red" Text="F(x)" 
                                Visible="False"></asp:Label>
                    </td>
                        
                    
                                                                  
                                                                    
                     
                                
                              
                        
					</tr>
					
			
        		
					
        	<tr>
						
						<td class="myLabel" style="vertical-align: top" colspan="2"> 
                           <hr /></td>
						
					</tr>
        	<tr>
						
						<td align="left"> 
                            
                            <asp:Panel ID="pnlReferencia" Visible="false" runat="server" Width="300px">
                           <span class="label label-default">Dentro de V.R</span>
                                <span class="label label-danger">Fuera de V.R</span>
                            </asp:Panel> 
                        
                            <asp:CheckBox ID="chkCerrarSinResultados" runat="server" CssClass="myLabelIzquierda" 
                                Text="Terminar protocolo" Visible="False" 
                                ToolTip="Da por terminado el protocolo con analisis sin resultados" />
                        
						<asp:CheckBox ID="chkWhonet" runat="server" CssClass="myLabelIzquierda" 
                                Text="Informa Whonet" Visible="False" 
                                ToolTip="Agrega el protocolo en el listado para Whonet." />
                        
						<asp:RadioButtonList ID="rdbImprimir" RepeatDirection="Horizontal" runat="server" CssClass="myLabel" 
                                Font-Names="Arial" Font-Size="8pt">
                                <asp:ListItem Selected="True" Value="0">Imprimir sólo seleccionados</asp:ListItem>
                                <asp:ListItem Value="1">Imprimir todos validados</asp:ListItem>
                            </asp:RadioButtonList>
                         
						</td>
						<td align="right" style="vertical-align: top" colspan="1"> 
                          
                           <asp:Button ID="btnValidarImprimir" AccessKey="I" runat="server" CssClass="btn btn-primary" ToolTip="Alt+Shift+I:Validar e Imprimir en impresora seleccionada" Text="Validar + Imprimir" 
                                onclick="btnValidarImprimir_Click" ValidationGroup="0" Visible="False" 
                                Width="180px" />
                                                  &nbsp;&nbsp;
                          <asp:Button ID="btnGuardar" AccessKey="s" Width="100px" runat="server" CssClass="btn btn-primary" Text="Guardar" 
                                onclick="btnGuardar_Click" ValidationGroup="0" TabIndex="600" ToolTip="Alt+Shift+S: Guarda resultados" />  
                          
                            </td>
						
					</tr>
						<tr>
						
						<td class="myLabel" style="vertical-align: top" colspan="2"> 
                           <hr /></td>
						
					</tr>
        	<tr>
						
						<td align="left" colspan="2" > 
                             
                                 
     <div class="footer" align="right">	
  
                           <asp:Button ID="btnRestringirAcceso" AccessKey="N" runat="server" CssClass="btn btn-danger" Text="No Publicar"  Visible="false" OnClientClick="ProtocoloPermisos();return false;" 
                                Width="130px" OnClick="btnRestringirAcceso_Click"/>
                           <%--    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />--%>
             <asp:Button ID="btnDeshacerRestringirAcceso" AccessKey="U" runat="server" CssClass="btn btn-warning" Text="Publicar" 
                                 ValidationGroup="0" Visible="false" OnClientClick="return PreguntoConfirmarPublicar();"
                                Width="100px" OnClick="btnDeshacerRestringirAcceso_Click"   />
         
            
                                 <asp:Button AccessKey="D" ID="btnDesValidar" Visible="false" runat="server" class="btn btn-warning" Text="Desvalidar"   OnClientClick="return PreguntoConfirmar();"
                                onclick="btnDesValidar_Click" ValidationGroup="0" TabIndex="600"  Width="100px" ToolTip="Alt+Shift+D:Desvalida lo validado por el usuario actual"/>
                      
                            
                            <asp:Button ID="btnValidarPendienteImprimir" AccessKey="L" runat="server" ToolTip="Alt+Shift+L:Validar pendiente de validar e Imprimir en impresora seleccionada" 
                                class="btn btn-info" Text="Validar Pend. + I " 
                                onclick="btnValidarPendienteImprimir_Click" Visible="false" ValidationGroup="0" Width="150px" TabIndex="600" /> 
                                   <asp:Button ID="btnValidarPendiente"  AccessKey="P" runat="server" 
                                       class="btn btn-info" Text="Validar pendiente"  ToolTip="Alt+Shift+P:Validar solo lo pendiente"
                                onclick="btnValidarPendiente_Click" Visible="false" ValidationGroup="0" Width="150px" TabIndex="600" />  

                                 &nbsp; 
           <asp:LinkButton ID="imgPdf" runat="server" CssClass="btn btn-info" ForeColor="White" Text="Buscar" OnClick="imgPdf_Click" Width="140px" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar PDF</asp:LinkButton>

           <asp:LinkButton ID="imgAdjunto" runat="server" CssClass="btn btn-info" Text="Adjuntar Archivos" ForeColor="White" Width="165px" 
               OnClientClick="AdjuntoProtocoloEdit(); return false;"  Visible="False"></asp:LinkButton>

          <%--   <br />

                                        <asp:CheckBox ID="chkInfo" runat="server" Text="Info Adicional"  />--%><%--    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />--%>


                           
      </div>
                                
                            </td>
						
						
						
					</tr>
        	<tr>
						
						<td align="left" colspan="2" > 
                             
                           </td>
						
						
						
					</tr>
        	</table>
                                          
                                           </div>
                                               
                                             
          <div id="tab5" >
            <asp:Panel  ID="pnlMicroOrganismo" runat="server" Height="500px" >    

             
           <asp:DropDownList class="form-control input-sm" ID="ddlPracticaAislamiento"  Width="350px" runat="server"> </asp:DropDownList> 
                   <asp:RangeValidator ID="rvPracticaAislamiento" ControlToValidate="ddlPracticaAislamiento" MinimumValue="1" MaximumValue="999999" ValidationGroup="AIS" runat="server" ErrorMessage="*"></asp:RangeValidator>
             
          <table width="850">           
                <tr>
                <td style="vertical-align: top" height="100%" width="400px">
                
                 
                </td>
                </tr>

                 
                <tr>
                <td style="vertical-align: top">
              Microorganismo: &nbsp; &nbsp; <anthem:TextBox ID="txtCodigoMicroorganismo" runat="server" class="form-control input-sm"
                        ontextchanged="txtCodigoMicroorganismo_TextChanged" Width="60px" AutoCallBack="True"  TabIndex="1" 
                        ToolTip="Ingrese el codigo de microorganismo"></anthem:TextBox>

                    <anthem:DropDownList class="form-control input-sm" Width="400px" ID="ddlAislamiento" runat="server"></anthem:DropDownList>
                    
                       <asp:RangeValidator ID="rvAislamiento" ControlToValidate="ddlAislamiento" MinimumValue="1" MaximumValue="999999" ValidationGroup="AIS" runat="server" ErrorMessage="*" Type="Integer"></asp:RangeValidator>
                         <asp:Button  CssClass="btn btn-danger" Width="80px" ID="btnAgregarGermen" ValidationGroup="AIS"  runat="server" Text="Agregar" onclick="btnAgregarGermen_Click"/>
                                                                    </td>
                </tr>
                
               
                <tr>
                <td>
                   </td>
                </tr>
                
                <tr>
                <td align="right">
                   <asp:LinkButton ID="lnkMarcarAislamiento" runat="server" CssClass="myLittleLink" onclick="lnkMarcarAislamiento_Click" >Marcar todas</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkDesMarcarAislamiento" runat="server" CssClass="myLittleLink" onclick="lnkDesMarcarAislamiento_Click" >Desmarcar</asp:LinkButton></td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvAislamientos" runat="server" AutoGenerateColumns="False" CellPadding="1" 
                            CssClass="table table-bordered bs-table" 
                            DataKeyNames="idProtocoloGermen"  onrowcommand="gvAislamientos_RowCommand" onrowdatabound="gvAislamientos_RowDataBound1" Width="100%">
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <Columns>
                                <asp:BoundField DataField="item" HeaderText="" />
                                <asp:BoundField DataField="numeroAislamiento" HeaderText="Nro.Cepa" />
                                <asp:BoundField DataField="germen" HeaderText="Aislamiento" />
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAtb" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "atb") %>' Text="ATB" />
                               <%--   <br />

                                        <asp:CheckBox ID="chkInfo" runat="server" Text="Info Adicional"  />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Observaciones">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtObservacionesAislamiento" runat="server" TextMode="MultiLine" class="form-control input-sm"  Text='<%# DataBinder.Eval(Container.DataItem, "observaciones") %>' Width="400px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valida">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkValida" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="" />
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" ImageUrl="~/App_Themes/default/images/eliminar.jpg" OnClientClick="return PreguntoEliminar();" />
                                    </ItemTemplate>
                                    <ItemStyle Height="18px" HorizontalAlign="Center" Width="20px" />
                                </asp:TemplateField>
                            </Columns>
                        <%--    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />--%>
                            <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                            <EditRowStyle BackColor="#999999" />
                        </asp:GridView>
                        <hr />
                        <asp:Button ID="btnGuardarAislamientos" runat="server"  CssClass="btn btn-primary" Width="100px" onclick="btnGuardarAislamiento_Click" Text="Guardar" />
                    </td>
                </tr>
           </table>
           </asp:Panel>
               <asp:Panel ID="pnlMicroorganismoHC" runat="server" Visible="False" >
                   <asp:GridView ID="gvAislamientosHC" runat="server" CellPadding="4" 
                        EmptyDataText="No se encontraron antibiogramas para el protocolo" 
                        ForeColor="Black" 
                        Font-Names="Arial" Font-Size="9pt" 
                       ShowFooter="false" BorderColor="#999999" BorderStyle="Solid" Font-Bold="True" 
                                UseAccessibleHeader="False" Width="100%" OnRowDataBound="gvAislamientosHC_RowDataBound" DataKeyNames="idProtocoloGermen">
                       <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                       <RowStyle BackColor="White" ForeColor="Black" Font-Bold="False" />
                       <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                       <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                       <HeaderStyle BackColor="#DADADA" Font-Bold="True" ForeColor="Black" />
                       <EditRowStyle BackColor="#999999" />
                       <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                   </asp:GridView>
              </asp:Panel>
          
          </div>    
            
       <div id="tab4" > 
                <asp:Panel ID="pnlAntibiograma" Height="650px" runat="server" >   
               
                &nbsp;&nbsp;<asp:DropDownList class="form-control input-sm" ID="ddlPracticaAtb" Width="250px" runat="server" AutoPostBack="true"  onselectedindexchanged="ddlPracticaAtb_SelectedIndexChanged"> </asp:DropDownList>  
          
               <br />
           <table width="850px">           
                <tr>
                <td style="vertical-align: top" height="100%"  rowspan="2" width="400px">
                
            
                <div align="left" style="width: 380px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">NUEVO ANTIBIOGRAMA</h3>
                        </div>

				<div class="panel-body">	
                     <asp:DropDownList class="form-control input-sm"  ID="ddlGermen" runat="server" Width="250px">  </asp:DropDownList>
               &nbsp;<asp:RangeValidator ID="rvGermen" runat="server" ControlToValidate="ddlGermen" 
                        ErrorMessage="*" MaximumValue="9999999" MinimumValue="1" Type="Integer" 
                        ValidationGroup="A"></asp:RangeValidator>

                  <asp:RadioButtonList CssClass="myLabelIzquierda"   onselectedindexchanged="rdbMetodologiaAntibiograma_SelectedIndexChanged"   Width="180px" ID="rdbMetodologiaAntibiograma" RepeatDirection="Horizontal"  runat="server">
                     <asp:ListItem Selected="True" Value="0">Disco</asp:ListItem>
                                <asp:ListItem Value="1">CIM</asp:ListItem>
                                <asp:ListItem Value="2">Etest</asp:ListItem>
                    </asp:RadioButtonList>
           
                   <asp:DropDownList class="form-control input-sm"  onselectedindexchanged="ddlPerfilAntibiotico_SelectedIndexChanged" AutoPostBack="true"   ID="ddlPerfilAntibiotico" runat="server" Width="250px">  </asp:DropDownList>
                   <hr />
<div  style="width:330px;height:250pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                    <asp:GridView ID="gvAntiobiograma" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="idAntibiotico"            CssClass="table table-bordered bs-table"               CellPadding="1"  Width="310px"                          >
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="White" ForeColor="#333333" Font-Bold="False" Font-Names="Verdana" Font-Size="8pt" />
        <Columns>

<asp:BoundField HeaderText="Antibiótico" DataField="descripcion"  />
<asp:TemplateField HeaderText="Inter.">
    <ItemTemplate>

        <asp:DropDownList ID="ddlAntibiotico" runat="server" Font-Size="10" class="form-control input-sm">
        <asp:ListItem Value="" ></asp:ListItem>
         <asp:ListItem Value="Resistente">R</asp:ListItem>
         <asp:ListItem Value="Intermedio">I</asp:ListItem>
         <asp:ListItem Value="Sensible">S</asp:ListItem>
         <asp:ListItem Value="Sensibilidad Disminuida">SD</asp:ListItem>
         <asp:ListItem Value="Apto para Sinergia">AS</asp:ListItem>
         <asp:ListItem Value="Sin Reactivo">SR</asp:ListItem>
        </asp:DropDownList>
    </ItemTemplate>

</asp:TemplateField>
<asp:TemplateField HeaderText="Valor">
    <ItemTemplate >
        <asp:TextBox class="form-control input-sm" ID="txtHalo" Width="60px"  runat="server"></asp:TextBox>
    </ItemTemplate>
</asp:TemplateField>

</Columns>
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                    <EditRowStyle BackColor="#999999" />
                   

</asp:GridView>
</div>


<div>
<hr />                    
<%--<b class="myLabelRojo">Referencias:</b> R: Resistente&nbsp;&nbsp;I:Intermedio&nbsp;&nbsp;S:Sensible&nbsp;&nbsp;<br />SD: Sensibilidad Disminuida&nbsp;&nbsp;AS: Apto para Sinergia&nbsp;&nbsp;<br />SR: Sin Reactivo--%>



</div>
<asp:Button ID="btnAgregarAntibiograma" runat="server" Text="Guardar" 
                        onclick="btnAgregarAntibiograma_Click"  CssClass="btn btn-primary" Width="80px"
                        ValidationGroup="A" />
                       
                    &nbsp;<asp:Button ID="btnAgregarAntibiogramaValidado" runat="server"  
                        CssClass="btn btn-primary" Width="150px" onclick="btnAgregarAntibiogramaValidado_Click" Text="Guardar y Validar" ValidationGroup="A" Visible="False"   />
                       
                    </div>
                  </div>
                    </div>
                </td>
                <td style="vertical-align: top">&nbsp;</td>
                <td style="vertical-align: top" height="100%" rowspan="2">
                
                
               
                   <div align="left" style="width: 450px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"> ANTIBIOGRAMAS ASOCIADOS</h3>
                        </div>

				<div class="panel-body">
                              
                                   ATB:
                                        <asp:DropDownList AutoPostBack="true" Width="100px" onselectedindexchanged="ddlMetodoAntibiograma_SelectedIndexChanged" ID="ddlMetodoAntibiograma" runat="server" class="form-control input-sm"> 
         <asp:ListItem Selected="True" Value="0">Disco</asp:ListItem>
         <asp:ListItem Value="1">CIM</asp:ListItem>
         <asp:ListItem Value="2">Etest</asp:ListItem>         
        </asp:DropDownList>
                        
                        <asp:DropDownList Width="250px" ID="ddlAntibiograma" runat="server" AutoPostBack="true"  onselectedindexchanged="ddlAntibiograma_SelectedIndexChanged" class="form-control input-sm">
                        </asp:DropDownList>
                        <asp:RangeValidator ID="rvAntibiograma" runat="server" CssClass="myList" 
                            ControlToValidate="ddlAntibiograma" ErrorMessage="*" MaximumValue="9999999" 
                            MinimumValue="1" Type="Integer" Enabled="false" ValidationGroup="EA"></asp:RangeValidator>

                    <br />
               

                                                    <asp:Button ID="btnEliminarAntibiograma" runat="server" CssClass="btn btn-danger" Enabled="false" onclick="btnEliminarAntibiograma_Click" OnClientClick="return PreguntoEliminar();" Text="Eliminar" ToolTip="Eliminar ATB" ValidationGroup="EA" Width="80px" />
                                                    <asp:Button ID="btnEditarAntibiograma" runat="server" CssClass="btn btn-success" Enabled="false" onclick="btnEditarAntibiograma_Click" OnClientClick="editarATB(); return false;" Text="Modificar" ToolTip="Modificar ATB" Width="120px" />
                                                    <%--<asp:ImageButton ID="ImageButton1" ToolTip="Actualizar vista de antibiogramas" runat="server" ImageUrl="~/App_Themes/default/images/actualizar.gif"  onclick="btnActualizarATB_Click"/>--%><%-- <asp:Button ID="btnActualizarATB" runat="server" Visible="true"  onclick="btnActualizarATB_Click" Text="Actualizar" CssClass="myButtonGris" Width="120px"/>--%>
                                                    <asp:Button ID="btnValidarAntibiograma" runat="server" CssClass="btn btn-success" onclick="btnEditarAntibiograma_Click" OnClientClick="validarATB(); return false;" Text="Modificar/Validar" ToolTip="Validar ATB" Visible="false" Width="120px" />
                                                    <asp:Button ID="btnActualizarAntibiograma" runat="server" CssClass="btn btn-success" onclick="btnActualizarAntibiograma_Click" Text="Actualizar" ToolTip="Actualizar ATB" Width="80px" />
                                              

                            
                       
                
                   
                   
                  <hr />                                                   
                  <div  style="width:500px;height:255pt;overflow:scroll;border:1px solid #CCCCCC;"> 
                    <asp:GridView ID="gvAntiBiograma2" runat="server" CellPadding="2"    CssClass="table table-bordered bs-table"    
                        EmptyDataText="No se encontraron antibiogramas para el protocolo" 
                        ForeColor="#333333"  
                        Font-Names="Arial" Font-Size="9pt" OnRowDataBound="gvAntiBiograma2_RowDataBound">
                      <%--  <FooterStyle  BackColor="White" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="White" ForeColor="#333333" Font-Bold="False" Font-Names="Verdana" Font-Size="8pt" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />--%>
                        <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" Height="40px" />
                       <%-- <EditRowStyle BackColor="#999999" />--%>

                    </asp:GridView>
                    </div>
                    
                  <hr />
                
   <asp:Button ID="btnValidarTodosAntibiogramas" runat="server"   OnClientClick="return PreguntoValidar();"  onclick="btnValidarTodosAntibiogramas_Click" Text="Validar todo" ToolTip="Validar todos los ATB" Visible="false" CssClass="btn btn-primary"  Width="125px" />
                    &nbsp;<asp:Button ID="btnValidarTodosAntibiogramasPendientes" runat="server"  OnClientClick="return PreguntoValidar();"  onclick="btnValidarTodosAntibiogramasPendientes_Click" Text="Validar sólo pendientes" ToolTip="Validar todos los ATB (sólo pendiente)" Visible="false" CssClass="btn btn-primary"    Width="195px" />               
                   </div>
       </div>
                    </div>                                          
                                                                    </td>
                </tr>
             
                
                 
           </table>
           </asp:Panel>               <asp:Panel ID="pnlAntibiogramaHC" runat="server" >    
                  <asp:GridView ID="gvAntiBiogramaHC" runat="server" CellPadding="4" 
                        EmptyDataText="No se encontraron antibiogramas para el protocolo" 
                        ForeColor="Black" 
                        Font-Names="Arial" Font-Size="9pt" 
                       ShowFooter="false" BorderColor="#999999" BorderStyle="Solid" Font-Bold="True" 
                                UseAccessibleHeader="False" Width="100%" OnRowDataBound="gvAntiBiogramaHC_RowDataBound">
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="White" ForeColor="Black" Font-Bold="False" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#DADADA" Font-Bold="True" ForeColor="Black" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                    </asp:GridView>
                </asp:Panel>
           
       </div>
       
          
         
            <div  id="tab2">   
          
              <asp:Panel ID="pnlObservacionProtocolo" Height="500px" runat="server" >  
                <table style="width: 850px;">
                    <tr>
                        <td class="myLabelIzquierda">

                            Observaciones codificadas: &nbsp;
						    <anthem:DropDownList class="form-control input-sm" ID="ddlObsCodificadaGeneral" runat="server" Width="350px">
                               </anthem:DropDownList>
                               <anthem:Button  CssClass="btn btn-primary"  OnClick="btnAgregarObsCodificadaGral_Click"  ID="btnAgregar"  Width="100px" runat="server" Text="Agregar" />
                               <br />
                           <anthem:TextBox class="form-control input-sm" ID="txtObservacion" runat="server" TextMode="MultiLine" 
                              Width="95%" MaxLength="4000" Height="400px"></anthem:TextBox>  
                              

                        </td>
                    </tr>
                    <tr>
                        <td><hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
      
  
                            <anthem:Button  CssClass="btn btn-primary"  OnClick="btnBorrarObsCodificadaGral_Click" ID="btnBorrarObservaciones" runat="server" Text="Borrar"  Width="100px" /> &nbsp;
                        
                            <asp:Button  CssClass="btn btn-primary"  OnClick="btnGuardarObsCodificadaGral_Click" ID="btnGuardarObservaciones" runat="server" Text="Guardar"  Width="100px" />  &nbsp;
                     
                          <%--  <anthem:Button  ID="btnValidarObservaciones" runat="server" Text="Guardar y Validar" />--%>  &nbsp;
                        </td>
                    </tr>
                    
                </table>
                
                </asp:Panel>
                            
                                                                    <br />         
                                               
                                               </div>
         
                                               <div  id="tab3" >

                                               <asp:Panel runat="server" Height="400px" id="pnlAntecedentes" >


                                                
                          <table width="850px">

                                  <tr>
                                                           <td >
                                                          Incidencias de Calidad
                                                          
                                                           </td>
                                                                                                                 </tr>

                                <tr>
                                                          <td >
                                                            
                                                               <asp:Button ID="btnIncidencia"  CssClass="btn btn-primary" ToolTip="" runat="server" Text="Incidencias"  Width="150px" 
                       OnClientClick="IncidenciaEdit(); return false;" />
<small>                                                               Gestione desde acá las incidencias detectadas del protocolo. </small>
                                                             <hr />
                                                            
                                                           </td>
                                                       </tr>


                        

                                                       <tr>
                                                           <td  >
                                                           Editar Prácticas
                                                           </td>
                                                         
                                                       </tr>
                                                       <tr>
                                                          <td >
                                                            <asp:Button ID="btnActualizarPracticas"  CssClass="btn btn-primary" ToolTip="" runat="server" Text="Editar Practicas"  Width="150px" 
                       OnClientClick="ProtocoloEdit(); return false;" />
                                                          <small>     Modifique desde acá las practicas solicitadas.</small>
                                                                <hr />                   
                                                            
                                                           </td>
                                                           
                                                       </tr>
                                                       <tr>
                                                           <td   colspan="3">
                                                               <div id="divAntecedentes" runat="server">
                                                                   <table>
                                                                       <tr>
                                                                           <td>Antecedentes 

                                                                           </td>
                                                                       </tr>
                                                                       <tr>
                                                           <td>
                                                               <asp:Label ID="lblAntecedentes" Visible="false"   runat="server" Text=""></asp:Label>                                                                                                                          
                                                                <asp:Button ID="btnAntecedente" runat="server" CssClass="btn btn-primary" OnClientClick="AntecedenteView(); return false;" Text="Ver Antecedentes" ToolTip="Antecedentes del Paciente" Width="150px" /> 
                                                               <small>Consulte los antecedentes por determinacion para el paciente actual</small>
                                                               <hr />
                                                                              
                                                               </td>
                                                                           
                                                       </tr>

                                                                           <tr>
                                                           <td  >
                                                           Auditoría
                                                           </td>
                                                           
                                                       </tr>
                                                       <tr>
                                                          <td >
                                                            <asp:Button ID="Button4"  CssClass="btn btn-primary" ToolTip="Auditoría del Protocolo" runat="server" Text="Ver Auditoria"  Width="150px" 
                       OnClientClick="AuditoriaView(); return false;" />
                                                        <small>      Desde acá puede ver la trazabilidad (los movimientos realizados por lo usuarios) del protocolo.</small>
                                                          <hr />
                                                             
                       
                                                           </td>
                                                           
                                                       </tr>
                                                                       
                                                                   </table>
                                                               </div>
                                                             
                                                             </td>
                                                       </tr>
                                                       
                                                          
                                                       
                                                   
                                                   </table>
                                          

                                               </asp:Panel> 
                                               
                                               </div>
                                               
                <%--         <div id="tab6" class="tab_content" style="height: 500px; overflow:scroll;overflow-x:hidden; ">
                                     <table width="850px">
                             <tr>
                                                           <td>    
                                    <uc1:IncidenciaEdit ID="IncidenciaEdit1" runat="server" />
                             <asp:Button ID="btnGuardarIncidencia" runat="server" Text="Guardar" onclick="btnGuardarIncidencia_Click" CssClass="btn-primary" Width="100px" />
                             <asp:Button ID="btnEliminarIncidencia" runat="server" Text="Borrar" onclick="btnEliminarIncidencia_Click" CssClass="btn-primary" Width="100px"/>
                                    </td>
                                 </tr>
                                         </table>
                                </div>          --%>             
                                               
</div>
</td>
						
					</tr>
					<tr>
						
						<td   colspan="3" align="right">
                           </td>
						
					</tr>
					
					<tr>
						
						<td   colspan="3" align="right">
                         <%--   <asp:Panel ID="pnlImpresora" Visible="false" runat="server" style="border:1px solid #C0C0C0; width:100%; background-color: #EFEFEF;">                                                      
 <table width="720px" align="center">
 	<tr>
						<td class="myLabelIzquierda" align="left" style="width: 140px; background-color: #EFEFEF;">
                                        Impresora del sistema: </td>
						<td  align="left">
                                        <asp:DropDownList ID="ddlImpresora" runat="server" class="form-control input-sm">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imgImprimir" runat="server" ImageUrl="~/App_Themes/default/images/imprimir.jpg" onclick="imgImprimir_Click" />
                            </td>
						
                                        </tr>
                                        </table>
														
 </asp:Panel>--%>
                         </td>
						
					</tr>
					        </table>
                            </td>
                            <td valign="top"></td>
						
					</tr>
											
					
					
					
						
						</table>						
 </div>

 <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
  
    function seleccionar_todo() {//Funcion que permite seleccionar todos los checkbox     
        form = document.forms[0];
        
        for (i = 0; i < form.elements.length; i++) {
            if (form.elements[i].type == "checkbox") 
            {
                if (form.elements[i].name.indexOf("TreeView2") == -1)
                    if (form.elements[i].name.indexOf("chkCerrarSinResultados") == -1)
                        if (form.elements[i].name.indexOf("chkWhonet") == -1)
                        
                 if (form.elements[i].name.indexOf("F")==-1) 
                    form.elements[i].checked = 1;                                       
            }
        }
    }

    function desmarcar_todo() {//Funcion que permite seleccionar todos los checkbox
        form = document.forms[0];
        for (i = 0; i < form.elements.length; i++) {
            if (form.elements[i].type == "checkbox") {
                if (form.elements[i].name.indexOf("TreeView2") == -1)
                    if (form.elements[i].name.indexOf("chkCerrarSinResultados") == -1)
                        if (form.elements[i].name.indexOf("chkWhonet") == -1)
                    if (form.elements[i].name.indexOf("F")==-1) 
                        form.elements[i].checked = 0;
            }
        }
    } 


    var iditem = $("#<%= HFIdItem.ClientID %>").val();
    var idProtocolo = $("#<%= HFIdProtocolo.ClientID %>").val();
    var idGermen = $("#<%= HFIdGermen.ClientID %>").val();

    var idMetodo = $("#<%= HFIdMetodo.ClientID %>").val();
    var numeroAislamiento = $("#<%= HFNumeroAislamiento.ClientID %>").val();
    var operacion = $("#<%= HFOperacion.ClientID %>").val();
    
 
    function editarATB() {
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
        $('<iframe src="ATBEdit.aspx?Operacion='+operacion+'&idItem=' + iditem + '&idMetodo='+idMetodo+'&numeroAislamiento='+ numeroAislamiento +'&idGermen='+ idGermen +'&idProtocolo='+idProtocolo+ '" />').dialog({
            title: 'Modificar ATB',
            autoOpen: true,
            width: 450,
            height: 500,
            modal: false,
            resizable: false,
            autoResize: true,
         <%--   open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnEditarAntibiograma))%>; }               
            },--%>
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(480);
    }


    function validarATB() {
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
        $('<iframe src="ATBValida.aspx?idItem=' + iditem + '&idMetodo=' + idMetodo + '&numeroAislamiento=' + numeroAislamiento + '&idGermen=' + idGermen + '&idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Validar ATB',
            autoOpen: true,
            width: 650,
            height: 560,
            modal: true,
            resizable: false,
            autoResize: false,
            //open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },
          <%--  buttons: {
                'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnEditarAntibiograma))%>; }
            },--%>
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(680);
}


    
    function IncidenciaEdit() {
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
        $('<iframe src="IncidenciaProtocolo.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Incidencias de Calidad del Protocolo',
            autoOpen: true,
            width: 750,
            height: 490,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(770);
    }
    function AntecedenteView() {
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
        $('<iframe src="AntecedentesView2.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Antecedentes del Paciente',
            autoOpen: true,
            width:750,
            height: 490,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(770);
    }

      function ObservacionEdit(idDetalle,idTipoServicio,operacion) {
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
        $('<iframe src="ObservacionesEdit.aspx?idDetalleProtocolo=' + idDetalle + '&idTipoServicio='+idTipoServicio+'&Operacion='+operacion+'" />').dialog({
            title: 'Observaciones',
            autoOpen: true,
            width: 500,
            height: 440,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(510);
      }

     function AdjuntoEdit(idDetalle,idTipoServicio,operacion) {
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
        $('<iframe src="AdjuntoEdit.aspx?idDetalleProtocolo=' + idDetalle + '&idTipoServicio='+idTipoServicio+'&Operacion='+operacion+'" />').dialog({
            title: 'Adjuntar',
            autoOpen: true,
            width: 750,
            height: 500,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(760);
     }

     function AdjuntoProtocoloEdit() {
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
         $('<iframe src="../Protocolos/ProtocoloAdjuntar.aspx?desde=valida&idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Adjuntar',
            autoOpen: true,
            width: 900,
            height: 650,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(920);
    }

    function PredefinidoSelect(idDetalle, operacion) {
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
        $('<iframe src="PredefinidoSelect.aspx?idDetalleProtocolo=' + idDetalle +'&Operacion='+operacion+'" />').dialog({        
            title: 'Resultados',
            autoOpen: true,
            width: 500,
            height: 440,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(510);
    }

    function AuditoriaView() {
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
        $('<iframe src="AuditoriaView.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Auditoria del Protocolo',
            autoOpen: true,
            width: 800,
            height: 520,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(900);
    }
 
 
 

    function ProtocoloEdit() {
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
        $('<iframe src="AnalisisEdit.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Practicas Pedidas',
            autoOpen: true,
            width: 800,
            height: 580,
            modal: true,
            resizable: false,
            autoResize: true,
            open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(900);
    }
    function AnalisisMetodoEdit(iditem, idProtocolo) {
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
      $('<iframe src="AnalisisMetodoEdit.aspx?idItem=' + iditem + '&idProtocolo=' + +idProtocolo + '" />').dialog({
            title: 'Presentaciones',
            autoOpen: true,
            width: 400,
            height: 250,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(920);
    }
 
 function ProtocoloEditCarga() {
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
        $('<iframe src="AnalisisEdit.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Practicas Pedidas',
            autoOpen: true,
            width: 800,
            height: 580,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(900);
    }
 
 
 
 
 
 function editDiagnostico() {
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
        $('<iframe src="DiagnosticoEdit.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Diagnostico del Paciente',
            autoOpen: true,
            width: 750,
            height: 555,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(765);
    }
 
   
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de borrar?'))
    return true;
    else
    return false;
    }


    function PreguntoConfirmarRestriccion( ) {
       
        if (confirm('¿Está seguro de restringir acceso al protocolo.?. Al restringir acceso los resultados validados no se publicaran en la Red'))
            return true;
        else
            return false;
    }

    function PreguntoConfirmarPublicar() {

        if (confirm('¿Está seguro de publicar el protocolo.?.'))
            return true;
        else
            return false;
    }
    


    
      function PreguntoConfirmar()
    {
    if (confirm('¿Está seguro de desvalidar lo seleccionado?'))
    return true;
    else
    return false;
    }
      function PreguntoValidar() {
          if (confirm('¿Está seguro de validar todos los ATB?'))
              return true;
          else
              return false;
      }


    function AntecedenteAnalisisView(idAnalisis, idPaciente, ancho,alto) {
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

        $('<iframe src="AntecedentesAnalisisView.aspx?idAnalisis=' +  idAnalisis+ '&idPaciente='+idPaciente+'" />').dialog({
            title: 'Evolucion',
            autoOpen: true,
            width:ancho,
            height: alto,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(800);
    }

   
 function ProtocoloPermisos() {
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
        $('<iframe src="ProtocoloPermiso.aspx?idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Permisos Restringidos',
            autoOpen: true,
            width: 550,
            height:400,
            modal: true,
            resizable: false,
            autoResize: true,
              open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnRestringirAcceso))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(600);
    }
 

     function PesquisaNeonatalView(idProtocolo, ancho,alto) {
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

        $('<iframe src="PesquisaNeonatalView.aspx?idProtocolo=' +  idProtocolo+ '" />').dialog({
            title: 'Tarjeta Pesquisa Neonatal',
            autoOpen: true,
            width:ancho,
            height: alto,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(670);
    }

    </script>



</asp:Content>
