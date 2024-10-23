<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="login.ascx.cs" Inherits="WebLab.login" %>

<asp:Login ID="Login1" runat="server" 
        DisplayRememberMe="False" LoginButtonText="Acceder" 
        onauthenticate="Login1_Authenticate" 
        PasswordRequiredErrorMessage="Contraseña es requerida" 
        TitleText="Nueva autenticación de usuario" UserNameLabelText="Usuario:" 
        UserNameRequiredErrorMessage="Usuario es requerido"
                    Width="350px">
    <LayoutTemplate>
        <table border="0" cellpadding="4" cellspacing="0" 
            style="border-collapse:collapse;">
            <tr>
                <td>
                    <table  style="width:350px;">
                        <tr>
                            
                            <td>
                                   <div class="form-group">
    <label for="ejemplo_password_1">Usuario</label>

                                <asp:TextBox ID="UserName" runat="server" class="form-control input-sm" placeholder="usuario"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Usuario es requerido" ToolTip="Usuario es requerido" ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            
                            <td>
                                 <div class="form-group">
    <label for="ejemplo_password_1">Contraseña</label>
    
                                                             <asp:TextBox ID="Password" runat="server"  class="form-control input-sm" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Contraseña es requerida" 
                                    ToolTip="Contraseña es requerida" ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
  </div>

        
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" style="color:Red;">
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="LoginButton" runat="server"  CommandName="Login" 
                                    class="btn btn-primary" Width="100px" Text="Acceder" 
                                    ValidationGroup="ctl00$Login1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </LayoutTemplate>
    </asp:Login>
