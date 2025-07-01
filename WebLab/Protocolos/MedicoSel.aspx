<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MedicoSel.aspx.cs" Inherits="WebLab.Protocolos.MedicoSel" %>
<%@ Register Src="~/Services/ObrasSociales.ascx" TagName="OSociales" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="shortcut icon" href="App_Themes/default/images/favicon.ico"/>
    <link rel="stylesheet" href="App_Themes/default/bootstrap.min.css" />	
	<link rel="stylesheet" type="text/css" href="App_Themes/default/style.css" />
      <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
<script src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script src='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="form-inline" >
        <table>
            <tr>
                <td>  Apellido:</td>
                <td> <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm" Width="280px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>  Nombre:</td>
                <td>
        <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="280px"></asp:TextBox></td>
                </tr>
        </table>
      
        
       
        <br />
        <asp:Button ID="btnBuscar" CssClass="btn btn-primary" Width="80px" runat="server" OnClick="btnBuscar_Click" Text="Buscar" />
        <asp:GridView ID="gvMedico" runat="server" AutoGenerateColumns="False" DataKeyNames="matriculaNumero" OnRowCommand="gvMedico_RowCommand" OnRowDataBound="gvMedico_RowDataBound" CssClass="table table-bordered bs-table"
            EmptyDataText="La busqueda no arrojo resultados. Verifique nombre y apellido.">
            <Columns>
                <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre"/>
                <asp:BoundField DataField="matriculaNumero" HeaderText="Matricula" />
                <asp:TemplateField HeaderText="Seleccionar">
                    <ItemTemplate>
                        <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return Close();">
                            <span class="glyphicon glyphicon-ok"></span></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
    
<script language="javascript" type="text/javascript">
    function Close() {
       window.close();
    }
    </script>
</body>
</html>
