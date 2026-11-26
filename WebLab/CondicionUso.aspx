<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CondicionUso.aspx.cs" Inherits="WebLab.CondicionUso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

 
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">
    <script type="text/javascript">

        document.getElementById('<%= cb_aceptaTerminos.ClientID %>').checked = false;

        function habilitarBotonAceptar() {
            check = document.getElementById('<%= cb_aceptaTerminos.ClientID %>');
            btnAceptar = document.getElementById('<%= btn_aceptarTerminosCondiciones.ClientID %>');
            if (check.checked) {
                btnAceptar.disabled  = false;
            } else {
                btnAceptar.disabled = true;
            }
       
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
              
            <div class="container">
                <div class="row">
                    <div class="col-md-8 col-md-offset-2" style=" margin-top: 20px;" >
                        <h4>Condiciones de Uso - Sistema Informático de Laboratorios</h4>
                       <asp:Literal ID="litCondicionesUso" runat="server" Mode="PassThrough" />
                                        <asp:CheckBox runat="server" ID="cb_aceptaTerminos" Text="Acepto las condiciones de uso del Sistema Informático de Laboratorios" onchange="habilitarBotonAceptar();" />
                   
                    </div>
                </div>
            </div>
              
       
          <div align="center" >
              <asp:Button runat="server" Enabled="false" ID="btn_aceptarTerminosCondiciones" CssClass="btn btn-success" Text="Aceptar"
                  OnClick="btn_aceptarTerminosCondiciones_Click"  UseSubmitBehavior="true"  CausesValidation="false"  Width="120px" />
          </div>
        </div>
    </form>
</body>
</html>
