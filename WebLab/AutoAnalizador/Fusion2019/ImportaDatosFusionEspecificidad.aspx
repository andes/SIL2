<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportaDatosFusionEspecificidad.aspx.cs" Inherits="WebLab.AutoAnalizador.Fusion2019.ImportaDatosFusionEspecificidad" MasterPageFile="~/Site1.Master" %>

<%@ Register src="tablaEspecificidad.ascx" tagname="tablaEspecificidad" tagprefix="uc1" %>

<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                  

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
  
   <div  style="width: 700px"  class="form-inline" >
           <div class="panel panel-default" runat="server" > 
                    <div class="panel-heading"> <h3 class="panel-title">Importación Fusion-Luminex-Especificidad</h3>

                        </div>
                   <div class="panel-body">
     <table width="100%"         >
     <tr>

         <td colspan="2">


            <div>
          
   <label>Tipo de archivo:</label> <asp:DropDownList ID="ddlTipoArchivo" runat="server" class="form-control input-sm" Font-Bold="True">
                         <asp:ListItem Value="3129">Especificidad I</asp:ListItem>
                         <asp:ListItem Value="3130">Especificidad II</asp:ListItem>
                      </asp:DropDownList>
                <br />
                Archivo:
               <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm" Width="400px" />
                <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="150px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />

            </div>
         
            <div>
                <strong>El archivo debe ser de tipo cvs</strong>
               <asp:Label ID="estatus" runat="server" 
                    Style="color:red"></asp:Label>
            </div>
            <hr />
                </td></tr>
                <tr>
              

                <td align="left">  
                  <div class="myLabelIzquierda" >
                      <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
       <br />     <input id="idProtocolo" runat="server" value="" disabled="disabled" />
                   

                    </div> 
        </td>
                <td align="right">   
                  <table>
                      <tr>
                          <td> <asp:CheckBox ID="chkValida" runat="server" Text="Valida Informe" /></td>
                          <td>  <asp:Button ID="btnImprimir" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-danger" Width="150px"
                Text="Generar Informe" /></td>
                          <td>  <asp:Button ID="btnAdjuntar" runat="server"     CssClass="btn btn-success" Width="150px"
                Text="Adjuntar a Protocolo" OnClick="btnAdjuntar_Click" /></td>
                      </tr>
                  </table>
                   
                  
               

              

                    <br />
               <asp:Label ID="estatus0" runat="server" 
                    Style="color:red"></asp:Label>
                   
                  
               

              

        </td>
                </tr> </table>
                        </div>
                    <div class="panel-footer">
                        <table>
                            <tr>
                                <td><div style="border: 1px solid #999999; height: 450px; width:650px; overflow: scroll; background-color: #EFEFEF;"> 

                                    <asp:Label ID="lblNro" runat="server" Text="Label" Font-Bold="True" Font-Size="16" Visible="False"></asp:Label>

                            <asp:GridView ID="gvListaEspecificidad" runat="server" AutoGenerateColumns="False"  Visible="False"
                DataKeyNames="idMindrayResultado" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
                EmptyDataText="No se encontraron resultados para incorporar">
            <Columns>
                <asp:BoundField DataField="numero"   HeaderText="Numero" >
                <ItemStyle Width="20%" HorizontalAlign="Center" Font-Bold="True" />
                </asp:BoundField>
                <asp:BoundField DataField="apellido" HeaderText="MFI" >
                <ItemStyle Width="20%" />
                </asp:BoundField>
                
                  <asp:BoundField DataField="dni" HeaderText="Bead" >
                <ItemStyle Width="20%" />
                </asp:BoundField>
                  <asp:BoundField DataField="transfusion" HeaderText="Sero" >
                <ItemStyle Width="20%" />
                </asp:BoundField>
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>

                                   

        <uc1:tablaEspecificidad ID="tablaEspecificidad1" runat="server" />
  

 
     
                                   
         </div></td>
                                <td>    &nbsp;</td>
                            </tr>
                        </table>
                		
                        </div>
      


    

   

        </div>

        
    </div>
</asp:Content>