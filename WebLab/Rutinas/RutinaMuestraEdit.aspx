<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RutinaMuestraEdit.aspx.cs" Inherits="WebLab.Rutinas.RutinaMuestraEdit" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
     
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

    <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
 RUTINA
                        </h3>
                       
                        </div>
        	<div class="panel-body">
       <table style="width:100%">
					 
					<tr>
						
						<td colspan="2" align="left" >
<h4> Nombre de Rutina:  <asp:Label ID="lblNombre" runat="server" Text="Label"></asp:Label></h4>
                        </td>
						
					</tr>
					 
					<tr>
						<td    style="vertical-align: top" colspan="2">
						
						<fieldset id="Fieldset1" title="Determinaciones" style="width:95%; text-align:left; ">
						<legend>Muestras</legend>

						<table align="left" width="100%">
						<tr>
						<td class="myLabelIzquierda" >	   Codigo:				</td>
						<td>		  
                            <anthem:TextBox 
                                ID="txtCodigo" runat="server" AutoCallBack="True" 
                                ontextchanged="txtCodigo_TextChanged1" Width="95px" 
                               class="form-control input-sm" TabIndex="3" Enabled="False" 
                                ToolTip="Codigo de muestra"></anthem:TextBox>  				</td>
						</tr>
						<tr>
						<td class="myLabelIzquierda" >     Nombre:
						</td>
						<td>   
                            <anthem:DropDownList ID="ddlItem" runat="server" AutoCallBack="True" 
                                                onselectedindexchanged="ddlItem_SelectedIndexChanged" 
                                                TextDuringCallBack="Cargando ..." 
                          class="form-control input-sm" TabIndex="5" Enabled="False" 
                                ToolTip="Seleccione la muestra">
                                            </anthem:DropDownList></td>
						</tr>
						<tr>
						<td colspan="2">   
                            <anthem:Button ID="btnAgregar" runat="server" 
                                                onclick="btnAgregar_Click" Text="Agregar"  CssClass="btn btn-primary"  Width="120px"
                             TabIndex="22" /></td>
						</tr>
						<tr>
						<td colspan="2">   
						
                                <asp:CustomValidator ID="cvListaDeterminaciones" runat="server" 
                                    ErrorMessage="Muestras" ValidationGroup="0" 
                                        onservervalidate="cvListaDeterminaciones_ServerValidate1">Debe 
                                ingresar al menos una muestra</asp:CustomValidator>
						
                             </td>
						</tr>
						<tr>
						<td colspan="2">   
						
                                <anthem:GridView ID="gvLista" runat="server" 
                                onrowdatabound="gvLista_RowDataBound1" AutoGenerateColumns="False" 
                              CssClass="table table-bordered bs-table" 
                                onrowcommand="gvLista_RowCommand" Width="100%" 
                                EmptyDataText="Agregue los analisis correspondientes" 
                            
                                GridLines="Horizontal">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#3A93D2" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:BoundField DataField="nombre" HeaderText="Muestra" >
                                        <ItemStyle Width="95%" />
                                    </asp:BoundField>
                                     <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                            CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
                                </Columns>
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <RowStyle BackColor="#EFF3FB" />
                            </anthem:GridView>
                            
                                </td>
						</tr>
						</table>
                             </td>
						</tr>
					<tr>
						<td   colspan="2">
                                            <hr /></td>
						
					</tr>
					<tr>
						<td align="left">
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="RutinaBroList.aspx">Regresar</asp:LinkButton>
                                        
                        </td>
						
						<td align="right">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los datos marcados como requeridos:" 
                                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" 
                                                onclick="btnGuardar_Click"  CssClass="btn btn-primary"  Width="120px" TabIndex="24" />
                                        
                        </td>
						
					</tr>
					</table>
                </div>
       </div>
  </div>
 </asp:Content>


