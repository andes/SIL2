<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemInsumo.aspx.cs" Inherits="WebLab.Items.ItemInsumo" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
      <div align="left" style="width: 1100px" class="form-inline"  >
   <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">  Auditoria Insumos </h3>
                       
                        </div>

				<div class="panel-body">

      <table  align="left" width="100%" >
					
					
					<tr>
						<td  >Efector:&nbsp;</td>
						<td>
                            <anthem:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector" AutoPostBack="True"  >
                            </anthem:DropDownList>
                                            </td>
					</tr>

          	<tr>
						<td   >Servicio:</td>
						<td>
                            <anthem:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="4" class="form-control input-sm" 
                                onselectedindexchanged="ddlServicio_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </anthem:DropDownList>
                            </td>
                  </tr>
                                        
          <tr>	<td    >
                                                        Area:</td>
						<td>
                                        
                            <anthem:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="5" class="form-control input-sm" 
                                AutoPostBack="True" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged"  >
                            </anthem:DropDownList>
                            </td>

          </tr>

          <tr>	<td    >
                                                        Estado:</td>
						<td>
                                        
                            <anthem:DropDownList ID="ddlEstado" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="5" class="form-control input-sm" 
                                AutoPostBack="True" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" >
                                <asp:ListItem Value="T">Todas</asp:ListItem>
                                <asp:ListItem Value="D">Disponible</asp:ListItem>
                                <asp:ListItem Value="S">Sin Insumo</asp:ListItem>
                            </anthem:DropDownList>
                            </td>

          </tr>

          </table>         
           
               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
          
            
                  <div  > Seleccionar: <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="form-control input-sm"  onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="form-control input-sm"  onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton>
                    <br />
                    </div> 
         </div>
      
                 
       <div class="panel-footer"> 
            <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
                		<div style="border: 1px solid #999999; height: 450px; width:1000px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="iditemefector" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
                EmptyDataText="No se encontraron resultados" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Cambiar Estado" >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                  
                </asp:TemplateField>
                <asp:BoundField DataField="Codigo"   HeaderText="Codigo" >
                
                </asp:BoundField>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" >
                
                </asp:BoundField>
                <asp:BoundField DataField="Estado" HeaderText="Estado" >
                
                </asp:BoundField>
                 
                
                    <asp:BoundField DataField="sininsumo" HeaderText="">
                
                </asp:BoundField>
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>
        
               
              
        </div>  
            <asp:Button ID="btnGuardar" runat="server" onclick="btnGuardar_Click"  CssClass="btn btn-primary" Width="150px"
                Text="Guardar" />

       </div>
          </div>
          </div>
        
</asp:Content>