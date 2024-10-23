<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="comparaString.aspx.cs" Inherits="WebLab.comparaString" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="HFIdPaciente" /> 
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="list" onclick="Button1_Click2" 
     
    />
    <%--OnClientClick="editPeticion(); return false;"
       onclick="Button1_Click1"  
    --%>
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
    <asp:Button ID="Button2" runat="server" OnClientClick="editPeticion(); return false;" Text="edit" />

  <%--   onclick="Button2_Click" --%>
    <div>
    
    </div>

<script src="script/Resources/jquery.min.js" type="text/javascript"></script>
<link href="script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
<script src="script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    
    var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();

    function editPeticion() {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }


        var $this = $(this);
        $('<iframe src="PeticionElectronica/PeticionEdit.aspx?idPaciente=' + idPaciente + '" />').dialog({
            title: 'Petición Electrónica Laboratorio',
            autoOpen: true,
            width: 1024,
            height: 768,
            modal: false,
            resizable: false,
            autoResize: false,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(1024);
    }


    </script>
    </form>
</body>
</html>
