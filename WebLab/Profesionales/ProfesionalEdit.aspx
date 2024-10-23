<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProfesionalEdit.aspx.cs" Inherits="WebLab.Profesionales.ProfesionalEdit" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
     
 
 <script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
</asp:Content>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  

    	 <div align="left" style="width: 600px" class="form-inline"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
    <h3 class="panel-title">MEDICO SOLICITANTE</h3>
                        </div>

				<div class="panel-body">
        <table style="width:600px">
         
            
            <tr>
                <td  style="vertical-align: top; width: 95px;">
                    Apellido:</td>
                <td colspan="2"  >
                    <asp:TextBox ID="txtApellido" runat="server" Width="350px" CssClass="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server" 
                        ControlToValidate="txtApellido" ErrorMessage="Apellido" 
                        ValidationGroup="0">*</asp:RequiredFieldValidator>
                    </td>
            </tr>
            <tr> 
                <td style="width: 95px">
                    Nombres:</td>
                <td colspan="2"  >
                    <asp:TextBox ID="txtNombre" runat="server" Width="350px" CssClass="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                        ControlToValidate="txtNombre" ErrorMessage="Nombres" ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="width: 95px">
                    Nro. Documento:</td>
                <td colspan="2"  >
                    <input id="txtNumeroDocumento" type="text" runat="server"  class="form-control input-sm"
                                onblur="valNumeroSinPunto(this)" maxlength="8"/>
                           
                           
                            <asp:CompareValidator ID="cvDni" runat="server" ControlToValidate="txtNumeroDocumento" 
                                ErrorMessage="Debe ingresar solo numeros" Operator="DataTypeCheck" 
                                Type="Integer" ValueToCompare="0">Debe ingresar solo numeros</asp:CompareValidator></td>
            </tr>
            <tr>
                <td style="width: 95px">
                    Matricula:</td>
                <td colspan="2"  >
                    <asp:TextBox ID="txtMatricula" runat="server" Width="150px" CssClass="form-control input-sm"
                        MaxLength="50"></asp:TextBox>
                </td>
            </tr>            
            
            <tr>
                <td style="width: 95px">
                    Activo:</td>
                <td colspan="2"  >
                    <asp:CheckBox ID="chkActivo" runat="server" Checked="True" />
                </td>
            </tr>
          </table>
            <div class="panel-footer">
                <table>
            <tr>
                <td >
                   <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
                                                PostBackUrl="ProfesionalList.aspx" CausesValidation="False">Regresar</asp:LinkButton></td>
                <td align="right" colspan="2" >
                    <asp:Button ID="btnGuardar" runat="server" Text="Grabar" 
                        onclick="btnGuardar_Click1"  CssClass="btn btn-primary" Width="100px" />
                   
                </td>
            </tr>
            <tr>
                <td >
                    &nbsp;</td>
                <td align="right" style="width: 495px" >
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        HeaderText="Debe completar los datos requeridos:" ShowMessageBox="True" 
                        ShowSummary="False" ValidationGroup="0" />
                   
                </td>
                <td align="right" style="width: 495px" >
                    &nbsp;</td>
            </tr>
        </table>
                </div>
                    </div>
       </div>
  
    
 </asp:Content>