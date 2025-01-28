<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Auditoria.aspx.cs" Inherits="WebLab.Informes.Auditoria" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">


    <link type="text/css" rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />

    <script type="text/javascript" src="../script/jquery.min.js"></script>
    <script type="text/javascript" src="../script/jquery-ui.min.js"></script>

    <script type="text/javascript" src="../script/jquery.ui.datepicker-es.js"></script>

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
</asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width: 800px" class="form-inline">

        <div class="panel panel-default">
            <div class="panel-heading">
                <h4>
                    <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h4>
            </div>

            <div class="panel-body">
                <asp:Panel ID="pnlControlProtocolo" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td class="myLabelIzquierda" style="width: 63px">Protocolo:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoServicio" runat="server" class="form-control input-sm" Visible="False">
                                    <asp:ListItem Selected="True" Value="1">LABORATORIO</asp:ListItem>
                                    <asp:ListItem Value="3">MICROBIOLOGIA</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox class="form-control input-sm" ID="txtProtocolo" runat="server" MaxLength="9" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtProtocolo" ErrorMessage="*" ValidationGroup="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 63px">Usuario:</td>
                            <td>
                                <asp:DropDownList ID="ddlUsuario" runat="server" class="form-control input-sm">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 63px">&nbsp;</td>
                            <td>
                                <asp:Button ID="btnControlProtocolo" runat="server" Width="100px" class="btn btn-primary"
                                    OnClick="btnControlProtocolo_Click" Text="Ver Informe" ValidationGroup="0" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel ID="pnlControlAcceso" runat="server">

                    <table style="width: 100%;">
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">Fecha desde:
                            </td>
                            <td>
                                <input id="txtFechaDesde" runat="server" type="text" maxlength="10"
                                    onblur="valFecha(this)"
                                    onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                    style="width: 100px" title="Ingrese la fecha de inicio" />
                                <asp:CustomValidator ID="cvFechas" runat="server"
                                    ErrorMessage="Debe ingresar el rango de fechas"
                                    OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="1">*</asp:CustomValidator>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">Fecha hasta:
                            </td>
                            <td>
                                <input id="txtFechaHasta" runat="server" type="text" maxlength="10"
                                    onblur="valFecha(this)"
                                    onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                    style="width: 100px" title="Ingrese la fecha de fin" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">Usuario:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUsuario2" runat="server" class="form-control input-sm">
                                </asp:DropDownList>
                                <asp:Label ID="lblMensaje" runat="server" ForeColor="#FF3300" Text="Label" Visible="False"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">Acciones:</td>
                            <td>
                                <asp:RadioButtonList ID="rdbTipoAuditoria" runat="server">
                                    <asp:ListItem Selected="True" Value="1">Auditoria de Acceso</asp:ListItem>
                                    <asp:ListItem Value="2">Auditoria de Acciones</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 79px">&nbsp;</td>
                            <td>
                                <br />
                                <asp:Button class="btn btn-primary" ID="btnControlAcceso" runat="server" Width="100px"
                                    OnClick="btnControlAcceso_Click" Text="Ver Informe" ValidationGroup="1" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>

                 <asp:Panel ID="pnControlLote" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td class="myLabelIzquierda" style="width: 63px">Lote:
                            </td>
                            <td>
                                
                                <asp:TextBox class="form-control input-sm" ID="txt_lote" runat="server" MaxLength="9" Width="200px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txt_lote" ErrorMessage="*" ValidationGroup="0"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 63px">Usuario:</td>
                            <td>
                                <asp:DropDownList ID="ddlUsuario3" runat="server" class="form-control input-sm">
                                </asp:DropDownList>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 63px">&nbsp;</td>
                            <td>
                                <asp:Button ID="btn_informeLote" runat="server" Width="100px" class="btn btn-primary"
                                    OnClick="btn_informeLote_Click" Text="Ver Informe" ValidationGroup="0" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>


            </div>
        </div>

    </div>
</asp:Content>
