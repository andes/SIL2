<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadoDesdeEquipo.aspx.cs" Inherits="WebLab.Resultados.ResultadoDesdeEquipo" MasterPageFile="~/Site1.Master" %>

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
           <div class="panel panel-success" runat="server" > 
                    <div class="panel-heading"> 
                        <h3 class="panel-title">VALIDACION DE RESULTADOS PENDIENTES EXTRAIDOS DESDE EL EQUIPO</h3>

                        </div>
                   <div class="panel-body">
     <table width="100%"
         >
     <tr>

         <td >


            <div>
                 <div class="row">
              <h3>    <asp:Label ID="lblCodigo" runat="server" Text="Label"></asp:Label>         &nbsp;&nbsp;            
                <asp:Label ID="lblDeterminacion" runat="server" Text="Label"></asp:Label></h3>
                     </div>
                 <div class="row">
           <br />
</div>
            </div>
         
          

                </td>
         <td class="auto-style1">
          <h3>    <asp:Label ID="lblProcesado" runat="server" Text="Procesado en este paso " Visible="false"></asp:Label></h3>
                             <asp:GridView ID="gvResumen" runat="server" Visible="False"  CssClass="table table-bordered bs-table" >
                                 <FooterStyle Font-Bold="True" />
                             </asp:GridView>
         </td>

     </tr>
                <tr>
              

                <td align="left">  
               
               

                    <div class="myLabelIzquierda">
                        Seleccionar:
                        <asp:LinkButton ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" ValidationGroup="0">Todas</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" ValidationGroup="0">Ninguna</asp:LinkButton>
                        <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
                    </div>
                  
               

        </td>
                <td align="right">   
                  
               

     <asp:Button ID="btnValidar" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-primary" Width="150px"
                Text="Validar" TabIndex="4" />

   

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
                	 
        
                        <br />
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"
                DataKeyNames="idDetalleProtocolo" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="12pt"
                EmptyDataText="No se encontraron resultados para incorporar" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="numero"   HeaderText="Protocolo" >
               
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="DNI" >
            
                </asp:BoundField>
                
                
                  <asp:BoundField DataField="paciente" HeaderText="Paciente" />
                <asp:BoundField DataField="resultado" HeaderText="Resultado" />
                <asp:BoundField DataField="fechaEnvio" HeaderText="Enviado" />
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

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