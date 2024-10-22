<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mensaje.aspx.cs" Inherits="WebLab.AutoAnalizador.Mensaje" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
 <div align="left" style="width: 1100px" class="form-inline"  >
   <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">   
                            RESULTADOS</h3>
                       
                        </div>

				<div class="panel-body">
    
		 
                                           <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>
                     
					<br />
                    <br />
					
                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
					
					
					   </div>
       </div>
		
		</div>				      

 </asp:Content>