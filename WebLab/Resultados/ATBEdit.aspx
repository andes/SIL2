<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ATBEdit.aspx.cs" Inherits="WebLab.Resultados.ATBEdit"  %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
  
     
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
         .auto-style1 {
             width: 415px;
         }
         .auto-style6 {
             width: 76px;
         }
         .auto-style7 {
             width: 329px;
         }
     </style>
 
   

 
</head>
<body  style="height:500px">  
    <form id="form1" runat="server"  >           
  
             
           <table >           
                <tr>
                <td style="vertical-align: top" class="myLabelIzquierda">
                    <table width="415" class="auto-style1" >
                        <tr>
                            <td class="auto-style6">
                               <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Size="16pt"></asp:Label>                               
                            </td>
                            <td bgcolor="#E8E8E8" class="auto-style7">
                                   <asp:Label ID="lblGermen" runat="server" Text="Label"></asp:Label>
                            &nbsp;<asp:Label ID="lblMetodo" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="lblIdMetodo" runat="server" Text="Label" Visible="False"></asp:Label>
                                 &nbsp;( <asp:Label ID="lblPractica" runat="server" Text="Label"></asp:Label>
                            &nbsp;) </td>
                        </tr>
              
                        <tr>
                            <td class="style2" colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style2" colspan="2">
                               NUEVO ANTIBIOTICO<hr /></td>
                        </tr>
                        <tr>
                            <td class="auto-style6">
                                Perfil:</td>
                            <td class="auto-style7">
                                <anthem:DropDownList ID="ddlPerfilAntibiotico" runat="server" 
                                    AutoCallBack="True" 
                                    onselectedindexchanged="ddlPerfilAntibiotico_SelectedIndexChanged1" 
                                    Width="200px" CssClass="myList">
                                </anthem:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style6">
                                Antibiótico:</td>
                            <td class="auto-style7">
                                <anthem:DropDownList ID="ddlAntibiotico" runat="server" Width="200px" CssClass="myList">
                                    
                                </anthem:DropDownList>
                                
                                <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                    ControlToValidate="ddlAntibiotico" ErrorMessage="Antibiotico" 
                                    MaximumValue="99999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style6">
                                Result./Valor:</td>
                            <td class="auto-style7">
                                <asp:DropDownList ID="ddlResultado" runat="server" Width="80px">
                              <asp:ListItem Value="" ></asp:ListItem>
         <asp:ListItem Value="Resistente">Resistente</asp:ListItem>
         <asp:ListItem Value="Intermedio">Intermedio</asp:ListItem>
         <asp:ListItem Value="Sensible">Sensible</asp:ListItem>
         <asp:ListItem Value="Sensibilidad Disminuida">Sensibilidad Disminuida</asp:ListItem>
         <asp:ListItem Value="Apto para Sinergia">Apto para Sinergia</asp:ListItem>
         <%--<asp:ListItem Value="Sin Reactivo">SR</asp:ListItem>--%>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="ddlResultado" ErrorMessage="Resultado" 
                                    ValidationGroup="0">*</asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtValor" runat="server" CssClass="myTexto" Width="50px"></asp:TextBox>
                                <asp:Button ID="btnGuardarAntibiograma" runat="server" Text="Agregar" 
                                    onclick="btnGuardarAntibiograma_Click" ValidationGroup="0" CssClass="myButtonGrisLittle" Height="18px" Width="60px" 
                                   />
                            </td>
                        </tr>
                                             
                        <tr>
                            <td class="auto-style7" colspan="2">
                               <hr /></td>
                        </tr>
                                             
                        </table>
                    </td>
                </tr>
                
                  <tr>
                      <td  style="vertical-align: top" >
                          <div  style="width:415px;height:175pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC; background-color: #EFEFEF;"> 
                          <asp:GridView ID="gvAntiobiograma" runat="server" AutoGenerateColumns="False" 
                              CellPadding="3" DataKeyNames="idAntibiograma" Width="400px" 
                              onrowcommand="gvAntiobiograma_RowCommand" 
                              onrowdatabound="gvAntiobiograma_RowDataBound" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                              <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                              <RowStyle Font-Names="Verdana" Font-Size="8pt" Height="20px" ForeColor="#000066" />
                              <Columns>
                                  <asp:BoundField DataField="descripcion" HeaderText="Antibiótico" >
                                  <ItemStyle Width="100px" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="resultado" HeaderText="Resultado" >
                                  <ItemStyle Width="100px" />
                                  </asp:BoundField>
                                  <asp:BoundField DataField="valor" HeaderText="Valor" >
                                  <ItemStyle Width="50px" />
                                  </asp:BoundField>
                                 <asp:TemplateField HeaderText="Estado">
                                      <ItemTemplate>
                                          <asp:Label ID="lblEstado" runat="server" Text=""></asp:Label>
                                       
                                      </ItemTemplate>
                                      <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
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
                              <FooterStyle BackColor="White" ForeColor="#000066" />
                              <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Font-Size="8pt" Font-Names="Verdana" />
                              <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                              <SortedAscendingCellStyle BackColor="#F1F1F1" />
                              <SortedAscendingHeaderStyle BackColor="#007DBB" />
                              <SortedDescendingCellStyle BackColor="#CAC9C9" />
                              <SortedDescendingHeaderStyle BackColor="#00547E" />
                          </asp:GridView>
                         </div>
                      </td>
                </tr>
                  <tr>
                            <td class="style2" colspan="3">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                    HeaderText="Debe seleccionar:" ShowMessageBox="True" ShowSummary="False" 
                                    ValidationGroup="0" />
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                        ErrorMessage="Ya existe el antibiótico en el ATB. Verifique." 
                                        onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                   
                            </td>
                        </tr>
               
           </table>
      
         
 </form>
</body>
  
</html>
