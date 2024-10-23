<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecomendacionEdit.aspx.cs" Inherits="WebLab.Recomendaciones.RecomendacionEdit" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server" />
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
 
     <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">RECOMENDACION</h3>
                        </div>

				<div class="panel-body">
  
       
		<table width="100%" >
			 
			<tr>
				<td  >Nombre</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="284px"  CssClass="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre de la recomendación"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			<tr>
				<td  style="vertical-align: top"  >Descripción detallada:</td>
				<td  >
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" 
                        Width="100%"  CssClass="form-control input-sm" Height="150px" TabIndex="2" 
                        ToolTip="Ingrese la descripción de la recomendación"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDescripcion" 
                        runat="server" ControlToValidate="txtDescripcion" 
                        ErrorMessage="Descripcion" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			

			

                </table>
                    </div>
                <div class="panel-footer">
                    <table>
                        <tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="RecomendacionList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" 
                        runat="server" 
            Text="Guardar" CssClass="btn btn-primary" Width="100px" onclick="btnGuardar_Click" ValidationGroup="0" 
                        TabIndex="3" ToolTip="Haga clic aquí para guardar" />
                </td>
			</tr>
			<tr>
				<td  >
                                            &nbsp;</td>
				<td   align="right">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        HeaderText="Debe completar los datos marcados como obligatorios:" 
                        ValidationGroup="0" ShowMessageBox="True" ShowSummary="False" />
                </td>
			</tr>
			</table>
                    </div>
        
	</div>
		</div>
	</asp:Content>
