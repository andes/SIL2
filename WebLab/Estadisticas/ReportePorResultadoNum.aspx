<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePorResultadoNum.aspx.cs" Inherits="WebLab.Estadisticas.ReportePorResultadoNum" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
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

    
    <div align="left" style="width: 1050px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h4 >REPORTE ESTADISTICO POR RESULTADOS NUMERICOS</h4>
                        </div>

				<div class="panel-body">
<table align="center" width="1000px">

      <tr>
						<td class="myLabelIzquierda" >Efector:</td>
						<td >
                            <asp:DropDownList ID="ddlEfector" runat="server" 
                                ToolTip="Seleccione el efector" TabIndex="9" Width="250px" class="form-control input-sm">
                            </asp:DropDownList>
                                        
                                            </td>
					</tr>

      
       
<tr>
<td    align="left" class="myLabelIzquierda"> 
  
 
    Análisis:&nbsp; </td>
<td align="left"  colspan="4"> 
  
 
                            <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True"  class="form-control input-sm" Width="200px"
                                onselectedindexchanged="ddlArea_SelectedIndexChanged">
                            </asp:DropDownList>
                                        
 
                            <asp:DropDownList ID="ddlAnalisis" runat="server" class="form-control input-sm"  Width="350px">
                                <asp:ListItem Selected="True">Chagas</asp:ListItem>
                                <asp:ListItem>HIV</asp:ListItem>
                                <asp:ListItem Value="Toxo">Toxoplasmosis</asp:ListItem>
                                <asp:ListItem>VDRL</asp:ListItem>
                            </asp:DropDownList>
                                        
                                            <asp:RangeValidator ID="rvAnalisis" runat="server" 
                                ControlToValidate="ddlAnalisis" ErrorMessage="RangeValidator" 
                                MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">Seleccione análisis</asp:RangeValidator>
                                        
                                            </td>
</tr>

      
       
<tr>
<td   align="left" class="myLabelIzquierda"> 
  
 
    Fecha Desde:</td>
<td align="left"   colspan="5"> 
  
 
                         <table><tr><td> 
  
 
  <input id="txtFechaDesde" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="3" class="form-control input-sm"
                                style="width: 90px" title="Ingrese la fecha de inicio"  /></td><td class="myLabelIzquierda">
                                 &nbsp;</td><td class="myLabelIzquierda">Fecha Hasta:</td><td><input id="txtFechaHasta" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="4" class="form-control input-sm"
                                style="width: 90px" title="Ingrese la fecha de fin"  /><asp:CustomValidator ID="cvFechas" runat="server" 
                                ErrorMessage="Fechas de inicio y de fin" 
                                onservervalidate="cvFechas_ServerValidate" ValidationGroup="0">Formato inválido de fecha</asp:CustomValidator>
                                        
                                            </td></tr></table></td>
</tr>

      
       
<tr>
<td   align="left" class="myLabelIzquierda"> 
  
 
    Valor Desde:</td>
<td  align="left" colspan="2"> 
  
 
   <table><tr><td> 
  
 

                            <asp:TextBox ID="txtValorDesde" class="form-control input-sm" runat="server" Width="80px"></asp:TextBox>
                           
                           
                            </td><td class="myLabelIzquierda">&nbsp;</td><td class="myLabelIzquierda">Valor Hasta:</td><td> 
  
 

                            <asp:TextBox ID="txtValorHasta" class="form-control input-sm" runat="server" Width="80px"></asp:TextBox>
                           
                           
                            </td></tr></table></td>
<td   align="left"> 
  
 
                            &nbsp;</td>
<td    align="left"> 
  
 
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="txtValorDesde" 
                                ErrorMessage="Debe ingresar solo valores numericos" 
                                ValidationExpression="^[0-9]{1,5}(\.[0-9]{0,2})?$" ValidationGroup="0"></asp:RegularExpressionValidator>
  
 
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                ControlToValidate="txtValorHasta" 
                                ErrorMessage="Debe ingresar solo valores numericos" 
                                ValidationExpression="^[0-9]{1,5}(\.[0-9]{0,2})?$" ValidationGroup="0"></asp:RegularExpressionValidator>
                                    </td>
<td align="left"  > 
  
 

                            &nbsp;</td>
</tr>

      
       
<tr>
<td class="myLabelGris" align="left" colspan="5"> 
  
 
    Si desea consultar la cantidad de casos independientemente del resultado 
    obtenido no ingrese Valor Desde y Valor Hasta.</td>
<td align="left"  > 
  
 

                           &nbsp;</td>
</tr>



      
       
<tr>
<td  align="left" class="myLabelIzquierda"> 
  
 
    Grupo Etáreo:</td>
<td align="left"   colspan="4"> 
  
 

                            <asp:DropDownList ID="ddlGrupoEtareo" runat="server"  class="form-control input-sm">
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
                                    <asp:ListItem Value="11">45-54 años</asp:ListItem>
         <asp:ListItem Value="12">55-64 años</asp:ListItem>
         <asp:ListItem Value="13">65-74 años</asp:ListItem>
                                
                                <asp:ListItem Value="10">&gt; 75 años</asp:ListItem>
                            </asp:DropDownList>
    </td>
<td align="left"  > 
  
 

                           &nbsp;</td>
</tr>

      
       
<tr>
<td  align="left" class="myLabelIzquierda"> 
  
 
    Sexo:</td>
<td align="left"  > 
  
 

                            <asp:DropDownList ID="ddlSexo" runat="server"  class="form-control input-sm">
                                <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Femenino</asp:ListItem>
                                <asp:ListItem Value="2">Masculino</asp:ListItem>
                            </asp:DropDownList>   
  
 

                           <asp:RadioButtonList ID="rdbPaciente" runat="server" 
                                RepeatDirection="Horizontal" CssClass="myLabelIzquierda">
                                <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Solo Embarazadas</asp:ListItem>
                            </asp:RadioButtonList>
    </td>
<td align="right"   colspan="3"> 
  
 

                           <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary" Width="130px"
                                onclick="btnGenerar_Click" Text="Generar Reporte"  
                                ValidationGroup="0" />
    </td>
<td align="left"  > 
  
 

                           &nbsp;</td>
</tr>

      
       

</table>
    <br />
     
                    </div>
     
        
        <div class="panel-footer">
            <asp:Panel ID="pnlResultado" runat="server" Visible="False">
          <table style="width:100%;">
              <tr>
                  <td>
                      <asp:Label CssClass="mytituloGris" ID="lblAnalisis" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                  </td>
                  <td align="right">
                     <asp:ImageButton ID="imgPdf" runat="server" 
            ImageUrl="~/App_Themes/default/images/pdf.jpg" 
            ToolTip="Exportar a Pdf" onclick="imgPdf_Click" style="height: 20px" />
&nbsp;
        &nbsp;
        <asp:ImageButton ID="imgExcel" runat="server" 
            ImageUrl="~/App_Themes/default/images/excelPeq.gif" onclick="imgExcel_Click" 
            ToolTip="Exportar a Excel" /></td>
              </tr>
              <tr>
                  <td colspan="2">
                      <asp:GridView ID="gvEstadistica" runat="server" Font-Bold="True" Font-Size="10pt" 
                          onrowcommand="gvEstadistica_RowCommand"   CssClass="table table-bordered bs-table"
                          onrowdatabound="gvEstadistica_RowDataBound" ShowFooter="True" 
                          BorderColor="#666666" BorderStyle="Double" BorderWidth="1px" 
                          AutoGenerateColumns="False">
                          <Columns>
                              <asp:BoundField DataField="Sexo" HeaderText="Sexo" >
                              <ItemStyle Width="10%" />
                              </asp:BoundField>
                              <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" >
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
                                <asp:BoundField DataField="45 a 64" HeaderText="55 a 64" Visible="false" >
                                  <FooterStyle BackColor="#CC3300" />
                                  <HeaderStyle BackColor="#CC3300" />
                                  <ItemStyle Width="5%"  />
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
                                  <asp:TemplateField HeaderText="Pac. PDF">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesPDF" runat="server" CommandName="PacientesPDF" 
                                          ImageUrl="../App_Themes/default/images/pdf.jpg" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Pac. EXCEL">
                                  <ItemTemplate>
                                      <asp:ImageButton ID="PacientesEXCEL" runat="server" CommandName="PacientesEXCEL" 
                                          ImageUrl="../App_Themes/default/images/excelPeq.gif" />
                                  </ItemTemplate>
                                  <ItemStyle Height="20px" HorizontalAlign="Center" Width="5%" />
                              </asp:TemplateField>
                          </Columns>
                          <FooterStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                          <HeaderStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                      </asp:GridView>
                  </td>
              </tr>
           
          </table>
      </asp:Panel>
            </div>
         </div>
        </div>
   
    </asp:Content>