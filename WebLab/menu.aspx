<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="menu.aspx.cs" Inherits="WebLab.menu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <p>Nuevo menu</p>
        <p>Objeto</p>
        <asp:TextBox ID="txtObjeto"  Width="300px" runat="server"></asp:TextBox>
          <p>URL</p>
        <asp:TextBox ID="txtURL" runat="server" Width="300px"></asp:TextBox>
          <p>MenuSuperior</p>
        <asp:DropDownList ID="ddlIdMenuSuperior" runat="server"></asp:DropDownList>
        <p>Orden</p>
         <asp:TextBox ID="txtOrden" runat="server"></asp:TextBox>
        <p>Perfiles</p>
             <asp:DropDownList ID="ddlPerfil" runat="server"></asp:DropDownList>
        <p>Permiso de escritura-Perfil</p>
          <asp:TextBox ID="txtPerfilPermiso" runat="server"></asp:TextBox>
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
    </div>

</asp:Content>
