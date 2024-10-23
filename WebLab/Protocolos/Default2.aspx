<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="WebLab.Protocolos.Default2" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajx" %>
<%@ Register Src="~/PeticionList.ascx" TagPrefix="uc1" TagName="PeticionList" %>
<%@ Register src="ProtocoloList.ascx" tagname="ProtocoloList" tagprefix="uc1" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title> 
    <%--<RowStyle BackColor="#F7F6F3" ForeColor="Black" Font-Names="Arial" 
                Font-Size="8pt" />--%>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script> 
  
  
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
     
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
  
      <ajx:toolkitscriptmanager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
    EnableScriptLocalization="true">
  </ajx:toolkitscriptmanager>
        <div align="left" style="width: 1200px" class="form-inline"  > <asp:Label ID="lblServicio" Visible="false" runat="server" CssClass="myLabelIzquierdaGde" 
                                       Text="Label"></asp:Label>
 <table  width="1200px" align="center" class="myTabla" >
					<tr>
						<td rowspan="10" style="vertical-align: top; width:300px;" >
						
                               <div id="pnlTitulo" runat="server" class="panel panel-default">
                    <div class="panel-heading">
                      <asp:Label Text="Ultimos 10 Protocolos" runat="server" ID="lblTituloLista"></asp:Label>
                        </div>

				<div class="panel-body" style="align-content:center;">
                            <uc1:ProtocoloList ID="ProtocoloList1" runat="server" />
                    <asp:Button ID="btnFinalizarCaso" runat="server" Text="Finalizar e Imprimir Caratula"   CssClass="btn btn-primary" Width="250px" OnClick="btnFinalizarCaso_Click" Visible="False" CausesValidation="False"/>
                    </div>
                                   </div>
                        </td>
						
						
						<td rowspan="10" style="vertical-align: top" >
                                            &nbsp;</td>
						
						
						<td rowspan="10" style="vertical-align: top" >
                              <div id="pnlTitulo2" runat="server" class="panel panel-default" style="width:720px;" >
                    <div class="panel-heading">
                    <h5>    <asp:Label ID="lblTitulo" runat="server" Text="NUEVO PROTOCOLO"></asp:Label></h5>
                         <asp:Image ID="imgUrgencia" ToolTip="URGENCIA" runat="server" ImageUrl="../App_Themes/default/images/urgencia.jpg" />
                          
                        </div>

				<div class="panel-body">
                                            <table class="myTabla" width="700px"   >
                                            <tr>
						<td colspan="4" ><h5><strong>Identificación de Persona</strong> </h5>						
                                  
                           
                                                </td>
					</tr>
                                           

                    
				
					<tr>
						<td class="myLabelIzquierda"  >
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
						<td class="myLabelIzquierda"  >
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
						<td class="myLabelIzquierda"  style="width: 150px">
                                            Sexo:<asp:RangeValidator ID="rvSexo" runat="server" 
                                ControlToValidate="ddlSexo" ErrorMessage="Sexo" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                                        </td>
						<td align="left" colspan="2">
                                            <asp:DropDownList ID="ddlSexo" runat="server" TabIndex="2" class="form-control input-sm">
                                                <asp:ListItem Value="0" Selected="True">--Seleccione--</asp:ListItem>
                                                <asp:ListItem Value="2">Femenino</asp:ListItem>
                                                <asp:ListItem Value="3">Masculino</asp:ListItem>
                                             
                                                <asp:ListItem Value="4">X-No Binario</asp:ListItem>
                                             
                                            </asp:DropDownList>
                        </td>
						<td rowspan="5" >   </td>
					</tr>
				
					
					<tr>
						<td class="myLabelIzquierda"  style="width: 150px">
                                            Nro. Ident. Adicional/Parentesco:</td>
						<td align="left" colspan="2">
                                            <asp:TextBox ID="txtNumeroAdicional" runat="server" CssClass="form-control input-sm" Enabled="False" TabIndex="3"></asp:TextBox>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0"  Width="100px"
                                                CssClass="btn btn-primary" TabIndex="5" onclick="btnBuscar_Click" 
                                                ToolTip="Buscar Persona" />

                               <asp:Button ID="btnSinPersona" runat="server" Text="Protocolo Sin Persona"   Width="200px"
                                                CssClass="btn btn-danger" TabIndex="5" Visible="false"
                                                ToolTip="Ignorar Datos de Persona" OnClick="btnSinPersona_Click" />
                        </td>
					</tr>
				
					
					<tr>
						<td  colspan="4" >
                                   <hr /></td>
						
					</tr>
					
					<tr>
						<td  colspan="4" >					
                                   <asp:LinkButton ID="lnkAmpliarFiltros"   CssClass="myLittleLink" runat="server" 
                                                onclick="lnkAmpliarFiltros_Click" Text="Ampliar filtros de búsqueda"></asp:LinkButton>
                            <br />
                        <asp:Panel ID="pnlParentesco" runat="server" Visible="false">
                          <table width="100%">
                                            
					<tr>
						<td  >
                                            Apellidos:</td>
						<td align="left" >
                                                          <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm" TabIndex="7" ToolTip="Ingrese el apellido del paciente" Width="300px"></asp:TextBox>
                        </td>
						
					</tr>
					          <tr>
                                  <td>Nombres:</td>
                                  <td align="left">
                                      <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" TabIndex="8" ToolTip="Ingrese el nombre del paciente" Width="300px"></asp:TextBox>
                                  </td>
                              </tr>
					<tr>
						<td   >
                                            DU Madre/Tutor:</td>
						<td align="left" >
                                            <input title="Ingrese el documento unico del parentesco" id="txtDniMadre" type="text" runat="server"  class="form-control input-sm"
                                 onblur="valNumero(this)" style="width: 150px" maxlength="8"/>
                                            <asp:CustomValidator ID="cvDNIMadre" runat="server" ErrorMessage="Numero " onservervalidate="cvDNIMadre_ServerValidate" ValidationGroup="2">Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                        </td>
						
					</tr>
					<tr>
						<td  >
                                            Apellido Madre/Tutor:</td>
						<td align="left" >
                                            <asp:TextBox ID="txtApellidoMadre" runat="server" class="form-control input-sm" TabIndex="4" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
				
					          <tr>
                                  <td >Nombres/s Madre/Tutor:</td>
                                  <td align="left">
                                      <asp:TextBox ID="txtNombreMadre" runat="server" class="form-control input-sm" TabIndex="4" ToolTip="Ingrese el apellido del paciente" Width="300px"></asp:TextBox>
                                  </td>
                              </tr>
				
					          <tr>
                                  <td>&nbsp;</td>
                                  <td align="left">
                                      <asp:Button ID="btnBuscarMas" runat="server" CssClass="btn btn-primary" onclick="btnBuscarMas_Click" TabIndex="5" Text="Buscar Adicional" ToolTip="Buscar Persona" ValidationGroup="2" Visible="False" Width="150px" />
                                  </td>
                              </tr>
				
					</table>
					    </asp:Panel>
                        </td>
					</tr>
                                                
					<tr>
						<td  colspan="4" >					

                             
                            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Text="Se encontraron los siguientes datos para el dni ingresado:" Visible="False"></asp:Label>
                            <asp:GridView ID="gvLista" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="1" CssClass="table table-bordered bs-table" DataKeyNames="idPaciente" EmptyDataText="No se encontraron pacientes para los parametros de busqueda ingresados" Font-Size="9pt" ForeColor="#666666" GridLines="Horizontal" onpageindexchanging="gvLista_PageIndexChanging" onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" PageSize="13" Width="100%">
               <%--<RowStyle BackColor="#F7F6F3" ForeColor="Black" Font-Names="Arial" 
                Font-Size="8pt" />--%>
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
                                                <asp:Button ID="Protocolo" runat="server" class="btn btn-primary" Text="Protocolo" Width="100px " />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="Consentimiento" runat="server" class="btn btn-info" Text="Consentimiento" Width="120px " />
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
						
						<td align="right" >
        <br />
        
        </td>
                                                   
						
					</tr>
					<tr>
						<td  >
                                            &nbsp;</td>
						<td align="right" colspan="2">
                                          <%--<RowStyle BackColor="#F7F6F3" ForeColor="Black" Font-Names="Arial" 
                Font-Size="8pt" />--%>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="3">
                            &nbsp;</td>
						
					</tr>
					

				
                                                
                                            </table>
                        </div>
                    
                                  </div>
                    
                    </td>
                        
						
						<td rowspan="10" style="vertical-align: top" >
                                     <uc1:PeticionList runat="server" ID="PeticionList" /></td>
					</tr>
					
					</table>

</div>
		  
    	
 </asp:Content>
