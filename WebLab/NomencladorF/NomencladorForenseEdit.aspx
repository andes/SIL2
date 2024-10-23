<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NomencladorForenseEdit.aspx.cs" Inherits="WebLab.NomencladorF.NomencladorForenseEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    
    
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
    
   
   <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
Nomenclador
                        </h3>
                       
                        </div>
        	<div class="panel-body">	
		<table width="500px" align="center" class="myTabla">
			

			<tr>
			<td class="myLabelIzquierda" >Codigo:</td>
				<td  >
                    <asp:TextBox ID="txtCodigo" runat="server" Width="100px" class="form-control input-sm" 
                        TabIndex="1" ToolTip="Ingrese el codigo" MaxLength="20"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre1" 
                        runat="server" ControlToValidate="txtCodigo" 
                        ErrorMessage="RequiredFieldValidator" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                             </td>
			</tr>
			

			<tr>
			<td class="myLabelIzquierda" >Nombre:</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="400px" class="form-control input-sm" 
                        TabIndex="1" ToolTip="Ingrese el nombre" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			

			<tr>
			<td class="myLabelIzquierda" >Precio:</td>
				<td  >
                    <asp:TextBox ID="txtPrecio" runat="server" Width="100px" class="form-control input-sm" 
                        TabIndex="1" ToolTip="Ingrese el precio" MaxLength="20"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre0" 
                        runat="server" ControlToValidate="txtPrecio" 
                        ErrorMessage="RequiredFieldValidator" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			

			</table>
                </div>
         <div class="panel-footer">		
             <table>
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="NomencladorForenseList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click"  Width="100px"
            Text="Guardar" CssClass="btn btn-primary" ToolTip="Haga clic aquí para guardar " ValidationGroup="0" />
                </td>
			</tr>
			</table>
</div>
	
	</div>
       </div>
	</asp:Content>
