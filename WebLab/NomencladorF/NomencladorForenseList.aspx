<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NomencladorForenseList.aspx.cs" Inherits="WebLab.NomencladorF.NomencladorForenseList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    
    
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
      
   <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
 Nomenclador Forense
                        </h3>
                       
                        </div>
        	<div class="panel-body">	
   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idNomencladorForense" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="9pt" Width="100%" 
                        EmptyDataText="No hay registros creados" CssClass="table table-bordered bs-table">
        
            <Columns>

                   <asp:BoundField DataField="codigo" 
                    HeaderText="Codigo" >
                   
                </asp:BoundField>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Nombre" >
                    <ItemStyle Width="90%" />
                </asp:BoundField>
                 <asp:BoundField DataField="precio" 
                    HeaderText="Precio" >
                    
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
                 <asp:TemplateField HeaderText="" Visible="false">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
            </Columns>
          
        </asp:GridView>
                 
                </div>
       <div class="footer">
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar"  CssClass="btn btn-primary" Width="150px"
                                        ToolTip="Haga clic aquí para agregar" />
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