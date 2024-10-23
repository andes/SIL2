<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MuestraSelect.aspx.cs" Inherits="WebLab.Muestras.MuestraSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server"  >           
  
             
                            <table  width="600px" align="left">
										
					
					<tr>
						<td  >
						
					
                          
                            <div id="tab2" style="height: 280px">
                           
                                <fieldset id="Fieldset2"                                  >
                                <legend class="myLabelIzquierda">Tipo de muestras</legend>
                                <table>
                                <tr>
                                <td>
                                    
						 <div id="tab11" runat="server">
                                     <table align="left" width="100%">
                                         <tr>
                                           <td class="myLabelIzquierda" colspan="3"  >
                                               Codigo:
                                                 <anthem:TextBox ID="txtCodigoDiagnostico" runat="server" AutoCallBack="True" 
                                                     CssClass="myTexto" ></anthem:TextBox>
                                             </td>  
                                           
                                         </tr>
                                           <tr>
                                           <td class="myLabelIzquierda" colspan="3"  >
                                            Nombre:<anthem:TextBox ID="txtNombreDiagnostico" runat="server" AutoCallBack="True" 
                                                     CssClass="myTexto"
                                                   Width="200px"></anthem:TextBox></td>
                                           </tr>
                                         
                                           <tr>
                                           <td class="myLabelIzquierda" colspan="4"  >
                                              <anthem:Button ID="btnBusquedaDiagnostico" CssClass="myButtonGris" runat="server" Text="Buscar" 
                                                   onclick="btnBusquedaDiagnostico_Click" ToolTip="Busca segun filtros de busqueda" />
                                               </td>
                                         </tr>
                                         <tr>
                                           <td class="myLabelIzquierda" colspan="4"  >
                                               <hr /></td>
                                         </tr>
                                         
                                        
                                         <tr>
                                           <td class="myLabelIzquierda"  colspan="4" >
                                               Tipos de muestras encontradas       </td>                                          
                                               
                                         </tr>
                                         <tr>
                                           <td class="myLabelIzquierda" colspan="4" >
                                             
                            
						
                                               <anthem:ListBox ID="lstDiagnosticos" runat="server" AutoCallBack="True" 
                                                   CssClass="myTexto" Height="100px" Width="300px">
                                               </anthem:ListBox>
                                              
                                             </td>
                                             
                                        </tr>
                                        </table>
                                      </div>
                                      	                                       
                                     
                                 </td>
                                             <td>
                                                 <anthem:ImageButton ID="btnAgregarDiagnostico" runat="server"  ToolTip="Agrega el diganostico a los del paciente"
                                                     ImageUrl="~/App_Themes/default/images/añadir.jpg" 
                                                     onclick="btnAgregarDiagnostico_Click1" /><br />
                                                     <p></p>
                                                 <anthem:ImageButton ID="btnSacarDiagnostico" runat="server"  ToolTip="Quita el diganostico del paciente"
                                                     ImageUrl="~/App_Themes/default/images/sacar.jpg" 
                                                     onclick="btnSacarDiagnostico_Click" />
                                             </td>                                         
                                             <td style="vertical-align: top">
                                             <p class="myLabelIzquierda">Tipo de muestras seleccionadas</p>
                                                 <anthem:ListBox ID="lstDiagnosticosFinal" runat="server" CssClass="myTexto" 
                                                     Height="200px" Width="300px" SelectionMode="Multiple">
                                                 </anthem:ListBox>
                                             </td>   
                                
                                </tr>
                                </table>
                                        
                                </fieldset>                                                                                          
                            </div>
                          
                          
                        </td>
					</tr>
					
																					
						
						
						
				
						
						
					
						
						
						
						</table>

      
         
 </form>
</body>
</html>
