<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PesquisaNeonatalView.aspx.cs" Inherits="WebLab.Resultados.PesquisaNeonatalView" %>

<%@ Register src="~/Resultados/PesquisaNeonatal.ascx" tagname="PesquisaNeonatal" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  
     
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />  
      <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
        
   
 
     
 
    
     <style type="text/css">
         .style1
         {
             border-style: none;
             font-size: 8pt;
             font-family: Arial, Helvetica, sans-serif;
             background-color: #F3F3F3;
             color: #333333;
             font-weight: normal;
             width: 88px;
         }
         .style2
         {
             width: 312px;
         }
     </style>
        
   
 
     
 
    
     </head>

<body> 

    <form id="form1" runat="server">
    
    <uc1:PesquisaNeonatal ID="PesquisaNeonatal2" runat="server" />
    
</form>
</body>