<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RutinaBroList.aspx.cs" Inherits="WebLab.Rutinas.RutinaBroList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head" />
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
   
      <div align="left" style="width: 800px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE RUTINAS</h3>
                        </div>

				<div class="panel-body">
       
	   
	 

	

   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idRutina" CssClass="table table-bordered bs-table" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound"  Width="100%" 
                        EmptyDataText="No hay rutinas creadas"   GridLines="None">
           
            <Columns>
             <asp:BoundField DataField="servicio" 
                    HeaderText="Servicio" >
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                 <asp:BoundField DataField="area" 
                    HeaderText="Area" >
                    <ItemStyle Width="40%" />
                </asp:BoundField>
                <asp:BoundField DataField="nombre" HeaderText="Rutina" >
                    <ItemStyle Width="40%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
                 <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                            <ItemTemplate> 
                                <asp:Button ID="Muestra" runat="server" Text="Vincular Muestra" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                    </div>
               	<div class="panel-footer">
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt"  CssClass="btn btn-primary"  Width="120px"
                                        ToolTip="Haga clic aquí para agregar una nueva rutina" />
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