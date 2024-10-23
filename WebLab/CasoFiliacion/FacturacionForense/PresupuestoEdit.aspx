<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="PresupuestoEdit.aspx.cs" Inherits="WebLab.CasoFiliacion.FacturacionForense.PresupuestoEdit" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <link type="text/css"rel="stylesheet"      href="../../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
	
   	 <script type="text/javascript" src="../../script/Mascara.js"></script>
    <script type="text/javascript" src="../../script/ValidaFecha.js"></script>   
	<script type="text/javascript">
 
	 
    

      
            
         $(function() {
             $("#<%=txtFecha.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
	        });
	 

           

        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
       <div align="left" style="width: 900px" class="form-inline"  >
            <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-danger" Width="100px"  Text="Regresar" 
        OnClick="btnRegresar_Click" TabIndex="14" /> 
            <br /> 
   <div class="panel panel-danger">

     
       <div class="panel-heading">
                                   
              
                        
           
                           <h3 class="panel-title">  Presupuesto Forense <asp:Label ID="nroPresupuesto" Visible="false" runat="server" Text="Label"></asp:Label></h3> 
           <br />
            <div class="form-group" runat="server"    >     
             <asp:Label class="label label-danger" ID="lblEstado" runat="server" Text=""></asp:Label> 
                 </div>
          </div>
         <div class="panel-body">	
           
 <div class="form-group" runat="server"    >                                     
        <h5>   Nombre: 
                                            </h5>
     <asp:TextBox ID="txtNombrePresupuesto" runat="server" Width="550px" class="form-control input-sm"></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNombrePresupuesto" ErrorMessage="*" ValidationGroup="0"></asp:RequiredFieldValidator>
                          </div>
              
      <div class="form-group" runat="server"    >                                     
        <h5>   Fecha: 
                                            </h5>
 <input id="txtFecha" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; position=absolute; z-index=0;"  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="1"/>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFecha" ErrorMessage="*" ValidationGroup="0"  >*</asp:RequiredFieldValidator>
                          </div>
             <br />

               <div class="form-group">
                             <h4>Dirigido a </h4>
                      <asp:DropDownList ID="ddlSectorServicio" class="form-control input-sm" runat="server" TabIndex="2" Width="700px">
                                        </asp:DropDownList>
                             <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="ddlSectorServicio" ErrorMessage="*" MaximumValue="9999" MinimumValue="1" ValidationGroup="0"></asp:RangeValidator>
                           
                             </div>
              <br />
              <div class="form-group">
                     <asp:TextBox ID="txtEncabezado1" runat="server" class="form-control input-sm" Width="700px" 
                               TextMode="MultiLine" Rows="4" TabIndex="3" ToolTip="Ingrese el resultado">Al asistente Letrado
Unidad fiscal .......................
Dr.  
</asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtEncabezado1" ErrorMessage="*" ValidationGroup="0"></asp:RequiredFieldValidator>
                  </div>
             <br />
               <div class="form-group">
                             <h4>Cuerpo</h4>
                              <asp:TextBox ID="txtEncabezado2" runat="server" class="form-control input-sm" Width="700px" 
                               TextMode="MultiLine" Rows="7" TabIndex="4" ToolTip="Ingrese el resultado">Me dirijo a Ud. en referencia al caso CASO Nº ________________________________________________  que tramitan por ante la Unidad Fiscal a su cargo a los efectos adjuntar presupuesto para la realización de la pericia de ADN ordenada, según “Contrato de locación de servicios de informes periciales con sustento en resultados del análisis por vía del polimorfismo de ADN en muestras suministradas por organismos del Poder Judicial de Neuquén , Expte N° 25384. Con vigencia desde el 01 de junio de 2019.
</asp:TextBox>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEncabezado2" ErrorMessage="*" ValidationGroup="4"  >*</asp:RequiredFieldValidator>
                    
                             </div>


              <br />
              <h4>Detalle</h4>
             <div id="tab3" class="tab_content" >
    <table style="width: 700px"  >
<tr>
						<td class="myLabelIzquierda" >
                                            Codigo:</td>
						<td>
                            <anthem:TextBox ID="txtCodigo" runat="server"   class="form-control input-sm"          
                               style="text-transform:uppercase"   ontextchanged="txtCodigo_TextChanged" Width="88px" AutoCallBack="True" 
                                TabIndex="5"></anthem:TextBox>
                            <anthem:DropDownList ID="ddlItem" runat="server"    class="form-control input-sm"          
                                onselectedindexchanged="ddlItem_SelectedIndexChanged" AutoCallBack="True" 
                                TabIndex="6" Width="300px">
                            </anthem:DropDownList>
                                        
                        </td>
						
					</tr>
<tr>
						<td class="myLabelIzquierda" >
                                            Precio Unitario:</td>
						<td>
                                        
                            <anthem:TextBox ID="txtPrecio" runat="server"    class="form-control input-sm"           Width="80px" AutoCallBack="True" 
                                 TabIndex="7" Enabled="False" MaxLength="50"></anthem:TextBox>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Descripción:</td>
						<td>
                            <anthem:TextBox ID="txtNombre" runat="server"    class="form-control input-sm"           Width="550px" AutoCallBack="True" 
                                 TabIndex="8" Rows="3" TextMode="MultiLine" Enabled="False" MaxLength="500"></anthem:TextBox>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Cantidad</td>
						<td>
                            <anthem:TextBox ID="txtCantidad" runat="server"    class="form-control input-sm"           Width="50px" AutoCallBack="True" 
                                 TabIndex="9" Enabled="False" MaxLength="50"></anthem:TextBox>
                                        
                            <anthem:Button ID="btnAgregar" runat="server" CssClass="btn btn-info" Enabled="False" onclick="btnAgregar_Click" TabIndex="10" Text="Agregar" Width="80px" />
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            &nbsp;</td>
						<td>
                            <asp:CustomValidator ID="cvAnalisis" runat="server" 
                                ErrorMessage="Debe completar al menos un codigo" 
                                onservervalidate="cvAnalisis_ServerValidate" ValidationGroup="0" 
                                Font-Size="8pt">Debe completar al menos un codigo</asp:CustomValidator>
                                        <anthem:Label ID="lblMensaje" runat="server" ForeColor="#FF3300">&nbsp;&nbsp; </anthem:Label>
                                        
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" colspan="2" >
						
                                <anthem:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                onrowdatabound="gvLista_RowDataBound1" AutoGenerateColumns="False" 
                               
                                onrowcommand="gvLista_RowCommand" Width="800px" 
                                EmptyDataText="Agregue los protocolos correspondientes" 
                               
                                GridLines="Horizontal" DataKeyNames="fila">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="fila" HeaderText="Linea" />
                                    <asp:BoundField DataField="codigo" HeaderText="Codigo Nom." >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="nombre" HeaderText="Descripcion" >
                                    </asp:BoundField>
                                     <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                                    <asp:BoundField DataField="precio" HeaderText="Precio Unitario" />
                                    <asp:BoundField DataField="total" HeaderText="Total" />
                                     <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                           <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                            </ItemTemplate>
                          
                            <ItemStyle HorizontalAlign="Center" Width="20px" Height="18px" />
                          
                        </asp:TemplateField>
                                </Columns>
                              <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
                               
                            </anthem:GridView>


                            
                                TOTAL:
                                        
                            <anthem:TextBox ID="txtTotal" runat="server"    class="form-control input-sm"           Width="150px" AutoCallBack="True" 
                                 TabIndex="11" Enabled="False" MaxLength="50"></anthem:TextBox>
                                        

                            
                                </td>
						
					</tr>
					</table>
</div>

             <br />
               <div class="form-group">
                             <h4>Pie
                             </h4>
                              <asp:TextBox ID="txtPie" runat="server" class="form-control input-sm" Width="700px" 
                               TextMode="MultiLine" Rows="5" TabIndex="12" ToolTip="Ingrese el resultado">El presupuesto corresponde al costo específicamente del análisis realizado; no se perciben honorarios.
Independientemente que el Laboratorio consiga o no extraer ADN de la muestra o del resultado obtenido, el costo del estudio deberá ser abonado ya que el análisis se habrá realizado.
Sin otro particular, me despido de Usted muy atentamente.
</asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPie" ErrorMessage="*" ValidationGroup="4"  >*</asp:RequiredFieldValidator>
                             </div>

             </div>


       <div class="panel-footer">
               <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-danger" Width="100px"  Text="Guardar" 
        onclick="btnGuardar_Click" ValidationGroup="0" TabIndex="13" />  
                        
               <asp:Button ID="btnImprimir" runat="server" CssClass="btn btn-danger" Width="100px"  Text="Descargar" 
        OnClick="btnImprimir_Click" TabIndex="14" />  
                        
           </div>
       </div>
           </div>

    <script language="javascript" type="text/javascript">
 

    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de eliminar la línea?'))
    return true;
    else
    return false;
}

    </script>
</asp:Content>
