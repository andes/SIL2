<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Derivados2.aspx.cs" Inherits="WebLab.Derivaciones.Derivados2" MasterPageFile="~/Site1.Master" %>


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
  &nbsp;<div align="left" style="width: 600px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">   <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>      </h3>
                        </div>
          				<div class="panel-body">	
                 <table  width="550px" align="center" 
                   class="myTabla">
					
					<tr class="mt-1">
						<td class="myLabelIzquierda">Servicio: <br /></td>
						<td>
                              <asp:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" CssClass="form-control input-sm" 
                                onselectedindexchanged="ddlServicio_SelectedIndexChanged" 
                                  AutoPostBack="True">
                            </asp:DropDownList>
                                        
                                            </td>
					</tr>
					<tr >
						<td class="myLabelIzquierda">Area:</td>
						<td class="pt-4">
                            <asp:dropdownlist ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="2" CssClass="form-control input-sm">
                            </asp:dropdownlist>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Fecha Desde:<asp:RequiredFieldValidator ID="rfvFechaDesde" 
                                runat="server" ControlToValidate="txtFechaDesde" ErrorMessage="Fecha Desde" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
						</td>
						<td>
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width:100px" title="Ingrese la fecha de inicio"  /></td>
					</tr>
						<tr>
						<td class="myLabelIzquierda">Fecha Hasta:<asp:RequiredFieldValidator ID="rfvFechaHasta" 
                                runat="server" ControlToValidate="txtFechaHasta" ErrorMessage="Fecha Hasta" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						<td>
                    <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  /></tr>
						<tr>
						<td class="myLabelIzquierda">Origen:</td>
						<td>
                            <asp:DropDownList ID="ddlOrigen" runat="server" 
                                ToolTip="Seleccione el origen" TabIndex="5" CssClass="form-control input-sm">
                            </asp:DropDownList>
                        </td>                      
					</tr>
					<tr>
						<td class="myLabelIzquierda">Prioridad:</td>
						<td>
                            <asp:DropDownList ID="ddlPrioridad" runat="server" 
                                ToolTip="Seleccione la prioridad" TabIndex="6" CssClass="form-control input-sm">
                            </asp:DropDownList>
                         </td>          
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top" colspan="2"><hr /></td>
                        </tr>
						<tr>
						<td class="myLabelIzquierda" style="vertical-align: top">Efector a Derivar:<asp:RangeValidator 
                                ID="RangeValidator1" runat="server" ControlToValidate="ddlEfector" 
                                ErrorMessage="Efector" MaximumValue="99999" MinimumValue="1" Type="Integer" 
                                ValidationGroup="0">*</asp:RangeValidator>
                            </td>
						<td>
                            <anthem:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="6" CssClass="form-control input-sm" 
                                AutoCallBack="True" onselectedindexchanged="ddlEfector_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
					    </td>
                         </tr>
                        <tr>
						<td class="myLabelIzquierda" style="vertical-align: top">Determinacion:</td>
						<td>
                           <anthem:DropDownList ID="ddlItem" runat="server" CssClass="form-control input-sm" >
                                            </anthem:DropDownList></td>
                            </tr>
						<tr>
						<td class="myLabelIzquierda">Estado:</td>
						<td>
                            <asp:RadioButtonList ID="rdbEstado" runat="server" RepeatDirection="Vertical" CellSpacing="15" CellPadding="15" Width="200"
                                TabIndex="12">
                                <%--<asp:ListItem Value="0" Selected="True">Pendientes de enviar</asp:ListItem>
                                <asp:ListItem Value="1">Enviados</asp:ListItem>
                                <asp:ListItem Value="2">Marcado como No enviados</asp:ListItem>--%>
                            </asp:RadioButtonList>
                                        
					  </tr>
						<tr>
						<td   colspan="2">
                            <hr />
                            <asp:CustomValidator ID="cvBotonBuscar"  runat="server" ValidationGroup="0"   OnServerValidate="cvBotonBuscar_ServerValidate"/>


						</td> 

						</tr>
						<tr>
						<td   colspan="2" align="right">
                                           <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" Width="100"
                                               onclick="btnBuscar_Click" Text="Buscar" ValidationGroup="0" />
                                           </td> 
						</tr>
                    

						<tr>
						<td   colspan="2">
                                           <asp:Panel ID="pnlHojaTrabajo" runat="server" Visible="False">
                                            <img alt="" src="../App_Themes/default/images/pdf.jpg"/>&nbsp;<asp:LinkButton 
                            ID="lnkPDF" runat="server" CssClass="myLittleLink" onclick="lnkPDF_Click" ValidationGroup="0" 
                                                   TabIndex="8" Visible="False">Visualizar 
                                               en formato pdf</asp:LinkButton><br />
                         <img alt="" src="../App_Themes/default/images/imprimir.jpg"/>&nbsp;<asp:LinkButton 
                            ID="lnkImprimir" runat="server" CssClass="myLittleLink" onclick="lnkImprimir_Click" 
                                               ValidationGroup="0" TabIndex="9" Visible="False">Imprimir</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                           </asp:Panel>
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los siguientes datos:" ShowMessageBox="true" 
                                                ValidationGroup="0" ShowSummary="false" />
                        </td>
						
					</tr>
					</table>	
                              
                              </div>
          </div>
      					
 </div>
 </asp:Content>