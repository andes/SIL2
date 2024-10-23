<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="PlacaResultado.aspx.cs" Inherits="WebLab.Placas.PlacaResultado" ValidateRequest="false" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <title>LABORATORIO</title> 
     <style type="text/css">
        @media print {
           .enca{ display:none }

 
}
    </style>
<link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
 <link href="../App_Themes/default/style.css" rel="stylesheet" type="text/css" />  
     <script src="../script/jquery.min.js" type="text/javascript"></script>  
                  <script src="../script/jquery-ui.min.js" type="text/javascript"></script> 
         <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
    
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
 
<script type="text/javascript">
    function enterToTab(pEvent) {///solo para internet explorer
        if (window.event) // IE
        {
            if (window.event.keyCode == 13) {
                if (pEvent.srcElement.id.indexOf('txt') == 0) {
                    window.event.keyCode = 9;
                }
            }
        }

    }

    function Enter(field, event) {
        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        if (keyCode == 13) {
            var i;
            for (i = 0; i < field.form.elements.length; i++)
                if (field == field.form.elements[i])
                    break;
            i = (i + 1) % field.form.elements.length;
            field.form.elements[i].focus();
            return false;
        }
        else
            return true;

    }

    function imprSelec(nombre) {
        var ficha = document.getElementById(nombre);
        var ventimp = window.open(' ', 'popimpr');
     
        ventimp.document.write( ficha.innerHTML );
        ventimp.document.close();
        ventimp.print( );
        ventimp.close();
    }


</script>
   

</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">     
     <div  style="width: 100%" class="form-inline"     >
           <div class="panel panel-success" runat="server"  > 
                    <div class="panel-heading" > 
                        <h1 class="panel-title">PLACA  <asp:Label runat="server" Text="lblEquipo" id="lblEquipo" ></asp:Label> 
                                  <asp:Label runat="server" Font-Bold="true" Text="lblNro" id="lblNro" ></asp:Label> </h1>

                        <input id="HdIdPlaca" type="hidden" runat="server" />
                        <input id="HdSeDetecta" type="hidden" runat="server" value="SE DETECTA GENOMA DE SARS-CoV-2"/>
                        <input id="HdNoSeDetecta" type="hidden" runat="server" value="NO SE DETECTA GENOMA DE SARS-CoV-2"/>
                         <input id="HdModelo" type="hidden" runat="server" value=""/>
                          <table style="width:100%;" id="enca" border="0"  class="table table-bordered bs-table" >
                              <tr>
                                  <td>
                        
                     
  <h4> 
                                      <asp:Label ID="lbltituloCSV" runat="server" Text="Archivo CSV:"></asp:Label></h4>
               <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm" Width="400px" />
                <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="150px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />
                                      <input id="hdProcesaArchivo" runat="server" type="hidden" />
                                  </td>
                                  <td align="right">
                        
                     
                <asp:Button  CssClass="btn btn-success" ID="btnValidar" runat="server" Width="100px" 
                    Text="Validar" OnClick="btnValidar_Click" />

                                  </td>
                                  <td rowspan="2">
                                       <table class="table table-bordered bs-table" id="cantidad" width="100px"  cellpadding="0" cellspacing="1" style="border-style: solid; border-width: thin; line-height: normal;">
                                          <tr>
                                              <td><strong>Positivos:</strong>   
                       
                        </td>
                                              <td><asp:Label ID="lblPosi" runat="server" Text="Label"></asp:Label></td>
                                          </tr>
                                          <tr>
                                              <td><strong>Negativos:</strong></td>
                                              <td><asp:Label ID="lblNega" runat="server" Text="Label"></asp:Label></td>
                                          </tr>
                                          <tr>
                                              <td><strong>Indeterminados:</strong></td>
                                              <td><asp:Label ID="lblIndeterminado" runat="server" Text="Label"></asp:Label></td>
                                          </tr>
                                           <tr>
                                              <td><strong>Total:</strong></td>
                                              <td><asp:Label ID="lblTotal" class="control-label" runat="server" Text="0"></asp:Label></td>
                                          </tr>
                                      </table>
                                  </td>
                              </tr>
                              <tr>
                                  <td>
                        
                     
                        <div>
                            <a href="javascript:seleccionar_todo()" id="marcar" runat="server" >Marcar todos</a>
                            <a href="javascript:desmarcar_todo()" id="desmarcar" runat="server">DesMarcar todos</a>
                        
                       
                                    <a href="javascript:imprSelec('muestra')" runat="server" id="textoimprimir" >Imprimir Placa</a>
                                            
                     
                <asp:Button  CssClass="btn btn-success" ID="btnImprimir" runat="server" Width="100px" 
                    Text="Imprimir" OnClick="btnImprimir_Click" />

                    </div>    

                                  </td>
                                  <td align="right">
                        
                     
                                      &nbsp;</td>
                              </tr>
                          </table>
                        
                     
                        </div>
               
                   <div class="panel-body" id="muestra" >
                        
                         <style>
       table, th, td {
  border: 1px solid black;
} 
       
       </style>

                       <asp:Panel ID="pnlAlplex" runat="server" Visible="false">
                           <table cellpadding="2"   cellspacing="3" class="myTabla">
                               <tr>
                                   <td align="center" style="height:100%;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                                <td align="center" style="height:50px; width:50px;">
                                                    
                                               </td>
                                           </tr>
                                           <tr>
                                              <td align="center" style="height:200px; width:50px;"><strong>A</strong></td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:50px;"><strong>B</strong></td>
                                           </tr>
                                           <tr>
                                              <td align="center" style="height:200px; width:50px;"><strong>C</strong></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:50px;"><strong>D</strong></td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:50px;"><strong>E</strong></td>
                                           </tr>
                                           <tr>
                                            <td align="center" style="height:200px; width:50px;"><strong>F</strong></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:50px;"><strong>G</strong></td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:50px;"><strong>H</strong></td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                     <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;1&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;" bgcolor="#CCCCCC">
                                                   <br />
                                                   &nbsp;&nbsp;
                                                   <br />
                                                   &nbsp;&nbsp;
                                                   <br />
                                                   &nbsp;&nbsp;
                                                   &nbsp;&nbsp; NTC&nbsp;&nbsp;
                                                   <br />
                                                   &nbsp;&nbsp;
                                                   <br />
                                                   &nbsp;&nbsp;
                                                   <br />
                                                   &nbsp;&nbsp;
                                               </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                       <asp:Panel ID="B01" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_B01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B01" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B01" runat="server" Text="Valida" />
                                                           </asp:Panel>
                                               </td>
                                               
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                    <asp:Panel ID="C01" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C01" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C01" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                              <td align="center" style="height:200px; width:200px;">
              <asp:Panel ID="D01" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_D01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_D01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_D01" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_D01" runat="server" Text="Label"></asp:Label>
                                                   <br />

                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_D01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_D01" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:200px;">
         <asp:Panel ID="E01" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_E01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_E01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_E01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_E01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_E01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_E01" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
              <asp:Panel ID="F01" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_F01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_F01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_F01" runat="server" Text="Label"></asp:Label>
                                               <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_F01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_F01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_F01" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
      <asp:Panel ID="G01" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_G01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                
                                                   <br />
                                                   <asp:Label ID="lblLugar_G01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_G01" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_G01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_G01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_G01" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                  <td align="center" style="height:200px; width:200px;">
     <asp:Panel ID="H01" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_H01" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_H01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_H01" runat="server" Text="Label"></asp:Label>
                                                  <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_H01" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_H01" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_H01" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                                      </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;2&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="A02" runat="server" Visible="false"  Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A02" runat="server" Text="Label"  Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A02" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A02" runat="server" Text="Label"></asp:Label>
                                                  
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A02" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A02" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A02" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="B02" runat="server" Visible="false"  Width="110px" Height="200px">
                             <asp:Label ID="lblProtocolo_B02" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_B02" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B02" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B02" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B02" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B02" runat="server" Text="Valida" />
                                                         </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="C02" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C02" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C02" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C02" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C02" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C02" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C02" runat="server" Text="Valida" />
                                                       </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D02" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D02" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                      
                                                       <br />
                                                       <asp:Label ID="lblLugar_D02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D02" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D02" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                      <br /> <asp:CheckBox ID="chkValida_D02" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E02" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E02" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E02" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E02" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E02" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F02" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F02" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F02" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F02" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F02" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G02" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G02" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G02" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G02" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G02" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H02" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H02" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H02" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H02" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H02" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H02" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;3&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="A03" runat="server" Visible="false"  Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A03" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A03"  runat="server" Text="Label"></asp:Label>
                                                  
                                                <br />
                                                  
                                                   <asp:Label  Font-Size="8px"  ID="lblCt1_A03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label Font-Size="8.5px"  ID="lblResultado_A03"   runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A03" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="B03" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B03" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                
                                                   <br />
                                                   <asp:Label ID="lblLugar_B03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B03"  runat="server" Text="Label"></asp:Label>
                                                      <br />
                                                   <asp:Label Font-Size="8px"  ID="lblCt1_B03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px"  ID="lblResultado_B03" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B03" runat="server" Text="Valida" />
                                                         </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                            <td align="center" style="height:200px; width:200px;">
                                                  <asp:Panel ID="C03" runat="server" Visible="false"  Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C03" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C03"    runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C03" runat="server" Text="Label"></asp:Label>

                                                   <br />
                                                   <asp:Label  Font-Size="8.5px"   ID="lblResultado_C03" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C03" runat="server" Text="Valida" />
                                                      </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="D03" runat="server" Visible="false"  Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_D03" runat="server" Text="Label"  Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_D03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_D03"   runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                                  
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_D03" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px"   ID="lblResultado_D03" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_D03" runat="server" Text="Valida" />
                                                          </asp:Panel></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E03" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E03" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px"   ID="lblFIS_E03"   runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px"  ID="lblCt1_E03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px"  ID="lblResultado_E03" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E03" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F03" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F03" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F03" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px"   ID="lblResultado_F03" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F03" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G03" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G03" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G03" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px"  ID="lblResultado_G03" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G03" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H03" runat="server" Visible="false"  Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H03" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H03" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H03" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H03" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H03" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;4&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                            <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="A04" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A04" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A04" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A04" runat="server" Text="Label"></asp:Label>
                                                  

                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A04" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A04" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A04" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="b04" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B04" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                
                                                   <br />
                                                   <asp:Label ID="lblLugar_B04" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B04" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B04" runat="server" Text="Label"></asp:Label>

                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B04" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B04" runat="server" Text="Valida" /></asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                           <td align="center" style="height:200px; width:200px;">
                                                <asp:Panel ID="C04" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C04" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C04" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C04" runat="server" Text="Label"></asp:Label>
                                                 <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C04" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C04" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C04" runat="server" Text="Valida" />
                                                    </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                    <asp:Panel ID="D04" runat="server" Visible="false" Width="110px" Height="200px">
                                                        <asp:Label ID="lblProtocolo_D04" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblLugar_D04" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label Font-Size="8px" ID="lblFIS_D04" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label Font-Size="8px" ID="lblCt1_D04" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label  Font-Size="8.5px" ID="lblResultado_D04" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                        <br />
                                                        <br />
                                                        <asp:CheckBox ID="chkValida_D04" runat="server" Text="Valida" />
                                                    </asp:Panel>
                                                </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="E04" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_E04" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_E04" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_E04" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_E04" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_E04" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_E04" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                 </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F04" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F04" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F04" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F04" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F04" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F04" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F04" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="G04" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_G04" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_G04" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_G04" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_G04" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_G04" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_G04" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                 </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="H04" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_H04" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_H04" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_H04" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_H04" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_H04" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_H04" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                 </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;5&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                           <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="A05" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A05" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A05" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A05" runat="server" Text="Label"></asp:Label>
                                                 

                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A05" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A05" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A05" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="B05" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B05" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                               
                                                   <br />
                                                   <asp:Label ID="lblLugar_B05" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B05" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B05" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B05" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B05" runat="server" Text="Valida" />
                                                         </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:200px;">
                                                 <asp:Panel ID="C05" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C05" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C05" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C05" runat="server" Text="Label"></asp:Label>
                                                 <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C05" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C05" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C05" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                    <asp:Panel ID="D05" runat="server" Visible="false" Width="110px" Height="200px">
                                                        <asp:Label ID="lblProtocolo_D05" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblLugar_D05" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label Font-Size="8px" ID="lblFIS_D05" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label Font-Size="8px" ID="lblCt1_D05" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label  Font-Size="8.5px" ID="lblResultado_D05" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                        <br />
                                                        <br />
                                                        <asp:CheckBox ID="chkValida_D05" runat="server" Text="Valida" />
                                                    </asp:Panel>
                                                </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="E05" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_E05" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_E05" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_E05" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_E05" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_E05" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_E05" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                 </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="F05" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_F05" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_F05" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_F05" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_F05" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_F05" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_F05" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                 </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="G05" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_G05" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_G05" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_G05" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_G05" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_G05" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_G05" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                 </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                    <asp:Panel ID="H05" runat="server" Visible="false" Width="110px" Height="200px">
                                                        <asp:Label ID="lblProtocolo_H05" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label ID="lblLugar_H05" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label Font-Size="8px" ID="lblFIS_H05" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label Font-Size="8px" ID="lblCt1_H05" runat="server" Text="Label"></asp:Label>
                                                        <br />
                                                        <asp:Label  Font-Size="8.5px" ID="lblResultado_H05" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                        <br />
                                                        <br />
                                                        <asp:CheckBox ID="chkValida_H05" runat="server" Text="Valida" />
                                                    </asp:Panel>
                                                </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;6&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                    <asp:Panel ID="A06" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A06" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A06" runat="server" Text="Label"></asp:Label>
                                                
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A06" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A06" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="B06" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B06" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                
                                                   <br />
                                                   <asp:Label ID="lblLugar_B06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B06" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B06" runat="server" Text="Valida" />
                                                         </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               
                                                   <td align="center" style="height:200px; width:200px;">
                                                 <asp:Panel ID="C06" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C06" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                   
                                                   <br />
                                                   <asp:Label ID="lblLugar_C06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C06" runat="server" Text="Label"></asp:Label>
                                                  <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C06" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C06" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C06" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="D06" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_D06" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label ID="lblLugar_D06" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_D06" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_D06" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_D06" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                         <br />
                                                         <asp:CheckBox ID="chkValida_D06" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                                     </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E06" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E06" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E06" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E06" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E06" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F06" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F06" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F06" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F06" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F06" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G06" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G06" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G06" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G06" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G06" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H06" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H06" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H06" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H06" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H06" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H06" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;7&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="A07" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A07" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A07" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A07" runat="server" Text="Label"></asp:Label>
                                                 
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A07" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A07" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A07" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="B07" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B07" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_B07" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B07" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B07" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B07" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B07" runat="server" Text="Valida" />
                                                         </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="C07" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_C07" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                     
                                                       <br />
                                                       <asp:Label ID="lblLugar_C07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_C07" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_C07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_C07" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                      <br /> <asp:CheckBox ID="chkValida_C07" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D07" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D07" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_D07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D07" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D07" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_D07" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E07" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E07" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E07" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E07" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E07" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F07" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F07" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F07" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F07" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F07" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G07" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G07" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G07" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G07" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G07" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H07" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H07" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H07" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H07" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H07" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H07" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;8&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="A08" runat="server" Visible="false"  Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A08" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A08" runat="server" Text="Label"></asp:Label>
                                                 
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A08" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A08" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                                      <asp:Panel ID="B08" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B08" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_B08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B08" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B08" runat="server" Text="Valida" />
                                                          </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   
                                                 <asp:Panel ID="C08" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C08" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C08" runat="server" Text="Label"></asp:Label>
                                                  <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C08" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C08" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C08" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td> 
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D08" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D08" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_D08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D08" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D08" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_D08" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E08" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E08" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E08" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E08" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E08" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F08" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F08" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F08" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F08" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F08" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G08" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G08" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G08" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G08" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G08" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H08" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H08" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H08" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H08" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H08" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H08" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;9&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                               <asp:Panel ID="A09" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A09" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A09" runat="server" Text="Label"></asp:Label>
                                                 
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A09" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A09" runat="server" Text="Valida" />
                                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="B09" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_B09" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_B09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B09" runat="server" Text="Label"></asp:Label>
                                                  <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B09" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B09" runat="server" Text="Valida" />
                                                         </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                 <asp:Panel ID="C09" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C09" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                  
                                                   <br />
                                                   <asp:Label ID="lblLugar_C09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C09" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C09" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C09" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D09" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D09" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_D09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D09" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D09" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_D09" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E09" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E09" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E09" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E09" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E09" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F09" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F09" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F09" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F09" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F09" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G09" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G09" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G09" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G09" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G09" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H09" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H09" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H09" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H09" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H09" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H09" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;10&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                                <td align="center" style="height:200px; width:200px;">
                                                     <asp:Panel ID="A10" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A10" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A10" runat="server" Text="Label"></asp:Label>
                                                 
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A10" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A10" runat="server" Text="Valida" />
</asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                             <asp:Panel ID="B10" runat="server" Visible="false" Width="110px" Height="200px">
 <asp:Label ID="lblProtocolo_B10" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                               
                                                   <br />
                                                   <asp:Label ID="lblLugar_B10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B10" runat="server" Text="Label"></asp:Label>
                                                     <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_B10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B10" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B10" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                             <td align="center" style="height:200px; width:200px;">
                                                 <asp:Panel ID="C10" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C10" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                
                                                   <br />
                                                   <asp:Label ID="lblLugar_C10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C10" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C10" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C10" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D10" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D10" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_D10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D10" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D10" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_D10" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E10" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E10" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E10" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E10" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E10" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F10" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F10" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F10" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F10" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F10" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G10" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G10" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G10" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G10" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G10" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H10" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H10" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H10" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H10" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H10" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H10" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;11&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                        <asp:Panel ID="A11" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A11" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A11" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A11" runat="server" Text="Label"></asp:Label>
                                                 
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A11" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A11" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A11" runat="server" Text="Valida" />
</asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                                 <td align="center" style="height:200px; width:200px;">
                                            
                                                     <asp:Panel ID="B11" runat="server" Visible="false" Width="110px" Height="200px">
                                                         <asp:Label ID="lblProtocolo_B11" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       
                                                         <br />
                                                         <asp:Label ID="lblLugar_B11" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label Font-Size="8px" ID="lblFIS_B11" runat="server" Text="Label"></asp:Label>
                                                           <br />
                                                         <asp:Label Font-Size="8px" ID="lblCt1_B11" runat="server" Text="Label"></asp:Label>
                                                         <br />
                                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_B11" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                         <br />
                                                        <br /> <asp:CheckBox ID="chkValida_B11" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                 <asp:Panel ID="C11" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C11" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_C11" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C11" runat="server" Text="Label"></asp:Label>
                                                    <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C11" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C11" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C11" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D11" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D11" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_D11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D11" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D11" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_D11" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E11" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E11" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E11" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E11" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E11" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="F11" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F11" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F11" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F11" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F11" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G11" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G11" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G11" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G11" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G11" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H11" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H11" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H11" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H11" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H11" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H11" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                                   <td align="center" style="height:200px; width:200px;">
                                       <table cellpadding="2" cellspacing="3" style="border-style: solid; border-width: thin; line-height: normal;">
                                           <tr>
                                               <td align="center" style="height:50px; width:200px;"><strong>&nbsp;&nbsp;12&nbsp;&nbsp;</strong></td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                    <asp:Panel ID="A12" runat="server" Visible="false" Width="110px" Height="200px">
                                                   <asp:Label ID="lblProtocolo_A12" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_A12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_A12" runat="server" Text="Label"></asp:Label>
                                                   
                                                <br />
                                                  
                                                   <asp:Label Font-Size="8px" ID="lblCt1_A12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                  
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_A12" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_A12" runat="server" Text="Valida" />
                                                        </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                              <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="B12" runat="server" Visible="false" Width="110px" Height="200px"> 
                                           
                                                       <asp:Label ID="lblProtocolo_B12" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label ID="lblLugar_B12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_B12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_B12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_B12" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_B12" runat="server" Text="Valida" />
                                                       </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                              <td align="center" style="height:200px; width:200px;">
                                                 <asp:Panel ID="C12" runat="server" Visible="false" Width="110px" Height="200px">
                                             <asp:Label ID="lblProtocolo_C12" runat="server" Text="Label" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                 
                                                   <br />
                                                   <asp:Label ID="lblLugar_C12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label Font-Size="8px" ID="lblFIS_C12" runat="server" Text="Label"></asp:Label>
                                                  <br />
                                                   <asp:Label Font-Size="8px" ID="lblCt1_C12" runat="server" Text="Label"></asp:Label>
                                                   <br />
                                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_C12" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                                                    <br />
                                                  <br /> <asp:CheckBox ID="chkValida_C12" runat="server" Text="Valida" />
                                                     </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="D12" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_D12" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_D12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_D12" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_D12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_D12" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_D12" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="E12" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_E12" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_E12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_E12" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_E12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_E12" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_E12" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                              
                                                   <asp:Panel ID="F12" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_F12" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_F12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_F12" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_F12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_F12" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_F12" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                              
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="G12" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_G12" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_G12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_G12" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_G12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_G12" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_G12" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                           <tr>
                                               <td align="center" style="height:200px; width:200px;">
                                                   <asp:Panel ID="H12" runat="server" Visible="false" Width="110px" Height="200px">
                                                       <asp:Label ID="lblProtocolo_H12" runat="server" Font-Bold="True" Font-Size="12pt" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label ID="lblLugar_H12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblFIS_H12" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label Font-Size="8px" ID="lblCt1_H12" runat="server" Text="Label"></asp:Label>
                                                       <br />
                                                       <asp:Label  Font-Size="8.5px" ID="lblResultado_H12" runat="server" Font-Bold="True" Text="Label"></asp:Label>
                                                       <br />
                                                       <br />
                                                       <asp:CheckBox ID="chkValida_H12" runat="server" Text="Valida" />
                                                   </asp:Panel>
                                               </td>
                                           </tr>
                                       </table>
                                   </td>
                               </tr>
                           </table>
                       </asp:Panel>
<asp:Panel ID="pnlPromega2" runat="server" Visible="False">
                       <table class="myTabla" cellpadding="2" cellspacing="3">
                           <tr>
                               <td colspan="2"> &nbsp;&nbsp;&nbsp;&nbsp;</td>
                               <td align="center" bgcolor="#CCCCCC"><h4>1</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>2</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>3</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>4</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>5</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>6</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>7</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>8</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>9</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>10</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>11</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>12</h4></td>                         

                           </tr>
                            <tr>
                               <td bgcolor="#CCCCCC"> &nbsp;&nbsp;</td>
                               <td bgcolor="#CCCCCC"> <h4>ID</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h1><asp:Label ID="Label2" runat="server" Font-Size="14px"   Text="NTC" Width="100px"></asp:Label></h1></td>
                                
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_2" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_2" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_2" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_3" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_3" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_3" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_4" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_4" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_4" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_5" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_5" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_5" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_6" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_6" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_6" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_7" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_7" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_7" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_8" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_8" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_8" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_9" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_9" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_9" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_10" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_10" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_10" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_11" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_11" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_11" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_ABC_12" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_ABC_12" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_ABC_12" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                                    

                           </tr>
                                <tr>
                                <td align="center" bgcolor="#CCCCCC">
                                    <h4>A</h4>
                                    </td>
                                      <td bgcolor="#CCCCCC"> <h4>N1</h4></td>
                                <td bgcolor="#CCCCCC"></td>
                               <td> <asp:Label  ID="lbl_A02" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_A03" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                              <td>
                                  <asp:Label ID="lbl_A04" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                    </td>
                               <td> <asp:Label ID="lbl_A05" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_A06" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_A07" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_A08" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                          <td> <asp:Label ID="lbl_A09" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                     <td> <asp:Label ID="lbl_A10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                                    <td> <asp:Label ID="lbl_A11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                               <td> <asp:Label ID="lbl_A12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>            

                           </tr>
                    
                            <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6> B</h6></td>
                                  <td bgcolor="#CCCCCC"> <h4>N2</h4></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                                <td> <asp:Label ID="lbl_B02" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_B03" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                              <td>
                                  <asp:Label ID="lbl_B04" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                    </td>
                               <td> <asp:Label ID="lbl_B05" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_B06" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_B07" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_B08" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                          <td> <asp:Label ID="lbl_B09" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                     <td> <asp:Label ID="lbl_B10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                                    <td> <asp:Label ID="lbl_B11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                               <td> <asp:Label ID="lbl_B12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>         

                           </tr>
                            <tr>
                               <td align="center" bgcolor="#CCCCCC">c</td>
                               
                               <td bgcolor="#CCCCCC"><h4>RP</h4></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                           <td> <asp:Label ID="lbl_C02" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_C03" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                              <td>
                                  <asp:Label ID="lbl_C04" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                    </td>
                               <td> <asp:Label ID="lbl_C05" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_C06" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_C07" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_C08" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                          <td> <asp:Label ID="lbl_C09" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                     <td> <asp:Label ID="lbl_C10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                                    <td> <asp:Label ID="lbl_C11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                               <td> <asp:Label ID="lbl_C12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                           </tr>
                                 <tr>
                                     <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                     <td bgcolor="#CCCCCC">
                                         <h4>Res</h4>
                                     </td>
                                     <td bgcolor="#CCCCCC">&nbsp;</td>
                                    <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_2" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_2" runat="server" Visible="false" Text="Valida" /></td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_3" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_3" runat="server" Visible="false" Text="Valida" /></td>
                                     <td>
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_4" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_4" runat="server" Visible="false" Text="Valida" />
                                     </td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_5" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_5" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_6" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_6" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_7" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_7" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_8" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_8" runat="server" Visible="false" Text="Valida" /></td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_9" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_9" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_10" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_11" runat="server" Visible="false" Text="Valida" /></td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_ABC_12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_ABC_12" runat="server" Visible="false" Text="Valida" /></td>
                           </tr>
                                 <tr>
                                     <td align="center" colspan="14">
                                        <br /></td>
                           </tr>
                                 
                                 <tr>

                              <td align="center" bgcolor="#CCCCCC" colspan="2" rowspan="2"  >
                                  <h4>D</h4>
                                    
                                     </td>
                               
                               <td> <h4>N1</h4>
                                   
                                   <asp:Label ID="lbl_D01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                              <td> <h4>N2</h4> <asp:Label ID="lbl_D02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td> <h4>Rp</h4><asp:Label ID="lbl_D03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                              <td>
                                  <h4>N1</h4>
                              <asp:Label ID="lbl_D04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td>
                                    <h4>N2</h4>
                                    <asp:Label ID="lbl_D05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                              <td>
                                  <h4>Rp</h4>
                                  <asp:Label ID="lbl_D06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                               <td>
                                   <h4>N1</h4>
                                  <asp:Label ID="lbl_D07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                               <td>
                                   <h4>N2</h4>
                                   <asp:Label ID="lbl_D08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td>
                                    <h4>Rp</h4>
                                    <asp:Label ID="lbl_D09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td>
                                    <h4>N1</h4>
                                    <asp:Label ID="lbl_D10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>             
                                <td>
                                    <h4>N2</h4>
                                    <asp:Label ID="lbl_D11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>             
                                       <td>
                                           <h4>Rp</h4>
                                           <asp:Label ID="lbl_D12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>             

                           </tr>
                           <tr>
                                     <td   colspan="3">
                                     <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_D_123" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_D_123" runat="server" Visible="false" Text="Valida" />
                                       </td>
                                     <td   colspan="3">
                                     <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_D_456" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_D_456" runat="server" Visible="false" Text="Valida" /></td>
                                     <td   colspan="3">
                                          <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_D_789" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_D_789" runat="server" Visible="false" Text="Valida" /></td>
                                     <td   colspan="3">
                                           <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_D_101112" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_D_101112" runat="server" Visible="false" Text="Valida" /></td>
                           </tr>
                        
                    
               
                               <tr>
                                   <td align="center" bgcolor="#CCCCCC" colspan="2">
                                       <h4>ID</h4>
                                   </td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                       <asp:Label ID="lbl_D_123" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                       <br />
                                       <asp:Label ID="lblLugar_D_123" runat="server" Text="Label" Visible="false"></asp:Label>
                                       -
                                       <asp:Label Font-Size="8px" ID="lblFIS_D_123" runat="server" Text="Label" Visible="false"></asp:Label>
                                   </td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                         <asp:Label ID="lbl_D_456" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                       <br />
                                       <asp:Label ID="lblLugar_D_456" runat="server" Text="Label" Visible="false"></asp:Label>
                                       -
                                       <asp:Label Font-Size="8px" ID="lblFIS_D_456" runat="server" Text="Label" Visible="false"></asp:Label>
                                       &nbsp;&nbsp; </td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                        <asp:Label ID="lbl_D_789" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                       <br />
                                       <asp:Label ID="lblLugar_D_789" runat="server" Text="Label" Visible="false"></asp:Label>
                                       -
                                       <asp:Label Font-Size="8px" ID="lblFIS_D_789" runat="server" Text="Label" Visible="false"></asp:Label>
                                       &nbsp;&nbsp; </td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                       <asp:Label ID="lbl_D_101112" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                       <br />
                                       <asp:Label ID="lblLugar_D_101112" runat="server" Text="Label" Visible="false"></asp:Label>
                                       -
                                       <asp:Label Font-Size="8px" ID="lblFIS_D_101112" runat="server" Text="Label" Visible="false"></asp:Label>
                                       &nbsp;&nbsp; </td>
                           </tr>
                        
                    
               
                               <tr>

                                <td align="center" colspan="14"  ><br /></td>
                               
                           </tr>
                           <tr>

                              <td align="center" bgcolor="#CCCCCC" colspan="2" rowspan="2"  >
                                  <h4>E</h4>
                                    
                                     </td>
                               
                                <td> <h4>N1</h4>
                                   
                                   <asp:Label ID="lbl_E01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                              <td> <h4>N2</h4> <asp:Label ID="lbl_E02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td> <h4>Rp</h4><asp:Label ID="lbl_E03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                              <td>
                                  <h4>N1</h4>
                              <asp:Label ID="lbl_E04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td>
                                    <h4>N2</h4>
                                    <asp:Label ID="lbl_E05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                              <td>
                                  <h4>Rp</h4>
                                  <asp:Label ID="lbl_E06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                               <td>
                                   <h4>N1</h4>
                                  <asp:Label ID="lbl_E07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                               <td>
                                   <h4>N2</h4>
                                   <asp:Label ID="lbl_E08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td>
                                    <h4>Rp</h4>
                                    <asp:Label ID="lbl_E09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                                <td>
                                    <h4>N1</h4>
                                    <asp:Label ID="lbl_E10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>             
                                <td>
                                    <h4>N2</h4>
                                    <asp:Label ID="lbl_E11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>             
                                       <td>
                                           <h4>Rp</h4>
                                           <asp:Label ID="lbl_E12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>             
           

                           </tr>
                      
                       
         
                 
                                <tr>
                                   
                                    <td bgcolor="#CCCCCC" colspan="3">
                                    <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_E_123" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_E_123" runat="server" Visible="false" Text="Valida" /></td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                         <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_E_456" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_E_456" runat="server" Visible="false" Text="Valida" /></td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                         <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_E_789" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_E_789" runat="server" Visible="false" Text="Valida" /></td>
                                   <td bgcolor="#CCCCCC" colspan="3">
                                         <h6>     RES:
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_E_101112" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></h6>
                                         <br /> <asp:CheckBox ID="chkValida_E_101112" runat="server" Visible="false" Text="Valida" /></td>
                                    
                                </tr>
                       
         
                 
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC" colspan="2">
                                        <h4>ID</h4>
                                    </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <asp:Label ID="lbl_E_123" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblLugar_E_123" runat="server" Text="Label" Visible="false"></asp:Label>
                                        -
                                        <asp:Label Font-Size="8px" ID="lblFIS_E_123" runat="server" Text="Label" Visible="false"></asp:Label>
                                    </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <asp:Label ID="lbl_E_456" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblLugar_E_456" runat="server" Text="Label" Visible="false"></asp:Label>
                                        -
                                        <asp:Label Font-Size="8px" ID="lblFIS_E_456" runat="server" Text="Label" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <asp:Label ID="lbl_E_789" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblLugar_E_789" runat="server" Text="Label" Visible="false"></asp:Label>
                                        -
                                        <asp:Label Font-Size="8px" ID="lblFIS_E_789" runat="server" Text="Label" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; </td>
                                    <td bgcolor="#CCCCCC" colspan="3">
                                        <asp:Label ID="lbl_E_101112" runat="server" Font-Bold="True" Font-Size="12pt" Visible="false"></asp:Label>
                                        <br />
                                        <asp:Label ID="lblLugar_E_101112" runat="server" Text="Label" Visible="false"></asp:Label>
                                        -
                                        <asp:Label Font-Size="8px" ID="lblFIS_E_101112" runat="server" Text="Label" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; </td>
                                </tr>
                       
         
                 
                                <tr>
                                    <td align="center" colspan="14">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                    <td bgcolor="#CCCCCC" class="auto-style1">
                                        <h4>ID</h4>
                                    </td>
                                    <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_1" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_1" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_1" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                   <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_2" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_2" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_2" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_3" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_3" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_3" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_4" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_4" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_4" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_5" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_5" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_5" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_6" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_6" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_6" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_7" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_7" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_7" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_8" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_8" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_8" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_9" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_9" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_9" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_10" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_10" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_10" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_11" runat="server" Visible="False" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_11" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_11" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_FGH_12" runat="server" Font-Bold="True" Font-Size="12pt" Width="100px"></asp:Label>
                                   <br />
                                 
                                    <asp:Label ID="lblLugar_FGH_12" runat="server" Visible="false" Text="Label"></asp:Label>-                                                   <asp:Label Font-Size="8px" ID="lblFIS_FGH_12" runat="server" Visible="false" Text="Label"></asp:Label>
                                </td>
                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">
                                        <h4>F</h4>
                                    </td>
                                    <td bgcolor="#CCCCCC" class="auto-style1">
                                        <h4>N1</h4>
                                    </td>
                                   <td> <asp:Label ID="lbl_F01" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                     <td> <asp:Label ID="lbl_F02" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_F03" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                              <td>
                                  <asp:Label ID="lbl_F04" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                    </td>
                               <td> <asp:Label ID="lbl_F05" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_F06" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_F07" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_F08" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                          <td> <asp:Label ID="lbl_F09" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                     <td> <asp:Label ID="lbl_F10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                                    <td> <asp:Label ID="lbl_F11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                               <td> <asp:Label ID="lbl_F12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>            

                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">
                                        <h4>G</h4>
                                    </td>
                                    <td bgcolor="#CCCCCC" class="auto-style1">
                                        <h4>N2</h4>
                                    </td>
                                   <td> <asp:Label ID="lbl_G01" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                     <td> <asp:Label ID="lbl_G02" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_G03" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                              <td>
                                  <asp:Label ID="lbl_G04" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                    </td>
                               <td> <asp:Label ID="lbl_G05" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_G06" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_G07" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_G08" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                          <td> <asp:Label ID="lbl_G09" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                     <td> <asp:Label ID="lbl_G10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                                    <td> <asp:Label ID="lbl_G11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                               <td> <asp:Label ID="lbl_G12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>            

                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC"><h4>H</h4></td>
                                    <td bgcolor="#CCCCCC" class="auto-style1"><h4>RP</h4></td>
                                   <td> <asp:Label ID="lbl_H01" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                     <td> <asp:Label ID="lbl_H02" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_H03" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                              <td>
                                  <asp:Label ID="lbl_H04" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                    </td>
                               <td> <asp:Label ID="lbl_H05" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_H06" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                       <td> <asp:Label ID="lbl_H07" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                            <td> <asp:Label ID="lbl_H08" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                          <td> <asp:Label ID="lbl_H09" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                     <td> <asp:Label ID="lbl_H10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                                                    <td> <asp:Label ID="lbl_H11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>
                               <td> <asp:Label ID="lbl_H12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label></td>            

                                </tr>
                                <tr>
                                    <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                    <td bgcolor="#CCCCCC" class="auto-style1">
                                        <h4>Res</h4>
                                    </td>
                                    <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_1" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                     <br />   <asp:CheckBox ID="chkValida_FGH_1" runat="server" Visible="false" Text="Valida" /></td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_2" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_2" runat="server" Visible="false" Text="Valida" /></td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_3" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_3" runat="server" Visible="false" Text="Valida" /></td>
                                     <td>
                                         <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_4" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_4" runat="server" Visible="false" Text="Valida" />
                                     </td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_5" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_5" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_6" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_6" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_7" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_7" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_8" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_8" runat="server" Visible="false" Text="Valida" /></td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_9" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_9" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td>
 <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_10" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_10" runat="server" Visible="false" Text="Valida" />
</td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_11" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_11" runat="server" Visible="false" Text="Valida" /></td>
                                     <td> <asp:Label  Font-Size="8.5px" ID="lblResultado_FGH_12" runat="server" Visible="false" Font-Bold="True" Text="Label"></asp:Label>
                                         <br /> <asp:CheckBox ID="chkValida_FGH_12" runat="server" Visible="false" Text="Valida" /></td>
                                </tr>
                            </table>

                           </asp:Panel>


                            <asp:Panel ID="pnlPromega" runat="server" Visible="true">
                       <table class="myTabla" cellpadding="2" cellspacing="3">
                           <tr>
                               <td colspan="2"> &nbsp;&nbsp;&nbsp;&nbsp;</td>
                               <td align="center" bgcolor="#CCCCCC"><h4>1</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>2</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>3</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>4</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>5</h4></td>
                                 <td align="center" bgcolor="#CCCCCC"><h4>6</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>7</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>8</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>9</h4></td>
                                <td align="center" bgcolor="#CCCCCC"><h4>10</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>11</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>12</h4></td>                         

                           </tr>
                            <tr>
                               <td bgcolor="#CCCCCC"> &nbsp;&nbsp;</td>
                               <td bgcolor="#CCCCCC"> <h4>ID</h4></td>
                               <td align="center" bgcolor="#CCCCCC"><h4>NTC</h4></td>
                                 <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_AB_2" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_AB_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_AB_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_AB_3" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                   <br />
                                   <asp:Label ID="lblLugar_AB_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                   <br />
                                   <asp:Label Font-Size="8px" ID="lblFIS_AB_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                  </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_AB_4" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                   <br />
                                   <asp:Label ID="lblLugar_AB_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                   <br />
                                   <asp:Label Font-Size="8px" ID="lblFIS_AB_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                  </td>
                              <td bgcolor="#CCCCCC">
                                  <asp:Label ID="lbl_AB_5" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                  <br />
                                  <asp:Label ID="lblLugar_AB_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                   <asp:Label Font-Size="8px" ID="lblFIS_AB_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                   </td>
                                <td bgcolor="#CCCCCC">
                                    <asp:Label ID="lbl_AB_6" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblLugar_AB_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                     <asp:Label Font-Size="8px" ID="lblFIS_AB_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                     </td>
                              <td bgcolor="#CCCCCC">
                                  <asp:Label ID="lbl_AB_7" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                  <br />
                                  <asp:Label ID="lblLugar_AB_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                   <asp:Label Font-Size="8px" ID="lblFIS_AB_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                   </td>
                                <td bgcolor="#CCCCCC">
                                    <asp:Label ID="lbl_AB_8" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblLugar_AB_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                     <asp:Label Font-Size="8px" ID="lblFIS_AB_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                     </td>
                              <td bgcolor="#CCCCCC">
                                  <asp:Label ID="lbl_AB_9" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                  <br />
                                  <asp:Label ID="lblLugar_AB_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                   <asp:Label Font-Size="8px" ID="lblFIS_AB_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                   </td>
                               <td bgcolor="#CCCCCC">
                                    <asp:Label ID="lbl_AB_10" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                   <br />
                                   <asp:Label ID="lblLugar_AB_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                    <asp:Label Font-Size="8px" ID="lblFIS_AB_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                   <asp:Label ID="lbl_AB_11" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                   <br />
                                   <asp:Label ID="lblLugar_AB_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                    <asp:Label Font-Size="8px" ID="lblFIS_AB_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td bgcolor="#CCCCCC">
                                    <asp:Label ID="lbl_AB_12" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblLugar_AB_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                     <asp:Label Font-Size="8px" ID="lblFIS_AB_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                     </td>
                                                    

                           </tr>
                                <tr>
                                <td align="center" bgcolor="#CCCCCC">
                                    <h6>A</h6>
                                    </td>
                                      <td bgcolor="#CCCCCC"> <h6>N1</h6></td>
                                <td bgcolor="#CCCCCC"></td>
                               <td>
                                   <asp:Label ID="lbl_P_A02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_A03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                              <td>
                                   <asp:Label ID="lbl_P_A04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                 <td>
                                   <asp:Label ID="lbl_P_A05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_A06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                  <td>
                                   <asp:Label ID="lbl_P_A07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_A08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_A09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_A10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_A11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_A12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>     

                           </tr>
                    
                            <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6> B</h6></td>
                                  <td bgcolor="#CCCCCC"> <h6>RP</h6></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                               <td>
                                   <asp:Label ID="lbl_P_B02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td>
                                   <asp:Label ID="lbl_P_B03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td>
                                   <asp:Label ID="lbl_P_B04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_B05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td>
                                   <asp:Label ID="lbl_P_B06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_B07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_B08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_B09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td>
                                   <asp:Label ID="lbl_P_B10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_B11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td>
                                   <asp:Label ID="lbl_P_B12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>

                           </tr>
                            <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               
                               <td bgcolor="#CCCCCC"><h6>Res</h6></td>
                               <td bgcolor="#CCCCCC">&nbsp;</td>
                              <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_2" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_AB_2" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                    <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_3" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    <br />
                                    <asp:CheckBox ID="chkValida_P_AB_3" runat="server" Text="Valida" Visible="false" />
                                </td>
                              <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_4" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_AB_4" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                    <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_5" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    <br />
                                    <asp:CheckBox ID="chkValida_P_AB_5" runat="server" Text="Valida" Visible="false" />
                                </td>
                              <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_6" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_AB_6" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_7" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                   <br />
                                   <asp:CheckBox ID="chkValida_P_AB_7" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                   <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_8" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                   <br />
                                   <asp:CheckBox ID="chkValida_P_AB_8" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                    <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_9" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    <br />
                                    <asp:CheckBox ID="chkValida_P_AB_9" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                    <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    <br />
                                    <asp:CheckBox ID="chkValida_P_AB_10" runat="server" Text="Valida" Visible="false" />
                                </td>             
                                <td>
                                    <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    <br />
                                    <asp:CheckBox ID="chkValida_P_AB_11" runat="server" Text="Valida" Visible="false" />
                                </td>             
                                 <td>
                                     <asp:Label  Font-Size="8.5px" ID="lblResultado_P_AB_12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                     <br />
                                     <asp:CheckBox ID="chkValida_P_AB_12" runat="server" Text="Valida" Visible="false" />
                                </td> 
                           </tr>
                                 <tr>
                                     <td align="center" colspan="14">
                                        <br /></td>
                           </tr>
                                 <tr>
                                     <td align="center" bgcolor="#CCCCCC">
                                         <h4>&nbsp;</h4>
                                     </td>
                                     <td bgcolor="#CCCCCC">
                                         <h4>ID</h4>
                                     </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_1" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_1" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_1" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_2" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                     <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_3" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_4" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_5" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_6" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_7" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_8" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                    <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_9" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                   <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_10" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                     <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_11" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                   <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_CD_12" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_CD_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_CD_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                           </tr>
                                 <tr>

                              <td align="center" bgcolor="#CCCCCC"  >
                                  <h6>C</h6>
                                     </td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>N1</h6>
                                     </td>
                            <td>
                                   <asp:Label ID="lbl_P_C01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                              <td>
                                   <asp:Label ID="lbl_P_C02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_C03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_C04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_C05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                              <td>
                                   <asp:Label ID="lbl_P_C06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_C07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                               <td>
                                   <asp:Label ID="lbl_P_C08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_C09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>
                                <td>
                                   <asp:Label ID="lbl_P_C10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>         
                               <td>
                                   <asp:Label ID="lbl_P_C11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>           
                                     <td>
                                   <asp:Label ID="lbl_P_C12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                    </td>       

                           </tr>

                           <tr>

                                <td align="center" bgcolor="#CCCCCC"  ><h6>D</h6></td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>RP</h6>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_D01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                             <td>
                                   <asp:Label ID="lbl_P_D02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_D03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td>
                                   <asp:Label ID="lbl_P_D04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td>
                                   <asp:Label ID="lbl_P_D05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td>
                                   <asp:Label ID="lbl_P_D06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                             <td>
                                   <asp:Label ID="lbl_P_D07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_D08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_D09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_D10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>            
                               <td>
                                   <asp:Label ID="lbl_P_D11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>          
                                <td>
                                   <asp:Label ID="lbl_P_D12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>          

                           </tr>
                               <tr>
                                   <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                                   <td bgcolor="#CCCCCC">
                                       <h6>Res</h6>
                                   </td>
                                    <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_1" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_1" runat="server" Text="Valida" Visible="false" />
                                </td>
                                  <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_2" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_2" runat="server" Text="Valida" Visible="false" />
                                </td>
                                   <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_3" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_3" runat="server" Text="Valida" Visible="false" />
                                </td>
                                 <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_4" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_4" runat="server" Text="Valida" Visible="false" />
                                </td>
                                   <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_5" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_5" runat="server" Text="Valida" Visible="false" />
                                </td>
                                   <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_6" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_6" runat="server" Text="Valida" Visible="false" />
                                </td>
                                    <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_7" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_7" runat="server" Text="Valida" Visible="false" />
                                </td>
                                   <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_8" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_8" runat="server" Text="Valida" Visible="false" />
                                </td>
                                   <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_9" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_9" runat="server" Text="Valida" Visible="false" />
                                </td>
                                  <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_10" runat="server" Text="Valida" Visible="false" />
                                </td>
                                  <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_11" runat="server" Text="Valida" Visible="false" />
                                </td>
                                   <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_CD_12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_CD_12" runat="server" Text="Valida" Visible="false" />
                                </td>
                           </tr>
                               <tr>

                                <td align="center" colspan="14"  ><br /></td>
                               
                           </tr>
                            <tr>
                                <td align="center" bgcolor="#CCCCCC">
                                    <h4>&nbsp;</h4>
                                </td>
                                <td bgcolor="#CCCCCC">
                                    <h4>ID</h4>
                                </td>
                               <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_1" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_1" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_1" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_2" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_3" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_4" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_5" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_6" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_7" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_8" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_9" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_10" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_11" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_EF_12" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_EF_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_EF_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                           </tr>
                            <tr>

                              <td align="center" bgcolor="#CCCCCC"  ><h6>E</h6></td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>N1</h6>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_E01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_E02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_E03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td>
                                   <asp:Label ID="lbl_P_E04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lbl_P_E05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                               <td>
                                   <asp:Label ID="lbl_P_E06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td>
                                   <asp:Label ID="lbl_P_E07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                 <td>
                                   <asp:Label ID="lbl_P_E08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                             <td>
                                   <asp:Label ID="lbl_P_E09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>
                                  <td>
                                   <asp:Label ID="lbl_P_E10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>           
                                 <td>
                                   <asp:Label ID="lbl_P_E11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>          
                                   <td>
                                   <asp:Label ID="lbl_P_E12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                </td>        

                           </tr>
                             <tr>

                               <td align="center" bgcolor="#CCCCCC"  ><h6>F</h6></td>
                               
                               <td bgcolor="#CCCCCC">
                                   <h6>RP</h6>
                                 </td>
                               <td>
                                   <asp:Label ID="lbl_P_F01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                              <td>
                                   <asp:Label ID="lbl_P_F02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                                 <td>
                                   <asp:Label ID="lbl_P_F03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                               <td>
                                   <asp:Label ID="lbl_P_F04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                                <td>
                                   <asp:Label ID="lbl_P_F05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                             <td>
                                   <asp:Label ID="lbl_P_F06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                               <td>
                                   <asp:Label ID="lbl_P_F07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                               <td>
                                   <asp:Label ID="lbl_P_F08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                                 <td>
                                   <asp:Label ID="lbl_P_F09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>
                                 <td>
                                   <asp:Label ID="lbl_P_F10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>           
                                <td>
                                   <asp:Label ID="lbl_P_F11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>         
                                  <td>
                                   <asp:Label ID="lbl_P_F12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                 </td>      
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               <td bgcolor="#CCCCCC">
                                   <h6>Res</h6>
                               </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_1" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_1" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_2" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_2" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_3" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_3" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_4" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_4" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_5" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_5" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_6" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_6" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_7" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_7" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_8" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_8" runat="server" Text="Valida" Visible="false" />
                                </td>
                              <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_9" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_9" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_10" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_11" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_EF_12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_EF_12" runat="server" Text="Valida" Visible="false" />
                                </td>
                           </tr>
                           <tr>
                               <td align="center" colspan="14"><br /></td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               <td bgcolor="#CCCCCC">
                                   <h4>ID</h4>
                               </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_1" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_1" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_1" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_2" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_2" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_3" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_3" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_4" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_4" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_5" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_5" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_6" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_6" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_7" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_7" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_8" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_8" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_9" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_9" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_10" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_10" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                              <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_11" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_11" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                             <td bgcolor="#CCCCCC">
                                     <asp:Label ID="lbl_GH_12" runat="server" Font-Bold="True" Font-Size="12pt" Visible="False" Width="100px"></asp:Label>
                                     <br />
                                     <asp:Label ID="lblLugar_GH_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                      <br /> <asp:Label Font-Size="8px" ID="lblFIS_GH_12" runat="server" Text="Label" Visible="False"></asp:Label>
                                </td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6>G</h6></td>
                               <td bgcolor="#CCCCCC">
                                   <h6>N1</h6>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_G01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                 <td>
                                   <asp:Label ID="lbl_P_G04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                 <td>
                                   <asp:Label ID="lbl_P_G08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                 <td>
                                   <asp:Label ID="lbl_P_G10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_G11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                 <td>
                                   <asp:Label ID="lbl_P_G12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC"><h6>H</h6></td>
                               <td bgcolor="#CCCCCC">
                                   <h6>RP</h6>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H01" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H02" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H03" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H04" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H05" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_H06" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H07" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_H08" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label ID="lbl_P_H09" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                               <td>
                                   <asp:Label ID="lbl_P_H12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                               </td>
                           </tr>
                           <tr>
                               <td align="center" bgcolor="#CCCCCC">&nbsp;</td>
                               <td bgcolor="#CCCCCC">
                                   <h6>Res</h6>
                               </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_1" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_1" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_2" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_2" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_3" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_3" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_4" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_4" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_5" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_5" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_6" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_6" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_7" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_7" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_8" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_8" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_9" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_9" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_10" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_10" runat="server" Text="Valida" Visible="false" />
                                </td>
                               <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_11" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_11" runat="server" Text="Valida" Visible="false" />
                                </td>
                                <td>
                                  <asp:Label  Font-Size="8.5px" ID="lblResultado_P_GH_12" runat="server" Font-Bold="True" Text="Label" Visible="False"></asp:Label>
                                  <br />
                                  <asp:CheckBox ID="chkValida_P_GH_12" runat="server" Text="Valida" Visible="false" />
                                </td>
                           </tr>
                       </table> 

                           </asp:Panel>
                        


                       </div>
               </div> 
         <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="btn btn-primary" OnClick="btnRegresar_Click" Width="100px" />

     </div>

       <iframe name="print_frame" width="0" height="0" frameborder="0" src="about:blank"></iframe>
          <script type="text/javascript">

                       

					       function seleccionar_todo() {//Funcion que permite seleccionar todos los checkbox 

					           form = document.forms[0];

					           for (i = 0; i < form.elements.length; i++) {
					               if (form.elements[i].type == "checkbox") {					            
					                       form.elements[i].checked = 1;
					               }
					           }
					       }

					       function desmarcar_todo() {//Funcion que permite desmarcar todos los checkbox

					           form = document.forms[0];

					           for (i = 0; i < form.elements.length; i++) {
					               if (form.elements[i].type == "checkbox") {					                   
					                       form.elements[i].checked = 0;
					               }
					           }
					       }


					     

 

 

     
	</script>
                        

                       </asp:Content>

   

