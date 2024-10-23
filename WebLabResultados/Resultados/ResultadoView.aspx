<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoView.aspx.cs" Inherits="WebLabResultados.Resultados.ResultadoView" MasterPageFile="~/Site1.Master"  UICulture="es" Culture="es-AR" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    
  <style>

 
     
    .label {
  color: white;
  padding: 8px;
  font-size:10pt;
}

.success {background-color: #4CAF50;} /* Green */
.info {background-color: #2196F3;} /* Blue */
.warning {background-color: #ff9800;} /* Orange */
.danger {background-color: #f44336;} /* Red */
.other {background-color: #e7e7e7; color: black;} /* Gray */




/*
	Max width before this PARTICULAR table gets nasty. This query will take effect for any screen smaller than 760px and also iPads specifically.
	*/
	@media screen and (max-width: 600px) {
       table {
           width:100%;
       }
       thead {
           display: none;
       }
       tr:nth-of-type(2n) {
           background-color: inherit;
       }
       tr td:first-child {
           background: #f0f0f0;
           font-weight:bold;
           font-size:1.3em;
       }
       tbody td {
           display: block;
           text-align:center;
       }
       tbody td:before {
           content: attr(data-th);
           display: block;
           text-align:center;
       }
}
       </style>

                   

  
   

    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">               
  
   
    
     <asp:HiddenField runat="server" ID="HFIdPaciente" /> 
    
<asp:HiddenField runat="server" ID="HFIdProtocolo" /> 
      
  <%--     <div class="item  col-xs-12 col-lg-12">--%>
                          
						<div align="left" style="width:800px"   class="form-inline" runat="server"  id="panelResultados" >
   <div class="panel panel-default">
                    <div class="panel-heading">
                      <div  align="center">   <asp:Label ID="lblEfector" runat="server" Text="Label" Font-Bold="True"  
                                Font-Size="14pt" ></asp:Label> 
                          </div>  
                        

                          
     <table  >
                            
      							<tr>
							<td colspan="2" >
                                   <asp:Label ID="lblPaciente" runat="server" Text="Label" Font-Bold="True"  
                                Font-Size="14pt" ></asp:Label> 
                                  <asp:Label ID="lblCodigoPaciente" runat="server" Text="Label" Font-Bold="True" 
                                Font-Size="10pt" Visible="False"></asp:Label></td>
						 
					 
					</tr>
					<tr>
							<td >
                                 DU:</td>
						<td  >
                          
                              
                          <asp:Label ID="lblDni" runat="server" Text="Label"   ></asp:Label>
                        </td>
					 
					</tr>
         	<tr>
							<td >
                                     Fecha Nacimiento: </td>
						<td  >
                          
                      <asp:Label ID="lblFechaNacimiento" runat="server" Text="Label"   ></asp:Label>
                        </td>
					 
					</tr>
         <tr>
							<td >
                                    Edad:  
                                   </td>
						<td  >
                          
                   <asp:Label ID="lblEdad" runat="server" Text="Label"  ></asp:Label>
                        </td>
					 
					</tr>
          <tr>
							<td >
                                         Sexo:
                                        
                                   </td>
						<td  >
                          
                    <asp:Label ID="lblSexo" runat="server" Text="Label"></asp:Label>
                        </td>
					 
					</tr>

					
					<tr>
							<td >
                                Protocolo</td>
						<td  >
                          
                              
                           <asp:Label ID="lblProtocolo" runat="server" Text="Label"   
                                Font-Size="12pt" ></asp:Label>
                        </td>
					 
					</tr>
						
					
					
					
					<tr>
							<td  >
                           Fecha Pedido:</td>
						<td    >
                          
                              
                                     <asp:Label 
                                Font-Size="10pt" ID="lblFecha" runat="server" Text="Label"></asp:Label>    
                        </td>
						 
					</tr>
						
					
					
					<tr>
							<td  >
                                Solicitante:</td>
						<td colspan="3"  >
                            <asp:Label ID="lblSolicitante" runat="server" Text="Label"    
                                Font-Size="10pt" ></asp:Label>
                        </td>
                      
					</tr>
						
					
					
					<tr>
							<td  >
                            Médico Solicitante:</td>
						<td colspan="3"  >
                           <asp:Label ID="lblMedico" runat="server" Text=""   
                                Font-Size="10pt"  ></asp:Label>   
                        </td>
                       
					</tr>
						
					
					
				
						
					
					
					<tr>
							<td  >
                                   <asp:Label ID="lblNroSISA" runat="server"  Font-Size="14px" Text="Label" Visible="true"></asp:Label></td>
						<td    >
                                                                                                                                      
                        </td>
                        
					</tr>
						
					
					
						
					 
					</table>
                        </div>

				<div class="panel-body">
						
						
						
						    
<asp:HiddenField runat="server" ID="HFCurrTabIndex" /> 
                           
                            <div >  
                  
                            
                            <asp:Panel ID="pnlHC" runat="server" > 
                  
                            </asp:Panel>
						 

   
        
       
                             <h4> <asp:Label  ID="lblCovid" runat="server" Visible="false" ></asp:Label>&nbsp;</h4>  
                             <h4> <asp:Label  ID="lblMuestra" runat="server" ></asp:Label></h4>  
                        <asp:Label CssClass="myLabelIzquierdaGde" ForeColor="Red" ID="lblEstadoProtocolo" Visible="false" runat="server" ></asp:Label>
						     <asp:Panel ID="Panel1" runat="server"  Width="100%" BackColor="#F2F2F2"  > 
                                                                                                                                             						 
                                               <asp:Table ID="tContenido"   CssClass="table table-bordered bs-table" 
                                                   Runat="server" 
                                                   ></asp:Table>                                                                                                                                     
                                           </asp:Panel> 
                            <asp:Label ID="lblObservacionResultado" runat="server" Font-Names="Courier New" 
                                Font-Size="10pt" ForeColor="Black" Font-Bold="true" ></asp:Label>
                                
                            <asp:Panel ID="pnlReferencia" Visible="false" runat="server" Width="300px">
                           <span class="label label-default">Dentro de V.R</span>
                                <span class="label label-danger">Fuera de V.R</span>
                            </asp:Panel> 
                                      
                                  
                                               
        
        
                                           
                                               
                                               
                           </div>               
</div>
 
       </div>
                            </div> 
</asp:Content>

