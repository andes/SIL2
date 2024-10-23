<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgendaList.aspx.cs" Inherits="WebLab.Agendas.AgendaList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
    
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <div align="left" class="form-inline" style="width: 1200px" >
  
          
	   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE AGENDAS</h3>
                        </div>

				<div class="panel-body">	
                    
		<table >
			
			<tr>
				<td class="myLabelIzquierda" style="width: 100px" >
                                    Tipo de servicio:</td>
				<td colspan="2" >
                    <asp:DropDownList ID="ddlTipoServicio" runat="server" class="form-control input-sm" 
                        ToolTip="Seleccione el servicio" AutoPostBack="True" 
                        onselectedindexchanged="ddlTipoServicio_SelectedIndexChanged">
                    </asp:DropDownList>
                            </td>
				<td align="right" style="width: 134px" >
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt"  CssClass="btn btn-primary" Width="100px" />
                </td>
			</tr>
			
			</table>

	
                    <br />
   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
             DataKeyNames="idAgenda" CssClass="table table-bordered bs-table" 
            onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" 
                        EmptyDataText="No hay agendas creadas" 
                        GridLines="Horizontal">
            <%--<RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Size="8pt" />--%>
            <Columns>
             <asp:BoundField DataField="nombre"  HeaderText="Tipo de Servicio" >
                  
                </asp:BoundField>
                   <asp:BoundField DataField="efector"  HeaderText="Efector" >
                   
                </asp:BoundField>
            <asp:BoundField DataField="item"  HeaderText="Practica" >
                   
                </asp:BoundField>
                <asp:BoundField DataField="fechadesde" HeaderText="Fecha Desde" >
              
                </asp:BoundField>
                <asp:BoundField DataField="fechahasta" HeaderText="Fecha Hasta">
                    
                </asp:BoundField>
                  <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha/hora" />
                  <asp:TemplateField>
                    <ItemTemplate>
                    <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                    ommandName="Editar" />
                    </ItemTemplate>                          
                    <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />                          
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                    <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                        OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                    </ItemTemplate>                          
                    <ItemStyle HorizontalAlign="Center" Width="20px" Height="20px" />                          
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
            <%--<FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
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

