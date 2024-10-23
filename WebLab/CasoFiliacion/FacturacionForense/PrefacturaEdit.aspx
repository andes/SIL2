<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PrefacturaEdit.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.PrefacturaEdit" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
   <div class="panel panel-default">
          <div class="panel-heading">
                           <h3>Prefactura</h3> 

          </div>
         <div class="panel-body">	

                 <div class="form-group">
                             <h4>Caso: 
                     <asp:Label ID="lblCaso" runat="server" Text="Label"></asp:Label>
                                 <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label>
                                 </h4>
                   </div>
             <div class="form-group">
                         <asp:Label ID="estatus" runat="server" Text="Label" ForeColor="Red" ></asp:Label>
						
                                <asp:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                AutoGenerateColumns="False" 
                               
                                Width="800px" 
                            
                               
                                GridLines="Horizontal" DataKeyNames="idDetallePresupuesto" OnRowCommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound" EmptyDataText="El caso no tiene presupuesto asignado.  Asignar desde la Edicion de Casos">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="codigo" HeaderText="Codigo" >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="nombre" HeaderText="Descripcion" >
                                    </asp:BoundField>
  <asp:BoundField DataField="cantidad" HeaderText="Cantidad Presupuesto"  >  </asp:BoundField>
                                        
                                     <asp:TemplateField HeaderText="Cantidad a Facturar">
                                       <ItemTemplate>
                               
                                           <asp:TextBox ID="txtCantidadPrefactura" runat="server" Text='<%# Convert.ToInt32(Eval("cantidadprefacturado")) %>' Width="50px"></asp:TextBox>
                            </ItemTemplate>
                                         

                        </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="precio" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="total" HeaderText="Total" />
                                      <asp:BoundField DataField="totalPrefactura" HeaderText="Total Prefactura" />
                                        <asp:BoundField DataField="estado" HeaderText="Estado" />
                                     

                                      
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

                       <anthem:Button ID="btnCalcularTotal" runat="server" CssClass="btn btn-success" Width="120px"  Text="Calcular Total" 
        onclick="btnCalcularTotal_Click" AutoUpdateAfterCallBack="True" />  
                            
                           
                      TOTAL:
                                        
                            <anthem:TextBox ID="txtTotal" runat="server"    class="form-control input-sm"           Width="150px" AutoCallBack="True" 
                                 TabIndex="11" Enabled="False" MaxLength="50"></anthem:TextBox>
                                
                    
                             </div>
             </div>
       <div class="panel-footer">
              <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-primary" Width="100px"  Text="Regresar" 
        onclick="btnRegresar_Click" ValidationGroup="0" />  
                        

                    
              <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Width="100px"  Text="Guardar" 
        onclick="btnGuardar_Click" ValidationGroup="0" />  
                        

                    
       </div>
       </div>
</asp:Content>
