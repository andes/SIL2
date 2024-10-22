<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditoriaUsuario.aspx.cs" Inherits="WebLab.Informes.AuditoriaUsuario" MasterPageFile="~/Site1.Master" %>

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
 
     
  </script>  
  
  	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
    </asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
     
    <div  style="width: 800px" class="form-inline" >
 
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h4><asp:Label ID="lblTitulo"  runat="server" Text="Label"></asp:Label></h4>
                        </div>

				<div class="panel-body">	 
                
                    <table style="width: 100%;">
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">
                               Fecha desde:
                            </td>
                            <td>
                               <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de inicio"  />
                            <asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Debe ingresar el rango de fechas" 
                                onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="1">*</asp:CustomValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">
                                Fecha hasta:
                            </td>
                            <td>
                                <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">
                                Usuario que realiza ABM:
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlUsuarioABM" runat="server" class="form-control input-sm">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="myLabelIzquierda" style="width: 79px">
                                Usuario Generado:</td>
                            <td>
                               <asp:DropDownList ID="ddlUsuarioModificado" runat="server" class="form-control input-sm">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 79px">
                                &nbsp;</td>
                            <td>
                                <br />
                                <asp:Button class="btn btn-primary" ID="btnControlAcceso" runat="server" Width="120px"
                                    onclick="btnControlAcceso_Click" Text="Ver Informe" ValidationGroup="1"/>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
              
                </div>
                    </div>    
             
        </div>
  </asp:Content>