<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObservacionList.aspx.cs" Inherits="WebLab.Observaciones.ObservacionList" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server" />

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

     <div align="left" style="width: 1000px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">OBSERVACIONES CODIFICADAS</h3>
                        </div>

				<div class="panel-body">
		<table   >
			 
			<tr>
				<td  >
                                    Tipo de Servicio:</td>
				<td align="left"  >
                  <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="form-control input-sm"
                        TabIndex="2" ToolTip="Seleccione el servicio" AutoPostBack="True" 
                                        onselectedindexchanged="ddlTipoServicio_SelectedIndexChanged">
                    </asp:DropDownList>
                    </td>
				<td  >
                                    &nbsp;</td>
			</tr>
            <tr>
				<td colspan="3">
                    <hr />
                    </td>
                </tr>
			 
			<tr>
				<td colspan="3">

	

   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idObservacion" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Width="100%" 
                        EmptyDataText="No hay observaciones creadas"   CssClass="table table-bordered bs-table" 
                        GridLines="Horizontal" 
                        onselectedindexchanged="gvLista_SelectedIndexChanged">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
            <Columns>
            <asp:BoundField DataField="tipoServicio" HeaderText="Tipo de Servicio" >
                    <ItemStyle Width="10%" HorizontalAlign="Center"/>
                </asp:BoundField>
                <asp:BoundField DataField="codigo" HeaderText="Codigo" >
                    <ItemStyle Width="10%" HorizontalAlign="Center"/>
                </asp:BoundField>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Descripción" >
                    <ItemStyle Width="70%" HorizontalAlign="left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" ToolTip="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
                 <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" ToolTip="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
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
                </td>
			</tr>
			 
			 
                                   
                           
			</table>
                    </div>

       		<div class="panel-footer">
                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" CssClass="btn btn-primary" Width="100px" />
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