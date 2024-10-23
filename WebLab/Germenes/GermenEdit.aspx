<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GermenEdit.aspx.cs" Inherits="WebLab.Germenes.GermenEdit" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head" />
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
  <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"> MICROORGANISMOS</h3>
                        </div>

				<div class="panel-body">
       
		<table width="500px" align="center"  >
		 
			<tr>
			<td class="myLabelIzquierda" >Codigo:</td>
				<td  >
                   <anthem:TextBox ID="txtCodigo" runat="server" MaxLength="50" 
                                Width="150px" style="text-transform:uppercase"  
                                ToolTip="Ingrese el codigo del analisis" 
                                CssClass="form-control input-sm" ontextchanged="txtCodigo_TextChanged" 
                                AutoCallBack="True" TabIndex="1" />&nbsp;&nbsp;   
                            <anthem:Label ID="lblMensajeCodigo" runat="server" Font-Bold="True" 
                                ForeColor="#CC3300" Visible="False" TabIndex="100">El codigo ya existe. Verifique.</anthem:Label></td>
				<td  >
                    &nbsp;</td>
			</tr>
			<tr>
			<td class="myLabelIzquierda" >Nombre:</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="400px"  CssClass="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre del germen" MaxLength="250"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                             </td>
				<td  >
                    &nbsp;</td>
			</tr>
			 
		
			</table>
                    </div>
       <div class="panel-footer">
           <table>
               	<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="GermenList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar" CssClass="btn btn-primary" Width="100px" ToolTip="Haga clic aquí para guardar el germen" />
                </td>
				<td   align="right">
                    &nbsp;</td>
			</tr>
           </table>
	
		</div>

       </div>
    </div>
    
	</asp:Content>
