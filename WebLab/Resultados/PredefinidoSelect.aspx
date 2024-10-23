<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PredefinidoSelect.aspx.cs" Inherits="WebLab.Resultados.PredefinidoSelect"  %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">       
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />        
</head>
<body >  
    <form id="form1" runat="server"  >           
  
             
           <table>           
                <tr>
                <td style="vertical-align: top" colspan="2">                                               
                              <asp:Label CssClass="mytituloGris" ID="lblObservacionAnalisis" runat="server" Text="Label"></asp:Label>
                           <br />   <asp:Label CssClass="myLabelIzquierda" ID="lblProtocolo" runat="server" Text="Label"></asp:Label>
                              
                              <hr />
                              </td>
                              </tr> 
                              <tr>
                <td style="vertical-align: top"  colspan="2" class="myLabelIzquierda">                                               
                
                         
                    
                           
                           <%--    <anthem:Button OnClick="btnBorrarObservacionesAnalisis_Click" 
                                  CssClass="myButtonGrisLittle" ToolTip="Borra el texto ingresado" ID="btnBorrarObservacionAnalisis"
                                   runat="server" Text="Limpiar" Width="45px" />
                           --%>         
					
                           
                               <anthem:TextBox  ID="txtObservacionAnalisis" runat="server" 
                                   TextMode="MultiLine" MaxLength="500" Rows="3" Width="450px" CssClass="myTexto"></anthem:TextBox>
                                    
					
                               
                         <hr />       
                    </td>
                </tr>
                
              
               
               
                <tr>
                <td style="vertical-align: top"  colspan="2" class="myLabelIzquierda">                                               
                    <div  style="width:450px;height:105pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                               <anthem:CheckBoxList ID="chk1" runat="server" AutoCallBack="True" 
                                   onselectedindexchanged="chk1_SelectedIndexChanged" RepeatColumns="2">
                               </anthem:CheckBoxList></div>
					
					
                    </td>
                </tr>
                
              
               <hr />
               
                <tr>
                <td style="vertical-align: top" align="left">
                    
                           &nbsp;</td>
                <td style="vertical-align: top" align="right">                    
                           <asp:Button onclick="btnGuardarObservacionesAnalisis_Click" ToolTip="Guarda el resultado ingresado"  CssClass="myButton" ID="btnGuardarObservacionAnalisis" runat="server" Text="Guardar" />&nbsp;&nbsp;&nbsp;
                               <%--<asp:Button onclick="btnValidarObservacionesAnalisis_Click" ToolTip="Guarda y Valida el resultado ingresado"  CssClass="myButton" ID="btnValidarObservacionAnalisis" runat="server" Text="Validar" />                                    --%>
                </td>
                </tr>
                              
               
               
                <%--<tr>
                <td style="vertical-align: top" align="left" colspan="2">
                    
                           <asp:Label CssClass="myLabelIzquierda" 
                        Visible="False" ID="lblUsuarioObservacionAnalisis" runat="server"></asp:Label>
                                    
					</td>
                </tr>--%>
                
              
               
               
           </table>
      
         
 </form>
</body>
  
</html>
