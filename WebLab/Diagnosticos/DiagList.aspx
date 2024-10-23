<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiagList.aspx.cs" Inherits="WebLab.Diagnosticos.DiagList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <br />
    <br />
    <div align="left" style="width: 900px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">DIAGNOSTICOS PRESUNTIVOS</h3>
                        </div>

				<div class="panel-body">
                       <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
                    <br />
                     <div align="left" style="overflow:scroll;overflow-x:hidden; height: 400px;">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered bs-table" 
            CellPadding="1" DataKeyNames="idDiagnostico" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="9pt" Width="97%" 
                        EmptyDataText="No hay diagnosticos creados" BorderColor="#3A93D2" 
                        BorderStyle="Solid" BorderWidth="3px" GridLines="Horizontal">
           
            <Columns>
                 <asp:BoundField DataField="codigo" 
                    HeaderText="Codigo" >
                    <ItemStyle Width="10%" />
                </asp:BoundField>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Nombre" >
                    <ItemStyle Width="80%" />
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
          <%--  <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />--%>
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
          <%--  <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
        </asp:GridView>
        </div>
                    </div>
       <div class="panel-footer">
             <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" Width="100px" CssClass="btn btn-info"
                                        ToolTip="Haga clic aquí para agregar uno nuevo" />
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