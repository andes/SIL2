<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CasoResultadoHistoView.aspx.cs" Inherits="WebLab.CasoFiliacion.CasoResultadoHistoView" MasterPageFile="~/Resultados/SitePE.Master" %>

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
  
    <style type="text/css">
        .auto-style1 {
            width: 600px;
        }
        .auto-style4 {
            width: 176px;
        }
        .auto-style5 {
            width: 462px;
        }
        .auto-style6 {
            height: 22px;
        }
        .auto-style7 {
            height: 23px;
        }
    </style>
  
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <br /> 
    <br /> <br /> 


   <div align="center" style="width: 700px" class="form-inline"  >

       <table>
           <tr>
               <td style="width:100px;">&nbsp;&nbsp;&nbsp;&nbsp; </td>
               <td><div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">INFORME DE RESULTADOS HISTOCOMPATIBILIDAD</h3><input runat="server"  type="hidden" id="id"/>
                        <input runat="server"  type="hidden" id="Desde"/>
                        </div>
       	<div class="panel-body">	
     <table  width="600px" align="center" class="auto-style1" >
				
			 
					<tr>
						<td class="auto-style4"   >
                        <h4>    <asp:Label ID="lblNumero" runat="server" Text="Numero de Caso"></asp:Label></h4>
                        </td>
						<td align="left" class="auto-style5" >
                         <h4>      <asp:Label ID="lblTitulo" runat="server" 
                                 Font-Bold="False"></asp:Label> </h4> </td>
						
					</tr>
					<tr>
						<td class="auto-style4"   ><h4>Título del caso:</h4></td>
						<td align="left" class="auto-style5" >
                            <asp:Label ID="lblNombre" runat="server" 
                                 Font-Bold="False"></asp:Label> 
                        </td>
						
					</tr>
					<tr>
						<td class="auto-style4"   ><h4>Entidad solicitante:</h4></td>
						<td align="left" class="auto-style5" >
                            <asp:Label ID="lblEntidad" runat="server" 
                                 Font-Bold="False"></asp:Label> 
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
						<td class="auto-style4" >&nbsp;</td>
						<td align="left" class="auto-style5" >
                           
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
                               
                                GridLines="Horizontal" Font-Size="9pt">
                            
                               
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
						<td colspan="2"><h4>Resultados</h4></td>
						
					</tr>
					<tr>
						<td colspan="2">
						
                                <anthem:GridView ID="gvResultado" runat="server"  CssClass="table table-bordered bs-table" 
                                onrowdatabound="gvLista_RowDataBound1" 
                               
                                onrowcommand="gvLista_RowCommand" Width="100%" 
                                EmptyDataText="Agregue los protocolos correspondientes" 
                               
                                GridLines="Horizontal" Font-Size="9pt">
                            
                               
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
						<td colspan="2">
                            <h4>
                                 Interpretación:</h4>
                        </td>
						
					</tr>
					<tr>
						<td colspan="2" class="auto-style6">
                            <asp:Label ID="lblConclusion" runat="server" 
                                 Font-Bold="true"></asp:Label> 
                            </td>
						
					</tr>
					<tr>
						<td class="auto-style7" colspan="2"></td>
						
					</tr>
					<tr>
						<td colspan="2"><h4> Método:</h4><asp:Label ID="lblMetodo" runat="server" 
                                 Font-Bold="False"></asp:Label> 
                            </td>
						
					</tr>
				

				

					<tr>
						<td colspan="2">&nbsp;</td>
						
					</tr>
				

				

					<tr>
						<td colspan="2"><table >
                    <tr>
                        <td >

                      <h4>      Observaciones: </h4>
                            <p>      
                            <asp:Label ID="lblObservaciones" runat="server" 
                                 Font-Bold="False"></asp:Label> 
                            </p>
                               <br />
                              

                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
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
                                   <asp:LinkButton ID="imgPdf" runat="server" CssClass="btn btn-info" Text="Buscar" OnClick="imgPdf_Click" Width="200px" >
                                             <span class="glyphicon glyphicon-download-alt"></span>&nbsp;Descargar Resultados</asp:LinkButton>

                                        
                        </td>
						
					</tr>
					</table>
               </div>
     
  </div></td>
           </tr>
       </table>

   
    </div>
</asp:Content>


