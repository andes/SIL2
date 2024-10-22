<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsuarioList.aspx.cs" Inherits="WebLab.Usuarios.UsuarioList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

 
     

   
  
   
    </asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">           
 
    	   <div align="center" class="form-inline" style="width:800px;"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"> USUARIOS
                        </h3>
                        </div>
       	<div class="panel-body">	
    
     <div>
         <h3>Efector:</h3> <asp:DropDownList ID="ddlEfector" CssClass="form-control input-sm" Width="300px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEfector_SelectedIndexChanged"></asp:DropDownList>
     </div>
               <br />
                <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Width="100px" Font-Size="10pt" CssClass="btn btn-primary" />
	

   
        
              
                  </div>
       	<div class="panel-footer">	
                       <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idUsuario" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound"   Width="100%" 
                        EmptyDataText="No hay usuarios creados"  CssClass="table table-bordered bs-table"  GridLines="Horizontal">
            
            <Columns>                
             <asp:BoundField DataField="username" 
                    HeaderText="Usuario" >
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                
                  <asp:BoundField DataField="apellido" HeaderText="Apellido" >
                      <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="perfil" HeaderText="Perfil" >
                        
                    <ItemStyle Width="10%" />
                </asp:BoundField>
                   <asp:BoundField DataField="Efector" HeaderText="Efector" >
                        
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="habilitado" HeaderText="Activo" />
                        
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" ToolTip="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
             <%--    <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>--%>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>            
                             </div>
       </div>
               

</div>	

   

    
        
    <script type="text/javascript" language="javascript">
    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar el registro?'))
    return true;
    else
    return false;
    }
    </script>
</asp:Content>