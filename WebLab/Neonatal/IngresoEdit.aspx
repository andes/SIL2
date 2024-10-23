<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IngresoEdit.aspx.cs" Inherits="WebLab.Neonatal.IngresoEdit" MasterPageFile="~/Site1.Master"  %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/default/style.css" />
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

    <script type="text/javascript"      src="../script/jquery.min.js"></script> 
    <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 

    <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    <script type="text/javascript"> 

    $(function() {
    $("#<%=txtFechaExtraccion.ClientID %>").datepicker({
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
        <table  width="900px" cellpadding="1" cellspacing="1" 
        >
					
					
					<tr>
						<td                                              
                           align="left" style="width: 6px"  >
                                &nbsp;</td>
						<td colspan="2"                                              
                           align="left">
                                &nbsp;</td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left" style="width: 6px"  >
                                &nbsp;</td>
						<td colspan="2"                                              
                           align="right">
                                <asp:ImageButton ID="imgPdf" runat="server" 
                                ImageUrl="~/App_Themes/default/images/pdf.jpg" onclick="imgPdf_Click" /> </td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left" style="width: 6px"  >
                                &nbsp;</td>
						                                          
                           <td class="mytituloTabla" colspan="2">
                               SOLICITUD DE PESQUISA NEONATAL
					    </td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left" style="width: 6px"  >
                                &nbsp;</td>
						<td colspan="2"                                              
                           align="left" style="font-weight: bold; font-size: 14px">
                                <hr /></td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 234px" >
                            Paciente:</td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:Label ID="lblPaciente" runat="server" Text="Label"></asp:Label>
                                  </td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 234px" >
                            Información de Contacto:</td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:Label ID="lblInfoContacto" runat="server" Text="Label"></asp:Label>
                                      </td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" colspan="2" >
                            <hr /></td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 234px" >
                            Hospital Solicitante:</td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:DropDownList ID="ddlEfector" runat="server">
                            
                              </asp:DropDownList>
                          </td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px" >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
						
                            Médico Solicitante:
                          </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                               <asp:DropDownList ID="ddlEspecialista" runat="server">
                            
                              </asp:DropDownList>
                          </td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                            <hr /></td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="mytituloGris" colspan="2">
                            Datos Adicionales Recien Nacido</td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px">
                            Apellido Materno:<asp:RequiredFieldValidator ID="rfvApellidoMaterno" runat="server" 
                                    ControlToValidate="txtApellidoMaterno" ErrorMessage="Apellido Materno" 
                                    ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                            <td  class="myLabelDerecha" style="width: 746px">
                                <asp:TextBox ID="txtApellidoMaterno" runat="server" Width="550px" TabIndex="2"></asp:TextBox>
                        </td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Apellido Paterno:</td>
                            <td  class="myLabelDerecha" style="width: 746px">
                                <asp:TextBox ID="txtApellidoPaterno" runat="server" Width="550px" TabIndex="3"></asp:TextBox>
                        </td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Fecha y
                            Hora de Nacimiento:<asp:RequiredFieldValidator 
                                      ID="rfvHoraNac" runat="server" ControlToValidate="txtHoraNacimiento" 
                                      ErrorMessage="Hora Nacimiento" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                            <td class="myLabelDerecha" style="width: 746px"  ><asp:Label 
                                      ID="lblFechaNacimiento" runat="server" Text="Label"></asp:Label>
                                  &nbsp;
                                  <input id="txtHoraNacimiento" runat="server" type="text" maxlength="5" 
                        style="width: 50px"   onblur="valHora(this)"              
                        onkeyup="mascara(this,':',patron,true)" tabindex="4" class="myTexto" 
                                title="Ingrese la hora de nacimiento"  />&nbsp;</td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Edad gestional:<asp:RequiredFieldValidator ID="rfvEdadGestacional" runat="server" 
                                    ControlToValidate="txtEdadGestacional" ErrorMessage="Edad Gestacional" 
                                    ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                            <td  class="myLabelDerecha" style="width: 746px">
                             <input id="txtEdadGestacional" runat="server" type="text" maxlength="2" 
                          style="width: 60px"  onblur="valNumero(this)" tabindex="5" class="myTexto" 
                                title="Ingrese la edad gestacional" />
                                (semanas)</td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Peso:<asp:RequiredFieldValidator 
                                    ID="rfvPeso" runat="server" ControlToValidate="txtPeso" ErrorMessage="Peso" 
                                    ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                            <td  class="myLabelDerecha" style="width: 746px">
                             <input id="txtPeso" runat="server" type="text" maxlength="6" 
                          style="width: 60px"  onblur="valNumero(this)" tabindex="6" class="myTexto" 
                                title="Ingrese en peso en gramos." /> (gramos)</td>
					</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Nacimiento:<asp:RequiredFieldValidator ID="rfvNacimiento" runat="server" 
                                    ControlToValidate="rdbNacimiento" ErrorMessage="Nacimiento" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                            <td  class="myLabelDerecha" style="width: 746px">
                                <asp:RadioButtonList ID="rdbNacimiento" runat="server" 
                                    RepeatDirection="Horizontal" TabIndex="7">
                                    <asp:ListItem Value="0">A término</asp:ListItem>
                                    <asp:ListItem Value="1">Pre-término</asp:ListItem>
                                </asp:RadioButtonList>
                        </td>
					</tr>
						
					
					
	
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                            <hr /></td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
					<td class="mytituloGris" colspan="2">
                            Datos de la Muestra</td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Muestra:<asp:RequiredFieldValidator ID="rfvMuestra" runat="server" 
                                  ControlToValidate="rdbMuestra" ErrorMessage="Muestra" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td class="myLabelDerecha" style="width: 746px" >
                              <asp:RadioButtonList ID="rdbMuestra" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="8">
                                  <asp:ListItem Value="1">Primera</asp:ListItem>
                                  <asp:ListItem Value="0">Repetición</asp:ListItem>
                              </asp:RadioButtonList></td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Fecha y hora de Extracción:<asp:RequiredFieldValidator 
                                ID="rfvFechaExtraccion" runat="server" 
                                  ControlToValidate="txtFechaExtraccion" 
                                ErrorMessage="Fecha de Extracción" ValidationGroup="0">*</asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvHoraExtraccion" runat="server" 
                                  ControlToValidate="txtHoraExtraccion" ErrorMessage="Hora de Extracción" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                                    </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <input id="txtFechaExtraccion" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="9" class="myTexto" 
                                style="width: 70px"  /> &nbsp;   <input id="txtHoraExtraccion" runat="server" 
                                  type="text" maxlength="5" 
                        style="width: 50px"   onblur="valHora(this)"              
                        onkeyup="mascara(this,':',patron,true)" tabindex="10" class="myTexto" 
                                title="Ingrese la hora de nacimiento"  />&nbsp;
                              <asp:CustomValidator ID="cvValidacionFechaExtraccion" runat="server" 
                                  ControlToValidate="txtFechaExtraccion" 
                                  onservervalidate="cvValidacionFechaExtraccion_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                              </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 234px"  >
                            Ingesta de leche en las primeras 24 horas:<asp:RequiredFieldValidator ID="rfvIngestaLeche" runat="server" 
                                  ControlToValidate="rdbIngestaLeche" ErrorMessage="Primera Ingesta Leche" 
                                  ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbIngestaLeche" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="11">
                                  <asp:ListItem Value="1">Si</asp:ListItem>
                                  <asp:ListItem Value="0">No</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 234px"  >
                            Alimentación:<asp:RequiredFieldValidator ID="rfvAlimentacion" runat="server" 
                                  ControlToValidate="rdbAlimentacion" ErrorMessage="Alimentacion" 
                                  ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbAlimentacion" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="12">
                                  <asp:ListItem Value="0">Pecho</asp:ListItem>
                                  <asp:ListItem Value="1">Biberón</asp:ListItem>
                                  <asp:ListItem Value="2">Parenteral</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Antibioticos:<asp:RequiredFieldValidator ID="rfvAntibiotico" runat="server" 
                                  ControlToValidate="rdbAntibiotico" ErrorMessage="Antibiotico" 
                                  ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbAntibiotico" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="13">
                                  <asp:ListItem Value="1">Si</asp:ListItem>
                                  <asp:ListItem Value="0">No</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            </td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Transfusión:<asp:RequiredFieldValidator ID="rfvTransfusion" runat="server" 
                                  ControlToValidate="rdbTransfusion" ErrorMessage="Transfusion" 
                                  ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbTransfusion" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="14">
                                  <asp:ListItem Value="1">Si</asp:ListItem>
                                  <asp:ListItem Value="0">No</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" style="width: 234px"  >
                            Corticoides:<asp:RequiredFieldValidator ID="rfvCorticoides" runat="server" 
                                  ControlToValidate="rdbCorticoide" ErrorMessage="Corticoides" 
                                  ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbCorticoide" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="15">
                                  <asp:ListItem Value="1">Si</asp:ListItem>
                                  <asp:ListItem Value="0">No</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Dopamina:<asp:RequiredFieldValidator ID="rfvDopamina" runat="server" 
                                  ControlToValidate="rdbDopamina" ErrorMessage="Dopamina" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbDopamina" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="16">
                                  <asp:ListItem Value="1">Si</asp:ListItem>
                                  <asp:ListItem Value="0">No</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                           <hr /></td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td class="mytituloGris" colspan="2">
                            Antecedentes Maternos</td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda" style="width: 234px" >
                            Enfermedad Tiroidea:<asp:RequiredFieldValidator ID="rfvEnfermedadTiroidea" runat="server" 
                                  ControlToValidate="rdbAntecedenteTiroidea" ErrorMessage="Enfermedad Tiroidea" 
                                  ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
                          <td  class="myLabelDerecha" style="width: 746px">
                              <asp:RadioButtonList ID="rdbAntecedenteTiroidea" runat="server" 
                                  RepeatDirection="Horizontal" TabIndex="17">
                                  <asp:ListItem Value="1">Si</asp:ListItem>
                                  <asp:ListItem Value="0">No</asp:ListItem>
                              </asp:RadioButtonList>
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px; height: 71px;"  >
                            </td>
						<td  class="myLabelIzquierda"  
                            style="vertical-align: top; width: 234px; height: 71px;">
                            Otros antecedentes:</td>
                          <td class="myLabelDerecha" style="width: 746px; height: 71px;">
                              <asp:TextBox ID="txtAntecedenteMadre" runat="server" TextMode="MultiLine" 
                                  Width="600px" Rows="4" TabIndex="18"></asp:TextBox>
                        </td>
						</tr>
					
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda"  style="vertical-align: top; " colspan="2">
                          <hr /></td>
						</tr>
					
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td  class="myLabelIzquierda"  style="vertical-align: top; width: 234px;">
                            Observaciones:</td>
                          <td class="myLabelDerecha" style="width: 746px">
                              <asp:TextBox ID="txtAntecedenteMadre0" runat="server" TextMode="MultiLine" 
                                  Width="600px" Rows="4" TabIndex="18"></asp:TextBox>
                        </td>
						</tr>
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                            <hr /></td>
						</tr>
						
						<tr>
						<td style="width: 6px; height: 13px;"  >
                            </td>
						<td class="mytituloGris" colspan="2" style="height: 13px">
                           ALARMAS</td>
						</tr>
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td align="left" colspan="2">
                              <asp:GridView ID="gvLista" runat="server" Width="100%"
                                EmptyDataText="No se generaron alarmas a tener en cuenta" 
                                  CssClass="myLabelRojo" ShowHeader="False">
                                  <EmptyDataRowStyle ForeColor="Black" />
                                  <HeaderStyle  BackColor="#CC3300" ForeColor="White" />
                              </asp:GridView>
                             
                        </td>
						</tr>
						
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                            <hr /></td>
						</tr>
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td align="left" colspan="2">
                            <asp:Button ID="btnGuardar" CssClass="myButton" runat="server" Text="Guardar" 
                                TabIndex="19" onclick="btnGuardar_Click" ValidationGroup="0" />
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                HeaderText="Debe completar los siguientes datos obligatorios:" 
                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />
                        </td>
						</tr>
						
					
					
					<tr>
						<td style="width: 6px"  >
                            &nbsp;</td>
						<td colspan="2">
                            &nbsp;</td>
						</tr>
						</table>
											
 
</asp:Content>

