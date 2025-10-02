<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeList3.aspx.cs" Inherits="WebLab.Derivaciones.InformeList3" MasterPageFile="~/Site1.Master" %>
<%--<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>--%>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function validarFormulario() {
            var todoOk = false;
            var validatorEstado = document.getElementById('<%= rv_estado.ClientID %>');
            <%--var txtObservacion = document.getElementById('<%= txtObservacion.ClientID %>');--%> //--> Cambio TextBox por DropDownList
            var motivoCancelacion = document.getElementById('<%= ddl_motivoCancelacion.ClientID%>');
            var estado = document.getElementById('<%= ddlEstado.ClientID %>');
            var label = document.getElementById('<%= lbl_ErrorMotivo.ClientID %>');

            //Limpio los labels de error 
            label.className = 'hidden';
            validatorEstado.style.visibility = 'hidden';
            reseteaLabelErrorGrilla();

            //Verifico los estados para el armado del Lote
            //console.log("estado.value", estado.value);
            //console.log("motivoCancelacion.value", motivoCancelacion.value);
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
            //console.log("final validacion ", todoOk);
            return todoOk;

        }


        function validarGrilla() {
            var todoOk = false;
            var gridView = document.getElementById('<%= gvLista.ClientID %>');
            var rows = gridView.getElementsByTagName('tr');

            for (var i = 1; i < rows.length; i++) { // Empieza en 1 para omitir el encabezado
                var row = rows[i];
                var CheckBox1 = row.querySelector('[id$="CheckBox1"]');

                if (CheckBox1.checked) {
                    return true; // Detiene la validación con al menos un check
                }
            }
            var label = document.getElementById('<%= lbl_errorLista.ClientID %>');
            label.className = 'exposed';
            return todoOk;
        }

        function reseteaLabelErrorGrilla() {
            var labelGrilla = document.getElementById('<%= lbl_errorLista.ClientID %>');
            labelGrilla.className = 'hidden';
        }

        function reseteaLabelErrorMotivo() {
            var labelMotivo = document.getElementById('<%= lbl_ErrorMotivo.ClientID %>');
            labelMotivo.className = 'hidden';
        }


        function checkDeterminaciones(checkbox, estado) {
            var itemCheck = checkbox.getElementsByTagName('input');
            if (itemCheck[0].checked) {
                reseteaLabelErrorGrilla();
            }// agrego que si la determinacion ya esta en un Lote mande un alerta
            else {

                //console.log(estado);
                const params = new URLSearchParams(window.location.search);
                console.log(params);
                const tipo = params.get("Tipo");
                console.log(tipo);
                if (estado == '4' && tipo == 'Modifica') { //Estado 4 es Pendiente de envio
                    console.log(window.location.search);
                    alert("Cuidado! Al desmarcar la determinacion no se enviara en el lote.");
                }
            }
        }

        function validaObservacion(text) {
            //console.log("validaObservacion", text);
            //console.log("validaObservacion value", text.value);
            if (text.value != "0") { /* if (text.value.trim()) {//--> Cambio TextBox por DropDownList */
                reseteaLabelErrorMotivo();
            }
        }

        function activaMotivos(estado) {
            var motivos = document.getElementById('<%= ddl_motivoCancelacion.ClientID %>');
            console.log(motivos);
            console.log(estado.value);
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
            if (seleccionar && document.getElementById('<%= lbl_errorLista.ClientID %>').className == 'exposed') {
                reseteaLabelErrorGrilla();
            }
        }

    </script>

  <script type="text/javascript">
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
  </script>




</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
<div align="left" style="width:1050px">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-6">
                    <b>DERIVACIONES</b> 
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

                    <td class="myLabelLitlle" style="vertical-align: top" colspan="3">
                                            Referencias:
                                                <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente de derivar&nbsp;
                                                <img alt="" src="../App_Themes/default/images/reloj-de-arena.png" /> Pendiente para enviar&nbsp;
                                                <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;
                                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;<br />
                                                &nbsp;<br />
                        </td>
				    </tr>
			       
                    <tr>
				        <td style="vertical-align: top" colspan="3">
                            <asp:Panel id="Panel1"   runat="server">
                                <table class="myTabla">
                                    <tr style="vertical-align: sub">
                                        <td>Marcar como:</td>
                                        <td >
                                             <asp:DropDownList ID="ddlEstado" runat="server"  class="form-control input-sm" onchange="activaMotivos(this);"> </asp:DropDownList> 
                                             <asp:RangeValidator id="rv_estado"
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
                                            <asp:DropDownList ID="ddl_motivoCancelacion" runat="server" class="form-control input-sm" onchange="return validaObservacion(this);" />

                                        </td>
                                        <td>
                                            <asp:Label  Text="* Seleccione un motivo" runat="server" ID="lbl_ErrorMotivo" CssClass="hidden"></asp:Label>
                                        </td>
                                        <td>
                                            Observaci&oacute;n:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_observacion" runat="server" CssClass="form-control input-sm" ></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnGuardar" runat="server" CausesValidation="true" CssClass="btn btn-primary"  Width="100" Text="Guardar" 
                                             onclick="btnGuardar_Click" OnClientClick="return validarFormulario();" ValidationGroup="0" /></td>
                                    </tr>
                                  
                                </table>
                             
                            </asp:Panel>

                        </td>
						
			        </tr>

				    <tr>
					    <td class="myLabelIzquierdaGde" colspan="3">
                                            <hr /></td>
						
				    </tr>
                    <tr>
                        <td>
                        <!-- (Casos anteriores) -->
                         <asp:LinkButton ID="lnkPDF" runat="server" CssClass="myLittleLink"  onclick="lnkPDF_Click">Generar PDF Derivacion (Sin Lote)</asp:LinkButton>
                                
                        </td>
                    </tr>
				    <tr>
					    <td colspan="2">
                             <asp:Label Text="* Seleccione una fila" runat="server" ID="lbl_errorLista" CssClass="hidden"></asp:Label>
                            <div class="mylabelizquierda" >Seleccionar:                                           
                            <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="myLittleLink" ValidationGroup="0" OnClientClick="seleccionarTodos(true); return false;">Todas</asp:LinkButton>&nbsp;
                            <asp:LinkButton runat="server" CssClass="myLittleLink" ValidationGroup="0"  ID="lnkDesMarcar" OnClientClick="seleccionarTodos(false); return false;" >Ninguna</asp:LinkButton>
                                &nbsp;&nbsp;
                                            
                                </div>
                        </td>
						
					    <td align="right">
                            <asp:Label ID="CantidadRegistros" runat="server"  forecolor="Blue" />
                        </td>
				    </tr>

				    <tr>
					    <td colspan="3">
                            <div  style="width:100%;height:450pt;overflow:scroll;;overflow-x:hidden;border:1px solid #CCCCCC; background-color: #F3F3F3;"> 
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                                    DataKeyNames="idDetalleProtocolo"   Width="98%" CellPadding="0"  ForeColor="#666666" PageSize="1" 
                                    EmptyDataText ="No se encontraron protocolos para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                                    BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                                    <Columns>
            
                                    <asp:TemplateField HeaderText="Sel." >
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" 
                                                Enabled='<%# HabilitarCheck(Convert.ToInt32(Eval("estado")), Convert.ToInt32(Eval("idLote"))) %> ' 
                                                onchange='<%# string.Format("checkDeterminaciones(this, {0});", Eval("estado")) %>'
                                                Checked='<%# HacerCheck(Convert.ToInt32(Eval("estado")))%> '/>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" 
                                            HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                         <asp:TemplateField>
                                           <ItemStyle Width="5%" HorizontalAlign="Center" />
                                           <ItemTemplate>
                                                <asp:Image ID="estado" runat="server" ImageUrl='<%# CargarImagenEstado(Convert.ToInt32(Eval("estado")))%> '/>
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
                                    <asp:BoundField DataField="paciente" HeaderText="Paciente">
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

                                    <asp:TemplateField HeaderText="Lote">
                                          <ItemTemplate> <asp:Label ID="lbl_nroLote" runat="server" Text='<%# Eval("idLote") %>'/> </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate> <asp:Label ID="lbl_estado" runat="server" Text='<%# Eval("estado") %>'/> </ItemTemplate>
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
                            </div>
                        </td>
						
				    </tr>
				    <tr>
					    <td colspan="3"><hr /></td>
				    </tr>
				    <tr>
					    <td colspan="3">
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink"  >Regresar</asp:HyperLink>
                        </td>
				    </tr>
				</table>
                  <asp:HiddenField ID="hdnDatosModificados" runat="server" Value="false" />
            </div>
						
</div>
   
 </div>
</asp:Content>
