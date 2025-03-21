<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObservacionesEdit.aspx.cs" Inherits="WebLab.Resultados.ObservacionesEdit"  %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">    
    
 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="shortcut icon" href="website/website/images/icolabo.ico">
 <%-- <link rel="stylesheet" href="website/style.css">--%>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css">  
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />        
</head>
<body >  
    <form id="form1" runat="server"  >           
  
             
           <table>           
                <tr>
                <td style="vertical-align: top" colspan="2">                                               
                              <asp:Label CssClass="mytituloGris" ID="lblObservacionAnalisis" runat="server" Text="Label"></asp:Label>
                                                            </td>
                              </tr> 
                              <tr>
                <td style="vertical-align: top"  colspan="2" class="myLabelIzquierda">                                               
                
                               Predefinidas:&nbsp;<asp:DropDownList CssClass="form-control input-sm" ID="ddlObservacionesCodificadas" runat="server" Width="250px">
                               </asp:DropDownList>
                               <anthem:Button  OnClick="btnAgregarObsCodificada_Click" 
                                  ToolTip="Agrega la observacion predefinida" CssClass="btn btn-default" 
                                  ID="btnAgregarObsCodificada" runat="server" Text="Agregar" Width="70px" />
                         &nbsp;
                    
                           
                               <anthem:Button OnClick="btnBorrarObservacionesAnalisis_Click" 
                                  CssClass="btn btn-default" ToolTip="Borra el texto de observaciones" ID="btnBorrarObservacionAnalisis"
                                   runat="server" Text="Limpiar" Width="70px" />
                                    
                    </td>
                </tr>
                
              
               
               
                <tr>
                <td style="vertical-align: top"  colspan="2">
                    
                           
                               <anthem:TextBox  ID="txtObservacionAnalisis" runat="server" TextMode="MultiLine" 
                                   MaxLength="1000" Rows="7" Width="450px" CssClass="form-control input-sm"></anthem:TextBox>
                                    
					
                               
                                    <br />
					
                    </td>
                </tr>
                
               
                <tr>
                <td style="vertical-align: top" align="left">
                    
                           <asp:Button 
                        onclick="btnBorrarGuardarObservacionAnalisis_Click" 
                        ToolTip="Borra y Guarda la observacion registrada"  CssClass="btn btn-warning" Width="80px"
                        ID="btnBorrarGuardarObservacionAnalisis" runat="server" Text="Borrar" />
                                    
					</td>
                <td style="vertical-align: top" align="right">
                    
                           <asp:Button onclick="btnGuardarObservacionesAnalisis_Click" ToolTip="Guarda la observacion registrada"  CssClass="btn btn-primary" ID="btnGuardarObservacionAnalisis" runat="server" Text="Guardar" Width="100px" />&nbsp;&nbsp;&nbsp;
                               <asp:Button onclick="btnValidarObservacionesAnalisis_Click" ToolTip="Guarda y Valida la observacion registrada"  CssClass="btn btn-primary" ID="btnValidarObservacionAnalisis" runat="server" Text="Validar" Width="100px"/>
                                    
                               </td>
                </tr>
                
              
               
               
                <tr>
                <td style="vertical-align: top" align="left" colspan="2">
                    
                           <asp:Label CssClass="myLabelIzquierda" 
                        Visible="False" ID="lblUsuarioObservacionAnalisis" runat="server"></asp:Label>
                                    
					</td>
                </tr>
                
              
               
               
           </table>
      
         
 </form>
</body>
  
</html>
