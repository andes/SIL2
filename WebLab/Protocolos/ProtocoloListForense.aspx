<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloListForense.aspx.cs" Inherits="WebLab.Protocolos.ProtocoloListForense" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>



<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript"> 
      

	$(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
		    maxDate: 0,
		    minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
	    $("#<%=txtFechaHasta.ClientID %>").datepicker({
	        maxDate: 0,
	        minDate: null,

			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});
 
     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
    <style type="text/css">
        .style4
        {
            width: 16%;
        }
        .style5
        {
            width: 101px;
        }
        .style8
        {
            width: 101px;
            height: 28px;
        }
        .style9
        {
            height: 28px;
        }
        .style10
        {
            width: 16%;
            height: 28px;
        }
    </style>
  
 

   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
   <div align="left" style="width: 1200px" class="form-inline"  >
        
               
				 <table  width="1150px" align="center"  class="myTabla" >
				
					<tr>
					<td colspan="5">
                        <div id="pnlTitulo"  runat="server" class="panel panel-default" >
                    <div class="panel-heading">
    <h3 class="panel-title"><asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h3>
                        </div>

				<div class="panel-body">	
					<table  width="1150px" align="center" >
					
					<tr >
						<td class="myLabelIzquierda" >Solicitante:</td>
						<td colspan="3">
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                            </td>
						<td class="myLabelIzquierda">
                            Fecha Desde:</td>
						<td>
                    <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px"  /></td>
						<td class="myLabelIzquierda" >
                            Fecha Hasta:</td>
						<td>
                    <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                        style="width: 100px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"  /></td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Lugar de toma de muestra:</td>
						<td colspan="3">
                                        <asp:DropDownList ID="ddlSectorServicio" runat="server" TabIndex="2" Width="250px" class="form-control input-sm"
                                            ToolTip="Seleccione el sector">
                                        </asp:DropDownList>
                                        
                                            </td>
						<td class="myLabelIzquierda" >
                                            Nro. de Cadena Custodia:</td>
						<td>
                            <asp:TextBox ID="txtNroOrigen" runat="server" class="form-control input-sm" style="width: 150px" ></asp:TextBox>
                                        
                        </td>
						<td class="myLabelIzquierda" >
                                            Nro. de Caso:</td>
						<td>
                            <input id="txtNumeroCaso" runat="server" type="text" maxlength="9" 
                        tabindex="6" class="form-control input-sm"  onblur="valNumero(this)"
                                style="width: 70px"  /><asp:CustomValidator ID="cvNumeroCaso" runat="server" 
                                ErrorMessage="Numero de Caso- Sólo numeros (sin puntos ni espacios)" 
                                onservervalidate="cvNumeroTarjeta_ServerValidate" ValidationGroup="0">*</asp:CustomValidator>
                            </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Protocolo Desde:</td>
						<td>
                    <input id="txtProtocoloDesde" runat="server" type="text" maxlength="9" 
                          tabindex="5" class="form-control input-sm"  onblur="valNumero(this)"
                                style="width: 100px"  /><asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                ErrorMessage="Numero de Protocolo- Sólo numeros (sin puntos ni espacios)" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="0" 
                                >*</asp:CustomValidator>
                                        
                                            </td>
						<td class="myLabelIzquierda" >
                                            Protocolo Hasta:</td>
						<td>
                    <input id="txtProtocoloHasta" runat="server" type="text" maxlength="9" 
                        tabindex="6" class="form-control input-sm"  onblur="valNumero(this)"
                                style="width: 100px"  /><asp:CustomValidator ID="cvNumeroHasta" runat="server" 
                                ErrorMessage="Numero de Protocolo - Sólo numeros (sin puntos ni espacios)" 
                                onservervalidate="cvNumeroHasta_ServerValidate" ValidationGroup="0">*</asp:CustomValidator>
                                        
                                            </td>
						<td class="myLabelIzquierda" >
                                            Estado:</td>
						<td>
                    <asp:DropDownList ID="ddlEstado" runat="server" 
                                class="form-control input-sm" TabIndex="12">
                                                <asp:ListItem Selected="True" Value="-1">Todos los activos</asp:ListItem>
                                                <asp:ListItem Value="0">No Procesado</asp:ListItem>
                                                <asp:ListItem Value="1">En Proceso</asp:ListItem>
                                                <asp:ListItem Value="2">Terminado</asp:ListItem>
                                                <asp:ListItem Value="3">Restringidos</asp:ListItem>
                           <asp:ListItem Value="4">Eliminados</asp:ListItem>
                                            </asp:DropDownList>
                                        
                                     </td>
						<td>
                            &nbsp;</td>
						<td>
                    &nbsp;</td>
                            </tr>
                            
						
						
					<tr>
						<td class="myLabelIzquierda" >DNI Persona: &nbsp;&nbsp;</td>
						<td colspan="3">
                             <input id="txtDni" type="text" runat="server"  class="form-control input-sm" style="width: 100px" 
                                onblur="valNumero(this)" tabindex="11" __designer:mapid="f0"/><asp:CompareValidator ID="cvDni" 
                                 runat="server" ControlToValidate="txtDni" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValidationGroup="0">Debe ingresar solo numeros</asp:CompareValidator>
                                        
                                            </td>
						<td class="myLabelIzquierda" >
                                            &nbsp;</td>
						<td>
                            &nbsp;</td>
						<td>
                            &nbsp;</td>
						<td>
                            &nbsp;</td>
                            </tr>
                            
						
						
					<tr>
						<td class="myLabelIzquierda" >Apellido/s:</td>
						<td colspan="3">
                                            <asp:TextBox ID="txtApellido" runat="server" class="form-control input-sm"  TabIndex="13" 
                                                Width="200px"></asp:TextBox>
                                        
                                            </td>
						<td class="myLabelIzquierda" >
                                            &nbsp;</td>
						<td>
                            &nbsp;</td>
						<td>
                            &nbsp;</td>
						<td>
                                                <asp:CheckBox ID="chkRecordarFiltro" runat="server" Checked="True" CssClass="myLabelIzquierda" Font-Italic="True" ForeColor="#993300" Text="Recordar filtros" />
                                     </td>
                            </tr>
                            
						
						
						</table>
					
					
					
					
                        </div>

                            <div class="panel-footer">
                                            <asp:Panel ID="pnlControl" runat="server">
                                           
                                                    <table style="width:100%;">
                                                        <tr>
                                                            <td align="left">
                                                              
                                                                <asp:CustomValidator ID="cvFechas" runat="server" ErrorMessage="Fechas de inicio y de fin" onservervalidate="cvFechas_ServerValidate" ValidationGroup="0">Debe ingresar fechas de inicio y fin</asp:CustomValidator>
                                                              
                                                            </td>
                                                            <td align="right">
                                                                
                                                                 &nbsp;&nbsp;&nbsp;
                                                                 <asp:Button ID="btnBuscarControl" runat="server" CssClass="btn btn-primary"
                                                                     onclick="btnBuscarControl_Click" TabIndex="15" Text="Buscar" 
                                                                     ValidationGroup="0" Width="100px"   />
                                                                 </td>
                                                        </tr>
                                                        
                                                    </table>
                                                
                                            </asp:Panel>
                                <asp:Panel ID="pnlLista" runat="server">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td>
                                                           <span class="label label-danger">No procesado</span>
<span class="label label-warning">En proceso</span>
<span class="label label-success">Terminado</span>
                                                         
                                                        </td>
                                                        <td align="right">
                                                         

                                                            <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-primary"
                                                                onclick="btnBuscar_Click" TabIndex="15" Text="Buscar" ValidationGroup="0" 
                                                                Width="77px" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;<asp:Label ID="CantidadRegistros" runat="server"  
                                                              forecolor="Blue" />
                                                            &nbsp;
                                                            <asp:Label ID="CurrentPageLabel" runat="server" forecolor="Blue" />
                                                        </td>
                                                        <td>
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                 
                                                            <asp:GridView ID="gvLista" runat="server" AllowPaging="True"  CssClass="table table-bordered bs-table" 
                                                                AutoGenerateColumns="False"  CellPadding="2" DataKeyNames="idProtocolo" 
                                                                EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados" 
                                                                GridLines="Horizontal" 
                                                                onpageindexchanging="gvLista_PageIndexChanging" 
                                                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                                                PageSize="20" Width="100%" BackColor="White">
                                                                     <PagerStyle HorizontalAlign = "Center" CssClass = "GridPager" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="estado" />
                                                                    <asp:BoundField DataField="impreso" />
                                                                    <asp:BoundField DataField="numero" HeaderText="Nro.">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="nroCaso" HeaderText="Nro. Caso" />
                                                                    <asp:BoundField DataField="nroOrigen" HeaderText="Nro. Origen">
                                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="paciente" HeaderText="Referencia">
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="muestra" HeaderText="Muestra">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="solicitante" HeaderText="Solicitante">
                                                                        <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="username" HeaderText="Usuario">
                                                                        <ItemStyle Width="5%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha Act.">
                                                                        <ItemStyle Font-Size="7pt" Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>

                                                                              <asp:LinkButton ID="Editar" runat="server" Text="" Width="20px"  >
                                             <span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                                                                          <%--  <asp:ImageButton ID="Editar" runat="server" 
                                                                                ImageUrl="~/App_Themes/default/images/editar.jpg" ommandName="Editar" />--%>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                                                               
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                       
                                                                              <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>

                                                                       <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                                               
                                                                            
                                                                           

                                                                             <asp:LinkButton ID="Adjuntar" runat="server" Text="" Width="20px" >
                                             <span class="glyphicon glyphicon-paperclip"></span></asp:LinkButton>

                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                
                                                                <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
                                                            </asp:GridView>


                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>

                            </div>
                        </div>
					</td>
					</tr>
					
					<tr>
						<td   colspan="5"> 
						
            
                                            <asp:Panel ID="pnlListadoOrdenado" CssClass="myLabelIzquierda" runat="server">
                                                     
                                                <div class="panel panel-default">
                    <div class="panel-heading"><h4>Mas filtros para exportar datos  </h4>

                        </div>

				<div class="panel-body">	
                                                    <table style="width:100%; vertical-align: top;">
                                                        <tr>
                                                            <td align="left" style="vertical-align: top" class="style8">
                                                                Area: &nbsp; &nbsp;<%--  <div  style="width:230px;height:60pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> --%><%--<asp:CheckBoxList ID="chkMuestra" runat="server">
                                                                </asp:CheckBoxList>--%>&nbsp;&nbsp;</td>
                                                            <td align="left"   style="vertical-align: top" class="style9">
                                                                <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" class="form-control input-sm"
                                                                    onselectedindexchanged="ddlArea_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left"   style="vertical-align: top" class="style9">
                                                                <anthem:LinkButton CssClass="myLabelIzquierda"  ID="btnSeleccionarTipoMuestra" runat="server"  Visible="false"
                                                                     >Tipos de muestras:</anthem:LinkButton>
                                                            </td>
                                                            <td align="left" rowspan="2" style="vertical-align: top">
                                                                <anthem:ListBox ID="lstMuestra" runat="server" Height="150px" Width="250px" 
                                                                    SelectionMode="Multiple" Visible="False">
                                                                </anthem:ListBox>
                                                            </td>
                                                            <td style="vertical-align: top" align="right" class="style10">
                                                                <asp:Button ID="btnBuscarExportar" runat="server" CssClass="myButton" 
                                                                   TabIndex="15" Text="Buscar" 
                                                                    ValidationGroup="0" Width="77px" />
                                                            </td>
                                                        </tr>
                                                        
                                                        
                                                        <tr>
                                                            <td align="left" class="style5" style="vertical-align: top">
                                                                Practica:</td>
                                                            <td align="left"   style="vertical-align: top">
                                                                <asp:DropDownList ID="ddlItem" runat="server" class="form-control input-sm">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left"   style="vertical-align: top">
                                                            <%--    <asp:DropDownList ID="ddlMuestra" runat="server" Visible="false">
                                                                </asp:DropDownList>--%>
                                                            </td>
                                                            <td align="right" class="style4" style="vertical-align: top">
                                                                &nbsp;</td>
                                                        </tr>
                                                        
                                                        
                                                        <tr>
                                                            <td align="left" style="vertical-align: top" colspan="5">
                                                                &nbsp;</td>
                                                        </tr>
                                                        
                                                        
                                                    
                                                    </table>

                                                
                    </div>
                                                    </div>
                                            </asp:Panel>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="5">
                                            
                                                                <asp:Panel ID="pnlImpresion" runat="server" Visible="false">
                                                                <hr />
                                                                  <table style="width:100%; vertical-align: top;">
                                                                        <tr>
                                                                            <td align="left" class="myLabelIzquierda" style="vertical-align: top">
                                                                                <asp:RadioButtonList ID="rdbTipoListaProtocolo" runat="server" 
                                                                                    CssClass="myLabel" RepeatDirection="Horizontal">
                                                                                    <asp:ListItem Selected="True" Value="0">Formato Reducido (Nombre)</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Formato Reducido (Codigo)</asp:ListItem>
                                                                                    <%-- <asp:ListItem Value="1">Formato Extendido</asp:ListItem>--%>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td align="right">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                           <td align="left" style="vertical-align: top" colspan="2">
                                                                <div class="panel-footer">			
 <table width="100%" align="center">
 	<tr>
					<td class="myLabelIzquierda" align="left" style="width: 140px; background-color: #EFEFEF;">
                                        Impresora del sistema: </td>
						<td align="left">
                                        <asp:DropDownList ID="ddlImpresora" runat="server" class="form-control input-sm" >
                                        </asp:DropDownList>
                            </td>
						
                                        <td align="right">
                                                  <img alt="" src="../App_Themes/default/images/excelPeq.gif"/>
                                                                <asp:LinkButton ID="lnkExcel" runat="server" CssClass="myLittleLink" 
                                                                    onclick="lnkExcel_Click" ValidationGroup="0">Exportar a Excel</asp:LinkButton>
                                            &nbsp;
                                                  <img alt="" src="../App_Themes/default/images/pdf.jpg" />
                                                                <asp:LinkButton ID="lnkPDF" runat="server" CssClass="myLittleLink" 
                                                                    onclick="lnkPDF_Click" ValidationGroup="0">Exportar a Pdf</asp:LinkButton>
                                                                <img alt="" src="../App_Themes/default/images/imprimir.jpg" />
                                                                &nbsp;
                                                                <asp:LinkButton ID="lnkImprimir" runat="server" CssClass="myLittleLink" 
                                                                    onclick="lnkImprimir_Click" TabIndex="11" 
                                                      ValidationGroup="0">Imprimir</asp:LinkButton></td>
						
                                        </tr>
                                        </table>
                                        </div>
                                        </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                            
                        </td>
						
					</tr>
					<tr>
						<td  align="left" colspan="3">
                            &nbsp;</td>
						
						<td   align="right" colspan="2">
                                            &nbsp;</td>
						
					</tr>
					</table>
						
<br />		
</div>
<script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

function VerificaLargo (source, arguments)
    {                
    var Observacion = arguments.Value.toString().length;
 //   alert(Observacion);
    if (Observacion>4000 )
        arguments.IsValid=false;    
    else   
        arguments.IsValid=true;    //Si llego hasta aqui entonces la validación fue exitosa        
}


    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de anular el protocolo?'))
    return true;
    else
    return false;
}



function muestraSelect() {
    var dom = document.domain;
    var domArray = dom.split('.');
    for (var i = domArray.length - 1; i >= 0; i--) {
        try {
            var dom = '';
            for (var j = domArray.length - 1; j >= i; j--) {
                dom = (j == domArray.length - 1) ? (domArray[j]) : domArray[j] + '.' + dom;
            }
            document.domain = dom;
            break;
        } catch (E) {
        }
    }


    var $this = $(this);

    $('<iframe src="../Muestras/MuestraSelect.aspx" />').dialog({
        title: 'Tipo de Muestras',
        autoOpen: true,
        width: 790,
        height: 420,
        modal: true,
        resizable: false,
        autoResize: true,
        overlay: {
            opacity: 0.5,
            background: "black"
        }
    }).width(800);
}
    </script>

   <%--.glyphicon .glyphicon-trash--%>
   
 
    </table>
   
 
    </div>
   
 
    </div>
   
 
 </asp:Content>
 