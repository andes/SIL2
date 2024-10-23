<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaracterList.aspx.cs" Inherits="WebLab.Caracteres.CaracterList" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server" />
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

       <div align="left" style="width: 800px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
CARACTER PROTOCOLO COVID19
                        </h3>
                       
                        </div>
        	<div class="panel-body">	
	   
		 
   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idCaracter" CssClass="table table-bordered bs-table" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="9pt" Width="100%" 
                        EmptyDataText="No hay registros creados" BorderColor="#3A93D2" 
                        BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
          
            <Columns>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Caracter" >
                    <ItemStyle Width="90%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
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
           
      
        </asp:GridView>
             
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt"   CssClass="btn btn-primary"     Width="100px"
                                        ToolTip="Haga clic aquí para agregar un nuevo registro" />
                              </div>

	

   
  </div>
    </div>
   
</asp:Content>