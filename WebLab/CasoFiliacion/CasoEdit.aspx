<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoEdit.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoEdit" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
    

      <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-1.5.1.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/jquery-ui-1.8.9.custom.min.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/Services/js/json2.js") %>'></script>
    <script type="text/javascript" src='<%= ResolveUrl("~/script/jquery-1.9.1.js") %>' ></script>
<script type="text/javascript" src ='<%= ResolveUrl("~/script/jquery-ui.js") %>' ></script>
    <script  type="text/javascript" src='<%= ResolveUrl("~/script/jquery.ui.datepicker-es.js") %>'  ></script>   
    <link href='<%= ResolveUrl("~/Services/css/redmond/jquery.ui.all.css") %>' rel="stylesheet" type="text/css" />
      <link rel="stylesheet" href='<%= ResolveUrl("~/script/jquery-ui.css") %>'  />
  <script type="text/javascript" src="../script/ValidaFecha.js"></script>                

     
     
   
  
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <br /> &nbsp;

   <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"><input runat="server"  type="hidden" id="idServicio"/><input runat="server"  type="hidden" id="id"/>
                            <asp:Label ID="Label1" runat="server" Text="lblTitulo"></asp:Label>
                        </h3>
                        </div>
       	<div class="panel-body">	
     <table  width="600px" align="left" >
				
				<%--	<tr>
						<td class="myLabelIzquierda" >Fecha:</td>
						<td align="left" >
                                <input id="txtFecha" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; position=absolute; z-index=0;"  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/></td>
						
					</tr>--%>
					<tr>
						<td class="myLabelIzquierda" >
                            <asp:Label ID="lblNumero" runat="server" Text="Numero de Caso"></asp:Label>
                        </td>
						<td align="left" >
                                      <asp:Label ID="lblTitulo" runat="server" 
                                 Font-Bold="False"></asp:Label>    </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >Titulo del caso:</td>
						<td align="left" >
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="300px"
                                TabIndex="2" ToolTip="Ingrese el nombre de la rutina"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                ControlToValidate="txtNombre" ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator>
                        </td>
						
					</tr>
					<tr>
						<td colspan="2">&nbsp;</td>
						
					</tr>
					<tr>
						<td    style="vertical-align: top" colspan="2">
						
						<fieldset id="Fieldset1" title="Determinaciones" style="width:95%; text-align:left; ">
						<legend>Protocolos/Muestras</legend>

						<table align="left" width="100%">
						<tr>
						<td class="myLabelIzquierda" >	   Protocolo:				</td>
						<td>		  
                            <anthem:TextBox 
                                ID="txtCodigo" runat="server" AutoCallBack="True" 
                                ontextchanged="txtCodigo_TextChanged1" Width="95px" 
                             class="form-control input-sm"  TabIndex="3" 
                                ToolTip="Codigo del analisis"></anthem:TextBox>  <anthem:Label ID="lblPaciente" runat="server" Text=""></anthem:Label>  				</td>
						</tr>
						<tr>
						<td class="myLabelIzquierda" >     Parentesco:
						</td>
						<td>   
                            <anthem:DropDownList ID="ddlParentesco" runat="server" AutoCallBack="True" 
                                              
                                                TextDuringCallBack="Cargando ..." 
                             class="form-control input-sm" TabIndex="5" 
                                ToolTip="Seleccione el parentesco">
                                            </anthem:DropDownList>   
                            <anthem:Button ID="btnAgregar" runat="server" 
                                                onclick="btnAgregar_Click" Text="Agregar"  
                               CssClass="btn btn-success" Width="80px"  TabIndex="22" Enabled="False" />   
                            <asp:Button ID="btnAgregar0" runat="server" 
                                               Text="Nuevo Protocolo"  
                               CssClass="btn btn-success" Width="120px"  TabIndex="22"  OnClick="btnAgregar0_Click" /></td>
						</tr>
						<tr>
						<td colspan="2">   
                            &nbsp;</td>
						</tr>
						<tr>
						<td colspan="2">   
						
                                <asp:CustomValidator ID="cvListaDeterminaciones" runat="server" 
                                    ErrorMessage="Determinaciones" ValidationGroup="0" 
                                        onservervalidate="cvListaDeterminaciones_ServerValidate1">Debe 
                                ingresar al menos un protocolo</asp:CustomValidator>
						
                             </td>
						</tr>
						<tr>
						<td colspan="2">   
						
                                <anthem:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                onrowdatabound="gvLista_RowDataBound1" AutoGenerateColumns="False" 
                               
                                onrowcommand="gvLista_RowCommand" Width="100%" 
                                EmptyDataText="Agregue los protocolos correspondientes" 
                               
                                GridLines="Horizontal">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="nombre" HeaderText="Parentesco" >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="protocolo" HeaderText="Protocolo" >
                                    </asp:BoundField>
                                     <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                            <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/App_Themes/default/images/eliminar.jpg"
                            CommandName="Eliminar" />
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
                                </Columns>
                              <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#339933" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
                               
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
                                               OnClick="lnkRegresar_Click">Regresar</asp:LinkButton>
                                        
                        </td>
						
						<td align="right">
                                        
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los datos marcados como requeridos:" 
                                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />

                                            <asp:Button ID="btnEliminar" runat="server" Text="Anular Caso" ValidationGroup="0" 
                                            CssClass="btn btn-danger" Width="100px"  TabIndex="24" OnClientClick="eliminar(); return false;" />
                                               
                                            <asp:Button ID="btnResultados0" runat="server" Text="Informar Resultados" ValidationGroup="0" 
                                                onclick="btnResultados0_Click" CssClass="btn btn-danger" Width="200px"  TabIndex="24" Visible="False" />
                                        
                                            <asp:Button ID="btnResultados" runat="server" Text="Resultados" ValidationGroup="0"
                                                onclick="btnResultados_Click" CssClass="btn btn-danger" Width="120px"  TabIndex="24" Visible="False" />
                                        
                                            

                                        
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" 
                                                onclick="btnGuardar_Click" CssClass="btn btn-success" Width="80px"  TabIndex="24" />
                                        
                        </td>
						
					</tr>
					<tr>
						<td align="left">
                                            &nbsp;</td>
						
						<td align="right">
                                           <div>
                                              <asp:Label ID="estatus" runat="server" 
                    Style="color: #0000FF"></asp:Label>
                                           </div>
                           

						</td>
						
					</tr>
					</table>
               </div>
       </div>
  </div>
      <script type="text/javascript">                     
            //$(function() {
            //     $("#tabContainer").tabs();                        
            //    $("#tabContainer").tabs({ selected: 0 });
            // });                         
              var idCaso = $("#<%= id.ClientID %>").val();
          function eliminar() {
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
                 $('<iframe src="CasoEliminar.aspx?idCaso=' + idCaso  + '" />').dialog({
            title: 'Anular Caso',
            autoOpen: true,
            width: 400,
            height: 300,
            modal: false,
            resizable: false,
            autoResize: true,
          open: function (event, ui) { jQuery('.ui-dialog-titlebar-close').hide();},
            buttons: {
             'Cerrar': function () { <%=this.Page.ClientScript.GetPostBackEventReference(new PostBackOptions(this.lnkRegresar))%>; }               
            },
            overlay: {
                opacity:0.5,
                background: "black"
            }
        }).width(480);
    }

   

  </script>  
 </asp:Content>


