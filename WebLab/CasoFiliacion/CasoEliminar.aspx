<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoEliminar.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoEliminar"  %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
  
     
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />  
     <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    

 
</head>
<body  style="height:500px">  
    <form id="form1" runat="server"  >           
  
             <div align="left" style="width: 85%" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">  <asp:Label ID="lblTitulo" runat="server" Text="Label" Font-Size="16pt"></asp:Label>       </h3>
                        </div>
          				<div class="panel-body">	
            
               
              
                        <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
               
                   <br />

                                Motivo de anulación:<asp:TextBox ID="txtMotivoBaja" class="form-control input-sm" runat="server" MaxLength="500" Width="350px" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                <br />
                            <asp:RequiredFieldValidator ID="rfvtxtMotivoBaja" runat="server" ErrorMessage="Debe ingresar un motivo" ControlToValidate="txtMotivoBaja" ValidationGroup="0"></asp:RequiredFieldValidator>
                                               
                            <br />
                                <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Anular" ValidationGroup="0" Width="100px" />
                           
                </div>
          </div>
                 </div>
         
 </form>
</body>
  
</html>
