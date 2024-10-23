<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaracterEdit.aspx.cs" Inherits="WebLab.Caracteres.CaracterEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server" />
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
      
   <div align="left" style="width: 700px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                           <h3 class="panel-title">
CARACTER PROTOCOLO COVID19
                        </h3>
                       
                        </div>
        	<div class="panel-body">	
		 Nombre: 
                    <asp:TextBox ID="txtNombre" runat="server" Width="400px" CssClass="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                              
                </div>
         <div class="panel-footer">		
            
                                    
                    <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" Width="100px"
            Text="Guardar" CssClass="btn btn-primary" ToolTip="Haga clic aquí para guardar el registro" />
             <br />
               <br />


                     <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="CaracterList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                        
                
</div>
	
	</div>
       </div>
	</asp:Content>
