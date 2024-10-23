<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PacienteEdit2.aspx.cs" Inherits="WebLab.Pacientes.PacienteEdit2" MasterPageFile="../Site1.Master" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
   

    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />

       
    
<%--		    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />--%>

     
  
            <style type="text/css">
                .auto-style1 {
                    display: block;
                    width: 100%;
                    height: 34px;
                    padding: 6px 12px;
                    font-size: 14px;
                    line-height: 1.42857143;
                    color: #555;
                    background-color: #fff;
                    background-image: none;
                    border: 1px solid #ccc;
                    border-radius: 4px;
                    -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
                    box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
                    -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
                    -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
                    transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
                    left: 0px;
                    top: 0px;
                }
            </style>



  
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <div class="container">
     <br />
     <div id="pnlTitulo" runat="server" class="panel panel-default">
              
               <h3>  &nbsp;Paciente</h3>
                      
                   
                           

            
         <asp:Label Visible="false" ID="lblTemporal" runat="server" Text="Persona Temporal" Font-Bold="True" Font-Size="12" ForeColor="#CC3300"></asp:Label>
           <asp:Label Visible="false" ID="lblMensaje" runat="server" Text="" Font-Bold="True" Font-Size="12" ForeColor="#CC3300"></asp:Label>
         <input runat="server"  type="hidden" id="idEstado"/>
         
				<div class="panel-body" >   

  <table class="table table-striped">
          <tbody>
             <tr>
                 

                <td colspan="1">
                 
                      <fieldset>
                        
                           <div class="form-group" runat="server" id="Div1">
                          
                            <div class="col-md-8 inputGroupContainer" >
                                                      
  <div class="row" style="width:180px;"  >        
						 <div class="thumbnail" id="pnlRenaper" runat="server"  >
                             <asp:Image ID="Image1" runat="server" />
                             </div>
                           </div>
                                           
                                  <label style="color:red;"> Actualizado el </label> <asp:Label id="fechaDomicilio" runat="server" ForeColor="Red" ></asp:Label>
                  
                            </div>
                         </div>
                    <div class="form-group" runat="server" id="grupoDNI">
                           
                        
                        
                        <div class="col-md-8 inputGroupContainer">
                              <label class="control-label">  Estado: </label>
                               <div class="input-group">
  
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-check"></i></span> 
 <asp:DropDownList ID="ddlEstadoP" runat="server" AutoPostBack="true" 
                                        DataTextField="nombre" DataValueField="idEstado" class="form-control input-sm"
                                        OnSelectedIndexChanged="ddlEstadoP_SelectedIndexChanged" TabIndex="1">
                                    </asp:DropDownList>
                                     
                         <asp:DropDownList Width="200px" ID="ddlMotivoNI" runat="server" AutoPostBack="true" class="form-control input-sm" DataTextField="nombre" DataValueField="idMotivoNI" TabIndex="2" OnSelectedIndexChanged="ddlMotivoNI_SelectedIndexChanged">
                                    </asp:DropDownList>
                                  
                                   <asp:RangeValidator ID="rvMotivo" runat="server" ControlToValidate="ddlMotivoNI" Enabled="False" ErrorMessage="Seleccione motivo" MaximumValue="99" MinimumValue="1" ValidationGroup="0"></asp:RangeValidator>
                                  
                                   <asp:Button ID="btnValidarRenaper" runat="server" Visible="false" ToolTip="Validar Persona con Renaper"  CssClass="btn btn-danger" Width="150px"   OnClick="lnkValidarRenaper_Click" Text="Validar Renaper">
                                             </asp:Button> 
                                  

                               </div>
                            </div>



                            <div class="col-md-8 inputGroupContainer">
                              <label class="control-label">   DNI: </label>
                               <div class="input-group">
  
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
<asp:TextBox ID="txtDNI" Enabled="false" runat="server" CssClass="auto-style1" ></asp:TextBox>
                                   <asp:RequiredFieldValidator ID="rfvDocumento" runat="server" ErrorMessage="Ingrese Numero de Documento" ControlToValidate="txtDNI" Enabled="False" ValidationGroup="0"></asp:RequiredFieldValidator>

                           
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0" ValidationGroup="0">Debe ingresar solo numeros</asp:CompareValidator>

                               </div>
                            </div>
                         </div>
                         <div class="form-group">
                            
                            <div class="col-md-8 inputGroupContainer">
                                <label class="control-label">  Apellidos:  </label>
                               <div class="input-group">

                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
<asp:TextBox ID="txtApellido"   runat="server" CssClass="form-control" ></asp:TextBox>
                                  

                               </div>
                            </div>
                         </div>


                            <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">   Nombres:    </label>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                <asp:TextBox ID="txtNombre"   runat="server" CssClass="form-control" ></asp:TextBox>
                            </div>
                         </div>
                                </div>

                             <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">      Fecha de Nacimiento:  </label>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                <input id="txtFechaNacimiento"   runat="server" name="fullName" placeholder="Fecha Nacimiento" class="form-control" required="true" value="" type="text"></div>
                            </div>
                         </div>
                                <div class="form-group">
                                   
                          
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Sexo Legal:    </label>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user">  </i></span> 
 <asp:DropDownList ID="ddlSexoLegal" runat="server"   class="form-control input-sm"
                                         TabIndex="1">
     <asp:ListItem Value="3">MASCULINO</asp:ListItem>
     <asp:ListItem Value="2">FEMENINO</asp:ListItem>
     <asp:ListItem Value="1">INDETERMINADO</asp:ListItem>
                                    </asp:DropDownList>
                                  
                                </div>
                            </div>
                         </div>

                               <div class="form-group">
                                   
                          
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Sexo Biologico:    </label>
                                <asp:RangeValidator ID="rvSexoBiologico" runat="server" ControlToValidate="ddlSexo" ErrorMessage="Seleccione Sexo Biologico" MaximumValue="9999999" MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                   <asp:DropDownList ID="ddlSexo" runat="server" name="fullName" placeholder="Sexo Biologico" class="form-control" required="true"></asp:DropDownList>
                              </div>
                            </div>
                         </div>
                               <div class="form-group">
                                   
                          
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Genero Autopercibido:    </label>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlGenero" ErrorMessage="Seleccione Genero AutoPercibido" MaximumValue="9999999" MinimumValue="0" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                   <asp:DropDownList ID="ddlGenero" runat="server" name="fullName" placeholder="Genero" class="form-control" required="true"></asp:DropDownList>
                                     <input id="txtNombreAutopercibido" runat="server" name="NombreAutopercibido" placeholder="Nombre Autopercibido" class="form-control"   value="" type="text"></div>

                              </div>
                            </div>
         

               
                                 <div class="form-group">
                                    
                            
                            <div class="col-md-8 inputGroupContainer"> <label class="control-label">   CUIL:  </label>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                 <input id="txtCuil" runat="server" disabled="true" name="fullName" placeholder="CUIL" class="form-control" required="true" value="0" type="text"></div>
                            </div>
                         </div>

                         <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">  <label class="control-label">    Calle: </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                                 <input id="txtCalle" name="addressLine1"  runat="server" placeholder="Domicilio-Calle y Nro." class="form-control"  value="" type="text" maxlength="50"></div>
                            </div>
                         </div>
                          <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">       <label class="control-label">   Barrio:  </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                                 <input id="txtBarrio" runat="server" name="city"   placeholder="Barrio" class="form-control"  value="" type="text" maxlength="50"></div>
                            </div>
                         </div>
                         <div class="form-group">
                             
                            <div class="col-md-8 inputGroupContainer">
                                  <label class="control-label">  Ciudad:  </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                                <input id="txtCiudad" runat="server" name="city"   placeholder="Ciudad" class="form-control"  value="" type="text" maxlength="50"></div>
                            </div>
                         </div>
                         <div class="form-group">
                            
                            <div class="col-md-8 inputGroupContainer">
                                  <label class="control-label">    Provincia: </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                             <input id="txtProvincia" runat="server"   name="state" placeholder="Provincia" class="form-control"  value="" type="text"></div>
                            </div>
                         </div>
                                  <div class="form-group">
                          
                            <div class="col-md-8 inputGroupContainer">  <label class="control-label">  Pais: </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                               <input id="txtPais" runat="server"  name="state" placeholder="Pais" class="form-control"  value="" type="text"></div>
                           <asp:RequiredFieldValidator ID="rfvPais" runat="server" ErrorMessage="Se requiere pais de origen" ControlToValidate="txtPais" Enabled="False" ValidationGroup="0"></asp:RequiredFieldValidator> </div>
                         </div>
                          
                         <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Codigo Postal:</label>  
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                               <input id="txtCodigoPostal" runat="server" name="postcode" placeholder="Codigo Postal" class="form-control" value="" type="text"></div>
                            </div>
                         </div>
                        
                         <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Contacto:</label> 
                               <div class="input-group"><span class="input-group-addon">
                                   <i class="glyphicon glyphicon-earphone"></i></span>
                                <input id="txtTelefono" runat="server" name="phoneNumber" placeholder="Numero de Telefono" class="form-control"  value="" type="text"></div>
                            </div>
                         </div>

                           <div class="form-group" id="divNumeroAdicional" runat="server" visible="false">
                           
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Numero Identificacion Adicional (por. ej. PASAPORTE):</label> 
                               <div class="input-group">  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>  
                                <input id="txtNumeroAdic" runat="server" name="NumberAdic" placeholder="Numero de Identificacion Adicional" class="form-control"  value="" type="text"/></div>
                            </div>
                         </div>

                          
                           <div class="form-group">
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Direccion de correo electronico:</label> 
                               <div class="input-group">  <span class="input-group-addon"><i class="glyphicon glyphicon-envelope"></i></span>  
                                <input id="txtMail" runat="server" name="NumberAdic" placeholder="Direccion de correo electronico" class="form-control"  value="" type="text"/></div>
                            </div>
                            </div>   
                        

         
                           <div class="form-group">
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Raza:</label> 
                               <div class="input-group">  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>  
                               
                                    <asp:DropDownList ID="ddlRaza" runat="server" name="fullName" placeholder="Genero" class="form-control" required="true">
                                           <asp:ListItem Selected="True" Value="0">Sin identificar</asp:ListItem>
                                <asp:ListItem Value="1">Afroamericano</asp:ListItem>
                                <asp:ListItem Value="4">Amarilla</asp:ListItem>
                                <asp:ListItem Value="2">Hispano</asp:ListItem>
                                <asp:ListItem Value="3">Nativo americano</asp:ListItem>
                                   </asp:DropDownList>
                               </div>
                            </div>
             </div>
                          <div class="form-group">
                 <div class="col-md-8 inputGroupContainer">   <label class="control-label">Se Declara Aborigen:</label> 
                               <div class="input-group">  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>  
                               
                                    <asp:DropDownList ID="ddlAborigen" runat="server" name="fullName" placeholder="Genero" class="form-control" required="true">
                                        <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                        <asp:ListItem Value="1">Si</asp:ListItem>
                                   </asp:DropDownList>
                               </div>
                            </div>
                        
                              </div>

                          <div class="form-group">
                 <div class="col-md-8 inputGroupContainer">   <label class="control-label">Parentescos:</label> 
                               <div class="input-group">  <span class="input-group-addon"> 
                               
                                     
                                <asp:gridview ID="gvParentesco"  CssClass="table table-bordered bs-table"  runat="server"></asp:gridview> </span>  
                                   </div>
                     </div>
                              </div>



                        
                      </fieldset>
                 
                    <asp:Label id="fallecimiento" runat="server" ForeColor="Red" ></asp:Label>
                   
                </td>
                
             </tr>
          </tbody>
       </table>
    </div>

           <asp:HiddenField runat="server" ID="HFIdPaciente" />     
                                                         <asp:HiddenField runat="server" ID="HFNumeroDocumento" />
                                                        <asp:HiddenField runat="server" ID="HFSexo" />
         <div class="panel-footer" >
                 <asp:Button ID="btnConfirmar" runat="server" Text="Guardar"   CssClass="btn btn-primary" Width="100px" OnClick="btnConfirmar_Click" ValidationGroup="0"/>
                 <asp:Button ID="btnContinuar" runat="server" Text="Continuar"   CssClass="btn btn-primary" Width="100px"     CausesValidation="False" OnClick="btnContinuar_Click"/>
         </div>
         </div>
     </div>
    
<script language="javascript" type="text/javascript">

    var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();
    
    var sexo =        $("#<%= HFSexo.ClientID %>").val(); 
         var numeroDocumento = $("#<%= HFNumeroDocumento.ClientID %>").val();
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
         alert(sexo);
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
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnValidarRenaper))%>; }               
            },
        
             overlay: {
                 opacity: 0.5,
                 background: "black"
             }
         }).width(600);
     }
  

    </script>
</asp:Content>