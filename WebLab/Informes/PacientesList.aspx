<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PacientesList.aspx.cs" Inherits="WebLab.Informes.PacientesList" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %> 
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript">


          $(function () {
              $("#<%=txtFechaDesde.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });

          $(function () {
              $("#<%=txtFechaHasta.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });
 
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
<br />
<br />
<div align=left>
<br />
      <table  width="1000px"  
                     style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000000" 
                     cellpadding="1" cellspacing="1" >
<tr><td><br />


				 <table  width="650px" align="center" 
                     
                     
                   
                     cellpadding="1" cellspacing="1" class="myTabla" 
        style="width: 650px" >
					<tr>
						<td colspan="4" class="mytituloTabla" >LISTA DE PACIENTES CON RESULTADOS URGENTES <hr /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >Fecha Desde:</td>
						<td style="width: 210px">
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="myTexto" 
                                style="width: 70px" title="Ingrese la fecha de inicio"  /></td>
				<td class="myLabelIzquierda" style="width: 150px" >
                            Fecha Hasta:</td>
						<td style="width: 144px">
                    <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 70px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="2" class="myTexto" 
                                title="Ingrese la fecha de fin"  /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            Origen:  
                                            </td>
                                            <td colspan="3">
                                        
					
				
                           
                           
                                                <asp:RadioButtonList ID="rdbOrigen" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="0" Selected="True">TODOS LOS URGENTES</asp:ListItem>
                                                    <asp:ListItem Value="2">INTERNACION</asp:ListItem>
                                                    <asp:ListItem Value="3">GUARDIA</asp:ListItem>
                                                </asp:RadioButtonList>
                           
                           
                            </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="4" >
                                           <hr /></td>
						
					</tr>
					<tr>
						<td style="width: 133px"  >
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0" 
                                                onclick="btnBuscar_Click" CssClass="myButton" TabIndex="24" Width="77px" 
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" />
                        </td>
						<td align="right" colspan="3">
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td style="width: 133px"  >
                                            </td>
						<td align="right" colspan="3">
                                            </td>
						
					</tr>
					<tr>
						<td   colspan="4" style="vertical-align: top">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idPaciente" Font-Size="10pt" 
                                                Width="100%" CellPadding="2" 
                                ForeColor="#666666" PageSize="20" 
                                
                                EmptyDataText="No se encontraron pacientes con historia clinica para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="3px" 
                                GridLines="Horizontal" EnableModelValidation="True" Font-Bold="True">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" 
                Font-Size="8pt" />
            <Columns>
                <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="60%" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac.">
                     <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:BoundField DataField="sexo" HeaderText="Sexo" />
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/zoom.png" 
                            CommandName="Editar" ToolTip="Visualizar historial" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
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
                   
                        <br />
                            &nbsp;</td>
						
					</tr>
					</table>
						




</td></tr>
   
 </table>
   
 </div>
 </asp:Content>