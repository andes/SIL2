<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoList.aspx.cs" Inherits="WebLab.Derivaciones.ResultadoList" MasterPageFile="~/Site1.Master" %>




<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

   
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          

        <div align="center" class="form-inline" style="width:1200px;"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">   <asp:Label ID="lblTitulo" runat="server" Text="DERIVACIONES DE PACIENTES"></asp:Label>
                        </h3>
                        </div>
       	<div class="panel-body">	
				 <table   >
					 
						<tr>
					<td class="myLabelIzquierda" >
                                            DNI Paciente:</td>
						<td colspan="3">
                             <input id="txtDni" type="text" runat="server"  class="form-control input-sm"  
                                onblur="valNumero(this)" tabindex="11"/></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Apellido/s:</td>
						<td align="left" colspan="3">
                                            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control input-sm" TabIndex="13" 
                                                Width="250px"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Nombres/s:</td>
						<td align="left" colspan="2">
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control input-sm" TabIndex="14" 
                                                Width="300px"></asp:TextBox>
                        </td>
						
						<td align="left">
                                                                 <asp:Button ID="btnBuscar" runat="server"  CssClass="btn btn-primary" Width="100px" 
                                                                     onclick="btnBuscar_Click1" TabIndex="15" Text="Buscar" 
                                                                     ValidationGroup="0"  />
                        </td>
						
					</tr>
					 
					<tr>
						<td   colspan="2">
						    <asp:Label ID="CantidadRegistros" runat="server"  
                                                              forecolor="Blue" />
                                            
                        </td>
						
												<td class="myLabelLitlle" colspan="2" align="right">
                                           Referencias:
                                               <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente&nbsp;
                                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;
                                               <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;</td>
					</tr>
					</TABLE>
               </div>
                	<div class="panel-footer">	
                                          
                                                            <asp:GridView ID="gvLista" runat="server" 
                                                                AutoGenerateColumns="False" 
                                BorderColor="#3A93D2" BorderStyle="Solid" 
                                                                BorderWidth="1px" CellPadding="2" 
                                                                EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados" 
                                                                Font-Size="9pt" ForeColor="#666666" 
                                                                PageSize="20" Width="100%">
                                                                <RowStyle BackColor="White" Font-Names="Arial" Font-Size="8pt" 
                                                                    ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="estadoDerivacion">
                                                                        <ItemStyle HorizontalAlign="Center" Width="3%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="numero" HeaderText="Nro.">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha">
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="dni" HeaderText="DNI">
                                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="determinacion" HeaderText="Analisis">
                                                                        <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="efectorDerivacion" HeaderText="Efector Derivado" >
                                                                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="resultado" HeaderText="Resultado">
                                                                        <ItemStyle Width="35%" HorizontalAlign="Left" Font-Bold="True" 
                                                                            ForeColor="Black" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                <HeaderStyle BackColor="#3A93D2" Font-Bold="False" Font-Names="Arial" 
                                                                    Font-Size="8pt" ForeColor="White" />
                                                                <EditRowStyle BackColor="#999999" />
                                                            </asp:GridView>
                                          
                    </div>
               
       </div>
						
</div>
   
 
 </asp:Content>
