<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloAdjuntar.aspx.cs" Inherits="WebLab.Protocolos.ProtocoloAdjuntar"  MasterPageFile="~/Site1.Master" %>



<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>

  

  
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
  
        <div align="left" style="width: 1200px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h1 class="panel-title">Archivos Anexos</h1>
                        </div>

				<div class="panel-body">	
   <div class="form-group">
  <h4>  <label for="ejemplo_password_1">Protocolo:</label> <asp:Label ID="lblProtocolo" runat="server" 
                    
        ></asp:Label></h4><input id="hdnProtocolo" runat="server" type="hidden" /></div>
                    <br />
                    <div class="form-group" id="datos" runat="server">
    <label for="ejemplo_password_1">Archivo:</label>
 <asp:FileUpload ID="trepador" runat="server"  class="form-control input-sm" />
                       
                 


    <label for="ejemplo_password_1">Visibilidad:</label>
                     <asp:DropDownList ID="ddlVisibilidad" runat="server"  class="form-control input-sm" >
    <asp:ListItem Selected="True" Value="0">No visible (interno)</asp:ListItem>
    <asp:ListItem Value="1">Todos los usuarios</asp:ListItem>
</asp:DropDownList>
    <label for="ejemplo_password_1">Descripcion:</label>       <asp:TextBox ID="txtDescripcion" runat="server" Width="200px"  class="form-control input-sm"></asp:TextBox>
                     <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="subir_Click"    Width="150px" >
                                             <span class="glyphicon glyphicon-arrow-up"></span>&nbsp;Subir Archivo</asp:LinkButton>
                    
                    
                     
            <div>
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
            </div>

                        <asp:Button ID="btnRegresar"  class="btn btn-warning" Width="120px"  runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
                          <br />
                   </div>
       <br />
    <div class="panel-footer">		
        


     <asp:DataList ID="DTlist" runat="server" OnItemDataBound="DTlist_ItemDataBound" 
          OnItemCommand="DTlist_ItemCommand" CellPadding="0" CellSpacing="4" >
     <ItemTemplate>
          

         <table cellpadding="2" cellspacing="2">
             <tr>
                 <td> <b><asp:Label ID="Label1" runat="server" Text='<%# Eval("descripcion") %>'></asp:Label></b> </td>
             </tr>
             <tr>
                 <td>
                       <asp:Image ID="img" runat="server" ImageUrl='<%# Eval("Url") %>'
             Width="300px" Height="300px" />
                 </td>
             </tr>
             <tr>
                 <td>
                     <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px" CommandName="Eliminar" CommandArgument='<%# Eval("idProtocoloAnexo") %>'  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
           <asp:Button ID="Descargar" runat="server" Text="Descargar" Width="90px" CssClass="btn btn-info" CommandName="Descargar"  CommandArgument='<%# Eval("idProtocoloAnexo") %>'
                         />
                      <b><asp:Label ID="lblVisible" runat="server" Text='<%# Eval("visible") %>'></asp:Label></b> 
                 </td>
             </tr>
             <tr>
                 <td>
                      Subido por <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("auditoria") %>'></asp:Label>
                 </td>
             </tr>
         </table>
      

     </ItemTemplate>
 
                                                    

               

             


</asp:DataList>
        </div>
    </div>
            </div>
            <script language="javascript" type="text/javascript">



    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar el archivo?'))
    return true;
    else
    return false;
}




    </script>
    </div>
</asp:Content>