<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AntibioticoList.aspx.cs" Inherits="WebLab.Antibioticos.AntibioticoList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
    	 <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE ANTIBIOTICOS</h3>
                        </div>

				<div class="panel-body">
	   
	 
			 
		 
			 
	

    <div align="left" style="overflow:scroll;overflow-x:hidden;height:600px;">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idAntibiotico" CssClass="table table-bordered bs-table" 
             onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="11pt" Width="97%" 
                        EmptyDataText="No hay Antibioticos creados"  
                         GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
            <Columns>  
             <asp:BoundField DataField="nombreCorto" HeaderText="Codigo" >
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
             <asp:BoundField DataField="descripcion" 
                    HeaderText="Descripcion" >
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
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        </div>
              

                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt"  CssClass="btn btn-primary" Width="100px" />
                             

	
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