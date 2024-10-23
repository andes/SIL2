<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProximoNumeroList.aspx.cs" Inherits="WebLab.ProximoNumeroList" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
      <br />   &nbsp;
     
     <div>
     <table width="400" >
     <tr>
     <td  >&nbsp;</td>
     <td class="mytituloTabla">Próximo Número de Protocolo</td>
     </tr>
     <tr>
     <td class="style2">  
                     
     <td><hr /></td>
     </tr>
     <tr>
     <td class="style2">&nbsp;</td>
     <td>      <asp:Label ID="lblProximoProtocolo1" runat="server" Font-Bold="True"   Font-Italic="True"
                               Font-Size="20pt" ForeColor="#CC3300" Text="Label"></asp:Label></td><asp:GridView ID="GridView1" runat="server" 
             CellPadding="2" ForeColor="#333333" Font-Names="Arial" Font-Size="9pt" 
             Width="400px">
             <RowStyle BackColor="WhiteSmoke" ForeColor="Black" />
             <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
             <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
             <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
             <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
             <AlternatingRowStyle BackColor="White" />
         </asp:GridView></td>
         <tr>
     <td class="style2">&nbsp;</td>
     <td><hr /></td>
     
     </tr>
     </tr>
         <tr>
     <td class="style2">&nbsp;</td>
     <td>
         <asp:LinkButton ID="lnkRegresar" runat="server" CssClass="myLink" 
             onclick="lnkRegresar_Click">Regresar</asp:LinkButton>
             </td>
     
     </tr>
     </table>
         
              </div>                                                                                        
    </asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

<title>LABORATORIO</title> 

    <style type="text/css">
        .style2
        {
            width: 31px;
        }
    </style>


</asp:Content>

