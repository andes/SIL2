<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormulaList.aspx.cs" Inherits="WebLab.Formulas.FormulaList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
    	 <div align="left" style="width: 1000px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE FORMULAS Y CONTROLES</h3>
                        </div>

				<div class="panel-body">
	   
	 
			 
  
       
	   
		 
			 
			 
			 
	

   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="2" DataKeyNames="idFormula" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" Font-Size="10pt" Width="100%" 
                        EmptyDataText="No hay fórmulas creadas" CssClass="table table-bordered bs-table"  GridLines="None">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                  <asp:BoundField DataField="codigo" 
                    HeaderText="Codigo" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%" />
                </asp:BoundField>
             <asp:BoundField DataField="item" 
                    HeaderText="Analisis" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="nombre" HeaderText="Expresión" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="50%" />
                </asp:BoundField>
                <asp:BoundField DataField="tipo" HeaderText="Tipo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                          
                        </asp:TemplateField>
                 <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                          
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
                                        Text="Agregar Formula" Font-Size="10pt" CssClass="btn btn-primary" Width="120px"
                                        ToolTip="Haga clic aquí para agregar una formula" /> &nbsp; &nbsp;
                                    <asp:Button ID="btnAgregarControl" runat="server" onclick="btnAgregarControl_Click" 
                                        Text="Agregar Control" Font-Size="10pt" CssClass="btn btn-danger" Width="120px" ToolTip="Haga clic aquí para agregar un control" />
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
