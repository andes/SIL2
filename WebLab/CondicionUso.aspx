
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CondicionUso.aspx.cs" Inherits="WebLab.CondicionUso" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Condiciones de Uso</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        function habilitarBotonAceptar() {
            check = document.getElementById('<%= cbAceptaTerminos.ClientID %>');
            btnAceptar = document.getElementById('<%= btnAceptarTerminosCondiciones.ClientID %>');
            if (check.checked) {
                btnAceptar.disabled = false;
            } else {
                btnAceptar.disabled = true;
            }

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="container">
            <div class="row">
                <div class="col-md-8 col-md-offset-2" style="margin-top: 20px;">
                    <h4>Condiciones de Uso - Sistema Informático de Laboratorios</h4>

                    <asp:Panel ID="pnlMiAceptacion" runat="server" Visible="false">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                             <h5>  <strong> <asp:Label ID="lblTitulo" runat="server"></asp:Label></strong></h5>
                                <asp:HiddenField ID="HFIdCondicion" runat="server" /> <br />
                                Version: <strong><asp:Label ID="lblVersion" runat="server" ></asp:Label></strong>   <br />
                                Fecha Aceptación:  <strong><asp:Label ID="lblFechaAceptacion" runat="server" ></asp:Label></strong>
                            </div>
                        </div>
                    </asp:Panel>


                    <asp:Literal ID="litCondicionesUso" runat="server" Mode="PassThrough" />

                    <asp:CheckBox runat="server" ID="cbAceptaTerminos"
                        Text="Acepto las condiciones de uso del Sistema Informático de Laboratorios"
                        onchange="habilitarBotonAceptar();" />
                </div>
            </div>
        </div>

        <div align="center">
            <asp:Button runat="server" Enabled="false"
                ID="btnAceptarTerminosCondiciones"
                CssClass="btn btn-success"
                Text="Aceptar"
                OnClick="btn_aceptarTerminosCondiciones_Click"
                UseSubmitBehavior="true"
                CausesValidation="false"
                Width="120px" />
        </div>
        <div>
             <asp:HyperLink ID="hlRegresar" runat="server" CssClass="myLink" Visible="false">Regresar</asp:HyperLink>
        </div>

    </form>
</body>
</html>
