<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadosBusqueda.aspx.cs" Inherits="WebLab.PeticionElectronica.ResultadosBusqueda" MasterPageFile="SitePE.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %> 
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
    
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
 

   
    </asp:Content>
 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
    <br />
<br />
<div align="center" style="width: 1200px" class="form-inline"  >
      <table  width="800px"  
                     style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: normal; color: #000000"  cellpadding="1" cellspacing="1" >
<tr><td>
                       <div class="panel panel-success">
                    <div class="panel-heading">
    <h3 class="panel-title"><asp:Label ID="lblTitulo" runat="server" Text="Buscador de Resultados"></asp:Label></h3>
                        </div>

				<div class="panel-body">	
                     <p> Recuerde que la Ley 2611, en su artículo 4°, inciso K, señala: “Los
pacientes tienen derecho a la intimidad y confidencialidad de los datos. En
correspondencia con este derecho el servidor público debe indefectiblemente
guardar y preservar el secreto profesional”</p>
				 <table  width="790px"  cellpadding="1" cellspacing="1"   >                     
					
					
					<tr>
					<td colspan="2" 
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;"> 
                                        Búsqueda e Identificación del Paciente</td>
						
					<td 
                            
                            style="color: #333333; font-weight: bold; font-size: 12px; vertical-align: top;"> 
                                        &nbsp;</td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            DU Paciente:  
                                            </td>
                                            <td>
                                                          <input id="txtDni"  type="text" runat="server"   class="form-control input-sm"
                                style="width: 150px"
                                 onblur="valNumero(this)" title="Ingrese el documento unico del paciente (DNI)" maxlength="8" tabindex="6"/>
                                             &nbsp;   &nbsp;  
                           
                           
                            <asp:CustomValidator ID="cvDNI" runat="server" 
                                ErrorMessage="Numero " 
                                onservervalidate="cvDNI_ServerValidate" ValidationGroup="0" 
                                >Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                                           
                        </td>
						
                                            <td>
                                                          &nbsp;</td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            Apellido/s:</td>
						<td align="left">
                                            <asp:TextBox ID="txtApellido" runat="server"  class="form-control input-sm" TabIndex="7" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
						<td align="left">
                                            &nbsp;</td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" style="width: 133px" >
                                            Nombres/s:</td>
						<td align="left" >
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control input-sm"  TabIndex="8" 
                                                Width="300px" ToolTip="Ingrese el nombre del paciente"></asp:TextBox>
                        </td>
						
						<td align="left" >
                                            &nbsp;</td>
						
					</tr>

					<tr>
						<td  colspan="2" >
                                   <anthem:LinkButton ID="lnkAmpliarFiltros"   CssClass="myLittleLink" runat="server" 
                                                onclick="lnkAmpliarFiltros_Click" Text="Ampliar filtros de búsqueda"></anthem:LinkButton></td>
						
						<td >
                                   &nbsp;</td>
						
					</tr>
					<tr>
						<td  colspan="2" >					
                        <anthem:Panel ID="pnlParentesco" runat="server" Visible="false">
                          <table width="100%">
                                            
					<tr>
						<td class="myLabelIzquierda" >
                                            DU Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                                          <input title="Ingrese el documento unico del parentesco" id="txtDniMadre" type="text" runat="server"  class="form-control input-sm"
                                 onblur="valNumero(this)" style="width: 150px" maxlength="8"/>
                                                          <asp:CustomValidator ID="cvDNIMadre" runat="server" ErrorMessage="Numero " 
                                                              onservervalidate="cvDNIMadre_ServerValidate" ValidationGroup="0">Sólo numeros (sin puntos ni espacios)</asp:CustomValidator>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda"  >
                                            Apellido Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtApellidoMadre" runat="server" class="form-control input-sm" TabIndex="4" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
					<tr>
						<td class="myLabelIzquierda" >
                                            Nombres/s Madre/Tutor:</td>
						<td align="left" colspan="3" >
                                            <asp:TextBox ID="txtNombreMadre" runat="server" class="form-control input-sm" TabIndex="4" 
                                                Width="300px" ToolTip="Ingrese el apellido del paciente"></asp:TextBox>
                        </td>
						
					</tr>
				
					</table>
					    </anthem:Panel>
					    </td>
						<td >					
                            &nbsp;</td>
					</tr>
					
					
					
					</table>
						
                    </div>
                          <div class="panel-footer">
                              <div>
                                                              <asp:Button ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="0" 
                                                onclick="btnBuscar_Click" CssClass="btn btn-success" TabIndex="12" Width="77px" 
                                                ToolTip="Haga clic aquí para buscar o presione ENTER" /> <asp:CustomValidator ID="cvDatosEntrada" runat="server" 
                                                ErrorMessage="Debe ingresar al menos un parametro de busqueda" 
                                                onservervalidate="cvDatosEntrada_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                
                              </div>
                              <div>
                                <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False" 
                                DataKeyNames="idPaciente" CssClass="table table-bordered bs-table"  PageSize="20" 
                                
                                EmptyDataText="No se encontraron resultados para los parametros de busqueda ingresados" 
                                onrowcommand="gvLista_RowCommand" onrowdatabound="gvLista_RowDataBound" 
                                
                                GridLines="Horizontal" >
            
            <Columns>
                <asp:BoundField DataField="numeroDocumento" HeaderText="DNI" >
                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Apellidos y Nombres">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle Width="60%" />
                </asp:BoundField>
                 <asp:BoundField DataField="fechaNacimiento" HeaderText="Fecha Nac.">
                     <ItemStyle Width="20%" HorizontalAlign="Left" />
                </asp:BoundField>
                 <asp:TemplateField>
                            <ItemTemplate>
                            <asp:ImageButton ID="Editar" runat="server" ImageUrl="~/App_Themes/default/images/zoom.png" 
                            CommandName="Editar" ToolTip="Visualizar laboratorios" />
                            </ItemTemplate>
                          
                               <ItemStyle Width="20px" HorizontalAlign="Center" Height="20px" />
                          
                        </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle ForeColor="#CC3300" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#3A93D2" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#666666" Font-Bold="False" ForeColor="White" 
            />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
                   </div>
                          </div>
                          </div>



</td></tr>
   
 </table>
   
 </div>
 </asp:Content>