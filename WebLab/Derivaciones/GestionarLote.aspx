<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionarLote.aspx.cs" Inherits="WebLab.Derivaciones.GestionarLote" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
<link type="text/css"rel="stylesheet" href="../script/jquery-ui-1.7.1.custom.css" />  
<script type="text/javascript" src="../script/jquery.min.js"></script> 
<script type="text/javascript" src="../script/jquery-ui.min.js"></script> 
<script type="text/javascript" src="../script/jquery.ui.datepicker-es.js"></script>   

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
  &nbsp;
<div align="left" style="width: 600px" class="form-inline"  >
     
    <div class="panel panel-default">
        <div class="panel-heading">
           <h3 class="panel-title">   <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>      </h3>
        </div>
       
          <div class="panel-body">	
               <table  width="550px" align="center" class="myTabla" >
					<tr >
						<td class="myLabelIzquierda">Fecha Desde:<asp:RequiredFieldValidator ID="rfvFechaDesde" 
                                runat="server" ControlToValidate="txtFechaDesde" ErrorMessage="Fecha Desde" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						<td>
                             <input id="txtFechaDesde" runat="server" type="text" maxlength="10"  onblur="valFecha(this)"  onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                             style="width:100px" title="Ingrese la fecha de inicio"  /></td>
					</tr>
                  
					<tr>
						<td class="myLabelIzquierda">Fecha Hasta:<asp:RequiredFieldValidator ID="rfvFechaHasta" 
                                runat="server" ControlToValidate="txtFechaHasta" ErrorMessage="Fecha Hasta" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						<td>
                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                                 onblur="valFecha(this)" onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  />
                        </td>
                    </tr>
                  
                  <tr>
                      <td class="myLabelIzquierda" style="vertical-align: top" >
                          <asp:Label Text="Nro. Lote:" runat="server"/>
                      </td>
                      <td class="" >
                          <asp:TextBox runat="server" ID="tb_nrolote" CssClass="form-control input-sm" Width="200px"  />
                      </td>
                  </tr>
						
				     <tr> <td class="myLabelIzquierda" style="vertical-align: top" colspan="2"><hr /></td> </tr>
						
                     <tr>
						<td class="myLabelIzquierda" style="vertical-align: top">Efector a Derivar:</td>
						<td>
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="6" CssClass="form-control input-sm"  AutoCallBack="True" >
                            </asp:DropDownList>
                          
					        </td>
                    </tr>
                       
					<tr>
						<td class="myLabelIzquierda">Estado:</td>
						<td>
                            <asp:RadioButtonList ID="rdbEstado" runat="server" RepeatDirection="Vertical" CellSpacing="15" CellPadding="15" Width="200"  TabIndex="12">
                            </asp:RadioButtonList> </td>
                                        
				    </tr>

					<tr> 	<td colspan="2"><hr /></td> </tr>
						
                     <tr>
						<td colspan="2" align="right">
                                <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary" Width="100" onclick="btnBuscar_Click" Text="Buscar" ValidationGroup="0" /> </td>
                     </tr>

				  
					</table>	
                </div>
        </div>
 </div>
 </asp:Content>