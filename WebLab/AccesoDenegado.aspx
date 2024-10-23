<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccesoDenegado.aspx.cs" Inherits="WebLab.AccesoDenegado" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

<div align="center" 
        style="color: #000000; font-family: Candara; font-size: 12px; font-weight: bold">
    <br />
    <img src="App_Themes/default/images/sysmessage.png" />
  <br />
    <br />Acceso Denegado
<br />
<br />
Verifique sus permisos de acceso con el Administrador del Sistema
    <br />
<br />
    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
     <br />
<br />
          <asp:Button ID="btnAceptar"  CssClass="btn btn-primary" Width="100px"  runat="server"   Text="Volver" OnClick="btnAceptar_Click"  />
</div>
          
 
       


  
       



			
			
	
</asp:Content>