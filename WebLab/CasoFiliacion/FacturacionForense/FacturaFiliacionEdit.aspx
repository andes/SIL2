<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="FacturaFiliacionEdit.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.FacturaFiliacionEdit" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
   
  
    <link type="text/css"rel="stylesheet"      href="../../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
	
   	 <script type="text/javascript" src="../../script/Mascara.js"></script>
    <script type="text/javascript" src="../../script/ValidaFecha.js"></script>   
   

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div align="left" style="width: 95%" class="form-inline"  >
      
   <div class="panel panel-default">
          <div class="panel-heading">
                           <h3 >FACTURA</h3> 

          </div>
         <div class="panel-body">	

                 <div class="form-group">
                             <h4>Caso: 
                     <asp:Label ID="lblCaso" runat="server" Text="Label"></asp:Label>
                   </h4>
                            
                     </div>
              <br />
               <br />
             <div class="form-group" runat="server"    >                                     
        <h5>   &nbsp;</h5>
 &nbsp;</div>
                   <div class="form-group">
                             <h4>Nro. Factura: 
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNumeroFactura" ErrorMessage="*" ValidationGroup="0"  >*</asp:RequiredFieldValidator>
                    </h4>
                       <asp:TextBox ID="txtNumeroFactura" runat="server"></asp:TextBox>
                            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-success" Width="100px"  Text="Guardar" 
        onclick="btnGuardar_Click" ValidationGroup="0" />  
                            <asp:Button ID="btnAnular" runat="server" CssClass="btn btn-success" Width="100px"  Text="Anular" 
        onclick="btnAnular_Click" ValidationGroup="0" />  
                            </div>
					</div>
       
           <div class="panel-footer">	
               <h3>Detalle de Factura</h3>	
             <div id="tab3" class="tab_content" >
    <table style="width: 700px"  >
<tr>
						<td class="myLabelIzquierda" >
                                            Codigo:</td>
						<td>
                            <anthem:TextBox ID="txtCodigo" runat="server"   class="form-control input-sm"          
                               style="text-transform:uppercase"   ontextchanged="txtCodigo_TextChanged" Width="88px" AutoCallBack="True" 
                                TabIndex="5"></anthem:TextBox>
                            <anthem:DropDownList ID="ddlItem" runat="server"    class="form-control input-sm"          
                                onselectedindexchanged="ddlItem_SelectedIndexChanged" AutoCallBack="True" 
                                TabIndex="6" Width="300px">
                            </anthem:DropDownList>
                                        
                        </td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" >
                                            Precio Unitario:</td>
						<td>
                                        
                            <anthem:TextBox ID="txtPrecio" runat="server"    class="form-control input-sm"           Width="80px" AutoCallBack="True" 
                                 TabIndex="7" Enabled="False" MaxLength="50"></anthem:TextBox>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Descripción:</td>
						<td>
                            <anthem:DropDownList ID="ddlProtocoloFiliacion" runat="server"    class="form-control input-sm" 
                                TabIndex="6" Width="300px" Enabled="False">
                            </anthem:DropDownList>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Cantidad</td>
						<td>
                            <anthem:TextBox ID="txtCantidad" runat="server"    class="form-control input-sm"           Width="50px" AutoCallBack="True" 
                                 TabIndex="9" Enabled="False" MaxLength="50"></anthem:TextBox>
                                        
                            <anthem:Button ID="btnAgregar" runat="server" CssClass="btn btn-info" Enabled="False" onclick="btnAgregar_Click" TabIndex="10" Text="Agregar" Width="80px" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            &nbsp;</td>
						<td>
                            <asp:CustomValidator ID="cvAnalisis" runat="server" 
                                ErrorMessage="Debe completar al menos un codigo" 
                                ValidationGroup="0" 
                                Font-Size="8pt">Debe completar al menos un codigo</asp:CustomValidator>
                                        <anthem:Label ID="lblMensaje" runat="server" ForeColor="#FF3300">&nbsp;&nbsp; </anthem:Label>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
						
                                <anthem:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                onrowdatabound="gvLista_RowDataBound1" AutoGenerateColumns="False" 
                               
                                onrowcommand="gvLista_RowCommand" Width="800px" 
                                EmptyDataText="Agregue los protocolos correspondientes" 
                               
                                GridLines="Horizontal" DataKeyNames="fila">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="fila" HeaderText="Linea" />
                                    <asp:BoundField DataField="codigo" HeaderText="Codigo Nom." >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="nombre" HeaderText="Descripcion" >
                                    </asp:BoundField>
                                     <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="precio" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="total" HeaderText="Total" />
                                     <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                           <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
                                </Columns>
                              <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
                               
                            </anthem:GridView>


                            
                                TOTAL:
                                        
                            <anthem:TextBox ID="txtTotal" runat="server"    class="form-control input-sm"           Width="150px" AutoCallBack="True" 
                                 TabIndex="11" Enabled="False" MaxLength="50"></anthem:TextBox>
                                        

                            
                                </td>
						
					</tr>
					</table>
</div>


                            
                          

                            
                             <br />
                  
                                           <asp:Button ID="btnImprimir" runat="server" CssClass="btn btn-success" Width="100px"  Text="Imprimir" 
           OnClick="btnImprimir_Click" />  
                            
                        

                            
                             
                                           <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-success" Width="100px"  Text="Regresar" 
           OnClick="btnRegresar_Click" />  
                            
                        

                            
                             
             </div>
        
         </div>
     </div>
</asp:Content>
