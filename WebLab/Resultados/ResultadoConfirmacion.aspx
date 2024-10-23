<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoConfirmacion.aspx.cs" Inherits="WebLab.Resultados.ResultadoConfirmacion" MasterPageFile="~/Site1.Master" %>
<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>
<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

  

    <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
     
  
   <%--  <asp:RequiredFieldValidator ID="rfvFechaDesde" 
                                runat="server" ControlToValidate="txtFecha" ErrorMessage="Fecha Desde" 
                                ValidationGroup="0">*</asp:RequiredFieldValidator>--%>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
   
   
    </asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
     
    
    <div  style="width: 1000px" class="form-inline" >
      
    
                     <div class="panel panel-danger" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
    <h2 class="panel-title">
        <asp:Label ID="lblTitulo" runat="server" Text="CONFIRMACION DE PREVALIDACION"></asp:Label>
        </h2>
                        <p class="panel-title">
                            &nbsp;</p>
                        <p class="panel-title">
        <asp:Label ID="lblTitulo0" runat="server" Font-Bold="True" Font-Size="16pt" ForeColor="#FF3300"></asp:Label>
        </p>
  </div>
                    <div class="panel-body">

                   <asp:GridView ID="gvResumen" CssClass="table table-bordered bs-table"  runat="server" EmptyDataText="No hay resultados pendientes de confirmación"></asp:GridView>
					    <br />
                        <div class="myLabelIzquierda">
                            Seleccionar:
                            <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" ValidationGroup="0">Todas</asp:LinkButton>
                            &nbsp;
                            <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" ValidationGroup="0">Ninguna</asp:LinkButton>
                        </div>
                        <br />
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"
                DataKeyNames="iddetalleprotocolo" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
                EmptyDataText="No se encontraron protocolos para confirmar">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Numero" >
                <ItemStyle Width="20%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="paciente" HeaderText="Paciente" >
                <ItemStyle Width="50%" />
                </asp:BoundField>
                
                
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>

					<br />
                            <asp:Button ID="btnValidarPendiente" runat="server" CssClass="btn btn-danger" Text="Confirmar PreValidacion" OnClick="btnValidarPendiente_Click" Width="200px" />

                       
                        <br />

                       
                        </div>
                        
                               <div class="panel-footer"><div class="panel panel-default" runat="server" id="divValidacion" visible="false">
                    <div class="panel-heading">
    Firma electronica
  </div>
                    <div class="panel-body" >


                            
                        
                                  <table style="width:100%;">
                                      <tr>
                                          <td align="left"><asp:Label Font-Bold="true" ID="lblUsuarioValida" runat="server"  ></asp:Label>                                </td>
                                      </tr>
                                      <tr>
                                          <td align="left">
                                              <asp:HyperLink ID="hplCambiarContrasenia"  runat="server">Cambiar Contraseña</asp:HyperLink>
                                          </td>
                                      </tr>
                                  </table>
                            
                                 </div>
                                       </div>	
                                 
					 			
                                   </div>
                    </div>
                        
                          

</div>       
      
         
 </asp:Content>