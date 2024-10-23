<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProtocoloMensaje.aspx.cs" Inherits="WebLab.Protocolos.ProtocoloMensaje" MasterPageFile="~/Site1.Master" %>


<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">         
 
        <div id="error" runat="server"  class="form-inline" align="center" > <br />
    <br />
            <asp:Label  ID="lblError" runat="server" Text="Label" Font-Bold="True" Font-Size="12" ForeColor="#CC3300"></asp:Label>
            <br />
    <br />
        </div>
            
     
   
                  <div id="altaMuestra" runat="server"  class="form-inline" >
     
                     <div class="panel panel-default" runat="server" id="pnlTitulo">
                    <div class="panel-heading">
                       ALTA DE MUESTRA
    
  </div>
                    <div class="panel-body">
                
     
                        <h3>    <asp:Label ID="lblTitulo" Width="200px" runat="server" ></asp:Label></h3>
                          <br />
           <h4><asp:Label ID="lblDescripcion" runat="server" ></asp:Label></h4>
       
 
       

        

                        </div>
                         <div class="panel-footer">
                              <asp:LinkButton Width="140px"
                            ID="lnkNuevo" runat="server" CssClass="btn btn-info"
                onclick="lnkRegresar_Click"><span class="glyphicon glyphicon-list-alt"></span>&nbsp;Nueva Muestra</asp:LinkButton>
        

                         </div>
                             
                         </div>
                      </div>

     
 </asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    

</asp:Content>

