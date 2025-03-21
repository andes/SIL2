<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloProductoEdit.aspx.cs" Inherits="WebLab.Protocolos.ProtocoloProductoEdit" MasterPageFile="~/Site1.Master" %>



<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<%@ Register src="../Calidad/IncidenciaEdit.ascx" tagname="IncidenciaEdit" tagprefix="uc1" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
     <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />

      
     
     <script type="text/javascript">                     
         $(function () {
             $("#tabContainer").tabs();
             $("#tabContainer").tabs({ selected: 0 });
         });

	        $(function() {
	            $("#<%=txtFecha.ClientID %>").datepicker({
	                maxDate: 0,
	                minDate: null,

			        showOn: 'button',
			        buttonImage: '../App_Themes/default/images/calend1.jpg',
			        buttonImageOnly: true
		        });
	        });

	        $(function() {
	            $("#<%=txtFechaOrden.ClientID %>").datepicker({
	                maxDate: 0,
	                minDate: null,

			        showOn: 'button',
			        buttonImage: '../App_Themes/default/images/calend1.jpg',
			        buttonImageOnly: true
		        });
	        });
              
          

            function enterToTab(pEvent) 
            {///solo para internet explorer
                if (window.event) // IE
                {
                    if (window.event.keyCode == 13) {
                        if (pEvent.srcElement.id.indexOf('Codigo_') == 0) {
                            window.event.keyCode = 9;
                        }
                    }
                }
               
             }

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

  </script>  
   
   
    </asp:Content>




 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    &nbsp;<br />
    
    <div class="form-inline">
        
    <table >        
  <tr>
   <td>
  &nbsp;
  
  </td>
  <td  style="vertical-align: top">
                
      <asp:Panel ID="pnlLista" runat="server" 
          style="width:100%;height:400pt;overflow:scroll;" 
          Visible="False">
            <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">Protocolos</h3>
                        </div>

				<div class="panel-body">	
  
  			 
    <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="idProtocolo" onrowcommand="gvLista_RowCommand" 
        onrowdatabound="gvLista_RowDataBound"  
         CssClass="table table-bordered bs-table" 
                                Width="100%" Visible="False" ShowHeader="False" 
                                        >
                                         <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                         <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
       
        <Columns>
            <asp:BoundField DataField="numero" HeaderText="Protocolo" />
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
  </asp:Panel>
  </td>
  <td>
  &nbsp;
  
  </td>
  <td>
  
      <div class="panel panel-default">
                                     <div class="panel-heading">
                                         
    <asp:Panel ID="pnlActualiza" runat="server">  
        <table width="100%">
            <tr>
                 <td align="left" valign="top">
                       <h3>    <span class="label label-default">     <asp:Label ID="lblTitulo" runat="server" 
                                 Font-Bold="False"></asp:Label>  </span>  </h3>
                </td>
                <td align="right" valign="top">
                        <h6> <asp:Label ID="lblEstado" runat="server" Font-Bold="True" Font-Size="8pt" Visible="false"
                                                Text="Label" class="label label-danger" ></asp:Label></h6>
                                                         
                    
                                       
                                                              
                </td>
            </tr>
            <tr>
                 <td align="left" valign="top">
                    <h5> <asp:Label ID="lblServicio1"  runat="server" Text="Label"></asp:Label>
                             </h5>
                </td>
                <td align="right" valign="top">
                    <span id="spanadjunto" runat="server" class="label label-danger">
                            <asp:Label  ID="lblAdjunto" runat="server" Text="El protocolo tiene archivos adjuntos"></asp:Label>
                            </span>                                                        
                                                              <asp:Button ID="btnArchivos" runat="server" OnClick="btnArchivos_Click" Text="Ver Adjuntos" />
                </td>
            </tr>
        </table> 
                      
                              </asp:Panel>
                                      <asp:Label ID="lblServicio" runat="server" Text="Label"></asp:Label>
        
                                         </div>
                                        
  <div class="panel-body">
  
                 <table  width="1010px" align="center">
					<tr>
						<td style="vertical-align: top" rowspan="5"  align="right"> 					
                               
                                    </td>
                        
						<td colspan="2"  >
					        <table style="width:100%;">                               
                                <tr>
                                    <td class="myLabelIzquierda" colspan="8">                                                                     
                                                                                    
                                  
						    
        
             
      <table style="width:100%;">
                                                <tr align="left">
                                                    <td >
                                                        
                                                       <table>
                                                           <tr>
                                                                             <td >
                                                                                Tipo de Muestra:
</td>
                                    <td colspan="3">
                                        

                        <anthem:TextBox ID="txtCodigoMuestra" Width="50px" class="form-control input-sm" runat="server"  ontextchanged="txtCodigoMuestra_TextChanged" AutoCallBack="true"></anthem:TextBox> 
                            <anthem:DropDownList ID="ddlMuestra" runat="server" AutoCallBack="true"  onselectedindexchanged="ddlMuestra_SelectedIndexChanged" class="form-control" TabIndex="1">
                            </anthem:DropDownList>                   
                          
                                        
                                            <asp:RangeValidator ID="rvTipoMuestra" runat="server" ControlToValidate="ddlMuestra" ErrorMessage="Tipo de Producto" MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                          
                                        
                                            </td>
                                                       
                                   
                                                       
                                                           </tr>
                                                           

                                                           <tr>
                                                                             <td >
                                                                                 Descripción:<asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcionProducto" ErrorMessage="Descripcion del producto" ValidationGroup="0">*</asp:RequiredFieldValidator>
                                                                             </td>
                                    <td colspan="7">
                                        

                                                                   <asp:TextBox ID="txtDescripcionProducto" runat="server" MaxLength="200" TabIndex="2" Width="426px" Rows="1" class="form-control" TextMode="MultiLine"></asp:TextBox>
                          
                                        
                                            </td>
                                                       
                                                           </tr>
                                                           
                                                                  <tr>
                                    <td class="myLabelIzquierda">
                                        Soporte/Conservación:</td>
                                    <td>
                                        
                                        
                                        
                                        <asp:DropDownList ID="ddlConservacion" Width="250px" runat="server" TabIndex="3" class="form-control input-sm">
                                            <asp:ListItem Selected="True" Value="0">--Seleccione--</asp:ListItem>
                                            <asp:ListItem>Caldo</asp:ListItem>
                                            <asp:ListItem>Placa</asp:ListItem>
                                        </asp:DropDownList>
                                        
                                        
                                            <asp:RangeValidator ID="rvConservacion" runat="server" ControlToValidate="ddlConservacion" ErrorMessage="Soporte/Conservacion" MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                          
                                        
                                                                      </td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                     <td class="myLabelIzquierda">
                                         &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                    <td align="left">
                                        &nbsp;</td>
                       
                                </tr>
                           
                                                                  <tr>
                                    <td class="myLabelIzquierda">
                                        Fecha Protocolo:</td>
                                    <td>
                                        
                                        
                                        
                    <input id="txtFecha" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px;  z-index:0;position:relative;"  /></td>
                                    <td>
                                        
                                        
                                        
                                        Fecha Orden/Planilla:</td>
                                    <td>
                                        
                                        
                                        
                    <input id="txtFechaOrden" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="5" class="form-control input-sm" /></td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                     <td class="myLabelIzquierda">
                                         &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                    <td align="left">
                                        &nbsp;</td>
                       
                                </tr>
                           
                                <tr>
                                    <td class="myLabelIzquierda">
                                         Nro. de Origen: </td>
                                    <td>
                                        
                                        
                                        
                                      <asp:TextBox ID="txtNumeroOrigen" runat="server" class="form-control input-sm" Width="120px" 
                                            TabIndex="6" MaxLength="50"></asp:TextBox>
                                        </td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                     <td class="myLabelIzquierda">
                                         &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                    <td align="left">
                                        &nbsp;</td>
                                </tr>
                           
                                <tr>
                                    <td class="myLabelIzquierda">
                                         Efector Solicitante: </td>
                                    <td>
                                        
                                        
                                        
                            <anthem:DropDownList ID="ddlEfector" runat="server" Width="250px" 
                                ToolTip="Seleccione el efector" TabIndex="7" class="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlEfector_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
					                 
                                        
                                        </td>
                                    <td align="right">
                                        
                                        
                                        
                                        Origen:</td>
                                    <td>
                                        
                                        
                                        
                                        <asp:DropDownList ID="ddlSectorServicio" runat="server" TabIndex="8" class="form-control input-sm">
                                        </asp:DropDownList>
                                        
					                    <asp:RangeValidator ID="rvSectorServicio" runat="server" 
                                ControlToValidate="ddlSectorServicio" ErrorMessage="Origen" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                        
                          
                                        
                                        </td>
                                    <td>
                                        
                                        
                                        
                                        &nbsp;</td>
                                     <td class="myLabelIzquierda">
                                         &nbsp;</td>
                                    <td>
                                        &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                      <td class="myLabelIzquierda">
                                          &nbsp;</td>
                                    <td align="left">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="myLabelIzquierda">
                                                                                Observaciones internas:</td>
                                    <td colspan="9">
                                    <asp:TextBox ID="txtObservacion" runat="server" class="form-control"
                                                TextMode="MultiLine"  TabIndex="9" Width="600px"  Rows="1"></asp:TextBox>             
                                        
					                 
                                        
                                            </td>
                                </tr>
                                                       </table>
                                                        
                                                            </td>
                                                          <td style="vertical-align: top"   align="right"> 
                                                              
                                                        
                            <asp:Label ID="lblUsuario" runat="server" Text="Label"  Visible="false"></asp:Label> 
                    
                                       
                                                              
                                                        </td>
                                                </tr>
   
                                            </table>
  
  


      
                                  
                             
                           
                         
                                        
                             
                                          
                                            
                               
                                      
                                     </td>
                                        
                                           
                                </tr>
                           
                                
                               
                               	
                            </table>
                    
                                        
                         </td>
					</tr>
					
                     <tr>
						<td colspan="2"  >
                            <hr />
                            </td>
                         </tr>
					
					<tr>
						<td colspan="2"  >
					
						
					<div id="tabContainer" style="width: 1000px; z-index:0; position:relative;" >  
						       <ul class="nav nav-pills">
    <li ><a href="#tab1">Analisis</a></li>        
    <li  id="tab3Titulo" runat="server"><a href="#tab3" >Etiquetas</a></li> 
   
    <li id="tituloCalidad" runat="server"><a  href="#tab5">Incidencias<img alt="tiene incidencias" runat="server" id="inci" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></a></li>    

</ul>
                          
                            <div id="tab1" style="height: 300px">
                   

                                 <table style="width:1000px;">
                                <tr>
                                    <td>
                                  <table cellpadding="1" cellspacing="0" >
                    <tr >
                        <td style="width: 10px; height: 13px">
                        </td>
                        <td style="width: 50px;" class="tituloCelda">
                            Codigo</td>
                        <td style="width: 350px;"  class="tituloCelda">
                            Descripcion</td>
                        <td style="width: 80px; " class="tituloCelda">
                            S/ Muestra</td>                    
                        <td style="width: 18px;" class="tituloCelda">
                        </td>
                    </tr>
                </table>
                
                <div  onkeydown="enterToTab(event)" style="width:545px;height:160pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                       
                    <table  class="mytablaIngreso"  border="0" id="tabla" cellpadding="0" cellspacing="0" >
	  	                <tbody id="Datos" >
		                    
		                </tbody>
	                </table>
	                
                    <input type="hidden" runat="server" name="TxtCantidadFilas" id="TxtCantidadFilas" value="0" />
                </div>
                   
                          
                                                          <asp:CustomValidator ID="cvValidacionInput" runat="server" 
                                                ErrorMessage="Debe completar al menos un analisis" 
                                    ValidationGroup="0" Font-Size="8pt" onservervalidate="cvValidacionInput_ServerValidate" 
                                             ></asp:CustomValidator>
                                                 
                                               
                                      </td>
                                    <td style="vertical-align: top" align="left">
                                       
                                       <fieldset id="Fieldset3" title="Analisis" >
                                       
                                       <table>   
                                         <tr><td   >  
                                             <anthem:DropDownList ID="ddlRutina" runat="server" AutoCallBack="True"                                                
                                                
                                class="form-control input-sm" TabIndex="20" 
                                onselectedindexchanged="ddlRutina_SelectedIndexChanged" Width="250px">
                                          </anthem:DropDownList>

                                              <anthem:LinkButton ID="lnkAgregarRutina" runat="server" ToolTip="Agregar Rutina" OnClientClick="javascript:AgregarDeLaListaRutina();"  Width="40px" >
                                             <span class="glyphicon glyphicon-ok-sign"></span></anthem:LinkButton>
                                                

                                </td></tr>                                       
                                       <tr><td  >	<anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True" 
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" 
                                                TextDuringCallBack="Cargando ..." 
                                class="form-control input-sm" TabIndex="20" Width="250px">
                                            </anthem:DropDownList>	      
                                               <anthem:LinkButton ID="lnkAgregarItem" runat="server" ToolTip="Agregar Determinacion" OnClientClick="javascript:AgregarDeLaLista();"  Width="40px" >
                                             <span class="glyphicon glyphicon-ok-sign"></span></anthem:LinkButton>                                             
                        


                                           </td>
                                       
                                       </tr>
                                      
                                       </table>
                                       </fieldset>
                                        </td>
                                </tr>
                                </table>
                                  <div id="pnlImpresoraAlta" runat="server">
                                   
                            Impresora de Etiquetas:   <asp:DropDownList    class="form-control input-sm" ToolTip="Seleccione Impresora de Etiqueta" ID="ddlImpresoraEtiqueta" runat="server">
                                             </asp:DropDownList>
                                </div>
                                <input type="hidden" runat="server" name="TxtDatosCargados" id="TxtDatosCargados" value="" />                                
                                   <input type="hidden" runat="server" name="TxtDatos" id="TxtDatos" value="" />                                
                <input id="txtTareas" name="txtTareas" runat="server" type="hidden"  />
                          
                          
                           
                            
                            </div>  
                                
                            <div id="tab3"  >
                                  <asp:Panel ID="pnlEtiquetas" runat="server" Visible="false">
                                             

                                              
                                                     
                          
                                             <anthem:RadioButtonList ID="rdbSeleccionarAreasEtiquetas" runat="server" Font-Size="9pt"
                                                 AutoCallBack="True" 
                                                 onselectedindexchanged="rdbSeleccionarAreasEtiquetas_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                 <asp:ListItem Value="1">Marcar Todas</asp:ListItem>
                                                 <asp:ListItem Value="0">Desmarcar Todas</asp:ListItem>
                                             </anthem:RadioButtonList>                  
                                         
                                
                                              <ul class="pagination">
                                     <li>  
                                             <anthem:CheckBoxList    ID="chkAreaCodigoBarra" runat="server" RepeatColumns="6" Font-Size="9pt"></anthem:CheckBoxList> 
                                         </li>
                                </ul>                                            
                                                
                                          <div>
                                     <br />
                            Impresora de Etiquetas:   <anthem:DropDownList    class="form-control input-sm" ToolTip="Seleccione Impresora de Etiqueta" ID="ddlImpresora2" runat="server">
                                             </anthem:DropDownList>
                                
                                              
                                         <anthem:Button  CssClass="btn btn-danger" Width="150px" ID="btnReimprimirCodigoBarras"  onclick="lnkReimprimirCodigoBarras_Click" runat="server" ValidationGroup="9" Text="Reimprimir " /></anthem:Button>
                                              <br />
                                              <anthem:Label ID="lblMensajeImpresion" runat="server" ForeColor="#FF3300" Font-Size="8pt"></anthem:Label>
                                        </div>
                                             <anthem:CheckBox ID="chkRecordarConfiguracion" Visible="false" runat="server" class="form-control input-sm" Text="Recordar ésta configuracion" />
                                                                                     


                                             <anthem:CheckBox ID="chkCodificaPaciente" runat="server" class="form-control input-sm"
                                                 Text="Codificar datos del paciente en todas las etiquetas" 
                                                 ForeColor="#CC3300" Visible="False" />
                                         </asp:Panel>
                            <%--     <div class="panel panel-info">
                                  
                                             <asp:Label ID="lblImprimeCodigoBarras" runat="server" Text=""></asp:Label>   
                                 
                                     <div class="panel-body">
                             
                                              <div class="chekbox">
                                             <anthem:RadioButtonList ID="rdbSeleccionarAreasEtiquetas" runat="server" 
                                                 AutoCallBack="True"
                                                 onselectedindexchanged="rdbSeleccionarAreasEtiquetas_SelectedIndexChanged" RepeatDirection="Horizontal">
                                                 <asp:ListItem Value="1">Marcar Todas</asp:ListItem>
                                                 <asp:ListItem Value="0">Desmarcar Todas</asp:ListItem>
                                             </anthem:RadioButtonList>      
                                                  </div>            
                                          
          <div class="chekbox">
                                             <anthem:CheckBoxList ID="chkAreaCodigoBarra" runat="server" RepeatColumns="4"></anthem:CheckBoxList>                                             
              </div>
      
                                              
                                     <label class="myLabel">Impresora de codigos de barras: </label>
                                            <asp:DropDownList class="form-control input-sm"  ToolTip="Seleccione Impresora de Etiqueta" ID="ddlImpresoraEtiqueta" runat="server">
                                             </asp:DropDownList>
                                            <anthem:LinkButton class="form-control input-sm"  ID="lnkReimprimirCodigoBarras"  onclick="lnkReimprimirCodigoBarras_Click" runat="server">Reimprimir Código de Barras</anthem:LinkButton>
                                            
                                        
                                              <div class="checkbox">
                                             <asp:CheckBox ID="chkRecordarConfiguracion" runat="server"   Text="Recordar ésta configuracion" />
                                                  </div>
                                        
                             </div>
                            
                                  </div>
                             --%>
                             </div>
                             
                         
                                <div id="tab5" style="overflow:scroll;overflow-x:hidden;">
                                    <asp:Panel  ID="pnlIncidencia" runat="server" Height="310px">   
                                    
                                    <uc1:IncidenciaEdit ID="IncidenciaEdit1" runat="server" />
                                     </asp:Panel>
                                </div>
                             
                        </div>
                             
						
						
                       
                
                        </td>

     
					</tr>	
						
						
				
						
						
					<tr>
						
						<td align="left" >
						
                                        
                                             <anthem:TextBox ID="txtCodigo" runat="server" BorderColor="White" ForeColor="White" 
                                BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
                                              <anthem:TextBox ID="txtCodigosRutina"  runat="server" BorderColor="White" 
                                ForeColor="White" BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
                                                 
                        </td>
						
						<td  align="right">
						
          </td>
						
					</tr>
				
						
					
                     <input id="hidToken" type="hidden" runat="server" />
						
						
						
						
						</table>
                           
			</div>
          <div class="panel-footer">	
              <table width="100%">
                  <tr>
                      <td align="left">    <asp:Button ID="btnCancelar" runat="server" Text="Regresar" 
                                                onclick="btnCancelar_Click" class="btn btn-default" TabIndex="99" 
                                                CausesValidation="False" Width="80px" />                                                                                          </td>
                      <td>
                             <div class="checkbox"> <asp:CheckBox ID="chkRecordarPractica" runat="server"  CssClass="myLabelRojo"  Text="Recordar Datos" /></div>
                      </td>
                      <td align="right">
                          
                                             <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" AccessKey="s" CausesValidation="true"
                                          ToolTip="Alt+Shift+S: Guarda Protocolo"  onclick="btnGuardar_Click" CssClass="btn btn-primary" TabIndex="100" Width="80px" Height="40px" /> 
                      </td>
                  </tr>
              </table>		

    </div>

    </div>
  </td>
   <td  style="vertical-align: top">
       <asp:Panel ID="pnlNavegacion" runat="server" Visible="false">
                                   <ul class="pagination">
                                     <li>         <asp:LinkButton ID="lnkAnterior" runat="server" CssClass="myLittleLink" 
                                onclick="lnkAnterior_Click" ToolTip="Ir al protocolo anterior cargado">Anterior</asp:LinkButton> </li>
                                         <li>     <asp:LinkButton 
                                ID="lnkSiguiente" runat="server" CssClass="myLittleLink" 
                                onclick="lnkSiguiente_Click" ToolTip="Ir al siguiente protocolo cargado">Siguiente</asp:LinkButton> </li>
                                </ul>
                                </asp:Panel>
  
  </td>
  
  </tr>
  </table>
        
        </div>
	     
			  <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                     ValidationGroup="0" ShowSummary="False" />			

<script language="javascript" type="text/javascript">

var contadorfilas = 0;
InicioPagina();

document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value  = contadorfilas;

function VerificaLargo (source, arguments)
    {                
    var Observacion = arguments.Value.toString().length;
 //   alert(Observacion);
    if (Observacion>4000 )
        arguments.IsValid=false;    
    else   
        arguments.IsValid=true;    //Si llego hasta aqui entonces la validación fue exitosa        
}              
        
        
        
        function InicioPagina()
        {    
            if (document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosCargados").ClientID %>').value == "")
            {///protocolo nuevo
                CrearFila(true);         
            }
            else
            {        ///modificacion de protocolo
                AgregarCargados();              
            }
              
        }
        
        
        function NuevaFila()
        {
            Grilla = document.getElementById('Datos');
         
             
            fila = document.createElement('tr');
            fila.id = 'cod_'+contadorfilas;
            fila.name='cod_'+contadorfilas;
        	
            celda1 = document.createElement('td');
            oNroFila = document.createElement('input');
            oNroFila.type = 'text';
            oNroFila.readOnly=true;
            
            oNroFila.runat = 'server';
            oNroFila.name = 'NroFila_'+contadorfilas;
            oNroFila.id = 'NroFila_'+contadorfilas;
            //oNroFila.onfocus= function() {PasarFoco(this)}
            oNroFila.className = 'fila';
            celda1.appendChild(oNroFila);
            fila.appendChild(celda1);
        			
            celda2 = document.createElement('td');		
            oCodigo = document.createElement('input');
            
            oCodigo.type = 'text';
            oCodigo.runat = 'server';
            oCodigo.name = 'Codigo_'+contadorfilas;
            oCodigo.id = 'Codigo_'+contadorfilas;
            oCodigo.className = 'codigo';            
            oCodigo.onblur= function () {              
                CargarTarea(this);
            };

            oCodigo.setAttribute("onkeypress", "javascript:return Enter(this, event)"); 
            //oCodigo onkeypress = function(){ return Enter(this, event) };
            //oCodigo.setAttribute( = function () { alert('hola'); if (event.keyCode == 13) event.keyCode = 9; };
            //oCodigo.onchange = function () {CargarDatos()};
            celda2.appendChild(oCodigo);
    	    fila.appendChild(celda2);
        	
    	    celda3 = document.createElement('td');		
            oTarea = document.createElement('input');
            oTarea.type = 'text';
            oTarea.className = 'form-control input-sm';
            oTarea.readOnly=true;
            oTarea.runat = 'server';
            oTarea.name = 'Tarea_'+contadorfilas;
            oTarea.id = 'Tarea_'+contadorfilas;
            oTarea.className = 'descripcion';
            oTarea.onchange = function () {CargarDatos()};
            celda3.appendChild(oTarea);
    	    fila.appendChild(celda3);
        	
    	    celda4 = document.createElement('td');		
            oDesde = document.createElement('input');
            oDesde.type = 'checkbox';
            oDesde.runat = 'server';         
            
            
            
            oDesde.name = 'Desde_'+contadorfilas;
            oDesde.id = 'Desde_'+contadorfilas;
               oDesde.alt="Sin muestra";
            
            oDesde.className = 'muestra';
            oDesde.onblur= function () {CargarDatos(); };

            celda4.appendChild(oDesde);
    	    fila.appendChild(celda4);
        	

        	        	
            celda6 = document.createElement('td');
            oBoton = document.createElement('input');
            oBoton.className='boton';
            oBoton.name= 'boton_'+contadorfilas;
            oBoton.type = 'button';
            oBoton.value= 'X';
            oBoton.onclick = function () {borrarfila(this)};
            celda6.appendChild(oBoton);
            fila.appendChild(celda6);
        	
            Grilla.appendChild(fila);
            contadorfilas = contadorfilas + 1;
        }
    
  
       function CrearFila(validar)
        {
            var valFila = contadorfilas - 1;
	        if (UltimaFilaCompleta(valFila, validar))
	        {
	   
	            NuevaFila();
    	       
    	        valFila = contadorfilas - 1;
    	        document.getElementById('NroFila_' + valFila).value = contadorfilas;
    	        
	            if (contadorfilas > 1)
	            {
	                var filaAnt = contadorfilas - 2;

	            }
    	        
	            document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtCantidadFilas").ClientID %>').value = contadorfilas;	           	            
	            document.getElementById('Codigo_' + valFila).focus();
	        }
        }
        
        
        function UltimaFilaCompleta(fila, validar)
        {
            if ((fila >= 0) && (validar))
            { 
                var cod = document.getElementById('Codigo_' + fila);
                if (cod.value == '') 
                {
       
                    return false;
                }

            }
            
            return true;
        }
        
        function CargarDatos()
        {
            var str = '';            
	        for (var i=0; i<contadorfilas; i++)
	        {	        
	            var nroFila = document.getElementById('NroFila_' + i);
	            var cod = document.getElementById('Codigo_' + i);
	            var tarea = document.getElementById('Tarea_' + i);
	            var desde = document.getElementById('Desde_' + i);	    	            		        
		        if (cod.value!='')
		         str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '#' + desde.checked + '@';
	        }	     
	         document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;
	        
	        
        }
        
        function PasarFoco(Fila)
        {
            var fila = Fila.id.substr(8);            
            document.getElementById('Codigo_' + fila).focus();
        }
        
        function CargarTarea(codigo)
        {
            var nroFila = codigo.name.replace('Codigo_', '');
            var tarea = document.getElementById('Tarea_' + nroFila);            
            var sinMu = document.getElementById('Desde_' + nroFila); 
             	     

           var lista =     document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtTareas").ClientID %>').value ;                             
          
            if (codigo.value == '')
            {
                tarea.value = '';
            }
            else
            {            
          
   
	            if (verificarRepetidos(codigo,tarea))	            
                {             
                    var i = lista.indexOf('#' + codigo.value + '#',0);                  
                    if (i < 0)
                    {
                        codigo.value = '';
                        tarea.value = '';
                        alert('El codigo de analisis no existe o no es válido.');
                        document.getElementById('Codigo_' + nroFila).focus();
                       
                    }
                    else
                    {          
                    
                           if (!verificaDisponible (codigo))
          {
           
                        alert('El codigo ' + codigo.value +' no está disponible. Verifique con al administrador del sistema.');
                        codigo.value = '';
                        tarea.value = '';
                        document.getElementById('Codigo_' + nroFila).focus();
          }
          else                                         
          {
                        var j = lista.indexOf('@',i);
                        i = lista.indexOf('#',i+1) +1;                    
                                        
//                        alert(i);alert(j);
                        tarea.value = lista.substring(i,j).replace ('#True','').replace ('#False',''); 
                    
                        //  sinMu.checked= sinMuestra;
                         CargarDatos();
                         CrearFila(true);                
                         }
                        
                    }
                }
               
            }
        }
        
        
        function verificaDisponible (objCodigo)
        { 
            var devolver=true;
            var esnuevo=true;
            var listaDatos=document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosCargados").ClientID %>').value ;


            var sTabla1 = listaDatos.split(';');                                    
            for (var i=0; i<(sTabla1.length); i++)
            {
                var sItem=sTabla1[i].split('#'); 
                var valorCodigo = sItem[0];
                if (valorCodigo==objCodigo.value)
                {
                    esnuevo=false; break;
                }
            }

            if (esnuevo)
            {         //no esta el codigo
                var listaItem =     document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtTareas").ClientID %>').value ;                             
                var sTabla = listaItem.split('@');                                 
                for (var i=0; i<(sTabla.length-1); i++)
                {
                    var sFila = sTabla[i].split('#');	                
                    if  (sFila[1]!="")
                    {
                        if (objCodigo.value== sFila[1])	                    
                        {
                            if (sFila[3]=="False")// campo que indica si está disponible
                            {
                                devolver=false;
                                break;
                            }
                        }
                    }	 
                }
            }
            return devolver;
        }
        
        
        function verificarRepetidos(objCodigo, objTarea)
        {
            ///Verifica si ya fue cargado en el txtDatos
            var devolver=true;
            var codigo=objCodigo.value;
            if (objTarea.value=='')
            {
                var listaExistente =document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtDatos").ClientID %>').value ;
                var cantidad=1;
                var sTabla = listaExistente.split('@');                                 
                for (var i=0; i<(sTabla.length-1); i++)
                {
                    var sFila = sTabla[i].split('#');	                
                    if  (sFila[1]!="")
                    {
                        if (codigo== sFila[1]) cantidad+=1;	                        	                     
                    }	 

                }

                if (cantidad>1)
                {
                    objCodigo.value = '';
                    objTarea.value = '';
                    alert('El código '+ codigo +' ya fue cargado. No se admiten analisis repetidos.');	
                    objCodigo.focus();	                    
                    devolver=false;       
                }
                else
                    devolver=true;
                ///Fin Verifica si ya fue cargado en el txtDatos
            }
            else
                devolver=true;
                
            return devolver;
        }
        
        
        function borrarfila(obj)
        {
            if(contadorfilas > 1)
            {
	            var delRow = obj.parentNode.parentNode;
	            var tbl = delRow.parentNode.parentNode;
	            var rIndex = delRow.sectionRowIndex;
	            Grilla = document.getElementById('Datos'); 
	            Grilla.parentNode.deleteRow(rIndex);
	            //alert('entra aca');
	            OrdenarDatos();
	            
	            contadorfilas = contadorfilas - 1;
            }
            else
            {
                
	            var cod = document.getElementById('Codigo_0').value = '';
	            var tarea = document.getElementById('Tarea_0').value = '';
	            var desde = document.getElementById('Desde_0').value = '';	           
	            	            
	            CargarDatos();
            }
        }
        
        
        
        function OrdenarDatos()
        {
            var pos = 0;
            var str = '';
            
	        for (var i=0; i<contadorfilas; i++)
	        {
	            var nroFila = document.getElementById('NroFila_' + i);
	            
	            if (nroFila != null)
	            {
	                nroFila.name = 'NroFila_' + pos;
	                nroFila.id = 'NroFila_' + pos;
	                nroFila.value = pos + 1;
	                var cod = document.getElementById('Codigo_' + i);
	                cod.name = 'Codigo_' + pos;
	                cod.id = 'Codigo_' + pos;
	                var tarea = document.getElementById('Tarea_' + i);
	                tarea.name = 'Tarea_' + pos;
	                tarea.id = 'Tarea_' + pos;
	                var desde = document.getElementById('Desde_' + i);
	                desde.name = 'Desde_' + pos;
	                desde.id = 'Desde_' + pos;
	                	                
	                pos = pos + 1;	                	               
	                str = str + nroFila.value + '#' + cod.value + '#' + tarea.value + '#' + desde.value + '@';
	            }   
	        }	        	        
	         document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatos").ClientID %>').value = str;
	      
        }
        
        function AgregarDeLaLista()
        {    
            var elvalor= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigo").ClientID %>').value;
            if (elvalor!='')
            {
                var con= contadorfilas-1;                   
	            if (UltimaFilaCompleta(con, true))	     
	            {
	            NuevaFila();
	            }       
                document.getElementById( 'Codigo_'+con).value=elvalor;          
                CargarTarea( document.getElementById( 'Codigo_'+con)); 

                OrdenarDatos();
            }
        }
        
        
        function AgregarDeLaListaRutina()
        {      
            var elvalor= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigosRutina").ClientID %>').value;
            if (elvalor!='')
            {
                var sTabla = elvalor.split(';');                                    
	            for (var i=0; i<(sTabla.length); i++)
	            {
	            
	                var valorCodigo = sTabla[i];	         
	                var con= contadorfilas-1;	            

	                document.getElementById( 'Codigo_'+con).value=valorCodigo;          
                    CargarTarea( document.getElementById( 'Codigo_'+con)); 
                
	            }
	                          
            }                    
        }
        
        
        function AgregarCargados()
        {      
    //    alert('entra');
            CrearFila(true); 
            var elvalor= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosCargados").ClientID %>').value;    
           
            if (elvalor!='')
            {                           	            
                var sTabla = elvalor.split(';');                                    
	            for (var i=0; i<(sTabla.length); i++)
	            {
	                var sItem=sTabla[i].split('#'); 	                
	                
	                var valorCodigo = sItem[0];	  
	                var sinMuestra=true;
	                if  (      sItem[1]=='No') sinMuestra=true;
	                else 	   sinMuestra=false; 	                      	               
	                
	                var con= contadorfilas-1;	               
	                document.getElementById( 'Codigo_'+con).value=valorCodigo;   
	                   
                    CargarTarea( document.getElementById( 'Codigo_'+con)); 
                      var desde = document.getElementById('Desde_' + con);	    	
                      var boton= document.getElementById( 'boton_'+con); 
                               
                            		        
		         /*if  (sItem[2]=='True') 
		             document.getElementById('Codigo_' + con).className = 'codigoConResultado';
                     */
		         if (sItem[2] == '1') ///resultado cargado
		             document.getElementById('Codigo_' + con).className = 'codigoConResultado';
		         if (sItem[2] == '2')///resultado validado
		             document.getElementById('Codigo_' + con).className = 'codigoConResultadoValidado';


		          desde.checked= sinMuestra;
		        
		         
	            }
            }                    
        }
        
        

     function PreguntoImprimir() {
         if (confirm('¿Está seguro de enviar a imprimir a la impresora seleccionada?'))
             return true;
         else
             return false;
     }


    </script>
   
 
 </asp:Content>

