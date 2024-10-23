<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoViewList.aspx.cs" Inherits="WebLab.Resultados.ResultadoViewList" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
 
   <link href="jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                   <script type="text/javascript">                     

             $(function() {
                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                        $("#tabContainer").tabs({ selected: currTab });
             });

                   
      
                  </script> 

  
    

    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
   <div align="left" style="width:1200px;"   class="form-inline"  >
     <div class="panel panel-default">
           <div class="panel-heading">
                <asp:Label ID="lblTitulo" runat="server" Text="CARGA DE RESULTADOS" 
                                ForeColor="#2B7EBD"></asp:Label>
               </div>
             <div class="panel-body">
   <asp:HiddenField runat="server" ID="HFIdPaciente" /> 
                 <table  width="100%"  >
					
				 
					
					<tr>
						<td   
                            
                            
                            style="font-family: Arial; font-size: 14px; font-weight: bold; vertical-align: top;" 
                            colspan="3">
                            
                                        <asp:Button ID="btnPeticion" runat="server" CssClass="myButton" 
                                            OnClientClick="editPeticion(); return false;" Text="Petición Electrónica" 
                                            ToolTip="Petición Electronica" Visible="false" Width="150px" />
                                    </td>
					</tr>
						
					
					
					<tr>
						<td style="width:100%;"  colspan="3">
    <asp:HyperLink ID="hypRegresar" AccessKey="r" ToolTip="Alt+Shift+R" runat="server" CssClass="myLink">Regresar</asp:HyperLink>
  
                        </td>
					</tr>
					
					
					
						
						<tr>
						
					
					
					
						
						<td   align="left" style="vertical-align: top">
                            <table style="width:180px;">
                                <tr>
                                   <td align="left">
                                       &nbsp;
      <asp:Label CssClass="myLabelGris" ID="lblCantidadRegistros" runat="server" Text="Label"></asp:Label>
                                           <asp:LinkButton ID="lnkAnterior" runat="server" CssClass="myLittleLink" ToolTip="Avanza al protocolo siguiente" 
                                onclick="lnkAnterior_Click">&lt;Anterior</asp:LinkButton> &nbsp;|&nbsp; <asp:LinkButton 
                                ID="lnkPosterior" runat="server" CssClass="myLittleLink" ToolTip="Avanza al protocolo anterior"
                                onclick="lnkPosterior_Click">Siguiente&gt;</asp:LinkButton>
                                      </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                      
                                        <div  style="width:315px; height:450pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                                            <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                            DataKeyNames="idProtocolo" onrowcommand="gvLista_RowCommand" 
                                            onrowdatabound="gvLista_RowDataBound" CellPadding="4" 
                                            HorizontalAlign="Left" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="12pt"  BorderColor="#DEDFDE" BorderStyle="None" 
                                            BorderWidth="1px" GridLines="Vertical" Font-Bold="False" ForeColor="Black" Width="300px" BackColor="White" >
                                            <FooterStyle BackColor="#CCCC99" />
                                            <RowStyle BackColor="#F7F7DE" Font-Names="Arial" 
                                            Font-Size="8pt" />

                                            <Columns>
                                                   <asp:BoundField HeaderText="DU" DataField="du" >
                                                   <ItemStyle Width="50px" />
                                                   </asp:BoundField>
                                                <asp:BoundField HeaderText="Paciente" DataField="paciente" >
                                                   <ItemStyle Width="200px" />
                                                   </asp:BoundField>
                                            <asp:BoundField DataField="numero" HeaderText="Protocolo" >
                                                   <ItemStyle Width="50px" />
                                                   </asp:BoundField>
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:ImageButton ID="Pdf" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                                            CommandName="Pdf" />
                                            </ItemTemplate>

                                            <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="estado" Visible="False" />

                                            </Columns>
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                                            Font-Names="Arial" Font-Size="8pt" />
                                            <AlternatingRowStyle BackColor="White" />
                                                <SortedAscendingCellStyle BackColor="#FBFBF2" />
                                                <SortedAscendingHeaderStyle BackColor="#848384" />
                                                <SortedDescendingCellStyle BackColor="#EAEAD3" />
                                                <SortedDescendingHeaderStyle BackColor="#575357" />
                                            </asp:GridView>
                                     </div>


                                        </td>
                                </tr>
                            </table>
                         
                        </td>
						
					
					
					
						
						<td   align="left" style="vertical-align: top">
                            &nbsp;</td>
						
						<td style="vertical-align: top">

                            <table visible="false" runat="server" id="tblResultados" style="width:820px;">
                              
							<tr>
						
						<td    colspan="3">  
						
						<table class="mytable1" >
      						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo" width="150px">
                            DU:</td>
						<td   width="250px">
                          
                              
                            <asp:Label ID="lblDni" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt"></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo" width="100px">
                            Paciente:</td>
						<td 
                            align="left" colspan="3" style="width: 280px"  >
                          
                              
                            <asp:Label ID="lblPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt"></asp:Label>
                            <asp:Label ID="lblCodigoPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt" Visible="False"></asp:Label>
                            </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo" width="150px">
                                Sexo:</td>
						<td   width="250px">
                          
                              
                            <asp:Label ID="lblSexo" runat="server" Text="Label" CssClass="myLabel" ></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo" width="100px">
                            Fecha Nac.:</td>
						<td 
                            align="left"   width="150px"  >
                          
                              
                            <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                            </td>
                      	<td class="myLabelIzquierdaFondo"  width="120px">
                            Edad:</td>
                        <td     align="left"  width="130px" >   
                                     <asp:Label ID="lblEdad" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>   
                            </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo" width="150px">
                                Protocolo</td>
						<td   width="250px">
                          
                              
                            <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="11pt" ></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo" width="100px">
                            Nro. de Origen:</td>
						<td 
                            align="left"   width="150px"  >
                          
                              
                            <asp:Label ID="lblNumeroOrigen" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                            </td>
                      	<td class="myLabelIzquierdaFondo"  width="120px">
                           Fecha</td>
                        <td     align="left"  width="130px" >   
                                     <asp:Label Font-Bold="True" 
                                Font-Size="10pt" ID="lblFecha" runat="server" Text="Label"></asp:Label>    </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                                Origen:</td>
						<td  >
                            <asp:Label ID="lblOrigen" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo">
                            Prioridad:</td>
						<td 
                            align="left"   >
                            <asp:Label ID="lblPrioridad" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                            </td>
                      	<td class="myLabelIzquierdaFondo">
                            Servicio:</td>
                        <td   align="left">   
                                   <asp:Label ID="lblServicio" runat="server" CssClass="myLabelIzquierdaGde" 
                                       Text="Label"> </asp:Label>
                                    </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                               Sector:</td>
						<td  >
                            <asp:Label ID="lblSector" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo">
                            Médico Sol:</td>
						<td 
                            align="left"   >
                           <asp:Label ID="lblMedico" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>   
                            </td>
                    	<td class="myLabelIzquierdaFondo"> Usuario</td>
                        <td    align="left"> <asp:Label  ID="lblUsuario" runat="server" Text="Label"></asp:Label>                     
                       
                        </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                                Diagnósticos:</td>
						<td colspan="3"  >
                            <asp:Label ID="lblDiagnostico" runat="server" Text="" Font-Bold="False" 
                                Font-Size="9pt" CssClass="myLabel"></asp:Label>                                                                                                               
                        </td>
                       	<td class="myLabelIzquierdaFondo">     
                               Fecha de Registro:</td>
                       <td   align="left">     
                                <asp:Label  ID="lblFechaRegistro" runat="server" Text="Label"></asp:Label>                
                                <asp:Label  ID="lblHoraRegistro" runat="server" Text="Label"></asp:Label>    
                        </td>
					</tr>
						
					
					
						
					
					
						<tr>
						<td class="myLabelIzquierdaFondo">Prácticas Solicitadas:</td>
						<td colspan="5"  >
                            <asp:Label ID="lblPedidoOriginal" runat="server" Text="Label"  
                                ></asp:Label> 
						   
                            </td>
					</tr>
					</table>
						</td>
						</tr>
				
					<tr>
						
						<td    colspan="3">  
						
						    
<asp:HiddenField runat="server" ID="HFCurrTabIndex" /> 
                           
                            <div id="tabContainer">  
                  
                            
                            <asp:Panel ID="pnlHC" runat="server" > 
                  
                            </asp:Panel>
						 

        <div id="tab1" >   
        
        <table width="850px">
       
        	<tr>
						
						<td  colspan="3" style="vertical-align: top">     <asp:Label CssClass="myLabelIzquierdaGde" ID="lblMuestra" runat="server" ></asp:Label>&nbsp; <br />&nbsp; <br />
                        <asp:Label CssClass="myLabelIzquierdaGde" ID="lblEstadoProtocolo" Visible="false" runat="server" ></asp:Label>
						     <asp:Panel ID="Panel1" runat="server"  Width="100%" BackColor="#F2F2F2" BorderColor="#D7D7D7" BorderStyle="Solid" BorderWidth="1">                                                                                                                                      						 
                                               <asp:Table ID="tContenido"  
                                                   Runat="server" CellPadding="0" CellSpacing="0" CssClass="mytable1" 
                                                   Width="100%" ></asp:Table>                                                                                                                                     
                                           </asp:Panel></td>
						
					</tr>
        	<tr>
						
						<td class="myLabel" style="vertical-align: top" colspan="3"> 
                         </td>
						
					</tr>
        	<tr>
						
						<td class="myLabel" style="vertical-align: top" colspan="3"> 
                  
                 
                                                                    </td>
						
					</tr>
     
        	<tr>
						
						
						<td  colspan="2" align="left"> 
						
                            <asp:Label ID="lblObservacionResultado" runat="server" Font-Names="Courier New" 
                                Font-Size="9pt" ForeColor="Black" Visible="False"></asp:Label>
                            
                                                                    </td>
                                                                    </tr>
                                                                    
                                                                    
                                                  
        		
					
        	<tr>
						
						<td class="myLabel" style="vertical-align: top" colspan="3"> 
                           <hr /></td>
						
					</tr>
     
        	<tr>
						
						<td align="left" colspan="2"  > 
                            <asp:Panel ID="pnlReferencia" runat="server" Width="300px">
                              <table style="border: 1px solid #3A93D2; font-size: 10px" width="100%">
                                                
                        <tr>
                        <td><b>Referencias:</b></td>
                            <td>
                               Dentro de VR</td>
                            <td style="color: #FF0000">
                                Fuera de VR</td>
                                <td style="color: #FF0000">
                                </td>

                        </tr>
                        
                        </table>
                            </asp:Panel> 
                                                        
                            </td>
						
						<td align="right"  > 
						      
                            </td>
						
					</tr>
        	</table>
                                          
                                           </div>
                                               
        
        
                                               <div  id="tab3" >
                                               <asp:Panel runat="server" Height="600px" id="pnlAntecedentes" >
                                                   <table width="850">
                                                       <tr>
                                                           <td class="myLabelIzquierdaGde">
                                                               &nbsp;
                                                               Análisis:&nbsp;&nbsp;
                                                               
                                                               <asp:DropDownList ID="ddlItem" runat="server">
                                                               </asp:DropDownList>
                                                              
                                                                <asp:Button ID="btnVerAntecendente" runat="server"  CssClass="myButtonGrisLittle"
                                                                   onclick="btnVerAntecendente_Click" Text="Ver Antecedentes" Width="120px" />
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               &nbsp;
                                                               &nbsp;
                                                               &nbsp;
                                                               <asp:GridView ID="gvAntecedente" runat="server" 
                                                                   CellPadding="1" EmptyDataText="No se encontraron antecedentes." Font-Size="9pt" 
                                                                   Width="100%" CssClass="mytable2">
                                                                   <HeaderStyle BackColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                                                                       ForeColor="#333333" BorderColor="#666666" />
                                                               </asp:GridView>
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               &nbsp;
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </asp:Panel> 
                                               
                                               </div>
                                               
                                               
                                          
</div>
</td>
						
					</tr>
					<tr>
						
						<td   colspan="3" align="right">
                       
                             <asp:ImageButton ID="imgImprimir" runat="server" 
                                ImageUrl="~/App_Themes/default/images/imprimir.jpg" 
                                   onclick="imgImprimir_Click" /> &nbsp; 
						       <asp:ImageButton ID="imgPdf" runat="server" ToolTip="Descargar PDF" 
                                ImageUrl="~/App_Themes/default/images/pdf.jpg" onclick="imgPdf_Click" /> </td>
						
					</tr>
					
					<tr>
						
						<td   colspan="3" align="right">
                            <asp:Panel ID="pnlImpresora" runat="server" style="border:1px solid #C0C0C0; width:100%; background-color: #EFEFEF;">                                                      
 <table width="720px" align="center">
 	<tr>
						<td class="myLabelIzquierda" align="left" style="width: 140px; background-color: #EFEFEF;">
                                        Impresora del sistema: </td>
						<td  align="left">
                                        <asp:DropDownList ID="ddlImpresora" runat="server" CssClass="myList" >
                                        </asp:DropDownList>
                            </td>
						
                                        </tr>
                                        </table>
														
 </asp:Panel>
                         </td>
						
					</tr>
					        </table>
                            </td>
						
					</tr>
											
					
					
					
						
						</table>			
                 
                 			
 </div>

     </div>
    </div>
    
 <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">




    var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();

  
  function AntecedenteView(idAnalisis, idPaciente, ancho, alto) {
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
    function editPeticion() {
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
        $('<iframe src="../PeticionElectronica/Default.aspx?idPaciente=' + idPaciente + '&idOrigen=3" />').dialog({
            title: 'Nueva Peticion',
            autoOpen: true,
            width: 950,
            height: 600,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(1020);
    }


    </script>

</asp:Content>

