<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HTList.aspx.cs" Inherits="WebLab.HojasTrabajo.HTList" MasterPageFile="~/Site1.Master" %>
 
<asp:Content ID="Content3" runat="server" contentplaceholderid="head" />

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
     
        <div align="center" style="width: 1000px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE HOJAS DE TRABAJO</h3>
                        </div>

				<div class="panel-body">
       
	   
		<table     >
		<tr>
		<td class="myLabelIzquierda" >
	

   
                    Tipo de Servicio:</td>
				<td>

	

   
                            <asp:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control input-sm"                            
                                onselectedindexchanged="ddlServicio_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                                        
                </td>
		<td class="myLabelIzquierda" >

	

   
                    &nbsp;  &nbsp; Area:</td>
				<td>

	

   
                            <asp:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="2" class="form-control input-sm"
                        onselectedindexchanged="ddlArea_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                                        
                </td>
				<td>

	
                      &nbsp;  &nbsp;
   
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0" 
                                                onclick="btnBuscar_Click" CssClass="btn btn-primary" Width="80px" TabIndex="24"   />
                </td>
            <td>
                  &nbsp;  &nbsp;  &nbsp;  &nbsp;
            </td>
          
				<td>

	

   
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" CssClass="btn btn-primary" Width="80px"
                                        ToolTip="Haga clic aquí para agregar una nueva hoja de trabajo" />
                                </td>
			</tr>
			


			<tr>
				<td align="right" colspan="6" >
                                    &nbsp;</td>
			</tr>
			 
			</table>

	</div>
         <div class="panel-footer">
   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
            CellPadding="1" DataKeyNames="idHojaTrabajo" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound"  Width="100%" 
                        EmptyDataText="No se encontraron registros para los filtros de busqueda ingresados" BorderColor="#3A93D2" 
                        BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
             <asp:BoundField DataField="servicio" 
                    HeaderText="Tipo de Servicio" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="area" HeaderText="Area">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="codigo" HeaderText="Codigo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
                           <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Pdf" runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg" 
                              CommandName="Pdf" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                        
                        
                 <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
         </div>

</div>
    
        </div>
    <script type="text/javascript" language="javascript">
    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar el registro?'))
    return true;
    else
    return false;
    }
    </script>
</asp:Content>