<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloPermiso.aspx.cs" Inherits="WebLab.Resultados.ProtocoloPermiso"  %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css"
     
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />  
   

 


   

 
</head>
<body  style="height:500px">  
    <form id="form1" runat="server"  >           
  
             
         
      <h3> Protocolo nro.
         
        <asp:Label ID="lblProtocolo" runat="server" Text="Label"></asp:Label>
  </h3>
             
         
      
         
 
        <h5> Sólo Personal del Laboratorio Central Excepto para...</h5>
          
     
        <asp:CheckBoxList ID="chkPerfiles" runat="server">
        </asp:CheckBoxList>
  
             
         
      
         
        <br />
        <asp:Button ID="btnGuardar" runat="server" AccessKey="s" CssClass="btn btn-danger"   TabIndex="600" Text="Guardar" ToolTip="Alt+Shift+S: Guarda resultados" ValidationGroup="0" Width="100px" OnClick="btnGuardar_Click1" />
  
             
         
      
         
 </form>
</body>
  
</html>
