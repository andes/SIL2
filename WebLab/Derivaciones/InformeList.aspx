<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeList.aspx.cs" Inherits="WebLab.Derivaciones.InformeList" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


 


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
   <br />   &nbsp;
      <div align="left" style="width:1000px">

				 <table  width="1000px" align="center" 
                     
                     
                   
                     cellpadding="1" cellspacing="1" class="myTabla">
					
					<tr>
						<td   colspan="4"><b class="mytituloTabla">DERIVACIONES</b>
                                            </td>
						
					</tr>
					
					<tr>
						<td   colspan="4">    <hr class="hrTitulo" /></td>
						
					</tr>
					<tr>
						<td class="myLabel" style="vertical-align: top">
                                            Practica:&nbsp;
                                            <asp:DropDownList ID="ddlItem" runat="server" 
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" AutoPostBack="True" 
                                                CssClass="myLabel">
                                            </asp:DropDownList>
                        </td>
						
					<td class="myLabel" style="vertical-align: top">
                                            Efector a derivar:
                                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                                 AutoPostBack="True" 
                                                onselectedindexchanged="ddlEfector_SelectedIndexChanged" 
                                                CssClass="myLabel">
                                            </asp:DropDownList>
                        </td>
						
						<td class="myLabel">
                                            Estado:
                                            <asp:DropDownList ID="ddlEstadoFiltro" runat="server" AutoPostBack="True" 
                                                CssClass="myLabel" 
                                                onselectedindexchanged="ddlEstadoFiltro_SelectedIndexChanged">
                                                <asp:ListItem Value="-1">Todos</asp:ListItem>
                                                <asp:ListItem Value="0">Pendientes</asp:ListItem>
                                                <asp:ListItem Value="1">Enviado</asp:ListItem>
                                                <asp:ListItem Value="2">No enviado</asp:ListItem>
                                            </asp:DropDownList>
                                                    </td>
						
						<td class="myLabel" align="right">
&nbsp;
                                            &nbsp;&nbsp;
                                                    </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierdaGde" colspan="4">
                                            <hr /></td>
						
					</tr>
					<tr>
					<td style="vertical-align: top">
                                           <div class="mylabelizquierda" >Seleccionar:                                           
                                               <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLittleLink" 
                                                   ValidationGroup="0" onclick="lnkMarcar_Click">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton runat="server" CssClass="myLittleLink" 
                                                   ValidationGroup="0" ID="lnkDesMarcar" onclick="lnkDesMarcar_Click">Ninguna</asp:LinkButton>
                                             &nbsp;&nbsp;
                                                Acciones: <asp:LinkButton 
                            ID="lnkPDF" runat="server" CssClass="myLittleLink" onclick="lnkPDF_Click" ValidationGroup="0">Visualizar 
                        en formato pdf</asp:LinkButton>  &nbsp; <%--<asp:LinkButton 
                            ID="lnkImprimir" runat="server" CssClass="myLittleLink" onclick="lnkImprimir_Click" 
                                               ValidationGroup="0">Imprimir</asp:LinkButton>--%>
                                               </div>
					</td>
						<td class="myLabelLitlle" colspan="3" align="right">
                                           Referencias:
                                               <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente&nbsp;
                                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;
                                               <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;</td>
						
					</tr>
					<tr>
					<td style="vertical-align: top" colspan="4">
                                            <table >
                                                <tr>
                                                    <td>Marcar como:</td>
                                                    <td>
                                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="myLabel">
                                                <asp:ListItem Value="1">Enviado</asp:ListItem>
                                                <asp:ListItem Value="2">No enviado</asp:ListItem>
                                                <asp:ListItem Value="0">Pendiente</asp:ListItem>
                                            </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Observaciones:</td>
                                                    <td>
                                                        <asp:TextBox ID="txtObservacion" runat="server" MaxLength="100" 
                                                            CssClass="myTexto"></asp:TextBox>
                                                    </td>
                                                    <td>
                                            <asp:Button ID="btnGuardar" runat="server" CssClass="myButton" Text="Guardar" 
                                                            onclick="btnGuardar_Click" />
                                                    </td>
                                                </tr>
                                                </table>
                                      
                                           </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierdaGde" colspan="4">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td colspan="4">
                            <asp:Label ID="CantidadRegistros" runat="server"  
                                                              forecolor="Blue" />
                        </td>
						
					</tr>
					<tr>
						<td colspan="4">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idDetalleProtocolo" Font-Size="9pt" 
                                                Width="100%" CellPadding="0" 
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
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
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
            

                 
                
                        
         
                        
                    <ItemStyle Width="5%" />
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
                        </td>
						
					</tr>
					<tr>
						<td colspan="4"><hr /></td>
					</tr>
					<tr>
						<td colspan="4">
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink" 
                                NavigateUrl="~/Derivaciones/Derivados.aspx?tipo=informe">Regresar</asp:HyperLink>
                        </td>
					</tr>
					</table>
						
</div>
   
 
 </asp:Content>
