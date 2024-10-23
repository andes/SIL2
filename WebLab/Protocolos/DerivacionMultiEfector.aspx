<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DerivacionMultiEfector.aspx.cs" Inherits="WebLab.Protocolos.DerivacionMultiEfector" MasterPageFile="../Site1.Master" %>


<%@ Register src="ProtocoloList.ascx" tagname="ProtocoloList" tagprefix="uc1" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

    
 <script src="Resources/jquery.min.js" type="text/javascript"></script>
    <link href="Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />
    <script src="Resources/jQuery-ui-1.8.18.min.js" type="text/javascript"></script>

  

  

  
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
   
        <div align="left" style="width:800px;" class="form-inline" >
 <table  width="800px" align="center" >
					<tr>
		<td  rowspan="6">
     <div id="pnlTitulo" runat="server" class="panel panel-default">
                    <div class="panel-heading">
                      <asp:Label Text="Ultimos 10 Protocolos" runat="server" ID="lblTituloLista"></asp:Label>
                        </div>

				<div class="panel-body" style="align-content:center;">
            <uc1:ProtocoloList ID="ProtocoloList1" runat="server" />
                    </div>
         </div>
    
        </td>
		<td  rowspan="6"  > &nbsp;</td>
        <td  rowspan="6"  style="vertical-align: top" >
               <div class="panel panel-primary">
                    <div class="panel-heading"> Recepción de Derivación
  </div>
                    <div class="panel-body">
        <table width="700px">
        

					<tr>
		<td >Efector Derivante:</td><td class="style1">
            <asp:DropDownList ID="ddlEfector" runat="server" class="form-control input-sm" >
            </asp:DropDownList> <asp:RangeValidator ID="rvEfector" runat="server" 
                                ControlToValidate="ddlEfector" ErrorMessage="Efector Derivante" MaximumValue="999999" 
                                MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
        </td>
                                            </tr>

                                            			<tr>
		<td >Nro. Protocolo:</td><td ><asp:TextBox ID="txtNumeroProtocolo" runat="server" 
                                                               class="form-control input-sm"  Width="100px" TabIndex="1"></asp:TextBox>
                                                            
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                                ControlToValidate="txtNumeroProtocolo" ErrorMessage="Nro. Protocolo" 
                                                                ValidationGroup="0">*</asp:RequiredFieldValidator>
                 <asp:LinkButton ID="lnkBuscar" runat="server" CssClass="btn btn-info" OnClick="btnBuscar_Click" ValidationGroup="0"   Width="90px" >
                                             <span class="glyphicon glyphicon-search"></span>&nbsp;Buscar</asp:LinkButton>                                           
                                                            </td>
                                            </tr>

                                            			<tr>
		<td >&nbsp;</td><td >
                                                            <div align="right" >
                                                                Seleccionar:
                                                                <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" ValidationGroup="0">Todas</asp:LinkButton>
                                                                &nbsp;
                                                                <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" ValidationGroup="0">Ninguna</asp:LinkButton>
                                                            </div>
                                                            </td>
                                            </tr>

                                            			<tr>
		<td   colspan="2">
            <asp:GridView ID="gvProtocolosDerivados" runat="server" CssClass="table table-bordered bs-table"  AutoGenerateColumns="False" DataKeyNames="idDerivacion" EmptyDataText="No se encontraron derivaciones pendientes de ingresar para los filtros seleccionados">
                <Columns>
                    <asp:BoundField DataField="fecha" HeaderText="Fecha Protocolo" />
                    <asp:BoundField DataField="numero" HeaderText="Numero" />
                    <asp:BoundField DataField="paciente" HeaderText="Paciente" />
                    <asp:BoundField DataField="Determinacion" HeaderText="Determinacion" />
                       <asp:TemplateField HeaderText="Seleccionar" >
                                                        <ItemTemplate>
                                                         <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                                                     </ItemTemplate>
                                                     <ItemStyle Width="5%" 
                                                            HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#ffffcc" />
        <EmptyDataRowStyle forecolor="Red" CssClass="table table-bordered" />
        
                                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                                                <PagerStyle BackColor="#E6E6E6" ForeColor="Black" HorizontalAlign="Right" />
                                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="#333333" />
                                                                
            </asp:GridView>
                                                            </td>
                                            </tr>

                                            			<tr>
		<td  >&nbsp;</td><td class="style7">
                 
                                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                                HeaderText="De completar los datos requeridos:" ShowMessageBox="True" 
                                                                ValidationGroup="0" />
                                                            
                                                            </td>
                                            </tr>

                                            			 
        </table>

                        </div>
                     <div class="panel-footer">
                                  <asp:Button ID="btnIngresar" runat="server" onclick="btnEnviar_Click" 
                                  Text="Ingresar Pedido" CssClass="btn btn-primary" Width="150px" />
                        </div>
                   </div>
        </td>
		
                                            </tr>
                                            </table>
</div>
		    
 </asp:Content>