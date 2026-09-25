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

        function PreguntoCambiarEstado() {
            if (confirm('Al desmarcar la determinacion se excluirá del lote.'))
                return true;
            else
                return false;
        }
       
    </script>

</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
<div align="left" style="width:1050px">
        <div class="panel panel-default">
            <div class="panel-heading">
                <div class="row">
                    <div class="col-md-6">
                    <b><asp:Label  ID="lblTitulo" Text="" runat="server"></asp:Label></b> 
                        <br />
                        <asp:Label ID="lblSubTitulo" Text="" runat="server" visible="false"></asp:Label>
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
                <asp:HiddenField ID="HFListaDetalles" runat="server" />
                <asp:HiddenField ID="HFIdLote" runat="server" />
                <asp:HiddenField ID="HFIdEfectorDerivacion" runat="server" />
				<table  width="1000px"  >
                    <tr>
                        <td class="myLabelLitlle" style="vertical-align: top" colspan="3">
                            <asp:Panel ID="pnlReferenciasAlta" runat="server">
                                Referencias:
                                    <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente de derivar&nbsp;
                                    <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;
                                    &nbsp;<br /> </asp:Panel>
                            <asp:Panel ID="pnlReferenciaEdit" runat="server" >
                                Referencias:
                                    <img alt="" src="../App_Themes/default/images/pendiente.png" /> Pendiente de derivar&nbsp;
                                    <img alt="" src="../App_Themes/default/images/block.png" /> No enviado&nbsp;
                                    <img alt="" src="../App_Themes/default/images/reloj-de-arena.png" /> Pendiente para enviar&nbsp;
                                    <img alt="" src="../App_Themes/default/images/enviado.png" /> Enviado&nbsp;
                                    <span class="glyphicon glyphicon-inbox"></span> Recibido&nbsp;&nbsp;<br />
                            </asp:Panel>
                           
                     
                        </td>
                        
				    </tr>
                    <tr>
				        <td style="vertical-align: top" colspan="3">
                            <asp:Panel id="Panel1"   runat="server">
                                <table class="myTabla" width="1000px">
                                    <tr style="vertical-align: middle">
                                        <td align="right">
                                            <asp:Button ID="btnGuardar" runat="server" CausesValidation="true" CssClass="btn btn-primary"  Width="100" Text="Crear Lote" 
                                             onclick="btnGuardar_Click"  ValidationGroup="0" />
                                              &nbsp;&nbsp;
                                            <asp:Button ID="btnImprimir" runat="server" CssClass="btn btn-primary" Width="100" Text="Imprimir" OnClientClick="window.print(); return false;" />

                                                &nbsp;&nbsp;
                                            <asp:Button ID="btnNoEnviado" runat="server" CssClass="btn btn-danger" Width="130" Font-Size="8" Text="Marcar No enviado" 
                                                ValidationGroup="1" OnClick="btnNoEnviado_Click"/>
                                              
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
					    <td class="myLabelIzquierdaGde" colspan="3"> <hr /></td>
				    </tr>
                    <tr>
                      <td></td>
                      <td> </td>  
                        <td align="right">
                            <asp:Button ID="btnAgregarDeterminaciones" runat="server" Text="Agregar Determinaciones" Visible="false" CssClass="btn btn-primary" Width="200"
                                OnClientClick="AgregarDeterminaciones(); return false; " OnClick="btnAgregarDeterminaciones_Click"/>
                        </td>
                    </tr>
                    <tr><td> <br /></td></tr>
				    <tr>
					    <td colspan="2">
                            <asp:CustomValidator ID="cvGeneral" runat="server" OnServerValidate="cvGeneral_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvNoEnviado" runat="server" OnServerValidate="cvNoEnviado_ServerValidate" ValidationGroup="1"></asp:CustomValidator>
                             <div class="mylabelizquierda" >Seleccionar:                                           
                                <asp:LinkButton  ID="lnkMarcar" runat="server" CssClass="myLittleLink"  onclick="lnkMarcar_Click">Todas</asp:LinkButton>&nbsp;
                                <asp:LinkButton  ID="lnkDesMarcar" runat="server" CssClass="myLittleLink"   onclick="lnkDesMarcar_Click" >Ninguna</asp:LinkButton>
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
                                <!-- Lista para el alta de lote -->
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table" 
                                    DataKeyNames="idDetalleProtocolo"   Width="98%" CellPadding="0"  ForeColor="#666666" PageSize="1" 
                                    EmptyDataText ="No se encontraron protocolos para los parametros de busqueda ingresados" BorderColor="#3A93D2" 
                                    BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal" onrowcommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                                    <Columns>
            
                                    <asp:TemplateField HeaderText="Sel." >
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                         <asp:TemplateField>
                                           <ItemStyle Width="5%" HorizontalAlign="Center" />
                                           <ItemTemplate>
                                                <asp:Image ID="estado" runat="server" 
                                                    ImageUrl='<%# 
                                                    Eval("estado").ToString() == "0" ? "~/App_Themes/default/images/pendiente.png" :
                                                    Eval("estado").ToString() == "2" ? "~/App_Themes/default/images/block.png" :
                                                    "~/App_Themes/default/images/transparente.jpg"%>'  />
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
                                        <ItemTemplate>
                                            <asp:LinkButton ID="Eliminar" OnClientClick="return PreguntoEliminar();" runat="server" Text="" Width="20px" CommandName="Eliminar" CommandArgument='<%# Eval("idDetalleProtocolo") %>'>
                                                <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
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
                                    BorderStyle="Solid" BorderWidth="1px" GridLines="Horizontal"
                                      OnRowDataBound="gvListaEdit_RowDataBound">
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" Font-Names="Arial"  Font-Size="8pt" />
                                    <Columns>
            
                                    <asp:TemplateField HeaderText="Sel." >
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSel" runat="server" EnableViewState="true" 
                                                OnCheckedChanged="chkSel_CheckedChanged"   AutoPostBack="true"
                                                Checked='<%# HacerCheck(Convert.ToInt32(Eval("estado")))%> ' 
                                               Enabled='<%# Convert.ToInt32(Eval("estado")) == 0 || 
                                                            Convert.ToInt32(Eval("estado")) == 2 || 
                                                            Convert.ToInt32(Eval("estado")) == 4 %>'
                                              
                                               />
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                         <asp:TemplateField>
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
					    <td colspan="3"><hr /></td>
				    </tr>
				    <tr>
					    <td colspan="3">
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="myLink"  >Regresar</asp:HyperLink>
                        </td>
				    </tr>
				</table>
            </div>
						
</div>
   
 </div>
</asp:Content>
