<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePesquisa.aspx.cs" Inherits="WebLab.Estadisticas.ReportePesquisa" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

     <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>

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
    <style type="text/css">
        .style3
        {
        }
        .style5
        {
            width: 643px;
        }
        .myButton
        {}
        </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
<br />&nbsp;&nbsp;<br />
         
    <div>
    <table width="800px">
    <tr>
<td class="mytituloTabla" align="left" colspan="7"> 
  <br />
 
    Estadisticas Pesquisa Neonatal
    <hr />
    </td>
</tr>
    <tr>
<td class="myLabelIzquierda" align="left"> 
  
 
    &nbsp;Desde:</td>
<td class="style3"  align="left"> 
  
 
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="myTexto" 
                                style="width: 70px" title="Ingrese la fecha de inicio"  /></td>
<td class="myLabelIzquierda"  align="left"> 
  
 
    &nbsp;Hasta:</td>
<td class="style3"  align="left"> 
  
 

                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="myTexto" 
                                style="width: 70px" title="Ingrese la fecha de fin"  /></td>
<td class="myLabelIzquierda"  align="left"> 
  
 
    Reporte:</td>
<td class="style3"  align="left"> 
  
 

        <asp:DropDownList ID="ddlVariable" runat="server" AutoPostBack="True"  CssClass="ddlBorde"
            onselectedindexchanged="ddlVariable_SelectedIndexChanged">
            <asp:ListItem>Muestras Pesquisadas</asp:ListItem>
            <asp:ListItem>Muestras por Patologia</asp:ListItem>
            <asp:ListItem>Repeticiones</asp:ListItem>
            <asp:ListItem>Tipo de Alimentacion</asp:ListItem>
            <asp:ListItem>Antibiotico</asp:ListItem>
            <asp:ListItem>Ingesta Leche 24hs.</asp:ListItem>
            <asp:ListItem>Transfusion</asp:ListItem>
            <asp:ListItem>Corticoides</asp:ListItem>
            <asp:ListItem>Dopamina</asp:ListItem>
            <asp:ListItem>Corticoides Materno</asp:ListItem>
            <asp:ListItem>Resultado</asp:ListItem>
        </asp:DropDownList>
        </td>
<td class="style3"  align="left" rowspan="2" style="vertical-align: top"> 
  
 
    <asp:Button CssClass="myButton" Width="120px" 
                                ID="btnBuscar" runat="server" 
        onclick="Button1_Click" Text="Generar Reporte" Height="30px" 
        ValidationGroup="0" />
        </td>
</tr>
    <tr>
<td class="myLabelIzquierda" align="left" colspan="4"> 
  
 
    <asp:CustomValidator ID="cvFechas" runat="server" 
        ErrorMessage="Fechas de inicio y de fin" 
        onservervalidate="cvFechas_ServerValidate" ValidationGroup="0">Debe ingresar fechas de inicio y fin</asp:CustomValidator>
        </td>
<td class="myLabelIzquierda"  align="left"> 
  
 
    &nbsp;</td>
<td class="style3"  align="left"> 
  
 

        <asp:DropDownList Visible="false" ID="ddlResultado" runat="server" AutoPostBack="True" CssClass="ddlBorde"
            onselectedindexchanged="ddlResultado_SelectedIndexChanged">
        </asp:DropDownList>
        </td>
</tr>
    <tr>
<td class="myLabelIzquierda" align="left" colspan="7"> 
  <hr /></td>
</tr>
    <tr>
<td class="myLabelIzquierda" align="left" colspan="7"> 
<asp:Panel id="muestraPesquisadas" runat="server" Visible="false">
<table>
<tr>
<td colspan="2" style="vertical-align: top" class="style5"> 
<div class="myLabelIzquierda">
<table width="100%">
<tr><td><asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" Width="20px" /></td>
<td align="right"><asp:ImageButton ID="imgExcelDetallePacientes" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif"
            ToolTip="Exportar a Excel" Width="20px" 
        onclick="imgExcelDetallePacientes_Click" />&nbsp;Detalle de Pacientes</td>
</tr></table>

 </div>   
        <asp:GridView ID="gv1" runat="server" Width="800px" ondatabound="gv1_DataBound" 
            onrowdatabound="gv1_RowDataBound" ShowFooter="True" BackColor="#CCCCCC" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
            CellSpacing="2" ForeColor="Black" 
        EmptyDataText="No se encontraron datos para las fechas ingresadas">
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>
   
    
    <br />
</td>
</tr>

           
            <tr>
            <td>
               <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </td>

            <td>
            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
            </td>
            </tr>
  
</table>
 
   </asp:Panel>
        </td>
</tr>
    <tr>
<td class="myLabelIzquierda" align="left" colspan="7"> 
  
 
  <asp:Panel id="muestraporpatologia" runat="server" Visible="false">
   <asp:ImageButton ID="imgExcel0" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel0_Click" 
            ToolTip="Exportar a Excel" Width="20px" />
        <asp:GridView ID="gv2" runat="server" Width="800px" 
            onrowdatabound="gv2_RowDataBound" ShowFooter="True" BackColor="#CCCCCC" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
            CellSpacing="2" ForeColor="Black" 
          EmptyDataText="No se encontraron datos para las fechas ingresadas" >
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
    </asp:GridView>

        
   </asp:Panel>

        </td>
</tr>
    <tr>
<td class="myLabelIzquierda" align="left" colspan="7"> 
  
 
        <asp:Panel ID="pnlGeneral" runat="server" Visible="false">
        <table>
        <tr>
        <td colspan="2">
            <asp:ImageButton ID="imgExcel1" runat="server" 
                ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel1_Click" 
                ToolTip="Exportar a Excel" Width="20px" />
        <asp:GridView ID="gv3" runat="server" Width="800px" 
            onrowdatabound="gv3_RowDataBound" ShowFooter="True" BackColor="#CCCCCC" 
            BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="4" 
            CellSpacing="2" ForeColor="Black" 
                onselectedindexchanged="gv3_SelectedIndexChanged" 
                EmptyDataText="No se encontraron datos para las fechas ingresadas" >
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="White" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
            <br />
        </td>
        
        </tr>


        <tr>
        <td style="vertical-align: top">
            <asp:Literal ID="Literal4" runat="server"></asp:Literal>
        </td>
        <td align="right" rowspan="2" style="vertical-align: top">
            <asp:Literal ID="Literal3" runat="server"></asp:Literal>
        </td>
        </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Literal ID="Literal5" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
            
            <br />
        </asp:Panel>
        </td>
</tr>
</table>
   
    </div>
 
                


    </asp:Content>