<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebLab.Protocolos.Default" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajx" %>
<%@ Register Src="~/PeticionList.ascx" TagPrefix="uc1" TagName="PeticionList" %>
<%@ Register src="ProtocoloList.ascx" tagname="ProtocoloList" tagprefix="uc1" %>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title> 
<%--<script type="text/javascript" src="../script/Mascara.js"></script>--%>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script> 
  
  
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        document.onkeydown = function (evt) { return (evt ? evt.which : event.keyCode) != 13; }
	        $(function() {
		        $("#<%=txtFechaNac.ClientID %>").datepicker({
			        showOn: 'button',
			        buttonImage: '../App_Themes/default/images/calend1.jpg',
			        buttonImageOnly: true
		        });
	        });

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

    function seleccionarPacienteInternado() {
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
        $('<iframe src="http://intranet.hospitalneuquen.org.ar/scripts/internacion/internacion.dll/mapa?inside=1" style="-moz-box-sizing: border-box; -webkit-box-sizing: border-box; box-sizing: border-box" />').dialog({
            title: 'Mapa de Camas',
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
    function closeDialog(success, historia) {
        var hfPaciente = $("#<%= hfPaciente.ClientID %>").val(historia.codigo);
        <%= GetPostBackEventReference(btnSeleccionarPaciente) %>
    }
 
</script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <br />   &nbsp;
      <ajx:toolkitscriptmanager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
    EnableScriptLocalization="true">
  </ajx:toolkitscriptmanager>
        <div align="left" style="width: 1200px" class="form-inline"  >
 <table  width="1200px" align="center" class="myTabla">
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
                        </div>

				<div class="panel-body">
                                            <table class="myTabla" width="700px"   >
                                            <tr>
						<td colspan="2" rowspan="2" ><strong>Identificación del Persona</strong></td>
						<td colspan="2" align="right" >
                                   <asp:Label ID="lblServicio" runat="server" CssClass="myLabelIzquierdaGde" 
                                       Text="Label"></asp:Label>
                            <asp:Image ID="imgUrgencia" ToolTip="URGENCIA" runat="server" ImageUrl="../App_Themes/default/images/urgencia.jpg" />
                           
                                                </td>
					</tr>
                                           

                    
					<tr>
						
						<td colspan="2" 
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;" 
                            align="right">                                         
                                               <asp:Button ToolTip="Buscar Pacientes Internados " CssClass="myButtonRojo" 
                                                ID="btnSeleccionarPacienteInternado" Width="200px" runat="server" 
                                                Text="Acceder a Pacientes Internados" Visible="false"
                                                OnClientClick="seleccionarPacienteInternado(); return false;" 
                                                onclick="btnSeleccionarPacienteInternado_Click" TabIndex="200" 
                                                   UseSubmitBehavior="False" />
                        </td>
						
					</tr>
					<tr>
						
						<td   colspan="4" 
                            style="color: #FF0000; font-weight: bold; font-size: 12px">
                                          <hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  >
                                            DU Paciente:</td>
						<td align="left" colspan="3">
                            <input id="txtDni" type="text" runat="server"  class="form-control input-sm"
                                onblur="valNumeroSinPunto(this)" maxlength="8" tabindex="0" style="width: 100px"/>
                           
                           
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  >
                                            Apellido/s:</td>
						<td align="left" colspan="3">
                                            <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm" TabIndex="1" 
                                                ValidationGroup="1" Width="300px"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  style="width: 150px">
                                            Nombres/s:</td>
						<td align="left" colspan="3">
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" TabIndex="2" 
                                                Width="300px"></asp:TextBox>
                          
                                                
                                                  
                                              
                        </td>
						
					</tr>
				
					
					<tr>
						<td class="myLabelIzquierda" style="width: 150px" >
                                            Fecha de Nac.:</td>
						<td align="left" colspan="3">
                                            <input id="txtFechaNac" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px"  /><asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Fecha de nacimiento incorrecta" 
                                onservervalidate="cvFecha_ServerValidate" ValidationGroup="0">formato inválido</asp:CustomValidator>
                        </td>
						
					</tr>
				
					
					<tr>
						<td class="myLabelIzquierda"  style="width: 150px">
                                            Sexo:</td>
						<td align="left" colspan="3">
                                            <asp:DropDownList ID="ddlSexo" runat="server" TabIndex="4" class="form-control input-sm">
                                                <asp:ListItem Value="0" Selected="True">Todos</asp:ListItem>
                                                <asp:ListItem Value="2">Femenino</asp:ListItem>
                                                <asp:ListItem Value="3">Masculino</asp:ListItem>
                                                <asp:ListItem Value="1">Indeterminado</asp:ListItem>
                                            </asp:DropDownList>
                        </td>
						
					</tr>
				
					
					<tr>
						<td  colspan="4" >
                                   <anthem:LinkButton ID="lnkAmpliarFiltros"   CssClass="myLittleLink" runat="server" 
                                                onclick="lnkAmpliarFiltros_Click" 
                                       Text="Ampliar filtros de búsqueda" TabIndex="100"></anthem:LinkButton>             <hr /></td>
						
					</tr>
					
					<tr>
						<td  colspan="4" >					
                        <anthem:Panel ID="pnlParentesco" runat="server" Visible="false">
                          <table width="100%">
                                            
					<tr>
						<td class="myLabelIzquierda"  >
                                            DU Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                                          <input id="txtDniMadre" type="text" runat="server"  class="form-control input-sm"
                                 onblur="valNumero(this)" maxlength="8" tabindex="101"/>
                                                          <asp:CompareValidator ID="cvDni0" runat="server" 
                                                              ControlToValidate="txtDniMadre" ErrorMessage="Debe ingresar solo numeros" 
                                                              Operator="DataTypeCheck" Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 150px" >
                                            Apellido Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtApellidoMadre" runat="server" class="form-control input-sm" TabIndex="102" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 150px" >
                                            Nombres/s Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtNombreMadre" runat="server" class="form-control input-sm" TabIndex="103" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
						<tr>
						<td  colspan="4" >
                                           <hr /></td>
						
					</tr>
					</table>
					    </anthem:Panel>
					    </td>
					</tr>
                                                
					<tr>
						<td>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0"  Width="100px"
                                                CssClass="btn btn-primary" TabIndex="5" onclick="btnBuscar_Click" 
                                                ToolTip="Buscar Pacientes en SGH" />
                                            <asp:Button ID="btnNuevoPaciente" runat="server" 
                                                onclick="btnNuevoPaciente_Click" Text="Nuevo Paciente" Visible="False" 
                                                TabIndex="500" />
                        </td>
						<td align="right" colspan="2">
                                            <asp:CustomValidator ID="cvDatosEntrada" runat="server" 
                                                ErrorMessage="Debe ingresar al menos un parametro de busqueda." 
                                                onservervalidate="cvDatosEntrada_ServerValidate" ValidationGroup="0" 
                                                TabIndex="500"></asp:CustomValidator>
                                           
                        </td>
						
						<td align="right" >
                                           <asp:HyperLink ID="hplNuevoPaciente"  runat="server"  CssClass="btn btn-danger" Width="150px" 
                                               ToolTip="Crear un nuevo paciente en el SIPS" Target="_self">Nuevo Paciente</asp:HyperLink>  
                                               <asp:Button ToolTip="Crear un nuevo paciente " CssClass="myButton"  Visible="false"
                                               ID="btnSeleccionarPaciente" Width="150px" runat="server" Text="Nuevo Paciente" 
                                               OnClientClick="seleccionarPaciente(); return false;" 
                                               onclick="btnSeleccionarPaciente_Click" TabIndex="100" 
                                               UseSubmitBehavior="False" />
        <br />
        <asp:HiddenField ID="hfPaciente" runat="server" />
        <asp:Label ID="lblPaciente" runat="server" Text="Label" Visible="False"></asp:Label>
        
        </td>
                                                   
						
					</tr>
					<tr>
						<td  >
                                            &nbsp;</td>
						<td align="right" colspan="3">
                                          <%--<INPUT id="txtOculto" style="WIDTH: 1px; HEIGHT: 1px" type="hidden" size="1" name="txtOculto"
        runat="server">--%>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="4">
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idPaciente" 
                                                Font-Size="9pt" 
                                                Width="100%" CellPadding="1" 
                                ForeColor="#666666" 
                                AllowPaging="True" PageSize="13" 
                                
                                EmptyDataText="No se encontraron pacientes para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                CssClass="table table-bordered bs-table" 
                                GridLines="Horizontal" onpageindexchanging="gvLista_PageIndexChanging">
               <%--<RowStyle BackColor="#F7F6F3" ForeColor="Black" Font-Names="Arial" 
                Font-Size="8pt" />--%>
            <Columns>
             <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                            ommandName="Editar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="18px" HorizontalAlign="Center" Height="18px" />
                          
                        </asp:TemplateField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                 <asp:BoundField DataField="sexo" HeaderText="Sexo">
                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac.">
                     <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                 <asp:TemplateField>
                            <ItemTemplate>
                     
                                 
                                  <div class="btn-group">
  
  <asp:Button id="Protocolo" runat="server" class="btn btn-primary" Text="Protocolo" Width="80px " />&nbsp;&nbsp;
                                        <asp:Button id="Consentimiento" runat="server" class="btn btn-info" Text="Continuar" Width="120px " />
 
</div>
                            </ItemTemplate>
                           <ItemStyle Width="30%" HorizontalAlign="Center" />
                        
                          
                        </asp:TemplateField>

             

                  
                 
            </Columns>
            <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                 />
       
        </asp:GridView>
                        </td>
						
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
