<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebLabResultados.Default" %> 
 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SIL - Sistema Informático de Laboratorio</title>
 
</head>
<body>

<%--<SCRIPT LANGUAGE="JavaScript">

 window.moveTo(0,0);
if (document.all) {
top.window.resizeTo(screen.availWidth,screen.availHeight);
}
else if (document.layers||document.getElementById) {
if (top.window.outerHeight<screen.availHeight||top.window.outerWidth<screen.availWidth){
top.window.outerHeight = screen.availHeight;
top.window.outerWidth = screen.availWidth;
}
}

</SCRIPT>--%>


    <form id="form1" runat="server">
    <div>
    
      Se ha producido un error....<br />
      URL inválida
        <asp:Label ID="lblError" runat="server" Text="Label"></asp:Label>
    </div>
    </form>
</body>
</html>
