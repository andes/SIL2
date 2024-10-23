<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CopiaTurnoList.aspx.cs" Inherits="WebLab.Turnos.CopiaTurnoList" MasterPageFile="~/Site1.Master" %>
 
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 
   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
     <br />   &nbsp;
   
        <div align="left" style="width:1000px; ">
      <table style="width:950px;" align="center" class="myTabla">
        <tr>
            <td width="20%" style="vertical-align: top">
                <asp:Panel ID="pnlDerecho" runat="server" BackColor="#F0F0F0">
               
                <table style="border: 1px double #3A93D2; background-color: #F3F3F3;">
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            TIPO DE SERVICIO</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="myList" 
                                Width="150px" 
                                onselectedindexchanged="ddlTipoServicio_SelectedIndexChanged" 
                                AutoPostBack="True">
                            </asp:DropDownList>
                          
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                           <hr /></td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                              <table style="width: 100%; font-family: Arial, Helvetica, sans-serif; font-size: 10px;">
                    <tr>
                        <td>
                            Buscar paciente:</td>
                        <td>
                            <asp:TextBox ID="txtPaciente" runat="server" Width="95px" CssClass="myTexto" 
                                ValidationGroup="1" MaxLength="8" TabIndex="1"></asp:TextBox>
                        </td>
                        <td>
                                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="1" 
                                               CssClass="myButton" TabIndex="4" onclick="btnBuscar_Click" 
                                Width="57px" />
                                          </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:RadioButtonList ID="rdbBusqueda" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">DNI</asp:ListItem>
                                <asp:ListItem>Apellido</asp:ListItem>
                                <asp:ListItem>Nombre</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="1">Ingresar solo numeros en DNI</asp:CustomValidator>
                        </td>
                    </tr>
                    </table></td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            <hr /></td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            <asp:Button ID="btnActualizar" runat="server" CssClass="myButton" 
                                onclick="btnActualizar_Click" Text="Actualizar" Width="100px" 
                                ValidationGroup="1" />
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                           <hr /></td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            Fecha del turno</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Calendar ID="cldTurno" runat="server" BackColor="#FFFFCC" 
                                BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" 
                                Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="200px" 
                                ondayrender="cldTurno_DayRender" onselectionchanged="cldTurno_SelectionChanged" 
                                onvisiblemonthchanged="cldTurno_VisibleMonthChanged" ShowGridLines="True" 
                                ToolTip="Seleccione la fecha para la cual desea dar un turno" Width="220px">
                                <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                <SelectorStyle BackColor="#FFCC66" />
                                <TodayDayStyle BackColor="#CCCCCC" />
                                <OtherMonthDayStyle ForeColor="#CC9966" />
                                <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                                <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" 
                                    ForeColor="#FFFFCC" />
                            </asp:Calendar>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            <hr />
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            <asp:Button ID="btnNuevo" runat="server" CssClass="myButton" 
                                onclick="btnNuevo_Click" Text="Nuevo Turno" Width="100px" />
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            <asp:LinkButton ID="lnkProtocolo" runat="server" CssClass="myLinkRojo" 
                                onclick="lnkProtocolo_Click" ToolTip="Cargar protocolo para paciente sin turno">Cargar 
                            Paciente sin Turno</asp:LinkButton>
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            &nbsp;</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                </table> </asp:Panel>
            </td>
            <td width="5%">
                &nbsp;</td>
            <td width="65%"   style="vertical-align: top">
            <table width="100%" >
            <tr>
            <td colspan="3" style="font-weight: bold" class="mytituloTabla">
                <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>              <br />
                <asp:Label CssClass="mytituloGris" ID="lblSubTitulo" runat="server" 
                    Text="Label" Visible="False"></asp:Label>                  
                </td>
            <td style="font-weight: bold" align="right" class="style1" >
                <asp:Label ID="lblFecha" runat="server" Font-Bold="True" Font-Size="9pt" 
                    ForeColor="#990000"></asp:Label>
                                </td>
            </tr>
            <tr>
            <td colspan="4">    <hr class="hrTitulo" /></td>
            </tr>
            <tr>
            <td colspan="3"  >
                <asp:Label ID="lblTipoServicio" runat="server" Text="Servicio: " 
                    Font-Bold="True"></asp:Label>
                </td>
            <td align="right" class="style3"  >
                <asp:Label ID="lblHorario" runat="server" Text="Horario de Atención: 07:30 - 12:30" 
                    Font-Bold="True" ForeColor="#333333"></asp:Label>
                </td>
            </tr>
            <tr>
            <td class="style2">
               Límite de Turnos:  
                <asp:Label ID="lblLimiteTurnos" runat="server" 
                    Text="0" ForeColor="#CC3300" Font-Bold="True"></asp:Label>
                </td>
            <td colspan="2">
                Turnos Dados:<asp:Label ID="lblTurnosDados" runat="server" ForeColor="#CC3300" 
                    Font-Bold="True">0</asp:Label>
                </td>
            <td align="right" class="style2">
                Turnos Disponibles:
                <asp:Label ID="lblTurnosDisponibles" runat="server" ForeColor="#CC3300" 
                    Font-Bold="True">0</asp:Label>
                </td>
            </tr>
            <tr>
            <td colspan="4">
             <hr /></td>
            </tr>
            <tr>
            <td class="myLabelIzquierda" colspan="4">
                Estado Turno:
                <asp:DropDownList ID="ddlEstadoTurno" runat="server" 
                    onselectedindexchanged="ddlEstadoTurno_SelectedIndexChanged" 
                    AutoPostBack="True" CssClass="myList">
                    <asp:ListItem Selected="True">Turnos Activos</asp:ListItem>
                    <asp:ListItem Value="Con Protocolo">Con Protocolo</asp:ListItem>
                    <asp:ListItem>Sin Protocolo</asp:ListItem>
                    <asp:ListItem>Turnos Eliminados</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblHoraTurno" runat="server" 
                    Font-Bold="True" Visible="False"></asp:Label>
                <br />
                                            <asp:Label ID="lblMensaje" 
                                runat="server" ForeColor="#CC3300" 
                    Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
            <td colspan="4">
                <hr /></td>
            </tr>
            
            
            <tr>
            <td colspan="2" >
                                            <asp:Label ID="lblUltimoProtocolo" runat="server" 
                    CssClass="myButtonRojo" Font-Size="12pt"></asp:Label> 
                </td>
            <td colspan="2" align=right >
               <table >
                                                
                        <tr>  <td class="myLabelLitlle"  style="vertical-align: top" align="left">
                                            &nbsp; 
                                            </td>
                    
                        <td class="myLabelLitlle"  style="vertical-align: top"><img src="../App_Themes/default/images/rojo.gif" />&nbsp;Sin Protocolo</td>
                      
                          <td >&nbsp;</td>
                          <td class="myLabelLitlle"  style="vertical-align: top"><img src="../App_Themes/default/images/verde.gif" />&nbsp;Con Protocolo</td>
                        </tr>
                        
                        </table>  </td>
            </tr>
            
            <tr>
            <td colspan="4">
              <div align="left" style="border: 1px solid #C0C0C0; overflow:scroll; overflow-x:hidden; height:500px; background-color: #F8F8F8;">
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                    CellPadding="2" DataKeyNames="idTurno" ForeColor="#333333" 
                         EmptyDataText="No hay turnos asignados para la fecha seleccionada" 
                    Width="99%" onrowcommand="gvLista_RowCommand" 
                    onrowdatabound="gvLista_RowDataBound" BorderColor="#3A93D2" 
                    BorderStyle="Solid" BorderWidth="2px" GridLines="Horizontal" Font-Names="Arial" 
                    Font-Size="8pt" EnableModelValidation="True">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField />
                        <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                            <ItemStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="apellido" HeaderText="Apellidos" >
                            <ItemStyle Width="30%" />
                        </asp:BoundField>
                        <asp:BoundField DataField="nombre" HeaderText="Nombres" >
                            <ItemStyle Width="45%" />
                        </asp:BoundField>
                          <asp:BoundField DataField="Protocolo" HeaderText="Protocolo" >
                              <ItemStyle Font-Bold="True" />
                        </asp:BoundField>
                          <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                            ommandName="Editar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton Visible="false" ID="Imprimir" runat="server" ImageUrl="~/App_Themes/default/images/imprimir.jpg" 
                              CommandName="Imprimir" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                        
          
                           <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton  ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                        
                             <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Protocolo" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                              CommandName="Imprimir" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                        <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha Act." />
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Left" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
               </div>
            </td>
            </tr>
            <tr>
            <td colspan="2" >
                                            &nbsp;</td>
            <td colspan="2" align=right >
                &nbsp;</td>
            </tr>
            <tr>
            <td colspan="2" >
                                            &nbsp;</td>
            <td colspan="2" align=right >
                <asp:LinkButton 
                            ID="lnkPlanilla" runat="server" CssClass="myLittleLink" 
                    ValidationGroup="0" onclick="lnkPlanilla_Click">Descargar Planilla</asp:LinkButton>&nbsp;&nbsp;
<asp:LinkButton 
                            ID="lnkPlanillaDetallada" runat="server" CssClass="myLittleLink" 
                    ValidationGroup="0" onclick="lnkPlanillaDetallada_Click">Descargar Planilla Detallada</asp:LinkButton>  </td>
            </tr>
            </table>
                     </td>
        </tr>
        <tr>
            <td width="20%" style="vertical-align: top">
                &nbsp;</td>
            <td width="5%">
                &nbsp;</td>
            <td width="65%"   style="vertical-align: top">
                &nbsp;</td>
        </tr>
        </table>
        </div>
    <script language="javascript" type="text/javascript">
  
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de anular el turno?'))
    return true;
    else
    return false;
    }
    </script>
</asp:Content>

