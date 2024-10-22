<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DescargaSolicitud.aspx.cs" Inherits="WebLab.Neonatal.DescargaSolicitud" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
     <div align="left" style="width:1000px">
  
       
	   
		<table width="800px" align="center" class="myTabla" style="width: 600px">
			<tr>
			
				<td colspan="2" ><b  class="mytituloTabla">DESCARGA DE SOLICITUDES DERIVADAS</b>
                    </td>
			
			<td align="right" style="width: 268px"> <a href="../Help/Documentos/Métodos.htm" target="_blank"  > <img style="border:none;" alt="Ayuda" src="../App_Themes/default/images/information.png" /></a></td>
			</tr>
			<tr>
				<td colspan="3" >
                                     <hr class="hrTitulo" /></td>
			</tr>
			<tr>
				<td class="myLabelIzquierda" style="width: 174px">

	                Archivo a descargar:</td>
				<td style="width: 277px">

	 <asp:FileUpload ID="fupSolicitud" runat="server" CssClass="myTexto" Width="250px" />   

   
        
                </td>
				<td align="right" style="width: 268px">

	                <asp:Button ID="btnGuardar" runat="server" CssClass="myButton" 
                        onclick="btnGuardar_Click" Text="Guardar Solicitudes" Width="150px" />

   
        
                </td>
			</tr>
			<tr>
				<td colspan="3" ><hr /></td>
			</tr>
			<tr>
				<td align="right" colspan="2" >
                               
                                </td>
				<td align="right" >
                               
                                &nbsp;</td>
			</tr>
			<tr>
				<td colspan="2" >
                                    &nbsp;</td>
				<td >
                                    &nbsp;</td>
			</tr>
			</table>

	

   
  </div>
    
        
   
</asp:Content>
