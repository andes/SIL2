<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeList4.aspx.cs" Inherits="WebLab.Derivaciones.InformeList4" MasterPageFile="~/Site1.Master" %>
<%--<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>--%>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript" src="../script/jquery.min.js"></script> 
  <script type="text/javascript" src="../script/jquery-ui.min.js"></script> 
  <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript">
       
        function PreguntoEliminar() {
            if (confirm('¿Está seguro de eliminar el registro?'))
                return true;
            else
                return false;
        }

        function envolverEncabezadoImpresion() {
            try {
                /* Envuelve la fila de encabezado de la grilla en thead para que se repita en cada página impresa */
                jQuery('.contenedorLista table').filter(':visible').each(function () {
                    var $t = jQuery(this);
                    if ($t.children('thead').length === 0) {
                        var $fila = $t.children('tr').first();
                        if ($fila.length) { $fila.wrap('<thead></thead>'); }
                    }
                });
            } catch (e) { }
        }

        /* Se ejecuta tanto con el botón Imprimir como con Ctrl+P del navegador */
        if (window.addEventListener) {
            window.addEventListener('beforeprint', envolverEncabezadoImpresion, false);
        }

        function imprimirListado() {
            envolverEncabezadoImpresion();
            window.print();
            return false;
        }
       
        function AgregarDeterminaciones() {
            var idLoteDerivacion = document.getElementById('<%= HFIdLote.ClientID %>').value;
            var idEfectorDerivacion = document.getElementById('<%= HFIdEfectorDerivacion.ClientID %>').value;

            $('<iframe src="EditarListaLote.aspx?id=' + idLoteDerivacion + '&idEfectorDerivacion=' + idEfectorDerivacion+'" />').dialog({
                title: 'Agregar Determinaciones a Lote ' + idLoteDerivacion,
                autoOpen: true,
                width: 800,
                height: 590,
                modal: true,
                resizable: false,
                autoResize: true,
                open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },

                buttons: {
                    'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.btnAgregarDeterminaciones))%>; }
                },
                overlay: {
                    opacity: 0.5,
                    background: "black"
                }

            }).width(800);

          
        }

        function NoEnviarDeterminaciones(listaIdDetalle) {


            $('<iframe src="DerivacionAnular.aspx?Lista=' + listaIdDetalle  +'" />').dialog({
                title: 'Marcar como no enviado',
                autoOpen: true,
                width: 500,
                height: 410,
                modal: true,
                resizable: false,
                autoResize: true,
                open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide(); },

                buttons: {
                    'Cerrar': function () {
                        $(this).dialog('close');
                        document.getElementById('<%= btnActualizar.ClientID %>').click();
                    }
                },
                overlay: {
                    opacity: 0.5,
                    background: "black"
                }

            }).width(500);


        }
    </script>
    <style type="text/css">
        /* =================== PANTALLA =================== */
        @media screen {
            /* Wrapper de la página */
            .pagina-ancho {
                width: 1050px;
            }

            /* Contenedor con scroll para la lista (altura fija solo en pantalla) */
            .contenedorLista {
                width: 100%;
                height: 450pt;
                overflow: scroll;
                overflow-x: hidden;
                border: 1px solid #CCCCCC;
                background-color: #F3F3F3;
            }
        }

        /* =================== IMPRESIÓN =================== */
        @media print {
            @page {
                margin: 1.5cm;
            }

            html, body {
                width: auto !important;
                height: auto !important;
                overflow: visible !important;
                background: #fff !important;
                margin: 0 !important;
                border: none !important;
            }

            body * {
                visibility: hidden;
            }

            #contenidoImprimir,
            #contenidoImprimir * {
                visibility: visible;
            }
            
            form > div > table {
                display: none !important;
            }

            form > div[style*="lavender"] {
                display: none !important;
            }

            /* Ocultar botones y controles de la página */
            .no-imprimir {
                display: none !important;
            }

            /* El panel arranca en el borde de la hoja (sin espacio del header/menú/pie) */
            #contenidoImprimir {
                position: absolute;
                left: 0;
                top: 0;
                width: 100% !important;
                margin: 0;
                padding: 0;
                border: none !important;
                box-shadow: none !important;
                background: #fff !important;
            }

            /* Anchos fijos del layout (table width="1000px", grilla 98%) no deben desbordar la hoja */
            #contenidoImprimir table {
                width: 100% !important;
            }

            /* El contenedor de la lista no debe recortar ni ocupar altura fija (elimina el hueco de media página) */
            .contenedorLista {
                width: 100% !important;
                height: auto !important;
                max-height: none !important;
                overflow: visible !important;
                overflow-x: visible !important;
                border: none !important;
                background: none !important;
            }

            /* Columnas que no se imprimen (Sel., Eliminar) */
            .col-imprimir-oculta {
                display: none !important;
            }

            /* Filas enteras de la grilla */
            .contenedorLista table tr {
                page-break-inside: avoid;
            }

            #contenidoImprimir .panel-body > table > tr,
            #contenidoImprimir .panel-body > table > tbody > tr {
                page-break-inside: auto !important;
            }
        }
    </style>
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
<div align="left" class="pagina-ancho">
        <div class="panel panel-default" id="contenidoImprimir">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-6">
                        <b> <asp:Label ID="lblTitulo" Text="" runat="server"></asp:Label></b>
                        <br />
                        <b> <asp:Label ID="lblEstadoLote" Text="" runat="server"></asp:Label></b>
                        <br />
                        <asp:Label ID="lblSubTitulo" Text="" runat="server" Visible="false"></asp:Label>
                    </div>
                    <div class="col-md-6" align="right" rowspan="4">
                        <asp:Panel runat="server" ID="pnlNroLote">
                            <h3><span class="label label-default">
                                <asp:Label ID="lblNroLote" runat="server"></asp:Label>
                            </span></h3>
                        </asp:Panel>
                    </div>
                </div>
                
               
            </div>

			<div class="panel-body">
                <asp:HiddenField ID="HFListaDetalles" runat="server" />
                <asp:HiddenField ID="HFIdLote" runat="server" />
                <asp:HiddenField ID="HFIdEfectorDerivacion" runat="server" />
				<table  width="1000px"  >
                    <tr>
				        <td style="vertical-align: top" colspan="3" class="no-imprimir">
                            <asp:Panel id="Panel1"   runat="server" >
                                <table class="myTabla" width="1000px">
                                    <tr style="vertical-align: middle">
                                        <td align="right">
                                            <asp:Button ID="btnGuardar" runat="server" CausesValidation="true" CssClass="btn btn-primary" Width="100" Text="Crear Lote"
                                                OnClick="btnGuardar_Click" ValidationGroup="0" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnImprimir" runat="server" CssClass="btn btn-primary" Width="100" Text="Imprimir" ToolTip="Ctrl+P" OnClientClick="return imprimirListado();" />

                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnNoEnviado" runat="server" CssClass="btn btn-danger" Width="130" Font-Size="8" Text="Marcar No enviado"
                                                ValidationGroup="1" OnClick="btnNoEnviado_Click" />

                                             &nbsp;&nbsp;
                                            <asp:Button ID="btnExcluir" runat="server" CssClass="btn btn-danger" Width="130" Font-Size="8" Text="Excluir del lote"
                                                ValidationGroup="2" OnClick="btnNoEnviado_Click" Visible="false"  />

                                            &nbsp;&nbsp;
                                             <asp:Button ID="btnAgregarDeterminaciones" runat="server" Text="Agregar Determinaciones" Visible="false" CssClass="btn btn-primary" Width="200"
                                                 OnClientClick="AgregarDeterminaciones(); return false; " OnClick="btnAgregarDeterminaciones_Click" />
                                            <asp:Button
                                                ID="btnActualizar"
                                                runat="server"
                                                Style="display: none"
                                                OnClick="btnActualizar_Click" />
                                        </td>
                                    </tr>
                                  
                                </table>
                            </asp:Panel>
                        </td>
			        </tr>

				    <tr>
					    <td class="myLabelIzquierdaGde no-imprimir" colspan="3"> <hr /></td>
				    </tr>
                   
                    <tr><td class="no-imprimir"> <br /></td></tr>
				    <tr>
					    <td colspan="2" class="no-imprimir">
                            <asp:CustomValidator ID="cvGeneral" runat="server" OnServerValidate="cvGeneral_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvNoEnviado" runat="server" OnServerValidate="cvNoEnviado_ServerValidate" ValidationGroup="1"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvExcluir" runat="server" OnServerValidate="cvExcluir_ServerValidate" ValidationGroup="2"></asp:CustomValidator>
                             <div class="mylabelizquierda" runat="server" id="linkSeleccionar">Seleccionar:                                           
                                <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="myLittleLink"  onclick="lnkMarcar_Click">Todas</asp:LinkButton>&nbsp;
                                <asp:LinkButton  ID="lnkDesMarcar" runat="server" CssClass="myLittleLink"   onclick="lnkDesMarcar_Click" >Ninguna</asp:LinkButton>
                                    &nbsp;&nbsp;
                              </div>
                        </td>
						
					    <td align="right" class="no-imprimir">
                            <asp:Label ID="CantidadRegistros" runat="server"  forecolor="Blue" />
                        </td>
				    </tr>

				    <tr>
					    <td colspan="3">
                            <div class="contenedorLista">
                                <!-- Lista para el alta de lote -->
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                                    DataKeyNames="idDetalleProtocolo"   Width="98%" CellPadding="0"  ForeColor="#666666" PageSize="1" 
                                    EmptyDataText ="No se encontraron protocolos para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                                    BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" onrowcommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                                    <Columns>
            
                                    <asp:TemplateField HeaderText="Sel." >
                                        <HeaderStyle CssClass="col-imprimir-oculta" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" CssClass="col-imprimir-oculta" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="numero"  HeaderText="Protocolo" >
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
                                    <asp:BoundField DataField="determinacion" HeaderText="Practica a derivar">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="efectorderivacion" HeaderText="Efector">
                                        <ItemStyle Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="username" HeaderText="Usuario" >
                                        <ItemStyle Width="15%" />
                                    </asp:BoundField>
                                    <asp:BoundField  DataField="observacion" HeaderText="Observacion">
                                        <ItemStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Motivo Cancelaci&oacute;n">
                                          <ItemTemplate> <asp:Label ID="lbl_motivo" runat="server" Text='<%# Eval("motivo") %>'/> </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate> <asp:Label ID="lbl_estado" runat="server" Text='<%# Eval("estado") %>'/> </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle CssClass="col-imprimir-oculta" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="Eliminar" OnClientClick="return PreguntoEliminar();" runat="server" Text="" Width="20px" CommandName="Eliminar" CommandArgument='<%# Eval("idDetalleProtocolo") %>'>
                                                <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" CssClass="col-imprimir-oculta" />
                                    </asp:TemplateField>
                                   </Columns>
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" 
                                    Font-Names="Arial" Font-Size="8pt" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>

                                <!-- Lista para modificacion -->
                                  <asp:GridView ID="gvListaEdit" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                                    DataKeyNames="idDetalleProtocolo"   Width="98%" CellPadding="0"  ForeColor="#666666" PageSize="1" 
                                    EmptyDataText ="No se encontraron protocolos para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                                    BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                                    <Columns>
            
                                    <asp:TemplateField HeaderText="Sel." >
                                        <HeaderStyle CssClass="col-imprimir-oculta" />
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSel" runat="server" EnableViewState="true"  />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" CssClass="col-imprimir-oculta" />
                                    </asp:TemplateField>

                                         <asp:TemplateField Visible="false">
                                           <ItemStyle Width="5%" HorizontalAlign="Center" />
                                           <ItemTemplate>
                                               <asp:Literal ID="estado" runat="server" />
                                           </ItemTemplate>
                                       </asp:TemplateField>

                                    <asp:BoundField DataField="numero"  HeaderText="Protocolo" >
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
                                    <asp:BoundField DataField="determinacion" HeaderText="Practica a derivar">
                                        <ItemStyle Width="20%" />
                                    </asp:BoundField>
                                   
                                    <asp:BoundField DataField="username" HeaderText="Usuario" >
                                        <ItemStyle Width="15%" />
                                    </asp:BoundField>
                                   
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate> <asp:Label ID="lbl_estado" runat="server" Text='<%# Eval("estado") %>'/> </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                   </Columns>
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White" Font-Names="Arial" Font-Size="8pt" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            </asp:GridView>
                            </div>
                        </td>
						
				    </tr>
				    <tr>
					    <td colspan="3" class="no-imprimir"><hr /></td>
				    </tr>
				    <tr>
					    <td colspan="3">
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink no-imprimir">Regresar</asp:HyperLink>
                        </td>
				    </tr>
				</table>
            </div>
						
</div>
   
 </div>
</asp:Content>
