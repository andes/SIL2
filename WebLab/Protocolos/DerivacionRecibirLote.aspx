<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DerivacionRecibirLote.aspx.cs" Inherits="WebLab.Protocolos.DerivacionRecibirLote" MasterPageFile="../Site1.Master" %>

<%@ Register Src="ProtocoloList.ascx" TagName="ProtocoloList" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>

    <script type="text/javascript">
            /** Con esta funcion verificamos que si coloca la Fecha de Hoy, no pueda poner una Hora superior  a la actual */

        document.addEventListener("DOMContentLoaded", function () {
            var txtFecha = document.getElementById('<%= txtFecha.ClientID %>');
            var txtHora = document.getElementById('<%= txtHora.ClientID %>');
            var txtEnvio = document.getElementById('<%= hidFechaEnvio.ClientID %>');

            function cambioHorario() {
            var fechaSeleccionada = new Date(txtFecha.value);
            var fechaHoy = new Date();

            // Normalizamos para comparar solo YYYY-MM-DD sin la hora
            var fechaSeleccionadaStr = fechaSeleccionada.toISOString().split('T')[0];  //toISOString() convierte la fecha en YYYY-MM-DDTHH:mm:ss.sssZ
            var fechaHoyStr = fechaHoy.toISOString().split('T')[0];
            //console.log(txtEnvio.value)
            var FechaENvioLote = new Date(txtEnvio.value);
            //console.log(FechaENvioLote)
            txtFecha.max = fechaHoyStr; // Limitar la fecha máxima al día de hoy
            txtFecha.min = FechaENvioLote.toISOString().split('T')[0];

            if (fechaSeleccionadaStr === fechaHoyStr) {
                var horaActual = fechaHoy.getHours().toString().padStart(2, '0') + ":" + fechaHoy.getMinutes().toString().padStart(2, '0');
                if (txtHora.max !== horaActual) { // Evita sobrescribir si ya está correcto
                    txtHora.max = horaActual; //Si elige la fecha de hoy no puede poner una hora superior a la actual
                }
            } else {
                if (txtHora.hasAttribute("max")) { // Solo eliminar si existe
                    txtHora.removeAttribute("max");
                }
             }
        }

        cambioHorario(); //Llamar a la función al cargar la página
        txtFecha.addEventListener("change", cambioHorario); //llamar la funcion para cambiar la hora
        });
    </script>

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
                            Recepción del Lotes
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
                                        <label for="lb_efector">Efector Origen:</label>
                                        <strong>
                                            <asp:Label ID="lbEfector" runat="server" Width="300px"></asp:Label></strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <strong> <asp:Label runat="server" ID="lblFechaPermitida"> </asp:Label></strong>
                                        <asp:HiddenField ID="hidFechaEnvio"  runat="server"  />
                                    </td>
                                </tr>
                               
                                
                                 <tr>
                                    <td>
                                        <strong>Transportista:
                                        <asp:Label ID="lblTransportista" runat="server"  ></asp:Label> </strong>
                                        <%--<asp:TextBox ID="txt_transportista" runat="server" class="form-control input-sm" Style="width: 600px"></asp:TextBox>--%></td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    
                                    <td class="">Fecha y Hora:
                                        <%--<input id="txt_Fecha" runat="server" type="date" class="form-control input-sm" title="Ingrese la fecha de ingreso" />--%>
                                        <asp:TextBox runat="server" ID="txtFecha" TextMode="Date" class="form-control input-sm"  />
                                        <input id="txtHora"  runat="server" type="time" class="form-control input-sm" name="txtHora" />

                                        <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ControlToValidate="txtFecha" ErrorMessage="Fecha" ValidationGroup="0">*Error en Fecha</asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvHora" runat="server" ControlToValidate="txtHora" ErrorMessage="Hora" ValidationGroup="0">*Error en Hora</asp:RequiredFieldValidator>
                                        <asp:RangeValidator runat="server" ID="rvFecha" ControlToValidate="txtFecha" />
                                        </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>

                               
                                <tr>
                                    <td>Observaciones:
                                        <textarea id="txtObs" runat="server" rows="3" cols="2" class="form-control input-sm" style="width: 600px"></textarea>
                                    </td>
                                </tr>

                            </table>
                        </div>

                        <div class="panel-footer">
                            <asp:Button ID="btnRecibirLote" runat="server" OnClick="btn_recibirLote_Click" Text="Guardar" CssClass="btn btn-success" Width="150px" Enabled="true" />

                            <asp:Button ID="btnVolver" runat="server" OnClick="btn_volver_Click" Text="Volver" CssClass="btn btn-primary" Width="150px" Enabled="true" />
                        </div>
                    </div>
                </td>

            </tr>
        </table>
    </div>

</asp:Content>
