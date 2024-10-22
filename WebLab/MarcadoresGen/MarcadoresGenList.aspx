<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarcadoresGenList.aspx.cs" Inherits="WebLab.MarcadoresGen.MarcadoresGenList" MasterPageFile="~/Site1.Master" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    
    
    
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
   

        <div align="center" style="width: 1000px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE MARCADORES</h3>
                        </div>

				<div class="panel-body">
       
	       <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
            CellPadding="1" DataKeyNames="idTipoMarcador" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound"  Width="100%" 
                        EmptyDataText="No se encontraron registros para los filtros de busqueda ingresados"   GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Nombre" >
                    <HeaderStyle HorizontalAlign="Left" />
                    
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
            
        </asp:GridView>
	 

	</div>
         <div class="panel-footer">
    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" CssClass="btn btn-primary" Width="80px"
                                        ToolTip="Haga clic aquí para agregar una nueva hoja de trabajo" />
    
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