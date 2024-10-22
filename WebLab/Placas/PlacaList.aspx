<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="PlacaList.aspx.cs" Inherits="WebLab.Placas.PlacaList" ValidateRequest="false" %>


<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>


<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
     
   

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
  <script type="text/javascript"> 


      $(function () {
          $("#<%=txtFechaDesde.ClientID %>").datepicker({
            maxDate: 0,
            minDate: null,

            showOn: 'button',
            buttonImage: '../App_Themes/default/images/calend1.jpg',
            buttonImageOnly: true
        });
    });

      $(function () {
          $("#<%=txtFechaHasta.ClientID %>").datepicker({
            maxDate: 0,
            minDate: null,

            showOn: 'button',
            buttonImage: '../App_Themes/default/images/calend1.jpg',
            buttonImageOnly: true
        });
    });


  </script>  
  
 
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
         
    <div align="left" style="width: 1400px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">PLACAS </h3> 
                        </div>
          				<div class="panel-body">	
                               <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
  <table  align="left" width="100%" >
					
					
						<tr>
						<td>
                                                        Fecha Desde:</td>
						<td>
                                                          <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px"  /></td>
						<td>
                            Fecha Hasta: <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"  /> </td>
						<td align="right">
                                            &nbsp;</td>
						
					</tr>
					
					
						<tr>
						<td>
                                    Tipo:</td>
						<td>
                            <asp:DropDownList ID="ddlEquipo" runat="server" 
                                class="form-control input-sm" TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlEquipo_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="-1">Todos</asp:ListItem>
                                                <asp:ListItem Value="Alplex">Alplex</asp:ListItem>
                                                <asp:ListItem Value="Promega">Promega-46M</asp:ListItem>
                                 <asp:ListItem Value="Promega-30M">Promega-30M</asp:ListItem>
                                            </asp:DropDownList>
                                                                    
                            </td>
						<td>
                            Ordenar: <asp:DropDownList ID="ddlTipo" runat="server" 
                                ToolTip="Seleccione el orden de la lista" TabIndex="6" class="form-control input-sm"  
                                AutoPostBack="True" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Desde la más reciente</asp:ListItem>
                                <asp:ListItem Value="1">Desde la más antiguo</asp:ListItem>
                                
                            </asp:DropDownList>
                                        
                        </td>
						<td align="right">
                            &nbsp;</td>
						
					</tr>
					
						<tr>
						<td>
                            Nro Placa: 
                                        
                            </td>
						<td>
                            <asp:TextBox ID="txtNumero" runat="server" MaxLength="20" 
                                Width="100px" 
                                ToolTip="Ingrese el numero de placa" TabIndex="1" 
                                class="form-control input-sm" BackColor="#FFFF99" Font-Bold="True" ForeColor="#990000" Height="30px" Font-Size="12pt"  />
                                        
                            </td>
						<td>
                            Estado:&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlEstado" runat="server" 
                                class="form-control input-sm" TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="-1">Todos los activos</asp:ListItem>
                                                <asp:ListItem Value="A">Abierta</asp:ListItem>
                                                <asp:ListItem Value="C">Cerrada</asp:ListItem>
                                <asp:ListItem Value="V">Validada</asp:ListItem>
                                              
                           <asp:ListItem Value="B">Anulados</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
						<td align="right">
                                        
                            &nbsp;</td>
						
					</tr>
					
					

						<tr>
						<td colspan="2">

                            <table>
                                <tr>
                                    
                                    <td>  <span class="label label-warning">Abierta</span> </td>
                                    <td> <span class="label label-success">Cerrada</span></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><br /></td>
                                </tr>
                                 <tr>
                                    <td>    <span class="label label-warning">  <asp:label id="lblAbierta"
                  
                  runat="server"/></span>	 </td>
                                    <td>   <span class="label label-success"><asp:label id="lblCerrada"
                  
                  runat="server"/>	</span></td>
                                </tr>
                            </table>
                            
                         
                            
                            
                          
                            
                            
                           </td>
						<td colspan="2" align="right">
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="100px" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" OnClick="btnBuscar_Click" />
                                            <br />
                            
                          
                                            &nbsp;<asp:Button ID="btnNuevoAlplex" runat="server" Text="Nueva Alplex" Width="150px" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para agregar una nueva placa" OnClick="btnNuevoAlplex_Click" />
                               &nbsp;<asp:Button ID="btnNuevoAlplex8V" runat="server" Text="Nueva Allplex Respiratorios 2" Width="250px" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para agregar una nueva placa" OnClick="btnNuevoAlplex8V_Click"   />
                                         
                                                                    
                                            <br />
                                         
                                                                    
                            &nbsp;<asp:Button ID="btnNuevoPromega" runat="server" Text="Nueva Promega-46M" Width="150px" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para agregar una nueva placa" OnClick="btnNuevoPromega_Click" />
                                        
                                                                    
                            &nbsp;<asp:Button ID="btnNuevoPromega2" runat="server" Text="Nueva Promega-30M" Width="150px" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para agregar una nueva placa" OnClick="btnNuevoPromega2_Click" />
                                 
                                    &nbsp;<asp:Button ID="btnNuevoMixta" runat="server" Text="Nuevo Panel Respiratorio" Width="200px" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para agregar una nueva placa" OnClick="btnNuevoMixta_Click"   />
                             
                                        
                            </td>
						
					</tr>
					
						</table>
					
					
					          </div>

			<div class="panel-footer">	
                      <asp:label id="CurrentPageLabel"
                  forecolor="Blue"
                  runat="server"/>	
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idPlaca" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" CssClass="table table-bordered bs-table" 
                                onselectedindexchanged="gvLista_SelectedIndexChanged" 
                                AllowPaging="True" onpageindexchanging="gvLista_PageIndexChanging" 
                                onselectedindexchanging="gvLista_SelectedIndexChanging" 
                                
                                GridLines="Horizontal" PageSize="20" 
                                EmptyDataText="No se encontraron datos para los filtros de busqueda ingresados" 
                                ToolTip="Lista de placas" ondatabound="gvLista_DataBound" CellSpacing="3" Font-Size="12pt">
            <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" 
                BorderColor="White"   />
                
               
      
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
    

                   <asp:TemplateField HeaderText="Nro.">
                                                                        <ItemTemplate>
                                                                   <asp:Label ID="lblNumero" runat="server" 
    Text=' <%#: Eval("idPlaca") %>' >
</asp:Label>
                                                                             

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>
                   <asp:BoundField DataField="tipo" HeaderText="Tipo" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="operador" HeaderText="Operador" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                   <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                 <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha Registro" />

                   <asp:BoundField DataField="usuario" HeaderText="Usuario Registro" />
                      <asp:TemplateField HeaderText="Imprimir">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Imprimir" runat="server" Text="" Width="20px"  >
                                          <span class="label label-primary">   Imprimir</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                       
                                                                    </asp:TemplateField>

                 <asp:TemplateField HeaderText="Modificar">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Editar" runat="server" Text="" Width="20px"  >
                                                                                  <span class="label label-primary">Editar</span>
                                           </asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Eliminar" >
                                                                        <ItemTemplate>
                                                                               <asp:LinkButton ID="Eliminar"   runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                         <span class="label label-primary">    Eliminar</span></asp:LinkButton>
                                                                       
                                                                        </ItemTemplate>
                                                                       
                                                                       
                                                                    </asp:TemplateField>

                  

                <asp:TemplateField HeaderText="Auditoria">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Auditoria" runat="server" Text="" Width="20px"  >
                                          <span class="label label-primary">   Auditoria</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                       
                                                                    </asp:TemplateField>

                

                <asp:TemplateField HeaderText="Validacion">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Valida" runat="server" Text="" Width="20px"  >
                                       <span class="label label-primary">      Validacion</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                    
                                                                    </asp:TemplateField>


                  <asp:TemplateField HeaderText="Consultar">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Consultar" runat="server" Text="" Width="20px"  >
                                    <span class="label label-primary">         Consultar</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>
                   <asp:BoundField DataField="estado"  HeaderText="Estado" />
                        
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="False" ForeColor="White" 
                />
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
      <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
                
               
      
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
                <div>
					</div>	
                		 
                                            
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