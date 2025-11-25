<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grafico.aspx.cs" Inherits="WebLab.Estadisticas.Grafico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadisticas SIL</title>
    <%-- <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>--%>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/chart.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="border: 1px solid #333333; width: 850px;" align="left">
            <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
            <canvas runat="server" id="miGrafico"></canvas>
        </div>
    </form>
    <script type="text/javascript">
        const ctx = document.getElementById('miGrafico');
        var labels = <%= LabelsJson %>;
        var datos = <%= DatosJson %>;
        var tipo = <%= TipoGrafico %>;
        var titulo = <%= TituloJson %>;

        new Chart(ctx, {
            type: tipo,
            data: {
                labels: labels,
                datasets: [{
                    label: titulo,
                    data: datos
                }]
            },
            //options: {
            //    plugins: {

            //        subtitle: {
            //            display: true,
            //            text: titulo
            //        }
            //    }
            //}
        });
    </script>
</body>
</html>
