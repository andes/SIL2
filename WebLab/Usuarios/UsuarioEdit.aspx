<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsuarioEdit.aspx.cs" Inherits="WebLab.Usuarios.UsuarioEdit" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     
     
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
    <!-- CSS -->
<link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="../script/chosen/chosen.css" />

    <!-- JS -->
<script src="https://code.jquery.com/jquery-1.8.3.min.js" type="text/javascript"></script>
<script src="../script/jquery-ui.min.js" type="text/javascript"></script>

<script src="../script/chosen/chosen.jquery.js" type="text/javascript"></script>
   
<script type="text/javascript">
  $(function() {

    $("#tabContainer").tabs();
        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
    $("#tabContainer").tabs({ selected: currTab });
  });
       
</script>
   
   
  
   
    <style type="text/css">
        .auto-style1 {
            width: 93px;
            height: 38px;
        }
        .auto-style2 {
            width: 497px;
            height: 38px;
        }
    </style>
   <script type="text/javascript">
     

       function inicializarChosen() {
           
          var select = $('#<%= ddlEfector3.ClientID %>');

           if (select.data('chosen')) { 
               select.chosen('destroy'); //El destroy evita que Anthem duplique la lista después de un callback.
           }

           //convierte el select en un dropdown Chosen
           select.chosen({
               width: "250px",
               search_contains: true,
               no_results_text: "No se encontraron resultados:"
           });
       }

       function habilitarListaEfectores() {

           var select = $('#<%= ddlEfector3.ClientID %>');
           var esAdministrador = $('#<%= chkAdministrador.ClientID %>').is(':checked');

           // destruir chosen SI existe
           if (select.data('chosen')) {
               select.chosen('destroy');
           }
           // cambiar estado real del select
           select.prop("disabled", esAdministrador);

           // recrear chosen (lee el disabled correctamente)
           select.chosen({
               width: "250px",
               search_contains: true,
               no_results_text: "No se encontraron resultados:",
               allow_single_deselect: true
           });
       }


       $(document).ready(function () {
           inicializarChosen();
           habilitarListaEfectores(); 
       });

       // Reaplicar después de callbacks Anthem
       if (typeof Anthem !== "undefined") {
           Anthem.addCallbackComplete(function () {

               habilitarListaEfectores();

           });

       }

      
   </script>
   
  
   
    </asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
    
  
    
<asp:HiddenField runat="server" ID="HFCurrTabIndex"   /> 
        <div align="center" class="form-inline" style="width:800px;"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"> USUARIO 
                        </h3>
                        </div>
       	<div class="panel-body">	
      <div id="tabContainer" style="border: 0px solid #C0C0C0">
 <ul class="myLabel">
    <li><a href="#tab1">Datos Generales</a></li>       
    <li><a href="#tab2">Efectores</a></li>
   
</ul>

    <div id="tab1" class="tab_content" style="border: 1px solid #C0C0C0">
        <table style="width:700px">
            
            
            <tr>
                <td   style="vertical-align: top; width: 93px;">
                    Apellido:</td>
                <td style="width: 497px"  >
                    <asp:TextBox ID="txtApellido" runat="server" Width="350px" class="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server" 
                        ControlToValidate="txtApellido" ErrorMessage="Apellido" 
                        ValidationGroup="0">*</asp:RequiredFieldValidator>
                    </td>
            </tr>
            <tr>
                <td  style="width: 93px">
                    Nombres:</td>
                <td style="width: 497px"  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="350px" class="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                        ControlToValidate="txtNombre" ErrorMessage="Nombres" ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td  style="width: 93px; vertical-align: top;">
                    Firma Validación:</td>
                <td style="width: 497px"  >
                    <asp:TextBox ID="txtFirmaValidacion" runat="server" Width="350px" class="form-control input-sm"
                        MaxLength="50" Rows="3" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <%-- <tr>
                <td  style="width: 70px">
                    Matricula:</td>
                <td  >
                    <asp:TextBox ID="txtMatricula" runat="server" Width="150px" CssClass="form-control input-sm"  
                        MaxLength="50"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td  style="width: 93px; vertical-align: top;">
                    Email:</td>
                <td style="width: 497px"  >
                    <input type="email" runat="server" id="email" class="form-control input-sm"
                        pattern="[^@\s]+@[^@\s]+\.[^@\s]+" size="30" maxlength="100">
                    
                </td>
            </tr>
            <tr>
                <td  style="width: 93px; vertical-align: top;">
                    Telefono Contacto:</td>
                <td style="width: 497px"  >
                    <asp:TextBox ID="txtTelefono" runat="server" Width="350px" class="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td  colspan="2">
                    <hr /></td>
            </tr>
             <tr>
                <td style="width: 93px">Tipo Autenticacion:</td>
                <td  style="width: 497px" ><asp:DropDownList ID="ddlTipoAutenticacion" runat="server" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoAutenticacion_SelectedIndexChanged">
                     <asp:ListItem Value="SIL" Text="SIL" ></asp:ListItem>
                     <asp:ListItem Value="ONELOGIN" Text="ONELOGIN"></asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td  style="width: 93px">
                    Usuario:</td>
                <td style="width: 497px"  >
                    <asp:TextBox ID="txtUsername" runat="server" Width="350px" class="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                        ControlToValidate="txtUsername" ErrorMessage="Usuario" ValidationGroup="0">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="customValidacionGeneral" runat="server" ErrorMessage="Usuario Existente" OnServerValidate="customValidacionGeneral_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                    <asp:CustomValidator ID="customValidacionGeneral0" runat="server" ErrorMessage="" OnServerValidate="customValidacionGeneral0_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                </td>
            </tr>
            <tr><td  style="width: 93px"></td>
                <td style="width: 497px"><asp:CustomValidator ID="customValidacionGeneral1" runat="server" ErrorMessage="Usuario no puede contener letras ni caracteres especiales" OnServerValidate="customValidacionGeneral1_ServerValidate1" ValidationGroup="0"></asp:CustomValidator></td></tr>
            <tr>
                <td  style="width: 93px">
                   <asp:Label Text="Contraseña:" runat="server" ID="lblPassword"></asp:Label></td>
                <td style="width: 497px"  >
                    <asp:TextBox ID="txtPassword" runat="server" Width="350px" TextMode="Password" 
                      class="form-control input-sm" MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                        ControlToValidate="txtPassword" ErrorMessage="Contraseña" 
                        ValidationGroup="0">*</asp:RequiredFieldValidator>

                </td>
            </tr>
            <tr>
                <td  style="width: 93px">
                    Administrador:</td>
                <td style="width: 497px"  >
                    <anthem:checkbox ID="chkAdministrador" runat="server" Text="SI" AutoCallBack="True" onclick="habilitarListaEfectores();" OnCheckedChanged="chkAdministrador_CheckedChanged" />
                </td>
            </tr>
           <%-- <tr>
                <td class="auto-style1">
                    Efector</td>
                <td class="auto-style2"  >
                     <anthem:dropdownlist ID="ddlEfector" runat="server" Width="300px" class="form-control input-sm">
                    </anthem:dropdownlist>
                     <anthem:rangevalidator ID="rvEfector" runat="server" ControlToValidate="ddlEfector" 
                        ErrorMessage="Efector" MaximumValue="999999" MinimumValue="1" Type="Integer" 
                        ValidationGroup="0">*</anthem:rangevalidator>
                </td>
            </tr>--%>
            <tr>
                <td  style="width: 93px">
                    Area/Laboratorio:</td>
                <td style="width: 497px"  >
                     <anthem:dropdownlist ID="ddlArea" runat="server" Width="200px" class="form-control input-sm">
                     </anthem:dropdownlist>
                </td>
            </tr>
            <tr>
                <td  style="width: 93px">
                    Perfil:</td>
                <td style="width: 497px"  >
                      <anthem:dropdownlist ID="ddlPerfil" runat="server" CssClass="form-control input-sm" AutoCallBack="True" OnSelectedIndexChanged="ddlPerfil_SelectedIndexChanged" >
                       </anthem:dropdownlist>
                      <anthem:rangevalidator ID="rvPerfil" runat="server" ControlToValidate="ddlPerfil" 
                        ErrorMessage="Perfil" MaximumValue="999999" MinimumValue="1" Type="Integer" 
                        ValidationGroup="0">*  </anthem:rangevalidator>
                     <anthem:dropdownlist ID="ddlEfectorDestino" runat="server" Width="200px" class="form-control input-sm" Visible="False">
                    </anthem:dropdownlist>
                     <anthem:rangevalidator ID="rvEfectorDestino" runat="server" ControlToValidate="ddlEfectorDestino" 
                        ErrorMessage="Efector Destino - Laboratorio" MaximumValue="999999" MinimumValue="1" Type="Integer" 
                        ValidationGroup="0" Enabled="False">*</anthem:rangevalidator>
                </td>
            </tr>
            <tr>
                <td  style="width: 93px">
                    Activo:</td>
                <td style="width: 497px"  >
                    <asp:CheckBox ID="chkActivo" runat="server" Checked="True" />
                </td>
            </tr>
         
            <tr>
                <td  colspan="2">
                    <hr /></td>
            </tr>
             <tr>
                <td colspan="2">
				  <asp:CheckBox ID="chkExterno" runat="server" Text="Exclusivo Rio Negro" />
                     <asp:CheckBox ID="chkRequiereContrasenia" runat="server" Checked="True" Text="Requiere nueva contraseña al ingresar" Font-Bold="True" />
                </td>
            </tr>
             <tr>  
                <td colspan="2">
                    <asp:CustomValidator ID="customValidatorExterno" runat="server" OnServerValidate="customValidator_ServerValidate" 
                           ErrorMessage="" ValidationGroup="0"  />
                </td>
            </tr>  
             <tr>  
                <td colspan="2">
                    <asp:CustomValidator ID="customValidatorEfector" runat="server" OnServerValidate="customValidatorEfector_ServerValidate" 
                           ErrorMessage="" ValidationGroup="0"  />
                </td>
            </tr>  
            <tr>
                <td>
                     <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                onclick="btnRegresar_Click" CausesValidation="False">Regresar</asp:LinkButton></td>
                <td align="right" style="width: 497px">
                    <asp:Button ID="btnGuardar" runat="server" Text="Grabar" 
                        onclick="btnGuardar_Click1" CssClass="btn btn-primary" Width="100px" ValidationGroup="0" />
                   
                </td>
            </tr>
            <tr>
                <td>
                     &nbsp;</td>
                <td align="right" style="width: 497px">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        HeaderText="De completar datos requeridos:" ShowMessageBox="True" 
                        ShowSummary="False" ValidationGroup="0" />
                   
                </td>
            </tr>
            <tr>
                <td>
                     &nbsp;</td>
                <td align="right" style="width: 497px">
                   
                       <asp:Button ID="btnAuditoria" runat="server" Text="Auditoria" 
                        onclick="btnAuditoria_Click" CssClass="btn btn-success" Width="100px" />
                    <asp:Button ID="btnBlanquear" runat="server" Text="Blanquear Contraseña" Visible="false"
                        onclick="btnBlanquear_Click" CssClass="btn btn-primary" Width="250px"  />
                   
                </td>
            </tr>
            </table>
               </div>

           <div id="tab2" class="tab_content" style="border: 1px solid #C0C0C0">
       <div class="panel-body">	
           <table>
            <tr>
                <td colspan="2">
                                         
                   <%--  <anthem:dropdownlist ID="ddlEfector2" runat="server" Width="200px" class="form-control input-sm">
                    </anthem:dropdownlist> --%>

                    <asp:DropDownList   ID="ddlEfector3" runat="server"    Width="200px"> </asp:DropDownList>

                    <anthem:Button ID="btnAgregarEfector" runat="server" Text="Agregar"   onclick="btnAgregarEfector_Click" CssClass="btn btn-primary" Width="100px" />

                  
                </td>
             
            </tr>
             <tr>
                 <td>
                     <anthem:Label ID="lblMensajeEfector" runat="server" Visible="false" Text="Label" Font-Bold="True" ForeColor="#CC0000"></anthem:Label>
                 </td>
             </tr>
          
            <tr>
                <td >
                        <anthem:GridView ID="gvListaEfector" runat="server" AutoGenerateColumns="False" 
                            DataKeyNames="idEfector" Font-Size="12pt" Width="100%"
                            ForeColor="#333333" EmptyDataText="Agregue al menos un efector"
                            CellPadding="0"
                            BorderColor="#3A93D2" BorderStyle="Solid" BorderWidth="1px"
                            GridLines="Horizontal" OnRowCommand="gvListaEfector_RowCommand" OnRowDataBound="gvListaEfector_RowDataBound">
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <Columns>
                               <asp:BoundField DataField="nombre" 
                                    HeaderText="Efector" >
                                    <ItemStyle Width="90%" />
                                </asp:BoundField>
                              <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                                                CommandName="Eliminar" />
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
                        <br />
                </td>
               
            </tr>
            <tr>
                <td>
                     <asp:LinkButton ID="lnkRegresar1" runat="server" CssClass="myLink"  onclick="btnRegresar_Click" CausesValidation="False">Regresar</asp:LinkButton></td>
                 
            </tr>
        </table>
       </div>
               
</div>
    </div>
    </div>

       </div>

            </div>
 
 </asp:Content>