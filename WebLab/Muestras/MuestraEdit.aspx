<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MuestraEdit.aspx.cs" Inherits="WebLab.Muestras.MuestraEdit" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head"/>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
              
    <div  style="width: 900px" class="form-inline" >
     
                     <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
                       TIPO DE MUESTRA
    
  </div>
                    <div class="panel-body">
     
       
		<table width="500px" align="left" >
			<tr>
			<td class="myLabelIzquierda" >Nombre:</td>
				<td align="left" colspan="2"  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="350px" class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre del tipo de muestra" 
                        MaxLength="500"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			<tr>
			<td class="myLabelIzquierda" >Codigo / Whonet:</td>
				<td align="left" colspan="2"  >
                    <asp:TextBox ID="txtCodigo" runat="server" Width="100px" class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre del tipo de muestra" 
                        MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre0" 
                        runat="server" ControlToValidate="txtCodigo" 
                        ErrorMessage="RequiredFieldValidator" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             <asp:CustomValidator ID="CustomValidator1" runat="server" 
                        ControlToValidate="txtCodigo" 
                        ErrorMessage="El código ingresado ya existe. Verifique" 
                        onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                             </td>
			</tr>
			<tr>
			<td class="myLabelIzquierda" >Tipo:</td>
				<td align="left" colspan="2"  >
                    <asp:RadioButtonList ID="rdbTipo" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0">Ambos</asp:ListItem>
                        <asp:ListItem Value="1">De Pacientes</asp:ListItem>
                        <asp:ListItem Value="2">De No Pacientes</asp:ListItem>
                    </asp:RadioButtonList>
                             </td>
			</tr>
		
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="MuestraList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" Width="100px"
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" class="btn btn-primary"
                        ToolTip="Haga clic aquí para guardar el tipo de muestra" ValidationGroup="0" />
                </td>
				<td   align="right">
                    &nbsp;</td>
			</tr>
			</table>
                        </div>
                         </div>
	
	</div>
	</asp:Content>
