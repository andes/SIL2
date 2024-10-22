<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteMicrobiologiaNoPaciente.aspx.cs" Inherits="WebLab.Estadisticas.ReporteMicrobiologiaNoPaciente" MasterPageFile="~/Site1.Master" ValidateRequest="false"%>
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


$(function () {
    $("#tabContainer").tabs();
    var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
    $("#tabContainer").tabs({ selected: currTab });
});

  
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
 <asp:HiddenField runat="server" ID="HFCurrTabIndex"  /> 
     
       <div align="left" style="width: 1000px" class="form-inline"  >
   <div class="panel panel-danger">
                    <div class="panel-heading">
    <h5 > REPORTE ESTADISTICO MICROBIOLOGIA DE MUESTRAS DE NO PACIENTES</h5>
                        </div>

				<div class="panel-body">

    <table align="center" class="style10" width="100%" >

  

      
       
<tr>
<td   align="left"> 
  
 
    Efector:</td>
<td class="style12"  align="left" colspan="4"> 
  
 
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                            </td>
</tr>

      
       
      
       
<tr>
<td   align="left"> 
  
 
    Fecha Desde:</td>
<td class="style12"  align="left"> 
  
 
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio"  /></td>
<td    align="left"> 
  
 
    Fecha Hasta:</td>
<td class="style13"  align="left"> 
  
 

                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  /></td>
<td class="style13"  align="left"> 
  
 

                                        
                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="130px"
                                onclick="btnGenerar_Click" Text="Generar Reporte"  
                                ValidationGroup="0" />
    
                                        
                                   
                                        
                                        </td>
</tr>

      
       
<tr>
<td class="auto-style1" align="left" 
        style="vertical-align: top;"> 
  
 
    </td>
<td class="auto-style1"  align="left" colspan="3"> 
  
 
                            <asp:CheckBoxList ID="ChckOrigen" Visible="false" runat="server" 
        RepeatDirection="Horizontal" RepeatColumns="5">
                            </asp:CheckBoxList>
    </td>
<td class="auto-style1"  align="left"> 
  
 
    </td>
</tr>

      
       
     </table>
                    </div>
                   
                    <div class="panel-footer">	
                  
  
      <asp:Panel ID="pnlResultado" runat="server">
      						    

        <div id="tabContainer">  
                           
                             <ul>
    <li><a href="#tab0"><b>Tipo de muestras</b></a></li>  
    <li><a href="#tab1"><b>Origen</b></a></li>                              
    <li><a href="#tab2"><b>Resultados</b></a></li>      
    <li><a href="#tab3"><b>Aislamiento</b></a></li>  
</ul>                          


          <table style="width:100%;">
              <tr>
                  <td>
                      
                  </td>
                  <td align="right">
                     <asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" 
            ToolTip="Exportar a Pdf" onclick="imgPdf_Click" Visible="False" />
&nbsp;
        &nbsp;
        </td>
              </tr>
              </table>


              <div  id="tab0" >   
             <asp:HiddenField ID="HFTipoMuestra" runat="server" />
                  <asp:HiddenField ID="HFMicroorganismo" runat="server" />
                  <asp:HiddenField ID="HFResistencia" runat="server" />
                        <table style="width:100%;">
              
              <tr>
                  <td align="left" colspan="2">
                   
                      <anthem:GridView ID="gvTipoMuestra" runat="server" Font-Bold="True" Font-Size="9pt" 
                           ShowFooter="True" Width="100%"
                            BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" 
                          CellPadding="1" onrowdatabound="gvTipoMuestra_RowDataBound" AutoGenerateColumns="False">
                          <Columns>
                              <asp:BoundField DataField="Tipo Muestra" HeaderText="Tipo Muestra" />
                                            <asp:BoundField DataField="cantidad" 
                                  HeaderText="Cantidad de Casos" >
                                            <ItemStyle BackColor="#EEEEEE" />
                              </asp:BoundField>
                          </Columns>
                             <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                          <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                             <RowStyle BorderColor="#CCCCCC" BorderStyle="Double" BorderWidth="1px" 
                                 ForeColor="#333333" />

                      </anthem:GridView>
                      
                         </td>
                         </tr>
                            <tr>
                                <td align="right"   colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left"  >
                                <asp:ImageButton ToolTip="Ver grafico de tortas" Visible="false" ID="btnVerGraficoTipoMuestra"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_torta.png"  OnClientClick="verGrafico('torta'); return false;"                       />
                                 &nbsp;&nbsp;<asp:ImageButton ToolTip="Ver grafico de barras" Visible="false" ID="btnVerGraficoTipoMuestra2"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_barra.png"  OnClientClick="verGrafico('barra'); return false;"                       />
                                 <%--<asp:Button ID="btnVerGraficoTipoMuestra"  runat="server" Text="Ver Grafico" CssClass="myButtonGris" Width="100px"  
                       OnClientClick="verGrafico(); return false;"                       />
                                    <asp:Button ID="btnGraficoTipMuestra" runat="server" 
                                        onclick="btnGraficoTipMuestra_Click" Text="Ver Gráfico" />--%>
                              
                                </td>
                                <td align="right"  >
                                          &nbsp;</td>
                            </tr>
                           
                         </table>
                    </div>
<div  id="tab1" >   
                        <table style="width:100%;">
              
              <tr>
                  <td align="left">
                      &nbsp;</td>
                         </tr>
                         
                            <tr>
                                <td align="left">
                                    <anthem:GridView ID="gvSolicitante" runat="server" AutoGenerateColumns="False" BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" CellPadding="1" Font-Bold="True" Font-Size="9pt" ShowFooter="True" Width="100%" OnRowDataBound="gvSolicitante_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="origen" HeaderText="Origen" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad de Casos">
                                            <ItemStyle BackColor="#EEEEEE" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <RowStyle BorderColor="#CCCCCC" BorderStyle="Double" BorderWidth="1px" ForeColor="#333333" />
                                    </anthem:GridView>
&nbsp;</td>
                            </tr>
                         </table>
                    </div>
                     <div  id="tab2" >   
                        <table style="width:100%;">
              
              <tr>
              
                  <td align="left" colspan="2">
                      <table style="width:100%;" align="left">
                         
                          <tr>
                              <td  >
                                  Determinacion:</td>
                              <td  >
                                  <asp:DropDownList ID="ddlAnalisis" runat="server" class="form-control input-sm">
                                  </asp:DropDownList>
                              </td>
                              <td  >
                                  &nbsp;</td>
                              <td  >
                             
                                  <asp:Button ID="btnBuscarAislamiento" runat="server"  CssClass="btn btn-primary" Width="100px"
                                      onclick="btnBuscarAislamiento_Click" Text="Buscar" />
                              </td>
                          </tr>
                      </table>
                    
                  </td>
                  </tr>
                  
                        
                            <tr>
                                <td align="left">
                                   
                                    &nbsp;</td>

                                <td align="right">
                                <asp:ImageButton ID="btnGraficoMicroorganismos" runat="server" ToolTip="Ver grafico de tortas" Visible="false"
                                        OnClientClick="verGraficoMicroorganismo('torta'); return false;" ImageUrl="~/App_Themes/default/images/ico_torta.png" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="btnGraficoMicroorganismos2" runat="server" ToolTip="Ver grafico de barras" Visible="false"
                                        OnClientClick="verGraficoMicroorganismo('barra'); return false;" ImageUrl="~/App_Themes/default/images/ico_barra.png" />
                                
                                    <%--<asp:Button ID="btnGraficoMicroorganismos" runat="server" CssClass="myButtonGris" Width="100px"  

                                        OnClientClick="verGraficoMicroorganismo(); return false;" Text="Ver Gráfico" />--%>
                                </td>

                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    
                                        <asp:GridView ID="gvResultado" runat="server" AutoGenerateColumns="False" BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" CellPadding="1" EmptyDataText="No se encontraron datos" Font-Bold="True" Font-Size="9pt" onrowcommand="gvResultado_RowCommand" onrowdatabound="gvResultado_RowDataBound" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="Determinacion" HeaderText="Determinacion" />
                                                <asp:BoundField DataField="Resultado" HeaderText="Resultado" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad de Casos">
                                                <ItemStyle BackColor="#EEEEEE" />
                                                </asp:BoundField>
                                            </Columns>
                                            <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" />
                                        </asp:GridView>
                                   
                                </td>
                            </tr>
                            <tr>
                                  <td align="left">
                                      &nbsp;</td>
                                  <td align="right">
                                      <asp:ImageButton ID="btnGraficoResistencia" runat="server"  ImageUrl="~/App_Themes/default/images/ico_barra.png"  Visible="false"
                                          OnClientClick="verGraficoResistencia(); return false;" />
                                  </td>
                            </tr>
                            <tr>
                                <td align="right"   colspan="2">
                                    <asp:ImageButton ID="imgExcel2" runat="server" ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel2_Click" ToolTip="Exportar a Excel Lista de Resultados" />
                                    Exportar a Excel
                                    </td>
                            </tr>
                            
                         </table>
                    </div>    


                    
                        

                     <div  id="tab3" >   
                        <table style="width:100%;">
              
              <tr>
              
                  <td align="left" colspan="2">
                      <table style="width:100%;" align="left">
                       

                          <tr>
                              <td  >
                                  Tipo de muestra:</td>
                              <td  >
                                  <asp:DropDownList ID="ddlTipoMuestra" class="form-control input-sm"  runat="server" DataValueField="idMuestra">
                                  </asp:DropDownList>
                              </td>
                              <td  >
                                   </td>
                              <td  >
                           

                                  <asp:Button ID="Button1" runat="server"  CssClass="btn btn-primary" Width="100px"
                                      onclick="btnBuscarAislamiento2_Click" Text="Buscar" />
                              </td>
                          </tr>
                          <tr>
                              <td  >&nbsp;</td>
                              <td  >&nbsp;</td>
                              <td  >&nbsp;</td>
                              <td  >&nbsp;</td>
                          </tr>
                      </table>
                    
                  </td>
                  </tr>
                 

                        
                           
                            <tr>
                                <td align="left" colspan="2">
                                  
                                        <asp:GridView ID="gvMicroorganismos" runat="server" 
                                            AutoGenerateColumns="False" BorderColor="#666666" BorderStyle="Double" 
                                            BorderWidth="1px" CellPadding="1" DataKeyNames="idGermen" 
                                            EmptyDataText="No se encontraron datos" Font-Bold="True" Font-Size="9pt" 
                                             Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="Microorganismo" HeaderText="Aislamiento" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad de Casos">
                                                <ItemStyle BackColor="#EEEEEE" />
                                                </asp:BoundField>
                                            </Columns>
                                             <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" />
                                        </asp:GridView>
                                  

                                </td>
                            </tr>
                          



                         </table>
                    </div>    


                    </div>  <%----tabContainer--%>
                     
      </asp:Panel>
         </div>
    </div>
           </div>
  
    <script src="../script/Resources/jquery.min.js" type="text/javascript"></script>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
    <script src="../script/Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">

    var valores = $("#<%= HFTipoMuestra.ClientID %>").val();
    var valoresMicroorganismo = $("#<%= HFMicroorganismo.ClientID %>").val();
    var valoresResistencia = $("#<%= HFResistencia.ClientID %>").val();
   

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
        $('<iframe src="Grafico.aspx?valores=' + valores + '&tipo=0&tipoGrafico=' + tipoGrafico + '" />').dialog({
            title: 'Gráfico Estadístico de Tipo de Muestras',
            autoOpen: true,
            width: 900,
            height:500,
            modal: true,
            resizable: false,
            autoResize: true,
            overlay: {
                opacity: 0.5,
                background: "black"
            }
        }).width(900);
    }


    function verGraficoMicroorganismo(tipoGrafico) {
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
        $('<iframe src="Grafico.aspx?valores=' + valoresMicroorganismo + '&tipo=1&tipoGrafico=' + tipoGrafico + '" />').dialog({
            title: 'Gráfico Estadístico de Aislamientos',
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


    function verGraficoResistencia() {
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
        $('<iframe src="Grafico.aspx?valores=' + valoresResistencia + '&tipo=2" />').dialog({
            title: 'Resistencia en ATB',
            autoOpen: true,
            width:900,
            height: 600,
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

  
    </div>
    </div>
</asp:Content>