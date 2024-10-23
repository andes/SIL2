<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CodigoBarrasEdit.aspx.cs" Inherits="WebLab.CodigoBarrasEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title>    
   <link href="script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
 <link href="App_Themes/default/style.css" rel="stylesheet" type="text/css" /> 
                  <script src="script/jquery.min.js" type="text/javascript"></script>  
                  <script src="script/jquery-ui.min.js" type="text/javascript"></script> 

                   <script type="text/javascript">  
                   

                        $(function() {

                 $("#tabContainer").tabs();
                        var currTab = $("#<%= HFCurrTabIndex.ClientID %>").val();
                      
                        $("#tabContainer").tabs({ selected: currTab });
             });
             
            
             

                  </script> 
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    
 <div align="left" style="width:1000px">
   <asp:HiddenField runat="server" ID="HFCurrTabIndex"   /> 
    <table style="width:800px;" >
        <tr>
            <td>
              <b class="mytituloTabla">  Código de Barras</b></td>
            
        </tr>
            <tr>
            <td>
             <hr /></td>
            
        </tr>
           
        <tr>
            <td>       
            <div id="tabContainer" style="border: 0px solid #C0C0C0">  
             <ul class="myLabel">
    <li><a href="#tab1">LABORATORIO GENERAL</a></li>    
   
    <li><a href="#tab2">MICROBIOLOGIA</a></li>
    
</ul>


    <div id="tab1"  class="tab_content" style="border: 1px solid #C0C0C0">
    <table style="width:100%;">
                    <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 138px;">
                            Genera Código de Barras:</td>
                        <td align="left" class="myLabel" style="width: 81%">
                            <anthem:CheckBox ID="chkImprimeCodigoBarrasLaboratorio" runat="server" Text="Si" 
                             
                                oncheckedchanged="chkImprimeCodigoBarrasLaboratorio_CheckedChanged" 
                                AutoCallBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; " colspan="2">
                           <hr /></td>
                    </tr>
                    <tr>
                    <td colspan="2">
                    <anthem:Panel ID="pnlLaboratorio" Enabled="false" runat="server">
                    <table>
                                        <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 156px;">
                            Fuente de código de barras:</td>
                        <td class="myLabel">
                            <asp:RadioButtonList ID="ddlFuente" runat="server">
                                <asp:ListItem Value="barcode39">Code 39</asp:ListItem>
                                <asp:ListItem Value="Code 128">Code 128</asp:ListItem>
                                <asp:ListItem Value="EAN-13">EAN-13</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del protocolo a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkProtocolo" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="False" Selected="True">Numero de Protocolo</asp:ListItem>
                                <asp:ListItem>Fecha</asp:ListItem>
                                <asp:ListItem>Origen</asp:ListItem>
                                <asp:ListItem>Sector</asp:ListItem>
                                <asp:ListItem>Nro. Origen</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del paciente a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkPaciente" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Apellido y Nombre</asp:ListItem>
                                <asp:ListItem>Sexo</asp:ListItem>
                                <asp:ListItem>Edad</asp:ListItem>
                                <asp:ListItem>Nro. Documento</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
 
 
                    </table>
                    </anthem:Panel>
                    </td>
                    </tr>
                    <tr>
 <td class="myLabelIzquierda"  colspan="2"> <asp:Button ID="btnGuardar" runat="server" CssClass="myButton" Text="Guardar" 
                    onclick="btnGuardar_Click" /></td>
 </tr>
               </table>
    </div>
    
    <div id="tab2"  class="tab_content" style="border: 1px solid #C0C0C0">
    <table style="width:100%;">
                    <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 143px;">
                            Generar Código de Barras:</td>
                        <td align="left" class="myLabel" style="width: 81%">
                            <anthem:CheckBox ID="chkImprimeCodigoBarrasMicrobiologia" runat="server" Text="Si" 
                             
                                oncheckedchanged="chkImprimeCodigoBarrasMicrobiologia_CheckedChanged" 
                                AutoCallBack="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; " colspan="2">
                           <hr /></td>
                    </tr>
                    <tr>
                    <td colspan="2">
                    <anthem:Panel ID="pnlMicrobiologia" Enabled=false runat="server">
                    <table>
                                        <tr>
                        <td class="myLabelIzquierda" style="vertical-align: top; width: 156px;">
                            Fuente de código de barras:</td>
                        <td class="myLabel">
                            <asp:RadioButtonList ID="rdbFuente2" runat="server">
                              <asp:ListItem Value="barcode39">Code 39</asp:ListItem>
                                <asp:ListItem Value="Code 128">Code 128</asp:ListItem>
                                <asp:ListItem Value="EAN-13">EAN-13</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del protocolo a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkProtocolo2" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Enabled="False" Selected="True">Numero de Protocolo</asp:ListItem>
                                <asp:ListItem>Fecha</asp:ListItem>
                                <asp:ListItem>Origen</asp:ListItem>
                                <asp:ListItem>Sector</asp:ListItem>
                                <asp:ListItem>Nro. Origen</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="myLabelIzquierda" style="width: 156px">
                            Datos del paciente a incluir:</td>
                        <td class="myLabel">
                            <asp:CheckBoxList ID="chkPaciente2" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Apellido y Nombre</asp:ListItem>
                                <asp:ListItem>Sexo</asp:ListItem>
                                <asp:ListItem>Edad</asp:ListItem>
                                <asp:ListItem>Nro. Documento</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
 
 
                    </table>
                    </anthem:Panel>
                    </td>
                    </tr>
                    <tr>
 <td class="myLabelIzquierda"  colspan="2"> 
     <asp:Button ID="btnGuardar2" runat="server" CssClass="myButton" Text="Guardar" 
                    onclick="btnGuardar2_Click" oncommand="btnGuardar2_Command" /></td>
 </tr>
               </table>
    </div>

</div>

</td>
            
        </tr>
        <tr>
            <td>
                
            </td>
            
        </tr>
        <tr>
            <td>
                <hr /></td>
            
        </tr>
         <tr>
            <td align="right">
                     
               
                     
</td>
            
        </tr>
        </table>
     </div>     
     
                    
        
   
    
    
    
    
    

    
</asp:Content>