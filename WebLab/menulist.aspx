<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="menulist.aspx.cs" Inherits="WebLab.menulist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <p>Nuevo menu</p>
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem Selected="True">sys_menu</asp:ListItem>
            <asp:ListItem>temp_mensaje</asp:ListItem>
        </asp:DropDownList><asp:Button ID="Button1" runat="server" Text="Borrar mensajes" OnClick="Button1_Click" />

        <p>&nbsp;</p>

                             <asp:GridView ID="gvProtocolosxEfector" runat="server" CssClass="table table-bordered bs-table"></asp:GridView>
  


    </div>

</asp:Content>
