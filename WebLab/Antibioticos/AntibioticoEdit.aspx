<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AntibioticoEdit.aspx.cs" Inherits="WebLab.Antibioticos.AntibioticoEdit" MasterPageFile="~/Site1.Master" %>

 <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
     <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">ANTIBIOTICO</h3>
                        </div>

				<div class="panel-body">
       
		<table width="500px" align="center"  ">
			 
			<tr>
				<td class="myLabelIzquierda" >Descripción:</td>
				<td>
                    <asp:TextBox ID="txtNombre" runat="server" Width="350px"  class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre del antibiotico" MaxLength="250"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="Descripción" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
				<td>
                    &nbsp;</td>
			</tr>
			<tr>
				<td class="myLabelIzquierda" >Abreviatura:</td>
				<td>
                    <asp:TextBox ID="txtAbreviatura" runat="server" Width="150px"  class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese la abreviatura" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvAbreviatura" 
                        runat="server" ControlToValidate="txtAbreviatura" 
                        ErrorMessage="Abreviatura" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
				<td>
                    &nbsp;</td>
			</tr>
            <%--</form>--%>
			
			<tr>
				<td   colspan="2"><hr /></td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="AntibioticoList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right"><asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar"  CssClass="btn btn-primary" Width="100px" TabIndex="4" 
                        ToolTip="Haga clic aquí para guardar los datos" ValidationGroup="0" />
                </td>
				<td   align="right">&nbsp;</td>
			</tr>
			<tr>
				<td  >
                                            &nbsp;</td>
				<td   align="right">
			
	
                 <asp:ValidationSummary ID="vs" runat="server" 
                     HeaderText="Debe completar los datos marcados como requeridos:" 
                     ShowMessageBox="True" ShowSummary="False" ValidationGroup="0" />
                </td>
				<td   align="right">
			
	
                    &nbsp;</td>
			</tr>
			</table>
</div>
       </div>
	
	</div>
	</asp:Content>

