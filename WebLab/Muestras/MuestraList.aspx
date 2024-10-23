<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MuestraList.aspx.cs" Inherits="WebLab.Muestras.MuestraList" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
             <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">TIPO DE MUESTRA</h3>
                        </div>

				<div class="panel-body">
  
       
	    
		 
			 
	
       <div align="left" style="overflow:scroll;overflow-x:hidden;height:600px;">
   
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" DataKeyNames="idMuestra"  CssClass="table table-bordered bs-table" 
            ForeColor="#333333" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound"   Width="100%" 
                        EmptyDataText="No hay tipos de muestras creadas"   GridLines="Both">
            
            <Columns>
                <asp:BoundField DataField="codigo" 
                    HeaderText="Codigo" >
                    <ItemStyle Width="10%" />
                </asp:BoundField>
             <asp:BoundField DataField="nombre" 
                    HeaderText="Descripción" >
                    <ItemStyle Width="80%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Modificar">
                            <ItemTemplate>
                            <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg"
                             CommandName="Modificar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                          
                        </asp:TemplateField>
                        <asp:TemplateField   HeaderText="Habilitado">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkStatus" runat="server" 
                            AutoPostBack="true" OnCheckedChanged="chkStatus_OnCheckedChanged"
                            Checked='<%# Convert.ToBoolean(Eval("Habilitado")) %>'
                            />
                    </ItemTemplate>                    
                       <ItemStyle HorizontalAlign="Center" Width="10%" />
                </asp:TemplateField>

              <%--   <asp:TemplateField HeaderText="">
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
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
           </div>
        </div>
              <div class="panel-footer">
                                    <asp:Button ID="btnAgregar" runat="server" onclick="btnAgregar_Click" 
                                        Text="Agregar" Font-Size="10pt" CssClass="btn btn-primary" Width="100px" 
                                        ToolTip="Haga clic aquí para agregar un nuevo tipo de muestra" />
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