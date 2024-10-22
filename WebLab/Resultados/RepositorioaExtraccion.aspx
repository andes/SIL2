<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepositorioaExtraccion.aspx.cs" Inherits="WebLab.Resultados.RepositorioaExtraccion" MasterPageFile="~/Site1.Master" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
           function clickButton(e, buttonid) {
               var evt = e ? e : window.event;
               var bt = document.getElementById(buttonid);
               if (bt) {
                   if (evt.keyCode == 13) {
                       bt.click();
                 

                       return false;
                   }
               }
           }

        
</script>                 

  



                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
   
   <div  style="width: 1200px" class="form-inline" >
           <div class="panel panel-danger" runat="server" > 
                    <div class="panel-heading"> 
                        <h3 class="panel-title">INGRESO DE CAJA A EXTRACCION</h3>

                        </div>
                   <div class="panel-body">
     <table width="100%"
         >
     <tr>

         <td >


            <div>
                 <div class="row">
           <br />
             <h4>   Numero de Repositorio:   &nbsp;&nbsp;     <anthem:TextBox ID="txtNumero" runat="server" class="form-control input-sm" Width="200px" 
                                            TabIndex="2" MaxLength="50" BackColor="#FFFF99" Font-Bold="True" Font-Size="18pt" AutoCallBack="True"  ></anthem:TextBox></h4> 
</div>
            </div>
         
          

                </td>
         <td class="auto-style1">
          <h3>    <asp:Label ID="lblProcesado" runat="server" Text="Caja generada en este paso " Visible="false"></asp:Label></h3>
                             <asp:GridView ID="gvResumen" runat="server" Visible="False"  CssClass="table table-bordered bs-table" >
                                 <FooterStyle Font-Bold="True" />
                             </asp:GridView>
         </td>

     </tr>
                <tr>
              

                <td align="left">  
               
               

                <anthem:Button  CssClass="btn btn-primary" ID="btnAgregar" runat="server" Width="100px" 
                    Text="AGREGAR" OnClick="btnAgregar_Click" TabIndex="3" />
                <asp:Button  CssClass="btn btn-primary" ID="btnComenzar" runat="server" Width="160px" Visible="false" 
                    Text="Comenzar nuevo lote" OnClick="btnComenzar_Click" TabIndex="11" />
                  
               

                <anthem:Button  CssClass="btn btn-primary" ID="btnHabilitar" runat="server" Width="120px" 
                    Text="Habilitar ingreso" OnClick="btnHabilitar_Click" TabIndex="3" BackColor="#CC3300" ForeColor="White" Visible="False" />
                  
               

        </td>
                <td align="right">   
                  
               

     <asp:Button ID="btnValidar" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-primary" Width="200px"
                Text="CONFIRMAR INGRESO A EXTRACCION" TabIndex="4" />

   

        </td>
                </tr> 
                <tr>
              

                <td align="left">  
               
               

                <anthem:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="#FF3300"></anthem:Label>
                  
               

        </td>
                <td align="right">   
                  
               

                <anthem:Label ID="lblCantidad" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="#000099"></anthem:Label>
                  
               

                    </td>
                </tr> </table>
                        </div>
              
                    <div class="panel-footer"> <input id="hiditem" type="hidden" runat="server" />
                	 
         <anthem:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"
                DataKeyNames="idRepositorio" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="12pt"
                EmptyDataText="No se encontraron resultados" Width="100%" OnRowCommand="gvLista_RowCommand" OnRowDataBound="gvLista_RowDataBound">
            <Columns>
                
                <asp:BoundField DataField="numero"   HeaderText="Nro. Caja" >
               
                </asp:BoundField>
                <asp:BoundField DataField="fecha" HeaderText="Fecha Recepcion" >
            
                </asp:BoundField>
                
                
                  <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                
           <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                       
                                                                              <asp:LinkButton ID="Eliminar" runat="server" Text="" Width="20px"  OnClientClick="return PreguntoEliminar();">
                                             <span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Height="20px" HorizontalAlign="Center" Width="40px" />
                                                                    </asp:TemplateField>
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </anthem:GridView>

        
                        <br />
                             <asp:GridView ID="gvResumeDetalle" runat="server" Visible="False"  CssClass="table table-bordered bs-table" >
                                 <FooterStyle Font-Bold="True" />
                             </asp:GridView>

        
                        </div>
      


        </div>

        
    </div>
  <script language="javascript" type="text/javascript">

 

    
    function PreguntoEliminar()
    {
    if (confirm('¿Está seguro de quitar el protocolo?'))
    return true;
    else
    return false;
}
  
      </script>
</asp:Content>