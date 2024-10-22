<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DerivacionProcesa.aspx.cs" Inherits="WebLab.Protocolos.DerivacionProcesa" MasterPageFile="../Site1.Master" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">


  
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    

    <br />   &nbsp;
       <input id="hfnumeroProtocolo" type="hidden" runat="server" /> 
       <input id="hffechaProtocolo" type="hidden" runat="server" /> 
       <input id="hfidEfector" type="hidden" runat="server" /> 
       <input id="hfanalisis" type="hidden" runat="server" /> 
       <input id="hfsala" type="hidden" runat="server" /> 
       <input id="hfcama" type="hidden" runat="server" /> 
       <input id="hfdiagnostico" type="hidden" runat="server" /> 

       <input id="hfapellido" type="hidden" runat="server" /> <input id="hfnombre" type="hidden" runat="server" /> 
       <input id="hffechaNacimiento" type="hidden" runat="server" /> <input id="hfsexo" type="hidden" runat="server" /> 
       <input id="hfreferencia" type="hidden" runat="server" /> <input id="hfinformacionContacto" type="hidden" runat="server" /> 

       <input id="hfapellidoParentesco" type="hidden" runat="server" />
       <input id="hfnombreParentesco" type="hidden" runat="server" /> 
       <input id="hffechaNacimientoParentesco" type="hidden" runat="server" /> 
       <input id="hfnumeroDocumentoParentesco" type="hidden" runat="server" /> 

        <div align="center" style="width:1000px; height:500pt;"> 
        <img src="../App_Themes/default/images/ico_alerta.png" />
         <br />  
        <asp:TextBox runat="server"   ID="lblMensaje" ReadOnly="true" TextMode="MultiLine" Rows="10" Width="600px" />
       
       
        <br /> &nbsp;   <br />   
            <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="idPaciente" onrowcommand="gvLista_RowCommand" 
                onrowdatabound="gvLista_RowDataBound" Width="600px" CellPadding="4" 
                ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvLista_SelectedIndexChanged">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" />
                    <asp:BoundField DataField="apellido" HeaderText="Apellido" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac." />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Seleccionar" runat="server" CommandName="Seleccionar" 
                            Text="Actualizar y Seleccionar" CssClass="myLinkRojo"                                                                          />
                        </ItemTemplate>
                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <br />
            <asp:Button ID="btnGenerarPaciente" runat="server" CssClass="myButton" Text="Generar Nuevo Paciente Temporal" Width="250px" ToolTip="Genera un nuevo paciente temporal con los datos de origen"
                onclick="btnGenerarPaciente_Click" />
            <br />
            <br />
            <br />
    <asp:HyperLink CssClass="myLink"  ID="hplRegresar" runat="server">Regresar</asp:HyperLink>
</div>
		    
 </asp:Content>