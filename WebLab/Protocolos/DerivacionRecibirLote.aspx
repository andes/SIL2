<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DerivacionRecibirLote.aspx.cs" Inherits="WebLab.Protocolos.DerivacionRecibirLote" MasterPageFile="../Site1.Master" %>

<%@ Register Src="ProtocoloList.ascx" TagName="ProtocoloList" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>
    

</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div align="left" style="width: 800px;" class="form-inline">
        <table width="800px" align="center">
            <tr>
                <td rowspan="8">
                    <div id="pnlTitulo" runat="server" class="panel panel-default">
                        <div class="panel-heading">
                            <asp:Label Text="Ultimos 10 Protocolos" runat="server" ID="lblTituloLista"></asp:Label>
                        </div>

                        <div class="panel-body" style="align-content: center;">
                            <uc1:ProtocoloList ID="ProtocoloList1" runat="server" />
                        </div>
                    </div>

                </td>
                <td>&nbsp;</td>
                <td rowspan="6" style="vertical-align: top">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            Recepción de Lotes
                        </div>
                        <div class="panel-body">
                            <table width="500px">
                                <tr>
                                    <td>
                                        <div>
                                            <label for="txtNumeroLote">Nro. Lote:</label>
                                            <strong>
                                                <asp:Label ID="txtNumeroLote" runat="server" Width="100px"></asp:Label></strong>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="lbl_FechaRegistro">Fecha Registro:</label>
                                        <strong>
                                            <asp:Label ID="lbl_FechaRegistro" runat="server" Width="100px"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="lb_efector">Efector Origen:</label>
                                        <strong>
                                            <asp:Label ID="lb_efector" runat="server" Width="300px"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td class="">Fecha y Hora:
                                        <input id="txt_Fecha" runat="server" type="date" class="form-control input-sm" title="Ingrese la fecha de ingreso" />
                                        <input id="txt_Hora"  runat="server" type="time" class="form-control input-sm" name="txt_Hora" />
                                        <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ControlToValidate="txt_Fecha" ErrorMessage="Fecha" ValidationGroup="0">*Error en Fecha</asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfv_Hora" runat="server" ControlToValidate="txt_Hora" ErrorMessage="Hora" ValidationGroup="0">*Error en Hora</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>

                                <tr>
                                    <td>Transportista:
                                        <asp:TextBox ID="txt_transportista" runat="server" class="form-control input-sm" Style="width: 600px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Observaciones:
                                        <textarea id="txt_obs" runat="server" rows="3" cols="2" class="form-control input-sm" style="width: 600px"></textarea>
                                    </td>
                                </tr>

                            </table>
                        </div>

                        <div class="panel-footer">
                            <asp:Button ID="btn_recibirLote" runat="server" OnClick="btn_recibirLote_Click" Text="Guardar" CssClass="btn btn-success" Width="150px" Enabled="true" />

                            <asp:Button ID="btn_volver" runat="server" OnClick="btn_volver_Click" Text="Volver" CssClass="btn btn-primary" Width="150px" Enabled="true" />
                        </div>
                    </div>
                </td>

            </tr>
        </table>
    </div>

</asp:Content>
