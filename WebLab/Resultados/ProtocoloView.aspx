<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloView.aspx.cs" Inherits="WebLab.Resultados.ProtocoloView" %>

<%@ Register src="PesquisaNeonatal.ascx" tagname="PesquisaNeonatal" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  
     
     <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      <link type="text/css"rel="stylesheet"      href="../App_Themes/default/principal/style.css" />  
      <script language="Javascript" type="text/javascript" src="../FusionCharts/FusionCharts.js"></script>
        
   
 
     
 
    
     <style type="text/css">
         .style1
         {
             border-style: none;
             font-size: 8pt;
             font-family: Arial, Helvetica, sans-serif;
             background-color: #F3F3F3;
             color: #333333;
             font-weight: normal;
             width: 88px;
         }
         .style2
         {
             width: 312px;
         }
     </style>
        
   
 
     
 
    
     </head>

<body> 

    <form id="form1" runat="server">
   <table class="mytable1" width="600px">
      						
					
					
					<tr>
							<td class="style1" width="150px">
                                DU:</td>
						<td   width="250px" class="style2">
                            <asp:Label ID="lblDni" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="9pt"></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo" width="100px">
                            Protocolo:</td>
						<td 
                            align="left"   width="150px"  >
                            <asp:Image ID="imgEstado" runat="server" 
                                ImageUrl="~/App_Themes/default/images/amarillo.gif" Width="12px" Height="12px" />
                              
                            <asp:Label ID="lblProtocolo" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt" ></asp:Label>
                            </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                                Paciente:</td>
						<td  >
                            <asp:Label CssClass="myLabelIzquierda" ID="lblPaciente" runat="server" Text="Label" 
                                ></asp:Label>
                            <asp:Label ID="lblCodigoPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="9pt" Visible="False"></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo">
                            Fecha:</td>
						<td 
                            align="left"   >
                                     <asp:Label Font-Bold="True" 
                                Font-Size="9pt" ID="lblFecha" runat="server" Text="Label"></asp:Label>    
                            </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                            Sexo:</td>
						<td  >
                            <asp:Label ID="lblSexo" runat="server" Text="Label" CssClass="myLabelIzquierda" ></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo">
                            Origen:
                            </td>
						<td 
                            align="left"   >
                            <asp:Label ID="lblOrigen" runat="server" Text="Label"  CssClass="myLabelIzquierda"></asp:Label>
                            <asp:Label ID="lblNumeroOrigen" runat="server" Text="Label"  CssClass="myLabelIzquierda"></asp:Label>
                        </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                            Fecha Nac.:</td>
						<td  >
                            <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"  CssClass="myLabelIzquierda"></asp:Label>
                        </td>
						<td class="myLabelIzquierdaFondo" >
                          Edad:  
                            </td>
						<td   
                            align="left" >
                              <asp:Label ID="lblEdad" runat="server" Text="Label"  CssClass="myLabelIzquierda"></asp:Label>   &nbsp;
                            <%-- <asp:Label ID="lblDatosScreening" runat="server" Text="Label"  CssClass="myLabel"></asp:Label>--%>
                          
                        </td>
					</tr>
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">     Sector:
                           </td>
						<td  >
                          <asp:Label ID="lblSector" runat="server" Text="Label"  CssClass="myLabelIzquierda"></asp:Label>
                        </td>
							<td class="myLabelIzquierdaFondo" >
                            Prioridad:
                        </td>
						<td   
                            align="left" >
                             <asp:Label ID="lblPrioridad" runat="server" Text="Label"  CssClass="myLabelIzquierda"></asp:Label>
                        </td>
					</tr>
						
							
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                                Solicitante:</td>
						<td  >
                            <asp:Label  ID="lblMedico" runat="server" CssClass="myLabelIzquierda" Text=""></asp:Label>    
                       
                        </td>
							<td class="myLabelIzquierdaFondo" >
                                Diagnósticos:</td>
						<td   
                            align="left" >
                            <asp:Label ID="lblDiagnostico" runat="server" Text="" Font-Bold="False" 
                                Font-Size="9pt" CssClass="myLabelRojo"></asp:Label>                                                                                                               
                                
                        </td>
					</tr>
						
							
						
					
					
					<tr>
							<td class="myLabelIzquierdaFondo">
                                Usuario:</td>
						<td  >
                            <asp:Label  ID="lblUsuario"  CssClass="myLabelIzquierda"  runat="server" Text="Label"></asp:Label>                     
                       
                        </td>
							<td class="myLabelIzquierdaFondo" >
                                Fecha de Registro:</td>
						<td   
                            align="left" >
                                <asp:Label  ID="lblFechaRegistro" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>                
                                 <asp:Label  ID="lblHoraRegistro" runat="server" Text="Label" CssClass="myLabelIzquierda"></asp:Label>    
						   
                        </td>
					</tr>
						
							
						
					
					
						<tr>
						<td class="myLabelIzquierdaFondo">Observaciones:</td>
						<td colspan="3"  >
                            <asp:Panel ID="pnlObservaciones" runat="server">
                            <asp:Label ID="lblObservacion" runat="server" Text="myLabelIzquierda"  CssClass="mytituloRojo"></asp:Label> 
                            </asp:Panel>
						   
                            </td>
					</tr>
						
							
						
					
					
						<tr>
						<td class="myLabelIzquierdaFondo">MUESTRA:</td>
						<td colspan="3"  >
                            <asp:Label ID="lblMuestra" runat="server" Text="" Font-Bold="True" 
                                Font-Size="10pt" CssClass="myLabelRojo"></asp:Label>                                                                                                               
                                
                            </td>
					</tr>
						
						</table>
            <br />     
              
     <uc1:PesquisaNeonatal ID="PesquisaNeonatal1" runat="server" />

</form>
</body>