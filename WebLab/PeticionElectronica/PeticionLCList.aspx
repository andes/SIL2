<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeticionLCList.aspx.cs" Inherits="WebLab.PeticionElectronica.PeticionLCList" MasterPageFile="~/PeticionElectronica/SitePE.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

     <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
  <script type="text/javascript">


   $(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
		    maxDate: 0,
		    minDate: null,

			//showOn: 'button',
			//buttonImage: '../App_Themes/default/images/calend1.jpg',
			//buttonImageOnly: true
		});
	});

	$(function() {
	    $("#<%=txtFechaHasta.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,

			//showOn: 'button',
			//buttonImage: '../App_Themes/default/images/calend1.jpg',
			//buttonImageOnly: true
		});
	});
 
 
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   

    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">       
      <div  class="form-inline"  >

           <div class="panel panel-success">
                    <div class="panel-heading">
    <h3 class="panel-title">MIS PETICIONES</h3>
                        </div>

				<div class="panel-body">	
      
              <table >
					<tr>
						<td    ><label style:"width:150px">Fecha Desde:</label></td>
						<td  >
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0" class="form-control input-sm"
                                style="width: 90px"  /></td>
						<td   >
                       <label style:"width:150px">     Fecha Hasta:</label></td>
						<td  >
                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 90px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1" class="form-control input-sm" /></td>
						<td >
                                            &nbsp;</td>
						<td  >
                            &nbsp;</td>
					</tr>
					        
						
					<tr>
						<td  ><label style:"width:150px">Nro. de Petición:</label></td>
						<td  >
                            <asp:TextBox ID="txtNro" runat="server" onblur="valNumeroSinPunto(this)" class="form-control input-sm" MaxLength="9" TabIndex="2" Width="80px"></asp:TextBox>
                           
                         </td>
						<td   >
                            <label style:"width:150px">DNI:</label></td>
						<td  >
                             <input id="txtDni" type="text" runat="server"  class="form-control input-sm" 
                                onblur="valNumero(this)" tabindex="5" width="60px"/></td>
						<td >
                         <label style:"width:150px">                   Estado:</label></td>
						<td  >
                                            <asp:DropDownList ID="ddlEstado" runat="server" 
                                class="form-control input-sm" TabIndex="3" ToolTip="Seleccione estado de la petición" Width="150px" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="-1">Todas las activas</asp:ListItem>
                                                <asp:ListItem Value="0">Enviadas</asp:ListItem>
                                                <asp:ListItem Value="1">Recibidas</asp:ListItem>

                                                  
                                                <asp:ListItem Value="2">Con Resultados</asp:ListItem>
                                                <asp:ListItem Value="3">Anuladas</asp:ListItem>
                                            </asp:DropDownList>
                                        
                                     </td>
                            </tr>
                            
						
					<tr>
						<td  >
                            <label style:"width:150px">                Apellido/s:</label></td>
						<td colspan="2"  >
                                        <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm" TabIndex="5" 
                                                Width="200px"></asp:TextBox>
                                        
                                     </td>
						<td  >
                              <label style:"width:150px">              Nombres/s:</label></td>
						<td colspan="2"  >
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" TabIndex="6" 
                                                Width="200px"></asp:TextBox>
                                        
                                     </td>
                            </tr>
                            
						
					<tr>
						<td   colspan="5" > 
                            <asp:CustomValidator ID="cvNumero" runat="server" 
                                ErrorMessage="Numero de Peticion" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="0" 
                                >Numero de Peticion: Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                            <asp:CompareValidator ID="cvDni" 
                                 runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValidationGroup="0">DNI: Debe ingresar solo numeros</asp:CompareValidator>
                        </td>
						
						<td align="right">
                                                        </td>
						
					</tr>
					</table>
                    <div>
                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-success"
                                                           TabIndex="7" Text="Buscar" ValidationGroup="0" 
                                                                Width="120px" onclick="btnBuscar_Click" />
                    </div>
                    </div>
                 <div class="panel-footer" >
                     <div>
                            
                                                            <asp:Label ID="CurrentPageLabel" runat="server" forecolor="Blue" />
                     </div>
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                    CellPadding="2" DataKeyNames="idPeticion" ForeColor="#333333" EmptyDataText="No hay peticiones generadas para los filtros de busqueda ingresados." 
                    Width="100%" onrowcommand="gvLista_RowCommand"  onrowdatabound="gvLista_RowDataBound" BorderColor="#CCCCCC" 
                    CssClass="table table-bordered bs-table"  GridLines="Horizontal" Font-Names="Arial" 
                    Font-Size="8pt" TabIndex="8" OnPageIndexChanging="gvLista_PageIndexChanging" AllowPaging="True" BorderStyle="None" >
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <Columns>
                   

                            <asp:TemplateField HeaderText="Estado">
                                                                        <ItemTemplate>

                                                           <asp:Label ID="estado" Font-Size="10px"  class="label label-warning" Text='<%# Eval("estado") %>' runat="server"></asp:Label> 
                                                                            
                                                                        </ItemTemplate>
                                                                        <ItemStyle   HorizontalAlign="Center"  Width="5%" />
                                                                    </asp:TemplateField>
                        
                            <asp:BoundField DataField="idPeticion" HeaderText="Peticion" >
                                  <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="5%" Font-Bold="True" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="10%" />
                        </asp:BoundField>
                         
                        <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                            <ItemStyle Width="10%" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="apellido" HeaderText="Apellidos" >
                            <ItemStyle Font-Bold="true" Width="10%" VerticalAlign="Top" />
                        </asp:BoundField>
                        <asp:BoundField DataField="nombre" HeaderText="Nombres" >
                            <ItemStyle  Font-Bold="true" Width="10%" VerticalAlign="Top" />
                        </asp:BoundField>                          
                         

                          <asp:BoundField DataField="observaciones" HeaderText="Observaciones" >
                        <ItemStyle Width="10%" HorizontalAlign="Left" VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Top" Width="20%" />
                        </asp:BoundField>
                        
                        
          
                          <asp:BoundField DataField="usuario" HeaderText="Usuario" >
                            <ItemStyle Width="10%"  VerticalAlign="Top"/>
                        </asp:BoundField>
                         
                        
          
                  
                                                           
                       <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>

                                                                       <div class="btn-group">
  <asp:Button id="btnAnular" runat="server" class="btn btn-danger" Text="Anular" Enabled="true" OnClientClick="return PreguntoEliminar();"  />
  <asp:Button id="btnEditar" runat="server" class="btn btn-primary" Text="Editar" />
  <asp:Button id="btnResultados" runat="server" class="btn btn-success" Text="Resultados" Visible="false"/>
</div>
                                                                        </ItemTemplate>
                                                                    
                                                                    </asp:TemplateField>

                    </Columns>
                        <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerSettings Position="Top" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" CssClass="pagination-sm" VerticalAlign="Top" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
                </asp:GridView>
               </div>
            
               </div> 
          </div>
    <script language="javascript" type="text/javascript">

                 function PreguntoEliminar() {
                     if (confirm('¿Está seguro de eliminar el pedido?'))
                         return true;
                     else
                         return false;
                 }
    </script>
  
    
</asp:Content>