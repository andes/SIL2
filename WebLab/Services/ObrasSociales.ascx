<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObrasSociales.ascx.cs"
    Inherits="WebLab.Services.ObrasSociales" %>
<style type="text/css">
    .auto-style1 {
        height: 62px;
    }
</style>
<script lang="ja"  type="text/javascript" >
    $(function () {

        $('#<%= txtAutoOs.ClientID %>').autocomplete({ source: '<%= ResolveUrl("~/Services/ObraSocial.aspx") %>', minlenght: 3,
            focus: function (event, ui) {
                $("#<%= txtAutoOs.ClientID %>").val(ui.item.nombre);
                return false;
            },
            select: function (event, ui) {
                if (ui.item.id != -1) {
                    $("#<%= txtAutoOs.ClientID %>").val(ui.item.nombre);
                    $('#<%= lblNombre.ClientID %>').text(ui.item.nombre);
                    $('#<%= lblSigla.ClientID %>').text("(" + ui.item.sigla + ")");
                    <%--$('#<%= lblCodigoNacion.ClientID %>').text(ui.item.codigoNacion);--%>
                    $("#<%= idOS.ClientID %>").val(ui.item.id);
                    $("#<%= codigoOS.ClientID %>").val(ui.item.id);
                    ActivarRequerido(false);
                }
                else {
                    ActivarRequerido(true);
                    return false;
                }
            }
        }).data("autocomplete")._renderItem = function (ul, item) {
            return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a><strong>" + item.nombre + "</strong> (" + item.sigla + ") </a>")
				.appendTo(ul);
        };

        $('#hlLimpiar').button();
    });

    function LimpiarPrincipal() {
        $("#<%= txtAutoOs.ClientID %>").val('');
        $('#<%= lblNombre.ClientID %>').text('Ingrese nombre, código o sigla de la Obra Social.');
        $('#<%= lblSigla.ClientID %>').text('');
        
        $("#<%= idOS.ClientID %>").val('');
        $("#<%= codigoOS.ClientID %>").val('-1');

        ActivarRequerido(true);
    }
</script>
<script type="text/javascript">

    function ActivarRequerido(Activa) {
        var boolRequerido = document.getElementById('<%=boolRequerido.ClientID%>');
        if (boolRequerido.value == "true") {

            mensaje = document.getElementById('<%=lblMensajeError.ClientID%>');
            mensaje.style.visibility = (Activa == true ? "visible" : "collapse");
        }

    }

    function validar(sender, args) {
        args.IsValid = false;
        var boolRequerido = document.getElementById('<%=boolRequerido.ClientID%>');
        var id = (isNaN(document.getElementById('<%=codigoOS.ClientID%>').value) ? -1 : document.getElementById('<%=codigoOS.ClientID%>').value);
        if (id == "") id = -1;

        if ((boolRequerido.value == "true") & (id >= 0)) {
            args.IsValid = true;
        }
    }

</script>
<div align="left" class="form-inline"  >
<table class="autoCompleter">
    <tr>    
        <td valign="top" class="auto-style1">
            <asp:TextBox runat="server" ID="txtAutoOs" class="form-control input-sm" Columns="60" ToolTip="Ingresar la Obra Social"></asp:TextBox>
            <br />
                <asp:Label runat="server" ID="lblNombre" Text="Ingrese nombre, código o sigla de la Obra Social."></asp:Label>
                <asp:Label runat="server" ID="lblSigla" Text=""></asp:Label>
               <br />
                <asp:Label ID="lblMensajeError" runat="server" Text="Debe seleccionar una Obra Social"
                    Style="color: #FF0000; font-size: 0.94em; visibility: collapse;"></asp:Label>
        </td>
        <td valign="top" class="auto-style1">
            <a href="javascript:LimpiarPrincipal();" id="hlLimpiar" style="float: left; vertical-align: top;" title="Eliminar Item actual">
                <asp:Image ToolTip="Borrar valor" runat="server" ID="imgLimpiar" ImageAlign="AbsMiddle"
                    ImageUrl="~/App_Themes/default/images/eliminar.jpg" AlternateText="Borrar valor" /></a>
        </td>
        <td class="auto-style1">
            <asp:CustomValidator ID="cvValidar" runat="server" ErrorMessage="Obra Social" Text="*" Display="Dynamic"
                ClientValidationFunction="validar"></asp:CustomValidator>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div id="acResult">                
                <asp:HiddenField ID="idOS" runat="server" />
                <asp:HiddenField ID="boolRequerido" runat="server" />
                <asp:HiddenField ID="codigoOS" runat="server"/>            
            </div>
            
        </td>
        <td>
        </td>
    </tr>
</table>
</div>