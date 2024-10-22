﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloEdit3.aspx.cs" Inherits="WebLab.Protocolos.ProtocoloEdit3" MasterPageFile="~/Site1.Master" %>

<%@ Register Src="~/Services/ObrasSociales.ascx" TagName="OSociales" TagPrefix="uc1" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<%@ Register src="../Calidad/IncidenciaEdit.ascx" tagname="IncidenciaEdit" tagprefix="uc1" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
  

    <link rel="stylesheet" href="../App_Themes/default/style.css"  />
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />

     
   	 
     <script type="text/javascript" src="../script/ValidaFecha.js"></script>                

     
     
     <script type="text/javascript">     
         
        
         $(function () {
             
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
            $(function() {
             $("#<%=txtFechaFIS.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
            });
           $(function() {
             $("#<%=txtFechaFUC.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
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
  
 

   
      <style type="text/css">
          .auto-style1 {
              width: 722px;
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
  <td>
     
      <table width="100%"><tr>
          <td>  <span class="label label-default">
      <asp:Label ID="lblServicio" runat="server" ></asp:Label>     </span></td>
          <td align="right">   <asp:Panel ID="pnlNavegacion" runat="server">
                                         <asp:Label ID="lblEstado" runat="server" Font-Bold="True" Font-Size="8pt" Text="Label" Visible="False"></asp:Label>
                                                    
                                             
                                                <ul class="pagination">
                                     <li>   
                                                     <asp:LinkButton ID="lnkAnterior" runat="server" CssClass="myLittleLink" onclick="lnkAnterior_Click" ToolTip="Ir al protocolo anterior cargado">&lt;Anterior</asp:LinkButton>
                                            </li>
                                                         <li>
                                                     <asp:LinkButton ID="lnkSiguiente" runat="server" CssClass="myLittleLink" onclick="lnkSiguiente_Click" ToolTip="Ir al siguiente protocolo cargado">Siguiente&gt;</asp:LinkButton>
                                                             </li>
                                                         </ul>


                                     </asp:Panel></td>
             </tr></table>

 <br />
                                                     
      <div align="left"  class="form-inline"  >
   <div id="pnlTitulo"  runat="server" class="panel panel-default">
                    <div class="panel-heading">
   <asp:Panel ID="pnlPaciente" runat="server" 
        >
                                            <table style="width:100%;">
                                              

                                                <tr>
                                                    <td>
                                                       <img src="../App_Themes/default/images/Renaper_(logotipo).png"  id="logoRenaper" runat="server" title="Validado con Renaper" width="30" height="30" visible="false" />
                                                        <asp:Label ID="lblPaciente" runat="server" Font-Bold="True" Font-Names="Arial" 
                                                            Font-Size="12pt" ForeColor="#333333" Text="zzzzzzz"></asp:Label>
                                                        <asp:HiddenField runat="server" ID="HFIdPaciente" />     
                                                         <asp:HiddenField runat="server" ID="HFNumeroDocumento" />
                                                        <asp:HiddenField runat="server" ID="HFSexo" />
                                                    
                                                           <asp:HyperLink ID="hplModificarPaciente" runat="server" Visible="true" CssClass="myLittleLink" ToolTip="Cambiar el paciente asociado al protocolo">Cambiar Paciente</asp:HyperLink>
                                                        &nbsp;<asp:HyperLink ID="hplActualizarPaciente" runat="server" CssClass="myLittleLink" ToolTip="Actualizar datos del paciente actual.">Datos del Paciente</asp:HyperLink>
                                                        <asp:Label ID="lblAlertaProtocolo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="Label" Visible="false"></asp:Label>
                                                </td>
                                                     <td align="right" valign="top"  rowspan="4">
                                                         
                                                              <asp:Panel ID="pnlNumero"  runat="server"  Visible="false">
                                                  <h2>          <span class="label label-default">
                                    <asp:Label ID="lblTitulo" runat="server" ></asp:Label>     </span></h2>
                                                                  <br />
                                                                   <asp:Label ID="lblUsuario" runat="server"  Text="Label"></asp:Label>
                                                                  <br />
                                                                  <asp:Button ID="btnNotificarSISA" runat="server" Text="Notificar SISA" Width="100px"  CssClass="btn btn-success"  Visible="false" OnClick="btnNotificarSISA_Click" />                    
                                                             <br />
                                                                  <asp:Label ID="lblNroSISA" runat="server" class="label label-success" Font-Size="14px" Text="Label" Visible="false"></asp:Label>
                                                                  <br />
                                                                      <span id="spanadjunto" runat="server" class="label label-danger">
                            <asp:Label  ID="lblAdjunto" runat="server" Text="El protocolo tiene archivos adjuntos"></asp:Label>
                                                                
                                                                  
                            </span>                                    
                                                                 </asp:Panel>
                                                                    
                                                  
                                                                    
                                      
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td >
                                                        <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"></asp:Label>
                                                        <label>
                                                        Edad:
                                                        </label>
                                                        <asp:Label ID="lblEdad" runat="server" Text="Label"></asp:Label>
                                                        <asp:Label ID="lblUnidadEdad" runat="server" Text="Label"></asp:Label>
                                                        <label>
                                                        Sexo:
                                                        </label>
                                                        <asp:Label ID="lblSexo" runat="server" Text="Label"></asp:Label>
                                                         &nbsp;  &nbsp;
                                                            <asp:LinkButton ID="lnkValidarRenaper" runat="server" Visible="false" ToolTip="Validar Persona con Renaper" OnClientClick="SelRenaper(); return false;" OnClick="lnkValidarRenaper_Click">
                                             <span class="glyphicon glyphicon-registration-mark"></span>Validar con Renaper</asp:LinkButton> 
                                                     
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td> <label>
                                                        Obra social:
                                                        </label>
                                                       <asp:Label ID="lblObraSocial" runat="server" Text="Label"></asp:Label>
                                                         <%--   <asp:Button ID="btnSelObraSocial"  ToolTip="" runat="server" Text="O.S"  CssClass="btn btn-primary" 
                                                                Width="50px"   
                       OnClientClick="SelObraSocial(); return false;" OnClick="btnSelObraSocial_Click" />--%>

                                                          <asp:LinkButton ID="btnSelObraSocial" runat="server" Visible="false" ToolTip="Cambiar O.S" OnClientClick="SelObraSocial(); return false;" OnClick="btnSelObraSocial_Click"    Width="40px" CausesValidation="False" >
                                             <span class="glyphicon glyphicon-search"></span></asp:LinkButton>
                                                        <anthem:Label ID="lblAlertaObraSocial" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="Label" Visible="false"></anthem:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><label>Teléfono Contacto</label>:
                                                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control input-sm" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                            
                                                       
                                                        &nbsp;</td>
                                                </tr>
                                            
                                            </table>
                                        </asp:Panel>
                        </div>

				<div class="panel-body">
                 <table style="width: 1100px;" 
                      align="left">
					<tr>
						<td style="vertical-align: top" rowspan="6"  > 
						
                                     
						
                                     </td>
						<td colspan="2"  >					
						 

                            <table style="width:100%;border-spacing:5px;border-collapse: separate;">
                            
                           
                              

                                <tr>
            
                                
                                  <td class="myLabelIzquierda">
                                   Fecha Protocolo:  </td>
                                    <td>
                                        
                                        <asp:Label ID="lblIdPaciente" runat="server" Text="Label" Visible="False"></asp:Label>
                               
           
                                     
                    <input id="txtFecha" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; position=absolute; z-index=0;"  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>
 
               </td>
                                    <td class="myLabelIzquierda" align="right">
                                        
                                        Fecha Orden: </td>
                                    <td  >
                                        
                                        <input id="txtFechaOrden" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  /></td>
                                     <td class="myLabelIzquierda">
                                        Fecha Toma Muestra:</td>
                                    <td>
                                        
                    <input id="txtFechaTomaMuestra" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="2" class="form-control input-sm"
                                style="width: 100px"  /></td>
                                      <td class="myLabelIzquierda" align="right">
                                         Nro. Origen-Laboratorio: </td>
                                      <td >
                                      <asp:TextBox ID="txtNumeroOrigen" runat="server" class="form-control input-sm" Width="120px" 
                                            TabIndex="3" MaxLength="50"></asp:TextBox>
                                        </td>
                                    
                        <td rowspan="3">
                         
                        </td>
                                </tr>
                                <tr>
                                    <td class="myLabelIzquierda">
                                                                                Efector Solicitante:       </td>
                                    <td   colspan="3">

                            <anthem:DropDownList ID="ddlEfector" runat="server" Width="310px"
                                ToolTip="Seleccione el efector" TabIndex="4" class="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlEfector_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
					                 
                                        
                                            </td>
                                     <td class="myLabelIzquierda">
                                            Médico Solicitante (MP):</td>
                                    <td colspan="4">
                                           <anthem:TextBox ToolTip="Ingrese la mátricula" ID="txtEspecialista" Width="80px" TabIndex="5" class="form-control input-sm" runat="server"   AutoCallBack="true" OnTextChanged="txtEspecialista_TextChanged"  ></anthem:TextBox> 
                            <anthem:DropDownList ID="ddlEspecialista" runat="server"  AutoCallBack="true"
                              TabIndex="6" class="form-control input-sm" Width="280px">
                               
                            </anthem:DropDownList>
                                        
                         
                                        <asp:LinkButton ID="LinkButton1"  Width="60px" CssClass="btn btn-primary" runat="server" OnClientClick="SelMedico(); return false;" OnClick="Button1_Click"> <span class="glyphicon glyphicon-zoom-in"></span></asp:LinkButton>
                                             
                                        
                          
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="myLabelIzquierda">
                                        Origen/Servicio:   <asp:RangeValidator ID="rvOrigen" runat="server" 
                                ControlToValidate="ddlOrigen" ErrorMessage="Origen" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                        </td><td   colspan="3">
                            <anthem:DropDownList ID="ddlOrigen" runat="server"  
                                ToolTip="Seleccione el origen" TabIndex="7" class="form-control input-sm">
                            </anthem:DropDownList>                                        
                                        <asp:DropDownList ID="ddlSectorServicio" class="form-control input-sm" runat="server" TabIndex="8" Width="180px">
                                        </asp:DropDownList>
                                        
					                    <asp:RangeValidator ID="rvSectorServicio" runat="server" 
                                ControlToValidate="ddlSectorServicio" ErrorMessage="Sector" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                        
                                            </td>
                                     <td class="myLabelIzquierda">
                                        Prioridad:</td>
                                        <td>
                            <asp:DropDownList ID="ddlPrioridad" runat="server" 
                                ToolTip="Seleccione la prioridad" TabIndex="9" class="form-control input-sm">
                            </asp:DropDownList>
					                    <asp:RangeValidator ID="rvPrioridad" runat="server" 
                                ControlToValidate="ddlPrioridad" ErrorMessage="Prioridad" MaximumValue="999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                      
                                        </td>
                                      <td class="myLabelIzquierda" align="right">  
                                    <asp:Label ID="lblSalaCama" runat="server" >     Sala/Cama:                        </asp:Label>
                                        </td>
      
                                      <td  >  
                                      <asp:TextBox ID="txtSala" runat="server" class="form-control input-sm" Width="50px" 
                                            TabIndex="10" MaxLength="50"></asp:TextBox>
                                        <asp:TextBox ID="txtCama" runat="server" class="form-control input-sm" Width="50px" 
                                            TabIndex="11" MaxLength="50"></asp:TextBox>                                           
                                      
                                        </td>
      
                                  

                                </tr>
                               
                               	<tr>
						<td colspan="4"  >  
						<asp:Panel CssClass="myLabelIzquierda" runat="server" ID="pnlMuestra" Width="100%">
						

                          Muestra a Analizar: &nbsp; &nbsp; 
                            <anthem:TextBox ID="txtCodigoMuestra" Width="50px" TabIndex="12" class="form-control input-sm" runat="server"  ontextchanged="txtCodigoMuestra_TextChanged" AutoCallBack="true"></anthem:TextBox> 
                            <anthem:DropDownList ID="ddlMuestra" runat="server" TabIndex="13"  AutoCallBack="true"  onselectedindexchanged="ddlMuestra_SelectedIndexChanged"  class="form-control input-sm" >
                            </anthem:DropDownList>
                            <anthem:RangeValidator ID="rvMuestra" runat="server"     ErrorMessage="Muestra" 
                                ControlToValidate="ddlMuestra" Enabled="False" MaximumValue="9999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                            
                            </asp:Panel>
						</td>
<td    >  
 <h4>  <label class = "label label-danger"> Caracter SISA: </label></h4>
					                
    </td>
                                       <td colspan="1" class="myLabelIzquierda"><anthem:DropDownList ID="ddlCaracter" runat="server" 
                                ToolTip="Seleccione el caracter" TabIndex="14" class="form-control input-sm">
                            </anthem:DropDownList></td>
                                        <td colspan="1"  >
                                   <h4>    <label class = "label label-danger">     Nro. Hisopado</label></h4></td>
                                       <td colspan="1" class="myLabelIzquierda">
                                      <asp:TextBox ID="txtNumeroOrigen2" runat="server" class="form-control input-sm" Width="140px" 
                                            TabIndex="15" MaxLength="50" BackColor="#FFFF99" Font-Bold="True" Font-Size="14pt"></asp:TextBox>
                                            </td>
					</tr>                                                          
                               	
                            </table>
                       
                                        
                                            </td>
					</tr>
					<tr>
						<td colspan="2" ><hr />
                                                        <anthem:Label ID="lblErrorMedico" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="#CC3300" Text="Label" Visible="False"></anthem:Label>
                                                    </td>
					</tr>
					
					
					
					<tr>
						<td colspan="2"  >
					
						
					<div id="tabContainer"  style="width: 1050px; z-index:1; position:relative;" >  
						      <ul class="nav nav-pills">
    <li><a href="#tab1">Analisis</a></li>    
    <li><a href="#tab2">Diagnósticos<img alt="tiene diagnostico" runat="server" id="diag" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></a></li>
   <li  id="tab3Titulo" runat="server"><a href="#tab3" >Etiquetas</a></li> 
       
    <li id="tituloCalidad" runat="server"><a  href="#tab5">Incidencias<img alt="tiene incidencias" runat="server" id="inci" visible="false" style="border:none;" src="~/App_Themes/default/images/red_pin.gif" /></a></li>
    

</ul>
                          
                            <div id="tab1" style="height: 350px">
                   

                                 <table style="width:1000px;">
                                <tr>
                                    <td>
                                  <table cellpadding="1" cellspacing="0" runat="server" id="tableTitulo" class="tituloCelda">
                    <tr >
                        <td style="width: 10px; height: 13px">
                        </td>
                        <td style="width: 50px;" >
                            Codigo</td>
                        <td style="width: 350px;"  >
                            Descripcion</td>
                        <td style="width: 80px; "  >
                            S/ Muestra</td>                    
                        <td style="width: 18px;" >
                        </td>
                    </tr>
                </table>
                
                <div  onkeydown="enterToTab(event)" style="width:545px;height:180pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                       
                    <table  class="mytablaIngreso"  border="0" id="tabla" cellpadding="0" cellspacing="0" >
	  	                <tbody id="Datos" >
		                    
		                </tbody>
	                </table>
	                
                    <input type="hidden" runat="server" name="TxtCantidadFilas" id="TxtCantidadFilas" value="0" />
                      <input type="hidden" runat="server" name="CodOS" id="CodOS" value="0" />
                </div>
                      <div> <asp:CheckBox ID="chkRecordarPractica" runat="server" CssClass="myLabelGris"
                                Text="Recordar análisis" />
                         
                          
                                             
                            
						
                                       
                                                 
                                </div>                 
                                      </td>
                                    <td style="vertical-align: top">
                                       
                                       <fieldset id="Fieldset3" title="Analisis" style="width:95%;text-align:left;">
                                   
                                       <table> 

                                         <tr><td  >  <anthem:DropDownList ID="ddlRutina" runat="server" AutoCallBack="True"                                                
                                                
                               class="form-control input-sm" TabIndex="20" 
                                onselectedindexchanged="ddlRutina_SelectedIndexChanged">
                                          </anthem:DropDownList>
                                                 <anthem:LinkButton ID="lnkAgregarRutina" runat="server" ToolTip="Agregar Rutina" OnClientClick="javascript:AgregarDeLaListaRutina();"  Width="40px" >
                                             <span class="glyphicon glyphicon-ok-sign"></span></anthem:LinkButton>
                                            


                                </td></tr>
                                

                                       <tr><td  >	<anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True" Width="250px"
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" 
                                                TextDuringCallBack="Cargando ..." 
                               class="form-control input-sm" TabIndex="20">
                                            </anthem:DropDownList>	                                                   
                          

                                             <anthem:LinkButton ID="lnkAgregarItem" runat="server" ToolTip="Agregar Determinacion" OnClientClick="javascript:AgregarDeLaLista();"  Width="40px" >
                                             <span class="glyphicon glyphicon-ok-sign"></span></anthem:LinkButton>

                                           </td>
                                       
                                       </tr>
                                      
                                       </table>
                                       </fieldset>
                                        </td>
                                </tr>
                                </table>  <div>
                                   
                            Impresora de Etiquetas:   <asp:DropDownList    class="form-control input-sm" ToolTip="Seleccione Impresora de Etiqueta" ID="ddlImpresoraEtiqueta" runat="server">
                                             </asp:DropDownList>
                                </div>
                                <input type="hidden" runat="server" name="TxtDatosCargados" id="TxtDatosCargados" value="" />                                
                                   <input type="hidden" runat="server" name="TxtDatos" id="TxtDatos" value="" />                                
                <input id="txtTareas" name="txtTareas" runat="server" type="hidden"  />

                                  
                            </div>
                          
                            <div id="tab2" style="height: 350px">
                           
                              
                               
                                <table>
                                <tr>
                                <td rowspan="4" style="vertical-align: top">
                                    
					 
                                     <table align="left" width="100%">
                                         <tr>
                                           <td   colspan="3"  >
                                               Codigo:
                                                 <br />
                                                 <anthem:TextBox ID="txtCodigoDiagnostico" runat="server" AutoCallBack="True" 
                                                         class="form-control input-sm" Width="100px"  ontextchanged="txtCodigoDiagnostico_TextChanged"></anthem:TextBox>
                                             </td>  
                                           
                                         </tr>
                                           <tr>
                                           <td class="myLabelIzquierda" colspan="3"  >
                                            Nombre:  <br /><anthem:TextBox ID="txtNombreDiagnostico" runat="server" AutoCallBack="True" 
                                                      class="form-control input-sm"  ontextchanged="txtNombreDiagnostico_TextChanged" 
                                                   Width="268px"></anthem:TextBox>
                                               <br />
                                               </td>
                                           </tr>
                                         
                                           <tr>
                                           <td class="myLabelIzquierda" colspan="4"  >
                                              <anthem:Button ID="btnBusquedaDiagnostico"   runat="server" Text="Buscar" CssClass="btn btn-primary" Width="100px"
                                                   onclick="btnBusquedaDiagnostico_Click" />
                                               </td>
                                         </tr>
                                      
                                         
                                        
                                         <tr>
                                           <td    colspan="4" >
                              <anthem:Button ID="btnBusquedaFrecuente" CssClass="btn btn-danger" Width="140px" runat="server" Text="Ver Frecuentes" 
                                                   onclick="btnBusquedaFrecuente_Click" />
                                               <br />
                                               <br />
                              <anthem:Button ID="btnRecordarDiagnostico" CssClass="btn btn-danger" Width="160px" runat="server" Text="Cargar Ultimos Dx" 
                                                   onclick="btnRecordarDiagnostico_Click" />
                                               <br />
                                             </td>                                          
                                               
                                         </tr>
                                        
                                        </table>
                                      
                                      	                                       
                                     
                                 </td>
                                             <td class="auto-style1">                                               
                                            
                                               Diagnósticos encontrados<anthem:ListBox ID="lstDiagnosticos" runat="server" AutoCallBack="True" 
                                                     class="form-control input-sm"  Height="100px" Width="750px">
                                               </anthem:ListBox>
                                              
                                             </td>                                         
                                             <td style="vertical-align: top">
                                            
                                                 <br />
&nbsp;<anthem:ImageButton ID="btnAgregarDiagnostico" runat="server" ToolTip="Agregar diagnostico"
                                                     ImageUrl="~/App_Themes/default/images/añadir.jpg" 
                                                     onclick="btnAgregarDiagnostico_Click1" />
                                      	                                       
                                     
                                             </td>   
                                
                                </tr>
                                <tr>
                                             <td class="auto-style1">
                                                 <p  >Diagnósticos del Paciente</p>
                                                 </td>                                         
                                             <td style="vertical-align: top">
                                                 &nbsp;</td>   
                                
                                </tr>
                                <tr>
                                             <td class="auto-style1">
                                                 <anthem:ListBox ID="lstDiagnosticosFinal" runat="server"    class="form-control input-sm"
                                                     Height="100px" Width="650px" SelectionMode="Multiple">
                                                 </anthem:ListBox></td>                                         
                                             <td style="vertical-align: top">
                                    
                                                 <anthem:ImageButton ID="btnSacarDiagnostico" runat="server" 
                                                     ImageUrl="~/App_Themes/default/images/sacar.jpg" ToolTip="Sacar Diagnostico"
                                                     onclick="btnSacarDiagnostico_Click" />
                                    
                                     
                                             </td>   
                                
                                </tr>
                                <tr>
                                             <td class="auto-style1">
                                               <div>  &nbsp;<anthem:Label ID="lblFechaFIS" runat="server" Text="FIS:"></anthem:Label>
                                                                                           
                                        &nbsp; <input id="txtFechaFIS" runat="server" type="text" maxlength="10" 
                        style="width: 120px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  />
                                                   <asp:CheckBox ID="chkSinFIS" runat="server" Text="Sin informar" />
                                                   &nbsp;&nbsp;&nbsp;&nbsp;
                                                  <anthem:Label ID="Label1" runat="server" Text="FUC:"></anthem:Label>
                                                            &nbsp;                                
                                        <input id="txtFechaFUC" runat="server" type="text" maxlength="10" 
                        style="width: 120px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm"  />  <asp:CheckBox ID="chkSinFUC" runat="server" Text="Sin informar" />
                                               </div></td>                                         
                                             <td style="vertical-align: top">
                                                 &nbsp;</td>   
                                
                                </tr>
                                </table>
                                        
                                                                                                                 
                            </div>
                            
                            
                                
                           <div id="tab3"   >
                                  <%--<div runat="server" id="pnlComprobantePaciente"  visible="true">
                                 <table style="width: 100%;">
                                    <tr>
                                         <td class="myLabelIzquierda" style="vertical-align: top; width: 222px;">
                                             <asp:Label ID="lblImprimeComprobantePaciente"    runat="server" Text="Comprobante para el paciente:&nbsp;"></asp:Label>
                                       
                                         </td>
                                         <td  style="vertical-align: top;">
                                             <asp:CheckBox ID="chkImprimir" runat="server" CssClass="myLabel"  Text="Imprimir" />
                                         </td>
                                         <td  class="myLabelIzquierda">
                                             <asp:DropDownList    class="form-control input-sm"   ToolTip="Seleccione Impresora" ID="ddlImpresora" runat="server">
                                             </asp:DropDownList>
                                         </td>
                                         <td><anthem:LinkButton CssClass="myLink" ID="lnkReimprimirComprobante" onclick="lnkReimprimirComprobante_Click" runat="server">Reimprimir Comprobante</anthem:LinkButton>
                                         </td>
                                     </tr>
                                       </table> 
                                     </div>--%>
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
                                
                                              
                                         <anthem:Button  CssClass="btn btn-danger" Width="100px" ID="btnReimprimirCodigoBarras"  onclick="lnkReimprimirCodigoBarras_Click" runat="server" ValidationGroup="9" Text="Reimprimir " /></anthem:Button>
                                              <br />
                                              <anthem:Label ID="lblMensajeImpresion" runat="server" ForeColor="#FF3300" Font-Size="8pt"></anthem:Label>
                                        </div>
                                             <anthem:CheckBox ID="chkRecordarConfiguracion" Visible="false" runat="server" class="form-control input-sm" Text="Recordar ésta configuracion" />
                                                                                     


                                             <anthem:CheckBox ID="chkCodificaPaciente" runat="server" class="form-control input-sm"
                                                 Text="Codificar datos del paciente en todas las etiquetas" 
                                                 ForeColor="#CC3300" Visible="False" />
                                         </asp:Panel>
                             
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
							<td colspan="2" style="vertical-align: top" class="myLabelIzquierda" >  
                                   
                            

                                Observaciones Internas:  						
                                    <asp:TextBox ID="txtObservacion" runat="server" class="form-control input-sm" 
                                                TextMode="MultiLine"  TabIndex="23" Width="600px"  ></asp:TextBox> 
                                Notificar Resultados: 
                                          <asp:CheckBox ID="chkNotificar" runat="server" class="form-control input-sm"/>
                                     <input id="hidToken" type="hidden" runat="server" />
                                
						    <anthem:TextBox ID="txtCodigo" runat="server" BorderColor="White" ForeColor="White" 
                                BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>  <asp:CustomValidator ID="cvValidacionInput" runat="server" 
                                                ErrorMessage="Debe completar al menos un analisis" 
                                    ValidationGroup="0" Font-Size="12pt" onservervalidate="cvValidacionInput_ServerValidate" 
                                             ></asp:CustomValidator>
                                              <anthem:TextBox ID="txtCodigosRutina"  runat="server" BorderColor="White" 
                                ForeColor="White" BackColor="White" BorderStyle="Solid" BorderWidth="0px"></anthem:TextBox>						
						
							</td>
						</tr>																
						
						
						
					
                
						
						</table>
                         </div>
       
                               <div class="panel-footer">
                                   <table width="100%">
                                       		
					<tr>
						
						<td align="left" >
						
                                            <asp:Button ID="btnCancelar" runat="server" Text="Regresar" 
                                                onclick="btnCancelar_Click"  CssClass="btn btn-primary"  TabIndex="25" 
                                                CausesValidation="False" Width="120px" />                                                                                          
                                         
                                                 
                        </td>
						
						<td  align="right">
						
                                               <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" AccessKey="s" CausesValidation="true"
                                          ToolTip="Alt+Shift+S: Guarda Protocolo"  onclick="btnGuardar_Click" CssClass="btn btn-primary" Width="80px" TabIndex="24"  /></td>
						
					</tr>
                                   </table>		
                                   </div>
       </div>  
          
			</div>
  </td>
      <td style="vertical-align: top">
            
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
         

            //Antes
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
         //   oDesde.disabled = 'true';
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
                               
                            		        
		         if  (sItem[2]=='True') 
	                document.getElementById( 'Codigo_'+con).className='codigoConResultado';
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

     var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();
     function SelObraSocial() {
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
         $('<iframe src="ObraSocialSel.aspx?idPaciente=' + idPaciente + '" />').dialog({
             title: 'Financiador del Protocolo',
             autoOpen: true,
             width: 550,
             height: 400,
             modal: true,
             resizable: false,
             autoResize: true,
                buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnSelObraSocial))%>; }               
            },
             overlay: {
                 opacity: 0.5,
                 background: "black"
             }
         }).width(600);
     }


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



     function SelMedico() {
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
         $('<iframe src="MedicoSel.aspx?id='+idPaciente+'" />').dialog({
             title: 'Buscar Médico',
             autoOpen: true,
             width: 620,
             height: 400,
             modal: true,
             resizable: false,
             autoResize: true,
           open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },
                 
          buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.LinkButton1))%>; }               
            },
             overlay: {
                 opacity: 0.5,
                 background: "black"
             }
         }).width(600);
     }
  

    </script>
   
 
 </asp:Content>

