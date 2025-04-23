<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObservacionEdit.aspx.cs" Inherits="WebLab.Observaciones.ObservacionEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server" >
 
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
     <div align="left" style="width: 1000px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">OBSERVACIONES CODIFICADAS</h3>
                        </div>

				<div class="panel-body">
       
		<table    >
		 
			 
			<tr>
				<td  style="vertical-align: top"  >Tipo de Servicio:</td>
				<td  colspan="3"  >
                  <asp:DropDownList ID="ddlTipoServicio" runat="server" CssClass="form-control input-sm" 
                        TabIndex="2" ToolTip="Seleccione el servicio">
                    </asp:DropDownList>
                    <asp:RangeValidator ID="rvTipoServicio" runat="server" 
                        ControlToValidate="ddlTipoServicio" ErrorMessage="Tipo de Servicio" 
                        MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator></td>
			</tr>
		
			<tr>
				<td  style="width: 103px" >Código:</td>
				<td  style="width: 328px" >
                    <asp:TextBox ID="txtAbreviatura" runat="server" Width="400px" CssClass="form-control input-sm"
                        TabIndex="2" ToolTip="Ingrese el codigo" MaxLength="100"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvAbreviatura" 
                        runat="server" ControlToValidate="txtAbreviatura" 
                        ErrorMessage="Codigo" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
				<td colspan="2">
                    &nbsp;</td>
			</tr>
        
			<tr>
				<td  style="vertical-align: top"  >Descripción:</td>
				<td colspan="3"  >
                    <asp:TextBox ID="txtNombre"  runat="server" Width="600px" CssClass="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese la descripcion a imprimir" MaxLength="1000" 
                        TextMode="MultiLine" Rows="10"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="Descripcion" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
		 
			
			</table>
                    </div>
     		<div class="panel-footer">
                 <table>
                     <tr>
				<td style="width: 250px" colspan="2"  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="ObservacionList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right" colspan="2">
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" CssClass="btn btn-primary" Width="100px" TabIndex="3" 
                        ToolTip="Haga clic aquí para guardar los datos" ValidationGroup="0" />
                </td>
			</tr>
			<tr>
				<td colspan="4"  >
			
	
                 <asp:ValidationSummary ID="vs" runat="server" 
                     HeaderText="Debe completar los datos marcados como requeridos:" 
                     ShowMessageBox="True" ShowSummary="False" ValidationGroup="0" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtNombre" ErrorMessage="CustomValidator" OnServerValidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                </td>
			</tr>
                 </table>
                 </div>
       </div>

	</div>
    
	</asp:Content>

