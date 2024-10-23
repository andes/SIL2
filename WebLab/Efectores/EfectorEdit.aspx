<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EfectorEdit.aspx.cs" Inherits="WebLab.Efectores.EfectorEdit" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
</asp:Content>


 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <br />   &nbsp;
  <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">Efector</h3>
                        </div>

				<div class="panel-body">
  
       
		<table >
			
			<tr>
				<td class="myLabelIzquierda" >Nombre:</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="284px" class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre del Efector" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			 <tr>
				<td class="myLabelIzquierda" >Zona:</td>
				<td  >
                    <asp:DropDownList ID="ddlZona" runat="server" class="form-control input-sm"
                        TabIndex="2" ToolTip="Seleccione la zona">
                    </asp:DropDownList>
                    <asp:RangeValidator ID="rvTipoServicio" runat="server" 
                        ControlToValidate="ddlZona" ErrorMessage="ddlZona" 
                        MaximumValue="999999" MinimumValue="1" Type="Integer" ValidationGroup="0">*</asp:RangeValidator>
                             </td>
			</tr>
			 
			 <tr>
				<td class="myLabelIzquierda" >Privado:</td>
				<td  >
                    <asp:CheckBox ID="chkPrivado" runat="server" Checked="True"  Text="Si" />
                             </td>
			</tr>
			 
			<tr>
				<td   colspan="2"><hr /></td>
			</tr>
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="EfectorList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right"><asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" CssClass="btn btn-primary" Width="100px" TabIndex="4" 
                        ToolTip="Haga clic aquí para guardar los datos" ValidationGroup="0" />
                </td>
			</tr>
			<tr>
				<td  >
                                            &nbsp;</td>
				<td   align="right">
			
	
                 <asp:ValidationSummary ID="vs" runat="server" 
                     HeaderText="Debe completar los datos marcados como requeridos:" 
                     ShowMessageBox="True" ShowSummary="False" ValidationGroup="0" />
                </td>
			</tr>
			</table>

	</div>
		</div>
      </div>
	</asp:Content>
