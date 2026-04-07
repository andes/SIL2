<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebLab.Turnos.Default" MasterPageFile="~/Site1.Master" %>


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
				
					 
				<%--<asp:RangeValidator ID="rvSexo" runat="server" 
                                ControlToValidate="ddlSexo" ErrorMessage="Sexo" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>--%>                      <%--		<td rowspan="4" >   </td>--%>



    <tr>
						<td   >
                                            Tipo de Doc.:
                        </td>
						<td align="left" colspan="2">
                                            <asp:DropDownList ID="ddlTipo" runat="server" class="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged">
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
                                onblur="valNumeroSinPunto(this)" maxlength="8" tabindex="1" style="width: 100px"/>
                           
                           
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                        </td>
						
					</tr>
				
					
					<tr>
						<td   style="width: 150px">
                                            Sexo:
                                            <%--	<td rowspan="3" >   </td>--%>
                                        </td>
						<td align="left" colspan="2">
                                            <asp:DropDownList ID="ddlSexo" runat="server" TabIndex="2" class="form-control input-sm">
                                                <asp:ListItem Value="0" Selected="True">--Seleccione--</asp:ListItem>
                                                <asp:ListItem Value="2">Femenino</asp:ListItem>
                                                <asp:ListItem Value="3">Masculino</asp:ListItem>
                                             
                                            </asp:DropDownList>
                                            <asp:Label ID="lblMensajeSexo" runat="server" Font-Bold="True" ForeColor="#CC3300" Visible="False"></asp:Label>
                        </td>
                        <%--	<td align="right" >
        <br />
        
        </td>--%>
					</tr>
				
					
					<tr>
						<td   style="width: 150px">
                                            Nro. Ident. Adicional:</td>
						<td align="left" colspan="2">
                                            <asp:TextBox ID="txtNumeroAdicional" runat="server" CssClass="form-control input-sm" Enabled="False" TabIndex="3"></asp:TextBox>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0"  Width="100px"
                                                CssClass="btn btn-primary" TabIndex="4" onclick="btnBuscar_Click" 
                                                ToolTip="Buscar Persona" />

                        </td>
					<%--	<td rowspan="3" >   </td>--%>
					</tr>
    <tr>
						<td  colspan="2" >					
                                   <asp:LinkButton ID="lnkAmpliarFiltros"   CssClass="myLittleLink" runat="server" 
                                                onclick="lnkAmpliarFiltros_Click" Text="Ampliar filtros de búsqueda"></asp:LinkButton>
                            <br />
                        <asp:Panel ID="pnlParentesco" runat="server" Visible="false">
                          <table width="100%">
                                            
					<tr>
						<td  >
                                            Apellidos:</td>
						<td align="left" >
                                                          <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm" TabIndex="5" ToolTip="Ingrese el apellido del paciente" Width="300px"></asp:TextBox>
                        </td>
						
					</tr>
					          <tr>
                                  <td>Nombres:</td>
                                  <td align="left">
                                      <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" TabIndex="6" ToolTip="Ingrese el nombre del paciente" Width="300px"></asp:TextBox>
                                  </td>
                              </tr>
					<tr>
						<td   >
                                            DU Madre/Tutor:</td>
						<td align="left" >
                                            <input title="Ingrese el documento unico del parentesco" id="txtDniMadre" type="text" runat="server"  class="form-control input-sm"
                                 onblur="valNumero(this)" style="width: 150px" maxlength="8" tabindex="7"/>
                                            <asp:CustomValidator ID="cvDNIMadre" runat="server" ErrorMessage="Numero " onservervalidate="cvDNIMadre_ServerValidate" ValidationGroup="2">Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                        </td>
						
					</tr>
					<tr>
						<td  >
                                            Apellido Madre/Tutor:</td>
						<td align="left" >
                                            <asp:TextBox ID="txtApellidoMadre" runat="server" class="form-control input-sm" TabIndex="8" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
				
					          <tr>
                                  <td >Nombres/s Madre/Tutor:</td>
                                  <td align="left">
                                      <asp:TextBox ID="txtNombreMadre" runat="server" class="form-control input-sm" TabIndex="9" ToolTip="Ingrese el apellido del paciente" Width="300px"></asp:TextBox>
                                  </td>
                              </tr>
				
					          <tr>
                                  <td>&nbsp;</td>
                                  <td align="left">
                                      <asp:Button ID="btnBuscarMas" runat="server" CssClass="btn btn-primary" onclick="btnBuscarMas_Click" TabIndex="10" Text="Buscar Adicional" ToolTip="Buscar Persona" ValidationGroup="2" Visible="False" Width="150px" />
                                  </td>
                              </tr>
				
					</table>
					    </asp:Panel>
                        </td>
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
                                EmptyDataText="No se encontraron pacientes para los parametros de busqueda ingresados"  ForeColor="#666666" GridLines="Horizontal" onpageindexchanging="gvLista_PageIndexChanging" onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" PageSize="13" Width="100%" TabIndex="11">
               
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