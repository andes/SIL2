<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PorResultado.aspx.cs" Inherits="WebLab.Estadisticas.PorResultado" MasterPageFile="~/Site1.Master" %>

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


                 <table  width="650px" align="center" 
                     
                     
                    
                     cellpadding="1" cellspacing="1" class="myTabla">
					<tr>
						<td colspan="2"><b class="mytituloTabla">
                            <asp:Label ID="lblTitulo" runat="server" Text="ESTADISTICAS DE RESULTADOS"></asp:Label></b><hr /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2">
                            <anthem:RadioButtonList ID="rdbTipoInforme" runat="server" 
                                onselectedindexchanged="rdbTipoInforme_SelectedIndexChanged" 
                                AutoCallBack="True" CssClass="myLink" RepeatDirection="Horizontal">
                              
                                <Items>
<asp:ListItem Selected="True" Value="0">Cantidad de Casos Encontrados </asp:ListItem>
<asp:ListItem Value="1">Promedio de resultado</asp:ListItem>
</Items>
                            </anthem:RadioButtonList>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2">
                            &nbsp;</td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2">
                            <anthem:Label ID="lblDescripcion" runat="server" CssClass="alert" 
                                Visible="False" Height="100%"></anthem:Label>
                                        
                                </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2"><hr /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Fecha Desde:</td>
						<td>
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="myTexto" 
                                style="width: 70px" title="Ingrese la fecha de inicio"  />&nbsp;&nbsp;&nbsp;
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Fecha Hasta:</td>
						<td>
                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="myTexto" 
                                style="width: 70px" title="Ingrese la fecha de fin"  /><asp:CustomValidator ID="CustomValidator1" runat="server" 
                                ErrorMessage="Debe ingresar un rango de fechas valido" 
                                onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0">Debe 
                            ingresar un rango de fechas valido</asp:CustomValidator>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Servicio:</td>
						<td>
                            <anthem:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" CssClass="myList" 
                                AutoCallBack="True" onselectedindexchanged="ddlServicio_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Area:</td>
						<td>
                            <anthem:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="1" CssClass="myList" 
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top" rowspan="2">Análisis:</td>
						<td>
                            <anthem:CheckBox ID="chkTodos" runat="server" AutoCallBack="True" 
                                oncheckedchanged="chkTodos_CheckedChanged" Text="Seleccionar Todos" />
                        </td>
					</tr>
					<tr>
						<td>
                            <anthem:ListBox ID="lstAnalisis" runat="server" CssClass="myTexto" 
                                Height="200px" SelectionMode="Multiple" Width="350px">
                            </anthem:ListBox>
                            <anthem:RequiredFieldValidator ID="rfvListaAnalisis" runat="server" 
                                ControlToValidate="lstAnalisis" ValidationGroup="0" 
                                ErrorMessage="Debe seleccionar al menos un analisis"></anthem:RequiredFieldValidator>
                        </td>
					</tr>
						<tr>
						<td class="myLabelIzquierda" colspan="2"><hr /></td></tr>
						<tr>
						<td colspan="2" align="right">
                           <asp:Button ID="btnGenerar" runat="server" CssClass="myButton" 
                                onclick="btnGenerar_Click" Text="Generar Reporte" Width="131px" 
                                ValidationGroup="0" />&nbsp;&nbsp;
                            </td>
                            </tr>
						<%--<tr>
						<td align="left">
                         </td>
						<td align="left">--%>
                       <%-- <img alt="" src="../App_Themes/default/images/pdf.jpg"/>&nbsp;<asp:LinkButton 
                            ID="lnkPdf" runat="server" CssClass="myLittleLink"  ValidationGroup="0" 
                                onclick="lnkPdf_Click1">En formato pdf</asp:LinkButton>
                         <br />
                             <img alt="" src="../App_Themes/default/images/imprimir.jpg"/>&nbsp;<asp:LinkButton 
                            ID="lnkImprimir" runat="server" CssClass="myLittleLink" 
                                               ValidationGroup="0" onclick="lnkImprimir_Click1">Imprimir</asp:LinkButton>
                        <br />                       
                                             <img alt="" src="../App_Themes/default/images/excelPeq.gif"/>&nbsp;     
                                               <asp:LinkButton ID="lnkExcel" 
           runat="server" CssClass="myLittleLink" 
                                                   ValidationGroup="0" 
           onclick="lnkExcel_Click">En formato Excel</asp:LinkButton>--%></td>
					<%--	<tr>
						<td class="myLabelIzquierda" colspan="2"><hr /></td>
						</tr>--%>
						<tr>
						<td  colspan="2">
                                        
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" 
                                                ValidationGroup="0" ShowSummary="False" />
                        </td>
						
					</tr>
					</table>						
 
 </asp:Content>
