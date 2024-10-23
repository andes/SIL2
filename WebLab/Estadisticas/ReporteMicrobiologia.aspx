<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReporteMicrobiologia.aspx.cs" Inherits="WebLab.Estadisticas.ReporteMicrobiologia" MasterPageFile="~/Site1.Master" ValidateRequest="false"%>
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
    <style type="text/css">
        .style6
        {
            width: 88%;
        }
        .style9
        {
            width: 89%;
        }
            
.myTexto
{
	border: 1px solid #808080; /*font-weight: bold;*/
	
	background-color: #FCFCFC;

}



        .style10
        {
            width: 1059px;
        }
        .style12
        {
            width: 390px;
        }
        .style13
        {
            width: 415px;
        }
        .style14
        {
        }



    </style>
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
 <asp:HiddenField runat="server" ID="HFCurrTabIndex"  /> 
 

       <div align="left" style="width: 1150px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h4 > REPORTE ESTADISTICO MICROBIOLOGIA DE PACIENTES</h4>
                        </div>

				<div class="panel-body">

    <table align="center" class="style10" width="1000px" >
           <tr>
						<td class="myLabelIzquierda" >Efector:</td>
						<td >
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                            </td>
					</tr>
       
<tr>
<td class="myLabelIzquierda"  align="left"> 
  
 
    Análisis:&nbsp; </td>
<td align="left" colspan="4"> 
  
 
                            <asp:DropDownList ID="ddlAnalisis" class="form-control input-sm"  runat="server">
                             
                            </asp:DropDownList>
                                        
                                            <asp:RangeValidator ID="rvAnalisis" runat="server" 
                                ControlToValidate="ddlAnalisis" ErrorMessage="RangeValidator" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">Seleccione 
                            una practica</asp:RangeValidator>
                                        
                                            </td>
</tr>

      
       
<tr>
<td class="myLabelIzquierda" align="left"> 
  
 
    Fecha Desde:</td>
<td class="style12"  align="left"> 
  
 
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de inicio"  /></td>
<td class="myLabelIzquierda"  align="left" colspan="2"> 
  
 
    Fecha Hasta:</td>
<td class="style13"  align="left"> 
  
 

                            <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm" 
                                style="width: 100px" title="Ingrese la fecha de fin"  /></td>
</tr>

      
       
<tr>
<td class="myLabelIzquierda" align="left" 
        style="vertical-align: top;"> 
  
 
    Origen:</td>
<td class="myLabelIzquierda"  align="left" colspan="4"> 
  
 
                            <asp:CheckBoxList ID="ChckOrigen" runat="server" 
        RepeatDirection="Horizontal" RepeatColumns="5">
                            </asp:CheckBoxList>
    </td>
</tr>

      
       
<tr>
<td class="style14" align="right" colspan="5"> 
    
                                        
                                   
                                        
                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="130px"
                                onclick="btnGenerar_Click" Text="Generar Reporte"  
                                ValidationGroup="0" />
    
                                        
                                   
                                        
                                        </td>
</tr>

     </table>
                    </div>
                   
                    <div class="panel-footer">	
                  
  
      <asp:Panel ID="pnlResultado" runat="server" Visible="False">
      						    

        <div id="tabContainer">  
                           
                             <ul>
    <li><a href="#tab1"><b>Tipo de muestras</b></a></li>  
    <li><a href="#tab4"><b>Resultados</b></a></li>                              
    <li><a href="#tab2"><b>Aislamientos</b></a></li>  
    <li><a href="#tab3"><b>Antibióticos</b></a></li>  
    
</ul>                          


          <table style="width:100%;">
              <tr>
                  <td>
                      <asp:Label ID="lblAnalisis" runat="server" CssClass="mytituloGris" 
                          Font-Bold="True" Text="Label"></asp:Label>
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

              <div  id="tab1" >   
                  <asp:HiddenField ID="HFTipoMuestra" runat="server" />
                  <asp:HiddenField ID="HFMicroorganismo" runat="server" />
                  <asp:HiddenField ID="HFResistencia" runat="server" />
                        <table style="width:100%;">
              
              <tr>
                  <td align="left" colspan="2">
                   
                      <anthem:GridView ID="gvTipoMuestra" runat="server" Font-Bold="True" Font-Size="9pt" 
                           ShowFooter="True" Width="100%" DataKeyNames="idMuestra"
                            BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" 
                          CellPadding="1" onrowdatabound="gvTipoMuestra_RowDataBound" 
                          EnableModelValidation="True" AutoGenerateColumns="False">
                          <Columns>
                              <asp:BoundField DataField="Tipo Muestra" HeaderText="Tipo Muestra" />
                                            <asp:BoundField DataField="cantidad" 
                                  HeaderText="Cantidad de Casos" >
                                            <ItemStyle BackColor="#EEEEEE" />
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
                                            <ItemStyle BackColor="#E6E6E6" />
                              </asp:BoundField>
                                            <asp:BoundField DataField="F" HeaderText="Fem." >
                                            <ItemStyle BackColor="#E6E6E6" />
                              </asp:BoundField>
                                <asp:BoundField DataField="E" HeaderText="Embar." >
                                            <ItemStyle BackColor="#E6E6E6" />
                              </asp:BoundField>
                                            <asp:BoundField DataField="I" HeaderText="S/D" >
                              <ItemStyle BackColor="#E6E6E6" />
                              </asp:BoundField>
                          </Columns>
                             <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#CC3300"  Font-Bold="True" ForeColor="White" />
                             <RowStyle BorderColor="#CCCCCC" BorderStyle="Double" BorderWidth="1px" 
                                 ForeColor="#333333" />

                      </anthem:GridView>
                      
                         </td>
                         </tr>
                            <tr>
                                <td align="right" class="myLabelIzquierda" colspan="2">
                                    Exportar a Excel
                                    <asp:ImageButton ID="imgExcel" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
                                        ToolTip="Exportar a Excel Lista de Tipo de Muestras" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="myLabelIzquierda">
                                <asp:ImageButton ToolTip="Ver grafico de tortas" ID="btnVerGraficoTipoMuestra"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_torta.png"  OnClientClick="verGrafico('torta'); return false;"                       />
                                 &nbsp;&nbsp;<asp:ImageButton ToolTip="Ver grafico de barras" ID="btnVerGraficoTipoMuestra2"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_barra.png"  OnClientClick="verGrafico('barra'); return false;"                       />
                                 <%--<asp:Button ID="btnVerGraficoTipoMuestra"  runat="server" Text="Ver Grafico" CssClass="myButtonGris" Width="100px"  
                       OnClientClick="verGrafico(); return false;"                       />
                                    <asp:Button ID="btnGraficoTipMuestra" runat="server" 
                                        onclick="btnGraficoTipMuestra_Click" Text="Ver Gráfico" />--%>
                              
                                </td>
                                <td align="right" class="myLabelIzquierda">
                                          Exportar lista de Pacientes
                                    <asp:ImageButton ID="imgExcelDetallePacientes" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" 
                                        onclick="imgExcelDetallePacientes_Click" 
                                        ToolTip="Exportar a Excel Lista de Pacientes" />&nbsp;</td>
                            </tr>
                           
                         </table>
                    </div>

                     <div  id="tab2" >   
                        <table style="width:100%;">
              
              <tr>
              
                  <td align="left" colspan="2">
                      <table style="width:100%;" align="left">
                          <tr>
                              <td class="mytituloGris" colspan="5">
                                  Filtros Adicionales</td>
                          </tr>
                          <tr>
                              <td class="myLabelIzquierda">
                                  Tipo de muestra:</td>
                              <td class="myLabelIzquierda">
                                  <asp:DropDownList ID="ddlTipoMuestra" class="form-control input-sm"  runat="server">
                                  </asp:DropDownList>
                              </td>
                              <td class="myLabelIzquierda">
                                  ATB:</td>
                              <td class="myLabelIzquierda">
                                  <asp:DropDownList ID="ddlATB" class="form-control input-sm"  runat="server">
                                      <asp:ListItem Selected="True">Todos</asp:ListItem>
                                      <asp:ListItem>Con ATB</asp:ListItem>
                                      <asp:ListItem>Sin ATB</asp:ListItem>
                                  </asp:DropDownList>
                                  <asp:Button ID="btnBuscarAislamiento" runat="server" 
                                      onclick="btnBuscarAislamiento_Click" Text="Buscar" />
                              </td>
                          </tr>
                      </table>
                    
                  </td>
                  </tr>
                  <tr>
                  <td colspan="2">  <hr /></td>

                  </tr>
                        
                            <tr>
                                <td align="left">
                                   
                                    <anthem:Label ID="lblFiltroMicroorganismo" runat="server" ForeColor="#3366CC"></anthem:Label>
                                </td>

                                <td align="right">
                                <asp:ImageButton ID="btnGraficoMicroorganismos" runat="server" ToolTip="Ver grafico de tortas"
                                        OnClientClick="verGraficoMicroorganismo('torta'); return false;" ImageUrl="~/App_Themes/default/images/ico_torta.png" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton ID="btnGraficoMicroorganismos2" runat="server" ToolTip="Ver grafico de barras"
                                        OnClientClick="verGraficoMicroorganismo('barra'); return false;" ImageUrl="~/App_Themes/default/images/ico_barra.png" />
                                
                                    <%--<asp:Button ID="btnGraficoMicroorganismos" runat="server" CssClass="myButtonGris" Width="100px"  

                                        OnClientClick="verGraficoMicroorganismo(); return false;" Text="Ver Gráfico" />--%>
                                </td>

                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <div align="left" 
                                        style="border: 1px solid #999999; overflow: scroll; overflow-x:hidden; height: 300px;">
                                        <asp:GridView ID="gvMicroorganismos" runat="server" 
                                            AutoGenerateColumns="False" BorderColor="#666666" BorderStyle="Double" 
                                            BorderWidth="1px" CellPadding="1" DataKeyNames="idGermen" 
                                            EmptyDataText="No se encontraron datos" Font-Bold="True" Font-Size="9pt" 
                                            onrowcommand="gvMicroorganismos_RowCommand" 
                                            onrowdatabound="gvMicroorganismos_RowDataBound" Visible="False" Width="100%">
                                            <Columns>
                                                <asp:BoundField DataField="Microorganismo" HeaderText="Aislamiento" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad de Casos">
                                                <ItemStyle BackColor="#EEEEEE" />
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
                                                <asp:BoundField DataField="M" HeaderText="Masc.">
                                                <ItemStyle BackColor="#E6E6E6" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="F" HeaderText="Fem.">
                                                <ItemStyle BackColor="#E6E6E6" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="E" HeaderText="Embar.">
                                                <ItemStyle BackColor="#E6E6E6" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="I" HeaderText="S/D">
                                                <ItemStyle BackColor="#E6E6E6" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="Editar" runat="server" CssClass="myLinkRojo" 
                                                            ForeColor="#990000" ommandName="Editar" Text="Ver Resistencia" />
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="myLink" Height="20px" HorizontalAlign="Center" 
                                                        Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                            <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" 
                                                ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#CC9900" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                  <td align="left">
                                      <asp:Label ID="lblFiltroMicroorganismoATB" runat="server" 
                                          ForeColor="#3366CC"></asp:Label>
                                </td>
                                  <td align="right">
                                      <asp:ImageButton ID="btnGraficoResistencia" runat="server"  ImageUrl="~/App_Themes/default/images/ico_barra.png"
                                          OnClientClick="verGraficoResistencia(); return false;" />
                                  </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                   
                                    <asp:GridView ID="gvMicroorganismosATB" runat="server" 
                                        AutoGenerateColumns="False" BorderColor="#666666" BorderStyle="Double" 
                                        BorderWidth="1px" CellPadding="1" 
                                        EmptyDataText="No se encontraron datos para el aislamiento seleccionado" 
                                        EnableModelValidation="True" Font-Bold="True" Font-Size="9pt" 
                                        onrowdatabound="gvMicroorganismosATB_RowDataBound" Visible="False" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Antibiotico" HeaderText="Antibiotico" />
                                            <asp:BoundField DataField="Resistente" HeaderText="Resistente">
                                            <ItemStyle Font-Bold="True" Font-Size="10pt" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Sensible" HeaderText="Sensible" />
                                            <asp:BoundField DataField="Intermedio" HeaderText="Intermedio" />
                                            <asp:BoundField DataField="No Probado" HeaderText="No Probado" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" 
                                            ForeColor="#333333" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="myLabelIzquierda" colspan="2">
                                    Exportar a Excel
                                    <asp:ImageButton ID="imgExcel0" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel0_Click" 
                                        ToolTip="Exportar a Excel Lista de Microorganismos" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="myLabelIzquierda" colspan="2">
                                    Exportar Detalle de Pacientes
                                    <asp:ImageButton ID="imgExcelDetallePacientesAislamientos" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" 
                                        onclick="imgExcelDetallePacientesAislamientos_Click" 
                                        ToolTip="Exportar a Excel Lista de Pacientes" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="myLabelIzquierda" colspan="2">
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                       <div style="border: 1px solid #C0C0C0">
                                    </div>
                                </td>
                            </tr>
                         </table>
                    </div>    

                       <div  id="tab3" >   
                        <table style="width:100%;">
              
                            <tr>
                                <td align="left" colspan="2">
                                    <table style="width:100%;">
                                        <tr>
                                            <td colspan="5">
                                                Filtros</td>
                                        </tr>
                                        <tr>
                                            <td class="myLabelIzquierda">
                                                Tipo de Muestra:</td>
                                            <td class="myLabelIzquierda">
                                                <asp:DropDownList ID="ddlTipoMuestraAntibioticos" class="form-control input-sm"  runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        
                                            <td class="myLabelIzquierda">
                                                Aislamiento:</td>
                                            <td class="myLabelIzquierda">
                                                <asp:DropDownList ID="ddlMicroorganismosAntibioticos" class="form-control input-sm"   Width="200px" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="myLabelIzquierda" >
                                                <asp:Button ID="btnBuscarAntibioticos" runat="server" Text="Buscar" 
                                                    onclick="btnBuscarAntibioticos_Click" />
                                                <asp:HiddenField ID="hdfidAntibiotico" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblFiltroAntibiotico" runat="server" ForeColor="#3366CC"></asp:Label>
                                </td>
                                <td align="right" class="myLabelIzquierda">
                                    Exportar a Excel
                                    <asp:ImageButton ID="imgExcel1" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel1_Click" 
                                        ToolTip="Exportar a Excel Lista de Antibiotico" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                 
                                    <asp:GridView ID="gvAntibiotico"  runat="server" AutoGenerateColumns="False" 
                                        BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" CellPadding="1" 
                                        DataKeyNames="idAntibiotico" EnableModelValidation="True" Font-Bold="True" 
                                        Font-Size="9pt" onrowcommand="gvAntibiotico_RowCommand" 
                                        onrowdatabound="gvAntibiotico_RowDataBound" Width="100%" 
                                        EmptyDataText="No se encontraron datos">
                                        <Columns>
                                            <asp:BoundField DataField="Antibiotico" HeaderText="Antibiotico" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                            <ItemStyle BackColor="#CCCCCC" />
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

                                            <asp:BoundField DataField="M" HeaderText="Masculino" >
                                            <ItemStyle BackColor="#CCCCCC" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="F" HeaderText="Femenino" >
                                            <ItemStyle BackColor="#CCCCCC" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="E" HeaderText="Embarazada" >
                                            <ItemStyle BackColor="#CCCCCC" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="I" HeaderText="S/D" >
                                            <ItemStyle BackColor="#CCCCCC" />
                                            </asp:BoundField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="Editar" runat="server" CssClass="myLinkRojo" 
                                                        ommandName="Editar" Text="Ver Resistencia" ForeColor="#990000" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="myLink" Height="20px" HorizontalAlign="Center" 
                                                    Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <RowStyle BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                                            ForeColor="#333333" />
                                    </asp:GridView>
                                 
                                    <br />
                                    <asp:Label ID="lblResistenciaAntibiotico" runat="server" CssClass="myLabelRojo"
                                        Visible="False"></asp:Label>
                                    <br />
                                 
                                    <asp:GridView ID="gvAntibioticoResistencia" runat="server" 
                                        BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" CellPadding="1" 
                                        EnableModelValidation="True" Font-Bold="True" Font-Size="9pt" 
                                        onrowdatabound="gvAntibioticoResistencia_RowDataBound" ShowFooter="True" 
                                        Width="100%" 
                                        EmptyDataText="No se encontraron datos">
                                        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="#333333" />
                                        <RowStyle BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" 
                                            ForeColor="#333333" />
                                    </asp:GridView>
                                 
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2" class="myLabelIzquierda">
                                    Exportar detalle de pacientes
                                    <asp:ImageButton ID="imgExcelDetalleAtb" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" 
                                        onclick="imgExcelDetalleAtb_Click" 
                                        ToolTip="Exportar a Excel Lista de Antibiotico" />
                                </td>
                            </tr>
                         </table>
                    </div> 

                    
                       <div  id="tab4" >   
                        <table style="width:100%;">
              
              <tr>
                  <td align="left">
                      &nbsp;</td>
                         </tr>
                         
                            <tr>
                                <td align="left">
                                Resultados Predefinidos
                                
                                    <asp:GridView ID="gvResultado" runat="server" AutoGenerateColumns="False" 
                                        BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" CellPadding="1" 
                                        Font-Bold="True" Font-Size="9pt" onrowcommand="gvResultado_RowCommand" 
                                        onrowdatabound="gvResultado_RowDataBound" Width="100%"  EmptyDataText="No se encontraron datos">
                                        <Columns>
                                            <asp:BoundField DataField="Determinacion" HeaderText="Determinacion" />
                                            <asp:BoundField DataField="Resultado" HeaderText="Resultado" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad de Casos">
                                            <ItemStyle BackColor="#EEEEEE" />
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
                                            <asp:BoundField DataField="M" HeaderText="Masc.">
                                            <ItemStyle BackColor="#E6E6E6" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="F" HeaderText="Fem.">
                                            <ItemStyle BackColor="#E6E6E6" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="E" HeaderText="Embar.">
                                            <ItemStyle BackColor="#E6E6E6" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="I" HeaderText="S/D">
                                            <ItemStyle BackColor="#E6E6E6" />
                                            </asp:BoundField>
                                        </Columns>
                                        <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BorderColor="#333333" BorderStyle="Solid" BorderWidth="1px" />
                                    </asp:GridView>
                                </td>
                            </tr>
                               <tr>
                                <td align="right" class="myLabelIzquierda">
                                    Exportar a Excel
                                    <asp:ImageButton ID="imgExcel2" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel2_Click" 
                                        ToolTip="Exportar a Excel Lista de Resultados" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="myLabelIzquierda">
                                    Exportar Detalle de Pacientes
                                    <asp:ImageButton ID="imgExcelResultadoPacientes" runat="server" 
                                        ImageUrl="~/App_Themes/default/images/excelPeq.gif" 
                                        onclick="imgExcelResultadoPacientes_Click1" 
                                        ToolTip="Exportar a Excel Lista de Resultados" />
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