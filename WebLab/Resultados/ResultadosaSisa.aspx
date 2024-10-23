<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultadosaSisa.aspx.cs" Inherits="WebLab.Resultados.ResultadosaSisa" MasterPageFile="~/Site1.Master" %>



<asp:Content ID="content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" src="../script/Mascara.js"></script>
    <script type="text/javascript" src="../script/ValidaFecha.js"></script>      
    <%--<script src="../script/jquery.min.js" type="text/javascript"></script>--%>
 <link href="../script/Resources/jquery-ui-1.8.20.css" rel="stylesheet" type="text/css" />   
 
                  <script src="jquery.min.js" type="text/javascript"></script>  
                  <script src="jquery-ui.min.js" type="text/javascript"></script> 

                    

  
    </asp:Content>





<asp:Content ID="content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"> 
    <br />&nbsp;
   <div  style="width: 1150px" class="form-inline" >
           <div class="panel panel-primary" runat="server" > 
                    <div class="panel-heading"> <h3 class="panel-title">RESULTADOS COVID A SISA</h3>

                        </div>
                   <div class="panel-body">
     <table width="100%"
         >
     <tr>

         <td colspan="2">


            <div>
                <p><strong>Seleccione el tipo de archivo</strong>  <asp:DropDownList ID="ddlOrigen" runat="server" class="form-control input-sm" >
                    <asp:ListItem Selected="True" Value="1">Desde Derivaciones sin Responder</asp:ListItem>
                    <asp:ListItem Value="2">Desde Eventos de SIL</asp:ListItem>
                </asp:DropDownList></p>
          

                <p><strong>Derivaciones sin Responder: cruza el numero de documento y fecha de toma de muestra para recuperar el resultado del SIL</strong></p>
                <p><strong>Desde Eventos de SIL: cruza el idEventoCaso generado en sisa desde el SIL para recuperar el resultados</strong></p>
                <br />
                Archivo:
               <asp:FileUpload ID="trepador" runat="server" class="form-control input-sm" Width="400px" />
                <asp:Button  CssClass="btn btn-primary" ID="subir" runat="server" Width="150px" 
                    Text="Procesar Archivo" OnClick="subir_Click" />

            </div>
         
            <div>
                
               <asp:Label ID="estatus" runat="server" 
                    Style="color:red"></asp:Label>

                <asp:Button ID="btnDescargarExcelControl" runat="server" Text="Descargar Excel Control" OnClick="btnDescargarExcelControl_Click"  CssClass="btn btn-success" Width="150px" Visible="False" />
            </div>
            <hr />
                </td></tr>
                <tr>
              

                <td align="left">  
                  <div class="myLabelIzquierda" > Seleccionar: <asp:LinkButton 
                            ID="lnkMarcar" runat="server" CssClass="myLink" onclick="lnkMarcar_Click" 
                                                   ValidationGroup="0">Todas</asp:LinkButton>&nbsp;
                                            <asp:LinkButton 
                            ID="lnkDesmarcar" runat="server" CssClass="myLink" onclick="lnkDesmarcar_Click" 
                                                   ValidationGroup="0">Ninguna</asp:LinkButton> <asp:Label ID="lblCantidadRegistros" runat="server" Style="color: #0000FF"></asp:Label>
            
                   

                    </div> 
        </td>
                <td align="right">   
                  
               

     <asp:Button ID="btnSISA" runat="server" onclick="btnGuardar_Click"   CssClass="btn btn-danger" Width="150px"
                Text="Informar a SISA" />

   

        </td>
                </tr> </table>
                        </div>
                    <div class="panel-footer">
                		<div style="border: 1px solid #999999; height: 450px; width:1050px; overflow: scroll; background-color: #EFEFEF;"> 
         <asp:GridView ID="gvLista" runat="server" AutoGenerateColumns="False"
                DataKeyNames="idDetalleProtocolo" CssClass="table table-bordered bs-table"  Font-Names="Arial" Font-Size="10pt"
                EmptyDataText="No se encontraron resultados para incorporar" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Sel." >
                    <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <ItemStyle Width="5%" 
                    HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:BoundField DataField="idEvento"   HeaderText="idEvento" >
               
                </asp:BoundField>
                <asp:BoundField DataField="dni" HeaderText="dni" >
            
                </asp:BoundField>
                
                
                  <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="idDerivacion" HeaderText="idDerivacion" />
                <asp:BoundField DataField="idEventoMuestra" HeaderText="idEventoMuestra" />
                
                        
         
                        
                <asp:BoundField DataField="resultadoCar" HeaderText="Resultado SIL" />
                
                        
         
                        
                <asp:BoundField DataField="fechatoma" HeaderText="Fecha Toma" />
                
                        
         
                        
            </Columns>
                <HeaderStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
             <EmptyDataRowStyle Font-Bold="True" ForeColor="#FF3300" />

         </asp:GridView>

         </div>
                        </div>
      


        </div>

        
    </div>
</asp:Content>