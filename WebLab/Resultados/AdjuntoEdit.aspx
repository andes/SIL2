<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdjuntoEdit.aspx.cs" Inherits="WebLab.Resultados.AdjuntoEdit"  %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">       
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />     
     <link rel="stylesheet" href="App_Themes/default/bootstrap.min.css" />	
	<link rel="stylesheet" type="text/css" href="App_Themes/default/style.css" />
     <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
   <script type='text/javascript' src="<%= Page.ResolveUrl("~/script/jquery.min.js") %>"></script>   
</head>
<body >  
    <form id="form1" runat="server"  >           
  <div align="left" style="width: 700px">
   <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">  <asp:Label  ID="lblObservacionAnalisis" runat="server" Text="Label"></asp:Label></h3>
                        </div>

				<div class="panel-body" runat="server" id="pnlbody">	
             <input id="hdnidDetalleProtocolo" runat="server" type="hidden" />
           <table width="100%">           
               
                              <tr>
                <td style="vertical-align: top"  colspan="2">                                               
                <div class="form-group" id="datos" runat="server">
   
 <asp:FileUpload ID="trepador" runat="server"  class="form-control input-sm" />
                       
                 


    <label for="ejemplo_password_1">Visibilidad:</label>
                     <asp:DropDownList ID="ddlVisibilidad" runat="server"   class="form-control input-sm" Width="200px">
    <asp:ListItem Selected="True" Value="0">No visible (interno)</asp:ListItem>
    <asp:ListItem Value="1">Todos los usuarios</asp:ListItem>
</asp:DropDownList>
     <asp:TextBox ID="txtDescripcion" runat="server" Width="200px" Visible="false"  class="form-control input-sm"></asp:TextBox>
                     <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="subir_Click"    Width="150px" >
                                             <span class="glyphicon glyphicon-arrow-up"></span>&nbsp;Subir Archivo</asp:LinkButton>
                    
                    
                     
            <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>
                        
                   </div>
                    </td>
                </tr>
                
              
        
                
              
               
               
           </table>

      
         </div>
       <div class="panel-footer" >
                    
                  <asp:DataList ID="DTlist" runat="server" OnItemDataBound="DTlist_ItemDataBound" 
          OnItemCommand="DTlist_ItemCommand" >
     <ItemTemplate>
          

         <table cellpadding="2" cellspacing="2">
             <tr>
                 <td> <b><asp:Label ID="Label1" runat="server" Text='<%# Eval("descripcion") %>'></asp:Label></b> </td>
                 <td>&nbsp;</td>
             </tr>
             <tr>
                 <td>
                       <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("Url") %>'
             Width="250px" Height="250px" />
                 </td>
                 </tr>
             <tr>
                 <td>
                     <table>
                         <tr>
                             <td>  <asp:Button ID="Descargar" runat="server" CommandArgument='<%# Eval("idProtocoloAnexo") %>' CommandName="Descargar" CssClass="btn btn-info" Text="Descargar" Width="90px" /></td>
                             <td>&nbsp; &nbsp; </td>
                             <td>  <asp:Label ID="lblVisible" runat="server" Text='<%# Eval("visible") %>'></asp:Label></td>
                               <td>&nbsp; &nbsp; </td>
                             <td>  Subido por <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("auditoria") %>'></asp:Label></td>
                               <td>&nbsp; &nbsp; </td>
                             <td>  <asp:LinkButton ID="Eliminar" runat="server" CommandArgument='<%# Eval("idProtocoloAnexo") %>' CommandName="Eliminar" OnClientClick="return PreguntoEliminar();" Text="" Width="20px">
                                             <span class="glyphicon glyphicon-remove">Eliminar</span></asp:LinkButton></td>
                         
                         </tr>
                     </table>
                   
                 
                   
                 
                   
                 
                   
                 </td>
             </tr>
         </table>
      

     </ItemTemplate>
 
                                                    

               

             


</asp:DataList>
                                    
				</div>
       </div>
      </div>
 </form>
</body>
  
</html>
