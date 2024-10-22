<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpresoraEdit.aspx.cs" Inherits="WebLab.ImpresoraEdit" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
     <%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>


<title>LABORATORIO</title> 

   


</asp:Content>
<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
       
             <div align="left" style="width:810px" class="form-inline"  >
   <div class="panel panel-primary">
                    <div class="panel-heading">
    <h3 class="panel-title">IMPRESORAS DE ETIQUETAS</h3>
                        </div>

				<div class="panel-body">   
     <table  style="width: 700px" >
         
    
     <tr>
     <td colspan="2" >
          Impresora del Servidor - Solo para SIL local: 
  
         <anthem:DropDownList ID="ddlImpresora" runat="server" Enabled="false"   class="form-control input-sm"  >
         </anthem:DropDownList>
         <hr />
          Impresora-Solo para SIL en la Nube:
         <anthem:TextBox ID="txtNuevaImpresora" runat="server"></anthem:TextBox>

         <anthem:Button CssClass="btn btn-success" Width="80px" ToolTip="Agrega la impresora a la lista del SIL." ID="btnAgregarImpresora" runat="server" Text="Agregar" 
             onclick="btnAgregarImpresora_Click" />
             
    </td>
    </tr>
    <tr>
    <td colspan="2"><hr /></td>
    </tr>
         
         <tr>
     <td colspan="2"  >
         <anthem:ListBox ID="lstImpresora" runat="server" class="form-control input-sm"   Height="200px" Width="450px" Font-Size="14px"></anthem:ListBox>
         <br />
           <anthem:Button  onClick="btnSacarImpresora_Click" CssClass="btn btn-success" Width="80px" Text="Sacar" ToolTip="Sacar impresora de la lista"    ID="btnBorrar" runat="server"  />
            
             </td>
     
     </tr>
     
         <tr>
     <td  align="left" colspan="2">
     <hr />
         <asp:Button ID="btnGuardar"  CssClass="btn btn-primary"  Width="150px" runat="server" onclick="btnGuardarImpresora_Click" 
             Text="Guardar" />
             </td>
     
     </tr>
        
     </table>


     </div>                                                                                      
       </div>
                 </div>
    </asp:Content>
