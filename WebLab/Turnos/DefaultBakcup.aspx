<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultBakcup.aspx.cs" Inherits="WebLab.Turnos.Default" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title> 
<script type="text/javascript" src="../script/Mascara.js"></script>
  
      <script type="text/javascript" src="../script/ValidaFecha.js"></script> 
  
 <script src="../Protocolos/Resources/jquery.min.js" type="text/javascript"></script>
    <link href="../Protocolos/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="../Protocolos/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>

    <script type="text/javascript">    
	      

    function seleccionarPaciente() {
        var dom = document.domain;
        var domArray = dom.split('.');
        for (var i = domArray.length - 1; i >= 0; i--) {
            try {
                var dom = '';
                for (var j = domArray.length - 1; j >= i; j--) {
                    dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
                }
                document.domain = dom;
                break;
            } catch (E) {
            }
        }

        var $this = $(this);
        $('<iframe src="http://intranet.hospitalneuquen.org.ar/scripts/historias/historias.dll/alta?inside=1" style="-moz-box-sizing: border-box; -webkit-box-sizing: border-box; box-sizing: border-box" />').dialog({
            title: 'Nuevo Paciente',
            autoOpen: true,
            width: 900,
            height: 500,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(900);
    }

    
   
 
</script>
</asp:Content>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">     
 
   
      <div align="left" style="width: 800px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"> NUEVO TURNO
                     <small>Búsqueda e Identificación del Paciente</small> </h3>
                        </div>

				<div class="panel-body">	
    
<table style="width:100%" >
				
					 
				<%--	<tr>
							<td   >
                                            DNI/LE/LC:</td>
						<td align="left" colspan="2">
                           <input id="txtDni" type="text" runat="server" style="width:100px;"  class="form-control input-sm"  
                                onblur="valNumero(this)" maxlength="8" tabindex="0" />
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValidationGroup="0"></asp:CompareValidator>
                        </td>
						
					</tr>
					<tr>
							<td   >
                                            Apellido/s:</td>
						<td align="left" colspan="2">
                                            <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm" TabIndex="1" 
                                                ValidationGroup="1" Width="150px"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
							<td   >
                                            Nombres/s:</td>
						<td align="left" colspan="2">
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm"  TabIndex="2" 
                                                Width="216px"></asp:TextBox>
                        </td>
						
					</tr>

                    
					<tr>
						<td   >
                                            Fecha de Nac.:</td>
						<td align="left" colspan="2">
                                            <input id="txtFechaNac" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width: 70px"  /></td>
						
					</tr>
				
					
					<tr>
						<td   >
                                            Sexo:</td>
						<td align="left" colspan="2">
                                            <asp:DropDownList ID="ddlSexo" runat="server" TabIndex="4" class="form-control input-sm" >
                                                <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                                                <asp:ListItem Value="2">Femenino</asp:ListItem>
                                                <asp:ListItem Value="3">Masculino</asp:ListItem>
                                                <asp:ListItem Value="1">Indeterminado</asp:ListItem>
                                            </asp:DropDownList>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="3">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td  >
                                  <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-info" OnClick="btnBuscar_Click"    Width="100px" >
                                             <span class="glyphicon glyphicon-search"></span>&nbsp;Buscar</asp:LinkButton>       
                                        <%--    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0" 
                                               CssClass="myButton" TabIndex="5" onclick="btnBuscar_Click" />--%>
                      <%--   </td>
						<td align="left">
                                            <asp:CustomValidator ID="cvDatosEntrada" runat="server" 
                                                ErrorMessage="Debe ingresar al menos un parametro de busqueda" 
                                                onservervalidate="cvDatosEntrada_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                        </td>
						
						<td align="right">
                                      
                              <asp:HyperLink ID="HyperLink1"  runat="server"  CssClass="btn btn-danger" Width="150px" 
                                               ToolTip="Crear un nuevo paciente en el SIPS" Target="_self">Nuevo Paciente</asp:HyperLink> 
                                               
                                                 <asp:Button ToolTip="Crear un nuevo paciente "  CssClass="btn btn-primary"
                                               ID="btnSeleccionarPaciente" Width="150px" runat="server" Text="Nuevo Paciente" 
                                               OnClientClick="seleccionarPaciente(); return false;" visible="false"
                                               onclick="btnSeleccionarPaciente_Click" TabIndex="100" 
                                               UseSubmitBehavior="False" />
        <br />
        <asp:HiddenField ID="hfPaciente" runat="server" />
                                               </td>
						
					</tr>
					<tr>
						<td   colspan="3">
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td   colspan="3" style="vertical-align: top">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idPaciente" CssClass="table table-bordered bs-table"  PageSize="15" 
                                
                                
                                EmptyDataText="No se encontraron pacientes para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                TabIndex="5" 
                                GridLines="None" onpageindexchanging="gvLista_PageIndexChanging" 
                               Width="600px">
           

            <Columns>
                <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                            ommandName="Editar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="18px" HorizontalAlign="Center" Height="18px" />
                          
                        </asp:TemplateField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                    <ItemStyle Width="15%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <ItemStyle Width="65%" />
                </asp:BoundField>
                 <asp:BoundField DataField="sexo" HeaderText="Sexo">
                     <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nacimiento">
                     <ItemStyle HorizontalAlign="Center" Width="15%" />
                </asp:BoundField>
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Turno" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                            ommandName="Turno" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="18px" HorizontalAlign="Center" Height="18px" />
                          
                        </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" 
                Font-Names="Arial" Font-Size="8pt" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                        </td>
						
					</tr>--%>



    <tr>
						<td   >
                                            Tipo de Doc.:
                        </td>
						<td align="left" colspan="2">
                                            <asp:DropDownList ID="ddlTipo" runat="server" TabIndex="4" class="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
                                                <asp:ListItem Value="DNI" Selected="True">DNI</asp:ListItem>
                                                <asp:ListItem Value="T">Temporal</asp:ListItem>
                                             
                                            </asp:DropDownList>
                        </td>
						
					</tr>
				
					
					<tr>
						<td   >
                                            Nro. Documento:</td>
						<td align="left" colspan="2">
                            <input id="txtDni" type="text" runat="server"  class="form-control input-sm"
                                onblur="valNumeroSinPunto(this)" maxlength="8" tabindex="0" style="width: 100px"/>
                           
                           
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                        </td>
						
					</tr>
				
					
					<tr>
						<td   style="width: 150px">
                                            Sexo:<asp:RangeValidator ID="rvSexo" runat="server" 
                                ControlToValidate="ddlSexo" ErrorMessage="Sexo" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                        </td>
						<td align="left" colspan="2">
                                            <asp:DropDownList ID="ddlSexo" runat="server" TabIndex="4" class="form-control input-sm">
                                                <asp:ListItem Value="0" Selected="True">--Seleccione--</asp:ListItem>
                                                <asp:ListItem Value="2">Femenino</asp:ListItem>
                                                <asp:ListItem Value="3">Masculino</asp:ListItem>
                                             
                                            </asp:DropDownList>
                        </td>
				<%--		<td rowspan="4" >   </td>--%>
					</tr>
				
					
					<tr>
						<td   style="width: 150px">
                                            Nro. Ident. Adicional:</td>
						<td align="left" colspan="2">
                                            <asp:TextBox ID="txtNumeroAdicional" runat="server" CssClass="form-control input-sm" Enabled="False" TabIndex="3"></asp:TextBox>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0"  Width="100px"
                                                CssClass="btn btn-primary" TabIndex="5" onclick="btnBuscar_Click" 
                                                ToolTip="Buscar Persona" />

                        </td>
					<%--	<td rowspan="3" >   </td>--%>
					</tr>
				
					
					<tr>
						<td  colspan="2" >
                                   <hr /></td>
						
					</tr>
					
					<tr>
						<td  colspan="2" >					
                            <br />
                            <asp:Label ID="lblMensaje" runat="server" ForeColor="Blue" Text="Se encontraron los siguientes datos para el dni ingresado:" Visible="False"></asp:Label>
                            <asp:GridView ID="gvLista" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="1" 
                                CssClass="table table-bordered bs-table" DataKeyNames="idPaciente" 
                                EmptyDataText="No se encontraron pacientes para los parametros de busqueda ingresados"  ForeColor="#666666" GridLines="Horizontal" onpageindexchanging="gvLista_PageIndexChanging" onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" PageSize="13" Width="100%">
               
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" ommandName="Editar" />
                                        </ItemTemplate>
                                        <ItemStyle Height="18px" HorizontalAlign="Center" Width="18px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="dni" HeaderText="DNI">
                                    <ItemStyle HorizontalAlign="Center" Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                                    <ItemStyle Width="45%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sexo" HeaderText="Sexo">
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac.">
                                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="estado" HeaderText="Estado"/>
                                    
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <asp:Button ID="Turno" runat="server" class="btn btn-primary" Text="TURNO" Width="100px " />
                                             
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" />
                            </asp:GridView>
                        </td>
					</tr>
                                                
					<tr>
						<td>
                                            &nbsp;</td>
						<td align="right">
                                            <asp:CustomValidator ID="cvDatosEntrada" runat="server" 
                                                ErrorMessage="Debe ingresar al menos un parametro de busqueda." 
                                                onservervalidate="cvDatosEntrada_ServerValidate" ValidationGroup="0" 
                                                TabIndex="500"></asp:CustomValidator>
                                           
                        </td>
						
					<%--	<td align="right" >
        <br />
        
        </td>--%>
                                                   
						
					</tr>
			
					
					
					</table>
                    </div>
       </div>
          </div>
     </asp:Content>