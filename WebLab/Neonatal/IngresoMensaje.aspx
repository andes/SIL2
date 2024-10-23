<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IngresoMensaje.aspx.cs" Inherits="SIPS.Laboratorio.Neonatal.IngresoMensaje" MasterPageFile="~/Laboratorio/login.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Laboratorio/default/style.css" />
     <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
   
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
</asp:Content>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
        <table  width="800px"                       cellpadding="1" cellspacing="1"  >
					
					
					<tr>
						<td                                              
                           align="left"  >
                                &nbsp;</td>
						<td                                              
                           align="left">
                                &nbsp;</td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left"  >
                                &nbsp;</td>
						<td                                              
                           align="left">
                                &nbsp;</td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left"  >
                                &nbsp;</td>
						                                          
                           <td class="myTituloGris">
                               NUEVA SOLICITUD DE PESQUISA NEONATAL
					    </td>
					</tr>
						
					
					
					<tr>
						<td                                              
                           align="left"  >
                                &nbsp;</td>
						<td                                              
                           align="left" style="font-weight: bold; font-size: 14px">
                                <hr /></td>
					</tr>
						
					
					
					<tr>
						<td  >
                            &nbsp;</td>						
                          <td  class="myLabelIzquierda"> Se ha generado la solictud Nro. 
                              <asp:Label CssClass="mytituloGris" ID="lblNumeroSolicitud" runat="server" Text="Label"></asp:Label>
                                  </td>
					</tr>
						
					
					
					<tr>
						<td  >
                            &nbsp;</td>						
                          <td  class="myLabelIzquierda"> 
                              <br />
                              <br />
                                  </td>
					</tr>
						
					
					
					<tr>
						<td  >
                            &nbsp;</td>
						 <td  class="myLabelIzquierda">   					
                              <asp:GridView ID="gvLista" runat="server" 
                                  EmptyDataText="No se generaron alarmas a tener en cuenta">
                                  <HeaderStyle BackColor="#CC3300" ForeColor="White" />
                              </asp:GridView></td>
					</tr>
						
					
					
					<tr>
						<td  >
                            &nbsp;</td>
						 <td  class="myLabelIzquierda">   					
                              <br />
                        </td>
					</tr>
						
					
					
					<tr>
						<td  >
                            &nbsp;</td>
						<td class="myLabelIzquierda" >
                            <asp:LinkButton ID="lnkDescargar" runat="server" onclick="lnkDescargar_Click">Descargar 
                            Comprobante</asp:LinkButton>
                        </td>
					</tr>
						
					
			
						
					
				
				
					
					
					<tr>
						<td  >
                            &nbsp;</td>
						<td>
                           <hr /></td>
						</tr>
						
					
				
					
				
						
					
					
					<tr>
						<td  >
                            &nbsp;</td>
						<td>
                            &nbsp;</td>
						</tr>
						</table>
											
 
</asp:Content>

