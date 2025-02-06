<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultFFEE2.aspx.cs" Inherits="WebLab.Protocolos.DefaultFFEE2" MasterPageFile="~/Site1.Master" %>
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

    <script type="text/javascript">

     



 
</script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
  
     
        <div align="left" style="width: 1000px" class="form-inline"  >
 <table  width="1200px" align="center" class="myTabla">
					<tr>
						<td rowspan="10" style="vertical-align: top; width:300px;" >
						
                               <div id="pnlTitulo" runat="server" class="panel panel-default">
                    <div class="panel-heading">
                      <asp:Label Text="Ultimos 10 Protocolos" runat="server" ID="lblTituloLista"></asp:Label>
                        </div>

				<div class="panel-body" style="align-content:center;">
                            <uc1:ProtocoloList ID="ProtocoloList1" runat="server" />
                   </div>
                                   </div>
                        </td>
						
						
						<td rowspan="10" style="vertical-align: top" >
                                            &nbsp;</td>
						
						
						<td rowspan="10" style="vertical-align: top" >
                              <div id="pnlTitulo2" runat="server" class="panel panel-default" style="width:720px;" >
                    <div class="panel-heading">
                    <h5>    <asp:Label ID="lblTitulo" runat="server" Text="NUEVO PROTOCOLO"></asp:Label></h5><strong><h4>Identificación de		
                                   FFEE</h4> </strong>  <asp:Label ID="lblServicio"   runat="server"  
                                       Text="Label"></asp:Label>
                         
                                  </div>

				<div class="panel-body">
               
                            <asp:Label ID="lblMensajeOK" runat="server" ForeColor="Red" Text="Se encontraron los siguientes datos para el dni ingresado:" Visible="False"></asp:Label>
                           
                             <div class="form-group">
                            <div class="col-md-8 inputGroupContainer">
                                <label class="control-label">   DNI Paciente:  </label>
                               <div class="input-group">

                                  <span class="input-group-addon"><i class="glyphicon glyphicon-user">   </i></span> 
                            <input id="txtDni" type="text" runat="server"  class="form-control input-sm"
                                onblur="valNumeroSinPunto(this)" maxlength="8" tabindex="1" style="width: 120px"/>
                                      <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0" ValidationGroup="0">Debe ingresar solo numeros</asp:CompareValidator>
                                
                                 
                                

                               </div>
                            </div>
                                 </div>
                         
                           <div class="form-group">
                            
                            <div class="col-md-8 inputGroupContainer">
                                <label class="control-label">   Codigo (Identificador PCR o Ficha):  </label>
                               <div class="input-group">

                                  <span class="input-group-addon"><i class="glyphicon glyphicon-barcode"></i></span> 
 <asp:TextBox ID="txtCodigo" runat="server"   class="form-control"     Width="200px"></asp:TextBox>
                                

                                   <br />
                                

                               </div>
                            </div>
                         </div>

                         
                           <div class="form-group">
                             <div class="col-md-8 inputGroupContainer">
                                <label class="control-label">  Tipo Ficha:  </label>
                               <div class="input-group">

                                  <span class="input-group-addon"><i class="glyphicon glyphicon-tag"></i></span> 
  <asp:DropDownList ID="ddlTipoFicha" runat="server" TabIndex="1" class="form-control input-sm">                                            
                                             
                                            </asp:DropDownList>                                 

                                 
                               </div>
                            </div>
                               </div>
                                  <div class="form-group">
                            <div class="col-md-8 inputGroupContainer">
                                <label class="control-label">  Impresora de Codigo de barras:  </label>
                               <div class="input-group">

                                  <span class="input-group-addon"><i class="glyphicon glyphicon-print"></i></span> 
  <asp:DropDownList ID="ddlImpresora" runat="server" TabIndex="1" class="form-control input-sm">                                            
                                             
                                            </asp:DropDownList>                                 

                               </div>
                            </div>
						 </div>
                                           
                                          
 				
                            
                          
                           
                        </div>

                        <div class="panel-footer" >
                                        <asp:Button ID="btnBuscar" runat="server" Text="Aceptar" ValidationGroup="0"  Width="100px"
                                                CssClass="btn btn-danger" TabIndex="2" onclick="btnBuscar_Click" 
                                                ToolTip="Buscar FFEE" />
                            <br />
                                            <asp:CustomValidator ID="cvValidacionInput" runat="server" 
                                                ErrorMessage="Debe completar al menos un analisis" 
                                    ValidationGroup="0" Font-Size="12pt" OnServerValidate="cvValidacionInput_ServerValidate"  
                                             ></asp:CustomValidator>
                                       <input id="hdEfectores" type="hidden" runat="server" />
                            <input id="hdTipoMuestra" type="hidden" runat="server" />
                                         <input id="hdCaracteres" type="hidden" runat="server" />
                            <input id="hdIdPaciente" type="hidden" runat="server" />
                             <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Text="Se encontraron los siguientes datos para el dni ingresado:" Visible="False"></asp:Label>
                          
                                       </div>
                     </div> 
                               
                                 
                   

                    </td>
                        
						
						<td rowspan="10" style="vertical-align: top" >
                                     &nbsp;</td>
					</tr>
					
					</table>

</div>
		  
    	
        </div>
		  
    	
 </asp:Content>
