<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcesaRenaper.aspx.cs" Inherits="WebLab.Protocolos.ProcesaRenaper" MasterPageFile="../Site1.Master" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
 <link href="../App_Themes/default/style.css" rel="stylesheet" type="text/css" />  
     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 
         <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    
   	 
    
		    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />


     
<script type="text/javascript">
     

	$(function() {
	    $("#<%=txtFechaNacimiento.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,

	        showOn: 'button',
	        buttonImage: '../App_Themes/default/images/calend1.jpg',
	        buttonImageOnly: true
	    });
	});
</script>
  
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
    <div class="container" >
   
     <div id="pnlTitulo" runat="server" class="panel panel-default">
              
              <img runat="server"  id="imgRenaper" style="text-align:right; width: 80px; height: 80px;" src="../App_Themes/default/images/renaper.jpg" />  
                       <img runat="server"  id="imgAndes" style="text-align:right; width: 190px; height: 107px;" src="../App_Themes/default/images/andes.jpg" />  
                      
         <asp:Label ID="lblValidador" runat="server" Text="Paciente VALIDADO POR MPI-ANDES" Visible="False" Font-Bold="True" Font-Size="16pt"></asp:Label>
                           

            
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
                          
                            <div class="col-md-8 inputGroupContainer">
                                  
                               
  <div class="row" style="width:450px;"  >        
						<%-- <div class="thumbnail"  >
                             <asp:Image ID="Image1" runat="server" />
                             </div>
                           </div>--%>
                                
                                   <asp:Label runat="server" Visible="false" ID="lblFechaDomicilio" style="color:red;"> Domicilio actualizado el día </asp:Label> <asp:Label id="fechaDomicilio" runat="server" ForeColor="Red" ></asp:Label>
                  
                            </div>
                         </div>
                    <div class="form-group" runat="server" id="grupoDNI">
                           

                            <div class="col-md-8 inputGroupContainer">
                              <label class="control-label">   DNI: </label>
                               <div class="input-group">
  
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
<asp:TextBox ID="txtDNI" Enabled="false" runat="server" CssClass="auto-style1" ></asp:TextBox>
                                  

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
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                              <input id="txtSexo" runat="server" disabled="true" name="fullName" placeholder="Sexo" class="form-control" required="true" value="" type="text"></div>
                            </div>
                         </div>
         <div class="form-group">
                                   
                          
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Sexo Biologico:    </label>
                                <asp:RangeValidator ID="rvSexoBiologico" runat="server" ControlToValidate="ddlSexo" ErrorMessage="Seleccione Sexo Biologico" MaximumValue="9999999" MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                   <asp:DropDownList ID="ddlSexo" runat="server" name="fullName" placeholder="Sexo" class="form-control" required="true"></asp:DropDownList>
                              </div>
                            </div>
                         </div>

               
                               <div class="form-group">
                                   
                          
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Genero Autopercibido:    </label>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlGenero" ErrorMessage="Seleccione Genero" MaximumValue="9999999" MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                               <div class="input-group">
                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span> 
                                   <asp:DropDownList ID="ddlGenero" runat="server" name="fullName" placeholder="Genero" class="form-control" required="true"></asp:DropDownList>
                                     <input id="txtNombreAutopercibido" runat="server" name="NombreAutopercibido" placeholder="Nombre Autopercibido" class="form-control"  value="" type="text"></div>

                              </div>
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
                                 <input id="txtCalle" name="addressLine1"  runat="server" placeholder="Domicilio-Calle y Nro." class="form-control" required="true" value="" type="text" maxlength="50"></div>
                            </div>
                         </div>
                          <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">       <label class="control-label">   Barrio:  </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                                 <input id="txtBarrio" runat="server" name="city"   placeholder="Barrio" class="form-control" required="true" value="" type="text" maxlength="50"></div>
                            </div>
                         </div>
                         <div class="form-group">
                             
                            <div class="col-md-8 inputGroupContainer">
                                  <label class="control-label">  Ciudad:  </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                                <input id="txtCiudad" runat="server" name="city"   placeholder="Ciudad" class="form-control" required="true" value="" type="text" maxlength="50"></div>
                            </div>
                         </div>
                         <div class="form-group">
                            
                            <div class="col-md-8 inputGroupContainer">
                                  <label class="control-label">    Provincia: </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                             <input id="txtProvincia" runat="server"   name="state" placeholder="Provincia" class="form-control" required="true" value="" type="text"></div>
                            </div>
                         </div>
                                  <div class="form-group">
                          
                            <div class="col-md-8 inputGroupContainer">  <label class="control-label">  Pais: </label>
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                               <input id="txtPais" runat="server"  name="state" placeholder="Pais" class="form-control" required="true" value="" type="text"></div>
                            </div>
                         </div>
                         <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Codigo Postal:</label>  
                               <div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                               <input id="txtCodigoPostal" runat="server" name="postcode" placeholder="Codigo Postal" class="form-control" required="true" value="" type="text"></div>
                            </div>
                         </div>
                        
                         <div class="form-group">
                           
                            <div class="col-md-8 inputGroupContainer">   <label class="control-label">Contacto:</label> 
                               <div class="input-group"><span class="input-group-addon">
                                   <i class="glyphicon glyphicon-earphone"></i></span>
                                <input id="txtTelefono" runat="server" name="phoneNumber" placeholder="Numero de Telefono" class="form-control"  value="" type="text"/></div>
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
                      </fieldset>
                 
                    <asp:Label id="fallecimiento" runat="server" ForeColor="Red" ></asp:Label>
                   
                      <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Fecha invalida: complete en formato dd/mm/aaaa o verifique que sea correcta" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                   
                </td>
                
             </tr>
          </tbody>
       </table>
    </div>


         <div class="panel-footer" >
                 <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar"   CssClass="btn btn-primary" Width="100px"     CausesValidation="False" OnClick="btnConfirmar_Click" ValidationGroup="0"/>
         </div>
         </div>
     </div>
     
</asp:Content>