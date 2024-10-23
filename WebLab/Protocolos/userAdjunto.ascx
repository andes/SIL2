<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="userAdjunto.ascx.cs" Inherits="WebLab.Protocolos.userAdjunto" %>

<div align="left" style="width: 1200px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h1 class="panel-title">Archivos Anexos</h1>
                        </div>

				<div class="panel-body">	
<asp:Label ID="lblProtocolo" runat="server" 
                    Style="color: #0000FF"></asp:Label><input id="hdnProtocolo" runat="server" type="hidden" />
                    <br />
Archivo: <asp:FileUpload ID="trepador" runat="server"  class="form-control input-sm" />
                    <br />
Visibilidad: <asp:DropDownList ID="ddlVisibilidad" runat="server"  class="form-control input-sm" >
    <asp:ListItem Selected="True" Value="0">No visible (interno)</asp:ListItem>
    <asp:ListItem Value="1">Todos los usuarios</asp:ListItem>
</asp:DropDownList><br />
Descripcion:                <asp:TextBox ID="txtDescripcion" runat="server"  class="form-control input-sm"></asp:TextBox>
                     <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="subir_Click"    Width="150px" >
                                             <span class="glyphicon glyphicon-arrow-up"></span>&nbsp;Subir Archivo</asp:LinkButton>
                    
                    
                     
            <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>

                    </div>
       
    <div class="panel-footer">		
        

<asp:GridView ID="gvListaProducto" runat="server" AllowPaging="True" 
                                                                 CssClass="table table-bordered bs-table"  AutoGenerateColumns="False" DataKeyNames="idProtocoloAnexo" 
                                                                EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados"                                                                 
                                                                onpageindexchanging="gvListaProducto_PageIndexChanging" 
                                                                onrowcommand="gvListaProducto_RowCommand" onrowdatabound="gvListaProducto_RowDataBound" 
                                                                PageSize="20" Width="100%"  GridLines="Horizontal" BackColor="White">                                                                
                                                                
                                                                <Columns>
                                                                      <asp:TemplateField HeaderText="Descargar">
			<ItemTemplate>
				<asp:HyperLink ID="Download" text = "Download" runat="server" Target="_blank">
				</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
                                                                    <asp:BoundField DataField="url" HeaderText="Archivo" >
                                                                    <ItemStyle Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="descripcion" HeaderText="Descripcion">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="visible" HeaderText="Visible" >
                                                                    <ItemStyle  Width="5%" />
                                                                        </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" 
                                                                                ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                                                                                OnClientClick="return PreguntoEliminar();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                                                                   
                                                                </Columns>
                                                                <PagerSettings Position="Top" />
                                                                <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <RowStyle BackColor="White" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                            </asp:GridView>
        </div>
    </div>
