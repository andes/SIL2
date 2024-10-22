<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="FacturaEdit.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.FacturaEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div align="left" style="width: 95%" class="form-inline"  >
      
   <div class="panel panel-default">
          <div class="panel-heading">
                           <h4 >FACTURA</h4> 

          </div>
         <div class="panel-body">	

                 <div class="form-group">
                             <h4>Caso: 
                     <asp:Label ID="lblCaso" runat="server" Text="Label"></asp:Label>
                   </h4>
                            
                     </div>
              <br />
              <div class="form-group">
                             <h4>Total Factura ($): 
                     <asp:Label ID="lblTotal" ForeColor="Red" runat="server" Text="Label"></asp:Label></h4>
                   
                           
                     </div>
               <br />
                   <div class="form-group">
                             <h4>Nro. Factura: 
                    </h4>
                       <asp:TextBox ID="txtNumeroFactura" runat="server"></asp:TextBox>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Width="100px"  Text="Guardar" 
        onclick="btnGuardar_Click" ValidationGroup="0" />  
                            </div>
					</div>
       
           <div class="panel-footer">	
               <h3>Detalle de Factura</h3>	
                                <asp:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                AutoGenerateColumns="False" 
                               
                                Width="800px" 
                            
                               
                                GridLines="Horizontal" DataKeyNames="idDetallePresupuesto">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="codigo" HeaderText="Codigo" >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="nombre" HeaderText="Descripcion" >
                                    </asp:BoundField>
  <asp:BoundField DataField="cantidad" HeaderText="Cantidad Autorizada"  >  </asp:BoundField>
                                        
                                    <asp:BoundField DataField="precio" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="total" HeaderText="Total" />
                                     

                                      
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


                            
                          

                            
                             <br />
                  
                                           <asp:Button ID="btnImprimir" runat="server" CssClass="btn btn-success" Width="100px"  Text="Imprimir" 
           OnClick="btnImprimir_Click" />  
                            
                        

                            
                             
                                           <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-success" Width="100px"  Text="Regresar" 
           OnClick="btnRegresar_Click" />  
                            
                        

                            
                             
             </div>
        
         </div>
     </div>
</asp:Content>
