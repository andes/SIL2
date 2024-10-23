<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MetodoForenseEdit.aspx.cs" Inherits="WebLab.MetodosForense.MetodoForenseEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />

    
    
    
</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    
    
   
   <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
 Metodo
                        </h3>
                       
                        </div>
        	<div class="panel-body">	
		<table width="500px" align="center" class="myTabla">
			

			<tr>
			<td class="myLabelIzquierda" >Nombre:</td>
				<td  >
                    <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm" Width="300px"
                        TabIndex="1" ToolTip="Ingrese el nombre del método" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                             </td>
			</tr>
			

			</table>
                </div>
         <div class="panel-footer">		
             <table>
			<tr>
				<td  >
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="MetodoForenseList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        </td>
				<td   align="right">
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click"  Width="100px"
            Text="Guardar" CssClass="btn btn-primary" ToolTip="Haga clic aquí para guardar el método" />
                </td>
			</tr>
			</table>
</div>
	
	</div>
       </div>
	</asp:Content>
