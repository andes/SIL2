<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ATBValida.aspx.cs" Inherits="WebLab.Resultados.ATBValida" EnableEventValidation="true"  %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head10" runat="server">
  
     
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />  
      <script language="javascript" type="text/javascript">
     


          function PreguntoEliminar() {
              if (confirm('¿Está seguro de eliminar el registro?'))
                  return true;
              else
                  return false;
          }
    </script>       
 
     <style type="text/css">
         .auto-style1 {}
         .auto-style3 {
             font-size: 10pt;
             font-family: Calibri;
             background-color: #FFFFFF;
             color: #333333;
             font-weight: bold;
             width: 64px;
         }
         .auto-style4 {
             width: 64px;
         }
         .auto-style5 {
             width: 58px;
         }
         .auto-style6 {
             width: 274px;
         }
         .auto-style7 {
             width: 600px;
         }
         .auto-style8 {
             width: 186px;
         }
     </style>
 
</head>
<body  style="height:650px">  
    <form id="form1" runat="server"  >           
  
             
           <table width="600px" >           
                <tr>
                <td style="vertical-align: top" class="myLabelIzquierda" colspan="2">
                    <table width="600px" class="auto-style7">
                        <tr>
                            <td class="auto-style3">
                                <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Size="22pt"></asp:Label>                               
                            </td>
                            <td class="auto-style1" colspan="3" style="background-color: #EEEEEE">
                                &nbsp;&nbsp;
                                <asp:Label ID="lblGermen" runat="server" Text="Label"></asp:Label>
                                &nbsp;(<asp:Label ID="lblMetodo" runat="server" Text="Label"></asp:Label>
                                )
                                <asp:Label ID="lblIdMetodo" runat="server" Text="Label" Visible="False"></asp:Label>
                                &nbsp;(<asp:Label ID="lblPractica" runat="server" Text="Label"></asp:Label>
                                )</td>
                        </tr>
                        <%--      <tr>
                            <td class="style2" style="vertical-align: top">
                                Mecanismos R:</td>
                            <td colspan="2">
                                <asp:Label ID="lblMecanismoResistencia" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="auto-style4">
                                &nbsp;</td>
                            <td colspan="3">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2" colspan="4">
                               NUEVO ANTIBIOTICO<hr /></td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                Perfil:</td>
                            <td colspan="3">
                                <anthem:DropDownList ID="ddlPerfilAntibiotico" runat="server" 
                                    AutoCallBack="True" 
                                    onselectedindexchanged="ddlPerfilAntibiotico_SelectedIndexChanged1" 
                                    Width="200px">
                                </anthem:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                Antibiótico:</td>
                            <td class="auto-style8">
                                <anthem:DropDownList ID="ddlAntibiotico" runat="server" Width="160px">
                                    
                                </anthem:DropDownList>
                                
                                <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                    ControlToValidate="ddlAntibiotico" ErrorMessage="Antibiotico" 
                                    MaximumValue="99999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                
                            </td>
                            <td class="auto-style5">
                                Resultado:</td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlResultado" runat="server" Width="80px">
                              <asp:ListItem Value="" ></asp:ListItem>
         <asp:ListItem Value="Resistente">Resistente</asp:ListItem>
         <asp:ListItem Value="Intermedio">Intermedio</asp:ListItem>
         <asp:ListItem Value="Sensible">Sensible</asp:ListItem>
         <asp:ListItem Value="Sensibilidad Disminuida">Sensibilidad Disminuida</asp:ListItem>
         <asp:ListItem Value="Apto para Sinergia">Apto para Sinergia</asp:ListItem>
         <%--<asp:ListItem Value="Sin Reactivo">SR</asp:ListItem>--%>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtValor" runat="server" CssClass="myTexto" Width="60px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="ddlResultado" ErrorMessage="Resultado" 
                                    ValidationGroup="0">*</asp:RequiredFieldValidator>
                                <asp:Button ID="btnGuardarAntibiograma" runat="server" Text="Agregar"  CssClass="myButtonGrisLittle"
                                    onclick="btnGuardarAntibiograma_Click" ValidationGroup="0" 
                                   />
                                
                            </td>
                        </tr>
                                              
                        <tr>
                            <td class="style2" colspan="4">
                              <hr /></td>
                        </tr>
                                              
                        </table>
                    </td>
                </tr>
             
                <tr>
                    <td  style="vertical-align: top" colspan="2" >
                            <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLittleLink" onclick="lnkMarcar_Click" >Marcar todas</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkDesMarcar" runat="server" CssClass="myLittleLink" onclick="lnkDesMarcar_Click" >Desmarcar</asp:LinkButton>
                                   
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                        ErrorMessage="Ya existe el antibiótico en el ATB. Verifique." 
                                        onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                   
                    </td>
                </tr>
                  <tr>
                      <td  style="vertical-align: top" colspan="2" >
                          <div  style="width:600px;height:200pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"" > 
                          <asp:GridView ID="gvAntiobiograma" runat="server" AutoGenerateColumns="False" DataKeyNames="idAntibiograma" Width="580px" 
                              onrowcommand="gvAntiobiograma_RowCommand" 
                              onrowdatabound="gvAntiobiograma_RowDataBound">
                              <Columns>
                                  <asp:BoundField DataField="descripcion" HeaderText="Antibiótico" >
                                  <ItemStyle Width="140px" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="resultado" HeaderText="Resultado" >
                                  <ItemStyle Width="80px" />
                                  </asp:BoundField>
                                  <asp:TemplateField HeaderText="Val">
                                      <ItemTemplate>
                                          <asp:CheckBox ID="chkValidar" runat="server"  />
                                      </ItemTemplate>
                                      <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                  </asp:TemplateField>
                                  <asp:BoundField DataField="valor" HeaderText="Valor" >
                                  <ItemStyle Width="50px" />
                                  </asp:BoundField>
                                 <asp:TemplateField HeaderText="Val.">
                                      <ItemTemplate>
                                          <asp:CheckBox ID="chkValidarValor" runat="server" />
                                      </ItemTemplate>
                                      <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                  </asp:TemplateField>
                                    <asp:BoundField DataField="resultadoFinal" HeaderText="Res. Final" >
                                  <ItemStyle ForeColor="#333399" Width="100px" />
                                  </asp:BoundField>
                                <asp:TemplateField HeaderText="Estado">
                                      <ItemTemplate>
                                          <asp:Label ID="lblEstado" runat="server" Font-Size="Small" />
                                      </ItemTemplate>
                                      <ItemStyle Height="20px" HorizontalAlign="Center" Width="80px" />
                                  </asp:TemplateField>
                                  <asp:TemplateField>
                                      <ItemTemplate>
                                          <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" 
                                              ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                                              OnClientClick="return PreguntoEliminar();" />
                                      </ItemTemplate>
                                      <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                  </asp:TemplateField>
                                
                              </Columns>
                              <HeaderStyle BackColor="#CCCCCC" Font-Names="Verdana" Font-Size="8pt" />
                              <RowStyle Font-Names="Verdana" Font-Size="8pt" />
                          </asp:GridView>
                            
                         </div>  
                                                 </td>
                </tr>
               
               
                  <tr>
                      <td  style="vertical-align: top" >
                                   
                                                 <asp:Button ID="btnDesValidar" runat="server" AccessKey="D" CssClass="myButtonGris" onclick="btnDesValidar_Click" 
                                                      TabIndex="600" Text="Desvalidar" ToolTip="Alt+Shift+D:Desvalida lo validado por el usuario actual"   />
                                   
                                                 </td>
                      <td  style="vertical-align: top" align="right" >
                          <asp:Button ID="btnValidarATB" runat="server" CssClass="myButtonGris" Text="Validar ATB" OnClick="btnValidarATB_Click" />
                                                 </td>
                </tr>
               
               
                  <tr>
                      <td  style="vertical-align: top" align="right" >
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                    HeaderText="Debe seleccionar:" ShowMessageBox="True" ShowSummary="False" 
                                    ValidationGroup="0" Width="235px" />
                                    </td>
                      <td  style="vertical-align: top" align="right" >
                          &nbsp;</td>
                </tr>
               
               
           </table>
      
         
 </form>
</body>
  
</html>
