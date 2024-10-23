<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemList.aspx.cs" Inherits="WebLab.Items.ItemList" MasterPageFile="~/Site1.Master" 
    Debug="true"
    %>



<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
<div align="left" style="width: 1200px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">LISTA DE ANALISIS</h3>
                        </div>
          				<div class="panel-body">	
  <table  align="left" width="100%" >
					
					
					<tr>
						<td class="myLabelIzquierda" >Efector:&nbsp;</td>
						<td colspan="4">
                            <anthem:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" AutoPostBack="True" OnSelectedIndexChanged="ddlEfector_SelectedIndexChanged">
                            </anthem:DropDownList>
                                            </td>
					</tr>
					
					
					<tr>
						<td class="myLabelIzquierda" >Codigo:</td>
						<td>
                            <asp:TextBox ID="txtCodigo" runat="server" MaxLength="100" 
                                Width="86px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el codigo del análisis" TabIndex="3" 
                                class="form-control input-sm"  />
                                            </td>
						<td class="myLabelIzquierda"  colspan="2">
                                                        Nombre corto :</td>
						<td>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" 
                                Width="236px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el nombre del analisis" TabIndex="3" 
                                class="form-control input-sm"  />
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Servicio:</td>
						<td>
                            <anthem:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="4" class="form-control input-sm" 
                                onselectedindexchanged="ddlServicio_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </anthem:DropDownList>
                                        
                                            </td>
						<td class="myLabelIzquierda"  colspan="2">
                                                        Area:</td>
						<td>
                                        
                            <anthem:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="5" class="form-control input-sm" 
                                AutoPostBack="True" onselectedindexchanged="ddlArea_SelectedIndexChanged1">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Ordenar por:</td>
						<td>
                            <asp:DropDownList ID="ddlTipo" runat="server" 
                                ToolTip="Seleccione el orden de la lista" TabIndex="6" class="form-control input-sm"  
                                AutoPostBack="True" onselectedindexchanged="ddlTipo_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Codigo</asp:ListItem>
                                <asp:ListItem Value="1">Nombre</asp:ListItem>
                                <asp:ListItem Value="2">Area</asp:ListItem>
                            </asp:DropDownList>
                                        
                                            </td>
						<td class="myLabelIzquierda"  colspan="2">
                                                        Derivaciones:</td>
						<td>
                                        
                            <anthem:DropDownList ID="ddlEfectorDerivacion" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" AutoPostBack="True" OnSelectedIndexChanged="ddlEfectorDerivacion_SelectedIndexChanged">
                            </anthem:DropDownList>
                                            <asp:CheckBox ID="chkDerivados" runat="server" Text="Solo Derivados" />
                                        
                                            </td>
					</tr>
						<tr>
						<td class="myLabelIzquierda" >
                                            &nbsp;</td>
						<td colspan="2">
                            &nbsp;</td>
						<td colspan="2" align="right">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="100px"
                                                onclick="btnBuscar_Click" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" />&nbsp;&nbsp;
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" Width="100px"
                                                onclick="btnNuevo_Click" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para agregar un nuevo analisis" />
                                        
                            </td>
						
					</tr>
					
					</table>
					
					
					          </div>

			<div class="panel-footer">		
                <div>
                    <asp:LinkButton ID="lnkPDF" runat="server" CssClass="btn btn-info"  OnClick="lnkPDF_Click" Width="200px" Visible="False" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar Nomenclador</asp:LinkButton>

                     
                               

                    <asp:LinkButton ID="lnkPdfReducido" runat="server" CssClass="btn btn-info"  OnClick="lnkPdfReducido_Click" Width="200px"  >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar Listado PDF</asp:LinkButton>
                    <asp:LinkButton ID="lnkExcel" runat="server" CssClass="btn btn-success"    Width="200px" OnClick="lnkExcel_Click" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar Listado Excel</asp:LinkButton>
					</div>	
                <asp:label id="CurrentPageLabel"
                  forecolor="Blue"
                  runat="server"/>

                   <asp:label id="lblError"
                  forecolor="Red"
                  runat="server"/>
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idItem" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" 
                                onselectedindexchanged="gvLista_SelectedIndexChanged" 
                                AllowPaging="True" onpageindexchanging="gvLista_PageIndexChanging" 
                                onselectedindexchanging="gvLista_SelectedIndexChanging" 
                                CssClass="table table-bordered bs-table" 
                                GridLines="Horizontal" PageSize="15" 
                                EmptyDataText="No se encontraron analisis para los filtros de busqueda ingresados" 
                                ToolTip="Lista de analisis" ondatabound="gvLista_DataBound">
            <RowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="codigoNomenclador" HeaderText="Nomenclador">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="15%" />
                </asp:BoundField>
          <asp:BoundField DataField="codigo" 
                    HeaderText="Codigo" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="15%" />
                </asp:BoundField>
                <asp:BoundField DataField="nombre" HeaderText="Nombre" >
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="40%" />
                </asp:BoundField>
                <asp:BoundField DataField="area" HeaderText="Area">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="tipo" HeaderText="Tipo">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                </asp:BoundField>
                <asp:BoundField DataField="disponible">
                    <ItemStyle Width="5px" />
                </asp:BoundField>
               <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                            CommandName="Editar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
               
                           <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" Height="20px" HorizontalAlign="Center" />
                          
                        </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" Position="Top" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" 
                BorderColor="White" VerticalAlign="Top" />
                
               
      
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                		 
                                            
                    </div>
          </div>
     </div>
<%--    </form>
    --%>
  
    
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
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
    
    <style type="text/css">
        .style1
        {}
    </style>
    
</asp:Content>
