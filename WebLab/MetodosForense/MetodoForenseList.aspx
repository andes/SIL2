﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MetodoForenseList.aspx.cs" Inherits="WebLab.MetodosForense.MetodoForenseList" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    
    
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
      
   <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
 LISTA DE METODOS
                        </h3>
                       
                        </div>
        	<div class="panel-body">	
	   
	 

	

   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idMetodo" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" CssClass="table table-bordered bs-table" Width="100%" 
                        EmptyDataText="No hay métodos creados"  >
          
            <Columns>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Metodo" >
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
            
        </asp:GridView>
                 
                </div>
      <div class="panel-footer">	
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" CssClass="btn btn-primary" Width="100px"
                                        ToolTip="Haga clic aquí para agregar un nuevo método" />
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