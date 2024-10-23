<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordEdit.aspx.cs" Inherits="WebLab.Usuarios.PasswordEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
 

   
  
   
    </asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
        <div class="panel panel-default">
           <div class="panel-heading">CAMBIO DE CONTRASEÑA
               </div>
     <div class="panel-body">
             <table >
                 <tr>
                     <td>
                    Usuario:</td>
                <td class="style2"  >
                    <asp:Label ID="lblUsuario" runat="server" Text="Label" 
                     class="form-control input-sm"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="myLabelIzquierda">
                    Contraseña Anterior:</td>
                <td class="style2"  >
                    <asp:TextBox ID="txtPasswordActual" runat="server" Width="350px" class="form-control input-sm"
                        MaxLength="50" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                        ControlToValidate="txtPasswordActual" ErrorMessage="Usuario" 
                        ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="myLabelIzquierda">
                    Nueva
                    Contraseña:</td>
                <td class="style2"  >
                    <asp:TextBox ID="txtPasswordNueva" runat="server" Width="350px" TextMode="Password" 
                       class="form-control input-sm" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                        ControlToValidate="txtPasswordNueva" ErrorMessage="Contraseña" 
                        ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="myLabelIzquierda">
                    Confirmación Contraseña:</td>
                <td class="style2"  >
                    <asp:TextBox ID="txtPasswordNueva1" runat="server" Width="350px" TextMode="Password" 
                 class="form-control input-sm" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword0" runat="server" 
                        ControlToValidate="txtPasswordNueva1" ErrorMessage="Contraseña" 
                        ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
           
          
            
        </table>
       
    </div>

             <div class="panel-footer">
                  <asp:Button ID="btnGuardar" runat="server" Text="Grabar" Width="60px"
                        onclick="btnGuardar_Click1"    CssClass="btn btn-primary" ValidationGroup="0" />

                  <asp:CompareValidator ID="CompareValidator1" runat="server" 
                         ControlToCompare="txtPasswordNueva" ControlToValidate="txtPasswordNueva1" 
                         ErrorMessage="La nueva contraseña no coincide con la confirmacion de la contraseña" 
                         ValidationGroup="0"></asp:CompareValidator>
                     <br />
                     <asp:CustomValidator ID="CustomValidator1" runat="server" 
                         ControlToValidate="txtPasswordActual" 
                         ErrorMessage="La contraseña actual es incorrecta. Verifique." 
                         onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        HeaderText="De completar datos requeridos:" 
                        ShowSummary="False" ValidationGroup="0" />
                 </div>
    
 </div>
</asp:Content>