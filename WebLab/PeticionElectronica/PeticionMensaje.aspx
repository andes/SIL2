<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeticionMensaje.aspx.cs" Inherits="WebLab.PeticionElectronica.PeticionMensaje" MasterPageFile="SitePE.Master" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">         
   
        <br />
    <br />
      
     
    <table  width="600px"   align="center">
        <tr>
            <td>
                  <div  class="form-inline" >
     
                     <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
                        SOLICITUD GENERADA</div>
                    <div class="panel-body">
                
      <table  align="left" >
    

    

    <tr>
    <td>
                        <h3>    <asp:Label ID="lblTitulo" Width="200px" runat="server" ></asp:Label></h3>
                         </td>
    <td align="left" style="vertical-align: top" >
        
        
           &nbsp;</td>    
    </tr>
   
    <tr>
    <td colspan="2">
        
        <br />
           <h4><asp:Label ID="lblDescripcion" runat="server" ></asp:Label></h4>
       

       
                         </td>
        
    </tr>
   
    <tr>
    <td colspan="2">
           <br />
        <asp:LinkButton Width="140px"
                            ID="lnkNuevo" runat="server" CssClass="btn btn-info"
                onclick="lnkRegresar_Click"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;Nueva Muestra</asp:LinkButton>
        

       
                         </td>
        
    </tr>
    </table>
   

                        </div>
                         </div>
                      </div>

    


            </td>
        </tr>

    </table>
        </div>
</div>
 </asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    

</asp:Content>

