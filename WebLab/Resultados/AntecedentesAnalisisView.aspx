<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AntecedentesAnalisisView.aspx.cs" Inherits="WebLab.Resultados.AntecedentesAnalisisView" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>       
  
     <link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
     <link rel="stylesheet" type="text/css" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
     <script src="../bootstrap-3.3.7-dist/js/jquery.min.js"></script>  
     <script type="text/javascript" src="../script/chart/chart.js"></script>

      <script type="text/javascript">


            function printDiv(divName) {
                var printContents = document.getElementById(divName).innerHTML;
                var originalContents = document.body.innerHTML;
                document.body.innerHTML = printContents;
                window.print();
                document.body.innerHTML = originalContents;
            }
     
  </script>                  
    <script type="text/javascript">
        const ctx = document.getElementById('miGrafico');


        var labels = <%= string.IsNullOrEmpty(LabelsJson) ? "[]" : LabelsJson %>;
        var datos = <%= string.IsNullOrEmpty(DatosJson) ? "[]" : DatosJson %>;

        if (!datos || datos.length === 0) {
            document.getElementById('miGrafico').style.display = 'none';
        }
        else {

                var tipo = <%= string.IsNullOrEmpty(TipoGrafico) ? "\"pie\"" : TipoGrafico %>;
                var titulo = <%= string.IsNullOrEmpty(TituloJson) ? "\"\"" : TituloJson %>;
                var valorMinimo =  <%= string.IsNullOrEmpty(minimo) ? "\"\"" : minimo %>;
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

                var opciones = {   plugins: { } };

                if (valorMinimo !== null && valorMinimo !== undefined) {
                    opciones.scales = {
                        y: {
                            min: valorMinimo
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

            }

    </script>


 
</head>

<body> 
  <div id="printableArea">    
    <form id="form1" runat="server">
    
  
   
                                               
         <div align="left" style="width:790px">

              
                         <asp:Label  ID="lblPaciente" runat="server" Text="Label"></asp:Label>
             <hr />
                <asp:Label  ID="lblItem" runat="server" Text="Label"></asp:Label>
                      
                         <asp:ImageButton ID="imgPdf" runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg" onclick="imgPdf_Click" ToolTip="Exportar a Pdf" />
                     
               <asp:Panel ID="pnlGrafico" runat="server">
                     <hr />

                   <div >
                        <canvas  id="miGrafico" ></canvas> 
                  </div>
             </asp:Panel>     
              <div>
                <asp:GridView ID="gvHistorico" runat="server" AutoGenerateColumns="False" 
                     DataKeyNames="idProtocolo" Width="100%"  EmptyDataText="No se encontraron datos para los filtros de búsqueda ingresados"  
                    CssClass="table table-bordered bs-table" >
                    <Columns>
                        <asp:BoundField DataField="numero" HeaderText="Protocolo">
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha">
                           
                        </asp:BoundField>
                        <asp:BoundField DataField="solicitante" HeaderText="Solicitante">
                           
                        </asp:BoundField>
                        <asp:BoundField DataField="resultadoNum" HeaderText="Resultado">
                          
                        </asp:BoundField>
                    
                        <asp:BoundField DataField="unidad" HeaderText="U.Med">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="ValorReferencia" HeaderText="V.Referencia">
                         
                        </asp:BoundField>
                        <asp:BoundField DataField="metodo" HeaderText="Metodo">
                           
                        </asp:BoundField>
                        <asp:BoundField DataField="firmante" HeaderText="Validado por." />
                    </Columns>
                </asp:GridView>
             </div>
    </div>
                                               
    
  
    </form> 
     </div>
</body>
</html>
