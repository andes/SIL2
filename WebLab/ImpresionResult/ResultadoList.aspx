<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoList.aspx.cs" Inherits="WebLab.ImpresionResult.ProtocoloList" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


 

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
<script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          

    <div align="left" style="width: 1100px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
<table width="100%"><tr>
    <td>  <h3 class="panel-title">LISTA DE PROTOCOLOS  <br /> 
         
       </h3></td>
    <td align="right"> <span class="label label-default" __designer:mapid="fd"><asp:Label ID="lblServicio" runat="server" 
                                       Text="Label"></asp:Label></span></td></tr></table>
  
                        </div>

				<div class="panel-body">

				 <table  width="1000px" align="left" cellpadding="0" cellspacing="0" >
					
				
				
				

					<tr>
						  <td colspan="2">
                                    
                            
                              <asp:LinkButton ID="lnkPDF" runat="server" CssClass="btn btn-info" ForeColor="White" Text="" OnClick="lnkPDF_Click" ValidationGroup="0" Width="140px" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar PDF</asp:LinkButton>

                               <asp:LinkButton ID="lnkMarcarImpresos" runat="server" CssClass="btn btn-info" ForeColor="White" Text="" OnClick="lnkMarcarImpresos_Click" ValidationGroup="0" Width="180px" >
                                             <span class="glyphicon glyphicon-check"></span>&nbsp;Marcar como Impresos</asp:LinkButton>
                                               <asp:CheckBox ID="chkAnalisisResultados" runat="server" 
                                                CssClass="myLabel" Text="Imprimir sólo análisis con resultados" 
                                                Checked="True" />
                                            <asp:CheckBox ID="chkAnalisisValidados" runat="server" 
                                                CssClass="myLabel" Text="Imprimir sólo análisis validados" 
                                                Checked="True" />   

						</td>
						

						
					<%--	<td align="right" class="myLabelIzquierda">
                                            <div>
                                                Impresora del sistema:<asp:DropDownList ID="ddlImpresora" runat="server" class="form-control input-sm">
                                                </asp:DropDownList>
                                                <asp:LinkButton ID="lnkImprimir" runat="server" CssClass="btn btn-danger" onclick="lnkImprimir_Click" Text="" Width="120px" ValidationGroup="0">
                                             <span class="glyphicon glyphicon-print">&nbsp;Imprimir</span></asp:LinkButton>
                                            </div>
                        </td>--%>
						
					</tr>
					
					
				
						<tr>
						<td  class="myLabelIzquierda" >
                                        <%--      <div class="mylabelizquierda"> Seleccionar:                                           <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton></div>--%>

                              <ul class="pagination">
                                                               <li>                                                   <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todos&nbsp;&nbsp;</asp:LinkButton></li>
                                            <li><asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguno</asp:LinkButton></li>
                                </ul>



						</td>
						
						<td   style="color: #333333; font-size: 12px"  align="right">
                                           <asp:Label ID="CantidadRegistros" runat="server"                                                                forecolor="Blue" Font-Size="8pt" />
                            <br />
                             <span class="label label-danger">No procesado</span>
<span class="label label-warning">En proceso</span>
<span class="label label-success">Terminado</span>
                                                            </td>
						
					</tr>
                     	</table>
                     
					 
						
</div>
       <div class="panel-footer">		
						<div  style="width:100%;height:450pt;overflow:scroll;overflow-x:hidden;border:1px solid #CCCCCC; background-color: #F3F3F3;"> 
        <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" DataKeyNames="idProtocolo" Font-Size="9pt" CssClass="table table-bordered bs-table" 
                                                Width="98%" CellPadding="1" 
                                ForeColor="#666666" PageSize="40" EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados" 
                                onrowdatabound="gvLista_RowDataBound" BorderColor="#3A93D2" 
                              
                                AllowPaging="False" onpageindexchanging="gvLista_PageIndexChanging">
         
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                <asp:BoundField DataField="impreso" />
          <asp:BoundField DataField="numero" 
                    HeaderText="Protocolo" >
                    <ItemStyle Width="5%" Font-Bold="true" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <ItemStyle Width="30%" />
                </asp:BoundField>
                <asp:BoundField DataField="origen" HeaderText="Origen">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="prioridad" HeaderText="Prioridad">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="sector" HeaderText="Sector">
                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="estado" HeaderText=" " >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
          <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>

        <asp:GridView ID="gvListaProducto" runat="server" AutoGenerateColumns="False" DataKeyNames="idProtocolo" Font-Size="9pt" Font-Bold="true" CssClass="table table-bordered bs-table" 
                                                Width="100%" CellPadding="1" 
                                ForeColor="#666666" PageSize="40" 
                                
                                
                                
                                EmptyDataText="No se encontraron protocolos para los parametros de busqueda ingresados" 
                                onrowdatabound="gvListaProducto_RowDataBound" BorderColor="#3A93D2" 
                              
                                AllowPaging="False" onpageindexchanging="gvListaProducto_PageIndexChanging">
         
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                <asp:BoundField DataField="impreso" />
          <asp:BoundField DataField="numero" 
                    HeaderText="Protocolo" >
                    <ItemStyle Width="5%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha" >
                    <ItemStyle Width="8%" HorizontalAlign="Center" />
                </asp:BoundField>
               <asp:BoundField DataField="conservacion" HeaderText="Muestra">
                                                                        <ItemStyle  Width="10%" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="muestra" HeaderText="Descripcion">
                                                                        <ItemStyle Width="30%" />
                                                                    </asp:BoundField>
                <asp:BoundField DataField="origen" HeaderText="Origen">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="prioridad" HeaderText="Prioridad">
                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="sector" HeaderText="Sector">
                    <ItemStyle HorizontalAlign="Left" Width="20%" />
                </asp:BoundField>
                <asp:BoundField DataField="estado" HeaderText=" " >
            

                 
                
                        
         
                        
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            

                 
                
                        
         
                        
            </Columns>
          <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle BackColor="#800000" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>

        </div>
                       
				 <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
				</div>
       </div>
        </div>



  
   
 
 </asp:Content>
