<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeLote.aspx.cs" Inherits="WebLab.Derivaciones.InformeLote"  %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
      
    <link type="text/css" rel="stylesheet" href="../App_Themes/default/style.css" />
    <link type="text/css" rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />
    <link rel="stylesheet" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
    <script type="text/javascript" src="../script/jquery.min.js"></script>
    <script type="text/javascript" src="../script/jquery-ui.min.js"></script>
    <style type="text/css">
        .tabla-compacta {
            border-collapse: separate;
            border-spacing: 2px 0;
        }
        .tabla-compacta td {
            padding: 2px 4px;
        }
    </style>
</head>
 
<body style="background-color: #ffffff;">
    <form id="form1" runat="server">      
    <div align="left" style="width:700px">
        <div class="panel panel-default">
            <div class="panel-heading">
                  <b > <asp:Label ID="lblTitulo" runat="server" Text="" /></b> 
                <br />
                 <asp:Label ID="lblSubtitulo" runat="server" Text="" ></asp:Label>
                <asp:HiddenField ID="hfEfectorDestino" runat="server" />
            </div>

			<div class="panel-body">
				 <table>
					<tr>
					<td style="vertical-align: auto" colspan="3">
                         <asp:Panel id="Panel1"   runat="server">
                             <table class="tabla-compacta" >
                                 <tr style="vertical-align: sub">
                                     <td >Marcar como: </td>
                                     <td ><asp:DropDownList ID="ddlEstados" runat="server"  class="form-control input-sm" OnSelectedIndexChanged="ddlEstados_SelectedIndexChanged" AutoPostBack="true" /> 
                                         <asp:RangeValidator id="Range1"
                                               ControlToValidate="ddlEstados"
                                               MinimumValue="2"
                                               MaximumValue="3"
                                               Type="Integer"
                                               Text="* Seleccione un estado"
                                               runat="server" SetFocusOnError="True" ValidationGroup="0" />
                                     </td>
                                    
                                     <td >Retira transporte:</td>
                                     <td > <asp:DropDownList ID="ddlTransporte" runat="server" class="form-control input-sm" ></asp:DropDownList>    
                                     </td>
                                    
                                    
                                 </tr>
                                 <tr style="vertical-align: sub">
                                      <td>Fecha y Hora de retiro:</td>
                                    
                                      <td >
                                            <asp:TextBox id="txtFecha" runat="server" class="form-control input-sm"   TextMode="Date" ></asp:TextBox>
                                            
                                            <asp:RequiredFieldValidator ID="rfvFecha" runat="server" ControlToValidate="txtFecha" ErrorMessage="Fecha" ValidationGroup="0">*Error en Fecha</asp:RequiredFieldValidator>
                                    </td>

                                     <td>
                                         <asp:TextBox id="txtHora"  runat="server"  class="form-control input-sm"  TextMode="Time"> </asp:TextBox>
                                         
                                         <asp:RequiredFieldValidator ID="rfvHora" runat="server" ControlToValidate="txtHora" ErrorMessage="Hora" ValidationGroup="0">*Error en Hora</asp:RequiredFieldValidator>
                                        
                                     </td>
                                 </tr>
                                 <tr style="vertical-align: sub">
                                      <td>Observaciones:</td>
                                      <td colspan="3"><asp:TextBox ID="txtObservacion" runat="server" MaxLength="3000" class="form-control input-sm" width="500px" ></asp:TextBox>
                                            
                                     </td>

                                 </tr>
                                 <tr><td colspan="3">
                                         <asp:CustomValidator ID="cvGeneral" runat="server" OnServerValidate="cvGeneral_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                     </td> </tr>
                                 <tr>
                                       <td align="left">
                                         <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary"  Width="100" Text="Guardar"  onclick="btnGuardar_Click" ValidationGroup="0" />
                                     </td>
                                     <td>
                                         <asp:Label ID="lblMensaje" runat="server" Text="" ></asp:Label>
                                     </td>

                                 </tr>
                             </table>
                           </asp:Panel>

                    </td>
						
				</tr>
			  </table>
            </div>
        </div>
 </div>
   </form>
</body>
</html>