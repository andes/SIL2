<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnvioMensaje.aspx.cs" Inherits="WebLab.AutoAnalizador.EnvioMensaje" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">



   
</asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
   <div align="left" style="width: 900px" class="form-inline"  >
   <div class="panel panel-success">
                    <div class="panel-heading">
    <h3 class="panel-title">      ENVIO DE PROTOCOLOS</h3>
                        </div>
                        	<div class="panel-body">
                           <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>
                                <br />
                           <asp:Button ID="btnDescargarArchivo" runat="server"  CssClass="btn btn-primary"
                                               onclick="btnDescargarArchivo_Click" Text="Descargar Archivo para el Equipo" Visible="False" 
                                               Width="250px" />
                                </div>
                           	<div class="panel-footer">
                           <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
                                   </div>

                        </div>
      
	 
		
		</div>				      

 </asp:Content>
