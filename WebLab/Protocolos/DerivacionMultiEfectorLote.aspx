<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DerivacionMultiEfectorLote.aspx.cs" Inherits="WebLab.Protocolos.DerivacionMultiEfectorLote" MasterPageFile="../Site1.Master" %>

<%@ Register Src="ProtocoloList.ascx" TagName="ProtocoloList" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validaInput() {
            var txtNumeroLote = document.getElementById('<%= txtNumeroLote.ClientID %>');
            //console.log(txtNumeroLote);
            var num = txtNumeroLote.value;
            //console.log(num);
            num = num.replace(/\D/g, '');
            //console.log(num);
            $("#<%=txtNumeroLote.ClientID%>").val(num);
        }

    </script>

</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div align="left" style="width: 800px;" class="form-inline">
        <table width="800px" align="center">
            <tr>
                <td rowspan="6">
                    <div id="pnlTitulo" runat="server" class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label Text="Ultimos 10 Protocolos" runat="server" ID="lblTituloLista"></asp:Label>
                        </div>

                        <div class="panel-body" style="align-content: center;">
                            <uc1:ProtocoloList ID="ProtocoloList1" runat="server" />
                        </div>
                    </div>

                </td>
                <td rowspan="6">&nbsp;</td>
                <td rowspan="6" style="vertical-align: top">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Recepción de Derivación por Lotes
                        </div>
                        <div class="panel-body">
                            <table width="700px">

                                <tr>
                                    <td>
                                        <div class="form-group" id="div_controlLote" runat="server">
                                            <label class="control-label" for="txtNumeroLote">Nro. Lote:</label>
                                            <asp:TextBox ID="txtNumeroLote" runat="server" class="form-control input-sm" Width="100px" OnTextChanged="txtNumeroLote_TextChanged" Onkeyup="validaInput();" AutoPostBack="true"></asp:TextBox>

                                        </div>


                                    </td>
                                    <td>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txtNumeroLote" ErrorMessage="Nro. Lote"
                                            ValidationGroup="0">*</asp:RequiredFieldValidator>
                                        <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="btnBuscar_Click" ValidationGroup="0" Width="90px">
                                             <span class="glyphicon glyphicon-search"></span>&nbsp;Buscar</asp:LinkButton>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <div class="form-group has-error" id="div_controlLote2" runat="server">
                                            <asp:Label ID="lbl_errorEfectorOrigen" runat="server" Visible="false" CssClass="help-block"></asp:Label>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td>Efector origen:
                                        <strong> <asp:Label runat="server" ID="lbl_efectorOrigen" Text=""></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Estado Lote: <strong>
                                        <asp:Label ID="lbl_estadoLote" runat="server" Text=""></asp:Label></strong> </td>

                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gvProtocolosDerivados" runat="server" CssClass="table table-bordered bs-table" AutoGenerateColumns="False"
                                            DataKeyNames="idProtocolo">
                                            <Columns>
                                                <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                                <asp:BoundField DataField="numero" HeaderText="Numero Protocolo" />
                                                <%--<asp:BoundField DataField="EstadoDerivacion" HeaderText="Estado Derivacion" />--%>
                                                <asp:BoundField DataField="idPaciente" Visible="false" />
                                                <asp:BoundField DataField="paciente" HeaderText="Paciente" />
                                                <asp:BoundField DataField="idProtocolo" Visible="false" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" ID="lnkIngresoProtocolo" OnCommand="lnkIngresoProtocolo_Command"
                                                            CommandArgument='<%# Eval("idProtocolo") %>'
                                                            CommandName='<%# Eval("idPaciente") %>'
                                                            enabled=' <%# HabilitarIngreso() %>'
                                                            Text="Ingresar Protocolo" CssClass="btn btn-success" Width="150px"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
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
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_cantidadRegistros" runat="server"></asp:Label></td>

                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td class="style7">

                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server"
                                            HeaderText="De completar los datos requeridos:" ShowMessageBox="True"
                                            ValidationGroup="0" />

                                    </td>
                                </tr>


                            </table>

                        </div>
                        <div class="panel-footer">
                            <asp:LinkButton ID="btn_recibirLote" runat="server"
                                OnClick="btn_recibirLote_Click" Text="Recibir Lote"
                                CssClass="btn btn-primary" Width="150px"
                                Enabled="false" />
                        </div>
                    </div>
                </td>

            </tr>
        </table>
    </div>

</asp:Content>
