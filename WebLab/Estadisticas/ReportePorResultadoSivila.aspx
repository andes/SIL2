<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePorResultadoSivila.aspx.cs" Inherits="WebLab.Estadisticas.ReportePorResultadoSivila" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  



  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
    
    <style type="text/css">
        .auto-style1 {
            width: 121px;
        }
        .auto-style2 {
            width: 153px;
        }
        .auto-style3 {
            width: 1000px;
        }
        .auto-style4 {
            border-style: none;
            font-size: 11pt;
            font-family: arial;
            background-color: #FFFFFF;
            color: #2B7EBD;
            vertical-align: top;
            font-weight: bold;
            text-transform: uppercase;
            width: 68px;
        }
        .auto-style5 {
            border-style: none;
            font-size: 11pt;
            font-family: arial;
            background-color: #FFFFFF;
            color: #2B7EBD;
            vertical-align: top;
            font-weight: bold;
            text-transform: uppercase;
            width: 504px;
        }
        .auto-style6 {
            border-style: none;
            font-size: 11pt;
            font-family: arial;
            background-color: #FFFFFF;
            color: #2B7EBD;
            vertical-align: top;
            font-weight: bold;
            text-transform: uppercase;
            width: 414px;
        }
        .auto-style8 {
        }
    </style>
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
        <br />&nbsp;&nbsp;<br />
         
    <div>
      <table align="center" width="1000px" class="auto-style3">

<tr>
<td class="auto-style4"> 
   <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/default/images/SNVS.PNG" /></td>
<td class="auto-style5"> 
    <br />
    GENERACION DE DATOS PARA 
    <br />
    SISTEMA NACIONAL DE VIGILANCIA 
    <br />
    LABORATORIAL
</td>
<td class="auto-style6" align="right"    > 
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/default/images/Sivila.PNG" />
            </td>
</tr>

<tr>
<td class="auto-style4"> 
    &nbsp;</td>
<td class="auto-style5"> 
    <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="8pt" NavigateUrl="~/Estadisticas/2015_calendario_epidemiologico.pdf" Target="_blank">Calendario 2015</asp:HyperLink>
&nbsp;<asp:HyperLink ID="HyperLink2" runat="server" Font-Size="8pt" NavigateUrl="~/Estadisticas/2016-calendario-epidemiologico.pdf" Target="_blank">Calendario 2016</asp:HyperLink>
</td>
<td class="auto-style6" align="right"    > 
            &nbsp;</td>
</tr>
<tr>
<td class="auto-style8" colspan="3"> <hr class="hrTitulo" />
    </td>
</tr>

      
       
<tr>
<td align="left" colspan="3"> 
  
 
    <table class="mytable1" style="font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: 10px; color: #000000">
        <tr>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Codigo Grupo</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Nombre Grupo</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Codigo SubGrupo</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold" class="auto-style1">Nombre SubGrupo</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Codigo Etiologia</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold" class="auto-style2">Nombre Etiologia</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Año</td>
            <td bgcolor="#99CCFF" style="font-weight: bold">Semana Epidemiologica</td>
            <td bgcolor="#99CCFF" style="font-weight: bold">Codigos SIL</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Ultimos Registros Generados</td>
            <td rowspan="2" bgcolor="#99CCFF" style="font-weight: bold">Generar Archivo</td>
        </tr>
        <tr>
            <td style="background-color: #E6E6E6">(separar por comas, rango separado por guiones)</td>
            <td style="background-color: #E6E6E6">(separar por comas)</td>
        </tr>
        <tr>
            <td>4</td>
            <td>VIH - Hombres</td>
            <td>-</td>
            <td class="auto-style1">-</td>
            <td>1353</td>
            <td class="auto-style2">VIH - Laboratorio General</td>
            <td>
                <asp:DropDownList ID="ddlAnio4" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>                    
                </asp:DropDownList></td>
            <td>
                <asp:TextBox  ID="txtSemana_4_1353" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox  ID="txtCodigo_4_1353" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
                <asp:Label ID="lblCantidad_4_1353" runat="server"></asp:Label>
            </td>
            <td>
                <asp:Button ID="btnDescargar4" runat="server" Text="Descargar" OnClick="btnDescargar4_Click" CssClass="myButton" /></td>
        </tr>
        <tr>
            <td>14</td>
            <td>VIH-Mujeres</td>
            <td>-</td>
            <td class="auto-style1">-</td>
            <td>1353</td>
            <td class="auto-style2">VIH - Laboratorio General</td>
            <td>
                <asp:DropDownList ID="ddlAnio14" runat="server">
                     <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_14_1353" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_14_1353" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
               <asp:Label ID="lblCantidad_14_1353" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar14" runat="server" Text="Descargar" OnClick="btnDescargar14_Click" CssClass="myButton" /></td>
        </tr>
        <tr>
            <td rowspan="16">18</td>
            <td rowspan="16">Embarazadas</td>
            <td rowspan="16">6</td>
            <td rowspan="16" class="auto-style1">Controles de Embarazos no especificados</td>
            <td>1858</td>
            <td class="auto-style2">Rubéola IgG (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1858" runat="server">
                   <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1858" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1858" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
              <asp:Label ID="lblCantidad_18_1858" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1858" runat="server" Text="Descargar" OnClick="btnDescargar1858_Click" CssClass="myButton"/></td>
        </tr>
        <tr>
            <td>696</td>
            <td class="auto-style2">Estreptococo beta hemolítico (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1859" runat="server">
                   <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1859" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1859" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
              <asp:Label ID="lblCantidad_18_1859" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1859" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1859_Click"/></td>
        </tr>
        <tr>
            <td>1422</td>
            <td class="auto-style2">Embarazadas estudiadas</td>
            <td>
                <asp:DropDownList ID="ddlAnio1422" runat="server">
                     <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1422" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1422" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
                      <asp:Label ID="lblCantidad_18_1422" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1422" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1422_Click"/></td>
        </tr>
        <tr>
            <td>1909</td>
            <td class="auto-style2">Chagas confirmado (estudiado por 2 técnicas) (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1909" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1909" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1909" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
              <asp:Label ID="lblCantidad_18_1909" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1909" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1909_Click" /></td>
        </tr>
        <tr>
            <td>1927</td>
            <td class="auto-style2">Sífilis (diagnóstico) por pruebas NO Treponémicas - caso probable (E) </td>
            <td>
                <asp:DropDownList ID="ddlAnio1927" runat="server">
                 <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1927" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1927" runat="server" CssClass="myTexto"></asp:TextBox></td>
             <td>
              <asp:Label ID="lblCantidad_18_1927" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1927" runat="server" Text="Descargar"  CssClass="myButton" OnClick="btnDescargar1927_Click"/></td>
        </tr>
        <tr>
            <td>1928</td>
            <td class="auto-style2">Sífilis (diagnóstico) por pruebas Treponémicas - caso confirmado (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1928" runat="server">
                <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1928" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1928" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
              <asp:Label ID="lblCantidad_18_1928" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1928" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1928_Click"/></td>
        </tr>
        <tr>
            <td>1929</td>
            <td class="auto-style2">VIH-pruebas confirmatorias durante el embarazo (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1929" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1929" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1929" runat="server" CssClass="myTexto"></asp:TextBox></td>
           <td>
              <asp:Label ID="lblCantidad_18_1929" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1929" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1929_Click"/></td>
        </tr>
        <tr>
            <td>1930</td>
            <td class="auto-style2">Hepatitis B probable - HBsAg (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1930" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1930" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1930" runat="server" CssClass="myTexto"></asp:TextBox></td>
          <td>
              <asp:Label ID="lblCantidad_18_1930" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1930" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1930_Click" /></td>
        </tr>
        <tr>
            <td>1931</td>
            <td class="auto-style2">Toxoplasmosis (IgG) (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1931" runat="server">
                  <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1931" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1931" runat="server" CssClass="myTexto"></asp:TextBox></td>
           <td>
              <asp:Label ID="lblCantidad_18_1931" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1931" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1931_Click" /></td>
        </tr>
        <tr>
            <td>1932</td>
            <td class="auto-style2">Toxoplasmosis (IgM) (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1932" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1932" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1932" runat="server" CssClass="myTexto"></asp:TextBox></td>
            <td>
              <asp:Label ID="lblCantidad_18_1932" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1932" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1932_Click" /></td>
        </tr>
        <tr>
            <td>1933</td>
            <td class="auto-style2">VIH-pruebas de tamizaje durante el embarazo (caso probable) (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1933" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1933" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1933" runat="server"></asp:TextBox></td>
               <td>
              <asp:Label ID="lblCantidad_18_1933" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1933" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1933_Click" /></td>
        </tr>
        <tr>
            <td>1934</td>
            <td class="auto-style2">VIH-test rápidos durante el embarazo (caso probable) (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1934" runat="server">
                  <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1934" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1934" runat="server"></asp:TextBox></td>
             <td>
              <asp:Label ID="lblCantidad_18_1934" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1934" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1934_Click" /></td>
        </tr>
        <tr>
            <td>1935</td>
            <td class="auto-style2">VIH - Pruebas de tamizaje durante el parto (caso probable)(E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1935" runat="server">
                  <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1935" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1935" runat="server"></asp:TextBox></td>
               <td>
              <asp:Label ID="lblCantidad_18_1935" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1935" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1935_Click" /></td>
        </tr>
        <tr>
            <td>1936</td>
            <td class="auto-style2">VIH - test rápidos durante el parto (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1936" runat="server">
                    <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1936" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1936" runat="server"></asp:TextBox></td>
             <td>
              <asp:Label ID="lblCantidad_18_1936" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1936" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1936_Click" /></td>
        </tr>
        <tr>
            <td>1937</td>
            <td class="auto-style2">VIH - pruebas confirmatorias durante el parto (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1937" runat="server">
                     <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtSemana_18_1937" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1937" runat="server"></asp:TextBox></td>
             <td>
              <asp:Label ID="lblCantidad_18_1937" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1937" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1937_Click" /></td>
        </tr>
        <tr>
            <td>1938</td>
            <td class="auto-style2">Brucelosis (E)</td>
            <td>
                <asp:DropDownList ID="ddlAnio1938" runat="server">
                     <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox CssClass="myTexto" ID="txtSemana_18_1938" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox ID="txtCodigo_18_1938" runat="server"></asp:TextBox></td>
               <td>
              <asp:Label ID="lblCantidad_18_1938" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1938" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1938_Click" /></td>
        </tr>
        <tr>
            <td>33</td>
            <td>Chagas</td>
            <td>-</td>
            <td class="auto-style1">-</td>
            <td>1321</td>
            <td class="auto-style2">Chagas crónico a demanda</td>
            <td>
                <asp:DropDownList ID="ddlAnio_33_1321" runat="server">
                     <asp:ListItem>2015</asp:ListItem>
                    <asp:ListItem>2016</asp:ListItem>      
                </asp:DropDownList></td>
            <td>
                <asp:TextBox CssClass="myTexto" ID="txtSemana_33_1321" runat="server"></asp:TextBox></td>
            <td> <asp:TextBox CssClass="myTexto" ID="txtCodigo_33_1321" runat="server"></asp:TextBox></td>
                <td>
              <asp:Label ID="lblCantidad_33_1321" runat="server"></asp:Label></td>
            <td>
                <asp:Button ID="btnDescargar1321" runat="server" Text="Descargar" CssClass="myButton" OnClick="btnDescargar1321_Click"  /></td>
        </tr>
        </table></td>
</tr>

      
       
<tr>
<td class="auto-style8"> 
  
      &nbsp;</td>
<td colspan="2"> 
  
      &nbsp;</td>
</tr>
</table> 
  </div>


    </asp:Content>