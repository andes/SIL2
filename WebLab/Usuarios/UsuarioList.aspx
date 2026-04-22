<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsuarioList.aspx.cs" Inherits="WebLab.Usuarios.UsuarioList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

  <link type="text/css" rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />
 <script type="text/javascript" src="../script/jquery.min.js"></script>
 <script type="text/javascript" src="../script/jquery-ui.min.js"></script>
<script type="text/javascript" language="javascript">


    function PreguntoCambiarEstado() {
        if (confirm('¿Está seguro de cambiar estado?'))
            return true;
        else
            return false;
    }

   
</script>
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">           
 
    	   <div align="center" class="form-inline" style="width:1000px;"  >
               <table width="1150px" align="center" class="myTabla">
                   <tr>
                       <td colspan="5">
                           <div id="pnlTitulo" runat="server" class="panel panel-default">
                                 
                            <div class="panel-heading">
                                <h3 class="panel-title"> USUARIOS
                                </h3>
                                </div>
       	                    <div class="panel-body">	
                        <table  align="left" width="100%" > 
                            <tr >
                                <td class="myLabelIzquierda" > Efector: </td>
                                <td >
                                     <asp:DropDownList ID="ddlEfector" CssClass="form-control input-sm" Width="300px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEfector_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td class="myLabelIzquierda">Perfil:  </td>
                                  <td>  <asp:DropDownList ID="ddlPerfil" CssClass="form-control input-sm" Width="300px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged">
                                        </asp:DropDownList>
                                 </td>
                                </tr>
                            <tr> 
                            </tr>
                            <tr>
                                <td class="myLabelIzquierda">Username: </td>
                                 <td><asp:TextBox ID="txtUsername" CssClass="form-control input-sm" Width="300px" runat="server"></asp:TextBox></td>

                                <td class="myLabelIzquierda"> Tipo Autenticacion: </td>
                                <td><asp:DropDownList ID="ddlTipoAutenticacion" CssClass="form-control input-sm" Width="300px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoAutenticacion_SelectedIndexChanged">
                                      <asp:ListItem Value="0" Text="Todos" ></asp:ListItem>
                                       <asp:ListItem Value="SIL" Text="SIL" ></asp:ListItem>
                                       <asp:ListItem Value="ONELOGIN" Text="ONELOGIN"></asp:ListItem>
                                    </asp:DropDownList>  
                                </td>
                            </tr>
                             <tr>
                                 <td class="myLabelIzquierda"> Nombre: </td>
                                <td> <asp:TextBox ID="txtNombre" CssClass="form-control input-sm" Width="300px" runat="server"></asp:TextBox></td>
                                <td class="myLabelIzquierda"> Apellido: </td>
                                <td><asp:TextBox ID="txtApellido" CssClass="form-control input-sm" Width="300px" runat="server"></asp:TextBox>   
                                </td> 

                             </tr>
                            <tr>
                                <td class="myLabelIzquierda">Habilitados:</td>
                                <td><asp:DropDownList ID="ddlHabilitados" CssClass="form-control input-sm" Width="300px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlHabilitados_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="Todos" ></asp:ListItem>
                                       <asp:ListItem Value="1" Text="Habilitados" ></asp:ListItem>
                                       <asp:ListItem Value="2" Text="Deshabilitados"></asp:ListItem>
                                    </asp:DropDownList></td>
                                
                                <td> <asp:CheckBox ID="chbAdministrador" Text="Solo Administradores" Visible="false" runat="server" OnCheckedChanged="chbAdministrador_CheckedChanged" AutoPostBack="true" /></td>
                            </tr>
        
                            <tr>
                                <td colspan="4" align="right">
                                          <img alt="" src="../App_Themes/default/images/excelPeq.gif" />
                    <asp:LinkButton ID="lnkExcel" runat="server" CssClass="myLittleLink"   ValidationGroup="0" OnClick="lnkExcel_Click">Exportar a Excel</asp:LinkButton> &nbsp;&nbsp;
	

                                     <asp:Button ID="btnBuscar" Text="Buscar" runat="server" OnClick="btnBuscar_Click" ToolTip="Haga clic aquí para buscar" Width="100px" Font-Size="10pt" CssClass="btn btn-primary"/>&nbsp;&nbsp;
                                     <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click"  Text="Agregar" Width="100px" Font-Size="10pt" CssClass="btn btn-primary" Visible="False" />
       
                                </td>
                            </tr>
                        </table>
              
            
   
        
              
                                      </div>
       	                    <div class="panel-footer">
                              <asp:Panel ID="pnlLista" runat="server">
                                    <table style="width: 100%;">
                                        <tr>
                                           
                                         <td>&nbsp;<asp:Label ID="CantidadRegistros" runat="server" ForeColor="Blue" />
                                             &nbsp;<asp:Label ID="CurrentPageLabel" runat="server" ForeColor="Blue" />
                                         </td>
                                         <td>&nbsp;</td>
 
                                        </tr>
                               <tr> 
                                   <td colspan="2"> 
                                        <asp:GridView ID="gvLista" runat="server"
                                            AutoGenerateColumns="False" 
                                            CellPadding="1" DataKeyNames="idUsuario" 
                                            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
                                            onrowdatabound="gvLista_RowDataBound"   Width="100%" 
                                            EmptyDataText="No hay usuarios creados"  CssClass="table table-bordered bs-table"  GridLines="Horizontal"
                                            PageSize="50"
                                            AllowPaging="true" OnPageIndexChanging="gvLista_PageIndexChanging" 
                                            AllowSorting="True" OnSorting="gvLista_Sorting"    >
                                            <Columns>                
                                        
                                                <asp:BoundField DataField="username"  HeaderText="Usuario" SortExpression="username">
                                                    <ItemStyle Width="20%" />
                                                </asp:BoundField>
                
                                                  <asp:BoundField DataField="apellido" HeaderText="Apellido" SortExpression="apellido">
                                                      <ItemStyle Width="20%" />
                                                  </asp:BoundField>

                                                <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre">
                                                    <ItemStyle Width="20%" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="perfil" HeaderText="Perfil" SortExpression="perfil" >
                                                    <ItemStyle Width="10%" />
                                                </asp:BoundField>
                                                 
                                                <asp:BoundField DataField="Efector" HeaderText="Efector" SortExpression="Efector" >
                                                    <ItemStyle Width="30%" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="habilitado" HeaderText="Activo" SortExpression="habilitado" />

                                               <asp:BoundField DataField="tipoAutenticacion" HeaderText="Tipo Autenticacion" SortExpression="tipoAutenticacion">
                                                    <ItemStyle Width="30%" />
                                                </asp:BoundField>

                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                    <asp:ImageButton ID="Modificar" ToolTip="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                                                        CommandName="Modificar" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                </asp:TemplateField>

                                                  <asp:TemplateField   HeaderText="Habilitar">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" runat="server"  
                                                            AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged"  UseSubmitBehavior="false"
                                                            Checked='<%# Convert.ToBoolean(Eval("activo")) %>'
                                                            />
                                                    </ItemTemplate>                    
                                                       <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                </asp:TemplateField>
           
                                            </Columns>
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" CssClass="GridPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
                                            <EditRowStyle BackColor="#999999" />
                                            <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                            <RowStyle Font-Size="12px" />
                                            <AlternatingRowStyle Font-Size="12px" BackColor="White" ForeColor="#284775" />

                                </asp:GridView>      
                                      </td> 
                               </tr> </table>
                              </asp:Panel>

       	                    </div>
                           </div>
                         
                       </td>
                   </tr>
               </table>
  
               

</div>	
</asp:Content>

