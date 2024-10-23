<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionEdit.aspx.cs" Inherits="WebLab.AutoAnalizador.SysmexKX21N.ConfiguracionEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
   
   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">            
  <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading"> 
    <h3 class="panel-title">Configuración SIL - Sysmex KX21N</h3>
                        </div>

				<div class="panel-body">
		
<table align="center" width="600" class="style1">
 
 
<tr>
						<td class="style4" >
                                            Area:</td>
						<td class="style5">
                            <anthem:DropDownList ID="ddlArea" runat="server"  class="form-control input-sm"
                                AutoCallBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged"></anthem:DropDownList>
                          
                                        
                        </td>
						
					</tr>
    <tr>
        <td>Determinacion:</td>
        <td>              
                            <anthem:DropDownList ID="ddlItem" runat="server"  class="form-control input-sm"
                                
                               >
                            </anthem:DropDownList>
                                        
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="ddlItem" ErrorMessage="*" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator></td>
    </tr>
					<tr>
						<td class="style4" >
                                            ID en Equipo:</td>                                        
						<td class="style5">
                            <asp:DropDownList ID="ddlItemEquipo" runat="server" class="form-control input-sm"  Width="150px">
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
                                <asp:ListItem Value="10">LYM%</asp:ListItem>
                                <asp:ListItem Value="11">MXD%</asp:ListItem>
                                <asp:ListItem Value="12">LYM#</asp:ListItem>
                                <asp:ListItem Value="13">MXD#</asp:ListItem>
                                <asp:ListItem Value="14">NEUT#</asp:ListItem>
                                <asp:ListItem Value="15">RDW-SD/CV</asp:ListItem>                                
                                <asp:ListItem Value="16">MPV</asp:ListItem>
                                <asp:ListItem Value="17">PDW</asp:ListItem>
                                <asp:ListItem Value="18">P-LCR</asp:ListItem>                               
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
                                  onclick="btnGuardar_Click2" Width="100px"  CssClass="btn btn-primary" ValidationGroup="0" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
                                              <asp:Label ID="lblMensajeValidacion" runat="server" 
                                                  ForeColor="#FF3300"></asp:Label>
                        </td>
						
					</tr>
					 
    </table>
                    </div>
                    	<div class="panel-footer">
                    <table>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
					
						<div style="border: 1px solid #999999; height: 500px; width:610px; overflow: scroll;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="590px" 
                                    DataKeyNames="idSysmexItem" onrowcommand="gvLista_RowCommand" 
                                    onrowdatabound="gvLista_RowDataBound"  CssClass="table table-bordered bs-table"
                                    EmptyDataText="No se configuraron prácticas para el equipo">
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
					</table>
		
        <br />

		</div>		
       </div>
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
