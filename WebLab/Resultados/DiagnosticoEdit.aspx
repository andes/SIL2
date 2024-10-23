<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiagnosticoEdit.aspx.cs" Inherits="WebLab.Resultados.DiagnosticoEdit" MasterPageFile="~/Site2.Master"  %>


<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="../script/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" /> 
       <link type="text/css"rel="stylesheet"      href="../App_Themes/default/style.css" />  
      
 <%--  	 <script type="text/javascript" src="../script/Mascara.js"></script>--%>
     <script type="text/javascript" src="../script/ValidaFecha.js"></script>                
     <script src="../script/query-ui-1.8.1.custom.min.js" type="text/javascript"></script>  
     <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
     
  


   
    </asp:Content>




 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">   
    <div align="left" style="width:700px"  class="form-inline"  >
       <input id="hidToken" type="hidden" runat="server" />  

                    <div    runat="server" class="panel panel-default" >                           
                            <div class="panel-heading">
    <h4>    <asp:Label  ID="lblProtocolo" runat="server" Text="Label"></asp:Label></h4>
                        </div>
        <div class="panel-body">
                 <table  width="600px" align="left">					 										
					<tr>
						<td colspan="2"  >						
					
                          
                            <div id="tab2" style="height: 280px">
                           
                                <fieldset id="Fieldset2"                                  >
                            <%--    <legend >Diagnósticos Presuntivos - CIE 10</legend>--%>
                                <table>
                                <tr>
                                <td>
                                    
						 <div id="tab11" runat="server">
                                     <table align="left" width="100%">
                                         <tr>
                                           <td  colspan="3"  >
                                               Codigo:
                                                 <anthem:TextBox ID="txtCodigoDiagnostico" runat="server" AutoCallBack="True"  Width="100px"
                                                   class="form-control input-sm"  ></anthem:TextBox>
                                             </td>  
                                           
                                         </tr>
                                           <tr>
                                           <td   colspan="3"  >
                                            Nombre:<anthem:TextBox ID="txtNombreDiagnostico" runat="server" AutoCallBack="True" 
                                                    class="form-control input-sm" 
                                                   Width="200px"></anthem:TextBox></td>
                                           </tr>
                                         
                                           <tr>
                                           <td   colspan="4"  >
                                              <anthem:Button ID="btnBusquedaDiagnostico"   runat="server" Text="Buscar" 
                                                   onclick="btnBusquedaDiagnostico_Click" ToolTip="Busca segun filtros de busqueda" />
                              <anthem:Button ID="btnBusquedaFrecuente"   runat="server" Text="Ver Frecuentes" ToolTip="Busca el ranking de los diagnosticos mas usados" 
                                                   onclick="btnBusquedaFrecuente_Click" /></td>
                                         </tr>
                                         
                                         
                                        
                                         <tr>
                                           <td   colspan="4" >
                                               Diagnósticos encontrados       </td>                                          
                                               
                                         </tr>
                                         <tr>
                                           <td   colspan="4" >
                                             
                            
						
                                               <anthem:ListBox ID="lstDiagnosticos" runat="server" AutoCallBack="True"  class="form-control input-sm"
                                                   CssClass="myTexto" Height="100px" Width="300px">
                                               </anthem:ListBox>
                                              
                                                   <anthem:Label ID="lblMensajeDiagnostico" runat="server" Visible="false" Text="Label" Font-Bold="True" ForeColor="#CC0000"></anthem:Label>

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
                                             <p  >Diagnósticos del Paciente</p>
                                                 <anthem:ListBox ID="lstDiagnosticosFinal" runat="server"  class="form-control input-sm"
                                                     Height="200px" Width="300px" SelectionMode="Multiple">
                                                 </anthem:ListBox>
                                             </td>   
                                
                                </tr>
                                </table>
                                        
                                </fieldset>                                                                                          
                            </div>
                          
                          
                        </td>
					</tr>
					
																					
						
						
						
				
						
						
					
						
						
						
					<tr>
						
						<td align="left" >
						
                                                 
                        </td>
						
						<td  align="right">
						
                                           <asp:Button CssClass="btn btn-primary" ID="btnGuardar" runat="server" onclick="btnGuardar_Click1"  ToolTip="Guarda los cambios realizados"
                                               Text="Guardar" Width="100px" />
						
                                           </td>
						
					</tr>
				
						
					
						
						
						
						
						
						</table>
              </div>             
		
			  <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                     HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                     ValidationGroup="0" ShowSummary="False" />			
        </div>
    </div>

   </asp:Content>