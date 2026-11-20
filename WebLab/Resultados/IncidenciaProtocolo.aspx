<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncidenciaProtocolo.aspx.cs" Inherits="WebLab.Resultados.IncidenciaProtocolo" %>

<%@ Register src="../Calidad/IncidenciaEdit.ascx" tagname="IncidenciaEdit" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" />
  
</head>

<body>
    <form id="form1" runat="server">
    
            
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
 
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                        </h3>
                       
                        </div>
        	<div class="panel-body">
                                              
        
                                     <table>
                             <tr>
                                                           <td>    
                                                                 <div  style="width:600px;height:200pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"" > 
                                    <uc1:IncidenciaEdit ID="IncidenciaEdit1" runat="server" />
                                                                     </div>
                             <asp:Button ID="btnGuardarIncidencia" runat="server" Text="Guardar" onclick="btnGuardarIncidencia_Click" CssClass="btn-primary" Width="100px" />
                             <asp:Button ID="btnEliminarIncidencia" Visible="false" runat="server" Text="Borrar" onclick="btnEliminarIncidencia_Click" CssClass="btn-primary" Width="100px"/>
                                    </td>
                                 </tr>
                                         </table>
                                </div>  
     </div>
    </form>
</body>
</html>
