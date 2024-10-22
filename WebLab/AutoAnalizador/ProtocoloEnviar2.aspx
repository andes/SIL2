<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloEnviar2.aspx.cs" Inherits="WebLab.AutoAnalizador.ProtocoloEnviar2" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

 
  
 

</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  <div align="left" style="width: 1050px" class="form-inline"  >
   <div class="panel panel-danger">
                    <div class="panel-heading">
    <h3 class="panel-title">   
                            ENVIO DE DATOS</h3>
                        <h4><asp:Label ID="lblTituloEquipo"   runat="server" Text="Label"></asp:Label></h4>
                        </div>

				<div class="panel-body">
				 
                               <asp:Label ID="lblCantidadProtocolos" runat="server" Text="Label"></asp:Label>
                        <br />
                              <img src="../App_Themes/default/images/amarillo.gif" /> Pendiente de enviar
                              <img src="../App_Themes/default/images/verde.gif" /> Enviado 
						
					 
                               <div class="myLabelIzquierda" align="right"> Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton>&nbsp;&nbsp;
  <asp:Button ID="btnEnviar" runat="server" onclick="btnEnviar_Click" 
                                  Text="Enviar" CssClass="btn btn-danger" Width="100px" />
                               </div>
                    
                     
                         
                         
                        </div>

					<div class="panel-footer">
					<div style="border: 1px solid #999999; height: 500px; width:1000px; overflow: scroll; background-color: #EFEFEF;">
                                <asp:GridView   ID="gvLista" runat="server" AutoGenerateColumns="False" Width="98%" 
                                    DataKeyNames="idProtocolo"  CssClass="table table-bordered bs-table"
                                    EmptyDataText="No se encontraron muestras para los filtros propuestos." 
                                    Font-Names="Arial" Font-Size="11pt">
                                    <RowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="numero" HeaderText="Numero" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                          <asp:BoundField DataField="numeroDocumento" HeaderText="DU" >
                                              <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="paciente" HeaderText="Paciente" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="edad" HeaderText="Edad" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                           <asp:BoundField DataField="sexo" HeaderText="Sexo" >
                                            <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                            <asp:BoundField DataField="origen" HeaderText="Origen" >
                                                  <ItemStyle Font-Bold="True" />
                                        </asp:BoundField>
                                              <asp:TemplateField HeaderText="Enviar" >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                         <asp:BoundField DataField="estado" >
                                             <ItemStyle Font-Bold="True" Font-Italic="True" Width="15px" />
                                        </asp:BoundField>
                                        <%-- <asp:BoundField  HeaderText="Usuario">
                                             <ItemStyle Font-Bold="True" Font-Italic="True" />
                                        </asp:BoundField>
                                         <asp:BoundField  HeaderText="Fecha Envio">
                                             <ItemStyle Font-Bold="True" Font-Italic="True" />
                                        </asp:BoundField>--%>
                                    </Columns>
                                    
                                    <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
                                </asp:GridView>
					</div>
						

					<table>
					<tr>
						<td align="left" style="width: 54px">       
                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
                          </td>
						<td align="right" colspan="3">       
                       
                        </td>
					</tr>
					 
					</table>
                        </div>
						

</div>
       </div>
      </div>
 
 </asp:Content>
