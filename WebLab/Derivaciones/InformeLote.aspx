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

           //Limpio los labels de error 
           label.className = 'hidden';
           validatorEstado.style.visibility = 'hidden'; 
           labelGrilla.className = 'hidden';

           //Verifico los estados para el envio del Lote
           switch (estado.value) {
               case "0": //Opcion  --Seleccion--
                   validatorEstado.style.visibility = 'visible';  
                   todoOk = false;

                   break;
               case "1": //Opcion: Enviado
                  todoOk = validarGrilla();
                   break;

               case "2": //Opcion: No enviado
                   if (txtObservacion.value.trim() == "") {
                       label.className = 'exposed';
                       todoOk = false; // Evitar el env�o del formulario
                   }
                   else { 
                      todoOk = validarGrilla();
                   }
                   break;
           }

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
                   return true; // Detiene la validaci�n con al menos un check
               }
           }
           var label = document.getElementById('<%= lbl_errorLista.ClientID %>');
           label.className = 'exposed';
           return todoOk; 
       }

       function habilitaTransporte() {
           var ddl = document.getElementById('<%= ddlEstados.ClientID %>');
            var estado = ddl.value;

           if (estado == 1 || estado == 0) {
                var radioButtons = document.getElementById('<%= rb_transportista.ClientID %>').getElementsByTagName('input');
                for (var i = 0; i < radioButtons.length; i++) {
                    radioButtons[i].disabled = false; // Habilitar
                }
            } else { //estado == 2
                var radioButtons = document.getElementById('<%= rb_transportista.ClientID %>').getElementsByTagName('input');
                for (var i = 0; i < radioButtons.length; i++) {
                    radioButtons[i].disabled = true; // Deshabilitar
                }
            }
        }

       function reseteaLabelErrorLote() {
           var labelGrilla = document.getElementById('<%= lbl_errorLista.ClientID %>');
           labelGrilla.className = 'hidden';
       }

       function checkLote(checkbox) {
           var itemCheck = checkbox.getElementsByTagName('input');
          // console.log(checkbox);
           if (itemCheck[0].checked) {
             reseteaLabelErrorLote();
           } 
       }
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
                            Referencias:
                                <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;
                                <img alt="" src="../App_Themes/default/images/reloj-de-arena.png" /> Pendiente para enviar&nbsp;
                                <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;<br />
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
                                               MinimumValue="1"
                                               MaximumValue="2"
                                               Type="Integer"
                                               Text="* Seleccione un estado"
                                               runat="server" SetFocusOnError="True" ValidationGroup="0" />
                                     </td>
                                    
                                     <td >Retira:</td>
                                     <td > <asp:RadioButtonList runat="server" ID="rb_transportista" CssClass="radio-inline"> 
                                            <asp:ListItem Text="Transportista Tramitar" Selected="True" />   
                                            <asp:ListItem Text="Otro" />
                                         </asp:RadioButtonList></td>
                                     <td></td>
                                     <td>Observaciones:</td>
                                     <td><asp:TextBox ID="txtObservacion" runat="server" MaxLength="100" class="form-control input-sm"  ></asp:TextBox>
                                     </td>
                                     <td>
                                        
                                        <asp:Label  Text="* Seleccione un motivo" runat="server" ID="lbl_ErrorMotivo" CssClass="hidden"></asp:Label>
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
                                           <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true"  Enabled='<%# cargarSegunEstado(Convert.ToInt32(Eval("estado"))) %>' 
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

                                    <asp:TemplateField ItemStyle-Width="5%" >
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkPDF" OnCommand="lnkPDF_Command" CommandArgument='<%# Eval("numero") %>' Visible='<%# tieneLoteGenerado((Int32)Eval("estado")) %>'>
                                                 <asp:Image  runat="server" ImageUrl="~/App_Themes/default/images/pdf.jpg"  />
                                            </asp:LinkButton>
                                            
                                        </ItemTemplate>
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