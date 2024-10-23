<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoNew.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoNew" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
    

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
  <script type="text/javascript" src="../script/ValidaFecha.js"></script>                
       <script type="text/javascript" language="javascript">
    
    function pregunta()
    {
    if (confirm('¿Está seguro de generar un nuevo caso?'))
    return true;
    else
    return false;
    }
    </script>
     
     
     <script type="text/javascript">                     
            $(function() {
                 $("#tabContainer").tabs();                        
                $("#tabContainer").tabs({ selected: 0 });
             });                         
          
            
   

  </script>  

  
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <br /> 
   <div align="center" style="width: 700px" class="form-inline"  >
       <br />
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h1 class="panel-title">CASO GENETICA FORENSE</h1>
                        <h2>Paso 1. Nuevo Caso</h2>
                        </div>
       	<div class="panel-body">	
     <table  >
				
				<%--	<tr>
						<td class="myLabelIzquierda" >Fecha:</td>
						<td align="left" >
                                <input id="txtFecha" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; position=absolute; z-index=0;"  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/></td>
						
					</tr>--%>
					<tr>
						<td  >Tipo de Caso:</td>
						<td align="left" >
                            <asp:DropDownList ID="ddlTipoCaso" runat="server" class="form-control input-sm">
                                <asp:ListItem Selected="True" Value="0">--Seleccione--</asp:ListItem>
                                <asp:ListItem Value="1">--FILIACION--</asp:ListItem>
                                <asp:ListItem Value="2">--FORENSE---</asp:ListItem>
                                   <asp:ListItem Value="3">--QUIMERISMO---</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlTipoCaso" ErrorMessage="Tipo de Caso" MaximumValue="99" MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                        </td>
						
					</tr>
					

					
					<tr>
						<td  >Carátula:</td>
						<td align="left" >
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="300px"
                                TabIndex="2" ToolTip="Ingrese la caratula del caso"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                ControlToValidate="txtNombre" ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						
					</tr>
					

					
					<tr>
						<td  >Solicitante:</td>
						<td align="left" >
                             <asp:DropDownList ID="ddlSectorServicio" class="form-control input-sm" runat="server" TabIndex="4" Width="300px">
                                        </asp:DropDownList>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="ddlSectorServicio" ErrorMessage="Solicitante" MaximumValue="999" MinimumValue="1" Type="Integer" ValidationGroup="0"></asp:RangeValidator>
                        </td>
						
					</tr>
					

					
					<tr>
						<td   colspan="2">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td align="left">
                                            &nbsp;</td>
						
						<td align="right">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los datos marcados como requeridos:" 
                                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />
                                        
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0"
                                                onclick="btnGuardar_Click" CssClass="btn btn-success" Width="80px"  TabIndex="1" OnClientClick="javascript:pregunta();" />
                                        
                        </td>
						
					</tr>
					</table>
               </div>
       </div>
  </div>
 </asp:Content>


