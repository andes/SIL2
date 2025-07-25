<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TurnoList.aspx.cs" Inherits="WebLab.Turnos.TurnoList" MasterPageFile="~/Site1.Master" %>

 
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 
   
    <style type="text/css">
        .auto-style2 {
            width: 29%;
        }
        .auto-style3 {
            width: 38%;
        }
    </style>
  
 
   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
   
      <div align="left" style="width: 1300px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"> <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>    </h3>
                            <asp:Label CssClass="mytituloGris" ID="lblSubTitulo" runat="server" 
                    Text="Label" Visible="False"></asp:Label>  
                        </div>

				<div class="panel-body">	
     <table style="width:100%" >
        <tr>
            <td  style="vertical-align: top">
             
               
                      <div align="left" class="form-inline" ID="pnlDerecho" runat="server"   >
   <div class="panel panel-primary">
                    <div class="panel-heading">
  Parametros     
                        </div>

				<div class="panel-body">	
                <table style="width:100%" >
                    
                    <tr>
                        <td align="left">
                            1.- Seleccione Servicio /Practica</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddlTipoServicio" runat="server" AutoPostBack="True" 
                                 class="form-control input-sm" onselectedindexchanged="ddlTipoServicio_SelectedIndexChanged"    Width="180px">
                            </asp:DropDownList>
                            <asp:ImageButton ID="imgServicioView" runat="server" Visible="false"  ImageUrl="~/App_Themes/default/images/zoom.png" 
                                ToolTip="Próximos días habilitados" />
                            <br />
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:DropDownList ID="ddlItem" runat="server" AutoPostBack="True" class="form-control input-sm" onselectedindexchanged="ddlTipoServicio_SelectedIndexChanged" Width="200px">
                            </asp:DropDownList>
                            <asp:ImageButton ID="imgCalendarioView" runat="server" ImageUrl="~/App_Themes/default/images/zoom.png" ToolTip="Próximos días habilitados" Visible="false" />
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
              
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Button ID="btnActualizar" runat="server" CssClass="btn btn-primary" onclick="btnActualizar_Click" Text="Actualizar" ValidationGroup="1" Width="100px" />
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <hr />
                            <br />
                        </td>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                            2.- Seleccione Fecha del Turno</td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                 
                    <tr>
                        <td>
                            <asp:Calendar ID="cldTurno" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px" ondayrender="cldTurno_DayRender" onselectionchanged="cldTurno_SelectionChanged" onvisiblemonthchanged="cldTurno_VisibleMonthChanged" ToolTip="Seleccione la fecha para la cual desea dar un turno" Width="350px" NextPrevFormat="FullMonth">
                                <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                                <TodayDayStyle BackColor="#CCCCCC" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <NextPrevStyle Font-Size="8pt" ForeColor="#333333" Font-Bold="True" VerticalAlign="Bottom" />
                                <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                                <TitleStyle BackColor="White" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" BorderColor="Black" BorderWidth="4px" />
                            </asp:Calendar>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-danger" onclick="btnNuevo_Click" Text="Nuevo Turno" Width="120px" />
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:LinkButton ID="lnkProtocolo" runat="server" CssClass="myLinkRojo" onclick="lnkProtocolo_Click" ToolTip="Cargar protocolo para paciente sin turno">Cargar 
                            Paciente sin Turno</asp:LinkButton>
                        </td>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;</td>
                        <td align="center">&nbsp;</td>
                    </tr>
                </table> 
                    </div>
       </div>
                          </div>
                
            </td>
            <td width="5%">
                &nbsp;</td>
            <td style="vertical-align: top">
              
                     <asp:Label ID="lblMensaje" 
                                runat="server" ForeColor="#CC3300" 
                    Font-Bold="True"></asp:Label>              <asp:Label ID="lblHoraTurno" runat="server" 
                    Font-Bold="True" Visible="False"></asp:Label>
            <table width="100%" >
            <tr>
            <td colspan="3" >
              <h4><span class="label label-default">     <asp:Label ID="lblTipoServicio" runat="server" Text="Servicio: " 
                    Font-Bold="True"></asp:Label></span></h4>
                         
                </td>
            <td  align="right" class="auto-style3"  >
            <h4>          <span class="label label-default">      <asp:Label ID="lblFecha" runat="server"></asp:Label></span></h4>
                                </td>
            </tr>
            <tr>
            <td colspan="4"  >
                    
                <asp:Label ID="lblHorario" runat="server" Text="Horario de Atención: 07:30 - 12:30" 
                    Font-Bold="True" ForeColor="#333333"></asp:Label>
                    
                </td>
            </tr>
            <tr>
            <td class="auto-style2">
               Límite de Turnos:  
                <asp:Label ID="lblLimiteTurnos" runat="server" 
                    Text="0" ForeColor="#CC3300" Font-Bold="True"></asp:Label>
                </td>
            <td colspan="2">
                Turnos Dados:<asp:Label ID="lblTurnosDados" runat="server" ForeColor="#CC3300" 
                    Font-Bold="True">0</asp:Label>
                </td>
            <td align="right" class="auto-style3">
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
            <td class="myLabelIzquierda" valign="top" colspan="4" >
                <table>
                    <tr>
                           <td>      Efector Solicitante:</td>
               
          
               <td colspan="4"> <asp:DropDownList ID="ddlEfectorSolicitante" runat="server" 
                  
                    AutoPostBack="True" class="form-control input-sm" OnSelectedIndexChanged="ddlEfectorSolicitante_SelectedIndexChanged">
                  
                </asp:DropDownList>
                   </td>
                        </tr>
                    <tr>
                    <td>      Estado Turno:</td>
               
          
               <td> <asp:DropDownList ID="ddlEstadoTurno" runat="server" 
                    onselectedindexchanged="ddlEstadoTurno_SelectedIndexChanged" 
                    AutoPostBack="True" class="form-control input-sm">
                    <asp:ListItem Selected="True">Turnos Activos</asp:ListItem>
                    <asp:ListItem Value="Con Protocolo">Con Protocolo</asp:ListItem>
                    <asp:ListItem>Sin Protocolo</asp:ListItem>
                    <asp:ListItem>Turnos Eliminados</asp:ListItem>
                </asp:DropDownList>
                   </td>
                        <td>Buscar Paciente:</td>
                        <td><asp:TextBox ID="txtPaciente" runat="server" class="form-control input-sm" MaxLength="8" TabIndex="1" ValidationGroup="1" Width="100px"></asp:TextBox></td>
                        <td>    <asp:RadioButtonList ID="rdbBusqueda" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">DNI</asp:ListItem>
                                <asp:ListItem>Apellido</asp:ListItem>
                                <asp:ListItem>Nombre</asp:ListItem>
                            </asp:RadioButtonList></td>
                        <td>   <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-info" OnClick="btnBuscar_Click"    Width="100px" >
                                             <span class="glyphicon glyphicon-search"></span>&nbsp;Buscar</asp:LinkButton>      </td>
                        </tr>

             </table>
                
                                       
                 <asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="1">Ingresar solo numeros en DNI</asp:CustomValidator>               
                
                
                                       
             
                    
                           
                           
                        
             
                </td>
                       
            </tr>
            <tr>
            <td colspan="4">
                <hr /></td>
            </tr>
            
            
            <tr>
            <td colspan="2" ><asp:Label ID="lblUltimoProtocolo" runat="server"  CssClass="myButtonRojo" Font-Size="12pt"></asp:Label> 
                               

            </td>

                <td  colspan="2" align="right">
                       <table >
                                                
                        <tr>  <td class="myLabelLitlle"  style="vertical-align: top" align="left">
                                            &nbsp; 
                                            </td>
                    
                        <td class="myLabelLitlle"  style="vertical-align: top"><img src="../App_Themes/default/images/rojo.gif" />&nbsp;Sin Protocolo</td>
                      
                          <td >&nbsp;</td>
                          <td class="myLabelLitlle"  style="vertical-align: top"><img src="../App_Themes/default/images/verde.gif" />&nbsp;Con Protocolo</td>
                        </tr>
                        
                        </table> 
                </td>
            
            </tr>
            
            <tr>
            <td colspan="4">
              <div align="left" style="border: 1px solid #C0C0C0; overflow:scroll; overflow-x:hidden;width:850px;  height:500px; background-color: #F8F8F8;">
                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                    CellPadding="0" DataKeyNames="idTurno" 
                         EmptyDataText="No hay turnos asignados para la fecha seleccionada" 
                    Width="100%" onrowcommand="gvLista_RowCommand" 
                    onrowdatabound="gvLista_RowDataBound" 

                   CssClass="table table-bordered bs-table" GridLines="Horizontal" PageSize="1"   >
                  

                    <Columns>
                      <asp:BoundField DataField="efector" HeaderText="" />
                          <asp:BoundField DataField="efector" HeaderText="Efector" />
                        <asp:BoundField DataField="hora" HeaderText="Hora" >
                          <ItemStyle Width="10%" />
                        </asp:BoundField>
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
                        <%--    <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/editar.jpg" 
                            ommandName="Editar" />--%>
                                    <asp:LinkButton ID="Editar" runat="server" Text="" Width="20px"  >
                                             <span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
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
                         <%--   <asp:ImageButton  ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg" 
                             OnClientClick="return PreguntoEliminar();" CommandName="Eliminar" />--%>
                                  <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                        
                             <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Protocolo" runat="server" ImageUrl="~/App_Themes/default/images/flecha.jpg" 
                              CommandName="Imprimir" />
                                <%--glyphicon .glyphicon-circle-arrow-right--%>
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
                          <asp:BoundField DataField="usuario" HeaderText="Usuario" >
                        </asp:BoundField>
                        <asp:BoundField DataField="fechaRegistro" HeaderText="Fecha" />
                    </Columns>
                 

                      <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
                </asp:GridView>
               </div>
            </td>
            </tr>
            
            <tr>
            <td colspan="2" >
                                            &nbsp;</td>
            <td colspan="2" align="right" >
                <asp:LinkButton 
                            ID="lnkPlanilla" runat="server" CssClass="btn btn-info" 
                    ValidationGroup="0" onclick="lnkPlanilla_Click" Width="150px">Descargar Planilla</asp:LinkButton>&nbsp;&nbsp;
<asp:LinkButton 
                            ID="lnkPlanillaDetallada" runat="server" CssClass="btn btn-info"  Width="190px"
                    ValidationGroup="0" onclick="lnkPlanillaDetallada_Click">Descargar Planilla Detallada</asp:LinkButton>  </td>
            </tr>
            </table>
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


    function CalendarioView(idTipoServicio,idItem) {
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

        $('<iframe src="calendarioView.aspx?idTipoServicio=' + idTipoServicio + '&idItem=' + idItem + '" />').dialog({
            title: 'Días habilitados',
            autoOpen: true,
            width:350,
            height: 300,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(800);
    }

  
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de anular el turno?'))
    return true;
    else
    return false;
    }
    </script>
</asp:Content>

