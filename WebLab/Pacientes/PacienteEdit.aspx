<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="PacienteEdit.aspx.cs" Inherits="WebLab.Pacientes.PacienteEdit" ValidateRequest="false" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

<title>LABORATORIO</title> 
<link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
 <link href="../App_Themes/default/style.css" rel="stylesheet" type="text/css" />  
     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 
         <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
   
<script type="text/javascript">
    $(function() {

                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                      
                        $("#tabContainer").tabs({ selected: currTab });
             });
             
               $(function() {
                   $("#<%=txtFalta.ClientID %>").datepicker({
                       maxDate: 0,
                       minDate: null,

                       showOn: 'button',
                       buttonImage: '../App_Themes/default/images/calend1.jpg',
                       buttonImageOnly: true
                   });
               });

             
             $(function() {
                 $("#<%=txtFechaNac.ClientID %>").datepicker({
                     maxDate: 0,
                     minDate: null,

                     showOn: 'button',
                     buttonImage: '../App_Themes/default/images/calend1.jpg',
                     buttonImageOnly: true
                 });
             });

	$(function() {
	    $("#<%=txtFechaN.ClientID %>").datepicker({
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
    <br />   &nbsp;
       <asp:HiddenField runat="server" ID="HFCurrTabIndex"   />
  <br />
    <table style="width:800px">
    <tr>
						<td><b class="mytituloTabla">Paciente</b></td>
						<td align="right"> <asp:LinkButton ID="lnkValidarRenaper" runat="server"   ToolTip="Validar Persona con Renaper" OnClientClick="SelRenaper(); return false;" >
                                             <span class="glyphicon glyphicon-registration-mark"></span>Validar con Renaper</asp:LinkButton>  </td>
						<td align="right"> &nbsp;</td>
					</tr>
			<tr>
						<td colspan="2"><hr class="hrTitulo" /></td>
						<td>&nbsp;</td>
					</tr>
    
    <tr>
    	<td colspan="2">
 <div id="tabContainer" style="border: 0px solid #C0C0C0">
 
             <ul class="myLabel">
                <li><a href="#tab1">Datos Identificatorios</a></li>       
                <li><a href="#tab2">Datos del Domicilio</a></li>
                <li><a href="#tab3">Datos de la Madre/Tutor</a></li>
            </ul>


            <div  id="tab1" class="tab_content" style="border: 1px solid #C0C0C0">
                    <anthem:Panel ID="Panel1"  runat="server" Width="600px">
                        <table class="myLabelIzquierda" style="width: 500px">
                        <tr>
                           <td >
                                    Estado:
                             </td>
                             <td>
                                    <asp:DropDownList ID="ddlEstadoP" runat="server" AutoPostBack="true" 
                                        DataTextField="nombre" DataValueField="idEstado" 
                                        OnSelectedIndexChanged="ddlEstadoP_SelectedIndexChanged" TabIndex="1">
                                    </asp:DropDownList>
                                   </td>
                        </tr>
                            <tr>
                                <td>Motivo NI:</td>
                                <td>
                                    <asp:DropDownList Width="100px" ID="ddlMotivoNI" runat="server" AutoPostBack="true" class="form-control input-sm" DataTextField="nombre" DataValueField="idMotivoNI" TabIndex="2">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblMensaje" runat="server"></asp:Label>
                                </td>
                    </tr>
                            <tr>
                                <td >
                                    DU:   <asp:HiddenField runat="server" ID="HFIdPaciente" />     
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroDocumento" runat="server"
                                          MaxLength="9"  class="form-control input-sm" Width="100px"
                                        TabIndex="3" ToolTip="Solo números" 
                          AutoPostBack="true"></asp:TextBox>
                                    <asp:CompareValidator ID="cvNroDoc" runat="server" 
                                        ControlToValidate="txtNumeroDocumento" ErrorMessage="Solo numeros" 
                                        Operator="DataTypeCheck" Type="Integer" />
                                    <asp:RequiredFieldValidator ID="rfvDI" runat="server" 
                                        ControlToValidate="txtNumeroDocumento" Display="Dynamic" 
                                        ErrorMessage="Documento" SetFocusOnError="true" ValidationGroup="I">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvDV" runat="server" 
                                        ControlToValidate="txtNumeroDocumento" Display="Dynamic" 
                                        ErrorMessage="Documento" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Fecha Alta: </td>
                                <td  >
                                     <input id="txtFalta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="44"  class="form-control input-sm"
                                style="width:100px" />
                                
                                    <asp:RequiredFieldValidator ID="rfvfaI" runat="server" 
                                        ControlToValidate="txtFalta" Display="Dynamic" ErrorMessage="Fecha de Alta" 
                                        SetFocusOnError="true" ValidationGroup="I">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvfaV" runat="server" 
                                        ControlToValidate="txtFalta" Display="Dynamic" ErrorMessage="Fecha de Alta" 
                                        SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvfaT" runat="server" 
                                        ControlToValidate="txtFalta" Display="Dynamic" ErrorMessage="Fecha de Alta" 
                                        SetFocusOnError="true" ValidationGroup="T">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvfaBB" runat="server" 
                                        ControlToValidate="txtFalta" Display="Dynamic" ErrorMessage="Fecha de Alta" 
                                        SetFocusOnError="true" ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Apellido:
                                </td>
                                <td>    
                                    <asp:TextBox ID="txtApellido" runat="server"  class="form-control input-sm"  TabIndex="4" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvI" runat="server" ControlToValidate="txtApellido"
                                        Display="Dynamic" ErrorMessage="Apellido" SetFocusOnError="true" 
                                        ValidationGroup="I">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvV" runat="server" ControlToValidate="txtApellido"
                                        Display="Dynamic" ErrorMessage="Apellido" SetFocusOnError="true" 
                                        ValidationGroup="V">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvT" runat="server" ControlToValidate="txtApellido"
                                        Display="Dynamic" ErrorMessage="Apellido" SetFocusOnError="true" 
                                        ValidationGroup="T">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvTBB" runat="server" ControlToValidate="txtApellido"
                                        Display="Dynamic" ErrorMessage="Apellido" SetFocusOnError="true" 
                                        ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                    <asp:Label ID="lblNombres" runat="server" CssClass="myLabelRojo" Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td    >
                                    Nombre:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombre" runat="server"  class="form-control input-sm"  TabIndex="5" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv4I" runat="server" ControlToValidate="txtNombre"
                                        Display="Dynamic" ErrorMessage="Nombre" SetFocusOnError="true" 
                                        ValidationGroup="I">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv4V" runat="server" ControlToValidate="txtNombre"
                                        Display="Dynamic" ErrorMessage="Nombre" SetFocusOnError="true" 
                                        ValidationGroup="V">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv4T" runat="server" ControlToValidate="txtNombre"
                                        Display="Dynamic" ErrorMessage="Nombre" SetFocusOnError="true" 
                                        ValidationGroup="T">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv4TBB" runat="server" ControlToValidate="txtNombre"
                                        Display="Dynamic" ErrorMessage="Nombre" SetFocusOnError="true" 
                                        ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    F. Nacimiento:
                                </td>
                                <td>
                                
                                 <input id="txtFechaNac" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="6"  class="form-control input-sm"
                                style="width: 100px" />
                                
                                   
                                    <asp:RequiredFieldValidator ID="rfv6I" runat="server" ControlToValidate="txtFechaNac"
                                        ErrorMessage="Fecha de Nacimiento" SetFocusOnError="true" ValidationGroup="I">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv6V" runat="server" ControlToValidate="txtFechaNac"
                                        ErrorMessage="Fecha" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv6TBB" runat="server" ControlToValidate="txtFechaNac"
                                        ErrorMessage="Fecha" SetFocusOnError="true" ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    Sexo:</td>
                                <td >
                                    <asp:DropDownList ID="ddlSexo" runat="server" OrderField="idSexo"  class="form-control input-sm" Width="120px"
                                        PromptText="Seleccionar" ShowPrompt="true" TabIndex="7" TableName="Sys_Sexo" 
                                        TextField="nombre">
                                    </asp:DropDownList>
                                    
                                                               



                                        <asp:RangeValidator ID="rfv5I" runat="server" 
                                        ErrorMessage="Sexo" ControlToValidate="ddlSexo" MaximumValue="9999" 
                                        MinimumValue="1" Type="Integer" ValidationGroup="I"></asp:RangeValidator>
                                        
                                                                                                                                        
                                        <asp:RangeValidator ID="rfv5V" runat="server" 
                                        ErrorMessage="Sexo" ControlToValidate="ddlSexo" MaximumValue="9999" 
                                        MinimumValue="1" Type="Integer" ValidationGroup="V"></asp:RangeValidator>
                                        
                                        
                                        <asp:RangeValidator ID="rfv5T" runat="server" 
                                        ErrorMessage="Sexo" ControlToValidate="ddlSexo" MaximumValue="9999" 
                                        MinimumValue="1" Type="Integer" ValidationGroup="T"></asp:RangeValidator>
                                        
                                    
                                        
                                        <asp:RangeValidator ID="rfv5TBB" runat="server" 
                                        ErrorMessage="Sexo" ControlToValidate="ddlSexo" MaximumValue="9999" 
                                        MinimumValue="1" Type="Integer" ValidationGroup="TBB"></asp:RangeValidator>
                                       <asp:HiddenField runat="server" ID="HFNumeroDocumento" />
                                                        <asp:HiddenField runat="server" ID="HFSexo" />
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    Estado Civil:
                                </td>
                                <td  ">
                                    <asp:DropDownList ID="ddlECivil" runat="server" OrderField="idEstadoCivil"  class="form-control input-sm" Width="120px"
                                        ShowPrompt="false" TableName="Sys_EstadoCivil" TextField="nombre" 
                                        TabIndex="8">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    ID Paciente Hospital:
                                </td>
                                <td >
                                    <asp:TextBox ID="txtHC" runat="server"  class="form-control input-sm" Width="50px"  TabIndex="9" 
                                        AutoPostBack="true" OnTextChanged="VerificarHClinica" 
                                        ToolTip="Solo números"></asp:TextBox>
                                    <asp:CompareValidator ID="cvHClinica" runat="server" ControlToValidate="txtHC" 
                                        ErrorMessage="*" Operator="DataTypeCheck" ToolTip="Solo números" 
                                        Type="Integer" />
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Label ID="lblOBl" runat="server"   Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblOS" runat="server"   Visible="false"></asp:Label>
                                </td>
                            </tr>
                          
                            <tr>
                                <td  >
                                    Inf. Contacto:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContacto" runat="server"   class="form-control input-sm" MaxLength="500" 
                                        Rows="2" TabIndex="10" TextMode="MultiLine" Width="97%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv8" runat="server" 
                                        ControlToValidate="txtContacto" Display="Dynamic" ErrorMessage="Contacto" 
                                        SetFocusOnError="true" ValidationGroup="I">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv8V" runat="server" 
                                        ControlToValidate="txtContacto" Display="Dynamic" ErrorMessage="Contacto" 
                                        SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv8T" runat="server" 
                                        ControlToValidate="txtContacto" Display="Dynamic" ErrorMessage="Contacto" 
                                        SetFocusOnError="true" ValidationGroup="T">*</asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfv8TBB" runat="server" 
                                        ControlToValidate="txtContacto" Display="Dynamic" ErrorMessage="Contacto" 
                                        SetFocusOnError="true" ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                         
                           
                        </table>
                    </anthem:Panel>
                </div>
             <div id="tab2" class="tab_content" style="border: 1px solid #C0C0C0">
                    <anthem:Panel ID="Panel2"  runat="server" Width="800px">
                        <table width="100%"  class="myLabelIzquierda">
                        
                        
                            <tr>
                                <td>                                    
                                        Provincia:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlProvinciaDomicilio" runat="server"    AutoCallBack="True"
                                        DataTextField="nombre" DataValueField="idProvincia" TabIndex="10"    OnSelectedIndexChanged="ddlProvinciaDomicilio_SelectedIndexChanged"
                                        ToolTip="Seleccionar la Provincia">
                                    </anthem:DropDownList>
                                   <%-- <asp:RequiredFieldValidator ID="rfvv1" runat="server" 
                                        ControlToValidate="ddlProvinciaDomicilio" Display="Dynamic" 
                                        ErrorMessage="Provincia" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Departamento:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlDepartamento" runat="server" DataTextField="nombre"     AutoCallBack="True"
                                        DataValueField="idDepartamento" 
                                        TabIndex="11" ToolTip="Seleccionar el Departamento" 
                                        OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged">
                                    </anthem:DropDownList>
                                  <%--  <asp:RequiredFieldValidator ID="rfvv2" runat="server" ControlToValidate="ddlDepartamento"
                                        Display="Dynamic" ErrorMessage="Departamento" SetFocusOnError="true" 
                                        ValidationGroup="V">*</asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Localidad:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlLocalidad" runat="server"     AutoCallBack="True"
                                    TabIndex="12" OnSelectedIndexChanged="ddlLocalidad_SelectedIndexChanged"
                                       ToolTip="Seleccionar Localidad" DataTextField="nombre"
                                        DataValueField="idLocalidad">
                                    </anthem:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv3" runat="server" ControlToValidate="ddlLocalidad"
                                        Display="Dynamic" ErrorMessage="Localidad" SetFocusOnError="true" 
                                        ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Barrio:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlBarrio" runat="server" TabIndex="13" 
                                        DataTextField="nombre" DataValueField="idBarrio">
                                    </anthem:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv4" runat="server" ControlToValidate="ddlBarrio"
                                        Display="Dynamic" ErrorMessage="Barrio" SetFocusOnError="true" 
                                        ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Calle:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCalle" runat="server"   TabIndex="14" 
                                        Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvv5" runat="server" ControlToValidate="ddlProvinciaDomicilio"
                                        Display="Dynamic" ErrorMessage="Calle" SetFocusOnError="true" 
                                        ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Número:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumero" runat="server"   Width="40px" TabIndex="15"></asp:TextBox>
                                    
                                     <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                        ControlToValidate="txtNumero" ErrorMessage="Solo numeros" 
                                        Operator="DataTypeCheck" Type="Integer"  ValidationGroup="T"/>
                                        
                                    <asp:CompareValidator ID="cvCompareNumeroDomicilio" runat="server" 
                                        ControlToValidate="txtNumero" ErrorMessage="Solo numeros" 
                                        Operator="DataTypeCheck" Type="Integer"  ValidationGroup="I"/>
                                        
                                         <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                        ControlToValidate="txtNumero" ErrorMessage="Solo numeros" 
                                        Operator="DataTypeCheck" Type="Integer"  ValidationGroup="V"/>
                                        
                                    <asp:RequiredFieldValidator
                                        ID="rfvv6" runat="server" ControlToValidate="txtNumero" Display="Dynamic"
                                        ErrorMessage="Numero" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                    Piso:<asp:TextBox ID="txtPiso" runat="server"   TabIndex="16" Width="60px"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="rfvv7" runat="server" 
                                        ControlToValidate="txtPiso" Display="Dynamic" ErrorMessage="Piso" 
                                        SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>--%>
                                </td>
                                <td>
                                    Departamento:<asp:TextBox ID="txtDepartamento" runat="server" Width="40px"
                                          TabIndex="17"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="rfvv8" runat="server" 
                                        ControlToValidate="txtDepartamento" Display="Dynamic" 
                                        ErrorMessage="Departamento" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>--%>
                                    Manzana:<asp:TextBox ID="txtManzana" runat="server"  Width="40px" 
                                        TabIndex="18"></asp:TextBox>
                                   <%-- <asp:RequiredFieldValidator ID="rfvv9" runat="server" 
                                        ControlToValidate="txtManzana" Display="Dynamic" ErrorMessage="Manzana" 
                                        SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Referencia:
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtReferencia"   runat="server" TabIndex="19" 
                                        Rows="3" Width="400px"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator
                                        ID="rfvv10" runat="server" ControlToValidate="txtReferencia" 
                                        Display="Dynamic" ErrorMessage="Referencia"
                                        SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    Nacionalidad:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlNacionalidad" runat="server" TabIndex="20" AutoCallBack="true"
                                       
                                        DataTextField="nombre" DataValueField="idPais" 
                                        onselectedindexchanged="ddlNacionalidad_SelectedIndexChanged1">
                                    </anthem:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv11" runat="server" ControlToValidate="ddlNacionalidad"
                                        Display="Dynamic" ErrorMessage="Nacionalidad" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    Lugar Nac:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlProvincia" runat="server" 
                                        TabIndex="21" ToolTip="Seleccionar la Provincia"
                                        DataTextField="nombre" DataValueField="idProvincia">
                                    </anthem:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv12" runat="server" ControlToValidate="ddlProvincia"
                                        Display="Dynamic" ErrorMessage="LugarNacimiento" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td  >
                                    Profesión:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProfesion" runat="server" TabIndex="22" TableName="Sys_Profesion"
                                        TextField="nombre" OrderField="idProfesion">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv13" runat="server" ControlToValidate="ddlProfesion"
                                        Display="Dynamic" ErrorMessage="Profesion" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nivel de Instrucción:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlNivelInstruccion" runat="server" TableName="Sys_NivelInstruccion"
                                        TabIndex="23" TextField="nombre" OrderField="idNivelInstruccion">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv14" runat="server" ControlToValidate="ddlNivelInstruccion"
                                        Display="Dynamic" ErrorMessage="NivelInstruccion" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Situación Laboral:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSituacionLaboral" TabIndex="24" runat="server" TableName="Sys_SituacionLaboral"
                                        TextField="nombre" OrderField="idSituacionLaboral">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv15" runat="server" ControlToValidate="ddlSituacionLaboral"
                                        Display="Dynamic" ErrorMessage="SituacionLaboral" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    Ocupación:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlOcupacion" TabIndex="25" runat="server" TableName="Sys_Ocupacion"
                                        TextField="nombre" OrderField="idOcupacion">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvv16" runat="server" ControlToValidate="ddlOcupacion"
                                        Display="Dynamic" ErrorMessage="Ocupacion" SetFocusOnError="true" ValidationGroup="V">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                          <%--  <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Defunción:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDefuncion" runat="server"   onblur="javascript:formatearFecha(this)" TabIndex="26"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Paciente Crónico:
                                    <asp:CheckBox ID="ckbCronico" runat="server" Checked="false" TabIndex="27" />
                                </td>
                                <td>    
                                  Activo:
                                    <asp:CheckBox ID="ckbActivo" runat="server" Checked="true" TabIndex="28" />
                                </td>
                            </tr>--%>
                        </table>
                    </anthem:Panel>
                </div>
            
              <div id="tab3" class="tab_content" style="border: 1px solid #C0C0C0">
                    <anthem:Panel ID="Panel3"  runat="server" Width="800px">
                        <table width="100%"  class="myLabelIzquierda">
                            <tr>
                                <td  >
                                    Parentesco:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlParentesco" runat="server" ShowPrompt="false" TabIndex="29">
                                        <asp:ListItem Value="Madre">Madre</asp:ListItem>
                                        <asp:ListItem Value="Padre">Padre</asp:ListItem>
                                        <asp:ListItem Value="Otro">Otro</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblIdParentesco" Visible="false" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    DU:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumeroP" runat="server" MaxLength="8" TabIndex="30" 
                                        ToolTip="Solo números"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rffv3" runat="server" 
                                        ControlToValidate="txtNumeroP" Display="Dynamic" 
                                        ErrorMessage="Documento de la Madre/Tutor" SetFocusOnError="true" 
                                        ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td  >
                                    Apellido:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApellidoP" runat="server"  Width="400px" TabIndex="31"></asp:TextBox><asp:RequiredFieldValidator
                                        ID="rffv1" runat="server" ControlToValidate="txtApellidoP" 
                                        Display="Dynamic" ErrorMessage="Apellido de la Madre/Padre"
                                        SetFocusOnError="true" ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nombre:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNombreP" runat="server"
                                        TabIndex="32" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rffv2" runat="server" 
                                        ControlToValidate="txtNombreP" Display="Dynamic" 
                                        ErrorMessage="Nombre de la Madre/Padre" SetFocusOnError="true" 
                                        ValidationGroup="TBB">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Nac:</td>
                                <td>
                                    <%--  <asp:DropDownList ID="ddlTipoDocP" runat="server" TableName="Sys_TipoDocumento"
                                        TabIndex="32" TextField="nombre" ShowPrompt="false" OrderField="idTipoDocumento">
                                    </asp:DropDownList>--%>
                                    <input ID="txtFechaN" runat="server" class="myTexto" maxlength="10" 
                                        onblur="valFecha(this)" onkeyup="mascara(this,'/',patron,true)" 
                                        style="width: 70px" tabindex="33" type="text" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Nacionalidad:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlNacionalidadP" runat="server" TabIndex="34" DataValueField="idPais"
                                        DataTextField="nombre" AutoCallBack="true" 
                                        OnSelectedIndexChanged="ddlNacionalidadP_SelectedIndexChanged" >
                                    </anthem:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Lugar Nac:
                                </td>
                                <td>
                                    <anthem:DropDownList ID="ddlProvinciaP" runat="server" DataTextField="nombre" 
                                        DataValueField="idProvincia" TabIndex="35">
                                    </anthem:DropDownList>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td>
                                    Niv. Instrucción:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlNIvelInstruccionP" runat="server" TabIndex="37" TableName="Sys_NivelInstruccion"
                                        TextField="nombre" OrderField="idNivelInstruccion">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Situación Laboral:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSituacionLaboralP" runat="server" TabIndex="38" TableName="Sys_SituacionLaboral"
                                        TextField="nombre" OrderField="idSituacionLaboral">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Profesión:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlProfesionP" runat="server" TableName="Sys_Profesion" TabIndex="39"
                                        TextField="nombre" OrderField="idProfesion">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                        </table>
                    </anthem:Panel>
                </div>
</div>
    </td>
    	<td>
                    

                

    	</td>
    </tr>
    
    <tr>
   	<td colspan="2" align="right">
      <asp:ValidationSummary ID="VST" runat="server" HeaderText="Completar Información:"
        ValidationGroup="T" ShowMessageBox="True" ShowSummary="False" />
    <asp:ValidationSummary ID="valSum" runat="server" HeaderText="Completar Información del Paciente:"
        ValidationGroup="I" ShowMessageBox="True" ShowSummary="False" />
    <asp:ValidationSummary ID="VS1" runat="server" HeaderText="Completar Otros Datos del Paciente:"
        ValidationGroup="V" ShowMessageBox="True" ShowSummary="true" />
    <asp:ValidationSummary ID="VS2"
        runat="server" HeaderText="Completar Datos de la Madre/Tutor:" 
            ValidationGroup="TBB" />
        
       
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar " TabIndex="40" 
            OnClick="btnGuardar_Click" ValidationGroup="I" CssClass="btn btn-info" Width="100px" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
            OnClick="btnCancelar_Click" CssClass="btn btn-info" TabIndex="50" Width="100px"/>
        <asp:Label EnableViewState="False" ID="Label1" runat="server"></asp:Label>
        
    </td>
   	<td align="right">
        &nbsp;</td>
    </tr>
    </table>

   
  

  
    
    
    
<script language="javascript" type="text/javascript">
      var sexo =        $("#<%= HFSexo.ClientID %>").val(); 
         var numeroDocumento = $("#<%= HFNumeroDocumento.ClientID %>").val();
         var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();
     function SelRenaper() {
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
         $('<iframe src="../Protocolos/ProcesaRenaper.aspx?master=1&Tipo=DNI&sexo=' + sexo + '&dni=' + numeroDocumento + '&id='+idPaciente+'" />').dialog({
             title: 'Valida Renaper',
             autoOpen: true,
             width: 600,
             height: 600,
             modal: true,
             resizable: false,
             autoResize: true,
           open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },
                 buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.lnkValidarRenaper))%>; }               
            },
        
             overlay: {
                 opacity: 0.5,
                 background: "black"
             }
         }).width(600);
     }
  

    </script>
</asp:Content>

   

