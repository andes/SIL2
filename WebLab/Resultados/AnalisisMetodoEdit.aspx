﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnalisisMetodoEdit.aspx.cs" Inherits="WebLab.Resultados.AnalisisMetodoEdit" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>       
  
     <link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
     <link rel="stylesheet" type="text/css" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
     <script src="../bootstrap-3.3.7-dist/js/jquery.min.js"></script>  
      <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
               
  
 
</head>

<body> 
 
    <form id="form1" runat="server">         
                                               
         <div align="left" style="width:790px">

              
                      <h5>   <asp:Label  ID="lblPaciente" runat="server" Text="Label"></asp:Label></h5>
             
                <h5><asp:Label  ID="lblItem" runat="server" Text="Label"></asp:Label></h5>
                      
         <br />
             Presentación:               <asp:DropDownList ID="ddlPresentacion" runat="server"></asp:DropDownList>
                         <asp:RangeValidator ID="rvddlPresentacion" runat="server" ControlToValidate="ddlPresentacion" 
                                ErrorMessage="Presentacion" MaximumValue="999999" MinimumValue="1" Type="Integer" 
                                ValidationGroup="0">*</asp:RangeValidator>
                                        
              <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0"  Width="100px" CssClass ="btn btn-primary"
                                                onclick="btnGuardar_Click"    TabIndex="24" />

    </div>
                                               
    
  
    </form> 
     
</body>
</html>
