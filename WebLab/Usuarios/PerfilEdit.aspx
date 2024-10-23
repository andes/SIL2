<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerfilEdit.aspx.cs" Inherits="WebLab.Usuarios.PerfilEdit" MasterPageFile="~/Site1.Master" %>




<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

 <link href="../App_Themes/default/style.css" rel="stylesheet" type="text/css" />  
     

   
  
   
    </asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
     <div align="center" class="form-inline" style="width:600px;"  >
   <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title"> PERFIL DE USUARIO 
                        </h3>
                        </div>
       	<div class="panel-body">	
        <table style="width:500px;">
           
            
            <tr>
                <td class="myLabelIzquierda"  style="vertical-align: top; width: 82px;">
                    Nombre Perfil:</td>
                <td class="style10" colspan="2">
                    <asp:TextBox ID="txtNombre" runat="server" Width="350px" class="form-control input-sm"></asp:TextBox>
                    </td>
            </tr>
            <tr>
                <td class="myLabelIzquierda" style="width: 82px">
                    Activo:</td>
                <td class="style10">
                    <asp:CheckBox ID="chkActivo" runat="server" Checked="True" class="form-control input-sm" />
                </td>
                <td class="style10">
                    &nbsp;</td>
            </tr>
          
           
        </table>
      </div>


       <div class="panel-footer">	
           <table>
                <tr>
                <td class="style8" style="width: 82px">
                  <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="btn btn-primary" Width="100px"
                                                PostBackUrl="PerfilList.aspx" CausesValidation="False">Regresar</asp:LinkButton>
                    </td>
                <td align="right" colspan="2">
                    <asp:Button ID="btnGuardar" runat="server" Text="Grabar" 
                        onclick="btnGuardar_Click1" CssClass="btn btn-primary" Width="100px" />
                   
                </td>
            </tr>
           </table>
           </div>
       </div>
    </div>
    
 </asp:Content>