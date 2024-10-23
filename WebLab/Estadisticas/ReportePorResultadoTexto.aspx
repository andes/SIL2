<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePorResultadoTexto.aspx.cs" Inherits="WebLab.Estadisticas.ReportePorResultadoTexto" MasterPageFile="~/Site1.Master" %>
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
    
  
  
    
    
    <style type="text/css">
        .auto-style1 {
            height: 33px;
        }
    </style>
    
  
  
    
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
    <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
  


    <div align="left" style="width: 1150px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h4 >Estadísticas por Resultados de Texto Libre<asp:HiddenField ID="HFTipoMuestra" runat="server" /></h4>
                        </div>

				<div class="panel-body">


    <table  >

         <tr>
						<td >Efector:</td>
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
  
 
    Determinación: </td>
<td class="style3"  align="left"> 
  
 
                            <asp:DropDownList ID="ddlAnalisis" runat="server" class="form-control input-sm" Width="250px">
                              
                            </asp:DropDownList>
                                        
                                            <%--<asp:RangeValidator ID="rvAnalisis" runat="server" 
                                ControlToValidate="ddlAnalisis" ErrorMessage="RangeValidator" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">Seleccione 
una determinación</asp:RangeValidator>--%>
                                        
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
      
       
<%--<tr>
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
</tr>--%>

      
       
<tr>
<td class="auto-style1" align="left"> 
  
 
    Fecha Desde:</td>
<td class="auto-style1"  align="left"> 
  
 
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
</tr>--%>

      
       
<%--       
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
</tr>--%>



      
    <%--   
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
</tr>--%>



      
<%--       
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
</tr>--%>



      
       
<tr>
<td class="auto-style2" align="left"> 
  
 
    Texto a buscar:</td>
<td class="style3"  align="left"> 
  
 
    <asp:TextBox ID="texto" runat="server" MaxLength="400" class="form-control input-sm" Width="400px"></asp:TextBox>
    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="texto" ErrorMessage="Debe ingresar un texto de busqueda" ValidationGroup="0"></asp:RequiredFieldValidator>
    
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

        <div class="panel-footer">	<asp:Panel ID="pnlResultado" runat="server" Visible="true">
          <table style="width:100%;">
              <tr>
                     <td align="right">
                    <%-- <asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" 
            ToolTip="Exportar a Pdf" onclick="imgPdf_Click" />--%>
&nbsp;
        &nbsp;
        <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" /></td>
              </tr>
             
              <tr>
                  <td>
                      <asp:Label ID="lblAnalisis" runat="server" CssClass="mytituloGris" 
                          Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                          <div align="left" style="border: 1px solid #999999; overflow: scroll; overflow-x:hidden; height: 500px; background-color: #F7F7F7;">
                      <asp:GridView ID="gvEstadistica" runat="server" CssClass="table table-bordered bs-table">
                      </asp:GridView>
                              </div>
                  </td>
               </tr>
           
             
          </table>
      </asp:Panel>
                   <%--   <br />
                        <asp:ImageButton ToolTip="Ver grafico de tortas" ID="btnVerGraficoTipoMuestra"  runat="server"  ImageUrl="~/App_Themes/default/images/ico_torta.png"  OnClientClick="verGrafico('torta'); return false;"                       />
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