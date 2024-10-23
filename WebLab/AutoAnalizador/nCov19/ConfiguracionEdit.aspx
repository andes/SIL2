<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.nCov19.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  
	<br />   &nbsp;
    	<div align="left" style="width: 750px">
		
<table align="center" width="700" >
<tr>
						<td class="myLink" colspan="2" >
                            </td>
						
					</tr>
<tr>
						<td class="mytituloGris" colspan="2" >
                                  Configuración LIS - nCov19<hr /></td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Area:</td>
						<td style="width: 475px">
                            <anthem:DropDownList ID="ddlArea" runat="server" CssClass="myList" 
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></anthem:DropDownList>
                                        
                        </td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Determinación:<asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                        </td>
						<td style="width: 475px">
                                        
                            <anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" 
                                
                               >
                            </anthem:DropDownList>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Resultado:<asp:RangeValidator ID="RangeValidator2" runat="server" 
                                ControlToValidate="ddlItemResultado" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                                                </td>
						<td style="width: 475px">
                                        
                            <anthem:DropDownList ID="ddlItemResultado" runat="server" 
                                
                               >
                            </anthem:DropDownList>
                                        
                                                </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top; width: 115px;" >
                                            Resultado del Equipo:<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="txtResultado" ErrorMessage="*" ValidationGroup="0"></asp:RequiredFieldValidator>
                        </td>
						<td style="width: 475px">
                            <asp:TextBox ID="txtResultado" runat="server" Width="400px" 
                                MaxLength="150"></asp:TextBox>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td style="width: 475px">
                                              <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                                  onclick="btnGuardar_Click2" CssClass="myButton" ValidationGroup="0" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                              <asp:Label ID="lblMensajeValidacion" runat="server" 
                                                  ForeColor="#FF3300"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                           <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
					
						<div style="border: 1px solid #999999; height: 600px; width:610px; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idResultadoItemNcov" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound" 
                                    onselectedindexchanged="gvLista_SelectedIndexChanged" 
                                    EmptyDataText="No se configuraron prácticas para el equipo.">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="Resultado" HeaderText="Resultado" />
                                        <asp:BoundField DataField="resultadoEquipo" HeaderText="Resultado en Equipo" />
                                                    
                                                    
                                        <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" 
                                                        ImageUrl="../../App_Themes/default/images/eliminar.jpg" 
                                                        OnClientClick="return PreguntoEliminar();" />
                                                </ItemTemplate>
                                                <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                    </Columns>
                                    
                                    <HeaderStyle BackColor="#999999" />
                                </asp:GridView>
					</div>
					
					    </td>
						
					</tr>
					</table>
		
		</div>				      
                            
                      
 <script language="javascript" type="text/javascript">

    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar?'))
    return true;
    else
    return false;
    }
    </script>
 </asp:Content>
