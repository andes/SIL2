<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Procesa.aspx.cs" Inherits="WebLab.Resultados.WebForm1" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">        
 <div align="center" style="width:1000px; height:500px;">
 <br />
    <br />
     <br />
    <br /> <br />
    <br /> <br />
    <br />
    <asp:Label ID="lblTitulo" runat="server" Text="No se encontraron datos para la filtros de búsqueda ingresados." Font-Bold="True" ForeColor="#CC3300" Width="500px"></asp:Label>
    <br /> <br />
    <br />
    <asp:HyperLink ID="hypRegresar" AccessKey="r" ToolTip="Alt+Shift+R" runat="server" CssClass="myLink">Regresar</asp:HyperLink>
    </div>
   </asp:Content>
