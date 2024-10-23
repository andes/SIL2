<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.Miura.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
    <style type="text/css">
        .myTexto
        {}
    </style>



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  
	<br />   &nbsp;
    	<div align="left" style="width: 700px">
		
<table align="center" width="700">
<tr>
						<td class="myLink" colspan="2" >
                            </td>
						
					</tr>
<tr>
						<td class="mytituloGris" colspan="2" >
                                    Configuración LIS - EQUIPO MIURA<hr /></td>
						
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
                                            Análisis del LIS:</td>
						<td style="width: 475px">
                                        
                            <anthem:DropDownList ID="ddlItem" runat="server" CssClass="myList" 
                                
                               >
                            </anthem:DropDownList>
                                        
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                          <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Método:<asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                                runat="server" ControlToValidate="txtOrden" ErrorMessage="*" 
                                ValidationGroup="0"></asp:RequiredFieldValidator>
                                        
                                                </td>
						<td style="width: 475px">
                            <asp:TextBox ID="txtOrden" runat="server" CssClass="myTexto" Width="124px" 
                                MaxLength="50"></asp:TextBox>
                                        
                                                </td>
						
					</tr>
  	<tr>
						<td class="myLabelIzquierda" style="vertical-align: top; width: 115px;" >
                                            Dilución:</td>
						<td class="myLabelIzquierda"  style="width: 475px">
                            <asp:TextBox ID="txtDilucion" runat="server" CssClass="myTexto" Width="107px" 
                                MaxLength="50"></asp:TextBox>
                                        
                                                </td>
						
					</tr>
  	<tr>
						<td class="myLabelIzquierda" style="vertical-align: top; width: 115px;" >
                                            Prefijo especial:</td>
						<td class="myLabelIzquierda"  style="width: 475px">
                            <asp:TextBox ID="txtPrefijo" runat="server" CssClass="myTexto" Width="60px" 
                                MaxLength="3"></asp:TextBox>
                                    &nbsp;(Sólo para casos como Glucemia Post ingesta, Glucemia Post Almuerzo, etc.)</td>
						
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
					
						<div style="border: 1px solid #999999; height: 500px; width:100%; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="99%" 
                                    DataKeyNames="idMiuraItem" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound" 
                                    EmptyDataText="No se configuraron prácticas para el equipo" 
                                    EnableModelValidation="True">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="idMiura" HeaderText="Método en Equipo" />
                                       <asp:BoundField DataField="dilucion" HeaderText="Dilucion" >
                                               
                                        <ControlStyle Font-Bold="True" />
                                        <ItemStyle Font-Bold="True" />
                                               
                                        </asp:BoundField>
                                                   <asp:BoundField DataField="prefijo" HeaderText="Prefijo" >
                                       
                                        
                                        </asp:BoundField>
                                       
                                         <asp:TemplateField HeaderText="Habilitado">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkStatus" runat="server" 
                            AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged"
                            Checked='<%# Convert.ToBoolean(Eval("Habilitado")) %>'
                            />
                    </ItemTemplate>                    
                                         
                </asp:TemplateField>
                                        
                                        <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" 
                                                                                ImageUrl="../../App_Themes/default/images/eliminar.jpg" 
                                                                                OnClientClick="return PreguntoEliminar();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                                    </asp:TemplateField>
                                    </Columns>
                                    
                                    <HeaderStyle BackColor="#BBBBBB" />
                                </asp:GridView>
					</div>
					
					    </td>
						
					</tr>
					</table>	<br />   &nbsp;
		
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
