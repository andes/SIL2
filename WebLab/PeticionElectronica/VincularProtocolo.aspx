<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VincularProtocolo.aspx.cs" Inherits="WebLab.PeticionElectronica.VincularProtocolo" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
  

  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
   


    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <br />   
     <div align="center" style="width:550px"  class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h1 class="panel-title">Vincular Petición a Protocolo</h1>
                        </div>

				<div class="panel-body">	

     
              <table  width="500px"  cellpadding="1" cellspacing="1" >
					<tr>
						<td  >Nro.Petición:</td>
						<td  >
                            <asp:Label ID="lblPeticion" runat="server" Text="Label" Font-Bold="True" Font-Size="14pt"></asp:Label>
                        </td>
					</tr>
					<tr>
						<td  >Paciente:</td>
						<td  >
                            <asp:Label ID="lblPaciente" runat="server" Text="Label" Font-Bold="True" Font-Size="14pt"></asp:Label>
                        </td>
					</tr>
					<tr>
						<td   >Protocolo Asignado:</td>
						<td  >
                            <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Bold="True" Font-Size="14pt"></asp:Label>
                                        
                                            </td>
                            </tr>
                            
						
					<tr>
						<td    >Nro. de Protocolo:</td>
						<td  >
                            <anthem:TextBox ID="txtNro" runat="server" onblur="valNumeroSinPunto(this)" class="form-control input-sm" MaxLength="9" TabIndex="2" Width="80px" OnTextChanged="txtNro_TextChanged" AutoCallBack="True"></anthem:TextBox>
                           
                        </td>
                            </tr>
                            
						
				

					<tr>
						<td  class="myLabelIzquierda" colspan="2" >
                           
                            <anthem:Label ID="status" runat="server" Text="Label" ForeColor="#0000CC" Visible="False"></anthem:Label>
                        </td>
                            </tr>
                            
						
				

					</table>
                        </div>
       <div class="panel-footer">
            <asp:CustomValidator ID="cvNumero" runat="server" 
                                ErrorMessage="Numero de Protocolo" 
                                onservervalidate="cvNumeros_ServerValidate" ValidationGroup="0" 
                                >Numero de Peticion: Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                                            <anthem:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary"
                                                           TabIndex="9" Text="Guardar" ValidationGroup="0"     OnClientClick="return PreguntoEliminar();"
                                                                Width="120px"  Enabled="False" OnClick="btnGuardar_Click" AutoUpdateAfterCallBack="True" />
               </div>
            </div>
         
         </div> 
  

  
     <script language="javascript" type="text/javascript">

                 function PreguntoEliminar() {
                     if (confirm('¿Está seguro de vincular la petición al protocolo seleccionado?'))
                         return true;
                     else
                         return false;
                 }
    </script>
</asp:Content>