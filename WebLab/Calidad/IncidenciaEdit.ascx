<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IncidenciaEdit.ascx.cs" Inherits="WebLab.Calidad.IncidenciaEdit" %>

		<table>
     
		
<tr>
		<td align="left">
                                                   
                                                                <asp:TreeView ID="TreeView2" runat="server" ImageSet="Arrows" ShowLines="True"    CssClass="table table-bordered bs-table" >
                                                                    <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                                    <Nodes>
                                                                          <asp:TreeNode ShowCheckBox="True" Text="FICHA NO CORRESPONDE A SOLICITUD" Value="45" SelectAction="None"></asp:TreeNode>
                                                                          <asp:TreeNode ShowCheckBox="True" Text="FALTAN DATOS EN LA FICHA" Value="44" SelectAction="None">

                                                                              <asp:TreeNode ShowCheckBox="True" Text="Falta FIS" Value="46" SelectAction="None"></asp:TreeNode>
                                                                                  <asp:TreeNode ShowCheckBox="True" Text="Falta FUC" Value="47" SelectAction="None"></asp:TreeNode>
                                                                              <asp:TreeNode ShowCheckBox="True" Text="Faltan Datos Filiatorios" Value="48" SelectAction="None"></asp:TreeNode>
                                                                              <asp:TreeNode ShowCheckBox="True" Text="Faltan Síntomas" Value="49" SelectAction="None"></asp:TreeNode>
                                                                              

                                                                          </asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="True"  Text="MUESTRA MAL IDENTIFICADA" Value="9" SelectAction="None"></asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="True" Text="MUESTRA INSUFICIENTE" Value="10" SelectAction="None">
                                                                        </asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="True" Text="MUESTRA NO RECIBIDA" Value="25" SelectAction="None">
                                                                        </asp:TreeNode>
                                                                        
                                                                       <asp:TreeNode Expanded="False" SelectAction="Expand"       Text="Muestra de sangre en condición inadecuada" Value="11">
                                                                            <asp:TreeNode ShowCheckBox="True" Text="coagulada" Value="12" SelectAction="None"></asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="hemolizada" Value="13" SelectAction="None"></asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="lipémica" Value="14" SelectAction="None"></asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="con interferentes" Value="15" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="ictérica" Value="16" SelectAction="None"></asp:TreeNode>
                                                                        </asp:TreeNode> 
                                                                        <asp:TreeNode Expanded="False" SelectAction="Expand" ShowCheckBox="False" 
                                                                            Text="Muestra de orina en condición inadecuada" Value="17">
                                                                            <asp:TreeNode ShowCheckBox="True" Text="contaminada" Value="18" SelectAction="None"></asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="mal recolectada" Value="19" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="mal conservada" Value="20" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                        </asp:TreeNode>
                                                                        <asp:TreeNode Expanded="False" SelectAction="Expand" ShowCheckBox="False" 
                                                                            Text="Muestra de materia fecal en condición inadecuada (Microbiologia)" Value="28">
                                                                            <asp:TreeNode ShowCheckBox="True" Text="escasa muestra" Value="29" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="mal conservada" Value="30" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="mal recolectada" Value="31" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="no corresponde (heces formes)" Value="32" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                        </asp:TreeNode>
                                                                         <asp:TreeNode Expanded="False" SelectAction="Expand" ShowCheckBox="False" 
                                                                            Text="Líquido de punción en condición inadecuada (Microbiologia)" Value="33">
                                                                            <asp:TreeNode ShowCheckBox="True" Text="coagulado" Value="34" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="hemolizado" Value="35" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="escasa muestra" Value="36" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                        </asp:TreeNode>
                                                                         <asp:TreeNode Expanded="False" SelectAction="Expand" ShowCheckBox="False" 
                                                                            Text="Muestras derivadas en condiciones inadecuadas" Value="21">
                                                                            <asp:TreeNode ShowCheckBox="True" Text="mal conservadas" Value="22" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="no cumple Bioseguridad" Value="23" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="tipo de muestra incorrecta" Value="24" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                        </asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="False" Text="Datos personales mal ingresados al SIL" Expanded="False"
                                                                            Value="26" SelectAction="Expand">
                                                                                <asp:TreeNode ShowCheckBox="True" Text="Datos filiatorios incorrectos" Value="37" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                             <asp:TreeNode ShowCheckBox="True" Text="Obra Social no informada o incorrecta" Value="38" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                        </asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="False"  Expanded="False"
                                                                            Text="Datos del Protocolo mal ingresados al SIL" Value="27" SelectAction="Expand">
                                                                              <asp:TreeNode ShowCheckBox="True" Text="Error en el ingreso de determinaciones" Value="39" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                            <asp:TreeNode ShowCheckBox="True" Text="Diagnosticos presuntivo incorrecto o no informado" Value="40" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                              <asp:TreeNode ShowCheckBox="True" Text="Efector Solicitante mal informado" Value="41" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                              <asp:TreeNode ShowCheckBox="True" Text="Origen o Servicio mal informado" Value="42" SelectAction="None">
                                                                            </asp:TreeNode>
                                                                             <asp:TreeNode ShowCheckBox="True" Text="Fechas mal ingresadas" Value="43" SelectAction="None">
                                                                            </asp:TreeNode>

                                                                        </asp:TreeNode>
                                                                    </Nodes>
                                                                    <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="25px" 
                                                                        HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                                    <ParentNodeStyle Font-Bold="False" BackColor="#003399" />
                                                                    <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
                                                                        HorizontalPadding="0px" VerticalPadding="0px" BackColor="#FF0066" BorderStyle="Dotted" />
                                                                </asp:TreeView>
                                                            </td>
</tr>

                                            			                                        
        </table>
