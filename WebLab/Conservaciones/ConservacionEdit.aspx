<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConservacionEdit.aspx.cs" Inherits="WebLab.Conservaciones.ConservacionEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head" />
    
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
    
      <div align="left" style="width: 520px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">Soporte/Conservacion</h3>
                        </div>

				<div class="panel-body">	
     
       
		<table width="500px" align="center" 
            >
			<tr>

		

			<td  >Descripción:</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="400px"  class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			<tr>
				<td   colspan="2"><hr /></td>
			</tr>
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="ConservacionList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" CssClass="btn btn-primary" Width="100px" ToolTip="Haga clic aquí para guardar" />
                </td>
			</tr>
			</table>
</div>
	</div>
	</div>
	</asp:Content>
