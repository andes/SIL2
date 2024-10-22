<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.SysmexXT1800.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  <br />   &nbsp;
	
    	<div align="left" style="width: 700px">
		
<table align="center" width="600">
<tr>
						<td class="mytituloTabla" >
                                </td>
						
						<td class="mytituloGris" colspan="2" > <img src="../../App_Themes/default/images/sysmex.jpg" />   <br />
                                        Configuración LIS - EQUIPO Sysmex XT 1800i<hr /></td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            Area:</td>
						<td style="width: 475px">
                              <anthem:DropDownList ID="ddlArea" runat="server" CssClass="myList" 
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></anthem:DropDownList>
                                        
                        </td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
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
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            ID en Equipo:</td>
						<td style="width: 475px">
                            <asp:DropDownList ID="ddlItemEquipo" runat="server" Width="150px">
                                <asp:ListItem Selected="True" Value="0">Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">WBC</asp:ListItem>
                                <asp:ListItem Value="2">RBC</asp:ListItem>
                                <asp:ListItem Value="3">HGB</asp:ListItem>
                                <asp:ListItem Value="4">HCT</asp:ListItem>
                                <asp:ListItem Value="5">MCV</asp:ListItem>
                                <asp:ListItem Value="6">MCH</asp:ListItem>
                                <asp:ListItem Value="7">MCHC</asp:ListItem>
                                <asp:ListItem Value="8">PLT</asp:ListItem>
                                <asp:ListItem Value="9">NEUT%</asp:ListItem>
                                <asp:ListItem Value="10">LYMPH%</asp:ListItem>
                                <asp:ListItem Value="11">MONO%</asp:ListItem>
                                <asp:ListItem Value="12">EO%</asp:ListItem>
                                <asp:ListItem Value="13">BASO%</asp:ListItem>
                                <asp:ListItem Value="14">NEUT#</asp:ListItem>
                                <asp:ListItem Value="15">LYMPH#</asp:ListItem>
                                <asp:ListItem Value="16">MONO#</asp:ListItem>
                                <asp:ListItem Value="17">EO#</asp:ListItem>
                                <asp:ListItem Value="18">BASO#</asp:ListItem>
                                <asp:ListItem Value="19">RDW-SD</asp:ListItem>
                                <asp:ListItem Value="20">RDW-CV</asp:ListItem>
                                <asp:ListItem Value="21">MPV</asp:ListItem>
                                <asp:ListItem Value="22">PCT</asp:ListItem>
                                <asp:ListItem Value="23">P-LCR</asp:ListItem>
                                <asp:ListItem Value="24">IG%</asp:ListItem>
                                <asp:ListItem Value="25">IG#</asp:ListItem>
                            </asp:DropDownList>
                                        
                                                <asp:RangeValidator ID="RangeValidator2" runat="server" 
                                ControlToValidate="ddlItemEquipo" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                                        
                                                </td>
						
					</tr>
    <%--<asp:BoundField DataField="tipoMuestra" HeaderText="Tipo de Muestra" />
                                        <asp:BoundField DataField="prefijo" HeaderText="Prefijo Especial" />--%>
					<tr>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 115px" >
                                            &nbsp;</td>
						<td style="width: 475px">
                                              <asp:Button ID="btnGuardar" runat="server" Text="Guardar" 
                                  onclick="btnGuardar_Click2" CssClass="myButton" ValidationGroup="0" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                              &nbsp;</td>
						
						<td class="myLabelIzquierda" colspan="2" >
                                              <asp:Label ID="lblMensajeValidacion" runat="server" 
                                                  ForeColor="#FF3300"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                           &nbsp;</td>
						
						<td class="myLabelIzquierda" colspan="2" >
                                           <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
					
						    <br />
                        </td>
						
						<td class="myLabelIzquierda" colspan="2" >
					
						<div style="border: 1px solid #999999; height: 600px; width:610px; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idSysmexItem" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound" 
                                    EmptyDataText="No se configuraron prácticas para el equipo.">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="idSysmex" HeaderText="ID en Equipo" />
                                       
                                           
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
					</table><br />   &nbsp;
		
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
