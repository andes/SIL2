<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePorResultadoCovid.aspx.cs" Inherits="WebLab.Estadisticas.ReportePorResultadoCovid" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
      
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  



  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    

   <script type="text/javascript">                    
                                  
          
            
         $(function() {
             $("#<%=txtFechaDesde.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
         });

              
         $(function() {
             $("#<%=txtFechaHasta.ClientID %>").datepicker({
                 maxDate: 0,
                 minDate: null,
		            showOn: "both",
		            buttonImage: '../App_Themes/default/images/calend1.jpg',
		            buttonImageOnly: true
		        });
	        });
          </script>


    
  
  
    
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
    &nbsp;<script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script><div align="left" style="width: 1150px" class="form-inline"  >
   <div class="panel panel-danger">
                    <div class="panel-heading">
    <h4 >Estadísticas por Resultados: COVID-19<asp:HiddenField ID="HFTipoMuestra" runat="server" /></h4>
                        </div>

				<div class="panel-body">
                      <div class="form-group" >
                         <strong>      Criterio Fechas:</strong>
                          <asp:DropDownList ID="ddlCriterio" runat="server" CssClass="form-control input-sm">
                              <asp:ListItem Selected="True">Fecha de Protocolo</asp:ListItem>
                              <asp:ListItem>Fecha de Resultado</asp:ListItem>
                              <asp:ListItem>Fecha de Toma de Muestra</asp:ListItem>
                          </asp:DropDownList>
                          </div>
                         <div class="form-group" >
                   <strong>    Desde:  </strong>
                     <input id="txtFechaDesde" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>

                  <strong>   Hasta: </strong>  <input id="txtFechaHasta" runat="server" type="text" class="form-control input-sm"  maxlength="10"   style="width: 100px; "  onblur="valFecha(this)"  
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0"/>
                                                     <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar"  CssClass="btn btn-danger" Width="100px"/>
         
             

</div>  
                    </div>
                     <div class="panel-footer">	
<asp:Panel ID="pnlResultado" runat="server" Visible="False">
          <table style="width:100%;">
              <tr>
                  <td>
                    
                  </td>
                  <td align="right">
&nbsp;
        &nbsp;
        <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" style="width: 20px" /></td>
              </tr>
           
             
          </table>
      </asp:Panel>
             <asp:GridView ID="gvEstadistica0" runat="server" AutoGenerateColumns="False" 
                          DataKeyNames="idCaracter" Font-Bold="True" Font-Size="8pt"    CssClass="table table-bordered bs-table" ShowFooter="True" Width="1100px"
                            BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" 
                          CellPadding="1" Font-Names="Verdana" OnRowCommand="gvEstadistica0_RowCommand" OnRowDataBound="gvEstadistica0_RowDataBound">
                             <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                             <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" />
                          <Columns>
                              <asp:BoundField DataField="nombre" HeaderText="Caracter" >
                                  <ItemStyle Width="30%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD DE MUESTRAS" >
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="< 6 meses" HeaderText="< 6 mes." >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="6 a 11 meses" HeaderText="6 a 11 mes." >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="1 a 2" HeaderText="1 a 2" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="2 a 4" HeaderText="2 a 4" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                             
                              <asp:BoundField DataField="5 a 9" HeaderText="5 a 9" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="10 a 14" HeaderText="10 a 14" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="15 a 19" HeaderText="15 a 19" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="20 a 24" HeaderText="20 a 24" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="25 a 34" HeaderText="25 a 34 " >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="35 a 44" HeaderText="35 a 44" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="45 a 64" HeaderText="45 a 64" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                           
                                  <asp:BoundField DataField="65 a 74" HeaderText="65 a 74" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="75 y +" HeaderText="75 y + " >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="M" HeaderText="Masc." >
                                  <FooterStyle BackColor="#336699" />
                                  <HeaderStyle BackColor="#336699" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="F" HeaderText="Fem." >
                                  <FooterStyle BackColor="#336699" />
                                  <HeaderStyle BackColor="#336699" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:TemplateField HeaderText="Resultados">
                                 
                                <ItemTemplate>
                                     <asp:Button runat="server" Text="Filtrar" id="btnFiltrar" CssClass="btn btn-danger" Width="60px" />
                                     <%-- <asp:ImageButton ID="PacientesPDF0" runat="server" CommandName="PacientesPDF" 
                                          ImageUrl="~/App_Themes/default/images/pdf.jpg" />--%>
                                  </ItemTemplate> 
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Exportar" Visible="False">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesEXCEL0" runat="server" CommandName="PacientesEXCEL" 
                                          ImageUrl="~/App_Themes/default/images/excelPeq.gif" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>

        <asp:ImageButton ID="imgExcel0" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif"  
            ToolTip="Exportar a Excel" style="width: 20px" OnClick="imgExcel0_Click" />

                         <br />
             <asp:GridView ID="gvEstadistica" runat="server" AutoGenerateColumns="False" 
                          DataKeyNames="idItem" Font-Bold="True" Font-Size="8pt" 
                          onrowcommand="gvEstadistica_RowCommand"    CssClass="table table-bordered bs-table danger"
                          onrowdatabound="gvEstadistica_RowDataBound" ShowFooter="True" Width="1100px"
                            BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" 
                          CellPadding="1" Font-Names="Verdana">
                             <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                             <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" />
                          <Columns>
                              <asp:BoundField DataField="RESULTADO" HeaderText="RESULTADO" >
                                  <ItemStyle Width="30%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD DE MUESTRAS" >
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="< 6 meses" HeaderText="< 6 mes." >
                                  <FooterStyle BackColor="#CC3300" />
                                  
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="6 a 11 meses" HeaderText="6 a 11 mes." >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="1 a 2" HeaderText="1 a 2" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="2 a 4" HeaderText="2 a 4" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                             
                              <asp:BoundField DataField="5 a 9" HeaderText="5 a 9" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="10 a 14" HeaderText="10 a 14" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="15 a 19" HeaderText="15 a 19" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="20 a 24" HeaderText="20 a 24" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="25 a 34" HeaderText="25 a 34 " >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="35 a 44" HeaderText="35 a 44" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="45 a 64" HeaderText="45 a 64" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                           
                                  <asp:BoundField DataField="65 a 74" HeaderText="65 a 74" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="75 y +" HeaderText="75 y + " >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="M" HeaderText="Masc." >
                                  <FooterStyle BackColor="#336699" />
                                  <HeaderStyle BackColor="#336699" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="F" HeaderText="Fem." >
                                  <FooterStyle BackColor="#336699" />
                                  <HeaderStyle BackColor="#336699" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:TemplateField HeaderText="Pac. PDF" Visible="False">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesPDF" runat="server" CommandName="PacientesPDF" 
                                          ImageUrl="~/App_Themes/default/images/pdf.jpg" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Pac. EXCEL" Visible="False">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesEXCEL" runat="server" CommandName="PacientesEXCEL" 
                                          ImageUrl="~/App_Themes/default/images/excelPeq.gif" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>

                    

      
            
                      <br />
                    <%--    <asp:ImageButton ToolTip="Ver grafico de tortas" ID="btnVerGraficoTipoMuestra"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_torta.png"  OnClientClick="verGrafico('torta'); return false;"                       />
                                 &nbsp;&nbsp;<asp:ImageButton ToolTip="Ver grafico de barras" ID="btnVerGraficoTipoMuestra2"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_barra.png"  OnClientClick="verGrafico('barra'); return false;"                       />

          --%>
       </div>
       </div>
        </div>
       
   <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

    var valores = $("#<%= HFTipoMuestra.ClientID %>").val();
    

    function verGrafico(tipoGrafico) {
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
        $('<iframe src="Grafico.aspx?valores=' + valores + '&tipo=3&tipoGrafico=' + tipoGrafico + '" />').dialog({
            title: 'Gráfico Estadístico',
            autoOpen: true,
            width: 900,
            height: 500,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(900);
    }


    
    </script>

    </asp:Content>