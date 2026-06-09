<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grafico.aspx.cs" Inherits="WebLab.Estadisticas.Grafico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estadisticas SIL</title>
    <%-- <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>--%>
   <%-- <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/chart.js"></script>--%>
    <script type="text/javascript" src="../script/chart/chart.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="border: 1px solid #333333; width: 850px; height:400px;" align="left">
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
        var colores = [
            '#1E88E5', // azul
            '#E53935', // rojo
            '#43A047', // verde
            '#FB8C00', // naranja
            '#8E24AA', // violeta
            '#00ACC1', // cyan
            '#FDD835', // amarillo
            '#6D4C41', // marron
            '#3949AB', // indigo
            '#D81B60', // rosa fuerte
            '#7CB342', // verde lima
            '#5E35B1', // violeta oscuro
            '#00897B', // teal
            '#EF6C00', // naranja oscuro
            '#C0CA33', // lima
            '#546E7A', // gris azulado
            '#8D6E63', // cafe claro
            '#EC407A', // rosa
            '#26A69A', // verde agua
            '#7E57C2', // lavanda
            '#FFA726', // naranja claro
            '#66BB6A', // verde claro
            '#29B6F6', // celeste
            '#FF7043', // coral
            '#9CCC65', // verde pastel
            '#AB47BC', // purpura
            '#26C6DA', // turquesa
            '#FFCA28', // amarillo fuerte
            '#BDBDBD', // gris
            '#8BC34A'  // verde manzana
        ];
       
        var opciones = {
              //responsive:true -> el gráfico se adapta al tamaño del contenedor.
            // maintainAspectRatio:false -> deja de forzar el cuadrado 1:1 típico del pie.
            responsive: true,
            maintainAspectRatio: false,
            plugins: {}
        };

        if (tipo === 'bar') {
            opciones.plugins.legend = {
                labels: {
                    boxWidth: 0,
                    boxHeight: 0
                }
            };
        }

        new Chart(ctx, {
            type: tipo,
            data: {
                labels: labels,
                datasets: [{
                    label: titulo,
                    data: datos,
                    backgroundColor: colores.slice(0, datos.length) //usa solamente la cantidad necesaria,evita repeticiones, y mantiene sincronizados labels ↔ colores.
                }]
            },
            options: opciones
        });
    </script>
</body>
</html>
