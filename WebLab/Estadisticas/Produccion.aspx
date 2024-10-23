<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Produccion.aspx.cs" Inherits="WebLab.Estadisticas.Produccion" MasterPageFile="~/Site1.Master" %>
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
    <style type="text/css">





    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
             <div align="left" style="width: 700px" class="form-inline"  >
       <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
   <h5>   <asp:Label ID="lblTitulo" runat="server" Text="ESTADISTICAS DE PRODUCCION"></asp:Label></h5>
  </div>
                    <div class="panel-body" >

                 <table  width="600px"   cellpadding="1" cellspacing="1"   >
				 
		  <tr>
						<td class="myLabelIzquierda" >Efector:</td>
              <td class="myLabelIzquierda" >&nbsp;</td>
						<td colspan="1" >
                            <anthem:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm" OnSelectedIndexChanged="ddlEfector_SelectedIndexChanged" AutoCallBack="True">
                            </anthem:DropDownList>
                                        
                                            </td>
					</tr>
				 
					<tr>
						<td class="myLabelIzquierda" >Fecha Desde:</td>
						<td class="myLabelIzquierda" >&nbsp;</td>
						<td >
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3"  class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio"  />&nbsp;&nbsp;&nbsp;
                                        
                                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top" >Fecha Hasta:</td>
						<td class="myLabelIzquierda" >&nbsp;</td>
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
						<td class="myLabelIzquierda" style="vertical-align: top" >Agrupado:</td>
						<td class="myLabelIzquierda" style="vertical-align: top" >&nbsp;</td>
						<td>
                            <anthem:DropDownList ID="ddlAgrupadoEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="150px" class="form-control input-sm" Enabled="False" AutoCallBack="True" OnSelectedIndexChanged="ddlAgrupadoEfector_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="S">Si</asp:ListItem>
                                <asp:ListItem Value="N">No</asp:ListItem>
                            </anthem:DropDownList>
                                        
                                           <small>Muestra Efectores en columnas en excel</small></td>
					</tr>
				
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top" >Formato:</td>
						<td class="myLabelIzquierda" style="vertical-align: top" >&nbsp;</td>
						<td>
                            <anthem:RadioButtonList ID="rblFormato" runat="server" RepeatDirection="Horizontal" AutoCallBack="True">
                                <asp:ListItem Selected="True">PDF</asp:ListItem>
                                <asp:ListItem>Excel</asp:ListItem>
                            </anthem:RadioButtonList>
                        </td>
					</tr>
				
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top" >Sector/Servicio:</td>
						<td class="myLabelIzquierda" style="vertical-align: top" >&nbsp;</td>
						<td>
                                                           <asp:ListBox ID="lstSector" runat="server" 
                               class="form-control input-sm"  Height="100px" 
                                                               SelectionMode="Multiple" TabIndex="11" 
                                ToolTip="Seleccione los sectores" Width="350px"></asp:ListBox>
                                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                               ControlToValidate="lstSector" ErrorMessage="Sector/Servicio" 
                                                               ValidationGroup="0">*</asp:RequiredFieldValidator>
                                <hr />
                                            </td>
					</tr>
				
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top" >Origen a incluir:
                            
                        </td>
						<td class="myLabelIzquierda" style="vertical-align: top" >&nbsp;&nbsp;</td>
						<td align="left">
                            <div class="mylabelizquierda">
                                Seleccionar:
                                <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLink" 
                                    onclick="lnkMarcar_Click">Todas</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="myLink" 
                                    onclick="lnkDesmarcar_Click">Ninguna</asp:LinkButton>
                                        <asp:CustomValidator ID="cvOrigen" runat="server" 
                                ErrorMessage="Debe seleccionar al menos un origen" 
                                onservervalidate="cvOrigen_ServerValidate" ValidationGroup="0">Debe seleccionar al menos un origen</asp:CustomValidator>
                            </div>
                            <asp:CheckBoxList ID="ChckOrigen" runat="server" RepeatDirection="Vertical"  
                                RepeatColumns="5">
                            </asp:CheckBoxList>
                                        <hr />
                                            </td>
					</tr>
				
					<tr>
						<td class="myLabelIzquierda" style="vertical-align: top;" >Areas a incluir:
                              </td>
						<td   style="vertical-align: top;">&nbsp;</td>
						<td align="left" >
                            <div  >
                                Seleccionar:
                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="myLink" 
                                    onclick="LinkButton1_Click">Todas</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="myLink" 
                                    onclick="LinkButton2_Click">Ninguna</asp:LinkButton>
                                        
                                        <asp:CustomValidator ID="cvOrigen0" runat="server" 
                                ErrorMessage="Debe seleccionar al menos un area" 
                                onservervalidate="cvOrigen0_ServerValidate" ValidationGroup="0">Debe seleccionar al menos un area</asp:CustomValidator>
                                        
                            </div>
                            <asp:CheckBoxList ID="ChckArea" RepeatDirection="Vertical" runat="server" 
                                RepeatColumns="1">
                            </asp:CheckBoxList>
                                        
                                            </td>
					</tr>
				
                      
					
					 
					
					</table>
                   </div>
            <div class="panel-footer" >
                              
                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="130px"
                                onclick="btnGenerar_Click" Text="Generar Reporte" 
                                ValidationGroup="0" />
                                 <asp:GridView ID="gvEstadistica" runat="server" CellPadding="0" 
          CssClass="table table-bordered bs-table"
           >
           
       </asp:GridView>
                                        
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" 
                                                ValidationGroup="0" ShowSummary="False" HeaderText="Debe seleccionar:" />
                                            <br />
                               <div class="myLabelRojo">           *Las prácticas procesadas son las validadas.</div> 

                </div>
           </div>
 </div>
 </asp:Content>