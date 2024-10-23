<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="calendarioView.aspx.cs" Inherits="WebLab.Turnos.calendarioView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
</head>

<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label CssClass="myLabelIzquierda" ID="item" runat="server" ></asp:Label><hr />
    <div align="left" style="border: 1px solid #C0C0C0; overflow:scroll; overflow-x:hidden; height:200px; background-color: #F8F8F8; width:300px">
    <asp:gridview CssClass="myLabelIzquierda" ID="gv" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None" EmptyDataText="No hay días habilitados." 
            AutoGenerateColumns="False" Width="260px">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Dia" HeaderText="Día"  />
            <asp:BoundField DataField="fecha" HeaderText="Fecha"  DataFormatString="{0:d}" />
            <asp:BoundField DataField="CantidadTurnosDisponibles" 
                HeaderText="Turnos disponibles" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" Font-Size="10pt" 
            Font-Names="Arial" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:gridview>
        </div>
    </div>
    </form>
</body>
</html>
