﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoriaClinicaFiltro.aspx.cs" Inherits="WebLab.Informes.HistoriaClinicaFiltro" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %> 
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript"> 
      

	$(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
		$("#<%=txtFechaHasta.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});


$(function () {
    $("#<%=txtFechaNac.ClientID %>").datepicker({
        showOn: 'button',
        buttonImage: '../App_Themes/default/images/calend1.jpg',
        buttonImageOnly: true
    });
});
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    
<div align="center" style="width: 1200px" class="form-inline"  >
      <table  width="800px"  
                     style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000000" 
                     cellpadding="1" cellspacing="1" >
<tr><td>
                      <div id="pnlTitulo"  runat="server" class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title"><asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h3>
                        </div>

				<div class="panel-body">	

				 <table  width="790px"  cellpadding="1" cellspacing="1"   >                     
					
					<tr>
						   <td colspan="2" style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;">Datos del Protocolo</td>
						   <td colspan="2" 
                            
                               style="vertical-align: top;" align="right">
                            <asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                ErrorMessage="Numero de Protocolo" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="0" 
                                >Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                           </td>
						   <td 
                            
                               style="vertical-align: top;" align="right">
                               &nbsp;&nbsp; &nbsp;</td>
					</tr>
					<tr>
						<td  style="width: 133px" >Tipo de Servicio:</td>
						<td style="width: 210px">
                            <asp:DropDownList ID="ddlServicio" runat="server" ToolTip="Seleccione el servicio" TabIndex="1"  class="form-control input-sm"></asp:DropDownList>
                            <%--    <asp:RadioButtonList ID="rdbNumero" runat="server" 
                                   RepeatDirection="Horizontal" RepeatLayout="Flow" 
                             >
                                   <asp:ListItem Selected="True" Value="Protocolo">Nro. Protocolo</asp:ListItem>
                                   <asp:ListItem Value="Origen">Nro. Origen</asp:ListItem>
                               </asp:RadioButtonList>--%>
                            </td>
				<td  align="left" >
                    Buscar por:</td>
						<td>
                    <asp:DropDownList ID="ddlNumero" runat="server"  class="form-control input-sm"
                        >
                        <asp:ListItem Selected="True" Value="Protocolo">Nro. Protocolo</asp:ListItem>
                        <asp:ListItem Value="Origen">Nro. de Origen</asp:ListItem>
                        <asp:ListItem Value="Tarjeta">Nro. Tarjeta Neonatal</asp:ListItem>
                    </asp:DropDownList>

                            <asp:TextBox  class="form-control input-sm" MaxLength="9"  ID="txtProtocolo" runat="server" 
                                Width="120px" ToolTip="Ingrese el numero que desea buscar"></asp:TextBox>
                        </td>
						<td>
                            &nbsp;</td>
					</tr>
					<tr>
						<td  style="width: 133px" >Fecha Desde:</td>
						<td style="width: 210px">
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1"  class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de inicio de la búsqueda"  /></td>
				<td  align="left" >
                            Fecha Hasta:</td>
						<td  >
                    <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="2"  class="form-control input-sm"
                                title="Ingrese la fecha de fin de la búsqueda"  /></td>
						<td  >
                            &nbsp;</td>
					</tr>
					<tr>
						<td  colspan="4" >
                            <asp:Panel ID="pnlAnalisis" runat="server">
                                <table style="width:100%;">
                                  <tr>
                                       <td colspan="2" 
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;">
                            <hr />Identificación de la Práctica</td>
                                        </tr>
                                    <tr>
                                        <td >
                                            Area/Sector:
                                        </td>
                                        <td >
                                            <anthem:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" 
                                                 class="form-control input-sm" onselectedindexchanged="ddlArea_SelectedIndexChanged" 
                                                TabIndex="3">
                                            </anthem:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  style="width: 130px" >
                                            Analisis:</td>
                                        <td>
                                            <anthem:TextBox ID="txtCodigo" runat="server" AutoCallBack="True" 
                                                 class="form-control input-sm" ontextchanged="txtCodigo_TextChanged" 
                                                style="text-transform:uppercase" TabIndex="4" Width="88px" 
                                                ToolTip="Si conoce el codigo del analisis ingreselo"></anthem:TextBox>
                                            <anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True" 
                                                 class="form-control input-sm"
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" TabIndex="5">
                                            </anthem:DropDownList>
                                            <anthem:Label ID="lblMensaje" runat="server" ForeColor="#FF3300"> </anthem:Label>                                                               
                                            <anthem:RangeValidator ID="rvAnalisis" runat="server" 
                                                ControlToValidate="ddlItem" ErrorMessage="Debe seleccionar un análisis" 
                                                MaximumValue="9999999" MinimumValue="1" Type="Integer">
                                                Debe seleccionar un análisis
                                            </anthem:RangeValidator>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            
                        </td>
						<td >
                            &nbsp;</td>
					</tr>
					<tr>
						<td  colspan="4" ><hr /></td>
						<td  >&nbsp;</td>
					</tr>
					<tr>
					<td colspan="4" 
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;"> 
                                    <small>    Búsqueda e Identificación del Paciente</small></td>
						
					<td 
                            
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;"> 
                                        &nbsp;</td>
						
					</tr>
					<tr>
						<td  style="width: 133px" >
                                            DU Paciente:  
                                            </td>
                                            <td colspan="3">
                                                          <input id="txtDni"  type="text" runat="server"   class="form-control input-sm"
                                style="width: 150px"
                                 onblur="valNumero(this)" title="Ingrese el documento unico del paciente (DNI)" maxlength="8" tabindex="6"/>
                                             &nbsp;   &nbsp;  
                           
                           
                            <asp:CustomValidator ID="cvDNI" runat="server" 
                                ErrorMessage="Numero " 
                                onservervalidate="cvDNI_ServerValidate" ValidationGroup="0" 
                                >Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                                           
                        </td>
						
                                            <td>
                                                          &nbsp;</td>
						
					</tr>
					<tr>
						<td  style="width: 133px" >
                                            Apellido/s:</td>
						<td align="left" colspan="3">
                                            <asp:TextBox ID="txtApellido" runat="server"  class="form-control input-sm" TabIndex="7" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
						<td align="left">
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td  style="width: 133px" >
                                            Nombres/s:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm"  TabIndex="8" 
                                                Width="300px" ToolTip="Ingrese el nombre del paciente"></asp:TextBox>
                        </td>
						
						<td align="left" >
                                            &nbsp;</td>
						
					</tr>

                    	<tr>
						<td   >
                                            Fecha de Nacim.:</td>
						<td align="left" colspan="3">
                                            <input id="txtFechaNac" runat="server" type="text" maxlength="10" title="Fecha de nacimiento del paciente"
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="9" class="form-control input-sm" 
                                style="width: 100px"  /><asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Fecha de nacimiento incorrecta" 
                                onservervalidate="cvFecha_ServerValidate" ValidationGroup="0">formato inválido</asp:CustomValidator>
                            </td>
						
						<td align="left">
                                            &nbsp;</td>
						
					</tr>
				
					
					<tr>
						<td   >
                                            Sexo:</td>
						<td align="left" colspan="3">
                                            <asp:DropDownList ID="ddlSexo" runat="server" TabIndex="10" class="form-control input-sm" >
                                                <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                                                <asp:ListItem Value="2">Femenino</asp:ListItem>
                                                <asp:ListItem Value="3">Masculino</asp:ListItem>
                                                <asp:ListItem Value="1">Indeterminado</asp:ListItem>
                                            </asp:DropDownList>
                        </td>
						
						<td align="left">
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td  colspan="4" >
                                   <anthem:LinkButton ID="lnkAmpliarFiltros"   CssClass="myLittleLink" runat="server" 
                                                onclick="lnkAmpliarFiltros_Click" Text="Ampliar filtros de búsqueda"></anthem:LinkButton></td>
						
						<td >
                                   &nbsp;</td>
						
					</tr>
					<tr>
						<td  colspan="4" >					
                        <anthem:Panel ID="pnlParentesco" runat="server" Visible="false">
                          <table width="100%">
                                            
					<tr>
						<td  >
                                            Numero Identificacion Adic.</td>
						<td align="left" >
                                                          <input title="Ingrese numero de identificacion adicional" id="txtNumeroAdic" type="text" runat="server"  class="form-control input-sm"
                            style="width: 150px" maxlength="8"/>
                        </td>
						
					</tr>
					<tr>
						<td   >
                                            DU Madre/Tutor:</td>
						<td align="left" >
                                            <input title="Ingrese el documento unico del parentesco" id="txtDniMadre" type="text" runat="server"  class="form-control input-sm"
                                 onblur="valNumero(this)" style="width: 150px" maxlength="8"/>
                                            <asp:CustomValidator ID="cvDNIMadre" runat="server" ErrorMessage="Numero " onservervalidate="cvDNIMadre_ServerValidate" ValidationGroup="0">Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
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
				
					</table>
					    </anthem:Panel>
					    </td>
						<td >					
                            &nbsp;</td>
					</tr>
					
					
					
					</table>
						
                    </div>
                          <div class="panel-footer">
                              <div>
                                                              <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0" 
                                                onclick="btnBuscar_Click" CssClass="btn btn-primary" TabIndex="12" Width="77px" 
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" /> <asp:CustomValidator ID="cvDatosEntrada" runat="server" 
                                                ErrorMessage="Debe ingresar al menos un parametro de busqueda" 
                                                onservervalidate="cvDatosEntrada_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                
                              </div>
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idPaciente" CssClass="table table-bordered bs-table"  PageSize="20" 
                                
                                EmptyDataText="No se encontraron resultados para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                
                                GridLines="Horizontal" >
            
            <Columns>
                <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="60%" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac.">
                     <ItemStyle Width="20%" HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/zoom.png" 
                            CommandName="Editar" ToolTip="Visualizar laboratorios" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle ForeColor="#CC3300" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#3A93D2" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
            />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                   
        <asp:GridView ID="gvListaProducto" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idProtocolo" CssClass="table table-bordered bs-table" PageSize="20" 
                                
                                EmptyDataText="No se encontraron resultados para los parametros de busqueda ingresados" 
                                onrowcommand="gvListaProducto_RowCommand" onrowdatabound="gvListaProducto_RowDataBound" 
                                
                                GridLines="Horizontal" >
            
            <Columns>
                <asp:BoundField DataField="numero" HeaderText="Protocolo" >
                    <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="numeroOrigen" HeaderText="Nro. Origen" >
                    <ItemStyle Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="producto" HeaderText="Producto">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="40%" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaProtocolo" HeaderText="Fecha Protocolo">
                     <ItemStyle Width="20%" HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar0" runat="server" ImageUrl="~/App_Themes/default/images/zoom.png" 
                            CommandName="Editar" ToolTip="Visualizar laboratorios" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle ForeColor="#CC3300" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#3A93D2" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                          </div>
                          </div>



</td></tr>
   
 </table>
   
 </div>
 </asp:Content>