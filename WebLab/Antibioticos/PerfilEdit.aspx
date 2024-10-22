<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerfilEdit.aspx.cs" Inherits="WebLab.Antibioticos.PerfilEdit" MasterPageFile="~/Site1.Master" %>


<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
    
 
  
   <link href="../App_Themes/default/style.css" rel="stylesheet" type="text/css" /> 


    


   
    </asp:Content>




 
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          
 

    <div align="left" class="form-inline" style="width: 900px" >
  
          
	   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">PERFIL DE ANTIBIOTICOS</h3>
                        </div>

				<div class="panel-body">	
                   
  <table>
 
  <tr>
        <td class="style1">
            &nbsp;</td>
                                              
        <td   colspan="4">
            <p>Perfil:</p> <asp:TextBox 
                ID="txtNombre" runat="server" MaxLength="50" Width="350px"   class="form-control input-sm" ></asp:TextBox>
                                               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="txtNombre" ErrorMessage="Nombre del Perfil" 
                ValidationGroup="0">*</asp:RequiredFieldValidator>
                                               </td>
                                              
                                        </tr>
  <tr>
        <td style="vertical-align: top" class="style2">
            &nbsp;</td>
                                              
        <td style="vertical-align: top">
      <p>Antibiotico</p>
                                               <anthem:ListBox ID="lstAntibiotico" 
                runat="server" AutoCallBack="True" 
                                                  class="form-control input-sm"  Height="300px" 
                Width="400px" SelectionMode="Multiple">
                                               </anthem:ListBox>
                                               </td>
                                              
        <td colspan="2">
                                    
                                                 <anthem:ImageButton ID="btnAgregarDiagnostico" runat="server" 
                                                     ImageUrl="~/App_Themes/default/images/añadir.jpg" 
                                                     onclick="btnAgregarDiagnostico_Click" /><br />
                                                     <p></p>
                                                 <anthem:ImageButton ID="btnSacarDiagnostico" runat="server" 
                                                     ImageUrl="~/App_Themes/default/images/sacar.jpg" 
                                                     onclick="btnSacarDiagnostico_Click" />

                                                     </td>                                   

                                                     <td>
                                    
                                             <p  >Antibioticos del Perfil&nbsp;&nbsp;
                                                 <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                                     ErrorMessage="Debe agregar antibioticos al perfil" 
                                                     onservervalidate="CustomValidator1_ServerValidate" ValidationGroup="0"></asp:CustomValidator>
                                                         </p>
                                                 <anthem:ListBox ID="lstAntibioticoFinal" runat="server" CssClass="form-control input-sm" 
                                                     Height="300px" Width="400px" SelectionMode="Multiple">
                                                 </anthem:ListBox>
                                                 </td>

                                        </tr>
   
  <tr>
        <td class="style2">
                                    &nbsp;</td>
                                              
        <td colspan="2">
                                    <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                        onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
        </td>
                                              
        <td colspan="2" align="right">
                                                         <asp:Button ID="btnGuardar" runat="server"  CssClass="btn btn-primary" Width="100px"
                                                             onclick="btnGuardar_Click" Text="Guardar" ValidationGroup="0" />
                                                 </td>
                                              
                                        </tr>
  <tr>
        <td class="style2">
                                    &nbsp;</td>
                                              
        <td colspan="2">
                                    &nbsp;</td>
                                              
        <td colspan="2" align="right">
                                                         <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                                             HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                                                             ShowSummary="False" ValidationGroup="0" />
                                                 </td>
                                              
                                        </tr>
  
                                        </table>

                     </div>
           </div>
        </div>
  
   </asp:Content>
