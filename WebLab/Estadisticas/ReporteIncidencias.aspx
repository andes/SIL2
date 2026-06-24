<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteIncidencias.aspx.cs" Inherits="WebLab.Estadisticas.ReporteIncidencias" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

   <%--  <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>--%>
      <script type="text/javascript" src="../script/chart/chart.js"></script>
  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript">


          $(function () {
              $("#<%=txtFechaDesde.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });

          $(function () {
              $("#<%=txtFechaHasta.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });
 
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 



    <div align="left" style="width:1100px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"> Estadisticas Incidencias</h3>
                        </div>

				<div class="panel-body">	
   

                    <div>Efector: 
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                         </div>
<div>Desde:    
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio"  /> 
    </div>
                    <div>
    Hasta: 

                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  /> 
                        </div>
 

                    <div>
    <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary"
        onclick="btnBuscar_Click" Text="Generar Reporte" TabIndex="2" 
        Width="150px" />
         </div>
</div>

        <div class="panel-footer">		
   <asp:Panel ID="pnlIncidencia" runat="server">
 <table>
 <tr><td style="vertical-align: top">
    Incidencias Pre Recepcion 
  </td>
     <td style="vertical-align: top" align="right">
  <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" Width="20px" /></td>
 <td>&nbsp;</td>
 <td align="left" style="vertical-align: top"> Incidencias de Protocolos </td>
 <td align="right" style="vertical-align: top"> <asp:ImageButton ID="imgExcel0" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel0_Click" 
            ToolTip="Exportar a Excel" Width="20px" /> </td>
 </tr>
 
 <tr><td style="vertical-align: top" colspan="2">
     <asp:GridView ID="GridView1" runat="server" 
        EmptyDataText="No se encontraron datos para los filtros seleccionados" 
        ShowHeaderWhenEmpty="True" onrowdatabound="GridView1_RowDataBound" 
        ShowFooter="True" Width="500px" Font-Names="Verdana" Font-Size="9pt">
        <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
    </asp:GridView><br />
     </td>
 <td>&nbsp;&nbsp;</td>
 <td align="left" style="vertical-align: top" colspan="2"> <asp:GridView ID="gvProtocolos" runat="server" 
        EmptyDataText="No se encontraron datos para los filros seleccionados" 
        ShowHeaderWhenEmpty="True" onrowdatabound="GridView2_RowDataBound" 
           ShowFooter="True" Width="500px" Font-Names="Verdana" Font-Size="9pt" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Incidencia" HeaderText="Incidencia" />
            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
        </Columns>
        <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
    </asp:GridView>        <asp:Button ID="btnDetalleProtocolos" runat="server"  OnClick="btnDetalleProtocolos_Click" Text="Descargar Detalle Protocolos" Width="200px" />
        
    

     </td>
 </tr>
 
 <tr><td style="vertical-align: top" colspan="2">
 <div style="border-style: solid; border-width: 1px">
    <%-- <asp:Literal ID="Literal1" runat="server"></asp:Literal>--%>
     <canvas id="miGrafico"> </canvas>
     </div></td>

 <td>&nbsp;</td>
 <td align="left" style="vertical-align: top" colspan="2">
  <div style="border-style: solid; border-width: 1px"> <%--<asp:Literal ID="Literal2" runat="server"></asp:Literal>--%>
         <canvas id="miGrafico10"> </canvas>
  </div></td>
 </tr>
 
 </table>
    
     </asp:Panel> 
     <asp:Panel ID="pnlMensaje" runat="server">
     <b style="color: #FF0000">No se encontraron datos para los filtros propuestos.</b>
     </asp:Panel>
       	
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