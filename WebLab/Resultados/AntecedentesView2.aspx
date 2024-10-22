<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AntecedentesView2.aspx.cs" Inherits="WebLab.Resultados.AntecedentesView2" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
      
     <link rel="stylesheet" type="text/css" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css"/> 
      
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
    
    <form id="form1" runat="server">
    <div  style="width: 750px" class="form-inline" >
      <div class="panel panel-default" runat="server"  id="printableArea" >
                    <div class="panel-heading">
                                                                          
                  <h5><strong><asp:Label ID="lblPaciente" runat="server" Text="Label"></asp:Label></strong></h5>  
                
                        </div>
                         
               <div class="panel-body">  
                   <table>
                       <tr>
                           <td>
                                Item:   <anthem:DropDownList ID="ddlItem" runat="server"  Width="200px" class="form-control input-sm"
                        onselectedindexchanged="ddlItem_SelectedIndexChanged" AutoCallBack="true">
                    </anthem:DropDownList>       
                           </td>

                           <td>   SubItem:<anthem:DropDownList ID="ddlSubItem" runat="server"  Width="200px" class="form-control input-sm">
                    </anthem:DropDownList>                                                              
                                                                               
                                  </td>

                           <td>                               
                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                        ControlToValidate="ddlItem" ErrorMessage="Analisis" MaximumValue="9999999" 
                        MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator></td>

                           <td>
                                    <asp:Button ID="btnVerAntecendente" runat="server"  CssClass="btn btn-primary"  
                        onclick="btnVerAntecendente_Click" Text="Buscar" Width="120px" 
                        ValidationGroup="0" />
                           </td>
                       </tr>
                   </table>                                                           
                                                                          
              
                                                              
               
                 
          </div>

               <div class="panel-footer">
                                    <div  style="width:680px;height:220pt;overflow:scroll;border:1px solid #CCCCCC;">                            
                    <asp:GridView ID="gvAntecedente" runat="server" 
                        CellPadding="1" EmptyDataText="No se encontraron antecedentes." Font-Size="9pt" 
                        Width="90%" CssClass="mytable1" Font-Names="Verdana">
                        <HeaderStyle BackColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                            ForeColor="#333333" BorderColor="#666666" />
                        <RowStyle Font-Names="Verdana" Font-Size="9pt" />
                    </asp:GridView>
                    </div>
                 </div>
          </div>
        </div>
                                               
    
  
    </form> 
     
</body>
</html>
