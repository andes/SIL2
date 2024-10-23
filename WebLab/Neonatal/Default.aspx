<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebLab.Neonatal.Default" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title> 
<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script> 
  

</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    

        <div align="left" style="width:1200px; height:500pt;">
 <table  width="1000px" align="center" class="myTabla">
					<tr>
						
						
						<td rowspan="10" style="vertical-align: top" >
                                            &nbsp;</td>
						
						
						<td rowspan="10" style="vertical-align: top" >
                                            <table class="myTabla" width="650px">
                                            <tr>
						<td colspan="2" >&nbsp;</td>
						<td colspan="2" align="right" >
                                   &nbsp;</td>
					</tr>
                                            <tr>
						<td colspan="2" class="mytituloTabla">SOLICITUD DE PESQUISA NEONATAL</td>
						<td colspan="2" align="right" >
                           <%--<asp:Label ID="lblUrgencia" runat="server" CssClass="mytituloRojo" 
                                Text="URGENCIA" Visible="False"></asp:Label>--%>
                                                </td>
					</tr>
                                            <tr>
						<td colspan="4" > <hr class="hrTitulo" /></td>
					</tr>
					<tr>
						
						<td colspan="4" 
                            
                            
                            
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;">
                                            &nbsp; 
                                            Identificación del Paciente</td>
						
					</tr>
					<tr>
						
						<td   colspan="4" 
                            style="color: #FF0000; font-weight: bold; font-size: 12px">
                                          <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  >
                                            DU Paciente:</td>
						<td align="left" colspan="3">
                            <input id="txtDni" type="text" runat="server"  class="myTexto"  
                                onblur="valNumeroSinPunto(this)" maxlength="8"/>
                           
                           
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  >
                                            Apellido/s:</td>
						<td align="left" colspan="3">
                                            <asp:TextBox ID="txtApellido" runat="server" CssClass="myTexto" TabIndex="2" 
                                                ValidationGroup="1" Width="150px"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  >
                                            Nombres/s:</td>
						<td align="left" colspan="3">
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="myTexto" TabIndex="3" 
                                                Width="217px"></asp:TextBox>
                        </td>
						
					</tr>
						<tr>
						<td class="myLabelIzquierda" colspan="4" >
                                           <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            DU Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                                          <input id="txtDniMadre" type="text" runat="server"  class="myTexto"  
                                 onblur="valNumero(this)" maxlength="8"/></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            Apellido Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtApellidoMadre" runat="server" CssClass="myTexto" TabIndex="4" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            Nombres/s Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtNombreMadre" runat="server" CssClass="myTexto" TabIndex="4" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						
						<td   colspan="4">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td  >
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0" 
                                               CssClass="myButton" TabIndex="4" onclick="btnBuscar_Click" 
                                                ToolTip="Buscar Pacientes en SGH" />
                                            <asp:Button ID="btnNuevoPaciente" runat="server" 
                                                onclick="btnNuevoPaciente_Click" Text="Nuevo Paciente" Visible="False" />
                        </td>
						<td align="right" colspan="2">
                                            <asp:CustomValidator ID="cvDatosEntrada" runat="server" 
                                                ErrorMessage="Debe ingresar al menos un parametro de busqueda." 
                                                onservervalidate="cvDatosEntrada_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                           
                        </td>
						
						<td align="right">
                                           <asp:HyperLink ID="HyperLink1" runat="server" 
                                                
                                                
                                           NavigateUrl="../Pacientes/PacienteEdit.aspx?id=0&llamada=LaboScreening"
                                               Font-Bold="True" Font-Size="9pt" Font-Underline="True" ForeColor="#006666" 
                                               ToolTip="Crear un nuevo paciente en el SGH ">Nuevo Paciente</asp:HyperLink></td>
						
					</tr>
					<tr>
						<td  >
                                            &nbsp;</td>
						<td align="right" colspan="3">
                                          <INPUT id="txtOculto" style="WIDTH: 1px; HEIGHT: 1px" type="hidden" size="1" name="txtOculto"
        runat="server">
                        </td>
						
					</tr>
					<tr>
						<td   colspan="4">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idPaciente" 
                                                Font-Size="8pt" 
                                                Width="100%" CellPadding="1" 
                                ForeColor="#666666" 
                                AllowPaging="True" PageSize="13" 
                                
                                EmptyDataText="No se encontraron pacientes para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="3px" 
                                GridLines="Horizontal" onpageindexchanging="gvLista_PageIndexChanging">
               <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" 
                Font-Size="8pt" />
            <Columns>
             <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                            ommandName="Editar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="18px" HorizontalAlign="Center" Height="18px" />
                          
                        </asp:TemplateField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <ItemStyle Width="50%" />
                </asp:BoundField>
                 <asp:BoundField DataField="sexo" HeaderText="Sexo">
                     <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac.">
                     <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Protocolo" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                            ommandName="Editar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="18px" HorizontalAlign="Center" Height="18px" />
                          
                        </asp:TemplateField>
                  
                 
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
						
						<td   colspan="1" style="vertical-align: top">
                            &nbsp;</td>
                                            </tr>
					<tr>
						
						<td   colspan="1" style="vertical-align: top">
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                                            </tr>
                                            </table></td>
						
						
					</tr>
					
					</table>

</div>
			
 </asp:Content>
