<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Informe.aspx.cs" Inherits="WebLab.Informes.Informe" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
 
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
    </asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
 
     <div  style="width: 950px" class="form-inline" >
           <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
      <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
  </div>
                    <div class="panel-body">
                 <table  width="900px" align="center"                                           
                    
                     cellpadding="1" cellspacing="1" >
					 
					<tr>
						<td colspan="4"><b class="mytituloTabla">
                          

                                        <asp:DropDownList ID="ddlSectorServicio" runat="server" TabIndex="5" 
                                            ToolTip="Seleccione el sector/cs" Visible="False">
                                        </asp:DropDownList>
                                        
					       </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Servicio:</td>
						<td>
                            <asp:DropDownList ID="ddlServicio" runat="server" 
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control input-sm"
                                 onselectedindexchanged="ddlServicio_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                                        
                                            </td>
						<td>
                            &nbsp;</td>
						<td class="myLabelIzquierda" rowspan="13" valign="top">
                            
                            <div class="panel panel-default">
                              <div class="panel-heading">
      Sector/Servicio:
  </div>
                    <div class="panel-body">
                                                           <asp:ListBox ID="lstSector" runat="server" 
                                 Height="180px" SelectionMode="Multiple" TabIndex="7" 
                                ToolTip="Seleccione los sectores" Width="300px"></asp:ListBox>
                        <br />
                        <ul class="pagination">
                                     <li>                                                   <asp:LinkButton 
                            ID="lnkMarcarSectores" runat="server" CssClass="myLink" 
                                                   ValidationGroup="0" OnClick="lnkMarcarSectores_Click">Todas</asp:LinkButton></li>
                                            <li><asp:LinkButton 
                            ID="lnkDesmarcarSectores" runat="server" CssClass="myLink" 
                                                   ValidationGroup="0" OnClick="lnkDesmarcarSectores_Click">Ninguna</asp:LinkButton></li>
                                </ul>
                        </div>

                            </div>
                            
                        </td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Area:</td>
						<td>
                            <asp:DropDownList ID="ddlArea" runat="server"  class="form-control input-sm"
                                 onselectedindexchanged="ddlArea_SelectedIndexChanged" 
                                TabIndex="2" ToolTip="Seleccione el area" AutoPostBack="True">
                            </asp:DropDownList>
                                        
                                            </td>
						<td>
                            &nbsp;</td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Fecha Desde:</td>
						<td class="myLabelIzquierda">
                  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de inicio"  />
                            <asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Debe ingresar el rango de fechas"                                 onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0">*</asp:CustomValidator>
                        &nbsp;hasta: &nbsp;
                           <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  /></td>
						<td>
                            &nbsp;</td>
					</tr>
					<tr>
						<td class="myLabelIzquierda">Efector Solicitante:</td>
						<td>
						
                   
                              
                            <asp:DropDownList ID="ddlEfector" runat="server" Width="250px"
                                ToolTip="Seleccione el efector" TabIndex="6" class="form-control input-sm"
                             >
                            </asp:DropDownList>
                                        
					        </td>
						<td>
						
                            &nbsp;</td>
                        </tr>
					<tr>
						<td class="myLabelIzquierda">Protocolo Desde:</td>
						<td class="myLabelIzquierda">
						
                   
                              
                    <input id="txtProtocoloDesde" runat="server" type="text" maxlength="9"                        
                       tabindex="7" class="form-control input-sm" onblur="valNumero(this)"  style="width: 100px" 
                                title="Ingrese el numero de protocolo de inicio (sin puntos ni guiones)"  />&nbsp;&nbsp;&nbsp; 
                            hasta:&nbsp;
						
                   
                              
                    <input id="txtProtocoloHasta" runat="server" type="text" maxlength="9" 
                         tabindex="8" class="form-control input-sm"  onblur="valNumero(this)"
                                style="width: 100px" title="Ingrese el numero de protocolo de fin (sin puntos ni guiones)"  /><asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                ErrorMessage="Numero de Protocolo" CssClass="myLabel"
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="0" 
                                >Sólo numeros</asp:CustomValidator>
                        </td>
						<td>
						
                            &nbsp;</td>
                        </tr>
                        
					<tr>
						<td class="myLabelIzquierda">&nbsp;</td>
						<td class="myLabelIzquierda">
						
                   
                              
                            <asp:CheckBox 
                                ID="chkDesdeUltimoListado" runat="server" CssClass="myLabelRojo" 
                                Text="Desde último nro. listado" />
                        </td>
						<td>
						
                            &nbsp;</td>
                        </tr>
                        
						<tr>
						<td class="myLabelIzquierda">Origen:</td>
						<td>
                            <asp:DropDownList ID="ddlOrigen" runat="server" 
                                ToolTip="Seleccione el origen" TabIndex="9" class="form-control input-sm">
                            </asp:DropDownList>
                        </td>                
					        </tr>
					        
					        <tr>
											
						
						<td class="myLabelIzquierda">Prioridad:</td>
						<td>
                            <asp:DropDownList ID="ddlPrioridad" runat="server" 
                                ToolTip="Seleccione la prioridad" TabIndex="10" class="form-control input-sm">
                            </asp:DropDownList>
                        </td>                
					        </tr>
											
					     <%--   <tr>
											
						
						<td class="myLabelIzquierda">Tipo de muestra:</td>
						<td>
                            <asp:Label ID="lblMuestrasSeleccionadas" runat="server" Text="Label"></asp:Label>
                            <input id="HFIdMuestra" runat="server" type="hidden" />
                            <asp:ImageButton ID="imgAgregarMuestras" runat="server"  OnClientClick="muestraSelect(); return false;"                                     
                                                    ImageUrl="~/App_Themes/default/images/add.png" />
                        </td>                
					        </tr>--%>
											
						<tr>
						<td class="myLabelIzquierda" style="vertical-align: top">Estado:</td>
						<td class="myLabelIzquierda"  style="vertical-align: top">
                            <asp:RadioButtonList ID="rdbEstadoAnalisis" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Solo Pendientes de Resultado</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>    
                            
                                        
					        </tr>

                     
                            
						<asp:Panel ID="pnlHojaTrabajoResultado" runat="server" Visible="False">
                                        
                                          
                            	<tr>
						
                                          
                       	<td class="myLabelIzquierda" style="vertical-align: top">Con Resultados:</td>
						<td class="myLabelIzquierda"  style="vertical-align: top">
                            <asp:RadioButtonList ID="rdbHojaConResultados" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                <asp:ListItem Value="1">Si</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>       
                            </tr>
                                 <tr>
                                      

                                       <td   colspan="3" class="myLabelIzquierda">
                                          <hr />
                                             <h4>     <label id="lblCaracterCovid" runat="server" class = "label label-danger"> Caracter-CoVid19: </label></h4>
					                
                                            
                                           <asp:CheckBoxList ID="chkCaracterCovid" runat="server" RepeatDirection="Vertical" RepeatColumns="4">
                                           </asp:CheckBoxList>
                                           </td>
                                     </tr> 
                            </asp:Panel>
																	
						<tr>
						<td class="myLabelIzquierda" style="vertical-align: top"> 
                             <anthem:LinkButton CssClass="btn btn-warning" ID="btnSeleccionarTipoMuestra" runat="server"   Width="160px"  Visible="false"
                                                                    onclick="btnSeleccionarTipoMuestra_Click" >Elegir Tipos de muestras</anthem:LinkButton></td>
						<td class="myLabelIzquierda"  style="vertical-align: top">
                          <anthem:ListBox ID="lstMuestra" runat="server" Height="150px" Width="260px" 
                                                                    SelectionMode="Multiple" Visible="False">
                                                                </anthem:ListBox>
                        </td>                
					        </tr>

											
											
					
																	
						<tr>
						<td   colspan="4" align="right">
                                          <asp:Panel ID="pnlEtiquetaCodigoBarras" runat="server" Visible="False">
                                              <asp:Button ID="btnImprimirEtiqueta" runat="server" Text="Imprimir"   OnClientClick="return PreguntoImprimir();"
                                                  CssClass="myButton"  onclick="btnImprimirEtiqueta_Click" ValidationGroup="0" />
                                          </asp:Panel>
                            </td>
                            </tr>
																	
						<tr>
						<td   colspan="4" >
                            <br />
                    
                                           <asp:Panel ID="pnlAnalisisFueraHT" runat="server">
                                                         <div class="panel panel-default">
                              <div class="panel-heading">
         Análisis a incluir en hoja de trabajo:
  </div>
                    <div class="panel-body">
                                               <table style="width:100%;">
                                                   
                                                   <tr>
                                                       <td align="left" class="myLabel" style="vertical-align: top">
                                                           Seleccionar:
                                                           <asp:LinkButton ID="lnkMarcarAnalisis" runat="server" CssClass="myLink" 
                                                               onclick="lnkMarcarAnalisis_Click" ValidationGroup="0">Todas</asp:LinkButton>
                                                           &nbsp;&nbsp;
                                                           <asp:LinkButton ID="lnkDesmarcarAnalisis" runat="server" CssClass="myLink" 
                                                               onclick="lnkDesmarcarAnalisis_Click" ValidationGroup="0">Ninguna</asp:LinkButton>
                                                       </td>
                                                       <td align="left" class="myLabelIzquierda" style="vertical-align: top">
                                                           <asp:RequiredFieldValidator ID="rfvAnalisis" runat="server" 
                                                               ControlToValidate="lstAnalisis" 
                                                               ErrorMessage="Debe seleccionar al menos un analisis" ValidationGroup="1">Debe 
                                                           seleccionar al menos un analisis</asp:RequiredFieldValidator></td>
                                                   </tr>
                                                   <tr>
                                                       <td align="left" class="myLabelIzquierda" style="vertical-align: top">
                                                           <asp:ListBox ID="lstAnalisis" runat="server" CssClass="myTexto" Height="220px" 
                                                               SelectionMode="Multiple" TabIndex="7" ToolTip="Seleccione los analisis" 
                                                               Width="450px"></asp:ListBox>
                                                       </td>
                                                       <td align="left" class="myLabelIzquierda" style="vertical-align: bottom">
                                                             <img alt="" src="../App_Themes/default/images/pdf.jpg"/>&nbsp;<asp:LinkButton 
                            ID="lnkPDFAnalisisFueraHT" runat="server" CssClass="myLittleLink" onclick="lnkPDFAnalisisFueraHT_Click" ValidationGroup="1" 
                                                   TabIndex="8">Visualizar 
                                               en formato pdf</asp:LinkButton>
                                               <br />
                                                            
                                                           <asp:LinkButton ID="lnkImprimirAnalisisFueraHT" runat="server" 
                                                               CssClass="myLittleLink" onclick="lnkImprimirAnalisisFueraHT_Click" 
                                                               TabIndex="11" ValidationGroup="1">Imprimir</asp:LinkButton>
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td colspan="2">
                                                           <hr /></td>
                                                   </tr>
                                                   <tr>
                                                       <td>
                                                           &nbsp;</td>
                                                       <td>
                                                           &nbsp;</td>
                                                   </tr>
                                               </table>
                        </div>
                                  </div>
                                           </asp:Panel>
                        
                            </td>
						</tr>
						
						<tr>
						<td   colspan="4" >
                    
                                           <asp:Panel ID="pnlHojaTrabajo" runat="server">
                                                       <div class="panel-footer">			
                                               <table style="width:100%;">
                                                   <tr>
                                                       <td align="left">
                                                           Seleccionar: <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLink" 
                                                               onclick="lnkMarcar_Click" ValidationGroup="0">Todas</asp:LinkButton> &nbsp;
                                                           <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="myLink" 
                                                               onclick="lnkDesmarcar_Click" ValidationGroup="0">Ninguna</asp:LinkButton>
                                                       </td>
                                                       <td align="right">
                                                           <asp:LinkButton 
                                                               ID="lnkImprimir" runat="server" CssClass="myLittleLink" 
                                                               onclick="lnkImprimir_Click" TabIndex="11" ValidationGroup="0">Imprimir</asp:LinkButton>
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td colspan="2">
                                                         
                                                           <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"   CssClass="table table-bordered bs-table" 
                                                               AutoUpdateAfterCallBack="False" BorderColor="#3A93D2" BorderStyle="Solid" 
                                                               BorderWidth="3px" CellPadding="1" DataKeyNames="idHojaTrabajo" 
                                                               EmptyDataText="No se encontraron registros para los filtros de busqueda ingresados" 
                                                                GridLines="Horizontal" 
                                                               onrowcommand="gvLista_RowCommand1" onrowdatabound="gvLista_RowDataBound" 
                                                               Width="100%" OnSelectedIndexChanged="gvLista_SelectedIndexChanged">
                                                            
                                                               <Columns>
                                                                   <asp:TemplateField HeaderText="Sel.">
                                                                       <ItemTemplate>
                                                                           <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                                       </ItemTemplate>
                                                                       <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                   </asp:TemplateField>
                                                                   <asp:BoundField DataField="servicio" HeaderText="Tipo de Servicio">
                                                                       <HeaderStyle HorizontalAlign="Left" />
                                                                       <ItemStyle Width="20%" />
                                                                   </asp:BoundField>
                                                                   <asp:BoundField DataField="area" HeaderText="Area">
                                                                       <HeaderStyle HorizontalAlign="Left" />
                                                                       <ItemStyle Width="30%" />
                                                                   </asp:BoundField>
                                                                   <asp:BoundField DataField="codigo" HeaderText="Codigo">
                                                                       <HeaderStyle HorizontalAlign="Left" />
                                                                       <ItemStyle Width="30%" />
                                                                   </asp:BoundField>
                                                                   <asp:TemplateField>
                                                                       <ItemTemplate>
                                                                           <asp:ImageButton ID="Pdf" runat="server" CommandName="Pdf" ToolTip="Exporta PDF" 
                                                                               ImageUrl="~/App_Themes/default/images/pdf.jpg"  ValidationGroup="0"/>
                                                                       </ItemTemplate>
                                                                       <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                                   </asp:TemplateField>
                                                                     <asp:TemplateField >
                                                                       <ItemTemplate>
                                                                           <asp:ImageButton ID="Imprimir" runat="server" CommandName="Imprimir" ToolTip="Envia a impresora seleccionada"
                                                                               ImageUrl="~/App_Themes/default/images/imprimir.jpg" ValidationGroup="0" />
                                                                       </ItemTemplate>
                                                                       <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                                 
                                                                          </asp:TemplateField>
                                                                     <asp:TemplateField Visible="false">
                                                                       <ItemTemplate>
                                                                           <asp:ImageButton ID="Excel" runat="server" CommandName="Imprimir" ToolTip="Envia a impresora seleccionada"
                                                                               ImageUrl="~/App_Themes/default/images/imprimir.jpg" ValidationGroup="0" />
                                                                       </ItemTemplate>
                                                                       <ItemStyle Height="20px" HorizontalAlign="Center" Width="20px" />
                                                                 
                                                                          </asp:TemplateField>
                                                               </Columns>
                                                               <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                               <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                               <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                               <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
                                                               <EditRowStyle BackColor="#999999" />
                                                               <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                           </asp:GridView>
                                                         
                                                       </td>
                                                   </tr>
                                               </table>
                                                           </div>
                                           </asp:Panel>
                                
                            </td>
       </tr>
						
						<tr>
						<td   colspan="2">
                                            <asp:Label ID="lblMensaje" runat="server" Text="Label" Visible="False"></asp:Label>
                            </td>
						
						<td  >
                                            &nbsp;</td>
						
						<td  >
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td   colspan="2">
                                        
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los siguientes datos" ShowMessageBox="True" 
                                                ValidationGroup="0" ShowSummary="False" />
                        </td>
						
						<td  >
                                        
                                            &nbsp;</td>
						
						<td  >
                                        
                                            &nbsp;</td>
						
					</tr>
								
                            
											
						<tr>
						<td align="left" colspan="4">
						  
                                      <div align="left" runat="server" id="pnlImpresora" style="border:1px solid #C0C0C0; width:100%; background-color: #EFEFEF;" >
 <table width="720px" align="center">
 	<tr>
						<td class="myLabelIzquierda" align="left" style="width: 140px; background-color: #EFEFEF;">
                                        Impresora del sistema: </td>
						<td align="left">
                                        <asp:DropDownList ID="ddlImpresora" runat="server" CssClass="myList" >
                                        </asp:DropDownList>
                            </td>
						
                                        </tr>
                                        </table>
														
 </div>
                                      </td>
						<td align="right">
                                        &nbsp;</td>
						<td align="right">
                                        &nbsp;</td>
                                        </tr>
					</table>						
 </div>
               </div>
       </div>
 <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
 <script language="javascript" type="text/javascript">

  


     function PreguntoImprimir() {
         if (confirm('¿Está seguro de enviar a imprimir a la impresora seleccionada con los filtros seleccionados?'))
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
 
 </asp:Content>