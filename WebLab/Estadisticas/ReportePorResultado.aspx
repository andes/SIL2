<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePorResultado.aspx.cs" Inherits="WebLab.Estadisticas.ReportePorResultado" MasterPageFile="~/Site1.Master" %>
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
 
    <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
  


    <div align="left" style="width: 1150px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h4 >Estadísticas por Resultados Predefinidos<asp:HiddenField ID="HFTipoMuestra" runat="server" /></h4>
                        </div>

				<div class="panel-body">


    <table align="center" >

         <tr>
						<td class="myLabelIzquierda" >Efector:</td>
						<td >
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                            </td>
					</tr>

       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Servicio:</td>
<td class="style3"  align="left"> 
  
 
                              <asp:dropdownlist ID="ddlServicio" runat="server"  
                                ToolTip="Seleccione el servicio" TabIndex="1" class="form-control input-sm" 
                                AutoCallBack="True" onselectedindexchanged="ddlServicio_SelectedIndexChanged" AutoPostBack="True">
                            </asp:dropdownlist></td>
<td class="auto-style3"  align="left" rowspan="8"> 
  
 
      <div  style="width: 320px" class="form-inline"  >
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h4 class="panel-title">Diagnósticos<asp:RequiredFieldValidator ID="rfvDiag" runat="server" ControlToValidate="lstDiag" ErrorMessage="*" Font-Size="8pt" ValidationGroup="0">Seleccione al menos uno</asp:RequiredFieldValidator>
                                    </h4>
                                </div>
                                <div class="panel-body">
                                    <asp:ListBox ID="lstDiag" runat="server" class="form-control input-sm" Height="160px" SelectionMode="Multiple" TabIndex="12" ToolTip="Seleccione los diagnósticos" Width="300px"></asp:ListBox>
                                    <asp:CheckBox ID="chkSinDiag" runat="server" Checked="true" Text="Incluir Sin Diagnostico" />
                                </div>
                                <div class="panel-footer">
                                    <ul class="pagination">
                                        <li>
                                            <asp:LinkButton ID="lnkMarcarSectores" runat="server"  OnClick="lnkMarcarSectores_Click">Todas</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkDesmarcarSectores" runat="server"   OnClick="lnkDesmarcarSectores_Click">Ninguna</asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </div>
          </div>
    </td>
<td class="auto-style3"  align="left" rowspan="8"> 
  
 
      <div id="Div3" runat="server" class="panel panel-default">
          <div class="panel-heading">
         <h4 class="panel-title">    Sector/Servicio<asp:RequiredFieldValidator ID="rfvSector" runat="server" ControlToValidate="lstSector" ErrorMessage="*" Font-Size="8pt" ValidationGroup="0">Seleccione al menos uno</asp:RequiredFieldValidator>
              </h4>
          </div>
          <div class="panel-body">
              <asp:ListBox ID="lstSector" runat="server" class="form-control input-sm" Height="160px" SelectionMode="Multiple" TabIndex="12" ToolTip="Seleccione los sectores" Width="300px"></asp:ListBox>
              </div>
              <div class="panel-footer">
              <ul class="pagination">
                  <li>
                      <asp:LinkButton ID="lnkMarcarSector" runat="server" OnClick="lnkMarcarSector_Click">Todos&nbsp;&nbsp;</asp:LinkButton>
                  </li>
                  <li>
                      <asp:LinkButton ID="lnkDesmarcarSector" runat="server" OnClick="lnkDesmarcarSector_Click">Ninguno</asp:LinkButton>
                  </li>
              </ul>
          </div>
      </div>
    </td>
</tr>

      
       
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Area:</td>
<td class="style3"  align="left"> 
  
 
                            <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" Width="250px"
                                onselectedindexchanged="ddlArea_SelectedIndexChanged" class="form-control input-sm">
                            </asp:DropDownList>
                                        
 
                                            </td>
</tr>

      
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Determinación: (*)</td>
<td class="style3"  align="left"> 
  
 
                            <asp:DropDownList ID="ddlAnalisis" runat="server" class="form-control input-sm" Width="250px">
                              
                            </asp:DropDownList>
                                        
                                            <asp:RangeValidator ID="rvAnalisis" runat="server" 
                                ControlToValidate="ddlAnalisis" ErrorMessage="RangeValidator" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">Seleccione 
una determinación</asp:RangeValidator>
                                        
                                            </td>
</tr>

      
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Fecha Desde:</td>
<td class="style3"  align="left"> 
  
 
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de inicio"  /></td>
</tr>

      
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Fecha Hasta:</td>
<td class="style3"  align="left"> 
  
 
    <input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 100px" title="Ingrese la fecha de fin"  /><asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Formato inválido de fechas " 
                                onservervalidate="cvFechas_ServerValidate" ValidationGroup="0">Formato inválido de fechas </asp:CustomValidator>
    
                                    </td>
</tr>



   <%--   
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Grupo Etáreo:</td>
<td class="style3"  align="left"> 
  
 

                            
                                   
                                    </td>
</tr>



      
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Sexo:</td>
<td class="style3"  align="left"> 
  
 

                            
                                    </td>
</tr>
--%>


      
       
<tr>
<td class="auto-style2" align="left" valign="top"> 
  
 
    Origen:</td>
<td    align="left"> 
  
 
    <asp:CheckBoxList ID="ChckOrigen" runat="server" RepeatColumns="3">
    </asp:CheckBoxList>
    <div style="font-size: 9px; font-family: Verdana">
        Seleccionar:
        <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLink" Font-Names="Verdana" Font-Size="8pt" onclick="lnkMarcar_Click">Todas</asp:LinkButton>
        &nbsp;
        <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="myLink" Font-Names="Verdana" Font-Size="8pt" onclick="lnkDesmarcar_Click">Ninguna</asp:LinkButton>
    </div>
                                    </td>
</tr>



      
       
<tr>
<td class="auto-style2" align="left" valign="top"> 
  
 
    Grupo Etáreo: </td>
<td    align="left"> 
  
 
     <asp:DropDownList ID="ddlGrupoEtareo"  runat="server" class="form-control input-sm">
                                <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1"><6 meses</asp:ListItem>
         <asp:ListItem Value="2">6 y 11 meses</asp:ListItem>
                                <asp:ListItem Value="3">1 a 2 años</asp:ListItem>
                                <asp:ListItem Value="4">2 a 4 años</asp:ListItem>
                                <asp:ListItem Value="5">5-9 años</asp:ListItem>
                                <asp:ListItem Value="6">10-14 años</asp:ListItem>
                                <asp:ListItem Value="7">15-19 años</asp:ListItem>
                                <asp:ListItem Value="8">20 a 24 años</asp:ListItem>
                                <asp:ListItem Value="9">25 a 34 años</asp:ListItem>     
         <asp:ListItem Value="10">35 a 44 años</asp:ListItem>     
                                    <asp:ListItem Value="11">45-64 años</asp:ListItem>
        
         <asp:ListItem Value="12">65-74 años</asp:ListItem>
                                
                                <asp:ListItem Value="13">&gt; 75 años</asp:ListItem>
                            </asp:DropDownList></td>
</tr>



      
       
<tr>
<td class="auto-style2" align="left" valign="top"> 
  
 
    Sexo:</td>
<td    align="left"> 
  
 
    <asp:DropDownList ID="ddlSexo"  runat="server" class="form-control input-sm">
                                <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Femenino</asp:ListItem>
                                <asp:ListItem Value="2">Masculino</asp:ListItem>

                            </asp:DropDownList>
                
       </td>
</tr>



      
       
<tr>
<td align="right" colspan="4"> 
  
 

                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="130px"
                                onclick="btnGenerar_Click" Text="Generar Reporte"  
                                ValidationGroup="0" Height="35px" /></td>
</tr>

      
       

      
       
</table> 

                    </div>

        <div class="panel-footer">	<asp:Panel ID="pnlResultado" runat="server" Visible="False">
          <table style="width:100%;">
              <tr>
                  <td>
                      <asp:Label ID="lblAnalisis" runat="server" CssClass="mytituloGris" 
                          Font-Bold="True" Text="Label"></asp:Label>
                  </td>
                  <td align="right">
                     <asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" 
            ToolTip="Exportar a Pdf" onclick="imgPdf_Click" />
&nbsp;
        &nbsp;
        <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" /></td>
              </tr>
           
             
          </table>
      </asp:Panel>
             <asp:GridView ID="gvEstadistica" runat="server" AutoGenerateColumns="False" 
                          DataKeyNames="idItem" Font-Bold="True" Font-Size="8pt" 
                          onrowcommand="gvEstadistica_RowCommand"    CssClass="table table-bordered bs-table"
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
                              <asp:BoundField DataField="CANTIDAD" HeaderText="CANTIDAD DE CASOS" >
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
                              <asp:BoundField DataField="I" HeaderText="Ind." >
                                  <FooterStyle BackColor="#336699" />
                                  <HeaderStyle BackColor="#336699" />
                                  <ItemStyle Width="5%" />
                              </asp:BoundField>
                              <asp:TemplateField HeaderText="Pac. PDF">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesPDF" runat="server" CommandName="PacientesPDF" 
                                          ImageUrl="~/App_Themes/default/images/pdf.jpg" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Pac. EXCEL">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesEXCEL" runat="server" CommandName="PacientesEXCEL" 
                                          ImageUrl="~/App_Themes/default/images/excelPeq.gif" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>
                      <br />
                        <asp:ImageButton ToolTip="Ver grafico de tortas" ID="btnVerGraficoTipoMuestra"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_torta.png"  OnClientClick="verGrafico('torta'); return false;"                       />
                                 &nbsp;&nbsp;<asp:ImageButton ToolTip="Ver grafico de barras" ID="btnVerGraficoTipoMuestra2"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_barra.png"  OnClientClick="verGrafico('barra'); return false;"                       />

        
          
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