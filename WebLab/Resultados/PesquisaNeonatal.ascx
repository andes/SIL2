<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PesquisaNeonatal.ascx.cs" Inherits="WebLab.Resultados.PesquisaNeonatal" %>



<div style="width: 600px">
<table class="mytable1"  style="width:600px;">
    <tr>
    <td class="myLabelIzquierdaFondo">
            DNI Materno:</td>
        <td class="myLabel">
            <asp:Label ID="lblDniMaterno"  runat="server" Text="Label"></asp:Label></td>
               <td class="myLabelIzquierdaFondo">

           Numero Tarjeta Cartón:</td>
               <td >

            <asp:Label CssClass="myLabelIzquierda" ID="lblNumeroTarjeta" runat="server" Text="Label"></asp:Label>

        </td>
        
    </tr>

    <tr>
    <td class="myLabelIzquierdaFondo">

Apellido y Nombres Materno:</td>
        <td class="myLabel">

            <asp:Label ID="lblApellidoMaterno" runat="server" Text="Label"></asp:Label>

            <asp:Label ID="lblNombreParentesco" runat="server" Text="Label"></asp:Label>

        </td>
               <td class="myLabelIzquierdaFondo">

                   Fecha Nac. Materno:</td>
               <td class="myLabel">

            <asp:Label ID="lblFechaNacimientoParentesco" runat="server" Text="Label"></asp:Label>

        </td>
        
    </tr>

    <tr>
    <td class="myLabelIzquierdaFondo">
            Lugar de Control:</td>
           <td class="myLabel">    
            <asp:Label ID="lblLugarControl"  runat="server" Text="Label"></asp:Label>    <asp:Label ID="lblIdLugarControl"  runat="server" Text="Label" Visible="false"></asp:Label>
            </td>
        <td class="myLabelIzquierdaFondo">
            Medico Solicitante:<asp:Label ID="lblApellidoPaterno" Visible="false" runat="server" Text="Label"></asp:Label></td>
          <td class="myLabel">
            <asp:Label ID="lblMedicoSolicitante"  runat="server" Text="Label"></asp:Label></td>
        
    </tr>

    </table>
<br />
<table class="mytable1" style="width:600px;">
    <tr>
        <td class="myLabelIzquierdaFondo">
            Hora de Nacimiento:</td>
        <td class="myLabel">
            <asp:Label ID="lblHoraNacimiento"  runat="server" Text="Label"></asp:Label>
        </td>
     <td class="myLabelIzquierdaFondo">
            Edad Gestacional (sem.):</td>
        <td class="myLabel">
            <asp:Label ID="lblEdadGestacional"  runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="myLabelIzquierdaFondo">
            Fecha Hora Extracción:</td>
        <td class="myLabel">
            <asp:Label ID="lblFechaHoraExtraccion"  runat="server" Text="Label"></asp:Label></td>
       <td class="myLabelIzquierdaFondo">
            Primera Muestra:</td>
           <td class="myLabel">
            <asp:Label ID="lblPrimeraMuestra"  runat="server" Text="Label"></asp:Label>
               </td>
        </tr>
     <tr>
        <td class="myLabelIzquierdaFondo">
            Peso (gr.):</td>
        <td class="myLabel">
            <asp:Label ID="lblPeso"  runat="server" Text="Label"></asp:Label>
        </td>
        <td class="myLabelIzquierdaFondo">
            Motivo Repetición:</td>
        <td class="myLabel">
            <asp:Label ID="lblMotivoRepeticion"  runat="server" Text="Label"></asp:Label>
              
        </td>
    </tr>
    <tr>
        <td class="myLabelIzquierdaFondo">
            Ingesta Leche 24 Hs.:</td>
        <td class="myLabel">
            <asp:Label ID="lblIngestaLeche"  runat="server" Text="Label"></asp:Label>
        </td>
        <td class="myLabelIzquierdaFondo">
            Tipo Alimentación:</td>
        <td class="myLabel">
            <asp:Label ID="lblTipoAlimentacion"  runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
       <td class="myLabelIzquierdaFondo">
            Transfusión:</td>
        <td class="myLabel">
            <asp:Label ID="lblTransfusion"  runat="server" Text="Label"></asp:Label>
        </td>
      <td class="myLabelIzquierdaFondo">
            Corticoides:</td>
        <td class="myLabel">
            <asp:Label ID="lblCorticoides"  runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
       <td class="myLabelIzquierdaFondo">
            Antibiótico:</td>
        <td class="myLabel">
            <asp:Label ID="lblAntibiotico"  runat="server" Text="Label"></asp:Label>
        </td>
      <td class="myLabelIzquierdaFondo">
            Dopamina:</td>
        <td class="myLabel">
            <asp:Label ID="lblDopamina"  runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
       <td class="myLabelIzquierdaFondo">
            Enfermedad Tiroidea Materna:</td>
        <td class="myLabel">
            <asp:Label ID="lblEnfermedadTiroidea"  runat="server" Text="Label"></asp:Label>
        </td>
      <td class="myLabelIzquierdaFondo">
            Corticoides Materno:</td>
        <td class="myLabel">
            <asp:Label ID="lblCorticoidesMaterno"  runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    <tr>
       <td class="myLabelIzquierdaFondo">
            Antecedentes Maternos:</td>
        <td colspan="3" class="myLabel">
            <asp:Label ID="lblAntecedentesMaterno"  runat="server" Text="Label"></asp:Label>
        </td>
    </tr>
    </table>
<hr />
    <asp:GridView ID="gvAlarmas" runat="server" AutoGenerateColumns="False" CssClass="myLabel"
        EmptyDataText="Sin alarmas" Width="600px">
        <Columns>
            <asp:BoundField DataField="descripcion" HeaderText="Alarmas" />
        </Columns>
        <HeaderStyle BackColor="#F2F2F2" ForeColor="Red" />
    </asp:GridView>        
        </div>