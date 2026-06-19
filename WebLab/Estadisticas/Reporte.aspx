<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="WebLab.Estadisticas.Reporte" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
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
  
    
      <div align="left" style="width: 1200px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
  <h4> <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
     
  
 
    <asp:Label ID="lblFiltro" runat="server"  
        Text="Label"></asp:Label></h4> 
   
         <asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" onclick="imgPdf_Click" 
            ToolTip="Exportar a Pdf" />
&nbsp;
        
        &nbsp;
        <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" />
        &nbsp;<br />
      
       <asp:LinkButton 
                            ID="lnkDetallePorDet" runat="server" CssClass="myLink" OnClick="lnkDetallePorDet_Click"  
            >Descargar Detalle Por Determinacion</asp:LinkButton>
  
   &nbsp;<asp:Panel ID="pnlGrafico" runat="server">
   <div style="border: 1px solid #C0C0C0; width: 400px; " >
      
       <canvas  id="miGrafico" visible="false"></canvas>  &nbsp &nbsp <canvas   id="miGrafico10" visible="false"></canvas>
         
      
      
       <asp:Label ID="lblInforme" runat="server" Text="Label" Visible="False"></asp:Label>
       <asp:Label ID="lblTipo" runat="server" Text="Label" Visible="False"></asp:Label>
    </div>
    </asp:Panel>
    </div>
          <div class="panel-body">	
    <div id="printableArea"  style="border: 1px solid #C0C0C0">
       <asp:GridView ID="gvEstadistica" runat="server" CellPadding="0" 
          CssClass="table table-bordered bs-table"  ShowFooter="True" Width="100%" Font-Size="9pt" 
           onrowdatabound="gvEstadistica_RowDataBound">
          
             <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
           <FooterStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
       </asp:GridView>
        
  </div>
              </div>
    <div class="panel-footer">	
  <img src="../App_Themes/default/images/imprimir.jpg" onclick="printDiv('printableArea')" /> 
      
       <asp:LinkButton 
                            ID="lnkRegresar" runat="server" CssClass="myLink"  
           ValidationGroup="0" onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
  
   </div>
              </div>
          </div>
  
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
                var subtitulo = <%= string.IsNullOrEmpty(Subtitulo) ? "\"\"" : Subtitulo %>;

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




            const ctx10 = document.getElementById('miGrafico10');
         
            var labels2 = <%= string.IsNullOrEmpty(LabelsJson2) ? "[]" : LabelsJson2 %>;
            var datos2 = <%= string.IsNullOrEmpty(DatosJson2) ? "[]" : DatosJson2 %>;
            var tipo2 = <%= string.IsNullOrEmpty(TipoGrafico2) ? "\"pie\"" : TipoGrafico2 %>;
            var titulo2 = <%= string.IsNullOrEmpty(TituloJson2) ? "\"\"" : TituloJson2 %>;

            if (!datos2 || datos2.length === 0) {
                document.getElementById('miGrafico10').style.display = 'none';
            }
            else {

                var opciones2 = {
                    plugins: {
                        title: {
                            display: true,
                            text: titulo2
                        },
                    }
                };

                if (tipo2 === 'bar') {
                    opciones2.plugins.legend = {
                        labels: {
                            boxWidth: 0,
                            boxHeight: 0
                        }
                    };
                }
                new Chart(ctx10, {
                    type: tipo2,
                    data: {
                        labels: labels2,
                        datasets: [{
                            label: titulo2,
                            data: datos2,
                            backgroundColor: colores.slice(0, datos2.length) //usa solamente la cantidad necesaria,evita repeticiones, y mantiene sincronizados labels ↔ colores.
                        }]
                    },
                    options: opciones2
                });
            }

          
               
               



            
           
           
        </script>


 
    </asp:Content>