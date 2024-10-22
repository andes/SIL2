<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoListResultado.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoListResultado" MasterPageFile="~/Resultados/SitePE.Master" %>

 



<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

  <br />   &nbsp;
                &nbsp;<div align="left" style="width: 1000px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">RESULTADOS DE CASOS DE HISTOCOMPATIBILIDAD</h3>
                        </div>
          				<div class="panel-body">	
                               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
  <table  align="left" width="100%" >
					
					
						<tr>
                                <td  class="auto-style3" >Titulo del Caso/DNI del Receptor:</td>
                            
						<td class="auto-style4">
                                                 
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" 
                                Width="236px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el nombre del caso" TabIndex="3" 
                                class="form-control input-sm"  />
                                        
                        </td>
					

					

						
					</tr>
					
						<tr>
                               <td  class="auto-style3" >Nro. de Caso:</td>
						<td class="auto-style4">
                              <asp:TextBox ID="txtNumero" runat="server" MaxLength="20" 
                                Width="136px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el numero de caso" TabIndex="3" 
                                class="form-control input-sm"  />
                                        
                            </td>
						

						
					</tr>
					
						<tr>
                              <td  class="auto-style3" >Ordenar:</td>
						<td class="auto-style4">
                           

                             <asp:DropDownList ID="ddlTipo" runat="server" 
                                ToolTip="Seleccione el orden de la lista" TabIndex="6" class="form-control input-sm"  
                                AutoPostBack="True" onselectedindexchanged="ddlTipo_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Desde el mas reciente</asp:ListItem>
                                <asp:ListItem Value="1">Desde el mas antiguo</asp:ListItem>
                            </asp:DropDownList>
                                        
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="100px"
                                                onclick="btnBuscar_Click" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" />
                           

						</td>
						

						
					</tr>
					

					
					 
					
					</table>
					  
					
					          </div>

			<div class="panel-footer">	
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idCasoFiliacion" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" CssClass="table table-bordered bs-table" 
                                onselectedindexchanged="gvLista_SelectedIndexChanged" 
                                AllowPaging="True" onpageindexchanging="gvLista_PageIndexChanging" 
                                onselectedindexchanging="gvLista_SelectedIndexChanging" 
                                
                                GridLines="Horizontal" PageSize="20" 
                                EmptyDataText="No se encontraron registros para los filtros de busqueda ingresados" 
                                ToolTip="Lista de casos" ondatabound="gvLista_DataBound">
            <RowStyle BackColor="White" />
            <Columns>
          <asp:BoundField DataField="idCasoFiliacion" 
                    HeaderText="Numero" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="nombre" HeaderText="Titulo" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
              


                  <asp:TemplateField HeaderText="Resultado">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Resultados" runat="server" Text="" Width="20px"  >
                                             Visualizar</asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>

           


             


               


                <asp:BoundField DataField="usuario" HeaderText="Usuario Validacion" />

           


             


               


            </Columns>
            <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
                 <asp:label id="CurrentPageLabel"
                  forecolor="Blue"
                  runat="server"/>	
                                      

				

                		      
                

          </div>
     </div>
                    </div>


  
    
  

</asp:Content>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style3 {
            width: 209px;
        }
        .auto-style4 {
            width: 78%;
        }
    </style>
</asp:Content>



