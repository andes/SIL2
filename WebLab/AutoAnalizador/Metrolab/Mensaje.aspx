<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mensaje.aspx.cs" Inherits="WebLab.AutoAnalizador.Metrolab.Mensaje" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
  <br />   &nbsp;
    
		<div align="left" style="width: 700px">
		
<table align="center" width="600" style="width: 600px">
<tr>
						<td class="mytituloTabla" >
                                            &nbsp;</td>
						
					</tr>
<tr>
						<td class="mytituloTabla" >
                                           RESULTADOS METROLAB<hr /></td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                           <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                           &nbsp;</td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
					
					
                            <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
					
					
					    </td>
						
					</tr>
					</table>
		
		</div>				      

 </asp:Content>