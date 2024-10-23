<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcesaSISA.aspx.cs" Inherits="WebLab.Protocolos.ProcesaSISA" MasterPageFile="../Site1.Master" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
 <link href="../App_Themes/default/style.css" rel="stylesheet" type="text/css" />  
     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 
         <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
   
    
		    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />



  
            

  
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
       <div align="center" style="width: 600px" class="form-inline"  >

     <div id="pnlTitulo" runat="server" class="panel panel-default">
              
               
                    <img runat="server"  id="imgRenaper" style="text-align:right;  " src="../App_Themes/default/images/imgSISA.jpg" /> 
         <br />
                  <h3>  Alta de Caso en SISA </h3>
                      
                   
                           

            
         <input runat="server"  type="hidden" id="idEstado"/>
         
				<div class="panel-body" >   

  <table class="table table-striped">
          <tbody>
             <tr>
                 

                <td colspan="1">
                  
                    
                           

                            <div class="col-md-8 inputGroupContainer">
                              <label class="control-label">   DNI: </label>
                               <div class="input-group">
  
                                  
                                   <asp:Label ID="lblDNI" runat="server" Text="Label"></asp:Label>
                                  
                                  

                               </div>
                            </div>
                         
                       </td>
                 </tr>
               <tr>
                 

                <td colspan="1">
                            <div class="col-md-8 inputGroupContainer">
                                <label class="control-label">  Apellidos:  </label>
                               <div class="input-group">

                               
                                   <asp:Label ID="lblApellido" runat="server" Text="Label"></asp:Label>
                                  

                               </div>
                            </div>
                         
</td>
                 </tr>
               <tr>
                 

                <td colspan="1">
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">   Nombres:    </label>
                               <div class="input-group">
                              
                                   <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label>
                                  
                            </div>
                         </div>
                                                  
</td>
                 </tr>

                            <tr>
                 

                <td colspan="1">
                           
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">      Fecha de Nacimiento:  </label>
                               <div class="input-group">
                                 
                                   <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"></asp:Label>
                                  
                                </div>
                            </div>
                                                            
</td>
                 </tr>
                           <tr>
                 

                <td colspan="1">
                          
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Sexo:    </label>
                               <div class="input-group">
                                  
                                   <asp:Label ID="lblSexo" runat="server" Text="Label"></asp:Label>
                                  
                                </div>
                            </div>
                       </td>
                 </tr>        
                 <tr>
                 

                <td colspan="1">      
                            <div class="col-md-8 inputGroupContainer"><label class="control-label">    Fecha Ficha:    </label>
                               <div class="input-group">
                                 
                                   <asp:Label ID="lblFechaPapel" runat="server" Text="Label"></asp:Label>
                                
                                </div>
                            </div>
                        </td>
                 </tr>      
         
               <tr>
                 

                <td colspan="1">  
                                    
                            
                            <div class="col-md-8 inputGroupContainer"> <label class="control-label">Grupo Evento:  </label>
                               <div class="input-group">
                                 <asp:Label runat="server" id="lblGrupoEvento"></asp:Label>
                                 <input id="txtGrupoEvento" runat="server" visible="false"   class="form-control" required="true" value="0" type="text">

                               </div>
                            </div>
                           </td>
                 </tr>      
         
               <tr>
                 

                <td colspan="1">  
                           
                            <div class="col-md-8 inputGroupContainer"> <label class="control-label"> Evento: </label>
                               <div class="input-group">
                                 
                                    <asp:Label runat="server" id="lblEvento"></asp:Label>
                                 <input id="txtEvento" visible="false" name="addressLine1"  runat="server" placeholder="Domicilio-Calle y Nro." class="form-control" required="true" value="" type="text" maxlength="50"></div>
                            </div>
                          </td>
                 </tr>      
                         <tr>
                 

                <td colspan="1">  
                           
                           
                            <div class="col-md-8 inputGroupContainer"> <label class="control-label">      Clasificacion Manual Caso:  </label>
                               <div class="input-group">
                                   
                                  <asp:Label runat="server" id="lblClasificacionManual"></asp:Label >
                                 <input id="txtClasificacionManualCaso" runat="server"  visible="false" class="form-control" required="true" value="" type="text" maxlength="50">

                               </div>
                            </div>
                         </td>
                 </tr>      
                 
                     
          </tbody>
       </table>
    </div>


         <div class="panel-footer" >
                 <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar"   CssClass="btn btn-primary" Width="100px"     CausesValidation="False" OnClick="btnConfirmar_Click"/>
                 <br />
                 <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red" Text=" gfdgdf" Visible="False"></asp:Label>
                 <br />
                 <asp:Button ID="btnSalir" runat="server" Text="Salir"   CssClass="btn btn-danger" Width="100px"     CausesValidation="False" OnClick="btnSalir_Click" Visible="False"/>
         </div>
         </div>
    </div>

</asp:Content>