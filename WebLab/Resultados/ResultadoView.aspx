<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoView.aspx.cs" Inherits="WebLab.Resultados.ResultadoView" MasterPageFile="~/Site1.Master"  UICulture="es" Culture="es-AR" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
 
   <link href="jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 
  <style>

 
     
    .label {
  color: white;
  padding: 8px;
  font-size:10pt;
}

.success {background-color: #4CAF50;} /* Green */
.info {background-color: #2196F3;} /* Blue */
.warning {background-color: #ff9800;} /* Orange */
.danger {background-color: #f44336;} /* Red */
.other {background-color: #e7e7e7; color: black;} /* Gray */
       .auto-style1 {
          width: 22px;
      }
       </style>

                   <script type="text/javascript">                     

             $(function() {
                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                        $("#tabContainer").tabs({ selected: currTab });
             });

                   
      
                  </script> 

  
   

    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
  
   
    
     <asp:HiddenField runat="server" ID="HFIdPaciente" /> 
    
<asp:HiddenField runat="server" ID="HFIdProtocolo" /> 
       <span class="label label-default"><asp:Label ID="lblTitulo" runat="server" Text="CARGA DE RESULTADOS" 
                                 ></asp:Label></span>   
       <asp:HyperLink ID="hypRegresar" AccessKey="r" ToolTip="Alt+Shift+R" runat="server" CssClass="myLink">Regresar</asp:HyperLink>   
<div align="left" style="width:1180px;"   class="form-inline"  >
     <div class="panel panel-default">
           <div class="panel-heading">
  <table id="tablaPaciente" runat="server" style="width:100%;">
                                <tr>
                                  
                                    <td>
                                        <h4><asp:Label ID="lblPaciente" runat="server" Text="Label" ></asp:Label></h4>
                            <asp:Label ID="lblCodigoPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt" Visible="False"></asp:Label>
                                     
                                          <img src="../App_Themes/default/images/Renaper_(logotipo).png"  id="logoRenaper" width="40" height="40" runat="server" title="Validado con Renaper" visible="false" />
                                            <span class="label other" runat="server" id="spanRenaper">Validado con Renaper</span>          
                                        
                                    </td>
                                    <td align="right" style="vertical-align: top">
                                        &nbsp;</td>
                                      <td   >
                                     
                                           <span class="label other">  DU:&nbsp; <asp:Label ID="lblDni" runat="server" Text="Label"   ></asp:Label></span>
                                           &nbsp;<br />
                                          &nbsp;<br />
                                           <span class="label other">      Fecha Nacimiento: <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"   ></asp:Label></span>
                                       
                                   <span class="label other">       Edad: <asp:Label ID="lblEdad" runat="server" Text="Label"  ></asp:Label> </span>
                                             <span class="label other">         Sexo:
                                        <asp:Label ID="lblSexo" runat="server" Text="Label"></asp:Label>  </span>
                                        <asp:Button ID="btnPeticion" runat="server" CssClass="myButton" 
                                            OnClientClick="editPeticion(); return false;" Text="Petición Electrónica" 
                                            ToolTip="Petición Electronica" Visible="false" Width="150px" Height="23px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td   >     &nbsp;<br />
                                        <span class="label label-primary">
                                         
                                        <asp:Label ID="lblCantidadRegistros" runat="server" Text="Label"></asp:Label>
                                        </span></td>
                                    <td  >

                                      <%--  <asp:HyperLink ID="hplResultadosRed" runat="server" Target="_blank">Resultados en el Red</asp:HyperLink>--%>
                                    </td>
                                    <td   align="left">
                                        &nbsp;</td>
                                    <td   align="left">
                                     
                                    </td>
                                    <td class="auto-style1"                >
                                        &nbsp;</td>
                                    <td                >
                                        &nbsp;</td>
                                </tr>
                                </table>
               </div>
                 <table    >
					
					
					
						
					
					
				

						
					<tr>
					
				

					
						
						<td   style="vertical-align: top">
                            &nbsp;</td>
						
					
					
					
						
						<td   align="left" style="vertical-align: top">
                            &nbsp;</td>
						
						<td style="vertical-align: top">
                            &nbsp;</td>
						
					</tr>
											
					
					
					
						
					
				

						
					<tr>
					
				

					
						
						<td   style="vertical-align: top">
                            
                 
   
                                   
                                      
                                        <div  style="width:200px;height:350pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC;"> 
                                            
                                           
                                            <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                            DataKeyNames="idProtocolo" onrowcommand="gvLista_RowCommand" 
                                            onrowdatabound="gvLista_RowDataBound"     CssClass="table table-bordered bs-table" 
                                Width="100%" 
                                        >
                                         <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                         <RowStyle BackColor="#F7F6F3" ForeColor="#333333"  />
                                          <%--  <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial" 
                                            Font-Size="8pt" />--%>

                                            <Columns>
                                            <asp:BoundField DataField="numero" HeaderText="Nro." />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:ImageButton ID="Pdf" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                                            CommandName="Pdf" />
                                            </ItemTemplate>

                                            <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />

                                            </asp:TemplateField>

                                            <asp:BoundField DataField="estado" Visible="False" />

                                            </Columns>
                                        <%--    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" 
                                            Font-Names="Arial" Font-Size="8pt" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#333333" />--%>
                                            </asp:GridView>
                                     </div>
                             <ul class="pagination">
                                     <li>  

                             <asp:LinkButton ID="lnkAnterior" runat="server" CssClass="myLittleLink" ToolTip="Avanza al protocolo siguiente" 
                                onclick="lnkAnterior_Click">&lt;Posterior</asp:LinkButton> </li>
                                <li><asp:LinkButton 
                                ID="lnkPosterior" runat="server" CssClass="myLittleLink" ToolTip="Avanza al protocolo anterior"
                                onclick="lnkPosterior_Click">Anterior&gt;</asp:LinkButton>
                                    </li>
                                 </ul>
                        </td>
						
					
					
					
						
						<td   align="left" style="vertical-align: top">
                            &nbsp;  &nbsp;</td>
						
						<td style="vertical-align: top">
                            <table >
                              
							<tr>
						
						<td    colspan="3">  
                          
						<div align="left"  class="form-inline" runat="server"  id="panelResultados" >
   <div class="panel panel-default">
                    <div class="panel-heading">
     <table style="width:100%;border-spacing:5px;border-collapse: separate;">
                            
      						
					<tr>
                        <td colspan="6">
                        <asp:Label ID="lblEfector" runat="server" Text="Label"  Font-Bold="True"  
                                Font-Size="13pt"></asp:Label></td>
					</tr>
					
					<tr>
							<td  width="150px">
                                Protocolo</td>
						<td   width="250px">
                          
                              
                           <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Bold="True"  
                                Font-Size="13pt" ></asp:Label>
                        </td>
						<td   width="100px">
                            &nbsp;</td>
						<td 
                            align="left"   width="150px"  >
                          
                              
                            &nbsp;</td>
                      	<td    width="120px">
                           Fecha</td>
                        <td     align="left"  width="130px" >   
                                     <asp:Label Font-Bold="True" 
                                Font-Size="10pt" ID="lblFecha" runat="server" Text="Label"></asp:Label>    </td>
					</tr>
						
					
					
					<tr>
							<td  >
                                Solicitante:</td>
						<td colspan="3"  >
                            <asp:Label ID="lblSolicitante" runat="server" Text="Label"   Font-Bold="True" 
                                Font-Size="10pt" ></asp:Label>
                        </td>
                      	<td  >
                            Servicio:</td>
                        <td   align="left">   
                                   <asp:Label ID="lblServicio" runat="server"  Font-Bold="True" 
                                Font-Size="10pt"
                                       Text="Label"> </asp:Label>
                                    </td>
					</tr>
						
					
					
					<tr>
							<td  >
                            Médico Solicitante:</td>
						<td colspan="3"  >
                           <asp:Label ID="lblMedico" runat="server" Text=""  Font-Bold="True" 
                                Font-Size="10pt"  ></asp:Label>   
                        </td>
                      	<td  >
                            Usuario Registro:</td>
                        <td   align="left">   
                                   <asp:Label  ID="lblUsuario" runat="server" Text="Label"  Font-Bold="True" 
                                Font-Size="10pt"></asp:Label>                     
                       
                                    </td>
					</tr>
						
					
					
					<tr>
							<td  >
                                Origen:</td>
						<td  >
                            <asp:Label ID="lblOrigen"  Font-Bold="True" 
                                Font-Size="10pt" runat="server" Text="Label"   ></asp:Label>
                        &nbsp;&nbsp;
                          
                              
                            <asp:Label ID="lblNumeroOrigen"  Font-Bold="True" 
                                Font-Size="10pt" runat="server" Text="Label"   ></asp:Label>
                        </td>
						<td  >
                            &nbsp;</td>
						<td 
                            align="left"   >
                            &nbsp;</td>
                    	<td  > Fecha Registro:</td>
                        <td    align="left">      
                                <asp:Label  ID="lblFechaRegistro" runat="server" Text="Label"  Font-Bold="True" 
                                Font-Size="10pt"></asp:Label>                
                       
                        </td>
					</tr>
						
					
					
					<tr>
							<td  >
                                Diagnósticos:</td>
						<td colspan="4"  >
                            <asp:Label ID="lblDiagnostico" runat="server" Text=""  Font-Bold="True" 
                                Font-Size="10pt"  ></asp:Label>                                                                                                               
                        </td>
                        <td rowspan="2">
                            <asp:Label ID="lblNroSISA" runat="server" class="label label-success" Font-Size="14px" Text="Label" Visible="true"></asp:Label>
                             </td>
					</tr>
						
					
					
						
					
					
						<tr>
						<td  >Prácticas Solicitadas:</td>
						<td colspan="4"  >
                            <asp:Label ID="lblPedidoOriginal" runat="server" Text="Label"   Font-Bold="True" 
                                Font-Size="10pt"
                                ></asp:Label> 
						   
                            </td>
					</tr>
					</table>
                        </div>

				<div class="panel-body">
						
						
						
						    
<asp:HiddenField runat="server" ID="HFCurrTabIndex" /> 
                           
                            <div >  
                  
                            
                            <asp:Panel ID="pnlHC" runat="server" > 
                  
                            </asp:Panel>
						 

   
        
        <table width="100%">
       
        	<tr>
						
						<td  colspan="3" style="vertical-align: top">  
                             <h4> <asp:Label  ID="lblCovid" runat="server" Visible="false" ></asp:Label>&nbsp;</h4>  
                             <h4> <asp:Label  ID="lblMuestra" runat="server" ></asp:Label></h4>  
                        <asp:Label CssClass="myLabelIzquierdaGde" ForeColor="Red" ID="lblEstadoProtocolo" Visible="false" runat="server" ></asp:Label>
						     <asp:Panel ID="Panel1" runat="server"  Width="100%" BackColor="#F2F2F2"  > 
                                 <img src="../App_Themes/default/images/amarillo.gif" runat="server" id="imgRestringido"/>
                                                         <asp:Label CssClass="myLabelIzquierdaGde" ID="lblRestringido" runat="server" >RESULTADOS RESTRINGIDOS. CONSULTAR CON EL LABORATORIO</asp:Label>                                                                                                                 						 
                                               <asp:Table ID="tContenido"   CssClass="table table-bordered bs-table" 
                                                   Runat="server" CellPadding="0" CellSpacing="0" 
                                                   Width="100%" ></asp:Table>                                                                                                                                     
                                           </asp:Panel></td>
						
					</tr>
        	
        
        	<tr>
						
						
						<td  colspan="2" align="left"> 
						<table class="mytable1" width="100%">
                            <tr><td>
                            <asp:Label ID="lblObservacionResultado" runat="server" Font-Names="Courier New" 
                                Font-Size="10pt" ForeColor="Black" Font-Bold="true" ></asp:Label>
                                </td></tr>
                            </table>
                            <br />
                                                                    </td>
                                                                    </tr>
                                                                    
                                                                    
                                                  
        		
					
        

     
        	<tr>
						
						<td align="left" colspan="2"  > 
                            <asp:Panel ID="pnlReferencia" Visible="false" runat="server" Width="300px">
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
</div>


        <div class="panel-footer">	
                             <asp:ImageButton ID="imgImprimir" runat="server" 
                                ImageUrl="~/App_Themes/default/images/imprimir.jpg" 
                                   onclick="imgImprimir_Click" /> &nbsp; 

               <asp:LinkButton ID="imgPdf" Visible="false" runat="server" CssClass="btn btn-info" Text="Buscar" OnClick="imgPdf_Click" Width="140px" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar</asp:LinkButton>

                 <asp:LinkButton ID="imgAdjunto" runat="server" CssClass="btn btn-info" Text="Buscar"  OnClientClick="AdjuntoProtocoloEdit(); return false;"   Width="160px" >
                                             <span class="glyphicon glyphicon-paperclip"></span>&nbsp;Ver Adjuntos</asp:LinkButton>

						      	
            </div>
       </div>
                            </div>
</td>
						
					</tr>
			
					<tr>
						
						<td   colspan="3" align="right">
                            <asp:Panel ID="pnlImpresora" runat="server" style="border:1px solid #C0C0C0; width:100%; background-color: #EFEFEF;">                                                      
 <table width="720px" align="center">
 	<tr>
						<td   align="left" style="width: 140px; background-color: #EFEFEF;">
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

    </script>

</asp:Content>

