<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FeriadoEdit.aspx.cs" Inherits="WebLab.Turnos.FeriadoEdit1" EnableEventValidation="true" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div>
    <table style="width: 100%;">
        <tr>
            <td rowspan="2" style="vertical-align: top" class="auto-style1">
                    <p style="color: #000000; font-style: italic; font-size: small; font-family: Arial, Helvetica, sans-serif">Seleccione fecha y haga clic en Guardar</p>
        <asp:Calendar ID="cldTurno" runat="server" BackColor="#FFFFCC" 
                                BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                                Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="200px" 
                               onselectionchanged="cldTurno_SelectionChanged" 
                              ShowGridLines="True" 
                                ToolTip="Seleccione la fecha de dia Feriado" Width="220px">
                                <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                <SelectorStyle BackColor="#FFCC66" />
                                <TodayDayStyle BackColor="#CCCCCC" />
                                <OtherMonthDayStyle ForeColor="#CC9966" />
                                <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                                <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                                    ForeColor="#FFFFCC" />
                            </asp:Calendar>
            
                <br />
        <asp:Label ID="lblFecha" runat="server" Font-Bold="True" Font-Size="12pt"></asp:Label>
        &nbsp;
        <asp:Button ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" Text="Guardar" ValidationGroup="0" />
                <br />
        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="La fecha ya existe en la lista de feriados" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
            </td>
            <td style="vertical-align: top" rowspan="2">
                 <div  style="width:198px; height:250pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;">
    <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idFeriado" OnRowCommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound" Width="100%" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
        <Columns>
            <asp:BoundField DataField="fecha1" HeaderText="Feriados" />
             <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton  ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#333333" />
        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="White" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F7F7F7" />
        <SortedAscendingHeaderStyle BackColor="#487575" />
        <SortedDescendingCellStyle BackColor="#E5E5E5" />
        <SortedDescendingHeaderStyle BackColor="#275353" />
        </asp:GridView>
 </div>
            </td>
           
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
       
    </table>

    <script type="text/javascript">


        function PreguntoEliminar() {
            if (confirm('¿Está seguro de eliminar?'))
                return true;
            else
                return false;
        }
    </script>
</div>
    </div>
    </form>
</body>
</html>
