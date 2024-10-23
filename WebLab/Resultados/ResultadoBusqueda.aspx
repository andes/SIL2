<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoBusqueda.aspx.cs" Inherits="WebLab.Resultados.ResultadoBusqueda" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

  

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript"> 
      

	$(function() {
	    $("#<%=txtFechaDesde.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});
	$(function() {
	    $("#<%=txtFechaHasta.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});


  </script>    
  
   <%--  <asp:RequiredFieldValidator ID="rfvFechaDesde" 
                                runat="server" ControlToValidate="txtFecha" ErrorMessage="Fecha Desde" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>--%>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
   
   
    </asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
     
    
    <div  style="width: 1000px" class="form-inline" >
     <table  width="950px" align="center" >
				
					<tr>
					<td >
    
                     <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
    <h3 class="panel-title">
        <asp:Label ID="lblTitulo" runat="server" Text="CARGA DE RESULTADOS"></asp:Label>
         <asp:Image ID="imgUrgencia" ToolTip="URGENCIA" runat="server" ImageUrl="../App_Themes/default/images/urgencia.jpg" />   



        </h3>
  </div>
                    <div class="panel-body">

                 
                        <table align="left">
					
					<tr>
						<td  valign="top" rowspan="12" >
                           
                            <div class="panel panel-default" runat="server" id="Div1">
                    <div class="panel-heading">
    <asp:Label ID="lblFormaCarga" runat="server" 
                                Text="Forma de Carga" ></asp:Label>
  </div>
                    <div class="panel-body">
                        <div class="radio">
                                   <anthem:RadioButtonList ID="rdbCargaResultados" runat="server" 
                                       AutoPostBack="True" 
                                       onselectedindexchanged="rdbCargaResultados_SelectedIndexChanged" 
                                       RepeatDirection="Vertical">
                                       <Items>
                                           <asp:ListItem Value="0">Lista de Protocolos</asp:ListItem>
                                           <asp:ListItem Value="1">Hoja de Trabajo</asp:ListItem>
                                           <asp:ListItem Value="2">Analisis</asp:ListItem>
                                           <asp:ListItem Value="3">Analisis a Demanda</asp:ListItem>
                                       </Items>
                                   </anthem:RadioButtonList>
                                        </div>
                        </div>
                                </div>
                            
                            
                            <div class="panel panel-default" runat="server" id="Div2">
                    <div class="panel-heading">
    Estado 
  </div>
                    <div class="panel-body">
                                
                            <asp:RadioButtonList ID="rdbEstado" runat="server" 
                                TabIndex="13" ToolTip="Seleccione el estado de los protocolos a buscar" Width="200px">
                                <asp:ListItem Value="0" Selected="True">No procesados y en Proceso</asp:ListItem>
                                <asp:ListItem Value="9">Procesado por el Equipo</asp:ListItem>
                                <asp:ListItem Value="1">Solo Validados</asp:ListItem>
                                <asp:ListItem Value="2">Todos</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                                </div>


                                   <div class="panel panel-default" runat="server" id="divValidacion" visible="false">
                    <div class="panel-heading">
    Firma electronica
  </div>
                    <div class="panel-body" >


                            
                        
                                  <table style="width:200px;">
                                      <tr>
                                          <td align="left"><asp:Label Font-Bold="true" ID="lblUsuarioValida" runat="server"  ></asp:Label>                                </td>
                                      </tr>
                                      <tr>
                                          <td align="left">
                                              <asp:HyperLink ID="hplCambiarContrasenia"  runat="server">Cambiar Contraseña</asp:HyperLink>
                                          </td>
                                      </tr>
                                  </table>
                            
                                 </div>
                                       </div>
                             <anthem:Label ID="lblMensaje" runat="server" ForeColor="#FF3300"></anthem:Label>


                          </td>
						<td  valign="top" rowspan="8" >
                           
                            &nbsp;  &nbsp;  &nbsp;</td>
						
						<td colspan="4">
                                    <h4>          <span class="label label-default"><asp:Label ID="lblServicio" runat="server" 
                                       Text="Label"></asp:Label>
   </span>                                     <small>   <asp:Label ID="lblModoPreValidacion" runat="server" Font-Bold="True"   Text="Label" Visible="False"></asp:Label>
                                            </small>   </h4>
                          
                            <anthem:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control"
                                AutoCallBack="True" onselectedindexchanged="ddlServicio_SelectedIndexChanged" 
                                       Height="16px">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td  >
                           
                          </td>
						<td colspan="3">
                            
                                       <%--   <asp:Button ID="btnBuscar" runat="server" Text="Buscar Protocolos" 
                                                ValidationGroup="0" class="btn btn-primary" TabIndex="15" 
                                                Width="150px" onclick="btnBuscar_Click" 
                                                ToolTip="Haga clic aqui para buscar los protocolos" />--%>
                                       
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Area:<anthem:RangeValidator ID="rvArea" runat="server" 
                                ControlToValidate="ddlArea" ErrorMessage="Area" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                                        
                                            </td>
						<td colspan="3">
                            <anthem:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="2" class="form-control input-sm"
                               AutoPostBack="True"  onselectedindexchanged="ddlArea_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                            <anthem:DropDownList ID="ddlHojaTrabajo" runat="server" 
                                ToolTip="Seleccione la hoja de trabajo" TabIndex="2" class="form-control input-sm">
                            </anthem:DropDownList>
                                        
                            <anthem:RangeValidator ID="rvHojaTrabajo" runat="server" 
                                ControlToValidate="ddlHojaTrabajo" ErrorMessage="Hoja Trabajo" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                                        
                                            <anthem:Image ID="imgAgregarArea" runat="server" 
                                ImageUrl="~/App_Themes/default/images/add.png" />
                            <anthem:DropDownList ID="ddlArea2" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="2" class="form-control input-sm"
                               AutoPostBack="True" Visible="false" >
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td  class="myLabelIzquierda" style="vertical-align: top">
                            <asp:Label ID="lblAnalisis" runat="server" Text="Análisis:"></asp:Label>
                        </td>
						<td colspan="3"  >
                            <anthem:TextBox ID="txtCodigo" runat="server" class="form-control input-sm"
                               style="text-transform:uppercase"   ontextchanged="txtCodigo_TextChanged" Width="88px" AutoCallBack="True" 
                                TabIndex="4" Enabled="False"></anthem:TextBox>
                            <anthem:DropDownList ID="ddlAnalisis" runat="server" Width="400px" 
                                ToolTip="Seleccione el analisis" TabIndex="1" class="form-control input-sm"
                                Enabled="False" onselectedindexchanged="ddlAnalisis_SelectedIndexChanged" 
                                AutoCallBack="True">  </anthem:DropDownList>
                               
                            <anthem:RangeValidator ID="rvAnalisis" runat="server" 
                                ControlToValidate="ddlAnalisis" ErrorMessage="Analisis" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                                        
                        </td>
					</tr>
					
					<tr>
						<td class="myLabelIzquierda">Fecha Desde:</td>
						<td>
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de inicio de busqueda"  />
                              <%--   <asp:Button ID="btnBuscar" runat="server" Text="Buscar Protocolos" 
                                                ValidationGroup="0" class="btn btn-primary" TabIndex="15" 
                                                Width="150px" onclick="btnBuscar_Click" 
                                                ToolTip="Haga clic aqui para buscar los protocolos" />--%>
                            <asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Fechas de inicio y de fin" 
                                onservervalidate="cvFechas_ServerValidate" ValidationGroup="0">*</asp:CustomValidator>
                        &nbsp;&nbsp;
                        </td>
						<td class="myLabelIzquierda">
                            Fecha Hasta:</td>
						<td>
                             <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin de busqueda"  /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Prot. Desde:</td>
						<td>
                    <input id="txtProtocoloDesde" runat="server" type="text" maxlength="9"                        
                       tabindex="5" class="form-control input-sm" style="width: 100px"
                                onblur="javascript:valNumero(this); copiarNumero();"                        
                                title="Ingrese el numero de protocolo de inicio"  /><asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                ErrorMessage="Numero de Protocolo" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="0" 
                                Font-Size="8pt">Sólo numeros</asp:CustomValidator>
                        </td>
						<td class="myLabelIzquierda">
                            Prot. Hasta:</td>
						<td>
                    <input id="txtProtocoloHasta" runat="server" type="text" maxlength="9" 
                         tabindex="6" class="form-control input-sm" onblur="valNumero(this)" style="width: 100px"
                                title="Ingrese el numero de protocolo de fin"  /><asp:CustomValidator ID="cvNumeroHasta" runat="server" 
                                ErrorMessage="Numero de Protocolo" 
                                onservervalidate="cvNumeroHasta_ServerValidate" ValidationGroup="0">Sólo numeros</asp:CustomValidator>
                        </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Nro. de Origen:</td>
						<td>
                            <asp:TextBox ID="txtNroOrigen" runat="server" class="form-control input-sm" TabIndex="7"  style="width: 100px"
                                ToolTip="Ingrese numero de origen"></asp:TextBox>
                            
                        </td>
						<td class="myLabelIzquierda">
                            <asp:Label ID="lblPrioridad" runat="server" Text="Prioridad:"></asp:Label>
                            </td>
						<td>
                                        
                            <anthem:DropDownList ID="ddlPrioridad" runat="server" 
                                ToolTip="Seleccione la prioridad" TabIndex="9" class="form-control input-sm">
                            </anthem:DropDownList>
                                        
                        </td>
					</tr>
					
				
					
						<tr>
						<td class="myLabelIzquierda"> Efector Solicitante:</td>
						<td colspan="3">
                            <asp:DropDownList ID="ddlEfector" runat="server" Width="350px"
                                ToolTip="Seleccione el efector" TabIndex="10" class="form-control input-sm">
                            </asp:DropDownList></td>
                                        
					</tr>
						
					
					
					<tr>
						<td class="myLabelIzquierda">&nbsp;</td>
						<td class="myLabelIzquierda"><asp:Label runat="server" ID="lblOrigen"> Origen:</asp:Label></td>
						<td>
                            <asp:DropDownList ID="ddlOrigen" runat="server" 
                                ToolTip="Seleccione el origen" TabIndex="8" class="form-control input-sm">
                            </asp:DropDownList>
                            
                        </td>
						<td class="myLabelIzquierda">
                            <asp:Label id="lblDniPaciente" runat="server"> DNI Paciente:</asp:Label></td>
						<td>
                                        
                    <input id="txtDNI" runat="server" type="text" maxlength="10"                        
                       tabindex="11" class="form-control input-sm" style="width: 120px"
                                onblur="valNumero(this)"                        
                                title="Ingrese el DNI del paciente"  /><asp:CompareValidator 
                                ID="cvtxtDNI" runat="server" ControlToValidate="txtDNI" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Sólo números</asp:CompareValidator>
                            </td>
					</tr>		
						

						
					
					
					<tr>
						<td class="myLabelIzquierda">&nbsp;</td>
						<td class="myLabelIzquierda">&nbsp;</td>
						<td>
                             <asp:CheckBox ID="chkFactura" runat="server" Text="Solo Para Facturar" /> 
                        </td>
						<td class="myLabelIzquierda">
                            &nbsp;</td>
						<td>
                                        
                            &nbsp;</td>
					</tr>		
						

						
					
					
					<tr>
                        <td></td>
						<td colspan="4">


                       <h4>     <label class = "label label-danger" runat="server" visible="false" id="lblCaracterSisa" style="vertical-align: top"> Caracter-CoVid19: </label></h4>
					                <br />
    
                                            <asp:CheckBoxList ID="chkCaracterCovid" runat="server" RepeatDirection="Vertical" RepeatColumns="4">
                                           </asp:CheckBoxList>

						</td>
						 
					</tr>		
						

						
					
					
						<tr>
						<td class="myLabelIzquierda" style="height: 12px; vertical-align: top;">
                            &nbsp;</td>
						<td style="height: 12px" colspan="4">
                            <div class="panel panel-default" runat="server" id="Div3">
                    <div class="panel-heading">
  Sector/Servicio
  </div>
                    <div class="panel-body">
                                                           <asp:ListBox ID="lstSector" runat="server" 
                                class="form-control input-sm" Height="160px" 
                                                               SelectionMode="Multiple" TabIndex="12" 
                                ToolTip="Seleccione los sectores" Width="350px"></asp:ListBox>
                                                           <ul class="pagination">
                                                               <li>                                                   <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todos&nbsp;&nbsp;</asp:LinkButton></li>
                                            <li><asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguno</asp:LinkButton></li>
                                </ul>

                        </div>
                                </div>
                             </td>           
					</tr>
						
					
					
																		
						
					
					
						
		</table>
                        </div>
                        
                               <div class="panel-footer">			
					<table width="100%">
																		
						
					
					
						
					
                        <tr>
                            <td align="left">             <div class="check">
                                            <asp:CheckBox ID="chkRecordarFiltro" runat="server" 
                                Text="Recordar filtros" Checked="True" TabIndex="14" /> 
                                             </div>
                             <asp:LinkButton ID="lnkLimpiar" runat="server"  class="btn btn-warning" Width="120px" 
                                                onclick="lnkLimpiar_Click">Limpiar Filtros</asp:LinkButton></td>
						<td align="right">
                             <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="btnBuscar_Click"    Width="100px" ValidationGroup="0" >
                                             <span class="glyphicon glyphicon-search"></span>&nbsp;Buscar</asp:LinkButton>

                                         <%--   <asp:Button ID="btnBuscar" runat="server" Text="Buscar Protocolos" 
                                                ValidationGroup="0" class="btn btn-primary" TabIndex="15" 
                                                Width="150px" onclick="btnBuscar_Click" 
                                                ToolTip="Haga clic aqui para buscar los protocolos" />--%>
                                         
                        </td>
						
					</tr>
					
					<tr>
						<td colspan="2" >
                               <p>Para optimizar el proceso  de búsqueda utilice los filtros disponibles. </p>                                        
                        </td>
						</tr>
					<tr>
						<td  colspan="2" >
                                            <anthem:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los siguientes datos" ShowMessageBox="True" 
                                                ValidationGroup="0" ShowSummary="False" />
                        </td>
						
					</tr>
					
					</table>						
                                   </div>
                    </div>
                        </td>
                        </tr>

    
                        
         
    <tr>
						<td >       
                                                                 
                        </td>
						
					</tr>
</table>

</div>       
      
                     <script type="text/javascript">


  

                         function copiarNumero() {                             
                             var numerito = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtProtocoloDesde").ClientID %>').value;                             
                         //    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtProtocoloHasta").ClientID %>').value = numerito;

                         }
  </script>    
  
 </div>
 </asp:Content>