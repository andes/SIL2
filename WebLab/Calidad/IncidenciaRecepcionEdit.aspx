<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IncidenciaRecepcionEdit.aspx.cs" Inherits="WebLab.Calidad.IncidenciaRecepcionEdit" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="Content1" runat="server" contentplaceholderid="head">

     <link type="text/css"rel="stylesheet"      href="../script/jquery-ui-1.7.1.custom.css" />  
     

  <script type="text/javascript"      src="../script/jquery.min.js"></script> 
  <script type="text/javascript"      src="../script/jquery-ui.min.js"></script> 
    
      <script type="text/javascript"     src="../script/jquery.ui.datepicker-es.js"></script>   
      
      <script type="text/javascript">


          $(function () {
              $("#<%=txtFecha.ClientID %>").datepicker({
                  showOn: 'button',
                  buttonImage: '../App_Themes/default/images/calend1.jpg',
                  buttonImageOnly: true
              });
          });

     
  </script>  
  
  
   	 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>   
  
  

  
    </asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
           
<div align="left" style="width:1000px" class="form-inline">
    <div class="panel panel-default" runat="server" >
                    <div class="panel-heading">
    <h3 class="panel-title">
    <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label>
        </h3>
  </div>
                    <div class="panel-body">
  
        
            <asp:Panel ID="pnlIncidencia" runat="server" Width="100%">
            <table width="100%">
            <tr>
            <td>Número:</td>
            <td> 
                <asp:Label ID="lblNumero" runat="server" Text="Label"></asp:Label>
            </td>
            </tr>
            <tr>
            <td>Fecha Registro:</td>
            <td> 
                <asp:Label ID="lblFecha" runat="server" Text="Label"></asp:Label>
            </td>
            </tr>
            <tr>
            <td>Usuario:</td>
            <td> 
                <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label>
            </td>
            </tr>
            <tr>
            <td colspan="2"><hr />
            </td>
            </tr>
            </table>
            </asp:Panel>
                      
                       
             <table>
                                            <tr>
                                            <td  >
 <b>Fecha:</b>  <input id="txtFecha" runat="server" type="text" maxlength="10" 
                         onblur="valFecha(this)" 
                        onkeyup="mascara(this,'/',patron,true)" tabindex="0" class="form-control input-sm"
                                style="width: 120px" 
        title="Ingrese la fecha de incidencia"  /><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                    ControlToValidate="txtFecha" ErrorMessage="Complete la fecha de la incidencia" 
                                                    ValidationGroup="0"></asp:RequiredFieldValidator>
                                                </td></tr>

                                            <tr>
                                            <td  >
                                       <b>        Numero Origen:</b> 
                                                <asp:TextBox ID="txtNumeroOrigen" runat="server" CssClass="form-control input-sm"></asp:TextBox>
                                                </td></tr>

                                            <tr>
                                            <td  >
                                          <b>      Origen:</b>
                                                <asp:DropDownList ID="ddlEfectorOrigen" runat="server"  class="form-control input-sm" TabIndex="4" ToolTip="Seleccione el efector" Width="700px">
                                                </asp:DropDownList>
                                                </td></tr>

                                            			<tr>
		<td 
            >
                                                         <asp:TreeView ID="TreeView1" runat="server" ImageSet="Arrows" Font-Bold="true">
                                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                                <Nodes>
                                                                    <asp:TreeNode Expanded="True" SelectAction="Expand" ShowCheckBox="False" 
                                                                        Text="No cumple con Indicación para toma de muestra" Value="1">
                                                                        <asp:TreeNode ShowCheckBox="True" Text="no la recibió" Value="2" SelectAction="None"></asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="True" Text="no la entendió bien" Value="3" SelectAction="None">
                                                                        </asp:TreeNode>
                                                                        <asp:TreeNode ShowCheckBox="True" Text="no la cumplió como debía" Value="4" SelectAction="None">
                                                                        </asp:TreeNode>
                                                                    </asp:TreeNode>
                                                                    <asp:TreeNode ShowCheckBox="True" Text="NO TRAE SOLICITUD MEDICA-FICHA" Value="5" SelectAction="None">
                                                                    </asp:TreeNode>
                                                                    <asp:TreeNode ShowCheckBox="True" Text="FALTAN DATOS PERSONALES" Value="6" SelectAction="None">
                                                                    </asp:TreeNode>
                                                                    <asp:TreeNode ShowCheckBox="True" Text="SOLICITUD MEDICA ILEGIBLE" Value="7" SelectAction="None">
                                                                    </asp:TreeNode>
                                                                    <asp:TreeNode ShowCheckBox="True" Text="FUERA DE HORARIO" Value="8" SelectAction="None">
                                                                    </asp:TreeNode>
                                                                </Nodes>
                                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="25px" 
                                                                    HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                                <ParentNodeStyle Font-Bold="False" />
                                                                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" 
                                                                    HorizontalPadding="0px" VerticalPadding="0px" BackColor="Yellow" />
                                                            </asp:TreeView>
                                                            <br />
                                                            </td><td  >
                                                          <%--  <anthem:CheckBoxList ID="chkIncidencia0" runat="server" 
                                                                >
                                                            </anthem:CheckBoxList>
                                                            
                                                            <anthem:CheckBoxList ID="chkIncidencia" runat="server" 
                                                                onselectedindexchanged="chkIncidencia_SelectedIndexChanged" 
                                                                AutoCallBack="True">
                                                            </anthem:CheckBoxList>
                                                          --%>  
                                                                <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                                                    ErrorMessage="Debe seleccionar al menos una incidencia" 
                                                                    onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                                            
                                                            </td>
                                            </tr>

                                            			<tr>
		<td   colspan="2"><hr /></td>
                                            </tr>

                                            			<tr>
		<td style="vertical-align: top" colspan="2"  ><b>Observaciones:</b>
            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control input-sm" Rows="4" TextMode="MultiLine" Width="600px"></asp:TextBox>
                                                            </td>
                                            </tr>

                                        
                                            		 
                                        
        </table>
         
</div>

          
         <div class="panel-footer">
             <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" CausesValidation="False" 
                                                ToolTip="Regresa a la pagina anterior" onclick="lnkRegresar_Click1" >Regresar</asp:LinkButton>

                                                            
                                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" Width="100px" ValidationGroup="0" TabIndex="2" 
                                                                onclick="btnGuardar_Click" />
             </div>


	</div>
    </div>	    
 </asp:Content>