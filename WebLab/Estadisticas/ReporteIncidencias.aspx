<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteIncidencias.aspx.cs" Inherits="WebLab.Estadisticas.ReporteIncidencias" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

   <%--  <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>--%>

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
     </div></td>

 <td>&nbsp;</td>
 <td align="left" style="vertical-align: top" colspan="2">
  <div style="border-style: solid; border-width: 1px"> <%--<asp:Literal ID="Literal2" runat="server"></asp:Literal>--%>
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
</asp:Content>