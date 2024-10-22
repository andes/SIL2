<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneralFiltro.aspx.cs" Inherits="WebLab.Estadisticas.GeneralFiltro" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

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
    <br />  <div align="center" style="width: 820px" class="form-inline"  >
      <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">  <asp:Label ID="lblTitulo" runat="server" Text="REPORTES ESTADISTICOS"></asp:Label></h3>
                        </div>
          				<div class="panel-body">	
                 <table  width="800px" align="center"  cellpadding="5" cellspacing="5" >
					  <tr>
						<td class="myLabelIzquierda" >Efector:</td>
						<td >
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                            </td>
					</tr>
					 <tr>
						<td class="myLabelIzquierda" colspan="2">
                            <hr />
                            </td>
                         </tr>
					<tr> 
						<td class="myLabelIzquierda" colspan="2">
                    <anthem:RadioButtonList ID="rdbTipoInforme" runat="server" AutoCallBack="True" 
                                onselectedindexchanged="rdbTipoInforme_SelectedIndexChanged" 
                            RepeatColumns="4">
                             
                                <Items>
<asp:ListItem Value="0">Conteo por Areas</asp:ListItem>
<asp:ListItem Value="1">Conteo por Análisis</asp:ListItem>
<asp:ListItem Value="4">Conteo por Derivaciones Enviadas</asp:ListItem>
                                    <asp:ListItem Value="11">Conteo por Derivaciones Recibidas</asp:ListItem>
                                    <asp:ListItem Value="6">Conteo por Diagnósticos</asp:ListItem>
<asp:ListItem Value="3">Conteo por Efector Solicitante</asp:ListItem>
<asp:ListItem Value="2">Conteo por Médico Solicitante</asp:ListItem>
                                    <asp:ListItem Value="8">Conteo por Sector Solicitante</asp:ListItem>
                                    <asp:ListItem Value="7">Protocolos por Dia</asp:ListItem>
                                    <asp:ListItem Value="10">Protocolos por Dia - Franja Horaria</asp:ListItem>
                                    <asp:ListItem Value="9">Ranking por día de la semana</asp:ListItem>
<asp:ListItem Value="5">Totalizado Resumido</asp:ListItem>
</Items>
                            </anthem:RadioButtonList>
                                        
                                            <anthem:RequiredFieldValidator 
                                ID="rfvTipoInforme" runat="server" ControlToValidate="rdbTipoInforme" 
                                ErrorMessage="Tipo de reporte" ValidationGroup="0">*</anthem:RequiredFieldValidator>
                                </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2">
                            <anthem:Label ID="lblDescripcion" runat="server" CssClass="alert" 
                                Visible="False" Height="100%"></anthem:Label>
                                        
                                </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2"><hr /></td>
					</tr>
				
					<tr>
						<td class="myLabelIzquierda" >Fecha Desde: </td>
						<td >
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3"  class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio"  />&nbsp;&nbsp;&nbsp;
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Fecha Hasta:</td>
						<td >
                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  /><asp:CustomValidator ID="CustomValidator1" runat="server" 
                                ErrorMessage="Debe ingresar un rango de fechas valido" 
                                onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0">Debe 
                            ingresar un rango de fechas valido</asp:CustomValidator>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Franja Horaria:</td>
						<td >
                            <anthem:TextBox ID="txtHoraDesde" runat="server" CssClass="form-control input-sm" Width="80px" Enabled="False" ></anthem:TextBox>
                                        
                                            &nbsp;-
                            <anthem:TextBox ID="txtHoraHasta" runat="server" CssClass="form-control input-sm" Width="80px" Enabled="False" ></anthem:TextBox>
                                        (formato: HH:MM - Entre 00:00 y 23:59)
                                            <anthem:CustomValidator ID="CustomValidatorHoras" runat="server" 
                                ErrorMessage="Debe ingresar un rango de horarios validos" 
                                onservervalidate="CustomValidatorHoras_ServerValidate" ValidationGroup="0" Enabled="False">Debe 
                            ingresar un rango de fechas valido</anthem:CustomValidator>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Servicio:</td>
						<td >
                            <anthem:DropDownList ID="ddlServicio" runat="server"  
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control input-sm" 
                                AutoCallBack="True" onselectedindexchanged="ddlServicio_SelectedIndexChanged">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Area:</td>
						<td  >
                            <anthem:DropDownList ID="ddlArea" runat="server"   
                                ToolTip="Seleccione el area" TabIndex="1" class="form-control input-sm" >
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
						<tr>
						<td class="myLabelIzquierda"  >Efector Solicitante:</td>
						<td  >
                            <anthem:DropDownList ID="ddlEfectorSolicitante" runat="server"  Width="500px"
                                ToolTip="Seleccione el area" TabIndex="1" class="form-control input-sm" >
                            </anthem:DropDownList>
                                        
                        </tr>
						<tr>
						<td class="myLabelIzquierda" >Medico Solicitante:</td>
						<td >
                            <anthem:DropDownList ID="ddlEspecialista" runat="server" 
                                ToolTip="Seleccione el area" TabIndex="1"   class="form-control input-sm">
                            </anthem:DropDownList>
                                        </td>
                        </tr>
						<tr>
						<td class="myLabelIzquierda"  >Agrupación:</td>
						<td  >
                            <asp:RadioButtonList ID="rdbAgrupacion" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">Por Origen</asp:ListItem>
                                <asp:ListItem Value="1">Por Prioridad</asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                        </tr>
                          
						<tr>
						<td class="myLabelIzquierda"  >Generar Gráfico:</td>
						<td >
                    <anthem:RadioButtonList ID="rdbGrafico" runat="server" 
                                onselectedindexchanged="rdbTipoInforme_SelectedIndexChanged" 
                                RepeatDirection="Horizontal">
                             
                                <Items>
<asp:ListItem Value="0">Si</asp:ListItem>
<asp:ListItem Value="1" Selected="True">No</asp:ListItem>
</Items>
                            </anthem:RadioButtonList>
                              </td>          
                        </tr>
                            <tr>
						<td class="myLabelIzquierda"  >Protocolos:</td>
						<td  >
                            <asp:RadioButtonList ID="rdbEstado" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="-1" Selected="True">Todos</asp:ListItem>
                                <asp:ListItem Value="0">No Procesados</asp:ListItem>
                                <asp:ListItem Value="1">En Proceso</asp:ListItem>
                                <asp:ListItem Value="2">Terminados</asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                        </tr>
                            <tr>
						<td class="myLabelIzquierda" colspan="2"><hr /></td>
                                </tr>
						<tr>
						<td colspan="2" align="right">
                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="180px"
                                onclick="btnGenerar_Click" Text="Generar Reporte" 
                                ValidationGroup="0" />&nbsp;&nbsp;
                                
                                </td>
                            </tr>
						<tr>
						<td   colspan="2">
                                        
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" 
                                                ValidationGroup="0" ShowSummary="False" />
                        </td>
						
					</tr>
					<tr>
						<td   colspan="2">
                            &nbsp;</td>
						
					</tr>
					</table>						
					</div>
          </div>
					</div>
 
 </asp:Content>
