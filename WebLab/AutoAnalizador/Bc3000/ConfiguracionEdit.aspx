<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.Bc3000.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   
    <style type="text/css">
        .style1
        {
            width: 618px;
        }
        .style4
        {
            width: 132px;
        }
        .style5
        {
            width: 476px;
        }
    </style>
   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">            
    <br />   &nbsp;

<div align="left" style="width: 700px">
		
<table align="center" width="600" class="style1">
<tr>
						<td class="myLink" colspan="2" >
                            </td>
						
					</tr>

<tr>
						<td class="mytituloGris" colspan="2" >
                                   Configuración LIS - Mindray BC-3000 Plus<hr /></td>
						
					</tr>
<tr>
						<td class="style4" >
                                            Area/Análisis 
                                            del LIS:</td>
						<td class="style5">
                            <anthem:DropDownList ID="ddlArea" runat="server" CssClass="myList" 
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></anthem:DropDownList>
                                        
                            <anthem:DropDownList ID="ddlItem" runat="server" CssClass="myList" 
                                
                               >
                            </anthem:DropDownList>
                                        
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="style4" >
                                            ID en Equipo:</td>                                        
						<td class="style5">
                            <asp:DropDownList ID="ddlItemEquipo" runat="server" Width="150px">
                                <asp:ListItem Selected="True" Value="0">Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">WBC</asp:ListItem>
                                <asp:ListItem Value="2">LYMPH#</asp:ListItem>
                                <asp:ListItem Value="3">MID#</asp:ListItem>
                                <asp:ListItem Value="4">GRAN#</asp:ListItem>
                                <asp:ListItem Value="5">LYMPH%</asp:ListItem>
                                <asp:ListItem Value="6">MID%</asp:ListItem>
                                <asp:ListItem Value="7">GRAN%</asp:ListItem>
                                <asp:ListItem Value="8">RCB</asp:ListItem>
                                <asp:ListItem Value="9">HGB</asp:ListItem>
                                <asp:ListItem Value="10">MCHC</asp:ListItem>
                                <asp:ListItem Value="11">MCV</asp:ListItem>
                                <asp:ListItem Value="12">MCH</asp:ListItem>
                                <asp:ListItem Value="13">RDW_CV</asp:ListItem>
                                <asp:ListItem Value="14">HTC</asp:ListItem>
                                <asp:ListItem Value="15">PLT</asp:ListItem>                                
                                <asp:ListItem Value="16">MPV</asp:ListItem>
                                <asp:ListItem Value="17">PDW</asp:ListItem>
                                <asp:ListItem Value="18">PCT</asp:ListItem>                               
                                <asp:ListItem Value="19">RDW_SD</asp:ListItem>                               
                            </asp:DropDownList>
                                        
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" 
                                ControlToValidate="ddlItemEquipo" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                                                </td>
						
					</tr>
    <%--<asp:BoundField DataField="tipoMuestra" HeaderText="Tipo de Muestra" />
                                        <asp:BoundField DataField="prefijo" HeaderText="Prefijo Especial" />--%>
					<tr>
						<td class="style4" >
                                            &nbsp;</td>
						<td class="style5">
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
					
						<div style="border: 1px solid #999999; height: 500px; width:610px; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idBcItem" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound" 
                                    EmptyDataText="No se configuraron prácticas para el equipo">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="idBc" HeaderText="ID en Equipo" />
                                       
                                           
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
                                    
                                    <HeaderStyle BackColor="#999999" />
                                </asp:GridView>
					            <br />
					</div>
					
					    </td>
						
					</tr>
					</table>
		
        <br />

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
