<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParentescoAlegadoEdit.aspx.cs" Inherits="WebLab.ParentescosAlegados.ParentescoAlegadoEdit" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
      <div align="left" style="width:1000px">
     
       
		<table width="500px" align="center" class="myTabla">
			<tr>
						<td><b  class="mytituloTabla"> Parentescos Alegados</b></td>
						<td align="right"> </td>
					</tr>
			<tr>
						<td colspan="2">    <hr class="hrTitulo" /></td>
					</tr>
			<tr>
			<td class="myLabelIzquierda" >Nombre:</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="400px" CssClass="myTexto" 
                        TabIndex="1" ToolTip="Ingrese el nombre del parentesco" MaxLength="50"></asp:TextBox>
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
                                                PostBackUrl="ParentescoAlegadoList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" CssClass="myButton" ToolTip="Haga clic aquí para guardar el parentesco" />
                </td>
			</tr>
			</table>

	
	</div>
	</asp:Content>
