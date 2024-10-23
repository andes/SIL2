<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudList.aspx.cs" Inherits="WebLab.Neonatal.SolicitudList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
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
        <table  width="1100px" cellpadding="1" cellspacing="1" >
					
					
					<tr>
						<td                                              
                           align="left" style="width: 9px"  >
                                &nbsp;</td>
						<td                                              
                           align="left" colspan="2">
                                &nbsp;</td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left" style="width: 9px"  >
                                &nbsp;</td>
						<td                                              
                           align="left" colspan="2">
                                &nbsp;</td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left" style="width: 9px"  >
                                &nbsp;</td>
						                                          
                           <td class="mytituloTabla" colspan="2">
                               SOLICITUDES DE PESQUISA NEONATAL
					    </td>
					</tr>
						
					
					
					<tr>
						<td align="left" style="width: 9px"  >
                                &nbsp;</td>
						<td align="left" style="font-weight: bold; font-size: 14px" colspan="2">
                                <hr /></td>
					</tr>
					
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 103px">
                            Zona/Efector:</td>
						<td class="myLabelDerecha" style="width: 884px">
                            <asp:DropDownList ID="ddlZona" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="ddlZona_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlEfector" runat="server">
                            </asp:DropDownList></td>
						</tr>
						
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 103px">
                            Fecha desde:</td>
						<td class="myLabelDerecha" style="width: 884px">
                            <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="9" class="myTexto" 
                                style="width: 70px"  /> </td>
						</tr>
						
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 103px">
                            Fechas hasta:</td>
						<td class="myLabelDerecha" style="width: 884px">
                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="9" class="myTexto" 
                                style="width: 70px"  /> </td>
						</tr>
                    
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td style="width: 103px">
                            &nbsp;</td>
						<td style="width: 884px">
                            &nbsp;</td>
					</tr>
						
					
				
					
				
						
					
					
					
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td align="left">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                                onclick="btnBuscar_Click" CssClass="myButton" />
                        </td>
						<td align="right">
                            <asp:Button ID="btnNueva" runat="server" Text="Nueva Solicitud" 
                                onclick="btnNueva_Click" CssClass="myButton" Width="120px" />
                        </td>
						</tr>
						
					
				
					
				
						
					
					
					
					<tr>
						<td style="width: 9px"  >
                            </td>
						<td colspan="2">
                         <hr /></td>
						</tr>
						
					
				
					
				
						
					
					
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td colspan="2">
						 <div align="left" style="overflow:scroll;overflow-x:hidden; height: 600px; width:100%">
                            <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                CssClass="myLabelIzquierda" BackColor="White" BorderColor="#3A93D2" 
                                BorderStyle="Groove" BorderWidth="1px" CellPadding="4" DataKeyNames="Numero" 
                                onrowcommand="gvLista_RowCommand" 
        onrowdatabound="gvLista_RowDataBound"
                                EmptyDataText="No se encontraron datos para los filtros de busqueda ingresados." 
                                Width="98%">
                                <RowStyle BackColor="White" ForeColor="#333333" />
                                <EmptyDataRowStyle ForeColor="#CC3300" />
                                <Columns>
                                    <asp:BoundField HeaderText="Nro." DataField="Numero" />
                                     <asp:BoundField HeaderText="Nro.Origen" DataField="numeroOrigen" />
                                    <asp:BoundField HeaderText="Efector" DataField="Efector" />
                                    <asp:BoundField HeaderText="DNI RN" DataField="DNI RN" />
                                    <asp:BoundField HeaderText="DNI Madre" DataField="DNI Madre" />
                                    <asp:BoundField HeaderText="Apellido Materno" DataField="Apellido Materno" />
                                    <asp:BoundField HeaderText="Apellido Paterno" DataField="Apellido Paterno" />
                                    <asp:BoundField HeaderText="Fecha Nac." DataField="Fecha Nac." />
                                    <asp:BoundField DataField="Hora Nac." HeaderText="Hora Nac." />
                                    <asp:BoundField DataField="Fecha Registro" HeaderText="Fecha Registro" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                    <asp:BoundField DataField="Protocolo" HeaderText="Protocolo" />
                                    <asp:TemplateField HeaderText="Ver Solicitud" >
                                        <ItemTemplate>
                                        <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                                        CommandName="Editar" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Protocolo" >
                                        <ItemTemplate>
                                        <asp:ImageButton ID="Protocolo" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                                        CommandName="Editar" />
                                        </ItemTemplate>                          
                                        <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />                          
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            </div>
                        </td>
						</tr>
						
					
				
					
				
						
					
					
					<tr>
						<td style="width: 9px"  >
                            &nbsp;</td>
						<td colspan="2">
                            &nbsp;</td>
						</tr>
						</table>
											
 
</asp:Content>