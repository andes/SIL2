<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeLote.aspx.cs" Inherits="WebLab.Derivaciones.InformeLote" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">  
   <script type="text/javascript">
       function validarFormulario(s, args) {
           var todoOk = false;
           var validatorEstado = document.getElementById('<%= Range1.ClientID %>');
           var txtObservacion = document.getElementById('<%= txtObservacion.ClientID %>');
           var estado = document.getElementById('<%= ddlEstados.ClientID %>');
           var label = document.getElementById('<%= lbl_ErrorMotivo.ClientID %>');
           var labelGrilla = document.getElementById('<%= lbl_errorLista.ClientID %>');
           var lista_transporte = document.getElementById('<%= ddl_Transporte.ClientID %>');
           var labelErrorTransporte = document.getElementById('<%= lbl_ErrorTransporte.ClientID %>');

           //Limpio los labels de error 
           label.className = 'hidden';
           validatorEstado.style.visibility = 'hidden'; 
           labelGrilla.className = 'hidden';
           labelErrorTransporte.className = 'hidden';

           //Verifico los estados para el envio del Lote
           switch (estado.value) {
               //case "0": //Opcion  --Seleccion--
               //    validatorEstado.style.visibility = 'visible';  
               //    todoOk = false;

                   //break;
               case "2": //Opcion: Derivado
                   if (lista_transporte.selectedIndex != 0) //Si no es la opcion "Seleccionar"
                       todoOk = validarGrilla();
                   else { //Para valor 0 es el "Seleccionar"
                       labelErrorTransporte.className = 'exposed'; //activa el cartel de error de transporte
                       todoOk = false; // Evitar el envío del formulario
                   }
                   break;

               case "3": //Opcion: Cancelado
                   if (txtObservacion.value.trim() == "") {
                       label.className = 'exposed'; //activa el cartel de error del Motivo en la observacion
                       todoOk = false; // Evitar el envío del formulario
                   }
                   else { 
                      todoOk = validarGrilla();
                   }
                   break;
           }

           return todoOk;
       }

       function validarGrilla() {
            //Revisa que haya al menos un lote seleccionado para Derivar o Cancelar
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

       function habilitaTransporte() {
        //Habilita la seleccion del listado desplegable donde estan los transportes

           var ddl = document.getElementById('<%= ddlEstados.ClientID %>');
           var labelErrorTransporte = document.getElementById('<%= lbl_ErrorTransporte.ClientID%>');
           var lista_transporte = document.getElementById('<%= ddl_Transporte.ClientID%>');
           var estado = ddl.value;
           //console.log("--");
           //console.log(lista_transporte);
           labelErrorTransporte.className = "hidden";

           if (estado == 2) { //Derivado  //Si va a generar el lote se tiene que habilitar la lista para elegir el transporte
               lista_transporte.disabled = false;
           } else { //estado Cancelado
               lista_transporte.selectedIndex = 0; //Se deja "Seleccionar" para que no haya confusiones 
               lista_transporte.disabled = true;
            }
        }

       function reseteaLabelErrorLote() {
        //Esconde el label que tiene el error
           var labelGrilla = document.getElementById('<%= lbl_errorLista.ClientID %>');
           labelGrilla.className = 'hidden';
       }

       function checkLote(checkbox) {
        //Si se seleccione un lote se borra el cartel de error
           
        var itemCheck = checkbox.getElementsByTagName('input');
          // console.log(checkbox);
           if (itemCheck[0].checked) {
             reseteaLabelErrorLote();
           } 
       }

       function verificaSeleccion() {
            //Esta funcion revisa que se haya selecionado un transporte
           var lista_transporte = document.getElementById('<%= ddl_Transporte.ClientID %>');
           var labelErrorTransporte = document.getElementById('<%= lbl_ErrorTransporte.ClientID %>');
           //console.log("transporte");
           //console.log(lista_transporte.value);
           if (lista_transporte.selectedIndex == 0) {
               labelErrorTransporte.className = "exposed";
           } else {
               labelErrorTransporte.className = "hidden";
           }
       }

       document.addEventListener("DOMContentLoaded", function () {
           var txtFecha = document.getElementById('<%= txt_Fecha.ClientID %>');
           var txtHora = document.getElementById('<%= txt_Hora.ClientID %>');

           function cambioFechaHorario() {
               var fechaSeleccionada = new Date(txtFecha.value);
               var fechaHoy = new Date();
              

               // Normalizamos para comparar solo YYYY-MM-DD sin la hora
               var fechaSeleccionadaStr = fechaSeleccionada.toISOString().split('T')[0];  //toISOString() convierte la fecha en YYYY-MM-DDTHH:mm:ss.sssZ
               var fechaHoyStr = fechaHoy.toISOString().split('T')[0];

               txtFecha.max = fechaHoyStr; // Limitar la fecha máxima al día de hoy

               if (fechaSeleccionadaStr === fechaHoyStr) {
                   var horaActual = fechaHoy.getHours().toString().padStart(2, '0') + ":" +
                       fechaHoy.getMinutes().toString().padStart(2, '0');
                   if (txtHora.max !== horaActual) { // Evita sobrescribir si ya está correcto
                       txtHora.max = horaActual; //Si elige la fecha de hoy no puede poner una hora superior a la actual
                   }
               } else {
                   if (txtHora.hasAttribute("max")) { // Solo eliminar si existe
                       txtHora.removeAttribute("max");
                   }
               }
           }

           cambioFechaHorario(); //Llamar a la función al cargar la página
           txtFecha.addEventListener("change", cambioFechaHorario); //llamar la funcion el usuario cambia la fecha
       });
   </script>

   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    <div align="left" style="width:1200px">
        <div class="panel panel-default">
            <div class="panel-heading">
                  <b >LOTES</b> 
            </div>

			<div class="panel-body">
				 <table  width="1000px"  >
					<tr>
					<td class="myLabelLitlle" style="vertical-align: top" colspan="3">
                            Referencias de lote:
                                <img alt="" src="../App_Themes/default/images/reloj-de-arena.png" /> Creado&nbsp;
                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Derivado&nbsp;
                                <img alt="" src="../App_Themes/default/images/block.png" /> Descartado&nbsp;<br />
                                &nbsp;<br />
                        </td>
						
					</tr>
				<tr>
					<td style="vertical-align: auto" colspan="3">
                         <asp:Panel id="Panel1"   runat="server">
                             <table style="border-spacing:1em 0; border-collapse: separate">
                                 <tr style="vertical-align: sub">
                                     <td >Marcar como: </td>
                                     <td ><asp:DropDownList ID="ddlEstados" runat="server"  class="form-control input-sm" enabled="false"  onchange="habilitaTransporte()" /> 
                                         <asp:RangeValidator id="Range1"
                                               ControlToValidate="ddlEstados"
                                               MinimumValue="2"
                                               MaximumValue="3"
                                               Type="Integer"
                                               Text="* Seleccione un estado"
                                               runat="server" SetFocusOnError="True" ValidationGroup="0" />
                                     </td>
                                    
                                     <td >Retira transporte:</td>
                                     <td > <asp:DropDownList ID="ddl_Transporte" runat="server" class="form-control input-sm" onchange="verificaSeleccion()"></asp:DropDownList>    
                                         <%-- <asp:RadioButtonList runat="server" ID="rb_transportista" CssClass="radio-inline" Visible="false"> 
                                            <asp:ListItem Text="Transportista Tramitar" Selected="True" />   
                                            <asp:ListItem Text="Otro" />
                                         </asp:RadioButtonList>--%>

                                     </td>
                                     <td> <asp:Label  Text="* Seleccione un transporte" runat="server" ID="lbl_ErrorTransporte" CssClass="hidden"></asp:Label></td>
                                    
                                 </tr>
                                 <tr style="vertical-align: sub">
                                      <td>Observaciones:</td>
                                      <td><asp:TextBox ID="txtObservacion" runat="server" MaxLength="100" class="form-control input-sm"  ></asp:TextBox>
                                            <asp:Label  Text="* Seleccione un motivo" runat="server" ID="lbl_ErrorMotivo" CssClass="hidden"></asp:Label>
                                     </td>
                                     <%-- <td>
                                        
                                       <asp:Label  Text="* Seleccione un motivo" runat="server" ID="lbl_ErrorMotivo" CssClass="hidden"></asp:Label>
                                     </td>--%>
                                     <td>Fecha y Hora de retiro:</td>
                                    
                                      <td >
                                            <asp:TextBox id="txt_Fecha" runat="server" class="form-control input-sm"   TextMode="Date" ></asp:TextBox>
                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Fecha" ErrorMessage="Fecha" ValidationGroup="0">*Error en Fecha</asp:RequiredFieldValidator>
                                    </td>

                                     <td>
                                         <asp:TextBox id="txt_Hora"  runat="server"  class="form-control input-sm"  TextMode="Time"> </asp:TextBox>
                                         
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Hora" ErrorMessage="Hora" ValidationGroup="0">*Error en Hora</asp:RequiredFieldValidator>
                                        
                                     </td>





                                     <td align="left">
                                         <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary"  Width="100" Text="Guardar" enabled="false"
                                             OnClientClick="return validarFormulario();"
                                             onclick="btnGuardar_Click" ValidationGroup="0" />

                                     </td>
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

						<td colspan="2">
                            <asp:Label Text="* Seleccione un lote" runat="server" ID="lbl_errorLista" CssClass="hidden"></asp:Label>
                            <br />
                            <div class="mylabelizquierda" >Seleccionar:                                           
                                <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="myLittleLink" ValidationGroup="0" onclick="lnkMarcar_Click" OnClientClick="reseteaLabelErrorLote()">Todas</asp:LinkButton>&nbsp;
                                <asp:LinkButton  ID="lnkDesMarcar" runat="server" CssClass="myLittleLink" ValidationGroup="0"  onclick="lnkDesMarcar_Click">Ninguna</asp:LinkButton>
                                &nbsp;&nbsp;
                                            
                             </div>
                        </td>
						
						<td align="right">
                            <asp:Label ID="CantidadRegistros" runat="server"  forecolor="Blue" />
                        </td>
					</tr>

					<tr>
						<td colspan="3">
                          
                             
                        	<div style="width:100%;height:450pt;overflow:scroll; overflow-x:hidden;border:1px solid #CCCCCC; background-color: #F3F3F3;"> 
                               
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                                   DataKeyNames="numero"   Width="98%" CellPadding="0"  ForeColor="#666666" PageSize="1" 
                                   EmptyDataText ="No se encontraron lotes para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                                   BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" AlternatingRowStyle-HorizontalAlign="Center" AutoGenerateSelectButton="False" >
                                  <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                                   <Columns >
            
                                   <asp:TemplateField HeaderText="Sel."  >
                                        <ItemTemplate>
                                           <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true"  Enabled='<%# habilitarCheckBoxSegunEstado(Convert.ToInt32(Eval("estado"))) %>' 
                                               onchange="checkLote(this);"
                                               />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                  
                                       <asp:TemplateField>
                                           <ItemStyle Width="5%" HorizontalAlign="Center" />
                                           <ItemTemplate>
                                                <asp:Image ID="estado" runat="server" ImageUrl='<%# ObtenerImagenEstado(Convert.ToInt32(Eval("estado")))%> '/>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="numero" HeaderText="Nro. Lote" >
                                        <ItemStyle Width="15%" HorizontalAlign="Center" />
                                    </asp:BoundField>

                                     <asp:BoundField DataField="efectorderivacion" HeaderText="Efector Destino">
                                        <ItemStyle Width="20%" HorizontalAlign="Center"/>
                                    </asp:BoundField>

                                      <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha Registro" >
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="usernameE" HeaderText="Usuario Registro" >
                                        <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                   
                                     <asp:BoundField DataField="fechaEnvio" HeaderText="Fecha Envio" >
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="usernameR" HeaderText="Usuario Envio" >
                                        <ItemStyle Width="15%" HorizontalAlign="Center"/>
                                    </asp:BoundField>

                                    <asp:BoundField DataField="observacion" HeaderText="Observacion" >
                                        <ItemStyle Width="25%" HorizontalAlign="Center" />
                                    </asp:BoundField>

                                    <asp:TemplateField ItemStyle-Width="20%" HeaderText="Descargar" >
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkPDF" OnCommand="lnkPDF_Command" CommandArgument='<%# Eval("numero") %>'>
                                                 <asp:Image  runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg"  />
                                            </asp:LinkButton>
                                          
                                           
                                            
                                        </ItemTemplate>
                                        <ItemStyle>
                                             
                                        </ItemStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="20%" HeaderText="Modificar" >
                                        <ItemTemplate>
                                           
                                           <asp:LinkButton runat="server" ID="lnkEdit" OnCommand="lnkEdit_Command" CommandArgument='<%# Eval("numero") %>'  CommandName = '<%# Eval("idEfectorDerivacion") %>'>
                                                 <asp:Image  runat="server" ImageUrl='<%# habilitarEditarSegunEstado(Convert.ToInt32(Eval("estado"))) %>'  />
                                            </asp:LinkButton>
                                            
                                        </ItemTemplate>
                                        <ItemStyle>
                                             
                                        </ItemStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="idEfectorDerivacion" Visible="false"/>

                                    
                                </Columns>

                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#3A93D2" Font-Bold="False" ForeColor="White"  Font-Names="Arial" Font-Size="8pt" HorizontalAlign="Center"   />
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
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink" NavigateUrl="~/Derivaciones/GestionarLote.aspx">Regresar</asp:HyperLink>
                        </td>
					</tr>
					</table>
            </div>
        </div>
 </div>
 </asp:Content>
