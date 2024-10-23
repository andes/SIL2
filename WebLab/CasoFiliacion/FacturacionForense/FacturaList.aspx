<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="FacturaList.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.FacturaList" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
   <div class="panel panel-default">
          <div class="panel-heading">
                           <h3  >Factura</h3> 

          </div>
         <div class="panel-body">	

                 <div class="form-group">
                             <h4>Estado: </h4>
                      <asp:DropDownList ID="ddlTipo" class="form-control input-sm" runat="server" TabIndex="1" Width="180px" OnSelectedIndexChanged="ddlCasoCerrado_SelectedIndexChanged" AutoPostBack="True">
                          <asp:ListItem  Selected="True" Value="1">Pendiente de Facturar</asp:ListItem>
                          <asp:ListItem Value="2">Facturadas</asp:ListItem>
                          <asp:ListItem  Value="0">Todas</asp:ListItem>
                                        </asp:DropDownList>
                    
				</div>
             </div>
              <div class="panel-footer">			
                                <asp:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                AutoGenerateColumns="False" 
                               
                                Width="900px" 
                            
                               
                                GridLines="Horizontal" DataKeyNames="idCasoFiliacion" OnRowCommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="idCasofiliacion" HeaderText="Nro. Caso" >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                                    </asp:BoundField>
                                     <asp:BoundField DataField="TotalPrefactura" HeaderText="Total" />
                                    <asp:BoundField DataField="factura" HeaderText="Facturado" />
                                      <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                      <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" />
                                     <asp:TemplateField HeaderText="Factura">
                            <ItemTemplate>
                                <asp:Button ID="btnFactura"  CssClass="btn btn-success" runat="server" Text="Factura" Width="100px"  /> 

                            </ItemTemplate>
                          
                         
                          
                        </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Anular">
                            <ItemTemplate>
                                <asp:Button ID="btnEliminar" runat="server" Text="Anular" CssClass="btn btn-danger"  OnClientClick="return PreguntoEliminar();" Width="100px" /> 

                            </ItemTemplate>
                          
                            
                          
                        </asp:TemplateField>
                                      
                                </Columns>
                              <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
                               
                            </asp:GridView> 

                   

                            
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
