<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeList2.aspx.cs" Inherits="WebLab.Derivaciones.InformeList2" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

 


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
 
      <div align="left" style="width:1200px">
             <div   class="panel panel-default">
                                
                    <div class="panel-heading">
                    <b  >DERIVACIONES</b> <b style="color:red">(solo se imprimiran las derivaciones marcadas como enviadas)</b>
                        </div>

				<div class="panel-body"  >
                    
				 <table  width="1000px"  >
					
					  
					<tr>
					<td class="myLabelLitlle" style="vertical-align: top" colspan="3">
                                           Referencias:
                                               <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente&nbsp;
                                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;
                                               <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;<br />
                                               &nbsp;<br />
                        </td>
						
					</tr>
					<tr>
					<td style="vertical-align: top">
                                           Acciones:
                                           <asp:LinkButton ID="lnkPDF" runat="server" CssClass="myLittleLink" 
                                               onclick="lnkPDF_Click" ValidationGroup="0">Generar PDF Derivacion</asp:LinkButton>
                                           &nbsp;
                                           <%--<asp:LinkButton ID="lnkImprimir" runat="server" CssClass="myLittleLink" 
                                               onclick="lnkImprimir_Click" ValidationGroup="0">Imprimir</asp:LinkButton>--%>
                        </td>
						<td class="myLabelLitlle" colspan="2" align="right">
                                    <%--       Impresora del sistema:
                                           <asp:DropDownList ID="ddlImpresora" runat="server" CssClass="myList">
                                           </asp:DropDownList>--%>
                        </td>
						
					</tr>
					<tr>
					<td style="vertical-align: top" colspan="3">
                    <div style="border: 1px solid #C0C0C0">
                                            <table >
                                                <tr>
                                                    <td>Marcar como:</td>
                                                    <td>
                                            <asp:DropDownList ID="ddlEstado" runat="server"  class="form-control input-sm">
                                                <asp:ListItem Value="1">Enviado</asp:ListItem>
                                                <asp:ListItem Value="2">No enviado</asp:ListItem>
                                               <%-- <asp:ListItem Value="0">Pendiente</asp:ListItem>--%>
                                            </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Observaciones:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtObservacion" runat="server" MaxLength="100" 
                                                             class="form-control input-sm"></asp:TextBox>
                                                    </td>
                                                    <td>
                                            <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary"  Width="100" Text="Guardar" 
                                                            onclick="btnGuardar_Click" />
                                                    </td>
                                                </tr>
                                                </table>
                                      </div>
                                           </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierdaGde" colspan="3">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td colspan="2">
                                           <div class="mylabelizquierda" >Seleccionar:                                           
                                               <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLittleLink" 
                                                   ValidationGroup="0" onclick="lnkMarcar_Click">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton runat="server" CssClass="myLittleLink" 
                                                   ValidationGroup="0" ID="lnkDesMarcar" onclick="lnkDesMarcar_Click">Ninguna</asp:LinkButton>
                                             &nbsp;&nbsp;
                                            
                                               </div>
                        </td>
						
						<td align="right">
                                       <%--     Practica:&nbsp;<asp:DropDownList ID="ddlItem" runat="server" 
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" AutoPostBack="True" 
                                                CssClass="myLabel">
                                            </asp:DropDownList>--%>
                            <asp:Label ID="CantidadRegistros" runat="server"  
                                                              forecolor="Blue" />
                        </td>
						
					</tr>
					<tr>
						<td colspan="3">
                        	<div  style="width:100%;height:450pt;overflow:scroll;;overflow-x:hidden;border:1px solid #CCCCCC; background-color: #F3F3F3;"> 
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                                DataKeyNames="idDetalleProtocolo"  
                                                Width="98%" CellPadding="0" 
                                ForeColor="#666666" PageSize="1" 
                                
                                
                                
                                
                                
                                
                                
                                EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                                BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" 
                Font-Size="8pt" />
            <Columns>
            
               <asp:TemplateField HeaderText="Sel." >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                <asp:BoundField DataField="estado">
                    <ItemStyle Width="15px" />
                </asp:BoundField>
          <asp:BoundField DataField="numero" 
                    HeaderText="Protocolo" >
                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Paciente">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="determinacion" HeaderText="Practica a derivar">
                    <ItemStyle Width="20%" />
                </asp:BoundField>
            

                 
                
                        
         
                        
                <asp:BoundField DataField="efectorderivacion" HeaderText="Efector">
                    <ItemStyle Width="15%" />
                </asp:BoundField>
            

                 
                
                        
         
                        
                <asp:BoundField DataField="username" HeaderText="Usuario" >
            

                 
                
                        
         
                        
                    <ItemStyle Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="observacion" HeaderText="Observaciones">
                    <ItemStyle Width="10%" />
                </asp:BoundField>
            

                 
                
                        
         
                        
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                Font-Names="Arial" Font-Size="8pt" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        </div>
                        </td>
						
					</tr>
					<tr>
						<td colspan="3"><hr /></td>
					</tr>
					<tr>
						<td colspan="3">
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink" 
                                NavigateUrl="~/Derivaciones/Derivados2.aspx?tipo=informe">Regresar</asp:HyperLink>
                        </td>
					</tr>
					</table>
                    <div>


                    </div>
                    </div>
						
</div>
   
 </div>
 </asp:Content>
