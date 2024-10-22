<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlPlanilla.aspx.cs" Inherits="WebLab.ControlResultados.ControlPlanilla" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript"> 
      

	$(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});
	$(function() {
		$("#<%=txtFechaHasta.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});


 
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
   
   
</asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div  style="width: 800px" class="form-inline" >
      <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
    
          <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
         </div>
                    <div class="panel-body">
                 <table  width="700px" align="center" 
                     
                     
                 
                     cellpadding="1" cellspacing="1" class="myTabla">
					 
					<tr>
						<td colspan="3" >
                           
                            <asp:Label ID="lblTituloFormula" runat="server" 
                                
                                Text="El cálculo de formulas aplicará los valores calculados solo a los analisis que no tienen resultados" 
                                CssClass="title" Visible="False"></asp:Label>
                        </td>
					</tr>
				 
					<tr>
						<td class="myLabelIzquierda">Servicio:</td>
						<td colspan="2">
                            <anthem:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" CssClass="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlServicio_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Area:<anthem:RangeValidator ID="rvArea" runat="server" 
                                ControlToValidate="ddlArea" ErrorMessage="Area" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                                        
                                            </td>
						<td colspan="2">
                            <anthem:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="2" CssClass="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                            <anthem:DropDownList ID="ddlHojaTrabajo" runat="server" 
                                ToolTip="Seleccione la hoja de trabajo" TabIndex="2" CssClass="form-control input-sm">
                            </anthem:DropDownList>
                                        
                            <anthem:RangeValidator ID="rvHojaTrabajo" runat="server" 
                                ControlToValidate="ddlHojaTrabajo" ErrorMessage="Hoja Trabajo" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="3"><hr /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Fecha Desde:</td>
						<td>
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio de busqueda"  />
                              <%--  <asp:RequiredFieldValidator ID="rfvFechaDesde" 
                                runat="server" ControlToValidate="txtFecha" ErrorMessage="Fecha Desde" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>--%>
                            <asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Fechas de inicio y de fin" 
                                onservervalidate="cvFechas_ServerValidate" ValidationGroup="0">*</asp:CustomValidator>
                        </td>
						<td  class="myLabelIzquierda">
                            Sector/Servicio:</td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Fecha Hasta:</td>
						<td>
                             <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin de busqueda"  /></td>
						<td rowspan="7">
                                                           <asp:ListBox ID="lstSector" runat="server" 
                                CssClass="myTexto" Height="170px" 
                                                               SelectionMode="Multiple" TabIndex="7" 
                                ToolTip="Seleccione los sectores" Width="250px"></asp:ListBox>
                        </td>
					</tr>
						<tr>
						<td class="myLabelIzquierda">Protocolo Desde:</td>
						<td>
                    <input id="txtProtocoloDesde" runat="server" type="text" maxlength="10"                        
                       tabindex="4" class="form-control input-sm"       
                                onblur="valNumero(this)"                          style="width:120px" 
                                title="Ingrese el numero de protocolo de inicio"  />
                           
                           
                            <asp:CompareValidator ID="cvProtocoloDesde" runat="server" ControlToValidate="txtProtocoloDesde" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                                </td>
                                </tr>
						<tr>
						<td class="myLabelIzquierda">Protocolo Hasta:</td>
						<td>
                    <input id="txtProtocoloHasta" runat="server" type="text" maxlength="10" 
                         tabindex="5" class="form-control input-sm"  onblur="valNumero(this)"
                                style="width: 120px" title="Ingrese el numero de protocolo de fin"  />
                           
                           
                            <asp:CompareValidator ID="cvProtocoloHasta" runat="server" ControlToValidate="txtProtocoloHasta" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                                </td>
                                </tr>
						<tr>
						<td class="myLabelIzquierda">Origen:</td>
						<td>
                            <asp:DropDownList ID="ddlOrigen" runat="server" 
                                ToolTip="Seleccione el origen" TabIndex="6" CssClass="form-control input-sm">
                            </asp:DropDownList>
                            
                                        </td>                                        
					</tr>
					
						<tr>
						<td class="myLabelIzquierda"> Efector Solicitante:</td>
						<td>
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="7" CssClass="form-control input-sm" Width="200px">
                            </asp:DropDownList></td>
                                        
					</tr>
						
					
					
						
						
					
					
						<tr>
						<td class="myLabelIzquierda">Prioridad:</td>
						<td>
                                        
                            <anthem:DropDownList ID="ddlPrioridad" runat="server" 
                                ToolTip="Seleccione la hoja de trabajo" TabIndex="2" CssClass="form-control input-sm">
                            </anthem:DropDownList>
                                        
					</tr>
						
					
					
												
						
					
					 
					
					
																								
						
					
					
						 
					<tr>
						<td 
						 align="left" colspan="2">
                            <div class="myLinkRojo" >Para optimizar el proceso 
                            de busqueda utilice los filtros de busqueda disponibles.</div>
                             <asp:CheckBox ID="chkRecordarFiltro" runat="server" CssClass="myLabelIzquierda2" 
                                Text="Recordar filtros" Checked="True" Font-Italic="True" />  &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;  <asp:LinkButton ID="lnkLimpiar" runat="server" CssClass="myLinkRojo" 
                                                onclick="lnkLimpiar_Click">Limpiar Filtros</asp:LinkButton>
                        </td>
						
						<td   align="right">
                                            &nbsp;</td>
						
					</tr>
					
				 
					</table>	
                        
                        </div>
           <div class="panel-footer">	
               <table>
                   <tr>
						<td   colspan="2">
                                            <anthem:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los siguientes datos" ShowMessageBox="True" 
                                                ValidationGroup="0" ShowSummary="False" />
                        </td>
						
						<td  >
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar Protocolos" 
                                                ValidationGroup="0" CssClass="btn btn-primary" TabIndex="9" 
                                                Width="132px" onclick="btnBuscar_Click" />
                        </td>
						
					</tr>
               </table>
               </div>
          
          </div>
     
     					
 </div>
 </asp:Content>