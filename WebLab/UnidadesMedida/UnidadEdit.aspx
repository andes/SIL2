<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnidadEdit.aspx.cs" Inherits="WebLab.UnidadesMedida.UnidadEdit" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
  <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">UNIDAD DE MEDIDA</h3>
                        </div>

				<div class="panel-body">
     
       
		<table width="500px" align="center"  >
			 <tr>
			<td class="myLabelIzquierda" >Nombre:</td>
				<td colspan="2"  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="284px" CssClass="form-control input-sm" 
                        TabIndex="1" ToolTip="Ingrese el nombre de la unidad de medida" 
                        MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			 
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="UnidadList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td  align="right"><asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" CssClass="btn btn-primary" Width="100px" TabIndex="2" 
                        ToolTip="Haga clic aquí para guardar" />
                </td>
				<td  align="right">&nbsp;</td>
			</tr>
			</table>

	</div>
       </div>
      </div>
	</asp:Content>