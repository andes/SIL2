<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloEnviar.aspx.cs" Inherits="WebLab.AutoAnalizador.ProtocoloEnviar" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

 
  
 

</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
   <br />   &nbsp;
    

<div align="left" style="left: 20px" >
				 <table cellpadding="1" cellspacing="1" style="width: 1016px" >
					
					<tr>
						<td class="mytituloTabla"  colspan="2">ENVIO DE DATOS</td>
						<td align="right"  colspan="2"> <asp:Label ID="lblTituloEquipo" CssClass="mytituloRojo2" runat="server" Text="Label"></asp:Label></td>
					</tr>
					
					<tr>
						<td   colspan="4">   <hr /></td>
						
					</tr>
					<tr>
						<td   style="color: #333333; font-size: 12px" colspan="2" 
                            class="myLabelIzquierda">
                               Protocolos encontrados:</td>
						
						<td  class="myLabelIzquierda" 
                            style="color: #333333; font-size: 12px; width: 279px;">
                               <asp:DropDownList ID="ddlProtocolo" runat="server" AutoPostBack="True" 
                                   onselectedindexchanged="ddlProtocolo_SelectedIndexChanged">
                               </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp; </td>
						
						<td   style="color: #333333; font-size: 12px; width: 467px;" align="right" 
                            class="myLabelIzquierda">
                               <asp:Label ID="lblCantidadProtocolos" runat="server" Text="Label"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td   style="color: #333333; font-size: 12px" colspan="2" 
                            class="myLabelIzquierda">
                               Referencias:</td>
						
						<td  class="myLabelIzquierda" 
                            style="color: #333333; font-size: 12px; width: 279px;">
                              <img src="../App_Themes/default/images/amarillo.gif" /> Pendiente de enviar
                              <img src="../App_Themes/default/images/verde.gif" /> Enviado </td>
						
						<td   style="color: #333333; font-size: 12px; width: 467px;" align="right">
                               <div class="myLabelIzquierda" align="right"> Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton></div>
                        </td>
						
					</tr>
					<tr>
						<td   style="color: #333333; font-size: 12px" colspan="4">
                               <hr /></td>
						
					</tr>
					<tr>
					<td colspan="4" style="height: 32px">
					
					<div style="border: 1px solid #999999; height: 600px; width:1010px; overflow: scroll; background-color: #EFEFEF;">
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" Width="1000px" 
                                    DataKeyNames="idDetalleProtocolo" 
                                    EmptyDataText="No se encontraron muestras para los filtros propuestos." 
                                    Font-Names="Arial" Font-Size="8pt">
                                    <RowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="numero" HeaderText="Numero" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="paciente" HeaderText="Paciente" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                       <asp:BoundField DataField="analisis" HeaderText="Analisis" >
                                              <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                              <asp:BoundField DataField="TipoMuestra" HeaderText="Muestra" >
                                                  <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                              <asp:TemplateField HeaderText="Enviar" >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                         <asp:BoundField DataField="enviado" >
                                             <ItemStyle Font-Bold="True" Font-Italic="True" Width="15px" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="usuarioEnvio"  HeaderText="Usuario">
                                             <ItemStyle Font-Bold="True" Font-Italic="True" />
                                        </asp:BoundField>
                                         <asp:BoundField DataField="fechaEnvio" HeaderText="Fecha Envio">
                                             <ItemStyle Font-Bold="True" Font-Italic="True" />
                                        </asp:BoundField>
                                    </Columns>
                                    
                                    <HeaderStyle BackColor="#999999" />
                                </asp:GridView>
					</div>
					
					</td>
					</tr>
					<tr>
						<td align="left" style="width: 54px">       
                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
                          </td>
						<td align="right" colspan="3">       
                              <asp:Button ID="btnEnviar" runat="server" onclick="btnEnviar_Click" 
                                  Text="Enviar" CssClass="myButtonRojo" Width="100px" />
                        </td>
					</tr>
					<tr>
						<td colspan="4"><hr /></td>
					</tr>
					</table>
						

</div>
 
 </asp:Content>
