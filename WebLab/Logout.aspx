<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="WebLab.Logout" %>

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

     <br />  
            <img src="App_Themes/default/images/logo.png"  class="d-flex justify-content-center h-100"/>  
                 <br />
                <br />
               
             <div style="border-top: 10px solid #337ab7; border-top-color: #333333; border-top-width: 5px;">
    <div class="row"  >
      	<div class="d-flex justify-content-center h-100">
              
	 <h3 class="display-1">Sistema Centralizado de Laboratorios</h3>
                 <br />
              <br />
       <uc1:loginSIL runat="server" ID="loginSIL" />
       </div>
        </div>
        </div>
       </div>
   
    
    </form>
</body>
</html>
