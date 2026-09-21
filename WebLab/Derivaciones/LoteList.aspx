<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoteList.aspx.cs" Inherits="WebLab.Derivaciones.LoteList" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />
    <script type="text/javascript" src="../script/jquery.min.js"></script>
    <script type="text/javascript" src="../script/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../script/jquery.ui.datepicker-es.js"></script>
    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>
    <script type="text/javascript"> 

        $(function () {
            $("#<%=txtFechaDesde.ClientID %>").datepicker({
                maxDate: 0,
                minDate: null,

                showOn: 'button',
                buttonImage: '../App_Themes/default/images/calend1.jpg',
                buttonImageOnly: true
            });
        });

        $(function () {
            $("#<%=txtFechaHasta.ClientID %>").datepicker({
                maxDate: 0,
                minDate: null,

                showOn: 'button',
                buttonImage: '../App_Themes/default/images/calend1.jpg',
                buttonImageOnly: true
            });
        });

        function soloNumeros(input) {
            input.value = input.value.replace(/\D/g, '');
        }

        function CambiarEstado(IdLote, Efector){
            $('<iframe src="InformeLote.aspx?IdLote=' + IdLote + '&Destino='+ Efector + '" />').dialog({
                title: 'Cambiar Estado',
                autoOpen: true,
                width: 800,
                height: 400,
                modal: true,
                resizable: false,
                autoResize: true,
                open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },

                buttons: {
                    'Cerrar': function () {
                        $(this).dialog('close');
                        document.getElementById('<%= btnBuscar.ClientID %>').click();
                      }
                  },
                  overlay: {
                      opacity: 0.5,
                      background: "black"
                  }

              }).width(800);
        }
    </script>
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div align="left" style="width: 1200px" class="form-inline">


        <table width="1150px" align="center" class="myTabla">

            <tr>
                <td colspan="5">
                    <div id="pnlTitulo" runat="server" class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">
                                <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
                            </h3>
                        </div>

                        <div class="panel-body">
                            <table width="1150px" align="center">
                                <tr>
                                    <td class="myLabelIzquierda">Fecha Desde:</td>
                                    <td>
                                        <input id="txtFechaDesde" runat="server" type="text" maxlength="10" onblur="valFecha(this)"
                                            onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm" style="width: 100px" /></td>
                                    <td class="myLabelIzquierda">Fecha Hasta:</td>
                                    <td>
                                        <input id="txtFechaHasta" runat="server" type="text" maxlength="10" style="width: 100px" onblur="valFecha(this)"
                                            onkeyup="mascara(this,'/',patron,true)" tabindex="2" class="form-control input-sm" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="myLabelIzquierda">Lote Desde:</td>
                                    <td>
                                        <asp:TextBox ID="txtLoteDesde" runat="server" MaxLength="9" TabIndex="3" class="form-control input-sm" Onkeyup="soloNumeros(this)" Style="width: 100px" />
                                    </td>
                                    <td class="myLabelIzquierda">Lote Hasta:</td>
                                    <td>
                                        <asp:TextBox ID="txtLoteHasta" runat="server" MaxLength="9" TabIndex="4" class="form-control input-sm" Onkeyup="soloNumeros(this)" Style="width: 100px" />
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td class="myLabelIzquierda">Estado:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlEstado" runat="server" class="form-control input-sm" TabIndex="5"></asp:DropDownList>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td class="myLabelIzquierda">Efector Origen:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlEfectorOrigen" runat="server" ToolTip="Seleccione el efector" TabIndex="6" Width="250px" class="form-control input-sm" OnSelectedIndexChanged="ddlEfectorOrigen_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>

                                    <td class="myLabelIzquierda">Efector Destino:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlEfectorDestino" runat="server" ToolTip="Seleccione el efector" TabIndex="7" Width="250px" class="form-control input-sm"></asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td class="myLabelIzquierda">Estado:</td>
                                    <td>
                                        <asp:CheckBoxList ID="chkEstados" runat="server" RepeatDirection="Horizontal"  CssClass="checkbox checkbox-inline" OnSelectedIndexChanged="chkEstados_SelectedIndexChanged" AutoPostBack="true"/>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div class="panel-footer">
                           
                                            <asp:CustomValidator ID="cvValidar" runat="server" OnServerValidate="cvValidar_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                       
                            <asp:Panel ID="pnlLista" runat="server">
                                <table style="width: 100%;">
                                    <tr>
                                        <td align="left" style="vertical-align: top" >
                                            <div class="panel-body">
                                                <table width="100%" align="center">
                                                    <tr>
                                                        <td align="right">
                                                            <img alt="" src="../App_Themes/default/images/excelPeq.gif" />
                                                            <asp:LinkButton ID="lnkExcel" runat="server" CssClass="myLittleLink" OnClick="lnkExcel_Click" ValidationGroup="0">Exportar a Excel</asp:LinkButton>
                                                            <label>Orden:</label><asp:DropDownList ID="ddlOrden" runat="server">
                                                                   <asp:ListItem Selected="True" Value="Asc">Ascendente</asp:ListItem>
                                                                   <asp:ListItem Value="Desc">Descendente</asp:ListItem>
                                                               </asp:DropDownList>
                                                            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" OnClick="btnBuscar_Click" TabIndex="8" Text="Buscar" ValidationGroup="0" Width="77px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td>&nbsp;<asp:Label ID="CantidadRegistros" runat="server" ForeColor="Blue" />
                                            &nbsp;<asp:Label ID="CurrentPageLabel" runat="server" ForeColor="Blue" />
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">

                                            <asp:GridView ID="gvLista" runat="server" AllowPaging="True" CssClass="table table-bordered bs-table"
                                                AutoGenerateColumns="False" CellPadding="2" DataKeyNames="numero"
                                                EmptyDataText="No se encontraron lotes para los parametros de busqueda ingresados"
                                                GridLines="Horizontal"
                                                OnPageIndexChanging="gvLista_PageIndexChanging"
                                                PageSize="20" Width="100%" BackColor="White"
                                               
                                                OnRowDataBound="gvLista_RowDataBound" OnRowCommand="gvLista_RowCommand"
                                                >
                                                <PagerStyle HorizontalAlign="Center" CssClass="GridPager" />
                                                <Columns>
                                                    <asp:BoundField DataField="numero" HeaderText="Nro." SortExpression="numero">
                                                        <ItemStyle Width="5%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" SortExpression="fechaRegistro">
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="efectorOrigen" HeaderText="Efector Origen" SortExpression="efectorOrigen" />
                                                    <asp:BoundField DataField="idEfectorOrigen">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="efectorDestino" HeaderText="Efector Destino" SortExpression="efectorDestino" />
                                                    <asp:BoundField DataField="estado" HeaderText="Estado" SortExpression="estado" />
                                                    <asp:BoundField DataField="username" HeaderText="Usuario Gen." SortExpression="username" />
                                                    <asp:BoundField DataField="fechaGeneracion" HeaderText="Fecha Gen." SortExpression="fechaGeneracion"/>
                                                    <asp:BoundField DataField="fechaEnvio" HeaderText="Fecha Envio" SortExpression="fechaEnvio"/>
                                                    <asp:BoundField DataField="fechaIngreso" HeaderText="Fecha Ing." SortExpression="fechaIngreso" />
                                                    <asp:TemplateField HeaderText="Modificar">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="lnkEdit"  >
                                                                <asp:Image  runat="server" ImageUrl='~/App_Themes/default/images/editar.jpg'  />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cambiar estado">
                                                        <ItemTemplate>
                                                           <asp:LinkButton runat="server" ID="lnkCambiarEstado" >
                                                                <asp:Image  runat="server" ImageUrl='~/App_Themes/default/images/actualizar.gif'  />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Auditoria">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="lnkPDFAuditoria" >
                                                                 <asp:Image  runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg"  />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Informe Derivacion">
                                                        <ItemTemplate>
                                                            <asp:LinkButton runat="server" ID="lnkPDFImprimir" >
                                                                 <asp:Image  runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg"  />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="60px" />
                                                    </asp:TemplateField>

                                                </Columns>

                                                <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                                                <EditRowStyle BackColor="#ffffcc" />
                                                <EmptyDataRowStyle ForeColor="Red" CssClass="table table-bordered" />
                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>


                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                </td>
            </tr>

            <tr>
               <%-- <td colspan="5">

                    <asp:Panel ID="pnlImpresion" runat="server">
                        <table style="width: 100%; vertical-align: top;">
                            <tr>
                                <td align="left" style="vertical-align: top" colspan="2">
                                    <div class="panel-footer">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td align="right">
                                                    <img alt="" src="../App_Themes/default/images/excelPeq.gif" />
                                                    <asp:LinkButton ID="lnkExcel" runat="server" CssClass="myLittleLink" OnClick="lnkExcel_Click" ValidationGroup="0">Exportar a Excel</asp:LinkButton>
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>

                </td>--%>

            </tr>
        </table>
     
    </div>

</asp:Content>
