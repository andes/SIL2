<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MensajeEdit.aspx.cs" Inherits="WebLab.MensajeEdit" MasterPageFile="~/Site1.Master" %>
  <asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">
  <link href="App_Themes/default/style.css" rel="stylesheet" type="text/css" /> 
      <style type="text/css">
          .style3
          {
              font-size: 10pt;
              font-family: Calibri;
              color: #333333;
              font-weight: bold;
              border-style: none;
              background-color: #FFFFFF;
          }
          .style4
          {
              width: 96%;
          }
      </style>
   </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
 
       <div align="center" class="form-inline" style="width:600px;"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"> Nuevo Mensaje
                        </h3>
                        </div>
       	<div class="panel-body">	
    
        <table style="width: 100%;">
          
            <tr>
                <td class="style3">
                    De:</td>
                <td class="style4">
                    <asp:TextBox ID="txtDe" runat="server" MaxLength="50" Width="400px" class="form-control input-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtDe" ErrorMessage="De" ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3">
                    Para:</td>
                <td class="style4">
                    <asp:TextBox ID="txtPara" runat="server" MaxLength="50" Width="400px" class="form-control input-sm" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="txtPara" ErrorMessage="Para" ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3" style="vertical-align: top">
                    Mensaje:</td>
                <td class="style4">
                    <asp:TextBox ID="txtMensaje" runat="server" Rows="10" TextMode="MultiLine"  class="form-control input-sm"
                        MaxLength="4000" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="txtMensaje" ErrorMessage="Mensaje" ValidationGroup="0">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="style3" style="vertical-align: top" colspan="2">
                   <hr /></td>
            </tr>
            <tr>
                <td class="style3" style="vertical-align: top">
                    &nbsp;</td>
                <td class="style4">
                    <asp:Button ID="btnEnviar" runat="server" onclick="btnEnviar_Click" 
                        Text="Enviar"  CssClass="btn btn-primary"  Width="100px" ValidationGroup="0" />
                    <br />
                    <br />
                    <br />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        HeaderText="Debe completar todos los datos solicitados" ShowMessageBox="True" 
                        ShowSummary="False" ValidationGroup="0" />
                </td>
            </tr>
        </table>
    
    </div>

       </div>
           </div>
    
    </asp:Content>