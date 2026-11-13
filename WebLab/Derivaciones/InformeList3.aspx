<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeList3.aspx.cs" Inherits="WebLab.Derivaciones.InformeList3" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript" src="../script/jquery.min.js"></script> 
  <script type="text/javascript" src="../script/jquery-ui.min.js"></script> 
  <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet"  type="text/css" />
    <script type="text/javascript">

        <%--    function validarFormulario() {
            var todoOk = false;
            var validatorEstado = document.getElementById('<%= rvEstado.ClientID %>');
            var txtObservacion = document.getElementById('<%= txtObservacion.ClientID %>');//--> Cambio TextBox por DropDownList
            var motivoCancelacion = document.getElementById('<%= ddlMotivoCancelacion.ClientID%>');
            var estado = document.getElementById('<%= ddlEstado.ClientID %>');
            var label = document.getElementById('<%= lblErrorMotivo.ClientID %>');

            //Limpio los labels de error 
            label.className = 'hidden';
            validatorEstado.style.visibility = 'hidden';
            reseteaLabelErrorGrilla();

            //Verifico los estados para el armado del Lote
            switch (estado.value) {
                case "0": //Opcion  --Seleccion--
                    validatorEstado.style.visibility = 'visible';
                    todoOk = false;
                    break;

                case "2": //Opcion: No enviado
                    if (motivoCancelacion.value == "0") { // if (txtObservacion.value.trim() == "") --> Cambio TextBox por DropDownList
                        label.className = 'exposed';
                        todoOk = false; // Evitar el envío del formulario
                    }
                    else {
                        todoOk = validarGrilla();
                    }
                    break;

                case "4": //Opcion: Pendiente para enviar
                    todoOk = validarGrilla();
                    break;
            }
            return todoOk;

        }
        
        function validarGrilla() {
            var todoOk = hayCheckSeleccionado();

            if (!todoOk) {
                var label = document.getElementById('<%= lblErrorLista.ClientID %>');
                label.className = 'exposed';
            }
            return todoOk;
        }

        function reseteaLabelErrorGrilla() {
            var labelGrilla = document.getElementById('<%= lblErrorLista.ClientID %>');
            labelGrilla.className = 'hidden';
        }
        
        function reseteaLabelErrorMotivo() {
            var labelMotivo = document.getElementById('<%= lblErrorMotivo.ClientID %>');
            labelMotivo.className = 'hidden';
        }


        function checkDeterminaciones(checkbox, estado) {
            var itemCheck = checkbox.getElementsByTagName('input');
            if (itemCheck[0].checked) {
                reseteaLabelErrorGrilla();
                Page_ClientValidate('SeleccionCheck');
            }// agrego que si la determinacion ya esta en un Lote mande un alerta
            else {

                const params = new URLSearchParams(window.location.search);
                const tipo = params.get("Tipo");
                if (estado == '4' && tipo == 'Modifica') { //Estado 4 es Pendiente de envio
                    alert("Al desmarcar la determinacion se excluirá del lote.");
                }
            }
        }

        function validaObservacion(text) {
            if (text.value != "0") { /* if (text.value.trim()) {//--> Cambio TextBox por DropDownList */
                reseteaLabelErrorMotivo();
            }
        }

       function activaMotivos(estado) {
            var motivos = document.getElementById('<%= ddlMotivoCancelacion.ClientID %>');
            if (estado.value == 2) {
                //Si el estado es "No enviado" habilitar el ddl de motivos de cancelacion
                motivos.disabled = false;
            } else {
                motivos.disabled = true;
            }
        }


        function seleccionarTodos(seleccionar) {
            var grid = document.getElementById('<%= gvLista.ClientID %>');
            if (!grid) return;
            // Buscar todos los checkbox dentro del GridView
            var checkboxes = grid.querySelectorAll('input[type="checkbox"]');

            checkboxes.forEach(function (chk) {
                if (!chk.disabled) {   // ✅ ignora los que están inhabilitados
                    chk.checked = seleccionar;
                }
            });
            //Si selecciono todas las derivaciones, deshabilito el error que no selecciono ninguna fila
           if (seleccionar && document.getElementById('<%= lblErrorLista.ClientID %>').className == 'exposed') {
                reseteaLabelErrorGrilla();
            }
        }

        function validarSeleccionPDF(sender, args) {
            args.IsValid = hayCheckSeleccionado();
        }

        function hayCheckSeleccionado() {
            var gridView = document.getElementById('<%= gvLista.ClientID %>');
            var rows = gridView.getElementsByTagName('tr');
            for (var i = 1; i < rows.length; i++) {
                var row = rows[i];
                var chk = row.querySelector('[id$="CheckBox1"]');
                if (chk && chk.checked) return true;
            }
            return false;
        }--%>

        function PreguntoEliminar() {
            if (confirm('¿Está seguro de eliminar el registro?'))
                return true;
            else
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


        function NoEnviarDeterminaciones(listaIdDetalle, idLote) {

            $('<iframe src="DerivacionAnular.aspx?Lista=' + listaIdDetalle + '&idLote=' + idLote +'" />').dialog({
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

  <%-- <script type="text/javascript">
     document.addEventListener("DOMContentLoaded", function () {
         const hdnDatos = document.getElementById('<%= hdnDatosModificados.ClientID %>');
        const hdnGrid = document.getElementById('<%= gvLista.ClientID %>');
          const formElements = document.querySelectorAll("input, select, textarea");

          // Guardamos los valores originales al cargar la página
          formElements.forEach(function (el) {
              const tipo = el.type?.toLowerCase();
              if (tipo === "checkbox") {
                  el.dataset.originalValue = el.checked.toString();
              } else {
                  el.dataset.originalValue = el.value;
              }
          });

          //Cuando se hace submit se evalua si hubo cambios (en la modificación)
          const form = document.forms[0];
          form.addEventListener("submit", function () {
              let cambioGeneral = false;
              let cambioEnGrid = false;

              formElements.forEach(function (el) {
                  const tipo = el.type?.toLowerCase();
                  const original = el.dataset.originalValue;
                  const actual = (tipo === "checkbox") ? el.checked.toString() : el.value;

                  if (original !== actual) {
                      cambioGeneral = true;

                      // Detectar si el checkbox pertenece a un GridView (ajustá si tu GridView tiene otro ID)
                      if (tipo === "checkbox" && el.closest("table")?.id?.includes("gv")) {
                          cambioEnGrid = true;
                      }
                  }
              });

              hdnDatos.value = cambioGeneral ? "true" : "false";
              hdnGrid.value = cambioEnGrid ? "true" : "false";
          });
      });
  </script> --%>

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
        <div class="panel panel-default"  id="contenidoImprimir">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-6">
                   <%-- <b>DERIVACIONES</b> --%>
                          <b> <asp:Label ID="lblTitulo" Text="" runat="server"></asp:Label></b>
                        <br />
                        <b> <asp:Label ID="lblEstadoLote" Text="" runat="server"></asp:Label></b>
                        <br />
                        <asp:Label ID="lblSubTitulo" Text="" runat="server" Visible="false"></asp:Label>
                    </div>
                     <div class="col-md-6" align="right"  rowspan="4">
                    <asp:Panel runat="server" ID="pnlNroLote" >
                    <h3>
                        <span class="label label-default"><asp:Label ID="lblNroLote" runat="server" ></asp:Label> </span>
                    </h3>
                </asp:Panel>
                </div>
                </div>
                
               
            </div>

			<div class="panel-body">
				<table  width="1000px"  >


                    <tr>

                    <%--<td class="myLabelLitlle" style="vertical-align: top" colspan="3">
                                            Referencias:
                                                <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente de derivar&nbsp;
                                                <img alt="" src="../App_Themes/default/images/reloj-de-arena.png" /> Pendiente para enviar&nbsp;
                                                <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;
                                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;<br />
                                                &nbsp;<br />
                        </td>--%>
                           <td style="vertical-align: top" colspan="3" class="no-imprimir">
                            <asp:Panel id="Panel2"   runat="server" >
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
			       
                  <%--  <tr>
				        <td style="vertical-align: top" colspan="3">
                            <asp:Panel id="Panel1"   runat="server">
                                <table class="myTabla">
                                    <tr style="vertical-align: sub">
                                        <td>Marcar como:</td>
                                        <td >
                                             <asp:DropDownList ID="ddlEstado" runat="server"  class="form-control input-sm" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged" AutoPostBack="true"> </asp:DropDownList> 
                                             <asp:RangeValidator id="rvEstado"
                                               ControlToValidate="ddlEstado"
                                               MinimumValue="1"
                                               MaximumValue="4"
                                               Type="Integer"
                                               Text="* Seleccione un estado"
                                               runat="server" SetFocusOnError="True" 
                                               ValidationGroup="0" />
                                        </td>
                                        <td>
                                            Motivo Cancelaci&oacute;n: </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMotivoCancelacion" runat="server" class="form-control input-sm" />
                                            <anthem:RangeValidator 
                                                ID="rvMotivoCancelacion" runat="server" ControlToValidate="ddlMotivoCancelacion" 
                                                ErrorMessage="* Seleccione un motivo" MaximumValue="99999" MinimumValue="1" Type="Integer" 
                                                ValidationGroup="0"></anthem:RangeValidator>
                                        </td>
                                       <%-- <td>
                                            <asp:Label  Text="* Seleccione un motivo" runat="server" ID="lblErrorMotivo" CssClass="hidden"></asp:Label>
                                        </td>--%>
                                        <%--  <td>
                                            Observaci&oacute;n:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtObservacion" runat="server" CssClass="form-control input-sm" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGuardar" runat="server" CausesValidation="true" CssClass="btn btn-primary"  Width="100" Text="Guardar" 
                                             onclick="btnGuardar_Click"  ValidationGroup="0" /></td>
                                    </tr>
                                  
                                </table>
                             
                            </asp:Panel>

                        </td>
						
			        </tr>--%>

				    <tr>
					    <td class="myLabelIzquierdaGde no-imprimir" colspan="3">
                                            <hr /></td>
						
				    </tr>
                    <tr>
                       <%-- <td>
                        <!-- PDF de Control de determinaciones -->
                            <asp:LinkButton ID="lnkPDF" runat="server" CssClass="myLittleLink" OnClientClick="return validarGrilla();"  onclick="lnkPDF_Click"> <asp:Image  runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg"  />Generar PDF de Control</asp:LinkButton>       
                        </td>--%>
                        <td>
                        </td>  

                        <td align="right">
                            <asp:HiddenField ID="HFIdLote" runat="server" />
                            <asp:HiddenField ID="HFIdEfectorDerivacion" runat="server" />
                              <asp:HiddenField ID="HFListaDetalles" runat="server" />
<%--                            <asp:Button ID="btnAgregarDeterminaciones" runat="server" Text="Agregar Determinaciones" Visible="false" CssClass="btn btn-primary" Width="200" OnClientClick="AgregarDeterminaciones(); return false; " OnClick="btnAgregarDeterminaciones_Click"/>--%>
                            <input id="hidToken" type="hidden" runat="server" />
                        </td>
                    </tr>
                    <tr><td class="no-imprimir"> <br /></td></tr>
				    <tr>
					    <td colspan="2" class="no-imprimir">
                            <%--<asp:Label Text="* Seleccione una fila" runat="server" ID="lblErrorLista" CssClass="hidden"></asp:Label--%>
                            <asp:CustomValidator id="cvGeneral" runat="server" OnServerValidate="cvGeneral_ServerValidate" ValidationGroup="0" ></asp:CustomValidator>
                            <asp:CustomValidator ID="cvNoEnviado" runat="server" OnServerValidate="cvNoEnviado_ServerValidate" ValidationGroup="1"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvExcluir" runat="server" OnServerValidate="cvExcluir_ServerValidate" ValidationGroup="2"></asp:CustomValidator>
                              
                            <div class="mylabelizquierda" runat="server" id="linkSeleccionar">Seleccionar:                                           
                            <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="myLittleLink"  OnClick= "lnkMarcar_Click">Todas</asp:LinkButton>&nbsp;
                            <asp:LinkButton runat="server" CssClass="myLittleLink"  ID="lnkDesMarcar" OnClick="lnkDesMarcar_Click" >Ninguna</asp:LinkButton>
                                &nbsp;&nbsp;
                                            
                                </div>
                        </td>
						
					    <td align="right" class="no-imprimir">
                            <asp:Label ID="CantidadRegistros" runat="server"  forecolor="Blue" />
                        </td>
				    </tr>

				    <tr>
					    <td colspan="3">
                            <div  class="contenedorLista">
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
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink no-imprimir"  >Regresar</asp:HyperLink>
                        </td>
				    </tr>
				</table>
                  <%--<asp:HiddenField ID="hdnDatosModificados" runat="server" Value="false" />--%>
            </div>
						
</div>
   
 </div>
</asp:Content>
