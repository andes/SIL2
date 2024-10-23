<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoList.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoList" MasterPageFile="~/Site1.Master" %>
 

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
     
   

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
  <script type="text/javascript"> 
      

	$(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
		    maxDate: 0,
		    minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
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
         
    <div align="left" style="width: 1200px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"><asp:Label ID="lbltitulo" Font-Bold="true" runat="server"></asp:Label></h3><input runat="server"  type="hidden" id="idServicio"/>
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
                                                        Nombre:</td>
						<td>
                                                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" 
                                Width="236px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el nombre del caso" TabIndex="3" 
                                class="form-control input-sm"  />
                                        
                        </td>
						<td>
                            Ordenar: <asp:DropDownList ID="ddlTipo" runat="server" 
                                ToolTip="Seleccione el orden de la lista" TabIndex="6" class="form-control input-sm"  
                                AutoPostBack="True" onselectedindexchanged="ddlTipo_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">Desde el mas reciente</asp:ListItem>
                                <asp:ListItem Value="1">Desde el mas antiguo</asp:ListItem>
                                 <asp:ListItem Value="2">Desde el próximo vencimiento</asp:ListItem>
                            </asp:DropDownList>
                                        
                        </td>
						<td align="right">
                                            &nbsp;&nbsp;
                                                                    
                            </td>
						
					</tr>
					
						<tr>
						<td>
                            Codigo: 
                                        
                            </td>
						<td>
                            <asp:TextBox ID="txtNumero" runat="server" MaxLength="20" 
                                Width="136px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el numero de caso" TabIndex="3" 
                                class="form-control input-sm"  />
                                        
                            </td>
						<td>
                            Estado:&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlEstado" runat="server" 
                                class="form-control input-sm" TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="-1">Todos los activos</asp:ListItem>
                                                <asp:ListItem Value="0">No Procesado</asp:ListItem>
                                                <asp:ListItem Value="1">En Proceso</asp:ListItem>
                                                <asp:ListItem Value="2">Terminado</asp:ListItem>
                                              
                           <asp:ListItem Value="3">Anulados</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
						<td align="right">
                                            &nbsp;</td>
						
					</tr>
					
						<tr>
						<td>
                            DU Persona: </td>
						<td>
                            <asp:TextBox ID="txtDU" runat="server" MaxLength="20" 
                                Width="136px" style="text-transform :uppercase"  
                                ToolTip="Ingrese el numero de caso" TabIndex="3" 
                                class="form-control input-sm"  />
                                        
                            </td>
						<td>
                                    <asp:Label runat="server" ID="lblTipoForense">      Tipo:&nbsp;&nbsp;</asp:Label>  
                            <asp:DropDownList ID="ddlTipoCaso" runat="server" 
                                class="form-control input-sm" TabIndex="12" AutoPostBack="True" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="-1">Todos</asp:ListItem>
                                                <asp:ListItem Value="1">Filiación</asp:ListItem>
                                                <asp:ListItem Value="2">Pericia Forense</asp:ListItem>
                                   <asp:ListItem Value="3">Quimerismo</asp:ListItem>
                                            </asp:DropDownList>
                                                                    
                            </td>
						<td align="right">
                                            &nbsp;</td>
						
					</tr>
					
					

						<tr>
						<td colspan="2">

                            <table>
                                <tr>
                                    <td>   <span class="label label-danger">No procesado</span> </td>
                                    <td>  <span class="label label-warning">En proceso</span> </td>
                                    <td> <span class="label label-success">Terminado</span></td>
                                </tr>
                                <tr>
                                    <td colspan="3"><br /></td>
                                </tr>
                                 <tr>
                                    <td> <span class="label label-danger"> <asp:label id="lblNoProcesado"
                  
                  runat="server"/>	</span> </td>
                                    <td>   <span class="label label-warning">  <asp:label id="lblEnProceso"
                  
                  runat="server"/></span>	 </td>
                                    <td>   <span class="label label-success"><asp:label id="lblTerminado"
                  
                  runat="server"/>	</span></td>
                                </tr>
                            </table>
                            
                         
                            
                            
                          
                            
                            
                           </td>
						<td>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="100px"
                                                onclick="btnBuscar_Click" CssClass="btn btn-success"
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" Width="100px" Visible="false"
                                                onclick="btnNuevo_Click" CssClass="btn btn-success"
                                                ToolTip="Haga clic aquí para agregar un nuevo caso" />
                                        
                                            <asp:Button ID="btnHojaTrabajo" runat="server" Text="Hoja de Trabajo" Width="150px"
                                                onclick="btnHojaTrabajo_Click" CssClass="btn btn-primary"
                                                ToolTip="Haga clic aquí para generar la hoja de trabajo" />
                                        
                            </td>
						<td align="right">
                                            &nbsp;</td>
						
					</tr>
					
						</table>
					
					
					          </div>

			<div class="panel-footer">	
                      <asp:label id="CurrentPageLabel"
                  forecolor="Blue"
                  runat="server"/>	
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idCasoFiliacion" onrowcommand="gvLista_RowCommand1" 
            onrowdatabound="gvLista_RowDataBound" CssClass="table table-bordered bs-table" 
                                onselectedindexchanged="gvLista_SelectedIndexChanged" 
                                AllowPaging="True" onpageindexchanging="gvLista_PageIndexChanging" 
                                onselectedindexchanging="gvLista_SelectedIndexChanging" 
                                
                                GridLines="Horizontal" PageSize="20" 
                                EmptyDataText="No se encontraron datos para los filtros de busqueda ingresados" 
                                ToolTip="Lista de casos" ondatabound="gvLista_DataBound">
      <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
            <Columns>
     <%--     <asp:BoundField DataField="idCasoFiliacion" ItemStyle-HorizontalAlign="NotSet" ShowHeader="False">
                     
                <ItemStyle BorderStyle="Solid" Height="10px" HorizontalAlign="Center" VerticalAlign="Bottom" Width="20px" Wrap="False" />
                     
                </asp:BoundField>--%>

                   <asp:TemplateField HeaderText="Nro.">
                                                                        <ItemTemplate>
                                                                   <asp:Label ID="lblNumero" runat="server" 
    Text=' <%#: Eval("idCasoFiliacion") %>' >
</asp:Label>
                                                                             

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>
                <asp:BoundField DataField="nombre" HeaderText="Titulo" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                   <asp:BoundField DataField="tipo" HeaderText="Tipo" >
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha Registro" />

                   <asp:BoundField DataField="fechaVencimiento" HeaderText="Fecha Vencimiento" />
                    

                 <asp:TemplateField HeaderText="Modificar">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Editar" runat="server" Text="" Width="20px"  >
                                                                                  <span class="label label-primary">Editar</span>
                                           </asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Eliminar" Visible="false">
                                                                        <ItemTemplate>
                                                                               <asp:LinkButton ID="Eliminar" Visible="false" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                         <span class="label label-primary">    Eliminar</span></asp:LinkButton>
                                                                       
                                                                        </ItemTemplate>
                                                                       
                                                                       
                                                                    </asp:TemplateField>

                  <asp:TemplateField HeaderText="Resultado">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Resultados" runat="server" Text=""  Width="20px"  >
                                          <span class="label label-primary">   Informe</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>

                <asp:TemplateField HeaderText="Auditoria">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Auditoria" runat="server" Text="" Width="20px"  >
                                          <span class="label label-primary">   Auditoria</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                       
                                                                    </asp:TemplateField>

                <asp:TemplateField HeaderText="Carga Resultados">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Carga" runat="server" Text="" Width="20px"  >
                                        <span class="label label-primary">     Resultados</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                    
                                                                    </asp:TemplateField>

                <asp:TemplateField HeaderText="Validacion">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Valida" runat="server" Text="" Width="20px"  >
                                       <span class="label label-primary">      Validacion</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                    
                                                                    </asp:TemplateField>


                  <asp:TemplateField HeaderText="Caratula">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="Caratula" runat="server" Text="" Width="20px"  >
                                    <span class="label label-primary">         Caratula</span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>

                  <asp:TemplateField HeaderText="Facturacion">
                                                                        <ItemTemplate>
                                                                   
                                                                              <asp:LinkButton ID="PreFactura" CssClass="label label-danger" runat="server" Text="PreFactura" Visible="false" Width="100px"  >
                                    </asp:LinkButton>

                                                                        </ItemTemplate>
                                                                      
                                                                    </asp:TemplateField>

                        
            </Columns>
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="20" Position="TopAndBottom" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" 
                BorderColor="White"   />
                
               
      
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#339933" Font-Bold="False" ForeColor="White" 
                />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                <div>
					</div>	
                		 
                                            
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
