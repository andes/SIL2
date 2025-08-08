<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemEdit2.aspx.cs" Inherits="WebLab.Items.ItemEdit2" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
--%>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
 

     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 


    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  

    <link rel="stylesheet" type="text/css" href ="../script/moverfilas/moverfilas.css" />
<script type="text/javascript" src="../script/moverfilas/codigo.js"></script>
<script type="text/javascript">
  $(function() {

                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                      
                        $("#tabContainer").tabs({ selected: currTab });
             });
</script>
   
  
   
    <style type="text/css">
        .auto-style1 {
            width: 258px;
        }
    </style>
   
  
   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
   

    
<asp:HiddenField runat="server" ID="HFCurrTabIndex"   /> 
 <div align="left" style="width: 1200px" class="form-inline"  >
  
<table align="center" width="1000px">

<tr>
<td colspan="4" > 
       <div class="panel panel-default">
              <div class="panel-heading">
                 <h4>ANALISIS
                 <small> Configuración <asp:Label ID="txtEfector" runat="server" ></asp:Label>
                     </small></h4> 
                  </div>
           				<div class="panel-body">	
		  <div id="tabContainer" style="border: 0px solid #C0C0C0">
 <ul class="myLabel">
    <li><a href="#tab1">Datos Generales</a></li>      
      <li><a href="#tab7">Presentaciones</a></li> 
    <li><a href="#tab2">Valores de Referencia</a></li>
  
    <li><a href="#tab3">Diagrama</a></li>
    <li><a href="#tab4">Resultados Predefinidos</a></li>
    <li><a href="#tab5">Recomendaciones</a></li>
     <li><a href="#tab8"  >Muestras</a></li>
 <li><a href="#tab6">Mas opciones</a></li>
     <li><a href="#tab9">Turnos</a></li>
     <li><a href="#tab10">AutoAnalizadores</a></li>
</ul>

    <div id="tab1" class="tab_content" style="border: 1px solid #C0C0C0">
				 <table  align="center"                    >
					
					
					<tr>
						<td class="control-label"  >Codigo:<asp:RequiredFieldValidator 
                                ID="rfvCodigo" runat="server" ControlToValidate="txtCodigo" 
                                ErrorMessage="Codigo" ValidationGroup="0">*</asp:RequiredFieldValidator>
                                            </td>
						<td>
                            <anthem:TextBox ID="txtCodigo" runat="server" MaxLength="100" 
                                Width="102px" style="text-transform:uppercase"  
                                ToolTip="Ingrese el codigo del analisis" 
                                class="form-control input-sm"  ontextchanged="txtCodigo_TextChanged" 
                                AutoCallBack="True" TabIndex="1" />&nbsp;&nbsp;<small>sin puntos ni comas   </small>
                            <anthem:Label ID="lblMensajeCodigo" runat="server" Font-Bold="True" 
                                ForeColor="#CC3300" Visible="False" TabIndex="100">El código ya existe. Verifique.</anthem:Label>
                                            </td>
					</tr>
					<tr>
						<td class="control-label"  >Nombre corto:<asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                ControlToValidate="txtNombre" ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator>
                                        
                        </td>
						<td>
                                            <anthem:TextBox ID="txtNombre" runat="server" 
                                AutoCallBack="True" class="form-control input-sm"  ontextchanged="txtNombre_TextChanged" Width="200px" 
                                                TabIndex="2" ToolTip="Ingrese el nombre corto del analisis"></anthem:TextBox>
                                            &nbsp;</td>
					</tr>
					<tr>
						<td class="control-label"  >Descripción (a imprimir):<asp:RequiredFieldValidator ID="rfvDescripcion" runat="server" 
                                ControlToValidate="txtDescripcion" ErrorMessage="Descripcion" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                                            </td>
						<td>
                            <anthem:TextBox ID="txtDescripcion" runat="server" class="form-control input-sm" 
                                Width="393px" TabIndex="3" 
                                ToolTip="Ingrese la descripcion a imprimir del analisis"></anthem:TextBox>
                                            </td>
						</tr>
					<tr>
							<td class="control-label"  >Servicio / Area:<asp:RangeValidator ID="rvArea" runat="server" ControlToValidate="ddlArea" 
                                ErrorMessage="Area" MaximumValue="999999" MinimumValue="1" Type="Integer" 
                                ValidationGroup="0">*</asp:RangeValidator>
                                        
					                        </td>
						<td>
                            <anthem:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="4" class="form-control input-sm" 
                                AutoCallBack="True" 
                                onselectedindexchanged="ddlServicio_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                            <anthem:DropDownList ID="ddlArea" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="5" class="form-control input-sm" >
                            </anthem:DropDownList>
                                        
					</tr>
					<tr>
							<td class="control-label"  >Unidad de Medida:</td>
						<td>
                            <asp:DropDownList ID="ddlUnidadMedida" runat="server" 
                                ToolTip="Seleccione la unidad de medida" TabIndex="6" class="form-control input-sm" >
                            </asp:DropDownList>
                                        
					</tr>
						<tr>
							<td class="control-label"  >
                                          </td>
						<td>
                            <asp:RadioButtonList ID="rdbRequiereMuestra" runat="server" 
                                RepeatDirection="Horizontal" TabIndex="7" 
                                ToolTip="Seleccione si el analisis requiere o no muestra" Visible="False">
                                <asp:ListItem Value="S">Si</asp:ListItem>
                                <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
						
					</tr>
					<tr>
							<td class="control-label"  >
                                            Derivable:</td>
						<td>
                            <anthem:DropDownList ID="ddlDerivable" runat="server" AutoCallBack="True" 
                                onselectedindexchanged="ddlDerivable_SelectedIndexChanged" 
                                class="form-control input-sm" TabIndex="8" 
                                ToolTip="Seleccione si el analisis es derivable">
                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                <asp:ListItem Value="1">Si</asp:ListItem>
                            </anthem:DropDownList>
                            <anthem:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" 
                                TabIndex="9" ToolTip="Seleccione el efector a derivar">
                            </anthem:DropDownList>
                            <anthem:RangeValidator ID="rvEfector" runat="server" 
                                                ControlToValidate="ddlEfector" Enabled="False" 
                                                ErrorMessage="Efector Derivación" MaximumValue="999999" MinimumValue="1" 
                                                Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                        </td>
						
					</tr>
					<tr>
							<td class="control-label"  >
                                            Analisis de Referencia:</td>
						<td>
                            <anthem:TextBox 
                                ID="txtCodigoReferencia" runat="server" AutoCallBack="True" ontextchanged="txtCodigoReferencia_TextChanged" 
                                                TextDuringCallBack="Cargando..." Width="61px" 
                                class="form-control input-sm" TabIndex="10"></anthem:TextBox>  				                             
                            <anthem:DropDownList ID="ddlItemReferencia" runat="server" 
                                ToolTip="Seleccione el analisis de referencia" TabIndex="11" 
                                class="form-control input-sm" AutoCallBack="True" 
                                onselectedindexchanged="ddlItemReferencia_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                            <br />
                        </td>
						
					</tr>
					<tr>
							<td class="control-label"  >
                                            Duración (dias):<asp:RequiredFieldValidator ID="rfvDuracion" runat="server" 
                                                ControlToValidate="txtDuracion" ErrorMessage="Duracion" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						<td>
                            <input id="txtDuracion"  onblur="valNumero(this)" runat="server" type="text" 
                                class="form-control input-sm" size="10" tabindex="12" value="1" /></td>
						
					</tr>
					<tr>
						<td class="control-label"  >
                                            Tipo de analisis:<asp:RequiredFieldValidator ID="rfvItem" runat="server" 
                                                ControlToValidate="rdbTipo" ErrorMessage="Tipo Item" ValidationGroup="0">*</asp:RequiredFieldValidator>
                                            </td>
						<td>
                            <asp:RadioButtonList ID="rdbTipo" runat="server" 
                                RepeatDirection="Horizontal" TabIndex="13" 
                                ToolTip="Seleccione el tipo de analisis">
                                <asp:ListItem Value="1">Práctica (Analisis Nomenclado)</asp:ListItem>
                                <asp:ListItem Value="0">Determinación (Analisis no nomenclado)</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
						
					</tr>
					<tr>
							<td class="control-label"  >
                                            Categoría:<anthem:RequiredFieldValidator ID="rfvCategoria" runat="server" 
                                                ControlToValidate="rdbCategoria" ErrorMessage="Categoría" ValidationGroup="0">*</anthem:RequiredFieldValidator>
                                            </td>
						<td>
                            <anthem:RadioButtonList ID="rdbCategoria" runat="server" 
                                RepeatDirection="Horizontal" AutoCallBack="True" 
                                onselectedindexchanged="rdbCategoria_SelectedIndexChanged" TabIndex="14">
                                <Items>
                                    <asp:ListItem Value="0">Simple</asp:ListItem>
                                    <asp:ListItem Value="1">Compuesta</asp:ListItem>
                                </Items>
                            </anthem:RadioButtonList>
                        </td>
						
					</tr>
					<tr>
						<td colspan="2"  >
                                          <hr /></td>
						
					</tr>
              
					<tr>
						<td class="control-label"  >
                                            Tipo de Muestra:</td>
						<td>
                            <anthem:DropDownList ID="ddlTipoMuestra" runat="server" class="form-control input-sm" TabIndex="15" 
                                ToolTip="Seleccione el tipo de resultado que genera el analisis" 
                                onselectedindexchanged="ddlTipoResultado_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">
                                Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">Numérico</asp:ListItem>
                                <asp:ListItem Value="2">Texto</asp:ListItem>
                                <asp:ListItem Value="3">Predefenidos (Selección simple)</asp:ListItem>
                                <asp:ListItem Value="4">Predefenidos (Selección múltiple)</asp:ListItem>
                                <asp:ListItem Value="5">Tabla Cruzada Fusion</asp:ListItem>
                            </anthem:DropDownList>

    <asp:CheckBox ID="chkImprimeMuestra" runat="server" CssClass="control-label" 
        Text="Imprime Muestra Etiqueta" />
                        </td>
						
					</tr>
              
					<tr>
						<td class="control-label" colspan="2"  >
                                          <hr /></td>
						
					</tr>
              
					<tr>
						<td class="control-label"  >
                                            Tipo de Resultado:<anthem:RangeValidator ID="rvTipoResultado" runat="server" 
                                                ControlToValidate="ddlTipoResultado" Enabled="False" 
                                                ErrorMessage="Tipo de Resultado" MaximumValue="999999" MinimumValue="1" 
                                                Type="Integer" ValidationGroup="0">*</anthem:RangeValidator>
                        </td>
						<td>
                            <anthem:DropDownList ID="ddlTipoResultado" runat="server" class="form-control input-sm" 
                                Enabled="False" TabIndex="15" 
                                ToolTip="Seleccione el tipo de resultado que genera el analisis" 
                                AutoCallBack="True" 
                                onselectedindexchanged="ddlTipoResultado_SelectedIndexChanged">
                                <asp:ListItem Value="0" Selected="True">
                                Seleccione</asp:ListItem>
                                <asp:ListItem Value="1">Numérico</asp:ListItem>
                                <asp:ListItem Value="2">Texto</asp:ListItem>
                                <asp:ListItem Value="3">Predefenidos (Selección simple)</asp:ListItem>
                                <asp:ListItem Value="4">Predefenidos (Selección múltiple)</asp:ListItem>
                                <asp:ListItem Value="5">Tabla Cruzada Fusion</asp:ListItem>
                            </anthem:DropDownList>
                        </td>
						
					</tr>
					<tr>
	<td class="control-label"  >
                                            Formato Decimal:</td>
						<td>
                            <anthem:DropDownList ID="ddlDecimales" runat="server" class="form-control input-sm" 
                                Enabled="False" TabIndex="15" 
                                ToolTip="Seleccione el tipo de resultado que genera el analisis" 
                                AutoCallBack="True" onselectedindexchanged="ddlDecimales_SelectedIndexChanged">
                                <asp:ListItem Value="0">1</asp:ListItem>
                                <asp:ListItem Value="1">1.0</asp:ListItem>
                                <asp:ListItem Value="2" Selected="True">1.00</asp:ListItem>
                                <asp:ListItem Value="3">1.000</asp:ListItem>
                                <asp:ListItem Value="4">1.0000</asp:ListItem>
                            </anthem:DropDownList>
                        </td>
						
					</tr>
					<tr>
	<td class="control-label"  >
                                            Multiplicador:</td>
						<td>
                            <anthem:DropDownList ID="ddlMultiplicador" runat="server" class="form-control input-sm" 
                                Enabled="False" TabIndex="15" 
                                ToolTip="Seleccione el multiplicador a aplicar">
                                <asp:ListItem Value="1" Selected="True">1</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="100">100</asp:ListItem>
                                <asp:ListItem Value="1000">1000</asp:ListItem>
                                <asp:ListItem Value="10000">10000</asp:ListItem>
                                <asp:ListItem>100000</asp:ListItem>
                                <asp:ListItem>1000000</asp:ListItem>
                            </anthem:DropDownList>
                        </td>
						
					</tr>
					<tr>
	<td class="control-label"  >
                                            Valor Crítico Mínimo:</td>
						<td>
                            <anthem:TextBox ID="txtValorMinimo" runat="server" class="form-control input-sm" 
                                Width="76px"></anthem:TextBox> &nbsp;
                            <anthem:RegularExpressionValidator ID="revValorMinimo" runat="server" 
                                ControlToValidate="txtValorMinimo" ErrorMessage="Formato incorrecto" 
                                ValidationGroup="0"></anthem:RegularExpressionValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="control-label"  >
                                            Valor Crítico Máximo:</td>
						<td>
                            <anthem:TextBox ID="txtValorMaximo" runat="server" class="form-control input-sm" 
                                Width="76px"></anthem:TextBox> &nbsp;
                            <anthem:RegularExpressionValidator ID="revValorMaximo" runat="server" 
                                ControlToValidate="txtValorMaximo" ErrorMessage="Formato incorrecto" 
                                ValidationGroup="0"></anthem:RegularExpressionValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="control-label"  >
                                            Valor por Defecto:</td>
						<td>
                            <anthem:TextBox ID="txtValorDefecto" runat="server" class="form-control input-sm" 
                                Width="76px"></anthem:TextBox> &nbsp;
                            <anthem:RegularExpressionValidator ID="revValorDefecto" runat="server" 
                                ControlToValidate="txtValorDefecto" ErrorMessage="Formato incorrecto" 
                                ValidationGroup="0"></anthem:RegularExpressionValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="control-label" colspan="2"  >
                                            <hr /></td>
						
					</tr>
					<tr>
						<td class="control-label"  >
                                            Disponible (activo):</td>
						<td>
                                            
                                            <asp:DropDownList ID="ddlDisponible" runat="server"  class="form-control input-sm" >
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                <asp:ListItem Value="1" Selected="True">Si</asp:ListItem>
                                            </asp:DropDownList>
                        </td>
						
					</tr>
                     <tr>
						<td class="control-label"  >
                                            Estado actual:</td>
						<td>
                                            
                            <asp:Label ID="lblSininsumo" runat="server" Text="Label"  Font-Size="12"></asp:Label>
                            <small>Para cambiar el estado acceder a Auditoria de Insumos</small>
                        </td>
						
					</tr>
					<tr>
						<td class="control-label"  >
                                            Informable:</td>
						<td>
                                            
                                            <asp:DropDownList ID="ddlInformable" runat="server" class="form-control input-sm">
                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                        </td>
						
					</tr>
                     <tr>
						<td class="control-label"  >
                                            Requiere Caracter:</td>
						<td>
                                            
                                            <asp:DropDownList ID="ddlCaracter" runat="server" class="form-control input-sm">
                                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                                <asp:ListItem  Value="1">Si</asp:ListItem>
                                            </asp:DropDownList>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="2">
                            <hr /></td>
						
					</tr>
					<tr>
						<td colspan="2"  >
                                             <table class="myTabla" style="border-color: #008080; width: 100%;">
                                <tr>
                                    <td colspan="2" 
                                        style="background-color: #008080; color: #FFFFFF; font-weight: bold;">
                                       
                                        NOMENCLADOR UNIFICADO</td>
                                </tr>
                              	<tr>
						<td class="control-label"  >	   CAPITULO:				</td>
						<td >		  
                            <anthem:TextBox 
                                ID="txtCodigoNomenclador" runat="server" AutoCallBack="True" ontextchanged="txtCodigo_TextChanged1" 
                                                TextDuringCallBack="Cargando..." Width="95px" 
                                class="form-control input-sm" TabIndex="18"></anthem:TextBox>  				                             &nbsp; &nbsp;                                        
                            <anthem:Label ID="lblMensaje" runat="server" 
                                Text="El codigo no existe en el nomenclador" Font-Bold="False" 
                                            ForeColor="Red" Visible="False"></anthem:Label>
                            
                            </td>
					
						
						</tr>
						<tr>
						<td class="control-label"  >    Nombre:
						</td>
						<td>   
                            <anthem:DropDownList ID="ddlItemNomenclador" runat="server" AutoCallBack="True" 
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" Width="400px" 
                                class="form-control input-sm" TabIndex="19" 
                                ToolTip="Seleccione el analisis correspondiente al nomenclador">
                                            </anthem:DropDownList></td>
						
						</tr>
                                <tr>
                                    <td class="control-label" >
                                        U.B:</td>
                                    <td>
                                        <anthem:Label ID="lblValorNomenclador" runat="server" Text="" Font-Bold="True" 
                                            ForeColor="#333333"></anthem:Label>
                                    </td>
                                </tr>
                                	</tr>
                            <%--    <tr>
                                    <td class="control-label" >
                                        Factor Producción:</td>
                                    <td>
                                        <anthem:Label ID="lblFactorProduccion" runat="server" Text="" Font-Bold="True" 
                                            ForeColor="#333333"></anthem:Label> 
                                    </td>
                                </tr>--%>
                            </table>
        
        <%--</div>

              </div>
                               </div>
           </div>--%>
        </td>
						
					</tr>
					<tr>
						<td colspan="2"  >
                                            <hr /></td>
						
					</tr>
					<tr>
                        <td align="left" >
					 <asp:Button ID="btnAuditoria" runat="server" Text="Auditoria"  
                                               CssClass="btn btn-success" TabIndex="20"  Width="100px"
                                                ToolTip="Hacer clic aqui para ver la auditoria de acciones" OnClick="btnAuditoria_Click" />
                               </td>
						<td align="right" >
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" 
                                                onclick="btnGuardar_Click" CssClass="btn btn-primary" TabIndex="20"  Width="100px"
                                                ToolTip="Hacer clic aqui para guardar el analisis" />
                        </td>
						
					</tr>
					
				
					</table>
			
					
					</div>	

    				<div id="tab7" class="tab_content" style="border: 1px solid #C0C0C0">
                        <table>
                            <tr>
                                <td>Marca:</td>
                                <td><anthem:DropDownList ID="ddlMarca" class="form-control input-sm"  runat="server" Width="200px"></anthem:DropDownList>    
                            <asp:RangeValidator ID="rvMarca" runat="server" ControlToValidate="ddlMarca" 
                                ErrorMessage="Marca Presentacion" MaximumValue="999999" MinimumValue="1" 
                                Type="Integer" ValidationGroup="6">*</asp:RangeValidator>
                                        
                                </td>
                            </tr>
                            <tr>
                                <td>Codigo:</td>
                                <td>   <anthem:TextBox ID="txtCodigoPresentacion" runat="server" 
                                class="form-control input-sm"   Width="150px" 
                                                TabIndex="2" ToolTip="Ingrese la presentacion de la marca"></anthem:TextBox><asp:RequiredFieldValidator ID="rfvCodigoPresentacion" runat="server" 
                                ControlToValidate="txtCodigoPresentacion" ErrorMessage="Codigo Presentacion" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        
                                </td>
                            </tr>
                            <tr>
                                <td>Presentacion:</td>
                                <td>   <anthem:TextBox ID="txtPresentacion" runat="server" 
                                class="form-control input-sm"   Width="400px" 
                                                TabIndex="2" ToolTip="Ingrese la presentacion de la marca"></anthem:TextBox><asp:RequiredFieldValidator ID="rfvPresentacion" runat="server" 
                                ControlToValidate="txtPresentacion" ErrorMessage="Nombre Presentacion" ValidationGroup="6">*</asp:RequiredFieldValidator>
                                        
                                </td>
                            </tr>
                              <tr>
                            <td align="right" colspan="2">
                                <anthem:Button ID="btnAgregarPresentacion" runat="server" CssClass="btn btn-primary"  TabIndex="9" Text="Agregar a Lista" ValidationGroup="6" Width="150px" OnClick="btnAgregarPresentacion_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h4>Presentaciones
                                </h4> 
                                <anthem:Label ID="lblMensagePresentacion" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></anthem:Label>
                                <anthem:GridView ID="gdPresentacion" runat="server" AutoGenerateColumns="False" 
                                    AutoUpdateAfterCallBack="True" BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" 
                                    CssClass="table table-bordered bs-table" DataKeyNames="iditempresentacion" 
                                    EmptyDataText="No hay presentaciones cargados para la determinación" 
                                    Font-Size="10pt" ForeColor="#333333" GridLines="Horizontal" 
                                   
                                    TabIndex="9" Width="100%" OnRowCommand="gdPresentacion_RowCommand" OnRowDataBound="gdPresentacion_RowDataBound" OnSelectedIndexChanged="gdPresentacion_SelectedIndexChanged" UpdateAfterCallBack="True">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:BoundField DataField="marca" HeaderText="Marca">                                        
                                        </asp:BoundField>
                                        <asp:BoundField DataField="codigo" HeaderText="Codigo" />
                                        <asp:BoundField DataField="presentacion" HeaderText="Presentacion">                                        
                                        </asp:BoundField>
                                        
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" ImageUrl="~/App_Themes/default/images/eliminar.jpg" OnClientClick="return PreguntoEliminar();" />
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
                                </anthem:GridView>
                            </td>
                        </tr>
                        </table>
                        </div>


    		<div id="tab10" class="tab_content" style="border: 1px solid #C0C0C0">
                        <table>
                            
                        <tr>
                            <td colspan="2">
                                <h4>Autoanalizadores
                                </h4> 
                                
                                <anthem:GridView ID="gvAutoAnalizadores" runat="server" AutoGenerateColumns="False" 
                                    AutoUpdateAfterCallBack="True" BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" 
                                    CssClass="table table-bordered bs-table" DataKeyNames="idEquipo" 
                                    EmptyDataText="No hay datos relacionaos para la determinación" 
                                    Font-Size="10pt" ForeColor="#333333" GridLines="Horizontal" 
                                   
                                    TabIndex="9" Width="100%"  >
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:BoundField DataField="Equipo" HeaderText="Equipo">                                        
                                        </asp:BoundField>
                                          <asp:BoundField DataField="Marca" HeaderText="Marca">                                        
                                        </asp:BoundField>
                                          <asp:BoundField DataField="Presentacion" HeaderText="Presentacion">                                        
                                        </asp:BoundField>
                                       
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </anthem:GridView>
                            </td>
                        </tr>
                        </table>
                        </div>
    
    				<div id="tab8"   class="tab_content" style="border: 1px solid #C0C0C0">
                           <asp:Label ID="lblMensajeMuestra1" runat="server"    ForeColor="Red">Sección habilitada para Servicio Microbiologia</asp:Label>
                        <table>
                            <tr>
                                <td>Muestra:</td>
                                <td><anthem:DropDownList ID="ddlMuestra1" class="form-control input-sm"  runat="server" Width="400px"></anthem:DropDownList>    
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlMuestra1" 
                                ErrorMessage="Muestra:" MaximumValue="999999" MinimumValue="1" 
                                Type="Integer" ValidationGroup="18">*</asp:RangeValidator>
                                        
                                </td>
                            </tr>
                              <tr>
                            <td align="right" colspan="2">
                                <anthem:Button ID="btnAgregarMuestra" runat="server" CssClass="btn btn-primary" Text="Agregar a Lista" ValidationGroup="18" Width="150px" OnClick="btnAgregarMuestra_Click"  />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <h4>Muestras Asociadas
                                <anthem:Label ID="lblMensajeMuestra" runat="server" Visible="False" Font-Bold="True" ForeColor="Red"></anthem:Label>
                                </h4> 
                                <anthem:GridView ID="gvMuestraItem" runat="server" AutoGenerateColumns="False" 
                                    AutoUpdateAfterCallBack="True" BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" 
                                    CssClass="table table-bordered bs-table" DataKeyNames="iditemMuestra" 
                                    EmptyDataText="No hay muestras cargados para la determinación" 
                                    Font-Size="10pt" ForeColor="#333333" GridLines="Horizontal" 
                                   
                                    TabIndex="9" Width="100%"  
                                       UpdateAfterCallBack="True" OnRowDataBound="gvMuestraItem_RowDataBound" OnRowCommand="gvMuestraItem_RowCommand">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>                                        
                                        <asp:BoundField DataField="muestra" HeaderText="Muestra">                                        
                                        </asp:BoundField>
                                        
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" ImageUrl="~/App_Themes/default/images/eliminar.jpg" OnClientClick="return PreguntoEliminar();" />
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
                                </anthem:GridView>
                            </td>
                        </tr>
                        </table>
                        </div>
					<div id="tab2" class="tab_content" style="border: 1px solid #C0C0C0">
					<anthem:Panel ID="pnlVR" runat="server">
					    
                        	<anthem:Panel ID="pnlNuevoVR" runat="server">
					 <table  width="800px"   >
                         
					
 					<tr>
						<td class="control-label" >Analisis:</td>
						<td colspan="3">
                         <asp:Label ID="lblItemVR" runat="server" Font-Bold="False" 
                        ></asp:Label>
                                            </td>
					</tr>
					<tr>
						<td class="control-label" >Alcance:</td>
						<td colspan="3">
                            <anthem:DropDownList ID="ddlTipoServicio" runat="server" AutoCallBack="True" class="form-control input-sm" onselectedindexchanged="ddlTipoServicio_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el sexo">
                                <asp:ListItem Selected="True" Value="P">Paciente</asp:ListItem>
                                <asp:ListItem Value="NP">No Paciente</asp:ListItem>
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr> 
					     <tr>
                             <td class="control-label" colspan="4"><hr /></td>
                         </tr>
					     <tr>
                             <td class="control-label">Sexo:</td>
                             <td colspan="3">
                                 <anthem:DropDownList ID="ddlSexo" runat="server" AutoCallBack="True" class="form-control input-sm" onselectedindexchanged="ddlSexo_SelectedIndexChanged" TabIndex="1" ToolTip="Seleccione el sexo">
                                     <asp:ListItem Selected="True" Value="I">Ambos Sexos</asp:ListItem>
                                     <asp:ListItem Value="F">Femenino</asp:ListItem>
                                     <asp:ListItem Value="M">Masculino</asp:ListItem>
                                     <asp:ListItem Value="E">Embarazada</asp:ListItem>
                                 </anthem:DropDownList>
                             </td>
                         </tr>
					<tr>
						<td class="control-label" >Edad Desde:<anthem:RequiredFieldValidator 
                                ID="rfvEdadDesde" runat="server" 
                                ControlToValidate="txtEdadDesde" ErrorMessage="Edad Desde" 
                                ValidationGroup="1">*</anthem:RequiredFieldValidator>
                                        
                        </td>
						<td  >
                            <input id="txtEdadDesde" name="txtEdadDesde" type="text" runat="server" 
                                onblur="valNumeroSinPunto(this)" class="form-control input-sm" style="width: 80px" 
                                tabindex="2" /></td>
					 
						<td class="control-label" >Edad Hasta:<anthem:RequiredFieldValidator 
                                ID="rfvEdadHasta" runat="server" 
                                ControlToValidate="txtEdadHasta" ErrorMessage="Edad Hasta" 
                                ValidationGroup="1">*</anthem:RequiredFieldValidator>
                                            </td>
						<td  >
                            <input id="txtEdadHasta" type="text" runat="server" onblur="valNumeroSinPunto(this)" 
                                class="form-control input-sm" style="width: 80px" tabindex="3" /> </td>
						</tr>
					     <tr>
                             <td class="control-label">
                                 Unidad Edad:</td>
                             <td colspan="3">
                                 <anthem:DropDownList ID="ddlUnidadEdad" runat="server" class="form-control input-sm" 
                                     TabIndex="4" ToolTip="Seleccione el método">
                                     <asp:ListItem Selected="True" Value="0">Años</asp:ListItem>
                                     <asp:ListItem Value="1">Meses</asp:ListItem>
                                     <asp:ListItem Value="2">Días</asp:ListItem>
                                 </anthem:DropDownList>
                             </td>
                         </tr>
					 
						<tr>
                            <td class="control-label">Método:</td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlMetodo" runat="server" class="form-control input-sm" TabIndex="4" ToolTip="Seleccione el método">
                                </asp:DropDownList>
                                <asp:Label ID="lblDecimales" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td >Rango de valores:</td>
                            <td colspan="3">
                                <anthem:RadioButtonList ID="rdbRango" runat="server" AutoCallBack="True" onselectedindexchanged="rdbRango_SelectedIndexChanged" RepeatDirection="Vertical" TabIndex="5" RepeatColumns="2">
                                    <Items>
                                        <asp:ListItem Selected="True" Value="0">Rango
                                    </asp:ListItem>
                                        <asp:ListItem Value="1">Sólo Limite Inferior (Mayor a ..)</asp:ListItem>
                                        <asp:ListItem Value="2">Sólo Limite Superior (Hasta ..)</asp:ListItem>
                                        <asp:ListItem Value="3">Sin Rango (con observación)</asp:ListItem>
                                    </Items>
                                </anthem:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="control-label">Valor Mínimo:</td>
                            <td  >
                                <anthem:TextBox ID="txtValorMinimoVR" runat="server" class="form-control input-sm" MaxLength="6" TabIndex="6" Width="60px"></anthem:TextBox>
                                &nbsp;
                                <anthem:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtValorMinimoVR" ErrorMessage="Ingresar como valor minimo solo numeros" ValidationExpression="^[0-9]+(\.[0-9]+)?$" ValidationGroup="1">Solo numeros</anthem:RegularExpressionValidator>
                            </td>
                             <td class="control-label">Valor Máximo:</td>
                            <td>
                                <anthem:TextBox ID="txtValorMaximoVR" runat="server" class="form-control input-sm" MaxLength="6" TabIndex="7" Width="60px"></anthem:TextBox>
                                &nbsp;
                                <anthem:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtValorMaximoVR" ErrorMessage="Ingresar como valor maximo solo numeros" ValidationExpression="^[0-9]+(\.[0-9]+)?$" ValidationGroup="1">Sólo numeros</anthem:RegularExpressionValidator>
                                <br />
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="control-label" style="vertical-align: top">Observaciones:</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control input-sm" MaxLength="300" Rows="3" TabIndex="8" TextMode="MultiLine" Width="500px" />
                                <br />
                                <anthem:CustomValidator ID="cvObservaciones" runat="server" ClientValidationFunction="VerificaLargo" ControlToValidate="txtObservaciones" ErrorMessage="El límite del campo Observaciones es de 4000 caracteres. Verifique." Font-Names="Arial" Font-Size="8pt" ValidationGroup="1">El límite es de 4000 caracteres. Verifique.</anthem:CustomValidator>
                                <anthem:CustomValidator ID="cvValores" runat="server" ErrorMessage="Debe ingresar los valores (minimo y maximo) o una observación" Font-Size="8pt" onservervalidate="cvValores_ServerValidate" ValidateEmptyText="True" ValidationGroup="1"></anthem:CustomValidator>
                            </td>
                        </tr>

                         <tr>
                                <td>Presentacion:</td>
                                <td colspan="2"><anthem:DropDownList class="form-control input-sm"  ID="ddlPresentacionItem" Width="300px" runat="server"></anthem:DropDownList></td>
                            </tr>
                        <tr>
                            <td align="right" colspan="3">
                                <anthem:Button ID="btGuardarVR" runat="server" CssClass="btn btn-primary" onclick="btnGuardarVR_Click" TabIndex="9" Text="Agregar a Lista" ValidationGroup="1" Width="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>

                         </table>
</anthem:Panel>
                        <anthem:Panel ID="PnlVREfector" runat="server" Visible="false">
                            <table>
                                <tr>
                                <td>Presentación:</td>
                                <td colspan="3"><anthem:DropDownList class="form-control input-sm"  ID="ddlPresentacionEfector" Width="400px" runat="server"></anthem:DropDownList></td>
                                <td>  <anthem:Button ID="btnAgregarVREfector" runat="server" CssClass="btn btn-primary"   TabIndex="9" Text="Agregar a Lista" ValidationGroup="1" Width="150px" OnClick="btnAgregarVREfector_Click" />
                            </td>
                            </tr>
                                <tr>
                                <td>Presentación por Defecto:</td>
                                <td colspan="3"><anthem:DropDownList class="form-control input-sm"  ID="ddlPresentacionEfectorDefecto" Width="400px" runat="server"></anthem:DropDownList></td>
                                <td>  <anthem:Button ID="btnPresentacionDefecto" runat="server" CssClass="btn btn-primary"   TabIndex="9" Text="Guardar" ValidationGroup="1" Width="150px" OnClick="btnPresentacionDefecto_Click"  />
                            </td>
                            </tr>
                            </table>
                        </anthem:Panel>
                        
                        <table>

                        <tr>
                            <td colspan="4">
                                <h4>V.R de Pacientes
                                </h4> 
                                <anthem:GridView ID="gvListaVR" runat="server" AutoGenerateColumns="False" AutoUpdateAfterCallBack="True" BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" CssClass="table table-bordered bs-table" DataKeyNames="idValorReferencia" EmptyDataText="No hay valores de referencia cargados para la determinación" Font-Size="10pt" ForeColor="#333333" GridLines="Horizontal" onrowcommand="gvListaVR_RowCommand1" onrowdatabound="gvListaVR_RowDataBound" TabIndex="9" Width="100%" UpdateAfterCallBack="True">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:BoundField DataField="sexo" HeaderText="Sexo">
                                        <ItemStyle HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="edad" HeaderText="Edad">
                                        <ItemStyle HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="unidadEdad" HeaderText="Un.Edad">
                                        <ItemStyle HorizontalAlign="Center"  />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="metodo" HeaderText="Metodo">
                                        <ItemStyle />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="minimo" HeaderText="Valor Mínimo">
                                       
                                        </asp:BoundField>
                                        <asp:BoundField DataField="maximo" HeaderText="Valor Maximo">
                                       
                                        </asp:BoundField>
                                        <asp:BoundField DataField="observacion" HeaderText="Observaciones">
                                        
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Eliminar" runat="server" CommandName="Eliminar" ImageUrl="~/App_Themes/default/images/eliminar.jpg" OnClientClick="return PreguntoEliminar();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="presentacion" HeaderText="Presentación" />
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </anthem:GridView>
                            </td>
                        </tr>
                         <tr>
                            <td colspan="4"><small> Tener en cuenta que no debe haber superposición de edades en la definición de los VR para una misma presentación. </small></td>
                        </tr>
                         <tr>
                            <td align="right" colspan="4">
                                <anthem:Label ID="lblMensaje3" runat="server" Font-Bold="True" ForeColor="Red" Text="Label" Visible="False"></anthem:Label>
                                <anthem:Button ID="btnAplicarEfectores" runat="server" CssClass="btn btn-primary" OnClick="btnAplicarEfectores_Click" Text="Aplicar a todos los Efectores" Visible="true" Width="280px" AutoUpdateAfterCallBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                 <h4>V.R de No Pacientes</h4> 
                                <anthem:GridView ID="gvListaVRNP" runat="server" AutoGenerateColumns="False" AutoUpdateAfterCallBack="True" BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" CssClass="table table-bordered bs-table" DataKeyNames="idValorReferencia" EmptyDataText="No hay valores de referencia cargados para la determinación" Font-Size="10pt" ForeColor="#333333" GridLines="Horizontal" onrowcommand="gvListaVRNP_RowCommand" onrowdatabound="gvListaVRNP_RowDataBound" TabIndex="9" UpdateAfterCallBack="True" Width="100%" OnSelectedIndexChanged="gvListaVRNP_SelectedIndexChanged">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <Columns>
                                        <asp:BoundField DataField="metodo" HeaderText="Metodo">
                                        
                                        </asp:BoundField>
                                        <asp:BoundField DataField="minimo" HeaderText="Valor Mínimo">
                                        
                                        </asp:BoundField>
                                        <asp:BoundField DataField="maximo" HeaderText="Valor Maximo">
                                        
                                        </asp:BoundField>
                                        <asp:BoundField DataField="observacion" HeaderText="Observaciones">
                                        
                                        </asp:BoundField>
                                          <asp:BoundField DataField="presentacion" HeaderText="Presentacion" />
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="EliminarVR" runat="server" CommandName="EliminarVR" ImageUrl="~/App_Themes/default/images/eliminar.jpg" OnClientClick="return PreguntoEliminar();" />
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
                                </anthem:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4">
                                <anthem:Label ID="lblMensaje2" Visible="False" runat="server" Text="Label" Font-Bold="True" ForeColor="Red"></anthem:Label>
                                <anthem:Button ID="btnAplicarEfectoresNP" runat="server" CssClass="btn btn-primary"  Text="Aplicar a todos los Efectores" Visible="true" Width="280px" OnClick="btnAplicarEfectoresNP_Click" AutoUpdateAfterCallBack="True" />
                            </td>
                        </tr>
                       
                             
					</table>
					    <anthem:ValidationSummary ID="ValidationSummary2" runat="server" 
                     HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                     ValidationGroup="1" ShowSummary="False" />





					    </anthem:Panel>
                       
					</div>
					

    				 
					
<div id="tab3" class="tab_content" style="border: 1px solid #C0C0C0">
<anthem:Panel ID="pnlDiagrama" runat="server">
  <table  width="600px" align="center" class="myTabla">
					
				
					<tr>
						<td  class="control-label" >Analisis:</td>
						<td colspan="2">
                            <asp:Label ID="lblItemDiagrama" runat="server" Font-Bold="False" 
                                CssClass="mytituloGris"></asp:Label>
                                            </td>
						<td>
                            &nbsp;</td>
					</tr>
					<tr>
						<td   colspan="4"><hr /></td>
					</tr>
					<tr>
						<td class="control-label"  >Codigo del Analisis:</td>
						<td colspan="2">
                            <anthem:TextBox ID="txtCodigoDiagrama" runat="server" AutoCallBack="True" 
                                ontextchanged="txtCodigoDiagrama_TextChanged" class="form-control input-sm" TabIndex="1" 
                                ToolTip="Ingrese el codigo del analisis"></anthem:TextBox> &nbsp;
                                            <anthem:Label ID="lblMensajeDiagrama" runat="server" 
                                ForeColor="#FF3300"></anthem:Label>
                                            </td>
						<td>
                            &nbsp;</td>
					</tr>
					<tr>
						<td  class="control-label" >
                                            Nombre del Analisis:</td>
						<td colspan="2">
                            <anthem:DropDownList ID="ddlItemDiagrama" runat="server" AutoCallBack="True" 
                                onselectedindexchanged="ddlItemDiagrama_SelectedIndexChanged" class="form-control input-sm" 
                                TabIndex="2" ToolTip="Seleccione el analisis">
                            </anthem:DropDownList>
                                     &nbsp;    
                            <asp:RangeValidator ID="rvItem" runat="server" ControlToValidate="ddlItemDiagrama" 
                                ErrorMessage="Determinacion" MaximumValue="999999" MinimumValue="1" 
                                Type="Integer" ValidationGroup="2">*</asp:RangeValidator>
                                        
                        </td>
						
						<td>
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td class="control-label"  >
                                            Texto a Imprimir:</td>
						<td colspan="2">
                            <anthem:TextBox ID="txtNombreDiagrama" ReadOnly="true"  runat="server" Width="343px" class="form-control input-sm" 
                                TabIndex="3" ToolTip="Ingrese el texto a imprimir"></anthem:TextBox> &nbsp;
                            <anthem:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtNombreDiagrama" ErrorMessage="Texto a Imprimir" 
                                ValidationGroup="2">*</anthem:RequiredFieldValidator>
                                        
                        </td>
						
						<td>
                                            <input ID="Button2" class="myButtonGris" onclick="CrearFilaDiagrama()" 
                                                type="button" value="Agregar a Lista" validationgroup="2" /></td>
						
					</tr>
					<tr>
						<td   colspan="4">
                                            <hr /></td>
						
					</tr>
					
					<tr>
					<td colspan="4" align="center">
                        <table id="Table2"  
        style="font-size:1em; margin-left:1%; background-color: #DFE7F7; font-weight: bold;" 
                                cellpadding="0" cellspacing="0" 
        >
<tr>
<td style="width:40px;">&nbsp</td><td width="450px" align="center" >Diagrama</td>
   
</tr>


</table>
<table summary="Tabla editable para sumar filas y columnas" id="Table3"  
        style="font-size:.9em; margin-left:1%; " cellpadding="0" cellspacing="0" 
        >

<tbody id="cuerpoDiagrama">

</tbody>
</table></td>
					</tr>
					<tr>
					<td colspan="4" align="left">
                                    
                                            <hr /></td>
					</tr>
					<tr>
                        <td align="left" colspan="2">
                            <input  type="hidden" runat="server" name="TxtDatosDiagrama" id="TxtDatosDiagrama" value=""/>
                            	 </td>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnGuardarDiagrama" runat="server"  
                                Text="Guardar" CssClass="btn btn-primary" Width="100px" 
                                onclick="btnGuardarDiagrama_Click" />
                        </td>
                    </tr>
					</table>
			</anthem:Panel>
</div>				


<div id="tab4" class="tab_content" style="border: 1px solid #C0C0C0">
<anthem:Panel ID="pnlPredefinidos" runat="server">
 <table  width="600px" align="center" class="myTabla">
					<tr>
						<td class="control-label" >Analisis:</td>
						<td colspan="2">
                            <asp:Label ID="lblItemRP" runat="server" Font-Bold="False" 
                                CssClass="mytituloGris"></asp:Label>
                                            </td>
					</tr>
					<tr>
						<td class="control-label" >
                                            Posible Resultado:</td>
						<td colspan="2">
                            <asp:TextBox ID="txtNombreRP" runat="server" Width="300px" class="form-control input-sm"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
                        <td class="control-label">Efector Derivante</td>
                        <td colspan="2">
                            <anthem:DropDownList ID="ddlEfectorItemDeriva" Width="300px" runat="server" class="form-control input-sm" TabIndex="9" ToolTip="Seleccione el efector a derivar">
                            </anthem:DropDownList>
                            <input id="Button1" type="button" value="Agregar a Lista" onclick="CrearFila()" class="myButtonGris" />
                        </td>
                    </tr>
					<tr>
						<td colspan="3" >
                                         <hr />   </td>
						
					</tr>
					<tr>
						
						<td colspan="3" align="center">
                           <table 
        >
<tr>
<td style="width:40px;">&nbsp</td><td width="500px" align="center" >Resultados 
    Predefinidos</td>
   
</tr>


</table>
<table summary="Tabla editable para sumar filas y columnas" id="tabla"  
        style="font-size:.9em; margin-left:1%; " cellpadding="0" cellspacing="0" 
        >

<tbody id="cuerpo">

</tbody>
</table></td>
						
					</tr>
					 
					<tr>
						<td colspan="2">
    
        
            
                                            &nbsp;</td>
						
						<td align="right">
                                            <anthem:Button ID="btnGuardarRP" runat="server" Text="Guardar" 
                                                onclick="btnGuardarRP_Click" CssClass="btn btn-primary" Width="100px"  ValidationGroup="0" />
        
            
                        </td>
						
					</tr>
					 
					
					<tr>
                        <td class="control-label" colspan="2">
                            &nbsp;</td>
                        <td>
                            <anthem:Label ID="lblMensajeRpD" runat="server" ForeColor="#990000" 
                                Visible="False">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </anthem:Label>
                        </td>
                    </tr>
					
					</table>
					<input type="hidden" runat="server" name="TxtDatosResultados" id="TxtDatosResultados" value="" /> 
					</anthem:Panel>

    <table>
            <tr>
                        <td colspan="3"    >
                            Resultado por Defecto:</td>
                        <td >
                            <anthem:DropDownList ID="ddlResultadoPorDefecto" Width="300px" runat="server" class="form-control input-sm">
                            </anthem:DropDownList>&nbsp;
                 </td>
                 <td >
                            <anthem:Button ID="btnGuardarRPDefecto" runat="server" CssClass="btn btn-primary" Width="100px" 
                                onclick="btnGuardarRPDefecto_Click" Text="Guardar" ValidationGroup="0" 
                                 />
                        </td>
                    </tr>
    </table>
</div>	

<div id="tab5" class="tab_content" style="border: 1px solid #C0C0C0">
<anthem:Panel ID="pnlRecomendacion" runat="server">
<table  width="750px" align="center" class="myTabla">					
					
					<tr>
						<td class="control-label" >Análisis:</td>
						<td>
                            <asp:Label ID="lblItemRecomendacion" runat="server" Font-Bold="False" 
                                CssClass="mytituloGris"></asp:Label>
                                            </td>
						<td>
                            &nbsp;</td>
					</tr>
					<tr>
						<td   colspan="3"><hr /></td>
					</tr>
					<tr>
						<td class="control-label" >
                                            Recomendación:</td>
						<td>
                            <asp:DropDownList ID="ddlRecomendacion" runat="server" class="form-control input-sm"
                                ToolTip="Seleccione la recomendación">
                            </asp:DropDownList> &nbsp;
                            <asp:RangeValidator ID="rvRecomendacion" runat="server" 
                                ControlToValidate="ddlRecomendacion" ErrorMessage="Recomendacion" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="5">*</asp:RangeValidator>
                        </td>
						
						<td>
                                            <anthem:Button ID="btnAgregarRecomendacion" runat="server" Text="Agregar" 
                                                onclick="btnAgregarRecomendacion_Click" CssClass="btn btn-primary" Width="100px" 
                               ValidationGroup="5" />
                        </td>
						
					</tr>
					<tr>
						<td   colspan="3">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td colspan="3">
        <anthem:GridView ID="gvListaRecomendacion" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idItemRecomendacion" onrowcommand="gvListaRecomendacion_RowCommand1" 
            onrowdatabound="gvListaRecomendacion_RowDataBound" Font-Size="8pt" Width="100%" 
                                ForeColor="#333333" EmptyDataText="Agregue recomendaciones para el analisis seleccionado" 
                                CellPadding="0" 
                                BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px" 
                                GridLines="Horizontal">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
               <asp:BoundField DataField="recomendacion" 
                    HeaderText="Recomendaciones" >
                    <ItemStyle Width="90%" />
                </asp:BoundField>
              <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="5%" HorizontalAlign="Center" />
                          
                        </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </anthem:GridView>
                        </td>
						
					</tr>
					<tr>
					<td colspan="3" align="right">
                      <hr /></td>
					</tr>
					
					
					</table>
	</anthem:Panel>
</div>

<div id="tab6" class="tab_content" style="border: 1px solid #C0C0C0">
<table width="750px">
<tr>
						<td class="control-label" colspan="2" >Análisis:		
							<asp:Label ID="lblItemHiv" runat="server" Font-Bold="True" 
                               class="control-label"></asp:Label>
								
                                                                                </td>
					</tr>
<tr>
						<td class="control-label" >&nbsp;</td>
						<td class="control-label" colspan="2" >&nbsp;</td>
					</tr>
                    <tr>
						<td class="mytituloGris" >&nbsp;</td>
						<td  colspan="2" ><h4>CONFIDENCIALIDAD </h4><hr />
                            <small  >
    Si la determinación corresponde a una <b>Prueba Diagnóstica</b> que requiere que se codifiquen los datos del paciente de acuerdo a la <b>LEY NACIONAL 23798</b> y a la correspondiente provincial <b>1922/91</b>;
    tilde <b>"Codificar Datos del Paciente"</b>.
    </small>
</td></tr>
                         
 
<tr>
						<td   >

                            &nbsp;</td>
						<td >

    <asp:CheckBox ID="chkCodificaHiv" runat="server" CssClass="control-label" 
        Text="Codificar Datos del Paciente" /> <br />
                              <small>Solo modificable por Administrador</small>
                            <br />
                        </td>
						<td class="myLabelDerechaGde" align="right" >

                                            &nbsp;</td>
					</tr>
<tr>
						<td class="myLabelDerechaGde" >

                            &nbsp;</td>
						<td class="myLabelDerechaGde" colspan="2" >

                            &nbsp;</td>
					</tr>
                  
    <%--<tr>
						<td class="myLabelDerechaGde" colspan="2" >

                            Si el analisis crresponde una <b>Prueba de Screening Neonatal</b>; marque que se deben 
                            solicitar datos adicionales al paciente: Hora de nacimiento, Peso al nacer y 
                            Semana de gestación.</td>
					</tr>
<tr>
						<td class="myLabelDerechaGde" colspan="2" >

                            &nbsp;</td>
					</tr>
<tr>
						<td class="myLabelDerechaGde" colspan="2" >

    <asp:CheckBox ID="chkIsScreening" runat="server" CssClass="control-label" 
        Text="Solicitar Datos Adicionales del Paciente" />
                        </td>
					</tr>--%>
<tr>
						<td >
                            &nbsp;</td>
						<td  colspan="2" >
                            <br /></td>
					</tr>
                     <tr>
						<td class="mytituloGris" >&nbsp;</td>
						<td  colspan="2" ><h4>ETIQUETA CODIGO DE BARRAS  </h4> <hr />
</td></tr>
<tr align="left">
						<td class="control-label" align="left" >
                            &nbsp;</td>
						<td class="control-label" align="left" colspan="2" >
                             <small>Si la determinación requiere un tubo o contenedor adicional a etiquetar; tilde la opción <b>"Generar Etiqueta Adicional"</b></small></td>
					</tr>
<tr align="left">
						<td class="control-label" align="left" >

                            &nbsp;</td>
						<td class="control-label" align="left" >

    <asp:CheckBox ID="chkEtiquetaAdicional" Enabled="false" runat="server" CssClass="control-label" 
        Text="Generar Etiqueta Adicional (envase propio)" />
                            <br />
                            <small>Solo modificable por Administrador</small>
                        </td>
						<td class="control-label" align="right" >

                                            &nbsp;</td>
					</tr>

<tr align="left">
						<td class="control-label" align="right" >
                            &nbsp;</td>
						<td class="control-label" align="right" >
                            &nbsp;</td>
						<td class="control-label" align="right" >
                           
                                            <asp:Button ID="btnGuardarHIV" runat="server" Text="Guardar" 
                                                onclick="btnGuardarHIV_Click" CssClass="btn btn-primary" Width="100px"  TabIndex="20" 
                                                ToolTip="Hacer clic aqui para guardar" />
                        </td>
					</tr>
</table>
</div>

    <div id="tab9" class="tab_content" style="border: 1px solid #C0C0C0">
        <table>
               <tr>
						<td class="mytituloGris" >&nbsp;</td>
						<td class="mytituloGris" colspan="2" >TURNOS<hr />
</td></tr>
<tr>
						<td class="control-label" >

                            &nbsp;</td>
						<td class="control-label" colspan="2" >

                         Límite de Turnos por Día:&nbsp;&nbsp;  <input id="txtLimite" runat="server" type="text" maxlength="3" 
                          style="width: 40px"  onblur="valNumero(this)" tabindex="4" class="form-control input-sm"
                                title="Ingrese el limite de turnos" />&nbsp;<asp:RequiredFieldValidator 
                                ID="rfvLimite" runat="server" 
                          ControlToValidate="txtLimite" ErrorMessage="Limite de turnos" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator> &nbsp;<p class="myLabelLitlle">Colocar 0 para especificar sin limites de turnos.<br /> Colocar -1 para NO permitir la dación de turnos para esta práctica.</p> </td>
					</tr>
        </table>

        </div>

				
					
					
	</div>					

</td>
</tr>
<tr>
<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" CausesValidation="False" 
                                                ToolTip="Regresa a la pagina anterior" onclick="lnkRegresar_Click1" >Regresar</asp:LinkButton>
                        </td>
<td align="right" colspan="2"  >
						 &nbsp;</td>
<td align="right"  >
						 
                                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" 
                                                onclick="btnNuevo_Click"  CssClass="btn btn-primary" Width="100px" 
                                                ToolTip="Haga clic aquí para agregar un nuevo analisis" />
                        </td>
</tr>
</table>

	
</div>
</div>

<script language="javascript" type="text/javascript">

function VerificaLargo (source, arguments)
    {                
    var Observacion = arguments.Value.toString().length;
 //   alert(Observacion);
    if (Observacion>4000 )
        arguments.IsValid=false;    
    else   
        arguments.IsValid=true;    //Si llego hasta aqui entonces la validación fue exitosa        
}



function InhabilitarEdades()
{
alert(document.all.getElementById('txtEdadDesde').value);
//document.all["txtEdadDesde"].value="0";
}



CargarDetalle();
CargarDetalleDiagrama();
// var contadorfilas = 0; 
var tab;
var filas;	


function CrearFila()
{
    var nombre = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtNombreRP").ClientID %>').value;
    var idEfector= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("ddlEfectorItemDeriva").ClientID %>').value;

if (nombre!='')
{
    NuevaFila(nombre, idEfector);
    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtNombreRP").ClientID %>').value = '';
    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("ddlEfectorItemDeriva").ClientID %>').value='';
}
}
  
  
  
function CrearFilaDiagrama()
{
var nombre= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtNombreDiagrama").ClientID %>').value;
var codigo = document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigoDiagrama").ClientID %>').value.toUpperCase();
if (nombre!='')
{
    if   (verificarRepetidos(codigo))
    {
    NuevaFilaDiagrama(codigo,nombre);
    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtNombreDiagrama").ClientID %>').value='';
    document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtCodigoDiagrama").ClientID %>').value='';
    }
}
}
  
function NuevaFila(nom ,  efector)
{
Grilla = document.getElementById('cuerpo');

fila = document.createElement('tr');

celdaflecha1= document.createElement('td');    
celdaflecha1.className= "orden";

////crea la primera flecha
oFlecha1= document.createElement('img');
oFlecha1.name= "pru";
oFlecha1.src="../script/moverfilas/arriba.gif";
oFlecha1.alt="subir fila";
////crea la segunda flecha
oFlecha2= document.createElement('img');            
oFlecha2.src="../script/moverfilas/abajo.gif";
oFlecha2.alt="bajar fila";                                  

celdaflecha1.appendChild(oFlecha1);
celdaflecha2= document.createElement('td');    
celdaflecha2.className= "orden";
celdaflecha2.appendChild(oFlecha2);
fila.appendChild(celdaflecha1);
fila.appendChild(celdaflecha2);

///////////////////////////////////        	
celdaCodigo = document.createElement('td');   
oCodigo = document.createElement('input');
oCodigo.type = 'text';                        
oCodigo.runat = 'server';
oCodigo.onblur= function () {CargarDatos()};
oCodigo.className = 'form-control input-sm';
oCodigo.width = '300px';
oCodigo.value= nom;

celdaCodigo.appendChild(oCodigo);
fila.appendChild(celdaCodigo);
    ///////////////////////////////////
celdaEfector = document.createElement('td');
oCodigo1 = document.createElement('input');
oCodigo1.type = 'text';
oCodigo1.runat = 'server';
//oCodigo1.onblur = function () { CargarDatos() };
oCodigo1.className = 'form-control input-sm';
oCodigo1.width = '200px';
oCodigo1.value = efector;

celdaEfector.appendChild(oCodigo1);
fila.appendChild(celdaEfector);
    /////////////////////

celda6 = document.createElement('td');  
celda6.className= "orden";
celda6.width="60px";
oBoton= document.createElement('img');
//    oBoton.name= "pru";
oBoton.src="../script/moverfilas/eliminar.gif";
oBoton.alt="eliminar fila";
oBoton.onclick = function () {borrarfila(this,'cuerpo')};
celda6.appendChild(oBoton);
fila.appendChild(celda6);

Grilla.appendChild(fila);

iniciarTabla('cuerpo');
CargarDatos();

}
        
        
function NuevaFilaDiagrama(cod,nom)
{
Grilla = document.getElementById('cuerpoDiagrama');

fila = document.createElement('tr');

celdaflecha1= document.createElement('td');    
celdaflecha1.className= "orden";

////crea la primera flecha
oFlecha1= document.createElement('img');
oFlecha1.name= "pru";
oFlecha1.src="../script/moverfilas/arriba.gif";
oFlecha1.alt="subir fila";
////crea la segunda flecha
oFlecha2= document.createElement('img');            
oFlecha2.src="../script/moverfilas/abajo.gif";
oFlecha2.alt="bajar fila";                                  

celdaflecha1.appendChild(oFlecha1);
celdaflecha2= document.createElement('td');    
celdaflecha2.className= "orden";
celdaflecha2.appendChild(oFlecha2);
fila.appendChild(celdaflecha1);
fila.appendChild(celdaflecha2);

///////////////////////////////////        	
///celda para el codigo
celdaCodigo = document.createElement('td');   
oCodigo = document.createElement('input');
oCodigo.type = 'text';                        
oCodigo.runat = 'server';
oCodigo.className = 'textoCorto';
oCodigo.value= cod;

celdaCodigo.appendChild(oCodigo);
fila.appendChild(celdaCodigo);
///////////////////////////////////

///////////////////////////////////        	
///celda para el NOMBRE
celdaNombre = document.createElement('td');   
oNombre = document.createElement('input');
oNombre.type = 'text';                        
oNombre.runat = 'server';
oNombre.className = 'textoLargo';
oNombre.value= nom;

celdaNombre.appendChild(oNombre);
fila.appendChild(celdaNombre);

///////////////////////////////////

celda6 = document.createElement('td');  
celda6.className= "orden";
celda6.width="60px";
oBoton= document.createElement('img');
//    oBoton.name= "pru";
oBoton.src="../script/moverfilas/eliminar.gif";
oBoton.alt="eliminar fila";
oBoton.onclick = function () {borrarfila(this,'cuerpoDiagrama')};
celda6.appendChild(oBoton);
fila.appendChild(celda6);

Grilla.appendChild(fila);
iniciarTabla('cuerpoDiagrama');
CargarDatosDiagrama();
}

        
function CargarDatos()
{
var str = '';            
var tab;
var filas;	   	     

tab = document.getElementById('cuerpo');
filas = tab.getElementsByTagName('tr');
for (i=0; ele = filas[i]; i++)
{  
    var codigo = ele.getElementsByTagName('input')[0].value;
    var efector = ele.getElementsByTagName('input')[1].value;
    if (codigo!='')
        str = str + codigo + '#'+ efector+ '@';
}     	     	     
document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosResultados").ClientID %>').value = str;	   	        

}
        
function CargarDatosDiagrama()
{
var str = '';            
var tab;
var filas;	   	     

tab = document.getElementById('cuerpoDiagrama');
filas = tab.getElementsByTagName('tr');
for (i=0; ele = filas[i]; i++)
{  
    var codigo=ele.getElementsByTagName('input')[0].value;
     var nombre=ele.getElementsByTagName('input')[1].value;
  
    if (codigo!='')
     str = str + codigo +'#'+nombre+  '@';
}     	     	     

document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosDiagrama").ClientID %>').value = str;	   	        
}
        
function CargarDetalle()
{ 
var datos= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosResultados").ClientID %>').value ;	   	        
if (datos!="")
{      
    var sTab = datos.split('@');                
    for (var i=0; i<(sTab.length-1); i++)
    {
        var sFi = sTab[i].split('#');
        if  (sFi[0]!="")
        {
            NuevaFila(sFi[0], sFi[1]);
        }
    }
}
}

function CargarDetalleDiagrama()
{ 
var datos= document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("TxtDatosDiagrama").ClientID %>').value ;	   	        
if (datos!="")
{      
    var sTab = datos.split('@');                
    for (var i=0; i<(sTab.length-1); i++)
    {
        var sFi = sTab[i].split('#');
        if  (sFi[0]!="")
        {
            NuevaFilaDiagrama(sFi[0],sFi[1]);	            
        }
    }
}
}



function verificarRepetidos(codigo)
{
///Verifica si ya fue cargado en el txtDatos

var devolver=true;
if (codigo!='')
{
    var listaExistente =document.getElementById('<%= Page.Master.FindControl("ContentPlaceHolder1").FindControl("txtDatosDiagrama").ClientID %>').value ;
    var cantidad=1;
    var sTabla = listaExistente.split('@');                                    
    for (var i=0; i<(sTabla.length-1); i++)
    {
        var sFila = sTabla[i].split('#');
        if  (sFila[0]!='')
        {
            if (codigo== sFila[0])	                    
            {
                cantidad+=1;
                devolver=false;   	                        	                     
            }
        }	                
    }
    if (cantidad>1)
    {
        alert('El código '+ codigo +' ya fue cargado. No se admiten analisis repetidos.');	                                          
        devolver=false;       
    }
    else
        devolver=true;	             
}	           
else
    devolver=true;

return devolver;
}

function PreguntoEliminar()
{
if (confirm('¿Está seguro de eliminar el registro?'))
return true;
else
return false;
}


</script>
                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                     ValidationGroup="0" ShowSummary="False" />

   
 
    </div>

   
 
 </asp:Content>

