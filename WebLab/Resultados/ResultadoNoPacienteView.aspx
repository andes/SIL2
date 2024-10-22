<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoNoPacienteView.aspx.cs" Inherits="WebLab.Resultados.ResultadoNoPacienteView" MasterPageFile="~/Site1.Master"  UICulture="es" Culture="es-AR" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
 
   <link href="jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                   <script type="text/javascript">                     
      
                  </script> 

  
    

    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
       
    
<div  style="width: 920px" class="form-inline" >
   <asp:HiddenField runat="server" ID="HFIdPaciente" />
     
<asp:HiddenField runat="server" ID="HFIdProtocolo" />  
                     <div class="panel panel-primary">
                    <div class="panel-heading">        <asp:Label ID="lblTitulo" runat="server" Text="Consulta de Resultados" 
                                ></asp:Label>
  </div>
                    <div class="panel-body">

                 <table  width="100%"                    
                     >
					
					
						
						<tr>
						
					
					
					
						
						<td   align="left" style="vertical-align: top">
                            &nbsp;</td>
						
						<td style="vertical-align: top">
                            <table >
                              
							<tr>
						
						<td    colspan="3">  
						
                    <div class="panel-body">

						<table width="100%" cellspacing="0"                    cellpadding="0">
      						
					
					
					<tr>
							<td class="auto-style2" >
                                Protocolo:</td>
						<td class="auto-style3"  >
                       <strong>
                              
                            <asp:Label ID="lblProtocolo" runat="server" Text="Label" 
                                Font-Size="11pt" ></asp:Label></strong>   
                        </td>
						<td class="auto-style4" >
                            &nbsp;</td>
						
                      	<td class="auto-style7" >
                           Fecha</td>
                        <td     align="left" class="auto-style8"   >   
                       <strong>              <asp:Label   
                                 ID="lblFecha" runat="server" Text="Label"></asp:Label>  </strong>  </td>
					</tr>
						
					
					
					<tr>
							<td class="auto-style2" >
                                Tipo de Producto:</td>
						<td class="auto-style3"  >
                          <strong><asp:Label  ID="lblMuestra" runat="server" ></asp:Label></strong>
                        </td>
						<td class="auto-style4" >
                            &nbsp;</td>
						
                      	<td class="auto-style7" >
                            Nro. de Origen:</td>
                        <td   align="left" class="auto-style8">   
                            
                                <strong>
                                <asp:Label ID="lblNumeroOrigen" runat="server"  Text="Label"></asp:Label>
                                </strong>
                            
                                    </td>
					</tr>
						
					
					
					<tr>
							<td class="auto-style2" >
                                Muestra:</td>
						<td class="auto-style3"  >
                          <strong><asp:Label  ID="lblConservacion" runat="server" ></asp:Label></strong>
                        </td>
						<td class="auto-style4" >
                            &nbsp;</td>
						
                      	<td class="auto-style7" >
                            &nbsp;</td>
                        <td   align="left" class="auto-style8">   
                            
                                &nbsp;</td>
					</tr>
						
					
					
					<tr>
							<td class="auto-style2" >
                                Descripcion:</td>
						<td class="auto-style3"  >
                         <strong>   <asp:Label ID="lblDescripcion" runat="server" ></asp:Label></strong>
                            </td>
						<td class="auto-style4" >&nbsp;</td>
						
                      	<td class="auto-style7" >
                            Usuario:</td>
                        <td   align="left" class="auto-style8">   
                         
                                <strong>
                                <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label>
                                
                                </strong>
                         
                          
                                    </td>
					</tr>
						
					
					
					<tr>
							<td class="auto-style2" >
                               Sector:</td>
						<td class="auto-style3"  >
                         <strong><asp:Label ID="lblSector" runat="server" Text="Label"  ></asp:Label></strong>
                        </td>
						<td class="auto-style4" >
                            &nbsp;</td>
						
                      	<td class="auto-style7" >
                               Fecha de Registro:</td>
                        <td   align="left" class="auto-style8">   
                          
                           <strong>
                            <asp:Label ID="lblFechaRegistro" runat="server" Text="Label"></asp:Label>
                            <asp:Label ID="lblHoraRegistro" runat="server" Text="Label"></asp:Label>
                          </strong>   
                              
                         
                                    </td>
					</tr>
						
					
					
						
					
					
						<tr>
						<td class="auto-style2" >Prácticas Solicitadas:</td>
						<td colspan="4"  >
                            <p><strong><asp:Label ID="lblPedidoOriginal" runat="server" Text="Label"  
                                ></asp:Label> </strong></p>
						   
                            </td>
					</tr>
					</table>
                      
                                             </div>
						</td>
						</tr>
				
					<tr>
						
						<td    colspan="3">  
						
						    
<asp:HiddenField runat="server" ID="HFCurrTabIndex" /> 
                           
                            <asp:Label ID="lblEstadoProtocolo" runat="server"  Visible="false"></asp:Label>
                           
                            <div id="tabContainer">  
                  
                            
                            <asp:Panel ID="pnlHC" runat="server" > 
                  
                            </asp:Panel>
						 

        <div id="tab1" >   
        
                       <div class="panel panel-default">
                    <div class="panel-heading">RESULTADOS OBTENIDOS
  </div>
                    <div class="panel-body">
        <table width="850px">
       
        	<tr>
						
						<td  colspan="3" style="vertical-align: top"> 
						     <asp:Panel ID="Panel1" runat="server"  Width="100%" >                                                                                                                                      						 
                                               <asp:Table ID="tContenido"  
                                                   Runat="server" CellPadding="0" CellSpacing="0" 
                                                   Width="100%" ></asp:Table>                                                                                                                                     
                                           </asp:Panel></td>
						
					</tr>
        	
        	<tr>
						
						
						<td  colspan="2" align="left"> 
						
                            <asp:Label ID="lblObservacionResultado" runat="server" Font-Names="Courier New" 
                                Font-Size="9pt" ForeColor="Black" Visible="False"></asp:Label>
                            
                                                                    </td>
                                                                    </tr>
                                                                    
                                                                    
                                                  
        		
					
        	<tr>
						
						<td class="myLabel" style="vertical-align: top" colspan="3"> 
                           <hr /></td>
						
					</tr>
     
        	<tr>
						
						<td align="left" colspan="2"  > 
                            <asp:Panel ID="pnlReferencia" runat="server" Width="300px">
<span class="label label-default">Dentro de V.R</span>
                                <span class="label label-danger">Fuera de V.R</span>

                            </asp:Panel> 
                                                        
                            </td>
						
						<td align="right"  > 
						       <span id="spanadjunto" runat="server" class="label label-danger">
                                   <asp:Label ID="lblAdjunto" runat="server" Text="El protocolo tiene archivos adjuntos"></asp:Label></span>
                            </td>
						
					</tr>
        	</table>
                        </div>
                           </div>
                                          
                                           </div>
                                               
        
        
                                               <div  id="tab3" >
                                               <asp:Panel runat="server" Height="600px" id="pnlAntecedentes" >
                                                   <table width="850">
                                                       <tr>
                                                           <td class="myLabelIzquierdaGde">
                                                               &nbsp;
                                                               Análisis:&nbsp;&nbsp;
                                                               
                                                               <asp:DropDownList ID="ddlItem" runat="server">
                                                               </asp:DropDownList>
                                                              
                                                                <asp:Button ID="btnVerAntecendente" runat="server"  CssClass="myButtonGrisLittle"
                                                                   onclick="btnVerAntecendente_Click" Text="Ver Antecedentes" Width="120px" />
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               &nbsp;
                                                               &nbsp;
                                                               &nbsp;
                                                               <asp:GridView ID="gvAntecedente" runat="server" 
                                                                   CellPadding="1" EmptyDataText="No se encontraron antecedentes." Font-Size="9pt" 
                                                                   Width="100%" CssClass="mytable2">
                                                                   <HeaderStyle BackColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                                                                       ForeColor="#333333" BorderColor="#666666" />
                                                               </asp:GridView>
                                                           </td>
                                                       </tr>
                                                       <tr>
                                                           <td>
                                                               &nbsp;
                                                           </td>
                                                       </tr>
                                                   </table>
                                               </asp:Panel> 
                                               
                                               </div>
                                               
                                               
                                          
</div>
</td>
						
					</tr>
					<tr>
						
						<td   colspan="3" align="right">
                       
                             </td>
						
					</tr>
					
					<tr>
						
						<td   colspan="3" align="right">
                            <asp:Panel ID="pnlImpresora" runat="server" style="border:1px solid #C0C0C0; width:100%; background-color: #EFEFEF;">                                                      
 <table width="720px" align="center">
 	<tr>
						<td class="myLabelIzquierda" align="left" style="width: 140px; background-color: #EFEFEF;">
                                        Impresora del sistema: </td>
						<td  align="left">
                                        <asp:DropDownList ID="ddlImpresora" runat="server" CssClass="myList" >
                                        </asp:DropDownList>
                            </td>
						
                                        </tr>
                                        </table>
														
 </asp:Panel>
                         </td>
						
					</tr>
					        </table>
                            </td>
						
					</tr>
											
					
					
					
						
						</table>		
                        
                        </div>
                         <div class="panel-footer">	
                             <table width="100%">
                                 <tr >
                                     <td align="left"><asp:HyperLink ID="hypRegresar" AccessKey="r" ToolTip="Alt+Shift+R" runat="server" class="btn btn-default" Width="100px"><span class="glyphicon glyphicon-arrow-left"></span>Regresar</asp:HyperLink>
                             </td>
                                     
                                     <td align="right">
                                         <asp:ImageButton ID="imgImprimir" runat="server" 
                                ImageUrl="~/App_Themes/default/images/imprimir.jpg" 
                                   onclick="imgImprimir_Click" /> &nbsp; 
						                 <asp:LinkButton ID="imgPdf" runat="server" CssClass="btn btn-info" Text="Buscar" OnClick="imgPdf_Click1" Width="140px" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar</asp:LinkButton>
                                         &nbsp; &nbsp; 
       <asp:LinkButton ID="imgAdjunto" runat="server" CssClass="btn btn-info" Text="Buscar"  OnClientClick="AdjuntoProtocoloEdit(); return false;"   Width="160px" >
                                             <span class="glyphicon glyphicon-paperclip"></span>&nbsp;Archivos Adjuntos</asp:LinkButton>
 

                          
                                     </td>
                                 </tr>
                             </table>
                             
                             </div>				
 </div>
    </div>
 <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
     var idProtocolo = $("#<%= HFIdProtocolo.ClientID %>").val();




    var idPaciente = $("#<%= HFIdPaciente.ClientID %>").val();

  
  function AntecedenteView(idAnalisis, idPaciente, ancho, alto) {
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

        $('<iframe src="AntecedentesAnalisisView.aspx?idAnalisis=' +  idAnalisis+ '&idPaciente='+idPaciente+'" />').dialog({
            title: 'Evolucion',
            autoOpen: true,
            width:ancho,
            height: alto,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(800);
    }
    function editPeticion() {
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
        $('<iframe src="../PeticionElectronica/Default.aspx?idPaciente=' + idPaciente + '&idOrigen=3" />').dialog({
            title: 'Nueva Peticion',
            autoOpen: true,
            width: 950,
            height: 600,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(1020);
    }

       function AdjuntoEdit(idDetalle,idTipoServicio,operacion) {
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
        $('<iframe src="AdjuntoEdit.aspx?idDetalleProtocolo=' + idDetalle + '&idTipoServicio='+idTipoServicio+'&Operacion='+operacion+'" />').dialog({
            title: 'Adjuntar',
            autoOpen: true,
            width: 750,
            height: 500,
            modal: true,
            resizable: false,
            autoResize: true,
       
           <%-- buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },--%>
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(760);
       }

       function AdjuntoProtocoloEdit() {
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
         $('<iframe src="../Protocolos/ProtocoloAdjuntar.aspx?desde=resultado&idProtocolo=' + idProtocolo + '" />').dialog({
            title: 'Archivos Adjuntos',
            autoOpen: true,
            width: 900,
            height: 650,
            modal: true,
            resizable: false,
            autoResize: true,
            <%--  open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnActualizar))%>; }               
            },--%>
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(920);
     }

    </script>

</asp:Content>

