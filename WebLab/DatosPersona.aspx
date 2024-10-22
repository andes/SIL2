<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="DatosPersona.aspx.cs" Inherits="WebLab.DatosPersona" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Bienvenido</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>Primer Ingreso</p>
          <p>Se ha identificado al usuario con los siguientes datos. Si son correctos haga clic en Confirmar, 
            se habilitará su acceso al sistema y se registrarán en el auditoria las consultas realizadas</p>
        <p>Documento</p>
        <asp:TextBox ID="txtDocumento"  Width="300px" runat="server" Enabled="False"></asp:TextBox>
          <p>Apellido</p>
        <asp:TextBox ID="txtApellido" runat="server" Width="300px" Enabled="False"></asp:TextBox>
          <p>Nombres</p>
        <asp:TextBox ID="txtNombre" runat="server" Width="300px" Enabled="False"></asp:TextBox>
        <p>Titulo</p>
         <asp:TextBox ID="txtTitulo" runat="server" Enabled="False"></asp:TextBox>
        <br />
      
        <asp:Button ID="btnGuardar" runat="server" Text="Confirmar" OnClick="btnGuardar_Click1" />
    </div>
          </form>
</body>
</html>
