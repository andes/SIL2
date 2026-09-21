<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DerivacionAnular.aspx.cs" Inherits="WebLab.Derivaciones.DerivacionAnular" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
      


  <link type="text/css"rel="stylesheet" href="../App_Themes/default/style.css" />  
  <link type="text/css"rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />  
<link rel="stylesheet" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />	
  <script type="text/javascript" src="../script/jquery.min.js"></script> 
  <script type="text/javascript" src="../script/jquery-ui.min.js"></script> 


</head>

<body style="background-color: #ffffff;">
    <form id="form1" runat="server">

        <div align="left" style="width: 85%" class="form-inline">
            <div class="panel panel-default">
               
                <div class="panel-body">
                    Motivo Cancelaci&oacute;n: 
                    <br />
                    <asp:DropDownList ID="ddlMotivoCancelacion" runat="server" class="form-control input-sm" />
                   
                    <asp:RangeValidator  ControlToValidate="ddlMotivoCancelacion"
                        MinimumValue="1"
                        MaximumValue="99"
                        Type="Integer"
                        Text="Debe seleccione un motivo de cancelacion"
                        runat="server" SetFocusOnError="True" 
                        ValidationGroup="0" />
                    <br />

                    Observaci&oacute;n:
                              
                    <br />
                    <asp:TextBox ID="txtObservacion" class="form-control input-sm" runat="server" MaxLength="500" Width="350px" Rows="3" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="rfvtxtMotivoBaja" runat="server" ErrorMessage="Debe ingresar una observacion" ControlToValidate="txtObservacion" ValidationGroup="0"></asp:RequiredFieldValidator>

                    <br />
                    <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Anular" ValidationGroup="0" Width="100px" CssClass="btn btn-danger" />
                    <asp:Label ID="lblMensaje" runat="server" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>
