﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerfilList.aspx.cs" Inherits="WebLab.Antibioticos.PerfilList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 <div align="left" class="form-inline" style="width: 800px" >
  
          
	   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">PERFILES DE ANTIBIOTICOS</h3>
                        </div>

				<div class="panel-body">	
       
                  
	    

	

     
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idPerfilAntibiotico"  CssClass="table table-bordered bs-table" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="11pt" Width="97%" 
                        EmptyDataText="No hay perfiles creados" BorderColor="#3A93D2" 
                        BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>  
             <asp:BoundField DataField="nombre" HeaderText="Nombre del Perfil" >
                    <ItemStyle Width="60%" HorizontalAlign="Left" />
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
        
                
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt"  CssClass="btn btn-primary" Width="100px" />
                                

	  </div>
           </div>
      </div>
    
        
    <script type="text/javascript" language="javascript">

        function PreguntoEliminar() {
            if (confirm('¿Está seguro de eliminar el registro?'))
                return true;
            else
                return false;
        }
    </script>
</asp:Content>
