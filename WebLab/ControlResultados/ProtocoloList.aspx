<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloList.aspx.cs" Inherits="WebLab.ControlResultados.ProtocoloList" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>



 

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
<div  style="width: 800px" class="form-inline" >
      <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
    LISTA DE PROTOCOLOS con formulas pendientes de calcular
                        </div>
                          <div class="panel-body">

				 <table  width="100%" align="center" cellpadding="1" cellspacing="1" >
				 
				 
					<tr>
						<td   style="color: #333333; font-size: 12px">
                                           <div class="mylabelizquierda"> Seleccionar:                                           <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton></div></td>
						<td align="right" style="color: #333333; font-size: 12px" colspan="2">
                                           &nbsp;</td>
						
					</tr>
					<tr>
						<td   style="color: #333333; font-size: 12px" colspan="3">
                                          <hr /></td>
						
					</tr>
					<tr>
						<td   style="color: #333333; font-size: 12px" colspan="2">
                                           <asp:Button ID="btnCalcularFormula" runat="server" CssClass="btn btn-danger" 
                                               onclick="btnCalcularFormula_Click" Text="Calcular Fórmulas" 
                                               Width="140px" />
                        </td>
						
						<td   style="color: #333333; font-size: 12px" align="right">
                                           <asp:Label ID="CantidadRegistros" runat="server"  
                                                              forecolor="Blue" />
                                                            </td>
						
					</tr>
					<tr>
						<td   colspan="3">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idProtocolo" Font-Size="9pt" 
                                   CssClass="table table-bordered bs-table"             Width="100%" CellPadding="1"  
                                ForeColor="#666666" PageSize="50" 
                                
                                
                                
                                EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados" 
                                onrowdatabound="gvLista_RowDataBound" BorderColor="#3A93D2" 
                                BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
            <RowStyle BackColor="White" ForeColor="#333333" Font-Names="Arial" 
                Font-Size="8pt" />
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                <asp:BoundField DataField="impreso" Visible="False" />
          <asp:BoundField DataField="numero" 
                    HeaderText="Protocolo" >
                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="origen" HeaderText="Origen">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="prioridad" HeaderText="Prioridad">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="sector" HeaderText="Sector">
                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="estado" HeaderText=" " >
            

                 
                
                        
         
                        
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            

                 
                
                        
         
                        
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#3A93D2" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                Font-Names="Arial" Font-Size="8pt" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                        </td>
						
					</tr>
					<tr>
						<td align="left">       
                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
                          </td>
						<td align="right" colspan="2">       
                              &nbsp;</td>
					</tr>
					<tr>
						<td colspan="3"><hr /></td>
					</tr>
					</table>
						
 	



    </div>

   </div>
    </div>
 
 </asp:Content>
