<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GraficoChart.ascx.cs" Inherits="WebLab.Estadisticas.GraficoChart" %>
<script type="text/javascript" src="../script/chart/chart.js"></script>

<canvas id="miGrafico" runat="server"></canvas>

<script type="text/javascript">
    function arrayVacio(arr) {
        if (!Array.isArray(arr) || arr.length === 0) return true;
        return arr.every(elemento => elemento === 0);
    }

    var ctx = document.getElementById('<%= miGrafico.ClientID %>');
    var labels = <%= string.IsNullOrEmpty(LabelsJson) ? "[]" : LabelsJson %>;
    var datos = <%= string.IsNullOrEmpty(DatosJson) ? "[]" : DatosJson %>;

    if (arrayVacio(datos)) {
        ctx.style.display = 'none';
    } else {
        var tipo = <%= string.IsNullOrEmpty(TipoGrafico) ? "\"pie\"" : TipoGrafico %>;
        var titulo = <%= string.IsNullOrEmpty(TituloJson) ? "\"\"" : TituloJson %>;
        var subtitulo = <%= string.IsNullOrEmpty(Subtitulo) ? "\"\"" : Subtitulo %>;
        var valorMinimo =  <%= string.IsNullOrEmpty(minimo) ? "\"\"" : minimo %>;
        var tituloX = <%= string.IsNullOrEmpty(tituloX) ? "\"\"" : tituloX %>;
        var tituloY = <%= string.IsNullOrEmpty(tituloY) ? "\"\"" : tituloY %>;


        var colores = [
            '#1E88E5','#E53935','#43A047','#FB8C00','#8E24AA','#00ACC1','#FDD835',
            '#6D4C41','#3949AB','#D81B60','#7CB342','#5E35B1','#00897B','#EF6C00',
            '#C0CA33','#546E7A','#8D6E63','#EC407A','#26A69A','#7E57C2','#FFA726',
            '#66BB6A','#29B6F6','#FF7043','#9CCC65','#AB47BC','#26C6DA','#FFCA28',
            '#BDBDBD','#8BC34A'
        ];

        var opciones = {
           
             plugins: {
                title: {
                    display: true,
                    text: titulo
                },
                subtitle: {
                    display: true,
                    text: subtitulo
                }
            }
        };

        if (tipo === 'bar') {
            opciones.plugins.legend = {
                labels: {
                    boxWidth: 0,
                    boxHeight: 0
                }
            };
        }

        if (tipo === 'pie') {
            opciones.scales = {
                x: { display: false },
                y: { display: false }
            };
        }

        //Si tiene valor minimo un escala, definimos la escala
        if (valorMinimo !== null && valorMinimo !== undefined && valorMinimo !== '') {
            opciones.scales = {
                x: {
                    title: {
                        display: true, text: tituloX 
                    }
                },
                y: {
                    min: valorMinimo,
                    ticks: {
                        callback: function (value) { return Number(value).toFixed(2); } //para que tenga solo dos decimales despues de la coma
                    },
                    title: {
                        display: true, text: tituloY 
                    }
                }
            };
        }
        console.log(valorMinimo)
        console.log(opciones.scales)
        new Chart(ctx, {
            type: tipo,
            data: {
                labels: labels,
                datasets: [{
                    label: titulo,
                    data: datos,
                    backgroundColor: tipo === 'line'
                        ? '#1E88E5'
                        : colores.slice(0, datos.length)
                }]
            },
            options: opciones
        });
    }
</script>
