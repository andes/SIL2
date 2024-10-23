<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AntecedentesView.aspx.cs" Inherits="WebLab.Resultados.AntecedentesView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  
     
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />  
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
   
                                               
        <table width="800">
            <tr>
                <td class="myLabelIzquierdaGde" colspan="4">                                                               
                    <asp:Label ID="lblPaciente" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
             <tr>
                <td class="myLabelIzquierda" colspan="3">                                                               
                  <hr /></td>
            </tr>
              <tr>
                <td class="myLabelIzquierda" >                                                               
                 </td>
            </tr>
            <tr>
                <td class="myLabelIzquierda">                                                               
                    Análisis:                                                               
                    </td>
                <td class="myLabelIzquierda">                                                               
                    <asp:DropDownList ID="ddlItem" runat="server">
                    </asp:DropDownList>                                                              
                    </td>
                <td >                                                               
                                                              
                    <asp:RangeValidator ID="RangeValidator1" runat="server" 
                        ControlToValidate="ddlItem" ErrorMessage="Analisis" MaximumValue="9999999" 
                        MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                                              
                    <asp:Button ID="btnVerAntecendente" runat="server"  CssClass="myButton"
                        onclick="btnVerAntecendente_Click" Text="Buscar" Width="120px" 
                        ValidationGroup="0" />
                </td>
                <td class="style4">                                                               
                                                              
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="myLabelIzquierda" colspan="3">                                                               
                  <hr /></td>
            </tr>
              <tr>
                <td class="myLabelIzquierda" >                                                               
                 </td>
            </tr>
            <tr>
                <td colspan="4">
                                                         
                    <asp:GridView ID="gvAntecedente" runat="server" 
                        CellPadding="1" EmptyDataText="No se encontraron antecedentes." Font-Size="9pt" 
                        Width="90%" CssClass="mytable1" Font-Names="Verdana">
                        <HeaderStyle BackColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                            ForeColor="#333333" BorderColor="#666666" />
                        <RowStyle Font-Names="Verdana" Font-Size="9pt" />
                    </asp:GridView>
                </td>
            </tr>
           <%-- <tr>
                <td colspan="3">
                    <br />                  
                    <img src="../App_Themes/default/images/imprimir.jpg" onclick="printDiv('printableArea')" />
                    </td>
            </tr>--%>
        </table>
                                               
    
  
    </form> 
     </div>
</body>
</html>
