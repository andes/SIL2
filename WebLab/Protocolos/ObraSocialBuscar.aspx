<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObraSocialBuscar.aspx.cs" Inherits="WebLab.Protocolos.ObraSocialBuscar" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <!-- CSS -->
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../script/chosen/chosen.css" />
      <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />

    <!-- JS -->
    <script src="../script//jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="../script/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../script/chosen/chosen.jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        function inicializarChosen() {

            var select = $('#<%= ddlObrasSociales.ClientID %>');

            if (select.data('chosen')) {
                select.chosen('destroy'); //El destroy evita que Anthem duplique la lista después de un callback.
            }

            //convierte el select en un dropdown Chosen
            select.chosen({
                width: "300px",
                search_contains: true,
                no_results_text: "No se encontraron resultados:"
            });
        }

        $(document).ready(function () {
            inicializarChosen();
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="form-group" >
        <br />
        <table>
            <tr>
                <td style="vertical-align: top" class="myLabelIzquierda">
                     Financiador / O.S:  <asp:DropDownList ID="ddlObrasSociales" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top" align="right">
                      <asp:Button ID="btnSeleccionar" CssClass="btn btn-primary" Width="100px" runat="server" OnClick="btnSeleccionar_Click" Text="Seleccionar" />
                </td>
            </tr>
        </table>
      
      
       
    </div>
    </form>
    

</body>
</html>
