<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregaEfectorSIL.aspx.cs" Inherits="WebLab.AgregaEfectorSIL" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
button,html input[type=button],input[type=reset],input[type=submit]{-webkit-appearance:button;cursor:pointer}.btn-info{color:#fff;background-color:#5bc0de;border-color:#46b8da}.btn{display:inline-block;padding:6px 12px;margin-bottom:0;font-size:14px;font-weight:400;line-height:1.42857143;text-align:center;white-space:nowrap;vertical-align:middle;-ms-touch-action:manipulation;touch-action:manipulation;cursor:pointer;-webkit-user-select:none;-moz-user-select:none;-ms-user-select:none;user-select:none;background-image:none;border:1px solid transparent;border-radius:4px}

.btn 
{
	text-align:right;
	width:650px;
	padding-top: 15px;
}	

input{line-height:normal}*{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}*,:after,:before{color:#000!important;text-shadow:none!important;background:0 0!important;-webkit-box-shadow:none!important;box-shadow:none!important}
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
          <h5> Deberia mostrar mas 1700 determinaciones impactadas</h5> 
            <asp:Button ID="btnInicializa" runat="server" Width="250px" CssClass="btn btn-info" Text="Inicia MultiEfector-Unica Vez" onclick="btnInicializa_Click" />

            <br />
            
    
                
        </div>
    <div>     
        <h3>Agregar nuevo efector</h3>          
                <asp:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" >
                            </asp:DropDownList>


            
    
            <asp:Button ID="btnGuardar" runat="server" Width="90px" CssClass="btn btn-info" Text="Agregar" onclick="btnGuardar_Click" />


            
    
                <asp:Label ID="lblMensaje" runat="server"></asp:Label>


            
    </div>
    </form>
</body>
</html>
