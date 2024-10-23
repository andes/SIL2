<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grafico.aspx.cs" Inherits="WebLab.Estadisticas.Grafico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadisticas SIL</title>
     <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="border: 1px solid #333333; width: 850px;" align="left">
                                    <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
                                    
                                        </div>
    </form>
</body>
</html>
