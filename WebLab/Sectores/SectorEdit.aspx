<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SectorEdit.aspx.cs" Inherits="WebLab.Sectores.SectorEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     

   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
    
          <div align="left" style="width:1000px">
       
    	 <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title"> SERVICIOS</h3>
                        </div>

				<div class="panel-body">
		 <div>
			 Nombre: 
                    <asp:TextBox ID="txtNombre" runat="server" Width="284px" class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese el nombre del sector" MaxLength="50"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvNombre" 
                        runat="server" ControlToValidate="txtNombre" 
                        ErrorMessage="Nombre" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             </div>
                    <div>
				 Abreviatura: 
                    <asp:TextBox ID="txtAbreviatura" runat="server" Width="100px" class="form-control input-sm"
                        TabIndex="1" ToolTip="Ingrese la abreviatura" MaxLength="3"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="rfvAbreviatura" 
                        runat="server" ControlToValidate="txtAbreviatura" 
                        ErrorMessage="Abreviatura" ValidationGroup="0">*</asp:RequiredFieldValidator>
                             <br />
                    <asp:CustomValidator ID="cvLetra" runat="server" 
                        ControlToValidate="txtAbreviatura" 
                        ErrorMessage="No es posible usar esta letra, la misma ya fue usada." 
                        onservervalidate="cvLetra_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                       <br />
                    La abreviatura serivirá como prefijo para la numeración de protocolos 
                    discriminadas por sector.</div>
                    </div>
      <div class="panel-footer">
                                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="SectorList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                      
				 <asp:Button ID="btnGuardar" 
                        runat="server" onclick="btnGuardar_Click" 
            Text="Guardar"  TabIndex="4" CssClass="btn btn-primary"  Width="120px"
                        ToolTip="Haga clic aquí para guardar los datos" ValidationGroup="0" />
                
			
	
                 <asp:ValidationSummary ID="vs" runat="server" 
                     HeaderText="Debe completar los datos marcados como requeridos:" 
                     ShowMessageBox="True" ShowSummary="False" ValidationGroup="0" />
               </div>
       </div>
	
	</div>
	</asp:Content>

