<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoResultadoHisto.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoResultadoHisto" MasterPageFile="~/Site1.Master" %>

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

     
     
     <script type="text/javascript">                     
            $(function() {
                 $("#tabContainer").tabs();                        
                $("#tabContainer").tabs({ selected: 0 });
             });                         
          
            
   

  </script>  
  
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <br /> 
    <br /> <br /> 
   <div align="center" style="width: 900px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">INFORME DE RESULTADOS HISTOCOMPATIBILIDAD</h3><input runat="server"  type="hidden" id="id"/>
                        <input runat="server"  type="hidden" id="Desde"/>
                        </div>
       	<div class="panel-body">	
     <table  width="600px" align="center" >
				
			 
					<tr>
						<td   >
                        <h4>    <asp:Label ID="lblNumero" runat="server" Text="Numero de Caso"></asp:Label></h4>
                        </td>
						<td align="left" >
                         <h4>      <asp:Label ID="lblTitulo" runat="server" 
                                 Font-Bold="False"></asp:Label> </h4> </td>
						
					</tr>
					<tr>
						<td   ><h4>Titulo del caso:</h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="1" ToolTip="Ingrese el titulo"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td   ><h4>Entidad solicitante:</h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtSolicitante" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="2"></asp:TextBox>
                        </td>
						
					</tr>
					<%--<tr>
						<td   ><h4>Autos:</h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtAutos" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="3"></asp:TextBox>
                        </td>
						
					</tr>--%>
					<%--<tr>
						<td  ><h4>Objetivo del caso:</h4></td>
						<td align="left" >
                            <asp:TextBox ID="txtObjetivo" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="4">Determinación de vínculo de parentesco.</asp:TextBox>
                        </td>
						
					</tr>--%>
					<tr>
						<td class="myLabelIzquierda" >&nbsp;</td>
						<td align="left" >
                           
                        </td>
						
					</tr>
					<tr>
						<td colspan="2"   ><h4>Muestras analizadas</h4></td>
						
					</tr>
					<tr>
						<td colspan="2">   
						
                                <anthem:GridView ID="gvLista" runat="server"  CssClass="table table-bordered bs-table" 
                                onrowdatabound="gvLista_RowDataBound1" AutoGenerateColumns="False" 
                               
                                onrowcommand="gvLista_RowCommand" Width="100%" 
                                EmptyDataText="Agregue los protocolos correspondientes" 
                               
                                GridLines="Horizontal" Font-Size="8pt">
                            
                               
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Muestra" />
                                    <asp:BoundField DataField="nombre" HeaderText="Parentesco" >
                                    </asp:BoundField>
                                       <asp:BoundField DataField="protocolo" HeaderText="Protocolo" >
                                    </asp:BoundField>
                              

                                </Columns>
                              <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle  Font-Bold="True" ForeColor="Black" />
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
					<tr>
						<td><h4>Resultados</h4></td>
						
						<td>
                           
                            </td>
						
					</tr>
					<tr>
						<td colspan="2">
						
                                <anthem:GridView ID="gvResultado" runat="server"  CssClass="table table-bordered bs-table" 
                                onrowdatabound="gvLista_RowDataBound1" 
                               
                                onrowcommand="gvLista_RowCommand" Width="100%" 
                                EmptyDataText="Agregue los protocolos correspondientes" 
                               
                                GridLines="Horizontal" Font-Size="8pt">
                            
                               
                              <PagerSettings Mode="NumericFirstLast" Position="Top" />
                                                                    <HeaderStyle  Font-Bold="True" ForeColor="Black" />
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
					<tr>
						<td>
                            <h4>
                                 Interpretación:</h4>
                        </td>
						
						<td>&nbsp;</td>
						
					</tr>
					<tr>
						<td colspan="2">
                            <asp:TextBox ID="txtConclusion" runat="server" class="form-control input-sm" Width="600px"
                                TabIndex="6" TextMode="MultiLine" Rows="10">
                            </asp:TextBox>
                            </td>
						
					</tr>
					<tr>
						<td>&nbsp;</td>
						
						<td>&nbsp;</td>
						
					</tr>
					<tr>
						<td><h4> Método:</h4></td>
						
						<td>
                            <asp:TextBox ID="txtMetodo" runat="server" class="form-control input-sm" Width="400px"
                                TabIndex="7">SSO por LUMINEX</asp:TextBox>
                            </td>
						
					</tr>
				

				

					<tr>
						<td colspan="2"><table >
                    <tr>
                        <td >

                      <h4>      Observaciones: </h4>
						    <anthem:DropDownList class="form-control input-sm" ID="ddlObsCodificadaGeneral" runat="server" >
                               </anthem:DropDownList>

                               <anthem:Button  CssClass="btn btn-danger"  OnClick="btnAgregarObsCodificadaGral_Click"  ID="btnAgregar"  Width="100px" runat="server" Text="Agregar" />
                               <br />
                           <anthem:TextBox class="form-control input-sm" ID="txtObservacion" runat="server" TextMode="MultiLine" 
                              Width="600px" MaxLength="4000" Height="200px"></anthem:TextBox>  
                              

                        </td>
                    </tr>
                    <tr>
                        <td><hr />
                        </td>
                    </tr>
                                        
                </table></td>
						
					</tr>
				

				

					<tr>
						<td colspan="2">
                            <asp:Label ID="lblUsuario" runat="server" Text="Label" Visible="False"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td   colspan="2">
                                            &nbsp;</td>
						
					</tr>
         </table>
               </div>
         <div class="panel-footer">		
             <table width="100%">
					<tr>
						<td align="left">
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                OnClick="lnkRegresar_Click">Regresar</asp:LinkButton>
                                        
                        </td>
						
						<td align="right">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                HeaderText="Debe completar los datos marcados como requeridos:" 
                                                ShowMessageBox="True" ValidationGroup="0" ShowSummary="False" />
                                            <asp:Button ID="btnValidar" runat="server" Text="Validar" ValidationGroup="0" 
                                                onclick="btnValidar_Click" CssClass="btn btn-success" Width="80px"  TabIndex="15" />
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" ValidationGroup="0" 
                                                onclick="btnGuardar_Click" CssClass="btn btn-success" Width="80px"  TabIndex="14" />
                                   <asp:LinkButton ID="imgPdf" runat="server" CssClass="btn btn-info" Text="Buscar" OnClick="imgPdf_Click" Width="200px" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar Resultados</asp:LinkButton>

                                        
                        </td>
						
					</tr>
					</table>
               </div>
     
  </div>
    </div>
</asp:Content>


