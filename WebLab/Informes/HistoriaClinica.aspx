<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoriaClinica.aspx.cs" Inherits="WebLab.Informes.HistoriaClinica" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
<script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    
<title>LABORATORIO</title> 

    <style type="text/css">
        .style1
        {
            border-style: none;
            font-size: 9pt;
            font-family: Candara;
            background-color: #FFFFFF;
            color: #333333;
            font-weight: bold;
            text-transform: inherit;
        }
    </style>


</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">     

    <div align="center" style="width: 900px" class="form-inline"  >

         <div id="pnlTitulo"  runat="server" class="panel panel-primary">
                    <div class="panel-heading">
                         Histórico de Analisis
                        </div>
                      
                <div class="panel-body">	
               
       <table  width="95%" 
                     style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000000" 
                     cellpadding="1" cellspacing="1" align="center">
  
       
        <tr>
            <td>
              <asp:Label ID="lblNumeroDocumento" runat="server" Font-Bold="True" Font-Names="Arial" 
                                Font-Size="12pt" ForeColor="#333333" 
                                Text="26982063 - PINTOS BEATRIZ CAROLINA"></asp:Label>
                <asp:Label ID="lblPaciente" runat="server" Font-Bold="True" Font-Names="Arial" 
                                Font-Size="12pt" ForeColor="#333333" 
                                Text="26982063 - PINTOS BEATRIZ CAROLINA"></asp:Label>
               
            </td>
            <td align="right">
                <%-- <asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" onclick="imgPdf_Click" 
            ToolTip="Exportar a Pdf" />
&nbsp;
        <asp:ImageButton ID="imgImprimir" runat="server" 
            ImageUrl="~/App_Themes/default/images/imprimir.jpg" onclick="imgImprimir_Click" 
            ToolTip="Imprimir reporte" />
        &nbsp;
        <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" />--%>
        &nbsp;<asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" onclick="imgPdf_Click" 
            ToolTip="Exportar a Pdf" />
            </td>
        </tr>
        <tr>
            <td class="myLabelIzquierda">
                Sexo:   &nbsp;<asp:Label ID="lblSexo" runat="server" Text="Label" CssClass="myLabel"></asp:Label>
               
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="myLabelIzquierda">
                Fecha de Nacimiento:   &nbsp;<asp:Label ID="lblFechaNacimiento" runat="server" 
                    Text="Label" CssClass="myLabel"></asp:Label>
               
            </td>
            <td>
                </td>
        </tr>
        <tr>
            <td class="myLabelIzquierda">
                Info de contacto:   &nbsp;<asp:Label ID="lblContacto" runat="server" Text="Label" 
                    CssClass="myLabel"></asp:Label>
               
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="myLabelIzquierda">
                Evolución de:&nbsp; <asp:Label ID="lblAnalisis" runat="server" Font-Bold="True" Font-Size="12pt" 
                    ForeColor="#CC3300" Text="Label"></asp:Label>
               
            </td>
            <td>
                &nbsp;</td>
        </tr>
      
        <tr>
            <td colspan="2">
               <asp:Panel ID="pnlGrafico" runat="server">
         </asp:Panel>
</td>
        </tr>
        <tr>
            <td colspan="2">
            
            </td>
        </tr>
      
    </table>
   <div style="border: 1px solid #C0C0C0">
       <asp:Literal ID="FCLiteral" runat="server"></asp:Literal>
</div>
 
                    </div>
             <div class="panel-footer">
                 
                 
                     <asp:GridView ID="gvHistorico" runat="server" AutoGenerateColumns="False" 
                    BorderStyle="Solid" BorderWidth="1px"    CssClass="table table-bordered bs-table"
                    CellPadding="2" DataKeyNames="idProtocolo" ForeColor="#333333" Width="100%" 
                    
                    
                    EmptyDataText="No se encontraron datos para los filtros de búsqueda ingresados" 
                    onrowdatabound="gvHistorico_RowDataBound" BorderColor="#666666">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="numero" HeaderText="Protocolo">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="solicitante" HeaderText="Solicitante">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="resultadoNum" HeaderText="Resultado">
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true"/>
                        </asp:BoundField>
                         <asp:BoundField DataField="resultadoCar" HeaderText="Resultado">
                            <ItemStyle HorizontalAlign="Center" Font-Bold="true" />
                        </asp:BoundField>
                        <asp:BoundField DataField="unidad" HeaderText="U.Med">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ValorReferencia" HeaderText="V.Referencia">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="metodo" HeaderText="Metodo">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="firmante" HeaderText="Validado por" />
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView> <asp:LinkButton 
                            ID="lnkRegresar" runat="server" CssClass="myLink"  
           ValidationGroup="0" onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
                 </div>
    </div>
        </div>
 </asp:Content>
