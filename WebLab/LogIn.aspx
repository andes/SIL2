<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="WebLab.Login" MasterPageFile="~/Site1.Master" %>

<%@ Register src="login.ascx" tagname="login" tagprefix="uc1" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

  
   
   
</asp:Content>




<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">          

<div align="center" style="width:50%">


        
                    <table style="width: 300px;">
        <tr>
            
         
          <td class="myLabelIzquierda">
         <div id="pnlTitulo" runat="server" class="panel panel-danger">
                    <div class="panel-heading">
    <h3 class="panel-title">  <asp:Label ID="lblSubtitulo" runat="server" Text="Label"></asp:Label>   </h3>
                        </div>

				<div class="panel-body">	


                    <uc1:login ID="login1" runat="server" />
                    </div>
             </div>    
            </td>
           
        </tr>
                
    </table>
        

<br />
    
</div>

</asp:Content>