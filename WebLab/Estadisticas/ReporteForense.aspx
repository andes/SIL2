<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteForense.aspx.cs" Inherits="WebLab.Estadisticas.ReporteForense" MasterPageFile="~/Site1.Master" ValidateRequest="false"%>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  


  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
  

                   <script type="text/javascript">


    


     
                 

	$(function() {
		$("#<%=txtFechaDesde.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

	$(function() {
		$("#<%=txtFechaHasta.ClientID %>").datepicker({
			showOn: 'button',
			buttonImage: '../App_Themes/default/images/calend1.jpg',
			buttonImageOnly: true
		});
	});

 
  
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
 <asp:HiddenField runat="server" ID="HFCurrTabIndex"  /> 
   

       <div       class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h4 > ESTADISTICAS GENETICA FORENSE</h4>
                        </div>

				<div class="panel-body">

       <div class="form-group">       
 
    <h5>Fecha Desde: </h5>
 
 
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio"  />
  </div>
   <div class="form-group">       
 
    <h5>
    Fecha Hasta: </h5>

                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  /> 
       </div>
             <div class="form-group">                                
                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="150px"
                                onclick="btnGenerar_Click" Text="Generar Reporte"  
                                ValidationGroup="0" />
    
                            </div>  

                    </div>
       <div class="panel-footer" ID="pnlResultados" runat="server" visible="false">
     <table>
         <tr>
               <td style="vertical-align: top;">
<div class="panel panel-primary" style="width:220px;">
                    <div class="panel-heading">
                         <h4 > Casos </h4>
  
                        </div>

				<div class="panel-body">
                   
   <asp:GridView ID="gvCasos"  runat="server" Width="30%"  CssClass="table table-bordered bs-table"  
                          CellPadding="1" onrowdatabound="gvTipoMuestra_RowDataBound" AutoGenerateColumns="False" EmptyDataText="No se encontraron resultados">
                          <Columns>
                              <asp:BoundField DataField="tipo" HeaderText="" />
                                            <asp:BoundField DataField="cantidad" 
                                  HeaderText="Cantidad" >
                                          

                              </asp:BoundField>
                          </Columns>
                            

                      </asp:GridView>
                    </div>
     	<div class="panel-footer">
      <h4>  <asp:Label ID="lblCantidadCasos" runat="server" Text="Label" Visible="false"></asp:Label></h4>

				</div>
     </div>
             </td>
              <td style="vertical-align: top;">
<div class="panel panel-primary" style="width:220px;">
                    <div class="panel-heading">
    <h4 > Muestras </h4>
                        </div>

				<div class="panel-body">
    <asp:GridView ID="gvMuestras"  runat="server" Width="30%"  CssClass="table table-bordered bs-table"  EmptyDataText="No se encontraron resultados"
                          CellPadding="1"   AutoGenerateColumns="False" onrowdatabound="gvMuestras_RowDataBound">
                          <Columns>
                              <asp:BoundField DataField="tipo" HeaderText="" />
                                            <asp:BoundField DataField="cantidad" 
                                  HeaderText="Cantidad" >
                                          

                              </asp:BoundField>
                          </Columns>
                            

                      </asp:GridView>
   
     </div>

				<div class="panel-footer">
                     <h4  ><asp:Label ID="lblCantidadMuestras" runat="server" Text="Label" Visible="false"></asp:Label></h4>
      
                    </div>
  </div>

             </td>
         </tr>
     </table>
 

      
   <table>
         <tr>
           <td style="vertical-align: top;">
                   

      <div class="panel panel-primary" style="width:500px;" >
                    <div class="panel-heading">
    <h4 >     Cantidad Muestras x Tipo  </h4>
                        </div>

				<div class="panel-body">
 
    <h5 >Distribucion por tipo de muestra</h5>
  <asp:GridView ID="gvTipoMuestra" runat="server" CssClass="table table-bordered bs-table">
                        
                        
                      </asp:GridView>  
   
</div>
          </div>
 </td>
             </tr>
       </table>
               
   <table>
         <tr>
             <td style="vertical-align: top;">
                       <div class="panel panel-primary" style="width:500px;" ID="Div1" runat="server">
                    <div class="panel-heading">
    <h4 >Tipo de Vinculo </h4>
                        </div>

				<div class="panel-body">
   
      <asp:GridView ID="gvParentesco" runat="server" CssClass="table table-bordered bs-table">
                        
                        
                      </asp:GridView>  
  </div>
</div>
                 </td>
              <td style="vertical-align: top;">
                        <div class="panel panel-primary" style="width:500px;" ID="Div2" runat="server">
                    <div class="panel-heading">
    <h4 >Lugar de Extracción</h4>
                        </div>

				<div class="panel-body">
   
      <asp:GridView ID="gvLugarExtraccion" runat="server" CssClass="table table-bordered bs-table">
                        
                        
                      </asp:GridView>  
  </div>
</div>
                 </td>
             </tr>
       </table> 


           <table>
         <tr>
              <td style="vertical-align: top;">
                    <div class="panel panel-danger" style="width:500px;" ID="Div3" runat="server">
                    <div class="panel-heading">
    <h4 >Solicitante Forense</h4>
                        </div>

				<div class="panel-body">
 
        <asp:GridView ID="gvOrigenFiliacion" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table"  ShowFooter="True" Width="90%" OnRowDataBound="gvOrigenFiliacion_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Origen Forense" HeaderText="Origen Forense" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Casos">
                                            <ItemStyle BackColor="#EEEEEE" />
                                            </asp:BoundField>
                                        </Columns>
                                  
                                    </asp:GridView> 
  </div>
</div>
 
                        </td>     

                      <td style="vertical-align: top;">
      <div class="panel panel-success" style="width:500px;" ID="Div4" runat="server">
                    <div class="panel-heading">
    <h4 >Solicitante Filiacion</h4>
                        </div>

				<div class="panel-body">
   
    <h5 >Distribucion de casos por Solicitante </h5>
    <asp:GridView ID="gvSolicitante" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered bs-table"  ShowFooter="True" Width="90%" OnRowDataBound="gvSolicitante_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Origen Filiacion" HeaderText="Origen Filiacion" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Casos">
                                            <ItemStyle BackColor="#EEEEEE" />
                                            </asp:BoundField>
                                        </Columns>
                                     

                                    </asp:GridView> 
  </div>
</div>
                      
              </td>
             </tr>
               </table>
       
            
         
           </div>

           </div>
   

    </div>
</asp:Content>