<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinSesion.aspx.cs" Inherits="WebLab.FinSesion" %>

<%@ Register src="login.ascx" tagname="login" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sistema de Laboratorio</title>
      
 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.1/jquery.slim.min.js"></script>

 
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
</head>
<body>
    <form id="form1" runat="server">
   <div class="container">
        
        	<h3>
   <img src="App_Themes/default/images/Logo.png" /> SIL-Laboratorio Central
                </h3>
		 
		<br />
  <h4>  La sesión de usuario ha caducado. </h4>
        <br />
   <br />
  <h4> Vuelva a identificarse nuevamente en el sistema.</h4>
        <br />
        <br />
      <%--  <uc1:login ID="login1" runat="server" />--%>
       <asp:Button ID="btnSIPS" CssClass="btn btn-success" Width="200px" runat="server" Text="Login con SIPS" OnClick="btnSIPS_Click"  />
      <br />
       <h3> </h3>
        <asp:Button ID="btnLaboCentral" runat="server" CssClass="btn btn-primary" Width="200px"  Text="Login Laboratorio Central" OnClick="btnLaboCentral_Click"  />
   </div>
    </form>
</body>
</html>
