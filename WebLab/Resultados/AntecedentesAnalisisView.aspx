<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AntecedentesAnalisisView.aspx.cs" Inherits="WebLab.Resultados.AntecedentesAnalisisView" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>       
  
     <link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
     <link rel="stylesheet" type="text/css" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
     <script src="../bootstrap-3.3.7-dist/js/jquery.min.js"></script>  
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
       <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
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
