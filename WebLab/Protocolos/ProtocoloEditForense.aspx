<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="ProtocoloEditForense.aspx.cs" Inherits="WebLab.Protocolos.ProtocoloEditForense" MasterPageFile="~/Site1.Master" %>

<%@ Register Src="~/Services/ObrasSociales.ascx" TagName="OSociales" TagPrefix="uc1" %>

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

      
   	 
     <script type="text/javascript" src="../script/ValidaFecha.js"></script>                

     
     
     <script type="text/javascript">                     
            $(function() {
                 $("#tabContainer").tabs();                        
                $("#tabContainer").tabs({ selected: 0 });
             });                         
          
            
         $(function() {
             $("#<%=txtFecha.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
	        });
	 
	        $(function() {
	            $("#<%=txtFechaOrden.ClientID %>").datepicker({
	                maxDate: 0,
	                minDate: null,

		            showOn: "both",
			        buttonImage: '../App_Themes/default/images/calend1.jpg',
			        buttonImageOnly: true
		        });
	        });
              
           $(function() {
               $("#<%=txtFechaTomaMuestra.ClientID %>").datepicker({
                   maxDate: 0,
                   minDate: null,

		            showOn: "both",
			        buttonImage: '../App_Themes/default/images/calend1.jpg',
			        buttonImageOnly: true
		        });
	        });
          
      
         function showContent() {
             element = document.getElementById("content");
             check = document.getElementById("check");
             if (check.checked) {
                 element.style.display = 'block';
             }
             else {
                 element.style.display = 'none';
             }
         }


  </script>  
  
 


  
 

   
      <style type="text/css">
          .auto-style1 {
              height: 23px;
          }
      </style>
  
 


  
 

   
    </asp:Content>




 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <div align="left"  class="form-inline"  >        
    <table>
  <tr>
   <td>
  &nbsp;
  
  </td>
  <td  style="vertical-align: top">
      <br />
      
   <div class="panel panel-default" id="pnlLista" runat="server" visible="false">
                    <div class="panel-heading">
  <h4>Protocolos</h4>
                        </div>

				<div class="panel-body"   style="height:700px; width:100%;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;">	
   
      
  
  			 
    <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="idProtocolo" onrowcommand="gvLista_RowCommand" 
        onrowdatabound="gvLista_RowDataBound" 
        HorizontalAlign="Left" CssClass="table table-bordered bs-table" 
                                Width="100px" Visible="False" ShowHeader="False" 
                                        >
                                       
                                       
       
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
     
                                         <EditRowStyle BackColor="#999999" />
                                         <AlternatingRowStyle BackColor="White" ForeColor="#333333" />
    </asp:GridView>
    
  
  
  </div>
       </div>
  </td>
  <td>
  &nbsp;
  
  </td>
 <td  style="vertical-align: top">
      <table width="100%">
          <tr>
          <td align="left">  
                <span class="label label-default" __designer:mapid="fd">
      <asp:Label ID="lblServicio" runat="server" ></asp:Label>     </span>
              <br /> &nbsp;
               <br /> &nbsp;
              <span class="label label-default" __designer:mapid="fd">
      <asp:Label ID="lblServicio0" runat="server" ></asp:Label>     </span></td>
          <td align="right">  
                                                              
                                                                      </td>
          <td align="right"><ul class="pagination">
                                     <li>   
                                                     <asp:LinkButton ID="lnkAnterior" runat="server" CssClass="myLittleLink" onclick="lnkAnterior_Click" ToolTip="Ir al protocolo anterior cargado">&lt;Anterior</asp:LinkButton>
                                            </li>
                                                         <li>
                                                     <asp:LinkButton ID="lnkSiguiente" runat="server" CssClass="myLittleLink" onclick="lnkSiguiente_Click" ToolTip="Ir al siguiente protocolo cargado">Siguiente&gt;</asp:LinkButton>
                                                             </li>
                                                         </ul></td>
             </tr></table>

 
                                                     
      <div align="left"  class="form-inline"  >
   <div id="pnlTitulo" style="width:900px;"  runat="server" class="panel panel-default">
                    <div class="panel-heading">
                        <asp:Panel ID="pnlNumero"  runat="server"  Visible="false">
                                                  <h2>          <span class="label label-default">
                                    <asp:Label ID="lblTitulo" runat="server" ></asp:Label>     </span></h2>
                                                                    
                                                                
                                                                                
                                                         
                                                                 </asp:Panel>
   <asp:Panel ID="pnlPaciente" runat="server" 
        >
                                            <table style="width:100%;">
                                              

                                                <tr>
                                                    <td rowspan="5"  >
                                                         <div class="row" style="width:120px;" id="divfoto" runat="server">        
						 <div class="thumbnail"  >
						   <asp:Image ID="imgFoto" ImageUrl="~/App_Themes/default/images/transparente.jpg" runat="server" />
                                   
                                  
                                   
                                     </div>   <asp:LinkButton ID="lnkConsentimiento" runat="server" OnClick="lnkConsentimiento_Click" ToolTip="Reimprimir Consentimiento" Visible="false">
                                             <span class="glyphicon glyphicon-file"></span>Consentimiento</asp:LinkButton>
                                   </div>  <asp:HiddenField ID="HFIdPaciente" runat="server" />
                                                       <asp:HiddenField runat="server" ID="HFNumeroDocumento" />
                                                        <asp:HiddenField runat="server" ID="HFSexo" />
                                                    </td>
                                                     <td>
                                                         <img src="../App_Themes/default/images/Renaper_(logotipo).png"  id="logoRenaper" runat="server" title="Validado con Renaper" width="30" height="30" visible="false" />
                                                         <asp:Label ID="lblPaciente" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="#333333" Text="zzzzzzz"></asp:Label>
                                                         <asp:HyperLink ID="hplModificarPaciente" runat="server" CssClass="myLittleLink" ToolTip="Cambiar el paciente asociado al protocolo" Visible="false">Cambiar Paciente</asp:HyperLink>
                                                         &nbsp;<asp:HyperLink ID="hplActualizarPaciente" runat="server" CssClass="myLittleLink" ToolTip="Actualizar datos del paciente actual.">Datos del Paciente</asp:HyperLink>
                                                         <br />
                                                         <asp:Label ID="lblAlertaProtocolo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="Label" Visible="False"></asp:Label>
                                                        
                                                           <asp:LinkButton ID="lnkValidarRenaper" runat="server" Visible="false" ToolTip="Validar Persona con Renaper" OnClientClick="SelRenaper(); return false;"  OnClick="lnkValidarRenaper_Click"  >
                                             <span class="glyphicon glyphicon-registration-mark"></span>Validar con Renaper</asp:LinkButton> 
                                                      

                                                    </td>
                                                     <td align="right" valign="top"  rowspan="5">
                                                             &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style1">
                                                        <label>
                                                        Fecha de Nacimiento:
                                                        </label><asp:Label ID="lblFechaNacimiento" runat="server" Text=""></asp:Label>
                                                       
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                        Edad:
                                                        </label>
                                                        <asp:Label ID="lblEdad" runat="server" Text="-1"></asp:Label>
                                                        <asp:Label ID="lblUnidadEdad" runat="server" Text="-1"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                        Sexo:
                                                        </label>
                                                        <asp:Label ID="lblSexo" runat="server" Text=""></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>
                                                        Telefono:
                                                        </label>
                                                        <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
                                                        </td>
                                                </tr>                                            
                                            </table>
       
                         
                                          
                                             &nbsp;&nbsp;&nbsp;  
                                        </asp:Panel>
                        </div>

				<div class="panel-body">

                    		
					<div id="tabContainer"  style="width: 850px; z-index:1; position:relative;" >  
						      <ul class="nav nav-pills">
    <li><a href="#tab1">Datos de la muestra</a></li>    
    <li><a href="#tab2">Casos Asociados</a></li>
     <li><a href="#tab3">Adjuntos<img alt="tiene adjunto" runat="server" id="pinadjunto" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></a></li>
    <li><a href="#tab4">Marcadores</a></li>
   

</ul>
                          
                            <div id="tab1" style="height: 400px">


                 <table style="width: 1100px;"
                      align="left">
					<tr>
						<td  >					
						 
                      
                            <table class="auto-style2">
                            
                           
                              

                                <tr>
            
                                
                                    <td >
                                        Fecha Protocolo:</td>
                                    <td>
                                        
                                        <asp:Label ID="lblIdPaciente" runat="server" Text="" Visible="False"></asp:Label>
                               
           
                                     
                    <input id="txtFecha" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 120px; position=absolute; z-index=0;"  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>
           
               </td>
                                    <td   align="right">
                                        
                                        &nbsp;</td>
                                    <td  >
                                        
                                        &nbsp;</td>
                                     <td  >
                                         &nbsp;</td>
                                    <td>
                                        
                                        &nbsp;</td>
                                      <td   align="right" rowspan="9">
                                          &nbsp;</td>
                        <td rowspan="9">
                         
                        </td>
                                </tr>
                            
                           
                              

                                <tr>
            
                                
                                    <td  >
                                        
                                        Fecha Pedido:</td>
                                    <td>
                                        <span class="d-inline-block" tabindex="1" data-toggle="tooltip" title="Fecha de Orden/Planilla">
                                        <input id="txtFechaOrden" runat="server" type="text" maxlength="10" 
                        style="width: 120px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  />
                                            </span></td>
                                    <td   align="right">
                                        
                                        &nbsp;</td>
                                    <td  >
                                        
                                        &nbsp;</td>
                                     <td  >
                                         &nbsp;</td>
                                    <td>
                                        
                                        &nbsp;</td>
                                </tr>
                            
                           
                              

                                <tr>
            
                                
                                    <td  >
                                        Fecha y Lugar Toma Muestra:</td>
                                    <td>
                                         <span class="d-inline-block" tabindex="2" data-toggle="tooltip" title="Fecha de Orden/Planilla">
                    <input id="txtFechaTomaMuestra" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="2" class="form-control input-sm"
                                style="width: 120px"  />
                                             </span>
                                        <span class="d-inline-block"  data-toggle="tooltip" title="Institución de Toma de Muestra">
                                         <asp:DropDownList ID="ddlEfector" runat="server" Width="250px"  TabIndex="3" class="form-control input-sm">
                                   
                            </asp:DropDownList>

                                             
                                        </span>
					                 
                                        
                                            </td>
                                    <td   align="right">
                                        
                                        &nbsp;</td>
                                    <td  >
                                        
                                        &nbsp;</td>
                                     <td  >
                                         &nbsp;</td>
                                    <td>
                                        
                                        &nbsp;</td>
                                </tr>
                            
                           
                              

                                <tr>
            
                                
                                    <td  >
                                        
                                        Solicitante:</td>
                                    <td colspan="3">
                                         <span class="d-inline-block" tabindex="1" data-toggle="tooltip" title="Entidad Solicitante">
                                        <asp:DropDownList ID="ddlSectorServicio" class="form-control input-sm" runat="server" TabIndex="4" Width="300px">
                                        </asp:DropDownList>
                                        </span>
					                    <asp:RangeValidator ID="rvSectorServicio" runat="server" 
                                ControlToValidate="ddlSectorServicio" ErrorMessage="Sector" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                        
					                 
                                        
               </td>
                                     <td  >
                                         &nbsp;</td>
                                    <td>
                                        
                                         &nbsp;</td>
                                </tr>
                               
                           
                              

                                <tr>
            
                                
                                    <td  >
                                        <asp:Label ID="lblOrigen" runat="server" Text="Nro. Origen"></asp:Label>
                                         </td>
                                    <td>
                                         <span class="d-inline-block" tabindex="5" data-toggle="tooltip" title="Nro. Cadena Custodia (para forense)">
                                        <asp:TextBox ID="txtNumeroOrigen" runat="server" class="form-control input-sm" Width="160px" 
                                            TabIndex="5" MaxLength="50"></asp:TextBox>
                                      </span>
                          
                                        <asp:Label ID="lblCadenaCustodia" Visible="false" runat="server" Text="Nro. Cadena Custodia" ForeColor="Red"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNumeroOrigen" ErrorMessage="Nro. Origen/Expediente" ValidationGroup="0"></asp:RequiredFieldValidator>
                                      
                          
                                        
               </td>
                                    <td   align="right">
                                        
                                         &nbsp;</td>
                                    <td colspan="2"  >
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                         &nbsp;</td>
                                </tr>
                               
                           
                              

                                <tr>
            
                                
                                    <td  >
                                                                                Muestra a Analizar:       </td>
                                    <td>
                                         <span class="d-inline-block"   data-toggle="tooltip" title="Codigo de muestra">
                            <anthem:TextBox ID="txtCodigoMuestra" Width="50px" TabIndex="6" class="form-control input-sm" runat="server"  ontextchanged="txtCodigoMuestra_TextChanged" AutoCallBack="true"></anthem:TextBox> 
                                             </span>
                                         <span class="d-inline-block"  data-toggle="tooltip" title="Muestra recibida">
                            <anthem:DropDownList ID="ddlMuestra" runat="server" TabIndex="7"  AutoCallBack="true"  onselectedindexchanged="ddlMuestra_SelectedIndexChanged"  class="form-control input-sm" >
                            </anthem:DropDownList>
                                             </span>
                            <anthem:RangeValidator ID="rvMuestra" runat="server"     ErrorMessage="Muestra" 
                                ControlToValidate="ddlMuestra" Enabled="False" MaximumValue="9999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                            
                          
                                        
               </td>
                                    <td   align="right">
                                        
                                         &nbsp;</td>
                                    <td colspan="2"  >
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                         &nbsp;</td>
                                </tr>
                               
                           
                              

                                <tr>
            
                                
                                    <td  >
                                                                                Descripcion de la muestra:</td>
                                    <td>
                                         <span class="d-inline-block"   data-toggle="tooltip" title="Descripción de la muestra">
                                        <asp:TextBox ID="txtDescripcionProducto" runat="server" class="form-control input-sm" Width="300px" 
                                            TabIndex="8" MaxLength="50"></asp:TextBox>
                                             </span>
                                      
                          
                                        
               </td>
                                    <td   align="right">
                                        
                                        &nbsp;</td>
                                    <td colspan="2"  >
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                         &nbsp;</td>
                                </tr>
                               
                           
                              

                                <tr>
            
                                
                                    <td  >
                                                                                Vínculo:</td>
                                    <td>
                                    <span class="d-inline-block"   data-toggle="tooltip" title="Parentesco Alegado">    
                            <asp:DropDownList ID="ddlParentesco" runat="server" TabIndex="9"  AutoCallBack="true"  onselectedindexchanged="ddlMuestra_SelectedIndexChanged"  class="form-control input-sm" >
                            </asp:DropDownList>
                            </span>
                          
                                        
                                        <asp:RangeValidator ID="rvParentesco" runat="server" 
                                ControlToValidate="ddlParentesco" ErrorMessage="Parentesco Alegado" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                            
                          
                                        
               </td>
                                    <td   align="right">
                                        
                                        &nbsp;</td>
                                    <td colspan="2"  >
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                         &nbsp;</td>
                                </tr>
                               
                           
                              

                                <tr>
            
                                
                                    <td  >
                                        
                                        Observación Parentesco:</td>
                                    <td>
                                         <span class="d-inline-block"   data-toggle="tooltip" title="Observación del parentesco">
                                        <asp:TextBox ID="txtObservacionParentesco" runat="server" class="form-control input-sm" Width="220px" 
                                            TabIndex="10" MaxLength="50"></asp:TextBox>
                                      </span>
                          
                                        
               </td>
                                    <td   align="right">
                                        
                                        &nbsp;</td>
                                    <td colspan="2"  >
                                        
                                        &nbsp;</td>
                                    <td>
                                        
                                         &nbsp;</td>
                                </tr>
                               
                               	</table>
                          
 
                                   
                                     

                                        
                                            </td>
					</tr>
				

					
					<tr>
							<td style="vertical-align: top"   >  
                                   
                                Observaciones Internas:  	
                                  <span class="d-inline-block" data-toggle="tooltip" title="Observaciones internas de la muestra">					
                                    <asp:TextBox ID="txtObservacion" runat="server" class="form-control input-sm" 
                                                TextMode="MultiLine"  TabIndex="11" Width="500px"  ></asp:TextBox>   
                                      </span>        
                                     <input id="hidToken" type="hidden" runat="server" />
						    <anthem:TextBox ID="txtCodigo" runat="server" BorderColor="White" ForeColor="White" 
                                BackColor="White" BorderStyle="Solid" BorderWidth="0px" TabIndex="999"></anthem:TextBox>  
                                <br /><asp:CustomValidator ID="cvValidacionInput" runat="server" 
                                                ErrorMessage="Debe completar al menos un analisis" 
                                    ValidationGroup="0" Font-Size="8pt"  onservervalidate="cvValidacionInput_ServerValidate" TabIndex="99" 
                                             ></asp:CustomValidator>

                                              <anthem:TextBox ID="txtCodigosRutina"  runat="server" BorderColor="White" 
                                ForeColor="White" BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>
						 <input type="hidden" runat="server" name="TxtDatosCargados" id="TxtDatosCargados" value="" />                                
                                   <input type="hidden" runat="server" name="TxtDatos" id="TxtDatos" value="" />                                
                <input id="txtTareas" name="txtTareas" runat="server" type="hidden"  />

                               
							</td>
						</tr>																
						
						
						
					
                
						
				
                     														
						
						
						
					
                
						
																		
						
						
						
					
                
						
				
                     														
						
						
						
					
                
						
						</table>

                                </div>

                               <div id="tab2" style="height: 400px">


                                                            <asp:GridView ID="gvListaCaso" runat="server" AllowPaging="True"  CssClass="table table-bordered bs-table" 
                                                                AutoGenerateColumns="False"  CellPadding="2" DataKeyNames="idCasoFiliacion" 
                                                                EmptyDataText="No se encontraron casos para el protocolo" 
                                                                GridLines="Horizontal" 
                                                            
                                                              Width="100%" BackColor="White" OnRowCommand="gvListaCaso_RowCommand" OnRowDataBound="gvListaCaso_RowDataBound">
                                                                
                                                                <Columns>
                                                                
                                                                    <asp:BoundField DataField="idCasoFiliacion" HeaderText="Nro.">
                                                                       
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="nombre" HeaderText="Nombre">
                                                                     
                                                                    </asp:BoundField>
                                                                     <asp:BoundField DataField="estado" HeaderText="Estado">
                                                                       
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>

                                                                              <asp:LinkButton ID="Editar" runat="server" Text=""  CssClass="btn btn-success" Width="100px" >
Desvincular</asp:LinkButton>
                                                                        
                                                                        </ItemTemplate>
                                                                    
                                                                    </asp:TemplateField>
                                                               
                                                                </Columns>
                
                                                                <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                  
                                                            </asp:GridView>


                                   </div>


                                      <div id="tab3" style="height: 400px">
                                          <p>Adjunto</p>
                                           <asp:LinkButton ID="lnkSiguiente0" runat="server" CssClass="myLittleLink" onclick="lnkSiguiente0_Click" ToolTip="Archivos Adjuntos">Adjuntos</asp:LinkButton>
                                          </div>


                        <div id="tab4">
                              <asp:Panel runat="server" ID="pnlMarcadores" Visible="true">
                             <div> Archivo Genotipos:   <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm"  />  <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="200px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />
                                  <asp:Label ID="estatus" runat="server" Style="color: #0000FF"></asp:Label>

               

               </div>
                                  </asp:Panel>
                 
                                  <asp:GridView ID="gvTablaForense" runat="server" Font-Names="Verdana" Font-Size="12pt" EmptyDataText="No se encontraron datos de los protocolos del caso"></asp:GridView>
                             <asp:Button ID="btnAgregar" runat="server" Text="Guardar" Visible="false" 
                                                onclick="btnAgregar_Click" CssClass="btn btn-success" Width="200px" />
                        </div>
                         </div>
       </div>
                               <div class="panel-footer">
                                    
                                   <table width="100%">
                                       <tr>
                                           <td colspan="2">
 <asp:Panel ID="pnlNavegacion" runat="server">
                                                                      <asp:Label ID="lblEstado" runat="server"  Text="Label" Visible="False"></asp:Label>
                                                                  <p>  Ingresado por:  <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label></p>
                                                                  </asp:Panel>

                                           </td>
                                       </tr>
                                       		
					<tr>
						
						<td align="left" >
						
                                            <asp:Button ID="btnCancelar" runat="server" Text="Regresar" 
                                                onclick="btnCancelar_Click"  CssClass="btn btn-success"  TabIndex="13" 
                                                CausesValidation="False" Width="120px" />  

                        </td>
						
						<td  align="right">
						
                                               <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" AccessKey="s" CausesValidation="true"
                                          ToolTip="Alt+Shift+S: Guarda Protocolo"  onclick="btnGuardar_Click" CssClass="btn btn-success" Width="80px" TabIndex="12"  /></td>
						
					</tr>
                                   </table>
                                   </div>
       </div>  
          
			</div>
  </td>
      <td style="vertical-align: top">
            
            &nbsp;</td>
  </tr>
       
            
       

  </table> 
	</div>	
			  <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                     ValidationGroup="0" ShowSummary="False" TabIndex="99" />			

<script language="javascript" type="text/javascript">


     var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();
    
    
    var sexo =        $("#<%= HFSexo.ClientID %>").val(); 
         var numeroDocumento = $("#<%= HFNumeroDocumento.ClientID %>").val();

     function SelRenaper() {
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
         $('<iframe src="ProcesaRenaper.aspx?master=1&Tipo=DNI&sexo=' + sexo + '&dni=' + numeroDocumento + '&id='+idPaciente+'" />').dialog({
             title: 'Valida Renaper',
             autoOpen: true,
             width: 600,
             height: 600,
             modal: true,
             resizable: false,
             autoResize: true,
             open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },
                 buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.lnkValidarRenaper))%>; }               
            },
        
             overlay: {
                 opacity: 0.5,
                 background: "black"
             }
         }).width(600);
     }


    </script>
   
 
    </div>
   
 
 </asp:Content>

