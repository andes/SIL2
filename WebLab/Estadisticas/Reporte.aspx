<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="WebLab.Estadisticas.Reporte" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script> 
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
            >Descargar Detalle Por Deteterminacion</asp:LinkButton>
  
   &nbsp;<asp:Panel ID="pnlGrafico" runat="server">
   <div style="border: 1px solid #C0C0C0">
       <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
     

  
       <asp:Literal  ID="FCLiteral0" runat="server"></asp:Literal> 
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
    </asp:Content>