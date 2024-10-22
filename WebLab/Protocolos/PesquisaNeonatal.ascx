<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PesquisaNeonatal.ascx.cs" Inherits="WebLab.Protocolos.PesquisaNeonatal" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
  <script type="text/javascript">
      $(function () {
          $("#<%=txtFechaExtraccion.ClientID %>").datepicker({
              showOn: 'button',
              buttonImage: '../App_Themes/default/images/calend1.jpg',
              buttonImageOnly: true
          });
      });


      $(function () {
          $("#<%=txtFechaNacimientoParentesco.ClientID %>").datepicker({
              showOn: 'button',
              buttonImage: '../App_Themes/default/images/calend1.jpg',
              buttonImageOnly: true
          });
      });
      
     </script>  
<div>
<table class="mytable1"  style="width:100%;">
    <tr>
    <td class="myLabel">
            DNI Materno:<asp:RequiredFieldValidator 
                         ID="rfdniparentesco" runat="server" ErrorMessage="DNI Materno" 
                         ValidationGroup="0" ControlToValidate="txtDniMaterno">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
            <asp:TextBox ID="txtDniMaterno"  runat="server" CssClass="myTexto" Width="80px" 
                TabIndex="100"></asp:TextBox></td>
               <td class="myLabel">

Apellido Materno:<asp:RequiredFieldValidator 
                         ID="rfapellidoparentesco" runat="server" ErrorMessage="Apellido Materno" 
                         ValidationGroup="0" ControlToValidate="txtApellidoMaterno">*</asp:RequiredFieldValidator>
        </td>
               <td class="myLabel">
            <asp:TextBox ID="txtApellidoMaterno" runat="server" CssClass="myTexto" TabIndex="101"></asp:TextBox>
        </td>
               <td class="myLabel">
                   Nombres Materno:<asp:RequiredFieldValidator 
                         ID="rfnombreparentesco" runat="server" ErrorMessage="Nombre Materno" 
                         ValidationGroup="0" ControlToValidate="txtNombreParentesco">*</asp:RequiredFieldValidator>
        </td>
               <td class="myLabel">

            <asp:TextBox ID="txtNombreParentesco" runat="server" CssClass="myTexto" TabIndex="102"></asp:TextBox>
        </td>
     <td class="myLabel">
                   Fecha Nac. Materno:</td>
          <td class="myLabel">
                    <input id="txtFechaNacimientoParentesco" runat="server" type="text" maxlength="10" 
                        style="width: 80px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="103" class="myTexto" 
                                title="Ingrese la fecha de nacimiento materno"  /></td>
        
    </tr>

    <tr>
    <td class="myLabel">
            Lugar de Control:<asp:RangeValidator 
                ID="rvLugarControl" runat="server" ControlToValidate="ddlLugarControl" 
                ErrorMessage="Lugar de control" MaximumValue="999999" MinimumValue="1" 
                Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
        </td>
           <td class="myLabel" colspan="2">    
               <asp:DropDownList ID="ddlLugarControl" runat="server" Width="200px" 
                   TabIndex="104">
               </asp:DropDownList>
        </td>
          <td class="myLabel">
            Solicitante Externo:</td>
          <td class="myLabel" colspan="2">
            <asp:Label ID="lblMedicoSolicitante"  runat="server" Text=""></asp:Label></td>
     <td class="myLabel">
           Nro. Tarjeta Cartón:</td>
          <td class="myLabel">
              <asp:TextBox  class="myTexto" MaxLength="8"  ID="txtNumeroTarjeta" runat="server" 
                  Width="60px" TabIndex="105"></asp:TextBox>

        </td>
        
    </tr>

</table>
<div align="right">
  <asp:CustomValidator ID="cvMotivoRepeticion" runat="server" 
        ControlToValidate="ddlMotivoRepeticion" ErrorMessage="Motivo Repetición" Text="Debe seleccionar motivo de repetición"
        onservervalidate="cvMotivoRepeticion_ServerValidate" ValidationGroup="0" 
        Font-Size="8pt"></asp:CustomValidator> 

                <asp:CustomValidator ID="cvNumeroDesde" runat="server" 
                                ErrorMessage="Numero de Origen" 
                                onservervalidate="cvNumeros_ServerValidate" 
        ValidationGroup="0" Font-Size="8pt" 
                                >Nro. Cartón: Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>

                                </div>
<table class="mytable1" style="width:100%;">
    <tr>
        <td class="myLabel">
            Hora de Nacimiento:</td>
        <td class="myLabelIzquierda">
                     <input id="txtHoraNacimiento" runat="server" type="text" maxlength="5" 
                        style="width: 40px"   onblur="valHora(this)"              
                        onkeyup="mascara(this,':',patron,true)" tabindex="106" class="myTexto" 
                                title="Ingrese la hora de nacimiento"  /><asp:RequiredFieldValidator 
                         ID="rfHoraNacimiento" runat="server" ErrorMessage="Hora Nacimiento" 
                         ValidationGroup="0" ControlToValidate="txtHoraNacimiento">*</asp:RequiredFieldValidator>
        </td>
     <td class="myLabel">
            Edad Gestacional (sem.):</td>
        <td class="myLabelIzquierda">
                      <input id="txtEdadGestacional" runat="server" type="text" maxlength="2" 
                          style="width: 30px"  onblur="valNumeroSinPunto(this)" tabindex="107" class="myTexto" 
                                title="Ingrese la edad gestacional en semanas" /><asp:RequiredFieldValidator 
                         ID="rfEdadGestacional" runat="server" ErrorMessage="Edad Gestacional" 
                          ValidationGroup="0" ControlToValidate="txtEdadGestacional">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
            Peso (gr.):</td>
        <td class="myLabelIzquierda">
                      <input id="txtPeso" runat="server" type="text" maxlength="4"  
                          style="width: 40px"  onblur="valNumeroSinPunto(this)" tabindex="108" class="myTexto" 
                                title="Ingrese el peso" /><asp:RequiredFieldValidator 
                         ID="rfPeso" runat="server" ErrorMessage="Peso" ValidationGroup="0" 
                          ControlToValidate="txtPeso">*</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="myLabel">
            Fecha y Hora Extracción:<asp:RequiredFieldValidator 
                         ID="rfPeso0" runat="server" ErrorMessage="Fecha Extraccion" 
                ValidationGroup="0" ControlToValidate="txtFechaExtraccion">*</asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator 
                         ID="rfPeso1" runat="server" ErrorMessage="Hora Extraccion" 
                ValidationGroup="0" ControlToValidate="txtHoraExtraccion">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabelIzquierda">
                    <input id="txtFechaExtraccion" runat="server" type="text" maxlength="10" 
                        style="width: 80px"  onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="109" class="myTexto" 
                                title="Ingrese la fecha de extracción de la muestra"  /><input 
                   id="txtHoraExtraccion" runat="server" type="text" maxlength="5" 
                        style="width: 40px"   onblur="valHora(this)"              
                        onkeyup="mascara(this,':',patron,true)" tabindex="110" class="myTexto" 
                                title="Ingrese la hora de extracción de la muestra"  /></td>
       <td class="myLabel">
            Primera Muestra:<asp:RequiredFieldValidator 
                         ID="rfPrimeraMuestra" runat="server" 
                ErrorMessage="Primera Muestra" ValidationGroup="0" 
                ControlToValidate="rdbPrimeraMuestra">*</asp:RequiredFieldValidator>
        </td>
           <td class="myLabel">
              <anthem:RadioButtonList ID="rdbPrimeraMuestra" runat="server" 
                  RepeatDirection="Horizontal" TabIndex="111" AutoCallBack="True" 
                   onselectedindexchanged="rdbPrimeraMuestra_SelectedIndexChanged">
                  <asp:ListItem Value="1">Si</asp:ListItem>
                  <asp:ListItem Value="0">No</asp:ListItem>
              </anthem:RadioButtonList>
               </td>
        
     <td class="myLabel">
            Motivo Repetición:</td>
        <td class="myLabelIzquierda">
        <anthem:DropDownList ID="ddlMotivoRepeticion" runat="server" TabIndex="112">
        </anthem:DropDownList>
            
        </td>
    </tr>
    <tr>
        <td class="myLabel">
            Ingesta Leche 24 hs.:<asp:RequiredFieldValidator 
                         ID="rfIngestaLeche" runat="server" ErrorMessage="Ingesta Leche" 
                ValidationGroup="0" ControlToValidate="rdbIngestaLeche24Horas">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
             <asp:RadioButtonList ID="rdbIngestaLeche24Horas" runat="server" 
                 RepeatDirection="Horizontal" TabIndex="113">
                 <asp:ListItem Value="1">Si</asp:ListItem>
                 <asp:ListItem Value="0">No</asp:ListItem>
             </asp:RadioButtonList>
        </td>
        <td class="myLabel">
            Tipo de Alimentación:<asp:RequiredFieldValidator 
                         ID="rfTipoAlimentacion" runat="server" 
                ErrorMessage="Tipo Alimentacion" ValidationGroup="0" 
                ControlToValidate="rdbTipoAlimentacion">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
                 <asp:RadioButtonList ID="rdbTipoAlimentacion" runat="server" 
                     RepeatDirection="Horizontal" TabIndex="114">
                     <asp:ListItem>Pecho</asp:ListItem>
                     <asp:ListItem>Biberón</asp:ListItem>
                     <asp:ListItem>Parenteral</asp:ListItem>
                 </asp:RadioButtonList>
        </td>
        <td class="myLabel">
            Antibiótico:<asp:RequiredFieldValidator  ID="rfAntibiotico" runat="server" ErrorMessage="Antibiotico" 
                ValidationGroup="0" ControlToValidate="rdbAntibiotico">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
                     <asp:RadioButtonList ID="rdbAntibiotico" runat="server" 
                         RepeatDirection="Horizontal" TabIndex="115">
                         <asp:ListItem Value="1">Si</asp:ListItem>
                         <asp:ListItem Value="0">No</asp:ListItem>
                     </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
       <td class="myLabel">
            Transfusión:<asp:RequiredFieldValidator 
                         ID="rfTransfusion" runat="server" ErrorMessage="Transfusion" 
                ValidationGroup="0" ControlToValidate="rdbTransfusion">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
            <asp:RadioButtonList ID="rdbTransfusion" runat="server" 
                RepeatDirection="Horizontal" TabIndex="116">
                <asp:ListItem Value="1">Si</asp:ListItem>
                <asp:ListItem Value="0">No</asp:ListItem>
            </asp:RadioButtonList>
        </td>
      <td class="myLabel">
            Corticoides:<asp:RequiredFieldValidator 
                         ID="rfCorticoide" runat="server" ErrorMessage="Corticoide" 
                ValidationGroup="0" ControlToValidate="rdbCorticoide">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
            <asp:RadioButtonList ID="rdbCorticoide" runat="server" 
                RepeatDirection="Horizontal" TabIndex="117">
                <asp:ListItem Value="1">Si</asp:ListItem>
                <asp:ListItem Value="0">No</asp:ListItem>
            </asp:RadioButtonList>
        </td>
     <td class="myLabel">
            Dopamina:<asp:RequiredFieldValidator 
                         ID="rfDopamina" runat="server" ErrorMessage="Dopamina" 
                ValidationGroup="0" ControlToValidate="rdbDopamina">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
              <asp:RadioButtonList ID="rdbDopamina" runat="server" 
                  RepeatDirection="Horizontal" TabIndex="118">
                  <asp:ListItem Value="1">Si</asp:ListItem>
                  <asp:ListItem Value="0">No</asp:ListItem>
              </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
       <td class="myLabel">
            Enfermedad Tiroidea Materna:<asp:RequiredFieldValidator 
                         ID="rfEnfermedadTiroideaMaterna" runat="server" 
                ErrorMessage="Enfermedad Tiroidea Materna" ValidationGroup="0" 
                ControlToValidate="rdbEnfermedadTiroideaMaterna">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
            <asp:RadioButtonList ID="rdbEnfermedadTiroideaMaterna" runat="server" 
                RepeatDirection="Horizontal" TabIndex="119">
                <asp:ListItem Value="1">Si</asp:ListItem>
                <asp:ListItem Value="0">No</asp:ListItem>
            </asp:RadioButtonList>
        </td>
      <td class="myLabel">
            Corticoides Materno:<asp:RequiredFieldValidator 
                         ID="rfCorticoide0" runat="server" 
                ErrorMessage="Corticoide Materno" ValidationGroup="0" 
                ControlToValidate="rdbCorticoideMaterno">*</asp:RequiredFieldValidator>
        </td>
        <td class="myLabel">
            <asp:RadioButtonList ID="rdbCorticoideMaterno" runat="server" 
                RepeatDirection="Horizontal" TabIndex="120">
                <asp:ListItem Value="1">Si</asp:ListItem>
                <asp:ListItem Value="0">No</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td class="myLabel">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
       <td class="myLabel">
            Antecedentes Maternos:</td>
        <td colspan="5" class="myLabel">
            <asp:TextBox ID="txtAntecedenteMaterno" runat="server" TabIndex="121" class="myTexto" 
                Width="600px"></asp:TextBox>
        </td>
    </tr>

       <tr>
       <td align="left">
         <anthem:RadioButtonList ID="rdbSeleccionarItem" CssClass="myLabel" 
               runat="server"  AutoCallBack="True" 
               onselectedindexchanged="rdbSeleccionarItem_SelectedIndexChanged" 
               RepeatDirection="Vertical" Font-Size="8pt">
                                                 <asp:ListItem Value="1">Marcar Todas</asp:ListItem>
                                                 <asp:ListItem Value="0">Desmarcar Todas</asp:ListItem>
                                             </anthem:RadioButtonList>  </td>
        <td align="left" colspan="5" >
           <anthem:CheckBoxList ID="chkItemPesquisa" runat="server" 
        RepeatDirection="Horizontal" Width="500px" Font-Bold="True" 
        TabIndex="122" CssClass="mytable1" Font-Size="12" >
    </anthem:CheckBoxList>                 
        </td>
    </tr>
    </table>

   
      <input type="hidden" runat="server" name="hfFechaRegistro" id="hfFechaRegistro" value="" />                     
<input type="hidden" runat="server" name="hfFechaEnvio" id="hfFechaEnvio" value="" />     
    
         </div>