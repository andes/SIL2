<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="select2.aspx.cs" Inherits="WebLab.select2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <!-- Select2 CSS -->
   <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" rel="stylesheet" />

    <!-- Incluir el theme Bootstrap 3
            <link href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" rel="stylesheet" />
            <link href="https://cdn.jsdelivr.net/npm/select2-bootstrap-theme@0.1.0-beta.10/dist/select2-bootstrap.min.css" rel="stylesheet" />
        -->
    <!-- Select2 JS -->
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>



    <!-- Mensajes en español -->
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/i18n/es.js"></script>
   
   


    <script>
        $(document).ready(function () {
            $('.select2').select2({
                placeholder: "Seleccione uno o más efectores",
                language: "es",
                allowClear: true,
                theme: "classic",
                width: '100%'
              
            });
        });
</script>
    <title>select2</title>
</head>
<body>
   
    <form id="form1" runat="server">
        <div>
            <h1>Sin uso de libreria</h1>
               DropDownList de SIL: <br />
            <asp:DropDownList 
                ID="ddlEfector3"  runat="server"  CssClass="form-control input-sm " ClientIDMode="Static" >
            </asp:DropDownList>
            <br />
            
            <h1> Usando Libreria Select2</h1>
            
            DropDownList de seleccion simple: 
            <asp:DropDownList 
                ID="ddlEfector"  runat="server"  CssClass="form-control input-sm select2 input-sm" ClientIDMode="Static" >
            </asp:DropDownList>
           
             DropDownList de seleccion multiple: 
            <asp:ListBox 
                ID="ddlEfector2"
                runat="server"
                CssClass="form-control select2 input-sm"
                SelectionMode="Multiple"
                ClientIDMode="Static">
            </asp:ListBox>
            
        </div>
    </form>
</body>
</html>
