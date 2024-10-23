<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnidadList.aspx.cs" Inherits="WebLab.UnidadesMedida.UnidadList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    
             <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">UNIDAD DE MEDIDA</h3>
                        </div>

				<div class="panel-body">
  
	    
	

   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  
            CellPadding="1" DataKeyNames="idUnidadMedida" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound"  Width="100%" 
                        EmptyDataText="No hay unidades de medida creadas" CssClass="table table-bordered bs-table"  GridLines="Horizontal">
          

            <Columns>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Unidad de Medida" >
                    <ItemStyle Width="90%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
                 <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
               <br />
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" CssClass="btn btn-primary" Width="100px" />
                                

	

   
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