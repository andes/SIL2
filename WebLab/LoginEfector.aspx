<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginEfector.aspx.cs" Inherits="WebLab.LoginEfector" %>

<%@ Register Src="~/loginSIL.ascx" TagPrefix="uc1" TagName="loginSIL" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sistema de Laboratorio</title>
   
 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.1/jquery.slim.min.js"></script>

 
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
   
</head>
 <body class="bg-purple">

    <form id="form1" runat="server">
        
         
    <div class="container">
            <img src="App_Themes/default/images/logo.png"  class="d-flex justify-content-center h-100"/> 
                 
    <div class="row">
      	  <div class="col-md-12 inputGroupContainer">
                  <br />
	<div class="panel-body" style="border-top: 3px solid #337ab7;padding-top: -5px; border-top-color: #000000;">
     <br />
      <h4 class="display-1"> Usuario: <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label></h4>
        
         <h4 class="display-1">     Apellido y Nombre:  <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></h4>
     

        <h3 class="display-1"><small>Seleccione el efector al que desea ingresar</small> </h3>
        
      
           <br />
            <asp:DropDownList ID="ddlEfector" Font-Size="Large" runat="server" Width="400px" class="form-control input-sm">
            </asp:DropDownList> 
        <br />
            <asp:Button ID="btnAceptar"  CssClass="btn btn-primary" Width="100px"  runat="server" OnClick="btnAceptar_Click" Text="Aceptar" ValidationGroup="0" />
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlEfector" ErrorMessage="Seleccione Efector" MaximumValue="99999" MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
            <br />
       </div>
              </div>
        </div>
        </div>
       
   
    
    </form>
</body>
</html>
