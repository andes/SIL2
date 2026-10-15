<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditarListaLote.aspx.cs" Inherits="WebLab.Derivaciones.EditarListaLote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="head1" runat="server">
      


  <link type="text/css"rel="stylesheet" href="../App_Themes/default/style.css" />  
  <link type="text/css"rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />  
      <link rel="stylesheet" href="../bootstrap-3.3.7-dist/css/bootstrap.min.css" />	
  <script type="text/javascript" src="../script/jquery.min.js"></script> 
  <script type="text/javascript" src="../script/jquery-ui.min.js"></script> 
  <script type="text/javascript" src="../script/jquery.ui.datepicker-es.js"></script>

    <!-- scripts propios-->
<script type="text/javascript" src="../script/Mascara.js"></script>
<script type="text/javascript" src="../script/ValidaFecha.js"></script>   
<script type="text/javascript" src="../script/jquery.ui.datepicker-es.js"></script>   
      
<script type="text/javascript"> 


    $(function () {
         $("#<%=txtFechaDesde.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
		$("#<%=txtFechaHasta.ClientID %>").datepicker({
            showOn: 'both',
            buttonImage: '../App_Themes/default/images/calend1.jpg',
            buttonImageOnly: true
        });
    });


</script>  
</head>

<body style="background-color: #ffffff;">
    <form id="form1" runat="server">
        <div style="width: 800px"  class="form-inline" >
            <asp:HiddenField ID="HFIdEfectorDerivacion"  runat="server"/>
            <!-- Filtros -->
            <table width="800px" >
                <tr>
						<td class="myLabelIzquierda">Fecha Desde:<asp:RequiredFieldValidator ID="rfvFechaDesde" 
                                runat="server" ControlToValidate="txtFechaDesde" ErrorMessage="Fecha Desde" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						<td>
                            <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                                   onblur="valFecha(this)"   onkeyup="mascara(this,'/',patron,true)"
                                class="form-control input-sm"  style="width:85px" 
                                title="Ingrese la fecha de inicio"  />    

						</td>

                         <td class="myLabelIzquierda">Fecha Hasta:<asp:RequiredFieldValidator ID="rfvFechaHasta" 
                                runat="server" ControlToValidate="txtFechaHasta" ErrorMessage="Fecha Hasta" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						<td>
                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                                  onblur="valFecha(this)"  onkeyup="mascara(this,'/',patron,true)" 
                                 class="form-control input-sm"  style="width: 85px"
                                 title="Ingrese la fecha de fin"  />    
                        </td>
				</tr>
                <tr>
						<td class="myLabelIzquierda">Servicio:</td>
						<td>
                              <asp:DropDownList ID="ddlServicio" runat="server"  ToolTip="Seleccione el servicio" class="form-control input-sm"
                                  OnSelectedIndexChanged="ddlServicio_SelectedIndexChanged" AutoPostBack="true" width="200">
                            </asp:DropDownList>
                                        
                        </td>
					
						<td class="myLabelIzquierda" >Area:</td>
						<td>
                            <asp:dropdownlist ID="ddlArea" runat="server"   ToolTip="Seleccione el area"  Width="200" class="form-control input-sm">
                            </asp:dropdownlist>
                                        
                        </td>
					</tr>
                <tr>
                    <td> <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" CssClass="btn btn-primary" Text="Buscar" Width="77px" />
                      
                    </td>
                    
                   
                </tr>
                <tr><td colspan="8">  <hr /></td></tr>
                </table>
           <table width="720px" style="table-layout: fixed;">
               <colgroup>
                    <col style="width: 190px;" />
                    <col style="width: 430px;" />
                    <col style="width: 100px;" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top; white-space: nowrap;"  >
                           <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLittleLink" onclick="lnkMarcar_Click" >Marcar todas</asp:LinkButton>
                          &nbsp;<asp:LinkButton ID="lnkDesMarcar" runat="server" CssClass="myLittleLink" onclick="lnkDesMarcar_Click" >Desmarcar</asp:LinkButton>
                    </td>
                     
                     <td style="vertical-align: top; white-space: nowrap; ">    
                         <asp:CustomValidator ID="cvGeneral" runat="server" OnServerValidate="cvGeneral_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                         <asp:Label ID="lblMensaje" Text="" runat="server" />

                      </td>
                      <td style="vertical-align: top; text-align: center; align-content:center">
                        <asp:Button ID="btnGuardar" runat="server" Text="Confirmar" OnClick="btnGuardar_Click" CssClass="btn btn-success" Width="100" ValidationGroup="0" />
                    </td>
                    
                </tr>
                <tr>
                       
                   
                </tr>
            
                <tr>
                    <td style="vertical-align: top" colspan="8">
              
                    <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                        DataKeyNames="idDetalleProtocolo"  Width="100%" CellPadding="0"  ForeColor="#666666" PageSize="1" 
                        EmptyDataText ="No se encontraron protocolos para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                        BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" >
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                        <Columns>
                        <asp:TemplateField HeaderText="Sel." >
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSel" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="5%"  HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="numero"  HeaderText="Nro. Protocolo" >
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="dni" HeaderText="DNI" >
                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="paciente" HeaderText="Paciente/Producto">
                            <ItemStyle Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="determinacion" HeaderText="Practica">
                            <ItemStyle Width="20%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="efectorderivacion" HeaderText="Efector">
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="username" HeaderText="Usuario" >
                            <ItemStyle Width="15%" />
                        </asp:BoundField>
                        </Columns>
                         <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                            Font-Names="Arial" Font-Size="8pt" />
                        <EditRowStyle BackColor="#999999" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
                        </td>
                   
                </tr>
                
            </table>
           
           
                
        </div>
    </form>
</body>
</html>
