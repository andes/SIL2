<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="loginSIL.ascx.cs" Inherits="WebLab.loginSIL" %>


 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

 
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
   
 


<asp:Login ID="Login1" runat="server" 
        DisplayRememberMe="False" LoginButtonText="Acceder" 
        onauthenticate="Login1_Authenticate" 
        PasswordRequiredErrorMessage="Contraseña es requerida" 
        TitleText="Nueva autenticación de usuario" UserNameLabelText="Usuario:" 
        UserNameRequiredErrorMessage="Usuario es requerido" 
                   >
    <LayoutTemplate>
       <div class="container" >
    <div class="row">
      	<div class="d-flex justify-content-center h-100">
            <div class="row">
                <div class="col-lg-6 col-md-8 mx-auto">

                    <!-- form card login -->
                    <div class="card rounded shadow shadow-sm">
                        <div class="card header" >
                      
                        </div>
                         <div class="card-body">
                                   <div class="form-group">
    <label for="ejemplo_password_1">Usuario</label>

                                <asp:TextBox ID="UserName" runat="server" class="form-control input-sm" placeholder="usuario"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="Usuario es requerido" ToolTip="Usuario es requerido" ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
                                       </div>
                             
                                 <div class="form-group">
    <label for="ejemplo_password_1">Contraseña</label>
    
                                                             <asp:TextBox ID="Password" runat="server"  class="form-control input-sm" TextMode="Password" placeholder="Contraseña"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                    ControlToValidate="Password" ErrorMessage="Contraseña es requerida" 
                                    ToolTip="Contraseña es requerida" ValidationGroup="ctl00$Login1">*</asp:RequiredFieldValidator>
  </div>
                             </div>
        
            </div>
                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            
                                <asp:Button ID="LoginButton" runat="server"  CommandName="Login" 
                               class="btn btn-primary" Width="120px" Text="Acceder" 
                                    ValidationGroup="ctl00$Login1" />
                    </div>
                </div>
            </div>
        </div>
       </div>                    
    </LayoutTemplate>
    </asp:Login>
