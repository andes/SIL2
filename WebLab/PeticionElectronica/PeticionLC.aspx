<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeticionLC.aspx.cs" Inherits="WebLab.PeticionElectronica.PeticionLC" MasterPageFile="~/PeticionElectronica/SitePE.Master" %>
<%@ Register Src="~/Services/ObrasSociales.ascx" TagName="OSociales" TagPrefix="uc1" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

     <link rel="shortcut icon" href="App_Themes/default/images/favicon.ico"/>

 
      <link rel="stylesheet"  href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
<script src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script src='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
     <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>

    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      
      <script type="text/javascript"> 
      
          	$(function() {
	    $("#<%=txtFechaNac.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,
	        
			//showOn: 'button',
			//buttonImage: '../App_Themes/default/images/calend1.jpg',
			//buttonImageOnly: true
		});
	});
	$(function() {
	    $("#<%=txtFecha.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,
	        
			//showOn: 'button',
			//buttonImage: '../App_Themes/default/images/calend1.jpg',
			//buttonImageOnly: true
		});
	});

      


  </script>  
  
    </asp:Content>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    
    
     <div  >
         <h3>Peticion Electronica <small>para Laboratorio Central</small></h3>
         <h4><asp:Label ID="lblEfectorSolicitante" runat="server" Visible="false">CLINICA SAN AGUSTIN</asp:Label></h4>
                     

                                             <div class="panel panel-success" runat="server" id="Div1">
                    <div class="panel-heading">
                        Datos del Paciente
    
  </div>
                    <div class="panel-body">
                         <div class="form-group">
    <h4><label   class="col-lg-10"><asp:Label id="idPeticion" runat="server" Visible="false"></asp:Label>
                                     </label></h4>
      <label   class="col-lg-10">   <anthem:Label ID="lblMensaje" runat="server" Style="color: #0000FF"></anthem:Label>
                                        <anthem:Label ID="lblMensaje2" runat="server" Style="color:red"></anthem:Label></label>

    
                             </div>
                         <div class="form-inline" >
   
   <div class="col-lg-10">
         <strong>DNI </strong> <anthem:RequiredFieldValidator ID="rfvDni" runat="server" ControlToValidate="txtNumeroDocumento" ErrorMessage="Numero de Documento" ValidationGroup="0">*</anthem:RequiredFieldValidator>  <anthem:TextBox ID="txtNumeroDocumento" runat="server" class="form-control input-sm" 
                                          MaxLength="9" ToolTip="Solo números" 
                          AutoPostBack="true" Width="200px" ></anthem:TextBox>
                                  
       <anthem:LinkButton ID="lnkBuscar" runat="server" ToolTip="Buscar Datos" CssClass="btn btn-success" OnClick="lnkBuscar_Click"    Width="40px" >
                                             <span class="glyphicon glyphicon-search"></span></anthem:LinkButton>
                                
                                    <asp:CompareValidator ID="cvNroDoc" runat="server" 
                                        ControlToValidate="txtNumeroDocumento" ErrorMessage="Solo numeros" 
                                        Operator="DataTypeCheck" Type="Integer" />
      
  
        
        
                                
    </div>
                             </div>
                              <div class="form-inline">
     <div class="col-sm-10"> <label style:"width:350px">
    Es Recien Nacido sin DNI?:<anthem:RadioButtonList ID="rdbRN" runat="server" RepeatDirection="Horizontal" AutoCallBack="true" OnSelectedIndexChanged="rdbRN_SelectedIndexChanged">
        
                                        <Items>
<asp:ListItem Value="1">Si</asp:ListItem>
<asp:ListItem Selected="True" Value="0">No</asp:ListItem>
</Items>
                                    </anthem:RadioButtonList></label>
        
                                </div>

  </div>
                             

                   <div class="form-group">
   
  <div class="col-sm-10"> <label style:"width:150px">Apellido: </label> <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="Apellido" ValidationGroup="0">*</asp:RequiredFieldValidator>
      <anthem:TextBox ID="txtApellido" runat="server"   TabIndex="1" Width="300px" class="form-control input-sm" Enabled="False" ></anthem:TextBox>
   

        </div>
                            </div>

                           
                         
             <div class="form-group">
     
   <div class="col-sm-10">
<label style:"width:150px">Nombres: </label> <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator> <anthem:TextBox ID="txtNombre" runat="server"   TabIndex="2" Width="300px" class="form-control input-sm" Enabled="False" ></anthem:TextBox>
    </div>
  </div>


                        <div class="form-group">


    <div class="col-lg-10"><label style:"width:150px">Fecha de Nacimiento:</label> <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ControlToValidate="txtFechaNac" ErrorMessage="Fecha de nacimiento" ValidationGroup="0">*</asp:RequiredFieldValidator> 
 <anthem:TextBox id="txtFechaNac" runat="server" type="text" maxlength="10"   onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width: 200px" ></anthem:TextBox>
    </div>
  </div>
                        <div class="form-group">

                              <div class="col-lg-10"> <label style:"width:150px">Sexo:<asp:RangeValidator ID="rfvSexo" runat="server" 
                                        ErrorMessage="Sexo" ControlToValidate="ddlSexo" MaximumValue="9999" 
                                        MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                           </label> 
    <anthem:DropDownList ID="ddlSexo" runat="server"    class="form-control input-sm" 
                                        PromptText="Seleccionar" ShowPrompt="true" TabIndex="4" TableName="Sys_Sexo" 
                                        TextField="nombre" Width="150px" Enabled="False">
                                    </anthem:DropDownList>
    </div>
  </div>

                        
                        
                                 <div class="form-group">
  
                                     
    &nbsp;<div class="col-lg-10">
              <label for="ejemplo_password_3" >  DNI Madre:     <anthem:Label ID="lblDniMadre" Font-Italic="true" CssClass="small" runat="server" Text="opcional"></anthem:Label> <anthem:RequiredFieldValidator ID="rfvDniMadre" runat="server" ControlToValidate="txtNumeroP" Enabled="False" ErrorMessage="DNI Madre" ValidationGroup="0">*</anthem:RequiredFieldValidator>
                       <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                        ControlToValidate="txtNumeroP" ErrorMessage="Solo numeros" 
                                        Operator="DataTypeCheck" Type="Integer" ValidationGroup="0" />
               

              </label>

           <anthem:textbox ID="txtNumeroP" runat="server" class="form-control input-sm" 
                                          MaxLength="9" 
                                        TabIndex="5" ToolTip="Solo números" 
                          AutoPostBack="true" Width="200px" Enabled="False"></anthem:textbox>
                           
                                   
    </div>
    
                             </div>

                        <div class="form-group">
   
   <div class="col-lg-10">
    <label for="ejemplo_password_3" >Apellido:  <anthem:Label ID="lblApellidoMadre" CssClass="small" runat="server" Text="opcional"></anthem:Label><anthem:RequiredFieldValidator ID="rfvApellidoMadre" runat="server" ControlToValidate="txtApellidoP" Enabled="False" ValidationGroup="0" ErrorMessage="Apellido Madre">*</anthem:RequiredFieldValidator>
                                    
                            </label> <anthem:TextBox ID="txtApellidoP" class="form-control input-sm"  runat="server"   TabIndex="6" Width="300px" Enabled="False"></anthem:TextBox>
           
    </div>
  </div>
                         <div class="form-group">

 <div class="col-lg-10">    <label for="ejemplo_password_3"  >Nombres:    <anthem:Label ID="lblNombreMadre" CssClass="small" runat="server" Text="opcional"></anthem:Label><anthem:RequiredFieldValidator ID="rfvNombreMadre" runat="server" ControlToValidate="txtNombreP" Enabled="False" ValidationGroup="0" ErrorMessage="Nombre de la Madre">*</anthem:RequiredFieldValidator>
                                    
                             </label>
  <anthem:TextBox ID="txtNombreP" runat="server"  class="form-control input-sm"  TabIndex="7" Width="300px" Enabled="False"></anthem:TextBox>
    
    </div>
  </div>
                        
                        
                          <div class="form-group">
   
    <div class="col-lg-10" style="z-index:10;">
         <label for="ejemplo_email_3"  >Obra Social/Financiador:</label>
                <asp:ScriptManager ID="scriptMgr" runat="server">
                                            </asp:ScriptManager>
                                            <asp:UpdatePanel ID="upOSocial" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>  
                                      <uc1:OSociales ID="OSociales" runat="server" TabIndex="10" Requerido="True" ValidationGroup="0" />
                                                    </ContentTemplate>
                                            </asp:UpdatePanel>
                                   
    </div>
                             <asp:ValidationSummary ID="valSum" runat="server" HeaderText="Completar Información del Paciente:"
        ValidationGroup="0" ShowMessageBox="True" ShowSummary="False" />

                       
</div>
                                                 </div>
                                                 </div>

                         <div class="panel panel-success" runat="server" id="Div2">
                    <div class="panel-heading">
                        Datos de la muestra    
  </div>
                    <div class="panel-body">
                        <div class="form-group">
   
  <div class="form-group">
      <label for="exampleInputEmail1">Fecha Pedido: </label>
                <div class='input-group date' id='datetimepicker1' style="width:150px;" >
                    <input type='text' runat="server" id="txtFecha"  class="form-control" tabindex="8" />
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
                              
                             </div>
                      
                        
                          <div class="form-group">
                               <div class="col-sm-10">
    <label for="exampleInputEmail1">Origen: </label>
               <asp:RadioButtonList ID="rdbOrigen"  runat="server" RepeatDirection="Horizontal" TabIndex="9">
                                        <asp:ListItem Value="1">Ambulatorio</asp:ListItem>
                                        <asp:ListItem Value="2">Internacion</asp:ListItem>
                                        <asp:ListItem Value="3">Guardia</asp:ListItem>
                                    </asp:RadioButtonList>                   
        <asp:RequiredFieldValidator ID="rfvOrigen" runat="server" ControlToValidate="rdbOrigen" ErrorMessage="Origen" ValidationGroup="0">*</asp:RequiredFieldValidator>
    </div>
                              </div>
                              
                           


                           

                <%--        <div class="form-group">
                            
    <label for="ejemplo_email_3" class="col-lg-2 control-label">Archivo Adjunto 1:</label>
    <div class="col-lg-10">
               <asp:FileUpload ID="FileUpload1" runat="server"  class="form-control"  />
               <asp:HyperLink ID="Anexo1" runat="server" Visible="false" Target="_blank" ForeColor="#CC3300">Anexo1</asp:HyperLink>
        <asp:Button ID="btnAnulaAnexo1" runat="server" Visible="false" Text="Eliminar Anexo" OnClick="btnAnulaAnexo1_Click" />

    </div>
                              
                             </div>

                        <div class="form-group">
                            
    <label for="ejemplo_email_3" class="col-lg-2 control-label">Archivo Adjunto 2:</label>
    <div class="col-lg-10">
               <asp:FileUpload ID="FileUpload2" runat="server"  class="form-control" />
         <asp:HyperLink ID="Anexo2" Visible="false" runat="server" Target="_blank" ForeColor="#CC3300">Anexo1</asp:HyperLink>
        <asp:Button ID="btnAnulaAnexo2" runat="server" Visible="false" Text="Eliminar Anexo" OnClick="btnAnulaAnexo1_Click" />
    </div>
                              
                             </div>

                        <div class="form-group">
                            
    <label for="ejemplo_email_3" class="col-lg-2 control-label">Archivo Adjunto 3:</label>
    <div class="col-lg-10">
               <asp:FileUpload ID="FileUpload3" runat="server"  class="form-control" />

       <asp:HyperLink ID="Anexo3" Visible="false" runat="server" Target="_blank" ForeColor="#CC3300">Anexo1</asp:HyperLink>
        <asp:Button ID="btnAnulaAnexo3" runat="server" Visible="false" Text="Eliminar Anexo" OnClick="btnAnulaAnexo1_Click" />
    </div>
                              
                             </div>--%>
                        
                                
                             <div class="form-group">
                            
   
    <div class="col-sm-10">
         <label for="ejemplo_email_3"  >Observaciones / Comentarios:</label>
               <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine"  class="form-control" Rows="7" Columns="10" MaxLength="500" TabIndex="10"></asp:TextBox>
    </div>
                              
                             </div>
                        

                        </div>

                             <div class="panel-footer">
                           <%--  Puede adjuntar hasta 3 órdenes de un mismo paciente.
Formatos válidos: .JPG, .PNG, .GIF y .PDF. Peso máximo: 5MB.--%>
                                 Para conocer las determinaciones y condiciones de derivacion puede buscarlas en el <a href=" http://laboratoriocentral.neuquen.gob.ar/website/DirectorioAnalisis.aspx" target="_blank">Directorio de Determinaciones del Laboratorio</a>
                                 </div>
                             </div>

                     <%--          <div class="panel panel-default" runat="server" id="Div3">
                    <div class="panel-heading">
                        Estudios solicitados
    
  </div>
                    <div class="panel-body">
                        <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatColumns="4" Width="600px" >
                            <asp:ListItem>CONGENITOS</asp:ListItem>
                            <asp:ListItem>LEPTO</asp:ListItem>
                            <asp:ListItem>HLA</asp:ListItem>
                            <asp:ListItem>RESPIRATORIOS</asp:ListItem>
                            <asp:ListItem>ENTEROPATOGENOS</asp:ListItem>
                            <asp:ListItem>NEURO</asp:ListItem>
                            <asp:ListItem>ITS</asp:ListItem>
                            <asp:ListItem>BQL</asp:ListItem>
                        </asp:RadioButtonList>
  </div>
                                   </div>

                        <div class="panel panel-default" runat="server" id="Div4">
                    <div class="panel-heading">
                        Datos Clínicos
    
  </div>
                    <div class="panel-body">
                        <table>
                            <tr>
                                <td>Fecha de inicio de los sintomas:</td>
                                <td><input id="txtFechaInicioSintomas" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de inicio"  /></td>
                            </tr>
                            <tr>
                                <td>Diagnostico presuntivo:</td>
                                <td>
                                    <asp:DropDownList ID="ddlDiagnostico" runat="server"  class="form-control input-sm">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td >Observaciones:</td>
                                <td>
                                    <asp:TextBox ID="txtObservaciones" Width="400px" runat="server" Rows="2" TextMode="MultiLine"  class="form-control input-sm"></asp:TextBox>
                                </td>
                            </tr>
                            
                        </table>


                        </div>
                             </div>--%>


                       
                            <asp:LinkButton ID="lnkGuardar" runat="server" CssClass="btn btn-success"     Width="100px" OnClick="lnkGuardar_Click" ValidationGroup="0" TabIndex="11" > <span class="glyphicon glyphicon-floppy-disk"></span>&nbsp;Guardar</asp:LinkButton>

    
                       <asp:CustomValidator ID="cvValidacionInput" runat="server" Font-Size="8pt" onservervalidate="cvValidacionInput_ServerValidate" ValidationGroup="0"></asp:CustomValidator>

    
                       <div>
                           <asp:Label ID="estatus" runat="server" Style="color: #0000FF"></asp:Label>
                           <input id="hidToken" type="hidden" runat="server" />
                           <anthem:TextBox id="hidPaciente" visible="false" runat="server" ></anthem:TextBox>
         </div>

    
                   
        
    </asp:Content>